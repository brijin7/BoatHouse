using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

public partial class BoatType : System.Web.UI.Page
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
                hfCreatedBy.Value = Session["UserId"].ToString().Trim();
                txtBoatType.Focus();
                BindBoatType();
                GetBoatHouse();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }

    //Creating class for Boat Type Master Form
    public class boatTypeMaster
    {
        public string QueryType { get; set; }
        public string BoatTypeId { get; set; }
        public string BoatType { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }

        public string CreatedBy { get; set; }


    }


    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = true;
        divEntry.Visible = true;
        lbtnNew.Visible = false;
        Clear();
        btnSubmit.Text = "Submit";
    }

    // Binding Boat Type Master Details
    public void BindBoatType()
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
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("BoatTypeMstr/BHId", BoatHouseId).Result;

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
                            GvBoatType.DataSource = dt;
                            GvBoatType.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;

                        }
                        else
                        {

                            GvBoatType.DataBind();
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

    //Getting Boat House Details in Dropdown
    public void GetBoatHouse()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("ddlBoatHouse?CorpId=" + Session["CorpId"].ToString() +"").Result;

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
                            ddlboathouse.DataSource = dt;
                            ddlboathouse.DataValueField = "BoatHouseId";
                            ddlboathouse.DataTextField = "BoatHouseName";
                            ddlboathouse.DataBind();

                        }
                        else
                        {
                            ddlboathouse.DataBind();
                        }
                    }
                    else
                    {

                        //  lblGridMsg.Text = ResponseMsg;

                    }
                    ddlboathouse.Items.Insert(0, new ListItem("Select Boat House", "0"));
                }
                else
                {

                    //  lblGridMsg.Text = response.ToString();
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
                    var BoatType = new boatTypeMaster()
                    {
                        QueryType = "Insert",
                        BoatTypeId = "0",
                        BoatType = txtBoatType.Text.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim()

                    };
                    response = client.PostAsJsonAsync("BoatTypeMaster", BoatType).Result;
                    sMSG = "Boat Type Inserted Successfully";

                }
                else
                {

                    var BoatType = new boatTypeMaster()
                    {
                        QueryType = "Update",
                        BoatTypeId = txtboatTypeId.Text.Trim(),
                        BoatType = txtBoatType.Text.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim()

                    };
                    response = client.PostAsJsonAsync("BoatTypeMaster", BoatType).Result;
                    sMSG = "Boat Type Updated Successfully";
                }

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindBoatType();
                        Clear();
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

    // Clearing the Fields
    public void Clear()
    {
        ddlboathouse.SelectedIndex = 0;
        txtBoatType.Text = string.Empty;
    }

    //Cancelling the Process
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        BindBoatType();
        btnSubmit.Text = "Submit";
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
    }

    protected void GvBoatType_RowDataBound(Object sender, GridViewRowEventArgs e)
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
            string boatTypeId = GvBoatType.DataKeys[gvrow.RowIndex].Value.ToString();

            Label BoatType = (Label)gvrow.FindControl("lblboatTypeName");
            Label BoatHouseId = (Label)gvrow.FindControl("lblboatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var boatTypeMaster = new boatTypeMaster()
                {

                    QueryType = "Delete",
                    BoatTypeId = boatTypeId.ToString().Trim(),
                    BoatType = BoatType.Text,
                    BoatHouseId = BoatHouseId.Text,
                    BoatHouseName = BoatHouseName.Text,
                    CreatedBy = hfCreatedBy.Value



                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("BoatTypeMaster", boatTypeMaster).Result;
                sMSG = "BoatType Master Details Deleted Successfully";


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatType();
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
            string boatTypeId = GvBoatType.DataKeys[gvrow.RowIndex].Value.ToString();

            Label BoatType = (Label)gvrow.FindControl("lblboatTypeName");
            Label BoatHouseId = (Label)gvrow.FindControl("lblboatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var boatTypeMaster = new boatTypeMaster()
                {

                    QueryType = "ReActive",
                    BoatTypeId = boatTypeId.ToString().Trim(),
                    BoatType = BoatType.Text,
                    BoatHouseId = BoatHouseId.Text,
                    BoatHouseName = BoatHouseName.Text,
                    CreatedBy = hfCreatedBy.Value



                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("BoatTypeMaster", boatTypeMaster).Result;
                sMSG = "BoatType Master Details Deleted Successfully";


                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindBoatType();
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
            string sTesfg = GvBoatType.DataKeys[gvrow.RowIndex].Value.ToString();
            Label BoatTypeId = (Label)gvrow.FindControl("lblboatTypeId");
            Label BoatType = (Label)gvrow.FindControl("lblboatTypeName");
            Label BoatHouseId = (Label)gvrow.FindControl("lblboatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");

            txtboatTypeId.Text = BoatTypeId.Text;
            txtBoatType.Text = BoatType.Text;
            ddlboathouse.SelectedValue = BoatHouseId.Text;
            hfboathouse.Value = BoatHouseName.Text;


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
}
