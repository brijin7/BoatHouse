using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sadmin_GatewayDetails : System.Web.UI.Page
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
                if (Session["UserRole"].ToString().Trim() == "Sadmin")
                {
                    divShowDetails.Visible = true;

                    BindGatewayDetails();
                }
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
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

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var Gateway = new GatewayDetails()
                    {
                        QueryType = "Insert",
                        UniqueId = "0",

                        GatewayName = txtGatewayName.Text.Trim(),
                        MerchantId = txtMerchantId.Text.Trim(),
                        AccessCode = txtAccessCode.Text.Trim(),
                        WorkingKey = txtWorkingKey.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("GatewayNameMaster", Gateway).Result;
                }
                else
                {
                    var Gateway = new GatewayDetails()
                    {
                        QueryType = "Update",
                        UniqueId = ViewState["UniqueId"].ToString().Trim(),

                        GatewayName = txtGatewayName.Text.Trim(),
                        MerchantId = txtMerchantId.Text.Trim(),
                        AccessCode = txtAccessCode.Text.Trim(),
                        WorkingKey = txtWorkingKey.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("GatewayNameMaster", Gateway).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            ClearInputs();
                            BindGatewayDetails();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                        }
                        else
                        {
                            ClearInputs();
                            BindGatewayDetails();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                        }
                    }
                    else
                    {
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();      
    }

    protected void gvGateway_RowDataBound(Object sender, GridViewRowEventArgs e)
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
            btnSubmit.Text = "Update";

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            ViewState["UniqueId"] = gvGateway.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
            txtGatewayName.Text = gvGateway.DataKeys[gvrow.RowIndex]["GatewayName"].ToString().Trim();
            txtMerchantId.Text = gvGateway.DataKeys[gvrow.RowIndex]["MerchantId"].ToString().Trim();
            txtAccessCode.Text = gvGateway.DataKeys[gvrow.RowIndex]["AccessCode"].ToString().Trim();
            txtWorkingKey.Text = gvGateway.DataKeys[gvrow.RowIndex]["WorkingKey"].ToString().Trim();
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Gateway = new GatewayDetails()
                {
                    QueryType = "Delete",

                    UniqueId = gvGateway.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim(),
                    GatewayName = gvGateway.DataKeys[gvrow.RowIndex]["GatewayName"].ToString().Trim(),
                    MerchantId = gvGateway.DataKeys[gvrow.RowIndex]["MerchantId"].ToString().Trim(),
                    AccessCode = gvGateway.DataKeys[gvrow.RowIndex]["AccessCode"].ToString().Trim(),
                    WorkingKey = gvGateway.DataKeys[gvrow.RowIndex]["WorkingKey"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString()
                };

                HttpResponseMessage response;


                response = client.PostAsJsonAsync("GatewayNameMaster", Gateway).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindGatewayDetails();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Gateway = new GatewayDetails()
                {
                    QueryType = "ReActive",

                    UniqueId = gvGateway.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim(),
                    GatewayName = gvGateway.DataKeys[gvrow.RowIndex]["GatewayName"].ToString().Trim(),
                    MerchantId = gvGateway.DataKeys[gvrow.RowIndex]["MerchantId"].ToString().Trim(),
                    AccessCode = gvGateway.DataKeys[gvrow.RowIndex]["AccessCode"].ToString().Trim(),
                    WorkingKey = gvGateway.DataKeys[gvrow.RowIndex]["WorkingKey"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("GatewayNameMaster", Gateway).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindGatewayDetails();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindGatewayDetails()
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
                    QueryType = "ShowGateway",
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
                        gvGateway.DataSource = dtExists;
                        gvGateway.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                    }
                    else
                    {
                        gvGateway.DataBind();
                        lblGridMsg.Text = ResponseMsg.ToString();
                        divGrid.Visible = true;
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

    public void ClearInputs()
    {
        txtGatewayName.Text = "";
        txtMerchantId.Text = "";
        txtAccessCode.Text = "";
        txtWorkingKey.Text = "";
        btnSubmit.Text = "Submit";
    }

    public class CommonClass
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
    }

    public class GatewayDetails
    {
        public string QueryType { get; set; }
        public string UniqueId { get; set; }
        public string GatewayName { get; set; }
        public string MerchantId { get; set; }
        public string AccessCode { get; set; }
        public string WorkingKey { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
    }
}