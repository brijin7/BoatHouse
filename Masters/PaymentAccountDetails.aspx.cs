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

public partial class Masters_PaymentAccountDetails : System.Web.UI.Page
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

				//CHANGES
				BindPayUPIDetails();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    protected void txtBankIFSC_TextChanged(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                string ZipURL = "https://api.techm.co.in/api/v1/ifsc/";
                client.BaseAddress = new Uri(ZipURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("https://api.techm.co.in/api/v1/ifsc/" + txtBankIFSC.Text.Trim() + "").Result;
                string mesg = " Invalid Bank IFS Code";
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    string Status = JObject.Parse(Locresponse)["status"].ToString();
                    string Message = JObject.Parse(Locresponse)["message"].ToString();
                    string data = JObject.Parse(Locresponse)["data"].ToString();
                    if (Status == "success")
                    {

                        DataTable dt = JsonConvert.DeserializeObject<DataTable>("[" + data + "]");
                        if (dt.Rows.Count > 0)
                        {
                            lblBankName.Text = dt.Rows[0]["BANK"].ToString();
                            lblBankBranch.Text = dt.Rows[0]["BRANCH"].ToString();
                            lblMICRCode.Text = dt.Rows[0]["MICRCODE"].ToString();
                            lblCity.Text = dt.Rows[0]["DISTRICT"].ToString();
                            lblDistrict.Text = dt.Rows[0]["CITY"].ToString();
                            lblState.Text = dt.Rows[0]["STATE"].ToString();

                        }
                        else
                        {
                            lblBankName.Text = "";
                            lblBankBranch.Text = "";
                            lblMICRCode.Text = "";
                            lblCity.Text = "";
                            lblDistrict.Text = "";
                            lblState.Text = "";
                        }

                    }
                    else
                    {
                        lblBankName.Text = "";
                        lblBankBranch.Text = "";
                        lblMICRCode.Text = "";
                        lblCity.Text = "";
                        lblDistrict.Text = "";
                        lblState.Text = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + mesg.ToString().Trim() + "');", true);
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

    protected void btnSubmit_Click(object sender, EventArgs e)
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
                string QType = string.Empty;
                string Id = string.Empty;

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    QType = "Insert";
                    Id = "0";

                }
                else
                {
                    QType = "Update";
                    Id = hfUniqueId.Value.Trim();
                }

                var Boatseatmaster = new PaymentAccDetails()
                {
                    QueryType = QType.Trim(),
                    UniqueId = Convert.ToInt32(Id.Trim()),
                    AccountName = txtAccountName.Text.Trim(),
                    AccountNo = txtAccountNo.Text.Trim(),
                    BankIFSCCode = txtBankIFSC.Text.Trim(),
                    BankName = lblBankName.Text.Trim(),
                    BranchName = lblBankBranch.Text.Trim(),
                    MICRCode = lblMICRCode.Text.Trim(),
                    City = lblCity.Text.Trim(),
                    District = lblDistrict.Text.Trim(),
                    State = lblState.Text.Trim(),
                    EntityType = "Boating",
                    CreatedBy = Session["UserId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("PaymentAccDetails", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            BindPayUPIDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else if (btnSubmit.Text.Trim() == "Update")
                        {
                            BindPayUPIDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {
                            BindPayUPIDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    BindPayUPIDetails();
                    ClearInputs();
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
        ClearInputs();
    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnSubmit.Text = "Update";

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sId = gvPayAccDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = sId.Trim();
            Label AccName = (Label)gvrow.FindControl("lblAccountName");
            Label AccNo = (Label)gvrow.FindControl("lblAccountNo");
            Label BankIFSC = (Label)gvrow.FindControl("lblBankIFSCCode");

            Label BankName = (Label)gvrow.FindControl("lblBankName");
            Label BankBranch = (Label)gvrow.FindControl("lblBranchName");
            Label BankMICR = (Label)gvrow.FindControl("lblMICRCode");

            Label City = (Label)gvrow.FindControl("lblCity");
            Label District = (Label)gvrow.FindControl("lblDistrict");
            Label State = (Label)gvrow.FindControl("lblState");

            txtAccountName.Text = AccName.Text.Trim();
            txtAccountNo.Text = AccNo.Text.Trim();
            txtBankIFSC.Text = BankIFSC.Text.Trim();

            lblBankName.Text = BankName.Text.Trim();
            lblBankBranch.Text = BankBranch.Text.Trim();
            lblMICRCode.Text = BankMICR.Text.Trim();

            lblCity.Text = City.Text.Trim();
            lblDistrict.Text = District.Text.Trim();
            lblState.Text = State.Text.Trim();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvPayAccDetails_RowDataBound(Object sender, GridViewRowEventArgs e)
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

    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sId = gvPayAccDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            Label AccName = (Label)gvrow.FindControl("lblAccountName");
            Label AccNo = (Label)gvrow.FindControl("lblAccountNo");
            Label BankIFSC = (Label)gvrow.FindControl("lblBankIFSCCode");

            Label BankName = (Label)gvrow.FindControl("lblBankName");
            Label BankBranch = (Label)gvrow.FindControl("lblBranchName");
            Label BankMICR = (Label)gvrow.FindControl("lblMICRCode");

            Label City = (Label)gvrow.FindControl("lblCity");
            Label District = (Label)gvrow.FindControl("lblDistrict");
            Label State = (Label)gvrow.FindControl("lblState");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Boatseatmaster = new PaymentAccDetails()
                {
                    QueryType = "Delete",
                    UniqueId = Convert.ToInt32(sId.Trim()),
                    AccountName = txtAccountName.Text.Trim(),
                    AccountNo = txtAccountNo.Text.Trim(),
                    BankIFSCCode = txtBankIFSC.Text.Trim(),
                    BankName = lblBankName.Text.Trim(),
                    BranchName = lblBankBranch.Text.Trim(),
                    MICRCode = lblMICRCode.Text.Trim(),
                    City = lblCity.Text.Trim(),
                    District = lblDistrict.Text.Trim(),
                    State = lblState.Text.Trim(),
                    EntityType = "Boating",
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("PaymentAccDetails", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString() + "');", true);
                    }
                    else
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindPayUPIDetails();
                    ClearInputs();
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
            string sId = gvPayAccDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            Label AccName = (Label)gvrow.FindControl("lblAccountName");
            Label AccNo = (Label)gvrow.FindControl("lblAccountNo");
            Label BankIFSC = (Label)gvrow.FindControl("lblBankIFSCCode");

            Label BankName = (Label)gvrow.FindControl("lblBankName");
            Label BankBranch = (Label)gvrow.FindControl("lblBranchName");
            Label BankMICR = (Label)gvrow.FindControl("lblMICRCode");

            Label City = (Label)gvrow.FindControl("lblCity");
            Label District = (Label)gvrow.FindControl("lblDistrict");
            Label State = (Label)gvrow.FindControl("lblState");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Boatseatmaster = new PaymentAccDetails()
                {
                    QueryType = "ReActive",
                    UniqueId = Convert.ToInt32(sId.Trim()),
                    AccountName = txtAccountName.Text.Trim(),
                    AccountNo = txtAccountNo.Text.Trim(),
                    BankIFSCCode = txtBankIFSC.Text.Trim(),
                    BankName = lblBankName.Text.Trim(),
                    BranchName = lblBankBranch.Text.Trim(),
                    MICRCode = lblMICRCode.Text.Trim(),
                    City = lblCity.Text.Trim(),
                    District = lblDistrict.Text.Trim(),
                    State = lblState.Text.Trim(),
                    EntityType = "Boating",
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                response = client.PostAsJsonAsync("PaymentAccDetails", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        BindPayUPIDetails();
                        ClearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    BindPayUPIDetails();
                    ClearInputs();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindPayUPIDetails()
    {
        try
        {
            divGrid.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var body = new CommonClass()
                {
                    QueryType = "PaymentAccountDetails",
                    ServiceType = "Boating",
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
                        gvPayAccDetails.DataSource = dtExists;
                        gvPayAccDetails.DataBind();

                        divGrid.Visible = true;
                    }
                    else
                    {
                        gvPayAccDetails.DataBind();
                    }

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
        txtAccountName.Text = string.Empty;
        txtAccountNo.Text = string.Empty;
        txtBankIFSC.Text = string.Empty;

        lblBankName.Text = string.Empty;
        lblBankBranch.Text = string.Empty;
        lblMICRCode.Text = string.Empty;

        lblCity.Text = string.Empty;
        lblDistrict.Text = string.Empty;
        lblState.Text = string.Empty;

        btnSubmit.Text = "Submit";
    }

    public class CommonClass
    {
        public string BoatHouseId
        {
            get;
            set;
        }
        public string QueryType
        {
            get;
            set;
        }
        public string BoatTypeId
        {
            get;
            set;
        }
        public string BoatSeaterId
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
        public string Input1
        {
            get;
            set;
        }
        public string Input2
        {
            get;
            set;
        }
        public string Input3
        {
            get;
            set;
        }
        public string Input4
        {
            get;
            set;
        }
        public string Input5
        {
            get;
            set;
        }

        public string ServiceType
        {
            get;
            set;
        }
    }

    public class PaymentAccDetails
    {
        public string QueryType { get; set; }
        public int UniqueId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankIFSCCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string MICRCode { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string EntityType { get; set; }
        public string CreatedBy { get; set; }


    }
}