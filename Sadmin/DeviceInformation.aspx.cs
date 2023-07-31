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

public partial class Sadmin_DeviceInformation : System.Web.UI.Page
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
                BindDeviceNumber();
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
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
                

                if (btnAdd.Text.Trim() == "Submit")
                {
                    QueryType = "Insert";
                    id = "0";
                }
                else
                {
                    QueryType = "Update";
                    id = hfUniqueId.Value.Trim();

                }


                var Device = new DeviceInformation()
                {
                    QueryType = QueryType.Trim(),
                    UniqueId = id.Trim(),
                    DeviceNo= txtDeviceNo.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("DeviceInformation/Insert", Device).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindDeviceNumber();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnAdd.Text = "Submit";
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
        txtDeviceNo.Text = string.Empty;


    }

    public void BindDeviceNumber()
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
                    QueryType = "GetDeviceInformation",
                    
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
                        gvDeviceNo.DataSource = dtExists;
                        gvDeviceNo.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                        gvDeviceNo.Visible = true;
                       
                       

                    }
                    else
                    {
                        gvDeviceNo.DataBind();
                        lblGridMsg.Text = "No Records Found";
                        divGrid.Visible = false;
                        gvDeviceNo.Visible = true;
                     
                    }
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
        string boatTypeId = gvDeviceNo.DataKeys[gvrow.RowIndex].Value.ToString();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var DeviceNo = new DeviceInformation()
            {
                QueryType = "ReActive",
                UniqueId = "",
                DeviceNo = gvDeviceNo.DataKeys[gvrow.RowIndex]["DeviceNo"].ToString().Trim(),
                ActiveStatus = gvDeviceNo.DataKeys[gvrow.RowIndex]["ActiveStatus"].ToString().Trim(),
                CreatedBy = Session["UserId"].ToString().Trim()

            };

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("DeviceInformation/Insert", DeviceNo).Result;

            if (response.IsSuccessStatusCode)
            {
                var submitresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                if (StatusCode == 1)
                {

                    BindDeviceNumber();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

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
                UniqueId = "",
                DeviceNo = gvDeviceNo.DataKeys[gvrow.RowIndex]["DeviceNo"].ToString().Trim(),
                ActiveStatus = gvDeviceNo.DataKeys[gvrow.RowIndex]["ActiveStatus"].ToString().Trim(),
                CreatedBy = Session["UserId"].ToString().Trim()

            };

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("DeviceInformation/Insert", DeviceNo).Result;

            if (response.IsSuccessStatusCode)
            {
                var submitresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                if (StatusCode == 1)
                {

                    BindDeviceNumber();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                }

            }
        }
    }

    protected void gvDeviceNo_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            
            btnAdd.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string id = gvDeviceNo.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
            hfUniqueId.Value = id.Trim();
          
           txtDeviceNo.Text = gvDeviceNo.DataKeys[gvrow.RowIndex]["DeviceNo"].ToString().Trim();
            ViewState["DeviceNo"] = txtDeviceNo.Text.Trim();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
        BindDeviceNumber();
    }

    public class DeviceInformation
    {
        public string QueryType { get; set; }
        public string UniqueId { get; set; }
        public string DeviceNo { get; set; }
        public string CreatedBy { get; set; }
        public string ActiveStatus { get; set; }
        public string BoatHouseId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string ServiceType { get; set; }
    }
    
}