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

public partial class Boating_RptRestaurant_Test : System.Web.UI.Page
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
                txtfromdate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");

                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                BindRestaurantAbstract();
                BindRestaurantlistall();
                //Restaurantlist();

                getCategoryName();
                getItemName();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    public class OtherRestaurant
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string NoOfItems { get; set; }
        public string NetAmount { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Category { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string BoatHouseId { get; set; }
    }

    public void getCategoryName()
    {
        try
        {
            ddlcategoryname.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var RowerCharges = new OtherRestaurant()
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
                            ddlcategoryname.DataSource = dt;
                            ddlcategoryname.DataValueField = "CategoryId";
                            ddlcategoryname.DataTextField = "CategoryName";
                            ddlcategoryname.DataBind();

                        }
                        else
                        {
                            ddlcategoryname.DataBind();
                        }
                        //ddlBoatType.DataBind();
                        ddlcategoryname.Items.Insert(0, new ListItem("All", "0"));
                        //ddlBoatType.Items.Insert(0, new ListItem("Select Boat Type", "0"));

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

    public void getItemName()
    {
        try
        {
            ddlItemName.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var RowerCharges = new OtherRestaurant()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Category = ddlcategoryname.Text.ToString()

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
                            ddlItemName.DataSource = dt;
                            ddlItemName.DataValueField = "ServiceId";
                            ddlItemName.DataTextField = "ServiceName";
                            ddlItemName.DataBind();

                        }
                        else
                        {
                            ddlItemName.DataBind();
                        }
                        //ddlBoatType.DataBind();
                        ddlItemName.Items.Insert(0, new ListItem("All", "0"));
                        //ddlBoatType.Items.Insert(0, new ListItem("Select Boat Type", "0"));

                    }
                    else
                    {
                        ddlItemName.Items.Insert(0, new ListItem("All", "0"));

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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindRestaurantAbstract();
        BindRestaurantlistall();
        //Restaurantlist();
        GvRptRes.Visible = false;
    }

    protected void GvRptRes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvRptRes.PageIndex = e.NewPageIndex;
        Restaurantlist();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlcategoryname.SelectedIndex = 0;
        ddlItemName.SelectedIndex = 0;
        ddlItemName.Items.Clear();
        ddlItemName.Items.Insert(0, new ListItem("All", "0"));
        txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        BindRestaurantlistall();
        BindRestaurantAbstract();
        Restaurantlist();
        GvRptRes.PageIndex = 0;


    }

    public void BindRestaurantAbstract()
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

                var BookingOtherServices = new OtherRestaurant()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtfromdate.Text,
                    ToDate = txttodate.Text,
                    CategoryId = ddlcategoryname.Text.Trim(),
                    ServiceId = ddlItemName.Text.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RestaurantAbstract", BookingOtherServices).Result;

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
                            GvRestarurant.DataSource = dt;
                            GvRestarurant.DataBind();
                            GvRestarurant.Visible = true;

                            int TotalQty = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Quantity")));

                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Total")));

                            GvRestarurant.FooterRow.Cells[0].Text = "Total";
                            GvRestarurant.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;

                            GvRestarurant.FooterRow.Cells[1].Text = TotalQty.ToString();
                            GvRestarurant.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            GvRestarurant.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
                            GvRestarurant.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            GvRestarurant.Visible = false;
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

    protected void ddlcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {

        getItemName();

    }


    public void BindRestaurantlistall()
    {
        try
        {
            divreslistall.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BookingOtherServices = new OtherRestaurant()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtfromdate.Text,
                    ToDate = txttodate.Text,
                    CategoryId = ddlcategoryname.Text.Trim(),
                    ServiceId = ddlItemName.Text.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("Restaurant/ListAll", BookingOtherServices).Result;

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
                            gvResListall.DataSource = dt;
                            gvResListall.DataBind();
                            gvResListall.Visible = true;

                            int TotalQty = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Quantity")));

                            decimal TotalItemRate = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ItemRate")));

                            decimal TotalChargeAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Charge")));

                            decimal TotalTaxAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));

                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Total")));

                            gvResListall.FooterRow.Cells[2].Text = "Total";
                            gvResListall.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                            gvResListall.FooterRow.Cells[3].Text = TotalItemRate.ToString("N2");
                            gvResListall.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                            gvResListall.FooterRow.Cells[4].Text = TotalQty.ToString();
                            gvResListall.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                            gvResListall.FooterRow.Cells[5].Text = TotalChargeAmount.ToString("N2");
                            gvResListall.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                            gvResListall.FooterRow.Cells[6].Text = TotalTaxAmount.ToString("N2");
                            gvResListall.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                            gvResListall.FooterRow.Cells[7].Text = TotalAmount.ToString("N2");
                            gvResListall.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            gvResListall.Visible = false;
                        }
                    }
                    else
                    {
                        divreslistall.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divreslistall.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    public void Restaurantlist()
    {
        try
        {
            divGvRptRes.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BookingOtherServices = new OtherRestaurant()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtfromdate.Text,
                    ToDate = txttodate.Text,
                    CategoryId = ddlcategoryname.Text.Trim(),
                    ServiceId = ddlItemName.Text.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("listingRestaurant_Test", BookingOtherServices).Result;

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
                            GvRptRes.DataSource = dt;
                            GvRptRes.DataBind();
                            GvRptRes.Visible = true;

                            int TotalQty = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("NoOfItems")));

                            decimal TotalItemRate = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ItemRate")));

                            decimal TotalChargeAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ChargePerItem")));

                            decimal TotalTaxAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));

                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Total")));

                            GvRptRes.FooterRow.Cells[3].Text = "Total";
                            GvRptRes.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                            GvRptRes.FooterRow.Cells[4].Text = TotalItemRate.ToString("N2");
                            GvRptRes.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                            GvRptRes.FooterRow.Cells[5].Text = TotalQty.ToString();
                            GvRptRes.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;

                            GvRptRes.FooterRow.Cells[6].Text = TotalChargeAmount.ToString("N2");
                            GvRptRes.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                            GvRptRes.FooterRow.Cells[7].Text = TotalTaxAmount.ToString("N2");
                            GvRptRes.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                            GvRptRes.FooterRow.Cells[8].Text = TotalAmount.ToString("N2");
                            GvRptRes.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            GvRptRes.Visible = false;
                        }
                    }
                    else
                    {
                        divGvRptRes.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divGvRptRes.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }



    protected void lbtnDetails_Click(object sender, EventArgs e)
    {
        GvRptRes.PageIndex = 0;
        Restaurantlist();
    }
}

