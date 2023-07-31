using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Helpers;

public partial class Reports_BoatHouseReport : System.Web.UI.Page
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
                 MpeTrip.Dispose();pnlTrip.Visible = false;
                if (Request.HttpMethod != "GET")
                {
                    AntiForgery.Validate(ViewState["__AntiForgeryCookie"] as string, Request.Form["__RequestVerificationToken"]);
                }
                
                GetPaymentType();
                GetAutoEndForNoDeposite();
                txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtBookingDate.Attributes.Add("readonly", "readonly");
                txtResFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtResFromDate.Attributes.Add("readonly", "readonly");
                txtResToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtResToDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Attributes.Add("readonly", "readonly");

                txt2000.Text = string.Empty;
                txt500.Text = string.Empty;
                txt200.Text = string.Empty;
                txt100.Text = string.Empty;
                txt50.Text = string.Empty;
                txt20.Text = string.Empty;
                txt10.Text = string.Empty;
                txt5.Text = string.Empty;
                txt2.Text = string.Empty;
                txt1.Text = string.Empty;

                divServiceTypeStatus.Visible = false;
                GetTripCount();
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    //GetUserName();
                    //btnSubmit.Enabled = true;
                    btnAbstractPrint.Enabled = true;
                    btnDtlRpt.Enabled = true;
                    //btnMiniPrint.Enabled = true;
                    btnClosed.Visible = false;
                    divServiceTypeStatus.Visible = false;
                    spnote.Visible = false;
                }
                else
                {
                    string UserName = Session["FirstName"].ToString().Trim() + " " + Session["LastName"].ToString().Trim();
                    ddlUserName.Items.Insert(0, new ListItem(UserName, Session["UserId"].ToString().Trim()));
                    ddlUserName.SelectedValue = Session["UserId"].ToString().Trim();
                    ddlUserName.SelectedItem.Text = UserName;
                    divServiceTypeStatus.Visible = true;
                    GetUserDenominationStatus(txtBookingDate.Text.Trim());
                    ddlUserName.Enabled = false;
                    //btnSubmit.Enabled = false;
                    btnAbstractPrint.Enabled = false;
                    btnDtlRpt.Enabled = false;
                    btnMiniPrint.Enabled = false;
                    btnClosed.Visible = false;
                    CheckServiceClosedStatus(txtBookingDate.Text.Trim());
                    spnote.Visible = false;

                }
               
              
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public void GetTripCount()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtBookingDate.Text.Trim(),
                    ToDate = "",
                    QueryType = "TripStartAndEndCount",
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        lblTripNotStartedCount.Text = dtExists.Rows[0]["TripNotStarted"].ToString();
                        lblTripNotEndedCount.Text = dtExists.Rows[0]["TripNotEnded"].ToString();
                    }
                    else
                    {
                        lblTripNotStartedCount.Text = "0";
                        lblTripNotStartedCount.Text = "0";
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

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtBookingDate.Text.Trim(),
                    ToDate = "",
                    QueryType = ViewState["sQueryType"].ToString(),
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlUserName.DataSource = dtExists;
                        ddlUserName.DataValueField = "UserId";
                        ddlUserName.DataTextField = "UserName";
                        ddlUserName.DataBind();

                        ddlMonthUserName.DataSource = dtExists;
                        ddlMonthUserName.DataValueField = "UserId";
                        ddlMonthUserName.DataTextField = "UserName";
                        ddlMonthUserName.DataBind();
                    }
                    else
                    {
                        ddlUserName.DataBind();
                        ddlMonthUserName.DataBind();
                    }
                    ddlUserName.Items.Insert(0, new ListItem("All", "0"));
                    ddlMonthUserName.Items.Insert(0, new ListItem("All", "0"));
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
    public void GetUserBoatingDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtBookingDate.Text.Trim(),
                    ToDate = "",
                    QueryType = "Boating/CashRefund",
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ddlUserName.SelectedValue

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {

                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    var ResponseMsg1 = JObject.Parse(vehicleEditresponse)["Table1"].ToString();
                    DataTable dtExists1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                    if (dtExists.Rows.Count > 0 && dtExists1.Rows.Count == 0)
                    {
                        ViewState["ServiceId"] = "4";
                        Session["Boating/CashRefund"] = "CashRefund";
                        ViewState["sQueryType"] = "GetServiceWiseCashRefundUserName";
                        if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                        {
                            ddlUserName.Items.Clear();
                            GetUserName();
                        }
                        if (ddlPaymentType.SelectedValue == "0" || ddlPaymentType.SelectedValue == "1")
                        {
                            GetRefundCashFromReport(txtBookingDate.Text.Trim());
                            GetRefundCashPaymentReport(txtBookingDate.Text.Trim());
                        }
                        else
                        {
                            divCashNote.Visible = false;
                        }

                        divServiceWise.Visible = false;
                        GvServiceWise.Visible = false;
                        hdrCollection.Visible = false;

                        hdrPayment.Visible = false;
                        GvServiceWisePayments.Visible = false;
                        tblBtCash.Visible = false;
                        // 10 Nov
                        divCashNote.Visible = false;
                        divDenomination.Visible = false;
                        // 10 Nov
                        ddlCategory.Items.Clear();
                        ddlCategory.Items.Insert(0, new ListItem("All", "0"));
                        ddlTypes.Items.Clear();
                        ddlTypes.Items.Insert(0, new ListItem("All", "0"));
                        divType.Visible = false;
                    }
                    else
                    {
                        ViewState["ServiceId"] = "1";
                        ViewState["sQueryType"] = "GetServiceWiseBoatingUserName";
                        if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                        {
                            ddlUserName.Items.Clear();
                            GetUserName();
                        }
                        GetBoatType();
                        if (ddlPaymentType.SelectedValue == "0")
                        {
                            PaymentReceivedTypes(txtBookingDate.Text.Trim());
                        }
                        CheckBoatingServiceAmountExists();
                        GridRecords();

                        Session["Boating/CashRefund"] = "Boating";

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

    public void GetAdminUserBoatingDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtBookingDate.Text.Trim(),
                    ToDate = "",
                    QueryType = "Boating/CashRefund",
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ddlUserName.SelectedValue
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    var ResponseMsg1 = JObject.Parse(vehicleEditresponse)["Table1"].ToString();
                    DataTable dtExists1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                    if (dtExists.Rows.Count > 0 && dtExists1.Rows.Count == 0)
                    {
                        ViewState["ServiceId"] = "4";
                        Session["Boating/CashRefund"] = "CashRefund";
                        ViewState["sQueryType"] = "GetServiceWiseCashRefundUserName";

                        if (ddlPaymentType.SelectedValue == "0" || ddlPaymentType.SelectedValue == "1")
                        {
                            GetRefundCashFromReport(txtBookingDate.Text.Trim());
                            GetRefundCashPaymentReport(txtBookingDate.Text.Trim());
                        }
                        else
                        {
                            divCashNote.Visible = false;
                        }

                        divServiceWise.Visible = false;
                        GvServiceWise.Visible = false;
                        hdrCollection.Visible = false;

                        hdrPayment.Visible = false;
                        GvServiceWisePayments.Visible = false;
                        tblBtCash.Visible = false;

                        divCashNote.Visible = false;
                        divDenomination.Visible = false;

                        ddlCategory.Items.Clear();
                        ddlCategory.Items.Insert(0, new ListItem("All", "0"));
                        ddlTypes.Items.Clear();
                        ddlTypes.Items.Insert(0, new ListItem("All", "0"));
                        divType.Visible = false;

                    }
                    else
                    {
                        ViewState["ServiceId"] = "1";
                        ViewState["sQueryType"] = "GetServiceWiseBoatingUserName";

                        GetBoatType();

                        CheckBoatingServiceAmountExists();
                        GridRecords();

                        Session["Boating/CashRefund"] = "Boating";
                        if (ddlUserName.SelectedIndex != 0)
                        {
                            if (ddlPaymentType.SelectedValue == "0")
                            {
                                PaymentReceivedTypes(txtBookingDate.Text.Trim());
                            }
                        }

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
    public void GetBoatType()
    {
        try
        {
            ddlCategory.Items.Clear();
            ddlTypes.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatTrip = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),


                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatType/BoatRateMstr", BoatTrip).Result;

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
                            ddlCategory.DataSource = dt;
                            ddlCategory.DataValueField = "BoatTypeId";
                            ddlCategory.DataTextField = "BoatType";
                            ddlCategory.DataBind();

                            ddlMonthCategory.DataSource = dt;
                            ddlMonthCategory.DataValueField = "BoatTypeId";
                            ddlMonthCategory.DataTextField = "BoatType";
                            ddlMonthCategory.DataBind();
                        }
                        else
                        {
                            ddlCategory.DataBind();
                            ddlMonthCategory.DataBind();
                        }

                        ddlCategory.Items.Insert(0, new ListItem("All", "0"));
                        ddlMonthCategory.Items.Insert(0, new ListItem("All", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetOtherservices()
    {
        try
        {
            ddlCategory.Items.Clear();
            ddlTypes.Items.Clear();
            ddlMonthCategory.Items.Clear();
            ddlMonthTypes.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CategoryList/BhId", CatType).Result;

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
                            ddlCategory.DataSource = dt;
                            ddlCategory.DataValueField = "ConfigId";
                            ddlCategory.DataTextField = "ConfigName";
                            ddlCategory.DataBind();

                            ddlMonthCategory.DataSource = dt;
                            ddlMonthCategory.DataValueField = "ConfigId";
                            ddlMonthCategory.DataTextField = "ConfigName";
                            ddlMonthCategory.DataBind();
                        }
                        else
                        {
                            ddlCategory.DataBind();
                            ddlMonthCategory.DataBind();
                            ddlCategory.Visible = false;
                            ddlMonthCategory.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Category Details Not Found...!');", true);
                        }
                    }

                    ddlCategory.Items.Insert(0, new ListItem("All", "0"));
                    ddlTypes.Items.Insert(0, new ListItem("All", "0"));
                    ddlMonthCategory.Items.Insert(0, new ListItem("All", "0"));
                    ddlMonthTypes.Items.Insert(0, new ListItem("All", "0"));
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

    public void getRestaurant()
    {
        try
        {
            ddlCategory.Items.Clear();
            ddlTypes.Items.Clear();
            ddlMonthCategory.Items.Clear();
            ddlMonthTypes.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var RowerCharges = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("FoodCategoryMstr/BHId", RowerCharges).Result;


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
                            ddlCategory.DataSource = dt;
                            ddlCategory.DataValueField = "CategoryId";
                            ddlCategory.DataTextField = "CategoryName";
                            ddlCategory.DataBind();

                            ddlMonthCategory.DataSource = dt;
                            ddlMonthCategory.DataValueField = "CategoryId";
                            ddlMonthCategory.DataTextField = "CategoryName";
                            ddlMonthCategory.DataBind();

                        }
                        else
                        {
                            ddlCategory.DataBind();
                            ddlMonthCategory.DataBind();
                        }
                        ddlCategory.Items.Insert(0, new ListItem("All", "0"));
                        ddlMonthCategory.Items.Insert(0, new ListItem("All", "0"));
                        ddlTypes.Items.Insert(0, new ListItem("All", "0"));
                        ddlMonthTypes.Items.Insert(0, new ListItem("All", "0"));
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

    protected void ddlServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
              MpeTrip.Dispose();pnlTrip.Visible = false;
            divCashNote.Visible = false;
            divType.Visible = false;

            divServiceWise.Visible = false;
            GvServiceWise.Visible = false;
            hdrCollection.Visible = false;

            hdrPayment.Visible = false;
            GvServiceWisePayments.Visible = false;
            tblBtCash.Visible = false;

            divDenomination.Visible = false;
            ViewState["ShowGrid"] = "0";

            ViewState["osTotal"] = "0";
            ViewState["rtTotal"] = "0";
            ViewState["AtTotal"] = "0";
            ViewState["ServiceId"] = "";
            // Select Services

            if (ddlServices.SelectedValue == "0")
            {
                ddlCategory.Items.Clear();
                ddlCategory.Items.Insert(0, new ListItem("All", "0"));
                ddlTypes.Items.Clear();
                ddlTypes.Items.Insert(0, new ListItem("All", "0"));
                ddlPaymentType.SelectedIndex = 0;
                txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            // Boating Services

            if (ddlServices.SelectedValue == "1")
            {
                GetUserBoatingDetails();
            }

            // Restaurant

            if (ddlServices.SelectedValue == "2")
            {

                ViewState["sQueryType"] = "GetServiceWiseRestaurantsUserName";
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    ddlUserName.Items.Clear();
                    GetUserName();
                }
                getRestaurant();
                divType.Visible = true;

                CheckOtherServiceAmountExists("RptRestaurantServiceWiseCollection");
                GridRecords();

            }

            // Other Services

            if (ddlServices.SelectedValue == "3")
            {

                ViewState["sQueryType"] = "GetServiceWiseOtherServicesUserName";
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    ddlUserName.Items.Clear();
                    GetUserName();
                }
                GetOtherservices();
                divType.Visible = true;

                CheckOtherServiceAmountExists("RptOtherServiceWiseCollection");
                GridRecords();
            }

            // Additional Tickets

            if (ddlServices.SelectedValue == "5")
            {

                ViewState["sQueryType"] = "GetServiceWiseAdditionalTicketsUserName";
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    ddlUserName.Items.Clear();
                    GetUserName();
                }
                GetAdditionalOtherservices();

                CheckOtherServiceAmountExists("RptOtherServiceWiseAdditionalCollection");
                GridRecords();

            }

            if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
            {
                btnAbstractPrint.Enabled = true;
                btnDtlRpt.Enabled = true;
                btnMiniPrint.Visible = false;
                btnClosed.Visible = false;
                divServiceTypeStatus.Visible = false;
                if (ddlUserName.SelectedIndex != 0)
                {
                    divServiceTypeStatus.Visible = true;
                    GetUserDenominationStatus(txtBookingDate.Text.Trim());
                    CheckServiceClosedStatus(txtBookingDate.Text.Trim());
                }
            }
            else
            {

                GetUserDenominationStatus(txtBookingDate.Text.Trim());
                CheckServiceClosedStatus(txtBookingDate.Text.Trim());

            }
            if (ddlPaymentType.SelectedValue == "0")
            {
                if (btnMiniPrint.Visible == true)
                {
                    btnMiniPrint.Visible = true;
                }
                else
                {
                    btnMiniPrint.Visible = false;
                }
                if (divDenomination.Visible == true)
                {
                    divDenomination.Visible = true;
                }
                else
                {
                    divDenomination.Visible = false;
                }

            }
            else
            {
                btnMiniPrint.Visible = false;
                divDenomination.Visible = false;
            }
            if (ddlUserName.SelectedItem.Text == "All")
            {
                divServiceWise.Visible = false;
                btnMiniPrint.Visible = false;
                divDenomination.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
              MpeTrip.Dispose();pnlTrip.Visible = false;
            btnMiniPrint.Enabled = false;
           
            if (ddlServices.SelectedIndex != 0)
            {
                GetUserDenominationStatus(txtBookingDate.Text.Trim());

                if (ddlServices.SelectedValue == "1")
                {

                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        if (ddlUserName.SelectedIndex != 0)
                        {
                            GetAdminUserBoatingDetails();
                        }

                    }

                    if (Session["Boating/CashRefund"].ToString() == "Boating")
                    {
                        CheckBoatingServiceAmountExists();
                        GridRecords();
                        divCashNote.Visible = false;
                    }
                    else
                    {
                        if (ddlPaymentType.SelectedValue == "0" || ddlPaymentType.SelectedValue == "1")
                        {
                            GetRefundCashFromReport(txtBookingDate.Text.Trim());
                            GetRefundCashPaymentReport(txtBookingDate.Text.Trim());
                        }
                        else
                        {
                            divCashNote.Visible = false;
                        }

                        CheckServiceClosedAdmin(txtBookingDate.Text.Trim());

                        btnClosed.Visible = false;
                    }


                }
                else if (ddlServices.SelectedValue == "2")
                {
                    CheckOtherServiceAmountExists("RptRestaurantServiceWiseCollection");
                    GridRecords();

                }
                else if (ddlServices.SelectedValue == "3")
                {
                    GetOtherservices();
                    CheckOtherServiceAmountExists("RptOtherServiceWiseCollection");
                    GridRecords();

                }
                else if (ddlServices.SelectedValue == "5")
                {
                    GetAdditionalOtherservices();
                    CheckOtherServiceAmountExists("RptOtherServiceWiseAdditionalCollection");
                    GridRecords();
                }


                if (ddlPaymentType.SelectedValue == "0")
                {
                    if (btnMiniPrint.Visible == true)
                    {
                        btnMiniPrint.Visible = true;
                    }
                    else
                    {
                        btnMiniPrint.Visible = false;
                    }
                    if (divDenomination.Visible == true)
                    {
                        divDenomination.Visible = true;
                    }
                    else
                    {
                        divDenomination.Visible = false;
                    }

                }
                else
                {
                    btnMiniPrint.Visible = false;
                    divDenomination.Visible = false;
                }
            }
            if (ddlUserName.SelectedItem.Text == "All" || ddlUserName.SelectedIndex == 0)
            {
                divServiceWise.Visible = false;
                btnMiniPrint.Visible = false;
                divDenomination.Visible = false;
            }
            if (ddlServices.SelectedIndex == 1 || ddlServices.SelectedIndex == 4)
            {
                ddlCategory.SelectedIndex = 0;
            }
            else
            {
                ddlCategory.SelectedIndex = 0;
                ddlTypes.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
              MpeTrip.Dispose();pnlTrip.Visible = false;
            if (ddlServices.SelectedValue == "2")
            {
                if (ddlCategory.SelectedIndex == 0)
                {
                    ddlTypes.Items.Clear();
                    ddlTypes.Items.Insert(0, new ListItem("All", "0"));
                }
                else
                {
                    GetRestaurantType();
                }
            }

            if (ddlServices.SelectedValue == "3")
            {
                if (ddlCategory.SelectedIndex == 0)
                {
                    ddlTypes.Items.Clear();
                    ddlTypes.Items.Insert(0, new ListItem("All", "0"));
                }
                else
                {
                    GetOtherServiceType();
                }
            }

            BindServiceWise();

            btnClosed.Visible = false;
            btnClosed.Visible = false;

            divDenomination.Visible = false;
            btnMiniPrint.Visible = false;
            if (ddlCategory.SelectedIndex == 0)
            {
                divDenomination.Visible = false;
            }
                     
            if (ddlUserName.SelectedItem.Text == "All")
            {
                divServiceWise.Visible = false;
                btnMiniPrint.Visible = false;
                divDenomination.Visible = false;
            }
            tblBtCash.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetRestaurantType()
    {
        try
        {
            ddlTypes.Items.Clear();
            ddlMonthTypes.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (rblServiceWise.Checked == true)
                {
                    var RowerCharges = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        Category = ddlCategory.SelectedValue.Trim()

                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RestaurantSvcCatDet", RowerCharges).Result;

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
                                ddlTypes.DataSource = dt;
                                ddlTypes.DataValueField = "ServiceId";
                                ddlTypes.DataTextField = "ServiceName";
                                ddlTypes.DataBind();

                            }
                            else
                            {
                                ddlTypes.DataBind();
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Restaurant Details Not Found !');", true);
                            }

                            ddlTypes.Items.Insert(0, new ListItem("All", "0"));

                        }
                        else
                        {
                            ddlTypes.Items.Insert(0, new ListItem("All", "0"));

                        }

                    }
                    else
                    {
                        return;
                    }
                }
                else if (rblMonthWise.Checked == true)
                {
                    var RowerCharges = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        Category = ddlMonthCategory.SelectedValue.Trim()

                    };
                    HttpResponseMessage response1 = client.PostAsJsonAsync("RestaurantSvcCatDet", RowerCharges).Result;

                    if (response1.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response1.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                            if (dt.Rows.Count > 0)
                            {
                                ddlMonthTypes.DataSource = dt;
                                ddlMonthTypes.DataValueField = "ServiceId";
                                ddlMonthTypes.DataTextField = "ServiceName";
                                ddlMonthTypes.DataBind();

                            }
                            else
                            {
                                ddlMonthTypes.DataBind();
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Restaurant Details Not Found !');", true);
                            }

                            ddlMonthTypes.Items.Insert(0, new ListItem("All", "0"));

                        }
                        else
                        {
                            ddlMonthTypes.Items.Insert(0, new ListItem("All", "0"));

                        }

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetOtherServiceType()
    {
        try
        {
            ddlTypes.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string boatTypeIds = string.Empty;
                if (rblServiceWise.Checked == true)
                {
                    var service = new Boating()
                    {
                        Category = ddlCategory.SelectedValue.Trim(),
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
                                ddlTypes.DataSource = dt;
                                ddlTypes.DataValueField = "ServiceId";
                                ddlTypes.DataTextField = "ServiceName";
                                ddlTypes.DataBind();
                            }
                            else
                            {
                                ddlTypes.DataBind();
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Service Details Not Found !');", true);
                            }
                        }

                        ddlTypes.Items.Insert(0, new ListItem("All", "0"));

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                    }
                }
                else if (rblMonthWise.Checked == true)
                {
                    var service = new Boating()
                    {
                        Category = ddlMonthCategory.SelectedValue.Trim(),
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
                                ddlMonthTypes.DataSource = dt;
                                ddlMonthTypes.DataValueField = "ServiceId";
                                ddlMonthTypes.DataTextField = "ServiceName";
                                ddlMonthTypes.DataBind();
                            }
                            else
                            {
                                ddlMonthTypes.DataBind();
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Service Details Not Found !');", true);
                            }
                        }

                        ddlMonthTypes.Items.Insert(0, new ListItem("All", "0"));

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
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

                            ddlMonthPayment.DataSource = dt;
                            ddlMonthPayment.DataValueField = "ConfigId";
                            ddlMonthPayment.DataTextField = "ConfigName";
                            ddlMonthPayment.DataBind();
                        }
                        else
                        {
                            ddlPaymentType.DataBind();
                            ddlMonthPayment.DataBind();
                        }


                        ddlPaymentType.Items.Insert(0, new ListItem("All", "0"));
                        ddlMonthPayment.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlPaymentType.Items.Insert(0, new ListItem("All", "0"));
                        ddlMonthPayment.Items.Insert(0, new ListItem("All", "0"));
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
    public void BindServiceWise()
    {
        hdrPayment.Visible = false;
        GvServiceWisePayments.Visible = false;
        tblBtCash.Visible = false;
        ViewState["btTotal"] = string.Empty;
        ViewState["btTotalPaid"] = string.Empty;
        ViewState["btTotalCashInHand"] = string.Empty;

        if (ddlServices.SelectedValue == "1")
        {

            try
            {
                divServiceWise.Visible = true;
                ViewState["btTotal"] = string.Empty;
                ViewState["btTotalPaid"] = string.Empty;
                ViewState["btTotalCashInHand"] = string.Empty;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ChallanAb = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        BookingDate = txtBookingDate.Text.Trim(),
                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                        CreatedBy = ddlUserName.SelectedValue.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RptServiceWiseCollection", ChallanAb).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response1 = response.Content.ReadAsStringAsync().Result;
                        if (Response1.Contains("No Records Found."))
                        {
                            GvServiceWise.DataBind();
                            GvServiceWise.Visible = false;
                            hdrCollection.Visible = false;

                            hdrPayment.Visible = false;
                            GvServiceWisePayments.Visible = false;
                            tblBtCash.Visible = false;

                            divDenomination.Visible = false;
                            spnote.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                            btnMiniPrint.Visible = false;
                            return;
                        }
                        else
                        {
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {

                                GvServiceWise.DataSource = dtExists;
                                GvServiceWise.DataBind();

                                GvServiceWise.Visible = true;
                                hdrCollection.Visible = true;

                                divServiceWise.Visible = true;
                                divDenomination.Visible = true;
                                btnMiniPrint.Visible = true;
                                int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                                decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                                GvServiceWise.FooterRow.Cells[1].Text = "Total";
                                GvServiceWise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                                GvServiceWise.FooterRow.Cells[2].Text = TotalCount.ToString();
                                GvServiceWise.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                                GvServiceWise.FooterRow.Cells[3].Text = TotalAmount.ToString("N2");
                                GvServiceWise.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;


                                ViewState["btTotal"] = TotalAmount.ToString().Trim();
                                // hfServiceName.Value = ViewState["btTotal"].ToString().Trim();
                                lblReceivedAmount.Text = TotalAmount.ToString("N2").Trim();
                                if (ddlPaymentType.SelectedValue == "0" || ddlPaymentType.SelectedValue == "1")
                                {
                                    using (var client1 = new HttpClient())
                                    {
                                        client1.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                                        client1.DefaultRequestHeaders.Clear();
                                        client1.DefaultRequestHeaders.Accept.Clear();
                                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                        var ChallanAb1 = new Boating()
                                        {
                                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                            BookingDate = txtBookingDate.Text.Trim(),
                                            BoatTypeId = ddlCategory.SelectedValue.Trim(),
                                            PaymentType = ddlPaymentType.SelectedValue.Trim(),
                                            CreatedBy = ddlUserName.SelectedValue.Trim()
                                        };
                                        HttpResponseMessage response1 = client.PostAsJsonAsync("RptServiceWisePayment", ChallanAb1).Result;

                                        if (response1.IsSuccessStatusCode)
                                        {
                                            var Response = response1.Content.ReadAsStringAsync().Result;
                                            if (Response.Contains("No Records Found."))
                                            {
                                                hdrPayment.Visible = false;
                                                GvServiceWisePayments.Visible = false;
                                                tblBtCash.Visible = false;
                                                GvServiceWisePayments.DataBind();

                                                //return;
                                            }
                                            else
                                            {
                                                var ResponseMsg1 = JObject.Parse(Response)["Table"].ToString();
                                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                                                if (dt.Rows.Count > 0)
                                                {

                                                    GvServiceWisePayments.DataSource = dt;
                                                    GvServiceWisePayments.DataBind();
                                                    hdrPayment.Visible = false;
                                                    GvServiceWisePayments.Visible = false;
                                                    tblBtCash.Visible = false;

                                                    GvServiceWise.Visible = true;
                                                    hdrCollection.Visible = true;

                                                    divServiceWise.Visible = true;
                                                    divDenomination.Visible = true;
                                                    btnMiniPrint.Visible = true;

                                                    int TotalCountPaid = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                                                    decimal TotalAmountPaid = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("Amount")));

                                                    GvServiceWisePayments.FooterRow.Cells[1].Text = "Total";
                                                    GvServiceWisePayments.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                                                    GvServiceWisePayments.FooterRow.Cells[2].Text = TotalCountPaid.ToString();
                                                    GvServiceWisePayments.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                                                    GvServiceWisePayments.FooterRow.Cells[3].Text = TotalAmountPaid.ToString("N2");
                                                    GvServiceWisePayments.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;


                                                    ViewState["btTotalPaid"] = TotalAmountPaid.ToString().Trim();

                                                }
                                                else
                                                {
                                                    hdrPayment.Visible = false;
                                                    GvServiceWisePayments.Visible = false;
                                                    tblBtCash.Visible = false;
                                                    GvServiceWisePayments.DataBind();
                                                    //divDenomination.Visible = false;
                                                    //spnote.Visible = false;
                                                    //btnMiniPrint.Visible = false;
                                                }
                                            }
                                        }
                                    }
                                    if (ViewState["btTotalPaid"].ToString().Trim() != "")
                                    {
                                        string PaidAmount = (Convert.ToDecimal(ViewState["btTotalPaid"].ToString().Trim())).ToString("N2");
                                        lblPaidAmount.Text = PaidAmount.Trim();
                                        lblBal.Text = (Convert.ToDecimal(lblReceivedAmount.Text) - Convert.ToDecimal(lblPaidAmount.Text)).ToString("N2");
                                    }
                                    else
                                    {
                                        lblPaidAmount.Text = "0.00";
                                        lblBal.Text = (Convert.ToDecimal(lblReceivedAmount.Text)).ToString("N2");
                                    }


                                    ViewState["btTotalCashInHand"] = lblBal.Text.Trim();
                                    string CashInHand = string.Empty;

                                    if (ddlUserName.SelectedValue != "0")
                                    {
                                        if (ddlPaymentType.SelectedValue == "0")
                                        {
                                            PaymentReceivedTypes(txtBookingDate.Text.Trim());
                                        }

                                    }
                                    if (ddlPaymentType.SelectedValue == "1")
                                    {
                                        ViewState["OtherPayments"] = "0.00";
                                    }
                                    if (ViewState["OtherPayments"] == null || ViewState["OtherPayments"].Equals("-1"))
                                    {
                                        ViewState["OtherPayments"] = "0.00";
                                        lblCard.Text = "0";
                                        lblOnline.Text = "0";
                                        lblUPI.Text = "0";
                                    }

                                    decimal CashInHands = (Convert.ToDecimal(ViewState["btTotalCashInHand"].ToString().Trim())) - (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()));
                                    CashInHand = Math.Round(CashInHands, 0).ToString();

                                    if ((Convert.ToDecimal(CashInHand.Trim())) < 0)
                                    {
                                        hfServiceName.Value = "0";
                                    }
                                    else
                                    {
                                        hfServiceName.Value = CashInHand.Trim();
                                    }


                                    ViewState["btFinalCashInHand"] = hfServiceName.Value;
                                    lblCashInHand.Text = (Convert.ToDecimal(CashInHand.Trim())).ToString("N2");
                                    lblFinalNetAmount.Text = ((Convert.ToDecimal(CashInHand.Trim())) + (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()))).ToString("N2");

                                    if (ddlPaymentType.SelectedValue == "1")
                                    {
                                        tblBtCash.Visible = false;
                                    }
                                    else
                                    {
                                        tblBtCash.Visible = true;
                                    }

                                }   //hfServiceName.Value = lblBal.Text.Trim();
                            }
                            else
                            {
                                divServiceWise.Visible = false;
                                hdrCollection.Visible = false;
                                GvServiceWise.Visible = false;

                                hdrPayment.Visible = false;
                                GvServiceWisePayments.Visible = false;
                                tblBtCash.Visible = false;

                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found');", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                        divServiceWise.Visible = false;

                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
                return;
            }

        }

        if (ddlServices.SelectedValue == "2")
        {

            try
            {
                divServiceWise.Visible = true;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ChallanAb = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        BookingDate = txtBookingDate.Text.Trim(),
                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                        Category = ddlTypes.SelectedValue.Trim(),
                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                        CreatedBy = ddlUserName.SelectedValue.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RptRestaurantServiceWiseCollection", ChallanAb).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response1 = response.Content.ReadAsStringAsync().Result;
                        if (Response1.Contains("No Records Found."))
                        {
                            GvServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            GvServiceWise.DataBind();

                            divDenomination.Visible = false;
                            spnote.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                            btnMiniPrint.Visible = false;
                            btnClosed.Visible = false;
                            return;

                        }
                        else
                        {
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {

                                GvServiceWise.DataSource = dtExists;
                                GvServiceWise.DataBind();
                                GvServiceWise.Visible = true;
                                hdrCollection.Visible = true;
                                divServiceWise.Visible = true;
                                divDenomination.Visible = true;

                                int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                                decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                                GvServiceWise.FooterRow.Cells[1].Text = "Total";
                                GvServiceWise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                                GvServiceWise.FooterRow.Cells[2].Text = TotalCount.ToString();
                                GvServiceWise.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                                GvServiceWise.FooterRow.Cells[3].Text = TotalAmount.ToString("N2");
                                GvServiceWise.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                                ViewState["rtTotal"] = string.Empty;
                                ViewState["rtTotal"] = TotalAmount.ToString().Trim();
                                hfServiceName.Value = ViewState["rtTotal"].ToString().Trim();
                                btnMiniPrint.Visible = true;
                            }
                            else
                            {
                                GvServiceWise.Visible = false;
                                hdrCollection.Visible = false;
                                divServiceWise.Visible = false;

                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Records Found');", true);
                                spnote.Visible = false;
                                divDenomination.Visible = false;
                                btnMiniPrint.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        divServiceWise.Visible = false;
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

        if (ddlServices.SelectedValue == "3")
        {

            try
            {
                divServiceWise.Visible = true;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ChallanAb = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        BookingDate = txtBookingDate.Text.Trim(),
                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                        Category = ddlTypes.SelectedValue.Trim(),
                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                        CreatedBy = ddlUserName.SelectedValue.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RptOtherServiceWiseCollection", ChallanAb).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response1 = response.Content.ReadAsStringAsync().Result;
                        if (Response1.Contains("No Records Found."))
                        {
                            GvServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            spnote.Visible = false;
                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;
                            btnClosed.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                            return;

                        }
                        else
                        {
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {

                                GvServiceWise.DataSource = dtExists;
                                GvServiceWise.DataBind();
                                GvServiceWise.Visible = true;
                                hdrCollection.Visible = true;
                                divServiceWise.Visible = true;
                                divDenomination.Visible = true;
                                btnMiniPrint.Visible = true;
                                int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                                decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                                GvServiceWise.FooterRow.Cells[1].Text = "Total";
                                GvServiceWise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                                GvServiceWise.FooterRow.Cells[2].Text = TotalCount.ToString();
                                GvServiceWise.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                                GvServiceWise.FooterRow.Cells[3].Text = TotalAmount.ToString("N2");
                                GvServiceWise.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                                ViewState["osTotal"] = string.Empty;
                                ViewState["osTotal"] = TotalAmount.ToString().Trim();
                                hfServiceName.Value = ViewState["osTotal"].ToString().Trim();
                            }
                            else
                            {
                                GvServiceWise.Visible = false;
                                hdrCollection.Visible = false;
                                divServiceWise.Visible = false;
                                spnote.Visible = false;
                                divDenomination.Visible = false;
                                btnMiniPrint.Visible = false;
                                btnClosed.Visible = false;
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Records Found');", true);
                            }
                        }
                    }
                    else
                    {
                        divServiceWise.Visible = false;
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

        if (ddlServices.SelectedValue == "5")
        {

            try
            {
                divServiceWise.Visible = true;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ChallanAb = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        BookingDate = txtBookingDate.Text.Trim(),
                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                        Category = ddlTypes.SelectedValue.Trim(),
                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                        CreatedBy = ddlUserName.SelectedValue.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RptOtherServiceWiseAdditionalCollection", ChallanAb).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response1 = response.Content.ReadAsStringAsync().Result;
                        if (Response1.Contains("No Records Found."))
                        {
                            GvServiceWise.DataBind();
                            GvServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            spnote.Visible = false;
                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                            return;
                        }
                        else
                        {
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {

                                GvServiceWise.DataSource = dtExists;
                                GvServiceWise.DataBind();
                                GvServiceWise.Visible = true;
                                hdrCollection.Visible = true;
                                divServiceWise.Visible = true;
                                btnMiniPrint.Visible = true;

                                int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                                decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                                GvServiceWise.FooterRow.Cells[1].Text = "Total";
                                GvServiceWise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                                GvServiceWise.FooterRow.Cells[2].Text = TotalCount.ToString();
                                GvServiceWise.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                                GvServiceWise.FooterRow.Cells[3].Text = TotalAmount.ToString("N2");
                                GvServiceWise.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                                ViewState["AtTotal"] = string.Empty;
                                ViewState["AtTotal"] = TotalAmount.ToString().Trim();
                                hfServiceName.Value = ViewState["AtTotal"].ToString().Trim();

                            }
                            else
                            {
                                GvServiceWise.Visible = false;
                                hdrCollection.Visible = false;
                                divServiceWise.Visible = false;

                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Records Found');", true);
                                spnote.Visible = false;
                                btnClosed.Visible = false;
                                divDenomination.Visible = false;
                                btnMiniPrint.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        divServiceWise.Visible = false;
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
    }

    protected void btnAbstractPrint_Click(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (ddlServices.SelectedValue == "1")
        {
            ReportDocument objReportDocument = new ReportDocument();
            ExportOptions objReportExport = new ExportOptions();
            DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
            string sFilePath = string.Empty;
            sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
            string rFileName = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ChallanAb = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        BookingDate = txtBookingDate.Text.Trim(),
                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                        CreatedBy = ddlUserName.SelectedValue.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RptAbstractServiceWiseCollection", ChallanAb).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response1 = response.Content.ReadAsStringAsync().Result;
                        if (Response1.Contains("No Records Found."))
                        {
                            //GvServiceWise.Visible = false;
                            //hdrCollection.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                        }
                        else
                        {
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseCollection.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Boating - " + ddlCategory.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                            }
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
            finally
            {
                if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                {
                    System.IO.File.Delete(Server.MapPath(sFilePath));
                }

                objReportDocument.Dispose();

                objReportDiskOption = null;
                objReportDocument = null;
                objReportExport = null;
                GC.Collect();
            }
        }

        if (ddlServices.SelectedValue == "2")
        {
            ReportDocument objReportDocument = new ReportDocument();
            ExportOptions objReportExport = new ExportOptions();
            DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
            string sFilePath = string.Empty;
            sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
            string rFileName = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ChallanAb = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        BookingDate = txtBookingDate.Text.Trim(),
                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                        Category = ddlTypes.SelectedValue.Trim(),
                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                        CreatedBy = ddlUserName.SelectedValue.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RptAbstractRestaurantServiceWiseCollection", ChallanAb).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response1 = response.Content.ReadAsStringAsync().Result;
                        if (Response1.Contains("No Records Found."))
                        {
                            GvServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);

                        }
                        else
                        {
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptOtherServiceWiseCollection.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(1, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(0, "Restaurant - " + ddlCategory.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                            }
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
            finally
            {
                if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                {
                    System.IO.File.Delete(Server.MapPath(sFilePath));
                }

                objReportDocument.Dispose();

                objReportDiskOption = null;
                objReportDocument = null;
                objReportExport = null;
                GC.Collect();
            }
        }

        if (ddlServices.SelectedValue == "3")
        {
            ReportDocument objReportDocument = new ReportDocument();
            ExportOptions objReportExport = new ExportOptions();
            DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
            string sFilePath = string.Empty;
            sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
            string rFileName = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ChallanAb = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        BookingDate = txtBookingDate.Text.Trim(),
                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                        Category = ddlTypes.SelectedValue.Trim(),
                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                        CreatedBy = ddlUserName.SelectedValue.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RptAbstractOtherServiceWiseCollection", ChallanAb).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response1 = response.Content.ReadAsStringAsync().Result;
                        if (Response1.Contains("No Records Found."))
                        {
                            GvServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                        }
                        else
                        {
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptOtherServiceWiseCollection.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(1, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(0, "Other Services - " + ddlCategory.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());

                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                            }
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
            finally
            {
                if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                {
                    System.IO.File.Delete(Server.MapPath(sFilePath));
                }

                objReportDocument.Dispose();

                objReportDiskOption = null;
                objReportDocument = null;
                objReportExport = null;
                GC.Collect();
            }
        }

        if (ddlServices.SelectedValue == "5")
        {
            ReportDocument objReportDocument = new ReportDocument();
            ExportOptions objReportExport = new ExportOptions();
            DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
            string sFilePath = string.Empty;
            sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
            string rFileName = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ChallanAb = new Boating()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        BookingDate = txtBookingDate.Text.Trim(),
                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                        Category = ddlTypes.SelectedValue.Trim(),
                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                        CreatedBy = ddlUserName.SelectedValue.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("RptAbstractAdditionalOtherServiceWiseCollection", ChallanAb).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response1 = response.Content.ReadAsStringAsync().Result;
                        if (Response1.Contains("No Records Found."))
                        {
                            GvServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                        }
                        else
                        {
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptOtherServiceWiseCollection.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(1, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(0, "Additional Charges - " + ddlCategory.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());

                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                            }
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
            finally
            {
                if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                {
                    System.IO.File.Delete(Server.MapPath(sFilePath));
                }

                objReportDocument.Dispose();

                objReportDiskOption = null;
                objReportDocument = null;
                objReportExport = null;
                GC.Collect();
            }
        }
    }

    protected void rblServiceWise_CheckedChanged(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (rblServiceWise.Checked == true)
        {
            divAllServices.Visible = true;
            divRestaurant.Visible = false;
            divMonthWise.Visible = false;
            rblRestaurant.Checked = false;
            rblMonthWise.Checked = false;
            //ddlUserName.SelectedIndex = 0;
            ddlUserName.ClearSelection();
            ddlServices.SelectedIndex = 0;
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("All", "0"));
            ddlTypes.Items.Clear();
            ddlTypes.Items.Insert(0, new ListItem("All", "0"));
            divType.Visible = false;
            ddlPaymentType.SelectedIndex = 0;
            txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            btnMiniPrint.Visible = false;
            divDenomination.Visible = false;
            divServiceWise.Visible = false;
            spnote.Visible = false;
            divServiceTypeStatus.Visible = false;

        }
    }

    protected void rblRestaurant_CheckedChanged(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (rblRestaurant.Checked == true)
        {
            divAllServices.Visible = false;
            divMonthWise.Visible = false;
            divRestaurant.Visible = true;
            rblServiceWise.Checked = false;
            rblMonthWise.Checked = false;
            txtResFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtResToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        rbtnResMonthWise.SelectedValue = "1";
        divResDateWise.Visible = true;
        divResMonthwise.Visible = false;
        GetFinYear();
        ddlResMonth.SelectedValue = "04";
        divResMonth.Visible = false;
        spnote.Visible = false;
        divServiceTypeStatus.Visible = false;
    }

    protected void rblMonthWise_CheckedChanged(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
        {
            GetMonthWiseUserName();
        }
        else
        {
            string UserName = Session["FirstName"].ToString().Trim() + " " + Session["LastName"].ToString().Trim();
            ddlMonthUserName.Items.Insert(0, new ListItem(UserName, Session["UserId"].ToString().Trim()));

            ddlMonthUserName.Enabled = false;
        }

        rbtnMonthWise.SelectedValue = "1";
        divDateWiseRpt.Visible = true;
        divMonthWiseRpt.Visible = false;
        GetFinYear();
        ddlMonth.SelectedValue = "04";
        divMonth.Visible = false;

        GetPaymentType();
        rblServiceWise.Checked = false;
        rblRestaurant.Checked = false;
        rblMonthWise.Checked = true;
        divRestaurant.Visible = false;
        divAllServices.Visible = false;
        divMonthWise.Visible = true;
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddlMonthUserName.SelectedIndex = 0;
        ddlMonthService.SelectedIndex = 0;
        ddlMonthCategory.Items.Clear();
        ddlMonthCategory.Items.Insert(0, new ListItem("All", "0"));
        ddlMonthTypes.Items.Clear();
        ddlMonthTypes.Items.Insert(0, new ListItem("All", "0"));
        divMonthType.Visible = false;
        ddlMonthPayment.SelectedIndex = 0;
        spnote.Visible = false;
        divServiceTypeStatus.Visible = false;
    }

    protected void btnResGenerate_Click(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sCreatedBy = string.Empty;
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    sCreatedBy = "0";
                }
                else
                {
                    sCreatedBy = Session["UserId"].ToString().Trim();
                }

                var ChallanAb = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    FromDate = txtResFromDate.Text.Trim(),
                    ToDate = txtResToDate.Text.Trim(),
                    CreatedBy = sCreatedBy
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RptRestaurantDetailedServiceWiseCollection", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        objReportDocument.Load(Server.MapPath("RptRestaurantServiceWiseCollection.rpt"));
                        objReportDocument.SetDataSource(dtExists);

                        objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                        objReportDocument.SetParameterValue(1, "Restaurant Detailed Sales Entries details Between " + txtResFromDate.Text.Trim() + " And " + txtResToDate.Text.Trim());
                        objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                        objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                        objReportExport = objReportDocument.ExportOptions;
                        objReportExport.ExportDestinationOptions = objReportDiskOption;
                        objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                        objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                        objReportDocument.Export();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(Server.MapPath(sFilePath));
                        Response.Flush();
                        Response.Close();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
        finally
        {
            if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
            {
                System.IO.File.Delete(Server.MapPath(sFilePath));
            }

            objReportDocument.Dispose();

            objReportDiskOption = null;
            objReportDocument = null;
            objReportExport = null;
            GC.Collect();
        }
    }

    protected void btnMiniPrint_Click(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (ddlServices.SelectedIndex == 0)
        {
            return;
        }

        if (Session["UserRole"].ToString().Trim() == "Admin" || Session["UserRole"].ToString().Trim() == "Sadmin")
        {
            if (ddlUserName.SelectedIndex == 0)
            {
                if (ddlServices.SelectedValue == "1")  // Boat Services
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Username !!');", true);
                    return;
                }

                if (ddlServices.SelectedValue == "2") // Restaurant Services
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Username !!');", true);
                    return;
                }
            }
        }

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HttpResponseMessage response;
                string TotalAmount = (txtfinalvalue.Value.Trim());
                string sMSG = string.Empty;
                string Type = string.Empty;
                string TypeName = string.Empty;
                string User = string.Empty;
                string UserName = string.Empty;
                string Category = string.Empty;
                string CategoryName = string.Empty;
                string PaymentType = string.Empty;
                string PaymentName = string.Empty;
                string sGvAmount = string.Empty;
                string ServiceId = string.Empty;
                string ServiceName = string.Empty;

                if (ddlServices.SelectedValue == "1")
                {
                    Type = "";
                    TypeName = "All";
                }
                else
                {
                    Type = ddlTypes.SelectedValue.Trim();
                    TypeName = ddlTypes.SelectedItem.Text.Trim();
                }

                if (ddlUserName.SelectedValue == "0")
                {
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        User = "0";
                        UserName = "All";
                    }
                    else
                    {
                        User = ddlUserName.SelectedValue.Trim();
                        UserName = ddlUserName.SelectedItem.Text;

                    }

                }
                else
                {
                    User = ddlUserName.SelectedValue.Trim();
                    UserName = ddlUserName.SelectedItem.Text.Trim();
                }
                if (ddlCategory.SelectedValue == "0")
                {
                    Category = "0";
                    CategoryName = "All";
                }
                else
                {
                    Category = ddlCategory.SelectedValue.Trim();
                    CategoryName = ddlCategory.SelectedItem.Text.Trim();
                }
                if (ddlPaymentType.SelectedValue == "0")
                {
                    PaymentType = "0";
                    PaymentName = "All";
                }
                else
                {
                    PaymentType = ddlPaymentType.SelectedValue.Trim();
                    PaymentName = ddlPaymentType.SelectedItem.Text.Trim();
                }

                if (ddlServices.SelectedValue == "1")
                {
                    if (Session["Boating/CashRefund"].ToString() == "Boating")
                    {
                        //sGvAmount = ViewState["btTotal"].ToString().Trim();
                        //string[] gvamount = ViewState["btTotalCashInHand"].ToString().Split('.');
                        sGvAmount = ViewState["btFinalCashInHand"].ToString().Trim();

                        sGvAmount = sGvAmount.ToString().Trim();
                        if (sGvAmount == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                        }
                    }
                    else
                    {
                        sGvAmount = lblRefundCashInHand.Text.Trim();
                        if (sGvAmount == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                        }
                    }

                }

                if (ddlServices.SelectedValue == "2")
                {
                    sGvAmount = ViewState["rtTotal"].ToString().Trim();
                    if (sGvAmount == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                    }
                }

                if (ddlServices.SelectedValue == "3")
                {
                    sGvAmount = ViewState["osTotal"].ToString().Trim();
                    if (sGvAmount == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                    }
                }

                //if (ddlServices.SelectedValue == "4")
                //{
                //    sGvAmount = lblRefundCashInHand.Text.Trim();
                //    if (sGvAmount == "")
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                //    }
                //}

                if (ddlServices.SelectedValue == "5")
                {
                    sGvAmount = ViewState["AtTotal"].ToString().Trim();
                    if (sGvAmount == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                    }
                }

                if (TotalAmount.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Enter Denomination Amount is Equal to Cash in Hand');", true);
                    return;
                }
                if (Convert.ToDecimal(sGvAmount.Trim()) != Convert.ToDecimal((TotalAmount.Trim())))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Enter Amount is Equal to Cash in Hand');", true);
                    txt2000.Text = string.Empty;
                    txt500.Text = string.Empty;
                    txt200.Text = string.Empty;
                    txt100.Text = string.Empty;
                    txt50.Text = string.Empty;
                    txt20.Text = string.Empty;
                    txt10.Text = string.Empty;
                    txt5.Text = string.Empty;
                    txt2.Text = string.Empty;
                    txt1.Text = string.Empty;
                    return;
                }
                if (ViewState["ServiceId"].ToString() == "4")
                {
                    ServiceId = "4";
                    ServiceName = "Cash Refund Report";
                }
                else if (ViewState["ServiceId"].ToString() == "1")
                {
                    ServiceId = "1";
                    ServiceName = "Boating";
                }
                else
                {
                    ServiceId = ddlServices.SelectedValue.Trim();
                    ServiceName = ddlServices.SelectedItem.Text.Trim();
                }
                var Report = new ServiceWise()
                {
                    QueryType = "Insert",
                    UserId = User,
                    UserName = UserName,
                    ServiceId = ServiceId,
                    Services = ServiceName,
                    CategoryId = Category,
                    Category = CategoryName,
                    TypeId = Type,
                    Types = TypeName,
                    PaymentTypeId = PaymentType,
                    PaymentType = PaymentName,
                    ServiceTotal = sGvAmount.Trim(),
                    ReferenceId = User.Trim(),
                    Denomination = "1",
                    DenominationCount = "6459",
                    DenominationAmount = "6459",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("ServiceWiseReportHistory", Report).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    sMSG = "Inserted Successfully";

                    if (response.IsSuccessStatusCode)
                    {
                        if (StatusCode == 1)
                        {

                            string Amount = string.Empty;
                            string Count = string.Empty;
                            string Deno = string.Empty;
                            string text2000 = inptxt2000.Value.Trim();
                            if (text2000.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text2000.Trim();
                                ViewState["Count"] = txt2000.Text.Trim();
                                ViewState["Deno"] = "2000";
                                Denomination();
                            }
                            string text500 = inptxt500.Value.Trim();
                            if (text500.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text500.Trim();
                                ViewState["Count"] = txt500.Text.Trim();
                                ViewState["Deno"] = "500";
                                Denomination();
                            }
                            string text200 = inptxt200.Value.Trim();
                            if (text200.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text200.Trim();
                                ViewState["Count"] = txt200.Text.Trim();
                                ViewState["Deno"] = "200";
                                Denomination();
                            }
                            string text100 = inptxt100.Value.Trim();
                            if (text100.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text100.Trim();
                                ViewState["Count"] = txt100.Text.Trim();
                                ViewState["Deno"] = "100";
                                Denomination();
                            }
                            string text50 = inptxt50.Value.Trim();
                            if (text50.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text50.Trim();
                                ViewState["Count"] = txt50.Text.Trim();
                                ViewState["Deno"] = "50";
                                Denomination();
                            }
                            string text20 = inptxt20.Value.Trim();
                            if (text20.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text20.Trim();
                                ViewState["Count"] = txt20.Text.Trim();
                                ViewState["Deno"] = "20";
                                Denomination();
                            }
                            string text10 = inptxt10.Value.Trim();
                            if (text10.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text10.Trim();
                                ViewState["Count"] = txt10.Text.Trim();
                                ViewState["Deno"] = "10";
                                Denomination();
                            }
                            string text5 = inptxt5.Value.Trim();
                            if (text5.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text5.Trim();
                                ViewState["Count"] = txt5.Text.Trim();
                                ViewState["Deno"] = "5";
                                Denomination();
                            }
                            string text2 = inptxt2.Value.Trim();
                            if (text2.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text2.Trim();
                                ViewState["Count"] = txt2.Text.Trim();
                                ViewState["Deno"] = "2";
                                Denomination();
                            }
                            string text1 = inptxt1.Value.Trim();
                            if (text1.Trim() != "0")
                            {
                                GetUniqueId();
                                ViewState["Amount"] = text1.Trim();
                                ViewState["Count"] = txt1.Text.Trim();
                                ViewState["Deno"] = "1";
                                Denomination();
                            }

                            string sden = string.Empty;

                            if (txt2000.Text.Trim() != "" && Convert.ToInt32(txt2000.Text.Trim()) > 0)
                            {

                                sden += "2000-" + txt2000.Text.Trim() + "-" + text2000.Trim();
                            }
                            if (txt500.Text.Trim() != "" && Convert.ToInt32(txt500.Text.Trim()) > 0)
                            {

                                sden += "~500-" + txt500.Text.Trim() + "-" + text500.Trim();
                            }
                            if (txt200.Text.Trim() != "" && Convert.ToInt32(txt200.Text.Trim()) > 0)
                            {

                                sden += "~200-" + txt200.Text.Trim() + "-" + text200.Trim();
                            }
                            if (txt100.Text.Trim() != "" && Convert.ToInt32(txt100.Text.Trim()) > 0)
                            {

                                sden += "~100-" + txt100.Text.Trim() + "-" + text100.Trim();
                            }
                            if (txt50.Text.Trim() != "" && Convert.ToInt32(txt50.Text.Trim()) > 0)
                            {

                                sden += "~50-" + txt50.Text.Trim() + "-" + text50.Trim();
                            }
                            if (txt20.Text.Trim() != "" && Convert.ToInt32(txt20.Text.Trim()) > 0)
                            {

                                sden += "~20-" + txt20.Text.Trim() + "-" + text20.Trim();
                            }
                            if (txt10.Text.Trim() != "" && Convert.ToInt32(txt10.Text.Trim()) > 0)
                            {

                                sden += "~10-" + txt10.Text.Trim() + "-" + text10.Trim();
                            }
                            if (txt5.Text.Trim() != "" && Convert.ToInt32(txt5.Text.Trim()) > 0)
                            {

                                sden += "~5-" + txt5.Text.Trim() + "-" + text5.Trim();
                            }
                            if (txt2.Text.Trim() != "" && Convert.ToInt32(txt2.Text.Trim()) > 0)
                            {

                                sden += "~2-" + txt2.Text.Trim() + "-" + text2.Trim();
                            }
                            if (txt1.Text.Trim() != "" && Convert.ToInt32(txt1.Text.Trim()) > 0)
                            {

                                sden += "~1-" + txt1.Text.Trim() + "-" + text1.Trim();
                            }

                            string FinalValue = sden.TrimStart('~');

                            if (FinalValue.ToString() == "")
                            {
                                sden += "0-0-0";
                                GetUniqueId();
                                ViewState["Amount"] = "0";
                                ViewState["Count"] = "0";
                                ViewState["Deno"] = "0";
                                Denomination();
                            }

                            if (ddlServices.SelectedValue == "1")  // Boat Services
                            {
                                if (Session["Boating/CashRefund"].ToString() == "Boating")
                                {
                                    Response.Redirect("~/Boating/Print.aspx?rt=rbss&UId=" + ddlUserName.SelectedValue + "&fDat=" + txtBookingDate.Text + "&bi=&UN=" + ddlUserName.SelectedItem.Text.Trim() + "&BTI=" + ddlCategory.SelectedValue.Trim() + "&BT=" + ddlCategory.SelectedItem.Text.Trim() + "&sDen=" + FinalValue.Trim() + "");
                                }
                                else
                                {
                                    Response.Redirect("~/Boating/Print?rt=recr&bi=&fDat=" + txtBookingDate.Text.Trim() + "&sDen=" + FinalValue.Trim() + "&UId=" + ddlUserName.SelectedValue + "");
                                }

                            }

                            if (ddlServices.SelectedValue == "2") // Restaurant Services
                            {
                                Response.Redirect("~/Boating/Print.aspx?rt=rrss&UId=" + ddlUserName.SelectedValue + "&fDat=" + txtBookingDate.Text + "&bi=&UN=" + ddlUserName.SelectedItem.Text.Trim() + "&sDen=" + FinalValue.Trim() + "");
                            }

                            if (ddlServices.SelectedValue == "3") // Other Services
                            {
                                Response.Redirect("~/Boating/Print.aspx?rt=ross&UId=" + ddlUserName.SelectedValue + "&fDat=" + txtBookingDate.Text + "&bi=&UN=" + ddlUserName.SelectedItem.Text.Trim() + "&sDen=" + FinalValue.Trim() + "");
                            }

                            //if (ddlServices.SelectedValue == "4") // Cash in Refund
                            //{
                            //    Response.Redirect("~/Boating/Print?rt=recr&bi=&fDat=" + txtBookingDate.Text.Trim() + "&sDen=" + FinalValue.Trim() + "");
                            //}

                            if (ddlServices.SelectedValue == "5") // Additional Ticket
                            {
                                Response.Redirect("~/Boating/Print.aspx?rt=rATss&UId=" + ddlUserName.SelectedValue + "&fDat=" + txtBookingDate.Text + "&bi=&UN=" + ddlUserName.SelectedItem.Text.Trim() + "&sDen=" + FinalValue.Trim() + "");
                            }


                            txt2000.Text = string.Empty;
                            txt500.Text = string.Empty;
                            txt200.Text = string.Empty;
                            txt100.Text = string.Empty;
                            txt50.Text = string.Empty;
                            txt20.Text = string.Empty;
                            txt10.Text = string.Empty;
                            txt5.Text = string.Empty;
                            txt2.Text = string.Empty;
                            txt1.Text = string.Empty;

                        }


                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);

                            btnAbstractPrint.Enabled = false;
                            btnDtlRpt.Enabled = false;
                            txt2000.Text = string.Empty;
                            txt500.Text = string.Empty;
                            txt200.Text = string.Empty;
                            txt100.Text = string.Empty;
                            txt50.Text = string.Empty;
                            txt20.Text = string.Empty;
                            txt10.Text = string.Empty;
                            txt5.Text = string.Empty;
                            txt2.Text = string.Empty;
                            txt1.Text = string.Empty;
                            btnMiniPrint.Visible = false;
                            divServiceWise.Visible = false;
                            divDenomination.Visible = false;


                        }
                    }

                    // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Inserted Successfully');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void btnDtlRpt_Click(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;

        if (ddlServices.SelectedValue == "1")
        {
            try
            {
                GvServiceWise.PageIndex = 0;
                Button lnkbtn = sender as Button;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

                ReportDocument objReportDocument = new ReportDocument();
                ExportOptions objReportExport = new ExportOptions();
                DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
                string sFilePath = string.Empty;
                sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
                string rFileName = string.Empty;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var ChallanAb = new Boating()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                            FromDate = txtBookingDate.Text.Trim(),
                            ToDate = txtBookingDate.Text.Trim(),
                            BoatTypeId = ddlCategory.SelectedValue.Trim(),
                            PaymentType = ddlPaymentType.SelectedValue.Trim(),
                            CreatedBy = ddlUserName.SelectedValue.Trim(),
                            ServiceName = hfServiceName.Value.Trim()
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("RptBoatingServiceWiseDetailed", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseDetail.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Boating Service - " + ddlUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
                finally
                {
                    if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                    {
                        System.IO.File.Delete(Server.MapPath(sFilePath));
                    }

                    objReportDocument.Dispose();

                    objReportDiskOption = null;
                    objReportDocument = null;
                    objReportExport = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
        if (ddlServices.SelectedValue == "2")
        {
            try
            {
                GvServiceWise.PageIndex = 0;
                Button lnkbtn = sender as Button;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

                ReportDocument objReportDocument = new ReportDocument();
                ExportOptions objReportExport = new ExportOptions();
                DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
                string sFilePath = string.Empty;
                sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
                string rFileName = string.Empty;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var ChallanAb = new Boating()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                            FromDate = txtBookingDate.Text.Trim(),
                            ToDate = txtBookingDate.Text.Trim(),
                            BoatTypeId = ddlCategory.SelectedValue.Trim(),
                            Category = ddlTypes.SelectedValue.Trim(),
                            PaymentType = ddlPaymentType.SelectedValue.Trim(),
                            CreatedBy = ddlUserName.SelectedValue.Trim(),
                            ServiceName = ""
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("RptRestaurantServiceDetailed", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseDetail.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Restaurant Service - " + ddlUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
                finally
                {
                    if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                    {
                        System.IO.File.Delete(Server.MapPath(sFilePath));
                    }

                    objReportDocument.Dispose();

                    objReportDiskOption = null;
                    objReportDocument = null;
                    objReportExport = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
        if (ddlServices.SelectedValue == "3")
        {
            try
            {
                GvServiceWise.PageIndex = 0;
                Button lnkbtn = sender as Button;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

                ReportDocument objReportDocument = new ReportDocument();
                ExportOptions objReportExport = new ExportOptions();
                DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
                string sFilePath = string.Empty;
                sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
                string rFileName = string.Empty;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var ChallanAb = new Boating()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                            FromDate = txtBookingDate.Text.Trim(),
                            ToDate = txtBookingDate.Text.Trim(),
                            BoatTypeId = ddlCategory.SelectedValue.Trim(),
                            Category = ddlTypes.SelectedValue.Trim(),
                            PaymentType = ddlPaymentType.SelectedValue.Trim(),
                            CreatedBy = ddlUserName.SelectedValue.Trim(),
                            ServiceName = ""
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("RptOtherServiceDetailed", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseDetail.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Other Service - " + ddlUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
                finally
                {
                    if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                    {
                        System.IO.File.Delete(Server.MapPath(sFilePath));
                    }

                    objReportDocument.Dispose();

                    objReportDiskOption = null;
                    objReportDocument = null;
                    objReportExport = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
        if (ddlServices.SelectedValue == "5")
        {
            try
            {
                GvServiceWise.PageIndex = 0;
                Button lnkbtn = sender as Button;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

                ReportDocument objReportDocument = new ReportDocument();
                ExportOptions objReportExport = new ExportOptions();
                DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
                string sFilePath = string.Empty;
                sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
                string rFileName = string.Empty;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var ChallanAb = new Boating()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                            BookingDate = txtBookingDate.Text.Trim(),
                            BoatTypeId = ddlCategory.SelectedValue.Trim(),
                            Category = ddlTypes.SelectedValue.Trim(),
                            PaymentType = ddlPaymentType.SelectedValue.Trim(),
                            CreatedBy = ddlUserName.SelectedValue.Trim(),
                            FromDate = txtBookingDate.Text,
                            ToDate = txtBookingDate.Text,
                            ServiceName = ""
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("RptOtherServiceAdditionalDtl", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseDetail.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Additional Charges - " + ddlUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
                finally
                {
                    if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                    {
                        System.IO.File.Delete(Server.MapPath(sFilePath));
                    }

                    objReportDocument.Dispose();

                    objReportDiskOption = null;
                    objReportDocument = null;
                    objReportExport = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
    }

    public void GetRefundCashFromReport(string FromDate)
    {
        try
        {
            ViewState["CashFromTotal"] = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                var RestaurantBooking = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate.ToString(),
                    CashflowTypes = "CashReceived",
                    RequestedBy = ddlUserName.SelectedValue
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RefundCashFromReport", RestaurantBooking).Result;

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
                            gvRefundCashfrom.DataSource = dt;
                            gvRefundCashfrom.DataBind();
                            gvRefundCashfrom.Visible = true;
                            divCashNote.Visible = true;
                            divDenomination.Visible = true;
                            btnClosed.Visible = true;
                            btnClosed.Enabled = true;

                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));
                            ViewState["CashFromTotal"] = TotalAmount;

                            gvRefundCashfrom.FooterRow.Cells[0].Text = "Total";
                            gvRefundCashfrom.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                            gvRefundCashfrom.FooterRow.Cells[1].Text = TotalAmount.ToString("N2");
                            gvRefundCashfrom.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            gvRefundCashfrom.DataBind();
                            divCashNote.Visible = false;
                            gvRefundCashfrom.Visible = false;
                            divDenomination.Visible = false;
                            lblRefundTotalFromCounter.Text = "0.00";
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                        divCashNote.Visible = false;
                        divDenomination.Visible = false;
                        btnClosed.Visible = false;
                        btnClosed.Enabled = false;
                        btnMiniPrint.Visible = false;
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

    public void GetRefundCashPaymentReport(string FromDate)
    {
        try
        {
            ViewState["CRRTotalAmount"] = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate,
                    ToDate = FromDate,
                    QueryType = "TripSheetSummaryDetails",
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = ddlUserName.SelectedValue,
                    Input5 = "ParticularUser"

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        gvRefundPayAmount.DataBind();
                        divCashNote.Visible = false;
                        divDenomination.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);

                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            gvRefundPayAmount.DataSource = dtExists;
                            gvRefundPayAmount.DataBind();
                            gvRefundPayAmount.Visible = true;
                            divCashNote.Visible = true;
                            divDenomination.Visible = true;
                            btnClosed.Visible = true;
                            btnClosed.Enabled = true;

                            decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<double>("ClaimedDeposit")));

                            ViewState["CRRTotalAmount"] = TotalAmount.ToString().Trim();

                            gvRefundPayAmount.FooterRow.Cells[0].Text = "Total";
                            gvRefundPayAmount.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                            gvRefundPayAmount.FooterRow.Cells[1].Text = TotalAmount.ToString("N2");
                            gvRefundPayAmount.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            //bblblReceivedAmt.Text = "0";
                            //bblblRefundAmt.Text = "0";
                            //bblblBal.Text = "0";
                            gvRefundPayAmount.DataBind();
                            divCashNote.Visible = false;
                            divDenomination.Visible = false;
                        }
                    }

                    if (ViewState["CashFromTotal"].ToString() != "")
                    {
                        lblRefundTotalFromCounter.Text = ViewState["CashFromTotal"].ToString();
                    }
                    else
                    {
                        lblRefundTotalFromCounter.Text = "0.00";
                    }

                    if (ViewState["CRRTotalAmount"].ToString() != "")
                    {
                        lblRefundLessAmount.Text = (Convert.ToDecimal(ViewState["CRRTotalAmount"])).ToString("N2");
                    }
                    else
                    {
                        lblRefundLessAmount.Text = "0.00";
                    }
                    if (lblRefundTotalFromCounter.Text != "0.00")
                    {
                        lblRefundCashInHand.Text = (Convert.ToDecimal(lblRefundTotalFromCounter.Text) - Convert.ToDecimal(lblRefundLessAmount.Text)).ToString();
                    }
                    else
                    {
                        lblRefundCashInHand.Text = (Convert.ToDecimal(lblRefundLessAmount.Text)).ToString();
                    }
                    
                        string[] cashinhand = lblRefundCashInHand.Text.Split('.');
                        hfServiceName.Value = cashinhand[0].ToString();                  

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Record Found');", true);
                    divCashNote.Visible = false;
                    divDenomination.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void GetAdditionalOtherservices()
    {
        try
        {
            ddlCategory.Items.Clear();
            ddlTypes.Items.Clear();
            ddlMonthCategory.Items.Clear();
            ddlMonthTypes.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("AdditionalBoatOtherService", CatType).Result;

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
                            ddlCategory.DataSource = dt;
                            ddlCategory.DataValueField = "ConfigId";
                            ddlCategory.DataTextField = "ConfigName";
                            ddlCategory.DataBind();

                            ddlMonthCategory.DataSource = dt;
                            ddlMonthCategory.DataValueField = "ConfigId";
                            ddlMonthCategory.DataTextField = "ConfigName";
                            ddlMonthCategory.DataBind();
                        }
                        else
                        {
                            ddlCategory.DataBind();
                            ddlMonthCategory.DataBind();
                            ddlCategory.Visible = false;
                            ddlMonthCategory.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Category Details Not Found...!');", true);
                        }
                    }

                    ddlCategory.Items.Insert(0, new ListItem("All", "0"));
                    ddlTypes.Items.Insert(0, new ListItem("All", "0"));
                    ddlMonthCategory.Items.Insert(0, new ListItem("All", "0"));
                    ddlMonthTypes.Items.Insert(0, new ListItem("All", "0"));
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

    public void Denomination()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string TotalAmount = (txtfinalvalue.Value.Trim());
                string sMSG = string.Empty;
                string Type = string.Empty;
                string TypeName = string.Empty;
                string User = string.Empty;
                string UserName = string.Empty;
                string Category = string.Empty;
                string CategoryName = string.Empty;
                string PaymentType = string.Empty;
                string PaymentName = string.Empty;
                string sGvAmount = string.Empty;
                string ServiceId = string.Empty;
                string ServiceName = string.Empty;

                if (ddlServices.SelectedValue == "1")
                {
                    Type = "";
                    TypeName = "NULL";
                }
                else
                {
                    Type = ddlTypes.SelectedValue.Trim();
                    TypeName = ddlTypes.SelectedItem.Text.Trim();
                }
                if (ddlUserName.SelectedValue == "0")
                {
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        User = "0";
                        UserName = "All";
                    }
                    else
                    {
                        User = ddlUserName.SelectedValue.Trim();
                        UserName = ddlUserName.SelectedItem.Text;

                    }

                }
                else
                {
                    User = ddlUserName.SelectedValue.Trim();
                    UserName = ddlUserName.SelectedItem.Text.Trim();
                }
                if (ddlCategory.SelectedValue == "0")
                {
                    Category = "0";
                    CategoryName = "ALL";
                }
                else
                {
                    Category = ddlCategory.SelectedValue.Trim();
                    CategoryName = ddlCategory.SelectedItem.Text.Trim();
                }
                if (ddlPaymentType.SelectedValue == "0")
                {
                    PaymentType = "0";
                    PaymentName = "ALL";
                }
                else
                {
                    PaymentType = ddlPaymentType.SelectedValue.Trim();
                    PaymentName = ddlPaymentType.SelectedItem.Text.Trim();
                }

                if (ddlServices.SelectedValue == "1")
                {
                    if (Session["Boating/CashRefund"].ToString() == "Boating")
                    {
                        // sGvAmount = ViewState["btTotal"].ToString().Trim();
                        // sGvAmount = ViewState["btTotalCashInHand"].ToString().Trim();
                        sGvAmount = ViewState["btFinalCashInHand"].ToString().Trim();
                        if (sGvAmount == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                        }
                    }
                    else
                    {
                        sGvAmount = lblRefundCashInHand.Text.Trim();
                        if (sGvAmount == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                        }
                    }

                }

                if (ddlServices.SelectedValue == "2")
                {
                    sGvAmount = ViewState["rtTotal"].ToString().Trim();
                    if (sGvAmount == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                    }
                }

                if (ddlServices.SelectedValue == "3")
                {
                    sGvAmount = ViewState["osTotal"].ToString().Trim();
                    if (sGvAmount == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                    }
                }

                //if (ddlServices.SelectedValue == "4")
                //{
                //    sGvAmount = lblRefundCashInHand.Text.Trim();
                //    if (sGvAmount == "")
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                //    }
                //}

                if (Convert.ToDecimal(sGvAmount.Trim()) != Convert.ToDecimal((TotalAmount.Trim())))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Enter Amount is Equal to Cash in Hand');", true);
                    txt2000.Text = string.Empty;
                    txt500.Text = string.Empty;
                    txt200.Text = string.Empty;
                    txt100.Text = string.Empty;
                    txt50.Text = string.Empty;
                    txt20.Text = string.Empty;
                    txt10.Text = string.Empty;
                    txt5.Text = string.Empty;
                    txt2.Text = string.Empty;
                    txt1.Text = string.Empty;
                    return;
                }
                if (ViewState["ServiceId"].ToString() == "4")
                {
                    ServiceId = "4";
                    ServiceName = "Cash Refund Report";
                }
                else if (ViewState["ServiceId"].ToString() == "1")
                {
                    ServiceId = "1";
                    ServiceName = "Boating";
                }
                else
                {
                    ServiceId = ddlServices.SelectedValue.Trim();
                    ServiceName = ddlServices.SelectedItem.Text.Trim();
                }
                var ReportDenomination = new ServiceWise()
                {
                    QueryType = "InsertDenomination",
                    ReferenceId = ViewState["UniqueId"].ToString().Trim(),
                    Denomination = ViewState["Deno"].ToString().Trim(),
                    DenominationCount = ViewState["Count"].ToString().Trim(),
                    DenominationAmount = ViewState["Amount"].ToString().Trim(),
                    UserId = User.Trim(),
                    UserName = UserName.Trim(),
                    ServiceId = ServiceId.Trim(),
                    Services = ServiceName.Trim(),
                    CategoryId = Category.Trim(),
                    Category = CategoryName.Trim(),
                    TypeId = Type.Trim(),
                    Types = TypeName.Trim(),
                    PaymentTypeId = PaymentType.Trim(),
                    PaymentType = PaymentName.Trim(),
                    ServiceTotal = sGvAmount.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };
                HttpResponseMessage response1 = client.PostAsJsonAsync("DenominationHistory", ReportDenomination).Result;

                var VehicleEditresponse = response1.Content.ReadAsStringAsync().Result;
                int Statuscode = Convert.ToInt32(JObject.Parse(VehicleEditresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(VehicleEditresponse)["Response"].ToString();


            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    public void GetUniqueId()
    {
        try
        {
            divServiceWise.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string Type = string.Empty;
                string TypeName = string.Empty;
                string User = string.Empty;
                string UserName = string.Empty;
                string Category = string.Empty;
                string CategoryName = string.Empty;
                string PaymentType = string.Empty;
                string PaymentName = string.Empty;
                string sGvAmount = string.Empty;
                string ServiceId = string.Empty;

                if (ddlServices.SelectedValue == "1")
                {
                    Type = "0";
                    //TypeName = "NULL";
                }
                else
                {
                    Type = ddlTypes.SelectedValue.Trim();
                    //TypeName = ddlTypes.SelectedItem.Text.Trim();
                }
                if (ddlUserName.SelectedValue == "0")
                {
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        User = "0";
                        //UserName = "All";
                    }
                    else
                    {
                        User = ddlUserName.SelectedValue.Trim();
                        //UserName = ddlUserName.SelectedItem.Text;

                    }
                    //User = "0";
                    ////UserName = "All";
                }
                else
                {
                    User = ddlUserName.SelectedValue.Trim();
                    //UserName = ddlUserName.SelectedItem.Text.Trim();
                }
                if (ddlCategory.SelectedValue == "0")
                {
                    Category = "0";
                    //CategoryName = "ALL";
                }
                else
                {
                    Category = ddlCategory.SelectedValue.Trim();
                    //CategoryName = ddlCategory.SelectedItem.Text.Trim();
                }
                if (ddlPaymentType.SelectedValue == "0")
                {
                    PaymentType = "0";
                    //PaymentName = "ALL";
                }
                else
                {
                    PaymentType = ddlPaymentType.SelectedValue.Trim();
                    //PaymentName = ddlPaymentType.SelectedItem.Text.Trim();
                }

                if (ddlServices.SelectedValue == "1")
                {
                    if (Session["Boating/CashRefund"].ToString() == "Boating")
                    {
                        sGvAmount = lblRefundCashInHand.Text.Trim();
                        if (sGvAmount == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                        }
                        //sGvAmount = ViewState["btTotal"].ToString().Trim();
                        //sGvAmount = ViewState["btTotalCashInHand"].ToString().Trim();
                        sGvAmount = ViewState["btFinalCashInHand"].ToString().Trim();
                        if (sGvAmount == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                        }
                    }
                    else
                    {
                        sGvAmount = lblRefundCashInHand.Text.Trim();

                        if (sGvAmount == "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                        }
                    }
                }

                if (ddlServices.SelectedValue == "2")
                {
                    sGvAmount = ViewState["rtTotal"].ToString().Trim();

                    if (sGvAmount == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                    }
                }

                if (ddlServices.SelectedValue == "3")
                {
                    sGvAmount = ViewState["osTotal"].ToString().Trim();

                    if (sGvAmount == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                    }
                }

                //if (ddlServices.SelectedValue == "4")
                //{
                //    sGvAmount = lblRefundCashInHand.Text.Trim();

                //    if (sGvAmount == "")
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                //    }
                //}
                if (ddlServices.SelectedValue == "5")
                {
                    sGvAmount = ViewState["AtTotal"].ToString().Trim();

                    if (sGvAmount == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!');", true);
                    }
                }

                if (ViewState["ServiceId"].ToString() == "4")
                {
                    ServiceId = "4";

                }
                else if (ViewState["ServiceId"].ToString() == "1")
                {
                    ServiceId = "1";
                }
                else
                {
                    ServiceId = ddlServices.SelectedValue.Trim();

                }
                var GetUnique = new ServiceWise()
                {

                    UserId = User.Trim(),
                    ServiceId = ServiceId.Trim(),
                    CategoryId = Category.Trim(),
                    TypeId = Type.Trim(),
                    PaymentTypeId = PaymentType.Trim(),

                    BookingDate = txtBookingDate.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("GetUniqueIdServiceWise", GetUnique).Result;

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
                            ViewState["UniqueId"] = dt.Rows[0]["ReferenceId"].ToString();
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

    protected void btnClosed_Click(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string sMSG = string.Empty;

                string Type = string.Empty;
                string TypeName = string.Empty;
                string User = string.Empty;
                string UserName = string.Empty;
                string Category = string.Empty;
                string CategoryName = string.Empty;
                string PaymentType = string.Empty;
                string PaymentName = string.Empty;
                string sGvAmount = string.Empty;
                string ServiceId = string.Empty;
                string ServiceName = string.Empty;

                if (ddlServices.SelectedValue == "1")
                {
                    Type = "";
                    TypeName = "All";
                }
                else
                {
                    Type = ddlTypes.SelectedValue.Trim();
                    TypeName = ddlTypes.SelectedItem.Text.Trim();
                }

                if (ViewState["ServiceId"].ToString() == "4")
                {
                    ServiceId = "4";
                    ServiceName = "Cash Refund Report";
                }
                else if (ViewState["ServiceId"].ToString() == "1")
                {
                    ServiceId = "1";
                    ServiceName = "Boating";
                }
                else
                {
                    ServiceId = ddlServices.SelectedValue.Trim();
                    ServiceName = ddlServices.SelectedItem.Text.Trim();
                }

                if (ddlUserName.SelectedValue == "0")
                {
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        User = "0";
                        UserName = "All";
                    }
                    else
                    {
                        User = ddlUserName.SelectedValue.Trim();
                        UserName = ddlUserName.SelectedItem.Text;

                    }
                    //User = "0";
                    //UserName = "All";
                }
                else
                {
                    User = ddlUserName.SelectedValue.Trim();
                    UserName = ddlUserName.SelectedItem.Text.Trim();
                }
                if (ddlCategory.SelectedValue == "0")
                {
                    Category = "0";
                    CategoryName = "All";
                }
                else
                {
                    Category = ddlCategory.SelectedValue.Trim();
                    CategoryName = ddlCategory.SelectedItem.Text.Trim();
                }
                if (ddlPaymentType.SelectedValue == "0")
                {
                    PaymentType = "0";
                    PaymentName = "All";
                }
                else
                {
                    PaymentType = ddlPaymentType.SelectedValue.Trim();
                    PaymentName = ddlPaymentType.SelectedItem.Text.Trim();
                }


                var Details = new ServiceWise()
                {
                    QueryType = "InsertBookingClosed",
                    UserId = User,
                    UserName = UserName,
                    ServiceId = ServiceId,
                    Services = ServiceName,
                    CategoryId = Category,
                    Category = CategoryName,
                    TypeId = Type,
                    Types = TypeName,
                    PaymentTypeId = PaymentType,
                    PaymentType = PaymentName,
                    ServiceTotal = "545",
                    ReferenceId = User.Trim(),
                    Denomination = "1",
                    DenominationCount = "6459",
                    DenominationAmount = "6459",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BookingClosedDetails", Details).Result;
                sMSG = "Inserted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);


                        if (ddlServices.SelectedValue == "1")
                        {
                            // if (ddlServices.SelectedValue == "4")
                            if (Session["Boating/CashRefund"].ToString() == "CashRefund")
                            {
                                divServiceWise.Visible = false;
                                GvServiceWise.Visible = false;
                                hdrCollection.Visible = false;

                                divCashNote.Visible = true;
                            }
                            else
                            {
                                divServiceWise.Visible = true;
                                GvServiceWise.Visible = true;
                                hdrCollection.Visible = true;

                                divCashNote.Visible = false;
                            }
                        }
                        else
                        {
                            //0911
                            if (Session["DataStatus"].ToString() == "Available")
                            {
                                //0911
                                divServiceWise.Visible = true;
                                GvServiceWise.Visible = true;
                                hdrCollection.Visible = true;
                                //0911

                            }
                            //0911
                        }
                        if (ViewState["ShowGrid"].ToString() == "1")//1011
                        {
                            hdrCollection.Visible = false;
                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;
                            btnMiniPrint.Enabled = false;
                        }
                        else
                        {
                            if (ddlPaymentType.SelectedValue == "0")
                            {
                                btnMiniPrint.Visible = true;
                                divDenomination.Visible = true;
                                //btnMiniPrint.Enabled = true;

                            }
                            else
                            {
                                btnMiniPrint.Visible = false;
                                divDenomination.Visible = false;
                            }
                            //divDenomination.Visible = true;
                            //btnMiniPrint.Visible = true;
                            //btnMiniPrint.Enabled = true;
                        }

                        btnClosed.Enabled = false;
                        //sendSMS();
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        //  btnAbstractPrint.Enabled = true;
                        if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")//1011
                        {
                            btnAbstractPrint.Enabled = true;
                        }

                        if (ddlServices.SelectedValue == "1")
                        {
                            // if (ddlServices.SelectedValue == "4")
                            if (Session["Boating/CashRefund"].ToString() == "CashRefund")
                            {
                                divServiceWise.Visible = false;
                                GvServiceWise.Visible = false;
                                hdrCollection.Visible = false;

                                divCashNote.Visible = true;
                            }
                            else
                            {
                                divServiceWise.Visible = true;
                                GvServiceWise.Visible = true;
                                hdrCollection.Visible = true;

                                divCashNote.Visible = false;
                            }
                        }

                        else
                        {
                            //0911
                            if (Session["DataStatus"].ToString() == "Available")
                            {
                                //0911
                                divServiceWise.Visible = true;
                                GvServiceWise.Visible = true;
                                hdrCollection.Visible = true;
                                //0911

                            }
                            //0911
                        }
                        if (ViewState["ShowGrid"].ToString() == "1")
                        {
                            hdrCollection.Visible = false;
                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;
                            btnMiniPrint.Enabled = false;
                        }
                        else
                        {
                            divDenomination.Visible = true;
                            btnMiniPrint.Visible = true;
                            //btnMiniPrint.Enabled = true;
                        }

                        btnDtlRpt.Enabled = true;
                        btnClosed.Enabled = false;
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void Go_Click(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        Response.Redirect("~/Reports/SWHGridView.aspx");
    }

    //Checking Grid Records//
    public void GridRecords()
    {
        //  BindServiceWise();
        string sCreatedBy = string.Empty;
        string ServiceId = string.Empty;

        try
        {
            divServiceWise.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (ddlUserName.SelectedIndex == 0)
                {
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        sCreatedBy = "0";
                    }
                    else
                    {
                        sCreatedBy = ddlUserName.SelectedValue.Trim();
                    }

                }
                else
                {
                    sCreatedBy = ddlUserName.SelectedValue.Trim();
                }
                if (ViewState["ServiceId"].ToString() == "4")
                {
                    ServiceId = "4";

                }
                else if (ViewState["ServiceId"].ToString() == "1")
                {
                    ServiceId = "1";

                }
                else
                {
                    ServiceId = ddlServices.SelectedValue.Trim();

                }
                var ChallanAb = new ServiceWise()
                {
                    UserId = sCreatedBy.Trim(),
                    ServiceId = ServiceId.Trim(),
                    CategoryId = ddlCategory.SelectedValue.Trim(),
                    TypeId = ddlTypes.SelectedValue.Trim(),
                    PaymentTypeId = ddlPaymentType.SelectedValue.Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    ToDate = txtBookingDate.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("ServiceWiseGrid", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sorry, Already Taken Mini Print !!!');", true);
                        spnote.Visible = true;
                        divDenomination.Visible = false;
                        btnMiniPrint.Visible = false;

                        btnClosed.Enabled = false;


                        if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                        {
                            //btnSubmit.Enabled = true;

                            ////btnMiniPrint.Enabled = true;                           
                            btnClosed.Visible = false;
                            spnote.Visible = true;
                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;
                            btnAbstractPrint.Enabled = false;
                            btnDtlRpt.Enabled = true;


                            if (ViewState["ShowGrid"].ToString().Trim() == "1")
                            {
                                divDenomination.Visible = false;
                                btnMiniPrint.Visible = false;
                            }
                        }
                        else
                        {
                            Page.Controls.Add(new LiteralControl("<script>alert('Sorry, Already Taken Mini Print !!!');</script>"));

                            spnote.Visible = true;
                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;
                            //if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")//1011
                            //{
                            btnAbstractPrint.Enabled = true;
                            btnDtlRpt.Enabled = true;
                            //}12 NOV
                            ////btnMiniPrint.Enabled = true;                           
                            btnClosed.Visible = true;
                            btnClosed.Enabled = false;
                        }

                    }
                    else
                    {

                        divDenomination.Visible = false;
                        btnMiniPrint.Visible = false;
                        spnote.Visible = false;
                        btnClosed.Visible = true;
                        btnMiniPrint.Visible = false;


                        if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                        {

                            btnAbstractPrint.Enabled = true;
                            btnDtlRpt.Enabled = true;
                            btnMiniPrint.Visible = true;
                            //btnMiniPrint.Enabled = true;
                            divDenomination.Visible = true;
                            divServiceWise.Visible = true;

                            btnClosed.Visible = false;

                            if (ViewState["ShowGrid"].ToString().Trim() == "1")
                            {
                                divDenomination.Visible = false;
                                btnMiniPrint.Visible = false;
                            }
                        }

                        else
                        {
                            btnClosed.Visible = true;
                            btnClosed.Enabled = true;

                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;

                            divServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            return;
                        }

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }


    // Check Amount & Closing Conditions
    /// <summary>
    /// Modified BY SILAMBARASU D 28 oct 2021
    /// </summary>

    public void CheckBoatingServiceAmountExists()
    {
        string sCreatedBy = string.Empty;
        hdrPayment.Visible = false;
        GvServiceWisePayments.Visible = false;
        tblBtCash.Visible = false;
        ViewState["btTotal"] = string.Empty;
        ViewState["btTotalPaid"] = string.Empty;
        ViewState["btTotalCashInHand"] = string.Empty;
        ViewState["btFinalCashInHand"] = string.Empty;

        try
        {
            divServiceWise.Visible = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                if (ddlUserName.SelectedIndex == 0)
                {
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        sCreatedBy = "0";
                    }
                    else
                    {
                        sCreatedBy = ddlUserName.SelectedValue.Trim();

                    }

                }
                else
                {
                    sCreatedBy = ddlUserName.SelectedValue.Trim();
                }
                var ChallanAb = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    BoatTypeId = ddlCategory.SelectedValue.Trim(),
                    PaymentType = ddlPaymentType.SelectedValue.Trim(),
                    CreatedBy = sCreatedBy.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RptServiceWiseCollection", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;

                    if (Response1.Contains("No Records Found."))
                    {
                        ViewState["ShowGrid"] = "1";
                        //btnSubmit.Enabled = false;
                        btnAbstractPrint.Enabled = false;
                        btnDtlRpt.Enabled = false;
                        btnMiniPrint.Enabled = false;
                        GvServiceWise.Visible = false;
                        hdrCollection.Visible = false;
                        divDenomination.Visible = false;
                        spnote.Visible = false;
                        tblBtCash.Visible = false;
                        btnMiniPrint.Visible = false;
                        btnClosed.Enabled = false;
                        GvServiceWise.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Response1 + "');", true);
                        return;

                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            divServiceWise.Visible = true;
                            hdrCollection.Visible = true;
                            GvServiceWise.Visible = true;

                            GvServiceWise.DataSource = dtExists;
                            GvServiceWise.DataBind();

                            divDenomination.Visible = false;
                            spnote.Visible = true;

                            int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                            decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                            GvServiceWise.FooterRow.Cells[1].Text = "Total";
                            GvServiceWise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            GvServiceWise.FooterRow.Cells[2].Text = TotalCount.ToString();
                            GvServiceWise.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                            GvServiceWise.FooterRow.Cells[3].Text = TotalAmount.ToString("N2");
                            GvServiceWise.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;


                            ViewState["btTotal"] = TotalAmount.ToString().Trim();

                            CheckServiceClosedExists();
                            lblReceivedAmount.Text = TotalAmount.ToString("N2").Trim();
                            if (ddlPaymentType.SelectedValue == "0" || ddlPaymentType.SelectedValue == "1")
                            {

                                using (var client1 = new HttpClient())
                                {
                                    client1.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                                    client1.DefaultRequestHeaders.Clear();
                                    client1.DefaultRequestHeaders.Accept.Clear();
                                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                    var ChallanAb1 = new Boating()
                                    {
                                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                        BookingDate = txtBookingDate.Text.Trim(),
                                        BoatTypeId = ddlCategory.SelectedValue.Trim(),
                                        PaymentType = ddlPaymentType.SelectedValue.Trim(),
                                        CreatedBy = ddlUserName.SelectedValue.Trim()
                                    };
                                    HttpResponseMessage response1 = client.PostAsJsonAsync("RptServiceWisePayment", ChallanAb1).Result;

                                    if (response1.IsSuccessStatusCode)
                                    {
                                        var Response = response1.Content.ReadAsStringAsync().Result;
                                        if (Response.Contains("No Records Found."))
                                        { // 02 NOV
                                            hdrPayment.Visible = false;
                                            GvServiceWisePayments.Visible = false;
                                            GvServiceWisePayments.DataBind();
                                        }
                                        else
                                        {
                                            var ResponseMsg1 = JObject.Parse(Response)["Table"].ToString();
                                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                                            if (dt.Rows.Count > 0)
                                            {

                                                GvServiceWisePayments.DataSource = dt;
                                                GvServiceWisePayments.DataBind();

                                                hdrPayment.Visible = true;
                                                GvServiceWisePayments.Visible = true;
                                                tblBtCash.Visible = true;

                                                //GvServiceWise.Visible = true;
                                                //hdrCollection.Visible = true;

                                                divServiceWise.Visible = true;
                                                divDenomination.Visible = true;
                                                btnMiniPrint.Visible = true;

                                                int TotalCountPaid = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                                                decimal TotalAmountPaid = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("Amount")));

                                                GvServiceWisePayments.FooterRow.Cells[1].Text = "Total";
                                                GvServiceWisePayments.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                                                GvServiceWisePayments.FooterRow.Cells[2].Text = TotalCountPaid.ToString();
                                                GvServiceWisePayments.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                                                GvServiceWisePayments.FooterRow.Cells[3].Text = TotalAmountPaid.ToString("N2");
                                                GvServiceWisePayments.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;


                                                ViewState["btTotalPaid"] = TotalAmountPaid.ToString().Trim();
                                                //hfServiceName.Value = ViewState["btTotalPaid"].ToString().Trim();
                                            }
                                            else
                                            {
                                                hdrPayment.Visible = false;
                                                GvServiceWisePayments.Visible = false;
                                                GvServiceWisePayments.DataBind();
                                            }
                                        }
                                    }
                                }


                                if (ViewState["btTotalPaid"].ToString().Trim() != "")
                                {
                                    lblPaidAmount.Text = (Convert.ToDecimal(ViewState["btTotalPaid"].ToString().Trim())).ToString("N2");
                                    ViewState["btTotalCashInHand"] = (Convert.ToDecimal(ViewState["btTotal"]) - Convert.ToDecimal(lblPaidAmount.Text)).ToString();
                                    // lblBal.Text = (Convert.ToDecimal(lblReceivedAmount.Text) - Convert.ToDecimal(lblPaidAmount.Text)).ToString("N2");
                                }
                                else
                                {
                                    lblPaidAmount.Text = "0.00";
                                    ViewState["btTotalCashInHand"] = ViewState["btTotal"].ToString();
                                    //lblBal.Text = (Convert.ToDecimal(lblReceivedAmount.Text)).ToString("N2");
                                }

                                lblBal.Text = ViewState["btTotalCashInHand"].ToString().Trim();

                                if (ddlPaymentType.SelectedValue == "1")
                                {
                                    ViewState["OtherPayments"] = "0";
                                }
                                if (ViewState["OtherPayments"] == null || ViewState["OtherPayments"].Equals("-1"))
                                {
                                    ViewState["OtherPayments"] = "0.00";
                                    lblCard.Text = "0";
                                    lblOnline.Text = "0";
                                    lblUPI.Text = "0";
                                }
                                decimal CashInHands = (Convert.ToDecimal(ViewState["btTotalCashInHand"].ToString())) - (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()));
                                string CashInHand = Math.Round(CashInHands, 0).ToString();

                                if ((Convert.ToDecimal(CashInHand.Trim())) < 0)
                                {
                                    hfServiceName.Value = "0";
                                }
                                else
                                {
                                    hfServiceName.Value = CashInHand.TrimEnd();
                                }

                                ViewState["btFinalCashInHand"] = hfServiceName.Value;
                                lblCashInHand.Text = (Convert.ToDecimal(CashInHand.Trim())).ToString("N2");
                                lblFinalNetAmount.Text = ((Convert.ToDecimal(CashInHand.Trim())) + (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()))).ToString("N2");

                                if (ddlPaymentType.SelectedValue == "1")
                                {
                                    tblBtCash.Visible = false;
                                }
                                else
                                {
                                    tblBtCash.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            ViewState["ShowGrid"] = "1";
                            divServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            GvServiceWise.Visible = false;
                            GvServiceWise.DataBind();
                            tblBtCash.Visible = false;
                            spnote.Visible = false;

                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found');", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    divServiceWise.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

    }

    public void CheckOtherServiceAmountExists(string sServviceApi)
    {
        string sCreatedBy = string.Empty;

        try
        {
            Session["DataStatus"] = "";
            ViewState["osTotal"] = "0";
            ViewState["rtTotal"] = "0";
            ViewState["AtTotal"] = "0";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (ddlUserName.SelectedIndex == 0)
                {
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        sCreatedBy = "0";
                    }
                    else
                    {
                        sCreatedBy = ddlUserName.SelectedValue.Trim();
                    }
                }
                else
                {
                    sCreatedBy = ddlUserName.SelectedValue.Trim();
                }
                var ChallanAb = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    BoatTypeId = ddlCategory.SelectedValue.Trim(),
                    Category = ddlTypes.SelectedValue.Trim(),
                    PaymentType = ddlPaymentType.SelectedValue.Trim(),
                    CreatedBy = sCreatedBy.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync(sServviceApi.Trim(), ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;

                    if (Response1.Contains("No Records Found."))
                    {
                        divServiceWise.Visible = false;
                        hdrCollection.Visible = false;
                        GvServiceWise.Visible = false;

                        ViewState["ShowGrid"] = "1";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Response1 + "');", true);
                        btnClosed.Enabled = false;
                        //btnSubmit.Enabled = false;
                        btnAbstractPrint.Enabled = false;
                        btnDtlRpt.Enabled = false;
                        divDenomination.Visible = false;
                        spnote.Visible = false;
                        btnMiniPrint.Visible = false;
                        GvServiceWise.DataBind();
                        tblBtCash.Visible = false;
                        return;
                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            Session["DataStatus"] = "Available";
                            GvServiceWise.DataSource = dtExists;
                            GvServiceWise.DataBind();

                            GvServiceWise.Visible = true;
                            hdrCollection.Visible = true;
                            divServiceWise.Visible = true;
                            divDenomination.Visible = false;

                            int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                            decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                            GvServiceWise.FooterRow.Cells[1].Text = "Total";
                            GvServiceWise.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            GvServiceWise.FooterRow.Cells[2].Text = TotalCount.ToString();
                            GvServiceWise.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                            GvServiceWise.FooterRow.Cells[3].Text = TotalAmount.ToString("N2");
                            GvServiceWise.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                            spnote.Visible = true;

                            //ViewState["rtTotal"] = TotalAmount.ToString().Trim();
                            //ViewState["osTotal"] = TotalAmount.ToString().Trim();
                            //ViewState["AtTotal"] = TotalAmount.ToString().Trim();
                            //hfServiceName.Value = TotalAmount.ToString().Trim();
                            lblReceivedAmount.Text = TotalAmount.ToString("N2").Trim();
                            CheckServiceClosedExists();
                            //btnSubmit.Enabled = false;      
                            if (ddlUserName.SelectedValue != "0")
                            {
                                if (ddlPaymentType.SelectedValue == "0")
                                {
                                    PaymentReceivedTypes(txtBookingDate.Text.Trim());
                                }

                            }

                            if (ddlPaymentType.SelectedValue == "1")
                            {
                                ViewState["OtherPayments"] = "0";
                            }

                            if (ViewState["OtherPayments"] == null || ViewState["OtherPayments"].Equals("-1"))
                            {
                                ViewState["OtherPayments"] = "0.00";
                                lblCard.Text = "0";
                                lblOnline.Text = "0";
                                lblUPI.Text = "0";
                            }
                            decimal CashInHands = (Convert.ToDecimal(TotalAmount.ToString().Trim())) - (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()));
                            string CashInHand = Math.Round(CashInHands, 0).ToString();

                            if ((Convert.ToDecimal(CashInHand.Trim())) < 0)
                            {
                                hfServiceName.Value = "0";
                                ViewState["rtTotal"] = hfServiceName.Value;
                                ViewState["osTotal"] = hfServiceName.Value;
                                ViewState["AtTotal"] = hfServiceName.Value;
                            }
                            else
                            {
                                hfServiceName.Value = CashInHand.Trim();
                                ViewState["rtTotal"] = hfServiceName.Value;
                                ViewState["osTotal"] = hfServiceName.Value;
                                ViewState["AtTotal"] = hfServiceName.Value;
                            }
                            lblPaidAmount.Text = "0.00";
                            lblCashInHand.Text = (Convert.ToDecimal(CashInHand.Trim())).ToString("N2");
                            lblFinalNetAmount.Text = ((Convert.ToDecimal(CashInHand.Trim())) + (Convert.ToDecimal(ViewState["OtherPayments"].ToString().Trim()))).ToString("N2");

                            if (ddlPaymentType.SelectedValue == "1")
                            {
                                tblBtCash.Visible = false;
                            }
                            else
                            {
                                tblBtCash.Visible = true;
                            }

                        }
                        else
                        {
                            ViewState["ShowGrid"] = "1";

                            GvServiceWise.DataBind();

                            GvServiceWise.Visible = false;
                            hdrCollection.Visible = false;
                            divServiceWise.Visible = false;
                            spnote.Visible = false;
                            tblBtCash.Visible = false;
                            btnClosed.Visible = false;
                            //btnSubmit.Enabled = false;
                            btnAbstractPrint.Enabled = false;
                            btnDtlRpt.Enabled = false;
                            // btnMiniPrint.Enabled = false;                           
                            divDenomination.Visible = false;
                            spnote.Visible = false;
                            btnMiniPrint.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found');", true);
                            return;

                        }
                    }
                }
                else
                {
                    divServiceWise.Visible = false;
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

    public void CheckServiceClosedExists()
    {
        string sCreatedBy = string.Empty;
        string ServiceId = string.Empty;
        string ServiceName = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (ddlUserName.SelectedIndex == 0)
                {
                    if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                    {
                        sCreatedBy = "0";
                    }
                    else
                    {
                        sCreatedBy = ddlUserName.SelectedValue.Trim();
                    }
                }
                else
                {
                    sCreatedBy = ddlUserName.SelectedValue.Trim();
                }
                if (ViewState["ServiceId"].ToString() == "4")
                {
                    ServiceId = "4";
                    ServiceName = "Cash Refund Report";
                }
                else if (ViewState["ServiceId"].ToString() == "1")
                {
                    ServiceId = "1";
                    ServiceName = "Boating";
                }
                else
                {
                    ServiceId = ddlServices.SelectedValue.Trim();
                    ServiceName = ddlServices.SelectedItem.Text.Trim();
                }

                var service = new ServiceWise()
                {

                    UserId = sCreatedBy.Trim(),
                    UserName = ddlUserName.SelectedItem.Text.Trim(),
                    ServiceId = ServiceId.Trim(),
                    Services = ServiceName.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BookingClosedService", service).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);

                        spnote.Visible = true;
                        divServiceWise.Visible = true;
                        divDenomination.Visible = true;

                        //if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")//1011
                        //{
                        btnAbstractPrint.Enabled = true;
                        btnDtlRpt.Enabled = true;
                        //}//1211
                        btnMiniPrint.Visible = true;
                        btnClosed.Visible = true;
                        btnClosed.Enabled = false;
                    }
                    else
                    {
                        spnote.Visible = false;
                        hdrCollection.Visible = true;
                        GvServiceWise.Visible = true;
                        divServiceWise.Visible = true;
                        divDenomination.Visible = true;

                        btnAbstractPrint.Enabled = false;
                        btnDtlRpt.Enabled = false;
                        btnMiniPrint.Visible = true;
                        btnClosed.Visible = true;
                        btnClosed.Enabled = true;
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ddlMonthService_SelectedIndexChanged(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        divCashNote.Visible = false;
        divMonthType.Visible = false;
        divServiceWise.Visible = false;
        GvServiceWise.Visible = false;
        hdrCollection.Visible = false;
        divDenomination.Visible = false;

        if (ddlMonthService.SelectedValue == "0")
        {
            ddlMonthCategory.Items.Clear();
            ddlMonthCategory.Items.Insert(0, new ListItem("All", "0"));
            ddlMonthTypes.Items.Clear();
            ddlMonthTypes.Items.Insert(0, new ListItem("All", "0"));
            ddlMonthPayment.SelectedIndex = 0;
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        // Boating Services

        if (ddlMonthService.SelectedValue == "1")
        {
            GetBoatType();
        }

        // Restaurant

        if (ddlMonthService.SelectedValue == "2")
        {
            getRestaurant();
            divMonthType.Visible = true;
        }

        // Other Services

        if (ddlMonthService.SelectedValue == "3")
        {
            GetOtherservices();
            divMonthType.Visible = true;
        }

        // Additional Tickets

        if (ddlMonthService.SelectedValue == "4")
        {
            GetAdditionalOtherservices();
        }
    }

    protected void ddlMonthCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (ddlMonthService.SelectedValue == "2")
        {
            GetRestaurantType();
        }

        if (ddlMonthService.SelectedValue == "3")
        {
            GetOtherServiceType();
        }
    }

    /************MONTH WISE COLLECTION**************************/

    protected void btnMonthDtlRpt_Click(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (ddlMonthService.SelectedValue == "1")
        {
            try
            {
                ReportDocument objReportDocument = new ReportDocument();
                ExportOptions objReportExport = new ExportOptions();
                DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
                string sFilePath = string.Empty;
                sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
                string rFileName = string.Empty;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response;
                        if (rbtnMonthWise.SelectedValue == "1")
                        {
                            var ChallanAb = new Boating()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                FromDate = txtFromDate.Text.Trim(),
                                ToDate = txtToDate.Text.Trim(),
                                BoatTypeId = ddlMonthCategory.SelectedValue.Trim(),
                                PaymentType = ddlMonthPayment.SelectedValue.Trim(),
                                CreatedBy = ddlMonthUserName.SelectedValue.Trim(),
                                ServiceName = "0"
                            };
                            response = client.PostAsJsonAsync("RptBoatingServiceWiseDetailed", ChallanAb).Result;
                        }
                        else
                        {
                            var ChallanAb = new Boating()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                FromDate = txtmFromDate.Text.Trim(),
                                ToDate = txtmToDate.Text.Trim(),
                                BoatTypeId = ddlMonthCategory.SelectedValue.Trim(),
                                PaymentType = ddlMonthPayment.SelectedValue.Trim(),
                                CreatedBy = ddlMonthUserName.SelectedValue.Trim(),
                                ServiceName = "0"
                            };
                            response = client.PostAsJsonAsync("RptBoatingServiceWiseDetailed", ChallanAb).Result;
                        }


                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseDetail.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Boating Service - " + ddlMonthUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtFromDate.Text.Trim() + " And " + txtToDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
                finally
                {
                    if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                    {
                        System.IO.File.Delete(Server.MapPath(sFilePath));
                    }

                    objReportDocument.Dispose();

                    objReportDiskOption = null;
                    objReportDocument = null;
                    objReportExport = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
        if (ddlMonthService.SelectedValue == "2")
        {
            try
            {

                ReportDocument objReportDocument = new ReportDocument();
                ExportOptions objReportExport = new ExportOptions();
                DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
                string sFilePath = string.Empty;
                sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
                string rFileName = string.Empty;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response;
                        if (rbtnMonthWise.SelectedValue == "1")
                        {
                            var ChallanAb = new Boating()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                FromDate = txtFromDate.Text.Trim(),
                                ToDate = txtToDate.Text.Trim(),
                                BoatTypeId = ddlMonthCategory.SelectedValue.Trim(),
                                Category = ddlMonthTypes.SelectedValue.Trim(),
                                PaymentType = ddlMonthPayment.SelectedValue.Trim(),
                                CreatedBy = ddlMonthUserName.SelectedValue.Trim(),
                                ServiceName = ""
                            };
                            response = client.PostAsJsonAsync("RptRestaurantServiceDetailed", ChallanAb).Result;
                        }
                        else
                        {
                            var ChallanAb = new Boating()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                FromDate = txtmFromDate.Text.Trim(),
                                ToDate = txtmToDate.Text.Trim(),
                                BoatTypeId = ddlMonthCategory.SelectedValue.Trim(),
                                Category = ddlMonthTypes.SelectedValue.Trim(),
                                PaymentType = ddlMonthPayment.SelectedValue.Trim(),
                                CreatedBy = ddlMonthUserName.SelectedValue.Trim(),
                                ServiceName = ""
                            };
                            response = client.PostAsJsonAsync("RptRestaurantServiceDetailed", ChallanAb).Result;
                        }

                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseDetail.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Restaurant Service - " + ddlMonthUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtFromDate.Text.Trim() + " And " + txtToDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
                finally
                {
                    if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                    {
                        System.IO.File.Delete(Server.MapPath(sFilePath));
                    }

                    objReportDocument.Dispose();

                    objReportDiskOption = null;
                    objReportDocument = null;
                    objReportExport = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
        if (ddlMonthService.SelectedValue == "3")
        {
            try
            {
                ReportDocument objReportDocument = new ReportDocument();
                ExportOptions objReportExport = new ExportOptions();
                DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
                string sFilePath = string.Empty;
                sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
                string rFileName = string.Empty;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response;
                        if (rbtnMonthWise.SelectedValue == "1")
                        {
                            var ChallanAb = new Boating()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                FromDate = txtFromDate.Text.Trim(),
                                ToDate = txtToDate.Text.Trim(),
                                BoatTypeId = ddlMonthCategory.SelectedValue.Trim(),
                                Category = ddlMonthTypes.SelectedValue.Trim(),
                                PaymentType = ddlMonthPayment.SelectedValue.Trim(),
                                CreatedBy = ddlMonthUserName.SelectedValue.Trim(),
                                ServiceName = ""
                            };
                            response = client.PostAsJsonAsync("RptOtherServiceDetailed", ChallanAb).Result;
                        }
                        else
                        {
                            var ChallanAb = new Boating()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                FromDate = txtmFromDate.Text.Trim(),
                                ToDate = txtmToDate.Text.Trim(),
                                BoatTypeId = ddlMonthCategory.SelectedValue.Trim(),
                                Category = ddlMonthTypes.SelectedValue.Trim(),
                                PaymentType = ddlMonthPayment.SelectedValue.Trim(),
                                CreatedBy = ddlMonthUserName.SelectedValue.Trim(),
                                ServiceName = ""
                            };
                            response = client.PostAsJsonAsync("RptOtherServiceDetailed", ChallanAb).Result;
                        }

                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseDetail.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Other Service - " + ddlMonthUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtFromDate.Text.Trim() + " And " + txtToDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
                finally
                {
                    if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                    {
                        System.IO.File.Delete(Server.MapPath(sFilePath));
                    }

                    objReportDocument.Dispose();

                    objReportDiskOption = null;
                    objReportDocument = null;
                    objReportExport = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
        if (ddlMonthService.SelectedValue == "4")
        {
            try
            {
                ReportDocument objReportDocument = new ReportDocument();
                ExportOptions objReportExport = new ExportOptions();
                DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
                string sFilePath = string.Empty;
                sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
                string rFileName = string.Empty;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response;
                        if (rbtnMonthWise.SelectedValue == "1")
                        {
                            var ChallanAb = new Boating()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                FromDate = txtFromDate.Text.Trim(),
                                ToDate = txtToDate.Text.Trim(),
                                BoatTypeId = ddlMonthCategory.SelectedValue.Trim(),
                                Category = ddlMonthTypes.SelectedValue.Trim(),
                                PaymentType = ddlMonthPayment.SelectedValue.Trim(),
                                CreatedBy = ddlMonthUserName.SelectedValue.Trim(),
                                ServiceName = ""
                            };
                            response = client.PostAsJsonAsync("RptOtherServiceAdditionalDtl", ChallanAb).Result;
                        }
                        else
                        {
                            var ChallanAb = new Boating()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                FromDate = txtmFromDate.Text.Trim(),
                                ToDate = txtmToDate.Text.Trim(),
                                BoatTypeId = ddlMonthCategory.SelectedValue.Trim(),
                                Category = ddlMonthTypes.SelectedValue.Trim(),
                                PaymentType = ddlMonthPayment.SelectedValue.Trim(),
                                CreatedBy = ddlMonthUserName.SelectedValue.Trim(),
                                ServiceName = ""
                            };
                            response = client.PostAsJsonAsync("RptOtherServiceAdditionalDtl", ChallanAb).Result;
                        }

                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptServiceWiseDetail.rpt"));
                                objReportDocument.SetDataSource(dtExists);

                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Additional Charges - " + ddlMonthUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + txtFromDate.Text.Trim() + " And " + txtToDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                                objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                                objReportExport = objReportDocument.ExportOptions;
                                objReportExport.ExportDestinationOptions = objReportDiskOption;
                                objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                                objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                                objReportDocument.Export();
                                Response.ClearContent();
                                Response.ClearHeaders();
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(Server.MapPath(sFilePath));
                                Response.Flush();
                                Response.Close();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
                finally
                {
                    if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
                    {
                        System.IO.File.Delete(Server.MapPath(sFilePath));
                    }

                    objReportDocument.Dispose();

                    objReportDiskOption = null;
                    objReportDocument = null;
                    objReportExport = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
    }

    public void GetFinYear()
    {
        try
        {
            ddlFinYear.Items.Clear();
            int iCurrentYear = 0, iFinYear = 0;
            string sFinYear = string.Empty;

            iCurrentYear = System.DateTime.Now.Year;

            if (DateTime.Now.Month >= 4)
            {
                iFinYear = iCurrentYear;
            }
            else
            {
                iFinYear = iCurrentYear - 1;
            }
            ddlFinYear.Items.Insert(0, new ListItem("Select", "0"));
            for (int iCount = 1; iCount <= 15; iCount++)
            {
                sFinYear = iFinYear.ToString() + "-" + (iFinYear + 1).ToString();
                iFinYear = iFinYear - 1;

                ddlFinYear.Items.Add(sFinYear);
               
                if (sFinYear == "2020-2021")
                {
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            Page.Controls.Add(new LiteralControl("<script>alert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');</script>"));
            return;
        }
    }

    protected void rbtnMonthWise_SelectedIndexChanged(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (rbtnMonthWise.SelectedValue == "1")
        {
            ClearMonthWise();
            divDateWiseRpt.Visible = true;
            divMonthWiseRpt.Visible = false;
        }
        else
        {
            ClearDateWise();
            divDateWiseRpt.Visible = false;
            divMonthWiseRpt.Visible = true;
            GetFinYear();
        }
    }

    protected void ddlFinYear_SelectedIndexChanged(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        if (ddlFinYear.SelectedValue != "0")
        {
            divMonth.Visible = true;
            int sMonth = Convert.ToInt32(ddlMonth.SelectedValue);
            string[] year = ddlFinYear.SelectedValue.Split('-');
            int days = 0;
            txtmFromDate.Attributes.Add("readonly", "readonly");
            txtmToDate.Attributes.Add("readonly", "readonly");
            if (sMonth >= 4 && sMonth <= 12)
            {
                days = DateTime.DaysInMonth(Convert.ToInt32(year[0]), sMonth);
                txtmFromDate.Text = "01/" + ddlMonth.SelectedValue.Trim() + "/" + year[0];
                txtmToDate.Text = days + "/" + ddlMonth.SelectedValue.Trim() + "/" + year[0];
            }
            else
            {
                days = DateTime.DaysInMonth(Convert.ToInt32(year[1]), sMonth);
                txtmFromDate.Text = "01/" + ddlMonth.SelectedValue.Trim() + "/" + year[1];
                txtmToDate.Text = days + "/" + ddlMonth.SelectedValue.Trim() + "/" + year[1];
            }
        }
        else
        {
            ddlMonth.SelectedValue = "04";
            divMonth.Visible = false;
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
          MpeTrip.Dispose();pnlTrip.Visible = false;
        int sMonth = Convert.ToInt32(ddlMonth.SelectedValue);
        string[] year = ddlFinYear.SelectedValue.Split('-');
        int days = 0;
        txtmFromDate.Attributes.Add("readonly", "readonly");
        txtmToDate.Attributes.Add("readonly", "readonly");
        if (sMonth >= 4 && sMonth <= 12)
        {
            days = DateTime.DaysInMonth(Convert.ToInt32(year[0]), sMonth);
            txtmFromDate.Text = "01/" + ddlMonth.SelectedValue.Trim() + "/" + year[0];
            txtmToDate.Text = days + "/" + ddlMonth.SelectedValue.Trim() + "/" + year[0];
        }
        else
        {
            days = DateTime.DaysInMonth(Convert.ToInt32(year[1]), sMonth);
            txtmFromDate.Text = "01/" + ddlMonth.SelectedValue.Trim() + "/" + year[1];
            txtmToDate.Text = days + "/" + ddlMonth.SelectedValue.Trim() + "/" + year[1];
        }
    }

    public void ClearMonthWise()
    {
        GetFinYear();
        ddlMonth.SelectedValue = "04";
        divMonth.Visible = false;
        txtmFromDate.Text = string.Empty;
        txtmToDate.Text = string.Empty;
    }

    public void ClearDateWise()
    {
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtFromDate.Attributes.Add("readonly", "readonly");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Attributes.Add("readonly", "readonly");
    }

    /************RESTAURANT WISE COLLECTION**************************/
    protected void btnResDtlRpt_Click(object sender, EventArgs e)
    {
            MpeTrip.Dispose();pnlTrip.Visible = false;
        CategoryWiseDtlRpt();
    }
    //public void CategoryWiseDtlRpt()
    //{
    //    ReportDocument objReportDocument = new ReportDocument();
    //    ExportOptions objReportExport = new ExportOptions();
    //    DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
    //    string sFilePath = string.Empty;
    //    sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
    //    string rFileName = string.Empty;
    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            string sCreatedBy = string.Empty;
    //            if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
    //            {
    //                sCreatedBy = "0";
    //            }
    //            else
    //            {
    //                sCreatedBy = Session["UserId"].ToString().Trim();
    //            }

    //            var ChallanAb = new Boating()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
    //                FromDate = txtResFromDate.Text.Trim(),
    //                ToDate = txtResToDate.Text.Trim(),
    //                CreatedBy = sCreatedBy
    //            };
    //            HttpResponseMessage response = client.PostAsJsonAsync("RptResDetailedCatWiseCollection", ChallanAb).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var Response1 = response.Content.ReadAsStringAsync().Result;
    //                var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
    //                DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

    //                if (dtExists.Rows.Count > 0)
    //                {
    //                    objReportDocument.Load(Server.MapPath("RptRestaurantWiseDetail.rpt"));
    //                    objReportDocument.SetDataSource(dtExists);

    //                    objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString().Trim());
    //                    objReportDocument.SetParameterValue(2, "Restaurant Detailed Sales Entries details Between " + txtResFromDate.Text.Trim() + " And " + txtResToDate.Text.Trim());
    //                    objReportDocument.SetParameterValue(1, txtResFromDate.Text);

    //                    objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
    //                    objReportExport = objReportDocument.ExportOptions;
    //                    objReportExport.ExportDestinationOptions = objReportDiskOption;
    //                    objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
    //                    objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
    //                    objReportDocument.Export();
    //                    Response.ClearContent();
    //                    Response.ClearHeaders();
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(Server.MapPath(sFilePath));
    //                    Response.Flush();
    //                    Response.Close();
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
    //                }
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
    //        return;
    //    }
    //    finally
    //    {
    //        if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
    //        {
    //            System.IO.File.Delete(Server.MapPath(sFilePath));
    //        }

    //        objReportDocument.Dispose();

    //        objReportDiskOption = null;
    //        objReportDocument = null;
    //        objReportExport = null;
    //        GC.Collect();
    //    }
    //}

    /// <summary>
    /// Changed By abhinaya
    /// Changed Date 30-dec-2021
    /// </summary>
    public void CategoryWiseDtlRpt()
    {
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sCreatedBy = string.Empty;
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    sCreatedBy = "0";
                }
                else
                {
                    sCreatedBy = Session["UserId"].ToString().Trim();
                }

                var ChallanAb = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    FromDate = txtResFromDate.Text.Trim(),
                    ToDate = txtResToDate.Text.Trim(),
                    CreatedBy = sCreatedBy
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RestaurantCatWiseReport", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;


                    if (Response1.Contains("No Records Found."))
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Response1 + "');", true);
                        return;
                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table1"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dtExists.Rows.Count > 0)
                        {
                            objReportDocument.Load(Server.MapPath("RptRestaurantWiseDetail.rpt"));
                            objReportDocument.SetDataSource(dtExists);

                            objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString().Trim());
                            objReportDocument.SetParameterValue(2, "Restaurant Detailed Sales Entries details Between " + txtResFromDate.Text.Trim() + " And " + txtResToDate.Text.Trim());
                            objReportDocument.SetParameterValue(1, txtResFromDate.Text);
                            objReportDocument.SetParameterValue(7, Session["CorpName"].ToString());
                            objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                            objReportExport = objReportDocument.ExportOptions;
                            objReportExport.ExportDestinationOptions = objReportDiskOption;
                            objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                            objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                            objReportDocument.Export();
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.ContentType = "application/pdf";
                            Response.WriteFile(Server.MapPath(sFilePath));
                            Response.Flush();
                            Response.Close();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                        }

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
        finally
        {
            if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
            {
                System.IO.File.Delete(Server.MapPath(sFilePath));
            }

            objReportDocument.Dispose();

            objReportDiskOption = null;
            objReportDocument = null;
            objReportExport = null;
            GC.Collect();
        }
    }

   

    public void ClearResMonthWise()
    {
        GetResFinYear();
        ddlMonth.SelectedValue = "04";
        divResMonth.Visible = false;
        txtResMonthFromDate.Text = string.Empty;
        txtResMonthToDate.Text = string.Empty;
    }

    public void ClearResDateWise()
    {
        txtResFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtResFromDate.Attributes.Add("readonly", "readonly");
        txtResToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtResToDate.Attributes.Add("readonly", "readonly");
    }

    public void GetResFinYear()
    {
        try
        {
            ddlResFinYear.Items.Clear();
            int iCurrentYear = 0, iFinYear = 0;
            string sFinYear = string.Empty;

            iCurrentYear = System.DateTime.Now.Year;

            if (DateTime.Now.Month >= 4)
            {
                iFinYear = iCurrentYear;
            }
            else
            {
                iFinYear = iCurrentYear - 1;
            }
            ddlResFinYear.Items.Insert(0, new ListItem("Select", "0"));
            for (int iCount = 1; iCount <= 15; iCount++)
            {
                sFinYear = iFinYear.ToString() + "-" + (iFinYear + 1).ToString();
                iFinYear = iFinYear - 1;

                ddlResFinYear.Items.Add(sFinYear);
              
                if (sFinYear == "2020-2021")
                {
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            Page.Controls.Add(new LiteralControl("<script>alert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');</script>"));
            return;
        }
    }

    protected void rbtnResMonthWise_SelectedIndexChanged(object sender, EventArgs e)
    {
            MpeTrip.Dispose();pnlTrip.Visible = false;
        if (rbtnResMonthWise.SelectedValue == "1")
        {
            ClearResMonthWise();
            divResDateWise.Visible = true;
            divResMonthwise.Visible = false;
        }
        else
        {
            ClearResDateWise();
            divResDateWise.Visible = false;
            divResMonthwise.Visible = true;
            GetResFinYear();
        }
    }

    protected void btnResCategory_Click(object sender, EventArgs e)
    {
            MpeTrip.Dispose();pnlTrip.Visible = false;
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sCreatedBy = string.Empty;
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    sCreatedBy = "0";
                }
                else
                {
                    sCreatedBy = Session["UserId"].ToString().Trim();
                }

                DateTime dFromDate = Convert.ToDateTime(txtResMonthFromDate.Text.Trim(), objEnglishDate);
                DateTime dToDate = dFromDate.AddMonths(1).AddDays(-1);

                var ChallanAb = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),                   
                    FromDate = dFromDate.ToString("dd/MM/yyyy"),
                    ToDate = dToDate.ToString("dd/MM/yyyy"),
                    CreatedBy = sCreatedBy
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RptResDetailedCatWiseCollection", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        objReportDocument.Load(Server.MapPath("RptRestaurantWiseDetail.rpt"));
                        objReportDocument.SetDataSource(dtExists);

                        objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString().Trim());
                        objReportDocument.SetParameterValue(2, "Restaurant Detailed Sales Entries details Between " + txtResMonthFromDate.Text.Trim() + " And " + txtResMonthToDate.Text.Trim());
                        objReportDocument.SetParameterValue(1, txtResMonthFromDate.Text);
                        objReportDocument.SetParameterValue(7, Session["CorpName"].ToString());

                        objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                        objReportExport = objReportDocument.ExportOptions;
                        objReportExport.ExportDestinationOptions = objReportDiskOption;
                        objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                        objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                        objReportDocument.Export();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(Server.MapPath(sFilePath));
                        Response.Flush();
                        Response.Close();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
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
        finally
        {
            if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
            {
                System.IO.File.Delete(Server.MapPath(sFilePath));
            }

            objReportDocument.Dispose();

            objReportDiskOption = null;
            objReportDocument = null;
            objReportExport = null;
            GC.Collect();
        }
    }

    protected void ddlResFinYear_SelectedIndexChanged(object sender, EventArgs e)
    {
            MpeTrip.Dispose();pnlTrip.Visible = false;
        if (ddlResFinYear.SelectedValue != "0")
        {
            divResMonth.Visible = true;
            int sMonth = Convert.ToInt32(ddlResMonth.SelectedValue);
            string[] year = ddlResFinYear.SelectedValue.Split('-');
            int days = 0;

            txtResMonthFromDate.Attributes.Add("readonly", "readonly");
            txtResMonthToDate.Attributes.Add("readonly", "readonly");

            if (sMonth >= 4 && sMonth <= 12)
            {
                days = DateTime.DaysInMonth(Convert.ToInt32(year[0]), sMonth);
                txtResMonthFromDate.Text = "01/" + ddlResMonth.SelectedValue.Trim() + "/" + year[0];
                txtResMonthToDate.Text = days + "/" + ddlResMonth.SelectedValue.Trim() + "/" + year[0];
            }
            else
            {
                days = DateTime.DaysInMonth(Convert.ToInt32(year[1]), sMonth);
                txtResMonthFromDate.Text = "01/" + ddlResMonth.SelectedValue.Trim() + "/" + year[1];
                txtResMonthToDate.Text = days + "/" + ddlResMonth.SelectedValue.Trim() + "/" + year[1];
            }
        }
        else
        {
            ddlResMonth.SelectedValue = "04";
            divResMonth.Visible = false;
        }
    }

    protected void ddlResMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
            MpeTrip.Dispose();pnlTrip.Visible = false;
        int sMonth = Convert.ToInt32(ddlResMonth.SelectedValue);
        string[] year = ddlResFinYear.SelectedValue.Split('-');
        int days = 0;
        txtResMonthFromDate.Attributes.Add("readonly", "readonly");
        txtResMonthToDate.Attributes.Add("readonly", "readonly");
        if (sMonth >= 4 && sMonth <= 12)
        {
            days = DateTime.DaysInMonth(Convert.ToInt32(year[0]), sMonth);
            txtResMonthFromDate.Text = "01/" + ddlResMonth.SelectedValue.Trim() + "/" + year[0];
            txtResToDate.Text = days + "/" + ddlResMonth.SelectedValue.Trim() + "/" + year[0];
        }
        else
        {
            days = DateTime.DaysInMonth(Convert.ToInt32(year[1]), sMonth);
            txtResMonthFromDate.Text = "01/" + ddlResMonth.SelectedValue.Trim() + "/" + year[1];
            txtResMonthToDate.Text = days + "/" + ddlResMonth.SelectedValue.Trim() + "/" + year[1];
        }
    }

    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
            MpeTrip.Dispose();pnlTrip.Visible = false;
        if (ddlServices.SelectedIndex != 0)
        {
            ddlUserName.SelectedIndex = 0;
        }
        divServiceTypeStatus.Visible = false;
        if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
        {
            ddlUserName.Items.Clear();
            ddlUserName.Items.Insert(0, new ListItem("All", "0"));
        }
        ddlServices.SelectedIndex = 0;
        divCashNote.Visible = false;
        divDenomination.Visible = false;
        divServiceWise.Visible = false;
        GvServiceWise.Visible = false;
        hdrCollection.Visible = false;
        ddlCategory.Items.Clear();
        ddlCategory.Items.Insert(0, new ListItem("All", "0"));
        ddlTypes.Items.Clear();
        ddlTypes.Items.Insert(0, new ListItem("All", "0"));
        divType.Visible = false;
        btnMiniPrint.Visible = false;
        txt2000.Text = string.Empty;
        txt500.Text = string.Empty;
        txt200.Text = string.Empty;
        txt100.Text = string.Empty;
        txt50.Text = string.Empty;
        txt20.Text = string.Empty;
        txt10.Text = string.Empty;
        txt5.Text = string.Empty;
        txt2.Text = string.Empty;
        txt1.Text = string.Empty;
        spnote.Visible = false;

    }

    protected void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
            MpeTrip.Dispose();pnlTrip.Visible = false;
        BindServiceWise();
        btnClosed.Visible = false;
        divDenomination.Visible = false;
        btnMiniPrint.Visible = false;
        if (ddlUserName.SelectedItem.Text == "All")
        {
            divServiceWise.Visible = false;
            btnMiniPrint.Visible = false;
            divDenomination.Visible = false;
        }
    }


    /************VIEW AUTO NOT ENDED TRIP**************************/
    public void GetAutoEndForNoDeposite()
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

                var body = new CommonClass()
                {
                    QueryType = "GetAutoEndForNoDeposite",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CommonReport", body).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["AutoEndForNoDeposite"] = dtExists.Rows[0]["AutoEndForNoDeposite"].ToString();

                        if (ViewState["AutoEndForNoDeposite"].ToString() == "Y")
                        {
                            btnautoend.Visible = true;

                        }

                        else
                        {
                            btnautoend.Visible = false;
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
    protected void btnautoend_Click(object sender, EventArgs e)
    {
        MpeTrip.Show();
        pnlTrip.Visible = true;
        TripSheetTripEndAll();
    }

    public void TripSheetTripEndAll()
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
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("TripSheetweb/ListAllTripEndDefault", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripEndDefault", vTripSheetSettlement).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ViewState["Count"] = dt.Rows.Count;
                        if (dt.Rows.Count > 0)
                        {
                            gvTripSheetSettelementEndAll.DataSource = dt;
                            gvTripSheetSettelementEndAll.DataBind();
                            divGridEndAll.Visible = true;
                            gvTripSheetSettelementEndAll.Visible = true;
                            divmsgEndAll.Visible = false;
                            btnAllEnd.Visible = true;

                        }
                        else
                        {
                            gvTripSheetSettelementEndAll.DataSource = dt;
                            gvTripSheetSettelementEndAll.DataBind();
                            gvTripSheetSettelementEndAll.Visible = false;
                            lblGridMsgEndAll.Text = ResponseMsg;
                            divmsgEndAll.Visible = true;
                            btnAllEnd.Visible = false;

                        }
                    }
                    else
                    {
                        gvTripSheetSettelementEndAll.Visible = false;
                        divGridEndAll.Visible = true;
                        lblGridMsgEndAll.Text = ResponseMsg;
                        divmsgEndAll.Visible = true;
                        btnAllEnd.Visible = false;
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
    protected void gvTripSheetSettelementEndAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {

                    Label lblBookingId = (Label)e.Row.FindControl("lblBookingId");
                    Label lblBoatReferenceNo = (Label)e.Row.FindControl("lblBoatReferenceNo");
                    Label UserId = (Label)e.Row.FindControl("lblUserId");
                    Label BoardingTime = (Label)e.Row.FindControl("lblBoardingTime");
                    Label PremiumStatus = (Label)e.Row.FindControl("lblPremiumStatus");

                    Label BookingDuration = (Label)e.Row.FindControl("lblBookingDuration");
                    Label BoatNum = (Label)e.Row.FindControl("lblBoatNum");
                    Label BookingPin = (Label)e.Row.FindControl("lblBookingPin");
                    Label lblBoatTypeId = (Label)e.Row.FindControl("lblBoatTypeId");
                    Label lblBoatType = (Label)e.Row.FindControl("lblBoatType");

                    Label lblBoatSeaterId = (Label)e.Row.FindControl("lblBoatSeaterId");
                    Label lblBoatSeater = (Label)e.Row.FindControl("lblBoatSeater");
                    Label lblBoatId = (Label)e.Row.FindControl("lblBoatId");
                    Label lblTripStartTime = (Label)e.Row.FindControl("lblTripStartTime");
                    //Label lblEndTripTime = (Label)e.Row.FindControl("lblEndTripTime");
                    Label lblBookingDuration = (Label)e.Row.FindControl("lblBookingDuration");

                    double Duration = 0;
                    Duration = Math.Round(Convert.ToDouble(lblBookingDuration.Text) / 2);
                    string Time = DateTime.Now.ToString("HH:mm");
                    DateTime d = DateTime.Parse(lblTripStartTime.Text);
                    DateTime now = DateTime.Parse(d.ToString("HH:mm"));
                    DateTime modifiedDatetime = now.AddMinutes(Duration);


                }
            }
        }
    }

    protected void btnAllEnd_Click(object sender, EventArgs e)
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
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        SDUserBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetWeb/UpdateEndDeafult", vTripSheetSettlement).Result;

                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        SDUserBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetWeb/UpdateEndDeafult", vTripSheetSettlement).Result;

                }


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        TripSheetTripEndAll();
                    }
                    else
                    {
                        TripSheetTripEndAll();

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

    protected void imgCloseTicket_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void gvTripSheetSettelementEndAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        MpeTrip.Show();
        gvTripSheetSettelementEndAll.PageIndex = e.NewPageIndex;
        TripSheetTripEndAll();
    }
    public class ServiceWise
    {
        public string UserId { get; set; }
        public string ReferenceId { get; set; }
        public string Denomination { get; set; }
        public string DenominationCount { get; set; }
        public string DenominationAmount { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string UserName { get; set; }
        public string Services { get; set; }
        public string Category { get; set; }
        public string Types { get; set; }
        public string PaymentType { get; set; }
        public string BookingDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceId { get; set; }
        public string CategoryId { get; set; }
        public string TypeId { get; set; }
        public string PaymentTypeId { get; set; }
        public string ServiceTotal { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string QueryType { get; set; }

    }


    public class CashReport
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string CashflowTypes { get; set; }
        public string ToDate { get; set; }
        public string QueryType { get; set; }

        public string ServiceType { get; set; }
        public string BookingId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string RequestedBy { get; set; }

    }


    public class Boating
    {
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BookingDate { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatType { get; set; }
        public string Account { get; set; }
        public string Amount { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string TaxableAmount { get; set; }
        public string Category { get; set; }
        public string PaymentType { get; set; }
        public string CreatedBy { get; set; }
        public string ServiceName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class TripSheetSettlement
    {
        public string QueryType { get; set; }
        public string BoatReferenceNo { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatHouseName { get; set; }
        public string RowerId { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string ActualBoatId { get; set; }
        public string BoatSeaterId { get; set; }
        public string BookingId { get; set; }
        public string BookingMedia { get; set; }
        public string UserId { get; set; }
        public string SSUserBy { get; set; }
        public string SDUserBy { get; set; }
        public string BarcodePin { get; set; }
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
    public void GetMonthWiseUserName()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ChallanAb = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("ddlUserName", ChallanAb).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlUserName.DataSource = dtExists;
                        ddlUserName.DataValueField = "UserId";
                        ddlUserName.DataTextField = "UserName";
                        ddlUserName.DataBind();

                        ddlMonthUserName.DataSource = dtExists;
                        ddlMonthUserName.DataValueField = "UserId";
                        ddlMonthUserName.DataTextField = "UserName";
                        ddlMonthUserName.DataBind();
                    }
                    else
                    {
                        ddlUserName.DataBind();
                        ddlMonthUserName.DataBind();
                    }
                    ddlUserName.Items.Insert(0, new ListItem("All", "0"));
                    ddlMonthUserName.Items.Insert(0, new ListItem("All", "0"));
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

    public void GetUserDenominationStatus(string FromDate)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate,
                    ToDate = FromDate,
                    QueryType = "GetUserDenominationStatus",
                    ServiceType = "",
                    BookingId = "",
                    Input1 = ddlUserName.SelectedValue.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        divCashNote.Visible = false;
                        divDenomination.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);

                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                        if (dtExists.Rows.Count > 0)
                        {
                            GvServiceTypeStatus.DataSource = dtExists;
                            GvServiceTypeStatus.DataBind();
                            GvServiceTypeStatus.Visible = true;
                            divServiceTypeStatus.Visible = true;
                        }
                        else
                        {
                            GvServiceTypeStatus.DataBind();
                            GvServiceTypeStatus.Visible = false;
                            hdrServiceTypeStatus.Visible = false;
                            divServiceTypeStatus.Visible = false;

                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Record Found');", true);
                    divServiceTypeStatus.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void CheckServiceClosedStatus(string FromDate)
    {
        try
        {
            string ServiceId = string.Empty;

            if (ViewState["ServiceId"].ToString() == "4")
            {
                ServiceId = "4";

            }
            else if (ViewState["ServiceId"].ToString() == "1")
            {
                ServiceId = "1";
            }
            else
            {
                ServiceId = ddlServices.SelectedValue.Trim();

            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate,
                    ToDate = FromDate,
                    QueryType = "CheckClosed/Service",
                    ServiceType = ServiceId.Trim(),
                    BookingId = "",
                    Input1 = ddlUserName.SelectedValue.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        //divCashNote.Visible = false;
                        //divDenomination.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);

                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();//BookingClosedDetails
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        var ResponseMsg1 = JObject.Parse(Response1)["Table1"].ToString();//ServiceWiseHistory
                        DataTable dtExists1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                        if (dtExists.Rows.Count > 0 || dtExists1.Rows.Count > 0) // && dtExists1.Rows.Count == 0
                        {
                            btnClosed.Enabled = false;
                        }
                        else
                        {
                            btnClosed.Enabled = true;
                        }

                        if (dtExists1.Rows.Count > 0 && btnClosed.Enabled == false)
                        {
                            spnote.Visible = true;
                            divDenomination.Visible = false;                           
                            btnMiniPrint.Visible = false;
                        }
                        else if (btnClosed.Enabled == true)
                        {
                            divCashNote.Visible = false;
                            divServiceWise.Visible = false;
                            divDenomination.Visible = false;
                            btnMiniPrint.Visible = false;
                        }
                        else
                        {
                            if (ViewState["ServiceId"].ToString() == "4")
                            {
                                divCashNote.Visible = true;
                            }
                            else
                            {
                                divServiceWise.Visible = true;
                            }

                            if (ViewState["ShowGrid"].ToString() == "1")
                            {
                                divDenomination.Visible = false;
                                btnMiniPrint.Visible = false;
                                btnMiniPrint.Enabled = false;
                            }
                            else
                            {
                                divDenomination.Visible = true;
                                btnMiniPrint.Visible = true;
                                //btnMiniPrint.Enabled = true;
                            }

                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Record Found');", true);
                    divServiceTypeStatus.Visible = false;
                }
            }
            if (ddlPaymentType.SelectedValue == "0")
            {
                if (btnMiniPrint.Visible == true)
                {
                    btnMiniPrint.Visible = true;
                }
                else
                {
                    btnMiniPrint.Visible = false;
                }
                if (divDenomination.Visible == true)
                {
                    divDenomination.Visible = true;
                }
                else
                {
                    divDenomination.Visible = false;
                }

            }
            else
            {
                btnMiniPrint.Visible = false;
                divDenomination.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    /// <summary>
    /// Boating/Cash refund
    /// </summary>
    /// <param name="FromDate"></param>
    public void CheckServiceClosedAdmin(string FromDate)
    {
        try
        {
            string ServiceId = string.Empty;

            if (ViewState["ServiceId"].ToString() == "4")
            {
                ServiceId = "4";

            }
            else if (ViewState["ServiceId"].ToString() == "1")
            {
                ServiceId = "1";
            }
            else
            {
                ServiceId = ddlServices.SelectedValue.Trim();

            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate,
                    ToDate = FromDate,
                    QueryType = "CheckClosed/Service",
                    ServiceType = ServiceId.Trim(),
                    BookingId = "",
                    Input1 = ddlUserName.SelectedValue.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        //divCashNote.Visible = false;
                        //divDenomination.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);

                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();//BookingClosedDetails
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        var ResponseMsg1 = JObject.Parse(Response1)["Table1"].ToString();//ServiceWiseHistory
                        DataTable dtExists1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                        if (dtExists.Rows.Count > 0 && dtExists1.Rows.Count > 0)
                        {
                            spnote.Visible = true;
                            divDenomination.Visible = false;
                            divCashNote.Visible = false;
                            btnMiniPrint.Visible = false;
                        }
                        else if (dtExists1.Rows.Count > 0)
                        {
                            spnote.Visible = true;
                            divDenomination.Visible = false;
                            divCashNote.Visible = false;
                            btnMiniPrint.Visible = false;
                        }
                        else
                        {
                            spnote.Visible = false;
                            if (ViewState["ServiceId"].ToString() == "4")
                            {
                                if (ddlPaymentType.SelectedValue == "0" || ddlPaymentType.SelectedValue == "1")
                                {
                                    divCashNote.Visible = true;
                                }
                                else
                                {
                                    divCashNote.Visible = false;
                                }

                            }
                            else
                            {
                                divServiceWise.Visible = true;
                            }
                            if (ViewState["ShowGrid"].ToString() == "1")
                            {
                                divDenomination.Visible = false;
                                btnMiniPrint.Visible = false;
                                btnMiniPrint.Enabled = false;
                            }
                            else
                            {
                                divDenomination.Visible = true;
                                btnMiniPrint.Visible = true;
                                //btnMiniPrint.Enabled = true;
                            }

                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Record Found');", true);
                    divServiceTypeStatus.Visible = false;
                }
            }
            if (ddlPaymentType.SelectedValue == "0")
            {
                if (btnMiniPrint.Visible == true)
                {
                    btnMiniPrint.Visible = true;
                }
                else
                {
                    btnMiniPrint.Visible = false;
                }
                if (divDenomination.Visible == true)
                {
                    divDenomination.Visible = true;
                }
                else
                {
                    divDenomination.Visible = false;
                }

            }
            else
            {
                btnMiniPrint.Visible = false;
                divDenomination.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /// <summary>
    /// OtherTypeOfpayments
    /// </summary>
    /// <param name="FromDate"></param>
    public void PaymentReceivedTypes(string FromDate)
    {
        try
        {
            string User = string.Empty;
            string UserName = string.Empty;
            string ServiceId = string.Empty;
            string ServiceType = string.Empty;
            if (ddlServices.SelectedIndex != 0)
            {
                ServiceId = ddlServices.SelectedValue.Trim();
                ServiceType = ddlServices.SelectedItem.Text;
            }
            if (ddlUserName.SelectedValue == "0")
            {
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    User = "0";
                    UserName = "All";
                }
                else
                {
                    User = ddlUserName.SelectedValue.Trim();
                    UserName = ddlUserName.SelectedItem.Text;
                }

            }
            else
            {
                User = ddlUserName.SelectedValue.Trim();
                UserName = ddlUserName.SelectedItem.Text.Trim();
            }
            ViewState["OtherPayments"] = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CashReport = new CashReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate,
                    ToDate = FromDate,
                    QueryType = "PaymentReceivedTypes",
                    ServiceType = ServiceType.Trim(),
                    BookingId = "",
                    Input1 = User.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", CashReport).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);
                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            lblCard.Text = dtExists.Rows[0]["Amount"].ToString();
                            lblOnline.Text = dtExists.Rows[1]["Amount"].ToString();
                            lblUPI.Text = dtExists.Rows[2]["Amount"].ToString();

                            ViewState["OtherPayments"] = (Convert.ToDecimal(lblCard.Text) + Convert.ToDecimal(lblOnline.Text) + Convert.ToDecimal(lblUPI.Text)).ToString();
                        }
                        else
                        {
                            lblCard.Text = "0";
                            lblOnline.Text = "0";
                            lblUPI.Text = "0";
                            ViewState["OtherPayments"] = "0.00";
                        }

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Record Found');", true);
                    divServiceTypeStatus.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void txtBookingDate_TextChanged(object sender, EventArgs e)
    {
        if(txtBookingDate.Text != "")
        {
            GetTripCount();
        }

    }
}

