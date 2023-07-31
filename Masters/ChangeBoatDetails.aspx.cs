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

public partial class Boating_ChangeBoatDetails : System.Web.UI.Page
{
	public string GetAntiForgeryToken()
	{
		string cookieToken, formToken;
		AntiForgery.GetTokens(null, out cookieToken, out formToken);
		ViewState["__AntiForgeryCookie"] = cookieToken;
		return formToken;

	}

	IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);
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
				BindBoatChangesLog();
				txtBookingId.Focus();
				hfBoatHouseId.Value = Session["BoatHouseId"].ToString().Trim();
				hfBoatHouseName.Value = Session["BoatHouseName"].ToString().Trim();
				hfUserId.Value = Session["UserId"].ToString().Trim();
				//CHANGES

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
        txtBookingId.Text = "";
        txtBookingId.Focus();
        lbtnNew.Visible = false;
        CustomDetails.Visible = false;
        divEntry.Visible = true;
        DivChangeBoatdtl.Visible = false;
        gvBoatChangedDisplay.Visible = false;
        divChargeDetails.Visible = false;
        divline.Visible = false;
    }

    public void BindBoatChangesLog()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "ChangeBoatDet",
                    ServiceType = "GetBoatChangesLog",
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
                        gvBoatChangedDisplay.DataSource = dtExists;
                        gvBoatChangedDisplay.DataBind();
                        divGridBoatChange.Visible = true;
                        gvBoatChangedDisplay.Visible = true;
                        divEntry.Visible = false;
                        DivChangeBoatdtl.Visible = false;
                        divExistDtl.Visible = false;
                        divChangeDetails.Visible = false;                                             
                        lbtnNew.Visible = true;
                        divbutton.Visible = false;
                        divChargeDetails.Visible = false;
                        divline.Visible = false;
                        divline1.Visible = false;
                    }
                    else
                    {
                        gvBoatChangedDisplay.DataBind();
                        divGridBoatChange.Visible = true;
                        gvBoatChangedDisplay.Visible = true;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void txtBookingId_TextChanged(object sender, EventArgs e)
    {
        CustomDetails.Visible = true;
        BindCustomDetails();
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "ChangeBoatDet",
                    ServiceType = "GetBookingDet",
                    Input1 = txtBookingId.Text.Trim(),
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
                        gvChangeBoat.DataSource = dtExists;
                        gvChangeBoat.DataBind();

                        DivChangeBoatdtl.Visible = true;
                    }
                    else
                    {
                        gvChangeBoat.DataBind();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindCustomDetails()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "ChangeBoatDet",
                    ServiceType = "GetBookingMobile",
                    Input1 = txtBookingId.Text.Trim(),
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
                        lblMblNo.Text = dtExists.Rows[0]["CustomerMobile"].ToString();
                        lblBookingdate.Text = dtExists.Rows[0]["BookingDate"].ToString(); 

                        string PremiumStatus = dtExists.Rows[0]["PremiumStatus"].ToString();                       
                        ViewState["BoatStatus"] = PremiumStatus;

                        if (PremiumStatus == Convert.ToString('N'))
                        {
                            lblStatus.Text = "Normal";
                        }
                        else
                        {
                            lblStatus.Text = "Premium";
                        }
                    }                   
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void lblBookingPin_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string BookingPin = gvChangeBoat.DataKeys[gvrow.RowIndex].Value.ToString();

            ViewState["BookingPin"] = BookingPin;
            Label BoatRefNo = (Label)gvrow.FindControl("lblBoatReferenceNo");
            ViewState["BoatReferenceNo"] = BoatRefNo.Text;

            BindExistingDetails();
            GetBoatType();

            ddlBoatSeat.Items.Clear();
            ddlBoatSeat.Items.Insert(0, new ListItem("Select", "0"));
            txtChangeBoatChrg.Text = "";
            txtChangeRowerChrg.Text = "";
            txtChangeDeposit.Text = "";
            txtChangeNetAmt.Text = "";
            txtChangeTax.Text = "";
            divExistDtl.Visible = true;
            divChangeDetails.Visible = true;
            divChargeDetails.Visible = false;       
            divbutton.Visible = true;
            divline.Visible = true;
            divline1.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindExistingDetails()
     {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "ChangeBoatDet",
                    ServiceType = "GetBookingCharges",
                    Input1 = txtBookingId.Text.Trim(),
                    Input2 = ViewState["BookingPin"].ToString().Trim(),
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
                        for (int i = 0; i < dtExists.Rows.Count; i++)
                        {
                            string BoatTypeId = dtExists.Rows[i]["BoatTypeId"].ToString();
                            ViewState["OldBoatTypeId"] = BoatTypeId;
                            string BoatType = dtExists.Rows[i]["BoatType"].ToString();
                            txtBoatType.Text = BoatType;
                            string BoatSeaterId = dtExists.Rows[i]["BoatSeaterId"].ToString();
                            ViewState["OldBoatSeatId"] = BoatSeaterId;
                            string BoatSeat = dtExists.Rows[i]["SeaterType"].ToString();
                            txtBoatSeat.Text = BoatSeat;
                            string BoatId = dtExists.Rows[i]["ActualBoatId"].ToString();
                            ViewState["OldBoatId"] = BoatId;
                            string BoatNum = dtExists.Rows[i]["ActualBoatNum"].ToString();
                            ViewState["OldBoatNum"] = BoatNum;
                            string BoatCharge = dtExists.Rows[i]["InitBoatCharge"].ToString();
                            txtBoatCharge.Text = BoatCharge;
                            string RowerCharge = dtExists.Rows[i]["InitRowerCharge"].ToString();
                            txtRowerCharge.Text = RowerCharge;
                            string OfferAmount = dtExists.Rows[i]["InitOfferAmount"].ToString();
                            txtOfferAmt.Text = OfferAmount;
                            string NetAmount = dtExists.Rows[i]["InitNetAmount"].ToString();
                            txtNetAmt.Text = NetAmount;
                            ViewState["NetAmount"] = NetAmount;
                            string Deposit = dtExists.Rows[i]["BoatDeposit"].ToString();
                            txtDeposit.Text = Deposit;
                            string TaxAmount = dtExists.Rows[i]["TaxAmt"].ToString();
                            txtTaxAmt.Text = TaxAmount;

                            divExistDtl.Visible = true;
                        }
                    }
                    else
                    {
                        divExistDtl.Visible = false;
                    }

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
                var BoatTrip = new CommonClass()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatType/BoatRateMstr", BoatTrip).Result;

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

                        ddlBoatType.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetBoatSeat()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "ChangeBoatDet",
                    ServiceType = "GetBoatSeater",
                    Input1 = "",
                    Input2 = ddlBoatType.SelectedValue.Trim(),
                    Input3 = ViewState["BoatStatus"].ToString().Trim(),
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
                        ddlBoatSeat.DataSource = dtExists;
                        ddlBoatSeat.DataValueField = "BoatSeaterId";
                        ddlBoatSeat.DataTextField = "SeaterType";
                        ddlBoatSeat.DataBind();
                    }
                    else
                    {
                        ddlBoatSeat.DataBind();
                        ddlBoatSeat.Items.Clear();
                    }
                    ddlBoatSeat.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    ddlBoatSeat.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    private void CheckBoatAvailableDetails()
    {
        try
        {
            IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new CommonClass()
                {
                    QueryType = "GetBoatAvailableDetails",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = ViewState["NewBoatTypeId"].ToString().Trim(),
                    Input2 = ViewState["NewBoatSeatId"].ToString().Trim(),
                    Input3 = DateTime.Parse(lblBookingdate.Text, objEnglishDate).ToString(),
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["cNormalAvailable"] = dtExists.Rows[0]["NormalAvailable"].ToString();
                        ViewState["cPremiumAvailable"] = dtExists.Rows[0]["PremiumAvailable"].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Available Boats Today...!');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindChangeDetails()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "ChangeBoatDet",
                    ServiceType = "GetBoatCharges",
                    Input1 = "",
                    Input2 = ddlBoatType.SelectedValue.Trim(),
                    Input3 = ddlBoatSeat.SelectedValue.Trim(),
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
                        for (int i = 0; i < dtExists.Rows.Count; i++)
                        {
                            string BoatTypeId = dtExists.Rows[i]["BoatTypeId"].ToString();
                            ViewState["NewBoatTypeId"] = BoatTypeId;

                            string BoatSeaterId = dtExists.Rows[i]["BoatSeaterId"].ToString();
                            ViewState["NewBoatSeatId"] = BoatSeaterId;

                            string NewBoatCharge = dtExists.Rows[i]["BoatMinCharge"].ToString();
                            txtChangeBoatChrg.Text = NewBoatCharge;

                            string NewRowerCharge = dtExists.Rows[i]["RowerMinCharge"].ToString();
                            txtChangeRowerChrg.Text = NewRowerCharge;

                            string NewNetAmount = dtExists.Rows[i]["BoatMinTotAmt"].ToString();                                                        
                            string DepositType = dtExists.Rows[i]["DepositType"].ToString();
                            string NewTaxAmount = dtExists.Rows[i]["BoatMinTaxAmt"].ToString();

                            txtChangeTax.Text = NewTaxAmount;

                            if (DepositType == "F")
                            {
                                string NewDeposit = dtExists.Rows[i]["Deposit"].ToString();
                                txtChangeDeposit.Text = NewDeposit;
                                
                                decimal FinalNetFixed = Convert.ToDecimal(NewBoatCharge) + Convert.ToDecimal(NewRowerCharge) 
                                    + Convert.ToDecimal(NewTaxAmount) + Convert.ToDecimal(NewDeposit);

                                txtChangeNetAmt.Text = FinalNetFixed.ToString();
                                ViewState["NewNetAmount"] = FinalNetFixed;
                            }
                            else if(DepositType == "P")
                            {
                                string NewDeposit = dtExists.Rows[i]["Deposit"].ToString(); 
                                
                                decimal NewDepositPerPercent = (Convert.ToDecimal(NewDeposit.Trim()) / 100)* Convert.ToDecimal(NewNetAmount.Trim());
                                txtChangeDeposit.Text = NewDepositPerPercent.ToString();

                                decimal FinalNetAmount = Convert.ToDecimal(NewBoatCharge) + Convert.ToDecimal(NewRowerCharge) + Convert.ToDecimal(NewTaxAmount) + NewDepositPerPercent;
                               
                                txtChangeNetAmt.Text = FinalNetAmount.ToString();
                                ViewState["NewNetAmount"] = FinalNetAmount;
                            }    
                            
                            divChangeDetails.Visible = true;
                        }
                    }
                    else
                    {
                        divChangeDetails.Visible = false;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBoatSeat();
        txtChangeBoatChrg.Text = "";
        txtChangeRowerChrg.Text = "";
        txtChangeDeposit.Text = "";
        txtChangeNetAmt.Text = "";
        txtChangeTax.Text = "";
        divRefund.Visible = false;
        divExtraCharge.Visible = false;
        divAvailable.Visible = false;
        divChargeDetails.Visible = false;
    }

    protected void ddlBoatSeat_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindChangeDetails();

        if(ViewState["OldBoatTypeId"].ToString() == ViewState["NewBoatTypeId"].ToString() && 
            ViewState["OldBoatSeatId"].ToString() == ViewState["NewBoatSeatId"].ToString())
        {
            divChargeDetails.Visible = true;
            divsameboat.Visible = true;
            lblSameBoat.Text = "Same Boat Type & Seater is Booked Before !!!";
            divAvailable.Visible = false;
            divRefund.Visible = false;
            divExtraCharge.Visible = false;
            divbutton.Visible = false;
            txtChangeBoatChrg.Text = "";
            txtChangeRowerChrg.Text = "";
            txtChangeDeposit.Text = "";
            txtChangeNetAmt.Text = "";
            txtChangeTax.Text = "";
        }
        else
        {
            CheckBoatAvailableDetails();
            divChargeDetails.Visible = true;      
            
            if (ViewState["BoatStatus"].ToString().Trim() == "N")
            {
                divAvailable.Visible = true;
                lblAvailableBoat.Text = ViewState["cNormalAvailable"].ToString().Trim();
            }
            else
            {
                divAvailable.Visible = true;
                lblAvailableBoat.Text = ViewState["cPremiumAvailable"].ToString().Trim();
            }

            if (Convert.ToDecimal(ViewState["NetAmount"].ToString()) > Convert.ToDecimal(ViewState["NewNetAmount"].ToString()))
            {
                divRefund.Visible = true;
                decimal Refund = Convert.ToDecimal(ViewState["NetAmount"].ToString()) - Convert.ToDecimal(ViewState["NewNetAmount"].ToString());
                lblRefundAmt.Text = Refund.ToString();
                divRefund.Visible = true;
                divExtraCharge.Visible = false;
                divsameboat.Visible = false;
                divbutton.Visible = true;
            }
            else if (Convert.ToDecimal(ViewState["NetAmount"].ToString()) < Convert.ToDecimal(ViewState["NewNetAmount"].ToString()))
            {
                divExtraCharge.Visible = true;
                decimal Extra = Convert.ToDecimal(ViewState["NewNetAmount"].ToString()) - Convert.ToDecimal(ViewState["NetAmount"].ToString());
                lblExtraAmt.Text = Extra.ToString();
                divExtraCharge.Visible = true;
                divRefund.Visible = false;
                divsameboat.Visible = false;
                divbutton.Visible = true;
            }
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
                string FilePath = string.Empty;
                string a = string.Empty;
                string QueryType = string.Empty;

                HttpResponseMessage response;
                string sMSG = string.Empty;
                string OldBoatCharge = string.Empty;
                string OldRowerCharge = string.Empty;
                string OldBoatDeposit = string.Empty;
                string offeramt = string.Empty;
                string OldNetAmount = string.Empty;
                string OldTaxAmount = string.Empty;

                string NewBoatCharge = string.Empty;
                string NewRowerCharge = string.Empty;
                string NewBoatDeposit = string.Empty;
                string NewNetAmount = string.Empty;
                string NewTaxAmount = string.Empty;
                string Refund = string.Empty;
                string ExtraCharge = string.Empty;

                if (txtBoatCharge.Text == "")
                {
                    OldBoatCharge = "0";
                }
                else
                {
                    OldBoatCharge = txtBoatCharge.Text;
                }

                if (txtRowerCharge.Text == "")
                {
                    OldRowerCharge = "0";
                }
                else
                {
                    OldRowerCharge = txtRowerCharge.Text;
                }

                if (txtDeposit.Text == "")
                {
                    OldBoatDeposit = "0";
                }
                else
                {
                    OldBoatDeposit = txtDeposit.Text;
                }

                if (txtOfferAmt.Text == "")
                {
                    offeramt = "0";
                }
                else
                {
                    offeramt = txtOfferAmt.Text;
                }

                if (txtNetAmt.Text == "")
                {
                    OldNetAmount = "0";
                }
                else
                {
                    OldNetAmount = txtNetAmt.Text;
                }

                if (txtTaxAmt.Text == "")
                {
                    OldTaxAmount = "0";
                }
                else
                {
                    OldTaxAmount = txtTaxAmt.Text;
                }

                if (txtChangeBoatChrg.Text == "")
                {
                    NewBoatCharge = "0";
                }
                else
                {
                    NewBoatCharge = txtChangeBoatChrg.Text;
                }

                if (txtChangeRowerChrg.Text == "")
                {
                    NewRowerCharge = "0";
                }
                else
                {
                    NewRowerCharge = txtChangeRowerChrg.Text;
                }

                if (txtChangeDeposit.Text == "")
                {
                    NewBoatDeposit = "0";
                }
                else
                {
                    NewBoatDeposit = txtChangeDeposit.Text;
                }

                if (txtChangeNetAmt.Text == "")
                {
                    NewNetAmount = "0";
                }
                else
                {
                    NewNetAmount = txtChangeNetAmt.Text;
                }

                if (txtChangeTax.Text == "")
                {
                    NewTaxAmount = "0";
                }
                else
                {
                    NewTaxAmount = txtChangeTax.Text;
                }

                if (lblRefundAmt.Text == "")
                {
                    Refund = "0";
                }
                else
                {
                    Refund = lblRefundAmt.Text;
                }

                if (lblExtraAmt.Text == "")
                {
                    ExtraCharge = "0";
                }
                else
                {
                    ExtraCharge = lblExtraAmt.Text;
                }

                var BoatRate = new ChangeBoatLog()
                {
                    QueryType = "Insert",
                    BookingId = txtBookingId.Text.Trim(),
                    BookingDate = lblBookingdate.Text.Trim(),
                    BookingPin = ViewState["BookingPin"].ToString().Trim(),
                    BoatReferenceNo = ViewState["BoatReferenceNo"].ToString(),
                    PremiumStatus = ViewState["BoatStatus"].ToString().Trim(),
                    OldBoatTypeId = ViewState["OldBoatTypeId"].ToString().Trim(),
                    OldBoatSeaterId = ViewState["OldBoatSeatId"].ToString().Trim(),
                    OldBoatId = ViewState["OldBoatId"].ToString().Trim(),
                    OldBoatNum = ViewState["OldBoatNum"].ToString().Trim(),
                    OldBoatCharge = OldBoatCharge,
                    OldRowerCharge = OldRowerCharge,
                    OldDeposit = OldBoatDeposit,
                    OldOfferAmount = offeramt,
                    OldNetAmount = OldNetAmount,
                    OldTaxAmount = OldTaxAmount,
                    NewBoatTypeId = ViewState["NewBoatTypeId"].ToString().Trim(),
                    NewBoatSeaterId = ViewState["NewBoatSeatId"].ToString().Trim(),
                    NewBoatCharge = NewBoatCharge,
                    NewRowerCharge = NewRowerCharge,
                    NewDeposit = NewBoatDeposit,
                    NewNetAmount = NewNetAmount,
                    NewTaxAmount = NewTaxAmount,
                    ExtraRefundAmount = Refund,
                    ExtraCharge = ExtraCharge,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    UpdatedBy = Session["UserId"].ToString().Trim()

                };

                response = client.PostAsJsonAsync("ChangeBoatDetails/Insert", BoatRate).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindBoatChangesLog();

                        sMSG = "Boat Change Details Inserted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        divGridBoatChange.Visible = false;
                        gvBoatChangedDisplay.Visible = false;
                        divEntry.Visible = true;
                        DivChangeBoatdtl.Visible = false;
                        divExistDtl.Visible = false;
                        divChangeDetails.Visible = false;
                        divRefund.Visible = false;
                        divExtraCharge.Visible = false;
                        lbtnNew.Visible = false;
                        CustomDetails.Visible = false;
                        divAvailable.Visible = false;
                        txtBookingId.Text = "";
                        ddlBoatSeat.Items.Clear();
                        ddlBoatSeat.Items.Insert(0, new ListItem("Select", "0"));

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Change Boat Details UnSuccessfull.');", true);
                    }
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtBookingId.Text = "";
        txtBookingId.Focus();
        divEntry.Visible = true;
        CustomDetails.Visible = false;
        DivChangeBoatdtl.Visible = false;
        divExistDtl.Visible = false;
        divChangeDetails.Visible = false;
        divRefund.Visible = false;
        divExtraCharge.Visible = false;
        divGridBoatChange.Visible = false;
        divAvailable.Visible = false;
        divChargeDetails.Visible = false;
        divline.Visible = false;
        divline1.Visible = false;
    }

    protected void ImgBtnView_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string Id = gvBoatChangedDisplay.DataKeys[gvrow.RowIndex].Value.ToString();
            Label lblBookId = (Label)gvrow.FindControl("lblBookingId");
            Label lblBookPin = (Label)gvrow.FindControl("lblBookingPin");
            Label lblBookRef = (Label)gvrow.FindControl("lblBoatRefNo");
            ViewState["BookingIdLog"] = lblBookId.Text.Trim();
            ViewState["BookingPinLog"] = lblBookPin.Text.Trim();
            ViewState["BookingRefLog"] = lblBookRef.Text.Trim();

            lblPopBookId.Text = ViewState["BookingIdLog"].ToString();
            lblPopBookPin.Text = ViewState["BookingPinLog"].ToString();
            lblPopBookRef.Text = ViewState["BookingRefLog"].ToString();

            BindPopOldLogDetails();
            BindPopNewLogDetails();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    public void BindPopOldLogDetails()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "ChangeBoatDet",
                    ServiceType = "GetOldBoatDet",
                    Input1 = ViewState["BookingIdLog"].ToString(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = ViewState["BookingPinLog"].ToString(),
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
                        gvOldLogDet.DataSource = dtExists;
                        gvOldLogDet.DataBind();
                        gvOldLogDet.Visible = true;

                    }
                    else
                    {
                        gvOldLogDet.DataBind();

                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindPopNewLogDetails()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "ChangeBoatDet",
                    ServiceType = "GetNewBoatDet",
                    Input1 = ViewState["BookingIdLog"].ToString(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = ViewState["BookingPinLog"].ToString(),
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
                        gvNewLogDet.DataSource = dtExists;
                        gvNewLogDet.DataBind();
                        gvNewLogDet.Visible = true;

                    }
                    else
                    {
                        gvNewLogDet.DataBind();

                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvBoatChangedDisplay_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void gvOldLogDet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOldLogDet.PageIndex = e.NewPageIndex;
        BindPopOldLogDetails();
    }

    protected void gvNewLogDet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNewLogDet.PageIndex = e.NewPageIndex;
        BindPopNewLogDetails();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divGridBoatChange.Visible = true;
        gvBoatChangedDisplay.Visible = true;
        lbtnNew.Visible = true;
        divEntry.Visible = false;
        DivChangeBoatdtl.Visible = false;
        divExistDtl.Visible = false;
        divChangeDetails.Visible = false;
        divRefund.Visible = false;
        divExtraCharge.Visible = false;
        divAvailable.Visible = false;
        divline.Visible = false;
        divline1.Visible = false;
        divChargeDetails.Visible = false;
        divbutton.Visible = false;
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

    public class ChangeBoatLog
    {
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public string BookingDate { get; set; }
        public string BookingPin { get; set; }
        public string BoatReferenceNo { get; set; }
        public string PremiumStatus { get; set; }
        public string OldBoatTypeId { get; set; }
        public string OldBoatSeaterId { get; set; }
        public string OldBoatId { get; set; }
        public string OldBoatNum { get; set; }
        public string OldBoatCharge { get; set; }
        public string OldRowerCharge { get; set; }
        public string OldDeposit { get; set; }
        public string OldOfferAmount { get; set; }
        public string OldNetAmount { get; set; }
        public string OldTaxAmount { get; set; }
        public string NewBoatTypeId { get; set; }
        public string NewBoatSeaterId { get; set; }
        public string NewBoatCharge { get; set; }
        public string NewRowerCharge { get; set; }
        public string NewDeposit { get; set; }
        public string NewNetAmount { get; set; }
        public string NewTaxAmount { get; set; }
        public string ExtraRefundAmount { get; set; }
        public string ExtraCharge { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string UpdatedBy { get; set; }
    }

}