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

public partial class Boating_PrintBoat : System.Web.UI.Page
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

                if (ViewState["rt"].ToString() == "b")
                {
                    GetBoatTickets(bi);
                    GetBoatTicketsSummaryReceipt(bi);
                    GetOtherTickets(bi);
                    divBoat.Visible = true;
                    divBookingReschedule.Visible = false;
                    divBoatDeposit.Visible = false;
                }
                if (ViewState["rt"].ToString() == "bb")
                {
                    GetBoatTickets(bi);
                    GetBoatTicketsSummaryReceipt(bi);
                    GetOtherTickets(bi);
                    divBoat.Visible = true;
                    divBookingReschedule.Visible = false;
                    divBoatDeposit.Visible = false;
                }
                if (ViewState["rt"].ToString() == "rb")
                {
                    GetBoatTickets(bi);
                    GetBoatTicketsSummaryReceipt(bi);
                    GetOtherTickets(bi);
                    divBoat.Visible = true;
                    divBookingReschedule.Visible = false;
                    divBoatDeposit.Visible = false;
                }
                if (ViewState["rt"].ToString() == "Cb")
                {
                    GetBoatTickets(bi);
                    GetBoatTicketsSummaryReceipt(bi);
                    GetOtherTickets(bi);
                    divBoat.Visible = true;
                    divBookingReschedule.Visible = false;
                    divBoatDeposit.Visible = false;
                }
                if (ViewState["rt"].ToString() == "brdl")
                {
                    string bpin = Request.QueryString["bpin"].ToString();
                    divBookingReschedule.Visible = true;
                    divBoat.Visible = false;
                    divBoatDeposit.Visible = false;
                    GetRescheduledDetails(bi.ToString().Trim(), bpin.ToString().Trim());
                    GetRescheduledSummary(bi.ToString().Trim(), bpin.ToString().Trim());
                }
                if (ViewState["rt"].ToString() == "dpt")
                {
                    string sBookingId = Request.QueryString["BId"].ToString();
                    string sBookingPin = Request.QueryString["BPin"].ToString();
                    GetBoatTicketsforDeposit(sBookingId, sBookingPin);
                    divBoat.Visible = false;
                    divBoatDeposit.Visible = true;
                    divBookingReschedule.Visible = false;
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
    public void GetRescheduledDetails(string sBookingId, string BookingPin)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var body = new BoatSearch();
                body = new BoatSearch()
                {
                    QueryType = "GetReschedulePrintDetails",
                    BookingId = sBookingId.ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    UserId = "0",
                    MobileNo = "0",
                    stgBookingPin = BookingPin.ToString().Trim(),
                    RescheduleOldDate = "",
                    RescheduleNewDate = "",
                    RescheduleType = ""/*Customer Reschedule*/
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BookingReschedule/RescheduleDetails", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        dtlistReschedule.DataSource = dtExists;
                        dtlistReschedule.DataBind();
                        divBookingReschedule.Visible = true;
                    }
                    else
                    {
                        divBookingReschedule.Visible = false;
                    }
                }
                else
                {
                    divBookingReschedule.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    public void GetRescheduledSummary(string sBookingId, string BookingPin)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var body = new BoatSearch();
                body = new BoatSearch()
                {
                    QueryType = "GetRescheduleSummary",
                    BookingId = sBookingId.ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    UserId = "0",
                    MobileNo = "0",
                    stgBookingPin = BookingPin.ToString().Trim(),
                    RescheduleOldDate = "",
                    RescheduleNewDate = "",
                    RescheduleType = ""/*Customer Reschedule*/
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BookingReschedule/RescheduleDetails", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        dtlistRescheduleSummary.DataSource = dtExists;
                        dtlistRescheduleSummary.DataBind();
                        divBookingReschedule.Visible = true;
                    }
                    else
                    {
                        divBookingReschedule.Visible = false;
                    }
                }
                else
                {
                    divBookingReschedule.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
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
            Response.Redirect("BoatBookingFinal.aspx");
        }
        else if (ViewState["rt"].ToString() == "rb")
        {
            Response.Redirect("~/Reports/RptBoatBooking.aspx");
        }
        else if (ViewState["rt"].ToString() == "Cb")
        {
            Session["ChangeBookingId"] = "";
            Response.Redirect("~/Boating/ChangeBoatDetails.aspx?rt=0");
        }
        else if (ViewState["rt"].ToString().Trim() == "brdl")
        {
            Response.Redirect("~/Boating/ReSchedule.aspx");
        }
        else if (ViewState["rt"].ToString().Trim() == "dpt")
        {
            Response.Redirect("~/Boating/TripSheetSettelement.aspx");
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

                            foreach (DataListItem dl in dtlistTicket.Items)
                            {
                                Label lblRescheduledDate = (Label)dl.FindControl("lblRescheduledDate");
                                Label lblBPResHdg = (Label)dl.FindControl("lblBPResHdg");

                                Label lblResDateHeading = (Label)dl.FindControl("lblResDateHeading");
                                Label lblBPResDate = (Label)dl.FindControl("lblBPResDate");
                                Label lblBoatHouseId = (Label)dl.FindControl("lblBoatHouseId");
                                Label lblBoatTypeId = (Label)dl.FindControl("lblBoatTypeId");
                                if (lblBoatHouseId.Text.Trim() == "21" && lblBoatTypeId.Text.Trim() == "24")
                                {
                                    Label OneRoundYCD = (Label)dl.FindControl("OneRoundYCD");
                                    OneRoundYCD.Visible = true;
                                }
                                else
                                {
                                    Label OneRoundYCD = (Label)dl.FindControl("OneRoundYCD");
                                    OneRoundYCD.Visible = false;

                                }
                                if (lblRescheduledDate.Text == "-")
                                {
                                    lblRescheduledDate.Visible = false;
                                    lblResDateHeading.Visible = false;
                                    lblBPResHdg.Visible = false;
                                    lblBPResDate.Visible = false;
                                }
                                else
                                {
                                    lblRescheduledDate.Visible = true;
                                    lblResDateHeading.Visible = true;
                                    lblBPResHdg.Visible = true;
                                    lblBPResDate.Visible = true;
                                }
                            }
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
                    if (lblActualBoatNum.Text != "" && lblExpectedTime.Text != "")
                    {
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
                    }
                    else
                    {
                        divrower.Visible = false;
                        divBPass.Visible = false;
                        divBPass1.Visible = false;
                    }
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


    private void GetBoatTicketsforDeposit(string sBookingId, string sBookingPin)
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
                    QueryType = "BoatDepositPrintTicket",
                    ServiceType = "",
                    Input1 = sBookingId,
                    Input2 = sBookingPin,
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
                        dtllistBoatDeposit.DataSource = dtExists;
                        dtllistBoatDeposit.DataBind();
                        dtllistBoatDeposit.Visible = true;
                        foreach (DataListItem dl in dtllistBoatDeposit.Items)
                        {

                            //Table divrower = (Table)dl.FindControl("divrower");
                            //divrower.Visible = false;
                            Label lblRescheduledDate = (Label)dl.FindControl("lblRescheduledDate");
                            Label lblBPResHdg = (Label)dl.FindControl("lblBPResHdg");

                            Label lblResDateHeading = (Label)dl.FindControl("lblResDateHeading");
                            Label lblBPResDate = (Label)dl.FindControl("lblBPResDate");
                            Label lblBoatHouseId = (Label)dl.FindControl("lblBoatHouseId");
                            Label lblBoatTypeId = (Label)dl.FindControl("lblBoatTypeId");
                            if (lblBoatHouseId.Text.Trim() == "21" && lblBoatTypeId.Text.Trim() == "24")
                            {
                                Label OneRoundYCD = (Label)dl.FindControl("OneRoundYCD");
                                OneRoundYCD.Visible = true;
                            }
                            else
                            {
                                Label OneRoundYCD = (Label)dl.FindControl("OneRoundYCD");
                                OneRoundYCD.Visible = false;

                            }
                            if (lblRescheduledDate.Text == "-")
                            {
                                lblRescheduledDate.Visible = false;
                                lblResDateHeading.Visible = false;
                                lblBPResHdg.Visible = false;
                                lblBPResDate.Visible = false;
                            }
                            else
                            {
                                lblRescheduledDate.Visible = true;
                                lblResDateHeading.Visible = true;
                                lblBPResHdg.Visible = true;
                                lblBPResDate.Visible = true;
                                divBoat.Visible = true;
                            }
                        }
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

    protected void dtllistBoatDeposit_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Label BookingId = (Label)e.Item.FindControl("lblBookingId");
                Label RowerCharge = (Label)e.Item.FindControl("lblBillRowerCharge");
                Label lblBookingPin = (Label)e.Item.FindControl("lblBookingPin");
                Label lblBoatReferenceNo = (Label)e.Item.FindControl("lblBoatReferenceNo");

                Control divrower = e.Item.FindControl("divrowerdep") as Control;
                Control divBPass = e.Item.FindControl("divBPassdep") as Control;
                Control divBPass1 = e.Item.FindControl("divBPass1dep") as Control;

                Label lblActualBoatNum = (Label)e.Item.FindControl("lblActualBoatNum");
                Label lblExpectedTime = (Label)e.Item.FindControl("lblExpectedTime");

                Label lblCheckDate = (Label)e.Item.FindControl("lblCheckDate");
                Label lblTripEndTime = (Label)e.Item.FindControl("lblTripEndTime");

                if (DateTime.Parse(lblCheckDate.Text, objEnglishDate).ToString() == DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"), objEnglishDate).ToString())
                {
                    if (lblActualBoatNum.Text != "" && lblExpectedTime.Text != "")
                    {
                        if (lblTripEndTime.Text != "")
                        {
                            divrower.Visible = false;
                            divBPass.Visible = true;
                            divBPass1.Visible = true;
                        }
                        else
                        {
                            if (Convert.ToDecimal(RowerCharge.Text) > 0)
                            {
                                divrower.Visible = false;
                            }
                            else
                            {
                                divrower.Visible = false;
                            }

                            divBPass.Visible = true;
                            divBPass1.Visible = true;
                        }
                    }
                    else
                    {
                        divrower.Visible = false;
                        divBPass.Visible = false;
                        divBPass1.Visible = false;
                    }
                }
                else
                {
                    dtllistBoatDeposit.Visible = false;
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
        public string RescheduleType { get; set; }
        public string stgBookingPin { get; set; }
        public string RescheduleOldDate { get; set; }
        public string RescheduleNewDate { get; set; }
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