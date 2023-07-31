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

public partial class Boating_CreditBoatBooking : System.Web.UI.Page
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

                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    if (Session["BBMBulkBooking"].ToString().Trim() == "Y")
                    {
                        GetTaxDetail();
                        hfBoatNature.Value = "N";
                        BoatBookedSummaryList();
                        ClearTempValues();
                        txtNoPersons.Focus();

                        BindBookingCountAmount();
                        divBookedShow.Visible = true;
                    }
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
                    UserId = Session["UserId"].ToString().Trim(),
                    UserRole = Session["UserRole"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("NewBoatBookingRights", BoatSearch).Result;

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

            decimal TotalBoatCharge = Math.Round(Convert.ToDecimal(BoatMinCharge)) + Convert.ToDecimal(RowerMinCharge);

            ViewState["TBoatTypeName"] = BoatTypeName;
            ViewState["TBoatTypeId"] = BoatTypeId;
            ViewState["TBoatSeaterType"] = BoatSeaterType;
            ViewState["TBoatSeaterId"] = BoatSeaterId;
            ViewState["TBoatSeaterCount"] = BoatSeaterCount;
            ViewState["TBoatTotalCharge"] = TotalBoatCharge.ToString();
            ViewState["TBoatMinCharge"] = Math.Round(Convert.ToDecimal(BoatMinCharge)).ToString();
            ViewState["TRowerMinCharge"] = RowerMinCharge;
            ViewState["TBoatTaxCharge"] = "0";
            ViewState["TDepositType"] = DepositType;
            ViewState["TBoatDepositAmount"] = "0.00";
            ViewState["TBoatMinTime"] = BoatMinTime;

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
        string BoatMinCharge, string RowerMinCharge, string BoatTaxCharge, string DepositType, string Deposit, string BoatMinTime, string PersonCount)
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
                    DataRow[] result = mt.Select("BoatTypeId = '" + BoatTypeId.Trim() + "' AND SeaterTypeId ='" + SeaterTypeId.Trim() + "'");

                    //foreach (DataRow row in result)
                    //{
                    //    BookedboatCount = Convert.ToDecimal(row["BoatCount"]) + Convert.ToDecimal(BoatCount);

                    //    CheckBoatAvailableDetails(Session["BoatHouseId"].ToString().Trim(), BoatTypeId, SeaterTypeId, DateTime.Now.ToString("dd/MM/yyyy"));

                    //    if (hfBoatNature.Value.Trim() == "P")
                    //    {
                    //        if (BookedboatCount > Convert.ToDecimal(ViewState["cPremiumAvailable"].ToString()))
                    //        {
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Selected boat trips exceeding the maximum trip limit !!!');", true);
                    //            return;
                    //        }
                    //    }

                    //    if (hfBoatNature.Value.Trim() == "N")
                    //    {
                    //        if (BookedboatCount > Convert.ToDecimal(ViewState["cNormalAvailable"].ToString()))
                    //        {
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Selected boat trips exceeding the maximum trip limit !!!');", true);
                    //            return;
                    //        }
                    //    }
                    //}
                }
            }
            else
            {
                //BookedboatCount = Convert.ToDecimal(BoatCount);

                //CheckBoatAvailableDetails(Session["BoatHouseId"].ToString().Trim(), BoatTypeId, SeaterTypeId, DateTime.Now.ToString("dd/MM/yyyy"));

                //if (hfBoatNature.Value.Trim() == "P")
                //{
                //    if (BookedboatCount > Convert.ToDecimal(ViewState["cPremiumAvailable"].ToString()))
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Selected boat trips exceeding the maximum trip limit !!!');", true);
                //        return;
                //    }
                //}

                //if (hfBoatNature.Value.Trim() == "N")
                //{
                //    if (BookedboatCount > Convert.ToDecimal(ViewState["cNormalAvailable"].ToString()))
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Selected boat trips exceeding the maximum trip limit !!!');", true);
                //        return;
                //    }
                //}
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

                        dr["UniqueId"] = BoatTypeId.Trim() + "~" + SeaterTypeId.Trim();
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

                DataRow dr1 = mytable.NewRow();

                for (int i = 0; i < Convert.ToInt32(BoatCount); i++)
                {
                    dr1 = mytable.NewRow();

                    dr1["UniqueId"] = BoatTypeId.Trim() + "~" + SeaterTypeId.Trim();
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
                                 BoatMinTime = t1.Key.BoatMinTime
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

    protected void btnBoatBooking_Click(object sender, EventArgs e)
    {
        try
        {
            dvContent.Attributes.Add("style", "display:none;");
            btnBoatBooking.Enabled = false;

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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var BoatBook = new BoatBook()
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
                    PremiumStatus = hfBoatNature.Value.Trim(),

                    BoatTypeId = ViewState["BoatTypeIds"].ToString().Trim('~'),
                    BoatSeaterId = ViewState["BoatSeaterIds"].ToString().Trim('~'),
                    BookingDuration = ViewState["BoatMinTimes"].ToString().Trim('~'),

                    InitBoatCharge = ViewState["InitBoatCharges"].ToString().Trim('~'),
                    InitRowerCharge = ViewState["RowerMinCharge"].ToString().Trim('~'),
                    InitNetAmount = ViewState["InitNetAmounts"].ToString().Trim('~'),

                    CreatedBy = Session["UserId"].ToString().Trim('~'),

                    BookingMedia = "DW"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CreditBoatBookingService", BoatBook).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        string[] sResult = ResponseMsg.Split('~');

                        if (ViewState["PINType"].ToString().Trim() == "D")
                        {
                            Response.Redirect("PrintCreditBoat.aspx?rt=b&bi=" + sResult[1].ToString() + "");
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
                        divBack.Style.Add("background-color", "white");
                    }
                    else
                    {
                        string BookingId = GetBookingIdByPin();

                        if (BookingId.Trim() != "")
                        {
                            Response.Redirect("PrintCreditBoat.aspx?rt=b&bi=" + BookingId.Trim() + "");
                            return;
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
                    }
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
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
        try
        {
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

                var BoatSearch = new Ticket()
                {
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CreditBookedDetails", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
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

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            dvContent.Attributes.Add("style", "display:none;");
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string BookingId = GvBoatBooking.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["BoOkingID"] = BookingId.ToString().Trim();

            if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
            {
                Mpepnlrsn.Show();
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
            btnBoatBooking.Enabled = true;

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

            ViewState["boatOfferAmount"] = "";

            bschar1.InnerText = "";
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

            btnBoatBooking.Text = "Submit";

            lblNoofPersonAlert.Text = "";
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

            DataTable dtCurrentTable = (DataTable)ViewState["CartRow"];

            for (int i = dtCurrentTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtCurrentTable.Rows[i];
                if (dr["UniqueId"].ToString().Trim() == sUniqueId.Trim())
                {
                    dr.Delete();
                }
            }

            dtCurrentTable.AcceptChanges();

            DataTable dtCurrentTable1 = (DataTable)ViewState["Row"];

            for (int i = dtCurrentTable1.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtCurrentTable1.Rows[i];
                if (dr["UniqueId"].ToString().Trim() == sUniqueId.Trim())
                {
                    dr.Delete();
                }
            }

            dtCurrentTable.AcceptChanges();
            dtCurrentTable1.AcceptChanges();

            if (dtCurrentTable.Rows.Count > 0)
            {
                gvBoatdtl.Visible = true;
                gvBoatdtl.DataSource = dtCurrentTable;
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

            ViewState["OfferAmountTotal"] = "0";

            CalculateSummary();

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
        if (CheckBoatSelectionStatus("P") == true)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sorry, Already Selected an Premium Boat Booking !');", true);
            return;
        }

        divBack.Style.Add("background-color", "white");
        ClearBooking();
        hfNatureVisible.Value = "0";
        premimumMsg.Visible = false;
        premimumMsg.InnerText = "";

        hfBoatNature.Value = "N";
        BoatBookedSummaryList();
        divBack.Style.Add("background-color", "white");
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    protected void btnPre_Click(object sender, EventArgs e)
    {
        if (CheckBoatSelectionStatus("N") == true)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sorry, Already Selected an Normal Boat Booking !');", true);
            return;
        }

        divBack.Attributes.Add("style", "background-image: repeating-linear-gradient(90deg, #bf9b30, #ffe34d, #ffcf40, #ffe34d, #e6c200, #ccac00);margin-right:-2px;margin-left:-10px;margin-bottom:5px;");
        ClearBooking();
        hfNatureVisible.Value = "0";
        premimumMsg.Visible = false;
        premimumMsg.InnerText = "";

        hfBoatNature.Value = "P";
        BoatBookedSummaryList();
        divBack.Attributes.Add("style", "background-image: repeating-linear-gradient(90deg, #bf9b30, #ffe34d, #ffcf40, #ffe34d, #e6c200, #ccac00);margin-right:-2px;margin-left:-10px;margin-bottom:5px;");
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "slideUpDownDiv();", true);
        dvContent.Attributes.Add("style", "display:none;");
    }

    private void CalculateSummary()
    {
        bschar1.InnerText = ViewState["BoatChargeSum"].ToString();
        bsgst1.InnerText = (Convert.ToDecimal(ViewState["BoatTaxSum"].ToString()) + Convert.ToDecimal(ViewState["OtherTaxSum"].ToString())).ToString();
        bsdeposit1.InnerText = ViewState["BoatDepositSum"].ToString();

        //bsTotal.InnerText = ViewState["BoatTotalSum"].ToString();

        bsTotal.InnerText = (Convert.ToDecimal(bschar1.InnerText) + Convert.ToDecimal(bsgst1.InnerText)
                       + Convert.ToDecimal(bsdeposit1.InnerText)).ToString();

        btnBoatBooking.Text = (Convert.ToDecimal(bschar1.InnerText) + Convert.ToDecimal(bsgst1.InnerText)
                                + Convert.ToDecimal(bsdeposit1.InnerText) - (Convert.ToDecimal(ViewState["OfferAmountTotal"].ToString()))).ToString();

        if (Convert.ToDecimal(ViewState["BoatTotalSum"].ToString()) >= 1)
        {
            divPin.Visible = true;

            if (txtPIN.Text == "")
            {
                btnDefaultPin.Visible = true;
            }
            if (btnDefaultPin.Visible == true)
            {
                txtPIN.ReadOnly = false;
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
            btnBoatBooking.Text = "";

            divBooking.Visible = false;
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

    protected void dtlBoat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["OfferType"] = "";
            ViewState["OfferPorF"] = "0";
            ViewState["OfferAmountTotal"] = 0;
            ViewState["OverAllSelectedPersonCount"] = 0;

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

            //if (CheckCount > NoPerson)
            //{
            //    lstValue.SelectedIndex = 0;
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Sorry, You Selected Excess Boat Seater !');", true);
            //    return;
            //}

            BindDataDynamicValue(BoatTypeName, BoatTypeId, BoatSeaterType, BoatSeaterId, BoatCount, BoatTotalCharge, BoatMinCharge,
                RowerMinCharge, BoatTaxCharge, DepositType, BoatDepositAmount, BoatMinTime, BoatSeaterCount);

            lstValue.SelectedIndex = 0;
            ClearTempValues();

            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "myColor();", true);

            Panel pnlBoatCount = (Panel)((DataList)dtlBoat).Controls[0].FindControl("pnlBoatCount");
            pnlBoatCount.Visible = false;

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

                    // Getting an Boat Selection

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

                        if (lblTax != "")
                        {
                            string[] taxlist = lblTax.Split(',');

                            decimal TaxAmt = 0;

                            foreach (var list in taxlist)
                            {
                                var TaxName = list;
                                var tx = list.Split('-');

                                TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                                BoatTotalTaxAmt = BoatTotalTaxAmt + TaxAmt;
                            }
                        }

                        iBoatMinCharge = Convert.ToDecimal(BoatMinCharge.Trim());
                        ViewState["InitBoatCharges"] += BoatMinCharge.Trim() + '~';

                        iRowerMinCharge = Convert.ToDecimal(RowerMinCharge.Trim());
                        ViewState["RowerMinCharge"] += RowerMinCharge.Trim() + '~';

                        ViewState["BoatTaxAfterDiscount"] = Convert.ToDecimal(BoatTaxCharge.Trim());

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

                var BoatSearch = new Ticket()
                {
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CreditBookedDetails", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        gvUserCountTotal.DataSource = dt;
                        gvUserCountTotal.DataBind();

                        gvUserCountTotal.FooterRow.Cells[4].Text = "Total";
                        gvUserCountTotal.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                        gvUserCountTotal.FooterRow.Cells[6].Text = dt.Compute("Sum(NetAmount)", "").ToString();
                        gvUserCountTotal.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

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

                HttpResponseMessage response = client.PostAsJsonAsync("CreditBookingDetailsCount", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        bblblCount.Text = dtExists.Rows[0]["Count"].ToString();
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
                    ServiceType = "Credit Booking",
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
                        Response.Redirect("PrintCreditBoat.aspx?rt=b&bi=" + ViewState["BoOkingID"].ToString().Trim() + "");

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
        public string FromDate { get; set; }
        public string ToDate { get; set; }

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
                    QueryType = "GetCreditBookingIdByPin",
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


    protected void BackToBooking_Click(object sender, EventArgs e)
    {
        Response.Redirect("BoatBookingFinal.aspx");
    }
}