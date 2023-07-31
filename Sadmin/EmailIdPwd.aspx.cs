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

public partial class Masters_EmailIdPwd : System.Web.UI.Page
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
                //Changes
                GetEmailServiceType();
                BindEmlPwdDetails();
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void GetEmailServiceType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ConfigMstr/DDLEmailServiceType").Result;

                if (response.IsSuccessStatusCode)
                {
                    var PayType = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(PayType)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(PayType)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlEmailService.DataSource = dt;
                            ddlEmailService.DataValueField = "ConfigId";
                            ddlEmailService.DataTextField = "ConfigName";
                            ddlEmailService.DataBind();

                          
                        }
                        else
                        {
                            ddlEmailService.DataBind();
                            ddlEmailService.DataBind();
                        }


                        ddlEmailService.Items.Insert(0, new ListItem("Select Service Type", "0"));
                        
                    }
                    else
                    {
                        ddlEmailService.Items.Insert(0, new ListItem("Select Service Type", "0"));
                        
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

                var Boatseatmaster = new EmailIdPwd()
                {
                    QueryType = QType.Trim(),
                    UniqueId = Convert.ToInt32(Id.Trim()),
                    EmailId = txtEmailId.Text.Trim(),
                    Password = txtPwd.Text.Trim(),
                    ServiceType= ddlEmailService.SelectedValue.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };
                response = client.PostAsJsonAsync("EmailIdPwdDetails", Boatseatmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            BindEmlPwdDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else if (btnSubmit.Text.Trim() == "Update")
                        {
                            BindEmlPwdDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {
                            BindEmlPwdDetails();
                            ClearInputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        BindEmlPwdDetails();
                        ClearInputs();
                       
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    BindEmlPwdDetails();
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
            string sId = gvEmlPwdDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = sId.Trim();
            Label Email = (Label)gvrow.FindControl("lblEmailId");
            Label Pwd = (Label)gvrow.FindControl("lblPassword");
            Label ServiceTypeId = (Label)gvrow.FindControl("lblServiceTypeId");

            txtEmailId.Text = Email.Text.Trim();
            txtPwd.Text = Pwd.Text.Trim();
            ddlEmailService.SelectedValue = ServiceTypeId.Text.Trim();
            ddlEmailService.Enabled = false;

            if (txtPwd.TextMode == TextBoxMode.Password)
            {
                txtPwd.Attributes.Add("value", txtPwd.Text);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public void BindEmlPwdDetails()
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
                    QueryType = "GridEmailDetails",
                    ServiceType = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",
                    BoatHouseId = ""
                };

                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", body).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        gvEmlPwdDetails.DataSource = dtExists;
                        gvEmlPwdDetails.DataBind();

                        divGrid.Visible = true;
                    }
                    else
                    {
                        gvEmlPwdDetails.DataBind();
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
        txtEmailId.Text = string.Empty;
        txtPwd.Text = string.Empty;
        ddlEmailService.SelectedIndex = 0;
        ddlEmailService.Enabled = true;

        this.txtPwd.Attributes["value"] = "";
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

    public class EmailIdPwd
    {
        public string QueryType { get; set; }
        public int UniqueId { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string ServiceType { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
    }
    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ClearInputs();
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sId = gvEmlPwdDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = sId.Trim();
            Label Email = (Label)gvrow.FindControl("lblEmailId");
            Label Pwd = (Label)gvrow.FindControl("lblPassword");
            Label ServiceTypeId = (Label)gvrow.FindControl("lblServiceTypeId");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
              
                var Boatseatmaster = new EmailIdPwd()
                {
                    QueryType = "Delete",
                    
                    UniqueId = Convert.ToInt32(hfUniqueId.Value.Trim()),
                    EmailId = Email.Text.Trim(),
                    Password = Pwd.Text.Trim(),
                    ServiceType = ServiceTypeId.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;


                response = client.PostAsJsonAsync("EmailIdPwdDetails", Boatseatmaster).Result;


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindEmlPwdDetails();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    lblPayGridMsg.Text = response.ToString();
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
            ClearInputs();
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sId = gvEmlPwdDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = sId.Trim();
            Label Email = (Label)gvrow.FindControl("lblEmailId");
            Label Pwd = (Label)gvrow.FindControl("lblPassword");
            Label ServiceTypeId = (Label)gvrow.FindControl("lblServiceTypeId");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                var otherservices = new EmailIdPwd()
                {
                    QueryType = "ReActive",
                    UniqueId = Convert.ToInt32(hfUniqueId.Value.Trim()),
                    EmailId = Email.Text.Trim(),
                    Password = Pwd.Text.Trim(),
                    ServiceType = ServiceTypeId.Text.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
               

                response = client.PostAsJsonAsync("EmailIdPwdDetails", otherservices).Result;
                

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindEmlPwdDetails();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    lblPayGridMsg.Text = response.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void gvEmlPwdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
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
}