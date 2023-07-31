using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_AbstractTripSheetReport : System.Web.UI.Page
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
                //txtFromDate.Enabled = false;
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindAbstractTripSheetReport()
    {
        try
        {
            divCash.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var AvailableBoats = new BoatSearch()
                {
                    QueryType = "AbstractTrip",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtFromDate.Text.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("AbstractTripSheetSummary", AvailableBoats).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvAbstractReport.Visible = true;
                        gvAbstractReport.DataSource = dtExists;
                        gvAbstractReport.DataBind();

                        //  decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Price"));

                        decimal BookedCounting = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BookedCount")));
                        decimal BookedAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BookedAmount")));
                        decimal BookedDeposit = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BookedDeposit")));
                        decimal BookedTotal = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BookedTotal")));

                        decimal BNotStartedCount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BNotStartedCount")));
                        decimal BNotStartedAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BNotStartedAmount")));
                        decimal BNotStartedDeposit = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BNotStartedDeposit")));
                        decimal BNotStartedTotal = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BNotStartedTotal")));

                        decimal BStartedCount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BStartedCount")));
                        decimal BStartedAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BStartedAmount")));
                        decimal BStartedDeposit = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BStartedDeposit")));
                        decimal BStartedTotal = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BStartedTotal")));

                        decimal CompletedCount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("CompletedCount")));
                        decimal CompletedAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("CompletedAmount")));

                        decimal TimeExtnCount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TimeExtnCount")));
                        decimal TimeExtnAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TimeExtnAmount")));

                        decimal DepositRefundCount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("DepositRefundCount")));
                        decimal DepositRefundAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("DepositRefundAmount")));

                        decimal UnClaimedRefundCount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("UnClaimedRefundCount")));
                        decimal UnClaimedRefundAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("UnClaimedRefundAmount")));

                        decimal TotalDeposit = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalDeposit")));

                        decimal CashInHand = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("CashInHand")));

                        gvAbstractReport.FooterRow.Cells[2].Text = "Total";
                        gvAbstractReport.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                        gvAbstractReport.FooterRow.Cells[3].Text = BookedCounting.ToString();
                        gvAbstractReport.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                        gvAbstractReport.FooterRow.Cells[4].Text = BookedAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[5].Text = BookedDeposit.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[6].Text = BookedTotal.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[7].Text = BNotStartedCount.ToString();
                        gvAbstractReport.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Center;

                        gvAbstractReport.FooterRow.Cells[8].Text = BNotStartedAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[9].Text = BNotStartedDeposit.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[10].Text = BNotStartedTotal.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[11].Text = BStartedCount.ToString();
                        gvAbstractReport.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;

                        gvAbstractReport.FooterRow.Cells[12].Text = BStartedAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[13].Text = BStartedDeposit.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[14].Text = BStartedTotal.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;


                        gvAbstractReport.FooterRow.Cells[15].Text = CompletedCount.ToString();
                        gvAbstractReport.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Center;

                        gvAbstractReport.FooterRow.Cells[16].Text = CompletedAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[17].Text = TimeExtnCount.ToString();
                        gvAbstractReport.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Center;

                        gvAbstractReport.FooterRow.Cells[18].Text = TimeExtnAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[19].Text = DepositRefundCount.ToString();
                        gvAbstractReport.FooterRow.Cells[19].HorizontalAlign = HorizontalAlign.Center;

                        gvAbstractReport.FooterRow.Cells[20].Text = DepositRefundAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[20].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[21].Text = UnClaimedRefundCount.ToString();
                        gvAbstractReport.FooterRow.Cells[21].HorizontalAlign = HorizontalAlign.Center;

                        gvAbstractReport.FooterRow.Cells[22].Text = UnClaimedRefundAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[22].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[23].Text = TotalDeposit.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[23].HorizontalAlign = HorizontalAlign.Right;

                        gvAbstractReport.FooterRow.Cells[24].Text = CashInHand.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        gvAbstractReport.FooterRow.Cells[24].HorizontalAlign = HorizontalAlign.Right;
                        lblTicketBalance.Text = CashInHand.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));

                        divCash.Visible = true;
                        btnExportToExcel.Visible = true;

                    }
                    else
                    {
                        gvAbstractReport.Visible = false;
                        gvAbstractReport.DataBind();
                        return;
                    }

                    // Collected Balance Amount Details

                    string ResponseMsg1 = JObject.Parse(vehicleEditresponse)["Table1"].ToString();
                    DataTable dtExists1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);

                    if (dtExists1.Rows.Count > 0)
                    {
                        lblCollectedBalance.Text = Convert.ToDecimal(dtExists1.Rows[0]["CollectedBalance"].ToString()).ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        lblRefundedBalance.Text = Convert.ToDecimal(dtExists1.Rows[0]["RefundedBalance"].ToString()).ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        lblBalance.Text = Convert.ToDecimal(dtExists1.Rows[0]["Balance"].ToString()).ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                    }

                    // Final Calculation
                    decimal dBalance = 0;
                    decimal dBoatBalance = 0;
                    decimal dCashInHand = 0;
                    decimal SettlementAmount = 0;

                    SettlementAmount = Convert.ToDecimal(RowerSettlement(txtFromDate.Text.Trim(), txtFromDate.Text.Trim()));
                    lblRowerSettlement.Text = SettlementAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN")); ;

                    if (Convert.ToDecimal(lblBalance.Text) > 0)
                    {
                        dBalance = Convert.ToDecimal(lblBalance.Text);
                    }

                    if (Convert.ToDecimal(lblTicketBalance.Text) > 0)
                    {
                        dBoatBalance = Convert.ToDecimal(lblTicketBalance.Text);
                    }

                    dCashInHand = dBalance + dBoatBalance - SettlementAmount;
                    lblFinalCashInHand.Text = dCashInHand.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
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

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        BindAbstractTripSheetReport();
    }

    protected void gvAbstractReport_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 4;
            HeaderCell.Text = "";
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 4;
            HeaderCell.Text = "";
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 4;
            HeaderCell.Text = "";
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 9;
            HeaderCell.Text = "Trip - Completed";
            HeaderGridRow.Cells.Add(HeaderCell);

            //HeaderCell = new TableCell();
            //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            //HeaderCell.Font.Bold = true;
            //HeaderCell.ColumnSpan = 3;
            //HeaderCell.Text = "";
            //HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 1;
            HeaderGridRow.Cells.Add(HeaderCell);


            GridView HeaderGrid1 = (GridView)sender;
            GridViewRow HeaderGridRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "";
            HeaderCell1.ColumnSpan = 3;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Booking";
            HeaderCell1.ColumnSpan = 4;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Trip - Not Started";
            HeaderCell1.ColumnSpan = 4;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Trip - Started";
            HeaderCell1.ColumnSpan = 4;
            HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Overall";
            HeaderCell1.ColumnSpan = 2;
            HeaderGridRow1.Cells.Add(HeaderCell1);


            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.Text = "Deposit Break-up";
            HeaderCell1.ColumnSpan = 7;
            HeaderGridRow1.Cells.Add(HeaderCell1);




            //HeaderCell1 = new TableCell();
            //HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            //HeaderCell1.Font.Bold = true;
            //HeaderCell1.ColumnSpan = 3;
            //HeaderCell1.Text = "Ex-change Amount";
            //HeaderGridRow1.Cells.Add(HeaderCell1);

            HeaderCell1 = new TableCell();
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell1.Font.Bold = true;
            HeaderCell1.ColumnSpan = 1;
            HeaderCell1.Text = "Cash In Hand";
            HeaderGridRow1.Cells.Add(HeaderCell1);



            gvAbstractReport.Controls[0].Controls.AddAt(0, HeaderGridRow1);
            gvAbstractReport.Controls[0].Controls.AddAt(0, HeaderGridRow);

        }
    }

    protected void ExportToExcel_Click(object sender, ImageClickEventArgs e)
    {
        ExportGridToExcel();
    }

    private void ExportGridToExcel()
    {
        Response.ClearContent();
        Response.AppendHeader("content-disposition", "attachment; filename=AbstractTripSheetSummary.xls");
        Response.ContentType = "application/excel";
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
        gvAbstractReport.HeaderRow.Style.Add("background-color", "#b3b3b3");
        gvAbstractReport.RenderControl(htw);
        Response.Write(stringWriter.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    public class BoatSearch
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public string RowerSettlement(string FromDate, string ToDate)
    {
        string rowerSettlementAmount = string.Empty;

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var body = new BoatSearch()
                {
                    QueryType = "RowerSettlement",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = FromDate.ToString().Trim(),
                    ToDate = ToDate.ToString().Trim(),
                };
                response = client.PostAsJsonAsync("AbstractTripSheetSummary", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        rowerSettlementAmount = dtExists.Rows[0]["RowerSettlement"].ToString();
                    }
                    else
                    {
                        rowerSettlementAmount = "0";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
        return rowerSettlementAmount;
    }

}