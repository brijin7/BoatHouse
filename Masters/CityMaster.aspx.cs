using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CityMaster : System.Web.UI.Page
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
				hfCreatedBy.Value = Session["UserId"].ToString();

				BindCIty();
				GetState();
				GetZone();
				getCity();
				GetDistrict();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

	//Class------
	public class CityMappingmstr
	{
		public string QueryType { get; set; }
		public string CityId { get; set; }
		public string CityName { get; set; }
		public string StateId { get; set; }
		public string StateName { get; set; }
		public string ZoneId { get; set; }
		public string ZoneName { get; set; }
		public string DistrictId { get; set; }
		public string DistrictName { get; set; }
		public string CityDescription { get; set; }
		public string CreatedBy { get; set; }
		public string ActiveStatus { get; set; }
	}

	//DropDown-------------

	public async void GetState()
	{
		try
		{
			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Clear();

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = await client.GetAsync("ConfigMstr/DDLState");

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

							ddlstateId.DataSource = dt;
							ddlstateId.DataValueField = "ConfigId";
							ddlstateId.DataTextField = "ConfigName";
							ddlstateId.DataBind();

						}
						else
						{

							ddlstateId.DataBind();
						}
						ddlstateId.Items.Insert(0, new ListItem("Select State", "0"));
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
	public async void GetZone()
	{
		try
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Clear();

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = await client.GetAsync("ConfigMstr/DDLZone");

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

							ddlZoneId.DataSource = dt;
							ddlZoneId.DataValueField = "ConfigId";
							ddlZoneId.DataTextField = "ConfigName";
							ddlZoneId.DataBind();

						}
						else
						{

							ddlZoneId.DataBind();
						}
						ddlZoneId.Items.Insert(0, new ListItem("Select Zone", "0"));
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

	public async void GetDistrict()
	{
		try
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Clear();

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = await client.GetAsync("ConfigMstr/DDLDistrict");

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

							ddlDistrictId.DataSource = dt;
							ddlDistrictId.DataValueField = "ConfigId";
							ddlDistrictId.DataTextField = "ConfigName";
							ddlDistrictId.DataBind();


						}
						else
						{

							ddlDistrictId.DataBind();
						}
						ddlDistrictId.Items.Insert(0, new ListItem("Select District", "0"));

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

	public async void getCity()
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

	public async void BindCIty()
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
				response = await client.GetAsync("CityMap/ListAll");
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
							gvCityMapping.DataSource = dt;
							gvCityMapping.DataBind();
							lblGridMsg.Text = string.Empty;

						}
						else
						{
							gvCityMapping.DataBind();
							lblGridMsg.Text = "No  Records Found";
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
				if (btnSubmit.Text.Trim() == "Submit")
				{
					var CityMappingmstr = new CityMappingmstr()
					{
						QueryType = "Insert",
						CityId = ddlCity.SelectedValue.Trim(),
						DistrictId = ddlDistrictId.SelectedValue.Trim(),
						ZoneId = ddlZoneId.SelectedValue.Trim(),
						StateId = ddlstateId.SelectedValue.Trim(),
						CityDescription = txtcityDes.Text,
						CreatedBy = hfCreatedBy.Value.Trim()
					};
					response = await client.PostAsJsonAsync("CityMapping", CityMappingmstr);

				}
				else
				{
					var CityMappingmstr = new CityMappingmstr()
					{
						QueryType = "Update",
						CityId = ddlCity.SelectedValue.Trim(),
						DistrictId = ddlDistrictId.SelectedValue.Trim(),
						ZoneId = ddlZoneId.SelectedValue.Trim(),
						StateId = ddlstateId.SelectedValue.Trim(),
						CityDescription = txtcityDes.Text,
						CreatedBy = hfCreatedBy.Value.Trim()
					};
					response = await client.PostAsJsonAsync("CityMapping", CityMappingmstr);

				}

				if (response.IsSuccessStatusCode)
				{
					var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
					int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
					string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
					if (StatusCode == 1)
					{
						if (btnSubmit.Text.Trim() == "Submit")
						{
							clearInputs();
							BindCIty();
							divGrid.Visible = true;
							divEntry.Visible = false;
							lbtnNew.Visible = true;
							ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
						}
						else
						{
							divGrid.Visible = true;
							divEntry.Visible = false;
							lbtnNew.Visible = true;
							BindCIty();
							clearInputs();
							btnSubmit.Text = "Submit";
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
		}
		catch (Exception ex)
		{
			ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
		}
	}

	protected void btnCancel_Click(object sender, EventArgs e)
	{
		clearInputs();
		BindCIty();

	}

	public void clearInputs()
	{
		ddlCity.SelectedIndex = 0;
		ddlDistrictId.SelectedIndex = 0;
		ddlstateId.SelectedIndex = 0;
		ddlZoneId.SelectedIndex = 0;
		txtcityDes.Text = string.Empty;
		divEntry.Visible = false;
		divGrid.Visible = true;
		lbtnNew.Visible = true;
		btnSubmit.Text = "Submit";


	}

	protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
	{
		try
		{
			divGrid.Visible = false;
			divEntry.Visible = true;
			lbtnNew.Visible = false;

			ImageButton lnkbtn = sender as ImageButton;
			GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
			string sTesfg = gvCityMapping.DataKeys[gvrow.RowIndex].Value.ToString();
			Label CityId = (Label)gvrow.FindControl("lblCityId");
			Label DistrictId = (Label)gvrow.FindControl("lblDistrictId");
			Label ZoneId = (Label)gvrow.FindControl("lblZoneId");
			Label StateId = (Label)gvrow.FindControl("lblStateId");
			Label CityDescription = (Label)gvrow.FindControl("lblCityDescription");

			ddlCity.SelectedValue = CityId.Text;
			ddlDistrictId.SelectedValue = DistrictId.Text;
			ddlZoneId.SelectedValue = ZoneId.Text;
			ddlstateId.SelectedValue = StateId.Text;
			txtcityDes.Text = CityDescription.Text;

			btnSubmit.Text = "Update";


		}
		catch (Exception ex)
		{
			ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
		}
	}
	protected void lbtnNew_Click(object sender, EventArgs e)
	{
		divEntry.Visible = true;
		divGrid.Visible = false;
		lbtnNew.Visible = false;
		btnSubmit.Text = "Submit";


	}

	protected void gvCityMapping_RowDataBound(object sender, GridViewRowEventArgs e)
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
			string sTesfg = gvCityMapping.DataKeys[gvrow.RowIndex].Value.ToString();
			Label StateId = (Label)gvrow.FindControl("lblStateId");
			Label StateName = (Label)gvrow.FindControl("lblStateName");
			Label ZoneId = (Label)gvrow.FindControl("lblZoneId");
			Label ZoneName = (Label)gvrow.FindControl("lblZoneName");
			Label DistrictId = (Label)gvrow.FindControl("lblDistrictId");
			Label DistrictName = (Label)gvrow.FindControl("lblDistrictName");
			Label CityId = (Label)gvrow.FindControl("lblCityId");
			Label CityName = (Label)gvrow.FindControl("lblCityName");
			Label CityDescription = (Label)gvrow.FindControl("lblCityDescription");
			ddlstateId.SelectedValue = StateId.Text;
			ddlZoneId.SelectedValue = ZoneId.Text;
			ddlDistrictId.SelectedValue = DistrictId.Text;
			ddlCity.SelectedValue = CityId.Text;
			txtcityDes.Text = CityDescription.Text;
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				var CityMappingmstr = new CityMappingmstr()
				{
					QueryType = "Delete",
					StateId = ddlstateId.SelectedValue,
					ZoneId = ddlZoneId.SelectedValue,
					DistrictId = ddlDistrictId.SelectedValue,
					CityId = ddlCity.SelectedValue,
					CityDescription = txtcityDes.Text,
					CreatedBy = hfCreatedBy.Value.Trim()
				};
				HttpResponseMessage response;
				string sMSG = string.Empty;
				response = client.PostAsJsonAsync("CityMapping", CityMappingmstr).Result;
				sMSG = "City Master Details Deleted Successfully";
				if (response.IsSuccessStatusCode)
				{
					var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
					int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
					string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
					if (StatusCode == 1)
					{
						BindCIty();
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
			string sTesfg = gvCityMapping.DataKeys[gvrow.RowIndex].Value.ToString();
			Label StateId = (Label)gvrow.FindControl("lblStateId");
			Label StateName = (Label)gvrow.FindControl("lblStateName");
			Label ZoneId = (Label)gvrow.FindControl("lblZoneId");
			Label ZoneName = (Label)gvrow.FindControl("lblZoneName");
			Label DistrictId = (Label)gvrow.FindControl("lblDistrictId");
			Label DistrictName = (Label)gvrow.FindControl("lblDistrictName");
			Label CityId = (Label)gvrow.FindControl("lblCityId");
			Label CityName = (Label)gvrow.FindControl("lblCityName");
			Label CityDescription = (Label)gvrow.FindControl("lblCityDescription");

			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var CityMappingmstr = new CityMappingmstr()
				{
					QueryType = "ReActive",
					StateId = StateId.Text,
					ZoneId = ZoneId.Text,
					DistrictId = DistrictId.Text,
					CityId = CityId.Text,
					CityDescription = CityDescription.Text,
					CreatedBy = hfCreatedBy.Value.Trim()
				};
				HttpResponseMessage response;
				string sMSG = string.Empty;
				response = client.PostAsJsonAsync("CityMapping", CityMappingmstr).Result;

				if (response.IsSuccessStatusCode)
				{
					var submitresponse = response.Content.ReadAsStringAsync().Result;
					int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
					string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
					if (StatusCode == 1)
					{
						BindCIty();
						clearInputs();
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
}