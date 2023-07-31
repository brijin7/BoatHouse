using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_RptTripSheetSummary : System.Web.UI.Page
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
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                //GetBoatType();
                // BindTripSheetSummary();
                BindNoOfTrips();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }

    //public void GetBoatType()
    //{
    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            var BoatHouseId = new RptTripSheetSummary()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim()
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("BoatType/BoatRateMstr", BoatHouseId).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        ddlBoatType.DataSource = dt;
    //                        ddlBoatType.DataValueField = "BoatTypeId";
    //                        ddlBoatType.DataTextField = "BoatType";
    //                        ddlBoatType.DataBind();
    //                    }
    //                    else
    //                    {
    //                        ddlBoatType.DataBind();
    //                    }
    //                    ddlBoatType.Items.Insert(0, new ListItem("All", "0"));
    //                }
    //            }
    //            else
    //            {
    //                var Errorresponse = response.Content.ReadAsStringAsync().Result;
    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}

    //public void GetBoatSeaterListByBoatType()
    //{
    //    try
    //    {
    //        ddlBoatSeater.Items.Clear();
    //        ddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));

    //        if (ddlBoatType.SelectedIndex == 0)
    //        {
    //            ddlBoatType.SelectedValue = "0";
    //            return;
    //        }

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            var boatmaster = new RptTripSheetSummary()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BoatTypeId = ddlBoatType.SelectedValue.Trim()
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("BoatSeat/BoatRateMstr", boatmaster).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        ddlBoatSeater.DataSource = dt;
    //                        ddlBoatSeater.DataValueField = "BoatSeaterId";
    //                        ddlBoatSeater.DataTextField = "SeaterType";
    //                        ddlBoatSeater.DataBind();
    //                    }
    //                    else
    //                    {
    //                        ddlBoatSeater.DataBind();
    //                    }
    //                    ddlBoatSeater.Items.Insert(0, new ListItem("All", "0"));
    //                }
    //            }
    //            else
    //            {
    //                var Errorresponse = response.Content.ReadAsStringAsync().Result;
    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}

    //public void GetBoatList()
    //{
    //    try
    //    {
    //        ddlBoat.Items.Clear();
    //        ddlBoat.Items.Insert(0, new ListItem("All", "0"));

    //        if (ddlBoatType.SelectedIndex == 0)
    //        {
    //            ddlBoatType.SelectedValue = "0";
    //            ddlBoatSeater.SelectedValue = "0";
    //            return;
    //        }
    //        else if (ddlBoatSeater.SelectedIndex == 0)
    //        {
    //            ddlBoatSeater.SelectedValue = "0";
    //            return;
    //        }

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();

    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            var boatmaster = new RptTripSheetSummary()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                BoatTypeId = ddlBoatType.SelectedValue.Trim(),
    //                BoatSeaterId = ddlBoatSeater.SelectedValue.Trim()
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("ddlBoatMaster/BHId", boatmaster).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        ddlBoat.DataSource = dt;
    //                        ddlBoat.DataValueField = "BoatId";
    //                        ddlBoat.DataTextField = "BoatName";
    //                        ddlBoat.DataBind();
    //                    }
    //                    else
    //                    {
    //                        ddlBoat.DataBind();
    //                    }
    //                    ddlBoat.Items.Insert(0, new ListItem("All", "0"));
    //                }
    //            }
    //            else
    //            {
    //                var Errorresponse = response.Content.ReadAsStringAsync().Result;
    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}

    //public void BindTripSheetSummary()
    //{
    //    try
    //    {
    //        string BoatSeaterId = "";
    //        if (ddlBoatSeater.SelectedValue == "")
    //        {
    //            BoatSeaterId = "0";
    //        }
    //        else
    //        {
    //            BoatSeaterId = ddlBoatSeater.SelectedValue.Trim();
    //        }
    //        string BoatId = "";
    //        if (ddlBoat.SelectedValue == "")
    //        {
    //            BoatId = "0";
    //        }
    //        else
    //        {
    //            BoatId = ddlBoat.SelectedValue.Trim();
    //        }
    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //            var BoatSearch = new RptTripSheetSummary()
    //            {
    //                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
    //                FromDate = txtFromDate.Text.Trim(),
    //                ToDate = txtToDate.Text.Trim(),
    //                DepositType = ddlDepositType.SelectedValue.Trim(),
    //                BoatTypeId = ddlBoatType.SelectedValue.Trim(),
    //                BoatSeaterId = BoatSeaterId,
    //                BoatId = BoatId,
    //                UserRole = Session["UserRole"].ToString().Trim(),
    //                UserId = Session["UserId"].ToString().Trim()
    //            };

    //            HttpResponseMessage response = client.PostAsJsonAsync("RptTripSheetSummary", BoatSearch).Result;

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var gvList = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(gvList)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(gvList)["Response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        if (ddlDepositType.SelectedValue == "Y")
    //                        {
    //                            gvTripSheetSummary.Columns[13].HeaderText = "Refund Amount";
    //                        }
    //                        else if (ddlDepositType.SelectedValue == "N")
    //                        {
    //                            gvTripSheetSummary.Columns[13].HeaderText = "Non Refund Amount";
    //                        }
    //                        else
    //                        {
    //                            gvTripSheetSummary.Columns[13].HeaderText = "Refund Amount";
    //                        }
    //                        gvTripSheetSummary.DataSource = dt;
    //                        gvTripSheetSummary.DataBind();
    //                        gvTripSheetSummary.Visible = true;
    //                        lblGridMsg.Visible = false;


    //                        //decimal total = 0;
    //                        //foreach (GridViewRow gvr in gvTripSheetSummary.Rows)
    //                        //{
    //                        //    Label tb = (Label)gvr.Cells[1].FindControl("lblDepRefundAmount");
    //                        //    decimal sum;
    //                        //    if (decimal.TryParse(tb.Text.Trim(), out sum))
    //                        //    {
    //                        //        total += sum;
    //                        //    }
    //                        //}
    //                        decimal BillAmount = 0;
    //                        for (int i = 0; i < dt.Rows.Count; i++)
    //                        {
    //                            string F = dt.Rows[i].ToString();
    //                            BillAmount += decimal.Parse(dt.Rows[i]["InitNetAmount"].ToString());
    //                        }

    //                        decimal sum = 0;
    //                        for (int i = 0; i < dt.Rows.Count; i++)
    //                        {
    //                            string F = dt.Rows[i].ToString();
    //                            sum += decimal.Parse(dt.Rows[i]["DepRefundAmount"].ToString());
    //                        }

    //                        gvTripSheetSummary.FooterRow.Cells[13].Text = sum.ToString();
    //                        gvTripSheetSummary.FooterRow.Cells[13].ForeColor = Color.Green;
    //                        gvTripSheetSummary.FooterRow.Cells[13].Font.Bold = true;
    //                        gvTripSheetSummary.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
    //                        gvTripSheetSummary.FooterRow.Cells[13].Font.Size = 20;

    //                        gvTripSheetSummary.FooterRow.Cells[6].Text = "Total";
    //                        gvTripSheetSummary.FooterRow.Cells[6].Font.Bold = true;
    //                        gvTripSheetSummary.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;
    //                        gvTripSheetSummary.FooterRow.Cells[6].Font.Size = 20;

    //                        gvTripSheetSummary.FooterRow.Cells[7].Text = BillAmount.ToString();
    //                        gvTripSheetSummary.FooterRow.Cells[7].ForeColor = Color.Green;
    //                        gvTripSheetSummary.FooterRow.Cells[7].Font.Bold = true;
    //                        gvTripSheetSummary.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
    //                        gvTripSheetSummary.FooterRow.Cells[7].Font.Size = 20;

    //                    }
    //                    else
    //                    {
    //                        gvTripSheetSummary.DataBind();
    //                        lblGridMsg.Visible = true;
    //                        lblGridMsg.Text = ResponseMsg.ToString();

    //                    }
    //                }
    //                else
    //                {
    //                    gvTripSheetSummary.DataBind();
    //                    lblGridMsg.Visible = true;
    //                    lblGridMsg.Text = ResponseMsg.ToString();

    //                }
    //            }
    //            else
    //            {
    //                gvTripSheetSummary.DataBind();
    //                lblGridMsg.Visible = true;
    //                lblGridMsg.Text = response.ReasonPhrase.ToString();

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }

    //}


    protected void gvTripSheetSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTripSheetSummary.PageIndex = e.NewPageIndex;

        bindTripSheet(ViewState["BoatTypeId"].ToString(), ViewState["BoatSeaterId"].ToString());
        //ViewState["BoatTypeId"] = "";
        //ViewState["BoatSeaterId"] = "";

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //BindTripSheetSummary();
        BindNoOfTrips();
        divGridList.Visible = false;
        gvTripSheetSummary.Visible = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        //ddlDepositType.SelectedValue = "0";
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtFromDate.Attributes.Add("readonly", "readonly");
        txtToDate.Attributes.Add("readonly", "readonly");
        //GetBoatType();
        //BindTripSheetSummary();
        divGridList.Visible = false;
        gvTripSheetSummary.Visible = false;
        BindNoOfTrips();

    }

    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetBoatSeaterListByBoatType();
        //GetBoatList();
        //BindTripSheetSummary();
        BindNoOfTrips();
    }

    protected void ddlBoatSeater_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetBoatList();
        //BindTripSheetSummary();
        BindNoOfTrips();
    }

    protected void gvNoOfTrips_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNoOfTrips.PageIndex = e.NewPageIndex;
        BindNoOfTrips();
    }

    public void BindNoOfTrips()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var vTripSheetSettlement = new RptTripSheetSummary();

                if (Session["UserRole"].ToString().ToLower().Trim() == "user")
                {
                    vTripSheetSettlement = new RptTripSheetSummary()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        QueryType = "TripSheetSummaryDetails",
                        ServiceType = "",
                        BookingId = "",
                        Input1 = "",
                        Input2 = "",
                        Input3 = "",
                        Input4 = Session["UserId"].ToString().Trim(),
                        Input5 = Session["UserRole"].ToString().ToLower().Trim()
                    };
                }
                else
                {
                    vTripSheetSettlement = new RptTripSheetSummary()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        QueryType = "TripSheetSummaryDetails",
                        ServiceType = "",
                        BookingId = "",
                        Input1 = "",
                        Input2 = "",
                        Input3 = "",
                        Input4 = "",
                        Input5 = ""
                    };
                }
                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var GetResponse = response.Content.ReadAsStringAsync().Result;
                    if (GetResponse.Contains("No Records Found"))
                    {
                        divNoOfTrips.Visible = true;
                        gvNoOfTrips.Visible = false;
                        btnPDF.Visible = false;
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + GetResponse + "');", true);
                        return;
                    }
                    else
                    {
                        var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                        var ResponseMsgCnt = JObject.Parse(GetResponse)["Table1"].ToString();
                        DataTable dtCnt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgCnt);

                        if (dt.Rows.Count > 0)
                        {
                            btnPDF.Visible = true;
                            gvNoOfTrips.DataSource = dt;
                            gvNoOfTrips.DataBind();
                            gvNoOfTrips.Visible = true;
                            lblGridMsgNoOfTrips.Visible = false;

                            decimal total = 0;
                            foreach (GridViewRow gvr in gvNoOfTrips.Rows)
                            {
                                LinkButton tb = (LinkButton)gvr.Cells[1].FindControl("lblNoOfTrips");
                                decimal sum;
                                if (decimal.TryParse(tb.Text.Trim(), out sum))
                                {
                                    total += sum;
                                }
                            }

                            decimal reftotal = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                reftotal += decimal.Parse(dt.Rows[i]["ClaimedDeposit"].ToString());
                            }

                            decimal Collection = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                Collection += decimal.Parse(dt.Rows[i]["CollectionAmt"].ToString());
                            }

                            decimal UnClaimedDep = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                UnClaimedDep += decimal.Parse(dt.Rows[i]["UnClaimedDeposit"].ToString());
                            }

                            decimal ExtentedAmt = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                ExtentedAmt += decimal.Parse(dt.Rows[i]["ExtendedAmt"].ToString());
                            }

                            decimal RowerAmt = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                RowerAmt += decimal.Parse(dt.Rows[i]["Roweramt"].ToString());
                            }

                            decimal RowerSettle = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                RowerSettle += decimal.Parse(dt.Rows[i]["RowerSettlementAmt"].ToString());
                            }

                            gvNoOfTrips.FooterRow.Cells[2].Text = "Total No. Of Trips";
                            gvNoOfTrips.FooterRow.Cells[2].Font.Bold = true;
                            gvNoOfTrips.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                            gvNoOfTrips.FooterRow.Cells[3].Text = total.ToString();
                            gvNoOfTrips.FooterRow.Cells[3].ForeColor = Color.Green;
                            gvNoOfTrips.FooterRow.Cells[3].Font.Bold = true;
                            gvNoOfTrips.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                            //gvNoOfTrips.FooterRow.Cells[3].Font.Size = 20;
                            gvNoOfTrips.FooterRow.Cells[4].Text = Collection.ToString();
                            gvNoOfTrips.FooterRow.Cells[4].ForeColor = Color.Green;
                            gvNoOfTrips.FooterRow.Cells[4].Font.Bold = true;
                            gvNoOfTrips.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                            gvNoOfTrips.FooterRow.Cells[5].Text = reftotal.ToString();
                            gvNoOfTrips.FooterRow.Cells[5].ForeColor = Color.Green;
                            gvNoOfTrips.FooterRow.Cells[5].Font.Bold = true;
                            gvNoOfTrips.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;

                            gvNoOfTrips.FooterRow.Cells[6].Text = UnClaimedDep.ToString();
                            gvNoOfTrips.FooterRow.Cells[6].ForeColor = Color.Green;
                            gvNoOfTrips.FooterRow.Cells[6].Font.Bold = true;
                            gvNoOfTrips.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                            gvNoOfTrips.FooterRow.Cells[7].Text = ExtentedAmt.ToString();
                            gvNoOfTrips.FooterRow.Cells[7].ForeColor = Color.Green;
                            gvNoOfTrips.FooterRow.Cells[7].Font.Bold = true;
                            gvNoOfTrips.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                            gvNoOfTrips.FooterRow.Cells[8].Text = RowerAmt.ToString();
                            gvNoOfTrips.FooterRow.Cells[8].ForeColor = Color.Green;
                            gvNoOfTrips.FooterRow.Cells[8].Font.Bold = true;
                            gvNoOfTrips.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;

                            gvNoOfTrips.FooterRow.Cells[9].Text = RowerSettle.ToString();
                            gvNoOfTrips.FooterRow.Cells[9].ForeColor = Color.Green;
                            gvNoOfTrips.FooterRow.Cells[9].Font.Bold = true;
                            gvNoOfTrips.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            gvNoOfTrips.DataBind();
                            lblGridMsgNoOfTrips.Visible = true;
                            lblGridMsgNoOfTrips.Text = "No Records Found !!!";
                            btnPDF.Visible = false;
                            divNoOfTrips.Visible = true;
                            gvNoOfTrips.Visible = false;

                        }
                    }
                }
                else
                {
                    gvNoOfTrips.DataBind();
                    lblGridMsgNoOfTrips.Visible = true;
                    lblGridMsgNoOfTrips.Text = "No Records Found !!!";
                    btnPDF.Visible = false;

                }

            }
        }
        catch (Exception ex)
        {
            btnPDF.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }
    protected void lblNoOfTrips_Click(object sender, EventArgs e)
    {
        ViewState["BoatTypeId"] = "";
        ViewState["BoatSeaterId"] = "";
        LinkButton lbtn = sender as LinkButton;
        GridViewRow row = lbtn.NamingContainer as GridViewRow;

        bindTripSheet(gvNoOfTrips.DataKeys[row.RowIndex]["BoatTypeId"].ToString().Trim(), gvNoOfTrips.DataKeys[row.RowIndex]["BoatSeaterId"].ToString().Trim());
        ViewState["BoatTypeId"] = gvNoOfTrips.DataKeys[row.RowIndex]["BoatTypeId"].ToString().Trim();
        ViewState["BoatSeaterId"] = gvNoOfTrips.DataKeys[row.RowIndex]["BoatSeaterId"].ToString().Trim();


    }
    public void bindTripSheet(string sBoattypeid, string sBoatseaterid)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new RptTripSheetSummary();
                if (Session["UserRole"].ToString().ToLower().Trim() == "user")
                {
                    vTripSheetSettlement = new RptTripSheetSummary()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        QueryType = "TripSheetSummaryBasedonType",
                        ServiceType = "",
                        BookingId = "",
                        Input1 = sBoattypeid,
                        Input2 = sBoatseaterid,
                        Input3 = "",
                        Input4 = Session["UserId"].ToString().Trim(),
                        Input5 = Session["UserRole"].ToString().ToLower().Trim()
                    };
                }
                else
                {
                    vTripSheetSettlement = new RptTripSheetSummary()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        QueryType = "TripSheetSummaryBasedonType",
                        ServiceType = "",
                        BookingId = "",
                        Input1 = sBoattypeid,
                        Input2 = sBoatseaterid,
                        Input3 = "",
                        Input4 = "",
                        Input5 = ""
                    };
                }

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var GetResponse = response.Content.ReadAsStringAsync().Result;
                    if (GetResponse.Contains("No Records Found"))
                    {
                        divGridList.Visible = true;
                        gvTripSheetSummary.Visible = false;
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + GetResponse + "');", true);
                        return;
                    }
                    else
                    {
                        var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                        if (dt.Rows.Count > 0)
                        {
                            //if (ddlDepositType.SelectedValue == "Y")
                            //{
                            //    gvTripSheetSummary.Columns[13].HeaderText = "Refund Amount";
                            //}
                            //else if (ddlDepositType.SelectedValue == "N")
                            //{
                            //    gvTripSheetSummary.Columns[13].HeaderText = "Non Refund Amount";
                            //}
                            //else
                            //{
                            //    gvTripSheetSummary.Columns[13].HeaderText = "Refund Amount";
                            //}
                            gvTripSheetSummary.DataSource = dt;
                            gvTripSheetSummary.DataBind();
                            divGridList.Visible = true;
                            gvTripSheetSummary.Visible = true;
                            lblGridMsg.Visible = false;



                            decimal BillAmount = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                BillAmount += decimal.Parse(dt.Rows[i]["InitNetAmount"].ToString());
                            }

                            decimal sum = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                sum += decimal.Parse(dt.Rows[i]["DepRefundAmount"].ToString());
                            }
                            decimal boatCharge = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BoatCharge")));
                            decimal boatDeposite = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("BoatDeposit")));

                            gvTripSheetSummary.FooterRow.Cells[6].Text = boatCharge.ToString();
                            gvTripSheetSummary.FooterRow.Cells[6].ForeColor = Color.Green;
                            gvTripSheetSummary.FooterRow.Cells[6].Font.Bold = true;
                            gvTripSheetSummary.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            gvTripSheetSummary.FooterRow.Cells[6].Font.Size = 20;
                            hfBoatCharge.Value = boatCharge.ToString().Trim();

                            gvTripSheetSummary.FooterRow.Cells[7].Text = boatDeposite.ToString();
                            gvTripSheetSummary.FooterRow.Cells[7].ForeColor = Color.Green;
                            gvTripSheetSummary.FooterRow.Cells[7].Font.Bold = true;
                            gvTripSheetSummary.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            gvTripSheetSummary.FooterRow.Cells[7].Font.Size = 20;
                            hfBoatDeposite.Value = boatDeposite.ToString().Trim();

                            gvTripSheetSummary.FooterRow.Cells[8].Text = BillAmount.ToString();
                            gvTripSheetSummary.FooterRow.Cells[8].ForeColor = Color.Green;
                            gvTripSheetSummary.FooterRow.Cells[8].Font.Bold = true;
                            gvTripSheetSummary.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            gvTripSheetSummary.FooterRow.Cells[8].Font.Size = 20;
                            hfBillAmount.Value = BillAmount.ToString();

                            gvTripSheetSummary.FooterRow.Cells[12].Text = sum.ToString();
                            gvTripSheetSummary.FooterRow.Cells[12].ForeColor = Color.Green;
                            gvTripSheetSummary.FooterRow.Cells[12].Font.Bold = true;
                            gvTripSheetSummary.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                            gvTripSheetSummary.FooterRow.Cells[12].Font.Size = 20;
                            hfRefundAmt.Value = sum.ToString();
                        }
                        else
                        {
                            gvTripSheetSummary.DataBind();
                            lblGridMsg.Visible = true;
                            lblGridMsg.Text = Response.ToString();

                        }
                    }
                }
                else
                {
                    gvTripSheetSummary.DataBind();
                    lblGridMsg.Visible = true;
                    lblGridMsg.Text = Response.ToString();

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }
    protected void btnPDF_Click(object sender, EventArgs e)
    {
        GeneratePDF();
    }

    public void GeneratePDF()
    {
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new RptTripSheetSummary();
                if (Session["UserRole"].ToString().ToLower().Trim() == "user")
                {
                    vTripSheetSettlement = new RptTripSheetSummary()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        QueryType = "TripSheetSummaryDetails",
                        ServiceType = "",
                        BookingId = "",
                        Input1 = "",
                        Input2 = "",
                        Input3 = "",
                        Input4 = Session["UserId"].ToString().Trim(),
                        Input5 = Session["UserRole"].ToString().ToLower().Trim()
                    };
                }
                else
                {
                    vTripSheetSettlement = new RptTripSheetSummary()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        QueryType = "TripSheetSummaryDetails",
                        ServiceType = "",
                        BookingId = "",
                        Input1 = "",
                        Input2 = "",
                        Input3 = "",
                        Input4 = "",
                        Input5 = ""
                    };
                }
       

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    var GetResponse = response.Content.ReadAsStringAsync().Result;
                    if (GetResponse.Contains("No Records Found"))
                    {
                        //divNoOfTrips.Visible = true;
                        gvNoOfTrips.Visible = false;
                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + GetResponse + "');", true);
                        return;
                    }
                    else
                    {
                        var ResponseMsgDtl = JObject.Parse(GetResponse)["Table"].ToString();
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgDtl);

                        var ResponseMsgCnt = JObject.Parse(GetResponse)["Table1"].ToString();
                        DataTable dtCnt = JsonConvert.DeserializeObject<DataTable>(ResponseMsgCnt);

                        if (dt.Rows.Count > 0)
                        {

                            objReportDocument.Load(Server.MapPath("CRTripsheetSummary.rpt"));
                            objReportDocument.SetDataSource(dt);

                            decimal total = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                total += decimal.Parse(dt.Rows[i]["NoOfTrips"].ToString());
                            }


                            decimal reftotal = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                reftotal += decimal.Parse(dt.Rows[i]["ClaimedDeposit"].ToString());
                            }

                            decimal Collection = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                Collection += decimal.Parse(dt.Rows[i]["CollectionAmt"].ToString());
                            }

                            decimal UnClaimedDep = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                UnClaimedDep += decimal.Parse(dt.Rows[i]["UnClaimedDeposit"].ToString());
                            }

                            decimal ExtentedAmt = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                ExtentedAmt += decimal.Parse(dt.Rows[i]["ExtendedAmt"].ToString());
                            }

                            decimal RowerAmt = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                RowerAmt += decimal.Parse(dt.Rows[i]["Roweramt"].ToString());
                            }

                            decimal RowerSettle = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string F = dt.Rows[i].ToString();
                                RowerSettle += decimal.Parse(dt.Rows[i]["RowerSettlementAmt"].ToString());
                            }

                            string date = DateTime.Now.ToString("dd/MM/yyyy") + ' ' + DateTime.Now.ToString("h:mm:ss tt");

                            string FromTodate = string.Empty;
                            if (txtFromDate.Text.Trim() == txtToDate.Text.Trim())
                            {
                                FromTodate = txtFromDate.Text.Trim();
                            }
                            else
                            {
                                FromTodate = txtFromDate.Text.Trim() + " to " + txtToDate.Text.Trim();
                            }


                            objReportDocument.SetParameterValue(0, total);
                            objReportDocument.SetParameterValue(1, Collection);
                            objReportDocument.SetParameterValue(2, reftotal);
                            objReportDocument.SetParameterValue(3, UnClaimedDep);
                            objReportDocument.SetParameterValue(4, ExtentedAmt);
                            objReportDocument.SetParameterValue(5, RowerAmt);
                            objReportDocument.SetParameterValue(6, RowerSettle);
                            objReportDocument.SetParameterValue(7, Session["BoatHouseName"].ToString().Trim());
                            objReportDocument.SetParameterValue(8, FromTodate);
                            objReportDocument.SetParameterValue(9, date);
                            objReportDocument.SetParameterValue(10, Session["CorpName"].ToString());

                            objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                            objReportExport = objReportDocument.ExportOptions;
                            objReportExport.ExportDestinationOptions = objReportDiskOption;
                            objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                            objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                            objReportDocument.Export();
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.ContentType = "application/pdf";
                            Response.WriteFile(Server.MapPath(sFilePath));
                            Response.Flush();
                            Response.Close();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                            gvNoOfTrips.Visible = false;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Response + "');", true);
                    gvNoOfTrips.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
        finally
        {
            if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
            {
                System.IO.File.Delete(Server.MapPath(sFilePath));
            }

            objReportDocument.Dispose();

            objReportDiskOption = null;
            objReportDocument = null;
            objReportExport = null;
            GC.Collect();
        }
    }
    public class RptTripSheetSummary
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DepositType { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
        public string BoatId { get; set; }
        public string UserRole { get; set; }
        public string UserId { get; set; }
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string BookingId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

    }


}