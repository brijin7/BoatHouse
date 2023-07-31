using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TaxMaster : System.Web.UI.Page
{
    public string GetAntiForgeryToken()
    {
        string cookieToken, formToken;
        AntiForgery.GetTokens(null, out cookieToken, out formToken);
        ViewState["__AntiForgeryCookie"] = cookieToken;
        return formToken;

    }

    IFormatProvider objCulture = new System.Globalization.CultureInfo("en-GB", true);

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
                BindTaxMaster();
                //BindBoatHouseName();
                GetCorporateOffice();
                DeleteTemp();
                //CHANGES

            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public void GetCorporateOffice()
    {
        try
        {
           
            ddlCorpId.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                var BranchType = new FA_CommonMethod()
                {
                    QueryType = "Dropdown",
                    ServiceType = "ddlCorporateOffice",
                    CorpId = "",
                    Input1 = "",
                    Input2 = "",
                    Input3 = "",
                    Input4 = "",
                    Input5 = "",

                };

                HttpResponseMessage response = client.PostAsJsonAsync("CM_CommonReport", BranchType).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Table"].ToString();
                    DataTable dtExists = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                    if (dtExists.Rows.Count > 0)
                    {
                        ddlCorpId.DataSource = dtExists;
                        ddlCorpId.DataValueField = "CorpId";
                        ddlCorpId.DataTextField = "CorpName";
                        ddlCorpId.DataBind();

                    }
                    else
                    {
                        ddlCorpId.DataBind();
                    }
                    ddlCorpId.Items.Insert(0, "Select Corporate Office");
                    ddlBoatHouseId.Items.Insert(0, "Select Boat House");

                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }

    protected void ddlCorpId_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBoatHouseName();
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

                HttpResponseMessage response = client.GetAsync("ddlBoatHouse?CorpId=" +ddlCorpId.SelectedValue +"").Result;

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

    /******************************Add**************************/

    public void clearAddinputs()
    {
        ddlBoatHouseId.Enabled = false;
        ddlServiceName.Enabled = false;
      
        ddlCorpId.Enabled = false;
        txtTaxDesc.Text = string.Empty;
        txtTaxPerc.Text = string.Empty;
        BtnAdd.Text = "Add";
    }

    //BINDING TAX DETAILS - SERVICENAME, TAX DESCRIPTION, TAX %
    public void BindAddTaxMaster()
    {
        try
        {
            btnSubmit.Enabled = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BindTempTax = new taxmaster()
                {
                    BoatHouseId = ddlBoatHouseId.SelectedValue
                };
                HttpResponseMessage response = client.PostAsJsonAsync("TaxMstr/AddListAll", BindTempTax).Result;

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
                            gvAddTaxMaster.DataSource = dt;
                            gvAddTaxMaster.DataBind();
                            lblGridMsg.Text = string.Empty;
                            btnSubmit.Enabled = true;
                        }
                        else
                        {
                            gvAddTaxMaster.DataBind();
                            lblGridMsg.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        gvAddTaxMaster.DataBind();
                        lblGridMsg.Text = ResponseMsg;
                        ddlBoatHouseId.SelectedIndex = 0;
                        ddlBoatHouseId.Enabled = true;
                        ddlServiceName.SelectedIndex = 0;
                        ddlServiceName.Enabled = true;
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


    /******************************Submit**************************/


    //BINDING FINALLY INSERTED RECORDS
    public void BindTaxMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BindTempTax = new taxmaster()
                {
                    BoatHouseId =""
                };
                HttpResponseMessage response = client.PostAsJsonAsync("TaxMstr/InsertListAll", BindTempTax).Result;


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
                            gvTaxMaster.DataSource = dt;
                            gvTaxMaster.DataBind();
                            lblGridMsg.Text = string.Empty;
                            divnew.Visible = false;
                            divGrid.Visible = true;
                            lbtnNew.Visible = true;
                        }
                        else
                        {
                            gvTaxMaster.DataBind();
                            divnew.Visible = true;
                            divGrid.Visible = false;
                            lbtnNew.Visible = false;
                        }
                    }
                    else
                    {
                        lblGridMsg.Text = ResponseMsg;
                        divnew.Visible = true;
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

    //DELETEING TEMP RECORDS IN ACTIVESTAUS='T'
    public void DeleteTemp()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync("TaxMstr/DeleteTemp").Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1) { }
                    else { }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void gvTaxMaster_DataBound(object sender, EventArgs e)
    {
        for (int currentRowIndex = 0; currentRowIndex < gvTaxMaster.Rows.Count; currentRowIndex++)
        {
            GridViewRow currentRow = gvTaxMaster.Rows[currentRowIndex];
            CombineColumnCells(currentRow, 0, "TaxId");
        }
    }

    private void CombineColumnCells(GridViewRow currentRow, int colIndex, string fieldName)
    {
        TableCell currentCell = currentRow.Cells[colIndex];

        Object currentValue = gvTaxMaster.DataKeys[currentRow.RowIndex].Values[fieldName];

        for (int nextRowIndex = currentRow.RowIndex + 1; nextRowIndex < gvTaxMaster.Rows.Count; nextRowIndex++)
        {
            Object nextValue = gvTaxMaster.DataKeys[nextRowIndex].Values[fieldName];

            if (nextValue.ToString() == currentValue.ToString())
            {
                GridViewRow nextRow = gvTaxMaster.Rows[nextRowIndex];
                TableCell nextCell = nextRow.Cells[colIndex];
                currentCell.RowSpan = Math.Max(1, currentCell.RowSpan) + 1;
                nextCell.Visible = false;
            }
            else
            {
                break;
            }
        }
    }

    public void UpdateTaxId()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var UpdTax = new taxmaster()
                {
                    BoatHouseId = ddlBoatHouseId.SelectedValue
                };
                HttpResponseMessage response = client.PostAsJsonAsync("TaxMaster/UpdateTaxId",UpdTax).Result;
              

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1) { BindTaxMaster(); }
                    else { }
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }


    /******************************Add**************************/

    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divnew.Visible = true;
        lbtnNew.Visible = false;
        divAddGrid.Visible = false;
        ddlBoatHouseId.SelectedIndex = 0;
        ddlBoatHouseId.Enabled = true;
        ddlServiceName.SelectedIndex = 0;
        ddlServiceName.Enabled = true;
        ddlCorpId.ClearSelection();
        ddlCorpId.Enabled = true;
        txtEffectiveFrom.Text = string.Empty;
        txtRefNum.Text = string.Empty;
        txtRefDate.Text = string.Empty;
        btnSubmit.Enabled = false;
    }

    //ADDING TAX DETAILS - SERVICENAME, TAX DESCRIPTION, TAX %
    protected void BtnAdd_Click(object sender, EventArgs e)
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
                string QType = string.Empty;
                string Unique = string.Empty;

                if (BtnAdd.Text == "Add")
                {
                    QType = "Add";
                    Unique = "0";

                }
                else
                {
                    QType = "AddUpdate";
                    Unique = hfUniqueId.Value.Trim();
                }

                var Taxmaster = new taxmaster()
                {
                    QueryType = QType.Trim(),
                    TaxId = "0",
                    UniqueId = Unique.Trim(),
                    BoatHouseId = ddlBoatHouseId.SelectedValue,
                    ServiceName = ddlServiceName.SelectedValue,
                    TaxDescription = txtTaxDesc.Text,
                    TaxPercentage = txtTaxPerc.Text,
                    CreatedBy = hfCreatedBy.Value.Trim()
                };

                response = client.PostAsJsonAsync("TaxMaster/Add", Taxmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                    if (StatusCode == 1)
                    {
                        divAddGrid.Visible = true;
                        clearAddinputs();
                        BindAddTaxMaster();
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

    protected void ImgBtnAddEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divAddGrid.Visible = true;
            lbtnNew.Visible = false;

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueId = gvAddTaxMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            hfUniqueId.Value = UniqueId.Trim();
            Label TaxDesc = (Label)gvrow.FindControl("lblTaxDesc");
            Label TaxPerc = (Label)gvrow.FindControl("lblTaxPerc");

            txtTaxDesc.Text = TaxDesc.Text.Trim();
            txtTaxPerc.Text = TaxPerc.Text.Trim();
            BtnAdd.Text = "Update";
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void ImgBtnAddDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divAddGrid.Visible = true;
            lbtnNew.Visible = false;

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string UniqueId = gvAddTaxMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            Label TaxDesc = (Label)gvrow.FindControl("lblTaxDesc");
            Label TaxPerc = (Label)gvrow.FindControl("lblTaxPerc");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                var Taxmaster = new taxmaster()
                {
                    QueryType = "AddDelete",
                    TaxId = "0",
                    UniqueId = UniqueId.Trim(),
                    BoatHouseId = ddlBoatHouseId.SelectedValue,
                    ServiceName = ddlServiceName.SelectedValue,
                    TaxDescription = TaxDesc.Text,
                    TaxPercentage = TaxPerc.Text,
                    CreatedBy = hfCreatedBy.Value.Trim()
                };
                response = client.PostAsJsonAsync("TaxMaster/Add", Taxmaster).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        divAddGrid.Visible = true;
                        clearAddinputs();
                        BindAddTaxMaster();
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

    /******************************Submit**************************/

    public void clear()
    {
        txtEffectiveFrom.Text = string.Empty;
        txtRefNum.Text = string.Empty;
        txtRefDate.Text = string.Empty;
        divAddGrid.Visible = false;
        clearAddinputs();
    }
    //FINAL INSERTION
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

                Label lblUniqueId;
                Label lblServiceId;
                Label lblTaxDesc;
                Label lblTaxPerc;



                for (int i = 0; i < gvAddTaxMaster.Rows.Count; i++)
                {
                    lblUniqueId = gvAddTaxMaster.Rows[i].FindControl("lblUniqueId") as Label;
                    lblServiceId = gvAddTaxMaster.Rows[i].FindControl("lblServiceId") as Label;
                    lblTaxDesc = gvAddTaxMaster.Rows[i].FindControl("lblTaxDesc") as Label;
                    lblTaxPerc = gvAddTaxMaster.Rows[i].FindControl("lblTaxPerc") as Label;

                    string strMappath = "~/Documents/";
                    string dirMapPath = Server.MapPath(strMappath);
                    if (!Directory.Exists(dirMapPath))
                    {
                        Directory.CreateDirectory(dirMapPath);
                    }
                    string subFolder = Path.Combine(dirMapPath, "TaxMaster");
                    if (!Directory.Exists(subFolder))
                    {
                        Directory.CreateDirectory(subFolder);
                    }
                    var fileName1 = Path.GetFileNameWithoutExtension(DocUpload.FileName) + Path.GetExtension(DocUpload.FileName);
                    var destinationPath1 = subFolder + "\\" + "" + lblUniqueId.Text + "" + Path.GetExtension(DocUpload.FileName);
                    DocUpload.SaveAs(destinationPath1);
                    bool dt = IsPDFHeader(destinationPath1);
                    if (dt == true)
                    {
                        MultipartFormDataContent content = new MultipartFormDataContent();

                        var values = new[]
                        {
                            new KeyValuePair<string, string>("QueryType", "Insert"),
                            new KeyValuePair<string, string>("TaxId", "0"),
                            new KeyValuePair<string, string>("UniqueId", lblUniqueId.Text),
                            new KeyValuePair<string, string>("ServiceName", lblServiceId.Text),
                            new KeyValuePair<string, string>("TaxDescription", lblTaxDesc.Text),
                            new KeyValuePair<string, string>("EffectiveFrom", txtEffectiveFrom.Text),
                            new KeyValuePair<string, string>("RefNum", txtRefNum.Text),
                            new KeyValuePair<string, string>("RefDate", txtRefDate.Text),
                            new KeyValuePair<string, string>("CreatedBy", hfCreatedBy.Value.Trim()),
                            new KeyValuePair<string, string>("BoatHouseId", ddlBoatHouseId.SelectedValue.Trim())
                        };

                        foreach (var keyValuePair in values)
                        {
                            content.Add(new StringContent(keyValuePair.Value),
                                String.Format("\"{0}\"", keyValuePair.Key));
                        }

                        var fileContent1 = new ByteArrayContent(System.IO.File.ReadAllBytes(destinationPath1));
                        fileContent1.Headers.ContentDisposition =
                                new ContentDispositionHeaderValue("form-data")
                                {
                                    Name = "RefDocLink",
                                    FileName = DocUpload.FileName,
                                };
                        content.Add(fileContent1);

                        response = client.PostAsync("TaxMaster/Insert", content).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                            string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                            if (StatusCode == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Enter Tax % equally');", true);
                        }
                        
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Your File Is Corrupted,Please Upload another File');", true);
                      
                    }

                }
                UpdateTaxId();
                BindTaxMaster();

            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public bool IsPDFHeader(string fileName)
    {

        byte[] buffer = null;
        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);

        long numBytes = new FileInfo(fileName).Length;
        //buffer = br.ReadBytes((int)numBytes);
        buffer = br.ReadBytes(5);

        var enc = new ASCIIEncoding();
        var header = enc.GetString(buffer);

        //%PDF−1.0
        // If you are loading it into a long, this is (0x04034b50).
        if (buffer[0] == 0x25 && buffer[1] == 0x50
            && buffer[2] == 0x44 && buffer[3] == 0x46)
        {
            return header.StartsWith("%PDF-");
        }
        return false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BindTaxMaster();
        DeleteTemp();
    }

    //DOWNLOAD REFERENCE DOCUMENT 
    protected void imgbtnDownload_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = (sender as ImageButton).CommandArgument;
        Stream stream = null;
        int bytesToRead = 10000;
        byte[] buffer = new Byte[bytesToRead];
        try
        {
            bool exist = false;
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(filePath);
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    exist = response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                exist = false;
            }
            if (exist == true)
            {
                HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(filePath);
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                if (fileReq.ContentLength > 0)
                    fileResp.ContentLength = fileReq.ContentLength;
                stream = fileResp.GetResponseStream();
                var resp = HttpContext.Current.Response;
                resp.ContentType = "application/octet-stream";
                resp.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());
                int length;
                do
                {
                    if (resp.IsClientConnected)
                    {
                        length = stream.Read(buffer, 0, bytesToRead);
                        resp.OutputStream.Write(buffer, 0, length);
                        resp.Flush();
                        buffer = new Byte[bytesToRead];
                    }
                    else
                    {
                        length = -1;
                    }
                }
                while (length > 0);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('File Not Found');", true);

            }
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }

    public class taxmaster
    {
        public string UniqueId { get; set; }
        public string TaxId { get; set; }
        public string TaxName { get; set; }
        public string ServiceName { get; set; }
        public string TaxDescription { get; set; }
        public string TaxPercentage { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTill { get; set; }
        public string RefNum { get; set; }
        public string RefDate { get; set; }
        public string RefDocLink { get; set; }
        public string BoatHouseId { get; set; }
        public string CreatedBy { get; set; }
        public string QueryType { get; set; }
    }
    public class FA_CommonMethod
    {
        public string QueryType { get; set; }
        public string ServiceType { get; set; }
        public string CorpId { get; set; }
        public string BranchCode { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Input4 { get; set; }
        public string Input5 { get; set; }
    }
}