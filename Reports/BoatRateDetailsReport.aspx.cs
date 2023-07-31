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

/// <summary>
/// Developed By Imran , K.Abhinaya
/// 05/Jul/2021
/// Modified By K.Abhinaya
/// 07/Jul/2021
/// </summary>

public partial class Boating_BoatRateDetailsReport : System.Web.UI.Page
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

            }
            BindBoatRateDetailsReport();
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
      
    }


/// <summary>
/// Binding The Boat Rate Details Record
/// </summary>
    public void BindBoatRateDetailsReport()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatRateDetails = new BoatRateDetails()
                {
                    QueryType = "GetBoatRateDetailsReport",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", BoatRateDetails).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvBoatRateDetails.DataSource = dtExists;
                        gvBoatRateDetails.DataBind();
                        gvBoatRateDetails.Visible = true;
                    }
                    else
                    {
                        gvBoatRateDetails.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Rate Details Not Found !');", true);
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
    /// <summary>
    /// Creating Header Cells For griview Records
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvBoatRateDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 8;
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 4;
            HeaderCell.Text = "Normal Charge";
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 4;
            HeaderCell.Text = "Premium Charge";
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.Font.Bold = true;
            HeaderCell.ColumnSpan = 9;
            HeaderCell.Text = "Extension Rate";
            HeaderGridRow.Cells.Add(HeaderCell);


            gvBoatRateDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);


        }
    }

    /// <summary>
    ///Export To Grid To Excel Button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ExportToExcel_Click(object sender, ImageClickEventArgs e)
    {
        ExportGridToExcel();
    }
    /// <summary>
    /// Export To Grid To Excel Method
    /// </summary>
    private void ExportGridToExcel()
    {
        DataTable dt = (DataTable)ViewState["BoatRateDetails"];
        Response.ClearContent();
        Response.AppendHeader("content-disposition", "attachment; filename=BoatRateDetails.xls");
        Response.ContentType = "application/excel";
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
        gvBoatRateDetails.HeaderRow.Style.Add("background-color", "#f9f9f9");
        gvBoatRateDetails.RenderControl(htw);
        Response.Write(stringWriter.ToString());
        Response.End();

       
    }
    /// <summary>
    ///  Handling RenderControl For Excel
    /// </summary>
    /// <param name="control"></param>
    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    public class BoatRateDetails
    {
        public string BoatHouseId { get; set; }
        public string BoatType { get; set; }
        public string BoatTypeId { get; set; }
        public string SeaterType { get; set; }
        public string BoatSeaterId { get; set; }
        public string SelfDrive { get; set; }
        public string TimeExtension { get; set; }
        public string BoatMinTime { get; set; }
        public string BoatGraceTime { get; set; }
        public string Deposit { get; set; }
        public string BoatMinCharge { get; set; }
        public string RowerMinCharge { get; set; }
        public string BoatMinTaxAmt { get; set; }
        public string BoatMinTotAmt { get; set; }
        public string BoatPremMinCharge { get; set; }
        public string RowerPremMinCharge { get; set; }
        public string BoatPremTaxAmt { get; set; }
        public string BoatPremTotAmt { get; set; }
        public string ExtensionType { get; set; }
        public string ExtFromTime { get; set; }
        public string ExtToTime { get; set; }
        public string AmountType { get; set; }
        public string Percentage { get; set; }
        public string BoatExtnCharge { get; set; }
        public string RowerExtnCharge { get; set; }
        public string BoatExtnTaxAmt { get; set; }
        public string BoatExtnTotAmt { get; set; }

        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

    }
   
   
    
}