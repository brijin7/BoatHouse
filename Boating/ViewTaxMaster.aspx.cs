using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Boating_ViewTaxMaster : System.Web.UI.Page
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
                BindTaxMaster();
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
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
            catch (Exception )
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
                    BoatHouseId = Session["BoatHouseId"].ToString()
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
                            divGrid.Visible = true;
                        }
                        else
                        {
                            gvTaxMaster.DataBind();
                            divGrid.Visible = false;
                        }
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
}