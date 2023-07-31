using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BoatMaster : System.Web.UI.Page
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

                hfCreatedBy.Value = Session["UserId"].ToString().Trim();
                BindBoatMaster();
                divPrivate.Visible = false;
                GetBoatType();
                GetBoatHouse();
                BindBoatMaster();
                BindBoatCount();
                GetBoatStatus();
                GetPaymentModel();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public class boatmaster
    {
        public string QueryType { get; set; }
        public string BoatId { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BoatNature { get; set; }
        public string BoatNum { get; set; }
        public string BoatName { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatStatus { get; set; }
        public string BoatSeaterId { get; set; }
        public string BoatOwner { get; set; }
        public string PaymentModel { get; set; }
        public string PaymentPercent { get; set; }
        public string PaymentAmount { get; set; }
        public string CreatedBy { get; set; }
        public string BoatSeaterName { get; set; }
    }

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

                var boatmaster = new boatmaster()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatType/BoatRateMstr", boatmaster).Result;

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
                            ddlBoatType.DataSource = dt;
                            ddlBoatType.DataValueField = "BoatTypeId";
                            ddlBoatType.DataTextField = "BoatType";
                            ddlBoatType.DataBind();

                        }
                        else
                        {
                            ddlBoatType.DataBind();
                        }
                        ddlBoatType.Items.Insert(0, "Select Boat Type");
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

    public void GetBoatHouse()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ddlBoatHouse?CorpId=" + Session["CorpId"].ToString() +"").Result;

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
                            ddlBoatHouse.DataSource = dt;
                            ddlBoatHouse.DataValueField = "BoatHouseId";
                            ddlBoatHouse.DataTextField = "BoatHouseName";
                            ddlBoatHouse.DataBind();

                        }
                        else
                        {
                            ddlBoatHouse.DataBind();
                        }
                    }
                    else
                    {

                        lblGridMsg.Text = ResponseMsg.ToString().Trim();

                    }
                    ddlBoatHouse.Items.Insert(0, new ListItem("Select Boat House", "0"));
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

    public void GetBoatSeater()
    {

    }
    public void GetPaymentModel()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLPayModel").Result;

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
                            ddlPayModel.DataSource = dt;
                            ddlPayModel.DataValueField = "ConfigId";
                            ddlPayModel.DataTextField = "ConfigName";
                            ddlPayModel.DataBind();

                        }
                        else
                        {
                            ddlPayModel.DataBind();
                        }
                        ddlPayModel.Items.Insert(0, new ListItem("Select Payment Mode", "0"));
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
    public void GetBoatStatus()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLBoatStatus").Result;

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
                            ddlBoatStatus.DataSource = dt;
                            ddlBoatStatus.DataValueField = "ConfigId";
                            ddlBoatStatus.DataTextField = "ConfigName";
                            ddlBoatStatus.DataBind();

                        }
                        else
                        {
                            ddlBoatStatus.DataBind();
                        }
                        ddlBoatStatus.Items.Insert(0, new ListItem("Select Boat Status", "0"));
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

    protected void lblBoatTypeName_Click(object sender, EventArgs e)
    {

        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string Id = gvCount.DataKeys[gvrow.RowIndex].Value.ToString();

        LinkButton lblBoatTypeName = (LinkButton)gvrow.FindControl("lblBoatTypeName");

        hfBoat.Value = lblBoatTypeName.Text.Trim();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string PayModel = "0";
            string PayPercent = "0";
            string PayAmt = "0";

            if (rblBoatOwner.SelectedValue == "P")
            {
                if (txtPayPercent.Text == "" && txtPayAmt.Text == "" || txtPayPercent.Text == "0" && txtPayAmt.Text == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Either Payment Percent or Amount ');", true);
                    return;
                }

                PayModel = ddlPayModel.SelectedValue.Trim();
                PayPercent = txtPayPercent.Text;
                PayAmt = txtPayAmt.Text;
            }

            if (rblBoatOwner.SelectedValue == "P" || rblBoatOwner.SelectedValue == "T")
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
                        var boatmaster = new boatmaster()
                        {
                            QueryType = "Insert",
                            BoatId = "0",
                            BoatNum = txtBoatNum.Text,
                            BoatName = txtBoatName.Text,
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                            BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                            BoatStatus = ddlBoatStatus.SelectedValue.Trim(),
                            BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                            BoatOwner = rblBoatOwner.SelectedValue.Trim(),
                            PaymentModel = PayModel.Trim(),
                            PaymentPercent = PayPercent,
                            PaymentAmount = PayAmt,
                            BoatNature = rblboatnature.SelectedValue.Trim(),
                            CreatedBy = hfCreatedBy.Value.Trim()
                        };

                        response = client.PostAsJsonAsync("BoatMstr", boatmaster).Result;
                    }
                    else
                    {
                        if (rblBoatOwner.SelectedValue == "T")
                        {
                            ddlPayModel.SelectedValue = "0";
                            txtPayPercent.Text = "0";
                            txtPayAmt.Text = "0";
                        }

                        var boatmaster = new boatmaster()
                        {
                            QueryType = "Update",
                            BoatId = txtBoatId.Text,
                            BoatNum = txtBoatNum.Text,
                            BoatName = txtBoatName.Text,
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                            BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                            BoatStatus = ddlBoatStatus.SelectedValue.Trim(),
                            BoatSeaterId = ddlBoatSeater.SelectedValue.Trim(),
                            BoatOwner = rblBoatOwner.SelectedValue.Trim(),
                            PaymentModel = PayModel.Trim(),
                            PaymentPercent = PayPercent,
                            PaymentAmount = PayAmt,
                            BoatNature = rblboatnature.SelectedValue.Trim(),
                            CreatedBy = hfCreatedBy.Value.Trim()
                        };

                        response = client.PostAsJsonAsync("BoatMstr", boatmaster).Result;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                        if (StatusCode == 1)
                        {
                            clearInputs();
                            BindBoatMaster();
                            divgrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            BindBoatCount();

                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {
                            divgrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            clearInputs();
                            BindBoatMaster();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        var Errorresponse = response.Content.ReadAsStringAsync().Result;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    }

                    BindBoatMaster();
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
        clearInputs();
        divEntry.Visible = false;
        divgrid.Visible = true;
        lbtnNew.Visible = true;

    }

    public void clearInputs()
    {
        ddlBoatType.SelectedIndex = 0;
        ddlBoatStatus.SelectedIndex = 1;
        txtBoatNum.Text = string.Empty;
        txtBoatName.Text = string.Empty;
        txtPayAmt.Text = string.Empty;
        ddlPayModel.SelectedIndex = 0;
        txtPayPercent.Text = string.Empty;
        ddlBoatHouse.SelectedIndex = 0;
        ddlBoatSeater.Items.Clear();
    }

    //Showing Records
    public void BindBoatCount()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatHouseId = new boatmaster()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatMstr/Count", BoatHouseId).Result;

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
                            gvCount.DataSource = dt;
                            gvCount.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divgrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;

                            int TotalNormal = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Normal")));
                            int TotalPremium = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Express")));
                            int Total = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Total")));

                            gvCount.FooterRow.Cells[2].Text = "Total";
                            gvCount.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                            gvCount.FooterRow.Cells[3].Text = TotalNormal.ToString();
                            gvCount.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                            gvCount.FooterRow.Cells[4].Text = TotalPremium.ToString();
                            gvCount.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                            gvCount.FooterRow.Cells[5].Text = Total.ToString();
                            gvCount.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            gvCount.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divgrid.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;
                        }

                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg;
                        divgrid.Visible = false;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindBoatMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var BoatHouseId = new boatmaster()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatMstr/BHId", BoatHouseId).Result;

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
                            gvBoatMaster.DataSource = dt;
                            gvBoatMaster.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvBoatMaster.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        clearInputs();
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
            string sTesfg = gvBoatMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label BoatId = (Label)gvrow.FindControl("lblBoatId");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label BoatNum = (Label)gvrow.FindControl("lblBoatNum");
            Label BoatName = (Label)gvrow.FindControl("lblBoatName");
            Label BoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
            Label BoatTypeName = (Label)gvrow.FindControl("lblBoatTypeName");
            Label BoatStatus = (Label)gvrow.FindControl("lblBoatStatus");
            Label BoatSeaterId = (Label)gvrow.FindControl("lblBoatSeaterId");
            Label BoatOwner = (Label)gvrow.FindControl("lblBoatOwner");
            Label BoatNature = (Label)gvrow.FindControl("lblBoatNature");
            Label PaymentModel = (Label)gvrow.FindControl("lblPaymentModel");

            Label PaymentPercent = (Label)gvrow.FindControl("lblPaymentPercent");
            Label PaymentAmount = (Label)gvrow.FindControl("lblPaymentAmount");

            txtBoatId.Text = BoatId.Text;
            ddlBoatHouse.SelectedValue = BoatHouseId.Text;
            ddlBoatType.SelectedValue = BoatTypeId.Text;

            BindSeaterType(ddlBoatType.SelectedValue.Trim());

            ddlBoatSeater.SelectedValue = BoatSeaterId.Text;
            txtBoatNum.Text = BoatNum.Text;
            txtBoatName.Text = BoatName.Text;

            ddlPayModel.SelectedValue = PaymentModel.Text;
            ddlBoatStatus.SelectedValue = BoatStatus.Text;
            txtPayAmt.Text = PaymentAmount.Text;
            txtPayPercent.Text = PaymentPercent.Text;
            string BoatOWNER = BoatOwner.Text;
            if (BoatOwner.Text == "Private")
            {
                rblBoatOwner.SelectedValue = "P";
            }
            else
            {
                rblBoatOwner.SelectedValue = "T";
            }

            if (rblBoatOwner.SelectedValue == "P")
            {
                divPrivate.Visible = true;
            }
            else
            {
                divPrivate.Visible = false;
            }
            string BoatNatures = BoatNature.Text;
            if (BoatNature.Text == "Normal")
            {
                rblboatnature.SelectedValue = "N";
            }
            else
            {
                rblboatnature.SelectedValue = "P";
            }

        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void rblBoatOwner_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblBoatOwner.SelectedValue == "P")
        {
            divPrivate.Visible = true;
        }
        else
        {
            divPrivate.Visible = false;
        }

    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divgrid.Visible = false;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        rblboatnature.SelectedValue = "N";
        clearInputs();
    }

    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBoatType.SelectedIndex == 0)
        {
            return;
        }

        BindSeaterType(ddlBoatType.SelectedValue.Trim());
    }

    public void BindSeaterType(string BoatTypeId)
    {
        try
        {
            ddlBoatSeater.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var boatmaster = new boatmaster()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = BoatTypeId
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSeat/BoatRateMstr", boatmaster).Result;

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
                            ddlBoatSeater.DataSource = dt;
                            ddlBoatSeater.DataValueField = "BoatSeaterId";
                            ddlBoatSeater.DataTextField = "SeaterType";
                            ddlBoatSeater.DataBind();

                        }
                        else
                        {
                            ddlBoatSeater.DataBind();
                        }
                        ddlBoatSeater.Items.Insert(0, "Select Boat Seater");
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

    protected void GenerateQRCode_Click(object sender, EventArgs e)
    {
        Button lnkbtn = sender as Button;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string sTesfg = gvBoatMaster.DataKeys[gvrow.RowIndex].Value.ToString();
        Label BoatId = (Label)gvrow.FindControl("lblBoatId");
        string IBoatId = BoatId.Text;
        Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
        string IBoatHouseId = BoatHouseId.Text;
        Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
        Label BoatNum = (Label)gvrow.FindControl("lblBoatNum");
        Label BoatName = (Label)gvrow.FindControl("lblBoatName");
        Label BoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
        string IBoatTypeId = BoatTypeId.Text;
        Label BoatTypeName = (Label)gvrow.FindControl("lblBoatTypeName");
        string IBoatTypeName = BoatTypeName.Text;
        Label BoatStatus = (Label)gvrow.FindControl("lblBoatStatus");
        Label BoatSeaterId = (Label)gvrow.FindControl("lblBoatSeaterId");
        string IBoatSeaterId = BoatSeaterId.Text;
        Label BoatOwner = (Label)gvrow.FindControl("lblBoatOwner");
        Label BoatNature = (Label)gvrow.FindControl("lblBoatNature");
        Label BoatSeaterName = (Label)gvrow.FindControl("lblBoatSeaterName");
        string IBoatSeaterName = BoatSeaterName.Text;

        BindIdCard(IBoatHouseId, IBoatTypeId, IBoatTypeName, IBoatSeaterId, IBoatSeaterName, IBoatId);
    }
    private byte[] GetImage(string url)
    {
        Stream stream = null;
        byte[] buf;

        try
        {
            WebProxy myProxy = new WebProxy();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                int len = (int)(response.ContentLength);
                buf = br.ReadBytes(len);
                br.Close();
            }

            stream.Close();
            response.Close();
        }
        catch (Exception)
        {
            buf = null;
        }

        return (buf);
    }
    public void BindIdCard(string BoatHouseId, string BoatTypeid, string BoatTypeName, string BoatSeaterId, string BoatSeaterName, string BoatId)
    {
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        try
        {
            string CorpLogo = Session["CorpLogo"].ToString();

            string strFn1 = LoadBarcodeImage(BoatHouseId,
                 BoatTypeid,
                 BoatSeaterId,
                 BoatId,
                 BoatSeaterName);
            using (var webClient = new WebClient())
            {
                byte[] imageBytes = webClient.DownloadData(CorpLogo);

                if (strFn1 == "QR Code not Generated.")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('QR Code Not Generated.');", true);
                }
                else
                {
                    StringBuilder _sb1 = new StringBuilder();
                    Byte[] _byte1 = this.GetImage(strFn1);
                    _sb1.Append(Convert.ToBase64String(_byte1, 0, _byte1.Length));
                    string sbBase641 = _sb1.ToString();

                    DataTable dtnew = new DataTable();
                    dtnew.Columns.Add("BoatHouseId");
                    dtnew.Columns.Add("BoatTypeId");
                    dtnew.Columns.Add("BoatTypeName");
                    dtnew.Columns.Add("BoatSeaterId");
                    dtnew.Columns.Add("BoatSeaterName");
                    dtnew.Columns.Add("BoatId");
                    dtnew.Columns.Add("Barcode", typeof(byte[]));
                    dtnew.Columns.Add("CorpLogo", typeof(byte[]));


                    dtnew.Rows.Add(BoatHouseId,
                      BoatTypeid,
                      BoatTypeName + "-" + BoatSeaterName,
                      BoatSeaterId,
                     BoatSeaterName,
                      "Boat ID : " + BoatId,
                      _byte1, imageBytes);

                    objReportDocument.Load(Server.MapPath("BoatIDCard.rpt"));
                    objReportDocument.SetDataSource(dtnew);
                    objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString());
                    objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                    objReportExport = objReportDocument.ExportOptions;
                    objReportExport.ExportDestinationOptions = objReportDiskOption;
                    objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                    objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                    objReportDocument.Export();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(Server.MapPath(sFilePath));
                    Response.Flush();
                    Response.Close();
                }
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
        finally
        {
            if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
            {
                System.IO.File.Delete(Server.MapPath(sFilePath));
            }

            objReportDocument.Dispose();

            objReportDiskOption = null;
            objReportDocument = null;
            objReportExport = null;
            GC.Collect();
        }
    }
    public string LoadBarcodeImage(string sBoatHouseId, string sBoatTypeId, string sBoatSeaterId, string sBoatId, string sBoatSeaterName)
    {
        string imgresponse = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseQRUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var QRd = new boatmaster()
                {
                    BoatHouseId = sBoatHouseId,
                    BoatTypeId = sBoatTypeId,
                    BoatSeaterId = sBoatSeaterId,
                    BoatId = sBoatId,
                    BoatSeaterName = sBoatSeaterName
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatInfoQR", QRd).Result;

                if (response.IsSuccessStatusCode)
                {
                    var GetResponse = response.Content.ReadAsStringAsync().Result;
                    string Status = JObject.Parse(GetResponse)["status"].ToString();
                    string ResponseMsg2 = JObject.Parse(GetResponse)["imageURL"].ToString();

                    if (Status == "Success.")
                    {
                        imgresponse = ResponseMsg2;
                    }
                    else
                    {
                        imgresponse = "QR Code not Generated.";
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);

        }
        return imgresponse;
    }

}


