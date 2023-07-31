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

public partial class Boating_PrintingInstruction : System.Web.UI.Page
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
                BindPrintInstruction();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
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
                if (btnSubmit.Text.Trim() == "Submit")
                {
                    var PrintInstruction = new PrintInstruction()
                    {
                        QueryType = "Insert",
                        UniqueId = "0",
                        ServiceType = ddlServiceName.SelectedValue.Trim(),
                        InstructionDtl = txtInstructionDtl.Text.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        Createdby = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("PrintingInstruction", PrintInstruction).Result;
                    sMSG = "Printing Instruction Inserted Successfully";
                }
                else
                {
                    var PrintInstructionUpdate = new PrintInstruction()
                    {
                        QueryType = "Update",
                        UniqueId = Session["UniqueIdEdit"].ToString().Trim(),
                        ServiceType = ddlServiceName.SelectedValue.Trim(),
                        InstructionDtl = txtInstructionDtl.Text.Trim(),
                        BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                        BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                        Createdby = Session["UserId"].ToString().Trim()
                    };
                    response = client.PostAsJsonAsync("PrintingInstruction", PrintInstructionUpdate).Result;
                    sMSG = "Printing Instruction Updated Successfully";
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
                            BindPrintInstruction();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            clearInputs();
                            BindPrintInstruction();
                            btnSubmit.Text = "Submit";
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + sMSG.Trim() + "');", true);
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                            gvPrintInst.Visible = true;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Printing Instruction Details Already Exists.');", true);
                        txtInstructionDtl.Text = string.Empty;
                    }
                }
                else
                {
                    // lblGridMsg.Text = response.ToString();
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
        divGrid.Visible = true;
        divEntry.Visible = false;
        lbtnNew.Visible = true;
        gvPrintInst.Visible = true;
    }

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divEntry.Visible = true;
        divGrid.Visible = false;
        lbtnNew.Visible = false;
        gvPrintInst.Visible = false;
        clearInputs();
    }

    public void clearInputs()
    {
        ddlServiceName.SelectedIndex = 0;
        txtInstructionDtl.Text = string.Empty;
        btnSubmit.Text = "Submit";
    }

    public class PrintInstruction
    {
        public string QueryType { get; set; }
        public string UniqueId { get; set; }
        public string ServiceType { get; set; }
        public string ServiceName { get; set; }
        public string InstructionDtl { get; set; }
        public string BoatHouseId { get; set; }
        public string BoatHouseName { get; set; }
        public string ActiveStatus { get; set; }
        public string Createdby { get; set; }
    }
    public void BindPrintInstruction()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatHouseId = new PrintInstruction()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("PrintingInstructionGrid", BoatHouseId).Result;

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
                            gvPrintInst.DataSource = dt;
                            gvPrintInst.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divGrid.Visible = true;
                            gvPrintInst.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            gvPrintInst.DataBind();
                            lblGridMsg.Text = "No Records Found";
                            divGrid.Visible = true;
                            divEntry.Visible = false;
                            lbtnNew.Visible = true;
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg.ToString();
                        divGrid.Visible = false;
                        divEntry.Visible = true;
                        btnSubmit.Text = "Submit";
                        lbtnNew.Visible = false;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnAddEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divGrid.Visible = false;
            divEntry.Visible = true;
            lbtnNew.Visible = false;
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueIdEdit = gvPrintInst.DataKeys[gvrow.RowIndex].Value.ToString();
            Session["UniqueIdEdit"] = UniqueIdEdit;
            Label ServiceName = (Label)gvrow.FindControl("lblServicetypeID");
            Label InstructionDtl = (Label)gvrow.FindControl("lblInstruction");
            ddlServiceName.SelectedValue = ServiceName.Text;
            txtInstructionDtl.Text = InstructionDtl.Text;
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
            string UniqueId = gvPrintInst.DataKeys[gvrow.RowIndex].Value.ToString();
            Session["UniqueId"] = UniqueId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var PrintInstructionDel = new PrintInstruction()
                {
                    QueryType = "Delete",
                    UniqueId = Session["UniqueId"].ToString().Trim(),
                    ServiceType = gvPrintInst.DataKeys[gvrow.RowIndex]["ServiceType"].ToString(),
                    InstructionDtl = gvPrintInst.DataKeys[gvrow.RowIndex]["InstructionDtl"].ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    Createdby = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response;
                string sMSG = string.Empty;

                response = client.PostAsJsonAsync("PrintingInstruction", PrintInstructionDel).Result;
                sMSG = "Printing Instruction Deleted Successfully";

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindPrintInstruction();
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

    protected void gvPrintInst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // for example, the row contains a certain property
                if (((Label)e.Row.FindControl("lblActiveStatus")).Text == "D")
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnAddEdit");
                    btnEdit.Visible = false;
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("ImgBtnDelete");
                    btnDelete.Visible = false;
                    LinkButton btnUndo = (LinkButton)e.Row.FindControl("ImgBtnUndo");
                    btnUndo.Visible = true;
                }

                else
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("ImgBtnAddEdit");
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



    protected void ImgBtnUndo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueId = gvPrintInst.DataKeys[gvrow.RowIndex].Value.ToString();
            Session["UniqueId"] = UniqueId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var PrintInstructionDel = new PrintInstruction()
                {
                    QueryType = "ReActive",
                    UniqueId = Session["UniqueId"].ToString().Trim(),
                    ServiceType = gvPrintInst.DataKeys[gvrow.RowIndex]["ServiceType"].ToString(),
                    InstructionDtl = gvPrintInst.DataKeys[gvrow.RowIndex]["InstructionDtl"].ToString(),
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim(),
                    BoatHouseName = Session["BoatHouseName"].ToString().Trim(),
                    Createdby = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response;
                response = client.PostAsJsonAsync("PrintingInstruction", PrintInstructionDel).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindPrintInstruction();
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