using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class site : System.Web.UI.MasterPage
{        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            menuNavigation.Items[0].Text = "Logg ut";
            menuNavigation.Items[0].NavigateUrl = "~/Logout.aspx";
            string currentRole = null;
            if (HttpContext.Current.User.IsInRole("Administrator"))
                currentRole = "Administrator";
            else if (HttpContext.Current.User.IsInRole("Moderator"))
                currentRole = "Moderator";
            else currentRole = "Reader";

            labelUserprofile.Text = "Bruker: " + HttpContext.Current.User.Identity.Name + ", Rolle: " + currentRole;
        }
        else
        {
            menuNavigation.Items[0].Text = "Logg inn";
            menuNavigation.Items[0].NavigateUrl = "~/Login.aspx";                        
            return;
        }                

        // Get page name from relative path
        string thisPage = Page.AppRelativeVirtualPath;
        int slashPos = thisPage.LastIndexOf("/"); 
        string pageName = "~" + thisPage.Substring(slashPos, thisPage.Length - slashPos);        

        // Select menu item with matching NavigateUrl property                        
        foreach (MenuItem parentMenu in menuNavigation.Items)
        {
            if (parentMenu.NavigateUrl.ToUpper() == pageName.ToUpper())
                parentMenu.Selected = true;        
        }
    }    
}
