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
using System.Globalization;
using System.Web.Helpers;

public partial class Boating_BoatBookingFinal : System.Web.UI.Page
{
    public decimal lblChargePerItem, lblNoOfItems, lblItemCharge, lblTaxAmount, lblNetAmount = 0;
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
                //if (Session["BMBooking"].ToString().Trim() == "Y")
                //{
                //    GetPaymentType();
                //    BindOtherCategoryList();
                //}

                if (Session["BMOtherService"].ToString().Trim() == "Y")
                {
                    GetPaymentType();
                    BindOtherCategoryList();

                    BindOtherCountAmount();

                    if (Session["BBMBulkOtherService"].ToString().Trim() == "Y")
                    {
                        BulkOther.Visible = true;
                    }
                    else
                    {
                        BulkOther.Visible = false;
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

                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
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
        ViewState["sOthGSTAmount"] = "";

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
                string SOthAmountArray;
                string[] OthAmountArray;

                SOthAmountArray = ViewState["sOthGSTAmount"].ToString();
                OthAmountArray = SOthAmountArray.Split('#');

                ViewState["CGSTOthTaxAmount"] += OthAmountArray[0].ToString() + '~';
                ViewState["SGSTOthTaxAmount"] += OthAmountArray[1].ToString() + '~';

                ViewState["OthTaxDetails"] += TaxDtl + '~';
                decimal OtherTotalAmount = Totalcharge + TotalTaxAmt;
                ViewState["OthNetAmount"] += OtherTotalAmount.ToString() + '~';
                ViewState["sOthGSTAmount"] = "";
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
            OtherBookingFinal();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                string sMSG = string.Empty;


                var Bookingotherservices = new Bookingotherservices()
                {
                    QueryType = "Insert",
                    BookingId = "0",
                    ServiceId = ViewState["OthServiceId"].ToString().Trim('~'),
                    BookingType = "I",
                    BoatHouseId = Session["BoatHouseId"].ToString(),

                    BoatHouseName = Session["BoatHouseName"].ToString(),
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"),

                    ChargePerItem = ViewState["OthChargePerItem"].ToString().Trim('~'),
                    NoOfItems = ViewState["OthNoOfItems"].ToString().Trim('~'),
                    CGSTTaxAmount = ViewState["CGSTOthTaxAmount"].ToString().Trim('~'),
                    SGSTTaxAmount = ViewState["SGSTOthTaxAmount"].ToString().Trim('~'),
                    // TaxDetails = ViewState["OthTaxDetails"].ToString().Trim('~'),
                    NetAmount = ViewState["OthNetAmount"].ToString().Trim('~'),

                    CustomerMobileNo = "",
                    Createdby = Session["UserId"].ToString(),
                    BookingMedia = "DW",
                    PaymentType = ddlPaymentType.SelectedValue.Trim()
                };
                response = client.PostAsJsonAsync("BookingOtherServices", Bookingotherservices).Result;

                sMSG = "Booking other services Details Inserted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        string[] sResult = ResponseMsg.Split('~');
                        //GetOtherTickets(sResult[1].ToString());
                        //GetOtherSummaryReceipt(sResult[1].ToString());
                        //OtherTicketInstructions(sResult[1].ToString());
                        //ClearBooking();
                        Response.Redirect("Print.aspx?rt=o&bi=" + sResult[1].ToString() + "");
                    }
                    else
                    {
                        ClearBooking();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    ClearBooking();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void imgbtnNewBook_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divShow.Visible = false;
            imgbtnNewBook.Visible = false;
            imgbtnBookedList.Visible = true;

            divGridList.Visible = false;
            ClearBooking();
            BindOtherCategoryList();
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

    protected void dtlistTicketOther_ItemDataBound(object sender, DataListItemEventArgs e)
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

    //protected void dtlistTicketOther_ItemDataBound(object sender, DataListItemEventArgs e)
    //{

    //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //    {
    //        Label BookingIds = (Label)e.Item.FindControl("lblBillOBookingId");
    //        Label ServiceIds = (Label)e.Item.FindControl("lblBillOServiceId");
    //        DataRowView dr = (DataRowView)e.Item.DataItem;
    //        Image ImageData = (Image)e.Item.FindControl("imgOtherQR");
    //        Image ImageData1 = (Image)e.Item.FindControl("imgOtherQR1");

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            var QRd = new QR()
    //            {
    //                ServiceId = ServiceIds.Text.Trim(),
    //                BookingId = BookingIds.Text.Trim(),
    //                BookingType = "I"
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("BookingOthersQR", QRd).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

    //                if (Status == "Success.")
    //                {
    //                    ImageData.Visible = true;
    //                    //ImageData.ImageUrl = @"ImgPost/" + dr.Row["ImgPath"].ToString();
    //                    ImageData.ImageUrl = ResponseMsg;
    //                    ImageData1.ImageUrl = ResponseMsg;
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


    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        Label lblBookingId = (Label)e.Item.FindControl("lblBookingId");

    //        var BoatServiceId = e.Item.FindControl("dtlisTicketInsOther") as DataList;

    //        try
    //        {
    //            Control trBoatIns = e.Item.FindControl("trInsOther") as Control;
    //            trBoatIns.Visible = false;

    //            using (var client = new HttpClient())
    //            {
    //                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //                client.DefaultRequestHeaders.Clear();
    //                client.DefaultRequestHeaders.Accept.Clear();

    //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //                string boatTypeIds = string.Empty;

    //                var service = new OtherBook()
    //                {
    //                    ServiceType = "2",// Default 1 is Boat boking//
    //                    BoatHouseId = Session["BoatHouseId"].ToString()
    //                };

    //                HttpResponseMessage response = client.PostAsJsonAsync("PrintInstrucSvc", service).Result;

    //                if (response.IsSuccessStatusCode)
    //                {
    //                    var BoatLst = response.Content.ReadAsStringAsync().Result;
    //                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
    //                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

    //                    if (StatusCode == 1)
    //                    {
    //                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

    //                        if (dt.Rows.Count > 0)
    //                        {
    //                            BoatServiceId.DataSource = dt;
    //                            BoatServiceId.DataBind();

    //                            trBoatIns.Visible = true;
    //                        }
    //                        else
    //                        {
    //                            BoatServiceId.DataBind();
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        }
    //    }
    //}

    protected void imgbtnBookedList_Click(object sender, ImageClickEventArgs e)
    {
        BackToList.Visible = false;
        txtSearch.Text = string.Empty;
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        BookedList();
    }
    public void BookedList()
    {
        ViewState["Flag"] = "T";

        divSearch.Visible = true;
        try
        {
            ClearBooking();

            divShow.Visible = true;
            imgbtnNewBook.Visible = true;
            imgbtnBookedList.Visible = false;

            divGridList.Visible = true;
            divEntry.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;

                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    GvBoatBooking.Columns[9].Visible = true;

                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "Admin",
                        CountStart = ViewState["hfstartvalue"].ToString()
                    };
                    response = client.PostAsJsonAsync("OtherBookedListV2", OtherBook).Result;
                }
                else
                {
                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = ViewState["hfstartvalue"].ToString()
                    };
                    response = client.PostAsJsonAsync("OtherBookedListV2", OtherBook).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

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
                            lblGridMsg.Text = "No Record Found...!";
                            GvBoatBooking.Visible = false;
                            Next.Enabled = false;
                        }
                    }
                    else
                    {

                        if (ResponseMsg.Trim() == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                        GvBoatBooking.Visible = false;
                        Next.Enabled = false;

                        divSearch.Visible = false;

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
            //GetOtherTickets(BookingId);
            //GetOtherSummaryReceipt(BookingId);
            //OtherTicketInstructions(BookingId);


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    private void GetOtherTickets(string sBookingId)
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
                    //var ticktList = response.Content.ReadAsStringAsync().Result;
                    //int StatusCode = Convert.ToInt32(JObject.Parse(ticktList)["StatusCode"].ToString());
                    //string ResponseMsg = JObject.Parse(ticktList)["Response"].ToString();               


                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        DlOtherReceipt.DataSource = dtExists;
                        DlOtherReceipt.DataBind();

                        lblBookingId.Text = dtExists.Rows[0]["BookingId"].ToString();
                        lblBookingDate.Text = dtExists.Rows[0]["BookingDate"].ToString();
                        lblBoatHouseName.Text = dtExists.Rows[0]["BoatHouseName"].ToString();
                        lblPaymentTypeName.Text = dtExists.Rows[0]["PaymentTypeName"].ToString();
                        lblCustomerMobileNo.Text = dtExists.Rows[0]["CustomerMobileNo"].ToString();
                        lblGST.Text = dtExists.Rows[0]["GSTNumber"].ToString();

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
            lblChargePerItem += Convert.ToDecimal(((Label)e.Item.FindControl("lblChargePerItem")).Text);
            lblNoOfItems += Convert.ToDecimal(((Label)e.Item.FindControl("lblNoOfItems")).Text);
            lblItemCharge += Convert.ToDecimal(((Label)e.Item.FindControl("lblItemCharge")).Text);
            lblTaxAmount += Convert.ToDecimal(((Label)e.Item.FindControl("lblTaxAmount")).Text);
            lblNetAmount += Convert.ToDecimal(((Label)e.Item.FindControl("lblNetAmount")).Text);
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            Label lblTotalChargePerItem = (Label)e.Item.FindControl("lblTotalChargePerItem");
            lblTotalChargePerItem.Text = lblChargePerItem.ToString();

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

            oschar1.InnerText = "";
            bsgst1.InnerText = "";

            ViewState["CartRowO"] = null;
            ViewState["RowO"] = null;

            gvOther.DataBind();
            dtlOther.DataBind();

            divpaymentType.Visible = false;
            btnOtherBooking.Text = "";

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
                divpaymentType.Visible = true;

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

                divpaymentType.Visible = false;
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

            divpaymentType.Visible = true;
        }
        else
        {
            divpaymentType.Visible = false;
            gvOther.Visible = false;
            gvOther.DataBind();
        }
    }
    protected void ChkBulk_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkBulk.Checked == true)
        {
            Response.Redirect("BulkOtherServices.aspx", true);

        }
        else
        {
            Response.Redirect("BulkOtherServices.aspx", false);

        }
    }
    public class Otherservices
    {
        public string BoatHouseId { get; set; }
        public string Category { get; set; }
    }

    public class OtherBook
    {
        public string Search { get; set; }
        public string BoatHouseId { get; set; }
        public string BookingType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceType { get; set; }
        public string UserId { get; set; }
        public string CountStart { get; set; }
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
        public string CGSTTaxAmount { get; set; }
        public string SGSTTaxAmount { get; set; }
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
        public string UserName { get; set; }
        public string Reason { get; set; }
        public string CreatedDate { get; set; }

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

    protected void gvUserCountTotal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUserCountTotal.PageIndex = e.NewPageIndex;
        BindUserCountTotal();
    }

    protected void ImgClose_Click(object sender, ImageClickEventArgs e)
    {
        MpeUserCount.Hide();
    }
    public void BindUserCountTotal()
    {
        try
        {


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;

                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "Admin"
                    };
                    response = client.PostAsJsonAsync("OtherBookedDetailsPopup", OtherBook).Result;
                }
                else
                {
                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("OtherBookedDetailsPopup", OtherBook).Result;
                }

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
                            gvUserCountTotal.DataSource = dt;
                            gvUserCountTotal.DataBind();

                            decimal TotalService = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ServiceFare")));
                            decimal TotalTax = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));
                            decimal NetAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));

                            gvUserCountTotal.FooterRow.Cells[3].Text = "Total";
                            gvUserCountTotal.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                            gvUserCountTotal.FooterRow.Cells[6].Text = TotalService.ToString();
                            gvUserCountTotal.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                            gvUserCountTotal.FooterRow.Cells[7].Text = TotalTax.ToString();
                            gvUserCountTotal.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                            gvUserCountTotal.FooterRow.Cells[8].Text = NetAmount.ToString("N2");
                            gvUserCountTotal.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;

                            gvUserCountTotal.Visible = true;
                            MpeUserCount.Show();


                        }
                        else
                        {
                            gvUserCountTotal.DataBind();
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
    public void BindOtherCountAmount()
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
                string sUserRole = Session["UserRole"].ToString();

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

                HttpResponseMessage response = client.PostAsJsonAsync("OtherServiceDetailsCount", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        bblblCount.Text = dtExists.Rows[0]["BookingCount"].ToString();

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

    protected void bblblCount_Click(object sender, EventArgs e)
    {
        BindUserCountTotal();
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
                    ServiceType = "Other Services",
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
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);

                        Mpepnlrsn.Hide();
                        Response.Redirect("Print.aspx?rt=o&bi=" + ViewState["BoOkingID"].ToString().Trim() + "");
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

    public void BindOtherServiceRevenuePopup()
    {
        try
        {
            if (bblblNetAmount.Text != "0")
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
                        sQueryType = "OtherServiceRevenueBooking";
                    }
                    else
                    {
                        sCreatedBy = Session["UserId"].ToString().Trim();
                        sQueryType = "OtherServiceRevenueUser";
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

                            decimal totAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("OtherServiceRevenue")));

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

    protected void bblblNetAmount_Click(object sender, EventArgs e)
    {
        BindOtherServiceRevenuePopup();
    }

    protected void ImgCloseBB_Click(object sender, ImageClickEventArgs e)
    {
        MPEBBpopup.Hide();
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
            BookedListSinglePin();
        }
        else
        {
            BookedList();
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
            BookedListSinglePin();
        }
        else
        {
            BookedList();
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
        BookedList();

    }
    public void BookedListSinglePin()
    {
        ViewState["Flag"] = "S";
        try
        {
            ClearBooking();


            divSearch.Visible = true;
            divShow.Visible = true;
            imgbtnNewBook.Visible = true;
            imgbtnBookedList.Visible = false;

            divGridList.Visible = true;
            divEntry.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;

                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    GvBoatBooking.Columns[9].Visible = true;

                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "Admin",
                        CountStart = ViewState["hfstartvalue"].ToString(),
                        Search = txtSearch.Text
                    };
                    response = client.PostAsJsonAsync("OtherBookedListPinV2", OtherBook).Result;
                }
                else
                {
                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = ViewState["hfstartvalue"].ToString(),
                        Search = txtSearch.Text
                    };
                    response = client.PostAsJsonAsync("OtherBookedListPinV2", OtherBook).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

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
                            lblGridMsg.Text = "No Record Found...!";
                            GvBoatBooking.Visible = false;

                        }
                    }
                    else
                    {

                        if (ResponseMsg.Trim() == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                        GvBoatBooking.Visible = false;

                        divSearch.Visible = false;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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
        BookedListSinglePin();
        BackToList.Visible = true;
    }
}