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

public partial class Master_DepartmentMaster : System.Web.UI.Page
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
                GetCorporateOffice();
                BindDeptMstr();
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divEntry.Visible = true;
        ClearInputs();
        lbtnNew.Visible = false;
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

                if (btnSubmit.Text == "Submit")
                {
                    var DeptMstr = new DeptMstr()
                    {
                        QueryType = "Insert",
                        CorpId = ddlCorporateOff.SelectedValue.Trim(),
                        BranchCode = ddlBranchCode.SelectedValue.Trim(),
                        DeptId = txtDeptId.Text.Trim(),
                        DeptName = txtDeptName.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("CM_InsDeptMstr", DeptMstr).Result;
                }
                else
                {
                    var DeptMstr = new DeptMstr()
                    {
                        QueryType = "Update",
                        CorpId = ddlCorporateOff.SelectedValue.Trim(),
                        BranchCode = ddlBranchCode.SelectedValue.Trim(),
                        DeptId = txtDeptId.Text.Trim(),
                        DeptName = txtDeptName.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("CM_InsDeptMstr", DeptMstr).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindDeptMstr();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        BindDeptMstr();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
        BindDeptMstr();
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

            ddlCorporateOff.SelectedValue = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["CorpId"].ToString();
            ddlBranchCode.SelectedValue = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["BranchCode"].ToString();
            txtDeptId.Text = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["DeptId"].ToString();
            txtDeptName.Text = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["DeptName"].ToString();

            ddlCorporateOff.Enabled = false;
            ddlBranchCode.Enabled = false;
            txtDeptId.Enabled = false;
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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var DeptMstr = new DeptMstr()
                {
                    QueryType = "Delete",
                    CorpId = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["CorpId"].ToString(),
                    BranchCode = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["BranchCode"].ToString(),
                    DeptId = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["DeptId"].ToString(),
                    DeptName = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["DeptName"].ToString(),
                    CreatedBy = Session["UserId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_InsDeptMstr", DeptMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindDeptMstr();
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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var DeptMstr = new DeptMstr()
                {
                    QueryType = "ReActive",
                    CorpId = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["CorpId"].ToString(),
                    BranchCode = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["BranchCode"].ToString(),
                    DeptId = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["DeptId"].ToString(),
                    DeptName = gvDepartmentMstr.DataKeys[gvrow.RowIndex]["DeptName"].ToString(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_InsDeptMstr", DeptMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindDeptMstr();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    public void ClearInputs()
    {
        ddlCorporateOff.Enabled = true;
        ddlBranchCode.Enabled = true;
        txtDeptId.Enabled = true;
        gvDptBranchBased.Visible = false;
        ddlBranchCode.SelectedIndex = -1;
        txtDeptId.Text = "";
        txtDeptName.Text = "";
        btnSubmit.Text = "Submit";
    }

    public void GetCorporateOffice()
    {
        try
        {
            ddlCorporateOff.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new DeptMstr()
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
                        ddlCorporateOff.DataSource = dtExists;
                        ddlCorporateOff.DataValueField = "CorpId";
                        ddlCorporateOff.DataTextField = "CorpName";
                        ddlCorporateOff.DataBind();
                    }
                    else
                    {
                        ddlCorporateOff.DataBind();
                    }
                    ddlCorporateOff.Items.Insert(0, "Select Corporate Office");
                    ddlCorporateOff.SelectedValue = "1";
                    GetBranch(ddlCorporateOff.SelectedValue);
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

    public void GetBranch(string sCorporateOff)
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
                var BranchType = new DeptMstr()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlBranchCode",
                    CorpId = sCorporateOff.ToString().Trim(),
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
                        ddlBranchCode.DataValueField = "BranchCode";
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

    protected void ddlBranchCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlBranchCode.SelectedIndex == 0)
        {
            txtDeptId.Text = "";
            gvDptBranchBased.Visible = false;
        }
        else
        {
            BindBranchBasedFilter();
            MaxDeptId();

            txtDeptName.Focus();
        }       
    }

    public void MaxDeptId()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BranchType = new DeptMstr()
                {
                    QueryType = "MaxDeptId",
                    ServiceType = "",
                    CorpId = "",
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
                        txtDeptId.Text = dtExists.Rows[0]["DeptId"].ToString();
                    }
                    else
                    {
                        txtDeptId.Text = "";
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

    public void BindBranchBasedFilter()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BranchType = new DeptMstr()
                {
                    QueryType = "GetDeptbasedonBranch",
                    ServiceType = "",
                    CorpId = "",
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
                        gvDptBranchBased.DataSource = dtExists;
                        gvDptBranchBased.DataBind();
                        gvDptBranchBased.Visible = true;
                        divEntry.Visible = true;
                        divGrid.Visible = false;
                        lbtnNew.Visible = true;
                    }
                    else
                    {
                        gvDptBranchBased.Visible = false;
                        divEntry.Visible = true;
                        divGrid.Visible = false;
                        gvDptBranchBased.DataBind();
                        //txtDeptId.Text = string.Empty ;
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

    public void BindDeptMstr()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new DeptMstr()
                {
                    QueryType = "GetDeptMaster",
                    ServiceType = "",
                    CorpId = "",
                    BranchCode = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvDepartmentMstr.DataSource = dtExists;
                        gvDepartmentMstr.DataBind();

                        divEntry.Visible = false;
                        divGrid.Visible = true;
                        lbtnNew.Visible = true;
                    }
                    else
                    {
                        divEntry.Visible = true;
                        divGrid.Visible = false;
                        lbtnNew.Visible = false;
                        gvDepartmentMstr.DataBind();
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

    public class DeptMstr
    {
        public string QueryType { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
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