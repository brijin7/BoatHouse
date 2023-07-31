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
public partial class Boating_RptAbstractBoatBooking : System.Web.UI.Page
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

                GvBoatBookingHdr.PageIndex = 0;
                BindBoatBooking();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public class Bookinghdr
    {
        public string BoatHouseId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string TotalAmount { get; set; }
        public string NoCount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string BookingDate { get; set; }
        public string Createdby { get; set; }
    }

    protected void GvBoatBookingHdr_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBoatBookingHdr.PageIndex = e.NewPageIndex;
        BindBoatBooking();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GvBoatBookingHdr.PageIndex = 0;
        divBoatGrid.Visible = false;
        divUBService.Visible = false;
        BindBoatBooking();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        GvBoatBookingHdr.PageIndex = 0;
        BindBoatBooking();
        GvBoatBooking.Visible = false;
        GVUBService.Visible = false;
    }

    public void BindBoatBooking()
    {
        try
        {
            divgridbooking.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Bookinghdr = new Bookinghdr()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("AbstractBoatBooking", Bookinghdr).Result;

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
                            GvBoatBookingHdr.DataSource = dt;
                            GvBoatBookingHdr.DataBind();
                            GvBoatBookingHdr.Visible = true;

                            int TotalCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("NoCount")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TotalAmount")));
                            decimal BoatCharge = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatCharge")));
                            decimal BoatDeposit = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatDeposit")));

                            GvBoatBookingHdr.FooterRow.Cells[1].Text = "Total";
                            GvBoatBookingHdr.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            GvBoatBookingHdr.FooterRow.Cells[2].Text = TotalCount.ToString();
                            GvBoatBookingHdr.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                            GvBoatBookingHdr.FooterRow.Cells[3].Text = BoatCharge.ToString("N2");
                            GvBoatBookingHdr.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                            GvBoatBookingHdr.FooterRow.Cells[4].Text = BoatDeposit.ToString("N2");
                            GvBoatBookingHdr.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            GvBoatBookingHdr.FooterRow.Cells[5].Text = TotalAmount.ToString("N2");
                            GvBoatBookingHdr.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            GvBoatBookingHdr.Visible = false;
                        }
                    }
                    else
                    {
                        divgridbooking.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divgridbooking.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void GvBoatBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBoatBooking.PageIndex = e.NewPageIndex;
        BindUserBoatBooking();
    }
    protected void lblcount_Click(object sender, EventArgs e)
    {
        try
        {
            GvBoatBooking.PageIndex = 0;
            divBoatGrid.Visible = true;
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = GvBoatBookingHdr.DataKeys[gvrow.RowIndex].Value.ToString();
            Label UserId = (Label)gvrow.FindControl("lblUserId");

            hfuserid.Value = UserId.Text;
            ServiceWiseCollection();
            BindUserBoatBooking();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void ServiceWiseCollection()
    {
        try
        {
            divUBService.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BookingServices = new ServiceWiseCollectionUser()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text,
                    ToDate = txtToDate.Text,
                    CreatedBy = hfuserid.Value.Trim(),
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RptUserBasedServiceList", BookingServices).Result;

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
                            GVUBService.DataSource = dt;
                            GVUBService.DataBind();
                            GVUBService.Visible = true;
                            divUBService.Visible = true;


                            int TotalCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Count")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));
                            decimal BoatCharge = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatCharge")));
                            decimal BoatDeposit = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatDeposit")));

                            GVUBService.FooterRow.Cells[3].Text = "Total";
                            GVUBService.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                            GVUBService.FooterRow.Cells[4].Text = TotalCount.ToString();
                            GVUBService.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;


                            GVUBService.FooterRow.Cells[5].Text = BoatCharge.ToString("N2");
                            GVUBService.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                            GVUBService.FooterRow.Cells[6].Text = BoatDeposit.ToString("N2");
                            GVUBService.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            GVUBService.FooterRow.Cells[7].Text = TotalAmount.ToString("N2");
                            GVUBService.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            GVUBService.Visible = false;
                        }
                    }
                    else
                    {
                        divUBService.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divUBService.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindUserBoatBooking()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Bookinghdr = new Bookinghdr()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
                    UserId = hfuserid.Value.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("UserBoatBookingDtl", Bookinghdr).Result;

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
                            GvBoatBooking.DataSource = dt;
                            GvBoatBooking.DataBind();
                            GvBoatBooking.Visible = true;
                            divBoatGrid.Visible = true;
                            int TotalCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("NoCount")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TotalAmount")));
                            decimal BoatCharge = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatCharge")));
                            decimal BoatDeposit = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("BoatDeposit")));

                            GvBoatBooking.FooterRow.Cells[4].Text = "Total";
                            GvBoatBooking.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                            GvBoatBooking.FooterRow.Cells[5].Text = TotalCount.ToString();
                            GvBoatBooking.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;

                            GvBoatBooking.FooterRow.Cells[6].Text = BoatCharge.ToString("N2");
                            GvBoatBooking.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            GvBoatBooking.FooterRow.Cells[7].Text = BoatDeposit.ToString("N2");
                            GvBoatBooking.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            GvBoatBooking.FooterRow.Cells[8].Text = TotalAmount.ToString("N2");
                            GvBoatBooking.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            GvBoatBooking.Visible = false;
                        }
                    }
                    else
                    {
                        GvBoatBooking.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    GvBoatBooking.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }


    public class ServiceWiseCollectionUser
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CreatedBy { get; set; }
    }


}