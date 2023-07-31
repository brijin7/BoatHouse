using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShiftMaster : System.Web.UI.Page
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
                BindShiftMaster();
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearInputs();
        BindShiftMaster();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divgrid.Visible = true;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";

    }

    protected void btnSubmit_Click1(object sender, EventArgs e)
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
                    var shiftmaster = new shiftmaster()
                    {
                        QueryType = "Insert",
                        ShiftId = "0",
                        ShiftName = txtshiftname.Text.Trim(),
                        StartTime = txtstarttime.Text.Trim(),
                        EndTime = txtendtime.Text.Trim(),
                        BreakStartTime = txtBreakStartTime.Text.Trim(),
                        BreakEndTime = txtBreakendtime.Text.Trim(),
                        GracePeriod = txtGracePeriod.Text.ToString(),
                        CreatedBy = Session["UserId"].ToString().Trim()

                    };

                    response = client.PostAsJsonAsync("ShiftMstr", shiftmaster).Result;                   
                }
                else
                {

                    var shiftmaster1 = new shiftmaster()
                    {
                        QueryType = "Update",
                        ShiftId = txtshiftId.Text.Trim(),
                        ShiftName = txtshiftname.Text.Trim(),
                        StartTime = txtstarttime.Text.Trim(),
                        EndTime = txtendtime.Text.Trim(),
                        BreakStartTime = txtBreakStartTime.Text.Trim(),
                        BreakEndTime = txtBreakendtime.Text.Trim(),
                        GracePeriod = txtGracePeriod.Text.ToString(),
                        CreatedBy = Session["UserId"].ToString().Trim()

                    };

                    response = client.PostAsJsonAsync("ShiftMstr", shiftmaster1).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindShiftMaster();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divgrid.Visible = true;
            divEntry.Visible = true;
            lbtnNew.Visible = false;

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvshifMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label ShiftId = (Label)gvrow.FindControl("lblshiftId");
            Label ShiftName = (Label)gvrow.FindControl("lblShiftName");
            Label StartTime = (Label)gvrow.FindControl("lblStartTime");
            Label EndTime = (Label)gvrow.FindControl("lblEndTime");
            Label BreakStartTime = (Label)gvrow.FindControl("lblBreakStartTime");
            Label BreakEndTime = (Label)gvrow.FindControl("lblBreakEndTime");
            Label GracePeriod = (Label)gvrow.FindControl("lblGracePeriod");

            txtshiftId.Text = ShiftId.Text;
            txtshiftname.Text = ShiftName.Text;
            txtstarttime.Text = StartTime.Text;
            txtendtime.Text = EndTime.Text;
            txtBreakStartTime.Text = BreakStartTime.Text;
            txtBreakendtime.Text = BreakEndTime.Text;
            txtGracePeriod.Text = GracePeriod.Text;
            btnSubmit.Text = "Update";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string shiftId = gvshifMaster.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var shiftmaster = new shiftmaster()
                {

                    QueryType = "Delete",
                    ShiftId = shiftId.ToString().Trim(),
                    ShiftName = gvshifMaster.DataKeys[gvrow.RowIndex]["ShiftName"].ToString().Trim(),
                    StartTime = gvshifMaster.DataKeys[gvrow.RowIndex]["StartTime"].ToString().Trim(),
                    EndTime = gvshifMaster.DataKeys[gvrow.RowIndex]["EndTime"].ToString().Trim(),
                    BreakStartTime = gvshifMaster.DataKeys[gvrow.RowIndex]["BreakStartTime"].ToString().Trim(),
                    BreakEndTime = gvshifMaster.DataKeys[gvrow.RowIndex]["BreakEndTime"].ToString().Trim(),
                    GracePeriod = gvshifMaster.DataKeys[gvrow.RowIndex]["GracePeriod"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                    //ShiftName = txtshiftname.Text.Trim(),
                    //StartTime = txtstarttime.Text.Trim(),
                    //EndTime = txtendtime.Text.Trim(),
                    //BreakStartTime = txtBreakStartTime.Text.Trim(),
                    //BreakEndTime = txtBreakendtime.Text.Trim(),
                    //GracePeriod = txtGracePeriod.Text.ToString(),
                    //CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ShiftMstr", shiftmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindShiftMaster();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                    }
                }
                else
                {
                    lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string shiftId = gvshifMaster.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var shiftmaster = new shiftmaster()
                {
                    QueryType = "ReActive",
                    ShiftId = shiftId.ToString().Trim(),
                    ShiftName = gvshifMaster.DataKeys[gvrow.RowIndex]["ShiftName"].ToString().Trim(),
                    StartTime = gvshifMaster.DataKeys[gvrow.RowIndex]["StartTime"].ToString().Trim(),
                    EndTime = gvshifMaster.DataKeys[gvrow.RowIndex]["EndTime"].ToString().Trim(),
                    BreakStartTime = gvshifMaster.DataKeys[gvrow.RowIndex]["BreakStartTime"].ToString().Trim(),
                    BreakEndTime = gvshifMaster.DataKeys[gvrow.RowIndex]["BreakEndTime"].ToString().Trim(),
                    GracePeriod = gvshifMaster.DataKeys[gvrow.RowIndex]["GracePeriod"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                    //ShiftName = txtshiftname.Text.Trim(),
                    //StartTime = txtstarttime.Text.Trim(),
                    //EndTime = txtendtime.Text.Trim(),
                    //BreakStartTime = txtBreakStartTime.Text.Trim(),
                    //BreakEndTime = txtBreakendtime.Text.Trim(),
                    //GracePeriod = txtGracePeriod.Text.ToString(),
                    //CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("ShiftMstr", shiftmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindShiftMaster();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void gvshifMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvshifMaster.PageIndex = e.NewPageIndex;
        BindShiftMaster();
    }

    protected void txtendtime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtstarttime.Text == null || txtstarttime.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Start Time');", true);
                txtendtime.Text = string.Empty;
                return;
            }

            DateTime startTime = DateTime.Parse(txtstarttime.Text);
            DateTime endTime = DateTime.Parse(txtendtime.Text);

            if (endTime <= startTime)
            {
                txtendtime.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('EndTime should be Greater than StartTime ');", true);
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void txtBreakendtime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtBreakStartTime.Text == null || txtBreakStartTime.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Break StartTime ');", true);
                txtBreakStartTime.Text = string.Empty;
                return;
            }

            DateTime startTime = DateTime.Parse(txtBreakStartTime.Text);
            DateTime endTime = DateTime.Parse(txtBreakendtime.Text);

            if (endTime <= startTime)
            {
                txtBreakendtime.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('EndTime should be Greater than StartTime ');", true);
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void txtBreakStartTime_TextChanged(object sender, EventArgs e)
    {
        txtBreakendtime.Text = string.Empty;
    }

    protected void txtstarttime_TextChanged(object sender, EventArgs e)
    {
        txtendtime.Text = string.Empty;
    }

    public void BindShiftMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new shiftmaster()
                {
                    QueryType = "GetShiftMaster",
                    ServiceType = "",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvshifMaster.DataSource = dtExists;
                        gvshifMaster.DataBind();
                        lblGridMsg.Text = string.Empty;
                    }
                    else
                    {
                        gvshifMaster.DataSource = dtExists;
                        gvshifMaster.DataBind();
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        btnSubmit.Text = "Submit";

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public class shiftmaster
    {
        public string QueryType { get; set; }
        public string ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string BreakStartTime { get; set; }
        public string BreakEndTime { get; set; }
        public string GracePeriod { get; set; }
        public string CreatedBy { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }

    public void clearInputs()
    {
        txtshiftId.Text = string.Empty;
        txtshiftname.Text = string.Empty;
        txtstarttime.Text = string.Empty;
        txtendtime.Text = string.Empty;
        txtBreakStartTime.Text = string.Empty;
        txtBreakendtime.Text = string.Empty;
        txtGracePeriod.Text = string.Empty;
        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
    }
}