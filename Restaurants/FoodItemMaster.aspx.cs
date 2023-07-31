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

public partial class Restaurants_FoodItemMaster : System.Web.UI.Page
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

				//CHANGES
				BindCategoryName();
				ViewState["Flag"] = "B";
				ViewState["hfstartvalue"] = "0";
				ViewState["hfendvalue"] = "0";
				int istart;
				int iend;
				AddProcess(0, 10, out istart, out iend);
				BindFoodItemServices();
				BindTaxDetails();
				BindCategory();


				txtTotalAmt.Attributes.Add("onkeyup", "ServiceCharge('" + txtChargePerItem.ClientID + "', '" + txtServiceTax.ClientID + "')");
				txtChargePerItem.Attributes.Add("readonly", "readonly");
				txtServiceTax.Attributes.Add("readonly", "readonly");
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public class FoodItemServices
    {
        public string CategoryId { get; set; }
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
        public string TaxId { get; set; }
        public string TaxName { get; set; }
        public string CreatedBy { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string QueryType { get; set; }
        public string ActiveStatus { get; set; }
        public string StockEntryMaintenance { get; set; }
        public string CountStart { get; set; }
       
        
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

                var cat = new FoodItemServices()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
               
               HttpResponseMessage response = client.PostAsJsonAsync("FoodCategoryMstr/BHId", cat).Result;
              

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
                            ddlCategoryName.DataValueField = "CategoryId";
                            ddlCategoryName.DataTextField = "CategoryName";
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

                var TaxService = new FoodItemServices()
                {
                    ServiceId = "5",
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
        ddlCategoryName.Enabled = true;
        ddlCategoryName.SelectedIndex = 0;
        txtServiceName.Text = "";
        txtShortName.Text = "";

        txtTotalAmt.Text = "0";
        txtChargePerItem.Text = "0";
        txtServiceTax.Text = "0";

        ddlTaxId.SelectedIndex = 0;
        btnSubmit.Text = "Submit";
        ddlCatName.SelectedValue = "0";
        ddlCatItem.SelectedValue = "0";
        ChkStockEntryMain.Checked = false;
    }

    public void BindFoodItemServices()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatHouseId = new FoodItemServices()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    CategoryId = ddlCatName.SelectedValue.Trim(),
                    ServiceId = ddlCatItem.SelectedValue.Trim(),
                    CountStart = ViewState["hfstartvalue"].ToString()
                };
                /* HttpResponseMessage response = client.PostAsJsonAsync("FoodItemMstr/BHId", BoatHouseId).Result;*/
                HttpResponseMessage response = client.PostAsJsonAsync("FoodItemMstr/BHIdV2", BoatHouseId).Result; 

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ViewState["Flag"] = "B";
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
                            gvFoodItemServices.Visible = true;
                            gvFoodItemServices.DataSource = dt;
                            gvFoodItemServices.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            
                        }
                        else
                        {
                            gvFoodItemServices.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        divGrid.Visible = true;
                        gvFoodItemServices.Visible = false;
                       
                        Next.Enabled = false;
                        back.Enabled = false;
                        if (ViewState["Flag"].ToString() == "N")
                        {
                            back.Enabled = true;
                        }
                        divprevnext.Visible = true;

                        lblGridMsg.Text = ResponseMsg.ToString();
                        //divGrid.Visible = false;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
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

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divSearch.Visible = false;
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

                string StockEntry = string.Empty;
                if (ChkStockEntryMain.Checked == true)
                {
                    StockEntry = "Y";
                }
                else
                {
                    StockEntry = "N";
                }

                HttpResponseMessage response;
                string sMSG = string.Empty;
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var otherservices = new FoodItemServices()
                    {
                        QueryType = "Insert",
                        ServiceId = "0",
                        CategoryId = ddlCategoryName.SelectedValue.Trim(),
                        ServiceName = txtServiceName.Text.Trim(),
                        ShortName = txtShortName.Text.Trim(),

                        ServiceTotalAmount = txtTotalAmt.Text.Trim(),
                        ChargePerItem = txtChargePerItem.Text.Trim(),
                        ChargePerItemTax = txtServiceTax.Text.Trim(),

                        TaxId = sTaxId.Trim(),
                        TaxName = ddlTaxId.SelectedItem.Text,

                        StockEntryMaintenance = StockEntry.ToString().Trim(),

                        CreatedBy = Session["UserId"].ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                   
                    };
                    response = client.PostAsJsonAsync("FoodItemMaster", otherservices).Result;
                    sMSG = "Item Master Details Inserted Successfully";
                }
                else
                {
                    var otherservices = new FoodItemServices()
                    {
                        QueryType = "Update",
                        CategoryId = ddlCategoryName.SelectedValue.Trim(),
                        ServiceId = txtserviceId.Text.Trim(),
                        ServiceName = txtServiceName.Text.Trim(),
                        ShortName = txtShortName.Text.Trim(),

                        ServiceTotalAmount = txtTotalAmt.Text.Trim(),
                        ChargePerItem = txtChargePerItem.Text.Trim(),
                        ChargePerItemTax = txtServiceTax.Text.Trim(),

                        TaxId = sTaxId.Trim(),
                        TaxName = ddlTaxId.SelectedItem.Text,

                        StockEntryMaintenance = StockEntry.ToString().Trim(),

                        CreatedBy = Session["UserId"].ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    };
                    response = client.PostAsJsonAsync("FoodItemMaster", otherservices).Result;
                    sMSG = "Item Master Details Updated Successfully";
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
                            ViewState["hfstartvalue"] = "0";
                            ViewState["hfendvalue"] = "0";
                            int istart;
                            int iend;
                            AddProcess(0, 10, out istart, out iend);

                            Next.Visible = true;
                            back.Visible = true;
                            back.Enabled = false;
                            BindFoodItemServices();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            divSearch.Visible = true;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            clearInputs();
                            ViewState["hfstartvalue"] = "0";
                            ViewState["hfendvalue"] = "0";
                            int istart;
                            int iend;
                            AddProcess(0, 10, out istart, out iend);

                            Next.Visible = true;
                            back.Visible = true;
                            back.Enabled = false;
                            BindFoodItemServices();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divGrid.Visible = true;
                            divSearch.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Item Master Details Already Exists !');", true);
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
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);

        Next.Visible = true;
        back.Visible = true;
        back.Enabled = false;
        clearInputs();
        btnSubmit.Text = "Submit";
        BindFoodItemServices();
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        divSearch.Visible = true;

    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divSearch.Visible = false;
            divGrid.Visible = false;
            divEntry.Visible = true;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            ddlCategoryName.Enabled = false;
            ddlCategoryName.SelectedValue = gvFoodItemServices.DataKeys[gvrow.RowIndex]["CategoryId"].ToString().Trim();
            txtserviceId.Text = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceId"].ToString().Trim();
            txtServiceName.Text = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceName"].ToString().Trim();
            txtShortName.Text = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ShortName"].ToString().Trim();

            txtTotalAmt.Text = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceTotalAmount"].ToString().Trim();
            txtChargePerItem.Text = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ChargePerItem"].ToString().Trim();
            txtServiceTax.Text = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ChargePerItemTax"].ToString().Trim();

            string TaxId = gvFoodItemServices.DataKeys[gvrow.RowIndex]["TaxId"].ToString().Trim();
            hfTaxSlab.Value = gvFoodItemServices.DataKeys[gvrow.RowIndex]["TaxName"].ToString().Trim();

            string StockEntryMain = gvFoodItemServices.DataKeys[gvrow.RowIndex]["StockEntryMaintenance"].ToString().Trim();
            if (StockEntryMain.ToString().Trim() == "Y")
            {
                ChkStockEntryMain.Checked = true;
            }
            else
            {
                ChkStockEntryMain.Checked = false;
            }

            BindTaxDetails();

            if (TaxId == "0")
            {
                ddlTaxId.SelectedIndex = 1;
            }
            else
            {
                ddlTaxId.SelectedValue = gvFoodItemServices.DataKeys[gvrow.RowIndex]["TaxId"].ToString().Trim();

                decimal Tax = 0;
                string[] taxlist = hfTaxSlab.Value.Split(',');

                foreach (var list in taxlist)
                {
                    var TaxName = list;
                    var tx = list.Split('-');

                    Tax += Convert.ToDecimal(tx[1].ToString());
                }

                hfservicetax.Value = Tax.ToString();
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
                var otherservices = new FoodItemServices()
                {
                    QueryType = "Delete",

                    CategoryId = gvFoodItemServices.DataKeys[gvrow.RowIndex]["CategoryId"].ToString().Trim(),
                    ServiceId = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceId"].ToString().Trim(),
                    ServiceName = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceName"].ToString().Trim(),
                    ShortName = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ShortName"].ToString().Trim(),

                    ServiceTotalAmount = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceTotalAmount"].ToString().Trim(),
                    ChargePerItem = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ChargePerItem"].ToString().Trim(),
                    ChargePerItemTax = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ChargePerItemTax"].ToString().Trim(),

                    TaxId = gvFoodItemServices.DataKeys[gvrow.RowIndex]["TaxId"].ToString().Trim(),
                    StockEntryMaintenance = gvFoodItemServices.DataKeys[gvrow.RowIndex]["StockEntryMaintenance"].ToString().Trim(),


                    CreatedBy = Session["UserId"].ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("FoodItemMaster", otherservices).Result;
                sMSG = "Item Master Details Deleted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindFoodItemServices();
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

    protected void gvFoodItemServices_RowDataBound(Object sender, GridViewRowEventArgs e)
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
                var otherservices = new FoodItemServices()
                {
                    QueryType = "ReActive",
                    CategoryId = gvFoodItemServices.DataKeys[gvrow.RowIndex]["CategoryId"].ToString().Trim(),
                    ServiceId = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceId"].ToString().Trim(),
                    ServiceName = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceName"].ToString().Trim(),
                    ShortName = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ShortName"].ToString().Trim(),

                    ServiceTotalAmount = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ServiceTotalAmount"].ToString().Trim(),
                    ChargePerItem = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ChargePerItem"].ToString().Trim(),
                    ChargePerItemTax = gvFoodItemServices.DataKeys[gvrow.RowIndex]["ChargePerItemTax"].ToString().Trim(),

                    TaxId = gvFoodItemServices.DataKeys[gvrow.RowIndex]["TaxId"].ToString().Trim(),
                    StockEntryMaintenance = gvFoodItemServices.DataKeys[gvrow.RowIndex]["StockEntryMaintenance"].ToString().Trim(),


                    CreatedBy = Session["UserId"].ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("FoodItemMaster", otherservices).Result;
                sMSG = "Item Master Details Deleted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindFoodItemServices();
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

    //Newly Added
    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        ViewState["Flag"] = "N";
        BindFoodItemServices();
    }

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BindFoodItemServices();
    }

    protected void AddProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;

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

                var cat = new FoodItemServices()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    CategoryId = ddlCatName.SelectedValue.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("FoodItemName/BHIdV2", cat).Result;

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
                            

                        }
                        else
                        {
                            ddlCatItem.DataBind();
                        }
                    }
                    else
                    {
                        ddlCatItem.Items.Clear();
                        
                        //  lblGridMsg.Text = ResponseMsg;

                    }
                    // ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
                }
                else
                {

                    //  lblGridMsg.Text = response.ToString();
                }
                ddlCatItem.Items.Insert(0, new ListItem("All", "0"));
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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

                var cat = new FoodItemServices()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("FoodCategoryMstr/BHId", cat).Result;
               
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
                            ddlCategoryName.DataBind();
                        }
                    }
                    else
                    {
                        ddlCatName.Items.Clear();
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

    protected void ddlCatName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCatItemName();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
       
        Next.Visible = true;
        back.Visible = true;
        back.Enabled = false;
        BindFoodItemServices();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        
        Next.Visible = true;
        back.Visible = true;
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        back.Enabled = false;
        ddlCatName.SelectedValue = "0";
        ddlCatItem.SelectedValue = "0";
        BindCatItemName();
        BindFoodItemServices();
    }
}