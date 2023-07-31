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

public partial class OfferMaster : System.Web.UI.Page
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
				hfCreatedBy.Value = Session["UserId"].ToString().Trim();
				hfBoatHouseId.Value = Session["BoatHouseId"].ToString().Trim();
				hfBoatHouseName.Value = Session["BoatHouseName"].ToString().Trim();
                //getBoatHouseAll();
             
                getOfferCategory();
				BindOfferMaster();
                //CHANGES
                GetCorporateOffice();

            }
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}
    public void GetCorporateOffice()
    {
        try
        {
            divBoatHouseAll.Visible = false;
            ddlCorpId.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlCorporateOffice",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlCorpId.DataSource = dtExists;
                        ddlCorpId.DataValueField = "CorpId";
                        ddlCorpId.DataTextField = "CorpName";
                        ddlCorpId.DataBind();
                       
                    }
                    else
                    {
                        ddlCorpId.DataBind();
                    }
                    ddlCorpId.Items.Insert(0, "Select Corporate Office");
                

                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ddlCorpId_SelectedIndexChanged(object sender, EventArgs e)
    {
        getBoatHouseAll();
    }
   
    public void getBoatHouseAll()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync("ddlBoatHouse/ListAll?CorpId="+ddlCorpId.SelectedValue+"").Result;


                if (response.IsSuccessStatusCode)
                {
                    var Getresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Getresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Getresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            divBoatHouseAll.Visible = true;
                            chkBoatHouse.DataSource = dt;
                            chkBoatHouse.DataValueField = "BoatHouseId";
                            chkBoatHouse.DataTextField = "BoatHouseName";
                            chkBoatHouse.DataBind();
                        }
                        else
                        {
                            divBoatHouseAll.Visible = false;
                            chkBoatHouse.DataBind();
                        }
                    }
                    else
                    {
                        divBoatHouseAll.Visible = false;
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

    public void getOfferCategory()
    {
        try
        {
            ddlOfferCat.Items.Clear();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/OfferCategory").Result;

                if (response.IsSuccessStatusCode)
                {
                    var Getresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Getresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Getresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlOfferCat.DataSource = dt;
                            ddlOfferCat.DataValueField = "ConfigId";
                            ddlOfferCat.DataTextField = "ConfigName";
                            ddlOfferCat.DataBind();
                        }
                        else
                        {
                            ddlOfferCat.DataBind();
                        }
                        ddlOfferCat.Items.Insert(0, "Select Offer Category");
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
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

    public void BindOfferMaster()
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
                response = client.GetAsync("OfferMstr/ListAll").Result;
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
                            GvOfferMaster.DataSource = dt;
                            GvOfferMaster.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            GvOfferMaster.DataSource = dt;
                            GvOfferMaster.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();

                        divEntry.Visible = true;
                        divgrid.Visible = false;
                        lbtnNew.Visible = false;
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

    protected void ddlOfferCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOfferCat.SelectedValue == "1")
        {
            divMinBillAmt.Visible = true;
            divMinNoTickets.Visible = false;
            txtMinBillAmt.Text = "0";
            txtMiniTickets.Text = "0";
        }
        else if (ddlOfferCat.SelectedValue == "2")
        {
            divMinBillAmt.Visible = false;
            divMinNoTickets.Visible = true;
            txtMinBillAmt.Text = "0";
            txtMiniTickets.Text = "0";
        }
        else
        {
            divMinBillAmt.Visible = false;
            divMinNoTickets.Visible = false;
            txtMinBillAmt.Text = "0";
            txtMiniTickets.Text = "0";
        }
    }

    public void clearInputs()
    {
        txtOfferName.Text = string.Empty;

        ddlOfferCat.SelectedIndex = 0;
        txtOffer.Text = string.Empty;
        divMinBillAmt.Visible = false;
        divMinNoTickets.Visible = false;

        txtMinBillAmt.Text = string.Empty;
        txtMiniTickets.Text = string.Empty;
        txtEffectivefrm.Text = string.Empty;
        txtEffectiveTill.Text = string.Empty;
        rbtnAmountType.SelectedIndex = 0;
        rbtnOfferType.SelectedIndex = 0;
        chkBoatHouse.ClearSelection();

        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        ddlCorpId.ClearSelection();
        ddlCorpId.Enabled = true;
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

                string a = rbtnAmountType.SelectedValue.Trim();
                HttpResponseMessage response;

                string BoatHouseIdList = string.Empty;
                string PselectedValues = String.Join(",",
                     chkBoatHouse.Items.OfType<ListItem>().Where(r => r.Selected)
                    .Select(r => r.Value));
                BoatHouseIdList = PselectedValues;

                string BoatHouseList = string.Empty;
                string PselectedItems = String.Join(",",
                     chkBoatHouse.Items.OfType<ListItem>().Where(r => r.Selected)
                    .Select(r => r.Text.Trim()));
                BoatHouseList = PselectedItems;

                if (txtMiniTickets.Text == "" && txtMinBillAmt.Text == "")
                {
                    txtMiniTickets.Text = "0";
                    txtMinBillAmt.Text = "0";
                }

                if (ddlOfferCat.SelectedValue == "1")
                {
                    divMinBillAmt.Visible = true;
                    divMinNoTickets.Visible = false;
                    txtMiniTickets.Text = "0";
                }
                else if (ddlOfferCat.SelectedValue == "2")
                {
                    divMinBillAmt.Visible = false;
                    divMinNoTickets.Visible = true;
                    txtMinBillAmt.Text = "0";
                }
                else
                {
                    divMinBillAmt.Visible = false;
                    divMinNoTickets.Visible = false;
                    txtMinBillAmt.Text = "0";
                    txtMiniTickets.Text = "0";
                }

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var Offermaster = new Offermaster()
                    {
                        QueryType = "Insert",
                        OfferId = "0",
                        OfferType = rbtnOfferType.SelectedValue.Trim(),
                        OfferCategory = ddlOfferCat.SelectedValue.Trim(),
                        OfferName = txtOfferName.Text,
                        AmountType = rbtnAmountType.SelectedValue.Trim(),
                        OfferAmount = txtOffer.Text,
                        MinBillAmount = txtMinBillAmt.Text,
                        MinNoOfTickets = txtMiniTickets.Text,
                        EffectiveFrom = txtEffectivefrm.Text,
                        EffectiveTill = txtEffectiveTill.Text,
                        BoatHouseId = BoatHouseIdList.Trim(),
                        BoatHouseName = BoatHouseList.Trim(),
                        Createdby = hfCreatedBy.Value.Trim(),
                        CorpId=ddlCorpId.SelectedValue,
                    };

                    response = client.PostAsJsonAsync("OfferDiscount", Offermaster).Result;

                }
                else
                {
                    var Offermaster = new Offermaster()
                    {
                        QueryType = "Update",
                        OfferId = txtOfferId.Text,
                        OfferType = rbtnOfferType.SelectedValue.Trim(),
                        OfferCategory = ddlOfferCat.SelectedValue.Trim(),
                        OfferName = txtOfferName.Text,
                        AmountType = rbtnAmountType.SelectedValue.Trim(),
                        OfferAmount = txtOffer.Text,
                        MinBillAmount = txtMinBillAmt.Text,
                        MinNoOfTickets = txtMiniTickets.Text,
                        EffectiveFrom = txtEffectivefrm.Text,
                        EffectiveTill = txtEffectiveTill.Text,
                        BoatHouseId = BoatHouseIdList.Trim(),
                        BoatHouseName = BoatHouseList.Trim(),
                        Createdby = hfCreatedBy.Value.Trim(),
                        CorpId = ddlCorpId.SelectedValue
                    };

                    response = client.PostAsJsonAsync("OfferDiscount", Offermaster).Result;

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindOfferMaster();
                        clearInputs();
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
        clearInputs();
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divgrid.Visible = false;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = GvOfferMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label OfferId = (Label)gvrow.FindControl("lblOfferId");
            Label OfferType = (Label)gvrow.FindControl("lblOfferType");
            Label OfferCategory = (Label)gvrow.FindControl("lblOfferCategory");
            Label OfferName = (Label)gvrow.FindControl("lblOfferName");
            Label AmountType = (Label)gvrow.FindControl("lblAmountType");
            Label Offer = (Label)gvrow.FindControl("lblOffer");
            Label MinBIllAmount = (Label)gvrow.FindControl("lblMinBillAmount");
            Label MinNoOfTickets = (Label)gvrow.FindControl("lblMinNoOfTickets");
            Label EffectiveFrom = (Label)gvrow.FindControl("lblEffectiveFrom");
            Label EffectiveTill = (Label)gvrow.FindControl("lblEffectiveTill");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label CorpId = (Label)gvrow.FindControl("lblCorpId");

            txtOfferId.Text = OfferId.Text;
            rbtnOfferType.SelectedValue = OfferType.Text;
            ddlOfferCat.SelectedValue = OfferCategory.Text;
            GetCorporateOffice();
            ddlCorpId.SelectedValue = CorpId.Text;
            if (ddlCorpId.SelectedIndex != 0)
            {
                ddlCorpId.Enabled = false;
                getBoatHouseAll();

            }
            
            if (ddlOfferCat.SelectedValue == "1")
            {
                divMinBillAmt.Visible = true;
                divMinNoTickets.Visible = false;
                txtMinBillAmt.Text = MinBIllAmount.Text;
                txtMiniTickets.Text = "0";
            }
            else if (ddlOfferCat.SelectedValue == "2")
            {
                divMinBillAmt.Visible = false;
                divMinNoTickets.Visible = true;
                txtMinBillAmt.Text = "0";
                txtMiniTickets.Text = MinNoOfTickets.Text;
            }
            else
            {
                divMinBillAmt.Visible = false;
                divMinNoTickets.Visible = false;
                txtMinBillAmt.Text = "0";
                txtMiniTickets.Text = "0";
            }

            txtOfferName.Text = OfferName.Text;
            rbtnAmountType.SelectedValue = AmountType.Text;
            txtOffer.Text = Offer.Text;
            txtEffectivefrm.Text = EffectiveFrom.Text;
            txtEffectiveTill.Text = EffectiveTill.Text;

            string btHouseId = BoatHouseId.Text.Trim();
            string[] sbtHouseId = btHouseId.Split(',');
            int btHouseIdCount = sbtHouseId.Count();
            for (int i = 0; i < btHouseIdCount; i++)
            {
                foreach (ListItem item in chkBoatHouse.Items)
                {
                    string selectedValue = item.Value;
                    if (selectedValue == sbtHouseId[i].ToString())
                    {
                        item.Selected = true;
                    }
                }
            }

            string btHouse = BoatHouseName.Text.Trim();
            string[] sbtHouse = btHouse.Split(',');
            int btHouseCount = sbtHouse.Count();
            for (int i = 0; i < btHouseCount; i++)
            {
                foreach (ListItem item in chkBoatHouse.Items)
                {
                    string selectedText = item.Text;
                    if (selectedText == sbtHouse[i].ToString())
                    {
                        item.Selected = true;
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
            string sTesfg = GvOfferMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label OfferId = (Label)gvrow.FindControl("lblOfferId");
            Label OfferName = (Label)gvrow.FindControl("lblOfferName");
            Label OfferType = (Label)gvrow.FindControl("lblOfferType");
            Label AmountType = (Label)gvrow.FindControl("lblAmountType");
            Label Offer = (Label)gvrow.FindControl("lblOffer");
            Label MinBIllAmount = (Label)gvrow.FindControl("lblMinBillAmount");
            Label MinNoOfTickets = (Label)gvrow.FindControl("lblMinNoOfTickets");
            Label EffectiveFrom = (Label)gvrow.FindControl("lblEffectiveFrom");
            Label EffectiveTill = (Label)gvrow.FindControl("lblEffectiveTill");
            Label CorpId = (Label)gvrow.FindControl("lblCorpId");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label OfferCategory = (Label)gvrow.FindControl("lblOfferCategory");
            hfBoatHouseId.Value = BoatHouseId.Text;
            hfBoatHouseName.Value = BoatHouseName.Text;
            txtOfferId.Text = OfferId.Text;
            txtOfferName.Text = OfferName.Text;
            rbtnOfferType.SelectedValue = OfferType.Text;
            rbtnAmountType.SelectedValue = AmountType.Text;
            txtOffer.Text = Offer.Text;
            txtMinBillAmt.Text = MinBIllAmount.Text;
            txtMiniTickets.Text = MinNoOfTickets.Text;
            txtEffectivefrm.Text = EffectiveFrom.Text;
            txtEffectiveTill.Text = EffectiveTill.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string a = rbtnAmountType.SelectedValue.Trim();
                HttpResponseMessage response;

                var Offermaster = new Offermaster()
                {
                    QueryType = "Delete",
                    OfferId = txtOfferId.Text,
                    OfferName = txtOfferName.Text,
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    BoatHouseName = hfBoatHouseName.Value.Trim(),
                    OfferCategory = OfferCategory.Text,
                    OfferType = rbtnOfferType.SelectedValue.Trim(),
                    AmountType = rbtnAmountType.SelectedValue.Trim(),
                    OfferAmount = txtOffer.Text,
                    MinBillAmount = "100",
                    MinNoOfTickets = txtMiniTickets.Text,
                    EffectiveFrom = txtEffectivefrm.Text,
                    EffectiveTill = txtEffectiveTill.Text,
                    Createdby = hfCreatedBy.Value.Trim(),
                    CorpId=CorpId.Text
                };

                response = client.PostAsJsonAsync("OfferDiscount", Offermaster).Result;





                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindOfferMaster();
                        clearInputs();
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

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        GetCorporateOffice();
        divEntry.Visible = true;
        divgrid.Visible = false;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
    }

    protected void GvOfferMaster_RowDataBound(Object sender, GridViewRowEventArgs e)
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
            string sTesfg = GvOfferMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label OfferId = (Label)gvrow.FindControl("lblOfferId");
            Label OfferName = (Label)gvrow.FindControl("lblOfferName");
            Label OfferType = (Label)gvrow.FindControl("lblOfferType");
            Label AmountType = (Label)gvrow.FindControl("lblAmountType");
            Label Offer = (Label)gvrow.FindControl("lblOffer");
            Label MinBIllAmount = (Label)gvrow.FindControl("lblMinBillAmount");
            Label MinNoOfTickets = (Label)gvrow.FindControl("lblMinNoOfTickets");
            Label EffectiveFrom = (Label)gvrow.FindControl("lblEffectiveFrom");
            Label EffectiveTill = (Label)gvrow.FindControl("lblEffectiveTill");
            Label CorpId = (Label)gvrow.FindControl("lblCorpId");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label OfferCategory = (Label)gvrow.FindControl("lblOfferCategory");

            hfBoatHouseId.Value = BoatHouseId.Text;
            hfBoatHouseName.Value = BoatHouseName.Text;
            txtOfferId.Text = OfferId.Text;
            txtOfferName.Text = OfferName.Text;
            rbtnOfferType.SelectedValue = OfferType.Text;
            rbtnAmountType.SelectedValue = AmountType.Text;
            txtOffer.Text = Offer.Text;
            txtMinBillAmt.Text = MinBIllAmount.Text;
            txtMiniTickets.Text = MinNoOfTickets.Text;
            txtEffectivefrm.Text = EffectiveFrom.Text;
            txtEffectiveTill.Text = EffectiveTill.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string a = rbtnAmountType.SelectedValue.Trim();
                HttpResponseMessage response;

                var Offermaster = new Offermaster()
                {
                    QueryType = "ReActive",
                    OfferId = txtOfferId.Text,
                    OfferName = txtOfferName.Text,
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    BoatHouseName = hfBoatHouseName.Value.Trim(),
                    OfferType = rbtnOfferType.SelectedValue.Trim(),
                    AmountType = rbtnAmountType.SelectedValue.Trim(),
                    OfferCategory = OfferCategory.Text,
                    OfferAmount = txtOffer.Text,
                    MinBillAmount = "100",
                    MinNoOfTickets = txtMiniTickets.Text,
                    EffectiveFrom = txtEffectivefrm.Text,
                    EffectiveTill = txtEffectiveTill.Text,
                    Createdby = hfCreatedBy.Value.Trim(),
                    CorpId = CorpId.Text
                };

                response = client.PostAsJsonAsync("OfferDiscount", Offermaster).Result;





                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindOfferMaster();
                        clearInputs();
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

    public class Offermaster
    {
        public string QueryType { get; set; }
        public string OfferId { get; set; }
        public string OfferType { get; set; }
        public string OfferCategory { get; set; }
        public string OfferName { get; set; }
        public string AmountType { get; set; }
        public string OfferAmount { get; set; }
        public string MinBillAmount { get; set; }
        public string MinNoOfTickets { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTill { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string Createdby { get; set; }
        public string CorpId { get; set; }
    }

    public class FA_CommonMethod
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }
}