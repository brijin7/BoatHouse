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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Microsoft.CSharp.RuntimeBinder;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Helpers;

public partial class Boating_RptRowerSettlement : System.Web.UI.Page
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
        try {
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

                txtMonthFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtMonthToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtMonthFromDate.Attributes.Add("readonly", "readonly");
                txtMonthToDate.Attributes.Add("readonly", "readonly");

                BindSettlementDetails();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grvSettle.PageIndex = 0;
        BindSettlementDetails();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        grvSettle.PageIndex = 0;
        BindSettlementDetails();
    }

    protected void grvSettle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvSettle.PageIndex = e.NewPageIndex;
        BindSettlementDetails();
    }

    protected void lblSettlementAmt_Click(object sender, EventArgs e)
    {
        try
        {
            MpeTrip.Show();
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblSettlementId = (Label)gvrow.FindControl("lblSettlementId");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync("RowerSettledIdGrid", new Settlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    SettlementId = lblSettlementId.Text.Trim()
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
                            lblRowerSettlementId.Text = lblSettlementId.Text.Trim();

                            gvRowerTripDetails.Visible = true;
                            gvRowerTripDetails.DataSource = dt;
                            gvRowerTripDetails.DataBind();

                            decimal totSettleAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ActualRowerCharge")));

                            gvRowerTripDetails.FooterRow.Cells[4].Text = "Total";
                            gvRowerTripDetails.FooterRow.Cells[5].Text = totSettleAmt.ToString();
                        }
                        else
                        {
                            MpeTrip.Hide();
                            gvRowerTripDetails.Visible = false;
                            return;
                        }
                    }
                    else
                    {
                        MpeTrip.Hide();
                        gvRowerTripDetails.Visible = false;
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

    protected void imgCloseTicket_Click(object sender, ImageClickEventArgs e)
    {
        MpeTrip.Hide();
        BindSettlementDetails();
    }

    public void BindSettlementDetails()
    {
        try
        {
            pnlTrip.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync("RowerSettledGrid", new Settlement()
                {
                    RowerId = "0",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim()
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
                            pnlTrip.Visible = true;
                            divGridList.Visible = true;
                            grvSettle.DataSource = dt;
                            grvSettle.DataBind();
                            grvSettle.Visible = true;
                            btnGeneratePrint.Visible = true;

                            decimal sum = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                sum += decimal.Parse(dt.Rows[i]["SettlementAmt"].ToString());
                            }

                            grvSettle.FooterRow.Cells[3].Text = "Total";
                            grvSettle.FooterRow.Cells[3].Font.Bold = true;
                            grvSettle.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                            grvSettle.FooterRow.Cells[3].Font.Size = 20;

                            grvSettle.FooterRow.Cells[4].Text = sum.ToString();
                            grvSettle.FooterRow.Cells[4].ForeColor = Color.Green;
                            grvSettle.FooterRow.Cells[4].Font.Bold = true;
                            grvSettle.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            grvSettle.FooterRow.Cells[4].Font.Size = 20;
                        }
                        else
                        {
                            grvSettle.Visible = false;
                        }
                    }
                    else
                    {
                        MpeTrip.Dispose();
                        pnlTrip.Visible = false;
                        divGridList.Visible = false;
                        btnGeneratePrint.Visible = false;
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
    protected void btnGeneratePrint_Click(object sender, EventArgs e)
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

                var ChallanAb = new Settlement()
                {
                    RowerId = "0",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RowerSettledGrid", ChallanAb).Result;

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
                            objReportDocument.Load(Server.MapPath("RptRowerSettlement.rpt"));
                            objReportDocument.SetDataSource(dt);

                            decimal total = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                total += decimal.Parse(dt.Rows[i]["SettlementAmt"].ToString());
                            }

                            string FromTodate = string.Empty;
                            if (txtFromDate.Text.Trim() == txtToDate.Text.Trim())
                            {
                                FromTodate = txtFromDate.Text.Trim();
                            }
                            else
                            {
                                FromTodate = txtFromDate.Text.Trim() + " to " + txtToDate.Text.Trim();
                            }

                            objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                            objReportDocument.SetParameterValue(1, "Rower Settlement - Sales Entries details For "+ FromTodate);
                            objReportDocument.SetParameterValue(2, total);
                            objReportDocument.SetParameterValue(3, Session["CorpName"].ToString());
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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

    protected void rbtnRowerMonthWise_SelectedIndexChanged(object sender, EventArgs e)
    {
        
       
        if (rbtnRowerMonthWise.SelectedValue == "1")
        {
            divRowerDateWise.Visible = true;
            divRowerMonthWise.Visible = false;
            BindSettlementDetails();
        }
        else
        {
            divRowerDateWise.Visible = false;
            divRowerMonthWise.Visible = true;
            grvSettle.Visible = false;
            GetResFinYear();
        }
        pnlTrip.Visible = false;
        MpeTrip.Dispose();
    }
    public void GetResFinYear()
    {
        try
        {
            ddlRowerFinYear.Items.Clear();
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
            ddlRowerFinYear.Items.Insert(0, new ListItem("Select", "0"));
            for (int iCount = 1; iCount <= 15; iCount++)
            {
                sFinYear = iFinYear.ToString() + "-" + (iFinYear + 1).ToString();
                iFinYear = iFinYear - 1;

                ddlRowerFinYear.Items.Add(sFinYear);
                //ddlRowerFinYear.Items.Insert(0, new ListItem("Select", "0"));
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
    protected void ddlRowerFinYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRowerFinYear.SelectedValue != "0")
        {
            divRowerMonth.Visible = true;
            int sMonth = Convert.ToInt32(ddlRowerMonth.SelectedValue);
            string[] year = ddlRowerFinYear.SelectedValue.Split('-');
            int days = 0;
            txtMonthFromDate.Attributes.Add("readonly", "readonly");
            txtMonthToDate.Attributes.Add("readonly", "readonly");
            if (sMonth >= 4 && sMonth <= 12)
            {
                days = DateTime.DaysInMonth(Convert.ToInt32(year[0]), sMonth);
                txtMonthFromDate.Text = "01/" + ddlRowerMonth.SelectedValue.Trim() + "/" + year[0];
                txtMonthToDate.Text = days + "/" + ddlRowerMonth.SelectedValue.Trim() + "/" + year[0];
            }
            else
            {
                days = DateTime.DaysInMonth(Convert.ToInt32(year[1]), sMonth);
                txtMonthFromDate.Text = "01/" + ddlRowerMonth.SelectedValue.Trim() + "/" + year[1];
                txtMonthToDate.Text = days + "/" + ddlRowerMonth.SelectedValue.Trim() + "/" + year[1];
            }
        }
        else
        {
            ddlRowerMonth.SelectedValue = "04";
            divRowerMonth.Visible = false;
        }
    }

    protected void ddlRowerMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        int sMonth = Convert.ToInt32(ddlRowerMonth.SelectedValue);
        string[] year = ddlRowerFinYear.SelectedValue.Split('-');
        int days = 0;
        txtMonthFromDate.Attributes.Add("readonly", "readonly");
        txtMonthToDate.Attributes.Add("readonly", "readonly");
        if (sMonth >= 4 && sMonth <= 12)
        {
            days = DateTime.DaysInMonth(Convert.ToInt32(year[0]), sMonth);
            txtMonthFromDate.Text = "01/" + ddlRowerMonth.SelectedValue.Trim() + "/" + year[0];
            txtMonthToDate.Text = days + "/" + ddlRowerMonth.SelectedValue.Trim() + "/" + year[0];
        }
        else
        {
            days = DateTime.DaysInMonth(Convert.ToInt32(year[1]), sMonth);
            txtMonthFromDate.Text = "01/" + ddlRowerMonth.SelectedValue.Trim() + "/" + year[1];
            txtMonthToDate.Text = days + "/" + ddlRowerMonth.SelectedValue.Trim() + "/" + year[1];
        }
    }
    protected void btnAbstractPrint_Click(object sender, EventArgs e)
    {
        MpeTrip.Dispose();
        pnlTrip.Visible = false;
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

                var ChallanAb = new Settlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtMonthFromDate.Text.Trim(),
                    ToDate = txtMonthToDate.Text.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RowerMonthSettledGrid", ChallanAb).Result;

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
                            objReportDocument.Load(Server.MapPath("RptRowerMonthWiseSettlement.rpt"));
                            objReportDocument.SetDataSource(dt);

                            decimal total = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                total += decimal.Parse(dt.Rows[i]["SettlementAmt"].ToString());
                            }

                            string FromTodate = string.Empty;
                            if (txtMonthFromDate.Text.Trim() == txtMonthToDate.Text.Trim())
                            {
                                FromTodate = txtMonthFromDate.Text.Trim();
                            }
                            else
                            {
                                FromTodate = txtMonthFromDate.Text.Trim() + " to " + txtMonthToDate.Text.Trim();
                            }

                            objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                            objReportDocument.SetParameterValue(2, "Rower Settlement - Month Wise Sales Entries details Between " + FromTodate);
                            objReportDocument.SetParameterValue(1, total);
                            objReportDocument.SetParameterValue(3, Session["CorpName"].ToString());
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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

    public class Settlement
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SettlementId { get; set; }

        public string RowerId { get; set; }
    }       
}