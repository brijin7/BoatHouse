using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorPage : System.Web.UI.Page
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
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}

    protected void lnkHomePage_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogOutUrl"].Trim());
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }

    }
}