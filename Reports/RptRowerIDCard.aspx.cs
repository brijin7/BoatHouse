using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_RptRowerIDCard : System.Web.UI.Page
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
                BindRowerMaster();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindRowerMaster()
    {
        try
        {
            ddlRowerName.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var FormBody = new RptRowerId()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("ShowRowerMstrDetails", FormBody).Result;

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
                            ddlRowerName.DataSource = dt;
                            ddlRowerName.DataValueField = "RowerId";
                            ddlRowerName.DataTextField = "RowerName";
                            ddlRowerName.DataBind();
                        }
                        else
                        {
                            ddlRowerName.DataBind();
                        }
                        ddlRowerName.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Rower Details Not found.');", true);
                    }
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

    private byte[] GetImage(string url)
    {
        Stream stream = null;
        byte[] buf;

        try
        {
            WebProxy myProxy = new WebProxy();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                int len = (int)(response.ContentLength);
                buf = br.ReadBytes(len);
                br.Close();
            }

            stream.Close();
            response.Close();
        }
        catch (Exception)
        {
            buf = null;
        }

        return (buf);
    }

    public void BindIdCard()
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

                var FormBody = new RptRowerId()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    RowerId = ddlRowerName.SelectedValue.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("GetRowerDetails", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["RowerDetails"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        string ImageUrl = dtExists.Rows[0]["PhotoLink"].ToString();
                        if (ImageUrl == string.Empty || ImageUrl == " ")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Upload Photo to Generate ID Card.');", true);
                        }
                        else
                        {
                            StringBuilder _sb = new StringBuilder();
                            Byte[] _byte = this.GetImage(dtExists.Rows[0]["PhotoLink"].ToString());
                            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                            string sbBase64 = _sb.ToString();

                            string strFn1 = LoadBarcodeImage(Session["BoatHouseId"].ToString().Trim(),
                                 Session["BoatHouseName"].ToString().Trim(),
                                 dtExists.Rows[0]["RowerId"].ToString(),
                                 dtExists.Rows[0]["RowerName"].ToString(),
                                 dtExists.Rows[0]["MobileNo"].ToString());
                            if (strFn1 == "QR Code not Generated.")
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('QR Code Not Generated.');", true);
                            }
                            else
                            {
                                StringBuilder _sb1 = new StringBuilder();
                                Byte[] _byte1 = this.GetImage(strFn1);
                                _sb1.Append(Convert.ToBase64String(_byte1, 0, _byte1.Length));
                                string sbBase641 = _sb1.ToString();

                                DataTable dtnew = new DataTable();
                                dtnew.Columns.Add("RowerId");
                                dtnew.Columns.Add("RowerName");
                                dtnew.Columns.Add("MobileNo");
                                dtnew.Columns.Add("Address1");
                                dtnew.Columns.Add("Address2");
                                dtnew.Columns.Add("Zipcode");
                                dtnew.Columns.Add("City");
                                dtnew.Columns.Add("District");
                                dtnew.Columns.Add("State");
                                dtnew.Columns.Add("Image", typeof(byte[]));
                                dtnew.Columns.Add("Barcode", typeof(byte[]));
                                dtnew.Columns.Add("CorpLogo", typeof(byte[]));
                                string CorpLogo = Session["CorpLogo"].ToString();
                                using (var webClient = new WebClient())
                                {
                                    byte[] imageBytes = webClient.DownloadData(CorpLogo);

                                    dtnew.Rows.Add("Boat " + dtExists.Rows[0]["DriverCategory"].ToString() + " ID : " + dtExists.Rows[0]["RowerId"].ToString(),
                                  dtExists.Rows[0]["RowerName"].ToString(),
                                  dtExists.Rows[0]["MobileNo"].ToString(),
                                  dtExists.Rows[0]["Address1"].ToString(),
                                  dtExists.Rows[0]["Address2"].ToString(),
                                  dtExists.Rows[0]["Zipcode"].ToString(),
                                  dtExists.Rows[0]["City"].ToString(),
                                  dtExists.Rows[0]["District"].ToString(),
                                  dtExists.Rows[0]["State"].ToString(), _byte, _byte1, imageBytes);
                                }
                                objReportDocument.Load(Server.MapPath("RowerIdCard.rpt"));
                                objReportDocument.SetDataSource(dtnew);
                                objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString());
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

    public string LoadBarcodeImage(string sBoatHouseId, string sBoatHouseName, string sRowerId, string sRowerName, string sMobileNo)
    {
        string imgresponse = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var QRd = new RptRowerId()
                {
                    BoatHouseId = sBoatHouseId,
                    BoatHouseName = sBoatHouseName,
                    RowerId = sRowerId,
                    RowerName = sRowerName,
                    MobileNo = sMobileNo
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RowerQR", QRd).Result;

                if (response.IsSuccessStatusCode)
                {
                    var GetResponse = response.Content.ReadAsStringAsync().Result;
                    string Status = JObject.Parse(GetResponse)["status"].ToString();
                    string ResponseMsg2 = JObject.Parse(GetResponse)["imageURL"].ToString();

                    if (Status == "Success.")
                    {
                        imgresponse = ResponseMsg2;
                    }
                    else
                    {
                        imgresponse = "QR Code not Generated.";
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);

        }
        return imgresponse;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        if (ddlRowerName.SelectedValue == "0")
        {
            BindIdCardBooklet();
        }
        else
        {
            BindIdCard();
        }

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        BindRowerMaster();
    }

    protected void lbtnRower_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Boating/RowerMaster.aspx");
    }

    public void BindIdCardBooklet()
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

                var FormBody = new RptRowerId()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                };
                HttpResponseMessage response = client.PostAsJsonAsync("ShowRowerMstrDetails", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {

                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        DataTable dtnew = new DataTable();
                        dtnew.Columns.Add("RowerId");
                        dtnew.Columns.Add("RowerName");
                        dtnew.Columns.Add("MobileNo");
                        dtnew.Columns.Add("Address1");
                        dtnew.Columns.Add("Address2");
                        dtnew.Columns.Add("Zipcode");
                        dtnew.Columns.Add("City");
                        dtnew.Columns.Add("District");
                        dtnew.Columns.Add("State");
                        dtnew.Columns.Add("Barcode", typeof(byte[]));
                        dtnew.Columns.Add("CorpLogo", typeof(byte[]));

                        for (int j = 0; j < dtExists.Rows.Count; j++)
                        {
                            string strFn2 = LoadBarcodeImage(Session["BoatHouseId"].ToString().Trim(),
                            Session["BoatHouseName"].ToString().Trim(),
                            dtExists.Rows[j]["RowerId"].ToString(),
                            dtExists.Rows[j]["RowerName"].ToString(),
                            dtExists.Rows[j]["MobileNo"].ToString());
                            if (strFn2 == "QR Code not Generated.")
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('QR Code Not Generated.');", true);
                                return;
                            }
                            else
                            {
                                StringBuilder _sb2 = new StringBuilder();
                                Byte[] _byte2 = this.GetImage(strFn2);
                                _sb2.Append(Convert.ToBase64String(_byte2, 0, _byte2.Length));
                                string sbBase642 = _sb2.ToString();

                                string Category = string.Empty;
                                if (dtExists.Rows[j]["DriverCategory"].ToString() == "")
                                {
                                    Category = "Rower";
                                }
                                else
                                {
                                    Category = dtExists.Rows[j]["DriverCategory"].ToString();
                                }
                                string CorpLogo = Session["CorpLogo"].ToString();
                                using (var webClient = new WebClient())
                                {
                                    byte[] imageBytes = webClient.DownloadData(CorpLogo);

                                    dtnew.Rows.Add("Boat " + Category + " ID : " + dtExists.Rows[j]["RowerId"].ToString(),
                                dtExists.Rows[j]["RowerName"].ToString(),
                                dtExists.Rows[j]["MobileNo"].ToString(),
                                dtExists.Rows[j]["Address1"].ToString(),
                                dtExists.Rows[j]["Address2"].ToString(),
                                dtExists.Rows[j]["Zipcode"].ToString(),
                                dtExists.Rows[j]["City"].ToString(),
                                dtExists.Rows[j]["District"].ToString(),
                                dtExists.Rows[j]["State"].ToString(), _byte2, imageBytes);
                                }
                            }
                        }
                        objReportDocument.Load(Server.MapPath("RptRowerIdCardBooklet.rpt"));
                        objReportDocument.SetDataSource(dtnew);
                        objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString());
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

    public class RptRowerId
    {
        public string BoatHouseId { get; set; }
        public string RowerId { get; set; }
        public string RowerName { get; set; }
        public string MobileNo { get; set; }
        public string BoatHouseName { get; set; }
    }
}