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
/// Created By:Pretheka.C
/// Created Date:19-10-2021
/// </summary>
public partial class Boating_DeactivateBoatSlotMaster : System.Web.UI.Page
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
                BindBoatHouse();
                GetBoatType();
                BinBoatHouseTime();
                btnDeactive.Visible = true;
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
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
                    QueryType = "BindSlotActiveInActive",
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
                        gvBoatSlot.DataSource = dtExists;
                        gvBoatSlot.DataBind();
                        lblGridMsg.Text = string.Empty;
                        if (Session["UserRole"].ToString().Trim() == "Sadmin")
                        {
                            divDeactive.Visible = true;
                        }
                    }
                    else
                    {


                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Slot Details Not Found !');", true);
                        divGrid.Visible = false;
                        divDeactive.Visible = false;
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
                        ddlBoatType.Items.Insert(0, new ListItem("All", "0"));
                        ddlBoatSeatId.Items.Insert(0, new ListItem("All", "0"));
                        //ddlBoatType.Items.Insert(0, "Select Boat Type");
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
            ddlBoatSeatId.Items.Insert(0, new ListItem("All", "0"));
            // ddlBoatSeatId.Items.Insert(0, "Select Boat Seater");
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
    /// Selected Index Change of Slot Type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSlotType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBoatType.SelectedIndex = 0;
        btnDeactive.Visible = true;
        ddlBoatSeatId.Items.Clear();
        BindActiveSlot();
        ddlBoatSeatId.Items.Insert(0, new ListItem("All", "0"));

    }
    /// <summary>
    /// Check Active Slot Type
    /// </summary>
    public void BindActiveSlot()
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

                    ServiceType = ddlSlotType.SelectedValue.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSlotTypeChk", BoatSlotMaster).Result;

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
                        }


                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
                        }


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                        ClearInputs();
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
            return;
        }
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
                //if (ddlSlotType.SelectedValue == 0)
                //{

                //}
                ViewState["SlotType"] = ddlSlotType.SelectedValue.Trim();
                if (ViewState["SlotType"].ToString().Trim() == "0")
                {
                    hfSlotType.Value = "";
                    hfQueryType.Value = "GetDeactiveWithoutSlot";
                }
                else
                {
                    hfSlotType.Value = ddlSlotType.SelectedValue.Trim();
                    hfQueryType.Value = "GetDeactiveWithSlot";
                }

                var BoatSlotMaster = new BoatSlotMaster()
                {

                    QueryType = hfQueryType.Value.Trim(),
                    ServiceType = hfSlotType.Value.Trim(),
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
                        btnDeactive.Visible = true;
                    }
                    else
                    {
                        ddlBoatSeatId.DataBind();
                        btnDeactive.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('There Is No Seater Available for Deactivate !');", true);
                    }
                    //ddlBoatSeatId.DataBind();
                    //ddlBoatSeatId.Items.Insert(0, "Select Boat Seater");
                    ddlBoatSeatId.Items.Insert(0, new ListItem("All", "0"));
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
    /// This Method is Used To Clear Input
    /// </summary>
    public void ClearInputs()
    {
        ddlBoatType.SelectedIndex = 0;
        ddlBoatSeatId.Items.Clear();
        ddlSlotType.SelectedIndex = 0;
        ddlBoatSeatId.Items.Insert(0, new ListItem("All", "0"));

        btnDeactive.Visible = true;

    }
    /// <summary>
    /// This Method Is Used To Deactivate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (ddlBoatSeatId.SelectedValue.Trim() != "")
                {
                    hfSeaterId.Value = ddlBoatSeatId.SelectedValue.Trim();
                }
                else
                {
                    hfSeaterId.Value = "0";
                }

                var BoatSlotMaster = new BoatSlotMaster()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    ServiceType = ddlSlotType.SelectedValue.Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = hfSeaterId.Value.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("DeactiveBoatSlotMaster", BoatSlotMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatHouse();
                        ClearInputs();

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                    }
                    else
                    {
                        BindBoatHouse();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
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
    /// This Method Is Used To Clear Input
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
    }

    protected void gvBoatSlot_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // for example, the row contains a certain property
                if (((Label)e.Row.FindControl("lblActiveStatus")).Text == "D")
                {

                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = false;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = true;
                }

                else
                {

                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = true;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = false;
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