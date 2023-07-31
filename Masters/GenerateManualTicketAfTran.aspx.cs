using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Helpers;

public partial class GenerateManualTicketAfTran : System.Web.UI.Page
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
				divCategoryother.Visible = false;
				divDate.Visible = false;
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if(ddlCategory.SelectedIndex == 0)
        {
            ddlCategory.Focus();
        }
        if (txtCategory.Text == "")
        {
            txtCategory.Focus();
        }

        getAfterTransaction();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlCategory.SelectedIndex = 0;
        txtCategory.Text = "";
        divCategoryother.Visible = false;
        divDate.Visible = false;
        divGridList.Visible = false;
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedIndex == 1)
        {
            lblCategory.InnerText = "Enter Transaction No";
        }
        else if (ddlCategory.SelectedIndex == 2)
        {
            lblCategory.InnerText = "Enter Amount ";
        }
        else if (ddlCategory.SelectedIndex == 3)
        {
            lblCategory.InnerText = "Enter Order Id";
        }
        else if (ddlCategory.SelectedIndex == 4)
        {
            lblCategory.InnerText = "Enter Tracking Id";
        }
        else if (ddlCategory.SelectedIndex == 5)
        {
            lblCategory.InnerText = "Enter Bank Reference No";
        }
        else if (ddlCategory.SelectedIndex == 6)
        {
            lblCategory.InnerText = "Enter Order Status";
        }
        else if (ddlCategory.SelectedIndex == 7)
        {
            lblCategory.InnerText = "Enter User Id";
        }

        else if (ddlCategory.SelectedIndex == 8)
        {
            lblCategory.InnerText = "Enter Mobile No";
        }
        else if (ddlCategory.SelectedIndex == 9)
        {
            lblCategory.InnerText = "Enter Email Id";

        }
        else //if (ddlCategory.SelectedIndex == 10)
        {
            lblCategory.InnerText = "Enter Boat House Name";

        }
        if(ddlCategory.SelectedIndex == 11)
        {
            txtCategory.Text = "";
            txtDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
            txtDate.Attributes.Add("readonly", "readonly");
            divDate.Visible = true;
            divCategoryother.Visible = false;
            divGridList.Visible = false;
        }
        else
        {
            txtCategory.Text = "";
            divCategoryother.Visible = true;
            divDate.Visible = false;
            divGridList.Visible = false;
        }
    }

    protected void ImgBtnPrint_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

        string BankReferenceNo = GvBoatBooking.DataKeys[gvrow.RowIndex]["BankReferenceNo"].ToString().Trim();
        string BookingMedia = GvBoatBooking.DataKeys[gvrow.RowIndex]["BookingMedia"].ToString().Trim();
        string BookingType = GvBoatBooking.DataKeys[gvrow.RowIndex]["BookingType"].ToString().Trim();
        string EmailId = GvBoatBooking.DataKeys[gvrow.RowIndex]["EmailId"].ToString().Trim();
        string MobileNo = GvBoatBooking.DataKeys[gvrow.RowIndex]["MobileNo"].ToString().Trim();
        string ModuleType = GvBoatBooking.DataKeys[gvrow.RowIndex]["ModuleType"].ToString().Trim();
        string OrderId = GvBoatBooking.DataKeys[gvrow.RowIndex]["OrderId"].ToString().Trim();
        string OrderStatus = GvBoatBooking.DataKeys[gvrow.RowIndex]["OrderStatus"].ToString().Trim();
        string TrackingId = GvBoatBooking.DataKeys[gvrow.RowIndex]["TrackingId"].ToString().Trim();
        string TransactionNo = GvBoatBooking.DataKeys[gvrow.RowIndex]["TransactionNo"].ToString().Trim();
        string UserId = GvBoatBooking.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();

        Boolean available = FindTicketStatus(TransactionNo);

        if (available == true)
        {
            PrintTicketStatus(BankReferenceNo, BookingMedia, BookingType, EmailId,
            MobileNo, ModuleType, OrderId, OrderStatus, TrackingId, TransactionNo, UserId);
        }
        else
        {
            return;
        }

    }

    public void getAfterTransaction()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var GenrateTicketaftn = new GenerateTicket()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    QueryType = ddlCategory.SelectedValue,
                    ServiceType = txtCategory.Text,
                    BookingDate = txtDate.Text.Trim()
                };


                HttpResponseMessage response = client.PostAsJsonAsync("CM_GenerateManualTicket", GenrateTicketaftn).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        divGridList.Visible = true;
                        GvBoatBooking.DataSource = dtExists;
                        GvBoatBooking.DataBind();

                    }
                    else
                    {
                        divGridList.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' No Records Found...!! ');", true);
                    }

                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    private Boolean FindTicketStatus(string TransactionNo)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var GenrateTicketaftn = new GenerateTicket()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    QueryType = "FindTicketStatus",
                    ServiceType = TransactionNo,
                    BookingDate = txtDate.Text.Trim()
                };


                HttpResponseMessage response = client.PostAsJsonAsync("CM_GenerateManualTicket", GenrateTicketaftn).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Ticket Already Issued...!! ');", true);
                        return false;
                    }
                    else
                    {
                        return true;

                    }

                }
                else
                {
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return false;
        }
    }

    private void PrintTicketStatus(string BankReferenceNo, string BookingMedia, string BookingType, string EmailId,
        string MobileNo, string ModuleType, string OrderId, string OrderStatus, string TrackingId, string TransactionNo, string UserId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var GenrateTicketaftn = new GenerateTicket()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString(),
                    BankReferenceNo = BankReferenceNo.Trim(),
                    BookingMedia = BookingMedia.Trim(),
                    BookingType = BookingType.Trim(),
                    EmailId = EmailId.Trim(),
                    MobileNo = MobileNo.Trim(),
                    ModuleType = ModuleType.Trim(),
                    OrderId = OrderId.Trim(),
                    OrderStatus = OrderStatus.Trim(),
                    TrackingId = TrackingId.Trim(),
                    TransactionNo = TransactionNo.Trim(),
                    UserId = UserId​​.Trim()

                };


                HttpResponseMessage response = client.PostAsJsonAsync("SuccessOnlineBoatBookingAftrTran", GenrateTicketaftn).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Postresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Postresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Postresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Ticket SMS has sent Successfuly...!! ');", true);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert(' Ticket Already Issued...!! ');", true);
                        return;

                    }
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public class GenerateTicket
    {
        public string BoatHouseId { get; set; }
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string BookingDate { get; set; }
        public string BankReferenceNo { get; set; }
        public string BookingMedia { get; set; }
        public string BookingType { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string ModuleType { get; set; }
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string TrackingId { get; set; }
        public string TransactionNo { get; set; }
        public string UserId { get; set; }

    }
}


