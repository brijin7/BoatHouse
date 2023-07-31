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

public partial class Sadmin_OnlinePaymentRights : System.Web.UI.Page
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
                GetCorporateOffice();
                BindPaymentRightsDetails();
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public void GetCorporateOffice()
    {
        try
        {
            divBoatHouse.Visible = false;
            ddlCorpId.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new PaymentRightsDetails()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlCorporateOffice",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlCorpId.DataSource = dtExists;
                        ddlCorpId.DataValueField = "CorpId";
                        ddlCorpId.DataTextField = "CorpName";
                        ddlCorpId.DataBind();

                    }
                    else
                    {
                        ddlCorpId.DataBind();
                    }
                    ddlCorpId.Items.Insert(0, "Select Corporate Office");


                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    protected void ddlCorpId_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBoatHouseName();
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

                var Gateway = new PaymentRightsDetails()
                {
                    QueryType = "Insert",
                    UniqueId = "0",
                    ApplicationType = rblApplicationType.SelectedItem.Text.Trim(),
                    BranchType = ddlBranchType.SelectedItem.Text.Trim(),
                    BranchId = ddlBoatHouseId.SelectedValue.Trim(),
                    BranchName = ddlBoatHouseId.SelectedItem.Text.Trim(),
                    BlockType = rblBlockType.SelectedItem.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim(),
                    ActiveStatus = "A",
                    UnBlockReason = "",
                    BlockReason=txtBlockReason.Text
                };

                response = client.PostAsJsonAsync("CM_PaymentRights", Gateway).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClearInputs();
                        BindPaymentRightsDetails();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                    }
                    else
                    {
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        MPEBBpopup.Show();

        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

        ViewState["eUniqueId"] = gvPaymentRights.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
        ViewState["eApplicationType"] = gvPaymentRights.DataKeys[gvrow.RowIndex]["ApplicationType"].ToString().Trim();
        ViewState["eBranchType"] = gvPaymentRights.DataKeys[gvrow.RowIndex]["BranchType"].ToString().Trim();
        ViewState["eBranchId"] = gvPaymentRights.DataKeys[gvrow.RowIndex]["BranchId"].ToString().Trim();
        ViewState["eBranchName"] = gvPaymentRights.DataKeys[gvrow.RowIndex]["BranchName"].ToString().Trim();
        ViewState["eBlockType"] = gvPaymentRights.DataKeys[gvrow.RowIndex]["BlockType"].ToString().Trim();
    }

    public void BindBoatHouseName()
    {
        try
        {
            ddlBoatHouseId.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("ddlBoatHouse?CorpId=" + ddlCorpId.SelectedValue + "").Result;

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
                            divBoatHouse.Visible = true;
                            ddlBoatHouseId.DataSource = dt;
                            ddlBoatHouseId.DataValueField = "BoatHouseId";
                            ddlBoatHouseId.DataTextField = "BoatHouseName";
                            ddlBoatHouseId.DataBind();
                        }
                        else
                        {
                            divBoatHouse.Visible = false;
                            ddlBoatHouseId.DataBind();
                        }

                        ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                    }
                    else
                    {
                        ddlBoatHouseId.DataBind();
                        ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                    }
                }
                else
                {
                    ddlBoatHouseId.DataBind();
                    ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindPaymentRightsDetails()
    {
        try
        {
            divGrid.Visible = false;
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

    public void ClearInputs()
    {
        rblApplicationType.SelectedIndex = 0;
        ddlBranchType.SelectedIndex = -1;
        ddlBoatHouseId.SelectedIndex = -1;
        rblBlockType.SelectedIndex = 0;
        txtBlockReason.Text = string.Empty;
        btnSubmit.Text = "Submit";
    }

    protected void ImgCloseBB_Click(object sender, ImageClickEventArgs e)
    {
        MPEBBpopup.Hide();
    }

    protected void btnSubmitReason_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Gateway = new PaymentRightsDetails()
                {
                    QueryType = "Delete",

                    UniqueId = ViewState["eUniqueId"].ToString(),
                    ApplicationType = ViewState["eApplicationType"].ToString(),
                    BranchType = ViewState["eBranchType"].ToString(),
                    BranchId = ViewState["eBranchId"].ToString(),
                    BranchName = ViewState["eBranchName"].ToString(),
                    BlockType = ViewState["eBlockType"].ToString(),
                    CreatedBy = Session["UserId"].ToString(),
                    UnBlockReason = txtUnBlockReason.Text,
                    ActiveStatus = "D",
                    BlockReason = ""
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("CM_PaymentRights", Gateway).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindPaymentRightsDetails();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        txtUnBlockReason.Text = string.Empty;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        txtUnBlockReason.Text = string.Empty;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                    txtUnBlockReason.Text = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void lbtnBlockReason_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Reports/RptOnlinePaymentBlockRights.aspx");
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