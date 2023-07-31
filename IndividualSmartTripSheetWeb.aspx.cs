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
/// <summary>
/// Created By Abhinaya
/// Date 21.08.2021
/// </summary>
public partial class IndividualSmartTripSheetWeb : System.Web.UI.Page
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
                txtBoatDetails.Focus();
                txtStartDetails.Visible = false;
                BindStartedTripList();
                BindEndedTripList();
                BindTripCountDetails();
                divTripStarted.Visible = true;
                divGridStart.Visible = true;
                divFinalStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divAlreadyEndStatus.Visible = false;
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    /// <summary>
    /// Boat Text Box Change Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtBoatDetails_TextChanged(object sender, EventArgs e)
    {
        hfBarcodePin.Value = "";
        hfBarcodeBoatCount.Value = "";
        hfActualBoatId.Value = "";
        if (txtBoatDetails.Text.Length != 0)
        {
            string BoatBarCode = txtBoatDetails.Text;
            string[] BoatCount = BoatBarCode.Split(';');
            string[] Value;

            if (BoatCount.Count() == 5 && BoatCount[0].Length > 1)
            {
                hfActualBoatId.Value = BoatCount[3].ToString();
                AutoTRipEnd();
                txtStartDetails.Attributes.Add("style", "display:inline-block");
                Value = BoatCount[4].Split('-');
                hfBarcodeBoatCount.Value = Value[0].ToString();
                txtStartDetails.Visible = true;
                MpeTrip.Show();
                lblBoatReponse.Visible = true;
                lblBoatReponse.Text = " Scan Ticket QR Code !!!";
                lblStartResponse.Text = "";
                lblStartResponse.Visible = false;
                txtStartDetails.Text = "";
                txtStartDetails.Focus();
                txtBoatDetails.Visible = false;
                txtBoatDetails.Text = "";
                BindStartedTripList();
                BindEndedTripList();
                BindTripCountDetails();
                divTripStarted.Visible = true;
                divGridStart.Visible = true;
                divFinalStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divAlreadyEndStatus.Visible = false;
                return;
            }
            else
            {
                MpeTrip.Show();
                lblStartResponse.Text = "Invalid QRCode !!! Scan Boat Id !!!";
                lblStartResponse.Visible = true;
                lblBoatReponse.Visible = false;
                lblBoatReponse.Text = "";
                txtBoatDetails.Text = "";
                txtBoatDetails.Focus();
                txtBoatDetails.Visible = true;
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
    /// <summary>
    /// WHen Boat Again scanning MEans TRip end Time Willbe Automatically Updated
    /// </summary>
    public void AutoTRipEnd()
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

                var vTripSheetSettlement2 = new TripSheetSettlementNew()
                {
                    QueryType = "AutoTripEndIndividual",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BarcodePin = "0",
                    BookingId = "0",
                    BoatReferenceNo = "0",
                    ActualBoatId = hfActualBoatId.Value,
                    SDUserBy = Session["UserId"].ToString().Trim(),
                    BookingMedia = "DW"
                };
                response = client.PostAsJsonAsync("TripEnd/Individual", vTripSheetSettlement2).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse1 = response.Content.ReadAsStringAsync().Result;
                    int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse1)["StatusCode"].ToString());
                    string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();
                    if (StatusCode1 == 1)
                    {
                        divTripStarted.Visible = false;
                        GvStartedList.Visible = false;
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

    /// <summary>
    /// This method is used for Trip start Change Event 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtStartDetails_TextChanged(object sender, EventArgs e)

    {
        hfBarcodePinRowerId.Value = "";

        if (txtStartDetails.Text.Length != 0)
        {

            string BarCode = txtStartDetails.Text;
            string[] TimeList = BarCode.Split(';');
            string BarcodepinCount;
            string[] sBarcodepinCount;
            if (TimeList.Count() == 4 && TimeList[0].Length > 1)
            {
                hfBarcodePin.Value = TimeList[3].ToString();

                hfBookingId.Value = TimeList[1].ToString();
                hfBoatRefNo.Value = TimeList[2].ToString();
                ViewState["BarcodepinCount"] += hfBarcodePin.Value + ",";

                BarcodepinCount = ViewState["BarcodepinCount"].ToString();
                sBarcodepinCount = BarcodepinCount.Split(',');
                if (Convert.ToInt32(hfBarcodeBoatCount.Value) == sBarcodepinCount.Count() - 1)
                {
                    GenerateBarCode();
                    return;
                }

                else
                {

                    txtStartDetails.Text = "";
                    txtStartDetails.Focus();
                    txtBoatDetails.Visible = false;
                    txtBoatDetails.Text = "";
                    txtStartDetails.Visible = true;
                    return;

                }
            }
            else if (TimeList[0].Length > 1 && TimeList.Count() == 5)
            {
                txtStartDetails.Focus();
                txtStartDetails.Visible = true;
                MpeTrip.Show();
                lblStartResponse.Text = "";
                txtStartDetails.Text = "";
                lblBoatReponse.Text = "";
                lblBoatReponse.Visible = false;
                lblStartResponse.Visible = true;
                txtStartDetails.Attributes.Add("style", "display:inline-block");
                txtRowerDetails.Attributes.Add("style", "display:none");
                lblStartResponse.Text = "Already Scan Boat Now Scan Ticket QR Code";
                lblRowerResponse.Text = "";
                divAlreadyEndStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divFinalStatus.Visible = false;
                imgEnd.Visible = false;
                ImgBoatType.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideStartLabel();", true);
            }
            else if (TimeList[0].Length == 1 && TimeList.Count() == 4)
            {

                if (hfBarcodePin.Value != "")
                {

                    BarCode = txtStartDetails.Text;
                    TimeList = BarCode.Split(';');
                    if (TimeList[0].Length == 1 && TimeList.Count() == 4)
                    {
                        ViewState["RowerBoatHouseId"] = TimeList[1].ToString();
                        hfBarcodePinRowerId.Value = TimeList[2].ToString();
                        hfBarcodePinRowerName.Value = TimeList[3].ToString();
                        GenerateBarCode();


                    }
                }
                else
                {

                    txtStartDetails.Focus();
                    txtStartDetails.Visible = true;
                    MpeTrip.Show();
                    lblStartResponse.Text = "";
                    txtStartDetails.Text = "";
                    lblBoatReponse.Text = "";
                    lblBoatReponse.Visible = false;
                    lblStartResponse.Visible = true;
                    txtStartDetails.Attributes.Add("style", "display:inline-block");
                    txtRowerDetails.Attributes.Add("style", "display:none");
                    lblStartResponse.Text = "First you have to Scan Ticket QR !!!";
                    lblRowerResponse.Text = "";
                    divAlreadyEndStatus.Visible = false;
                    divFinalEndStatus.Visible = false;
                    divFinalStatus.Visible = false;
                    imgEnd.Visible = false;
                    ImgBoatType.Visible = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideStartLabel();", true);
                }
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



        }
        else
        {
            MpeTrip.Show();
            lblStartResponse.Text = "Invalid QRCode !!!";
            lblBoatReponse.Visible = false;
            lblBoatReponse.Text = "";
            txtStartDetails.Text = "";
            txtStartDetails.Focus();
            lblRowerResponse.Text = "";
            divAlreadyEndStatus.Visible = false;
            divFinalEndStatus.Visible = false;
            divFinalStatus.Visible = false;
            imgEnd.Visible = false;
            ImgBoatType.Visible = false;
            ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:PinBox();", true);
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
    /// <summary>
    ///  This method is used For Rower Change Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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


                BarCodeTripStart();
                BindStartedTripList();
                BindEndedTripList();
                return;

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
                                            lblStartResponse.Visible = false;
                                            lblRowerResponse.Visible = false;
                                            lblBoatReponse.Visible = false;
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
    /// <summary>
    ///  Checking when Given Booking Pin is Start Time And End Time Is null or not
    /// </summary>
    public void GenerateBarCode()
    {
        ViewState["TripstartBookingPin"] = "";
        ViewState["TripEndBookingPin"] = "";
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


                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        var vTripSheetSettlement1 = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = ViewState["BarcodepinCount"].ToString().Trim(),
                            UserId = "0"
                        };

                        response = client.PostAsJsonAsync("IndividualSmartTripStart", vTripSheetSettlement1).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement2 = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response = client.PostAsJsonAsync("UserBasedNewTripScanTripSheetStart", vTripSheetSettlement2).Result;
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            ViewState["BarcodepinCount"] = "";
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string TripStart = dt.Rows[i]["TripStartTime"].ToString();

                                if (TripStart == "")
                                {
                                    ViewState["TripstartBookingPin"] += dt.Rows[i]["BookingPin"].ToString() + ",";

                                }
                                string TripEnd = dt.Rows[i]["TripEndTime"].ToString();
                                if (TripEnd == "")

                                {
                                    if (ViewState["TripstartBookingPin"].ToString() != "")
                                    {
                                        ViewState["TripEndBookingPin"] += dt.Rows[i]["BookingPin"].ToString() + ",";
                                    }
                                    else
                                    {
                                        ViewState["TripstartBookingPin"] = "0";
                                    }



                                }

                                else
                                {
                                    BindTripEndGrid();
                                    lblStartResponse.Text = "";
                                    lblRowerResponse.Text = "";
                                    lblRowerResponse.Visible = false;
                                    imgEnd.Visible = false;
                                    ImgBoatType.Visible = false;
                                    lblAlreadyStatus.Text = "Trip Already Ended !!!";
                                    txtStartDetails.Text = "";
                                    divAlreadyEndStatus.Style.Add("background-color", "#98c8e6");
                                    gvTripSheetSettelementEnd.Visible = true;
                                    txtBoatDetails.Focus();
                                    txtBoatDetails.Text = "";
                                    lblStartResponse.Visible = false;
                                    header.Visible = true;
                                    GvStartedList.Visible = true;
                                    divFinalStatus.Visible = false;
                                    divFinalEndStatus.Visible = false;
                                    divAlreadyEndStatus.Visible = true;
                                    divTripStarted.Visible = true;
                                    MpeTrip.Hide();
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideAlreadyEndLabel", "javascript:HideAlreadyEndLabel();", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid();", true);
                                    BindStartedTripList();
                                    BindEndedTripList();
                                    return;
                                }

                            }




                            string TripEndBookingPin;
                            string[] sTripEndBookingPin;
                            TripEndBookingPin = ViewState["TripstartBookingPin"].ToString();
                            sTripEndBookingPin = TripEndBookingPin.Split(',');
                            if (sTripEndBookingPin.Count() - 1 > 0)
                            {
                                BarCodeTripStart();
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
                    lblBoatReponse.Visible = false;
                    lblBoatReponse.Text = "";
                    lblStartResponse.Text = "Scan Only Individual Tickets";
                    lblStartResponse.Visible = true;
                    txtStartDetails.Text = "";
                    txtStartDetails.Focus();
                    hfBarcodePin.Value = "";
                    lblRowerResponse.Text = "";
                    divAlreadyEndStatus.Visible = false;
                    divFinalEndStatus.Visible = false;
                    divFinalStatus.Visible = false;
                    imgEnd.Visible = false;
                    ImgBoatType.Visible = false;
                    ClientScript.RegisterStartupScript(Page.GetType(), "StartBox", "javascript:StartBox();", true);
                    ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideStartLabel();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg1.ToString().Trim() + "');", true);



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
                    var body = new IndividualTripSheet()
                    {
                        QueryType = "TripSheetCount",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = "0",
                        UserRole = "Admin",
                        RowerId = "0",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", body).Result;
                }
                else
                {
                    var body = new IndividualTripSheet()
                    {
                        QueryType = "TripSheetCount",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        UserRole = "User",
                        RowerId = "0",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", body).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var responseMsg = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt16(JObject.Parse(responseMsg)["StatusCode"].ToString());
                    string sResponseMsg = JObject.Parse(responseMsg)["DataTable"].ToString();
                    string sSuccessOrFailureMsg = JObject.Parse(responseMsg)["ResponseMsg"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(sResponseMsg);

                    if (dt.Rows.Count > 0)
                    {
                        lblAlreadyStartedCount.Text = "Trip Already Started :  " + dt.Rows[0]["TripSheetEndCount"].ToString().Trim();
                        lblAlreadyEndedCount.Text = "Trip Already Ended :  " + dt.Rows[0]["TripSheetClosedCount"].ToString().Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    /// <summary>
    /// Trip Start Gird
    /// </summary>
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
                    var vTripSheetSettlement = new IndividualTripSheet()
                    {
                        QueryType = "ScanTripStartAdmin",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = "0",
                        UserRole = "Admin",
                        RowerId = "0",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                    };

                    response = client.PostAsJsonAsync("IndividualTripSheet/View", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new IndividualTripSheet()
                    {
                        QueryType = "ScanTripStartUser",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        UserRole = "User",
                        RowerId = "0",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                    };

                    response = client.PostAsJsonAsync("IndividualTripSheet/View", vTripSheetSettlement).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    var responseMsg = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt16(JObject.Parse(responseMsg)["StatusCode"].ToString());
                    string sResponseMsg = JObject.Parse(responseMsg)["DataTable"].ToString();
                    string sSuccessOrFailureMsg = JObject.Parse(responseMsg)["ResponseMsg"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(sResponseMsg);

                    if (StatusCode == 1)
                    {

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
    /// <summary>
    /// Trip End Grid
    /// </summary>
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
                    var vTripSheetSettlement = new IndividualTripSheet()
                    {
                        QueryType = "ScanTripEndAdmin",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = "0",
                        UserRole = "Admin",
                        RowerId = "0",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                    };

                    response = client.PostAsJsonAsync("IndividualTripSheet/View", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new IndividualTripSheet()
                    {
                        QueryType = "ScanTripSEndUser",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        UserRole = "User",
                        RowerId = "0",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                    };

                    response = client.PostAsJsonAsync("IndividualTripSheet/View", vTripSheetSettlement).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    var responseMsg = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt16(JObject.Parse(responseMsg)["StatusCode"].ToString());
                    string sResponseMsg = JObject.Parse(responseMsg)["DataTable"].ToString();
                    string sSuccessOrFailureMsg = JObject.Parse(responseMsg)["ResponseMsg"].ToString();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(sResponseMsg);

                    if (StatusCode == 1)
                    {

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
    /// <summary>
    ///  This Method is used for Trip Start
    /// </summary>
    public void BarCodeTripStart()
    {
        try
        {
            hfBoatTypeId.Value = "";
            hfBoatSeaterId.Value = "";
            string BookingPin = string.Empty;
            string BoatRefNo = string.Empty;


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
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BarcodePin = ViewState["TripstartBookingPin"].ToString().Trim(),
                        UserId = "0"

                    };

                    response = client.PostAsJsonAsync("IndividualSmartTripStart", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BarcodePin = ViewState["TripstartBookingPin"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("UserBasedNewTripScanTripSheetStart", vTripSheetSettlement).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            BookingPin += dt.Rows[i]["BookingId"].ToString() + ",";
                            BoatRefNo += dt.Rows[i]["BoatReferenceNo"].ToString() + ",";

                        }

                        try
                        {
                            HttpResponseMessage response1;
                            string sMSG = string.Empty;
                            string Rower = string.Empty;
                            if (hfBarcodePinRowerId.Value == "")
                            {
                                txtRowerDetails.Focus();
                                txtRowerDetails.Visible = true;
                                txtStartDetails.Visible = false;
                                txtBoatDetails.Visible = false;
                                txtBoatDetails.Text = "";
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
                                txtStartDetails.Attributes.Add("style", "display:none");
                                txtStartDetails.Text = "";
                                lblStartResponse.Visible = false;
                                lblBoatReponse.Text = "";
                                //lblBoatReponse.Visible = tre;
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelRower", "javascript:HideLabelRower();", true);
                                //ClientScript.RegisterStartupScript(Page.GetType(), "HideModalPopup", "javascript:HideModalPopup();", true);
                                return;
                            }
                            else
                            {
                                string RowerId = hfBarcodePinRowerId.Value.Trim();

                                string RowerName = hfBarcodePinRowerName.Value.Trim();// New changes 

                                if (RowerId == "")
                                {
                                    Rower = "0";
                                }
                                else
                                {
                                    Rower = RowerId;
                                    hfBoatTypeId.Value = dt.Rows[0]["BoatTypeId"].ToString();
                                    hfBoatSeaterId.Value = dt.Rows[0]["BoatSeaterId"].ToString();
                                    getRowerBoatAssign();

                                }
                            }
                            string BoatId = hfActualBoatId.Value;


                            string TripBookingPin;
                            string[] sTripBookingPin;
                            TripBookingPin = BookingPin.ToString();
                            sTripBookingPin = TripBookingPin.Split(',');
                            string BoatReferenceNo;
                            string[] sBoatReferenceNo;
                            BoatReferenceNo = BoatRefNo.ToString();
                            sBoatReferenceNo = BoatReferenceNo.Split(',');



                            var vTripSheetSettlement1 = new TripSheetSettlement()
                            {
                                QueryType = "IndividualTripStart",
                                BookingId = sTripBookingPin,
                                BoatReferenceNo = sBoatReferenceNo,
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

                            response1 = client.PostAsJsonAsync("InividualNewTripSheetWeb", vTripSheetSettlement1).Result;
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

                                    string Display = ResponseMsg1.ToString().Trim();
                                    string[] TimeList = Display.Split(',');
                                    lblStartStatus.Text = "Trip Started Successfully !!!";
                                    divFinalStatus.Style.Add("background-color", "#006400");
                                    hfBarcodePinRowerName.Value = "";

                                    txtEndDetails.Focus();
                                    txtEndDetails.Visible = true;
                                    txtRowerDetails.Visible = false;
                                    txtStartDetails.Visible = false;
                                    txtBoatDetails.Visible = false;
                                    txtBoatDetails.Text = "";
                                    txtEndDetails.Attributes.Add("style", "display:inline-block");
                                    //  lblStartResponse.Visible = true;
                                    // MpeTrip.Show();
                                    // lblStartResponse.Text = "Scan QR Code !!!";
                                    lblStartResponse.Text = "";
                                    txtRowerDetails.Text = "";
                                    divAlreadyEndStatus.Visible = false;
                                    divFinalEndStatus.Visible = false;
                                    divFinalStatus.Visible = true;
                                    imgEnd.Visible = false;
                                    ImgBoatType.Visible = true;
                                    txtStartDetails.Attributes.Add("style", "display:none");
                                    txtRowerDetails.Attributes.Add("style", "display:none");
                                    txtBoatDetails.Attributes.Add("style", "display:none");
                                    txtStartDetails.Text = "";
                                    lblRowerResponse.Visible = false;
                                    MpeTrip.Hide();

                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideStartLabel", "javascript:HideLabelGrid();", true);
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


    /// <summary>
    /// This Method is used For trip end
    /// </summary>
    public void GetTRipMethod()
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
                HttpResponseMessage response1;


                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var vTripSheetSettlement2 = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BarcodePin = hfBarcodePin.Value.Trim()
                    };
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

                        string TripEnd = dt.Rows[0]["TripEndTime"].ToString();
                        string TripstartEnd = dt.Rows[0]["TripStartTime"].ToString();
                        if (TripstartEnd == "")
                        {
                            txtEndDetails.Text = "";
                            txtEndDetails.Focus();
                            txtEndDetails.Visible = true;
                            lblStartResponse.Visible = true;
                            lblStartResponse.Attributes.Add("style", "display:block");
                            lblStartResponse.Text = "Trip Is Not Started Scan Started QR Code";
                            txtStartDetails.Attributes.Add("style", "display:none");
                            txtStartDetails.Visible = false;
                            lblBoatReponse.Visible = false;
                            lblBoatReponse.Text = "";
                            txtBoatDetails.Visible = false;
                            txtBoatDetails.Text = "";
                            BindStartedTripList();
                            BindEndedTripList();
                            BindTripCountDetails();
                            divTripStarted.Visible = true;
                            divGridStart.Visible = true;
                            divFinalStatus.Visible = false;
                            divFinalEndStatus.Visible = false;
                            divAlreadyEndStatus.Visible = false;
                            return;
                        }
                        if (TripEnd == "")
                        {
                            var vTripSheetSettlement2 = new TripSheetSettlementNew()
                            {
                                @QueryType = "TripEndIndividual",
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BarcodePin = hfBarcodePin.Value.Trim(),
                                BookingId = hfBookingId.Value.Trim(),
                                BoatReferenceNo = hfBoatRefNo.Value,
                                ActualBoatId = "0",
                                SDUserBy = Session["UserId"].ToString().Trim(),
                                BookingMedia = "DW"
                            };
                            response1 = client.PostAsJsonAsync("TripEnd/Individual", vTripSheetSettlement2).Result;
                            if (response1.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse1)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();
                                if (StatusCode1 == 1)
                                {
                                    string Display = ResponseMsg1.ToString().Trim();
                                    string[] TimeList = Display.Split(',');

                                    lblEndStatus.Text = "Trip Ended SuccessFully !!!";
                                    divFinalEndStatus.Style.Add("background-color", "#ff0000");
                                    GvStartedList.Visible = false;
                                    BindEndedTripList();
                                    txtBoatDetails.Focus();
                                    txtBoatDetails.Visible = true;
                                    txtRowerDetails.Visible = false;
                                    txtStartDetails.Visible = false;
                                    txtBoatDetails.Text = "";
                                    txtBoatDetails.Attributes.Add("style", "display:inline-block");
                                    //lblStartResponse.Visible = true;
                                    // MpeTrip.Show();
                                    // lblStartResponse.Text = "Scan QR Code !!!";
                                    txtRowerDetails.Text = "";
                                    txtEndDetails.Text = "";
                                    divAlreadyEndStatus.Visible = false;
                                    divFinalEndStatus.Visible = true;
                                    divFinalStatus.Visible = false;
                                    imgEnd.Visible = true;
                                    ImgBoatType.Visible = false;
                                    txtStartDetails.Attributes.Add("style", "display:none");
                                    txtRowerDetails.Attributes.Add("style", "display:none");
                                    txtEndDetails.Attributes.Add("style", "display:none");
                                    txtStartDetails.Text = "";
                                    lblRowerResponse.Visible = false;
                                    hfBarcodePinRowerName.Value = "";
                                    MpeTrip.Hide();
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabel", "javascript:HideEndLabel();", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid();", true);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            BindTripEndGrid();
                            lblStartResponse.Text = "";
                            lblRowerResponse.Text = "";
                            lblRowerResponse.Visible = false;
                            imgEnd.Visible = false;
                            ImgBoatType.Visible = false;
                            txtEndDetails.Text = "";
                            lblAlreadyStatus.Text = "Trip Already Ended !!!";
                            txtStartDetails.Text = "";
                            txtBoatDetails.Text = "";
                            txtEndDetails.Visible = false;
                            divAlreadyEndStatus.Style.Add("background-color", "#98c8e6");
                            gvTripSheetSettelementEnd.Visible = true;
                            txtBoatDetails.Focus();
                            txtBoatDetails.Visible = true;
                            lblStartResponse.Visible = false;
                            header.Visible = true;
                            GvStartedList.Visible = true;
                            divFinalStatus.Visible = false;
                            divFinalEndStatus.Visible = false;
                            divAlreadyEndStatus.Visible = true;
                            divTripStarted.Visible = true;
                            MpeTrip.Hide();
                            ClientScript.RegisterStartupScript(Page.GetType(), "HideAlreadyEndLabel", "javascript:HideAlreadyEndLabel();", true);
                            ClientScript.RegisterStartupScript(Page.GetType(), "HideEndLabelGrid", "javascript:HideEndLabelGrid();", true);
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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    /// <summary>
    /// Trip end QrCode Button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtEndDetails_TextChanged(object sender, EventArgs e)
    {
        hfBarcodePin.Value = "";

        hfBookingId.Value = "";
        hfBoatRefNo.Value = "";
        if (txtEndDetails.Text.Length != 0)
        {

            string BarCodes = txtEndDetails.Text;
            string[] TimeLists = BarCodes.Split(';');

            if (TimeLists.Count() == 4 && TimeLists[0].Length > 1)
            {
                hfBarcodePin.Value = TimeLists[3].ToString();

                hfBookingId.Value = TimeLists[1].ToString();
                hfBoatRefNo.Value = TimeLists[2].ToString();
                GetTRipMethod();
                return;
            }
            else if (TimeLists.Count() == 5 && TimeLists[0].Length > 1)
            {
                string[] Value;
                txtEndDetails.Visible = false;
                txtEndDetails.Text = "";
                hfActualBoatId.Value = TimeLists[3].ToString();
                AutoTRipEnd();
                txtStartDetails.Attributes.Add("style", "display:inline-block");
                Value = TimeLists[4].Split('-');
                hfBarcodeBoatCount.Value = Value[0].ToString();
                txtStartDetails.Visible = true;
                MpeTrip.Show();
                lblBoatReponse.Text = "Scan Ticket QR Code !!!";
                lblBoatReponse.Visible = true;
                lblStartResponse.Text = "";
                lblStartResponse.Visible = false;
                txtStartDetails.Text = "";
                txtStartDetails.Focus();
                txtBoatDetails.Visible = false;
                txtBoatDetails.Text = "";
                BindStartedTripList();
                BindEndedTripList();
                BindTripCountDetails();
                divTripStarted.Visible = true;
                divGridStart.Visible = true;
                divFinalStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divAlreadyEndStatus.Visible = false;
                return;
            }
            else
            {
                MpeTrip.Show();
                lblStartResponse.Attributes.Add("style", "display:block");
                lblStartResponse.Text = "Invalid QRCode !!! Scan Ticket QR Code";
                lblStartResponse.Visible = true;
                lblBoatReponse.Text = "";
                lblBoatReponse.Visible = false;
                txtEndDetails.Text = "";
                txtEndDetails.Focus();
                txtEndDetails.Visible = true;
                lblRowerResponse.Text = "";
                divAlreadyEndStatus.Visible = false;
                divFinalEndStatus.Visible = false;
                divFinalStatus.Visible = false;
                imgEnd.Visible = false;
                ImgBoatType.Visible = false;
                return;
            }
        }
    }



    protected void ImageHome_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Boating/NewDashboard.aspx");
    }

    public class TripSheetSettlement
    {
        public string QueryType { get; set; }
        public string[] BoatReferenceNo { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatHouseName { get; set; }
        public string RowerId { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string ActualBoatId { get; set; }
        public string BoatSeaterId { get; set; }
        public string[] BookingId { get; set; }
        public string BookingMedia { get; set; }
        public string BarcodePin { get; set; }
        public string UserId { get; set; }
        public string SSUserBy { get; set; }
        public string SDUserBy { get; set; }
    }
    public class IndividualTripSheet
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatTypeId { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string BoatSeaterId { get; set; }
        public string RowerId { get; set; }
        public string TripUser { get; set; }

    }

    public class TripSheetSettlementNew
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

    protected void BackToTripSheet_Click(object sender, EventArgs e)
    {
        Response.Redirect("ScanTripSheetWeb.aspx");
    }


}