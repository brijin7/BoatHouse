using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_PrintExtraCharge : System.Web.UI.Page
{
    public decimal lblChargePerItem, lblNoOfItems, lblItemCharge, lblNetAmount = 0;
    IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);
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
            ViewState["rte"] = Request.QueryString["rte"].ToString();
            string bi = Request.QueryString["bi"].ToString();

            //BindPrintingRightsDetails();

            if (!IsPostBack)
            {
                if (Request.HttpMethod != "GET")
                {
                    AntiForgery.Validate(ViewState["__AntiForgeryCookie"] as string, Request.Form["__RequestVerificationToken"]);
                }
                if (ViewState["rte"].ToString() == "rRefsse")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();
                    string sBoatType = Request.QueryString["BTId"].ToString();
                    string sBoatSeater = Request.QueryString["BSId"].ToString();
                    string sBookingDate = Request.QueryString["BDate"].ToString();
                    string sStartTime = Request.QueryString["BSTime"].ToString();
                    string sEndTime = Request.QueryString["BETime"].ToString();
                    string sBoatDuration = Request.QueryString["BDur"].ToString();
                    string sTotalDuration = Request.QueryString["BTDur"].ToString();
                    string ExtraBoatCharge = Request.QueryString["BEBC"].ToString();
                    string ExtraRowerCharge = Request.QueryString["BERC"].ToString();
                    string ExtraNetAmount = Request.QueryString["BENC"].ToString();
                    string Tax = Request.QueryString["Tax"].ToString();
                    string ActualNetAmount = Request.QueryString["ANA"].ToString();
                    BindExtracharge(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, ExtraBoatCharge, ExtraRowerCharge, ExtraNetAmount, Tax, ActualNetAmount);


                    divRefund.Visible = true;

                }



                if (ViewState["rte"].ToString() == "rRefssse")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();
                    string sBoatType = Request.QueryString["BTId"].ToString();
                    string sBoatSeater = Request.QueryString["BSId"].ToString();
                    string sBookingDate = Request.QueryString["BDate"].ToString();
                    string sStartTime = Request.QueryString["BSTime"].ToString();
                    string sEndTime = Request.QueryString["BETime"].ToString();
                    string sBoatDuration = Request.QueryString["BDur"].ToString();
                    string sTotalDuration = Request.QueryString["BTDur"].ToString();
                    string ExtraBoatCharge = Request.QueryString["BEBC"].ToString();
                    string ExtraRowerCharge = Request.QueryString["BERC"].ToString();
                    string ExtraNetAmount = Request.QueryString["BENC"].ToString();
                    string Tax = Request.QueryString["Tax"].ToString();
                    string ActualNetAmount = Request.QueryString["ANA"].ToString();
                    BindExtracharge(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, ExtraBoatCharge, ExtraRowerCharge, ExtraNetAmount, Tax, ActualNetAmount);

                    divRefund.Visible = true;

                }
                if (ViewState["rte"].ToString() == "rRefssea")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();
                    string sBoatType = Request.QueryString["BTId"].ToString();
                    string sBoatSeater = Request.QueryString["BSId"].ToString();
                    string sBookingDate = Request.QueryString["BDate"].ToString();
                    string sStartTime = Request.QueryString["BSTime"].ToString();
                    string sEndTime = Request.QueryString["BETime"].ToString();
                    string sBoatDuration = Request.QueryString["BDur"].ToString();
                    string sTotalDuration = Request.QueryString["BTDur"].ToString();
                    string ExtraBoatCharge = Request.QueryString["BEBC"].ToString();
                    string ExtraRowerCharge = Request.QueryString["BERC"].ToString();
                    string ExtraNetAmount = Request.QueryString["BENC"].ToString();
                    string Tax = Request.QueryString["Tax"].ToString();
                    string ActualNetAmount = Request.QueryString["ANA"].ToString();
                    string BoatDeposit = Request.QueryString["BD"].ToString();
                    string DepRefundAmount = Request.QueryString["DRA"].ToString();
                    tblExtraDetailsAllowed.Visible = true;
                    BindExtrachargeAllowed(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, ExtraBoatCharge, ExtraRowerCharge, ExtraNetAmount, Tax, ActualNetAmount, BoatDeposit, DepRefundAmount);

                    divRefund.Visible = true;

                }

                if (ViewState["rte"].ToString() == "rRefsssea")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();
                    string sBoatType = Request.QueryString["BTId"].ToString();
                    string sBoatSeater = Request.QueryString["BSId"].ToString();
                    string sBookingDate = Request.QueryString["BDate"].ToString();
                    string sStartTime = Request.QueryString["BSTime"].ToString();
                    string sEndTime = Request.QueryString["BETime"].ToString();
                    string sBoatDuration = Request.QueryString["BDur"].ToString();
                    string sTotalDuration = Request.QueryString["BTDur"].ToString();
                    string ExtraBoatCharge = Request.QueryString["BEBC"].ToString();
                    string ExtraRowerCharge = Request.QueryString["BERC"].ToString();
                    string ExtraNetAmount = Request.QueryString["BENC"].ToString();
                    string Tax = Request.QueryString["Tax"].ToString();
                    string ActualNetAmount = Request.QueryString["ANA"].ToString();
                    string BoatDeposit = Request.QueryString["BD"].ToString();
                    string DepRefundAmount = Request.QueryString["DRA"].ToString();
                    tblExtraDetailsAllowed.Visible = true;
                    BindExtrachargeAllowed(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, ExtraBoatCharge, ExtraRowerCharge, ExtraNetAmount, Tax, ActualNetAmount, BoatDeposit, DepRefundAmount);

                    divRefund.Visible = true;

                }

                if (ViewState["rte"].ToString() == "rRefssePin")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();
                    string sBoatType = Request.QueryString["BTId"].ToString();
                    string sBoatSeater = Request.QueryString["BSId"].ToString();
                    string sBookingDate = Request.QueryString["BDate"].ToString();
                    string sStartTime = Request.QueryString["BSTime"].ToString();
                    string sEndTime = Request.QueryString["BETime"].ToString();
                    string sBoatDuration = Request.QueryString["BDur"].ToString();
                    string sTotalDuration = Request.QueryString["BTDur"].ToString();
                    string ExtraBoatCharge = Request.QueryString["BEBC"].ToString();
                    string ExtraRowerCharge = Request.QueryString["BERC"].ToString();
                    string ExtraNetAmount = Request.QueryString["BENC"].ToString();
                    string Tax = Request.QueryString["Tax"].ToString();
                    string ActualNetAmount = Request.QueryString["ANA"].ToString();
                    BindExtracharge(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, ExtraBoatCharge, ExtraRowerCharge, ExtraNetAmount, Tax, ActualNetAmount);

                    divRefund.Visible = true;
                }

                if (ViewState["rte"].ToString() == "rRefsseaPin")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();
                    string sBoatType = Request.QueryString["BTId"].ToString();
                    string sBoatSeater = Request.QueryString["BSId"].ToString();
                    string sBookingDate = Request.QueryString["BDate"].ToString();
                    string sStartTime = Request.QueryString["BSTime"].ToString();
                    string sEndTime = Request.QueryString["BETime"].ToString();
                    string sBoatDuration = Request.QueryString["BDur"].ToString();
                    string sTotalDuration = Request.QueryString["BTDur"].ToString();
                    string ExtraBoatCharge = Request.QueryString["BEBC"].ToString();
                    string ExtraRowerCharge = Request.QueryString["BERC"].ToString();
                    string ExtraNetAmount = Request.QueryString["BENC"].ToString();
                    string Tax = Request.QueryString["Tax"].ToString();
                    string ActualNetAmount = Request.QueryString["ANA"].ToString();
                    string BoatDeposit = Request.QueryString["BD"].ToString();
                    string DepRefundAmount = Request.QueryString["DRA"].ToString();
                    tblExtraDetailsAllowed.Visible = true;
                    BindExtrachargeAllowed(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, ExtraBoatCharge, ExtraRowerCharge, ExtraNetAmount, Tax, ActualNetAmount, BoatDeposit, DepRefundAmount);

                    divRefund.Visible = true;
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }


    public void BindPrintingRightsDetails()
    {
        try
        {
            ViewState["PrintOS"] = "Both";
            ViewState["PrintRS"] = "Both";
            ViewState["PrintAT"] = "Both";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var GetCommonMaster = new CashReport()
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
                        ViewState["PrintOS"] = dtExists.Rows[0]["OtherService"].ToString().Trim();
                        ViewState["PrintRS"] = dtExists.Rows[0]["Restaurant"].ToString().Trim();
                        ViewState["PrintAT"] = dtExists.Rows[0]["AdditionalTicket"].ToString().Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);

        }
    }

    public void BindExtracharge(string sBookingId, string sBookingPin, string sBoatType, string sBoatSeater, string sBookingDate, string sStartTime, string sEndTime, string sBoatDuration, string sTotalDuration, string ExtraBoatCharge, string ExtraRowerCharge, string ExtraNetAmount, string Tax, string ActualNetAmount)
    {
        try
        {
            lblRefundBoatHouseName.Text = Session["BoatHouseName"].ToString();

            // lblRefundHeader.Text = "Extension Boat Charges";
            lblRefundHeader.Text = "Extension Charge";

            lblRefundBookingId.Text = sBookingId;
            lblRefundBookingPin.Text = sBookingPin;
            lblRefundBoatType.Text = sBoatType;
            lblRefundBoatSeater.Text = sBoatSeater;
            lblRefundDate.Text = sBookingDate;
            lblRefundDuration.Text = sBoatDuration + " Mins";
            //lblRefundNotEligible.Text = "Refund Not Eligible";
            lblRefundStartTime.Text = sStartTime;
            lblRefundEndTime.Text = sEndTime;
            lblRefundTotalDuration.Text = sTotalDuration + " Mins ";

            lblExtraBoatCharge.Text = Convert.ToDecimal(ExtraBoatCharge).ToString("0.00");
            lblExtraRowerCharge.Text = Convert.ToDecimal(ExtraRowerCharge).ToString("0.00");
            lblExtraNetAmount.Text = Convert.ToDecimal(ExtraNetAmount).ToString("0.00");
            lblTax.Text = Convert.ToDecimal(Tax).ToString("0.00");
            //lblActualNet.Text = ActualNetAmount;
            tblExtraDetailsAllowed.Visible = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }


    public void BindExtrachargeAllowed(string sBookingId, string sBookingPin, string sBoatType, string sBoatSeater, string sBookingDate, string sStartTime, string sEndTime, string sBoatDuration, string sTotalDuration, string ExtraBoatCharge, string ExtraRowerCharge, string ExtraNetAmount, string Tax, string ActualNetAmount, string BoatDeposit, string DepRefundAmount)
    {
        try
        {

            lblRefundBoatHouseName.Text = Session["BoatHouseName"].ToString();

            lblRefundHeader.Text = "Extension Charge & Refund";

            lblRefundBookingId.Text = sBookingId;
            lblRefundBookingPin.Text = sBookingPin;
            lblRefundBoatType.Text = sBoatType;
            lblRefundBoatSeater.Text = sBoatSeater;
            lblRefundDate.Text = sBookingDate;
            lblRefundDuration.Text = sBoatDuration + " Mins";

            lblRefundStartTime.Text = sStartTime;
            lblRefundEndTime.Text = sEndTime;
            lblRefundTotalDuration.Text = sTotalDuration + " Mins ";

            lblExtraBoatCharge.Text = Convert.ToDecimal(ExtraBoatCharge).ToString("0.00");
            lblExtraRowerCharge.Text = Convert.ToDecimal(ExtraRowerCharge).ToString("0.00");
            lblExtraNetAmount.Text = Convert.ToDecimal(ExtraNetAmount).ToString("0.00");
            lblTax.Text = Convert.ToDecimal(Tax).ToString("0.00");
            lblBoatDeposit.Text = Convert.ToDecimal(BoatDeposit).ToString("0.00");
            lblExtensionCharge.Text = Convert.ToDecimal(ExtraNetAmount).ToString("0.00");
            lblRefundAmount.Text = Convert.ToDecimal(DepRefundAmount).ToString("0.00");

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {

        if (ViewState["rte"].ToString() == "rRefsse")
        {
            Response.Redirect("~/Boating/TripSheetWeb.aspx");
        }
        else if (ViewState["rte"].ToString() == "rRefssse")
        {
            Response.Redirect("~/ScanTripSheetWeb.aspx");
        }
        else if (ViewState["rte"].ToString() == "rRefssea")
        {
            Response.Redirect("~/Boating/TripSheetWeb.aspx");
        }
        else if (ViewState["rte"].ToString() == "rRefsssea")
        {
            Response.Redirect("~/ScanTripSheetWeb.aspx");
        }
        else if (ViewState["rte"].ToString() == "rRefssePin")
        {
            Response.Redirect("~/Boating/TripSheetWeb_Single.aspx");
        }
        else if (ViewState["rte"].ToString() == "rRefsseaPin")
        {
            Response.Redirect("~/Boating/TripSheetWeb_Single.aspx");
        }
        else
        {
            return;
        }
    }


    public class CashReport
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string CashflowTypes { get; set; }
        public string ToDate { get; set; }
        public string QueryType { get; set; }
        public string BooKingId { get; set; }
        public string ServiceType { get; set; }
        public string BookingId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }

}