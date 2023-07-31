using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Data;
using Newtonsoft.Json;
using System.Web.Helpers;

public partial class Boating_IndividualTripSheetWeb : System.Web.UI.Page
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
                GetBoatType();
                BindTripCountDetails();
                if (Session["BBMTripSheetWeb"].ToString().Trim() == "Y")
                {
                    if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                    {
                        divTripStart.Visible = true;
                        divTripEnd.Visible = true;
                        divTripClosed.Visible = true;
                        BindTripStart();
                    }
                    else
                    {
                        divTripStart.Visible = false;
                        divTripEnd.Visible = false;

                        if (Session["BBMTripSheetStart"].ToString().Trim() == "Y" && Session["BBMTripSheetEnd"].ToString().Trim() == "Y")
                        {
                            divTripStart.Visible = true;
                            divTripEnd.Visible = true;
                            BindTripStart();
                        }
                        else if (Session["BBMTripSheetStart"].ToString().Trim() == "Y" && Session["BBMTripSheetEnd"].ToString().Trim() == "N")
                        {
                            divTripStart.Visible = true;
                            divTripEnd.Visible = false;
                            BindTripStart();
                        }
                        else if (Session["BBMTripSheetStart"].ToString().Trim() == "N" && Session["BBMTripSheetEnd"].ToString().Trim() == "Y")
                        {
                            divTripStart.Visible = false;
                            divTripEnd.Visible = true;
                            BindTripStart();
                        }
                    }
                    BindTripCountDetails();
                    txtStartDetails.Focus();
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    /// <summary>
    /// This Function Is Used To Get BoatType
    /// </summary>
    public void GetBoatType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var IndTripSheet = new IndividualTripSheet()
                {
                    QueryType = "GetBoatType",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    UserId = "0",
                    UserRole = "",
                    BoatTypeId = "0",
                    BoatSeaterId = "0",
                    RowerId = "0",
                    TripUser = ""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("IndividualTripSheet/View", IndTripSheet).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseMsg = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt16(JObject.Parse(responseMsg)["StatusCode"].ToString());
                    string sResponseMsg = JObject.Parse(responseMsg)["DataTable"].ToString();
                    string sSuccessOrFailureMsg = JObject.Parse(responseMsg)["ResponseMsg"].ToString();
                    if (sSuccessOrFailureMsg == "No Records Found !!!.")
                    {
                        ddlBoatType.Items.Insert(0, new ListItem("Select BoatType", "0"));
                        return;
                    }
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(sResponseMsg);

                    ddlBoatType.Items.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        ddlBoatType.DataSource = dt;
                        ddlBoatType.DataValueField = "BoatTypeId";
                        ddlBoatType.DataTextField = "BoatType";
                        ddlBoatType.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!!.');", true);
                    }
                    ddlBoatType.Items.Insert(0, new ListItem("Select BoatType", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }
    /// <summary>
    /// This Function Is Used To Get Seater Type. 
    /// </summary>
    public void GetSeaterType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var IndTripSheet = new IndividualTripSheet()
                {
                    QueryType = "GetBoatSeaterType",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    UserId = "0",
                    UserRole = "",
                    BoatSeaterId = "0",
                    RowerId = "0",
                    TripUser = ""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("IndividualTripSheet/View", IndTripSheet).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseMsg = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt16(JObject.Parse(responseMsg)["StatusCode"].ToString());
                    string sResponseMsg = JObject.Parse(responseMsg)["DataTable"].ToString();
                    string sSuccessOrFailureMsg = JObject.Parse(responseMsg)["ResponseMsg"].ToString();
                    if (sSuccessOrFailureMsg == "No Records Found !!!.")
                    {
                        ddlSeaterType.Items.Insert(0, new ListItem("Select Seater Type", "0"));
                        return;
                    }
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(sResponseMsg);

                    ddlSeaterType.Items.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        ddlSeaterType.DataSource = dt;
                        ddlSeaterType.DataValueField = "BoatSeaterId";
                        ddlSeaterType.DataTextField = "SeaterType";
                        ddlSeaterType.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!!.');", true);
                    }
                    ddlSeaterType.Items.Insert(0, new ListItem("Select Seater Type", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }
    /// <summary>
    /// This Function Is Used To Get Seater Type Based On BoatType.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBoatType.SelectedValue != "0")
        {
            GetSeaterType();
        }
        else
        {

            foreach (GridViewRow gvr in gvTripSheetSettelementStart.Rows)
            {
                CheckBox chkBox = (CheckBox)gvr.FindControl("gvTripStartChkBox");
                if (chkBox.Checked)
                {
                    chkBox.Checked = false;
                }
            }
            ddlSeaterType.Items.Clear();
        }
    }
    /// <summary>
    /// This Method Is Used To Get Trip Start,End And Closed Count.
    /// </summary>
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
                    var IndTripSheet = new IndividualTripSheet()
                    {
                        QueryType = "TripSheetCount",
                        UserId = "00",
                        UserRole = "Admin",
                        RowerId = "0",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndTripSheet).Result;
                }
                else
                {
                    var IndTripSheetIndTripSheet = new IndividualTripSheet()
                    {
                        QueryType = "TripSheetCount",
                        UserRole = "User",
                        RowerId = "0",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        UserId = Session["UserId"].ToString().Trim(),
                        TripUser = "",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndTripSheetIndTripSheet).Result;
                }
                if (response.IsSuccessStatusCode)
                {
                    var responseMsg = response.Content.ReadAsStringAsync().Result;
                    string sResponseMsg = JObject.Parse(responseMsg)["DataTable"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(sResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        btnStart.Text = "Trip Start - " + dtExists.Rows[0]["TripSheetStartCount"].ToString().Trim();
                        btnEnd.Text = "Trip End - " + dtExists.Rows[0]["TripSheetEndCount"].ToString().Trim();
                        btnClosed.Text = "Trip Closed - " + dtExists.Rows[0]["TripSheetClosedCount"].ToString().Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.ToString().Trim() + "');", true);
            return;
        }
    }
    /// <summary>
    /// This Method Is Used To Bind GridView For TripStart.
    /// </summary>
    public void BindTripStart()
    {
        try
        {
            ddlBoatType.SelectedValue = "0";
            ddlSeaterType.Items.Clear();

            BindTripCountDetails();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var IndtripSheet = new IndividualTripSheet()
                    {
                        QueryType = "TripStartAdmin",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = "0",
                        UserRole = "",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                        RowerId = "0"
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndtripSheet).Result;
                }
                else
                {
                    var IndtripSheet = new IndividualTripSheet()
                    {
                        QueryType = "TripStartUser",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        UserRole = "",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        TripUser = "",
                        RowerId = "0"
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndtripSheet).Result;
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
                            gvTripSheetSettelementStart.DataSource = dt;
                            gvTripSheetSettelementStart.DataBind();
                            BtnAdd.Visible = true;
                            gvTripSheetSettelementStart.Visible = true;
                            divMsgStart.Visible = false;

                            divbtnAdd.Visible = true;
                            divGridStart.Visible = true;
                            divGridOnlyGridStart.Visible = true;

                            divGridEnd.Visible = false;
                            divGridClosed.Visible = false;
                        }
                        else
                        {
                            gvTripSheetSettelementStart.DataSource = dt;
                            gvTripSheetSettelementStart.DataBind();
                            BtnAdd.Visible = false;
                            gvTripSheetSettelementStart.Visible = false;
                            lblGridMsgStart.Text = sSuccessOrFailureMsg;
                            divMsgStart.Visible = true;

                            divbtnAdd.Visible = false;
                            divGridOnlyGridStart.Visible = false;

                            divGridStart.Visible = true;
                            divGridEnd.Visible = false;
                            divGridClosed.Visible = false;
                        }
                    }
                    else
                    {
                        BtnAdd.Visible = false;
                        gvTripSheetSettelementEnd.Visible = false;
                        divGridEnd.Visible = true;
                        lblGridMsgStart.Text = sSuccessOrFailureMsg;
                        lblGridMsgStart.ForeColor = System.Drawing.Color.Red;
                        divmsgEnd.Visible = true;

                        divbtnAdd.Visible = false;
                        divGridOnlyGridStart.Visible = false;
                        divGridStart.Visible = true;
                        divGridEnd.Visible = false;
                        divGridClosed.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }
    /// <summary>
    /// This Method Is Used to Get Image QR Code.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvTripSheetSettelementStart_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["TripUser"].ToString().Trim() == "D")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    Label Bookingid = (Label)e.Row.FindControl("lblBookingId");
                    Label lblBoatReferenceNo = (Label)e.Row.FindControl("lblBoatReferenceNo");
                    Label BookingDuration = (Label)e.Row.FindControl("lblBookingDuration");
                    Label BookingPin = (Label)e.Row.FindControl("lblBookingPin");
                    Image ImageData = (Image)e.Row.FindControl("imgOtherQRRc");

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var QRd = new QR()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString(),
                            BookingId = Bookingid.Text.Trim(),
                            Pin = BookingPin.Text.Trim(),
                            BookingRef = lblBoatReferenceNo.Text.Trim()
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("PassQR", QRd).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                            string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                            string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                            if (Status == "Success.")
                            {
                                ImageData.Visible = true;
                                ImageData.ImageUrl = ResponseMsg;
                            }
                            else
                            {
                                lblStartResponse.Text = ResponseMsg.ToString();
                            }
                        }
                        else
                        {
                            lblStartResponse.Text = response.StatusCode.ToString();
                        }
                    }
                }
            }
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    Label Bookingid = (Label)e.Row.FindControl("lblBookingId");
                    Label lblBoatReferenceNo = (Label)e.Row.FindControl("lblBoatReferenceNo");
                    Label BookingDuration = (Label)e.Row.FindControl("lblBookingDuration");
                    Label BookingPin = (Label)e.Row.FindControl("lblBookingPin");
                    Image ImageData = (Image)e.Row.FindControl("imgOtherQRRc");

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var QRd = new QR()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString(),
                            BookingId = Bookingid.Text.Trim(),
                            Pin = BookingPin.Text.Trim(),
                            BookingRef = lblBoatReferenceNo.Text.Trim()
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("PassQR", QRd).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                            string Status = JObject.Parse(vehicleEditresponse)["status"].ToString();
                            string ResponseMsg = JObject.Parse(vehicleEditresponse)["imageURL"].ToString();

                            if (Status == "Success.")
                            {
                                ImageData.Visible = true;
                                ImageData.ImageUrl = ResponseMsg;
                            }
                            else
                            {
                                lblStartResponse.Text = ResponseMsg.ToString();
                            }
                        }
                        else
                        {
                            lblStartResponse.Text = response.StatusCode.ToString();
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// This Method Is Used to Close Popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPopUpCancel_Click(object sender, EventArgs e)
    {
        PopUdRowerBoatNo.Hide();
        modPopUpVisible.Style.Add("display", "none");
    }
    /// <summary>
    /// This Method Is Used to Add Boat Number And Rower.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            foreach (GridViewRow gvr in gvTripSheetSettelementStart.Rows)
            {
                CheckBox chkBox = (CheckBox)gvr.FindControl("gvTripStartChkBox");
                if (chkBox.Checked)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Atleast One Ticket.');", true);
                return;
            }

            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "validateCheckedCheckBox();", true);

            getBoatNumber(ddlBoatType.SelectedValue.Trim(), ddlSeaterType.SelectedValue.Trim());
            getRower();
            PopUdRowerBoatNo.Show();
            modPopUpVisible.Style.Add("display", "Block");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
        finally
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "getBookingIdAndBoatRefNo();", true);
        }
    }
    /// <summary>
    /// This Function Is Used To Call Trip Start Function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPopUpOkay_Click(object sender, EventArgs e)
    {
        TripStart();
        PopUdRowerBoatNo.Hide();
        modPopUpVisible.Style.Add("display", "None");
    }
    /// <summary>
    /// This Method Is Used To Get Boat Number Based On BoatTypeId And SeaterId.
    /// </summary>
    /// <param name="sBoatTypeId">This Parameter Is Used To Pass BoatTypeId</param>
    /// <param name="sSeaterId">This Parameter Is Used To Pass SeaterId</param>
    /// <returns></returns>
    public void getBoatNumber(string sBoatTypeId, string sSeaterId)
    {
        try
        {
            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var individualTrip = new IndividualTripSheet()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = sBoatTypeId.ToString(),
                    BoatSeaterId = sSeaterId.ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ddlBoatMaster/BHId", individualTrip).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (ResponseMsg == "No Records Found.")
                    {
                        ddlBoatNo.Items.Insert(0, new ListItem("Select Boat Type", "0"));
                        return;
                    }
                    dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (StatusCode == 1)
                    {
                        ddlBoatNo.Items.Clear();
                        if (dt.Rows.Count > 0)
                        {
                            ddlBoatNo.DataSource = dt;
                            ddlBoatNo.DataTextField = "BoatNum";
                            ddlBoatNo.DataValueField = "BoatId";
                            ddlBoatNo.DataBind();
                        }
                        ddlBoatNo.Items.Insert(0, new ListItem("Select Boat No", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }
    /// <summary>
    /// This Function Is Used To get Rower
    /// </summary>
    /// <returns></returns>
    public void getRower()
    {
        try
        {
            DataTable dt = new DataTable();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var IndividualTrip = new IndividualTripSheet();
                HttpResponseMessage response;
                if (Session["TripUser"].ToString().Trim() == "D")
                {
                    IndividualTrip = new IndividualTripSheet()
                    {
                        QueryType = "GetRower",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = "0",
                        UserRole = "",
                        BoatTypeId = "0",
                        BoatSeaterId = "0",
                        RowerId = "0",
                        TripUser = "D"
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndividualTrip).Result;
                }
                else
                {
                    IndividualTrip = new IndividualTripSheet()
                    {
                        QueryType = "GetRower",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                        BoatSeaterId = ddlSeaterType.SelectedValue.Trim(),
                        UserId = "0",
                        UserRole = "",
                        RowerId = "0",
                        TripUser = ""
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndividualTrip).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var responseMsg = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt16(JObject.Parse(responseMsg)["StatusCode"].ToString());
                    string sResponseMsg = JObject.Parse(responseMsg)["DataTable"].ToString();
                    string sSuccessOrFailureMsg = JObject.Parse(responseMsg)["ResponseMsg"].ToString();
                    if (sSuccessOrFailureMsg == "No Records Found.")
                    {
                        ddlRower.Items.Insert(0, new ListItem("Select Rower", "0"));
                        return;
                    }
                    dt = JsonConvert.DeserializeObject<DataTable>(sResponseMsg);

                    if (StatusCode == 1)
                    {
                        ddlRower.Items.Clear();
                        if (dt.Rows.Count > 0)
                        {
                            ddlRower.DataSource = dt;
                            ddlRower.DataTextField = "RowerName";
                            ddlRower.DataValueField = "RowerId";
                            ddlRower.DataBind();
                        }
                        ddlRower.Items.Insert(0, new ListItem("Select Rower", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }
    /// <summary>
    /// This Method Is Used To Start Trip.
    /// </summary>
    public void TripStart()
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
                string[] splitBookingId = hfBookingIdArray.Value.Split(',');
                string[] splitBoatRefNo = hfBoatRefNoArray.Value.Split(',');
                var IndTripStart = new IndividualTripStart()
                {
                    QueryType = "IndividualTripStart",
                    BookingId = splitBookingId,
                    BoatReferenceNo = splitBoatRefNo,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    RowerId = ddlRower.SelectedValue.Trim(),
                    TripStartTime = "",
                    TripEndTime = "",//End Time Pass Empty
                    ActualBoatId = ddlBoatNo.SelectedValue.Trim(),
                    SSUserBy = Session["UserId"].ToString().Trim(),
                    SDUserBy = Session["UserId"].ToString().Trim(),
                    BookingMedia = "DW"
                };
                response = client.PostAsJsonAsync("InividualNewTripSheetWeb", IndTripStart).Result;
                sMSG = "Inserted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindTripStart();
                        lblStartResponse.Text = "Trip Started SuccessFully !!!.";
                        ddlBoatType.SelectedValue = "0";
                        ddlSeaterType.Items.Clear();
                        txtStartDetails.Focus();
                    }
                    else
                    {
                        lblStartResponse.Text = ResponseMsg.ToString();
                    }
                }
                else
                {
                    lblStartResponse.Text = response.ReasonPhrase.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }
    /// <summary>
    /// This Function Used to Bind TripStart GridView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTripStart_Click(object sender, EventArgs e)
    {
        txtStartDetails.Text = "";
        txtStartDetails.Focus();
        lblStartResponse.Text = string.Empty;
        BindTripStart();
    }
    /// <summary>
    /// This Class Is Used For Bind Grid Views
    /// </summary>
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
    /// <summary>
    /// This Class Is Used For QR Code.
    /// </summary>
    public class QR
    {
        public string BoatHouseId { get; set; }
        public string BookingId { get; set; }
        public string Pin { get; set; }
        public string BookingRef { get; set; }
    }
    /// <summary>
    /// This Class Is Used For Trip Start.
    /// </summary>
    public class IndividualTripStart
    {
        public string QueryType { get; set; }
        public string[] BookingId { get; set; }
        public string[] BoatReferenceNo { get; set; }
        public string BoatId { get; set; }
        public string BoatNum { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatType { get; set; }
        public string RowerId { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string ActualBoatId { get; set; }
        public string BoatHouseName { get; set; }
        public string BookingMedia { get; set; }
        public string SSUserBy { get; set; }
        public string SDUserBy { get; set; }
    }

    /*****************************TripEnd**********************************/

    /// <summary>
    /// This Method Is Used TO The Bind The Started TripS .
    /// </summary>
    public void BindTripEnd()
    {
        try
        {
            BindTripCountDetails();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var IndtripSheet = new IndividualTripSheet()
                    {
                        QueryType = "TripEndAdmin",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = "0",
                        UserRole = "",
                        RowerId = "0",
                        BoatTypeId = "0",
                        TripUser = "",
                        BoatSeaterId = "0"
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndtripSheet).Result;
                }
                else
                {
                    var IndtripSheet = new IndividualTripSheet()
                    {
                        QueryType = "TripEndUser",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        UserRole = "",
                        RowerId = "0",
                        BoatTypeId = "0",
                        TripUser = "",
                        BoatSeaterId = "0"
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndtripSheet).Result;
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
                            gvTripSheetSettelementEnd.DataSource = dt;
                            gvTripSheetSettelementEnd.DataBind();
                            //ViewState["ToolTipBKid"] = dt.Rows[0]["BookingId"].ToString();
                            //Label lblActualBookId = (Label)gvTripSheetSettelementEnd.FindControl("lblActualBoatId");
                            //lblActualBookId.ToolTip = ViewState["ToolTipBKid"].ToString();
                            gvTripSheetSettelementEnd.Visible = true;
                            divmsgEnd.Visible = false;

                            divGridEnd.Visible = true;
                            divGridStart.Visible = false;
                            divGridClosed.Visible = false;
                        }
                        else
                        {
                            gvTripSheetSettelementEnd.DataSource = dt;
                            gvTripSheetSettelementEnd.DataBind();
                            gvTripSheetSettelementEnd.Visible = false;
                            lblGridMsgStart.Text = sSuccessOrFailureMsg;
                            divmsgEnd.Visible = true;

                            divGridEnd.Visible = true;
                            divGridStart.Visible = false;
                            divGridClosed.Visible = false;
                        }
                    }
                    else
                    {
                        gvTripSheetSettelementEnd.Visible = false;
                        lblGridMsgEnd.Text = sSuccessOrFailureMsg;
                        divmsgEnd.Visible = true;
                        ImgBtnEnd.Visible = false;
                        PopUdRowerBoatNo.Hide();
                        modPopUpVisible.Style.Add("display", "none");
                        gvTripSheetSettelementEnd.Visible = false;

                        divGridEnd.Visible = true;
                        divGridStart.Visible = false;
                        divGridClosed.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }


    /// <summary>
    /// This Method Is Used On The End Button Click Event.
    /// </summary>
    protected void btnTripEnd_Click(object sender, EventArgs e)
    {
        txtEndDetails.Text = "";
        lblEndResponse.Text = "";
        BindTripEnd();
        txtEndDetails.Focus();
    }
    /// <summary>
    /// This Method Is Used To call TripEnd.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgBtnEnd_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ViewState["BookingId"] = "";
            ViewState["ActualBoatId"] = "";
            ViewState["ActualBoatNum"] = "";
            ViewState["Rower"] = "";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            int count = 0;
            foreach (GridViewRow dr in gvTripSheetSettelementEnd.Rows)
            {
                CheckBox chkItem = (CheckBox)dr.FindControl("gvTripEndChkBox");

                Label lblTripStartTime = (Label)dr.FindControl("lblTripStartTime");
                ViewState["TripStartTime"] = lblTripStartTime.Text;

                if (chkItem.Checked)
                {
                    count++;
                    Label BookingId = (Label)dr.FindControl("lblBookingId");
                    ViewState["BookingId"] += BookingId.Text + '.';

                    Label lblActualBoatId = (Label)dr.FindControl("lblActualBoatId");
                    ViewState["ActualBoatId"] += lblActualBoatId.Text + '~';

                    Label lblActualBoatNum = (Label)dr.FindControl("lblActualBoatNum");
                    ViewState["ActualBoatNum"] += lblActualBoatNum.Text + '~';


                    Label lblRowerId = (Label)dr.FindControl("lblRowerId");

                    string Rower = string.Empty;
                    if (lblRowerId.Text == null || lblRowerId.Text == "")
                    {
                        Rower = "0";
                        ViewState["Rower"] += Rower.ToString() + '~';
                    }
                    else
                    {
                        Rower = lblRowerId.Text;
                        ViewState["Rower"] += Rower.ToString() + '~';
                    }

                }

            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please, Select Atleast One Ticket.');", true);
                return;
            }
            TripEnd();
        }
        catch (Exception ex)
        {
            lblEndResponse.Text = ex.ToString();
        }

    }
    /// <summary>
    /// This Method Is Used To End Trip.
    /// </summary>
    public void TripEnd()
    {
        try
        {
            string BookingId = string.Empty;
            string ActualBoatNum = string.Empty;
            string RowerId = string.Empty;
            string ActualBoatId = string.Empty;

            string[] sBookingId;
            string[] sActualBoatNum;
            string[] sRowerId;
            string[] sActualBoatId;

            BookingId = ViewState["BookingId"].ToString();
            sBookingId = BookingId.Split('.');

            ActualBoatNum = ViewState["ActualBoatNum"].ToString();
            sActualBoatNum = ActualBoatNum.Split('~');

            RowerId = ViewState["Rower"].ToString();
            sRowerId = RowerId.Split('~');

            ActualBoatId = ViewState["ActualBoatId"].ToString();
            sActualBoatId = ActualBoatId.TrimEnd().Split('~');

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                string sMSG = string.Empty;

                var vTripSheetSettlement2 = new IndividualTripEnd()
                {
                    QueryType = "IndividualTripEnd",
                    BookingId = sBookingId,
                    BoatReferenceNo = sActualBoatId,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    RowerIds = sRowerId,
                    TripStartTime = ViewState["TripStartTime"].ToString().Trim(),//Start Time Empty
                    TripEndTime = "",
                    BoatId = sActualBoatNum,
                    SSUserBy = Session["UserId"].ToString().Trim(),
                    SDUserBy = Session["UserId"].ToString().Trim(),
                    BookingMedia = "DW"
                };

                response = client.PostAsJsonAsync("IndividualNewTripSheetWebEnd", vTripSheetSettlement2).Result;
                sMSG = "Inserted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindTripEnd();
                        txtEndDetails.Text = "";
                        lblEndResponse.Text = "";
                        BindTripEnd();
                        lblEndResponse.Text = "Trip Ended Successfully !!!.";
                        txtEndDetails.Text = "";
                        txtEndDetails.Focus();
                        txtEndDetails.Focus();
                    }
                    else
                    {
                        lblEndResponse.Text = ResponseMsg.ToString();
                    }
                }
                else
                {
                    lblEndResponse.Text = response.ReasonPhrase.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            lblEndResponse.Text = ex.ToString();
        }
        return;
    }
    /// <summary>
    /// This Class Is Used For Trip End.
    /// </summary>
    public class IndividualTripEnd
    {
        public string QueryType { get; set; }
        public string[] BookingId { get; set; }
        public string[] BoatReferenceNo { get; set; }
        public string[] BoatId { get; set; }
        public string BoatNum { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatType { get; set; }
        public string RowerId { get; set; }
        public string[] RowerIds { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string ActualBoatId { get; set; }
        public string BoatHouseName { get; set; }
        public string BookingMedia { get; set; }
        public string SSUserBy { get; set; }
        public string SDUserBy { get; set; }
    }

    /***********************************Trip Closed*******************************/
    /// <summary>
    /// This Method Is Used To Bind Trip Closed
    /// </summary>
    public void BindTripClosed()
    {
        try
        {
            BindTripCountDetails();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (Session["UserRole"].ToString().Trim() == "Sadmin" || Session["UserRole"].ToString().Trim() == "Admin")
                {
                    var IndtripSheet = new IndividualTripSheet()
                    {
                        QueryType = "TripClosedAdmin",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = "0",
                        UserRole = "",
                        RowerId = "0",
                        BoatTypeId = "0",
                        TripUser = "",
                        BoatSeaterId = "0"
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndtripSheet).Result;
                }
                else
                {
                    var IndtripSheet = new IndividualTripSheet()
                    {
                        QueryType = "TripClosedUser",
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        UserId = Session["UserId"].ToString().Trim(),
                        UserRole = "",
                        RowerId = "0",
                        BoatTypeId = "0",
                        TripUser = "",
                        BoatSeaterId = "0"
                    };
                    response = client.PostAsJsonAsync("IndividualTripSheet/View", IndtripSheet).Result;
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
                            gvTripSheetSettelementClosed.DataSource = dt;
                            gvTripSheetSettelementClosed.DataBind();
                            gvTripSheetSettelementClosed.Visible = true;
                            divMsgStart.Visible = false;
                            divmsgClosed.Visible = false;

                            divGridClosed.Visible = true;
                            divGridStart.Visible = false;
                            divGridEnd.Visible = false;
                        }
                        else
                        {
                            gvTripSheetSettelementClosed.DataSource = dt;
                            gvTripSheetSettelementClosed.DataBind();
                            gvTripSheetSettelementClosed.Visible = false;
                            lblGridMsgStart.Text = sSuccessOrFailureMsg;
                            divMsgStart.Visible = true;

                            divGridClosed.Visible = true;
                            divGridStart.Visible = false;
                            divGridEnd.Visible = false;
                        }
                    }
                    else
                    {
                        gvTripSheetSettelementEnd.Visible = false;
                        divGridEnd.Visible = true;
                        lblGridMsgEnd.Text = sSuccessOrFailureMsg;
                        divmsgEnd.Visible = true;
                        ImgBtnEnd.Visible = false;
                        PopUdRowerBoatNo.Hide();
                        modPopUpVisible.Style.Add("display", "none");
                        divGridClosed.Visible = true;
                        divGridStart.Visible = false;
                        divGridEnd.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }
    /// <summary>
    /// This Method Is Used To Call BindTripClosed Method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTripClosed_Click(object sender, EventArgs e)
    {
        BindTripClosed();
    }


    protected void BackToTripSheet_Click(object sender, EventArgs e)
    {
        Response.Redirect("TripSheetWeb.aspx");
    }


}