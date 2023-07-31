using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Boating_PrinterSetup : System.Web.UI.Page
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
				string FilePath = Server.MapPath("PrinterSetup.pdf");
				WebClient User = new WebClient();
				Byte[] FileBuffer = User.DownloadData(FilePath);
				if (FileBuffer != null)
				{
					Response.ContentType = "application/pdf";
					Response.AddHeader("content-length", FileBuffer.Length.ToString());
					Response.BinaryWrite(FileBuffer);
				}
				//CHANGES

			}
		}
		catch (HttpException)
		{
			Response.StatusCode = 403;
			Response.End();
		}
	}
}