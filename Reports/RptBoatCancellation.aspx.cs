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

public partial class Reports_RptBoatCancellation : System.Web.UI.Page
{
    public IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);
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
                GetBoatType();
                GetPaymentType();

                //Newly Added
                ViewState["Flag"] = "B";
                ViewState["hfstartvalue"] = "0";
                ViewState["hfendvalue"] = "0";
                int istart;
                int iend;
                AddProcess(0, 10, out istart, out iend);
                BindCancelBookingDetails();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public class CancelMstr
    {

        public string BoatHouseId
        {
            get;
            set;
        }
        public int BookingId
        {
            get;
            set;
        }
        public string MobileNo
        {
            get;
            set;
        }
        public string BookingDate
        {
            get;
            set;
        }
        public string PaymentType
        {
            get;
            set;
        }
        public string FromDate
        {
            get;
            set;
        }
        public string ToDate
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

        public string CountStart
        {
            get;
            set;
        }

    }
    //Dropdowns
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
                            ddlBoatType.DataBind();
                        }


                        ddlPaymentType.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlPaymentType.Items.Insert(0, new ListItem("All", "0"));
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
            ddlBoatType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatTrip = new CancelMstr()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

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
                            ddlBoatType.DataSource = dt;
                            ddlBoatType.DataValueField = "BoatTypeId";
                            ddlBoatType.DataTextField = "BoatType";
                            ddlBoatType.DataBind();
                        }
                        else
                        {
                            ddlBoatType.DataBind();
                        }

                        ddlBoatType.Items.Insert(0, new ListItem("All", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetBoatSeater()
    {
        try
        {
            ddlBoatSeater.Items.Clear();

            if (ddlBoatType.SelectedIndex == 0)
            {
                ddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
                return;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatTrip = new CancelMstr()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSeat/BoatRateMstr", BoatTrip).Result;

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
                            ddlBoatSeater.DataSource = dt;
                            ddlBoatSeater.DataValueField = "BoatSeaterId";
                            ddlBoatSeater.DataTextField = "SeaterType";
                            ddlBoatSeater.DataBind();

                        }
                        else
                        {
                            ddlBoatSeater.DataBind();
                        }
                        ddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));

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
    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBoatSeater();
    }
    //Grid
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

                var BoatHouseId = new CancelMstr()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                    PaymentType = ddlPaymentType.SelectedValue.Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
                    CountStart = ViewState["hfstartvalue"].ToString()

                };
                //HttpResponseMessage response = client.PostAsJsonAsync("RptBookingCancelDetails", BoatHouseId).Result;
                HttpResponseMessage response = client.PostAsJsonAsync("RptBookingCancelDetailsV2", BoatHouseId).Result;

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
                            gvBoatCancellation.DataSource = dt;
                            gvBoatCancellation.DataBind();
                            divGridList.Visible = true;
                            gvBoatCancellation.Visible = true;
                            divSearch.Visible = true;
                        }
                        else
                        {
                            gvBoatCancellation.DataBind();
                            divGridList.Visible = false;
                        }
                    }
                    else
                    {

                        divGridList.Visible = true;
                        gvBoatCancellation.Visible = false;
                        divSearch.Visible = false;
                        Next.Enabled = false;
                        back.Enabled = false;
                        if (ViewState["Flag"].ToString() == "N")
                        {
                            back.Enabled = true;
                        }
                        divprevnext.Visible = true;
                        if (ResponseMsg == "No Records Found")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }


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
    protected void gvBoatCancellation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBoatCancellation.PageIndex = e.NewPageIndex;
        BindCancelBookingDetails();
    }

    //Final Submit and Reset

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime Fromdate = DateTime.Parse(txtFromDate.Text, objEnglishDate);
        DateTime Todate = DateTime.Parse(txtToDate.Text, objEnglishDate);
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        txtSearch.Text = string.Empty;
        BackToList.Visible = false;
        Next.Visible = true;
        back.Visible = true;
        back.Enabled = false;

        //Newly Modified
        if (Fromdate <= Todate)
        {
            BindCancelBookingDetails();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('From Date Should Be Lesser Than To Date');", true);
            return;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        BackToList.Visible = false;
        Next.Visible = true;
        back.Visible = true;
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        back.Enabled = false;
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddlBoatType.SelectedValue = "0";
        GetBoatSeater();
        ddlPaymentType.SelectedValue = "0";
        BindCancelBookingDetails();
    }

    //Newly Added

    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        ViewState["Flag"] = "N";
        BindCancelBookingDetails();
    }

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BindCancelBookingDetails();
    }

    protected void AddProcess(int start, int end, out int istart, out int iend)
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
        BookedListSinglePin();
        back.Visible = false;
        Next.Visible = false;
        BackToList.Visible = true;
    }

    protected void BackToList_Click(object sender, EventArgs e)
    {
        back.Visible = true;
        Next.Visible = true;
        BackToList.Visible = false;
        txtSearch.Text = string.Empty;
        BindCancelBookingDetails();

    }

    public void BookedListSinglePin()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new CancelMstr()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                    PaymentType = ddlPaymentType.SelectedValue.Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
                    CountStart = txtSearch.Text.Trim()

                };
                //HttpResponseMessage response = client.PostAsJsonAsync("RptBookingCancelDetails", BoatHouseId).Result;
                HttpResponseMessage response = client.PostAsJsonAsync("RptBookingCancelListPinV2", BoatHouseId).Result;

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
                            gvBoatCancellation.DataSource = dt;
                            gvBoatCancellation.DataBind();
                            divGridList.Visible = true;
                            gvBoatCancellation.Visible = true;
                            divSearch.Visible = true;
                        }
                        else
                        {
                            gvBoatCancellation.DataBind();
                            divGridList.Visible = false;
                        }
                    }
                    else
                    {

                        divGridList.Visible = true;
                        gvBoatCancellation.Visible = false;
                        divSearch.Visible = false;
                        //Next.Enabled = false;
                        //back.Enabled = false;
                        divprevnext.Visible = true;
                        if (ResponseMsg == "No Records Found")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
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
}