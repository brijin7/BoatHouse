using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class AdminUser : System.Web.UI.Page
{
    public string GetAntiForgeryToken()
    {
        string cookieToken, formToken;
        AntiForgery.GetTokens(null, out cookieToken, out formToken);
        ViewState["__AntiForgeryCookie"] = cookieToken;
        return formToken;

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.HttpMethod != "GET")
                {
                    AntiForgery.Validate(ViewState["__AntiForgeryCookie"] as string, Request.Form["__RequestVerificationToken"]);
                }

                //CHANGES
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim().ToUpper() == "SADMIN")
                {
                    GetBranch();
                    getEmpRole();
                    getEmpShift();
                    getEmployeeType();
                    BindEmployee();
                    btnSubmit.Text = "Submit";

                }
                else
                {
                    lbtnNew.Visible = false;
                }
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void ImageUploadAPI(string QueryType, string FormName, string PrevImageLink)
    {
        string destinationPath = string.Empty;
        try
        {
            var ImageUpload = new ImageUpload()
            {
                QueryType = QueryType.Trim(),
                FormName = FormName.Trim(),
                PrevImageLink = PrevImageLink
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                string strMappath = "~/ImgUpload/";
                string dirMapPath = Server.MapPath(strMappath);

                if (!Directory.Exists(dirMapPath))
                {
                    Directory.CreateDirectory(dirMapPath);
                }

                string tString = System.DateTime.Now.ToString("yyyyMMddHHmmssss");
                Random generator = new Random();
                String rString = generator.Next(0, 999999).ToString("D6");
                string NewFileName = tString.Trim() + "" + rString.Trim();

                string file = string.Empty;

                if (btnSubmit.Text == "Submit")
                {
                    file = fupEmpLink.FileName;
                }
                else
                {
                    if (hfPrevImageLink.Value != "")
                    {
                        file = Path.GetFileName(hfPrevImageLink.Value.Trim());
                    }
                    else
                    {
                        file = fupEmpLink.FileName;
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                destinationPath = dirMapPath + ImageUpload.ImageLink + NewFileName + Path.GetExtension(file);
                fupEmpLink.SaveAs(destinationPath);
                ImageUpload.ImageLink = destinationPath;

                MultipartFormDataContent content = new MultipartFormDataContent();

                var values = new[]
                {
                    new KeyValuePair<string, string>("QueryType", ImageUpload.QueryType),
                    new KeyValuePair<string, string>("FormName", ImageUpload.FormName),
                    new KeyValuePair<string, string>("PrevImageLink", ImageUpload.PrevImageLink)
                };

                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value),
                        String.Format("\"{0}\"", keyValuePair.Key));
                }

                var fileContent1 = new ByteArrayContent(System.IO.File.ReadAllBytes(ImageUpload.ImageLink));

                fileContent1.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "ImageLink",
                        FileName = ImageUpload.ImageLink,
                    };
                content.Add(fileContent1);

                HttpResponseMessage response = client.PostAsync("ImageAPI", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Postresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Postresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Postresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        hfResponse.Value = ResponseMsg.Trim();

                        if (File.Exists(destinationPath))
                        {
                            File.Delete(destinationPath);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
        finally
        {
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }
        }
    }

    protected void dddlBranchCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        getDesignation();
    }

    //Submit The Values
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string FilePath = string.Empty;
            string QueryType = string.Empty;
            string EmpId = string.Empty, UserId = string.Empty;
            string sPwd = string.Empty;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               

                if (btnSubmit.Text == "Submit")
                {
                    QueryType = "Insert";
                    ImageUploadAPI(QueryType, "EM", "");

                    UserId = "0";
                    EmpId = "0";
                    sPwd = txtEmpPassword.Text.Trim();
                }
                else
                {
                    QueryType = "Update";


                    UserId = ViewState["UserId"].ToString().Trim();
                    EmpId = txtEmpId.Text.Trim();
                    sPwd = hfPassword.Value.Trim();

                    if (hfImageCheckValue.Value == "1")
                    {
                        if (hfPrevImageLink.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", "EM", "");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, "EM", hfPrevImageLink.Value.Trim());
                        }
                    }
                    else
                    {
                        hfResponse.Value = hfPrevImageLink.Value.Trim();
                    }
                }

                string sBranch = string.Empty;
                sBranch = ddlBranchCode.SelectedItem.Text.Trim();
                string[] branchlist = sBranch.Split('~');
                string branch = branchlist[1].Trim();
                
                var EmpMstr = new employeeMaster()
                {

                    QueryType = QueryType,
                    UserId = UserId.ToString(),
                    EmpId = EmpId.ToString(),
                    EmpType = ddlEmpType.SelectedValue.Trim(),
                    EmpFirstName = txtEmployeeFirstName.Text.Trim(),
                    EmpLastName = txtEmployeeLastName.Text.Trim(),
                    BranchId = ddlBranchCode.SelectedValue.Trim(),
                    BranchName = branch.ToString(),
                    EmpDesignation = ddlEmpDesignation.SelectedValue.Trim(),
                    EmpMobileNo = txtMobNo.Text.Trim(),
                    Address1 = txtAddr1.Text.Trim(),
                    Address2 = txtAddr2.Text.Trim(),
                    ZipCode = txtZipCode.Text.Trim(),
                    City = txtCity.Text.Trim(),
                    District = txtDistrict.Text.Trim(),
                    State = txtState.Text.Trim(),
                    EmpDOJ = txtDOJ.Text.Trim(),
                    EmpAadharId = txtAadharid.Text.Trim(),
                    EmpMailId = txtemailid.Text.Trim(),
                    EmpPhotoLink = hfResponse.Value.Trim(),
                    ShiftId = ddlshift.SelectedValue.Trim(),
                    RoleId = ddlRole.SelectedValue.Trim(),
                    UserName = txtUserName.Text.Trim(),
                    Password = sPwd.Trim(),
                    MobileAppAccess = rblMobileAccess.SelectedValue.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("EmpMstr", EmpMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            BindEmployee();
                            Clear();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                        }
                        else
                        {
                            BindEmployee();
                            Clear();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
                    }
                }
                else
                {

                    // lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {            
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            
            btnSubmit.Text = "Update";

            txtUserName.Enabled = false;
            txtEmpPassword.Enabled = false;
            ddlBranchCode.Enabled = false;

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            ViewState["UserId"] = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();
            txtEmpId.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpId"].ToString().Trim();
            ddlEmpType.SelectedValue = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpType"].ToString().Trim();
            txtEmployeeFirstName.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpFirstName"].ToString().Trim();
            txtEmployeeLastName.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpLastName"].ToString().Trim();

            GetAllBranch();
            ddlBranchCode.SelectedValue = gvmstrEmployee.DataKeys[gvrow.RowIndex]["BranchId"].ToString().Trim();

            getDesignation();
            ddlEmpDesignation.SelectedValue = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpDesignation"].ToString().Trim();

            ddlshift.SelectedValue = gvmstrEmployee.DataKeys[gvrow.RowIndex]["ShiftId"].ToString().Trim();
            txtMobNo.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMobileNo"].ToString().Trim();
            txtAddr1.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Address1"].ToString().Trim();
            txtAddr2.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Address2"].ToString().Trim();
            txtZipCode.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Zipcode"].ToString().Trim();

            divAddressDet.Visible = true;
            divAddressDet1.Visible = true;
            divCity.Visible = true;
            divState.Visible = true;
            divDistrict.Visible = true;
            divMapView.Visible = true;

            txtCity.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["City"].ToString().Trim();
            txtDistrict.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["District"].ToString().Trim();
            txtState.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["State"].ToString().Trim();
            txtAadharid.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpAadharId"].ToString().Trim();
            txtDOJ.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["DOJ"].ToString().Trim();
            txtemailid.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMailId"].ToString().Trim();
            txtUserName.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim();
            txtEmpPassword.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Password"].ToString().Trim();
            imgEmpPhotoPrev.ImageUrl = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpPhotoLink"].ToString().Trim();
            hfPrevImageLink.Value = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpPhotoLink"].ToString().Trim();
            getEmpRole();
            ddlRole.SelectedValue = gvmstrEmployee.DataKeys[gvrow.RowIndex]["RoleId"].ToString().Trim();

            if (imgEmpPhotoPrev.ImageUrl == "")
            {
                imgEmpPhotoPrev.ImageUrl = "~/images/EmptyImage.png";
            }

            string MobileAppAccess = string.Empty;
            MobileAppAccess = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MobApp"].ToString().Trim();

            if (MobileAppAccess.ToString() == "Yes")
            {
                rblMobileAccess.SelectedValue = "Y";
            }
            else
            {
                rblMobileAccess.SelectedValue = "N";
            }

            //txtEmpPassword.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Password"].ToString().Trim();
            hfPassword.Value = txtEmpPassword.Text.Trim();

            if (txtEmpPassword.TextMode == TextBoxMode.Password)
            {
                txtEmpPassword.Attributes.Add("value", txtEmpPassword.Text);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string EmpId = gvmstrEmployee.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var EmpMstr = new employeeMaster()
                {

                    QueryType = "Delete",
                    UserId= gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim(),
                    EmpId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpId"].ToString().Trim(),
                    EmpType = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpType"].ToString().Trim(),
                    EmpFirstName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpFirstName"].ToString().Trim(),
                    EmpLastName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpLastName"].ToString().Trim(),
                    BranchId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["BranchId"].ToString().Trim(),
                    BranchName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["BranchName"].ToString().Trim(),
                    EmpDesignation = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpDesignation"].ToString().Trim(),
                    EmpMobileNo = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMobileNo"].ToString().Trim(),
                    Address1 = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Address1"].ToString().Trim(),
                    Address2 = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Address2"].ToString().Trim(),
                    ZipCode = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Zipcode"].ToString().Trim(),
                    City = gvmstrEmployee.DataKeys[gvrow.RowIndex]["City"].ToString().Trim(),
                    District = gvmstrEmployee.DataKeys[gvrow.RowIndex]["District"].ToString().Trim(),
                    State = gvmstrEmployee.DataKeys[gvrow.RowIndex]["State"].ToString().Trim(),

                    EmpDOJ = gvmstrEmployee.DataKeys[gvrow.RowIndex]["DOJ"].ToString().Trim(),
                    EmpAadharId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpAadharId"].ToString().Trim(),
                    EmpMailId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMailId"].ToString().Trim(),
                    EmpPhotoLink = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpPhotoLink"].ToString().Trim(),
                    ShiftId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["ShiftId"].ToString().Trim(),
                    RoleId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["RoleId"].ToString().Trim(),
                    UserName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim(),
                    Password = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Password"].ToString().Trim(),
                    MobileAppAccess = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MobApp"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };
                var response = client.PostAsJsonAsync("EmpMstr", EmpMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindEmployee();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                    }
                }
                else
                {
                    //lblGridMsg.Text = response.ToString();
                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    // Cancelling Process
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        BindEmployee();
    }

    // Add New Button
    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
        divAddressDet.Visible = false;
        divAddressDet1.Visible = false;
        divCity.Visible = false;
        divState.Visible = false;
        divDistrict.Visible = false;
        divMapView.Visible = false;
        btnSubmit.Text = "Submit";
        txtEmpPassword.Enabled = true;
        Clear();
    }

    protected void gvmstrEmployee_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // for example, the row contains a certain property
                if (((Label)e.Row.FindControl("lblActiveStatus")).Text == "D")
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnEdit");
                    btnEdit.Visible = false;
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = false;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = true;
                }

                else
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnEdit");
                    btnEdit.Visible = true;
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = true;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string EmpId = gvmstrEmployee.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var EmpMstr = new employeeMaster()
                {

                    QueryType = "ReActive",
                    UserId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim(),
                    EmpId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpId"].ToString().Trim(),
                    EmpType = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpType"].ToString().Trim(),
                    EmpFirstName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpFirstName"].ToString().Trim(),
                    EmpLastName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpLastName"].ToString().Trim(),
                    BranchId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["BranchId"].ToString().Trim(),
                    BranchName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["BranchName"].ToString().Trim(),
                    EmpDesignation = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpDesignation"].ToString().Trim(),
                    EmpMobileNo = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMobileNo"].ToString().Trim(),
                    Address1 = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Address1"].ToString().Trim(),
                    Address2 = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Address2"].ToString().Trim(),
                    ZipCode = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Zipcode"].ToString().Trim(),
                    City = gvmstrEmployee.DataKeys[gvrow.RowIndex]["City"].ToString().Trim(),
                    District = gvmstrEmployee.DataKeys[gvrow.RowIndex]["District"].ToString().Trim(),
                    State = gvmstrEmployee.DataKeys[gvrow.RowIndex]["State"].ToString().Trim(),

                    EmpDOJ = gvmstrEmployee.DataKeys[gvrow.RowIndex]["DOJ"].ToString().Trim(),
                    EmpAadharId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpAadharId"].ToString().Trim(),
                    EmpMailId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMailId"].ToString().Trim(),
                    EmpPhotoLink = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpPhotoLink"].ToString().Trim(),
                    ShiftId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["ShiftId"].ToString().Trim(),
                    RoleId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["RoleId"].ToString().Trim(),
                    UserName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim(),
                    Password = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Password"].ToString().Trim(),
                    MobileAppAccess = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MobApp"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };
                var response = client.PostAsJsonAsync("EmpMstr", EmpMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindEmployee();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    //lblGridMsg.Text = response.ToString();
                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void txtZipCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string sPassword = hfPassword.Value;
            txtEmpPassword.Attributes.Add("value", sPassword);

            using (var client = new HttpClient())
            {
                string ZipURL = "http://www.postalpincode.in/api/pincode/";
                client.BaseAddress = new Uri(ZipURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("http://www.postalpincode.in/api/pincode/" + txtZipCode.Text + "").Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    string Status = JObject.Parse(Locresponse)["Status"].ToString();
                    string Message = JObject.Parse(Locresponse)["Message"].ToString();
                    string PostOffice = JObject.Parse(Locresponse)["PostOffice"].ToString();
                    if (Status == "Success")
                    {

                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(PostOffice);
                        if (dt.Rows.Count > 0)
                        {
                            divAddressDet.Visible = true;
                            divAddressDet1.Visible = true;
                            divCity.Visible = true;
                            divState.Visible = true;
                            divDistrict.Visible = true;
                            divMapView.Visible = true;
                            txtAddr1.Focus();
                            txtCity.Enabled = false;
                            txtDistrict.Enabled = false;
                            txtState.Enabled = false;
                            txtCity.Text = dt.Rows[0]["Circle"].ToString();
                            txtDistrict.Text = dt.Rows[0]["District"].ToString();
                            txtState.Text = dt.Rows[0]["State"].ToString();
                        }
                        else
                        {
                            txtCity.Text = "";
                            txtDistrict.Text = "";
                            txtState.Text = "";

                        }

                    }
                    else
                    {

                        txtCity.Text = "";
                        txtDistrict.Text = "";
                        txtState.Text = "";
                        divAddressDet.Visible = false;
                        divAddressDet1.Visible = false;
                        divCity.Visible = false;
                        divState.Visible = false;
                        divDistrict.Visible = false;
                        divMapView.Visible = false;
                        return;

                    }
                }
                else
                {

                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);

                    txtCity.Text = "";
                    txtDistrict.Text = "";
                    txtState.Text = "";
                    divAddressDet.Visible = false;
                    divAddressDet1.Visible = false;
                    divCity.Visible = false;
                    divState.Visible = false;
                    divDistrict.Visible = false;
                    divMapView.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
        finally
        {
            string sPassword = hfPassword.Value;
            txtEmpPassword.Attributes.Add("value", sPassword);
        }
    }

    public void GetBranch()
    {
        try
        {
            ddlBranchCode.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new employeeMaster()
                {
                    QueryType = "Dropdown",
                    ServiceType = "AdminBranchAll",
                    CorpId = "",
                    BranchCode = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlBranchCode.DataSource = dtExists;
                        ddlBranchCode.DataValueField = "BranchId";
                        ddlBranchCode.DataTextField = "BranchName";
                        ddlBranchCode.DataBind();
                    }
                    else
                    {
                        ddlBranchCode.DataBind();
                    }
                    ddlBranchCode.Items.Insert(0, "Select Branch Category");
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void GetAllBranch()
    {
        try
        {
            ddlBranchCode.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new employeeMaster()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlBranchEditData",
                    CorpId = "",
                    BranchCode = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlBranchCode.DataSource = dtExists;
                        ddlBranchCode.DataValueField = "BranchId";
                        ddlBranchCode.DataTextField = "BranchName";
                        ddlBranchCode.DataBind();
                    }
                    else
                    {
                        ddlBranchCode.DataBind();
                    }
                    ddlBranchCode.Items.Insert(0, "Select Branch Category");
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //DropDowns 
    public void getDesignation()
    {
        try
        {
            ddlEmpDesignation.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new employeeMaster()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlBranchDesignation",
                    CorpId = "",
                    Input1 = ddlBranchCode.SelectedValue.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlEmpDesignation.DataSource = dtExists;
                        ddlEmpDesignation.DataValueField = "Designation";
                        ddlEmpDesignation.DataTextField = "DesignationName";
                        ddlEmpDesignation.DataBind();
                    }
                    else
                    {
                        ddlEmpDesignation.DataBind();
                    }
                    ddlEmpDesignation.Items.Insert(0, "Select Designation");
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void getEmployeeType()
    {
        try
        {
            ddlEmpType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new employeeMaster()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlEmployeeType",
                    CorpId = "",
                    BranchCode = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlEmpType.DataSource = dtExists;
                        ddlEmpType.DataValueField = "ConfigId";
                        ddlEmpType.DataTextField = "ConfigName";
                        ddlEmpType.DataBind();
                    }
                    else
                    {
                        ddlEmpType.DataBind();
                    }
                    ddlEmpType.Items.Insert(0, "Select Employee Type");
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void getEmpShift()
    {
        try
        {
            ddlshift.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new employeeMaster()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlShift",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlshift.DataSource = dtExists;
                        ddlshift.DataValueField = "ShiftID";
                        ddlshift.DataTextField = "ShiftName";
                        ddlshift.DataBind();
                    }
                    else
                    {
                        ddlshift.DataBind();
                    }
                    ddlshift.Items.Insert(0, "Select Shift");
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void getEmpRole()
    {
        try
        {
            ddlRole.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new employeeMaster()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlRole",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlRole.DataSource = dtExists;
                        ddlRole.DataValueField = "ConfigId";
                        ddlRole.DataTextField = "ConfigName";
                        ddlRole.DataBind();
                    }
                    else
                    {
                        ddlRole.DataBind();
                    }
                    ddlRole.Items.Insert(0, "Select Role");
                    ddlRole.SelectedValue = "2";
                    ddlRole.Enabled = false;
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    // Clearing All Fields
    public void Clear()
    {       
        GetBranch();
        ddlBranchCode.Enabled = true;

        ddlEmpType.SelectedIndex = -1;
        txtEmployeeFirstName.Text = string.Empty;
        txtEmployeeLastName.Text = string.Empty;
        ddlEmpDesignation.SelectedIndex = -1;
        ddlBranchCode.SelectedIndex = -1;
        txtMobNo.Text = string.Empty;
        txtAddr1.Text = string.Empty;
        txtAddr2.Text = string.Empty;
        txtZipCode.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        txtState.Text = string.Empty;
        txtDOJ.Text = string.Empty;
        txtAadharid.Text = string.Empty;
        txtemailid.Text = string.Empty;
        ddlshift.SelectedIndex = -1;
        ddlRole.SelectedIndex = -1;
        txtUserName.Text = string.Empty;
        this.txtEmpPassword.Attributes["value"] = "";
        imgEmpPhotoPrev.ImageUrl = "../images/EmptyImage.png";
        hfResponse.Value = string.Empty;
        hfPrevImageLink.Value = string.Empty;
        hfImageCheckValue.Value = "0";
        txtUserName.Enabled = true;

        getEmployeeType();
        getEmpRole();
    }

    //Bind Employee
    public void BindEmployee()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new employeeMaster()
                {
                    QueryType = "GetEmpMaster",
                    ServiceType = "Admin",
                    CorpId = "",
                    Input1 = "2",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvmstrEmployee.DataSource = dtExists;
                        gvmstrEmployee.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                    }
                    else
                    {
                        gvmstrEmployee.DataBind();
                        lblGridMsg.Text = "No Records Found";
                        divGrid.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                    }
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    // Creating Class for Employee Master
    public class employeeMaster
    {
        public string QueryType { get; set; }
        public string UserId { get; set; }
        public string EmpId { get; set; }
        public string EmpType { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EmpDesignation { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string EmpMobileNo { get; set; }
        public string EmpMailId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string EmpDOJ { get; set; }
        public string ShiftId { get; set; }
        public string RoleId { get; set; }
        public string EmpAadharId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string EmpPhotoLink { get; set; }
        public string MobileAppAccess { get; set; }
        public string CreatedBy { get; set; }
        public string DeptId { get; set; }

        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

    }

    public class ImageUpload
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string FormName { get; set; }
        public string PrevImageLink { get; set; }
        public string ImageLink { get; set; }
    }
}