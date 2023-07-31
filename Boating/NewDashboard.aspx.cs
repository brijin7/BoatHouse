using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_NewDashboard : System.Web.UI.Page
{
    HttpResponseMessage response;
    public static string ResponseMsg;
    public static string GetResponse;
    DataTable dt;
    HttpClient client;
    Dashboard FormBody;
       
    public string GetAntiForgeryToken()
    {
        string cookieToken, formToken;
        AntiForgery.GetTokens(null, out cookieToken, out formToken);
        ViewState["__AntiForgeryCookie"] = cookieToken;
        return formToken;
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 17-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {          

            if (!IsPostBack)
            {
                Session["CorpId"] = "1";

                if (Request.HttpMethod != "GET")
                {
                    AntiForgery.Validate(ViewState["__AntiForgeryCookie"] as string, Request.Form["__RequestVerificationToken"]);
                }
                hfUrl.Value = Session["BaseUrl"].ToString().Trim();
                hfUserRole.Value = Session["UserRole"].ToString().Trim();

                if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    divBoatType.Visible = false;
                    divBoatSeat.Visible = false;

                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    if (Session["UserRole"].ToString() == "Sadmin")
                    {
                        BindBoatHouse(Session["CorpId"].ToString());
                        getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), "1", "0", "0", Session["CorpId"].ToString());
                        GetDrillBooking("1", Session["CorpId"].ToString());
                        GetDrillCancelled("1", Session["CorpId"].ToString());
                        GetDrillTravelled("1", Session["CorpId"].ToString());
                        getOtherServiceChart(ddlBoatHouse.SelectedValue.Trim(), "1", Session["CorpId"].ToString());
                        getRestaurantChart("1", ddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
                        AllRestaurantTicket("1", Session["CorpId"].ToString());
                        GetDrillRescheduleRevenue(Session["CorpId"].ToString());
                        AllBHOtherService("1", Session["CorpId"].ToString());
                        AllBHRestaurantService("1", Session["CorpId"].ToString());

                        divBookingSummary.Visible = true;
                        divAbstractBoatCount.Visible = false;
                        divPriceComparison.Visible = false;

                        BindBookingCountAmount(Session["CorpId"].ToString());
                        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString());

                        imgDownloadExcel.Visible = true;
                    }
                }
                if (Session["UserRole"].ToString() == "User")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    BindBoatHouse(Session["CorpId"].ToString());
                    ulTabList.Visible = true;
                    divrbtnType.Visible = false;
                    lbtnBoatRunningStatus.Visible = false;
                    lbtnBoatUtilization.Visible = false;
                    lbtnBookingSummary.Visible = true;
                    lbtnAbstractBoatCount.Visible = false;
                    lbtnPriceComparison.Visible = false;
                    lbtnRevenueComparison.Visible = false;

                    divBookingSummary.Visible = true;
                    divBoatType.Visible = false;
                    divBoatSeat.Visible = false;
                    divServicename.Visible = false;
                    divAbstractBoatCount.Visible = false;
                    divPriceComparison.Visible = false;
                    divRevenueComparison.Visible = false;
                    divBoatRunningStatus.Visible = false;
                    divBoatUtilization.Visible = false;

                    imgDownloadExcel.Visible = false;
                    BindBookingCountAmount(Session["CorpId"].ToString());
                    BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString());
                }
                else if (Session["UserRole"].ToString() == "Admin")
                {
                    lbtnPriceComparison.Visible = false;
                    imgDownloadExcel.Visible = false;
                }
                else if (Session["UserRole"].ToString() == "Sadmin" && Session["BoatHouseId"].ToString() != "")
                {
                    lbtnPriceComparison.Visible = false;
                }
                else
                {
                    lbtnPriceComparison.Visible = true;
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    //Boat House List in Dropdown for all Tabs
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BindBoatHouse(string sCorpId)
    {
        try
        {
            ddlBoatHouse.Items.Clear();
            abDdlBoatHouse.Items.Clear();
            RCddlBoatHouse.Items.Clear();
            BRSddlBoatHouse.Items.Clear();
            BUddlBoatHouse.Items.Clear();

            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                FormBody = new Dashboard()
                {
                    QueryType = "",
                    ServiceType = "",
                    Input1 = Session["UserRole"].ToString().Trim(),
                    Input2 = Session["CorpId"].ToString().Trim(),
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = ""
                };

                response = client.PostAsJsonAsync("AllBoatHousesList", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBoatHouse.DataSource = dt;
                        ddlBoatHouse.DataValueField = "BoatHouseId";
                        ddlBoatHouse.DataTextField = "BoatHouseName";
                        ddlBoatHouse.DataBind();

                        abDdlBoatHouse.DataSource = dt;
                        abDdlBoatHouse.DataValueField = "BoatHouseId";
                        abDdlBoatHouse.DataTextField = "BoatHouseName";
                        abDdlBoatHouse.DataBind();

                        //Block
                        //ListItem removeItem = abDdlBoatHouse.Items.FindByValue("74");
                        //abDdlBoatHouse.Items.Remove(removeItem);

                        RCddlBoatHouse.DataSource = dt;
                        RCddlBoatHouse.DataValueField = "BoatHouseId";
                        RCddlBoatHouse.DataTextField = "BoatHouseName";
                        RCddlBoatHouse.DataBind();

                        BRSddlBoatHouse.DataSource = dt;
                        BRSddlBoatHouse.DataValueField = "BoatHouseId";
                        BRSddlBoatHouse.DataTextField = "BoatHouseName";
                        BRSddlBoatHouse.DataBind();

                        BUddlBoatHouse.DataSource = dt;
                        BUddlBoatHouse.DataValueField = "BoatHouseId";
                        BUddlBoatHouse.DataTextField = "BoatHouseName";
                        BUddlBoatHouse.DataBind();
                    }
                    else
                    {
                        ddlBoatHouse.DataBind();
                        abDdlBoatHouse.DataBind();
                        RCddlBoatHouse.DataBind();
                        BRSddlBoatHouse.DataBind();
                        BUddlBoatHouse.DataBind();
                    }
                    ddlBoatHouse.Items.Insert(0, new ListItem("All", "0"));
                    abDdlBoatHouse.Items.Insert(0, new ListItem("All", "0"));
                    RCddlBoatHouse.Items.Insert(0, new ListItem("All", "0"));
                    BUddlBoatHouse.Items.Insert(0, new ListItem("All", "0"));

                    if (Session["UserRole"].ToString() == "Admin")
                    {
                        ddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        ddlBoatHouse.Enabled = false;

                        abDdlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        abDdlBoatHouse.Enabled = false;

                        RCddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        RCddlBoatHouse.Enabled = false;

                        BRSddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        BRSddlBoatHouse.Enabled = false;

                        BUddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        BUddlBoatHouse.Enabled = false;
                    }
                    if (Session["UserRole"].ToString() == "User")
                    {
                        ddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        ddlBoatHouse.Enabled = false;

                        abDdlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        abDdlBoatHouse.Enabled = false;

                        RCddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        RCddlBoatHouse.Enabled = false;

                        BRSddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        BRSddlBoatHouse.Enabled = false;

                        BUddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                        BUddlBoatHouse.Enabled = false;
                    }
                    else
                    {
                        string abc = Session["BoatHouseId"].ToString();
                        //   if (Session["UserRole"].ToString() == "Sadmin" && Session["BoatHouseId"].ToString() != "") Sillu 2023-04-28
                        if (Session["UserRole"].ToString() != "Sadmin" && Session["BoatHouseId"].ToString() != "")
                        {
                            ddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                            ddlBoatHouse.Enabled = false;

                            abDdlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                            abDdlBoatHouse.Enabled = false;

                            RCddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                            RCddlBoatHouse.Enabled = false;

                            BRSddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                            BRSddlBoatHouse.Enabled = false;

                            BUddlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                            BUddlBoatHouse.Enabled = false;
                        }
                        else
                        {
                            ddlBoatHouse.Enabled = true;
                            abDdlBoatHouse.Enabled = true;
                            RCddlBoatHouse.Enabled = true;
                            BRSddlBoatHouse.Enabled = true;
                            BUddlBoatHouse.Enabled = true;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }

    //Boat Type List in Dropdown for all Tabs
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 17-July-2023
    /// </summary>
    /// <param name="BoatHouse"></param>
    /// <param name="sCorpId"></param>
    public void BindBoatType(string BoatHouse, string sCorpId)
    {
        try
        {
            ddlBoatType.Items.Clear();
            abDdlBoatType.Items.Clear();
            RCddlBoatType.Items.Clear();
            PCddlBoatType.Items.Clear();
            BRSddlBoatType.Items.Clear();
            BUddlBoatType.Items.Clear();

            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (BoatHouse == "0")
                {
                    FormBody = new Dashboard()
                    {
                        QueryType = "BoatType",
                        ServiceType = "",
                        Input1 = "",
                        Input2 = "",
                        Input3 = "",
                        Input4 = "",
                        Input5 = sCorpId.Trim(),
                        BoatHouseId = ""
                    };
                    response = client.PostAsJsonAsync("CommonReport", FormBody).Result;
                }
                else
                {
                    FormBody = new Dashboard()
                    {
                        BoatHouseId = BoatHouse.Trim()
                    };

                    response = client.PostAsJsonAsync("BoatType/BoatRateMstr", FormBody).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;

                    if (BoatHouse == "0")
                    {
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    }
                    else
                    {
                        ResponseMsg = JObject.Parse(GetResponse)["Response"].ToString();
                    }
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBoatType.DataSource = dt;
                        ddlBoatType.DataValueField = "BoatTypeId";
                        ddlBoatType.DataTextField = "BoatType";
                        ddlBoatType.DataBind();

                        abDdlBoatType.DataSource = dt;
                        abDdlBoatType.DataValueField = "BoatTypeId";
                        abDdlBoatType.DataTextField = "BoatType";
                        abDdlBoatType.DataBind();

                        RCddlBoatType.DataSource = dt;
                        RCddlBoatType.DataValueField = "BoatTypeId";
                        RCddlBoatType.DataTextField = "BoatType";
                        RCddlBoatType.DataBind();

                        PCddlBoatType.DataSource = dt;
                        PCddlBoatType.DataValueField = "BoatTypeId";
                        PCddlBoatType.DataTextField = "BoatType";
                        PCddlBoatType.DataBind();

                        BRSddlBoatType.DataSource = dt;
                        BRSddlBoatType.DataValueField = "BoatTypeId";
                        BRSddlBoatType.DataTextField = "BoatType";
                        BRSddlBoatType.DataBind();

                        BUddlBoatType.DataSource = dt;
                        BUddlBoatType.DataValueField = "BoatTypeId";
                        BUddlBoatType.DataTextField = "BoatType";
                        BUddlBoatType.DataBind();
                    }
                    else
                    {
                        ddlBoatType.DataBind();
                        abDdlBoatType.DataBind();
                        RCddlBoatType.DataBind();
                        PCddlBoatType.DataBind();
                        BRSddlBoatType.DataBind();
                        BUddlBoatType.DataBind();
                    }

                    ddlBoatType.Items.Insert(0, new ListItem("All", "0"));
                    abDdlBoatType.Items.Insert(0, new ListItem("All", "0"));
                    RCddlBoatType.Items.Insert(0, new ListItem("All", "0"));
                    PCddlBoatType.Items.Insert(0, new ListItem("Select Boat Type", "0"));
                    BRSddlBoatType.Items.Insert(0, new ListItem("All", "0"));
                    BUddlBoatType.Items.Insert(0, new ListItem("All", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }

    //Boat Seater List in Dropdown for all Tabs
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="BoatHouse"></param>
    /// <param name="BoatTypeId"></param>
    /// <param name="sCorpId"></param>
    public void BindSeaterType(string BoatHouse, string BoatTypeId, string sCorpId)
    {
        try
        {
            ddlBoatSeater.Items.Clear();
            abDdlBoatSeater.Items.Clear();
            RCddlSeaterType.Items.Clear();
            PCddlBoatSeater.Items.Clear();
            BRSddlBoatSeater.Items.Clear();
            BUddlBoatSeater.Items.Clear();

            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (BoatHouse == "0")
                {
                    FormBody = new Dashboard()
                    {
                        QueryType = "SeaterType",
                        ServiceType = "",
                        Input1 = BoatTypeId,
                        Input2 = "",
                        Input3 = "",
                        Input4 = "",
                        Input5 = sCorpId.Trim(),
                        BoatHouseId = ""
                    };

                    response = client.PostAsJsonAsync("CommonReport", FormBody).Result;
                }
                else
                {
                    FormBody = new Dashboard()
                    {
                        BoatHouseId = BoatHouse.Trim(),
                        BoatTypeId = BoatTypeId.Trim()
                    };

                    response = client.PostAsJsonAsync("BoatSeat/BoatRateMstr", FormBody).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;

                    if (BoatHouse == "0")
                    {
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    }
                    else
                    {
                        ResponseMsg = JObject.Parse(GetResponse)["Response"].ToString();
                    }
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        ddlBoatSeater.DataSource = dt;
                        ddlBoatSeater.DataValueField = "BoatSeaterId";
                        ddlBoatSeater.DataTextField = "SeaterType";
                        ddlBoatSeater.DataBind();

                        abDdlBoatSeater.DataSource = dt;
                        abDdlBoatSeater.DataValueField = "BoatSeaterId";
                        abDdlBoatSeater.DataTextField = "SeaterType";
                        abDdlBoatSeater.DataBind();

                        RCddlSeaterType.DataSource = dt;
                        RCddlSeaterType.DataValueField = "BoatSeaterId";
                        RCddlSeaterType.DataTextField = "SeaterType";
                        RCddlSeaterType.DataBind();

                        PCddlBoatSeater.DataSource = dt;
                        PCddlBoatSeater.DataValueField = "BoatSeaterId";
                        PCddlBoatSeater.DataTextField = "SeaterType";
                        PCddlBoatSeater.DataBind();

                        BRSddlBoatSeater.DataSource = dt;
                        BRSddlBoatSeater.DataValueField = "BoatSeaterId";
                        BRSddlBoatSeater.DataTextField = "SeaterType";
                        BRSddlBoatSeater.DataBind();

                        BUddlBoatSeater.DataSource = dt;
                        BUddlBoatSeater.DataValueField = "BoatSeaterId";
                        BUddlBoatSeater.DataTextField = "SeaterType";
                        BUddlBoatSeater.DataBind();
                    }
                    else
                    {
                        ddlBoatSeater.DataBind();
                        abDdlBoatSeater.DataBind();
                        RCddlSeaterType.DataBind();
                        PCddlBoatSeater.DataBind();
                        BRSddlBoatSeater.DataBind();
                        BUddlBoatSeater.DataBind();
                    }
                    ddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
                    abDdlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
                    RCddlSeaterType.Items.Insert(0, new ListItem("All", "0"));
                    PCddlBoatSeater.Items.Insert(0, new ListItem("Select Boat Seater", "0"));
                    BRSddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
                    BUddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }

    /********************************************BOOKING SUMMARY***********************************************************/

    //Displaying Overall, Boat Booking, Other Service, Restaurant Total Revenues
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BindBookingCountAmount(string sCorpId)
    {
        try
        {
            lblOverallRevenue.Text = "0";
            lblBoatingOverallRevenue.Text = "0";
            lblOSOverallRevenue.Text = "0";
            lblRESOverallRevenue.Text = "0";
            lblRowerOverallRevenue.Text = "0";

            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    FormBody = new Dashboard()
                    {
                        QueryType = "OverallTotalRevenue",
                        BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        ServiceType = sCorpId.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };
                }
                else
                {
                    FormBody = new Dashboard()
                    {
                        QueryType = "OverallTotalRevenueUser", //Need To Validate Vediyappan.V
                        BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        ServiceType = Session["UserId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };
                }


                response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;

                    var BBRevenue = JObject.Parse(GetResponse)["Table"].ToString();
                    var OSRevenue = JObject.Parse(GetResponse)["Table1"].ToString();
                    var RESRevenue = JObject.Parse(GetResponse)["Table2"].ToString();
                    var RowerRevenue = JObject.Parse(GetResponse)["Table3"].ToString();

                    DataTable dtBBRevenue = JsonConvert.DeserializeObject<DataTable>(BBRevenue);
                    DataTable dtOSRevenue = JsonConvert.DeserializeObject<DataTable>(OSRevenue);
                    DataTable dtRESRevenue = JsonConvert.DeserializeObject<DataTable>(RESRevenue);
                    DataTable dtRowerRevenue = JsonConvert.DeserializeObject<DataTable>(RowerRevenue);

                    if (dtBBRevenue.Rows.Count > 0 || dtOSRevenue.Rows.Count > 0 || dtRESRevenue.Rows.Count > 0 || dtRowerRevenue.Rows.Count > 0)

                    {
                        double dTotalRevenue = 0;
                        double dBoatingOverallRev = 0;
                        double dOSOverallRev = 0;
                        double dRESOverallRev = 0;
                        double dOtherRev = 0;
                        double dRowerOverallRev = 0;

                        dBoatingOverallRev = Convert.ToDouble(dtBBRevenue.Rows[0]["BoatBookingRevenue"].ToString());
                        dOSOverallRev = Convert.ToDouble(dtOSRevenue.Rows[0]["OtherServiceRevenue"].ToString());
                        dRESOverallRev = Convert.ToDouble(dtRESRevenue.Rows[0]["RestaurantRevenue"].ToString());
                        dRowerOverallRev = Convert.ToDouble(dtRowerRevenue.Rows[0]["RowerRevenue"].ToString());
                        BindBookingOtherRevenueCountAmount(sCorpId.Trim());
                        dOtherRev = Convert.ToDouble(lblOROverallRevenue.Text);

                        dTotalRevenue = dBoatingOverallRev + dOSOverallRev + dRESOverallRev + dOtherRev - dRowerOverallRev;

                        if (dTotalRevenue != 0)
                        {
                            lblOverallRevenue.Text = dTotalRevenue.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        }
                        else
                        {
                            lblOverallRevenue.Text = "0";
                        }

                        if (dBoatingOverallRev != 0)
                        {
                            lblBoatingOverallRevenue.Text = dBoatingOverallRev.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        }
                        else
                        {
                            lblBoatingOverallRevenue.Text = "0";
                        }

                        if (dOSOverallRev != 0)
                        {
                            lblOSOverallRevenue.Text = dOSOverallRev.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        }
                        else
                        {
                            lblOSOverallRevenue.Text = "0";
                        }

                        if (dRESOverallRev != 0)
                        {
                            lblRESOverallRevenue.Text = dRESOverallRev.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        }
                        else
                        {
                            lblRESOverallRevenue.Text = "0";
                        }
                        //Newly Added Rower
                        if (dRowerOverallRev != 0)
                        {
                            lblRowerOverallRevenue.Text = dRowerOverallRev.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        }
                        else
                        {
                            lblRowerOverallRevenue.Text = "0";
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Displaying Other Revenues
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BindBookingOtherRevenueCountAmount(string sCorpId)
    {
        try
        {
            lblOROverallRevenue.Text = "0";
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sQueryType = string.Empty;
                if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    if (ddlBoatHouse.SelectedValue == "0")
                    {
                        sQueryType = "BSOtherRevenueAllBH";
                    }
                    else
                    {
                        sQueryType = "BSOtherRevenueBasedonBH";
                    }

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        UserId = "0",
                        ToDate = txtToDate.Text.Trim(),
                        CorpId = sCorpId.Trim()
                    };

                }
                else
                {
                    sQueryType = "BSOtherRevenueBasedonUser";
                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        CorpId = sCorpId.Trim()
                    };

                }
                response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["OtherRevenue"].ToString() != "0")
                        {
                            double dOtherOverallRev = 0;
                            dOtherOverallRev = Convert.ToDouble(dt.Rows[0]["OtherRevenue"].ToString());
                            lblOROverallRevenue.Text = dOtherOverallRev.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                        }
                        else
                        {
                            lblOROverallRevenue.Text = "0";
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Boat Booking Chart Method
    /// <summary>
    /// Auther : Vediyappan.V
    /// Date : 13-Jul-2023
    /// </summary>
    /// <param name="sBoatHouseId"></param>
    /// <param name="sType"></param>
    /// <param name="sBoatTypeId"></param>
    /// <param name="sBoatSeaterId"></param>
    /// <param name="sCorpId"></param>
    public void getBoatBookingChart(string sBoatHouseId, string sType, string sBoatTypeId, string sBoatSeaterId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sBoatTypeId == "")
                {
                    sBoatTypeId = "0";
                }
                if (sBoatSeaterId == "")
                {
                    sBoatSeaterId = "0";
                }

                if (sType == "0")
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;
                    string ssBoatStatus = string.Empty;

                    if (sBoatHouseId == "0" && sBoatTypeId != "0" && sBoatSeaterId != "0")
                    {
                        sQueryType = "BoatBookingAllBSTicketCount";
                        ssBoatHouseId = "0";
                        ssBoatTypeId = sBoatTypeId;
                        ssBoatSeaterId = sBoatSeaterId;
                        ssBoatStatus = "Booked";
                    }
                    else if (sBoatHouseId != "0" && sBoatTypeId != "0" && sBoatSeaterId != "0")
                    {
                        sQueryType = "AllBoatSeatsCountBasedOnBoatHouse";
                        ssBoatHouseId = sBoatHouseId;
                        ssBoatTypeId = sBoatTypeId;
                        ssBoatSeaterId = sBoatSeaterId;
                        ssBoatStatus = "";
                    }
                    else if (sBoatHouseId == "0" && sBoatTypeId != "0")
                    {
                        sQueryType = "BoatBookingAllBTTicketCount";
                        ssBoatHouseId = "0";
                        ssBoatTypeId = sBoatTypeId;
                        ssBoatSeaterId = "0";
                        ssBoatStatus = "Booked";
                    }
                    else if (sBoatHouseId != "0" && sBoatTypeId != "0")
                    {
                        sQueryType = "AllBoatTypesCountBasedOnBoatHouse";
                        ssBoatTypeId = sBoatTypeId;
                        ssBoatHouseId = sBoatHouseId;
                        ssBoatSeaterId = "";
                        ssBoatStatus = "";
                    }
                    else if (sBoatHouseId == "0")
                    {
                        sQueryType = "BoatBookingAllBHTicketCount";
                        ssBoatHouseId = "0";
                        ssBoatHouseId = "0";
                        ssBoatTypeId = "0";
                        ssBoatStatus = "Booked";
                    }
                    else
                    {
                        sQueryType = "BoatChargeCountBasedOnBoatHouse";
                        ssBoatTypeId = "";
                        ssBoatHouseId = sBoatHouseId;
                        ssBoatStatus = "";
                        ssBoatSeaterId = "";
                    }

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ssBoatStatus
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divBoatBookingGraph.Style.Add("display", "block");

                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            StringBuilder sbBHID = new StringBuilder();
                            string sBHID = string.Empty;
                            string sStatus = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Field2"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfCount.Value = strCount;

                                if (sBoatHouseId == "0")
                                {
                                    string sBHouse = dt.Rows[i]["BoatHouseName"].ToString();
                                    string a = sBHouse.Replace("Boat", "");
                                    string b = a.Replace("House", "");
                                    string result = string.Empty;
                                    result = b.Trim();
                                    sStatus = result;
                                    var ssStatus = sStatus.ToString();
                                    sbm.Append(ssStatus);
                                    sbm.Append(",");
                                    string strStatus = sbm.ToString().TrimEnd(',');
                                    hfStatus.Value = strStatus;

                                    sBHID = dt.Rows[i]["BoatHouseId"].ToString();
                                    var ssBHID = sBHID.ToString();
                                    sbBHID.Append(ssBHID);
                                    sbBHID.Append(",");
                                    string strBHID = sbBHID.ToString().TrimEnd(',');
                                    hfAllBHID.Value = strBHID;
                                }
                                else
                                {
                                    sStatus = dt.Rows[i]["Field1"].ToString();
                                    var ssStatus = sStatus.ToString();
                                    sbm.Append(ssStatus);
                                    sbm.Append(",");
                                    string strStatus = sbm.ToString().TrimEnd(',');
                                    hfStatus.Value = strStatus;
                                }
                            }
                        }
                        else
                        {
                            divBoatBookingGraph.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        divBoatBookingGraph.Style.Add("display", "none");
                    }
                }
                else
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatHouseId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatStatus = string.Empty;

                    if (sBoatHouseId == "0" && sBoatTypeId != "0" && sBoatSeaterId != "0")
                    {
                        sQueryType = "BoatBookingAllBSRevenue";
                        ssBoatHouseId = "0";
                        ssBoatTypeId = sBoatTypeId;
                        ssBoatSeaterId = sBoatSeaterId;
                        ssBoatStatus = "Booking";
                    }
                    else if (sBoatHouseId != "0" && sBoatTypeId != "0" && sBoatSeaterId != "0")
                    {
                        sQueryType = "AllBoatSeaterRevenueBasedOnBoatHouse";
                        ssBoatHouseId = sBoatHouseId;
                        ssBoatTypeId = sBoatTypeId;
                        ssBoatSeaterId = sBoatSeaterId;
                        ssBoatStatus = "";
                    }
                    else if (sBoatHouseId == "0" && sBoatTypeId != "0")
                    {
                        sQueryType = "BoatBookingAllBTRevenue";
                        ssBoatHouseId = "0";
                        ssBoatTypeId = sBoatTypeId;
                        ssBoatSeaterId = "0";
                        ssBoatStatus = "Booking";
                    }
                    else if (sBoatHouseId != "0" && sBoatTypeId != "0")
                    {
                        sQueryType = "AllBoatTypesRevenueBasedOnBoatHouse";
                        ssBoatTypeId = sBoatTypeId;
                        ssBoatHouseId = sBoatHouseId;
                        ssBoatSeaterId = "";
                        ssBoatStatus = "";
                    }
                    else if (sBoatHouseId == "0")
                    {
                        sQueryType = "BoatBookingAllBHRevenue";
                        ssBoatHouseId = "0";
                        ssBoatTypeId = "0";
                        ssBoatStatus = "Booking";
                        ssBoatSeaterId = "0";
                    }
                    else
                    {
                        sQueryType = "BoatChargeRevenueBasedOnBoatHouse";
                        ssBoatTypeId = "";
                        ssBoatHouseId = sBoatHouseId;
                        ssBoatSeaterId = "";
                        ssBoatStatus = "";
                    }

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = "",
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ssBoatStatus
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divBoatBookingGraph.Style.Add("display", "block");
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            StringBuilder sbBHID = new StringBuilder();
                            string sStatus = string.Empty;
                            string sBHID = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Field2"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfCount.Value = strCount;
                                if (sBoatHouseId == "0")
                                {
                                    string sBHouse = dt.Rows[i]["BoatHouseName"].ToString();
                                    string a = sBHouse.Replace("Boat", "");
                                    string b = a.Replace("House", "");
                                    string result = string.Empty;
                                    result = b.Trim();
                                    sStatus = result;
                                    var ssStatus = sStatus.ToString();
                                    sbm.Append(ssStatus);
                                    sbm.Append(",");
                                    string strStatus = sbm.ToString().TrimEnd(',');
                                    hfStatus.Value = strStatus;

                                    sBHID = dt.Rows[i]["BoatHouseId"].ToString();
                                    var ssBHID = sBHID.ToString();
                                    sbBHID.Append(ssBHID);
                                    sbBHID.Append(",");
                                    string strBHID = sbBHID.ToString().TrimEnd(',');
                                    hfAllBHID.Value = strBHID;
                                }
                                else
                                {
                                    sStatus = dt.Rows[i]["Field1"].ToString();
                                    var ssStatus = sStatus.ToString();
                                    sbm.Append(ssStatus);
                                    sbm.Append(",");
                                    string strStatus = sbm.ToString().TrimEnd(',');
                                    hfStatus.Value = strStatus;
                                }
                            }
                        }
                        else
                        {
                            divBoatBookingGraph.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        divBoatBookingGraph.Style.Add("display", "none");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Other Service Chart Method
    /// <summary>
    /// Auther : Vediyappan.V
    /// Date : 13-Jul-2023
    /// </summary>
    /// <param name="sBoatHouseId"></param>
    /// <param name="sType"></param>
    /// <param name="sCorpId"></param>
    public void getOtherServiceChart(string sBoatHouseId, string sType, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sType == "0")
                {
                    string sQueryType = string.Empty;
                    if (sBoatHouseId == "0")
                    {
                        sQueryType = "OtherServiceAllBHTicketCount";
                    }
                    else
                    {
                        sQueryType = "OtherServiceCountBasedOnBoatHouse";
                    }

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = sBoatHouseId,
                        BoatTypeId = "",
                        BoatSeaterId = "",
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divOtherServiceGraph.Style.Add("display", "block");
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            StringBuilder sbBHID = new StringBuilder();
                            string sStatus = string.Empty;
                            string sBHID = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfOtherServiceCount.Value = strCount;

                                if (sBoatHouseId == "0")
                                {
                                    string sBHouse = dt.Rows[i]["BoatHouseName"].ToString();
                                    string a = sBHouse.Replace("Boat", "");
                                    string b = a.Replace("House", "");
                                    string result = string.Empty;
                                    result = b.Trim();
                                    sStatus = result;
                                    var ssStatus = sStatus.ToString();
                                    sbm.Append(ssStatus);
                                    sbm.Append(",");
                                    string strStatus = sbm.ToString().TrimEnd(',');
                                    hfOtherServiceCategory.Value = strStatus;

                                    sBHID = dt.Rows[i]["BoatHouseId"].ToString();
                                    var ssBHID = sBHID.ToString();
                                    sbBHID.Append(ssBHID);
                                    sbBHID.Append(",");
                                    string strBHID = sbBHID.ToString().TrimEnd(',');
                                    hfAllBHIDOS.Value = strBHID;
                                }
                                else
                                {
                                    sStatus = dt.Rows[i]["Category"].ToString();
                                    var ssStatus = sStatus.ToString();
                                    sbm.Append(ssStatus);
                                    sbm.Append(",");
                                    string strStatus = sbm.ToString().TrimEnd(',');
                                    hfOtherServiceCategory.Value = strStatus;
                                }
                            }
                        }
                        else
                        {
                            divOtherServiceGraph.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        divOtherServiceGraph.Style.Add("display", "none");
                    }
                }
                else
                {
                    string sQueryType = string.Empty;
                    if (sBoatHouseId == "0")
                    {
                        sQueryType = "OtherServiceAllBHRevenue";
                    }
                    else
                    {
                        sQueryType = "OtherServiceRevenueBasedOnBoatHouse";
                    }

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = sBoatHouseId,
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        //ServiceType = "0",
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divOtherServiceGraph.Style.Add("display", "block");
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            StringBuilder sbBHID = new StringBuilder();
                            string sStatus = string.Empty;
                            string sBHID = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["NetAmount"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfOtherServiceCount.Value = strCount;

                                if (sBoatHouseId == "0")
                                {
                                    string sBHouse = dt.Rows[i]["BoatHouseName"].ToString();
                                    string a = sBHouse.Replace("Boat", "");
                                    string b = a.Replace("House", "");
                                    string result = string.Empty;
                                    result = b.Trim();
                                    sStatus = result;
                                    var ssStatus = sStatus.ToString();
                                    sbm.Append(ssStatus);
                                    sbm.Append(",");
                                    string strStatus = sbm.ToString().TrimEnd(',');
                                    hfOtherServiceCategory.Value = strStatus;

                                    sBHID = dt.Rows[i]["BoatHouseId"].ToString();
                                    var ssBHID = sBHID.ToString();
                                    sbBHID.Append(ssBHID);
                                    sbBHID.Append(",");
                                    string strBHID = sbBHID.ToString().TrimEnd(',');
                                    hfAllBHIDOS.Value = strBHID;
                                }
                                else
                                {
                                    sStatus = dt.Rows[i]["Category"].ToString();
                                    var ssStatus = sStatus.ToString();
                                    sbm.Append(ssStatus);
                                    sbm.Append(",");
                                    string strStatus = sbm.ToString().TrimEnd(',');
                                    hfOtherServiceCategory.Value = strStatus;
                                }
                            }
                        }
                        else
                        {
                            divOtherServiceGraph.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        divOtherServiceGraph.Style.Add("display", "none");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Restaurant Chart Method
    /// <summary>
    /// Auther : Vediyappan.V
    /// Date : 13-Jul-2023
    /// </summary>
    /// <param name="sType"></param>
    /// <param name="sBoatHouseId"></param>
    /// <param name="sCorp"></param>
    public void getRestaurantChart(string sType, string sBoatHouseId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;
                string sQueryType = string.Empty;
                string ssBoatHouseId = string.Empty;
                if (sType == "0")
                {
                    if (sBoatHouseId == "0")
                    {
                        sQueryType = "RestaurantAllBHTicketCount";
                        ssBoatHouseId = "0";
                    }
                    else
                    {
                        sQueryType = "RestaurantTicketCountBH";
                        ssBoatHouseId = sBoatHouseId;
                    }
                }
                else
                {
                    if (sBoatHouseId == "0")
                    {
                        sQueryType = "RestaurantAllBHRevenue";
                        ssBoatHouseId = "0";
                    }
                    else
                    {
                        sQueryType = "RestaurantRevenueBH";
                        ssBoatHouseId = sBoatHouseId;
                    }
                }

                FormBody = new Dashboard()
                {
                    QueryType = sQueryType,
                    BoatHouseId = ssBoatHouseId,
                    BoatTypeId = "",
                    BoatSeaterId = "",
                    ServiceType = sCorpId,
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
                    BoatStatus = ""
                };

                response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        divRestaurantGraph.Style.Add("display", "block");
                        StringBuilder sbc = new StringBuilder();
                        StringBuilder sbm = new StringBuilder();
                        StringBuilder sbBHID = new StringBuilder();
                        string sStatus = string.Empty;
                        string sBHID = string.Empty;
                        string sCategory = string.Empty;
                        decimal Count = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (sType == "0")
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                            }
                            else
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["NetAmount"].ToString());
                            }
                            var sCount = Count.ToString();
                            sbc.Append(sCount);
                            sbc.Append(",");
                            string strCount = sbc.ToString().TrimEnd(',');
                            hfRCount.Value = strCount;

                            if (sBoatHouseId == "0")
                            {
                                string sBHouse = dt.Rows[i]["BoatHouseName"].ToString();
                                string a = sBHouse.Replace("Boat", "");
                                string b = a.Replace("House", "");
                                string result = string.Empty;
                                result = b.Trim();
                                sStatus = result;
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfRCategoryName.Value = strStatus;

                                sBHID = dt.Rows[i]["BoatHouseId"].ToString();
                                var ssBHID = sBHID.ToString();
                                sbBHID.Append(ssBHID);
                                sbBHID.Append(",");
                                string strBHID = sbBHID.ToString().TrimEnd(',');
                                hfAllBHIDRES.Value = strBHID;
                            }
                            else
                            {
                                sStatus = dt.Rows[i]["Category"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfRCategoryName.Value = strStatus;
                            }
                        }
                    }
                    else
                    {
                        divRestaurantGraph.Style.Add("display", "none");
                    }
                }
                else
                {
                    divRestaurantGraph.Style.Add("display", "none");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //All Boat House Bookings Ticket Count
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sType"></param>
    public void GetDrillBooking(string sType, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sType == "0")
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;

                    sQueryType = "AllBHBookings";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divBoatBookingGraph.Style.Add("display", "block");

                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Field2"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfdCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfdStatus.Value = strStatus;
                            }
                        }
                    }
                }
                else
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatHouseId = string.Empty;
                    string ssBoatSeaterId = string.Empty;

                    sQueryType = "AllBHBookingsRevenue";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divBoatBookingGraph.Style.Add("display", "block");
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Field2"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfdCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfdStatus.Value = strStatus;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //All Boat House Cancelled Ticket Count
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sType"></param>
    /// <param name="sCorpId"></param>
    public void GetDrillCancelled(string sType, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sType == "0")
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;

                    sQueryType = "AllBHCancelled";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divBoatBookingGraph.Style.Add("display", "block");

                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Field2"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfCancelledCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfCancelledBoatHouse.Value = strStatus;
                            }
                        }
                    }
                }
                else
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatHouseId = string.Empty;
                    string ssBoatSeaterId = string.Empty;

                    sQueryType = "AllBHCancelledRevenue";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divBoatBookingGraph.Style.Add("display", "block");
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Field2"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfCancelledCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfCancelledBoatHouse.Value = strStatus;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //All Boat House Travelled Ticket Count
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sType"></param>
    /// <param name="sCorpId"></param>
    public void GetDrillTravelled(string sType, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sType == "0")
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;

                    sQueryType = "AllBHTravelled";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divBoatBookingGraph.Style.Add("display", "block");

                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Field2"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfTravelledCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfTravelledBoatHouse.Value = strStatus;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //All Boat House Restaurant Ticket Count
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sType"></param>
    /// <param name="sCorpId"></param>
    public void AllRestaurantTicket(string sType, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sType == "0")
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;

                    sQueryType = "AllRestaurantTicket";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            divBoatBookingGraph.Style.Add("display", "block");

                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfAllResServiceCount.Value = strCount;

                                sStatus = dt.Rows[i]["ServiceName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfAllResService.Value = strStatus;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //All Boat House Rescheduled Ticket Count
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void GetDrillRescheduleRevenue(string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                string sQueryType = string.Empty;
                string ssBoatTypeId = string.Empty;
                string ssBoatSeaterId = string.Empty;
                string ssBoatHouseId = string.Empty;

                sQueryType = "AllBHRescheduledRevenue";
                ssBoatHouseId = "";
                ssBoatHouseId = "";
                ssBoatTypeId = "";

                FormBody = new Dashboard()
                {
                    QueryType = sQueryType,
                    BoatHouseId = ssBoatHouseId,
                    BoatTypeId = ssBoatTypeId,
                    BoatSeaterId = ssBoatSeaterId,//
                    ServiceType = sCorpId,
                    FromDate = txtFromDate.Text.Trim(),
                    ToDate = txtToDate.Text.Trim(),
                    BoatStatus = ""
                };

                response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                     var GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        divBoatBookingGraph.Style.Add("display", "block");

                        StringBuilder sbc = new StringBuilder();
                        StringBuilder sbm = new StringBuilder();
                        StringBuilder sbPre = new StringBuilder();
                        string sStatus = string.Empty;
                        decimal Count = 0;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Count = Convert.ToDecimal(dt.Rows[i]["Field2"].ToString());
                            var sCount = Count.ToString();
                            sbc.Append(sCount);
                            sbc.Append(",");
                            string strCount = sbc.ToString().TrimEnd(',');
                            hfRescheduleCharge.Value = strCount;

                            sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                            var ssStatus = sStatus.ToString();
                            sbm.Append(ssStatus);
                            sbm.Append(",");
                            string strStatus = sbm.ToString().TrimEnd(',');
                            hfRescheduleBH.Value = strStatus;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //All Boat House Other Service Ticket Count
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sType"></param>
    /// <param name="sCorpId"></param>
    public void AllBHOtherService(string sType, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sType == "0")
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;

                    sQueryType = "AllBHOtherServiceTicketCount";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            string sCategory = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfOSCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfOSBoatHouse.Value = strStatus;

                                sCategory = dt.Rows[i]["ConfigName"].ToString();
                                var ssCategory = sCategory.ToString();
                                sbPre.Append(ssCategory);
                                sbPre.Append(",");
                                string strCategory = sbPre.ToString().TrimEnd(',');
                                hfOSCategory.Value = strCategory;
                            }
                        }
                    }
                }
                else
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;

                    sQueryType = "AllBHOtherServiceRevenue";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = "",
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            string sCategory = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfOSCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfOSBoatHouse.Value = strStatus;

                                sCategory = dt.Rows[i]["ConfigName"].ToString();
                                var ssCategory = sCategory.ToString();
                                sbPre.Append(ssCategory);
                                sbPre.Append(",");
                                string strCategory = sbPre.ToString().TrimEnd(',');
                                hfOSCategory.Value = strCategory;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sType"></param>
    /// <param name="sCorpId"></param>
    public void AllBHRestaurantService(string sType, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sType == "0")
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;

                    sQueryType = "AllBHRestaurantTicketCount";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = sCorpId,
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            string sCategory = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfRESCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfRESBoatHouse.Value = strStatus;

                                sCategory = dt.Rows[i]["CategoryName"].ToString();
                                var ssCategory = sCategory.ToString();
                                sbPre.Append(ssCategory);
                                sbPre.Append(",");
                                string strCategory = sbPre.ToString().TrimEnd(',');
                                hfRESCategory.Value = strCategory;
                            }
                        }
                    }
                }
                else
                {
                    string sQueryType = string.Empty;
                    string ssBoatTypeId = string.Empty;
                    string ssBoatSeaterId = string.Empty;
                    string ssBoatHouseId = string.Empty;

                    sQueryType = "AllBHRestaurantRevenue";
                    ssBoatHouseId = "";
                    ssBoatHouseId = "";
                    ssBoatTypeId = "";

                    FormBody = new Dashboard()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = ssBoatHouseId,
                        BoatTypeId = ssBoatTypeId,
                        BoatSeaterId = ssBoatSeaterId,
                        ServiceType = "",
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        BoatStatus = ""
                    };

                    response = client.PostAsJsonAsync("GetDashBoardBookingDetails", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            StringBuilder sbc = new StringBuilder();
                            StringBuilder sbm = new StringBuilder();
                            StringBuilder sbPre = new StringBuilder();
                            string sStatus = string.Empty;
                            string sCategory = string.Empty;
                            decimal Count = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Count = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                                var sCount = Count.ToString();
                                sbc.Append(sCount);
                                sbc.Append(",");
                                string strCount = sbc.ToString().TrimEnd(',');
                                hfRESCount.Value = strCount;

                                sStatus = dt.Rows[i]["BoatHouseName"].ToString();
                                var ssStatus = sStatus.ToString();
                                sbm.Append(ssStatus);
                                sbm.Append(",");
                                string strStatus = sbm.ToString().TrimEnd(',');
                                hfRESBoatHouse.Value = strStatus;

                                sCategory = dt.Rows[i]["CategoryName"].ToString();
                                var ssCategory = sCategory.ToString();
                                sbPre.Append(ssCategory);
                                sbPre.Append(",");
                                string strCategory = sbPre.ToString().TrimEnd(',');
                                hfRESCategory.Value = strCategory;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BookingSummaryClearInputs(string sCorpId)
    {
        BindBoatHouse(sCorpId);
        ddlServiceName.SelectedValue = "0";
        rbtnType.SelectedValue = "1";
        divBoatType.Visible = false;
        divBoatSeat.Visible = false;

        var today = DateTime.Now;
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        hfUrl.Value = Session["BaseUrl"].ToString().Trim();

        getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), "1", "0", "0", sCorpId.Trim());
        GetDrillBooking("1", sCorpId.Trim());
        GetDrillCancelled("1", sCorpId.Trim());
        GetDrillTravelled("1", sCorpId.Trim());
        getOtherServiceChart(ddlBoatHouse.SelectedValue.Trim(), "1", sCorpId.Trim());
        getRestaurantChart("1", ddlBoatHouse.SelectedValue.Trim(), sCorpId.Trim());
        AllRestaurantTicket("1", sCorpId.Trim());
        GetDrillRescheduleRevenue(sCorpId.Trim());
        AllBHOtherService("1", sCorpId.Trim());
        AllBHRestaurantService("1", sCorpId.Trim());

        divBookingSummary.Visible = true;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = false;
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBoatHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString());

        getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), ddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString());
        getOtherServiceChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString());
        getRestaurantChart(rbtnType.SelectedValue.Trim(), ddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());

        ddlServiceName.SelectedValue = "0";
        ddlBoatSeater.Items.Clear();
        divBoatSeat.Visible = false;
        ddlBoatType.Items.Clear();
        divBoatType.Visible = false;
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlServiceName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString());
        if (ddlServiceName.SelectedValue == "1")
        {
            divBoatType.Visible = true;
            divBoatSeat.Visible = true;
            BindBoatType(ddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            ddlBoatSeater.Items.Clear();
            ddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
            getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), ddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString());
            divOtherServiceGraph.Style.Add("display", "none");
            divRestaurantGraph.Style.Add("display", "none");
        }
        else if (ddlServiceName.SelectedValue == "2")
        {
            divBoatType.Visible = false;
            divBoatSeat.Visible = false;
            getOtherServiceChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString());
            divBoatBookingGraph.Style.Add("display", "none");
            divRestaurantGraph.Style.Add("display", "none");
        }
        else if (ddlServiceName.SelectedValue == "3")
        {
            divBoatType.Visible = false;
            divBoatSeat.Visible = false;
            getRestaurantChart(rbtnType.SelectedValue.Trim(), ddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
            divBoatBookingGraph.Style.Add("display", "none");
            divOtherServiceGraph.Style.Add("display", "none");
        }
        else
        {
            divBoatType.Visible = false;
            divBoatSeat.Visible = false;
            getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), ddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString());
            getOtherServiceChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString());
            getRestaurantChart(rbtnType.SelectedValue.Trim(), ddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString());
        if (ddlBoatType.SelectedValue != "0")
        {
            BindSeaterType(ddlBoatHouse.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        }
        else
        {
            ddlBoatSeater.Items.Clear();
            ddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        }

        getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), ddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbtnType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        GetDrillRescheduleRevenue(Session["CorpId"].ToString().Trim());
        AllBHOtherService(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        AllBHRestaurantService(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), ddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        GetDrillBooking(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        GetDrillCancelled(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        getOtherServiceChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        getRestaurantChart(rbtnType.SelectedValue.Trim(), ddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());

        if (ddlServiceName.SelectedValue == "1")
        {
            divOtherServiceGraph.Style.Add("display", "none");
            divRestaurantGraph.Style.Add("display", "none");
        }
        else if (ddlServiceName.SelectedValue == "2")
        {
            divBoatBookingGraph.Style.Add("display", "none");
            divRestaurantGraph.Style.Add("display", "none");
        }
        else if (ddlServiceName.SelectedValue == "3")
        {
            divBoatBookingGraph.Style.Add("display", "none");
            divOtherServiceGraph.Style.Add("display", "none");
        }
    }
   
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBoatSeater_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), ddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (Session["UserRole"].ToString() == "User")
        {
            ulTabList.Visible = true;

            lbtnBoatRunningStatus.Visible = false;
            lbtnBoatUtilization.Visible = false;
            lbtnBookingSummary.Visible = true;
            lbtnAbstractBoatCount.Visible = false;
            lbtnPriceComparison.Visible = false;
            lbtnRevenueComparison.Visible = false;

            divBookingSummary.Visible = true;
            divBoatType.Visible = false;
            divBoatSeat.Visible = false;
            divServicename.Visible = false;
            divAbstractBoatCount.Visible = false;
            divPriceComparison.Visible = false;
            divRevenueComparison.Visible = false;
            divBoatRunningStatus.Visible = false;
            divBoatUtilization.Visible = false;

            imgDownloadExcel.Visible = false;
            BindBookingCountAmount(Session["CorpId"].ToString().Trim());
            BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
            divusergraph.Visible = false;
        }
        else
        {
            BindBookingCountAmount(Session["CorpId"].ToString().Trim());
            BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
            GetDrillBooking(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            GetDrillCancelled("1", Session["CorpId"].ToString().Trim());
            GetDrillTravelled(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            AllRestaurantTicket(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            GetDrillRescheduleRevenue(Session["CorpId"].ToString().Trim());
            AllBHOtherService(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            AllBHRestaurantService(rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            BindBookingCountAmount(Session["CorpId"].ToString().Trim());
            BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());

            if (ddlServiceName.SelectedValue == "1")
            {
                getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), ddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
                divOtherServiceGraph.Style.Add("display", "none");
                divRestaurantGraph.Style.Add("display", "none");
            }
            else if (ddlServiceName.SelectedValue == "2")
            {
                getOtherServiceChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
                divBoatBookingGraph.Style.Add("display", "none");
                divRestaurantGraph.Style.Add("display", "none");
            }
            else if (ddlServiceName.SelectedValue == "3")
            {
                getRestaurantChart(rbtnType.SelectedValue.Trim(), ddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
                divBoatBookingGraph.Style.Add("display", "none");
                divOtherServiceGraph.Style.Add("display", "none");
            }
            else
            {
                getBoatBookingChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), ddlBoatType.SelectedValue.Trim(), ddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
                getOtherServiceChart(ddlBoatHouse.SelectedValue.Trim(), rbtnType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
                getRestaurantChart(rbtnType.SelectedValue.Trim(), ddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            }
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        BookingSummaryClearInputs(Session["CorpId"].ToString().Trim());
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnBookingSummary_Click(object sender, EventArgs e)
    {
        if (Session["UserRole"].ToString() == "User")
        {
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindBoatHouse(Session["CorpId"].ToString().Trim());
            ulTabList.Visible = true;

            lbtnBoatRunningStatus.Visible = false;
            lbtnBoatUtilization.Visible = false;
            lbtnBookingSummary.Visible = true;
            lbtnAbstractBoatCount.Visible = false;
            lbtnPriceComparison.Visible = false;
            lbtnRevenueComparison.Visible = false;

            divBookingSummary.Visible = true;
            divBoatType.Visible = false;
            divBoatSeat.Visible = false;
            divServicename.Visible = false;
            divAbstractBoatCount.Visible = false;
            divPriceComparison.Visible = false;
            divRevenueComparison.Visible = false;
            divBoatRunningStatus.Visible = false;
            divBoatUtilization.Visible = false;

            imgDownloadExcel.Visible = false;
            BindBookingCountAmount(Session["CorpId"].ToString().Trim());
            BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
            divusergraph.Visible = false;
        }
        else
        {
            BookingSummaryClearInputs(Session["CorpId"].ToString().Trim());
            BindBookingCountAmount(Session["CorpId"].ToString().Trim());
            BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        }

    }

    /***********************************BOOKING SUMMARY POPUPS********************************************/

    //Other Revenue Pop-Up
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BindOtherRevenuePopup(string sCorpId)
    {
        try
        {
            if (lblOROverallRevenue.Text != "0")
            {
                using (client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string sQueryType = string.Empty;
                    if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        if (ddlBoatHouse.SelectedValue == "0")
                        {
                            sQueryType = "BSOtherRevenueAllBH";
                        }
                        else
                        {
                            sQueryType = "BSOtherRevenueBasedonBH";
                        }

                        FormBody = new Dashboard()
                        {
                            QueryType = sQueryType,
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            FromDate = txtFromDate.Text.Trim(),
                            UserId = "0",
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }
                    else
                    {
                        sQueryType = "BSOtherRevenueBasedonUser";
                        FormBody = new Dashboard()
                        {
                            QueryType = sQueryType,
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            FromDate = txtFromDate.Text.Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };

                    }
                    response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["ExtendedAmt"].ToString() != "0")
                            {
                                double dExtendedAmt = 0;
                                dExtendedAmt = Convert.ToDouble(dt.Rows[0]["ExtendedAmt"].ToString());
                                lblExtensionCharges.Text = dExtendedAmt.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            }
                            else
                            {
                                lblExtensionCharges.Text = "0";
                            }

                            if (dt.Rows[0]["UnClaimedDeposit"].ToString() != "0")
                            {
                                double dUnClaimedDeposit = 0;
                                dUnClaimedDeposit = Convert.ToDouble(dt.Rows[0]["UnClaimedDeposit"].ToString());
                                lblUnclaimedDeposit.Text = dUnClaimedDeposit.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            }
                            else
                            {
                                lblUnclaimedDeposit.Text = "0";
                            }

                            if (dt.Rows[0]["CancellationCharges"].ToString() != "0")
                            {
                                double dCancellationCharges = 0;
                                dCancellationCharges = Convert.ToDouble(dt.Rows[0]["CancellationCharges"].ToString());
                                lblCancellationCharges.Text = dCancellationCharges.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            }
                            else
                            {
                                lblCancellationCharges.Text = "0";
                            }

                            if (dt.Rows[0]["RescheduleCharges"].ToString() != "0")
                            {
                                double dRescheduleCharges = 0;
                                dRescheduleCharges = Convert.ToDouble(dt.Rows[0]["RescheduleCharges"].ToString());
                                lblReschedulingCharges.Text = dRescheduleCharges.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            }
                            else
                            {
                                lblReschedulingCharges.Text = "0";
                            }

                            if (dt.Rows[0]["BalanceAmount"].ToString() != "0")
                            {
                                double dBalanceAmount = 0;
                                dBalanceAmount = Convert.ToDouble(dt.Rows[0]["BalanceAmount"].ToString());
                                lblBalanceAmount.Text = dBalanceAmount.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            }
                            else
                            {
                                lblBalanceAmount.Text = "0";
                            }

                            lblTotalCharges.Text = lblOROverallRevenue.Text;

                            MpeRevenue.Show();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                        MpeRevenue.Hide();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Boat Booking Revenue Pop-Up
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BindBoatBookingRevenuePopup(string sCorpId)
    {
        try
        {
            if (lblBoatingOverallRevenue.Text != "0")
            {
                using (client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        FormBody = new Dashboard()
                        {
                            QueryType = "BoatBookingRevenue",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            FromDate = txtFromDate.Text.Trim(),
                            UserId = "0",
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }
                    else
                    {
                        FormBody = new Dashboard()
                        {
                            QueryType = " BoatBookingRevenueUser",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            FromDate = txtFromDate.Text.Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }

                    response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvBBpopup.DataSource = dt;
                            gvBBpopup.DataBind();
                            gvBBpopup.Visible = true;

                            decimal totAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BoatBookingRevenue")));

                            gvBBpopup.FooterRow.Cells[1].Text = "Total";
                            gvBBpopup.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            gvBBpopup.FooterRow.Cells[2].Text = totAmt.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            gvBBpopup.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            MPEBBpopup.Show();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                        MPEBBpopup.Hide();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Other Service Revenue Pop-Up
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BindOtherServiceRevenuePopup(string sCorpId)
    {
        try
        {
            if (lblOSOverallRevenue.Text != "0")
            {
                using (client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        FormBody = new Dashboard()
                        {
                            QueryType = "OtherServiceRevenue",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            UserId = "0",
                            FromDate = txtFromDate.Text.Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }
                    else
                    {
                        FormBody = new Dashboard()
                        {
                            QueryType = "OtherServiceRevenueUser",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            FromDate = txtFromDate.Text.Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }

                    response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvOSpopup.DataSource = dt;
                            gvOSpopup.DataBind();
                            gvOSpopup.Visible = true;

                            decimal totAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("OtherServiceRevenue")));

                            gvOSpopup.FooterRow.Cells[1].Text = "Total";
                            gvOSpopup.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            gvOSpopup.FooterRow.Cells[2].Text = totAmt.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            gvOSpopup.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            MPEOSpopup.Show();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                        MPEOSpopup.Hide();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Restaurant Revenue Pop-Up
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BindRestaurantRevenuePopup(string sCorpId)
    {
        try
        {
            if (lblRESOverallRevenue.Text != "0")
            {
                using (client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {

                        FormBody = new Dashboard()
                        {
                            QueryType = "RestaurantRevenue",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            UserId = "0",
                            FromDate = txtFromDate.Text.Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }
                    else
                    {
                        FormBody = new Dashboard()
                        {
                            QueryType = " RestaurantRevenueUser",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            FromDate = txtFromDate.Text.Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }

                    response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvRESpopup.DataSource = dt;
                            gvRESpopup.DataBind();
                            gvRESpopup.Visible = true;

                            decimal totAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("RestaurantRevenue")));

                            gvRESpopup.FooterRow.Cells[1].Text = "Total";
                            gvRESpopup.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            gvRESpopup.FooterRow.Cells[2].Text = totAmt.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            gvRESpopup.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            MPERESpopup.Show();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                        MPERESpopup.Hide();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Overall Revenue Pop-Up
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void BindOverallRevenuePopup(string sCorpId)
    {
        try
        {
            if (lblOverallRevenue.Text != "0")
            {
                using (client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string sQueryType = string.Empty;

                    if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        FormBody = new Dashboard()
                        {
                            QueryType = "OverallRevenue",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            UserId = "0",
                            FromDate = txtFromDate.Text.Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };

                    }
                    else
                    {
                        FormBody = new Dashboard()
                        {
                            QueryType = "OverallRevenueUser",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            FromDate = txtFromDate.Text.Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }
                    response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvORpopup.DataSource = dt;
                            gvORpopup.DataBind();
                            gvORpopup.Visible = true;

                            decimal totAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("OverallRevenue")));

                            gvORpopup.FooterRow.Cells[1].Text = "Total";
                            gvORpopup.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            gvORpopup.FooterRow.Cells[2].Text = totAmt.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            gvORpopup.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            MPEORpopup.Show();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                        MPEORpopup.Hide();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lblOROverallRevenue_Click(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        BindOtherRevenuePopup(Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgClose_Click(object sender, ImageClickEventArgs e)
    {
        MpeRevenue.Hide();
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lblBoatingOverallRevenue_Click(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        BindBoatBookingRevenuePopup(Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lblOSOverallRevenue_Click(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        BindOtherServiceRevenuePopup(Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lblRESOverallRevenue_Click(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        BindRestaurantRevenuePopup(Session["CorpId"].ToString().Trim());

    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgCloseBB_Click(object sender, ImageClickEventArgs e)
    {
        MPEBBpopup.Hide();
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgCloseOS_Click(object sender, ImageClickEventArgs e)
    {
        MPEOSpopup.Hide();
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgCloseRes_Click(object sender, ImageClickEventArgs e)
    {
        MPERESpopup.Hide();
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgCloseOR_Click(object sender, ImageClickEventArgs e)
    {
        MPEORpopup.Hide();
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lblOverallRevenue_Click(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        BindOverallRevenuePopup(Session["CorpId"].ToString().Trim());
    }

    /*****************************************ABSTRACT BOAT COUNT****************************************************/

    ///Listing Boat Status in Dropdown
    /// Get Boat Status From Config Master table.
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    public void GetBoatStatus()
    {
        try
        {
            ddlBoatStatus.Items.Clear();
            ddlBoatStatus.Items.Insert(0, new ListItem("All", "0"));

            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = client.GetAsync("ConfigMstr/DDLBoatStatus").Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(GetResponse)["StatusCode"].ToString());
                    ResponseMsg = JObject.Parse(GetResponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlBoatStatus.DataSource = dt;
                            ddlBoatStatus.DataValueField = "ConfigId";
                            ddlBoatStatus.DataTextField = "ConfigName";
                            ddlBoatStatus.DataBind();
                        }
                        else
                        {
                            ddlBoatStatus.DataBind();
                        }
                        ddlBoatStatus.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoatStatus.Items.Insert(0, new ListItem("All", "0"));
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Boat Count Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    public void BindDashboardBoatCount(string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (Session["UserRole"].ToString().Trim() == "Sadmin")
                {
                    FormBody = new Dashboard()
                    {
                        BoatHouseId = abDdlBoatHouse.SelectedValue.Trim(),
                        BoatTypeId = abDdlBoatType.SelectedValue.Trim(),
                        BoatSeaterId = abDdlBoatSeater.SelectedValue.Trim(),
                        BoatStatusId = ddlBoatStatus.SelectedValue.Trim(),
                        CorpId = sCorpId.Trim()
                    };
                    ddlBoatHouse.Enabled = true;
                    response = client.PostAsJsonAsync("GetDashboardBoatCount", FormBody).Result;
                }
                else
                {
                    FormBody = new Dashboard()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        BoatTypeId = abDdlBoatType.SelectedValue.Trim(),
                        BoatSeaterId = abDdlBoatSeater.SelectedValue.Trim(),
                        BoatStatusId = ddlBoatStatus.SelectedValue.Trim(),
                        CorpId = Session["CorpId"].ToString().Trim()
                    };
                    abDdlBoatHouse.SelectedValue = Session["BoatHouseId"].ToString();
                    abDdlBoatHouse.Attributes.Add("disabled", "disabled");
                    response = client.PostAsJsonAsync("GetDashboardBoatCount", FormBody).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["TableShow"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        string[] labels = new string[dt.Rows.Count];
                        string[] result = new string[dt.Rows.Count];
                        string boathouse = string.Empty;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            labels[i] = dt.Rows[i]["BoatType"].ToString();
                            result[i] = dt.Rows[i]["BoatCount"].ToString();
                            boathouse = dt.Rows[0]["BoatHouseName"].ToString();

                        }
                        hfData.Value = String.Join(",", result);
                        hfLabel.Value = String.Join(",", labels);
                        hfBoatHouse.Value = boathouse.ToString();

                        divAbBoatBookingGraph.Visible = true;
                    }
                    else
                    {
                        divAbBoatBookingGraph.Visible = false;
                        return;
                    }
                }
                else
                {
                    divAbBoatBookingGraph.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void abDdlBoatHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBoatType(abDdlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        abDdlBoatSeater.Items.Clear();
        abDdlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        divAbBoatBookingGraph.Visible = false;
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void abDdlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (abDdlBoatType.SelectedValue != "0")
        {
            BindSeaterType(abDdlBoatHouse.SelectedValue.Trim(), abDdlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        }
        else
        {
            abDdlBoatSeater.Items.Clear();
            abDdlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnABSubmit_Click(object sender, EventArgs e)
    {
        divAbBoatBookingGraph.Visible = true;
        BindDashboardBoatCount(Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnABReset_Click(object sender, EventArgs e)
    {
        BindBoatHouse(Session["CorpId"].ToString().Trim());
        BindBoatType(abDdlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        abDdlBoatSeater.Items.Clear();
        abDdlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        GetBoatStatus();
        BindDashboardBoatCount(Session["CorpId"].ToString().Trim());
        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = true;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = false;

        if (Session["UserRole"].ToString() == "Admin")
        {
            divNote1.Visible = false;
        }
        else if (Session["UserRole"].ToString() == "Sadmin" && Session["BoatHouseId"].ToString() != "")
        {
            divNote1.Visible = false;
        }
        else
        {
            divNote1.Visible = true;
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnAbstractBoatCount_Click(object sender, EventArgs e)
    {
        BindBoatHouse(Session["CorpId"].ToString().Trim());
        BindBoatType(abDdlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        abDdlBoatSeater.Items.Clear();
        abDdlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        GetBoatStatus();
        BindDashboardBoatCount(Session["CorpId"].ToString().Trim());
        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = true;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = false;

        if (Session["UserRole"].ToString() == "Admin")
        {
            divNote1.Visible = false;
        }
        else if (Session["UserRole"].ToString() == "Sadmin" && Session["BoatHouseId"].ToString() != "")
        {
            divNote1.Visible = false;
        }
        else
        {
            divNote1.Visible = true;
        }
    }

    /***************************************PRICE COMPARISON******************************************/

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnPriceComparison_Click(object sender, EventArgs e)
    {
        PCddlServiceName.SelectedValue = "0";
        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = true;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = false;

        divGraph.Visible = false;
        divRestaurantCategory.Visible = false;
        divRestaurantService.Visible = false;

        divOtherServiceCategory.Visible = false;
        divOtherServiceServices.Visible = false;

        pchfheading1.Value = "Pedal Boat";
        pchfheading2.Value = "2 - Seater";
        divType.Visible = true;
        rbtnNormPre.SelectedValue = "N";

        PCddlServiceName.SelectedValue = "1";
        divGraph.Visible = false;
        PCdivBoatType.Visible = true;
        PCdivBoatSeater.Visible = true;

        divOtherServiceCategory.Visible = false;
        divOtherServiceServices.Visible = false;

        divRestaurantCategory.Visible = false;
        divRestaurantService.Visible = false;
        BindBoatType("0", Session["CorpId"].ToString().Trim());
        PCddlBoatType.Items.FindByText("Pedal Boat").Selected = true;
        BindSeaterType("0", PCddlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        PCddlBoatSeater.Items.FindByText("2 - Seater").Selected = true;

        getBoatCharges("7, 16, 19, 22, 27, 185", "10, 21, 25, 28, 39, 148", Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPCSubmit_Click(object sender, EventArgs e)
    {
        if (PCddlServiceName.SelectedValue == "1")
        {
            getBoatCharges(PCddlBoatType.SelectedValue, PCddlBoatSeater.SelectedValue, Session["CorpId"].ToString().Trim());

        }
        else if (PCddlServiceName.SelectedValue == "2")
        {
            getOtherServices(ddlCategoryOtherService.SelectedValue, ddlServiceOtherService.SelectedValue, Session["CorpId"].ToString().Trim());
        }
        else if (PCddlServiceName.SelectedValue == "3")
        {
            getRestaurant(ddlCategoryRestaurant.SelectedValue, ddlServiceRestaurant.SelectedValue, Session["CorpId"].ToString().Trim());
        }
        else
        {
            divGraph.Visible = false;
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPCReset_Click(object sender, EventArgs e)
    {
        PCddlServiceName.SelectedValue = "0";
        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = true;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = false;

        divGraph.Visible = false;
        divRestaurantCategory.Visible = false;
        divRestaurantService.Visible = false;

        PCdivBoatType.Visible = false;
        PCdivBoatSeater.Visible = false;

        divOtherServiceCategory.Visible = false;
        divOtherServiceServices.Visible = false;

        pchfheading1.Value = "Pedal Boat";
        pchfheading2.Value = "2 - Seater";
        divType.Visible = true;
        rbtnNormPre.SelectedValue = "N";

        PCddlServiceName.SelectedValue = "1";
        divGraph.Visible = false;
        PCdivBoatType.Visible = true;
        PCdivBoatSeater.Visible = true;

        divOtherServiceCategory.Visible = false;
        divOtherServiceServices.Visible = false;

        divRestaurantCategory.Visible = false;
        divRestaurantService.Visible = false;
        BindBoatType("0", Session["CorpId"].ToString().Trim());
        PCddlBoatType.Items.FindByText("Pedal Boat").Selected = true;
        BindSeaterType("0", PCddlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        PCddlBoatSeater.Items.FindByText("2 - Seater").Selected = true;

        getBoatCharges("7, 16, 19, 22, 27, 185", "10, 21, 25, 28, 39, 148", Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PCddlServiceName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PCddlServiceName.SelectedValue == "1")
        {
            divGraph.Visible = false;
            PCdivBoatType.Visible = true;
            PCdivBoatSeater.Visible = true;

            divOtherServiceCategory.Visible = false;
            divOtherServiceServices.Visible = false;

            divRestaurantCategory.Visible = false;
            divRestaurantService.Visible = false;
            BindBoatType("0", Session["CorpId"].ToString().Trim());
            PCddlBoatSeater.Items.Clear();
            PCddlBoatSeater.Items.Insert(0, new ListItem("Select Boat Seater", "0"));

            divType.Visible = true;
        }
        else if (PCddlServiceName.SelectedValue == "2")
        {
            divGraph.Visible = false;
            divOtherServiceCategory.Visible = true;
            divOtherServiceServices.Visible = true;

            PCdivBoatType.Visible = false;
            PCdivBoatSeater.Visible = false;

            divRestaurantCategory.Visible = false;
            divRestaurantService.Visible = false;
            GetOtherServiceCategory(Session["CorpId"].ToString().Trim());
            ddlServiceOtherService.Items.Clear();
            ddlServiceOtherService.Items.Insert(0, new ListItem("Select Service", "0"));

            divType.Visible = false;
        }
        else if (PCddlServiceName.SelectedValue == "3")
        {
            divGraph.Visible = false;
            divRestaurantCategory.Visible = true;
            divRestaurantService.Visible = true;

            PCdivBoatType.Visible = false;
            PCdivBoatSeater.Visible = false;

            divOtherServiceCategory.Visible = false;
            divOtherServiceServices.Visible = false;
            GetRestaurantCategory(Session["CorpId"].ToString().Trim());
            ddlServiceRestaurant.Items.Clear();
            ddlServiceRestaurant.Items.Insert(0, new ListItem("Select Item Name", "0"));

            divType.Visible = false;
        }
        else
        {
            pchfheading1.Value = "Pedal Boat";
            pchfheading2.Value = "2 - Seater";
            divType.Visible = true;
            rbtnNormPre.SelectedValue = "N";

            PCddlServiceName.SelectedValue = "1";
            divGraph.Visible = false;
            PCdivBoatType.Visible = true;
            PCdivBoatSeater.Visible = true;

            divOtherServiceCategory.Visible = false;
            divOtherServiceServices.Visible = false;

            divRestaurantCategory.Visible = false;
            divRestaurantService.Visible = false;
            BindBoatType("0", Session["CorpId"].ToString().Trim());
            PCddlBoatType.Items.FindByText("Pedal Boat").Selected = true;
            BindSeaterType("0", PCddlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            PCddlBoatSeater.Items.FindByText("2 - Seater").Selected = true;

            getBoatCharges("7, 16, 19, 22, 27, 185", "10, 21, 25, 28, 39, 148", Session["CorpId"].ToString().Trim());
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PCddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PCddlBoatType.SelectedValue != "0")
        {
            BindSeaterType("0", PCddlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            divGraph.Visible = false;
        }
        else
        {
            PCddlBoatSeater.Items.Clear();
            PCddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        }
    }

    //List Other Service Types in Dropdown
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void GetOtherServiceCategory(string sCorpId)
    {
        try
        {
            ddlCategoryOtherService.Items.Clear();
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                FormBody = new Dashboard()
                {
                    QueryType = "OtherCatName",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = sCorpId.Trim(),
                    BoatHouseId = ""
                };

                response = client.PostAsJsonAsync("CommonReport", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        ddlCategoryOtherService.DataSource = dt;
                        ddlCategoryOtherService.DataValueField = "CategoryId";
                        ddlCategoryOtherService.DataTextField = "CategoryName";
                        ddlCategoryOtherService.DataBind();
                    }
                    else
                    {
                        ddlCategoryOtherService.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Category Details Not Found...!');", true);
                    }
                    ddlCategoryOtherService.Items.Insert(0, new ListItem("Select Category", "0"));
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

    //List Services based on Other Service Type in Dropdown
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sCatId"></param>
    /// <param name="sCorpId"></param>
    public void GetOtherServiceServiceName(string sCatId, string sCorpId)
    {
        try
        {
            ddlServiceOtherService.Items.Clear();

            if (ddlCategoryOtherService.SelectedIndex == 0)
            {
                ddlServiceOtherService.Items.Insert(0, new ListItem("Select Service", "0"));
                return;
            }
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string boatTypeIds = string.Empty;

                FormBody = new Dashboard()
                {
                    QueryType = "OtherServiceName",
                    ServiceType = "",
                    Input1 = sCatId.ToString(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = sCorpId.Trim(),
                    BoatHouseId = ""
                };

                response = client.PostAsJsonAsync("CommonReport", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dt.Rows.Count > 0)
                    {
                        ddlServiceOtherService.DataSource = dt;
                        ddlServiceOtherService.DataValueField = "ServiceId";
                        ddlServiceOtherService.DataTextField = "ServiceName";
                        ddlServiceOtherService.DataBind();
                    }
                    else
                    {
                        ddlServiceOtherService.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Service Details Not Found !');", true);
                    }
                    ddlServiceOtherService.Items.Insert(0, new ListItem("Select Service", "0"));
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

    //List Restaurant Service Types in Dropdown
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void GetRestaurantCategory(string sCorpId)
    {
        try
        {
            ddlCategoryRestaurant.Items.Clear();
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                FormBody = new Dashboard()
                {
                    QueryType = "RestCatName",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = sCorpId.Trim(),
                    BoatHouseId = ""
                };

                response = client.PostAsJsonAsync("CommonReport", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dt.Rows.Count > 0)
                    {
                        ddlCategoryRestaurant.DataSource = dt;
                        ddlCategoryRestaurant.DataValueField = "CategoryId";
                        ddlCategoryRestaurant.DataTextField = "CategoryName";
                        ddlCategoryRestaurant.DataBind();
                    }
                    else
                    {
                        ddlCategoryRestaurant.DataBind();
                    }
                    ddlCategoryRestaurant.Items.Insert(0, new ListItem("Select Category", "0"));
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

    //List Items based on Restaurant Service Type in Dropdown
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sCorpId"></param>
    public void GetRestaurantService(string sCorpId)
    {
        try
        {
            ddlServiceRestaurant.Items.Clear();

            if (ddlCategoryRestaurant.SelectedIndex == 0)
            {
                ddlServiceRestaurant.Items.Insert(0, new ListItem("Select Item Name", "0"));
                return;
            }
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                FormBody = new Dashboard()
                {
                    QueryType = "RestItemName",
                    ServiceType = "",
                    Input1 = ddlCategoryRestaurant.SelectedValue.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = sCorpId.Trim(),
                    BoatHouseId = ""
                };
                response = client.PostAsJsonAsync("CommonReport", FormBody).Result;
                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dt.Rows.Count > 0)
                    {
                        ddlServiceRestaurant.DataSource = dt;
                        ddlServiceRestaurant.DataValueField = "ServiceId";
                        ddlServiceRestaurant.DataTextField = "ServiceName";
                        ddlServiceRestaurant.DataBind();
                    }
                    else
                    {
                        ddlServiceRestaurant.DataBind();
                    }
                    ddlServiceRestaurant.Items.Insert(0, new ListItem("Select Item Name", "0"));
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
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCategoryOtherService_SelectedIndexChanged(object sender, EventArgs e)
    {
        divGraph.Visible = false;
        GetOtherServiceServiceName(ddlCategoryOtherService.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCategoryRestaurant_SelectedIndexChanged(object sender, EventArgs e)
    {
        divGraph.Visible = false;
        GetRestaurantService(Session["CorpId"].ToString());
    }

    //Boat Charge Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sBoatTypeId"></param>
    /// <param name="sBoatSeaterId"></param>
    /// <param name="sCorpId"></param>
    public void getBoatCharges(string sBoatTypeId, string sBoatSeaterId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                FormBody = new Dashboard()
                {
                    QueryType = "BoatCharges",
                    Input1 = sBoatTypeId,
                    Input2 = sBoatSeaterId,
                    Input3 = rbtnNormPre.SelectedValue.Trim(),
                    CorpId = sCorpId.Trim()
                };

                response = client.PostAsJsonAsync("PriceComparison", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sbc = new StringBuilder();
                        StringBuilder sbm = new StringBuilder();
                        StringBuilder sbPre = new StringBuilder();
                        string sBoatHouse = string.Empty;
                        decimal Price = 0;
                        decimal PremiumPrice = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (rbtnNormPre.SelectedValue == "N")
                            {
                                Price = Convert.ToDecimal(dt.Rows[i]["Normal"].ToString());
                                var sPrice = Price.ToString();
                                sbc.Append(sPrice);
                                sbc.Append(",");
                                string strPrice = sbc.ToString().TrimEnd(',');
                                hfPrice.Value = strPrice;
                            }
                            else
                            {
                                PremiumPrice = Convert.ToDecimal(dt.Rows[i]["Premium"].ToString());
                                var sPremiumPrice = PremiumPrice.ToString();
                                sbPre.Append(sPremiumPrice);
                                sbPre.Append(",");
                                string strPremiumPrice = sbPre.ToString().TrimEnd(',');
                                hfPrice.Value = strPremiumPrice;
                            }
                            string sBHouse = dt.Rows[i]["BoatHouseName"].ToString();
                            string a = sBHouse.Replace("Boat", "");
                            string b = a.Replace("House", "");
                            string result = string.Empty;
                            result = b.Trim();
                            sBoatHouse = result;
                            var ssBoatHouse = sBoatHouse.ToString();
                            sbm.Append(ssBoatHouse);
                            sbm.Append(",");
                            string strBoatHouse = sbm.ToString().TrimEnd(',');
                            pchfBoathouse.Value = strBoatHouse;

                            divGraph.Visible = true;
                            if (PCddlBoatType.SelectedValue != "0" && PCddlBoatSeater.SelectedValue != "0")
                            {
                                pchfheading1.Value = PCddlBoatType.SelectedItem.Text;
                                pchfheading2.Value = PCddlBoatSeater.SelectedItem.Text;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Other Service Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sCategoryId"></param>
    /// <param name="sServiceId"></param>
    /// <param name="sCorpId"></param>
    public void getOtherServices(string sCategoryId, string sServiceId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                FormBody = new Dashboard()
                {
                    QueryType = "OtherServices",
                    Input1 = sCategoryId,
                    Input2 = sServiceId,
                    Input3 = "",
                    CorpId = sCorpId.Trim()
                };

                response = client.PostAsJsonAsync("PriceComparison", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sbc = new StringBuilder();
                        StringBuilder sbm = new StringBuilder();
                        string sBoatHouse = string.Empty;
                        decimal Price = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Price = Convert.ToDecimal(dt.Rows[i]["Total"].ToString());
                            var sPrice = Price.ToString();
                            sbc.Append(sPrice);
                            sbc.Append(",");
                            string strPrice = sbc.ToString().TrimEnd(',');
                            hfPrice.Value = strPrice;

                            string sBHouse = dt.Rows[i]["BoatHouseName"].ToString();
                            string a = sBHouse.Replace("Boat", "");
                            string b = a.Replace("House", "");
                            string result = string.Empty;
                            result = b.Trim();

                            sBoatHouse = result;
                            var ssBoatHouse = sBoatHouse.ToString();
                            sbm.Append(ssBoatHouse);
                            sbm.Append(",");
                            string strBoatHouse = sbm.ToString().TrimEnd(',');
                            pchfBoathouse.Value = strBoatHouse;
                            divGraph.Visible = true;

                            pchfheading1.Value = ddlCategoryOtherService.SelectedItem.Text;
                            pchfheading2.Value = ddlServiceOtherService.SelectedItem.Text;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Restaurant Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 15-July-2023
    /// </summary>
    /// <param name="sCategoryId"></param>
    /// <param name="sServiceId"></param>
    /// <param name="sCorpId"></param>
    public void getRestaurant(string sCategoryId, string sServiceId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                FormBody = new Dashboard()
                {
                    QueryType = "Restaurant",
                    Input1 = sCategoryId,
                    Input2 = sServiceId,
                    Input3 = "",
                    CorpId = sCorpId.Trim()

                };
                response = client.PostAsJsonAsync("PriceComparison", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sbc = new StringBuilder();
                        StringBuilder sbm = new StringBuilder();
                        string sBoatHouse = string.Empty;
                        decimal Price = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Price = Convert.ToDecimal(dt.Rows[i]["Total"].ToString());
                            var sPrice = Price.ToString();
                            sbc.Append(sPrice);
                            sbc.Append(",");
                            string strPrice = sbc.ToString().TrimEnd(',');
                            hfPrice.Value = strPrice;

                            string sBHouse = dt.Rows[i]["BoatHouseName"].ToString();
                            string a = sBHouse.Replace("Boat", "");
                            string b = a.Replace("House", "");
                            string result = string.Empty;
                            result = b.Trim();

                            sBoatHouse = result;
                            var ssBoatHouse = sBoatHouse.ToString();
                            sbm.Append(ssBoatHouse);
                            sbm.Append(",");
                            string strBoatHouse = sbm.ToString().TrimEnd(',');
                            pchfBoathouse.Value = strBoatHouse;
                            divGraph.Visible = true;

                            pchfheading1.Value = ddlCategoryRestaurant.SelectedItem.Text;
                            pchfheading2.Value = ddlServiceRestaurant.SelectedItem.Text;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /************************************DAY WISE REVENUE COMPARISON***********************************/

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnRevenueComparison_Click(object sender, EventArgs e)
    {
        var today = DateTime.Now;
        RCtxtFromDate.Text = today.AddDays(-6).ToString("dd/MM/yyyy");
        RCtxtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = true;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = false;
        BindBoatHouse(Session["CorpId"].ToString().Trim());
        RCddlBoatType.Items.Clear();
        RCddlSeaterType.Items.Clear();
        RevenueComparionBooking(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), RCddlSeaterType.SelectedValue.Trim(), Session["CorpId"].ToString());
        RevenueComparionOtherService(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
        RevenueComparionRestaurant(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());

        RCdivBoatType.Visible = false;
        RCdivSeaterType.Visible = false;
        RCddlServiceName.SelectedValue = "0";
    }

    //Boat Booking Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sBoatHouseId"></param>
    /// <param name="sBoatTypeId"></param>
    /// <param name="sBoatSeaterId"></param>
    /// <param name="sCorpId"></param>
    public void RevenueComparionBooking(string sBoatHouseId, string sBoatTypeId, string sBoatSeaterId, string sCorpId)
    {
        try
        {
            hfBookedAmt.Value = string.Empty;
            hfCancelledAmt.Value = string.Empty;
            hfRescheduleAmt.Value = string.Empty;
            hfRcDate.Value = string.Empty;
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sBoatTypeId == "")
                {
                    sBoatTypeId = "0";
                }

                if (sBoatSeaterId == "")
                {
                    sBoatSeaterId = "0";
                }

                FormBody = new Dashboard()
                {
                    QueryType = "DayWiseRevenueComparisonBoatBooking",
                    BoatHouseId = sBoatHouseId,
                    BoatTypeId = sBoatTypeId,
                    BoatSeaterId = sBoatSeaterId,
                    ServiceType = sCorpId.Trim(),
                    FromDate = RCtxtFromDate.Text,
                    ToDate = RCtxtToDate.Text,
                    BoatStatus = ""
                };

                response = client.PostAsJsonAsync("DayWiseRevenueComparison", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        divRCBoatGraph.Style.Add("display", "block");
                        StringBuilder sbRCD = new StringBuilder();
                        StringBuilder sbBAmt = new StringBuilder();
                        StringBuilder sbCAmt = new StringBuilder();
                        StringBuilder sbRAmt = new StringBuilder();

                        string RCDate = string.Empty;
                        decimal BookingAmt = 0;
                        decimal CancelAmt = 0;
                        decimal ResAmt = 0;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            BookingAmt = Convert.ToDecimal(dt.Rows[i]["BookingAmt"].ToString());
                            var sBookingAmt = BookingAmt.ToString();
                            sbBAmt.Append(sBookingAmt);
                            sbBAmt.Append(",");
                            string strBookingAmt = sbBAmt.ToString().TrimEnd(',');
                            hfBookedAmt.Value = strBookingAmt;

                            CancelAmt = Convert.ToDecimal(dt.Rows[i]["CancelledAmt"].ToString());
                            var sCancelAmt = CancelAmt.ToString();
                            sbCAmt.Append(sCancelAmt);
                            sbCAmt.Append(",");
                            string strCancelAmt = sbCAmt.ToString().TrimEnd(',');
                            hfCancelledAmt.Value = strCancelAmt;

                            ResAmt = Convert.ToDecimal(dt.Rows[i]["RescheduledAmt"].ToString());
                            var sResAmt = ResAmt.ToString();
                            sbRAmt.Append(sResAmt);
                            sbRAmt.Append(",");
                            string strResAmt = sbRAmt.ToString().TrimEnd(',');
                            hfRescheduleAmt.Value = strResAmt;

                            RCDate = dt.Rows[i]["RCDate"].ToString();
                            var sRCDate = RCDate.ToString();
                            sbRCD.Append(sRCDate);
                            sbRCD.Append(",");
                            string strRCDate = sbRCD.ToString().TrimEnd(',');
                            hfRcDate.Value = strRCDate;
                        }
                    }
                    else
                    {
                        divBoatBookingGraph.Style.Add("display", "none");
                    }
                }
                else
                {
                    divBoatBookingGraph.Style.Add("display", "none");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Other Service Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sBoatHouseId"></param>
    /// <param name="sCorpId"></param>
    public void RevenueComparionOtherService(string sBoatHouseId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                FormBody = new Dashboard()
                {
                    QueryType = "DayWiseRevenueComparisonOtherService",
                    BoatHouseId = sBoatHouseId,
                    BoatTypeId = "",
                    BoatSeaterId = "",
                    ServiceType = sCorpId.Trim(),
                    FromDate = RCtxtFromDate.Text,
                    ToDate = RCtxtToDate.Text,
                    BoatStatus = ""
                };

                response = client.PostAsJsonAsync("DayWiseRevenueComparison", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        divRCOtherServiceGraph.Style.Add("display", "block");
                        StringBuilder sbRCD = new StringBuilder();
                        StringBuilder sbOtherSerAmt = new StringBuilder();

                        string Date = string.Empty;
                        decimal OtherServiceAmt = 0;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            OtherServiceAmt = Convert.ToDecimal(dt.Rows[i]["OtherServiceAmt"].ToString());
                            var sOtherServiceAmt = OtherServiceAmt.ToString();
                            sbOtherSerAmt.Append(sOtherServiceAmt);
                            sbOtherSerAmt.Append(",");
                            string strOtherSerAmt = sbOtherSerAmt.ToString().TrimEnd(',');
                            hfOtherServiceAmt.Value = strOtherSerAmt;

                            Date = dt.Rows[i]["BookingDate"].ToString();
                            var sDate = Date.ToString();
                            sbRCD.Append(sDate);
                            sbRCD.Append(",");
                            string strRCDate = sbRCD.ToString().TrimEnd(',');
                            hfRcDate.Value = strRCDate;
                        }
                    }
                    else
                    {
                        divRCOtherServiceGraph.Style.Add("display", "none");
                    }
                }
                else
                {
                    divRCOtherServiceGraph.Style.Add("display", "none");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Restaurant Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sBoatHouseId"></param>
    /// <param name="sCorpId"></param>
    public void RevenueComparionRestaurant(string sBoatHouseId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                FormBody = new Dashboard()
                {
                    QueryType = "DayWiseRevenueComparisonRestaurant",
                    BoatHouseId = sBoatHouseId,
                    BoatTypeId = "",
                    BoatSeaterId = "",
                    ServiceType = sCorpId.Trim(),
                    FromDate = RCtxtFromDate.Text,
                    ToDate = RCtxtToDate.Text,
                    BoatStatus = ""
                };

                response = client.PostAsJsonAsync("DayWiseRevenueComparison", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        divRCRestaurantGraph.Style.Add("display", "block");
                        StringBuilder sbRCD = new StringBuilder();
                        StringBuilder sbResSerAmt = new StringBuilder();

                        string Date = string.Empty;
                        decimal RestaurantAmt = 0;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            RestaurantAmt = Convert.ToDecimal(dt.Rows[i]["RestaurantAmt"].ToString());
                            var sRestaurantAmt = RestaurantAmt.ToString();
                            sbResSerAmt.Append(sRestaurantAmt);
                            sbResSerAmt.Append(",");
                            string strRestaurantAmt = sbResSerAmt.ToString().TrimEnd(',');
                            hfRestaurantAmt.Value = strRestaurantAmt;

                            Date = dt.Rows[i]["BookingDate"].ToString();
                            var sDate = Date.ToString();
                            sbRCD.Append(sDate);
                            sbRCD.Append(",");
                            string strRCDate = sbRCD.ToString().TrimEnd(',');
                            hfRcDate.Value = strRCDate;
                        }
                    }
                    else
                    {
                        divRCRestaurantGraph.Style.Add("display", "none");
                    }
                }
                else
                {
                    divRCRestaurantGraph.Style.Add("display", "none");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RCddlBoatHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        RCdivBoatType.Visible = false;
        RCdivSeaterType.Visible = false;

        RCddlServiceName.SelectedValue = "0";
        RCddlSeaterType.Items.Clear();
        RCddlSeaterType.Items.Insert(0, new ListItem("All", "0"));
        RCddlBoatType.Items.Clear();
        RCddlBoatType.Items.Insert(0, new ListItem("All", "0"));

        RevenueComparionBooking(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), RCddlSeaterType.SelectedValue.Trim(), Session["CorpId"].ToString());
        RevenueComparionOtherService(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
        RevenueComparionRestaurant(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    protected void RCddlServiceName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RCddlServiceName.SelectedValue == "1")
        {
            RCdivBoatType.Visible = true;
            RCdivSeaterType.Visible = true;
            BindBoatType(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            RCddlSeaterType.Items.Clear();
            RCddlSeaterType.Items.Insert(0, new ListItem("All", "0"));
            RevenueComparionBooking(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), RCddlSeaterType.SelectedValue.Trim(), Session["CorpId"].ToString());
            divRCOtherServiceGraph.Style.Add("display", "none");
            divRCRestaurantGraph.Style.Add("display", "none");

        }
        else if (RCddlServiceName.SelectedValue == "2")
        {
            RCdivBoatType.Visible = false;
            RCdivSeaterType.Visible = false;
            RevenueComparionOtherService(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
            divRCBoatGraph.Style.Add("display", "none");
            divRCRestaurantGraph.Style.Add("display", "none");

        }
        else if (RCddlServiceName.SelectedValue == "3")
        {
            RCdivBoatType.Visible = false;
            RCdivSeaterType.Visible = false;
            RevenueComparionRestaurant(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
            divRCBoatGraph.Style.Add("display", "none");
            divRCOtherServiceGraph.Style.Add("display", "none");

        }
        else
        {
            RCdivBoatType.Visible = false;
            RCdivSeaterType.Visible = false;
            RevenueComparionBooking(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), RCddlSeaterType.SelectedValue.Trim(), Session["CorpId"].ToString());
            RevenueComparionOtherService(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
            RevenueComparionRestaurant(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RCddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RCddlBoatType.SelectedValue != "0")
        {
            BindSeaterType(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        }
        else
        {
            RCddlSeaterType.Items.Clear();
            RCddlSeaterType.Items.Insert(0, new ListItem("All", "0"));
        }
        RevenueComparionBooking(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), RCddlSeaterType.SelectedValue.Trim(), Session["CorpId"].ToString());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RCddlSeaterType_SelectedIndexChanged(object sender, EventArgs e)
    {
        RevenueComparionBooking(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), RCddlSeaterType.SelectedValue.Trim(), Session["CorpId"].ToString());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RCbtnReset_Click(object sender, EventArgs e)
    {
        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = true;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = false;
        BindBoatHouse(Session["CorpId"].ToString());
        RCddlBoatType.Items.Clear();
        RCddlSeaterType.Items.Clear();

        RCdivBoatType.Visible = false;
        RCdivSeaterType.Visible = false;

        RCddlServiceName.SelectedValue = "0";

        var today = DateTime.Now;
        RCtxtFromDate.Text = today.AddDays(-6).ToString("dd/MM/yyyy");
        RCtxtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        RevenueComparionBooking(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), RCddlSeaterType.SelectedValue.Trim(), Session["CorpId"].ToString());
        RevenueComparionOtherService(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
        RevenueComparionRestaurant(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RCbtnSearch_Click(object sender, EventArgs e)
    {
        RevenueComparionBooking(RCddlBoatHouse.SelectedValue.Trim(), RCddlBoatType.SelectedValue.Trim(), RCddlSeaterType.SelectedValue.Trim(), Session["CorpId"].ToString());
        RevenueComparionOtherService(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
        RevenueComparionRestaurant(RCddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString());
    }

    //************************ BOAT RUNNING STATUS ************************************** Need To Validate CorpId/

    //Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023    /// </summary>
    /// <param name="sBoatHouseId"></param>
    /// <param name="sBoatTypeId"></param>
    /// <param name="sBoatSeaterId"></param>
    /// <param name="sCorpId"></param>
    public void BoatRunningStatus(string sBoatHouseId, string sBoatTypeId, string sBoatSeaterId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sBoatTypeId == "")
                {
                    sBoatTypeId = "0";
                }
                if (sBoatSeaterId == "")
                {
                    sBoatSeaterId = "0";
                }

                string sQueryType = string.Empty;
                if (BRSrbtnBoatNature.SelectedValue == "N")
                {
                    sQueryType = "BoatRunningStatusNormal";
                }
                else
                {
                    sQueryType = "BoatRunningStatusPremium";
                }

                FormBody = new Dashboard()
                {
                    QueryType = sQueryType,
                    BoatHouseId = sBoatHouseId,
                    BoatTypeId = sBoatTypeId,
                    BoatSeaterId = sBoatSeaterId,
                };

                response = client.PostAsJsonAsync("BoatRunningStatus", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        divBoatRunningStatusGraph.Style.Add("display", "block");

                        StringBuilder sbBRSHeading = new StringBuilder();
                        StringBuilder sbBRSTotalCapacity = new StringBuilder();
                        StringBuilder sbBRSBookedTrips = new StringBuilder();
                        StringBuilder sbBRSOnTravelTrips = new StringBuilder();
                        StringBuilder sbBRSAvailableTrips = new StringBuilder();

                        string sBRSHeading = string.Empty;
                        string sBRSTotalCapacity = string.Empty;
                        string sBRSBookedTrips = string.Empty;
                        string sBRSOnTravelTrips = string.Empty;
                        string sBRSAvailableTrips = string.Empty;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sBRSHeading = dt.Rows[i]["BoatType-Seater"].ToString();
                            var BRSHeading = sBRSHeading.ToString();
                            sbBRSHeading.Append(BRSHeading);
                            sbBRSHeading.Append(",");
                            string strBRSHeading = sbBRSHeading.ToString().TrimEnd(',');
                            hfBRSHeading.Value = strBRSHeading;

                            sBRSTotalCapacity = dt.Rows[i]["TotalCapacity"].ToString();
                            var BRSTotalCapacity = sBRSTotalCapacity.ToString();
                            sbBRSTotalCapacity.Append(BRSTotalCapacity);
                            sbBRSTotalCapacity.Append(",");
                            string strBRSTotalCapacity = sbBRSTotalCapacity.ToString().TrimEnd(',');
                            hfBRSTotalCapacity.Value = strBRSTotalCapacity;

                            sBRSBookedTrips = dt.Rows[i]["BookedTrips"].ToString();
                            var BRSBookedTrips = sBRSBookedTrips.ToString();
                            sbBRSBookedTrips.Append(BRSBookedTrips);
                            sbBRSBookedTrips.Append(",");
                            string strBRSBookedTrips = sbBRSBookedTrips.ToString().TrimEnd(',');
                            hfBRSBookedTrips.Value = strBRSBookedTrips;

                            sBRSOnTravelTrips = dt.Rows[i]["OnTravelTrips"].ToString();
                            var BRSOnTravelTrips = sBRSOnTravelTrips.ToString();
                            sbBRSOnTravelTrips.Append(BRSOnTravelTrips);
                            sbBRSOnTravelTrips.Append(",");
                            string strBRSOnTravelTrips = sbBRSOnTravelTrips.ToString().TrimEnd(',');
                            hfBRSOnTravelTrips.Value = strBRSOnTravelTrips;

                            sBRSAvailableTrips = dt.Rows[i]["AvailableTrips"].ToString();
                            var BRSAvailableTrips = sBRSAvailableTrips.ToString();
                            sbBRSAvailableTrips.Append(BRSAvailableTrips);
                            sbBRSAvailableTrips.Append(",");
                            string strBRSAvailableTrips = sbBRSAvailableTrips.ToString().TrimEnd(',');
                            hfBRSAvailableTrips.Value = strBRSAvailableTrips;
                        }
                    }
                    else
                    {
                        divBoatRunningStatusGraph.Style.Add("display", "none");
                    }
                }
                else
                {
                    divBoatRunningStatusGraph.Style.Add("display", "none");
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnBoatRunningStatus_Click(object sender, EventArgs e)
    {
        BindBoatHouse(Session["CorpId"].ToString().Trim());
        BRSrbtnBoatNature.SelectedValue = "N";
        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = true;
        divBoatUtilization.Visible = false;

        if (BRSddlBoatHouse.SelectedValue == "0")
        {
            divBRSGraph.Visible = false;
            divBRSBoatType.Visible = false;
            divBRSBoatSeater.Visible = false;
            BRSddlBoatSeater.Items.Clear();
            BRSddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
            BRSddlBoatType.Items.Clear();
            BRSddlBoatType.Items.Insert(0, new ListItem("All", "0"));
        }
        else
        {
            BindBoatType(BRSddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            BRSddlBoatSeater.Items.Clear();
            BRSddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
            BoatRunningStatus(BRSddlBoatHouse.SelectedValue.Trim(), BRSddlBoatType.SelectedValue.Trim(), BRSddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            divBRSBoatType.Visible = true;
            divBRSBoatSeater.Visible = true;
            divBRSGraph.Visible = true;
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BRSddlBoatHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BRSddlBoatHouse.SelectedValue != "0")
        {
            BindBoatType(BRSddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            divBRSBoatType.Visible = true;
            divBRSBoatSeater.Visible = true;
            BRSddlBoatSeater.Items.Clear();
            BRSddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
            divBRSGraph.Visible = true;
            BoatRunningStatus(BRSddlBoatHouse.SelectedValue.Trim(), BRSddlBoatType.SelectedValue.Trim(), BRSddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString());
        }
        else
        {
            divBRSBoatType.Visible = false;
            divBRSBoatSeater.Visible = false;
            BRSddlBoatType.Items.Clear();
            BRSddlBoatType.Items.Insert(0, new ListItem("All", "0"));
            BRSddlBoatSeater.Items.Clear();
            BRSddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
            divBRSGraph.Visible = false;
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BRSddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BRSddlBoatType.SelectedValue != "0")
        {
            BindSeaterType(BRSddlBoatHouse.SelectedValue.Trim(), BRSddlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        }
        else
        {
            BRSddlBoatSeater.Items.Clear();
            BRSddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BRSbtnSearch_Click(object sender, EventArgs e)
    {
        divBRSGraph.Visible = true;
        BoatRunningStatus(BRSddlBoatHouse.SelectedValue.Trim(), BRSddlBoatType.SelectedValue.Trim(), BRSddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BRSbtnReset_Click(object sender, EventArgs e)
    {
        BindBoatHouse(Session["CorpId"].ToString());
        BRSrbtnBoatNature.SelectedValue = "N";
        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = true;
        divBoatUtilization.Visible = false;
        if (BRSddlBoatHouse.SelectedValue == "0")
        {
            divBRSGraph.Visible = false;
            divBRSBoatType.Visible = false;
            divBRSBoatSeater.Visible = false;
            BRSddlBoatSeater.Items.Clear();
            BRSddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
            BRSddlBoatType.Items.Clear();
            BRSddlBoatType.Items.Insert(0, new ListItem("All", "0"));
        }
        else
        {
            BindBoatType(BRSddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
            BRSddlBoatSeater.Items.Clear();
            BRSddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
            BoatRunningStatus(BRSddlBoatHouse.SelectedValue.Trim(), BRSddlBoatType.SelectedValue.Trim(), BRSddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString());
            divBRSBoatType.Visible = true;
            divBRSBoatSeater.Visible = true;
            divBRSGraph.Visible = true;
        }
    }

    /************************ BOAT UTILIZATION **************************************/

    //=== Need To Validate
    //Chart Method
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 17-July-2023
    /// </summary>
    /// <param name="sBoatHouseId"></param>
    /// <param name="sBoatTypeId"></param>
    /// <param name="sBoatSeaterId"></param>
    /// <param name="sCorpId"></param>
    public void BoatUtilization(string sBoatHouseId, string sBoatTypeId, string sBoatSeaterId, string sCorpId)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sMSG = string.Empty;

                if (sBoatTypeId == "")
                {
                    sBoatTypeId = "0";
                }
                if (sBoatSeaterId == "")
                {
                    sBoatSeaterId = "0";
                }

                string sQueryType = string.Empty;
                if (sBoatHouseId == "0" && sBoatTypeId == "0" && sBoatSeaterId == "0")
                {
                    sQueryType = "BoatUtilizationBasedOnAllBH";
                }
                else
                {
                    if (sBoatHouseId != "0" && sBoatTypeId == "0" && sBoatSeaterId == "0")
                    {
                        sQueryType = "BoatUtilizationBasedOnBH";
                    }
                    else if (sBoatHouseId != "0" && sBoatTypeId != "0" && sBoatSeaterId == "0")
                    {
                        sQueryType = "BoatUtilizationBasedOnBHBT";
                    }
                    else if (sBoatHouseId != "0" && sBoatTypeId != "0" && sBoatSeaterId != "0")
                    {
                        sQueryType = "BoatUtilizationBasedOnBHBS";
                    }
                    else if (sBoatHouseId == "0" && sBoatTypeId != "0" && sBoatSeaterId == "0")
                    {
                        sQueryType = "BoatUtilizationBasedOnAllBT";
                    }
                    else if (sBoatHouseId == "0" && sBoatTypeId != "0" && sBoatSeaterId != "0")
                    {
                        sQueryType = "BoatUtilizationBasedOnAllBS";
                    }
                }

                FormBody = new Dashboard()
                {
                    QueryType = sQueryType,
                    BoatHouseId = sBoatHouseId,
                    BoatTypeId = sBoatTypeId,
                    BoatSeaterId = sBoatSeaterId,
                    FromDate = BUFromDate.Text,
                    ToDate = BUToDate.Text,
                    CorpId = sCorpId.Trim()

                };
                response = client.PostAsJsonAsync("BoatUtilization", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        divBuGraphTripCount.Style.Add("display", "block");
                        divBuGraphRevenue.Style.Add("display", "block");

                        StringBuilder sbBUBoatHouse = new StringBuilder();
                        StringBuilder sbBUTotalCapacity = new StringBuilder();
                        StringBuilder sbBUBookedTripsNos = new StringBuilder();
                        StringBuilder sbBUBookedTripsRevenue = new StringBuilder();
                        StringBuilder sbBUUnBookedTrips = new StringBuilder();
                        StringBuilder sbBURevenueLoss = new StringBuilder();
                        StringBuilder sbBURevenueGain = new StringBuilder();
                        StringBuilder sbBUTotCapacityRevenue = new StringBuilder();
                        StringBuilder sbBUUnBookedRevenue = new StringBuilder();

                        string sBUBoatHouse = string.Empty;
                        string sBUTotalCapacity = string.Empty;
                        string sBUBookedTripsNos = string.Empty;
                        string sBUBookedTripsRevenue = string.Empty;
                        string sBUUnBookedTrips = string.Empty;
                        string sBURevenueLoss = string.Empty;
                        string sBURevenueGain = string.Empty;
                        string sBUTotCapacityRevenue = string.Empty;
                        string sBUUnBookedRevenue = string.Empty;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sBHouse = dt.Rows[i]["BoatHouse"].ToString();
                            string a = sBHouse.Replace("Boat", "");
                            string b = a.Replace("House", "");
                            string result = string.Empty;
                            result = b.Trim();
                            sBUBoatHouse = result;
                            var BUBoatHouse = sBUBoatHouse.ToString();
                            sbBUBoatHouse.Append(BUBoatHouse);
                            sbBUBoatHouse.Append(",");
                            string strBUBoatHouse = sbBUBoatHouse.ToString().TrimEnd(',');
                            hfBUBoatHouse.Value = strBUBoatHouse;

                            sBUTotalCapacity = dt.Rows[i]["TotalCapacity"].ToString();
                            var BUTotalCapacity = sBUTotalCapacity.ToString();
                            sbBUTotalCapacity.Append(BUTotalCapacity);
                            sbBUTotalCapacity.Append(",");
                            string strBUTotalCapacity = sbBUTotalCapacity.ToString().TrimEnd(',');
                            hfBUTotalCapacity.Value = strBUTotalCapacity;

                            sBUBookedTripsNos = dt.Rows[i]["BookedTrips(Nos)"].ToString();
                            var BUBookedTripsNos = sBUBookedTripsNos.ToString();
                            sbBUBookedTripsNos.Append(BUBookedTripsNos);
                            sbBUBookedTripsNos.Append(",");
                            string strBUBookedTripsNos = sbBUBookedTripsNos.ToString().TrimEnd(',');
                            hfBUBookedTripsNos.Value = strBUBookedTripsNos;

                            sBUBookedTripsRevenue = dt.Rows[i]["BookedTrips(Revenue)"].ToString();
                            var BUBookedTripsRevenue = sBUBookedTripsRevenue.ToString();
                            sbBUBookedTripsRevenue.Append(BUBookedTripsRevenue);
                            sbBUBookedTripsRevenue.Append(",");
                            string strBUBookedTripsRevenue = sbBUBookedTripsRevenue.ToString().TrimEnd(',');
                            hfBUBookedTripsRevenue.Value = strBUBookedTripsRevenue;

                            sBUUnBookedTrips = dt.Rows[i]["UnBookedTrips"].ToString();
                            var BUUnBookedTrips = sBUUnBookedTrips.ToString();
                            sbBUUnBookedTrips.Append(BUUnBookedTrips);
                            sbBUUnBookedTrips.Append(",");
                            string strBUUnBookedTrips = sbBUUnBookedTrips.ToString().TrimEnd(',');
                            hfBUUnBookedTrips.Value = strBUUnBookedTrips;

                            sBURevenueLoss = dt.Rows[i]["RevenueLoss(%)"].ToString();
                            var BURevenueLoss = sBURevenueLoss.ToString();
                            sbBURevenueLoss.Append(BURevenueLoss);
                            sbBURevenueLoss.Append(",");
                            string strBURevenueLoss = sbBURevenueLoss.ToString().TrimEnd(',');
                            hfBURevenueLoss.Value = strBURevenueLoss;

                            sBURevenueGain = dt.Rows[i]["RevenueGain(%)"].ToString();
                            var BURevenueGain = sBURevenueGain.ToString();
                            sbBURevenueGain.Append(BURevenueGain);
                            sbBURevenueGain.Append(",");
                            string strBURevenueGain = sbBURevenueGain.ToString().TrimEnd(',');
                            hfBURevenueGain.Value = strBURevenueGain;

                            sBUTotCapacityRevenue = dt.Rows[i]["TotCapacityRevenue"].ToString();
                            var BUTotCapacityRevenue = sBUTotCapacityRevenue.ToString();
                            sbBUTotCapacityRevenue.Append(BUTotCapacityRevenue);
                            sbBUTotCapacityRevenue.Append(",");
                            string strBUTotCapacityRevenue = sbBUTotCapacityRevenue.ToString().TrimEnd(',');
                            hfBUTotCapacityRevenue.Value = strBUTotCapacityRevenue;

                            sBUUnBookedRevenue = dt.Rows[i]["UnBookedRevenue"].ToString();
                            var BUUnBookedRevenue = sBUUnBookedRevenue.ToString();
                            sbBUUnBookedRevenue.Append(BUUnBookedRevenue);
                            sbBUUnBookedRevenue.Append(",");
                            string strBUUnBookedRevenue = sbBUUnBookedRevenue.ToString().TrimEnd(',');
                            hfBUUnBookedRevenue.Value = strBUUnBookedRevenue;
                        }
                    }
                    else
                    {
                        divBuGraphTripCount.Style.Add("display", "none");
                        divBuGraphRevenue.Style.Add("display", "block");
                    }
                }
                else
                {
                    divBuGraphTripCount.Style.Add("display", "none");
                    divBuGraphRevenue.Style.Add("display", "block");
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 17-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BUddlBoatHouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBoatType(BUddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        BUddlBoatSeater.Items.Clear();
        BUddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        divBuGraphTripCount.Visible = true;
        divBuGraphRevenue.Visible = true;
        BoatUtilization(BUddlBoatHouse.SelectedValue.Trim(), BUddlBoatType.SelectedValue.Trim(), BUddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 17-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BUddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BUddlBoatType.SelectedValue != "0")
        {
            BindSeaterType(BUddlBoatHouse.SelectedValue.Trim(), BUddlBoatType.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        }
        else
        {
            BUddlBoatSeater.Items.Clear();
            BUddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 17-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnBoatUtilization_Click(object sender, EventArgs e)
    {
        var today = DateTime.Now;
        BUFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        BUToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = true;
        BindBoatHouse(Session["CorpId"].ToString().Trim());
        BindBoatType(BUddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        BUddlBoatSeater.Items.Clear();
        BUddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        BoatUtilization(BUddlBoatHouse.SelectedValue.Trim(), BUddlBoatType.SelectedValue.Trim(), BUddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        divBuGraphTripCount.Visible = true;
        divBuGraphRevenue.Visible = true;
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 17-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BUbtnSearch_Click(object sender, EventArgs e)
    {
        divBuGraphTripCount.Visible = true;
        divBuGraphRevenue.Visible = true;
        BoatUtilization(BUddlBoatHouse.SelectedValue.Trim(), BUddlBoatType.SelectedValue.Trim(), BUddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 17-July-2023 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BUbtnReset_Click(object sender, EventArgs e)
    {
        var today = DateTime.Now;
        BUFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        BUToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        divBookingSummary.Visible = false;
        divAbstractBoatCount.Visible = false;
        divPriceComparison.Visible = false;
        divRevenueComparison.Visible = false;
        divBoatRunningStatus.Visible = false;
        divBoatUtilization.Visible = true;

        BindBoatHouse(Session["CorpId"].ToString().Trim());
        BindBoatType(BUddlBoatHouse.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        BUddlBoatSeater.Items.Clear();
        BUddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
        BoatUtilization(BUddlBoatHouse.SelectedValue.Trim(), BUddlBoatType.SelectedValue.Trim(), BUddlBoatSeater.SelectedValue.Trim(), Session["CorpId"].ToString().Trim());
        divBuGraphTripCount.Visible = true;
        divBuGraphRevenue.Visible = true;
    }

    /********************************************************************************************/

    public class Dashboard
    {
        public string BoatHouseId { get; set; }
        public string QueryType { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string Category { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string ServiceType { get; set; }
        public string BoatId { get; set; }
        public string BoatHouseName { get; set; }
        public string BoatStatus { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BoatStatusId { get; set; }
        public string CreatedBy { get; set; }
        public string UserId { get; set; }
        public string CorpId { get; set; }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgDownloadExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                FormBody = new Dashboard()
                {
                    QueryType = "ExcelQuery",
                    BoatHouseId = "0",
                    FromDate = txtFromDate.Text.Trim(),
                    UserId = "0",
                    ToDate = txtToDate.Text.Trim(),
                    CorpId = Session["CorpId"].ToString().Trim()

                };

                response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    GetResponse = response.Content.ReadAsStringAsync().Result;
                    ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        grvAllExcelRevenue.DataSource = dt;
                        grvAllExcelRevenue.DataBind();

                        grvAllExcelRevenue.FooterRow.Cells[1].Text = "Total";

                        grvAllExcelRevenue.FooterRow.Cells[2].Text = dt.Compute("SUM(Boating)", "").ToString();
                        grvAllExcelRevenue.FooterRow.Cells[3].Text = dt.Compute("SUM(OtherService)", "").ToString();
                        grvAllExcelRevenue.FooterRow.Cells[4].Text = dt.Compute("SUM(Restaurant)", "").ToString();
                        grvAllExcelRevenue.FooterRow.Cells[5].Text = dt.Compute("SUM(OtherRevenue)", "").ToString();
                        grvAllExcelRevenue.FooterRow.Cells[6].Text = dt.Compute("SUM(Total)", "").ToString();
                        grvAllExcelRevenue.FooterRow.Cells[7].Text = dt.Compute("SUM(Average)", "").ToString();

                        ExcelDownload();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered, tem a ver com o botão de exportação para excel*/
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    public void ExcelDownload()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=BoatHouseIncomeDetails.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            grvAllExcelRevenue.HeaderRow.BackColor = System.Drawing.Color.White;

            foreach (TableCell cell in grvAllExcelRevenue.HeaderRow.Cells)
            {
                cell.BackColor = grvAllExcelRevenue.HeaderStyle.BackColor;
            }

            //foreach (GridViewRow row in grvAllExcelRevenue.Rows)
            //{
            //    row.BackColor = System.Drawing.Color.White;
            //    foreach (TableCell cell in row.Cells)
            //    {
            //        if (row.RowIndex % 2 == 0)
            //        {
            //            cell.BackColor = grvAllExcelRevenue.AlternatingRowStyle.BackColor;
            //        }
            //        else
            //        {
            //            cell.BackColor = grvAllExcelRevenue.RowStyle.BackColor;
            //        }
            //    }
            //}

            grvAllExcelRevenue.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        //string sFileName = string.Empty;

        //sFileName = "BoatHouseIncomeDetails.xls";

        //Response.Clear();
        //Response.Buffer = true;
        //Response.ClearContent();
        //Response.ClearHeaders();
        //Response.Charset = "";
        //System.IO.StringWriter strwritter = new System.IO.StringWriter();
        //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ContentType = "application/vnd.ms-excel";
        //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", sFileName.Trim()));
        //grvAllExcelRevenue.Visible = true;
        //grvAllExcelRevenue.RenderControl(htmltextwrtter);
        //Response.Write(strwritter.ToString());
        //Response.End();
    }

    ///Rower Revenue Pop-Up
    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="CorpId"></param>
    public void BindRowerRevenuePopup(string sCorpId)
    {
        try
        {
            if (lblRowerOverallRevenue.Text != "0")
            {
                using (client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {

                        FormBody = new Dashboard()
                        {
                            QueryType = "RowerRevenue",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            UserId = "0",
                            FromDate = txtFromDate.Text.Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }
                    else
                    {
                        FormBody = new Dashboard()
                        {
                            QueryType = " RowerRevenueUser",
                            BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            FromDate = txtFromDate.Text.Trim(),
                            ToDate = txtToDate.Text.Trim(),
                            CorpId = sCorpId.Trim()
                        };
                    }

                    response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        GetResponse = response.Content.ReadAsStringAsync().Result;
                        ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvRowerpopup.DataSource = dt;
                            gvRowerpopup.DataBind();
                            gvRowerpopup.Visible = true;

                            decimal totAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("RowerRevenue")));

                            gvRowerpopup.FooterRow.Cells[1].Text = "Total";
                            gvRowerpopup.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            gvRowerpopup.FooterRow.Cells[2].Text = totAmt.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            gvRowerpopup.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            MPERowerpopup.Show();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                        MPERowerpopup.Hide();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgCloseRower_Click(object sender, ImageClickEventArgs e)
    {
        MPERowerpopup.Hide();
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lblRowerOverallRevenue_Click(object sender, EventArgs e)
    {
        BindBookingCountAmount(Session["CorpId"].ToString().Trim());
        BindBookingOtherRevenueCountAmount(Session["CorpId"].ToString().Trim());
        BindRowerRevenuePopup(Session["CorpId"].ToString().Trim());
    }

    /// <summary>
    /// Author : Vediyappan.V
    /// Date : 13-July-2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvRowerpopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRowerpopup.PageIndex = e.NewPageIndex;
        BindRowerRevenuePopup(Session["CorpId"].ToString().Trim());
    }
}