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

public partial class Boating_RptRowerCharges : System.Web.UI.Page
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
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                GetBoatType();
                GetRowerName();
                ViewState["hfstartvalue"] = "0";
                ViewState["hfendvalue"] = "0";
                int istart;
                int iend;
                AddProcess(0, 10, out istart, out iend);

                BindRowerChargesAll();
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
        Next.Enabled = true;

        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        BindRowerChargesAll();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        txtSearch.Text = string.Empty;
        BackToList.Visible = false;
        Next.Visible = true;
        back.Visible = true;
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        clearinputs();
    }

    public void BindRowerChargesAll()
    {
        try
        {
            divGridList.Visible = true;
            divSearch.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var RowerCharges = new RowerCharges()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    FromDate = txtFromDate.Text,
                    ToDate = txtToDate.Text,
                    RowerId = ddlrowerName.SelectedValue.Trim(),
                    CountStart = ViewState["hfstartvalue"].ToString()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("RowerCharger/ListAllV2", RowerCharges).Result;

                if (response.IsSuccessStatusCode)
                {
                    var gvList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count < 10)
                        {
                            Next.Enabled = false;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            GvRowerCharges.DataSource = dt;
                            GvRowerCharges.DataBind();
                            GvRowerCharges.Visible = true;
                        }
                        else
                        {
                            GvRowerCharges.Visible = false;
                        }
                    }
                    else
                    {
                        // divGridList.Visible = false;
                        GvRowerCharges.Visible = false;
                        txtSearch.Text = string.Empty;
                        Next.Enabled = false;
                        divSearch.Visible = false;
                        divprevnext.Visible = true;
                        if (ResponseMsg.Trim() == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                    }
                }
                else
                {
                    //divGridList.Visible = false;
                    GvRowerCharges.Visible = false;
                    txtSearch.Text = string.Empty;
                    Next.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BindRowerChargesAll();
    }
    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        BindRowerChargesAll();
    }

    protected void AddProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;
            back.Enabled = false;
            Next.Enabled = true;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = start + 10;

        }
        else
        {
            iend = end;

        }
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
    }
    protected void subProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
            Next.Enabled = true;
            back.Enabled = false;

        }
        else
        {
            istart = start;

        }
        if (end == 10)
        {
            iend = end;
            back.Enabled = false;
            Next.Enabled = true;

        }
        else
        {
            iend = end;
            Next.Enabled = true;

        }
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
    }

    protected void BackToList_Click(object sender, EventArgs e)
    {
        back.Visible = true;
        Next.Visible = true;
        BackToList.Visible = false;
        txtSearch.Text = string.Empty;
        BindRowerChargesAll();

    }

    public void BindRowerChargesAllSearch()
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

                var RowerCharges = new RowerCharges()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    FromDate = txtFromDate.Text,
                    ToDate = txtToDate.Text,
                    RowerId = ddlrowerName.SelectedValue.Trim(),
                    SearchBy = txtSearch.Text

                };

                HttpResponseMessage response = client.PostAsJsonAsync("RowerCharger/ListAllPinV2", RowerCharges).Result;

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
                            GvRowerCharges.DataSource = dt;
                            GvRowerCharges.DataBind();
                            GvRowerCharges.Visible = true;
                        }
                        else
                        {
                            GvRowerCharges.Visible = false;
                        }
                    }
                    else
                    {
                        // divGridList.Visible = false;
                        GvRowerCharges.Visible = false;
                        divSearch.Visible = false;
                        divprevnext.Visible = true;
                        BackToList.Visible = true;
                        txtSearch.Text = string.Empty;
                        if (ResponseMsg.Trim() == "No Records Found.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
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
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        BindRowerChargesAllSearch();
        back.Visible = false;
        Next.Visible = false;
        BackToList.Visible = true;
    }
    public void GetRowerName()
    {
        try
        {
            ddlrowerName.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var RowerCharges = new RowerCharges()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlRowerName", RowerCharges).Result;

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
                            ddlrowerName.DataSource = dt;
                            ddlrowerName.DataValueField = "RowerId";
                            ddlrowerName.DataTextField = "RowerName";
                            ddlrowerName.DataBind();

                        }
                        else
                        {
                            ddlrowerName.DataBind();
                        }

                        ddlrowerName.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        divGridList.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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

    public void GetBoatType()
    {
        try
        {
            ddlBoatType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var RowerCharges = new RowerCharges()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatType/BoatRateMstr", RowerCharges).Result;

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

                        ddlBoatType.Items.Insert(0, new ListItem("All", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void clearinputs()
    {
        ddlrowerName.SelectedIndex = 0;
        ddlBoatType.SelectedIndex = 0;
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        BindRowerChargesAll();
    }

    public class RowerCharges
    {
        public string BookingId { get; set; }
        public string BoatReferenceNo { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatType { get; set; }
        public string BoatSeaterId { get; set; }
        public string SeaterType { get; set; }
        public string RowerId { get; set; }
        public string RowerName { get; set; }
        public string ActualRowerCharge { get; set; }
        public string BoatHouseId { get; set; }
        public string CountStart { get; set; }
        public string RowNumber { get; set; }
        public string SearchBy { get; set; }
    }
}