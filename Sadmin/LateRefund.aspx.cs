using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Helpers;

public partial class Sadmin_LateRefund : System.Web.UI.Page
{
    IFormatProvider obj = new System.Globalization.CultureInfo("en-GB", true);
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
                DateTime date = DateTime.Now.AddDays(-1);
                txtFromDate.Text = date.ToString("dd-MM-yyyy");
                BindBoatHouseName();
            }

        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }
    #region Bind Boat House Name
    public void BindBoatHouseName()
    {
        try
        {
            ddlBoatHouseId.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("ddlBoatHouse?CorpId=" + Session["CorpId"].ToString() +"").Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ddlBoatHouseId.DataSource = dt;
                            ddlBoatHouseId.DataValueField = "BoatHouseId";
                            ddlBoatHouseId.DataTextField = "BoatHouseName";
                            ddlBoatHouseId.DataBind();
                        }
                        else
                        {
                            ddlBoatHouseId.DataBind();
                        }

                        ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                    }
                    else
                    {
                        ddlBoatHouseId.DataBind();
                        ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                    }
                }
                else
                {
                    ddlBoatHouseId.DataBind();
                    ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    #endregion
    #region AdminMobileNo
    public void GetAdminMobileNo()
    {
        try
        {
            DateTime dts = DateTime.Parse(txtFromDate.Text, obj);
            string fromdate = dts.ToString("yyyy-MM-dd");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new CommonAPIMethod()
                {
                    QueryType = "GetAdminMobileNo",
                    ServiceType = "",
                    CorpId = "",
                    Input1 = ddlBoatHouseId.SelectedValue,
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                    if (dt.Rows.Count > 0)
                    {
                        ViewState["AdminNo"] = dt.Rows[0]["EmpMobileNo"].ToString();
                    }
                    else
                    {


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
    #endregion
   
    #region BtnSearch
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BoatTripCompletedWaitingForDepositRefund();
    }
    #endregion
    #region Bind Deposit Refund DEtails
    public void BoatTripCompletedWaitingForDepositRefund()
    {
        try
        {
            DateTime dts = DateTime.Parse(txtFromDate.Text, obj);
            string fromdate = dts.ToString("yyyy-MM-dd");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new CommonAPIMethod()
                {
                    QueryType = "GetLateRefundDetails",
                    ServiceType = "",
                    BoatHouseId = ddlBoatHouseId.SelectedValue,
                    Input1 = fromdate,
                    Input2 = txtPin.Text,
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                    if (dt.Rows.Count > 0)
                    {
                        GvBoatBookingTrip.DataSource = dt;
                        GvBoatBookingTrip.DataBind();
                        divGridList.Visible = true;
                        //Complaint.Visible = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found');", true);
                        GvBoatBookingTrip.DataBind();

                        divGridList.Visible = false;
                        // Complaint.Visible = false;
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
    #endregion
    #region Btn SMS 
    protected void ImgBtnSMS_Click(object sender, ImageClickEventArgs e)
    {

        if (Page.IsValid)
        {

            DateTime date = DateTime.Now.AddDays(-1);
            string fromdate = date.ToString("dd-MM-yyyy");
            if (txtFromDate.Text != fromdate)
            {
                txtFromDate.Text = fromdate;
            }

            ImageButton img = sender as ImageButton;
            GridViewRow grv = img.NamingContainer as GridViewRow;
            Label lblBookingId = grv.FindControl("lblBookingId") as Label;
            TextBox txtComplaintNo = grv.FindControl("txtComplaintNo") as TextBox;
            LinkButton lnkPinNum = grv.FindControl("lnkPinNum") as LinkButton;
            InsertLateRefund(lblBookingId.Text, lnkPinNum.Text, txtComplaintNo.Text);
        }

    }

    public void InsertLateRefund(string BookingId, string BookingPin,string ComplaintNo)
    {
        try
        {
            GetAdminMobileNo();
            DateTime dts = DateTime.Parse(txtFromDate.Text, obj);
            string fromdate = dts.ToString("yyyy-MM-dd");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new CommonAPIMethod()
                {
                    QueryType = "Insert",
                    BoatHouseId = ddlBoatHouseId.SelectedValue,
                    ComplaintNo = ComplaintNo.Trim(),
                    BookingId = BookingId.Trim(),
                    BookingPin = BookingPin.Trim(),
                    RefundDate = fromdate.ToString(),
                    Createdby = Session["UserId"].ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("LateRefund", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        SendSMS("LateDepositRefund", ViewState["AdminNo"].ToString(), BookingId + "," + BookingPin, "");
                        BoatTripCompletedWaitingForDepositRefund();
                        Clear();
                    }
                    else
                    {
                        BoatTripCompletedWaitingForDepositRefund();
                        Clear();
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
    public void SendSMS(string ServiceType, string sMobileNo, string BookingId, string sAmount)
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
                        BookingId = BookingId,
                        MobileNo = ViewState["AdminNo"].ToString(),
                        MediaType = "DW",
                        BranchId = ddlBoatHouseId.SelectedValue,
                        BranchName = ddlBoatHouseId.SelectedItem.Text,
                        Remarks = sAmount.ToString()
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
    #endregion

    #region  Clear 
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Clear();
        divGridList.Visible = false;

    }
    public void Clear()
    {
        ddlBoatHouseId.ClearSelection();
        //txtComplaintNo.Text = "";
        txtPin.Text = "";
        divGridList.Visible = false;

    }
    #endregion

    #region Refund Class

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

    public class CommonAPIMethod
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string BoatHouseId { get; set; }
        public string ComplaintNo { get; set; }
        public string RefundDate { get; set; }
        public string BookingPin { get; set; }
        public string BoatHouseName { get; set; }
        public string BookingId { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string BoatStatusId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Createdby { get; set; }
        public string CorpId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }

    #endregion
}