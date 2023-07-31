using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_CorporateOffice : System.Web.UI.Page
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
                hfCreatedBy.Value = Session["UserId"].ToString().Trim();
                GetCountry();
                BindCompanyMaster();
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    
    public void ImageUploadAPI(string QueryType, string FormName, string PrevImageLink, string sFileType)
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
                    if (sFileType.Trim() == "CorLogo")
                    {
                        file = fupCorLogo.FileName;
                    }

                    if (sFileType.Trim() == "CorLogo1")
                    {
                        file = fupCorLogo1.FileName;
                    }

                    if (sFileType.Trim() == "CorPhoto")
                    {
                        file = fupCorPhoto.FileName;
                    }

                    if (sFileType.Trim() == "CorPhoto1")
                    {
                        file = fupCorPhoto1.FileName;
                    }
                }
                else
                {
                    if (sFileType.Trim() == "CorLogo")
                    {
                        if (hfCorLoPreImgLk.Value != "")
                        {
                            file = Path.GetFileName(hfCorLoPreImgLk.Value.Trim());
                        }
                        else
                        {
                            file = Path.GetFileName(imgCorLogoPrev.ImageUrl);
                        }
                    }

                    if (sFileType.Trim() == "CorLogo1")
                    {
                        if (hfCorLoPreImgLk1.Value != "")
                        {
                            file = Path.GetFileName(hfCorLoPreImgLk1.Value.Trim());
                        }
                        else
                        {
                            file = Path.GetFileName(imgCorLogo1Prev.ImageUrl);
                        }
                    }

                    if (sFileType.Trim() == "CorPhoto")
                    {
                        if (hfCorPhPreImgLk1.Value != "")
                        {
                            file = Path.GetFileName(hfCorPhPreImgLk.Value.Trim());
                        }
                        else
                        {
                            file = Path.GetFileName(imgCorPhoto1Prev.ImageUrl);
                        }
                    }

                    if (sFileType.Trim() == "CorPhoto1")
                    {
                        if(hfCorPhPreImgLk1.Value != "")
                        {
                            file = Path.GetFileName(hfCorPhPreImgLk1.Value.Trim());
                        }
                        else
                        {
                            file = Path.GetFileName(imgCorPhoto1Prev.ImageUrl);
                        }
                    }

                }

                string fileName = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                destinationPath = dirMapPath + ImageUpload.ImageLink + NewFileName + Path.GetExtension(file);

                if (sFileType.Trim() == "CorLogo")
                {
                    fupCorLogo.SaveAs(destinationPath);
                }

                if (sFileType.Trim() == "CorLogo1")
                {
                    fupCorLogo1.SaveAs(destinationPath);
                }

                if (sFileType.Trim() == "CorPhoto")
                {
                    fupCorPhoto.SaveAs(destinationPath);
                }

                if (sFileType.Trim() == "CorPhoto1")
                {
                    fupCorPhoto1.SaveAs(destinationPath);
                }

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

                HttpResponseMessage response = client.PostAsync("CM_ImageAPI", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Postresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Postresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Postresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {

                        if (sFileType.Trim() == "CorLogo")
                        {
                            hfCorLoRes.Value = ResponseMsg.Trim();
                        }

                        if (sFileType.Trim() == "CorLogo1")
                        {
                            hfCorLoRes1.Value = ResponseMsg.Trim();
                        }

                        if (sFileType.Trim() == "CorPhoto")
                        {
                            hfCorPhRes.Value = ResponseMsg.Trim();
                        }

                        if (sFileType.Trim() == "CorPhoto1")
                        {
                            hfCorPhRes1.Value = ResponseMsg.Trim();
                        }

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
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
                string sMSG = string.Empty;
                string QueryType = string.Empty;
                int CorpID = 0;

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";

                    ImageUploadAPI(QueryType, "COL", "", "CorLogo");
                    ImageUploadAPI(QueryType, "COL1", "", "CorLogo1");
                    ImageUploadAPI(QueryType, "COP", "", "CorPhoto");
                    ImageUploadAPI(QueryType, "COP1", "", "CorPhoto1");
                    CorpID = 0;
                }
                else
                {
                    QueryType = "Update";
                    CorpID = Convert.ToInt32(txtCropId.Text.Trim());

                    if (hfCorLogoChkVal.Value == "1")
                    {
                        ImageUploadAPI(QueryType, "COL", hfCorLoPreImgLk.Value.Trim(), "CorLogo");
                    }
                    else
                    {
                        hfCorLoRes.Value = hfCorLoPreImgLk.Value.Trim();
                    }

                    if (hfCorLogo1ChkVal.Value == "1")
                    {
                        if(hfCorLoPreImgLk1.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", "COL1", hfCorLoPreImgLk1.Value.Trim(), "CorLogo1");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, "COL1", hfCorLoPreImgLk1.Value.Trim(), "CorLogo1");
                        }
                    }
                    else
                    {
                        if(hfCorLoMinus.Value == "1")
                        {
                            hfCorLoRes1.Value = "";
                        }
                        else
                        {
                            hfCorLoRes1.Value = hfCorLoPreImgLk1.Value.Trim();
                        }
                    }

                    if (hfCorPhotoChkVal.Value == "1")
                    {
                        ImageUploadAPI(QueryType, "COP", hfCorPhPreImgLk.Value.Trim(), "CorPhoto");
                    }
                    else
                    {
                        hfCorPhRes.Value = hfCorPhPreImgLk.Value.Trim();
                    }

                    if (hfCorPhoto1ChkVal.Value == "1")
                    {
                        if (hfCorPhPreImgLk1.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", "COP1", hfCorPhPreImgLk1.Value.Trim(), "CorPhoto1");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, "COP1", hfCorPhPreImgLk1.Value.Trim(), "CorPhoto1");
                        }
                    }
                    else
                    {
                        if (hfCorPhMinus.Value == "1")
                        {
                            hfCorPhRes1.Value = "";
                        }
                        else
                        {
                            hfCorPhRes1.Value = hfCorPhPreImgLk1.Value.Trim();
                        }
                    }
                }

                if (txtaddress2.Text == "")
                {
                    txtaddress2.Text = hfAddres2.Value.Trim();
                }
                if (txtAddress1.Text == "")
                {
                    txtAddress1.Text = hfAddres1.Value.Trim();
                }

                var companymaster = new companymaster()
                {
                    QueryType = QueryType,
                    CorpId = Convert.ToString(CorpID),
                    CorpName = txtCropName.Text.Trim(),
                    ShortName = txtshortName.Text,
                    Address1 = txtAddress1.Text,
                    Address2 = txtaddress2.Text,
                    Zipcode = txtZipcode.Text,
                    City = txtCity.Text,
                    District = txtDistrict.Text,
                    State = txtState.Text,
                    Country = ddlCountry.SelectedValue.Trim(),
                    Phone1 = txtphone1.Text.Trim(),
                    Phone2 = txtphone2.Text.Trim(),
                    Fax = txtFax.Text.Trim(),
                    MailId = txtEmailId.Text.Trim(),
                    CorpLogo = hfCorLoRes.Value.Trim(),
                    CorpLogo1 = hfCorLoRes1.Value.Trim(),
                    AppName = txtappname.Text.Trim(),
                    CorpPhoto = hfCorPhRes.Value.Trim(),
                    CorpPhoto1 = hfCorPhRes1.Value.Trim()
                };

                response = client.PostAsJsonAsync("CM_CompanyMaster", companymaster).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindCompanyMaster();
                        Clear();
                        divGrid.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        divEntry.Visible = false;
                        divGrid.Visible = true;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        divGrid.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                        Clear();
                        BindCompanyMaster();
                        btnSubmit.Text = "Submit";
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        divEntry.Visible = false;
        divGrid.Visible = true;
        lbtnNew.Visible = true;
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            mapview.Visible = true;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvCompanyMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label CorpID = (Label)gvrow.FindControl("lblCorpID");
            Label ShortName = (Label)gvrow.FindControl("lblShortName");
            Label CorpName = (Label)gvrow.FindControl("lblCorpName");
            Label Address1 = (Label)gvrow.FindControl("lblAddress1");
            Label Address2 = (Label)gvrow.FindControl("lblAddress2");
            Label Zipcode = (Label)gvrow.FindControl("lblZipcode");
            Label City = (Label)gvrow.FindControl("lblCity");
            Label District = (Label)gvrow.FindControl("lblDistrict");
            Label State = (Label)gvrow.FindControl("lblState");
            Label Country = (Label)gvrow.FindControl("lblCountry");
            Label Phone1 = (Label)gvrow.FindControl("lblPhone1");
            Label Phone2 = (Label)gvrow.FindControl("lblPhone2");
            Label Fax = (Label)gvrow.FindControl("lblFax");
            Label MailId = (Label)gvrow.FindControl("lblMailId");
            Label AppName = (Label)gvrow.FindControl("lblAppName");

            hfCorLoPreImgLk.Value = gvCompanyMaster.DataKeys[gvrow.RowIndex]["CorpLogo"].ToString().Trim();
            hfCorLoPreImgLk1.Value = gvCompanyMaster.DataKeys[gvrow.RowIndex]["CorpLogo1"].ToString().Trim();

            if(hfCorLoPreImgLk.Value.Trim() == "")
            {
                imgCorLogoPrev.ImageUrl = "~/images/FileUpload.png";
            }
            else
            {
                imgCorLogoPrev.ImageUrl = hfCorLoPreImgLk.Value.Trim();
            }

            if (hfCorLoPreImgLk1.Value.Trim() == "")
            {
                imgCorLogo1Prev.ImageUrl = "~/images/FileUpload.png";
            }
            else
            {
                imgCorLogo1Prev.ImageUrl = hfCorLoPreImgLk1.Value.Trim();
            }

            if (hfCorLoPreImgLk1.Value.Trim() != "")
            {
                imgCorLogo1Prev.ImageUrl = hfCorLoPreImgLk1.Value.Trim();
                hfCorLoDisp.Value = "1";
            }
            else
            {
                hfCorLoMinus.Value = "0";
            }

            hfCorPhPreImgLk.Value = gvCompanyMaster.DataKeys[gvrow.RowIndex]["CorpPhoto"].ToString().Trim();
            hfCorPhPreImgLk1.Value = gvCompanyMaster.DataKeys[gvrow.RowIndex]["CorpPhoto1"].ToString().Trim();

            if (hfCorPhPreImgLk.Value.Trim() == "")
            {
                imgCorPhotoPrev.ImageUrl = "~/images/FileUpload.png";
            }
            else
            {
                imgCorPhotoPrev.ImageUrl = hfCorPhPreImgLk.Value.Trim();
            }

            if (hfCorPhPreImgLk1.Value.Trim() == "")
            {
                imgCorPhoto1Prev.ImageUrl = "~/images/FileUpload.png";
            }
            else
            {
                imgCorPhoto1Prev.ImageUrl = hfCorPhPreImgLk1.Value.Trim();
            }

            if (hfCorPhPreImgLk1.Value.Trim() != "")
            {
                imgCorPhoto1Prev.ImageUrl = hfCorPhPreImgLk1.Value.Trim();
                hfCorPhDisp.Value = "1";
            }
            else
            {
                hfCorPhMinus.Value = "0";
            }

            txtCropId.Text = CorpID.Text;
            txtshortName.Text = ShortName.Text;
            txtCropName.Text = CorpName.Text;
            txtAddress1.Text = Address1.Text;
            txtaddress2.Text = Address2.Text;
            txtZipcode.Text = Zipcode.Text;
            txtCity.Text = City.Text;
            txtDistrict.Text = District.Text;
            txtState.Text = State.Text;
            ddlCountry.SelectedValue = Country.Text;
            txtphone1.Text = Phone1.Text;
            txtphone2.Text = Phone2.Text;
            txtFax.Text = Fax.Text;
            txtEmailId.Text = MailId.Text;
            txtappname.Text = AppName.Text;
            ZipcodeEntry.Visible = true;
            Address.Visible = true;
            PhoneNumber.Visible = true;
            FaxEmail.Visible = true;
            CorporationLogo.Visible = true;
            CorporationPhoto12.Visible = true;
            btnCancel.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindCompanyMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new companymaster()
                {
                    QueryType = "GetCompanyMaster",
                    ServiceType = "",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvCompanyMaster.DataSource = dtExists;
                        gvCompanyMaster.DataBind();
                        lblGridMsg.Text = string.Empty;
                    }
                    else
                    {
                        gvCompanyMaster.DataBind();
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        btnSubmit.Text = "Submit";
                    }
                }
                else
                {
                    divEntry.Visible = true;
                    divGrid.Visible = false;
                    lbtnNew.Visible = false;

                    var Errorresponse = response.Content.ReadAsStringAsync().Result;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void GetCountry()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new companymaster()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlCountry",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlCountry.DataSource = dtExists;
                        ddlCountry.DataValueField = "ConfigId";
                        ddlCountry.DataTextField = "ConfigName";
                        ddlCountry.DataBind();
                    }
                    else
                    {
                        ddlCountry.DataBind();
                    }

                    ddlCountry.Items.Insert(0, "Select Country");
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void Clear()
    {
        txtCropName.Text = string.Empty;
        txtshortName.Text = string.Empty;
        txtAddress1.Text = string.Empty;
        txtaddress2.Text = string.Empty;
        txtZipcode.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        txtState.Text = string.Empty;
        ddlCountry.SelectedIndex = 0;
        txtphone1.Text = string.Empty;
        txtphone2.Text = string.Empty;
        txtEmailId.Text = string.Empty;
        txtappname.Text = string.Empty;
        txtFax.Text = string.Empty;
        ZipcodeEntry.Visible = false;
        Address.Visible = false;
        PhoneNumber.Visible = false;
        FaxEmail.Visible = false;
        CorporationLogo.Visible = false;
        CorporationPhoto12.Visible = false;
        btnCancel.Visible = false;
        hfCorLoDisp.Value = "0";
        hfCorPhDisp.Value = "0";
        imgCorLogoPrev.ImageUrl = "~/images/FileUpload.png";
        imgCorLogo1Prev.ImageUrl = "~/images/FileUpload.png";
        imgCorPhotoPrev.ImageUrl = "~/images/FileUpload.png";
        imgCorPhoto1Prev.ImageUrl = "~/images/FileUpload.png";
        hfCorLoMinus.Value = "0";
        hfCorPhMinus.Value = "0";

        hfCorLogoChkVal.Value = "0";
        hfCorLogo1ChkVal.Value = "0";

        hfCorPhotoChkVal.Value = "0";
        hfCorPhoto1ChkVal.Value = "0";
    }

    protected void txtZipcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtZipcode.Text == "" || txtZipcode.Text == null)
            {

               
                txtAddress1.Text = string.Empty;
                txtaddress2.Text = string.Empty;
                txtZipcode.Text = string.Empty;
                txtCity.Text = string.Empty;
                txtDistrict.Text = string.Empty;
                txtState.Text = string.Empty;
                ddlCountry.SelectedIndex = 0;
                txtphone1.Text = string.Empty;
                txtphone2.Text = string.Empty;
                txtEmailId.Text = string.Empty;
                txtappname.Text = string.Empty;
                txtFax.Text = string.Empty;

                ZipcodeEntry.Visible = false;
                Address.Visible = false;
                PhoneNumber.Visible = false;
                FaxEmail.Visible = false;
                CorporationLogo.Visible = false;
                CorporationPhoto12.Visible = false;
                mapview.Visible = false;
                return;

            }

            using (var client = new HttpClient())
            {
                string ZipURL = "http://www.postalpincode.in/api/pincode/";
                client.BaseAddress = new Uri(ZipURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("http://www.postalpincode.in/api/pincode/" + txtZipcode.Text + "").Result;
                string mesg = " Invalid Zip Code";
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
                            txtCity.Text = dt.Rows[0]["Circle"].ToString();
                            txtDistrict.Text = dt.Rows[0]["District"].ToString();
                            txtState.Text = dt.Rows[0]["State"].ToString();
                            ZipcodeEntry.Visible = true;
                            Address.Visible = true;
                            PhoneNumber.Visible = true;
                            FaxEmail.Visible = true;
                            CorporationLogo.Visible = true;
                            CorporationPhoto12.Visible = true;
                            mapview.Visible = true;
                            txtAddress1.Text = "";
                            txtaddress2.Text = "";
                            btnCancel.Visible = true;
                        }
                        else
                        {
                            txtCity.Text = "";
                            txtDistrict.Text = "";
                            txtState.Text = "";
                            txtAddress1.Text = "";
                            txtaddress2.Text = "";
                            ZipcodeEntry.Visible = false;
                           
                            Address.Visible = false;
                            PhoneNumber.Visible = false;
                            FaxEmail.Visible = false;
                            CorporationLogo.Visible = false;
                            CorporationPhoto12.Visible = false;
                            mapview.Visible = false;
                        }
                    }
                    else
                    {
                        txtCity.Text = "";
                        txtDistrict.Text = "";
                        txtState.Text = "";
                        txtAddress1.Text = "";
                        txtaddress2.Text = "";
                        ZipcodeEntry.Visible = false;
                        Address.Visible = false;
                        PhoneNumber.Visible = false;
                        FaxEmail.Visible = false;
                        CorporationLogo.Visible = false;
                        CorporationPhoto12.Visible = false;
                        mapview.Visible = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + mesg.ToString().Trim() + "');", true);
                        return;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        Clear();
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        mapview.Visible = false;
    }

    public class companymaster
    {
        public string QueryType { get; set; }
        public string CorpId { get; set; }
        public string CorpName { get; set; }
        public string ShortName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string MailId { get; set; }
        public string CorpLogo { get; set; }
        public string CorpLogo1 { get; set; }
        public string AppName { get; set; }
        public string CorpPhoto { get; set; }
        public string CorpPhoto1 { get; set; }
        public string ServiceType { get; set; }
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