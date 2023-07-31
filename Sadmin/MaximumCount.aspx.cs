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


public partial class Sadmin_MaximumCount : System.Web.UI.Page
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
                if (Session["UserRole"].ToString().Trim() == "Sadmin")
                {
                    divShow.Visible = true;
                    BindBoatHouseName();
                    BindDisplayMaxCountDetails();
                }
                //Changes

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    public void BindBoatHouseName()
    {
        try
        {
           ddlBoatHouseId.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("ddlBoatHouse?CorpId=" + Session["CorpId"].ToString() +"").Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["Response"].ToString();

                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        if (dt.Rows.Count > 0)
                        {
                            ddlBoatHouseId.DataSource = dt;
                            ddlBoatHouseId.DataValueField = "BoatHouseId";
                            ddlBoatHouseId.DataTextField = "BoatHouseName";
                            ddlBoatHouseId.DataBind();
                        }
                        else
                        {
                            ddlBoatHouseId.DataBind();
                        }

                        ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                    }
                    else
                    {
                        ddlBoatHouseId.DataBind();
                        ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                    }
                }
                else
                {
                    ddlBoatHouseId.DataBind();
                    ddlBoatHouseId.Items.Insert(0, "Select Boat House");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    // Binding Display MaxCount Details
    public void BindDisplayMaxCountDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseId = new boatTypeMaster()
                {
                    BoatHouseId = "",
                };
                HttpResponseMessage response = client.PostAsJsonAsync("GetDisplayMaxCount", BoatHouseId).Result;

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
                            gvCount.DataSource = dt;
                            gvCount.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divgrid.Visible = true;                       
                        }
                        else
                        {

                            gvCount.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divgrid.Visible = true;                           
                        }

                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg;
                        divgrid.Visible = false;                       
                    }
                }
                else
                {
                    lblGridMsg.Text = response.ToString();
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
            string count = ddlBoatHouseId.SelectedValue;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                string sMSG = string.Empty;
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var BoatType = new boatTypeMaster()
                    {
                        QueryType = "Insert",
                        MaxCount = txtDispalyCount.Text.Trim(),
                        BoatHouseId = ddlBoatHouseId.SelectedValue.Trim(),                        
                        CreatedBy = Session["UserId"].ToString().Trim()

                };
                    response = client.PostAsJsonAsync("DisplayMaximumCount", BoatType).Result;
                    sMSG = "Display Count Details Inserted Successfully";

                }
                else
                {
                    
                    var BoatType = new boatTypeMaster()
                    {
                        QueryType = "Update",                    
                        MaxCount = txtDispalyCount.Text.Trim(),
                        BoatHouseId = ddlBoatHouseId.SelectedValue.Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()

                    };
                    response = client.PostAsJsonAsync("DisplayMaximumCount", BoatType).Result;
                    sMSG = "Display Count Details Updated Successfully";
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                      
                        btnSubmit.Text = "Submit";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindDisplayMaxCountDetails();
                        Clear();


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                      
                    }
                }
                else
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
        Clear();

    }
    public class boatTypeMaster
    {
        public string QueryType { get; set; }
        public string MaxCount { get; set; }       
        public string BoatHouseId { get; set; }
        public string CreatedBy { get; set; }


    }
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divgrid.Visible = true;          
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvCount.DataKeys[gvrow.RowIndex].Value.ToString();        
            Label Maxcount = (Label)gvrow.FindControl("lblMaxcount");
            Label BoatHouseId = (Label)gvrow.FindControl("lblboatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            
            txtDispalyCount.Text = Maxcount.Text;
            ddlBoatHouseId.SelectedValue = BoatHouseId.Text;
            hfboathouse.Value = BoatHouseName.Text;


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
      
    protected void ImgDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvCount.DataKeys[gvrow.RowIndex].Value.ToString();
            string WeekDayDescrp = string.Empty;
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatType = new boatTypeMaster()
                {
                    QueryType = "Delete",
                    MaxCount = "0",
                    BoatHouseId = BoatHouseId.Text,
                    CreatedBy = Session["UserId"].ToString().Trim()

                };
                HttpResponseMessage response;
                response = client.PostAsJsonAsync("DisplayMaximumCount", BoatType).Result;


                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindDisplayMaxCountDetails();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    public void Clear()
    {
        ddlBoatHouseId.SelectedIndex = 0;
        txtDispalyCount.Text = string.Empty;
    }


}