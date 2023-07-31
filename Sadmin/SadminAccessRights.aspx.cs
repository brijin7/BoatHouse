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

public partial class Sadmin_SadminAccessRights : System.Web.UI.Page
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
                getSadminDetails("NormalDdl", "");
                BindgvSadminDetails();
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    /// <summary>
    /// This Function Used To Bind Sadmin Details In Drop Down
    /// </summary>
    private void getSadminDetails(string sType, string sUserId)
    {
        try
        {
            ddlSadminList.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var SadminLists = new SelectSadminLists();
                if (sType.ToString().Trim() == "NormalDdl")
                {
                    SadminLists = new SelectSadminLists()
                    {
                        QueryType = "GetSadminDdl",
                        ServiceType = "",
                        BoatHouseId = "",
                        Input1 = "",
                        Input2 = "",
                        Input3 = "",
                        Input4 = "",
                        Input5 = ""
                    };
                }
                else
                {
                    SadminLists = new SelectSadminLists()
                    {
                        QueryType = "EditSadminDdl",
                        ServiceType = "",
                        BoatHouseId = "",
                        Input1 = sUserId.ToString().Trim(),
                        Input2 = "",
                        Input3 = "",
                        Input4 = "",
                        Input5 = ""
                    };
                }
                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", SadminLists).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SadminResponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(SadminResponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        ddlSadminList.DataSource = dtExists;
                        ddlSadminList.DataValueField = "UserId";
                        ddlSadminList.DataTextField = "UserName";
                        ddlSadminList.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sadmin Name, No Records Found !!!');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Sadmin Name, No Records Found !!!');", true);
                }
                ddlSadminList.Items.Insert(0, new ListItem("Select Sadmin Name", "0"));
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    /// <summary>
    /// This Method Is Uded To Bind GridView
    /// </summary>
    private void BindgvSadminDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var SadminLists = new SelectSadminLists()
                {
                    QueryType = "GetSadminGrid",
                    ServiceType = "",
                    BoatHouseId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = ""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("CommonReport", SadminLists).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SadminResponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(SadminResponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                    if (dtExists.Rows.Count > 0)
                    {
                        gvSadminDetails.DataSource = dtExists;
                        gvSadminDetails.DataBind();
                        lblNoRecords.Visible = false;

                        foreach (GridViewRow gvr in gvSadminDetails.Rows)
                        {
                            LinkButton lblBtnDelete = (LinkButton)gvr.FindControl("lnkBtnDelete");
                            ImageButton lblBtnEdit = (ImageButton)gvr.FindControl("ImgBtnEdit");

                            if (lblBtnDelete.Text == "Inactive")
                            {
                                lblBtnDelete.Enabled = false;
                                lblBtnEdit.Visible = false;
                                lblBtnDelete.Style.Add("Color","Red");
                            }
                            else
                            {
                                lblBtnDelete.Enabled = true;
                                lblBtnEdit.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        gvSadminDetails.DataSource = dtExists;
                        gvSadminDetails.DataBind();
                        lblNoRecords.Visible = true;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No Records Found !!!');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    /// <summary>
    /// This Method Is Used To Submit Details.
    /// </summary>
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
                var SadminLists = new InsertSadminLists()
                {
                    QueryType = btnSubmit.Text.Trim() == "Submit" ? "Insert" : btnSubmit.Text.Trim(),
                    UserId = ddlSadminList.SelectedValue.Trim(),
                    CreatedBy = Session["UserId"].ToString().Trim(),
                    UniqueId = btnSubmit.Text.Trim() == "Update" ? ViewState["UniqueId"].ToString().Trim() : ""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("SadminAccessRights", SadminLists).Result;
                if (response.IsSuccessStatusCode)
                {
                    var ResPonseMsq = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(ResPonseMsq)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(ResPonseMsq)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindgvSadminDetails();
                        getSadminDetails("NormalDdl", "");
                        btnSubmit.Text = "Submit";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Failed To Insert Sadmin Details !!!');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    /// <summary>
    /// This Method Is Used To Edit Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            ViewState["UniqueId"] = gvSadminDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            Label lblUserId = (Label)gvrow.FindControl("lblUserId");
            Label lblUserName = (Label)gvrow.FindControl("lblUserName");
            getSadminDetails("EditDdl", lblUserId.Text.Trim());
            ddlSadminList.SelectedValue = lblUserId.Text.Trim();
            btnSubmit.Text = "Update";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    /// <summary>
    /// This Method Is Used To Inactive Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueId = gvSadminDetails.DataKeys[gvrow.RowIndex].Value.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var SadminLists = new InsertSadminLists()
                {
                    QueryType = "InActive",
                    UserId = "",
                    CreatedBy = Session["UserId"].ToString().Trim(),
                    UniqueId = UniqueId.ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("SadminAccessRights", SadminLists).Result;
                if (response.IsSuccessStatusCode)
                {
                    var ResPonseMsq = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(ResPonseMsq)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(ResPonseMsq)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindgvSadminDetails();
                        getSadminDetails("NormalDdl", "");
                        btnSubmit.Text = "Submit";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Failed To Insert Sadmin Details !!!');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    /// <summary>
    /// This Method Is Used To Reset Fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnSubmit.Text = "Submit";
        getSadminDetails("NormalDdl", "");
    }
    public class SelectSadminLists
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string BoatHouseId { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }
    public class InsertSadminLists
    {
        public string QueryType { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UniqueId { get; set; }
    }
}