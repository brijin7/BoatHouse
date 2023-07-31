using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BoatHouseMaster : System.Web.UI.Page
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
                GetCorporateOffice();
                GetLocation();
                getBoatHouse();
                GethouseManager();
                BindBoatHouse();
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public void GetLocation()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ddlLocation").Result;

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

                            ddlLocation.DataSource = dt;
                            ddlLocation.DataValueField = "ConfigId";
                            ddlLocation.DataTextField = "ConfigName";
                            ddlLocation.DataBind();

                        }
                        else
                        {

                            ddlLocation.DataBind();
                        }
                        ddlLocation.Items.Insert(0, new ListItem("Select Location", "0"));
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
    /// <summary>
    /// Included corporate office 13 jul 2023
    /// </summary>
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
                var BranchType = new CommonClass()
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
    public void getBoatHouseAll()
    {
        ddlBoatHouse.Items.Clear();
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "SingleBranch",
                    ServiceType = "",
                    CorpId = "",
                    Input1 = "3",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlBoatHouse.DataSource = dtExists;
                        ddlBoatHouse.DataValueField = "BranchId";
                        ddlBoatHouse.DataTextField = "BranchName";
                        ddlBoatHouse.DataBind();
                    }
                    else
                    {
                        ddlBoatHouse.DataBind();
                    }
                    ddlBoatHouse.Items.Insert(0, "Select Boat House");
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

    public void getBoatHouse()
    {
        ddlBoatHouse.Items.Clear();
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "SingleBranchNotIn",
                    ServiceType = "",
                    CorpId = "",
                    Input1 = "3",
                    Input2 = ddlCorporateOff.SelectedValue.Trim(),
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlBoatHouse.DataSource = dtExists;
                        ddlBoatHouse.DataValueField = "BranchId";
                        ddlBoatHouse.DataTextField = "BranchName";
                        ddlBoatHouse.DataBind();
                    }
                    else
                    {
                        ddlBoatHouse.DataBind();
                    }
                    ddlBoatHouse.Items.Insert(0, "Select Boat House");
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

    public void GethouseManager()
    {
        try
        {
            ddlHouseManager.Items.Clear();

            ddlHouseManager.Items.Insert(0, new ListItem("Select Boat House Manager ", "0"));
            ddlHouseManager.Items.Insert(1, new ListItem("Admin", "1"));

            //using (var client = new HttpClient())
            //{

            //    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Accept.Clear();

            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = client.GetAsync("ddlEmpBoatHouse").Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var Getresponse = response.Content.ReadAsStringAsync().Result;
            //        int StatusCode = Convert.ToInt32(JObject.Parse(Getresponse)["StatusCode"].ToString());
            //        string ResponseMsg = JObject.Parse(Getresponse)["Response"].ToString();
            //        if (StatusCode == 1)
            //        {
            //            DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
            //            if (dt.Rows.Count > 0)
            //            {
            //                ddlHouseManager.DataSource = dt;
            //                ddlHouseManager.DataValueField = "EmpId";
            //                ddlHouseManager.DataTextField = "EmpName";
            //                ddlHouseManager.DataBind();
            //            }
            //            else
            //            {

            //                ddlHouseManager.DataBind();
            //            }
            //            ddlHouseManager.Items.Insert(0, new ListItem("Select Boat House Manager ", "0"));
            //        }
            //        else
            //        {
            //            lblGridMsg.Text = ResponseMsg.ToString().Trim();
            //        }
            //    }
            //    else
            //    {
            //        var Errorresponse = response.Content.ReadAsStringAsync().Result;

            //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
            //    }
            //}
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (chkWorkingDays.Items[0].Selected == false && chkWorkingDays.Items[1].Selected == false && chkWorkingDays.Items[2].Selected == false
           && chkWorkingDays.Items[3].Selected == false && chkWorkingDays.Items[4].Selected == false && chkWorkingDays.Items[5].Selected == false
           && chkWorkingDays.Items[6].Selected == false)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Working Days!!!!');", true);
            return;
        }

        try
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < chkWorkingDays.Items.Count; i++)
            {
                if (chkWorkingDays.Items[i].Selected)
                {
                    string str = chkWorkingDays.Items[i].ToString();
                    string bindwd = string.Empty;
                    if (str == "Sunday")
                    {
                        bindwd = "1";
                    }
                    else if (str == "Monday")
                    {
                        bindwd = "2";
                    }
                    else if (str == "Tuesday")
                    {
                        bindwd = "3";
                    }
                    else if (str == "Wednesday")
                    {
                        bindwd = "4";
                    }
                    else if (str == "Thursday")
                    {
                        bindwd = "5";
                    }
                    else if (str == "Friday")
                    {
                        bindwd = "6";
                    }
                    else if (str == "Saturday")
                    {
                        bindwd = "7";
                    }
                    Session["Workingdays"] = sb.Append(bindwd);
                }
            }

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (txtMaxChildage.Text == "" || txtMaxChildage.Text == null)
                {
                    txtMaxChildage.Text = "0";
                }
                if (txtMaxInfa.Text == "" || txtMaxInfa.Text == null)
                {
                    txtMaxInfa.Text = "0";
                }
                if (txtaddress2.Text == "")
                {
                    txtaddress2.Text = hfAddres2.Value.Trim();
                }
                if (txtAddress1.Text == "")
                {
                    txtAddress1.Text = hfAddres1.Value.Trim();
                }

                HttpResponseMessage response;
                string sMSG = string.Empty;
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var BoatHouse = new boatHouse()
                    {

                        QueryType = "Insert",
                        CorpId = ddlCorporateOff.SelectedValue.Trim(),
                        BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                        BoatHouseName = ddlBoatHouse.SelectedItem.Text.Trim(),
                        BoatLocnId = ddlLocation.SelectedValue.Trim(),
                        BoatHouseManager = ddlHouseManager.SelectedValue.Trim(),
                        Address1 = txtAddress1.Text,
                        Address2 = txtaddress2.Text,
                        City = txtCity.Text,
                        District = txtDistrict.Text,
                        State = txtState.Text,
                        ZipCode = txtZipcode.Text,
                        MaxChildAge = txtMaxChildage.Text,
                        MaxInfantAge = txtMaxInfa.Text,
                        BookingFrom = txtBookingFrm.Text,
                        BookingTo = txtBookngTo.Text,
                        MailId = txtEmailId.Text,
                        GSTNumber = txtGSTNumber.Text.Trim(),
                        WorkingDays = Session["Workingdays"].ToString(),
                        TripStartAlertTime = "5",
                        TripEndAlertTime = "5",
                        CreatedBy = hfCreatedBy.Value.Trim(),
                        ReprintTime = "30",
                        RefundDuration = "60",
                        AutoEndForNoDeposite = "N",
                        QRcodeGenerate = "Y",
                        BHShortCode = " ",
                        ExtensionPrint = "Y",
                        ExtnChargeStatus = "Y"
                    };

                    response = client.PostAsJsonAsync("BoatHouseMstr", BoatHouse).Result;
                }
                else
                {

                    var BoatHouse = new boatHouse()
                    {
                        QueryType = "Update",
                        CorpId = ddlCorporateOff.SelectedValue.Trim(),
                        BoatHouseId = ddlBoatHouse.SelectedValue.Trim(),
                        BoatHouseName = ddlBoatHouse.SelectedItem.Text.Trim(),
                        BoatLocnId = ddlLocation.SelectedValue.Trim(),
                        BoatHouseManager = ddlHouseManager.SelectedValue.Trim(),
                        Address1 = txtAddress1.Text,
                        Address2 = txtaddress2.Text,
                        City = txtCity.Text,
                        District = txtDistrict.Text,
                        State = txtState.Text,
                        ZipCode = txtZipcode.Text,
                        MaxChildAge = txtMaxChildage.Text,
                        MaxInfantAge = txtMaxInfa.Text,
                        BookingFrom = txtBookingFrm.Text,
                        BookingTo = txtBookngTo.Text,
                        MailId = txtEmailId.Text,
                        GSTNumber = txtGSTNumber.Text.Trim(),
                        WorkingDays = Session["Workingdays"].ToString(),
                        TripStartAlertTime = hfTripStartAlertTime.Value.Trim(),
                        TripEndAlertTime = hfTripEndAlertTime.Value.Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim(),
                        ReprintTime = hfReprintTime.Value.Trim(),
                        RefundDuration = hfRefundDuration.Value.Trim(),
                        AutoEndForNoDeposite = hfAutoEndForNoDeposite.Value.Trim(),
                        QRcodeGenerate = hfQRcodeGenerate.Value.Trim(),
                        BHShortCode = hfBHShortCode.Value.Trim(),
                        ExtensionPrint = hfExtensionPrint.Value.Trim(),
                        ExtnChargeStatus = hfExtnChargeStatus.Value.Trim()
                    };

                    response = client.PostAsJsonAsync("BoatHouseMstr", BoatHouse).Result;

                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindBoatHouse();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        divEntry.Visible = false;
                        divGrid.Visible = true;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        divEntry.Visible = true;
                        divGrid.Visible = false;
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
        ddlHouseManager.SelectedIndex = 0;
        ddlBoatHouse.SelectedIndex = -1;
        ddlLocation.SelectedIndex = 0;
        txtBookingFrm.Text = string.Empty;
        txtBookngTo.Text = string.Empty;
        txtAddress1.Text = string.Empty;
        txtaddress2.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtZipcode.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        txtState.Text = string.Empty;
        txtMaxChildage.Text = string.Empty;
        txtMaxInfa.Text = string.Empty;
        chkWorkingDays.Items[1].Selected = false;
        chkWorkingDays.Items[2].Selected = false;
        chkWorkingDays.Items[3].Selected = false;
        chkWorkingDays.Items[4].Selected = false;
        chkWorkingDays.Items[5].Selected = false;
        chkWorkingDays.Items[6].Selected = false;
        chkWorkingDays.Items[0].Selected = false;
        chkSelectAll.Checked = false;
        txtEmailId.Text = string.Empty;
        ChkChildApplicable.Checked = false;
        txtGSTNumber.Text = "";
    }

    public void BindBoatHouse()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("BoatHouseMstr/ListAll").Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatHouseresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatHouseresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatHouseresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvBoatHouse.DataSource = dt;
                            gvBoatHouse.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;

                        }
                        else
                        {
                            gvBoatHouse.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            divGrid.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;
                        }

                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        divGrid.Visible = false;
                        divEntry.Visible = true;
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        getBoatHouse();
        divEntry.Visible = false;
        divGrid.Visible = true;
        lbtnNew.Visible = true;
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            divAddrs1.Visible = true;
            divAddrs2.Visible = true;
            divCityDisSta.Visible = true;
            divEmafrmTo.Visible = true;
            divWrkngdays.Visible = true;
            divMaxMinChild.Visible = true;

            divGrid.Visible = false;
            divEntry.Visible = true;
            lbtnNew.Visible = false;
            mapview.Visible = true;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvBoatHouse.DataKeys[gvrow.RowIndex].Value.ToString();
            Label CorpId = (Label)gvrow.FindControl("lblCorpId");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label BoatLocnId = (Label)gvrow.FindControl("lblBoatLocnId");
            Label BoatHouseMngId = (Label)gvrow.FindControl("lblBoatHouseMngId");
            Label BookingFrom = (Label)gvrow.FindControl("lblBookingFrom");
            Label BookingTo = (Label)gvrow.FindControl("lblBookingTo");
            Label WorkingDays = (Label)gvrow.FindControl("lblWorkingDays");
            Label Address1 = (Label)gvrow.FindControl("lblAddress1");
            Label Address2 = (Label)gvrow.FindControl("lblAddress2");
            Label City = (Label)gvrow.FindControl("lblCity");
            Label District = (Label)gvrow.FindControl("lblDistrict");
            Label State = (Label)gvrow.FindControl("lblState");
            Label ZipCode = (Label)gvrow.FindControl("lblZipCode");
            Label mailid = (Label)gvrow.FindControl("lblmailid");
            Label MaxChildAge = (Label)gvrow.FindControl("lblMaxChildAge");
            Label MaxInfantAge = (Label)gvrow.FindControl("lblMaxInfantAge");
            Label TaxId = (Label)gvrow.FindControl("lblTaxID");
            Label GSTNumber = (Label)gvrow.FindControl("lblGSTNumber");

            Label lblTripStartAlertTime = (Label)gvrow.FindControl("lblTripStartAlertTime");
            Label lblTripEndAlertTime = (Label)gvrow.FindControl("lblTripEndAlertTime");
            Label lblReprintTime = (Label)gvrow.FindControl("lblReprintTime");
            Label lblRefundDuration = (Label)gvrow.FindControl("lblRefundDuration");
            Label lblAutoEndForNoDeposite = (Label)gvrow.FindControl("lblAutoEndForNoDeposite");
            Label lblQRcodeGenerate = (Label)gvrow.FindControl("lblQRcodeGenerate");
            Label lblBHShortCode = (Label)gvrow.FindControl("lblBHShortCode");
            Label lblExtensionPrint = (Label)gvrow.FindControl("lblExtensionPrint");
            Label lblExtnChargeStatus = (Label)gvrow.FindControl("lblExtnChargeStatus");


            getBoatHouseAll();
            ddlCorporateOff.SelectedValue = CorpId.Text.Trim();
            ddlBoatHouse.SelectedValue = BoatHouseId.Text.Trim();
            ddlLocation.SelectedValue = Convert.ToInt32(BoatLocnId.Text).ToString();
            ddlHouseManager.SelectedValue = Convert.ToInt32(BoatHouseMngId.Text).ToString();
            txtBookingFrm.Text = BookingFrom.Text;
            txtBookngTo.Text = BookingTo.Text;
            txtAddress1.Text = Address1.Text;
            txtaddress2.Text = Address2.Text;
            txtCity.Text = City.Text;
            txtDistrict.Text = District.Text;
            txtState.Text = State.Text;
            txtZipcode.Text = ZipCode.Text;
            txtEmailId.Text = mailid.Text;
            txtGSTNumber.Text = GSTNumber.Text.Trim();
            txtMaxChildage.Text = MaxChildAge.Text;
            txtMaxInfa.Text = MaxInfantAge.Text;
            ddlLocation.Enabled = false;
            ddlBoatHouse.Enabled = false;
            ddlCorporateOff.Enabled = false;


            hfTripStartAlertTime.Value = lblTripStartAlertTime.Text.Trim();
            hfTripEndAlertTime.Value = lblTripEndAlertTime.Text.Trim();
            hfReprintTime.Value = lblReprintTime.Text.Trim();
            hfRefundDuration.Value = lblRefundDuration.Text.Trim();
            hfAutoEndForNoDeposite.Value = lblAutoEndForNoDeposite.Text.Trim();
            hfQRcodeGenerate.Value = lblQRcodeGenerate.Text.Trim();
            hfBHShortCode.Value = lblBHShortCode.Text.Trim();
            hfExtensionPrint.Value = lblExtensionPrint.Text.Trim();
            hfExtnChargeStatus.Value = lblExtnChargeStatus.Text.Trim();


            string str = WorkingDays.Text;
            char[] Split = str.ToCharArray();

            for (int i = 0; i < Split.Count(); i++)
            {
                string split = Split[i].ToString();
                int a = Convert.ToInt16(split);

                if (a == 1)
                {
                    chkWorkingDays.Items[0].Selected = true;
                }
                else if (a == 2)
                {
                    chkWorkingDays.Items[1].Selected = true;
                }
                else if (a == 3)
                {
                    chkWorkingDays.Items[2].Selected = true;
                }
                else if (a == 4)
                {
                    chkWorkingDays.Items[3].Selected = true;
                }
                else if (a == 5)
                {
                    chkWorkingDays.Items[4].Selected = true;
                }
                else if (a == 6)
                {
                    chkWorkingDays.Items[5].Selected = true;
                }
                else if (a == 7)
                {
                    chkWorkingDays.Items[6].Selected = true;
                }
            }

            if (Convert.ToInt32(Split.Count()) > 6)
            {
                chkSelectAll.Checked = true;
            }

            if (txtMaxChildage.Text == "")
            {
                txtMaxChildage.Text = "0";
            }
            if (txtMaxInfa.Text == "")
            {
                txtMaxInfa.Text = "0";
            }

            if (txtMaxChildage.Text != "" || txtMaxChildage.Text != null)
            {
                divChild.Visible = true;
                ChkChildApplicable.Checked = true;
            }
            if (txtMaxInfa.Text != "" || txtMaxInfa.Text != null)
            {
                divInfant.Visible = true;
                ChkChildApplicable.Checked = true;
            }
            if (txtMaxChildage.Text == "" || txtMaxChildage.Text == null)
            {
                divChild.Visible = false;
                ChkChildApplicable.Checked = false;
            }
            if (txtMaxInfa.Text == "" || txtMaxInfa.Text == null)
            {
                divInfant.Visible = false;
                ChkChildApplicable.Checked = false;
            }
            if (txtMaxChildage.Text == "0" && txtMaxInfa.Text == "0")
            {
                ChkChildApplicable.Checked = false;
                divInfant.Visible = false;
                divChild.Visible = false;
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
            string sTesfg = gvBoatHouse.DataKeys[gvrow.RowIndex].Value.ToString();
            Label CorpId = (Label)gvrow.FindControl("lblCorpId");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseMaster1 = new boatHouse()
                {
                    QueryType = "Delete",
                    CorpId = CorpId.Text,
                    BoatHouseId = BoatHouseId.Text,
                    BoatHouseName = BoatHouseName.Text,
                    BoatLocnId = "1",
                    BoatHouseManager = "8",
                    Address1 = "Adr1",
                    Address2 = "Adr2",
                    City = "BHosur",
                    District = "Krishnagiri",
                    State = "TN",
                    ZipCode = "632598",
                    MaxChildAge = "0",
                    MaxInfantAge = "0",
                    BookingFrom = "02-06-2020",
                    BookingTo = "03-09-2020",
                    MailId = "Mail123@gmil.bm",
                    GSTNumber = "0",
                    WorkingDays = "123456",
                    CreatedBy = hfCreatedBy.Value.Trim(),
                    ReprintTime = "30",
                    TripStartAlertTime = "5",
                    TripEndAlertTime = "5",
                    RefundDuration = "60",
                    AutoEndForNoDeposite = "N",
                    QRcodeGenerate = "Y",
                    BHShortCode = " ",
                    ExtensionPrint = "Y",
                    ExtnChargeStatus = "Y"
                };

                HttpResponseMessage response;

                response = client.PostAsJsonAsync("BoatHouseMstr", BoatHouseMaster1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatHouse();
                        Clear();
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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ChkChildApplicable_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkChildApplicable.Checked)
        {
            divChild.Visible = true; divInfant.Visible = true;
        }
        else if (!ChkChildApplicable.Checked)
        {
            divChild.Visible = false; divInfant.Visible = false;
        }
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        getBoatHouse();
        Clear();
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        ddlLocation.Enabled = true;
        ddlBoatHouse.Enabled = true;
        ddlCorporateOff.Enabled = true;
        divAddrs1.Visible = false;
        divAddrs2.Visible = false;
        divCityDisSta.Visible = false;
        divEmafrmTo.Visible = false;
        divWrkngdays.Visible = false;
        divMaxMinChild.Visible = false;
        mapview.Visible = false;

    }

    protected void gvBoatHouse_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {

        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvBoatHouse.DataKeys[gvrow.RowIndex].Value.ToString();
            Label CorpId = (Label)gvrow.FindControl("lblCorpId");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseMaster1 = new boatHouse()
                {
                    QueryType = "ReActive",
                    CorpId = CorpId.Text,
                    BoatHouseId = BoatHouseId.Text,
                    BoatHouseName = BoatHouseName.Text,
                    BoatLocnId = "1",
                    BoatHouseManager = "8",
                    Address1 = "Adr1",
                    Address2 = "Adr2",
                    City = "BHosur",
                    District = "Krishnagiri",
                    State = "TN",
                    ZipCode = "632598",
                    MaxChildAge = "0",
                    MaxInfantAge = "0",
                    BookingFrom = "02-06-2020",
                    BookingTo = "03-09-2020",
                    MailId = "Mail123@gmil.bm",
                    WorkingDays = "123456",
                    ReprintTime = "30",
                    GSTNumber = "0",
                    TripStartAlertTime = "5",
                    TripEndAlertTime = "5",
                    RefundDuration = "60",
                    CreatedBy = hfCreatedBy.Value.Trim(),
                    AutoEndForNoDeposite = "N",
                    QRcodeGenerate = "Y",
                    BHShortCode = " ",
                    ExtensionPrint = "Y",
                    ExtnChargeStatus = "Y"


                };

                HttpResponseMessage response;

                response = client.PostAsJsonAsync("BoatHouseMstr", BoatHouseMaster1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatHouse();
                        Clear();
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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void txtZipcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtZipcode.Text == "" || txtZipcode.Text == null)
            {
                divAddrs1.Visible = false;
                divAddrs2.Visible = false;
                divCityDisSta.Visible = false;
                divEmafrmTo.Visible = false;
                divWrkngdays.Visible = false;
                divMaxMinChild.Visible = false;
                mapview.Visible = false;

                return;

            }
            using (var client = new HttpClient())
            {
                string ZipURL = "http://www.postalpincode.in/api/pincode/";
                client.BaseAddress = new Uri(ZipURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("http://www.postalpincode.in/api/pincode/" + txtZipcode.Text + "").Result;
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
                            //   txtPostalCode.Value = txtZipcode.Text.Trim();
                            txtCity.Text = dt.Rows[0]["Circle"].ToString();
                            txtDistrict.Text = dt.Rows[0]["District"].ToString();
                            txtState.Text = dt.Rows[0]["State"].ToString();
                            divAddrs1.Visible = true;
                            divAddrs2.Visible = true;
                            divCityDisSta.Visible = true;
                            divEmafrmTo.Visible = true;
                            divWrkngdays.Visible = true;
                            divMaxMinChild.Visible = true;
                            mapview.Visible = true;
                            txtAddress1.Text = "";
                            txtaddress2.Text = "";

                        }
                        else
                        {
                            txtCity.Text = "";
                            txtDistrict.Text = "";
                            txtState.Text = "";
                            txtAddress1.Text = "";
                            txtaddress2.Text = "";
                            divAddrs1.Visible = false;
                            divAddrs2.Visible = false;
                            divCityDisSta.Visible = false;
                            divEmafrmTo.Visible = false;
                            divWrkngdays.Visible = false;
                            divMaxMinChild.Visible = false;
                            mapview.Visible = false;
                        }

                    }
                    else
                    {
                        txtCity.Text = "";
                        txtDistrict.Text = "";
                        txtState.Text = "";
                        txtAddress1.Text = "";
                        txtaddress2.Text = "";
                        divAddrs1.Visible = false;
                        divAddrs2.Visible = false;
                        divCityDisSta.Visible = false;
                        divEmafrmTo.Visible = false;
                        divWrkngdays.Visible = false;
                        divMaxMinChild.Visible = false;
                        mapview.Visible = false;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetMapData()
    {
        try
        {
            using (var client = new HttpClient())
            {
                string ZipURL = "http://www.postalpincode.in/api/pincode/";
                client.BaseAddress = new Uri(ZipURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("http://www.postalpincode.in/api/pincode/" + txtZipcode.Text + "").Result;

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
                            //   txtPostalCode.Value = txtZipcode.Text.Trim();
                            txtCity.Text = dt.Rows[0]["Circle"].ToString();
                            txtDistrict.Text = dt.Rows[0]["District"].ToString();
                            txtState.Text = dt.Rows[0]["State"].ToString();

                        }
                        else
                        {
                        }

                    }
                    else
                    {

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

    protected void txtBookngTo_TextChanged(object sender, EventArgs e)
    {
        if (txtBookingFrm.Text == null || txtBookingFrm.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Booking From');", true);
            txtBookngTo.Text = string.Empty;
            return;
        }
        DateTime startTime = DateTime.Parse(txtBookingFrm.Text);
        DateTime endTime = DateTime.Parse(txtBookngTo.Text);
        if (endTime <= startTime)
        {
            txtBookngTo.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Booking To should be Greater than Booking From ');", true);

        }
        else
        {
            return;
        }
    }

    protected void txtBookingFrm_TextChanged(object sender, EventArgs e)
    {
        DateTime startTime = DateTime.Parse(txtBookingFrm.Text);
        DateTime endTime = DateTime.Parse(txtBookngTo.Text);
    }

    protected void ddlCorporateOff_SelectedIndexChanged(object sender, EventArgs e)
    {
        getBoatHouse();
    }

    protected void ddlBoatHouse_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public class boatHouse
    {
        public string QueryType { get; set; }
        public string CorpId { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BoatLocnId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string BoatHouseManager { get; set; }
        public string BookingFrom { get; set; }
        public string BookingTo { get; set; }
        public string WorkingDays { get; set; }
        public string MailId { get; set; }
        public string MaxChildAge { get; set; }
        public string MaxInfantAge { get; set; }
        public string TaxId { get; set; }
        public string CreatedBy { get; set; }
        public string ActiveStatus { get; set; }
        public string GSTNumber { get; set; }
        public string TripStartAlertTime { get; set; }
        public string TripEndAlertTime { get; set; }
        public string ReprintTime { get; set; }
        public string RefundDuration { get; set; }
        public string AutoEndForNoDeposite { get; set; }
        public string QRcodeGenerate { get; set; }
        public string BHShortCode { get; set; }
        public string ExtensionPrint { get; set; }
        public string ExtnChargeStatus { get; set; }


    }
    public class CommonClass
    {
        public string BoatHouseId
        {
            get;
            set;
        }
        public string QueryType
        {
            get;
            set;
        }
        public string BoatTypeId
        {
            get;
            set;
        }
        public string CorpId
        {
            get;
            set;
        }
        public string BoatSeaterId
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
        public string Input1
        {
            get;
            set;
        }
        public string Input2
        {
            get;
            set;
        }
        public string Input3
        {
            get;
            set;
        }
        public string Input4
        {
            get;
            set;
        }
        public string Input5
        {
            get;
            set;
        }
        public string ServiceType
        {
            get;
            set;
        }
    }

}