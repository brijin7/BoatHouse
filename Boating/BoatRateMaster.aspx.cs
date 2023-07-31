using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Boating_BoatRateMaster : System.Web.UI.Page
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
            if (!Page.IsPostBack)
            {
                if (Request.HttpMethod != "GET")
                {
                    AntiForgery.Validate(ViewState["__AntiForgeryCookie"] as string, Request.Form["__RequestVerificationToken"]);
                }
                GetTaxDetail();
                GetBoatType();
                //GetBoatSeat();
                Clear();
                BindBoatRate();
                BindMinute();
                ddlBoatHouse.Focus();

                hfCreatedBy.Value = Session["UserId"].ToString().Trim();
                btnSubmit.Text = "Submit";

                txtBoatMinCharge.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtBoatMinCharge.ClientID + "', '" + txtRowerMinCharge.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtBtNorCharges.ClientID + "', '" + txtTaxBtNorChrg.ClientID + "')");

                txtRowerMinCharge.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtBoatMinCharge.ClientID + "', '" + txtRowerMinCharge.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtBtNorCharges.ClientID + "', '" + txtTaxBtNorChrg.ClientID + "')");

                txtBtNorCharges.Attributes.Add("readonly", "readonly");
                txtTaxBtNorChrg.Attributes.Add("readonly", "readonly");

                txtBoatPrmMin.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtBoatPrmMin.ClientID + "', '" + txtRowerPrmMin.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtBtPremCharge.ClientID + "', '" + txtTaxBtPremChrg.ClientID + "')");

                txtRowerPrmMin.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtBoatPrmMin.ClientID + "', '" + txtRowerPrmMin.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtBtPremCharge.ClientID + "', '" + txtTaxBtPremChrg.ClientID + "')");

                txtBtPremCharge.Attributes.Add("readonly", "readonly");
                txtTaxBtPremChrg.Attributes.Add("readonly", "readonly");


                txtWkEdAmt.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtWkEdAmt.ClientID + "', '" + txtWkEdPamt.ClientID + "', '"
                 + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtWkEdChrge.ClientID + "', '" + TxtWkEdToTax.ClientID + "')");

                txtWkEdPamt.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtWkEdAmt.ClientID + "', '" + txtWkEdPamt.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtWkEdChrge.ClientID + "', '" + TxtWkEdToTax.ClientID + "')");

                txtWkEdChrge.Attributes.Add("readonly", "readonly");
                TxtWkEdToTax.Attributes.Add("readonly", "readonly");
                //**********************//

                txtIWDAmt.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtIWDAmt.ClientID + "', '" + txtIWdPaymt.ClientID + "', '"
                 + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtIWDBtChrg.ClientID + "', '" + txtIWdToTax.ClientID + "')");

                txtIWdPaymt.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtIWDAmt.ClientID + "', '" + txtIWdPaymt.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtIWDBtChrg.ClientID + "', '" + txtIWdToTax.ClientID + "')");

                txtIWDBtChrg.Attributes.Add("readonly", "readonly");
                txtIWdToTax.Attributes.Add("readonly", "readonly");


                txtIWEAmt.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtIWEAmt.ClientID + "', '" + txtIWEPaymt.ClientID + "', '"
                 + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtIWEBtChrg.ClientID + "', '" + txtIWEToTax.ClientID + "')");

                txtIWEPaymt.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtIWEAmt.ClientID + "', '" + txtIWEPaymt.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtIWEBtChrg.ClientID + "', '" + txtIWEToTax.ClientID + "')");

                txtIWEBtChrg.Attributes.Add("readonly", "readonly");
                txtIWEToTax.Attributes.Add("readonly", "readonly");


                ///***************************///
                txtBoatExtnCharge.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtBoatExtnCharge.ClientID + "', '" + txtRowerExtnCharge.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtBtNorExtCharges.ClientID + "', '" + txtTaxBtExtChrg.ClientID + "')");

                txtRowerExtnCharge.Attributes.Add("onkeyup", "ShowBoatGSTCalculation('" + txtBoatExtnCharge.ClientID + "', '" + txtRowerExtnCharge.ClientID + "', '"
                    + hfTaxValue.ClientID + "', '" + hfTaxCount.ClientID + "', '" + txtBtNorExtCharges.ClientID + "', '" + txtTaxBtExtChrg.ClientID + "')");

                txtBtNorExtCharges.Attributes.Add("readonly", "readonly");
                txtTaxBtExtChrg.Attributes.Add("readonly", "readonly");

                txtChildCharge.Attributes.Add("onkeyup", "ShowOtherGSTCalculation('" + txtChildCharge.ClientID + "', '" + hfTaxValue.ClientID + "', '"
                    + hfTaxCount.ClientID + "', '" + txtActChildChrg.ClientID + "', '" + txtChildTaxAmt.ClientID + "')");

                txtActChildChrg.Attributes.Add("readonly", "readonly");
                txtChildTaxAmt.Attributes.Add("readonly", "readonly");
                AutomaticDeleteRateExtn();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void GetTaxDetail()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatSearch = new boatRate()
                {
                    ServiceId = "1",
                    ValidDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                };

                HttpResponseMessage response = client.PostAsJsonAsync("TaxMstr/IdDate", BoatSearch).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Taxd = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(Taxd)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Taxd)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            //hfTaxValue.Value = dt.Rows[0]["TaxName"].ToString().Trim();

                            decimal Tax = 0;
                            string[] taxlist = dt.Rows[0]["TaxName"].ToString().Split(',');

                            foreach (var list in taxlist)
                            {
                                var TaxName = list;
                                var tx = list.Split('-');

                                Tax += Convert.ToDecimal(tx[1].ToString());

                                hfTaxValue.Value = tx[1].ToString();
                            }

                            hfTaxCount.Value = taxlist.Length.ToString();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Tax Details Not Found...!');", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //DropDowns
    public void GetBoatSeat()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatRateMaster = new boatRate()
                {
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BoatRate/Seat", BoatRateMaster).Result;

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
                        //ddlBoatSeatId.DataBind();
                        ddlBoatSeatId.Items.Insert(0, "Select Boat Seater");
                    }
                    else
                    {
                        return;
                        //  lblGridMsg.Text = ResponseMsg;

                    }

                }
                else
                {
                    return;
                    //  lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetBoatSeatAll()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatRateMaster = new boatRate()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("BHMstr/ddlBoatSeater", BoatRateMaster).Result;

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
                        //ddlBoatSeatId.DataBind();
                        ddlBoatSeatId.Items.Insert(0, "Select Boat Seater");
                    }
                    else
                    {
                        return;
                        //  lblGridMsg.Text = ResponseMsg;

                    }

                }
                else
                {
                    return;
                    //  lblGridMsg.Text = response.ToString();
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
                var BoatRateMaster = new boatRate()
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
                        //ddlBoatType.DataBind();
                        ddlBoatType.Items.Insert(0, "Select Boat Type");
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

    public void BindMinute()
    {
        for (int i = 0; i <= 60; i++)
        {
            ListItem item = new ListItem(i.ToString(), i.ToString());
            dlstDurationMin.Items.Add(item);
            dlstGraceTimeMin.Items.Add(item);
            ddlExtFromTime.Items.Add(item);
            ddlExtToTime.Items.Add(item);
        }

        dlstDurationMin.Items.Insert(0, "Select Duration");
        dlstGraceTimeMin.Items.Insert(0, "Select Grace Time");
        ddlExtFromTime.Items.Insert(0, "Select From Time (Min)");
        ddlExtToTime.Items.Insert(0, "Select To Time (Min)");
    }

    public void Clear()
    {
        ddlBoatHouse.SelectedIndex = 0;
        dlstDurationMin.SelectedIndex = 0;
        dlstGraceTimeMin.SelectedIndex = 0;

        txtBoatMinCharge.Text = "0";
        txtBtNorCharges.Text = "0";
        txtRowerMinCharge.Text = "0";
        txtTaxBtNorChrg.Text = "0";

        txtBoatPrmMin.Text = "0";
        txtBtPremCharge.Text = "0";
        txtRowerPrmMin.Text = "0";
        txtTaxBtPremChrg.Text = "0";

        txtFixedAmount.Text = "0";
        txtPercentage.Text = "0";

        hfImageCheckValue.Value = "0";
        hfPrevImageLink.Value = "";
        txtTripPerday.Text = string.Empty;
        imgBtRtPrev.ImageUrl = "../images/FileUpload.png";
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;

        ddlBoatType.Enabled = true;
        ddlBoatSeatId.Enabled = true;

        rblDepositType.SelectedValue = "P";
        divPercentage.Attributes.Add("style", "display:block;");
        divFixedAmount.Attributes.Add("style", "display:none;");

        ChkChildApplicable.Checked = false;
        txtChildApp.Text = "";
        txtChildCharge.Text = "0";
        txtActChildChrg.Text = "0";
        txtChildTaxAmt.Text = "0";

        txtDisplayOrder.Text = "";
        divChildInfantAge.Attributes.Add("style", "display:none;");
        divChild.Attributes.Add("style", "display:none;");
        divActChrg.Attributes.Add("style", "display:none;");
        divChildTax.Attributes.Add("style", "display:none;");

        txtWkEdAmt.Text = "0";
        txtWkEdChrge.Text = "0";
        txtWkEdPamt.Text = "0";
        TxtWkEdToTax.Text = "0";

        txtIWDAmt.Text = "0";
        txtIWDBtChrg.Text = "0";
        txtIWdPaymt.Text = "0";
        txtIWdToTax.Text = "0";

        txtIWEAmt.Text = "0";
        txtIWEBtChrg.Text = "0";
        txtIWEPaymt.Text = "0";
        txtIWEToTax.Text = "0";

        rblSelfdrive.SelectedValue = "N";
        rblTimeExtension.SelectedValue = "N";

        ChkSingleAllwd.Checked = false;
        txtIWDAmt.Text = "0";
        txtIWDBtChrg.Text = "0";
        txtIWdPaymt.Text = "0";
        txtIWdToTax.Text = "0";

        txtIWEAmt.Text = "0";
        txtIWEBtChrg.Text = "0";
        txtIWEPaymt.Text = "0";
        txtIWEToTax.Text = "0";

        txtDisplayOrder.Text = "";
        divChildInfantAge.Attributes.Add("style", "display:none;");
        divChild.Attributes.Add("style", "display:none;");
        divActChrg.Attributes.Add("style", "display:none;");
        divChildTax.Attributes.Add("style", "display:none;");


        txtBoatMinCharge.Enabled = true;
        txtRowerMinCharge.Enabled = true;
        txtBoatPrmMin.Enabled = true;
        txtRowerPrmMin.Enabled = true;

        txtWkEdAmt.Enabled = true;
        txtWkEdPamt.Enabled = true;
        txtIWDAmt.Enabled = true;
        txtIWdPaymt.Enabled = true;
        txtIWEAmt.Enabled = true;
        txtIWEPaymt.Enabled = true;


        gvBoatRateExtnChrg.DataBind();
    }

    public void ClearBoatRateExtn()
    {
        ddlExtFromTime.SelectedIndex = -1;
        ddlExtToTime.SelectedIndex = -1;

        rblAmountType.SelectedIndex = 0;
        txtAmtPer.Text = string.Empty;
        txtBtAmt.Text = string.Empty;
        txtRwAmt.Text = string.Empty;

        txtBoatExtnCharge.Text = "";
        txtRowerExtnCharge.Text = "";
        txtBtNorExtCharges.Text = "";
        txtTaxBtExtChrg.Text = "";

        hfUniqueId.Value = string.Empty;
        btnAdd.Text = "Add";

        divAmtPer.Attributes.Add("style", "display:block;");
        divbtAmt.Attributes.Add("style", "display:block;");
        divrwAmt.Attributes.Add("style", "display:block;");
    }

    //Showing Records
    public void BindBoatRate()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new boatRate()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatRateMstr/BHId", BoatHouseId).Result;

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
                            gvBoatRate.DataSource = dt;
                            gvBoatRate.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            btnExportToPdf.Visible = true;
                        }
                        else
                        {
                            gvBoatRate.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divGrid.Visible = false;
                            divEntry.Visible = true;
                            lbtnNew.Visible = false;
                            btnExportToPdf.Visible = false;
                        }

                    }
                    else
                    {
                        btnExportToPdf.Visible = false;
                        lblGridMsg.Text = ResponseMsg;
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;

                    }
                }
                else
                {
                    return;
                    //lblGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //Showing Records
    public void BindBoatRateExtnChrg()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new boatRate()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeatId.SelectedValue.Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatRateExtnChrg/BHId", BoatHouseId).Result;

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
                            gvBoatRateExtnChrg.DataSource = dt;
                            gvBoatRateExtnChrg.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divModalGrid.Visible = true;
                            divExtn.Attributes.Add("style", "display:block;");


                            txtBoatMinCharge.Enabled = false;
                            txtRowerMinCharge.Enabled = false;
                            txtBoatPrmMin.Enabled = false;
                            txtRowerPrmMin.Enabled = false;
                        }
                        else
                        {
                            gvBoatRateExtnChrg.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divModalGrid.Visible = false;
                            divExtn.Attributes.Add("style", "display:block;");
                            txtBoatMinCharge.Enabled = true;
                            txtRowerMinCharge.Enabled = true;
                            txtBoatPrmMin.Enabled = true;
                            txtRowerPrmMin.Enabled = true;
                        }

                    }
                    else
                    {
                        lblGridMsg.Text = "No Records Found";
                        divModalGrid.Visible = false;
                        divExtn.Attributes.Add("style", "display:block;");
                        txtBoatMinCharge.Enabled = true;
                        txtRowerMinCharge.Enabled = true;
                        txtBoatPrmMin.Enabled = true;
                        txtRowerPrmMin.Enabled = true;
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

    public void BindBoatExtn()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new boatRate()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                    BoatSeaterId = ddlBoatSeatId.SelectedValue.Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatRateExtnChrg/BHId", BoatHouseId).Result;

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
                            gvExtn.DataSource = dt;
                            gvExtn.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divExtnGrid.Visible = true;
                            divExtn.Attributes.Add("style", "display:block;");
                            txtBoatMinCharge.Enabled = false;
                            txtRowerMinCharge.Enabled = false;
                            txtBoatPrmMin.Enabled = false;
                            txtRowerPrmMin.Enabled = false;
                            //txtWkEdAmt.Enabled = false;
                            //txtWkEdPamt.Enabled = false;
                        }
                        else
                        {
                            gvExtn.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divExtnGrid.Visible = false;
                            divExtn.Attributes.Add("style", "display:block;");
                            txtBoatMinCharge.Enabled = true;
                            txtRowerMinCharge.Enabled = true;
                            txtBoatPrmMin.Enabled = true;
                            txtRowerPrmMin.Enabled = true;
                            txtWkEdAmt.Enabled = true;
                            txtWkEdPamt.Enabled = true;
                        }

                    }
                    else
                    {
                        lblGridMsg.Text = "No Records Found";
                        divExtnGrid.Visible = false;
                        divExtn.Attributes.Add("style", "display:block;");
                        txtBoatMinCharge.Enabled = true;
                        txtRowerMinCharge.Enabled = true;
                        txtBoatPrmMin.Enabled = true;
                        txtRowerPrmMin.Enabled = true;
                        txtWkEdAmt.Enabled = true;
                        txtWkEdPamt.Enabled = true;
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

    public void AutomaticDeleteRateExtn()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var boatrate = new BoatRateExtnCharge()
                {
                    BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString().Trim())
                };
                HttpResponseMessage response = client.PostAsJsonAsync("AutomaticDeleteBoatRate", boatrate).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //New Button
    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        divExtnGrid.Visible = false;
        divModalGrid.Visible = false;
        AutomaticDeleteRateExtn();
        lbtnNew.Visible = false;
        btnSubmit.Text = "Submit";
        GetBoatSeat();
        GetBoatType();
        ddlBoatType.Enabled = true;
        ddlBoatSeatId.Enabled = true;
        btnExportToPdf.Visible = false;
        divChildInfantAge.Attributes.Add("style", "display:none;");
        divChild.Attributes.Add("style", "display:none;");
        divActChrg.Attributes.Add("style", "display:none;");
        divChildTax.Attributes.Add("style", "display:none;");

        divAmtPer.Attributes.Add("style", "display:block;");
        divbtAmt.Attributes.Add("style", "display:block;");
        divrwAmt.Attributes.Add("style", "display:block;");

        divExtn.Attributes.Add("style", "display:none;");
        //divAmtPer.Attributes.Add("style", "display:none;");
        //divbtAmt.Attributes.Add("style", "display:none;");
        //divrwAmt.Attributes.Add("style", "display:none;");

        //lblsa.Attributes.Add("style", "display:none;");
        divSAcharge.Attributes.Add("style", "display:none;");
        DivSAWeekday.Attributes.Add("style", "display:none;");
        DivSAWeekEnd.Attributes.Add("style", "display:none;");

    }

    protected void ddlBoatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBoatSeat();
    }
    static bool IsValidImage(string filePath)
    {
        return IsValidImage(new FileStream(filePath, FileMode.Open, FileAccess.Read));
    }

    static bool IsValidImage(Stream imageStream)
    {
        if (imageStream.Length > 0)
        {
            byte[] header = new byte[6]; // Change size if needed. 
            string[] imageHeaders = new[]{
                "\xFF\xD8", // JPEG 
                "BM",       // BMP 
                "GIF",      // GIF 
             Encoding.ASCII.GetString(new byte[]{137, 80, 78, 71}), // PNG 
             Encoding.ASCII.GetString(new byte[] { 255, 216, 255, 224}), // jpeg 
             Encoding.ASCII.GetString(new byte[] { 255, 216, 255, 225 }), // jpeg cannon 
             Encoding.ASCII.GetString(new byte[] { 77, 77, 42 }) }; // tiff 

            imageStream.Read(header, 0, header.Length);

            bool isImageHeader = imageHeaders.Count(str => Encoding.ASCII.GetString(header).StartsWith(str)) > 0;
            if (isImageHeader == true)
            {
                try
                {
                    //Image.FromStream(imageStream).Dispose();
                    imageStream.Close();
                    return true;
                }

                catch
                {

                }
            }
        }

        imageStream.Close();
        return false;
    }
    public void ImageUploadAPI(string QueryType, string BoatHouseId, string FormName, string PrevImageLink)
    {
        string destinationPath = string.Empty;
        try
        {
            var ImageUpload = new ImageUpload()
            {
                QueryType = QueryType.Trim(),
                BoatHouseId = BoatHouseId.Trim(),
                FormName = FormName.Trim(),
                PrevImageLink = PrevImageLink
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                string strMappath = "~/ImgUpload/";
                string dirMapPath = Server.MapPath(strMappath);

                if (!Directory.Exists(dirMapPath))
                {
                    Directory.CreateDirectory(dirMapPath);
                }

                string tString = System.DateTime.Now.ToString("yyyyMMddHHmmssss");
                Random generator = new Random();
                String rString = generator.Next(0, 999999).ToString("D6");
                string NewFileName = tString.Trim() + "" + rString.Trim();

                string file = string.Empty;

                if (btnSubmit.Text == "Submit")
                {
                    file = fupBtRtLink.FileName;
                }
                else
                {
                    if (hfPrevImageLink.Value != "")
                    {
                        file = Path.GetFileName(hfPrevImageLink.Value.Trim());
                    }
                    else
                    {
                        file = fupBtRtLink.FileName;
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(file) + Path.GetExtension(file);
                destinationPath = dirMapPath + ImageUpload.ImageLink + NewFileName + Path.GetExtension(file);
                fupBtRtLink.SaveAs(destinationPath);
                ImageUpload.ImageLink = destinationPath;
                bool dt = IsValidImage(ImageUpload.ImageLink);
                if (dt == true)
                {
                    MultipartFormDataContent content = new MultipartFormDataContent();

                    var values = new[]
                    {
                    new KeyValuePair<string, string>("QueryType", ImageUpload.QueryType),
                    new KeyValuePair<string, string>("BoatHouseId", ImageUpload.BoatHouseId),
                    new KeyValuePair<string, string>("FormName", ImageUpload.FormName),
                    new KeyValuePair<string, string>("PrevImageLink", ImageUpload.PrevImageLink)
                };

                    foreach (var keyValuePair in values)
                    {
                        content.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    var fileContent1 = new ByteArrayContent(System.IO.File.ReadAllBytes(ImageUpload.ImageLink));

                    fileContent1.Headers.ContentDisposition =
                        new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "ImageLink",
                            FileName = ImageUpload.ImageLink,
                        };
                    content.Add(fileContent1);

                    HttpResponseMessage response = client.PostAsync("ImageAPI", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var Postresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(Postresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(Postresponse)["Response"].ToString();

                        if (StatusCode == 1)
                        {
                            hfResponse.Value = ResponseMsg.Trim();

                            if (File.Exists(destinationPath))
                            {
                                File.Delete(destinationPath);
                            }
                        }
                    }
                }
                else
                {
                    if (file == "")
                    {

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Your File Is Corrupted,Please Upload another File');", true);
                        return;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
        finally
        {
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            if (rblTimeExtension.SelectedValue == "A")
            {
                if (gvBoatRateExtnChrg.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(txtBoatMinCharge.Text.Trim()) > 0)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            string FilePath = string.Empty;
                            string a = string.Empty;
                            string QueryType = string.Empty;
                            if (rblDepositType.SelectedValue == "P")
                            {
                                a = txtPercentage.Text.Trim();
                            }
                            else if (rblDepositType.SelectedValue == "F")
                            {
                                a = txtFixedAmount.Text.Trim();
                            }
                            else
                            {
                                rblDepositType.Text = null;
                            }
                            if (btnSubmit.Text.Trim() == "Submit")
                            {
                                QueryType = "Insert";
                                ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "BRM", "");
                            }
                            else
                            {
                                QueryType = "Update";

                                if (hfImageCheckValue.Value == "1")
                                {
                                    if (hfPrevImageLink.Value.Trim() == "")
                                    {
                                        ImageUploadAPI("Insert", Session["BoatHouseId"].ToString().Trim(), "BRM", "");
                                    }
                                    else
                                    {
                                        ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "BRM", hfPrevImageLink.Value.Trim());
                                    }
                                }
                                else
                                {
                                    hfResponse.Value = hfPrevImageLink.Value.Trim();
                                }
                            }


                            HttpResponseMessage response;
                            string sMSG = string.Empty;

                            string Child, ChargeChild, CheckValue;
                            string IWeekDayAmt = string.Empty;
                            string IWeekDayBtCharge = string.Empty;
                            string IweekDayPayMent = string.Empty;
                            string IWeekDayToTax = string.Empty;
                            string IWeekEndAmt = string.Empty;
                            string IWeekEndBtCharge = string.Empty;
                            string IweekEndPayMent = string.Empty;
                            string IWeekEndToTax = string.Empty;
                            string PerHeadCheck = string.Empty;


                            if (ChkChildApplicable.Checked == true)
                            {
                                CheckValue = "Y";
                                Child = txtChildApp.Text.Trim();
                                ChargeChild = txtChildCharge.Text.Trim();
                            }
                            else
                            {
                                CheckValue = "N";
                                Child = "0";
                                ChargeChild = "0";
                                txtActChildChrg.Text = "0";
                                txtChildTaxAmt.Text = "0";
                            }
                            if (ChkSingleAllwd.Checked == true)
                            {
                                PerHeadCheck = "Y";
                                IWeekDayAmt = txtIWDAmt.Text.Trim();
                                IWeekDayBtCharge = txtIWDBtChrg.Text.Trim();
                                IweekDayPayMent = txtIWdPaymt.Text.Trim();
                                IWeekDayToTax = txtIWdToTax.Text.Trim();

                                IWeekEndAmt = txtIWEAmt.Text.Trim();
                                IWeekEndBtCharge = txtIWEBtChrg.Text.Trim();
                                IweekEndPayMent = txtIWEPaymt.Text.Trim();
                                IWeekEndToTax = txtIWEToTax.Text.Trim();
                            }
                            else
                            {
                                PerHeadCheck = "N";
                                IWeekDayAmt = "0";
                                IWeekDayBtCharge = "0";
                                IweekDayPayMent = "0";
                                IWeekDayToTax = "0";

                                IWeekEndAmt = "0";
                                IWeekEndBtCharge = "0";
                                IweekEndPayMent = "0";
                                IWeekEndToTax = "0";
                            }
                            var BoatRate = new boatRate()
                            {
                                QueryType = QueryType,
                                BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                                BoatSeaterId = ddlBoatSeatId.SelectedValue.Trim(),
                                BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                                BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                                SelfDrive = rblSelfdrive.SelectedValue.Trim(),
                                TimeExtension = rblTimeExtension.SelectedValue.Trim(),
                                BoatImageLink = hfResponse.Value,
                                DepositType = rblDepositType.SelectedValue.Trim(),
                                Deposit = a,
                                MaxTripsPerDay = txtTripPerday.Text.Trim(),
                                BoatMinTime = dlstDurationMin.SelectedValue.Trim(),
                                BoatGraceTime = dlstGraceTimeMin.SelectedValue.Trim(),

                                PerHeadApplicable = PerHeadCheck.ToString().Trim(),

                                BoatMinTotAmt = txtBoatMinCharge.Text.Trim(),
                                BoatMinCharge = txtBtNorCharges.Text.Trim(),
                                RowerMinCharge = txtRowerMinCharge.Text.Trim(),
                                BoatMinTaxAmt = txtTaxBtNorChrg.Text.Trim(),

                                WEBoatMinTotAmt = txtWkEdAmt.Text.Trim(),
                                WEBoatMinCharge = txtWkEdChrge.Text.Trim(),
                                WERowerMinCharge = txtWkEdPamt.Text.Trim(),
                                WEBoatMinTaxAmt = TxtWkEdToTax.Text.Trim(),

                                IWDBoatMinTotAmt = IWeekDayAmt.ToString().Trim(),
                                IWDBoatMinCharge = IWeekDayBtCharge.ToString().Trim(),
                                IWDRowerMinCharge = IweekDayPayMent.ToString().Trim(),
                                IWDBoatMinTaxAmt = IWeekDayToTax.ToString().Trim(),

                                IWEBoatMinTotAmt = IWeekEndAmt.ToString().Trim(),
                                IWEBoatMinCharge = IWeekEndBtCharge.ToString().Trim(),
                                IWERowerMinCharge = IweekEndPayMent.ToString().Trim(),
                                IWEBoatMinTaxAmt = IWeekEndToTax.ToString().Trim(),

                                BoatPremTotAmt = txtBoatPrmMin.Text.Trim(),
                                BoatPremMinCharge = txtBtPremCharge.Text.Trim(),
                                RowerPremMinCharge = txtRowerPrmMin.Text.Trim(),
                                BoatPremTaxAmt = txtTaxBtPremChrg.Text.Trim(),

                                ChildApplicable = CheckValue.Trim(),
                                NoofChildApplicable = Child,
                                ChargePerChild = ChargeChild.Trim(),
                                ChargePerChildTotAmt = txtActChildChrg.Text.Trim(),
                                ChargePerChildTaxAmt = txtChildTaxAmt.Text.Trim(),

                                Createdby = hfCreatedBy.Value.Trim(),
                                DisplayOrder = txtDisplayOrder.Text

                            };

                            response = client.PostAsJsonAsync("BoatRateMstr", BoatRate).Result;



                            if (response.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                                if (StatusCode == 1)
                                {
                                    if (btnSubmit.Text.Trim() == "Submit")
                                    {
                                        BindBoatRate();
                                        Clear();
                                        btnExportToPdf.Visible = true;
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                                    }
                                    else
                                    {
                                        BindBoatRate();
                                        Clear();
                                        sMSG = "Boat Rate Details Updated Successfully";
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                                        btnSubmit.Text = "Submit";
                                        btnExportToPdf.Visible = true;
                                    }

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Rate Details Already Exists.');", true);
                                }
                            }
                            else
                            {
                                //  lblGridMsg.Text = response.ToString();
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Must Enter Boat Normal Charge Amount.!');", true);
                    }
                }
                else
                {
                    if (rblTimeExtension.SelectedValue == "A")
                    {
                        if ((Convert.ToDecimal(txtBoatMinCharge.Text.Trim()) > 0) || ((Convert.ToDecimal(txtBoatPrmMin.Text.Trim()) > 0)) || ((Convert.ToDecimal(txtWkEdAmt.Text.Trim()) > 0)))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Must Enter Extension Charge Amount.!');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showExtnType();", true);
                            divExtn.Attributes.Add("style", "display:block;");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Must Enter Week Day Tariff or Week End Tariff or Express Charge Amount.!');", true);
                            divExtn.Attributes.Add("style", "display:none;");
                        }
                    }
                }
            }
            else
            {
                if (Convert.ToDecimal(txtBoatMinCharge.Text.Trim()) > 0)
                {
                    if (txtIWDAmt.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Select Single Allowed');", true);
                        return;

                    }
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string FilePath = string.Empty;
                        string a = string.Empty;
                        string QueryType = string.Empty;
                        if (rblDepositType.SelectedValue == "P")
                        {
                            a = txtPercentage.Text.Trim();
                        }
                        else if (rblDepositType.SelectedValue == "F")
                        {
                            a = txtFixedAmount.Text.Trim();
                        }
                        else
                        {
                            rblDepositType.Text = null;
                        }


                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            QueryType = "Insert";
                            ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "BRM", "");
                        }
                        else
                        {
                            QueryType = "Update";

                            if (hfImageCheckValue.Value == "1")
                            {
                                if (hfPrevImageLink.Value.Trim() == "")
                                {
                                    ImageUploadAPI("Insert", Session["BoatHouseId"].ToString().Trim(), "BRM", "");
                                }
                                else
                                {
                                    ImageUploadAPI(QueryType, Session["BoatHouseId"].ToString().Trim(), "BRM", hfPrevImageLink.Value.Trim());
                                }
                            }
                            else
                            {
                                hfResponse.Value = hfPrevImageLink.Value.Trim();
                            }
                        }


                        HttpResponseMessage response;
                        string sMSG = string.Empty;

                        string Child, ChargeChild, CheckValue;
                        string IWeekDayAmt = string.Empty;
                        string IWeekDayBtCharge = string.Empty;
                        string IweekDayPayMent = string.Empty;
                        string IWeekDayToTax = string.Empty;
                        string IWeekEndAmt = string.Empty;
                        string IWeekEndBtCharge = string.Empty;
                        string IweekEndPayMent = string.Empty;
                        string IWeekEndToTax = string.Empty;
                        string PerHeadCheck = string.Empty;

                        if (ChkChildApplicable.Checked == true)
                        {
                            CheckValue = "Y";
                            Child = txtChildApp.Text.Trim();
                            ChargeChild = txtChildCharge.Text.Trim();
                        }
                        else
                        {
                            CheckValue = "N";
                            Child = "0";
                            ChargeChild = "0";
                            txtActChildChrg.Text = "0";
                            txtChildTaxAmt.Text = "0";
                        }
                        if (ChkSingleAllwd.Checked == true)
                        {
                            PerHeadCheck = "Y";
                            IWeekDayAmt = txtIWDAmt.Text.Trim();
                            IWeekDayBtCharge = txtIWDBtChrg.Text.Trim();
                            IweekDayPayMent = txtIWdPaymt.Text.Trim();
                            IWeekDayToTax = txtIWdToTax.Text.Trim();

                            IWeekEndAmt = txtIWEAmt.Text.Trim();
                            IWeekEndBtCharge = txtIWEBtChrg.Text.Trim();
                            IweekEndPayMent = txtIWEPaymt.Text.Trim();
                            IWeekEndToTax = txtIWEToTax.Text.Trim();
                        }
                        else
                        {
                            PerHeadCheck = "N";
                            IWeekDayAmt = "0";
                            IWeekDayBtCharge = "0";
                            IweekDayPayMent = "0";
                            IWeekDayToTax = "0";

                            IWeekEndAmt = "0";
                            IWeekEndBtCharge = "0";
                            IweekEndPayMent = "0";
                            IWeekEndToTax = "0";
                        }


                        var BoatRate = new boatRate()
                        {
                            QueryType = QueryType,
                            BoatTypeId = ddlBoatType.SelectedValue.Trim(),
                            BoatSeaterId = ddlBoatSeatId.SelectedValue.Trim(),
                            BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                            BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                            SelfDrive = rblSelfdrive.SelectedValue.Trim(),
                            TimeExtension = rblTimeExtension.SelectedValue.Trim(),
                            BoatImageLink = hfResponse.Value,
                            DepositType = rblDepositType.SelectedValue.Trim(),
                            Deposit = a,
                            MaxTripsPerDay = txtTripPerday.Text.Trim(),
                            BoatMinTime = dlstDurationMin.SelectedValue.Trim(),
                            BoatGraceTime = dlstGraceTimeMin.SelectedValue.Trim(),
                            PerHeadApplicable = PerHeadCheck.ToString().Trim(),

                            BoatMinTotAmt = txtBoatMinCharge.Text.Trim(),
                            BoatMinCharge = txtBtNorCharges.Text.Trim(),
                            RowerMinCharge = txtRowerMinCharge.Text.Trim(),
                            BoatMinTaxAmt = txtTaxBtNorChrg.Text.Trim(),


                            WEBoatMinTotAmt = txtWkEdAmt.Text.Trim(),
                            WEBoatMinCharge = txtWkEdChrge.Text.Trim(),
                            WERowerMinCharge = txtWkEdPamt.Text.Trim(),
                            WEBoatMinTaxAmt = TxtWkEdToTax.Text.Trim(),


                            IWDBoatMinTotAmt = IWeekDayAmt.ToString().Trim(),
                            IWDBoatMinCharge = IWeekDayBtCharge.ToString().Trim(),
                            IWDRowerMinCharge = IweekDayPayMent.ToString().Trim(),
                            IWDBoatMinTaxAmt = IWeekDayToTax.ToString().Trim(),

                            IWEBoatMinTotAmt = IWeekEndAmt.ToString().Trim(),
                            IWEBoatMinCharge = IWeekEndBtCharge.ToString().Trim(),
                            IWERowerMinCharge = IweekEndPayMent.ToString().Trim(),
                            IWEBoatMinTaxAmt = IWeekEndToTax.ToString().Trim(),

                            BoatPremTotAmt = txtBoatPrmMin.Text.Trim(),
                            BoatPremMinCharge = txtBtPremCharge.Text.Trim(),
                            RowerPremMinCharge = txtRowerPrmMin.Text.Trim(),
                            BoatPremTaxAmt = txtTaxBtPremChrg.Text.Trim(),

                            ChildApplicable = CheckValue.Trim(),
                            NoofChildApplicable = Child,
                            ChargePerChild = ChargeChild.Trim(),
                            ChargePerChildTotAmt = txtActChildChrg.Text.Trim(),
                            ChargePerChildTaxAmt = txtChildTaxAmt.Text.Trim(),

                            Createdby = hfCreatedBy.Value.Trim(),
                            DisplayOrder = txtDisplayOrder.Text

                        };

                        response = client.PostAsJsonAsync("BoatRateMstr", BoatRate).Result;



                        if (response.IsSuccessStatusCode)
                        {
                            var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                if (btnSubmit.Text.Trim() == "Submit")
                                {
                                    BindBoatRate();
                                    Clear();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                                    btnExportToPdf.Visible = true;

                                }
                                else
                                {
                                    BindBoatRate();
                                    Clear();
                                    sMSG = "Boat Rate Details Updated Successfully";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                                    btnSubmit.Text = "Submit";
                                    btnExportToPdf.Visible = true;
                                }

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Rate Details Already Exists.');", true);
                            }
                        }
                        else
                        {
                            //  lblGridMsg.Text = response.ToString();
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Must Enter Boat Normal Charge Amount.!');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    //Cancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        BindBoatRate();
        AutomaticDeleteRateExtn();
        btnExportToPdf.Visible = true;
    }
    //Edit
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            btnExportToPdf.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string BoatTypeId = gvBoatRate.DataKeys[gvrow.RowIndex].Value.ToString();
            GetBoatType();
            ddlBoatType.SelectedValue = ((Label)gvrow.FindControl("lblBoatTypeId")).Text;
            GetBoatSeatAll();
            ddlBoatSeatId.SelectedValue = ((Label)gvrow.FindControl("lblBoatSeaterId")).Text;

            rblDepositType.SelectedValue = ((Label)gvrow.FindControl("lblDepositType")).Text;


            if (rblDepositType.SelectedValue == "P")
            {
                divPercentage.Attributes.Add("style", "display:block;");
                divFixedAmount.Attributes.Add("style", "display:none;");
            }
            else
            {
                divPercentage.Attributes.Add("style", "display:none;");
                divFixedAmount.Attributes.Add("style", "display:block;");
            }

            txtPercentage.Text = ((Label)gvrow.FindControl("lbldeposit")).Text;
            txtFixedAmount.Text = ((Label)gvrow.FindControl("lbldeposit")).Text;
            txtTripPerday.Text = ((Label)gvrow.FindControl("lblmaxTripsPerDay")).Text;

            dlstDurationMin.SelectedValue = ((Label)gvrow.FindControl("lblBoatMinTime")).Text.Trim();
            dlstGraceTimeMin.SelectedValue = ((Label)gvrow.FindControl("lblboatgracetime")).Text;

            txtBoatMinCharge.Text = ((Label)gvrow.FindControl("lblBoatMinTotAmt")).Text;
            txtBtNorCharges.Text = ((Label)gvrow.FindControl("lblBoatMinCharge")).Text;
            txtRowerMinCharge.Text = ((Label)gvrow.FindControl("lblRowerMinCharge")).Text;
            txtTaxBtNorChrg.Text = ((Label)gvrow.FindControl("lblBoatMinTaxAmt")).Text;

            txtBoatPrmMin.Text = ((Label)gvrow.FindControl("lblBoatPremTotAmt")).Text;
            txtBtPremCharge.Text = ((Label)gvrow.FindControl("lblBoatPremMinCharge")).Text;
            txtRowerPrmMin.Text = ((Label)gvrow.FindControl("lblRowerPremMinCharge")).Text;
            txtTaxBtPremChrg.Text = ((Label)gvrow.FindControl("lblBoatPremTaxAmt")).Text;

            txtWkEdAmt.Text = ((Label)gvrow.FindControl("lblWEBoatMinTotAmt")).Text;
            txtWkEdChrge.Text = ((Label)gvrow.FindControl("lblWEBoatMinCharge")).Text;
            txtWkEdPamt.Text = ((Label)gvrow.FindControl("lblWERowerMinCharge")).Text;
            TxtWkEdToTax.Text = ((Label)gvrow.FindControl("lblWEBoatMinTaxAmt")).Text;

            txtDisplayOrder.Text = ((Label)gvrow.FindControl("lblDisplayOrder")).Text;
            Label ChildApplicable = (Label)gvrow.FindControl("lblChildApplicable");
            Label NoofChildApplicable = (Label)gvrow.FindControl("lblNoofChildApplicable");
            Label ChargePerChild = (Label)gvrow.FindControl("lblChargePerChild");
            Label ChargePerChildTotAmt = (Label)gvrow.FindControl("lblChargePerChildTotAmt");
            Label ChargePerChildTaxAmt = (Label)gvrow.FindControl("lblChargePerChildTaxAmt");

            Label PerHeadApplicable = (Label)gvrow.FindControl("lblPerHeadApplicable");
            Label IWDBoatMinTotAmt = (Label)gvrow.FindControl("lblIWDBoatMinTotAmt");
            Label IWDBoatMinCharge = (Label)gvrow.FindControl("lblIWDBoatMinCharge");
            Label IWDRowerMinCharge = (Label)gvrow.FindControl("lblIWDRowerMinCharge");
            Label IWDBoatMinTaxAmt = (Label)gvrow.FindControl("lblIWDBoatMinTaxAmt");


            Label IWEBoatMinTotAmt = (Label)gvrow.FindControl("lblIWEBoatMinTotAmt");
            Label IWEBoatMinCharge = (Label)gvrow.FindControl("lblIWEBoatMinCharge");
            Label IWERowerMinCharge = (Label)gvrow.FindControl("lblIWERowerMinCharge");
            Label IWEBoatMinTaxAmt = (Label)gvrow.FindControl("lblIWEBoatMinTaxAmt");

            if (ChildApplicable.Text == "N")
            {
                ChkChildApplicable.Checked = false;
                txtChildApp.Text = "";
                divChildInfantAge.Attributes.Add("style", "display:none;");
                divChild.Attributes.Add("style", "display:none;");
                divActChrg.Attributes.Add("style", "display:none;");
                divChildTax.Attributes.Add("style", "display:none;");
            }
            else
            {
                ChkChildApplicable.Checked = true;
                txtChildApp.Text = NoofChildApplicable.Text;
                divChildInfantAge.Attributes.Add("style", "display:block;");
                divChild.Attributes.Add("style", "display:block;");
                divActChrg.Attributes.Add("style", "display:block;");
                divChildTax.Attributes.Add("style", "display:block;");
            }
            txtChildCharge.Text = ChargePerChild.Text;
            txtActChildChrg.Text = ChargePerChildTotAmt.Text;
            txtChildTaxAmt.Text = ChargePerChildTaxAmt.Text;


            if (PerHeadApplicable.Text == "N" || PerHeadApplicable.Text == "")
            {
                ChkSingleAllwd.Checked = false;

                divSAcharge.Attributes.Add("style", "display:none;");
                DivSAWeekday.Attributes.Add("style", "display:none;");
                DivSAWeekEnd.Attributes.Add("style", "display:none;");
                lblsa.Attributes.Add("style", "display:none;");
            }
            else
            {

                ChkSingleAllwd.Checked = true;
                divSAcharge.Attributes.Add("style", "display:block;");
                DivSAWeekday.Attributes.Add("style", "display:block;");
                DivSAWeekEnd.Attributes.Add("style", "display:block;");
                lblsa.Attributes.Add("style", "display:block;");
            }
            txtIWDAmt.Text = IWDBoatMinTotAmt.Text;
            txtIWDBtChrg.Text = IWDBoatMinCharge.Text;
            txtIWdPaymt.Text = IWDRowerMinCharge.Text;
            txtIWdToTax.Text = IWDBoatMinTaxAmt.Text;

            txtIWEAmt.Text = IWEBoatMinTotAmt.Text;
            txtIWEBtChrg.Text = IWEBoatMinCharge.Text;
            txtIWEPaymt.Text = IWERowerMinCharge.Text;
            txtIWEToTax.Text = IWEBoatMinTaxAmt.Text;


            imgBtRtPrev.ImageUrl = ((Label)gvrow.FindControl("lblBoatImageLink")).Text.Trim();
            hfPrevImageLink.Value = ((Label)gvrow.FindControl("lblBoatImageLink")).Text.Trim();

            if (imgBtRtPrev.ImageUrl == "")
            {
                imgBtRtPrev.ImageUrl = "../images/FileUpload.png";
            }



            Label lblSelfDrive = (Label)gvrow.FindControl("lblSelfDrive");

            if (lblSelfDrive.Text.Trim() == "A" || lblSelfDrive.Text.Trim() == "Allowed")
            {
                rblSelfdrive.SelectedValue = "A";
            }

            Label lblTimeExtension = (Label)gvrow.FindControl("lblTimeExtension");

            ddlBoatType.Enabled = false;
            ddlBoatSeatId.Enabled = false;
            BindBoatRateExtnChrg();
            BindBoatExtn();

            if (lblTimeExtension.Text.Trim() == "A" || lblTimeExtension.Text.Trim() == "Allowed")
            {
                rblTimeExtension.SelectedValue = "A";
                divExtn.Attributes.Add("style", "display:block !important;");
                lblsa.Attributes.Add("style", "display:none !important;");
                Label8.Attributes.Add("style", "display:none !important;");
                divSAcharge.Attributes.Add("style", "display:none !important;");
                DivSAWeekEnd.Attributes.Add("style", "display:none !important;");
                DivSAWeekday.Attributes.Add("style", "display:none !important;");
                txtIWDAmt.Text = "0";
                txtIWDBtChrg.Text = "0";
                txtIWdPaymt.Text = "0";
                txtIWdToTax.Text = "0";

                txtIWEAmt.Text = "0";
                txtIWEBtChrg.Text = "0";
                txtIWEPaymt.Text = "0";
                txtIWEToTax.Text = "0";


            }
            else
            {
                Label8.Attributes.Add("style", "display:block !important;");
                divExtn.Attributes.Add("style", "display:none !important;");

            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }
    //Delete
    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvBoatRate.DataKeys[gvrow.RowIndex].Value.ToString();
            Label BoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
            Label BoatSeaterId = (Label)gvrow.FindControl("lblBoatSeaterId");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            Label DepositType = (Label)gvrow.FindControl("lblDepositType");
            Label deposit = (Label)gvrow.FindControl("lbldeposit");
            Label SelfDrive = (Label)gvrow.FindControl("lblSelfDrive");
            Label TimeExtension = (Label)gvrow.FindControl("lblTimeExtension");
            Label BoatMinTime = (Label)gvrow.FindControl("lblBoatMinTime");
            Label BoatGraceTime = (Label)gvrow.FindControl("lblboatgracetime");

            Label BoatMinTotCharge = (Label)gvrow.FindControl("lblBoatMinTotAmt");
            Label BoatMinCharge = (Label)gvrow.FindControl("lblBoatMinCharge");
            Label RowerMinCharge = (Label)gvrow.FindControl("lblRowerMinCharge");
            Label BoatMinTaxAmt = (Label)gvrow.FindControl("lblBoatMinTaxAmt");

            Label BoatPremTotAmt = (Label)gvrow.FindControl("lblBoatPremTotAmt");
            Label BoatPremMinCharge = (Label)gvrow.FindControl("lblBoatPremMinCharge");
            Label RowerPremMinCharge = (Label)gvrow.FindControl("lblRowerPremMinCharge");
            Label BoatPremTaxAmt = (Label)gvrow.FindControl("lblBoatPremTaxAmt");


            Label WEBoatMinTotAmt = (Label)gvrow.FindControl("lblWEBoatMinTotAmt");
            Label WEBoatMinCharge = (Label)gvrow.FindControl("lblWEBoatMinCharge");
            Label WERowerMinCharge = (Label)gvrow.FindControl("lblWERowerMinCharge");
            Label WEBoatMinTaxAmt = (Label)gvrow.FindControl("lblWEBoatMinTaxAmt");

            Label MaxTripsPerDay = (Label)gvrow.FindControl("lblmaxTripsPerDay");

            Label ChildApplicable = (Label)gvrow.FindControl("lblChildApplicable");
            Label NoofChildApplicable = (Label)gvrow.FindControl("lblNoofChildApplicable");
            Label ChargePerChild = (Label)gvrow.FindControl("lblChargePerChild");
            Label ChargePerChildTotAmt = (Label)gvrow.FindControl("lblChargePerChildTotAmt");
            Label ChargePerChildTaxAmt = (Label)gvrow.FindControl("lblChargePerChildTaxAmt");
            Label DisplayOrder = (Label)gvrow.FindControl("lblDisplayOrder");

            Label PerHeadApplicable = (Label)gvrow.FindControl("lblPerHeadApplicable");
            Label IWDBoatMinTotAmt = (Label)gvrow.FindControl("lblIWDBoatMinTotAmt");
            Label IWDBoatMinCharge = (Label)gvrow.FindControl("lblIWDBoatMinCharge");
            Label IWDRowerMinCharge = (Label)gvrow.FindControl("lblIWDRowerMinCharge");
            Label IWDBoatMinTaxAmt = (Label)gvrow.FindControl("lblIWDBoatMinTaxAmt");


            Label IWEBoatMinTotAmt = (Label)gvrow.FindControl("lblIWEBoatMinTotAmt");
            Label IWEBoatMinCharge = (Label)gvrow.FindControl("lblIWEBoatMinCharge");
            Label IWERowerMinCharge = (Label)gvrow.FindControl("lblIWERowerMinCharge");
            Label IWEBoatMinTaxAmt = (Label)gvrow.FindControl("lblIWEBoatMinTaxAmt");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var BoatRate = new boatRate()
                {
                    QueryType = "Delete",
                    BoatTypeId = BoatTypeId.Text,
                    BoatSeaterId = BoatSeaterId.Text,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),

                    BoatImageLink = "Test",
                    SelfDrive = SelfDrive.Text,
                    DepositType = DepositType.Text,
                    Deposit = deposit.Text,

                    TimeExtension = TimeExtension.Text,
                    BoatMinTime = BoatMinTime.Text.Trim(),
                    BoatGraceTime = BoatGraceTime.Text.Trim(),

                    PerHeadApplicable = PerHeadApplicable.Text.Trim(),
                    BoatMinTotAmt = BoatMinTotCharge.Text.Trim(),
                    BoatMinCharge = BoatMinCharge.Text.Trim(),
                    RowerMinCharge = RowerMinCharge.Text.Trim(),
                    BoatMinTaxAmt = BoatMinTaxAmt.Text.Trim(),

                    WEBoatMinTotAmt = "0.00",
                    WEBoatMinCharge = "0.00",
                    WERowerMinCharge = "0.00",
                    WEBoatMinTaxAmt = "0.00",



                    IWDBoatMinTotAmt = "0.00",
                    IWDBoatMinCharge = "0.00",
                    IWDRowerMinCharge = "0.00",
                    IWDBoatMinTaxAmt = "0.00",

                    IWEBoatMinTotAmt = "0.00",
                    IWEBoatMinCharge = "0.00",
                    IWERowerMinCharge = "0.00",
                    IWEBoatMinTaxAmt = "0.00",



                    BoatPremTotAmt = BoatPremTotAmt.Text.Trim(),
                    BoatPremMinCharge = BoatPremMinCharge.Text.Trim(),
                    RowerPremMinCharge = RowerPremMinCharge.Text.Trim(),
                    BoatPremTaxAmt = BoatPremTaxAmt.Text.Trim(),


                    MaxTripsPerDay = MaxTripsPerDay.Text.Trim(),

                    ChildApplicable = ChildApplicable.Text.Trim(),
                    NoofChildApplicable = NoofChildApplicable.Text.Trim(),
                    ChargePerChild = ChargePerChild.Text.Trim(),
                    ChargePerChildTotAmt = ChargePerChildTotAmt.Text.Trim(),
                    ChargePerChildTaxAmt = ChargePerChildTaxAmt.Text.Trim(),

                    Createdby = hfCreatedBy.Value.Trim(),
                    DisplayOrder = DisplayOrder.Text.Trim()

                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("BoatRateMstr", BoatRate).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatRate();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        BindBoatRate();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindBoatRate();
                    Clear();
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
            string sTesfg = gvBoatRate.DataKeys[gvrow.RowIndex].Value.ToString();
            Label BoatTypeId = (Label)gvrow.FindControl("lblBoatTypeId");
            Label BoatSeaterId = (Label)gvrow.FindControl("lblBoatSeaterId");
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            Label DepositType = (Label)gvrow.FindControl("lblDepositType");
            Label deposit = (Label)gvrow.FindControl("lbldeposit");
            Label SelfDrive = (Label)gvrow.FindControl("lblSelfDrive");
            Label TimeExtension = (Label)gvrow.FindControl("lblTimeExtension");
            Label BoatMinTime = (Label)gvrow.FindControl("lblBoatMinTime");
            Label BoatGraceTime = (Label)gvrow.FindControl("lblboatgracetime");

            Label BoatMinTotCharge = (Label)gvrow.FindControl("lblBoatMinTotAmt");
            Label BoatMinCharge = (Label)gvrow.FindControl("lblBoatMinCharge");
            Label RowerMinCharge = (Label)gvrow.FindControl("lblRowerMinCharge");
            Label BoatMinTaxAmt = (Label)gvrow.FindControl("lblBoatMinTaxAmt");

            Label BoatPremTotAmt = (Label)gvrow.FindControl("lblBoatPremTotAmt");
            Label BoatPremMinCharge = (Label)gvrow.FindControl("lblBoatPremMinCharge");
            Label RowerPremMinCharge = (Label)gvrow.FindControl("lblRowerPremMinCharge");
            Label BoatPremTaxAmt = (Label)gvrow.FindControl("lblBoatPremTaxAmt");

            Label MaxTripsPerDay = (Label)gvrow.FindControl("lblmaxTripsPerDay");

            Label ChildApplicable = (Label)gvrow.FindControl("lblChildApplicable");
            Label NoofChildApplicable = (Label)gvrow.FindControl("lblNoofChildApplicable");
            Label ChargePerChild = (Label)gvrow.FindControl("lblChargePerChild");
            Label ChargePerChildTotAmt = (Label)gvrow.FindControl("lblChargePerChildTotAmt");
            Label ChargePerChildTaxAmt = (Label)gvrow.FindControl("lblChargePerChildTaxAmt");
            Label DisplayOrder = (Label)gvrow.FindControl("lblDisplayOrder");

            Label WEBoatMinTotAmt = (Label)gvrow.FindControl("lblWEBoatMinTotAmt");
            Label WEBoatMinCharge = (Label)gvrow.FindControl("lblWEBoatMinCharge");
            Label WERowerMinCharge = (Label)gvrow.FindControl("lblWERowerMinCharge");
            Label WEBoatMinTaxAmt = (Label)gvrow.FindControl("lblWEBoatMinTaxAmt");


            Label PerHeadApplicable = (Label)gvrow.FindControl("lblPerHeadApplicable");
            Label IWDBoatMinTotAmt = (Label)gvrow.FindControl("lblIWDBoatMinTotAmt");
            Label IWDBoatMinCharge = (Label)gvrow.FindControl("lblIWDBoatMinCharge");
            Label IWDRowerMinCharge = (Label)gvrow.FindControl("lblIWDRowerMinCharge");
            Label IWDBoatMinTaxAmt = (Label)gvrow.FindControl("lblIWDBoatMinTaxAmt");


            Label IWEBoatMinTotAmt = (Label)gvrow.FindControl("lblIWEBoatMinTotAmt");
            Label IWEBoatMinCharge = (Label)gvrow.FindControl("lblIWEBoatMinCharge");
            Label IWERowerMinCharge = (Label)gvrow.FindControl("lblIWERowerMinCharge");
            Label IWEBoatMinTaxAmt = (Label)gvrow.FindControl("lblIWEBoatMinTaxAmt");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var BoatRate = new boatRate()
                {
                    QueryType = "ReActive",
                    BoatTypeId = BoatTypeId.Text,
                    BoatSeaterId = BoatSeaterId.Text,
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),

                    BoatImageLink = "Test",
                    SelfDrive = SelfDrive.Text,
                    DepositType = DepositType.Text,
                    Deposit = deposit.Text,

                    TimeExtension = TimeExtension.Text,
                    BoatMinTime = BoatMinTime.Text.Trim(),
                    BoatGraceTime = BoatGraceTime.Text.Trim(),
                    PerHeadApplicable = PerHeadApplicable.Text.Trim(),

                    BoatMinTotAmt = BoatMinTotCharge.Text.Trim(),
                    BoatMinCharge = BoatMinCharge.Text.Trim(),
                    RowerMinCharge = RowerMinCharge.Text.Trim(),
                    BoatMinTaxAmt = BoatMinTaxAmt.Text.Trim(),

                    WEBoatMinTotAmt = "0.00",
                    WEBoatMinCharge = "0.00",
                    WERowerMinCharge = "0.00",
                    WEBoatMinTaxAmt = "0.00",



                    IWDBoatMinTotAmt = "0.00",
                    IWDBoatMinCharge = "0.00",
                    IWDRowerMinCharge = "0.00",
                    IWDBoatMinTaxAmt = "0.00",

                    IWEBoatMinTotAmt = "0.00",
                    IWEBoatMinCharge = "0.00",
                    IWERowerMinCharge = "0.00",
                    IWEBoatMinTaxAmt = "0.00",


                    BoatPremTotAmt = BoatPremTotAmt.Text.Trim(),
                    BoatPremMinCharge = BoatPremMinCharge.Text.Trim(),
                    RowerPremMinCharge = RowerPremMinCharge.Text.Trim(),
                    BoatPremTaxAmt = BoatPremTaxAmt.Text.Trim(),

                    MaxTripsPerDay = MaxTripsPerDay.Text.Trim(),

                    ChildApplicable = ChildApplicable.Text.Trim(),
                    NoofChildApplicable = NoofChildApplicable.Text.Trim(),
                    ChargePerChild = ChargePerChild.Text.Trim(),
                    ChargePerChildTotAmt = ChargePerChildTotAmt.Text.Trim(),
                    ChargePerChildTaxAmt = ChargePerChildTaxAmt.Text.Trim(),

                    Createdby = hfCreatedBy.Value.Trim(),
                    DisplayOrder = DisplayOrder.Text.Trim()

                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("BoatRateMstr", BoatRate).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatRate();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        BindBoatRate();
                        Clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindBoatRate();
                    Clear();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void gvBoatRate_RowDataBound(object sender, GridViewRowEventArgs e)
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

    /****************************************Extension Charges*******************************************************/

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDecimal(txtBoatExtnCharge.Text.Trim()) > 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string FilePath = string.Empty;
                    string a = string.Empty;
                    string b = string.Empty;
                    string QueryType = string.Empty;
                    string sMSG = string.Empty;

                    if (btnAdd.Text.Trim() == "Add")
                    {
                        QueryType = "Insert";
                        a = "0";
                    }
                    else
                    {
                        QueryType = "Update";
                        a = hfUniqueId.Value.Trim();
                    }
                    if (rblAmountType.SelectedValue == "P")
                    {
                        b = txtAmtPer.Text.Trim();
                    }
                    else
                    {
                        b = "0";
                    }

                    HttpResponseMessage response;
                    var BoatRateExtn = new BoatRateExtnCharge()
                    {
                        QueryType = QueryType,
                        UniqueId = Convert.ToInt32(a),
                        BoatTypeId = Convert.ToInt32(ddlBoatType.SelectedValue.Trim()),
                        BoatTypeName = ddlBoatType.SelectedItem.Text.Trim(),
                        BoatSeaterId = Convert.ToInt32(ddlBoatSeatId.SelectedValue.Trim()),
                        BoatSeaterName = ddlBoatSeatId.SelectedItem.Text.Trim(),

                        ExtensionType = ddlExtensionType.SelectedValue.Trim(),
                        ExtFromTime = ddlExtFromTime.SelectedItem.Text.Trim(),
                        ExtToTime = ddlExtToTime.SelectedItem.Text.Trim(),
                        AmtType = rblAmountType.SelectedValue.Trim(),

                        AmtPer = Convert.ToDecimal(b),
                        BoatExtnTotAmt = Convert.ToDecimal(txtBoatExtnCharge.Text.Trim()),
                        RowerExtnCharge = Convert.ToDecimal(txtRowerExtnCharge.Text.Trim()),
                        BoatExtnCharge = Convert.ToDecimal(txtBtNorExtCharges.Text.Trim()),
                        BoatExtnTaxAmt = Convert.ToDecimal(txtTaxBtExtChrg.Text.Trim()),

                        BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString().Trim()),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        Createdby = Convert.ToInt32(hfCreatedBy.Value.Trim())

                    };

                    response = client.PostAsJsonAsync("BoatRateExtnCharge", BoatRateExtn).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                        if (StatusCode == 1)
                        {
                            if (btnAdd.Text.Trim() == "Add")
                            {
                                BindBoatRateExtnChrg();
                                BindBoatExtn();
                                ClearBoatRateExtn();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            }
                            else
                            {
                                BindBoatRateExtnChrg();
                                BindBoatExtn();
                                ClearBoatRateExtn();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                                btnAdd.Text = "Add";
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Rate Extension Details Already Exists.');", true);
                        }
                    }
                    else
                    {
                        //  lblGridMsg.Text = response.ToString();
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal(); showExtnType();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Must Enter Boat Extension Charge Amount!');", true);
                if (rblAmountType.SelectedValue == "P")
                {
                    divAmtPer.Attributes.Add("style", "display:block;");
                    divbtAmt.Attributes.Add("style", "display:block;");
                    divrwAmt.Attributes.Add("style", "display:block;");
                    if (ddlExtensionType.SelectedValue == "WD")
                    {
                        decimal actPer = Convert.ToDecimal(txtAmtPer.Text);
                        decimal per = (actPer / 100);
                        txtBtAmt.Text = txtBoatMinCharge.Text.Trim();
                        txtRwAmt.Text = txtRowerMinCharge.Text.Trim();
                    }
                    else if (ddlExtensionType.SelectedValue == "EC")
                    {
                        decimal actPer = Convert.ToDecimal(txtAmtPer.Text);
                        decimal per = (actPer / 100);
                        txtBtAmt.Text = txtBoatPrmMin.Text.Trim();
                        txtRwAmt.Text = txtRowerPrmMin.Text.Trim();
                    }
                    else if (ddlExtensionType.SelectedValue == "WE")
                    {
                        decimal actPer = Convert.ToDecimal(txtAmtPer.Text);
                        decimal per = (actPer / 100);
                        txtBtAmt.Text = txtWkEdAmt.Text.Trim();
                        txtRwAmt.Text = txtWkEdPamt.Text.Trim();
                    }
                }
                else
                {
                    divAmtPer.Attributes.Add("style", "display:none;");
                    divbtAmt.Attributes.Add("style", "display:none;");
                    divrwAmt.Attributes.Add("style", "display:none;");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showExtnType();", true);
        divExtn.Attributes.Add("style", "display:block;");
        if (ChkSingleAllwd.Checked == true)
        {
            divSAcharge.Attributes.Add("style", "display:block;");
            DivSAWeekday.Attributes.Add("style", "display:block;");
            DivSAWeekEnd.Attributes.Add("style", "display:block;");
        }
        else
        {
            divSAcharge.Attributes.Add("style", "display:none;");
            DivSAWeekday.Attributes.Add("style", "display:none;");
            DivSAWeekEnd.Attributes.Add("style", "display:none;");
        }

        ClearBoatRateExtn();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "hideModal();", true);
    }

    protected void ImgBtnAddEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);

            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            btnAdd.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueId = gvBoatRateExtnChrg.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = UniqueId.Trim();

            ddlExtensionType.SelectedValue = ((Label)gvrow.FindControl("lblExtensionType")).Text;
            ddlExtFromTime.SelectedValue = ((Label)gvrow.FindControl("lblExtFromTime")).Text;
            ddlExtToTime.SelectedValue = ((Label)gvrow.FindControl("lblExtToTime")).Text;
            rblAmountType.SelectedValue = ((Label)gvrow.FindControl("lblAmtType")).Text;

            txtBoatExtnCharge.Text = ((Label)gvrow.FindControl("lblBoatExtnTotAmt")).Text;
            txtRowerExtnCharge.Text = ((Label)gvrow.FindControl("lblRowerExtnCharge")).Text;
            txtBtNorExtCharges.Text = ((Label)gvrow.FindControl("lblBoatExtnCharge")).Text;
            txtTaxBtExtChrg.Text = ((Label)gvrow.FindControl("lblBoatExtnTaxAmt")).Text;

            if (rblAmountType.SelectedValue == "P")
            {
                divAmtPer.Attributes.Add("style", "display:block;");
                divbtAmt.Attributes.Add("style", "display:block;");
                divrwAmt.Attributes.Add("style", "display:block;");

                txtBoatExtnCharge.Attributes.Add("readonly", "readonly");
                txtRowerExtnCharge.Attributes.Add("readonly", "readonly");

                txtAmtPer.Text = ((Label)gvrow.FindControl("lblAmtPer")).Text;

                decimal actPer = Convert.ToDecimal(txtAmtPer.Text);
                decimal per = (actPer / 100);
                txtBtAmt.Text = Convert.ToString(Convert.ToDecimal(txtBoatExtnCharge.Text) / per);
                txtRwAmt.Text = Convert.ToString(Convert.ToDecimal(txtRowerExtnCharge.Text) / per);
            }
            else
            {
                divAmtPer.Attributes.Add("style", "display:none;");
                divbtAmt.Attributes.Add("style", "display:none;");
                divrwAmt.Attributes.Add("style", "display:none;");


                txtBoatExtnCharge.Attributes.Remove("readonly");
                txtRowerExtnCharge.Attributes.Remove("readonly");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnAddDelete_Click(object sender, EventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;

            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueId = gvBoatRateExtnChrg.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = UniqueId.Trim();

            Label boatType = (Label)gvrow.FindControl("lblBoatTypeId");
            Label boatTypeName = (Label)gvrow.FindControl("lblBoatTypeName");
            Label boatSeater = (Label)gvrow.FindControl("lblBoatSeaterId");
            Label boatSeaterName = (Label)gvrow.FindControl("lblBoatSeaterName");

            Label extensionType = (Label)gvrow.FindControl("lblExtensionType");
            Label extensionTypeName = (Label)gvrow.FindControl("lblExtensionType");
            Label extensionFromTime = (Label)gvrow.FindControl("lblExtFromTime");
            Label extensionToTime = (Label)gvrow.FindControl("lblExtToTime");
            Label AmtType = (Label)gvrow.FindControl("lblAmtType");
            Label AmtPer = (Label)gvrow.FindControl("lblAmtPer");

            Label boatExtnTotAmt = (Label)gvrow.FindControl("lblBoatExtnTotAmt");
            Label rowerExtnChrg = (Label)gvrow.FindControl("lblRowerExtnCharge");
            Label boatExtnChrg = (Label)gvrow.FindControl("lblBoatExtnCharge");
            Label boatExtnTaxAmt = (Label)gvrow.FindControl("lblBoatExtnTaxAmt");

            string a = string.Empty;
            a = hfUniqueId.Value.Trim();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var BoatRateExtn = new BoatRateExtnCharge()
                {
                    QueryType = "Delete",
                    UniqueId = Convert.ToInt32(a),

                    BoatTypeId = Convert.ToInt32(boatType.Text.Trim()),
                    BoatTypeName = boatTypeName.Text.Trim(),
                    BoatSeaterId = Convert.ToInt32(boatSeater.Text.Trim()),
                    BoatSeaterName = boatSeaterName.Text.Trim(),

                    ExtensionType = extensionType.Text.Trim(),
                    ExtFromTime = extensionFromTime.Text.Trim(),
                    ExtToTime = extensionToTime.Text.Trim(),
                    AmtType = AmtType.Text.Trim(),
                    AmtPer = Convert.ToDecimal(AmtPer.Text.Trim()),

                    BoatExtnTotAmt = Convert.ToDecimal(boatExtnTotAmt.Text.Trim()),
                    RowerExtnCharge = Convert.ToDecimal(rowerExtnChrg.Text.Trim()),
                    BoatExtnCharge = Convert.ToDecimal(boatExtnChrg.Text.Trim()),
                    BoatExtnTaxAmt = Convert.ToDecimal(boatExtnTaxAmt.Text.Trim()),

                    BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString().Trim()),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    Createdby = Convert.ToInt32(Session["UserId"].ToString().Trim())

                };

                response = client.PostAsJsonAsync("BoatRateExtnCharge", BoatRateExtn).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatRateExtnChrg();
                        BindBoatExtn();
                        ClearBoatRateExtn();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void ImgBtnAddUndo_Click(object sender, EventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;

            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueId = gvBoatRateExtnChrg.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = UniqueId.Trim();

            Label boatType = (Label)gvrow.FindControl("lblBoatTypeId");
            Label boatTypeName = (Label)gvrow.FindControl("lblBoatTypeName");
            Label boatSeater = (Label)gvrow.FindControl("lblBoatSeaterId");
            Label boatSeaterName = (Label)gvrow.FindControl("lblBoatSeaterName");

            Label extensionType = (Label)gvrow.FindControl("lblExtensionType");
            Label extensionTypeName = (Label)gvrow.FindControl("lblExtensionType");
            Label extensionFromTime = (Label)gvrow.FindControl("lblExtFromTime");
            Label extensionToTime = (Label)gvrow.FindControl("lblExtToTime");
            Label AmtType = (Label)gvrow.FindControl("lblAmtType");
            Label AmtPer = (Label)gvrow.FindControl("lblAmtPer");

            Label boatExtnTotAmt = (Label)gvrow.FindControl("lblBoatExtnTotAmt");
            Label rowerExtnChrg = (Label)gvrow.FindControl("lblRowerExtnCharge");
            Label boatExtnChrg = (Label)gvrow.FindControl("lblBoatExtnCharge");
            Label boatExtnTaxAmt = (Label)gvrow.FindControl("lblBoatExtnTaxAmt");

            string a = string.Empty;
            a = hfUniqueId.Value.Trim();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                var BoatRateExtn = new BoatRateExtnCharge()
                {
                    QueryType = "Undo",
                    UniqueId = Convert.ToInt32(a),

                    BoatTypeId = Convert.ToInt32(boatType.Text.Trim()),
                    BoatTypeName = boatTypeName.Text.Trim(),
                    BoatSeaterId = Convert.ToInt32(boatSeater.Text.Trim()),
                    BoatSeaterName = boatSeaterName.Text.Trim(),

                    ExtensionType = extensionType.Text.Trim(),
                    ExtFromTime = extensionFromTime.Text.Trim(),
                    ExtToTime = extensionToTime.Text.Trim(),
                    AmtType = AmtType.Text.Trim(),
                    AmtPer = Convert.ToDecimal(AmtPer.Text.Trim()),

                    BoatExtnTotAmt = Convert.ToDecimal(boatExtnTotAmt.Text.Trim()),
                    RowerExtnCharge = Convert.ToDecimal(rowerExtnChrg.Text.Trim()),
                    BoatExtnCharge = Convert.ToDecimal(boatExtnChrg.Text.Trim()),
                    BoatExtnTaxAmt = Convert.ToDecimal(boatExtnTaxAmt.Text.Trim()),

                    BoatHouseId = Convert.ToInt32(Session["BoatHouseId"].ToString().Trim()),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    Createdby = Convert.ToInt32(Session["UserId"].ToString().Trim())

                };

                response = client.PostAsJsonAsync("BoatRateExtnCharge", BoatRateExtn).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatRateExtnChrg();
                        BindBoatExtn();
                        ClearBoatRateExtn();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }

    protected void gvBoatRateExtnChrg_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // for example, the row contains a certain property
                if (((Label)e.Row.FindControl("lblActiveStatus")).Text == "D")
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnAddEdit");
                    btnEdit.Visible = false;
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnAddDelete");
                    btnDelete.Visible = false;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnAddUndo");
                    btnUndo.Visible = true;
                }

                else
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnAddEdit");
                    btnEdit.Visible = true;
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnAddDelete");
                    btnDelete.Visible = true;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnAddUndo");
                    btnUndo.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    /// <summary>
    /// Start
    /// Modified By Silambarasu 21 SEP 2021
    /// </summary>
    /// <param name="dstReports"></param>
    public void GetMainBoatRateDetailsReportExtn(ref DataSet dstReports)
    {
        try
        {
            DataRow drwReport;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatRateDetails = new BoatRateDetailsReport()
                {
                    QueryType = "GetBoatRateDetailsReportExtn",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", BoatRateDetails).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                        {
                            drwReport = dstReports.Tables["BoatRateDetails"].NewRow();

                            drwReport[0] = dtExists.Rows[iRowIdx]["BoatHouseId"].ToString();
                            drwReport[1] = dtExists.Rows[iRowIdx]["BoatType"].ToString();
                            drwReport[2] = dtExists.Rows[iRowIdx]["BoatTypeId"].ToString();
                            drwReport[3] = dtExists.Rows[iRowIdx]["SeaterType"].ToString();
                            drwReport[4] = dtExists.Rows[iRowIdx]["BoatSeaterId"].ToString();
                            drwReport[5] = dtExists.Rows[iRowIdx]["SelfDrive"].ToString();
                            drwReport[6] = dtExists.Rows[iRowIdx]["TimeExtension"].ToString();
                            drwReport[7] = dtExists.Rows[iRowIdx]["BoatGraceTime"].ToString();
                            drwReport[8] = dtExists.Rows[iRowIdx]["Deposit"].ToString();
                            drwReport[9] = dtExists.Rows[iRowIdx]["BoatMinCharge"].ToString();
                            drwReport[10] = dtExists.Rows[iRowIdx]["RowerMinCharge"].ToString();
                            drwReport[11] = dtExists.Rows[iRowIdx]["BoatMinTaxAmt"].ToString();
                            drwReport[12] = dtExists.Rows[iRowIdx]["BoatPremMinCharge"].ToString();
                            drwReport[13] = dtExists.Rows[iRowIdx]["RowerPremMinCharge"].ToString();
                            drwReport[14] = dtExists.Rows[iRowIdx]["BoatPremTaxAmt"].ToString();
                            drwReport[15] = dtExists.Rows[iRowIdx]["BoatPremTotAmt"].ToString();
                            drwReport[16] = dtExists.Rows[iRowIdx]["BoatMinTime"].ToString();
                            drwReport[17] = dtExists.Rows[iRowIdx]["BoatMinTotAmt"].ToString();
                            drwReport[18] = dtExists.Rows[iRowIdx]["WEBoatMinCharge"].ToString();
                            drwReport[19] = dtExists.Rows[iRowIdx]["WERowerMinCharge"].ToString();
                            drwReport[20] = dtExists.Rows[iRowIdx]["WEBoatMinTaxAmt"].ToString();
                            drwReport[21] = dtExists.Rows[iRowIdx]["WEBoatMinTotAmt"].ToString();

                            dstReports.Tables["BoatRateDetails"].Rows.Add(drwReport);
                        }
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Rate Details Not Found !');", true);
                    }
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    public void GetSubBoatRateDetailsReport(ref DataSet dstReports)
    {
        try
        {
            DataRow drwReport;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatRateDetails = new BoatRateDetailsReport()
                {
                    QueryType = "GetBoatRateDetailsRpt",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", BoatRateDetails).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                        {
                            drwReport = dstReports.Tables["BoatRateDetailsSub"].NewRow();

                            drwReport[0] = dtExists.Rows[iRowIdx]["ExtensionType"].ToString();
                            drwReport[1] = dtExists.Rows[iRowIdx]["ExtFromTime"].ToString();
                            drwReport[2] = dtExists.Rows[iRowIdx]["ExtToTime"].ToString();
                            drwReport[3] = dtExists.Rows[iRowIdx]["AmountType"].ToString();
                            drwReport[4] = dtExists.Rows[iRowIdx]["Percentage"].ToString();
                            drwReport[5] = dtExists.Rows[iRowIdx]["BoatExtnCharge"].ToString();
                            drwReport[6] = dtExists.Rows[iRowIdx]["RowerExtnCharge"].ToString();
                            drwReport[7] = dtExists.Rows[iRowIdx]["BoatExtnTaxAmt"].ToString();
                            drwReport[8] = dtExists.Rows[iRowIdx]["BoatExtnTotAmt"].ToString();
                            drwReport[9] = dtExists.Rows[iRowIdx]["BoatHouseId"].ToString();
                            drwReport[10] = dtExists.Rows[iRowIdx]["BoatTypeId"].ToString();
                            drwReport[11] = dtExists.Rows[iRowIdx]["BoatSeaterId"].ToString();





                            dstReports.Tables["BoatRateDetailsSub"].Rows.Add(drwReport);
                        }
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Rate Details Not Found !');", true);
                    }
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetMainBoatRateDetailsReportNoExtn(ref DataSet dstReports)
    {
        ViewState["BoatTypeIdextn"] = "0";
        try
        {
            DataRow drwReport;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatRateDetails = new BoatRateDetailsReport()
                {
                    QueryType = "GetBoatRateDetailsReportNoExtn",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", BoatRateDetails).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                        {
                            drwReport = dstReports.Tables["BoatRateDtlsNoExtn"].NewRow();

                            drwReport[0] = dtExists.Rows[iRowIdx]["BoatHouseId"].ToString();
                            drwReport[1] = dtExists.Rows[iRowIdx]["BoatType"].ToString();
                            drwReport[2] = dtExists.Rows[iRowIdx]["BoatTypeId"].ToString();
                            drwReport[3] = dtExists.Rows[iRowIdx]["SeaterType"].ToString();
                            drwReport[4] = dtExists.Rows[iRowIdx]["BoatSeaterId"].ToString();
                            drwReport[5] = dtExists.Rows[iRowIdx]["SelfDrive"].ToString();
                            drwReport[6] = dtExists.Rows[iRowIdx]["TimeExtension"].ToString();
                            drwReport[7] = dtExists.Rows[iRowIdx]["BoatGraceTime"].ToString();
                            drwReport[8] = dtExists.Rows[iRowIdx]["Deposit"].ToString();
                            drwReport[9] = dtExists.Rows[iRowIdx]["BoatMinCharge"].ToString();
                            drwReport[10] = dtExists.Rows[iRowIdx]["RowerMinCharge"].ToString();
                            drwReport[11] = dtExists.Rows[iRowIdx]["BoatMinTaxAmt"].ToString();
                            drwReport[12] = dtExists.Rows[iRowIdx]["BoatMinTotAmt"].ToString();
                            drwReport[13] = dtExists.Rows[iRowIdx]["BoatPremMinCharge"].ToString();
                            drwReport[14] = dtExists.Rows[iRowIdx]["RowerPremMinCharge"].ToString();
                            drwReport[15] = dtExists.Rows[iRowIdx]["BoatPremTaxAmt"].ToString();
                            drwReport[16] = dtExists.Rows[iRowIdx]["BoatPremTotAmt"].ToString();
                            drwReport[17] = dtExists.Rows[iRowIdx]["BoatMinTime"].ToString();
                            drwReport[18] = dtExists.Rows[iRowIdx]["WEBoatMinCharge"].ToString();
                            drwReport[19] = dtExists.Rows[iRowIdx]["WERowerMinCharge"].ToString();
                            drwReport[20] = dtExists.Rows[iRowIdx]["WEBoatMinTaxAmt"].ToString();
                            drwReport[21] = dtExists.Rows[iRowIdx]["WEBoatMinTotAmt"].ToString();

                            dstReports.Tables["BoatRateDtlsNoExtn"].Rows.Add(drwReport);
                            ViewState["BoatTypeIdextn"] = dtExists.Rows[0]["BoatTypeId"].ToString();
                        }
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Rate Details Not Found !');", true);
                    }
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GetMainBoatRateDetailsReportInd(ref DataSet dstReports)
    {
        ViewState["BoatTypeIdInd"] = "0";
        try
        {
            DataRow drwReport;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatRateDetails = new BoatRateDetailsReport()
                {
                    QueryType = "GetBoatRateDetailsReportInd",
                    ServiceType = "",
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", BoatRateDetails).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        for (int iRowIdx = 0; iRowIdx < dtExists.Rows.Count; iRowIdx++)
                        {
                            drwReport = dstReports.Tables["BoatRateDtlsInd"].NewRow();

                            drwReport[0] = dtExists.Rows[iRowIdx]["BoatHouseId"].ToString();
                            drwReport[1] = dtExists.Rows[iRowIdx]["BoatType"].ToString();
                            drwReport[2] = dtExists.Rows[iRowIdx]["BoatTypeId"].ToString();
                            drwReport[3] = dtExists.Rows[iRowIdx]["SeaterType"].ToString();
                            drwReport[4] = dtExists.Rows[iRowIdx]["BoatSeaterId"].ToString();
                            drwReport[5] = dtExists.Rows[iRowIdx]["SelfDrive"].ToString();
                            drwReport[6] = dtExists.Rows[iRowIdx]["TimeExtension"].ToString();
                            drwReport[7] = dtExists.Rows[iRowIdx]["BoatGraceTime"].ToString();
                            drwReport[8] = dtExists.Rows[iRowIdx]["Deposit"].ToString();
                            drwReport[9] = dtExists.Rows[iRowIdx]["IWDBoatMinCharge"].ToString();
                            drwReport[10] = dtExists.Rows[iRowIdx]["IWDRowerMinCharge"].ToString();
                            drwReport[11] = dtExists.Rows[iRowIdx]["IWDBoatMinTaxAmt"].ToString();
                            drwReport[12] = dtExists.Rows[iRowIdx]["IWDBoatMinTotAmt"].ToString();
                            drwReport[13] = dtExists.Rows[iRowIdx]["IWEBoatMinCharge"].ToString();
                            drwReport[14] = dtExists.Rows[iRowIdx]["IWERowerMinCharge"].ToString();
                            drwReport[15] = dtExists.Rows[iRowIdx]["IWEBoatMinTaxAmt"].ToString();
                            drwReport[16] = dtExists.Rows[iRowIdx]["IWEBoatMinTotAmt"].ToString();
                            drwReport[17] = dtExists.Rows[iRowIdx]["BoatMinTime"].ToString();

                            dstReports.Tables["BoatRateDtlsInd"].Rows.Add(drwReport);
                            ViewState["BoatTypeIdInd"] = dtExists.Rows[0]["BoatTypeId"].ToString();
                        }
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Boat Rate Details Not Found !');", true);
                    }
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void GenerateToPDF()
    {
        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();
        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;


        try
        {
            DataSet dstRports = new BoatRateDetails();
            GetMainBoatRateDetailsReportExtn(ref dstRports);
            GetSubBoatRateDetailsReport(ref dstRports);
            GetMainBoatRateDetailsReportNoExtn(ref dstRports);
            GetMainBoatRateDetailsReportInd(ref dstRports);
            objReportDocument.Load(Server.MapPath("BoatRateDetailsReport.rpt"));
            objReportDocument.SetDataSource(dstRports);
            objReportDocument.SetParameterValue(0, Session["BoatHouseName"].ToString().Trim());
            objReportDocument.SetParameterValue(1, ViewState["BoatTypeIdextn"].ToString().Trim());
            objReportDocument.SetParameterValue(2, ViewState["BoatTypeIdInd"].ToString().Trim());
            objReportDocument.SetParameterValue(3, Session["CorpName"].ToString());
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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
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
    protected void btnExportToPdf_Click(object sender, ImageClickEventArgs e)
    {
        GenerateToPDF();
    }

    // Creating Class For Boat Rate
    public class boatRate
    {

        public string QueryType { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatTypeName { get; set; }
        public string BoatSeaterId { get; set; }
        public string BoatSeaterName { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string BoatImageLink { get; set; }
        public string SelfDrive { get; set; }
        public string DepositType { get; set; }
        public string DepositTypeName { get; set; }
        public string TimeExtension { get; set; }
        public string BoatMinTime { get; set; }
        public string BoatGraceTime { get; set; }
        public string Deposit { get; set; }

        public string BoatMinTotAmt { get; set; }
        public string BoatMinCharge { get; set; }
        public string RowerMinCharge { get; set; }
        public string BoatMinTaxAmt { get; set; }



        public string WEBoatMinTotAmt { get; set; }
        public string WEBoatMinCharge { get; set; }
        public string WERowerMinCharge { get; set; }
        public string WEBoatMinTaxAmt { get; set; }



        public string BoatPremTotAmt { get; set; }
        public string BoatPremMinCharge { get; set; }
        public string RowerPremMinCharge { get; set; }
        public string BoatPremTaxAmt { get; set; }



        public string IWDBoatMinTotAmt { get; set; }
        public string IWDBoatMinCharge { get; set; }
        public string IWDRowerMinCharge { get; set; }
        public string IWDBoatMinTaxAmt { get; set; }



        public string IWEBoatMinTotAmt { get; set; }
        public string IWEBoatMinCharge { get; set; }
        public string IWERowerMinCharge { get; set; }
        public string IWEBoatMinTaxAmt { get; set; }

        public string MaxTripsPerDay { get; set; }

        public string ChildApplicable { get; set; }
        public string NoofChildApplicable { get; set; }
        public string ChargePerChild { get; set; }
        public string ChargePerChildTotAmt { get; set; }
        public string ChargePerChildTaxAmt { get; set; }

        public string Createdby { get; set; }
        public string ActiveStatus { get; set; }
        public string ServiceId { get; set; }
        public string ValidDate { get; set; }

        public string DisplayOrder { get; set; }
        public string PerHeadApplicable { get; set; }
    }

    public class ImageUpload
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string FormName { get; set; }
        public string PrevImageLink { get; set; }
        public string ImageLink { get; set; }
    }

    public class BoatRateExtnCharge
    {
        public string QueryType { get; set; }
        public int UniqueId { get; set; }
        public int BoatTypeId { get; set; }
        public string BoatTypeName { get; set; }
        public int BoatSeaterId { get; set; }
        public string BoatSeaterName { get; set; }

        public string ExtensionType { get; set; }
        public string ExtFromTime { get; set; }
        public string ExtToTime { get; set; }
        public string AmtType { get; set; }
        public decimal AmtPer { get; set; }

        public decimal BoatExtnTotAmt { get; set; }
        public decimal RowerExtnCharge { get; set; }
        public decimal BoatExtnCharge { get; set; }
        public decimal BoatExtnTaxAmt { get; set; }

        public int BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }

        public int Createdby { get; set; }
        public string ActiveStatus { get; set; }


    }
    public class BoatRateDetailsReport
    {

        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string ServiceType { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }

    }





}