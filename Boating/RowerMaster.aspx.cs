using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_RowerMaster : System.Web.UI.Page
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
                int istart;
                int iend;
                AddProcess(0, 10, out istart, out iend);
                hfCreatedBy.Value = Session["UserId"].ToString().Trim();
                hfBoatHouseId.Value = Session["BoatHouseId"].ToString().Trim();
                hfBoatHouseName.Value = Session["BoatHouseName"].ToString().Trim();
                BindRowerMaster();
                getRowerType();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindRowerMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var rowerMaster1 = new rowerMaster()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    CountStart = hfstartvalue.Value
                };

                response = client.PostAsJsonAsync("ShowRowerMstrDetailsGridV2", rowerMaster1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            gvRowerMstr.Visible = true;
                            gvRowerMstr.DataSource = dt;
                            gvRowerMstr.DataBind();
                            lblGridMsg.Text = string.Empty;

                            if (dt.Rows.Count < 10)
                            {
                                Next.Enabled = false;

                            }
                            else
                            {
                                Next.Enabled = true;

                            }

                        }
                        else
                        {
                            gvRowerMstr.DataSource = dt;
                            gvRowerMstr.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        lblGridMsg.Visible = true;
                        divEntry.Visible = false;
                        divGrid.Visible = true;
                        gvRowerMstr.Visible = false;
                        Next.Enabled = false;
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void getRowerType()
    {
        try
        {
            ddlRowerType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new RowerMaster()
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
                        ddlRowerType.DataSource = dtExists;
                        ddlRowerType.DataValueField = "ConfigId";
                        ddlRowerType.DataTextField = "ConfigName";
                        ddlRowerType.DataBind();
                    }
                    else
                    {
                        ddlRowerType.DataBind();
                    }
                    ddlRowerType.Items.Insert(0, "Select Rower Type");
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

    public void Clear()
    {
        txtRower.Text = string.Empty;
        txtMobNo.Text = string.Empty;
        txtemailid.Text = string.Empty;
        txtAadharid.Text = string.Empty;
        ddlRowerType.SelectedIndex = -1;
        ddlDriverType.SelectedIndex = -1;

        txtAddr1.Text = string.Empty;
        txtAddr2.Text = string.Empty;
        txtZipCode.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        txtState.Text = string.Empty;
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        imgEmpPhotoPrev.ImageUrl = "../images/EmptyImage.png";
        hfResponse.Value = string.Empty;
        hfPrevImageLink.Value = string.Empty;
        hfImageCheckValue.Value = "0";
        divAddrDet.Visible = false;
        divMapView.Visible = false;

    }

    protected void txtZipCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtZipCode.Text == "" || txtZipCode.Text == null)
            {
                divAddrDet.Visible = false;
                //mapview.Visible = false;
                divMapView.Visible = false;

                return;

            }
            using (var client = new HttpClient())
            {
                string ZipURL = "http://www.postalpincode.in/api/pincode/";
                client.BaseAddress = new Uri(ZipURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("http://www.postalpincode.in/api/pincode/" + txtZipCode.Text + "").Result;
                string mesg = "Invalid Zip Code";

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
                            divAddrDet.Visible = true;
                            divMapView.Visible = true;
                            txtAddr1.Focus();
                            txtCity.Enabled = false;
                            txtDistrict.Enabled = false;
                            txtState.Enabled = false;
                            txtCity.Text = dt.Rows[0]["Circle"].ToString();
                            txtDistrict.Text = dt.Rows[0]["District"].ToString();
                            txtState.Text = dt.Rows[0]["State"].ToString();
                            txtAddr1.Text = "";
                            txtAddr2.Text = "";
                            //  mapview.Visible = true;
                            divMapView.Visible = true;
                        }
                        else
                        {
                            txtCity.Text = "";
                            txtDistrict.Text = "";
                            txtState.Text = "";
                            divAddrDet.Visible = false;
                            //  mapview.Visible = false;
                            divMapView.Visible = false;
                        }
                    }
                    else
                    {
                        txtCity.Text = "";
                        txtDistrict.Text = "";
                        txtState.Text = "";
                        divAddrDet.Visible = false;
                        //  mapview.Visible = false;
                        divMapView.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + mesg.ToString().Trim() + "');", true);

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
                    divAddrDet.Visible = false;
                    divMapView.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    static bool IsValidImage(string filePath)
    {
        return IsValidImage(new FileStream(filePath, FileMode.Open, FileAccess.Read));
    }

    static bool IsValidImage(Stream imageStream)
    {
        if (imageStream.Length > 0)
        {
            byte[] header = new byte[6]; // Change size if needed. 
            string[] imageHeaders = new[]{
                "\xFF\xD8", // JPEG 
                "BM",       // BMP 
                "GIF",      // GIF 
             Encoding.ASCII.GetString(new byte[]{137, 80, 78, 71}), // PNG 
             Encoding.ASCII.GetString(new byte[] { 255, 216, 255, 224}), // jpeg 
             Encoding.ASCII.GetString(new byte[] { 255, 216, 255, 225 }), // jpeg cannon 
             Encoding.ASCII.GetString(new byte[] { 77, 77, 42 }) }; // tiff 

            imageStream.Read(header, 0, header.Length);

            bool isImageHeader = imageHeaders.Count(str => Encoding.ASCII.GetString(header).StartsWith(str)) > 0;
            if (isImageHeader == true)
            {
                try
                {
                    //Image.FromStream(imageStream).Dispose();
                    imageStream.Close();
                    return true;
                }

                catch
                {

                }
            }
        }

        imageStream.Close();
        return false;
    }
    public void ImageUploadAPI(string QueryType, string BoatHouseId, string FormName, string PrevImageLink)
    {
        string destinationPath = string.Empty;
        try
        {
            var ImageUpload = new ImageUpload()
            {
                QueryType = QueryType.Trim(),
                BoatHouseId = BoatHouseId.Trim(),
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
                bool dt = IsValidImage(ImageUpload.ImageLink);
                if (dt == true)
                {
                    MultipartFormDataContent content = new MultipartFormDataContent();

                    var values = new[]
                    {
                    new KeyValuePair<string, string>("QueryType", ImageUpload.QueryType),
                    new KeyValuePair<string, string>("BoatHouseId", ImageUpload.BoatHouseId),
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
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Your File Is Corrupted,Please Upload another File');", true);
                    return;
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                string QueryType = string.Empty;
                int RowerId = 0;

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                    ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "RM", "");
                    RowerId = 0;
                }
                else
                {
                    QueryType = "Update";
                    RowerId = Convert.ToInt32(txtRowerId.Text.Trim());

                    if (hfImageCheckValue.Value == "1")
                    {
                        if (hfPrevImageLink.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", Session["BoatHouseId"].ToString().Trim(), "RM", "");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "RM", hfPrevImageLink.Value.Trim());
                        }
                    }
                    else
                    {
                        hfResponse.Value = hfPrevImageLink.Value.Trim();
                    }
                }


                if (txtAddr1.Text == "")
                {
                    txtAddr1.Text = hfAddres1.Value.Trim();

                }
                if (txtAddr2.Text == "")
                {

                    txtAddr2.Text = hfAddres2.Value.Trim();
                }

                var rowerMaster1 = new rowerMaster()
                {
                    QueryType = QueryType.Trim(),
                    RowerId = RowerId.ToString().Trim(),
                    RowerName = txtRower.Text,
                    MobileNo = txtMobNo.Text,
                    MailId = txtemailid.Text.Trim(),
                    DriverCategory = ddlDriverType.SelectedItem.Text.Trim(),
                    AadharId = txtAadharid.Text,
                    RowerType = ddlRowerType.SelectedValue.Trim(),

                    Address1 = txtAddr1.Text,
                    Address2 = txtAddr2.Text,
                    City = txtCity.Text,
                    District = txtDistrict.Text,
                    State = txtState.Text,
                    Zipcode = txtZipCode.Text,
                    PhotoLink = hfResponse.Value.Trim(),
                    BoatHouseId = hfBoatHouseId.Value,
                    BoatHouseName = hfBoatHouseName.Value,
                    CreatedBy = hfCreatedBy.Value,
                };

                response = client.PostAsJsonAsync("RowerMaster", rowerMaster1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindRowerMaster();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnEdit_Click1(object sender, ImageClickEventArgs e)
    {
        try
        {

            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            // mapview.Visible = true;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvRowerMstr.DataKeys[gvrow.RowIndex].Value.ToString();
            Label lblRowerId = (Label)gvrow.FindControl("lblRowerId");
            Label lblRowerName = (Label)gvrow.FindControl("lblRowerName");
            Label lblMobileNo = (Label)gvrow.FindControl("lblMobileNo");
            Label lblMailId = (Label)gvrow.FindControl("lblMailId");
            Label lblAadharId = (Label)gvrow.FindControl("lblAadharId");

            Label lblAddress1 = (Label)gvrow.FindControl("lblAddress1");
            Label lblAddress2 = (Label)gvrow.FindControl("lblAddress2");
            Label lblZipCode = (Label)gvrow.FindControl("lblZipCode");
            Label lblCity = (Label)gvrow.FindControl("lblCity");
            Label lblDistrict = (Label)gvrow.FindControl("lblDistrict");
            Label lblState = (Label)gvrow.FindControl("lblState");
            Label lblPhotoLink = (Label)gvrow.FindControl("lblPhotoLink");

            txtRowerId.Text = lblRowerId.Text;
            txtRower.Text = lblRowerName.Text;
            txtMobNo.Text = lblMobileNo.Text;
            txtemailid.Text = lblMailId.Text;
            txtAadharid.Text = lblAadharId.Text;
            ddlRowerType.SelectedValue = gvRowerMstr.DataKeys[gvrow.RowIndex]["RowerType"].ToString().Trim();

            if (gvRowerMstr.DataKeys[gvrow.RowIndex]["DriverCategory"].ToString().Trim() == "Rower")
            { ddlDriverType.SelectedValue = "1"; }
            else if (gvRowerMstr.DataKeys[gvrow.RowIndex]["DriverCategory"].ToString().Trim() == "Driver")
            {
                ddlDriverType.SelectedValue = "2";
            }
            else
            {
                ddlDriverType.SelectedValue = "0";
            }

            txtAddr1.Text = lblAddress1.Text;

            txtAddr2.Text = lblAddress2.Text;
            txtZipCode.Text = lblZipCode.Text;
            txtState.Text = lblState.Text;
            txtCity.Text = lblCity.Text;
            txtDistrict.Text = lblDistrict.Text;
            txtCity.Enabled = false;
            txtDistrict.Enabled = false;
            txtState.Enabled = false;

            imgEmpPhotoPrev.ImageUrl = lblPhotoLink.Text.Trim();
            hfPrevImageLink.Value = lblPhotoLink.Text.Trim();

            if (imgEmpPhotoPrev.ImageUrl == "")
            {
                imgEmpPhotoPrev.ImageUrl = "../images/EmptyImage.png";
            }

            divAddrDet.Visible = true;
            divMapView.Visible = true;



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
            string sTesfg = gvRowerMstr.DataKeys[gvrow.RowIndex].Value.ToString();
            Label lblRowerId = (Label)gvrow.FindControl("lblRowerId");
            Label lblRowerName = (Label)gvrow.FindControl("lblRowerName");
            Label lblMobileNo = (Label)gvrow.FindControl("lblMobileNo");
            Label lblMailId = (Label)gvrow.FindControl("lblMailId");
            Label lblAadharId = (Label)gvrow.FindControl("lblAadharId");
            Label lblAddress1 = (Label)gvrow.FindControl("lblAddress1");
            Label lblAddress2 = (Label)gvrow.FindControl("lblAddress2");
            Label lblZipCode = (Label)gvrow.FindControl("lblZipCode");
            Label lblCity = (Label)gvrow.FindControl("lblCity");
            Label lblDistrict = (Label)gvrow.FindControl("lblDistrict");
            Label lblState = (Label)gvrow.FindControl("lblState");
            Label lblPhotoLink = (Label)gvrow.FindControl("lblPhotoLink");

            txtRowerId.Text = lblRowerId.Text;
            txtRower.Text = lblRowerName.Text;
            txtMobNo.Text = lblMobileNo.Text;
            txtemailid.Text = lblMailId.Text;
            txtAadharid.Text = lblAadharId.Text;
            ddlRowerType.SelectedValue = gvRowerMstr.DataKeys[gvrow.RowIndex]["RowerType"].ToString().Trim();

            if (gvRowerMstr.DataKeys[gvrow.RowIndex]["DriverCategory"].ToString().Trim() == "Rower")
            { ddlDriverType.SelectedValue = "1"; }
            else if (gvRowerMstr.DataKeys[gvrow.RowIndex]["DriverCategory"].ToString().Trim() == "Driver")
            {
                ddlDriverType.SelectedValue = "2";
            }
            else
            {
                ddlDriverType.SelectedValue = "0";
            }

            txtAddr1.Text = lblAddress1.Text;
            txtAddr2.Text = lblAddress2.Text;
            txtZipCode.Text = lblZipCode.Text;
            txtState.Text = lblState.Text;
            txtCity.Text = lblCity.Text;
            txtDistrict.Text = lblDistrict.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                var rowerMaster1 = new rowerMaster()
                {
                    QueryType = "Delete",
                    RowerId = txtRowerId.Text,
                    RowerName = txtRower.Text,
                    MobileNo = txtMobNo.Text,
                    MailId = txtemailid.Text,
                    AadharId = txtAadharid.Text,
                    RowerType = ddlRowerType.SelectedValue,
                    DriverCategory = ddlDriverType.SelectedValue,
                    Address1 = txtAddr1.Text,
                    Address2 = txtAddr2.Text,
                    City = txtCity.Text,
                    District = txtDistrict.Text,
                    State = txtState.Text,
                    Zipcode = txtZipCode.Text,
                    PhotoLink = "Image",
                    BoatHouseId = hfBoatHouseId.Value,
                    BoatHouseName = hfBoatHouseName.Value,
                    CreatedBy = hfCreatedBy.Value,
                };

                response = client.PostAsJsonAsync("RowerMaster", rowerMaster1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindRowerMaster();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        //  mapview.Visible = false;
    }

    protected void lbtnGenRowerCard_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Reports/RptRowerIDCard.aspx");
    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvRowerMstr.DataKeys[gvrow.RowIndex].Value.ToString();
            Label lblRowerId = (Label)gvrow.FindControl("lblRowerId");
            Label lblRowerName = (Label)gvrow.FindControl("lblRowerName");
            Label lblMobileNo = (Label)gvrow.FindControl("lblMobileNo");
            Label lblMailId = (Label)gvrow.FindControl("lblMailId");
            Label lblAadharId = (Label)gvrow.FindControl("lblAadharId");
            Label lblAddress1 = (Label)gvrow.FindControl("lblAddress1");
            Label lblAddress2 = (Label)gvrow.FindControl("lblAddress2");
            Label lblZipCode = (Label)gvrow.FindControl("lblZipCode");
            Label lblCity = (Label)gvrow.FindControl("lblCity");
            Label lblDistrict = (Label)gvrow.FindControl("lblDistrict");
            Label lblState = (Label)gvrow.FindControl("lblState");
            Label lblPhotoLink = (Label)gvrow.FindControl("lblPhotoLink");

            txtRowerId.Text = lblRowerId.Text;
            txtRower.Text = lblRowerName.Text;
            txtMobNo.Text = lblMobileNo.Text;
            txtemailid.Text = lblMailId.Text;
            txtAadharid.Text = lblAadharId.Text;
            ddlRowerType.SelectedValue = gvRowerMstr.DataKeys[gvrow.RowIndex]["RowerType"].ToString().Trim();
            if (gvRowerMstr.DataKeys[gvrow.RowIndex]["DriverCategory"].ToString().Trim() == "Rower")
            {
                ddlDriverType.SelectedValue = "1";
            }
            else if (gvRowerMstr.DataKeys[gvrow.RowIndex]["DriverCategory"].ToString().Trim() == "Driver")
            {
                ddlDriverType.SelectedValue = "2";
            }
            else
            {
                ddlDriverType.SelectedValue = "0";
            }

            txtAddr1.Text = lblAddress1.Text;
            txtAddr2.Text = lblAddress2.Text;
            txtZipCode.Text = lblZipCode.Text;
            txtState.Text = lblState.Text;
            txtCity.Text = lblCity.Text;
            txtDistrict.Text = lblDistrict.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                var rowerMaster1 = new rowerMaster()
                {
                    QueryType = "ReActive",
                    RowerId = txtRowerId.Text,
                    RowerName = txtRower.Text,
                    MobileNo = txtMobNo.Text,
                    MailId = txtemailid.Text,
                    AadharId = txtAadharid.Text,
                    RowerType = ddlRowerType.SelectedValue,
                    DriverCategory = ddlDriverType.SelectedValue,
                    Address1 = txtAddr1.Text,
                    Address2 = txtAddr2.Text,
                    City = txtCity.Text,
                    District = txtDistrict.Text,
                    State = txtState.Text,
                    Zipcode = txtZipCode.Text,
                    PhotoLink = "Image",
                    BoatHouseId = hfBoatHouseId.Value,
                    BoatHouseName = hfBoatHouseName.Value,
                    CreatedBy = hfCreatedBy.Value,
                };

                response = client.PostAsJsonAsync("RowerMaster", rowerMaster1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindRowerMaster();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvRowerMstr_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

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

    public class rowerMaster
    {
        public string QueryType { get; set; }
        public string RowerId { get; set; }
        public string RowerName { get; set; }
        public string MobileNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string AadharId { get; set; }
        public string RowerType { get; set; }
        public string MailId { get; set; }
        public string PhotoLink { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string CreatedBy { get; set; }
        public string DriverCategory { get; set; }
        public string CountStart { get; set; }
        public string Search { get; set; }
    }



    public class RowerMaster
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
    protected void AddProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = start + 10;

        }
        else
        {
            iend = end;

        }
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
    }
    protected void AddProcessSearch(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = start + 10;

        }
        else
        {
            iend = end;

        }
        hfSearchValue.Value = istart.ToString();
        hfSearchEndValue.Value = iend.ToString();
    }
    protected void subProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
            back.Enabled = false;
        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            back.Enabled = false;
        }
        else
        {
            iend = end;

        }
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
    }

    protected void subProcessSearch(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
            searchback.Enabled = false;
        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            searchback.Enabled = false;
        }
        else
        {
            iend = end;

        }
        hfSearchValue.Value = istart.ToString();
        hfSearchEndValue.Value = iend.ToString();
    }

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(hfstartvalue.Value) - 10, Int32.Parse(hfendvalue.Value) - 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        BindRowerMaster();
    }

    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;

        AddProcess(Int32.Parse(hfendvalue.Value) + 1, Int32.Parse(hfendvalue.Value) + 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        BindRowerMaster();
    }

    protected void BackToList_Click(object sender, EventArgs e)
    {
        back.Visible = true;
        Next.Visible = true;
        searchback.Visible = false;
        searchNext.Visible = false;
        BackToList.Visible = false;
        txtSearch.Text = string.Empty;
        BindRowerMaster();
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        BackToList.Visible = true;
        back.Visible = false;
        Next.Visible = false;
        int istart;
        int iend;
        AddProcessSearch(0, 10, out istart, out iend);
        BindRowerMasterSingle();


        /*******************/
        searchback.Enabled = false;
    }

    public void BindRowerMasterSingle()
    {
        try
        {
            searchback.Visible = true;
            searchNext.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var rowerMaster1 = new rowerMaster()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    CountStart = hfSearchValue.Value,
                    Search = txtSearch.Text
                };

                response = client.PostAsJsonAsync("ShowRowerMstrDetailsGridSingleV2", rowerMaster1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            gvRowerMstr.DataSource = dt;
                            gvRowerMstr.DataBind();
                            lblGridMsg.Text = string.Empty;

                            if (dt.Rows.Count < 10)
                            {
                                searchNext.Enabled = false;
                            }
                            else
                            {
                                searchNext.Enabled = true;
                            }
                            searchback.Enabled = true;



                        }
                        else
                        {
                            gvRowerMstr.DataSource = dt;
                            gvRowerMstr.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Record Found');", true);
                        BindRowerMaster();
                        BackToList.Visible = false;
                        back.Visible = true;
                        Next.Visible = true;
                        searchNext.Visible = false;
                        searchback.Visible = false;
                        txtSearch.Text = "";
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void searchback_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcessSearch(Int32.Parse(hfSearchValue.Value) - 10, Int32.Parse(hfSearchEndValue.Value) - 10, out istart, out iend);
        BindRowerMasterSingle();

        if (istart == 1)
        {
            searchback.Enabled = false;

        }
        else
        {
            searchback.Enabled = true;
        }
    }

    protected void searchNext_Click(object sender, EventArgs e)
    {
        searchback.Enabled = true;
        int istart;
        int iend;
        AddProcessSearch(Int32.Parse(hfSearchEndValue.Value) + 1, Int32.Parse(hfSearchEndValue.Value) + 10, out istart, out iend);
        BindRowerMasterSingle();
    }

}
