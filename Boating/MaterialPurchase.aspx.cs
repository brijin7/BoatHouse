using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Department_Boating_MaterialPurchase : System.Web.UI.Page
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
                hfCreatedBy.Value = Session["UserId"].ToString().Trim();
                hfBoatHouseId.Value = Session["BoatHouseId"].ToString().Trim();
                hfBoatHouseName.Value = Session["BoatHouseName"].ToString().Trim();

                txtPurchaseDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtPurchaseDate.Attributes.Add("readonly", "readonly");

                GetPurchaseMaxNo();
                GetItem();
                BindMaterialPurchase();
                ddlItem.Enabled = true;
                ViewState["Update"] = "NotUpdate";
                ViewState["Edit"] = "Not Edit";

                AutomaticDeleteMaterial();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public class MaterialPurchase
    {
        public string QueryType { get; set; }
        public string PurchaseId { get; set; }
        public string ItemId { get; set; }

        public string EntityId { get; set; }
        public string PurchaseDate { get; set; }
        public string EntityName { get; set; }
        public string VendorRef { get; set; }
        public string ReceivedQty { get; set; }
        public string AcceptedQty { get; set; }
        public string RejectionReason { get; set; }
        public string RejectedQty { get; set; }
        public string PurchaseRate { get; set; }
        public string Createdby { get; set; }
        public string BoatHouseId { get; set; }
        public string NoOfItems { get; set; }
    }
    //Dropdown
    public void GetItem()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var MaterialPurchase1 = new MaterialPurchase()
                {
                    EntityId = hfBoatHouseId.Value.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("ddlItemMstr/BHId", MaterialPurchase1).Result;

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
                            ddlItem.DataSource = dt;
                            ddlItem.DataValueField = "ItemId";
                            ddlItem.DataTextField = "ItemDescription";
                            ddlItem.DataBind();
                        }
                        else
                        {

                            ddlItem.DataBind();
                        }

                        ddlItem.Items.Insert(0, new ListItem("Select Item", "0"));
                    }
                    else
                    {
                        //lblGridMsg.Text = ResponseMsg.ToString().Trim();
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

    //Get Method(Bind the Records)
    public void GetPurchaseMaxNo()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var MaterialPurchase1 = new MaterialPurchase()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("MaxPurchaseNo", MaterialPurchase1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();
                    Session["MaxPurchaseId"] = ResponseMsg.ToString();

                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindMaterialPurchaseAdd()
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
                if (ViewState["Update"].ToString() != "Update")
                {
                    var MaterialPurchase1 = new MaterialPurchase()
                    {
                        PurchaseId = Session["MaxPurchaseId"].ToString(),
                        BoatHouseId = hfBoatHouseId.Value.Trim()
                    };
                    response = client.PostAsJsonAsync("GetMaterialPurchaseDetails", MaterialPurchase1).Result;
                    ViewState["Update"] = "Not Update";
                }
                else
                {
                    var MaterialPurchase1 = new MaterialPurchase()
                    {
                        PurchaseId = Session["Purchaseee"].ToString(),
                        BoatHouseId = hfBoatHouseId.Value.Trim()

                    };
                    response = client.PostAsJsonAsync("GetMaterialPurchaseDetails", MaterialPurchase1).Result;
                }


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
                            gvAddGrid.DataSource = dt;
                            gvAddGrid.DataBind();
                            divEntry.Visible = true;
                            divgrid.Visible = false;
                            divAddGrid.Visible = true;
                            lbtnNew.Visible = false;
                            btnSubmit.Visible = true;
                            btnAdd.Visible = true;
                            btnCancel.Visible = true;
                            if (ViewState["Update"].ToString() == "Update")
                            {
                                btnSubmit.Visible = false;
                                btnAdd.Visible = true;
                                btnCancel.Visible = true;
                            }

                            if (ViewState["Edit"].ToString() == "Edit")
                            {
                                btnSubmit.Visible = false;
                                btnAdd.Visible = true;
                                btnCancel.Visible = true;
                            }

                        }
                        else
                        {

                            gvAddGrid.DataBind();
                            divEntry.Visible = true;
                            divgrid.Visible = false;
                            divAddGrid.Visible = false;
                            lbtnNew.Visible = false;
                            btnSubmit.Visible = false;
                            btnAdd.Visible = true;
                            btnCancel.Visible = true;
                            Clear();
                        }
                    }
                    else
                    {
                        divEntry.Visible = true;
                        divgrid.Visible = false;
                        divAddGrid.Visible = false;
                        lbtnNew.Visible = false;
                        btnSubmit.Visible = false;
                        btnAdd.Visible = true;
                        btnCancel.Visible = true;
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

    public void BindMaterialPurchase()
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
                var MaterialPurchase1 = new MaterialPurchase()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                response = client.PostAsJsonAsync("AllMaterialPurchase", MaterialPurchase1).Result;
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
                            gvMaterialPurchase.DataSource = dt;
                            gvMaterialPurchase.DataBind();
                            gvMaterialPurchase.Visible = true;
                            divgrid.Visible = true;
                            divAddGrid.Visible = false;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            //lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            divgrid.Visible = false;
                            divAddGrid.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;

                            //lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        //lblGridMsg.Text = ResponseMsg.ToString().Trim();

                        divgrid.Visible = false;
                        divAddGrid.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
                        btnSubmit.Visible = false;
                        btnCancel.Visible = false;

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

    public void MaterialDetailsBasedOnPurchaseId()
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
                var MaterialPurchase1 = new MaterialPurchase()
                {
                    PurchaseId = hfBoatHouseId.Value.Trim(),
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                response = client.PostAsJsonAsync("MaterialDetailsBasedOnPurchaseId", MaterialPurchase1).Result;
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
                            gvMaterialDetails.DataSource = dt;
                            gvMaterialDetails.DataBind();

                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        //lblGridMsg.Text = ResponseMsg.ToString().Trim();

                        return;

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

    public void AutomaticDeleteMaterial()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var MaterialPurchase1 = new MaterialPurchase()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("AutomaticDeleteMaterial", MaterialPurchase1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Clear Inputs

    public void Clear()
    {
        txtPurchaseId.Text = string.Empty;
        ddlItem.SelectedIndex = 0;
        txtPurchaseDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtReceivedQu.Text = string.Empty;
        txtAcceptedQu.Text = string.Empty;
        txtRejectedQu.Text = string.Empty;
        txtRejectedReson.Text = string.Empty;
        txtVendorRemark.Text = string.Empty;
        txtPurchaseRate.Text = string.Empty;
        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        ViewState["Update"] = "Not Update";
        ViewState["Edit"] = "Not Edit";
    }

    public void ClearSomeInputs()
    {
        txtPurchaseId.Text = string.Empty;
        ddlItem.SelectedIndex = 0;

        txtReceivedQu.Text = string.Empty;
        txtAcceptedQu.Text = string.Empty;
        txtRejectedQu.Text = string.Empty;
        txtRejectedReson.Text = string.Empty;

        txtPurchaseRate.Text = string.Empty;
        divgrid.Visible = false;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        divAddGrid.Visible = true;
        ddlItem.Enabled = true;
        btnAdd.Text = "Add";
        //ViewState["Update"] = "Update";
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divgrid.Visible = false;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        txtRejectedQu.ReadOnly = true;
        ViewState["Update"] = "Not Update";
        btnSubmit.Visible = false;
        btnAdd.Visible = true;
        btnCancel.Visible = false;
        GetPurchaseMaxNo();
        ViewState["Edit"] = "Not Edit";
        txtPurchaseDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        btnAdd.Text = "Add";
        ddlItem.Enabled = true;
    }

    //Submit
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
                    var MaterialPurchase1 = new MaterialPurchase()
                    {
                        QueryType = "Insert",
                        PurchaseId = Session["MaxPurchaseId"].ToString(),
                        ItemId = "0",
                        EntityId = hfBoatHouseId.Value.Trim(),
                        EntityName = hfBoatHouseName.Value.Trim(),
                        PurchaseDate = txtPurchaseDate.Text.Trim(),
                        VendorRef = txtVendorRemark.Text.Trim(),
                        ReceivedQty = "0",
                        AcceptedQty = "0",
                        RejectionReason = "0",
                        RejectedQty = "0",
                        PurchaseRate = "0",
                        Createdby = hfCreatedBy.Value.Trim()
                    };

                    response = client.PostAsJsonAsync("MaterialPurchaseDetail", MaterialPurchase1).Result;

                }
                else
                {
                    var MaterialPurchase1 = new MaterialPurchase()
                    {
                        QueryType = "Update",
                        PurchaseId = txtPurchaseId.Text,
                        ItemId = ddlItem.SelectedValue.Trim(),
                        EntityId = hfBoatHouseId.Value.Trim(),
                        EntityName = hfBoatHouseName.Value.Trim(),
                        PurchaseDate = txtPurchaseDate.Text.Trim(),
                        VendorRef = txtVendorRemark.Text.Trim(),
                        ReceivedQty = txtReceivedQu.Text,
                        AcceptedQty = txtAcceptedQu.Text,
                        RejectionReason = txtRejectedReson.Text,
                        RejectedQty = txtRejectedQu.Text,
                        PurchaseRate = txtPurchaseRate.Text,
                        Createdby = hfCreatedBy.Value.Trim()
                    };

                    response = client.PostAsJsonAsync("MaterialPurchaseDetail", MaterialPurchase1).Result;

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        Clear();
                        BindMaterialPurchase();
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

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {

        try
        {

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label PurchaseId = (Label)gvrow.FindControl("lblPurchaseId");
            ViewState["Edit"] = "Edit";

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var MaterialPurchase1 = new MaterialPurchase()
                {
                    PurchaseId = PurchaseId.Text.Trim(),
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                response = client.PostAsJsonAsync("MaterilEditDetails", MaterialPurchase1).Result;
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
                            ViewState["Update"] = "Update";
                            txtPurchaseId.Text = dt.Rows[0]["PurchaseId"].ToString();
                            Session["PId"] = txtPurchaseId.Text.Trim();

                            Session["Purchaseee"] = txtPurchaseId.Text.Trim();
                            txtPurchaseDate.Text = dt.Rows[0]["PurchaseDate"].ToString();

                            txtVendorRemark.Text = dt.Rows[0]["VendorRef"].ToString();

                            response = client.PostAsJsonAsync("GetMaterialPurchaseDetails", MaterialPurchase1).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse1 = response.Content.ReadAsStringAsync().Result;
                                int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse1)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();
                                if (StatusCode1 == 1)
                                {
                                    DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);
                                    if (dt1.Rows.Count > 0)
                                    {
                                        gvAddGrid.DataSource = dt1;
                                        gvAddGrid.DataBind();
                                        divEntry.Visible = true;
                                        divgrid.Visible = false;
                                        divAddGrid.Visible = true;
                                        lbtnNew.Visible = false;

                                        btnCancel.Visible = true;
                                        btnSubmit.Visible = false;
                                        //divFinalSubmit.Visible = true;

                                    }
                                    else
                                    {


                                        divEntry.Visible = true;
                                        divgrid.Visible = false;
                                        divAddGrid.Visible = false;
                                        lbtnNew.Visible = false;

                                        btnCancel.Visible = false;
                                        btnSubmit.Visible = false;

                                    }
                                }
                                else
                                {


                                    divEntry.Visible = true;
                                    divgrid.Visible = false;
                                    lbtnNew.Visible = false;
                                    divAddGrid.Visible = false;
                                }

                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            //lblGridMsg.Text = ResponseMsg.ToString().Trim();

                            return;

                        }
                    }
                    else
                    {
                        var Errorresponse = response.Content.ReadAsStringAsync().Result;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }




    }

    protected void ImgBtnDelete_Click1(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string PurChaseId = gvMaterialPurchase.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                var MaterialPurchase1 = new MaterialPurchase()
                {
                    QueryType = "Delete",
                    PurchaseId = PurChaseId.ToString().Trim(),
                    BoatHouseId = hfBoatHouseId.Value.Trim()

                };

                response = client.PostAsJsonAsync("ActiveInActiveMaterial", MaterialPurchase1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindMaterialPurchase();
                        Clear();
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
            string PurChaseId = gvMaterialPurchase.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                var MaterialPurchase1 = new MaterialPurchase()
                {
                    QueryType = "ReActive",
                    PurchaseId = PurChaseId.ToString().Trim(),
                    BoatHouseId = hfBoatHouseId.Value.Trim()

                };

                response = client.PostAsJsonAsync("ActiveInActiveMaterial", MaterialPurchase1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindMaterialPurchase();
                        Clear();
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtPurchaseDate.Text = string.Empty;
        txtVendorRemark.Text = string.Empty;
        ViewState["Update"] = "Not Update";
        ViewState["Edit"] = "Not Edit";
        AutomaticDeleteMaterial();
        Clear();
        BindMaterialPurchase();
    }

    //Text Changed
    protected void txtAcceptedQu_TextChanged(object sender, EventArgs e)
    {
        decimal Accpected;
        decimal Recevied;
        decimal RejectedQu;

        if (txtReceivedQu.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Received Quantity');", true);
            txtAcceptedQu.Text = "";
            txtRejectedReson.Text = "";
            txtReceivedQu.Focus();
            return;
        }

        Accpected = Convert.ToDecimal(txtAcceptedQu.Text);
        Recevied = Convert.ToDecimal(txtReceivedQu.Text);
        RejectedQu = (Recevied) - (Accpected);

        bool v = Accpected <= Recevied;

        if (v)
        {
            txtRejectedQu.Text = RejectedQu.ToString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Accepted Quantity Should be less than OR Equal to Received Quantity');", true);
            txtAcceptedQu.Text = "";
            txtRejectedQu.Text = "";
            txtAcceptedQu.Focus();
        }

    }

    protected void txtReceivedQu_TextChanged(object sender, EventArgs e)
    {
        txtAcceptedQu.Text = "";
        txtRejectedQu.Text = "";
        txtAcceptedQu.Focus();
    }

    protected void gvMaterialPurchase_RowDataBound(Object sender, GridViewRowEventArgs e)
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

    //Add Button 

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtRejectedQu.Text != "0" && txtRejectedQu.Text != "0.000")
            {
                if (txtRejectedReson.Text == null || txtRejectedReson.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('You Must Enter Rejected Reason');", true);
                    return;
                }

            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string PurchaseId = string.Empty;
                if (ViewState["Edit"].ToString() == "Edit")
                {
                    PurchaseId = Session["PId"].ToString();
                }
                else
                {
                    PurchaseId = Session["MaxPurchaseId"].ToString();
                }

                HttpResponseMessage response;


                if (btnAdd.Text == "Add")
                {
                    var MaterialPurchase1 = new MaterialPurchase()
                    {
                        QueryType = "Add",

                        PurchaseId = PurchaseId.Trim(),
                        ItemId = ddlItem.SelectedValue.Trim(),
                        EntityId = hfBoatHouseId.Value.Trim(),
                        EntityName = hfBoatHouseName.Value.Trim(),
                        PurchaseDate = txtPurchaseDate.Text.Trim(),
                        VendorRef = txtVendorRemark.Text.Trim(),
                        ReceivedQty = txtReceivedQu.Text,
                        AcceptedQty = txtAcceptedQu.Text,
                        RejectionReason = txtRejectedReson.Text,
                        RejectedQty = txtRejectedQu.Text,
                        PurchaseRate = txtPurchaseRate.Text,
                        Createdby = hfCreatedBy.Value.Trim()
                    };
                    response = client.PostAsJsonAsync("MaterialPurchaseDetail", MaterialPurchase1).Result;

                }
                else
                {
                    if (ViewState["Update"].ToString() == "Update")
                    {
                        var MaterialPurchase1 = new MaterialPurchase()
                        {
                            QueryType = "Update",
                            PurchaseId = Session["Purchaseee"].ToString(),
                            ItemId = ddlItem.SelectedValue.Trim(),
                            EntityId = hfBoatHouseId.Value.Trim(),
                            EntityName = hfBoatHouseName.Value.Trim(),
                            PurchaseDate = txtPurchaseDate.Text.Trim(),
                            VendorRef = txtVendorRemark.Text.Trim(),
                            ReceivedQty = txtReceivedQu.Text,
                            AcceptedQty = txtAcceptedQu.Text,
                            RejectionReason = txtRejectedReson.Text,
                            RejectedQty = txtRejectedQu.Text,
                            PurchaseRate = txtPurchaseRate.Text,
                            Createdby = hfCreatedBy.Value.Trim()
                        };
                        response = client.PostAsJsonAsync("MaterialPurchaseDetail", MaterialPurchase1).Result;


                    }


                    else
                    {

                        var MaterialPurchase1 = new MaterialPurchase()
                        {
                            QueryType = "AddUpdate",
                            PurchaseId = Session["MaxPurchaseId"].ToString(),
                            ItemId = ddlItem.SelectedValue.Trim(),
                            EntityId = hfBoatHouseId.Value.Trim(),
                            EntityName = hfBoatHouseName.Value.Trim(),
                            PurchaseDate = txtPurchaseDate.Text.Trim(),
                            VendorRef = txtVendorRemark.Text.Trim(),
                            ReceivedQty = txtReceivedQu.Text,
                            AcceptedQty = txtAcceptedQu.Text,
                            RejectionReason = txtRejectedReson.Text,
                            RejectedQty = txtRejectedQu.Text,
                            PurchaseRate = txtPurchaseRate.Text,
                            Createdby = hfCreatedBy.Value.Trim()

                        };
                        response = client.PostAsJsonAsync("MaterialPurchaseDetail", MaterialPurchase1).Result;

                    }

                }


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        ClearSomeInputs();
                        BindMaterialPurchaseAdd();

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

    protected void ImgBtnAddEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divAddGrid.Visible = true;
            divgrid.Visible = false;
            lbtnNew.Visible = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string PurchaseId = gvAddGrid.DataKeys[gvrow.RowIndex].Value.ToString();
            Session["PurChase"] = PurchaseId.ToString();
            Label ItemId = (Label)gvrow.FindControl("lblItemId");
            Label ReceivedQuantity = (Label)gvrow.FindControl("lblIReceivedQty");
            Label AcceptedQuantity = (Label)gvrow.FindControl("lblQuantity");
            Label RejectedQuantity = (Label)gvrow.FindControl("lblRejectedQty");
            Label RejectReason = (Label)gvrow.FindControl("lblRejectionReason");
            Label PurchaseRate = (Label)gvrow.FindControl("lblPurchaseRate");

            GetItem();
            ddlItem.SelectedValue = ItemId.Text.Trim();
            ddlItem.Enabled = false;
            txtReceivedQu.Text = ReceivedQuantity.Text;
            txtAcceptedQu.Text = AcceptedQuantity.Text;
            txtRejectedQu.Text = RejectedQuantity.Text;
            txtRejectedReson.Text = RejectReason.Text;
            txtPurchaseRate.Text = PurchaseRate.Text;





            btnAdd.Text = "Update";
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void ImgBtnAddDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            divEntry.Visible = true;
            divAddGrid.Visible = true;
            divgrid.Visible = false;
            lbtnNew.Visible = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string PurchaseId = gvAddGrid.DataKeys[gvrow.RowIndex].Value.ToString();
            Session["PurChase"] = PurchaseId.ToString();
            Label ItemId = (Label)gvrow.FindControl("lblItemId");
            Label ReceivedQuantity = (Label)gvrow.FindControl("lblIReceivedQty");
            Label AcceptedQuantity = (Label)gvrow.FindControl("lblQuantity");
            Label RejectedQuantity = (Label)gvrow.FindControl("lblRejectedQty");
            Label RejectReason = (Label)gvrow.FindControl("lblRejectionReason");
            Label PurchaseRate = (Label)gvrow.FindControl("lblPurchaseRate");



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var MaterialPurchase1 = new MaterialPurchase()
                {
                    QueryType = "AddDelete",
                    PurchaseId = PurchaseId.ToString(),
                    ItemId = ItemId.Text.Trim(),
                    EntityId = hfBoatHouseId.Value.Trim(),
                    EntityName = hfBoatHouseName.Value.Trim(),
                    PurchaseDate = txtPurchaseDate.Text.Trim(),
                    VendorRef = txtVendorRemark.Text.Trim(),
                    ReceivedQty = ReceivedQuantity.Text,
                    AcceptedQty = AcceptedQuantity.Text,
                    RejectionReason = RejectReason.Text,
                    RejectedQty = RejectedQuantity.Text,
                    PurchaseRate = PurchaseRate.Text,
                    Createdby = hfCreatedBy.Value.Trim()
                };
                HttpResponseMessage response;
                response = client.PostAsJsonAsync("MaterialPurchaseDetail", MaterialPurchase1).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ClearSomeInputs();
                        BindMaterialPurchaseAdd();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
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

    protected void btnAddCancel_Click(object sender, EventArgs e)
    {
        ClearSomeInputs();
        txtPurchaseDate.Text = string.Empty;
        txtVendorRemark.Text = string.Empty;
        ViewState["Update"] = "Not Update";
        ViewState["Edit"] = "Not Edit";

    }

    protected void lbtnNoOfItems_Click(object sender, EventArgs e)
    {
        try
        {
            MpeMaterial.Show();

            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label PurchaseId = (Label)gvrow.FindControl("lblPurchaseId");

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                var MaterialPurchase1 = new MaterialPurchase()
                {
                    PurchaseId = PurchaseId.Text.Trim(),
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                response = client.PostAsJsonAsync("MaterialDetailsBasedOnPurchaseId", MaterialPurchase1).Result;

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
                            gvMaterialDetails.Visible = true;
                            gvMaterialDetails.DataSource = dt;
                            gvMaterialDetails.DataBind();
                            //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
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

    protected void gvAddGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ViewState["Edit"].ToString() == "Edit")
        {
            ((DataControlField)gvAddGrid.Columns
            .Cast<DataControlField>()
            .Where(fld => fld.HeaderText == "Delete")
            .SingleOrDefault()).Visible = false;

        }
        else
        {

            ((DataControlField)gvAddGrid.Columns
            .Cast<DataControlField>()
            .Where(fld => fld.HeaderText == "Delete")
            .SingleOrDefault()).Visible = true;
        }
    }

    protected void imgCloseTicket_Click(object sender, ImageClickEventArgs e)
    {
        MpeMaterial.Hide();
    }
}