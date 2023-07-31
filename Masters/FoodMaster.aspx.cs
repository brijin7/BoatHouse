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

public partial class FoodMaster : System.Web.UI.Page
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
				BindFoodMaster();
				getCity();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public class Foodmaster
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public string FoodImageLink { get; set; }
        public string VegNonVeg { get; set; }
        public string FoodCity { get; set; }
        public string CreatedBy { get; set; }
        public string QueryType { get; set; }
        public string ActiveStatus { get; set; }

    }
    public class ImageUpload
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string FormName { get; set; }
        public string PrevImageLink { get; set; }
        public string ImageLink { get; set; }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BindFoodMaster();
        Clear();
        divEntry.Visible = false;
        divgrid.Visible = true;
        lbtnNew.Visible = true;
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
                    file = fupfoodLink.FileName;
                }
                else
                {
                    if (hfPrevImageLink.Value != "")
                    {
                        file = Path.GetFileName(hfPrevImageLink.Value.Trim());
                    }
                    else
                    {
                        file = fupfoodLink.FileName;
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                destinationPath = dirMapPath + ImageUpload.ImageLink + NewFileName + Path.GetExtension(file);
                fupfoodLink.SaveAs(destinationPath);
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
    public void Clear()
    {
        txtFoodName.Text = string.Empty;
        ddlfood.SelectedIndex = 0;
        ddlfoodcity.SelectedIndex = 0;
        txtfoodDes.Text = string.Empty;
        imgfoodPhotoPrev.ImageUrl = "../images/FileUpload.png";

    }
    public async void getCity()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ConfigMstr/DDLCity");

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

                            ddlfoodcity.DataSource = dt;
                            ddlfoodcity.DataValueField = "ConfigId";
                            ddlfoodcity.DataTextField = "ConfigName";
                            ddlfoodcity.DataBind();

                        }
                        else
                        {

                            ddlfoodcity.DataBind();
                        }
                        ddlfoodcity.Items.Insert(0, new ListItem("Select City", "0"));
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);

                    }
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public async void BindFoodMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("FoodMaster/ListAll");

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
                                if (dt.Rows[i]["FoodImageLink"].ToString() != "")
                                {
                                    string ImageUrl = dt.Rows[i]["FoodImageLink"].ToString();
                                    hfPrevImageLink.Value = dt.Rows[i]["FoodImageLink"].ToString();
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
                            gvFoodMaster.DataSource = dt;
                            gvFoodMaster.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvFoodMaster.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                     
                        divEntry.Visible = true;
                        divgrid.Visible = false;
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
    protected async void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string FilePath = string.Empty;
                int FoodId = 0;
                string QueryType = string.Empty;
                HttpResponseMessage response;

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                    ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "FM", "");
                    FoodId = 0;
                }
                else
                {
                    QueryType = "Update";
                    FoodId = Convert.ToInt32(txtFoodId.Text.Trim());

                    if (hfImageCheckValue.Value == "1")
                    {
                        if (hfPrevImageLink.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", Session["BoatHouseId"].ToString().Trim(), "FM", "");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "FM", hfPrevImageLink.Value.Trim());
                        }
                    }
                    else
                    {
                        hfResponse.Value = hfPrevImageLink.Value.Trim();
                    }
                }



                var Foodmaster = new Foodmaster()
                {
                    QueryType = QueryType,
                    FoodId = FoodId,
                    FoodName = txtFoodName.Text.Trim(),
                    FoodDescription = txtfoodDes.Text.Trim(),
                    FoodImageLink = hfResponse.Value.Trim(),
                    FoodCity = ddlfoodcity.SelectedValue,
                    VegNonVeg = ddlfood.SelectedValue,
                    CreatedBy = hfCreatedBy.Value.Trim()

                };
                response = await client.PostAsJsonAsync("FoodMaster", Foodmaster);
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        Clear();
                        BindFoodMaster();
                        divgrid.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }
                    else
                    {
                        divgrid.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                        Clear();
                        BindFoodMaster();
                        btnSubmit.Text = "Submit";
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

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divgrid.Visible = false;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        Clear();

    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divgrid.Visible = false;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvFoodMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label FoodId = (Label)gvrow.FindControl("lblFoodId");
            Label FoodName = (Label)gvrow.FindControl("lblFoodName");
            Label FoodDescription = (Label)gvrow.FindControl("lblFoodDescription");
            Label VegNonVeg = (Label)gvrow.FindControl("lblVegNonVeg");
            Label FoodCity = (Label)gvrow.FindControl("lblFoodCity");
            Label FoodType = (Label)gvrow.FindControl("lblFoodType");
            Label FoodImageLink = (Label)gvrow.FindControl("lblFoodImageLink");
            txtFoodId.Text = FoodId.Text;
            txtFoodName.Text = FoodName.Text;
            txtfoodDes.Text = FoodDescription.Text;
            ddlfoodcity.SelectedValue = FoodCity.Text;
            imgfoodPhotoPrev.ImageUrl = FoodImageLink.Text.Trim();
            hfPrevImageLink.Value = FoodImageLink.Text.Trim();
            if (FoodType.Text == "Veg")
            {
                ddlfood.SelectedValue = "V";
            }
            else
            {
                ddlfood.SelectedValue = "N";
            }
            //   ddlfood.SelectedValue = FoodType.Text;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }



    protected void gvFoodMaster_RowDataBound(object sender, GridViewRowEventArgs e)
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



    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string FoodId = gvFoodMaster.DataKeys[gvrow.RowIndex].Value.ToString();


            Label FoodName = (Label)gvrow.FindControl("lblFoodName");
            Label FoodDescription = (Label)gvrow.FindControl("lblFoodDescription");
            Label VegNonVeg = (Label)gvrow.FindControl("lblVegNonVeg");
            Label FoodCity = (Label)gvrow.FindControl("lblFoodCity");
            Label FoodType = (Label)gvrow.FindControl("lblFoodType");
            Label FoodImageLink = (Label)gvrow.FindControl("lblFoodImageLink");
            imgfoodPhotoPrev.ImageUrl = FoodImageLink.Text.Trim();
            hfPrevImageLink.Value = FoodImageLink.Text.Trim();
            txtFoodId.Text = FoodId;
            txtFoodName.Text = FoodName.Text;
            txtfoodDes.Text = FoodDescription.Text;
            ddlfoodcity.SelectedValue = FoodCity.Text;
            if (FoodType.Text == "Veg")
            {
                ddlfood.SelectedValue = "V";
            }
            else
            {
                ddlfood.SelectedValue = "N";
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Foodmaster = new Foodmaster()
                {
                    QueryType = "Delete",
                    FoodId = Convert.ToInt32(txtFoodId.Text),
                    FoodName = txtFoodName.Text,
                    FoodDescription = txtfoodDes.Text,
                    FoodImageLink = hfPrevImageLink.Value.Trim(),
                    FoodCity = ddlfoodcity.SelectedValue,
                    VegNonVeg = ddlfood.SelectedValue,
                    CreatedBy = hfCreatedBy.Value.Trim()

                };



                HttpResponseMessage response;


                response = client.PostAsJsonAsync("FoodMaster", Foodmaster).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindFoodMaster();
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

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvFoodMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label FoodId = (Label)gvrow.FindControl("lblFoodId");
            Label FoodName = (Label)gvrow.FindControl("lblFoodName");
            Label FoodDescription = (Label)gvrow.FindControl("lblFoodDescription");
            Label VegNonVeg = (Label)gvrow.FindControl("lblVegNonVeg");
            Label FoodCity = (Label)gvrow.FindControl("lblFoodCity");
            Label FoodType = (Label)gvrow.FindControl("lblFoodType");
            Label FoodImageLink = (Label)gvrow.FindControl("lblFoodImageLink");
            imgfoodPhotoPrev.ImageUrl = FoodImageLink.Text.Trim();
            hfPrevImageLink.Value = FoodImageLink.Text.Trim();
            txtFoodId.Text = FoodId.Text;
            txtFoodName.Text = FoodName.Text;
            txtfoodDes.Text = FoodDescription.Text;
            ddlfoodcity.SelectedValue = FoodCity.Text;



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Foodmaster = new Foodmaster()
                {
                    QueryType = "ReActive",
                    FoodId = Convert.ToInt32(txtFoodId.Text),
                    FoodName = txtFoodName.Text,
                    FoodDescription = txtfoodDes.Text,
                    FoodImageLink = hfPrevImageLink.Value.Trim(),
                   FoodCity = ddlfoodcity.SelectedValue,
                    VegNonVeg = ddlfood.SelectedValue,
                    CreatedBy = hfCreatedBy.Value.Trim()
                };
                HttpResponseMessage response;


                response =  client.PostAsJsonAsync("FoodMaster", Foodmaster).Result;




                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindFoodMaster();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }
}




