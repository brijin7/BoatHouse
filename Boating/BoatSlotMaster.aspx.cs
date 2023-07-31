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
/// <summary>
/// Developed By K.Abhinaya
/// Date 23/Jul/2021
/// </summary>
public partial class Boating_BoatSlotMaster : System.Web.UI.Page
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
                GetBoatType();
                BinBoatHouseTime();
                BindBoatHouse();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    /// <summary>
    /// This Method Is Used For Getting BoatType
    /// </summary>
    public void GetBoatType()
    {
        try
        {
            ddlBoatType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatSlotMaster = new BoatSlotMaster()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlBoatType", BoatSlotMaster).Result;

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
                            ddlBoatType.DataSource = dt;
                            ddlBoatType.DataValueField = "BoatTypeId";
                            ddlBoatType.DataTextField = "BoatType";
                            ddlBoatType.DataBind();
                        }
                        else
                        {
                            ddlBoatType.DataBind();
                        }
                        //ddlBoatType.DataBind();
                        ddlBoatType.Items.Insert(0, "Select Boat Type");
                    }
                    else
                    {
                        return;
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
    /// This Method Is Used BoatType Will Change Bind BOat Seat And Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBoatType.SelectedIndex != 0)
        {
            GetBoatSeat();
            // BinBoatSlotMasterBoatType();
        }
        else
        {
            ddlBoatSeatId.Items.Clear();
            ddlBoatSeatId.Items.Insert(0, "Select Boat Seater");
        }
    }
    protected void ddlSlotType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBoatType.SelectedIndex = 0;
    }
    /// <summary>
    /// This Method Is Used For Getting SeaterType
    /// </summary>
    public void GetBoatSeat()
    {
        try
        {
            ddlBoatSeatId.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatSlotMaster = new BoatSlotMaster()
                {

                    QueryType = "SeaterType",
                    //ServiceType = "",
                    ServiceType = ddlSlotType.SelectedValue.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    SlotDuration = "",
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = "",
                    CreatedBy = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSlotMaster", BoatSlotMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlBoatSeatId.DataSource = dtExists;
                        ddlBoatSeatId.DataValueField = "BoatSeaterId";
                        ddlBoatSeatId.DataTextField = "SeaterType";
                        ddlBoatSeatId.DataBind();
                    }
                    else
                    {
                        ddlBoatSeatId.DataBind();
                    }
                    //ddlBoatSeatId.DataBind();
                    ddlBoatSeatId.Items.Insert(0, "Select Boat Seater");
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
    /// This Method Is Used  Bind Boat Seat  Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBoatSeatId_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BinBoatSlotMasterSeatType();
    }
    /// <summary>
    /// This Method Is Used For Insert In BoatSlotMaster Table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                string sMSG = string.Empty;

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var BoatSlotMaster = new BoatSlotMaster()
                    {
                        QueryType = "InsertBoatSlot",
                        ServiceType = ddlSlotType.SelectedValue.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        SlotDuration = dlOh1OpenHours.SelectedValue.Trim(),
                        BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                        BoatSeaterId = ddlBoatSeatId.SelectedValue.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("BoatSlotMaster", BoatSlotMaster).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            sMSG = dtExists.Rows[0]["Message"].ToString();
                            if (sMSG.Trim() == "Sorry Normal Boat Not Available")
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + sMSG.ToString().Trim() + "');", true);
                            }
                            else if (sMSG.Trim() == "Boat Slot Details Already Added")
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + sMSG.ToString().Trim() + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.ToString().Trim() + "');", true);
                            }
                            GetMaximumTrips();
                            ClearInputs();
                            BindBoatHouse();
                            divGrid.Visible = true;
                        }
                        else
                        {
                            divGrid.Visible = true;
                            GetMaximumTrips();
                            ClearInputs();
                            BindBoatHouse();
                            btnSubmit.Text = "Submit";
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
    public void GetMaximumTrips()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                string sMSG = string.Empty;



                var BoatSlotMaster = new BoatSlotMaster()
                {
                    QueryType = "InsertAvailableCount",
                    ServiceType = ddlSlotType.SelectedValue.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    SlotDuration = "",
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeatId.SelectedValue.Trim(),
                    CreatedBy = ""
                };

                response = client.PostAsJsonAsync("BoatSlotMaster", BoatSlotMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        //sMSG = dtExists.Rows[0]["Message"].ToString();
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.ToString().Trim() + "');", true);

                    }
                    else
                    {
                        divGrid.Visible = true;

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
    /// Clearing All Values
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
    }
    /// <summary>
    /// Clearing All Values
    /// </summary>
    public void ClearInputs()
    {
        ddlBoatType.SelectedIndex = 0;
        ddlBoatSeatId.Items.Clear();
        ddlSlotType.SelectedIndex = 0;
        dlOh1OpenHours.SelectedIndex = 0;
    }
    /// <summary>
    /// Binding Grid Based On BoatType
    /// </summary>
    public void BinBoatSlotMasterBoatType()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSlotMaster = new BoatSlotMaster()
                {
                    QueryType = "GetBoatSlotMaster",
                    ServiceType = "WithBoatType",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    SlotDuration = "",
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = "",
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSlotMaster", BoatSlotMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvBoatSlot.DataSource = dtExists;
                        gvBoatSlot.DataBind();
                        lblGridMsg.Text = string.Empty;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Slot Details Not Found !');", true);
                        divGrid.Visible = false;
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
    /// <summary>
    ///  Binding Grid Based On BoatSeat
    /// </summary>
    public void BinBoatSlotMasterSeatType()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSlotMaster = new BoatSlotMaster()
                {
                    QueryType = "GetBoatSlotMaster",
                    ServiceType = "WithBoatSeater",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    SlotDuration = "",
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeatId.SelectedValue.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSlotMaster", BoatSlotMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvBoatSlot.DataSource = dtExists;
                        gvBoatSlot.DataBind();
                        lblGridMsg.Text = string.Empty;
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Slot Details Not Found !');", true);
                        divGrid.Visible = false;
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
    /// <summary>
    /// Binding Grid Based On BoatHouse
    /// </summary>
    public void BindBoatHouse()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSlotMaster = new BoatSlotMaster()
                {
                    QueryType = "GetBoatSlotMaster",
                    ServiceType = "WithBoatHouseId",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    SlotDuration = "",
                    BoatTypeId = "",
                    BoatSeaterId = "",
                    CreatedBy = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSlotMaster", BoatSlotMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvBoatSlot.DataSource = dtExists;
                        gvBoatSlot.DataBind();
                        lblGridMsg.Text = string.Empty;
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Slot Details Not Found !');", true);
                        divGrid.Visible = false;
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
    /// Binding Label Based On BoatHouseTime
    /// </summary>
    public void BinBoatHouseTime()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSlotMaster = new BoatSlotMaster()
                {
                    QueryType = "GetSrtEndTime",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    SlotDuration = "",
                    BoatTypeId = "",
                    BoatSeaterId = "",
                    CreatedBy = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSlotMaster", BoatSlotMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        BoatHseStartTme.Text = dtExists.Rows[0]["BookingFrom"].ToString().Trim();
                        BoatHseEndTme.Text = dtExists.Rows[0]["BookingTo"].ToString().Trim();
                    }
                    else
                    {

                        ClearInputs();

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
    /// <summary>
    /// This Method Is Used To Allow Slot Duration Based On BoatMinTime
    /// </summary>
    public void CheckSlotDuration()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSlotMaster = new BoatSlotMaster()
                {
                    QueryType = "GetBoatMinTime",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    SlotDuration = "",
                    BoatTypeId = "",
                    BoatSeaterId = "",
                    CreatedBy = ""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatSlotMaster", BoatSlotMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        int BoatTime = Convert.ToInt32(dtExists.Rows[0]["BoatMinTime"].ToString().Trim());

                        if (BoatTime <= 30)
                        {
                            dlOh1OpenHours.SelectedValue = "00:30:00";
                            dlOh1OpenHours.Enabled = false;
                        }
                        else
                        {
                            dlOh1OpenHours.SelectedValue = "01:00:00";
                            dlOh1OpenHours.Enabled = false;
                        }
                    }
                    else
                    {
                        dlOh1OpenHours.Enabled = true;
                    }
                }
                else
                {
                    dlOh1OpenHours.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public class BoatSlotMaster
    {
        public string QueryType { get; set; }
        public string SlotId { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string SlotStartTime { get; set; }
        public string SlotEndTime { get; set; }
        public string SlotDuration { get; set; }
        public string BoatHouseId { get; set; }
        public string AvailableSlot { get; set; }
        public string CreatedBy { get; set; }
        public string ServiceType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }
}