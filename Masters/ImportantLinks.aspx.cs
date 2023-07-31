using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ImportantLinks : System.Web.UI.Page
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
				BindImportant();
				getLocation();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}



    public class importantlink
    {
        public string QueryType { get; set; }
        public string LinkId { get; set; }
        public string LinkType { get; set; }
        public string LinkName { get; set; }
        public string LinkUrl { get; set; }
        public string CreatedBy { get; set; }
        public string ActiveStatus { get; set; }
    }

    public async void getLocation()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ConfigMstr/ImpLinks");

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

                            ddllinktype.DataSource = dt;
                            ddllinktype.DataValueField = "ConfigId";
                            ddllinktype.DataTextField = "ConfigName";
                            ddllinktype.DataBind();

                        }
                        else
                        {

                            ddllinktype.DataBind();
                        }
                        ddllinktype.Items.Insert(0, new ListItem("Select LinkType", "0"));
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

    public void clearinputs()
    {
        txtLinkid.Text = string.Empty;
        ddllinktype.SelectedIndex = 0;
        txtlinkname.Text = string.Empty;
        txtLinkUrl.Text = string.Empty;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearinputs();
        BindImportant();
        divEntry.Visible = false;
        divgrid.Visible = true;
        lbtnNew.Visible = true;


    }
    public async void BindImportant()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Importantlinks/ListAll");

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
                            gvimportantlinks.DataSource = dt;
                            gvimportantlinks.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvimportantlinks.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        clearinputs();
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
                HttpResponseMessage response;
                string sMSG = string.Empty;


                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var importantlink = new importantlink()
                    {
                        QueryType = "Insert",
                        LinkId = "0",
                        LinkType = ddllinktype.SelectedValue,
                        LinkName = txtlinkname.Text.Trim(),
                        LinkUrl = txtLinkUrl.Text.Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim()

                    };
                    response = await client.PostAsJsonAsync("ImportantLink", importantlink);

                }
                else
                {
                    var importantlink = new importantlink()
                    {
                        QueryType = "Update",
                        LinkId = txtLinkid.Text.Trim(),
                        LinkType = ddllinktype.SelectedValue,
                        LinkName = txtlinkname.Text.Trim(),
                        LinkUrl = txtLinkUrl.Text.Trim(),

                        CreatedBy = hfCreatedBy.Value.Trim()

                    };
                    response = await client.PostAsJsonAsync("ImportantLink", importantlink);

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        clearinputs();
                        BindImportant();
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
                        clearinputs();
                        BindImportant();
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
        clearinputs();
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
            string sTesfg = gvimportantlinks.DataKeys[gvrow.RowIndex].Value.ToString();
            Label LinkId = (Label)gvrow.FindControl("lblLinkId");
            Label LinkType = (Label)gvrow.FindControl("lblLinkType");
            Label LinkName = (Label)gvrow.FindControl("lblLinkName");
            Label LinkURL = (Label)gvrow.FindControl("lblLinkURL");

            txtLinkid.Text = LinkId.Text;
            ddllinktype.SelectedValue = LinkType.Text;
            txtlinkname.Text = LinkName.Text;
            txtLinkUrl.Text = LinkURL.Text;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }





    protected void gvimportantlinks_RowDataBound(object sender, GridViewRowEventArgs e)
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
            string LinkId = gvimportantlinks.DataKeys[gvrow.RowIndex].Value.ToString();

            Label LinkType = (Label)gvrow.FindControl("lblLinkType");
            Label LinkName = (Label)gvrow.FindControl("lblLinkName");
            Label LinkURL = (Label)gvrow.FindControl("lblLinkURL");


            ddllinktype.SelectedValue = LinkType.Text;
            txtlinkname.Text = LinkName.Text;
            txtLinkUrl.Text = LinkURL.Text;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var importantlink = new importantlink()
                {
                    QueryType = "Delete",
                    LinkId = LinkId.ToString().Trim(),
                    LinkType = ddllinktype.SelectedValue,
                    LinkName = txtlinkname.Text.Trim(),
                    LinkUrl = txtLinkUrl.Text.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()

                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("ImportantLink", importantlink).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindImportant();
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
            string LinkId = gvimportantlinks.DataKeys[gvrow.RowIndex].Value.ToString();

            Label LinkType = (Label)gvrow.FindControl("lblLinkType");
            Label LinkName = (Label)gvrow.FindControl("lblLinkName");
            Label LinkURL = (Label)gvrow.FindControl("lblLinkURL");


            ddllinktype.SelectedValue = LinkType.Text;
            txtlinkname.Text = LinkName.Text;
            txtLinkUrl.Text = LinkURL.Text;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var importantlink = new importantlink()
                {
                    QueryType = "ReActive",
                    LinkId = LinkId.ToString().Trim(),
                    LinkType = ddllinktype.SelectedValue,
                    LinkName = txtlinkname.Text.Trim(),
                    LinkUrl = txtLinkUrl.Text.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()

                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response =  client.PostAsJsonAsync("ImportantLink", importantlink).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindImportant();
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