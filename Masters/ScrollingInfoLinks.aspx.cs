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

public partial class ScrollingInfoLinks : System.Web.UI.Page
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
				BindScrollingInfo();
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public class scrollinginfo
    {
        public string QueryType { get; set; }
        public string InfoId { get; set; }
        public string Information { get; set; }
        public string InfoLinkURL { get; set; }
        public string InfoType { get; set; }
        public string CreatedBy { get; set; }
        public string ActiveStatus { get; set; }
    }

    public async void BindScrollingInfo()
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

                response = await client.GetAsync("ScrollingInfo/ListAll");
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
                            gvscrollinginfo.DataSource = dt;
                            gvscrollinginfo.DataBind();
                            lblGridMsg.Text = string.Empty;
                        }
                        else
                        {
                            gvscrollinginfo.DataBind();
                            gvscrollinginfo.DataSource = dt;
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
                    var scrollinginfo = new scrollinginfo()
                    {
                        QueryType = "Insert",
                        InfoId = "0",
                        Information = txtinformation.Text.Trim(),
                        InfoLinkURL = txtInfolinkurl.Text.Trim(),
                        InfoType = RbtInfoType.SelectedValue.Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim()


                    };

                    response = await client.PostAsJsonAsync("ScrollingInfo", scrollinginfo);


                }
                else
                {
                    var scrollinginfo = new scrollinginfo()
                    {

                        QueryType = "Update",
                        InfoId = txtInfoId.Text.Trim(),
                        Information = txtinformation.Text.Trim(),
                        InfoLinkURL = txtInfolinkurl.Text.Trim(),
                        InfoType = RbtInfoType.SelectedValue.Trim(),
                        CreatedBy = hfCreatedBy.Value.Trim()

                    };

                    response = await client.PostAsJsonAsync("ScrollingInfo", scrollinginfo);


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

                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            BindScrollingInfo();
                            clearinputs();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        else
                        {
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            BindScrollingInfo();
                            clearinputs();
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
        clearinputs();
        BindScrollingInfo();
        divEntry.Visible = false;
        divGrid.Visible = true;
        lbtnNew.Visible = true;
        btnSubmit.Text = "Submit";

    }
    public void clearinputs()
    {
        txtInfolinkurl.Text = string.Empty;
        txtinformation.Text = string.Empty;

    }

    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divEntry.Visible = true;
            divGrid.Visible = false;
            lbtnNew.Visible = false;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvscrollinginfo.DataKeys[gvrow.RowIndex].Value.ToString();
            Label InfoId = (Label)gvrow.FindControl("lblInfoId");
            Label Information = (Label)gvrow.FindControl("lblInformation");
            Label InfoLinkUrl = (Label)gvrow.FindControl("lblInfoLinkUrl");
            Label InfoType = (Label)gvrow.FindControl("lblInfoType");

            txtInfoId.Text = InfoId.Text;
            txtinformation.Text = Information.Text;
            txtInfolinkurl.Text = InfoLinkUrl.Text;
            if (InfoType.Text == "Information")
            {
                RbtInfoType.SelectedValue = "I";
            }
            else
            {
                RbtInfoType.SelectedValue = "L";
            }
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
        BindScrollingInfo();
        clearinputs();
    }



    protected void gvscrollinginfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

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
            string InfoId = gvscrollinginfo.DataKeys[gvrow.RowIndex].Value.ToString();

            Label Information = (Label)gvrow.FindControl("lblInformation");
            Label InfoLinkUrl = (Label)gvrow.FindControl("lblInfoLinkUrl");
            Label InfoType = (Label)gvrow.FindControl("lblInfoType");


            txtinformation.Text = Information.Text;
            txtInfolinkurl.Text = InfoLinkUrl.Text;
            if (InfoType.Text == "Information")
            {
                RbtInfoType.SelectedValue = "I";
            }
            else
            {
                RbtInfoType.SelectedValue = "L";
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var scrollinginfo = new scrollinginfo()
                {

                    QueryType = "Delete",
                    InfoId = InfoId.ToString().Trim(),
                    Information = txtinformation.Text.Trim(),
                    InfoLinkURL = txtInfolinkurl.Text.Trim(),
                    InfoType = RbtInfoType.SelectedValue.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()

                };
                HttpResponseMessage response;
                string sMSG = string.Empty;
                response = client.PostAsJsonAsync("ScrollingInfo", scrollinginfo).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindScrollingInfo();
                        clearinputs();
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
            string InfoId = gvscrollinginfo.DataKeys[gvrow.RowIndex].Value.ToString();

            Label Information = (Label)gvrow.FindControl("lblInformation");
            Label InfoLinkUrl = (Label)gvrow.FindControl("lblInfoLinkUrl");
            Label InfoType = (Label)gvrow.FindControl("lblInfoType");


            txtinformation.Text = Information.Text;
            txtInfolinkurl.Text = InfoLinkUrl.Text;
            if (InfoType.Text == "Information")
            {
                RbtInfoType.SelectedValue = "I";
            }
            else
            {
                RbtInfoType.SelectedValue = "L";
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var scrollinginfo = new scrollinginfo()
                {

                    QueryType = "ReActive",
                    InfoId = InfoId.ToString().Trim(),
                    Information = txtinformation.Text.Trim(),
                    InfoLinkURL = txtInfolinkurl.Text.Trim(),
                    InfoType = RbtInfoType.SelectedValue.Trim(),
                    CreatedBy = hfCreatedBy.Value.Trim()

                };
                HttpResponseMessage response;
                string sMSG = string.Empty;
                response =  client.PostAsJsonAsync("ScrollingInfo", scrollinginfo).Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindScrollingInfo();
                        clearinputs();
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
}