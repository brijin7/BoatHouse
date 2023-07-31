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

public partial class _Default : System.Web.UI.Page
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
				Session["UserId"] = Request.QueryString["qUserId"].ToString().Trim();
				//Session["EmpId"] = Request.QueryString["qUserId"].ToString().Trim();
				Session["BoatHouseId"] = Request.QueryString["qBranchId"].ToString().Trim();
				Session["BoatHouseName"] = Request.QueryString["qBranchName"].ToString().Trim();
				Session["UserType"] = Request.QueryString["qUserType"].ToString().Trim();
				Session["UserRole"] = Request.QueryString["qUserRole"].ToString().Trim();

				Session["FirstName"] = "";
				Session["ShowPage"] = "0";
				Session["SupportUser"] = "";
				Session["TripUser"] = "A";

				Session["BaseUrl"] = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].Trim();
				Session["BaseQRUrl"] = System.Configuration.ConfigurationManager.AppSettings["BaseQRUrl"].Trim();
                Session["DomainUrl"] = System.Configuration.ConfigurationManager.AppSettings["DomainUrl"].Trim();
                Session["LogOutUrl"] = System.Configuration.ConfigurationManager.AppSettings["LogOutUrl"].Trim();

                BindUserProfile();

				Response.Redirect("Boating/NewDashboard.aspx", false);
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    public void BindUserProfile()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var UserProfile = new UserProfile()
                {
                    QueryType = "Department",
                    UserId = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("GetUserProfile/UserId", UserProfile).Result;

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
                            Session["FirstName"] = dt.Rows[0]["FirstName"].ToString().Trim();
                            Session["LastName"] = dt.Rows[0]["LastName"].ToString().Trim();

                            Session["SupportUser"] = dt.Rows[0]["SupportUser"].ToString();

                            if (Session["SupportUser"].ToString().Trim() == "F")
                            {
                                Session["ShowPage"] = "1";
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public class UserProfile
    {
        public string QueryType { get; set; }
        public string UserId { get; set; }
    }
}