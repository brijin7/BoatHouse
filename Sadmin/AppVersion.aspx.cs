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

public partial class Sadmin_AppVersion : System.Web.UI.Page
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
                //Changes
                BindactiveApp();
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindactiveApp()
    {
        try
        {


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("CM_AppVersionBindA").Result;

                if (response.IsSuccessStatusCode)
                {


                    var Locresponse = response.Content.ReadAsStringAsync().Result;

                    string ResponseMsg = JObject.Parse(Locresponse)["Table"].ToString();

                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        gvAppVersion.DataSource = dt;

                        gvAppVersion.DataBind();
                    }
                    else
                    {
                        gvAppVersion.DataBind();
                        lblGridMsg.Text = "No Records Found !";
                        divGrid.Visible = true;
                    }


                }
                else
                {
                    gvAppVersion.DataBind();
                    lblGridMsg.Text = "No Records Found !";
                    divGrid.Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindInactiveApp()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var Gateway = new AppVersion()
                {
                    AppType = ddlapp.SelectedValue.ToString().Trim(),



                };

                response = client.PostAsJsonAsync("CM_AppVersionBindD", Gateway).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    // int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Table"].ToString();

                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        gvInactive.DataSource = dt;

                        gvInactive.DataBind();
                    }
                    else
                    {
                        gvInactive.DataBind();
                    }



                }
                else
                {
                    gvInactive.DataBind();
                    lblGridMsg1.Text = "No Records Found !";
                    divInactive.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var Gateway = new AppVersion()
                {
                    AppType = ddlapp.SelectedValue.ToString().Trim(),
                    VersionNo = txtversion.Text,
                    CreatedBy = Session["UserId"].ToString()
                };

                response = client.PostAsJsonAsync("CMAppVersionDetails", Gateway).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClearInputs();
                        BindactiveApp();


                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                    }
                    else
                    {
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.Trim() + "');", true);
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
        ddlapp.SelectedIndex = 0;
        txtversion.Text = string.Empty;
    }

    protected void lbtnBlockReason_Click(object sender, EventArgs e)
    {

        divEntry.Visible = false;
        BindInactiveApp();
        divGrid.Visible = false;
        lnkback.Visible = true;
        lbtnBlockReason.Visible = false;
        divInactive.Visible = true;
    }

    public class AppVersion
    {
        public string AppType { get; set; }
        public string VersionNo { get; set; }
        public string CreatedBy { get; set; }
    }

    protected void lnkback_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        BindactiveApp();
        lnkback.Visible = false;
        lbtnBlockReason.Visible = true;
        divGrid.Visible = true;
        divInactive.Visible = false;
    }
}