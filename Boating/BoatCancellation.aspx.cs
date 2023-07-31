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
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BoatCancellation : System.Web.UI.Page
{
    //private object gvddlTripStartTime;
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

                BindCancelBookingDetails();
                txtBookingId.Enabled = true;
                ddlCancellationType.SelectedValue = "1";
                GetPaymentType();
                GetTaxDetail();
                divRepay.Visible = true;
                divBookingPaymentType.Visible = false;
                GetCancellationReason();
                divFromDate.Visible = false;
                divToDate.Visible = false;

                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                txtFromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    //Common Methods
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
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
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

                            ddlPaymentTypeSearch.DataSource = dt;
                            ddlPaymentTypeSearch.DataValueField = "ConfigId";
                            ddlPaymentTypeSearch.DataTextField = "ConfigName";
                            ddlPaymentTypeSearch.DataBind();

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Payment Type Details Not Found...!');", true);
                        }

                        ddlPaymentTypeSearch.Items.Insert(0, new ListItem("All", "0"));
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

    public void GetCancellationReason()
    {
        try
        {
            ddlReason.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatCancellaions = new BoatCancellaion()
                {
                    TypeId = "32"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ConfigMstrList/Type", BoatCancellaions).Result;



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
                            ddlReason.DataSource = dt;
                            ddlReason.DataValueField = "ConfigId";
                            ddlReason.DataTextField = "ConfigName";
                            ddlReason.DataBind();

                        }


                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Cancellation Details Not Found...!');", true);
                        }

                        ddlReason.Items.Insert(0, new ListItem("Select Cancellation Reason", "0"));
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

    protected void ddlCancellationType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCancellationType.SelectedValue == "1")
        {
            clearInputs();
            divBookingId.Visible = true;
            divBulkCancellation.Visible = false;
            divBookingId.Visible = true;
            divBookingPaymentType.Visible = false;
            divRepay.Visible = true;
            divAmountSummary.Visible = true;
            divFromDate.Visible = false;
            divToDate.Visible = false;
        }
        else if (ddlCancellationType.SelectedValue == "3")
        {
            divRepay.Visible = true;
            divAmountSummary.Visible = false;
            clearInputs();
            ddlCancellationType.SelectedValue = "3";
        }
        else
        {
            divBookingId.Visible = false;
            txtBookingId.Text = string.Empty;
            BulkCancellationBookingDetails();
            divBulkCancellation.Visible = true;
            exittimedetails.Visible = false;
            ddlPaymentType.SelectedValue = "1";
            ddlPaymentTypeSearch.SelectedValue = "0";

            divFromDate.Visible = true;
            divToDate.Visible = true;

            divBookingPaymentType.Visible = true;
            divRepay.Visible = true;

            lblRefundAmount.Text = string.Empty;
            lblBoatCharges.Text = string.Empty;
            lblDepositamount.Text = string.Empty;
            lblTotalCharges.Text = string.Empty;
            lblCancelCharges.Text = string.Empty;
        }
    }

    protected void txtBookingId_TextChanged(object sender, EventArgs e)

    {
        if (txtBookingId.Text == " ")
        {
            exittimedetails.Visible = false;
            //  BookingDetailsBasedCharges();
            BookingDetailsBasedOnBookingId();
            BookingNoteDetails();
        }
        else
        {
            exittimedetails.Visible = false;
            // BookingDetailsBasedCharges();
            BookingDetailsBasedOnBookingId();
            BookingNoteDetails();
            divCharges.Visible = false;
            if (ddlCancellationType.SelectedValue == "3")
            {
                divAmountSummary.Visible = false;
                divRepay.Visible = true;
            }
            else
            {
                divAmountSummary.Visible = true;
                divRepay.Visible = true;
            }
        }
    }

    public void BookingNoteDetails()
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

                var Cancellation = new BoatCancellaion()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString()
                };

                response = client.PostAsJsonAsync("CancellationNoteDetails", Cancellation).Result;

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
                            gvNoteDetails.DataSource = dt;
                            gvNoteDetails.DataBind();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            txtBookingId.Focus();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Cancellation Note Details Found  ');", true);
                        txtBookingId.Text = string.Empty;
                        txtBookingId.Focus();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindCancelBookingDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new BoatCancellaion()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("CancellationDetails", BoatHouseId).Result;

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
                            gvCancelBooking.DataSource = dt;
                            gvCancelBooking.DataBind();


                            //ViewState["BookingId"] = dt.Rows[0]["BookingId"].ToString();
                            //ViewState["BoatReferenceNo"] = dt.Rows[0]["BoatReferenceNo"].ToString();
                            //ViewState["BookingPin"] = dt.Rows[0]["BookingPin"].ToString();
                            //ViewState["BookingDate"] = dt.Rows[0]["BookingDate"].ToString();
                            //ViewState["InitNetAmount"] = dt.Rows[0]["InitNetAmount"].ToString();
                            //ViewState["BoatCharge"] = dt.Rows[0]["BoatCharge"].ToString();
                            //ViewState["DepositAmount"] = dt.Rows[0]["DepositAmount"].ToString();
                            //ViewState["CancelCharges"] = dt.Rows[0]["CancelCharges"].ToString();
                            //ViewState["CancelRefund"] = dt.Rows[0]["CancelRefund"].ToString();
                            //ViewState["CustomerMobile"] = dt.Rows[0]["CustomerMobile"].ToString();
                            //ViewState["PaymentType"] = dt.Rows[0]["PaymentType"].ToString();
                            //ViewState["CancellationDate"] = dt.Rows[0]["CancellationDate"].ToString();
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            gvCancelBooking.DataBind();

                            divGrid.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;
                        }
                    }
                    else
                    {
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;

                    }
                }
                else
                {
                    return;
                    //lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Single Cancellation

    public void BookingDetailsBasedOnBookingId()
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

                var Cancellation = new BoatCancellaion()
                {
                    BookingId = txtBookingId.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString()
                };

                response = client.PostAsJsonAsync("BookingDetailsBasedOnBookingId", Cancellation).Result;

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
                            gvBookingDetails.DataSource = dt;
                            gvBookingDetails.DataBind();
                            exittimedetails.Visible = true;
                            divRepay.Visible = true;
                            divFromDate.Visible = false;
                            divToDate.Visible = false;

                            gvBookingDetails.Visible = true;

                            ddlPaymentType.SelectedValue = dt.Rows[0]["PaymentType"].ToString();

                            //Calculate Sum and display in Footer Row


                            decimal dBoatCharge = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatCharges")));
                            decimal BoatDeposit = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatDeposit")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("InitNetAmount")));


                            gvBookingDetails.FooterRow.Cells[7].Text = dBoatCharge.ToString("N2");
                            gvBookingDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                            gvBookingDetails.FooterRow.Cells[8].Text = BoatDeposit.ToString("N2");
                            gvBookingDetails.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;

                            gvBookingDetails.FooterRow.Cells[9].Text = TotalAmount.ToString("N2");
                            gvBookingDetails.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;


                            // BookingDetailsBasedCharges();

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            txtBookingId.Focus();
                            divRepay.Visible = false;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Booking Details Found  ');", true);
                        txtBookingId.Text = string.Empty;
                        txtBookingId.Focus();

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BookingDetailsBasedCharges()
    {
        try
        {
            string BookingIdList = string.Empty;
            string BoatReferenceList = string.Empty;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();

            foreach (GridViewRow row in gvBookingDetails.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);

                    if (chkRow.Checked)
                    {
                        Label MyLabel = (Label)row.FindControl("lblBookingId");
                        Label MyLabel1 = (Label)row.FindControl("lblBoatReferenceNo");
                        string value = MyLabel.Text;
                        sb.Append("," + value);
                        string value1 = MyLabel1.Text;
                        sb1.Append("," + value1);
                    }
                }
                else
                {
                    sb.Append("NoBookingId");
                    sb1.Append("NoBookingId");
                }
            }

            BookingIdList = sb.ToString().TrimStart(',');
            BoatReferenceList = sb1.ToString().TrimStart(',');
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                var Cancellation = new BoatCancellaion()
                {
                    ActivityType = "C",
                    BookingId = txtBookingId.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    Cgst = Convert.ToDecimal(ViewState["CGST"].ToString()),
                    Sgst = Convert.ToDecimal(ViewState["SGST"].ToString()),
                    BoatReferenceNo = BoatReferenceList.ToString()
                };

                response = client.PostAsJsonAsync("PublicAndDepartmentBookingCancellationDetails", Cancellation).Result;

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
                            string a = dt.Rows[0]["ChargeType"].ToString();
                            string b = dt.Rows[0]["Charges"].ToString();
                            string c = dt.Rows[0]["TotalAmount"].ToString();
                            string d = dt.Rows[0]["NetAmount"].ToString();
                            string e = dt.Rows[0]["DepositAmount"].ToString();
                            string f = dt.Rows[0]["ChargesAmount"].ToString();
                            string g = dt.Rows[0]["CancelBoatCharges"].ToString();
                            string h = dt.Rows[0]["Cgst"].ToString();
                            string i = dt.Rows[0]["Sgst"].ToString();
                            string j = dt.Rows[0]["Refund"].ToString();
                            ViewState["FinalCharges"] = b.ToString();
                            ViewState["FinalChargeType"] = a.ToString();
                            ViewState["CancelHours"] = "Yes";
                            lblRefundAmount.Text = j.ToString();
                            lblCancelCharges.Text = f.ToString();
                            lblBoatCharges.Text = d.ToString();
                            lblDepositamount.Text = e.ToString();
                            lblTotalCharges.Text = c.ToString();
                        }
                        else
                        {
                            ViewState["FinalCharges"] = "100";
                            ViewState["FinalChargeType"] = "P";
                            ViewState["CancelHours"] = "No";

                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            txtBookingId.Focus();
                        }
                    }
                    else if (StatusCode == 2)
                    {
                        ViewState["FinalCharges"] = "100";
                        ViewState["FinalChargeType"] = "P";
                        ViewState["CancelHours"] = "No";

                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            string c = dt.Rows[0]["TotalAmount"].ToString();
                            string d = dt.Rows[0]["NetAmount"].ToString();
                            string e = dt.Rows[0]["DepositAmount"].ToString();
                            string f = dt.Rows[0]["ChargesAmount"].ToString();
                            string j = dt.Rows[0]["Refund"].ToString();
                            lblRefundAmount.Text = j.ToString();
                            lblCancelCharges.Text = f.ToString();
                            lblBoatCharges.Text = d.ToString();
                            lblDepositamount.Text = e.ToString();
                            lblTotalCharges.Text = c.ToString();
                        }
                    }
                    else
                    {
                        divAmountSummary.Visible = false;
                        divCharges.Visible = false;
                        gvBookingDetails.Visible = false;

                        ViewState["CancelHours"] = "No";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        txtBookingId.Text = string.Empty;
                        txtBookingId.Focus();
                    }
                }
                else
                {
                    ViewState["CancelHours"] = "No";
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void gvBookingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBookingDetails.PageIndex = e.NewPageIndex;
        BookingDetailsBasedOnBookingId();
    }

    protected void gvCancelBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCancelBooking.PageIndex = e.NewPageIndex;
        BindCancelBookingDetails();
    }

    //Bulk Cancellation
    public void BulkCancellationBookingDetails()
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
                var Cancellation = new BoatCancellaion()
                {

                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    Fromdate = txtFromDate.Text,
                    Todate = txtToDate.Text,
                    PaymentTypeId = ddlPaymentTypeSearch.SelectedValue.Trim()

                };

                response = client.PostAsJsonAsync("BulkCancellationDetails", Cancellation).Result;

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
                            gvBulkCancellation.DataSource = dt;
                            gvBulkCancellation.DataBind();
                            divFromDate.Visible = true;
                            divToDate.Visible = true;
                            divRepay.Visible = true;
                        }
                        else
                        {
                            gvBulkCancellation.DataBind();
                            divRepay.Visible = false;
                        }
                    }
                    else
                    {
                        gvBulkCancellation.DataBind();
                        divRepay.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Today No Booking Details Found ');", true);
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


    //Clear Inputs and Cancel Process
    public void clearInputs()
    {
        txtBookingId.Text = string.Empty;
        exittimedetails.Visible = false;
        theDivstatement.Visible = false;
        txtstatement.Text = string.Empty;
        ddlCancellationType.SelectedValue = "1";
        txtBookingId.Enabled = true;
        ddlPaymentType.SelectedValue = "1";
        divBookingId.Visible = true;
        divBulkCancellation.Visible = false;
        gvBulkCancellation.DataSource = null;
        gvBulkCancellation.DataBind();

        lblRefundAmount.Text = string.Empty;
        lblBoatCharges.Text = string.Empty;
        lblDepositamount.Text = string.Empty;
        lblTotalCharges.Text = string.Empty;
        lblCancelCharges.Text = string.Empty;

        divRepay.Visible = true;
        divBookingPaymentType.Visible = false;
        ddlReason.SelectedValue = "0";

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearInputs();
        BindCancelBookingDetails();
    }

    //Add New

    protected void lbtnNew_Click(object sender, EventArgs e)
    {

        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
        divFromDate.Visible = false;
        divToDate.Visible = false;
        clearInputs();
        divRepay.Visible = true;
        divBookingPaymentType.Visible = false;

    }

    //Final Process Cancellation Both Single and Bulk
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;

            foreach (GridViewRow row in gvBookingDetails.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);


                    if (chkRow.Checked)
                    {
                        i++;
                    }
                }
            }

            //var j = i;




            if (i == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Select Any One Booking Id! ');", true);
                return;

            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                string comments = string.Empty;
                string APIName = string.Empty;
                string BookingIdList = string.Empty;
                string BoatReferenceList = string.Empty;
                string QueryType = string.Empty;
                string CancellationHours = string.Empty;
                string sCancellationType = string.Empty;
                string BookingPinList = string.Empty;
                string BookingDateList = string.Empty;

                StringBuilder sb = new StringBuilder();
                StringBuilder sb1 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                StringBuilder sb3 = new StringBuilder();

                foreach (GridViewRow row in gvBookingDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);

                        if (chkRow.Checked)
                        {
                            Label MyLabel = (Label)row.FindControl("lblBookingId");
                            Label MyLabel1 = (Label)row.FindControl("lblBoatReferenceNo");
                            Label MyLabelBpin = (Label)row.FindControl("lblBookinPin");
                            Label MyLabelBDate = (Label)row.FindControl("lblBookingDate");


                            BookingDateList = MyLabelBDate.Text.Trim();


                            string value = MyLabel.Text;
                            sb.Append("," + value);
                            string value1 = MyLabel1.Text;
                            sb1.Append("," + value1);
                            string valueBPin = MyLabelBpin.Text;
                            sb2.Append("," + valueBPin);


                        }
                    }
                    else
                    {
                        sb.Append("NoBookingId");
                        sb1.Append("NoBookingId");
                    }
                }

                BookingIdList = sb.ToString().TrimStart(',');
                BoatReferenceList = sb1.ToString().TrimStart(',');
                BookingPinList = sb2.ToString().TrimStart(',');



                if (ddlCancellationType.SelectedValue == "1")
                {
                    // BookingIdList = "null";
                    //BoatReferenceList = "null";
                    APIName = "CancellationUpdate";
                    QueryType = "CancellationUpdate";
                    CancellationHours = ViewState["CancelHours"].ToString();
                    sCancellationType = "Customer";

                }
                else if (ddlCancellationType.SelectedValue == "3")
                {
                    // BookingIdList = "null";
                    // BoatReferenceList = "null";
                    APIName = "CancellationUpdate";
                    QueryType = "CancellationUpdate";
                    CancellationHours = ViewState["CancelHours"].ToString();
                    sCancellationType = "BoatHouse";

                }
                else
                {
                    QueryType = "BulkCancellationUpdate";
                    APIName = "BulkCancellationUpdate";
                    txtBookingId.Text = string.Empty;
                    sCancellationType = "";
                    CancellationHours = "No";


                    foreach (GridViewRow row in gvBulkCancellation.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("CheckBox1") as CheckBox);

                            if (chkRow.Checked)
                            {
                                Label MyLabel = (Label)row.FindControl("lblBookingId");
                                Label MyLabel1 = (Label)row.FindControl("lblBoatReferenceNo");
                                Label MyLabelBpin = (Label)row.FindControl("lblBookinPin");
                                Label MyLabelBDate = (Label)row.FindControl("lblBookingDate");
                                BookingDateList = MyLabelBDate.Text.Trim();
                                string value = MyLabel.Text;
                                sb.Append("," + value);
                                string value1 = MyLabel1.Text;
                                sb1.Append("," + value1);
                                string valueBPin = MyLabelBpin.Text;
                                sb2.Append("," + valueBPin);

                            }
                        }
                        else
                        {
                            sb.Append("NoBookingId");
                            sb1.Append("NoBookingId");
                        }
                    }

                    BookingIdList = sb.ToString().TrimStart(',');
                    BoatReferenceList = sb1.ToString().TrimStart(',');
                    BookingPinList = sb2.ToString().TrimStart(',');


                    ViewState["FinalCharges"] = "0";
                    ViewState["FinalChargeType"] = "F";
                    ViewState["CGST"] = "0";
                    ViewState["SGST"] = "0";

                    if (BookingIdList.Length <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Booking Id For Cancellation !');", true);
                        return;
                    }
                }

                if (ddlReason.SelectedValue == "5")
                {
                    comments = txtstatement.Text.Trim();
                }
                else
                {
                    comments = ddlReason.SelectedItem.Text.Trim();
                }

                var Cancellation1 = new BoatCancellaion()
                {
                    QueryType = QueryType.ToString(),
                    BookingId = txtBookingId.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    ActivityType = "C",
                    Charges = ViewState["FinalCharges"].ToString(),
                    ChargeType = ViewState["FinalChargeType"].ToString(),
                    Comments = comments.Trim(),
                    CancelledBy = Session["UserId"].ToString(),
                    CancelledMedia = "DW",
                    CreatedBy = Session["UserId"].ToString(),
                    Cgst = Convert.ToDecimal(ViewState["CGST"].ToString()),
                    Sgst = Convert.ToDecimal(ViewState["SGST"].ToString()),
                    PaymentType = ddlPaymentType.SelectedValue.Trim(),
                    BookingIdList = BookingIdList.ToString(),
                    BoatReferenceNoList = BoatReferenceList.ToString(),
                    CancellationHours = CancellationHours.ToString(),
                    CancellationType = sCancellationType.ToString().Trim(),
                    CancelOtherService = "",
                    OtherCancelledBy = ""
                };
                response = client.PostAsJsonAsync(APIName.ToString(), Cancellation1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        if (ddlCancellationType.SelectedValue == "1")
                        {
                            Response.Redirect("~/Boating/Print?rt=rRefCancel&bi=&BId=" + txtBookingId.Text.Trim() + "&BPin=" + BookingPinList.Trim() + "&BDate=" + BookingDateList.Trim() + "&CPayType=" + ddlPaymentType.SelectedItem.Text.Trim() + "&CBtCharge=" + lblBoatCharges.Text.Trim() + "&CDepAmnt=" + lblDepositamount.Text.Trim() + "&CTotCharge=" + lblTotalCharges.Text.Trim() + "&CnclCharge=" + lblCancelCharges.Text.Trim() + "&CReAmt=" + lblRefundAmount.Text.Trim() + "&CUserId=" + Session["PrintUserName"].ToString() + " ");
                            clearInputs();
                        }
                        else if (ddlCancellationType.SelectedValue == "3")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Boat Cancellation Successfully !');", true);
                            clearInputs();
                            BindCancelBookingDetails();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ddlPaymentTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BulkCancellationBookingDetails();
    }
    protected void btnBind_Click(object sender, EventArgs e)
    {
        BulkCancellationBookingDetails();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        BulkCancellationBookingDetails();
    }

    public class BoatCancellaion
    {
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public string BoatHouseId { get; set; }
        public string ActivityType { get; set; }
        public string DeductedCharges { get; set; }
        public string TotalCharges { get; set; }
        public string Comments { get; set; }
        public string CancelledBy { get; set; }
        public string CancelledMedia { get; set; }
        public string CreatedBy { get; set; }
        public string Charges { get; set; }
        public string ChargeType { get; set; }
        public string ServiceId { get; set; }
        public string ValidDate { get; set; }
        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeId { get; set; }
        public string BookingIdList { get; set; }
        public string BoatReferenceNoList { get; set; }
        public string CancellationHours { get; set; }
        public string TypeId { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string CancellationType { get; set; }
        public string BoatReferenceNo { get; set; }
        public string CancelOtherService { get; set; }
        public string OtherCancelledBy { get; set; }
    }


    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        divCharges.Visible = true;

        if (ddlCancellationType.SelectedValue == "3")
        {

            ViewState["FinalCharges"] = "0";
            ViewState["FinalChargeType"] = "0";
            ViewState["CGST"] = 0;
            ViewState["SGST"] = 0;

            ViewState["CancelHours"] = "Yes";
            divAmountSummary.Visible = false;
            divRepay.Visible = true;
        }
        else
        {
            divAmountSummary.Visible = true;
            divRepay.Visible = true;

            BookingDetailsBasedCharges();
        }
    }
}