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

public partial class BoatMaterialConsumption : System.Web.UI.Page
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

                txtConsumptionDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtConsumptionId.Attributes.Add("readonly", "readonly");
                txtConsumptionDate.Attributes.Add("readonly", "readonly");

                GetBoatType();
                GetBoatSeater();
                GetItemDetails();

                DeleteBoatConsumptionItem();
                BindBoatConsumption();
                BindMaxBoatConsumptionId();

                ddlItem.Enabled = true;
                ViewState["Update"] = "Not Update";
                ViewState["Edit"] = "Not Edit";
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        btnSubmit.Visible = false;
        btnCancel.Visible = false;
        divgrid.Visible = false;
        lbtnNew.Visible = false;

        txtConsumptionDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        BindMaxBoatConsumptionId();

        btnAdd.Text = "Add";
        ViewState["Edit"] = "Not Edit";
        ViewState["Update"] = "Not Update";
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
                    var btConsumption = new boatConsumption()
                    {
                        QueryType = "Insert",
                        ConsumptionId = ViewState["MaxConsumpId"].ToString(),
                        ConsumptionDate = txtConsumptionDate.Text.Trim(),
                        BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                        BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                        BoatHouseId = hfBoatHouseId.Value.Trim(),
                        BoatHouseName = hfBoatHouseName.Value.Trim(),
                        ItemId = "0",
                        ItemQtyPerTrip = "0",
                        Createdby = hfCreatedBy.Value.Trim()
                    };
                    response = client.PostAsJsonAsync("MstrBoatConsumption", btConsumption).Result;
                }
                else
                {
                    var btConsumption = new boatConsumption()
                    {
                        QueryType = "Update",
                        ConsumptionId = txtConsumptionId.Text.Trim(),
                        ConsumptionDate = txtConsumptionDate.Text.Trim(),
                        BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                        BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                        BoatHouseId = hfBoatHouseId.Value.Trim(),
                        BoatHouseName = hfBoatHouseName.Value.Trim(),
                        ItemId = ddlItem.SelectedValue.Trim(),
                        ItemQtyPerTrip = txtItemQty.Text.Trim(),
                        Createdby = hfCreatedBy.Value.Trim()
                    };
                    response = client.PostAsJsonAsync("MstrBoatConsumption", btConsumption).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        Clear();
                        BindBoatConsumption();
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
            Label ConsumptionId = (Label)gvrow.FindControl("lblConsumptionId");
            ViewState["Edit"] = "Edit";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var btConsumption = new boatConsumption()
                {
                    ConsumptionId = ConsumptionId.Text.Trim(),
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                response = client.PostAsJsonAsync("BoatConsumpEditDet", btConsumption).Result;

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
                            txtConsumptionId.Text = dt.Rows[0]["ConsumptionId"].ToString();
                            ViewState["ConsumpId"] = txtConsumptionId.Text.Trim();
                            txtConsumptionDate.Text = dt.Rows[0]["ConsumptionDate"].ToString();
                            GetBoatType();
                            ddlBoatType.SelectedValue = dt.Rows[0]["BoatTypeId"].ToString();
                            GetBoatSeater();
                            ddlBoatSeater.SelectedValue = dt.Rows[0]["BoatSeaterId"].ToString();

                            response = client.PostAsJsonAsync("GetBoatConsumpDetails", btConsumption).Result;
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

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string ConsumptionId = gvBoatConsumptionDetails.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                var btConsumption = new boatConsumption()
                {
                    QueryType = "Delete",
                    ConsumptionId = ConsumptionId.ToString().Trim(),
                    ConsumptionDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BoatTypeId = "0",
                    BoatSeaterId = "0",
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    BoatHouseName = hfBoatHouseName.Value.Trim(),
                    ItemId = "0",
                    ItemQtyPerTrip = "0",
                    Createdby = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("MstrBoatConsumption", btConsumption).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        Clear();
                        BindBoatConsumption();
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
            string ConsumptionId = gvBoatConsumptionDetails.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                var btConsumption = new boatConsumption()
                {
                    QueryType = "ReActive",
                    ConsumptionId = ConsumptionId.ToString().Trim(),
                    ConsumptionDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BoatTypeId = "0",
                    BoatSeaterId = "0",
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    BoatHouseName = hfBoatHouseName.Value.Trim(),
                    ItemId = "0",
                    ItemQtyPerTrip = "0",
                    Createdby = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("MstrBoatConsumption", btConsumption).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        Clear();
                        BindBoatConsumption();
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
        Clear();
        DeleteBoatConsumptionItem();
        BindBoatConsumption();
    }

    //Add Button 
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string ConsumpId = string.Empty;
                if (ViewState["Edit"].ToString() == "Edit")
                {
                    ConsumpId = ViewState["ConsumpId"].ToString();
                }
                else
                {
                    ConsumpId = ViewState["MaxConsumpId"].ToString();
                }

                HttpResponseMessage response;

                if (btnAdd.Text == "Add")
                {
                    var btConsumption = new boatConsumption()
                    {
                        QueryType = "Add",
                        ConsumptionId = ConsumpId.Trim(),
                        ConsumptionDate = txtConsumptionDate.Text.Trim(),
                        BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                        BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                        BoatHouseId = hfBoatHouseId.Value.Trim(),
                        BoatHouseName = hfBoatHouseName.Value.Trim(),
                        ItemId = ddlItem.SelectedValue.Trim(),
                        ItemQtyPerTrip = txtItemQty.Text.Trim(),
                        Createdby = hfCreatedBy.Value.Trim()
                    };
                    response = client.PostAsJsonAsync("MstrBoatConsumption", btConsumption).Result;
                }
                else
                {
                    if (ViewState["Update"].ToString() == "Update")
                    {
                        var btConsumption = new boatConsumption()
                        {
                            QueryType = "Update",
                            ConsumptionId = ViewState["ConsumpId"].ToString().Trim(),
                            ConsumptionDate = txtConsumptionDate.Text.Trim(),
                            BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                            BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                            BoatHouseId = hfBoatHouseId.Value.Trim(),
                            BoatHouseName = hfBoatHouseName.Value.Trim(),
                            ItemId = ddlItem.SelectedValue.Trim(),
                            ItemQtyPerTrip = txtItemQty.Text.Trim(),
                            Createdby = hfCreatedBy.Value.Trim()
                        };
                        response = client.PostAsJsonAsync("MstrBoatConsumption", btConsumption).Result;
                    }
                    else
                    {
                        var btConsumption = new boatConsumption()
                        {
                            QueryType = "AddUpdate",
                            ConsumptionId = ConsumpId.Trim(),
                            ConsumptionDate = txtConsumptionDate.Text.Trim(),
                            BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                            BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                            BoatHouseId = hfBoatHouseId.Value.Trim(),
                            BoatHouseName = hfBoatHouseName.Value.Trim(),
                            ItemId = ddlItem.SelectedValue.Trim(),
                            ItemQtyPerTrip = txtItemQty.Text.Trim(),
                            Createdby = hfCreatedBy.Value.Trim()
                        };
                        response = client.PostAsJsonAsync("MstrBoatConsumption", btConsumption).Result;
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ClearSomeInputs();
                        BindBoatConsumpAdd();
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
            string ConsumptionId = gvAddGrid.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ItemId = (Label)gvrow.FindControl("lblItemId");
            Label ItemQty = (Label)gvrow.FindControl("lblItemQtyPerTrip");

            GetItemDetails();
            ddlItem.SelectedValue = ItemId.Text.Trim();
            ddlItem.Enabled = false;
            txtItemQty.Text = ItemQty.Text.Trim();
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
            string ConsumptionId = gvAddGrid.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ItemId = (Label)gvrow.FindControl("lblItemId");
            Label ItemQty = (Label)gvrow.FindControl("lblItemQtyPerTrip");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var btConsumption = new boatConsumption()
                {
                    QueryType = "AddDelete",
                    ConsumptionId = ConsumptionId.Trim(),
                    ConsumptionDate = txtConsumptionDate.Text.Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    BoatHouseName = hfBoatHouseName.Value.Trim(),
                    ItemId = ItemId.Text.Trim(),
                    ItemQtyPerTrip = ItemQty.Text.Trim(),
                    Createdby = hfCreatedBy.Value.Trim()
                };
                response = client.PostAsJsonAsync("MstrBoatConsumption", btConsumption).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ClearSomeInputs();
                        BindBoatConsumpAdd();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        ClearSomeInputs();
                        BindBoatConsumpAdd();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    ClearSomeInputs();
                    BindBoatConsumpAdd();
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


    }

    protected void lbtnNoOfItems_Click(object sender, EventArgs e)
    {
        try
        {
            MpeMaterial.Show();
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label ConsumptionId = (Label)gvrow.FindControl("lblConsumptionId");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                var btConsumption = new boatConsumption()
                {
                    ConsumptionId = ConsumptionId.Text.Trim(),
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                response = client.PostAsJsonAsync("BoatConsumpDet/ConsId", btConsumption).Result;

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
                            gvConsumptionItemDetails.Visible = true;
                            gvConsumptionItemDetails.DataSource = dt;
                            gvConsumptionItemDetails.DataBind();
                            lblGvMsgPopup.Text = "";
                        }
                        else
                        {
                            gvConsumptionItemDetails.DataBind();
                            return;
                        }
                    }
                    else
                    {
                        MpeMaterial.Hide();
                        gvConsumptionItemDetails.DataBind();
                        lblGvMsgPopup.Text = "No Records Found..!";
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

    protected void gvBoatConsumptionDetails_RowDataBound(object sender, GridViewRowEventArgs e)
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

    // ***** User Defined Functions ***** //

    public class boatConsumption
    {
        public string QueryType { get; set; }
        public string ConsumptionId { get; set; }
        public string ConsumptionDate { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string Createdby { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string BoatType { get; set; }
        public string SeaterType { get; set; }
        public string ItemId { get; set; }
        public string ItemDescription { get; set; }
        public string ItemQtyPerTrip { get; set; }
        public string EntityId { get; set; }
    }

    public void GetBoatSeater()
    {
        try
        {
            ddlBoatSeater.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var boatMaterialConsumption = new boatConsumption()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlBoatSeater", boatMaterialConsumption).Result;

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
                            ddlBoatSeater.DataSource = dt;
                            ddlBoatSeater.DataValueField = "BoatSeaterId";
                            ddlBoatSeater.DataTextField = "SeaterType";
                            ddlBoatSeater.DataBind();
                        }
                        else
                        {
                            ddlBoatSeater.DataBind();
                        }

                        ddlBoatSeater.Items.Insert(0, new ListItem("Select Boat Seater", "0"));
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

                var boatMaterialConsumption = new boatConsumption()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlBoatType", boatMaterialConsumption).Result;

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

                        ddlBoatType.Items.Insert(0, new ListItem("Select Boat Type", "0"));
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

    public void GetItemDetails()
    {
        try
        {
            ddlItem.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var boatMaterialConsumption = new boatConsumption()
                {
                    EntityId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("ddlItemMstr/BHId", boatMaterialConsumption).Result;

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

    public void BindMaxBoatConsumptionId()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var boatMaterialConsumption = new boatConsumption()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("MaxBoatConsumptionId", boatMaterialConsumption).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                    txtConsumptionId.Text = ResponseMsg.ToString().Trim();
                    ViewState["MaxConsumpId"] = txtConsumptionId.Text.Trim();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindBoatConsumpAdd()
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
                    var btConsumption = new boatConsumption()
                    {
                        ConsumptionId = ViewState["MaxConsumpId"].ToString(),
                        BoatHouseId = hfBoatHouseId.Value.Trim()
                    };
                    response = client.PostAsJsonAsync("GetBoatConsumpDetails", btConsumption).Result;
                    ViewState["Update"] = "Not Update";
                }
                else
                {
                    var btConsumption = new boatConsumption()
                    {
                        ConsumptionId = ViewState["ConsumpId"].ToString(),
                        BoatHouseId = hfBoatHouseId.Value.Trim()

                    };
                    response = client.PostAsJsonAsync("GetBoatConsumpDetails", btConsumption).Result;
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
                            divAddGrid.Visible = true;
                            divgrid.Visible = false;
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
                            btnCancel.Visible = false;
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

    public void BindBoatConsumption()
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
                var btConsumption = new boatConsumption()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                response = client.PostAsJsonAsync("BoatConsumptionAll/BhId", btConsumption).Result;
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
                            gvBoatConsumptionDetails.DataSource = dt;
                            gvBoatConsumptionDetails.DataBind();
                            divgrid.Visible = true;
                            divAddGrid.Visible = false;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            divgrid.Visible = false;
                            divAddGrid.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
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

    public void DeleteBoatConsumptionItem()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var MaterialPurchase1 = new boatConsumption()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("DeleteBoatConsumptionItem", MaterialPurchase1).Result;

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

    public void ClearSomeInputs()
    {
        ddlItem.SelectedIndex = 0;
        txtItemQty.Text = string.Empty;

        divgrid.Visible = false;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        divAddGrid.Visible = true;
        ddlItem.Enabled = true;

        btnAdd.Text = "Add";
    }

    public void Clear()
    {
        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;

        txtConsumptionId.Text = string.Empty;
        ddlItem.SelectedIndex = 0;
        ddlItem.Enabled = true;
        txtConsumptionDate.Text = string.Empty;
        ddlBoatType.SelectedIndex = 0;
        ddlBoatSeater.SelectedIndex = 0;
        txtItemQty.Text = string.Empty;

        ViewState["Update"] = "Not Update";
        ViewState["Edit"] = "Not Edit";
    }
}