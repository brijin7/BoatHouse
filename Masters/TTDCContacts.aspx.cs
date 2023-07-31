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

public partial class TTDCContacts : System.Web.UI.Page
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
				getContact();
				BindTTDC();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}
    public class tTDCContact
    {
        public string QueryType { get; set; }
        public string ContactId { get; set; }
        public string ContactTypeId { get; set; }
        public string ContactName { get; set; }
        public string Designation { get; set; }
        public string ContactInfo { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }

    }
    public async void getContact()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ddlContactType");

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

                            ddlContTypeId.DataSource = dt;
                            ddlContTypeId.DataValueField = "ConfigId";
                            ddlContTypeId.DataTextField = "ConfigName";
                            ddlContTypeId.DataBind();

                        }
                        else
                        {

                            ddlContTypeId.DataBind();
                        }
                        ddlContTypeId.Items.Insert(0, new ListItem("Select Contact", "0"));
                    }
                    else
                    {

                        //     lblGridMsg.Text = ResponseMsg;

                    }
                }
                else
                {

                    //      lblGridMsg.Text = response.ToString();
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
        ddlContTypeId.SelectedIndex = 0;

        txtContactinfo.Text = string.Empty;
        txtContactName.Text = string.Empty;
        txtdesignation.Text = string.Empty;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        BindTTDC();
        divEntry.Visible = false;
        divgrid.Visible = true;
        lbtnNew.Visible = true;


    }

    public async void BindTTDC()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("TTDCContacts/ListAll");

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
                            gvTTDCContact.DataSource = dt;
                            gvTTDCContact.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvTTDCContact.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        Clear();
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
                    var tTDCContact = new tTDCContact()
                    {
                        QueryType = "Insert",
                        ContactId = "0",
                        ContactTypeId = ddlContTypeId.SelectedValue.Trim(),
                        ContactName = txtContactName.Text.Trim(),
                        Designation = txtdesignation.Text.Trim(),
                        ContactInfo = txtContactinfo.Text.Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim()

                    };
                    response = await client.PostAsJsonAsync("TTDCContacts", tTDCContact);

                    //Session["BoatHouseId"] = boatmaster.boat;

                }
                else
                {
                    var tTDCContact = new tTDCContact()
                    {
                        QueryType = "Update",
                        ContactId = txtcontactId.Text.Trim(),
                        ContactTypeId = ddlContTypeId.SelectedValue.Trim(),
                        ContactName = txtContactName.Text.Trim(),
                        Designation = txtdesignation.Text.Trim(),
                        ContactInfo = txtContactinfo.Text.Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim()

                    };
                    response = await client.PostAsJsonAsync("TTDCContacts", tTDCContact);

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        Clear();
                        BindTTDC();
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
                        BindTTDC();
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
            string sTesfg = gvTTDCContact.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ContactId = (Label)gvrow.FindControl("lblContactId");
            Label ContactTypeId = (Label)gvrow.FindControl("lblContactTypeId");
            Label ContactName = (Label)gvrow.FindControl("lblContactName");
            Label Designation = (Label)gvrow.FindControl("lblDesignation");
            Label ContactInfo = (Label)gvrow.FindControl("lblContactInfo");

            txtcontactId.Text = ContactId.Text;
            ddlContTypeId.SelectedValue = ContactTypeId.Text;
            txtContactName.Text = ContactName.Text;
            txtdesignation.Text = Designation.Text;
            txtContactinfo.Text = ContactInfo.Text;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }





    protected void gvTTDCContact_RowDataBound(object sender, GridViewRowEventArgs e)
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
            string ContactId = gvTTDCContact.DataKeys[gvrow.RowIndex].Value.ToString();

            Label ContactTypeId = (Label)gvrow.FindControl("lblContactTypeId");
            Label ContactName = (Label)gvrow.FindControl("lblContactName");
            Label Designation = (Label)gvrow.FindControl("lblDesignation");
            Label ContactInfo = (Label)gvrow.FindControl("lblContactInfo");

            ddlContTypeId.SelectedValue = ContactTypeId.Text;
            txtContactName.Text = ContactName.Text;
            txtdesignation.Text = Designation.Text;
            txtContactinfo.Text = ContactInfo.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tTDCContact = new tTDCContact()
                {
                    QueryType = "Delete",
                    ContactId = ContactId.ToString().Trim(),
                    ContactTypeId = ddlContTypeId.SelectedValue.Trim(),
                    ContactName = txtContactName.Text,
                    Designation = txtdesignation.Text,
                    ContactInfo = txtContactinfo.Text,
                    CreatedBy = hfCreatedBy.Value.Trim()

                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("TTDCContacts", tTDCContact).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindTTDC();
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
            string ContactId = gvTTDCContact.DataKeys[gvrow.RowIndex].Value.ToString();

            Label ContactTypeId = (Label)gvrow.FindControl("lblContactTypeId");
            Label ContactName = (Label)gvrow.FindControl("lblContactName");
            Label Designation = (Label)gvrow.FindControl("lblDesignation");
            Label ContactInfo = (Label)gvrow.FindControl("lblContactInfo");

            ddlContTypeId.SelectedValue = ContactTypeId.Text;
            txtContactName.Text = ContactName.Text;
            txtdesignation.Text = Designation.Text;
            txtContactinfo.Text = ContactInfo.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tTDCContact = new tTDCContact()
                {
                    QueryType = "ReActive",
                    ContactId = ContactId.ToString().Trim(),
                    ContactTypeId = ddlContTypeId.SelectedValue.Trim(),
                    ContactName = txtContactName.Text,
                    Designation = txtdesignation.Text,
                    ContactInfo = txtContactinfo.Text,
                    CreatedBy = hfCreatedBy.Value.Trim()

                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("TTDCContacts", tTDCContact).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindTTDC();
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