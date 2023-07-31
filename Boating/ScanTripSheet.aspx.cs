using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_ScanTripSheet : System.Web.UI.Page
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
                hfCreatedBy.Value = Session["UserId"].ToString();
                hfBoathouseId.Value = Session["BoatHouseId"].ToString();
                hfBoathouseName.Value = Session["BoatHouseName"].ToString();

                if (Session["BBMTripSheetWeb"].ToString().Trim() == "Y")
                {
                    txtStartDetails.Focus();
                }

                BindStartedTripList();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
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
                hfBarcodePinRowerId.Value = TimeList[2].ToString();
                getRowerBoatAssign();
            }
            else if (TimeList[0].Length > 1 && TimeList.Count() == 4)
            {
                lblRowerResponse.Text = "This is Ticket QR, Scan in Ticket QR Box !!!";
                txtStartDetails.Focus();
                txtRowerDetails.Text = "";
                lblStartResponse.Text = "";
                txtStartDetails.Visible = true;
                txtRowerDetails.Visible = false;
                gvTripSheetSettelementStart.Visible = false;
                gvTripSheetSettelementEnd.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelRower();", true);
            }
            else
            {
                lblRowerResponse.Text = "Invalid BarCode !!!";
                txtRowerDetails.Text = "";
                txtRowerDetails.Focus();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelRower();", true);
            }

        }
    }
    public void getRowerBoatAssign()
    {
        DataTable dt = new DataTable();
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
                BoatSeaterId = hfBoatSeaterId.Value.Trim()

            };
            HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/RowerBoatAssign", BoatHouseId).Result;

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
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string RowerId = dt.Rows[i]["RowerId"].ToString();
                            if (RowerId == hfBarcodePinRowerId.Value)
                            {
                                txtRowerDetails.Text = "";
                                txtStartDetails.Focus();
                                GenerateBarCode();
                                return;
                            }
                            else
                            {
                                divGridStart.Visible = false;
                                txtRowerDetails.Text = "";
                                txtRowerDetails.Focus();
                                lblRowerResponse.Text = "This Rower Trip is not Ended or This Rower is not assigned to this BoatType !!!";
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelRower();", true);
                            }
                        }
                    }
                }
                //else
                //{
                //    txtRowerDetails.Text = "";
                //    txtRowerDetails.Focus();
                //    lblRowerResponse.Text = "This Rower Trip is not Ended Still !!!";
                //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelRower();", true);

                //}
            }
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
                GenerateBarCode();
                return;
            }
            else if (TimeList[0].Length == 1 && TimeList.Count() == 4)
            {
                lblStartResponse.Text = "First you have to Scan Ticket QR !!!";
                txtStartDetails.Focus();
                divGridStart.Visible = false;
                txtStartDetails.Text = "";
                lblRowerResponse.Text = "";
                gvTripSheetSettelementStart.Visible = false;
                gvTripSheetSettelementEnd.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
            else
            {
                lblStartResponse.Text = "Invalid BarCode !!!";
                txtStartDetails.Text = "";
                txtStartDetails.Focus();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
    }

    public void GenerateBarCode()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BarcodePin = hfBarcodePin.Value.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
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
                            return;
                        }
                        string TripEnd = dt.Rows[0]["TripEndTime"].ToString();
                        if (TripEnd == "")
                        {
                            BarCodeTripEnd();
                            return;
                        }
                        else
                        {
                            BindStartedTripList();
                            BindTripEndGrid();
                            lblStartResponse.Text = "";
                            lblStartResponse.Text = "Trip Already Ended !!!";
                            txtStartDetails.Text = "";
                            divGridStart.Style.Add("background-color", "#f29da7");
                            divGridStart.Visible = true;
                            gvTripSheetSettelementStart.Visible = false;
                            gvTripSheetSettelementEnd.Visible = true;
                            txtStartDetails.Focus();
                            lblStartResponse.Visible = true;
                            header.Visible = true;
                            GvStartedList.Visible = true;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            return;
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

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheetweb/ScanTripStartedList", vTripSheetSettlement).Result;
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
                            GvStartedList.DataSource = dt;
                            GvStartedList.DataBind();
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
    protected void GvStartedList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvStartedList.PageIndex = e.NewPageIndex;
        BindStartedTripList();
    }

    public void BindTripStartGrid()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BarcodePin = hfBarcodePin.Value.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheetweb/ScanTripStart", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        txtStartDetails.Text = string.Empty;
                        lblStartResponse.Text = string.Empty;
                        txtStartDetails.Visible = true;
                        lblStartResponse.Visible = true;

                        txtStartDetails.Focus();
                        divGridStart.Visible = true;
                        gvTripSheetSettelementStart.DataSource = dt;
                        gvTripSheetSettelementStart.DataBind();

                        gvTripSheetSettelementStart.Visible = true;
                        gvTripSheetSettelementEnd.Visible = false;
                        lblRowerResponse.Text = "";
                        hfBarcodePinRowerId.Value = "";
                        divGridStart.Style.Add("background-color", "#d6f2b8");
                        txtRowerDetails.Visible = false;
                        GvStartedList.Visible = true;
                        divTripStarted.Visible = true;
                        header.Visible = true;
                        ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                    }
                    else
                    {

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
            BindStartedTripList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BarcodePin = hfBarcodePin.Value.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheetweb/ScanTripStart", vTripSheetSettlement).Result;

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
                            hfBoatSeaterId.Value = dt.Rows[0]["BoatSeaterId"].ToString();
                            string RowerId = hfBarcodePinRowerId.Value.Trim();
                            if (RowerId == "")
                            {
                                Rower = "0";
                            }
                            else
                            {
                                Rower = RowerId;
                            }
                            string BoatId = dt.Rows[0]["BoatId"].ToString();
                            if (SelfDrive == "N" && Convert.ToDecimal(RowerCharge) > 0)
                            {
                                if (Convert.ToInt32(Rower) == 0)
                                {
                                    txtRowerDetails.Visible = true;
                                    divGridStart.Visible = false;
                                    lblRowerResponse.Text = "Scan Rower Id !!!";
                                    txtRowerDetails.Text = "";
                                    txtRowerDetails.Focus();
                                    txtStartDetails.Visible = false;
                                    txtStartDetails.Text = "";
                                    gvTripSheetSettelementStart.Visible = false;
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
                                BookingMedia = "DW"
                            };

                            response1 = client.PostAsJsonAsync("TripSheetWeb/Update", vTripSheetSettlement1).Result;
                            sMSG = "Inserted Successfully";

                            if (response1.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                                if (StatusCode1 == 1)
                                {
                                    BindTripStartGrid();
                                    lblStartResponse.Text = ResponseMsg1.ToString();
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
                        gvTripSheetSettelementStart.Visible = false;
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

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BarcodePin = hfBarcodePin.Value.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheetweb/ScanTripEndDuration", vTripSheetSettlement).Result;
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
                            txtStartDetails.Text = "";
                            lblStartResponse.Text = "";
                            txtStartDetails.Focus();
                            gvTripSheetSettelementEnd.DataSource = dt;
                            gvTripSheetSettelementEnd.DataBind();
                            divGridStart.Visible = true;
                            gvTripSheetSettelementEnd.Visible = true;
                            gvTripSheetSettelementStart.Visible = false;
                            lblRowerResponse.Text = "";
                            lblStartResponse.Visible = true;
                            divGridStart.Style.Add("background-color", "#f2f19b");
                            GvStartedList.Visible = false;
                            header.Visible = false;
                            hfBarcodePinRowerId.Value = "";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
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
            BindStartedTripList();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BarcodePin = hfBarcodePin.Value.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("TripSheetweb/ScanTripEnd", vTripSheetSettlement).Result;

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
                            string RowerId = dt.Rows[0]["RowerId"].ToString();
                            if (RowerId == "")
                            {
                                Rower = "0";
                            }
                            else
                            {
                                Rower = RowerId;
                            }
                            string BoatId = dt.Rows[0]["BoatId"].ToString();
                            string BookingDuration = dt.Rows[0]["BookingDuration"].ToString();
                            string TripStartTime = dt.Rows[0]["TripStartTime"].ToString();

                            double Duration = 0;
                            Duration = Math.Round(Convert.ToDouble(BookingDuration) / 3);
                            string Time = DateTime.Now.ToString("HH:mm");
                            string TimeTrip = DateTime.Now.ToString("HH:mm:ss");
                            DateTime d = DateTime.Parse(TripStartTime);
                            DateTime now = DateTime.Parse(d.ToString("HH:mm"));
                            DateTime nowTrip = DateTime.Parse(d.ToString("HH:mm:ss"));
                            DateTime modifiedDatetime = now.AddMinutes(Duration);
                            if (modifiedDatetime > DateTime.Parse(Time))
                            {
                                txtStartDetails.Text = "";
                                txtStartDetails.Focus();
                                divGridStart.Visible = false;
                                gvTripSheetSettelementEnd.Visible = false;
                                gvTripSheetSettelementStart.Visible = false;
                                lblStartResponse.Visible = true;
                                lblStartResponse.Text = "Trip Can't be Ended Now !!!";
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                                return;
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
                                    BookingMedia = "DW"
                                };

                                response1 = client.PostAsJsonAsync("TripSheetWeb/Update", vTripSheetSettlement1).Result;
                                sMSG = "Inserted Successfully";

                                if (response1.IsSuccessStatusCode)
                                {
                                    var vehicleEditresponse1 = response1.Content.ReadAsStringAsync().Result;
                                    int StatusCode1 = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                    string ResponseMsg1 = JObject.Parse(vehicleEditresponse1)["Response"].ToString();

                                    if (StatusCode1 == 1)
                                    {
                                        BindTripEndGrid();
                                        BindStartedTripList();
                                        lblStartResponse.Text = ResponseMsg1.ToString();
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
                        gvTripSheetSettelementStart.Visible = false;
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

}




