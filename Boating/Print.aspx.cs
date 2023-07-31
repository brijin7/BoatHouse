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

public partial class Boating_Print : System.Web.UI.Page
{
    public decimal lblChargePerItem, lblNoOfItems, lblItemCharge, lblTaxAmount, lblNetAmount = 0;
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
            if (!IsPostBack)
            {
                if (Request.HttpMethod != "GET")
                {
                    AntiForgery.Validate(ViewState["__AntiForgeryCookie"] as string, Request.Form["__RequestVerificationToken"]);
                }
                ViewState["rt"] = Request.QueryString["rt"].ToString();
                string bi = Request.QueryString["bi"].ToString();


                BindPrintingRightsDetails();

                tblRefundAmount.Visible = false;

                if (ViewState["rt"].ToString() == "b" || ViewState["rt"].ToString() == "bb" || ViewState["rt"].ToString() == "rb") // Print -- Boating Services
                {
                    GetBoatTickets(bi);
                    GetBoatTicketsSummaryReceipt(bi);
                    GetOtherTickets(bi);

                    divBoat.Visible = true;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditional.Visible = false;
                }
                else if (ViewState["rt"].ToString() == "o" || ViewState["rt"].ToString() == "bo" || ViewState["rt"].ToString() == "ro") // Print -- Other Services
                {
                    OtrGetOtherTickets(bi);
                    GetOtherSummaryReceipt(bi);
                    OtherTicketInstructions(bi);

                    if (ViewState["PrintOS"].ToString().Trim() == "Single")
                    {
                        divOtherServiceSingle.Visible = true;
                        divOtherServiceAbstract.Visible = false;
                    }

                    if (ViewState["PrintOS"].ToString().Trim() == "Abstract")
                    {
                        divOtherServiceSingle.Visible = false;
                        divOtherServiceAbstract.Visible = true;
                    }

                    divBoat.Visible = false;
                    divOthr.Visible = true;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditional.Visible = false;
                }
                else if (ViewState["rt"].ToString() == "at" || ViewState["rt"].ToString() == "rat") // Print -- Additional Ticket
                {
                    GvBindAdditionalTicket(bi);
                    GetAdditionalTicketSummaryReceipt(bi);

                    if (ViewState["PrintAT"].ToString().Trim() == "Single")
                    {
                        divAddtTicketSingle.Visible = true;
                        divAddtTicketAbstract.Visible = false;
                    }

                    if (ViewState["PrintAT"].ToString().Trim() == "Abstract")
                    {
                        divAddtTicketSingle.Visible = false;
                        divAddtTicketAbstract.Visible = true;
                    }

                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditional.Visible = true;
                }
                else if (ViewState["rt"].ToString() == "r" || ViewState["rt"].ToString() == "BulkRes" || ViewState["rt"].ToString() == "rr") // Print -- Restaurant Service
                {

                    CheckQRcodeGeneration();//newly Changes On 29-04-21
                    GvBindRestaurant(bi);
                    GetRestaurantSummaryReceipt(bi);
                    RestaurantTicketInstructions();

                    if (ViewState["PrintRS"].ToString().Trim() == "Single")
                    {
                        divRestaurantSingle.Visible = true;
                        divRestaurantAbstract.Visible = false;
                    }

                    if (ViewState["PrintRS"].ToString().Trim() == "Abstract")
                    {
                        divRestaurantSingle.Visible = false;
                        divRestaurantAbstract.Visible = true;
                    }

                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = true;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditional.Visible = false;
                }
                else if (ViewState["rt"].ToString() == "ross" || ViewState["rt"].ToString() == "rrss") //report other service summary OR report restaurant service summary
                {
                    string sUserId = Request.QueryString["UId"].ToString();
                    string fDat = Request.QueryString["fDat"].ToString();

                    string bTypeId = "0";
                    string CatTypeId = "0";
                    string PayTypeId = "0";

                    BindEntranceSummarybyUser(fDat, bTypeId, CatTypeId, PayTypeId, sUserId.Trim());
                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = true;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditional.Visible = false;

                    buildDenomination();
                }
                else if (ViewState["rt"].ToString() == "rbss") // Boating  Service Summary -- Report /
                {
                    string fDat = Request.QueryString["fDat"].ToString();
                    string sUserId = Request.QueryString["UId"].ToString();
                    string sBoatTypeId = Request.QueryString["BTI"].ToString();
                    string sBoatType = Request.QueryString["BT"].ToString();

                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = true;
                    divRefundCashRpt.Visible = false;
                    divAdditional.Visible = false;

                    BoatWiseCollectionSummary(fDat, fDat, sUserId.Trim(), sBoatTypeId.Trim(), sBoatType.Trim());
                    BindBoatServiceWise(fDat, fDat, sUserId.Trim(), sBoatTypeId.Trim(), sBoatType.Trim());
                    buildDenomination();
                }
                else if (ViewState["rt"].ToString() == "recr") // Refund Cash -- Report
                {
                    string fDat = Request.QueryString["fDat"].ToString();
                    string sUserId = Request.QueryString["UId"].ToString();

                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = true;
                    divAdditional.Visible = false;

                    GetRefundCashFromReport(fDat, sUserId);
                    GetRefundCashPaymentReport(fDat, sUserId);
                }
                else if (ViewState["rt"].ToString() == "rATss") // Additional Ticket Summary -- Report
                {
                    string sUserId = Request.QueryString["UId"].ToString();
                    string fDat = Request.QueryString["fDat"].ToString();

                    string bTypeId = "0";
                    string CatTypeId = "0";
                    string PayTypeId = "0";

                    BindEntranceAddSummarybyUser(fDat, bTypeId, CatTypeId, PayTypeId, sUserId.Trim());
                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditionalTicket.Visible = true;

                    buildDenomination();
                }
                else if (ViewState["rt"].ToString() == "rRefss") // Refund Not Eligible -- Report
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
                    string sBillAmount = Request.QueryString["BReBillAmo"].ToString();
                    string sDepositAmount = Request.QueryString["BReDepositAmo"].ToString();
                    string sExtraCharges = Request.QueryString["BReExtraCharges"].ToString();
                    string sRefundAmount = Request.QueryString["BReAmt"].ToString();



                    BindRefundNotEligible(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, sBillAmount, sDepositAmount, sExtraCharges, sRefundAmount);

                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditionalTicket.Visible = false;
                    divRefund.Visible = true;

                }
                else if (ViewState["rt"].ToString() == "rRefundEligible") // Refund Eligible
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
                    string sRefundAmount = Request.QueryString["BReAmt"].ToString();
                    string sPayment = Request.QueryString["BRePay"].ToString();
                    string sBillAmount = Request.QueryString["BReBillAmo"].ToString();
                    string sDepositAmount = Request.QueryString["BReDepositAmo"].ToString();
                    string sExtraCharge = Request.QueryString["BReExtraCharges"].ToString();



