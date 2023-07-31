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

public partial class Sadmin_OfficeUser : System.Web.UI.Page
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
                //Changes
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim().ToUpper() == "SADMIN")
                {
                    BindEmployee();
                    btnSubmit.Text = "Submit";
                }
                else
                {
                    lbtnNew.Visible = false;
                }
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    //Submit The Values
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string FilePath = string.Empty;
            string QueryType = string.Empty;
            int EmpId = 0;
            string UserId = string.Empty;
            string sPwd = string.Empty;

            string mStatus = string.Empty;
            string bStatus = string.Empty;
            string tStatus = string.Empty;
            string hStatus = string.Empty;
            string aStatus = string.Empty;

            mStatus = "N";
            bStatus = "N";
            tStatus = "N";
            hStatus = "N";
            aStatus = "N";

            if (chkModuleMaster.Checked == true)
            {
                mStatus = "Y";
            }

            if (chkModuleBoating.Checked == true)
            {
                bStatus = "Y";
            }

            if (chkModuleTour.Checked == true)
            {
                tStatus = "Y";
            }

            if (chkModuleHotel.Checked == true)
            {
                hStatus = "Y";
            }
            if (chkModuleFixedAssets.Checked == true)
            {
                aStatus = "Y";
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                    EmpId = 0;
                    UserId = "0";
                    sPwd = txtEmpPassword.Text.Trim();
                }
                else
                {
                    QueryType = "Update";
                    EmpId = Convert.ToInt32(txtEmpId.Text.Trim());
                    UserId = hfUserId.Value.Trim();
                    sPwd = hfPassword.Value.Trim();
                }


                var EmpMstr = new employeeMaster()
                {

                    QueryType = QueryType,
                    UserId = UserId.Trim(),
                    EmpId = EmpId.ToString(),
                    EmpFirstName = txtEmployeeFirstName.Text.Trim(),
                    EmpLastName = txtEmployeeLastName.Text.Trim(),
                    EmpMobileNo = txtMobNo.Text.Trim(),
                    EmpMailId = txtemailid.Text.Trim(),
                    RoleId = "1",
                    UserName = txtUserName.Text.Trim(),
                    Password = sPwd.Trim(),
                    UserType = rblRole.SelectedValue.Trim(),

                    MMaster = mStatus.Trim(),
                    MBoating = bStatus.Trim(),
                    MHotel = hStatus.Trim(),
                    MTour = tStatus.Trim(),
                    MFixedAssets = aStatus.Trim(),

                    CreatedBy = Session["UserId"].ToString().Trim()


                };
                HttpResponseMessage response;
                response = client.PostAsJsonAsync("SupportUser", EmpMstr).Result;

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

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            hfUserId.Value = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();
            txtEmpId.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpID"].ToString().Trim();
            txtEmployeeFirstName.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpFirstName"].ToString().Trim();
            txtEmployeeLastName.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpLastName"].ToString().Trim();

            txtMobNo.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMobileNo"].ToString().Trim();
            txtemailid.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMailId"].ToString().Trim();
            txtUserName.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim();

            txtEmpPassword.Text = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Password"].ToString().Trim();
            rblRole.SelectedValue = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserType"].ToString().Trim();
            hfPassword.Value = txtEmpPassword.Text.Trim();

            if (txtEmpPassword.TextMode == TextBoxMode.Password)
            {
                txtEmpPassword.Attributes.Add("value", txtEmpPassword.Text);
            }

            string MMaster = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MMaster"].ToString().Trim();
            string MBoating = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MBoating"].ToString().Trim();
            string MHotel = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MHotel"].ToString().Trim();
            string MTour = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MTour"].ToString().Trim();
            string MFixedAssets = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MFixedAssets"].ToString().Trim();

            // Module Rights

            chkModuleMaster.Checked = false;
            chkModuleBoating.Checked = false;
            chkModuleTour.Checked = false;
            chkModuleHotel.Checked = false;
            chkModuleFixedAssets.Checked = false;

            if (MMaster.Trim() == "Y")
            {
                chkModuleMaster.Checked = true;
            }

            if (MBoating.Trim() == "Y")
            {
                chkModuleBoating.Checked = true;
            }

            if (MTour.Trim() == "Y")
            {
                chkModuleTour.Checked = true;
            }

            if (MHotel.Trim() == "Y")
            {
                chkModuleHotel.Checked = true;
            }

            if (MFixedAssets.Trim() == "Y")
            {
                chkModuleFixedAssets.Checked = true;
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
                    UserId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim(),
                    EmpId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpID"].ToString().Trim(),
                    EmpFirstName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpFirstName"].ToString().Trim(),
                    EmpLastName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpLastName"].ToString().Trim(),
                    EmpMobileNo = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMobileNo"].ToString().Trim(),
                    EmpMailId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMailId"].ToString().Trim(),
                    RoleId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["RoleId"].ToString().Trim(),
                    UserName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim(),
                    Password = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Password"].ToString().Trim(),
                    UserType = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserType"].ToString().Trim(),

                    MMaster = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MMaster"].ToString().Trim(),
                    MBoating = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MBoating"].ToString().Trim(),
                    MHotel = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MHotel"].ToString().Trim(),
                    MTour = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MTour"].ToString().Trim(),
                    MFixedAssets = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MFixedAssets"].ToString().Trim(),

                    CreatedBy = Session["UserId"].ToString().Trim()

                };
                var response = client.PostAsJsonAsync("SupportUser", EmpMstr).Result;

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
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

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
        btnSubmit.Text = "Submit";
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
                    EmpId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpID"].ToString().Trim(),
                    EmpFirstName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpFirstName"].ToString().Trim(),
                    EmpLastName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpLastName"].ToString().Trim(),
                    EmpMobileNo = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMobileNo"].ToString().Trim(),
                    EmpMailId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["EmpMailId"].ToString().Trim(),
                    RoleId = gvmstrEmployee.DataKeys[gvrow.RowIndex]["RoleId"].ToString().Trim(),
                    UserName = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim(),
                    Password = gvmstrEmployee.DataKeys[gvrow.RowIndex]["Password"].ToString().Trim(),
                    UserType = gvmstrEmployee.DataKeys[gvrow.RowIndex]["UserType"].ToString().Trim(),

                    MMaster = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MMaster"].ToString().Trim(),
                    MBoating = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MBoating"].ToString().Trim(),
                    MHotel = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MHotel"].ToString().Trim(),
                    MTour = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MTour"].ToString().Trim(),
                    MFixedAssets = gvmstrEmployee.DataKeys[gvrow.RowIndex]["MFixedAssets"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };
                var response = client.PostAsJsonAsync("SupportUser", EmpMstr).Result;

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

    // Clearing All Fields
    public void Clear()
    {
        txtEmployeeFirstName.Text = string.Empty;
        txtEmployeeLastName.Text = string.Empty;
        txtMobNo.Text = string.Empty;
        txtemailid.Text = string.Empty;
        txtUserName.Text = string.Empty;
        this.txtEmpPassword.Attributes["value"] = "";
        hfResponse.Value = string.Empty;
        hfImageCheckValue.Value = "0";
        txtUserName.Enabled = true;
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
                    ServiceType = "SupportUser",
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
        public string EmpId { get; set; }
        public string UserId { get; set; }
        public string UserType { get; set; }
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

        public string MMaster { get; set; }
        public string MBoating { get; set; }
        public string MTour { get; set; }
        public string MHotel { get; set; }
        public string MFixedAssets { get; set; }

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