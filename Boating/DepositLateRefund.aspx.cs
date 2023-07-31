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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class Boating_TripSheetSettelementV2 : System.Web.UI.Page
{
    IFormatProvider obj = new System.Globalization.CultureInfo("en-GB", true);
    DataTable dtAmount = new DataTable();
    DataTable dtTotal = new DataTable();
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

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
     
    protected void btnSearch_Click(object sender, EventArgs e)
     {
        try
        {
            divalert.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    QueryType = "GetSmsServiceLogDetailsForLateRefund",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    Input1 = txtReferenceNo.Text,
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {

                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                    if (dt.Rows.Count > 0)
                    {
                        string[] Booking;
                        string BookingDetails = dt.Rows[0]["BookingId"].ToString();
                        Booking = BookingDetails.Split(',');
                        DateTime date = DateTime.Now.AddDays(-1);
                     
                        string Fromdate = date.ToString("yyyy-MM-dd");

                        if (Booking.Count() > 0)
                        {
                            BoatTripCompletedWaitingForDepositRefund(Fromdate, Booking[1]);
                        }
                    }
                    else
                    {
                        GvBoatBookingTrip.Visible = false;
                        divalert.Visible = true;
                        lblalert.Text = "No Records Found";


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
    public void BoatTripCompletedWaitingForDepositRefund(string fromdate, string Pin)
    {
        try
        {
            divalert.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    QueryType = "GetLateRefundDetails",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    Input1 = fromdate,
                    Input2 = Pin,
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    ViewState["GvBoatBookingTrip"] = dt;

                    if (dt.Rows.Count > 0)
                    {
                        if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                        {
                            GvBoatBookingTrip.DataSource = dt;
                            GvBoatBookingTrip.DataBind();
                            divRefund.Visible = true;
                            GvBoatBookingTrip.Visible = true;
                        }
                        else
                        {
                            if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y")
                            {
                                if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")
                                {

                                    GvBoatBookingTrip.DataSource = dt;
                                    GvBoatBookingTrip.DataBind();
                                    GvBoatBookingTrip.Visible = true;
                                    divRefund.Visible = true;
                                }
                                else
                                {
                                    divRefund.Visible = true;
                                }

                            }
                            else if (Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")
                            {

                                GvBoatBookingTrip.DataSource = dt;
                                GvBoatBookingTrip.DataBind();
                                divRefund.Visible = true;
                                GvBoatBookingTrip.Visible = true;

                            }

                        }


                    }
                    else
                    {
                        clear();

                        GvBoatBookingTrip.Visible = false;
                        divalert.Visible = true;
                        lblalert.Text = "No Records Found";

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

    public void GetUserName(string fromDate)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = fromDate.Trim(),
                    ToDate = "",
                    QueryType = "GetServiceWiseBoatingUserName",
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlUserName.DataSource = dtExists;
                        ddlUserName.DataValueField = "UserId";
                        ddlUserName.DataTextField = "UserName";
                        ddlUserName.DataBind();

                    }
                    else
                    {
                        ddlUserName.DataBind();

                    }
                    ddlUserName.Items.Insert(0, new ListItem("Select User", "0"));

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void GetPaymentType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLPayType").Result;

                if (response.IsSuccessStatusCode)
                {
                    var PayType = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(PayType)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(PayType)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ddlPaymentType.DataSource = dt;
                            ddlPaymentType.DataValueField = "ConfigId";
                            ddlPaymentType.DataTextField = "ConfigName";
                            ddlPaymentType.DataBind();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Payment Type Details Not Found...!');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void lnkPinNum_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string bookingId = GvBoatBookingTrip.DataKeys[gvrow.RowIndex].Value.ToString();
        GetPaymentType();

        int a = gvrow.RowIndex;
        ViewState["RowIndex"] = a;

        Label lblBookingDate = (Label)gvrow.FindControl("lblBookingDate");
        ViewState["BookingDate"] = lblBookingDate.Text.Trim();
        DateTime date = DateTime.Parse(lblBookingDate.Text, obj);
       
        string fromdate = date.ToString("yyyy-MM-dd");
        GetUserName(fromdate);
        Label lblBookingId = (Label)gvrow.FindControl("lblBookingId");
        Label lblBoatRefNo = (Label)gvrow.FindControl("lblBoatReferenceNo");
        LinkButton lnkBookingPin = (LinkButton)gvrow.FindControl("lnkPinNum");
        Label lblBoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
        Label lblBoatSeaterId = (Label)gvrow.FindControl("lblBoatSeaterId");
        Label lblActualBoatId = (Label)gvrow.FindControl("lblActualBoatId");
        Label lblActualBoatNum = (Label)gvrow.FindControl("lblActualBoatNum");
        Label lblRowerId = (Label)gvrow.FindControl("lblRowerId");
        Label lblRowerNames = (Label)gvrow.FindControl("lblRowerNames");
        Label lblTripStartTime = (Label)gvrow.FindControl("lblTripStartTime");
        Label lblTripEndTime = (Label)gvrow.FindControl("lblTripEndTime");
        Label lblPremiumStatus = (Label)gvrow.FindControl("lblPreStatus");
        //NEW
        Label lblDeposit = (Label)gvrow.FindControl("lblDeposit");
        ViewState["Deposit"] = lblDeposit.Text.Trim();
        Label lblInitNetAmount = (Label)gvrow.FindControl("lblInitNetAmount");
        ViewState["BillAmount"] = lblInitNetAmount.Text.Trim();
        //NEW
        Label lblCustomerMobile = (Label)gvrow.FindControl("lblCustomerMobile");
        Label lblTravelDuration = (Label)gvrow.FindControl("lblTravelDuration");
        Label lblRefundDuration = (Label)gvrow.FindControl("lblRefundDuration");
        Label lblBookingDuration = (Label)gvrow.FindControl("lblBookingDuration");
        Label lblBalanceAmount = (Label)gvrow.FindControl("lblBalanceAmount");
        Label lblRefundPrint = (Label)gvrow.FindControl("lblRefundPrint");

        Label BoatType = (Label)gvrow.FindControl("lblBoatTypeName");
        ViewState["BoatType"] = BoatType.Text.Trim();

        Label BoatSeater = (Label)gvrow.FindControl("lblBoatSeaterName");
        ViewState["BoatSeater"] = BoatSeater.Text.Trim();
        ViewState["StartTime"] = lblTripStartTime.Text.Trim();
        ViewState["EndTime"] = lblTripEndTime.Text.Trim();
        ViewState["BookingDuration"] = lblBookingDuration.Text.Trim();

        lblBookingDateDisp.Text = lblBookingDate.Text.Trim();
        lblBookingIdDisp.Text = lblBookingId.Text.Trim();

        lblInitialBillAmountDisp.Text = lblInitNetAmount.Text.Trim();
        lblInitDepositDisp.Text = lblDeposit.Text.Trim();

        if (lblCustomerMobile.Text != "")
        {
            txtCustomerMobile.Text = lblCustomerMobile.Text;
            txtCustomerMobile.Enabled = false;
        }
        else
        {
            txtCustomerMobile.Text = "";
            txtCustomerMobile.Enabled = true;
        }

        ViewState["BoatRefNo"] = lblBoatRefNo.Text.Trim();
        ViewState["BookingPin"] = lnkBookingPin.Text.Trim();
        ViewState["PremiumStatus"] = lblPremiumStatus.Text.Trim();
        ViewState["BookingId"] = lblBookingId.Text.Trim();
        ViewState["TravelDuration"] = lblTravelDuration.Text.Trim();
        ViewState["RefundDuration"] = lblRefundDuration.Text.Trim();
        //ViewState["ReundPrintStatus"] = lblRefundPrint.Text.Trim();

        GetCharges(ViewState["BoatRefNo"].ToString().Trim(),
            ViewState["PremiumStatus"].ToString(), "GetRefund", ViewState["BookingId"].ToString(), ViewState["BookingPin"].ToString());

        txtCustomerMobile.Focus();
    }

    public void GetCharges(string ReferenceNo, string sPremiumStatus, string QueryType, string BookingId, string BookingPin)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;


                var body = new TripSheetSettlement()
                {
                    QueryType = QueryType,
                    BoatReferenceNo = ReferenceNo.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BoatHouseName = Session["BoatHouseName"].ToString(),
                    BookingPin = BookingPin.ToString().Trim(),
                    BookingId = BookingId.ToString().Trim(),
                    PremiumStatus = sPremiumStatus.Trim()
                };

                response = client.PostAsJsonAsync("TripSheet/Details", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Response)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Response)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            // divcustomerdetails.Visible = true;

                            if (QueryType == "GetRefund")
                            {
                                ViewState["Duration"] = dt.Rows[0]["BoatMinTime"].ToString().Trim();
                                ViewState["TotalDuration"] = dt.Rows[0]["MinutesTaken"].ToString().Trim();
                                lblTotaltime.Text = dt.Rows[0]["BoatMinTime"].ToString() + " Mins";
                                lblTimeDiff.Text = dt.Rows[0]["MinutesTaken"].ToString() + " Mins";

                                lblExtraTime.Text = dt.Rows[0]["ExtraMinutes"].ToString() + " Mins";

                                lblTotalDeduction.Text = dt.Rows[0]["TotalDeduction"].ToString();
                                ViewState["ExtraCharges"] = lblTotalDeduction.Text;
                                lblBoatDeduction.Text = dt.Rows[0]["BoatDeduction"].ToString();
                                lblRowerdeduction.Text = dt.Rows[0]["Rowerdeduction"].ToString();
                                lblTaxDeduction.Text = dt.Rows[0]["TaxDeduction"].ToString();

                                lblRefundAmt.Text = dt.Rows[0]["RefundAmount"].ToString();
                                ViewState["RefundAmount"] = dt.Rows[0]["RefundAmount"].ToString();
                                lblBalanceAmount.Text = dt.Rows[0]["CollectedBalance"].ToString();
                                lblNetRefund.Text = (Convert.ToDecimal(lblRefundAmt.Text) + Convert.ToDecimal(lblBalanceAmount.Text)).ToString();


                                // check refund amount is greater than zero or not.

                                if (Convert.ToDecimal(lblRefundAmt.Text) <= 0)
                                {
                                    divrepaymentType.Visible = false;
                                }
                                else
                                {
                                    divrepaymentType.Visible = true;
                                }

                                if (dt.Rows[0]["TimeExtension"].ToString() == "A")
                                {
                                    lblTimeExtension.Text = "Allowed";

                                    if (dt.Rows[0]["ExtensionSlabId"].ToString() == "0" || dt.Rows[0]["ExtensionSlabId"].ToString() == "")
                                    {
                                        if (dt.Rows[0]["ExtensionMsg"].ToString() != "success")
                                        {
                                            lblAlertSlabMsg.Text = dt.Rows[0]["ExtensionMsg"].ToString();
                                        }
                                        else
                                        {
                                            lblAlertSlabMsg.Text = string.Empty;
                                        }
                                    }
                                }
                                else if (dt.Rows[0]["TimeExtension"].ToString() == "N")
                                {
                                    lblTimeExtension.Text = "Not Allowed";
                                }
                                else
                                {
                                    lblTimeExtension.Text = "Not Defined";
                                }

                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "showModal();", true);
                                // MpeBillService.Show();
                            }
                        }
                        else
                        {
                            //  divcustomerdetails.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                        }
                    }
                    else
                    {
                        // divcustomerdetails.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    // divcustomerdetails.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void lbtnNewRefund_Click(object sender, EventArgs e)
    {
        Response.Redirect("TripSheetSettelement.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        RequiredFieldValidator2.Validate();
        if (Page.IsValid)
        {
            if (ddlUserName.SelectedValue != "0")
            {
                try
                {
                    string sPaymentMode = string.Empty;

                    if (Convert.ToDecimal(lblRefundAmt.Text) <= 0)
                    {
                        sPaymentMode = "0";
                    }
                    else
                    {
                        sPaymentMode = ddlPaymentType.SelectedValue.Trim();

                    }

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            QueryType = "PreviousDepositInsert",
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BookingId = lblBookingIdDisp.Text.Trim(),
                            BoatReferenceNo = ViewState["BoatRefNo"].ToString().Trim(),
                            BookingPin = ViewState["BookingPin"].ToString().Trim(),

                            ActualNetAmountExtn = lblTotalDeduction.Text.Trim(),
                            ActualBoatChargeExtn = lblBoatDeduction.Text.Trim(),
                            ActualRowerChargeExtn = lblRowerdeduction.Text.Trim(),
                            ActualTaxExtn = lblTaxDeduction.Text.Trim(),
                            ActualOfferAmountExtn = "0",

                            DepRefundAmount = lblRefundAmt.Text.Trim(),
                            RePaymentType = sPaymentMode.Trim(),
                            CreatedBy = ddlUserName.SelectedValue,
                            CustomerMobile = txtCustomerMobile.Text.Trim()
                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/Update", vTripSheetSettlement).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                GvBoatBookingTrip.Visible = false;
                                clear();
                                // lblStart.Text = ResponseMsg.ToString().Trim(); //SILLU 20 April 2022

                                if (ResponseMsg.ToString() == "Tripsheet Settlement Done Successfully" || ResponseMsg.ToString() == "Tripsheet Settlement Successfully !")
                                {
                                    lblStart.Text = "<span style='color:Green'> Deposit Refund Successful <span> <hr> <span style='color:DarkBlue'> Booking Id : " + lblBookingIdDisp.Text.Trim() + "<br> Booking Pin : " + ViewState["BookingPin"].ToString() + "<br> Amount : " + lblRefundAmt.Text.Trim() + " Was Refunded Successfully !!!  </span>";
                                }

                                MpeTrip.Show();

                                ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                if (txtCustomerMobile.Text.Trim() != "" && Convert.ToInt32(txtCustomerMobile.Text.Length) == 10)
                                {
                                    SendSMS("DepositRefund", txtCustomerMobile.Text.Trim(), lblBookingIdDisp.Text.Trim(), lblRefundAmt.Text.Trim());
                                }


                                if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "N")
                                {

                                    GvBoatBookingTrip.Visible = false;

                                }

                                txtCustomerMobile.Text = "";


                                string BookingId = ViewState["BookingId"].ToString().Trim();
                                string BookingPin = ViewState["BookingPin"].ToString().Trim();
                                string BoatType = ViewState["BoatType"].ToString().Trim();
                                string BoatSeater = ViewState["BoatSeater"].ToString().Trim();
                                string BookingDate = ViewState["BookingDate"].ToString().Trim();
                                string Duration = ViewState["BookingDuration"].ToString().Trim();
                                string StartTime = ViewState["StartTime"].ToString().Trim();
                                string EndTime = ViewState["EndTime"].ToString().Trim();
                                string TotalDuration = ViewState["TotalDuration"].ToString().Trim();
                                decimal rrRefundAmount = Convert.ToDecimal(ViewState["RefundAmount"].ToString().Trim());
                                decimal rrBillAmount = Convert.ToDecimal(ViewState["BillAmount"].ToString().Trim());
                                decimal rrDepositAmount = Convert.ToDecimal(ViewState["Deposit"].ToString().Trim());
                                decimal rrExtraCharge = Convert.ToDecimal(ViewState["ExtraCharges"].ToString().Trim());
                                string RePaymentType = sPaymentMode.Trim();

                                if (sPaymentMode == "1")
                                {
                                    lblRepaymentType.Text = "Cash";
                                }
                                else if (sPaymentMode == "2")
                                {
                                    lblRepaymentType.Text = "Card";
                                }
                                else if (sPaymentMode == "3")
                                {
                                    lblRepaymentType.Text = "Online";
                                }
                                else if (sPaymentMode == "4")
                                {
                                    lblRepaymentType.Text = "UPI";
                                }

                                //if (Convert.ToInt32(ViewState["TotalDuration"].ToString().Trim()) > Convert.ToInt32(ViewState["Duration"].ToString().Trim()))
                                //{
                                //    if (rrRefundAmount <= 0)
                                //    {
                                //        Response.Redirect("~/Boating/Print?rt=rRefss&bi=&BId=" + BookingId.Trim() + "&BPin=" + BookingPin.Trim() + "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + Duration.Trim() + "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BReBillAmo=" + rrBillAmount + "&BReDepositAmo=" + rrDepositAmount + "&BReExtraCharges=" + rrExtraCharge + "&BReAmt=" + rrRefundAmount + " ");
                                //    }
                                //    else
                                //    {
                                //        Response.Redirect("~/Boating/Print?rt=rRefundEligible&bi=&BId=" + BookingId.Trim() + "&BPin=" + BookingPin.Trim() + "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + Duration.Trim() + "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BReAmt=" + rrRefundAmount + "&BRePay=" + lblRepaymentType.Text + "&BReBillAmo=" + rrBillAmount + "&BReDepositAmo=" + rrDepositAmount + "&BReExtraCharges=" + rrExtraCharge + " ");
                                //    }
                                //}
                                //else
                                //{
                                //    // Incase Full amount, that time no need to print out. so blocked.
                                //    //Response.Redirect("~/Boating/Print?rt=rRefsCorrectDuration&bi=&BId=" + BookingId.Trim() + "&BPin=" + BookingPin.Trim() + "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + Duration.Trim() + "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BReAmt=" + rrRefundAmount + "&BRePay=" + lblRepaymentType.Text + "&BReBillAmo=" + rrBillAmount + "&BReDepositAmo=" + rrDepositAmount + "&BReExtraCharges=" + rrExtraCharge + " ");
                                //}
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select User');", true);

            }

        }

    }
    public void SendSMS(string ServiceType, string sMobileNo, string sTransactionNo, string sAmount)
    {
        try
        {
            if (sMobileNo.Trim() != "" && sMobileNo.Trim().Length == 10)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var OtpSendSms = new OtpSMS()
                    {
                        ServiceType = ServiceType.Trim(),
                        BookingId = sTransactionNo.ToString() + " of Booking Pin " + ViewState["BookingPin"].ToString().Trim(),
                        MobileNo = sMobileNo.ToString(),
                        MediaType = "DW",
                        BranchId = Session["BoatHouseId"].ToString().Trim(),
                        BranchName = Session["BoatHouseName"].ToString().Trim(),
                        Remarks = sAmount.ToString()
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("CM_SendSMSMsg", OtpSendSms).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase.ToString() + "');", true);
                    }
                }
            }
            else
            {
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ShowPopup(object sender, EventArgs e)
    {
        string title = "Greetings";
        string body = "Welcome to ASPSnippets.com";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
    }
    protected void ImgBtnPrint_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string bookingId = GvBoatBookingTrip.DataKeys[gvrow.RowIndex].Value.ToString();
        Label lblBookingId = (Label)gvrow.FindControl("lblBookingId");
        LinkButton lnkBookingPin = (LinkButton)gvrow.FindControl("lnkPinNum");
        UpdateRefundPrintStatus(lblBookingId.Text.Trim(), lnkBookingPin.Text.Trim());
        Response.Redirect("PrintBoat.aspx?rt=dpt&bi=&BId=" + lblBookingId.Text.Trim() + "&BPin=" + lnkBookingPin.Text.Trim() + " ");

    }

    private void UpdateRefundPrintStatus(string sBookingId, string sBookingPin)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var body = new CommonClass()
                {
                    QueryType = "RefundPinUpdate",
                    ServiceType = "",
                    Input1 = "Y",
                    Input2 = sBookingId,
                    Input3 = sBookingPin,
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", body).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Boat Booking Today...!');", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        clear();
    }

    public void clear()
    {
        txtReferenceNo.Text = string.Empty;

    }

    public class OtpSMS
    {
        public string ServiceType { get; set; }
        public string BookingId { get; set; }
        public string MobileNo { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string Remarks { get; set; }
        public string MediaType { get; set; }
    }

    public class CommonClass
    {
        public string BoatHouseId
        {
            get;
            set;
        }
        public string QueryType
        {
            get;
            set;
        }
        public string BoatTypeId
        {
            get;
            set;
        }
        public string BoatSeaterId
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
        public string Input1
        {
            get;
            set;
        }
        public string Input2
        {
            get;
            set;
        }
        public string Input3
        {
            get;
            set;
        }
        public string Input4
        {
            get;
            set;
        }
        public string Input5
        {
            get;
            set;
        }
        public string ServiceType
        {
            get;
            set;
        }
    }
    public class TripSheetSettlement
    {
        public string QueryType { get; set; }
        public string BoatReferenceNo { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BoatId { get; set; }
        public string BoatNum { get; set; }
        public string CreatedBy { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BHTripStartTime { get; set; }
        public string BHTripEndTime { get; set; }
        public string RowerId { get; set; }
        public string ActualNetAmount { get; set; }
        public string DepRefundAmount { get; set; }
        public string BookingId { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string PremiumStatus { get; set; }
        public string ActualOfferAmount { get; set; }
        public string ActualBoatCharge { get; set; }
        public string ActualRowerCharge { get; set; }
        public string BoatStatus { get; set; }
        public string ActualNetAmountExtn { get; set; }
        public string ActualBoatChargeExtn { get; set; }
        public string ActualRowerChargeExtn { get; set; }
        public string ActualTaxExtn { get; set; }
        public string ActualOfferAmountExtn { get; set; }
        public string BookingPin { get; set; }
        public string RePaymentType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string ServiceType { get; set; }
        public string CustomerMobile { get; set; }
        public string CountStart { get; set; }
        public string BookingIdORPin { get; set; }
        public string ActualBoatId { get; set; }
        public string SSUserBy { get; set; }
        public string SDUserBy { get; set; }
        public string BookingMedia { get; set; }
    }

    public class CashReport
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string CashflowTypes { get; set; }
        public string ToDate { get; set; }
        public string QueryType { get; set; }

        public string ServiceType { get; set; }
        public string BookingId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string RequestedBy { get; set; }

    }

}