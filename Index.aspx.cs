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
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    public string GetAntiForgeryToken()
    {
        string cookieToken, formToken;
        AntiForgery.GetTokens(null, out cookieToken, out formToken);
        ViewState["__AntiForgeryCookie"] = cookieToken;
        return formToken;

    }

    string BaseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].Trim();
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
                string UserId = string.Empty;
                string BoatHouseId = string.Empty;
                string BoatHouseName = string.Empty;
                string UserType = string.Empty;
                string UserRole = string.Empty;
                string Module = string.Empty;

                // Sadmin Role Login details

                //UserId = "1001";
                //BoatHouseId = "";
                //BoatHouseName = "";
                //UserType = "Department";
                //UserRole = "Sadmin";
                //Module = "";

                //UserId = "1004";
                //BoatHouseId = "18";
                //BoatHouseName = "BHMT";
                //UserType = "Department";
                //UserRole = "admin";
                //Module = "";

                //Admin Role Login

                //UserId = "1002";
                //BoatHouseId = "16";
                //BoatHouseName = "Ooty Boat House";
                //UserType = "Department";
                //UserRole = "Admin";
                //Module = "";

                //UserId = "1010";
                //BoatHouseId = "72";
                //BoatHouseName = "Test Boat House";
                //UserType = "Department";
                //UserRole = "Admin";
                //Module = "";

                //User Role Login

                //UserId = "1063";
                //BoatHouseId = "16";
                //BoatHouseName = "Ooty Boat House";
                //UserType = "Department";
                //UserRole = "User";
                //Module = "";

                //Response.Redirect("Default.aspx?qUserId=" + UserId.Trim() + "&qUserRole= " + UserRole.Trim() + "&qUserType="
                //    + UserType.Trim() + "&qBranchId=" + BoatHouseId.Trim() + "&qBranchName=" + BoatHouseName.Trim() + "&qModule=" + Module.Trim() + "");
                ////CHANGES

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

            if (txtUserName.Text != "" && txtPassword.Text != "")
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var UserRegistration = new UserLoginDtl()
                    {
                        QueryType = "Login",
                        UserName = txtUserName.Text.Trim(),
                        Password = txtPassword.Text.Trim()
                    };
                    HttpResponseMessage response = client.PostAsJsonAsync("CM_UserLogin", UserRegistration).Result;

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
                                Session["UserId"] = dt.Rows[0]["UserId"].ToString();
                                Session["EmpId"] = dt.Rows[0]["EmpId"].ToString();
                                Session["UserName"] = dt.Rows[0]["UserName"].ToString();
                                Session["FirstName"] = dt.Rows[0]["FirstName"].ToString();
                                Session["LastName"] = dt.Rows[0]["LastName"].ToString();
                                Session["EmailId"] = dt.Rows[0]["EmailId"].ToString();
                                Session["BranchId"] = dt.Rows[0]["BranchId"].ToString();
                                Session["BranchName"] = dt.Rows[0]["BranchName"].ToString();
                                Session["MobileNo"] = dt.Rows[0]["MobileNo"].ToString();
                                Session["MobAppAccess"] = dt.Rows[0]["MobAppAccess"].ToString();
                                Session["UserType"] = dt.Rows[0]["UserType"].ToString();
                                Session["UserRole"] = dt.Rows[0]["UserRole"].ToString();
                                Session["UserRoleId"] = dt.Rows[0]["UserRoleId"].ToString();
                                Session["CorpId"] = dt.Rows[0]["CorpId"].ToString();
                                Session["ShowPage"] = "0";
                                Session["CorpLogo"] = "";
                                Session["BaseUrl"] = BaseUrl.Trim();
                                Session["SupportUser"] = dt.Rows[0]["SupportUser"].ToString();

                                if (Session["UserType"].ToString() == "Department")
                                {
                                    if (dt.Rows[0]["OfflineRights"].ToString().Trim() == "Y")
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Sorry, User is Offline Mode !');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Response.Redirect("Default.aspx?qUserId=" + Session["UserId"].ToString().Trim() + "&qUserRole= " + Session["UserRole"].ToString() + "&qUserType="
            + Session["UserType"].ToString().Trim() + "&qBranchId=" + Session["BranchId"].ToString().Trim() + "&qBranchName=" + Session["BranchName"].ToString().Trim() + "&qModule=" + Session["MobileNo"].ToString().Trim() + "");
                                    }
                                }
                                else
                                {
                                    Response.Redirect("PLanding.aspx");
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('No User Details Found !');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ResponseMsg + "');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + response.StatusCode + "');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('Enter Username & Password !');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public class UserLoginDtl
    {
        public string QueryType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    protected void Page_Init(object sender, EventArgs e)
    {
        //First, check for the existence of the Anti-XSS cookie
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;

        //If the CSRF cookie is found, parse the token from the cookie.
        //Then, set the global page variable and view state user
        //key. The global variable will be used to validate that it matches 
        //in the view state form field in the Page.PreLoad method.
        if (requestCookie != null
            && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            //Set the global token variable so the cookie value can be
            //validated against the value in the view state form field in
            //the Page.PreLoad method.
            _antiXsrfTokenValue = requestCookie.Value;

            //Set the view state user key, which will be validated by the
            //framework during each request
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        //If the CSRF cookie is not found, then this is a new session.
        else
        {
            //Generate a new Anti-XSRF token
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

            //Set the view state user key, which will be validated by the
            //framework during each request
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            //Create the non-persistent CSRF cookie
            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                //Set the HttpOnly property to prevent the cookie from
                //being accessed by client side script
                HttpOnly = true,

                //Add the Anti-XSRF token to the cookie value
                Value = _antiXsrfTokenValue
            };

            //If we are using SSL, the cookie should be set to secure to
            //prevent it from being sent over HTTP connections
            if (FormsAuthentication.RequireSSL &&
                Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }

            //Add the CSRF cookie to the response
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        //During the initial page load, add the Anti-XSRF token and user
        //name to the ViewState
        if (!IsPostBack)
        {
            //Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

            //If a user name is assigned, set the user name
            ViewState[AntiXsrfUserNameKey] =
                   Context.User.Identity.Name ?? String.Empty;
        }
        //During all subsequent post backs to the page, the token value from
        //the cookie should be validated against the token in the view state
        //form field. Additionally user name should be compared to the
        //authenticated users name
        else
        {
            //Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] !=
                     (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of " +
                                    "Anti-XSRF token failed.");
            }
        }
    }
}