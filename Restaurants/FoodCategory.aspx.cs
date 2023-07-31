using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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

public partial class Restaurants_FoodCategory : System.Web.UI.Page
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
				BindFoodCategory();
				txtCategoryId.Focus();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = true;
        divEntry.Visible = true;
        lbtnNew.Visible = false;
        txtCategoryName.Text = "";
        btnSubmit.Text = "Submit";
    }

    public void BindFoodCategory()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var cat = new FoodCategory()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("FoodCategoryMstr/BHId", cat).Result;

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
                            GvFoodCategory.DataSource = dt;
                            GvFoodCategory.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            GvFoodCategory.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg;
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
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


    //Submitting the Values
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

                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var Cat = new FoodCategory()
                    {
                        QueryType = "Insert",
                        CategoryId = "0",
                        CategoryName = txtCategoryName.Text.Trim(),

                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("FoodCategoryMstr", Cat).Result;
                    sMSG = "Category Inserted Successfully";

                }
                else
                {
                    var Cat = new FoodCategory()
                    {
                        QueryType = "Update",
                        CategoryId = txtCategoryId.Text.Trim(),
                        CategoryName = txtCategoryName.Text.Trim(),

                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        CreatedBy = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("FoodCategoryMstr", Cat).Result;
                    sMSG = "Category Updated Successfully";
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindFoodCategory();
                        txtCategoryName.Text = "";
                        btnSubmit.Text = "Submit";
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        divGrid.Visible = true;
                        divEntry.Visible = false;
                        lbtnNew.Visible = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        divGrid.Visible = true;
                        divEntry.Visible = true;
                        lbtnNew.Visible = false;
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
        

    //Cancelling the Process
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BindFoodCategory();

        txtCategoryName.Text = "";        
        btnSubmit.Text = "Submit";
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
    }

    protected void GvFoodCategory_RowDataBound(Object sender, GridViewRowEventArgs e)
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
           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var cat = new FoodCategory()
                {
                    QueryType = "Delete",
                    CategoryId = GvFoodCategory.DataKeys[gvrow.RowIndex]["CategoryId"].ToString().Trim(),
                    CategoryName = GvFoodCategory.DataKeys[gvrow.RowIndex]["CategoryName"].ToString().Trim(),

                    CreatedBy = Session["UserId"].ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("FoodCategoryMstr", cat).Result;
                sMSG = "Category Deleted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindFoodCategory();
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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Cat = new FoodCategory()
                {
                    QueryType = "ReActive",
                    CategoryId = GvFoodCategory.DataKeys[gvrow.RowIndex]["CategoryId"].ToString().Trim(),
                    CategoryName = GvFoodCategory.DataKeys[gvrow.RowIndex]["CategoryName"].ToString().Trim(),

                    CreatedBy = Session["UserId"].ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("FoodCategoryMstr", Cat).Result;
                sMSG = "Category Deleted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindFoodCategory();
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

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divGrid.Visible = true;
            divEntry.Visible = true;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            txtCategoryId.Text = GvFoodCategory.DataKeys[gvrow.RowIndex]["CategoryId"].ToString().Trim();
            txtCategoryName.Text = GvFoodCategory.DataKeys[gvrow.RowIndex]["CategoryName"].ToString().Trim();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    public class FoodCategory
    {
        public string QueryType { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string CreatedBy { get; set; }
    }
}