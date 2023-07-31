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

public partial class Master_BranchMaster : System.Web.UI.Page
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
                BindBranchMaster();
                GetCorporateOffice();
                GetBranchType();
                GetBranchRegion();
                GetCountry();
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
        divGrid.Visible = true;
        divEntry.Visible = true;
        lbtnNew.Visible = false;
        DivNotOperation.Visible = false;
        ClearInputs();
        btnSubmit.Text = "Submit";
        ddlBranchType.SelectedValue = "3";
    }

    protected void txtZipCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtZipCode.Text == "" || txtZipCode.Text == null)
            {

            }
            using (var client = new HttpClient())
            {
                string ZipURL = "http://www.postalpincode.in/api/pincode/";
                client.BaseAddress = new Uri(ZipURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("http://www.postalpincode.in/api/pincode/" + txtZipCode.Text + "").Result;

                string mesg = " Invalid Zip Code";
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    string Status = JObject.Parse(Locresponse)["Status"].ToString();
                    string Message = JObject.Parse(Locresponse)["Message"].ToString();
                    string PostOffice = JObject.Parse(Locresponse)["PostOffice"].ToString();
                    if (Status == "Success")
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(PostOffice);
                        if (dt.Rows.Count > 0)
                        {
                            txtCity.Text = dt.Rows[0]["Circle"].ToString();
                            txtDistrict.Text = dt.Rows[0]["District"].ToString();
                            txtState.Text = dt.Rows[0]["State"].ToString();
                        }
                        else
                        {
                            txtCity.Text = "";
                            txtDistrict.Text = "";
                            txtState.Text = "";
                        }
                    }
                    else
                    {
                        txtCity.Text = "";
                        txtDistrict.Text = "";
                        txtState.Text = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + mesg.ToString().Trim() + "');", true);
                        return;
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
                string qType = string.Empty;
                string sBranchId = string.Empty;
                string OperatingStatus = string.Empty;                

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    qType = "Insert";
                    sBranchId = "";
                    OperatingStatus = "Y";
                }
                else
                {
                    qType = "Update";
                    sBranchId = hfBranchId.Value.Trim();
                    OperatingStatus = "";
                }


                var BranchMaster = new FA_BranchMaster()
                {
                    QueryType = qType.Trim(),
                    CorpId = ddlCorporateOff.SelectedValue.Trim(),
                    BranchId = sBranchId.ToString().Trim(),
                    BranchCode = txtBranchCode.Text.Trim(),
                    BranchName = txtBranchName.Text.Trim(),
                    BranchType = ddlBranchType.SelectedValue.Trim(),
                    BranchRegion = ddlBranchRegion.SelectedValue.Trim(),
                    Address1 = txtAddress1.Text.Trim(),
                    Address2 = txtAddress2.Text.Trim(),
                    ZipCode = txtZipCode.Text.Trim(),
                    City = txtCity.Text.Trim(),
                    District = txtDistrict.Text.Trim(),
                    State = txtState.Text.Trim(),
                    Country = ddlCountry.SelectedValue.Trim(),
                    OperatingStatus= OperatingStatus.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("CM_InsBranchMstr", BranchMaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            BindBranchMaster();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            divEntry.Visible = false;
                            divGrid.Visible = true;
                            lbtnNew.Visible = true;
                        }
                        else if (btnSubmit.Text.Trim() == "Update")
                        {
                            BindBranchMaster();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            divEntry.Visible = false;
                            divGrid.Visible = true;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            BindBranchMaster();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            divEntry.Visible = false;
                            divGrid.Visible = true;
                            lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        BindBranchMaster();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    BindBranchMaster();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
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
        btnSubmit.Text = "Submit";
        BindBranchMaster();
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
           
        {
            
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            string Notoperation = string.Empty;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            
            txtBranchCode.Text = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchCode"].ToString().Trim();
            hfBranchId.Value = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchId"].ToString().Trim();
            txtBranchName.Text = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchName"].ToString().Trim();
            ddlBranchType.SelectedValue = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchType"].ToString().Trim();
            ddlBranchRegion.SelectedValue = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchRegion"].ToString().Trim();

            txtAddress1.Text = gvBranchMaster.DataKeys[gvrow.RowIndex]["Address1"].ToString().Trim();
            txtAddress2.Text = gvBranchMaster.DataKeys[gvrow.RowIndex]["Address2"].ToString().Trim();
            txtZipCode.Text = gvBranchMaster.DataKeys[gvrow.RowIndex]["Zipcode"].ToString().Trim();
            txtCity.Text = gvBranchMaster.DataKeys[gvrow.RowIndex]["City"].ToString().Trim();

            txtDistrict.Text = gvBranchMaster.DataKeys[gvrow.RowIndex]["District"].ToString().Trim();
            txtState.Text = gvBranchMaster.DataKeys[gvrow.RowIndex]["State"].ToString().Trim();
            ddlCountry.SelectedValue = gvBranchMaster.DataKeys[gvrow.RowIndex]["Country"].ToString().Trim();
            Notoperation= gvBranchMaster.DataKeys[gvrow.RowIndex]["OperatingStatus"].ToString().Trim();
            if(Notoperation == "Y")
            {              
                DivNotOperation.Visible = false;
            }
            else
            {                
                DivNotOperation.Visible = true;
            }
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
            string OperatingStatus = string.Empty;
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            OperatingStatus = gvBranchMaster.DataKeys[gvrow.RowIndex]["OperatingStatus"].ToString().Trim();
            if (OperatingStatus == "Y")
            {
                OperatingStatus = "Y";

            }
            else
            {
                OperatingStatus = "N";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BranchMstr = new FA_BranchMaster()
                {
                    QueryType = "Delete",
                    CorpId = gvBranchMaster.DataKeys[gvrow.RowIndex]["CorpId"].ToString().Trim(),
                    BranchCode = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchCode"].ToString().Trim(),
                    BranchId = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchId"].ToString().Trim(),
                    BranchName = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchName"].ToString().Trim(),
                    BranchType = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchType"].ToString().Trim(),
                    BranchRegion = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchRegion"].ToString().Trim(),
                    Address1 = gvBranchMaster.DataKeys[gvrow.RowIndex]["Address1"].ToString().Trim(),
                    Address2 = gvBranchMaster.DataKeys[gvrow.RowIndex]["Address2"].ToString().Trim(),
                    ZipCode = gvBranchMaster.DataKeys[gvrow.RowIndex]["Zipcode"].ToString().Trim(),
                    City = gvBranchMaster.DataKeys[gvrow.RowIndex]["City"].ToString().Trim(),
                    District = gvBranchMaster.DataKeys[gvrow.RowIndex]["District"].ToString().Trim(),
                    State = gvBranchMaster.DataKeys[gvrow.RowIndex]["State"].ToString().Trim(),
                    Country = gvBranchMaster.DataKeys[gvrow.RowIndex]["Country"].ToString().Trim(),
                    OperatingStatus = OperatingStatus.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("CM_InsBranchMstr", BranchMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindBranchMaster();
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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchMstr = new FA_BranchMaster()
                {
                    QueryType = "ReActive",
                    CorpId = gvBranchMaster.DataKeys[gvrow.RowIndex]["CorpId"].ToString().Trim(),
                    BranchCode = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchCode"].ToString().Trim(),
                    BranchId = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchId"].ToString().Trim(),
                    BranchName = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchName"].ToString().Trim(),
                    BranchType = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchType"].ToString().Trim(),
                    BranchRegion = gvBranchMaster.DataKeys[gvrow.RowIndex]["BranchRegion"].ToString().Trim(),
                    Address1 = gvBranchMaster.DataKeys[gvrow.RowIndex]["Address1"].ToString().Trim(),
                    Address2 = gvBranchMaster.DataKeys[gvrow.RowIndex]["Address2"].ToString().Trim(),
                    ZipCode = gvBranchMaster.DataKeys[gvrow.RowIndex]["Zipcode"].ToString().Trim(),
                    City = gvBranchMaster.DataKeys[gvrow.RowIndex]["City"].ToString().Trim(),
                    District = gvBranchMaster.DataKeys[gvrow.RowIndex]["District"].ToString().Trim(),
                    State = gvBranchMaster.DataKeys[gvrow.RowIndex]["State"].ToString().Trim(),
                    Country = gvBranchMaster.DataKeys[gvrow.RowIndex]["Country"].ToString().Trim(),
                    OperatingStatus = gvBranchMaster.DataKeys[gvrow.RowIndex]["OperatingStatus"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("CM_InsBranchMstr", BranchMstr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindBranchMaster();
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

    public void GetBranchType()
    {
        try
        {
            ddlBranchType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "GetConfigurationMaster",
                    ServiceType = "Type",
                    CorpId = "",
                    BranchCode = "",
                    Input1 = "6",
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
                        ddlBranchType.DataSource = dtExists;
                        ddlBranchType.DataValueField = "ConfigId";
                        ddlBranchType.DataTextField = "ConfigName";
                        ddlBranchType.DataBind();
                    }
                    else
                    {
                        ddlBranchType.DataBind();
                    }
                    ddlBranchType.Items.Insert(0, "Select Branch Type");
                    
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

    public void GetBranchRegion()
    {
        try
        {
            ddlBranchRegion.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "GetConfigurationMaster",
                    ServiceType = "Type",
                    CorpId = "",
                    BranchCode = "",
                    Input1 = "7",
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
                        ddlBranchRegion.DataSource = dtExists;
                        ddlBranchRegion.DataValueField = "ConfigId";
                        ddlBranchRegion.DataTextField = "ConfigName";
                        ddlBranchRegion.DataBind();
                    }
                    else
                    {
                        ddlBranchRegion.DataBind();
                    }
                    ddlBranchRegion.Items.Insert(0, "Select Branch Region");
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

    public void GetCountry()
    {
        try
        {
            ddlCountry.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "GetConfigurationMaster",
                    ServiceType = "Type",
                    CorpId = "",
                    BranchCode = "",
                    Input1 = "5",
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
                        ddlCountry.DataSource = dtExists;
                        ddlCountry.DataValueField = "ConfigId";
                        ddlCountry.DataTextField = "ConfigName";
                        ddlCountry.DataBind();
                    }
                    else
                    {
                        ddlCountry.DataBind();
                    }
                    ddlCountry.Items.Insert(0, "Select Country");
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

    public void ClearInputs()
    {
        txtBranchCode.Text = string.Empty;
        hfBranchId.Value = string.Empty;
        txtBranchName.Text = string.Empty;

        ddlBranchType.SelectedIndex = 0;
        ddlBranchRegion.SelectedIndex = 0;

        txtAddress1.Text = string.Empty;
        txtAddress2.Text = string.Empty;
        txtZipCode.Text = string.Empty;

        txtCity.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        txtState.Text = string.Empty;

        ddlCountry.SelectedIndex = 0;
    }

    public void BindBranchMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "GetBranchDetails",
                    ServiceType = "",
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
                        gvBranchMaster.DataSource = dtExists;
                        gvBranchMaster.DataBind();
                        lblGridMsg.Text = string.Empty;

                        divEntry.Visible = false;
                        divGrid.Visible = true;
                        lbtnNew.Visible = true;
                    }
                    else
                    {
                        gvBranchMaster.DataBind();
                        lblGridMsg.Text = "No Records Found";

                        divEntry.Visible = true;
                        divGrid.Visible = false;
                        lbtnNew.Visible = false;
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
    public class FA_BranchMaster
    {
        public string QueryType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchType { get; set; }
        public string BranchRegion { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string OperatingStatus { get; set; }
    }
    protected void lbtnOpStatus_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Common/BranchOperatingHistory.aspx",true);
    }
}