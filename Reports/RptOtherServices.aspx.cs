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

public partial class Boating_RptOtherServices : System.Web.UI.Page
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
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                BindOtherCategoryList();

                BindOtherAbstract();
                BingOtherServiceSummary();
                BindBookingOtherServices();
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
            ddlCategory.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new OtherBook()
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
                        }
                        else
                        {
                            ddlCategory.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Category Details Not Found...!');", true);
                        }
                    }

                    ddlCategory.Items.Insert(0, new ListItem("All", "0"));
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

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetServiceName(ddlCategory.SelectedValue.Trim());
    }

    public void GetServiceName(string sCatId)
    {
        try
        {
            ddlServiceName.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string boatTypeIds = string.Empty;

                var service = new OtherBook()
                {
                    Category = sCatId.Trim(),
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
                            ddlServiceName.DataSource = dt;
                            ddlServiceName.DataValueField = "ServiceId";
                            ddlServiceName.DataTextField = "ServiceName";
                            ddlServiceName.DataBind();
                        }
                        else
                        {
                            ddlServiceName.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Service Details Not Found !');", true);
                        }
                    }

                    ddlServiceName.Items.Insert(0, new ListItem("All", "0"));

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

    public void BindOtherAbstract()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var service = new OtherBook()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Category = ddlCategory.SelectedValue.Trim(),
                    ServiceId = ddlServiceName.SelectedValue.Trim(),
                    BookingType = ddlBookingType.SelectedValue.Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RptAbstractOtherService", service).Result;

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
                            GVabstractsrv.DataSource = dt;
                            GVabstractsrv.DataBind();
                            GVabstractsrv.Visible = true;

                            int TotalAdult = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("TicketCount")));

                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));

                            GVabstractsrv.FooterRow.Cells[0].Text = "Total";
                            GVabstractsrv.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;

                            GVabstractsrv.FooterRow.Cells[1].Text = TotalAdult.ToString();
                            GVabstractsrv.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            GVabstractsrv.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
                            GVabstractsrv.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            GVabstractsrv.Visible = false;
                        }
                    }
                    else
                    {
                        GVabstractsrv.Visible = false;
                    }
                }
                else
                {
                    GVabstractsrv.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindBookingOtherServices()

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

                var BookingOtherServices = new OtherBook()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Category = ddlCategory.SelectedValue.Trim(),
                    ServiceId = ddlServiceName.SelectedValue.Trim(),
                    BookingType = ddlBookingType.SelectedValue.Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RptOtherService", BookingOtherServices).Result;

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            GvOtherServices.DataSource = dt;
                            GvOtherServices.DataBind();
                            GvOtherServices.Visible = true;
                            int NoOfItems = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("NoOfItems")));

                            decimal ServiceFare = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ServiceFare")));

                            decimal TaxAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));

                            decimal NetAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));


                            GvOtherServices.FooterRow.Cells[4].Text = "Total";
                            GvOtherServices.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                            GvOtherServices.FooterRow.Cells[5].Text = NoOfItems.ToString();
                            GvOtherServices.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;

                            GvOtherServices.FooterRow.Cells[6].Text = ServiceFare.ToString("N2");
                            GvOtherServices.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                            GvOtherServices.FooterRow.Cells[7].Text = TaxAmount.ToString("N2");
                            GvOtherServices.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                            GvOtherServices.FooterRow.Cells[8].Text = NetAmount.ToString("N2");
                            GvOtherServices.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            GvOtherServices.Visible = false;
                        }
                    }
                    else
                    {
                        divGridList.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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

    public void BingOtherServiceSummary()

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

                var BookingOtherServices = new OtherBook()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Category = ddlCategory.SelectedValue.Trim(),
                    ServiceId = ddlServiceName.SelectedValue.Trim(),
                    BookingType = ddlBookingType.SelectedValue.Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RptOtherServiceSummary", BookingOtherServices).Result;

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            GvOtherServiceSummary.DataSource = dt;
                            GvOtherServiceSummary.DataBind();
                            GvOtherServiceSummary.Visible = true;

                            int NoOfItems = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("NoOfItems")));

                            //decimal ItemAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ItemAmount")));

                            decimal NetAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));

                            decimal ItemFare = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ItemFare")));

                            decimal taxAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));


                            GvOtherServiceSummary.FooterRow.Cells[3].Text = "Total";
                            GvOtherServiceSummary.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                            GvOtherServiceSummary.FooterRow.Cells[4].Text = NoOfItems.ToString();
                            GvOtherServiceSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;


                            GvOtherServiceSummary.FooterRow.Cells[5].Text = ItemFare.ToString();
                            GvOtherServiceSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                            GvOtherServiceSummary.FooterRow.Cells[6].Text = taxAmount.ToString();
                            GvOtherServiceSummary.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                            GvOtherServiceSummary.FooterRow.Cells[7].Text = NetAmount.ToString("N2");
                            GvOtherServiceSummary.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            GvOtherServiceSummary.Visible = false;
                            GvOtherServices.Visible = false;
                        }
                    }
                    else
                    {
                        GvOtherServiceSummary.Visible = false;
                        GvOtherServices.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    GvOtherServiceSummary.Visible = false;
                    GvOtherServices.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlCategory.SelectedIndex = 0;
        ddlServiceName.SelectedIndex = 0;
        ddlBookingType.SelectedIndex = 0;
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        GvOtherServices.PageIndex = 0;

        BindBookingOtherServices();
        BindOtherAbstract();
        BingOtherServiceSummary();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        GvOtherServices.PageIndex = 0;
        BindOtherAbstract();
        BingOtherServiceSummary();
        BindBookingOtherServices();
    }

    protected void GvOtherServices_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvOtherServices.PageIndex = e.NewPageIndex;
        BindBookingOtherServices();
    }


    public class OtherBook
    {
        public string BoatHouseId { get; set; }
        public string BookingType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingId { get; set; }
        public string ServiceId { get; set; }
        public string Category { get; set; }
    }

    public class BookingOtherServices
    {
        public string BookingId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string BookingType { get; set; }
        public string BookingDate { get; set; }
        public string NetAmount { get; set; }
        public string BoatHouseId { get; set; }
        public string ChargePerItem { get; set; }
        public string NoOfItems { get; set; }
        public string GetTaxAmountDetails { get; set; }
        public string ServiceFare { get; set; }
        public string TaxAmount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }


}