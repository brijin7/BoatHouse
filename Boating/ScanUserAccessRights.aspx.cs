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

public partial class Boating_ScanUserAccessRights : System.Web.UI.Page
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
                        UserId = ddlRower.SelectedValue.Trim(),
                        UserName = ddlRower.SelectedItem.Text,
                        BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                        BoatType = ddlBoatType.SelectedItem.Text,
                        SeaterId = SeaterIdList.Trim(),
                        SeaterName = SeaterList.ToString(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()

                    };
                    response = client.PostAsJsonAsync("ScanUserRights", RowerBoatAssign).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                        if (StatusCode == 1)
                        {
                            bindRowerBoatAssign();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            ClearInputs();
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
            getUser();
            //GetRowerBasedOnBoatType(ddlBoatType.SelectedValue);
        }
        else
        {
            chkSeaterType.ClearSelection();
            divseater.Visible = false;
            chkSelectAll.Checked = false;
            ddlRower.Items.Clear();
            ddlRower.Items.Insert(0, new ListItem("Select User", "0"));
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
            string id = gvScanUser.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim();
            hfUniqueId.Value = id.Trim();
            getUser();
            ddlRower.SelectedValue = gvScanUser.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim();
            GetBoatType();
            ddlBoatType.SelectedValue = gvScanUser.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim();
            GetBoatSeat();
            string btSeaterId = gvScanUser.DataKeys[gvrow.RowIndex]["SeaterId"].ToString().Trim();
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

            string btSeaterName = gvScanUser.DataKeys[gvrow.RowIndex]["SeaterName"].ToString().Trim();
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


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        string id = string.Empty;
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        string boatTypeId = gvScanUser.DataKeys[gvrow.RowIndex].Value.ToString();

        Label BoatType = (Label)gvrow.FindControl("lblRowerName");
        Label BoatHouseId = (Label)gvrow.FindControl("lblBoatType");
        Label BoatHouseName = (Label)gvrow.FindControl("lblSeaterType");

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var RowerBoatAssign = new BH_RowerBoatAssign()
            {
                QueryType = "ReActive",
                UniqueId = gvScanUser.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim(),
                UserId = gvScanUser.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim(),
                UserName = gvScanUser.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim(),
                BoatTypeId = gvScanUser.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim(),
                BoatType = gvScanUser.DataKeys[gvrow.RowIndex]["BoatType"].ToString().Trim(),
                SeaterId = gvScanUser.DataKeys[gvrow.RowIndex]["SeaterId"].ToString().Trim(),
                SeaterName = gvScanUser.DataKeys[gvrow.RowIndex]["SeaterName"].ToString().Trim(),
                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                CreatedBy = Session["UserId"].ToString().Trim()

            };

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("ScanUserRights", RowerBoatAssign).Result;

            if (response.IsSuccessStatusCode)
            {
                var submitresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                if (StatusCode == 1)
                {

                    bindRowerBoatAssign();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                }

            }
        }
    }

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {

        string id = string.Empty;
        LinkButton lnkbtn = sender as LinkButton;
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
                UniqueId = gvScanUser.DataKeys[gvrow.RowIndex]["UniqueId"].ToString().Trim(),
                UserId = gvScanUser.DataKeys[gvrow.RowIndex]["UserId"].ToString().Trim(),
                UserName = gvScanUser.DataKeys[gvrow.RowIndex]["UserName"].ToString().Trim(),
                BoatTypeId = gvScanUser.DataKeys[gvrow.RowIndex]["BoatTypeId"].ToString().Trim(),
                BoatType = gvScanUser.DataKeys[gvrow.RowIndex]["BoatType"].ToString().Trim(),
                SeaterId = gvScanUser.DataKeys[gvrow.RowIndex]["SeaterId"].ToString().Trim(),
                SeaterName = gvScanUser.DataKeys[gvrow.RowIndex]["SeaterName"].ToString().Trim(),
                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                CreatedBy = Session["UserId"].ToString().Trim()

            };

            HttpResponseMessage response;

            response = client.PostAsJsonAsync("ScanUserRights", RowerBoatAssign).Result;

            if (response.IsSuccessStatusCode)
            {
                var submitresponse = response.Content.ReadAsStringAsync().Result;
                int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                if (StatusCode == 1)
                {

                    bindRowerBoatAssign();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                }

            }
        }
    }

    protected void gvScanUser_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
        bindRowerBoatAssign();
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
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
        gvScanUser.PageIndex = e.NewPageIndex;
        bindRowerBoatAssign();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
    }

    public void getUser()
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
                HttpResponseMessage response = client.PostAsJsonAsync("ScanUserAccessRights/getusername", BoatHouseId).Result;

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
                        ddlRower.Items.Insert(0, new ListItem("Select User", "0"));

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

        ddlRower.Items.Clear();
        ddlRower.Items.Insert(0, new ListItem("Select User", "0"));

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
                    QueryType = "GetScanUserAccessRights",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
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
                        gvScanUser.DataSource = dtExists;
                        gvScanUser.DataBind();
                        lblGridMsg.Text = string.Empty;
                        divGrid.Visible = true;
                        gvScanUser.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;

                    }
                    else
                    {
                        gvScanUser.DataBind();
                        lblGridMsg.Text = "No Records Found";
                        divGrid.Visible = false;
                        gvScanUser.Visible = true;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
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
        public string UserId { get; set; }
        public string UserName { get; set; }
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
    }


}