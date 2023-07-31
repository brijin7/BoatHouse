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

public partial class OtherServices : System.Web.UI.Page
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
                BindOtherServices();
                BindTaxDetails();

                txtChargePerItem.Attributes.Add("readonly", "readonly");
                txtServiceTax.Attributes.Add("readonly", "readonly");
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCategoryName.Items.Clear();

        if (ddlServiceType.SelectedIndex == 0)
        {
            return;
        }

        if (ddlServiceType.SelectedValue == "OS")
        {
            BindCategory();
        }
        else
        {
            BindBoatTicketCategory();
        }
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;

        divEntry.Visible = true;
        lbtnNew.Visible = false;
        clearInputs();
        btnSubmit.Text = "Submit";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string sTaxId = string.Empty;

            if (ddlTaxId.SelectedItem.Text == "Nil Tax")
            {
                sTaxId = "0";
            }
            else
            {
                sTaxId = ddlTaxId.SelectedValue.Trim();
            }

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
                    var otherservices = new otherservices()
                    {
                        QueryType = "Insert",
                        ServiceId = "0",
                        Category = ddlCategoryName.SelectedValue.Trim(),
                        ServiceName = txtServiceName.Text.Trim(),
                        ShortName = txtShortName.Text.Trim(),

                        ServiceTotalAmount = txtTotalAmt.Text.Trim(),
                        ChargePerItem = txtChargePerItem.Text.Trim(),
                        ChargePerItemTax = txtServiceTax.Text.Trim(),

                        TaxID = sTaxId.Trim(),
                        TaxName = ddlTaxId.SelectedItem.Text,
                        ServiceType = ddlServiceType.SelectedValue,
                        CreatedBy = Session["UserId"].ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    };
                    response = client.PostAsJsonAsync("OtherServices", otherservices).Result;
                    sMSG = "OtherServices Details Inserted Successfully";
                }
                else
                {
                    var otherservices = new otherservices()
                    {
                        QueryType = "Update",
                        Category = ddlCategoryName.SelectedValue.Trim(),
                        ServiceId = txtserviceId.Text.Trim(),
                        ServiceName = txtServiceName.Text.Trim(),
                        ShortName = txtShortName.Text.Trim(),

                        ServiceTotalAmount = txtTotalAmt.Text.Trim(),
                        ChargePerItem = txtChargePerItem.Text.Trim(),
                        ChargePerItemTax = txtServiceTax.Text.Trim(),

                        TaxID = sTaxId.Trim(),
                        TaxName = ddlTaxId.SelectedItem.Text,
                        ServiceType = ddlServiceType.SelectedValue,
                        CreatedBy = Session["UserId"].ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    };
                    response = client.PostAsJsonAsync("OtherServices", otherservices).Result;
                    sMSG = "OtherServices Details Updated Successfully";
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
                            clearInputs();
                            BindOtherServices();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            clearInputs();
                            BindOtherServices();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Other Services Details Already Exists.');", true);
                    }
                }
                else
                {
                    // lblGridMsg.Text = response.ToString();
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
        clearInputs();
        btnSubmit.Text = "Submit";
        BindOtherServices();
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divGrid.Visible = false;
            divEntry.Visible = true;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            ddlCategoryName.Enabled = false;
            ddlServiceType.Enabled = false;

            ddlServiceType.SelectedValue = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceType"].ToString().Trim();

            if (ddlServiceType.SelectedValue == "OS")
            {
                BindCategory();
            }
            else
            {
                BindBoatTicketCategory();
            }

            ddlCategoryName.SelectedValue = gvOtherServices.DataKeys[gvrow.RowIndex]["Category"].ToString().Trim();
            txtserviceId.Text = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceId"].ToString().Trim();
            txtServiceName.Text = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceName"].ToString().Trim();
            txtShortName.Text = gvOtherServices.DataKeys[gvrow.RowIndex]["ShortName"].ToString().Trim();

            txtTotalAmt.Text = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceTotalAmount"].ToString().Trim();
            txtChargePerItem.Text = gvOtherServices.DataKeys[gvrow.RowIndex]["ChargePerItem"].ToString().Trim();
            txtServiceTax.Text = gvOtherServices.DataKeys[gvrow.RowIndex]["ChargePerItemTax"].ToString().Trim();

            string TaxId = gvOtherServices.DataKeys[gvrow.RowIndex]["TaxID"].ToString().Trim();
            hfTaxSlab.Value = gvOtherServices.DataKeys[gvrow.RowIndex]["TaxName"].ToString().Trim();

            BindTaxDetails();

            if (TaxId == "0")
            {
                ddlTaxId.SelectedIndex = 1;

                hfservicetax.Value = ddlTaxId.SelectedItem.Text;
                hfserviceCount.Value = "0";
            }
            else
            {
                ddlTaxId.SelectedValue = gvOtherServices.DataKeys[gvrow.RowIndex]["TaxID"].ToString().Trim();

                decimal Tax = 0;
                string[] taxlist = hfTaxSlab.Value.Split(',');

                foreach (var list in taxlist)
                {
                    var TaxName = list;
                    var tx = list.Split('-');

                    Tax += Convert.ToDecimal(tx[1].ToString());
                }

                hfservicetax.Value = Tax.ToString();
                hfserviceCount.Value = taxlist.Length.ToString();
            }

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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var otherservices = new otherservices()
                {
                    QueryType = "Delete",

                    Category = gvOtherServices.DataKeys[gvrow.RowIndex]["Category"].ToString().Trim(),
                    ServiceId = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceId"].ToString().Trim(),
                    ServiceName = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceName"].ToString().Trim(),
                    ShortName = gvOtherServices.DataKeys[gvrow.RowIndex]["ShortName"].ToString().Trim(),

                    ServiceTotalAmount = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceTotalAmount"].ToString().Trim(),
                    ChargePerItem = gvOtherServices.DataKeys[gvrow.RowIndex]["ChargePerItem"].ToString().Trim(),
                    ChargePerItemTax = gvOtherServices.DataKeys[gvrow.RowIndex]["ChargePerItemTax"].ToString().Trim(),
                    ServiceType = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceType"].ToString().Trim(),
                    TaxID = gvOtherServices.DataKeys[gvrow.RowIndex]["TaxID"].ToString().Trim(),

                    CreatedBy = Session["UserId"].ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("OtherServices", otherservices).Result;
                sMSG = "BoatType Master Details Deleted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindOtherServices();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvOtherServices_RowDataBound(Object sender, GridViewRowEventArgs e)
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
                var otherservices = new otherservices()
                {
                    QueryType = "ReActive",
                    Category = gvOtherServices.DataKeys[gvrow.RowIndex]["Category"].ToString().Trim(),
                    ServiceId = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceId"].ToString().Trim(),
                    ServiceName = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceName"].ToString().Trim(),
                    ShortName = gvOtherServices.DataKeys[gvrow.RowIndex]["ShortName"].ToString().Trim(),

                    ServiceTotalAmount = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceTotalAmount"].ToString().Trim(),
                    ChargePerItem = gvOtherServices.DataKeys[gvrow.RowIndex]["ChargePerItem"].ToString().Trim(),
                    ChargePerItemTax = gvOtherServices.DataKeys[gvrow.RowIndex]["ChargePerItemTax"].ToString().Trim(),
                    ServiceType = gvOtherServices.DataKeys[gvrow.RowIndex]["ServiceType"].ToString().Trim(),
                    TaxID = gvOtherServices.DataKeys[gvrow.RowIndex]["TaxID"].ToString().Trim(),

                    CreatedBy = Session["UserId"].ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("OtherServices", otherservices).Result;
                sMSG = "BoatType Master Details Deleted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindOtherServices();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindCategory()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/ddlGroupName").Result;

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
                            ddlCategoryName.DataSource = dt;
                            ddlCategoryName.DataValueField = "ConfigId";
                            ddlCategoryName.DataTextField = "ConfigName";
                            ddlCategoryName.DataBind();

                        }
                        else
                        {
                            ddlCategoryName.DataBind();
                        }
                    }
                    else
                    {
                        //  lblGridMsg.Text = ResponseMsg;
                    }

                    ddlCategoryName.Items.Insert(0, "Select Category");
                }
                else
                {

                    //  lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindBoatTicketCategory()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ddlAddBoatTicketGroupName").Result;

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
                            ddlCategoryName.DataSource = dt;
                            ddlCategoryName.DataValueField = "ConfigId";
                            ddlCategoryName.DataTextField = "ConfigName";
                            ddlCategoryName.DataBind();

                        }
                        else
                        {
                            ddlCategoryName.DataBind();
                        }
                    }
                    else
                    {
                        //  lblGridMsg.Text = ResponseMsg;
                    }

                    ddlCategoryName.Items.Insert(0, "Select Category");
                }
                else
                {

                    //  lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindTaxDetails()
    {
        try
        {
            ddlTaxId.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var TaxService = new otherservices()
                {
                    ServiceId = "2",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ddlTaxMasterList", TaxService).Result;

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
                            ddlTaxId.DataSource = dt;
                            ddlTaxId.DataValueField = "TaxId";
                            ddlTaxId.DataTextField = "TaxName";
                            ddlTaxId.DataBind();
                        }
                        else
                        {
                            ddlTaxId.DataBind();
                        }
                    }
                    else
                    {
                    }

                    ddlTaxId.Items.Insert(0, "Select Tax");
                    ddlTaxId.Items.Insert(1, "Nil Tax");
                }
                else
                {

                    //  lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void clearInputs()
    {
        ddlServiceType.Enabled = true;
        ddlCategoryName.Enabled = true;
        ddlCategoryName.SelectedIndex = -1;
        ddlServiceType.SelectedIndex = -1;
        txtServiceName.Text = "";
        txtShortName.Text = "";

        txtTotalAmt.Text = "0";
        txtChargePerItem.Text = "0";
        txtServiceTax.Text = "0";

        ddlTaxId.SelectedIndex = 0;
        btnSubmit.Text = "Submit";

        hfservicetax.Value = "";
        hfserviceCount.Value = "";
    }

    public void BindOtherServices()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatHouseId = new otherservices()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("OtherServicesMstr/BHId", BoatHouseId).Result;

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
                            gvOtherServices.DataSource = dt;
                            gvOtherServices.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            gvOtherServices.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString();
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public class otherservices
    {
        public string Category { get; set; }
        public string CategoryName { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ShortName { get; set; }
        public string ServiceTotalAmount { get; set; }
        public string ChargePerItem { get; set; }
        public string ChargePerItemTax
        {
            get;
            set;
        }
        public string TaxID { get; set; }
        public string TaxName { get; set; }
        public string CreatedBy { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string QueryType { get; set; }
        public string ActiveStatus { get; set; }
        public string ServiceType { get; set; }
    }
    public void GetBoatChargeAndTaxAfterDiscount(decimal BoatCharge, decimal RowerCharge, decimal TaxPer, decimal sTimes)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://paypre.in/node/gst/myapp/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("gst/" + BoatCharge + "/" + RowerCharge + "/" + TaxPer + "/" + sTimes + "").Result;

                if (response.IsSuccessStatusCode)
                {
                    var ResponseMsg = response.Content.ReadAsStringAsync().Result;
                    string[] BCharge = ResponseMsg.Split(':');

                    string[] bamt = BCharge[1].ToString().Split(',');
                    string[] bgst = BCharge[2].ToString().Split('}');

                    ViewState["PerchargeTax"] = bamt[0].ToString();
                    ViewState["ServiceTax"] = bgst[0].ToString();
                    txtChargePerItem.Text = ViewState["PerchargeTax"].ToString().Trim();
                    txtServiceTax.Text = ViewState["ServiceTax"].ToString().Trim();
                }
                else
                {
                    ViewState["PerchargeTax"] = 0;
                    ViewState["ServiceTax"] = 0;
                    txtChargePerItem.Text = ViewState["PerchargeTax"].ToString().Trim();
                    txtServiceTax.Text = ViewState["ServiceTax"].ToString().Trim();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    protected void txtTotalAmt_TextChanged(object sender, EventArgs e)
    {
        string taxid = ddlTaxId.SelectedValue;
        if (ddlTaxId.SelectedValue == "Nil Tax")
        {
            txtChargePerItem.Text = txtTotalAmt.Text;
            txtServiceTax.Text = Convert.ToString("0").Trim();
        }
        else
        {
            decimal Rowercharge = 0;

            // decimal SerTax =Convert.ToDecimal( hfservicetax.Value.Trim());
            decimal SerTax = Convert.ToDecimal(hfservicetax.Value.Trim());
            decimal PerChargeTax = (SerTax / 2);
            decimal Times = 2;
            decimal TotalAmnt = Convert.ToDecimal(txtTotalAmt.Text.Trim());

            if ((txtTotalAmt.Text == "0"))
            {
                txtTotalAmt.Text = Convert.ToString("0").Trim();
            }
            var strarray = hfTaxSlab.Value.Trim();
            if (strarray == "Select Tax")
            {
                txtTotalAmt.Text = Convert.ToString("0").Trim();
                txtChargePerItem.Text = Convert.ToString("0").Trim();
                txtServiceTax.Text = Convert.ToString("0").Trim();
            }
            if (strarray != "Nil Tax")
            {
                GetBoatChargeAndTaxAfterDiscount(TotalAmnt, Rowercharge, PerChargeTax, Times);
            }
        }

    }
}