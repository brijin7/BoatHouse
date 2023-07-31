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

public partial class Sadmin_DeviceAllocation : System.Web.UI.Page
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
                //Changes
                BindDeviceAllocation();
                BindDeviceName();
                GetCorporateOffice();
                //BindBoatHouseName();
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public void BindDeviceName()
    {
        try
        {
            ddlDeviceName.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("ddlDeviceNo").Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlDeviceName.DataSource = dt;
                            ddlDeviceName.DataValueField = "DeviceNo";
                            ddlDeviceName.DataTextField = "DeviceNo";
                            ddlDeviceName.DataBind();
                        }
                        else
                        {
                            ddlDeviceName.DataBind();
                        }

                        ddlDeviceName.Items.Insert(0, new ListItem("Select Device No", "0"));
                    }
                    else
                    {
                        ddlDeviceName.DataBind();
                        ddlDeviceName.Items.Insert(0, new ListItem("Select Device No", "0"));
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
    public void BindDeviceNameAll()
    {
        try
        {
            ddlDeviceName.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("ddlDeviceNoAll").Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlDeviceName.DataSource = dt;
                            ddlDeviceName.DataValueField = "DeviceNo";
                            ddlDeviceName.DataTextField = "DeviceNo";
                            ddlDeviceName.DataBind();
                        }
                        else
                        {
                            ddlDeviceName.DataBind();
                        }

                        ddlDeviceName.Items.Insert(0, new ListItem("Select Device No", "0"));
                    }
                    else
                    {
                        ddlDeviceName.DataBind();
                        ddlDeviceName.Items.Insert(0, new ListItem("Select Device No", "0"));
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
    /// Added BY Abhinaya K
    /// Date :17.07.2023
    /// </summary>
    public void GetCorporateOffice()
    {
        try
        {

            ddlCorpId.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlCorporateOffice",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlCorpId.DataSource = dtExists;
                        ddlCorpId.DataValueField = "CorpId";
                        ddlCorpId.DataTextField = "CorpName";
                        ddlCorpId.DataBind();

                    }
                    else
                    {
                        ddlCorpId.DataBind();
                    }
                    ddlCorpId.Items.Insert(0, "Select Corporate Office");
                    ddlCorpId.SelectedValue = dtExists.Rows[0]["CorpId"].ToString();
               
                    BindBoatHouseName();
                }
                else
                {
                    return;
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
    /// Added BY Abhinaya K
    /// Date :17.07.2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCorpId_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBoatHouseName();
    }

   
    public void BindBoatHouseName()
    {
        try
        {
            ddlBoatHouse.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("ddlBoatHouse?CorpId=" + ddlCorpId.SelectedValue+"").Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ddlBoatHouse.DataSource = dt;
                            ddlBoatHouse.DataValueField = "BoatHouseId";
                            ddlBoatHouse.DataTextField = "BoatHouseName";
                            ddlBoatHouse.DataBind();
                        }
                        else
                        {
                            ddlBoatHouse.DataBind();
                        }

                        ddlBoatHouse.Items.Insert(0, new ListItem("Select Boat House", "0"));
                    }
                    else
                    {
                        ddlBoatHouse.DataBind();
                        ddlBoatHouse.Items.Insert(0, new ListItem("Select Boat House", "0"));
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
                if (btnSubmit.Text == "Submit")
                {
                    var Device = new DeviceAllocation()
                    {
                        QueryType = "Insert",
                        UniqueId = "",
                        DeviceNo = ddlDeviceName.SelectedItem.Text.Trim(),
                        BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                        BoatHouseName = ddlBoatHouse.SelectedItem.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("DeviceAllocationINS", Device).Result;
                }
                else
                {
                    var Device = new DeviceAllocation()
                    {
                        QueryType = "Update",
                        UniqueId = ViewState["UniqueId"].ToString().Trim(),
                        DeviceNo = ddlDeviceName.SelectedItem.Text.Trim(),
                        BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                        BoatHouseName = ddlBoatHouse.SelectedItem.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("DeviceAllocationINS", Device).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClearInputs();
                        BindDeviceAllocation();
                        btnSubmit.Text = "Submit";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                    }
                    else
                    {
                        ClearInputs();
                        btnSubmit.Text = "Submit";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                    }
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label DeviceNo = (Label)gvrow.FindControl("lblDeviceNO");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label UniqueId = (Label)gvrow.FindControl("lblUniqueId");
            Label CorpId = (Label)gvrow.FindControl("lblCorpId");
            divAllServices.Visible = true;
            divGrid.Visible = true;
            BindDeviceNameAll();
            ddlDeviceName.SelectedValue = DeviceNo.Text.Trim();
            ddlDeviceName.Enabled = false;
            ddlCorpId.SelectedValue = CorpId.Text.Trim();
            BindBoatHouseName();
            ddlBoatHouse.SelectedValue = BoatHouseId.Text.Trim();
            ViewState["UniqueId"] = UniqueId.Text.Trim();
            btnSubmit.Text = "Update";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string ConsumptionId = gvDeviceAllocation.DataKeys[gvrow.RowIndex].Value.ToString();
            Label DeviceNo = (Label)gvrow.FindControl("lblDeviceNO");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                var btConsumption = new DeviceAllocation()
                {
                    QueryType = "Delete",
                    UniqueId = "",
                    DeviceNo = DeviceNo.Text.Trim(),
                    BoatHouseId = BoatHouseId.Text.Trim(),
                    BoatHouseName = BoatHouseName.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("DeviceAllocationINS", btConsumption).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindDeviceAllocation();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }
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

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string ConsumptionId = gvDeviceAllocation.DataKeys[gvrow.RowIndex].Value.ToString();
            Label DeviceNo = (Label)gvrow.FindControl("lblDeviceNO");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                var btConsumption = new DeviceAllocation()
                {
                    QueryType = "ReActive",
                    UniqueId = "",
                    DeviceNo = DeviceNo.Text.Trim(),
                    BoatHouseId = BoatHouseId.Text.Trim(),
                    BoatHouseName = BoatHouseName.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("DeviceAllocationINS", btConsumption).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindDeviceAllocation();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        
                    }
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
    public void BindDeviceAllocation()
    {
        try
        {
            divGrid.Visible = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "GetDeviceAllocationBasedOnCorpId",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvDeviceAllocation.DataSource = dtExists;
                        gvDeviceAllocation.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                    }
                    else
                    {
                        gvDeviceAllocation.DataBind();
                        lblGridMsg.Text = ResponseMsg.ToString();
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
    }
    protected void gvDeviceAllocation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // for example, the row contains a certain property
                if (((Label)e.Row.FindControl("lblActiveStatus")).Text == "D")
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnEdit");
                    btnEdit.Visible = false;
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = false;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = true;
                }

                else
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnEdit");
                    btnEdit.Visible = true;
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
    public void ClearInputs()
    {
        ddlDeviceName.Items.Clear();
        ddlBoatHouse.Items.Clear();
        BindDeviceName();
        GetCorporateOffice();
        ddlDeviceName.Enabled = true;
        ddlDeviceName.SelectedIndex = 0;
        ddlBoatHouse.SelectedIndex = 0;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ClearInputs();
        divAllServices.Visible = true;
        divGrid.Visible = true;
        btnSubmit.Text = "Submit";
    }
    public class DeviceAllocation
    {
        public string QueryType { get; set; }
        public string UniqueId { get; set; }
        public string DeviceNo { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
    }
    public class CommonClass
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string BoatHouseId { get; set; }
    }
    public class FA_CommonMethod
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }
}