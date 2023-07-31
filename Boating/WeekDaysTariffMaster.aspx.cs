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

public partial class Department_Boating_WeekDaysTariffMaster : System.Web.UI.Page
{
    public string GetAntiForgeryToken()
    {
        string cookieToken, formToken;
        AntiForgery.GetTokens(null, out cookieToken, out formToken);
        ViewState["__AntiForgeryCookie"] = cookieToken;
        return formToken;
    }
    DataTable dt = new DataTable();
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
                txtBoatId.Text = Session["BoatHouseId"].ToString().Trim();
                txthousename.Text = Session["BoatHouseName"].ToString().Trim();
                txthousename.Enabled = false;
                txtHouseManager.Text = Session["UserRole"].ToString().Trim();
                txtHouseManager.Enabled = false;
                chkWorkingDays.ClearSelection();
                chkWeekend.ClearSelection();
                chkWeekend.Enabled = false;
                if (Session["UserRole"].ToString().Trim() == "Admin" || Session["UserRole"].ToString().Trim().ToUpper() == "ADMIN"
               || Session["UserRole"].ToString().Trim() == "User" || Session["UserRole"].ToString().Trim().ToUpper() == "USER")
                {
                    gvBoatHouse.Columns[8].Visible = false;
                }

                ViewState["WDStatus"] = "WD";
                BindWeekDaysTariff();
                Clear();
                btnSubmit.Text = "Submit";
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public void Clear()
    {
        txtPubHldyDate.Text = string.Empty;
        txtPubHldyDescp.Text = string.Empty;

        divTempGrid.Visible = false;
        gvPHData.DataSource = null;
        gvPHData.DataBind();

        Session["Workingdays"] = "";
        Session["Weekend"] = "";
        ViewState["HolidayData"] = "";

        chkWorkingDays.Items[1].Selected = false;
        chkWorkingDays.Items[2].Selected = false;
        chkWorkingDays.Items[3].Selected = false;
        chkWorkingDays.Items[4].Selected = false;
        chkWorkingDays.Items[5].Selected = false;
        chkWorkingDays.Items[6].Selected = false;
        chkWorkingDays.Items[0].Selected = false;
        //chkSelectAll.Checked = false;
        chkWeekend.Items[1].Selected = false;
        chkWeekend.Items[2].Selected = false;
        chkWeekend.Items[3].Selected = false;
        chkWeekend.Items[4].Selected = false;
        chkWeekend.Items[5].Selected = false;
        chkWeekend.Items[6].Selected = false;
        chkWeekend.Items[0].Selected = false;

    }
    public void BindWeekDaysTariff()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var BoatHouseId = new boatHouse()
                {
                    BoatHouseId = Session["BoatHouseId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("GetWeekDayTariff", BoatHouseId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var BoatHouseresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(BoatHouseresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(BoatHouseresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtBndWdTarif = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ViewState["WeekdaysAvail"] = "0";

                        if (dtBndWdTarif.Rows.Count > 0)
                        {
                            foreach (DataRow dtRow in dtBndWdTarif.Rows)
                            {
                                // On all tables' columns
                                foreach (DataColumn dc in dtBndWdTarif.Columns)
                                {
                                    //var field1 = dtRow[dc].ToString();
                                    string sWeekDaysDesc = dtRow[dc].ToString().Trim();
                                    if (sWeekDaysDesc.ToString().Trim() == "WD")
                                    {
                                        ViewState["WeekdaysAvail"] = "1";

                                    }
                                    //else if (sWeekDaysDesc.ToString().Trim() == "WE")
                                    //{
                                    //    ViewState["WDStatus"] = "Inserted";
                                    //}
                                }
                            }

                            gvBoatHouse.DataSource = dtBndWdTarif;
                            gvBoatHouse.DataBind();
                            lblGridMsg.Text = string.Empty;
                            Clear();
                            divGrid.Visible = true;
                            //divEntry.Visible = false;
                            if (Session["UserRole"].ToString().Trim() == "Admin" || Session["UserRole"].ToString().Trim().ToUpper() == "ADMIN"
                                || Session["UserRole"].ToString().Trim() == "User" || Session["UserRole"].ToString().Trim().ToUpper() == "USER")
                            {
                                gvBoatHouse.Columns[8].Visible = false;

                            }
                            if (ViewState["WeekdaysAvail"].ToString().Trim() == "1" && ViewState["WeekdaysAvail"].ToString().Trim() != "0")
                            {

                                divWeekDays.Visible = false;
                                //divPublicholidays.Visible = true;                              

                            }
                            else if (ViewState["WDStatus"].ToString().Trim() == "Inserted")
                            {
                                divWeekDays.Visible = false;
                                //divPublicholidays.Visible = true;
                            }
                            else
                            {
                                divWeekDays.Visible = true;
                                //divPublicholidays.Visible = false;
                            }
                        }
                        else
                        {
                            divGrid.Visible = false;
                            gvBoatHouse.DataBind();
                            Clear();
                            lblGridMsg.Text = ResponseMsg.ToString().Trim();
                        }

                    }
                    else
                    {
                        Clear();
                        divGrid.Visible = false;
                        divWeekDays.Visible = true;
                        divPublicholidays.Visible = false;
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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();


            for (int i = 0; i < chkWorkingDays.Items.Count; i++)
            {
                if (chkWorkingDays.Items[i].Selected)
                {
                    string str = chkWorkingDays.Items[i].ToString();
                    string bindwd = string.Empty;
                    if (str == "Sunday")
                    {
                        bindwd = "Sunday,";
                    }
                    else if (str == "Monday")
                    {
                        bindwd = "Monday,";
                    }
                    else if (str == "Tuesday")
                    {
                        bindwd = "Tuesday,";
                    }
                    else if (str == "Wednesday")
                    {
                        bindwd = "Wednesday,";
                    }
                    else if (str == "Thursday")
                    {
                        bindwd = "Thursday,";
                    }
                    else if (str == "Friday")
                    {
                        bindwd = "Friday,";
                    }
                    else if (str == "Saturday")
                    {
                        bindwd = "Saturday";
                    }
                    Session["Workingdays"] = sb.Append(bindwd);
                }
            }

            if (Session["Workingdays"].ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Please Select Week Days.');", true);
                return;
            }



            for (int i = 0; i < chkWeekend.Items.Count; i++)
            {
                if (chkWeekend.Items[i].Selected)
                {
                    string str1 = chkWeekend.Items[i].ToString();
                    string bindwe = string.Empty;
                    if (str1 == "Sunday")
                    {
                        bindwe = "Sunday,";
                    }
                    else if (str1 == "Monday")
                    {
                        bindwe = "Monday,";
                    }
                    else if (str1 == "Tuesday")
                    {
                        bindwe = "Tuesday,";
                    }
                    else if (str1 == "Wednesday")
                    {
                        bindwe = "Wednesday,";
                    }
                    else if (str1 == "Thursday")
                    {
                        bindwe = "Thursday,";
                    }
                    else if (str1 == "Friday")
                    {
                        bindwe = "Friday,";
                    }
                    else if (str1 == "Saturday")
                    {
                        bindwe = "Saturday,";
                    }
                    Session["Weekend"] = sb1.Append(bindwe);
                }
            }

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

                    //if (Session["Workingdays"].ToString() != "" && Session["Weekend"].ToString() != "")
                    //{
                    string[] WD = Session["Workingdays"].ToString().TrimEnd().Split(',');
                    int WeekDaycount = WD.Count();

                    if (WeekDaycount > 1)
                    {
                        for (int i = 0; i < WD.Count(); i++)
                        {
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var BoatHouse = new boatHouse()
                            {
                                QueryType = "Insert",
                                BoatHouseId = txtBoatId.Text,
                                WeekDays = WD[i].ToString().Trim(),
                                WeekDaysDesc = "WD",
                                HolidayDate = "",
                                HolidayDesc = "",
                                CreatedBy = hfCreatedBy.Value.Trim()
                            };

                            response = client.PostAsJsonAsync("InsWeekDayTariff", BoatHouse).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                var DisposalResponse = response.Content.ReadAsStringAsync().Result;
                                int StatusCode1 = Convert.ToInt32(JObject.Parse(DisposalResponse)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(DisposalResponse)["Response"].ToString();

                                if (StatusCode1 == 1)
                                {
                                    ViewState["WDStatus"] = "Inserted";
                                }

                            }
                            else
                            {

                            }


                        }
                    }

                    string[] WE = Session["Weekend"].ToString().Split(',');
                    int weekendcount = WE.Count();

                    if (weekendcount > 1)
                    {
                        for (int i = 0; i < WE.Count(); i++)
                        {
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var BoatHouse = new boatHouse()
                            {
                                QueryType = "Insert",
                                BoatHouseId = txtBoatId.Text,
                                WeekDays = WE[i].ToString(),
                                WeekDaysDesc = "WE",
                                HolidayDate = "",
                                HolidayDesc = "",
                                CreatedBy = hfCreatedBy.Value.Trim()
                            };

                            response = client.PostAsJsonAsync("InsWeekDayTariff", BoatHouse).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                var DisposalResponse = response.Content.ReadAsStringAsync().Result;
                                int StatusCode1 = Convert.ToInt32(JObject.Parse(DisposalResponse)["StatusCode"].ToString());
                                string ResponseMsg1 = JObject.Parse(DisposalResponse)["Response"].ToString();

                                if (StatusCode1 == 1)
                                {
                                    ViewState["WDStatus"] = "Inserted";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg1.ToString().Trim() + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg1.ToString().Trim() + "');", true);
                                }
                            }
                            else
                            {

                            }

                        }
                    }
                    //}
                    using (var client1 = new HttpClient())
                    {
                        DataTable dtHolData = ViewState["HolidayData"] as DataTable;

                        if (ViewState["HolidayData"].ToString() != "")
                        {
                            for (int i = 0; i < dtHolData.Rows.Count; i++)
                            {

                                if (client.BaseAddress == null)
                                {
                                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                                }

                                client1.DefaultRequestHeaders.Clear();
                                client1.DefaultRequestHeaders.Accept.Clear();
                                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                var body1 = new boatHouse()
                                {
                                    QueryType = "InsertPHdate",
                                    BoatHouseId = txtBoatId.Text,
                                    WeekDays = dtHolData.Rows[i]["WeekDays"].ToString(),
                                    WeekDaysDesc = "WE",
                                    HolidayDate = dtHolData.Rows[i]["HolidayDate"].ToString(),
                                    HolidayDesc = dtHolData.Rows[i]["HolidayDesc"].ToString(),
                                    CreatedBy = hfCreatedBy.Value.Trim()
                                };

                                HttpResponseMessage response1 = client.PostAsJsonAsync("InsWeekDayTariff", body1).Result;

                                if (response1.IsSuccessStatusCode)
                                {
                                    var DisposalResponse = response1.Content.ReadAsStringAsync().Result;
                                    int StatusCode1 = Convert.ToInt32(JObject.Parse(DisposalResponse)["StatusCode"].ToString());
                                    string ResponseMsg1 = JObject.Parse(DisposalResponse)["Response"].ToString();

                                    if (StatusCode1 == 1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg1.ToString().Trim() + "');", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg1.ToString().Trim() + "');", true);
                                    }
                                }
                                else
                                {

                                }

                            }
                        }

                    }

                    BindWeekDaysTariff();
                    Clear();

                }
                else
                {
                    var BoatHouse = new boatHouse()
                    {
                        QueryType = "Update",
                        BoatHouseId = txtBoatId.Text,
                        WeekDays = Session["Workingdays"].ToString(),
                        WeekDaysDesc = "WD",
                        HolidayDate = "-",
                        HolidayDesc = "-",
                        CreatedBy = hfCreatedBy.Value.Trim()
                    };

                    response = client.PostAsJsonAsync("InsWeekDayTariff", BoatHouse).Result;


                    if (response.IsSuccessStatusCode)
                    {
                        var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                        string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();
                        if (StatusCode == 1)
                        {
                            BindWeekDaysTariff();
                            Clear();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            //divEntry.Visible = false;
                            //divGrid.Visible = true;

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            //divEntry.Visible = true;
                            //divGrid.Visible = false;
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
        Clear();
        //divEntry.Visible = false;
        //divGrid.Visible = true;
    }
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divCheckWorkingdays.Visible = true;

            //divGrid.Visible = false;
            //divEntry.Visible = true;
            btnSubmit.Text = "Update";

            Clear();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvBoatHouse.DataKeys[gvrow.RowIndex].Value.ToString();
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label BoatLocnId = (Label)gvrow.FindControl("lblBoatLocnId");
            Label BoatHouseMngId = (Label)gvrow.FindControl("lblBoatHouseMngId");
            Label WorkingDays = (Label)gvrow.FindControl("lblWorkingDays");

            txtBoatId.Text = BoatHouseId.Text.Trim();
            txthousename.Text = BoatHouseName.Text;
            txthousename.Enabled = false;


            string str = WorkingDays.Text;
            char[] Split = str.ToCharArray();

            for (int i = 0; i < Split.Count(); i++)
            {
                string split = Split[i].ToString();
                int a = Convert.ToInt16(split);

                if (a == 1)
                {
                    chkWorkingDays.Items[0].Selected = true;
                }
                else if (a == 2)
                {
                    chkWorkingDays.Items[1].Selected = true;
                }
                else if (a == 3)
                {
                    chkWorkingDays.Items[2].Selected = true;
                }
                else if (a == 4)
                {
                    chkWorkingDays.Items[3].Selected = true;
                }
                else if (a == 5)
                {
                    chkWorkingDays.Items[4].Selected = true;
                }
                else if (a == 6)
                {
                    chkWorkingDays.Items[5].Selected = true;
                }
                else if (a == 7)
                {
                    chkWorkingDays.Items[6].Selected = true;
                }
            }

            if (Convert.ToInt32(Split.Count()) > 6)
            {
                //chkSelectAll.Checked = true;
            }

        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sTesfg = gvBoatHouse.DataKeys[gvrow.RowIndex].Value.ToString();
            string WeekDayDescrp = string.Empty;
            Label BoatHouseId = (Label)gvrow.FindControl("lblBoatHouseId");
            Label BoatHouseName = (Label)gvrow.FindControl("lblBoatHouseName");
            Label WeekDays = (Label)gvrow.FindControl("lblWeekDays");
            Label WeekDayDescp = (Label)gvrow.FindControl("lblWdDescp");
            Label HolDate = (Label)gvrow.FindControl("lblHdate");
            if (HolDate.Text == "")
            {
                ViewState["WDStatus"] = "Deleted";
            }
            if (WeekDayDescp.Text == "Week Day Tariff")
            {
                WeekDayDescrp = "WD";
            }
            else
            {
                WeekDayDescrp = "WE";

            }
            Label PubHolDate = (Label)gvrow.FindControl("lblHdate");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var BoatHouseMaster1 = new boatHouse()
                {
                    QueryType = "Delete",
                    BoatHouseId = txtBoatId.Text,
                    WeekDays = WeekDays.Text,
                    WeekDaysDesc = WeekDayDescrp.ToString().Trim(),
                    HolidayDate = PubHolDate.Text,
                    HolidayDesc = "",
                    CreatedBy = hfCreatedBy.Value.Trim()

                };

                HttpResponseMessage response;

                response = client.PostAsJsonAsync("InsWeekDayTariff", BoatHouseMaster1).Result;

                if (response.IsSuccessStatusCode)
                {
                    var submitresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(submitresponse)["StatusCode"].ToString());
                    string ResponseMsg = JObject.Parse(submitresponse)["Response"].ToString();
                    if (StatusCode == 1)
                    {
                        BindWeekDaysTariff();
                        Clear();
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

    /// <summary>
    /// This is the checkbox selection index changed method 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkWorkingDays_SelectedIndexChanged(object sender, EventArgs e)
    {
        //    if (chkWorkingDays.Items[0].Selected == true)
        //    {
        //        chkWeekend.Items[0].Enabled = false;
        //        chkWeekend.Items[0].Selected = false;
        //    }
        //    else
        //    {
        //        chkWeekend.Items[0].Selected = true;
        //    }

        //    if (chkWorkingDays.Items[1].Selected == true)
        //    {
        //        chkWeekend.Items[1].Enabled = false;
        //        chkWeekend.Items[1].Selected = false;
        //    }
        //    else
        //    {
        //        chkWeekend.Items[1].Selected = true;
        //    }

        //    if (chkWorkingDays.Items[2].Selected == true)
        //    {
        //        chkWeekend.Items[2].Enabled = false;
        //        chkWeekend.Items[2].Selected = false;
        //    }
        //    else
        //    {
        //        chkWeekend.Items[2].Selected = true;
        //    }

        //    if (chkWorkingDays.Items[3].Selected == true)
        //    {
        //        chkWeekend.Items[3].Enabled = false;
        //        chkWeekend.Items[3].Selected = false;
        //    }
        //    else
        //    {
        //        chkWeekend.Items[3].Selected = true;
        //    }

        //    if (chkWorkingDays.Items[4].Selected == true)
        //    {
        //        chkWeekend.Items[4].Enabled = false;
        //        chkWeekend.Items[4].Selected = false;
        //    }
        //    else
        //    {
        //        chkWeekend.Items[4].Selected = true;
        //    }

        //    if (chkWorkingDays.Items[5].Selected == true)
        //    {
        //        chkWeekend.Items[5].Enabled = false;
        //        chkWeekend.Items[5].Selected = false;
        //    }
        //    else
        //    {
        //        chkWeekend.Items[5].Selected = true;
        //    }

        //    if (chkWorkingDays.Items[6].Selected == true)
        //    {
        //        chkWeekend.Items[6].Enabled = false;
        //        chkWeekend.Items[6].Selected = false;
        //    }
        //    else
        //    {
        //        chkWeekend.Items[6].Selected = true;
        //    }
    }

    /// <summary>
    /// Add new button click event to add data in Temporary Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnNew_Click(object sender, EventArgs e)
    {
        string Date = string.Empty;
        Date = txtPubHldyDate.Text;
        string[] Datesplit = Date.Split('/');
        DateTime dateValue = new DateTime(Convert.ToInt32(Datesplit[2]), Convert.ToInt32(Datesplit[1]), Convert.ToInt32(Datesplit[0]));
        string Day = string.Empty;
        Day = dateValue.ToString("dddddddd");

        DataTable dttmpgrd = new DataTable();

        dttmpgrd.Columns.Add("HolidayDate");
        dttmpgrd.Columns.Add("WeekDays");
        dttmpgrd.Columns.Add("HolidayDesc");


        for (int row = 0; row < gvPHData.Rows.Count; row++)
        {
            string value1 = gvPHData.Rows[row].Cells[0].Text;
            if (txtPubHldyDate.Text == value1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Public Holiday Date Already Exist');", true);
                return;
            }
            DataRow dr = dttmpgrd.NewRow();

            dr[0] = gvPHData.Rows[row].Cells[0].Text;
            dr[1] = gvPHData.Rows[row].Cells[1].Text;
            dr[2] = gvPHData.Rows[row].Cells[2].Text;
            dttmpgrd.Rows.Add(dr);
        }

        dttmpgrd.Rows.Add(txtPubHldyDate.Text.Trim(), Day.Trim(), txtPubHldyDescp.Text.Trim());

        gvPHData.DataSource = dttmpgrd;
        gvPHData.DataBind();
        gvPHData.Visible = true;

        ViewState["HolidayData"] = dttmpgrd;

        txtPubHldyDate.Text = string.Empty;
        txtPubHldyDescp.Text = string.Empty;

        if (dttmpgrd.Rows.Count > 0)
        {
            divTempGrid.Visible = true;
        }

    }

    /// <summary>
    /// Delete icon click event in Temporary Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImgDelete_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;
        DataTable dt = ViewState["HolidayData"] as DataTable;
        txtPubHldyDate.Text.Contains(gvPHData.DataKeys[row.RowIndex]["HolidayDate"].ToString().Trim());
        dt.Rows.RemoveAt(row.RowIndex);
        ViewState["HolidayData"] = dt;
        gvPHData.DataSource = dt;
        gvPHData.DataBind();
        if (dt.Rows.Count == 0)
        {
            divTempGrid.Visible = false;
        }
    }


    /// <summary>
    /// This method contains the class members 
    /// </summary>
    public class boatHouse
    {
        public string QueryType { get; set; }
        public string BoatHouseId { get; set; }
        public string WeekDays { get; set; }
        public string WeekDaysDesc { get; set; }
        public string HolidayDate { get; set; }
        public string HolidayDesc { get; set; }
        public string CreatedBy { get; set; }

    }

}