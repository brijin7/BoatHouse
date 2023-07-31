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

public partial class Reports_UBServiceWiseCollection : System.Web.UI.Page
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
                GetUserName();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }
    public void GetUserName()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ChallanAb = new ChallanUser()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),

                };
                HttpResponseMessage response = client.PostAsJsonAsync("ddlUserName", ChallanAb).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlUserName.DataSource = dtExists;
                        ddlUserName.DataValueField = "UserId";
                        ddlUserName.DataTextField = "UserName";
                        ddlUserName.DataBind();
                    }
                    else
                    {
                        ddlUserName.DataBind();
                    }
                    ddlUserName.Items.Insert(0, new ListItem("All", "0"));
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
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

                var ChallanAb = new Boating()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim(),
                    CreatedBy = ddlUserName.SelectedValue.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RptUserBasedBookingIdList", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        objReportDocument.Load(Server.MapPath("UBBookingIdReport.rpt"));
                        objReportDocument.SetDataSource(dtExists);

                        objReportDocument.SetParameterValue(0, "BoatHouse - " + Session["BoatHouseName"].ToString().Trim());
                        objReportDocument.SetParameterValue(1,  ddlUserName.SelectedItem.Text.Trim() + " - BookingId Based Sales Entries details Between " + txtBookingDate.Text.Trim() + " And " + txtBookingDate.Text.Trim());
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


    public class ChallanUser
    {
        public string BookingDate { get; set; }
        public string RadioButtonValue { get; set; }
        public string BoatNetAmount { get; set; }
        public string OtherNetAmount { get; set; }
        public string RestNetAmount { get; set; }
        public string BookingId { get; set; }
        public string Total { get; set; }
        public string UserName { get; set; }
        public string BoatHouseId { get; set; }
        public string PayMode { get; set; }
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string BookingType { get; set; }
        public string UserId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string FromDate { get; set; }
    }


    public class Boating
    {

        public string BoatHouseId { get; set; }
        public string BookingDate { get; set; }
        public string CreatedBy { get; set; }
    }

}

