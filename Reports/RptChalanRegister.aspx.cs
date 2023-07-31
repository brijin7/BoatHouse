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

public partial class Reports_RptChalanRegister : System.Web.UI.Page
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
                txtChallanDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtChallanDate.Attributes.Add("readonly", "readonly");
                rbtnChalan.SelectedValue = "6";
                if (rbtnChalan.SelectedValue == "4")
                {
                    divServiceType.Visible = false;
                    divPaymentType.Visible = false;
                    divUserName.Visible = false;
                }
                else
                {
                    GetPaymentType();
                    if (ddlServiceType.SelectedValue.Trim() == "B")
                    {
                        GetUserName("GetServiceWiseBoatingUserName");
                    }
                    else if (ddlServiceType.SelectedValue.Trim() == "OS")
                    {
                        GetUserName("GetServiceWiseOtherServicesUserName");
                    }
                    else if (ddlServiceType.SelectedValue.Trim() == "AT")
                    {
                        GetUserName("GetServiceWiseAdditionalTicketsUserName");
                    }
                    else//R
                    {
                        GetUserName("GetServiceWiseRestaurantsUserName");
                    }
                    divServiceType.Visible = true;
                    divPaymentType.Visible = true;
                    divUserName.Visible = true;
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }
    protected void rbtnChalan_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlPaymentType.Enabled = true;
        if (rbtnChalan.SelectedValue == "4")
        {
            divServiceType.Visible = false;
            divPaymentType.Visible = false;
            divUserName.Visible = false;
        }
        else
        {
            ddlServiceType.SelectedValue = "B";
            GetPaymentType();
            if (ddlServiceType.SelectedValue.Trim() == "B")
            {
                GetUserName("GetServiceWiseBoatingUserName");
            }
            else if (ddlServiceType.SelectedValue.Trim() == "OS")
            {
                GetUserName("GetServiceWiseOtherServicesUserName");
            }
            else if (ddlServiceType.SelectedValue.Trim() == "AT")
            {
                GetUserName("GetServiceWiseAdditionalTicketsUserName");
            }
            else//R
            {
                GetUserName("GetServiceWiseRestaurantsUserName");
            }
            divServiceType.Visible = true;
            divPaymentType.Visible = true;
            divUserName.Visible = true;
        }
    }
    public void GetPaymentType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLPayType").Result;

                if (response.IsSuccessStatusCode)
                {
                    var PayType = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(PayType)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(PayType)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlPaymentType.DataSource = dt;
                            ddlPaymentType.DataValueField = "ConfigId";
                            ddlPaymentType.DataTextField = "ConfigName";
                            ddlPaymentType.DataBind();
                        }
                        else
                        {
                            ddlPaymentType.DataBind();
                        }
                        ddlPaymentType.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlPaymentType.Items.Insert(0, new ListItem("All", "0"));
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
    public void GenerateRpt()
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

                string sUserId = string.Empty;
                string sPayMode = string.Empty;
                string sBoatHouseId = string.Empty;
                string sQueryType = string.Empty;
                string sServiceType = string.Empty;
                string sUserRole = string.Empty;
                sUserRole = Session["UserRole"].ToString();
                if (ddlUserName.SelectedValue == "")
                {
                    if (Session["UserRole"].ToString() == "User")
                    {
                        sUserId = Session["UserId"].ToString();
                    }
                    else
                    {
                        sUserId = "0";
                    }
                }
                else
                {
                    sUserId = ddlUserName.SelectedValue;
                }

                if (ddlPaymentType.SelectedValue == "")
                {
                    sPayMode = "0";
                }
                else
                {
                    sPayMode = ddlPaymentType.SelectedValue;
                }

                if (ddlServiceType.SelectedValue.Trim() == "B")
                {
                    sServiceType = "Boating";
                }
                else if (ddlServiceType.SelectedValue.Trim() == "OS")
                {
                    sServiceType = "OtherService";
                }
                else if (ddlServiceType.SelectedValue.Trim() == "R")
                {
                    sServiceType = "Restaurant";
                }
                else if (ddlServiceType.SelectedValue.Trim() == "AT")
                {
                    sServiceType = "AdditionalTicket";
                }
                sQueryType = "ServiceTypeAbstract";
                if (sQueryType == "ServiceTypeAbstract")
                {
                    if (ddlUserName.SelectedItem.Text == "OnlineUser")
                    {
                        sUserRole = "OnlineUser";
                    }
                    else
                    {
                        sUserRole = Session["UserRole"].ToString();
                    }
                }
                else
                {
                    sUserRole = Session["UserRole"].ToString();
                }

                var ChallanAb = new Challan()
                {
                    QueryType = sQueryType,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtChallanDate.Text,
                    UserId = sUserId,
                    Input1 = sUserRole,
                    ServiceType = sServiceType,
                    Input2 = sPayMode
                };
                HttpResponseMessage response = client.PostAsJsonAsync("AbstractChalanRegister", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        objReportDocument.Load(Server.MapPath("CRChalanAbstractDtl.rpt"));
                        objReportDocument.SetDataSource(dtExists);

                        if (ddlServiceType.SelectedValue.Trim() == "B")
                        {
                            objReportDocument.SetParameterValue(0, "Challan Abstract - Boating");
                            objReportDocument.SetParameterValue(4, "Boating Charges");
                        }
                        else if (ddlServiceType.SelectedValue.Trim() == "OS")
                        {
                            objReportDocument.SetParameterValue(0, "Challan Abstract - Other Service");
                            objReportDocument.SetParameterValue(4, "Other Service Charges");
                        }
                        else if (ddlServiceType.SelectedValue.Trim() == "R")
                        {
                            objReportDocument.SetParameterValue(0, "Challan Abstract - Restaurant");
                            objReportDocument.SetParameterValue(4, "Item Fare");
                        }
                        else if (ddlServiceType.SelectedValue.Trim() == "AT")
                        {
                            objReportDocument.SetParameterValue(0, "Challan Abstract - Additional Ticket");
                            objReportDocument.SetParameterValue(4, "Ticket Amount");
                        }
                        objReportDocument.SetParameterValue(1, "Payment Type : " + ddlPaymentType.SelectedItem.Text);
                        objReportDocument.SetParameterValue(2, "User : " + ddlUserName.SelectedItem.Text);
                        objReportDocument.SetParameterValue(3, "Challan Date : " + txtChallanDate.Text);
                        objReportDocument.SetParameterValue(5, Session["BoatHouseName"].ToString());
                        objReportDocument.SetParameterValue(6, Session["CorpName"].ToString());

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
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        if (rbtnChalan.SelectedValue == "4")
        {
            GenerateRptAbstract();
        }
        else
        {
            GenerateRpt();
        }
    }
    protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUserName.SelectedItem.Text == "OnlineUser")
        {
            ddlPaymentType.SelectedValue = "3";
            ddlPaymentType.Enabled = false;
        }
        else
        {
            ddlPaymentType.Enabled = true;
            ddlPaymentType.SelectedValue = "0";
        }
    }
    //OVERALL ABSTRACT
    public void GetMainBoatRateDetailsReportOverall(ref DataSet dstReports)
    {
        try
        {
            DataRow drwReport;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var ChallanAb = new Challan()
                {
                    QueryType = "ActualAbstract",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtChallanDate.Text,
                    UserId = Session["UserId"].ToString().Trim(),
                    Input1 = Session["UserRole"].ToString(),
                    ServiceType = "",
                    Input2 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("AbstractChalanRegister", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                        {
                            drwReport = dstReports.Tables["OverallAbtractChallan"].NewRow();

                            drwReport[0] = dtExists.Rows[iRowIdx]["UserId"].ToString();
                            drwReport[1] = dtExists.Rows[iRowIdx]["UserName"].ToString();
                            drwReport[2] = dtExists.Rows[iRowIdx]["DepositamtReceive"].ToString();
                            drwReport[3] = dtExists.Rows[iRowIdx]["BoatCharge"].ToString();
                            drwReport[4] = dtExists.Rows[iRowIdx]["RowerCharge"].ToString();
                            drwReport[5] = dtExists.Rows[iRowIdx]["BoatingGST"].ToString();
                            drwReport[6] = dtExists.Rows[iRowIdx]["Deposit"].ToString();
                            drwReport[7] = dtExists.Rows[iRowIdx]["BoatingTotal"].ToString();
                            drwReport[8] = dtExists.Rows[iRowIdx]["Balance"].ToString();
                            drwReport[9] = dtExists.Rows[iRowIdx]["OthersItemCharge"].ToString();
                            drwReport[10] = dtExists.Rows[iRowIdx]["OthersGST"].ToString();
                            drwReport[11] = dtExists.Rows[iRowIdx]["OthersTotal"].ToString();
                            drwReport[12] = dtExists.Rows[iRowIdx]["ResItemCharge"].ToString();
                            drwReport[13] = dtExists.Rows[iRowIdx]["ResGST"].ToString();
                            drwReport[14] = dtExists.Rows[iRowIdx]["ResTotal"].ToString();
                            drwReport[15] = dtExists.Rows[iRowIdx]["Refund"].ToString();
                            drwReport[16] = dtExists.Rows[iRowIdx]["RowerChargeSettle"].ToString();
                            drwReport[17] = dtExists.Rows[iRowIdx]["DepositTransfer"].ToString();
                        

                            dstReports.Tables["OverallAbtractChallan"].Rows.Add(drwReport);
                        }
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' No Records Found !');", true);
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
    public void GetSubBoatRateDetailsReportOverall(ref DataSet dstReports)
    {
        string UserId = string.Empty;
        string UserRole = string.Empty;
        try
        {
            DataRow drwReport;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["UserRole"].ToString().ToLower().Trim() == "user")
                {
                    UserId = Session["UserId"].ToString();
                    UserRole = Session["UserRole"].ToString().ToLower().Trim();
                }
                else
                {
                    UserId = "";
                    UserRole = "";
                }
                var ChallanAb = new Challan()
                {
                    QueryType = "PaymentDetails",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtChallanDate.Text,
                    UserId = UserId.ToString().Trim(),
                    Input1 = UserRole.ToString().Trim(),
                    ServiceType = "",
                    Input2 = ""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("AbstractChalanRegister", ChallanAb).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                        {
                            drwReport = dstReports.Tables["OverallAbtractChallanSubReport"].NewRow();

                            drwReport[0] = dtExists.Rows[iRowIdx]["UserId"].ToString();
                            drwReport[1] = dtExists.Rows[iRowIdx]["UserName"].ToString();
                            drwReport[2] = dtExists.Rows[iRowIdx]["BoatingCash"].ToString();
                            drwReport[3] = dtExists.Rows[iRowIdx]["BoatingCard"].ToString();
                            drwReport[4] = dtExists.Rows[iRowIdx]["BoatingOnline"].ToString();
                            drwReport[5] = dtExists.Rows[iRowIdx]["BoatingUPI"].ToString();

                            drwReport[6] = dtExists.Rows[iRowIdx]["OthersCash"].ToString();
                            drwReport[7] = dtExists.Rows[iRowIdx]["OthersCard"].ToString();
                            drwReport[8] = dtExists.Rows[iRowIdx]["OthersOnline"].ToString();
                            drwReport[9] = dtExists.Rows[iRowIdx]["OthersUPI"].ToString();

                            drwReport[10] = dtExists.Rows[iRowIdx]["RestaurantCash"].ToString();
                            drwReport[11] = dtExists.Rows[iRowIdx]["RestaurantCard"].ToString();
                            drwReport[12] = dtExists.Rows[iRowIdx]["RestaurantOnline"].ToString();
                            drwReport[13] = dtExists.Rows[iRowIdx]["RestaurantUPI"].ToString();





                            dstReports.Tables["OverallAbtractChallanSubReport"].Rows.Add(drwReport);
                        }
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !');", true);
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


    public void GenerateRptAbstract()
    {
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        try
             {


                        DataSet dstRports = new ChalanAbstract();
                        GetMainBoatRateDetailsReportOverall(ref dstRports);
                        GetSubBoatRateDetailsReportOverall(ref dstRports);

                        objReportDocument.Load(Server.MapPath("NewCRChalanAbstractOverall.rpt"));
                        objReportDocument.SetDataSource(dstRports);

                        objReportDocument.SetParameterValue(0, "Challan Abstract");
                        objReportDocument.SetParameterValue(3, "Challan Date : " + txtChallanDate.Text);
                        objReportDocument.SetParameterValue(4, Session["BoatHouseName"].ToString());
                        objReportDocument.SetParameterValue(5, "Printed By : " + Session["FirstName"].ToString().Trim() + " " + Session["LastName"].ToString().Trim() + " " + DateTime.Now.ToString("dd-MM-yyyy") + "");
                        objReportDocument.SetParameterValue(18, Session["CorpName"].ToString());
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
    public void GetUserName(string QueryType)
    {
        try
        {
            string UserRole = string.Empty;
            string UserId = string.Empty;
            ddlUserName.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["UserRole"].ToString().ToLower().Trim() == "user")
                {
                    UserRole = Session["UserRole"].ToString().ToLower().Trim();
                    UserId = Session["UserId"].ToString().Trim();
                }
                else
                {
                    UserRole = "";
                    UserId = "";
                }
                var UserName = new Challan()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtChallanDate.Text.Trim(),
                    ToDate = "",
                    QueryType = QueryType.ToString().Trim(),
                    ServiceType = "",
                    BookingId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = UserId.ToString().Trim(),
                    Input5 = UserRole.ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", UserName).Result;
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
                        ddlUserName.DataSource = null;
                        ddlUserName.DataBind();
                    }
                    ddlUserName.Items.Insert(0, new ListItem("Select User Name", "0"));
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
    protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlServiceType.SelectedValue.Trim() == "B")
            {
                GetUserName("GetServiceWiseBoatingUserName");
            }
            else if (ddlServiceType.SelectedValue.Trim() == "OS")
            {
                GetUserName("GetServiceWiseOtherServicesUserName");
            }
            else if (ddlServiceType.SelectedValue.Trim() == "AT")
            {
                GetUserName("GetServiceWiseAdditionalTicketsUserName");
            }
            else//R
            {
                GetUserName("GetServiceWiseRestaurantsUserName");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void txtChallanDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlServiceType.SelectedValue.Trim() == "B")
            {
                GetUserName("GetServiceWiseBoatingUserName");
            }
            else if (ddlServiceType.SelectedValue.Trim() == "OS")
            {
                GetUserName("GetServiceWiseOtherServicesUserName");
            }
            else if (ddlServiceType.SelectedValue.Trim() == "AT")
            {
                GetUserName("GetServiceWiseAdditionalTicketsUserName");
            }
            else//R
            {
                GetUserName("GetServiceWiseRestaurantsUserName");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void PaymentDetails(string Qt)
    {
        DataTable dt = new DataTable();
        string UserId = string.Empty;
        string UserRole = string.Empty;
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (Session["UserRole"].ToString().ToLower().Trim() == "user")
            {
                UserId = Session["UserId"].ToString();
                UserRole = Session["UserRole"].ToString().ToLower().Trim();
            }
            else
            {
                UserId = "";
                UserRole = "";
            }
            var ChallanAb = new Challan()
            {
                QueryType = Qt,
                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                FromDate = txtChallanDate.Text,
                UserId = UserId.ToString().Trim(),
                Input1 = UserRole.ToString().Trim(),
                ServiceType = "",
                Input2 = ""
            };
            HttpResponseMessage response = client.PostAsJsonAsync("AbstractChalanRegister", ChallanAb).Result;

            if (response.IsSuccessStatusCode)
            {
                var Response1 = response.Content.ReadAsStringAsync().Result;
                var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                if (dtExists.Rows.Count > 0)
                {
                    if (Qt == "BoatingPaymentDetails")
                    {
                        ViewState["BoatingCash"] = dtExists.Rows[0]["BoatingCash"].ToString();
                        ViewState["BoatingCard"] = dtExists.Rows[0]["BoatingCard"].ToString();
                        ViewState["BoatingOnline"] = dtExists.Rows[0]["BoatingOnline"].ToString();
                        ViewState["BoatingUPI"] = dtExists.Rows[0]["BoatingUPI"].ToString();
                    }
                    else if (Qt == "OtherServicesPaymentDetails")
                    {
                        ViewState["OthersCash"] = dtExists.Rows[0]["OthersCash"].ToString();
                        ViewState["OthersCard"] = dtExists.Rows[0]["OthersCard"].ToString();
                        ViewState["OthersOnline"] = dtExists.Rows[0]["OthersOnline"].ToString();
                        ViewState["OthersUPI"] = dtExists.Rows[0]["OthersUPI"].ToString();
                    }
                    else if (Qt == "RestaurantServicesPaymentDetails")
                    {
                        ViewState["RestaurantCash"] = dtExists.Rows[0]["RestaurantCash"].ToString();
                        ViewState["RestaurantCard"] = dtExists.Rows[0]["RestaurantCard"].ToString();
                        ViewState["RestaurantOnline"] = dtExists.Rows[0]["RestaurantOnline"].ToString();
                        ViewState["RestaurantUPI"] = dtExists.Rows[0]["RestaurantUPI"].ToString();
                    }
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
    public class Challan
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
        public string ToDate { get; set; }
    }
}