                    BindRefundAmount(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, sRefundAmount, sPayment, sBillAmount, sDepositAmount, sExtraCharge);

                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditionalTicket.Visible = false;
                    divRefund.Visible = true;
                }

                else if (ViewState["rt"].ToString() == "rRefsCorrectDuration") // Correct Duration
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
                    string sRefundAmount = Request.QueryString["BReAmt"].ToString();
                    string sPayment = Request.QueryString["BRePay"].ToString();
                    string sBillAmount = Request.QueryString["BReBillAmo"].ToString();
                    string sDepositAmount = Request.QueryString["BReDepositAmo"].ToString();
                    string sExtraCharge = Request.QueryString["BReExtraCharges"].ToString();

                    BindCorrectAmount(sBookingId, sBookingPin, sBoatType, sBoatSeater, sBookingDate, sStartTime, sEndTime, sBoatDuration, sTotalDuration, sRefundAmount, sPayment, sBillAmount, sDepositAmount, sExtraCharge);


                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditionalTicket.Visible = false;
                    divRefund.Visible = true;

                }
                else if (ViewState["rt"].ToString() == "rChangeBoat")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();

                    Session["ChangeBookingId"] = sBookingId.Trim();
                    BindChangeBoatDetails(sBookingId, sBookingPin);

                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditionalTicket.Visible = false;
                    divRefund.Visible = false;
                    divChangeBoat.Visible = true;
                }
                else if (ViewState["rt"].ToString() == "rRefCancel")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();
                    string sPaymentType = Request.QueryString["CPayType"].ToString();
                    string sBoatCharge = Request.QueryString["CBtCharge"].ToString();
                    string sBookingDate = Request.QueryString["BDate"].ToString();
                    string sDepositCharge = Request.QueryString["CDepAmnt"].ToString();
                    string sTotalCharge = Request.QueryString["CTotCharge"].ToString();
                    string sCancelCharge = Request.QueryString["CnclCharge"].ToString();
                    string sRefundAmount = Request.QueryString["CReAmt"].ToString();


                    BindRefundCancelAmount(sBookingId, sBookingPin, sBookingDate, sPaymentType, sBoatCharge, sDepositCharge, sTotalCharge, sCancelCharge, sRefundAmount);

                    divBoat.Visible = false;
                    divOthr.Visible = false;
                    divRestaurant.Visible = false;
                    divEntranceSummary.Visible = false;
                    divBoatingSummary.Visible = false;
                    divRefundCashRpt.Visible = false;
                    divAdditionalTicket.Visible = false;
                    divRefund.Visible = false;
                    divRefCancel.Visible = true;
                }

                else
                {

                }
            }

            lblPrintedByName.Text = Session["PrintUserName"].ToString();
            lblPrintDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/");
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (ViewState["rt"].ToString() == "b")
        {
            Response.Redirect("BoatBookingFinal.aspx");
        }
        else if (ViewState["rt"].ToString() == "bb")
        {
            Response.Redirect("BulkBoatBooking.aspx");
        }
        else if (ViewState["rt"].ToString() == "rb")
        {
            Response.Redirect("~/Reports/RptBoatBooking.aspx");
        }
        else if (ViewState["rt"].ToString() == "o")
        {
            Response.Redirect("BookingOtherServices.aspx");
        }
        else if (ViewState["rt"].ToString() == "at")
        {
            Response.Redirect("AdditionalTicket.aspx");
        }
        else if (ViewState["rt"].ToString() == "bo")
        {
            Response.Redirect("BulkOtherServices.aspx");
        }
        else if (ViewState["rt"].ToString() == "rat")
        {
            Response.Redirect("~/Reports/RptPrintAdditionalTicket.aspx");
        }
        else if (ViewState["rt"].ToString() == "ro")
        {
            Response.Redirect("~/Reports/RptBookingOtherSevices.aspx");
        }
        else if (ViewState["rt"].ToString() == "r")
        {
            Response.Redirect("~/Restaurants/BookingRestaurantServices.aspx");
        }
        else if (ViewState["rt"].ToString() == "BulkRes")
        {
            Response.Redirect("~/Restaurants/BulkRestaurantService.aspx");
        }
        else if (ViewState["rt"].ToString() == "rr")
        {
            Response.Redirect("~/Reports/RptPrintRestaurantServices.aspx");
        }
        else if (ViewState["rt"].ToString() == "ross" || ViewState["rt"].ToString() == "rrss" || ViewState["rt"].ToString() == "rbss")
        {
            Response.Redirect("~/Reports/RptServiceWiseCollection.aspx");
        }
        else if (ViewState["rt"].ToString() == "recr")
        {
            Response.Redirect("~/Reports/RptServiceWiseCollection.aspx");
        }
        else if (ViewState["rt"].ToString() == "rATss")
        {
            Response.Redirect("~/Reports/RptServiceWiseCollection.aspx");
        }
        else if (ViewState["rt"].ToString() == "rRefss" || ViewState["rt"].ToString() == "rRefundEligible" || ViewState["rt"].ToString() == "rRefsCorrectDuration")
        {
            Response.Redirect("~/Boating/TripSheetSettelement.aspx");
        }
        else if (ViewState["rt"].ToString() == "rChangeBoat")
        {
            Response.Redirect("~/Boating/ChangeBoatDetails.aspx?rt=" + Session["ChangeBookingId"].ToString() + " ");
        }
        else if (ViewState["rt"].ToString() == "rRefCancel")
        {
            Response.Redirect("~/Boating/BoatCancellation.aspx");
        }
        else
        {
            return;
        }
    }

    //Boatbooking with Others service

    private void GetBoatTickets(string sBookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = sBookingId.ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatBookedTicket", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var ticktList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(ticktList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(ticktList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlistTicket.DataSource = dt;
                            dtlistTicket.DataBind();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Boat Booking Today...!');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    private void GetBoatTicketsSummaryReceipt(string sBookingId)
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new Ticket()
                {
                    QueryType = "BoatPrintTicketBulkReceipts",
                    ServiceType = "",
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BookingId = sBookingId.ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    UserId = "0"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonAPIMethod", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        DLReceipt.DataSource = dtExists;
                        DLReceipt.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Boat Booking Found !');", true);
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

    private void GetOtherTickets(string sBookingId)
    {
        try
        {
            dtlistTicketOther.Visible = false;

            //MpeBillService.Show();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = sBookingId.ToString(),
                    BookingType = "B"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatOtherTicket", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Oticket = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Oticket)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Oticket)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlistTicketOther.DataSource = dt;
                            dtlistTicketOther.DataBind();
                            dtlistTicketOther.Visible = true;
                        }
                        else
                        {
                            dtlistTicketOther.Visible = false;
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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

    protected void dtlistTicket_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Label BookingId = (Label)e.Item.FindControl("lblBookingId");
                Label RowerCharge = (Label)e.Item.FindControl("lblBillRowerCharge");
                Label lblBookingPin = (Label)e.Item.FindControl("lblBookingPin");
                Label lblBoatReferenceNo = (Label)e.Item.FindControl("lblBoatReferenceNo");

                Control divrower = e.Item.FindControl("divrower") as Control;
                Control divBPass = e.Item.FindControl("divBPass") as Control;
                Control divBPass1 = e.Item.FindControl("divBPass1") as Control;

                Label lblActualBoatNum = (Label)e.Item.FindControl("lblActualBoatNum");
                Label lblExpectedTime = (Label)e.Item.FindControl("lblExpectedTime");

                Label lblCheckDate = (Label)e.Item.FindControl("lblCheckDate");
                Label lblTripEndTime = (Label)e.Item.FindControl("lblTripEndTime");

                if (DateTime.Parse(lblCheckDate.Text, objEnglishDate).ToString() == DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"), objEnglishDate).ToString())
                {
                    //if (lblActualBoatNum.Text != "" && lblExpectedTime.Text != "")
                    //{
                    if (lblTripEndTime.Text != "")
                    {
                        divrower.Visible = false;
                        divBPass.Visible = false;
                        divBPass1.Visible = false;
                    }
                    else
                    {
                        if (Convert.ToDecimal(RowerCharge.Text) > 0)
                        {
                            divrower.Visible = true;
                        }
                        else
                        {
                            divrower.Visible = false;
                        }

                        divBPass.Visible = true;
                        divBPass1.Visible = true;
                    }
                    //}
                    //else
                    //{
                    //    divrower.Visible = false;
                    //    divBPass.Visible = false;
                    //    divBPass1.Visible = false;
                    //}
                }
                else
                {
                    dtlistTicket.Visible = false;
                    divrower.Visible = false;
                    divBPass.Visible = false;
                    divBPass1.Visible = false;
                }

                Image ImageData = (Image)e.Item.FindControl("imgQRBRoCopy");
                Image ImageData1 = (Image)e.Item.FindControl("imgQRBBoCopy");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var QRd = new QRBoat()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        BookingId = BookingId.Text.Trim(),
                        Pin = lblBookingPin.Text.Trim(),
                        BookingRef = lblBoatReferenceNo.Text.Trim()
                    };


                    HttpResponseMessage response = client.PostAsJsonAsync("PassQR", QRd).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                        if (Status == "Success.")
                        {
                            ImageData.ImageUrl = ResponseMsg;
                            ImageData1.ImageUrl = ResponseMsg;
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
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void dtlistTicketOther_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label BookingIds = (Label)e.Item.FindControl("lblBillOBookingId");
            Label ServiceIds = (Label)e.Item.FindControl("lblBillOServiceId");

            Image ImageData = (Image)e.Item.FindControl("imgOtherServiceQR");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var QRd = new QR()
                {
                    ServiceId = ServiceIds.Text.Trim(),
                    BookingId = BookingIds.Text.Trim(),
                    BookingType = "B"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BookingOthersQR", QRd).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                    if (Status == "Success.")
                    {
                        ImageData.ImageUrl = ResponseMsg;
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
    }

    protected void DLReceipt_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label BookingIds = (Label)e.Item.FindControl("lblBillBookingId");
                Label CustomerIDs = (Label)e.Item.FindControl("lblCustomerid");
                Label CustomerMobile = (Label)e.Item.FindControl("lblCustomerMobile");

                Image ImageData = (Image)e.Item.FindControl("imgBoatBulkReceiptQR");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    var QRd = new QRBoat()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        BookingId = BookingIds.Text.Trim(),
                        Pin = "Bulk",
                        BookingRef = "Bulk"
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("PassQR", QRd).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                        if (Status == "Success.")
                        {
                            ImageData.ImageUrl = ResponseMsg;
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

                // Print Boating Instruction

                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblBookingId = (Label)e.Item.FindControl("lblBookingId");

                var BoatServiceId = e.Item.FindControl("dtlisTicketInsBulk") as DataList;

                try
                {
                    Control trBoatIns = e.Item.FindControl("trBoatInsBulk") as Control;
                    trBoatIns.Visible = false;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var service = new Bookingotherservices()
                        {
                            ServiceType = "1",// Default 1 is Boat boking//
                            BoatHouseId = Session["BoatHouseId"].ToString()
                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("PrintInstrucSvc", service).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var BoatLst = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                                if (dt.Rows.Count > 0)
                                {
                                    BoatServiceId.DataSource = dt;
                                    BoatServiceId.DataBind();

                                    trBoatIns.Visible = true;
                                }
                                else
                                {
                                    BoatServiceId.DataBind();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    //Other Service Booking

    private void OtrGetOtherTickets(string sBookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = sBookingId.ToString(),
                    BookingType = "I"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("OtherTicket", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Oticket = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Oticket)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Oticket)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlistTicketOtherOtr.DataSource = dt;
                            dtlistTicketOtherOtr.DataBind();
                            dtlistTicketOtherOtr.Visible = true;
                        }
                        else
                        {
                            dtlistTicketOtherOtr.Visible = false;
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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

    private void GetOtherSummaryReceipt(string sBookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    QueryType = "OtherPrintTicketBulkReceipts",
                    ServiceType = "",
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BookingId = sBookingId.ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    UserId = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonAPIMethod", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        DlOtherReceipt.DataSource = dtExists;
                        DlOtherReceipt.DataBind();

                        // DlOtherReceipt.Columns[1].FooterText = dtExists.AsEnumerable().Select(x => x.Field<int>("NoOfItems")).Sum().ToString();
                        // DlOtherReceipt.Columns[2].FooterText = dtExists.AsEnumerable().Select(x => x.Field<decimal>("ItemCharge")).Sum().ToString();

                        Int64 TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt64(row.Field<Int64>("NoOfItems")));
                        decimal ItemCharge = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ItemCharge")));

                        decimal TaxAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));
                        decimal NetAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));

                        DlOtherReceipt.FooterRow.Cells[0].Text = "Total";
                        DlOtherReceipt.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                        DlOtherReceipt.FooterRow.Cells[1].Text = TotalCount.ToString();
                        DlOtherReceipt.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                        DlOtherReceipt.FooterRow.Cells[2].Text = ItemCharge.ToString();
                        DlOtherReceipt.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                        DlOtherReceipt.FooterRow.Cells[3].Text = TaxAmount.ToString();
                        DlOtherReceipt.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                        DlOtherReceipt.FooterRow.Cells[4].Text = NetAmount.ToString();
                        DlOtherReceipt.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                        lblGrandNetAmount.Text = NetAmount.ToString();

                        //dtblBalanceDetails.Compute("SUM(Property_Tax)", "").ToString();

                        lblBookingId.Text = dtExists.Rows[0]["BookingId"].ToString();
                        lblBookingDate.Text = dtExists.Rows[0]["BookingDate"].ToString();
                        lblBoatHouseName.Text = dtExists.Rows[0]["BoatHouseName"].ToString();
                        lblPaymentTypeName.Text = dtExists.Rows[0]["PaymentTypeName"].ToString();
                        lblCustomerMobileNo.Text = dtExists.Rows[0]["CustomerMobileNo"].ToString();
                        lblGSTOtr.Text = dtExists.Rows[0]["GSTNumber"].ToString();
                        lblCustomerGSTNoOtr.Text = "";

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Boat Booking Today...!');", true);
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

    private void OtherTicketInstructions(string sBookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string boatTypeIds = string.Empty;

                var service = new OtherBook()
                {
                    ServiceType = "2",// Default 1 is Boat boking//
                    BoatHouseId = Session["BoatHouseId"].ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("PrintInstrucSvc", service).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlisTicketInsOther.DataSource = dt;
                            dtlisTicketInsOther.DataBind();

                            //trBoatIns.Visible = true;
                        }
                        else
                        {
                            dtlisTicketInsOther.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
        }
    }

    protected void DataList1OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            lblChargePerItem = 0;
            lblNoOfItems = 0;
            lblItemCharge = 0;
            lblTaxAmount = 0;
            lblNetAmount = 0;
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //lblChargePerItem += Convert.ToDecimal(((Label)e.Item.FindControl("lblChargePerItem")).Text);
            lblNoOfItems += Convert.ToDecimal(((Label)e.Item.FindControl("lblNoOfItems")).Text);
            lblItemCharge += Convert.ToDecimal(((Label)e.Item.FindControl("lblItemCharge")).Text);
            lblTaxAmount += Convert.ToDecimal(((Label)e.Item.FindControl("lblTaxAmount")).Text);
            lblNetAmount += Convert.ToDecimal(((Label)e.Item.FindControl("lblNetAmount")).Text);
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            //Label lblTotalChargePerItem = (Label)e.Item.FindControl("lblTotalChargePerItem");
            //lblTotalChargePerItem.Text = lblChargePerItem.ToString();

            Label lblTotalNoOfItems = (Label)e.Item.FindControl("lblTotalNoOfItems");
            lblTotalNoOfItems.Text = lblNoOfItems.ToString();

            Label lblTotalItemCharge = (Label)e.Item.FindControl("lblTotalItemCharge");
            lblTotalItemCharge.Text = lblItemCharge.ToString();

            Label lblTotalTaxAmount = (Label)e.Item.FindControl("lblTotalTaxAmount");
            lblTotalTaxAmount.Text = lblTaxAmount.ToString();

            Label lblTotalNetAmount = (Label)e.Item.FindControl("lblTotalNetAmount");
            lblTotalNetAmount.Text = lblNetAmount.ToString();


            lblGrandNetAmount.Text = lblNetAmount.ToString();
        }
    }

    protected void OtrdtlistTicketOther_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label BookingIds = (Label)e.Item.FindControl("lblBillOBookingId");
            Label ServiceIds = (Label)e.Item.FindControl("lblBillOServiceId");
            DataRowView dr = (DataRowView)e.Item.DataItem;
            Image ImageData = (Image)e.Item.FindControl("imgOtherQR");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var QRd = new QR()
                {
                    ServiceId = ServiceIds.Text.Trim(),
                    BookingId = BookingIds.Text.Trim(),
                    BookingType = "I"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BookingOthersQR", QRd).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                    if (Status == "Success.")
                    {
                        ImageData.Visible = true;
                        //ImageData.ImageUrl = @"ImgPost/" + dr.Row["ImgPath"].ToString();
                        ImageData.ImageUrl = ResponseMsg;
                        imgOtherReceiptQR.ImageUrl = ResponseMsg;
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
    }

    //Restaurant

    public void CheckQRcodeGeneration()//newly added for QRcode 
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

                var body = new CommonClass()
                {
                    QueryType = "GetQRcodeGeneration",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CommonReport", body).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["QRcodeGenerate"] = dtExists.Rows[0]["QRcodeGenerate"].ToString().Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void GvBindRestaurant(string BookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var RestaurantBooking = new RestaurantBooking()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = BookingId.ToString(),
                    BookingType = "R"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RestaurantTicket", RestaurantBooking).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Oticket = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Oticket)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Oticket)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            //dtlistTicketRestaurant.DataSource = dt;
                            //dtlistTicketRestaurant.DataBind();
                            //dtlistTicketRestaurant.Visible = true;
                            if (ViewState["QRcodeGenerate"].ToString().Trim() == "Y")
                            {
                                dtlistTicketRestaurant.DataSource = dt;
                                dtlistTicketRestaurant.DataBind();
                                dtlistTicketRestaurant.Visible = true;
                            }
                            if (ViewState["QRcodeGenerate"].ToString().Trim() == "N")
                            {
                                dtlistTicketRestaurantnoQRcode.DataSource = dt;
                                dtlistTicketRestaurantnoQRcode.DataBind();
                                dtlistTicketRestaurantnoQRcode.Visible = true;
                            }
                        }
                        else
                        {
                            dtlistTicketRestaurant.Visible = false;
                            dtlistTicketRestaurantnoQRcode.Visible = false;
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


    protected void dtlistTicketRestaurantnoQRcode_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label BookingIds = (Label)e.Item.FindControl("lblBillOBookingId");
                Label ServiceIds = (Label)e.Item.FindControl("lblBillOServiceId");
                DataRowView dr = (DataRowView)e.Item.DataItem;
                Image ImageData = (Image)e.Item.FindControl("imgRestaurantQR");


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    var QRd = new QR()
                    {
                        ServiceId = ServiceIds.Text.Trim(),
                        BookingId = BookingIds.Text.Trim(),
                        BookingType = "R"
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("BookingOthersQR", QRd).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                        if (Status == "Success.")
                        {
                            //ImageData.Visible = false;
                            //ImageData.ImageUrl = @"ImgPost/" + dr.Row["ImgPath"].ToString();
                            //ImageData.ImageUrl = ResponseMsg;
                            //imgRestaurantQR1.ImageUrl = ResponseMsg;
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

                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblBookingId = (Label)e.Item.FindControl("lblBookingId");

                var BoatServiceId = e.Item.FindControl("dtlistTicketOtherRes") as DataList;


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void dtlistTicketRestaurant_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label BookingIds = (Label)e.Item.FindControl("lblBillOBookingId");
                Label ServiceIds = (Label)e.Item.FindControl("lblBillOServiceId");
                DataRowView dr = (DataRowView)e.Item.DataItem;
                Image ImageData = (Image)e.Item.FindControl("imgRestaurantQR");


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    var QRd = new QR()
                    {
                        ServiceId = ServiceIds.Text.Trim(),
                        BookingId = BookingIds.Text.Trim(),
                        BookingType = "R"
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("BookingOthersQR", QRd).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                        if (Status == "Success.")
                        {
                            ImageData.Visible = true;
                            //ImageData.ImageUrl = @"ImgPost/" + dr.Row["ImgPath"].ToString();
                            ImageData.ImageUrl = ResponseMsg;
                            imgRestaurantQR1.ImageUrl = ResponseMsg;
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

                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblBookingId = (Label)e.Item.FindControl("lblBookingId");

                var BoatServiceId = e.Item.FindControl("dtlistTicketOtherRes") as DataList;

                //try
                //{
                //    Control trBoatIns = e.Item.FindControl("trInsRestaurant") as Control;
                //    trInsRestaurant.Visible = false;

                //    using (var client = new HttpClient())
                //    {
                //        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                //        client.DefaultRequestHeaders.Clear();
                //        client.DefaultRequestHeaders.Accept.Clear();

                //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //        string boatTypeIds = string.Empty;

                //        var service = new OtherBook()
                //        {
                //            ServiceType = "3",// Default 1 is Boat boking//
                //            BoatHouseId = Session["BoatHouseId"].ToString()
                //        };

                //        HttpResponseMessage response = client.PostAsJsonAsync("PrintInstrucSvc", service).Result;

                //        if (response.IsSuccessStatusCode)
                //        {
                //            var BoatLst = response.Content.ReadAsStringAsync().Result;
                //            int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                //            string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                //            if (StatusCode == 1)
                //            {
                //                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                //                if (dt.Rows.Count > 0)
                //                {
                //                    BoatServiceId.DataSource = dt;
                //                    BoatServiceId.DataBind();

                //                    trBoatIns.Visible = true;
                //                }
                //                else
                //                {
                //                    BoatServiceId.DataBind();
                //                }
                //            }
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
                //}
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetRestaurantSummaryReceipt(string sBookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new Ticket()
                {
                    QueryType = "RestaurantTicketBulkReceipts",
                    ServiceType = "",
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BookingId = sBookingId.ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    UserId = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonAPIMethod", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {

                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {

                        DlRestaurantReceipt.DataSource = dtExists;
                        DlRestaurantReceipt.DataBind();

                        Int64 TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt64(row.Field<Int64>("NoOfItems")));
                        decimal ItemCharge = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ItemCharge")));

                        decimal TaxAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));
                        decimal NetAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));

                        DlRestaurantReceipt.FooterRow.Cells[0].Text = "Total";
                        DlRestaurantReceipt.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                        DlRestaurantReceipt.FooterRow.Cells[1].Text = TotalCount.ToString();
                        DlRestaurantReceipt.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                        DlRestaurantReceipt.FooterRow.Cells[2].Text = ItemCharge.ToString();
                        DlRestaurantReceipt.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                        DlRestaurantReceipt.FooterRow.Cells[3].Text = TaxAmount.ToString();
                        DlRestaurantReceipt.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                        DlRestaurantReceipt.FooterRow.Cells[4].Text = NetAmount.ToString();
                        DlRestaurantReceipt.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                        lblGrandNetAmountRestaurant.Text = NetAmount.ToString();



                        lblBookingIdRestaurant.Text = dtExists.Rows[0]["BookingId"].ToString();
                        lblBookingDateRestaurant.Text = dtExists.Rows[0]["BookingDate"].ToString();
                        lblBoatHouseNameRestaurant.Text = dtExists.Rows[0]["BoatHouseName"].ToString();
                        lblPaymentTypeNameRestaurant.Text = dtExists.Rows[0]["PaymentTypeName"].ToString();
                        lblCustomerMobileNoRestaurant.Text = dtExists.Rows[0]["CustomerMobileNo"].ToString();
                        lblGSTNumberRestaurant.Text = dtExists.Rows[0]["GSTNumber"].ToString();

                        if (lblCustomerMobileNoRestaurant.Text != "")
                        {
                            lblMobile.Visible = true;

                        }
                        else
                        {
                            lblMobile.Visible = false;
                        }

                        //object sumNetAmount;
                        //sumNetAmount = dtExists.Compute("Sum(Convert.ToDecimal(NetAmount))", string.Empty);
                        //decimal dNet = (Convert.ToDecimal(sumNetAmount.ToString()));
                        //lblGrandNetAmount.Text = dNet.ToString();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Boat Booking Today...!');", true);
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

    private void RestaurantTicketInstructions()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string boatTypeIds = string.Empty;

                var service = new Ticket()
                {
                    ServiceType = "3",// Default 1 is Boat boking//
                    BoatHouseId = Session["BoatHouseId"].ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("PrintInstrucSvc", service).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlisTicketInsRestaurant.DataSource = dt;
                            dtlisTicketInsRestaurant.DataBind();
                        }
                        else
                        {
                            dtlisTicketInsRestaurant.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
        }
    }

    public void BindChangeBoatDetails(string sBookingId, string sBookingPin)
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

                var body = new CommonClass()
                {
                    QueryType = "BindChangeBoatDetails",
                    ServiceType = "",
                    Input1 = sBookingId,
                    Input2 = sBookingPin,
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CommonReport", body).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {

                        string sOldBoatType = dtExists.Rows[0]["OldBoatType"].ToString().Trim();
                        string sOldBoatSeater = dtExists.Rows[0]["OldSeaterType"].ToString().Trim();
                        string sNewBoatType = dtExists.Rows[0]["NewBoatType"].ToString().Trim();
                        string sNewBoatSeater = dtExists.Rows[0]["NewSeaterType"].ToString().Trim();
                        string sBookingDate = dtExists.Rows[0]["BookingDate"].ToString().Trim();
                        string sOldNetAmount = dtExists.Rows[0]["OldNetAmount"].ToString().Trim();
                        string sNewNetAmount = dtExists.Rows[0]["NewNetAmount"].ToString().Trim();
                        string sExtraCharge = dtExists.Rows[0]["ExtraCharge"].ToString().Trim();
                        string sRefundAmount = dtExists.Rows[0]["ExtraRefundAmount"].ToString().Trim();
                        string sPaymentMode = dtExists.Rows[0]["PaymentModeType"].ToString().Trim();
                        Session["ChangeBookingId"] = sBookingId.Trim();
                        BindChangeBoat(sBookingId, sBookingPin, sOldBoatType, sOldBoatSeater,
                  sNewBoatType, sNewBoatSeater, sBookingDate, sOldNetAmount, sNewNetAmount, sExtraCharge, sRefundAmount, sPaymentMode);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void BindChangeBoat(string sBookingId, string sBookingPin, string sOldBoatType, string sOldBoatSeater,
       string sNewBoatType, string sNewBoatSeater, string sBookingDate, string sOldNetAmount, string sNewNetAmount,
       string sExtraCharge, string sRefundAmount, string sPaymentMode)
    {
        try
        {
            if (sRefundAmount.Trim() != "0")
            {
                ChangeRefundAmount.Visible = true;
                lblChangeBoatHeader.Text = "CHANGE BOAT PAYMENT";
                lblChangeRefundAmount.Text = sRefundAmount;
                ChangeExtraCharge.Visible = false;
            }
            else
            {
                ChangeExtraCharge.Visible = true;
                ChangeRefundAmount.Visible = false;
                lblChangeBoatHeader.Text = "CHANGE BOAT RECEIPT";
                lblChangeExtraCharge.Text = sExtraCharge;

            }
            lblChangeBoatHouseName.Text = Session["BoatHouseName"].ToString();

            lblChangeBookingId.Text = sBookingId;
            lblChangeBookingPin.Text = sBookingPin;
            lblOldBoatType.Text = sOldBoatType;
            lblOldBoatSeater.Text = sOldBoatSeater;
            lblNewBoatType.Text = sNewBoatType;
            lblNewBoatSeater.Text = sNewBoatSeater;
            lblChangeBookingDate.Text = sBookingDate;
            lblChangeOldAmount.Text = sOldNetAmount;
            lblChangeNewAmount.Text = sNewNetAmount;
            lblChangePaymentMode.Text = sPaymentMode;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindRefundCancelAmount(string sBookingId, string sBookingPin, string sBookingDate, string sPaymentType, string sBoatCharge, string sDepositCharge, string sTotalCharge, string sCancelCharge, string sRefundAmount)
    {
        try
        {
            lblRefCancelBoatHouseName.Text = Session["BoatHouseName"].ToString();
            lblRefCancelHeader.Text = " CANCELLATION REFUND PAYMENT";
            lblRefCancelVoucherNo.Text = sBookingId + "/" + sBookingPin;
            lblRefCancelBookingId.Text = sBookingId;
            lblRefCancelBookingPin.Text = sBookingPin;

            lblRefCancelDate.Text = sBookingDate;
            lblRefCancelPayment.Text = sPaymentType;
            lblRefCancelBoatCharge.Text = sBoatCharge;
            lblRefCancelDepAmnt.Text = sDepositCharge;
            lblRefCancelTotalCharges.Text = sTotalCharge;
            lblRefCancellationCharges.Text = sCancelCharge;
            tblRefCancelAmount.Visible = true;
            lblRefCancelAmount.Text = "₹" + sRefundAmount.Trim();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
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


    //Refunf cash report
    public void GetRefundCashFromReport(string FromDate, string userid)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var RestaurantBooking = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate.ToString(),
                    CashflowTypes = "CashReceived",
                    RequestedBy = userid.ToString(),
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RefundCashFromReport", RestaurantBooking).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Oticket = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Oticket)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Oticket)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            lblBoatHouseNameRefund.Text = Session["BoatHouseName"].ToString();
                            lblrptTitleRefund.Text = "Refund Cash Report";

                            DateTime now = DateTime.Now;
                            lblEntPrintDateRefund.Text = now.ToString("dd/MM/yyy HH/mm/ss");
                            lblrptDateRefund.Text = FromDate.ToString();

                            //lblUserFullNameBoat.Text = Request.QueryString["UN"].ToString().Trim();

                            gvRefundCashfrom.DataSource = dt;
                            gvRefundCashfrom.DataBind();
                            gvRefundCashfrom.Visible = true;

                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));
                            ViewState["CashFromTotal"] = TotalAmount;

                            gvRefundCashfrom.FooterRow.Cells[0].Text = "Total";
                            gvRefundCashfrom.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                            gvRefundCashfrom.FooterRow.Cells[1].Text = TotalAmount.ToString("N2");
                            gvRefundCashfrom.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            gvRefundCashfrom.Visible = false;
                            lblRefundTotalFromCounter.Text = "0.00";
                        }
                    }
                    else
                    {
                        gvRefundCashfrom.Visible = false;
                        lblRefundTotalFromCounter.Text = "0.00";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    gvRefundCashfrom.Visible = false;
                    lblRefundTotalFromCounter.Text = "0.00";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void buildDenomination()
    {
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Denomination", typeof(string)), new DataColumn("Count", typeof(string)), new DataColumn("Amount", typeof(decimal)) });

        // Split authors separated by a comma followed by space  
        //string sDen = "2000-10-20000~500-10-5000~200-2-400~100-2-200~50-1-50~20-1-20~10-1-10~5-1-5~2-1-2~1-1-1";

        string sDen = Request.QueryString["sDen"].ToString();
        if (sDen.Trim() == "" || sDen.Trim() == "0-0-0")
        {
            Table14.Visible = false; //Boating => divBoatingSummary
            Table11.Visible = false; //RefundCashRpt => divRefundCashRpt
            Table19.Visible = false; //AdditionalTicket => divAdditionalTicket
            Table15.Visible = false; //Restaurant/Others => divEntranceSummary
            return;
            //sDen = "0-0-0";
        }
        string[] authorsList = sDen.Split('~');
        foreach (string author in authorsList)
        {
            string[] ausub = author.Split('-');
            dt.Rows.Add(ausub[0], ausub[1], ausub[2]);
        }

        if (ViewState["rt"].ToString() == "rbss")
        {
            gvBoatDenomination.DataSource = dt;
            gvBoatDenomination.DataBind();
            gvBoatDenomination.Visible = true;

            decimal TotalCount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Count")));
            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Amount")));

            gvBoatDenomination.FooterRow.Cells[0].Text = "Total";
            gvBoatDenomination.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

            gvBoatDenomination.FooterRow.Cells[1].Text = TotalCount.ToString();
            gvBoatDenomination.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;


            //gvBoatDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
            //gvBoatDenomination.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

            CultureInfo ci = new CultureInfo("en-IN");
            // assign your custom Rupee symbol of your country
            ci.NumberFormat.CurrencySymbol = "&#8377;";
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

            //gvDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
            gvBoatDenomination.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmount);
            gvBoatDenomination.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (ViewState["rt"].ToString() == "ross" || ViewState["rt"].ToString() == "rrss") //report other service summary OR report restaurant service summary
        {
            gvOtherResDenomination.DataSource = dt;
            gvOtherResDenomination.DataBind();
            gvOtherResDenomination.Visible = true;

            decimal TotalCount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Count")));
            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Amount")));

            gvOtherResDenomination.FooterRow.Cells[0].Text = "Total";
            gvOtherResDenomination.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

            gvOtherResDenomination.FooterRow.Cells[1].Text = TotalCount.ToString();
            gvOtherResDenomination.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;


            CultureInfo ci = new CultureInfo("en-IN");
            // assign your custom Rupee symbol of your country
            ci.NumberFormat.CurrencySymbol = "&#8377;";
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

            //gvDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
            gvOtherResDenomination.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmount);
            gvOtherResDenomination.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (ViewState["rt"].ToString() == "recr")
        {
            gvDenomination.DataSource = dt;
            gvDenomination.DataBind();
            gvDenomination.Visible = true;

            decimal TotalCount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Count")));
            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Amount")));

            gvDenomination.FooterRow.Cells[0].Text = "Total";
            gvDenomination.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

            gvDenomination.FooterRow.Cells[1].Text = TotalCount.ToString();
            gvDenomination.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;


            CultureInfo ci = new CultureInfo("en-IN");
            // assign your custom Rupee symbol of your country
            ci.NumberFormat.CurrencySymbol = "&#8377;";
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

            //gvDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
            gvDenomination.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmount);
            gvDenomination.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

        }

        else if (ViewState["rt"].ToString() == "rATss" || ViewState["rt"].ToString() == "rATss") //report Additional Ticket summary 
        {
            gvAdditionalTicketDenomination.DataSource = dt;
            gvAdditionalTicketDenomination.DataBind();
            gvAdditionalTicketDenomination.Visible = true;

            decimal TotalCount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Count")));
            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Amount")));

            gvAdditionalTicketDenomination.FooterRow.Cells[0].Text = "Total";
            gvAdditionalTicketDenomination.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

            gvAdditionalTicketDenomination.FooterRow.Cells[1].Text = TotalCount.ToString();
            gvAdditionalTicketDenomination.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;


            CultureInfo ci = new CultureInfo("en-IN");
            // assign your custom Rupee symbol of your country
            ci.NumberFormat.CurrencySymbol = "&#8377;";
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

            //gvDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
            gvAdditionalTicketDenomination.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmount);
            gvAdditionalTicketDenomination.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

        }

    }

    public void GetRefundCashPaymentReport(string FromDate, string Userid)
    {
        try
        {
            ViewState["CRRTotalAmount"] = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate,
                    ToDate = FromDate,
                    QueryType = "TripSheetSummaryDetails",
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = Userid.ToString(),
                    Input5 = "ParticularUser"

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                    if (dtExists.Rows.Count > 0)
                    {

                        gvRefundPayAmount.DataSource = dtExists;
                        gvRefundPayAmount.DataBind();
                        gvRefundPayAmount.Visible = true;

                        decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<double>("ClaimedDeposit")));

                        ViewState["CRRTotalAmount"] = TotalAmount.ToString().Trim();

                        gvRefundPayAmount.FooterRow.Cells[0].Text = "Total";
                        gvRefundPayAmount.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                        gvRefundPayAmount.FooterRow.Cells[1].Text = TotalAmount.ToString("N2");
                        gvRefundPayAmount.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                        //lblRefundTotalFromCounter.Text = ViewState["CashFromTotal"].ToString();
                        //lblRefundLessAmount.Text = TotalAmount.ToString("N2");

                        //lblRefundCashInHand.Text = (Convert.ToDecimal(lblRefundTotalFromCounter.Text) - Convert.ToDecimal(lblRefundLessAmount.Text)).ToString();

                    }
                    else
                    {
                        //bblblReceivedAmt.Text = "0";
                        //bblblRefundAmt.Text = "0";
                        //bblblCashInHand.Text = "0";
                    }

                    lblBhn.Text = Session["BoatHouseName"].ToString();
                    lblbhn1.Text = Session["BoatHouseName"].ToString();

                    buildDenomination();

                    if (ViewState["CashFromTotal"].ToString() != "")
                    {
                        lblRefundTotalFromCounter.Text = ViewState["CashFromTotal"].ToString();
                    }
                    else
                    {
                        lblRefundTotalFromCounter.Text = "0.00";
                    }

                    if (ViewState["CRRTotalAmount"].ToString() != "")
                    {
                        lblRefundLessAmount.Text = (Convert.ToDecimal(ViewState["CRRTotalAmount"])).ToString("N2");
                    }
                    else
                    {
                        lblRefundLessAmount.Text = "0.00";
                    }
                    if (lblRefundTotalFromCounter.Text != "0.00")
                    {
                        lblRefundCashInHand.Text = (Convert.ToDecimal(lblRefundTotalFromCounter.Text) - Convert.ToDecimal(lblRefundLessAmount.Text)).ToString();
                    }
                    else
                    {
                        lblRefundCashInHand.Text = (Convert.ToDecimal(lblRefundLessAmount.Text)).ToString();
                    }
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        string[] cashinhand = lblRefundCashInHand.Text.Split('.');
                    }
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Additional
    public void GvBindAdditionalTicket(string BookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var RestaurantBooking = new RestaurantBooking()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = BookingId.ToString(),
                    BookingType = "A"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("PrintAdditionalTicket", RestaurantBooking).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Oticket = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Oticket)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Oticket)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlistAdditional.DataSource = dt;
                            dtlistAdditional.DataBind();
                            dtlistAdditional.Visible = true;
                        }
                        else
                        {
                            dtlistAdditional.Visible = false;
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

    protected void dtlistAdditional_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label BookingIds = (Label)e.Item.FindControl("lblBillOBookingId");
                Label ServiceIds = (Label)e.Item.FindControl("lblBillOServiceId");
                DataRowView dr = (DataRowView)e.Item.DataItem;
                Image ImageData = (Image)e.Item.FindControl("imgAddlTicketQR");


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    var QRd = new QR()
                    {
                        ServiceId = ServiceIds.Text.Trim(),
                        BookingId = BookingIds.Text.Trim(),
                        BookingType = "A"
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("BookingOthersQR", QRd).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                        if (Status == "Success.")
                        {
                            ImageData.Visible = true;
                            ImageData.ImageUrl = ResponseMsg;
                            imgAddlTicketQRAbstract.ImageUrl = ResponseMsg;
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
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetAdditionalTicketSummaryReceipt(string sBookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new Ticket()
                {
                    QueryType = "PrintAdditionalTicketAbstract",
                    ServiceType = "",
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BookingId = sBookingId.ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    UserId = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonAPIMethod", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {

                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {

                        DlAdditionalTicketReceipt.DataSource = dtExists;
                        DlAdditionalTicketReceipt.DataBind();

                        Int64 TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt64(row.Field<Int64>("NoOfItems")));
                        decimal ItemCharge = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ItemCharge")));

                        decimal TaxAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));
                        decimal NetAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));

                        DlAdditionalTicketReceipt.FooterRow.Cells[0].Text = "Total";
                        DlAdditionalTicketReceipt.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                        DlAdditionalTicketReceipt.FooterRow.Cells[1].Text = TotalCount.ToString();
                        DlAdditionalTicketReceipt.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                        DlAdditionalTicketReceipt.FooterRow.Cells[2].Text = ItemCharge.ToString();
                        DlAdditionalTicketReceipt.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                        DlAdditionalTicketReceipt.FooterRow.Cells[3].Text = TaxAmount.ToString();
                        DlAdditionalTicketReceipt.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                        DlAdditionalTicketReceipt.FooterRow.Cells[4].Text = NetAmount.ToString();
                        DlAdditionalTicketReceipt.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                        lblAdditionalTicketNetAmt.Text = NetAmount.ToString();

                        lblATBookingId.Text = dtExists.Rows[0]["BookingId"].ToString();
                        lblATBookingDate.Text = dtExists.Rows[0]["BookingDate"].ToString();
                        lblATBoatHouseName.Text = dtExists.Rows[0]["BoatHouseName"].ToString();
                        lblATPaymentType.Text = dtExists.Rows[0]["PaymentTypeName"].ToString();
                        lblATMobileNo.Text = dtExists.Rows[0]["CustomerMobileNo"].ToString();
                        lblAdditionalGstNo.Text = dtExists.Rows[0]["GSTNumber"].ToString();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Boat Booking Today...!');", true);
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

    // Entrance Summary Report
    //15 Nov
    public void BindEntranceSummarybyUser(string bDat, string bTypeId, string CatTypeId, string PayTypeId, string sUserId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var sEntranceSummary = new EntranceSummary()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = bDat.Trim(),
                    BoatTypeId = bTypeId.Trim(),
                    Category = CatTypeId.Trim(),
                    PaymentType = PayTypeId.Trim(),
                    CreatedBy = sUserId.Trim()
                };

                string APIName = string.Empty;

                if (ViewState["rt"].ToString() == "ross")
                {
                    APIName = "RptOtherServiceWiseCollection";
                    lblrptTitle.Text = "Entrance Collection Report";
                }
                else if (ViewState["rt"].ToString() == "rrss")
                {
                    APIName = "RptRestaurantServiceWiseCollection";
                    lblrptTitle.Text = "Restaurant Collection Report";
                }

                HttpResponseMessage response = client.PostAsJsonAsync(APIName, sEntranceSummary).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        gvEntrance.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            lblBoatHouseNameEnt.Text = Session["BoatHouseName"].ToString();

                            DateTime now = DateTime.Now;
                            lblEntPrintDate.Text = now.ToString("dd/MM/yyy hh:mm:ss tt");

                            lblEntrptDate.Text = bDat.ToString();

                            lblUserFullName.Text = Request.QueryString["UN"].ToString().Trim();

                            gvEntrance.DataSource = dtExists;
                            gvEntrance.DataBind();
                            gvEntrance.Visible = true;

                            int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                            decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                            gvEntrance.FooterRow.Cells[0].Text = "Total Amount";
                            gvEntrance.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;


                            gvEntrance.FooterRow.Cells[1].Text = TotalCount.ToString();
                            gvEntrance.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            CultureInfo ci = new CultureInfo("en-IN");
                            // assign your custom Rupee symbol of your country
                            ci.NumberFormat.CurrencySymbol = "&#8377;";
                            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

                            //gvDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
                            gvEntrance.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmount);
                            gvEntrance.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            PaymentReceivedTypes(bDat.Trim(), sUserId.Trim());
                            lblReceivedAmount.Text = TotalAmount.ToString().Trim();
                            lblPaidAmount.Text = "0.00";
                            decimal CashInHands = (Convert.ToDecimal(TotalAmount.ToString().Trim())) - (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()));
                            lblCashInHand.Text = Math.Round(CashInHands, 0).ToString();

                            ViewState["btFinalCashInHand"] = lblCashInHand.Text.Trim();

                            lblFinalNetAmount.Text = ((Convert.ToDecimal(lblCashInHand.Text)) + (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()))).ToString("N2");

                            lblroBhname.Text = Session["BoatHouseName"].ToString();
                            lblroBhname1.Text = Session["BoatHouseName"].ToString();
                        }
                        else
                        {
                            gvEntrance.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                        }
                    }
                }
                else
                {
                    gvEntrance.Visible = false;
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


    //Boatbooking Summary by user wise collection

    public void BoatWiseCollectionSummary(string fDat, string tDat, string sUserId, string sBoatTypeId, string sBoatType)
    {
        try
        {

            lblBoatHouseNameBoat.Text = Session["BoatHouseName"].ToString();

            if (sBoatTypeId.Trim() == "0")
            {
                lblrptTitleBoat.Text = "All Boat Collection Report";
            }
            else
            {
                lblrptTitleBoat.Text = sBoatType.Trim() + " Collection Report";
            }

            DateTime now = DateTime.Now;
            lblEntPrintDateBoat.Text = now.ToString("dd/MM/yyy hh:mm:ss tt");

            lblrptDateBoat.Text = fDat.ToString();

            lblUserFullNameBoat.Text = Request.QueryString["UN"].ToString().Trim();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var sBoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = sBoatTypeId.Trim(),
                    FromDate = fDat.Trim(),
                    ToDate = tDat.Trim(),
                    CreatedBy = sUserId.Trim(),
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RptUserBasedServiceListPrint", sBoatSearch).Result;

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
                            ////17 Nov Commented for Extnsn ticket print
                            //lblBoatHouseNameBoat.Text = Session["BoatHouseName"].ToString();

                            //if (sBoatTypeId.Trim() == "0")
                            //{
                            //    lblrptTitleBoat.Text = "All Boat Collection Report";
                            //}
                            //else
                            //{
                            //    lblrptTitleBoat.Text = sBoatType.Trim() + " Collection Report";
                            //}

                            //DateTime now = DateTime.Now;
                            //lblEntPrintDateBoat.Text = now.ToString("dd/MM/yyy hh:mm:ss tt");

                            //lblrptDateBoat.Text = fDat.ToString();

                            //lblUserFullNameBoat.Text = Request.QueryString["UN"].ToString().Trim();

                            gvBoatServiceSummary.DataSource = dt;
                            gvBoatServiceSummary.DataBind();
                            gvBoatServiceSummary.Visible = true;





                            int TotalCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Count")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));

                            gvBoatServiceSummary.FooterRow.Cells[0].Text = "Total";
                            gvBoatServiceSummary.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                            gvBoatServiceSummary.FooterRow.Cells[1].Text = TotalCount.ToString();
                            gvBoatServiceSummary.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            CultureInfo ci = new CultureInfo("en-IN");
                            // assign your custom Rupee symbol of your country
                            ci.NumberFormat.CurrencySymbol = "&#8377;";
                            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

                            //gvDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
                            gvBoatServiceSummary.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmount);
                            gvBoatServiceSummary.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            gvBoatServiceSummary.Visible = false;
                        }
                    }
                    else
                    {
                        //divBoatingSummary.Visible = false;
                        gvBoatServiceSummary.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    //divBoatingSummary.Visible = false;
                    gvBoatServiceSummary.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindBoatServiceWise(string fDat, string tDat, string sUserId, string sBoatTypeId, string sBoatType)
    {
        try
        {
            ViewState["btTotal"] = string.Empty;
            ViewState["btTotalPaid"] = string.Empty;
            ViewState["btTotalCashInHand"] = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var ChallanAb = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = fDat.Trim(),
                    BoatTypeId = sBoatTypeId.Trim(),
                    PaymentType = "0",
                    CreatedBy = sUserId.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RptServiceWiseCollection", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        GvBoatServiceWise.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);

                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {

                            GvBoatServiceWise.DataSource = dtExists;
                            GvBoatServiceWise.DataBind();
                            GvBoatServiceWise.Visible = true;
                            GvBoatServiceWise.Visible = true;
                            int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                            decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                            GvBoatServiceWise.FooterRow.Cells[0].Text = "Total";
                            GvBoatServiceWise.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                            GvBoatServiceWise.FooterRow.Cells[1].Text = TotalCount.ToString();
                            GvBoatServiceWise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            CultureInfo ci = new CultureInfo("en-IN");
                            // assign your custom Rupee symbol of your country
                            ci.NumberFormat.CurrencySymbol = "&#8377;";
                            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

                            //gvDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
                            GvBoatServiceWise.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmount);
                            GvBoatServiceWise.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            ViewState["btTotal"] = TotalAmount.ToString().Trim();
                            lblReceivedAmount.Text = TotalAmount.ToString("N2").Trim();

                            //1011
                            using (var client1 = new HttpClient())
                            {
                                client1.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                                client1.DefaultRequestHeaders.Clear();
                                client1.DefaultRequestHeaders.Accept.Clear();
                                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                var ChallanAb1 = new BoatSearch()
                                {
                                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                    BookingDate = fDat.Trim(),
                                    BoatTypeId = sBoatTypeId.Trim(),
                                    PaymentType = "0",
                                    CreatedBy = sUserId.Trim()
                                };
                                HttpResponseMessage response1 = client.PostAsJsonAsync("RptServiceWisePayment", ChallanAb1).Result;

                                if (response1.IsSuccessStatusCode)
                                {
                                    var Response = response1.Content.ReadAsStringAsync().Result;
                                    if (Response.Contains("No Records Found."))
                                    {
                                        GvServiceWisePayments.Visible = false;
                                        Payments.Visible = false;
                                    }
                                    else
                                    {
                                        var ResponseMsg1 = JObject.Parse(Response)["Table"].ToString();
                                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                                        if (dt.Rows.Count > 0)
                                        {
                                            GvServiceWisePayments.DataSource = dt;
                                            GvServiceWisePayments.DataBind();
                                            GvServiceWisePayments.Visible = true;

                                            int TotalCountPaid = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                                            decimal TotalAmountPaid = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("Amount")));

                                            GvServiceWisePayments.FooterRow.Cells[0].Text = "Total";
                                            GvServiceWisePayments.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;

                                            GvServiceWisePayments.FooterRow.Cells[1].Text = TotalCountPaid.ToString();
                                            GvServiceWisePayments.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                                            CultureInfo c = new CultureInfo("en-IN");
                                            // assign your custom Rupee symbol of your country
                                            c.NumberFormat.CurrencySymbol = "&#8377;";
                                            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = c;

                                            GvServiceWisePayments.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmountPaid);
                                            GvServiceWisePayments.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                                            ViewState["btTotalPaid"] = TotalAmountPaid.ToString().Trim();
                                        }
                                        else
                                        {
                                            GvServiceWisePayments.DataBind();
                                            GvServiceWisePayments.Visible = false;
                                            Payments.Visible = false;
                                        }
                                    }
                                }
                            }

                            lblBBHName.Text = Session["BoatHouseName"].ToString();
                            lblBBHName1.Text = Session["BoatHouseName"].ToString();

                            if (ViewState["btTotalPaid"].ToString().Trim() != "")
                            {
                                string PaidAmount = (Convert.ToDecimal(ViewState["btTotalPaid"].ToString().Trim())).ToString("N2");
                                lblPaidAmount.Text = PaidAmount.Trim();
                                lblBal.Text = (Convert.ToDecimal(lblReceivedAmount.Text) - Convert.ToDecimal(lblPaidAmount.Text)).ToString("N2");
                            }
                            else
                            {
                                lblPaidAmount.Text = "0.00";
                                lblBal.Text = (Convert.ToDecimal(lblReceivedAmount.Text)).ToString("N2");
                            }


                            ViewState["btTotalCashInHand"] = lblBal.Text.Trim();

                            PaymentReceivedTypes(fDat.Trim(), sUserId.Trim());

                            decimal CashInHands = (Convert.ToDecimal(ViewState["btTotalCashInHand"].ToString().Trim())) - (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()));
                            lblCashInHand.Text = Math.Round(CashInHands, 0).ToString();

                            //Math.Round(TaxAmt, 2);
                            ViewState["btFinalCashInHand"] = lblCashInHand.Text.Trim();

                            lblFinalNetAmount.Text = ((Convert.ToDecimal(lblCashInHand.Text)) + (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()))).ToString("N2");


                        }
                        else
                        {
                            GvBoatServiceWise.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                        }
                    }
                }
                else
                {
                    GvBoatServiceWise.Visible = false;
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

    public void PaymentReceivedTypes(string FromDate, string UserId)
    {
        try
        {
            ViewState["OtherPayments"] = string.Empty;
            string ServiceType = string.Empty;

            if (ViewState["rt"].ToString() == "rbss")
            {
                ServiceType = "Boating";
            }
            else if (ViewState["rt"].ToString() == "rrss")
            {
                ServiceType = "Restaurant";

            }
            else if (ViewState["rt"].ToString() == "ross")
            {
                ServiceType = "Other Services";

            }
            else if (ViewState["rt"].ToString() == "rATss")
            {
                ServiceType = "Additional Ticket";

            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate,
                    ToDate = FromDate,
                    QueryType = "PaymentReceivedTypes",
                    ServiceType = ServiceType.Trim(),
                    BookingId = "",
                    Input1 = UserId.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        lblCard.Text = "0";
                        lblOnline.Text = "0";
                        lblUPI.Text = "0";
                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            lblCard.Text = dtExists.Rows[0]["Amount"].ToString();
                            lblOnline.Text = dtExists.Rows[1]["Amount"].ToString();
                            lblUPI.Text = dtExists.Rows[2]["Amount"].ToString();

                            ViewState["OtherPayments"] = (Convert.ToDecimal(lblCard.Text) + Convert.ToDecimal(lblOnline.Text) + Convert.ToDecimal(lblUPI.Text)).ToString();
                        }
                        else
                        {
                            lblCard.Text = "0";
                            lblOnline.Text = "0";
                            lblUPI.Text = "0";
                        }

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Record Found');", true);

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    // Additional summary by user 
    public void BindEntranceAddSummarybyUser(string bDat, string bTypeId, string CatTypeId, string PayTypeId, string sUserId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var sEntranceSummary = new EntranceSummary()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = bDat.Trim(),
                    BoatTypeId = bTypeId.Trim(),
                    Category = CatTypeId.Trim(),
                    PaymentType = PayTypeId.Trim(),
                    CreatedBy = sUserId.Trim()

                };

                string APIName = string.Empty;

                if (ViewState["rt"].ToString() == "rATss")
                {
                    APIName = "RptAdditionalTicketCollection";
                    lblrptTitleAdd.Text = "Additional Ticket Collection Report";
                }


                HttpResponseMessage response = client.PostAsJsonAsync(APIName, sEntranceSummary).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        gvEntranceAdd.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                    }
                    else
                    {
                        //var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                lblBoatHouseNameEntAdd.Text = Session["BoatHouseName"].ToString();

                                DateTime now = DateTime.Now;
                                lblEntPrintDateAdd.Text = now.ToString("dd/MM/yyy hh:mm:ss tt");

                                lblEntrptDateAdd.Text = bDat.ToString();

                                lblUserFullNameAdd.Text = Request.QueryString["UN"].ToString().Trim();

                                gvEntranceAdd.DataSource = dtExists;
                                gvEntranceAdd.DataBind();
                                gvEntranceAdd.Visible = true;

                                int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("TotalCount")));
                                decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));

                                gvEntranceAdd.FooterRow.Cells[0].Text = "Total Amount";
                                gvEntranceAdd.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;


                                gvEntranceAdd.FooterRow.Cells[1].Text = TotalCount.ToString();
                                gvEntranceAdd.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                                CultureInfo ci = new CultureInfo("en-IN");
                                // assign your custom Rupee symbol of your country
                                ci.NumberFormat.CurrencySymbol = "&#8377;";
                                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = ci;

                                //gvDenomination.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
                                gvEntranceAdd.FooterRow.Cells[2].Text = string.Format("{0:C}", TotalAmount);
                                gvEntranceAdd.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                                PaymentReceivedTypes(bDat.Trim(), sUserId.Trim());
                                lblReceivedAmount.Text = TotalAmount.ToString().Trim();
                                lblPaidAmount.Text = "0.00";

                                decimal CashInHands = (Convert.ToDecimal(TotalAmount.ToString().Trim())) - (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()));
                                lblCashInHand.Text = Math.Round(CashInHands, 0).ToString();

                                ViewState["btFinalCashInHand"] = lblCashInHand.Text.Trim();

                                lblFinalNetAmount.Text = ((Convert.ToDecimal(lblCashInHand.Text)) + (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()))).ToString("N2");

                                lblBhname.Text = Session["BoatHouseName"].ToString();
                                lblBhname1.Text = Session["BoatHouseName"].ToString();
                            }
                            else
                            {
                                gvEntranceAdd.Visible = false;
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                            }
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    gvEntranceAdd.Visible = false;
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

    // Refund Not Eligible

    public void BindRefundNotEligible(string sBookingId, string sBookingPin, string sBoatType, string sBoatSeater, string sBookingDate, string sStartTime, string sEndTime, string sBoatDuration, string sTotalDuration, string sBillAmount, string sDepositAmount, string sExtraCharge, string sRefundAmount)
    {
        try
        {
            lblRefundBoatHouseName.Text = Session["BoatHouseName"].ToString();

            //lblRefundHeader.Text = "REFUND RECEIPT";
            lblRefundHeader.Text = "REFUND PAYMENT";
            lblRefundVoucher.Text = "Payment No.  : " + sBookingId + "/" + sBookingPin;
            lblRefundBookingId.Text = sBookingId;
            lblRefundBookingPin.Text = sBookingPin;
            lblRefundBoatType.Text = sBoatType;
            lblRefundBoatSeater.Text = sBoatSeater;
            lblRefundDate.Text = sBookingDate;
            lblRefundDuration.Text = sBoatDuration + " Mins";
            tblRefundEligible.Visible = true;
            lblRefundNotEligible.Text = "REFUND NOT ELIGIBLE";
            lblRefundStartTime.Text = sStartTime;
            lblRefundEndTime.Text = sEndTime;
            lblRefundTotalDuration.Text = sTotalDuration + " Mins ";
            lblBillAmount.Text = "₹" + sBillAmount.Trim();
            lblDepositAmount.Text = "₹" + sDepositAmount.Trim();
            lblExtraCharges.Text = "₹" + sExtraCharge.Trim();
            tblDesignTable.Visible = true;
            tblRefundAmount.Visible = true;
            lblReundAmount.Text = "₹" + sRefundAmount.Trim();




        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Refund Amount
    public void BindRefundAmount(string sBookingId, string sBookingPin, string sBoatType, string sBoatSeater, string sBookingDate, string sStartTime, string sEndTime, string sBoatDuration, string sTotalDuration, string sRefundAmount, string sPayment, string sBillAmount, string sDepositAmount, string sExtraCharge)
    {
        try
        {
            lblRefundBoatHouseName.Text = Session["BoatHouseName"].ToString();
            //lblRefundHeader.Text = "REFUND RECEIPT";
            lblRefundHeader.Text = "REFUND PAYMENT ";
            lblRefundPayment.Visible = true;
            lblRefundVoucher.Text = "Payment No.  : " + sBookingId + "/" + sBookingPin;
            lblRefundBookingId.Text = sBookingId;
            lblRefundBookingPin.Text = sBookingPin;
            lblRefundBoatType.Text = sBoatType;
            lblRefundBoatSeater.Text = sBoatSeater;
            lblRefundDate.Text = sBookingDate;
            lblRefundDuration.Text = sBoatDuration + " Mins";
            tblRefundEligible.Visible = true;
            lblRefundNotEligible.Text = "REFUND IS ELIGIBLE";
            lblRefundStartTime.Text = sStartTime;
            lblRefundEndTime.Text = sEndTime;
            lblRefundTotalDuration.Text = sTotalDuration + " Mins ";
            lblBillAmount.Text = "₹" + sBillAmount.Trim();
            lblDepositAmount.Text = "₹" + sDepositAmount.Trim();
            lblExtraCharges.Text = "₹" + sExtraCharge.Trim();
            tblDesignTable.Visible = true;
            tblRefundAmount.Visible = true;
            lblReundAmount.Text = "₹" + sRefundAmount.Trim();
            tblRefundPayment.Visible = true;
            lblRepaymentType.Text = sPayment.Trim();


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }


    public void BindCorrectAmount(string sBookingId, string sBookingPin, string sBoatType, string sBoatSeater, string sBookingDate, string sStartTime, string sEndTime, string sBoatDuration, string sTotalDuration, string sRefundAmount, string sPayment, string sBillAmount, string sDepositAmount, string sExtraCharge)
    {
        try
        {

            lblRefundBoatHouseName.Text = Session["BoatHouseName"].ToString();

            //lblRefundHeader.Text = "REFUND RECEIPT";
            lblRefundHeader.Text = "REFUND PAYMENT";
            lblRefundPayment.Visible = true;
            lblRefundVoucher.Text = "Payment No.  : " + sBookingId + "/" + sBookingPin;
            lblRefundBookingId.Text = sBookingId;
            lblRefundBookingPin.Text = sBookingPin;
            lblRefundBoatType.Text = sBoatType;
            lblRefundBoatSeater.Text = sBoatSeater;
            lblRefundDate.Text = sBookingDate;
            lblRefundDuration.Text = sBoatDuration + " Mins";
            tblRefundEligible.Visible = true;
            lblRefundNotEligible.Text = "REFUND IS ELIGIBLE";
            lblRefundStartTime.Text = sStartTime;
            lblRefundEndTime.Text = sEndTime;
            lblRefundTotalDuration.Text = sTotalDuration + " Mins ";
            lblBillAmount.Text = "₹" + sBillAmount.Trim();
            lblDepositAmount.Text = "₹" + sDepositAmount.Trim();
            lblExtraCharges.Text = "₹" + sExtraCharge.Trim();
            tblDesignTable.Visible = true;
            tblRefundAmount.Visible = true;
            lblReundAmount.Text = "₹" + sRefundAmount.Trim();
            tblRefundPayment.Visible = true;
            lblRepaymentType.Text = sPayment.Trim();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    // Printing Rights For OtherServices, Restaurant, Additional Ticket

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
        public string RequestedBy { get; set; }
    }

    public class BoatSearch
    {
        public string QueryType { get; set; }
        public string BoatHouseName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string BookingMedia { get; set; }
        public string NoOfPass { get; set; }
        public string NoOfChild { get; set; }
        public string NoOfInfant { get; set; }
        public string InitBillAmount { get; set; }
        public string PaymentType { get; set; }
        public string ActualBillAmount { get; set; }
        public string Status { get; set; }
        public string BoatSeaterId { get; set; }
        public string BookingDuration { get; set; }
        public string InitBoatCharge { get; set; }
        public string InitRowerCharge { get; set; }
        public string BoatDeposit { get; set; }
        public string TaxDetails { get; set; }
        public string InitOfferAmount { get; set; }
        public string InitNetAmount { get; set; }
        public string CreatedBy { get; set; }
        public string OthServiceStatus { get; set; }
        public string BoatHouseId { get; set; }
        public string PremiumStatus { get; set; }
        public string OfferId { get; set; }
        public string BoatTypeId { get; set; }
        public string BookingDate { get; set; }
        public string TaxId { get; set; }
        public string ValidDate { get; set; }
        public string BookingId { get; set; }
        public string BookingType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceId { get; set; }
        public string OthServiceId { get; set; }
        public string OthChargePerItem { get; set; }
        public string OthNoOfItems { get; set; }
        public string OthTaxDetails { get; set; }
        public string OthNetAmount { get; set; }
        public string Bookingpin { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string WebKey { get; set; }
        public string ServiceType { get; set; }

    }

    public class QRBoat
    {
        public string BoatHouseId { get; set; }
        public string BookingId { get; set; }
        public string Pin { get; set; }
        public string BookingRef { get; set; }
    }

    public class QR
    {
        public string ServiceId { get; set; }
        public string BookingId { get; set; }
        public string BookingType { get; set; }
    }

    public class Bookingotherservices
    {
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public string ServiceId { get; set; }
        public string BookingType { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BookingDate { get; set; }
        public string ChargePerItem { get; set; }
        public string NoOfItems { get; set; }
        public string TaxDetails { get; set; }
        public string NetAmount { get; set; }
        public string Createdby { get; set; }
        public string Category { get; set; }
        public string BookingMedia { get; set; }
        public string ServiceType { get; set; }
    }

    public class Ticket
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string BoatHouseId { get; set; }
        public string BookingId { get; set; }
        public string BookingDate { get; set; }
        public string BookingType { get; set; }
        public string UserId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }

    public class OtherBook
    {
        public string BoatHouseId { get; set; }
        public string BookingType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceType { get; set; }
    }

    public class RestaurantBooking
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BoatHouseId { get; set; }
        public string BookingId { get; set; }
        public string BookingType { get; set; }
        public string ServiceFare { get; set; }
        public string BookingDate { get; set; }
        public string TaxAmount { get; set; }
        public string NetAmount { get; set; }
    }

    public class EntranceSummary
    {
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BookingDate { get; set; }
        public string BoatTypeId { get; set; }
        public string Category { get; set; }
        public string PaymentType { get; set; }
        public string CreatedBy { get; set; }

    }
}