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

public partial class MasterData : System.Web.UI.Page
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
                BindConfigurationType();
                BindConfigurationType("All");
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = true;
        divEntry.Visible = true;
        ClearInputs();
        lbtnNew.Visible = false;

    }

    protected void ddlConfigType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConfigType.SelectedIndex == 0)
        {
            BindConfigurationType("All");
        }
        else
        {
            BindConfigurationType(ddlConfigType.SelectedValue.Trim());
        }
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
                        ConfigId = "",
                        ConfigName = txtConfigName.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("CM_InsConfigMstr", ConfigurationMstr).Result;
                }
                else
                {
                    var ConfigurationMstr1 = new ConfigurationMstr()
                    {
                        QueryType = "Update",
                        TypeId = ddlConfigType.SelectedValue.Trim(),
                        ConfigId = ViewState["ConfigId"].ToString().Trim(),
                        ConfigName = txtConfigName.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("CM_InsConfigMstr", ConfigurationMstr1).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClearInputs();
                        BindConfigurationType("All");

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
        BindConfigurationType("All");
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = true;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ddlConfigType.Enabled = false;

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            ddlConfigType.SelectedValue = gvConfiguration.DataKeys[gvrow.RowIndex]["TypeId"].ToString().Trim();
            ViewState["ConfigId"] = gvConfiguration.DataKeys[gvrow.RowIndex]["ConfigId"].ToString().Trim();
            txtConfigName.Text = gvConfiguration.DataKeys[gvrow.RowIndex]["ConfigName"].ToString().Trim();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var ConfigurationMstr = new ConfigurationMstr()
                {
                    QueryType = "Delete",
                    TypeId = gvConfiguration.DataKeys[gvrow.RowIndex]["TypeId"].ToString().Trim(),
                    ConfigId = gvConfiguration.DataKeys[gvrow.RowIndex]["ConfigId"].ToString().Trim(),
                    ConfigName = gvConfiguration.DataKeys[gvrow.RowIndex]["ConfigName"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_InsConfigMstr", ConfigurationMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindConfigurationType("All");
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var ConfigurationMstr = new ConfigurationMstr()
                {
                    QueryType = "ReActive",
                    TypeId = gvConfiguration.DataKeys[gvrow.RowIndex]["TypeId"].ToString().Trim(),
                    ConfigId = gvConfiguration.DataKeys[gvrow.RowIndex]["ConfigId"].ToString().Trim(),
                    ConfigName = gvConfiguration.DataKeys[gvrow.RowIndex]["ConfigName"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_InsConfigMstr", ConfigurationMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindConfigurationType("All");
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

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

                var BoatSearch = new ConfigurationMstr()
                {
                    QueryType = "GetConfigTypeDetails",
                    ServiceType = "",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlConfigType.DataSource = dtExists;
                        ddlConfigType.DataValueField = "TypeId";
                        ddlConfigType.DataTextField = "TypeName";
                        ddlConfigType.DataBind();
                    }
                    else
                    {
                        ddlConfigType.DataBind();
                    }

                    ddlConfigType.Items.Insert(0, "Select Master Type");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindConfigurationType(string sType)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new ConfigurationMstr()
                {
                    QueryType = "GetConfigurationMaster",
                    ServiceType = sType.Trim(),
                    CorpId = "",
                    Input1 = sType.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvConfiguration.DataSource = dtExists;
                        gvConfiguration.DataBind();
                        if (sType == "All")
                        {
                            divEntry.Visible = false;
                            divGrid.Visible = true;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            divEntry.Visible = true;
                            divGrid.Visible = true;
                            lbtnNew.Visible = false;
                        }
                    }
                    else
                    {
                        gvConfiguration.DataBind();
                        divEntry.Visible = true;
                        divGrid.Visible = false;
                        lbtnNew.Visible = false;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void ClearInputs()
    {
        ddlConfigType.SelectedIndex = 0;
        ddlConfigType.Enabled = true;
        txtConfigName.Text = "";
        btnSubmit.Text = "Submit";
        gvConfiguration.PageIndex = 0;
    }

    public class ConfigurationMstr
    {
        public string QueryType { get; set; }
        public string TypeId { get; set; }
        public string ConfigName { get; set; }
        public string CreatedBy { get; set; }
        public string ConfigId { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }
}