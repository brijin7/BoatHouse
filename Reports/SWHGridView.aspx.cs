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

public partial class Reports_SWHGridView : System.Web.UI.Page
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
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Attributes.Add("readonly", "readonly");

                BindserviceWise();

                if (Session["UserRole"].ToString().Trim() == "Sadmin")
                {
                    GVServiceWiseHistroy.Columns[7].Visible = true;
                }
            }

        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindserviceWise()
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

                var ServiceWise = new ServiceWise()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
                    QueryType = "MiniPrintHistory",
                    ServiceType = Session["UserRole"].ToString().Trim(),
                    BookingId = "",
                    Input1 = Session["UserId"].ToString().Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", ServiceWise).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        GVServiceWiseHistroy.DataSource = dtExists;
                        GVServiceWiseHistroy.DataBind();
                        GVServiceWiseHistroy.Visible = true;
                    }
                    else
                    {
                        GVServiceWiseHistroy.Visible = false;
                        spnote.Visible = false;
                    }
                }
                else
                {
                    divServiceWise.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindserviceWiseDeno()
    {
        try
        {
            divServiceWise.Visible = true;
            divServiceDeno.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var ServiceDeno = new ServiceWise()
                {
                    ReferenceId = ViewState["UniqueId"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("ServiceWiseDenoGrid", ServiceDeno).Result;

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
                            GVServiceWiseDenomination.DataSource = dt;
                            GVServiceWiseDenomination.DataBind();
                            GVServiceWiseDenomination.Visible = true;
                            int TotalCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("DenominationCount")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("DenominationAmount")));


                            GVServiceWiseDenomination.FooterRow.Cells[1].Text = "Total";
                            GVServiceWiseDenomination.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            GVServiceWiseDenomination.FooterRow.Cells[2].Text = TotalCount.ToString();
                            GVServiceWiseDenomination.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            GVServiceWiseDenomination.FooterRow.Cells[3].Text = TotalAmount.ToString();
                            GVServiceWiseDenomination.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                            for (int i = 0; i < GVServiceWiseDenomination.Rows.Count; i++)
                            {
                                string Denomination = dt.Rows[i]["Denomination"].ToString();
                                Session["Denomination"] = Denomination;
                                string DenominationCount = dt.Rows[i]["DenominationCount"].ToString();
                                Session["DenominationCount"] = DenominationCount;
                                string DenominationAmount = dt.Rows[i]["DenominationAmount"].ToString();
                                Session["DenominationAmount"] = DenominationAmount;
                            }

                        }
                        else
                        {
                            GVServiceWiseHistroy.Visible = false;
                        }
                    }
                    else
                    {
                        divServiceWise.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divServiceWise.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void Pdf_Click(object sender, ImageClickEventArgs e)
    {

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var ServiceDeno = new ServiceWise()
            {
                ReferenceId = ViewState["UniqueId"].ToString().Trim(),
                BoatHouseId = Session["BoatHouseId"].ToString().Trim()

            };
            HttpResponseMessage response = client.PostAsJsonAsync("ServiceWiseDenoGrid", ServiceDeno).Result;

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
                        GVServiceWiseDenomination.DataSource = dt;
                        GVServiceWiseDenomination.DataBind();
                        GVServiceWiseDenomination.Visible = true;
                        int TotalCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("DenominationCount")));
                        decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("DenominationAmount")));


                        GVServiceWiseDenomination.FooterRow.Cells[1].Text = "Total";
                        GVServiceWiseDenomination.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                        GVServiceWiseDenomination.FooterRow.Cells[2].Text = TotalCount.ToString();
                        GVServiceWiseDenomination.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                        GVServiceWiseDenomination.FooterRow.Cells[3].Text = TotalAmount.ToString();
                        GVServiceWiseDenomination.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                        string sden = string.Empty;

                        for (int i = 0; i < GVServiceWiseDenomination.Rows.Count; i++)
                        {
                            string Denomination = dt.Rows[i]["Denomination"].ToString();
                            //Session["Denomination"] = Denomination;
                            string DenominationCount = dt.Rows[i]["DenominationCount"].ToString();
                            //Session["DenominationCount"] = DenominationCount;
                            string DenominationAmount = dt.Rows[i]["DenominationAmount"].ToString();
                            //Session["DenominationAmount"] = DenominationAmount;
                            //GridViewRow gvrow = NamingContainer as GridViewRow;
                            //Label Denomination = (Label)gvrow.FindControl("lblDenomination");
                            //ViewState["Denomination"] = Denomination.Text;
                            //Label DenominationCount = (Label)gvrow.FindControl("lblDenominationCount");
                            //ViewState["DenominationCount"] = DenominationCount.Text;
                            //Label DenominationAmount = (Label)gvrow.FindControl("lblDenominationAmount");
                            //ViewState["DenominationAmount"] = DenominationAmount.Text;


                            sden += "~" + Denomination.Trim() + "-" + DenominationCount.Trim() + "-" + DenominationAmount.Trim();

                        }
                        string FinalValue = sden.TrimStart('~');
                        if (ViewState["Services"].ToString().Trim() == "1")  // Boat Services
                        {
                            Response.Redirect("~/Boating/Print.aspx?rt=rbss&UId=" + ViewState["Username"].ToString().Trim() + "&fDat=" + ViewState["BookingDate"].ToString().Trim() + "&bi=&UN=" + ViewState["UsernameId"].ToString().Trim() + "&BTI=" + ViewState["Category"].ToString().Trim() + "&BT=" + ViewState["CategoryId"].ToString().Trim() + "&sDen=" + FinalValue.Trim() + "");
                        }

                        if (ViewState["Services"].ToString().Trim() == "2") // Restaurant Services
                        {
                            Response.Redirect("~/Boating/Print.aspx?rt=rrss&UId=" + ViewState["Username"].ToString().Trim() + "&fDat=" + ViewState["BookingDate"].ToString().Trim() + "&bi=&UN=" + ViewState["UsernameId"].ToString().Trim() + "&sDen=" + FinalValue.Trim() + "");
                        }

                        if (ViewState["Services"].ToString().Trim() == "3") // Other Services
                        {
                            Response.Redirect("~/Boating/Print.aspx?rt=ross&UId=" + ViewState["Username"].ToString().Trim() + "&fDat=" + ViewState["BookingDate"].ToString().Trim() + "&bi=&UN=" + ViewState["UsernameId"].ToString().Trim() + "&sDen=" + FinalValue.Trim() + "");
                        }

                        if (ViewState["Services"].ToString().Trim() == "4") // Cash in Refund
                        {
                            Response.Redirect("~/Boating/Print?rt=recr&bi=&fDat=" + ViewState["BookingDate"].ToString().Trim() + "&sDen=" + FinalValue.Trim() + "&bi=&UId=" + ViewState["Username"].ToString().Trim() + "");
                        }

                    }
                }
            }
        }
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
        public string BookingId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

    }


    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindserviceWise();
    }

    protected void lblBookingDate_Click(object sender, EventArgs e)
    {
        try
        {
            GVServiceWiseHistroy.PageIndex = 0;
            divServiceWise.Visible = true;
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex].Value.ToString();
            Label UserId = (Label)gvrow.FindControl("lblUniqueId");
            ViewState["UniqueId"] = UserId.Text;
            MpeTrip.Show();

            ViewState["UsernameId"] = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim();
            ViewState["Username"] = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();
            ViewState["Category"] = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["CategoryId"].ToString().Trim();
            ViewState["CategoryId"] = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["CategoryName"].ToString().Trim();
            ViewState["BookingDate"] = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["BookingDate"].ToString().Trim();
            ViewState["Services"] = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["ServiceId"].ToString().Trim();

            BindserviceWiseDeno();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void imgCloseTicket_Click(object sender, ImageClickEventArgs e)
    {
        MpeTrip.Hide();
    }

    protected void lbtnServiceReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("RptServiceWiseCollection.aspx");
    }

    protected void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueId = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
            string BookingDate = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["BookingDate"].ToString().Trim();
            string ServiceId = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["ServiceId"].ToString().Trim();
            string UserId = GVServiceWiseHistroy.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                var DeleteMiniPrintHistory = new ServiceWise()
                {
                    QueryType = "DeleteMiniPrintHistory",
                    Input1 = UniqueId.ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = BookingDate.ToString().Trim(),
                    Input2 = ServiceId.ToString().Trim(),
                    UserId = UserId.ToString().Trim()
                };
                response = client.PostAsJsonAsync("ServiceWiseDeletePrintHistory", DeleteMiniPrintHistory).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindserviceWise();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        return;

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
            return;
        }
    }
}