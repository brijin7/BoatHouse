using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using System.Web.Helpers;

public partial class Restaurants_BookingRestaurantServices : System.Web.UI.Page
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
				if (Session["BMBookingRestaurant"].ToString().Trim() == "Y")
				{
					GetPaymentType();
					BindOtherCategoryList();
					BindOtherServiceNameList();

					BindResCountAmount();
					getStockStatus();
				}
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public void BindOtherCategoryList()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var CatType = new Otherservices()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("FoodCategory/BHId", CatType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        DataRow dr = null;
                        if (dt.Rows.Count > 0)
                        {
                            dr = dt.NewRow();
                            dr["CategoryId"] = 0;
                            dr["CategoryName"] = "All";
                            dt.Rows.InsertAt(dr, 0);
                            dtlOther.DataSource = dt;
                            dtlOther.DataBind();
                            dtlOther.Visible = true;
                            lblothermsg.Visible = false;
                        }
                        else
                        {
                            dtlOther.Visible = false;
                            lblothermsg.Visible = true;
                            lblothermsg.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        dtlOther.Visible = false;
                        lblothermsg.Visible = true;
                        lblothermsg.Text = ResponseMsg.ToString();
                    }
                }
                else
                {
                    dtlOther.Visible = false;
                    lblothermsg.Visible = true;
                    lblothermsg.Text = response.ReasonPhrase.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public int getList()
    {
        string sCategoryId = "";

        int count = dtlOther.Items.Count;
        for (int i = 0; i < count; i++)
        {
            Label CategoryId = (Label)(dtlOther.Items[i].FindControl("lblOthCatId"));
            sCategoryId += CategoryId.Text + ",";
        }
        sCategoryId = sCategoryId.Trim(',');
        ViewState["CategoryId"] = sCategoryId;

        return 1;
    }

    public int getStockStatus()
    {
       

        int count = dtlOtherChild.Items.Count;
        for (int i = 0; i < count; i++)
        {
            Label StockStatus = (Label)(dtlOtherChild.Items[i].FindControl("lblStockEntryMaintenance"));          
          
            if (StockStatus.Text == "N")
            {
                Label Qty = (Label)(dtlOtherChild.Items[i].FindControl("lblAvailableQty"));
                Label Qtys = (Label)(dtlOtherChild.Items[i].FindControl("qty"));
                
                Qty.Visible = false;
                Qtys.Visible = false;


            }

           
        }
        //sCategoryId = sCategoryId.Trim(',');
        //ViewState["CategoryId"] = sCategoryId;

        return 1;
    }

    public void BindOtherServiceNameList()//SERVICE NAME LIST
    {
        try
        {
            if (getList() == 1)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string boatTypeIds = string.Empty;

                    var service = new Otherservices()
                    {
                        Category = "0",
                        BoatHouseId = Session["BoatHouseId"].ToString()
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("RestaurantSvcCatDet", service).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var BoatLst = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dt.Rows.Count > 0)
                            {
                                dtlOtherChild.DataSource = dt;
                                dtlOtherChild.DataBind();
                                dtlOtherChild.Visible = true;
                                lblotherchildmsg.Visible = false;
                            }
                            else
                            {
                                dtlOtherChild.Visible = false;
                                lblotherchildmsg.Visible = true;
                                lblotherchildmsg.Text = "No Records Found.";
                            }
                        }
                        else
                        {
                            dtlOtherChild.Visible = false;
                            lblotherchildmsg.Visible = true;
                            lblotherchildmsg.Text = ResponseMsg.ToString();
                        }
                    }
                    else
                    {
                        dtlOtherChild.Visible = false;
                        lblotherchildmsg.Visible = true;
                        lblotherchildmsg.Text = response.ReasonPhrase.ToString();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
        }
    }

    public void GetPaymentType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLPayType").Result;

                if (response.IsSuccessStatusCode)
                {
                    var PayType = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(PayType)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(PayType)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ddlPaymentType.DataSource = dt;
                            ddlPaymentType.DataValueField = "ConfigId";
                            ddlPaymentType.DataTextField = "ConfigName";
                            ddlPaymentType.DataBind();

                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Online"));
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("UPI"));
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Payment Type Details Not Found...!');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    private void OtherBookingFinal()
    {
        ViewState["OthServiceId"] = "";
        ViewState["OthChargePerItem"] = "";
        ViewState["OthNoOfItems"] = "";
        ViewState["OthTaxDetails"] = "";
        ViewState["OthNetAmount"] = "";

        try
        {
            foreach (GridViewRow item in gvOther.Rows)
            {
                string NoOfChild = string.Empty;
                string ChargePerItemTax = string.Empty;
                string ChargePerItem = string.Empty;
                string AdultCount = string.Empty;
                string Status = string.Empty;
                string ServiceId = gvOther.DataKeys[item.RowIndex]["ServiceId"].ToString().Trim();
                ViewState["OthServiceId"] += ServiceId.Trim() + '~';
                Status = gvOther.DataKeys[item.RowIndex]["ServiceId"].ToString().Trim();
                ViewState["sStockStatus"] += Status.Trim();
                ChargePerItem = gvOther.DataKeys[item.RowIndex]["ChargePerItem"].ToString().Trim();
                ViewState["OthChargePerItem"] += ChargePerItem.Trim() + '~';

                AdultCount = gvOther.DataKeys[item.RowIndex]["AdultCount"].ToString().Trim();
                ViewState["OthNoOfItems"] += AdultCount.Trim() + '~';
                hfAdultCount.Value = ViewState["OthNoOfItems"].ToString().Trim();
                ChargePerItemTax = gvOther.DataKeys[item.RowIndex]["ChargePerItemTax"].ToString().Trim();

                //------- Tax ------------//   
                decimal Totalcharge = (Convert.ToDecimal(ChargePerItem) * Convert.ToDecimal(AdultCount));
                string lblTax = gvOther.DataKeys[item.RowIndex]["TaxName"].ToString().Trim();

                decimal OtherTaxAmt = Convert.ToDecimal(ChargePerItemTax) * Convert.ToDecimal(AdultCount);

                decimal TotalTaxAmt = 0;
                string TaxDtl = string.Empty;

                if (lblTax != "")
                {
                    string[] taxlist = lblTax.Split(',');

                    foreach (var list in taxlist)
                    {
                        var TaxName = list;
                        var tx = list.Split('-');
                        decimal taxper = Convert.ToDecimal(tx[1].ToString());

                        decimal TaxAmt = ((OtherTaxAmt) / 2);
                        TaxAmt = Math.Round(TaxAmt, 2);

                        TaxDtl += (TaxName + "#" + (TaxAmt).ToString() + ",").Trim();
                        TotalTaxAmt = TotalTaxAmt + TaxAmt;
                    }
                }

                ViewState["OthTaxDetails"] += TaxDtl + '~';
                decimal OtherTotalAmount = Totalcharge + TotalTaxAmt;
                ViewState["OthNetAmount"] += OtherTotalAmount.ToString() + '~';
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void btnOtherBooking_Click(object sender, EventArgs e)
    {
        string SCustomerMobileNo = string.Empty;
        if (chkCustMobileNo.Checked == true)
        {

            if (txtCustMobileNo.Text == "" || txtCustMobileNo.Text == "Null")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Enter Mobile No !');", true);
                return;
            }
           
            else if (ViewState["CustomerMobileNo"].ToString().Trim() == "" || ViewState["CustomerMobileNo"].ToString().Trim() == "NULL")
             
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Enter 10 Digit Mobile No !');", true);
                return;
                
            }
            else
            {
                SCustomerMobileNo = ViewState["CustomerMobileNo"].ToString().Trim();
            }
        }
        else if (chkCustMobileNo.Checked == false)
        {
            SCustomerMobileNo = "";
        }

        OtherBookingFinal();
        string NoItems = string.Empty;
        string Available = string.Empty;
        NoItems = ViewState["OthNoOfItems"].ToString().Trim('~');
        Available = hfAvailableQty.Value;

        
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response;
                    string sMSG = string.Empty;
                    string sCustMsg = string.Empty;


                var Bookingotherservices = new Bookingotherservices()
                    {
                        QueryType = "Insert",
                        BookingId = "0",
                        ServiceId = ViewState["OthServiceId"].ToString().Trim('~'),
                        BookingType = "R",
                        BoatHouseId = Session["BoatHouseId"].ToString(),

                        BoatHouseName = Session["BoatHouseName"].ToString(),
                        BookingDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"),
                        

                        ChargePerItem = ViewState["OthChargePerItem"].ToString().Trim('~'),
                        NoOfItems = ViewState["OthNoOfItems"].ToString().Trim('~'),
                        TaxDetails = ViewState["OthTaxDetails"].ToString().Trim('~'),
                        NetAmount = ViewState["OthNetAmount"].ToString().Trim('~'),

                        CustomerMobileNo = SCustomerMobileNo.Trim(),
                        Createdby = Session["UserId"].ToString(),
                        BookingMedia = "DW",
                        PaymentType = ddlPaymentType.SelectedValue.Trim()
                    };
                    response = client.PostAsJsonAsync("BookingRestaurantServices", Bookingotherservices).Result;

                    sMSG = "Restaurant Services Details Inserted Successfully";

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            string[] sResult = ResponseMsg.Split('~');
                        if(chkCustMobileNo.Checked == true)

                        {
                            sCustMsg= "e-Receipt Link Send to Customer Mobile !";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('"+ sCustMsg + "');", true);
                            ClearBooking();
                            getStockStatus();
                        }
                        else 
                        {
                            //GetOtherTickets(sResult[1].ToString());
                            //ClearBooking();
                            Response.Redirect("~/Boating/Print.aspx?rt=r&bi=" + sResult[1].ToString() + "");
                        }
                        }
                        else
                        {
                            ClearBooking();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        }
                    }
                    else
                    {
                        ClearBooking();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            }
        }
    //    else
    //    {
    //        getStockStatus();
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Stock Is Not Available');", true);

    //        ClearBooking();
    //        return;
    //    }
    //}

    protected void imgbtnNewBook_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divShow.Visible = false;
            imgbtnNewBook.Visible = false;
            imgbtnBookedList.Visible = true;
            txtSearch.Text = string.Empty;
            BackToList.Visible = false;
            back.Visible = true;
            Next.Visible = true;
            back.Enabled = false;
            backSearch.Visible = false;
            NextSearch.Visible = false;
            divGridList.Visible = false;
            ClearBooking();
            BindOtherCategoryList();
            BindOtherServiceNameList();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void imgCloseTicket_Click(object sender, ImageClickEventArgs e)
    {
        MpeBillService.Hide();
        ClearBooking();
    }

    protected void dtlistTicketOther_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label BookingIds = (Label)e.Item.FindControl("lblBillOBookingId");
                Label ServiceIds = (Label)e.Item.FindControl("lblBillOServiceId");
                DataRowView dr = (DataRowView)e.Item.DataItem;
                Image ImageData = (Image)e.Item.FindControl("imgOtherQR");
                Image ImageData1 = (Image)e.Item.FindControl("imgOtherQR1");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    var QRd = new QR()
                    {
                        ServiceId = ServiceIds.Text.Trim(),
                        BookingId = BookingIds.Text.Trim(),
                        BookingType = "R"
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("BookingOthersQR", QRd).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                        if (Status == "Success.")
                        {
                            ImageData.Visible = true;
                            //ImageData.ImageUrl = @"ImgPost/" + dr.Row["ImgPath"].ToString();
                            ImageData.ImageUrl = ResponseMsg;
                            ImageData1.ImageUrl = ResponseMsg;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                    }
                }

                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblBookingId = (Label)e.Item.FindControl("lblBookingId");

                var BoatServiceId = e.Item.FindControl("dtlisTicketInsOther") as DataList;

                try
                {
                    Control trBoatIns = e.Item.FindControl("trInsOther") as Control;
                    trBoatIns.Visible = false;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var service = new OtherBook()
                        {
                            ServiceType = "3",// Default 1 is Boat boking//
                            BoatHouseId = Session["BoatHouseId"].ToString()
                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("PrintInstrucSvc", service).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var BoatLst = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                                if (dt.Rows.Count > 0)
                                {
                                    BoatServiceId.DataSource = dt;
                                    BoatServiceId.DataBind();

                                    trBoatIns.Visible = true;
                                }
                                else
                                {
                                    BoatServiceId.DataBind();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void imgbtnBookedList_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        back.Visible = true;
        Next.Visible = true;
        back.Enabled = false;
        AddProcess(0, 10, out istart, out iend);
        BookedList();
    }
    public void BookedList()
    {
        try
        {
            ClearBooking();

            divShow.Visible = true;
            imgbtnNewBook.Visible = true;
            imgbtnBookedList.Visible = false;

            divGridList.Visible = true;
            divEntry.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;

                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    GvBoatBooking.Columns[9].Visible = true;

                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "Admin",
                        CountStart = ViewState["hfstartvalue"].ToString().Trim()


                    };
                    response = client.PostAsJsonAsync("RestaurantBookedListV2", OtherBook).Result;
                }
                else
                {
                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = ViewState["hfstartvalue"].ToString().Trim()

                    };
                    response = client.PostAsJsonAsync("RestaurantBookedListV2", OtherBook).Result;
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
                            if (dt.Rows.Count < 10)
                            {
                                Next.Enabled = false;
                                NextSearch.Enabled = false;

                            }
                            else
                            {
                                Next.Enabled = true;
                                NextSearch.Enabled = true;
                                backSearch.Enabled = false;

                            }
                            GvBoatBooking.DataSource = dt;
                            GvBoatBooking.DataBind();
                            GvBoatBooking.Visible = true;
                            Search.Visible = true;
                            //Newly added by Brijin and Imran on 2022-05-24
                            if (UserRole == "Admin")
                            {
                                GvBoatBooking.Columns[9].Visible = true;
                            }
                            else
                            {
                                GvBoatBooking.Columns[9].Visible = false;
                            }

                        }
                        else
                        {
                            lblGridMsg.Text = "No Record Found...!";
                            GvBoatBooking.Visible = false;
                            Next.Enabled = false;
                            Search.Visible = false;

                        }
                    }
                    else
                    {
                        if (ResponseMsg == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                        
                        GvBoatBooking.Visible = false;
                        Next.Enabled = false;
                        Search.Visible = false;

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string BookingId = GvBoatBooking.DataKeys[gvrow.RowIndex].Value.ToString();
            //GetOtherTickets(BookingId);
            ViewState["BoOkingID"] = BookingId.ToString().Trim();
            string Status = GvBoatBooking.DataKeys[gvrow.RowIndex]["Status"].ToString().Trim();
            if (Session["UserRole"].ToString() == "Admin" || Session["UserRole"].ToString() == "Sadmin")
            {
                Mpepnlrsn.Show();
            }
            else
            {

                if (Status == "Y")
                {
                    Mpepnlrsn.Show();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Time Out, Cannot Print Details');", true);
                    return;
                }
            }



        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    private void GetOtherTickets(string sBookingId)
    {
        try
        {
            MpeBillService.Show();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = sBookingId.ToString(),
                    BookingType = "R"
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RestaurantTicket", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Oticket = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Oticket)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Oticket)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlistTicketOther.DataSource = dt;
                            dtlistTicketOther.DataBind();
                            dtlistTicketOther.Visible = true;
                        }
                        else
                        {
                            dtlistTicketOther.Visible = false;
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    private void ClearBooking()
    {
        try
        {
            ddlPaymentType.SelectedIndex = 0;

            ViewState["OthServiceId"] = "";
            ViewState["OthChargePerItem"] = "";
            ViewState["OthNoOfItems"] = "";
            ViewState["OthNetAmount"] = "";
            ViewState["OthTaxDetails"] = "";

            oschar1.InnerText = "";
            bsgst1.InnerText = "";

            ViewState["CartRowO"] = null;
            ViewState["RowO"] = null;

            gvOther.Visible = false;

            chkCustMobileNo.Enabled = true;
            chkCustMobileNo.Checked = false;
            txtCustMobileNo.Text = "";
            divCustMobile.Visible = false;

            divpaymentType.Visible = false;
            btnOtherBooking.Text = "";

            BindOtherCategoryList();
            BindOtherServiceNameList();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //chkCustMobileNo.Enabled = true;
            ClearBooking();
            getStockStatus();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }


    // ***** Other Services ***** //

    protected void ImgBtnDeleteOther_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sUniqueId = gvOther.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();

            DataTable dtCurrentTableO = (DataTable)ViewState["CartRowO"];

            for (int i = dtCurrentTableO.Rows.Count - 1; i >= 0; i--)
            {
                DataRow drO = dtCurrentTableO.Rows[i];
                if (drO["UniqueId"].ToString().Trim() == sUniqueId.Trim())
                {
                    drO.Delete();
                }
            }

            dtCurrentTableO.AcceptChanges();

            DataTable dtCurrentTable1O = (DataTable)ViewState["RowO"];

            for (int i = dtCurrentTable1O.Rows.Count - 1; i >= 0; i--)
            {
                DataRow drO = dtCurrentTable1O.Rows[i];
                if (drO["UniqueId"].ToString().Trim() == sUniqueId.Trim())
                {
                    drO.Delete();
                }
            }

            dtCurrentTableO.AcceptChanges();
            dtCurrentTable1O.AcceptChanges();

            if (dtCurrentTableO.Rows.Count > 0)
            {
                divpaymentType.Visible = true;

                gvOther.Visible = true;
                gvOther.DataSource = dtCurrentTableO;
                gvOther.DataBind();

                DataTable dtTableO = (DataTable)ViewState["RowO"];

                ViewState["OtherChargeSum"] = "0";
                ViewState["OtherTaxSum"] = "0";

                decimal dServiceTotalAmount = dtTableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItem")));
                ViewState["OtherChargeSum"] = (Convert.ToDecimal(dServiceTotalAmount)).ToString();

                decimal dChargePerItemTax = dtTableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItemTax")));

                ViewState["OtherTaxSum"] = (Convert.ToDecimal(dChargePerItemTax)).ToString();


            }
            else
            {
                ViewState["OtherChargeSum"] = "0";
                ViewState["OtherTaxSum"] = "0";
                chkCustMobileNo.Checked = false;
                divCustMobile.Visible = false;
                divpaymentType.Visible = false;
                ViewState["CartRowO"] = null;
                ViewState["RowO"] = null;
                gvOther.Visible = false;
            }

            CalculateSummary();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    private void CalculateSummary()
    {
      
        oschar1.InnerText = ViewState["OtherChargeSum"].ToString();
        bsgst1.InnerText = ViewState["OtherTaxSum"].ToString();
        btnOtherBooking.Text = (Convert.ToDecimal(oschar1.InnerText) + Convert.ToDecimal(bsgst1.InnerText)).ToString();
        if (Convert.ToDecimal(btnOtherBooking.Text.Trim()) >= 1)
        {

            chkCustMobileNo.Enabled = false;

            if (chkCustMobileNo.Checked == true)
            {
                divCustMobile.Visible = true;
                ViewState["MobileNo"] = txtCustMobileNo.Text.Trim();

            }
            else
            {

                divCustMobile.Visible = false;
            }
        }
        else
        {

            oschar1.InnerText = "";
            bsgst1.InnerText = "";

            chkCustMobileNo.Enabled = true;
            divCustMobile.Visible = false;
       
        }
        //chkCustMobileNo.Enabled = false;
        //if (chkCustMobileNo.Checked == true)
        //{
        //    divCustMobile.Visible = true;
        //    ViewState["MobileNo"] = txtCustMobileNo.Text.Trim();

        //}
    }

    protected void DtlOther_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.DataItem != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblOthCatId = (Label)e.Item.FindControl("lblOthCatId");
                Label lblOthCatName = (Label)e.Item.FindControl("lblOthCatName");

                //var OthServiceId = e.Item.FindControl("dtlOtherChild") as DataList;

                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var service = new Otherservices()
                        {
                            Category = lblOthCatId.Text.Trim(),
                            BoatHouseId = Session["BoatHouseId"].ToString()
                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("RestaurantSvcCatDet", service).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var BoatLst = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                                if (dt.Rows.Count > 0)
                                {
                                    dtlOtherChild.DataSource = dt;
                                    dtlOtherChild.DataBind();
                                }
                                else
                                {
                                    dtlOtherChild.DataBind();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Details Not Found !');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert(" + ResponseMsg + "');", true);
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
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void dtlOtherChild_ItemCommand(object source, DataListCommandEventArgs e)
    {
        string CategoryName = string.Empty;
        string ServiceId = string.Empty;
        string ServiceName = string.Empty;
        string ChargePerItem = string.Empty;

        string ServiceTotalAmount = string.Empty;
        string ChargePerItemTax = string.Empty;
        string TaxId = string.Empty;
        string TaxName = string.Empty;
        string AvailableQty = string.Empty;
        string Stock = string.Empty;
        string AdultCount = string.Empty;

        Label lblCategoryName = (Label)e.Item.FindControl("lblCategoryName");
        CategoryName = lblCategoryName.Text;

        Label lblServiceId = (Label)e.Item.FindControl("lblOthServiceId");
        ServiceId = lblServiceId.Text;

        Label lblServiceName = (Label)e.Item.FindControl("lblOthServiceName");
        ServiceName = lblServiceName.Text;

        Label lblServiceTotalAmount = (Label)e.Item.FindControl("lblServiceTotalAmount");
        ServiceTotalAmount = lblServiceTotalAmount.Text;

        Label lblChargePerItem = (Label)e.Item.FindControl("lblChargePerItem");
        ChargePerItem = lblChargePerItem.Text;

        Label lblChargePerItemTax = (Label)e.Item.FindControl("lblChargePerItemTax");
        ChargePerItemTax = lblChargePerItemTax.Text;

        Label lblTaxId = (Label)e.Item.FindControl("lblTaxId");
        TaxId = lblTaxId.Text;

        Label lblTaxName = (Label)e.Item.FindControl("lblTaxName");
        TaxName = lblTaxName.Text;

        Label lblAvailableQty = (Label)e.Item.FindControl("lblAvailableQty");
        AvailableQty = lblAvailableQty.Text;


        Label lblStockEntryMaintenance = (Label)e.Item.FindControl("lblStockEntryMaintenance");
        Stock = lblStockEntryMaintenance.Text;

        AdultCount = "1";
       
    }

    private void BindDataDynamicValueOthers(string ServiceId, string ServiceName, string ServiceTotalAmount, string ChargePerItem,
        string ChargePerItemTax, string AdultCount, string TaxId, string TaxName,string AvailableQty,string Stock)
    {
        
        DataTable mytableO = new DataTable();

        if (ViewState["RowO"] != null)
        {
            mytableO = (DataTable)ViewState["RowO"];
            DataRow dr = null;

            DataRow[] fndUniqueId = mytableO.Select("UniqueId = '" + ServiceId.Trim() + "'");


            if (mytableO.Rows.Count > 0)
            {
                dr = mytableO.NewRow();

                dr["UniqueId"] = ServiceId.Trim();
                dr["ServiceId"] = ServiceId.Trim();
                dr["ServiceName"] = ServiceName.Trim();

                dr["ServiceTotalAmount"] = ServiceTotalAmount.Trim();
                dr["ChargePerItem"] = ChargePerItem.Trim();
                dr["ChargePerItemTax"] = ChargePerItemTax.Trim();
                dr["AdultCount"] = AdultCount;

                dr["TaxId"] = TaxId;
                dr["TaxName"] = TaxName.Trim();
                dr["AvailableQty"] = AvailableQty.Trim();
                dr["StockEntryMaintenance"] = Stock.Trim();
                dr["OtherGrandTotalAmount"] = 0;

                mytableO.Rows.Add(dr);

                ViewState["RowO"] = mytableO;

            }
        }
        else
        {
            mytableO.Columns.Add(new DataColumn("UniqueId", typeof(string)));
            mytableO.Columns.Add(new DataColumn("ServiceId", typeof(string)));
            mytableO.Columns.Add(new DataColumn("ServiceName", typeof(string)));

            mytableO.Columns.Add(new DataColumn("ServiceTotalAmount", typeof(decimal)));
            mytableO.Columns.Add(new DataColumn("ChargePerItem", typeof(decimal)));
            mytableO.Columns.Add(new DataColumn("ChargePerItemTax", typeof(decimal)));
            mytableO.Columns.Add(new DataColumn("AdultCount", typeof(Int32)));

            mytableO.Columns.Add(new DataColumn("TaxId", typeof(string)));
            mytableO.Columns.Add(new DataColumn("TaxName", typeof(string)));

            mytableO.Columns.Add(new DataColumn("OtherGrandTotalAmount", typeof(decimal)));
            mytableO.Columns.Add(new DataColumn("AvailableQty", typeof(Int32)));
            mytableO.Columns.Add(new DataColumn("StockEntryMaintenance", typeof(string)));

            DataRow dr1 = mytableO.NewRow();
            dr1 = mytableO.NewRow();

            dr1["UniqueId"] = ServiceId.Trim();
            dr1["ServiceId"] = ServiceId.Trim();
            dr1["ServiceName"] = ServiceName.Trim();

            dr1["ServiceTotalAmount"] = ServiceTotalAmount.Trim();
            dr1["ChargePerItem"] = ChargePerItem.Trim();
            dr1["ChargePerItemTax"] = ChargePerItemTax.Trim();
            dr1["AdultCount"] = AdultCount;

            dr1["TaxId"] = TaxId;
            dr1["TaxName"] = TaxName.Trim();

            dr1["OtherGrandTotalAmount"] = 0;
            dr1["AvailableQty"] = AvailableQty.Trim();
            dr1["StockEntryMaintenance"] = Stock.Trim();
            mytableO.Rows.Add(dr1);

            ViewState["RowO"] = mytableO;
        }
       
        if (Stock.Trim() == "Y")
        {
        
                if (mytableO.Rows.Count > 0)
                {

                    ViewState["OtherChargeSum"] = "0";
                    ViewState["OtherTaxSum"] = "0";

                    decimal dChargePerItem = mytableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItem")));
                    decimal dChargePerItemTax = mytableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItemTax")));

                    ViewState["OtherChargeSum"] = (Convert.ToDecimal(dChargePerItem)).ToString();
                    ViewState["OtherTaxSum"] = (Convert.ToDecimal(dChargePerItemTax)).ToString();

                    CalculateSummary();
                }
           
         
                DataTable dtsM = mytableO.Clone();

            var CartTableM = (from row in mytableO.AsEnumerable()
                              group row by new
                              {
                                  UniqueId = row.Field<string>("UniqueId"),
                                  ServiceId = row.Field<string>("ServiceId"),
                                  ServiceName = row.Field<string>("ServiceName"),

                                  //ServiceTotalAmount = row.Field<decimal>("ServiceTotalAmount"),
                                  ChargePerItem = row.Field<decimal>("ChargePerItem"),
                                  ChargePerItemTax = row.Field<decimal>("ChargePerItemTax"),
                                  //AdultCount = row.Field<Int32>("AdultCount"),                            

                                  TaxId = row.Field<string>("TaxId"),
                                  TaxName = row.Field<string>("TaxName"),

                                  // OtherGrandTotalAmount = row.Field<decimal>("OtherGrandTotalAmount")

                              } into t1 
                                  select new
                                  {
                                      UniqueId = t1.Key.UniqueId,
                                      ServiceId = t1.Key.ServiceId,
                                      ServiceName = t1.Key.ServiceName,

                                      //ServiceTotalAmount = t1.Key.ServiceTotalAmount,
                                      ChargePerItem = t1.Key.ChargePerItem,
                                      ChargePerItemTax = t1.Key.ChargePerItemTax,
                                      AdultCount = t1.Sum(a => a.Field<Int32>("AdultCount")),

                                      TaxId = t1.Key.TaxId,
                                      TaxName = t1.Key.TaxName,

                                      OtherGrandTotalAmount = t1.Sum(a => a.Field<decimal>("ServiceTotalAmount")),

                                  })
                     .Select(g =>
                     {
                         var h = dtsM.NewRow();
                         h["UniqueId"] = g.UniqueId;
                         h["ServiceId"] = g.ServiceId;
                         h["ServiceName"] = g.ServiceName;

                 //h["ServiceTotalAmount"] = g.ServiceTotalAmount;
                 h["ChargePerItem"] = g.ChargePerItem;
                         h["ChargePerItemTax"] = g.ChargePerItemTax;

                         h["AdultCount"] = g.AdultCount;

                         h["OtherGrandTotalAmount"] = g.OtherGrandTotalAmount;

                         h["TaxId"] = g.TaxId;
                         h["TaxName"] = g.TaxName;
                         return h;
                     } ).CopyToDataTable();
            DataRow[] filterData1 = CartTableM.Select("ServiceId='"+ServiceId.Trim()+"'");
            int sQTY = 0;
            foreach (DataRow item in filterData1)
            {
                sQTY = item.Field<int>("AdultCount");
                //"Roll No : " + item.Field<int>("RollNo") + ", Name : " + item.Field<string>("Name") + ", Address : " + item.Field<string>("Address"));
            }
          
            ViewState["sQTY"] = sQTY;


            if (Convert.ToInt32(ViewState["sQTY"].ToString().Trim()) <= Convert.ToInt32(AvailableQty.Trim()))
                
            {
                if (CartTableM.Rows.Count > 0)
                {
                    ViewState["CartRowO"] = CartTableM;
                    gvOther.Visible = true;
                    gvOther.DataSource = CartTableM;
                    gvOther.DataBind();
                    divpaymentType.Visible = true;
                    

                }
                else
                {
                    divpaymentType.Visible = false;
                    gvOther.Visible = false;
                    gvOther.DataBind();
                }
                
            }
            else
            {
                
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Stock Is Not Available');", true);
                ClearBooking();
                getStockStatus();
                return;
            }

        }

        if (mytableO.Rows.Count > 0)
        {

            ViewState["OtherChargeSum"] = "0";
            ViewState["OtherTaxSum"] = "0";

            decimal dChargePerItem = mytableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItem")));
            decimal dChargePerItemTax = mytableO.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("ChargePerItemTax")));

            ViewState["OtherChargeSum"] = (Convert.ToDecimal(dChargePerItem)).ToString();
            ViewState["OtherTaxSum"] = (Convert.ToDecimal(dChargePerItemTax)).ToString();

            CalculateSummary();
        }

        DataTable dtsO = mytableO.Clone();

        var CartTableO = (from row in mytableO.AsEnumerable()
                          group row by new
                          {
                              UniqueId = row.Field<string>("UniqueId"),
                              ServiceId = row.Field<string>("ServiceId"),
                              ServiceName = row.Field<string>("ServiceName"),

                              //ServiceTotalAmount = row.Field<decimal>("ServiceTotalAmount"),
                              ChargePerItem = row.Field<decimal>("ChargePerItem"),
                              ChargePerItemTax = row.Field<decimal>("ChargePerItemTax"),
                              //AdultCount = row.Field<Int32>("AdultCount"),                            

                              TaxId = row.Field<string>("TaxId"),
                              TaxName = row.Field<string>("TaxName"),

                              // OtherGrandTotalAmount = row.Field<decimal>("OtherGrandTotalAmount")

                          } into t1
                          select new
                          {
                              UniqueId = t1.Key.UniqueId,
                              ServiceId = t1.Key.ServiceId,
                              ServiceName = t1.Key.ServiceName,

                              //ServiceTotalAmount = t1.Key.ServiceTotalAmount,
                              ChargePerItem = t1.Key.ChargePerItem,
                              ChargePerItemTax = t1.Key.ChargePerItemTax,
                              AdultCount = t1.Sum(a => a.Field<Int32>("AdultCount")),

                              TaxId = t1.Key.TaxId,
                              TaxName = t1.Key.TaxName,

                              OtherGrandTotalAmount = t1.Sum(a => a.Field<decimal>("ServiceTotalAmount")),

                          })
             .Select(g =>
             {
                 var h = dtsO.NewRow();
                 h["UniqueId"] = g.UniqueId;
                 h["ServiceId"] = g.ServiceId;
                 h["ServiceName"] = g.ServiceName;

                 //h["ServiceTotalAmount"] = g.ServiceTotalAmount;
                 h["ChargePerItem"] = g.ChargePerItem;
                 h["ChargePerItemTax"] = g.ChargePerItemTax;

                 h["AdultCount"] = g.AdultCount;

                 h["OtherGrandTotalAmount"] = g.OtherGrandTotalAmount;

                 h["TaxId"] = g.TaxId;
                 h["TaxName"] = g.TaxName;
                 return h;
             }).CopyToDataTable();


        if (CartTableO.Rows.Count > 0)
        {
            ViewState["CartRowO"] = CartTableO;

            gvOther.Visible = true;
            gvOther.DataSource = CartTableO;
            gvOther.DataBind();

            divpaymentType.Visible = true;
        }
        else
        {
            divpaymentType.Visible = false;
            gvOther.Visible = false;
            gvOther.DataBind();
        }
    }

    public void BindResCountAmount()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sCreatedBy = string.Empty;

                if (Session["UserRole"].ToString() == "SAdmin" || Session["UserRole"].ToString() == "ADMIN"
                    || Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString() == "Admin")
                {
                    sCreatedBy = "0";
                }
                else
                {
                    sCreatedBy = Session["UserId"].ToString().Trim();
                }

                var BoatSearch = new BoatSearch()
                {
                    CreatedBy = sCreatedBy,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = DateTime.Today.ToString("dd/MM/yyyy")
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RestaurantDetailsCount", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        bblblCount.Text = dtExists.Rows[0]["BookingCount"].ToString();

                        bblblNetAmount.Text = dtExists.Rows[0]["NetAmount"].ToString();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindRestaurantBookedList()
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            string UserRole = Session["UserRole"].ToString().Trim();
            HttpResponseMessage response;

            if (UserRole == "Sadmin" || UserRole == "Admin")
            {
                var OtherBook = new OtherBook()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    UserId = "Admin"
                };
                response = client.PostAsJsonAsync("RestaurantBookedList", OtherBook).Result;
            }
            else
            {
                var OtherBook = new OtherBook()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    UserId = Session["UserId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("RestaurantBookedList", OtherBook).Result;
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
                        gvCountTotal.DataSource = dt;
                        gvCountTotal.DataBind();

                        decimal dServiceFare = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ServiceFare")));
                        decimal dTaxAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("TaxAmount")));
                        decimal dNetAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("NetAmount")));

                        gvCountTotal.FooterRow.Cells[3].Text = "Total";
                        gvCountTotal.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                        gvCountTotal.FooterRow.Cells[6].Text = dServiceFare.ToString();
                        gvCountTotal.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                        gvCountTotal.FooterRow.Cells[7].Text = dTaxAmount.ToString();
                        gvCountTotal.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                        gvCountTotal.FooterRow.Cells[8].Text = dNetAmount.ToString();
                        gvCountTotal.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;

                        gvCountTotal.Visible = true;
                        MpeUserCount.Show();
                    }
                    else
                    {
                        MpeUserCount.Hide();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
            }
        }
    }

    protected void ImgClose_Click(object sender, ImageClickEventArgs e)
    {
        MpeUserCount.Hide();
    }

    protected void bblblCount_Click(object sender, EventArgs e)
    {
        BindRestaurantBookedList();
    }


    protected void gvCountTotal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCountTotal.PageIndex = e.NewPageIndex;
        BindRestaurantBookedList();
    }
    public void GetUserName()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatHouseId = new BoatSearch()
                {
                    UserId = Session["UserId"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("getUserName", BoatHouseId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatHouseresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatHouseresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatHouseresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            string UserName = dt.Rows[0]["UserName"].ToString();
                            ViewState["EmpName"] = UserName.ToString().Trim();

                        }

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


    protected void RsnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            if (ddlReason.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Select Reason');", true);
                return;
            }
            GetUserName();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var EmpMstr = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    UserId = Session["UserId"].ToString().Trim(),
                    UserName = ViewState["EmpName"].ToString().Trim(),
                    ServiceType = "Restaurant Services",
                    BookingId = ViewState["BoOkingID"].ToString().Trim(),
                    Reason = ddlReason.SelectedItem.Text,
                    CreatedBy = Session["UserId"].ToString().Trim()


                };


                HttpResponseMessage response = client.PostAsJsonAsync("ReprintReason", EmpMstr).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.Trim() + "');", true);
                        //  Response.Redirect("Print.aspx?rt=b&bi=" + ViewState["BoOkingID"].ToString().Trim() + "");
                        Mpepnlrsn.Hide();
                        Response.Redirect("~/Boating/Print.aspx?rt=r&bi=" + ViewState["BoOkingID"].ToString().Trim() + "");
                        ViewState["BoOkingID"] = null;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message + "');", true);
            return;
        }
    }

    protected void CloseRsnButton_Click(object sender, ImageClickEventArgs e)
    {
        Mpepnlrsn.Hide();
    }

    protected void ChkBulk_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkBulk.Checked == true)
        {
            Response.Redirect("BulkRestaurantService.aspx", true);

        }
        else
        {
            Response.Redirect("BulkRestaurantService.aspx", false);

        }
    }

    public class Otherservices
    {
        public string BoatHouseId { get; set; }
        public string Category { get; set; }

        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Dinner { get; set; }
    }

    public class OtherBook
    {
        public string BoatHouseId { get; set; }
        public string BookingType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceType { get; set; }
        public string UserId { get; set; }

        public string CountStart { get; set; }
        public string Input1 { get; set; }
      

    }

    public class Bookingotherservices
    {
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public string ServiceId { get; set; }
        public string BookingType { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BookingDate { get; set; }
        public string ChargePerItem { get; set; }
        public string NoOfItems { get; set; }
        public string TaxDetails { get; set; }
        public string NetAmount { get; set; }
        public string CustomerMobileNo { get; set; }
        public string Createdby { get; set; }
        public string Category { get; set; }
        public string BookingMedia { get; set; }
        public string PaymentType { get; set; }
    }

    public class BoatSearch
    {
        public string QueryType { get; set; }
        public string BoatHouseName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string BookingMedia { get; set; }
        public string NoOfPass { get; set; }
        public string NoOfChild { get; set; }
        public string NoOfInfant { get; set; }
        public string InitBillAmount { get; set; }
        public string PaymentType { get; set; }
        public string ActualBillAmount { get; set; }
        public string Status { get; set; }
        public string BoatSeaterId { get; set; }
        public string BookingDuration { get; set; }
        public string InitBoatCharge { get; set; }
        public string InitRowerCharge { get; set; }
        public string BoatDeposit { get; set; }
        public string TaxDetails { get; set; }
        public string InitOfferAmount { get; set; }
        public string InitNetAmount { get; set; }
        public string CreatedBy { get; set; }
        public string OthServiceStatus { get; set; }
        public string BoatHouseId { get; set; }

        public string PremiumStatus { get; set; }

        public string OfferId { get; set; }

        public string BoatTypeId { get; set; }

        public string BookingDate { get; set; }

        public string TaxId { get; set; }

        public string ValidDate { get; set; }

        public string BookingId { get; set; }

        public string BookingType { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string ServiceId { get; set; }

        public string OthServiceId { get; set; }

        public string OthChargePerItem { get; set; }

        public string OthNoOfItems { get; set; }

        public string OthTaxDetails { get; set; }

        public string OthNetAmount { get; set; }

        public string ServiceType { get; set; }
        public string UserName { get; set; }
        public string Reason { get; set; }
        public string CreatedDate { get; set; }
        public string UserId{ get; set; }


    }

    public class QRBoat
    {
        public int BookingId { get; set; }
        public int BoatTypeId { get; set; }
        public int BoatSeaterid { get; set; }
        public int BookingSerial { get; set; }
        public int CustomerID { get; set; }
    }

    public class QR
    {
        public string ServiceId { get; set; }
        public string BookingId { get; set; }
        public string BookingType { get; set; }
    }

    protected void btnGet_Click(object sender, EventArgs e)

    {
       
        if (hfAvailableQty.Value == "0" && hfStock.Value == "Y")
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Stock Is Not Available');", true);
        }
        else
        {

            BindDataDynamicValueOthers(hfServiceId.Value, hfCategoryName.Value + " - " + hfServiceName.Value, hfServiceTotalAmount.Value, hfChargePerItem.Value, hfChargePerItemTax.Value, hfAdultCount.Value, hfTaxId.Value, hfTaxName.Value, hfAvailableQty.Value, hfStock.Value);
        }
    }

    public void BindOtherServiceNameList(string CategoryId)//SERVICE NAME LIST
    {
        try
        {
            if (getList() == 1)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string boatTypeIds = string.Empty;

                    var service = new Otherservices()
                    {
                        Category = CategoryId.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString()
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("RestaurantSvcCatDet", service).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var BoatLst = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                            if (dt.Rows.Count > 0)
                            {
                                dtlOtherChild.DataSource = dt;
                                dtlOtherChild.DataBind();

                                dtlOtherChild.Visible = true;
                                lblotherchildmsg.Visible = false;
                                getStockStatus();
                            }
                            else
                            {
                                dtlOtherChild.Visible = false;
                                lblotherchildmsg.Visible = true;
                                lblotherchildmsg.Text = ResponseMsg.ToString();
                            }
                        }
                        else
                        {
                            dtlOtherChild.Visible = false;
                            lblotherchildmsg.Visible = true;
                            lblotherchildmsg.Text = ResponseMsg.ToString();
                        }
                    }
                    else
                    {
                        dtlOtherChild.Visible = false;
                        lblotherchildmsg.Visible = true;
                        lblotherchildmsg.Text = response.ReasonPhrase.ToString();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
        }
    }

    protected void btnBind_Click(object sender, EventArgs e)
    {
        BindOtherServiceNameList(hfBindCategoryId.Value);
        getStockStatus();

    }

    public void BindRestaurantRevenuePopup()
    {
        try
        {
            if (bblblNetAmount.Text != "0")
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string sCreatedBy = string.Empty;
                    string sQueryType = string.Empty;

                    if (Session["UserRole"].ToString() == "SAdmin" || Session["UserRole"].ToString() == "ADMIN" 
                        || Session["UserRole"].ToString() == "Sadmin" || Session["UserRole"].ToString() == "Admin")
                    {
                        sCreatedBy = "0";
                        sQueryType = "RestaurantRevenue";
                    }
                    else
                    {
                        sCreatedBy = Session["UserId"].ToString().Trim();
                        sQueryType = "RestaurantRevenueUser";
                    }

                    var FormBody = new BoatSearch()
                    {
                        QueryType = sQueryType,
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        UserId = sCreatedBy,
                        FromDate = DateTime.Today.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Today.ToString("dd/MM/yyyy")
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync("ViewBookingSummaryOtherRevenue", FormBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var GetResponse = response.Content.ReadAsStringAsync().Result;
                        var ResponseMsg = JObject.Parse(GetResponse)["Table"].ToString();
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvRESpopup.DataSource = dt;
                            gvRESpopup.DataBind();
                            gvRESpopup.Visible = true;

                            decimal totAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("RestaurantRevenue")));

                            gvRESpopup.FooterRow.Cells[1].Text = "Total";
                            gvRESpopup.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            gvRESpopup.FooterRow.Cells[2].Text = totAmt.ToString("0,0", CultureInfo.CreateSpecificCulture("hi-IN"));
                            gvRESpopup.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            MPERESpopup.Show();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.ReasonPhrase + "');", true);
                        MPERESpopup.Hide();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    protected void bblblNetAmount_Click(object sender, EventArgs e)
    {
        BindRestaurantRevenuePopup();
    }

    protected void ImgCloseRes_Click(object sender, ImageClickEventArgs e)
    {
        MPERESpopup.Hide();
    }

    protected void txtCustMobileNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtCustMobileNo.Text != "")
            {
                if (txtCustMobileNo.Text.Length == txtCustMobileNo.MaxLength)
                {
                   
                    ViewState["CustomerMobileNo"] = txtCustMobileNo.Text;
                    
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Enter 10 Digit Number!');", true);
                    ViewState["CustomerMobileNo"] = "NULL";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString().Trim()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) - 10, out istart, out iend);
       ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BookedList();

    }

    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BookedList();

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

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        back.Visible = false;
        Next.Visible = false;
        BackToList.Visible = true;
        ViewState["hfSearchstartvalue"] = "0";
        ViewState["hfSearchendvalue"] = "0";
        int istart;
        int iend;
        backSearch.Enabled = false;
        AddProcessSearch(0, 10, out istart, out iend);
        BindsearchBookedList();
        //back.Visible = false;
        //Next.Visible = false;
        //BackToList.Visible = true;
    }

    protected void BackToList_Click(object sender, EventArgs e)
    {
        //back.Visible = true;
        //Next.Visible = true;
        //BackToList.Visible = false;
        //Search.Visible = true;
        //txtSearch.Text = string.Empty;
        //BookedList();
        back.Visible = true;
        back.Enabled = false;

        Next.Visible = true;
        BackToList.Visible = false;
        Search.Visible = true;
        txtSearch.Text = string.Empty;
        backSearch.Visible = false;
        NextSearch.Visible = false;
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        BookedList();
    }

    public void BindsearchBookedList()
    {
        try
        {
            backSearch.Visible = true;
            NextSearch.Visible = true;
            ClearBooking();

            divShow.Visible = true;
            imgbtnNewBook.Visible = true;
            imgbtnBookedList.Visible = false;

            divGridList.Visible = true;
            divEntry.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;

                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    GvBoatBooking.Columns[9].Visible = true;

                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = "Admin",
                        Input1= txtSearch.Text.Trim(),
                        CountStart = ViewState["hfSearchstartvalue"].ToString().Trim()


                    };
                    response = client.PostAsJsonAsync("RestaurantBookedListPinV2", OtherBook).Result;
                }
                else
                {
                    var OtherBook = new OtherBook()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UserId = Session["UserId"].ToString().Trim(),
                        Input1 = txtSearch.Text.Trim(),
                        CountStart = ViewState["hfSearchstartvalue"].ToString().Trim()

                    };
                    response = client.PostAsJsonAsync("RestaurantBookedListPinV2", OtherBook).Result;
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
                            if (dt.Rows.Count < 10)
                            {
                                NextSearch.Enabled = false;

                            }
                            else
                            {
                                NextSearch.Enabled = true;
                                back.Enabled = false;
                            }
                                GvBoatBooking.DataSource = dt;
                            GvBoatBooking.DataBind();
                            GvBoatBooking.Visible = true;
                            Search.Visible = true;
                            backSearch.Visible = true;
                        }
                        else
                        {
                            lblGridMsg.Text = "No Record Found...!";
                            GvBoatBooking.Visible = false;
                            Search.Visible = false;
                            backSearch.Visible = false;
                            NextSearch.Visible = false;
                        }
                    }
                    else
                    {
                        if (ResponseMsg == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                           
                            backSearch.Visible = false;
                            NextSearch.Visible = false;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                        
                        GvBoatBooking.Visible = false;
                        txtSearch.Text = string.Empty;
                        Search.Visible = false;

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void backSearch_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcessSearch(Int32.Parse(ViewState["hfSearchstartvalue"].ToString().Trim()) - 10, Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) - 10, out istart, out iend);
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();

        BindsearchBookedList();
        if (istart == 1)
        {
            backSearch.Enabled = false;
        }
        else
        {
            backSearch.Enabled = true;
        }
    }

    protected void NextSearch_Click(object sender, EventArgs e)
    {
        backSearch.Enabled = true;
        int istart;
        int iend;
        AddProcessSearch(Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) + 1, Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) + 10, out istart, out iend);
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
        BindsearchBookedList();
    }
    protected void AddProcessSearch(int start, int end, out int istart, out int iend)
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
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
    }


    protected void subProcessSearch(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
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
        }
        else
        {
            iend = end;

        }
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
    }

   


}