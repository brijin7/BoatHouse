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
using System.Globalization;
using System.Web.Helpers;

public partial class Boating_BulkBoatBooking : System.Web.UI.Page
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
                if (Session["BBMBulkBooking"].ToString().Trim() == "Y")
                {
                    btnNor.Visible = true;
                    btnPre.Visible = true;
                    GetPaymentType();
                    GetTaxDetail();
                    hfBoatNature.Value = "N";
                    BoatBookedSummaryList();
                    ClearTempValues();
                    GetOfferDiscountlist();
                    txtNoPersons.Focus();

                    if (Session["BMBookingOthers"].ToString().Trim() == "Y")
                    {
                        btnOther.Visible = true;
                    }

                    BindBookingCountAmount();
                    // ViewState["OkFlag"] = "P";

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
                        string Message = "No Premium Boat Available";

                        if (hfBoatNature.Value.Trim() == "N")
                        {
                            Message = "No Normal Boat Available";
                        }

                        premimumMsg.InnerText = Message;
                        premimumMsg.Visible = true;
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

    protected void dtlBoatchild_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {


            if (txtNoPersons.Text == "")
            {
                txtNoPersons.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Enter No of Person !');", true);
                dvContent.Attributes.Add("style", "display:none;");
                return;
            }

            ClearTempValues();

            string BoatTypeId = string.Empty;
            string BoatTypeName = string.Empty;

            string BoatSeaterId = string.Empty;
            string BoatSeaterType = string.Empty;

            //string BoatCount = string.Empty;
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

            Label lblSeaterCount = (Label)e.Item.FindControl("lblNoOfSeats");
            string BoatSeaterCount = lblSeaterCount.Text;

            //DropDownList NoCount = (DropDownList)e.Item.FindControl("dlstCount");
            //BoatCount = NoCount.SelectedValue;

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
                BoatDepositAmount = Convert.ToDecimal(Deposit);
            }
            else
            {
                //decimal bAmount = (Convert.ToDecimal(BoatCount) * Convert.ToDecimal(BoatMinCharge)) + (Convert.ToDecimal(BoatCount) * Convert.ToDecimal(RowerMinCharge));

                BoatDepositAmount = ((Convert.ToDecimal(BoatTotalCharge) * Convert.ToDecimal(Deposit)) / 100);
            }

            //BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatCount, BoatTotalCharge, BoatMinCharge,
            //    RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount.ToString("0.00"), BoatMinTime);

            ViewState["TBoatTypeName"] = BoatTypeName;
            ViewState["TBoatTypeId"] = BoatTypeId;
            ViewState["TBoatSeaterType"] = BoatSeaterType;
            ViewState["TBoatSeaterId"] = BoatSeaterId;
            ViewState["TBoatSeaterCount"] = BoatSeaterCount;
            ViewState["TBoatTotalCharge"] = BoatTotalCharge;
            ViewState["TBoatMinCharge"] = BoatMinCharge;
            ViewState["TRowerMinCharge"] = RowerMinCharge;
            ViewState["TBoatTaxCharge"] = BoatTaxCharge;
            ViewState["TDepositType"] = DepositType;
            ViewState["TBoatDepositAmount"] = BoatDepositAmount.ToString("0.00");
            ViewState["TBoatMinTime"] = BoatMinTime;

            //Button btnSeaterType = (Button)e.Item.FindControl("btnSeaterType");
            //btnSeaterType.Attributes.Add("style", "background-color:#5cbf2a");

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "addItem();", true);

            Panel pnlBoatCount = (Panel)((DataList)dtlBoat).Controls[0].Controls[0].FindControl("pnlBoatCount");
            pnlBoatCount.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindDataDynamicValue(string BoatType, string BoatTypeId, string SeaterType, string SeaterTypeId, string BoatCount, string BoatTotalCharge,
        string BoatMinCharge, string RowerMinCharge, string BoatTaxCharge, string DepositType, string Deposit, string BoatMinTime, string PersonCount,
     string Status, string SlotId, string SlotType, string BlockId)
    {
        try
        {
            // Check Boat Seater Availability Conditions

            //decimal BookedboatCount = 0;
            string SlotIds;
            string[] sSlotId;
            SlotIds = SlotId;
            sSlotId = SlotIds.Split('~');

            string SlotTypes;
            string[] sSlotType;
            SlotTypes = SlotType;
            sSlotType = SlotTypes.Split('~');

            string BlockIds;
            string[] sBlockId;

            BlockIds = BlockId;
            sBlockId = BlockIds.Split('~');

            if (ViewState["CartRow"] != null)
            {
                DataTable mt = (DataTable)ViewState["CartRow"];

                if (mt.Rows.Count > 0)
                {
                    DataRow[] result = mt.Select("BoatTypeId = '" + BoatTypeId.Trim() + "' AND SeaterTypeId ='" + SeaterTypeId.Trim() + "'");

                }
            }




            // Bind an Value in Temp DataTable

            DataTable mytable = new DataTable();

            if (ViewState["Row"] != null)
            {
                mytable = (DataTable)ViewState["Row"];
                DataRow dr = null;

                string Value = BoatTypeId.Trim() + "~" + SeaterTypeId.Trim();

                DataRow[] fndUniqueId = mytable.Select("UniqueId = '" + Value.Trim() + "'");

                if (mytable.Rows.Count > 0)
                {
                    int NoPerson = Convert.ToInt32(txtNoPersons.Text);
                    int CheckCount = (Convert.ToInt32(BoatCount) * Convert.ToInt32(PersonCount))
                        + Convert.ToInt32(mytable.Compute("SUM(PersonCount)", "").ToString());

                    //if (CheckCount <= NoPerson)
                    //{
                    for (int i = 0; i < Convert.ToInt32(BoatCount); i++)
                    {
                        dr = mytable.NewRow();

                        dr["UniqueId"] = BoatTypeId.Trim() + "~" + SeaterTypeId.Trim() + "~" + sSlotId[i];
                        dr["BoatType"] = BoatType.Trim(); ;
                        dr["BoatTypeId"] = BoatTypeId.Trim();

                        dr["SeaterType"] = SeaterType.Trim();
                        dr["SeaterTypeId"] = SeaterTypeId.Trim();
                        dr["PersonCount"] = PersonCount.Trim();

                        dr["BoatCount"] = 1;
                        dr["BoatTotalCharge"] = BoatTotalCharge;

                        dr["BoatMinCharge"] = BoatMinCharge.Trim();
                        dr["RowerMinCharge"] = RowerMinCharge.Trim();
                        dr["BoatTaxCharge"] = BoatTaxCharge.Trim();
                        dr["DepositType"] = DepositType.Trim();
                        dr["Deposit"] = Deposit.Trim();
                        dr["BoatMinTime"] = BoatMinTime.Trim();

                        dr["Status"] = Status.Trim();
                        dr["SlotId"] = sSlotId[i].Trim();
                        dr["SlotType"] = sSlotType[i].Trim();
                        dr["BlockId"] = sBlockId[i].Trim();


                        mytable.Rows.Add(dr);
                    }

                    ViewState["Row"] = mytable;
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Sorry, You Selected Excess Boat Seater !');", true);
                    //    return;
                    //}
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
                mytable.Columns.Add(new DataColumn("PersonCount", typeof(Int32)));
                mytable.Columns.Add(new DataColumn("BoatTotalCharge", typeof(decimal)));

                mytable.Columns.Add(new DataColumn("BoatMinCharge", typeof(string)));
                mytable.Columns.Add(new DataColumn("RowerMinCharge", typeof(string)));
                mytable.Columns.Add(new DataColumn("BoatTaxCharge", typeof(string)));
                mytable.Columns.Add(new DataColumn("DepositType", typeof(string)));
                mytable.Columns.Add(new DataColumn("Deposit", typeof(decimal)));
                mytable.Columns.Add(new DataColumn("BoatMinTime", typeof(string)));

                mytable.Columns.Add(new DataColumn("Status", typeof(string)));
                mytable.Columns.Add(new DataColumn("SlotId", typeof(string)));
                mytable.Columns.Add(new DataColumn("SlotType", typeof(string)));
                mytable.Columns.Add(new DataColumn("BlockId", typeof(string)));

                DataRow dr1 = mytable.NewRow();

                for (int i = 0; i < Convert.ToInt32(BoatCount); i++)
                {
                    dr1 = mytable.NewRow();

                    dr1["UniqueId"] = BoatTypeId.Trim() + "~" + SeaterTypeId.Trim() + "~" + sSlotId[i];
                    dr1["BoatType"] = BoatType.Trim();
                    dr1["BoatTypeId"] = BoatTypeId.Trim();

                    dr1["SeaterType"] = SeaterType.Trim();
                    dr1["SeaterTypeId"] = SeaterTypeId.Trim();
                    dr1["PersonCount"] = PersonCount.Trim();

                    dr1["BoatCount"] = 1;
                    dr1["BoatTotalCharge"] = BoatTotalCharge;

                    dr1["BoatMinCharge"] = BoatMinCharge.Trim();
                    dr1["RowerMinCharge"] = RowerMinCharge.Trim();
                    dr1["BoatTaxCharge"] = BoatTaxCharge.Trim();
                    dr1["DepositType"] = DepositType.Trim();
                    dr1["Deposit"] = Deposit.Trim();
                    dr1["BoatMinTime"] = BoatMinTime.Trim();

                    dr1["Status"] = Status.Trim();
                    dr1["SlotId"] = sSlotId[i].Trim();
                    dr1["SlotType"] = sSlotType[i].Trim();
                    dr1["BlockId"] = sBlockId[i].Trim();
                    mytable.Rows.Add(dr1);
                }

                ViewState["Row"] = mytable;
            }

            if (mytable.Rows.Count > 0)
            {

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

                //Assign this values for calculation Discoun when Fixed.
                ViewState["AllBoatChargeForDiscountCalc"] = (Convert.ToDecimal(dBoatMinCharge)) + (Convert.ToDecimal(dRowerMinCharge)) + (Convert.ToDecimal(dGSTAmount));

                //Over all selected Person count.
                ViewState["OverAllSelectedPersonCount"] = mytable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Int32>("PersonCount")));

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

                                 //BoatCount = row.Field<Int32>("BoatCount"),
                                 //BoatTotalCharge = row.Field<decimal>("BoatTotalCharge"),

                                 BoatMinCharge = row.Field<string>("BoatMinCharge"),
                                 RowerMinCharge = row.Field<string>("RowerMinCharge"),
                                 BoatTaxCharge = row.Field<string>("BoatTaxCharge"),

                                 DepositType = row.Field<string>("DepositType"),
                                 Deposit = row.Field<decimal>("Deposit"),
                                 BoatMinTime = row.Field<string>("BoatMinTime"),

                                 Status = row.Field<string>("Status"),
                                 SlotId = row.Field<string>("SlotId"),
                                 SlotType = row.Field<string>("SlotType"),

                             } into t1
                             select new
                             {
                                 UniqueID = t1.Key.UniqueId,
                                 BoatType = t1.Key.BoatType,
                                 BoatTypeId = t1.Key.BoatTypeId,

                                 SeaterType = t1.Key.SeaterType,
                                 SeaterTypeId = t1.Key.SeaterTypeId,

                                 BoatCount = t1.Sum(a => a.Field<Int32>("BoatCount")),
                                 PersonCount = t1.Sum(a => a.Field<Int32>("PersonCount")),
                                 BoatTotalCharge = t1.Sum(a => a.Field<decimal>("BoatTotalCharge")) + t1.Sum(b => b.Field<decimal>("Deposit")),

                                 BoatMinCharge = t1.Key.BoatMinCharge,
                                 RowerMinCharge = t1.Key.RowerMinCharge,
                                 BoatTaxCharge = t1.Key.BoatTaxCharge,

                                 DepositType = t1.Key.DepositType,
                                 Deposit = t1.Key.Deposit,
                                 BoatMinTime = t1.Key.BoatMinTime,

                                 Status = t1.Key.Status,
                                 SlotId = t1.Key.SlotId,
                                 SlotType = t1.Key.SlotType,
                             })
                 .Select(g =>
                 {
                     var h = dts.NewRow();
                     h["UniqueId"] = g.UniqueID;
                     h["BoatType"] = g.BoatType;
                     h["BoatTypeId"] = g.BoatTypeId;

                     h["SeaterType"] = g.SeaterType;
                     h["SeaterTypeId"] = g.SeaterTypeId;

                     h["BoatCount"] = g.BoatCount;
                     h["PersonCount"] = g.PersonCount;
                     h["BoatTotalCharge"] = g.BoatTotalCharge;

                     h["BoatMinCharge"] = g.BoatMinCharge;
                     h["RowerMinCharge"] = g.RowerMinCharge;
                     h["BoatTaxCharge"] = g.BoatTaxCharge;

                     h["DepositType"] = g.DepositType;
                     h["Deposit"] = g.Deposit;
                     h["BoatMinTime"] = g.BoatMinTime;


                     h["Status"] = g.Status;
                     h["SlotId"] = g.SlotId;
                     h["SlotType"] = g.SlotType;
                     return h;
                 }).CopyToDataTable();

            if (CartTable.Rows.Count > 0)
            {
                ViewState["CartRow"] = CartTable;

                gvBoatdtl.Visible = true;
                gvBoatdtl.DataSource = CartTable;
                gvBoatdtl.DataBind();

                txtNoPersons.Enabled = false;
            }
            else
            {
                gvBoatdtl.Visible = false;
                gvBoatdtl.DataBind();

                txtNoPersons.Enabled = true;
                txtNoPersons.Focus();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

        BookingPersonAlert(Convert.ToDecimal(txtNoPersons.Text.Trim()), Convert.ToDecimal(ViewState["OverAllSelectedPersonCount"].ToString()));
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
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
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
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
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Payment Type Details Not Found...!');", true);
                        }

                        //ddlPaymentType.Items.Insert(0, new ListItem("Select Payment Type", "0"));
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
    /// <summary>
    /// Abhinaya
    /// </summary>
    private void OtherBookingFinal()
    {
        ViewState["OthServiceStatus"] = "";
        ViewState["OthServiceId"] = "";
        ViewState["OthChargePerItem"] = "";
        ViewState["OthNoOfItems"] = "";
        ViewState["OthTaxDetails"] = "";
        ViewState["OthNetAmount"] = "";
        ViewState["CGSTOthTaxAmount"] = "";
        ViewState["SGSTOthTaxAmount"] = "";

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

                        TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                        TaxAmount += (TaxAmt + "#").Trim();
                        TotalTaxAmt = TotalTaxAmt + TaxAmt;
                    }
                }
                ///New
                ViewState["sOthGSTAmount"] = TaxAmount.ToString();
                string SOthAmountArray;
                string[] OthAmountArray;

                SOthAmountArray = ViewState["sOthGSTAmount"].ToString();
                OthAmountArray = SOthAmountArray.Split('#');
                ViewState["CGSTOthTaxAmount"] += OthAmountArray[0].ToString() + '~';
                ViewState["SGSTOthTaxAmount"] += OthAmountArray[1].ToString() + '~';
                ///New
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
                    for (int i = sCount.Count(); i < sAdult.Count(); i++)
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

                        string BFDInitBoatCharge = "0";
                        ViewState["BFDInitBoatCharge"] += BFDInitBoatCharge.ToString().Trim() + '~';

                        string BFDInitNetAmount = "0";
                        ViewState["BFDInitNetAmount"] += BFDInitNetAmount.ToString().Trim() + '~';

                        string BFDGST = "0";
                        ViewState["BFDGST"] += BFDGST.ToString().Trim() + '~';

                        string sUserId = Session["UserId"].ToString();
                        ViewState["sUserId"] += sUserId.ToString().Trim() + '~';
                        string SlotIds = "";
                        ViewState["SlotIds"] += SlotIds.ToString().Trim() + '~';
                        string sBlockId = "";
                        ViewState["BlockIdss"] += sBlockId.ToString().Trim() + '~';
                        string Status = "";
                        ViewState["Status"] += Status.ToString().Trim() + '~';



                    }
                }
                else
                {
                    //foreach (GridViewRow item in gvBoatdtl.Rows)
                    //{
                    //    Label NumberOfBoat = (Label)item.FindControl("lblBoatCount");
                    //    int iNumofBoat = Convert.ToInt32(NumberOfBoat.Text.Trim());

                    for (int i = sAdult.Count(); i < sCount.Count(); i++)
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

            else
            {
                ViewState["OthServiceStatus"] = "N";

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

                            int ServiceId = 0;
                            ViewState["OthServiceId"] += ServiceId.ToString().Trim() + '~';

                            int ChargePerItem = 0;
                            ViewState["OthChargePerItem"] += ChargePerItem.ToString().Trim() + '~';

                            int AdultCount = 0;
                            ViewState["OthNoOfItems"] += AdultCount.ToString().Trim() + '~';

                            int TaxDtl = 0;

                            ViewState["sOthGST"] += TaxDtl.ToString() + '~';



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

    protected void btnBoatBooking_Click(object sender, EventArgs e)
    {
        try
        {
            dvContent.Attributes.Add("style", "display:none;");
            btnBoatBooking.Enabled = false;

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

            if (ddlOfferDiscount.SelectedIndex == 0)
            {
                BoatBookingFinal();
            }

            OtherBookingFinal();
            ///BoatBooking
            string SlotIds = string.Empty;
            string BoatTypeIds = string.Empty;
            string SeaterTypeIds = string.Empty;
            string BoatMinTime = string.Empty;
            string InitBoatCharges = string.Empty;
            string RowerMinCharge = string.Empty;
            string BoatDeposits = string.Empty;
            string boatOfferAmount = string.Empty;
            string TaxAmountDetlBoat = string.Empty;
            string InitNetAmounts = string.Empty;
            string UserIds = string.Empty;
            string BlockId = string.Empty;
            string TaxDetails = string.Empty;
            string TaxAmount = string.Empty;
            string CGSTTaxAmount = string.Empty;
            string SGSTTaxAmount = string.Empty;
            string Status = string.Empty;
            string Count = string.Empty;

            string BFDInitBoatCharge = string.Empty;
            string BFDInitNetAmount = string.Empty;
            string BFDGST = string.Empty;


            string[] sBoatTypeIds;
            string[] sSeaterTypeIds;
            string[] sBoatMinTime;
            string[] sInitBoatCharges;
            string[] sRowerMinCharge;
            string[] sBoatDeposits;
            string[] sboatOfferAmount;
            string[] sTaxAmountDetlBoat;
            string[] sInitNetAmounts;
            string[] sUserId;
            string[] sSlotIds;
            string[] sBlockId;


            string[] sStatus;
            string[] sCount;
            string[] sCGSTTaxAmount;
            string[] sSGSTTaxAmount;

            string[] sBFDInitBoatCharge;
            string[] sBFDInitNetAmount;
            string[] sBFDGST;



            /////OTHER BOOKING
            string OthServiceId = string.Empty;
            string OthServiceStatus = string.Empty;
            string OthChargePerItem = string.Empty;
            string OthNoOfItems = string.Empty;
            string OthTaxDetails = string.Empty;
            string OthTaxAmount = string.Empty;
            string CGSTOthTaxAmount = string.Empty;
            string SGSTOthTaxAmount = string.Empty;
            string OthNetAmount = string.Empty;

            string[] sOthServiceStatus;
            string[] sOthServiceId;
            string[] sOthChargePerItem;
            string[] sOthNoOfItems;


            string[] sOthNetAmount;
            string[] sCGSTOthTaxAmount;
            string[] sSGSTOthTaxAmount;


            OthServiceStatus = ViewState["OthServiceStatus"].ToString();
            sOthServiceStatus = OthServiceStatus.Split('~');

            Count = ViewState["Count"].ToString();
            sCount = Count.Split('~');
            string LoopCount;
            string[] sLoopCount;
            for (int i = 1; i < sCount.Count(); i++)
            {
                int FirstCount = i;
                ViewState["Counts"] += FirstCount.ToString() + "~";

            }
            LoopCount = ViewState["Counts"].ToString();
            sLoopCount = LoopCount.Split('~');


            //TaxDetails = ViewState["sGST"].ToString();
            //sTaxDetails = TaxDetails.Split('~');

            //TaxAmount = ViewState["sGSTAmount"].ToString();
            //sTaxAmount = TaxAmount.Split('~');

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

            TaxAmountDetlBoat = ViewState["TaxAmountDetlBoat"].ToString();
            sTaxAmountDetlBoat = TaxAmountDetlBoat.Split('~');

            InitNetAmounts = ViewState["InitNetAmounts"].ToString();
            sInitNetAmounts = InitNetAmounts.Split('~');


            BFDInitBoatCharge = ViewState["BFDInitBoatCharge"].ToString();
            sBFDInitBoatCharge = BFDInitBoatCharge.Split('~');

            BFDInitNetAmount = ViewState["BFDInitNetAmount"].ToString();
            sBFDInitNetAmount = BFDInitNetAmount.Split('~');

            BFDGST = ViewState["BFDGST"].ToString();
            sBFDGST = BFDGST.Split('~');


            UserIds = ViewState["sUserId"].ToString();
            sUserId = UserIds.Split('~');

            BlockId = ViewState["BlockIdss"].ToString();
            sBlockId = BlockId.Split('~');

            SlotIds = ViewState["SlotIds"].ToString().TrimEnd(' ');
            sSlotIds = SlotIds.Split('~');

            Status = ViewState["Status"].ToString();
            sStatus = Status.Split('~');
            string HeaderStatus = string.Empty;
            int Ncount = sStatus.Count(s => s == "N");

            int Pcount = sStatus.Count(s => s == "P");

            if (Ncount > 0 && Pcount > 0)
            {
                HeaderStatus = "M";
            }
            if (Ncount > 0 && Pcount > 0)
            {
                HeaderStatus = "M";
            }

            if (Ncount > 0 && Pcount == 0)
            {
                HeaderStatus = "N";
            }
            if (Ncount == 0 && Pcount > 0)
            {
                HeaderStatus = "P";
            }



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

            SGSTOthTaxAmount = ViewState["SGSTOthTaxAmount"].ToString();
            sSGSTOthTaxAmount = SGSTOthTaxAmount.Split('~');

            // 2023-01-28 Abhinaya blocked for UPI payment UPI like CASH.
            //if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE" || ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "UPI")

            if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE")
            {
                if (lblUserMobileNo.Text.Trim() != null)
                {
                    if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE")
                    {
                        ServiceType = "DOnlineBooking";
                        UserId = ViewState["CUserId"].ToString().Trim();
                    }
                    // 2023-01-28 Abhinaya blocked for UPI payment UPI like CASH.
                    //else
                    //{
                    //    ServiceType = "DUPI";
                    //    UserId = Session["UserId"].ToString().Trim();
                    //}

                    OnlineBeforeTransactionDetails(ServiceType, UserId);

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
                        dtlOther.Visible = false;
                        divBack.Style.Add("background-color", "white");
                    }

                    return;
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //if (ViewState["OthServiceStatus"].ToString() == "N")
                //{
                //    ViewState["OthServiceId"] = "0";
                //    ViewState["OthChargePerItem"] = "0";
                //    ViewState["OthNoOfItems"] = "0";
                //    ViewState["OthTaxDetails"] = "0";
                //    ViewState["OthNetAmount"] = "0";
                //}

                string GSTNO = string.Empty;
                string CollectedAmount = string.Empty;
                string BalanceAmount = string.Empty;

                if (chkGSTNo.Checked == true)
                {
                    GSTNO = txtINSGSTNO.Text.Trim();
                }
                else
                {
                    GSTNO = "";
                }

                if (txtAmountPaid.Text.Trim() != "")
                {
                    CollectedAmount = txtAmountPaid.Text.Trim();
                }
                else
                {
                    CollectedAmount = "0";
                }

                if (hfBalanceAmt.Value.Trim() != "")
                {
                    BalanceAmount = hfBalanceAmt.Value.Trim();
                }
                else
                {
                    BalanceAmount = "0";
                }

                var BoatBook = new BoatBookNew()
                {
                    QueryType = "Insert",
                    BookingId = "0",
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),

                    Bookingpin = txtPIN.Text.Trim(),
                    CustomerId = ViewState["CUserId"].ToString().Trim(),
                    CustomerMobileNo = lblUserMobileNo.Text.Trim(),
                    CustomerName = ViewState["CName"].ToString().Trim(),
                    CustomerAddress = "",
                    // PremiumStatus = hfBoatNature.Value.Trim(),

                    NoOfPass = txtNoPersons.Text.Trim(),
                    NoOfChild = "0",
                    NoOfInfant = "0",
                    OfferId = "0",

                    InitBillAmount = bsTotal.InnerText.Trim(),
                    PaymentType = ddlPaymentType.SelectedValue.Trim(),
                    ActualBillAmount = bsTotal.InnerText.Trim(), //need what amount this;
                    Status = "B",
                    BoatTypeId = sBoatTypeIds,

                    BoatSeaterId = sSeaterTypeIds,
                    BookingDuration = sBoatMinTime,
                    InitBoatCharge = sInitBoatCharges,
                    InitRowerCharge = sRowerMinCharge,
                    BoatDeposit = sBoatDeposits,

                    //TaxDetails = ViewState["TaxAmountDetlBoat"].ToString().Trim('~'),
                    CGSTTaxAmount = sCGSTTaxAmount,
                    SGSTTaxAmount = sSGSTTaxAmount,
                    InitOfferAmount = sboatOfferAmount,
                    InitNetAmount = sInitNetAmounts,
                    CreatedBy = sUserId,

                    ////Other Service Booking
                    OthServiceStatus = sOthServiceStatus,
                    OthServiceId = sOthServiceId,
                    OthChargePerItem = sOthChargePerItem,
                    OthNoOfItems = sOthNoOfItems,
                    CGSTOthTaxAmount = sCGSTOthTaxAmount,
                    SGSTOthTaxAmount = sSGSTOthTaxAmount,
                    // OthTaxDetails = ViewState["OthTaxDetails"].ToString().Trim('~'),
                    OthNetAmount = sOthNetAmount,
                    BookingMedia = "DW",

                    BFDInitBoatCharges = sBFDInitBoatCharge,
                    BFDInitNetAmounts = sBFDInitNetAmount,
                    BFDGSTs = sBFDGST,
                    CustomerGSTNo = GSTNO,
                    CollectedAmount = CollectedAmount.Trim(),
                    BalanceAmount = BalanceAmount.Trim(),

                    Countslotids = sLoopCount,
                    BookingTimeSlotId = sSlotIds,
                    BookingBlockId = sBlockId,
                    PremiumStatus = HeaderStatus,
                    BoatPremiumStatus = sStatus,
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatBookingBulk", BoatBook).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        string[] sResult = ResponseMsg.Split('~');

                        string[] Pin = sResult[2].Split(',');
                        string Pins = Pin[0].Replace("\"", string.Empty).Trim();
                        string Pinss = Pins.Replace("]", string.Empty).Trim();

                        if (ViewState["PINType"].ToString().Trim() == "D")
                        {

                            if (chkCustMobileNo.Checked == true)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Boat Booking Successfully !');", true);
                            }
                            else
                            {
                                Response.Redirect("PrintBoat.aspx?rt=bb&bi=" + Pinss + "");
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Boat Booking Successfully !');", true);
                        }

                        divBack.Style.Add("background-color", "white");
                        ClearBooking();
                        hfNatureVisible.Value = "0";
                        premimumMsg.Visible = false;
                        premimumMsg.InnerText = "";

                        hfBoatNature.Value = "N";
                        BoatBookedSummaryList();
                        dtlOther.Visible = false;
                        divBack.Style.Add("background-color", "white");
                        txtINSGSTNO.Text = string.Empty;
                        divGST.Visible = false;
                        chkGSTNo.Checked = false;
                    }
                    else
                    {
                        string BookingId = GetBookingIdByPin();

                        if (BookingId.Trim() != "")
                        {
                            Response.Redirect("PrintBoat.aspx?rt=bb&bi=" + BookingId.Trim() + "");
                            return;
                        }

                        txtINSGSTNO.Text = string.Empty;
                        divGST.Visible = false;
                        chkGSTNo.Checked = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
                    }
                }
                else
                {
                    txtINSGSTNO.Text = string.Empty;
                    divGST.Visible = false;
                    chkGSTNo.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            txtINSGSTNO.Text = string.Empty;
            divGST.Visible = false;
            chkGSTNo.Checked = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void imgbtnNewBook_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divGridList.Visible = false;
            divEntry.Visible = true;
            divShow.Visible = false;
            imgbtnNewBook.Visible = false;
            imgbtnBookedList.Visible = true;
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
        dvContent.Attributes.Add("style", "display:none;");
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

    protected void imgbtnBookedList_Click(object sender, ImageClickEventArgs e)
    {
        BackToList.Visible = false;
        txtSearch.Text = string.Empty;
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);

        BookedListdtl();

    }
    public void BookedListdtl()
    {
        ViewState["Flag"] = "T";
        try
        {

            divSearch.Visible = true;
            divShow.Visible = true;
            imgbtnNewBook.Visible = true;
            imgbtnBookedList.Visible = false;

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
                        UserId = "Admin",
                        CountStart = ViewState["hfstartvalue"].ToString()
                    };
                    response = client.PostAsJsonAsync("BoatBookedListV2", BoatSearch).Result;
                }
                else
                {
                    var BoatSearch = new BoatSearch()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingType = "B",
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = ViewState["hfstartvalue"].ToString()
                    };
                    response = client.PostAsJsonAsync("BoatBookedListV2", BoatSearch).Result;
                }


                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {

                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count < 10)
                        {
                            Next.Enabled = false;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            GvBoatBooking.DataSource = dt;
                            GvBoatBooking.DataBind();
                            GvBoatBooking.Visible = true;
                            lblGridMsg.Visible = false;
                            //Newly added by Brijin and Imran on 2022-05-24
                            if (UserRole == "Admin")
                            {
                                GvBoatBooking.Columns[9].Visible = true;
                            }
                            else
                            {
                                GvBoatBooking.Columns[9].Visible = false;
                            }

                        }
                        else
                        {
                            GvBoatBooking.Visible = false;
                            lblGridMsg.Text = "No Boat Booking Today...!";
                        }
                    }
                    else
                    {
                        GvBoatBooking.Visible = false;
                        Next.Enabled = false;
                        divSearch.Visible = false;

                        if (ResponseMsg.Trim() == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
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
            return;
        }
    }
    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        if (ViewState["Flag"].ToString() == "S")
        {
            BookedListdtlSinglePin();
        }
        else
        {
            BookedListdtl();
        }

    }

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        if (ViewState["Flag"].ToString() == "S")
        {
            BookedListdtlSinglePin();
        }
        else
        {
            BookedListdtl();
        }

    }

    protected void AddProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;
            back.Enabled = false;
            Next.Enabled = true;

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
            Next.Enabled = true;
            back.Enabled = false;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            back.Enabled = false;
            Next.Enabled = true;

        }
        else
        {
            iend = end;
            Next.Enabled = true;

        }
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
    }
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            dvContent.Attributes.Add("style", "display:none;");
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
            dvContent.Attributes.Add("style", "display:none;");
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
                    UserId = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonAPIMethod", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    //var ticktList = response.Content.ReadAsStringAsync().Result;
                    //int StatusCode = Convert.ToInt32(JObject.Parse(ticktList)["StatusCode"].ToString());
                    //string ResponseMsg = JObject.Parse(ticktList)["Response"].ToString();               


                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    //if (StatusCode == 1)
                    //{
                    //    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        DLReceipt.DataSource = dtExists;
                        DLReceipt.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Boat Booking Today...!');", true);
                    }
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    //}
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
            btnBoatBooking.Enabled = true;
            ddlPaymentType.SelectedIndex = 0;

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
            ViewState["Count"] = string.Empty;
            ViewState["sGST"] = string.Empty;
            ViewState["CGSTTaxAmount"] = string.Empty;
            ViewState["SGSTTaxAmount"] = string.Empty;
            ViewState["sUserId"] = string.Empty;
            ViewState["SlotIds"] = string.Empty;
            ViewState["BlockIdss"] = string.Empty;
            ViewState["Status"] = string.Empty;
            ViewState["Counts"] = string.Empty;
            ViewState["OthServiceStatus"] = "N";
            ViewState["OthServiceId"] = "";
            ViewState["OthChargePerItem"] = "";
            ViewState["OthNoOfItems"] = "";
            ViewState["OthNetAmount"] = "";
            ViewState["OthTaxDetails"] = "";
            ViewState["CGSTOthTaxAmount"] = "";
            ViewState["SGSTOthTaxAmount"] = "";
            ViewState["boatOfferAmount"] = "";

            bschar1.InnerText = "";
            oschar1.InnerText = "";
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
            btnDefaultPin.Visible = true;
            ViewState["PINType"] = "";

            ViewState["CartRow"] = null;
            ViewState["Row"] = null;

            ViewState["CartRowO"] = null;
            ViewState["RowO"] = null;

            ViewState["BoatChargeSum"] = "0";
            ViewState["BoatTaxSum"] = "0";
            ViewState["BoatDepositSum"] = "0";
            ViewState["BoatTotalSum"] = "0";

            ClearTempValues();
            txtNoPersons.Text = "";
            txtNoPersons.Enabled = true;
            txtNoPersons.Focus();

            gvBoatdtl.Visible = false;
            gvOther.DataBind();
            dtlOther.DataBind();

            GetPaymentType();

            btnBoatBooking.Text = "Submit";

            divOfferDiscount.Visible = false;
            lblBoatChargeOnly.Text = "";
            lblOfferAmountAndTaxAmount.Text = "0";
            lblNoofPersonAlert.Text = "";
            lblOfferAlert.Text = "";
            lblInfo.Visible = false;

            chkCustMobileNo.Enabled = true;
            txtCustMobileNo.Text = "";
            divCustMobile.Visible = false;

            txtAmountPaid.Text = "";
            txtBalanceAmnt.Text = "";
            hfBalanceAmt.Value = "";

            txtNoPersons.Focus();
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
            dvContent.Attributes.Add("style", "display:none;");

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
                                        SlotType = row.Field<string>("SlotType"),
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
                                        PersonCount = t1.Sum(a => a.Field<Int32>("PersonCount")),
                                        BoatTotalCharge = t1.Sum(a => a.Field<decimal>("BoatTotalCharge")) + t1.Sum(b => b.Field<decimal>("Deposit")),

                                        BoatMinCharge = t1.Key.BoatMinCharge,
                                        RowerMinCharge = t1.Key.RowerMinCharge,
                                        BoatTaxCharge = t1.Key.BoatTaxCharge,

                                        DepositType = t1.Key.DepositType,
                                        Deposit = t1.Key.Deposit,
                                        BoatMinTime = t1.Key.BoatMinTime,
                                        SlotType = t1.Key.SlotType,
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
                         h["PersonCount"] = g.PersonCount;
                         h["BoatTotalCharge"] = g.BoatTotalCharge;

                         h["BoatMinCharge"] = g.BoatMinCharge;
                         h["RowerMinCharge"] = g.RowerMinCharge;
                         h["BoatTaxCharge"] = g.BoatTaxCharge;

                         h["DepositType"] = g.DepositType;
                         h["Deposit"] = g.Deposit;
                         h["BoatMinTime"] = g.BoatMinTime;
                         h["SlotType"] = g.SlotType;
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

                //Over all selected Person count.
                ViewState["OverAllSelectedPersonCount"] = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Int32>("PersonCount")));

                decimal dDeposit = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Deposit")));
                //bsdeposit1.InnerText = dDeposit.ToString();
                ViewState["BoatDepositSum"] = dDeposit.ToString();

                decimal dTotal = Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge) + Convert.ToDecimal(dGSTAmount) + Convert.ToDecimal(dDeposit);
                bsTotal.InnerText = dTotal.ToString();
                ViewState["BoatTotalSum"] = dTotal.ToString();

                txtNoPersons.Enabled = false;
            }
            else
            {
                gvBoatdtl.DataBind();

                ViewState["CartRow"] = null;
                ViewState["Row"] = null;
                gvBoatdtl.Visible = false;

                bschar1.InnerText = "";
                bsgst1.InnerText = "";
                bsdeposit1.InnerText = "";
                bsTotal.InnerText = "";
                btnBoatBooking.Text = "";

                txtNoPersons.Enabled = true;
                txtNoPersons.Focus();

                //Over all selected Person count.
                ViewState["OverAllSelectedPersonCount"] = "0";
                txtPIN.Text = "";
            }
            if (hfchkoffer.Value == "1")
            {
                ViewState["OfferAmountTotal"] = "0";
                hfchkoffer.Value = "0";
            }


            CalculateSummary();
            ResetDiscountInput();

            BookingPersonAlert(Convert.ToDecimal(txtNoPersons.Text.Trim()), Convert.ToDecimal(ViewState["OverAllSelectedPersonCount"].ToString()));

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

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
    //protected void ImgBtnDelete_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ViewState["BoatChargeSum"] = "0";
    //        ViewState["BoatTaxSum"] = "0";
    //        ViewState["BoatDepositSum"] = "0";
    //        ViewState["BoatTotalSum"] = "0";
    //        lblInfo.Visible = false;
    //        ImageButton lnkbtn = sender as ImageButton;
    //        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
    //        string sUniqueId = gvBoatdtl.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();

    //        DataTable dtCurrentTable = (DataTable)ViewState["CartRow"];

    //        for (int i = dtCurrentTable.Rows.Count - 1; i >= 0; i--)
    //        {
    //            DataRow dr = dtCurrentTable.Rows[i];
    //            if (dr["UniqueId"].ToString().Trim() == sUniqueId.Trim())
    //            {
    //                dr.Delete();
    //            }
    //        }

    //        dtCurrentTable.AcceptChanges();

    //        DataTable dtCurrentTable1 = (DataTable)ViewState["Row"];

    //        for (int i = dtCurrentTable1.Rows.Count - 1; i >= 0; i--)
    //        {
    //            DataRow dr = dtCurrentTable1.Rows[i];
    //            if (dr["UniqueId"].ToString().Trim() == sUniqueId.Trim())
    //            {
    //                dr.Delete();
    //            }
    //        }

    //        dtCurrentTable.AcceptChanges();
    //        dtCurrentTable1.AcceptChanges();

    //        if (dtCurrentTable.Rows.Count > 0)
    //        {
    //            gvBoatdtl.Visible = true;
    //            gvBoatdtl.DataSource = dtCurrentTable;
    //            gvBoatdtl.DataBind();

    //            DataTable dtTable = (DataTable)ViewState["Row"];

    //            decimal dBoatMinCharge = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatMinCharge")));
    //            decimal dRowerMinCharge = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("RowerMinCharge")));
    //            //bschar1.InnerText = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();
    //            ViewState["BoatChargeSum"] = (Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge)).ToString();


    //            decimal dGSTAmount = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatTaxCharge")));
    //            //bsgst1.InnerText = dGSTAmount.ToString();
    //            ViewState["BoatTaxSum"] = dGSTAmount.ToString();

    //            //Over all selected Person count.
    //            ViewState["OverAllSelectedPersonCount"] = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Int32>("PersonCount")));

    //            decimal dDeposit = dtTable.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("Deposit")));
    //            //bsdeposit1.InnerText = dDeposit.ToString();
    //            ViewState["BoatDepositSum"] = dDeposit.ToString();

    //            decimal dTotal = Convert.ToDecimal(dBoatMinCharge) + Convert.ToDecimal(dRowerMinCharge) + Convert.ToDecimal(dGSTAmount) + Convert.ToDecimal(dDeposit);
    //            bsTotal.InnerText = dTotal.ToString();
    //            ViewState["BoatTotalSum"] = dTotal.ToString();

    //            txtNoPersons.Enabled = false;
    //        }
    //        else
    //        {
    //            gvBoatdtl.DataBind();

    //            ViewState["CartRow"] = null;
    //            ViewState["Row"] = null;
    //            gvBoatdtl.Visible = false;

    //            bschar1.InnerText = "";
    //            bsgst1.InnerText = "";
    //            bsdeposit1.InnerText = "";
    //            bsTotal.InnerText = "";
    //            btnBoatBooking.Text = "";

    //            txtNoPersons.Enabled = true;
    //            txtNoPersons.Focus();

    //            //Over all selected Person count.
    //            ViewState["OverAllSelectedPersonCount"] = "0";
    //            txtPIN.Text = "";
    //        }
    //        if (hfchkoffer.Value == "1")
    //        {
    //            ViewState["OfferAmountTotal"] = "0";
    //            hfchkoffer.Value = "0";
    //        }


    //        CalculateSummary();
    //        ResetDiscountInput();

    //        BookingPersonAlert(Convert.ToDecimal(txtNoPersons.Text.Trim()), Convert.ToDecimal(ViewState["OverAllSelectedPersonCount"].ToString()));

    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);

    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            dvContent.Attributes.Add("style", "display:none;");
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

    protected void btnNor_Click(object sender, EventArgs e)
    {
        //    if (CheckBoatSelectionStatus("P") == true)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sorry, Already Selected an Premium Boat Booking !');", true);
        //        return;
        //    }

        divBack.Style.Add("background-color", "white");
        // ClearBooking();
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

        divBack.Attributes.Add("style", "background-image: repeating-linear-gradient(90deg, #bf9b30, #ffe34d, #ffcf40, #ffe34d, #e6c200, #ccac00);margin-right:-2px;margin-left:-10px;margin-bottom:5px;");
        //ClearBooking();
        hfNatureVisible.Value = "0";
        premimumMsg.Visible = false;
        premimumMsg.InnerText = "";

        hfBoatNature.Value = "P";
        BoatBookedSummaryList();
        dtlOther.Visible = false;
        divBack.Attributes.Add("style", "background-image: repeating-linear-gradient(90deg, #bf9b30, #ffe34d, #ffcf40, #ffe34d, #e6c200, #ccac00);margin-right:-2px;margin-left:-10px;margin-bottom:5px;");
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
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

            if (Convert.ToDecimal(btnBoatBooking.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Boat !');", true);
                return;
            }

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
            dvContent.Attributes.Add("style", "display:none;");
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

    //private void CalculateSummary()
    //{
    //    bschar1.InnerText = ViewState["BoatChargeSum"].ToString();
    //    oschar1.InnerText = ViewState["OtherChargeSum"].ToString();
    //    bsgst1.InnerText = (Convert.ToDecimal(ViewState["BoatTaxSum"].ToString()) + Convert.ToDecimal(ViewState["OtherTaxSum"].ToString())).ToString();
    //    bsdeposit1.InnerText = ViewState["BoatDepositSum"].ToString();

    //    bsTotal.InnerText = ViewState["BoatTotalSum"].ToString();

    //    btnBoatBooking.Text = (Convert.ToDecimal(bschar1.InnerText) + Convert.ToDecimal(oschar1.InnerText) + Convert.ToDecimal(bsgst1.InnerText) + Convert.ToDecimal(bsdeposit1.InnerText)).ToString();

    //    bsTotal.InnerText = btnBoatBooking.Text;

    //    if (Convert.ToDecimal(ViewState["BoatTotalSum"].ToString()) >= 1)
    //    {
    //        divPin.Visible = true;
    //        txtPIN.ReadOnly = false;
    //    }
    //    else
    //    {
    //        divPin.Visible = false;
    //        txtPIN.ReadOnly = true;

    //        bschar1.InnerText = "";
    //        bsgst1.InnerText = "";
    //        bsdeposit1.InnerText = "";
    //        bsTotal.InnerText = "";
    //        oschar1.InnerText = "";
    //        btnBoatBooking.Text = "";

    //        divBooking.Visible = false;
    //    }
    //}

    private void CalculateSummary()
    {
        bschar1.InnerText = ViewState["BoatChargeSum"].ToString();
        oschar1.InnerText = ViewState["OtherChargeSum"].ToString();
        bsgst1.InnerText = (Convert.ToDecimal(ViewState["BoatTaxSum"].ToString()) + Convert.ToDecimal(ViewState["OtherTaxSum"].ToString())).ToString();
        bsdeposit1.InnerText = ViewState["BoatDepositSum"].ToString();

        toolTipDpt.Visible = true;
        toolTipDpt.InnerText = "Charge Includes Refundable Deposit of ₹ " + " " + ViewState["BoatDepositSum"].ToString();

        //bsTotal.InnerText = ViewState["BoatTotalSum"].ToString();

        bsTotal.InnerText = (Convert.ToDecimal(bschar1.InnerText) + Convert.ToDecimal(oschar1.InnerText) + Convert.ToDecimal(bsgst1.InnerText)
                       + Convert.ToDecimal(bsdeposit1.InnerText)).ToString();

        btnBoatBooking.Text = (Convert.ToDecimal(bschar1.InnerText) + Convert.ToDecimal(oschar1.InnerText) + Convert.ToDecimal(bsgst1.InnerText)
                           + Convert.ToDecimal(bsdeposit1.InnerText) - (Convert.ToDecimal(ViewState["OfferAmountTotal"].ToString()))).ToString();

        //bsTotal.InnerText = btnBooatBooking.Text;

        if (Convert.ToDecimal(ViewState["BoatTotalSum"].ToString()) >= 1)
        {
            divPin.Visible = true;
            divOfferDiscount.Visible = true;

            if (txtPIN.Text == "")
            {
                btnDefaultPin.Visible = true;
            }
            if (btnDefaultPin.Visible == true)
            {
                txtPIN.ReadOnly = false;
            }

            GetOfferDiscount();


            chkCustMobileNo.Enabled = false;

            if (chkCustMobileNo.Checked == true)
            {
                divCustMobile.Visible = true;
                divCustPin.Visible = false;
            }
            else
            {
                divCustPin.Visible = true;
                divCustMobile.Visible = false;
            }
        }
        else
        {
            divPin.Visible = false;
            txtPIN.ReadOnly = true;

            bschar1.InnerText = "";
            bsgst1.InnerText = "";
            bsdeposit1.InnerText = "";
            bsTotal.InnerText = "";
            oschar1.InnerText = "";
            btnBoatBooking.Text = "";

            divBooking.Visible = false;

            divOfferDiscount.Visible = false;
            chkCustMobileNo.Enabled = true;
            divCustMobile.Visible = false;
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

                        HttpResponseMessage response = client.PostAsJsonAsync("OtherSvcCatDet", service).Result;

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
        try
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

            //BindDataDynamicValueOthers(ServiceId, CategoryName + " - " + ServiceName, ServiceTotalAmount, ChargePerItem, ChargePerItemTax, AdultCount, TaxId, TaxName);

            ViewState["TOServiceId"] = ServiceId;
            ViewState["TOServiceName"] = CategoryName + " - " + ServiceName;
            ViewState["TOServiceTotalAmount"] = ServiceTotalAmount;
            ViewState["TOChargePerItem"] = ChargePerItem;
            ViewState["TOChargePerItemTax"] = ChargePerItemTax;
            ViewState["TOTaxId"] = TaxId;
            ViewState["TOTaxName"] = TaxName;

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "addItemOther();", true);

            Panel pnlTicketCount = (Panel)((DataList)dtlOther).Controls[0].Controls[0].FindControl("pnlTicketCount");
            pnlTicketCount.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
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
                for (int i = 0; i < Convert.ToInt32(AdultCount); i++)
                {
                    dr = mytableO.NewRow();

                    dr["UniqueId"] = ServiceId.Trim();
                    dr["ServiceId"] = ServiceId.Trim();
                    dr["ServiceName"] = ServiceName.Trim();

                    dr["ServiceTotalAmount"] = ServiceTotalAmount.Trim();
                    dr["ChargePerItem"] = ChargePerItem.Trim();
                    dr["ChargePerItemTax"] = ChargePerItemTax.Trim();
                    dr["AdultCount"] = 1;

                    dr["TaxId"] = TaxId;
                    dr["TaxName"] = TaxName.Trim();

                    dr["OtherGrandTotalAmount"] = 0;

                    mytableO.Rows.Add(dr);
                }

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

            for (int i = 0; i < Convert.ToInt32(AdultCount); i++)
            {
                dr1 = mytableO.NewRow();

                dr1["UniqueId"] = ServiceId.Trim();
                dr1["ServiceId"] = ServiceId.Trim();
                dr1["ServiceName"] = ServiceName.Trim();

                dr1["ServiceTotalAmount"] = ServiceTotalAmount.Trim();
                dr1["ChargePerItem"] = ChargePerItem.Trim();
                dr1["ChargePerItemTax"] = ChargePerItemTax.Trim();
                dr1["AdultCount"] = 1;

                dr1["TaxId"] = TaxId;
                dr1["TaxName"] = TaxName.Trim();

                dr1["OtherGrandTotalAmount"] = 0;

                mytableO.Rows.Add(dr1);
            }

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
                                divBooking.Visible = true;
                                btnDefaultPin.Visible = false;
                                ViewState["PINType"] = "U";

                                if (Session["DeptPaymentRights"].ToString().Trim() == "N")
                                {
                                    ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                                    ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
                                }
                                else
                                {
                                    if (Session["DeptOnlineRights"].ToString().Trim() == "N")
                                    {
                                        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                                    }

                                    if (Session["DeptUPIRights"].ToString().Trim() == "N")
                                    {
                                        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                                    }
                                }
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
                                btnDefaultPin.Visible = true;
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
                        divBooking.Visible = true;
                        lblUserMobileNo.Text = "";
                        imgPinStatus.ImageUrl = "";
                        btnDefaultPin.Visible = false;

                        ViewState["CUserId"] = "";
                        ViewState["CName"] = "";
                        ViewState["PINType"] = "D";
                        ViewState["CMailId"] = "";

                        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));

                        // 2023-01-28 Abhinaya blocked for UPI payment UPI like CASH.
                        //ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
                    }
                    else
                    {
                        txtPIN.Text = "";
                        txtPIN.ReadOnly = false;
                        divBooking.Visible = false;
                        btnDefaultPin.Visible = true;

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

    public void OnlineBeforeTransactionDetails(string ServiceType, string CustUserId)
    {
        try
        {
            ViewState["TranStatus"] = "N";

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
                    BookingPin = txtPIN.Text.Trim(),

                    ///UserId = ViewState["CUserId"].ToString().Trim(),

                    UserId = CustUserId.Trim(),
                    MobileNo = lblUserMobileNo.Text.Trim(),
                    EmailId = ViewState["CMailId"].ToString().Trim(),

                    PaymentMode = "Online",
                    Amount = bsTotal.InnerText.Trim(),
                    BookingType = "B",

                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    PremiumStatus = hfBoatNature.Value.Trim(),

                    NoOfPass = "0",
                    NoOfChild = "0",
                    NoOfInfant = "0",

                    BoatTypeId = ViewState["BoatTypeIds"].ToString().Trim('~'),
                    BoatSeaterId = ViewState["BoatSeaterIds"].ToString().Trim('~'),
                    BookingDuration = ViewState["BoatMinTimes"].ToString().Trim('~'),

                    InitBoatCharge = ViewState["InitBoatCharges"].ToString().Trim('~'),
                    InitRowerCharge = ViewState["RowerMinCharge"].ToString().Trim('~'),
                    BoatDeposit = ViewState["BoatDeposits"].ToString().Trim('~'),

                    InitOfferAmount = ViewState["boatOfferAmount"].ToString().Trim('~'),
                    InitNetAmount = ViewState["InitNetAmounts"].ToString().Trim('~'),
                    TaxDetails = ViewState["TaxAmountDetlBoat"].ToString().Trim('~'),

                    // Other Service Booking

                    OthServiceStatus = ViewState["OthServiceStatus"].ToString().Trim('~'),
                    OthServiceId = ViewState["OthServiceId"].ToString().Trim('~'),
                    OthChargePerItem = ViewState["OthChargePerItem"].ToString().Trim('~'),
                    OthNoOfItems = ViewState["OthNoOfItems"].ToString().Trim('~'),
                    OthTaxDetails = ViewState["OthTaxDetails"].ToString().Trim('~'),
                    OthNetAmount = ViewState["OthNetAmount"].ToString().Trim('~'),

                    BookingMedia = "PW",

                    BFDInitBoatCharge = ViewState["BFDInitBoatCharge"].ToString().Trim('~'),
                    BFDInitNetAmount = ViewState["BFDInitNetAmount"].ToString().Trim('~'),
                    BFDGST = ViewState["BFDGST"].ToString().Trim('~'),

                    EntryType = "BB",
                    ModuleType = "Boating"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("OnlineBoatBookingBfrTran", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        string[] sResult = ResponseMsg.Split('~');

                        ViewState["TranStatus"] = "Y";

                        SendSMS(ServiceType, lblUserMobileNo.Text.Trim(), sResult[1].ToString().Trim(), bsTotal.InnerText.Trim());
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
                        //ServiceType = "DOnlineBooking",

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

    /// <summary>
    /// Abhinaya
    /// </summary>
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
                            // ViewState["SlotType"] = dt.Rows[0]["SlotStartTime"].ToString().Trim() + "-" + dt.Rows[0]["SlotEndTime"].ToString().Trim();
                            ViewState["SlotType"] = dt.Rows[0]["SlotStartTime"].ToString().Trim();

                            ViewState["BlockId"] += dt.Rows[0]["BlockId"].ToString().Trim() + "~";

                            ViewState["sBlockId"] += dt.Rows[0]["BlockId"].ToString().Trim() + "~";
                            ViewState["sSlotId"] += dt.Rows[0]["SlotId"].ToString().Trim() + "~";
                            //  ViewState["sSlotType"] += dt.Rows[0]["SlotStartTime"].ToString().Trim() + "-" + dt.Rows[0]["SlotEndTime"].ToString().Trim() + "~";

                            ViewState["sSlotType"] += dt.Rows[0]["SlotStartTime"].ToString().Trim() + "~";
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
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Slot Is Not Available');", true);
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
    /// Created by - Abhinaya
    /// Modified by - Preetika
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dtlBoat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["OfferType"] = "";
            ViewState["OfferPorF"] = "0";
            ViewState["OfferAmountTotal"] = 0;
            ViewState["OverAllSelectedPersonCount"] = 0;
            ViewState["sSlotId"] = "";
            ViewState["sSlotType"] = "";
            ViewState["BlockId"] = "";

            if (ddlOfferDiscount.SelectedIndex != 0)
            {
                ResetDiscountInput();
            }

            DropDownList ddl = sender as DropDownList;
            DataListItem item = ddl.NamingContainer as DataListItem;

            DropDownList lstValue = item.FindControl("dlstBoatCount") as DropDownList;

            if (txtNoPersons.Text == "")
            {
                txtNoPersons.Focus();
                lstValue.SelectedIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Enter No of Person !');", true);
                return;
            }

            if (ViewState["TBoatTypeName"].ToString().Trim() == null || ViewState["TBoatTypeName"].ToString().Trim() == "")
            {
                lstValue.SelectedIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Any One Boat Seater !');", true);
                return;
            }

            string BoatCount = lstValue.SelectedValue.Trim();

            string BoatTypeName = ViewState["TBoatTypeName"].ToString().Trim();
            string BoatTypeId = ViewState["TBoatTypeId"].ToString().Trim();

            string BoatSeaterType = ViewState["TBoatSeaterType"].ToString().Trim();
            string BoatSeaterId = ViewState["TBoatSeaterId"].ToString().Trim();
            string BoatSeaterCount = ViewState["TBoatSeaterCount"].ToString().Trim();

            string BoatTotalCharge = ViewState["TBoatTotalCharge"].ToString().Trim();
            string BoatMinCharge = ViewState["TBoatMinCharge"].ToString().Trim();
            string RowerMinCharge = ViewState["TRowerMinCharge"].ToString().Trim();
            string BoatTaxCharge = ViewState["TBoatTaxCharge"].ToString().Trim();

            string DepositType = ViewState["TDepositType"].ToString().Trim();
            string BoatDepositAmount = ViewState["TBoatDepositAmount"].ToString().Trim();
            string BoatMinTime = ViewState["TBoatMinTime"].ToString().Trim();

            int NoPerson = Convert.ToInt32(txtNoPersons.Text);
            int CheckCount = Convert.ToInt32(BoatCount) * Convert.ToInt32(BoatSeaterCount);
            string BoatNature = string.Empty;

            if (hfBoatNature.Value == "N")
            {
                BoatNature = "Normal";
            }
            else if (hfBoatNature.Value == "P")
            {
                BoatNature = "Express";
            }

            ViewState["SlotBoatTypeId"] = BoatTypeId.ToString().Trim();
            ViewState["SlotBoatSeaterId"] = BoatSeaterId.ToString().Trim();
            for (int i = 1; i <= Convert.ToInt32(BoatCount); i++)
            {
                BindSlotType();
            }
            string SlotId;
            string[] sSlotId;
            SlotId = ViewState["sSlotId"].ToString();
            sSlotId = SlotId.Split('~');
            //Added
            ViewState["NewSlot"] = sSlotId[0].Trim();


            if (ViewState["sSlotId"].ToString().Trim() != "" && ViewState["AvailableTripCount"].ToString().Trim() != "")
            {
                if (ViewState["NewSlot"].ToString().Trim() != "")
                {
                    BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatCount, BoatTotalCharge, BoatMinCharge,
                RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount, BoatMinTime, BoatSeaterCount, BoatNature,
                ViewState["sSlotId"].ToString(), ViewState["sSlotType"].ToString(), ViewState["BlockId"].ToString());
                }
                else
                {
                    ViewState["sSlotId"] = "";
                    ViewState["AvailableTripCount"] = "0";
                    ViewState["sSlotType"] = "";
                    ViewState["BlockId"] = "";
                    //if (ViewState["OkFlag"].ToString() == "O")
                    //{
                    MpepnlPopup.Hide();
                    for (int i = 1; i <= Convert.ToInt32(BoatCount); i++)
                    {
                        BindLastSlot();
                    }

                    if (ViewState["sSlotId"].ToString().Trim() != "")
                    {
                        if (ViewState["NewSlotStatus"].ToString().Trim() == "A")
                        {
                            BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatCount, BoatTotalCharge, BoatMinCharge,
                       RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount, BoatMinTime, BoatSeaterCount, BoatNature,
                       ViewState["sSlotId"].ToString(), ViewState["sSlotType"].ToString(), ViewState["BlockId"].ToString());
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Slot Is Not Available');", true);
                            return;

                        }
                    }
                }
            }
            else
            {
                ViewState["sSlotId"] = "";
                ViewState["AvailableTripCount"] = "0";
                ViewState["sSlotType"] = "";
                ViewState["BlockId"] = "";
                //if (ViewState["OkFlag"].ToString() == "O")
                //{
                MpepnlPopup.Hide();
                for (int i = 1; i <= Convert.ToInt32(BoatCount); i++)
                {
                    BindLastSlot();
                }

                if (ViewState["sSlotId"].ToString().Trim() != "")
                {

                    BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatCount, BoatTotalCharge, BoatMinCharge,
                   RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount, BoatMinTime, BoatSeaterCount, BoatNature,
                   ViewState["sSlotId"].ToString(), ViewState["sSlotType"].ToString(), ViewState["BlockId"].ToString());
                }
                //}

                //else
                //{

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
                //    ViewState["BoatDepositAmountLastSlot"] = BoatDepositAmount;
                //    ViewState["BoatMinTimeLastSlot"] = BoatMinTime;
                //    ViewState["BoatSeaterCountLastSlot"] = BoatSeaterCount;
                //    MpepnlPopup.Show();
                //}
            }

            lstValue.SelectedIndex = 0;
            ClearTempValues();

            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "myColor();", true);

            Panel pnlBoatCount = (Panel)((DataList)dtlBoat).Controls[0].FindControl("pnlBoatCount");
            pnlBoatCount.Visible = false;


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    //protected void dtlBoat_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ViewState["OfferType"] = "";
    //        ViewState["OfferPorF"] = "0";
    //        ViewState["OfferAmountTotal"] = 0;
    //        ViewState["OverAllSelectedPersonCount"] = 0;
    //        ViewState["sSlotId"] = "";
    //        ViewState["sSlotType"] = "";
    //        ViewState["BlockId"] = "";

    //        if (ddlOfferDiscount.SelectedIndex != 0)
    //        {
    //            ResetDiscountInput();
    //        }

    //        DropDownList ddl = sender as DropDownList;
    //        DataListItem item = ddl.NamingContainer as DataListItem;

    //        DropDownList lstValue = item.FindControl("dlstBoatCount") as DropDownList;

    //        if (txtNoPersons.Text == "")
    //        {
    //            txtNoPersons.Focus();
    //            lstValue.SelectedIndex = 0;
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Enter No of Person !');", true);
    //            return;
    //        }

    //        if (ViewState["TBoatTypeName"].ToString().Trim() == null || ViewState["TBoatTypeName"].ToString().Trim() == "")
    //        {
    //            lstValue.SelectedIndex = 0;
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Any One Boat Seater !');", true);
    //            return;
    //        }

    //        string BoatCount = lstValue.SelectedValue.Trim();

    //        string BoatTypeName = ViewState["TBoatTypeName"].ToString().Trim();
    //        string BoatTypeId = ViewState["TBoatTypeId"].ToString().Trim();

    //        string BoatSeaterType = ViewState["TBoatSeaterType"].ToString().Trim();
    //        string BoatSeaterId = ViewState["TBoatSeaterId"].ToString().Trim();
    //        string BoatSeaterCount = ViewState["TBoatSeaterCount"].ToString().Trim();

    //        string BoatTotalCharge = ViewState["TBoatTotalCharge"].ToString().Trim();
    //        string BoatMinCharge = ViewState["TBoatMinCharge"].ToString().Trim();
    //        string RowerMinCharge = ViewState["TRowerMinCharge"].ToString().Trim();
    //        string BoatTaxCharge = ViewState["TBoatTaxCharge"].ToString().Trim();

    //        string DepositType = ViewState["TDepositType"].ToString().Trim();
    //        string BoatDepositAmount = ViewState["TBoatDepositAmount"].ToString().Trim();
    //        string BoatMinTime = ViewState["TBoatMinTime"].ToString().Trim();

    //        int NoPerson = Convert.ToInt32(txtNoPersons.Text);
    //        int CheckCount = Convert.ToInt32(BoatCount) * Convert.ToInt32(BoatSeaterCount);
    //        string BoatNature = string.Empty;

    //        if (hfBoatNature.Value == "N")
    //        {
    //            BoatNature = "Normal";
    //        }
    //        else if (hfBoatNature.Value == "P")
    //        {
    //            BoatNature = "Express";
    //        }

    //        ViewState["SlotBoatTypeId"] = BoatTypeId.ToString().Trim();
    //        ViewState["SlotBoatSeaterId"] = BoatSeaterId.ToString().Trim();
    //        for (int i = 1; i <= Convert.ToInt32(BoatCount); i++)
    //        {
    //            BindSlotType();
    //        }
    //        string SlotId;
    //        string[] sSlotId;
    //        SlotId = ViewState["sSlotId"].ToString();
    //        sSlotId = SlotId.Split('~');
    //        //Added
    //        ViewState["NewSlot"] = sSlotId[0].Trim();

    //        if (ViewState["sSlotId"].ToString().Trim() != "" && ViewState["AvailableTripCount"].ToString().Trim() != "")
    //        {
    //            BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatCount, BoatTotalCharge, BoatMinCharge,
    //            RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount, BoatMinTime, BoatSeaterCount, BoatNature,
    //            ViewState["sSlotId"].ToString(), ViewState["sSlotType"].ToString(), ViewState["BlockId"].ToString());
    //        }
    //        else
    //        {
    //            ViewState["sSlotId"] = "";
    //            ViewState["AvailableTripCount"] = "0";
    //            ViewState["sSlotType"] = "";
    //            ViewState["BlockId"] = "";
    //            //if (ViewState["OkFlag"].ToString() == "O")
    //            //{
    //                MpepnlPopup.Hide();
    //                for (int i = 1; i <= Convert.ToInt32(BoatCount); i++)
    //                {
    //                    BindLastSlot();
    //                }

    //                if (ViewState["sSlotId"].ToString().Trim() != "")
    //                {

    //                    BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatCount, BoatTotalCharge, BoatMinCharge,
    //                   RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount, BoatMinTime, BoatSeaterCount, BoatNature,
    //                   ViewState["sSlotId"].ToString(), ViewState["sSlotType"].ToString(), ViewState["BlockId"].ToString());
    //                }
    //            //}

    //            //else
    //            //{

    //            //    ViewState["BoatTypeNameLastSlot"] = BoatTypeName;

    //            //    ViewState["BoatTypeIdLastSlot"] = BoatTypeId;

    //            //    ViewState["BoatSeaterIdLastSlot"] = BoatSeaterId;
    //            //    ViewState["BoatSeaterTypeLastSlot"] = BoatSeaterType;

    //            //    ViewState["BoatNatureLastSlot"] = BoatNature;

    //            //    ViewState["BoatCountLastSlot"] = BoatCount;
    //            //    ViewState["BoatTotalChargeLastSlot"] = BoatTotalCharge;

    //            //    ViewState["BoatMinChargeLastSlot"] = BoatMinCharge;

    //            //    ViewState["RowerMinChargeLastSlot"] = RowerMinCharge;
    //            //    ViewState["BoatTaxChargeLastSlot"] = BoatTaxCharge;
    //            //    ViewState["DepositTypeLastSlot"] = DepositType;
    //            //    ViewState["BoatDepositAmountLastSlot"] = BoatDepositAmount;
    //            //    ViewState["BoatMinTimeLastSlot"] = BoatMinTime;
    //            //    ViewState["BoatSeaterCountLastSlot"] = BoatSeaterCount;
    //            //    MpepnlPopup.Show();
    //            //}
    //        }

    //        lstValue.SelectedIndex = 0;
    //        ClearTempValues();

    //        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "myColor();", true);

    //        Panel pnlBoatCount = (Panel)((DataList)dtlBoat).Controls[0].FindControl("pnlBoatCount");
    //        pnlBoatCount.Visible = false;


    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //}
    protected void BtnPopUpCancel_Click(object sender, EventArgs e)
    {
        MpepnlPopup.Hide();
        ViewState["OkFlag"] = "P";
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        for (int i = 1; i <= Convert.ToInt32(ViewState["BoatCountLastSlot"]); i++)
        {
            BindLastSlot();
        }

        if (ViewState["sSlotId"].ToString().Trim() != "")
        {
            ViewState["OkFlag"] = "O";

            BindDataDynamicValue(ViewState["BoatTypeNameLastSlot"].ToString(), ViewState["BoatTypeIdLastSlot"].ToString(), ViewState["BoatSeaterTypeLastSlot"].ToString(),
            ViewState["BoatSeaterIdLastSlot"].ToString(), ViewState["BoatCountLastSlot"].ToString(),
            ViewState["BoatTotalChargeLastSlot"].ToString(), ViewState["BoatMinChargeLastSlot"].ToString(),
            ViewState["RowerMinChargeLastSlot"].ToString(), ViewState["BoatTaxChargeLastSlot"].ToString(),
            ViewState["DepositTypeLastSlot"].ToString(), ViewState["BoatDepositAmountLastSlot"].ToString(),
            ViewState["BoatMinTimeLastSlot"].ToString(), ViewState["BoatSeaterCountLastSlot"].ToString(), ViewState["BoatNatureLastSlot"].ToString(),
            ViewState["sSlotId"].ToString(), ViewState["sSlotType"].ToString(), ViewState["BlockId"].ToString());


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
            dvContent.Attributes.Add("style", "display:none;");
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
                            ViewState["SlotId"] += dt.Rows[0]["SlotId"].ToString().Trim();
                            //  ViewState["SlotType"] += dt.Rows[0]["SlotStartTime"].ToString().Trim() + "-" + dt.Rows[0]["SlotEndTime"].ToString().Trim();
                            ViewState["SlotType"] += dt.Rows[0]["SlotStartTime"].ToString().Trim();
                            if (Convert.ToInt32(dt.Rows[0]["BlockId"].ToString()) > 0)
                            {
                                ViewState["BlockId"] += dt.Rows[0]["BlockId"].ToString().Trim() + "~";
                                ViewState["sSlotId"] += dt.Rows[0]["SlotId"].ToString().Trim() + "~";
                                //  ViewState["sSlotType"] += dt.Rows[0]["SlotStartTime"].ToString().Trim() + "-" + dt.Rows[0]["SlotEndTime"].ToString().Trim() + "~";
                                ViewState["sSlotType"] += dt.Rows[0]["SlotStartTime"].ToString().Trim() + "~";
                                ViewState["sBlockId"] += dt.Rows[0]["BlockId"].ToString().Trim() + "~";

                                ViewState["SlotFlag"] = "Y";

                                if (hfBoatNature.Value == "N" || hfBoatNature.Value == "P")
                                {
                                    ViewState["AvailableTripCount"] = dt.Rows[0]["AvailableBoatCount"].ToString().Trim();
                                }

                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Slot Is Not Available');", true);
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
    private void BookingPersonAlert(decimal dTotalPerson, decimal dSelectedPerson)
    {
        if (Convert.ToDecimal(dSelectedPerson) < dTotalPerson && dSelectedPerson != 0)
        {
            lblNoofPersonAlert.Text = "Please select additional boats for " + (dTotalPerson - dSelectedPerson) + " more people !!!";
            btnBoatBooking.Enabled = false;
            lblNoofPersonAlert.Attributes.Add("style", "Color:#DC143C");

        }
        else if (dSelectedPerson > dTotalPerson)
        {
            lblNoofPersonAlert.Text = "You have booked " + (dSelectedPerson - dTotalPerson) + " seats more than what you required !!!";
            btnBoatBooking.Enabled = true;
            lblNoofPersonAlert.Attributes.Add("style", "Color:#5cbf2a");
        }
        else if (dSelectedPerson == dTotalPerson)
        {
            lblNoofPersonAlert.Text = "Boat selection & Number of people are matching !!!";
            btnBoatBooking.Enabled = true;
            lblNoofPersonAlert.Attributes.Add("style", "Color:#114a79");
        }
        else
        {
            lblNoofPersonAlert.Text = "";
            btnBoatBooking.Enabled = true;
            lblOfferAlert.Text = "";
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void dlstTicketsCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = sender as DropDownList;
            DataListItem item = ddl.NamingContainer as DataListItem;

            DropDownList lstValue = item.FindControl("dlstTicketsCount") as DropDownList;

            if (ViewState["TOServiceId"].ToString().Trim() == null || ViewState["TOServiceId"].ToString().Trim() == "")
            {
                lstValue.SelectedIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Any One Service Name !');", true);
                return;
            }

            string TicketsCount = lstValue.SelectedValue.Trim();

            string ServiceId = ViewState["TOServiceId"].ToString().Trim();
            string ServiceName = ViewState["TOServiceName"].ToString().Trim();
            string ServiceTotalAmount = ViewState["TOServiceTotalAmount"].ToString().Trim();
            string ChargePerItem = ViewState["TOChargePerItem"].ToString().Trim();
            string ChargePerItemTax = ViewState["TOChargePerItemTax"].ToString().Trim();
            string TaxId = ViewState["TOTaxId"].ToString().Trim();
            string TaxName = ViewState["TOTaxName"].ToString().Trim();

            BindDataDynamicValueOthers(ServiceId, ServiceName, ServiceTotalAmount, ChargePerItem, ChargePerItemTax, TicketsCount, TaxId, TaxName);

            lstValue.SelectedIndex = 0;
            ClearTempValues();

            Panel pnlTicketCount = (Panel)((DataList)dtlOther).Controls[0].FindControl("pnlTicketCount");
            pnlTicketCount.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
            dvContent.Attributes.Add("style", "display:none;");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void ClearTempValues()
    {
        ViewState["TBoatTypeName"] = "";
        ViewState["TBoatTypeId"] = "";
        ViewState["TBoatSeaterType"] = "";
        ViewState["TBoatSeaterId"] = "";
        ViewState["TBoatSeaterCount"] = "";
        ViewState["TBoatTotalCharge"] = "";
        ViewState["TBoatMinCharge"] = "";
        ViewState["TRowerMinCharge"] = "";
        ViewState["TBoatTaxCharge"] = "";
        ViewState["TDepositType"] = "";
        ViewState["TBoatDepositAmount"] = "";
        ViewState["TBoatMinTime"] = "";

        ViewState["TOServiceId"] = "";
        ViewState["TOServiceName"] = "";
        ViewState["TOServiceTotalAmount"] = "";
        ViewState["TOChargePerItem"] = "";
        ViewState["TOChargePerItemTax"] = "";
        ViewState["TOTaxId"] = "";
        ViewState["TOTaxName"] = "";
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    // Deposit Part Details

    public void GetOfferDiscountlist()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var boatmaster = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ddlOffer/BHId", boatmaster).Result;

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
                            ddlOfferDiscount.DataSource = dt;
                            ddlOfferDiscount.DataValueField = "OfferId";
                            ddlOfferDiscount.DataTextField = "OfferName";
                            ddlOfferDiscount.DataBind();
                        }
                        else
                        {
                            ddlOfferDiscount.DataBind();
                        }

                        //ddlOfferDiscount.Items.Insert(0, "Select Offer-Discount");
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();

                    }

                    ddlOfferDiscount.Items.Insert(0, "Select Offer-Discount");
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

    protected void ddlOfferDiscount_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetOfferDiscountDetails();
        CalculateSummary();
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    public void GetOfferDiscountDetails()
    {
        try
        {
            ViewState["OfferType"] = "";
            ViewState["OfferPorF"] = "0";
            ViewState["OfferAmountTotal"] = 0;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var boatmaster = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    OfferId = ddlOfferDiscount.SelectedValue.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("OfferMstr/OfferEdit", boatmaster).Result;

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
                            ViewState["MinBillAmount"] = dt.Rows[0]["MinBillAmount"].ToString();

                            if (Convert.ToDecimal(ViewState["BoatChargeSum"].ToString()) >= Convert.ToDecimal(ViewState["MinBillAmount"].ToString()))
                            {

                                ViewState["OfferType"] = dt.Rows[0]["AmountType"].ToString();
                                ViewState["OfferPorF"] = dt.Rows[0]["Offer"].ToString();

                                if (ViewState["OfferType"].ToString() == "P")
                                {
                                    BoatBookingFinal();

                                    //lblBoatChargeOnly.Text = "(" + ViewState["OnlyBoatchargeSumAmount"].ToString() + "-" + dt.Rows[0]["Offer"] + "%)";
                                    lblBoatChargeOnly.Text = "(" + dt.Rows[0]["Offer"] + " %)";
                                    lblOfferAmountAndTaxAmount.Text = ViewState["OfferAmountTotal"].ToString() + " (Exclusive of Deposit)";
                                    hfchkoffer.Value = "1";
                                }
                                else if (ViewState["OfferType"].ToString() == "F")
                                {
                                    BoatBookingFinal();

                                    //lblBoatChargeOnly.Text = "(" + ViewState["OnlyBoatchargeSumAmount"].ToString() + "-" + dt.Rows[0]["Offer"] + "₹)";
                                    lblBoatChargeOnly.Text = "( ₹ " + dt.Rows[0]["Offer"] + ")";
                                    lblOfferAmountAndTaxAmount.Text = ViewState["OfferAmountTotal"].ToString() + " (Exclusive of Deposit)";
                                    hfchkoffer.Value = "1";
                                }
                                else
                                {
                                    lblBoatChargeOnly.Text = "";
                                    lblOfferAmountAndTaxAmount.Text = "0";
                                    ViewState["OnlyBoatchargeSumAmount"] = 0;
                                    ViewState["OfferAmountTotal"] = 0;
                                }
                            }
                            else
                            {
                                ddlOfferDiscount.SelectedIndex = 0;
                                ViewState["OnlyBoatchargeSumAmount"] = 0;
                                ViewState["OfferAmountTotal"] = 0;
                                lblBoatChargeOnly.Text = "";
                                lblOfferAmountAndTaxAmount.Text = "0";
                                lblOfferAlert.Visible = false;
                                lblInfo.Enabled = true;
                                lblInfo.Visible = true;
                                lblInfo.Text = "Sorry, You are not Eligible for Offer/Discount !!!";

                            }

                        }
                    }
                    else
                    {
                        ViewState["OfferType"] = "";
                        ViewState["OfferPorF"] = 0;

                        //lblBoatChargeOnly.Text = "";
                        lblOfferAmountAndTaxAmount.Text = "0";
                        ViewState["OnlyBoatchargeSumAmount"] = 0;
                        ViewState["OfferAmountTotal"] = 0;

                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
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
    /// <summary>
    /// Abhinaya
    /// </summary>
    private void BoatBookingFinal()
    {
        try
        {
            ViewState["BoatTypeIds"] = "";
            ViewState["BoatSeaterIds"] = "";
            ViewState["BoatMinTimes"] = "";
            ViewState["BFDInitBoatCharge"] = "";
            ViewState["BFDInitNetAmount"] = "";
            ViewState["BFDGST"] = "";
            ViewState["InitBoatCharges"] = "";
            ViewState["RowerMinCharge"] = "";
            ViewState["TaxAmountDetlBoat"] = "";
            ViewState["BoatDeposits"] = "";
            ViewState["InitNetAmount"] = "";
            ViewState["InitNetAmounts"] = "";
            ViewState["BoatChargeTotal"] = "";
            ViewState["RowerChargeTotal"] = "";
            ViewState["BoatDepositTotal"] = "";
            ViewState["BoatTaxTotal"] = "";
            ViewState["boatOfferAmount"] = "";
            ViewState["OnlyBoatchargeSumAmount"] = "";
            ViewState["OfferAmountTotal"] = "0";
            ViewState["FinalOfferNetAmount"] = "0";
            ViewState["CGSTTaxAmount"] = "";
            ViewState["SGSTTaxAmount"] = "";
            ViewState["Counts"] = "";
            ViewState["Status"] = "";
            ViewState["Count"] = "";
            ViewState["BlockIdss"] = "";
            ViewState["SlotIds"] = "";
            ViewState["sUserId"] = "";

            //--Incase Other service not booking assign default value N//

            ViewState["OthServiceStatus"] = "N";

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
                ViewState["boatOfferAmount"] = "0";

                decimal iOfferChargeOneByOne = 0;
                decimal iOnlyBoatchargeSumAmount = 0;

                var lblTax = ViewState["TaxPercentBoat"].ToString();
                decimal taxOnebytwo = 0;

                if (lblTax != "")
                {
                    string[] taxlist = lblTax.Split(',');

                    foreach (var list in taxlist)
                    {
                        var TaxName = list;
                        var tx = list.Split('-');
                        taxOnebytwo = Convert.ToDecimal(tx[1]);
                    }
                }

                ViewState["BFDInitBoatCharge"] = "";
                ViewState["BFDInitNetAmount"] = "";
                ViewState["BFDGST"] = "";

                foreach (GridViewRow item in gvBoatdtl.Rows)
                {
                    string cBoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
                    string cSeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();

                    Label NumberOfBoat = (Label)item.FindControl("lblBoatCount");
                    int iNumofBoat = Convert.ToInt32(NumberOfBoat.Text.Trim());


                    for (int i = 1; i <= iNumofBoat; i++)
                    {
                        string BoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
                        ViewState["BoatTypeIds"] += BoatTypeId.Trim() + '~';

                        string SeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();
                        ViewState["BoatSeaterIds"] += SeaterTypeId.Trim() + '~';

                        string BoatMinTime = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinTime"].ToString().Trim();
                        ViewState["BoatMinTimes"] += BoatMinTime.Trim() + '~';

                        /////////////////New

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

                        int Count = i;

                        ViewState["Count"] += Count.ToString().Trim() + '~';
                        ////////////////////////NEW
                        string BoatMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinCharge"].ToString().Trim();
                        string RowerMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["RowerMinCharge"].ToString().Trim();
                        string BoatTaxCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatTaxCharge"].ToString().Trim();
                        decimal iBoatDeposit = Convert.ToDecimal(gvBoatdtl.DataKeys[item.RowIndex]["Deposit"].ToString().Trim());

                        string BFDInitBoatCharge = BoatMinCharge.ToString();
                        string BFDInitNetAmount = (Convert.ToDecimal(BoatMinCharge) + Convert.ToDecimal(RowerMinCharge) + Convert.ToDecimal(BoatTaxCharge) + Convert.ToDecimal(iBoatDeposit)).ToString();

                        string BFDInitNetAmountOffer = (Convert.ToDecimal(BoatMinCharge) + Convert.ToDecimal(RowerMinCharge) + Convert.ToDecimal(BoatTaxCharge)).ToString();
                        string BFDGST = BoatTaxCharge.ToString();

                        ViewState["BFDInitBoatCharge"] += BFDInitBoatCharge.Trim() + '~';
                        ViewState["BFDInitNetAmount"] += BFDInitNetAmount.Trim() + '~';
                        ViewState["BFDGST"] += BFDGST.Trim() + '~';

                        decimal iBoatMinCharge = 0;
                        decimal iRowerMinCharge = 0;
                        decimal OfferAmount = 0;

                        decimal BoatTotalTaxAmt = 0;
                        string TaxDtl = string.Empty;
                        string TaxAmount = string.Empty;
                        string SAmountArray;
                        string[] AmountArray;



                        if (ViewState["OfferType"].ToString() == "P")
                        {
                            //decimal BoatChargeWoDeposit = Convert.ToDecimal(BoatMinCharge.Trim()) + Convert.ToDecimal(RowerMinCharge.Trim())
                            //    + Convert.ToDecimal(BoatTaxCharge.Trim());

                            decimal BoatChargeWoDeposit = Convert.ToDecimal(BoatMinCharge.Trim());

                            iOnlyBoatchargeSumAmount = (iOnlyBoatchargeSumAmount + BoatChargeWoDeposit);

                            OfferAmount = Math.Truncate(Convert.ToDecimal(BoatChargeWoDeposit) * Convert.ToDecimal(ViewState["OfferPorF"].ToString()) / 100);
                            decimal BoatChargeAfterDiscount = (Convert.ToDecimal(BFDInitNetAmountOffer) - OfferAmount);
                            iOfferChargeOneByOne = (iOfferChargeOneByOne + OfferAmount);

                            GetBoatChargeAndTaxAfterDiscount(BoatChargeAfterDiscount, Convert.ToDecimal(RowerMinCharge), taxOnebytwo, 2);//  2 is times of tax calculation 

                            //= ROUND(C3 / 118 * 9, 2) + ROUND(C3 / 118 * 9, 2)                                                     

                            iBoatMinCharge = Convert.ToDecimal(ViewState["BoatChargeAfterDiscount"].ToString());
                            ViewState["InitBoatCharges"] += iBoatMinCharge.ToString() + '~';

                            iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
                            ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';


                            if (lblTax != "")
                            {
                                string[] taxlist = lblTax.Split(',');

                                decimal TaxAmt = Convert.ToDecimal(ViewState["BoatTaxAfterDiscount"].ToString()) / 2;

                                foreach (var list in taxlist)
                                {
                                    var TaxName = list;
                                    var tx = list.Split('-');

                                    TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                                    ///New
                                    TaxAmount += (TaxAmt + "#").Trim();
                                    ///New
                                    BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
                                }
                            }


                            ViewState["sGSTAmount"] = TaxAmount.ToString();

                            SAmountArray = ViewState["sGSTAmount"].ToString();
                            AmountArray = SAmountArray.Split('#');
                        }
                        else if (ViewState["OfferType"].ToString() == "F")
                        {
                            //decimal BoatChargeWoDeposit = Convert.ToDecimal(BoatMinCharge.Trim()) + Convert.ToDecimal(RowerMinCharge.Trim())
                            //   + Convert.ToDecimal(BoatTaxCharge.Trim());

                            decimal BoatChargeWoDeposit = Convert.ToDecimal(BoatMinCharge.Trim());

                            iOnlyBoatchargeSumAmount = iOnlyBoatchargeSumAmount + BoatChargeWoDeposit;

                            OfferAmount = Math.Truncate(Convert.ToDecimal(BoatChargeWoDeposit) / Convert.ToDecimal(ViewState["AllBoatChargeForDiscountCalc"].ToString()) * Convert.ToDecimal(ViewState["OfferPorF"].ToString()));



                            if (Convert.ToDecimal(ViewState["OfferPorF"].ToString()) <= iOfferChargeOneByOne)
                            {
                                decimal val = iOfferChargeOneByOne - Convert.ToDecimal(ViewState["OfferPorF"].ToString());
                                OfferAmount = val;
                            }

                            decimal BoatChargeAfterDiscount = Convert.ToDecimal(BFDInitNetAmountOffer) - OfferAmount;
                            iOfferChargeOneByOne = iOfferChargeOneByOne + OfferAmount;

                            GetBoatChargeAndTaxAfterDiscount(BoatChargeAfterDiscount, Convert.ToDecimal(RowerMinCharge), taxOnebytwo, 2);//  2 is times of tax calculation 

                            iBoatMinCharge = Convert.ToDecimal(ViewState["BoatChargeAfterDiscount"].ToString());
                            ViewState["InitBoatCharges"] += iBoatMinCharge.ToString() + '~';

                            iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
                            ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';

                            if (lblTax != "")
                            {
                                string[] taxlist = lblTax.Split(',');

                                decimal TaxAmt = Convert.ToDecimal(ViewState["BoatTaxAfterDiscount"].ToString()) / 2;

                                foreach (var list in taxlist)
                                {
                                    var TaxName = list;
                                    var tx = list.Split('-');

                                    TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();

                                    ///New
                                    TaxAmount += (TaxAmt + "#").Trim();
                                    ///New
                                    BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
                                }
                            }
                            ViewState["sGSTAmount"] = TaxAmount.ToString();

                            SAmountArray = ViewState["sGSTAmount"].ToString();
                            AmountArray = SAmountArray.Split('#');
                        }
                        else
                        {
                            iBoatMinCharge = Convert.ToDecimal(BoatMinCharge.Trim());
                            ViewState["InitBoatCharges"] += BoatMinCharge.Trim() + '~';

                            iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
                            ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';

                            ViewState["BoatTaxAfterDiscount"] = Convert.ToDecimal(BoatTaxCharge.Trim());

                            if (lblTax != "")
                            {
                                string[] taxlist = lblTax.Split(',');

                                decimal TaxAmt = Convert.ToDecimal(ViewState["BoatTaxAfterDiscount"].ToString()) / 2;

                                foreach (var list in taxlist)
                                {
                                    var TaxName = list;
                                    var tx = list.Split('-');

                                    TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();

                                    ///New
                                    TaxAmount += (TaxAmt + "#").Trim();
                                    ///New

                                    BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
                                }
                            }
                            ViewState["sGSTAmount"] = TaxAmount.ToString();

                            SAmountArray = ViewState["sGSTAmount"].ToString();
                            AmountArray = SAmountArray.Split('#');
                        }

                        ViewState["CGSTTaxAmount"] += AmountArray[0].ToString() + '~';
                        ViewState["SGSTTaxAmount"] += AmountArray[1].ToString() + '~';

                        ViewState["TaxAmountDetlBoat"] += Convert.ToString(TaxDtl.ToString() + '~');


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

                        ViewState["boatOfferAmount"] += OfferAmount + "~";

                        ViewState["UserId"] = Session["UserId"].ToString();
                        ViewState["sUserId"] += ViewState["UserId"].ToString() + '~';
                    }
                }

                ViewState["OnlyBoatchargeSumAmount"] = iOnlyBoatchargeSumAmount;
                ViewState["OfferAmountTotal"] = iOfferChargeOneByOne;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

        try
        {
            DataTable BlockList = (DataTable)ViewState["Row"];
            for (int i = 0; i < BlockList.Rows.Count; i++)
            {
                string BlockId = BlockList.Rows[i]["BlockId"].ToString().Trim();
                ViewState["BlockIdss"] += BlockId.Trim() + '~';

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    private void BoatBookingFinal123()
    {
        try
        {
            ViewState["BoatTypeIds"] = "";
            ViewState["BoatSeaterIds"] = "";
            ViewState["BoatMinTimes"] = "";
            ViewState["BFDInitBoatCharge"] = "";
            ViewState["BFDInitNetAmount"] = "";
            ViewState["BFDGST"] = "";
            ViewState["InitBoatCharges"] = "";
            ViewState["RowerMinCharge"] = "";
            ViewState["TaxAmountDetlBoat"] = "";
            ViewState["BoatDeposits"] = "";
            ViewState["InitNetAmount"] = "";
            ViewState["InitNetAmounts"] = "";
            ViewState["BoatChargeTotal"] = "";
            ViewState["RowerChargeTotal"] = "";
            ViewState["BoatDepositTotal"] = "";
            ViewState["BoatTaxTotal"] = "";
            ViewState["boatOfferAmount"] = "";
            ViewState["OnlyBoatchargeSumAmount"] = "";
            ViewState["OfferAmountTotal"] = "";
            ViewState["FinalOfferNetAmount"] = "";

            //--Incase Other service not booking assign default value N//

            ViewState["OthServiceStatus"] = "N";

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
                ViewState["boatOfferAmount"] = "0";

                decimal iOfferChargeOneByOne = 0;
                decimal iOnlyBoatchargeSumAmount = 0;

                var lblTax = ViewState["TaxPercentBoat"].ToString();
                decimal taxOnebytwo = 0;

                if (lblTax != "")
                {
                    string[] taxlist = lblTax.Split(',');

                    foreach (var list in taxlist)
                    {
                        var TaxName = list;
                        var tx = list.Split('-');
                        taxOnebytwo = Convert.ToDecimal(tx[1]);
                    }
                }

                ViewState["BFDInitBoatCharge"] = "";
                ViewState["BFDInitNetAmount"] = "";
                ViewState["BFDGST"] = "";

                foreach (GridViewRow item in gvBoatdtl.Rows)
                {
                    Label NumberOfBoat = (Label)item.FindControl("lblBoatCount");
                    int iNumofBoat = Convert.ToInt32(NumberOfBoat.Text.Trim());

                    for (int i = 1; i <= iNumofBoat; i++)
                    {
                        string BoatTypeId = gvBoatdtl.DataKeys[item.RowIndex]["BoatTypeId"].ToString().Trim();
                        ViewState["BoatTypeIds"] += BoatTypeId.Trim() + '~';

                        string SeaterTypeId = gvBoatdtl.DataKeys[item.RowIndex]["SeaterTypeId"].ToString().Trim();
                        ViewState["BoatSeaterIds"] += SeaterTypeId.Trim() + '~';

                        string BoatMinTime = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinTime"].ToString().Trim();
                        ViewState["BoatMinTimes"] += BoatMinTime.Trim() + '~';


                        string BoatMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatMinCharge"].ToString().Trim();
                        string RowerMinCharge = gvBoatdtl.DataKeys[item.RowIndex]["RowerMinCharge"].ToString().Trim();
                        string BoatTaxCharge = gvBoatdtl.DataKeys[item.RowIndex]["BoatTaxCharge"].ToString().Trim();
                        decimal iBoatDeposit = Convert.ToDecimal(gvBoatdtl.DataKeys[item.RowIndex]["Deposit"].ToString().Trim());

                        string BFDInitBoatCharge = BoatMinCharge.ToString();
                        string BFDInitNetAmount = (Convert.ToDecimal(BoatMinCharge) + Convert.ToDecimal(RowerMinCharge) + Convert.ToDecimal(BoatTaxCharge) + Convert.ToDecimal(iBoatDeposit)).ToString();
                        string BFDGST = BoatTaxCharge.ToString();

                        ViewState["BFDInitBoatCharge"] += BFDInitBoatCharge.Trim() + '~';
                        ViewState["BFDInitNetAmount"] += BFDInitNetAmount.Trim() + '~';
                        ViewState["BFDGST"] += BFDGST.Trim() + '~';

                        decimal iBoatMinCharge = 0;
                        decimal iRowerMinCharge = 0;
                        decimal OfferAmount = 0;

                        decimal BoatTotalTaxAmt = 0;
                        string TaxDtl = string.Empty;

                        if (ViewState["OfferType"].ToString() == "P")
                        {
                            decimal BoatChargeWoDeposit = Convert.ToDecimal(BoatMinCharge.Trim());
                            iOnlyBoatchargeSumAmount = (iOnlyBoatchargeSumAmount + BoatChargeWoDeposit);

                            OfferAmount = Math.Round(Convert.ToDecimal(BoatChargeWoDeposit) * Convert.ToDecimal(ViewState["OfferPorF"].ToString()) / 100);
                            decimal BoatChargeAfterDiscount = (Convert.ToDecimal(BFDInitNetAmount) - OfferAmount);
                            iOfferChargeOneByOne = (iOfferChargeOneByOne + OfferAmount);

                            GetBoatChargeAndTaxAfterDiscount(BoatChargeAfterDiscount, Convert.ToDecimal(RowerMinCharge), taxOnebytwo, 2);//  2 is times of tax calculation 

                            //= ROUND(C3 / 118 * 9, 2) + ROUND(C3 / 118 * 9, 2)                                                     

                            iBoatMinCharge = Convert.ToDecimal(ViewState["BoatChargeAfterDiscount"].ToString());
                            ViewState["InitBoatCharges"] += iBoatMinCharge.ToString() + '~';

                            iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
                            ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';


                            if (lblTax != "")
                            {
                                string[] taxlist = lblTax.Split(',');

                                decimal TaxAmt = Convert.ToDecimal(ViewState["BoatTaxAfterDiscount"].ToString()) / 2;

                                foreach (var list in taxlist)
                                {
                                    var TaxName = list;
                                    var tx = list.Split('-');

                                    TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                                    BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
                                }
                            }
                        }
                        else if (ViewState["OfferType"].ToString() == "F")
                        {

                            decimal BoatChargeWoDeposit = Convert.ToDecimal(BoatMinCharge.Trim());

                            iOnlyBoatchargeSumAmount = iOnlyBoatchargeSumAmount + BoatChargeWoDeposit;

                            OfferAmount = Math.Round(Convert.ToDecimal(BoatChargeWoDeposit) / Convert.ToDecimal(ViewState["AllBoatChargeForDiscountCalc"].ToString()) * Convert.ToDecimal(ViewState["OfferPorF"].ToString()));



                            if (Convert.ToDecimal(ViewState["OfferPorF"].ToString()) <= iOfferChargeOneByOne)
                            {
                                decimal val = iOfferChargeOneByOne - Convert.ToDecimal(ViewState["OfferPorF"].ToString());
                                OfferAmount = val;
                            }

                            decimal BoatChargeAfterDiscount = Convert.ToDecimal(BFDInitNetAmount) - OfferAmount;
                            iOfferChargeOneByOne = iOfferChargeOneByOne + OfferAmount;

                            GetBoatChargeAndTaxAfterDiscount(BoatChargeAfterDiscount, Convert.ToDecimal(RowerMinCharge), taxOnebytwo, 2);//  2 is times of tax calculation 

                            iBoatMinCharge = Convert.ToDecimal(ViewState["BoatChargeAfterDiscount"].ToString());
                            ViewState["InitBoatCharges"] += iBoatMinCharge.ToString() + '~';

                            iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
                            ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';


                            if (lblTax != "")
                            {
                                string[] taxlist = lblTax.Split(',');

                                decimal TaxAmt = Convert.ToDecimal(ViewState["BoatTaxAfterDiscount"].ToString()) / 2;

                                foreach (var list in taxlist)
                                {
                                    var TaxName = list;
                                    var tx = list.Split('-');

                                    TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                                    BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
                                }
                            }
                        }
                        else
                        {
                            iBoatMinCharge = Convert.ToDecimal(BoatMinCharge.Trim());
                            ViewState["InitBoatCharges"] += BoatMinCharge.Trim() + '~';

                            iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
                            ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';

                            ViewState["BoatTaxAfterDiscount"] = Convert.ToDecimal(BoatTaxCharge.Trim());

                            if (lblTax != "")
                            {
                                string[] taxlist = lblTax.Split(',');

                                decimal TaxAmt = Convert.ToDecimal(ViewState["BoatTaxAfterDiscount"].ToString()) / 2;

                                foreach (var list in taxlist)
                                {
                                    var TaxName = list;
                                    var tx = list.Split('-');

                                    TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                                    BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
                                }
                            }
                        }

                        ViewState["TaxAmountDetlBoat"] += Convert.ToString(TaxDtl.ToString() + '~');


                        decimal BoatDepositAmount = 0;
                        BoatDepositAmount = iBoatDeposit;

                        ViewState["BoatDeposits"] += BoatDepositAmount.ToString() + '~';

                        ViewState["InitNetAmount"] = iBoatMinCharge + iRowerMinCharge + BoatDepositAmount + BoatTotalTaxAmt.ToString();

                        ViewState["InitNetAmounts"] += ViewState["InitNetAmount"].ToString() + '~';

                        // Do whatever you need with that string value here

                        ViewState["BoatChargeTotal"] = (Convert.ToDecimal(ViewState["BoatChargeTotal"]) + iBoatMinCharge).ToString();

                        ViewState["RowerChargeTotal"] = (Convert.ToDecimal(ViewState["RowerChargeTotal"]) + iRowerMinCharge).ToString();

                        ViewState["BoatDepositTotal"] = (Convert.ToDecimal(ViewState["BoatDepositTotal"]) + BoatDepositAmount).ToString();

                        ViewState["BoatTaxTotal"] = (Convert.ToDecimal(ViewState["BoatTaxTotal"]) + BoatTotalTaxAmt).ToString();

                        ViewState["boatOfferAmount"] += OfferAmount + "~";
                    }
                }


                ViewState["OnlyBoatchargeSumAmount"] = iOnlyBoatchargeSumAmount;
                ViewState["OfferAmountTotal"] = iOfferChargeOneByOne;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

        //= ROUND(C3 / 118 * 9, 2) + ROUND(C3 / 118 * 9, 2)
        //dBoatTaxAfterDiscount = Math.Round((Convert.ToDecimal(BoatChargeAfterDiscount) / (100 + Convert.ToDecimal(taxOnebytwo) * 2) * Convert.ToDecimal(taxOnebytwo)), 2)
        //                      + Math.Round((Convert.ToDecimal(BoatChargeAfterDiscount) / (100 + Convert.ToDecimal(taxOnebytwo) * 2) * Convert.ToDecimal(taxOnebytwo)), 2);
        //iBoatMinCharge = BoatChargeAfterDiscount - (Convert.ToDecimal(dBoatTaxAfterDiscount) + Convert.ToDecimal(RowerMinCharge.Trim()));

    }

    public void GetBoatChargeAndTaxAfterDiscount(decimal BoatCharge, decimal RowerCharge, decimal TaxPer, decimal sTimes)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://paypre.in/node/gst/myapp/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("gst/" + BoatCharge + "/" + RowerCharge + "/" + TaxPer + "/" + sTimes + "").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ResponseMsg = response.Content.ReadAsStringAsync().Result;
                    string[] BCharge = ResponseMsg.Split(':');

                    string[] bamt = BCharge[1].ToString().Split(',');
                    string[] bgst = BCharge[2].ToString().Split('}');

                    ViewState["BoatChargeAfterDiscount"] = bamt[0].ToString();
                    ViewState["BoatTaxAfterDiscount"] = bgst[0].ToString();
                }
                else
                {
                    ViewState["BoatChargeAfterDiscount"] = 0;
                    ViewState["BoatTaxAfterDiscount"] = 0;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    private void ResetDiscountInput()
    {
        ddlOfferDiscount.SelectedIndex = 0;
        lblBoatChargeOnly.Text = "";

        lblOfferAmountAndTaxAmount.Text = "0";
        CalculateSummary();
    }

    /* Offer Discount Changes */

    public void GetOfferDiscount()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var boatmaster = new CategoryService()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = bschar1.InnerText
                };

                HttpResponseMessage response = client.PostAsJsonAsync("chkOfferApplicable", boatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        lblOfferAlert.Visible = true;
                        lblInfo.Enabled = false;
                        lblInfo.Visible = false;
                        lblOfferAlert.Text = "You are Eligible for Offer/Discount !!!";
                    }
                    else
                    {
                        lblOfferAlert.Text = "";
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


    protected void chkGSTNo_CheckedChanged(object sender, EventArgs e)
    {
        if (chkGSTNo.Checked == true)
        {
            divGST.Visible = true;
        }
        else
        {
            txtINSGSTNO.Text = string.Empty;
            divGST.Visible = false;
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
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
                        divBooking.Visible = true;
                        lblUserMobileNo.Text = "";
                        imgPinStatus.ImageUrl = "";
                        btnDefaultPin.Visible = false;

                        ViewState["CUserId"] = "";
                        ViewState["CName"] = "";
                        ViewState["PINType"] = "D";
                        ViewState["CMailId"] = "";

                        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                        ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
                    }
                    else
                    {
                        txtPIN.Text = "";
                        txtPIN.ReadOnly = false;
                        divBooking.Visible = false;
                        btnDefaultPin.Visible = true;

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
                        Response.Redirect("PrintBoat.aspx?rt=bb&bi=" + ViewState["BoOkingID"].ToString().Trim() + "");

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

                        if (dtExists.Rows[0]["Other"].ToString() == "" || dtExists.Rows[0]["Other"].ToString() == "0")
                        {
                            btnOther.Visible = false;

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
        public string CountStart { get; set; }

        public string Search { get; set; }
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

        public string CustomerGSTNo { get; set; }
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
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceId { get; set; }
        public string OthServiceId { get; set; }
        public string OthChargePerItem { get; set; }
        public string OthNoOfItems { get; set; }
        public string OthChildCharge { get; set; }
        public string OthNoOfChild { get; set; }
        public string OthTaxDetails { get; set; }
        public string OthNetAmount { get; set; }
        public string Category { get; set; }
        public string BookingPin { get; set; }

        public string BFDInitBoatCharge { get; set; }
        public string BFDInitNetAmount { get; set; }
        public string BFDGST { get; set; }
        public string EntryType { get; set; }
        public string ModuleType { get; set; }
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

    protected void bblblNetAmount_Click(object sender, EventArgs e)
    {
        BindBoatBookingRevenuePopup();
        dvContent.Attributes.Add("style", "display:none;");
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
        dvContent.Attributes.Add("style", "display:none;");
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

        public string[] BFDInitBoatCharges { get; set; }
        public string[] BFDInitNetAmounts { get; set; }
        public string[] BFDGSTs { get; set; }
        public string[] BookingTimeSlotId { get; set; }
        public string[] Countslotids { get; set; }
        public string[] BookingBlockId { get; set; }
        public string PremiumStatus { get; set; }
        public string[] CGSTTaxAmount { get; set; }
        public string[] SGSTTaxAmount { get; set; }

        public string[] CGSTOthTaxAmount { get; set; }

        public string[] SGSTOthTaxAmount { get; set; }

    }

    protected void BackToBooking_Click(object sender, EventArgs e)
    {
        Response.Redirect("BoatBookingFinal.aspx");
    }

    protected void BackToList_Click(object sender, EventArgs e)
    {
        back.Visible = true;
        Next.Visible = true;
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        BackToList.Visible = false;
        txtSearch.Text = string.Empty;
        BookedListdtl();

    }
    public void BookedListdtlSinglePin()
    {
        ViewState["Flag"] = "S";
        try
        {
            divSearch.Visible = true;
            divShow.Visible = true;
            imgbtnNewBook.Visible = true;
            imgbtnBookedList.Visible = false;

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
                    //if (UserRole == "Sadmin")
                    //{
                    //    GvBoatBooking.Columns[9].Visible = false;
                    //    GvBoatBooking.Columns[10].Visible = false;
                    //   // GvBoatBooking.Columns[11].Visible = false;
                    //}
                    //else
                    //{
                    //    GvBoatBooking.Columns[9].Visible = true;
                    //}

                    var BoatSearch = new BoatSearch()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingType = "B",
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "Admin",
                        CountStart = ViewState["hfstartvalue"].ToString(),
                        Search = txtSearch.Text

                    };
                    response = client.PostAsJsonAsync("BoatBookedListPinV2", BoatSearch).Result;
                }
                else
                {
                    var BoatSearch = new BoatSearch()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingType = "B",
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = ViewState["hfstartvalue"].ToString(),
                        Search = txtSearch.Text
                    };
                    response = client.PostAsJsonAsync("BoatBookedListPinV2", BoatSearch).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {

                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count < 10)
                        {
                            Next.Enabled = false;
                        }
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

                        GvBoatBooking.Visible = false;

                        divSearch.Visible = false;
                        if (ResponseMsg.Trim() == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                        Next.Enabled = false;
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
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        back.Visible = true;
        Next.Visible = true;
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        BookedListdtlSinglePin();
        BackToList.Visible = true;
    }
}