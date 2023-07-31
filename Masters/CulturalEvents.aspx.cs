using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

public partial class CulturalEvents : System.Web.UI.Page
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
				BindCulturalEvents();
				getEventCity();
				getEventType();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}
    public class culturalEvents
    {
        public int EventId { get; set; }
        public string QueryType { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventImageLink { get; set; }
        public string EventType { get; set; }
        public string EventCity { get; set; }
        public string CreatedBy { get; set; }

    }
    public class ImageUpload
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string FormName { get; set; }
        public string PrevImageLink { get; set; }
        public string ImageLink { get; set; }
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
                    file = fupCulturalLink.FileName;
                }
                else
                {
                    if (hfPrevImageLink.Value != "")
                    {
                        file = Path.GetFileName(hfPrevImageLink.Value.Trim());
                    }
                    else
                    {
                        file = fupCulturalLink.FileName;
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                destinationPath = dirMapPath + ImageUpload.ImageLink + NewFileName + Path.GetExtension(file);
                fupCulturalLink.SaveAs(destinationPath);
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

    public async void getEventCity()
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

                            ddlEventCity.DataSource = dt;
                            ddlEventCity.DataValueField = "ConfigId";
                            ddlEventCity.DataTextField = "ConfigName";
                            ddlEventCity.DataBind();

                        }
                        else
                        {

                            ddlEventCity.DataBind();
                        }
                        ddlEventCity.Items.Insert(0, new ListItem("Select City", "0"));
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
    public async void getEventType()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ConfigMstr/ddlEventType");

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

                            ddlEventType.DataSource = dt;
                            ddlEventType.DataValueField = "ConfigId";
                            ddlEventType.DataTextField = "ConfigName";
                            ddlEventType.DataBind();

                        }
                        else
                        {

                            ddlEventType.DataBind();
                        }
                        ddlEventType.Items.Insert(0, new ListItem("Select Type", "0"));
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
    public async void BindCulturalEvents()
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

                response = await client.GetAsync("CulturalEvents/ListAll");
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
                            gvCulturalEvents.DataSource = dt;
                            gvCulturalEvents.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvCulturalEvents.DataSource = dt;
                            gvCulturalEvents.DataBind();
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
    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divgrid.Visible = false;
        divEntry.Visible = true;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        //ddl.Enabled = true;
        //ddlCOntact.Enabled = true;
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

                HttpResponseMessage response;
                string FilePath = string.Empty;
                int EventId = 0;
                string QueryType = string.Empty;
                //if (btnSubmit.Text.Trim() == "Submit")
                //{
                //    var culturalEvents1 = new culturalEvents()
                //    {
                //        QueryType = "Insert",
                //        EventId = "0",
                //        EventName = txtEventName.Text.Trim(),
                //        EventDescription = txtEventDes.Text.Trim(),
                //        EventImageLink = "Image",
                //        EventType = ddlEventType.SelectedValue.Trim(),
                //        EventCity = ddlEventCity.SelectedValue.Trim(),
                //        CreatedBy = hfCreatedBy.Value.Trim()
                //    };

                //    response = await client.PostAsJsonAsync("CulturalEvents", culturalEvents1);

                //}
                //else
                //{
                //    var culturalEvents1 = new culturalEvents()
                //    {
                //        QueryType = "Update",
                //        EventId = txtEventId.Text,
                //        EventName = txtEventName.Text.Trim(),
                //        EventDescription = txtEventDes.Text.Trim(),
                //        EventImageLink = "Image",
                //        EventType = ddlEventType.SelectedValue.Trim(),
                //        EventCity = ddlEventCity.SelectedValue.Trim(),
                //        CreatedBy = hfCreatedBy.Value.Trim()
                //    };

                //    response = await client.PostAsJsonAsync("CulturalEvents", culturalEvents1);

                //}
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                    ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "CE", "");
                    EventId = 0;
                }
                else
                {
                    QueryType = "Update";
                    EventId = Convert.ToInt32(txtEventId.Text.Trim());

                    if (hfImageCheckValue.Value == "1")
                    {
                        if (hfPrevImageLink.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", Session["BoatHouseId"].ToString().Trim(), "CE", "");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "CE", hfPrevImageLink.Value.Trim());
                        }
                    }
                    else
                    {
                        hfResponse.Value = hfPrevImageLink.Value.Trim();
                    }
                }

                var culturalEvents1 = new culturalEvents()
                {
                    QueryType = QueryType,
                    EventId = EventId,
                    EventName = txtEventName.Text.Trim(),
                    EventDescription = txtEventDes.Text.Trim(),
                    EventImageLink = hfResponse.Value.Trim(),
                    EventType = ddlEventType.SelectedValue.Trim(),
                    EventCity = ddlEventCity.SelectedValue.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = await client.PostAsJsonAsync("CulturalEvents", culturalEvents1);

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindCulturalEvents();
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

    public void Clear()
    {
        ddlEventCity.SelectedIndex = 0;
        ddlEventType.SelectedIndex = 0;
        txtEventDes.Text = string.Empty;
        txtEventName.Text = string.Empty;
        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        btnSubmit.Text = "Submit";
        imgCulturalPrev.ImageUrl = "../images/FileUpload.png";

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        BindCulturalEvents();
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
            string sTesfg = gvCulturalEvents.DataKeys[gvrow.RowIndex].Value.ToString();
            Label EventId = (Label)gvrow.FindControl("lblEventId");
            Label EventName = (Label)gvrow.FindControl("lblEventName");
            Label EventTypeId = (Label)gvrow.FindControl("lblEventTypeId");
            Label EventCityId = (Label)gvrow.FindControl("lblEventCityId");
   
            Label EventDescription = (Label)gvrow.FindControl("lblEventDescription");
            Label EventImageLink = (Label)gvrow.FindControl("lblEventImageLink");

            imgCulturalPrev.ImageUrl = EventImageLink.Text.Trim();
            hfPrevImageLink.Value = EventImageLink.Text.Trim();

            txtEventId.Text = EventId.Text;
            txtEventName.Text = EventName.Text;
            ddlEventCity.SelectedValue = EventCityId.Text;
            ddlEventType.SelectedValue = EventTypeId.Text;
            txtEventDes.Text = EventDescription.Text;





        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvCulturalEvents_RowDataBound(object sender, GridViewRowEventArgs e)
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
            string sTesfg = gvCulturalEvents.DataKeys[gvrow.RowIndex].Value.ToString();
            Label EventId = (Label)gvrow.FindControl("lblEventId");
            Label EventName = (Label)gvrow.FindControl("lblEventName");
            Label EventTypeId = (Label)gvrow.FindControl("lblEventTypeId");
            Label EventCityId = (Label)gvrow.FindControl("lblEventCityId");
          
            Label EventDescription = (Label)gvrow.FindControl("lblEventDescription");
            Label EventImageLink = (Label)gvrow.FindControl("lblEventImageLink");

            imgCulturalPrev.ImageUrl = EventImageLink.Text.Trim();
            hfPrevImageLink.Value = EventImageLink.Text.Trim();

            txtEventId.Text = EventId.Text;
            txtEventName.Text = EventName.Text;
            ddlEventCity.SelectedValue = EventCityId.Text;
            ddlEventType.SelectedValue = EventTypeId.Text;
            txtEventDes.Text = EventDescription.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;
                var culturalEvents1 = new culturalEvents()
                {
                    QueryType = "Delete",
                    EventId = Convert.ToInt32(txtEventId.Text),
                    EventName = txtEventName.Text.Trim(),
                    EventDescription = txtEventDes.Text.Trim(),
                    EventImageLink = hfPrevImageLink.Value,
                    EventType = ddlEventType.SelectedValue.Trim(),
                    EventCity = ddlEventCity.SelectedValue.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("CulturalEvents", culturalEvents1).Result;




                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindCulturalEvents();
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

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvCulturalEvents.DataKeys[gvrow.RowIndex].Value.ToString();
            Label EventId = (Label)gvrow.FindControl("lblEventId");
            Label EventName = (Label)gvrow.FindControl("lblEventName");
            Label EventTypeId = (Label)gvrow.FindControl("lblEventTypeId");
            Label EventCityId = (Label)gvrow.FindControl("lblEventCityId");
           
            Label EventDescription = (Label)gvrow.FindControl("lblEventDescription");
            Label EventImageLink = (Label)gvrow.FindControl("lblEventImageLink");

            imgCulturalPrev.ImageUrl = EventImageLink.Text.Trim();
            hfPrevImageLink.Value = EventImageLink.Text.Trim();
            txtEventId.Text = EventId.Text;
            txtEventName.Text = EventName.Text;
            ddlEventCity.SelectedValue = EventCityId.Text;
            ddlEventType.SelectedValue = EventTypeId.Text;
            txtEventDes.Text = EventDescription.Text;



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var culturalEvents1 = new culturalEvents()
                {
                    QueryType = "ReActive",
                    EventId = Convert.ToInt32(txtEventId.Text.Trim()),
                    EventName = txtEventName.Text.Trim(),
                    EventDescription = txtEventDes.Text.Trim(),
                    EventImageLink = hfPrevImageLink.Value,
                    EventType = ddlEventType.SelectedValue.Trim(),
                    EventCity = ddlEventCity.SelectedValue.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response =  client.PostAsJsonAsync("CulturalEvents", culturalEvents1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindCulturalEvents();
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