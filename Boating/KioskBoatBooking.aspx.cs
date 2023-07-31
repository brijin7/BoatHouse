using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Globalization;
using System.Web.Helpers;

public partial class Boating_KioskBoatBooking : System.Web.UI.Page
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
                toolTipDpt.Visible = false;
                if (Session["BMBooking"].ToString().Trim() == "Y")
                {
                    GetPaymentType();
                    GetTaxDetail();
                    hfBoatNature.Value = "N";
                    BoatBookedSummaryList();

                    if (Session["BMBookingOthers"].ToString().Trim() == "Y")
                    {
                        btnOther.Visible = false;
                    }

                    if (Session["BBMAdditionalService"].ToString().Trim() == "Y")
                    {
                        btnAdditional.Visible = false;
                    }

                    BindBookingCountAmount();
                    //ViewState["OkFlag"] = "P";

                    BoatAvailableLists();
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BoatBookedSummaryList()
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
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    PremiumStatus = hfBoatNature.Value,
                    UserId = Session["UserId"].ToString().Trim(),
                    UserRole = Session["UserRole"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatTypeListForBoatBooking", BoatSearch).Result;

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
                            dtlBoat.Visible = true;
                            dtlBoat.DataSource = dt;
                            dtlBoat.DataBind();
                        }
                        else
                        {
                            dtlBoat.DataBind();
                            dtlBoat.Visible = false;
                        }
                    }
                    else
                    {
                        dtlBoat.Visible = false;
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

    protected void DtlBoat_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.DataItem != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblBoatTypeId = (Label)e.Item.FindControl("lblBoatTypeId");
                Label lblBtBoatType = (Label)e.Item.FindControl("lblBtBoatType");

                var BoatSeat = e.Item.FindControl("dtlBoatchild") as DataList;

                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var BoatSearch = new BoatSearch()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString(),
                            PremiumStatus = hfBoatNature.Value.Trim(),
                            BoatTypeId = lblBoatTypeId.Text.Trim('~'),
                            BookingDate = DateTime.Today.ToString("dd/MM/yyyy")

                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("BoatBooking/BoatAvailableList", BoatSearch).Result;

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
                                    hfNatureVisible.Value = "1";
                                    premimumMsg.InnerText = "";
                                    premimumMsg.Visible = false;

                                    BoatSeat.DataSource = dt;
                                    BoatSeat.DataBind();
                                    BoatSeat.Visible = true;
                                }
                                else
                                {
                                    BoatSeat.DataSource = dt;
                                    BoatSeat.DataBind();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Details Not Found !');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert(" + ResponseMsg + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                        }
                    }

                    if (hfNatureVisible.Value.Trim() == "0")
                    {
                        string Message = "No Express Boat Available";

                        if (hfBoatNature.Value.Trim() == "N")
                        {
                            Message = "No Normal Boat Available";
                        }

                        premimumMsg.InnerText = Message;
                        premimumMsg.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert(" + premimumMsg + "');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindSlotType()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var AvailableBoatBookingSlotDept = new AvailableBoatBookingSlotDept()
                {
                    QueryType = "GetAvailableSlot",
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    CheckInDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    BoatTypeId = ViewState["SlotBoatTypeId"].ToString().Trim(),
                    BoatSeaterId = ViewState["SlotBoatSeaterId"].ToString().Trim(),
                    SlotType = hfBoatNature.Value,
                    UserId = Session["UserId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("GetAvailableBoatSlotTimeDept", AvailableBoatBookingSlotDept).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Slto = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Slto)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Slto)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ViewState["SlotId"] = dt.Rows[0]["SlotId"].ToString().Trim();
                            //  ViewState["SlotType"] = dt.Rows[0]["SlotStartTime"].ToString().Trim() + "-" + dt.Rows[0]["SlotEndTime"].ToString().Trim();
                            ViewState["SlotType"] = dt.Rows[0]["SlotStartTime"].ToString().Trim();
                            ViewState["BlockId"] = dt.Rows[0]["BlockId"].ToString().Trim();

                            ViewState["sBlockId"] += dt.Rows[0]["BlockId"].ToString().Trim() + "~";

                            ViewState["SlotFlag"] = "N";

                            if (hfBoatNature.Value == "N" || hfBoatNature.Value == "P")
                            {
                                ViewState["AvailableTripCount"] = dt.Rows[0]["AvailableBoatCount"].ToString().Trim();
                            }
                            else if (hfBoatNature.Value == "I")
                            {
                                ViewState["AvailableTripCount"] = dt.Rows[0]["AvailableSeatCount"].ToString().Trim();
                            }
                            else
                            {
                                return;
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
                            return;
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


    /// <summary>
    /// Modified By Silambarasu D 
    /// Date 10 Aug 2021
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>

    protected void dtlBoatchild_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
            string BoatTypeId = string.Empty;
            string BoatTypeName = string.Empty;

            string BoatSeaterId = string.Empty;
            string BoatSeaterType = string.Empty;

            string BoatCount = string.Empty;
            string BoatTotalCharge = string.Empty;

            string BoatMinCharge = string.Empty;
            string RowerMinCharge = string.Empty;
            string BoatTaxCharge = string.Empty;
            string DepositType = string.Empty;
            string Deposit = string.Empty;

            Label lblBoatTypeId = (Label)e.Item.FindControl("lblBoatTypeId");
            BoatTypeId = lblBoatTypeId.Text;

            Label lblBoatTypes = (Label)e.Item.FindControl("lblBoatTypes");
            BoatTypeName = lblBoatTypes.Text;

            Label lblBoatSeaterId = (Label)e.Item.FindControl("lblBoatSeaterId");
            BoatSeaterId = lblBoatSeaterId.Text;

            Label lblSeaterTypes = (Label)e.Item.FindControl("lblSeaterTypes");
            BoatSeaterType = lblSeaterTypes.Text;

            DropDownList NoCount = (DropDownList)e.Item.FindControl("dlstCount");
            BoatCount = NoCount.SelectedValue;

            Label lblBoatTotalCharge = (Label)e.Item.FindControl("lblBoatTotalCharge");
            BoatTotalCharge = lblBoatTotalCharge.Text;

            Label lblBoatMinCharge = (Label)e.Item.FindControl("lblBoatMinCharge");
            BoatMinCharge = lblBoatMinCharge.Text;

            Label lblRowerMinCharge = (Label)e.Item.FindControl("lblRowerMinCharge");
            RowerMinCharge = lblRowerMinCharge.Text;

            Label lblBoatTaxCharge = (Label)e.Item.FindControl("lblBoatTaxCharge");
            BoatTaxCharge = lblBoatTaxCharge.Text;

            Label lblDepositType = (Label)e.Item.FindControl("lblDepositType");
            DepositType = lblDepositType.Text;

            Label lblDeposit = (Label)e.Item.FindControl("lblDeposit");
            Deposit = lblDeposit.Text;

            Label lblBoatMinTime = (Label)e.Item.FindControl("lblBoatMinTime");
            string BoatMinTime = lblBoatMinTime.Text;

            decimal BoatDepositAmount = 0;

            if (DepositType == "F")
            {
                BoatDepositAmount = (Convert.ToDecimal(BoatCount) * Convert.ToDecimal(Deposit));
            }
            else
            {
                //decimal bAmount = (Convert.ToDecimal(BoatCount) * Convert.ToDecimal(BoatMinCharge)) + (Convert.ToDecimal(BoatCount) * Convert.ToDecimal(RowerMinCharge));

                BoatDepositAmount = ((Convert.ToDecimal(BoatTotalCharge) * Convert.ToDecimal(Deposit)) / 100);
            }
            ViewState["SlotBoatTypeId"] = BoatTypeId.ToString().Trim();
            ViewState["SlotBoatSeaterId"] = BoatSeaterId.ToString().Trim();

            BindSlotType();
            string BoatNature = string.Empty;
            if (hfBoatNature.Value == "N")
            {
                BoatNature = "Normal";
            }
            else if (hfBoatNature.Value == "P")
            {
                BoatNature = "Express";
            }
            else
            {
                BoatNature = "Individual";
            }
            if (ViewState["SlotId"].ToString().Trim() != "" && ViewState["AvailableTripCount"].ToString().Trim() != "")
            {

                BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatNature, BoatCount, BoatTotalCharge, BoatMinCharge,
                    RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount.ToString("0.00"), BoatMinTime, ViewState["SlotType"].ToString().Trim(),
                    ViewState["SlotId"].ToString().Trim(), ViewState["AvailableTripCount"].ToString().Trim(), ViewState["BlockId"].ToString().Trim());
            }
            else
            {
                ViewState["SlotId"] = "0";
                ViewState["AvailableTripCount"] = "0";
                ViewState["SlotType"] = "";
                ViewState["BlockId"] = "";
                //if (ViewState["OkFlag"].ToString() == "O")
                //{
                MpepnlPopup.Hide();
                BindLastSlot();

                if (ViewState["SlotId"].ToString().Trim() != "")
                {
                    if (ViewState["NewSlotStatus"].ToString().Trim() == "A")
                    {
                        BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatNature, BoatCount, BoatTotalCharge, BoatMinCharge,
                       RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount.ToString("0.00"), BoatMinTime, ViewState["SlotType"].ToString().Trim(),
                       ViewState["SlotId"].ToString().Trim(), ViewState["AvailableTripCount"].ToString().Trim(), ViewState["BlockId"].ToString().Trim());
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
                        return;
                    }
                }
                //}
                //else
                //{
                //    MpepnlPopup.Show();
                //    ViewState["BoatTypeNameLastSlot"] = BoatTypeName;

                //    ViewState["BoatTypeIdLastSlot"] = BoatTypeId;

                //    ViewState["BoatSeaterIdLastSlot"] = BoatSeaterId;
                //    ViewState["BoatSeaterTypeLastSlot"] = BoatSeaterType;

                //    ViewState["BoatNatureLastSlot"] = BoatNature;

                //    ViewState["BoatCountLastSlot"] = BoatCount;
                //    ViewState["BoatTotalChargeLastSlot"] = BoatTotalCharge;

                //    ViewState["BoatMinChargeLastSlot"] = BoatMinCharge;

                //    ViewState["RowerMinChargeLastSlot"] = RowerMinCharge;
                //    ViewState["BoatTaxChargeLastSlot"] = BoatTaxCharge;
                //    ViewState["DepositTypeLastSlot"] = DepositType;
                //    ViewState["BoatDepositAmountLastSlot"] = BoatDepositAmount.ToString("0.00");
                //    ViewState["BoatMinTimeLastSlot"] = BoatMinTime;
                //}


                // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", false);
            }

            btnResend.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void BtnPopUpCancel_Click(object sender, EventArgs e)
    {
        MpepnlPopup.Hide();
        ViewState["OkFlag"] = "P";
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        BindLastSlot();

        if (ViewState["SlotId"].ToString().Trim() != "")
        {
            ViewState["OkFlag"] = "O";
            BindDataDynamicValue(ViewState["BoatTypeNameLastSlot"].ToString(), ViewState["BoatTypeIdLastSlot"].ToString(), ViewState["BoatSeaterTypeLastSlot"].ToString(),
            ViewState["BoatSeaterIdLastSlot"].ToString(), ViewState["BoatNatureLastSlot"].ToString(), ViewState["BoatCountLastSlot"].ToString(),
            ViewState["BoatTotalChargeLastSlot"].ToString(), ViewState["BoatMinChargeLastSlot"].ToString(),
            ViewState["RowerMinChargeLastSlot"].ToString(), ViewState["BoatTaxChargeLastSlot"].ToString(),
            ViewState["DepositTypeLastSlot"].ToString(), ViewState["BoatDepositAmountLastSlot"].ToString(),
            ViewState["BoatMinTimeLastSlot"].ToString(), ViewState["SlotType"].ToString().Trim(),
            ViewState["SlotId"].ToString().Trim(), ViewState["AvailableTripCount"].ToString().Trim(), ViewState["BlockId"].ToString().Trim());
            ViewState["BoatTypeNameLastSlot"] = "";
            ViewState["BoatTypeIdLastSlot"] = "";
            ViewState["BoatSeaterIdLastSlot"] = "";
            ViewState["BoatSeaterTypeLastSlot"] = "";
            ViewState["BoatNatureLastSlot"] = "";
            ViewState["BoatCountLastSlot"] = "";
            ViewState["BoatTotalChargeLastSlot"] = "";
            ViewState["BoatMinChargeLastSlot"] = "";
            ViewState["RowerMinChargeLastSlot"] = "";
            ViewState["BoatTaxChargeLastSlot"] = "";
            ViewState["DepositTypeLastSlot"] = "";
            ViewState["BoatDepositAmountLastSlot"] = "";
            ViewState["BoatMinTimeLastSlot"] = "";
        }
    }

    public void BindLastSlot()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var AvailableBoatBookingSlotDept = new AvailableBoatBookingSlotDept()
                {
                    QueryType = "GetLastSlot",
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    CheckInDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    BoatTypeId = ViewState["SlotBoatTypeId"].ToString().Trim(),
                    BoatSeaterId = ViewState["SlotBoatSeaterId"].ToString().Trim(),
                    SlotType = hfBoatNature.Value,
                    UserId = Session["UserId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("GetAvailableBoatSlotTimeDept", AvailableBoatBookingSlotDept).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Slto = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Slto)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Slto)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ViewState["NewSlotStatus"] = "A";
                            ViewState["SlotId"] = dt.Rows[0]["SlotId"].ToString().Trim();
                            // ViewState["SlotType"] = dt.Rows[0]["SlotStartTime"].ToString().Trim() + "-" + dt.Rows[0]["SlotEndTime"].ToString().Trim();
                            ViewState["SlotType"] = dt.Rows[0]["SlotStartTime"].ToString().Trim();


                            ViewState["BlockId"] = dt.Rows[0]["BlockId"].ToString().Trim();

                            ViewState["sBlockId"] += dt.Rows[0]["BlockId"].ToString().Trim() + "~";

                            ViewState["SlotFlag"] = "Y";

                            if (hfBoatNature.Value == "N" || hfBoatNature.Value == "P")
                            {
                                ViewState["AvailableTripCount"] = dt.Rows[0]["AvailableBoatCount"].ToString().Trim();
                            }
                            else if (hfBoatNature.Value == "I")
                            {
                                ViewState["AvailableTripCount"] = dt.Rows[0]["AvailableSeatCount"].ToString().Trim();
                            }
                            else
                            {
                                return;
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
                            return;
                        }
                    }
                    else
                    {
                        ViewState["NewSlotStatus"] = "D";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
                        return;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

    }


    /// <summary>
    /// Modified By Silambarasu D 
    /// Date 10 Aug 2021
    /// </summary>
    /// <param name="BoatType"></param>
    /// <param name="BoatTypeId"></param>
    /// <param name="SeaterType"></param>
    /// <param name="SeaterTypeId"></param>
    /// <param name="Status"></param>
    /// <param name="BoatCount"></param>
    /// <param name="BoatTotalCharge"></param>
    /// <param name="BoatMinCharge"></param>
    /// <param name="RowerMinCharge"></param>
    /// <param name="BoatTaxCharge"></param>
    /// <param name="DepositType"></param>
    /// <param name="Deposit"></param>
    /// <param name="BoatMinTime"></param>
    /// <param name="SlotDesc"></param>
    /// <param name="SlotId"></param>
    /// <param name="AvailableCount"></param>


    public void BindDataDynamicValue(string BoatType, string BoatTypeId, string SeaterType, string SeaterTypeId, string Status, string BoatCount, string BoatTotalCharge, string BoatMinCharge,
     string RowerMinCharge, string BoatTaxCharge, string DepositType, string Deposit, string BoatMinTime, string SlotDesc, string SlotId, string AvailableCount, string BlockId)
    {

        try
        {
            // Check Boat Seater Availability Conditions

            //decimal BookedboatCount = 0;

            if (ViewState["CartRow"] != null)
            {
                DataTable mt = (DataTable)ViewState["CartRow"];

                if (mt.Rows.Count > 0)
                {
                    DataRow[] result = mt.Select("BoatTypeId = '" + BoatTypeId.Trim() + "' AND SeaterTypeId ='" + SeaterTypeId.Trim() + "' ");


                }
            }


            // Bind an Value in Temp DataTable

            DataTable mytable = new DataTable();

            if (ViewState["Row"] != null)
            {
                mytable = (DataTable)ViewState["Row"];
                DataRow dr = null;

                string Value = BoatTypeId.Trim() + "~" + SeaterTypeId.Trim() + "~" + SlotId.Trim();

                DataRow[] fndUniqueId = mytable.Select("UniqueId = '" + Value.Trim() + "'");

                if (mytable.Rows.Count > 0)
                {
                    dr = mytable.NewRow();

                    dr["UniqueId"] = BoatTypeId.Trim() + "~" + SeaterTypeId.Trim() + "~" + SlotId.Trim();
                    dr["BoatType"] = BoatType.Trim(); ;
                    dr["BoatTypeId"] = BoatTypeId.Trim();

                    dr["SeaterType"] = SeaterType.Trim();
                    dr["SeaterTypeId"] = SeaterTypeId.Trim();
                    dr["Status"] = Status.Trim();

                    dr["BoatCount"] = BoatCount.Trim();
                    dr["BoatTotalCharge"] = BoatTotalCharge;

                    dr["BoatMinCharge"] = BoatMinCharge.Trim();
                    dr["RowerMinCharge"] = RowerMinCharge.Trim();
                    dr["BoatTaxCharge"] = BoatTaxCharge.Trim();
                    dr["DepositType"] = DepositType.Trim();
                    dr["Deposit"] = Deposit.Trim();
                    dr["BoatMinTime"] = BoatMinTime.Trim();
                    dr["SlotDesc"] = SlotDesc.Trim();
                    dr["SlotId"] = SlotId.Trim();
                    dr["BlockId"] = BlockId.Trim();

                    mytable.Rows.Add(dr);

                    ViewState["Row"] = mytable;
                }
            }
            else
            {
                mytable.Columns.Add(new DataColumn("UniqueId", typeof(string)));
                mytable.Columns.Add(new DataColumn("BoatType", typeof(string)));
                mytable.Columns.Add(new DataColumn("BoatTypeId", typeof(string)));

                mytable.Columns.Add(new DataColumn("SeaterType", typeof(string)));
                mytable.Columns.Add(new DataColumn("SeaterTypeId", typeof(string)));
                mytable.Columns.Add(new DataColumn("Status", typeof(string)));

                mytable.Columns.Add(new DataColumn("BoatCount", typeof(Int32)));
                mytable.Columns.Add(new DataColumn("BoatTotalCharge", typeof(decimal)));

                mytable.Columns.Add(new DataColumn("BoatMinCharge", typeof(string)));
                mytable.Columns.Add(new DataColumn("RowerMinCharge", typeof(string)));
                mytable.Columns.Add(new DataColumn("BoatTaxCharge", typeof(string)));
                mytable.Columns.Add(new DataColumn("DepositType", typeof(string)));
                mytable.Columns.Add(new DataColumn("Deposit", typeof(decimal)));
                mytable.Columns.Add(new DataColumn("BoatMinTime", typeof(string)));
                mytable.Columns.Add(new DataColumn("SlotDesc", typeof(string)));
                mytable.Columns.Add(new DataColumn("SlotId", typeof(string)));
                mytable.Columns.Add(new DataColumn("BlockId", typeof(string)));

                DataRow dr1 = mytable.NewRow();
                dr1 = mytable.NewRow();

                dr1["UniqueId"] = BoatTypeId.Trim() + "~" + SeaterTypeId.Trim() + "~" + SlotId.Trim();
                dr1["BoatType"] = BoatType.Trim();
                dr1["BoatTypeId"] = BoatTypeId.Trim();

                dr1["SeaterType"] = SeaterType.Trim();
                dr1["SeaterTypeId"] = SeaterTypeId.Trim();
                dr1["Status"] = Status.Trim();

                dr1["BoatCount"] = BoatCount.Trim();
                dr1["BoatTotalCharge"] = BoatTotalCharge;

                dr1["BoatMinCharge"] = BoatMinCharge.Trim();
                dr1["RowerMinCharge"] = RowerMinCharge.Trim();
                dr1["BoatTaxCharge"] = BoatTaxCharge.Trim();
                dr1["DepositType"] = DepositType.Trim();
                dr1["Deposit"] = Deposit.Trim();
                dr1["BoatMinTime"] = BoatMinTime.Trim();
                dr1["SlotDesc"] = SlotDesc.Trim();
                dr1["SlotId"] = SlotId.Trim();
                dr1["BlockId"] = BlockId.Trim();

                mytable.Rows.Add(dr1);

                ViewState["Row"] = mytable;
            }

            if (mytable.Rows.Count > 0)
            {
                //CalculateSummary();
                ViewState["BoatChargeSum"] = "0";
                ViewState["BoatTaxSum"] = "0";
                ViewState["BoatDepositSum"] = "0";
                ViewState["BoatTotalSum"] = "0";

                ViewState["OtherChargeSum"] = "0"; //Other Service
                ViewState["OtherTaxSum"] = "0"; //Other Service
                if (ViewState["RowO"] != null)
                {
                    DataTable dtTableO = (DataTable)ViewState["RowO"];
                    if (dtTableO.Rows.Count > 0)
                    {
                        decimal dServiceTotalAmount = dtTableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItem")));
                        ViewState["OtherChargeSum"] = Convert.ToDecimal(dServiceTotalAmount).ToString();

                        decimal dChargePerItemTax = dtTableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItemTax")));

                        ViewState["OtherTaxSum"] = (Convert.ToDecimal(dChargePerItemTax)).ToString();
                    }
                }



                decimal dBoatMinCharge = mytable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatMinCharge")));
                decimal dRowerMinCharge = mytable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("RowerMinCharge")));

                ViewState["BoatChargeSum"] = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();

                decimal dGSTAmount = mytable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatTaxCharge")));

                ViewState["BoatTaxSum"] = dGSTAmount.ToString();

                decimal dDeposit = mytable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Deposit")));

                ViewState["BoatDepositSum"] = dDeposit.ToString();

                decimal dTotal = Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge) + Convert.ToDecimal(dGSTAmount) + Convert.ToDecimal(dDeposit);
                bsTotal.InnerText = dTotal.ToString();
                ViewState["BoatTotalSum"] = dTotal.ToString();

                CalculateSummary();
            }

            DataTable dts = mytable.Clone();

            var CartTable = (from row in mytable.AsEnumerable()
                             group row by new
                             {
                                 UniqueId = row.Field<string>("UniqueId"),
                                 BoatType = row.Field<string>("BoatType"),
                                 BoatTypeId = row.Field<string>("BoatTypeId"),

                                 SeaterType = row.Field<string>("SeaterType"),
                                 SeaterTypeId = row.Field<string>("SeaterTypeId"),
                                 Status = row.Field<string>("Status"),

                                 //BoatCount = row.Field<Int32>("BoatCount"),
                                 //BoatTotalCharge = row.Field<decimal>("BoatTotalCharge"),

                                 BoatMinCharge = row.Field<string>("BoatMinCharge"),
                                 RowerMinCharge = row.Field<string>("RowerMinCharge"),
                                 BoatTaxCharge = row.Field<string>("BoatTaxCharge"),

                                 DepositType = row.Field<string>("DepositType"),
                                 Deposit = row.Field<decimal>("Deposit"),
                                 BoatMinTime = row.Field<string>("BoatMinTime"),
                                 SlotDesc = row.Field<string>("SlotDesc"),
                                 SlotId = row.Field<string>("SlotId"),

                             } into t1
                             select new
                             {
                                 UniqueID = t1.Key.UniqueId,
                                 BoatType = t1.Key.BoatType,
                                 BoatTypeId = t1.Key.BoatTypeId,

                                 SeaterType = t1.Key.SeaterType,
                                 SeaterTypeId = t1.Key.SeaterTypeId,
                                 Status = t1.Key.Status,

                                 BoatCount = t1.Sum(a => a.Field<Int32>("BoatCount")),
                                 BoatTotalCharge = t1.Sum(a => a.Field<decimal>("BoatTotalCharge")) + t1.Sum(b => b.Field<decimal>("Deposit")),

                                 BoatMinCharge = t1.Key.BoatMinCharge,
                                 RowerMinCharge = t1.Key.RowerMinCharge,
                                 BoatTaxCharge = t1.Key.BoatTaxCharge,

                                 DepositType = t1.Key.DepositType,
                                 Deposit = t1.Key.Deposit,
                                 BoatMinTime = t1.Key.BoatMinTime,
                                 SlotDesc = t1.Key.SlotDesc,
                                 SlotId = t1.Key.SlotId,

                             })
                 .Select(g =>
                 {
                     var h = dts.NewRow();
                     h["UniqueId"] = g.UniqueID;
                     h["BoatType"] = g.BoatType;
                     h["BoatTypeId"] = g.BoatTypeId;

                     h["SeaterType"] = g.SeaterType;
                     h["SeaterTypeId"] = g.SeaterTypeId;
                     h["Status"] = g.Status;

                     h["BoatCount"] = g.BoatCount;
                     h["BoatTotalCharge"] = g.BoatTotalCharge;

                     h["BoatMinCharge"] = g.BoatMinCharge;
                     h["RowerMinCharge"] = g.RowerMinCharge;
                     h["BoatTaxCharge"] = g.BoatTaxCharge;

                     h["DepositType"] = g.DepositType;
                     h["Deposit"] = g.Deposit;
                     h["BoatMinTime"] = g.BoatMinTime;
                     h["SlotDesc"] = g.SlotDesc;
                     h["SlotId"] = g.SlotId;

                     return h;
                 }).CopyToDataTable();


            if (CartTable.Rows.Count > 0)
            {
                ViewState["CartRow"] = CartTable;

                gvBoatdtl.Visible = true;
                gvBoatdtl.DataSource = CartTable;
                gvBoatdtl.DataBind();
            }
            else
            {
                gvBoatdtl.Visible = false;
                gvBoatdtl.DataBind();
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void GrvEditSlot_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList DropDownList1 = (e.Row.FindControl("ddlSlot") as DropDownList);
            DataTable dt = (DataTable)ViewState["AvailSlot"];

            DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "SlotStartTime";
            DropDownList1.DataValueField = "SlotId";

            DropDownList1.DataBind();
            DropDownList1.SelectedValue = ViewState["SlotIdOld"].ToString();

        }

    }
    public void BindChangeSlotGrid(string BoatType, string BoatTypeId, string SeaterType, string SeaterTypeId, string BoatCount,
        string BlockId, string SlotId, string SlotDesc, string Status)
    {
        try
        {

            if (ViewState["CartRow1"] != null)
            {
                DataTable mt = (DataTable)ViewState["CartRow1"];

                if (mt.Rows.Count > 0)
                {
                    DataRow[] result = mt.Select("BoatTypeId = '" + BoatTypeId.Trim() + "' AND SeaterTypeId ='" + SeaterTypeId.Trim() + "' ");
                }
            }


            // Bind an Value in Temp DataTable

            DataTable mytable = new DataTable();

            if (ViewState["Row1"] != null)
            {
                mytable = (DataTable)ViewState["Row1"];
                DataRow dr = null;

                string Value = ViewState["UpdateUniqueId"].ToString();

                DataRow[] fndUniqueId = mytable.Select("UniqueId = '" + Value.Trim() + "'");
                if (mytable.Rows.Count > 0)
                {
                    dr = mytable.NewRow();

                    dr["UniqueId"] = ViewState["UpdateUniqueId"];
                    dr["BoatType"] = BoatType.Trim(); ;
                    dr["BoatTypeId"] = BoatTypeId.Trim();
                    dr["Status"] = Status.Trim();

                    dr["SeaterType"] = SeaterType.Trim();
                    dr["SeaterTypeId"] = SeaterTypeId.Trim();

                    dr["BoatCount"] = BoatCount.Trim();

                    dr["SlotId"] = SlotId.Trim();
                    dr["SlotDesc"] = SlotDesc.Trim();

                    dr["BlockId"] = BlockId.Trim();
                    mytable.Rows.Add(dr);

                    ViewState["Row1"] = mytable;
                }
            }
            else
            {
                mytable.Columns.Add(new DataColumn("UniqueId", typeof(string)));
                mytable.Columns.Add(new DataColumn("BoatType", typeof(string)));
                mytable.Columns.Add(new DataColumn("BoatTypeId", typeof(string)));

                mytable.Columns.Add(new DataColumn("SeaterType", typeof(string)));
                mytable.Columns.Add(new DataColumn("SeaterTypeId", typeof(string)));

                mytable.Columns.Add(new DataColumn("BoatCount", typeof(Int32)));

                mytable.Columns.Add(new DataColumn("SlotDesc", typeof(string)));
                mytable.Columns.Add(new DataColumn("SlotId", typeof(string)));
                mytable.Columns.Add(new DataColumn("BlockId", typeof(string)));
                mytable.Columns.Add(new DataColumn("Status", typeof(string)));

                DataRow dr1 = mytable.NewRow();
                dr1 = mytable.NewRow();

                dr1["UniqueId"] = ViewState["UpdateUniqueId"];
                dr1["BoatType"] = BoatType.Trim();
                dr1["BoatTypeId"] = BoatTypeId.Trim();
                dr1["Status"] = Status.Trim();
                dr1["SeaterType"] = SeaterType.Trim();
                dr1["SeaterTypeId"] = SeaterTypeId.Trim();

                dr1["BoatCount"] = BoatCount.Trim();

                dr1["SlotDesc"] = SlotDesc.Trim();
                dr1["SlotId"] = SlotId.Trim();
                dr1["BlockId"] = BlockId.Trim();

                mytable.Rows.Add(dr1);

                ViewState["Row1"] = mytable;
            }

            DataTable dts = mytable.Clone();

            var CartTable = (from row in mytable.AsEnumerable()
                             group row by new
                             {
                                 UniqueId = row.Field<string>("UniqueId"),
                                 BoatType = row.Field<string>("BoatType"),
                                 BoatTypeId = row.Field<string>("BoatTypeId"),
                                 SeaterType = row.Field<string>("SeaterType"),
                                 Status = row.Field<string>("Status"),
                                 SeaterTypeId = row.Field<string>("SeaterTypeId"),

                                 SlotDesc = row.Field<string>("SlotDesc"),
                                 SlotId = row.Field<string>("SlotId"),
                                 BlockId = row.Field<string>("BlockId"),

                             } into t1
                             select new
                             {
                                 UniqueID = t1.Key.UniqueId,
                                 BoatType = t1.Key.BoatType,
                                 BoatTypeId = t1.Key.BoatTypeId,
                                 Status = t1.Key.Status,
                                 SeaterType = t1.Key.SeaterType,
                                 SeaterTypeId = t1.Key.SeaterTypeId,

                                 BoatCount = t1.Sum(a => a.Field<Int32>("BoatCount")),

                                 SlotDesc = t1.Key.SlotDesc,
                                 SlotId = t1.Key.SlotId,
                                 BlockId = t1.Key.BlockId
                             })
                 .Select(g =>
                 {
                     var h = dts.NewRow();
                     h["UniqueId"] = g.UniqueID;
                     h["BoatType"] = g.BoatType;
                     h["BoatTypeId"] = g.BoatTypeId;
                     h["Status"] = g.Status;

                     h["SeaterType"] = g.SeaterType;
                     h["SeaterTypeId"] = g.SeaterTypeId;

                     h["BoatCount"] = g.BoatCount;

                     h["SlotDesc"] = g.SlotDesc;
                     h["SlotId"] = g.SlotId;
                     h["BlockId"] = g.BlockId;

                     return h;
                 }).CopyToDataTable();


            if (mytable.Rows.Count > 0)
            {
                ViewState["CartRow1"] = mytable;
                //gvBoatdtl.Visible = true;
                GrvEditSlot.DataSource = mytable;
                GrvEditSlot.DataBind();
            }
            else
            {
                //GrvEditSlot.Visible = false;
                GrvEditSlot.DataBind();
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void lblSlot_Click(object sender, EventArgs e)
    {
        MpeUpdateSlot.Show();
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        //string BlockId = gvBoatdtl.DataKeys[gvrow.RowIndex]["BlockId"].ToString().Trim();

        ViewState["UpdateUniqueId"] = gvBoatdtl.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
        ViewState["LnkBlockId"] = "";
        DataTable BlockDt = (DataTable)ViewState["Row"];

        for (int bk = 0; bk < BlockDt.Rows.Count; bk++)
        {
            if (ViewState["UpdateUniqueId"].ToString().Trim() == BlockDt.Rows[bk]["UniqueId"].ToString().Trim())
            {
                ViewState["LnkBlockId"] += BlockDt.Rows[bk]["BlockId"].ToString().Trim() + ',';

            }

        }
        string Status = gvBoatdtl.DataKeys[gvrow.RowIndex]["Status"].ToString().Trim();
        string sStatus = string.Empty;
        if (Status.Trim() == "Normal")
        {
            sStatus = "N";
        }
        if (Status.Trim() == "Express")
        {
            sStatus = "P";
        }
        if (Status.Trim() == "Individual")
        {
            sStatus = "I";
        }
        string BoatTypeId = gvBoatdtl.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim();
        string BoatSeaterId = gvBoatdtl.DataKeys[gvrow.RowIndex]["SeaterTypeId"].ToString().Trim();
        string sSlotId = gvBoatdtl.DataKeys[gvrow.RowIndex]["SlotId"].ToString().Trim();
        string sSlotDesc = gvBoatdtl.DataKeys[gvrow.RowIndex]["SlotDesc"].ToString().Trim();

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var AvailableBoatBookingSlotDept = new AvailableBoatBookingSlotDept()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    CheckInDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    BoatTypeId = BoatTypeId,
                    BoatSeaterId = BoatSeaterId,
                    SlotType = sStatus,
                    SlotIdold = sSlotId.Trim(),
                    BlockId = ViewState["LnkBlockId"].ToString().Trim(),
                    UserId = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("GetBlockIdToUpdateDept", AvailableBoatBookingSlotDept).Result;

                if (response.IsSuccessStatusCode)
                {

                    var Slto = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Slto)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Slto)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ViewState["TempBlockDetails"] = dt;
                            string CStatus = string.Empty;
                            if (Status.Trim() == "Normal")
                            {
                                CStatus = "N";
                            }
                            if (Status.Trim() == "Express")
                            {
                                CStatus = "P";
                            }
                            if (Status.Trim() == "Individual")
                            {
                                CStatus = "I";
                            }

                            //using (var client1 = new HttpClient())
                            //{
                            //    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Clear();

                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var AvailableBoatBookingSlotDept1 = new AvailableBoatBookingSlotDept()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString(),
                                CheckInDate = DateTime.Today.ToString("dd/MM/yyyy"),
                                BoatTypeId = BoatTypeId,
                                BoatSeaterId = BoatSeaterId,
                                SlotType = CStatus,
                                UserId = Session["UserId"].ToString().Trim()
                            };

                            HttpResponseMessage response1 = client.PostAsJsonAsync("ChangeAvailbleBoatSlotTime", AvailableBoatBookingSlotDept1).Result;

                            if (response1.IsSuccessStatusCode)
                            {
                                var Slto1 = response1.Content.ReadAsStringAsync().Result;
                                int StatusCode1 = Convert.ToInt32(JObject.Parse(Slto1)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(Slto1)["Response"].ToString();

                                if (StatusCode1 == 1)
                                {
                                    DataTable dt0 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                                    if (dt0.Rows.Count > 0)
                                    {
                                        ViewState["AvailSlot"] = dt0;
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
                                        return;
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
                            // }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
                            return;
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
        DataTable dt1 = (DataTable)ViewState["TempBlockDetails"];
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            string BoatTypeName = dt1.Rows[i]["BoatTypeName"].ToString().Trim();
            string BoatTypeid = dt1.Rows[i]["BoatTypeId"].ToString().Trim();
            string BoatSeaterType = dt1.Rows[i]["BoatSeaterName"].ToString().Trim();
            string BoatSeaterid = dt1.Rows[i]["BoatSeaterId"].ToString().Trim();
            string BoatCount = dt1.Rows[i]["TotalTripCount"].ToString().Trim();
            string BlockId = dt1.Rows[i]["BlockId"].ToString().Trim();
            //  string Status = dt1.Rows[i]["Status"].ToString().Trim();
            ViewState["SlotIdOld"] = dt1.Rows[i]["SlotId"].ToString().Trim();
            ViewState["SlotIdDesc"] = dt1.Rows[i]["SlotStartTime"].ToString().Trim();

            BindChangeSlotGrid(BoatTypeName, BoatTypeid, BoatSeaterType, BoatSeaterid, BoatCount, BlockId,
                ViewState["SlotIdOld"].ToString(), ViewState["SlotIdDesc"].ToString(), "");

        }
    }

    //protected void lblSlot_Click(object sender, EventArgs e)
    //{
    //    MpeUpdateSlot.Show();
    //    LinkButton lnkbtn = sender as LinkButton;
    //    GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
    //    //string BlockId = gvBoatdtl.DataKeys[gvrow.RowIndex]["BlockId"].ToString().Trim();

    //    ViewState["UpdateUniqueId"] = gvBoatdtl.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
    //    ViewState["LnkBlockId"] = "";
    //    DataTable BlockDt = (DataTable)ViewState["Row"];

    //    for (int bk = 0; bk < BlockDt.Rows.Count; bk++)
    //    {
    //        if (ViewState["UpdateUniqueId"].ToString().Trim() == BlockDt.Rows[bk]["UniqueId"].ToString().Trim())
    //        {
    //            ViewState["LnkBlockId"] += BlockDt.Rows[bk]["BlockId"].ToString().Trim() + ',';

    //        }

    //    }
    //    string Status = gvBoatdtl.DataKeys[gvrow.RowIndex]["Status"].ToString().Trim();
    //    string sStatus = string.Empty;
    //    if (Status.Trim() == "Normal")
    //    {
    //        sStatus = "N";
    //    }
    //    if (Status.Trim() == "Express")
    //    {
    //        sStatus = "P";
    //    }
    //    if (Status.Trim() == "Individual")
    //    {
    //        sStatus = "I";
    //    }
    //    string BoatTypeId = gvBoatdtl.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim();
    //    string BoatSeaterId = gvBoatdtl.DataKeys[gvrow.RowIndex]["SeaterTypeId"].ToString().Trim();
    //    string sSlotId = gvBoatdtl.DataKeys[gvrow.RowIndex]["SlotId"].ToString().Trim();
    //    string sSlotDesc = gvBoatdtl.DataKeys[gvrow.RowIndex]["SlotDesc"].ToString().Trim();

    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            var AvailableBoatBookingSlotDept = new AvailableBoatBookingSlotDept()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString(),
    //                CheckInDate = DateTime.Today.ToString("dd/MM/yyyy"),
    //                BoatTypeId = BoatTypeId,
    //                BoatSeaterId = BoatSeaterId,
    //                SlotType = sStatus,
    //                SlotIdold = sSlotId.Trim(),
    //                BlockId = ViewState["LnkBlockId"].ToString().Trim(),
    //                UserId = Session["UserId"].ToString().Trim()
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("GetBlockIdToUpdateDept", AvailableBoatBookingSlotDept).Result;
    //            if (Status.Trim() == "Normal")
    //            {
    //                sStatus = "N";
    //            }
    //            if (Status.Trim() == "Express")
    //            {
    //                sStatus = "P";
    //            }
    //            if (Status.Trim() == "Individual")
    //            {
    //                sStatus = "N";
    //            }
    //            if (response.IsSuccessStatusCode)
    //            {

    //                var Slto = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(Slto)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(Slto)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        ViewState["TempBlockDetails"] = dt;

    //                        //using (var client1 = new HttpClient())
    //                        //{
    //                        //    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //                        client.DefaultRequestHeaders.Clear();
    //                        client.DefaultRequestHeaders.Accept.Clear();

    //                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //                        var AvailableBoatBookingSlotDept1 = new AvailableBoatBookingSlotDept()
    //                        {
    //                            BoatHouseId = Session["BoatHouseId"].ToString(),
    //                            CheckInDate = DateTime.Today.ToString("dd/MM/yyyy"),
    //                            BoatTypeId = BoatTypeId,
    //                            BoatSeaterId = BoatSeaterId,
    //                            SlotType = sStatus,
    //                            UserId = Session["UserId"].ToString().Trim()
    //                        };

    //                        HttpResponseMessage response1 = client.PostAsJsonAsync("ChangeAvailbleBoatSlotTime", AvailableBoatBookingSlotDept1).Result;

    //                        if (response1.IsSuccessStatusCode)
    //                        {
    //                            var Slto1 = response1.Content.ReadAsStringAsync().Result;
    //                            int StatusCode1 = Convert.ToInt32(JObject.Parse(Slto1)["StatusCode"].ToString());
    //                            string ResponseMsg1 = JObject.Parse(Slto1)["Response"].ToString();

    //                            if (StatusCode1 == 1)
    //                            {
    //                                DataTable dt0 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

    //                                if (dt0.Rows.Count > 0)
    //                                {
    //                                    ViewState["AvailSlot"] = dt0;
    //                                }
    //                                else
    //                                {
    //                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
    //                                    return;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
    //                        }
    //                        // }
    //                    }
    //                    else
    //                    {
    //                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
    //                }
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //    DataTable dt1 = (DataTable)ViewState["TempBlockDetails"];
    //    for (int i = 0; i < dt1.Rows.Count; i++)
    //    {
    //        string BoatTypeName = dt1.Rows[i]["BoatTypeName"].ToString().Trim();
    //        string BoatTypeid = dt1.Rows[i]["BoatTypeId"].ToString().Trim();
    //        string BoatSeaterType = dt1.Rows[i]["BoatSeaterName"].ToString().Trim();
    //        string BoatSeaterid = dt1.Rows[i]["BoatSeaterId"].ToString().Trim();
    //        string BoatCount = dt1.Rows[i]["TotalTripCount"].ToString().Trim();
    //        string BlockId = dt1.Rows[i]["BlockId"].ToString().Trim();
    //        //  string Status = dt1.Rows[i]["Status"].ToString().Trim();
    //        ViewState["SlotIdOld"] = dt1.Rows[i]["SlotId"].ToString().Trim();
    //        ViewState["SlotIdDesc"] = dt1.Rows[i]["SlotStartTime"].ToString().Trim();

    //        BindChangeSlotGrid(BoatTypeName, BoatTypeid, BoatSeaterType, BoatSeaterid, BoatCount, BlockId,
    //            ViewState["SlotIdOld"].ToString(), ViewState["SlotIdDesc"].ToString(), "");
    //    }
    //}

    protected void BtnUpdateSlotTime_Click(object sender, EventArgs e)
    {
        ViewState["LnBlockId"] = null;
        foreach (GridViewRow gvRow in GrvEditSlot.Rows)
        {
            Label blckId = (Label)gvRow.FindControl("lbBlockId");
            ViewState["LnBlockId"] += blckId.Text + ",";

            DropDownList ddlSlotValues = (DropDownList)gvRow.FindControl("ddlSlot");

            ViewState["ChngdSlotId"] += ddlSlotValues.SelectedValue + "~";
            ViewState["ChngdSlotDesc"] += ddlSlotValues.SelectedItem.Text + "~";
        }
        try
        {
            string SlotId;
            string[] sSlotId;
            string SlotDesc;
            string[] sSlotDesc;
            string BlockIds;
            string[] sBlockId;

            SlotId = ViewState["ChngdSlotId"].ToString();
            sSlotId = SlotId.Split('~');
            SlotDesc = ViewState["ChngdSlotDesc"].ToString();
            sSlotDesc = SlotDesc.Split('~');
            BlockIds = ViewState["LnBlockId"].ToString();
            sBlockId = BlockIds.Split(',');

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var AvailableBoatBookingSlotDept = new Update()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    SlotIds = sSlotId,
                    BookingBlockId = sBlockId,
                    BookingUser = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("UpdateTmpBookedSlotDeptNew", AvailableBoatBookingSlotDept).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Slto = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Slto)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Slto)["Response"].ToString();

                    if (StatusCode == 1)
                    {

                        DataTable dtCurrentTable1 = (DataTable)ViewState["Row"];

                        for (int s = 0; s < sBlockId.Count(); s++)
                        {
                            for (int J = 0; J < dtCurrentTable1.Rows.Count; J++)
                            {
                                DataRow dr = dtCurrentTable1.Rows[J];
                                if (dtCurrentTable1.Rows[J]["BlockId"].ToString().Trim() == sBlockId[s].ToString().Trim())
                                {
                                    dtCurrentTable1.Rows[J]["SlotId"] = sSlotId[s];
                                    dtCurrentTable1.Rows[J]["SlotDesc"] = sSlotDesc[s];
                                    string UniqueIds = dtCurrentTable1.Rows[J]["BoatTypeId"] + "~" + dtCurrentTable1.Rows[J]["SeaterTypeId"] + "~" + dtCurrentTable1.Rows[J]["SlotId"];
                                    dtCurrentTable1.Rows[J]["UniqueId"] = UniqueIds;

                                }
                            }
                        }

                        dtCurrentTable1.AcceptChanges();

                        DataTable dt2 = (DataTable)dtCurrentTable1;

                        DataTable dts = dt2.Clone();

                        var UpdatedTable = (from row in dt2.AsEnumerable()
                                            group row by new
                                            {
                                                UniqueId = row.Field<string>("UniqueId"),
                                                BoatType = row.Field<string>("BoatType"),
                                                BoatTypeId = row.Field<string>("BoatTypeId"),

                                                SeaterType = row.Field<string>("SeaterType"),
                                                SeaterTypeId = row.Field<string>("SeaterTypeId"),
                                                Status = row.Field<string>("Status"),
                                                //BoatCount = row.Field<Int32>("BoatCount"),
                                                //BoatTotalCharge = row.Field<decimal>("BoatTotalCharge"),

                                                BoatMinCharge = row.Field<string>("BoatMinCharge"),
                                                RowerMinCharge = row.Field<string>("RowerMinCharge"),
                                                BoatTaxCharge = row.Field<string>("BoatTaxCharge"),

                                                DepositType = row.Field<string>("DepositType"),
                                                Deposit = row.Field<decimal>("Deposit"),
                                                BoatMinTime = row.Field<string>("BoatMinTime"),
                                                SlotDesc = row.Field<string>("SlotDesc"),
                                                SlotId = row.Field<string>("SlotId"),

                                            } into t1
                                            select new
                                            {
                                                UniqueID = t1.Key.UniqueId,
                                                BoatType = t1.Key.BoatType,
                                                BoatTypeId = t1.Key.BoatTypeId,

                                                SeaterType = t1.Key.SeaterType,
                                                SeaterTypeId = t1.Key.SeaterTypeId,
                                                Status = t1.Key.Status,

                                                BoatCount = t1.Sum(a => a.Field<Int32>("BoatCount")),
                                                BoatTotalCharge = t1.Sum(a => a.Field<decimal>("BoatTotalCharge")) + t1.Sum(b => b.Field<decimal>("Deposit")),

                                                BoatMinCharge = t1.Key.BoatMinCharge,
                                                RowerMinCharge = t1.Key.RowerMinCharge,
                                                BoatTaxCharge = t1.Key.BoatTaxCharge,

                                                DepositType = t1.Key.DepositType,
                                                Deposit = t1.Key.Deposit,
                                                BoatMinTime = t1.Key.BoatMinTime,
                                                SlotDesc = t1.Key.SlotDesc,
                                                SlotId = t1.Key.SlotId,

                                            })
                             .Select(g =>
                             {
                                 var h = dts.NewRow();
                                 h["UniqueId"] = g.UniqueID;
                                 h["BoatType"] = g.BoatType;
                                 h["BoatTypeId"] = g.BoatTypeId;

                                 h["SeaterType"] = g.SeaterType;
                                 h["SeaterTypeId"] = g.SeaterTypeId;
                                 h["Status"] = g.Status;

                                 h["BoatCount"] = g.BoatCount;
                                 h["BoatTotalCharge"] = g.BoatTotalCharge;

                                 h["BoatMinCharge"] = g.BoatMinCharge;
                                 h["RowerMinCharge"] = g.RowerMinCharge;
                                 h["BoatTaxCharge"] = g.BoatTaxCharge;

                                 h["DepositType"] = g.DepositType;
                                 h["Deposit"] = g.Deposit;
                                 h["BoatMinTime"] = g.BoatMinTime;
                                 h["SlotDesc"] = g.SlotDesc;
                                 h["SlotId"] = g.SlotId;


                                 return h;
                             }).CopyToDataTable();


                        if (UpdatedTable.Rows.Count > 0)
                        {
                            ViewState["CartRow"] = UpdatedTable;
                            gvBoatdtl.Visible = true;
                            gvBoatdtl.DataSource = UpdatedTable;
                            gvBoatdtl.DataBind();
                        }
                        else
                        {
                            gvBoatdtl.Visible = false;
                            gvBoatdtl.DataBind();
                        }

                        if (dtCurrentTable1.Rows.Count > 0)
                        {
                            DataTable dtTable = (DataTable)ViewState["Row"];

                            decimal dBoatMinCharge = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatMinCharge")));
                            decimal dRowerMinCharge = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("RowerMinCharge")));
                            //bschar1.InnerText = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();
                            ViewState["BoatChargeSum"] = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();


                            decimal dGSTAmount = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatTaxCharge")));
                            //bsgst1.InnerText = dGSTAmount.ToString();
                            ViewState["BoatTaxSum"] = dGSTAmount.ToString();

                            decimal dDeposit = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Deposit")));
                            //bsdeposit1.InnerText = dDeposit.ToString();
                            ViewState["BoatDepositSum"] = dDeposit.ToString();

                            decimal dTotal = Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge) + Convert.ToDecimal(dGSTAmount) + Convert.ToDecimal(dDeposit);
                            bsTotal.InnerText = dTotal.ToString();
                            ViewState["BoatTotalSum"] = dTotal.ToString();
                        }
                        else
                        {
                            gvBoatdtl.DataBind();

                            ViewState["CartRow1"] = null;
                            ViewState["Row1"] = null;
                            gvBoatdtl.Visible = false;

                            bschar1.InnerText = "";
                            bsgst1.InnerText = "";
                            bsdeposit1.InnerText = "";
                            bsTotal.InnerText = "";
                            // btnBoatBooking.Text = "";
                            GrvEditSlot.DataSource = null;
                            txtPIN.Text = "";
                        }

                        CalculateSummary();

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
                        dvContent.Attributes.Add("style", "display:none;");
                        ViewState["SlotIdOld"] = "";
                        ViewState["SlotDescOld"] = "";
                        ViewState["UpdateUniqueId"] = "";
                        ViewState["ChngdSlotId"] = "";
                        ViewState["ChngdSlotDesc"] = "";
                        ViewState["CartRow1"] = null;
                        ViewState["Row1"] = null;
                        ViewState["LnBlockId"] = null;
                        GrvEditSlot.DataSource = null;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        ViewState["SlotIdOld"] = "";
                        ViewState["SlotDescOld"] = "";
                        ViewState["UpdateUniqueId"] = "";
                        ViewState["ChngdSlotId"] = "";
                        ViewState["ChngdSlotDesc"] = "";
                        ViewState["CartRow1"] = null;
                        ViewState["Row1"] = null;
                        ViewState["LnBlockId"] = null;
                        GrvEditSlot.DataSource = null;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                    ViewState["SlotIdOld"] = "";
                    ViewState["SlotDescOld"] = "";
                    ViewState["UpdateUniqueId"] = "";
                    ViewState["ChngdSlotId"] = "";
                    ViewState["ChngdSlotDesc"] = "";
                    ViewState["CartRow1"] = null;
                    ViewState["Row1"] = null;
                    ViewState["LnBlockId"] = null;
                    GrvEditSlot.DataSource = null;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            ViewState["SlotIdOld"] = "";
            ViewState["SlotDescOld"] = "";
            ViewState["UpdateUniqueId"] = "";
            ViewState["ChngdSlotId"] = "";
            ViewState["ChngdSlotDesc"] = "";
            ViewState["CartRow1"] = null;
            ViewState["Row1"] = null;
            GrvEditSlot.DataSource = null;
            return;
        }



    }
    protected void btnCancelSlot_Click(object sender, EventArgs e)
    {
        MpeUpdateSlot.Hide();
        ViewState["SlotIdOld"] = "";
        ViewState["SlotDescOld"] = "";
        ViewState["UpdateUniqueId"] = "";
        ViewState["ChngdSlotId"] = "";
        ViewState["ChngdSlotDesc"] = "";
        ViewState["CartRow1"] = null;
        ViewState["Row1"] = null;
        ViewState["LnBlockId"] = null;
        GrvEditSlot.DataSource = null;

    }

    public void GetTaxDetail()
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
                    ServiceId = "1",
                    ValidDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TaxMstr/IdDate", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Taxd = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Taxd)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Taxd)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ViewState["TaxPercentBoat"] = dt.Rows[0]["TaxName"].ToString();
                        }
                        else
                        {
                            ViewState["TaxPercentBoat"] = string.Empty;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Tax Details Not Found...!');", true);
                            return;
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

    public void GetPaymentType()
    {
        try
        {
            ddlPaymentType.Items.Clear();

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
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Payment Type Details Not Found !');", true);
                        }

                        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Cash"));
                        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Card"));

                        if (Session["KioskPaymentRights"].ToString().Trim() == "N")
                        {
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
                        }
                        else
                        {
                            if (Session["KioskOnlineRights"].ToString().Trim() == "N")
                            {
                                ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                            }

                            if (Session["KioskUPIRights"].ToString().Trim() == "N")
                            {
                                ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                            }
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

    //private void BoatBookingFinal()
    //{
    //    try
    //    {
    //        //--Incase Other service not booking assign default value N//

    //        ViewState["OthServiceStatus"] = "N";

    //        if (Convert.ToDecimal(btnBoatBooking.Text) <= 0)
    //        {
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Select Any One Boat !');", true);
    //            return;
    //        }
    //        else
    //        {
    //            ViewState["BoatChargeTotal"] = "0";
    //            ViewState["RowerChargeTotal"] = "0";
    //            ViewState["BoatDepositTotal"] = "0";
    //            ViewState["BoatTaxTotal"] = "0";
    //            ViewState["boatOfferAmount"] = "0";

    //            foreach (GridViewRow item in gvBoatdtl.Rows)
    //            {
    //                string cBoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
    //                string cSeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();

    //                Label NumberOfBoat = (Label)item.FindControl("lblBoatCount");
    //                int iNumofBoat = Convert.ToInt32(NumberOfBoat.Text.Trim());

    //                //CheckBoatAvailableDetails(Session["BoatHouseId"].ToString().Trim(), cBoatTypeId.Trim(), cSeaterTypeId.Trim(), DateTime.Now.ToString("dd/MM/yyyy"));

    //                //if (hfBoatNature.Value.Trim() == "P")
    //                //{
    //                //    if (iNumofBoat > Convert.ToDecimal(ViewState["cPremiumAvailable"].ToString()))
    //                //    {
    //                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Selected boat trips exceeding the maximum trip limit !!!');", true);
    //                //        return;
    //                //    }
    //                //}

    //                //if (hfBoatNature.Value.Trim() == "N")
    //                //{
    //                //    if (iNumofBoat > Convert.ToDecimal(ViewState["cNormalAvailable"].ToString()))
    //                //    {
    //                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Selected boat trips exceeding the maximum trip limit !!!');", true);
    //                //        return;
    //                //    }
    //                //}

    //                for (int i = 1; i <= iNumofBoat; i++)
    //                {
    //                    string BoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
    //                    ViewState["BoatTypeIds"] += BoatTypeId.Trim() + '~';

    //                    string SeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();
    //                    ViewState["BoatSeaterIds"] += SeaterTypeId.Trim() + '~';

    //                    string BoatMinTime = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinTime"].ToString().Trim();
    //                    ViewState["BoatMinTimes"] += BoatMinTime.Trim() + '~';

    //                    string BoatMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinCharge"].ToString().Trim();
    //                    decimal iBoatMinCharge = Convert.ToDecimal(BoatMinCharge.Trim());
    //                    ViewState["InitBoatCharges"] += BoatMinCharge.Trim() + '~';

    //                    string RowerMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["RowerMinCharge"].ToString().Trim();
    //                    decimal iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
    //                    ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';

    //                    string BoatTaxCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatTaxCharge"].ToString().Trim();
    //                    decimal dBoatTaxCharge = Convert.ToDecimal(BoatTaxCharge.Trim());

    //                    string SlotId = gvBoatdtl.DataKeys[item.RowIndex]["SlotId"].ToString().Trim();
    //                    ViewState["SlotIds"] += SlotId.Trim() + '~';

    //                    //string BlockId = gvBoatdtl.DataKeys[item.RowIndex]["BlockId"].ToString().Trim();
    //                    //ViewState["BlockIds"] += BlockId.Trim() + ',';
    //                    //ViewState["BlockId"] += "664,665,666";

    //                    var lblTax = ViewState["TaxPercentBoat"].ToString();

    //                    decimal BoatTotalTaxAmt = 0;
    //                    string TaxDtl = string.Empty;

    //                    if (lblTax != "")
    //                    {
    //                        string[] taxlist = lblTax.Split(',');

    //                        decimal TaxAmt = dBoatTaxCharge / 2;

    //                        foreach (var list in taxlist)
    //                        {
    //                            var TaxName = list;
    //                            var tx = list.Split('-');

    //                            TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
    //                            BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
    //                        }
    //                    }

    //                    ViewState["TaxAmountDetlBoat"] += Convert.ToString(TaxDtl.ToString() + '~');

    //                    decimal iBoatDeposit = Convert.ToDecimal(gvBoatdtl.DataKeys[item.RowIndex]["Deposit"].ToString().Trim());

    //                    decimal BoatDepositAmount = 0;
    //                    BoatDepositAmount = iBoatDeposit;

    //                    ViewState["BoatDeposits"] += BoatDepositAmount.ToString() + '~';

    //                    ViewState["InitNetAmount"] = (iBoatMinCharge + iRowerMinCharge + BoatDepositAmount + BoatTotalTaxAmt).ToString();
    //                    ViewState["InitNetAmounts"] += ViewState["InitNetAmount"].ToString() + '~';

    //                    // Do whatever you need with that string value here

    //                    ViewState["BoatChargeTotal"] = (Convert.ToDecimal(ViewState["BoatChargeTotal"]) + iBoatMinCharge).ToString();

    //                    ViewState["RowerChargeTotal"] = (Convert.ToDecimal(ViewState["RowerChargeTotal"]) + iRowerMinCharge).ToString();

    //                    ViewState["BoatDepositTotal"] = (Convert.ToDecimal(ViewState["BoatDepositTotal"]) + BoatDepositAmount).ToString();

    //                    ViewState["BoatTaxTotal"] = (Convert.ToDecimal(ViewState["BoatTaxTotal"]) + BoatTotalTaxAmt).ToString();

    //                    ViewState["boatOfferAmount"] += "0" + "~";
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //}

    //private void OtherBookingFinal()
    //{
    //    ViewState["OthServiceStatus"] = "N";
    //    ViewState["OthServiceId"] = "";
    //    ViewState["OthChargePerItem"] = "";
    //    ViewState["OthNoOfItems"] = "";
    //    ViewState["OthTaxDetails"] = "";
    //    ViewState["OthNetAmount"] = "";

    //    try
    //    {
    //        foreach (GridViewRow item in gvOther.Rows)
    //        {
    //            ViewState["OthServiceStatus"] = "Y";

    //            string NoOfChild = string.Empty;
    //            string ChargePerItemTax = string.Empty;
    //            string ChargePerItem = string.Empty;
    //            string AdultCount = string.Empty;

    //            string ServiceId = gvOther.DataKeys[item.RowIndex]["ServiceId"].ToString().Trim();
    //            ViewState["OthServiceId"] += ServiceId.Trim() + '~';

    //            ChargePerItem = gvOther.DataKeys[item.RowIndex]["ChargePerItem"].ToString().Trim();
    //            ViewState["OthChargePerItem"] += ChargePerItem.Trim() + '~';

    //            AdultCount = gvOther.DataKeys[item.RowIndex]["AdultCount"].ToString().Trim();
    //            ViewState["OthNoOfItems"] += AdultCount.Trim() + '~';

    //            ChargePerItemTax = gvOther.DataKeys[item.RowIndex]["ChargePerItemTax"].ToString().Trim();

    //            //------- Tax ------------//   
    //            decimal Totalcharge = (Convert.ToDecimal(ChargePerItem) * Convert.ToDecimal(AdultCount));
    //            string lblTax = gvOther.DataKeys[item.RowIndex]["TaxName"].ToString().Trim();

    //            decimal OtherTaxAmt = Convert.ToDecimal(ChargePerItemTax) * Convert.ToDecimal(AdultCount);

    //            decimal TotalTaxAmt = 0;
    //            string TaxDtl = string.Empty;

    //            if (lblTax != "")
    //            {
    //                string[] taxlist = lblTax.Split(',');

    //                foreach (var list in taxlist)
    //                {
    //                    var TaxName = list;
    //                    var tx = list.Split('-');
    //                    decimal taxper = Convert.ToDecimal(tx[1].ToString());

    //                    decimal TaxAmt = ((OtherTaxAmt) / 2);
    //                    TaxAmt = Math.Round(TaxAmt, 2);

    //                    TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
    //                    TotalTaxAmt = TotalTaxAmt + TaxAmt;
    //                }
    //            }

    //            ViewState["OthTaxDetails"] += TaxDtl + '~';
    //            decimal OtherTotalAmount = Totalcharge + TotalTaxAmt;
    //            ViewState["OthNetAmount"] += OtherTotalAmount.ToString() + '~';
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //}

    //protected void btnBoatBooking_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        btnBoatBooking.Enabled = false;

    //        string ServiceType = "";
    //        string UserId = "";

    //        if (bsTotal.InnerText.Trim() == "")
    //        {
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
    //            return;
    //        }

    //        if (Convert.ToDecimal(btnBoatBooking.Text.Trim()) <= 0)
    //        {
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
    //            return;
    //        }

    //        BoatBookingFinal();
    //        OtherBookingFinal();

    //        if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE" || ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "UPI")
    //        {
    //            if (lblUserMobileNo.Text.Trim() != null)
    //            {
    //                if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE")
    //                {
    //                    ServiceType = "DOnlineBooking";
    //                    UserId = ViewState["CUserId"].ToString().Trim();
    //                }
    //                else
    //                {
    //                    ServiceType = "DUPI";
    //                    UserId = Session["UserId"].ToString().Trim();
    //                }

    //                OnlineBeforeTransactionDetails(ServiceType, UserId);

    //                if (ViewState["TranStatus"].ToString().Trim() == "Y")
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Payment SMS Link Send to Customer Mobile !');", true);

    //                    divBack.Style.Add("background-color", "white");
    //                    ClearBooking();
    //                    hfNatureVisible.Value = "0";
    //                    premimumMsg.Visible = false;
    //                    premimumMsg.InnerText = "";

    //                    hfBoatNature.Value = "N";
    //                    BoatBookedSummaryList();
    //                    dtlOther.Visible = false;
    //                    divBack.Style.Add("background-color", "white");
    //                }

    //                return;
    //            }
    //        }

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            if (ViewState["OthServiceStatus"].ToString() == "N")
    //            {
    //                ViewState["OthServiceId"] = "0";
    //                ViewState["OthChargePerItem"] = "0";
    //                ViewState["OthNoOfItems"] = "0";
    //                ViewState["OthTaxDetails"] = "0";
    //                ViewState["OthNetAmount"] = "0";
    //            }

    //            string GSTNO = string.Empty;
    //            string CollectedAmount = string.Empty;
    //            string BalanceAmount = string.Empty;

    //            if (chkGSTNo.Checked == true)
    //            {
    //                GSTNO = txtINSGSTNO.Text.Trim();
    //            }
    //            else
    //            {
    //                GSTNO = "";
    //            }

    //            if (txtAmountPaid.Text.Trim() != "")
    //            {
    //                CollectedAmount = txtAmountPaid.Text.Trim();
    //            }
    //            else
    //            {
    //                CollectedAmount = "0";
    //            }

    //            if (hfBalanceAmt.Value.Trim() != "")
    //            {
    //                BalanceAmount = hfBalanceAmt.Value.Trim();
    //            }
    //            else
    //            {
    //                BalanceAmount = "0";
    //            }

    //            var BoatBook = new BoatBook()
    //            {
    //                QueryType = "Insert",
    //                BookingId = "0",
    //                BookingDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"),
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),

    //                Bookingpin = txtPIN.Text.Trim(),
    //                CustomerId = ViewState["CUserId"].ToString().Trim(),
    //                CustomerMobileNo = lblUserMobileNo.Text.Trim(),
    //                CustomerName = ViewState["CName"].ToString().Trim(),
    //                CustomerAddress = "",
    //                PremiumStatus = hfBoatNature.Value.Trim(),

    //                NoOfPass = "0",
    //                NoOfChild = "0",
    //                NoOfInfant = "0",
    //                OfferId = "0",

    //                InitBillAmount = bsTotal.InnerText.Trim(),
    //                PaymentType = ddlPaymentType.SelectedValue.Trim(),
    //                ActualBillAmount = bsTotal.InnerText.Trim(), //need what amount this;
    //                Status = "B",
    //                BoatTypeId = ViewState["BoatTypeIds"].ToString().Trim('~'),

    //                BoatSeaterId = ViewState["BoatSeaterIds"].ToString().Trim('~'),
    //                BookingDuration = ViewState["BoatMinTimes"].ToString().Trim('~'),
    //                InitBoatCharge = ViewState["InitBoatCharges"].ToString().Trim('~'),
    //                InitRowerCharge = ViewState["RowerMinCharge"].ToString().Trim('~'),
    //                BoatDeposit = ViewState["BoatDeposits"].ToString().Trim('~'),

    //                TaxDetails = ViewState["TaxAmountDetlBoat"].ToString().Trim('~'),
    //                InitOfferAmount = ViewState["boatOfferAmount"].ToString().Trim('~'),
    //                InitNetAmount = ViewState["InitNetAmounts"].ToString().Trim('~'),
    //                CreatedBy = Session["UserId"].ToString().Trim('~'),

    //                ////Other Service Booking
    //                OthServiceStatus = ViewState["OthServiceStatus"].ToString().Trim('~'),
    //                OthServiceId = ViewState["OthServiceId"].ToString().Trim('~'),
    //                OthChargePerItem = ViewState["OthChargePerItem"].ToString().Trim('~'),
    //                OthNoOfItems = ViewState["OthNoOfItems"].ToString().Trim('~'),
    //                OthTaxDetails = ViewState["OthTaxDetails"].ToString().Trim('~'),
    //                OthNetAmount = ViewState["OthNetAmount"].ToString().Trim('~'),
    //                BookingMedia = "DW",

    //                BFDInitBoatCharge = "",
    //                BFDInitNetAmount = "",
    //                BFDGST = "",
    //                CustomerGSTNo = GSTNO,
    //                CollectedAmount = CollectedAmount.Trim(),
    //                BalanceAmount = BalanceAmount.Trim(),
    //                BookingTimeSlotId = ViewState["SlotIds"].ToString().Trim('~'),
    //                BookingBlockId = ViewState["BlockId"].ToString().Trim(','),

    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("BoatBookingServiceNew", BoatBook).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    string[] sResult = ResponseMsg.Split('~');

    //                    if (ViewState["PINType"].ToString().Trim() == "D")
    //                    {
    //                        //GetBoatTickets(sResult[1].ToString());
    //                        //GetBoatTicketsSummaryReceipt(sResult[1].ToString());
    //                        //GetOtherTickets(sResult[1].ToString());

    //                        if (chkCustMobileNo.Checked == true)
    //                        {
    //                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Boat Booking Successfully !');", true);
    //                        }
    //                        else
    //                        {
    //                            Response.Redirect("PrintBoat.aspx?rt=b&bi=" + sResult[1].ToString() + "");
    //                        }
    //                    }
    //                    else
    //                    {
    //                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Boat Booking Successfully !');", true);
    //                    }

    //                    divBack.Style.Add("background-color", "white");
    //                    ClearBooking();
    //                    hfNatureVisible.Value = "0";
    //                    premimumMsg.Visible = false;
    //                    premimumMsg.InnerText = "";

    //                    hfBoatNature.Value = "N";
    //                    BoatBookedSummaryList();
    //                    dtlOther.Visible = false;
    //                    divBack.Style.Add("background-color", "white");

    //                    txtINSGSTNO.Text = string.Empty;
    //                    divGST.Visible = false;
    //                    chkGSTNo.Checked = false;
    //                }
    //                else
    //                {
    //                    string BookingId = GetBookingIdByPin();

    //                    if (BookingId.Trim() != "")
    //                    {
    //                        Response.Redirect("PrintBoat.aspx?rt=b&bi=" + BookingId.Trim() + "");
    //                        return;
    //                    }

    //                    txtINSGSTNO.Text = string.Empty;
    //                    divGST.Visible = false;
    //                    chkGSTNo.Checked = false;

    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
    //                }
    //            }
    //            else
    //            {
    //                txtINSGSTNO.Text = string.Empty;
    //                divGST.Visible = false;
    //                chkGSTNo.Checked = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        txtINSGSTNO.Text = string.Empty;
    //        divGST.Visible = false;
    //        chkGSTNo.Checked = false;
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //}
    /// <summary>
    /// Modified By Abhinaya
    /// Date 11 Aug 2021
    /// </summary>
    //private void BoatBookingFinal()
    //{
    //    try
    //    {
    //        //--Incase Other service not booking assign default value N//

    //        ViewState["OthServiceStatus"] = "N";

    //        if (Convert.ToDecimal(btnBoatBooking.Text) <= 0)
    //        {
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Select Any One Boat !');", true);
    //            return;
    //        }
    //        else
    //        {
    //            ViewState["BoatChargeTotal"] = "0";
    //            ViewState["RowerChargeTotal"] = "0";
    //            ViewState["BoatDepositTotal"] = "0";
    //            ViewState["BoatTaxTotal"] = "0";
    //            ViewState["boatOfferAmount"] = "0";

    //            foreach (GridViewRow item in gvBoatdtl.Rows)
    //            {
    //                string cBoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
    //                string cSeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();

    //                Label NumberOfBoat = (Label)item.FindControl("lblBoatCount");
    //                int iNumofBoat = Convert.ToInt32(NumberOfBoat.Text.Trim());



    //                for (int i = 1; i <= iNumofBoat; i++)
    //                {
    //                    string BoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
    //                    ViewState["BoatTypeIds"] += BoatTypeId.Trim() + '~';

    //                    string SeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();
    //                    ViewState["BoatSeaterIds"] += SeaterTypeId.Trim() + '~';

    //                    string BoatMinTime = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinTime"].ToString().Trim();
    //                    ViewState["BoatMinTimes"] += BoatMinTime.Trim() + '~';

    //                    string BoatMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinCharge"].ToString().Trim();
    //                    decimal iBoatMinCharge = Convert.ToDecimal(BoatMinCharge.Trim());
    //                    ViewState["InitBoatCharges"] += BoatMinCharge.Trim() + '~';

    //                    string RowerMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["RowerMinCharge"].ToString().Trim();
    //                    decimal iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
    //                    ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';

    //                    string BoatTaxCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatTaxCharge"].ToString().Trim();
    //                    decimal dBoatTaxCharge = Convert.ToDecimal(BoatTaxCharge.Trim());

    //                    string SlotId = gvBoatdtl.DataKeys[item.RowIndex]["SlotId"].ToString().Trim();
    //                    ViewState["SlotIds"] += SlotId.Trim() + '~';

    //                    string Status = gvBoatdtl.DataKeys[item.RowIndex]["Status"].ToString().Trim();
    //                    if (Status == "Normal")
    //                    {
    //                        Status = "N";
    //                    }
    //                    else if (Status == "Express")
    //                    {
    //                        Status = "P";
    //                    }
    //                    else
    //                    {
    //                        Status = "I";
    //                    }


    //                    ViewState["Status"] += Status.Trim() + '~';

    //                    int Count = i;

    //                    ViewState["Count"] += Count.ToString().Trim() + '~';

    //                    //string BlockId = gvBoatdtl.DataKeys[item.RowIndex]["BlockId"].ToString().Trim();
    //                    //ViewState["BlockIds"] += BlockId.Trim() + ',';
    //                    //ViewState["BlockId"] += "664,665,666";

    //                    var lblTax = ViewState["TaxPercentBoat"].ToString();

    //                    decimal BoatTotalTaxAmt = 0;
    //                    string TaxDtl = string.Empty;
    //                    string TaxAmount = string.Empty;


    //                    if (lblTax != "")
    //                    {
    //                        string[] taxlist = lblTax.Split(',');

    //                        decimal TaxAmt = dBoatTaxCharge / 2;

    //                        foreach (var list in taxlist)
    //                        {
    //                            var TaxName = list;
    //                            var tx = list.Split('-');
    //                            TaxDtl += (TaxName + "#").Trim();
    //                            TaxAmount += (TaxAmt + "#").Trim();

    //                            BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
    //                        }
    //                    }
    //                    string SAmountArray;
    //                    string[] AmountArray;

    //                    ViewState["sGST"] += TaxDtl.ToString() + '~';
    //                    ViewState["sGSTAmount"] = TaxAmount.ToString();

    //                    SAmountArray = ViewState["sGSTAmount"].ToString();
    //                    AmountArray = SAmountArray.Split('#');

    //                    ViewState["CGSTTaxAmount"] += AmountArray[0].ToString() + '~';
    //                    ViewState["SGSTTaxAmount"] += AmountArray[1].ToString() + '~';

    //                    ViewState["TaxAmountDetlBoat"] += Convert.ToString(TaxDtl.ToString() + '~');


    //                    ViewState["UserId"] = Session["UserId"].ToString();
    //                    ViewState["sUserId"] += ViewState["UserId"].ToString() + '~';

    //                    decimal iBoatDeposit = Convert.ToDecimal(gvBoatdtl.DataKeys[item.RowIndex]["Deposit"].ToString().Trim());

    //                    decimal BoatDepositAmount = 0;
    //                    BoatDepositAmount = iBoatDeposit;

    //                    ViewState["BoatDeposits"] += BoatDepositAmount.ToString() + '~';

    //                    ViewState["InitNetAmount"] = (iBoatMinCharge + iRowerMinCharge + BoatDepositAmount + BoatTotalTaxAmt).ToString();
    //                    ViewState["InitNetAmounts"] += ViewState["InitNetAmount"].ToString() + '~';

    //                    // Do whatever you need with that string value here

    //                    ViewState["BoatChargeTotal"] = (Convert.ToDecimal(ViewState["BoatChargeTotal"]) + iBoatMinCharge).ToString();

    //                    ViewState["RowerChargeTotal"] = (Convert.ToDecimal(ViewState["RowerChargeTotal"]) + iRowerMinCharge).ToString();

    //                    ViewState["BoatDepositTotal"] = (Convert.ToDecimal(ViewState["BoatDepositTotal"]) + BoatDepositAmount).ToString();

    //                    ViewState["BoatTaxTotal"] = (Convert.ToDecimal(ViewState["BoatTaxTotal"]) + BoatTotalTaxAmt).ToString();

    //                    ViewState["boatOfferAmount"] += "0" + "~";

    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //}
    /// <summary>
    /// Modified By Abhinaya 
    /// Date 11 Aug 2021
    /// Modified By Abhinaya 
    /// Modified Date: 06 Sep 2021
    /// </summary>
    private void OtherBookingFinal()
    {
        ViewState["OthServiceStatus"] = "";
        ViewState["OthServiceId"] = "";
        ViewState["OthChargePerItem"] = "";
        ViewState["OthNoOfItems"] = "";
        ViewState["OthTaxDetails"] = "";
        ViewState["CGSTOthTaxAmount"] = "";
        ViewState["SGSTOthTaxAmount"] = "";
        ViewState["OthNetAmount"] = "";
        ViewState["sOthGSTAmount"] = "";

        try
        {

            foreach (GridViewRow item in gvOther.Rows)
            {

                string Status = "Y";
                ViewState["OthServiceStatus"] += Status.ToString().Trim() + '~';

                string NoOfChild = string.Empty;
                string ChargePerItemTax = string.Empty;
                string ChargePerItem = string.Empty;
                string AdultCount = string.Empty;

                string ServiceId = gvOther.DataKeys[item.RowIndex]["ServiceId"].ToString().Trim();
                ViewState["OthServiceId"] += ServiceId.Trim() + '~';


                ChargePerItem = gvOther.DataKeys[item.RowIndex]["ChargePerItem"].ToString().Trim();
                ViewState["OthChargePerItem"] += ChargePerItem.Trim() + '~';

                AdultCount = gvOther.DataKeys[item.RowIndex]["AdultCount"].ToString().Trim();
                ViewState["OthNoOfItems"] += AdultCount.Trim() + '~';

                ChargePerItemTax = gvOther.DataKeys[item.RowIndex]["ChargePerItemTax"].ToString().Trim();

                //------- Tax ------------//   
                decimal Totalcharge = (Convert.ToDecimal(ChargePerItem) * Convert.ToDecimal(AdultCount));
                string lblTax = gvOther.DataKeys[item.RowIndex]["TaxName"].ToString().Trim();

                decimal OtherTaxAmt = Convert.ToDecimal(ChargePerItemTax) * Convert.ToDecimal(AdultCount);

                decimal TotalTaxAmt = 0;
                string TaxDtl = string.Empty;
                string TaxAmount = string.Empty;

                if (lblTax != "")
                {
                    string[] taxlist = lblTax.Split(',');

                    foreach (var list in taxlist)
                    {
                        var TaxName = list;
                        var tx = list.Split('-');
                        decimal taxper = Convert.ToDecimal(tx[1].ToString());

                        decimal TaxAmt = ((OtherTaxAmt) / 2);
                        TaxAmt = Math.Round(TaxAmt, 2);
                        TaxDtl += (TaxName + "#").Trim();
                        TaxAmount += (TaxAmt + "#").Trim();
                        // TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                        TotalTaxAmt = TotalTaxAmt + TaxAmt;
                    }
                }
                ViewState["sOthGSTAmount"] = TaxAmount.ToString();
                string SOthAmountArray;
                string[] OthAmountArray;

                ViewState["sOthGST"] += TaxDtl.ToString() + '~';


                SOthAmountArray = ViewState["sOthGSTAmount"].ToString();
                OthAmountArray = SOthAmountArray.Split('#');
                ViewState["CGSTOthTaxAmount"] += OthAmountArray[0].ToString() + '~';
                ViewState["SGSTOthTaxAmount"] += OthAmountArray[1].ToString() + '~';

                ViewState["OthTaxDetails"] += TaxDtl + '~';
                decimal OtherTotalAmount = Totalcharge + TotalTaxAmt;
                ViewState["OthNetAmount"] += OtherTotalAmount.ToString() + '~';
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
        try
        {
            string Available;
            string[] savailable;
            string Count;
            string[] sCount;
            string Adult;
            string[] sAdult;

            Adult = ViewState["OthNoOfItems"].ToString();
            sAdult = Adult.Split('~');
            Count = ViewState["Count"].ToString();
            sCount = Count.Split('~');
            Available = ViewState["OthServiceStatus"].ToString();
            savailable = Available.Split('~');

            if (savailable[0] == "Y")
            {
                if (sAdult.Count() == sCount.Count())
                {
                }


                if (sAdult.Count() > sCount.Count())
                {


                    for (int i = gvBoatdtl.Rows.Count; i < gvOther.Rows.Count; i++)
                    {
                        string Counts;
                        string[] sCounts;
                        Counts = ViewState["Count"].ToString();
                        sCounts = Counts.Split('~');

                        int Countss = i;
                        ViewState["Count"] += Countss.ToString().Trim() + '~';


                        string sGST = "";
                        ViewState["sGST"] += sGST.ToString().Trim() + '~';
                        string CGSTTaxAmount = "";
                        ViewState["CGSTTaxAmount"] += CGSTTaxAmount.ToString() + '~';
                        string SGSTTaxAmount = "";
                        ViewState["SGSTTaxAmount"] += SGSTTaxAmount.ToString() + '~';
                        //string sGSTAmount = "";
                        //ViewState["sGSTAmount"] += sGSTAmount.ToString().Trim() + '~';
                        string BoatDeposits = "";
                        ViewState["BoatDeposits"] += BoatDeposits.ToString().Trim() + '~';
                        string BoatTypeIds = "0";
                        ViewState["BoatTypeIds"] += BoatTypeIds.ToString().Trim() + '~';
                        string BoatMinTimes = "";
                        ViewState["BoatMinTimes"] += BoatMinTimes.ToString().Trim() + '~';
                        string BoatSeaterIds = "";
                        ViewState["BoatSeaterIds"] += BoatSeaterIds.ToString().Trim() + '~';
                        string InitBoatCharges = "";
                        ViewState["InitBoatCharges"] += InitBoatCharges.ToString().Trim() + '~';
                        string RowerMinCharge = "";
                        ViewState["RowerMinCharge"] += RowerMinCharge.ToString().Trim() + '~';
                        string boatOfferAmount = "";
                        ViewState["boatOfferAmount"] += boatOfferAmount.ToString().Trim() + '~';
                        string TaxAmountDetlBoat = "";
                        ViewState["TaxAmountDetlBoat"] += TaxAmountDetlBoat.ToString().Trim() + '~';
                        string InitNetAmounts = "";
                        ViewState["InitNetAmounts"] += InitNetAmounts.ToString().Trim() + '~';


                        string sUserId = Session["UserId"].ToString();
                        ViewState["sUserId"] += sUserId.ToString().Trim() + '~';
                        string SlotIds = "";
                        ViewState["SlotIds"] += SlotIds.ToString().Trim() + '~';
                        string sBlockId = "";
                        ViewState["sBlockId"] += sBlockId.ToString().Trim() + '~';
                        string Status = "";
                        ViewState["Status"] += Status.ToString().Trim() + '~';



                    }
                }
                else
                {
                    foreach (GridViewRow item in gvBoatdtl.Rows)
                    {
                        Label NumberOfBoat = (Label)item.FindControl("lblBoatCount");
                        int iNumofBoat = Convert.ToInt32(NumberOfBoat.Text.Trim());
                        for (int i = gvOther.Rows.Count; i <= iNumofBoat; i++)
                        {


                            string Status = "N";
                            ViewState["OthServiceStatus"] += Status.ToString().Trim() + '~';

                            int ServiceId = 0;
                            ViewState["OthServiceId"] += ServiceId.ToString().Trim() + '~';

                            int ChargePerItem = 0;
                            ViewState["OthChargePerItem"] += ChargePerItem.ToString().Trim() + '~';

                            int AdultCount = 0;
                            ViewState["OthNoOfItems"] += AdultCount.ToString().Trim() + '~';

                            int TaxDtl = 0;

                            ViewState["sOthGST"] += TaxDtl.ToString() + '~';

                            //int TaxAmount = 0;
                            //ViewState["sOthGSTAmount"] += TaxAmount.ToString() + '~';

                            int CGSTOthTaxAmount = 0;
                            ViewState["CGSTOthTaxAmount"] += CGSTOthTaxAmount.ToString() + '~';
                            int SGSTOthTaxAmount = 0;
                            ViewState["SGSTOthTaxAmount"] += SGSTOthTaxAmount.ToString() + '~';

                            ViewState["OthTaxDetails"] += TaxDtl.ToString() + '~';


                            ViewState["OthNetAmount"] += CGSTOthTaxAmount.ToString() + '~';

                        }

                    }
                }
            }

            else
            {
                //  ViewState["OthServiceStatus"] = "N";

                if (ViewState["OthServiceStatus"].ToString() == "N")
                {
                    ViewState["OthServiceStatus"] = "";

                    foreach (GridViewRow item in gvBoatdtl.Rows)
                    {
                        Label NumberOfBoat = (Label)item.FindControl("lblBoatCount");
                        int iNumofBoat = Convert.ToInt32(NumberOfBoat.Text.Trim());
                        for (int i = 1; i <= iNumofBoat; i++)
                        {
                            string Status = "N";
                            ViewState["OthServiceStatus"] += Status.ToString().Trim() + '~';

                            //ViewState["OthServiceId"] = "0";
                            //ViewState["OthChargePerItem"] = "0";
                            //ViewState["OthNoOfItems"] = "0";
                            //ViewState["sOthGST"] = "0";
                            //ViewState["sOthGSTAmount"] = "0";
                            //ViewState["OthNetAmount"] = "0";


                            int ServiceId = 0;
                            ViewState["OthServiceId"] += ServiceId.ToString().Trim() + '~';

                            int ChargePerItem = 0;
                            ViewState["OthChargePerItem"] += ChargePerItem.ToString().Trim() + '~';

                            int AdultCount = 0;
                            ViewState["OthNoOfItems"] += AdultCount.ToString().Trim() + '~';

                            int TaxDtl = 0;

                            ViewState["sOthGST"] += TaxDtl.ToString() + '~';

                            //int TaxAmount = 0;
                            //ViewState["sOthGSTAmount"] += TaxAmount.ToString() + '~';

                            int CGSTOthTaxAmount = 0;
                            ViewState["CGSTOthTaxAmount"] += CGSTOthTaxAmount.ToString() + '~';
                            int SGSTOthTaxAmount = 0;
                            ViewState["SGSTOthTaxAmount"] += SGSTOthTaxAmount.ToString() + '~';

                            ViewState["OthTaxDetails"] += TaxDtl.ToString() + '~';


                            ViewState["OthNetAmount"] += CGSTOthTaxAmount.ToString() + '~';
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    /// <summary>
    /// Modified By Abhinaya
    /// Date 11 Aug 2021
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnBoatBooking_Click(object sender, EventArgs e)

    //{
    //    try
    //    {
    //        //btnBoatBooking.Enabled = false;

    //        string ServiceType = "";
    //        string UserId = "";

    //        if (bsTotal.InnerText.Trim() == "")
    //        {
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
    //            return;
    //        }

    //        //if (Convert.ToDecimal(btnBoatBooking.Text.Trim()) <= 0)
    //        //{
    //        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
    //        //    return;
    //        //}

    //       // BoatBookingFinal();
    //        OtherBookingFinal();


    //        ///BoatBooking
    //        string SlotIds = string.Empty;
    //        string BoatTypeIds = string.Empty;
    //        string SeaterTypeIds = string.Empty;
    //        string BoatMinTime = string.Empty;
    //        string InitBoatCharges = string.Empty;
    //        string RowerMinCharge = string.Empty;
    //        string BoatDeposits = string.Empty;
    //        string boatOfferAmount = string.Empty;
    //        string TaxAmountDetlBoat = string.Empty;
    //        string InitNetAmounts = string.Empty;
    //        string UserIds = string.Empty;
    //        string BlockId = string.Empty;
    //        string TaxDetails = string.Empty;
    //        string TaxAmount = string.Empty;
    //        string CGSTTaxAmount = string.Empty;
    //        string SGSTTaxAmount = string.Empty;
    //        string Status = string.Empty;
    //        string Count = string.Empty;


    //        string[] sBoatTypeIds;
    //        string[] sSeaterTypeIds;
    //        string[] sBoatMinTime;
    //        string[] sInitBoatCharges;
    //        string[] sRowerMinCharge;
    //        string[] sBoatDeposits;
    //        string[] sboatOfferAmount;
    //        string[] sTaxAmountDetlBoat;
    //        string[] sInitNetAmounts;
    //        string[] sUserId;
    //        string[] sSlotIds;
    //        string[] sBlockId;
    //        string[] sTaxDetails;
    //        string[] sTaxAmount;
    //        string[] sStatus;
    //        string[] sCount;
    //        string[] sCGSTTaxAmount;
    //        string[] sSGSTTaxAmount;



    //        /////OTHER BOOKING
    //        string OthServiceId = string.Empty;
    //        string OthServiceStatus = string.Empty;
    //        string OthChargePerItem = string.Empty;
    //        string OthNoOfItems = string.Empty;
    //        string OthTaxDetails = string.Empty;
    //        string OthTaxAmount = string.Empty;
    //        string CGSTOthTaxAmount = string.Empty;
    //        string SGSTOthTaxAmount = string.Empty;
    //        string OthNetAmount = string.Empty;

    //        string[] sOthServiceStatus;
    //        string[] sOthServiceId;
    //        string[] sOthChargePerItem;
    //        string[] sOthNoOfItems;
    //        string[] sOthTaxDetails;
    //        string[] sOthTaxAmount;
    //        string[] sOthNetAmount;
    //        string[] sCGSTOthTaxAmount;
    //        string[] sSGSTOthTaxAmount;


    //        OthServiceStatus = ViewState["OthServiceStatus"].ToString();
    //        sOthServiceStatus = OthServiceStatus.Split('~');

    //        Count = ViewState["Count"].ToString();
    //        sCount = Count.Split('~');
    //        string LoopCount;
    //        string[] sLoopCount;
    //        for (int i = 1; i < sCount.Count(); i++)
    //        {
    //            int FirstCount = i;
    //            ViewState["Counts"] += FirstCount.ToString() + "~";

    //        }
    //        LoopCount = ViewState["Counts"].ToString();
    //        sLoopCount = LoopCount.Split('~');


    //        TaxDetails = ViewState["sGST"].ToString();
    //        sTaxDetails = TaxDetails.Split('~');

    //        //TaxAmount = ViewState["sGSTAmount"].ToString();
    //        //sTaxAmount = TaxAmount.Split('~');

    //        CGSTTaxAmount = ViewState["CGSTTaxAmount"].ToString();
    //        sCGSTTaxAmount = CGSTTaxAmount.Split('~');

    //        SGSTTaxAmount = ViewState["SGSTTaxAmount"].ToString();
    //        sSGSTTaxAmount = SGSTTaxAmount.Split('~');

    //        BoatDeposits = ViewState["BoatDeposits"].ToString().TrimEnd(' ');
    //        sBoatDeposits = BoatDeposits.Split('~');

    //        BoatTypeIds = ViewState["BoatTypeIds"].ToString().TrimEnd(' ');
    //        sBoatTypeIds = BoatTypeIds.Split('~');

    //        BoatMinTime = ViewState["BoatMinTimes"].ToString();
    //        sBoatMinTime = BoatMinTime.Split('~');

    //        SeaterTypeIds = ViewState["BoatSeaterIds"].ToString();
    //        sSeaterTypeIds = SeaterTypeIds.Split('~');

    //        InitBoatCharges = ViewState["InitBoatCharges"].ToString();
    //        sInitBoatCharges = InitBoatCharges.Split('~');

    //        RowerMinCharge = ViewState["RowerMinCharge"].ToString();
    //        sRowerMinCharge = RowerMinCharge.Split('~');

    //        boatOfferAmount = ViewState["boatOfferAmount"].ToString();
    //        sboatOfferAmount = boatOfferAmount.Split('~');

    //        TaxAmountDetlBoat = ViewState["TaxAmountDetlBoat"].ToString();
    //        sTaxAmountDetlBoat = TaxAmountDetlBoat.Split('~');



    //        InitNetAmounts = ViewState["InitNetAmounts"].ToString();
    //        sInitNetAmounts = InitNetAmounts.Split('~');

    //        UserIds = ViewState["sUserId"].ToString();
    //        sUserId = UserIds.Split('~');

    //        BlockId = ViewState["sBlockId"].ToString();
    //        sBlockId = BlockId.Split('~');

    //        SlotIds = ViewState["SlotIds"].ToString().TrimEnd(' ');
    //        sSlotIds = SlotIds.Split('~');

    //        Status = ViewState["Status"].ToString();
    //        sStatus = Status.Split('~');
    //        string HeaderStatus = string.Empty;
    //        int Ncount = sStatus.Count(s => s == "N");

    //        int Pcount = sStatus.Count(s => s == "P");
    //        int Icount = sStatus.Count(s => s == "I");

    //        if (Ncount > 0 && Pcount > 0 && Icount > 0)
    //        {
    //            HeaderStatus = "M";
    //        }
    //        if (Ncount > 0 && Pcount > 0)
    //        {
    //            HeaderStatus = "M";
    //        }
    //        if (Ncount > 0 && Icount > 0)
    //        {
    //            HeaderStatus = "M";
    //        }
    //        if (Pcount > 0 && Icount > 0)
    //        {
    //            HeaderStatus = "M";
    //        }
    //        if (Ncount > 0 && Pcount == 0 && Icount == 0)
    //        {
    //            HeaderStatus = "N";
    //        }
    //        if (Ncount == 0 && Pcount > 0 && Icount == 0)
    //        {
    //            HeaderStatus = "P";
    //        }
    //        if (Ncount == 0 && Pcount == 0 && Icount > 0)
    //        {
    //            HeaderStatus = "I";
    //        }


    //        OthServiceId = ViewState["OthServiceId"].ToString();
    //        sOthServiceId = OthServiceId.Split('~');

    //        OthChargePerItem = ViewState["OthChargePerItem"].ToString();
    //        sOthChargePerItem = OthChargePerItem.Split('~');

    //        OthNoOfItems = ViewState["OthNoOfItems"].ToString();
    //        sOthNoOfItems = OthNoOfItems.Split('~');

    //        OthTaxDetails = ViewState["OthTaxDetails"].ToString();
    //        sOthTaxDetails = OthTaxDetails.Split('~');

    //        OthNetAmount = ViewState["OthNetAmount"].ToString();
    //        sOthNetAmount = OthNetAmount.Split('~');


    //        OthTaxDetails = ViewState["sOthGST"].ToString();
    //        sOthTaxDetails = OthTaxDetails.Split('~');

    //        //OthTaxAmount = ViewState["sOthGSTAmount"].ToString();
    //        //sOthTaxAmount = OthTaxAmount.Split('~');

    //        CGSTOthTaxAmount = ViewState["CGSTOthTaxAmount"].ToString();
    //        sCGSTOthTaxAmount = CGSTOthTaxAmount.Split('~');

    //        SGSTOthTaxAmount = ViewState["SGSTOthTaxAmount"].ToString();
    //        sSGSTOthTaxAmount = SGSTOthTaxAmount.Split('~');

    //        if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE" || ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "UPI")
    //        {
    //            if (lblUserMobileNo.Text.Trim() != null)
    //            {
    //                if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE")
    //                {
    //                    ServiceType = "DOnlineBooking";
    //                    UserId = ViewState["CUserId"].ToString().Trim();
    //                }
    //                else
    //                {
    //                    ServiceType = "DUPI";
    //                    UserId = Session["UserId"].ToString().Trim();
    //                }

    //                OnlineBeforeTransactionDetails(ServiceType, UserId);

    //                if (ViewState["TranStatus"].ToString().Trim() == "Y")
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Payment SMS Link Send to Customer Mobile !');", true);

    //                    divBack.Style.Add("background-color", "white");
    //                    ClearBooking();
    //                    hfNatureVisible.Value = "0";
    //                    premimumMsg.Visible = false;
    //                    premimumMsg.InnerText = "";

    //                    hfBoatNature.Value = "N";
    //                    BoatBookedSummaryList();
    //                    dtlOther.Visible = false;
    //                    divBack.Style.Add("background-color", "white");
    //                }

    //                return;
    //            }
    //        }

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));




    //            string GSTNO = string.Empty;
    //            string CollectedAmount = string.Empty;
    //            string BalanceAmount = string.Empty;
    //            GSTNO = "";
    //           // if (chkGSTNo.Checked == true)
    //            //{
    //            //    GSTNO = txtINSGSTNO.Text.Trim();
    //            //}
    //            //else
    //            //{
    //            //    GSTNO = "";
    //            //}

    //            if (txtAmountPaid.Text.Trim() != "")
    //            {
    //                CollectedAmount = txtAmountPaid.Text.Trim();
    //            }
    //            else
    //            {
    //                CollectedAmount = "0";
    //            }

    //            if (hfBalanceAmt.Value.Trim() != "")
    //            {
    //                BalanceAmount = hfBalanceAmt.Value.Trim();
    //            }
    //            else
    //            {
    //                BalanceAmount = "0";
    //            }

    //            var BoatBook = new BoatBookNew()
    //            {
    //                QueryType = "Insert",
    //                BookingId = "0",
    //                BookingDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"),
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),

    //                Bookingpin = txtPIN.Text.Trim(),
    //                CustomerId = ViewState["CUserId"].ToString().Trim(),
    //                CustomerMobileNo = lblUserMobileNo.Text.Trim(),
    //                CustomerName = ViewState["CName"].ToString().Trim(),
    //                CustomerAddress = "",
    //                BoatPremiumStatus = sStatus,

    //                NoOfPass = "0",
    //                NoOfChild = "0",
    //                NoOfInfant = "0",
    //                OfferId = "0",

    //                InitBillAmount = bsTotal.InnerText.Trim(),
    //                PaymentType = ddlPaymentType.SelectedValue.Trim(),
    //                ActualBillAmount = bsTotal.InnerText.Trim(), //need what amount this;
    //                Status = "B",
    //                BoatTypeId = sBoatTypeIds,

    //                BoatSeaterId = sSeaterTypeIds,
    //                BookingDuration = sBoatMinTime,
    //                InitBoatCharge = sInitBoatCharges,
    //                InitRowerCharge = sRowerMinCharge,
    //                BoatDeposit = sBoatDeposits,

    //                // TaxDetails = sTaxAmountDetlBoat,
    //                // TaxDetails = sTaxDetails,
    //                // TaxAmount = sTaxAmount,
    //                CGSTTaxAmount = sCGSTTaxAmount,
    //                SGSTTaxAmount = sSGSTTaxAmount,
    //                InitOfferAmount = sboatOfferAmount,
    //                InitNetAmount = sInitNetAmounts,
    //                CreatedBy = sUserId,

    //                ////Other Service Booking
    //                OthServiceStatus = sOthServiceStatus,
    //                OthServiceId = sOthServiceId,
    //                OthChargePerItem = sOthChargePerItem,
    //                OthNoOfItems = sOthNoOfItems,
    //                CGSTOthTaxAmount = sCGSTOthTaxAmount,
    //                SGSTOthTaxAmount = sSGSTOthTaxAmount,

    //                //  OthTaxDetails = sOthTaxDetails,
    //                // OthTaxAmount = sOthTaxAmount,
    //                OthNetAmount = sOthNetAmount,
    //                BookingMedia = "DW",

    //                BFDInitBoatCharge = "0",
    //                BFDInitNetAmount = "0",
    //                BFDGST = "0",
    //                CustomerGSTNo = GSTNO,
    //                CollectedAmount = CollectedAmount.Trim(),
    //                BalanceAmount = BalanceAmount.Trim(),
    //                Countslotids = sLoopCount,
    //                BookingTimeSlotId = sSlotIds,
    //                BookingBlockId = sBlockId,
    //                PremiumStatus = HeaderStatus

    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("BoatBookingDept", BoatBook).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    string[] sResult = ResponseMsg.Split('~');

    //                    string[] Pin = sResult[2].Split(',');
    //                    string Pins = Pin[0].Replace("\"", string.Empty).Trim();
    //                    string Pinss = Pins.Replace("]", string.Empty).Trim();

    //                    if (ViewState["PINType"].ToString().Trim() == "D")
    //                    {
    //                        //GetBoatTickets(sResult[1].ToString());
    //                        //GetBoatTicketsSummaryReceipt(sResult[1].ToString());
    //                        //GetOtherTickets(sResult[1].ToString());

    //                        //if (chkCustMobileNo.Checked == true)
    //                        //{
    //                        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Boat Booking Successfully !');", true);
    //                        //}
    //                        //else
    //                        //{
    //                        //    Response.Redirect("PrintBoat.aspx?rt=b&bi=" + Pinss.ToString() + "");
    //                        //}
    //                    }
    //                    else
    //                    {
    //                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Boat Booking Successfully !');", true);
    //                    }

    //                    divBack.Style.Add("background-color", "white");
    //                    ClearBooking();
    //                    hfNatureVisible.Value = "0";
    //                    premimumMsg.Visible = false;
    //                    premimumMsg.InnerText = "";

    //                    hfBoatNature.Value = "N";
    //                    BoatBookedSummaryList();
    //                    dtlOther.Visible = false;
    //                    divBack.Style.Add("background-color", "white");

    //                    txtINSGSTNO.Text = string.Empty;
    //                    divGST.Visible = false;
    //                    //chkGSTNo.Checked = false;
    //                }
    //                else
    //                {
    //                    string BookingId = GetBookingIdByPin();

    //                    if (BookingId.Trim() != "")
    //                    {
    //                        Response.Redirect("PrintBoat.aspx?rt=b&bi=" + BookingId.Trim() + "");
    //                        return;
    //                    }

    //                    txtINSGSTNO.Text = string.Empty;
    //                    divGST.Visible = false;
    //                   //// chkGSTNo.Checked = false;

    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
    //                }
    //            }
    //            else
    //            {
    //                txtINSGSTNO.Text = string.Empty;
    //                divGST.Visible = false;
    //                //chkGSTNo.Checked = false;
    //            }
    //        }
    //    }

    //    catch (Exception ex)
    //    {
    //        txtINSGSTNO.Text = string.Empty;
    //        divGST.Visible = false;
    //        //chkGSTNo.Checked = false;
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //}

    protected void imgbtnNewBook_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divGridList.Visible = false;
            divEntry.Visible = true;
            ////divShow.Visible = false;
            //imgbtnNewBook.Visible = false;
            //imgbtnBookedList.Visible = true;
            ClearBooking();
            BoatBookedSummaryList();
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void imgCloseTicket_Click(object sender, ImageClickEventArgs e)
    {
        MpeBillService.Hide();
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

                if (Convert.ToDecimal(RowerCharge.Text) <= 0)
                {
                    divrower.Visible = false;
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

    public void BookedListdtl()
    {
        try
        {
            /// divShow.Visible = true;
            // imgbtnNewBook.Visible = true;
            //imgbtnBookedList.Visible = false;

            //MpeBillServiceGrid.Show();
            divGridList.Visible = true;
            divEntry.Visible = false;

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
                    GvBoatBooking.Columns[9].Visible = true;

                    var BoatSearch = new BoatSearch()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingType = "B",
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "Admin"
                    };
                    response = client.PostAsJsonAsync("BoatBookedList", BoatSearch).Result;
                }
                else
                {
                    var BoatSearch = new BoatSearch()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingType = "B",
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("BoatBookedList", BoatSearch).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            GvBoatBooking.DataSource = dt;
                            GvBoatBooking.DataBind();
                            GvBoatBooking.Visible = true;
                        }
                        else
                        {
                            GvBoatBooking.Visible = false;
                            lblGridMsg.Text = "No Boat Booking Today...!";
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

    protected void imgbtnBookedList_Click(object sender, ImageClickEventArgs e)
    {
        BookedListdtl();
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string BookingId = GvBoatBooking.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["BoOkingID"] = BookingId.ToString().Trim();
            string Status = GvBoatBooking.DataKeys[gvrow.RowIndex]["Status"].ToString().Trim();
            if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
            {
                Mpepnlrsn.Show();
            }
            else
            {
                if (Status == "Y")
                {
                    Mpepnlrsn.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Time Out, Cannot Print Details');", true);
                    return;
                }
            }

            //GetBoatTickets(BookingId);
            //GetBoatTicketsSummaryReceipt(BookingId);
            //GetOtherTickets(BookingId);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ImgBtnSMS_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            string BookingId = GvBoatBooking.DataKeys[gvrow.RowIndex]["BookingId"].ToString().Trim();
            string sMobileNo = GvBoatBooking.DataKeys[gvrow.RowIndex]["CustomerMobile"].ToString().Trim();

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
                        ServiceType = "BBReceipt",
                        BookingId = BookingId.ToString(),
                        MobileNo = sMobileNo.ToString(),
                        MediaType = "DW",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        Remarks = ""
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("SendSMSMsg", OtpSendSms).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('e-Receipt Link Send to Customer Mobile !');", true);
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

    private void GetBoatTickets(string sBookingId)
    {
        try
        {
            MpeBillService.Show();

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

            MpeBillService.Show();

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

    private void ClearBooking()
    {
        try
        {
            toolTipDpt.Visible = false;

            //btnBoatBooking.Enabled = true;
            //ddlPaymentType.SelectedIndex = 0;

            ViewState["BoatTypeIds"] = string.Empty;
            ViewState["BoatSeaterIds"] = string.Empty;
            ViewState["BoatMinTimes"] = string.Empty;
            ViewState["InitBoatCharges"] = string.Empty;
            ViewState["RowerMinCharges"] = string.Empty;
            ViewState["RowerMinCharge"] = string.Empty;
            ViewState["BoatDeposits"] = string.Empty;
            ViewState["InitNetAmounts"] = string.Empty;
            ViewState["InitNetAmount"] = string.Empty;
            ViewState["TaxAmountDetlBoat"] = string.Empty;


            ViewState["OthServiceStatus"] = "N";
            ViewState["OthServiceId"] = "";
            ViewState["OthChargePerItem"] = "";
            ViewState["OthNoOfItems"] = "";
            ViewState["OthNetAmount"] = "";
            ViewState["OthTaxDetails"] = "";

            ViewState["boatOfferAmount"] = "";

            bschar1.InnerText = "";
            // oschar1.InnerText = "";
            bsgst1.InnerText = "";
            bsdeposit1.InnerText = "";
            bsTotal.InnerText = "";
            txtPIN.Text = "";
            txtPIN.ReadOnly = false;
            divPin.Visible = false;
            imgPinStatus.ImageUrl = "";
            lblUserMobileNo.Text = "";
            ViewState["CUserId"] = "";
            ViewState["CName"] = "";
            ViewState["CMailId"] = "";
            divBooking.Visible = false;
            btnDefaultPin.Visible = false;
            ViewState["PINType"] = "";

            ViewState["CartRow"] = null;
            ViewState["Row"] = null;

            ViewState["CartRowO"] = null;
            ViewState["RowO"] = null;

            ViewState["BoatChargeSum"] = "0";
            ViewState["BoatTaxSum"] = "0";
            ViewState["BoatDepositSum"] = "0";
            ViewState["BoatTotalSum"] = "0";

            gvBoatdtl.Visible = false;
            gvOther.DataBind();
            dtlOther.DataBind();

            // chkCustMobileNo.Checked = false;
            //chkCustMobileNo.Enabled = true;
            txtCustMobileNo.Text = "";
            divCustMobile.Visible = false;

            GetPaymentType();
            //btnBoatBooking.Text = "Submit";

            //txtAmountPaid.Text = "";
            //txtBalanceAmnt.Text = "";
            hfBalanceAmt.Value = "";

            // Add Four new values. 
            ViewState["SlotId"] = "";
            ViewState["SlotType"] = "";
            ViewState["BlockId"] = "";
            ViewState["AvailableTripCount"] = "";
            txtMobileNo.Text = "";

            lblName.Text = "";
            lblEmailId.Text = "";
            trMobileNo.Visible = false;
            txtMobileNo.Enabled = true;
            divOtpbtn.Visible = false;
            divOtptxt.Visible = false;
            imgPinStatus.ImageUrl = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["BoatChargeSum"] = "0";
            ViewState["BoatTaxSum"] = "0";
            ViewState["BoatDepositSum"] = "0";
            ViewState["BoatTotalSum"] = "0";

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sUniqueId = gvBoatdtl.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
            string sStatusDelete = gvBoatdtl.DataKeys[gvrow.RowIndex]["Status"].ToString().Trim();
            string StatusDelete = string.Empty;
            if (sStatusDelete.Trim() == "Normal")
            {
                StatusDelete = "N";
            }
            if (sStatusDelete.Trim() == "Express")
            {
                StatusDelete = "P";
            }
            if (sStatusDelete.Trim() == "Individual")
            {
                StatusDelete = "I";
            }
            string sBoatTypeId = gvBoatdtl.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim();
            string sSeaterTypeId = gvBoatdtl.DataKeys[gvrow.RowIndex]["SeaterTypeId"].ToString().Trim();
            string sSlotId = gvBoatdtl.DataKeys[gvrow.RowIndex]["SlotId"].ToString().Trim();
            ViewState["DelBlockId"] = "";
            DataTable BlockDt = (DataTable)ViewState["Row"];

            for (int bk = 0; bk < BlockDt.Rows.Count; bk++)
            {
                if (sUniqueId == BlockDt.Rows[bk]["UniqueId"].ToString().Trim())
                {
                    if (sStatusDelete == BlockDt.Rows[bk]["Status"].ToString().Trim())
                    {
                        ViewState["DelBlockId"] += BlockDt.Rows[bk]["BlockId"].ToString().Trim() + ',';
                    }

                }

            }
            DataTable dtCurrentTable = (DataTable)ViewState["Row"];
            string DelBlockId;
            string[] sDelBlockid;
            DelBlockId = ViewState["DelBlockId"].ToString();
            sDelBlockid = DelBlockId.Split(',');
            for (int s = 0; s < sDelBlockid.Count(); s++)
            {
                for (int J = 0; J < dtCurrentTable.Rows.Count; J++)
                {
                    DataRow dr = dtCurrentTable.Rows[J];

                    if (dtCurrentTable.Rows[J]["BlockId"].ToString().Trim() == sDelBlockid[s].ToString().Trim())
                    {
                        dr.Delete();
                        DeleteTmpBookedSlot(sBoatTypeId.Trim(), sSeaterTypeId.Trim(), sSlotId.Trim(), StatusDelete.Trim(), sDelBlockid[s]);
                        dtCurrentTable.AcceptChanges();

                    }
                }
            }


            dtCurrentTable.AcceptChanges();
            if (dtCurrentTable.Rows.Count > 0)
            {
                DataTable dt2 = (DataTable)dtCurrentTable;

                DataTable dts = dt2.Clone();

                var UpdatedTable = (from row in dt2.AsEnumerable()
                                    group row by new
                                    {
                                        UniqueId = row.Field<string>("UniqueId"),
                                        BoatType = row.Field<string>("BoatType"),
                                        BoatTypeId = row.Field<string>("BoatTypeId"),

                                        SeaterType = row.Field<string>("SeaterType"),
                                        SeaterTypeId = row.Field<string>("SeaterTypeId"),
                                        Status = row.Field<string>("Status"),
                                        //BoatCount = row.Field<Int32>("BoatCount"),
                                        //BoatTotalCharge = row.Field<decimal>("BoatTotalCharge"),

                                        BoatMinCharge = row.Field<string>("BoatMinCharge"),
                                        RowerMinCharge = row.Field<string>("RowerMinCharge"),
                                        BoatTaxCharge = row.Field<string>("BoatTaxCharge"),

                                        DepositType = row.Field<string>("DepositType"),
                                        Deposit = row.Field<decimal>("Deposit"),
                                        BoatMinTime = row.Field<string>("BoatMinTime"),
                                        SlotDesc = row.Field<string>("SlotDesc"),
                                        SlotId = row.Field<string>("SlotId"),

                                    } into t1
                                    select new
                                    {
                                        UniqueID = t1.Key.UniqueId,
                                        BoatType = t1.Key.BoatType,
                                        BoatTypeId = t1.Key.BoatTypeId,

                                        SeaterType = t1.Key.SeaterType,
                                        SeaterTypeId = t1.Key.SeaterTypeId,
                                        Status = t1.Key.Status,

                                        BoatCount = t1.Sum(a => a.Field<Int32>("BoatCount")),
                                        BoatTotalCharge = t1.Sum(a => a.Field<decimal>("BoatTotalCharge")) + t1.Sum(b => b.Field<decimal>("Deposit")),

                                        BoatMinCharge = t1.Key.BoatMinCharge,
                                        RowerMinCharge = t1.Key.RowerMinCharge,
                                        BoatTaxCharge = t1.Key.BoatTaxCharge,

                                        DepositType = t1.Key.DepositType,
                                        Deposit = t1.Key.Deposit,
                                        BoatMinTime = t1.Key.BoatMinTime,
                                        SlotDesc = t1.Key.SlotDesc,
                                        SlotId = t1.Key.SlotId,

                                    })
                     .Select(g =>
                     {
                         var h = dts.NewRow();
                         h["UniqueId"] = g.UniqueID;
                         h["BoatType"] = g.BoatType;
                         h["BoatTypeId"] = g.BoatTypeId;

                         h["SeaterType"] = g.SeaterType;
                         h["SeaterTypeId"] = g.SeaterTypeId;
                         h["Status"] = g.Status;

                         h["BoatCount"] = g.BoatCount;
                         h["BoatTotalCharge"] = g.BoatTotalCharge;

                         h["BoatMinCharge"] = g.BoatMinCharge;
                         h["RowerMinCharge"] = g.RowerMinCharge;
                         h["BoatTaxCharge"] = g.BoatTaxCharge;

                         h["DepositType"] = g.DepositType;
                         h["Deposit"] = g.Deposit;
                         h["BoatMinTime"] = g.BoatMinTime;
                         h["SlotDesc"] = g.SlotDesc;
                         h["SlotId"] = g.SlotId;


                         return h;
                     }).CopyToDataTable();

                //dtCurrentTable1.AcceptChanges();


                ViewState["CartRow"] = UpdatedTable;
                gvBoatdtl.Visible = true;
                gvBoatdtl.DataSource = UpdatedTable;
                gvBoatdtl.DataBind();

                DataTable dtTable = (DataTable)ViewState["Row"];

                decimal dBoatMinCharge = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatMinCharge")));
                decimal dRowerMinCharge = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("RowerMinCharge")));
                //bschar1.InnerText = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();
                ViewState["BoatChargeSum"] = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();


                decimal dGSTAmount = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatTaxCharge")));
                //bsgst1.InnerText = dGSTAmount.ToString();
                ViewState["BoatTaxSum"] = dGSTAmount.ToString();

                decimal dDeposit = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Deposit")));
                //bsdeposit1.InnerText = dDeposit.ToString();
                ViewState["BoatDepositSum"] = dDeposit.ToString();

                decimal dTotal = Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge) + Convert.ToDecimal(dGSTAmount) + Convert.ToDecimal(dDeposit);
                bsTotal.InnerText = dTotal.ToString();
                ViewState["BoatTotalSum"] = dTotal.ToString();
            }
            else
            {
                // gvBoatdtl.DataBind();

                ViewState["CartRow"] = null;
                ViewState["Row"] = null;
                gvBoatdtl.Visible = false;

                bschar1.InnerText = "";
                bsgst1.InnerText = "";
                bsdeposit1.InnerText = "";
                bsTotal.InnerText = "";
                // btnBoatBooking.Text = "";

                txtPIN.Text = "";
            }

            CalculateSummary();
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //protected void ImgBtnDelete_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        ViewState["BoatChargeSum"] = "0";
    //        ViewState["BoatTaxSum"] = "0";
    //        ViewState["BoatDepositSum"] = "0";
    //        ViewState["BoatTotalSum"] = "0";

    //        ImageButton lnkbtn = sender as ImageButton;
    //        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
    //        string sUniqueId = gvBoatdtl.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
    //        string sStatusDelete = gvBoatdtl.DataKeys[gvrow.RowIndex]["Status"].ToString().Trim();
    //        string StatusDelete = string.Empty;
    //        if (sStatusDelete.Trim() == "Normal")
    //        {
    //            StatusDelete = "N";
    //        }
    //        if (sStatusDelete.Trim() == "Express")
    //        {
    //            StatusDelete = "P";
    //        }
    //        if (sStatusDelete.Trim() == "Individual")
    //        {
    //            StatusDelete = "I";
    //        }
    //        string sBoatTypeId = gvBoatdtl.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim();
    //        string sSeaterTypeId = gvBoatdtl.DataKeys[gvrow.RowIndex]["SeaterTypeId"].ToString().Trim();
    //        string sSlotId = gvBoatdtl.DataKeys[gvrow.RowIndex]["SlotId"].ToString().Trim();
    //        ViewState["DelBlockId"] = "";
    //        DataTable BlockDt = (DataTable)ViewState["Row"];

    //        for (int bk = 0; bk < BlockDt.Rows.Count; bk++)
    //        {
    //            if (sUniqueId == BlockDt.Rows[bk]["UniqueId"].ToString().Trim())
    //            {
    //                if (sStatusDelete == BlockDt.Rows[bk]["Status"].ToString().Trim())
    //                {
    //                    ViewState["DelBlockId"] += BlockDt.Rows[bk]["BlockId"].ToString().Trim() + ',';
    //                }

    //            }

    //        }
    //        DataTable dtCurrentTable = (DataTable)ViewState["Row"];
    //        string DelBlockId;
    //        string[] sDelBlockid;
    //        DelBlockId = ViewState["DelBlockId"].ToString();
    //        sDelBlockid = DelBlockId.Split(',');
    //        for (int s = 0; s < sDelBlockid.Count(); s++)
    //        {
    //            for (int J = 0; J < dtCurrentTable.Rows.Count; J++)
    //            {
    //                DataRow dr = dtCurrentTable.Rows[J];

    //                if (dtCurrentTable.Rows[J]["BlockId"].ToString().Trim() == sDelBlockid[s].ToString().Trim())
    //                {
    //                    dr.Delete();
    //                    DeleteTmpBookedSlot(sBoatTypeId.Trim(), sSeaterTypeId.Trim(), sSlotId.Trim(), StatusDelete.Trim(), sDelBlockid[s]);
    //                    dtCurrentTable.AcceptChanges();

    //                }
    //            }
    //        }
    //        // DeleteTmpBookedSlot(sBoatTypeId.Trim(), sSeaterTypeId.Trim(), sSlotId.Trim(), StatusDelete.Trim());



    //        //for (int i = dtCurrentTable.Rows.Count - 1; i >= 0; i--)
    //        //{
    //        //    DataRow dr = dtCurrentTable.Rows[i];
    //        //    if (dr["UniqueId"].ToString().Trim() == sUniqueId.Trim())
    //        //    {
    //        //        dr.Delete();
    //        //    }
    //        //}

    //        //dtCurrentTable.AcceptChanges();

    //        //DataTable dtCurrentTable1 = (DataTable)ViewState["Row"];

    //        //for (int i = dtCurrentTable1.Rows.Count - 1; i >= 0; i--)
    //        //{
    //        //    DataRow dr = dtCurrentTable1.Rows[i];
    //        //    if (dr["UniqueId"].ToString().Trim() == sUniqueId.Trim())
    //        //    {
    //        //        dr.Delete();
    //        //    }
    //        //}

    //        dtCurrentTable.AcceptChanges();
    //        if (dtCurrentTable.Rows.Count > 0)
    //        {
    //            DataTable dt2 = (DataTable)dtCurrentTable;

    //            DataTable dts = dt2.Clone();

    //            var UpdatedTable = (from row in dt2.AsEnumerable()
    //                                group row by new
    //                                {
    //                                    UniqueId = row.Field<string>("UniqueId"),
    //                                    BoatType = row.Field<string>("BoatType"),
    //                                    BoatTypeId = row.Field<string>("BoatTypeId"),

    //                                    SeaterType = row.Field<string>("SeaterType"),
    //                                    SeaterTypeId = row.Field<string>("SeaterTypeId"),
    //                                    Status = row.Field<string>("Status"),
    //                                    //BoatCount = row.Field<Int32>("BoatCount"),
    //                                    //BoatTotalCharge = row.Field<decimal>("BoatTotalCharge"),

    //                                    BoatMinCharge = row.Field<string>("BoatMinCharge"),
    //                                    RowerMinCharge = row.Field<string>("RowerMinCharge"),
    //                                    BoatTaxCharge = row.Field<string>("BoatTaxCharge"),

    //                                    DepositType = row.Field<string>("DepositType"),
    //                                    Deposit = row.Field<decimal>("Deposit"),
    //                                    BoatMinTime = row.Field<string>("BoatMinTime"),
    //                                    SlotDesc = row.Field<string>("SlotDesc"),
    //                                    SlotId = row.Field<string>("SlotId"),

    //                                } into t1
    //                                select new
    //                                {
    //                                    UniqueID = t1.Key.UniqueId,
    //                                    BoatType = t1.Key.BoatType,
    //                                    BoatTypeId = t1.Key.BoatTypeId,

    //                                    SeaterType = t1.Key.SeaterType,
    //                                    SeaterTypeId = t1.Key.SeaterTypeId,
    //                                    Status = t1.Key.Status,

    //                                    BoatCount = t1.Sum(a => a.Field<Int32>("BoatCount")),
    //                                    BoatTotalCharge = t1.Sum(a => a.Field<decimal>("BoatTotalCharge")) + t1.Sum(b => b.Field<decimal>("Deposit")),

    //                                    BoatMinCharge = t1.Key.BoatMinCharge,
    //                                    RowerMinCharge = t1.Key.RowerMinCharge,
    //                                    BoatTaxCharge = t1.Key.BoatTaxCharge,

    //                                    DepositType = t1.Key.DepositType,
    //                                    Deposit = t1.Key.Deposit,
    //                                    BoatMinTime = t1.Key.BoatMinTime,
    //                                    SlotDesc = t1.Key.SlotDesc,
    //                                    SlotId = t1.Key.SlotId,

    //                                })
    //                 .Select(g =>
    //                 {
    //                     var h = dts.NewRow();
    //                     h["UniqueId"] = g.UniqueID;
    //                     h["BoatType"] = g.BoatType;
    //                     h["BoatTypeId"] = g.BoatTypeId;

    //                     h["SeaterType"] = g.SeaterType;
    //                     h["SeaterTypeId"] = g.SeaterTypeId;
    //                     h["Status"] = g.Status;

    //                     h["BoatCount"] = g.BoatCount;
    //                     h["BoatTotalCharge"] = g.BoatTotalCharge;

    //                     h["BoatMinCharge"] = g.BoatMinCharge;
    //                     h["RowerMinCharge"] = g.RowerMinCharge;
    //                     h["BoatTaxCharge"] = g.BoatTaxCharge;

    //                     h["DepositType"] = g.DepositType;
    //                     h["Deposit"] = g.Deposit;
    //                     h["BoatMinTime"] = g.BoatMinTime;
    //                     h["SlotDesc"] = g.SlotDesc;
    //                     h["SlotId"] = g.SlotId;


    //                     return h;
    //                 }).CopyToDataTable();

    //            //dtCurrentTable1.AcceptChanges();


    //            ViewState["CartRow"] = UpdatedTable;
    //            gvBoatdtl.Visible = true;
    //            gvBoatdtl.DataSource = UpdatedTable;
    //            gvBoatdtl.DataBind();

    //            //  DataTable dtTable = (DataTable)ViewState["Row"];

    //            decimal dBoatMinCharge = UpdatedTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatMinCharge")));
    //            decimal dRowerMinCharge = UpdatedTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("RowerMinCharge")));
    //            //bschar1.InnerText = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();
    //            ViewState["BoatChargeSum"] = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();


    //            decimal dGSTAmount = UpdatedTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatTaxCharge")));
    //            //bsgst1.InnerText = dGSTAmount.ToString();
    //            ViewState["BoatTaxSum"] = dGSTAmount.ToString();

    //            decimal dDeposit = UpdatedTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Deposit")));
    //            //bsdeposit1.InnerText = dDeposit.ToString();
    //            ViewState["BoatDepositSum"] = dDeposit.ToString();

    //            decimal dTotal = Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge) + Convert.ToDecimal(dGSTAmount) + Convert.ToDecimal(dDeposit);
    //            bsTotal.InnerText = dTotal.ToString();
    //            ViewState["BoatTotalSum"] = dTotal.ToString();
    //        }
    //        else
    //        {
    //            // gvBoatdtl.DataBind();

    //            ViewState["CartRow"] = null;
    //            ViewState["Row"] = null;
    //            gvBoatdtl.Visible = false;

    //            bschar1.InnerText = "";
    //            bsgst1.InnerText = "";
    //            bsdeposit1.InnerText = "";
    //            bsTotal.InnerText = "";
    //            btnBoatBooking.Text = "";

    //            txtPIN.Text = "";
    //        }

    //        CalculateSummary();
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //}

    public void DeleteTmpBookedSlot(string sBoatTypeId, string sBoatSeaterTypeId, string sSlotId, string statusDeleteTmp, string sblockid)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var AvailableBoatBookingSlotDept = new AvailableBoatBookingSlotDept()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    CheckInDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    BoatTypeId = sBoatTypeId.Trim(),
                    BoatSeaterId = sBoatSeaterTypeId.Trim(),
                    SlotType = statusDeleteTmp.Trim(),
                    SlotId = sSlotId.Trim(),
                    UserId = Session["UserId"].ToString().Trim(),
                    BlockId = sblockid.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("DeleteTmpBookedSlotDept", AvailableBoatBookingSlotDept).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Slto = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Slto)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Slto)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg + "');", true);
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            divBack.Style.Add("background-color", "white");
            ClearBooking();
            hfNatureVisible.Value = "0";
            premimumMsg.Visible = false;
            premimumMsg.InnerText = "";

            hfBoatNature.Value = "N";
            BoatBookedSummaryList();
            dtlOther.Visible = false;
            divBack.Style.Add("background-color", "white");
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void BulletedList1_Click(object sender, BulletedListEventArgs e)
    {
        try
        {

            BulletedList ddl = sender as BulletedList;

            //get the namingcontainer from the dropdownlist and cast it as a datalistitem
            DataListItem item = ddl.NamingContainer as DataListItem;

            BulletedList lstValue = item.FindControl("BulletedList1") as BulletedList;

            string BoatTypeId = string.Empty;
            string BoatTypeName = string.Empty;

            string BoatSeaterId = string.Empty;
            string BoatSeaterType = string.Empty;

            string BoatCount = string.Empty;
            string BoatTotalCharge = string.Empty;

            string BoatMinCharge = string.Empty;
            string RowerMinCharge = string.Empty;
            string BoatTaxCharge = string.Empty;
            string DepositType = string.Empty;
            string Deposit = string.Empty;

            Label lblBoatTypeId = item.FindControl("lblBoatTypeId") as Label;
            BoatTypeId = lblBoatTypeId.Text;

            Label lblBoatTypes = item.FindControl("lblBoatTypes") as Label;
            BoatTypeName = lblBoatTypes.Text;

            Label lblBoatSeaterId = item.FindControl("lblBoatSeaterId") as Label;
            BoatSeaterId = lblBoatSeaterId.Text;

            Label lblSeaterTypes = item.FindControl("lblSeaterTypes") as Label;
            BoatSeaterType = lblSeaterTypes.Text;

            DropDownList NoCount = item.FindControl("dlstCount") as DropDownList;
            BoatCount = lstValue.Items[e.Index].Text.Trim();

            Label lblBoatTotalCharge = item.FindControl("lblBoatTotalCharge") as Label;
            BoatTotalCharge = lblBoatTotalCharge.Text;

            Label lblBoatMinCharge = item.FindControl("lblBoatMinCharge") as Label;
            BoatMinCharge = lblBoatMinCharge.Text;

            Label lblRowerMinCharge = item.FindControl("lblRowerMinCharge") as Label;
            RowerMinCharge = lblRowerMinCharge.Text;

            Label lblBoatTaxCharge = item.FindControl("lblBoatTaxCharge") as Label;
            BoatTaxCharge = lblBoatTaxCharge.Text;

            Label lblDepositType = item.FindControl("lblDepositType") as Label;
            DepositType = lblDepositType.Text;

            Label lblDeposit = item.FindControl("lblDeposit") as Label;
            Deposit = lblDeposit.Text;

            Label lblBoatMinTime = item.FindControl("lblBoatMinTime") as Label;
            string BoatMinTime = lblBoatMinTime.Text;

            decimal BoatDepositAmount = 0;

            if (DepositType == "F")
            {
                BoatDepositAmount = (Convert.ToDecimal(BoatCount) * Convert.ToDecimal(Deposit));
            }
            else
            {
                decimal bAmount = (Convert.ToDecimal(BoatCount) * Convert.ToDecimal(BoatMinCharge)) + (Convert.ToDecimal(BoatCount) * Convert.ToDecimal(RowerMinCharge));
                BoatDepositAmount = ((bAmount * Convert.ToDecimal(Deposit)) / 100);
            }
            string BoatNature = string.Empty;
            if (hfBoatNature.Value == "N")
            {
                BoatNature = "Normal";
            }
            else if (hfBoatNature.Value == "P")
            {
                BoatNature = "Express";
            }
            else
            {
                BoatNature = "Individual";
            }
            BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatNature, BoatCount, BoatTotalCharge, BoatMinCharge,
                RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount.ToString("0.00"), BoatMinTime, "NULL", "NULL", "NULL", "NULL");
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void btnNor_Click(object sender, EventArgs e)
    {


        //if (CheckBoatSelectionStatus("P") == true)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sorry, Already Selected an Express Boat Booking !');", true);
        //    return;
        //}
        //if (CheckBoatSelectionStatus("I") == true)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sorry, Already Selected an Indidual Boat Booking !');", true);
        //    return;
        //}

        divBack.Style.Add("background-color", "white");
        //ClearBooking();
        hfNatureVisible.Value = "0";
        premimumMsg.Visible = false;
        premimumMsg.InnerText = "";

        hfBoatNature.Value = "N";
        BoatBookedSummaryList();
        dtlOther.Visible = false;
        divBack.Style.Add("background-color", "white");
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    protected void btnPre_Click(object sender, EventArgs e)
    {

        //if (CheckBoatSelectionStatus("N") == true)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sorry, Already Selected an Normal Boat Booking !');", true);
        //    return;
        //}

        //if (CheckBoatSelectionStatus("I") == true)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sorry, Already Selected an Individual Boat Booking !');", true);
        //    return;
        //}

        divBack.Attributes.Add("style", "background-image: repeating-linear-gradient(90deg, #bf9b30, #ffe34d, #ffcf40, #ffe34d, #e6c200, #ccac00); margin-right:-2px;margin-left:-12px;");
        // ClearBooking();
        hfNatureVisible.Value = "0";
        premimumMsg.Visible = false;
        premimumMsg.InnerText = "";

        hfBoatNature.Value = "P";
        BoatBookedSummaryList();
        dtlOther.Visible = false;
        divBack.Attributes.Add("style", "background-image: repeating-linear-gradient(90deg, #bf9b30, #ffe34d, #ffcf40, #ffe34d, #e6c200, #ccac00); margin-right:-2px;margin-left:-12px;");
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    /// <summary>
    /// Individual Ticket 
    /// Created K.Abhinaya
    /// Date 29.Jul.2021
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnIndividual_Click(object sender, EventArgs e)
    {
        divBack.Attributes.Add("style", "background-image: repeating-linear-gradient(90deg, #bf308c45, #ff4dfa85, #ff40f069, #ff4de191, #f11cb95e, #c900cc73); margin-right:-3px;margin-left:-11px;");
        hfNatureVisible.Value = "0";

        hfBoatNature.Value = "I";
        BoatBookedSummaryList();
        dtlOther.Visible = false;
        divBack.Attributes.Add("style", "background-image: repeating-linear-gradient(90deg, #bf308c45, #ff4dfa85, #ff40f069, #ff4de191, #f11cb95e, #c900cc73); margin-right:-3px;margin-left:-11px;");
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    //Additional Boat Tickets//

    protected void btnAdditional_Click(object sender, EventArgs e)
    {
        try
        {
            if (bsTotal.InnerText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
                return;
            }

            //if (Convert.ToDecimal(btnBoatBooking.Text.Trim()) <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
            //    return;
            //}

            ViewState["OtherChargeSum"] = "0";
            ViewState["OtherTaxSum"] = "0";

            BindAddBoatCategoryList();
            divBack.Style.Add("background-color", "#4f94efab");
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    // ***** Other Services ***** //

    protected void btnOther_Click(object sender, EventArgs e)
    {
        try
        {

            if (bsTotal.InnerText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
                return;
            }

            //if (Convert.ToDecimal(btnBoatBooking.Text.Trim()) <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
            //    return;
            //}

            ViewState["OtherChargeSum"] = "0";
            ViewState["OtherTaxSum"] = "0";

            BindOtherCategoryList();
            divBack.Style.Add("background-color", "#9ACD32");
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void GetOtherServiceDtl()
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
                    BoatHouseId = Session["BoatHouseId"].ToString()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("OtherServicesDetails", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var OServ = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(OServ)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(OServ)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlOther.DataSource = dt;
                            dtlOther.DataBind();

                            dtlBoat.Visible = false;
                            dtlOther.Visible = true;
                        }
                        else
                        {
                            dtlBoat.Visible = true;
                            dtlOther.Visible = false;

                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Service Details Not Found...!');", true);
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

    protected void ImgBtnDeleteOther_Click(object sender, EventArgs e)
    {
        try
        {




            ViewState["OtherChargeSum"] = "0";
            ViewState["OtherTaxSum"] = "0";



            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sUniqueId = gvOther.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();



            DataTable dtCurrentTableO = (DataTable)ViewState["CartRowO"];



            for (int i = dtCurrentTableO.Rows.Count - 1; i >= 0; i--)
            {
                DataRow drO = dtCurrentTableO.Rows[i];
                if (drO["UniqueId"].ToString().Trim() == sUniqueId.Trim())
                {
                    drO.Delete();
                }
            }



            dtCurrentTableO.AcceptChanges();



            DataTable dtCurrentTable1O = (DataTable)ViewState["RowO"];



            for (int i = dtCurrentTable1O.Rows.Count - 1; i >= 0; i--)
            {
                DataRow drO = dtCurrentTable1O.Rows[i];
                if (drO["UniqueId"].ToString().Trim() == sUniqueId.Trim())
                {
                    drO.Delete();
                }
            }



            dtCurrentTableO.AcceptChanges();
            dtCurrentTable1O.AcceptChanges();



            if (dtCurrentTableO.Rows.Count > 0)
            {
                gvOther.Visible = true;
                gvOther.DataSource = dtCurrentTableO;
                gvOther.DataBind();



                DataTable dtTableO = (DataTable)ViewState["RowO"];



                decimal dServiceTotalAmount = dtTableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItem")));
                ViewState["OtherChargeSum"] = Convert.ToDecimal(dServiceTotalAmount).ToString();



                decimal dChargePerItemTax = dtTableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItemTax")));



                ViewState["OtherTaxSum"] = (Convert.ToDecimal(dChargePerItemTax)).ToString();
            }
            else
            {
                gvOther.DataBind();



                ViewState["CartRowO"] = null;
                ViewState["RowO"] = null;
                gvOther.Visible = false;
            }



            CalculateSummary();
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    private void CalculateSummary()
    {
        bschar1.InnerText = ViewState["BoatChargeSum"].ToString();

        bsgst1.InnerText = (Convert.ToDecimal(ViewState["BoatTaxSum"].ToString()) + Convert.ToDecimal(ViewState["OtherTaxSum"].ToString())).ToString();
        bsdeposit1.InnerText = ViewState["BoatDepositSum"].ToString();
        toolTipDpt.Visible = true;
        toolTipDpt.InnerText = "Charge Includes Refundable Deposit of ₹ " + ViewState["BoatDepositSum"].ToString();


        bsTotal.InnerText = ViewState["BoatTotalSum"].ToString();

        btnBoatBooking.Text = (Convert.ToDecimal(bschar1.InnerText) + Convert.ToDecimal(bsgst1.InnerText) + Convert.ToDecimal(bsdeposit1.InnerText)).ToString();

        bsTotal.InnerText = btnBoatBooking.Text;

        if (Convert.ToDecimal(ViewState["BoatTotalSum"].ToString()) >= 1)
        {
            trMobileNo.Visible = true;
        }
        else
        {
            txtMobileNo.Text = "";
            trMobileNo.Visible = false;
            txtMobileNo.Enabled = true;

            bschar1.InnerText = "";
            bsgst1.InnerText = "";
            bsdeposit1.InnerText = "";
            bsTotal.InnerText = "";
            btnBoatBooking.Text = "";

            lblEmailId.Text = "";
            lblName.Text = "";
            divBooking.Visible = false;
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

                var CatType = new CategoryService()
                {
                    QueryType = "GetOtherCategoryList",
                    ServiceType = Session["UserRole"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = Session["UserId"].ToString().Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", CatType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["ServiceType"] = "OS";

                        dtlOther.DataSource = dtExists;
                        dtlOther.DataBind();

                        dtlBoat.Visible = false;
                        dtlOther.Visible = true;
                    }
                    else
                    {
                        dtlBoat.Visible = true;
                        dtlOther.Visible = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Service Details Not Found !');", true);
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

    protected void DtlOther_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {


            if (e.Item.DataItem != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblOthCatId = (Label)e.Item.FindControl("lblOthCatId");
                Label lblOthCatName = (Label)e.Item.FindControl("lblOthCatName");

                var OthServiceId = e.Item.FindControl("dtlOtherChild") as DataList;

                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var service = new Bookingotherservices()
                        {
                            Category = lblOthCatId.Text.Trim(),
                            BoatHouseId = Session["BoatHouseId"].ToString()
                        };

                        HttpResponseMessage response;

                        if (ViewState["ServiceType"].ToString().Trim() == "OS")
                        {
                            response = client.PostAsJsonAsync("OtherSvcCatDet", service).Result;
                        }
                        else
                        {
                            response = client.PostAsJsonAsync("OtherBoatSvcCatDet", service).Result;
                        }

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
                                    OthServiceId.DataSource = dt;
                                    OthServiceId.DataBind();
                                }
                                else
                                {
                                    OthServiceId.DataBind();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Details Not Found !');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert(" + ResponseMsg + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
                    dvContent.Attributes.Add("style", "display:none;");
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void dtlOtherChild_ItemCommand(object source, DataListCommandEventArgs e)
    {


        string CategoryName = string.Empty;
        string ServiceId = string.Empty;
        string ServiceName = string.Empty;
        string ChargePerItem = string.Empty;

        string ServiceTotalAmount = string.Empty;
        string ChargePerItemTax = string.Empty;
        string TaxId = string.Empty;
        string TaxName = string.Empty;

        string AdultCount = string.Empty;

        Label lblCategoryName = (Label)e.Item.FindControl("lblCategoryName");
        CategoryName = lblCategoryName.Text;

        Label lblServiceId = (Label)e.Item.FindControl("lblOthServiceId");
        ServiceId = lblServiceId.Text;

        Label lblServiceName = (Label)e.Item.FindControl("lblOthServiceName");
        ServiceName = lblServiceName.Text;

        Label lblServiceTotalAmount = (Label)e.Item.FindControl("lblServiceTotalAmount");
        ServiceTotalAmount = lblServiceTotalAmount.Text;

        Label lblChargePerItem = (Label)e.Item.FindControl("lblChargePerItem");
        ChargePerItem = lblChargePerItem.Text;

        Label lblChargePerItemTax = (Label)e.Item.FindControl("lblChargePerItemTax");
        ChargePerItemTax = lblChargePerItemTax.Text;

        Label lblTaxId = (Label)e.Item.FindControl("lblTaxId");
        TaxId = lblTaxId.Text;

        Label lblTaxName = (Label)e.Item.FindControl("lblTaxName");
        TaxName = lblTaxName.Text;

        AdultCount = "1";

        BindDataDynamicValueOthers(ServiceId, CategoryName + " - " + ServiceName, ServiceTotalAmount, ChargePerItem, ChargePerItemTax, AdultCount, TaxId, TaxName);
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    private void BindDataDynamicValueOthers(string ServiceId, string ServiceName, string ServiceTotalAmount, string ChargePerItem,
        string ChargePerItemTax, string AdultCount, string TaxId, string TaxName)
    {
        DataTable mytableO = new DataTable();

        if (ViewState["RowO"] != null)
        {
            mytableO = (DataTable)ViewState["RowO"];
            DataRow dr = null;

            DataRow[] fndUniqueId = mytableO.Select("UniqueId = '" + ServiceId.Trim() + "'");


            if (mytableO.Rows.Count > 0)
            {
                dr = mytableO.NewRow();

                dr["UniqueId"] = ServiceId.Trim();
                dr["ServiceId"] = ServiceId.Trim();
                dr["ServiceName"] = ServiceName.Trim();

                dr["ServiceTotalAmount"] = ServiceTotalAmount.Trim();
                dr["ChargePerItem"] = ChargePerItem.Trim();
                dr["ChargePerItemTax"] = ChargePerItemTax.Trim();
                dr["AdultCount"] = AdultCount;

                dr["TaxId"] = TaxId;
                dr["TaxName"] = TaxName.Trim();

                dr["OtherGrandTotalAmount"] = 0;

                mytableO.Rows.Add(dr);

                ViewState["RowO"] = mytableO;

            }
        }
        else
        {
            mytableO.Columns.Add(new DataColumn("UniqueId", typeof(string)));
            mytableO.Columns.Add(new DataColumn("ServiceId", typeof(string)));
            mytableO.Columns.Add(new DataColumn("ServiceName", typeof(string)));

            mytableO.Columns.Add(new DataColumn("ServiceTotalAmount", typeof(decimal)));
            mytableO.Columns.Add(new DataColumn("ChargePerItem", typeof(decimal)));
            mytableO.Columns.Add(new DataColumn("ChargePerItemTax", typeof(decimal)));
            mytableO.Columns.Add(new DataColumn("AdultCount", typeof(Int32)));

            mytableO.Columns.Add(new DataColumn("TaxId", typeof(string)));
            mytableO.Columns.Add(new DataColumn("TaxName", typeof(string)));

            mytableO.Columns.Add(new DataColumn("OtherGrandTotalAmount", typeof(decimal)));


            DataRow dr1 = mytableO.NewRow();
            dr1 = mytableO.NewRow();

            dr1["UniqueId"] = ServiceId.Trim();
            dr1["ServiceId"] = ServiceId.Trim();
            dr1["ServiceName"] = ServiceName.Trim();

            dr1["ServiceTotalAmount"] = ServiceTotalAmount.Trim();
            dr1["ChargePerItem"] = ChargePerItem.Trim();
            dr1["ChargePerItemTax"] = ChargePerItemTax.Trim();
            dr1["AdultCount"] = AdultCount;

            dr1["TaxId"] = TaxId;
            dr1["TaxName"] = TaxName.Trim();

            dr1["OtherGrandTotalAmount"] = 0;

            mytableO.Rows.Add(dr1);

            ViewState["RowO"] = mytableO;
        }


        if (mytableO.Rows.Count > 0)
        {

            ViewState["OtherChargeSum"] = "0";
            ViewState["OtherTaxSum"] = "0";

            decimal dChargePerItem = mytableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItem")));
            decimal dChargePerItemTax = mytableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItemTax")));

            ViewState["OtherChargeSum"] = (Convert.ToDecimal(dChargePerItem)).ToString();
            ViewState["OtherTaxSum"] = (Convert.ToDecimal(dChargePerItemTax)).ToString();

            CalculateSummary();
        }

        DataTable dtsO = mytableO.Clone();

        var CartTableO = (from row in mytableO.AsEnumerable()
                          group row by new
                          {
                              UniqueId = row.Field<string>("UniqueId"),
                              ServiceId = row.Field<string>("ServiceId"),
                              ServiceName = row.Field<string>("ServiceName"),

                              //ServiceTotalAmount = row.Field<decimal>("ServiceTotalAmount"),
                              ChargePerItem = row.Field<decimal>("ChargePerItem"),
                              ChargePerItemTax = row.Field<decimal>("ChargePerItemTax"),
                              //AdultCount = row.Field<Int32>("AdultCount"),                            

                              TaxId = row.Field<string>("TaxId"),
                              TaxName = row.Field<string>("TaxName"),

                              // OtherGrandTotalAmount = row.Field<decimal>("OtherGrandTotalAmount")

                          } into t1
                          select new
                          {
                              UniqueId = t1.Key.UniqueId,
                              ServiceId = t1.Key.ServiceId,
                              ServiceName = t1.Key.ServiceName,

                              //ServiceTotalAmount = t1.Key.ServiceTotalAmount,
                              ChargePerItem = t1.Key.ChargePerItem,
                              ChargePerItemTax = t1.Key.ChargePerItemTax,
                              AdultCount = t1.Sum(a => a.Field<Int32>("AdultCount")),

                              TaxId = t1.Key.TaxId,
                              TaxName = t1.Key.TaxName,

                              OtherGrandTotalAmount = t1.Sum(a => a.Field<decimal>("ServiceTotalAmount")),

                          })
             .Select(g =>
             {
                 var h = dtsO.NewRow();
                 h["UniqueId"] = g.UniqueId;
                 h["ServiceId"] = g.ServiceId;
                 h["ServiceName"] = g.ServiceName;

                 //h["ServiceTotalAmount"] = g.ServiceTotalAmount;
                 h["ChargePerItem"] = g.ChargePerItem;
                 h["ChargePerItemTax"] = g.ChargePerItemTax;

                 h["AdultCount"] = g.AdultCount;

                 h["OtherGrandTotalAmount"] = g.OtherGrandTotalAmount;

                 h["TaxId"] = g.TaxId;
                 h["TaxName"] = g.TaxName;
                 return h;
             }).CopyToDataTable();


        if (CartTableO.Rows.Count > 0)
        {
            ViewState["CartRowO"] = CartTableO;

            gvOther.Visible = true;
            gvOther.DataSource = CartTableO;
            gvOther.DataBind();
        }
        else
        {
            gvOther.Visible = false;
            gvOther.DataBind();
        }
    }

    protected void txtPIN_TextChanged(object sender, EventArgs e)
    {

        if (txtPIN.Text.Length == txtPIN.MaxLength)
        {
            GetBookingPinDetail();
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    public void GetBookingPinDetail()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var PinCheck = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Bookingpin = txtPIN.Text.Trim(),
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy")
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CheckPinTransaction", PinCheck).Result;

                if (response.IsSuccessStatusCode)
                {
                    var pinstatus = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(pinstatus)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(pinstatus)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["ResponseCode"].ToString() == "1")
                            {
                                ViewState["CUserId"] = dt.Rows[0]["UserId"].ToString();
                                ViewState["CName"] = dt.Rows[0]["Name"].ToString();
                                ViewState["CMailId"] = dt.Rows[0]["MailId"].ToString();

                                imgPinStatus.ImageUrl = "~/images/tick.png";
                                lblUserMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                                lblUserMobileNo.ForeColor = System.Drawing.Color.Black;
                                txtPIN.ReadOnly = true;
                                divBooking.Visible = false;
                                btnDefaultPin.Visible = false;
                                ViewState["PINType"] = "U";

                                //if (Session["DeptPaymentRights"].ToString().Trim() == "N")
                                //{
                                //    //ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                                //    //ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
                                //}
                                //else
                                //{
                                //    if (Session["DeptOnlineRights"].ToString().Trim() == "N")
                                //    {
                                //        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                                //    }

                                //    if (Session["DeptUPIRights"].ToString().Trim() == "N")
                                //    {
                                //        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                                //    }
                                //}
                            }
                            else
                            {
                                ViewState["CUserId"] = dt.Rows[0]["UserId"].ToString();
                                ViewState["CName"] = dt.Rows[0]["Name"].ToString();
                                ViewState["CMailId"] = dt.Rows[0]["MailId"].ToString();

                                imgPinStatus.ImageUrl = "~/images/Remove.png";
                                lblUserMobileNo.Text = dt.Rows[0]["ResponseMsg"].ToString();
                                lblUserMobileNo.ForeColor = System.Drawing.Color.Red;
                                lblUserMobileNo.Font.Size = 14;


                                txtPIN.ReadOnly = false;
                                divBooking.Visible = false;
                                btnDefaultPin.Visible = false;
                                ViewState["PINType"] = "";
                            }
                        }
                        else
                        {
                            dtlBoat.DataBind();
                        }
                    }
                    else
                    {

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

    protected void btnDefaultPin_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    UserId = Session["UserId"].ToString().Trim(),
                    MobileNo = "",
                    BookingType = "D",
                    BookingMedia = "DW"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("SavePinTransaction", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        txtPIN.Text = ResponseMsg;
                        txtPIN.ReadOnly = true;
                        divBooking.Visible = false;
                        lblUserMobileNo.Text = "";
                        imgPinStatus.ImageUrl = "";
                        btnDefaultPin.Visible = false;

                        ViewState["CUserId"] = "";
                        ViewState["CName"] = "";
                        ViewState["PINType"] = "D";
                        ViewState["CMailId"] = "";

                        //ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                        //ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
                    }
                    else
                    {
                        txtPIN.Text = "";
                        txtPIN.ReadOnly = false;
                        divBooking.Visible = false;
                        btnDefaultPin.Visible = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    /// <summary>
    /// Modified Abhinaya
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    //public Boolean CheckBoatSelectionStatus(string Value)
    //{
    //    if (bschar1.InnerText.Trim() != "")
    //    {
    //        if (Convert.ToDecimal(bschar1.InnerText) > 0)
    //        {
    //            if (Value.Trim() == "N")
    //            {
    //                if (hfBoatNature.Value.Trim() == "N")
    //                {
    //                    return true;
    //                }
    //            }

    //            if (Value.Trim() == "P")
    //            {
    //                if (hfBoatNature.Value.Trim() == "P")
    //                {
    //                    return true;
    //                }
    //            }
    //            if (Value.Trim() == "I")
    //            {
    //                if (hfBoatNature.Value.Trim() == "N")
    //                {
    //                    return true;
    //                }
    //            }

    //            return false;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //public void OnlineBeforeTransactionDetails(string ServiceType, string CustUserId)
    //{
    //    try
    //    {
    //        ViewState["TranStatus"] = "N";

    //        if (bsTotal.InnerText.Trim() == "")
    //        {
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
    //            return;
    //        }

    //        //if (Convert.ToDecimal(btnBoatBooking.Text.Trim()) <= 0)
    //        //{
    //        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
    //        //    return;
    //        //}

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            var BoatSearch = new OnlineBoatSearch()
    //            {
    //                BookingDate = DateTime.Now.ToString("dd/MM/yyyy"),
    //                BookingPin = txtPIN.Text.Trim(),

    //                //UserId = ViewState["CUserId"].ToString().Trim(),

    //                UserId = CustUserId.Trim(),
    //                MobileNo = lblUserMobileNo.Text.Trim(),
    //                EmailId = ViewState["CMailId"].ToString().Trim(),

    //                PaymentMode = "Online",
    //                Amount = bsTotal.InnerText.Trim(),
    //                BookingType = "B",

    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
    //                PremiumStatus = hfBoatNature.Value.Trim(),

    //                NoOfPass = "0",
    //                NoOfChild = "0",
    //                NoOfInfant = "0",

    //                BoatTypeId = ViewState["BoatTypeIds"].ToString().Trim('~'),
    //                BoatSeaterId = ViewState["BoatSeaterIds"].ToString().Trim('~'),
    //                BookingDuration = ViewState["BoatMinTimes"].ToString().Trim('~'),

    //                InitBoatCharge = ViewState["InitBoatCharges"].ToString().Trim('~'),
    //                InitRowerCharge = ViewState["RowerMinCharge"].ToString().Trim('~'),
    //                BoatDeposit = ViewState["BoatDeposits"].ToString().Trim('~'),

    //                InitOfferAmount = ViewState["boatOfferAmount"].ToString().Trim('~'),
    //                InitNetAmount = ViewState["InitNetAmounts"].ToString().Trim('~'),
    //                TaxDetails = ViewState["TaxAmountDetlBoat"].ToString().Trim('~'),

    //                // Other Service Booking

    //                OthServiceStatus = ViewState["OthServiceStatus"].ToString().Trim('~'),
    //                OthServiceId = ViewState["OthServiceId"].ToString().Trim('~'),
    //                OthChargePerItem = ViewState["OthChargePerItem"].ToString().Trim('~'),
    //                OthNoOfItems = ViewState["OthNoOfItems"].ToString().Trim('~'),
    //                OthTaxDetails = ViewState["OthTaxDetails"].ToString().Trim('~'),
    //                OthNetAmount = ViewState["OthNetAmount"].ToString().Trim('~'),

    //                BookingMedia = "PW",

    //                BFDInitBoatCharge = "0",
    //                BFDInitNetAmount = "0",
    //                BFDGST = "0",
    //                EntryType = "DB",
    //                ModuleType = "Boating"
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("OnlineBoatBookingBfrTran", BoatSearch).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var BoatLst = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    string[] sResult = ResponseMsg.Split('~');

    //                    ViewState["TranStatus"] = "Y";

    //                    SendSMS(ServiceType, lblUserMobileNo.Text.Trim(), sResult[1].ToString().Trim(), bsTotal.InnerText.Trim());
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
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
                        // ServiceType = "DOnlineBooking",

                        ServiceType = ServiceType.Trim(),
                        BookingId = sTransactionNo.ToString(),
                        MobileNo = sMobileNo.ToString(),
                        MediaType = "DW",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        Remarks = sAmount.ToString()
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("SendSMSMsg", OtpSendSms).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            ViewState["ResendBookingId"] = sTransactionNo.ToString();
                            ViewState["ResendMobNo"] = sMobileNo.ToString();
                            ViewState["ReferenceNo"] = ResponseMsg;
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

            // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    private void CheckBoatAvailableDetails(string sBoatHouseId, string sBoatTypeId, string sBoatSeaterId, string sBookingDate)
    {
        try
        {
            IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new Ticket()
                {
                    QueryType = "GetBoatAvailableDetails",
                    ServiceType = "",
                    BoatHouseId = sBoatHouseId.Trim(),
                    Input1 = sBoatTypeId.Trim(),
                    Input2 = sBoatSeaterId.Trim(),
                    Input3 = DateTime.Parse(sBookingDate.Trim(), objEnglishDate).ToString(),
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["cNormalAvailable"] = dtExists.Rows[0]["NormalAvailable"].ToString();
                        ViewState["cPremiumAvailable"] = dtExists.Rows[0]["PremiumAvailable"].ToString();
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

    protected void lnkGST_Click(object sender, EventArgs e)
    {

        chkEmail.Checked = false;
        chkSMS.Checked = false;
        txtGSTNo.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtEmailId.Text = string.Empty;
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

        ViewState["BookingId"] = GvBoatBooking.DataKeys[gvrow.RowIndex]["BookingId"].ToString().Trim();
        txtEmailId.Text = GvBoatBooking.DataKeys[gvrow.RowIndex]["CustomerEmailId"].ToString().Trim();
        txtGSTNo.Text = GvBoatBooking.DataKeys[gvrow.RowIndex]["CustomerGSTNo"].ToString().Trim();
        txtMobileNo.Text = GvBoatBooking.DataKeys[gvrow.RowIndex]["CustomerMobile"].ToString().Trim();

        if (txtMobileNo.Text != "") { txtMobileNo.Enabled = false; }
        else { txtMobileNo.Enabled = true; }
        if (txtEmailId.Text != "") { txtEmailId.Enabled = false; }
        else { txtEmailId.Enabled = true; }
        if (txtGSTNo.Text != "") { txtGSTNo.Enabled = false; }
        else { txtGSTNo.Enabled = true; }



        MpeTrip.Show();
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    protected void btnPOPCancel_Click(object sender, EventArgs e)
    {

        txtGSTNo.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtEmailId.Text = string.Empty;
        chkEmail.Checked = false;
        chkSMS.Checked = false;
        MpeTrip.Hide();
        BookedListdtl();
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    protected void btnPOPSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtMobileNo.Text == "" && txtEmailId.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Mobile No or Email-Id');", true);

                return;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var Body = new BoatBook()
                {
                    BookingId = ViewState["BookingId"].ToString().Trim(),
                    CustomerMobileNo = txtMobileNo.Text.Trim(),
                    CustomerGSTNo = txtGSTNo.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim(),
                    CustomerEmailId = txtEmailId.Text.Trim()
                };
                response = client.PostAsJsonAsync("UpdateGST/BookingHdr", Body).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BookedListdtl();
                        MpeTrip.Hide();

                        if (chkSMS.Checked == true)
                        {
                            PopUpMobileSMS();
                        }

                        if (chkEmail.Checked == true)
                        {
                            EmailTicketReceipt();
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                        chkEmail.Checked = false;
                        chkSMS.Checked = false;
                    }
                    else
                    {

                        MpeTrip.Hide();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            MpeTrip.Hide();
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void PopUpMobileSMS()
    {
        try
        {

            string BookingId = ViewState["BookingId"].ToString();
            string sMobileNo = txtMobileNo.Text;

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
                        ServiceType = "BBReceipt",
                        BookingId = BookingId.ToString(),
                        MobileNo = sMobileNo.ToString(),
                        MediaType = "DW",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        Remarks = ""
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("SendSMSMsg", OtpSendSms).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('e-Receipt Link Send to Customer Mobile !');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void chkGSTNo_CheckedChanged(object sender, EventArgs e)
    {

        //if (chkGSTNo.Checked == true)
        //{
        //    divGST.Visible = true;
        //}
        //else
        //{
        //    txtINSGSTNO.Text = string.Empty;
        //    divGST.Visible = false;
        //}
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    public void BindAddBoatCategoryList()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new CategoryService()
                {
                    QueryType = "GetAddBoatCategoryList",
                    ServiceType = Session["UserRole"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = Session["UserId"].ToString().Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", CatType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["ServiceType"] = "AB";

                        dtlOther.DataSource = dtExists;
                        dtlOther.DataBind();

                        dtlBoat.Visible = false;
                        dtlOther.Visible = true;
                    }
                    else
                    {
                        //dtlBoat.Visible = true;
                        dtlBoat.Visible = false;
                        dtlOther.Visible = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Additional Tickets Details Not Found !');", true);
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

    // Email Send Receipt Details

    public void GetEmailPwdDetails()
    {
        try
        {
            ViewState["EmailId"] = "";
            ViewState["EPass"] = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "SendEmail",
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
                        ViewState["EmailId"] = dtExists.Rows[0]["EmailId"].ToString().Trim();
                        ViewState["EPass"] = dtExists.Rows[0]["Password"].ToString().Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void EmailTicketReceipt()
    {
        string sQuery = string.Empty;
        string sError = string.Empty;
        string sEmailId = string.Empty;
        string BoatHouseName = string.Empty;
        string TransactionNo = string.Empty;
        string BookingId = string.Empty;
        string BookingType = string.Empty;
        string Amount = string.Empty;

        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        try
        {

            DataSet dstRports = new Receipt();

            BindBoatTickets(ref dstRports);
            BindOtherserviceTicket(ref dstRports);
            BindBoatReceipt(ref dstRports);
            BindBoatTicketRower(ref dstRports);

            objReportDocument.Load(Server.MapPath("../Reports/BookingReceipt.rpt"));
            objReportDocument.SetDataSource(dstRports);
            System.Text.StringBuilder sbText = new System.Text.StringBuilder();

            sbText.Append("<table border=1 width=100% style='border-collapse: collapse; border: thin dashed DeepPink'>");
            sbText.Append("<tr>");
            sbText.Append("<td>");

            sbText.Append("<img src='" + Session["DomainUrl"].ToString() + "Public/images/Payment.png' />");
            sbText.Append("<font size=2 face=verdana />");
            sbText.Append("<br />");
            sbText.Append("<br />");

            sbText.Append("<span style='padding-left: 10px'><b>Dear Customer,</b></span>");
            sbText.Append("<br />");
            sbText.Append("<br />");

            sbText.Append("<p style='padding-left: 10px'>");
            sbText.Append("Thanks for your booking with our Boat House. "
                + " We acknowledge the receipt of your payment through our payment gateway." +
                " <br /> <br /> The details of your booking are stated as below:");
            sbText.Append("</p>");

            sbText.Append("<br />");
            sbText.Append("<br />");

            //sbText.Append("<tr><td>");
            sbText.Append("<p style='padding-left: 10px'>");
            sbText.Append("<font Color=Blue>This is a system generated e-mail, please do not reply to this e-mail. "
                              + "For further details, kindly visit our website " + Session["DomainUrl"].ToString() + "</font>");
            sbText.Append("</p>");

            sbText.Append("<span style='padding-left: 10px'>Enjoy your trip.</span>");


            GetEmailPwdDetails();

            sEmailId = ViewState["EmailId"].ToString().Trim();
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new System.Net.Mail.MailAddress(sEmailId);
            message.To.Add(new System.Net.Mail.MailAddress(txtEmailId.Text.Trim()));

            message.Subject = BoatHouseName.ToString() + " Booking Receipt";
            message.IsBodyHtml = true;
            message.Body = sbText.ToString();

            System.Net.Mail.Attachment atm = new System.Net.Mail.Attachment(objReportDocument.ExportToStream(ExportFormatType.PortableDocFormat), "BookingReceipt.pdf");
            message.Attachments.Add(atm);


            message.Priority = System.Net.Mail.MailPriority.High;

            System.Net.Mail.SmtpClient objClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            objClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            objClient.EnableSsl = true;
            objClient.UseDefaultCredentials = true;
            objClient.Credentials = new System.Net.NetworkCredential(sEmailId, ViewState["EPass"].ToString().Trim());
            objClient.Send(message);

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('e-Receipt Send to Customer Email !');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
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

    private byte[] GetImage(string url)
    {
        Stream stream = null;
        byte[] buf;

        try
        {
            WebProxy myProxy = new WebProxy();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                int len = (int)(response.ContentLength);
                buf = br.ReadBytes(len);
                br.Close();
            }

            stream.Close();
            response.Close();
        }
        catch (Exception ex)
        {
            buf = null;
        }

        return (buf);
    }

    public void BindBoatTickets(ref DataSet dstReports)
    {
        try
        {
            DataRow drwReport;
            string ImageUrl = string.Empty;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = ViewState["BookingId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatBookedTicket", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var ticktList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(ticktList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(ticktList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg); //BookingPin

                        if (dtExists.Rows.Count > 0)
                        {
                            for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                            {
                                drwReport = dstReports.Tables["TicketList"].NewRow();

                                drwReport[0] = dtExists.Rows[iRowIdx]["BookingPin"].ToString();
                                drwReport[1] = dtExists.Rows[iRowIdx]["BookingId"].ToString();
                                drwReport[2] = dtExists.Rows[iRowIdx]["BoatReferenceNo"].ToString();
                                drwReport[3] = dtExists.Rows[iRowIdx]["BookingDate"].ToString();
                                drwReport[4] = dtExists.Rows[iRowIdx]["BoatHouseName"].ToString();
                                drwReport[5] = dtExists.Rows[iRowIdx]["PremiumStatus"].ToString();
                                drwReport[6] = dtExists.Rows[iRowIdx]["BoatType"].ToString();
                                drwReport[7] = dtExists.Rows[iRowIdx]["SeaterType"].ToString();
                                drwReport[8] = dtExists.Rows[iRowIdx]["ActualBoatNum"].ToString();
                                drwReport[9] = dtExists.Rows[iRowIdx]["InitRowerCharge"].ToString();
                                drwReport[10] = dtExists.Rows[iRowIdx]["ExpectedTime"].ToString();
                                drwReport[11] = dtExists.Rows[iRowIdx]["InitNetAmount"].ToString();
                                drwReport[12] = dtExists.Rows[iRowIdx]["TripEndTime"].ToString();
                                drwReport[13] = dtExists.Rows[iRowIdx]["TripFlag"].ToString();

                                LoadBarcodeImage(dtExists.Rows[iRowIdx]["BookingPin"].ToString().Trim(),
                                    dtExists.Rows[iRowIdx]["BookingId"].ToString().Trim(),
                                    dtExists.Rows[iRowIdx]["BoatReferenceNo"].ToString().Trim(), ref ImageUrl);

                                StringBuilder _sb = new StringBuilder();
                                Byte[] _byte = this.GetImage(ImageUrl);
                                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                                string sbBase64 = _sb.ToString();

                                drwReport[14] = _byte;

                                dstReports.Tables["TicketList"].Rows.Add(drwReport);
                            }
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindBoatTicketRower(ref DataSet dstReports)
    {
        try
        {
            DataRow drwReport;
            string ImageUrl = string.Empty;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = ViewState["BookingId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatBookedTicket", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var ticktList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(ticktList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(ticktList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg); //BookingPin

                        if (dtExists.Rows.Count > 0)
                        {
                            for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                            {
                                drwReport = dstReports.Tables["TicketListRower"].NewRow();

                                drwReport[0] = dtExists.Rows[iRowIdx]["BookingPin"].ToString();
                                drwReport[1] = dtExists.Rows[iRowIdx]["BookingId"].ToString();
                                drwReport[2] = dtExists.Rows[iRowIdx]["BoatReferenceNo"].ToString();
                                drwReport[3] = dtExists.Rows[iRowIdx]["BookingDate"].ToString();
                                drwReport[4] = dtExists.Rows[iRowIdx]["BoatHouseName"].ToString();
                                drwReport[5] = dtExists.Rows[iRowIdx]["PremiumStatus"].ToString();
                                drwReport[6] = dtExists.Rows[iRowIdx]["BoatType"].ToString();
                                drwReport[7] = dtExists.Rows[iRowIdx]["SeaterType"].ToString();
                                drwReport[8] = dtExists.Rows[iRowIdx]["ActualBoatNum"].ToString();
                                drwReport[9] = dtExists.Rows[iRowIdx]["InitRowerCharge"].ToString();
                                drwReport[10] = dtExists.Rows[iRowIdx]["ExpectedTime"].ToString();
                                drwReport[11] = dtExists.Rows[iRowIdx]["InitNetAmount"].ToString();
                                drwReport[12] = dtExists.Rows[iRowIdx]["TripEndTime"].ToString();
                                drwReport[13] = dtExists.Rows[iRowIdx]["TripFlag"].ToString();

                                LoadBarcodeImage(dtExists.Rows[iRowIdx]["BookingPin"].ToString().Trim(),
                                    dtExists.Rows[iRowIdx]["BookingId"].ToString().Trim(),
                                    dtExists.Rows[iRowIdx]["BoatReferenceNo"].ToString().Trim(), ref ImageUrl);

                                StringBuilder _sb = new StringBuilder();
                                Byte[] _byte = this.GetImage(ImageUrl);
                                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                                string sbBase64 = _sb.ToString();

                                drwReport[14] = _byte;

                                dstReports.Tables["TicketListRower"].Rows.Add(drwReport);
                            }
                        }
                    }
                    else
                    {
                        //  ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);

                    }
                }
                else
                {
                    //  ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindOtherserviceTicket(ref DataSet dstReports)
    {
        try
        {
            DataRow drwReport;
            string ImageUrl = string.Empty;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = ViewState["BookingId"].ToString().Trim(),
                    BookingType = "B"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatOtherTicket", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var ticktList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(ticktList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(ticktList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg); //BookingPin

                        if (dtExists.Rows.Count > 0)
                        {
                            for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                            {
                                drwReport = dstReports.Tables["TicketOtherList"].NewRow();

                                drwReport[0] = dtExists.Rows[iRowIdx]["BookingId"].ToString();
                                drwReport[1] = dtExists.Rows[iRowIdx]["BookingDate"].ToString();
                                drwReport[2] = dtExists.Rows[iRowIdx]["BoatHouseName"].ToString();
                                drwReport[3] = dtExists.Rows[iRowIdx]["ServiceName"].ToString();
                                drwReport[4] = dtExists.Rows[iRowIdx]["NetAmount"].ToString();
                                drwReport[5] = dtExists.Rows[iRowIdx]["NoOfItems"].ToString();
                                drwReport[6] = dtExists.Rows[iRowIdx]["ChargePerItem"].ToString();
                                drwReport[7] = dtExists.Rows[iRowIdx]["ServiceFare"].ToString();
                                drwReport[8] = dtExists.Rows[iRowIdx]["ServiceId"].ToString();

                                LoadBarcodeImageOther(dtExists.Rows[iRowIdx]["ServiceId"].ToString().Trim(),
                                    dtExists.Rows[iRowIdx]["BookingId"].ToString().Trim(), ref ImageUrl);

                                StringBuilder _sb = new StringBuilder();
                                Byte[] _byte = this.GetImage(ImageUrl);
                                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                                string sbBase64 = _sb.ToString();

                                drwReport[9] = _byte;

                                dstReports.Tables["TicketOtherList"].Rows.Add(drwReport);
                            }
                        }
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);

                    }
                }
                else
                {
                    // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindBoatReceipt(ref DataSet dstReports)
    {
        try
        {
            DataRow drwReport;
            string ImageUrl = string.Empty;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    QueryType = "BoatPrintTicketBulkReceipts",
                    ServiceType = "",
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BookingId = ViewState["BookingId"].ToString().Trim(),
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
                        for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                        {
                            drwReport = dstReports.Tables["Receipt"].NewRow();

                            drwReport[0] = dtExists.Rows[iRowIdx]["BookingDate"].ToString();
                            drwReport[1] = dtExists.Rows[iRowIdx]["BoatHouseName"].ToString();
                            drwReport[2] = dtExists.Rows[iRowIdx]["PremiumStatus"].ToString();
                            drwReport[3] = dtExists.Rows[iRowIdx]["BookingId"].ToString();
                            drwReport[4] = dtExists.Rows[iRowIdx]["CustomerName"].ToString();
                            drwReport[5] = dtExists.Rows[iRowIdx]["CustomerMobile"].ToString();
                            drwReport[6] = dtExists.Rows[iRowIdx]["PaymentTypeName"].ToString();
                            drwReport[7] = dtExists.Rows[iRowIdx]["BFDInitBoatCharge"].ToString();
                            drwReport[8] = dtExists.Rows[iRowIdx]["BFDInitRowerCharge"].ToString();
                            drwReport[9] = dtExists.Rows[iRowIdx]["OtherService"].ToString();
                            drwReport[10] = dtExists.Rows[iRowIdx]["BoatDeposit"].ToString();
                            drwReport[11] = dtExists.Rows[iRowIdx]["BFDTaxAmount"].ToString();
                            drwReport[12] = dtExists.Rows[iRowIdx]["BFDInitNetAmount"].ToString();
                            drwReport[13] = dtExists.Rows[iRowIdx]["InitOfferAmount"].ToString();
                            drwReport[14] = dtExists.Rows[iRowIdx]["InitNetAmount"].ToString();
                            drwReport[15] = dtExists.Rows[iRowIdx]["GSTNumber"].ToString();
                            drwReport[16] = dtExists.Rows[iRowIdx]["CustomerGSTNo"].ToString();
                            // drwReport[17] = dtExists.Rows[iRowIdx]["InstructionDtl"].ToString();
                            // drwReport[18] = dtExists.Rows[iRowIdx]["TTDCLogo"].ToString();

                            LoadBarcodeImageReceipt(dtExists.Rows[iRowIdx]["BookingId"].ToString().Trim(), ref ImageUrl);

                            StringBuilder _sb = new StringBuilder();
                            Byte[] _byte = this.GetImage(ImageUrl);
                            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                            string sbBase64 = _sb.ToString();

                            drwReport[19] = _byte;
                            string CorpLogo = Session["CorpLogo"].ToString();
                            using (var webClient = new WebClient())
                            {
                                byte[] imageBytes = webClient.DownloadData(CorpLogo);
                                drwReport[20] = imageBytes;

                            }
                            dstReports.Tables["Receipt"].Rows.Add(drwReport);
                        }
                    }

                }
                else
                {
                    // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void LoadBarcodeImage(string BookingPin, string BookingId, string BookingRef, ref string imgresponse)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var QRd = new QRBoat()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BookingId = BookingId.Trim(),
                    Pin = BookingPin.Trim(),
                    BookingRef = BookingRef.Trim()
                };


                HttpResponseMessage response = client.PostAsJsonAsync("PassQR", QRd).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                    string ResponseMsg2 = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                    if (Status == "Success.")
                    {
                        imgresponse = ResponseMsg2;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void LoadBarcodeImageOther(string ServiceId, string BookingId, ref string imgOtherResponse)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var QRd = new QR()
                {
                    ServiceId = ServiceId.Trim(),
                    BookingId = BookingId.Trim(),
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
                        imgOtherResponse = ResponseMsg;
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

    public void LoadBarcodeImageReceipt(string BookingId, ref string imgReceiptResponse)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var QRd = new QRBoat()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BookingId = BookingId.Trim(),
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
                        imgReceiptResponse = ResponseMsg;
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

    public void BindUserCountTotal()
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
                    var BoatSearch = new BoatSearch()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingType = "B",
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "Admin"
                    };
                    response = client.PostAsJsonAsync("BoatBookedList", BoatSearch).Result;
                }
                else
                {
                    var BoatSearch = new BoatSearch()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingType = "B",
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("BoatBookedList", BoatSearch).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {

                            gvUserCountTotal.DataSource = dt;
                            gvUserCountTotal.DataBind();

                            decimal TotalCount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("InitBillAmount")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("OtherService")));

                            gvUserCountTotal.FooterRow.Cells[5].Text = "Total";
                            gvUserCountTotal.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;

                            gvUserCountTotal.FooterRow.Cells[6].Text = TotalCount.ToString();
                            gvUserCountTotal.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                            gvUserCountTotal.FooterRow.Cells[7].Text = TotalAmount.ToString("N2");
                            gvUserCountTotal.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;


                            gvUserCountTotal.Visible = true;
                            MpeUserCount.Show();


                        }
                        else
                        {
                            MpeUserCount.Hide();
                            gvUserCountTotal.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found');", true);
                        }
                    }
                    else
                    {
                        MpeUserCount.Hide();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    MpeUserCount.Hide();
                }
            }
        }
        catch (Exception ex)
        {
            MpeUserCount.Hide();
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void bblblCount_Click(object sender, EventArgs e)
    {

        BindUserCountTotal();
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    protected void gvUserCountTotal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
        gvUserCountTotal.PageIndex = e.NewPageIndex;
        BindUserCountTotal();
    }

    protected void ImgClose_Click(object sender, ImageClickEventArgs e)
    {

        MpeUserCount.Hide();
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    public void BindBookingCountAmount()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sCreatedBy = string.Empty;

                if (Session["UserRole"].ToString() == "SAdmin" || Session["UserRole"].ToString() == "ADMIN"
                    || Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString() == "Admin")
                {
                    sCreatedBy = "0";
                }
                else
                {
                    sCreatedBy = Session["UserId"].ToString().Trim();
                }

                var BoatSearch = new BoatSearch()
                {
                    CreatedBy = sCreatedBy,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy")
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BookingDetailsCount", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        bblblCount.Text = dtExists.Rows[0]["Count"].ToString();
                        bblblAmount.Text = dtExists.Rows[0]["Amount"].ToString();
                        bblblDeposit.Text = dtExists.Rows[0]["Deposit"].ToString();
                        bblblNetAmount.Text = dtExists.Rows[0]["NetAmount"].ToString();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void grvMergeHeader_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 5;
            HeaderCell.Text = "Boat Booked Details";
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 8;
            HeaderGridRow.Cells.Add(HeaderCell);


            GridView HeaderGrid1 = (GridView)sender;
            GridViewRow HeaderGridRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Boat Details";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Max Available Trips";
            HeaderCell1.ColumnSpan = 4;
            HeaderGridRow1.Cells.Add(HeaderCell1);




            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.ColumnSpan = 2;
            HeaderCell1.Text = "Online";
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.ColumnSpan = 2;
            HeaderCell1.Text = "Boat House";
            HeaderGridRow1.Cells.Add(HeaderCell1);



            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Waiting Trip";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Total";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Trip Completed";
            HeaderCell1.ColumnSpan = 4;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            gvAvailableBoats.Controls[0].Controls.AddAt(0, HeaderGridRow1);
            gvAvailableBoats.Controls[0].Controls.AddAt(0, HeaderGridRow);

        }


    }

    public void AvailableBoat()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var AvailableBoats = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    UserId = Session["UserId"].ToString().Trim(),
                    UserRole = Session["UserRole"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("NewBoatBookingRights", AvailableBoats).Result;

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

                            gvAvailableBoats.Visible = true;
                            gvAvailableBoats.DataSource = dt;
                            gvAvailableBoats.DataBind();
                        }
                        else
                        {
                            gvAvailableBoats.DataBind();
                            return;
                        }
                    }
                    else
                    {
                        gvAvailableBoats.DataBind();

                        return;
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

    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "CloseToggle();", true);

    }

    protected void gvAvailableBoats_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)

            {

                Label lblBoatTypeId = (Label)e.Row.FindControl("lblBoatTypeId");

                GridView gv_Child = (GridView)e.Row.FindControl("gv_Child");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response;
                    var BoatSearch = new BoatSearch()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        PremiumStatus = "N",
                        BoatTypeId = lblBoatTypeId.Text.Trim('~'),
                        BookingDate = DateTime.Today.ToString("dd/MM/yyyy")

                    };
                    response = client.PostAsJsonAsync("BoatBooking/BoatAvailableList", BoatSearch).Result;

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
                                gv_Child.DataSource = dt;
                                gv_Child.DataBind();

                                Int16 AvailTrips = 0;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string F = dt.Rows[i].ToString();
                                    AvailTrips += Int16.Parse(dt.Rows[i]["RemainTrips"].ToString());
                                }


                                gv_Child.FooterRow.Cells[2].Text = "Total";
                                gv_Child.FooterRow.Cells[2].Font.Bold = true;
                                gv_Child.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                                gv_Child.FooterRow.Cells[2].Font.Size = 10;

                                gv_Child.FooterRow.Cells[3].Text = AvailTrips.ToString();
                                gv_Child.FooterRow.Cells[3].Font.Bold = true;
                                gv_Child.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                gv_Child.FooterRow.Cells[3].Font.Size = 10;
                            }
                            else
                            {
                                gv_Child.DataBind();
                                return;
                            }
                        }
                        else
                        {
                            gv_Child.DataBind();
                            return;
                        }
                    }
                    else
                    {
                        var Errorresponse = response.Content.ReadAsStringAsync().Result;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    // Customer Mobile No Conditions

    protected void txtCustMobileNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtCustMobileNo.Text != "")
            {
                if (txtCustMobileNo.Text.Length == txtCustMobileNo.MaxLength)
                {
                    DefaultPinGeneration();
                    lblUserMobileNo.Text = txtCustMobileNo.Text;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void DefaultPinGeneration()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    UserId = Session["UserId"].ToString().Trim(),
                    MobileNo = "",
                    BookingType = "D",
                    BookingMedia = "DW"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("SavePinTransaction", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        txtPIN.Text = ResponseMsg;
                        txtPIN.ReadOnly = true;
                        divBooking.Visible = false;
                        lblUserMobileNo.Text = "";
                        imgPinStatus.ImageUrl = "";
                        btnDefaultPin.Visible = false;

                        ViewState["CUserId"] = "";
                        ViewState["CName"] = "";
                        ViewState["PINType"] = "D";
                        ViewState["CMailId"] = "";

                        //ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                        //ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
                    }
                    else
                    {
                        txtPIN.Text = "";
                        txtPIN.ReadOnly = false;
                        divBooking.Visible = false;
                        btnDefaultPin.Visible = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void GetUserName()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatHouseId = new BoatSearch()
                {
                    UserId = Session["UserId"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("getUserName", BoatHouseId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatHouseresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatHouseresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatHouseresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            string UserName = dt.Rows[0]["UserName"].ToString();
                            ViewState["EmpName"] = UserName.ToString().Trim();

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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void RsnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            if (ddlReason.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Select Reason');", true);
                return;
            }

            GetUserName();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var EmpMstr = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    UserId = Session["UserId"].ToString().Trim(),
                    UserName = ViewState["EmpName"].ToString().Trim(),
                    ServiceType = "Boat Booking",
                    BookingId = ViewState["BoOkingID"].ToString().Trim(),
                    Reason = ddlReason.SelectedItem.Text,
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ReprintReason", EmpMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        Mpepnlrsn.Hide();
                        Response.Redirect("PrintBoat.aspx?rt=b&bi=" + ViewState["BoOkingID"].ToString().Trim() + "");
                        ViewState["BoOkingID"] = null;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message + "');", true);
            return;
        }
    }

    protected void CloseRsnButton_Click(object sender, ImageClickEventArgs e)
    {
        Mpepnlrsn.Hide();
    }

    protected void gvBoatdtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btn = (LinkButton)e.Row.FindControl("lblSlot");
            if (ViewState["SlotFlag"].ToString() == "Y")
            {

                btn.Enabled = false;
            }
            else
            {

                btn.Enabled = true;
            }
        }
    }

    protected void ChkAvailBoat_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkAvailBoat.Checked == true)
        {
            dvContent.Attributes.Add("style", "display:block;");
            AvailableBoat();
        }



    }


    public void BoatAvailableLists()
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
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
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
                        if (dtExists.Rows[0]["Normal"].ToString() == "" || dtExists.Rows[0]["Normal"].ToString() == "0")
                        {
                            btnNor.Visible = false;

                        }
                        if (dtExists.Rows[0]["Express"].ToString() == "" || dtExists.Rows[0]["Express"].ToString() == "0")
                        {
                            btnPre.Visible = false;

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


    public class AvailableBoats
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string BookingDate { get; set; }
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
        public string UserRole { get; set; }

        public string ServiceType { get; set; }
        public string UserName { get; set; }
        public string Reason { get; set; }
        public string CreatedDate { get; set; }

    }
    public class BoatBook
    {
        //Header Details
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public string BookingDate { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PremiumStatus { get; set; }
        public string SingleMultiStatus { get; set; }
        public string NoOfPass { get; set; }
        public string NoOfChild { get; set; }
        public string NoOfInfant { get; set; }
        public string OfferId { get; set; }
        public string InitBillAmount { get; set; }
        public string PaymentType { get; set; }
        public string ActualBillAmount { get; set; }
        public string Status { get; set; }
        public string Bookingpin { get; set; }
        public string CollectedAmount { get; set; }
        public string BalanceAmount { get; set; }

        //Book Detail Details
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string BookingDuration { get; set; }
        public string InitBoatCharge { get; set; }
        public string InitRowerCharge { get; set; }
        public string BoatDeposit { get; set; }
        public string TaxDetails { get; set; }
        public string InitOfferAmount { get; set; }
        public string InitNetAmount { get; set; }
        public string CreatedBy { get; set; }
        public string CustomerGSTNo { get; set; }
        public string CustomerEmailId { get; set; }


        //Other Service
        public string OthServiceStatus { get; set; }
        public string OthServiceId { get; set; }
        public string OthChargePerItem { get; set; }
        public string OthNoOfItems { get; set; }
        public string OthTaxDetails { get; set; }
        public string OthNetAmount { get; set; }
        public string BookingMedia { get; set; }

        public string BFDInitBoatCharge { get; set; }
        public string BFDInitNetAmount { get; set; }
        public string BFDGST { get; set; }
        public string BookingTimeSlotId { get; set; }
        public string BookingBlockId { get; set; }
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

    public class OnlineBoatSearch
    {
        public string QueryType { get; set; }
        public string BoatHouseName { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string PaymentMode { get; set; }
        public string Amount { get; set; }
        public string BookingType { get; set; }
        public string BookingMedia { get; set; }
        public string NoOfPass { get; set; }
        public string NoOfChild { get; set; }
        public string NoOfInfant { get; set; }
        public string[] BoatSeaterId { get; set; }
        public string[] BookingDuration { get; set; }
        public string[] InitBoatCharge { get; set; }
        public string[] InitRowerCharge { get; set; }
        public string[] BoatDeposit { get; set; }
        public string[] TaxDetails { get; set; }
        public string[] InitOfferAmount { get; set; }
        public string[] InitNetAmount { get; set; }
        public string CreatedBy { get; set; }
        public string[] OthServiceStatus { get; set; }
        public string BoatHouseId { get; set; }
        public string PremiumStatus { get; set; }
        public string[] BoatPremiumStatus { get; set; }
        public string OfferId { get; set; }
        public string[] BoatTypeId { get; set; }
        public string BookingDate { get; set; }
        public string TaxId { get; set; }
        public string ValidDate { get; set; }
        public string BookingId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceId { get; set; }
        public string[] OthServiceId { get; set; }
        public string[] OthChargePerItem { get; set; }
        public string[] OthNoOfItems { get; set; }
        public string[] OthChildCharge { get; set; }
        public string[] OthNoOfChild { get; set; }
        public string[] OthTaxDetails { get; set; }
        public string[] OthNetAmount { get; set; }
        public string Category { get; set; }
        public string BookingPin { get; set; }
        public string BFDInitBoatCharge { get; set; }
        public string BFDInitNetAmount { get; set; }
        public string BFDGST { get; set; }
        public string EntryType { get; set; }
        public string ModuleType { get; set; }
        public string[] BookingTimeSlotId { get; set; }
        public string[] BookingBlockId { get; set; }
        public string[] CGSTTaxAmount { get; set; }
        public string[] SGSTTaxAmount { get; set; }
        public string[] CGSTOthTaxAmount { get; set; }
        public string[] SGSTOthTaxAmount { get; set; }
    }

    public class OtpSMS
    {
        public string ServiceType { get; set; }
        public string BookingId { get; set; }
        public string MobileNo { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string Remarks { get; set; }
        public string MediaType { get; set; }
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
    public class CommonClass
    {
        public string BoatHouseId { get; set; }
        public string QueryType { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string Category { get; set; }

        public string CorpId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string ServiceType { get; set; }
    }
    public class CommonMaster
    {
        public string BookingDate { get; set; }
        public string BoatHouseId { get; set; }
        public string BooKingId { get; set; }
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

    }
    public class AvailableBoatBookingSlotDept
    {

        public string QueryType
        {
            get;
            set;
        }

        public string BoatHouseId
        {
            get;
            set;
        }
        public string CheckInDate
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
        public string SlotType
        {
            get;
            set;
        }
        public string BlockId { get; set; }
        public string SlotId { get; set; }

        public string UserId { get; set; }
        public string SlotTime { get; set; }
        public string SlotIdold { get; set; }
    }
    protected void bblblNetAmount_Click(object sender, EventArgs e)
    {
        BindBoatBookingRevenuePopup();
    }

    public void BindBoatBookingRevenuePopup()
    {
        try
        {
            if (bblblAmount.Text != "0")
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string sCreatedBy = string.Empty;
                    string sQueryType = string.Empty;

                    if (Session["UserRole"].ToString() == "SAdmin" || Session["UserRole"].ToString() == "ADMIN"
                       || Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString() == "Admin")
                    {
                        sCreatedBy = "0";
                        sQueryType = "BoatBookingRevenueWithDpt";
                    }
                    else
                    {
                        sCreatedBy = Session["UserId"].ToString().Trim();
                        sQueryType = "BoatBookingRevenueUserWithDpt";
                    }

                    var FormBody = new BoatSearch()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Today.ToString("dd/MM/yyyy"),
                        UserId = sCreatedBy,
                        ToDate = DateTime.Today.ToString("dd/MM/yyyy")
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var GetResponse = response.Content.ReadAsStringAsync().Result;
                        var ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvBBpopup.DataSource = dt;
                            gvBBpopup.DataBind();
                            gvBBpopup.Visible = true;

                            decimal totAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BoatBookingRevenue")));

                            gvBBpopup.FooterRow.Cells[1].Text = "Total";
                            gvBBpopup.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            gvBBpopup.FooterRow.Cells[2].Text = totAmt.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            gvBBpopup.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            MPEBBpopup.Show();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                        MPEBBpopup.Hide();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void bblblAmount_Click(object sender, EventArgs e)
    {
        BindBoatBookingRevenuePopup();
    }

    protected void ImgCloseBB_Click(object sender, ImageClickEventArgs e)
    {
        MPEBBpopup.Hide();
    }

    public string GetBookingIdByPin()
    {
        string BookingId = string.Empty;

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var GetCommonMaster = new CommonMaster()
                {
                    QueryType = "GetBookingIdByPin",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BooKingId = "",
                    ServiceType = "",
                    Input1 = txtPIN.Text.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
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
                        BookingId = dtExists.Rows[0]["BookingId"].ToString().Trim();

                        return BookingId;
                    }
                }

                return BookingId;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return BookingId;
        }
    }
    protected void ImgCloseSlotTime_Click(object sender, ImageClickEventArgs e)
    {
        MpeUpdateSlot.Hide();
        ViewState["SlotIdOld"] = "";
        ViewState["SlotDescOld"] = "";
        ViewState["UpdateUniqueId"] = "";
        ViewState["ChngdSlotId"] = "";
        ViewState["ChngdSlotDesc"] = "";
        ViewState["CartRow1"] = null;
        ViewState["Row1"] = null;
        ViewState["LnBlockId"] = null;
        GrvEditSlot.DataSource = null;
    }

    public class BoatBookNew
    {
        //Header Details
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public string BookingDate { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string[] BoatPremiumStatus { get; set; }
        public string SingleMultiStatus { get; set; }
        public string NoOfPass { get; set; }
        public string NoOfChild { get; set; }
        public string NoOfInfant { get; set; }
        public string OfferId { get; set; }
        public string InitBillAmount { get; set; }
        public string PaymentType { get; set; }
        public string ActualBillAmount { get; set; }
        public string Status { get; set; }
        public string Bookingpin { get; set; }
        public string CollectedAmount { get; set; }
        public string BalanceAmount { get; set; }

        //Book Detail Details
        public string[] BoatTypeId { get; set; }
        public string[] BoatSeaterId { get; set; }
        public string[] BookingDuration { get; set; }
        public string[] InitBoatCharge { get; set; }
        public string[] InitRowerCharge { get; set; }
        public string[] BoatDeposit { get; set; }
        public string[] TaxDetails { get; set; }
        public string[] InitOfferAmount { get; set; }
        public string[] InitNetAmount { get; set; }
        public string[] CreatedBy { get; set; }
        public string[] TaxAmount { get; set; }

        public string CreatedByUpdate { get; set; }


        public string CustomerGSTNo { get; set; }
        public string CustomerEmailId { get; set; }


        //Other Service
        public string[] OthServiceStatus { get; set; }
        public string[] OthServiceId { get; set; }
        public string[] OthChargePerItem { get; set; }
        public string[] OthNoOfItems { get; set; }
        public string[] OthTaxDetails { get; set; }
        public string[] OthNetAmount { get; set; }
        public string[] OthTaxAmount { get; set; }


        public string BookingMedia { get; set; }

        public string BFDInitBoatCharge { get; set; }
        public string BFDInitNetAmount { get; set; }
        public string BFDGST { get; set; }
        public string[] BookingTimeSlotId { get; set; }
        public string[] Countslotids { get; set; }
        public string[] BookingBlockId { get; set; }
        public string PremiumStatus { get; set; }
        public string[] CGSTTaxAmount { get; set; }
        public string[] SGSTTaxAmount { get; set; }

        public string[] CGSTOthTaxAmount { get; set; }

        public string[] SGSTOthTaxAmount { get; set; }

    }
    public class Update
    {
        public string[] BookingBlockId { get; set; }
        public string[] SlotIds { get; set; }
        public string BoatHouseId { get; set; }
        public string BookingUser { get; set; }
    }


    protected void txtMobileNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtMobileNo.Text.Length == txtMobileNo.MaxLength)
            {
                divBooking.Visible = true;
                ViewState["PINType"] = "U";
                ViewState["UserId"] = "N";

                if (txtMobileNo.Text != "")
                {
                    CheckMobileonBookingTime();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Enter Public Credentials !');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnOTP_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtMobileNo.Text != "" && txtMobileNo.Text.Length == 10)
            {
                divDetails.Visible = false;
                divOtpbtn.Visible = false;
                divOtptxt.Visible = true;
                divBooking.Visible = false;
                txtOTP.Text = string.Empty;
                txtMobileNo.Enabled = false;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var otpsendsms = new otpsendsms()
                    {
                        ServiceType = "SignUp",
                        MobileNo = txtMobileNo.Text.Trim(),
                        MediaType = "PW",
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("SendSMSMsg", otpsendsms).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            hfOTPMsg.Value = ResponseMsg;
                            ViewState["OTPMsg"] = ResponseMsg;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase.ToString() + "');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Check Mobile Number !');", true);
                txtMobileNo.Focus();
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void txtOTP_TextChanged(object sender, EventArgs e)
    {

        if (txtOTP.Text.Length == 4)
        {
            if (txtOTP.Text == ViewState["OTPMsg"].ToString().Trim())
            {
                divDetails.Visible = false;
                divOtpbtn.Visible = false;
                divOtptxt.Visible = false;
                txtOTP.Text = string.Empty;
                txtMobileNo.Enabled = false;
                divBooking.Visible = true;
                divImgStatus.Visible = true;
                imgPinStatus.ImageUrl = "~/images/tick.png";
            }
            else
            {
                divDetails.Visible = false;
                divOtpbtn.Visible = false;
                divOtptxt.Visible = true;
                txtOTP.Text = string.Empty;
                txtMobileNo.Enabled = false;
                divBooking.Visible = false;
                divImgStatus.Visible = true;
                imgPinStatus.ImageUrl = "~/images/Remove.png";
            }
        }
        else
        {
            divDetails.Visible = false;
            divOtpbtn.Visible = false;
            divOtptxt.Visible = true;
            txtOTP.Text = string.Empty;
            txtMobileNo.Enabled = false;
            divBooking.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Invalid OTP !');", true);
        }
    }

    public class otpsendsms
    {
        public string ServiceType { get; set; }
        public string MobileNo { get; set; }
        public string MediaType { get; set; }
    }

    public void CheckMobileonBookingTime()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var UserData = new CommonClass()
                {
                    QueryType = "KioskBooking",
                    ServiceType = "GetTranDetails",
                    Input1 = txtMobileNo.Text.Trim(),
                    Input2 = "B",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", UserData).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        string Min = dtExists.Rows[0]["ExpireMinute"].ToString();

                        if (Convert.ToInt32(Min) > 10)
                        {
                            GetMobileUserDetails();
                        }
                        else
                        {
                            txtMobileNo.Text = "";
                            divBooking.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Payment SMS is Already Sent To Your Mobile, You Can Try After 10 Mins !');", true);
                        }
                    }
                    else
                    {
                        GetMobileUserDetails();
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

    public void GetMobileUserDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var UserData = new CommonClass()
                {
                    QueryType = "KioskBooking",
                    ServiceType = "GetUserId",
                    Input1 = txtMobileNo.Text.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", UserData).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                    if (dtExists.Rows.Count > 0)
                    {
                        divDetails.Visible = true;
                        divOtpbtn.Visible = false;
                        divOtptxt.Visible = false;
                        txtOTP.Text = string.Empty;
                        txtMobileNo.Enabled = false;
                        divBooking.Visible = true;

                        ViewState["UserId"] = dtExists.Rows[0]["UserId"].ToString();
                        ViewState["EmailId"] = dtExists.Rows[0]["MailId"].ToString();
                        ViewState["Name"] = dtExists.Rows[0]["FirstName"].ToString() + ' ' + dtExists.Rows[0]["LastName"].ToString();

                        lblName.Text = ViewState["Name"].ToString().Trim();
                        lblEmailId.Text = ViewState["EmailId"].ToString().Trim();
                    }
                    else
                    {
                        divDetails.Visible = false;
                        divOtpbtn.Visible = true;
                        divOtptxt.Visible = false;
                        txtOTP.Text = string.Empty;
                        divBooking.Visible = false;

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

    public void SignIn(string MobileNo, string Pwd)
    {
        try
        {
            if (txtMobileNo.Text != "")
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var UserRegistration = new UserRegistration()
                    {
                        QueryType = "Login",
                        UserName = MobileNo.ToString().Trim(),
                        Password = Pwd.ToString().Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("CM_UserLogin", UserRegistration).Result;

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
                                ViewState["UserId"] = dt.Rows[0]["UserId"].ToString();

                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Enter Username & Password !');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void SignUp()
    {
        try
        {
            if (txtMobileNo.Text != "")
            {
                ViewState["UserId"] = "N";

                int numberOfDigits = (int)Math.Floor(Math.Log10(Convert.ToDouble(txtMobileNo.Text.Trim())) + 1);
                int first2digit = 0;
                if (numberOfDigits >= 4)
                {
                    first2digit = (int)Math.Truncate(((Convert.ToDouble(txtMobileNo.Text.Trim())) / Math.Pow(10, numberOfDigits - 4)));
                }

                //int last2Digits = (Convert.ToInt32(txtMobileNo.Text.Trim()) % 100);

                string Pwd = first2digit.ToString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var PublicUserReg = new PublicUserRegistration()
                    {
                        QueryType = "New",
                        MobileNo = txtMobileNo.Text.Trim(),
                        Password = Pwd.ToString().Trim(),
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("CM_UserReg", PublicUserReg).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            SignIn(txtMobileNo.Text.Trim(), Pwd.ToString().Trim());
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

    public void BindDefaultPin()
    {
        try
        {
            ViewState["DefaultPin"] = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    UserId = ViewState["UserId"].ToString().Trim(),
                    MobileNo = txtMobileNo.Text.Trim(),
                    BookingType = "U",
                    BookingMedia = "DW"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("SavePinTransaction", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ViewState["DefaultPin"] = ResponseMsg;
                        divBooking.Visible = true;

                        ViewState["CUserId"] = "";
                        ViewState["CName"] = "";
                        ViewState["PINType"] = "D";
                        ViewState["CMailId"] = "";

                    }
                    else
                    {
                        ExistBookingPin();

                        divBooking.Visible = false;
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

    public Boolean CheckBoatSelectionStatus(string Value)
    {
        if (bschar1.InnerText.Trim() != "")
        {
            if (Convert.ToDecimal(bschar1.InnerText) > 0)
            {
                if (Value.Trim() == "N")
                {
                    if (hfBoatNature.Value.Trim() == "N")
                    {
                        return true;
                    }
                }

                if (Value.Trim() == "P")
                {
                    if (hfBoatNature.Value.Trim() == "P")
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void ExistBookingPin()
    {
        try
        {
            ViewState["DefaultPin"] = "0";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    QueryType = "Pin",
                    ServiceType = "BookPin",
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy"),
                    BookingId = "0",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    UserId = ViewState["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonAPIMethod", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["DefaultPin"] = dtExists.Rows[0]["BookingPin"].ToString().Trim();
                    }
                }
                else
                {
                    ViewState["DefaultPin"] = "0";
                }
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    public class PublicUserRegistration
    {
        public string QueryType { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
    }

    public class UserRegistration
    {
        public string QueryType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    protected void btnBoatBooking_Click(object sender, EventArgs e)
    {
        try
        {
            string ServiceType = "";
            string UserId = "";

            if (bsTotal.InnerText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
                return;
            }

            if (Convert.ToDecimal(btnBoatBooking.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
                return;
            }

            BoatBookingFinal();

            if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE" || ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "UPI")
            {
                if (txtMobileNo.Text.Trim() != null)
                {
                    if (ViewState["UserId"].ToString() == "N" || ViewState["UserId"].ToString() == "" || ViewState["UserId"].ToString().Trim() == null)
                    {
                        SignUp();
                    }

                    BindDefaultPin();

                    if (ViewState["DefaultPin"].ToString().Trim() != "0" || ViewState["DefaultPin"].ToString().Trim() != ""
                        || ViewState["DefaultPin"].ToString().Trim() != null
                        || ViewState["UserId"].ToString().Trim() != "" || ViewState["UserId"].ToString() != "N"
                        || ViewState["UserId"].ToString().Trim() != null)
                    {
                        if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE")
                        {
                            ServiceType = "DOnlineBooking";
                            UserId = ViewState["UserId"].ToString().Trim();
                        }
                        else
                        {
                            ServiceType = "DUPI";
                            UserId = ViewState["UserId"].ToString().Trim();
                        }

                        OnlineBeforeTransactionDetails(ServiceType, UserId, ViewState["DefaultPin"].ToString().Trim(), txtMobileNo.Text.Trim());

                        if (ViewState["TranStatus"].ToString().Trim() == "Y")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Payment SMS Link Send to Customer Mobile !');", true);
                            divBack.Style.Add("background-color", "white");
                            ClearBooking();
                            hfNatureVisible.Value = "0";
                            premimumMsg.Visible = false;
                            premimumMsg.InnerText = "";
                            hfBoatNature.Value = "N";
                            BoatBookedSummaryList();
                            divBack.Style.Add("background-color", "white");
                            ScriptManager.RegisterStartupScript(this, GetType(), "SendOtp", "SendOtp();", true);
                            btnResend.Visible = true;

                        }
                    }

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

    private void BoatBookingFinal()
    {
        try
        {
            //--Incase Other service not booking assign default value N//

            //  ViewState["OthServiceStatus"] = "N";

            if (Convert.ToDecimal(btnBoatBooking.Text) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Select Any One Boat !');", true);
                return;
            }
            else
            {
                ViewState["BoatChargeTotal"] = "0";
                ViewState["RowerChargeTotal"] = "0";
                ViewState["BoatDepositTotal"] = "0";
                ViewState["BoatTaxTotal"] = "0";
                ViewState["boatOfferAmount"] = "";
                ViewState["SlotIds"] = "";
                ViewState["Status"] = "";
                ViewState["CGSTTaxAmount"] = "";
                ViewState["SGSTTaxAmount"] = "";
                ViewState["sGSTAmount"] = "0";
                ViewState["OthServiceStatus"] = "";
                ViewState["OthServiceId"] = "";
                ViewState["OthChargePerItem"] = "";
                ViewState["OthNoOfItems"] = "";
                ViewState["OthTaxDetails"] = "";
                ViewState["CGSTOthTaxAmount"] = "";
                ViewState["SGSTOthTaxAmount"] = "";

                foreach (GridViewRow item in gvBoatdtl.Rows)
                {
                    string cBoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
                    string cSeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();

                    Label NumberOfBoat = (Label)item.FindControl("lblBoatCount");
                    int iNumofBoat = Convert.ToInt32(NumberOfBoat.Text.Trim());

                    CheckBoatAvailableDetails(Session["BoatHouseId"].ToString().Trim(), cBoatTypeId.Trim(), cSeaterTypeId.Trim(), DateTime.Now.ToString("dd/MM/yyyy"));

                    if (hfBoatNature.Value.Trim() == "P")
                    {
                        if (iNumofBoat > Convert.ToDecimal(ViewState["cPremiumAvailable"].ToString()))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Selected boat trips exceeding the maximum trip limit !!!');", true);
                            return;
                        }
                    }

                    if (hfBoatNature.Value.Trim() == "N")
                    {
                        if (iNumofBoat > Convert.ToDecimal(ViewState["cNormalAvailable"].ToString()))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Selected boat trips exceeding the maximum trip limit !!!');", true);
                            return;
                        }
                    }

                    for (int i = 1; i <= iNumofBoat; i++)
                    {
                        string BoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
                        ViewState["BoatTypeIds"] += BoatTypeId.Trim() + '~';

                        string SeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();
                        ViewState["BoatSeaterIds"] += SeaterTypeId.Trim() + '~';

                        string BoatMinTime = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinTime"].ToString().Trim();
                        ViewState["BoatMinTimes"] += BoatMinTime.Trim() + '~';

                        string BoatMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinCharge"].ToString().Trim();
                        decimal iBoatMinCharge = Convert.ToDecimal(BoatMinCharge.Trim());
                        ViewState["InitBoatCharges"] += BoatMinCharge.Trim() + '~';

                        string RowerMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["RowerMinCharge"].ToString().Trim();
                        decimal iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
                        ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';

                        string BoatTaxCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatTaxCharge"].ToString().Trim();
                        decimal dBoatTaxCharge = Convert.ToDecimal(BoatTaxCharge.Trim());

                        string SlotId = gvBoatdtl.DataKeys[item.RowIndex]["SlotId"].ToString().Trim();
                        ViewState["SlotIds"] += SlotId.Trim() + '~';

                        string Status = gvBoatdtl.DataKeys[item.RowIndex]["Status"].ToString().Trim();
                        if (Status == "Normal")
                        {
                            Status = "N";
                        }
                        else if (Status == "Express")
                        {
                            Status = "P";
                        }
                        ViewState["Status"] += Status.Trim() + '~';
                        var lblTax = ViewState["TaxPercentBoat"].ToString();

                        decimal BoatTotalTaxAmt = 0;
                        string TaxDtl = string.Empty;
                        string TaxAmount = string.Empty;
                        if (lblTax != "")
                        {
                            string[] taxlist = lblTax.Split(',');

                            decimal TaxAmt = dBoatTaxCharge / 2;

                            foreach (var list in taxlist)
                            {
                                var TaxName = list;
                                var tx = list.Split('-');

                                TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                                BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
                                TaxAmount += (TaxAmt + "#").Trim();
                            }
                        }

                        string SAmountArray;
                        string[] AmountArray;

                        ViewState["sGST"] += TaxDtl.ToString() + '~';
                        ViewState["sGSTAmount"] = TaxAmount.ToString();

                        SAmountArray = ViewState["sGSTAmount"].ToString();
                        AmountArray = SAmountArray.Split('#');

                        ViewState["CGSTTaxAmount"] += AmountArray[0].ToString() + '~';
                        ViewState["SGSTTaxAmount"] += AmountArray[1].ToString() + '~';

                        ViewState["TaxAmountDetlBoat"] += Convert.ToString(TaxDtl.ToString() + '~');

                        decimal iBoatDeposit = Convert.ToDecimal(gvBoatdtl.DataKeys[item.RowIndex]["Deposit"].ToString().Trim());

                        decimal BoatDepositAmount = 0;
                        BoatDepositAmount = iBoatDeposit;

                        ViewState["BoatDeposits"] += BoatDepositAmount.ToString() + '~';

                        ViewState["InitNetAmount"] = (iBoatMinCharge + iRowerMinCharge + BoatDepositAmount + BoatTotalTaxAmt).ToString();
                        ViewState["InitNetAmounts"] += ViewState["InitNetAmount"].ToString() + '~';

                        // Do whatever you need with that string value here

                        ViewState["BoatChargeTotal"] = (Convert.ToDecimal(ViewState["BoatChargeTotal"]) + iBoatMinCharge).ToString();

                        ViewState["RowerChargeTotal"] = (Convert.ToDecimal(ViewState["RowerChargeTotal"]) + iRowerMinCharge).ToString();

                        ViewState["BoatDepositTotal"] = (Convert.ToDecimal(ViewState["BoatDepositTotal"]) + BoatDepositAmount).ToString();

                        ViewState["BoatTaxTotal"] = (Convert.ToDecimal(ViewState["BoatTaxTotal"]) + BoatTotalTaxAmt).ToString();
                        string boatOfferAmount = "0";
                        ViewState["boatOfferAmount"] += boatOfferAmount.ToString().Trim() + '~';
                        string OthServiceStatus = "0";
                        ViewState["OthServiceStatus"] += OthServiceStatus.ToString().Trim() + '~';
                        string OthServiceId = "0";
                        ViewState["OthServiceId"] += OthServiceId.ToString().Trim() + '~';
                        string OthChargePerItem = "0";
                        ViewState["OthChargePerItem"] += OthChargePerItem.ToString().Trim() + '~';
                        string OthNoOfItems = "0";
                        ViewState["OthNoOfItems"] += OthNoOfItems.ToString().Trim() + '~';
                        string OthNetAmount = "0";
                        ViewState["OthNetAmount"] += OthNetAmount.ToString().Trim() + '~';
                        string CGSTOthTaxAmount = "0";
                        ViewState["CGSTOthTaxAmount"] += CGSTOthTaxAmount.ToString().Trim() + '~';
                        string SGSTOthTaxAmount = "0";
                        ViewState["SGSTOthTaxAmount"] += SGSTOthTaxAmount.ToString().Trim() + '~';


                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }


    public void OnlineBeforeTransactionDetails(string ServiceType, string CustUserId, string BookedPin, string MobileNo)
    {
        try
        {
            ViewState["TranStatus"] = "N";


            string BoatTypeIds = string.Empty;
            string SeaterTypeIds = string.Empty;
            string BoatMinTime = string.Empty;
            string InitBoatCharges = string.Empty;
            string RowerMinCharge = string.Empty;
            string BoatDeposits = string.Empty;
            string boatOfferAmount = string.Empty;

            string InitNetAmounts = string.Empty;
            string UserIds = string.Empty;
            string BlockId = string.Empty;
            string SlotId = string.Empty;
            string TaxDetails = string.Empty;
            string TaxAmount = string.Empty;
            string CGSTTaxAmount = string.Empty;
            string SGSTTaxAmount = string.Empty;
            string Status = string.Empty;
            string OthServiceStatus = string.Empty;
            string OthServiceId = string.Empty;
            string OthChargePerItem = string.Empty;
            string OthNoOfItems = string.Empty;
            string OthNetAmount = string.Empty;
            string CGSTOthTaxAmount = string.Empty;
            string SGSTOthTaxAmount = string.Empty;



            string[] sBoatTypeIds;
            string[] sSeaterTypeIds;
            string[] sBoatMinTime;
            string[] sInitBoatCharges;
            string[] sRowerMinCharge;
            string[] sBoatDeposits;
            string[] sboatOfferAmount;
            string[] sInitNetAmounts;
            string[] sSlotIds;
            string[] sBlockId;
            string[] sStatus;
            string[] sCGSTTaxAmount;
            string[] sSGSTTaxAmount;

            CGSTTaxAmount = ViewState["CGSTTaxAmount"].ToString();
            sCGSTTaxAmount = CGSTTaxAmount.Split('~');

            SGSTTaxAmount = ViewState["SGSTTaxAmount"].ToString();
            sSGSTTaxAmount = SGSTTaxAmount.Split('~');

            BoatDeposits = ViewState["BoatDeposits"].ToString().TrimEnd(' ');
            sBoatDeposits = BoatDeposits.Split('~');

            BoatTypeIds = ViewState["BoatTypeIds"].ToString().TrimEnd(' ');
            sBoatTypeIds = BoatTypeIds.Split('~');

            BoatMinTime = ViewState["BoatMinTimes"].ToString();
            sBoatMinTime = BoatMinTime.Split('~');

            SeaterTypeIds = ViewState["BoatSeaterIds"].ToString();
            sSeaterTypeIds = SeaterTypeIds.Split('~');

            InitBoatCharges = ViewState["InitBoatCharges"].ToString();
            sInitBoatCharges = InitBoatCharges.Split('~');

            RowerMinCharge = ViewState["RowerMinCharge"].ToString();
            sRowerMinCharge = RowerMinCharge.Split('~');

            boatOfferAmount = ViewState["boatOfferAmount"].ToString();
            sboatOfferAmount = boatOfferAmount.Split('~');

            InitNetAmounts = ViewState["InitNetAmounts"].ToString();
            sInitNetAmounts = InitNetAmounts.Split('~');

            BlockId = ViewState["sBlockId"].ToString();
            sBlockId = BlockId.Split('~');

            SlotId = ViewState["SlotIds"].ToString().TrimEnd(' ');
            sSlotIds = SlotId.Split('~');
            Status = ViewState["Status"].ToString();
            sStatus = Status.Split('~');
            string HeaderStatus = string.Empty;
            int Ncount = sStatus.Count(s => s == "N");

            int Pcount = sStatus.Count(s => s == "P");
            int Icount = sStatus.Count(s => s == "I");

            if (Ncount > 0 && Pcount > 0 && Icount > 0)
            {
                HeaderStatus = "M";
            }
            if (Ncount > 0 && Pcount > 0)
            {
                HeaderStatus = "M";
            }
            if (Ncount > 0 && Icount > 0)
            {
                HeaderStatus = "M";
            }
            if (Pcount > 0 && Icount > 0)
            {
                HeaderStatus = "M";
            }
            if (Ncount > 0 && Pcount == 0 && Icount == 0)
            {
                HeaderStatus = "N";
            }
            if (Ncount == 0 && Pcount > 0 && Icount == 0)
            {
                HeaderStatus = "P";
            }
            if (Ncount == 0 && Pcount == 0 && Icount > 0)
            {
                HeaderStatus = "I";
            }


            string[] sOthServiceStatus;
            string[] sOthServiceId;
            string[] sOthChargePerItem;
            string[] sOthNoOfItems;
            string[] sOthNetAmount;
            string[] sCGSTOthTaxAmount;
            string[] sSGSTOthTaxAmount;

            OthServiceStatus = ViewState["OthServiceStatus"].ToString();
            sOthServiceStatus = OthServiceStatus.Split('~');

            OthServiceId = ViewState["OthServiceId"].ToString();
            sOthServiceId = OthServiceId.Split('~');

            OthChargePerItem = ViewState["OthChargePerItem"].ToString();
            sOthChargePerItem = OthChargePerItem.Split('~');

            OthNoOfItems = ViewState["OthNoOfItems"].ToString();
            sOthNoOfItems = OthNoOfItems.Split('~');

            OthNetAmount = ViewState["OthNetAmount"].ToString();
            sOthNetAmount = OthNetAmount.Split('~');

            CGSTOthTaxAmount = ViewState["CGSTOthTaxAmount"].ToString();
            sCGSTOthTaxAmount = CGSTOthTaxAmount.Split('~');

            SGSTOthTaxAmount = ViewState["SGSTOthTaxAmount"].ToString().TrimEnd(' ');
            sSGSTOthTaxAmount = SGSTOthTaxAmount.Split('~');


            if (bsTotal.InnerText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
                return;
            }

            if (Convert.ToDecimal(btnBoatBooking.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
                return;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                var BoatSearch = new OnlineBoatSearch()
                {
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BookingPin = BookedPin.Trim(),

                    UserId = CustUserId.Trim(),
                    MobileNo = MobileNo.Trim(),
                    EmailId = lblEmailId.Text.Trim(),

                    PaymentMode = "Online",
                    Amount = bsTotal.InnerText.Trim(),
                    BookingType = "B",

                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    PremiumStatus = HeaderStatus,
                    BoatPremiumStatus = sStatus,

                    NoOfPass = "0",
                    NoOfChild = "0",
                    NoOfInfant = "0",

                    BoatTypeId = sBoatTypeIds,
                    BoatSeaterId = sSeaterTypeIds,
                    BookingDuration = sBoatMinTime,

                    InitBoatCharge = sInitBoatCharges,
                    InitRowerCharge = sRowerMinCharge,
                    BoatDeposit = sBoatDeposits,

                    InitOfferAmount = sboatOfferAmount,
                    InitNetAmount = sInitNetAmounts,
                    CGSTTaxAmount = sCGSTTaxAmount,
                    SGSTTaxAmount = sSGSTTaxAmount,


                    // Other Service Booking

                    OthServiceStatus = sOthServiceStatus,
                    OthServiceId = sOthServiceId,
                    OthChargePerItem = sOthChargePerItem,
                    OthNoOfItems = sOthNoOfItems,
                    CGSTOthTaxAmount = sCGSTOthTaxAmount,
                    SGSTOthTaxAmount = sSGSTOthTaxAmount,
                    OthNetAmount = sOthNetAmount,

                    BookingMedia = "PW",

                    BFDInitBoatCharge = "0",
                    BFDInitNetAmount = "0",
                    BFDGST = "0",
                    EntryType = "KB",
                    ModuleType = "Boating",
                    BookingTimeSlotId = sSlotIds,
                    BookingBlockId = sBlockId

                };

                HttpResponseMessage response = client.PostAsJsonAsync("PublicOnlineBoatBookingBfrTran", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        string[] sResult = ResponseMsg.Split('~');
                        //ViewState["TranStatus"] = "Y";
                        //ViewState["sBlockId"] = "";
                        //SendSMS(ServiceType, MobileNo.Trim(), sResult[1].ToString().Trim(), bsTotal.InnerText.Trim());
                        ViewState["TranStatus"] = "Y";
                        ViewState["sBlockId"] = "";
                        ViewState["ResendServiceType"] = ServiceType;
                        ViewState["ResendMobileNo"] = MobileNo.Trim();
                        ViewState["ResendResult"] = sResult[1].ToString().Trim();
                        ViewState["ResendTotal"] = bsTotal.InnerText.Trim();
                        SendSMS(ServiceType, MobileNo.Trim(), sResult[1].ToString().Trim(), bsTotal.InnerText.Trim());
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                        return;
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

    public class OtpResendSMS
    {

        public string BookingId { get; set; }
        public string MobileNo { get; set; }
        public string ReferenceNo { get; set; }

    }

    //Resending SMS

    protected void BtnResendSMS_Click(object sender, EventArgs e)
    {
        string MobileNo = string.Empty;
        string BookingId = string.Empty;
        string ReferenceNo = string.Empty;

        BookingId = ViewState["ResendBookingId"].ToString();
        MobileNo = ViewState["ResendMobNo"].ToString();
        ReferenceNo = ViewState["ReferenceNo"].ToString();

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var OtpReSendSms = new OtpResendSMS()
                {
                    BookingId = BookingId.Trim(),
                    MobileNo = MobileNo.Trim(),
                    ReferenceNo = ReferenceNo.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("ReSendSMSMsg", OtpReSendSms).Result;

                if (response.IsSuccessStatusCode)
                {

                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Payment SMS Link ReSended to Customer Mobile !');", true);
                        btnResend.Visible = false;
                        ClearBooking();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Payment SMS Link Not ReSended to Customer Mobile !');", true);
                    }
                }

            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

    }


}