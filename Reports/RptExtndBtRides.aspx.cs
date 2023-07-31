using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Globalization;
using System.Web.Helpers;

/// <summary>
/// Silambarasu D , 14 SEP 2021
/// </summary>
public partial class Boating_RptExtndBtRides : System.Web.UI.Page
{
    public string GetAntiForgeryToken()
    {
        string cookieToken, formToken;
        AntiForgery.GetTokens(null, out cookieToken, out formToken);
        ViewState["__AntiForgeryCookie"] = cookieToken;
        return formToken;

    }

    /// <summary>
    /// Page Load Method 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                txtFromDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                GetRowerName();
                bindBoatExtnsnRides();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    /// <summary>
    /// Submit Click method used to bind data to grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        bindBoatExtnsnRides();
    }

    /// <summary>
    /// Reset Click method used to reset all the inputs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        clearinputs();
    }

    /// <summary>
    /// used to generate to crystal report 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPDF_Click(object sender, EventArgs e)
    {
        GeneratePDF();
    }

    /// <summary>
    /// Used To bind the record
    /// </summary>
    public void bindBoatExtnsnRides()
    {
        string ServiceType = string.Empty;

        if (ddlrowerName.SelectedIndex == 0)
        {
            ServiceType = "All";
        }
        else
        {
            ServiceType = "ParticularRower";
        }
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var rptExtndBtRides = new ExtndBtRides()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
                    QueryType = "ExtendedBoatRides",
                    ServiceType = ServiceType.Trim(),
                    BookingId = "",
                    Input1 = ddlrowerName.SelectedValue,
                    Input2 = ddlExtnsnType.SelectedValue,
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", rptExtndBtRides).Result;

                if (response.IsSuccessStatusCode)
                {
                    var GetResponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                    if (dt.Rows.Count > 0)
                    {
                        btnPDF.Visible = true;
                        GvExtnsnBtRides.DataSource = dt;
                        GvExtnsnBtRides.DataBind();
                        divGridList.Visible = true;
                        GvExtnsnBtRides.Visible = true;
                        lblGridMsg.Visible = false;
                    }
                    else
                    {
                        GvExtnsnBtRides.DataBind();
                        divGridList.Visible = true;
                        lblGridMsg.Visible = true;
                        lblGridMsg.Text = "No Records Found";
                        GvExtnsnBtRides.Visible = false;
                        btnPDF.Visible = false;

                    }

                }
                else
                {
                    GvExtnsnBtRides.DataBind();
                    divGridList.Visible = true;
                    lblGridMsg.Visible = true;
                    lblGridMsg.Text = "No Records Found";
                    GvExtnsnBtRides.Visible = false;
                    btnPDF.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    /// <summary>
    /// Used To generate crystal report
    /// </summary>
    public void GeneratePDF()
    {
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        string ServiceType = string.Empty;

        if (ddlrowerName.SelectedIndex == 0)
        {
            ServiceType = "All";
        }
        else
        {
            ServiceType = "ParticularRower";
        }
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var rptExtndBtRides = new ExtndBtRides()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
                    QueryType = "ExtendedBoatRides",
                    ServiceType = ServiceType.Trim(),
                    BookingId = "",
                    Input1 = ddlrowerName.SelectedValue,
                    Input2 = ddlExtnsnType.SelectedValue,
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", rptExtndBtRides).Result;

                if (response.IsSuccessStatusCode)
                {
                    var GetResponse = response.Content.ReadAsStringAsync().Result;
                    if (GetResponse.Contains("No Records Found"))
                    {
                        //divNoOfTrips.Visible = true;
                        GvExtnsnBtRides.Visible = false;
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + GetResponse + "');", true);
                        return;
                    }
                    else
                    {
                        var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                        if (dt.Rows.Count > 0)
                        {

                            objReportDocument.Load(Server.MapPath("ExtensionBoatRides.rpt"));
                            objReportDocument.SetDataSource(dt);

                            string date = DateTime.Now.ToString("dd'/'MM'/'yyyy") + ' ' + DateTime.Now.ToString("h:mm:ss tt");

                            string FromTodate = string.Empty;
                            if (txtFromDate.Text.Trim() == txtToDate.Text.Trim())
                            {
                                FromTodate = txtFromDate.Text.Trim();
                            }
                            else
                            {
                                FromTodate = txtFromDate.Text.Trim() + " to " + txtToDate.Text.Trim();
                            }

                            objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString().Trim());
                            objReportDocument.SetParameterValue(1, FromTodate);
                            objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
                            //objReportDocument.SetParameterValue(2, date);

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
                            GvExtnsnBtRides.Visible = false;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Response + "');", true);
                    GvExtnsnBtRides.Visible = false;
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

    /// <summary>
    /// Used to GetRowerName
    /// </summary>
    public void GetRowerName()
    {
        try
        {
            ddlrowerName.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var RowerCharges = new ExtndBtRides()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlRowerName", RowerCharges).Result;

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
                            ddlrowerName.DataSource = dt;
                            ddlrowerName.DataValueField = "RowerId";
                            ddlrowerName.DataTextField = "RowerName";
                            ddlrowerName.DataBind();

                        }
                        else
                        {
                            ddlrowerName.DataBind();
                        }

                        ddlrowerName.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        divGridList.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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

    /// <summary>
    /// Used to ClearInputs
    /// </summary>
    public void clearinputs()
    {
        ddlrowerName.SelectedIndex = 0;
        ddlExtnsnType.SelectedIndex = 0;
        txtFromDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
        txtFromDate.Attributes.Add("readonly", "readonly");
        txtToDate.Attributes.Add("readonly", "readonly");
        bindBoatExtnsnRides();
    }

    public class ExtndBtRides
    {
        public string BookingId { get; set; }
        public string BoatReferenceNo { get; set; }
        public string BoatHouseId { get; set; }
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }

}