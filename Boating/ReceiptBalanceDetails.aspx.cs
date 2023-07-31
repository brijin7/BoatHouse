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

public partial class Boating_ReceiptBalanceDetails : System.Web.UI.Page
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
                txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtBookingDate.Attributes.Add("readonly", "readonly");
                RadbtnReceiptBalanceDetails.SelectedValue = "0";
                BindReceiptDtlStatusRefund();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindReceiptDtlStatusRefund()
    {
        try
        {
            divReceiptBalanceDetails.Visible = true;
            GvReceiptBalanceDetails.Visible = true;
            GvSettledList.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new ReceiptBalanceDetails()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    RStatus = "N"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ReceiptBalanceDetails", vTripSheetSettlement).Result;

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
                            GvReceiptBalanceDetails.DataSource = dt;
                            GvReceiptBalanceDetails.DataBind();
                            MpeBillService.Dispose();
                            //MpeBillService.Hide();
                            pnlBillService.Visible = false;
                        }
                        else
                        {
                            GvReceiptBalanceDetails.DataBind();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found !!!');", true);
                        GvReceiptBalanceDetails.Visible = false;
                        pnlBillService.Visible = false;
                        MpeBillService.Dispose();
                        //MpeBillService.Hide();

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

    public void BindReceiptDtlStatusSettled()
    {
        try
        {
            divReceiptBalanceDetails.Visible = true;
            GvReceiptBalanceDetails.Visible = false;
            GvSettledList.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new ReceiptBalanceDetails()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    RStatus = "Y"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ReceiptBalanceDetailsSettled", vTripSheetSettlement).Result;

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
                            GvSettledList.DataSource = dt;
                            GvSettledList.DataBind();
                        }
                        else
                        {
                            GvSettledList.DataBind();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found !!!');", true);
                        GvSettledList.Visible = false;
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

    protected void RadbtnReceiptBalanceDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        MpeBillService.Dispose();
        pnlBillService.Visible = false;
        if (RadbtnReceiptBalanceDetails.SelectedValue == "0")
        {
            txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            divAllDetails.Visible = true;
            GvSettledList.Visible = false;
            GvReceiptBalanceDetails.Visible = true;
            BindReceiptDtlStatusRefund();
        }
        else if (RadbtnReceiptBalanceDetails.SelectedValue == "1")
        {
            txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            divAllDetails.Visible = true;
            GvReceiptBalanceDetails.Visible = false;
            GvSettledList.Visible = true;
            BindReceiptDtlStatusSettled();
        }
        else
        {
            txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            divAllDetails.Visible = false;
            GvReceiptBalanceDetails.Visible = false;
            GvSettledList.Visible = false;
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

    protected void lnkBookingId_Click(object sender, EventArgs e)
    {
        pnlBillService.Visible = true;

        hfBookingid.Value = string.Empty;
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string bookingId = GvReceiptBalanceDetails.DataKeys[gvrow.RowIndex].Value.ToString();
        hfBookingid.Value = bookingId.ToString().Trim();
        Label lblBookingDate = (Label)gvrow.FindControl("lblBookingDate");
        Label lblBookingId = (Label)gvrow.FindControl("lblBookingDate");
        Label lblCollectedAmount = (Label)gvrow.FindControl("lblCollectedAmount");
        Label lblBillAmount = (Label)gvrow.FindControl("lblBillAmount");
        Label lblBalanceAmount = (Label)gvrow.FindControl("lblBalanceAmount");
        Label lblCustomerMobile = (Label)gvrow.FindControl("lblCustomerMobile");
        lblBookingDateDisp.Text = lblBookingDate.Text.Trim();
        lblBookingIdDisp.Text = bookingId.ToString();
        lblCollectedAmountDisp.Text = lblCollectedAmount.Text.Trim();
        lblBillAmountDisp.Text = lblBillAmount.Text.Trim();
        lblBalanceAmountDisp.Text = lblBalanceAmount.Text.Trim();
        // ViewState["BalanceAmount"]= lblBalanceAmount.Text.Trim(); 

        GetPaymentType();
        MpeBillService.Show();

        if (lblCustomerMobile.Text != "")
        {
            txtCustomerMobile.Text = lblCustomerMobile.Text;
            txtCustomerMobile.Enabled = false;
        }
        else
        {
            txtCustomerMobile.Text = "";
            txtCustomerMobile.Enabled = true;
        }

        txtCustomerMobile.Focus();
    }

    protected void ImgClose_Click(object sender, ImageClickEventArgs e)
    {
        MpeBillService.Hide();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        MpeBillService.Hide();
        if (RadbtnReceiptBalanceDetails.SelectedValue == "0")
        {
            BindReceiptDtlStatusRefund();
        }
        else if (RadbtnReceiptBalanceDetails.SelectedValue == "1")
        {
            BindReceiptDtlStatusSettled();
        }
        else
        {
        }
    }

    protected void btnRefund_Click(object sender, EventArgs e)
    {
        try
        {
            string sPaymentMode = string.Empty;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new ReceiptBalanceDetails()
                {
                    QueryType = "UPDATE",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    CustomerMobile = txtCustomerMobile.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim(),
                    BookingId = hfBookingid.Value.Trim(),
                    RePaymentType = ddlPaymentType.SelectedValue.Trim(),
                    BookingMedia = "DW"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ReceiptBalanceDetails/Update", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        MpeBillService.Hide();

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                        BindReceiptDtlStatusRefund();
                        if (txtCustomerMobile.Text.Trim() != "" && Convert.ToInt32(txtCustomerMobile.Text.Length) == 10)
                        {
                            SendSMS("BalanceRefund", txtCustomerMobile.Text.Trim(), lblBookingIdDisp.Text.Trim(), lblBalanceAmountDisp.Text.Trim());
                        }
                        ddlPaymentType.SelectedIndex = 0;
                        hfBookingid.Value = string.Empty;
                        txtCustomerMobile.Text = string.Empty;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        BindReceiptDtlStatusRefund();
                        ddlPaymentType.SelectedIndex = 0;
                        hfBookingid.Value = string.Empty;
                        txtCustomerMobile.Text = string.Empty;
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    ddlPaymentType.SelectedIndex = 0;
                    hfBookingid.Value = string.Empty;
                    txtCustomerMobile.Text = string.Empty;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    public class ReceiptBalanceDetails
    {
        public string BoatHouseId { get; set; }
        public string RStatus { get; set; }
        public string BookingDate { get; set; }
        public string CustomerMobile { get; set; }
        public string CreatedBy { get; set; }
        public string RePaymentType { get; set; }
        public string BookingMedia { get; set; }
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public string BoatHouseName { get; set; }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        if (RadbtnReceiptBalanceDetails.SelectedValue == "0")
        {
            divAllDetails.Visible = true;
            BindReceiptDtlStatusRefund();
        }
        else if (RadbtnReceiptBalanceDetails.SelectedValue == "1")
        {
            divAllDetails.Visible = true;
            BindReceiptDtlStatusSettled();
        }
        else
        { }

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
                        ServiceType = ServiceType.Trim(),
                        BookingId = sTransactionNo.ToString(),
                        MobileNo = sMobileNo.ToString(),
                        MediaType = "DW",
                        BranchId = Session["BoatHouseId"].ToString().Trim(),
                        BranchName = Session["BoatHouseName"].ToString().Trim(),
                        Remarks = sAmount.ToString().Trim()
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("CM_SendSMSMsg", OtpSendSms).Result;

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

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public class OtpSMS
    {
        public string ServiceType { get; set; }
        public string BookingId { get; set; }
        public string MobileNo { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string Remarks { get; set; }
        public string MediaType { get; set; }
    }
}