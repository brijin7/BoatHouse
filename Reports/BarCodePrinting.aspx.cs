using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

public partial class QR_Code_Generator_BarCodePrinting : System.Web.UI.Page
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
                getBoatHouseAll();
            }
            if (ddlServices.SelectedIndex == 0 || ddlServices.SelectedIndex == 1)
            {
                serviceType.Visible = false;
            }
            else
            {
                serviceType.Visible = true;
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void getBoatHouseAll()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("ddlBoatHouse/ListAll?CorpId=" + Session["CorpId"].ToString() +"").Result;


                if (response.IsSuccessStatusCode)
                {
                    var Getresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Getresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Getresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlBoatHouseName.DataSource = dt;
                            ddlBoatHouseName.DataValueField = "BoatHouseId";
                            ddlBoatHouseName.DataTextField = "BoatHouseName";
                            ddlBoatHouseName.DataBind();
                        }
                        else
                        {
                            ddlBoatHouseName.DataBind();
                        }
                        ddlBoatHouseName.Items.Insert(0, new ListItem("Select Boat House Name", "0"));
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        return;
                    }

                    ddlBoatHouseName.Enabled = false;
                    ddlBoatHouseName.SelectedValue = Session["BoatHouseId"].ToString().Trim();
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

    protected void ddlBoatHouseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlServices.SelectedIndex = 0;

        if (ddlBoatHouseName.SelectedIndex == 0)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Boat House Name');", true);
            ddlBoattypes.Items.Clear();
            serviceType.Visible = false;
            return;
        }

        if (ddlBoattypes.SelectedValue != "")
        {
            ddlBoattypes.SelectedIndex = 0;
            ddlBoattypes.Items.Clear();
        }
    }

    public void GetBoatType()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatRateMaster = new BH_RowerBoatAssign()
                {
                    BoatHouseId = ddlBoatHouseName.SelectedValue

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlBoatType", BoatRateMaster).Result;

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
                            ddlBoattypes.DataSource = dt;
                            ddlBoattypes.DataValueField = "BoatTypeId";
                            ddlBoattypes.DataTextField = "BoatType";
                            ddlBoattypes.DataBind();
                        }
                        else
                        {
                            ddlBoattypes.DataBind();
                            ddlBoattypes.Items.Clear();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boating Details Not Found...!');", true);
                            return;
                        }
                        ddlBoattypes.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoattypes.Items.Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boating Details Not Found...!');", true);
                        return;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }

    public void getRestaurant()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var RowerCharges = new BH_RowerBoatAssign()
                {
                    BoatHouseId = ddlBoatHouseName.SelectedValue

                };
                HttpResponseMessage response = client.PostAsJsonAsync("FoodCategory/BHId", RowerCharges).Result;


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
                            ddlBoattypes.DataSource = dt;
                            ddlBoattypes.DataValueField = "CategoryId";
                            ddlBoattypes.DataTextField = "CategoryName";
                            ddlBoattypes.DataBind();

                        }
                        else
                        {
                            ddlBoattypes.DataBind();
                            ddlBoattypes.Items.Clear();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Restaurant Details Not Found...!');", true);
                            return;
                        }
                        ddlBoattypes.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoattypes.Items.Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Restaurant Details Not Found...!');", true);
                        return;

                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetOtherservices()
    {
        try
        {
            ddlBoattypes.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new BH_RowerBoatAssign()
                {
                    BoatHouseId = ddlBoatHouseName.SelectedValue
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CategoryList/BhId", CatType).Result;

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
                            ddlBoattypes.DataSource = dt;
                            ddlBoattypes.DataValueField = "ConfigId";
                            ddlBoattypes.DataTextField = "ConfigName";
                            ddlBoattypes.DataBind();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Service Category Details Not Found...!');", true);
                            ddlBoattypes.Items.Clear();
                            return;
                        }
                        ddlBoattypes.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoattypes.Items.Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Service Category Details Not Found...!');", true);
                        return;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ddlServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBoatHouseName.SelectedIndex == 0)
        {
            ddlServices.SelectedIndex = 0;
            ddlBoattypes.Items.Clear();
            serviceType.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Boat House Name');", true);
            return;
        }
        if (ddlServices.SelectedValue == "0")
        {
            ddlBoattypes.Items.Clear();
            serviceType.Visible = false;
        }
        if (ddlServices.SelectedValue == "1")
        {
            GetBoatType();
        }
        if (ddlServices.SelectedValue == "2")
        {
            getRestaurant();
        }
        if (ddlServices.SelectedValue == "3")
        {
            GetOtherservices();
        }
        if (ddlServices.SelectedValue == "5")
        {
            ddlBoattypes.Items.Clear();
            serviceType.Visible = false;
        }

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (ddlServices.SelectedValue == "1" && ddlBoattypes.SelectedValue == "0")
        {
            GetBoatTypeAll();
            BindIdCardAll();
            hfseatertype.Value = "";
            hfAllSeaterId.Value = "";
            hfAllSeater.Value = "";
            hfAllBoatType.Value = "";
            hfallReport.Value = "";
        }
        else
        {
            getseater();
            BindIdCard();
            hfseatertype.Value = "";
        }
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
                   

                    string strFn1 = LoadBarcodeImage(Session["BoatHouseId"].ToString().Trim(),
                                    ddlServices.SelectedValue.Trim(),
                                   ddlBoattypes.SelectedValue.Trim());
                string CorpLogo = Session["CorpLogo"].ToString();
                using (var webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(CorpLogo);

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

                        dtnew.Columns.Add("BarCode", typeof(byte[]));
                        dtnew.Columns.Add("CorpLogo", typeof(byte[]));

                        dtnew.Rows.Add(_byte1, imageBytes);


                        objReportDocument.Load(Server.MapPath("RptBarCodeGenerator.rpt"));
                        objReportDocument.SetDataSource(dtnew);
                        objReportDocument.SetParameterValue(0, ddlBoatHouseName.SelectedItem.Text);
                        objReportDocument.SetParameterValue(1, ddlServices.SelectedItem.Text);
                        if (ddlServices.SelectedIndex == 0)
                        {
                            objReportDocument.SetParameterValue(2, "");
                            objReportDocument.SetParameterValue(3, "Scan To Sign In");
                        }
                        else if (ddlServices.SelectedValue == "5")
                        {
                            objReportDocument.SetParameterValue(2, "");
                            objReportDocument.SetParameterValue(3, "Android App");
                        }
                        else
                        {
                            if (ddlServices.SelectedValue == "1" && ddlBoattypes.SelectedValue != "0")
                            {
                                if (hfseatertype.Value != "")
                                {
                                    objReportDocument.SetParameterValue(2, "" + ddlBoattypes.SelectedItem.Text + " : " + hfseatertype.Value + "- Seater");
                                    hfseatertype.Value = "";
                                }
                                else
                                {
                                    objReportDocument.SetParameterValue(2, "" + ddlBoattypes.SelectedItem.Text + "");
                                    hfseatertype.Value = "";
                                }
                                objReportDocument.SetParameterValue(3, "Scan & Book");
                            }
                            else
                            {
                                objReportDocument.SetParameterValue(2, "" + ddlBoattypes.SelectedItem.Text + "");
                                objReportDocument.SetParameterValue(3, "Scan & Book");
                            }
                        }
                       
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

    public string LoadBarcodeImage(string sBoatHouseId, string sServiceType, string sBoatType)
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
                if (ddlServices.SelectedIndex == 0)
                {
                    var QRd = new BH_RowerBoatAssign()
                    {
                        String1 = "https://paypre.in/boating"
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("http://qr.paypre.in/QRGenerator/SingleStrQR", QRd).Result;
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
                else if (ddlServices.SelectedValue == "5")
                {
                    var QRd = new BH_RowerBoatAssign()
                    {
                        String1 = "https://play.google.com/store/apps/details?id=com.ttdc.tourism"

                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("http://qr.paypre.in/QRGenerator/SingleStrQR", QRd).Result;
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
                else
                {
                    var QRd = new BH_RowerBoatAssign()
                    {
                        String1 = "https://paypre.in/boating/ER/EScan.aspx?BHId=" + ddlBoatHouseName.SelectedValue.Trim() + "&ST=" + ddlServices.SelectedValue.Trim() + "&BT=" + ddlBoattypes.SelectedValue.Trim() + ""

                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("http://qr.paypre.in/QRGenerator/SingleStrQR", QRd).Result;

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

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);

        }
        return imgresponse;
    }

    public void getseater()
    {
        try
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string boatTypeIds = string.Empty;

                    var BoatSearch = new BH_RowerBoatAssign()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        PremiumStatus = "N",
                        BoatTypeId = ddlBoattypes.SelectedValue.Trim(),
                        BookingDate = DateTime.Today.ToString("dd/MM/yyyy")

                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("BoatBooking/BoatAvailableList", BoatSearch).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var BoatLst = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();
                        string name = string.Empty;
                        string Removeseater = string.Empty;
                        string Finalname = string.Empty;
                        if (StatusCode == 1)
                        {
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    name = dt.Rows[i]["SeaterType"].ToString() + ",";
                                    Removeseater = name.Remove(2).Trim();
                                    Finalname += Removeseater + ",";
                                }
                                hfseatertype.Value = Finalname.TrimEnd(',');
                            }
                            else
                            {
                                hfseatertype.Value = string.Empty;
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Details Not Found !');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert(" + ResponseMsg + "');", true);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindIdCardAll()
    {

        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        hfallReport.Value = hfAllBoatType.Value.Trim();
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                string CorpLogo = Session["CorpLogo"].ToString();
                string strFn1 = LoadBarcodeImage(Session["BoatHouseId"].ToString().Trim(),
                                ddlServices.SelectedValue.Trim(),
                               ddlBoattypes.SelectedValue.Trim());
                using (var webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(CorpLogo);
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

                        dtnew.Columns.Add("BarCode", typeof(byte[]));
                        dtnew.Columns.Add("CorpLogo", typeof(byte[]));

                        dtnew.Rows.Add(_byte1, imageBytes);

                        objReportDocument.Load(Server.MapPath("RptBarCodeGeneratorAll.rpt"));
                        objReportDocument.SetDataSource(dtnew);
                        objReportDocument.SetParameterValue(0, ddlBoatHouseName.SelectedItem.Text);
                        objReportDocument.SetParameterValue(1, ddlServices.SelectedItem.Text);
                        objReportDocument.SetParameterValue(2, "" + hfallReport.Value.TrimEnd(',') + "");
                        objReportDocument.SetParameterValue(3, "Scan & Book");
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

    public void getseaterAll()
    {
        try
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string boatTypeIds = string.Empty;

                    var BoatSearch = new BH_RowerBoatAssign()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        PremiumStatus = "N",
                        BoatTypeId = hfAllSeaterId.Value.Trim(),
                        BookingDate = DateTime.Today.ToString("dd/MM/yyyy")

                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("BoatBooking/BoatAvailableList", BoatSearch).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var BoatLst = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();
                        string name = string.Empty;
                        string Removeseater = string.Empty;
                        string Finalname = string.Empty;
                        if (StatusCode == 1)
                        {
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    name = dt.Rows[i]["SeaterType"].ToString() + ",";
                                    Removeseater = name.Remove(2).Trim();
                                    Finalname += Removeseater + ",";
                                }
                                hfseatertype.Value = Finalname.TrimEnd(',');
                                hfAllBoatType.Value += hfAllSeater.Value + " " + ":" + " " + hfseatertype.Value + " " + "- Seater" + "\n" + "\n";

                            }
                            else
                            {
                                hfseatertype.Value = string.Empty;
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Details Not Found !');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert(" + ResponseMsg + "');", true);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void GetBoatTypeAll()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatRateMaster = new BH_RowerBoatAssign()
                {
                    BoatHouseId = ddlBoatHouseName.SelectedValue

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlBoatType", BoatRateMaster).Result;

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
                            for (int i = 0; dt.Rows.Count > i; i++)
                            {
                                hfAllSeaterId.Value = dt.Rows[i]["BoatTypeId"].ToString();
                                hfAllSeater.Value = dt.Rows[i]["BoatType"].ToString();
                                getseaterAll();
                            }

                            ddlBoattypes.DataSource = dt;
                            ddlBoattypes.DataValueField = "BoatTypeId";
                            ddlBoattypes.DataTextField = "BoatType";
                            ddlBoattypes.DataBind();
                        }
                        else
                        {
                            ddlBoattypes.DataBind();
                            ddlBoattypes.Items.Clear();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boating Details Not Found...!');", true);
                            return;
                        }
                        ddlBoattypes.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoattypes.Items.Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boating Details Not Found...!');", true);
                        return;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }

    public class BH_RowerBoatAssign
    {
        public string BoatHouseId { get; set; }
        public string ServiceType { get; set; }
        public string BoatTypes { get; set; }
        public string String1 { get; set; }
        public string RowerId { get; set; }
        public string BoatTypeId { get; set; }
        public string BookingDate { get; set; }
        public string PremiumStatus { get; set; }
    }
}