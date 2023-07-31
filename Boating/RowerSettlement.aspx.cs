using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Helpers;

/// <summary>
/// Modified By:Pretheka.C
/// Modified Date:13th Oct 2021
/// </summary>
public partial class Boating_RowerSettlement : System.Web.UI.Page
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
                txtTripDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                getRower();
                BindRowerSummary();
                divSettleAmt.Attributes.Add("style", "display:none;");
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
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

                var BoatHouseId = new RowerSettlement()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    TripDate = txtTripDate.Text.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("ddlRower/RowerSettle", BoatHouseId).Result;

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
                        ddlRower.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlRower.DataBind();
                        ddlRower.Items.Insert(0, new ListItem("All", "0"));
                    }
                }
                else
                {
                    ddlRower.DataBind();
                    ddlRower.Items.Insert(0, new ListItem("All", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindRowerSummary()
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

                var pRowerSettlement = new RowerSettlement()
                {
                    RowerId = ddlRower.SelectedValue.Trim(),
                    TripDate = txtTripDate.Text,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                };

                response = client.PostAsJsonAsync("RowerSettlementSummary", pRowerSettlement).Result;

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
                            lblmsgSummary.Text = "";
                            GVRowerSummary.DataSource = dt;
                            GVRowerSummary.DataBind();
                            divSummary.Visible = true;
                            divEntry.Visible = true;
                            divSettle.Visible = false;
                            GVRowerSummary.Visible = true;
                            MpeMaterial.Dispose();
                        }
                        else
                        {
                            lblmsgSummary.Text = "";
                            divEntry.Visible = true;
                            divSettle.Visible = false;
                        }
                    }
                    else
                    {
                        lblmsgSummary.Text = ResponseMsg;
                        //divGrid.Visible = false;
                        //divSummary.Visible = false;
                        GVRowerSummary.Visible = false;
                        MpeMaterial.Dispose();
                        divEntry.Visible = true;
                        divSettle.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        // lbtnNew.Visible = false;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void txtTripDate_TextChanged(object sender, EventArgs e)
    {
        getRower();
        divSummary.Visible = false;
        divGrid.Visible = false;

    }


    public void bindRowerSettledGrid()
    {
        if (rblSettlement.SelectedValue == "TS")
        {
            rblSettlement.SelectedValue = "S";
        }
        else
        {

        }
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
                //ViewState["TripDate"] = Date;
                var pRowerSettlement = new RowerSettlement()
                {
                    RowerId = ddlRower.SelectedValue.Trim(),
                    FromDate = txtTripDate.Text,
                    ToDate = txtTripDate.Text,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("RowerSettledGrid", pRowerSettlement).Result;

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
                            gvRowerSettled.DataSource = dt;
                            gvRowerSettled.DataBind();
                            lblPayGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divSummary.Visible = false;
                            divEntry.Visible = true;
                            divSettle.Visible = false;
                            //lbtnNew.Visible = true;

                            decimal sum = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                sum += decimal.Parse(dt.Rows[i]["SettlementAmt"].ToString());
                            }

                            gvRowerSettled.FooterRow.Cells[3].Text = "Total";
                            gvRowerSettled.FooterRow.Cells[3].Font.Bold = true;
                            gvRowerSettled.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                            gvRowerSettled.FooterRow.Cells[3].Font.Size = 20;

                            gvRowerSettled.FooterRow.Cells[4].Text = sum.ToString();
                            gvRowerSettled.FooterRow.Cells[4].ForeColor = Color.Green;
                            gvRowerSettled.FooterRow.Cells[4].Font.Bold = true;
                            gvRowerSettled.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            gvRowerSettled.FooterRow.Cells[4].Font.Size = 20;
                        }
                        else
                        {
                            lblPayGridMsg.Text = "No Records Found";
                            divGrid.Visible = true;
                            divSummary.Visible = false;
                            divEntry.Visible = true;
                            divSettle.Visible = false;
                            //lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        lblPayGridMsg.Text = ResponseMsg;
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        divSettle.Visible = false;
                        //lbtnNew.Visible = false;
                        ////BindRowerSummary();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    private void bindSettlement()
    {
        try
        {

            if (Convert.ToDecimal(txtTotal.Text) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Select Any One Rower to Settle !');", true);
                return;
            }
            else
            {
                ViewState["TripDate"] = "";
                ViewState["RowerId"] = "";
                ViewState["RowerName"] = "";
                ViewState["Trips"] = "";
                ViewState["ToatlAmt"] = "";
                ViewState["SettleAmt"] = "";
                ViewState["BalanceAmt"] = "";

                foreach (GridViewRow item in GVRowerSummary.Rows)
                {
                    CheckBox chk = (CheckBox)GVRowerSummary.Rows[item.RowIndex].Cells[0].FindControl("chkSettle");
                    if (chk.Checked)
                    {
                        string TripDate = GVRowerSummary.DataKeys[item.RowIndex]["TripDate"].ToString().Trim();
                        ViewState["TripDate"] += TripDate.Trim() + '~';

                        string RowerId = GVRowerSummary.DataKeys[item.RowIndex]["RowerId"].ToString().Trim();
                        ViewState["RowerId"] += RowerId.Trim() + '~';


                        string TripCount = GVRowerSummary.DataKeys[item.RowIndex]["TripCount"].ToString().Trim();
                        ViewState["TripCount"] += TripCount.Trim() + '~';

                        string BalanceCharge = GVRowerSummary.DataKeys[item.RowIndex]["BalanceCharge"].ToString().Trim();
                        ViewState["BalanceCharge"] += BalanceCharge.Trim() + '~';

                        string Rowername = GVRowerSummary.DataKeys[item.RowIndex]["RowerName"].ToString().Trim();
                        ViewState["RowerName"] += Rowername.Trim();



                    }
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
        ddlRower.SelectedIndex = 0;
        txtTripDate.Text = string.Empty;
        divSummary.Visible = false;
        divEntry.Visible = false;
        divSettle.Visible = false;
        divGrid.Visible = true;
        txtTripDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        MpeMaterial.Dispose();
        if (rblSettlement.SelectedValue == "TS")
        {
            BindRowerSummary();
        }
        else
        {

            bindRowerSettledGrid();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearInputs();
        //bindRowerSettledGrid(txtTripDate.Text);
        bindRowerSettledGrid();
    }

    protected void lbtnTripModal_Click(object sender, EventArgs e)
    {
        MpeMaterial.Dispose();
        gvBoatRowerSettle.PageIndex = 0;
        LinkButton lbtnSettle = sender as LinkButton;
        GridViewRow gvrow = lbtnSettle.NamingContainer as GridViewRow;
        Label RowerId = (Label)gvrow.FindControl("lblRowerId");
        Label TripDate = (Label)gvrow.FindControl("lblTripDate");
        ViewState["RowerId"] = RowerId.Text.Trim();
        ViewState["TripDate"] = TripDate.Text.Trim();
        BindModalPageIndex();
    }

    public void BindModalPageIndex()
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
                string tripDate = ViewState["TripDate"].ToString().Trim();

                var pRowerSettlement = new RowerSettlement()
                {
                    RowerId = ViewState["RowerId"].ToString().Trim(),
                    TripDate = tripDate.ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("RowerSettlement/SettlementAmount", pRowerSettlement).Result;

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
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

                            gvBoatRowerSettle.DataSource = dt;
                            gvBoatRowerSettle.DataBind();
                            gvBoatRowerSettle.Visible = true;
                            divEntry.Visible = true;
                            divSummary.Visible = true;
                            divSettle.Visible = true;

                            decimal totSettleAmt = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("SettlementAmt")));

                            gvBoatRowerSettle.FooterRow.Cells[9].Text = "Total";
                            gvBoatRowerSettle.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;

                            gvBoatRowerSettle.FooterRow.Cells[10].Text = totSettleAmt.ToString("N2");
                            gvBoatRowerSettle.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;

                            hfActRowerCharge.Value = dt.Rows[0]["SettlementAmt"].ToString();
                            lblRowerModal.Text = dt.Rows[0]["RowerName"].ToString();
                        }
                        else
                        {
                            divEntry.Visible = true;
                            divSummary.Visible = false;
                            divSettle.Visible = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Trip has been settled');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnSettlement_Click(object sender, EventArgs e)
    {
        try
        {
            bindSettlement();
            string[] sResult = ViewState["TripDate"].ToString().Split('~');
            string[] sResult1 = ViewState["RowerId"].ToString().Split('~');

            string Rower = ViewState["RowerName"].ToString().Trim();



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                string sMSG = string.Empty;

                var pRowerSettlement = new RowerSettlement()
                {
                    QueryType = "Pay",
                    RowerId = ViewState["RowerId"].ToString().Trim(),
                    TripDate = ViewState["TripDate"].ToString().Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                response = client.PostAsJsonAsync("RowerSettlement", pRowerSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Response = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Response)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Response)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                        bindRowerSettledGrid();
                        // bindRowerSettledGrid(txtTripDate.Text);
                        string[] sSettlementId = ResponseMsg.Split('-');
                        ViewState["Responsemsg"] = sSettlementId[1].ToString().Trim();
                        BindRedirect();
                        ClearInputs();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    public void BindRedirect()
    {

        string sResult1 = ViewState["Responsemsg"].ToString();
        string RowerName = ViewState["RowerName"].ToString();
        Response.Redirect("PrintRower.aspx?SId=" + sResult1.ToString() + "&BId=" + Session["BoatHouseId"].ToString() + "");
    }


    protected void lblSettlementAmt_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;


        Label SettlementId = (Label)gvrow.FindControl("lblSettlementId");
        ViewState["SettlementId"] = SettlementId.Text;
        string SetlId = ViewState["SettlementId"].ToString().Trim();

        Label RowerName = (Label)gvrow.FindControl("lblRowerName");
        ViewState["RowerName"] = RowerName.Text;
        string Rown = ViewState["RowerName"].ToString().Trim();

        Label SettlementDate = (Label)gvrow.FindControl("lblSettlementDate");
        ViewState["SettlementDate"] = SettlementDate.Text;
        string SetlDate = ViewState["SettlementDate"].ToString().Trim();



        Response.Redirect("PrintRower.aspx?SId=" + SetlId.Trim() + "&BId=" + Session["BoatHouseId"].ToString() + "");


    }

    protected void GVRowerSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToDouble(((Label)e.Row.FindControl("lblBalanceCharge")).Text) == 0.00)
                {
                    CheckBox btnEdit = (CheckBox)e.Row.FindControl("chkSettle");
                    btnEdit.Visible = false;

                    LinkButton lbtnTripModal = (LinkButton)e.Row.FindControl("lbtnTripModal");
                    lbtnTripModal.Enabled = false;
                }

                else
                {
                    CheckBox btnEdit = (CheckBox)e.Row.FindControl("chkSettle");
                    btnEdit.Visible = true;
                    LinkButton lbtnTripModal = (LinkButton)e.Row.FindControl("lbtnTripModal");
                    lbtnTripModal.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void GVRowerSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVRowerSummary.PageIndex = e.NewPageIndex;
        BindRowerSummary();
    }

    protected void gvBoatRowerSettle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBoatRowerSettle.PageIndex = e.NewPageIndex;
        BindModalPageIndex();
    }

    protected void gvRowerSettled_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRowerSettled.PageIndex = e.NewPageIndex;
        //bindRowerSettledGrid(ViewState["TripDate"].ToString());
        bindRowerSettledGrid();
    }

    protected void imgCloseTicket_Click(object sender, ImageClickEventArgs e)
    {
        MpeMaterial.Hide();
    }

    public class RowerSettlement
    {
        public string QueryType { get; set; }
        public string RowerId { get; set; }
        public string TripDate { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string CreatedBy { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SettlementId { get; set; }

    }

    //protected void rblSettlement_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (rblSettlement.SelectedValue == "TS")
    //    {
    //        BindRowerSummary();
    //        divSummary.Visible = true;
    //        divGrid.Visible = false;
    //        divEntry.Visible = true;
    //        ddlRower.SelectedIndex = 0;
    //        txtTripDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

    //    }
    //    else
    //    {
    //        bindRowerSettledGrid(txtTripDate.Text);
    //        ClearInputs();
    //    }
    //}

    protected void ddlRower_SelectedIndexChanged(object sender, EventArgs e)
    {
        divSummary.Visible = false;
        divGrid.Visible = false;
    }

    protected void rblSettlement_SelectedIndexChanged(object sender, EventArgs e)
    {
        MpeMaterial.Dispose();
        divSummary.Visible = false;
        divGrid.Visible = false;
    }

}