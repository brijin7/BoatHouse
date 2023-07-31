using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class OtherGallery : System.Web.UI.Page
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
				BindOtherGallery();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}
    public class otherGallery
    {
        public string GalleryId { get; set; }
        public string Type { get; set; }
        public string ImageVideoLink { get; set; }
        public string QueryType { get; set; }
        public string CreatedBy { get; set; }

    }

    public async void BindOtherGallery()
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

                response = await client.GetAsync("OtherGallery/ListAll");
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
                            gvOtherGallery.DataSource = dt;
                            gvOtherGallery.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvOtherGallery.DataSource = dt;
                            gvOtherGallery.DataBind();
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
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var otherGallery1 = new otherGallery()
                    {
                        QueryType = "Insert",
                        GalleryId = "0",
                        Type = rblGalleryType.SelectedValue.Trim(),
                        ImageVideoLink = "Image/Video",
                        CreatedBy = hfCreatedBy.Value.Trim()
                    };

                    response = await client.PostAsJsonAsync("OtherGallery", otherGallery1);

                }
                else
                {
                    var otherGallery1 = new otherGallery()
                    {
                        QueryType = "Update",
                        GalleryId = txtGalleryId.Text,
                        Type = rblGalleryType.SelectedValue.Trim(),
                        ImageVideoLink = "Video/Image",
                        CreatedBy = hfCreatedBy.Value.Trim()
                    };

                    response = await client.PostAsJsonAsync("OtherGallery", otherGallery1);

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindOtherGallery();
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

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvOtherGallery.DataKeys[gvrow.RowIndex].Value.ToString();
            Label GalleryId = (Label)gvrow.FindControl("lblGalleryId");
            Label Type = (Label)gvrow.FindControl("lblType");
            Label ImgVideo = (Label)gvrow.FindControl("lblImgVideo");
            txtGalleryId.Text = GalleryId.Text;
            rblGalleryType.SelectedValue = Type.Text;

        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void rblGalleryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblGalleryType.SelectedValue.Trim() == "V")
        {
            divVideo.Visible = false;
            divImage.Visible = false;          
            fuVideolink.Dispose();

        }
        else
        {
            fuVideolink.Dispose();
            fuVideolink.Attributes.Clear();
            divImage.Visible = false;
            divVideo.Visible = false;
            imgothimagePrev.ImageUrl = "../images/FileUpload.png";


        }
    }

    public void Clear()
    {
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        rblGalleryType.SelectedValue = "P";
        divImage.Visible = false;
        divVideo.Visible = false;
        imgothimagePrev.ImageUrl = "../images/FileUpload.png";
        imgothvideoPrev.ImageUrl = "../images/FileUpload.png";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        BindOtherGallery();
    }
    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divEntry.Visible = true;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        rblGalleryType.SelectedValue = "P";
        divImage.Visible = false;
        divVideo.Visible = false;

    }

    protected void gvOtherGallery_RowDataBound(object sender, GridViewRowEventArgs e)
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
            string sTesfg = gvOtherGallery.DataKeys[gvrow.RowIndex].Value.ToString();
            Label GalleryId = (Label)gvrow.FindControl("lblGalleryId");
            Label Type = (Label)gvrow.FindControl("lblType");
            Label ImgVideo = (Label)gvrow.FindControl("lblImgVideo");
            txtGalleryId.Text = GalleryId.Text;
            rblGalleryType.SelectedValue = Type.Text;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;
                var otherGallery1 = new otherGallery()
                {
                    QueryType = "Delete",
                    GalleryId = txtGalleryId.Text,
                    Type = rblGalleryType.SelectedValue.Trim(),
                    ImageVideoLink = "Image/Video",
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("OtherGallery", otherGallery1).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindOtherGallery();
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
            string sTesfg = gvOtherGallery.DataKeys[gvrow.RowIndex].Value.ToString();
            Label GalleryId = (Label)gvrow.FindControl("lblGalleryId");
            Label Type = (Label)gvrow.FindControl("lblType");
            Label ImgVideo = (Label)gvrow.FindControl("lblImgVideo");
            txtGalleryId.Text = GalleryId.Text;
            rblGalleryType.SelectedValue = Type.Text;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;
                var otherGallery1 = new otherGallery()
                {
                    QueryType = "ReActive",
                    GalleryId = txtGalleryId.Text,
                    Type = rblGalleryType.SelectedValue.Trim(),
                    ImageVideoLink = "Image/Video",
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response =  client.PostAsJsonAsync("OtherGallery", otherGallery1).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindOtherGallery();
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