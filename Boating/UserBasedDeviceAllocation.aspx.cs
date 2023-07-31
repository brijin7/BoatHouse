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

public partial class Boating_UserBasedDeviceAllocation : System.Web.UI.Page
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
                GetDeviceNo();
                GetUserName();
                BindUBDetail();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public void GetDeviceNo()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var DeviceNo = new DeviceInformation()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("GetDeviceNo", DeviceNo).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        ddlDeviceName.DataSource = dtExists;
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
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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
                var UserName = new DeviceInformation()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("DeviceNo/ddlUserName", UserName).Result;
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string id = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                string QueryType = string.Empty;


                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var Device = new DeviceInformation()
                    {
                        QueryType = "Insert",
                        UniqueId = "",
                        UserId = ddlUserName.SelectedValue.Trim(),
                        UserName = ddlUserName.SelectedItem.Text.Trim(),
                        DeviceNo = ddlDeviceName.SelectedValue.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("UBDeviceAllocation/Insert", Device).Result;
                }
                else
                {
                    var Device = new DeviceInformation()
                    {
                        QueryType = "Update",
                        UniqueId = hfUniqueId.Value.Trim(),
                        UserId = ddlUserName.SelectedValue.Trim(),
                        UserName = ddlUserName.SelectedItem.Text.Trim(),
                        DeviceNo = ViewState["DeviceNo"].ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("UBDeviceAllocation/Insert", Device).Result;

                }




                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindUBDetail();
                        ClearInputs();
                        btnSubmit.Text = "Submit";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ClearInputs();
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
    public void ClearInputs()
    {
        ddlDeviceName.Items.Clear();
        GetDeviceNo();
        ddlUserName.SelectedIndex = 0;
        ddlDeviceName.SelectedIndex = 0;
        ddlDeviceName.Enabled = true;
    }
    public void BindUBDetail()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new DeviceInformation()
                {
                    QueryType = "GetUBDeviceAllocation",

                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;

                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        gvUBDeviceNo.DataSource = dtExists;
                        gvUBDeviceNo.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                        gvUBDeviceNo.Visible = true;



                    }
                    else
                    {
                        gvUBDeviceNo.DataBind();
                        lblGridMsg.Text = "No Records Found";
                        divGrid.Visible = false;
                        gvUBDeviceNo.Visible = true;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void gvUBDeviceNo_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        string id = string.Empty;
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string boatTypeId = gvUBDeviceNo.DataKeys[gvrow.RowIndex].Value.ToString();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var DeviceNo = new DeviceInformation()
            {
                QueryType = "ReActive",
                UniqueId = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim(),
                UserId = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim(),
                UserName = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim(),
                DeviceNo = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["DeviceNo"].ToString().Trim(),
                BoatHouseId = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["BoatHouseId"].ToString().Trim(),
                BoatHouseName = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["BoatHouseName"].ToString().Trim(),
                ActiveStatus = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["ActiveStatus"].ToString().Trim(),
                CreatedBy = Session["UserId"].ToString().Trim()

            };

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("UBDeviceAllocation/Insert", DeviceNo).Result;

            if (response.IsSuccessStatusCode)
            {
                var submitresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                if (StatusCode == 1)
                {

                    BindUBDetail();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                }
                else
                {
                    BindUBDetail();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                }

            }
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {

        string id = string.Empty;
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;


        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var DeviceNo = new DeviceInformation()
            {
                QueryType = "Delete",
                UniqueId = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim(),
                UserId = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim(),
                UserName = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim(),
                DeviceNo = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["DeviceNo"].ToString().Trim(),
                BoatHouseId = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["BoatHouseId"].ToString().Trim(),
                BoatHouseName = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["BoatHouseName"].ToString().Trim(),
                ActiveStatus = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["ActiveStatus"].ToString().Trim(),
                CreatedBy = Session["UserId"].ToString().Trim()

            };

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("UBDeviceAllocation/Insert", DeviceNo).Result;

            if (response.IsSuccessStatusCode)
            {
                var submitresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                if (StatusCode == 1)
                {

                    BindUBDetail();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                }
                else
                {
                    BindUBDetail();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                }
            }
        }
    }
    public void GetDeviceNoAll()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var DeviceNo = new DeviceInformation()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("GetDeviceNoAll", DeviceNo).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        ddlDeviceName.DataSource = dtExists;
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
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string id = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
            hfUniqueId.Value = id.Trim();
            GetDeviceNoAll();
            ddlDeviceName.SelectedValue = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["DeviceNo"].ToString().Trim();

            ddlUserName.SelectedValue = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();

            ddlUserName.SelectedItem.Text = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim();
            ViewState["DeviceNo"] = gvUBDeviceNo.DataKeys[gvrow.RowIndex]["DeviceNo"].ToString().Trim();
            ddlDeviceName.Enabled = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }



    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
        BindUBDetail();
    }
    public class DeviceInformation
    {
        public string QueryType { get; set; }
        public string UniqueId { get; set; }
        public string DeviceNo { get; set; }
        public string CreatedBy { get; set; }
        public string ActiveStatus { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string ServiceType { get; set; }
    }






}