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

public partial class Master_BranchOperatingHistory : System.Web.UI.Page
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
                GetCorporateOffice();
                txtOpDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                //CHANGES

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
            ddlCorporateOff.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new BranchOpHistry()
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
                        ddlCorporateOff.DataSource = dtExists;
                        ddlCorporateOff.DataValueField = "CorpId";
                        ddlCorporateOff.DataTextField = "CorpName";
                        ddlCorporateOff.DataBind();
                    }
                    else
                    {
                        ddlCorporateOff.DataBind();
                    }
                    ddlCorporateOff.Items.Insert(0, "Select Corporate Office");
                    ddlCorporateOff.SelectedValue = "1";
                    GetBranch(ddlCorporateOff.SelectedValue);
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
    public void GetBranch(string sCorporateOff)
    {
        try
        {
            ddlBranchCode.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new BranchOpHistry()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlBranchCode",
                    CorpId = sCorporateOff.ToString().Trim(),
                    BranchCode = "",
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
                        ddlBranchCode.DataSource = dtExists;
                        ddlBranchCode.DataValueField = "BranchCode";
                        ddlBranchCode.DataTextField = "BranchName";
                        ddlBranchCode.DataBind();
                    }
                    else
                    {
                        ddlBranchCode.DataBind();
                    }
                    ddlBranchCode.Items.Insert(0, "Select Branch");
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
    public void GetBranchOpStatus(string sCorporateOff, string sBranchCode)
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchStatus = new BranchOpHistry()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlBranchOpStatus",
                    CorpId = sCorporateOff.ToString().Trim(),
                    Input1 = sBranchCode.ToString().Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchStatus).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlOpStatus.SelectedValue = dtExists.Rows[0]["OperatingStatus"].ToString();
                    }
                    else
                    {
                        ddlOpStatus.DataBind();
                    }

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


    protected void ddlBranchCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranchCode.SelectedIndex == 0)
        {

        }
        else
        {
            GetBranchOpStatus(ddlCorporateOff.SelectedValue, ddlBranchCode.SelectedValue);
        }

    }


    protected void lbtnBrnchMstr_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Common/BranchMaster.aspx");
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
                string sMSG = string.Empty;
                string qType = string.Empty;
                string sBranchId = string.Empty;
                string OperatingStatus = string.Empty;

                var BranchMaster = new FA_BranchOpHstry()
                {
                    QueryType = "Insert",
                    CorpId = ddlCorporateOff.SelectedValue.Trim(),
                    BranchCode = ddlBranchCode.SelectedValue.Trim(),
                    OperatingStatus = ddlOpStatus.SelectedValue.Trim(),
                    OperativeDate = txtOpDate.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CM_BranchOperatingHistory", BranchMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {

                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
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
    

    public void ClearInputs()
    {
        ddlBranchCode.SelectedIndex = 0;
        ddlOpStatus.SelectedIndex = 0;
        txtOpDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
    }


    public class FA_BranchOpHstry
    {
        public string QueryType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string OperatingStatus { get; set; }
        public string OperativeDate { get; set; }
        public string CreatedBy { get; set; }

    }
    public class BranchOpHistry
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }
 
}