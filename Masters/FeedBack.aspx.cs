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

public partial class Boating_FeedBack : System.Web.UI.Page
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
				hfCreatedBy.Value = Session["UserId"].ToString().Trim();
				BindFeedBack();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public class feedBack
    {
        public string QueryType { get; set; }
        public string FeedbackId { get; set; }
        public string GivenBy { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string MailId { get; set; }
        public string FeedbackDet { get; set; }
        public string HomePageDisplay { get; set; }
        public string ActionDetails { get; set; }
        public string Status { get; set; }
        public string ActionBy { get; set; }

        public string ActionDate { get; set; }

    }

    public async void BindFeedBack()
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

                response = await client.GetAsync("FeedBack/ListAll");
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
                            gvFeedBack.DataSource = dt;
                            gvFeedBack.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvFeedBack.DataSource = dt;
                            gvFeedBack.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();

                        divEntry.Visible = true;
                        divgrid.Visible = false;
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
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var feedBack1 = new feedBack()
                    {
                        QueryType = "Insert",
                        FeedbackId = "0",
                        GivenBy = txtGivenBy.Text,
                        Address = txtAddress.Text,
                        MobileNo = txtMobileNumbr.Text,
                        MailId = txtMailId.Text,
                        FeedbackDet = txtFeedBackComm.Text,
                        HomePageDisplay = rblImagedisplay.SelectedValue.Trim(),
                        ActionDetails = txtActionDetails.Text,
                        Status = "1",
                        ActionBy = hfCreatedBy.Value.Trim(),
                        ActionDate = DateTime.Now.ToString("dd/MM/yyyy")


                };

                    response = await client.PostAsJsonAsync("Feedbacks", feedBack1);

                }
                else
                {
                    var feedBack1 = new feedBack()
                    {
                        QueryType = "Update",
                        FeedbackId = txtFeedBackId.Text,
                        GivenBy = txtGivenBy.Text,
                        Address = txtAddress.Text,
                        MobileNo = txtMobileNumbr.Text,
                        MailId = txtMailId.Text,
                        FeedbackDet = txtFeedBackComm.Text,
                        HomePageDisplay = rblImagedisplay.SelectedValue.Trim(),
                        ActionDetails = txtActionDetails.Text,
                        Status = "1",
                        ActionBy = hfCreatedBy.Value.Trim(),
                        ActionDate = DateTime.Now.ToString("dd/MM/yyyy")


                    };

                    response = await client.PostAsJsonAsync("Feedbacks", feedBack1);
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindFeedBack();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    public void Clear()
    {
        txtGivenBy.Text = string.Empty;
        txtActionDetails.Text = string.Empty;
        txtMailId.Text= string.Empty;
        txtMobileNumbr.Text= string.Empty; ;
        txtAddress.Text = string.Empty;
        txtFeedBackComm.Text = string.Empty;
        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        btnSubmit.Text = "Submit";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        BindFeedBack();
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divgrid.Visible = false;
        divEntry.Visible = true;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
      
    }
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divgrid.Visible = false;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvFeedBack.DataKeys[gvrow.RowIndex].Value.ToString();
            Label FeedbackId = (Label)gvrow.FindControl("lblFeedbackId");
            Label GivenBy = (Label)gvrow.FindControl("lblGivenBy");
            Label MobileNo = (Label)gvrow.FindControl("lblMobileNo");
            Label MailId = (Label)gvrow.FindControl("lblMailId");
            Label Address = (Label)gvrow.FindControl("lblAddress");
            Label FeedbackDet = (Label)gvrow.FindControl("lblFeedbackDet");
            Label HomePageDisplay = (Label)gvrow.FindControl("lblHomePageDisplay");
            Label ActionDetails = (Label)gvrow.FindControl("lblActionDetails");

            txtFeedBackId.Text = FeedbackId.Text;
            txtGivenBy.Text = GivenBy.Text;
            txtMobileNumbr.Text = MobileNo.Text;
            txtMailId.Text = MailId.Text;
            txtAddress.Text = Address.Text;
            txtFeedBackComm.Text = FeedbackDet.Text;
            rblImagedisplay.SelectedValue = HomePageDisplay.Text;
            txtActionDetails.Text = ActionDetails.Text;
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
    //        string sTesfg = gvFeedBack.DataKeys[gvrow.RowIndex].Value.ToString();
    //        Label FeedbackId = (Label)gvrow.FindControl("lblFeedbackId");
    //        Label GivenBy = (Label)gvrow.FindControl("lblGivenBy");
    //        Label MobileNo = (Label)gvrow.FindControl("lblMobileNo");
    //        Label MailId = (Label)gvrow.FindControl("lblMailId");
    //        Label Address = (Label)gvrow.FindControl("lblAddress");
    //        Label FeedbackDet = (Label)gvrow.FindControl("lblFeedbackDet");
    //        Label HomePageDisplay = (Label)gvrow.FindControl("lblHomePageDisplay");
    //        Label ActionDetails = (Label)gvrow.FindControl("lblActionDetails");

    //        txtFeedBackId.Text = FeedbackId.Text;
    //        txtGivenBy.Text = GivenBy.Text;
    //        txtMobileNumbr.Text = MobileNo.Text;
    //        txtMailId.Text = MailId.Text;
    //        txtAddress.Text = Address.Text;
    //        txtFeedBackComm.Text = FeedbackDet.Text;
    //        rblImagedisplay.SelectedValue = HomePageDisplay.Text;
    //        txtActionDetails.Text = ActionDetails.Text;

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


    //            HttpResponseMessage response;
    //            var feedBack1 = new feedBack()
    //            {
    //                QueryType = "Delete",
    //                FeedbackId = txtFeedBackId.Text,
    //                GivenBy = txtGivenBy.Text,
    //                Address = txtAddress.Text,
    //                MobileNo = txtMobileNumbr.Text,
    //                MailId = txtMailId.Text,
    //                FeedbackDet = txtFeedBackComm.Text,
    //                HomePageDisplay = rblImagedisplay.SelectedValue.Trim(),
    //                ActionDetails = txtActionDetails.Text,
    //                Status =  "1",
    //                ActionBy = hfCreatedBy.Value.Trim(),
    //                ActionDate = DateTime.Now.ToString("dd/MM/yyyy")


    //            };

    //            response = await client.PostAsJsonAsync("Feedbacks", feedBack1);

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
    //                if (StatusCode == 1)
    //                {

    //                    BindFeedBack();
    //                    Clear();
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

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
}