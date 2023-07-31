using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_BoatSeatMaster : System.Web.UI.Page
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

                BindSeatBoatMaster();
                GetBoatHouse();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    protected void gvBoatMaster_RowDataBound(Object sender, GridViewRowEventArgs e)
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

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divgrid.Visible = true;
            divEntry.Visible = true;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvBoatMaster.DataKeys[gvrow.RowIndex].Value.ToString();

            Label boatSeaterId = (Label)gvrow.FindControl("lblboatSeaterId");
            Label SeaterType = (Label)gvrow.FindControl("lblSeaterType");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label NoOfSeats = (Label)gvrow.FindControl("lblNoOfSeats");
            Label AllowedNoOfSeats = (Label)gvrow.FindControl("lblAllowedNoOfSeats");
            Label RestrictionReason = (Label)gvrow.FindControl("lblRestrictionReason");

            txtSeaterType.Text = SeaterType.Text;
            txtNoofseats.Text = NoOfSeats.Text;
            ddlBoatHouseId.SelectedValue = BoatHouseId.Text;
            ddlBoatHouseId.SelectedItem.Text = BoatHouseName.Text;
            Session["boatSeaterId"] = boatSeaterId.Text;
            txtAllowedSeats.Text = AllowedNoOfSeats.Text;
            txtRestrictionReason.Text = RestrictionReason.Text;

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
            string sTesfg = gvBoatMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label boatSeaterId = (Label)gvrow.FindControl("lblboatSeaterId");
            Label SeaterType = (Label)gvrow.FindControl("lblSeaterType");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label NoOfSeats = (Label)gvrow.FindControl("lblNoOfSeats");
            Label AllowedNoOfSeats = (Label)gvrow.FindControl("lblAllowedNoOfSeats");
            Label RestrictionReason = (Label)gvrow.FindControl("lblRestrictionReason");

            Session["boatSeaterId"] = boatSeaterId.Text;
            string abc = Session["boatSeaterId"].ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Boatseatmaster = new Boatseatmaster()
                {
                    QueryType = "Delete",
                    BoatSeaterId = boatSeaterId.Text,
                    SeaterType = SeaterType.Text,
                    BoatHouseId = BoatHouseId.Text,
                    BoatHouseName = BoatHouseName.Text,
                    NoOfSeats = NoOfSeats.Text,
                    AllowedNoOfSeats = AllowedNoOfSeats.Text,
                    RestrictionReason = RestrictionReason.Text,
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("BoatSeatMstr", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindSeatBoatMaster();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        BindSeatBoatMaster();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindSeatBoatMaster();
                    clearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
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
            string sTesfg = gvBoatMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label boatSeaterId = (Label)gvrow.FindControl("lblboatSeaterId");
            Label SeaterType = (Label)gvrow.FindControl("lblSeaterType");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label NoOfSeats = (Label)gvrow.FindControl("lblNoOfSeats");
            Label AllowedNoOfSeats = (Label)gvrow.FindControl("lblAllowedNoOfSeats");
            Label RestrictionReason = (Label)gvrow.FindControl("lblRestrictionReason");

            Session["boatSeaterId"] = boatSeaterId.Text;
            string abc = Session["boatSeaterId"].ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Boatseatmaster = new Boatseatmaster()
                {
                    QueryType = "ReActive",
                    BoatSeaterId = boatSeaterId.Text,
                    SeaterType = SeaterType.Text,
                    BoatHouseId = BoatHouseId.Text,
                    BoatHouseName = BoatHouseName.Text,
                    NoOfSeats = NoOfSeats.Text,
                    AllowedNoOfSeats = AllowedNoOfSeats.Text,
                    RestrictionReason = RestrictionReason.Text,
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("BoatSeatMstr", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindSeatBoatMaster();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        BindSeatBoatMaster();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindSeatBoatMaster();
                    clearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtAllowedSeats.Text != "")
            {
                if (Convert.ToInt32(txtAllowedSeats.Text) >= Convert.ToInt32(txtNoofseats.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Allowed Seats Shoud Be Less Than Number of Seats.');", true);
                    return;
                }
            }
            if (txtAllowedSeats.Text != "" && txtRestrictionReason.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Reason for Restriction');", true);
                return;
            }

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
                    var Boatseatmaster = new Boatseatmaster()
                    {
                        QueryType = "Insert",
                        BoatSeaterId = "0",
                        SeaterType = txtSeaterType.Text.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        NoOfSeats = txtNoofseats.Text.Trim(),
                        AllowedNoOfSeats = txtAllowedSeats.Text.Trim(),
                        RestrictionReason = txtRestrictionReason.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("BoatSeatMstr", Boatseatmaster).Result;
                    sMSG = "Boat Seat Details Inserted Successfully";
                }
                else
                {
                    var Boatseatmaster = new Boatseatmaster()
                    {
                        QueryType = "Update",
                        BoatSeaterId = Session["boatSeaterId"].ToString(),
                        SeaterType = txtSeaterType.Text.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        NoOfSeats = txtNoofseats.Text.Trim(),
                        AllowedNoOfSeats = txtAllowedSeats.Text.Trim(),
                        RestrictionReason = txtRestrictionReason.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("BoatSeatMstr", Boatseatmaster).Result;
                    sMSG = "Boat Seat Details Updated Successfully";
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            BindSeatBoatMaster();
                            clearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divgrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else if (btnSubmit.Text.Trim() == "Update")
                        {
                            BindSeatBoatMaster();
                            clearInputs();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divgrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            BindSeatBoatMaster();
                            clearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg + "');", true);
                            divgrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        divgrid.Visible = true;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
                    }
                }
                else
                {
                    BindSeatBoatMaster();
                    clearInputs();
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
        clearInputs();
        divgrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divgrid.Visible = true;
        divEntry.Visible = true;
        lbtnNew.Visible = false;
        clearInputs();
        btnSubmit.Text = "Submit";
    }

    public void BindSeatBoatMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new Boatseatmaster()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatSeaterMstr/BHId", BoatHouseId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Response)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Response)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            gvBoatMaster.DataSource = dt;
                            gvBoatMaster.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divgrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            gvBoatMaster.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divgrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
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
                    lblGridMsg.Text = response.ToString();
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
                var BoatHouseId = new Boatseatmaster()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("ddlBoatHouse/BHID", BoatHouseId).Result;

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
                            ddlBoatHouseId.DataSource = dt;
                            ddlBoatHouseId.DataValueField = "BoatHouseId";
                            ddlBoatHouseId.DataTextField = "BoatHouseName";
                            ddlBoatHouseId.DataBind();
                        }
                        else
                        {
                            ddlBoatHouseId.DataBind();
                        }
                        ddlBoatHouseId.Items.Insert(0, new ListItem("Select Boat House", "0"));
                    }
                    else
                    {
                        ddlBoatHouseId.DataBind();
                        ddlBoatHouseId.Items.Insert(0, new ListItem("Select Boat House", "0"));
                    }
                }
                else
                {
                    ddlBoatHouseId.DataBind();
                    ddlBoatHouseId.Items.Insert(0, new ListItem("Select Boat House", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void clearInputs()
    {
        txtNoofseats.Text = string.Empty;
        ddlBoatHouseId.SelectedIndex = 0;
        txtSeaterType.Text = string.Empty;
        txtAllowedSeats.Text = string.Empty;
        txtRestrictionReason.Text = string.Empty;
        btnSubmit.Text = "Submit";
    }

    public class Boatseatmaster
    {
        public string QueryType { get; set; }
        public string BoatSeaterId { get; set; }
        public string SeaterType { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string NoOfSeats { get; set; }
        public string CreatedBy { get; set; }
        public string AllowedNoOfSeats { get; set; }
        public string RestrictionReason { get; set; }
    }
}
