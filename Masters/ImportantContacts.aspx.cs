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

public partial class ImportantContacts : System.Web.UI.Page
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
				BindImportantContact();
				getCity();
				getContactType();
				getContactType();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public class importantcontacts
    {
        public string QueryType { get; set; }
        public string CityId { get; set; }
        public string ContactId { get; set; }
        public string ContactTypeId { get; set; }
        public string Description { get; set; }
        public string ContactInfo { get; set; }
        public string CreatedBy { get; set; }

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
                        ddlCity.Items.Insert(0, new ListItem("Select Item", "0"));
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
    public async void getContactType()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ddlContactType");

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

                            ddlCOntact.DataSource = dt;
                            ddlCOntact.DataValueField = "ConfigId";
                            ddlCOntact.DataTextField = "ConfigName";
                            ddlCOntact.DataBind();

                        }
                        else
                        {

                            ddlCOntact.DataBind();
                        }
                        ddlCOntact.Items.Insert(0, new ListItem("Select Contact Type", "0"));
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
                    var importantcontacts1 = new importantcontacts()
                    {
                        QueryType = "Insert",
                        ContactId = "0",
                        CityId = ddlCity.SelectedValue.Trim(),
                        ContactTypeId = ddlCOntact.SelectedValue.Trim(),
                        Description = txtdes.Text,
                        ContactInfo = txtcontactinfo.Text,
                        CreatedBy = hfCreatedBy.Value.Trim()
                    };

                    response = await client.PostAsJsonAsync("ImportantContact", importantcontacts1);

                }
                else
                {
                    var importantcontacts1 = new importantcontacts()
                    {
                        QueryType = "Update",
                        ContactId = txtContactId.Text,
                        CityId = ddlCity.SelectedValue.Trim(),
                        ContactTypeId = ddlCOntact.SelectedValue.Trim(),
                        Description = txtdes.Text,
                        ContactInfo = txtcontactinfo.Text,
                        CreatedBy = hfCreatedBy.Value.Trim()
                    };

                    response = await client.PostAsJsonAsync("ImportantContact", importantcontacts1);

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindImportantContact();
                        clearInputs();
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

    public async void BindImportantContact()
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

                response = await client.GetAsync("ImpContactList/ListAll");
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
                            gvImportantContacts.DataSource = dt;
                            gvImportantContacts.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvImportantContacts.DataSource = dt;
                            gvImportantContacts.DataBind();
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


    protected void btnCancel_Click(object sender, EventArgs e)
    {

        clearInputs();
        BindImportantContact();
    }


    public void clearInputs()
    {
        ddlCity.SelectedIndex = 0;
        txtContactId.Text = string.Empty;
        txtcontactinfo.Text = string.Empty;
        txtdes.Text = string.Empty;
        ddlCOntact.SelectedIndex = 0;
        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        ddlCity.Enabled = true;
        ddlCOntact.Enabled = true;
        btnSubmit.Text = "Submit";
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
            string sTesfg = gvImportantContacts.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ContactId = (Label)gvrow.FindControl("lblContactId");
            Label CityId = (Label)gvrow.FindControl("lblCityId");
            Label ContactTypeId = (Label)gvrow.FindControl("lblContactTypeId");
            Label Description = (Label)gvrow.FindControl("lblDescription");
            Label ContactInfo = (Label)gvrow.FindControl("lblContactInfo");

            ddlCity.SelectedValue = CityId.Text;
            ddlCOntact.SelectedValue = ContactTypeId.Text;
            txtContactId.Text = ContactId.Text;
            txtdes.Text = Description.Text;
            txtcontactinfo.Text = ContactInfo.Text;
            ddlCity.Enabled = false;
            ddlCOntact.Enabled = false;
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
        ddlCity.Enabled = true;
        ddlCOntact.Enabled = true;
    }

    protected void gvImportantContacts_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {

        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvImportantContacts.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ContactId = (Label)gvrow.FindControl("lblContactId");
            Label CityId = (Label)gvrow.FindControl("lblCityId");
            Label ContactTypeId = (Label)gvrow.FindControl("lblContactTypeId");
            Label Description = (Label)gvrow.FindControl("lblDescription");
            Label ContactInfo = (Label)gvrow.FindControl("lblContactInfo");

            ddlCity.SelectedValue = CityId.Text;
            ddlCOntact.SelectedValue = ContactTypeId.Text;
            txtContactId.Text = ContactId.Text;
            txtdes.Text = Description.Text;
            txtcontactinfo.Text = ContactInfo.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;
                var importantcontacts1 = new importantcontacts()
                {
                    QueryType = "Delete",
                    ContactId = txtContactId.Text,
                    CityId = ddlCity.SelectedValue.Trim(),
                    ContactTypeId = ddlCOntact.SelectedValue.Trim(),
                    Description = txtdes.Text,
                    ContactInfo = txtcontactinfo.Text,
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("ImportantContact", importantcontacts1).Result;




                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindImportantContact();
                        clearInputs();
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
            string sTesfg = gvImportantContacts.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ContactId = (Label)gvrow.FindControl("lblContactId");
            Label CityId = (Label)gvrow.FindControl("lblCityId");
            Label ContactTypeId = (Label)gvrow.FindControl("lblContactTypeId");
            Label Description = (Label)gvrow.FindControl("lblDescription");
            Label ContactInfo = (Label)gvrow.FindControl("lblContactInfo");

            ddlCity.SelectedValue = CityId.Text;
            ddlCOntact.SelectedValue = ContactTypeId.Text;
            txtContactId.Text = ContactId.Text;
            txtdes.Text = Description.Text;
            txtcontactinfo.Text = ContactInfo.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;
                var importantcontacts1 = new importantcontacts()
                {
                    QueryType = "ReActive",
                    ContactId = txtContactId.Text,
                    CityId = ddlCity.SelectedValue.Trim(),
                    ContactTypeId = ddlCOntact.SelectedValue.Trim(),
                    Description = txtdes.Text,
                    ContactInfo = txtcontactinfo.Text,
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("ImportantContact", importantcontacts1).Result;




                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindImportantContact();
                        clearInputs();
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