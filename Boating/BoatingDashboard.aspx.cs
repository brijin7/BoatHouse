using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Helpers;

public partial class Boating_BoatingDashboard : System.Web.UI.Page
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
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    BindBoatBooking();
                    BindOtherAbstract();
                    AvailableBoat();
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void AvailableBoat()
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
                var AvailableBoats = new AvailableBoats()
                {
                    QueryType = "NewBoatBooking",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy")

                };
                response = client.PostAsJsonAsync("NewBoatBookingDet", AvailableBoats).Result;

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

                            gvAvailableBoats.Visible = true;
                            gvAvailableBoats.DataSource = dt;
                            gvAvailableBoats.DataBind();



                        }
                        else
                        {
                            gvAvailableBoats.DataBind();
                            return;
                        }
                    }
                    else
                    {
                        gvAvailableBoats.DataBind();

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
        }

    }

    protected void gvAvailableBoats_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAvailableBoats.PageIndex = e.NewPageIndex;
    }

    protected void grvMergeHeader_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 4;
            HeaderCell.Text = "Booked";
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);


            GridView HeaderGrid1 = (GridView)sender;
            GridViewRow HeaderGridRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Boat Details";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Available";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);




            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.ColumnSpan = 2;
            HeaderCell1.Text = "Online";
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.ColumnSpan = 2;
            HeaderCell1.Text = "Boat House";
            HeaderGridRow1.Cells.Add(HeaderCell1);



            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Waiting Trip";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Total";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Trip Completed";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            gvAvailableBoats.Controls[0].Controls.AddAt(0, HeaderGridRow1);
            gvAvailableBoats.Controls[0].Controls.AddAt(0, HeaderGridRow);

        }
    }

    public void BindBoatBooking()
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
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy")
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
                            divboatBooking.Visible = true;

                            GvBoatBookingHdr.DataSource = dt;
                            GvBoatBookingHdr.DataBind();
                            GvBoatBookingHdr.Visible = true;

                            int TotalCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("NoCount")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TotalAmount")));

                            GvBoatBookingHdr.FooterRow.Cells[0].Text = "Total";
                            GvBoatBookingHdr.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;

                            GvBoatBookingHdr.FooterRow.Cells[1].Text = TotalCount.ToString();
                            GvBoatBookingHdr.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            GvBoatBookingHdr.FooterRow.Cells[2].Text = TotalAmount.ToString("N2");
                            GvBoatBookingHdr.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            GvBoatBookingHdr.Visible = false;
                            divboatBooking.Visible = false;
                        }
                    }
                    else
                    {

                        GvBoatBookingHdr.Visible = false;
                        divboatBooking.Visible = false;
                    }
                }
                else
                {

                    GvBoatBookingHdr.Visible = false;
                    divboatBooking.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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

                HttpResponseMessage response = client.PostAsJsonAsync("AbstractOtherSvc", new OtherBook()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy")
                }).Result;

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
                            divothrserv.Visible = true;

                            GVabstractsrv.DataSource = dt;
                            GVabstractsrv.DataBind();
                            GVabstractsrv.Visible = true;

                            int TotalAdult = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("NoCount")));

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
                            divothrserv.Visible = false;
                        }
                    }
                    else
                    {
                        GVabstractsrv.Visible = false;
                        divothrserv.Visible = false;

                    }
                }
                else
                {
                    GVabstractsrv.Visible = false;
                    divothrserv.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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
    }
    public class OtherBook
    {
        public string BoatHouseId { get; set; }
        public string BookingType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingId { get; set; }
        public string ServiceId { get; set; }
    }
    public class AvailableBoats
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string BookingDate { get; set; }
    }

}