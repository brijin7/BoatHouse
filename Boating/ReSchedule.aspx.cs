using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_Reschedule : System.Web.UI.Page
{
    public string GetAntiForgeryToken()
    {
        string cookieToken, formToken;
        AntiForgery.GetTokens(null, out cookieToken, out formToken);
        ViewState["__AntiForgeryCookie"] = cookieToken;
        return formToken;
    }
    IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);
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
                txtBookingId.Focus();
                GetRescheduleReason();
                divSingleReschedule.Visible = true;
                GetPaymentType();
                GetTaxDetail();
                divBookingNewTime.Visible = false;
                btnBind.Style.Add("display", "none");
                btnAdd.Style.Add("display", "none");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public DataTable GetBoatSlotTimeNew(string sBoatTypeId, string sCheckInDate, string sBoatSeaterId, string sSlotType)
    {
        DataTable dt = new DataTable();
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var GetBoatSlot = new ReScheduling()
                {
                    BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                    CheckInDate = sCheckInDate.Trim(),
                    BoatTypeId = sBoatTypeId.Trim(),
                    BoatSeaterId = sBoatSeaterId.Trim(),
                    SlotType = sSlotType.Trim(),
                    UserId = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("GetAvailableBoatSlotTime", GetBoatSlot).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vResponseMessage = response.Content.ReadAsStringAsync().Result;
                    string sResponseMessage = JObject.Parse(vResponseMessage)["Response"].ToString();
                    int iStatusCode = Convert.ToInt16(JObject.Parse(vResponseMessage)["StatusCode"].ToString());
                    if (sResponseMessage == "No Records Found.")
                    {
                        dt = null;
                    }
                    dt = JsonConvert.DeserializeObject<DataTable>(sResponseMessage);

                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        dt = null;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
        return dt;
    }
    public void GetBookingDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var body = new ReScheduling();
                HttpResponseMessage response;
                if (ddlReschedule.SelectedValue.Trim() == "1")
                {
                    body = new ReScheduling()
                    {
                        QueryType = "SingleBookingDetails",
                        BookingId = txtBookingId.Text.Trim(),
                        BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "0",
                        MobileNo = "0",
                        stgBookingPin = "",
                        RescheduleOldDate = "",
                        RescheduleNewDate = "",
                        RescheduleType = "CR"/* Customer Reschedule*/
                    };
                }
                else if (ddlReschedule.SelectedValue.Trim() == "3")
                {
                    body = new ReScheduling()
                    {
                        QueryType = "SingleBookingDetails",
                        BookingId = txtBookingId.Text.Trim(),
                        BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "0",
                        MobileNo = "0",
                        stgBookingPin = "",
                        RescheduleOldDate = "",
                        RescheduleNewDate = "",
                        RescheduleType = "BHR" /*Boat House Reschedule*/
                    };
                }
                response = client.PostAsJsonAsync("BookingReschedule/RescheduleDetails", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows[0]["Alert"].ToString().Split('~')[0].Trim() == "Success")
                    {
                        gvSingleReschedule.Visible = true;
                        gvSingleReschedule.DataSource = dtExists;
                        gvSingleReschedule.DataBind();
                        ViewState["txtBookingNewDate"] = dtExists.Rows[0]["date"].ToString();
                        DataTable dt = new DataTable();
                        foreach (GridViewRow gvr in gvSingleReschedule.Rows)
                        {
                            TextBox txtDate = (TextBox)gvr.FindControl("stxtDate");
                            txtDate.Attributes.Add("readonly", "readonly");
                            txtDate.Text = dtExists.Rows[gvr.RowIndex]["RescheduledDate"].ToString();

                            dt = GetBoatSlotTimeNew(dtExists.Rows[gvr.RowIndex]["BoatTypeId"].ToString(),
                                                    txtDate.Text,
                                                    dtExists.Rows[gvr.RowIndex]["BoatSeaterId"].ToString(),
                                                    dtExists.Rows[gvr.RowIndex]["PremiumStatus"].ToString());

                            DropDownList ddlSlottime = (DropDownList)gvr.FindControl("sddlSlotTime");
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                ddlSlottime.DataSource = dt;
                                ddlSlottime.DataValueField = "SlotId";
                                ddlSlottime.DataTextField = "SlotStartTime";
                                ddlSlottime.DataBind();
                                ddlSlottime.Items.Insert(0, new ListItem("Select Time Slot", "0"));
                            }
                            else
                            {
                                ddlSlottime.Items.Clear();
                                ddlSlottime.Items.Insert(0, new ListItem("Select Time Slot", "0"));
                            }
                        }


                        decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<double>("BoatCharge")));

                        gvSingleReschedule.FooterRow.Cells[6].Text = "Total";
                        gvSingleReschedule.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;

                        gvSingleReschedule.FooterRow.Cells[7].Text = TotalAmount.ToString();
                        gvSingleReschedule.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                        decimal TotalDepAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<double>("DepositAmount")));

                        ViewState["BillAmount"] = TotalAmount.ToString();
                        gvSingleReschedule.FooterRow.Cells[8].Text = TotalDepAmount.ToString();
                        gvSingleReschedule.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    }
                    else
                    {
                        clearInputs();
                        gvSingleReschedule.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + dtExists.Rows[0]["Alert"].ToString().Split('~')[1].Trim() + "');", true);
                        txtBookingId.Text = string.Empty;
                    }
                }
                else
                {
                    clearInputs();
                    gvSingleReschedule.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    txtBookingId.Text = string.Empty;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    /// <summary>
    /// Validated By : Vediyappan.V
    /// Validated Date : 2023-05-02
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtBookingId_TextChanged(object sender, EventArgs e)
    {
        try
        {

            divSummary.Visible = false;
            if (txtBookingId.Text.Trim() != "")
            {
                string BookingId = string.Empty;
                if (txtBookingId.Text.Substring(0, 1).Trim() == "b")
                {
                    BookingId = txtBookingId.Text.Replace("b", "B");
                }
                else
                {
                    BookingId = txtBookingId.Text.Trim();
                }
                if (ddlReschedule.SelectedValue == "1")
                {
                    GetPaymentType();
                    getRescheduleCharge(BookingId.ToString().Trim());
                }
                else if (ddlReschedule.SelectedValue == "3")
                {
                    getRescheduleCharge(BookingId.ToString().Trim());
                    if (gvSingleReschedule.Visible == true)
                    {
                        divBoatHouseReschedule.Visible = true;
                    }
                    ddlReschedule.SelectedValue = "3";
                }
            }
            else
            {
                clearInputs();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearInputs();

    }
    public void clearInputs()
    {
        divSingleReschedule.Visible = true;
        txtBookingId.Text = string.Empty;
        ddlReschedule.SelectedValue = "1";
        gvSingleReschedule.Visible = false;
        gvBulkReschedule.Visible = false;
        divSummary.Visible = false;
        divBoatHouseReschedule.Visible = false;
        divBookingNewTime.Visible = false;
        divSingleRescheduleGrid.Visible = true;
        divBulkRescheduleGrid.Visible = false;
        divNote.Visible = false;
        divFromDate.Visible = false;
        divToDate.Visible = false;
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddlRescheduleReason.SelectedValue = "0";
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                string[] BookingPin;
                string[] BookingDate;
                string[] SlotId;

                string sBookingPin = string.Empty;
                string sBookingDate = string.Empty;
                string sSlotId = string.Empty;
                string sMSG = string.Empty;
                if (ddlReschedule.SelectedValue.Trim() == "1")
                {
                    foreach (GridViewRow gvr in gvSingleReschedule.Rows)
                    {
                        DropDownList ddlSlottime = (DropDownList)gvr.FindControl("sddlSlotTime");
                        Label lblBookingPin = (Label)gvr.FindControl("slblBookingPin");

                        if (ddlSlottime.SelectedValue != "0")
                        {
                            TextBox txtDate = (TextBox)gvr.FindControl("stxtDate");
                            sBookingPin += lblBookingPin.Text.Trim() + ",";
                            sBookingDate += txtDate.Text.Trim() + ",";
                            sSlotId += ddlSlottime.SelectedValue.Trim() + ",";
                        }
                    }
                    BookingPin = sBookingPin.Split(',');
                    BookingDate = sBookingDate.Split(',');
                    SlotId = sSlotId.Split(',');

                    if (SlotId[0] == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Slot time.');", true);
                        return;
                    }

                    string BookingId = txtBookingId.Text.Trim();
                    if (txtBookingId.Text.Substring(0, 1).Trim() == "b")
                    {
                        BookingId = txtBookingId.Text.Replace("b", "B");
                    }
                    else
                    {
                        BookingId = txtBookingId.Text.Trim();
                    }
                    var Reschedule = new ReScheduling()
                    {
                        QueryType = "SingleRescheduleSlotType",
                        BookingId = BookingId.ToString().Trim(),
                        BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                        BoatHouseName = Session["BoatHouseName"].ToString(),
                        BookingMedia = "DW",
                        BookingDate = BookingDate,
                        RescheduledTotalcharge = "0",
                        CGST = ViewState["CGST"].ToString().Trim(),
                        SGST = ViewState["SGST"].ToString().Trim(),
                        RescheduledCharge = "0",
                        PaymentType = ddlPaymentType.SelectedValue,
                        ActivityId = ViewState["ActivityId"].ToString(),
                        Hour = "0",
                        Minute = "0",
                        BookingPin = BookingPin,
                        SlotId = SlotId,
                        CreatedBy = Convert.ToInt32(Session["UserId"].ToString()),
                        ChargeType = ViewState["ChargeType"].ToString().Trim(),
                        ChargeAmount = ViewState["RescheduleCharge"].ToString().Trim(),
                        RescheduleReason = ddlRescheduleReason.SelectedValue.Trim(),
                        OthersRescheduleDate = ""
                    };

                    response = client.PostAsJsonAsync("BookingReschedule", Reschedule).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                        List<string> ParticularResponse = new List<string>();
                        ParticularResponse = JsonConvert.DeserializeObject<List<string>>(ResponseMsg);
                        string success = string.Empty;
                        string Failure = string.Empty;
                        string[] SuccessRes;

                        foreach (string SccOrFail in ParticularResponse)
                        {
                            SuccessRes = SccOrFail.Split('~');
                            if (SuccessRes[0].ToString().Trim() == "Success")
                            {
                                success += SuccessRes[1] + " ,";
                            }
                            else
                            {
                                Failure += SuccessRes[1] + " ,";
                            }
                        }

                        if (success != "" && Failure == "")
                        {
                            clearInputs();
                            Response.Redirect("PrintBoat.aspx?rt=brdl&bi=" + BookingId.ToString().Trim() + "&bpin=" + success.ToString().TrimEnd(',') + "");
                        }
                        else if (success == "" && Failure != "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Failure.ToString().TrimEnd(',') + "- Reschedule UnSuccessful" + "');", true);
                            clearInputs();
                        }
                        else
                        {
                            Response.Redirect("PrintBoat.aspx?rt=brdl&bi=" + BookingId.ToString().Trim() + "&bpin=" + success.ToString().TrimEnd(',') + "");
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + success.ToString().TrimEnd(',') + "- Rescheduled Successfully And " + Failure.ToString().TrimEnd(',') + "- Recschedule UnSuccessful" + "');", true);
                            clearInputs();
                        }
                    }
                    else
                    {
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    }
                }
                else if (ddlReschedule.SelectedValue.Trim() == "2")
                {
                    string sdate_format = string.Empty;
                    sdate_format = string.Concat(dlOh1OpenHours.SelectedValue, ':', ddlOh1OpenMinutes.SelectedValue);

                    int totalCount = gvBulkReschedule.Rows.Cast<GridViewRow>().Count(r => ((CheckBox)r.FindControl("chkBookingId")).Checked);

                    if (totalCount != 0)
                    {
                        StringBuilder sb1 = new StringBuilder();
                        foreach (GridViewRow row in gvBulkReschedule.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chkRow = (row.Cells[0].FindControl("chkBookingId") as CheckBox);
                                if (chkRow.Checked)
                                {
                                    Label MyLabel1 = (Label)row.FindControl("blblBookingId");
                                    string value1 = MyLabel1.Text;
                                    Boolean result = sb1.ToString().Contains(value1);
                                    if (result != true)
                                    {
                                        sb1.Append(value1);
                                        sb1.Append(",");
                                    }
                                }
                            }
                            else
                            {
                                sb1.Append("NoBookingId");
                            }
                        }

                        string a = sb1.ToString().TrimEnd(','); ;
                        string[] sResult = a.Split(',');
                        string sBookingId = "";
                        for (int i = 0; i < sResult.Count(); i++)
                        {
                            sBookingId = sResult[i].ToString();

                            var Reschedule = new ReScheduling()
                            {
                                QueryType = "BulkReschedule",
                                BookingId = sBookingId,
                                BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                                BoatHouseName = Session["BoatHouseName"].ToString(),
                                BookingMedia = "DW",
                                RescheduledTotalcharge = "0",
                                CGST = "0",
                                SGST = "0",
                                RescheduledCharge = "0",
                                PaymentType = "0",
                                ActivityId = "0",
                                Hour = dlOh1OpenHours.SelectedValue,
                                Minute = ddlOh1OpenMinutes.SelectedValue,
                                CreatedBy = Convert.ToInt32(Session["UserId"].ToString()),
                                OthersRescheduleDate = ""
                            };

                            response = client.PostAsJsonAsync("BookingReschedule", Reschedule).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                                if (StatusCode == 1)
                                {
                                    clearInputs();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                                }
                                else
                                {
                                    clearInputs();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                                }
                            }
                            else
                            {
                                clearInputs();
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Check Atleast Any Of the Booking for Further Process.');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void ddlReschedule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReschedule.SelectedValue == "1")
        {
            clearInputs();
        }
        else if (ddlReschedule.SelectedValue == "3")
        {
            clearInputs();
            ddlReschedule.SelectedValue = "3";
        }
        else
        {
            divSummary.Visible = false;
            divSingleReschedule.Visible = false;
            gvSingleReschedule.Visible = false;
            divBookingNewTime.Visible = true;
            var hours = Enumerable.Range(00, 24).Select(i => i.ToString("D2"));
            var minutes = Enumerable.Range(00, 60).Select(i => i.ToString("D2"));
            dlOh1OpenHours.DataSource = hours;
            dlOh1OpenHours.DataBind();
            ddlOh1OpenMinutes.DataSource = minutes;
            ddlOh1OpenMinutes.DataBind();
            GetRescheduleReason();
            GetBulkBookingDetails();
            divSingleRescheduleGrid.Visible = false;
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

    /// <summary>
    /// Validated By : Vesdiyappan.V 
    /// Validated On : 2023-05-02
    /// Bulk Reschedule already blocked, reason is when try to reschedule assign new slot in feature day, so our managment said bulk reschedule block this concept.
    /// </summary>
    public void GetBulkBookingDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new ReScheduling()
                {
                    QueryType = "BulkBookingDetails",
                    BookingId = "0",
                    BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                    FromDate = txtFromDate.Text,
                    ToDate = txtToDate.Text,
                    UserId = "0",
                    MobileNo = "0",
                    stgBookingPin = "",
                    RescheduleOldDate = "",
                    RescheduleNewDate = "",
                    RescheduleType = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BookingReschedule/RescheduleDetails", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        string status = dtExists.Rows[0]["Status"].ToString();
                        if (status == "")
                        {
                            divBulkRescheduleGrid.Visible = true;
                            divNote.Visible = true;
                            divFromDate.Visible = true;
                            divToDate.Visible = true;
                            divSingleRescheduleGrid.Visible = false;
                            gvBulkReschedule.Visible = true;
                            gvBulkReschedule.DataSource = dtExists;
                            gvBulkReschedule.DataBind();

                            decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<double>("NetAmount")));

                            gvBulkReschedule.FooterRow.Cells[6].Text = "Total";
                            gvBulkReschedule.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;

                            gvBulkReschedule.FooterRow.Cells[7].Text = TotalAmount.ToString();
                            gvBulkReschedule.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                            decimal TotalDepAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<double>("DepositAmount")));

                            gvBulkReschedule.FooterRow.Cells[8].Text = TotalDepAmount.ToString();
                            gvBulkReschedule.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            clearInputs();
                            gvBulkReschedule.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + dtExists.Rows[0]["Status"].ToString() + "');", true); ;
                            txtBookingId.Text = string.Empty;
                        }
                    }
                    else
                    {

                        divBulkRescheduleGrid.Visible = false;

                        divNote.Visible = true;
                        divFromDate.Visible = true;
                        divToDate.Visible = true;

                        gvBulkReschedule.Visible = false;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Bookings Found.');", true);

                        txtBookingId.Text = string.Empty;
                    }

                }
                else
                {
                    clearInputs();

                    gvBulkReschedule.Visible = false;
                    txtBookingId.Text = string.Empty;
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
    public void GetTaxDetail()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatCancellaions = new BoatCancellaion()
                {
                    ServiceId = "1",
                    ValidDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TaxMstr/IdDate", BoatCancellaions).Result;

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
                            string aa = dt.Rows[0]["TaxName"].ToString();
                            string[] b = aa.Split(',');

                            ViewState["TaxCGST"] = b[0].ToString();
                            ViewState["TaxSGST"] = b[1].ToString();

                            int a = getIndexofNumber(b[0]);
                            string Numberpart = b[0].Substring(a, b[0].Length - a);

                            int a1 = getIndexofNumber(b[1]);
                            string Numberpart1 = b[1].Substring(a1, b[1].Length - a);

                            decimal Add = Convert.ToDecimal(Numberpart) + Convert.ToDecimal(Numberpart1);
                            ViewState["CGST"] = Numberpart.ToString();
                            ViewState["SGST"] = Numberpart1.ToString();
                        }
                        else
                        {
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    private int getIndexofNumber(string cell)
    {
        int indexofNum = -1;
        foreach (char c in cell)
        {
            indexofNum++;
            if (Char.IsDigit(c))
            {
                return indexofNum;
            }
        }
        return indexofNum;
    }
    private void CalculateSummary(decimal iNetAmount)
    {
        try
        {
            decimal ChargesAmount = 0;
            decimal InitNetAmount = 0;
            decimal Percentage = 0;
            decimal Cgst = 0;
            decimal Sgst = 0;
            decimal BoatCharge = 0;

            if (ViewState["ChargeType"].ToString() == "P")
            {
                InitNetAmount = iNetAmount;
                ChargesAmount = Math.Round(iNetAmount);
                Percentage = (100) + ((Convert.ToDecimal(ViewState["CGST"])) + (Convert.ToDecimal(ViewState["SGST"])));
                Cgst = Math.Round(ChargesAmount / Percentage * (Convert.ToDecimal(ViewState["CGST"])), 2);
                ViewState["submitCGST"] = Cgst;
                Sgst = Math.Round(ChargesAmount / Percentage * (Convert.ToDecimal(ViewState["SGST"])), 2);
                ViewState["submitSGST"] = Sgst;
                BoatCharge = Math.Round(((ChargesAmount) - (Cgst + Sgst)), 2);

                sumResheduleCharge.Text = ChargesAmount.ToString();
                SpNetAmount.InnerText = ChargesAmount.ToString();
                sumGst.InnerText = (Cgst + Sgst).ToString();
                sumTotal.InnerText = BoatCharge.ToString();
            }
            else
            {
                InitNetAmount = iNetAmount;
                ChargesAmount = Math.Round(iNetAmount);
                Percentage = (100) + ((Convert.ToDecimal(ViewState["CGST"])) + (Convert.ToDecimal(ViewState["SGST"])));
                Cgst = Math.Round(ChargesAmount / Percentage * (Convert.ToDecimal(ViewState["CGST"])), 2);
                ViewState["submitCGST"] = Cgst;
                Sgst = Math.Round(ChargesAmount / Percentage * (Convert.ToDecimal(ViewState["SGST"])), 2);
                ViewState["submitSGST"] = Sgst;
                BoatCharge = Math.Round(((ChargesAmount) - (Cgst + Sgst)), 2);

                sumResheduleCharge.Text = ChargesAmount.ToString();
                SpNetAmount.InnerText = ChargesAmount.ToString();
                sumGst.InnerText = (Cgst + Sgst).ToString();
                sumTotal.InnerText = BoatCharge.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void GetRescheduledDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new ReScheduling()
                {
                    QueryType = "Grid",
                    BookingId = "0",
                    BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    UserId = "0",
                    MobileNo = "0",
                    stgBookingPin = "",
                    RescheduleOldDate = "",
                    RescheduleNewDate = "",
                    RescheduleType = ""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BookingReschedule/RescheduleDetails", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        GvBoatRescheduling.Visible = true;
                        GvBoatRescheduling.DataSource = dtExists;
                        GvBoatRescheduling.DataBind();
                    }
                    else
                    {
                        divEntry.Visible = true;
                        divGridList.Visible = false;
                        GvBoatRescheduling.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found.');", true);
                    }
                }
                else
                {
                    divEntry.Visible = true;
                    divGridList.Visible = false;
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
    public string CheckEligibleForReschedule(string sBookingId, string sBookingPin, string sRescheduleOldDate, string sRescheduleNewDate)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new ReScheduling()
                {
                    QueryType = "CheckWDAndWEBoatCharge",
                    BookingId = sBookingId.ToString().Trim(),
                    BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy").Trim(),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy").Trim(),
                    UserId = "0",
                    MobileNo = "0",
                    stgBookingPin = sBookingPin.ToString().Trim(),
                    RescheduleType = "",
                    RescheduleOldDate = sRescheduleOldDate.ToString().Trim(),
                    RescheduleNewDate = sRescheduleNewDate.ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BookingReschedule/RescheduleDetails", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        string sMessage = dtExists.Rows[0]["Alert"].ToString().Trim();
                        return sMessage;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return "";
        }
    }
    protected void imgbuttonPrint_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = false;
            divGridList.Visible = true;
            clearInputs();
            GetRescheduledDetails();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    protected void imgbtnTicket_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGridList.Visible = false;
            clearInputs();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    protected void GvBoatRescheduling_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBoatRescheduling.PageIndex = e.NewPageIndex;
        GetRescheduledDetails();
    }
    protected void GvBoatRescheduling_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((Label)e.Row.FindControl("lblRescheduledTotalCharge")).Text == "0")
                {
                    Label RescheduledTotalCharge = (Label)e.Row.FindControl("lblRescheduledTotalCharge");
                    RescheduledTotalCharge.ForeColor = Color.Red;
                }
                else
                {
                    Label RescheduledTotalCharge = (Label)e.Row.FindControl("lblRescheduledTotalCharge");
                    RescheduledTotalCharge.ForeColor = Color.Green;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    public void GetRescheduleReason()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var RescheduleReason = new RescheduleReason()
                {
                    TypeId = "33",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ConfigMstrList/Type", RescheduleReason).Result;

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
                            ddlRescheduleReason.DataSource = dt;
                            ddlRescheduleReason.DataValueField = "ConfigId";
                            ddlRescheduleReason.DataTextField = "ConfigName";
                            ddlRescheduleReason.DataBind();
                        }
                        else
                        {
                            ddlRescheduleReason.DataBind();
                        }
                        ddlRescheduleReason.Items.Insert(0, new ListItem("Select Reschedule Reason", "0"));
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    /// <summary>
    /// Validated By : Vediyappan.V 
    /// Validated Date : 2023-05-02
    /// </summary>
    /// <param name="sBookingId"></param>
    public void getRescheduleCharge(string sBookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var body = new ReScheduling();

                if (ddlReschedule.SelectedValue.Trim() == "1")
                {
                    body = new ReScheduling()
                    {
                        QueryType = "GetRescheduleCharges",
                        BookingId = sBookingId,
                        BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "0",
                        MobileNo = "0",
                        stgBookingPin = "",
                        RescheduleOldDate = "",
                        RescheduleNewDate = "",
                        RescheduleType = "CR"/* Customer Reschedule*/
                    };
                }
                else if (ddlReschedule.SelectedValue.Trim() == "3")
                {
                    body = new ReScheduling()
                    {
                        QueryType = "GetRescheduleCharges",
                        BookingId = sBookingId,
                        BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "0",
                        MobileNo = "0",
                        stgBookingPin = "",
                        RescheduleOldDate = "",
                        RescheduleNewDate = "",
                        RescheduleType = "BHR" /*Boat House Reschedule*/
                    };
                }
                else
                {
                    //This Used For BulK Rescheduling
                    body = new ReScheduling()
                    {
                        QueryType = "GetRescheduleCharges",
                        BookingId = sBookingId,
                        BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "0",
                        MobileNo = "0",
                        RescheduleType = "CR",
                        RescheduleOldDate = "",
                        RescheduleNewDate = "",
                        stgBookingPin = ""
                    };
                }

                response = client.PostAsJsonAsync("BookingReschedule/RescheduleDetails", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows[0]["Alert"].ToString().Split('~')[0].Trim() == "Success")
                    {
                        GetBookingDetails();
                        divBulkRescheduleGrid.Visible = false;
                        divSingleRescheduleGrid.Visible = true;
                        ViewState["ActivityId"] = dtExists.Rows[0]["ActivityId"].ToString();
                        if (ddlReschedule.SelectedValue == "1")
                        {
                            ViewState["ChargeType"] = dtExists.Rows[0]["ChargeType"].ToString();
                            ViewState["RescheduleCharge"] = dtExists.Rows[0]["Charges"].ToString();
                            ViewState["RescheduleRule"] = dtExists.Rows[0]["Description"].ToString();

                            if (dtExists.Rows[0]["ChargeType"].ToString() == "P")
                            {
                                lblRescheduleChargeFixed.Visible = false;
                                lblRescheduleChargePercent.Visible = true;
                                lblRescheduleChargePercent.Text = ViewState["RescheduleCharge"].ToString() + " % Per Boat";
                                lblReasonName.Text = ViewState["RescheduleRule"].ToString();

                                decimal NetAmount = Math.Round((((Convert.ToDecimal(ViewState["RescheduleCharge"])) * Convert.ToDecimal(ViewState["BillAmount"])) / 100));
                            }
                            else
                            {
                                lblRescheduleChargeFixed.Visible = true;
                                lblRescheduleChargePercent.Visible = false;
                                lblRescheduleChargeFixed.Text = "₹" + ViewState["RescheduleCharge"].ToString() + " Per Boat";
                                lblReasonName.Text = ViewState["RescheduleRule"].ToString();

                                decimal NetAmount = Math.Round(Convert.ToDecimal(ViewState["RescheduleCharge"]) * (gvSingleReschedule.Rows.Count));
                            }
                        }
                    }
                    else
                    {
                        txtBookingId.Text = string.Empty;
                        divBulkRescheduleGrid.Visible = false;
                        gvSingleReschedule.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + dtExists.Rows[0]["Alert"].ToString().Split('~')[1].Trim() + "');", true);
                    }
                }
                else
                {
                    divBulkRescheduleGrid.Visible = false;
                    gvSingleReschedule.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GetBulkBookingDetails();
    }
    protected void btnBHReschedule_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                string[] BookingPin;
                string[] BookingDate;
                string[] SlotId;

                string sBookingPin = string.Empty;
                string sBookingDate = string.Empty;
                string sSlotId = string.Empty;
                string sMSG = string.Empty;

                if (ddlReschedule.SelectedValue.Trim() == "3")
                {
                    foreach (GridViewRow gvr in gvSingleReschedule.Rows)
                    {
                        DropDownList ddlSlottime = (DropDownList)gvr.FindControl("sddlSlotTime");
                        Label lblBookingPin = (Label)gvr.FindControl("slblBookingPin");

                        if (ddlSlottime.SelectedValue != "0")
                        {
                            TextBox txtDate = (TextBox)gvr.FindControl("stxtDate");
                            sBookingPin += lblBookingPin.Text.Trim() + ",";
                            sBookingDate += txtDate.Text.Trim() + ",";
                            sSlotId += ddlSlottime.SelectedValue.Trim() + ",";
                        }
                    }
                    BookingPin = sBookingPin.Split(',');
                    BookingDate = sBookingDate.Split(',');
                    SlotId = sSlotId.Split(',');

                    if (SlotId[0] == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Slot time.');", true);
                        return;
                    }
                    var Reschedule = new ReScheduling()
                    {
                        QueryType = "BoatHouseRescheduleSlotType",
                        BookingId = txtBookingId.Text.Trim(),
                        BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString()),
                        BoatHouseName = Session["BoatHouseName"].ToString(),
                        BookingMedia = "DW",
                        BookingDate = BookingDate,
                        RescheduledTotalcharge = "0",
                        CGST = "0",
                        SGST = "0",
                        RescheduledCharge = "0",
                        PaymentType = "0",
                        ActivityId = ViewState["ActivityId"].ToString(),
                        Hour = "0",
                        Minute = "0",
                        BookingPin = BookingPin,
                        SlotId = SlotId,
                        CreatedBy = Convert.ToInt32(Session["UserId"].ToString()),
                        ChargeType = "",
                        ChargeAmount = "0",
                        RescheduleReason = ddlRescheduleReason.SelectedValue.Trim(),
                        OthersRescheduleDate = ""
                    };
                    response = client.PostAsJsonAsync("BookingReschedule", Reschedule).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                        List<string> ParticularResponse = new List<string>();
                        ParticularResponse = JsonConvert.DeserializeObject<List<string>>(ResponseMsg);
                        string success = string.Empty;
                        string Failure = string.Empty;
                        string[] SuccessRes;

                        foreach (string SccOrFail in ParticularResponse)
                        {
                            SuccessRes = SccOrFail.Split('~');
                            if (SuccessRes[0].ToString() == "Success")
                            {
                                success += SuccessRes[1] + " ,";
                            }
                            else
                            {
                                Failure += SuccessRes[1] + " ,";
                            }
                        }

                        if (success != "" && Failure == "")
                        {
                            clearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + success.ToString().TrimEnd(',') + "- Rescheduled Successfully" + "');", true);
                        }
                        else if (success == "" && Failure != "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Failure.ToString().TrimEnd(',') + "- Reschedule UnSuccessful" + "');", true);
                            clearInputs();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + success.ToString().TrimEnd(',') + "- Rescheduled Successfully And " + Failure.ToString().TrimEnd(',') + "- Recschedule UnSuccessful" + "');", true);
                            clearInputs();
                        }
                    }
                    else
                    {
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    protected void btnBHRescheduleCancel_Click(object sender, EventArgs e)
    {
        clearInputs();
    }
    protected void stxtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;

            Decimal FinalBoatCharges = 0;
            Decimal FinalBoatCharges_P = 0;
            Label lblBookingId = (Label)row.FindControl("slblBookingId");
            Label lblBookingPin = (Label)row.FindControl("slblBookingPin");
            Label lblRescheduleOldDate = (Label)row.FindControl("slblRescheduledDate");
            TextBox txtRescheduleNewDate = (TextBox)row.FindControl("stxtDate");

            if (ViewState["ChargeType"].ToString() == "P" && ddlReschedule.SelectedValue.Trim() == "1")
            {
                string sMsg = CheckEligibleForReschedule(lblBookingId.Text.Trim(),
                                                     lblBookingPin.Text.Trim(),
                                                     lblRescheduleOldDate.Text.Trim(),
                                                     txtRescheduleNewDate.Text.Trim());

                string[] sMsgSplit = sMsg.Split('~');

                if (sMsgSplit[0].ToString().Trim() == "Failure")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + sMsgSplit[1].ToString().Trim() + "');", true);
                    txtRescheduleNewDate.Text = lblRescheduleOldDate.Text.Trim();
                    return;
                }
            }

            int index = row.RowIndex;
            string BoatTypeId = string.Empty;
            string SeaterTypeId = string.Empty;
            string PremiumStatus = string.Empty;

            foreach (GridViewRow gvr in gvSingleReschedule.Rows)
            {
                DropDownList ddlSlottime = (DropDownList)gvr.FindControl("sddlSlotTime");
                if (index == gvr.RowIndex)
                {
                    TextBox txtDate = (TextBox)gvr.FindControl("stxtDate");
                    BoatTypeId = gvSingleReschedule.DataKeys[gvr.RowIndex]["BoatTypeId"].ToString();
                    SeaterTypeId = gvSingleReschedule.DataKeys[gvr.RowIndex]["BoatSeaterId"].ToString();
                    PremiumStatus = gvSingleReschedule.DataKeys[gvr.RowIndex]["PremiumStatus"].ToString();
                    dt = GetBoatSlotTimeNew(BoatTypeId.ToString().Trim(),
                                            txtDate.Text.Trim(),
                                            SeaterTypeId.ToString().Trim(),
                                            PremiumStatus.ToString().Trim());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ddlSlottime.DataSource = dt;
                        ddlSlottime.DataValueField = "SlotId";
                        ddlSlottime.DataTextField = "SlotTime";
                        ddlSlottime.DataBind();
                        ddlSlottime.Items.Insert(0, new ListItem("Select Time Slot", "0"));
                    }
                    else
                    {
                        ddlSlottime.Items.Clear();
                        ddlSlottime.Items.Insert(0, new ListItem("Select Time Slot", "0"));
                    }
                }
                if (ddlReschedule.SelectedValue == "1")
                {
                    if (ViewState["ChargeType"].ToString() == "P")
                    {
                        Label BoatCharge = (Label)gvr.FindControl("slblNetAmount");
                        if (ddlSlottime.SelectedValue != "0")
                        {
                            FinalBoatCharges += Convert.ToDecimal(BoatCharge.Text.Trim());
                        }
                    }
                    else
                    {
                        if (ddlSlottime.SelectedValue != "0")
                        {
                            FinalBoatCharges += Convert.ToDecimal(ViewState["RescheduleCharge"].ToString().Trim());
                        }
                    }
                    if (ViewState["ChargeType"].ToString() == "P")
                    {
                        FinalBoatCharges_P = Math.Round((((Convert.ToDecimal(ViewState["RescheduleCharge"])) * FinalBoatCharges) / 100));
                        CalculateSummary(FinalBoatCharges_P);
                    }
                    else
                    {
                        CalculateSummary(FinalBoatCharges);
                    }
                    if (FinalBoatCharges != 0)
                    {
                        divSummary.Visible = true;
                    }
                    else
                    {
                        divSummary.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
        finally
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ScrollChanege", "ScrollChanege();", true);
        }
    }
    protected void sddlSlotTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;

            DropDownList ddlSlotTimeCheck = (DropDownList)row.FindControl("sddlSlotTime");
            Label lblRescheduledDate = (Label)row.FindControl("slblRescheduledDate");
            Label lblSlotTime = (Label)row.FindControl("slblSlotTime");
            TextBox txtDate = (TextBox)row.FindControl("stxtDate");
            string TimeSlotId = gvSingleReschedule.DataKeys[row.RowIndex]["TimeSlotId"].ToString().Trim();
            if (lblRescheduledDate.Text.Trim() == txtDate.Text.Trim() && ddlSlotTimeCheck.SelectedValue.Trim() == TimeSlotId.ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(`Slot - ${'" + lblSlotTime.Text.Trim() + "'} "
                                                    + "Is Already Exists On ${'" + lblRescheduledDate.Text.Trim() + "'}. "
                                                    + " Please Select Different Slot Time`);", true);
                ddlSlotTimeCheck.SelectedValue = "0";
            }

            if (ddlReschedule.SelectedValue == "1")
            {
                Decimal FinalBoatCharges = 0;
                Decimal FinalBoatCharges_P = 0;

                if (ViewState["ChargeType"].ToString() == "P")
                {
                    foreach (GridViewRow gvr in gvSingleReschedule.Rows)
                    {
                        Label BoatCharge = (Label)gvr.FindControl("slblNetAmount");
                        DropDownList ddlSlottime = (DropDownList)gvr.FindControl("sddlSlotTime");
                        if (ddlSlottime.SelectedValue != "0")
                        {
                            FinalBoatCharges += Convert.ToDecimal(BoatCharge.Text.Trim());
                        }
                    }
                    FinalBoatCharges_P = Math.Round((((Convert.ToDecimal(ViewState["RescheduleCharge"])) * FinalBoatCharges) / 100));
                    CalculateSummary(FinalBoatCharges_P);
                }
                else
                {
                    foreach (GridViewRow gvr in gvSingleReschedule.Rows)
                    {
                        DropDownList ddlSlottime = (DropDownList)gvr.FindControl("sddlSlotTime");
                        if (ddlSlottime.SelectedValue != "0")
                        {
                            FinalBoatCharges += Convert.ToDecimal(ViewState["RescheduleCharge"].ToString().Trim());
                        }
                    }
                    CalculateSummary(FinalBoatCharges);
                }
                if (FinalBoatCharges != 0)
                {
                    divSummary.Visible = true;
                }
                else
                {
                    divSummary.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);

        }
        finally
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ScrollChanege", "ScrollChanege();", true);
        }
    }
    public class BoatCancellaion
    {
        public string ServiceId { get; set; }
        public string ValidDate { get; set; }
        public string BoatHouseId { get; set; }
    }
    public class RescheduleReason
    {
        public string TypeId { get; set; }
    }
    public class ReScheduling
    {
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public int BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BookingMedia { get; set; }
        public string[] BookingDate { get; set; }
        public int CreatedBy { get; set; }
        public string ActivityId { get; set; }
        public string RescheduleCharge { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RescheduledTotalcharge { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string RescheduledCharge { get; set; }
        public string PaymentType { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string CheckInDate { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string SlotType { get; set; }
        public string ChargeType { get; set; }
        public string ChargeAmount { get; set; }
        public string[] SlotId { get; set; }
        public string[] BookingPin { get; set; }
        public string stgBookingPin { get; set; }
        public string RescheduleType { get; set; }
        public string RescheduleReason { get; set; }
        public string RescheduleOldDate { get; set; }
        public string RescheduleNewDate { get; set; }
        public string OthersRescheduleDate { get; set; }
    }
}