using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_UserAccessRights : System.Web.UI.Page
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
                int istart;
                int iend;
                AddProcess(0, 10, out istart, out iend);
                getEmployee();
                BindBoatingServiceList();
                BindAddlServiceList();
                BindOtherCategoryList();
                BindAdminAccess();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void getEmployee()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var User = new AdminAccess()
                {
                    QueryType = "GetEmpMaster",
                    ServiceType = "BranchUser",
                    CorpId = "",
                    Input1 = "3",
                    Input2 = Session["BoatHouseId"].ToString().Trim(),
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };
                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", User).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlUserName.DataSource = dtExists;
                        ddlUserName.DataValueField = "UserId";
                        ddlUserName.DataTextField = "EmpName";
                        ddlUserName.DataBind();
                    }
                    else
                    {
                        ddlUserName.DataBind();
                    }

                    ddlUserName.Items.Insert(0, "Select User Name");
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
       
    public void BindBoatingServiceList()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new AdminAccess()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatTypeMstr/BHId", CatType).Result;

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
                            ckblBoatingSvc.DataSource = dt;
                            ckblBoatingSvc.DataValueField = "BoatTypeId";
                            ckblBoatingSvc.DataTextField = "BoatType";
                            ckblBoatingSvc.DataBind();
                        }
                        else
                        {
                            ckblBoatingSvc.DataBind();
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

    public void BindAddlServiceList()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new AdminAccess()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("AdditionalBoatOtherService", CatType).Result;

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
                            ckblAddlSvc.DataSource = dt;
                            ckblAddlSvc.DataValueField = "ConfigId";
                            ckblAddlSvc.DataTextField = "ConfigName";
                            ckblAddlSvc.DataBind();
                        }
                        else
                        {
                            ckblAddlSvc.DataBind();
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

    public void BindOtherCategoryList()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new AdminAccess()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CategoryList/BhId", CatType).Result;

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
                            ckblOtherSvc.DataSource = dt;
                            ckblOtherSvc.DataValueField = "ConfigId";
                            ckblOtherSvc.DataTextField = "ConfigName";
                            ckblOtherSvc.DataBind();
                        }
                        else
                        {
                            ckblOtherSvc.DataBind();
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

    //public void BindEmpAdminAccess()
    //{
    //    try
    //    {
    //        string Userid = string.Empty;
    //        string BranchId = string.Empty;
    //        string Svc = string.Empty;

    //        string UserId = string.Empty;
    //        string UserRole = string.Empty;

    //        string MBInfoDisp = string.Empty;

    //        string MBS = string.Empty;
    //        string BMaster = string.Empty;
    //        string BTransaction = string.Empty;
    //        string BBooking = string.Empty;
    //        string BReports = string.Empty;
    //        string BRestaurant = string.Empty;

    //        string BGeneratingBoardingPass = string.Empty;
    //        string BGenerateManualTicket = string.Empty;

    //        string BMBooking = string.Empty;
    //        string BMBookingOthers = string.Empty;
    //        string BMBulkBooking = string.Empty;
    //        string BMAdditionalService = string.Empty;

    //        string BMOtherService = string.Empty;
    //        string BMBulkOtherService = string.Empty;
    //        string BMKioskBooking = string.Empty;
    //        string BMTripSheet = string.Empty;
    //        string BMChangeTripSheet = string.Empty;
    //        string BMBoatReTripDetails = string.Empty;
    //        string BMChangeBoatDetails = string.Empty;
    //        string BMCancellation = string.Empty;
    //        string BMReSchedule = string.Empty;

    //        string TMMaterialPur = string.Empty;
    //        string TMMaterialIss = string.Empty;
    //        string TMTripSheetSettle = string.Empty;
    //        string TMRowerSettle = string.Empty;
    //        string TMRefundCounter = string.Empty;
    //        string TMStockEntryMaintance = string.Empty;
    //        string TMReceiptBalanceRefund = string.Empty;



    //        string RMBooking = string.Empty;
    //        string RMOtherSvc = string.Empty;
    //        string RMRestaurantService = string.Empty;
    //        //Newly Added
    //        string RMAdditionalTicket = string.Empty;
    //        string RMAbstractAdditionalTicket = string.Empty;
    //        string RMDepositStatus = string.Empty;
    //        string RMDiscountReport = string.Empty;

    //        string RMCashinHands = string.Empty;
    //        string RMExtendedBoatHouse = string.Empty;
    //        string RMPrintBoatBooking = string.Empty;

    //        string RMTripWiseDetails = string.Empty;
    //        string RMReceiptBalance = string.Empty;

    //        string RMRePrintReport = string.Empty;
    //        string RMQRCodeGeneration = string.Empty;




    //        //Newly Added
    //        string RMAbstractBoatBook = string.Empty;
    //        string RMAbstractOthSvc = string.Empty;
    //        string RMAbstractResSvc = string.Empty;


    //        string RMAvailBoatCapacity = string.Empty;
    //        string RMBoatwiseTrip = string.Empty;
    //        string RMTripSheetSettle = string.Empty;

    //        string RMRowerCharges = string.Empty;
    //        string RMBoatCancellation = string.Empty;
    //        string RMRowerSettle = string.Empty;

    //        string RMChallanRegister = string.Empty;
    //        string RMAbstractChallanRegister = string.Empty;
    //        string RMServiceWiseCollection = string.Empty;

    //        string RMUserBookingReport = string.Empty;
    //        string RMTripWiseCollection = string.Empty;
    //        string RMBoatTypeRowerList = string.Empty;

    //        string BoatHouseId = string.Empty;
    //        string BoatHouseName = string.Empty;

    //        string BBoatingService = string.Empty;
    //        string BAdditionalService = string.Empty;
    //        string BOtherService = string.Empty;
    //        string BTripSheetOptions = string.Empty;
    //        string BDepositRefundOptions = string.Empty;
    //        string BOfflineRights = string.Empty;

    //        //Newly Added

    //        string BMKioskOtherService = string.Empty;

    //        //Newly Added


    //        using (var client = new HttpClient())
    //        {

    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            if (ddlUserName.SelectedValue != "")
    //            {
    //                if (Convert.ToInt32(ddlUserName.SelectedValue) > 0)
    //                {
    //                    Userid = ddlUserName.SelectedValue.Trim();
    //                    Svc = "UserRole";
    //                }
    //                else
    //                {
    //                    Userid = "";
    //                    Svc = "UserRole";
    //                }
    //            }
    //            else
    //            {
    //                Userid = "";
    //                Svc = "UserRole";
    //            }

    //            var UserRights = new AdminAccess()
    //            {
    //                QueryType = "UserAccess",
    //                ServiceType = Svc.Trim(),
    //                UserId = Userid.Trim(),
    //                UserRole = "3",
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim()
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("GetUserAccess", UserRights).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
    //                DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

    //                if (dtExists.Rows.Count > 0)
    //                {
    //                    UserId = dtExists.Rows[0]["UserId"].ToString();
    //                    UserRole = dtExists.Rows[0]["UserRole"].ToString();
    //                    MBInfoDisp = dtExists.Rows[0]["MBoatInfoDisplay"].ToString();

    //                    MBS = dtExists.Rows[0]["MBoating"].ToString();
    //                    BMaster = dtExists.Rows[0]["BMaster"].ToString();
    //                    BTransaction = dtExists.Rows[0]["BTransaction"].ToString();
    //                    BBooking = dtExists.Rows[0]["BBooking"].ToString();
    //                    BReports = dtExists.Rows[0]["BReports"].ToString();
    //                    BRestaurant = dtExists.Rows[0]["BRestaurant"].ToString();

    //                    BGeneratingBoardingPass = dtExists.Rows[0]["BGeneratingBoardingPass"].ToString();
    //                    BGenerateManualTicket = dtExists.Rows[0]["BGenerateManualTicket"].ToString();

    //                    BMBooking = dtExists.Rows[0]["BBMBooking"].ToString();
    //                    BMBookingOthers = dtExists.Rows[0]["BBMBookingOthers"].ToString();
    //                    BMBulkBooking = dtExists.Rows[0]["BBMBulkBooking"].ToString();
    //                    BMAdditionalService = dtExists.Rows[0]["BBMAdditionalService"].ToString();
    //                    BMOtherService = dtExists.Rows[0]["BBMOtherService"].ToString();
    //                    BMBulkOtherService = dtExists.Rows[0]["BBMBulkOtherService"].ToString();
    //                    BMKioskBooking = dtExists.Rows[0]["BBMKioskBooking"].ToString();
    //                    //Newly Added BMKioskOtherService
    //                    BMKioskOtherService = dtExists.Rows[0]["BMKioskOtherService"].ToString();
    //                    //Newly Added

    //                    BMTripSheet = dtExists.Rows[0]["BBMTripSheet"].ToString();
    //                    BMChangeTripSheet = dtExists.Rows[0]["BBMChangeTripSheet"].ToString();
    //                    BMBoatReTripDetails = dtExists.Rows[0]["BBMBoatReTripDetails"].ToString();
    //                    BMChangeBoatDetails = dtExists.Rows[0]["BBMChangeBoatDetails"].ToString();
    //                    BMCancellation = dtExists.Rows[0]["BBMCancellation"].ToString();
    //                    BMReSchedule = dtExists.Rows[0]["BBMReScheduling"].ToString();

    //                    TMMaterialPur = dtExists.Rows[0]["BTMMaterialPurchase"].ToString();
    //                    TMMaterialIss = dtExists.Rows[0]["BTMMaterialIssue"].ToString();
    //                    TMTripSheetSettle = dtExists.Rows[0]["BTMTripSheetSettle"].ToString();
    //                    TMRowerSettle = dtExists.Rows[0]["BTMRowerSettle"].ToString();
    //                    TMRefundCounter = dtExists.Rows[0]["BTMRefundCounter"].ToString();
    //                    TMStockEntryMaintance = dtExists.Rows[0]["BTMFoodStockEntryMaintance"].ToString();
    //                    TMReceiptBalanceRefund= dtExists.Rows[0]["BTMReceiptBalanceRefund"].ToString();

    //                    RMBooking = dtExists.Rows[0]["BRMBooking"].ToString();
    //                    RMOtherSvc = dtExists.Rows[0]["BRMOtherService"].ToString();
    //                    RMRestaurantService = dtExists.Rows[0]["BRMRestaurantService"].ToString();
    //                    //Newly Added
    //                    RMAdditionalTicket = dtExists.Rows[0]["BRMAdditionalTicket"].ToString().Trim();
    //                    RMAbstractAdditionalTicket = dtExists.Rows[0]["BRMAbstractAdditionalTicket"].ToString().Trim();
    //                    RMDepositStatus = dtExists.Rows[0]["BRMDepositStatus"].ToString().Trim();
    //                    RMDiscountReport = dtExists.Rows[0]["BRMDiscountReport"].ToString().Trim();

    //                     RMCashinHands = dtExists.Rows[0]["BRMCashinHands"].ToString().Trim();
    //                     RMExtendedBoatHouse = dtExists.Rows[0]["BRMExtendedBoatHouse"].ToString().Trim();
    //                     RMPrintBoatBooking = dtExists.Rows[0]["BRMPrintBoatBooking"].ToString().Trim();

    //                    RMTripWiseDetails = dtExists.Rows[0]["BRMTripWiseDetails"].ToString().Trim();
    //                    RMReceiptBalance = dtExists.Rows[0]["BRMReceiptBalance"].ToString().Trim();
    //                    //RMAbstractBooking = dtExists.Rows[0]["BRMAbstractBooking"].ToString().Trim();
    //                    RMRePrintReport = dtExists.Rows[0]["BRMRePrintReport"].ToString().Trim();
    //                    RMQRCodeGeneration = dtExists.Rows[0]["BRMQRCodeGeneration"].ToString().Trim();

    //                    //Newly Added

    //                    RMAbstractBoatBook = dtExists.Rows[0]["BRMAbstractBoatBook"].ToString();
    //                    RMAbstractOthSvc = dtExists.Rows[0]["BRMAbstractOthSvc"].ToString();
    //                    RMAbstractResSvc = dtExists.Rows[0]["BRMAbstractResSvc"].ToString();

    //                    RMAvailBoatCapacity = dtExists.Rows[0]["BRMAvailBoatCapacity"].ToString();
    //                    RMBoatwiseTrip = dtExists.Rows[0]["BRMBoatwiseTrip"].ToString();
    //                    RMTripSheetSettle = dtExists.Rows[0]["BRMTripSheet"].ToString();

    //                    RMRowerCharges = dtExists.Rows[0]["BRMRowerCharges"].ToString();
    //                    RMBoatCancellation = dtExists.Rows[0]["BRMBoatCancellation"].ToString();
    //                    RMRowerSettle = dtExists.Rows[0]["BRMRowerSettle"].ToString();

    //                    RMChallanRegister = dtExists.Rows[0]["BRMChallanRegister"].ToString();
    //                    //RMAbstractChallanRegister = dtExists.Rows[0]["BRMAbstractChallanRegister"].ToString();
    //                    RMServiceWiseCollection = dtExists.Rows[0]["BRMServiceWiseCollection"].ToString();

    //                    RMUserBookingReport = dtExists.Rows[0]["BRMUserBookingReport"].ToString();
    //                    RMTripWiseCollection = dtExists.Rows[0]["BRMTripWiseCollection"].ToString();
    //                    RMBoatTypeRowerList = dtExists.Rows[0]["BRMBoatTypeRowerList"].ToString();

    //                    BoatHouseId = dtExists.Rows[0]["BoatHouseId"].ToString();
    //                    BoatHouseName = dtExists.Rows[0]["BoatHouseName"].ToString();

    //                    BBoatingService = dtExists.Rows[0]["BBoatingService"].ToString();
    //                    BAdditionalService = dtExists.Rows[0]["BAdditionalService"].ToString();
    //                    BOtherService = dtExists.Rows[0]["BOtherService"].ToString();
    //                    BTripSheetOptions = dtExists.Rows[0]["BTripSheetOptions"].ToString();
    //                    BDepositRefundOptions = dtExists.Rows[0]["BDepositRefundOptions"].ToString();
    //                    BOfflineRights = dtExists.Rows[0]["OfflineRights"].ToString();

    //                    ddlUserName.SelectedValue = UserId.Trim();
    //                    ddlUserName.Enabled = true;

    //                    //Info Display
    //                    chkBoatInfoDisp.Checked = false;

    //                    if (MBInfoDisp.Trim() == "Y")
    //                    {
    //                        chkBoatInfoDisp.Checked = true;
    //                    }

    //                    // Module Rights

    //                    ChkBoatModule.Checked = false;

    //                    if (MBS.Trim() == "Y")
    //                    {
    //                        ChkBoatModule.Checked = true;
    //                    }

    //                    // Android Offline Rights

    //                    ChkAnroidOffline.Checked = false;

    //                    if (BOfflineRights.Trim() == "Y")
    //                    {
    //                        ChkAnroidOffline.Checked = true;
    //                    }

    //                    // Menu Rights

    //                    chkMenuBooking.Checked = false;
    //                    chkMenuTransaction.Checked = false;
    //                    chkMenuReports.Checked = false;

    //                    if (BBooking.Trim() == "Y" || BBooking.Trim().ToUpper() == "YES")
    //                    {
    //                        chkMenuBooking.Checked = true;
    //                    }

    //                    if (BTransaction.Trim() == "Y" || BTransaction.Trim().ToUpper() == "YES")
    //                    {
    //                        chkMenuTransaction.Checked = true;
    //                    }

    //                    if (BReports.Trim() == "Y" || BReports.Trim().ToUpper() == "YES")
    //                    {
    //                        chkMenuReports.Checked = true;
    //                    }



    //                    // Sub Menu Rights For Boat Service Menu

    //                    if (BMBooking.Trim() == "Y" && BMBulkBooking.Trim() == "Y")
    //                    {
    //                        BindBoatingServiceList();

    //                        if (BBoatingService.Trim() == "")
    //                        {
    //                            chkMenuBoatingSvc.Checked = false;
    //                            foreach (ListItem item in ckblBoatingSvc.Items)
    //                            {
    //                                item.Selected = false;
    //                            }

    //                        }
    //                        else
    //                        {
    //                            chkMenuBoatingSvc.Checked = true;
    //                            string bBoatSvc = BBoatingService.Trim().Trim();
    //                            string[] sbBoatSvc = bBoatSvc.Split(',');
    //                            int bBoatSvcCount = sbBoatSvc.Count();
    //                            for (int i = 0; i < bBoatSvcCount; i++)
    //                            {
    //                                foreach (ListItem item in ckblBoatingSvc.Items)
    //                                {
    //                                    string selectedValue = item.Value;
    //                                    if (selectedValue == sbBoatSvc[i].ToString())
    //                                    {
    //                                        item.Selected = true;
    //                                    }
    //                                }
    //                            }

    //                        }
    //                    }
    //                    else
    //                    {
    //                        BindBoatingServiceList();
    //                        chkMenuBoatingSvc.Checked = false;
    //                    }

    //                    // Sub Menu Rights For Additional Service Menu

    //                    if (BMAdditionalService.Trim() == "Y")
    //                    {
    //                        BindAddlServiceList();

    //                        if (BAdditionalService.Trim() == "")
    //                        {
    //                            chkMenuAdditionalSvc.Checked = false;
    //                            foreach (ListItem item in ckblAddlSvc.Items)
    //                            {
    //                                item.Selected = false;
    //                            }

    //                        }
    //                        else
    //                        {
    //                            chkMenuAdditionalSvc.Checked = true;
    //                            string bAddlSvc = BAdditionalService.Trim();
    //                            string[] sbAddlSvc = bAddlSvc.Split(',');
    //                            int bAddlSvcCount = sbAddlSvc.Count();
    //                            for (int i = 0; i < bAddlSvcCount; i++)
    //                            {
    //                                foreach (ListItem item in ckblAddlSvc.Items)
    //                                {
    //                                    string selectedValue = item.Value;
    //                                    if (selectedValue == sbAddlSvc[i].ToString())
    //                                    {
    //                                        item.Selected = true;
    //                                    }
    //                                }
    //                            }

    //                        }
    //                    }
    //                    else
    //                    {
    //                        BindAddlServiceList();
    //                        chkMenuAdditionalSvc.Checked = false;
    //                    }


    //                    // Sub Menu Rights For OtherService Menu

    //                    if (BMOtherService.Trim() == "Y")
    //                    {
    //                        BindOtherCategoryList();

    //                        if (BOtherService.Trim() == "")
    //                        {
    //                            chkMenuOtherSvc.Checked = false;
    //                            foreach (ListItem item in ckblOtherSvc.Items)
    //                            {
    //                                item.Selected = true;
    //                            }

    //                        }
    //                        else
    //                        {
    //                            chkMenuOtherSvc.Checked = true;
    //                            string bOtherSvc = BOtherService.Trim();
    //                            string[] sbOtherSvc = bOtherSvc.Split(',');
    //                            int bOtherSvcCount = sbOtherSvc.Count();
    //                            for (int i = 0; i < bOtherSvcCount; i++)
    //                            {
    //                                foreach (ListItem item in ckblOtherSvc.Items)
    //                                {
    //                                    string selectedValue = item.Value;
    //                                    if (selectedValue == sbOtherSvc[i].ToString())
    //                                    {
    //                                        item.Selected = true;
    //                                    }
    //                                }
    //                            }

    //                        }
    //                    }
    //                    else
    //                    {
    //                        BindOtherCategoryList();
    //                        chkMenuOtherSvc.Checked = false;
    //                    }

    //                    // Sub Menu Rights For Trip Sheet Menu


    //                    if (BMTripSheet.Trim() == "Y")
    //                    {
    //                        if (BTripSheetOptions.Trim() == "")
    //                        {
    //                            chkMenuTripSheet.Checked = false;
    //                            foreach (ListItem itemTS in ckblTripSheet.Items)
    //                            {
    //                                itemTS.Selected = false;
    //                            }

    //                        }
    //                        else
    //                        {
    //                            chkMenuTripSheet.Checked = true;
    //                            string bTripSheet = BTripSheetOptions.Trim();
    //                            string[] sbTripSheet = bTripSheet.Split(',');
    //                            int bTripSheetCount = sbTripSheet.Count();
    //                            for (int i = 0; i < bTripSheetCount; i++)
    //                            {
    //                                if (sbTripSheet[i].ToString() == "Y")
    //                                {
    //                                    ckblTripSheet.Items[i].Selected = true;
    //                                }
    //                                else
    //                                {
    //                                    ckblTripSheet.Items[i].Selected = false;
    //                                }
    //                            }

    //                        }
    //                    }
    //                    else
    //                    {
    //                        chkMenuTripSheet.Checked = false;
    //                    }


    //                    // Sub Menu Rights For Booking Menu

    //                    for (int i = 0; i < ckblBooking.Items.Count; i++)
    //                    {
    //                        if (BMBooking.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[0].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[0].Selected = false;
    //                        }
    //                        if (BMBookingOthers.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[1].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[1].Selected = false;
    //                        }
    //                        if (BMBulkBooking.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[2].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[2].Selected = false;
    //                        }
    //                        if (BMAdditionalService.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[3].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[3].Selected = false;
    //                        }
    //                        if (BMOtherService.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[4].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[4].Selected = false;
    //                        }
    //                        if (BMBulkOtherService.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[5].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[5].Selected = false;
    //                        }

    //                        if (BMKioskBooking.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[6].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[6].Selected = false;
    //                        }
    //                        if (BMTripSheet.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[7].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[7].Selected = false;
    //                        }
    //                        if (BMChangeTripSheet.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[8].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[8].Selected = false;
    //                        }
    //                        if (BMBoatReTripDetails.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[9].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[9].Selected = false;
    //                        }
    //                        if (BMChangeBoatDetails.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[10].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[10].Selected = false;
    //                        }
    //                        if (BMCancellation.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[11].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[11].Selected = false;
    //                        }
    //                        if (BMReSchedule.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[12].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[12].Selected = false;
    //                        }

    //                        if (BRestaurant.Trim() == "Y" || BRestaurant.Trim().ToUpper() == "YES")
    //                        {
    //                            ckblBooking.Items[13].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[13].Selected = false;
    //                        }

    //                        if (BGeneratingBoardingPass.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[14].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[14].Selected = false;
    //                        }
    //                        if (BGenerateManualTicket.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[15].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[15].Selected = false;
    //                        }

    //                        //Newly added

    //                        if (BMKioskOtherService.Trim() == "Y")
    //                        {
    //                            ckblBooking.Items[16].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblBooking.Items[16].Selected = false;
    //                        }

    //                        //Newly added

    //                    }

    //                    // Sub Menu Rights For Transaction Menu

    //                    for (int i = 0; i < ckblTransaction.Items.Count; i++)
    //                    {
    //                        if (TMMaterialPur.Trim() == "Y")
    //                        {
    //                            ckblTransaction.Items[0].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblTransaction.Items[0].Selected = false;
    //                        }
    //                        if (TMMaterialIss.Trim() == "Y")
    //                        {
    //                            ckblTransaction.Items[1].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblTransaction.Items[1].Selected = false;
    //                        }
    //                        if (TMTripSheetSettle.Trim() == "Y")
    //                        {
    //                            ckblTransaction.Items[2].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblTransaction.Items[2].Selected = false;
    //                        }
    //                        if (TMRowerSettle.Trim() == "Y")
    //                        {
    //                            ckblTransaction.Items[3].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblTransaction.Items[3].Selected = false;
    //                        }
    //                        if (TMRefundCounter.Trim() == "Y")
    //                        {
    //                            ckblTransaction.Items[4].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblTransaction.Items[4].Selected = false;
    //                        }

    //                        if (TMStockEntryMaintance.Trim() == "Y")
    //                        {
    //                            ckblTransaction.Items[5].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblTransaction.Items[5].Selected = false;
    //                        }

    //                        if(TMReceiptBalanceRefund.Trim() == "Y")
    //                        {
    //                            ckblTransaction.Items[6].Selected = true;
    //                        }

    //                        else
    //                        {
    //                            ckblTransaction.Items[6].Selected = false;
    //                        }


    //                    }

    //                    // Sub Menu Rights For Deposit Refund Menu


    //                    if (TMTripSheetSettle.Trim() == "Y")
    //                    {
    //                        if (BDepositRefundOptions.Trim() == "")
    //                        {
    //                            chkMenuDepositRefund.Checked = false;
    //                            foreach (ListItem itemTS in ckblDepositRefund.Items)
    //                            {
    //                                itemTS.Selected = false;
    //                            }

    //                        }
    //                        else
    //                        {
    //                            chkMenuDepositRefund.Checked = true;
    //                            string bDepositRefund = BDepositRefundOptions.Trim();
    //                            string[] sbDepositRefund = bDepositRefund.Split(',');
    //                            int bDepositRefundCount = sbDepositRefund.Count();
    //                            for (int i = 0; i < bDepositRefundCount; i++)
    //                            {
    //                                if (sbDepositRefund[i].ToString() == "Y")
    //                                {
    //                                    ckblDepositRefund.Items[i].Selected = true;
    //                                }
    //                                else
    //                                {
    //                                    ckblDepositRefund.Items[i].Selected = false;
    //                                }
    //                            }

    //                        }
    //                    }
    //                    else
    //                    {
    //                        chkMenuDepositRefund.Checked = false;
    //                    }

    //                    // Sub Menu Rights For Reports Menu



    //                    for (int i = 0; i < ckblReports.Items.Count; i++)
    //                    {
    //                        if (RMBooking.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[0].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[0].Selected = false;
    //                        }
    //                        if (RMOtherSvc.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[1].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[1].Selected = false;
    //                        }
    //                        if (RMRestaurantService.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[2].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[2].Selected = false;
    //                        }


    //                        if (RMAbstractBoatBook.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[3].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[3].Selected = false;
    //                        }
    //                        if (RMAbstractOthSvc.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[4].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[4].Selected = false;
    //                        }
    //                        if (RMAbstractResSvc.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[5].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[5].Selected = false;
    //                        }

    //                        if (RMAvailBoatCapacity.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[6].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[6].Selected = false;
    //                        }
    //                        if (RMBoatwiseTrip.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[7].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[7].Selected = false;
    //                        }
    //                        if (RMTripSheetSettle.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[8].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[8].Selected = false;
    //                        }

    //                        if (RMRowerCharges.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[9].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[9].Selected = false;
    //                        }
    //                        if (RMBoatCancellation.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[10].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[10].Selected = false;
    //                        }
    //                        if (RMRowerSettle.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[11].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[11].Selected = false;
    //                        }
    //                        if (RMChallanRegister.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[12].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[12].Selected = false;
    //                        }
    //                        if (RMServiceWiseCollection.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[13].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[13].Selected = false;
    //                        }
    //                        if (RMUserBookingReport.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[14].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[14].Selected = false;
    //                        }
    //                        if (RMTripWiseCollection.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[15].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[15].Selected = false;
    //                        }
    //                        if (RMBoatTypeRowerList.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[16].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[16].Selected = false;
    //                        }
    //                        //if (RMAbstractChallanRegister.Trim() == "Y")                          
    //                        //{
    //                        //    ckblReports.Items[17].Selected = true;
    //                        //}
    //                        //else
    //                        //{
    //                        //    ckblReports.Items[17].Selected = false;
    //                        //}
    //                        //Newly Added 
    //                        if (RMAdditionalTicket.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[17].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[17].Selected = false;
    //                        }

    //                        if (RMAbstractAdditionalTicket.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[18].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[18].Selected = false;
    //                        }

    //                        if (RMDepositStatus.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[19].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[19].Selected = false;
    //                        }
    //                        if (RMDiscountReport.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[20].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[20].Selected = false;
    //                        }


    //                        if (RMCashinHands.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[21].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[21].Selected = false;
    //                        }
    //                        if (RMExtendedBoatHouse.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[22].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[22].Selected = false;
    //                        }
    //                        if (RMPrintBoatBooking.Trim() == "Y")
    //                        {
    //                            ckblReports.Items[23].Selected = true;
    //                        }
    //                        else
    //                        {
    //                            ckblReports.Items[23].Selected = false;
    //                        }
    //                    }
    //                }


    //                else
    //                {
    //                    ChkBoatModule.Checked = false;
    //                    ChkAnroidOffline.Checked = false;
    //                    chkMenuBooking.Checked = false;
    //                    chkMenuTransaction.Checked = false;
    //                    chkMenuReports.Checked = false;

    //                    chkMenuBoatingSvc.Checked = false;
    //                    chkMenuOtherSvc.Checked = false;
    //                    chkMenuTripSheet.Checked = false;

    //                    ckblBooking.ClearSelection();
    //                    ckblTransaction.ClearSelection();
    //                    ckblReports.ClearSelection();
    //                    ckblBoatingSvc.ClearSelection();
    //                    ckblOtherSvc.ClearSelection();
    //                    ckblTripSheet.ClearSelection();
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

    public void BindEmpAdminAccess()
    {
        try
        {
            string Userid = string.Empty;
            string BranchId = string.Empty;
            string Svc = string.Empty;

            string UserId = string.Empty;
            string UserRole = string.Empty;

            string MBInfoDisp = string.Empty;

            string MBS = string.Empty;
            string BMaster = string.Empty;
            string BTransaction = string.Empty;
            string BBooking = string.Empty;
            string BReports = string.Empty;
            string BRestaurant = string.Empty;

            string BGeneratingBoardingPass = string.Empty;
            string BGenerateManualTicket = string.Empty;

            string BMBooking = string.Empty;
            string BMBookingOthers = string.Empty;
            string BMBulkBooking = string.Empty;
            string BMAdditionalService = string.Empty;

            string BMOtherService = string.Empty;
            string BMBulkOtherService = string.Empty;
            string BMKioskBooking = string.Empty;
            string BMTripSheet = string.Empty;
            string BMChangeTripSheet = string.Empty;
            string BMBoatReTripDetails = string.Empty;
            string BMChangeBoatDetails = string.Empty;
            string BMCancellation = string.Empty;
            string BMReSchedule = string.Empty;

            string TMMaterialPur = string.Empty;
            string TMMaterialIss = string.Empty;
            string TMTripSheetSettle = string.Empty;
            string TMRowerSettle = string.Empty;
            string TMRefundCounter = string.Empty;
            string TMStockEntryMaintance = string.Empty;
            string TMReceiptBalanceRefund = string.Empty;



            string RMBooking = string.Empty;
            string RMOtherSvc = string.Empty;
            string RMRestaurantService = string.Empty;
            //Newly Added
            string RMAdditionalTicket = string.Empty;
            string RMAbstractAdditionalTicket = string.Empty;
            string RMDepositStatus = string.Empty;
            string RMDiscountReport = string.Empty;

            string RMCashinHands = string.Empty;
            string RMExtendedBoatHouse = string.Empty;
            string RMPrintBoatBooking = string.Empty;

            string RMTripWiseDetails = string.Empty;
            string RMReceiptBalance = string.Empty;

            string RMRePrintReport = string.Empty;
            string RMQRCodeGeneration = string.Empty;




            //Newly Added
            string RMAbstractBoatBook = string.Empty;
            string RMAbstractOthSvc = string.Empty;
            string RMAbstractResSvc = string.Empty;


            string RMAvailBoatCapacity = string.Empty;
            string RMBoatwiseTrip = string.Empty;
            string RMTripSheetSettle = string.Empty;

            string RMRowerCharges = string.Empty;
            string RMBoatCancellation = string.Empty;
            string RMRowerSettle = string.Empty;

            string RMChallanRegister = string.Empty;
            string RMAbstractChallanRegister = string.Empty;
            string RMServiceWiseCollection = string.Empty;

            string RMUserBookingReport = string.Empty;
            string RMTripWiseCollection = string.Empty;
            string RMBoatTypeRowerList = string.Empty;

            string BoatHouseId = string.Empty;
            string BoatHouseName = string.Empty;

            string BBoatingService = string.Empty;
            string BAdditionalService = string.Empty;
            string BOtherService = string.Empty;
            string BTripSheetOptions = string.Empty;
            string BDepositRefundOptions = string.Empty;
            string BOfflineRights = string.Empty;

            //Newly Added

            string BMKioskOtherService = string.Empty;

            //Newly Added


            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (ddlUserName.SelectedValue != "")
                {
                    if (Convert.ToInt32(ddlUserName.SelectedValue) > 0)
                    {
                        Userid = ddlUserName.SelectedValue.Trim();
                        Svc = "UserRole";
                    }
                    else
                    {
                        Userid = "";
                        Svc = "UserRole";
                    }
                }
                else
                {
                    Userid = "";
                    Svc = "UserRole";
                }

                var UserRights = new AdminAccess()
                {
                    QueryType = "UserAccess",
                    ServiceType = Svc.Trim(),
                    UserId = Userid.Trim(),
                    UserRole = "3",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("GetUserAccess", UserRights).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        UserId = dtExists.Rows[0]["UserId"].ToString();
                        UserRole = dtExists.Rows[0]["UserRole"].ToString();
                        MBInfoDisp = dtExists.Rows[0]["MBoatInfoDisplay"].ToString();

                        MBS = dtExists.Rows[0]["MBoating"].ToString();
                        BMaster = dtExists.Rows[0]["BMaster"].ToString();
                        BTransaction = dtExists.Rows[0]["BTransaction"].ToString();
                        BBooking = dtExists.Rows[0]["BBooking"].ToString();
                        BReports = dtExists.Rows[0]["BReports"].ToString();
                        BRestaurant = dtExists.Rows[0]["BRestaurant"].ToString();

                        BGeneratingBoardingPass = dtExists.Rows[0]["BGeneratingBoardingPass"].ToString();
                        BGenerateManualTicket = dtExists.Rows[0]["BGenerateManualTicket"].ToString();

                        BMBooking = dtExists.Rows[0]["BBMBooking"].ToString();
                        BMBookingOthers = dtExists.Rows[0]["BBMBookingOthers"].ToString();
                        BMBulkBooking = dtExists.Rows[0]["BBMBulkBooking"].ToString();
                        BMAdditionalService = dtExists.Rows[0]["BBMAdditionalService"].ToString();
                        BMOtherService = dtExists.Rows[0]["BBMOtherService"].ToString();
                        BMBulkOtherService = dtExists.Rows[0]["BBMBulkOtherService"].ToString();
                        BMKioskBooking = dtExists.Rows[0]["BBMKioskBooking"].ToString();
                        //Newly Added BMKioskOtherService
                        BMKioskOtherService = dtExists.Rows[0]["BMKioskOtherService"].ToString();
                        //Newly Added

                        BMTripSheet = dtExists.Rows[0]["BBMTripSheet"].ToString();
                        BMChangeTripSheet = dtExists.Rows[0]["BBMChangeTripSheet"].ToString();
                        BMBoatReTripDetails = dtExists.Rows[0]["BBMBoatReTripDetails"].ToString();
                        BMChangeBoatDetails = dtExists.Rows[0]["BBMChangeBoatDetails"].ToString();
                        BMCancellation = dtExists.Rows[0]["BBMCancellation"].ToString();
                        BMReSchedule = dtExists.Rows[0]["BBMReScheduling"].ToString();

                        TMMaterialPur = dtExists.Rows[0]["BTMMaterialPurchase"].ToString();
                        TMMaterialIss = dtExists.Rows[0]["BTMMaterialIssue"].ToString();
                        TMTripSheetSettle = dtExists.Rows[0]["BTMTripSheetSettle"].ToString();
                        TMRowerSettle = dtExists.Rows[0]["BTMRowerSettle"].ToString();
                        TMRefundCounter = dtExists.Rows[0]["BTMRefundCounter"].ToString();
                        TMStockEntryMaintance = dtExists.Rows[0]["BTMFoodStockEntryMaintance"].ToString();
                        TMReceiptBalanceRefund = dtExists.Rows[0]["BTMReceiptBalanceRefund"].ToString();

                        RMBooking = dtExists.Rows[0]["BRMBooking"].ToString();
                        RMOtherSvc = dtExists.Rows[0]["BRMOtherService"].ToString();
                        RMRestaurantService = dtExists.Rows[0]["BRMRestaurantService"].ToString();
                        //Newly Added
                        RMAdditionalTicket = dtExists.Rows[0]["BRMAdditionalTicket"].ToString().Trim();
                        RMAbstractAdditionalTicket = dtExists.Rows[0]["BRMAbstractAdditionalTicket"].ToString().Trim();
                        RMDepositStatus = dtExists.Rows[0]["BRMDepositStatus"].ToString().Trim();
                        RMDiscountReport = dtExists.Rows[0]["BRMDiscountReport"].ToString().Trim();

                        RMCashinHands = dtExists.Rows[0]["BRMCashinHands"].ToString().Trim();
                        RMExtendedBoatHouse = dtExists.Rows[0]["BRMExtendedBoatHouse"].ToString().Trim();
                        RMPrintBoatBooking = dtExists.Rows[0]["BRMPrintBoatBooking"].ToString().Trim();

                        RMTripWiseDetails = dtExists.Rows[0]["BRMTripWiseDetails"].ToString().Trim();
                        RMReceiptBalance = dtExists.Rows[0]["BRMReceiptBalance"].ToString().Trim();
                        //RMAbstractBooking = dtExists.Rows[0]["BRMAbstractBooking"].ToString().Trim();
                        RMRePrintReport = dtExists.Rows[0]["BRMRePrintReport"].ToString().Trim();
                        RMQRCodeGeneration = dtExists.Rows[0]["BRMQRCodeGeneration"].ToString().Trim();

                        //Newly Added

                        RMAbstractBoatBook = dtExists.Rows[0]["BRMAbstractBoatBook"].ToString();
                        RMAbstractOthSvc = dtExists.Rows[0]["BRMAbstractOthSvc"].ToString();
                        RMAbstractResSvc = dtExists.Rows[0]["BRMAbstractResSvc"].ToString();

                        RMAvailBoatCapacity = dtExists.Rows[0]["BRMAvailBoatCapacity"].ToString();
                        RMBoatwiseTrip = dtExists.Rows[0]["BRMBoatwiseTrip"].ToString();
                        RMTripSheetSettle = dtExists.Rows[0]["BRMTripSheet"].ToString();

                        RMRowerCharges = dtExists.Rows[0]["BRMRowerCharges"].ToString();
                        RMBoatCancellation = dtExists.Rows[0]["BRMBoatCancellation"].ToString();
                        RMRowerSettle = dtExists.Rows[0]["BRMRowerSettle"].ToString();

                        RMChallanRegister = dtExists.Rows[0]["BRMChallanRegister"].ToString();
                        //RMAbstractChallanRegister = dtExists.Rows[0]["BRMAbstractChallanRegister"].ToString();
                        RMServiceWiseCollection = dtExists.Rows[0]["BRMServiceWiseCollection"].ToString();

                        RMUserBookingReport = dtExists.Rows[0]["BRMUserBookingReport"].ToString();
                        RMTripWiseCollection = dtExists.Rows[0]["BRMTripWiseCollection"].ToString();
                        RMBoatTypeRowerList = dtExists.Rows[0]["BRMBoatTypeRowerList"].ToString();

                        BoatHouseId = dtExists.Rows[0]["BoatHouseId"].ToString();
                        BoatHouseName = dtExists.Rows[0]["BoatHouseName"].ToString();

                        BBoatingService = dtExists.Rows[0]["BBoatingService"].ToString();
                        BAdditionalService = dtExists.Rows[0]["BAdditionalService"].ToString();
                        BOtherService = dtExists.Rows[0]["BOtherService"].ToString();
                        BTripSheetOptions = dtExists.Rows[0]["BTripSheetOptions"].ToString();
                        BDepositRefundOptions = dtExists.Rows[0]["BDepositRefundOptions"].ToString();
                        BOfflineRights = dtExists.Rows[0]["OfflineRights"].ToString();

                        ddlUserName.SelectedValue = UserId.Trim();
                        ddlUserName.Enabled = true;


                        //========================================================================
                        //Info Display
                        chkBoatInfoDisp.Checked = false;

                        if (MBInfoDisp.Trim() == "Y")
                        {
                            chkBoatInfoDisp.Checked = true;
                        }

                        // Module Rights

                        ChkBoatModule.Checked = false;

                        if (MBS.Trim() == "Y")
                        {
                            ChkBoatModule.Checked = true;
                        }

                        // Android Offline Rights

                        ChkAnroidOffline.Checked = false;

                        if (BOfflineRights.Trim() == "Y")
                        {
                            ChkAnroidOffline.Checked = true;
                        }

                        // Menu Rights

                        chkMenuBooking.Checked = false;
                        chkMenuTransaction.Checked = false;
                        chkMenuReports.Checked = false;

                        if (BBooking.Trim() == "Y" || BBooking.Trim().ToUpper() == "YES")
                        {
                            chkMenuBooking.Checked = true;
                        }

                        if (BTransaction.Trim() == "Y" || BTransaction.Trim().ToUpper() == "YES")
                        {
                            chkMenuTransaction.Checked = true;
                        }

                        if (BReports.Trim() == "Y" || BReports.Trim().ToUpper() == "YES")
                        {
                            chkMenuReports.Checked = true;
                        }



                        // Sub Menu Rights For Boat Service Menu

                        if (BMBooking.Trim() == "Y" && BMBulkBooking.Trim() == "Y")
                        {
                            BindBoatingServiceList();

                            if (BBoatingService.Trim() == "")
                            {
                                chkMenuBoatingSvc.Checked = false;
                                foreach (ListItem item in ckblBoatingSvc.Items)
                                {
                                    item.Selected = false;
                                }

                            }
                            else
                            {
                                chkMenuBoatingSvc.Checked = true;
                                string bBoatSvc = BBoatingService.Trim().Trim();
                                string[] sbBoatSvc = bBoatSvc.Split(',');
                                int bBoatSvcCount = sbBoatSvc.Count();
                                for (int i = 0; i < bBoatSvcCount; i++)
                                {
                                    foreach (ListItem item in ckblBoatingSvc.Items)
                                    {
                                        string selectedValue = item.Value;
                                        if (selectedValue == sbBoatSvc[i].ToString())
                                        {
                                            item.Selected = true;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            BindBoatingServiceList();
                            chkMenuBoatingSvc.Checked = false;
                        }

                        // Sub Menu Rights For Additional Service Menu

                        if (BMAdditionalService.Trim() == "Y")
                        {
                            BindAddlServiceList();

                            if (BAdditionalService.Trim() == "")
                            {
                                chkMenuAdditionalSvc.Checked = false;
                                foreach (ListItem item in ckblAddlSvc.Items)
                                {
                                    item.Selected = false;
                                }

                            }
                            else
                            {
                                chkMenuAdditionalSvc.Checked = true;
                                string bAddlSvc = BAdditionalService.Trim();
                                string[] sbAddlSvc = bAddlSvc.Split(',');
                                int bAddlSvcCount = sbAddlSvc.Count();
                                for (int i = 0; i < bAddlSvcCount; i++)
                                {
                                    foreach (ListItem item in ckblAddlSvc.Items)
                                    {
                                        string selectedValue = item.Value;
                                        if (selectedValue == sbAddlSvc[i].ToString())
                                        {
                                            item.Selected = true;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            BindAddlServiceList();
                            chkMenuAdditionalSvc.Checked = false;
                        }


                        // Sub Menu Rights For OtherService Menu

                        if (BMOtherService.Trim() == "Y")
                        {
                            BindOtherCategoryList();

                            if (BOtherService.Trim() == "")
                            {
                                chkMenuOtherSvc.Checked = false;
                                foreach (ListItem item in ckblOtherSvc.Items)
                                {
                                    item.Selected = true;
                                }

                            }
                            else
                            {
                                chkMenuOtherSvc.Checked = true;
                                string bOtherSvc = BOtherService.Trim();
                                string[] sbOtherSvc = bOtherSvc.Split(',');
                                int bOtherSvcCount = sbOtherSvc.Count();
                                for (int i = 0; i < bOtherSvcCount; i++)
                                {
                                    foreach (ListItem item in ckblOtherSvc.Items)
                                    {
                                        string selectedValue = item.Value;
                                        if (selectedValue == sbOtherSvc[i].ToString())
                                        {
                                            item.Selected = true;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            BindOtherCategoryList();
                            chkMenuOtherSvc.Checked = false;
                        }

                        // Sub Menu Rights For Trip Sheet Menu


                        if (BMTripSheet.Trim() == "Y")
                        {
                            if (BTripSheetOptions.Trim() == "")
                            {
                                chkMenuTripSheet.Checked = false;
                                foreach (ListItem itemTS in ckblTripSheet.Items)
                                {
                                    itemTS.Selected = false;
                                }

                            }
                            else
                            {
                                chkMenuTripSheet.Checked = true;
                                string bTripSheet = BTripSheetOptions.Trim();
                                string[] sbTripSheet = bTripSheet.Split(',');
                                int bTripSheetCount = sbTripSheet.Count();
                                for (int i = 0; i < bTripSheetCount; i++)
                                {
                                    if (sbTripSheet[i].ToString() == "Y")
                                    {
                                        ckblTripSheet.Items[i].Selected = true;
                                    }
                                    else
                                    {
                                        ckblTripSheet.Items[i].Selected = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            chkMenuTripSheet.Checked = false;
                        }


                        // Sub Menu Rights For Booking Menu

                        for (int i = 0; i < ckblBooking.Items.Count; i++)
                        {
                            if (BMBooking.Trim() == "Y")
                            {
                                ckblBooking.Items[0].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[0].Selected = false;
                            }
                            //if (BMBookingOthers.Trim() == "Y")
                            //{
                            //    ckblBooking.Items[1].Selected = true;
                            //}
                            //else
                            //{
                            //    ckblBooking.Items[1].Selected = false;
                            //}
                            if (BMBulkBooking.Trim() == "Y")
                            {
                                ckblBooking.Items[1].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[1].Selected = false;
                            }
                            if (BMAdditionalService.Trim() == "Y")
                            {
                                ckblBooking.Items[2].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[2].Selected = false;
                            }
                            if (BMOtherService.Trim() == "Y")
                            {
                                ckblBooking.Items[3].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[3].Selected = false;
                            }
                            if (BMBulkOtherService.Trim() == "Y")
                            {
                                ckblBooking.Items[4].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[4].Selected = false;
                            }
                            if (BRestaurant.Trim() == "Y" || BRestaurant.Trim().ToUpper() == "YES")
                            {
                                ckblBooking.Items[5].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[5].Selected = false;
                            }

                            if (BMTripSheet.Trim() == "Y")
                            {
                                ckblBooking.Items[6].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[6].Selected = false;
                            }
                            if (BMCancellation.Trim() == "Y")
                            {
                                ckblBooking.Items[7].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[7].Selected = false;
                            }
                            if (BMReSchedule.Trim() == "Y")
                            {
                                ckblBooking.Items[8].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[8].Selected = false;
                            }



                            if (BGeneratingBoardingPass.Trim() == "Y")
                            {
                                ckblBooking.Items[9].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[9].Selected = false;
                            }
                            if (BGenerateManualTicket.Trim() == "Y")
                            {
                                ckblBooking.Items[10].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[10].Selected = false;
                            }
                            if (BMChangeBoatDetails.Trim() == "Y")
                            {
                                ckblBooking.Items[11].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[11].Selected = false;
                            }
                            if (BMChangeTripSheet.Trim() == "Y")
                            {
                                ckblBooking.Items[12].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[12].Selected = false;
                            }
                            if (BMBoatReTripDetails.Trim() == "Y")
                            {
                                ckblBooking.Items[13].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[13].Selected = false;
                            }
                            if (BMKioskBooking.Trim() == "Y")
                            {
                                ckblBooking.Items[14].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[14].Selected = false;
                            }


                            //Newly added

                            if (BMKioskOtherService.Trim() == "Y")
                            {
                                ckblBooking.Items[15].Selected = true;
                            }
                            else
                            {
                                ckblBooking.Items[15].Selected = false;
                            }

                            //Newly added

                        }

                        // Sub Menu Rights For Transaction Menu

                        for (int i = 0; i < ckblTransaction.Items.Count; i++)
                        {

                            if (TMTripSheetSettle.Trim() == "Y")
                            {
                                ckblTransaction.Items[0].Selected = true;
                            }
                            else
                            {
                                ckblTransaction.Items[0].Selected = false;
                            }
                            if (TMRowerSettle.Trim() == "Y")
                            {
                                ckblTransaction.Items[1].Selected = true;
                            }
                            else
                            {
                                ckblTransaction.Items[1].Selected = false;
                            }
                            if (TMRefundCounter.Trim() == "Y")
                            {
                                ckblTransaction.Items[2].Selected = true;
                            }
                            else
                            {
                                ckblTransaction.Items[2].Selected = false;
                            }

                            if (TMReceiptBalanceRefund.Trim() == "Y")
                            {
                                ckblTransaction.Items[3].Selected = true;
                            }

                            else
                            {
                                ckblTransaction.Items[3].Selected = false;
                            }


                            if (TMStockEntryMaintance.Trim() == "Y")
                            {
                                ckblTransaction.Items[4].Selected = true;
                            }
                            else
                            {
                                ckblTransaction.Items[4].Selected = false;
                            }
                            if (TMMaterialPur.Trim() == "Y")
                            {
                                ckblTransaction.Items[5].Selected = true;
                            }
                            else
                            {
                                ckblTransaction.Items[5].Selected = false;
                            }
                            if (TMMaterialIss.Trim() == "Y")
                            {
                                ckblTransaction.Items[6].Selected = true;
                            }
                            else
                            {
                                ckblTransaction.Items[6].Selected = false;
                            }



                        }

                        // Sub Menu Rights For Deposit Refund Menu


                        if (TMTripSheetSettle.Trim() == "Y")
                        {
                            if (BDepositRefundOptions.Trim() == "")
                            {
                                chkMenuDepositRefund.Checked = false;
                                foreach (ListItem itemTS in ckblDepositRefund.Items)
                                {
                                    itemTS.Selected = false;
                                }

                            }
                            else
                            {
                                chkMenuDepositRefund.Checked = true;
                                string bDepositRefund = BDepositRefundOptions.Trim();
                                string[] sbDepositRefund = bDepositRefund.Split(',');
                                int bDepositRefundCount = sbDepositRefund.Count();
                                for (int i = 0; i < bDepositRefundCount; i++)
                                {
                                    if (sbDepositRefund[i].ToString() == "Y")
                                    {
                                        ckblDepositRefund.Items[i].Selected = true;
                                    }
                                    else
                                    {
                                        ckblDepositRefund.Items[i].Selected = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            chkMenuDepositRefund.Checked = false;
                        }

                        // Sub Menu Rights For Reports Menu



                        for (int i = 0; i < ckblReports.Items.Count; i++)
                        {
                            if (RMBooking.Trim() == "Y")
                            {
                                ckblReports.Items[0].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[0].Selected = false;
                            }
                            if (RMAdditionalTicket.Trim() == "Y")
                            {
                                ckblReports.Items[1].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[1].Selected = false;
                            }
                            if (RMOtherSvc.Trim() == "Y")
                            {
                                ckblReports.Items[2].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[2].Selected = false;
                            }
                            if (RMRestaurantService.Trim() == "Y")
                            {
                                ckblReports.Items[3].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[3].Selected = false;
                            }


                            if (RMAbstractBoatBook.Trim() == "Y")
                            {
                                ckblReports.Items[4].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[4].Selected = false;
                            }

                            if (RMAbstractAdditionalTicket.Trim() == "Y")
                            {
                                ckblReports.Items[5].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[5].Selected = false;
                            }

                            if (RMAbstractOthSvc.Trim() == "Y")
                            {
                                ckblReports.Items[6].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[6].Selected = false;
                            }
                            if (RMAbstractResSvc.Trim() == "Y")
                            {
                                ckblReports.Items[7].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[7].Selected = false;
                            }
                            if (RMBoatwiseTrip.Trim() == "Y")
                            {
                                ckblReports.Items[8].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[8].Selected = false;
                            }
                            if (RMTripSheetSettle.Trim() == "Y")
                            {
                                ckblReports.Items[9].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[9].Selected = false;
                            }
                            if (RMTripWiseCollection.Trim() == "Y")
                            {
                                ckblReports.Items[10].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[10].Selected = false;
                            }

                            if (RMDepositStatus.Trim() == "Y")
                            {
                                ckblReports.Items[11].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[11].Selected = false;
                            }
                            if (RMDiscountReport.Trim() == "Y")
                            {
                                ckblReports.Items[12].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[12].Selected = false;
                            }


                            if (RMCashinHands.Trim() == "Y")
                            {
                                ckblReports.Items[13].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[13].Selected = false;
                            }



                            if (RMRowerCharges.Trim() == "Y")
                            {
                                ckblReports.Items[14].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[14].Selected = false;
                            }

                            if (RMRowerSettle.Trim() == "Y")
                            {
                                ckblReports.Items[15].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[15].Selected = false;
                            }
                            if (RMBoatTypeRowerList.Trim() == "Y")
                            {
                                ckblReports.Items[16].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[16].Selected = false;
                            }
                            if (RMExtendedBoatHouse.Trim() == "Y")
                            {
                                ckblReports.Items[17].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[17].Selected = false;
                            }
                            if (RMPrintBoatBooking.Trim() == "Y")
                            {
                                ckblReports.Items[18].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[18].Selected = false;
                            }
                            if (RMTripWiseDetails.Trim() == "Y")
                            {
                                ckblReports.Items[19].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[19].Selected = false;
                            }
                            if (RMAvailBoatCapacity.Trim() == "Y")
                            {
                                ckblReports.Items[20].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[20].Selected = false;
                            }
                            if (RMBoatCancellation.Trim() == "Y")
                            {
                                ckblReports.Items[21].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[21].Selected = false;
                            }
                            if (RMChallanRegister.Trim() == "Y")
                            {
                                ckblReports.Items[22].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[22].Selected = false;
                            }
                            if (RMServiceWiseCollection.Trim() == "Y")
                            {
                                ckblReports.Items[23].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[23].Selected = false;
                            }
                            if (RMUserBookingReport.Trim() == "Y")
                            {
                                ckblReports.Items[24].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[24].Selected = false;
                            }


                            //if (RMAbstractChallanRegister.Trim() == "Y")                          
                            //{
                            //    ckblReports.Items[17].Selected = true;
                            //}
                            //else
                            //{
                            //    ckblReports.Items[17].Selected = false;
                            //}
                            //Newly Added 




                            if (RMReceiptBalance.Trim() == "Y")
                            {
                                ckblReports.Items[25].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[25].Selected = false;
                            }
                            if (RMRePrintReport.Trim() == "Y")
                            {
                                ckblReports.Items[26].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[26].Selected = false;
                            }
                            if (RMQRCodeGeneration.Trim() == "Y")
                            {
                                ckblReports.Items[27].Selected = true;
                            }
                            else
                            {
                                ckblReports.Items[27].Selected = false;
                            }
                        }

                        //==============================================
                    }


                    else
                    {
                        ChkBoatModule.Checked = false;
                        ChkAnroidOffline.Checked = false;
                        chkMenuBooking.Checked = false;
                        chkMenuTransaction.Checked = false;
                        chkMenuReports.Checked = false;

                        chkMenuBoatingSvc.Checked = false;
                        chkMenuOtherSvc.Checked = false;
                        chkMenuTripSheet.Checked = false;

                        ckblBooking.ClearSelection();
                        ckblTransaction.ClearSelection();
                        ckblReports.ClearSelection();
                        ckblBoatingSvc.ClearSelection();
                        ckblOtherSvc.ClearSelection();
                        ckblTripSheet.ClearSelection();
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

    public void BindAdminAccess()
    {
        try
        {
            string Userid = string.Empty;
            string BranchId = string.Empty;
            string Svc = string.Empty;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (ddlUserName.SelectedItem.Text.Trim() != "Select User Name")
                {
                    if (Convert.ToInt32(ddlUserName.SelectedValue) > 0)
                    {
                        Userid = ddlUserName.SelectedValue.Trim();
                        Svc = "UserRoleV2";
                    }
                    else
                    {
                        Userid = "0";
                        Svc = "UserRoleV2";
                    }
                }
                else
                {
                    Userid = "0";
                    Svc = "UserRoleV2";
                }

                var UserRights = new AdminAccess()
                {
                    QueryType = "UserAccess",
                    ServiceType = Svc.Trim(),
                    UserId = Userid.Trim(),
                    UserRole = "3",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    CountStart = hfstartvalue.Value.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("GetUserAccessV2", UserRights).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvAdminAccess.DataSource = dtExists;
                        gvAdminAccess.DataBind();
                        lblGridMsg.Text = string.Empty;
                        if (dtExists.Rows.Count < 10)
                        {
                            Next.Enabled = false;
                        }
                        else
                        {
                            Next.Enabled = true;
                        }
                    }
                    else
                    {
                        gvAdminAccess.DataBind();
                        lblGridMsg.Text = "No Record Found";
                        Next.Enabled = false;
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

    public void clear()
    {
        btnSubmit.Text = "Submit";
        btnCancel.Visible = false;
        ddlUserName.SelectedIndex = 0;
        ddlUserName.Enabled = true;

        ChkBoatModule.Checked = false;
        chkBoatInfoDisp.Checked = false;

        ChkAnroidOffline.Checked = false;

        chkMenuBooking.Checked = false;
        chkMenuBoatingSvc.Checked = false;
        chkMenuAdditionalSvc.Checked = false;
        chkMenuOtherSvc.Checked = false;
        chkMenuTripSheet.Checked = false;
        chkMenuTransaction.Checked = false;
        chkMenuDepositRefund.Checked = false;
        chkMenuReports.Checked = false;

        ckblBooking.ClearSelection();
        ckblBoatingSvc.ClearSelection();
        ckblAddlSvc.ClearSelection();
        ckblOtherSvc.ClearSelection();
        ckblTripSheet.ClearSelection();
        ckblTransaction.ClearSelection();
        ckblDepositRefund.ClearSelection();
        ckblReports.ClearSelection();
    }

    protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUserName.SelectedItem.Text.Trim() != "Select UserName")
        {
            BindEmpAdminAccess();
        }
        else
        {

            ChkBoatModule.Checked = false;

            ChkAnroidOffline.Checked = false;

            chkMenuBooking.Checked = false;
            chkMenuTransaction.Checked = false;
            chkMenuReports.Checked = false;


            ckblBooking.ClearSelection();
            ckblTransaction.ClearSelection();
            ckblReports.ClearSelection();



        }

        btnSubmit.Text = "Submit";
        btnCancel.Visible = false;

        BindAdminAccess();
    }

    protected void ckblBooking_SelectedIndexChanged(object sender, System.EventArgs e)
    {

        string BtMenu = string.Empty;
        string selectMenu = String.Join(",",
             ckblBooking.Items.OfType<ListItem>().Where(r => r.Selected)
            .Select(r => r.Text.Trim()));
        if (selectMenu == "")
        {
            chkMenuAdditionalSvc.Checked = false;
            foreach (ListItem item in ckblAddlSvc.Items)
            {
                item.Selected = false;
            }
            chkMenuOtherSvc.Checked = false;
            foreach (ListItem item in ckblOtherSvc.Items)
            {
                item.Selected = false;
            }

            chkMenuTripSheet.Checked = false;
            foreach (ListItem item in ckblTripSheet.Items)
            {
                item.Selected = false;
            }

            chkMenuBoatingSvc.Checked = false;
            foreach (ListItem item in ckblBoatingSvc.Items)
            {
                item.Selected = false;
            }
        }
        else
        {
            BtMenu = selectMenu;
            string[] sBTReslt = BtMenu.Split(',');
            int sBTCount = sBTReslt.Count();

            foreach (ListItem lb in ckblBooking.Items)
            {
                if (lb.Value == "bbmb1")
                {
                    if (lb.Selected)
                    {
                        chkMenuBoatingSvc.Checked = true;
                        foreach (ListItem lo in ckblBoatingSvc.Items)
                        {
                            lo.Selected = true;
                        }
                    }
                    else
                    {
                        chkMenuBoatingSvc.Checked = false;
                        foreach (ListItem lo in ckblBoatingSvc.Items)
                        {
                            lo.Selected = false;
                        }
                    }
                }

                if (lb.Value == "bbmb4")
                {
                    if (lb.Selected)
                    {
                        chkMenuAdditionalSvc.Checked = true;
                        foreach (ListItem lo in ckblAddlSvc.Items)
                        {
                            lo.Selected = true;
                        }
                    }
                    else
                    {
                        chkMenuAdditionalSvc.Checked = false;
                        foreach (ListItem lo in ckblAddlSvc.Items)
                        {
                            lo.Selected = false;
                        }
                    }
                }

                if (lb.Value == "bbmb5")
                {
                    if (lb.Selected)
                    {
                        chkMenuOtherSvc.Checked = true;
                        foreach (ListItem lo in ckblOtherSvc.Items)
                        {
                            lo.Selected = true;
                        }
                    }
                    else
                    {
                        chkMenuOtherSvc.Checked = false;
                        foreach (ListItem lo in ckblOtherSvc.Items)
                        {
                            lo.Selected = false;
                        }
                    }
                }
                else if (lb.Value == "bbmb8")
                {
                    if (lb.Selected)
                    {
                        chkMenuTripSheet.Checked = true;
                        foreach (ListItem lt in ckblTripSheet.Items)
                        {
                            lt.Selected = true;
                        }
                    }
                    else
                    {
                        chkMenuTripSheet.Checked = false;
                        foreach (ListItem lt in ckblTripSheet.Items)
                        {
                            lt.Selected = false;
                        }
                    }
                }

            }

        }
    }

    protected void chkMenuBooking_CheckedChanged(object sender, System.EventArgs e)
    {
        if (chkMenuBooking.Checked == false)
        {
            foreach (ListItem item in ckblOtherSvc.Items)
            {
                item.Selected = false;
            }
            foreach (ListItem item1 in ckblTripSheet.Items)
            {
                item1.Selected = false;
            }
            foreach (ListItem item2 in ckblBoatingSvc.Items)
            {
                item2.Selected = false;
            }
        }
        else
        {
            foreach (ListItem item in ckblOtherSvc.Items)
            {
                item.Selected = true;
            }
            foreach (ListItem item1 in ckblTripSheet.Items)
            {
                item1.Selected = true;
            }
            foreach (ListItem item2 in ckblBoatingSvc.Items)
            {
                item2.Selected = true;
            }
        }
    }

    protected void chkMenuBoatingSvc_CheckedChanged(object sender, System.EventArgs e)
    {
        if (chkMenuBoatingSvc.Checked == false)
        {
            foreach (ListItem lt in ckblBooking.Items)
            {
                if (lt.Value == "bbmb1" || lt.Value == "bbmb3")
                    if (lt.Selected)
                    {

                        foreach (ListItem lm in ckblBooking.Items)
                        {
                            if (lm.Value == "bbmb1" || lm.Value == "bbmb3")
                                lm.Selected = false;
                            chkMenuBooking.Checked = false;
                        }
                    }
            }
            if (chkMenuTripSheet.Checked == true || chkMenuOtherSvc.Checked == true || chkMenuBoatingSvc.Checked == true || chkMenuAdditionalSvc.Checked == true)
            {
                chkMenuBooking.Checked = true;
            }


            if (chkMenuOtherSvc.Checked == false && chkMenuTripSheet.Checked == false && chkMenuBoatingSvc.Checked == false || chkMenuAdditionalSvc.Checked == false)
            {
                foreach (ListItem lt in ckblBooking.Items)
                {
                    if (lt.Selected == true)
                    {
                        chkMenuBooking.Checked = true;
                        return;
                    }
                    else if (lt.Selected != true)
                    {
                        chkMenuBooking.Checked = false;
                    }
                }
            }
        }
        else
        {
            foreach (ListItem lt in ckblBooking.Items)
            {
                if (lt.Value == "bbmb1" || lt.Value == "bbmb3")
                    if (!lt.Selected)
                    {

                        foreach (ListItem lm in ckblBooking.Items)
                        {
                            if (lm.Value == "bbmb1" || lm.Value == "bbmb3")
                                lm.Selected = true;
                            chkMenuBooking.Checked = true;
                        }
                    }
            }

        }
    }

    protected void ckblBoatingSvc_Changed(object sender, EventArgs e)
    {
    }

    protected void chkMenuAdditionalSvc_CheckedChanged(object sender, System.EventArgs e)
    {
        if (chkMenuAdditionalSvc.Checked == false)
        {
            foreach (ListItem lt in ckblBooking.Items)
            {
                if (lt.Value == "bbmb4")
                    if (lt.Selected)
                    {

                        foreach (ListItem lm in ckblBooking.Items)
                        {
                            if (lm.Value == "bbmb4")
                                lm.Selected = false;
                            chkMenuBooking.Checked = false;
                        }
                    }
            }
            if (chkMenuTripSheet.Checked == true || chkMenuOtherSvc.Checked == true || chkMenuBoatingSvc.Checked == true || chkMenuAdditionalSvc.Checked == true)
            {
                chkMenuBooking.Checked = true;
            }


            if (chkMenuOtherSvc.Checked == false && chkMenuTripSheet.Checked == false && chkMenuBoatingSvc.Checked == false || chkMenuAdditionalSvc.Checked == false)
            {
                foreach (ListItem lt in ckblBooking.Items)
                {
                    if (lt.Selected == true)
                    {
                        chkMenuBooking.Checked = true;
                        return;
                    }
                    else if (lt.Selected != true)
                    {
                        chkMenuBooking.Checked = false;
                    }
                }
            }
        }
        else
        {
            foreach (ListItem lt in ckblBooking.Items)
            {
                if (lt.Value == "bbmb4")
                    if (!lt.Selected)
                    {

                        foreach (ListItem lm in ckblBooking.Items)
                        {
                            if (lm.Value == "bbmb4")
                                lm.Selected = true;
                            chkMenuBooking.Checked = true;
                        }
                    }
            }

        }
    }

    protected void ckblAddlSvc_Changed(object sender, EventArgs e)
    {
    }

    protected void chkMenuOtherSvc_CheckedChanged(object sender, System.EventArgs e)
    {
        if (chkMenuOtherSvc.Checked == false)
        {
            foreach (ListItem lt in ckblBooking.Items)
            {
                if (lt.Value == "bbmb5")
                    if (lt.Selected)
                    {

                        foreach (ListItem lm in ckblBooking.Items)
                        {
                            if (lm.Value == "bbmb5")
                                lm.Selected = false;
                            chkMenuBooking.Checked = false;
                        }
                    }
            }
            if (chkMenuTripSheet.Checked == true || chkMenuOtherSvc.Checked == true || chkMenuBoatingSvc.Checked == true || chkMenuAdditionalSvc.Checked == true)
            {
                chkMenuBooking.Checked = true;
            }


            if (chkMenuOtherSvc.Checked == false && chkMenuTripSheet.Checked == false && chkMenuBoatingSvc.Checked == false || chkMenuAdditionalSvc.Checked == false)
            {
                foreach (ListItem lt in ckblBooking.Items)
                {
                    if (lt.Selected == true)
                    {
                        chkMenuBooking.Checked = true;
                        return;
                    }
                    else if (lt.Selected != true)
                    {
                        chkMenuBooking.Checked = false;
                    }
                }
            }
        }
        else
        {
            foreach (ListItem lt in ckblBooking.Items)
            {
                if (lt.Value == "bbmb5")
                    if (!lt.Selected)
                    {

                        foreach (ListItem lm in ckblBooking.Items)
                        {
                            if (lm.Value == "bbmb5")
                                lm.Selected = true;
                            chkMenuBooking.Checked = true;
                        }
                    }
            }

        }
    }

    protected void ckblOtherSvc_Changed(object sender, EventArgs e)
    {
    }

    protected void chkMenuTripSheet_CheckedChanged(object sender, System.EventArgs e)
    {
        if (chkMenuTripSheet.Checked == false)
        {
            foreach (ListItem lt in ckblBooking.Items)
            {
                if (lt.Value == "bbmb8")
                    if (lt.Selected)
                    {

                        foreach (ListItem lm in ckblBooking.Items)
                        {
                            if (lm.Value == "bbmb8")
                                lm.Selected = false;
                            chkMenuBooking.Checked = false;
                        }
                    }
            }
            if (chkMenuOtherSvc.Checked == true || chkMenuTripSheet.Checked == true || chkMenuBoatingSvc.Checked == true || chkMenuAdditionalSvc.Checked == true)
            {
                chkMenuBooking.Checked = true;
            }


            if (chkMenuOtherSvc.Checked == false && chkMenuTripSheet.Checked == false && chkMenuBoatingSvc.Checked == false || chkMenuAdditionalSvc.Checked == false)
            {
                foreach (ListItem lt in ckblBooking.Items)
                {
                    if (lt.Selected == true)
                    {
                        chkMenuBooking.Checked = true;
                        return;
                    }
                    else if (lt.Selected != true)
                    {
                        chkMenuBooking.Checked = false;
                    }
                }
            }
        }
        else
        {
            foreach (ListItem lt in ckblBooking.Items)
            {
                if (lt.Value == "bbmb8")
                    if (!lt.Selected)
                    {

                        foreach (ListItem lm in ckblBooking.Items)
                        {
                            if (lm.Value == "bbmb8")
                                lm.Selected = true;
                            chkMenuBooking.Checked = true;
                        }
                    }
            }
        }
    }

    protected void ckblTripSheet_Changed(object sender, EventArgs e)
    {
    }

    protected void ckblTransaction_SelectedIndexChanged(object sender, System.EventArgs e)
    {

        string TrMenu = string.Empty;
        string TrselectMenu = String.Join(",",
             ckblTransaction.Items.OfType<ListItem>().Where(r => r.Selected)
            .Select(r => r.Text.Trim()));
        if (TrselectMenu == "")
        {
            chkMenuDepositRefund.Checked = false;
            foreach (ListItem item in ckblDepositRefund.Items)
            {
                item.Selected = false;
            }
        }
        else
        {
            TrMenu = TrselectMenu;
            string[] sTrReslt = TrMenu.Split(',');
            int sTrCount = sTrReslt.Count();

            foreach (ListItem lb in ckblTransaction.Items)
            {
                if (lb.Value == "bbmt3")
                {
                    if (lb.Selected)
                    {
                        chkMenuDepositRefund.Checked = true;
                        foreach (ListItem lo in ckblDepositRefund.Items)
                        {
                            lo.Selected = true;
                        }
                    }
                    else
                    {
                        chkMenuDepositRefund.Checked = false;
                        foreach (ListItem lo in ckblDepositRefund.Items)
                        {
                            lo.Selected = false;
                        }
                    }
                }
            }

        }
    }

    protected void chkMenuDepositRefund_CheckedChanged(object sender, System.EventArgs e)
    {
        if (chkMenuDepositRefund.Checked == false)
        {
            foreach (ListItem lt in ckblTransaction.Items)
            {
                if (lt.Value == "bbmt3")
                    if (lt.Selected)
                    {

                        foreach (ListItem lm in ckblTransaction.Items)
                        {
                            if (lm.Value == "bbmt3")
                                lm.Selected = false;
                            chkMenuTransaction.Checked = false;
                        }
                    }
            }
            if (chkMenuDepositRefund.Checked == true)
            {
                chkMenuTransaction.Checked = true;
            }


            if (chkMenuDepositRefund.Checked == false)
            {
                foreach (ListItem lt in ckblTransaction.Items)
                {
                    if (lt.Selected == true)
                    {
                        chkMenuTransaction.Checked = true;
                        return;
                    }
                    else if (lt.Selected != true)
                    {
                        chkMenuTransaction.Checked = false;
                    }
                }
            }
        }
        else
        {
            foreach (ListItem lt in ckblTransaction.Items)
            {
                if (lt.Value == "bbmt3")
                    if (!lt.Selected)
                    {

                        foreach (ListItem lm in ckblTransaction.Items)
                        {
                            if (lm.Value == "bbmt3")
                                lm.Selected = true;
                            chkMenuTransaction.Checked = true;
                        }
                    }
            }
        }
    }

    protected void ckblDepositRefund_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            string ModuleList = string.Empty;

            string mStatus = string.Empty;
            string bStatus = string.Empty;
            string tStatus = string.Empty;
            string hStatus = string.Empty;
            string aStatus = string.Empty;

            //Info Display
            string bInfoDispStatus = string.Empty;

            bInfoDispStatus = "N";

            if (chkBoatInfoDisp.Checked == true)
            {
                bInfoDispStatus = "Y";
            }
            else
            {
                bInfoDispStatus = "N";
            }

            //  Module Menu Rights

            mStatus = "N";
            bStatus = "N";
            tStatus = "N";
            hStatus = "N";
            aStatus = "N";

            if (ChkBoatModule.Checked == true)
            {
                bStatus = "Y";
            }

            //  Android Offline Rights

            string AndroidOffline = string.Empty;

            AndroidOffline = "N";

            if (ChkAnroidOffline.Checked == true)
            {
                AndroidOffline = "Y";
            }

            string mCMStatus = string.Empty;
            string mBHMStatus = string.Empty;
            string mHMStatus = string.Empty;
            string mTMtatus = string.Empty;
            string mARStatus = string.Empty;
            string mOMStatus = string.Empty;

            mCMStatus = "N";
            mBHMStatus = "N";
            mHMStatus = "N";
            mTMtatus = "N";
            mARStatus = "N";
            mOMStatus = "N";

            // Menu Rights

            string bMStatus = string.Empty;
            string bTStatus = string.Empty;
            string bBStatus = string.Empty;
            string bRStatus = string.Empty;

            bMStatus = "N";
            bBStatus = "N";
            bTStatus = "N";
            bRStatus = "N";

            if (chkMenuBooking.Checked == true)
            {
                bBStatus = "Y";
            }

            if (chkMenuTransaction.Checked == true)
            {
                bTStatus = "Y";
            }

            if (chkMenuReports.Checked == true)
            {
                bRStatus = "Y";
            }

            // Sub Menu Rights For Booking Menu

            string BSMenuList = string.Empty;

            string bBMBStatus = string.Empty;
            string bBMBBOStatus = string.Empty;
            string bBMBBBStatus = string.Empty;
            string bBMAStatus = string.Empty;
            string bBMOStatus = string.Empty;
            string bBMBOStatus = string.Empty;
            string bBMKBStatus = string.Empty;
            string bBMTSStatus = string.Empty;
            string bBMCTSStatus = string.Empty;
            string bBMBRTtatus = string.Empty;
            string bBMCBDStatus = string.Empty;
            string bBMCStatus = string.Empty;
            string bBMRStatus = string.Empty;
            string bBMResStatus = string.Empty;

            string bBMGeneratingPassStatus = string.Empty;
            string BbMGenManualTicketStatus = string.Empty;
            //NEWLY ADDED
            string bBMKBOStatus = string.Empty;
            //NEWLY ADDED
            string selectedBtBsmenuItems = String.Join(",",
             ckblBooking.Items.OfType<ListItem>().Where(r => r.Selected)
            .Select(r => r.Text.Trim()));
            if (selectedBtBsmenuItems == "")
            {
                bBMBStatus = "N";
                bBMBBOStatus = "N";
                bBMBBBStatus = "N";
                bBMAStatus = "N";
                bBMOStatus = "N";
                bBMBOStatus = "N";
                bBMKBStatus = "N";
                bBMTSStatus = "N";
                bBMCTSStatus = "N";
                bBMBRTtatus = "N";
                bBMCBDStatus = "N";
                bBMCStatus = "N";
                bBMRStatus = "N";
                bBMResStatus = "N";
                bBMGeneratingPassStatus = "N";
                BbMGenManualTicketStatus = "N";
                bBMKBOStatus = "N";
            }
            else
            {
                BSMenuList = selectedBtBsmenuItems;
                string[] sBBReslt = BSMenuList.Split(',');
                int sBBCount = sBBReslt.Count();

                bBMBStatus = "N";
                bBMBBOStatus = "N";
                bBMBBBStatus = "N";
                bBMAStatus = "N";
                bBMOStatus = "N";
                bBMBOStatus = "N";
                bBMKBStatus = "N";
                bBMTSStatus = "N";
                bBMCTSStatus = "N";
                bBMBRTtatus = "N";
                bBMCBDStatus = "N";
                bBMCStatus = "N";
                bBMRStatus = "N";
                bBMResStatus = "N";
                bBMGeneratingPassStatus = "N";
                BbMGenManualTicketStatus = "N";
                bBMKBOStatus = "N";


                for (int i = 0; i < sBBCount; i++)
                {
                    string abcBB = sBBReslt[i];

                    if (abcBB == "Boat Booking")
                    {
                        bBMBStatus = "Y";
                    }
                    //else if (abcBB == "Boat Booking - Other Facilities")        ---SILLU----
                    //{
                    //    bBMBBOStatus = "Y";
                    //}
                    else if (abcBB == "Bulk Boat Booking")
                    {
                        bBMBBBStatus = "Y";
                    }
                    else if (abcBB == "Additional Ticket")
                    {
                        bBMAStatus = "Y";
                    }
                    else if (abcBB == "Other Services")
                    {
                        bBMOStatus = "Y";
                        bBMBBOStatus = "Y";
                    }
                    else if (abcBB == "Bulk Other Services")
                    {
                        bBMBOStatus = "Y";
                    }
                    else if (abcBB == "Kiosk Boat Booking")
                    {
                        bBMKBStatus = "Y";
                    }
                    else if (abcBB == "Trip Sheet")
                    {
                        bBMTSStatus = "Y";
                    }
                    else if (abcBB == "Change Trip Sheet")
                    {
                        bBMCTSStatus = "Y";
                    }
                    else if (abcBB == "Re-Trip Entry")
                    {
                        bBMBRTtatus = "Y";
                    }
                    else if (abcBB == "Change Boat")
                    {
                        bBMCBDStatus = "Y";
                    }
                    else if (abcBB == "Cancellation")
                    {
                        bBMCStatus = "Y";
                    }
                    else if (abcBB == "Re-Scheduling")
                    {
                        bBMRStatus = "Y";
                    }
                    else if (abcBB == "Restaurant Services")
                    {
                        bBMResStatus = "Y";
                    }
                    else if (abcBB == "Generate Boarding Pass")
                    {
                        bBMGeneratingPassStatus = "Y";
                    }

                    else if (abcBB == "Generate Manual Ticket")
                    {
                        BbMGenManualTicketStatus = "Y";
                    }
                    else if (abcBB == "Kiosk Other Services")
                    {
                        bBMKBOStatus = "Y";
                    }
                }
            }

            // Sub Menu Rights For Boating Service Menu

            string BoatSvcIdList = string.Empty;
            string BSselectedValues = String.Join(",",
                 ckblBoatingSvc.Items.OfType<ListItem>().Where(r => r.Selected)
                .Select(r => r.Value));
            BoatSvcIdList = BSselectedValues;

            // Sub Menu Rights For Additional Service Menu

            string AddlSvcIdList = string.Empty;
            string ASselectedValues = String.Join(",",
                 ckblAddlSvc.Items.OfType<ListItem>().Where(r => r.Selected)
                .Select(r => r.Value));
            AddlSvcIdList = ASselectedValues;

            // Sub Menu Rights For Other Service Menu

            string OthSvcIdList = string.Empty;
            string PselectedValues = String.Join(",",
                 ckblOtherSvc.Items.OfType<ListItem>().Where(r => r.Selected)
                .Select(r => r.Value));
            OthSvcIdList = PselectedValues;

            // Sub Menu Rights For Trip Sheet Menu

            string TripSheetIdList = string.Empty;

            string bBMTSSStatus = string.Empty;
            string bBMTSEStatus = string.Empty;
            string bBMScTSStatus = string.Empty;

            string TSselectedValues = String.Join(",",
             ckblTripSheet.Items.OfType<ListItem>().Where(r => r.Selected)
            .Select(r => r.Text.Trim()));
            if (TSselectedValues == "")
            {
                bBMTSSStatus = "N";
                bBMTSEStatus = "N";
                bBMScTSStatus = "N";
            }
            else
            {
                TripSheetIdList = TSselectedValues;
                string[] sTSReslt = TripSheetIdList.Split(',');
                int sTSCount = sTSReslt.Count();

                bBMTSSStatus = "N";
                bBMTSEStatus = "N";
                bBMScTSStatus = "N";

                for (int i = 0; i < sTSCount; i++)
                {
                    string abcTS = sTSReslt[i];

                    if (abcTS == "Trip Start")
                    {
                        bBMTSSStatus = "Y";
                    }
                    else if (abcTS == "Trip End")
                    {
                        bBMTSEStatus = "Y";
                    }
                    else if (abcTS == "Smart Trip Sheet")
                    {
                        bBMScTSStatus = "Y";
                    }
                }
            }
            TripSheetIdList = String.Concat(bBMTSSStatus + "," + bBMTSEStatus + "," + bBMScTSStatus);

            // Sub Menu Rights For Transaction Menu

            string TSMenuList = string.Empty;

            string bTMMPStatus = string.Empty;
            string bTMMIStatus = string.Empty;
            string bTMTSStatus = string.Empty;
            string bTMRSStatus = string.Empty;
            string bTMRCStatus = string.Empty;
            string bTFSEMStatus = string.Empty;
            string bTRBRStatus = string.Empty;

            string selectedBtTsmenuItems = String.Join(",",
             ckblTransaction.Items.OfType<ListItem>().Where(r => r.Selected)
            .Select(r => r.Text.Trim()));
            if (selectedBtTsmenuItems == "")
            {
                bTMMPStatus = "N";
                bTMMIStatus = "N";
                bTMTSStatus = "N";
                bTMRSStatus = "N";
                bTMRCStatus = "N";
                bTFSEMStatus = "N";
                bTRBRStatus = "N";
            }
            else
            {
                TSMenuList = selectedBtTsmenuItems;
                string[] sBTReslt = TSMenuList.Split(',');
                int sBTCount = sBTReslt.Count();

                bTMMPStatus = "N";
                bTMMIStatus = "N";
                bTMTSStatus = "N";
                bTMRSStatus = "N";
                bTMRCStatus = "N";
                bTFSEMStatus = "N";
                bTRBRStatus = "N";

                for (int i = 0; i < sBTCount; i++)
                {
                    string abcBT = sBTReslt[i];

                    if (abcBT == "Material Purchase")
                    {
                        bTMMPStatus = "Y";
                    }
                    else if (abcBT == "Material Issue")
                    {
                        bTMMIStatus = "Y";
                    }
                    else if (abcBT == "Deposit Refund")
                    {
                        bTMTSStatus = "Y";
                    }
                    else if (abcBT == "Rower Settlement")
                    {
                        bTMRSStatus = "Y";
                    }
                    else if (abcBT == "Refund Counter")
                    {
                        bTMRCStatus = "Y";
                    }
                    else if (abcBT == "Restaurant Stock Entry")
                    {
                        bTFSEMStatus = "Y";
                    }
                    else if (abcBT == "Receipt Balance Refund")
                    {
                        bTRBRStatus = "Y";
                    }
                }
            }

            // Sub Menu Rights For Deposit Refund Menu

            string DepositRefundList = string.Empty;

            string bTMDRSQStatus = string.Empty;
            string bTMDRPStatus = string.Empty;

            string DRselectedValues = String.Join(",",
             ckblDepositRefund.Items.OfType<ListItem>().Where(r => r.Selected)
            .Select(r => r.Text.Trim()));
            if (DRselectedValues == "")
            {
                bTMDRSQStatus = "N";
                bTMDRPStatus = "N";
            }
            else
            {
                DepositRefundList = DRselectedValues;
                string[] sDRReslt = DepositRefundList.Split(',');
                int sDRCount = sDRReslt.Count();

                bTMDRSQStatus = "N";
                bTMDRPStatus = "N";

                for (int i = 0; i < sDRCount; i++)
                {
                    string abcDR = sDRReslt[i];

                    if (abcDR == "Scan QR")
                    {
                        bTMDRSQStatus = "Y";
                    }
                    else if (abcDR == "Pin")
                    {
                        bTMDRPStatus = "Y";
                    }
                }
            }
            DepositRefundList = String.Concat(bTMDRSQStatus + "," + bTMDRPStatus);

            // Sub Menu Rights For Reports Menu

            string RSMenuList = string.Empty;

            string bRMBStatus = string.Empty;
            string bRMOStatus = string.Empty;
            string bRMRStatus = string.Empty;
            //New
            string bRMATStatus = string.Empty;
            string bRMAATStatus = string.Empty;
            string bRMDpstStatus = string.Empty;
            string bRMDisStatus = string.Empty;

            string bRMCashStatus = string.Empty;
            string bRMExtBoatStatus = string.Empty;
            string bRMPrintBoatStatus = string.Empty;

            string bRMTripWiseStatus = string.Empty;
            string bRMReceiptBalStatus = string.Empty;
            //string bRMAbstractBookingStatus = string.Empty;
            string bRMRePrintReportStatus = string.Empty;
            string bRMQRCodeGeneration = string.Empty;

            //New
            string bRMABBStatus = string.Empty;
            string bRMAOSStatus = string.Empty;
            string bRMARSStatus = string.Empty;

            string bRMABCStatus = string.Empty;
            string bRMBTStatus = string.Empty;
            string bRMTSStatus = string.Empty;

            string bRMRCStatus = string.Empty;
            string bRMBCStatus = string.Empty;
            string bRMRSStatus = string.Empty;

            string bRMCRStatus = string.Empty;
            string bRMACRStatus = string.Empty;
            string bRMSWCStatus = string.Empty;

            string bRMUBRStatus = string.Empty;
            string bRMTWCStatus = string.Empty;
            string bRMBTRLStatus = string.Empty;

            string selectedBtRsmenuItems = String.Join(",",
             ckblReports.Items.OfType<ListItem>().Where(r => r.Selected)
            .Select(r => r.Text.Trim()));
            if (selectedBtRsmenuItems == "")
            {
                bRMBStatus = "N";
                bRMOStatus = "N";
                bRMRStatus = "N";
                //New
                bRMATStatus = "N";
                bRMAATStatus = "N";
                bRMDpstStatus = "N";
                bRMDisStatus = "N";
                bRMCashStatus = "N";
                bRMExtBoatStatus = "N";
                bRMPrintBoatStatus = "N";


                bRMTripWiseStatus = "N";
                bRMReceiptBalStatus = "N";
                //bRMAbstractBookingStatus =  "N";
                bRMRePrintReportStatus = "N";
                bRMQRCodeGeneration = "N";

                //New
                bRMABBStatus = "N";
                bRMAOSStatus = "N";
                bRMARSStatus = "N";

                bRMABCStatus = "N";
                bRMBTStatus = "N";
                bRMTSStatus = "N";

                bRMRCStatus = "N";
                bRMBCStatus = "N";
                bRMRSStatus = "N";

                bRMCRStatus = "N";
                //bRMACRStatus = "N";
                bRMSWCStatus = "N";

                bRMUBRStatus = "N";
                bRMTWCStatus = "N";
                bRMBTRLStatus = "N";
            }
            else
            {
                RSMenuList = selectedBtRsmenuItems;
                string[] sBRReslt = RSMenuList.Split(',');
                int sBRCount = sBRReslt.Count();

                bRMBStatus = "N";
                bRMOStatus = "N";
                bRMRStatus = "N";
                //New
                bRMATStatus = "N";
                bRMAATStatus = "N";
                bRMDpstStatus = "N";
                bRMDisStatus = "N";
                bRMCashStatus = "N";
                bRMExtBoatStatus = "N";
                bRMPrintBoatStatus = "N";

                bRMTripWiseStatus = "N";
                bRMReceiptBalStatus = "N";
                //bRMAbstractBookingStatus = "N";
                bRMRePrintReportStatus = "N";
                bRMQRCodeGeneration = "N";
                //New
                bRMABBStatus = "N";
                bRMAOSStatus = "N";
                bRMARSStatus = "N";

                bRMABCStatus = "N";
                bRMBTStatus = "N";
                bRMTSStatus = "N";

                bRMRCStatus = "N";
                bRMBCStatus = "N";
                bRMRSStatus = "N";

                bRMCRStatus = "N";
                //bRMACRStatus = "N";
                bRMSWCStatus = "N";

                bRMUBRStatus = "N";
                bRMTWCStatus = "N";
                bRMBTRLStatus = "N";

                for (int i = 0; i < sBRCount; i++)
                {
                    string abcBR = sBRReslt[i];

                    if (abcBR == "Print Boat Booking")
                    {
                        bRMBStatus = "Y";
                    }
                    else if (abcBR == "Print Other Services")
                    {
                        bRMOStatus = "Y";
                    }
                    else if (abcBR == "Print Restaurant Services")
                    {
                        bRMRStatus = "Y";
                    }

                    else if (abcBR == "Abstract Boat Booking")
                    {
                        bRMABBStatus = "Y";
                    }
                    else if (abcBR == "Abstract Other Services")
                    {
                        bRMAOSStatus = "Y";
                    }
                    else if (abcBR == "Abstract Restaurant Services")
                    {
                        bRMARSStatus = "Y";
                    }

                    else if (abcBR == "Available Boats With Capacity")
                    {
                        bRMABCStatus = "Y";
                    }
                    else if (abcBR == "Boat Wise Trip")
                    {
                        bRMBTStatus = "Y";
                    }
                    else if (abcBR == "Trip Sheet Summary")
                    {
                        bRMTSStatus = "Y";
                    }

                    else if (abcBR == "Rower Charges")
                    {
                        bRMRCStatus = "Y";
                    }
                    else if (abcBR == "Boat Cancellation")
                    {
                        bRMBCStatus = "Y";
                    }
                    else if (abcBR == "Rower Settlement")
                    {
                        bRMRSStatus = "Y";
                    }
                    else if (abcBR == "Challan Register")
                    {
                        bRMCRStatus = "Y";
                    }
                    //else if (abcBR == "Abstract Challan Rgeister")
                    //{
                    //    bRMACRStatus = "Y";
                    //}
                    else if (abcBR == "Service Wise Report")
                    {
                        bRMSWCStatus = "Y";
                    }
                    else if (abcBR == "User Based Booking Details")
                    {
                        bRMUBRStatus = "Y";
                    }
                    else if (abcBR == "Trip Wise Collection")
                    {
                        bRMTWCStatus = "Y";
                    }
                    else if (abcBR == "Boat Type Rower List")
                    {
                        bRMBTRLStatus = "Y";
                    }
                    //Newly Added
                    else if (abcBR == "Print Additional Ticket")
                    {
                        bRMATStatus = "Y";
                    }
                    else if (abcBR == "Abstract Additional Ticket")
                    {
                        bRMAATStatus = "Y";
                    }
                    else if (abcBR == "Deposit Status")
                    {
                        bRMDpstStatus = "Y";
                    }
                    else if (abcBR == "Discount Report")
                    {
                        bRMDisStatus = "Y";
                    }
                    else if (abcBR == "Cash In Hand")
                    {
                        bRMCashStatus = "Y";
                    }
                    else if (abcBR == "Extended Boat Rides")
                    {
                        bRMExtBoatStatus = "Y";
                    }
                    else if (abcBR == "Print Credit Boat Booking")
                    {
                        bRMPrintBoatStatus = "Y";
                    }

                    else if (abcBR == "Credit Trip Wise Details")
                    {
                        bRMTripWiseStatus = "Y";
                    }

                    else if (abcBR == "Receipt Balance Report")
                    {
                        bRMReceiptBalStatus = "Y";
                    }
                    //else if (abcBR == "Abstract Booking")
                    //{
                    //    bRMAbstractBookingStatus = "Y";
                    //}

                    else if (abcBR == "Reprint Report")
                    {
                        bRMRePrintReportStatus = "Y";
                    }

                    else if (abcBR == "QR Code Generation")
                    {
                        bRMQRCodeGeneration = "Y";
                    }




                    //Newly Added


                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                string sMSG = string.Empty;
                string QueryType = string.Empty;

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                }
                else
                {
                    QueryType = "Update";
                }

                string[] sUserName = ddlUserName.SelectedItem.Text.Split('-');

                var UserAccess = new AdminAccess()
                {
                    QueryType = QueryType,
                    UserId = ddlUserName.SelectedValue.Trim(),
                    UserName = sUserName[0].ToString(),
                    UserRole = "3",
                    MMaster = mStatus.Trim(),
                    MBS = bStatus.Trim(),
                    MTMS = tStatus.Trim(),
                    MHMS = hStatus.Trim(),
                    MAccounts = aStatus.Trim(),

                    //Info Display
                    MBoatInfoDisplay = bInfoDispStatus.Trim(),

                    MComMaster = mCMStatus.Trim(),
                    MBhMaster = mBHMStatus.Trim(),
                    MHotelMaster = mHMStatus.Trim(),
                    MTourMaster = mTMtatus.Trim(),
                    MAccessRights = mARStatus.Trim(),
                    MOtherMaster = mOMStatus.Trim(),

                    BMaster = bMStatus.Trim(),
                    BTransaction = bTStatus.Trim(),
                    BBooking = bBStatus.Trim(),
                    BReports = bRStatus.Trim(),
                    BBoatingService = BoatSvcIdList,
                    BAdditionalService = AddlSvcIdList,
                    BOtherService = OthSvcIdList,

                    BMBooking = bBMBStatus.Trim(),
                    BMBookingOthers = bBMBBOStatus.Trim(),
                    BMBulkBooking = bBMBBBStatus.Trim(),
                    BMAdditionalService = bBMAStatus.Trim(),
                    BMOtherService = bBMOStatus.Trim(),
                    BMBulkOtherService = bBMBOStatus.Trim(),
                    BMKioskBooking = bBMKBStatus.Trim(),
                    BMTripSheet = bBMTSStatus.Trim(),
                    //Newly Added

                    BMKioskOtherService = bBMKBOStatus.Trim(),

                    BTripSheetOptions = TripSheetIdList,

                    BMChangeTripSheet = bBMCTSStatus.Trim(),
                    BMBoatReTripDetails = bBMBRTtatus.Trim(),
                    BMChangeBoatDetails = bBMCBDStatus.Trim(),
                    BMCancellation = bBMCStatus.Trim(),
                    BMReSchedule = bBMRStatus.Trim(),
                    BRestaurant = bBMResStatus.Trim(),
                    BGeneratingBoardingPass = bBMGeneratingPassStatus.Trim(),
                    BGenerateManualTicket = BbMGenManualTicketStatus.Trim(),



                    TMMaterialPur = bTMMPStatus.Trim(),
                    TMMaterialIss = bTMMIStatus.Trim(),
                    TMTripSheetSettle = bTMTSStatus.Trim(),
                    TMRowerSettle = bTMRSStatus.Trim(),
                    TMRefundCounter = bTMRCStatus.Trim(),
                    TMStockEntryMaintance = bTFSEMStatus.Trim(),
                    TMReceiptBalanceRefund = bTRBRStatus.Trim(),

                    BDepositRefundOptions = DepositRefundList,

                    RMBooking = bRMBStatus.Trim(),
                    RMOtherSvc = bRMOStatus.Trim(),
                    RMRestaurantService = bRMRStatus.Trim(),
                    //Newly Added
                    RMAdditionalTicket = bRMATStatus.Trim(),
                    RMAbstractAdditionalTicket = bRMAATStatus.Trim(),
                    RMDepositStatus = bRMDpstStatus.Trim(),
                    RMDiscountReport = bRMDisStatus.Trim(),

                    RMCashinHands = bRMCashStatus.Trim(),
                    RMExtendedBoatHouse = bRMExtBoatStatus.Trim(),
                    RMPrintBoatBooking = bRMPrintBoatStatus.Trim(),

                    RMTripWiseDetails = bRMTripWiseStatus.Trim(),
                    RMReceiptBalance = bRMReceiptBalStatus.Trim(),
                    //RMAbstractBooking = bRMAbstractBookingStatus.Trim(),
                    RMRePrintReport = bRMRePrintReportStatus.Trim(),
                    RMQRCodeGeneration = bRMQRCodeGeneration.Trim(),



                    //Newly Added
                    RMAbstractBoatBook = bRMABBStatus.Trim(),
                    RMAbstractOthSvc = bRMAOSStatus.Trim(),
                    RMAbstractResSvc = bRMARSStatus.Trim(),

                    RMAvailBoatCapacity = bRMABCStatus.Trim(),
                    RMBoatwiseTrip = bRMBTStatus.Trim(),
                    RMTripSheetSettle = bRMTSStatus.Trim(),

                    RMRowerCharges = bRMRCStatus.Trim(),
                    RMBoatCancellation = bRMBCStatus.Trim(),
                    RMRowerSettle = bRMRSStatus.Trim(),

                    RMChallanRegister = bRMCRStatus.Trim(),
                    // RMAbstractChallanRegister = bRMACRStatus.Trim(),
                    RMServiceWiseCollection = bRMSWCStatus.Trim(),

                    RMUserBookingReport = bRMUBRStatus.Trim(),
                    RMTripWiseCollection = bRMTWCStatus.Trim(),
                    RMBoatTypeRowerList = bRMBTRLStatus.Trim(),

                    OfflineRights = AndroidOffline,

                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim(),
                };

                response = client.PostAsJsonAsync("AdminUserAccess", UserAccess).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        clear();
                        BindAdminAccess();
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clear();
        BindAdminAccess();
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnSubmit.Text = "Update";
            btnCancel.Visible = true;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvAdminAccess.DataKeys[gvrow.RowIndex].Value.ToString();
            Label UserId = (Label)gvrow.FindControl("lblUserId");
            Label UserRole = (Label)gvrow.FindControl("lblUserRole");
            string MBS = gvAdminAccess.DataKeys[gvrow.RowIndex]["MBoating"].ToString().Trim();

            Label MBoatInfoDisplay = (Label)gvrow.FindControl("lblMBoatInfoDisplay");
            Label BMaster = (Label)gvrow.FindControl("lblBMaster");
            Label BTransaction = (Label)gvrow.FindControl("lblBTransaction");
            Label BBooking = (Label)gvrow.FindControl("lblBBooking");
            Label BReports = (Label)gvrow.FindControl("lblBReports");

            Label BMBooking = (Label)gvrow.FindControl("lblBMBooking");
            Label BMBookingOthers = (Label)gvrow.FindControl("lblBMBookingOthers");
            Label BMBulkBooking = (Label)gvrow.FindControl("lblBMBulkBooking");
            Label BMAdditionalService = (Label)gvrow.FindControl("lblBMAdditionalService");
            Label BMOtherService = (Label)gvrow.FindControl("lblBMOtherService");
            Label BMBulkOtherService = (Label)gvrow.FindControl("lblBMBulkOtherService");
            Label BMKioskBooking = (Label)gvrow.FindControl("lblBMKioskBooking");
            Label BMTripSheet = (Label)gvrow.FindControl("lblBMTripSheet");
            Label BMChangeTripSheet = (Label)gvrow.FindControl("lblBMChangeTripSheet");
            Label BMBoatReTripDetails = (Label)gvrow.FindControl("lblBMBoatReTripDetails");
            Label BMChangeBoatDetails = (Label)gvrow.FindControl("lblBMChangeBoatDetails");
            Label BMCancellation = (Label)gvrow.FindControl("lblBMCancellation");
            Label BMReSchedule = (Label)gvrow.FindControl("lblBMReSchedule");
            Label BRestaurant = (Label)gvrow.FindControl("lblBRestaurant");
            Label BGeneratingBoardingPass = (Label)gvrow.FindControl("lblBGeneratingBoardingPass");
            Label BGenerateManualTicket = (Label)gvrow.FindControl("lblBGenerateManualTicket");
            //Newly Added
            Label BMKioskOtherService = (Label)gvrow.FindControl("lblBMKioskOtherService");

            //Newly Added
            Label TMMaterialPur = (Label)gvrow.FindControl("lblTMMaterialPur");
            Label TMMaterialIss = (Label)gvrow.FindControl("lblTMMaterialIss");
            Label TMTripSheetSettle = (Label)gvrow.FindControl("lblTMTripSheetSettle");
            Label TMRowerSettle = (Label)gvrow.FindControl("lblTMRowerSettle");
            Label TMRefundCounter = (Label)gvrow.FindControl("lblTMRefundCounter");
            Label TMStockEntryMaintance = (Label)gvrow.FindControl("lblTMFoodStockEntryMaintance");
            Label TMReceiptBalanceRefund = (Label)gvrow.FindControl("lblTMReceiptBalanceRefund");

            Label RMBooking = (Label)gvrow.FindControl("lblRMBooking");
            Label RMOtherSvc = (Label)gvrow.FindControl("lblRMOtherSvc");
            Label RMRestaurantService = (Label)gvrow.FindControl("lblRMRestaurantService");

            //Newly Added 

            Label RMAdditionalTicket = (Label)gvrow.FindControl("lblRMAdditionalTicket");

            Label RMAbstractAdditionalTicket = (Label)gvrow.FindControl("lblRMAbstractAdditionalTicket");

            Label RMDepositStatus = (Label)gvrow.FindControl("lblRMDepositStatus");

            Label RMDiscountReport = (Label)gvrow.FindControl("lblRMDiscountReport");

            Label RMCashinHands = (Label)gvrow.FindControl("lblRMCashinHands");

            Label RMExtendedBoatHouse = (Label)gvrow.FindControl("lblRMExtendedBoatHouse");

            Label RMPrintBoatBooking = (Label)gvrow.FindControl("lblRMPrintBoatBooking");



            Label RMTripWiseDetails = (Label)gvrow.FindControl("lblRMTripWiseDetails");

            Label RMReceiptBalance = (Label)gvrow.FindControl("lblRMReceiptBalance");

            //Label RMAbstractBooking = (Label)gvrow.FindControl("lblRMAbstractBooking");

            Label RMRePrintReport = (Label)gvrow.FindControl("lblRMRePrintReport");

            Label RMQRCodeGeneration = (Label)gvrow.FindControl("lblRMQRCodeGeneration");

            //Newly Added

            Label RMAbstractBoatBook = (Label)gvrow.FindControl("lblRMAbstractBoatBook");
            Label RMAbstractOthSvc = (Label)gvrow.FindControl("lblRMAbstractOthSvc");
            Label RMAbstractResSvc = (Label)gvrow.FindControl("lblRMAbstractResSvc");

            Label RMAvailBoatCapacity = (Label)gvrow.FindControl("lblRMAvailBoatCapacity");
            Label RMBoatwiseTrip = (Label)gvrow.FindControl("lblRMBoatwiseTrip");
            Label RMTripSheetSettle = (Label)gvrow.FindControl("lblRMTripSheetSettle");

            Label RMRowerCharges = (Label)gvrow.FindControl("lblRMRowerCharges");
            Label RMBoatCancellation = (Label)gvrow.FindControl("lblRMBoatCancellation");
            Label RMRowerSettle = (Label)gvrow.FindControl("lblRMRowerSettle");

            Label RMChallanRegister = (Label)gvrow.FindControl("lblRMChallanRegister");
            //Label RMAbstractChallanRegister = (Label)gvrow.FindControl("lblRMAbstractChallanRegister");
            Label RMServiceWiseCollection = (Label)gvrow.FindControl("lblRMServiceWiseCollection");

            Label RMUserBookingReport = (Label)gvrow.FindControl("lblRMUserBookingReport");
            Label RMTripWiseCollection = (Label)gvrow.FindControl("lblRMTripWiseCollection");
            Label RMBoatTypeRowerList = (Label)gvrow.FindControl("lblRMBoatTypeRowerList");

            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            Label BBoatingService = (Label)gvrow.FindControl("lblBBoatingService");
            Label BAdditionalService = (Label)gvrow.FindControl("lblBAdditionalService");
            Label BOtherService = (Label)gvrow.FindControl("lblBOtherService");
            Label BTripSheetOptions = (Label)gvrow.FindControl("lblBTripSheetOptions");
            Label BDepositRefundOptions = (Label)gvrow.FindControl("lblBDepositRefundOptions");

            ddlUserName.SelectedValue = UserId.Text.Trim();
            ddlUserName.Enabled = true;

            //Info Display
            chkBoatInfoDisp.Checked = false;

            if (MBoatInfoDisplay.Text.Trim() == "Y")
            {
                chkBoatInfoDisp.Checked = true;
            }

            // Module Rights

            ChkBoatModule.Checked = false;

            if (MBS.Trim() == "Y")
            {
                ChkBoatModule.Checked = true;
            }

            // Android Offline Rights

            ChkAnroidOffline.Checked = false;

            if (gvAdminAccess.DataKeys[gvrow.RowIndex]["OfflineRights"].ToString().Trim() == "Y")
            {
                ChkAnroidOffline.Checked = true;
            }

            // Menu Rights

            chkMenuBooking.Checked = false;
            chkMenuTransaction.Checked = false;
            chkMenuReports.Checked = false;

            if (BBooking.Text == "Y" || BBooking.Text.ToUpper() == "YES")
            {
                chkMenuBooking.Checked = true;
            }

            if (BTransaction.Text == "Y" || BTransaction.Text.ToUpper() == "YES")
            {
                chkMenuTransaction.Checked = true;
            }

            if (BReports.Text == "Y" || BReports.Text.ToUpper() == "YES")
            {
                chkMenuReports.Checked = true;
            }

            // Sub Menu Rights For Boat Service Menu

            if (BMBooking.Text == "Y" || BMBulkBooking.Text == "Y")
            {
                BindBoatingServiceList();

                if (BBoatingService.Text == "")
                {
                    chkMenuBoatingSvc.Checked = false;
                    foreach (ListItem item in ckblBoatingSvc.Items)
                    {
                        item.Selected = false;
                    }

                }
                else
                {
                    chkMenuBoatingSvc.Checked = true;
                    string bBoatSvc = BBoatingService.Text.Trim();
                    string[] sbBoatSvc = bBoatSvc.Split(',');
                    int bBoatSvcCount = sbBoatSvc.Count();
                    for (int i = 0; i < bBoatSvcCount; i++)
                    {
                        foreach (ListItem item in ckblBoatingSvc.Items)
                        {
                            string selectedValue = item.Value;
                            if (selectedValue == sbBoatSvc[i].ToString())
                            {
                                item.Selected = true;
                            }
                        }
                    }

                }
            }
            else
            {
                BindBoatingServiceList();
                chkMenuBoatingSvc.Checked = false;
            }

            // Sub Menu Rights For Additional Service Menu

            if (BMAdditionalService.Text == "Y")
            {
                BindAddlServiceList();

                if (BAdditionalService.Text == "")
                {
                    chkMenuAdditionalSvc.Checked = false;
                    foreach (ListItem item in ckblAddlSvc.Items)
                    {
                        item.Selected = false;
                    }

                }
                else
                {
                    chkMenuAdditionalSvc.Checked = true;
                    string bAddlSvc = BAdditionalService.Text.Trim();
                    string[] sbAddlSvc = bAddlSvc.Split(',');
                    int bAddlSvcCount = sbAddlSvc.Count();
                    for (int i = 0; i < bAddlSvcCount; i++)
                    {
                        foreach (ListItem item in ckblAddlSvc.Items)
                        {
                            string selectedValue = item.Value;
                            if (selectedValue == sbAddlSvc[i].ToString())
                            {
                                item.Selected = true;
                            }
                        }
                    }

                }
            }
            else
            {
                BindAddlServiceList();
                chkMenuAdditionalSvc.Checked = false;
            }

            // Sub Menu Rights For OtherService Menu

            if (BMOtherService.Text == "Y")
            {
                BindOtherCategoryList();

                if (BOtherService.Text == "")
                {
                    chkMenuOtherSvc.Checked = false;
                    foreach (ListItem item in ckblOtherSvc.Items)
                    {
                        item.Selected = true;
                    }

                }
                else
                {
                    chkMenuOtherSvc.Checked = true;
                    string bOtherSvc = BOtherService.Text.Trim();
                    string[] sbOtherSvc = bOtherSvc.Split(',');
                    int bOtherSvcCount = sbOtherSvc.Count();
                    for (int i = 0; i < bOtherSvcCount; i++)
                    {
                        foreach (ListItem item in ckblOtherSvc.Items)
                        {
                            string selectedValue = item.Value;
                            if (selectedValue == sbOtherSvc[i].ToString())
                            {
                                item.Selected = true;
                            }
                        }
                    }

                }
            }
            else
            {
                BindOtherCategoryList();
                chkMenuOtherSvc.Checked = false;
            }

            // Sub Menu Rights For Trip Sheet Menu


            if (BMTripSheet.Text == "Y")
            {
                if (BTripSheetOptions.Text == "")
                {
                    chkMenuTripSheet.Checked = false;
                    foreach (ListItem itemTS in ckblTripSheet.Items)
                    {
                        itemTS.Selected = false;
                    }

                }
                else
                {
                    chkMenuTripSheet.Checked = true;
                    string bTripSheet = BTripSheetOptions.Text.Trim();
                    string[] sbTripSheet = bTripSheet.Split(',');
                    int bTripSheetCount = sbTripSheet.Count();
                    for (int i = 0; i < bTripSheetCount; i++)
                    {
                        if (sbTripSheet[i].ToString() == "Y")
                        {
                            ckblTripSheet.Items[i].Selected = true;
                        }
                        else
                        {
                            ckblTripSheet.Items[i].Selected = false;
                        }
                    }

                }
            }
            else
            {
                chkMenuTripSheet.Checked = false;
            }


            // Sub Menu Rights For Booking Menu

            for (int i = 0; i < ckblBooking.Items.Count; i++)
            {
                if (BMBooking.Text == "Y")
                {
                    ckblBooking.Items[0].Selected = true;
                }
                else
                {
                    ckblBooking.Items[0].Selected = false;
                }

                if (BMBulkBooking.Text == "Y")
                {
                    ckblBooking.Items[1].Selected = true;
                }
                else
                {
                    ckblBooking.Items[1].Selected = false;
                }

                if (BMAdditionalService.Text == "Y")
                {
                    ckblBooking.Items[2].Selected = true;
                }
                else
                {
                    ckblBooking.Items[2].Selected = false;
                }

                if (BMOtherService.Text == "Y")
                {
                    ckblBooking.Items[3].Selected = true;
                }
                else
                {
                    ckblBooking.Items[3].Selected = false;
                }

                if (BMBulkOtherService.Text == "Y")
                {
                    ckblBooking.Items[4].Selected = true;
                }
                else
                {
                    ckblBooking.Items[4].Selected = false;
                }

                if (BRestaurant.Text == "Y" || BRestaurant.Text.ToUpper() == "YES")
                {
                    ckblBooking.Items[5].Selected = true;
                }
                else
                {
                    ckblBooking.Items[5].Selected = false;
                }

                if (BMTripSheet.Text == "Y")
                {
                    ckblBooking.Items[6].Selected = true;
                }
                else
                {
                    ckblBooking.Items[6].Selected = false;
                }

                if (BMCancellation.Text == "Y")
                {
                    ckblBooking.Items[7].Selected = true;
                }
                else
                {
                    ckblBooking.Items[7].Selected = false;
                }

                if (BMReSchedule.Text == "Y")
                {
                    ckblBooking.Items[8].Selected = true;
                }
                else
                {
                    ckblBooking.Items[8].Selected = false;
                }

                if (BGeneratingBoardingPass.Text == "Y")
                {
                    ckblBooking.Items[9].Selected = true;
                }
                else
                {
                    ckblBooking.Items[9].Selected = false;
                }

                if (BGenerateManualTicket.Text == "Y")
                {
                    ckblBooking.Items[10].Selected = true;
                }
                else
                {
                    ckblBooking.Items[10].Selected = false;
                }

                if (BMChangeBoatDetails.Text == "Y")
                {
                    ckblBooking.Items[11].Selected = true;
                }
                else
                {
                    ckblBooking.Items[11].Selected = false;
                }
                if (BMChangeTripSheet.Text == "Y")
                {
                    ckblBooking.Items[12].Selected = true;
                }
                else
                {
                    ckblBooking.Items[12].Selected = false;
                }
                if (BMBoatReTripDetails.Text == "Y")
                {
                    ckblBooking.Items[13].Selected = true;
                }
                else
                {
                    ckblBooking.Items[13].Selected = false;
                }

                if (BMKioskBooking.Text == "Y")
                {
                    ckblBooking.Items[14].Selected = true;
                }
                else
                {
                    ckblBooking.Items[14].Selected = false;
                }

                //Newly added 


                if (BMKioskOtherService.Text == "Y")
                {
                    ckblBooking.Items[15].Selected = true;
                }
                else
                {
                    ckblBooking.Items[15].Selected = false;
                }
            }

            // Sub Menu Rights For Transaction Menu

            for (int i = 0; i < ckblTransaction.Items.Count; i++)
            {

                if (TMTripSheetSettle.Text == "Y")
                {
                    ckblTransaction.Items[0].Selected = true;
                }
                else
                {
                    ckblTransaction.Items[0].Selected = false;
                }
                if (TMRowerSettle.Text == "Y")
                {
                    ckblTransaction.Items[1].Selected = true;
                }
                else
                {
                    ckblTransaction.Items[1].Selected = false;
                }
                if (TMRefundCounter.Text == "Y")
                {
                    ckblTransaction.Items[2].Selected = true;
                }
                else
                {
                    ckblTransaction.Items[2].Selected = false;
                }
                if (TMReceiptBalanceRefund.Text == "Y")
                {
                    ckblTransaction.Items[3].Selected = true;
                }
                else
                {
                    ckblTransaction.Items[3].Selected = false;
                }
                if (TMStockEntryMaintance.Text == "Y")
                {
                    ckblTransaction.Items[4].Selected = true;
                }
                else
                {
                    ckblTransaction.Items[4].Selected = false;
                }
                if (TMMaterialPur.Text == "Y")
                {
                    ckblTransaction.Items[5].Selected = true;
                }
                else
                {
                    ckblTransaction.Items[5].Selected = false;
                }
                if (TMMaterialIss.Text == "Y")
                {
                    ckblTransaction.Items[6].Selected = true;
                }
                else
                {
                    ckblTransaction.Items[6].Selected = false;
                }
            }

            // Sub Menu Rights For Deposit Refund Menu


            if (TMTripSheetSettle.Text == "Y")
            {
                if (BDepositRefundOptions.Text == "")
                {
                    chkMenuDepositRefund.Checked = false;
                    foreach (ListItem itemTS in ckblDepositRefund.Items)
                    {
                        itemTS.Selected = false;
                    }

                }
                else
                {
                    chkMenuDepositRefund.Checked = true;
                    string bDepositRefund = BDepositRefundOptions.Text.Trim();
                    string[] sbDepositRefund = bDepositRefund.Split(',');
                    int bDepositRefundCount = sbDepositRefund.Count();
                    for (int i = 0; i < bDepositRefundCount; i++)
                    {
                        if (sbDepositRefund[i].ToString() == "Y")
                        {
                            ckblDepositRefund.Items[i].Selected = true;
                        }
                        else
                        {
                            ckblDepositRefund.Items[i].Selected = false;
                        }
                    }

                }
            }
            else
            {
                chkMenuDepositRefund.Checked = false;
            }

            // Sub Menu Rights For Reports Menu


            for (int i = 0; i < ckblReports.Items.Count; i++)
            {
                if (RMBooking.Text == "Y")
                {
                    ckblReports.Items[0].Selected = true;
                }
                else
                {
                    ckblReports.Items[0].Selected = false;
                }
                if (RMAdditionalTicket.Text.Trim() == "Y")
                {
                    ckblReports.Items[1].Selected = true;
                }
                else
                {
                    ckblReports.Items[1].Selected = false;
                }

                if (RMOtherSvc.Text == "Y")
                {
                    ckblReports.Items[2].Selected = true;
                }
                else
                {
                    ckblReports.Items[2].Selected = false;
                }
                if (RMRestaurantService.Text == "Y")
                {
                    ckblReports.Items[3].Selected = true;
                }
                else
                {
                    ckblReports.Items[3].Selected = false;
                }

                if (RMAbstractBoatBook.Text == "Y")
                {
                    ckblReports.Items[4].Selected = true;
                }
                else
                {
                    ckblReports.Items[4].Selected = false;
                }
                if (RMAbstractAdditionalTicket.Text == "Y")
                {
                    ckblReports.Items[5].Selected = true;
                }
                else
                {
                    ckblReports.Items[5].Selected = false;
                }

                if (RMAbstractOthSvc.Text == "Y")
                {
                    ckblReports.Items[6].Selected = true;
                }
                else
                {
                    ckblReports.Items[6].Selected = false;
                }
                if (RMAbstractResSvc.Text == "Y")
                {
                    ckblReports.Items[7].Selected = true;
                }
                else
                {
                    ckblReports.Items[7].Selected = false;
                }
                if (RMBoatwiseTrip.Text == "Y")
                {
                    ckblReports.Items[8].Selected = true;
                }
                else
                {
                    ckblReports.Items[8].Selected = false;
                }

                if (RMTripSheetSettle.Text == "Y")
                {
                    ckblReports.Items[9].Selected = true;
                }
                else
                {
                    ckblReports.Items[9].Selected = false;
                }
                if (RMTripWiseCollection.Text == "Y")
                {
                    ckblReports.Items[10].Selected = true;
                }
                else
                {
                    ckblReports.Items[10].Selected = false;
                }
                if (RMDepositStatus.Text == "Y")
                {
                    ckblReports.Items[11].Selected = true;
                }
                else
                {
                    ckblReports.Items[11].Selected = false;
                }
                if (RMDiscountReport.Text == "Y")
                {
                    ckblReports.Items[12].Selected = true;
                }
                else
                {
                    ckblReports.Items[12].Selected = false;
                }

                if (RMCashinHands.Text == "Y")
                {
                    ckblReports.Items[13].Selected = true;
                }
                else
                {
                    ckblReports.Items[13].Selected = false;
                }

                if (RMRowerCharges.Text == "Y")
                {
                    ckblReports.Items[14].Selected = true;
                }
                else
                {
                    ckblReports.Items[14].Selected = false;
                }
                if (RMRowerSettle.Text == "Y")
                {
                    ckblReports.Items[15].Selected = true;
                }
                else
                {
                    ckblReports.Items[15].Selected = false;
                }
                if (RMBoatTypeRowerList.Text == "Y")
                {
                    ckblReports.Items[16].Selected = true;
                }
                else
                {
                    ckblReports.Items[16].Selected = false;
                }
                if (RMExtendedBoatHouse.Text == "Y")
                {
                    ckblReports.Items[17].Selected = true;
                }
                else
                {
                    ckblReports.Items[17].Selected = false;
                }
                if (RMPrintBoatBooking.Text == "Y")
                {
                    ckblReports.Items[18].Selected = true;
                }
                else
                {
                    ckblReports.Items[18].Selected = false;
                }
                if (RMTripWiseDetails.Text == "Y")
                {
                    ckblReports.Items[19].Selected = true;
                }
                else
                {
                    ckblReports.Items[19].Selected = false;
                }
                if (RMAvailBoatCapacity.Text == "Y")
                {
                    ckblReports.Items[20].Selected = true;
                }
                else
                {
                    ckblReports.Items[20].Selected = false;
                }
                if (RMBoatCancellation.Text == "Y")
                {
                    ckblReports.Items[21].Selected = true;
                }
                else
                {
                    ckblReports.Items[21].Selected = false;
                }
                if (RMChallanRegister.Text == "Y")
                {
                    ckblReports.Items[22].Selected = true;
                }
                else
                {
                    ckblReports.Items[22].Selected = false;
                }
                if (RMServiceWiseCollection.Text == "Y")
                {
                    ckblReports.Items[23].Selected = true;
                }
                else
                {
                    ckblReports.Items[23].Selected = false;
                }
                if (RMUserBookingReport.Text == "Y")
                {
                    ckblReports.Items[24].Selected = true;
                }
                else
                {
                    ckblReports.Items[24].Selected = false;
                }

                if (RMReceiptBalance.Text == "Y")
                {
                    ckblReports.Items[25].Selected = true;
                }
                else
                {
                    ckblReports.Items[25].Selected = false;
                }

                if (RMRePrintReport.Text.Trim() == "Y")
                {
                    ckblReports.Items[26].Selected = true;
                }
                else
                {
                    ckblReports.Items[26].Selected = false;
                }

                if (RMQRCodeGeneration.Text.Trim() == "Y")
                {
                    ckblReports.Items[27].Selected = true;
                }
                else
                {
                    ckblReports.Items[27].Selected = false;
                }

            }
          
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvAdminAccess_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAdminAccess.PageIndex = e.NewPageIndex;
        BindAdminAccess();
    }

    public class AdminAccess
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }

        public string MBoatInfoDisplay { get; set; }

        public string MMaster { get; set; }
        public string MBS { get; set; }
        public string MTMS { get; set; }
        public string MHMS { get; set; }
        public string MAccounts { get; set; }

        public string MComMaster { get; set; }
        public string MBhMaster { get; set; }
        public string MHotelMaster { get; set; }
        public string MTourMaster { get; set; }
        public string MAccessRights { get; set; }
        public string MOtherMaster { get; set; }

        public string BMaster { get; set; }
        public string BTransaction { get; set; }
        public string BBooking { get; set; }
        public string BReports { get; set; }
        public string BRestaurant { get; set; }

        public string BGeneratingBoardingPass { get; set; }

        public string BGenerateManualTicket { get; set; }
        public string BBoatingService { get; set; }
        public string BAdditionalService { get; set; }
        public string BOtherService { get; set; }

        public string BMBooking { get; set; }
        public string BMBookingOthers { get; set; }
        public string BMBulkBooking { get; set; }
        public string BMOtherService { get; set; }
        public string BMAdditionalService { get; set; }
        public string BMBulkOtherService { get; set; }
        public string BMKioskBooking { get; set; }
        public string BMTripSheet { get; set; }
        public string BTripSheetOptions { get; set; }
        public string BMChangeTripSheet { get; set; }
        public string BMBoatReTripDetails { get; set; }
        public string BMChangeBoatDetails { get; set; }
        public string BMCancellation { get; set; }
        public string BMReSchedule { get; set; }


        public string TMMaterialPur { get; set; }
        public string TMMaterialIss { get; set; }
        public string TMTripSheetSettle { get; set; }
        public string TMRowerSettle { get; set; }
        public string TMRefundCounter { get; set; }

        public string TMStockEntryMaintance { get; set; }

        public string TMReceiptBalanceRefund { get; set; }

        public string BDepositRefundOptions { get; set; }

        public string RMBooking { get; set; }
        public string RMOtherSvc { get; set; }
        public string RMRestaurantService { get; set; }

        //Newly Added 
        public string RMAdditionalTicket { get; set; }

        public string RMAbstractAdditionalTicket { get; set; }

        public string RMDepositStatus { get; set; }
        public string RMDiscountReport { get; set; }

        public string RMCashinHands { get; set; }
        public string RMExtendedBoatHouse { get; set; }
        public string RMPrintBoatBooking { get; set; }


        public string RMTripWiseDetails { get; set; }

        public string RMReceiptBalance { get; set; }



        public string RMRePrintReport { get; set; }

        public string RMQRCodeGeneration { get; set; }


        public string BMKioskOtherService { get; set; }



        //Newly Added




        public string RMAbstractBoatBook { get; set; }
        public string RMAbstractOthSvc { get; set; }
        public string RMAbstractResSvc { get; set; }

        public string RMAvailBoatCapacity { get; set; }
        public string RMBoatwiseTrip { get; set; }
        public string RMTripSheetSettle { get; set; }

        public string RMRowerCharges { get; set; }
        public string RMBoatCancellation { get; set; }
        public string RMRowerSettle { get; set; }

        public string RMChallanRegister { get; set; }
        public string RMAbstractChallanRegister { get; set; }
        public string RMServiceWiseCollection { get; set; }

        public string RMUserBookingReport { get; set; }
        public string RMTripWiseCollection { get; set; }
        public string RMBoatTypeRowerList { get; set; }

        public string OfflineRights { get; set; }

        public string CreatedBy { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string EmpId { get; set; }
        public string RoleId { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string CountStart { get; set; }
    }

    protected void Back_Click(object sender, EventArgs e)
    {

        int istart;
        int iend;
        subProcess(Int32.Parse(hfstartvalue.Value) - 10, Int32.Parse(hfendvalue.Value) - 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        BindAdminAccess();
    }

    protected void Next_Click(object sender, EventArgs e)
    {
        Back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(hfendvalue.Value) + 1, Int32.Parse(hfendvalue.Value) + 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        BindAdminAccess();
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
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
    }
    protected void subProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
            Back.Enabled = false;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            Back.Enabled = false;

        }
        else
        {
            iend = end;

        }
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
    }
}