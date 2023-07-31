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

public partial class Enquiry : System.Web.UI.Page
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
				hfCreatedBy.Value = Session["UserId"].ToString().Trim();
				BindEnquiry();
				GetEnquiry();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}
    public class enquiry
    {
        public string QueryType { get; set; }
        public string EnquiryId { get; set; }
        public string EnquiryType { get; set; }
        public string EnquiredBy { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string MailId { get; set; }
        public string QueryDetails { get; set; }
        public string ResponseDetails { get; set; }
        public string ResponseBy { get; set; }
        public string ResponseDate { get; set; }
    }

    public async void GetEnquiry()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ConfigMstr/DDLEnquiry");

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

                            ddlEnquiryType.DataSource = dt;
                            ddlEnquiryType.DataValueField = "ConfigId";
                            ddlEnquiryType.DataTextField = "ConfigName";
                            ddlEnquiryType.DataBind();

                        }
                        else
                        {

                            ddlEnquiryType.DataBind();
                        }
                        ddlEnquiryType.Items.Insert(0, new ListItem("Select Enquiry", "0"));
                    }
                    else
                    {

                        lblGridMsg.Text = ResponseMsg.ToString().Trim();

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

    public async void BindEnquiry()
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

                response = await client.GetAsync("Enquiry/ListAll");
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
                            gvEnquiry.DataSource = dt;
                            gvEnquiry.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvEnquiry.DataBind();
                            gvEnquiry.DataSource = dt;
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        divEntry.Visible = true;
                        divGrid.Visible = false;
                        lbtnNew.Visible = false;
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
    protected async void btnSubmit_Click(object sender, EventArgs e)
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
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var enquiry = new enquiry()
                    {
                        QueryType = "Insert",
                        EnquiryId = "0",
                        EnquiryType = ddlEnquiryType.SelectedValue.Trim(),
                        EnquiredBy = txtEnquiryBy.Text.Trim(),
                        Address = txtAddr.Text.Trim(),
                        MobileNo = txtMobno.Text.Trim(),
                        MailId = txtEmailid.Text.Trim(),
                        QueryDetails = txtQuerydetails.Text.Trim(),
                        ResponseDetails = txtresponseDetails.Text.Trim(),
                        ResponseBy = hfCreatedBy.Value.Trim()


                    };

                    response = await client.PostAsJsonAsync("Enquiry", enquiry);
                  

                }
                else
                {
                    var enquiry = new enquiry()
                    {

                        QueryType = "Update",
                        EnquiryId = txtEnquiryid.Text.Trim(),
                        EnquiryType = ddlEnquiryType.SelectedValue.Trim(),
                        EnquiredBy = txtEnquiryBy.Text.Trim(),
                        Address = txtAddr.Text.Trim(),
                        MobileNo = txtMobno.Text.Trim(),
                        MailId = txtEmailid.Text.Trim(),
                        QueryDetails = txtQuerydetails.Text.Trim(),
                        ResponseDetails = txtresponseDetails.Text.Trim(),
                        ResponseBy = hfCreatedBy.Value.Trim()


                    };

                    response = await client.PostAsJsonAsync("Enquiry", enquiry);
                  

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {

                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            BindEnquiry();
                            clearinputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            BindEnquiry();
                            clearinputs();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearinputs();
        BindEnquiry();
        divEntry.Visible = false;
        divGrid.Visible = true;
        lbtnNew.Visible = true;
        btnSubmit.Text = "Submit";
    }

    public void clearinputs()
    {
        txtAddr.Text = string.Empty;
        ddlEnquiryType.SelectedIndex = 0;
        txtEnquiryBy.Text = string.Empty;
        txtMobno.Text = string.Empty;
        txtEmailid.Text = string.Empty;
        txtQuerydetails.Text = string.Empty;
        txtresponseDetails.Text = string.Empty;
    }
    
    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        BindEnquiry();
        clearinputs();
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvEnquiry.DataKeys[gvrow.RowIndex].Value.ToString();
            Label EnquiryId = (Label)gvrow.FindControl("lblEnquiryId");
            Label EnquiryType = (Label)gvrow.FindControl("lblEnquiryType");
            Label Address = (Label)gvrow.FindControl("lblAddress");
            Label EnquiredBy = (Label)gvrow.FindControl("lblEnquiredBy");
            Label MobileNo = (Label)gvrow.FindControl("lblMobileNo");
            Label MailId = (Label)gvrow.FindControl("lblMailId");
            Label QueryDetails = (Label)gvrow.FindControl("lblQueryDetails");
            Label ResponseDetails = (Label)gvrow.FindControl("lblResponseDetails");

            txtEnquiryid.Text = EnquiryId.Text;
            ddlEnquiryType.SelectedValue = EnquiryType.Text;
            txtAddr.Text = Address.Text;
            txtEnquiryBy.Text = EnquiredBy.Text;
            txtMobno.Text = MobileNo.Text;
            txtEmailid.Text = MailId.Text;
            txtQuerydetails.Text = QueryDetails.Text;
            txtresponseDetails.Text = ResponseDetails.Text;

            btnSubmit.Text = "Update";
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //protected async void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    //{
      


    //    try
    //    {
    //        ImageButton lnkbtn = sender as ImageButton;
    //        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        

    //        string EnquiryId = gvEnquiry.DataKeys[gvrow.RowIndex].Value.ToString();

    //        Label EnquiryType = (Label)gvrow.FindControl("lblEnquiryType");
    //        Label Address = (Label)gvrow.FindControl("lblAddress");
    //        Label EnquiredBy = (Label)gvrow.FindControl("lblEnquiredBy");
    //        Label MobileNo = (Label)gvrow.FindControl("lblMobileNo");
    //        Label MailId = (Label)gvrow.FindControl("lblMailId");
    //        Label QueryDetails = (Label)gvrow.FindControl("lblQueryDetails");
    //        Label ResponseDetails = (Label)gvrow.FindControl("lblResponseDetails");


    //        ddlEnquiryType.SelectedValue = EnquiryType.Text;
    //        txtAddr.Text = Address.Text;
    //        txtEnquiryBy.Text = EnquiredBy.Text;
    //        txtMobno.Text = MobileNo.Text;
    //        txtEmailid.Text = MailId.Text;
    //        txtQuerydetails.Text = QueryDetails.Text;
    //        txtresponseDetails.Text = ResponseDetails.Text;

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //            var enquiry = new enquiry()
    //            {

    //                QueryType = "Delete",
    //                EnquiryId = EnquiryId.ToString().Trim(),
    //                EnquiryType = ddlEnquiryType.SelectedValue.Trim(),
    //                EnquiredBy = txtEnquiryBy.Text.Trim(),
    //                Address = txtAddr.Text.Trim(),
    //                MobileNo = txtMobno.Text.Trim(),
    //                MailId = txtEmailid.Text.Trim(),
    //                QueryDetails = txtQuerydetails.Text.Trim(),
    //                ResponseDetails = txtresponseDetails.Text.Trim(),
    //                ResponseBy = hfCreatedBy.Value.Trim()


    //            };
    //            HttpResponseMessage response;
    //            string sMSG = string.Empty;
    //            response = await client.PostAsJsonAsync("Enquiry", enquiry);
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
    //                if (StatusCode == 1)
    //                {
    //                    BindEnquiry();
    //                    clearinputs();
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

    //                }
    //            }
    //            else
    //            {
    //                lblGridMsg.Text = response.ToString();
    //            }
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}
}