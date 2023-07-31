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
using System.Web.Helpers;

public partial class Boating_KioskOtherService : System.Web.UI.Page
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
                if (Session["BMBooking"].ToString().Trim() == "Y")
                {
                    GetPaymentType();
                    BindOtherCategoryList();
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
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
                        dtlOther.Visible = true;
                    }
                    else
                    {
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

    private void OtherBookingFinal()
    {
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
                        TaxAmount += (TaxAmt + "#").Trim();
                        TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                        TotalTaxAmt = TotalTaxAmt + TaxAmt;
                    }
                }
                ViewState["sOthGSTAmount"] = TaxAmount.ToString();
                string SOthAmountArray = string.Empty;
                string[] OthAmountArray;

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
    }

    protected void btnOtherBooking_Click(object sender, EventArgs e)
    {
        try
        {
            string ServiceType = "";
            string UserId = "";

            if (bsTotal.InnerText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Service !');", true);
                return;
            }

            if (Convert.ToDecimal(btnOtherBooking.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Service !');", true);
                return;
            }

            OtherBookingFinal();

            if (ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "ONLINE" || ddlPaymentType.SelectedItem.Text.Trim().ToUpper() == "UPI")
            {
                if (txtMobileNo.Text.Trim() != null)
                {
                    if (ViewState["UserId"].ToString() == "N" || ViewState["UserId"].ToString() == "" || ViewState["UserId"].ToString().Trim() == null)
                    {
                        SignUp();
                    }

                    if (ViewState["UserId"].ToString().Trim() != "" || ViewState["UserId"].ToString() != "N"
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

                        OnlineBeforeTransactionDetails(ServiceType, UserId, txtMobileNo.Text.Trim());

                        if (ViewState["TranStatus"].ToString().Trim() == "Y")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Payment SMS Link Send to Customer Mobile !');", true);

                            ClearBooking();
                            ScriptManager.RegisterStartupScript(this, GetType(), "SendOtp", "SendOtp();", true);
                            btnResend.Visible = true;
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

    private void ClearBooking()
    {
        try
        {
            ddlPaymentType.SelectedIndex = 0;

            ViewState["OthServiceId"] = "";
            ViewState["OthChargePerItem"] = "";
            ViewState["OthNoOfItems"] = "";
            ViewState["OthNetAmount"] = "";
            ViewState["OthTaxDetails"] = "";
            ViewState["CGSTOthTaxAmount"] = "";
            ViewState["SGSTOthTaxAmount"] = "";

            oschar1.InnerText = "";
            bsgst1.InnerText = "";
            bsTotal.InnerText = "";

            ViewState["CartRowO"] = null;
            ViewState["RowO"] = null;

            gvOther.DataBind();
            dtlOther.DataBind();

            divBooking.Visible = false;
            btnOtherBooking.Text = "";

            txtMobileNo.Text = "";

            lblName.Text = "";
            lblEmailId.Text = "";
            trMobileNo.Visible = false;
            txtMobileNo.Enabled = true;
            divOtpbtn.Visible = false;
            divOtptxt.Visible = false;
            imgPinStatus.ImageUrl = "";

            BindOtherCategoryList();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearBooking();
            BindOtherCategoryList();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }


    // ***** Other Services ***** //

    protected void ImgBtnDeleteOther_Click(object sender, EventArgs e)
    {
        try
        {
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

                ViewState["OtherChargeSum"] = "0";
                ViewState["OtherTaxSum"] = "0";

                decimal dServiceTotalAmount = dtTableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItem")));
                ViewState["OtherChargeSum"] = (Convert.ToDecimal(dServiceTotalAmount)).ToString();

                decimal dChargePerItemTax = dtTableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItemTax")));

                ViewState["OtherTaxSum"] = (Convert.ToDecimal(dChargePerItemTax)).ToString();

            }
            else
            {
                ViewState["OtherChargeSum"] = "0";
                ViewState["OtherTaxSum"] = "0";

                divBooking.Visible = false;
                ViewState["CartRowO"] = null;
                ViewState["RowO"] = null;
                gvOther.Visible = false;
            }

            CalculateSummary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    private void CalculateSummary()
    {
        oschar1.InnerText = ViewState["OtherChargeSum"].ToString();
        bsgst1.InnerText = ViewState["OtherTaxSum"].ToString();
        btnOtherBooking.Text = (Convert.ToDecimal(oschar1.InnerText) + Convert.ToDecimal(bsgst1.InnerText)).ToString();
        bsTotal.InnerText = btnOtherBooking.Text.ToString();

        if (Convert.ToDecimal(ViewState["OtherChargeSum"].ToString()) >= 1)
        {
            trMobileNo.Visible = true;
        }
        else
        {
            txtMobileNo.Text = "";
            trMobileNo.Visible = false;
            txtMobileNo.Enabled = true;
            oschar1.InnerText = "";
            bsgst1.InnerText = "";
            bsTotal.InnerText = "";

            divBooking.Visible = false;
            lblEmailId.Text = "";
            lblName.Text = "";
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

                        var service = new Otherservices()
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
        btnResend.Visible = false;
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
            divBooking.Visible = false;
            gvOther.Visible = false;
            gvOther.DataBind();
        }

    }

    // ***** Check User Details & Creation

    protected void txtMobileNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtMobileNo.Text.Length == txtMobileNo.MaxLength)
            {
                divBooking.Visible = true;
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
                    Input2 = "I",
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

    public void OnlineBeforeTransactionDetails(string ServiceType, string CustUserId, string MobileNo)
    {
        try
        {
            string OthServiceId = string.Empty;
            string OthChargePerItem = string.Empty;
            string OthNoOfItems = string.Empty;
            string CGSTOthTaxAmount = string.Empty;
            string SGSTOthTaxAmount = string.Empty;
            string OthNetAmount = string.Empty;



            string[] sOthServiceStatus = new string[] { };
            string[] sOthServiceId = new string[1];
            string[] sOthChargePerItem = new string[1];
            string[] sOthNoOfItems = new string[1];
            string[] sOthNetAmount = new string[1];
            string[] sCGSTOthTaxAmount = new string[1];
            string[] sSGSTOthTaxAmount = new string[1];

            string[] sBoatPrimumStatus = new string[] { };
            string[] sInitNetAmount = new string[] { };
            string[] sCGSTTaxAmount = new string[] { };
            string[] sSGSTTaxAmount = new string[] { };

            string[] sInitOfferAmount = new string[] { };
            string[] sBoatTypeId = new string[] { };
            string[] sBoatSeaterId = new string[] { };
            string[] sBookingDuration = new string[] { };
            string[] sInitBoatCharge = new string[] { };
            string[] sInitRowerCharge = new string[] { };
            string[] sBoatDeposit = new string[] { };
            string[] sBookingBlockId = new string[] { };
            string[] sBookingTimeSlotId = new string[] { };

            sOthServiceStatus = new string[] { "Y" };

            OthServiceId = ViewState["OthServiceId"].ToString();
            sOthServiceId[0] = OthServiceId;
            // sOthServiceId += sOthServiceId + '~';

            OthChargePerItem = ViewState["OthChargePerItem"].ToString();
            sOthChargePerItem[0] = OthChargePerItem;

            OthNoOfItems = ViewState["OthNoOfItems"].ToString();
            sOthNoOfItems[0] = OthNoOfItems;

            CGSTOthTaxAmount = ViewState["CGSTOthTaxAmount"].ToString();
            sCGSTOthTaxAmount[0] = CGSTOthTaxAmount;

            SGSTOthTaxAmount = ViewState["SGSTOthTaxAmount"].ToString();
            sSGSTOthTaxAmount[0] = SGSTOthTaxAmount;

            OthNetAmount = ViewState["OthNetAmount"].ToString();
            sOthNetAmount[0] = OthNetAmount;


            ViewState["TranStatus"] = "N";

            if (bsTotal.InnerText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Services !');", true);
                return;
            }

            if (Convert.ToDecimal(btnOtherBooking.Text.Trim()) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Services !');", true);
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
                    BookingPin = "0",

                    UserId = CustUserId.Trim(),
                    MobileNo = MobileNo.Trim(),
                    EmailId = lblEmailId.Text.Trim(),

                    PaymentMode = "Online",
                    Amount = bsTotal.InnerText.Trim(),
                    BookingType = "I",

                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    PremiumStatus = "",
                    BoatPremiumStatus = sBoatPrimumStatus,

                    NoOfPass = "0",
                    NoOfChild = "0",
                    NoOfInfant = "0",

                    BoatTypeId = sBoatTypeId,
                    BoatSeaterId = sBoatSeaterId,
                    BookingDuration = sBookingDuration,

                    InitBoatCharge = sInitBoatCharge,
                    InitRowerCharge = sInitRowerCharge,
                    BoatDeposit = sBoatDeposit,

                    InitOfferAmount = sInitOfferAmount,
                    InitNetAmount = sInitNetAmount,
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
                    BookingBlockId = sBookingBlockId,
                    BookingTimeSlotId = sBookingTimeSlotId
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

                        ViewState["TranStatus"] = "Y";

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

                    var OtpSendSms = new otpsendsms()
                    {
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

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public class Otherservices
    {
        public string BoatHouseId { get; set; }
        public string Category { get; set; }
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
        public string CustomerMobileNo { get; set; }
        public string Createdby { get; set; }
        public string Category { get; set; }
        public string BookingMedia { get; set; }
        public string PaymentType { get; set; }
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
        public string ServiceType { get; set; }
        public string UserId { get; set; }
    }


    public class QRBoat
    {
        public int BookingId { get; set; }
        public int BoatTypeId { get; set; }
        public int BoatSeaterid { get; set; }
        public int BookingSerial { get; set; }
        public int CustomerID { get; set; }
    }

    public class QR
    {
        public string ServiceId { get; set; }
        public string BookingId { get; set; }
        public string BookingType { get; set; }
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

    public class otpsendsms
    {
        public string ServiceType { get; set; }
        public string MobileNo { get; set; }
        public string MediaType { get; set; }
        public string BookingId { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string Remarks { get; set; }
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
        public string[] ServiceId { get; set; }
        public string[] OthServiceId { get; set; }
        public string[] OthChargePerItem { get; set; }
        public string[] OthNoOfItems { get; set; }
        public string OthChildCharge { get; set; }
        public string OthNoOfChild { get; set; }
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

        //public string[] BookingDuration { get; set; }
        //public string[] InitBoatCharge { get; set; }
        //public string[] InitRowerCharge { get; set; }
        //public string[] BoatDeposit { get; set; }
        //public string[] TaxDetails { get; set; }
        //public string[] TaxAmount { get; set; }
        //public string[] InitOfferAmount { get; set; }
        //public string[] InitNetAmount { get; set; }
    }

    public class OtpResendSMS
    {

        public string BookingId { get; set; }
        public string MobileNo { get; set; }
        public string ReferenceNo { get; set; }

    }

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
