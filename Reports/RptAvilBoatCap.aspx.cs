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

public partial class RptAvilBoatCap : System.Web.UI.Page
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
                txtTripDate.Attributes.Add("readonly", "readonly");

                txtTripDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                GetBoatStatus();
                GetBoatType();
                GetBoatSeatType();
                GetSeatType();
                BindBoatAvailCapALL();
                BindAvailSummary();
                Clear();

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }

    }
    
    public void GetBoatStatus()
    {
        try
        {
            ddlBoatStatus.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLBoatStatus").Result;

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
                            ddlBoatStatus.DataSource = dt;
                            ddlBoatStatus.DataValueField = "ConfigId";
                            ddlBoatStatus.DataTextField = "ConfigName";
                            ddlBoatStatus.DataBind();
                        }
                        else
                        {
                            ddlBoatStatus.DataBind();
                        }

                        ddlBoatStatus.Items.Insert(0, new ListItem("All", "0"));
                    }
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
            ddlBoatType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var availBoatCap = new AvailBoatCap()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatStatus = ddlBoatStatus.SelectedValue.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BStatus/ddlBoatType", availBoatCap).Result;

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
                    else
                    {
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

    public void GetBoatSeatType()

    {
        try
        {
            ddlBoatSeattype.Items.Clear();
           using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var availBoatCap = new AvailBoatCap()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatStatus = ddlBoatStatus.SelectedValue.Trim()

                };

                HttpResponseMessage response = client.PostAsJsonAsync("BStatus/ddlBoatSeat", availBoatCap).Result;

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
                            ddlBoatSeattype.DataSource = dt;
                            ddlBoatSeattype.DataValueField = "BoatSeaterId";
                            ddlBoatSeattype.DataTextField = "SeaterType";
                            ddlBoatSeattype.DataBind();

                        }
                        else
                        {
                            ddlBoatSeattype.DataBind();
                        }
                        ddlBoatSeattype.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlBoatSeattype.Items.Insert(0, new ListItem("All", "0"));

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

    public void GetSeatType()
    {
        try
        {
            ddlSeatType.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var availBoatCap = new AvailBoatCap()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeattype.SelectedValue.Trim(),
                    BoatStatus = ddlBoatStatus.SelectedValue.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BStatus/ddlBoatName", availBoatCap).Result;

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
                            ddlSeatType.DataSource = dt;
                            ddlSeatType.DataValueField = "BoatNum";
                            ddlSeatType.DataTextField = "BoatName";
                            ddlSeatType.DataBind();

                        }
                        else
                        {
                            ddlSeatType.DataBind();
                        }
                        ddlSeatType.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ddlSeatType.Items.Insert(0, new ListItem("All", "0"));
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

    public void BindAvailSummary()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var availBoatCap = new AvailBoatCap()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    TripStartTime = txtTripDate.Text.ToString()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BH/RptAvalBoatCapSummary", availBoatCap).Result;
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
                            divAvailSummary.Visible = true;
                            gvAvailTripSummary.DataSource = dt;
                            gvAvailTripSummary.DataBind();
                        }
                        else
                        {
                            divAvailSummary.Visible = false;
                            gvAvailTripSummary.DataSource = dt;
                            gvAvailTripSummary.DataBind();
                           // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        }
                    }
                    else
                    {
                        divAvailSummary.Visible = false;
                     //   ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
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

    public void BindBoatAvailCapALL()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var availBoatCap = new AvailBoatCap()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatSeaterId = ddlBoatSeattype.SelectedValue.Trim(),
                    BoatNum = ddlSeatType.SelectedValue.Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatStatus = ddlBoatStatus.SelectedValue.Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BH/RptAvalBoatCapALL", availBoatCap).Result;
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
                            gvAvailall.DataSource = dt;
                            gvAvailall.DataBind();
                            divAvailAll.Visible = true;
                      

                        }
                        else
                        {
                            divAvailAll.Visible = false;
                            gvAvailall.DataSource = dt;
                            gvAvailall.DataBind();
                       

                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        }
                    }
                    else
                    {
                        divAvailAll.Visible = false;
                       ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //Newly added by Brijin on 2022-05-24 for Page index
        gvAvailall.PageIndex = 0;
        BindBoatAvailCapALL();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
     
        Clear();
        GetBoatType();
        GetBoatSeatType();
        GetSeatType();
        BindBoatAvailCapALL();
        BindAvailSummary();
    }

    public void Clear()
    {
        txtTripDate.Attributes.Add("readonly", "readonly");
        txtTripDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddlBoatStatus.SelectedIndex = 0;
        ddlBoatType.SelectedIndex = 0;
        ddlBoatSeattype.SelectedIndex = 0;
        ddlSeatType.SelectedIndex = 0;
        //Newly added by Brijin on 2022-05-24 for Page index
        gvAvailall.PageIndex = 0;

    }

    protected void ddlBoatStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBoatStatus.SelectedIndex == 0)
        {
            ddlBoatType.SelectedIndex = 0;
            ddlBoatSeattype.SelectedIndex = 0;
            ddlSeatType.SelectedIndex = 0;
            divAvailAll.Visible = true;
           
        }
        GetBoatType();
        GetBoatSeatType();
        GetSeatType();


    }

    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBoatSeatType();
        GetSeatType();

    }

    protected void ddlBoatSeattype_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSeatType();
   }
        
    protected void txtTripDate_TextChanged(object sender, EventArgs e)
    {
        //Newly added by Brijin on 2022-05-24 for Page index
        gvAvailall.PageIndex = 0;
        BindAvailSummary();
    }

    protected void gvAvailall_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAvailall.PageIndex = e.NewPageIndex;
        BindBoatAvailCapALL();
    }

    public class AvailBoatCap
    {
        public string BoatTypeId { get; set; }
        public string BoatType { get; set; }
        public string BoatStatus { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatSeaterId { get; set; }
        public string SeaterType { get; set; }
        public string BoatNum { get; set; }
        public string BoatName { get; set; }
        public string BoatStatusName { get; set; }
        public string MaxTripsPerDay { get; set; }
        public string TripStartTime { get; set; }
    }
}