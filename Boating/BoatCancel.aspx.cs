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

public partial class BoatCancel : System.Web.UI.Page
{
    //private object gvddlTripStartTime;
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

                BindCancelBookingDetails();
                txtBookingId.Enabled = true;
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }


    protected void txtBookingId_TextChanged(object sender, EventArgs e)
    {
        if (txtBookingId.Text == " ")
        {
            exittimedetails.Visible = false;
            BookingDetailsBasedOnBookingId();
            BookingNoteDetails();
        }
        else
        {
            exittimedetails.Visible = false;
            BookingDetailsBasedOnBookingId();
            BookingNoteDetails();
        }
    }

    public void BookingNoteDetails()
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

                var Cancellation = new BoatCancellaion()
                {
                    QueryType = "BoatCancelDetails",
                    ServiceType = "BookCancelTripList",
                    BookingId = txtBookingId.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString()
                };

                response = client.PostAsJsonAsync("CommonOperation", Cancellation).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    if (vehicleEditresponse.Contains("No Records Found."))
                    {
                        gvNoteDetails.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + vehicleEditresponse + "');", true);

                    }
                    else
                    {
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            gvNoteDetails.DataSource = dtExists;
                            gvNoteDetails.DataBind();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            txtBookingId.Focus();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindCancelBookingDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new BoatCancellaion()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("CancellationDetails", BoatHouseId).Result;

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
                            gvCancelBooking.DataSource = dt;
                            gvCancelBooking.DataBind();

                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            gvCancelBooking.DataBind();

                            divGrid.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;
                        }
                    }
                    else
                    {
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;

                    }
                }
                else
                {
                    return;
                    //lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    //Single Cancellation

    public void BookingDetailsBasedOnBookingId()
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

                var Cancellation = new BoatCancellaion()
                {
                    QueryType = "BoatCancelDetails",
                    ServiceType = "BookCancelList",
                    BookingId = txtBookingId.Text.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString()
                };

                response = client.PostAsJsonAsync("CommonOperation", Cancellation).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    if (vehicleEditresponse.Contains("No Records Found."))
                    {
                        gvBookingDetails.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + vehicleEditresponse + "');", true);

                    }
                    else
                    {
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                        DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dtExists.Rows.Count > 0)
                        {
                            gvBookingDetails.DataSource = dtExists;
                            gvBookingDetails.DataBind();
                            exittimedetails.Visible = true;

                            //Calculate Sum and display in Footer Row
                            //double TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDouble(row.Field<double>("InitNetAmount")));
                            //double BoatDeposit = dtExists.AsEnumerable().Sum(row => Convert.ToDouble(row.Field<double>("BoatDeposit")));

                            //gvBookingDetails.FooterRow.Cells[8].Text = "Total";
                            //gvBookingDetails.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            //gvBookingDetails.FooterRow.Cells[9].Text = TotalAmount.ToString("N2");
                            //gvBookingDetails.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                            //gvBookingDetails.FooterRow.Cells[10].Text = BoatDeposit.ToString("N2");
                            //gvBookingDetails.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;


                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            txtBookingId.Focus();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void gvBookingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBookingDetails.PageIndex = e.NewPageIndex;
        BookingDetailsBasedOnBookingId();
    }

    protected void gvCancelBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCancelBooking.PageIndex = e.NewPageIndex;
        BindCancelBookingDetails();
    }

    protected void ImgBtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sBookingId = gvNoteDetails.DataKeys[gvrow.RowIndex]["BookingId"].ToString().Trim();
            string sBookingPin = gvNoteDetails.DataKeys[gvrow.RowIndex]["BookingPin"].ToString().Trim();
            string sBoatReferenceNo = gvNoteDetails.DataKeys[gvrow.RowIndex]["BoatReferenceNo"].ToString().Trim();
            string sBoatTypeId = gvNoteDetails.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim();
            string sBoatSeaterId = gvNoteDetails.DataKeys[gvrow.RowIndex]["BoatSeaterId"].ToString().Trim();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Cancellation = new BoatCancellaion()
                {
                    QueryType = "BoatBookingCancel",
                    BookingId = sBookingId.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BookingPin = sBookingPin.Trim(),
                    BoatReferenceNo = sBoatReferenceNo.Trim(),
                    BoatTypeId = sBoatTypeId.Trim(),
                    BoatSeaterId = sBoatSeaterId.Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("BookingCancel", Cancellation).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        txtBookingId.Text = sBookingId.Trim();
                        BookingDetailsBasedOnBookingId();
                        BookingNoteDetails();

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Clear Inputs and Cancel Process
    public void clearInputs()
    {
        txtBookingId.Text = string.Empty;
        exittimedetails.Visible = false;
        txtBookingId.Enabled = true;
        divBookingId.Visible = true;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearInputs();
        BindCancelBookingDetails();
    }

    //Add New

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
        clearInputs();
    }


    public class BoatCancellaion
    {
        public string QueryType { get; set; }
        public string BookingId { get; set; }
        public string BoatHouseId { get; set; }
        public string BookingPin { get; set; }
        public string BoatReferenceNo { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }


        public string ServiceType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }

}