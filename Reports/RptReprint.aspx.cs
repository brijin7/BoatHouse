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

public partial class Reports_RptReprint : System.Web.UI.Page
{
    IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);

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
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
            }

        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDateTime(txtFromDate.Text, objEnglishDate) <= Convert.ToDateTime(txtToDate.Text, objEnglishDate))
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

                        var ChallanAb = new Reprint()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            FromDate = txtFromDate.Text,
                            ToDate = txtToDate.Text,
                            ServiceName = ddlServiceType.SelectedItem.Text
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("RptReprint", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var Response1 = response.Content.ReadAsStringAsync().Result;
                            if (Response1.Contains("No Records Found."))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                                ddlServiceType.SelectedIndex = 0;
                                return;
                            }
                            var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                            DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dtExists.Rows.Count > 0)
                            {
                                objReportDocument.Load(Server.MapPath("RptReprint.rpt"));
                                objReportDocument.SetDataSource(dtExists);
                                objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                objReportDocument.SetParameterValue(1, "Reprint Report - " + ddlServiceType.SelectedItem.Text.Trim() + " Reprint Entries Between " + txtFromDate.Text.Trim() + " And " + txtToDate.Text.Trim());
                                objReportDocument.SetParameterValue(2, Session["CorpName"].ToString());
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
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('To Date Should Be Greater Than OR Equal To From Date');", true);
                ddlServiceType.SelectedIndex = 0;
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public class Reprint
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceName { get; set; }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
}