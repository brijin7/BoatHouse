using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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

public partial class Reports_RptCreditTripWiseDetails : System.Web.UI.Page
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
                if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
                {
                    GetFinYear();
                    GetBoatType();

                    rbtnDateOrMonthWise.SelectedValue = "0";
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFromDate.Attributes.Add("readonly", "readonly");

                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Attributes.Add("readonly", "readonly");

                    divShow.Visible = true;
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    /********************************** GET Financial Year********************************/
    public void GetFinYear()
    {
        try
        {
            ddlFinyear.Items.Clear();
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

            for (int iCount = 0; iCount < 15; iCount++)
            {
                sFinYear = iFinYear.ToString() + "-" + (iFinYear + 1).ToString();
                iFinYear = iFinYear - 1;

                ddlFinyear.Items.Add(sFinYear);

                if (sFinYear == "2020-2021")
                {
                    return;
                }
                ddlFinyear.Items.Insert(0, new ListItem("Select", "0"));
            }


        }
        catch (Exception ex)
        {
            Page.Controls.Add(new LiteralControl("<script>alert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');</script>"));
            return;
        }
    }

    /********************************** Get Boat Type********************************/
    public void GetBoatType()
    {
        try
        {
            ddlDateWiseBoatType.Items.Clear();
            ddlMonthWiseBoatType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatTrip = new tripWiseReport()
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
                            ddlDateWiseBoatType.DataSource = dt;
                            ddlDateWiseBoatType.DataValueField = "BoatTypeId";
                            ddlDateWiseBoatType.DataTextField = "BoatType";
                            ddlDateWiseBoatType.DataBind();

                            ddlMonthWiseBoatType.DataSource = dt;
                            ddlMonthWiseBoatType.DataValueField = "BoatTypeId";
                            ddlMonthWiseBoatType.DataTextField = "BoatType";
                            ddlMonthWiseBoatType.DataBind();
                        }
                        else
                        {
                            ddlDateWiseBoatType.DataBind();
                            ddlMonthWiseBoatType.DataBind();
                        }

                        ddlDateWiseBoatType.Items.Insert(0, new ListItem("All", "0"));
                        ddlMonthWiseBoatType.Items.Insert(0, new ListItem("All", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /*********************************Radio Button Selected Index Change Event******************************************/
    protected void rbtnDateOrMonthWise_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnDateOrMonthWise.SelectedValue == "0")
        {
            divDateWiseRpt.Visible = true;
            divMonthWiseRpt.Visible = false;
        }
        else
        {
            divDateWiseRpt.Visible = false;
            divMonthWiseRpt.Visible = true;
        }
    }
    /*****************************************DropDown Selected Index Change Event*************************************/
    protected void ddlFinyear_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlFinyear.SelectedValue == "0")
        {
            divMonth.Visible = false;
            divMonthBtn.Visible = false;
            divMonthWiseBoatType.Visible = false;
        }
        else
        {
            divMonth.Visible = true;
            divMonthBtn.Visible = true;
            divMonthWiseBoatType.Visible = true;
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMonth.SelectedValue != "0")
        {
            int sMonth = Convert.ToInt32(ddlMonth.SelectedValue);
            string[] year = ddlFinyear.SelectedValue.Split('-');
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
    }

    /********************************************Binding Crystal Report DateWise*************************************************/
    public void DateWiseRpt()
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

                var DateWiseRpt = new tripWiseReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlDateWiseBoatType.SelectedValue.Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RprCreditTripWiseDetailsDateAndMonthWise", DateWiseRpt).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                        return;
                    }
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(Response1);

                    if (dtExists.Rows.Count > 0)
                    {
                        objReportDocument.Load(Server.MapPath("~/Reports/RprCreditTripWiseDetails.rpt"));
                        objReportDocument.SetDataSource(dtExists);
                        objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString().Trim());
                        if (ddlDateWiseBoatType.SelectedValue == "0")
                        {
                            objReportDocument.SetParameterValue(1, ddlDateWiseBoatType.SelectedItem.Text.Trim() + " " + "Boats");
                        }
                        else
                        {
                            objReportDocument.SetParameterValue(1, ddlDateWiseBoatType.SelectedItem.Text.Trim());
                        }
                        objReportDocument.SetParameterValue(2, txtFromDate.Text.Trim() + "-" + txtToDate.Text.Trim());
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
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    return;
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

    /********************************************Binding Crystal Report MonthWise*************************************************/
    public void MonthWiseRpt()
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

                var DateWiseRpt = new tripWiseReport()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlMonthWiseBoatType.SelectedValue.Trim(),
                    FromDate = txtmFromDate.Text.Trim(),
                    ToDate = txtmToDate.Text.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RprCreditTripWiseDetailsDateAndMonthWise", DateWiseRpt).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                        return;
                    }
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(Response1);

                    if (dtExists.Rows.Count > 0)
                    {
                        objReportDocument.Load(Server.MapPath("~/Reports/RprCreditTripWiseDetails.rpt"));
                        objReportDocument.SetDataSource(dtExists);
                        objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString().Trim());
                        if (ddlMonthWiseBoatType.SelectedValue == "0")
                        {
                            objReportDocument.SetParameterValue(1, ddlMonthWiseBoatType.SelectedItem.Text.Trim() + " " + "Boats");
                        }
                        else
                        {
                            objReportDocument.SetParameterValue(1, ddlMonthWiseBoatType.SelectedItem.Text.Trim());
                        }
                        objReportDocument.SetParameterValue(2, txtmFromDate.Text.Trim() + "-" + txtmToDate.Text.Trim());
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
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    return;
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

    /*************************************OnClick For DateWise *****************************************/
    protected void btnDateWise_Click(object sender, EventArgs e)
    {
        DateWiseRpt();
    }

    /*************************************OnClick For MonthWise *****************************************/
    protected void btnMontWise_Click(object sender, EventArgs e)
    {
        MonthWiseRpt();
    }

    /************************************* Class *****************************************/
    public class tripWiseReport
    {
        public string BoatHouseId { get; set; }
        public string BoatTypeId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}