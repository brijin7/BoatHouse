using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class CancellationRule : System.Web.UI.Page
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

                Clear();
                hfCreatedBy.Value = Session["UserId"].ToString().Trim();
                hfBoatHouseId.Value = Session["BoatHouseId"].ToString().Trim();
                hfBoatHouseName.Value = Session["BoatHouseName"].ToString().Trim();
                rblChargeType.SelectedValue = "F";
                ChangDynamicDataDisplay();
                BindCancellationAmount();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public class cancellationAmount
    {
        public string ActivityId { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; }
        public string ChargeType { get; set; }
        public string Charges { get; set; }
        public string ApplicableBefore { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTill { get; set; }
        public string MaxNoOfResched { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string Createdby { get; set; }


    }
    public void ChangDynamicDataDisplay()
    {
        if (rblChargeType.SelectedValue == "P")
        {
            lblCharges.Text = "Charges Percentage";
            lblChargePer.Visible = true;
            lblChargeFix.Visible = false;
            txtCharges.Text = string.Empty;
        }
        else
        {
            lblCharges.Text = "Charges Fixed";
            lblChargePer.Visible = false;
            lblChargeFix.Visible = true;
            txtCharges.Text = string.Empty;
        }
    }

    protected void rblChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangDynamicDataDisplay();
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
                    var cancellationAmount1 = new cancellationAmount()
                    {
                        QueryType = "Insert",
                        ActivityId = "0",
                        Description = txtDescription.Text,
                        BoatHouseId = hfBoatHouseId.Value.Trim(),
                        BoatHouseName = hfBoatHouseName.Value.Trim(),
                        ActivityType = rblActivityType.SelectedValue.Trim(),
                        ChargeType = rblChargeType.SelectedValue.Trim(),
                        Charges = txtCharges.Text.Trim(),
                        ApplicableBefore = txtAppliBefore.Text.Trim(),
                        EffectiveFrom = txtEffectiveFrom.Text.Trim(),
                        EffectiveTill = txtEffectiveTill.Text.Trim(),
                        MaxNoOfResched = txtMaxNoOfSch.Text.Trim(),
                        Createdby = hfCreatedBy.Value.Trim()

                    };

                    response = await client.PostAsJsonAsync("MstrCancelReschedMaster", cancellationAmount1);

                }
                else
                {
                    var cancellationAmount1 = new cancellationAmount()
                    {
                        QueryType = "Update",
                        ActivityId = txtActivityId.Text,
                        Description = txtDescription.Text,
                        BoatHouseId = hfBoatHouseId.Value.Trim(),
                        BoatHouseName = hfBoatHouseName.Value.Trim(),
                        ActivityType = rblActivityType.SelectedValue.Trim(),
                        ChargeType = rblChargeType.SelectedValue.Trim(),
                        Charges = txtCharges.Text.Trim(),
                        ApplicableBefore = txtAppliBefore.Text.Trim(),
                        EffectiveFrom = txtEffectiveFrom.Text.Trim(),
                        EffectiveTill = txtEffectiveTill.Text.Trim(),
                        MaxNoOfResched = txtMaxNoOfSch.Text.Trim(),
                        Createdby = hfCreatedBy.Value.Trim()

                    };

                    response = await client.PostAsJsonAsync("MstrCancelReschedMaster", cancellationAmount1);

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindCancellationAmount();
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
        rblActivityType.SelectedValue = "C";
        txtDescription.Text = string.Empty;
        txtAppliBefore.Text = string.Empty;
        txtCharges.Text = string.Empty;
        txtEffectiveFrom.Text = string.Empty;
        txtEffectiveTill.Text = string.Empty;
        txtMaxNoOfSch.Text = string.Empty;
        divMaxSchedule.Visible = false;
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        BindCancellationAmount();
    }

    public async void BindCancellationAmount()
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
                var cancellationAmount1 = new cancellationAmount()
                {
                    BoatHouseId = hfBoatHouseId.Value.Trim()
                };
                response = await client.PostAsJsonAsync("CancelReschedMstr/BHId", cancellationAmount1);
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
                            gvCancellationAmount.DataSource = dt;
                            gvCancellationAmount.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvCancellationAmount.DataSource = dt;
                            gvCancellationAmount.DataBind();
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

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divGrid.Visible = false;
            divEntry.Visible = true;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvCancellationAmount.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ActivityId = (Label)gvrow.FindControl("lblActivityId");
            Label Description = (Label)gvrow.FindControl("lblDescription");
            Label ActivityType = (Label)gvrow.FindControl("lblActivityType");
            Label ChargeType = (Label)gvrow.FindControl("lblChargeType");
            Label Charges = (Label)gvrow.FindControl("lblCharges");
            Label ApplicableBefore = (Label)gvrow.FindControl("lblApplicableBefore");
            Label EffectiveFrom = (Label)gvrow.FindControl("lblEffectiveFrom");
            Label EffectiveTill = (Label)gvrow.FindControl("lblEffectiveTill");
            Label MaxNoOfResched = (Label)gvrow.FindControl("lblMaxNoOfResched");

            txtActivityId.Text = ActivityId.Text;
            txtDescription.Text = Description.Text;
            rblActivityType.SelectedValue = ActivityType.Text;
            rblChargeType.SelectedValue = ChargeType.Text;
            ChangDynamicDataDisplay();
            txtCharges.Text = Charges.Text;
            txtAppliBefore.Text = ApplicableBefore.Text;
            txtEffectiveFrom.Text = EffectiveFrom.Text;
            txtEffectiveTill.Text = EffectiveTill.Text;

            if (ActivityType.Text.Trim() == "R")
            {
                divMaxSchedule.Visible = true;
                txtMaxNoOfSch.Text = MaxNoOfResched.Text;
            }
            else
            {
                divMaxSchedule.Visible = false;
                txtMaxNoOfSch.Text = "0";
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected async void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try

        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvCancellationAmount.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ActivityId = (Label)gvrow.FindControl("lblActivityId");
            Label Description = (Label)gvrow.FindControl("lblDescription");
            Label ActivityType = (Label)gvrow.FindControl("lblActivityType");
            Label ChargeType = (Label)gvrow.FindControl("lblChargeType");
            Label Charges = (Label)gvrow.FindControl("lblCharges");
            Label ApplicableBefore = (Label)gvrow.FindControl("lblApplicableBefore");
            Label EffectiveFrom = (Label)gvrow.FindControl("lblEffectiveFrom");
            Label EffectiveTill = (Label)gvrow.FindControl("lblEffectiveTill");
            Label MaxNoOfResched = (Label)gvrow.FindControl("lblMaxNoOfResched");

            txtActivityId.Text = ActivityId.Text;
            txtDescription.Text = Description.Text;
            rblActivityType.SelectedValue = ActivityType.Text;
            rblChargeType.SelectedValue = ChargeType.Text;
            txtCharges.Text = Charges.Text;
            txtAppliBefore.Text = ApplicableBefore.Text;
            txtEffectiveFrom.Text = EffectiveFrom.Text;
            txtEffectiveTill.Text = EffectiveTill.Text;
            txtMaxNoOfSch.Text = MaxNoOfResched.Text;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                var cancellationAmount1 = new cancellationAmount()
                {
                    QueryType = "Delete",
                    ActivityId = txtActivityId.Text,
                    Description = txtDescription.Text,
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    BoatHouseName = hfBoatHouseName.Value.Trim(),
                    ActivityType = rblActivityType.SelectedValue.Trim(),
                    ChargeType = rblChargeType.SelectedValue.Trim(),
                    Charges = txtCharges.Text.Trim(),
                    ApplicableBefore = txtAppliBefore.Text.Trim(),
                    EffectiveFrom = txtEffectiveFrom.Text.Trim(),
                    EffectiveTill = txtEffectiveTill.Text.Trim(),
                    MaxNoOfResched = txtMaxNoOfSch.Text.Trim(),
                    Createdby = hfCreatedBy.Value.Trim()

                };

                response = await client.PostAsJsonAsync("MstrCancelReschedMaster", cancellationAmount1);


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindCancellationAmount();
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

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        Clear();
        btnSubmit.Text = "Submit";
        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
    }

    protected void gvCancellationAmount_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // for example, the row contains a certain property
                if (((Label)e.Row.FindControl("lblActiveStatus")).Text == "D")
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnEdit");
                    btnEdit.Visible = false;
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = false;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = true;
                }

                else
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnEdit");
                    btnEdit.Visible = true;
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

    protected async void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try

        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvCancellationAmount.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ActivityId = (Label)gvrow.FindControl("lblActivityId");
            Label Description = (Label)gvrow.FindControl("lblDescription");
            Label ActivityType = (Label)gvrow.FindControl("lblActivityType");
            Label ChargeType = (Label)gvrow.FindControl("lblChargeType");
            Label Charges = (Label)gvrow.FindControl("lblCharges");
            Label ApplicableBefore = (Label)gvrow.FindControl("lblApplicableBefore");
            Label EffectiveFrom = (Label)gvrow.FindControl("lblEffectiveFrom");
            Label EffectiveTill = (Label)gvrow.FindControl("lblEffectiveTill");
            Label MaxNoOfResched = (Label)gvrow.FindControl("lblMaxNoOfResched");

            txtActivityId.Text = ActivityId.Text;
            txtDescription.Text = Description.Text;
            rblActivityType.SelectedValue = ActivityType.Text;
            rblChargeType.SelectedValue = ChargeType.Text;
            txtCharges.Text = Charges.Text;
            txtAppliBefore.Text = ApplicableBefore.Text;
            txtEffectiveFrom.Text = EffectiveFrom.Text;
            txtEffectiveTill.Text = EffectiveTill.Text;
            txtMaxNoOfSch.Text = MaxNoOfResched.Text;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                var cancellationAmount1 = new cancellationAmount()
                {
                    QueryType = "ReActive",
                    ActivityId = txtActivityId.Text,
                    Description = txtDescription.Text,
                    BoatHouseId = hfBoatHouseId.Value.Trim(),
                    BoatHouseName = hfBoatHouseName.Value.Trim(),
                    ActivityType = rblActivityType.SelectedValue.Trim(),
                    ChargeType = rblChargeType.SelectedValue.Trim(),
                    Charges = txtCharges.Text.Trim(),
                    ApplicableBefore = txtAppliBefore.Text.Trim(),
                    EffectiveFrom = txtEffectiveFrom.Text.Trim(),
                    EffectiveTill = txtEffectiveTill.Text.Trim(),
                    MaxNoOfResched = txtMaxNoOfSch.Text.Trim(),
                    Createdby = hfCreatedBy.Value.Trim()

                };

                response = await client.PostAsJsonAsync("MstrCancelReschedMaster", cancellationAmount1);


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindCancellationAmount();
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

    protected void rblActivityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        divMaxSchedule.Visible = false;
        txtMaxNoOfSch.Text = "0";

        if (rblActivityType.SelectedValue == "R")
        {
            divMaxSchedule.Visible = true;
            txtMaxNoOfSch.Text = "";
        }
    }
}