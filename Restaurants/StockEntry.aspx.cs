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


public partial class Restaurants_StockEntry : System.Web.UI.Page
{
	public string GetAntiForgeryToken()
	{
		string cookieToken, formToken;
		AntiForgery.GetTokens(null, out cookieToken, out formToken);
		ViewState["__AntiForgeryCookie"] = cookieToken;
		return formToken;

	}

	public object Covert { get; private set; }

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

				//CHANGES
				txtFromDates.Text = DateTime.Now.ToString("dd/MM/yyyy");
				txtToDates.Text = DateTime.Now.ToString("dd/MM/yyyy");
				txtFromDates.Attributes.Add("readonly", "readonly");
				txtToDates.Attributes.Add("readonly", "readonly");
				BindCategory();
				ViewState["hfstartvalue"] = "0";
				ViewState["hfendvalue"] = "0";
				int istart;
				int iend;
				AddProcess(0, 10, out istart, out iend);
				ViewState["Flag"] = "B";
				BindStockEntry();
				getUOM();
				BindCategoryName();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
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

                var cat = new StockEntry()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("FoodCategory/BHId", cat).Result;

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
                            ddlItemCategory.DataSource = dt;
                            ddlItemCategory.DataValueField = "CategoryId";
                            ddlItemCategory.DataTextField = "CategoryName";
                            ddlItemCategory.DataBind();

                        }
                        else
                        {
                            ddlItemCategory.DataBind();
                        }
                    }
                    else
                    {

                        //  lblGridMsg.Text = ResponseMsg;

                    }

                    ddlItemCategory.Items.Insert(0, new ListItem("Select Category", "0"));
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

    public void BindItemName()
    {

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var cat = new StockEntry()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    CategoryId = ddlItemCategory.SelectedValue.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("FoodItemName/BHId", cat).Result;

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
                            ddlItemName.DataSource = dt;
                            ddlItemName.DataValueField = "ServiceId";
                            ddlItemName.DataTextField = "ServiceName";
                            ddlItemName.DataBind();
                            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));

                        }
                        else
                        {
                            ddlItemName.DataBind();
                        }
                    }
                    else
                    {
                        ddlItemName.Items.Clear();
                        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
                        //  lblGridMsg.Text = ResponseMsg;

                    }
                    // ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
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
    public void getUOM()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ddlUOM").Result;

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
    protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemCategory.SelectedValue != "0")
        {
            BindItemName();
        }
        else
        {
            ddlItemName.Items.Clear();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
    }

    public void BindStockEntry()
    {
       
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatHouseId = new StockEntry()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = txtFromDates.Text.Trim(),
                    ToDate = txtToDates.Text.Trim(),
                    CountStart = ViewState["hfstartvalue"].ToString(),
                    ItemCategoryId = ddlCatName.SelectedValue.Trim(),
                    ItemNameId=ddlCatItem.SelectedValue.Trim(),

                };
                HttpResponseMessage response = client.PostAsJsonAsync("StockEntryDetails/BHIdV2", BoatHouseId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                       
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count < 10)
                        {
                            Next.Enabled = false;
                        }
                        else
                        {
                            Next.Enabled = true;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            gvStockEntry.DataSource = dt;
                            gvStockEntry.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            fromTodate.Visible = true;
                            PrevNext.Visible = true;
                        }
                        else
                        {
                            gvStockEntry.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            fromTodate.Visible = true;

                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString();
                        divGrid.Visible = false;
                        divEntry.Visible = false;
                        divlabel.Visible = true;
                        back.Enabled = false;
                        Next.Enabled = false;
                        lbtnNew.Visible = true;
                        fromTodate.Visible = true;
                        PrevNext.Visible = true;
                        if (ViewState["Flag"].ToString() == "R")
                        {
                            lblGridMsg.Text = ResponseMsg.ToString();
                            divGrid.Visible = false;
                            divEntry.Visible = false;
                            divlabel.Visible = true;
                            back.Enabled = false;
                            Next.Enabled = false; 
                            lbtnNew.Visible = true;
                            fromTodate.Visible = true;
                            PrevNext.Visible = true;

                        }

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
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var otherservices = new StockEntry()
                    {
                        QueryType = "Insert",
                        StockId = "0",
                        ItemCategoryId = ddlItemCategory.SelectedValue.Trim(),
                        ItemCategory = ddlItemCategory.SelectedItem.Text.Trim(),
                        ItemNameId = ddlItemName.SelectedValue.Trim(),
                        ItemName = ddlItemName.SelectedItem.Text.Trim(),
                        Date = txtFromDate.Text.Trim(),
                        UOM = ddlUOM.SelectedValue.Trim(),
                        Quantity = txtOpeninQty.Text.Trim(),
                        Reference = txtRef.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    };
                    response = client.PostAsJsonAsync("RestaurantStockEntry", otherservices).Result;
                    sMSG = "Stock Entry Details Inserted Successfully";
                }
                else
                {
                    var otherservices = new StockEntry()
                    {
                        QueryType = "Update",
                        StockId = ViewState["StockId"].ToString(),
                        ItemCategoryId = ddlItemCategory.SelectedValue.Trim(),
                        ItemCategory = ddlItemCategory.SelectedItem.Text.Trim(),
                        ItemNameId = ddlItemName.SelectedValue.Trim(),
                        ItemName = ddlItemName.SelectedItem.Text.Trim(),
                        Date = txtFromDate.Text.Trim(),
                        UOM = ddlUOM.SelectedValue.Trim(),
                        Quantity = txtOpeninQty.Text.Trim(),
                        Reference = txtRef.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    };
                    response = client.PostAsJsonAsync("RestaurantStockEntry", otherservices).Result;
                    sMSG = "Stock Entry Details Updated Successfully";
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
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            fromTodate.Visible = true;
                            ViewState["hfstartvalue"] = "0";
                            ViewState["hfendvalue"] = "0";
                            int istart;
                            int iend;
                            AddProcess(0, 10, out istart, out iend);
                            BindStockEntry();


                        }
                        else
                        {
                            clearInputs();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            ViewState["hfstartvalue"] = "0";
                            ViewState["hfendvalue"] = "0";
                            int istart;
                            int iend;
                            AddProcess(0, 10, out istart, out iend);
                            BindStockEntry();


                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Stock Details Already Exists !');", true);
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

    public void clearInputs()
    {
        ddlItemCategory.Enabled = true;
        ddlItemName.Enabled = true;
        txtFromDate.Enabled = true;
        ddlUOM.Enabled = true;
        txtRef.Enabled = true;
        ddlItemCategory.SelectedIndex = 0;
        ddlItemName.SelectedIndex = 0;

        ddlUOM.SelectedIndex = 0;
        txtFromDate.Text = "";
        txtOpeninQty.Text = "0";
        txtRef.Text = "0";
        btnSubmit.Text = "Submit";
    }

    protected void gvStockEntry_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divGrid.Visible = false;
            divEntry.Visible = true;
            back.Enabled = false;
            Next.Enabled = false;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            fromTodate.Visible = false;
            PrevNext.Visible = false;

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            string stockId = gvStockEntry.DataKeys[gvrow.RowIndex]["StockId"].ToString().Trim();
            ViewState["StockId"] = stockId.ToString();
            BindCategory();
            ddlItemCategory.SelectedValue = gvStockEntry.DataKeys[gvrow.RowIndex]["ItemCategoryId"].ToString().Trim();
            ddlItemCategory.Enabled = false;
            BindItemName();
            ddlItemName.SelectedValue = gvStockEntry.DataKeys[gvrow.RowIndex]["ItemNameId"].ToString().Trim();
            ddlItemName.Enabled = false;
            txtFromDate.Text = gvStockEntry.DataKeys[gvrow.RowIndex]["Date"].ToString().Trim();
            txtFromDate.Enabled = false;
            getUOM();
            ddlUOM.SelectedValue = gvStockEntry.DataKeys[gvrow.RowIndex]["UOM"].ToString().Trim();
            ddlUOM.Enabled = false;
            txtOpeninQty.Text = gvStockEntry.DataKeys[gvrow.RowIndex]["Quantity"].ToString().Trim();
            txtRef.Text = gvStockEntry.DataKeys[gvrow.RowIndex]["Reference"].ToString().Trim();
            txtRef.Enabled = false;

            btnSubmit.Text = "Update";
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divEntry.Visible = true;
        back.Enabled = false;
        Next.Enabled = false;
        lbtnNew.Visible = false;
        clearInputs();
        btnSubmit.Text = "Submit";
        fromTodate.Visible = false;
        divlabel.Visible = false;
        PrevNext.Visible = false;

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        clearInputs();
        ViewState["Flag"] = "R";
        btnSubmit.Text = "Submit";
        BindStockEntry();
     
    }

    public class StockEntry
    {
        public string QueryType { get; set; }
        public string StockId { get; set; }
        public string CategoryId { get; set; }
        public string ItemCategory { get; set; }
        public string ItemNameId { get; set; }
        public string ItemName { get; set; }
        public string Date { get; set; }
        public string UOM { get; set; }
        public string Quantity { get; set; }
        public string Reference { get; set; }
        public string ActiveStatus { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string CreatedBy { get; set; }

        public string ItemCategoryId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string CountStart { get; set; }
        public string Input2 { get; set; }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        gvStockEntry.PageIndex = 0;
        BindStockEntry();
       
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        divEntry.Visible = false;
        divGrid.Visible = false;
        fromTodate.Visible = true;
        txtFromDates.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDates.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        gvStockEntry.PageIndex = 0;
        ViewState["Flag"] = "R";
        ddlCatItem.SelectedValue ="0";
        ddlCatName.SelectedValue = "0";
        BindCatItemName();
        BindStockEntry();
    }


    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BindStockEntry();
    }

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BindStockEntry();
    }

    protected void AddProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;
            Next.Enabled = true;
            back.Enabled = false;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = start + 10;

        }
        else
        {
            iend = end;

        }
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
    }
    protected void subProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
            Next.Enabled = true;
            back.Enabled = false;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            back.Enabled = false;
            Next.Enabled = true;

        }
        else
        {
            iend = end;
            Next.Enabled = true;

        }
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
    }

    

    protected void BackToList_Click(object sender, EventArgs e)
    {
        back.Visible = true;
        Next.Visible = true;
        BackToList.Visible = false;
       BindStockEntry();
    }



    protected void ddlCatName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindCategoryName();
        if (ddlCatName.SelectedValue != "0")
        {
            BindCatItemName();
        }
        else
        {
            ddlCatItem.Items.Clear();
            ddlCatItem.Items.Insert(0, new ListItem("All", "0"));
        }
    }

    public void BindCategoryName()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var cat = new StockEntry()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("FoodCategory/BHId", cat).Result;

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
                            ddlCatName.DataSource = dt;
                            ddlCatName.DataValueField = "CategoryId";
                            ddlCatName.DataTextField = "CategoryName";
                            ddlCatName.DataBind();

                        }
                        else
                        {
                            ddlCatName.DataBind();
                        }
                    }
                    else
                    {

                        //  lblGridMsg.Text = ResponseMsg;

                    }

                    ddlCatName.Items.Insert(0, new ListItem("All", "0"));
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

  

    public void BindCatItemName()
    {

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var cat = new StockEntry()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    CategoryId = ddlCatName.SelectedValue.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("FoodItemName/BHId", cat).Result;

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
                            ddlCatItem.DataSource = dt;
                            ddlCatItem.DataValueField = "ServiceId";
                            ddlCatItem.DataTextField = "ServiceName";
                            ddlCatItem.DataBind();
                            ddlCatItem.Items.Insert(0, new ListItem("All", "0"));

                        }
                        else
                        {
                            ddlCatItem.DataBind();
                        }
                    }
                    else
                    {
                        ddlCatItem.Items.Clear();
                        ddlCatItem.Items.Insert(0, new ListItem("Select Item Name", "0"));
                        //  lblGridMsg.Text = ResponseMsg;

                    }
                    // ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
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
}