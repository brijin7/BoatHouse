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

public partial class Boating_BoatWiseTrip : System.Web.UI.Page
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
                ViewState["IsSeatAvail"] = "0";
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                GetBoatType();
                Trips();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
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
                var BoatTrip = new BoatTrip()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatType/BoatRateMstr", BoatTrip).Result;

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


    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBoatSeater();
        ddlBoatSelection.Items.Clear();
        ddlBoatSelection.Items.Insert(0, new ListItem("All", "0"));
    }
    public void GetBoatSeater()
    {
        try
        {
            ddlBoatSeatId.Items.Clear();

            if (ddlBoatType.SelectedIndex == 0)
            {
                ddlBoatSeatId.Items.Insert(0, new ListItem("All", "0"));
                return;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatTrip = new BoatType()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatSeat/BoatRateMstr", BoatTrip).Result;

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
                            ddlBoatSeatId.DataSource = dt;
                            ddlBoatSeatId.DataValueField = "BoatSeaterId";
                            ddlBoatSeatId.DataTextField = "SeaterType";
                            ddlBoatSeatId.DataBind();

                        }
                        else
                        {
                            ddlBoatSeatId.DataBind();
                        }
                        ddlBoatSeatId.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoatSeatId.Items.Insert(0, new ListItem("All", "0"));

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


    protected void ddlBoatSeatId_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBoatSelection();
    }
    public void GetBoatSelection()
    {
        try
        {
            ddlBoatSelection.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var boatmaster = new BoatType()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeatId.SelectedValue.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("ddlBoatMaster/BHId", boatmaster).Result;

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
                            ddlBoatSelection.DataSource = dt;
                            ddlBoatSelection.DataValueField = "BoatNum";
                            ddlBoatSelection.DataTextField = "BoatName";
                            ddlBoatSelection.DataBind();

                        }
                        else
                        {
                            ddlBoatSelection.DataBind();
                        }
                        ddlBoatSelection.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoatSelection.Items.Insert(0, new ListItem("All", "0"));
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


    public void Trips()
    {
        try
        {
            divmaxtrips.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;


                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    var BoatTrip = new BoatTrip()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        BoatType = ddlBoatType.SelectedValue.Trim('~'),
                        BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                        BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = "Admin"

                    };
                    response = client.PostAsJsonAsync("BoatwiseTrip/ListAll", BoatTrip).Result;
                }
                else
                {
                    var BoatTrip = new BoatTrip()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        BoatType = ddlBoatType.SelectedValue.Trim('~'),
                        BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                        BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("BoatwiseTrip/ListAll", BoatTrip).Result;
                }


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
                            GvMaxTrips.DataSource = dt;
                            GvMaxTrips.DataBind();
                            GvMaxTrips.Visible = true;
                            lblmesg.Visible = false;
                            int TripCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Trips")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));

                            GvMaxTrips.FooterRow.Cells[2].Text = "Total";
                            GvMaxTrips.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;

                            GvMaxTrips.FooterRow.Cells[3].Text = TripCount.ToString();
                            GvMaxTrips.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                            GvMaxTrips.FooterRow.Cells[4].Text = TotalAmount.ToString("N2");
                            GvMaxTrips.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            GvMaxTrips.Visible = false;
                            lblGridMsg.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        GvMaxTrips.Visible = false;
                        lblmesg.Visible = true;
                        lblGridMsg.Text = "No Records Found";
                        divmaxtrips.Visible = false;

                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);

                    }
                }
                else
                {
                    divmaxtrips.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
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
        Trips();

        divGridSummary.Visible = false;
        divGridList.Visible = false;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddlBoatType.SelectedValue = "0";
        GetBoatSeater();
        GetBoatSelection();
        Trips();
        divGridList.Visible = false;
        divGridSummary.Visible = false;
    }


    protected void GvBoatwiseTrip_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBoatwiseTrip.PageIndex = e.NewPageIndex;
       
        if (ViewState["IsSeatAvail"].Equals("0"))
        {
            BindoverAllTrips();
        }
        else
        {
            BindBoatwiseTrip();
        }

        


    }
    public void BindBoatwiseTrip()
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

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;


                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    var BoatTrip = new BoatTrip()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatType = ddlBoatType.SelectedValue.Trim(),
                        BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                        BoatTypeId = hfBoatId.Value.Trim(),
                        BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                        BoatSeaterId = hfBoatSeaterId.Value.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = "Admin"

                    };
                    response = client.PostAsJsonAsync("BoatwiseTripType/Detail", BoatTrip).Result;
                }

                else
                {
                    var BoatTrip = new BoatTrip()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatType = ddlBoatType.SelectedValue.Trim(),
                        BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                        BoatTypeId = hfBoatId.Value.Trim(),
                        BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                        BoatSeaterId = hfBoatSeaterId.Value.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("BoatwiseTripType/Detail", BoatTrip).Result;
                }



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
                            GvBoatwiseTrip.DataSource = dt;
                            GvBoatwiseTrip.DataBind();
                            GvBoatwiseTrip.Visible = true;

                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));

                            GvBoatwiseTrip.FooterRow.Cells[8].Text = "Total";
                            GvBoatwiseTrip.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Center;

                            GvBoatwiseTrip.FooterRow.Cells[9].Text = TotalAmount.ToString("N2");
                            GvBoatwiseTrip.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            GvBoatwiseTrip.Visible = false;
                        }
                    }
                    else
                    {
                        divGridList.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
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


    public void BindBoatwiseSummary()
    {
        try
        {
            divGridSummary.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;

                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    var BoatTrip = new BoatTrip()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        BoatType = hfBoatTypeId.Value.Trim(),
                        BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                        BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = "Admin"

                    };
                    response = client.PostAsJsonAsync("BoatwiseTrip/Summary", BoatTrip).Result;
                }
                else
                {
                    var BoatTrip = new BoatTrip()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString(),
                        BoatType = hfBoatTypeId.Value.Trim(),
                        BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                        BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("BoatwiseTrip/Summary", BoatTrip).Result;
                }



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
                            GvSummary.DataSource = dt;
                            GvSummary.DataBind();
                            GvSummary.Visible = true;

                            int TripCount = dt.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("Trips")));
                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));

                            GvSummary.FooterRow.Cells[4].Text = "Total";
                            GvSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                            GvSummary.FooterRow.Cells[5].Text = TripCount.ToString();
                            GvSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;

                            GvSummary.FooterRow.Cells[6].Text = TotalAmount.ToString("N2");
                            GvSummary.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            GvSummary.Visible = false;
                            divGridSummary.Visible = false;
                        }
                    }
                    else
                    {
                        GvSummary.Visible = false;
                        divGridSummary.Visible = false;

                    }
                }
                else
                {
                    GvSummary.Visible = false;
                    divGridSummary.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void GvSummary_pageindexchanging(object sender, GridViewPageEventArgs e)
    {
        GvSummary.PageIndex = e.NewPageIndex;
        BindBoatwiseSummary();
    }
    protected void lblTrips_Click(object sender, EventArgs e)
    {
        LinkButton lnktrips = sender as LinkButton;
        GridViewRow gvrow = lnktrips.NamingContainer as GridViewRow;
        Label BoatId = (Label)gvrow.FindControl("lblBoatTypeId");
        hfBoatId.Value = BoatId.Text;
        Label BoatSeaterId = (Label)gvrow.FindControl("lblBoatSeaterId");
        hfBoatSeaterId.Value = BoatSeaterId.Text;
        ViewState["IsSeatAvail"] = "1";
        GvBoatwiseTrip.PageIndex = 0;
        BindBoatwiseTrip();
    }


    protected void lblTotalTrips_Click(object sender, EventArgs e)
    {
        LinkButton lnktrips = sender as LinkButton;
        GridViewRow gvrow = lnktrips.NamingContainer as GridViewRow;
        Label BoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
        hfBoatTypeId.Value = BoatTypeId.Text;
        ViewState["IsSeatAvail"] = "0";
        GvBoatwiseTrip.PageIndex = 0;
        BindBoatwiseSummary();
        BindoverAllTrips();
    }
    public void BindoverAllTrips()
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

                //var BoatTrip = new BoatTrip()
                //{
                //    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                //    BoatType = ddlBoatType.SelectedValue.Trim(),
                //    BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                //    BoatTypeId = hfBoatTypeId.Value.Trim(),
                //    BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                //    //BoatSeaterId = hfBoatSeaterId.Value.Trim(),
                //    FromDate = txtFromDate.Text.Trim(),
                //    ToDate = txtToDate.Text.Trim()

                //};

                //HttpResponseMessage response = client.PostAsJsonAsync("BoatwiseAllType/Detail", BoatTrip).Result;

                string UserRole = Session["UserRole"].ToString().Trim();
                HttpResponseMessage response;


                if (UserRole == "Sadmin" || UserRole == "Admin")
                {
                    var BoatTrip = new BoatTrip()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatType = ddlBoatType.SelectedValue.Trim(),
                        BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                        BoatTypeId = hfBoatTypeId.Value.Trim(),
                        BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                        //BoatSeaterId = hfBoatSeaterId.Value.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = "Admin"

                    };
                    response = client.PostAsJsonAsync("BoatwiseAllType/Detail", BoatTrip).Result;
                }
                else
                {
                    var BoatTrip = new BoatTrip()
                    {
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatType = ddlBoatType.SelectedValue.Trim(),
                        BoatSelection = ddlBoatSelection.SelectedValue.Trim(),
                        BoatTypeId = hfBoatTypeId.Value.Trim(),
                        BoatSeater = ddlBoatSeatId.SelectedValue.Trim(),
                        //BoatSeaterId = hfBoatSeaterId.Value.Trim(),
                        FromDate = txtFromDate.Text.Trim(),
                        ToDate = txtToDate.Text.Trim(),
                        UserId = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("BoatwiseAllType/Detail", BoatTrip).Result;
                }



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
                            GvBoatwiseTrip.DataSource = dt;
                            GvBoatwiseTrip.DataBind();
                            GvBoatwiseTrip.Visible = true;

                            decimal TotalAmount = dt.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("Amount")));

                            GvBoatwiseTrip.FooterRow.Cells[8].Text = "Total";
                            GvBoatwiseTrip.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Center;

                            GvBoatwiseTrip.FooterRow.Cells[9].Text = TotalAmount.ToString("N2");
                            GvBoatwiseTrip.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                        }
                        else
                        {
                            GvBoatwiseTrip.Visible = false;
                        }
                    }
                    else
                    {
                        divGridList.Visible = false;

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


    public class BoatType
    {
        public string BoatHouseId { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeaterId { get; set; }
    }
    public class BoatTrip
    {
        public string BoatHouseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BoatType { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatSeater { get; set; }
        public string BookingId { get; set; }
        public string BookingDate { get; set; }
        public string BoatSelection { get; set; }
        public string BoatSeaterId { get; set; }
        public string UserId { get; set; }


    }
}