using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Helpers;

public partial class Boating_PaymentUPIDetails : System.Web.UI.Page
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
                //if (!Page.IsPostBack)

                BindPayAccDetails();
                BindPayUPIDetails();
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
                string sMSG = string.Empty;
                string QType = string.Empty;
                string Id = string.Empty;

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QType = "Insert";
                    Id = "0";

                }
                else
                {
                    QType = "Update";
                    Id = hfUniqueId.Value.Trim();
                }

                var Boatseatmaster = new PaymentUPIDetails()
                {
                    QueryType = QType.Trim(),
                    UniqueId = Convert.ToInt32(Id.Trim()),
                    Name = txtName.Text.Trim(),
                    MobileNo = txtMobileNo.Text.Trim(),
                    UPIId = txtUPIId.Text.Trim(),
                    MerchantCode = txtMerchantCode.Text.Trim(),
                    MerchantId = txtMerchantId.Text.Trim(),
                    EntityId = Convert.ToInt32(Session["BoatHouseId"].ToString().Trim()),
                    EntityName = Session["BoatHouseName"].ToString().Trim(),
                    EntityType = "Boating",
                    CreatedBy = Session["UserId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("PaymentUPIDetails", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            BindPayUPIDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else if (btnSubmit.Text.Trim() == "Update")
                        {
                            BindPayUPIDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {
                            BindPayUPIDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    BindPayUPIDetails();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnSubmit.Text = "Update";

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sId = gvPayUPIDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = sId.Trim();
            Label Name = (Label)gvrow.FindControl("lblName");
            Label Mobile = (Label)gvrow.FindControl("lblMobileNo");
            Label UPI = (Label)gvrow.FindControl("lblUPIId");
            Label MerchantCode = (Label)gvrow.FindControl("lblMerchantCode");
            Label MerchantId = (Label)gvrow.FindControl("lblMerchantId");

            txtName.Text = Name.Text.Trim();
            txtMobileNo.Text = Mobile.Text.Trim();
            txtUPIId.Text = UPI.Text.Trim();
            txtMerchantCode.Text = MerchantCode.Text.Trim();
            txtMerchantId.Text = MerchantId.Text.Trim();
            txtMobileNo.Enabled = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvPayUPIDetails_RowDataBound(Object sender, GridViewRowEventArgs e)
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

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sId = gvPayUPIDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            Label Name = (Label)gvrow.FindControl("lblName");
            Label Mobile = (Label)gvrow.FindControl("lblMobileNo");
            Label UPI = (Label)gvrow.FindControl("lblUPIId");
            Label MerchantCode = (Label)gvrow.FindControl("lblMerchantCode");
            Label MerchantId = (Label)gvrow.FindControl("lblMerchantId");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Boatseatmaster = new PaymentUPIDetails()
                {
                    QueryType = "Delete",
                    UniqueId = Convert.ToInt32(sId.Trim()),
                    Name = Name.Text.Trim(),
                    MobileNo = Mobile.Text.Trim(),
                    UPIId = UPI.Text.Trim(),
                    MerchantCode = MerchantCode.Text.Trim(),
                    MerchantId = MerchantId.Text.Trim(),
                    EntityId = Convert.ToInt32(Session["BoatHouseId"].ToString().Trim()),
                    EntityName = Session["BoatHouseName"].ToString().Trim(),
                    EntityType = "Boating",
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("PaymentUPIDetails", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindPayUPIDetails();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
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
            string sId = gvPayUPIDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            Label Name = (Label)gvrow.FindControl("lblName");
            Label Mobile = (Label)gvrow.FindControl("lblMobileNo");
            Label UPI = (Label)gvrow.FindControl("lblUPIId");
            Label MerchantCode = (Label)gvrow.FindControl("lblMerchantCode");
            Label MerchantId = (Label)gvrow.FindControl("lblMerchantId");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Boatseatmaster = new PaymentUPIDetails()
                {
                    QueryType = "ReActive",
                    UniqueId = Convert.ToInt32(sId.Trim()),
                    Name = Name.Text.Trim(),
                    MobileNo = Mobile.Text.Trim(),
                    UPIId = UPI.Text.Trim(),
                    MerchantCode = MerchantCode.Text.Trim(),
                    MerchantId = MerchantId.Text.Trim(),
                    EntityId = Convert.ToInt32(Session["BoatHouseId"].ToString().Trim()),
                    EntityName = Session["BoatHouseName"].ToString().Trim(),
                    EntityType = "Boating",
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("PaymentUPIDetails", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindPayUPIDetails();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindPayUPIDetails()
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
                    QueryType = "PaymentUPIDetails",
                    ServiceType = "Boating",
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
                        gvPayUPIDetails.DataSource = dtExists;
                        gvPayUPIDetails.DataBind();

                        divGrid.Visible = true;
                    }
                    else
                    {
                        gvPayUPIDetails.DataBind();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindPayAccDetails()
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
                    QueryType = "PaymentAccountDetails",
                    ServiceType = "Boating",
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
                        lblBankName.Text = dtExists.Rows[0]["BankName"].ToString();
                        lblBankIFSCode.Text = dtExists.Rows[0]["BankIFSCCode"].ToString();
                        lblAccNo.Text = dtExists.Rows[0]["AccountNo"].ToString();
                    }
                    else
                    {
                        lblBankName.Text = "";
                        lblBankIFSCode.Text = "";
                        lblAccNo.Text = "";
                    }

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
        txtName.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtUPIId.Text = string.Empty;
        txtMerchantCode.Text = string.Empty;
        txtMerchantId.Text = string.Empty;
        btnSubmit.Text = "Submit";
        txtMobileNo.Enabled = true;

    }

    public class PaymentUPIDetails
    {
        public string QueryType { get; set; }
        public int UniqueId { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string UPIId { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantId { get; set; }

    }

    public class CommonClass
    {
        public string BoatHouseId
        {
            get;
            set;
        }
        public string QueryType
        {
            get;
            set;
        }
        public string BoatTypeId
        {
            get;
            set;
        }
        public string BoatSeaterId
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
        public string Input1
        {
            get;
            set;
        }
        public string Input2
        {
            get;
            set;
        }
        public string Input3
        {
            get;
            set;
        }
        public string Input4
        {
            get;
            set;
        }
        public string Input5
        {
            get;
            set;
        }

        public string ServiceType
        {
            get;
            set;
        }
    }
}