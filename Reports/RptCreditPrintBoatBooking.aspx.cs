using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Helpers;

public partial class Reports_RptCreditPrintBoatBooking : System.Web.UI.Page
{
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
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    txtFromDate.Attributes.Add("readonly", "readonly");
                    txtToDate.Attributes.Add("readonly", "readonly");

                    BindBoatTicketDetails();

                    divShow.Visible = true;
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GvBoatBooking.PageIndex = 0;
        BindBoatTicketDetails();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        GvBoatBooking.PageIndex = 0;
        BindBoatTicketDetails();
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            dtlistTicket.DataSource = "";
            dtlistTicket.DataBind();

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
           
            string BookingId = GvBoatBooking.DataKeys[gvrow.RowIndex].Value.ToString();

            ViewState["PrBookingId"] = BookingId.ToString();

            GetBoatTickets(BookingId);
            GetBoatTicketsSummaryReceipt(BookingId); 
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
                dtlistTicket.Visible = true;

                Label BookingId = (Label)e.Item.FindControl("lblBookingId");
                Label RowerCharge = (Label)e.Item.FindControl("lblActualRowerCharge");
                Label lblBookingPin = (Label)e.Item.FindControl("lblBookingPin");
                Label lblUniqueId = (Label)e.Item.FindControl("lblUniqueId");

                Control divRower = e.Item.FindControl("divrower") as Control;

                if (Convert.ToDecimal(RowerCharge.Text) > 0)
                {
                    divRower.Visible = true;
                }
                else
                {
                    divRower.Visible = false;
                }

                // Print QRCode Details

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
                        BookingRef = lblUniqueId.Text.Trim()
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

                        var service = new BoatSearch()
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

    protected void GvBoatBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBoatBooking.PageIndex = e.NewPageIndex;
        BindBoatTicketDetails();
    }

    public void BindBoatTicketDetails()
    {
        try
        {
            divGridList.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
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
                        divGridList.Visible = false;
                        GvBoatBooking.Visible = false;
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

                HttpResponseMessage response = client.PostAsJsonAsync("CreditBoatBookedTicket", BoatSearch).Result;

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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
                    QueryType = "CreditBoatPrintTicketBulkReceipts",
                    ServiceType = "",
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BookingId = sBookingId.ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    UserId = ""
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
     
    public class BoatSearch
    {
        public string BoatHouseId { get; set; }
        public string BookingId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceType { get; set; }
        public string UserId { get; set; }

        public string BoatHouseName { get; set; }
        public string UserName { get; set; }
        public string Reason { get; set; }
        public string CreatedBy { get; set; }
    }

    public class QRBoat
    {
        public string BoatHouseId { get; set; }
        public string BookingId { get; set; }
        public string Pin { get; set; }
        public string BookingRef { get; set; }
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

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
        {
            Mpepnlrsn.Show();
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
                        Response.Redirect("~/Boating/PrintCreditBoat.aspx?rt=rb&bi=" + ViewState["PrBookingId"].ToString() + "");
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
}