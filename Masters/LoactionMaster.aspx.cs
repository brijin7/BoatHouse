using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

public partial class LoactionMaster : System.Web.UI.Page
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
				BindLocationMaster();
				GetCity();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}
    public class locationMaster
    {
        public string QueryType { get; set; }
        public string CityId { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public string LocationImageLink { get; set; }
        public string CreatedBy { get; set; }
        public string ActiveStatus { get; set; }
        public string CityName { get; set; }
        public int LocationId { get; set; }
        public string HomePageDisplay { get; set; }

    }


    public class ImageUpload
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string FormName { get; set; }
        public string PrevImageLink { get; set; }
        public string ImageLink { get; set; }
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
                    file = fuLocationimage.FileName;
                }
                else
                {
                    if (hfPrevImageLink.Value != "")
                    {
                        file = Path.GetFileName(hfPrevImageLink.Value.Trim());
                    }
                    else
                    {
                        file = fuLocationimage.FileName;
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                destinationPath = dirMapPath + ImageUpload.ImageLink + NewFileName + Path.GetExtension(file);
                fuLocationimage.SaveAs(destinationPath);
                ImageUpload.ImageLink = destinationPath;

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

    public async void GetCity()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ConfigMstr/DDLCity");

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {

                            ddlCity.DataSource = dt;
                            ddlCity.DataValueField = "ConfigId";
                            ddlCity.DataTextField = "ConfigName";
                            ddlCity.DataBind();

                        }
                        else
                        {

                            ddlCity.DataBind();
                        }
                        ddlCity.Items.Insert(0, new ListItem("Select City", "0"));
                    }
                    else
                    {

                        lblGridMsg.Text = ResponseMsg.ToString().Trim();

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


    public async void BindLocationMaster()
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

                response = await client.GetAsync("LocationMaster/ListAll");
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
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["LocationImageLink"].ToString() != "")
                                {
                                    string ImageUrl = dt.Rows[i]["LocationImageLink"].ToString();
                                    hfPrevImageLink.Value = dt.Rows[i]["LocationImageLink"].ToString();
                                    bool VehicleImage_exist = false;
                                    try
                                    {
                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ImageUrl);
                                        using (HttpWebResponse Response = (HttpWebResponse)request.GetResponse())
                                        {
                                            VehicleImage_exist = Response.StatusCode == HttpStatusCode.OK;
                                        }
                                    }
                                    catch
                                    {
                                        VehicleImage_exist = false;
                                    }

                                    //if (VehicleImage_exist == true)
                                    //{
                                    //    WebClient wc = new WebClient();
                                    //    byte[] imageBytes = wc.DownloadData(ImageUrl);
                                    //    var base64 = Convert.ToBase64String(imageBytes);
                                    //    dt.Rows[i]["Carousel"] = string.Format("data:image/jpg;base64,{0}", base64);

                                    //}
                                }
                            }

                            gvlocation.DataSource = dt;
                            gvlocation.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvlocation.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                     
                        divEntry.Visible = true;
                        divGrid.Visible = false;
                        lbtnNew.Visible = false;
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

    protected  void btnSubmit_Click(object sender, EventArgs e)
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
                string FilePath = string.Empty;
                int LocationId = 0;
                string QueryType = string.Empty;
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                    ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "LM", "");
                    LocationId = 0;
                }
                else
                {
                    QueryType = "Update";
                    LocationId = Convert.ToInt32(txtlocationid.Text.Trim());

                    if (hfImageCheckValue.Value == "1")
                    {
                        if (hfPrevImageLink.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", Session["BoatHouseId"].ToString().Trim(), "LM", "");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "LM", hfPrevImageLink.Value.Trim());
                        }
                    }
                    else
                    {
                        hfResponse.Value = hfPrevImageLink.Value.Trim();
                    }
                }
                var locationMaster = new locationMaster()
                {

                    QueryType = QueryType,
                    LocationId = LocationId,
                    LocationName = txtLocationName.Text,
                    CityId = ddlCity.SelectedValue.Trim(),
                    LocationDescription = txtLocationDescription.Text,
                    LocationImageLink = hfResponse.Value.Trim(),
                    HomePageDisplay = Imagedisplay.SelectedValue.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()

                };
                response =  client.PostAsJsonAsync("LocationMaster", locationMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            Clear();
                            BindLocationMaster();
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;

                            BindLocationMaster();
                            Clear();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        var Errorresponse = response.Content.ReadAsStringAsync().Result;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    }
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
        BindLocationMaster();
        btnSubmit.Text = "Submit";
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
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvlocation.DataKeys[gvrow.RowIndex].Value.ToString();
            Label LocationId = (Label)gvrow.FindControl("lblLocationId");
            Label CityId = (Label)gvrow.FindControl("lblCityId");
            Label LocationName = (Label)gvrow.FindControl("lblLocationName");
            Label LocationDescription = (Label)gvrow.FindControl("lblLocationDescription");
            Label HomePageDisplay = (Label)gvrow.FindControl("lblhomedisplay");
            Label LocationImageLink=(Label)gvrow.FindControl("lblimgLink12");
           
            txtlocationid.Text = LocationId.Text;
            ddlCity.SelectedValue = CityId.Text;
            txtLocationName.Text = LocationName.Text;
            txtLocationDescription.Text = LocationDescription.Text;
            Imagedisplay.SelectedValue = HomePageDisplay.Text;
            imgphotolocation.ImageUrl = LocationImageLink.Text.Trim();
            hfPrevImageLink.Value = LocationImageLink.Text.Trim();
            btnSubmit.Text = "Update";
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //protected async void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        ImageButton lnkbtn = sender as ImageButton;
    //        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
    //        string shiftId = gvlocation.DataKeys[gvrow.RowIndex].Value.ToString();
    //        Label LocationId = (Label)gvrow.FindControl("lblLocationId");
    //        Label CityId = (Label)gvrow.FindControl("lblCityId");
    //        Label LocationName = (Label)gvrow.FindControl("lblLocationName");
    //        Label LocationDescription = (Label)gvrow.FindControl("lblLocationDescription");
    //        Label HomePageDisplay = (Label)gvrow.FindControl("lblhomedisplay");
    //        //Label LocationImageLink=(Label)gvrow.FindControl("lblLocationImageLink");

    //        txtlocationid.Text = LocationId.Text;
    //        ddlCity.SelectedValue = CityId.Text;
    //        txtLocationName.Text = LocationName.Text;
    //        txtLocationDescription.Text = LocationDescription.Text;
    //        Imagedisplay.SelectedValue = HomePageDisplay.Text;

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //            var locationMaster = new locationMaster()
    //            {

    //                QueryType = "Delete",

    //                LocationId = txtlocationid.Text.Trim(),
    //                LocationName = txtLocationName.Text,
    //                CityId = ddlCity.SelectedValue.Trim(),
    //                LocationDescription = txtLocationDescription.Text,
    //                LocationImageLink = "Image",
    //                HomePageDisplay = Imagedisplay.SelectedValue.Trim(),


    //                CreatedBy = hfCreatedBy.Value.Trim()



    //            };

    //            HttpResponseMessage response;
    //            string sMSG = string.Empty;

    //            response = await client.PostAsJsonAsync("LocationMaster", locationMaster);
    //            sMSG = "Location Master Details Deleted Successfully";


    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
    //                if (StatusCode == 1)
    //                {
    //                    BindLocationMaster();
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

    //                }
    //            }
    //            else
    //            {
    //                lblGridMsg.Text = response.ToString();
    //            }
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}

    protected void btnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        Clear();
    }

    public void Clear()
    {
        txtLocationName.Text = string.Empty;
        ddlCity.SelectedIndex = 0;
        txtLocationDescription.Text = string.Empty;
        imgphotolocation.ImageUrl = "../images/FileUpload.png";

    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string shiftId = gvlocation.DataKeys[gvrow.RowIndex].Value.ToString();
            Label LocationId = (Label)gvrow.FindControl("lblLocationId");
            Label CityId = (Label)gvrow.FindControl("lblCityId");
            Label LocationName = (Label)gvrow.FindControl("lblLocationName");
            Label LocationDescription = (Label)gvrow.FindControl("lblLocationDescription");
            Label HomePageDisplay = (Label)gvrow.FindControl("lblhomedisplay");
            //Label LocationImageLink=(Label)gvrow.FindControl("lblLocationImageLink");

            txtlocationid.Text = LocationId.Text;
            ddlCity.SelectedValue = CityId.Text;
            txtLocationName.Text = LocationName.Text;
            txtLocationDescription.Text = LocationDescription.Text;
            Imagedisplay.SelectedValue = HomePageDisplay.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var locationMaster = new locationMaster()
                {

                    QueryType = "ReActive",

                    LocationId = Convert.ToInt32(txtlocationid.Text.Trim()),
                    LocationName = txtLocationName.Text,
                    CityId = ddlCity.SelectedValue.Trim(),
                    LocationDescription = txtLocationDescription.Text,
                    LocationImageLink = "Image",
                    HomePageDisplay = Imagedisplay.SelectedValue.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;
                response = client.PostAsJsonAsync("LocationMaster", locationMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindLocationMaster();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }
                }
                else
                {
                    lblGridMsg.Text = response.ToString();
                }
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
            string shiftId = gvlocation.DataKeys[gvrow.RowIndex].Value.ToString();
            Label LocationId = (Label)gvrow.FindControl("lblLocationId");
            Label CityId = (Label)gvrow.FindControl("lblCityId");
            Label LocationName = (Label)gvrow.FindControl("lblLocationName");
            Label LocationDescription = (Label)gvrow.FindControl("lblLocationDescription");
            Label HomePageDisplay = (Label)gvrow.FindControl("lblhomedisplay");
            //Label LocationImageLink=(Label)gvrow.FindControl("lblLocationImageLink");

            txtlocationid.Text = LocationId.Text;
            ddlCity.SelectedValue = CityId.Text;
            txtLocationName.Text = LocationName.Text;
            txtLocationDescription.Text = LocationDescription.Text;
            Imagedisplay.SelectedValue = HomePageDisplay.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var locationMaster = new locationMaster()
                {

                    QueryType = "Delete",

                    LocationId = Convert.ToInt32(txtlocationid.Text.Trim()),
                    LocationName = txtLocationName.Text,
                    CityId = ddlCity.SelectedValue.Trim(),
                    LocationDescription = txtLocationDescription.Text,
                    LocationImageLink = "Image",
                    HomePageDisplay = Imagedisplay.SelectedValue.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;
                response = client.PostAsJsonAsync("LocationMaster", locationMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindLocationMaster();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }
                }
                else
                {
                    lblGridMsg.Text = response.ToString();
                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvlocation_RowDataBound(object sender, GridViewRowEventArgs e)
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
}