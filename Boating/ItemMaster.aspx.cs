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

public partial class Department_Boating_ItemMaster : System.Web.UI.Page
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
                Session["EntityFlag"] = "B";
                Session["EntityId"] = Session["BoatHouseId"].ToString().Trim();
                Session["EntityName"] = Session["BoatHouseName"].ToString().Trim();
                Session["UserId"] = Session["UserId"].ToString().Trim();

                getItypeItem();
                getUOM();
                BindItemMaster();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public class ItemMaster
    {
        public string QueryType { get; set; }
        public string ItemId { get; set; }
        public string ItemDescription { get; set; }
        public string ItemType { get; set; }
        public string EntityFlag { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string UOM { get; set; }
        public string ItemRate { get; set; }
        public string OpeningQty { get; set; }
        public string CreatedBy { get; set; }
        public string UOMName { get; set; }
        public string ItemName { get; set; }

        public string ActiveStatus { get; set; }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        btnNew.Visible = false;
        clearInputs();
        btnSubmit.Text = "Submit";
        lblGridMsg.Text = " ";
        divGrid.Visible = false;
        divEntry.Visible = true;
    }

    public async void getItypeItem()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ddlItemType");

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
                            ddlItemType.DataSource = dt;
                            ddlItemType.DataValueField = "ConfigId";
                            ddlItemType.DataTextField = "ConfigName";
                            ddlItemType.DataBind();

                        }
                        else
                        {
                            ddlItemType.DataBind();
                        }
                    }
                    else
                    {

                        //  lblGridMsg.Text = ResponseMsg;

                    }
                    ddlItemType.Items.Insert(0, new ListItem("Select Item Type", "0"));
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

    public async void getUOM()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ddlUOM");

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
                            ddlUOM.DataSource = dt;
                            ddlUOM.DataValueField = "ConfigId";
                            ddlUOM.DataTextField = "ConfigName";
                            ddlUOM.DataBind();

                        }
                        else
                        {
                            ddlUOM.DataBind();
                        }
                    }
                    else
                    {

                        //  lblGridMsg.Text = ResponseMsg;

                    }
                    ddlUOM.Items.Insert(0, new ListItem("Select Unit Of Measure", "0"));
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

    protected async void btnSubmit_Click(object sender, EventArgs e)
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
                    var ItemMaster = new ItemMaster()
                    {
                        QueryType = "Insert",
                        ItemId = "0",
                        ItemType = ddlItemType.SelectedValue.Trim(),
                        UOM = ddlUOM.SelectedValue.Trim(),
                        ItemDescription = txtItemDescription.Text.Trim(),
                        EntityFlag = Session["EntityFlag"].ToString(),
                        EntityId = Session["EntityId"].ToString(),
                        EntityName = Session["EntityName"].ToString(),
                        ItemRate = txtItemRate.Text,
                        OpeningQty = txtOpeninQty.Text,
                        CreatedBy = Session["UserId"].ToString(),

                    };
                    response = await client.PostAsJsonAsync("ItemMaster", ItemMaster);

                }
                else
                {
                    var ItemMaster = new ItemMaster()
                    {
                        QueryType = "Update",
                        ItemId = txtItemId.Text.Trim(),
                        ItemType = ddlItemType.SelectedValue.Trim(),
                        UOM = ddlUOM.SelectedValue.Trim(),
                        ItemDescription = txtItemDescription.Text.Trim(),
                        EntityFlag = Session["EntityFlag"].ToString(),
                        EntityId = Session["EntityId"].ToString(),
                        EntityName = Session["EntityName"].ToString(),
                        ItemRate = txtItemRate.Text,
                        OpeningQty = txtOpeninQty.Text,
                        CreatedBy = Session["UserId"].ToString(),

                    };

                    response = await client.PostAsJsonAsync("ItemMaster", ItemMaster);

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
                            BindItemMaster();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {


                            BindItemMaster();
                            clearInputs();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    public async void BindItemMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ItemMaster = new ItemMaster()
                {
                    EntityId = Session["EntityId"].ToString()

                };
                HttpResponseMessage response = await client.PostAsJsonAsync("ItemMaster/BhId", ItemMaster);

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
                            gvItemMaster.DataSource = dt;
                            gvItemMaster.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            btnNew.Visible = true;
                        }
                        else
                        {
                            gvItemMaster.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divGrid.Visible = false;
                            divEntry.Visible = true;
                            btnNew.Visible = false;

                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg;
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        btnNew.Visible = false;
                    }
                }
                else
                {
                    divGrid.Visible = false;
                    divEntry.Visible = true;
                    btnNew.Visible = false;
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
        divGrid.Visible = true;
        divEntry.Visible = false;
        btnNew.Visible = true;
    }

    public void clearInputs()
    {
        ddlUOM.SelectedIndex = 0;
        ddlItemType.SelectedIndex = 0;
        txtOpeninQty.Text = string.Empty;
        txtItemRate.Text = string.Empty;
        txtItemDescription.Text = string.Empty;
        btnSubmit.Text = "Submit";

    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divGrid.Visible = false;
        divEntry.Visible = true;
        btnNew.Visible = false;
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string sTesfg = gvItemMaster.DataKeys[gvrow.RowIndex].Value.ToString();
        Label ItemId = (Label)gvrow.FindControl("lblItemId");
        Label ItemType = (Label)gvrow.FindControl("lblItemType");
        Label ItemDescription = (Label)gvrow.FindControl("lblItemDescription");
        Label UOM = (Label)gvrow.FindControl("lblUOM");
        Label ItemRate = (Label)gvrow.FindControl("lblItemRate");
        Label OpeningQty = (Label)gvrow.FindControl("lblOpeningQty");
        txtItemId.Text = ItemId.Text;
        ddlItemType.SelectedValue = ItemType.Text;
        ddlUOM.SelectedValue = UOM.Text;
        txtItemDescription.Text = ItemDescription.Text;
        txtItemRate.Text = ItemRate.Text;
        txtOpeninQty.Text = OpeningQty.Text;
        btnSubmit.Text = "Update";


    }

    protected async void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string ItemId = gvItemMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ItemType = (Label)gvrow.FindControl("lblItemType");
            Label ItemDescription = (Label)gvrow.FindControl("lblItemDescription");
            Label UOM = (Label)gvrow.FindControl("lblUOM");
            Label ItemRate = (Label)gvrow.FindControl("lblItemRate");
            Label OpeningQty = (Label)gvrow.FindControl("lblOpeningQty");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ItemMaster = new ItemMaster()
                {

                    QueryType = "Delete",
                    ItemId = ItemId.ToString().Trim(),
                    ItemType = ItemType.Text,
                    ItemDescription = ItemDescription.Text,
                    UOM = UOM.Text,
                    ItemRate = ItemRate.Text,
                    OpeningQty = OpeningQty.Text,
                    EntityFlag = Session["EntityFlag"].ToString(),
                    EntityId = Session["EntityId"].ToString(),
                    EntityName = Session["EntityName"].ToString(),
                    CreatedBy = Session["UserId"].ToString(),

                };

                HttpResponseMessage response;


                response = await client.PostAsJsonAsync("ItemMaster", ItemMaster);

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindItemMaster();
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

    protected void gvItemMaster_RowDataBound(Object sender, GridViewRowEventArgs e)
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

    protected async void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string ItemId = gvItemMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ItemType = (Label)gvrow.FindControl("lblItemType");
            Label ItemDescription = (Label)gvrow.FindControl("lblItemDescription");
            Label UOM = (Label)gvrow.FindControl("lblUOM");
            Label ItemRate = (Label)gvrow.FindControl("lblItemRate");
            Label OpeningQty = (Label)gvrow.FindControl("lblOpeningQty");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ItemMaster = new ItemMaster()
                {

                    QueryType = "ReActive",
                    ItemId = ItemId.ToString().Trim(),
                    ItemType = ItemType.Text,
                    ItemDescription = ItemDescription.Text,
                    UOM = UOM.Text,
                    ItemRate = ItemRate.Text,
                    OpeningQty = OpeningQty.Text,
                    EntityFlag = Session["EntityFlag"].ToString(),
                    EntityId = Session["EntityId"].ToString(),
                    EntityName = Session["EntityName"].ToString(),
                    CreatedBy = Session["UserId"].ToString(),

                };

                HttpResponseMessage response;


                response = await client.PostAsJsonAsync("ItemMaster", ItemMaster);


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindItemMaster();
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
}