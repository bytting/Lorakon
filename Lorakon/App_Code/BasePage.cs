using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Threading;
using System.Globalization;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : Page
{
    private const string m_DefaultCulture = "nb-NO";	    
        
    protected override void InitializeCulture()
    {        
        //retrieve culture information from session
        string culture = Convert.ToString(Session["CurrentCulture"]);

        //check whether a culture is stored in the session
        if (!string.IsNullOrEmpty(culture)) Culture = culture;
        else Culture = m_DefaultCulture;

        //set culture to current thread
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",";
        Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
        Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator = ",";
        Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";

        //call base class
        base.InitializeCulture();
    }

    protected override void OnLoad(EventArgs e)
    {
        string thisPage = Page.AppRelativeVirtualPath;
        int slashPos = thisPage.LastIndexOf("/");
        string pageName = "~" + thisPage.Substring(slashPos, thisPage.Length - slashPos);

        if (!HttpContext.Current.User.Identity.IsAuthenticated && pageName.ToLower() != "~/login.aspx" && pageName.ToLower() != "~/logout.aspx")
        {
            Response.Redirect("~/Login.aspx");
            return;
        }

        // Be sure to call the base class's OnLoad method!
        base.OnLoad(e);
    }
}
