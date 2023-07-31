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

public partial class ReTripEntryDtl : System.Web.UI.Page
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
                hfBoatHouseId.Value = Session["BoatHouseId"].ToString().Trim();
                hfBoatHouseName.Value = Session["BoatHouseName"].ToString().Trim();
                BindReEnrtyFinal();
                tripdtls.Visible = false;
                hfCreatedBy.Value = Session["UserId"].ToString().Trim();
                txtstartTime.Text = DateTime.Now.ToString("HH:mm:ss");
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindReEnrtyBookingIdPiN()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var ReEnrty = new ReEnrty()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = txtBookingId.Text.Trim(),
                    BookingPin = ViewState["BookingPin"].ToString()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("REEntrpTripdtls/BookingId/BookingPin", ReEnrty).Result;

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
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string BoatId = BoatId = dt.Rows[i]["BoatId"].ToString();
                                ViewState["BoatId"] = BoatId;
                                string BoatNumber = dt.Rows[i]["BoatNumber"].ToString();
                                lblBoatNumber.Text = BoatNumber;
                                string RowerId = dt.Rows[i]["RowerId"].ToString();
                                ViewState["RowerId"] = RowerId;
                                string Rower = dt.Rows[i]["Rower"].ToString();
                                txtRower.Text = Rower;
                                string BoatTypeId = dt.Rows[i]["BoatTypeId"].ToString();
                                ViewState["BoatTypeId"] = BoatTypeId;
                                string BoatType = dt.Rows[i]["BoatType"].ToString();
                                txtBoatType.Text = BoatType;
                                string BoatSeaterId = dt.Rows[i]["BoatSeaterId"].ToString();
                                ViewState["BoatSeaterId"] = BoatSeaterId;
                                string BoatSeat = dt.Rows[i]["BoatSeat"].ToString();
                                txtBoatSeater.Text = BoatSeat;
                                string BookedDate = dt.Rows[i]["BookedDate"].ToString();
                                txtBookedDate.Text = BookedDate;
                                string TripStartTime = dt.Rows[i]["TripStartTime"].ToString();
                                txtstartTime.Text = TripStartTime;
                                string TripEndTime = dt.Rows[i]["TripEndTime"].ToString();
                                txtEndTime.Text = TripEndTime;
                                string ReTripStartTime = dt.Rows[i]["ReTripStartTime"].ToString();
                                txtReStartTime.Text = ReTripStartTime;
                                string ReTripEndTime = dt.Rows[i]["ReTripEndTime"].ToString();
                                txtReEndTime.Text = ReTripEndTime;
                                string BookingPin = dt.Rows[i]["BookingPin"].ToString();
                                lblboatpin.Text = BookingPin;
                                //btntripdtl.Visible = false;
                                tripdtls.Visible = true;
                            }

                        }

                    }
                    else
                    {


                        var Errorresponse = response.Content.ReadAsStringAsync().Result;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Trip Will Not be Started and Ended');", true);

                        divEntry.Visible = true;

                        tripdtls.Visible = false;
                    }


                }
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void ValidateBookingId()
    {
        try
        {

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ReEnrty = new ReEnrty()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = txtBookingId.Text

                };
                HttpResponseMessage response = client.PostAsJsonAsync("REEntrpTripdtls/GetBookingId", ReEnrty).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatHouseresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatHouseresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatHouseresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DivAddRentrydtl.Visible = true;
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
                        }
                    }
                    else
                    {
                        txtBookingId.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.Trim() + "');", true);
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

    protected void txtBookingId_TextChanged(object sender, EventArgs e)
    {
        ValidateBookingId();

        try
        {


            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ReEnrty = new ReEnrty()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingId = txtBookingId.Text.Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("REEntrpTripdtls/BookingId", ReEnrty).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatHouseresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatHouseresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatHouseresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DivAddRentrydtl.Visible = true;
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvRentrydtlBookingId.DataSource = dt;
                            gvRentrydtlBookingId.DataBind();
                            tripdtls.Visible = false;


                        }
                        else
                        {
                            gvRentrydtlBookingId.DataBind();
                            tripdtls.Visible = false;
                        }

                    }
                    else
                    {
                        tripdtls.Visible = false;
                        DivAddRentrydtl.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Today No Boat Booking');", true);

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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        tripdtls.Visible = true;
        lbtnNew.Visible = false;
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
                var retrip = new ReEnrty()
                {
                    Querytype = "Insert",
                    BookingId = txtBookingId.Text.Trim(),
                    BookingPin = lblboatpin.Text.Trim(),
                    BoatTypeId = ViewState["BoatTypeId"].ToString(),
                    BoatType = txtBoatType.Text.Trim(),
                    BoatSeaterId = ViewState["BoatSeaterId"].ToString(),
                    BoatSeat = txtBoatSeater.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BoatId = ViewState["BoatId"].ToString(),
                    BoatNumber = lblBoatNumber.Text.Trim(),
                    TripStartTime = txtstartTime.Text.Trim(),
                    TripEndTime = txtEndTime.Text.Trim(),
                    ReTripStartTime = txtReStartTime.Text.Trim(),
                    ReTripEndTime = txtReEndTime.Text.Trim(),
                    BookedDate = txtBookedDate.Text.Trim(),
                    RowerId = ViewState["RowerId"].ToString(),
                    Rower = txtRower.Text.Trim(),
                    Reason = txtreason.Text.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };
                response = client.PostAsJsonAsync("REEntrpTripdtls/Insert", retrip).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        clearInputs();
                        tripdtls.Visible = false;
                        BindReEnrtyFinal();
                        lbtnNew.Visible = true;

                        divGridReenrty.Visible = true;
                        divEntry.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        divEntry.Visible = true;
                        tripdtls.Visible = false;
                        DivAddRentrydtl.Visible = false;
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
        tripdtls.Visible = false;
        clearInputs();
        BindReEnrtyFinal();
        DivAddRentrydtl.Visible = false;


    }

    public void clearInputs()
    {
        txtBookingId.Text = string.Empty;
        lblboatpin.Text = string.Empty;
        lblBoatNumber.Text = string.Empty;
        txtRower.Text = string.Empty;
        txtBoatType.Text = string.Empty;
        txtBoatSeater.Text = string.Empty;
        txtBookedDate.Text = string.Empty;
        txtstartTime.Text = string.Empty;
        txtEndTime.Text = string.Empty;
        txtReStartTime.Text = string.Empty;
        txtReEndTime.Text = string.Empty;
        txtreason.Text = string.Empty;
    }
    public void BindReEnrtyFinal()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var ReEnrty = new ReEnrty()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BindReEnrty", ReEnrty).Result;

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
                            gvReEnrty.DataSource = dt;
                            gvReEnrty.DataBind();
                            lblGridMsg.Visible = false;
                            lbtnNew.Visible = true;
                            divGridReenrty.Visible = true;
                            divEntry.Visible = false;
                        }
                        else
                        {
                            gvReEnrty.DataBind();

                        }

                    }
                    else
                    {
                        lbtnNew.Visible = false;
                        divEntry.Visible = true;
                        divGridReenrty.Visible = false;
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

        divGridReenrty.Visible = false;
        lbtnNew.Visible = false;
        divEntry.Visible = true;
        DivAddRentrydtl.Visible = false;
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvReEnrty.DataKeys[gvrow.RowIndex].Value.ToString();
            Label BookingId = (Label)gvrow.FindControl("lblBookingId");
            Label BookingPin = (Label)gvrow.FindControl("lblBookingPin");
            Label BoatTypeId = (Label)gvrow.FindControl("BoatTypeId");
            Label BoatType = (Label)gvrow.FindControl("lblBoatType");
            Label BoatSeaterId = (Label)gvrow.FindControl("BoatSeaterId");
            Label SeaterType = (Label)gvrow.FindControl("lblSeaterType");
            Label BookedDate = (Label)gvrow.FindControl("lblBookedDate");
            Label BoatId = (Label)gvrow.FindControl("BoatId");
            Label BoatNumber = (Label)gvrow.FindControl("lblBoatNumber");
            Label TripStartTime = (Label)gvrow.FindControl("lblTripStartTime");
            Label TripEndTime = (Label)gvrow.FindControl("lblTripEndTime");
            Label ReTripStartTime = (Label)gvrow.FindControl("lblReTripStartTime");
            Label ReTripEndTime = (Label)gvrow.FindControl("lblReTripEndTime");
            Label RowerId = (Label)gvrow.FindControl("RowerId");
            Label RowerName = (Label)gvrow.FindControl("lblRowerName");
            Label Reason = (Label)gvrow.FindControl("lblReason");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Boatseatmaster = new ReEnrty()
                {
                    Querytype = "Delete",
                    BookingId = BookingId.Text,
                    BookingPin = BookingPin.Text,
                    BoatTypeId = BoatTypeId.Text,
                    BoatType = BoatType.Text.Trim(),
                    BoatSeaterId = BoatSeaterId.Text,
                    BoatSeat = SeaterType.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BoatId = BoatId.Text,
                    BoatNumber = lblBoatNumber.Text.Trim(),
                    TripStartTime = TripStartTime.Text.Trim(),
                    TripEndTime = TripEndTime.Text.Trim(),
                    ReTripStartTime = txtReStartTime.Text.Trim(),
                    ReTripEndTime = txtReEndTime.Text.Trim(),
                    BookedDate = BookedDate.Text.Trim(),
                    RowerId = RowerId.Text,
                    Rower = RowerName.Text.Trim(),
                    Reason = Reason.Text.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("REEntrpTripdtls/Insert", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindReEnrtyFinal();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        BindReEnrtyFinal();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindReEnrtyFinal();
                    clearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
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
            string sTesfg = gvReEnrty.DataKeys[gvrow.RowIndex].Value.ToString();
            Label BookingId = (Label)gvrow.FindControl("lblBookingId");
            Label BookingPin = (Label)gvrow.FindControl("lblBookingPin");
            Label BoatTypeId = (Label)gvrow.FindControl("BoatTypeId");
            Label BoatType = (Label)gvrow.FindControl("lblBoatType");
            Label BoatSeaterId = (Label)gvrow.FindControl("BoatSeaterId");
            Label SeaterType = (Label)gvrow.FindControl("lblSeaterType");
            Label BookedDate = (Label)gvrow.FindControl("lblBookedDate");
            Label BoatId = (Label)gvrow.FindControl("BoatId");
            Label BoatNumber = (Label)gvrow.FindControl("lblBoatNumber");
            Label TripStartTime = (Label)gvrow.FindControl("lblTripStartTime");
            Label TripEndTime = (Label)gvrow.FindControl("lblTripEndTime");
            Label ReTripStartTime = (Label)gvrow.FindControl("lblReTripStartTime");
            Label ReTripEndTime = (Label)gvrow.FindControl("lblReTripEndTime");
            Label RowerId = (Label)gvrow.FindControl("RowerId");
            Label RowerName = (Label)gvrow.FindControl("lblRowerName");
            Label Reason = (Label)gvrow.FindControl("lblReason");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Boatseatmaster = new ReEnrty()
                {
                    Querytype = "ReActive",
                    BookingId = BookingId.Text,
                    BookingPin = BookingPin.Text,
                    BoatTypeId = BoatTypeId.Text,
                    BoatType = BoatType.Text.Trim(),
                    BoatSeaterId = BoatSeaterId.Text,
                    BoatSeat = SeaterType.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    BoatId = BoatId.Text,
                    BoatNumber = lblBoatNumber.Text.Trim(),
                    TripStartTime = TripStartTime.Text.Trim(),
                    TripEndTime = TripEndTime.Text.Trim(),
                    ReTripStartTime = txtReStartTime.Text.Trim(),
                    ReTripEndTime = txtReEndTime.Text.Trim(),
                    BookedDate = BookedDate.Text.Trim(),
                    RowerId = RowerId.Text,
                    Rower = RowerName.Text.Trim(),
                    Reason = Reason.Text.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("REEntrpTripdtls/Insert", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindReEnrtyFinal();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        BindReEnrtyFinal();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindReEnrtyFinal();
                    clearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvReEnrty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // for example, the row contains a certain property
                if (((Label)e.Row.FindControl("lblActiveStatus")).Text == "D")
                {
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = false;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = true;
                }

                else
                {

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



    protected void lblBookingPin_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string BookingPin = gvRentrydtlBookingId.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["BookingPin"] = BookingPin;
            BindReEnrtyBookingIdPiN();
            //tripdtls.Visible = true;
            lbtnNew.Visible = false;
            //}
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    protected void txtEndTime_TextChanged(object sender, EventArgs e)
    {

        if (txtstartTime.Text == "")
        {
            txtstartTime.Text = string.Empty;
            txtEndTime.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Trip Start time');", true);
        }
        else
        {
            DateTime startTime = DateTime.Parse(txtstartTime.Text);
            DateTime endTime = DateTime.Parse(txtEndTime.Text);

            if (startTime >= endTime)
            {
                txtEndTime.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Trip Start Time should be Greater than Trip End Time !');", true);
            }
        }

    }

    protected void txtstartTime_TextChanged(object sender, EventArgs e)
    {
        DateTime startTime = DateTime.Parse(txtstartTime.Text);

    }


    protected void txtReStartTime_TextChanged(object sender, EventArgs e)
    {
        DateTime startTime = DateTime.Parse(txtReStartTime.Text);
    }

    protected void txtReEndTime_TextChanged(object sender, EventArgs e)
    {

        if (txtReStartTime.Text == "")
        {
            txtReStartTime.Text = string.Empty;
            txtReEndTime.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Trip Start time');", true);
        }
        else
        {
            DateTime startTime = DateTime.Parse(txtReStartTime.Text);
            DateTime endTime = DateTime.Parse(txtReEndTime.Text);

            if (startTime >= endTime)
            {
                txtReEndTime.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Trip Start Time should be Greater than Trip End Time !');", true);
            }
        }
    }
    public class ReEnrty
    {
        public string BookingId { get; set; }
        public string BookingPin { get; set; }
        public string BoatType { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeat { get; set; }
        public string BoatSeaterId { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BoatNumber { get; set; }
        public string BoatId { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string ReTripStartTime { get; set; }
        public string ReTripEndTime { get; set; }
        public string BookedDate { get; set; }
        public string Rower { get; set; }
        public string RowerId { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string Querytype { get; set; }
        public string BoatReferenceNo { get; set; }

        public string Reason { get; set; }

    }
}