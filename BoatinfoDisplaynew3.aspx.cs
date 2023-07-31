using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblBoatHouseName.Text = Session["BoatHouseName"].ToString().Trim() + " - " + "Boating Departure";
            BoatBookedSummaryList();
           
        }
    }
    public void BoatBookedSummaryList()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new BoatSearch()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatListDisplaynew/BoatTypenew", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatLst = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            dtlBoat.Visible = true;
                            dtlBoat.DataSource = dt;
                            dtlBoat.DataBind();
                            dtlBoat2.Visible = true;
                            dtlBoat2.DataSource = dt;
                            dtlBoat2.DataBind();
                            divDatalist.Visible = true;
                           


                            int Count = dt.Rows.Count;
                            if (Count <= 3) { txtCarouselTime.Value = Convert.ToInt32("15000").ToString(); }
                            if (Count == 4) { txtCarouselTime.Value = Convert.ToInt32("25000").ToString(); }
                            if (Count == 5) { txtCarouselTime.Value = Convert.ToInt32("25000").ToString(); }
                            if (Count == 6) { txtCarouselTime.Value = Convert.ToInt32("25000").ToString(); }
                            if (Count == 7) { txtCarouselTime.Value = Convert.ToInt32("35000").ToString(); }
                            if (Count == 8) { txtCarouselTime.Value = Convert.ToInt32("35000").ToString(); }
                            if (Count == 9) { txtCarouselTime.Value = Convert.ToInt32("35000").ToString(); }
                            if (Count == 10) { txtCarouselTime.Value = Convert.ToInt32("45000").ToString(); }
                            if (Count == 11) { txtCarouselTime.Value = Convert.ToInt32("45000").ToString(); }
                            if (Count == 12) { txtCarouselTime.Value = Convert.ToInt32("45000").ToString(); }
                            if (Count == 13) { txtCarouselTime.Value = Convert.ToInt32("55000").ToString(); }
                            if (Count == 14) { txtCarouselTime.Value = Convert.ToInt32("55000").ToString(); }
                            if (Count == 15) { txtCarouselTime.Value = Convert.ToInt32("55000").ToString(); }
                            if (Count == 16) { txtCarouselTime.Value = Convert.ToInt32("65000").ToString(); }
                            if (Count == 17) { txtCarouselTime.Value = Convert.ToInt32("65000").ToString(); }
                            if (Count == 18) { txtCarouselTime.Value = Convert.ToInt32("65000").ToString(); }
                              }
                        else
                        {
                            dtlBoat.DataBind();
                           
                            divDatalist.Visible = true;
                        }
                    }
                    else
                    {
                        divDatalist.Visible = true;
                    }
                }
                else
                {
                    divDatalist.Visible = true;
                }
            }
        }
        catch (Exception)
        {
            divDatalist.Visible = false;
        }
    }

   
    public class BoatSearch
    {
        public string QueryType { get; set; }
        public string BoatHouseName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string BookingMedia { get; set; }
        public string NoOfPass { get; set; }
        public string NoOfChild { get; set; }
        public string NoOfInfant { get; set; }
        public string InitBillAmount { get; set; }
        public string PaymentType { get; set; }
        public string ActualBillAmount { get; set; }
        public string Status { get; set; }
        public string BoatSeaterId { get; set; }
        public string BookingDuration { get; set; }
        public string InitBoatCharge { get; set; }
        public string InitRowerCharge { get; set; }
        public string BoatDeposit { get; set; }
        public string TaxDetails { get; set; }
        public string InitOfferAmount { get; set; }
        public string InitNetAmount { get; set; }
        public string CreatedBy { get; set; }
        public string OthServiceStatus { get; set; }
        public string BoatHouseId { get; set; }
        public string PremiumStatus { get; set; }
        public string OfferId { get; set; }
        public string BoatTypeId { get; set; }
        public string BookingDate { get; set; }
        public string TaxId { get; set; }
        public string ValidDate { get; set; }
        public string BookingId { get; set; }
        public string BookingType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceId { get; set; }
        public string OthServiceId { get; set; }
        public string OthChargePerItem { get; set; }
        public string OthNoOfItems { get; set; }
        public string OthTaxDetails { get; set; }
        public string OthNetAmount { get; set; }
        public string Bookingpin { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string UserRole { get; set; }
        public string ServiceType { get; set; }
    }

    protected void dtlBoat_ItemDataBound1(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.DataItem != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblBoatTypeId = (Label)e.Item.FindControl("lblBoatTypeId");
                Label lblBtBoatType = (Label)e.Item.FindControl("lblBtBoatType");
                Control divCList1 = e.Item.FindControl("divCList1");
                Control divCLListli1 = e.Item.FindControl("divCLListli1");
                Image ImageBoat = (Image)e.Item.FindControl("lblBtBoatImgLink");
                HtmlControl control = e.Item.FindControl("divCList1") as HtmlControl;

                HtmlControl control2 = e.Item.FindControl("divCLListli1") as HtmlControl;

                var BoatSeatNOTMAL = e.Item.FindControl("dtlBoatchild1") as DataList;

                var BoatPERMIUM = e.Item.FindControl("dtDataChild2") as DataList;
                var AvailPERMIUM = e.Item.FindControl("dtAvailPre") as DataList;
                var AvailNormal = e.Item.FindControl("dtAvailNrml") as DataList;

                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        //client.BaseAddress = new Uri("http://localhost:50773/api/");
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var BoatSearch = new BoatSearch()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            PremiumStatus = "N",
                            BoatTypeId = lblBoatTypeId.Text.Trim('~')

                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("BoatListDisplaynew/Departurenew/TypeIdnew", BoatSearch).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var BoatLst = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                                if (dt.Rows.Count > 0)
                                {
                                    BoatSeatNOTMAL.DataSource = dt;
                                    BoatSeatNOTMAL.DataBind();
                                    BoatSeatNOTMAL.Visible = true;
                                    int Count = dt.Rows.Count;
                                    //ImageBoat.ImageUrl = dt.Rows[0]["BoatImageLink"].ToString();

                                }
                                else
                                {
                                    BoatSeatNOTMAL.DataSource = dt;
                                    BoatSeatNOTMAL.DataBind();
                                    BoatSeatNOTMAL.Visible = false;
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                        }
                    }
                    //----------------------------------------------------------------------------------------------------
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        //client.BaseAddress = new Uri("http://localhost:50773/api/");
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var BoatSearch = new BoatSearch()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            PremiumStatus = "P",
                            BoatTypeId = lblBoatTypeId.Text.Trim('~')

                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("BoatListDisplaynew/Departurenew/TypeIdnew", BoatSearch).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var BoatLst = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                                if (dt.Rows.Count > 0)
                                {
                                    BoatPERMIUM.DataSource = dt;
                                    BoatPERMIUM.DataBind();
                                    BoatPERMIUM.Visible = true;
                                }
                                else
                                {
                                    BoatPERMIUM.DataSource = dt;
                                    BoatPERMIUM.DataBind();
                                    BoatPERMIUM.Visible = false;
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                    }


                   

                }
                catch (Exception )
                {
                }
            }
        }
        catch (Exception )
        {

        }
    }

    



   
    
    protected void dtlBoat2_ItemDataBound2(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.DataItem != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblBoatTypeId = (Label)e.Item.FindControl("lblBoatTypeId");
                Label lblBtBoatType = (Label)e.Item.FindControl("lblBtBoatType");
                Control divCList1 = e.Item.FindControl("divCList1");
                Control divCLListli1 = e.Item.FindControl("divCLListli1");
                Image ImageBoat = (Image)e.Item.FindControl("lblBtBoatImgLink");
                HtmlControl control = e.Item.FindControl("divCList1") as HtmlControl;

                HtmlControl control2 = e.Item.FindControl("divCLListli1") as HtmlControl;

                var BoatSeatNOTMAL = e.Item.FindControl("dtlBoatchild1") as DataList;

                var BoatPERMIUM = e.Item.FindControl("dtDataChild2") as DataList;
                var AvailPERMIUM = e.Item.FindControl("dtAvailPre") as DataList;
                var AvailNormal = e.Item.FindControl("dtAvailNrml") as DataList;

                try
                {
                   
                    //------------------------------------------AVAIL NOR----------------------------------------------------------
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        //client.BaseAddress = new Uri("http://localhost:50773/api/");
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var BoatSearch = new BoatSearch()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            PremiumStatus = "N",
                            BoatTypeId = lblBoatTypeId.Text.Trim('~'),

                            BookingDate = DateTime.Today.ToString("dd/MM/yyyy")
                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("BoatBooking/BoatAvailableList", BoatSearch).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var BoatLst = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                                if (dt.Rows.Count > 0)
                                {
                                    AvailNormal.DataSource = dt;
                                    AvailNormal.DataBind();
                                    AvailNormal.Visible = true;
                                }
                                else
                                {
                                    AvailNormal.DataSource = dt;
                                    AvailNormal.DataBind();
                                    AvailNormal.Visible = false;
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                    //------------------------------------------AVAIL PRE----------------------------------------------------------
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        //client.BaseAddress = new Uri("http://localhost:50773/api/");
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string boatTypeIds = string.Empty;

                        var BoatSearch = new BoatSearch()
                        {
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            PremiumStatus = "P",
                            BoatTypeId = lblBoatTypeId.Text.Trim('~'),
                            BookingDate = DateTime.Today.ToString("dd/MM/yyyy")

                        };

                        HttpResponseMessage response = client.PostAsJsonAsync("BoatBooking/BoatAvailableList", BoatSearch).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var BoatLst = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(BoatLst)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(BoatLst)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                                if (dt.Rows.Count > 0)
                                {
                                    AvailPERMIUM.DataSource = dt;
                                    AvailPERMIUM.DataBind();
                                    AvailPERMIUM.Visible = true;
                                }
                                else
                                {
                                    AvailPERMIUM.DataSource = dt;
                                    AvailPERMIUM.DataBind();
                                    AvailPERMIUM.Visible = false;
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                    }


                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {

        }
    }
}