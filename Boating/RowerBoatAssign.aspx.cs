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


public partial class Boating_RowerBoatAssign : System.Web.UI.Page
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
                GetBoatType();
                ViewState["hfstartvalue"] = "0";
                ViewState["hfendvalue"] = "0";
                int istart;
                int iend;
                AddProcess(0, 10, out istart, out iend);
                bindRowerBoatAssign();
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
            string qType = string.Empty;
            string id = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response;

                string SeaterIdList = string.Empty;
                string PselectedValues = String.Join(",",
                     chkSeaterType.Items.OfType<ListItem>().Where(r => r.Selected)
                    .Select(r => r.Value));
                SeaterIdList = PselectedValues;

                string SeaterList = string.Empty;
                string PselectedItems = String.Join(",",
                     chkSeaterType.Items.OfType<ListItem>().Where(r => r.Selected)
                    .Select(r => r.Text.Trim()));
                SeaterList = PselectedItems;

                //Newly Added By Imran on 05-11-2021
                string MultipleTripRights = string.Empty;
                if (ChkMultipleTrip.Checked == true)
                {
                    MultipleTripRights = "Y";
                }
                else
                {
                    MultipleTripRights = "N";
                }

                if (SeaterIdList == "" || SeaterIdList == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Atleast One Seater !');", true);
                    return;
                }
                else
                {
                    if (btnSubmit.Text.Trim() == "Submit")
                    {
                        qType = "Insert";
                        id = "0";
                    }

                    else
                    {
                        qType = "Update";
                        id = hfUniqueId.Value.Trim();
                    }

                    var RowerBoatAssign = new BH_RowerBoatAssign()
                    {
                        QueryType = qType.Trim(),
                        UniqueId = id.Trim(),
                        RowerId = ddlRower.SelectedValue.Trim(),
                        RowerName = ddlRower.SelectedItem.Text,
                        BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                        BoatType = ddlBoatType.SelectedItem.Text,
                        SeaterId = SeaterIdList.Trim(),
                        SeaterName = SeaterList.ToString(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim(),
                        MultipleTripRights = MultipleTripRights.Trim()

                    };
                    response = client.PostAsJsonAsync("RowerBoatAssign", RowerBoatAssign).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                        if (StatusCode == 1)
                        {
                            ViewState["hfstartvalue"] = "0";
                            ViewState["hfendvalue"] = "0";
                            int istart;
                            int iend;
                            AddProcess(0, 10, out istart, out iend);
                            BackToList.Visible = false;
                            back.Enabled = false;

                            //backSearch.Enabled = false;                        
                            bindRowerBoatAssign();
                            ClearInputs();
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

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlBoatType.SelectedValue != "0")
        {
            GetBoatSeat();
            GetRowerBasedOnBoatType(ddlBoatType.SelectedValue);
        }
        else
        {
            chkSeaterType.ClearSelection();
            divseater.Visible = false;
            chkSelectAll.Checked = false;
            ddlRower.Items.Clear();
            ddlRower.Items.Insert(0, new ListItem("Select Rower", "0"));
        }

    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string id = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
            hfUniqueId.Value = id.Trim();
            getRower();
            ddlRower.SelectedValue = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["RowerId"].ToString().Trim();
            GetBoatType();
            ddlBoatType.SelectedValue = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim();
            GetBoatSeat();
            string btSeaterId = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["SeaterId"].ToString().Trim();
            string[] sbtSeaterId = btSeaterId.Split(',');
            int btHouseIdCount = sbtSeaterId.Count();
            string CheckAllCount = ViewState["CheckCount"].ToString();
            for (int i = 0; i < btHouseIdCount; i++)
            {
                foreach (ListItem item in chkSeaterType.Items)
                {
                    string selectedValue = item.Value;
                    if (selectedValue == sbtSeaterId[i].ToString())
                    {
                        item.Selected = true;
                    }
                }
                if (btHouseIdCount == Convert.ToInt32(CheckAllCount))
                {
                    chkSelectAll.Checked = true;
                }
            }

            string btSeaterName = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["SeaterName"].ToString().Trim();
            string[] sbtSeaterName = btSeaterName.Split(',');
            int btHouseCount = sbtSeaterName.Count();
            for (int i = 0; i < btHouseCount; i++)
            {
                foreach (ListItem item in chkSeaterType.Items)
                {
                    string selectedText = item.Text;
                    if (selectedText == sbtSeaterName[i].ToString())
                    {
                        item.Selected = true;
                    }
                }
            }
            ddlBoatType.Enabled = false;
            ddlRower.Enabled = false;
            divseater.Visible = true;
            //Newly added by Imran on 05-11-2021

            string MultipleTripRights = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["MultiTripRights"].ToString().Trim();
            if (MultipleTripRights.ToString().Trim() == "Y")
            {
                ChkMultipleTrip.Checked = true;
            }
            else
            {
                ChkMultipleTrip.Checked = false;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {

        string id = string.Empty;
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;


        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var RowerBoatAssign = new BH_RowerBoatAssign()
            {
                QueryType = "Delete",
                UniqueId = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim(),
                RowerId = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["RowerId"].ToString().Trim(),
                RowerName = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["RowerName"].ToString().Trim(),
                BoatTypeId = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim(),
                BoatType = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["BoatType"].ToString().Trim(),
                SeaterId = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["SeaterId"].ToString().Trim(),
                SeaterName = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["SeaterName"].ToString().Trim(),
                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                CreatedBy = Session["UserId"].ToString().Trim(),
                MultipleTripRights = gvRowerBoatAssign.DataKeys[gvrow.RowIndex]["MultiTripRights"].ToString().Trim()

            };

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("RowerBoatAssign", RowerBoatAssign).Result;

            if (response.IsSuccessStatusCode)
            {
                var submitresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                if (StatusCode == 1)
                {
                    ClearInputs();
                    ViewState["hfstartvalue"] = "0";
                    ViewState["hfendvalue"] = "0";
                    int istart;
                    int iend;
                    back.Visible = true;
                    Next.Visible = true;
                    backSearch.Visible = false;
                    NextSearch.Visible = false;
                    BackToList.Visible = false;
                    back.Enabled = false;
                    AddProcess(0, 10, out istart, out iend);
                    bindRowerBoatAssign();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                }

            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
        ViewState["hfstartvalue"] = "0";
        ViewState["hfendvalue"] = "0";
        int istart;
        int iend;
        back.Visible = true;
        Next.Visible = true;
        backSearch.Visible = false;
        NextSearch.Visible = false;
        BackToList.Visible = false;
        back.Enabled = false;
        AddProcess(0, 10, out istart, out iend);
        bindRowerBoatAssign();
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        lblGridMsg.Visible = false;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        GetBoatType();
        ddlBoatType.Enabled = true;
        ddlRower.Enabled = true;
        lblSeatMsg.Visible = false;
        lblSeatMsg.Text = string.Empty;

    }

    protected void gvRowerBoatAssign_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRowerBoatAssign.PageIndex = e.NewPageIndex;
        bindRowerBoatAssign();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
    }

    public void getRower()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new BH_RowerBoatAssign()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("TripSheet/Rower", BoatHouseId).Result;

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
                            ddlRower.DataSource = dt;
                            ddlRower.DataValueField = "RowerId";
                            ddlRower.DataTextField = "RowerName";
                            ddlRower.DataBind();
                        }
                        else
                        {
                            ddlRower.DataBind();
                        }
                        ddlRower.Items.Insert(0, new ListItem("Select Rower", "0"));

                    }
                    else
                    {
                        return;
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

    public void GetRowerBasedOnBoatType(string sBoatTypeId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var FormBody = new BH_RowerBoatAssign()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = sBoatTypeId
                };
                HttpResponseMessage response = client.PostAsJsonAsync("RowerBoatAssign/GetRower", FormBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response1 = response.Content.ReadAsStringAsync().Result;
                    var ResponseMsg = JObject.Parse(Response1)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        ddlRower.DataSource = dtExists;
                        ddlRower.DataValueField = "RowerId";
                        ddlRower.DataTextField = "RowerName";
                        ddlRower.DataBind();
                    }
                    else
                    {
                        ddlRower.DataBind();
                    }
                    ddlRower.Items.Insert(0, new ListItem("Select Rower", "0"));
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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatRateMaster = new BH_RowerBoatAssign()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlBoatType", BoatRateMaster).Result;

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
                        ddlBoatType.Items.Insert(0, new ListItem("Select Boat Type", "0"));
                    }
                    else
                    {
                        return;
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

    public void GetBoatSeat()
    {
        chkSeaterType.Items.Clear();
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatRateMaster = new BH_RowerBoatAssign()
                {
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatRate/SeaterIn", BoatRateMaster).Result;

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
                            int CheckCount = dt.Rows.Count;
                            ViewState["CheckCount"] = CheckCount;
                            chkSeaterType.DataSource = dt;
                            chkSeaterType.DataValueField = "BoatSeaterId";
                            chkSeaterType.DataTextField = "SeaterType";
                            chkSeaterType.DataBind();
                            divseater.Visible = true;
                            lblSeatMsg.Visible = false;
                            lblSeatMsg.Text = string.Empty;

                        }
                        else
                        {
                            chkSeaterType.DataBind();
                            divseater.Visible = false;
                            lblSeatMsg.Visible = true;
                            lblSeatMsg.Text = "No Boat Seat Available.!";
                        }

                    }
                    else
                    {
                        chkSeaterType.DataBind();
                        divseater.Visible = false;
                        lblSeatMsg.Visible = true;
                        lblSeatMsg.Text = "No Boat Seat Available.!";
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

    public void ClearInputs()
    {

        ddlBoatType.SelectedIndex = 0;
        chkSeaterType.ClearSelection();
        divseater.Visible = false;
        chkSelectAll.Checked = false;
        ChkMultipleTrip.Checked = false;

        txtSearch.Text = string.Empty;
        ddlRower.Items.Clear();
        ddlRower.Items.Insert(0, new ListItem("Select Rower", "0"));

    }

    public void bindRowerBoatAssign()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new BH_RowerBoatAssign()
                {
                    QueryType = "GetRowerBoatAssignDtlsV2",
                    ServiceType = "",
                    Input1 = ViewState["hfstartvalue"].ToString().Trim(),
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RowerBoatAssignV2", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;

                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        if (dtExists.Rows.Count < 10)
                        {
                            Next.Enabled = false;
                            NextSearch.Enabled = false;

                        }
                        else
                        {
                            Next.Enabled = true;
                            NextSearch.Enabled = true;
                            backSearch.Enabled = false;

                        }
                        gvRowerBoatAssign.DataSource = dtExists;
                        gvRowerBoatAssign.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                        gvRowerBoatAssign.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                        Search.Visible = true;



                    }
                    else
                    {
                        gvRowerBoatAssign.DataBind();
                        lblGridMsg.Visible = true;
                        lblGridMsg.Text = "No Records Found";
                        gvRowerBoatAssign.Visible = false;
                        gvRowerBoatAssign.Visible = false;

                        lbtnNew.Visible = true;

                        Search.Visible = false;
                        NextSearch.Visible = false;
                        divEntry.Visible = false;

                        Next.Enabled = false;



                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public class BH_RowerBoatAssign
    {
        public string QueryType { get; set; }
        public string UniqueId { get; set; }
        public string RowerId { get; set; }
        public string RowerName { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatType { get; set; }
        public string SeaterId { get; set; }
        public string SeaterName { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string CreatedBy { get; set; }
        public string ActiveStatus { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
        public string ServiceType { get; set; }
        public string MultipleTripRights { get; set; }
    }


    protected void back_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcess(Int32.Parse(ViewState["hfstartvalue"].ToString().Trim()) - 10, Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) - 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        bindRowerBoatAssign();
    }

    protected void Next_Click(object sender, EventArgs e)
    {
        back.Enabled = true;
        int istart;
        int iend;
        AddProcess(Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) + 1, Int32.Parse(ViewState["hfendvalue"].ToString().Trim()) + 10, out istart, out iend);
        ViewState["hfstartvalue"] = istart.ToString();
        ViewState["hfendvalue"] = iend.ToString();
        bindRowerBoatAssign();
    }

    protected void BackToList_Click(object sender, EventArgs e)
    {
        back.Visible = true;
        back.Enabled = false;

        Next.Visible = true;
        BackToList.Visible = false;
        Search.Visible = true;
        txtSearch.Text = string.Empty;
        backSearch.Visible = false;
        NextSearch.Visible = false;
        int istart;
        int iend;
        AddProcess(0, 10, out istart, out iend);
        bindRowerBoatAssign();
    }

    protected void AddProcess(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;

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


    protected void AddProcessSearch(int start, int end, out int istart, out int iend)
    {
        if (start == 0)
        {
            istart = start + 1;

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
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
    }


    protected void subProcessSearch(int start, int end, out int istart, out int iend)
    {
        if (start == 1)
        {
            istart = start;
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
        }
        else
        {
            iend = end;

        }
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
    }
    public void bindRowerBoatAssignPinV2()
    {
        try
        {
            backSearch.Visible = true;
            NextSearch.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new BH_RowerBoatAssign()
                {
                    QueryType = "",
                    ServiceType = "",
                    Input1 = ViewState["hfSearchstartvalue"].ToString().Trim(),
                    Input2 = "",
                    Input3 = txtSearch.Text.Trim(),
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("RowerBoatAssignSearchV2", body).Result;

                if (response.IsSuccessStatusCode)
                {


                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;

                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        if (dtExists.Rows.Count < 10)
                        {
                            NextSearch.Enabled = false;

                        }
                        else
                        {
                            NextSearch.Enabled = true;
                            back.Enabled = false;


                        }
                        gvRowerBoatAssign.DataSource = dtExists;
                        gvRowerBoatAssign.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                        gvRowerBoatAssign.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                        Search.Visible = true;
                        backSearch.Visible = true;

                        //NextSearch.Enabled = true;


                    }
                    else
                    {
                        gvRowerBoatAssign.DataBind();
                        lblGridMsg.Visible = true;
                        lblGridMsg.Text = "No Records Found";
                        gvRowerBoatAssign.Visible = false;
                        gvRowerBoatAssign.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = true;
                        Search.Visible = false;
                        NextSearch.Visible = false;
                        divEntry.Visible = false;
                        backSearch.Visible = false;


                    }
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

        back.Visible = false;
        Next.Visible = false;
        BackToList.Visible = true;
        ViewState["hfSearchstartvalue"] = "0";
        ViewState["hfSearchendvalue"] = "0";
        int istart;
        int iend;
        backSearch.Enabled = false;
        AddProcessSearch(0, 10, out istart, out iend);
        bindRowerBoatAssignPinV2();
    }

    protected void backSearch_Click(object sender, EventArgs e)
    {
        int istart;
        int iend;
        subProcessSearch(Int32.Parse(ViewState["hfSearchstartvalue"].ToString().Trim()) - 10, Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) - 10, out istart, out iend);
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();

        bindRowerBoatAssignPinV2();
        if (istart == 1)
        {
            backSearch.Enabled = false;
        }
        else
        {
            backSearch.Enabled = true;
        }
    }

    protected void NextSearch_Click(object sender, EventArgs e)
    {
        backSearch.Enabled = true;
        int istart;
        int iend;
        AddProcessSearch(Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) + 1, Int32.Parse(ViewState["hfSearchendvalue"].ToString().Trim()) + 10, out istart, out iend);
        ViewState["hfSearchstartvalue"] = istart.ToString();
        ViewState["hfSearchendvalue"] = iend.ToString();
        bindRowerBoatAssignPinV2();


    }
}