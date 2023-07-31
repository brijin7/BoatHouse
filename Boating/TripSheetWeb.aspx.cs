using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_TripSheetWeb_Test : System.Web.UI.Page
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
                ChkIndTrpSht.Checked = false;
                hfCreatedBy.Value = Session["UserId"].ToString();
                hfBoathouseId.Value = Session["BoatHouseId"].ToString();
                hfBoathouseName.Value = Session["BoatHouseName"].ToString();

                if (Session["BBMTripSheetWeb"].ToString().Trim() == "Y")
                {
                    //BindBoatHouse();

                    int istart;
                    int iend;
                    AddProcess(0, 10, out istart, out iend);

                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        divTripStart.Visible = true;
                        divTripEnd.Visible = true;
                        divTripClosed.Visible = true;
                        //int istart;
                        //int iend;
                        //AddProcess(0, 10, out istart, out iend);
                        TripSheetTripStart();
                    }
                    else
                    {
                        divTripStart.Visible = false;
                        divTripEnd.Visible = false;

                        if (Session["BBMTripSheetStart"].ToString().Trim() == "Y" && Session["BBMTripSheetEnd"].ToString().Trim() == "Y")
                        {
                            divTripStart.Visible = true;
                            divTripEnd.Visible = true;
                            TripSheetTripStart();
                        }
                        else if (Session["BBMTripSheetStart"].ToString().Trim() == "Y" && Session["BBMTripSheetEnd"].ToString().Trim() == "N")
                        {
                            divTripStart.Visible = true;
                            divTripEnd.Visible = false;
                            TripSheetTripStart();
                        }
                        else if (Session["BBMTripSheetStart"].ToString().Trim() == "N" && Session["BBMTripSheetEnd"].ToString().Trim() == "Y")
                        {
                            divTripStart.Visible = false;
                            divTripEnd.Visible = true;
                            TripSheetTripEnd();
                        }
                    }

                    BindTripCountDetails();
                    GetExtensionPrint();
                    txtStartDetails.Focus();

                    //int TripStartCount = Int32.Parse(ViewState["TripStartCount"].ToString().Trim()); 

                    //if (TripStartCount < 10)
                    //{
                    //    back.Enabled = false;
                    //}

                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    protected void gvTripSheetSettelement_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["TripUser"].ToString().Trim() == "D")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DropDownList ddlBoatNumber = (DropDownList)e.Row.FindControl("gvddlBoatNumber");
                    Label Bookingid = (Label)e.Row.FindControl("lblBookingId");
                    Label lblBoatReferenceNo = (Label)e.Row.FindControl("lblBoatReferenceNo");
                    Label UserId = (Label)e.Row.FindControl("lblUserId");
                    Label BoardingTime = (Label)e.Row.FindControl("lblBoardingTime");

                    Label PremiumStatus = (Label)e.Row.FindControl("lblPremiumStatus");
                    Label BookingDuration = (Label)e.Row.FindControl("lblBookingDuration");
                    Label BoatNum = (Label)e.Row.FindControl("lblBoatNum");
                    Label BookingPin = (Label)e.Row.FindControl("lblBookingPin");
                    Label lblBoatTypeId = (Label)e.Row.FindControl("lblBoatTypeId");

                    Label lblBoatType = (Label)e.Row.FindControl("lblBoatType");
                    Label lblBoatSeaterId = (Label)e.Row.FindControl("lblBoatSeaterId");
                    Label lblBoatSeater = (Label)e.Row.FindControl("lblBoatSeater");
                    Label lblBoatId = (Label)e.Row.FindControl("lblBoatId");
                    Label lblTripStartTime = (Label)e.Row.FindControl("lblTripStartTime");
                    Label lblSelfDrive = (Label)e.Row.FindControl("lblSelfDrive");
                    Label lblRowerCharge = (Label)e.Row.FindControl("lblRowerCharge");

                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    //Bind subcategories data to dropdownlist
                    ddlBoatNumber.DataTextField = "BoatNum";
                    ddlBoatNumber.DataValueField = "BoatId";
                    ddlBoatNumber.DataSource = getBoatNumber(lblBoatTypeId.Text.Trim(), lblBoatSeaterId.Text.Trim());
                    ddlBoatNumber.DataBind();
                    //DataRowView dr = e.Row.DataItem as DataRowView;
                    //ddlBoatNumber.SelectedValue = dr["BoatId"].ToString();
                    ddlBoatNumber.SelectedValue = lblBoatId.Text.ToString();

                    DropDownList ddlRowerId = (DropDownList)e.Row.FindControl("gvddlRowerId");
                    ddlRowerId.DataTextField = "RowerName";
                    ddlRowerId.DataValueField = "RowerId";
                    ddlRowerId.DataSource = getRower();
                    ddlRowerId.DataBind();
                    ddlRowerId.Items.Insert(0, new ListItem("Select", "0"));

                    if (lblSelfDrive.Text == "A" && Convert.ToDecimal(lblRowerCharge.Text) == 0)
                    {
                        ddlRowerId.Enabled = false;
                    }
                    else
                    {
                        ddlRowerId.Enabled = true;
                    }


                    // Image ImageData = (Image)e.Row.FindControl("imgOtherQRRc");

                    //using (var client = new HttpClient())
                    //{
                    //    client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                    //    client.DefaultRequestHeaders.Clear();
                    //    client.DefaultRequestHeaders.Accept.Clear();

                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //    var QRd = new QR()
                    //    {
                    //        BoatHouseId = Session["BoatHouseId"].ToString(),
                    //        BookingId = Bookingid.Text.Trim(),
                    //        Pin = BookingPin.Text.Trim(),
                    //        BookingRef = lblBoatReferenceNo.Text.Trim()

                    //    };

                    //    HttpResponseMessage response = client.PostAsJsonAsync("PassQR", QRd).Result;

                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    //        string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                    //        string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                    //        if (Status == "Success.")
                    //        {
                    //            ImageData.Visible = true;                                
                    //            ImageData.ImageUrl = ResponseMsg;

                    //        }
                    //        else
                    //        {
                    //            lblStartResponse.Text = ResponseMsg.ToString();

                    //        }
                    //    }
                    //    else
                    //    {
                    //        lblStartResponse.Text = response.StatusCode.ToString();

                    //    }
                    //}
                }
            }
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DropDownList ddlBoatNumber = (DropDownList)e.Row.FindControl("gvddlBoatNumber");
                    Label Bookingid = (Label)e.Row.FindControl("lblBookingId");
                    Label lblBoatReferenceNo = (Label)e.Row.FindControl("lblBoatReferenceNo");
                    Label UserId = (Label)e.Row.FindControl("lblUserId");
                    Label BoardingTime = (Label)e.Row.FindControl("lblBoardingTime");

                    Label PremiumStatus = (Label)e.Row.FindControl("lblPremiumStatus");
                    Label BookingDuration = (Label)e.Row.FindControl("lblBookingDuration");
                    Label BoatNum = (Label)e.Row.FindControl("lblBoatNum");
                    Label BookingPin = (Label)e.Row.FindControl("lblBookingPin");
                    Label lblBoatTypeId = (Label)e.Row.FindControl("lblBoatTypeId");
                    ViewState["BoatTypeIdRower"] = lblBoatTypeId.Text;

                    Label lblBoatType = (Label)e.Row.FindControl("lblBoatType");
                    Label lblBoatSeaterId = (Label)e.Row.FindControl("lblBoatSeaterId");
                    ViewState["BoatSeaterIdRower"] = lblBoatSeaterId.Text;
                    Label lblBoatSeater = (Label)e.Row.FindControl("lblBoatSeater");
                    Label lblBoatId = (Label)e.Row.FindControl("lblBoatId");
                    Label lblTripStartTime = (Label)e.Row.FindControl("lblTripStartTime");
                    Label lblSelfDrive = (Label)e.Row.FindControl("lblSelfDrive");
                    Label lblRowerCharge = (Label)e.Row.FindControl("lblRowerCharge");

                    DataRowView dr = (DataRowView)e.Row.DataItem;
                    //Bind subcategories data to dropdownlist
                    ddlBoatNumber.DataTextField = "BoatNum";
                    ddlBoatNumber.DataValueField = "BoatId";
                    ddlBoatNumber.DataSource = getBoatNumber(lblBoatTypeId.Text.Trim(), lblBoatSeaterId.Text.Trim());
                    ddlBoatNumber.DataBind();

                    ddlBoatNumber.SelectedValue = lblBoatId.Text.ToString();

                    DropDownList ddlRowerId = (DropDownList)e.Row.FindControl("gvddlRowerId");
                    ddlRowerId.DataTextField = "RowerName";
                    ddlRowerId.DataValueField = "RowerId";
                    ddlRowerId.DataSource = getRowerBoatAssign();
                    ddlRowerId.DataBind();
                    ddlRowerId.Items.Insert(0, new ListItem("Select", "0"));

                    if (lblSelfDrive.Text == "A" && Convert.ToDecimal(lblRowerCharge.Text) == 0)
                    {
                        ddlRowerId.Enabled = false;
                    }
                    else
                    {
                        ddlRowerId.Enabled = true;
                    }


                    //Image ImageData = (Image)e.Row.FindControl("imgOtherQRRc");

                    //using (var client = new HttpClient())
                    //{
                    //    client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                    //    client.DefaultRequestHeaders.Clear();
                    //    client.DefaultRequestHeaders.Accept.Clear();

                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //    var QRd = new QR()
                    //    {
                    //        BoatHouseId = Session["BoatHouseId"].ToString(),
                    //        BookingId = Bookingid.Text.Trim(),
                    //        Pin = BookingPin.Text.Trim(),
                    //        BookingRef = lblBoatReferenceNo.Text.Trim()

                    //    };

                    //    HttpResponseMessage response = client.PostAsJsonAsync("PassQR", QRd).Result;

                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    //        string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                    //        string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                    //        if (Status == "Success.")
                    //        {
                    //            ImageData.Visible = true;

                    //            ImageData.ImageUrl = ResponseMsg;

                    //        }
                    //        else
                    //        {
                    //            lblStartResponse.Text = ResponseMsg.ToString();

                    //        }
                    //    }
                    //    else
                    //    {
                    //        lblStartResponse.Text = response.StatusCode.ToString();

                    //    }
                    //}
                }
            }
        }
    }


    //protected void gvTripSheetSettelementEnd_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (e.Row.DataItem != null)
    //        {

    //            Label lblBookingId = (Label)e.Row.FindControl("lblBookingId");
    //            Label lblBoatReferenceNo = (Label)e.Row.FindControl("lblBoatReferenceNo");
    //            Label UserId = (Label)e.Row.FindControl("lblUserId");
    //            Label BoardingTime = (Label)e.Row.FindControl("lblBoardingTime");
    //            Label PremiumStatus = (Label)e.Row.FindControl("lblPremiumStatus");

    //            Label BookingDuration = (Label)e.Row.FindControl("lblBookingDuration");
    //            Label BoatNum = (Label)e.Row.FindControl("lblBoatNum");
    //            Label BookingPin = (Label)e.Row.FindControl("lblBookingPin");
    //            Label lblBoatTypeId = (Label)e.Row.FindControl("lblBoatTypeId");
    //            Label lblBoatType = (Label)e.Row.FindControl("lblBoatType");

    //            Label lblBoatSeaterId = (Label)e.Row.FindControl("lblBoatSeaterId");
    //            Label lblBoatSeater = (Label)e.Row.FindControl("lblBoatSeater");
    //            Label lblBoatId = (Label)e.Row.FindControl("lblBoatId");
    //            Label lblTripStartTime = (Label)e.Row.FindControl("lblTripStartTime");
    //            //DropDownList ddlTripEndTime = (DropDownList)e.Row.FindControl("gvddlTripEndTime");

    //            //DataTable dtt = new DataTable();
    //            //String hourMinute = DateTime.Now.ToString("HH:mm");
    //            //DateTime StartTime = DateTime.ParseExact(hourMinute, "HH:mm", null);

    //            //String BookingTo = DateTime.Now.ToString(hfBookingTo.Value);
    //            //DateTime EndTime = DateTime.ParseExact(BookingTo, "HH:mm", null);

    //            ////Set 1 minutes interval
    //            //TimeSpan Interval = new TimeSpan(0, 1, 0);
    //            //if (StartTime > EndTime)
    //            //{
    //            //    ddlTripEndTime.Items.Add(EndTime.ToShortTimeString().Trim().Replace(" ", ""));
    //            //}
    //            //else
    //            //{
    //            //    while (StartTime <= EndTime)
    //            //    {
    //            //        ddlTripEndTime.Items.Add(StartTime.ToShortTimeString().Trim().Replace(" ", ""));
    //            //        StartTime = StartTime.Add(Interval);
    //            //    }
    //            //}
    //            //ddlTripEndTime.SelectedValue = EndTime.ToShortTimeString().Trim().Replace(" ", "");

    //            Image ImageData = (Image)e.Row.FindControl("imgOtherQRRc");

    //            using (var client = new HttpClient())
    //            {
    //                client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
    //                client.DefaultRequestHeaders.Clear();
    //                client.DefaultRequestHeaders.Accept.Clear();

    //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //                var QRd = new QR()
    //                {
    //                    BoatHouseId = Session["BoatHouseId"].ToString(),
    //                    BookingId = lblBookingId.Text.Trim(),
    //                    Pin = BookingPin.Text.Trim(),
    //                    BookingRef = lblBoatReferenceNo.Text.Trim()
    //                };

    //                HttpResponseMessage response = client.PostAsJsonAsync("PassQR", QRd).Result;

    //                if (response.IsSuccessStatusCode)
    //                {
    //                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                    string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
    //                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

    //                    if (Status == "Success.")
    //                    {
    //                        ImageData.Visible = true;
    //                        //ImageData.ImageUrl = @"ImgPost/" + dr.Row["ImgPath"].ToString();
    //                        ImageData.ImageUrl = ResponseMsg;
    //                    }
    //                    else
    //                    {
    //                        lblEndResponse.Text = ResponseMsg.ToString();
    //                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
    //                    }
    //                }
    //                else
    //                {
    //                    lblEndResponse.Text = response.StatusCode.ToString();
    //                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
    //                }
    //            }
    //        }
    //    }
    //}

    protected void ImgBtnStart_Click(object sender, ImageClickEventArgs e)
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

                ImageButton lnkbtn = sender as ImageButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sTesfg = gvTripSheetSettelementStart.DataKeys[gvrow.RowIndex].Value.ToString();

                Label BookingId = (Label)gvrow.FindControl("lblBookingId");
                Label BoatReferenceNo = (Label)gvrow.FindControl("lblBoatReferenceNo");
                Label BookingPin = (Label)gvrow.FindControl("lblBookingPin");
                DropDownList ddlBoatName = (DropDownList)gvrow.FindControl("gvddlBoatNumber");
                DropDownList ddlRowerId = (DropDownList)gvrow.FindControl("gvddlRowerId");
                //DropDownList ddlTripStartTime = (DropDownList)gvrow.FindControl("gvddlTripStartTime");

                //string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                //string TripStart = CurrentDate + " " + ddlTripStartTime.SelectedItem.Text.Trim();

                Label lblSelfDrive = (Label)gvrow.FindControl("lblSelfDrive");
                Label lblRowerCharge = (Label)gvrow.FindControl("lblRowerCharge");
                //Newly added by Imran 0n 05-11-2021
                Label BoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
                Label BoatSeaterId = (Label)gvrow.FindControl("lblBoatSeaterId");

                if (lblSelfDrive.Text == "N" && Convert.ToDecimal(lblRowerCharge.Text) > 0)
                {
                    if (ddlRowerId.SelectedIndex == 0)
                    {
                        lblStartResponse.Text = "Select Rower Name";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Rower Name');", true);
                        return;
                    }
                }

                // Changes on 2021-04-21--------------------------------------------------------------------------------------------------//

                if (ddlRowerId.SelectedValue.Trim() != "0")
                {
                    autoTripEndDetails(ddlRowerId.SelectedValue.Trim());
                    GetMultipleTripRights(ddlRowerId.SelectedValue.Trim(), BoatTypeId.Text, BoatSeaterId.Text);
                    if (ViewState["AutoEndForNoDepositeTripEnd"].ToString().Trim() == "Y")
                    {
                        if (ViewState["MultipleTripRights"].ToString().Trim() == "N")
                        {
                            if (Convert.ToDecimal(ViewState["BoatDeposit"].ToString().Trim()) > 0)
                            {
                                divRowerInfoMsg.Visible = true;
                                lblStartResponse.Visible = false;
                                lblRowerInfoMsg.Text = "Rower-'" + ddlRowerId.SelectedItem.Text.Trim() + "' is Already On A Trip, Cannot Start Trip.";
                                return;
                            }
                            else
                            {
                                divRowerInfoMsg.Visible = true;
                                lblStartResponse.Visible = true;
                            }


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
                                lblRowerInfoMsg.Text = "Trip Cannot Be Started With  Rower-'" + ddlRowerId.SelectedItem.Text.Trim() + "', before-'" + FinalTripTime.ToString("hh:mm tt").Trim() + "'";
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

                // Changes End--------------------------------------------------------------------------------------------------//

                var vTripSheetSettlement = new TripSheetSettlement()
                {
                    QueryType = "TripStart",
                    BookingId = BookingId.Text.Trim(),
                    BoatReferenceNo = BoatReferenceNo.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    RowerId = ddlRowerId.SelectedValue.Trim(),
                    TripStartTime = "",
                    TripEndTime = "",//End Time Pas Empty
                    ActualBoatId = ddlBoatName.SelectedValue,
                    SSUserBy = Session["UserId"].ToString().Trim(),
                    SDUserBy = Session["UserId"].ToString().Trim(),
                    BookingMedia = "DW"
                };

                response = client.PostAsJsonAsync("NewTripSheetWeb/Update", vTripSheetSettlement).Result;
                sMSG = "Inserted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        TripSheetTripStart();
                        lblStartResponse.Text = ResponseMsg.ToString();
                        txtStartDetails.Focus();
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        lblStartResponse.Text = ResponseMsg.ToString();
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    lblStartResponse.Text = response.ReasonPhrase.ToString();
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblStartResponse.Text = ex.ToString();
            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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

    public void GetMultipleTripRights(string RowerId, string BoatTypeId, string BoatSeaterId)
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
                    QueryType = "GetMultipleAccessRights",
                    ServiceType = "",
                    Input1 = RowerId.ToString().Trim(),
                    Input2 = BoatTypeId.ToString().Trim(),
                    Input3 = BoatSeaterId.ToString().Trim(),
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

                        ViewState["MultipleTripRights"] = dtExists.Rows[0]["MultiTripRights"].ToString();
                    }
                    else
                    {

                        ViewState["MultipleTripRights"] = "0";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //protected void ImgBtnEnd_Click(object sender, ImageClickEventArgs e)
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
    //            string sMSG = string.Empty;

    //            ImageButton lnkbtn = sender as ImageButton;
    //            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
    //            string sTesfg = gvTripSheetSettelementEnd.DataKeys[gvrow.RowIndex].Value.ToString();

    //            Label BookingId = (Label)gvrow.FindControl("lblBookingId");
    //            Label BoatReferenceNo = (Label)gvrow.FindControl("lblBoatReferenceNo");

    //            Label lblBoatId = (Label)gvrow.FindControl("lblBoatId");

    //            Label lblRowerId = (Label)gvrow.FindControl("lblRowerId");

    //            Label lblTripStartTime = (Label)gvrow.FindControl("lblTripStartTime");
    //            Label lblBookingPin = (Label)gvrow.FindControl("lblBookingPin");
    //            Label lblBookingDuration = (Label)gvrow.FindControl("lblBookingDuration");

    //            double Duration = 0;
    //            Duration = Math.Round(Convert.ToDouble(lblBookingDuration.Text) / 2);
    //            string Time = DateTime.Now.ToString("HH:mm");
    //            DateTime d = DateTime.Parse(lblTripStartTime.Text);
    //            DateTime now = DateTime.Parse(d.ToString("HH:mm"));              
    //            DateTime modifiedDatetime = now.AddMinutes(Duration);                            
    //            if(modifiedDatetime > DateTime.Parse(Time))
    //            {
    //                lblEndResponse.Text = "Your Trip Cannot be Ended now";
    //                txtEndDetails.Text = "";
    //                txtEndDetails.Focus();
    //            }
    //            else
    //            {
    //                var vTripSheetSettlement = new TripSheetSettlement()
    //                {
    //                    QueryType = "TripEnd",
    //                    BookingId = BookingId.Text.Trim(),
    //                    BoatReferenceNo = BoatReferenceNo.Text.Trim(),
    //                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
    //                    RowerId = lblRowerId.Text.Trim(),
    //                    TripStartTime = "",//Start Time Empty
    //                    TripEndTime = "",
    //                    ActualBoatId = lblBoatId.Text.Trim(),
    //                    BookingMedia = "DW"
    //                };

    //                response = client.PostAsJsonAsync("TripSheetWeb/Update", vTripSheetSettlement).Result;
    //                sMSG = "Inserted Successfully";

    //                if (response.IsSuccessStatusCode)
    //                {
    //                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

    //                    if (StatusCode == 1)
    //                    {
    //                        TripSheetTripEnd();
    //                        lblEndResponse.Text = ResponseMsg.ToString();
    //                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
    //                    }
    //                    else
    //                    {
    //                        lblEndResponse.Text = ResponseMsg.ToString();
    //                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
    //                    }
    //                }
    //                else
    //                {
    //                    lblEndResponse.Text = response.ReasonPhrase.ToString();
    //                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
    //                }
    //            }
    //        }
    //            //if(PresentTime < TotalEndTime.ToString())
    //            //DropDownList ddlTripEndTime = (DropDownList)gvrow.FindControl("gvddlTripEndTime");

    //            //string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
    //            //string TripEnd = CurrentDate + " " + ddlTripEndTime.SelectedItem.Text.Trim();
    //            //string StartCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
    //            //string TripStart = CurrentDate + " " + lblTripStartTime.Text.Trim();
    //            //if (DateTime.Parse(TripEnd.ToString()) > DateTime.Parse(TripStart.ToString()))
    //            //{

    //        // }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblEndResponse.Text = ex.ToString();
    //        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}

    protected void btnStart_Click(object sender, EventArgs e)
    {
        //GetTimeStamp();
        txtStartDetails.Text = "";
        txtStartDetails.Focus();
        lblStartResponse.Text = string.Empty;
        back.Enabled = false;
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        TripSheetTripStart();
        txtStartBookingPin.Text = string.Empty;
        txtEndBookingPin.Text = string.Empty;
        txtClosedPin.Text = string.Empty;
    }

    protected void btnEnd_Click(object sender, EventArgs e)
    {
        txtEndDetails.Text = "";
        lblEndResponse.Text = "";
        EndBack.Enabled = false;
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        TripSheetTripEnd();
        txtEndDetails.Focus();
        txtStartBookingPin.Text = string.Empty;
        txtEndBookingPin.Text = string.Empty;
        txtClosedPin.Text = string.Empty;
    }

    protected void btnClosed_Click(object sender, EventArgs e)
    {
        CloseBack.Enabled = false;
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        TripSheetTripClosed();
        txtStartBookingPin.Text = string.Empty;
        txtEndBookingPin.Text = string.Empty;
        txtClosedPin.Text = string.Empty;
    }

    public DataTable getBoatNumber(string sBoatTypeId, string sSeaterId)
    {
        DataTable dt1 = new DataTable();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var BoatHouseId = new TripSheetSettlement()
            {
                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                BoatTypeId = sBoatTypeId.ToString(),
                BoatSeaterId = sSeaterId.ToString()
            };

            HttpResponseMessage response = client.PostAsJsonAsync("ddlBoatMaster/BHId", BoatHouseId).Result;

            if (response.IsSuccessStatusCode)
            {
                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                if (StatusCode == 1)
                {
                    dt1 = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                }
            }
        }

        return dt1;
    }

    public void TripSheetTripStart()
    {
        try
        {
            divRowerInfoMsg.Visible = false;

            BindTripCountDetails();

            divGridEnd.Visible = false;
            divGridClosed.Visible = false;
            divmsgEnd.Visible = false;
            divmsgClosed.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        CountStart = hfstartvalue.Value
                    };
                    response = client.PostAsJsonAsync("TripSheetweb/ListAllTripStartV2", vTripSheetSettlement).Result;

                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = hfstartvalue.Value
                    };
                    response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripStartV2", vTripSheetSettlement).Result;
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
                            gvTripSheetSettelementStart.DataSource = dt;
                            gvTripSheetSettelementStart.DataBind();
                            divGridStart.Visible = true;
                            gvTripSheetSettelementStart.Visible = true;
                            divMsgStart.Visible = false;
                            Next.Enabled = true;

                            if (dt.Rows.Count < 10)
                            {
                                Next.Enabled = false;
                            }
                            else
                            {
                                Next.Enabled = true;
                            }
                        }
                        else
                        {
                            gvTripSheetSettelementStart.DataSource = dt;
                            gvTripSheetSettelementStart.DataBind();
                            gvTripSheetSettelementStart.Visible = false;
                            //divGridStart.Visible = true;
                            lblGridMsgStart.Text = ResponseMsg;
                            divMsgStart.Visible = true;
                            Next.Enabled = false;
                        }
                    }
                    else
                    {
                        gvTripSheetSettelementStart.Visible = false;
                        divGridStart.Visible = true;
                        lblGridMsgStart.Text = ResponseMsg;
                        divMsgStart.Visible = true;
                        Next.Enabled = false;
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

    public void TripSheetTripEnd()
    {
        try
        {
            BindTripCountDetails();

            divGridStart.Visible = false;
            divGridClosed.Visible = false;
            divMsgStart.Visible = false;
            divmsgClosed.Visible = false;

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
                        CountStart = hfstartvalue.Value.Trim()
                    };

                    response = client.PostAsJsonAsync("TripSheetweb/ListAllTripEndV2", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = hfstartvalue.Value.Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripEndV2", vTripSheetSettlement).Result;

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
                            gvTripSheetSettelementEnd.DataSource = dt;
                            gvTripSheetSettelementEnd.DataBind();
                            divGridEnd.Visible = true;
                            gvTripSheetSettelementEnd.Visible = true;
                            divmsgEnd.Visible = false;
                            EndNext.Enabled = true;

                            if (dt.Rows.Count < 10)
                            {
                                EndNext.Enabled = false;
                            }
                            else
                            {
                                EndNext.Enabled = true;
                            }
                        }
                        else
                        {
                            gvTripSheetSettelementEnd.DataSource = dt;
                            gvTripSheetSettelementEnd.DataBind();
                            gvTripSheetSettelementEnd.Visible = false;
                            lblGridMsgEnd.Text = ResponseMsg;
                            divmsgEnd.Visible = true;
                            EndNext.Enabled = false;
                        }
                    }
                    else
                    {
                        gvTripSheetSettelementEnd.Visible = false;
                        divGridEnd.Visible = true;
                        lblGridMsgEnd.Text = ResponseMsg;
                        divmsgEnd.Visible = true;
                        EndNext.Enabled = false;
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

    public void TripSheetTripClosed()
    {
        try
        {
            BindTripCountDetails();

            divGridStart.Visible = false;
            divGridEnd.Visible = false;
            divMsgStart.Visible = false;
            divmsgEnd.Visible = false;

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
                        CountStart = hfstartvalue.Value.Trim()
                    };

                    response = client.PostAsJsonAsync("TripSheetweb/ListAllTripClosedV2", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        CountStart = hfstartvalue.Value.Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripClosedV2", vTripSheetSettlement).Result;
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
                            gvTripSheetSettelementClosed.DataSource = dt;
                            gvTripSheetSettelementClosed.DataBind();
                            divGridClosed.Visible = true;
                            gvTripSheetSettelementClosed.Visible = true;
                            divmsgClosed.Visible = false;
                            CloseNext.Enabled = true;

                            if (dt.Rows.Count < 10)
                            {
                                CloseNext.Enabled = false;
                            }
                            else
                            {
                                CloseNext.Enabled = true;
                            }
                        }
                        else
                        {
                            gvTripSheetSettelementClosed.DataSource = dt;
                            gvTripSheetSettelementClosed.DataBind();
                            divGridClosed.Visible = false;
                            lblGridMsgClosed.Text = ResponseMsg;
                            divmsgClosed.Visible = true;
                            CloseNext.Enabled = false;
                        }
                    }
                    else
                    {
                        gvTripSheetSettelementClosed.Visible = false;
                        divGridClosed.Visible = true;
                        lblGridMsgClosed.Text = ResponseMsg;
                        divmsgClosed.Visible = true;
                        CloseNext.Enabled = false;
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

    public DataTable getRower()
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
                BoatHouseId = Session["BoatHouseId"].ToString().Trim()
            };
            HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/Rower", BoatHouseId).Result;

            if (response.IsSuccessStatusCode)
            {
                var Locresponse = response.Content.ReadAsStringAsync().Result;
                int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                if (statusCode == 1)
                {
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                }
            }
        }

        return dt;
    }

    public DataTable getRowerBoatAssign()
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
                BoatTypeId = ViewState["BoatTypeIdRower"].ToString().Trim(),
                BoatSeaterId = ViewState["BoatSeaterIdRower"].ToString().Trim()

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
                }
            }
        }

        return dt;
    }

    protected void txtStartDetails_TextChanged(object sender, EventArgs e)
    {
        if (txtStartDetails.Text.Length != 0)
        {

            string BarCode = txtStartDetails.Text;
            string[] TimeList = BarCode.Split(';');

            if (TimeList.Count() == 4 && TimeList[0].Length > 1)
            {
                hfBarcodePin.Value = TimeList[3].ToString();
                BarCodeTripStart();
                return;
            }
            else
            {
                lblStartResponse.Text = "Invalid BarCode";
                txtStartDetails.Text = "";
                txtStartDetails.Focus();
            }

        }
    }

    protected void gvddlRowerId_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtStartDetails.Focus();
    }

    protected void gvddlBoatNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtStartDetails.Focus();
    }

    //public void BarCodeTripEnd()
    //{
    //    try
    //    {
    //        divGridStart.Visible = false;
    //        divGridClosed.Visible = false;
    //        divMsgStart.Visible = false;
    //        divmsgClosed.Visible = false;

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            var vTripSheetSettlement = new TripSheetSettlement()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("TripSheetweb/ListAllTripEnd", vTripSheetSettlement).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        if (hfBarCodePinEnd.Value != "")
    //                        {
    //                            for (int i = 0; i < gvTripSheetSettelementEnd.Rows.Count; i++)
    //                            {
    //                                string BarcodeBookingPinEnd = dt.Rows[i]["BookingPin"].ToString();
    //                                if (BarcodeBookingPinEnd == hfBarCodePinEnd.Value)
    //                                {
    //                                    string BookingId = dt.Rows[i]["BookingId"].ToString();
    //                                    string BoatReferenceNum = dt.Rows[i]["BoatReferenceNo"].ToString();
    //                                    string BoatNum = dt.Rows[i]["BoatId"].ToString();
    //                                    string RowerName = dt.Rows[i]["RowerId"].ToString();
    //                                    string SelfDrive = dt.Rows[i]["SelfDrive"].ToString();
    //                                    string RowerCharge = dt.Rows[i]["RowerCharge"].ToString();
    //                                    hfBookingId.Value = BookingId.ToString();
    //                                    hfRefNum.Value = BoatReferenceNum.ToString();
    //                                    hfSelfDrive.Value = SelfDrive.ToString();
    //                                    hfRowerChrg.Value = RowerCharge.ToString();
    //                                    hfBoatName.Value = BoatNum.ToString();
    //                                    hfRowerId.Value = RowerName.ToString();
    //                                    string BookingDuration = dt.Rows[i]["BookingDuration"].ToString();
    //                                    string TripStart = dt.Rows[i]["TripStartTime"].ToString();

    //                                    double Duration = 0;
    //                                    Duration = Math.Round(Convert.ToDouble(BookingDuration) / 2);
    //                                    string Time = DateTime.Now.ToString("HH:mm");
    //                                    DateTime d = DateTime.Parse(TripStart.ToString());
    //                                    DateTime now = DateTime.Parse(d.ToString("HH:mm"));
    //                                    DateTime modifiedDatetime = now.AddMinutes(Duration);
    //                                    if (modifiedDatetime > DateTime.Parse(Time))
    //                                    {
    //                                        lblEndResponse.Text = "Your Trip Cannot be Ended now";
    //                                        txtEndDetails.Text = "";
    //                                        txtEndDetails.Focus();
    //                                    }
    //                                    else
    //                                    {
    //                                        try
    //                                        {
    //                                            HttpResponseMessage response2;
    //                                            string sMSG = string.Empty;

    //                                            var vTripSheetSettlement2 = new TripSheetSettlement()
    //                                            {
    //                                                QueryType = "TripEnd",
    //                                                BookingId = hfBookingId.Value.Trim(),
    //                                                BoatReferenceNo = hfRefNum.Value.Trim(),
    //                                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
    //                                                RowerId = hfRowerId.Value.Trim(),
    //                                                TripStartTime = "",//Start Time Empty
    //                                                TripEndTime = "",
    //                                                ActualBoatId = hfBoatName.Value.Trim(),
    //                                                BookingMedia = "DW"
    //                                            };

    //                                            response2 = client.PostAsJsonAsync("TripSheetWeb/Update", vTripSheetSettlement2).Result;
    //                                            sMSG = "Inserted Successfully";

    //                                            if (response2.IsSuccessStatusCode)
    //                                            {
    //                                                var vehicleEditresponse2 = response2.Content.ReadAsStringAsync().Result;
    //                                                int StatusCode2 = Convert.ToInt32(JObject.Parse(vehicleEditresponse2)["StatusCode"].ToString());
    //                                                string ResponseMsg2 = JObject.Parse(vehicleEditresponse2)["Response"].ToString();

    //                                                if (StatusCode2 == 1)
    //                                                {
    //                                                    txtEndDetails.Text = "";
    //                                                    lblEndResponse.Text = "";
    //                                                    lblEndResponse.Text = ResponseMsg2.ToString();
    //                                                    TripSheetTripEnd();
    //                                                    txtEndDetails.Focus();
    //                                                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
    //                                                }
    //                                                else
    //                                                {
    //                                                    lblEndResponse.Text = ResponseMsg2.ToString();
    //                                                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                lblEndResponse.Text = response2.ReasonPhrase.ToString();
    //                                                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
    //                                            }
    //                                        }
    //                                        catch (Exception ex)
    //                                        {
    //                                            lblEndResponse.Text = ex.ToString();
    //                                            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //                                        }
    //                                        return;
    //                                    }                                      

    //                                }

    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {

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

    protected void txtEndDetails_TextChanged(object sender, EventArgs e)
    {
        if (txtEndDetails.Text.Length != 0)
        {

            string BarCodeEnd = txtEndDetails.Text;
            string[] TimeList = BarCodeEnd.Split(';');

            if (TimeList.Count() == 4)
            {
                hfBarCodePinEnd.Value = TimeList[3].ToString();
                BarCodeTripEnd();
                return;
            }
            else
            {
                lblEndResponse.Text = "Invalid BarCode";
                txtEndDetails.Text = "";
                txtEndDetails.Focus();
            }
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
                        ViewState["TripStartCount"] = dtExists.Rows[0]["TripSheetStartCount"].ToString().Trim();
                        btnStart.Text = "Trip Start - " + dtExists.Rows[0]["TripSheetStartCount"].ToString().Trim();
                        btnEnd.Text = "Trip End - " + dtExists.Rows[0]["TripSheetEndCount"].ToString().Trim();
                        btnClosed.Text = "Trip Closed - " + dtExists.Rows[0]["TripSheetClosedCount"].ToString().Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //public void TripEndPopUP()
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
    //            string sMSG = string.Empty;
    //            var vTripSheetSettlement = new TripSheetSettlement()
    //            {
    //                QueryType = "TripEnd",
    //                BookingId = ViewState["BookingId"].ToString().Trim(),
    //                BoatReferenceNo = ViewState["BoatReferenceNo"].ToString().Trim(),
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
    //                RowerId = ViewState["Rower"].ToString().Trim(),
    //                TripStartTime = "",//Start Time Empty
    //                TripEndTime = "",
    //                ActualBoatId = ViewState["BoatId"].ToString().Trim(),
    //                BookingMedia = "DW"
    //            };

    //            response = client.PostAsJsonAsync("TripSheetWeb/Update", vTripSheetSettlement).Result;
    //            sMSG = "Inserted Successfully";

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    TripSheetTripEnd();
    //                    lblEndResponse.Text = ResponseMsg.ToString();
    //                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
    //                }
    //                else
    //                {
    //                    lblEndResponse.Text = ResponseMsg.ToString();
    //                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
    //                }
    //            }
    //            else
    //            {
    //                lblEndResponse.Text = response.ReasonPhrase.ToString();
    //                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblEndResponse.Text = ex.ToString();
    //    }
    //}

    protected void btnPopUpOkay_Click(object sender, EventArgs e)
    {

        PopUpTripEnd();
    }

    protected void btnPopUpCancel_Click(object sender, EventArgs e)
    {
        MpeTrip.Hide();
        txtEndDetails.Text = string.Empty;
        txtEndDetails.Focus();
    }

    //New Changes Start --------------------------------------//
    protected void ImgBtnEnd_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvTripSheetSettelementEnd.DataKeys[gvrow.RowIndex].Value.ToString();

            //Label BookingId = (Label)gvrow.FindControl("lblBookingId");
            //ViewState["BookingId"] = BookingId.Text;
            //Label BoatReferenceNo = (Label)gvrow.FindControl("lblBoatReferenceNo");
            //ViewState["BoatReferenceNo"] = BoatReferenceNo.Text;
            //Label lblBoatId = (Label)gvrow.FindControl("lblBoatId");
            //ViewState["BoatId"] = lblBoatId.Text;
            //Label lblRowerId = (Label)gvrow.FindControl("lblRowerId");
            //Label lblTripStartTime = (Label)gvrow.FindControl("lblTripStartTime");
            //Label lblBookingPin = (Label)gvrow.FindControl("lblBookingPin");
            //Label lblBookingDuration = (Label)gvrow.FindControl("lblBookingDuration");

            //string Rower = string.Empty;
            //if (lblRowerId.Text == null || lblRowerId.Text == "")
            //{
            //    Rower = "0";
            //    ViewState["Rower"] = Rower.ToString();
            //}
            //else
            //{
            //    Rower = lblRowerId.Text;
            //    ViewState["Rower"] = Rower.ToString();
            //}

            //    double Duration = 0;
            //    Duration = Math.Round(Convert.ToDouble(lblBookingDuration.Text) / 2);
            //    string Time = DateTime.Now.ToString("HH:mm");
            //    string TimeTrip = DateTime.Now.ToString("HH:mm:ss");
            //    DateTime d = DateTime.Parse(lblTripStartTime.Text);
            //    DateTime now = DateTime.Parse(d.ToString("HH:mm"));
            //    DateTime nowTrip = DateTime.Parse(d.ToString("HH:mm:ss"));
            //    DateTime modifiedDatetime = now.AddMinutes(Duration);
            //    TimeSpan Diff = DateTime.Parse(TimeTrip) - nowTrip;
            //    if (modifiedDatetime > DateTime.Parse(Time))
            //    {
            //        MpeTrip.Show();
            //        lblStartTime.Text = Diff.ToString().Trim();
            //        lblPopBookingId.Text = BookingId.Text.Trim();
            //        lblPopBookingPin.Text = lblBookingPin.Text.Trim();
            //    }
            //    else
            //    {
            //        PopUpTripEnd();

            //    }

            //}

            //--------------------------------------------------------------------------

            //newly changed

            Label BookingId = (Label)gvrow.FindControl("lblBookingId");
            ViewState["BookingId"] = BookingId.Text;
            Label BoatReferenceNo = (Label)gvrow.FindControl("lblBoatReferenceNo");
            ViewState["BoatReferenceNo"] = BoatReferenceNo.Text;
            Label lblBoatId = (Label)gvrow.FindControl("lblBoatId");
            ViewState["BoatId"] = lblBoatId.Text;
            Label lblRowerId = (Label)gvrow.FindControl("lblRowerId");
            Label lblTripStartTime = (Label)gvrow.FindControl("lblTripStartTime");
            Label lblBookingPin = (Label)gvrow.FindControl("lblBookingPin");
            ViewState["BookingPinExtn"] = lblBookingPin.Text;
            Label lblBookingDuration = (Label)gvrow.FindControl("lblBookingDuration");
            ViewState["BookingDuration"] = lblBookingDuration.Text;
            Label lblBoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
            ViewState["BoatTypeIdExtn"] = lblBoatTypeId.Text;
            Label lblBoatSeaterId = (Label)gvrow.FindControl("lblBoatSeaterId");
            ViewState["BoatSeaterIdExtn"] = lblBoatSeaterId.Text;
            Label lblBoatType = (Label)gvrow.FindControl("lblBoatType");
            ViewState["BoatTypeExtn"] = lblBoatType.Text;
            Label lblBoatSeater = (Label)gvrow.FindControl("lblBoatSeater");
            ViewState["BoatSeaterExtn"] = lblBoatSeater.Text;

            GetDurationGraceTime();

            string DurationGracetime = ViewState["DurationGraceTime"].ToString().Trim();

            ViewState["BoatSeaterExtn"] = lblBoatSeater.Text;

            string Rower = string.Empty;
            if (lblRowerId.Text == null || lblRowerId.Text == "")
            {
                Rower = "0";
                ViewState["Rower"] = Rower.ToString();
            }
            else
            {
                Rower = lblRowerId.Text;
                ViewState["Rower"] = Rower.ToString();
            }




            //newly changes added
            double Duration = 0;
            Duration = Math.Round(Convert.ToDouble(lblBookingDuration.Text) / 2);
            // Duration AND Grace Time
            double ExDuration = 0;
            ExDuration = Math.Round(Convert.ToDouble(DurationGracetime.ToString().Trim()));

            string Time = DateTime.Now.ToString("HH:mm");
            string TimeTrip = DateTime.Now.ToString("HH:mm:ss");
            DateTime d = DateTime.Parse(lblTripStartTime.Text);
            DateTime now = DateTime.Parse(d.ToString("HH:mm"));
            DateTime nowTrip = DateTime.Parse(d.ToString("HH:mm:ss"));
            DateTime modifiedDatetime = now.AddMinutes(Duration);
            DateTime ExtensionDatetime = now.AddMinutes(ExDuration);
            TimeSpan Diff = DateTime.Parse(TimeTrip) - nowTrip;
            if (modifiedDatetime > DateTime.Parse(Time))
            {
                MpeTrip.Show();
                lblStartTime.Text = Diff.ToString().Trim();
                lblPopBookingId.Text = BookingId.Text.Trim();
                lblPopBookingPin.Text = lblBookingPin.Text.Trim();
            }
            //else
            //{
            //    PopUpTripEnd();

            //}
            else if (DateTime.Parse(Time) > ExtensionDatetime)
            {
                CheckExtension();
                if (ViewState["ExtensionType"].ToString().Trim() == "N")
                {
                    PopUpTripEnd();

                    AmountAfterExtension();
                    string BookId = ViewState["BookingId"].ToString().Trim();
                    string BookingPin = ViewState["BookingPinExtn"].ToString().Trim();
                    string BoatType = ViewState["BoatTypeExtn"].ToString().Trim();
                    string BoatSeater = ViewState["BoatSeaterExtn"].ToString().Trim();
                    string BookingDate = ViewState["BDate"].ToString().Trim();
                    string BookingDuration = ViewState["BookingDuration"].ToString().Trim();
                    string StartTime = ViewState["TripStartTime"].ToString().Trim();
                    string EndTime = ViewState["TripEndTime"].ToString().Trim();
                    string TotalDuration = ViewState["ExtensionDuration"].ToString().Trim();
                    string ExtraBoatCharge = ViewState["ExtraBoatCharge"].ToString().Trim();
                    string ExtraRowerCharge = ViewState["ExtraRowerCharge"].ToString().Trim();
                    string ExtraNetAmount = ViewState["ExtraNetAmountForNotAllowed"].ToString().Trim();
                    string Tax = ViewState["Tax"].ToString().Trim();
                    string ActualNetAmount = ViewState["ActualNetAmount"].ToString().Trim();

                    if (Convert.ToDecimal(ExtraBoatCharge) > 0 && Convert.ToDecimal(ExtraNetAmount) > 0)
                    //if (Convert.ToDecimal(ExtraBoatCharge) > 0 && Convert.ToDecimal(ExtraRowerCharge) > 0 && Convert.ToDecimal(ExtraNetAmount) > 0)
                    {
                        if (Session["ExtensionPrint"].ToString().Trim() == "Y")
                        {
                            Response.Redirect("~/Boating/PrintExtraCharge?rte=rRefsse&bi=&BId=" + BookId.Trim() + "&BPin=" + BookingPin.Trim() +
                          "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + BookingDuration.Trim() +
                          "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BEBC=" + ExtraBoatCharge.Trim() +
                          "&BERC=" + ExtraRowerCharge.Trim() + "&BENC=" + ExtraNetAmount.Trim() + "&Tax=" + Tax.Trim() + "&ANA=" + ActualNetAmount.Trim() + " ");
                        }
                    }

                }
                //else
                //{
                //    PopUpTripEnd();
                //}

                if (ViewState["ExtensionType"].ToString().Trim() == "A")
                {
                    PopUpTripEnd();

                    AmountAfterExtension();
                    string BookId = ViewState["BookingId"].ToString().Trim();
                    string BookingPin = ViewState["BookingPinExtn"].ToString().Trim();
                    string BoatType = ViewState["BoatTypeExtn"].ToString().Trim();
                    string BoatSeater = ViewState["BoatSeaterExtn"].ToString().Trim();
                    string BookingDate = ViewState["BDate"].ToString().Trim();
                    string BookingDuration = ViewState["BookingDuration"].ToString().Trim();
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

                    if (Convert.ToDecimal(ExtraBoatCharge) > 0 && Convert.ToDecimal(ExtraNetAmount) > 0)
                    //if (Convert.ToDecimal(ExtraBoatCharge) > 0 && Convert.ToDecimal(ExtraRowerCharge) > 0 && Convert.ToDecimal(ExtraNetAmount) > 0)
                    {
                        if (Session["ExtensionPrint"].ToString().Trim() == "Y")
                        {
                            Response.Redirect("~/Boating/PrintExtraCharge?rte=rRefssea&bi=&BId=" + BookId.Trim() + "&BPin=" + BookingPin.Trim() +
                          "&BTId=" + BoatType.Trim() + "&BSId=" + BoatSeater.Trim() + "&BDate=" + BookingDate.Trim() + "&BDur=" + BookingDuration.Trim() +
                          "&BSTime=" + StartTime.Trim() + "&BETime=" + EndTime.Trim() + "&BTDur=" + TotalDuration.Trim() + "&BEBC=" + ExtraBoatCharge.Trim() +
                          "&BERC=" + ExtraRowerCharge.Trim() + "&BENC=" + ExtraNetAmount.Trim() + "&Tax=" + Tax.Trim() + "&ANA=" + ActualNetAmount.Trim() +
                          "&BD=" + BoatDeposit.Trim() + "&DRA=" + DepRefundAmount.Trim() + " ");
                        }
                    }

                }

            }
            else
            {
                PopUpTripEnd();

            }
        }
        catch (Exception ex)
        {
            lblEndResponse.Text = ex.ToString();
            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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
                    Input2 = ViewState["BookingPinExtn"].ToString().Trim(),
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
                    Input1 = ViewState["BoatTypeIdExtn"].ToString().Trim(),
                    Input2 = ViewState["BoatSeaterIdExtn"].ToString().Trim(),
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
                    Input2 = ViewState["BookingPinExtn"].ToString().Trim(),
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

    //New Changes End--------------------------------------//


    public void PopUpTripEnd()
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

                var vTripSheetSettlement2 = new TripSheetSettlement()
                {
                    QueryType = "TripEnd",
                    BookingId = ViewState["BookingId"].ToString().Trim(),
                    BoatReferenceNo = ViewState["BoatReferenceNo"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    RowerId = ViewState["Rower"].ToString().Trim(),
                    TripStartTime = "",//Start Time Empty
                    TripEndTime = "",
                    ActualBoatId = ViewState["BoatId"].ToString().Trim(),
                    SSUserBy = Session["UserId"].ToString().Trim(),
                    SDUserBy = Session["UserId"].ToString().Trim(),
                    BookingMedia = "DW"
                };

                response = client.PostAsJsonAsync("NewTripSheetWeb/Update", vTripSheetSettlement2).Result;
                sMSG = "Inserted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        //txtEndDetails.Text = "";
                        //lblEndResponse.Text = "";

                        TripSheetTripEnd();
                        lblEndResponse.Text = ResponseMsg.ToString();
                        txtEndDetails.Text = "";
                        txtEndDetails.Focus();
                        //txtEndDetails.Focus();
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        lblEndResponse.Text = ResponseMsg.ToString();
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    lblEndResponse.Text = response.ReasonPhrase.ToString();
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblEndResponse.Text = ex.ToString();
            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
        return;
    }

    public void BarCodeTripStart()
    {
        try
        {
            divGridEnd.Visible = false;
            divGridClosed.Visible = false;
            divmsgEnd.Visible = false;
            divmsgClosed.Visible = false;

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

                    response = client.PostAsJsonAsync("TripSheetweb/ListAllTripStart", vTripSheetSettlement).Result;
                }
                else
                {
                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripStart", vTripSheetSettlement).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        HttpResponseMessage response2;
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarcodePin.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response2 = client.PostAsJsonAsync("NewTripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
                        if (response2.IsSuccessStatusCode)
                        {
                            var vehicleEditresponse2 = response2.Content.ReadAsStringAsync().Result;
                            int StatusCode2 = Convert.ToInt32(JObject.Parse(vehicleEditresponse2)["StatusCode"].ToString());
                            string ResponseMsg2 = JObject.Parse(vehicleEditresponse2)["Response"].ToString();

                            {
                                if (StatusCode2 == 1)
                                {

                                    if (dt.Rows.Count > 0)
                                    {
                                        if (hfBarcodePin.Value != "")
                                        {
                                            for (int i = 0; i < gvTripSheetSettelementStart.Rows.Count; i++)
                                            {
                                                string BarcodeBookingPin = dt.Rows[i]["BookingPin"].ToString();
                                                if (BarcodeBookingPin == hfBarcodePin.Value)
                                                {
                                                    string BookingId = dt.Rows[i]["BookingId"].ToString();
                                                    string BoatReferenceNum = dt.Rows[i]["BoatReferenceNo"].ToString();
                                                    DropDownList ddlBoatName = gvTripSheetSettelementStart.Rows[i].FindControl("gvddlBoatNumber") as DropDownList;
                                                    string BoatName = ddlBoatName.SelectedValue.Trim();
                                                    DropDownList ddlRowerId = (DropDownList)gvTripSheetSettelementStart.Rows[i].FindControl("gvddlRowerId");
                                                    hfRowerId.Value = ddlRowerId.SelectedValue.Trim();
                                                    string SelfDrive = dt.Rows[i]["SelfDrive"].ToString();
                                                    string RowerCharge = dt.Rows[i]["RowerCharge"].ToString();
                                                    hfBookingId.Value = BookingId.ToString();
                                                    hfRefNum.Value = BoatReferenceNum.ToString();
                                                    hfSelfDrive.Value = SelfDrive.ToString();
                                                    hfRowerChrg.Value = RowerCharge.ToString();
                                                    hfBoatName.Value = BoatName.ToString();

                                                    try
                                                    {
                                                        HttpResponseMessage response1;
                                                        string sMSG = string.Empty;

                                                        if (hfSelfDrive.Value == "N" && Convert.ToDecimal(hfRowerChrg.Value) > 0)
                                                        {
                                                            if (Convert.ToInt32(hfRowerId.Value.Trim()) == 0)
                                                            {
                                                                lblStartResponse.Text = "Select Rower Name";
                                                                txtStartDetails.Text = "";
                                                                txtStartDetails.Focus();
                                                                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Rower Name');", true);
                                                                return;
                                                            }
                                                        }


                                                        //----------------------------------------------------------------//
                                                        if (ddlRowerId.SelectedValue.Trim() != "0")
                                                        {
                                                            autoTripEndDetails(ddlRowerId.SelectedValue.Trim());
                                                            if (ViewState["AutoEndForNoDepositeTripEnd"].ToString().Trim() == "Y")
                                                            {
                                                                if (Convert.ToDecimal(ViewState["BoatDeposit"].ToString().Trim()) > 0)
                                                                {
                                                                    divRowerInfoMsg.Visible = true;
                                                                    lblStartResponse.Visible = false;
                                                                    lblRowerInfoMsg.Text = "Rower-'" + ddlRowerId.SelectedItem.Text.Trim() + "' is Already On A Trip, Cannot Start Trip.";
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
                                                                        lblRowerInfoMsg.Text = "Trip Cannot Be Started With  Rower-'" + ddlRowerId.SelectedItem.Text.Trim() + "', before-'" + FinalTripTime.ToString("hh:mm tt").Trim() + "'";
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


                                                        //---------------------------------------------------------------//



                                                        var vTripSheetSettlement1 = new TripSheetSettlement()
                                                        {
                                                            QueryType = "TripStart",
                                                            BookingId = hfBookingId.Value.Trim(),
                                                            BoatReferenceNo = hfRefNum.Value.Trim(),
                                                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                                            RowerId = hfRowerId.Value.Trim(),
                                                            TripStartTime = "",
                                                            TripEndTime = "",//End Time Pas Empty
                                                            ActualBoatId = hfBoatName.Value.Trim(),
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
                                                                txtStartDetails.Text = "";
                                                                lblStartResponse.Text = "";
                                                                TripSheetTripStart();

                                                                lblStartResponse.Text = ResponseMsg1.ToString();
                                                                txtStartDetails.Focus();
                                                                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                                                            }
                                                            else
                                                            {
                                                                lblStartResponse.Text = ResponseMsg1.ToString();
                                                                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            lblStartResponse.Text = response1.ReasonPhrase.ToString();
                                                            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                                                        }
                                                        //}
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        lblStartResponse.Text = ex.ToString();
                                                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
                                                    }
                                                    return;
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        gvTripSheetSettelementStart.DataSource = dt;
                                        gvTripSheetSettelementStart.DataBind();
                                        divGridStart.Visible = false;
                                        lblGridMsgStart.Text = ResponseMsg;
                                        divMsgStart.Visible = true;
                                    }
                                }
                                else
                                {
                                    txtStartDetails.Text = "";
                                    txtStartDetails.Focus();
                                    lblStartResponse.Text = "User Doesn't have Rights to Proceed this Ticket";
                                }
                            }
                        }
                        else
                        {
                            lblGridMsgStart.Text = ResponseMsg;
                            divGridStart.Visible = false;
                            lblGridMsgStart.Text = ResponseMsg;
                            divMsgStart.Visible = true;
                        }
                    }
                    else
                    {
                        txtStartDetails.Text = "";
                        txtStartDetails.Focus();
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

    public void BarCodeTripEnd()
    {
        try
        {
            divGridStart.Visible = false;
            divGridClosed.Visible = false;
            divMsgStart.Visible = false;
            divmsgClosed.Visible = false;

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

                    response = client.PostAsJsonAsync("TripSheetweb/ListAllTripEnd", vTripSheetSettlement).Result;
                }
                else
                {

                    var vTripSheetSettlement = new TripSheetSettlement()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };

                    response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripEnd", vTripSheetSettlement).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        HttpResponseMessage response2;
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BarcodePin = hfBarCodePinEnd.Value.Trim(),
                            UserId = Session["UserId"].ToString().Trim()
                        };

                        response2 = client.PostAsJsonAsync("NewTripSheetweb/ScanTripStart", vTripSheetSettlement).Result;
                        if (response2.IsSuccessStatusCode)
                        {
                            var vehicleEditresponse2 = response2.Content.ReadAsStringAsync().Result;
                            int StatusCode2 = Convert.ToInt32(JObject.Parse(vehicleEditresponse2)["StatusCode"].ToString());
                            string ResponseMsg2 = JObject.Parse(vehicleEditresponse2)["Response"].ToString();

                            {
                                if (StatusCode2 == 1)
                                {

                                    if (dt.Rows.Count > 0)
                                    {
                                        if (hfBarCodePinEnd.Value != "")
                                        {
                                            for (int i = 0; i < gvTripSheetSettelementEnd.Rows.Count; i++)
                                            {
                                                string BarcodeBookingPinEnd = dt.Rows[i]["BookingPin"].ToString();
                                                if (BarcodeBookingPinEnd == hfBarCodePinEnd.Value)
                                                {
                                                    string BookingId = dt.Rows[i]["BookingId"].ToString();
                                                    string BoatReferenceNum = dt.Rows[i]["BoatReferenceNo"].ToString();
                                                    string BoatNum = dt.Rows[i]["BoatId"].ToString();
                                                    string RowerName = dt.Rows[i]["RowerId"].ToString();
                                                    string SelfDrive = dt.Rows[i]["SelfDrive"].ToString();
                                                    string RowerCharge = dt.Rows[i]["RowerCharge"].ToString();
                                                    ViewState["BookingId"] = BookingId.ToString();
                                                    ViewState["BoatReferenceNo"] = BoatReferenceNum.ToString();
                                                    hfSelfDrive.Value = SelfDrive.ToString();
                                                    hfRowerChrg.Value = RowerCharge.ToString();
                                                    ViewState["BoatId"] = BoatNum.ToString();
                                                    hfRowerId.Value = RowerName.ToString();
                                                    string BookingDuration = dt.Rows[i]["BookingDuration"].ToString();
                                                    string TripStart = dt.Rows[i]["TripStartTime"].ToString();

                                                    string Rower = string.Empty;
                                                    if (RowerName.ToString() == null || RowerName.ToString() == "")
                                                    {
                                                        Rower = "0";
                                                        ViewState["Rower"] = Rower.ToString();
                                                    }
                                                    else
                                                    {
                                                        Rower = RowerName.ToString();
                                                        ViewState["Rower"] = Rower.ToString();
                                                    }
                                                    double Duration = 0;
                                                    Duration = Math.Round(Convert.ToDouble(BookingDuration) / 2);
                                                    string Time = DateTime.Now.ToString("HH:mm");
                                                    string TimeTrip = DateTime.Now.ToString("HH:mm:ss");
                                                    DateTime d = DateTime.Parse(TripStart.ToString());
                                                    DateTime now = DateTime.Parse(d.ToString("HH:mm"));
                                                    DateTime nowTrip = DateTime.Parse(d.ToString("HH:mm:ss"));
                                                    DateTime modifiedDatetime = now.AddMinutes(Duration);
                                                    TimeSpan Diff = DateTime.Parse(TimeTrip) - nowTrip;
                                                    if (modifiedDatetime > DateTime.Parse(Time))
                                                    {
                                                        MpeTrip.Show();
                                                        lblStartTime.Text = Diff.ToString().Trim();
                                                        lblPopBookingId.Text = BookingId.ToString().Trim();
                                                        lblPopBookingPin.Text = hfBarCodePinEnd.Value.Trim();
                                                    }
                                                    else
                                                    {
                                                        PopUpTripEnd();
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    txtEndDetails.Text = "";
                                    txtEndDetails.Focus();
                                    lblEndResponse.Text = "User Doesn't have Rights to Proceed this Ticket";
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        txtEndDetails.Text = "";
                        txtEndDetails.Focus();
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
        public string UserId { get; set; }
        public string SSUserBy { get; set; }
        public string SDUserBy { get; set; }
        public string BarcodePin { get; set; }
        public string CountStart { get; set; }
        public string CountEnd { get; set; }
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

    protected void ChkIndTrpSht_CheckedChanged(object sender, EventArgs e)
    {

        if (ChkIndTrpSht.Checked == true)
        {
            Response.Redirect("IndividualTripSheetWeb.aspx", true);

        }
        else
        {
            Response.Redirect("IndividualTripSheetWeb.aspx", false);

        }

    }


    //****************** Newly Added By Vinitha *********************
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

                var body = new UserRights()
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

    public class UserRights
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BookingId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

        //****************** Newly Added By Vinitha *********************

    }

    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(hfendvalue.Value) + 1, Int32.Parse(hfendvalue.Value) + 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();

        TripSheetTripStart();
    }

    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(hfstartvalue.Value) - 10, Int32.Parse(hfendvalue.Value) - 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        TripSheetTripStart();
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
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
    }
    protected void subProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
            back.Enabled = false;
            EndBack.Enabled = false;
            CloseBack.Enabled = false;
        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            back.Enabled = false;
            EndBack.Enabled = false;
            CloseBack.Enabled = false;
        }
        else
        {
            iend = end;

        }
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
    }

    protected void EndBack_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(hfstartvalue.Value) - 10, Int32.Parse(hfendvalue.Value) - 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        TripSheetTripEnd();
    }

    protected void EndNext_Click(object sender, EventArgs e)
    {
        EndBack.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(hfendvalue.Value) + 1, Int32.Parse(hfendvalue.Value) + 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        TripSheetTripEnd();
    }

    protected void CloseBack_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(hfstartvalue.Value) - 10, Int32.Parse(hfendvalue.Value) - 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        TripSheetTripClosed();
    }

    protected void CloseNext_Click(object sender, EventArgs e)
    {
        CloseBack.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(hfendvalue.Value) + 1, Int32.Parse(hfendvalue.Value) + 10, out istart, out iend);
        hfstartvalue.Value = istart.ToString();
        hfendvalue.Value = iend.ToString();
        TripSheetTripClosed();
    }

    protected void BackToList_Click(object sender, EventArgs e)
    {
        back.Visible = true;
        Next.Visible = true;
        BackToList.Visible = false;
        txtStartBookingPin.Text = string.Empty;
        TripSheetTripStart();

    }
    protected void EndBackToList_Click(object sender, EventArgs e)
    {
        EndBack.Visible = true;
        EndNext.Visible = true;
        EndBackToList.Visible = false;
        txtEndBookingPin.Text = string.Empty;
        TripSheetTripEnd();
    }

    //***********************************************************************

    protected void txtStartBookingPin_TextChanged(object sender, EventArgs e)
    {
        TripSheetTripStartSingle();
        lblStartResponse.Text = string.Empty;
        back.Visible = false;
        Next.Visible = false;
        BackToList.Visible = true;
    }

    public void TripSheetTripStartSingle()
    {
        try
        {
            divRowerInfoMsg.Visible = false;

            BindTripCountDetails();

            divGridEnd.Visible = false;
            divGridClosed.Visible = false;
            divmsgEnd.Visible = false;
            divmsgClosed.Visible = false;
            if (txtStartBookingPin.Text.Trim() != "")
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    HttpResponseMessage response;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {

                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BookingPin = txtStartBookingPin.Text.Trim()
                        };
                        response = client.PostAsJsonAsync("TripSheetweb/ListAllTripStartSingleV2", vTripSheetSettlement).Result;

                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            BookingPin = txtStartBookingPin.Text.Trim()
                        };
                        response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripStartSingleV2", vTripSheetSettlement).Result;
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
                                gvTripSheetSettelementStart.DataSource = dt;
                                gvTripSheetSettelementStart.DataBind();
                                divGridStart.Visible = true;
                                gvTripSheetSettelementStart.Visible = true;
                                divMsgStart.Visible = false;
                                lblGridMsgStart.Text = string.Empty;

                            }
                            else
                            {
                                gvTripSheetSettelementStart.DataSource = dt;
                                gvTripSheetSettelementStart.DataBind();
                                gvTripSheetSettelementStart.Visible = false;
                                //divGridStart.Visible = true;
                                lblGridMsgStart.Text = ResponseMsg;
                                divMsgStart.Visible = true;
                            }
                        }
                        else
                        {
                            gvTripSheetSettelementStart.Visible = false;
                            divGridStart.Visible = true;
                            lblGridMsgStart.Text = ResponseMsg;
                            divMsgStart.Visible = true;
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

    protected void txtEndBookingPin_TextChanged(object sender, EventArgs e)
    {

        TripSheetTripEndSingle();
        lblEndResponse.Text = string.Empty;
        EndBack.Visible = false;
        EndNext.Visible = false;
        EndBackToList.Visible = true;

    }

    public void TripSheetTripEndSingle()
    {
        try
        {
            if (txtEndBookingPin.Text.Trim() != "")
            {
                BindTripCountDetails();

                divGridStart.Visible = false;
                divGridClosed.Visible = false;
                divMsgStart.Visible = false;
                divmsgClosed.Visible = false;

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
                            BookingPin = txtEndBookingPin.Text.Trim()
                        };

                        response = client.PostAsJsonAsync("TripSheetweb/ListAllTripEndSingleV2", vTripSheetSettlement).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            BookingPin = txtEndBookingPin.Text.Trim()
                        };

                        response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripEndSingleV2", vTripSheetSettlement).Result;
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
                                gvTripSheetSettelementEnd.DataSource = dt;
                                gvTripSheetSettelementEnd.DataBind();
                                divGridEnd.Visible = true;
                                gvTripSheetSettelementEnd.Visible = true;
                                divmsgEnd.Visible = false;
                                lblGridMsgEnd.Text = string.Empty;
                            }
                            else
                            {
                                gvTripSheetSettelementEnd.DataSource = dt;
                                gvTripSheetSettelementEnd.DataBind();
                                gvTripSheetSettelementEnd.Visible = false;
                                lblGridMsgEnd.Text = ResponseMsg;
                                divmsgEnd.Visible = true;
                            }
                        }
                        else
                        {
                            gvTripSheetSettelementEnd.Visible = false;
                            divGridEnd.Visible = true;
                            lblGridMsgEnd.Text = ResponseMsg;
                            divmsgEnd.Visible = true;
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





    protected void txtClosedPin_TextChanged(object sender, EventArgs e)
    {
        TripSheetTripClosedSingle();
        lblEndResponse.Text = string.Empty;
        CloseBack.Visible = false;
        CloseNext.Visible = false;
        CloseBacktoList.Visible = true;
    }

    protected void CloseBacktoList_Click(object sender, EventArgs e)
    {
        CloseBack.Visible = true;
        CloseNext.Visible = true;
        CloseBacktoList.Visible = false;
        txtClosedPin.Text = string.Empty;
        TripSheetTripClosed();
    }
    public void TripSheetTripClosedSingle()
    {
        try
        {
            if (txtClosedPin.Text.Trim() != "")
            {
                BindTripCountDetails();

                divGridStart.Visible = false;
                divGridEnd.Visible = false;
                divMsgStart.Visible = false;
                divmsgEnd.Visible = false;

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
                            BookingPin = txtClosedPin.Text.Trim()
                        };

                        response = client.PostAsJsonAsync("TripSheetweb/ListAllTripClosedSingleV2", vTripSheetSettlement).Result;
                    }
                    else
                    {
                        var vTripSheetSettlement = new TripSheetSettlement()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            UserId = Session["UserId"].ToString().Trim(),
                            BookingPin = txtClosedPin.Text.Trim()
                        };

                        response = client.PostAsJsonAsync("NewTripSheetweb/ListAllTripClosedSingleV2", vTripSheetSettlement).Result;
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
                                gvTripSheetSettelementClosed.DataSource = dt;
                                gvTripSheetSettelementClosed.DataBind();
                                divGridClosed.Visible = true;
                                gvTripSheetSettelementClosed.Visible = true;
                                divmsgClosed.Visible = false;
                                CloseNext.Enabled = true;

                                if (dt.Rows.Count < 10)
                                {
                                    CloseNext.Enabled = false;
                                }
                                else
                                {
                                    CloseNext.Enabled = true;
                                }
                            }
                            else
                            {
                                gvTripSheetSettelementClosed.DataSource = dt;
                                gvTripSheetSettelementClosed.DataBind();
                                divGridClosed.Visible = false;
                                lblGridMsgClosed.Text = ResponseMsg;
                                divmsgClosed.Visible = true;
                                CloseNext.Enabled = false;
                            }
                        }
                        else
                        {
                            gvTripSheetSettelementClosed.Visible = false;
                            divGridClosed.Visible = true;
                            lblGridMsgClosed.Text = ResponseMsg;
                            divmsgClosed.Visible = true;
                            CloseNext.Enabled = false;
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
}