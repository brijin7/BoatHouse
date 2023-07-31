using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_Boating : System.Web.UI.MasterPage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
            
                lblCurrentYear.Text = DateTime.Now.Year.ToString();

                lblLoginUser.Text = Session["FirstName"].ToString().Trim() + " " + Session["LastName"].ToString().Trim();

                Session["PrintUserName"] = lblLoginUser.Text.Trim();

                lblBoatHouse.Text = Session["BoatHouseName"].ToString();
               
                   
                if (Session["UserRole"].ToString().Trim() != "Sadmin")
                {
                   
                    AccountUnlock.Visible = false;
                    if (Session["UserRole"].ToString().Trim() == "User")
                    {
                        if (Session["CorpId"].ToString() != "")
                        {
                            GetCorporateOffice();
                        }
                        MenuRightsBasedOnRole();
                        liAndroid.Visible = false;
                        liHelp.Visible = true;//SILLU 31 Mar 2023
                        divCreditReport.Visible = false; //SILLU
                        rptReprintReport.Visible = false;
                        rptQRCodeGeneration.Visible = false;
                    }
                    else if (Session["UserRole"].ToString().Trim() == "Admin" || Session["UserRole"].ToString().Trim() == "admin")
                    {
                        if (Session["CorpId"].ToString() != "")
                        {
                            GetCorporateOffice();
                        }


                        liAndroid.Visible = true;
                        liCMMaster.Visible = true; //Sillu 2023-07-19
                        liMaster.Visible = true;
                        liBooking.Visible = true;
                        liTransactions.Visible = true;
                        liReports.Visible = true;
                        liKioskBooking.Visible = true;
                        liHelp.Visible = true;

                        //bCreditBoatBooking.Visible = true;--2021-0-23
                        divCreditReport.Visible = true;

                        Session["BMBooking"] = "Y";
                        Session["BMBookingOthers"] = "Y";
                        Session["BBMBulkBooking"] = "Y";

                        Session["BBMAdditionalService"] = "Y";
                        Session["BMOtherService"] = "Y";
                        Session["BBMBulkOtherService"] = "Y";

                        Session["BMBookingRestaurant"] = "Y";
                        Session["BBMKioskBooking"] = "Y";

                        //Newly Added 
                        Session["BMKioskOtherService"] = "Y";
                        //Newly Added 
                        Session["BBMTripSheetWeb"] = "Y";
                        Session["BBMTripSheetStart"] = "Y";
                        Session["BBMTripSheetEnd"] = "Y";
                        Session["BBMChangeBoatDetails"] = "Y";
                        Session["BBMChangeTripSheet"] = "Y";

                        Session["BTMDepositRefund"] = "Y";
                        Session["BTMDepositRefundScanQR"] = "Y";
                        Session["BTMDepositRefundScanPin"] = "Y";
                    }

                    BindPaymentStatus();
                    CheckBookingPaymentRights();

                }
                else
                {
                    AccountUnlock.Visible = true;
                    liAndroid.Visible = true;
                    bmDeactiveSlotMstr.Visible = true;
                    if (Session["ShowPage"].ToString().Trim() == "1")
                    {
                        liMaster.Visible = false;
                        liBooking.Visible = false;
                        liTransactions.Visible = false;
                        liReports.Visible = true;
                        liKioskBooking.Visible = false;

                        divCreditReport.Visible = true;

                        if (Session["UserId"].ToString().Trim() == "1001" || CheckSadminRights() == "Y")
                        {
                            liMaster.Visible = true;
                            liBooking.Visible = true;
                            liTransactions.Visible = true;
                            liReports.Visible = true;
                            liKioskBooking.Visible = true;
                        }

                        Session["BMBooking"] = "Y";
                        Session["BMBookingOthers"] = "Y";
                        Session["BBMBulkBooking"] = "Y";

                        Session["BBMAdditionalService"] = "Y";
                        Session["BMOtherService"] = "Y";
                        Session["BBMBulkOtherService"] = "Y";

                        Session["BMBookingRestaurant"] = "Y";
                        Session["BBMKioskBooking"] = "Y";

                        //Newly Added 
                        Session["BMKioskOtherService"] = "Y";
                        //Newly Added 
                        Session["BBMTripSheetWeb"] = "Y";
                        Session["BBMChangeBoatDetails"] = "Y";
                        Session["BBMChangeTripSheet"] = "Y";

                        Session["BTMDepositRefund"] = "Y";
                        Session["BTMDepositRefundScanQR"] = "Y";
                        Session["BTMDepositRefundScanPin"] = "Y";

                        Session["KioskPaymentRights"] = "Y";
                        Session["KioskOnlineRights"] = "Y";
                        Session["KioskUPIRights"] = "Y";

                        Session["DeptPaymentRights"] = "Y";
                        Session["DeptOnlineRights"] = "Y";
                        Session["DeptUPIRights"] = "Y";

                        if (Session["SupportUser"].ToString().Trim() == "D")
                        {
                            liBack.Visible = true;
                        }
                    }
                    else
                    {
                        liCommonMaster.Visible = true;
                        liBHMaster.Visible = true;
                        liOtherMaster.Visible = false;
                        liSadminMaster.Visible = true;
                        liPrematix.Visible = false;
                        liComplaint.Visible = false;

                        if (Session["UserId"].ToString().Trim() == "1001")
                        {
                            liPrematix.Visible = true;
                            bmSadminAccessRights.Visible = true;
                            bmSmsBlock.Visible = true;
                        }
                        ////3419 - Prabhakar,3420 - Madhumita,1101 - Mohan
                        //if (Session["UserId"].ToString().Trim() == "1001" || Session["UserId"].ToString().Trim() == "3419" ||
                        //    Session["UserId"].ToString().Trim() == "3420" || Session["UserId"].ToString().Trim() == "1101")
                        //{
                        //    liComplaint.Visible = true;
                        //}

                    }
                }
                BoatAdditionalTicketLists();

            }
            catch (Exception)
            {
                Response.Redirect("~/Error/ErrorBack.aspx", false);
            }
        }
        // Below method used for user login details
        //CheckUserLoginSession();
    }

    //Modified By: Vinitha.M
    //Modified Date: 20-Oct-2021


    public void BoatAdditionalTicketLists()
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

                if (Session["UserRole"].ToString().Trim() == "Admin" || Session["UserRole"].ToString().Trim() == "admin")

                {
                    var CatType = new CategoryService()
                    {
                        QueryType = "GetBoatListForShow",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = ""

                    };

                    response = client.PostAsJsonAsync("BoatListHideShow", CatType).Result;
                }
                else
                {
                    var CatType = new CategoryService()
                    {
                        QueryType = "GetBoatListForShowUser",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("BoatListHideShow", CatType).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        if (dtExists.Rows[0]["Additional"].ToString() == "" || dtExists.Rows[0]["Additional"].ToString() == "0")
                        {
                            bAdditionalTicket.Visible = false;

                        }


                    }
                    else
                    {
                        bAdditionalTicket.Visible = true;

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

    public class CategoryService
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string BoatHouseId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string UserId { get; set; }

    }

    protected void btnHome_Click(object sender, EventArgs e)
    {
        Session["ShowPage"] = "0";

        if (Session["UserRole"].ToString().Trim() == "Sadmin")
        {
            Session["BoatHouseId"] = "";
            Session["BoatHouseName"] = "";
        }

        Response.Redirect("/Login/InterPage.aspx?qUserId=" + Session["UserId"].ToString().Trim() + "&qUserRole= " + Session["UserRole"].ToString().Trim() + "&qUserType="
            + Session["UserType"].ToString().Trim() + "&qBranchId=" + Session["BoatHouseId"].ToString().Trim() + "&qBranchName="
            + Session["BoatHouseName"].ToString().Trim() + "&qModule=Dept");
    }

    protected void lbtnBack_Click(object sender, EventArgs e)
    {
        Session["ShowPage"] = "0";
        Session["BoatHouseId"] = "";
        Session["BoatHouseName"] = "";
        Session["CorpLogo"] = "";
        Session["CorpName"] = "";
        Session["CorpId"] = "";
   
        Response.Redirect("~/Boating/NewDashboard.aspx");
    }

    public void MenuRightsBasedOnRole()
    {
        try
        {
            Session["BMBooking"] = "N";
            Session["BMBookingOthers"] = "N";
            Session["BBMBulkBooking"] = "N";

            Session["BBMAdditionalService"] = "N";
            Session["BMOtherService"] = "N";
            Session["BBMBulkOtherService"] = "N";

            Session["BMBookingRestaurant"] = "N";
            Session["BBMKioskBooking"] = "N";
            //Newly added BMKioskOtherService
            Session["BMKioskOtherService"] = "N";
            //Newly added 
            Session["BBMTripSheetWeb"] = "N";
            Session["BBMTripSheetStart"] = "N";
            Session["BBMTripSheetEnd"] = "N";
            Session["BBMChangeBoatDetails"] = "N";
            Session["BBMChangeTripSheet"] = "N";

            Session["BTMDepositRefund"] = "N";
            Session["BTMDepositRefundScanQR"] = "N";
            Session["BTMDepositRefundScanPin"] = "N";

            // Booking Menu

            bBoatBooking.Visible = false;
            //bBulkBoatBooking.Visible = false; --2021-08-23 
            bAdditionalTicket.Visible = false;
            bBOtherService.Visible = false;
            //bBBulkOtherService.Visible = false;

            //bBTripSheet.Visible = false;
            bBTripSheet_Test.Visible = false;

            bBSmartTrip.Visible = false;
            //bBSmartTrip_new.Visible = false;
            bBChangeBoatDetails.Visible = false;
            bBChangeTripDetails.Visible = false;
            //bBChangeTripDetailsNew.Visible = false;
            bBBoatRetrip.Visible = false;
            divRights1.Visible = false;

            bBCanlMstr.Visible = false;
            bBReSeMstr.Visible = false;
            bBManualTicket.Visible = false;
            bBBoardingPass.Visible = false;

            // Transaction Menu
            divRefundDetails.Visible = false;
            bTMPMstr.Visible = false;
            bTMIMstr.Visible = false;
            bTDepositRefund.Visible = false;
            bTRower.Visible = false;
            bTRefundCounter.Visible = false;
            bFSEMain.Visible = false;
            btReceiptBalance.Visible = false;

            // Report Menu
            divPrintService.Visible = false;
            rptPrintBoatBooking.Visible = false;
            rptPrintAdditionalTicket.Visible = false;
            rptPrintOtherService.Visible = false;
            rptPrintRestaurantService.Visible = false;

            divAbstractBooking.Visible = false;
            rptAbstractBoatBooking.Visible = false;
            rptAbstractAdditionalTicket.Visible = false;
            rptAbstractOtherService.Visible = false;
            rptAbstractRestaurant.Visible = false;

            divTripSheet.Visible = false;
            rptAvalBoats.Visible = false;
            rptBoatwiseTrip.Visible = false;
            rptTripSheetSummary.Visible = false;
            rptBoatCancellation.Visible = false;
            rptDepositStatus.Visible = false;
            rptDiscount.Visible = false;
            rptAbstractTripSheet.Visible = false;

            divRptRower.Visible = false;
            rptRowerCharges.Visible = false;
            rptRowerSettlement.Visible = false;
            rptChalanRegister.Visible = false;
            rptServiceWiseCollection.Visible = false;
            rptExtndBtRides.Visible = false;

            //Credit//
            rptCreditPrintBoat.Visible = false;
            rptCreditTripWise.Visible = false;
            divCreditReport.Visible = false;

            // Restaurant Otptions

            liRestaurant.Visible = false;
            RBService.Visible = false;
            //RBulkRestaurant.Visible = false;

            rptUserBasedBooked.Visible = false;
            rptTripWiseCollection.Visible = false;
            rptBoatTypeRower.Visible = false;
            rptBoatInfoDisplay.Visible = false;

            rptReceiptBalance.Visible = false;
            rptQRCodeGeneration.Visible = false;
            rptReprintReport.Visible = false;

            //Kiosk Booking
            liKioskBooking.Visible = false;
            bKBoatBooking.Visible = false;
            bKOtherService.Visible = false;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var UserRights = new UserRights()
                {
                    QueryType = "UserAccess",
                    ServiceType = "MenuUserBased",
                    UserId = Session["UserId"].ToString().Trim(),
                    UserRole = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("GetUserAccess", UserRights).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        // Master Menu

                        if (dt.Rows[0]["BMaster"].ToString().Trim() == "Y")
                        {
                            liMaster.Visible = true;
                            liCMMaster.Visible = true; //Sillu 2023-07-19
                        }

                        // Booking Menu 

                        if (dt.Rows[0]["BBooking"].ToString().Trim() == "Y")
                        {
                            liBooking.Visible = true;

                            // Sub Menu

                            if (dt.Rows[0]["BBMBooking"].ToString().Trim() == "Y")
                            {
                                Session["BMBooking"] = "Y";
                                bBoatBooking.Visible = true;

                                if (dt.Rows[0]["BBMBookingOthers"].ToString().Trim() == "Y")
                                {
                                    Session["BMBookingOthers"] = "Y";
                                }
                            }

                            if (dt.Rows[0]["BBMBulkBooking"].ToString().Trim() == "Y")
                            {
                                Session["BBMBulkBooking"] = "Y";
                                //bBulkBoatBooking.Visible = true;--2021-08-23
                            }

                            // Additional Tikcet

                            if (dt.Rows[0]["BBMAdditionalService"].ToString().Trim() == "Y")

                            {
                                Session["BBMAdditionalService"] = "Y";
                                bAdditionalTicket.Visible = true;
                                rptPrintAdditionalTicket.Visible = true;
                                rptAbstractAdditionalTicket.Visible = true;
                            }

                            if (dt.Rows[0]["BBMOtherService"].ToString().Trim() == "Y")
                            {
                                Session["BMOtherService"] = "Y";
                                bBOtherService.Visible = true;
                            }

                            if (dt.Rows[0]["BBMBulkOtherService"].ToString().Trim() == "Y")
                            {
                                Session["BBMBulkOtherService"] = "Y";
                                //bBBulkOtherService.Visible = true;
                            }

                            //Kiosk Booking
                            if (dt.Rows[0]["BBMKioskBooking"].ToString().Trim() == "Y")
                            {
                                bKBoatBooking.Visible = true;
                            }

                            if (dt.Rows[0]["BMKioskOtherService"].ToString().Trim() == "Y")
                            {
                                bKOtherService.Visible = true;
                            }

                            if (dt.Rows[0]["BBMKioskBooking"].ToString().Trim() == "Y" || dt.Rows[0]["BMKioskOtherService"].ToString().Trim() == "Y")
                            {
                                liKioskBooking.Visible = true;
                            }

                            // Cancellation

                            if (dt.Rows[0]["BBMCancellation"].ToString().Trim() == "Y")
                            {
                                bBCanlMstr.Visible = true;
                            }

                            // ReScheduling

                            if (dt.Rows[0]["BBMReScheduling"].ToString().Trim() == "Y")
                            {
                                bBReSeMstr.Visible = true;
                            }
                            if (dt.Rows[0]["BBMReScheduling"].ToString().Trim() == "Y")
                            {
                                bBReSeMstr.Visible = true;
                            }

                            if (dt.Rows[0]["BGenerateManualTicket"].ToString().Trim() == "Y")
                            {
                                bBManualTicket.Visible = true;
                            }



                            //Booking
                            if (dt.Rows[0]["BGeneratingBoardingPass"].ToString().Trim() == "Y")
                            {
                                bBBoardingPass.Visible = true;
                            }

                            //Exception Sub Menu
                            //if (dt.Rows[0]["BBMChangeBoatDetails"].ToString().Trim() == "Y")
                            //{
                            //    bBChangeBoatDetails.Visible = true;
                            //}
                            //if (dt.Rows[0]["BBMChangeTripSheet"].ToString().Trim() == "Y")
                            //{
                            //    bBChangeTripDetails.Visible = true;
                            //}

                            //if (dt.Rows[0]["BBMChangeBoatDetails"].ToString().Trim() == "Y" || dt.Rows[0]["BBMChangeTripSheet"].ToString().Trim() == "Y" || dt.Rows[0]["BRMRestaurantService"].ToString().Trim() == "Y")
                            //{
                            //    divRights1.Visible = true;
                            //}







                            // Booking Restaurant Services

                            if (dt.Rows[0]["BRestaurant"].ToString().Trim() == "Y")
                            {
                                Session["BMBookingRestaurant"] = "Y";

                                liRestaurant.Visible = true;
                                RFoodCategory.Visible = true;
                                RFoodItem.Visible = true;

                                RBService.Visible = true;
                                //RBulkRestaurant.Visible = true;
                            }

                            // Trip Sheet Web Options

                            if (dt.Rows[0]["BBMTripSheet"].ToString().Trim() == "Y")
                            {
                                Session["BBMTripSheetWeb"] = "Y";

                                //bBTripSheet.Visible = true;
                                bBTripSheet_Test.Visible = true;

                                if (Session["UserRole"].ToString().Trim() != "admin")
                                {
                                    string[] TripDetails = dt.Rows[0]["BTripSheetOptions"].ToString().Trim().Split(',');

                                    Session["BBMTripSheetStart"] = TripDetails[0].ToString();
                                    Session["BBMTripSheetEnd"] = TripDetails[1].ToString();

                                    if (TripDetails[2].ToString() == "Y")
                                    {
                                        bBSmartTrip.Visible = true;
                                        //bBSmartTrip_new.Visible = true;
                                    }
                                }
                            }

                            // Change Boat Seater Details

                            if (dt.Rows[0]["BBMChangeBoatDetails"].ToString().Trim() == "Y")
                            {
                                Session["BBMChangeBoatDetails"] = "Y";

                                bBChangeBoatDetails.Visible = true;
                            }

                            // Change Trip Details

                            if (dt.Rows[0]["BBMChangeTripSheet"].ToString().Trim() == "Y")
                            {
                                Session["BBMChangeTripSheet"] = "Y";
                                bBChangeTripDetails.Visible = true;
                                //bBChangeTripDetailsNew.Visible = true;
                            }
                            // Boat ReTrip Details
                            if (dt.Rows[0]["BBMBoatReTripDetails"].ToString().Trim() == "Y")
                            {
                                bBBoatRetrip.Visible = true;
                            }

                            if (dt.Rows[0]["BBMChangeBoatDetails"].ToString().Trim() == "Y" || dt.Rows[0]["BBMChangeTripSheet"].ToString().Trim() == "Y" || dt.Rows[0]["BBMBoatReTripDetails"].ToString().Trim() == "Y")
                            {
                                divRights1.Visible = true;
                            }



                        }

                        // KioskBooking Menu 

                        if (dt.Rows[0]["BBMKioskBooking"].ToString().Trim() == "Y")
                        {
                            Session["BBMKioskBooking"] = "Y";
                            liKioskBooking.Visible = true;
                        }

                        if (dt.Rows[0]["BMKioskOtherService"].ToString().Trim() == "Y")
                        {
                            Session["BMKioskOtherService"] = "Y";
                            liKioskBooking.Visible = true;
                        }

                        // Transaction Menu  

                        if (dt.Rows[0]["BTransaction"].ToString().Trim() == "Y")
                        {
                            liTransactions.Visible = true;

                            // Sub Menu

                            if (dt.Rows[0]["BTMMaterialPurchase"].ToString().Trim() == "Y")
                            {
                                bTMPMstr.Visible = true;
                            }

                            if (dt.Rows[0]["BTMMaterialIssue"].ToString().Trim() == "Y")
                            {
                                bTMIMstr.Visible = true;
                            }

                            // Refund Details                                                      
                            if (dt.Rows[0]["BTMTripSheetSettle"].ToString().Trim() == "Y" || dt.Rows[0]["BTMReceiptBalanceRefund"].ToString().Trim() == "Y")
                            {
                                divRefundDetails.Visible = true;
                            }
                            // Refund Details  Sub menu
                            // Deposit Refund Options
                            if (dt.Rows[0]["BTMTripSheetSettle"].ToString().Trim() == "Y")
                            {
                                bTDepositRefund.Visible = true;

                                Session["BTMDepositRefund"] = "Y";

                                if (Session["UserRole"].ToString().Trim() != "admin")
                                {
                                    string[] DepRefDetails = dt.Rows[0]["BDepositRefundOptions"].ToString().Trim().Split(',');

                                    Session["BTMDepositRefundScanQR"] = DepRefDetails[0].ToString();
                                    Session["BTMDepositRefundScanPin"] = DepRefDetails[1].ToString();
                                }
                            }

                            //Receipt Balance Refund
                            if (dt.Rows[0]["BTMReceiptBalanceRefund"].ToString().Trim() == "Y")
                            {
                                btReceiptBalance.Visible = true;
                            }

                            // Rower Settlement

                            if (dt.Rows[0]["BTMRowerSettle"].ToString().Trim() == "Y")
                            {
                                bTRower.Visible = true;
                            }

                            // Deposit Refund Counter

                            if (dt.Rows[0]["BTMRefundCounter"].ToString().Trim() == "Y")
                            {
                                bTRefundCounter.Visible = true;
                            }

                            // Food Stock Entry Maintenance

                            if (dt.Rows[0]["BTMFoodStockEntryMaintance"].ToString().Trim() == "Y")
                            {
                                bFSEMain.Visible = true;
                            }

                        }

                        // Reports Menu 

                        if (dt.Rows[0]["BReports"].ToString().Trim() == "Y")
                        {
                            liReports.Visible = true;

                            // Sub Menu -- Print

                            if (dt.Rows[0]["BRMBooking"].ToString().Trim() == "Y")
                            {
                                rptPrintBoatBooking.Visible = true;
                            }

                            if (dt.Rows[0]["BRMOtherService"].ToString().Trim() == "Y")
                            {
                                rptPrintOtherService.Visible = true;
                            }

                            if (dt.Rows[0]["BRMRestaurantService"].ToString().Trim() == "Y")
                            {
                                rptPrintRestaurantService.Visible = true;
                            }

                            if (dt.Rows[0]["BRMAdditionalTicket"].ToString().Trim() == "Y")
                            {
                                rptPrintAdditionalTicket.Visible = true;
                            }

                            if (dt.Rows[0]["BRMBooking"].ToString().Trim() == "Y" || dt.Rows[0]["BRMOtherService"].ToString().Trim() == "Y" || dt.Rows[0]["BRMRestaurantService"].ToString().Trim() == "Y" ||
                            dt.Rows[0]["BRMAdditionalTicket"].ToString().Trim() == "Y")
                            {
                                divPrintService.Visible = true;
                            }



                            if (dt.Rows[0]["BRMReceiptBalance"].ToString().Trim() == "Y")
                            {
                                rptReceiptBalance.Visible = true;
                            }

                            if (dt.Rows[0]["BRMRePrintReport"].ToString().Trim() == "Y")
                            {
                                rptReprintReport.Visible = true;
                            }

                            if (dt.Rows[0]["BRMQRCodeGeneration"].ToString().Trim() == "Y")
                            {
                                rptQRCodeGeneration.Visible = true;

                            }





                            // Sub Menu -- Abstract

                            if (dt.Rows[0]["BRMAbstractBoatBook"].ToString().Trim() == "Y")
                            {
                                rptAbstractBoatBooking.Visible = true;
                            }

                            if (dt.Rows[0]["BRMAbstractOthSvc"].ToString().Trim() == "Y")
                            {
                                rptAbstractOtherService.Visible = true;
                            }

                            if (dt.Rows[0]["BRMAbstractResSvc"].ToString().Trim() == "Y")
                            {
                                rptAbstractRestaurant.Visible = true;
                            }

                            if (dt.Rows[0]["BRMAbstractAdditionalTicket"].ToString().Trim() == "Y")
                            {
                                rptAbstractAdditionalTicket.Visible = true;
                            }

                            if (dt.Rows[0]["BRMAbstractBoatBook"].ToString().Trim() == "Y" || dt.Rows[0]["BRMAbstractOthSvc"].ToString().Trim() == "Y" || dt.Rows[0]["BRMAbstractResSvc"].ToString().Trim() == "Y" ||
                            dt.Rows[0]["BRMAbstractAdditionalTicket"].ToString().Trim() == "Y")
                            {
                                divAbstractBooking.Visible = true;
                            }

                            // Sub Menu -- Boat Wise Reports

                            if (dt.Rows[0]["BRMBoatwiseTrip"].ToString().Trim() == "Y")
                            {
                                rptBoatwiseTrip.Visible = true;
                            }

                            if (dt.Rows[0]["BRMTripSheet"].ToString().Trim() == "Y")
                            {
                                rptTripSheetSummary.Visible = true;
                            }

                            if (dt.Rows[0]["BRMTripWiseCollection"].ToString().Trim() == "Y")
                            {
                                rptTripWiseCollection.Visible = true;
                            }

                            if (dt.Rows[0]["BRMDepositStatus"].ToString().Trim() == "Y")
                            {
                                rptDepositStatus.Visible = true;
                            }
                            if (dt.Rows[0]["BRMDiscountReport"].ToString().Trim() == "Y")
                            {
                                rptDiscount.Visible = true;
                            }
                            if (dt.Rows[0]["BRMCashinHands"].ToString().Trim() == "Y")
                            {
                                rptAbstractTripSheet.Visible = true;
                            }

                            if (dt.Rows[0]["BRMBoatwiseTrip"].ToString().Trim() == "Y" || dt.Rows[0]["BRMTripSheet"].ToString().Trim() == "Y" || dt.Rows[0]["BRMTripWiseCollection"].ToString().Trim() == "Y" ||
                     dt.Rows[0]["BRMDepositStatus"].ToString().Trim() == "Y" || dt.Rows[0]["BRMDiscountReport"].ToString().Trim() == "Y" || dt.Rows[0]["BRMCashinHands"].ToString().Trim() == "Y")
                            {
                                divTripSheet.Visible = true;
                            }


                            if (dt.Rows[0]["BRMAvailBoatCapacity"].ToString().Trim() == "Y")
                            {
                                rptAvalBoats.Visible = true;
                            }


                            if (dt.Rows[0]["BRMBoatCancellation"].ToString().Trim() == "Y")
                            {
                                rptBoatCancellation.Visible = true;
                            }



                            // Sub Menu -- Rower Reports

                            if (dt.Rows[0]["BRMRowerCharges"].ToString().Trim() == "Y")
                            {
                                rptRowerCharges.Visible = true;
                            }

                            if (dt.Rows[0]["BRMRowerSettle"].ToString().Trim() == "Y")
                            {
                                rptRowerSettlement.Visible = true;
                            }

                            if (dt.Rows[0]["BRMBoatTypeRowerList"].ToString().Trim() == "Y")
                            {
                                rptBoatTypeRower.Visible = true;
                            }

                            if (dt.Rows[0]["BRMExtendedBoatHouse"].ToString().Trim() == "Y")
                            {
                                rptExtndBtRides.Visible = true;
                            }


                            if (dt.Rows[0]["BRMRowerCharges"].ToString().Trim() == "Y" || dt.Rows[0]["BRMRowerSettle"].ToString().Trim() == "Y" ||
    dt.Rows[0]["BRMBoatTypeRowerList"].ToString().Trim() == "Y" || dt.Rows[0]["BRMExtendedBoatHouse"].ToString().Trim() == "Y")
                            {
                                divRptRower.Visible = true;
                            }


                            // Challan Register 

                            if (dt.Rows[0]["BRMChallanRegister"].ToString().Trim() == "Y")
                            {
                                rptChalanRegister.Visible = true;
                            }

                            if (dt.Rows[0]["BRMServiceWiseCollection"].ToString().Trim() == "Y")
                            {
                                rptServiceWiseCollection.Visible = true;
                            }

                            //CREDIT

                            if (dt.Rows[0]["BRMPrintBoatBooking"].ToString().Trim() == "Y")
                            {
                                rptCreditPrintBoat.Visible = true;
                            }

                            if (dt.Rows[0]["BRMTripWiseDetails"].ToString().Trim() == "Y")
                            {
                                rptCreditTripWise.Visible = true;
                            }

                            if (dt.Rows[0]["BRMPrintBoatBooking"].ToString().Trim() == "Y" || dt.Rows[0]["BRMTripWiseDetails"].ToString().Trim() == "Y")
                            {
                                divCreditReport.Visible = true;
                            }





                            // Userwise Booking Report 

                            if (dt.Rows[0]["BRMUserBookingReport"].ToString().Trim() == "Y")
                            {
                                rptUserBasedBooked.Visible = true;
                            }

                            if (dt.Rows[0]["MBoatInfoDisplay"].ToString().Trim() == "Y")
                            {
                                rptBoatInfoDisplay.Visible = true;
                            }


                            // Trip Wise Collection Report 



                            // Boat Type Rower List


                        }

                        // Offline Rights 

                        if (dt.Rows[0]["OfflineRights"].ToString().Trim() == "Y")
                        {
                            Response.Redirect(Session["LogOutUrl"].ToString().Trim());
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    public class UserRights
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string BoatHouseId { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

    }

    public void BindPaymentStatus()
    {
        try
        {
            liAlert.Visible = false;
            lblStartResponse.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var UserProfile = new UserRights()
                {
                    UserId = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("GetPaymentStatus", UserProfile).Result;

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
                            if (dt.Rows[0]["PaymentStatus"].ToString().Trim() == "U")
                            {
                                liAlert.Visible = true;
                                lblStartResponse.Visible = true;
                            }
                            else
                            {
                                lblStartResponse.Visible = false;
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
                else
                {
                    return;
                }
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void CheckBookingPaymentRights()
    {
        try
        {
            Session["KioskPaymentRights"] = "Y";
            Session["KioskOnlineRights"] = "Y";
            Session["KioskUPIRights"] = "Y";

            Session["DeptPaymentRights"] = "Y";
            Session["DeptOnlineRights"] = "Y";
            Session["DeptUPIRights"] = "Y";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var ServiceWise = new UserRights()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    QueryType = "CheckBookingRights",
                    ServiceType = Session["UserRole"].ToString().Trim(),
                    BookingId = "",
                    Input1 = Session["UserId"].ToString().Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", ServiceWise).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        // Kiosk Booking Payment Rights 

                        if (dtExists.Rows[0]["KioskPaymentRights"].ToString().Trim() == "1")
                        {
                            Session["KioskPaymentRights"] = "N";
                            Session["KioskOnlineRights"] = "N";
                            Session["KioskUPIRights"] = "N";

                            liKioskBooking.Visible = false;
                        }

                        if (dtExists.Rows[0]["KioskOnlineRights"].ToString().Trim() == "1")
                        {
                            Session["KioskOnlineRights"] = "N";
                        }

                        if (dtExists.Rows[0]["KioskUPIRights"].ToString().Trim() == "1")
                        {
                            Session["KioskUPIRights"] = "N";
                        }

                        // Department Form Payment Rights 

                        if (dtExists.Rows[0]["DeptPaymentRights"].ToString().Trim() == "1")
                        {
                            Session["DeptPaymentRights"] = "N";
                            Session["DeptOnlineRights"] = "N";
                            Session["DeptUPIRights"] = "N";
                        }

                        if (dtExists.Rows[0]["DeptOnlineRights"].ToString().Trim() == "1")
                        {
                            Session["DeptOnlineRights"] = "N";
                        }

                        if (dtExists.Rows[0]["DeptUPIRights"].ToString().Trim() == "1")
                        {
                            Session["DeptUPIRights"] = "N";
                        }

                        // Boating Service Closed Conditions

                        if (dtExists.Rows[0]["BoatingRights"].ToString().Trim() == "1")
                        {
                            Session["BMBooking"] = "N";
                            Session["BMBookingOthers"] = "N";
                            Session["BBMBulkBooking"] = "N";
                            Session["BBMAdditionalService"] = "N";

                            bBoatBooking.Visible = false;
                            //bBulkBoatBooking.Visible = false; --2021-08-23
                        }

                        // Restaurant Service Conditions

                        if (dtExists.Rows[0]["RestaurantRights"].ToString().Trim() == "1")
                        {
                            Session["BMBookingRestaurant"] = "N";

                            RBService.Visible = false;
                            //RBulkRestaurant.Visible = false;
                        }

                        // Other Service Conditions

                        if (dtExists.Rows[0]["OtherRights"].ToString().Trim() == "1")
                        {
                            Session["BMOtherService"] = "N";
                            Session["BBMBulkOtherService"] = "N";

                            bBOtherService.Visible = false;
                            //bBBulkOtherService.Visible = false;
                        }

                        // Additional Ticket Conditions

                        if (dtExists.Rows[0]["AddtlTicketRights"].ToString().Trim() == "1")
                        {
                            Session["BBMAdditionalService"] = "N";

                            bAdditionalTicket.Visible = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public string CheckSadminRights()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var SadminLists = new UserRights()
                {
                    QueryType = "CheckSadminRights",
                    ServiceType = "",
                    BoatHouseId = "",
                    Input1 = Session["UserId"].ToString().Trim(),//Passing UserId In Input 1
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", SadminLists).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SadminResponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(SadminResponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        return "Y";
                    }
                    else
                    {
                        return "N";
                    }
                }
                else
                {
                    return "N";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return "N";
        }
    }

    //Nwly added by imran on 2022-03-30bBSmartTrip_new
    protected void lnkLogOut_Click(object sender, EventArgs e)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            string sMSG = string.Empty;
            var LoginLog = new LoginLog()
            {
                QueryType = "LogOut",
                UserName = "",
                SystemIP = "",
                SessionId = "",
                PublicIP = "",
                Browser = "",
                BVersion = "",
                Log = "",
                UserId = Session["UserId"].ToString().Trim()
            };
            response = client.PostAsJsonAsync("LoginLog", LoginLog).Result;


            if (response.IsSuccessStatusCode)
            {
                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                if (StatusCode == 1)
                {
                    Response.Redirect(Session["LogOutUrl"].ToString().Trim());
                }
            }
        }

    }

    public void CheckUserLoginSession()
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            string sMSG = string.Empty;
            var LoginLog = new LoginLog()
            {
                SessionId = Session.SessionID.ToString().Trim(),
                UserId = Session["UserId"].ToString().Trim()
            };
            response = client.PostAsJsonAsync("GetSessionId", LoginLog).Result;


            if (response.IsSuccessStatusCode)
            {

                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                if (StatusCode != 1)
                {
                    Response.Redirect(Session["LogOutUrl"].ToString().Trim());
                }
            }
        }
    }
    public void GetCorporateOffice()
    {
        Session["CorpId"] = "1";
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "GetCorpLogoBasedonCorpId",
                    ServiceType = "",
                    CorpId = Session["CorpId"].ToString(),
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

                        Session["CorpLogo"] = dtExists.Rows[0]["CorpLogo"].ToString();
                        Session["CorpName"] = dtExists.Rows[0]["CorpName"].ToString();

                    }
                    else
                    {

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

    public class FA_CommonMethod
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
    public class LoginLog
    {
        public string QueryType { get; set; }
        public string UserName { get; set; }
        public string SystemIP { get; set; }
        public string SessionId { get; set; }
        public string PublicIP { get; set; }
        public string Browser { get; set; }
        public string BVersion { get; set; }
        public string Log { get; set; }
        public string UserId { get; set; }
    }

    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    protected void Page_Init(object sender, EventArgs e)
    {
        //First, check for the existence of the Anti-XSS cookie
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;

        //If the CSRF cookie is found, parse the token from the cookie.
        //Then, set the global page variable and view state user
        //key. The global variable will be used to validate that it matches 
        //in the view state form field in the Page.PreLoad method.
        if (requestCookie != null
            && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            //Set the global token variable so the cookie value can be
            //validated against the value in the view state form field in
            //the Page.PreLoad method.
            _antiXsrfTokenValue = requestCookie.Value;

            //Set the view state user key, which will be validated by the
            //framework during each request
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        //If the CSRF cookie is not found, then this is a new session.
        else
        {
            //Generate a new Anti-XSRF token
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

            //Set the view state user key, which will be validated by the
            //framework during each request
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            //Create the non-persistent CSRF cookie
            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                //Set the HttpOnly property to prevent the cookie from
                //being accessed by client side script
                HttpOnly = true,

                //Add the Anti-XSRF token to the cookie value
                Value = _antiXsrfTokenValue
            };

            //If we are using SSL, the cookie should be set to secure to
            //prevent it from being sent over HTTP connections
            if (FormsAuthentication.RequireSSL &&
                Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }

            //Add the CSRF cookie to the response
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        //During the initial page load, add the Anti-XSRF token and user
        //name to the ViewState
        if (!IsPostBack)
        {
            //Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

            //If a user name is assigned, set the user name
            ViewState[AntiXsrfUserNameKey] =
                   Context.User.Identity.Name ?? String.Empty;
        }
        //During all subsequent post backs to the page, the token value from
        //the cookie should be validated against the token in the view state
        //form field. Additionally user name should be compared to the
        //authenticated users name
        else
        {
            //Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] !=
                     (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of " +
                                    "Anti-XSRF token failed.");
            }
        }
    }

    protected void liNewComplaint_Click(object sender, EventArgs e)
    {
        Page.Controls.Add(new LiteralControl("<script>window.open('/Complaint/ComplaintHome.aspx?&qUId=" + Session["UserId"].ToString() + "&qUT=D&qM=BT&qT=A','A','toolbar=yes,location=yes,directories=yes,status=yes,menubar=yes,scrollbars=yes,resizable=yes,copyhistory=no, width=850, height=850');</script>"));
    }

    protected void liViewComplaint_Click(object sender, EventArgs e)
    {
        Page.Controls.Add(new LiteralControl("<script>window.open('/Complaint/ComplaintHome.aspx?&qUId=" + Session["UserId"].ToString() + "&qUT=D&qM=BT&qT=V','A','toolbar=yes,location=yes,directories=yes,status=yes,menubar=yes,scrollbars=yes,resizable=yes,copyhistory=no, width=850, height=850');</script>"));
    }

    protected void liSendComplaint_Click(object sender, EventArgs e)
    {
        Page.Controls.Add(new LiteralControl("<script>window.open('/Complaint/ComplaintHome.aspx?&qUId=" + Session["UserId"].ToString() + "&qUT=D&qM=BT&qT=S','A','toolbar=yes,location=yes,directories=yes,status=yes,menubar=yes,scrollbars=yes,resizable=yes,copyhistory=no, width=850, height=850');</script>"));
    }
}

