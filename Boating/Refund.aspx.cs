using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_Refund : System.Web.UI.Page
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
                BindUserName();

                RequestingAmountDetails("ByMe");
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Attributes.Add("readonly", "readonly");
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
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


                var ReFund = new Refund()
                {
                    QueryType = "Insert",
                    UniqueId = "0",
                    UserId = ViewState["DUId"].ToString(),
                    UserName = ViewState["DUName"].ToString(),
                    BoatTypeId = ViewState["DUBoatId"].ToString(),
                    BoatTypeName = ViewState["DUBoatName"].ToString(),
                    RequestedAmount = txtAmount.Text.Trim(),
                    RequestedBy = Session["UserId"].ToString().Trim(),
                    PaidBy = "0",
                    PaidDate = DateTime.Now.ToString(),
                    PaidAmount = "0",
                    PaymentStatus = "0",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),

                };

                response = client.PostAsJsonAsync("Refund/INS", ReFund).Result;
                sMSG = "Inserted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        clearInputs();
                        RequestingAmountDetails("ByMe");
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        clearInputs();
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
            clearInputs();
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void lbtnByMe_Click(object sender, EventArgs e)
    {
        RequestingAmountDetails("ByMe");
    }

    protected void lbtnByOthers_Click(object sender, EventArgs e)
    {
        RequestingAmountDetails("ByOthers");
    }

    public void RequestingAmountDetails(string type)
    {
        try
        {
            string sType = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                if (type == "ByMe")
                {
                    sType = "ByMe";
                }
                else
                {
                    sType = "ByOthers";
                }

                var body = new Refund()
                {
                    QueryType = "GetRequestAmount",
                    ServiceType = sType.Trim(),
                    Input1 = "",
                    Input2 = Session["UserId"].ToString().Trim(),
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;

                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (type == "ByMe")
                    {
                        if (dtExists.Rows.Count > 0)
                        {
                            GVRequestingAmount.DataSource = dtExists;
                            GVRequestingAmount.DataBind();
                            lblReqByMeMsg.Text = string.Empty;
                            divReqByMe.Visible = true;
                            GVRequestingAmount.Visible = true;
                            divReqByOthers.Visible = false;
                            int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Double>("RequestedAmount")));
                            decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("PaidAmount")));

                            GVRequestingAmount.FooterRow.Cells[3].Text = "Total";
                            GVRequestingAmount.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                            GVRequestingAmount.FooterRow.Cells[4].Text = TotalCount.ToString("N2");
                            GVRequestingAmount.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                            GVRequestingAmount.FooterRow.Cells[5].Text = TotalAmount.ToString("N2");
                            GVRequestingAmount.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;


                            divReqByOthers.Visible = false;
                        }
                        else
                        {
                            GVRequestingAmount.DataBind();
                            divReqByMe.Visible = true;
                            divReqByOthers.Visible = false;
                            lblReqByMeMsg.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        if (dtExists.Rows.Count > 0)
                        {
                            gvRequestAmtByOthers.DataSource = dtExists;
                            gvRequestAmtByOthers.DataBind();
                            divReqByOthers.Visible = true;
                            divReqByMe.Visible = false;
                            int TotalCount = dtExists.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Double>("RequestedAmount")));
                            decimal TotalAmount = dtExists.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("PaidAmount")));

                            gvRequestAmtByOthers.FooterRow.Cells[2].Text = "Total";
                            gvRequestAmtByOthers.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                            gvRequestAmtByOthers.FooterRow.Cells[3].Text = TotalCount.ToString("N2");
                            gvRequestAmtByOthers.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                            gvRequestAmtByOthers.FooterRow.Cells[4].Text = TotalAmount.ToString("N2");
                            gvRequestAmtByOthers.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;


                        }
                        else
                        {
                            gvRequestAmtByOthers.DataBind();
                            divReqByOthers.Visible = true;
                            divReqByMe.Visible = false;
                            lblReqByOthersMsg.Text = "No Records Found";

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

    public void PopUpRequestingAmountDetails()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new Refund()
                {
                    QueryType = "GetRequestAmountUserId",
                    ServiceType = "",
                    Input1 = ViewState["UniqueId"].ToString().Trim(),
                    Input2 = ViewState["UserId"].ToString().Trim(),
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;

                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {

                        gvRefundPay.DataSource = dtExists;
                        gvRefundPay.DataBind();


                    }
                    else
                    {
                        gvRefundPay.DataBind();

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnSettlement_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            ViewState["UserIdPay"] = gvRequestAmtByOthers.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();
            ViewState["RequestedBy"] = gvRequestAmtByOthers.DataKeys[gvrow.RowIndex]["RequestedBy"].ToString().Trim();
            if (ViewState["UserIdPay"].ToString() != Session["UserId"].ToString())
            {
                return;
            }
            else
            {

                ViewState["UniqueId"] = gvRequestAmtByOthers.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
                ViewState["UserId"] = gvRequestAmtByOthers.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();
                ViewState["UserName"] = gvRequestAmtByOthers.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim();
                ViewState["BoatTypeId"] = gvRequestAmtByOthers.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim();
                ViewState["BoatTypeName"] = gvRequestAmtByOthers.DataKeys[gvrow.RowIndex]["BoatTypeName"].ToString().Trim();

                ViewState["RequestedAmount"] = gvRequestAmtByOthers.DataKeys[gvrow.RowIndex]["RequestedAmount"].ToString().Trim();
                txtTotal.Text = ViewState["RequestedAmount"].ToString();
                PopUpRequestingAmountDetails();
                gvRefundPay.Visible = true;
                divPaidAmt.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void GVRequestingAmount_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVRequestingAmount.PageIndex = e.NewPageIndex;

        //RequestingAmountDetails();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearInputs();
    }

    public void clearInputs()
    {
        // ddlUserName.SelectedIndex = 0;
        txtUserName.Text = "";
        txtAmount.Text = "";
        txtBoatType.Text = "";

    }

    protected void btnFinalPay_Click(object sender, EventArgs e)
    {
        try
        {
            string Reqamount = ViewState["RequestedAmount"].ToString();
            string PayStatus = string.Empty;
            if (Convert.ToInt32(txtTotal.Text) <= Convert.ToInt32(Reqamount))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    PayStatus = "P";


                    HttpResponseMessage response;
                    string sMSG = string.Empty;

                    var Pay = new Refund()
                    {
                        QueryType = "Updatepay",
                        UniqueId = ViewState["UniqueId"].ToString().Trim(),
                        UserId = ViewState["UserId"].ToString(),
                        UserName = ViewState["UserName"].ToString(),
                        BoatTypeId = ViewState["BoatTypeId"].ToString(),
                        BoatTypeName = ViewState["BoatTypeName"].ToString(),
                        RequestedAmount = "0",
                        RequestedBy = "",
                        PaidBy = ViewState["UserIdPay"].ToString().Trim(),
                        PaidDate = DateTime.Now.ToString(),
                        PaidAmount = txtTotal.Text.Trim(),
                        PaymentStatus = PayStatus,
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),

                    };

                    response = client.PostAsJsonAsync("Refund/INS", Pay).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Response = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(Response)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(Response)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            RequestingAmountDetails("ByOthers");
                            clearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);

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
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Entered Amount is greater than Requested Amount .!');", true);
                txtTotal.Text = string.Empty;
                txtTotal.Focus();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvRefundPay_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRefundPay.PageIndex = e.NewPageIndex;

    }

    protected void GvUsername_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvUsername.PageIndex = e.NewPageIndex;
        BindUserName();
    }

    public void BindUserName()
    {
        try
        {
            divUsername.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Bookinghdr = new Refund()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim(),
                    BookingDate = DateTime.Now.ToString("dd/MM/yyyy")

                };
                HttpResponseMessage response = client.PostAsJsonAsync("UserName", Bookinghdr).Result;

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
                            GvUsername.DataSource = dt;
                            GvUsername.DataBind();
                            GvUsername.Visible = true;

                        }
                        else
                        {

                            divUsername.DataBind();
                            GvUsername.Visible = false;
                            divUsername.Visible = false;
                            lblReqByOthersMsg.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        divUsername.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    divUsername.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void lblBookingTotalAmount_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            Label UserName = (Label)gvrow.FindControl("lblName");
            Label UserId = (Label)gvrow.FindControl("lblUserId");

            string getUserId = UserId.Text.Trim();
            string logInUserId = Session["UserId"].ToString().Trim();

            if (getUserId.ToString().Trim() == logInUserId.ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Self Request Is Not Allowed !!!');", true);
                return;
            }

            Label BoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
            Label BoatType = (Label)gvrow.FindControl("lblBoatType");


            string User = UserName.Text.Trim();
            string Id = UserId.Text.Trim();
            string Type = BoatType.Text.Trim();

            txtBoatType.Text = BoatType.Text.Trim();
            txtUserName.Text = UserName.Text.Trim();
            ViewState["DUId"] = UserId.Text.Trim();
            ViewState["DUName"] = UserName.Text.Trim();
            ViewState["DUBoatId"] = BoatTypeId.Text.Trim();
            ViewState["DUBoatName"] = BoatType.Text.Trim();

            ////hfuserid.Value = UserName.Text;
            //ddlUserName.Items.Insert(0, new ListItem(User,Id));
            //ddlUserName.Enabled = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        lbtnView.Visible = false;
        lblCounter.Visible = true;
        divViewLog.Visible = true;
        divEntry.Visible = false;
        divReqAmnt.Visible = false;
        divbrdr.Visible = true;
        GvViewLog.PageIndex = 0;
        ViewLog();
    }

    protected void lblCounter_Click(object sender, EventArgs e)
    {
        lblCounter.Visible = false;
        lbtnView.Visible = true;
        divViewLog.Visible = false;
        divEntry.Visible = true;
        divReqAmnt.Visible = true;
        divbrdr.Visible = false;
    }

    public void ViewLog()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var body = new Refund()
                {
                    QueryType = "ViewLogRefundCounter",
                    ServiceType = "",
                    BookingId = "",
                    FromDate = txtDate.Text,
                    ToDate = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", body).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        GvViewLog.DataSource = dtExists;
                        GvViewLog.DataBind();
                        GvViewLog.Visible = true;
                        decimal Requestedamt = 0;
                        for (int i = 0; i < dtExists.Rows.Count; i++)
                        {
                            string F = dtExists.Rows[i].ToString();
                            Requestedamt += decimal.Parse(dtExists.Rows[i]["RequestedAmount"].ToString());
                        }

                        decimal PaidAmt = 0;
                        for (int i = 0; i < dtExists.Rows.Count; i++)
                        {
                            string F = dtExists.Rows[i].ToString();
                            PaidAmt += decimal.Parse(dtExists.Rows[i]["PaidAmount"].ToString());
                        }
                        GvViewLog.FooterRow.Cells[2].Font.Size = 20;
                        GvViewLog.FooterRow.Cells[2].Font.Bold = true;
                        GvViewLog.FooterRow.Cells[2].Text = "Total";
                        GvViewLog.FooterRow.Cells[3].Text = Requestedamt.ToString();
                        GvViewLog.FooterRow.Cells[3].ForeColor = Color.Green;
                        GvViewLog.FooterRow.Cells[3].Font.Bold = true;
                        GvViewLog.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                        GvViewLog.FooterRow.Cells[3].Font.Size = 20;

                        GvViewLog.FooterRow.Cells[5].Font.Bold = true;
                        GvViewLog.FooterRow.Cells[5].Font.Size = 20;
                        GvViewLog.FooterRow.Cells[5].Text = "Total";
                        GvViewLog.FooterRow.Cells[6].Text = PaidAmt.ToString();
                        GvViewLog.FooterRow.Cells[6].ForeColor = Color.Green;
                        GvViewLog.FooterRow.Cells[6].Font.Bold = true;
                        GvViewLog.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                        GvViewLog.FooterRow.Cells[6].Font.Size = 20;

                    }
                    else
                    {
                        GvViewLog.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('No Records Found');", true);
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        GvViewLog.PageIndex = 0;
        ViewLog();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        GvViewLog.PageIndex = 0;
        ViewLog();
    }

    protected void GvViewLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvViewLog.PageIndex = e.NewPageIndex;
        ViewLog();
    }

}

public class Refund
{
    public string QueryType { get; set; }
    public string CorpId { get; set; }
    public string ServiceType { get; set; }
    public string Input1 { get; set; }
    public string BranchCode { get; set; }
    public string Input2 { get; set; }
    public string Input3 { get; set; }
    public string Input4 { get; set; }
    public string Input5 { get; set; }
    public string UniqueId { get; set; }
    public string BoatTypeId { get; set; }

    public string BoatTypeName { get; set; }
    public string UserId { get; set; }

    public string UserName { get; set; }

    public string RequestedAmount { get; set; }

    public string RequestedBy { get; set; }

    public string RequestedDate { get; set; }

    public string PaidAmount { get; set; }

    public string PaidBy { get; set; }

    public string PaidDate { get; set; }

    public string RequestStatus { get; set; }

    public string PaymentStatus { get; set; }

    public string BoatHouseId { get; set; }

    public string BoatHouseName { get; set; }
    public string CreatedBy { get; set; }
    public string BookingDate { get; set; }

    public string BookingId { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }

}









