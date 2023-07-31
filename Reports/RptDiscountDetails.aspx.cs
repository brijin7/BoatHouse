using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Helpers;

public partial class Reports_RptDiscountDetails : System.Web.UI.Page
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

                var Discount = new Discount()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("RptDiscountDetails", Discount).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    if (Response1.Contains("No Records Found."))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + Response1 + "');", true);

                    }
                    else
                    {
                        var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            objReportDocument.Load(Server.MapPath("RptDiscount.rpt"));
                            objReportDocument.SetDataSource(dtExists);

                            objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString().Trim());
                            objReportDocument.SetParameterValue(1, "Discount Details for the Date Between " + txtFromDate.Text.Trim() + " & " + txtToDate.Text.Trim());
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

    public class Discount
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}