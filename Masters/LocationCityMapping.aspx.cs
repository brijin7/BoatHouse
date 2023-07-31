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

public partial class LocationCityMapping : System.Web.UI.Page
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
				hfCreatedBy.Value = Session["UserId"].ToString().Trim();
				GetCity();
				GetLocation();
				BindLocationCityMapping();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}
    public class locationCityMapping
    {
        public string LocationId { get; set; }
        public string CityId { get; set; }
        public string Distance { get; set; }
        public string CreatedBy { get; set; }
        public string QueryType {get;set;}

    }

    public async void BindLocationCityMapping()
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

                response = await client.GetAsync("ShowLocCityMapList/ListAll");
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
                            gvLocMapping.DataSource = dt;
                            gvLocMapping.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvLocMapping.DataSource = dt;
                            gvLocMapping.DataBind();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                            btnSubmit.Text = "Submit";
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString().Trim();

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
    public async void GetCity()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ConfigMstr/DDLCity");

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

                            ddlCity.DataSource = dt;
                            ddlCity.DataValueField = "ConfigId";
                            ddlCity.DataTextField = "ConfigName";
                            ddlCity.DataBind();

                        }
                        else
                        {

                            ddlCity.DataBind();
                        }
                        ddlCity.Items.Insert(0, new ListItem("Select City", "0"));
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
    public async void GetLocation()
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
                        ddlLocation.Items.Insert(0, new ListItem("Select Location", "0"));
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
    public void clear()
    {
        ddlLocation.SelectedIndex = 0;
        ddlCity.SelectedIndex = 0;
        txtDistance.Text = string.Empty;
        ddlCity.Enabled = true;
        ddlLocation.Enabled = true;
        btnSubmit.Text = "Submit";
        ddlCity.Enabled = true;
        ddlLocation.Enabled = true;
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clear();
        BindLocationCityMapping();
    }

    protected async void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDistance.Text != "" && ddlLocation.SelectedIndex != 0 && ddlCity.SelectedIndex !=0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response;
                    string QueryType = string.Empty;
                   

                    if (btnSubmit.Text.Trim() == "Submit")
                    {
                        QueryType = "Insert";
                       
                    }
                    else
                    {
                        QueryType = "Update";
                       
                    }
                    var locationCityMapping1 = new locationCityMapping()
                    {
                        QueryType = QueryType,
                        CityId = ddlCity.SelectedValue.Trim(),
                        LocationId = ddlLocation.SelectedValue.Trim(),
                        Distance = txtDistance.Text.Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim()
                    };
                    response = await client.PostAsJsonAsync("LocationCityMapping", locationCityMapping1);

                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                        if (StatusCode == 1)
                        {

                            BindLocationCityMapping();
                            clear();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        }
                    }
                    else
                    {
                        var Errorresponse = response.Content.ReadAsStringAsync().Result;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                    }
                }
            }
            else
            {
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


  

    protected  void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvLocMapping.DataKeys[gvrow.RowIndex].Value.ToString();
            Label CityId = (Label)gvrow.FindControl("lblCityId");
            Label locationId = (Label)gvrow.FindControl("lbllocationId");
            Label Distance = (Label)gvrow.FindControl("lblDistance");
            ddlCity.SelectedValue = CityId.Text.Trim();
            ddlLocation.SelectedValue = locationId.Text.Trim();
            txtDistance.Text = Distance.Text.Trim();
            ddlLocation.Enabled = false;
            ddlCity.Enabled = false;
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    //protected async void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //            ImageButton lnkbtn = sender as ImageButton;
    //            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
    //            string sTesfg = gvLocMapping.DataKeys[gvrow.RowIndex].Value.ToString();
    //            Label CityId = (Label)gvrow.FindControl("lblCityId");
    //            Label locationId = (Label)gvrow.FindControl("lbllocationId");
    //            Label Distance = (Label)gvrow.FindControl("lblDistance");
    //            ddlCity.SelectedValue = CityId.Text.Trim();
    //            ddlLocation.SelectedValue = locationId.Text.Trim();
    //            txtDistance.Text = Distance.Text.Trim();

    //            HttpResponseMessage response;

    //            var locationCityMapping1 = new locationCityMapping()
    //            {
    //                QueryType = "Delete",
    //                CityId = ddlCity.SelectedValue.Trim(),
    //                LocationId = ddlLocation.SelectedValue.Trim(),
    //                Distance = txtDistance.Text.Trim(),
    //                CreatedBy = hfCreatedBy.Value.Trim()
    //            };
    //            response = await client.PostAsJsonAsync("LocationCityMapping", locationCityMapping1);
    //         if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
    //                if (StatusCode == 1)
    //                {

    //                    BindLocationCityMapping();
    //                    clear();
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

    //                }
    //            }
    //            else
    //            {
    //                var Errorresponse = response.Content.ReadAsStringAsync().Result;

    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
    //            }
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }
    //}

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divEntry.Visible = true;
        lbtnNew.Visible = false;
        ddlCity.Enabled = true;
        ddlLocation.Enabled = true;
    }

    //protected async void ImgBtnUndo_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //            ImageButton lnkbtn = sender as ImageButton;
    //            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
    //            string sTesfg = gvLocMapping.DataKeys[gvrow.RowIndex].Value.ToString();
    //            Label CityId = (Label)gvrow.FindControl("lblCityId");
    //            Label locationId = (Label)gvrow.FindControl("lbllocationId");
    //            Label Distance = (Label)gvrow.FindControl("lblDistance");
    //            ddlCity.SelectedValue = CityId.Text.Trim();
    //            ddlLocation.SelectedValue = locationId.Text.Trim();
    //            txtDistance.Text = Distance.Text.Trim();

    //            HttpResponseMessage response;

    //            var locationCityMapping1 = new locationCityMapping()
    //            {
    //                QueryType = "ReActive",
    //                CityId = ddlCity.SelectedValue.Trim(),
    //                LocationId = ddlLocation.SelectedValue.Trim(),
    //                Distance = txtDistance.Text.Trim(),
    //                CreatedBy = hfCreatedBy.Value.Trim()
    //            };
    //            response = await client.PostAsJsonAsync("LocationCityMapping", locationCityMapping1);
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
    //                if (StatusCode == 1)
    //                {

    //                    BindLocationCityMapping();
    //                    clear();
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

    //                }
    //            }
    //            else
    //            {
    //                var Errorresponse = response.Content.ReadAsStringAsync().Result;

    //                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
    //            }
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
    //    }

    //}

    protected void gvLocMapping_RowDataBound(object sender, GridViewRowEventArgs e)
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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sTesfg = gvLocMapping.DataKeys[gvrow.RowIndex].Value.ToString();
                Label CityId = (Label)gvrow.FindControl("lblCityId");
                Label locationId = (Label)gvrow.FindControl("lbllocationId");
                Label Distance = (Label)gvrow.FindControl("lblDistance");
                ddlCity.SelectedValue = CityId.Text.Trim();
                ddlLocation.SelectedValue = locationId.Text.Trim();
                txtDistance.Text = Distance.Text.Trim();

                HttpResponseMessage response;

                var locationCityMapping1 = new locationCityMapping()
                {
                    QueryType = "Delete",
                    CityId = ddlCity.SelectedValue.Trim(),
                    LocationId = ddlLocation.SelectedValue.Trim(),
                    Distance = txtDistance.Text.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };
                response = client.PostAsJsonAsync("LocationCityMapping", locationCityMapping1).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindLocationCityMapping();
                        clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

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

    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sTesfg = gvLocMapping.DataKeys[gvrow.RowIndex].Value.ToString();
                Label CityId = (Label)gvrow.FindControl("lblCityId");
                Label locationId = (Label)gvrow.FindControl("lbllocationId");
                Label Distance = (Label)gvrow.FindControl("lblDistance");
                ddlCity.SelectedValue = CityId.Text.Trim();
                ddlLocation.SelectedValue = locationId.Text.Trim();
                txtDistance.Text = Distance.Text.Trim();

                HttpResponseMessage response;

                var locationCityMapping1 = new locationCityMapping()
                {
                    QueryType = "ReActive",
                    CityId = ddlCity.SelectedValue.Trim(),
                    LocationId = ddlLocation.SelectedValue.Trim(),
                    Distance = txtDistance.Text.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()
                };
                response = client.PostAsJsonAsync("LocationCityMapping", locationCityMapping1).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {

                        BindLocationCityMapping();
                        clear();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

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
}