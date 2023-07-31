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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Microsoft.CSharp.RuntimeBinder;
using System.Web.Helpers;

public partial class Boating_RptBookingOtherSevices : System.Web.UI.Page
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
                //New
                ViewState["Flag"] = "B";
                ViewState["hfstartvalue"] = "0";
                ViewState["hfendvalue"] = "0";
                int istart;
                int iend;
                AddProcess(0, 10, out istart, out iend);
                BindOtherTicketDetails();

                lblPrintedByName.Text = Session["PrintUserName"].ToString();
                lblPrintDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/");

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }


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

    protected void GvBoatBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBoatBooking.PageIndex = e.NewPageIndex;
        BindOtherTicketDetails();
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

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string Status = GvBoatBooking.DataKeys[gvrow.RowIndex]["Status"].ToString().Trim();
            ViewState["Status"] = Status.ToString().Trim();
            string BookingId = GvBoatBooking.DataKeys[gvrow.RowIndex].Value.ToString();

            ViewState["PrBookingId"] = BookingId.ToString();
            GetOtherTickets(BookingId);
            GetOtherSummaryReceipt(BookingId);
            OtherTicketInstructions(BookingId);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        GvBoatBooking.PageIndex = 0;
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        txtSearch.Text = string.Empty;
        BackToList.Visible = false;
        Next.Visible = true;
        back.Visible = true;
        back.Enabled = false;
        BindOtherTicketDetails();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        BackToList.Visible = false;
        Next.Visible = true;
        back.Visible = true;
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        back.Enabled = false;
        GvBoatBooking.PageIndex = 0;
        BindOtherTicketDetails();
       
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

                var BoatSearch = new OtherBook()
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

    public void BindOtherTicketDetails()
    {
        ViewState["Flagee"] = "T";
        try
        {
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
                    GvBoatBooking.Columns[9].Visible = true;

                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = "Admin",
                        CountStart = ViewState["hfstartvalue"].ToString()
                    };
                    //response = client.PostAsJsonAsync("OtherBookedList", OtherBook).Result;
                    response = client.PostAsJsonAsync("OtherBookedListV2", OtherBook).Result; 
                }
                else
                {
                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = ViewState["hfstartvalue"].ToString()
                    };
                    //response = client.PostAsJsonAsync("OtherBookedList", OtherBook).Result;
                    response = client.PostAsJsonAsync("OtherBookedListV2", OtherBook).Result;
                }


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ViewState["Flag"] = "B";
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count < 10)
                        {
                            Next.Enabled = false;
                        }
                        else
                        {
                            Next.Enabled = true;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            GvBoatBooking.DataSource = dt;
                            GvBoatBooking.DataBind();
                            GvBoatBooking.Visible = true;

                            divSearch.Visible = true;
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
                            Next.Enabled = false;
                        }
                    }
                    else
                    {
                        divGridList.Visible = true;
                        GvBoatBooking.Visible = false;
                        divSearch.Visible = false;
                        Next.Enabled = false;
                        back.Enabled = false;
                        if(ViewState["Flag"].ToString() == "N")
                        {
                            back.Enabled = true;
                        }
                        divprevnext.Visible = true;
                        if (ResponseMsg.Trim() == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divGridList.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
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
                var BoatHouseId = new OtherBook()
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
                var EmpMstr = new OtherBook()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    UserId = Session["UserId"].ToString().Trim(),
                    UserName = ViewState["EmpName"].ToString().Trim(),
                    ServiceType = "Other Services",
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
                        Response.Redirect("~/Boating/Print.aspx?rt=ro&bi=" + ViewState["PrBookingId"].ToString() + "");
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
        public string Reason { get; set; }
        public string CreatedDate { get; set; }
        public string BoatHouseName { get; set; }
        public string CreatedBy { get; set; }
        public string UserName { get; set; }

        public string CountStart { get; set; }
        public string Input2 { get; set; }

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
    public class QR
    {
        public string ServiceId { get; set; }
        public string BookingId { get; set; }
        public string BookingType { get; set; }
    }


    //private void GetOtherTickets(string sBookingId)
    //{
    //    try
    //    {
    //        MpeBillService.Show();

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


    //            var OtherBook = new OtherBook()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BookingId = sBookingId.ToString()
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("OtherTicket", OtherBook).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        dtlistTicketOther.DataSource = dt;
    //                        dtlistTicketOther.DataBind();
    //                        dtlistTicketOther.Visible = true;
    //                    }
    //                    else
    //                    {
    //                        dtlistTicketOther.Visible = false;
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
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}

    //New

    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        ViewState["Flag"] = "N";
        if (ViewState["Flagee"].ToString() == "S")
        {
            BookedListSinglePin();
        }
        else
        {
            BindOtherTicketDetails();
        }
        
    }

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        if (ViewState["Flagee"].ToString() == "S")
        {
            BookedListSinglePin();
        }
        else
        {
            BindOtherTicketDetails();
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
    
        BindOtherTicketDetails();

    }

    public void BookedListSinglePin()
    {
        ViewState["Flagee"] = "S";
        try
        {
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
                    GvBoatBooking.Columns[9].Visible = true;

                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = "Admin",
                        CountStart = ViewState["hfstartvalue"].ToString(),
                        Search = txtSearch.Text
                    };
                    //response = client.PostAsJsonAsync("OtherBookedList", OtherBook).Result;
                    response = client.PostAsJsonAsync("OtherBookedListPinV2", OtherBook).Result;
                }
                else
                {
                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = ViewState["hfstartvalue"].ToString(),
                        Search = txtSearch.Text
                    };
                    //response = client.PostAsJsonAsync("OtherBookedList", OtherBook).Result;
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
                            GvBoatBooking.Visible = false;
                           
                        }
                    }
                    else
                    {
                        //divGridList.Visible = false;
                        divGridList.Visible = true;
                        GvBoatBooking.Visible = false;
                        divSearch.Visible = false;
                        Next.Enabled = false;
                        back.Enabled = false;
                        divprevnext.Visible = true;
                        if (ResponseMsg.Trim() == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divGridList.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
}