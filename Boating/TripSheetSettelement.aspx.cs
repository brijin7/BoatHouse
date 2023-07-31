using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
                // divCusMob.Visible = false;
                int istart;
                int iend;
                AddProcess(0, 10, out istart, out iend);
                SettledAddProcess(0, 10, out istart, out iend);
                txtBookingIdPin.Text = "";


                if (Session["BTMDepositRefund"].ToString().Trim() == "Y")
                {
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        divScanQR.Visible = true;
                        GvBoatBookingTrip.Columns[17].Visible = true;
                        GvBoatBookingTrip.Columns[18].Visible = true;
                        BoatTripCompletedWaitingForDepositRefund();
                        GetPaymentType();
                        lbtnNewRefund.Visible = false;
                        BindAmount();
                        if (Session["UserRole"].ToString().Trim() == "Admin")
                        {
                            getPreviousDayRefundApproved();
                        }
                        else
                        {
                            lbtnLateRefund.Visible = true;
                        }

                    }
                    else
                    {
                        lbtnLateRefund.Visible = false;
                        if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")//Access for Both Scan & pin
                        {
                            divScanQR.Visible = true;
                            GvBoatBookingTrip.Columns[1].Visible = true;
                            BoatTripCompletedWaitingForDepositRefund();
                            GvBoatBookingTrip.Columns[17].Visible = false;
                            GvBoatBookingTrip.Columns[18].Visible = false;
                            GetPaymentType();
                            lbtnNewRefund.Visible = false;
                            BindAmount();
                        }
                        else if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "N")//Access for Scan only
                        {
                            divScanQR.Visible = true;
                            GvBoatBookingTrip.Columns[1].Visible = true;
                            GvBoatBookingTrip.Columns[17].Visible = false;
                            GvBoatBookingTrip.Columns[18].Visible = false;
                            //BoatTripCompletedWaitingForDepositRefund();
                            GetPaymentType();
                            lbtnNewRefund.Visible = false;
                            BindAmount();
                            txtSearchPin.Focus();

                            divRefundPreNext.Visible = false;
                        }

                        else if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "N" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")//Access for Pin only
                        {
                            divScanQR.Visible = false;
                            GvBoatBookingTrip.Columns[1].Visible = true;

                            BoatTripCompletedWaitingForDepositRefund();
                            GvBoatBookingTrip.Columns[17].Visible = false;
                            GvBoatBookingTrip.Columns[18].Visible = false;
                            GetPaymentType();
                            lbtnNewRefund.Visible = false;
                            BindAmount();
                        }
                    }

                }

                txtSearchPin.Focus();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    //public void BoatTripCompletedWaitingForDepositRefund()
    //{
    //    try
    //    {
    //        divalert.Visible = false;

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            var vTripSheetSettlement = new TripSheetSettlement()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BoatHouseName = Session["BoatHouseName"].ToString()
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/getBookingDet", vTripSheetSettlement).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

    //                    ViewState["GvBoatBookingTrip"] = dt;

    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        GvBoatBookingTrip.DataSource = dt;
    //                        GvBoatBookingTrip.DataBind();
    //                        divRefund.Visible = true;
    //                        divGridSettledList.Visible = false;
    //                    }
    //                    else
    //                    {
    //                        GvBoatBookingTrip.DataBind();
    //                        divRefund.Visible = false;
    //                    }
    //                }
    //                else
    //                {
    //                    gvTripSheetSettelement.DataBind();
    //                    divGridSettledList.Visible = false;
    //                    divRefund.Visible = false;

    //                    divalert.Visible = true;
    //                    lblalert.Text = ResponseMsg;
    //                }

    //            }
    //            else
    //            {
    //                var Errorresponse = response.Content.ReadAsStringAsync().Result;
    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}


    public void BoatTripCompletedWaitingForDepositRefund()
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
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString(),
                    CountStart = ViewState["hfstartvalue"].ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/getBookingDetV2", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        ViewState["GvBoatBookingTrip"] = dt;

                        if (dt.Rows.Count > 0)
                        {
                            //GvBoatBookingTrip.DataSource = dt;
                            //GvBoatBookingTrip.DataBind();
                            //divRefund.Visible = true;
                            //divGridSettledList.Visible = false;
                            if (dt.Rows.Count < 10)
                            {
                                RefundNext.Enabled = false;
                            }
                            else
                            {
                                RefundNext.Enabled = true;
                            }


                            if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                            {
                                GvBoatBookingTrip.DataSource = dt;
                                GvBoatBookingTrip.DataBind();
                                divRefund.Visible = true;
                                divScanQR.Visible = true;
                                GvBoatBookingTrip.Visible = true;
                                divGridSettledList.Visible = false;
                                divRefundPreNext.Visible = true;
                            }
                            else
                            {
                                if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y")
                                {
                                    if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")
                                    {
                                        divScanQR.Visible = true;
                                        GvBoatBookingTrip.DataSource = dt;
                                        GvBoatBookingTrip.DataBind();
                                        GvBoatBookingTrip.Visible = true;
                                        divRefund.Visible = true;
                                        divGridSettledList.Visible = false;
                                        divRefundPreNext.Visible = true;
                                    }
                                    else
                                    {
                                        divScanQR.Visible = true;
                                        divRefund.Visible = true;
                                        divGridSettledList.Visible = false;
                                        divRefundPreNext.Visible = false;
                                        txtSearchPin.Focus();
                                    }

                                }
                                else if (Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")
                                {
                                    divScanQR.Visible = false;
                                    GvBoatBookingTrip.DataSource = dt;
                                    GvBoatBookingTrip.DataBind();
                                    divRefund.Visible = true;
                                    GvBoatBookingTrip.Visible = true;
                                    divGridSettledList.Visible = false;
                                }


                            }

                        }
                        else
                        {
                            GvBoatBookingTrip.DataBind();
                            //divRefund.Visible = false;
                            divScanQR.Visible = false;
                            GvBoatBookingTrip.Visible = false;
                            RefundNext.Enabled = false;
                        }
                    }
                    else
                    {
                        gvTripSheetSettelement.DataBind();
                        divGridSettledList.Visible = false;
                        //divRefund.Visible = false;
                        divScanQR.Visible = false;
                        GvBoatBookingTrip.Visible = false;
                        RefundNext.Enabled = false;

                        divalert.Visible = true;
                        lblalert.Text = ResponseMsg;
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

        if ((Convert.ToDecimal(ViewState["CashInHand"]) <= 0))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Cash In Hand Is Less Than Zero !!!');", true);
            return;
        }

        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string bookingId = GvBoatBookingTrip.DataKeys[gvrow.RowIndex].Value.ToString();

        int a = gvrow.RowIndex;
        ViewState["RowIndex"] = a;

        Label lblBookingDate = (Label)gvrow.FindControl("lblBookingDate");
        ViewState["BookingDate"] = lblBookingDate.Text.Trim();

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
        ViewState["ReundPrintStatus"] = lblRefundPrint.Text.Trim();

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

                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
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
                }
                else
                {
                    if (ViewState["ReundPrintStatus"].ToString().Trim() == "N")
                    {
                        string TravelDuration = ViewState["TravelDuration"].ToString();
                        string RefundDuration = ViewState["RefundDuration"].ToString();

                        if (Convert.ToInt32(TravelDuration) <= Convert.ToInt32(RefundDuration))
                        {
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
                        }
                        else
                        {
                            int RowIndex = Convert.ToInt16(ViewState["RowIndex"].ToString());
                            LinkButton lbl = GvBoatBookingTrip.Rows[RowIndex].FindControl("lnkPinNum") as LinkButton;
                            lbl.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Duration Is More than " + RefundDuration.Trim() + " Minutes');", true);
                            return;
                        }
                    }
                    else
                    {
                        string TravelDuration = ViewState["TravelDuration"].ToString();
                        string RefundDuration = ViewState["RefundDuration"].ToString();

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

                    }

                }

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

    protected void lbtnList_Click(object sender, EventArgs e)
    {
        ViewState["hfsettledstartvalue"] = "1";
        ViewState["hfsettledendvalue"] = "10";
        ViewState["hfstartvalue"] = "1";
        ViewState["hfendvalue"] = "10";
        SettledBack.Enabled = false;
        divRefundPreNext.Visible = false;
        BindDepositSettledList();
        lbtnList.Visible = false;
        lbtnNewRefund.Visible = true;
        divRefundPreNext.Visible = false;


    }

    protected void lbtnNewRefund_Click(object sender, EventArgs e)
    {
        lbtnList.Visible = true;
        lbtnNewRefund.Visible = false;
        Refundback.Enabled = false;

        ViewState["hfsettledstartvalue"] = "1";
        ViewState["hfsettledendvalue"] = "10";
        ViewState["hfstartvalue"] = "1";
        ViewState["hfendvalue"] = "10";
        BoatTripCompletedWaitingForDepositRefund();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
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
                    QueryType = "Insert",
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
                    CreatedBy = Session["UserId"].ToString(),
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

                        if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")
                        {
                            BoatTripCompletedWaitingForDepositRefund();
                        }

                        if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "N" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")
                        {
                            BoatTripCompletedWaitingForDepositRefund();
                        }

                        if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "N")
                        {

                            GvBoatBookingTrip.Visible = false;
                            txtSearchPin.Focus();
                        }

                        txtCustomerMobile.Text = "";
                        txtSearchPin.Focus();

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

                        if (Convert.ToInt32(ViewState["TotalDuration"].ToString().Trim()) > Convert.ToInt32(ViewState["Duration"].ToString().Trim()))
                        {
                            if (rrRefundAmount <= 0)
                            {
                                Response.Redirect("~/Boating/Print?rt=rRefss&bi=&BId=" + BookingId.Trim() + "&BPin=" + BookingPin.Trim() + "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + Duration.Trim() + "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BReBillAmo=" + rrBillAmount + "&BReDepositAmo=" + rrDepositAmount + "&BReExtraCharges=" + rrExtraCharge + "&BReAmt=" + rrRefundAmount + " ");
                            }
                            else
                            {
                                Response.Redirect("~/Boating/Print?rt=rRefundEligible&bi=&BId=" + BookingId.Trim() + "&BPin=" + BookingPin.Trim() + "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + Duration.Trim() + "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BReAmt=" + rrRefundAmount + "&BRePay=" + lblRepaymentType.Text + "&BReBillAmo=" + rrBillAmount + "&BReDepositAmo=" + rrDepositAmount + "&BReExtraCharges=" + rrExtraCharge + " ");
                            }
                        }
                        else
                        {
                            // Incase Full amount, that time no need to print out. so blocked.
                            //Response.Redirect("~/Boating/Print?rt=rRefsCorrectDuration&bi=&BId=" + BookingId.Trim() + "&BPin=" + BookingPin.Trim() + "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + Duration.Trim() + "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BReAmt=" + rrRefundAmount + "&BRePay=" + lblRepaymentType.Text + "&BReBillAmo=" + rrBillAmount + "&BReDepositAmo=" + rrDepositAmount + "&BReExtraCharges=" + rrExtraCharge + " ");
                        }
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

            BindAmount();
            txtSearchPin.Focus();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //protected void btnPopUpOkay_Click(object sender, EventArgs e)
    //{
    //    lblStart.Text = "";
    //    MpeTrip.Hide();
    //    txtSearchPin.Focus();
    //}

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

    public void BindDepositSettledList()
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
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    ToDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    CountStart = ViewState["hfsettledstartvalue"].ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/BindListAllV2", vTripSheetSettlement).Result;

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
                            if (dt.Rows.Count < 10)
                            {
                                SettledNext.Enabled = false;
                            }
                            else
                            {
                                SettledNext.Enabled = true;
                            }

                            gvTripSheetSettelement.DataSource = dt;
                            gvTripSheetSettelement.DataBind();
                            gvTripSheetSettelement.Visible = true;
                            divGridSettledList.Visible = true;
                            divRefund.Visible = false;
                            divRefundPreNext.Visible = true;
                        }
                        else
                        {
                            gvTripSheetSettelement.DataBind();
                            //divGridSettledList.Visible = false;
                            gvTripSheetSettelement.Visible = false;
                            SettledNext.Enabled = false;
                        }
                    }
                    else
                    {
                        gvTripSheetSettelement.DataBind();
                        //divGridSettledList.Visible = false;
                        gvTripSheetSettelement.Visible = false;
                        SettledNext.Enabled = false;
                        divRefundPreNext.Visible = false;
                        divRefund.Visible = false;

                        divalert.Visible = true;
                        lblalert.Text = ResponseMsg;
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

    protected void ShowPopup(object sender, EventArgs e)
    {
        string title = "Greetings";
        string body = "Welcome to ASPSnippets.com";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
    }

    protected void txtSearchPin_TextChanged(object sender, EventArgs e)
    {
        if (txtSearchPin.Text.Length != 0)
        {
            lblalert.Text = "";
            string BarCodeEnd = txtSearchPin.Text;
            string[] TimeList = BarCodeEnd.Split(';');

            if (TimeList.Count() == 4)
            {
                //DataTable dtbl = (DataTable)ViewState["GvBoatBookingTrip"];
                //DataRow[] dr1 = dtbl.Select("BookingPin='"+ TimeList[3].ToString() + "'"); 

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString(),
                        BookingId = TimeList[1].ToString(),
                        BookingPin = TimeList[3].ToString()
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/getBookingDetByPin", vTripSheetSettlement).Result;

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
                                GvBoatBookingTrip.Visible = true;
                                GvBoatBookingTrip.DataSource = dt;
                                GvBoatBookingTrip.DataBind();
                                divRefund.Visible = true;
                                divGridSettledList.Visible = false;
                                txtSearchPin.Text = "";

                                // Show popup 

                                divalert.Visible = false;

                                if ((Convert.ToDecimal(ViewState["CashInHand"]) <= 0))
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Cash In Hand Is Less Than Zero !!!');", true);
                                    return;

                                }
                                else
                                {
                                    lblBookingDateDisp.Text = dt.Rows[0]["BookingDate"].ToString();
                                    lblBookingIdDisp.Text = dt.Rows[0]["BookingId"].ToString();
                                    lblInitialBillAmountDisp.Text = dt.Rows[0]["InitNetAmount"].ToString();
                                    lblInitDepositDisp.Text = dt.Rows[0]["Deposit"].ToString();
                                    string lblCustomerMobile = dt.Rows[0]["CustomerMobile"].ToString();
                                    string lblBoatType = dt.Rows[0]["BoatType"].ToString();
                                    string lblBoatSeater = dt.Rows[0]["BoatSeaterName"].ToString();
                                    string lblStartTime = dt.Rows[0]["TripStartTime"].ToString();
                                    string lblEndTime = dt.Rows[0]["TripEndTime"].ToString();
                                    string lblBookingDuration = dt.Rows[0]["BookingDuration"].ToString();

                                    ViewState["BookingDate"] = lblBookingDateDisp.Text.Trim();
                                    ViewState["BoatType"] = lblBoatType.ToString().Trim();
                                    ViewState["BoatSeater"] = lblBoatSeater.ToString().Trim();
                                    ViewState["StartTime"] = lblStartTime.ToString().Trim();
                                    ViewState["EndTime"] = lblEndTime.ToString().Trim();
                                    ViewState["BookingDuration"] = lblBookingDuration.ToString().Trim();

                                    if (lblCustomerMobile != "")
                                    {
                                        txtCustomerMobile.Text = lblCustomerMobile;
                                        txtCustomerMobile.Enabled = false;
                                    }
                                    else
                                    {
                                        txtCustomerMobile.Text = "";
                                        txtCustomerMobile.Enabled = true;
                                    }

                                    ViewState["BoatRefNo"] = dt.Rows[0]["BoatReferenceNo"].ToString();
                                    ViewState["BookingPin"] = dt.Rows[0]["BookingPin"].ToString();
                                    ViewState["PremiumStatus"] = dt.Rows[0]["PremiumStatus"].ToString();
                                    ViewState["BookingId"] = dt.Rows[0]["BookingId"].ToString();
                                    ViewState["TravelDuration"] = dt.Rows[0]["TravelDuration"].ToString();
                                    ViewState["RefundDuration"] = dt.Rows[0]["RefundDuration"].ToString();
                                    //NEW
                                    ViewState["BillAmount"] = dt.Rows[0]["InitNetAmount"].ToString();
                                    ViewState["Deposit"] = dt.Rows[0]["Deposit"].ToString();
                                    ViewState["sPaymentMode"] = dt.Rows[0]["RePaymentType"].ToString();
                                    //New
                                    ViewState["ReundPrintStatus"] = dt.Rows[0]["RStatus"].ToString(); ;
                                    GetCharges(ViewState["BoatRefNo"].ToString().Trim(),
                                        ViewState["PremiumStatus"].ToString(), "GetRefund", ViewState["BookingId"].ToString(), ViewState["BookingPin"].ToString());

                                    txtCustomerMobile.Focus();
                                }
                            }
                            else
                            {
                                GvBoatBookingTrip.DataBind();
                                divRefund.Visible = false;
                            }
                        }
                        else
                        {
                            var vTripSheetSettlement1 = new TripSheetSettlement()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString(),
                                BookingId = TimeList[1].ToString(),
                                BookingPin = TimeList[3].ToString()
                            };

                            HttpResponseMessage response1 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinEnd", vTripSheetSettlement1).Result;

                            if (response1.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse1)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                                if (StatusCode1 == 1)
                                {
                                    DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                                    if (dt1.Rows.Count > 0)
                                    {
                                        //lblStart.Text = "Booking pin : " + TimeList[3].ToString().Trim() + " is not Ended Still !!!";
                                        //MpeTrip1.Show();
                                        //txtSearchPin.Text = "";
                                        //ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                        lblEnd.Text = " Trip Not Ended Still !!!";
                                        lblPopBookingId.Text = dt1.Rows[0]["BookingId"].ToString().Trim();
                                        lblPopBookingPin.Text = dt1.Rows[0]["BookingPin"].ToString().Trim();
                                        lblStartTime.Text = dt1.Rows[0]["TripStartTime"].ToString().Trim();

                                        ViewState["BookingId"] = dt1.Rows[0]["BookingId"].ToString().Trim();
                                        ViewState["BookingPinExtn"] = dt1.Rows[0]["BookingPin"].ToString().Trim();
                                        ViewState["TripStartTime"] = dt1.Rows[0]["TripStartTime"].ToString().Trim();

                                        ViewState["BoatReferenceNo"] = dt1.Rows[0]["BoatReferenceNo"].ToString().Trim();
                                        ViewState["BoatId"] = dt1.Rows[0]["BoatId"].ToString().Trim();
                                        ViewState["Rower"] = dt1.Rows[0]["RowerId"].ToString().Trim();

                                        ViewState["BookingDuration"] = dt1.Rows[0]["BookingDuration"].ToString().Trim();
                                        ViewState["BoatTypeIdExtn"] = dt1.Rows[0]["BoatTypeId"].ToString().Trim();
                                        ViewState["BoatSeaterIdExtn"] = dt1.Rows[0]["BoatSeaterId"].ToString().Trim();
                                        ViewState["BoatTypeExtn"] = dt1.Rows[0]["BoatType"].ToString().Trim();
                                        ViewState["BoatSeaterExtn"] = dt1.Rows[0]["BoatSeaterName"].ToString().Trim();

                                        MpeTrip1.Show();
                                        txtSearchPin.Text = "";
                                        ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                                    }
                                }
                                else
                                {
                                    var vTripSheetSettlement2 = new TripSheetSettlement()
                                    {
                                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                        BoatHouseName = Session["BoatHouseName"].ToString(),
                                        BookingId = TimeList[1].ToString(),
                                        BookingPin = TimeList[3].ToString()
                                    };

                                    HttpResponseMessage response2 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinNotStarted", vTripSheetSettlement2).Result;

                                    if (response2.IsSuccessStatusCode)
                                    {
                                        var vehicleEditresponse2 = response2.Content.ReadAsStringAsync().Result;
                                        int StatusCode2 = Convert.ToInt32(JObject.Parse(vehicleEditresponse2)["StatusCode"].ToString());
                                        string ResponseMsg2 = JObject.Parse(vehicleEditresponse2)["Response"].ToString();

                                        if (StatusCode2 == 1)
                                        {
                                            DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg2);

                                            if (dt2.Rows.Count > 0)
                                            {
                                                gvTripSheetSettelement.DataBind();
                                                divGridSettledList.Visible = false;
                                                divRefund.Visible = true;

                                                divalert.Visible = true;
                                                //lblStart.Text = "Booking Pin : " + TimeList[3].ToString().Trim() + " is Not Started Still !!!";  SILLU 20 April 2022
                                                lblStart.Text = "<span style='color:red'> Trip Not Started Still !!! </span> <hr><br/> <span style='color:DarkBlue'> Booking Id : " + TimeList[1].ToString() + "<br> Booking Pin : " + TimeList[3].ToString().Trim() + " <br> is Not Started Still !!!  </span>";

                                                MpeTrip.Show();
                                                txtSearchPin.Text = "";
                                                ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                                divScanQR.Visible = true;
                                                txtSearchPin.Text = "";
                                                txtSearchPin.Focus();
                                            }
                                        }
                                        else
                                        {
                                            var vTripSheetSettlement3 = new TripSheetSettlement()
                                            {
                                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                                BoatHouseName = Session["BoatHouseName"].ToString(),
                                                BookingId = TimeList[1].ToString(),
                                                BookingPin = TimeList[3].ToString()
                                            };

                                            HttpResponseMessage response3 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinAlreadyRefunded", vTripSheetSettlement).Result;

                                            if (response3.IsSuccessStatusCode)
                                            {
                                                var vehicleEditresponse3 = response3.Content.ReadAsStringAsync().Result;
                                                int StatusCode3 = Convert.ToInt32(JObject.Parse(vehicleEditresponse3)["StatusCode"].ToString());
                                                string ResponseMsg3 = JObject.Parse(vehicleEditresponse3)["Response"].ToString();

                                                if (StatusCode3 == 1)
                                                {
                                                    DataTable dt3 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg3);

                                                    if (dt3.Rows.Count > 0)
                                                    {
                                                        gvTripSheetSettelement.DataBind();
                                                        divGridSettledList.Visible = false;
                                                        divRefund.Visible = true;

                                                        divalert.Visible = true;
                                                        lblStart.Text = "<span style='color:red;'>Deposit Already Refunded !!!<span><hr><span style='color:DarkBlue;'> Booking Id / Pin : " + dt3.Rows[0]["BookingId"].ToString().Trim() + " / " + dt3.Rows[0]["BookingPin"].ToString().Trim() + "<br> Refund Amount : " + dt3.Rows[0]["DepRefundAmount"].ToString() + "<br> Trip Start : " + dt3.Rows[0]["TripStartTime"] + "<br> Trip End : " + dt3.Rows[0]["TripEndTime"] + "<br> Refunded By : " + dt3.Rows[0]["DepositRefundBy"] + "<br> Refunded Date : " + dt3.Rows[0]["DepRefundDate"] + "</span>";
                                                        MpeTrip.Show();
                                                        txtSearchPin.Text = "";
                                                        ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                                        divScanQR.Visible = true;
                                                        txtSearchPin.Text = "";
                                                        txtSearchPin.Focus();
                                                    }
                                                }
                                                else
                                                {
                                                    lblStart.Text = "<span style='color:red'> Invalid Pin </span> <hr> <span style='color:DarkBlue'> <br> Please Scan Valid QR Code !!!  </span>";
                                                    MpeTrip.Show();
                                                    ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                                    txtSearchPin.Text = "";
                                                    txtSearchPin.Focus();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var Errorresponse = response.Content.ReadAsStringAsync().Result;
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                            }
                        }
                    }
                    else
                    {
                        lblSearchPinResponse.Text = "Invalid QRCode";
                        txtSearchPin.Text = "";
                        txtSearchPin.Focus();
                    }
                }
            }
            else
            {
                ///SILLU 21 April 2022
                if (Session["BTMDepositRefundScanPin"].ToString().Trim() == "N")///Check Pin Access, if 'N', then No Access so should not check
                {
                    txtSearchPin.Text = "";
                }
                else
                {
                    if (txtSearchPin.Text.Length != 0 && txtSearchPin.Text.Length >= 4)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Clear();

                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var vTripSheetSettlement = new TripSheetSettlement()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString(),
                                BookingIdORPin = txtSearchPin.Text.Trim()

                            };

                            HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/getBookingDetByPinORIdV2", vTripSheetSettlement).Result;

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
                                        if (dt.Rows.Count < 10)
                                        {
                                            RefundNext.Enabled = false;
                                        }
                                        else
                                        {
                                            RefundNext.Enabled = true;
                                        }
                                        GvBoatBookingTrip.Visible = true;
                                        GvBoatBookingTrip.DataSource = dt;
                                        GvBoatBookingTrip.DataBind();
                                        divRefund.Visible = true;
                                        divGridSettledList.Visible = false;
                                        txtSearchPin.Text = "";

                                        // Show popup 

                                        divalert.Visible = false;

                                        if ((Convert.ToDecimal(ViewState["CashInHand"]) <= 0))
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Cash In Hand Is Less Than Zero !!!');", true);
                                            return;

                                        }
                                        else
                                        {
                                            lblBookingDateDisp.Text = dt.Rows[0]["BookingDate"].ToString();
                                            lblBookingIdDisp.Text = dt.Rows[0]["BookingId"].ToString();
                                            lblInitialBillAmountDisp.Text = dt.Rows[0]["InitNetAmount"].ToString();
                                            lblInitDepositDisp.Text = dt.Rows[0]["Deposit"].ToString();
                                            string lblCustomerMobile = dt.Rows[0]["CustomerMobile"].ToString();
                                            string lblBoatType = dt.Rows[0]["BoatType"].ToString();
                                            string lblBoatSeater = dt.Rows[0]["BoatSeaterName"].ToString();
                                            string lblStartTime = dt.Rows[0]["TripStartTime"].ToString();
                                            string lblEndTime = dt.Rows[0]["TripEndTime"].ToString();
                                            string lblBookingDuration = dt.Rows[0]["BookingDuration"].ToString();

                                            ViewState["BookingDate"] = lblBookingDateDisp.Text.Trim();
                                            ViewState["BoatType"] = lblBoatType.ToString().Trim();
                                            ViewState["BoatSeater"] = lblBoatSeater.ToString().Trim();
                                            ViewState["StartTime"] = lblStartTime.ToString().Trim();
                                            ViewState["EndTime"] = lblEndTime.ToString().Trim();
                                            ViewState["BookingDuration"] = lblBookingDuration.ToString().Trim();

                                            if (lblCustomerMobile != "")
                                            {
                                                txtCustomerMobile.Text = lblCustomerMobile;
                                                txtCustomerMobile.Enabled = false;
                                            }
                                            else
                                            {
                                                txtCustomerMobile.Text = "";
                                                txtCustomerMobile.Enabled = true;
                                            }

                                            ViewState["BoatRefNo"] = dt.Rows[0]["BoatReferenceNo"].ToString();
                                            ViewState["BookingPin"] = dt.Rows[0]["BookingPin"].ToString();
                                            ViewState["PremiumStatus"] = dt.Rows[0]["PremiumStatus"].ToString();
                                            ViewState["BookingId"] = dt.Rows[0]["BookingId"].ToString();
                                            ViewState["TravelDuration"] = dt.Rows[0]["TravelDuration"].ToString();
                                            ViewState["RefundDuration"] = dt.Rows[0]["RefundDuration"].ToString();
                                            //NEW
                                            ViewState["BillAmount"] = dt.Rows[0]["InitNetAmount"].ToString();
                                            ViewState["Deposit"] = dt.Rows[0]["Deposit"].ToString();
                                            ViewState["sPaymentMode"] = dt.Rows[0]["RePaymentType"].ToString();
                                            //New
                                            ViewState["ReundPrintStatus"] = dt.Rows[0]["RStatus"].ToString();
                                            //SILLU
                                            //GetCharges(ViewState["BoatRefNo"].ToString().Trim(),
                                            //    ViewState["PremiumStatus"].ToString(), "GetRefund", ViewState["BookingId"].ToString(), ViewState["BookingPin"].ToString());
                                            //SILLU
                                            txtCustomerMobile.Focus();
                                        }
                                    }
                                    else
                                    {
                                        GvBoatBookingTrip.DataBind();
                                        divRefund.Visible = false;
                                    }
                                }
                                else
                                {
                                    var vTripSheetSettlement1 = new TripSheetSettlement()
                                    {
                                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                        BoatHouseName = Session["BoatHouseName"].ToString(),
                                        BookingIdORPin = txtSearchPin.Text.Trim()
                                    };

                                    HttpResponseMessage response1 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinORIdEndV2", vTripSheetSettlement1).Result;

                                    if (response1.IsSuccessStatusCode)
                                    {
                                        var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                        int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse1)["StatusCode"].ToString());
                                        string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                                        if (StatusCode1 == 1)
                                        {
                                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                                            if (dt1.Rows.Count > 0)
                                            {
                                                lblEnd.Text = " Trip Not Ended Still !!!";
                                                lblPopBookingId.Text = dt1.Rows[0]["BookingId"].ToString().Trim();
                                                lblPopBookingPin.Text = dt1.Rows[0]["BookingPin"].ToString().Trim();
                                                lblStartTime.Text = dt1.Rows[0]["TripStartTime"].ToString().Trim();

                                                ViewState["BookingId"] = dt1.Rows[0]["BookingId"].ToString().Trim();
                                                ViewState["BookingPinExtn"] = dt1.Rows[0]["BookingPin"].ToString().Trim();
                                                ViewState["TripStartTime"] = dt1.Rows[0]["TripStartTime"].ToString().Trim();

                                                ViewState["BoatReferenceNo"] = dt1.Rows[0]["BoatReferenceNo"].ToString().Trim();
                                                ViewState["BoatId"] = dt1.Rows[0]["BoatId"].ToString().Trim();
                                                ViewState["Rower"] = dt1.Rows[0]["RowerId"].ToString().Trim();

                                                ViewState["BookingDuration"] = dt1.Rows[0]["BookingDuration"].ToString().Trim();
                                                ViewState["BoatTypeIdExtn"] = dt1.Rows[0]["BoatTypeId"].ToString().Trim();
                                                ViewState["BoatSeaterIdExtn"] = dt1.Rows[0]["BoatSeaterId"].ToString().Trim();
                                                ViewState["BoatTypeExtn"] = dt1.Rows[0]["BoatType"].ToString().Trim();
                                                ViewState["BoatSeaterExtn"] = dt1.Rows[0]["BoatSeaterName"].ToString().Trim();

                                                MpeTrip1.Show();
                                                txtSearchPin.Text = "";
                                                ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                                            }
                                        }
                                        else
                                        {
                                            var vTripSheetSettlement2 = new TripSheetSettlement()
                                            {
                                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                                BoatHouseName = Session["BoatHouseName"].ToString(),
                                                BookingIdORPin = txtSearchPin.Text.Trim()
                                            };

                                            HttpResponseMessage response2 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinORIdNotStartedV2", vTripSheetSettlement2).Result;

                                            if (response2.IsSuccessStatusCode)
                                            {
                                                var vehicleEditresponse2 = response2.Content.ReadAsStringAsync().Result;
                                                int StatusCode2 = Convert.ToInt32(JObject.Parse(vehicleEditresponse2)["StatusCode"].ToString());
                                                string ResponseMsg2 = JObject.Parse(vehicleEditresponse2)["Response"].ToString();

                                                if (StatusCode2 == 1)
                                                {
                                                    DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg2);

                                                    if (dt2.Rows.Count > 0)
                                                    {
                                                        gvTripSheetSettelement.DataBind();
                                                        divGridSettledList.Visible = false;
                                                        divRefund.Visible = true;

                                                        divalert.Visible = true;
                                                        //lblStart.Text = "Booking : " + txtSearchPin.Text.Trim() + " is Not Started Still !!!";
                                                        lblStart.Text = "<span style='color:red'> Trip Not Started Still !!! <span><hr><br/><span style='color:DarkBlue'> Booking Id : " + dt2.Rows[0]["BookingId"].ToString().Trim() + "<br> Booking Pin : " + dt2.Rows[0]["BookingPin"].ToString().Trim() + " is Not Started Still !!!  </span>";

                                                        MpeTrip.Show();
                                                        txtSearchPin.Text = "";
                                                        ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                                        divScanQR.Visible = true;
                                                        txtSearchPin.Text = "";
                                                        txtSearchPin.Focus();
                                                    }
                                                }
                                                else
                                                {
                                                    var vTripSheetSettlement3 = new TripSheetSettlement()
                                                    {
                                                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                                        BoatHouseName = Session["BoatHouseName"].ToString(),
                                                        BookingIdORPin = txtSearchPin.Text.Trim()
                                                    };

                                                    HttpResponseMessage response3 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinORAlreadyRefundedV2", vTripSheetSettlement).Result;

                                                    if (response3.IsSuccessStatusCode)
                                                    {
                                                        var vehicleEditresponse3 = response3.Content.ReadAsStringAsync().Result;
                                                        int StatusCode3 = Convert.ToInt32(JObject.Parse(vehicleEditresponse3)["StatusCode"].ToString());
                                                        string ResponseMsg3 = JObject.Parse(vehicleEditresponse3)["Response"].ToString();

                                                        if (StatusCode3 == 1)
                                                        {
                                                            DataTable dt3 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg3);

                                                            if (dt3.Rows.Count > 0)
                                                            {
                                                                gvTripSheetSettelement.DataBind();
                                                                divGridSettledList.Visible = false;
                                                                divRefund.Visible = true;

                                                                divalert.Visible = true;
                                                                lblStart.Text = "<span style='color:red;'>Deposit Already Refunded !!!<span><hr><span style='color:DarkBlue;'> Booking Id / Pin : " + dt3.Rows[0]["BookingId"].ToString().Trim() + " / " + dt3.Rows[0]["BookingPin"].ToString().Trim() + "<br> Refund Amount : " + dt3.Rows[0]["DepRefundAmount"].ToString() + "<br> Trip Start : " + dt3.Rows[0]["TripStartTime"] + "<br> Trip End : " + dt3.Rows[0]["TripEndTime"] + "<br> Refunded By : " + dt3.Rows[0]["DepositRefundBy"] + "<br> Refunded Date : " + dt3.Rows[0]["DepRefundDate"] + "</span>";
                                                                //lblalert.Text = ResponseMsg;
                                                                MpeTrip.Show();
                                                                txtSearchPin.Text = "";
                                                                ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                                                divScanQR.Visible = true;
                                                                txtSearchPin.Text = "";
                                                                txtSearchPin.Focus();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            lblStart.Text = "<span style='color:red'> Invalid Pin Or Id </span> <hr> <span style='color:DarkBlue'> <br> Please Enter Valid Pin Or Id !!!  </span>";
                                                            MpeTrip.Show();
                                                            ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                                                            BoatTripCompletedWaitingForDepositRefund();
                                                            txtSearchPin.Text = "";
                                                            txtSearchPin.Focus();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var Errorresponse = response.Content.ReadAsStringAsync().Result;
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                BoatTripCompletedWaitingForDepositRefund();
                                lblSearchPinResponse.Text = "Invalid Pin / Booking Id";
                                txtSearchPin.Text = "";
                                txtSearchPin.Focus();
                            }
                        }

                    }
                    else
                    {
                        BoatTripCompletedWaitingForDepositRefund();
                        txtSearchPin.Text = "";
                        txtSearchPin.Focus();
                    }
                }

            }
        }
    }


    public void BindNetDeposit()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    ToDate = DateTime.Now.ToString("yyyy-MM-dd")
                };

                HttpResponseMessage response = client.PostAsJsonAsync("DepositRefundCash/NetTotal", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        dtAmount = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {

                        }

                    }
                    else
                    {
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

    public void BindNetRefundTotal()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    ToDate = DateTime.Now.ToString("yyyy-MM-dd")
                };

                HttpResponseMessage response = client.PostAsJsonAsync("DepositRefundCashInHand", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        dtTotal = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        dtTotal.Columns.Add("CorpLogo", typeof(byte[]));
                        string CorpLogo = Session["CorpLogo"].ToString();
                        using (var webClient = new WebClient())
                        {
                            byte[] imageBytes = webClient.DownloadData(CorpLogo);
                            dtTotal.Rows[0]["CorpLogo"] = (imageBytes);
                        }
                      
                    }
                    else
                    {
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

    protected void btnPdf_Click(object sender, EventArgs e)
    {
        BindDepositRefundReport();
    }

    public void BindDepositRefundReport()
    {
        BindNetDeposit();
        BindNetRefundTotal();
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    ToDate = DateTime.Now.ToString("yyyy-MM-dd")
                };

                HttpResponseMessage response = client.PostAsJsonAsync("DepositRefundCash/RPT", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        DataSet dstReports = new DepositRefund();

                        dstReports.Tables["DepositRefund"].Merge(dt);
                        dstReports.Tables["DepositNetCash"].Merge(dtAmount);
                        dstReports.Tables["Total"].Merge(dtTotal);
                        objReportDocument.Load(Server.MapPath("/Reports/DepositRefundRpt.rpt"));
                        objReportDocument.SetDataSource(dstReports);
                        objReportDocument.SetParameterValue(0, Session["CorpName"].ToString());
                        objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                        objReportExport = objReportDocument.ExportOptions;
                        objReportExport.ExportDestinationOptions = objReportDiskOption;
                        objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                        objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                        objReportDocument.Export();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(Server.MapPath(sFilePath));
                        Response.Flush();

                    }
                    else
                    {
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

        finally
        {
            if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
            {
                System.IO.File.Delete(Server.MapPath(sFilePath));
            }

            objReportDocument.Dispose();

            objReportDiskOption = null;
            objReportDocument = null;
            objReportExport = null;
            GC.Collect();
        }


    }

    public void BindAmount()
    {
        try
        {
            ViewState["CashInHand"] = "0";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    QueryType = "TripSheetSummaryDetails",
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = Session["UserId"].ToString(),
                    Input5 = "DepositRefundUser"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var GetResponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                    DataTable dtDtl = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                    var ResponseMsg = JObject.Parse(GetResponse)["Table1"].ToString();
                    DataTable dtDpst = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    var ResponseMsgCnt = JObject.Parse(GetResponse)["Table2"].ToString();
                    DataTable dtCnt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgCnt);

                    if (dtDtl.Rows.Count > 0 || dtDpst.Rows.Count > 0 || dtCnt.Rows.Count > 0)
                    {
                        decimal dClaimedDeposit = 0, sCashInHand = 0, dReceivedAmt = 0, dBalanceRefund = 0, dTotalRefund = 0, dCollectedDeposit = 0;

                        for (int i = 0; i < dtDtl.Rows.Count; i++)
                        {
                            string F = dtDtl.Rows[i].ToString();
                            dClaimedDeposit += decimal.Parse(dtDtl.Rows[i]["ClaimedDeposit"].ToString());
                        }
                        //SILLI 05 Nov
                        for (int i = 0; i < dtDpst.Rows.Count; i++)
                        {
                            string d = dtDpst.Rows[i].ToString();
                            dCollectedDeposit += decimal.Parse(dtDpst.Rows[i]["Deposit"].ToString());
                        }
                        //SILLI 05 Nov
                        if (dtCnt.Rows.Count > 0)
                        {
                            dReceivedAmt = decimal.Parse(dtCnt.Rows[0]["ReceivedAmount"].ToString());
                            bblblReceivedAmt.Text = dtCnt.Rows[0]["ReceivedAmount"].ToString();

                            dBalanceRefund = Convert.ToDecimal(dtCnt.Rows[0]["BalanceRefund"].ToString());
                        }
                        else
                        {
                            bblblReceivedAmt.Text = "0";
                        }

                        dTotalRefund = dClaimedDeposit + dBalanceRefund;

                        bblblRefundAmt.Text = dTotalRefund.ToString();
                        sCashInHand = (dCollectedDeposit + dReceivedAmt) - dTotalRefund;//SILLI 05 Nov
                        bblblCashInHand.Text = sCashInHand.ToString();

                        ViewState["CashInHand"] = sCashInHand.ToString();
                    }
                    else
                    {
                        bblblReceivedAmt.Text = "0";
                        bblblRefundAmt.Text = "0";
                        bblblCashInHand.Text = "0";
                    }
                }
                else
                {
                    bblblReceivedAmt.Text = "0";
                    bblblRefundAmt.Text = "0";
                    bblblCashInHand.Text = "0";
                }
            }

            txtSearchPin.Focus();
        }
        catch (Exception ex)
        {
            bblblReceivedAmt.Text = "0";
            bblblRefundAmt.Text = "0";
            bblblCashInHand.Text = "0";

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnSMS_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string bookingId = GvBoatBookingTrip.DataKeys[gvrow.RowIndex].Value.ToString();

        int a = gvrow.RowIndex;
        ViewState["RowIndex"] = a;

        Label lblBookingDate = (Label)gvrow.FindControl("lblBookingDate");
        ViewState["BookingDate"] = lblBookingDate.Text.Trim();

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

        ViewState["BoatRefNo"] = lblBoatRefNo.Text.Trim();
        ViewState["BookingPin"] = lnkBookingPin.Text.Trim();
        ViewState["PremiumStatus"] = lblPremiumStatus.Text.Trim();
        ViewState["BookingId"] = lblBookingId.Text.Trim();
        ViewState["TravelDuration"] = lblTravelDuration.Text.Trim();
        ViewState["RefundDuration"] = lblRefundDuration.Text.Trim();
        if (lblCustomerMobile.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "showModalMob();", true);

        }
        else
        {
            // MpeUpdateSlot.Hide();
            SendSMSPrint("RefundRePrint", lblCustomerMobile.Text.Trim(), lblBookingId.Text.Trim(), lnkBookingPin.Text.Trim());
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Ticket Details Sended Successfully');", true);
        }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //if(txtGridMobileNum.Text.Length < 10)
        // {
        //     txtGridMobileNum.Text = string.Empty;
        //     ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "showModalMob();", true);
        //     lblinfomsg.Visible = true;
        //     return;


        // }
        // else
        // {
        //     lblinfomsg.Visible = false;

        // }

        UpdateCusmobForSms(ViewState["BookingId"].ToString().Trim());

    }

    public void SendSMSPrint(string ServiceType, string sMobileNo, string BookingId, string sAmount)
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
                        BookingId = BookingId.Trim(),
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
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Ticket Details Sended Successfully');", true);
                            BoatTripCompletedWaitingForDepositRefund();
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
    private void UpdateCusmobForSms(string sBookingId)
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
                    QueryType = "RefundMobileNoUpdate",
                    ServiceType = "",
                    Input1 = txtGridMobileNum.Text.Trim(),
                    Input2 = sBookingId,
                    Input3 = "",
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

                    GetCusMobSms(sBookingId);
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

    private void GetCusMobSms(string sBookingId)
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
                    QueryType = "GetCusMobSms",
                    ServiceType = "",
                    Input1 = sBookingId.Trim(),
                    Input2 = "",
                    Input3 = "",
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
                        string CusMobNUmber = dtExists.Rows[0]["CustomerMobile"].ToString().Trim();
                        SendSMSPrint("RefundRePrint", CusMobNUmber.Trim(), ViewState["BookingId"].ToString().Trim(), ViewState["BookingPin"].ToString().Trim());
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

    protected void ImgClose_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hideModalMob();", true);
        txtGridMobileNum.Text = "";
    }

    protected void RefundNext_Click(object sender, EventArgs e)
    {
        Refundback.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BoatTripCompletedWaitingForDepositRefund();
    }

    protected void Refundback_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BoatTripCompletedWaitingForDepositRefund();
    }

    protected void AddProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = start + 10;

        }
        else
        {
            iend = end;

        }
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
    }
    protected void subProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
            Refundback.Enabled = false;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            Refundback.Enabled = false;

        }
        else
        {
            iend = end;

        }
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
    }


    protected void SettledNext_Click(object sender, EventArgs e)
    {

        SettledBack.Enabled = true;
        int istart;
        int iend;
        SettledAddProcess(Int32.Parse(ViewState["hfsettledendvalue"].ToString()) + 1, Int32.Parse(ViewState["hfsettledendvalue"].ToString()) + 10, out istart, out iend);
        ViewState["hfsettledstartvalue"] = istart.ToString();
        ViewState["hfsettledendvalue"] = iend.ToString();
        BindDepositSettledList();
    }
    protected void SettledBack_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        SettledsubProcess(Int32.Parse(ViewState["hfsettledstartvalue"].ToString()) - 10, Int32.Parse(ViewState["hfsettledendvalue"].ToString()) - 10, out istart, out iend);
        ViewState["hfsettledstartvalue"] = istart.ToString();
        ViewState["hfsettledendvalue"] = iend.ToString();
        BindDepositSettledList();
    }


    protected void SettledAddProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = start + 10;

        }
        else
        {
            iend = end;

        }
        ViewState["hfsettledstartvalue"] = istart.ToString();
        ViewState["hfsettledendvalue"] = iend.ToString();
    }
    protected void SettledsubProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
            SettledBack.Enabled = false;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            SettledBack.Enabled = false;

        }
        else
        {
            iend = end;

        }
        ViewState["hfsettledstartvalue"] = istart.ToString();
        ViewState["hfsettledendvalue"] = iend.ToString();
    }


    /// <summary>
    /// Developed By Silambarasu D
    /// Developed By 18 April 2022
    /// NOT used , same process done in Scan qr itself
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtBookingIdPin_TextChanged(object sender, EventArgs e)
    {
        if (txtBookingIdPin.Text.Length != 0 && txtBookingIdPin.Text.Length >= 4)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString(),
                    BookingIdORPin = txtBookingIdPin.Text

                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/getBookingDetByPinORIdV2", vTripSheetSettlement).Result;

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
                            if (dt.Rows.Count < 10)
                            {
                                RefundNext.Enabled = false;
                            }
                            else
                            {
                                RefundNext.Enabled = true;
                            }
                            GvBoatBookingTrip.Visible = true;
                            GvBoatBookingTrip.DataSource = dt;
                            GvBoatBookingTrip.DataBind();
                            divRefund.Visible = true;
                            divGridSettledList.Visible = false;
                            txtBookingIdPin.Text = "";

                            // Show popup 

                            divalert.Visible = false;

                            if ((Convert.ToDecimal(ViewState["CashInHand"]) <= 0))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Cash In Hand Is Less Than Zero !!!');", true);
                                return;

                            }
                            else
                            {
                                lblBookingDateDisp.Text = dt.Rows[0]["BookingDate"].ToString();
                                lblBookingIdDisp.Text = dt.Rows[0]["BookingId"].ToString();
                                lblInitialBillAmountDisp.Text = dt.Rows[0]["InitNetAmount"].ToString();
                                lblInitDepositDisp.Text = dt.Rows[0]["Deposit"].ToString();
                                string lblCustomerMobile = dt.Rows[0]["CustomerMobile"].ToString();
                                string lblBoatType = dt.Rows[0]["BoatType"].ToString();
                                string lblBoatSeater = dt.Rows[0]["BoatSeaterName"].ToString();
                                string lblStartTime = dt.Rows[0]["TripStartTime"].ToString();
                                string lblEndTime = dt.Rows[0]["TripEndTime"].ToString();
                                string lblBookingDuration = dt.Rows[0]["BookingDuration"].ToString();

                                ViewState["BookingDate"] = lblBookingDateDisp.Text.Trim();
                                ViewState["BoatType"] = lblBoatType.ToString().Trim();
                                ViewState["BoatSeater"] = lblBoatSeater.ToString().Trim();
                                ViewState["StartTime"] = lblStartTime.ToString().Trim();
                                ViewState["EndTime"] = lblEndTime.ToString().Trim();
                                ViewState["BookingDuration"] = lblBookingDuration.ToString().Trim();

                                if (lblCustomerMobile != "")
                                {
                                    txtCustomerMobile.Text = lblCustomerMobile;
                                    txtCustomerMobile.Enabled = false;
                                }
                                else
                                {
                                    txtCustomerMobile.Text = "";
                                    txtCustomerMobile.Enabled = true;
                                }

                                ViewState["BoatRefNo"] = dt.Rows[0]["BoatReferenceNo"].ToString();
                                ViewState["BookingPin"] = dt.Rows[0]["BookingPin"].ToString();
                                ViewState["PremiumStatus"] = dt.Rows[0]["PremiumStatus"].ToString();
                                ViewState["BookingId"] = dt.Rows[0]["BookingId"].ToString();
                                ViewState["TravelDuration"] = dt.Rows[0]["TravelDuration"].ToString();
                                ViewState["RefundDuration"] = dt.Rows[0]["RefundDuration"].ToString();
                                //NEW
                                ViewState["BillAmount"] = dt.Rows[0]["InitNetAmount"].ToString();
                                ViewState["Deposit"] = dt.Rows[0]["Deposit"].ToString();
                                ViewState["sPaymentMode"] = dt.Rows[0]["RePaymentType"].ToString();
                                //New
                                ViewState["ReundPrintStatus"] = dt.Rows[0]["RStatus"].ToString();
                                //SILLU
                                //GetCharges(ViewState["BoatRefNo"].ToString().Trim(),
                                //    ViewState["PremiumStatus"].ToString(), "GetRefund", ViewState["BookingId"].ToString(), ViewState["BookingPin"].ToString());
                                //SILLU
                                txtCustomerMobile.Focus();
                            }
                        }
                        else
                        {
                            GvBoatBookingTrip.DataBind();
                            divRefund.Visible = false;
                        }
                    }
                    else
                    {
                        var vTripSheetSettlement1 = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BoatHouseName = Session["BoatHouseName"].ToString(),
                            BookingIdORPin = txtBookingIdPin.Text
                        };

                        HttpResponseMessage response1 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinORIdEndV2", vTripSheetSettlement1).Result;

                        if (response1.IsSuccessStatusCode)
                        {
                            var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                            int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse1)["StatusCode"].ToString());
                            string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                            if (StatusCode1 == 1)
                            {
                                DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                                if (dt1.Rows.Count > 0)
                                {
                                    lblStart.Text = "Booking : " + txtBookingIdPin.Text.Trim() + " is not Ended Still !!!";
                                    MpeTrip.Show();
                                    txtSearchPin.Text = "";
                                    ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                                }
                            }
                            else
                            {
                                var vTripSheetSettlement2 = new TripSheetSettlement()
                                {
                                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                    BoatHouseName = Session["BoatHouseName"].ToString(),
                                    BookingIdORPin = txtBookingIdPin.Text
                                };

                                HttpResponseMessage response2 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinORIdNotStartedV2", vTripSheetSettlement2).Result;

                                if (response2.IsSuccessStatusCode)
                                {
                                    var vehicleEditresponse2 = response2.Content.ReadAsStringAsync().Result;
                                    int StatusCode2 = Convert.ToInt32(JObject.Parse(vehicleEditresponse2)["StatusCode"].ToString());
                                    string ResponseMsg2 = JObject.Parse(vehicleEditresponse2)["Response"].ToString();

                                    if (StatusCode2 == 1)
                                    {
                                        DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg2);

                                        if (dt2.Rows.Count > 0)
                                        {
                                            gvTripSheetSettelement.DataBind();
                                            divGridSettledList.Visible = false;
                                            divRefund.Visible = true;

                                            divalert.Visible = true;
                                            //lblStart.Text = "Booking : " + txtBookingIdPin.Text.Trim() + " is Not Started Still !!!";
                                            lblStart.Text = "<span style='color:red'> Trip Not Started Still !!! <span> <hr> <br/><span style='color:DarkBlue'> Booking Id : " + dt2.Rows[0]["BookingId"].ToString().Trim() + "<br> Booking Pin : " + dt2.Rows[0]["BookingPin"].ToString().Trim() + " is Not Started Still !!!  </span>";


                                            MpeTrip.Show();
                                            txtSearchPin.Text = "";
                                            ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                            divScanQR.Visible = true;
                                            txtSearchPin.Text = "";
                                            txtSearchPin.Focus();
                                        }
                                    }
                                    else
                                    {
                                        var vTripSheetSettlement3 = new TripSheetSettlement()
                                        {
                                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                            BoatHouseName = Session["BoatHouseName"].ToString(),
                                            BookingIdORPin = txtBookingIdPin.Text
                                        };

                                        HttpResponseMessage response3 = client.PostAsJsonAsync("TripSheet/getBookingDetByPinORAlreadyRefundedV2", vTripSheetSettlement).Result;

                                        if (response3.IsSuccessStatusCode)
                                        {
                                            var vehicleEditresponse3 = response3.Content.ReadAsStringAsync().Result;
                                            int StatusCode3 = Convert.ToInt32(JObject.Parse(vehicleEditresponse3)["StatusCode"].ToString());
                                            string ResponseMsg3 = JObject.Parse(vehicleEditresponse3)["Response"].ToString();

                                            if (StatusCode3 == 1)
                                            {
                                                DataTable dt3 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg3);

                                                if (dt3.Rows.Count > 0)
                                                {
                                                    gvTripSheetSettelement.DataBind();
                                                    divGridSettledList.Visible = false;
                                                    divRefund.Visible = true;

                                                    divalert.Visible = true;
                                                    lblStart.Text = "<span style='color:red;'>Deposit Already Refunded !!!<span><hr><span style='color:DarkBlue;'> Booking Id / Pin : " + dt3.Rows[0]["BookingId"].ToString().Trim() + " / " + dt3.Rows[0]["BookingPin"].ToString().Trim() + "<br> Refund Amount : " + dt3.Rows[0]["DepRefundAmount"].ToString() + "<br> Trip Start : " + dt3.Rows[0]["TripStartTime"] + "<br> Trip End : " + dt3.Rows[0]["TripEndTime"] + "<br> Refunded By : " + dt3.Rows[0]["DepositRefundBy"] + "<br> Refunded Date : " + dt3.Rows[0]["DepRefundDate"] + "</span>";
                                                    //lblalert.Text = ResponseMsg;
                                                    MpeTrip.Show();
                                                    txtSearchPin.Text = "";
                                                    ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);

                                                    divScanQR.Visible = true;
                                                    txtSearchPin.Text = "";
                                                    txtSearchPin.Focus();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var Errorresponse = response.Content.ReadAsStringAsync().Result;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                        }
                    }
                }
                else
                {
                    BoatTripCompletedWaitingForDepositRefund();
                    lblSearchPinResponse.Text = "Invalid Pin / Booking Id";
                    txtBookingIdPin.Text = "";
                    txtBookingIdPin.Focus();
                }
            }

        }
        else
        {
            BoatTripCompletedWaitingForDepositRefund();
            //lblSearchPinResponse.Text = "Invalid Pin / Booking Id";
            txtBookingIdPin.Text = "";
            txtBookingIdPin.Focus();
        }
    }

    /// <summary>
    /// Developed By Silambarasu D
    /// Developed By 21 April 2022
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPopUpOkay_Click(object sender, EventArgs e)
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

                var vTripSheetSettlement2 = new TripSheetSettlement()
                {
                    QueryType = "TripEnd",
                    BookingId = ViewState["BookingId"].ToString().Trim(),
                    BoatReferenceNo = ViewState["BoatReferenceNo"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    RowerId = ViewState["Rower"].ToString().Trim(),
                    TripStartTime = "",//Start Time Empty
                    TripEndTime = "",
                    ActualBoatId = ViewState["BoatId"].ToString().Trim(),
                    SSUserBy = Session["UserId"].ToString().Trim(),
                    SDUserBy = Session["UserId"].ToString().Trim(),
                    BookingMedia = "DW"
                };

                response = client.PostAsJsonAsync("NewTripSheetWeb/Update", vTripSheetSettlement2).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        AfterEndRefund();
                    }

                }

            }
        }
        catch (Exception ex)
        {
            lblAlertMsg.Text = ex.ToString();

        }
        return;
    }
    protected void btnPopUpCancel_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        SettledAddProcess(0, 10, out istart, out iend);
        txtBookingIdPin.Text = "";
        MpeTrip1.Hide();

        if (Session["BTMDepositRefund"].ToString().Trim() == "Y")
        {
            if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
            {
                divScanQR.Visible = true;
                GvBoatBookingTrip.Columns[17].Visible = true;
                GvBoatBookingTrip.Columns[18].Visible = true;
                BoatTripCompletedWaitingForDepositRefund();
                GetPaymentType();
                lbtnNewRefund.Visible = false;
                BindAmount();
            }
            else
            {
                if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")//Access for Both Scan & pin
                {
                    divScanQR.Visible = true;
                    GvBoatBookingTrip.Columns[1].Visible = true;
                    BoatTripCompletedWaitingForDepositRefund();
                    GvBoatBookingTrip.Columns[17].Visible = false;
                    GvBoatBookingTrip.Columns[18].Visible = false;
                    GetPaymentType();
                    lbtnNewRefund.Visible = false;
                    BindAmount();
                }
                else if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "Y" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "N")//Access for Scan only
                {
                    divScanQR.Visible = true;
                    GvBoatBookingTrip.Columns[1].Visible = true;
                    GvBoatBookingTrip.Columns[17].Visible = false;
                    GvBoatBookingTrip.Columns[18].Visible = false;
                    //BoatTripCompletedWaitingForDepositRefund();
                    GetPaymentType();
                    lbtnNewRefund.Visible = false;
                    BindAmount();
                    txtSearchPin.Focus();

                    divRefundPreNext.Visible = false;
                }

                else if (Session["BTMDepositRefundScanQR"].ToString().Trim() == "N" && Session["BTMDepositRefundScanPin"].ToString().Trim() == "Y")//Access for Pin only
                {
                    divScanQR.Visible = false;
                    GvBoatBookingTrip.Columns[1].Visible = true;

                    BoatTripCompletedWaitingForDepositRefund();
                    GvBoatBookingTrip.Columns[17].Visible = false;
                    GvBoatBookingTrip.Columns[18].Visible = false;
                    GetPaymentType();
                    lbtnNewRefund.Visible = false;
                    BindAmount();
                }
            }
        }
        txtSearchPin.Focus();
    }

    /// <summary>
    /// TO get refund pop up after ending the trip
    /// </summary>
    public void AfterEndRefund()
    {

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var vTripSheetSettlement = new TripSheetSettlement()
            {
                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                BoatHouseName = Session["BoatHouseName"].ToString(),
                BookingId = ViewState["BookingId"].ToString().Trim(),
                BookingPin = ViewState["BookingPinExtn"].ToString().Trim()
            };

            HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/getBookingDetByPin", vTripSheetSettlement).Result;

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
                        GvBoatBookingTrip.Visible = true;
                        GvBoatBookingTrip.DataSource = dt;
                        GvBoatBookingTrip.DataBind();
                        divRefund.Visible = true;
                        divGridSettledList.Visible = false;
                        txtSearchPin.Text = "";

                        divalert.Visible = false;

                        if ((Convert.ToDecimal(ViewState["CashInHand"]) <= 0))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Cash In Hand Is Less Than Zero !!!');", true);
                            return;

                        }
                        else
                        {
                            lblBookingDateDisp.Text = dt.Rows[0]["BookingDate"].ToString();
                            lblBookingIdDisp.Text = dt.Rows[0]["BookingId"].ToString();
                            lblInitialBillAmountDisp.Text = dt.Rows[0]["InitNetAmount"].ToString();
                            lblInitDepositDisp.Text = dt.Rows[0]["Deposit"].ToString();
                            string lblCustomerMobile = dt.Rows[0]["CustomerMobile"].ToString();
                            string lblBoatType = dt.Rows[0]["BoatType"].ToString();
                            string lblBoatSeater = dt.Rows[0]["BoatSeaterName"].ToString();
                            string lblStartTime = dt.Rows[0]["TripStartTime"].ToString();
                            string lblEndTime = dt.Rows[0]["TripEndTime"].ToString();
                            string lblBookingDuration = dt.Rows[0]["BookingDuration"].ToString();

                            ViewState["BookingDate"] = lblBookingDateDisp.Text.Trim();
                            ViewState["BoatType"] = lblBoatType.ToString().Trim();
                            ViewState["BoatSeater"] = lblBoatSeater.ToString().Trim();
                            ViewState["StartTime"] = lblStartTime.ToString().Trim();
                            ViewState["EndTime"] = lblEndTime.ToString().Trim();
                            ViewState["BookingDuration"] = lblBookingDuration.ToString().Trim();

                            if (lblCustomerMobile != "")
                            {
                                txtCustomerMobile.Text = lblCustomerMobile;
                                txtCustomerMobile.Enabled = false;
                            }
                            else
                            {
                                txtCustomerMobile.Text = "";
                                txtCustomerMobile.Enabled = true;
                            }

                            ViewState["BoatRefNo"] = dt.Rows[0]["BoatReferenceNo"].ToString();
                            ViewState["BookingPin"] = dt.Rows[0]["BookingPin"].ToString();
                            ViewState["PremiumStatus"] = dt.Rows[0]["PremiumStatus"].ToString();
                            ViewState["BookingId"] = dt.Rows[0]["BookingId"].ToString();
                            ViewState["TravelDuration"] = dt.Rows[0]["TravelDuration"].ToString();
                            ViewState["RefundDuration"] = dt.Rows[0]["RefundDuration"].ToString();
                            //NEW
                            ViewState["BillAmount"] = dt.Rows[0]["InitNetAmount"].ToString();
                            ViewState["Deposit"] = dt.Rows[0]["Deposit"].ToString();
                            ViewState["sPaymentMode"] = dt.Rows[0]["RePaymentType"].ToString();
                            //New
                            ViewState["ReundPrintStatus"] = dt.Rows[0]["RStatus"].ToString(); ;
                            GetCharges(ViewState["BoatRefNo"].ToString().Trim(),
                                ViewState["PremiumStatus"].ToString(), "GetRefund", ViewState["BookingId"].ToString(), ViewState["BookingPin"].ToString());

                            txtCustomerMobile.Focus();
                        }
                    }
                    else
                    {
                        GvBoatBookingTrip.DataBind();
                        divRefund.Visible = false;
                    }
                }
            }
        }
    }

    protected void lbtnLateRefund_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepositLateRefund.aspx");
    }

    /// <summary>
    /// Developed By Silambarasu
    /// Developed Date 2023-04-28
    /// To check whether any late refund has been approved and make visible of Previous Day Refund btn
    /// </summary>
    protected void getPreviousDayRefundApproved()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    QueryType = "GetPreviousDayRefundApproved",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = "";
                    if (vehicleEditresponse != "{}")
                    {
                        ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    }
                    //ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        lbtnLateRefund.Visible = true;
                    }
                    else
                    {
                        lbtnLateRefund.Visible = false;
                    }
                }
                else
                {
                    lbtnLateRefund.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }
}