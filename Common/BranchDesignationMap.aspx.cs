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

public partial class DepartmentDesignationMap : System.Web.UI.Page
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
        if (!IsPostBack)
        {

           
        }
        try
        {
            if (!IsPostBack)
            {
                if (Request.HttpMethod != "GET")
                {
                    AntiForgery.Validate(ViewState["__AntiForgeryCookie"] as string, Request.Form["__RequestVerificationToken"]);
                }

                //CHANGES
                GetCorporateOffice();
               // GetBranch();
                getDesignation();
                BindMapping();
                //CHANGES

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
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                string sQtype = string.Empty;
                string Id = string.Empty;


                if (btnSubmit.Text == "Submit")
                {

                    sQtype = "Insert";
                    Id = "0";
                }
                else
                {
                    sQtype = "Update";
                    Id = hfUniqueId.Value.Trim();
                }
                var Dept = new BranchDesig()
                {

                    QueryType = sQtype.Trim(),
                    UniqueId = Id.Trim(),
                    BranchId = ddlBranchCode.SelectedValue.Trim(),
                    Designation = ddlDesignation.SelectedValue.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("CM_BranchDesgMap", Dept).Result;


                if (response.IsSuccessStatusCode)
                {
                    var Response = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Response)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Response)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                        BindMapping();
                        ClearInputs();

                    }
                    else
                    {
                        BindMapping();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);

                    }
                }
                else
                {
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
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
        BindMapping();
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divgrid.Visible = true;
        divEntry.Visible = true;
        ClearInputs();
        lbtnNew.Visible = false;
        ddlBranchCode.Enabled = true;
        GetCorporateOffice();
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divgrid.Visible = true;
            lbtnNew.Visible = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            hfUniqueId.Value = gvMapping.DataKeys[gvrow.RowIndex]["UniqueId"].ToString();
            ddlCorpId.SelectedValue = gvMapping.DataKeys[gvrow.RowIndex]["CorpId"].ToString().Trim();
            GetBranch();
            ddlBranchCode.SelectedValue = gvMapping.DataKeys[gvrow.RowIndex]["BranchId"].ToString();
            ddlDesignation.Text = gvMapping.DataKeys[gvrow.RowIndex]["Designation"].ToString().Trim();

            ddlCorpId.Enabled = false;
            ddlBranchCode.Enabled = false;
            btnSubmit.Text = "Update";
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void ClearInputs()
    {
        ddlDesignation.SelectedIndex = 0;
        ddlBranchCode.ClearSelection();
        ddlBranchCode.Items.Clear();
        ddlBranchCode.Items.Insert(0, "Select Branch");
        ddlCorpId.ClearSelection();
        btnSubmit.Text = "Submit";
        ddlBranchCode.Enabled = true;
        ddlCorpId.Enabled = true;
    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        try
        {

            string MapId = gvMapping.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Mapping = new BranchDesig()
                {
                    QueryType = "ReActive",
                    UniqueId = gvMapping.DataKeys[gvrow.RowIndex]["UniqueId"].ToString(),
                    BranchId = gvMapping.DataKeys[gvrow.RowIndex]["BranchId"].ToString(),
                    Designation = gvMapping.DataKeys[gvrow.RowIndex]["Designation"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response= client.PostAsJsonAsync("CM_BranchDesgMap", Mapping).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        BindMapping();
                        ClearInputs();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        BindMapping();
                        ClearInputs();

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    BindMapping();
                    ClearInputs();

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var Mapping = new BranchDesig()
                {
                    QueryType = "Delete",
                    UniqueId = gvMapping.DataKeys[gvrow.RowIndex]["UniqueId"].ToString(),
                    BranchId = gvMapping.DataKeys[gvrow.RowIndex]["BranchId"].ToString(),
                    Designation = gvMapping.DataKeys[gvrow.RowIndex]["Designation"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_BranchDesgMap", Mapping).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindMapping();
                        ClearInputs();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        BindMapping();
                        ClearInputs();

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                    BindMapping();
                    ClearInputs();

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void dddlBranchCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindParticularMapping();
    }
    /// <summary>
    /// Added BY Abhinaya K
    /// Date :17.07.2023
    /// </summary>
    public void GetCorporateOffice()
    {
        try
        {
            
            ddlCorpId.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlCorporateOffice",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlCorpId.DataSource = dtExists;
                        ddlCorpId.DataValueField = "CorpId";
                        ddlCorpId.DataTextField = "CorpName";
                        ddlCorpId.DataBind();

                    }
                    else
                    {
                        ddlCorpId.DataBind();
                    }
                    ddlCorpId.Items.Insert(0, "Select Corporate Office");
                    ddlCorpId.SelectedValue = dtExists.Rows[0]["CorpId"].ToString();
                    GetBranch();
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
    /// <summary>
    /// Added BY Abhinaya K
    /// Date :17.07.2023
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCorpId_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBranch();
    }

    public void GetBranch()
    {
        try
        {
            ddlBranchCode.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new BranchDesig()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlBranchBasedOnCorpId",
                    CorpId = ddlCorpId.SelectedValue,
                    BranchCode = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlBranchCode.DataSource = dtExists;
                        ddlBranchCode.DataValueField = "BranchId";
                        ddlBranchCode.DataTextField = "BranchName";
                        ddlBranchCode.DataBind();
                    }
                    else
                    {
                        ddlBranchCode.DataBind();
                    }
                    ddlBranchCode.Items.Insert(0, "Select Branch");
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

    //DropDowns 
    public void getDesignation()
    {
        try
        {
            ddlDesignation.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new BranchDesig()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlDesignation",
                    CorpId = "",
                    BranchCode = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlDesignation.DataSource = dtExists;
                        ddlDesignation.DataValueField = "ConfigId";
                        ddlDesignation.DataTextField = "ConfigName";
                        ddlDesignation.DataBind();
                    }
                    else
                    {
                        ddlDesignation.DataBind();
                    }
                    ddlDesignation.Items.Insert(0, "Select Designation");
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

    public void BindMapping()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BranchType = new BranchDesig()
                {
                    QueryType = "GetBranchDesgMapBasedOnCorpId",
                    ServiceType = "All",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvMapping.DataSource = dtExists;
                        gvMapping.DataBind();

                        divEntry.Visible = false;
                        divgrid.Visible = true;
                        lbtnNew.Visible = true;
                    }
                    else
                    {
                        divEntry.Visible = true;
                        divgrid.Visible = false;
                        lbtnNew.Visible = false;

                        gvMapping.DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void BindParticularMapping()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BranchType = new BranchDesig()
                {
                    QueryType = "GetBranchDesgMapBasedOnCorpId",
                    ServiceType = "",
                    CorpId = ddlCorpId.SelectedValue.Trim(),
                    Input1 = ddlBranchCode.SelectedValue.Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvMapping.DataSource = dtExists;
                        gvMapping.DataBind();

                        divEntry.Visible = true;
                        divgrid.Visible = true;
                        lbtnNew.Visible = false;
                    }
                    else
                    {
                        divEntry.Visible = true;
                        divgrid.Visible = false;
                        lbtnNew.Visible = false;

                        gvMapping.DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

    }

    public class BranchDesig
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string UniqueId { get; set; }
        public string BranchId { get; set; }
        public string Designation { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
    }
    public class FA_CommonMethod
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }
}