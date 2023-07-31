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

public partial class Boating_PrintingRights : System.Web.UI.Page
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
                BindPrintingRightsDetails();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
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

                string sQueryType = string.Empty;
                string sUniqueId = string.Empty;

                if (btnSubmit.Text == "Submit")
                {
                    sQueryType = "Insert";
                    sUniqueId = "0";
                }
                else
                {
                    sQueryType = "Update";
                    sUniqueId = ViewState["UniqueId"].ToString();
                }

                var Gateway = new PrintRightsDetails()
                {
                    QueryType = sQueryType,
                    UniqueId = sUniqueId,
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BoatHouseName = Session["BoatHouseName"].ToString(),
                    OtherService = rblOtherService.SelectedValue.Trim(),
                    Restaurant = rblRestaurant.SelectedValue.Trim(),
                    AdditionalTicket = rblAdditionalTkt.SelectedValue.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("PrintingRights", Gateway).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClearInputs();
                        BindPrintingRightsDetails();
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

    public void BindPrintingRightsDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var GetCommonMaster = new PrintRightsDetails()
                {
                    QueryType = "GetPrintingRights",
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BookingId = "",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    FromDate = "",
                    ToDate = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", GetCommonMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvPrintingRights.DataSource = dtExists;
                        gvPrintingRights.DataBind();
                        lblGridMsg.Visible = false;
                        divSubmit.Visible = false;
                        divEntry.Visible = false;

                    }
                    else
                    {
                        gvPrintingRights.DataBind();
                        lblGridMsg.Visible = true;
                        lblGridMsg.Text = "No Records Found.";
                        divSubmit.Visible = true;
                        btnSubmit.Text = "Submit";
                        divEntry.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);

        }
    }

    public void ClearInputs()
    {
        rblOtherService.SelectedValue = "Both";
        rblAdditionalTkt.SelectedValue = "Both";
        rblRestaurant.SelectedValue = "Both";
        btnSubmit.Text = "Submit";
        BindPrintingRightsDetails();
    }

    protected void ImgBtnEdit_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

        rblAdditionalTkt.SelectedValue = gvPrintingRights.DataKeys[gvrow.RowIndex]["AdditionalTicket"].ToString().Trim();
        rblOtherService.SelectedValue = gvPrintingRights.DataKeys[gvrow.RowIndex]["OtherService"].ToString().Trim();
        rblRestaurant.SelectedValue = gvPrintingRights.DataKeys[gvrow.RowIndex]["Restaurant"].ToString().Trim();
        ViewState["UniqueId"] = gvPrintingRights.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
        divSubmit.Visible = true;
        btnSubmit.Text = "Update";
    }

    public class PrintRightsDetails
    {
        public string QueryType { get; set; }
        public string UniqueId { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string OtherService { get; set; }
        public string Restaurant { get; set; }
        public string AdditionalTicket { get; set; }
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
        public string BookingId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}