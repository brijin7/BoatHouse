using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_DepositStatusReport : System.Web.UI.Page
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
                txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtBookingDate.Attributes.Add("readonly", "readonly");
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        if (ddlStatusType.SelectedValue == "0")
        {
            try
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

                        var ChallanAb = new StatusType()
                        {

                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            FromDate = txtBookingDate.Text.Trim(),
                            ToDate = txtBookingDate.Text.Trim(),
                            QueryType = "StatusTypeAllDeposit",
                            ServiceType = "",
                            BookingId = "",
                            Input1 = "",
                            Input2 = "",
                            Input3 = "",
                            Input4 = "",
                            Input5 = ""
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var GetResponse = response.Content.ReadAsStringAsync().Result;
                            if (GetResponse.Contains("No Records Found"))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + GetResponse + "');", true);
                                return;
                            }
                            else
                            {
                                var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                                if (dt.Rows.Count > 0)
                                {
                                    objReportDocument.Load(Server.MapPath("RptAllDepositStatus.rpt"));
                                    objReportDocument.SetDataSource(dt);

                                    objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                    objReportDocument.SetParameterValue(1, "Deposit Status - " + ddlStatusType.SelectedItem.Text.Trim() + " Sales Entries details For " + txtBookingDate.Text.Trim());

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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }

        if (ddlStatusType.SelectedValue == "1")
        {
            try
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

                        var ChallanAb = new StatusType()
                        {

                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            FromDate = txtBookingDate.Text.Trim(),
                            ToDate = txtBookingDate.Text.Trim(),
                            QueryType = "StatusTypeUnClaimed",
                            ServiceType = "",
                            BookingId = "",
                            Input1 = "",
                            Input2 = "",
                            Input3 = "",
                            Input4 = "",
                            Input5 = ""
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var GetResponse = response.Content.ReadAsStringAsync().Result;
                            if (GetResponse.Contains("No Records Found"))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + GetResponse + "');", true);
                                return;
                            }
                            else
                            {
                                var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                                if (dt.Rows.Count > 0)
                                {
                                    objReportDocument.Load(Server.MapPath("RptDepositStatusUnClaimed.rpt"));
                                    objReportDocument.SetDataSource(dt);

                                    objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                    objReportDocument.SetParameterValue(1, "Deposit Status - " + ddlStatusType.SelectedItem.Text.Trim() + " Sales Entries details For " + txtBookingDate.Text.Trim());
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }


        if (ddlStatusType.SelectedValue == "2")
        {
            try
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

                        var ChallanAb = new StatusType()
                        {

                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            FromDate = txtBookingDate.Text.Trim(),
                            ToDate = txtBookingDate.Text.Trim(),
                            QueryType = "StatusTypeTimeExtended",
                            ServiceType = "",
                            BookingId = "",
                            Input1 = "",
                            Input2 = "",
                            Input3 = "",
                            Input4 = "",
                            Input5 = ""
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var GetResponse = response.Content.ReadAsStringAsync().Result;
                            if (GetResponse.Contains("No Records Found"))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + GetResponse + "');", true);
                                return;
                            }
                            else
                            {
                                var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                                if (dt.Rows.Count > 0)
                                {
                                    objReportDocument.Load(Server.MapPath("RptDepositStatusTimeExtended.rpt"));
                                    objReportDocument.SetDataSource(dt);

                                    objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                    objReportDocument.SetParameterValue(1, "Deposit Status - " + ddlStatusType.SelectedItem.Text.Trim() + " Sales Entries details For " + txtBookingDate.Text.Trim());
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }

        if (ddlStatusType.SelectedValue == "3")
        {
            try
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

                        var ChallanAb = new StatusType()
                        {

                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            FromDate = txtBookingDate.Text.Trim(),
                            ToDate = txtBookingDate.Text.Trim(),
                            QueryType = "StatusTypeRefunded",
                            ServiceType = "",
                            BookingId = "",
                            Input1 = "",
                            Input2 = "",
                            Input3 = "",
                            Input4 = "",
                            Input5 = ""
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", ChallanAb).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var GetResponse = response.Content.ReadAsStringAsync().Result;
                            if (GetResponse.Contains("No Records Found"))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + GetResponse + "');", true);
                                return;
                            }
                            else
                            {
                                var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                                if (dt.Rows.Count > 0)
                                {
                                    objReportDocument.Load(Server.MapPath("RptDepositStatusRefunded.rpt"));
                                    objReportDocument.SetDataSource(dt);

                                    objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                                    objReportDocument.SetParameterValue(1, "Deposit Status - " + ddlStatusType.SelectedItem.Text.Trim() + " Sales Entries details For " + txtBookingDate.Text.Trim());
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
    }

    public class StatusType
    {
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string BookingId { get; set; }
    }
}