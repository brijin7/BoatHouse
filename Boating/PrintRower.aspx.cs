using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Modified By:Pretheka.C
/// Modified Date:13th Oct 2021
/// </summary>

public partial class Boating_PrintRower : System.Web.UI.Page
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
                Session["CorpLogo"] = "http://saveme.live/paypre-image-api/upload?fileId=649ea82c06bfb6b6b9922803.png";
                if (Request.HttpMethod != "GET")
                {
                    AntiForgery.Validate(ViewState["__AntiForgeryCookie"] as string, Request.Form["__RequestVerificationToken"]);
                }
                string sBoatHouseId = Request.QueryString["BId"].ToString();
                string sSettlementId = Request.QueryString["SId"].ToString();
                BindBoatCharge(sBoatHouseId, sSettlementId);
                divRefund.Visible = true;
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindBoatCharge(string sBoatHouseId, string sSettlementId)
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
                var btConsumption = new RowerSettlement()
                {
                    SettlementId = sSettlementId.Trim(),
                    BoatHouseId = sBoatHouseId.Trim()
                };
                response = client.PostAsJsonAsync("RowerSettledIdGrid", btConsumption).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dPrint = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dPrint.Rows.Count > 0)
                        {

                            lblRowerBoatHouseName.Text = Session["BoatHouseName"].ToString();
                            lblRowerHeader.Text = "Rower Settlement";
                            lblSettlemtId.Text = sSettlementId.Trim();
                            lblSettlementDate.Text = dPrint.Rows[0]["SettlementDate"].ToString();
                            lblTripDate.Text = dPrint.Rows[0]["TripDate"].ToString();
                            lblRowerName.Text = dPrint.Rows[0]["RowerName"].ToString();
                            if (dPrint.Rows[0]["CustomerMobile"].ToString() == "")
                            {
                                divMobile.Visible = false;
                            }
                            else
                            {
                                divMobile.Visible = true;
                                lblCustomerMob.Text = dPrint.Rows[0]["CustomerMobile"].ToString();
                            }

                            decimal totSettleAmt = dPrint.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<string>("ActualRowerCharge")));
                            int Count = dPrint.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<string>("NoOfTrips")));
                            lblTripCount.Text = Count.ToString();
                            lblSettlementAmt.Text = totSettleAmt.ToString("N2");
                            lblPrintedByName.Text = Session["PrintUserName"].ToString();
                            lblPrintDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/");

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Details Found.');", true);
                            return;
                        }
                    }
                    else
                    {

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


    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("RowerSettlement.aspx");
    }




}