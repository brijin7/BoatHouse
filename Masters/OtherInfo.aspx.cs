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

public partial class OtherInfo : System.Web.UI.Page
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
				BindOtherInfo();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public class ohterinfotype
    {
        public string QueryType { get; set; }
        public int InfoId { get; set; }
        public string InfoName { get; set; }
        public string InfoDescription { get; set; }
        public string InfoImageLink { get; set; }
        public string ActiveStatus { get; set; }
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

    public void clearInputs()
    {
        txtinfoid.Text = string.Empty;
        txtInfoName.Text = string.Empty;
        imgOtherPrev.ImageUrl = "../images/FileUpload.png";
        txtDescription.Text = string.Empty;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearInputs();
        BindOtherInfo();
        divEntry.Visible = false;
        divgrid.Visible = true;
        lbtnNew.Visible = true;
    }


    public async void BindOtherInfo()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("GetOtherInfo/ListAll");

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
                                if (dt.Rows[i]["InfoImageLink"].ToString() != "")
                                {
                                    string ImageUrl = dt.Rows[i]["InfoImageLink"].ToString();
                                    hfPrevImageLink.Value = dt.Rows[i]["InfoImageLink"].ToString();
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
                            gvotherinfo.DataSource = dt;
                            gvotherinfo.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvotherinfo.DataBind();
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
                    file = fupotherLink.FileName;
                }
                else
                {
                    if (hfPrevImageLink.Value != "")
                    {
                        file = Path.GetFileName(hfPrevImageLink.Value.Trim());
                    }
                    else
                    {
                        file = fupotherLink.FileName;
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                destinationPath = dirMapPath + ImageUpload.ImageLink + NewFileName + Path.GetExtension(file);
                fupotherLink.SaveAs(destinationPath);
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
                int InfoId = 0;
                string QueryType = string.Empty;

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                    ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "OI", "");
                    InfoId = 0;
                }
                else
                {
                    QueryType = "Update";
                    InfoId = Convert.ToInt32(txtinfoid.Text.Trim());

                    if (hfImageCheckValue.Value == "1")
                    {
                        if (hfPrevImageLink.Value.Trim() == "")
                        {
                            ImageUploadAPI("Insert", Session["BoatHouseId"].ToString().Trim(), "OI", "");
                        }
                        else
                        {
                            ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "OI", hfPrevImageLink.Value.Trim());
                        }
                    }
                    else
                    {
                        hfResponse.Value = hfPrevImageLink.Value.Trim();
                    }
                }

                var ohterinfotype = new ohterinfotype()
                {
                    QueryType = QueryType,
                    InfoId = InfoId,
                    InfoName = txtInfoName.Text.Trim(),
                    InfoDescription = txtDescription.Text.Trim(),
                    InfoImageLink = hfResponse.Value.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()

                };
                response = await client.PostAsJsonAsync("OtherInfo", ohterinfotype);
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        clearInputs();
                        BindOtherInfo();
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
                        clearInputs();
                        BindOtherInfo();
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
            string sTesfg = gvotherinfo.DataKeys[gvrow.RowIndex].Value.ToString();
            Label InfoId = (Label)gvrow.FindControl("lblInfoId");
            Label InfoName = (Label)gvrow.FindControl("lblInfoName");
            Label InfoDescription = (Label)gvrow.FindControl("lblInfoDescription");
            Label InfoImageLink = (Label)gvrow.FindControl("lblimgLink12");

            txtinfoid.Text = InfoId.Text;
            txtInfoName.Text = InfoName.Text;
            txtDescription.Text = InfoDescription.Text;
            imgOtherPrev.ImageUrl = InfoImageLink.Text.Trim();
            hfPrevImageLink.Value = InfoImageLink.Text.Trim();



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
        clearInputs();

    }

    protected void gvotherinfo_RowDataBound(object sender, GridViewRowEventArgs e)
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
            string InfoId = gvotherinfo.DataKeys[gvrow.RowIndex].Value.ToString();


            Label InfoName = (Label)gvrow.FindControl("lblInfoName");
            Label InfoDescription = (Label)gvrow.FindControl("lblInfoDescription");


            txtInfoName.Text = InfoName.Text;
            txtDescription.Text = InfoDescription.Text;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ohterinfotype = new ohterinfotype()
                {
                    QueryType = "Delete",
                    InfoId = Convert.ToInt32(InfoId.ToString().Trim()),
                    InfoName = txtInfoName.Text.Trim(),
                    InfoDescription = txtDescription.Text.Trim(),
                    InfoImageLink = "Image",
                    CreatedBy = hfCreatedBy.Value.Trim()

                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("OtherInfo", ohterinfotype).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindOtherInfo();
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
            string InfoId = gvotherinfo.DataKeys[gvrow.RowIndex].Value.ToString();


            Label InfoName = (Label)gvrow.FindControl("lblInfoName");
            Label InfoDescription = (Label)gvrow.FindControl("lblInfoDescription");


            txtInfoName.Text = InfoName.Text;
            txtDescription.Text = InfoDescription.Text;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ohterinfotype = new ohterinfotype()
                {
                    QueryType = "ReActive",
                    InfoId = Convert.ToInt32(InfoId.ToString().Trim()),
                    InfoName = txtInfoName.Text.Trim(),
                    InfoDescription = txtDescription.Text.Trim(),
                    InfoImageLink = "Image",
                    CreatedBy = hfCreatedBy.Value.Trim()

                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response =  client.PostAsJsonAsync("OtherInfo", ohterinfotype).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindOtherInfo();
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
}