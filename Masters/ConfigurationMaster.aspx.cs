using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConfigurationMaster : System.Web.UI.Page
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
				getConfigurationType();
				getConfiguration();

				GridValueBind("All");
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public class ConfigurationMstr
    {
        public string QueryType { get; set; }
        public string TypeId { get; set; }
        public string Configname { get; set; }
        public string CreatedBy { get; set; }
        public string ConfigId { get; set; }
    }
    
    public void ChangDynamicDataDisplay()
    {
        VehicleId.InnerText = ddlConfigType.SelectedItem.Text.Trim() + " ID";
        VehicleName.InnerText = ddlConfigType.SelectedItem.Text.Trim() + " Name";
    }

    public void getConfiguration()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLConType").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ddlresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(ddlresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(ddlresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlConType.DataSource = dt;
                            ddlConType.DataValueField = "TypeId";
                            ddlConType.DataTextField = "TypeName";
                            ddlConType.DataBind();
                        }
                        else
                        {
                            ddlConType.DataBind();
                        }
                        ddlConType.Items.Insert(0, new ListItem("Select Master Type", "0"));
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

    public void getConfigurationType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLConType").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ddlresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(ddlresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(ddlresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlConfigType.DataSource = dt;
                            ddlConfigType.DataValueField = "TypeId";
                            ddlConfigType.DataTextField = "TypeName";
                            ddlConfigType.DataBind();
                        }
                        else
                        {
                            ddlConfigType.DataBind();
                        }
                        ddlConfigType.Items.Insert(0, new ListItem("Select Master Type", "0"));
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Main Grid Configuration
    public void BindConfiguration()
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
                if (ddlConType.SelectedValue.Trim() != "0")
                {
                    var ConMstrList1 = new ConfigurationMstr()
                    {
                        TypeId = ddlConType.SelectedValue.Trim()
                    };


                    response = client.PostAsJsonAsync("ConfigMstr/ListAll/Type", ConMstrList1).Result;
                }
                else
                {
                    var ConMstrList1 = new ConfigurationMstr()
                    {
                        TypeId = ""
                    };

                    response = client.PostAsJsonAsync("ConfigMstr/ListAll/Type", ConMstrList1).Result;
                }

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
                            gvConfiguration.DataSource = dt;
                            gvConfiguration.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divFilter.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            gvConfiguration.DataBind();

                            divGrid.Visible = false;
                            divFilter.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;

                        }
                    }
                    else
                    {
                        gvConfiguration.DataBind();
                        divGrid.Visible = false;
                        divFilter.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Sub Grid Configuration
    public void BindConfigurationType()
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
                if (ddlConfigType.SelectedValue.Trim() != "0")
                {
                    var ConMstrList1 = new ConfigurationMstr()
                    {
                        TypeId = ddlConfigType.SelectedValue.Trim()
                    };


                    response = client.PostAsJsonAsync("ConfigMstr/ListAll/Type", ConMstrList1).Result;
                }
                else
                {
                    var ConMstrList1 = new ConfigurationMstr()
                    {
                        TypeId = ""
                    };

                    response = client.PostAsJsonAsync("ConfigMstr/ListAll/Type", ConMstrList1).Result;
                }

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
                            gvConfiguration.DataSource = dt;
                            gvConfiguration.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            lbtnNew.Visible = false;
                        }
                        else
                        {
                            gvConfiguration.DataBind();

                            divGrid.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;

                        }
                    }
                    else
                    {
                        gvConfiguration.DataBind();
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void clearInputs()
    {
        txtConfigName.Text = string.Empty;
        btnSubmit.Text = "Submit";
        ddlConfigType.SelectedIndex = 0;
        ddlConfigType.Enabled = true;
        GridValueBind("All");

        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        VehicleId.InnerText = string.Empty;
        VehicleName.InnerText = string.Empty;


    }
    
    protected void ddlConfigType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangDynamicDataDisplay();
        GridValueBind("Search");
    }

    protected void ddlConType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridValueBind("All");
    }

    protected void gvConfiguration_RowDataBound(Object sender, GridViewRowEventArgs e)
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

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divFilter.Visible = false;
        divGrid.Visible = true;
        lbtnNew.Visible = false;
        getConfigurationType();
        gvConfiguration.PageIndex = 0;
        GridValueBind("Search");
        btnSubmit.Text = "Submit";
        lblGridMsg.Text = " ";
        VehicleName.InnerText = "Configuration Name";

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

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var ConfigurationMstr = new ConfigurationMstr()
                    {
                        QueryType = "Insert",
                        TypeId = ddlConfigType.SelectedValue.Trim(),
                        Configname = txtConfigName.Text.Trim(),
                        ConfigId = "0",
                        CreatedBy = hfCreatedBy.Value.Trim()
                    };
                    response = client.PostAsJsonAsync("ConfigMstr", ConfigurationMstr).Result;

                }
                else
                {
                    var ConfigurationMstr1 = new ConfigurationMstr()
                    {
                        QueryType = "Update",
                        TypeId = ddlConfigType.SelectedValue.Trim(),
                        Configname = txtConfigName.Text.Trim(),
                        ConfigId = txtConfigId.Text,
                        CreatedBy = hfCreatedBy.Value.Trim()
                    };



                    response = client.PostAsJsonAsync("ConfigMstr", ConfigurationMstr1).Result;

                }

                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        clearInputs();
                        GridValueBind("All");
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

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
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = true;
            divFilter.Visible = false;
            lbtnNew.Visible = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvConfiguration.DataKeys[gvrow.RowIndex].Value.ToString();
            Label TypeId = (Label)gvrow.FindControl("lblTypeId");
            Label configId = (Label)gvrow.FindControl("lblConfigId");
            Label ConfigName = (Label)gvrow.FindControl("lblConfigName");

            ddlConfigType.SelectedValue = TypeId.Text;
            ddlConfigType.Enabled = false;
            txtConfigId.Text = configId.Text;
            txtConfigName.Text = ConfigName.Text;
            ChangDynamicDataDisplay();
            gvConfiguration.PageIndex = 0;
            GridValueBind("Search");
            btnSubmit.Text = "Update";

        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {

        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string sTesfg = gvConfiguration.DataKeys[gvrow.RowIndex].Value.ToString();
        Label TypeId = (Label)gvrow.FindControl("lblTypeId");
        Label configId = (Label)gvrow.FindControl("lblConfigId");



        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var ConfigurationMstr = new ConfigurationMstr()
            {
                QueryType = "Delete",
                TypeId = TypeId.Text,
                ConfigId = configId.Text,
                Configname = " ",
                CreatedBy = hfCreatedBy.Value.Trim()

            };

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("ConfigMstr", ConfigurationMstr).Result;

            if (response.IsSuccessStatusCode)
            {
                var submitresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                if (StatusCode == 1)
                {

                    GridValueBind("All");
                    clearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                }

            }
        }


    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvConfiguration.DataKeys[gvrow.RowIndex].Value.ToString();
            Label TypeId = (Label)gvrow.FindControl("lblTypeId");
            Label configId = (Label)gvrow.FindControl("lblConfigId");



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var ConfigurationMstr = new ConfigurationMstr()
                {
                    QueryType = "ReActive",
                    TypeId = TypeId.Text,
                    ConfigId = configId.Text,
                    Configname = " ",
                    CreatedBy = hfCreatedBy.Value.Trim()

                };

                HttpResponseMessage response;

                response = client.PostAsJsonAsync("ConfigMstr", ConfigurationMstr).Result;


                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        GridValueBind("All");
                        clearInputs();
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

    protected void gvConfiguration_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvConfiguration.PageIndex = e.NewPageIndex;

        if (ddlConfigType.SelectedValue.Trim() != "")
        {
            GridValueBind("All");
        }
        else
        {
            GridValueBind("Search");
        }
    }

    public void GridValueBind(string sType)
    {
        if(sType.Trim() == "All")
        {
            BindConfiguration();
        }
        else
        {
            BindConfigurationType();
        }
    }
}