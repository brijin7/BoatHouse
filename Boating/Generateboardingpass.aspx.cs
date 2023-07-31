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

public partial class Boating_Generateboardingpass : System.Web.UI.Page
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
                txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtBookingDate.Attributes.Add("readonly", "readonly");
                BindBoardingPass();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindBoardingPass()
    {
        try
        {
            divShowDetails.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new BoardingPass()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BookingDate = txtBookingDate.Text.Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("GenerateBoardingPass", vTripSheetSettlement).Result;

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
                            divShowDetails.Visible = true;

                            GvGenerateBoardingPass.DataSource = dt;
                            GvGenerateBoardingPass.DataBind();
                            divmsbeb.Visible = false;
                        }
                        else
                        {
                            GvGenerateBoardingPass.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found !!!');", true);
                            divmsbeb.Visible = true;
                            lblNorecords.Text = "No Details Found !!!";
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found !!!');", true);
                        divmsbeb.Visible = true;
                        lblNorecords.Text = "No Details Found !!!";
                        return;
                    }

                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtBookingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        BindBoardingPass();
    }


    protected void lnkbtnBookingId_Click(object sender, EventArgs e)
    {
        LinkButton imgbtn = sender as LinkButton;
        GridViewRow gvrow = imgbtn.NamingContainer as GridViewRow;
        string BookingId = GvGenerateBoardingPass.DataKeys[gvrow.RowIndex]["BookingId"].ToString();
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var vTripSheetSettlement = new BoardingPass()
                {
                    QueryType = "GenerateBoardingPass",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BooKingId = BookingId.ToString().Trim(),
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    FromDate = "",
                    ToDate = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonOperation", vTripSheetSettlement).Result;

                if (response.IsSuccessStatusCode)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Boarding Pass Generated Successfully !!!');", true);
                    BindBoardingPass();
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
            return;
        }
    }

    protected void btnSubmit_Click1(object sender, EventArgs e)
    {
        BindBoardingPass();
    }

    public class BoardingPass
    {
        public string BookingDate { get; set; }
        public string BoatHouseId { get; set; }
        public string BooKingId { get; set; }
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

    }
}