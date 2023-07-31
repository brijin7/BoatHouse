using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_RptPrintRestaurantServices : System.Web.UI.Page
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
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");


                lblPrintedByName.Text = Session["PrintUserName"].ToString();
                lblPrintDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/");
                ViewState["hfstartvalue"] = "0";
                ViewState["hfendvalue"] = "0";
                int istart;
                int iend;
                AddProcess(0, 10, out istart, out iend);
                BindPrintRestaurant();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    protected void ImgBtnPrint_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string Status = GVRestaurant.DataKeys[gvrow.RowIndex]["Status"].ToString().Trim();
            ViewState["Status"] = Status.ToString().Trim();
            string BookingId = GVRestaurant.DataKeys[gvrow.RowIndex].Value.ToString();

            ViewState["PrBookingId"] = BookingId.ToString();
            GvBindRestaurant(BookingId);
            GetOtherSummaryReceipt(BookingId);
            OtherTicketInstructions();
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
            MpeBillService.Show();

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
                            dtlistRestTicketOther.DataSource = dt;
                            dtlistRestTicketOther.DataBind();
                            dtlistRestTicketOther.Visible = true;
                        }
                        else
                        {
                            dtlistRestTicketOther.Visible = false;
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

    protected void GVRestaurant_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVRestaurant.PageIndex = e.NewPageIndex;
        BindPrintRestaurant();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        txtSearch.Text = string.Empty;
        BackToList.Visible = false;
        back.Enabled = false;
        back.Visible = true;
        Next.Visible = true;
        backSearch.Visible = false;
        NextSearch.Visible = false;
        BindPrintRestaurant();
        
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtSearch.Text = string.Empty;
        BackToList.Visible = false;
        Next.Visible = true;
        back.Visible = true;
       
        backSearch.Visible = false;
        NextSearch.Visible = false;
        GVRestaurant.PageIndex = 0;
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        BindPrintRestaurant();
    }

    public void BindPrintRestaurant()
    {
        try
        {
            Search.Visible = true;
            divGridList.Visible = true;

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
                    GVRestaurant.Columns[6].Visible = true;

                    var OtherBook = new RestaurantBooking()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = "Admin",
                        CountStart = ViewState["hfstartvalue"].ToString()
                    };
                    response = client.PostAsJsonAsync("RestaurantBookedListV2", OtherBook).Result;
                }
                else
                {
                    var OtherBook = new RestaurantBooking()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = ViewState["hfstartvalue"].ToString()
                    };
                    response = client.PostAsJsonAsync("RestaurantBookedListV2", OtherBook).Result;
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
                            if (dt.Rows.Count < 10)
                            {
                                Next.Enabled = false;
                                NextSearch.Enabled = false;

                            }
                            else
                            {
                                Next.Enabled = true;
                                NextSearch.Enabled = true;
                                backSearch.Enabled = false;

                            }
                            GVRestaurant.DataSource = dt;
                            GVRestaurant.DataBind();
                            GVRestaurant.Visible = true;
                            Search.Visible = true;
                            GVRestaurant.FooterRow.Cells[2].Text = "Total";
                            GVRestaurant.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            decimal ServiceFare = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ServiceFare")));
                            decimal TaxAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));
                            decimal NetAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));


                            GVRestaurant.FooterRow.Cells[3].Text = ServiceFare.ToString("N2");
                            GVRestaurant.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                            GVRestaurant.FooterRow.Cells[4].Text = TaxAmount.ToString("N2");
                            GVRestaurant.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;


                            GVRestaurant.FooterRow.Cells[5].Text = NetAmount.ToString("N2");
                            GVRestaurant.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                            //Newly added by Brijin and Imran on 2022-05-24
                            if (UserRole == "Admin")
                            {
                                GVRestaurant.Columns[6].Visible = true;
                            }
                            else
                            {
                                GVRestaurant.Columns[6].Visible = false;
                            }
                        }
                        else
                        {
                            GVRestaurant.Visible = false;
                            Next.Enabled = false;
                            Search.Visible = false;
                        }
                    }
                    else
                    {
                        if (ResponseMsg == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                       
                        GVRestaurant.Visible = false;
                        Search.Visible = false;
                        Next.Enabled = false;
                    }
                }
                else
                {
                  
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                    GVRestaurant.Visible = false;
                    Next.Enabled = false;
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

    private void OtherTicketInstructions()
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

    protected void dtlistRestTicketOther_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label BookingIds = (Label)e.Item.FindControl("lblBillOBookingId");
                Label ServiceIds = (Label)e.Item.FindControl("lblBillOServiceId");
                DataRowView dr = (DataRowView)e.Item.DataItem;
                Image ImageData = (Image)e.Item.FindControl("imgOtherQR");
                //Image ImageData1 = (Image)e.Item.FindControl("imgOtherQR1");

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
                            //ImageData1.ImageUrl = ResponseMsg;
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

    protected void imgCloseTicket_Click(object sender, ImageClickEventArgs e)
    {
        MpeBillService.Hide();

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
        {
            Mpepnlrsn.Show();
        }
        else
        {

            if (ViewState["Status"].ToString() == "Y")
            {
                ViewState["Status"] = null;
                Mpepnlrsn.Show();
            }
            else
            {
                ViewState["Status"] = null;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Time Out, Cannot Print Details');", true);
                return;
            }
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
                var BoatHouseId = new RestaurantBooking()
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
                var EmpMstr = new RestaurantBooking()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    UserId = Session["UserId"].ToString().Trim(),
                    UserName = ViewState["EmpName"].ToString().Trim(),
                    ServiceType = "Restaurant Services",
                    BookingId = ViewState["PrBookingId"].ToString().Trim(),
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
                        Response.Redirect("~/Boating/Print.aspx?rt=rr&bi=" + ViewState["PrBookingId"].ToString() + "");
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

    public class QR
    {
        public string ServiceId { get; set; }
        public string BookingId { get; set; }
        public string BookingType { get; set; }
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
        public string UserId { get; set; }
        public string Reason { get; set; }
        public string CreatedDate { get; set; }
        public string BoatHouseName { get; set; }
        public string CreatedBy { get; set; }
        public string UserName { get; set; }
        public string ServiceType { get; set; }
        public string CountStart { get; set; }
        public string Input1 { get; set; }

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

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString().Trim()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        Search.Visible = true;
        BindPrintRestaurant();
    }

    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BindPrintRestaurant();
    }

    protected void BackToList_Click(object sender, EventArgs e)
    {
        back.Visible = true;
        back.Enabled = false;

        Next.Visible = true;
        BackToList.Visible = false;
        Search.Visible = true;
        txtSearch.Text = string.Empty;
        backSearch.Visible = false;
        NextSearch.Visible = false;
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        BindPrintRestaurant();
        //back.Visible = true;
        //Next.Visible = true;
        //BackToList.Visible = false;
        //Search.Visible = true;
        //txtSearch.Text = string.Empty;
        //BindPrintRestaurant();
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

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        back.Visible = false;
        Next.Visible = false;
        BackToList.Visible = true;
        ViewState["hfSearchstartvalue"] = "0";
        ViewState["hfSearchendvalue"] = "0";
        int istart;
        int iend;
        backSearch.Enabled = false;
        AddProcessSearch(0, 10, out istart, out iend);
        BindPrintRestaurantPin();
        //back.Visible = false;
        //Next.Visible = false;
        //BackToList.Visible = true;
        //BindPrintRestaurantPin();
    }

    public void BindPrintRestaurantPin()
    {
        try
        {
            backSearch.Visible = true;
            NextSearch.Visible = true;
            Search.Visible = true;
            divGridList.Visible = true;

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
                    GVRestaurant.Columns[6].Visible = true;

                    var OtherBook = new RestaurantBooking()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = "Admin",
                        Input1 = txtSearch.Text.Trim(),
                        CountStart = ViewState["hfSearchstartvalue"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("RestaurantBookedListPinV2", OtherBook).Result;
                }
                else
                {
                    var OtherBook = new RestaurantBooking()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        Input1 = txtSearch.Text.Trim(),
                        CountStart = ViewState["hfSearchstartvalue"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("RestaurantBookedListPinV2", OtherBook).Result;
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
                            if (dt.Rows.Count < 10)
                            {
                                NextSearch.Enabled = false;

                            }
                            else
                            {
                                NextSearch.Enabled = true;
                                back.Enabled = false;
                            }
                            GVRestaurant.DataSource = dt;
                            GVRestaurant.DataBind();
                            GVRestaurant.Visible = true;
                            Search.Visible = true;
                            backSearch.Visible = true;
                            GVRestaurant.FooterRow.Cells[2].Text = "Total";
                            GVRestaurant.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            decimal ServiceFare = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ServiceFare")));
                            decimal TaxAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));
                            decimal NetAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));


                            GVRestaurant.FooterRow.Cells[3].Text = ServiceFare.ToString("N2");
                            GVRestaurant.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                            GVRestaurant.FooterRow.Cells[4].Text = TaxAmount.ToString("N2");
                            GVRestaurant.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;


                            GVRestaurant.FooterRow.Cells[5].Text = NetAmount.ToString("N2");
                            GVRestaurant.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            GVRestaurant.Visible = false;
                            Search.Visible = false;
                            backSearch.Visible = false;
                            NextSearch.Visible = false;
                        }
                    }
                    else
                    {
                        if(ResponseMsg == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                            backSearch.Visible = false;
                            NextSearch.Visible = false;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                       

                        GVRestaurant.Visible = false;
                        Search.Visible = false;
                    }
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                    GVRestaurant.Visible = false;
                    Search.Visible = false;


                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void backSearch_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcessSearch(Int32.Parse(ViewState["hfSearchstartvalue"].ToString().Trim()) - 10, Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) - 10, out istart, out iend);
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();

        BindPrintRestaurantPin();
        if (istart == 1)
        {
            backSearch.Enabled = false;
        }
        else
        {
            backSearch.Enabled = true;
        }
    }

    protected void NextSearch_Click(object sender, EventArgs e)
    {
        backSearch.Enabled = true;
        int istart;
        int iend;
        AddProcessSearch(Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) + 1, Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) + 10, out istart, out iend);
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
        BindPrintRestaurantPin();
    }
    protected void AddProcessSearch(int start, int end, out int istart, out int iend)
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
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
    }


    protected void subProcessSearch(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
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
        }
        else
        {
            iend = end;

        }
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
    }
}