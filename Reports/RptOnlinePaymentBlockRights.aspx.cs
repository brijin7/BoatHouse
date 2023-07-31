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

public partial class Reports_RptOnlinePaymentBlockRights : System.Web.UI.Page
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

                BindPaymentRightsDetailsBlock();
                //if(ddlReason.SelectedValue=="1")
                //{
                //    gvPaymentRights.Columns[7].Visible = false;
                //    gvPaymentRights.Columns[8].Visible = false;
                //    gvPaymentRights.Columns[5].Visible = true;
                //    gvPaymentRights.Columns[6].Visible = true;
                //}
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    public void BindPaymentRightsDetailsBlock()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new PaymentRightsDetails()
                {
                    QueryType = "ShowOnlinePaymentRightsBlock",
                    ServiceType = "",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvPaymentRights.DataSource = dtExists;
                        gvPaymentRights.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                    }
                    else
                    {
                        gvPaymentRights.DataBind();
                        lblGridMsg.Text = "No Records Found !";
                        divGrid.Visible = true;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindPaymentRightsDetailsUnBlock()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new PaymentRightsDetails()
                {
                    QueryType = "ShowOnlinePaymentRightsUnBlock",
                    ServiceType = "",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvPaymentRights.DataSource = dtExists;
                        gvPaymentRights.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                    }
                    else
                    {
                        gvPaymentRights.DataBind();
                        lblGridMsg.Text = "No Records Found !";
                        divGrid.Visible = true;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReason.SelectedValue == "1")
        {
            BindPaymentRightsDetailsBlock();
            //gvPaymentRights.Columns[7].Visible = false;
            //gvPaymentRights.Columns[8].Visible = false;
            //gvPaymentRights.Columns[5].Visible = true;
            //gvPaymentRights.Columns[6].Visible = true;
        }
        else
        {
            BindPaymentRightsDetailsUnBlock();
            //gvPaymentRights.Columns[7].Visible = true;
            //gvPaymentRights.Columns[8].Visible = true;
            //gvPaymentRights.Columns[5].Visible = false;
            //gvPaymentRights.Columns[6].Visible = false;
        }
    }

    protected void lbtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Sadmin/OnlinePaymentRights.aspx");
    }

    public class PaymentRightsDetails
    {
        public string QueryType { get; set; }
        public string UniqueId { get; set; }
        public string ApplicationType { get; set; }
        public string BranchType { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string BlockType { get; set; }
        public string CreatedBy { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string ActiveStatus { get; set; }
        public string UnBlockReason { get; set; }
        public string BlockReason { get; set; }
    }

    
}