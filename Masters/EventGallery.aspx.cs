﻿using Newtonsoft.Json;
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
public partial class EventGallery : System.Web.UI.Page
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
                getEvent();
                BindEventsGallery();
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public class eventGallery
    {
        public string EventId { get; set; }
        public int GalleryId { get; set; }
        public string QueryType { get; set; }

        public string EventImageLink { get; set; }

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
                    file = fuEventimage.FileName;
                }
                else
                {
                    if (hfPrevImageLink.Value != "")
                    {
                        file = Path.GetFileName(hfPrevImageLink.Value.Trim());
                    }
                    else
                    {
                        file = fuEventimage.FileName;
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                destinationPath = dirMapPath + ImageUpload.ImageLink + NewFileName + Path.GetExtension(file);
                fuEventimage.SaveAs(destinationPath);
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

    public async void getEvent()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ddlCulturalEvents");

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

                            ddlEvent.DataSource = dt;
                            ddlEvent.DataValueField = "EventId";
                            ddlEvent.DataTextField = "EventName";
                            ddlEvent.DataBind();

                        }
                        else
                        {

                            ddlEvent.DataBind();
                        }
                        ddlEvent.Items.Insert(0, new ListItem("Select Event", "0"));
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
    public async void BindEventsGallery()
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

                response = await client.GetAsync("GetEventGallery/ListAll");
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
                                if (dt.Rows[i]["EventImageLink"].ToString() != "")
                                {
                                    string ImageUrl = dt.Rows[i]["EventImageLink"].ToString();
                                    hfPrevImageLink.Value = dt.Rows[i]["EventImageLink"].ToString();
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
                            gvEventsGallery.DataSource = dt;
                            gvEventsGallery.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvEventsGallery.DataSource = dt;
                            gvEventsGallery.DataBind();
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
                int GalleryId = 0;
                string QueryType = string.Empty;
                //if (btnSubmit.Text.Trim() == "Submit")
                //{
                //    var eventGallery1 = new eventGallery()
                //    {
                //        QueryType = "Insert",
                //        GalleryId = "0",
                //        EventId = ddlEvent.SelectedValue.Trim(),
                //        EventImageLink = "Image",
                //        CreatedBy = hfCreatedBy.Value.Trim()
                //    };

                //    response = await client.PostAsJsonAsync("EventGallery", eventGallery1);

                //}

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                    ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "EG", "");
                    GalleryId = 0;
                }
                else
                {
                    QueryType = "Update";

                    GalleryId = Convert.ToInt32(txtGalleryId.Text.Trim());

                    if (hfImageCheckValue.Value == "1")
                    {
                        if (hfPrevImageLink.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", Session["BoatHouseId"].ToString().Trim(), "EG", "");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "EG", hfPrevImageLink.Value.Trim());
                        }
                    }
                    else
                    {
                        hfResponse.Value = hfPrevImageLink.Value.Trim();
                    }
                }
                var eventGallery1 = new eventGallery()
                {
                    QueryType = QueryType,
                    GalleryId = GalleryId,
                    EventId = ddlEvent.SelectedValue.Trim(),
                    EventImageLink = hfResponse.Value.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = await client.PostAsJsonAsync("EventGallery", eventGallery1);

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindEventsGallery();
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
        ddlEvent.SelectedIndex = 0;
        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        btnSubmit.Text = "Submit";
        imgeventPhotoPrev.ImageUrl = "../images/FileUpload.png";

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        BindEventsGallery();

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
            string sTesfg = gvEventsGallery.DataKeys[gvrow.RowIndex].Value.ToString();
            Label EventId = (Label)gvrow.FindControl("lblEventId");
            Label EventImageLink = (Label)gvrow.FindControl("lblEventImageLink");
            Label GalleryId = (Label)gvrow.FindControl("lblGalleryId");

            ddlEvent.SelectedValue = EventId.Text;
            txtGalleryId.Text = GalleryId.Text;
            imgeventPhotoPrev.ImageUrl = EventImageLink.Text.Trim();
            hfPrevImageLink.Value = EventImageLink.Text.Trim();
            //lblOutput.Visible = false;


        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvEventsGallery_RowDataBound(object sender, GridViewRowEventArgs e)
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
            string sTesfg = gvEventsGallery.DataKeys[gvrow.RowIndex].Value.ToString();
            Label EventId = (Label)gvrow.FindControl("lblEventId");
            Label EventImageLink = (Label)gvrow.FindControl("lblEventImageLink");
            Label GalleryId = (Label)gvrow.FindControl("lblGalleryId");

            ddlEvent.SelectedValue = EventId.Text;
            txtGalleryId.Text = GalleryId.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;
                var eventGallery1 = new eventGallery()
                {
                    QueryType = "Delete",
                    GalleryId = Convert.ToInt32(txtGalleryId.Text),
                    EventId = ddlEvent.SelectedValue.Trim(),
                    EventImageLink = "Image",
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("EventGallery", eventGallery1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindEventsGallery();
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
            string sTesfg = gvEventsGallery.DataKeys[gvrow.RowIndex].Value.ToString();
            Label EventId = (Label)gvrow.FindControl("lblEventId");
            Label EventImageLink = (Label)gvrow.FindControl("lblEventImageLink");
            Label GalleryId = (Label)gvrow.FindControl("lblGalleryId");

            ddlEvent.SelectedValue = EventId.Text;
            txtGalleryId.Text = GalleryId.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;
                var eventGallery1 = new eventGallery()
                {
                    QueryType = "ReActive",
                    GalleryId = Convert.ToInt32(txtGalleryId.Text),
                    EventId = ddlEvent.SelectedValue.Trim(),
                    EventImageLink = "Image",
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("EventGallery", eventGallery1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindEventsGallery();
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
}