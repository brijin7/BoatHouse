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

public partial class ScanTripSheetWeb : System.Web.UI.Page
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
				hfCreatedBy.Value = Session["UserId"].ToString();
				hfBoathouseId.Value = Session["BoatHouseId"].ToString();
				hfBoathouseName.Value = Session["BoatHouseName"].ToString();
				lblUserName.Text = Session["FirstName"].ToString().Trim() + "" + Session["LastName"].ToString().Trim();
				lblBoatHouse.Text = Session["BoatHouseName"].ToString();
				txtStartDetails.Focus();
				BindStartedTripList();
				BindEndedTripList();
				BindTripCountDetails();
				divTripStarted.Visible = true;
				divGridStart.Visible = true;
				divFinalStatus.Visible = false;
				divFinalEndStatus.Visible = false;
				divAlreadyEndStatus.Visible = false;
				GetExtensionPrint();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    protected void txtStartDetails_TextChanged(object sender, EventArgs e)
    {
        hfBarcodePinRowerId.Value = "";

        if (txtStartDetails.Text.Length != 0)
        {
            string BarCode = txtStartDetails.Text;
            string[] TimeList = BarCode.Split(';');

            if (TimeList.Count() == 4 && TimeList[0].Length > 1)
            {
                hfBarcodePin.Value = TimeList[3].ToString();
                hfBookingId.Value = TimeList[1].ToString();
                GenerateBarCode();
                return;
            }
            else if (TimeList[0].Length == 1 && TimeList.Count() == 4)
            {
                txtStartDetails.Text = "";
                txtStartDetails.Focus();
                MpeTrip.Show();
                lblStartResponse.Text = "First you have to Scan Ticket QR !!!";
                lblRowerResponse.Text = "";
                divAlreadyEndStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divFinalStatus.Visible = false;
                imgEnd.Visible = false;
                ImgBoatType.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideStartLabel();", true);
            }
            else
            {
                MpeTrip.Show();
                lblStartResponse.Text = "Invalid QRCode !!!";
                txtStartDetails.Text = "";
                txtStartDetails.Focus();
                lblRowerResponse.Text = "";
                divAlreadyEndStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divFinalStatus.Visible = false;
                imgEnd.Visible = false;
                ImgBoatType.Visible = false;
                ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideStartLabel();", true);

            }
        }
    }

    public void TripStart()
    {
        hfBarcodePinRowerId.Value = "";

        if (txtStartDetails.Text.Length != 0)
        {

            string BarCode = txtStartDetails.Text;
            string[] TimeList = BarCode.Split(';');

            if (TimeList.Count() == 4 && TimeList[0].Length > 1)
            {
                hfBarcodePin.Value = TimeList[3].ToString();
                GenerateBarCode();
                return;
            }
            else if (TimeList[0].Length == 1 && TimeList.Count() == 4)
            {
                txtStartDetails.Text = "";
                txtStartDetails.Focus();
                MpeTrip.Show();
                lblStartResponse.Text = "First you have to Scan Ticket QR !!!";
                lblRowerResponse.Text = "";
                divAlreadyEndStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divFinalStatus.Visible = false;
                imgEnd.Visible = false;
                ImgBoatType.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideStartLabel();", true);
            }
            else
            {
                MpeTrip.Show();
                lblStartResponse.Text = "Invalid QRCode !!!";
                txtStartDetails.Text = "";
                txtStartDetails.Focus();
                lblRowerResponse.Text = "";
                divAlreadyEndStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divFinalStatus.Visible = false;
                imgEnd.Visible = false;
                ImgBoatType.Visible = false;
                ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideStartLabel();", true);

            }
        }
    }

    protected void txtRowerDetails_TextChanged(object sender, EventArgs e)
    {
        hfBarcodePinRowerId.Value = "";

        if (txtRowerDetails.Text.Length != 0)
        {
            string BarCode = txtRowerDetails.Text;
            string[] TimeList = BarCode.Split(';');
            if (TimeList[0].Length == 1 && TimeList.Count() == 4)
            {
                ViewState["RowerBoatHouseId"] = TimeList[1].ToString();
                hfBarcodePinRowerId.Value = TimeList[2].ToString();
                hfBarcodePinRowerName.Value = TimeList[3].ToString();
                getRowerBoatAssign();
            }
            else if (TimeList[0].Length > 1 && TimeList.Count() == 4)
            {
                txtStartDetails.Focus();
                lblStartResponse.Text = "";
                txtStartDetails.Attributes.Add("style", "display:inline-block");
                txtRowerDetails.Attributes.Add("style", "display:none");
                divAlreadyEndStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divFinalStatus.Visible = false;
                imgEnd.Visible = false;
                ImgBoatType.Visible = false;
                txtStartDetails.Text = txtRowerDetails.Text.Trim();
                TripStart();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelRower();", true);
            }
            else
            {
                MpeTrip.Show();
                lblRowerResponse.Text = "Invalid QRCode !!!";
                txtRowerDetails.Text = "";
                txtRowerDetails.Focus();
                lblStartResponse.Text = "";
                divAlreadyEndStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divFinalStatus.Visible = false;
                imgEnd.Visible = false;
                ImgBoatType.Visible = false;
                ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelRower", "javascript:HideLabelRower();", true);
            }

        }
    }

    public void getRowerBoatAssign()
    {
        if (Session["BoatHouseId"].ToString().Trim() == ViewState["RowerBoatHouseId"].ToString().Trim())
        {

            DataTable dt = new DataTable();
            DataTable dtbl = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = hfBoatTypeId.Value.Trim(),
                    BoatSeaterId = hfBoatSeaterId.Value.Trim(),
                    RowerId = hfBarcodePinRowerId.Value.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/RowerBoatBasedAssign", BoatHouseId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                    if (statusCode == 1)
                    {
                        dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (hfBarcodePinRowerId.Value != "")
                        {
                            var BoatHouse = new TripSheetSettlement()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatTypeId = hfBoatTypeId.Value.Trim(),
                                BoatSeaterId = hfBoatSeaterId.Value.Trim()
                            };
                            HttpResponseMessage response1 = client.PostAsJsonAsync("TripSheet/RowerBoatAssign", BoatHouse).Result;
                            if (response1.IsSuccessStatusCode)
                            {
                                var Locresponse1 = response1.Content.ReadAsStringAsync().Result;
                                int statusCode1 = Convert.ToInt32(JObject.Parse(Locresponse1)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(Locresponse1)["Response"].ToString();

                                if (statusCode1 == 1)
                                {
                                    dtbl = JsonConvert.DeserializeObject<DataTable>(ResponseMsg1);
                                    for (int i = 0; i < dtbl.Rows.Count; i++)
                                    {
                                        string RowerId = dtbl.Rows[i]["RowerId"].ToString();
                                        if (RowerId == hfBarcodePinRowerId.Value)
                                        {
                                            txtRowerDetails.Text = "";
                                            txtRowerDetails.Attributes.Add("style", "display:none");
                                            txtStartDetails.Attributes.Add("style", "display:inline-block");
                                            lblStartResponse.Visible = false;
                                            lblRowerResponse.Visible = false;
                                            txtStartDetails.Focus();
                                            GenerateBarCode();
                                            return;
                                        }
                                        else
                                        {
                                            txtRowerDetails.Text = "";
                                            txtRowerDetails.Focus();
                                            imgEnd.Visible = false;
                                            ImgBoatType.Visible = false;
                                            MpeTrip.Show();
                                            lblRowerResponse.Text = "Rower is on travel, trip is not yet Ended !!!";
                                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelRower();", true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        txtRowerDetails.Text = "";
                        txtRowerDetails.Focus();
                        imgEnd.Visible = false;
                        ImgBoatType.Visible = false;
                        MpeTrip.Show();
                        lblRowerResponse.Text = "Rower not assigned to this BoatType";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelRower();", true);
                        return;
                    }

                }
            }
        }

        else
        {
            txtRowerDetails.Text = "";
            txtRowerDetails.Focus();
            imgEnd.Visible = false;
            ImgBoatType.Visible = false;
            MpeTrip.Show();
            lblRowerResponse.Text = "Invalid Rower";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelRower();", true);
        }

    }

    //public void GenerateBarCode()
    //{
    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //            HttpResponseMessage response;
    //            if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
    //            {
    //                var vTripSheetSettlement = new TripSheetSettlement()
    //                {
    //                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                    BarcodePin = hfBarcodePin.Value.Trim()
    //                };

    //                response = client.PostAsJsonAsync("TripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
    //            }
    //            else
    //            {
    //                var vTripSheetSettlement = new TripSheetSettlement()
    //                {
    //                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                    BarcodePin = hfBarcodePin.Value.Trim(),
    //                    UserId = Session["UserId"].ToString().Trim()
    //                };

    //                response = client.PostAsJsonAsync("NewTripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
    //            }
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
    //                    string TripStart = dt.Rows[0]["TripStartTime"].ToString();


    //                    //string SelfDrive = dt.Rows[0]["SelfDrive"].ToString();
    //                    //string RowerCharge = dt.Rows[0]["RowerCharge"].ToString();
    //                    //string BookingId = dt.Rows[0]["BookingId"].ToString();
    //                    //string BoatRefNo = dt.Rows[0]["BoatReferenceNo"].ToString();
    //                    //string BoatTypeId = dt.Rows[0]["BoatTypeId"].ToString();
    //                    //string BoatType = dt.Rows[0]["BoatType"].ToString();
    //                    //string BoatSeaterId = dt.Rows[0]["BoatSeaterId"].ToString();
    //                    //string SeaterType = dt.Rows[0]["SeaterType"].ToString();
    //                    //string BookingDuration = dt.Rows[0]["BookingDuration"].ToString();
    //                    //string BoatId = dt.Rows[0]["BoatId"].ToString();



    //                    if (TripStart == "")
    //                    {
    //                        BarCodeTripStart();
    //                        BindStartedTripList();
    //                        BindEndedTripList();

    //                        return;
    //                    }
    //                    string TripEnd = dt.Rows[0]["TripEndTime"].ToString();
    //                    if (TripEnd == "")
    //                    {
    //                        BarCodeTripEnd();
    //                        BindStartedTripList();
    //                        BindEndedTripList();
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        BindTripEndGrid();
    //                        lblStartResponse.Text = "";
    //                        lblRowerResponse.Text = "";
    //                        lblRowerResponse.Visible = false;
    //                        imgEnd.Visible = false;
    //                        ImgBoatType.Visible = false;
    //                        lblAlendBookingId.Text = ViewState["BookingId"].ToString().Trim();
    //                        lblAlendBookingPin.Text = ViewState["BookingPin"].ToString().Trim();
    //                        lblAlendDuartion.Text = ViewState["BookingDuration"].ToString().Trim();
    //                        lblAlendTravelledMins.Text = ViewState["TravelledMins"].ToString().Trim();
    //                        lblAlendStartTime.Text = ViewState["TripStartTime"].ToString().Trim();
    //                        lblAlendEndTime.Text = ViewState["TripEndTime"].ToString().Trim();
    //                        lblAlreadyBoatType.Text = ViewState["AlreadyBoatType"].ToString().Trim();
    //                        lblAlreadyBoatSeater.Text = ViewState["AlreadyBoatSeater"].ToString().Trim();
    //                        if (ViewState["RowerName"].ToString().Trim() != "")
    //                        {
    //                            lblAlendRowerName.Text = ViewState["RowerName"].ToString().Trim();
    //                            divAlreadyRower.Visible = true;
    //                        }
    //                        else
    //                        {
    //                            divAlreadyRower.Visible = false;
    //                        }
    //                        lblAlreadyStatus.Text = "Trip Already Ended !!!";
    //                        txtStartDetails.Text = "";
    //                        divAlreadyEndStatus.Style.Add("background-color", "#98c8e6");
    //                        gvTripSheetSettelementEnd.Visible = true;
    //                        txtStartDetails.Focus();
    //                        lblStartResponse.Visible = false;
    //                        header.Visible = true;
    //                        GvStartedList.Visible = true;
    //                        divFinalStatus.Visible = false;
    //                        divFinalEndStatus.Visible = false;
    //                        divAlreadyEndStatus.Visible = true;
    //                        divTripStarted.Visible = true;
    //                        MpeTrip.Hide();
    //                        ClientScript.RegisterStartupScript(Page.GetType(), "HideAlreadyEndLabel", "javascript:HideAlreadyEndLabel();", true);
    //                        ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid();", true);
    //                        BindStartedTripList();
    //                        BindEndedTripList();
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    txtStartDetails.Text = "";
    //                    txtStartDetails.Focus();
    //                    MpeTrip.Show();
    //                    lblStartResponse.Text = "User Doesn't have Rights to Proceed this Ticket !!!";
    //                    lblStartResponse.Visible = true;
    //                    ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideStartLabel();", true);
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                var Errorresponse = response.Content.ReadAsStringAsync().Result;
    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}

    public void GenerateBarCode()
    {
        ViewState["BoatStatus"] = "";
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                HttpResponseMessage response1;

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BarcodePin = hfBarcodePin.Value.Trim()
                };
                response1 = client.PostAsJsonAsync("TripSheetweb/GetPremiumStatus", vTripSheetSettlement).Result;
                var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse1)["StatusCode"].ToString());
                string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();
                if (StatusCode1 == 1)
                {

                    MpeTrip.Show();
                    lblStartResponse.Text = ResponseMsg1;
                    txtStartDetails.Text = "";
                    txtStartDetails.Focus();
                    lblRowerResponse.Text = "";
                    divAlreadyEndStatus.Visible = false;
                    divFinalEndStatus.Visible = false;
                    divFinalStatus.Visible = false;
                    imgEnd.Visible = false;
                    ImgBoatType.Visible = false;
                    ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                    ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideStartLabel();", true);
                    // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg1.ToString().Trim() + "');", true);


                }
                else
                {
                    var vTripSheetSettlement3 = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingPin = hfBarcodePin.Value.Trim(),
                        BookingId = hfBookingId.Value.Trim(),
                    };
                    response1 = client.PostAsJsonAsync("ScanRescheduling", vTripSheetSettlement3).Result;
                    if (response1.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse2 = response1.Content.ReadAsStringAsync().Result;
                        int StatusCode2 = Convert.ToInt32(JObject.Parse(vehicleEditresponse2)["StatusCode"].ToString());
                        string ResponseMsg2 = JObject.Parse(vehicleEditresponse2)["Response"].ToString();
                        if (StatusCode2 == 1)
                        {
                            DataTable dtRes = JsonConvert.DeserializeObject<DataTable>(ResponseMsg2);
                            string BookingNewDate = DateTime.Parse(dtRes.Rows[0]["BookingDate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            string Status = dtRes.Rows[0]["RStatus"].ToString().Trim();
                            ViewState["BoatStatus"] = Status.ToString().Trim();
                            string NewDate = DateTime.Now.ToString("dd/MM/yyyy");
                            if (BookingNewDate == NewDate)
                            {
                                var vTripSheetSettlement2 = new TripSheetSettlement()
                                {
                                    QueryType = "TripSheetweb/ScanTripStart",
                                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                    BarcodePin = hfBarcodePin.Value.Trim(),
                                    UserId = Session["UserId"].ToString().Trim()
                                };
                                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                                {
                                    response = client.PostAsJsonAsync("TripStartResch", vTripSheetSettlement2).Result;
                                }

                                else
                                {
                                    var vTripSheetSettlement1 = new TripSheetSettlement()
                                    {
                                        QueryType = "NewTripSheetweb/ScanTripStart",
                                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                        BarcodePin = hfBarcodePin.Value.Trim(),
                                        UserId = Session["UserId"].ToString().Trim()
                                    };

                                    response = client.PostAsJsonAsync("TripStartResch", vTripSheetSettlement1).Result;
                                }
                                if (response.IsSuccessStatusCode)
                                {
                                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                                    if (StatusCode == 1)
                                    {
                                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                                        string TripStart = dt.Rows[0]["TripStartTime"].ToString();

                                        if (TripStart == "")
                                        {
                                            BarCodeTripStart();
                                            BindStartedTripList();
                                            BindEndedTripList();

                                            return;
                                        }
                                        string TripEnd = dt.Rows[0]["TripEndTime"].ToString();
                                        if (TripEnd == "")
                                        {
                                            BarCodeTripEnd();
                                            BindStartedTripList();
                                            BindEndedTripList();
                                            return;
                                        }
                                        else
                                        {
                                            BindTripEndGrid();
                                            lblStartResponse.Text = "";
                                            lblRowerResponse.Text = "";
                                            lblRowerResponse.Visible = false;
                                            imgEnd.Visible = false;
                                            ImgBoatType.Visible = false;
                                            lblAlendBookingId.Text = ViewState["BookingId"].ToString().Trim();
                                            lblAlendBookingPin.Text = ViewState["BookingPin"].ToString().Trim();
                                            lblAlendDuartion.Text = ViewState["BookingDuration"].ToString().Trim();
                                            lblAlendTravelledMins.Text = ViewState["TravelledMins"].ToString().Trim();
                                            lblAlendStartTime.Text = ViewState["TripStartTime"].ToString().Trim();
                                            lblAlendEndTime.Text = ViewState["TripEndTime"].ToString().Trim();
                                            lblAlreadyBoatType.Text = ViewState["AlreadyBoatType"].ToString().Trim();
                                            lblAlreadyBoatSeater.Text = ViewState["AlreadyBoatSeater"].ToString().Trim();
                                            if (ViewState["RowerName"].ToString().Trim() != "")
                                            {
                                                lblAlendRowerName.Text = ViewState["RowerName"].ToString().Trim();
                                                divAlreadyRower.Visible = true;
                                            }
                                            else
                                            {
                                                divAlreadyRower.Visible = false;
                                            }
                                            lblAlreadyStatus.Text = "Trip Already Ended !!!";
                                            txtStartDetails.Text = "";
                                            divAlreadyEndStatus.Style.Add("background-color", "#98c8e6");
                                            gvTripSheetSettelementEnd.Visible = true;
                                            txtStartDetails.Focus();
                                            lblStartResponse.Visible = false;
                                            header.Visible = true;
                                            GvStartedList.Visible = true;
                                            divFinalStatus.Visible = false;
                                            divFinalEndStatus.Visible = false;
                                            divAlreadyEndStatus.Visible = true;
                                            divTripStarted.Visible = true;
                                            MpeTrip.Hide();
                                            divExtendedRower.Style.Add("display", "none");
                                            ClientScript.RegisterStartupScript(Page.GetType(), "HideAlreadyEndLabel", "javascript:HideAlreadyEndLabel();", true);
                                       
                                            ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGridAlready", "javascript:HideEndLabelGridAlready(5);", true);
                                            BindStartedTripList();
                                            BindEndedTripList();
                                            
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        txtStartDetails.Text = "";
                                        txtStartDetails.Focus();
                                        MpeTrip.Show();
                                        lblStartResponse.Text = "User Doesn't have Rights to Proceed this Ticket !!!";
                                        lblStartResponse.Visible = true;
                                        ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideStartLabel();", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                                }
                            }
                            else
                            {

                                MpeTrip.Show();
                                lblStartResponse.Visible = true;
                                lblStartResponse.Text = "Booking Rescheduled To " + BookingNewDate.ToString() + ",Ticket is Invalid !!!";
                                txtStartDetails.Text = "";
                                txtStartDetails.Focus();
                                lblRowerResponse.Text = "";
                                divAlreadyEndStatus.Visible = false;
                                divFinalEndStatus.Visible = false;
                                divFinalStatus.Visible = false;
                                imgEnd.Visible = false;
                                ImgBoatType.Visible = false;
                                ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideStartLabel();", true);
                            }

                        }
                        else
                        {
                            ViewState["BoatStatus"] = "B";
                            var vTripSheetSettlement2 = new TripSheetSettlement()
                            {
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BarcodePin = hfBarcodePin.Value.Trim()
                            };
                            if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                            {
                                response = client.PostAsJsonAsync("TripSheetweb/ScanTripStart", vTripSheetSettlement2).Result;
                            }

                            else
                            {
                                var vTripSheetSettlement1 = new TripSheetSettlement()
                                {
                                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                    BarcodePin = hfBarcodePin.Value.Trim(),
                                    UserId = Session["UserId"].ToString().Trim()
                                };

                                response = client.PostAsJsonAsync("NewTripSheetweb/ScanTripStart", vTripSheetSettlement1).Result;
                            }
                            if (response.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                                if (StatusCode == 1)
                                {
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                                    string TripStart = dt.Rows[0]["TripStartTime"].ToString();
                                    if (TripStart == "")
                                    {
                                        BarCodeTripStart();
                                        BindStartedTripList();
                                        BindEndedTripList();

                                        return;
                                    }
                                    string TripEnd = dt.Rows[0]["TripEndTime"].ToString();
                                    if (TripEnd == "")
                                    {
                                        BarCodeTripEnd();
                                        BindStartedTripList();
                                        BindEndedTripList();
                                        return;
                                    }
                                    else
                                    {
                                        BindTripEndGrid();
                                        lblStartResponse.Text = "";
                                        lblRowerResponse.Text = "";
                                        lblRowerResponse.Visible = false;
                                        imgEnd.Visible = false;
                                        ImgBoatType.Visible = false;
                                        lblAlendBookingId.Text = ViewState["BookingId"].ToString().Trim();
                                        lblAlendBookingPin.Text = ViewState["BookingPin"].ToString().Trim();
                                        lblAlendDuartion.Text = ViewState["BookingDuration"].ToString().Trim();
                                        lblAlendTravelledMins.Text = ViewState["TravelledMins"].ToString().Trim();
                                        lblAlendStartTime.Text = ViewState["TripStartTime"].ToString().Trim();
                                        lblAlendEndTime.Text = ViewState["TripEndTime"].ToString().Trim();
                                        lblAlreadyBoatType.Text = ViewState["AlreadyBoatType"].ToString().Trim();
                                        lblAlreadyBoatSeater.Text = ViewState["AlreadyBoatSeater"].ToString().Trim();
                                        if (ViewState["RowerName"].ToString().Trim() != "")
                                        {
                                            lblAlendRowerName.Text = ViewState["RowerName"].ToString().Trim();
                                            divAlreadyRower.Visible = true;
                                        }
                                        else
                                        {
                                            divAlreadyRower.Visible = false;
                                        }
                                        lblAlreadyStatus.Text = "Trip Already Ended !!!";
                                        txtStartDetails.Text = "";
                                        divAlreadyEndStatus.Style.Add("background-color", "#98c8e6");
                                        gvTripSheetSettelementEnd.Visible = true;
                                        txtStartDetails.Focus();
                                        lblStartResponse.Visible = false;
                                        header.Visible = true;
                                        GvStartedList.Visible = true;
                                        divFinalStatus.Visible = false;
                                        divFinalEndStatus.Visible = false;
                                        divAlreadyEndStatus.Visible = true;
                                        divTripStarted.Visible = true;
                                        MpeTrip.Hide();
                                        divExtendedRower.Style.Add("display", "none");
                                        ClientScript.RegisterStartupScript(Page.GetType(), "HideAlreadyEndLabel", "javascript:HideAlreadyEndLabel();", true);
                                        ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGridAlready", "javascript:HideEndLabelGridAlready(5);", true);
                                        BindStartedTripList();
                                        BindEndedTripList();
                                      
                                        return;
                                    }
                                }
                                else
                                {
                                    txtStartDetails.Text = "";
                                    txtStartDetails.Focus();
                                    MpeTrip.Show();
                                    lblStartResponse.Text = "User Doesn't have Rights to Proceed this Ticket !!!";
                                    lblStartResponse.Visible = true;
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideStartLabel();", true);
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
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindTripCountDetails()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var body = new CommonClass()
                    {
                        QueryType = "TripSheetCount",
                        ServiceType = "Admin",
                        Input1 = "",
                        Input2 = "",
                        Input3 = "",
                        Input4 = "",
                        Input5 = "",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("CommonReport", body).Result;
                }
                else
                {
                    var body = new CommonClass()
                    {
                        QueryType = "TripSheetCount",
                        ServiceType = "User",
                        Input1 = Session["UserId"].ToString().Trim(),
                        Input2 = "",
                        Input3 = "",
                        Input4 = "",
                        Input5 = "",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("CommonReport", body).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        lblAlreadyStartedCount.Text = "Trip Already Started :  " + dtExists.Rows[0]["TripSheetEndCount"].ToString().Trim();
                        lblAlreadyEndedCount.Text = "Trip Already Ended :  " + dtExists.Rows[0]["TripSheetClosedCount"].ToString().Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindStartedTripList()
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
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("TripSheetweb/ScanTripStartedList", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetweb/ScanTripStartedList", vTripSheetSettlement).Result;
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
                            BindTripCountDetails();
                            GvStartedList.DataSource = dt;
                            GvStartedList.DataBind();
                            divTripStarted.Visible = true;
                            GvStartedList.Visible = true;
                            header.Visible = true;

                        }
                        else
                        {
                            GvStartedList.DataBind();
                            GvStartedList.Visible = false;

                        }
                    }
                    else
                    {
                        header.Visible = false;
                        // var Errorresponse = response.Content.ReadAsStringAsync().Result;
                        // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindEndedTripList()
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
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("TripSheetweb/ListTop5TripClosed", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetweb/ListTop5TripClosed", vTripSheetSettlement).Result;
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
                            BindTripCountDetails();
                            gvTripSheetSettelementEnd.DataSource = dt;
                            gvTripSheetSettelementEnd.DataBind();
                            divGridStart.Visible = true;
                            gvTripSheetSettelementEnd.Visible = true;
                            headerend.Visible = true;

                        }
                        else
                        {
                            gvTripSheetSettelementEnd.DataBind();
                            gvTripSheetSettelementEnd.Visible = false;

                        }
                    }
                    else
                    {
                        headerend.Visible = false;
                        // var Errorresponse = response.Content.ReadAsStringAsync().Result;
                        // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BarCodeTripStart()
    {
        try
        {
            hfBoatTypeId.Value = "";
            hfBoatSeaterId.Value = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                if (ViewState["BoatStatus"].ToString().Trim() == "B")
                {
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim()

                        };

                        response = client.PostAsJsonAsync("TripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("NewTripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
                    }
                }
                else
                {
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            QueryType = "TripSheetweb/ScanTripStart",
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()

                        };

                        response = client.PostAsJsonAsync("TripStartResch", vTripSheetSettlement).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            QueryType = "NewTripSheetweb/ScanTripStart",
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("TripStartResch", vTripSheetSettlement).Result;
                    }
                }

                ////------------------------------------------------------------------------------------------------------//
                //if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                //{
                //    var vTripSheetSettlement = new TripSheetSettlement()
                //    {
                //        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                //        BarcodePin = hfBarcodePin.Value.Trim()
                //    };

                //    response = client.PostAsJsonAsync("TripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
                //}
                //else
                //{
                //    var vTripSheetSettlement = new TripSheetSettlement()
                //    {
                //        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                //        BarcodePin = hfBarcodePin.Value.Trim(),
                //        UserId = Session["UserId"].ToString().Trim()
                //    };

                //    response = client.PostAsJsonAsync("NewTripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
                //}

                ////------------------------------------------------------------------------------------------------------//




                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        try
                        {
                            HttpResponseMessage response1;
                            string sMSG = string.Empty;
                            string Rower = string.Empty;

                            string SelfDrive = dt.Rows[0]["SelfDrive"].ToString();
                            string RowerCharge = dt.Rows[0]["RowerCharge"].ToString();
                            string BookingId = dt.Rows[0]["BookingId"].ToString();
                            string BoatRefNo = dt.Rows[0]["BoatReferenceNo"].ToString();
                            hfBoatTypeId.Value = dt.Rows[0]["BoatTypeId"].ToString();
                            ViewState["BoatType"] = dt.Rows[0]["BoatType"].ToString();
                            hfBoatSeaterId.Value = dt.Rows[0]["BoatSeaterId"].ToString();
                            ViewState["BoatSeater"] = dt.Rows[0]["SeaterType"].ToString();
                            hfBookingDuration.Value = "";
                            hfBookingDuration.Value = dt.Rows[0]["BookingDuration"].ToString();
                            string RowerId = hfBarcodePinRowerId.Value.Trim();

                            string RowerName = hfBarcodePinRowerName.Value.Trim();// New changes 

                            if (RowerId == "")
                            {
                                Rower = "0";
                            }
                            else
                            {
                                Rower = RowerId;
                            }
                            string BoatId = dt.Rows[0]["BoatId"].ToString();

                            // New Changes on 2021-04-21 -------------------------------------------------------------------------------------------------------------------//

                            if (RowerId.ToString().Trim() != "")
                            {
                                autoTripEndDetails(RowerId);

                                if (ViewState["AutoEndForNoDepositeTripEnd"].ToString().Trim() == "Y")
                                {
                                    if (Convert.ToDecimal(ViewState["BoatDeposit"].ToString().Trim()) > 0)
                                    {
                                        divRowerInfoMsg.Visible = true;
                                        lblStartResponse.Visible = false;
                                        MpeTrip.Hide();
                                        lblRowerInfoMsg.Text = "Rower-'" + RowerName.ToString().Trim() + "' is Already On A Trip, Cannot Start Trip.";
                                        return;
                                    }
                                    else
                                    {
                                        divRowerInfoMsg.Visible = true;
                                        lblStartResponse.Visible = true;
                                    }

                                    //Minmum Trip Time Check
                                    if (ViewState["BookingDurationTripEnd"].ToString().Trim() != "0")
                                    {
                                        double Duration = 0;
                                        Duration = Math.Round(Convert.ToDouble(ViewState["BookingDurationTripEnd"].ToString().Trim()) / 3);
                                        DateTime TripStartTime = DateTime.Parse(ViewState["TripStartTimeTripEnd"].ToString().Trim());
                                        DateTime TripStartTimeHourseMinuteFormat = DateTime.Parse(TripStartTime.ToString("HH:mm"));
                                        DateTime FinalTripTime = TripStartTimeHourseMinuteFormat.AddMinutes(Duration);
                                        DateTime NowTime = Convert.ToDateTime(DateTime.Now.ToString());
                                        if (NowTime < FinalTripTime)
                                        {
                                            divRowerInfoMsg.Visible = true;
                                            lblStartResponse.Visible = false;
                                            MpeTrip.Hide();
                                            lblRowerInfoMsg.Text = "Trip Cannot Be Started With  Rower-'" + RowerName.ToString().Trim() + "', before-'" + FinalTripTime.ToString("hh:mm tt").Trim() + "'";
                                            return;
                                        }
                                        else
                                        {
                                            divRowerInfoMsg.Visible = false;
                                            lblStartResponse.Visible = true;
                                        }
                                    }
                                }
                                else
                                {
                                    divRowerInfoMsg.Visible = false;
                                    lblStartResponse.Visible = true;
                                }
                            }
                            else
                            {
                                divRowerInfoMsg.Visible = false;
                                lblStartResponse.Visible = true;
                            }

                            // New Changes End -------------------------------------------------------------------------------------------------------------------//


                            if (SelfDrive == "N" && Convert.ToDecimal(RowerCharge) > 0)
                            {
                                if (Convert.ToInt32(Rower) == 0)
                                {
                                    txtRowerDetails.Attributes.Add("style", "display:inline-block");
                                    lblRowerResponse.Visible = true;
                                    MpeTrip.Show();
                                    lblRowerResponse.Text = "Scan Rower Id !!!";
                                    txtRowerDetails.Text = "";
                                    divAlreadyEndStatus.Visible = false;
                                    divFinalEndStatus.Visible = false;
                                    divFinalStatus.Visible = false;
                                    imgEnd.Visible = false;
                                    ImgBoatType.Visible = false;
                                    txtRowerDetails.Focus();
                                    txtStartDetails.Attributes.Add("style", "display:none");
                                    txtStartDetails.Text = "";
                                    lblStartResponse.Visible = false;
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelRower", "javascript:HideLabelRower();", true);
                                    return;
                                }
                            }

                            var vTripSheetSettlement1 = new TripSheetSettlement()
                            {
                                QueryType = "TripStart",
                                BookingId = BookingId.Trim(),
                                BoatReferenceNo = BoatRefNo.Trim(),
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                RowerId = Rower.Trim(),
                                TripStartTime = "",
                                TripEndTime = "",//End Time Pas Empty
                                ActualBoatId = BoatId.Trim(),
                                SSUserBy = Session["UserId"].ToString().Trim(),
                                SDUserBy = Session["UserId"].ToString().Trim(),
                                BookingMedia = "DW"
                            };

                            response1 = client.PostAsJsonAsync("NewTripSheetWeb/Update", vTripSheetSettlement1).Result;
                            sMSG = "Inserted Successfully";

                            if (response1.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                                if (StatusCode1 == 1)
                                {
                                    if (hfBoatTypeId.Value == "5")
                                    {
                                        ImgBoatType.ImageUrl = "~/images/RowBoat.png";
                                    }
                                    if (hfBoatTypeId.Value == "6")
                                    {
                                        ImgBoatType.ImageUrl = "~/images/MotorBoat.png";
                                    }
                                    if (hfBoatTypeId.Value == "7")
                                    {
                                        ImgBoatType.ImageUrl = "~/images/PedalBoat.png";
                                    }
                                    if (hfBoatTypeId.Value != "7" & hfBoatTypeId.Value != "6" & hfBoatTypeId.Value != "5")
                                    {
                                        ImgBoatType.ImageUrl = "~/images/CommonBoat.png";
                                    }
                                    lblStartResponse.Visible = false;
                                    lblRowerResponse.Text = "";
                                    lblRowerResponse.Visible = false;
                                    divFinalStatus.Visible = true;
                                    divFinalEndStatus.Visible = false;
                                    divAlreadyEndStatus.Visible = false;
                                    GvStartedList.Visible = false;
                                    header.Visible = false;
                                    imgEnd.Visible = false;
                                    ImgBoatType.Visible = true;
                                    txtStartDetails.Text = "";
                                    txtStartDetails.Focus();
                                    if (Rower != "0")
                                    {
                                        divFinalRower.Visible = true;
                                    }
                                    else
                                    {
                                        divFinalRower.Visible = false;
                                    }
                                    string Display = ResponseMsg1.ToString().Trim();
                                    string[] TimeList = Display.Split(',');
                                    lblFinalBookingId.Text = TimeList[0].ToString().Trim();
                                    lblFinalBookingPin.Text = TimeList[1].ToString().Trim();
                                    lblFinalBookingDuration.Text = hfBookingDuration.Value.Trim();
                                    lblFinalStartTime.Text = TimeList[2].ToString().Trim();
                                    lblFinalBoatType.Text = ViewState["BoatType"].ToString().Trim();
                                    lblFinalBoatSeater.Text = ViewState["BoatSeater"].ToString().Trim();
                                    lblFinalRowerName.Text = hfBarcodePinRowerName.Value.Trim();
                                    lblStartStatus.Text = TimeList[3].ToString().Trim();
                                    divFinalStatus.Style.Add("background-color", "#006400");
                                    hfBarcodePinRowerName.Value = "";
                                    MpeTrip.Hide();
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelGrid", "javascript:HideLabelGrid();", true);

                                    return;
                                }
                                else
                                {
                                    lblStartResponse.Text = ResponseMsg1.ToString();
                                    return;
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                                }
                            }
                            else
                            {
                                lblStartResponse.Text = response1.ReasonPhrase.ToString();
                                return;
                                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                            }


                        }
                        catch (Exception ex)
                        {
                            lblStartResponse.Text = ex.ToString();
                            return;
                            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
                        }
                        return;

                    }
                    else
                    {
                        divGridStart.Visible = false;
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

    public void BindTripEndGrid()
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
                if (ViewState["BoatStatus"].ToString().Trim() == "B")
                {
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim()
                        };

                        response = client.PostAsJsonAsync("TripSheetweb/ScanTripEndDuration", vTripSheetSettlement).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("NewTripSheetweb/ScanTripEndDuration", vTripSheetSettlement).Result;
                    }
                }
                else
                {
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            QueryType = "TripSheetweb/ScanTripEndDuration",
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("TripEndDurationResch", vTripSheetSettlement).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            QueryType = "NewTripSheetweb/ScanTripEndDuration",
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("TripEndDurationResch", vTripSheetSettlement).Result;
                    }
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
                            ViewState["BookingId"] = dt.Rows[0]["BookingId"].ToString();
                            ViewState["BookingPin"] = dt.Rows[0]["BookingPin"].ToString();
                            ViewState["RowerName"] = dt.Rows[0]["RowerName"].ToString();
                            ViewState["TripEndTime"] = dt.Rows[0]["TripEndTime"].ToString();
                            ViewState["TripStartTime"] = dt.Rows[0]["TripStartTime"].ToString();
                            ViewState["BookingDuration"] = dt.Rows[0]["BookingDuration"].ToString();
                            ViewState["TravelledMins"] = dt.Rows[0]["TraveledTime"].ToString();
                            ViewState["AlreadyBoatType"] = dt.Rows[0]["BoatType"].ToString();
                            ViewState["AlreadyBoatSeater"] = dt.Rows[0]["SeaterType"].ToString();
                            return;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        header.Visible = false;
                        // var Errorresponse = response.Content.ReadAsStringAsync().Result;
                        // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BarCodeTripEnd()
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
                if (ViewState["BoatStatus"].ToString().Trim() == "B")
                {
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim()
                        };

                        response = client.PostAsJsonAsync("TripSheetweb/ScanTripEnd", vTripSheetSettlement).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("NewTripSheetweb/ScanTripEnd", vTripSheetSettlement).Result;
                    }
                }

                else
                {
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            QueryType = "TripSheetweb/ScanTripEnd",
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("TripEndResch", vTripSheetSettlement).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            QueryType = "NewTripSheetweb/ScanTripEnd",
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("TripEndResch", vTripSheetSettlement).Result;
                    }
                }
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        try
                        {
                            HttpResponseMessage response1;
                            string sMSG = string.Empty;
                            string Rower = string.Empty;

                            ////    string SelfDrive = dt.Rows[0]["SelfDrive"].ToString();
                            ////    string RowerCharge = dt.Rows[0]["RowerCharge"].ToString();
                            ////    string BookingId = dt.Rows[0]["BookingId"].ToString();
                            ////    string BoatRefNo = dt.Rows[0]["BoatReferenceNo"].ToString();
                            ////    string RowerId = dt.Rows[0]["RowerId"].ToString();
                            ////    if (RowerId == "")
                            ////    {
                            ////        Rower = "0";
                            ////    }
                            ////    else
                            ////    {
                            ////        Rower = RowerId;
                            ////    }
                            ////    string RowerName = dt.Rows[0]["RowerName"].ToString();
                            ////    ViewState["EndBoatType"] = dt.Rows[0]["BoatType"].ToString();
                            ////    ViewState["EndBoatSeater"] = dt.Rows[0]["SeaterType"].ToString();
                            ////    string BoatId = dt.Rows[0]["BoatId"].ToString();
                            ////    string BookingDuration = dt.Rows[0]["BookingDuration"].ToString();
                            ////    string TripStartTime = dt.Rows[0]["TripStartTime"].ToString();

                            ////    double Duration = 0;
                            ////    Duration = Math.Round(Convert.ToDouble(BookingDuration) / 3);
                            ////    string Time = DateTime.Now.ToString("HH:mm");
                            ////    string TimeTrip = DateTime.Now.ToString("HH:mm:ss");
                            ////    DateTime d = DateTime.Parse(TripStartTime);
                            ////    DateTime now = DateTime.Parse(d.ToString("HH:mm"));
                            ////    DateTime nowTrip = DateTime.Parse(d.ToString("HH:mm:ss"));
                            ////    DateTime modifiedDatetime = now.AddMinutes(Duration);
                            ////    var ModTime = modifiedDatetime.ToString("hh:mm tt");
                            ////    if (modifiedDatetime > DateTime.Parse(Time))
                            ////    {
                            ////        txtStartDetails.Text = "";
                            ////        txtStartDetails.Focus();
                            ////        lblStartResponse.Text = "";
                            ////        lblRowerResponse.Text = "";
                            ////        lblStartResponse.Visible = true;
                            ////        divFinalStatus.Visible = false;
                            ////        divFinalEndStatus.Visible = false;
                            ////        divAlreadyEndStatus.Visible = false;
                            ////        ImgBoatType.Visible = false;
                            ////        imgEnd.Visible = false;
                            ////        MpeTrip.Show();
                            ////        lblStartResponse.Text = "Trip started only at " + TripStartTime + ", can be end only after " + Duration + " minutes ( " + ModTime + " )";
                            ////        bdycolor.Style.Add("background-color", "#ffffff");
                            ////        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideStartLabel();", true);

                            ////        BindStartedTripList();
                            ////        BindEndedTripList();
                            ////        return;
                            ////    }
                            ////    else
                            ////    {

                            ////        var vTripSheetSettlement1 = new TripSheetSettlement()
                            ////        {
                            ////            QueryType = "TripEnd",
                            ////            BookingId = BookingId.Trim(),
                            ////            BoatReferenceNo = BoatRefNo.Trim(),
                            ////            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            ////            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                            ////            RowerId = Rower.Trim(),
                            ////            TripStartTime = "",//Start Time Empty
                            ////            TripEndTime = "",
                            ////            ActualBoatId = BoatId.Trim(),
                            ////            SSUserBy = Session["UserId"].ToString().Trim(),
                            ////            SDUserBy = Session["UserId"].ToString().Trim(),
                            ////            BookingMedia = "DW"
                            ////        };

                            ////        response1 = client.PostAsJsonAsync("NewTripSheetWeb/Update", vTripSheetSettlement1).Result;
                            ////        sMSG = "Inserted Successfully";

                            ////        if (response1.IsSuccessStatusCode)
                            ////        {
                            ////            var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                            ////            int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                            ////            string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                            ////            if (StatusCode1 == 1)
                            ////            {
                            ////                BindTripEndGrid();
                            ////                lblStartResponse.Visible = false;
                            ////                lblRowerResponse.Text = "";
                            ////                lblRowerResponse.Visible = false;
                            ////                divFinalEndStatus.Visible = true;
                            ////                divGridStart.Visible = false;
                            ////                divTripStarted.Visible = false;
                            ////                divFinalStatus.Visible = false;
                            ////                divAlreadyEndStatus.Visible = false;
                            ////                GvStartedList.Visible = false;
                            ////                gvTripSheetSettelementEnd.Visible = false;
                            ////                header.Visible = false;
                            ////                headerend.Visible = false;
                            ////                txtStartDetails.Text = "";
                            ////                ImgBoatType.Visible = false;
                            ////                imgEnd.Visible = true;
                            ////                txtStartDetails.Focus();
                            ////                if (Rower != "0")
                            ////                {
                            ////                    divEndRower.Visible = true;
                            ////                }
                            ////                else
                            ////                {
                            ////                    divEndRower.Visible = false;
                            ////                }
                            ////                string Display = ResponseMsg1.ToString().Trim();
                            ////                string[] TimeList = Display.Split(',');
                            ////                lblEndBookingId.Text = TimeList[0].ToString().Trim();
                            ////                lblEndBookingpin.Text = TimeList[1].ToString().Trim();
                            ////                lblEndDuration.Text = ViewState["BookingDuration"].ToString().Trim();
                            ////                lblEndTravelledMins.Text = ViewState["TravelledMins"].ToString().Trim();
                            ////                lblEndStartTime.Text = ViewState["TripStartTime"].ToString().Trim();
                            ////                lblFinalEndTime.Text = TimeList[2].ToString().Trim();
                            ////                lblEndBoatType.Text = ViewState["EndBoatType"].ToString().Trim();
                            ////                lblEndBoatSeater.Text = ViewState["EndBoatSeater"].ToString().Trim();
                            ////                lblEndRowerName.Text = RowerName.ToString().Trim();
                            ////                lblEndStatus.Text = TimeList[3].ToString().Trim();
                            ////                divFinalEndStatus.Style.Add("background-color", "#ff0000");
                            ////                MpeTrip.Hide();
                            ////                ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabel", "javascript:HideEndLabel();", true);
                            ////                ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid();", true);

                            ////                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                            ////            }
                            ////            else
                            ////            {
                            ////                lblStartResponse.Text = ResponseMsg1.ToString();
                            ////                return;
                            ////                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                            ////            }
                            ////        }


                            ////        else
                            ////        {
                            ////            lblStartResponse.Text = response1.ReasonPhrase.ToString();
                            ////            return;
                            ////            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                            ////        }
                            ////    }

                            ////}
                            ////


                            string SelfDrive = dt.Rows[0]["SelfDrive"].ToString();
                            string RowerCharge = dt.Rows[0]["RowerCharge"].ToString();
                            string BookingId = dt.Rows[0]["BookingId"].ToString();
                            ViewState["BookingId"] = BookingId.ToString();

                            string BoatRefNo = dt.Rows[0]["BoatReferenceNo"].ToString();
                            ViewState["BoatReferenceNo"] = BoatRefNo.ToString();

                            string RowerId = dt.Rows[0]["RowerId"].ToString();
                            ViewState["BoatTypeId"] = dt.Rows[0]["BoatTypeId"].ToString();
                            ViewState["BoatSeaterId"] = dt.Rows[0]["BoatSeaterId"].ToString();

                            if (RowerId == "")
                            {
                                Rower = "0";
                            }
                            else
                            {
                                Rower = RowerId;
                            }

                            ViewState["BookingPin"] = dt.Rows[0]["BookingPin"].ToString();
                            string RowerName = dt.Rows[0]["RowerName"].ToString();
                            ViewState["EndBoatType"] = dt.Rows[0]["BoatType"].ToString();
                            ViewState["EndBoatSeater"] = dt.Rows[0]["SeaterType"].ToString();
                            string BoatId = dt.Rows[0]["BoatId"].ToString();
                            string BookingDuration = dt.Rows[0]["BookingDuration"].ToString();
                            ViewState["BookingDuration"] = BookingDuration.ToString();
                            string TripStartTime = dt.Rows[0]["TripStartTime"].ToString();
                            GetDurationGraceTime();
                            string DurationGracetime = ViewState["DurationGraceTime"].ToString().Trim();

                            double Duration = 0;
                            Duration = Math.Round(Convert.ToDouble(BookingDuration) / 3);
                            double ExDuration = 0;
                            ExDuration = Math.Round(Convert.ToDouble(DurationGracetime.ToString().Trim()));
                            string Time = DateTime.Now.ToString("HH:mm");
                            string TimeTrip = DateTime.Now.ToString("HH:mm:ss");
                            DateTime d = DateTime.Parse(TripStartTime);
                            DateTime now = DateTime.Parse(d.ToString("HH:mm"));
                            DateTime nowTrip = DateTime.Parse(d.ToString("HH:mm:ss"));
                            DateTime modifiedDatetime = now.AddMinutes(Duration);
                            DateTime ExtensionDatetime = now.AddMinutes(ExDuration);
                            var ModTime = modifiedDatetime.ToString("hh:mm tt");

                            if (modifiedDatetime > DateTime.Parse(Time))
                            {
                                txtStartDetails.Text = "";
                                txtStartDetails.Focus();
                                lblStartResponse.Text = "";
                                lblRowerResponse.Text = "";
                                lblStartResponse.Visible = true;
                                divFinalStatus.Visible = false;
                                divFinalEndStatus.Visible = false;
                                divAlreadyEndStatus.Visible = false;
                                ImgBoatType.Visible = false;
                                imgEnd.Visible = false;
                                MpeTrip.Show();
                                lblStartResponse.Text = "Trip started only at " + TripStartTime + ", can be end only after " + Duration + " minutes ( " + ModTime + " )";
                                bdycolor.Style.Add("background-color", "#ffffff");
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideStartLabel();", true);

                                BindStartedTripList();
                                BindEndedTripList();
                                return;
                            }
                            else if (DateTime.Parse(Time) > ExtensionDatetime)
                            {
                                CheckExtension();
                                if (ViewState["ExtensionType"].ToString().Trim() == "N")
                                {
                                    var vTripSheetSettlement1 = new TripSheetSettlement()
                                    {
                                        QueryType = "TripEnd",
                                        BookingId = BookingId.Trim(),
                                        BoatReferenceNo = BoatRefNo.Trim(),
                                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                        RowerId = Rower.Trim(),
                                        TripStartTime = "",//Start Time Empty
                                        TripEndTime = "",
                                        ActualBoatId = BoatId.Trim(),
                                        SSUserBy = Session["UserId"].ToString().Trim(),
                                        SDUserBy = Session["UserId"].ToString().Trim(),
                                        BookingMedia = "DW"
                                    };

                                    response1 = client.PostAsJsonAsync("NewTripSheetWeb/Update", vTripSheetSettlement1).Result;
                                    sMSG = "Inserted Successfully";

                                    if (response1.IsSuccessStatusCode)
                                    {
                                        var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                        int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                        string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                                        if (StatusCode1 == 1)
                                        {
                                            BindTripEndGrid();
                                            lblStartResponse.Visible = false;
                                            lblRowerResponse.Text = "";
                                            lblRowerResponse.Visible = false;
                                            divFinalEndStatus.Visible = true;
                                            //divGridStart.Visible = false;
                                            //divTripStarted.Visible = false;
                                            divGridStart.Style.Add("display", "none");
                                            divTripStarted.Style.Add("display", "none");
                                            divFinalStatus.Visible = false;
                                            divAlreadyEndStatus.Visible = false;
                                            GvStartedList.Visible = false;
                                            gvTripSheetSettelementEnd.Visible = false;
                                            header.Visible = false;
                                            headerend.Visible = false;
                                            txtStartDetails.Text = "";
                                            ImgBoatType.Visible = false;
                                            imgEnd.Visible = true;
                                            txtStartDetails.Focus();

                                            if (Rower != "0")
                                            {
                                                divEndRower.Visible = true;
                                            }
                                            else
                                            {
                                                divEndRower.Visible = false;
                                            }

                                            string Display = ResponseMsg1.ToString().Trim();
                                            string[] TimeList = Display.Split(',');
                                            lblEndBookingId.Text = TimeList[0].ToString().Trim();
                                            lblEndBookingpin.Text = TimeList[1].ToString().Trim();
                                            lblEndDuration.Text = ViewState["BookingDuration"].ToString().Trim();
                                            lblEndTravelledMins.Text = ViewState["TravelledMins"].ToString().Trim();
                                            lblEndStartTime.Text = ViewState["TripStartTime"].ToString().Trim();
                                            lblFinalEndTime.Text = TimeList[2].ToString().Trim();
                                            lblEndBoatType.Text = ViewState["EndBoatType"].ToString().Trim();
                                            lblEndBoatSeater.Text = ViewState["EndBoatSeater"].ToString().Trim();
                                            lblEndRowerName.Text = RowerName.ToString().Trim();
                                            lblEndStatus.Text = TimeList[3].ToString().Trim();

                                        }
                                        else
                                        {
                                            lblStartResponse.Text = ResponseMsg1.ToString();
                                            //return;
                                            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        lblStartResponse.Text = response1.ReasonPhrase.ToString();
                                        //return;
                                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                                    }

                                    AmountAfterExtension();
                                    string BookId = ViewState["BookingId"].ToString().Trim();
                                    string BookingPin = ViewState["BookingPin"].ToString().Trim();
                                    string BoatType = ViewState["EndBoatType"].ToString().Trim();
                                    string BoatSeater = ViewState["EndBoatSeater"].ToString().Trim();
                                    string BookingDate = ViewState["BDate"].ToString().Trim();
                                    //string BookingDuration = ViewState["BookingDuration"].ToString().Trim();
                                    string StartTime = ViewState["TripStartTime"].ToString().Trim();
                                    string EndTime = ViewState["TripEndTime"].ToString().Trim();
                                    string TotalDuration = ViewState["ExtensionDuration"].ToString().Trim();
                                    string ExtraBoatCharge = ViewState["ExtraBoatCharge"].ToString().Trim();
                                    string ExtraRowerCharge = ViewState["ExtraRowerCharge"].ToString().Trim();
                                    string ExtraNetAmount = ViewState["ExtraNetAmountForNotAllowed"].ToString().Trim();
                                    string Tax = ViewState["Tax"].ToString().Trim();
                                    string ActualNetAmount = ViewState["ActualNetAmount"].ToString().Trim();

                                    //if (Convert.ToDecimal(ExtraBoatCharge) > 0 && Convert.ToDecimal(ExtraRowerCharge) > 0 && Convert.ToDecimal(ExtraNetAmount) > 0)
                                    if (Convert.ToDecimal(ExtraBoatCharge) > 0 && Convert.ToDecimal(ExtraNetAmount) > 0)
                                    {
                                        if (Session["ExtensionPrint"].ToString().Trim() == "Y")
                                        {
                                            Response.Redirect("~/Boating/PrintExtraCharge?rte=rRefssse&bi=&BId=" + BookId.Trim() + "&BPin=" + BookingPin.Trim() +
                                          "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + BookingDuration.Trim() +
                                          "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BEBC=" + ExtraBoatCharge.Trim() +
                                          "&BERC=" + ExtraRowerCharge.Trim() + "&BENC=" + ExtraNetAmount.Trim() + "&Tax=" + Tax.Trim() + "&ANA=" + ActualNetAmount.Trim() + " ");
                                        }
                                    }
                                    divFinalEndStatus.Style.Add("background-color", "#ff0000");
                                    MpeTrip.Hide();

                                    BindEndedTripList_New(Rower);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabel", "javascript:HideEndLabel();", true);
                                    if (ViewState["flag"].ToString().Trim() == "0")
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid(5);", true);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid(8);", true);
                                    }
                                }

                                if (ViewState["ExtensionType"].ToString().Trim() == "A")
                                {
                                    var vTripSheetSettlement1 = new TripSheetSettlement()
                                    {
                                        QueryType = "TripEnd",
                                        BookingId = BookingId.Trim(),
                                        BoatReferenceNo = BoatRefNo.Trim(),
                                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                        RowerId = Rower.Trim(),
                                        TripStartTime = "",//Start Time Empty
                                        TripEndTime = "",
                                        ActualBoatId = BoatId.Trim(),
                                        SSUserBy = Session["UserId"].ToString().Trim(),
                                        SDUserBy = Session["UserId"].ToString().Trim(),
                                        BookingMedia = "DW"
                                    };

                                    response1 = client.PostAsJsonAsync("NewTripSheetWeb/Update", vTripSheetSettlement1).Result;
                                    sMSG = "Inserted Successfully";

                                    if (response1.IsSuccessStatusCode)
                                    {
                                        var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                        int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                        string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                                        if (StatusCode1 == 1)
                                        {
                                            BindTripEndGrid();
                                            lblStartResponse.Visible = false;
                                            lblRowerResponse.Text = "";
                                            lblRowerResponse.Visible = false;
                                            divFinalEndStatus.Visible = true;
                                            divGridStart.Style.Add("display", "none");
                                            divTripStarted.Style.Add("display", "none");
                                            //divGridStart.Visible = false;
                                            //divTripStarted.Visible = false;
                                            divFinalStatus.Visible = false;
                                            divAlreadyEndStatus.Visible = false;
                                            GvStartedList.Visible = false;
                                            gvTripSheetSettelementEnd.Visible = false;
                                            header.Visible = false;
                                            headerend.Visible = false;
                                            txtStartDetails.Text = "";
                                            ImgBoatType.Visible = false;
                                            imgEnd.Visible = true;
                                            txtStartDetails.Focus();

                                            if (Rower != "0")
                                            {
                                                divEndRower.Visible = true;
                                            }
                                            else
                                            {
                                                divEndRower.Visible = false;
                                            }

                                            string Display = ResponseMsg1.ToString().Trim();
                                            string[] TimeList = Display.Split(',');
                                            lblEndBookingId.Text = TimeList[0].ToString().Trim();
                                            lblEndBookingpin.Text = TimeList[1].ToString().Trim();
                                            lblEndDuration.Text = ViewState["BookingDuration"].ToString().Trim();
                                            lblEndTravelledMins.Text = ViewState["TravelledMins"].ToString().Trim();
                                            lblEndStartTime.Text = ViewState["TripStartTime"].ToString().Trim();
                                            lblFinalEndTime.Text = TimeList[2].ToString().Trim();
                                            lblEndBoatType.Text = ViewState["EndBoatType"].ToString().Trim();
                                            lblEndBoatSeater.Text = ViewState["EndBoatSeater"].ToString().Trim();
                                            lblEndRowerName.Text = RowerName.ToString().Trim();
                                            lblEndStatus.Text = TimeList[3].ToString().Trim();
                                        }
                                        else
                                        {
                                            lblStartResponse.Text = ResponseMsg1.ToString();
                                            //return;
                                            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        lblStartResponse.Text = response1.ReasonPhrase.ToString();
                                        //return;
                                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                                    }

                                    AmountAfterExtension();
                                    string BookId = ViewState["BookingId"].ToString().Trim();
                                    string BookingPin = ViewState["BookingPin"].ToString().Trim();
                                    string BoatType = ViewState["EndBoatType"].ToString().Trim();
                                    string BoatSeater = ViewState["EndBoatSeater"].ToString().Trim();
                                    string BookingDate = ViewState["BDate"].ToString().Trim();
                                    //string BookingDuration = ViewState["BookingDuration"].ToString().Trim();
                                    string StartTime = ViewState["TripStartTime"].ToString().Trim();
                                    string EndTime = ViewState["TripEndTime"].ToString().Trim();
                                    string TotalDuration = ViewState["ExtensionDuration"].ToString().Trim();
                                    string ExtraBoatCharge = ViewState["ExtraBoatCharge"].ToString().Trim();
                                    string ExtraRowerCharge = ViewState["ExtraRowerCharge"].ToString().Trim();
                                    string ExtraNetAmount = ViewState["ExtraNetAmountForAllowed"].ToString().Trim();
                                    string Tax = ViewState["Tax"].ToString().Trim();
                                    string ActualNetAmount = ViewState["ActualNetAmount"].ToString().Trim();
                                    string BoatDeposit = ViewState["BoatDeposit"].ToString().Trim();
                                    string DepRefundAmount = ViewState["DepRefundAmount"].ToString().Trim();

                                    //if (Convert.ToDecimal(ExtraBoatCharge) > 0 && Convert.ToDecimal(ExtraRowerCharge) > 0 && Convert.ToDecimal(ExtraNetAmount) > 0)
                                    if (Convert.ToDecimal(ExtraBoatCharge) > 0 && Convert.ToDecimal(ExtraNetAmount) > 0)
                                    {
                                        if (Session["ExtensionPrint"].ToString().Trim() == "Y")
                                        {
                                            Response.Redirect("~/Boating/PrintExtraCharge?rte=rRefsssea&bi=&BId=" + BookId.Trim() + "&BPin=" + BookingPin.Trim() +
                                          "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + BookingDuration.Trim() +
                                          "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BEBC=" + ExtraBoatCharge.Trim() +
                                          "&BERC=" + ExtraRowerCharge.Trim() + "&BENC=" + ExtraNetAmount.Trim() + "&Tax=" + Tax.Trim() + "&ANA=" + ActualNetAmount.Trim() +
                                          "&BD=" + BoatDeposit.Trim() + "&DRA=" + DepRefundAmount.Trim() + " ");
                                        }
                                    }
                                    divFinalEndStatus.Style.Add("background-color", "#ff0000");
                                    MpeTrip.Hide();

                                    BindEndedTripList_New(Rower);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabel", "javascript:HideEndLabel();", true);
                                    if (ViewState["flag"].ToString().Trim() == "0")
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid(5);", true);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid(8);", true);
                                    }

                                }

                            }
                            else
                            {
                                var vTripSheetSettlement1 = new TripSheetSettlement()
                                {
                                    QueryType = "TripEnd",
                                    BookingId = BookingId.Trim(),
                                    BoatReferenceNo = BoatRefNo.Trim(),
                                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                    RowerId = Rower.Trim(),
                                    TripStartTime = "",//Start Time Empty
                                    TripEndTime = "",
                                    ActualBoatId = BoatId.Trim(),
                                    SSUserBy = Session["UserId"].ToString().Trim(),
                                    SDUserBy = Session["UserId"].ToString().Trim(),
                                    BookingMedia = "DW"
                                };

                                response1 = client.PostAsJsonAsync("NewTripSheetWeb/Update", vTripSheetSettlement1).Result;
                                sMSG = "Inserted Successfully";

                                if (response1.IsSuccessStatusCode)
                                {
                                    var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                    int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                    string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                                    if (StatusCode1 == 1)
                                    {
                                        BindTripEndGrid();
                                        lblStartResponse.Visible = false;
                                        lblRowerResponse.Text = "";
                                        lblRowerResponse.Visible = false;
                                        divFinalEndStatus.Visible = true;
                                        divGridStart.Style.Add("display", "none");
                                        divTripStarted.Style.Add("display", "none");
                                        //divGridStart.Visible = false;
                                        //divTripStarted.Visible = false;
                                        divFinalStatus.Visible = false;
                                        divAlreadyEndStatus.Visible = false;
                                        GvStartedList.Visible = false;
                                        gvTripSheetSettelementEnd.Visible = false;
                                        header.Visible = false;
                                        headerend.Visible = false;
                                        txtStartDetails.Text = "";
                                        ImgBoatType.Visible = false;
                                        imgEnd.Visible = true;
                                        txtStartDetails.Focus();

                                        if (Rower != "0")
                                        {
                                            divEndRower.Visible = true;
                                        }
                                        else
                                        {
                                            divEndRower.Visible = false;
                                        }

                                        string Display = ResponseMsg1.ToString().Trim();
                                        string[] TimeList = Display.Split(',');
                                        lblEndBookingId.Text = TimeList[0].ToString().Trim();
                                        lblEndBookingpin.Text = TimeList[1].ToString().Trim();
                                        lblEndDuration.Text = ViewState["BookingDuration"].ToString().Trim();
                                        lblEndTravelledMins.Text = ViewState["TravelledMins"].ToString().Trim();
                                        lblEndStartTime.Text = ViewState["TripStartTime"].ToString().Trim();
                                        lblFinalEndTime.Text = TimeList[2].ToString().Trim();
                                        lblEndBoatType.Text = ViewState["EndBoatType"].ToString().Trim();
                                        lblEndBoatSeater.Text = ViewState["EndBoatSeater"].ToString().Trim();
                                        lblEndRowerName.Text = RowerName.ToString().Trim();
                                        lblEndStatus.Text = TimeList[3].ToString().Trim();
                                        divFinalEndStatus.Style.Add("background-color", "#ff0000");
                                        MpeTrip.Hide();

                                     
                                        BindEndedTripList_New(Rower);
                                        ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabel", "javascript:HideEndLabel();", true);
                                        if (ViewState["flag"].ToString().Trim() == "0")
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid(5);", true);
                                        }
                                        else
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid(8);", true);
                                        }
                                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                                    }
                                    else
                                    {
                                        lblStartResponse.Text = ResponseMsg1.ToString();
                                        return;
                                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                                    }
                                }
                                else
                                {
                                    lblStartResponse.Text = response1.ReasonPhrase.ToString();
                                    return;
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblStartResponse.Text = ex.ToString();
                            return;
                            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
                        }
                        return;
                    }
                    else
                    {
                        divGridStart.Visible = false;
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

    //newly added
    public void GetDurationGraceTime()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var body = new CommonClass()
                {
                    QueryType = "GetDurationGraceTime",
                    ServiceType = "",
                    Input1 = ViewState["BookingId"].ToString().Trim(),
                    Input2 = ViewState["BookingPin"].ToString().Trim(),
                    Input3 = ViewState["BoatReferenceNo"].ToString().Trim(),
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CommonReport", body).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {

                        ViewState["DurationGraceTime"] = dtExists.Rows[0]["Duration"].ToString();

                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //newly added
    public void CheckExtension()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var body = new CommonClass()
                {
                    QueryType = "GetExtensionType",
                    ServiceType = "",
                    Input1 = ViewState["BoatTypeId"].ToString().Trim(),
                    Input2 = ViewState["BoatSeaterId"].ToString().Trim(),
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CommonReport", body).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["ExtensionType"] = dtExists.Rows[0]["TimeExtension"].ToString().Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //newly added
    public void AmountAfterExtension()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var body = new CommonClass()
                {
                    QueryType = "GetChargeAfterExtension",
                    ServiceType = "",
                    Input1 = ViewState["BookingId"].ToString().Trim(),
                    Input2 = ViewState["BookingPin"].ToString().Trim(),
                    Input3 = ViewState["BoatReferenceNo"].ToString().Trim(),
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CommonReport", body).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        if (ViewState["ExtensionType"].ToString().Trim() == "N")
                        {
                            ViewState["BDate"] = dtExists.Rows[0]["BDate"].ToString();
                            ViewState["ExtraBoatCharge"] = dtExists.Rows[0]["ExtraBoatCharge"].ToString();
                            ViewState["ExtraRowerCharge"] = dtExists.Rows[0]["ExtraRowerCharge"].ToString();
                            ViewState["ExtraNetAmountForNotAllowed"] = dtExists.Rows[0]["ExtraNetAmountForNotAllowed"].ToString();
                            ViewState["ExtensionDuration"] = dtExists.Rows[0]["ExtensionDuration"].ToString();
                            ViewState["TripStartTime"] = dtExists.Rows[0]["TripStartTime"].ToString();
                            ViewState["TripEndTime"] = dtExists.Rows[0]["TripEndTime"].ToString();
                            ViewState["Tax"] = dtExists.Rows[0]["Tax"].ToString();
                            ViewState["ActualNetAmount"] = dtExists.Rows[0]["ActualNetAmount"].ToString();
                            ViewState["BoatDeposit"] = dtExists.Rows[0]["BoatDeposit"].ToString();
                            ViewState["DepRefundAmount"] = dtExists.Rows[0]["DepRefundAmount"].ToString();

                        }
                        if (ViewState["ExtensionType"].ToString().Trim() == "A")
                        {
                            ViewState["BDate"] = dtExists.Rows[0]["BDate"].ToString();
                            ViewState["ExtraBoatCharge"] = dtExists.Rows[0]["ExtraBoatCharge"].ToString();
                            ViewState["ExtraRowerCharge"] = dtExists.Rows[0]["ExtraRowerCharge"].ToString();
                            ViewState["ExtraNetAmountForAllowed"] = dtExists.Rows[0]["ExtraNetAmountForAllowed"].ToString();
                            ViewState["ExtensionDuration"] = dtExists.Rows[0]["ExtensionDuration"].ToString();
                            ViewState["TripStartTime"] = dtExists.Rows[0]["TripStartTime"].ToString();
                            ViewState["TripEndTime"] = dtExists.Rows[0]["TripEndTime"].ToString();
                            ViewState["Tax"] = dtExists.Rows[0]["Tax"].ToString();
                            ViewState["ActualNetAmount"] = dtExists.Rows[0]["ActualNetAmount"].ToString();
                            ViewState["BoatDeposit"] = dtExists.Rows[0]["BoatDeposit"].ToString();
                            ViewState["DepRefundAmount"] = dtExists.Rows[0]["DepRefundAmount"].ToString();

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

    protected void ImageHome_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Boating/NewDashboard.aspx");
    }
    /// <summary>
    /// CREATED DATE SURIYA, Abhinaya k
    /// CREATED DATE 21-APR-2022
    /// </summary>
    /// <param name="RowerId"></param>
    public void BindEndedTripList_New(string RowerId)
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
                var vTripSheetSettlement = new CommonClass()
                {
                    QueryType = "RowerLateRides",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = RowerId.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                response = client.PostAsJsonAsync("CommonReport", vTripSheetSettlement).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dt.Rows.Count > 0)
                    {
                        
                        gvExtendedRower.DataSource = dt;
                        gvExtendedRower.DataBind();
                        gvExtendedRower.Visible = true;
                        divExtendedRower.Style.Add("display", "block");
                        //headerend.Visible = true;
                        ViewState["flag"] = "1";
                    }
                    else
                    {
                        gvExtendedRower.DataBind();
                        gvExtendedRower.Visible = false;
                        divExtendedRower.Style.Add("display", "none");
                        ViewState["flag"] = "0";
                    }
                }
                else
                {
                    // headerend.Visible = false;
                    // var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }

            }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('"+ ex.ToString().Trim() + "');", true);
        }
    }
    public class TripSheetSettlement
    {
        public string QueryType { get; set; }
        public string BoatReferenceNo { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatHouseName { get; set; }
        public string RowerId { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string ActualBoatId { get; set; }
        public string BoatSeaterId { get; set; }
        public string BookingId { get; set; }
        public string BookingMedia { get; set; }
        public string BarcodePin { get; set; }
        public string UserId { get; set; }
        public string SSUserBy { get; set; }
        public string SDUserBy { get; set; }
        public string BookingPin { get; set; }
    }

    public class boatHouse
    {
        public string BoatHouseId { get; set; }
        public string BookingTo { get; set; }
    }

    public class QR
    {
        public string BoatHouseId { get; set; }
        public string BookingId { get; set; }
        public string Pin { get; set; }
        public string BookingRef { get; set; }
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

    public void GetExtensionPrint()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var body = new CommonClass()
                {
                    QueryType = "GetExtensionPrint",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CommonReport", body).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        Session["ExtensionPrint"] = dtExists.Rows[0]["ExtensionPrint"].ToString();

                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void autoTripEndDetails(string RowerId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var body = new CommonClass()
                {
                    QueryType = "AutoTripEndDetails",
                    ServiceType = "",
                    Input1 = RowerId.ToString().Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CommonReport", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        ViewState["BookingDurationTripEnd"] = dtExists.Rows[0]["BookingDuration"].ToString();
                        ViewState["TripStartTimeTripEnd"] = dtExists.Rows[0]["TripStartTime"].ToString();
                        ViewState["BoatDeposit"] = dtExists.Rows[0]["BoatDeposit"].ToString();
                        ViewState["AutoEndForNoDepositeTripEnd"] = dtExists.Rows[0]["AutoEndForNoDeposite"].ToString();
                    }
                    else
                    {
                        ViewState["BookingDurationTripEnd"] = "0";
                        ViewState["TripStartTimeTripEnd"] = "0";
                        ViewState["BoatDeposit"] = "0";
                        ViewState["AutoEndForNoDepositeTripEnd"] = "0";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ChkIndTrpSht_CheckedChanged(object sender, EventArgs e)
    {

        if (ChkIndTrpSht.Checked == true)
        {
            Response.Redirect("IndividualSmartTripSheetWeb.aspx", true);

        }
        else
        {
            Response.Redirect("IndividualSmartTripSheetWeb.aspx", false);

        }

    }

}