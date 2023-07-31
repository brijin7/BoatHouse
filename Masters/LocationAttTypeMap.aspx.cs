using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LocationAttTypeMap : System.Web.UI.Page
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
				hfUserId.Value = Session["UserId"].ToString();
				getLocation();
				getAttraction();
				BindlocationAttType();
				//divnew.Visible = false;
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}

	}
    public class locationAttType
    {
        public string QueryType { get; set; }
        public string AttractionId { get; set; }
        public string AttractionTypeName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string ConfigName
        { get; set; }
        public string ConfigId { get; set; }
    }
    public async void getLocation()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ddlGetLocation");

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

                            ddlLocation.DataSource = dt;
                            ddlLocation.DataValueField = "LocationId";
                            ddlLocation.DataTextField = "LocationName";
                            ddlLocation.DataBind();

                        }
                        else
                        {

                            ddlLocation.DataBind();
                        }
                        ddlLocation.Items.Insert(0, new ListItem("Select Item", "0"));
                    }
                    else
                    {

                        lblGridMsg.Text = ResponseMsg.ToString().Trim();

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
    public async void getAttraction()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ConfigMstr/DDLAttraction");

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
                            ddlAttraction.DataSource = dt;
                            ddlAttraction.DataValueField = "ConfigId";
                            ddlAttraction.DataTextField = "ConfigName";
                            ddlAttraction.DataBind();
                            //BindConfiguration(ddlVehicleId);
                        }
                        else
                        {
                            ddlAttraction.DataBind();
                        }
                        ddlAttraction.Items.Insert(0, new ListItem("Select Attraction", "0"));
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg;

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
    public async void BindlocationAttType()
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

                response = await client.GetAsync("GetLocAttrtypeMap/ListAll ");
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
                            gvLocationAttrType.DataSource = dt;
                            gvLocationAttrType.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvLocationAttrType.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        clearInputs();
                        divEntry.Visible = true;
                        divGrid.Visible = false;
                        lbtnNew.Visible = false;
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearInputs();
        divEntry.Visible = false;
        divGrid.Visible = true;
        lbtnNew.Visible = true;
        BindlocationAttType();
        btnSubmit.Text = "Submit";
    }
    public void clearInputs()
    {
        ddlAttraction.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
    }
    protected async void btnSubmit_Click(object sender, EventArgs e)
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

                var locationAttType = new locationAttType()
                {
                    QueryType = "Insert",
                    AttractionId = ddlAttraction.SelectedValue.Trim(),
                    LocationId = ddlLocation.SelectedValue.Trim(),

                    ActiveStatus = "A",
                    CreatedBy = hfUserId.Value.Trim()
                };
                response = await client.PostAsJsonAsync("LocationAttrTypeMap", locationAttType);
                sMSG = "Location AttractionType Mapping  Inserted Successfully";



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        if (btnSubmit.Text.Trim() == "Submit")
                        {
                            BindlocationAttType();
                            clearInputs();
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                        }


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Configuration Details Already Exists.');", true);
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

    //protected async void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        divEntry.Visible = true;
    //        divGrid.Visible = false;
    //        lbtnNew.Visible = false;
    //        ImageButton lnkbtn = sender as ImageButton;
    //        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
    //        string sTesfg = gvLocationAttrType.DataKeys[gvrow.RowIndex].Value.ToString();
    //        Label LocationId = (Label)gvrow.FindControl("lblLocationId");
    //        Label AttractionId = (Label)gvrow.FindControl("lblAttractionTypeId");
    //        //Label LocationImageLink=(Label)gvrow.FindControl("lblLocationImageLink");
    //        ddlAttraction.SelectedValue = AttractionId.Text;
    //        ddlLocation.SelectedValue = LocationId.Text;
    //        btnSubmit.Text = "Update";
    //    }

    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}



    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
    }





    protected void ImgBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string shiftId = gvLocationAttrType.DataKeys[gvrow.RowIndex].Value.ToString();
            Label LocationId = (Label)gvrow.FindControl("lblLocationId");
            Label AttractionId = (Label)gvrow.FindControl("lblAttractionTypeId");

            ddlAttraction.SelectedValue = AttractionId.Text;
            ddlLocation.SelectedValue = LocationId.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var locationAttType = new locationAttType()
                {
                    QueryType = "Delete",
                    AttractionId = ddlAttraction.SelectedValue.Trim(),
                    LocationId = ddlLocation.SelectedValue.Trim(),

                    ActiveStatus = "A",
                    CreatedBy = hfUserId.Value.Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("LocationAttrTypeMap", locationAttType).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindlocationAttType();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

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

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string shiftId = gvLocationAttrType.DataKeys[gvrow.RowIndex].Value.ToString();
            Label LocationId = (Label)gvrow.FindControl("lblLocationId");
            Label AttractionId = (Label)gvrow.FindControl("lblAttractionTypeId");

            ddlAttraction.SelectedValue = AttractionId.Text;
            ddlLocation.SelectedValue = LocationId.Text;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var locationAttType = new locationAttType()
                {
                    QueryType = "ReActive",
                    AttractionId = ddlAttraction.SelectedValue.Trim(),
                    LocationId = ddlLocation.SelectedValue.Trim(),

                    ActiveStatus = "A",
                    CreatedBy = hfUserId.Value.Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("LocationAttrTypeMap", locationAttType).Result;



                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindlocationAttType();
                        clearInputs();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

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

    protected void gvLocationAttrType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // for example, the row contains a certain property
                if (((Label)e.Row.FindControl("lblActiveStatus")).Text == "D")
                {                   
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = false;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = true;
                }
                else
                {
                  
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