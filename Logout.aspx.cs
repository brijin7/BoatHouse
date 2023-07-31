using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
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
                try
                {
                    if (!string.IsNullOrEmpty(Session["UserId"] as string))
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            HttpResponseMessage response;
                            string sMSG = string.Empty;
                            var LoginLog = new LoginLog()
                            {
                                QueryType = "LogOut",
                                UserName = "",
                                SystemIP = "",
                                SessionId = "",
                                PublicIP = "",
                                Browser = "",
                                BVersion = "",
                                Log = "",
                                UserId = Session["UserId"].ToString().Trim()
                            };
                            response = client.PostAsJsonAsync("LoginLog", LoginLog).Result;


                            if (response.IsSuccessStatusCode)
                            {
                                var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                                int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["StatusCode"].ToString());
                                string ResponseMsg = JObject.Parse(vehicleEditresponse)["Response"].ToString();

                                if (StatusCode == 1)
                                {
                                    Response.AddHeader("Pragma", "no-cache");
                                    Response.CacheControl = "no-cache";
                                    Response.Cache.SetAllowResponseInBrowserHistory(false);
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    Response.Cache.SetNoStore();
                                    Response.Cache.SetExpires(DateTime.Now.AddSeconds(10));
                                    Response.Cache.SetValidUntilExpires(true);
                                    Response.Expires = -1;
                                    Session.Clear();
                                    Session.Abandon();
                                    Session.RemoveAll();
                                    if (Request.Cookies["ASP.NET_SessionId"] != null)
                                    {
                                        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                                        Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                                        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", string.Empty));
                                    }
                                    if (Request.Cookies["AuthToken"] != null)
                                    {
                                        Response.Cookies["AuthToken"].Value = string.Empty;
                                        Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                                        Response.Cookies.Add(new HttpCookie("AuthToken", ""));
                                    }
                                }
                                Response.Redirect(Session["LogOutUrl"].ToString().Trim());
                            }
                            else
                            {
                                //CHANGES
                                Response.AddHeader("Pragma", "no-cache");
                                Response.CacheControl = "no-cache";
                                Response.Cache.SetAllowResponseInBrowserHistory(false);
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Cache.SetNoStore();
                                Response.Cache.SetExpires(DateTime.Now.AddSeconds(10));
                                Response.Cache.SetValidUntilExpires(true);
                                Response.Expires = -1;
                                Session.Clear();
                                Session.Abandon();

                                Response.Redirect(Session["LogOutUrl"].ToString().Trim());

                            }
                        }
                    }
                    else
                    {
                        Session.Clear();
                        Session.Abandon();
                        Session.RemoveAll();
                        if (Request.Cookies["ASP.NET_SessionId"] != null)
                        {
                            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", string.Empty));
                        }
                        if (Request.Cookies["AuthToken"] != null)
                        {
                            Response.Cookies["AuthToken"].Value = string.Empty;
                            Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                            Response.Cookies.Add(new HttpCookie("AuthToken", ""));
                        }
                        Response.Redirect(Session["LogOutUrl"].ToString().Trim());
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
                }
            }
        }
        catch (HttpException)
        {
            Response.StatusCode = 403;
            Response.End();
        }
    }
    public class LoginLog
    {
        public string QueryType { get; set; }
        public string UserName { get; set; }
        public string SystemIP { get; set; }
        public string SessionId { get; set; }
        public string PublicIP { get; set; }
        public string Browser { get; set; }
        public string BVersion { get; set; }
        public string Log { get; set; }
        public string UserId { get; set; }
    }
}