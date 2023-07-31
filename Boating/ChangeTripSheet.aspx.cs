using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_ChangeTripSheet : System.Web.UI.Page
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

                txtBookingDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtBookingDate.Attributes.Add("readonly", "readonly");

                hfBookingDate.Value = DateTime.Now.ToString("dd-MM-yyyy");

                if (Session["UserRole"].ToString() == "Sadmin")
                {
                    divsadmintrpsheet.Visible = true;
                    divtripSheet.Visible = true;
                }
                else
                {
                    divsadmintrpsheet.Visible = false;
                    divtripSheet.Visible = true;
                }

                if (Session["UserRole"].ToString() == "Sadmin")
                {
                    if (Session["BBMChangeTripSheet"].ToString().Trim() == "Y")
                    {
                        BindBoatDetails("ChangeTripSheetNewPagingSadmin", "", "1", "10");
                        GetTimeStamp();
                    }
                }
                else
                {
                    if (Session["BBMChangeTripSheet"].ToString().Trim() == "Y")
                    {
                        BindBoatDetails("ChangeTripSheetNewPaging", "", "1", "10");
                        GetTimeStamp();
                    }
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    protected void txtendtime_TextChanged(object sender, EventArgs e)
    {
        if (txtNewstarttime.Text == "")
        {
            txtNewstarttime.Text = string.Empty;
            txtNewendtime.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Trip Start time');", true);
        }
        else
        {
            DateTime startTime = DateTime.Parse(txtNewstarttime.Text);
            DateTime endTime = DateTime.Parse(txtNewendtime.Text);

            TimeSpan span = endTime.Subtract(startTime);
            txtNewTravelDuration.Text = Convert.ToString(span.TotalMinutes);


            if (startTime > endTime)
            {

                txtNewDepositRefundAmt.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Trip End Time should not be Less than Trip Start Time ');", true);
                txtNewTravelDuration.Text = string.Empty;
            }

            else if (startTime == endTime)
            {

                txtNewDepositRefundAmt.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Trip End Time should not be Equal to Trip Start Time ');", true);
                txtNewTravelDuration.Text = string.Empty;
            }
            //else if (Convert.ToInt32(txtNewTravelDuration.Text.Trim()) <= Convert.ToInt32(txtNewBoatDuration.Text.Trim()))
            else if (Convert.ToInt32(txtNewTravelDuration.Text.Trim()) <= Convert.ToInt32(txtOldTravelledDuration.Text.Trim()))
            {
                GetRefundAmount();
            }          
            else
            {
                //GetRefundAmount();
                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Trip End Time Should not be Greater than Boat Duration ');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Trip End Time Should not be Greater than Old Travel Duration ');", true);                
                txtNewTravelDuration.Text = string.Empty;
                txtNewDepositRefundAmt.Text = string.Empty;
            }
        }
    }
    protected void txtstarttime_TextChanged(object sender, EventArgs e)
    {
        if (txtNewstarttime.Text == "")
        {
            txtNewstarttime.Text = DateTime.Now.ToString("hh:mm");
            txtNewendtime.Text = DateTime.Now.ToString("hh:mm");
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Trip Start time');", true);
        }
        else
        {
            DateTime startTime = DateTime.Parse(txtNewstarttime.Text);
        }
    }
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        divReScheduleBoat.Visible = true;
        divGridList.Visible = false;
        divLogDetails.Visible = false;
        lbtnView.Visible = false;
        lbtnGrid.Visible = false;
        divtripSheet.Visible = false;

        ImageButton lnktrips = sender as ImageButton;
        GridViewRow gvrow = lnktrips.NamingContainer as GridViewRow;
        Label BookingId = (Label)gvrow.FindControl("lblBookingId");
        ViewState["BookingId"] = BookingId.Text;
        txtBookId.Text = BookingId.Text;

        Label OldRowerId = (Label)gvrow.FindControl("lblrowerid");
        //ViewState["RowerId"] = OldRowerId.Text;

        Label BoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
        ViewState["BoatTypeId"] = BoatTypeId.Text;

        Label BoatType = (Label)gvrow.FindControl("lblBoatType");
        txtBtType.Text = BoatType.Text;

        Label SeatTypeId = (Label)gvrow.FindControl("lblBoatseatId");
        ViewState["SeatTypeId"] = SeatTypeId.Text;
        Label SeatType = (Label)gvrow.FindControl("lblBoatseat");
        txtBtSeater.Text = SeatType.Text;

        Label BoatReferenceNo = (Label)gvrow.FindControl("lblBoatRefNo");
        ViewState["BoatReferenceNo"] = BoatReferenceNo.Text;

        Label OldBoatNum = (Label)gvrow.FindControl("lblBoatNum");
        ViewState["OldBoatNum"] = OldBoatNum.Text;

        Label BookingPin = (Label)gvrow.FindControl("lblBookingPin");
        ViewState["BookingPin"] = BookingPin.Text;
        txtBookPin.Text = BookingPin.Text;

        Label ExpectedTime = (Label)gvrow.FindControl("lblExpectedTime");
        ViewState["OldExpectedTime"] = ExpectedTime.Text.Trim();

        Label OldBoatDuration = (Label)gvrow.FindControl("lblOldBoatDuration");
        txtOldBoatDuration.Text = OldBoatDuration.Text.Trim();
        txtNewBoatDuration.Text = OldBoatDuration.Text.Trim();


        Label OldTravelDuration = (Label)gvrow.FindControl("lblOldTravelDuration");
        txtOldTravelledDuration.Text = OldTravelDuration.Text.Trim();



        GetTimeStamp();
        if (ExpectedTime.Text.Length > 0)
        {
            string[] array = ExpectedTime.Text.Trim().Split(':');
            string sHour = string.Empty;

            if (array[0].Length < 2)
            {
                sHour = "0" + ExpectedTime.Text.Trim();
            }
            else
            {
                sHour = ExpectedTime.Text.Trim();
            }

            ddlExpectedTime.SelectedValue = sHour.Trim();
        }
        else
        {
            DateTime d = DateTime.Parse(DateTime.Now.ToString("HH:mm"));
            txtstarttime.Text = d.ToString("h:mmtt");

            ddlExpectedTime.SelectedValue = "0" + d.ToString("h:mmtt");
        }

        if (hfBookingDate.Value == txtBookingDate.Text)
        {
            txtTripStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNewTripStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Label oldTripStartTime = (Label)gvrow.FindControl("lbloldTripStartTime");

            if (oldTripStartTime.Text.Length > 0)
            {
                DateTime d = DateTime.Parse(oldTripStartTime.Text.Trim(), objEnglishDate);
                txtstarttime.Text = d.ToString("HH:mm");
                txtNewstarttime.Text = d.ToString("HH:mm");
                ViewState["oldTripStartTime"] = txtTripStartDate.Text + " " + txtstarttime.Text;
                ViewState["NewTripStartTime"] = txtNewTripStartDate.Text + " " + txtNewstarttime.Text;
            }
            else
            {
                txtstarttime.Text = DateTime.Now.ToString("hh:mm");
                txtNewstarttime.Text = DateTime.Now.ToString("hh:mm");
                ViewState["oldTripStartTime"] = txtTripStartDate.Text + " " + txtstarttime.Text;
                ViewState["NewTripStartTime"] = txtNewTripStartDate.Text + " " + txtNewstarttime.Text;
            }


            txtenddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNewenddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Label oldTripEndTime = (Label)gvrow.FindControl("lbloldTripEndTime");


            if (oldTripEndTime.Text.Length > 0)
            {
                DateTime d = DateTime.Parse(oldTripEndTime.Text.Trim(), objEnglishDate);
                txtendtime.Text = d.ToString("HH:mm");
                txtNewendtime.Text = d.ToString("HH:mm");
                ViewState["oldTripEndTime"] = txtenddate.Text + " " + txtendtime.Text;
                ViewState["NewTripEndTime"] = txtNewenddate.Text + " " + txtNewendtime.Text;

            }
            else
            {
                txtendtime.Text = DateTime.Now.ToString("hh:mm");
                txtNewendtime.Text = DateTime.Now.ToString("hh:mm");
                ViewState["oldTripEndTime"] = txtenddate.Text + " " + txtendtime.Text;
                ViewState["NewTripEndTime"] = txtNewenddate.Text + " " + txtNewendtime.Text;
            }

        }
        else
        {
            txtTripStartDate.Text = txtBookingDate.Text;
            txtNewTripStartDate.Text = txtBookingDate.Text;
            Label oldTripStartTime = (Label)gvrow.FindControl("lbloldTripStartTime");

            if (oldTripStartTime.Text.Length > 0)
            {
                DateTime d = DateTime.Parse(oldTripStartTime.Text.Trim(), objEnglishDate);
                txtstarttime.Text = d.ToString("HH:mm");
                txtNewstarttime.Text = d.ToString("HH:mm");
                ViewState["oldTripStartTime"] = txtTripStartDate.Text + " " + txtstarttime.Text;
                ViewState["NewTripStartTime"] = txtNewTripStartDate.Text + " " + txtNewstarttime.Text;
            }
            else
            {
                txtstarttime.Text = DateTime.Now.ToString("hh:mm");
                txtNewstarttime.Text = DateTime.Now.ToString("hh:mm");
                ViewState["oldTripStartTime"] = txtTripStartDate.Text + " " + txtstarttime.Text;
                ViewState["NewTripStartTime"] = txtNewTripStartDate.Text + " " + txtNewstarttime.Text;
            }


            txtenddate.Text = txtBookingDate.Text;
            txtNewenddate.Text = txtBookingDate.Text;
            Label oldTripEndTime = (Label)gvrow.FindControl("lbloldTripEndTime");


            if (oldTripEndTime.Text.Length > 0)
            {
                DateTime d = DateTime.Parse(oldTripEndTime.Text.Trim(), objEnglishDate);
                txtendtime.Text = d.ToString("HH:mm");
                txtNewendtime.Text = d.ToString("HH:mm");
                ViewState["oldTripEndTime"] = txtenddate.Text + " " + txtendtime.Text;
                ViewState["NewTripEndTime"] = txtNewenddate.Text + " " + txtNewendtime.Text;
            }
            else
            {
                txtendtime.Text = DateTime.Now.ToString("hh:mm");
                txtNewendtime.Text = DateTime.Now.ToString("hh:mm");
                ViewState["oldTripEndTime"] = txtenddate.Text + " " + txtendtime.Text;
                ViewState["NewTripEndTime"] = txtNewenddate.Text + " " + txtNewendtime.Text;
            }


        }


        Label BoatStatus = (Label)gvrow.FindControl("lblStatus");
        hfBoatStatus.Value = BoatStatus.Text;
        txtPremiumStatus.Text = BoatStatus.Text;

        GetBoatNum();
        GetRowerName();
        Label ActualId = (Label)gvrow.FindControl("lblActualBoatId");
        ViewState["OldBaotId"] = ActualId.Text;
        hfActualId.Value = ActualId.Text;

        if (ActualId.Text == "0" || ActualId.Text == "")
        {
            ddlBoatNum.SelectedValue = "0";
        }
        else
        {
            ddlBoatNum.SelectedValue = ActualId.Text;
        }
        string Rower = string.Empty;
        if (OldRowerId.Text == "0" || OldRowerId.Text == "")
        {
            ddlrowername.SelectedValue = "0";
            Rower = "0";
            ViewState["RowerId"] = Rower;
            ddlrowername.Enabled = false;
        }
        else
        {
            ViewState["RowerId"] = OldRowerId.Text;
            ddlrowername.SelectedValue = ViewState["RowerId"].ToString().Trim();
            ddlrowername.Enabled = false;
        }

        Label OldBoatDeposit = (Label)gvrow.FindControl("lblOldBoatDeposit");
        txtOldBoatDeposit.Text = OldBoatDeposit.Text.Trim();
        Label OldDepRefundAmount = (Label)gvrow.FindControl("lblOldDepRefundAmount");
        txtOldDepositRefundAmt.Text = OldDepRefundAmount.Text.Trim();
    }
    protected void btnReCancel_Click(object sender, EventArgs e)
    {
        txtBookingPin.Text = string.Empty;
        GvChangeBoatBooking.Visible = false;

        if (Session["UserRole"].ToString() == "Sadmin")
        {
            divsadmintrpsheet.Visible = true;
            divtripSheet.Visible = true;
        }
        else
        {
            divsadmintrpsheet.Visible = false;
            divtripSheet.Visible = true;
        }
        divReScheduleBoat.Visible = false;
        divGridList.Visible = true;
        divLogDetails.Visible = false;
        lbtnView.Visible = true;
        lbtnGrid.Visible = false;
        clearInputs();
        BackToTableList();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserRole"].ToString() == "Sadmin")
            {
                divsadmintrpsheet.Visible = true;
                divtripSheet.Visible = true;
            }
            else
            {
                divsadmintrpsheet.Visible = false;
                divtripSheet.Visible = true;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                string sMSG = string.Empty;

                if (ViewState["RowerId"].ToString() != "0")
                {
                    if (ddlrowername.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select the Rower');", true);
                        return;
                    }
                }

                var Reschedule = new BoatSearch()
                {
                    QueryType = "RescheduleLog",
                    BookingId = ViewState["BookingId"].ToString().Trim(),
                    BookingPin = ViewState["BookingPin"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BoatHouseName = Session["BoatHouseName"].ToString(),
                    BoatTypeId = ViewState["BoatTypeId"].ToString().Trim(),
                    BoatSeaterId = ViewState["SeatTypeId"].ToString().Trim(),
                    BoatReferenceNum = ViewState["BoatReferenceNo"].ToString().Trim(),

                    OldBoatId = ViewState["OldBaotId"].ToString().Trim(),
                    OldBoatNum = ViewState["OldBoatNum"].ToString().Trim(),
                    OldRowerId = ViewState["RowerId"].ToString().Trim(),
                    OldExpectedTime = ViewState["OldExpectedTime"].ToString().Trim(),
                    OldTripStartTime = ViewState["oldTripStartTime"].ToString().Trim(),
                    OldTripEndTime = ViewState["oldTripEndTime"].ToString().Trim(),

                    NewBoatNum = ddlBoatNum.SelectedItem.ToString(),
                    NewBoatId = ddlBoatNum.SelectedValue.Trim(),
                    NewRowerId = ddlrowername.SelectedValue.Trim(),
                    NewTripStartTime = txtNewTripStartDate.Text + " " + txtNewstarttime.Text,
                    NewTripEndTime = txtNewenddate.Text + " " + txtNewendtime.Text,
                    NewExpectedTime = ddlExpectedTime.SelectedValue.Trim(),
                    OldDepRefundAmount = txtOldDepositRefundAmt.Text.Trim(),
                    NewDepRefundAmount = txtNewDepositRefundAmt.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString()
                };
                response = client.PostAsJsonAsync("RescheduleBoardingPass", Reschedule).Result;
                sMSG = "Trip Sheet Updated Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        clearInputs();
                        divLogDetails.Visible = false;
                        divGridList.Visible = true;
                        divReScheduleBoat.Visible = false;
                        lbtnView.Visible = true;
                        lbtnGrid.Visible = false;
                        txtBookingPin.Text = string.Empty;
                        GvChangeBoatBooking.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                        BackToTableList();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void lbtnViewLog_Click(object sender, EventArgs e)
    {
        divLogDetails.Visible = true;
        divtripSheet.Visible = false;
        divGridList.Visible = false;
        divReScheduleBoat.Visible = false;
        lbtnView.Visible = false;
        btnNextLog.Visible = true;
        lbtnGrid.Visible = true;
        BindLogDetails("ViewLogDetails", "", "1", "10");
    }
    protected void lbtnGrid_Click(object sender, EventArgs e)
    {
        txtBookingPin.Text = string.Empty;
        GvChangeBoatBooking.Visible = false;
        btnNext.Visible = true;
        divLogDetails.Visible = false;
        divtripSheet.Visible = true;
        divGridList.Visible = true;
        divReScheduleBoat.Visible = false;
        lbtnView.Visible = true;
        lbtnGrid.Visible = false;
        BindBoatDetails("ChangeTripSheetNewPaging", "", "1", "10");
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
    protected void ImgBtnView_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string Id = gvViewLogDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            Label lblBookId = (Label)gvrow.FindControl("lblBookingId");
            Label lblBoatType = (Label)gvrow.FindControl("lblBoatType");
            Label lblSeaterType = (Label)gvrow.FindControl("lblSeaterType");
            Label lblBookPin = (Label)gvrow.FindControl("lblBookingPin");
            Label lblBookRef = (Label)gvrow.FindControl("lblBoatRefNo");
            Label lblRowerName = (Label)gvrow.FindControl("lblRowerName");
            ViewState["BookingId"] = lblBookId.Text.Trim();
            ViewState["BoatType"] = lblBoatType.Text.Trim();
            ViewState["SeaterType"] = lblSeaterType.Text.Trim();
            ViewState["BookingPin"] = lblBookPin.Text.Trim();
            ViewState["BookingRef"] = lblBookRef.Text.Trim();
            ViewState["RowerName"] = lblRowerName.Text.Trim();

            lblPopBookId.Text = ViewState["BookingId"].ToString();
            lblPopBoatType.Text = ViewState["BoatType"].ToString();
            lblPopBoatSeat.Text = ViewState["SeaterType"].ToString();
            lblPopBookPin.Text = ViewState["BookingPin"].ToString();
            lblPopBookRef.Text = ViewState["BookingRef"].ToString();
            lblPopRower.Text = ViewState["RowerName"].ToString();

            BindPopOldLogDetails();
            BindPopNewLogDetails();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }
    private void GetTimeStamp()
    {

        //String hourMinute = DateTime.Now.ToString("HH:mm");
        String hourMinute = DateTime.Now.ToString("00:00");
        DateTime StartTime = DateTime.ParseExact(hourMinute, "HH:mm", null);

        int hour = StartTime.Hour; // 10 

        //Set the end time (23:59 means 11:59 PM)
        String BookingTo = DateTime.Now.ToString("23:59");
        DateTime EndTime = DateTime.ParseExact(BookingTo, "HH:mm", null);

        //Set 1 minutes interval
        TimeSpan Interval = new TimeSpan(0, 1, 0);

        DataTable myTable;
        DataRow myNewRow;

        // Create a new DataTable.
        myTable = new DataTable("My Table");
        myTable.Columns.Add(new DataColumn("DateTimeCol", typeof(string)));

        while (StartTime <= EndTime)
        {
            //ADDING ROW TO DATA-Table
            myNewRow = myTable.NewRow();

            string[] array = StartTime.ToShortTimeString().Trim().Split(':');
            string sHour = string.Empty;

            if (array[0].Length < 2)
            {
                sHour = "0" + StartTime.ToShortTimeString().Trim();
            }
            else
            {
                sHour = StartTime.ToShortTimeString().Trim();
            }

            myNewRow["DateTimeCol"] = sHour.Trim().Replace(" ", "");
            myTable.Rows.Add(myNewRow);



            StartTime = StartTime.Add(Interval);
        }

        // ViewState["TimeStampDataTable"] = myTable;

        ddlExpectedTime.DataSource = myTable;
        ddlExpectedTime.DataValueField = "DateTimeCol";
        ddlExpectedTime.DataTextField = "DateTimeCol";
        ddlExpectedTime.DataBind();
    }
    public void GetBoatNum()
    {
        try
        {
            ddlBoatNum.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (hfBoatStatus.Value == "Normal")
                {
                    hfBoatStatus.Value = "N";
                }
                else
                {
                    hfBoatStatus.Value = "P";
                }

                var boatmaster = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ViewState["BoatTypeId"].ToString().Trim(),
                    BoatSeaterId = ViewState["SeatTypeId"].ToString().Trim(),
                    BoatStatus = hfBoatStatus.Value.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("ddlBoatNumStatus/BHId", boatmaster).Result;

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
                            ddlBoatNum.DataSource = dt;
                            ddlBoatNum.DataValueField = "BoatId";
                            ddlBoatNum.DataTextField = "BoatNum";
                            ddlBoatNum.DataBind();

                        }
                        else
                        {
                            ddlBoatNum.DataBind();
                        }

                        ddlBoatNum.Items.Insert(0, new ListItem("Select Boat Number", "0"));
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
    public void GetRefundAmount()
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

                var Reschedule = new BoatSearch()
                {
                    QueryType = "GetRefundAmt",
                    BookingId = ViewState["BookingId"].ToString().Trim(),
                    BoatReferenceNo = ViewState["BoatReferenceNo"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BoatHouseName = Session["BoatHouseName"].ToString(),
                    ActualBoatId = ddlBoatNum.SelectedValue.Trim(),
                    RowerId = ddlrowername.SelectedValue.Trim(),
                    TripStartTime = txtNewTripStartDate.Text + " " + txtNewstarttime.Text,
                    TripEndTime = txtNewenddate.Text + " " + txtNewendtime.Text,

                    BookingMedia = "B"
                };
                response = client.PostAsJsonAsync("TripSheetRefundAmt", Reschedule).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        txtNewDepositRefundAmt.Text = ResponseMsg.ToString().Trim();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void BindBoatDetails(string sQueryType, string sBookingPin_Or_BookingId, string sFirstPageNo, string sLastPageNo)
    {
        try
        {
            divGridList.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch();
                if (Session["UserRole"].ToString() == "Sadmin")
                {
                    BoatSearch = new BoatSearch()
                    {
                        QueryType = sQueryType.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingPin = sBookingPin_Or_BookingId.ToString().Trim(),
                        BookingId = sBookingPin_Or_BookingId.ToString().Trim(),
                        FirstPageNo = sFirstPageNo.ToString().Trim(),
                        LastPageNo = sLastPageNo.ToString().Trim(),
                        Date = txtBookingDate.Text.Trim()
                    };
                }
                else
                {
                    BoatSearch = new BoatSearch()
                    {
                        QueryType = sQueryType.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingPin = sBookingPin_Or_BookingId.ToString().Trim(),
                        BookingId = sBookingPin_Or_BookingId.ToString().Trim(),
                        FirstPageNo = sFirstPageNo.ToString().Trim(),
                        LastPageNo = sLastPageNo.ToString().Trim(),
                        Date = DateTime.Now.ToString("dd/MM/yyyy")
                    };
                }
                HttpResponseMessage response = client.PostAsJsonAsync("ChangeBoatBookingV2", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                        if (dt.Rows.Count > 0)
                        {
                            if (sFirstPageNo != "")
                            {
                                divBackToTableLsit.Visible = false;
                                divPreviousAndNext.Visible = true;
                            }
                            else
                            {

                                divBackToTableLsit.Visible = true;
                                divPreviousAndNext.Visible = false;
                            }
                            if (Session["UserRole"].ToString() == "Sadmin")
                            {
                                if (GetCount("getCountSadmin", txtBookingDate.Text.Trim()) <= 10)
                                {
                                    //divBackToTableLsit.Visible = false;
                                    divPreviousAndNext.Visible = false;
                                }
                            }
                            else
                            {
                                if (GetCount("getCount", DateTime.Now.ToString("dd/MM/yyyy")) <= 10)
                                {
                                    //divBackToTableLsit.Visible = false;
                                    divPreviousAndNext.Visible = false;
                                }
                            }

                            if (sFirstPageNo != "")
                            {
                                ViewState["PreviousPageNo"] = sFirstPageNo.ToString().Trim();
                                ViewState["NextPageNo"] = sLastPageNo.ToString().Trim();
                            }
                            if (sFirstPageNo.ToString().Trim() == "1")
                            {
                                btnPrevious.Visible = false;
                            }
                            else
                            {
                                btnPrevious.Visible = true;
                            }
                            GvChangeBoatBooking.DataSource = dt;
                            GvChangeBoatBooking.DataBind();
                            GvChangeBoatBooking.Visible = true;
                            divBookingPin_Main.Visible = true;
                        }
                        else
                        {
                            divBookingPin_Main.Visible = false;
                            ViewState["PreviousPageNo"] = 1;
                            ViewState["NextPageNo"] = 10;
                            GvChangeBoatBooking.Visible = false;
                            if (Session["UserRole"].ToString() == "Sadmin")
                            {
                                if (GetCount("getCountSadmin", "") > 0)
                                {
                                    divBackToTableLsit.Visible = true;
                                }
                            }
                            else
                            {
                                if (GetCount("getCount", DateTime.Now.ToString("dd/MM/yyyy")) > 0)
                                {
                                    divBackToTableLsit.Visible = true;
                                }
                            }
                        }
                    }
                    else
                    {

                        divBookingPin_Main.Visible = false;
                        GvChangeBoatBooking.Visible = false;
                        divPreviousAndNext.Visible = false;
                        divBackToTableLsit.Visible = false;
                        if (Session["UserRole"].ToString() == "Sadmin")
                        {
                            if (GetCount("getCountSadmin", txtBookingDate.Text.Trim()) > 0)
                            {
                                divBackToTableLsit.Visible = true;
                            }
                        }
                        else
                        {
                            if (GetCount("getCount", DateTime.Now.ToString("dd/MM/yyyy")) > 0)
                            {
                                divBackToTableLsit.Visible = true;
                            }
                        }
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divGridList.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void BindLogDetails(string sQueryType, string sBookingPin_Bookingid, string sFirstPageNo, string sLastPageNo)
    {
        try
        {
            divLogDetails.Visible = true;
            lbtnGrid.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatSearch = new BoatSearch();

                BoatSearch = new BoatSearch()
                {
                    QueryType = sQueryType.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingPin = sBookingPin_Bookingid.ToString().Trim(),
                    BookingId = sBookingPin_Bookingid.ToString().Trim(),
                    FirstPageNo = sFirstPageNo.ToString().Trim(),
                    LastPageNo = sLastPageNo.ToString().Trim(),
                    Date = DateTime.Now.ToString("dd/MM/yyyy")
                };
                HttpResponseMessage response = client.PostAsJsonAsync("ChangeBoatBookingV2", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();
                    if (StatusCode == 0)
                    {
                        gvViewLogDetails.Visible = false;
                        divPreviousAndNextLog.Visible = false;
                        divBackToLog.Visible = false;
                        divBookingPin_Log.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        if (GetCount("ViewLogCount", DateTime.Now.ToString("dd/MM/yyyy")) > 0)
                        {
                            divBackToLog.Visible = true;
                        }
                        return;
                    }
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dt.Rows.Count > 0)
                    {
                        if (sFirstPageNo != "")
                        {
                            divBackToLog.Visible = false;
                            divPreviousAndNextLog.Visible = true;
                        }
                        else
                        {
                            divBackToLog.Visible = true;
                            divPreviousAndNextLog.Visible = false;
                        }
                        if (GetCount("ViewLogCount", DateTime.Now.ToString("dd/MM/yyyy")) <= 10)
                        {
                            divBackToLog.Visible = false;
                            divPreviousAndNextLog.Visible = false;
                        }
                        if (sFirstPageNo != "")
                        {
                            ViewState["PreviousPageNoLog"] = sFirstPageNo.ToString().Trim();
                            ViewState["NextPageNoLog"] = sLastPageNo.ToString().Trim();
                        }
                        if (sFirstPageNo.ToString().Trim() == "1")
                        {
                            btnPreviousLog.Visible = false;
                        }
                        else
                        {
                            btnPreviousLog.Visible = true;
                        }
                        gvViewLogDetails.DataSource = dt;
                        gvViewLogDetails.DataBind();
                        gvViewLogDetails.Visible = true;
                        divBookingPin_Log.Visible = true;
                    }
                    else
                    {
                        gvViewLogDetails.Visible = false;
                        divPreviousAndNextLog.Visible = false;
                        divBackToLog.Visible = false;
                        divBookingPin_Log.Visible = false;
                        if (GetCount("ViewLogCount", DateTime.Now.ToString("dd/MM/yyyy")) > 0)
                        {
                            divBackToLog.Visible = true;
                        }
                    }
                }
                else
                {
                    divBookingPin_Log.Visible = false;
                    divLogDetails.Visible = false;
                    lbtnGrid.Visible = false;
                    divPreviousAndNextLog.Visible = false;
                    divBackToLog.Visible = false;
                    if (GetCount("ViewLogCount", DateTime.Now.ToString("dd/MM/yyyy")) > 0)
                    {
                        divBackToLog.Visible = true;
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
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
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = ViewState["BookingId"].ToString(),
                    BookingPin = ViewState["BookingPin"].ToString()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("ViewPopLogDetails", BoatSearch).Result;

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
                        gvOldLogDet.Visible = false;
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
    public void BindPopNewLogDetails()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = ViewState["BookingId"].ToString(),
                    BookingPin = ViewState["BookingPin"].ToString()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("ViewPopLogDetails", BoatSearch).Result;

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
                        gvNewLogDet.Visible = false;
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
    public void GetRowerName()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var boatmaster = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlRowerName", boatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlrowername.DataSource = dt;
                            ddlrowername.DataValueField = "RowerId";
                            ddlrowername.DataTextField = "RowerName";
                            ddlrowername.DataBind();
                        }
                        else
                        {
                            ddlrowername.DataBind();
                        }
                        ddlrowername.Items.Insert(0, new ListItem("Select Rower Name", "0"));
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
    public void clearInputs()
    {
        txtendtime.Text = string.Empty;
        txtNewendtime.Text = string.Empty;
        txtstarttime.Text = string.Empty;
        txtNewstarttime.Text = string.Empty;
        ddlBoatNum.SelectedValue = "0";
        ddlrowername.SelectedValue = "0";
        txtOldBoatDuration.Text = string.Empty;
        txtOldTravelledDuration.Text = string.Empty;
        txtNewBoatDuration.Text = string.Empty;
        txtNewTravelDuration.Text = string.Empty;
        txtOldBoatDeposit.Text = string.Empty;
        txtOldDepositRefundAmt.Text = string.Empty;
        txtNewDepositRefundAmt.Text = string.Empty;
    }
    public void BindBoatDetailsSadmin(string sBookingPin)
    {
        try
        {
            divGridList.Visible = true;
            ViewState["BookingDate"] = txtBookingDate.Text.Trim();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = ViewState["BookingDate"].ToString(),
                    BookingPin = sBookingPin.ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ChangeBoatBookingSadminNew", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            GvChangeBoatBooking.DataSource = dt;
                            GvChangeBoatBooking.DataBind();
                            GvChangeBoatBooking.Visible = true;
                        }
                        else
                        {
                            GvChangeBoatBooking.Visible = false;
                        }
                    }
                    else
                    {
                        divGridList.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divGridList.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public int GetCount(string sQuertType, string sDate)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatSearch = new BoatSearch();
                if (Session["UserRole"].ToString() == "Sadmin")
                {
                    BoatSearch = new BoatSearch()
                    {
                        QueryType = sQuertType.ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingPin = "",
                        BookingId = "",
                        FirstPageNo = "",
                        LastPageNo = "",
                        Date = sDate.ToString().Trim()
                    };
                }
                else
                {
                    BoatSearch = new BoatSearch()
                    {
                        QueryType = sQuertType.ToString().Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BookingPin = "",
                        BookingId = "",
                        FirstPageNo = "",
                        LastPageNo = "",
                        Date = sDate.ToString().Trim()
                    };
                }
                HttpResponseMessage response = client.PostAsJsonAsync("ChangeBoatBookingV2", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            return Convert.ToInt16(dt.Rows[0]["Count"].ToString());
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
        return 0;
    }
    protected void txtBookingDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtBookingPin.Text = string.Empty;
            btnNext.Visible = true;
            BindBoatDetails("ChangeTripSheetNewPagingSadmin", txtBookingPin.Text.ToString().Trim(), "1", "10");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void txtBookingPin_TextChanged(object sender, EventArgs e)
    {
        if (txtBookingPin.Text.ToString().Trim() == "")
        {
            GvChangeBoatBooking.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Enter Booking Pin');", true);
            return;
        }
        if (Session["UserRole"].ToString() == "Sadmin")
        {
            BindBoatDetails("BookingPinBasedSadmin", txtBookingPin.Text.ToString().Trim(), "", "");
        }
        else
        {
            if (Session["BBMChangeTripSheet"].ToString().Trim() == "Y")
            {
                BindBoatDetails("BookingPinBased", txtBookingPin.Text.Trim(), "", "");
                GetTimeStamp();
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //BindLogDetails(DateTime.Now.ToString("dd-mm-yyyy"), DateTime.Now.ToString("dd-mm-yyyy"));
    }
    public class BoatSearch
    {
        public string BoatHouseId { get; set; }
        public string BookingId { get; set; }
        public string BookingPin { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string BoatReferenceNum { get; set; }
        public string QueryType { get; set; }
        public string BoatHouseName { get; set; }
        public string OldBoatNum { get; set; }
        public string OldExpectedTime { get; set; }
        public string NewBoatNum { get; set; }
        public string NewExpectedTime { get; set; }
        public string BoatStatus { get; set; }
        public string CreatedBy { get; set; }
        public string NewRowerId { get; set; }
        public string BoatNature { get; set; }
        public string NewTripStartTime { get; set; }
        public string NewTripEndTime { get; set; }
        public string OldBoatId { get; set; }
        public string NewBoatId { get; set; }
        public string OldRowerId { get; set; }
        public string OldTripStartTime { get; set; }
        public string OldTripEndTime { get; set; }
        public string OldDepRefundAmount { get; set; }
        public string NewDepRefundAmount { get; set; }
        public string BookingMedia { get; set; }
        public string BoatReferenceNo { get; set; }
        public string ActualBoatId { get; set; }
        public string RowerId { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string BookingDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FirstPageNo { get; set; }
        public string LastPageNo { get; set; }
        public string Date { get; set; }
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            int iFirstPage = 0;
            int iLastPage = 0;
            if (Session["UserRole"].ToString() == "Sadmin")
            {

                if ((Convert.ToInt16(ViewState["PreviousPageNo"].ToString().Trim()) > 10))
                {
                    iFirstPage = (Convert.ToInt16(ViewState["PreviousPageNo"].ToString().Trim())) - 10;
                    iLastPage = (Convert.ToInt16(ViewState["NextPageNo"].ToString().Trim())) - 10;
                }
                if (GetCount("getCountSadmin", txtBookingDate.Text.Trim()) < iLastPage)
                {
                    btnNext.Visible = false;
                }
                else
                {
                    btnNext.Visible = true;
                }

                BindBoatDetails
                    (
                    "ChangeTripSheetNewPagingSadmin",
                    "",
                    iFirstPage.ToString().Trim(),
                    iLastPage.ToString().Trim()
                    );
            }
            else
            {
                if ((Convert.ToInt16(ViewState["PreviousPageNo"].ToString().Trim()) > 10))
                {
                    iFirstPage = (Convert.ToInt16(ViewState["PreviousPageNo"].ToString().Trim())) - 10;
                    iLastPage = (Convert.ToInt16(ViewState["NextPageNo"].ToString().Trim())) - 10;
                }
                if (GetCount("getCount", DateTime.Now.ToString("dd/MM/yyyy")) < iLastPage)
                {
                    btnNext.Visible = false;
                }
                else
                {
                    btnNext.Visible = true;
                }

                BindBoatDetails
                    (
                    "ChangeTripSheetNewPaging",
                    "",
                    iFirstPage.ToString().Trim(),
                    iLastPage.ToString().Trim()
                    );
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserRole"].ToString() == "Sadmin")
            {
                if (GetCount("getCountSadmin", txtBookingDate.Text.Trim()) <= (Convert.ToInt16(ViewState["NextPageNo"].ToString().Trim()) + 10))
                {
                    btnNext.Visible = false;
                }
                else
                {
                    btnNext.Visible = true;
                }
                int iFirstPage = 10 + (Convert.ToInt16(ViewState["PreviousPageNo"].ToString().Trim()));
                int iLastPage = 10 + (Convert.ToInt16(ViewState["NextPageNo"].ToString().Trim()));
                BindBoatDetails
                    (
                    "ChangeTripSheetNewPagingSadmin",
                    "",
                    iFirstPage.ToString().Trim(),
                    iLastPage.ToString().Trim()
                    );
            }
            else
            {
                if (GetCount("getCount", DateTime.Now.ToString("dd/MM/yyyy")) <= (Convert.ToInt16(ViewState["NextPageNo"].ToString().Trim()) + 10))
                {
                    btnNext.Visible = false;
                }
                else
                {
                    btnNext.Visible = true;
                }
                int iFirstPage = 10 + (Convert.ToInt16(ViewState["PreviousPageNo"].ToString().Trim()));
                int iLastPage = 10 + (Convert.ToInt16(ViewState["NextPageNo"].ToString().Trim()));
                BindBoatDetails
                    (
                    "ChangeTripSheetNewPaging",
                    "",
                    iFirstPage.ToString().Trim(),
                    iLastPage.ToString().Trim()
                    );
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void btnBackToTableList_Click(object sender, EventArgs e)
    {
        BackToTableList();
    }
    public void BackToTableList()
    {
        try
        {
            if (Session["UserRole"].ToString() == "Sadmin")
            {
                txtBookingPin.Text = string.Empty;
                divPreviousAndNext.Visible = true;
                divBackToTableLsit.Visible = false;
                int iFirstPage = (Convert.ToInt16(ViewState["PreviousPageNo"].ToString().Trim()));
                int iLastPage = (Convert.ToInt16(ViewState["NextPageNo"].ToString().Trim()));
                BindBoatDetails
                (
                "ChangeTripSheetNewPagingSadmin",
                "",
                iFirstPage.ToString().Trim(),
                iLastPage.ToString().Trim()
                );
            }
            else
            {
                txtBookingPin.Text = string.Empty;
                divPreviousAndNext.Visible = true;
                divBackToTableLsit.Visible = false;
                int iFirstPage = (Convert.ToInt16(ViewState["PreviousPageNo"].ToString().Trim()));
                int iLastPage = (Convert.ToInt16(ViewState["NextPageNo"].ToString().Trim()));
                BindBoatDetails
                (
                "ChangeTripSheetNewPaging",
                "",
                iFirstPage.ToString().Trim(),
                iLastPage.ToString().Trim()
                );
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void BackToTableListLog()
    {
        try
        {
            txtBookingPinOrIdLog.Text = string.Empty;
            divPreviousAndNextLog.Visible = true;
            divBackToLog.Visible = false;
            int iFirstPage = (Convert.ToInt16(ViewState["PreviousPageNoLog"].ToString().Trim()));
            int iLastPage = (Convert.ToInt16(ViewState["NextPageNoLog"].ToString().Trim()));
            BindLogDetails
            (
            "ViewLogDetails",
            "",
            iFirstPage.ToString().Trim(),
            iLastPage.ToString().Trim()
            );
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void GvChangeBoatBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvChangeBoatBooking.PageIndex = e.NewPageIndex;
        if (Session["UserRole"].ToString() == "Sadmin")
        {
            BindBoatDetails("BookingPinBasedSadmin", txtBookingPin.Text.Trim(), "", "");
        }
        else
        {
            BindBoatDetails("BookingPinBased", txtBookingPin.Text.Trim(), "", "");
        }
    }
    protected void btnBackToLog_Click(object sender, EventArgs e)
    {
        BackToTableListLog();
    }
    protected void btnPreviousLog_Click(object sender, EventArgs e)
    {
        int iFirstPage = 0;
        int iLastPage = 0;
        if ((Convert.ToInt16(ViewState["PreviousPageNoLog"].ToString().Trim()) > 10))
        {
            iFirstPage = (Convert.ToInt16(ViewState["PreviousPageNoLog"].ToString().Trim())) - 10;
            iLastPage = (Convert.ToInt16(ViewState["NextPageNoLog"].ToString().Trim())) - 10;
        }
        if (GetCount("ViewLogCount", DateTime.Now.ToString("dd/MM/yyyy")) <= iLastPage)
        {
            btnNextLog.Visible = false;
        }
        else
        {
            btnNextLog.Visible = true;
        }

        BindLogDetails
            (
            "ViewLogDetails",
            "",
            iFirstPage.ToString().Trim(),
            iLastPage.ToString().Trim()
            );
    }
    protected void btnNextLog_Click(object sender, EventArgs e)
    {
        try
        {
            if (GetCount("ViewLogCount", DateTime.Now.ToString("dd/MM/yyyy")) <= (Convert.ToInt16(ViewState["NextPageNoLog"].ToString().Trim()) + 10))
            {
                btnNextLog.Visible = false;
            }
            else
            {
                btnNextLog.Visible = true;
            }
            int iFirstPage = 10 + (Convert.ToInt16(ViewState["PreviousPageNoLog"].ToString().Trim()));
            int iLastPage = 10 + (Convert.ToInt16(ViewState["NextPageNoLog"].ToString().Trim()));
            BindLogDetails
                (
                "ViewLogDetails",
                "",
                iFirstPage.ToString().Trim(),
                iLastPage.ToString().Trim()
                );
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void txtBookingPinOrIdLog_TextChanged(object sender, EventArgs e)
    {
        try
        {
            BindLogDetails("BookinPinBasedFilterLog", txtBookingPinOrIdLog.Text.Trim(), "", "");
            divBackToLog.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
}
