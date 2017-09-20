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
            menuNavigation.Items[0].Text = Resources.Localization.Logout;
            menuNavigation.Items[0].NavigateUrl = "~/Logout.aspx";

            try
            {
                string accName = (string)Session["AccountName"];
                if (String.IsNullOrEmpty(accName))
                {
                    Database.Interface.open();
                    Database.Account account = new Database.Account();
                    MembershipUser user = Membership.GetUser();
                    account.select_all_where_ID((Guid)user.ProviderUserKey);
                    Session["AccountName"] = account.Name;
                    accName = account.Name;
                }
                labelProfile.Text = Resources.Localization.Account + ": " + accName;
            }
            catch (Exception ex)
            {
                Database.Interface.close();
            }
        }
        else
        {
            menuNavigation.Items[0].Text = Resources.Localization.Login;
            menuNavigation.Items[0].NavigateUrl = "~/Login.aspx";

            menuNavigation.Visible = false;

            return;
        }

        // Get page name from relative path
        string thisPage = Page.AppRelativeVirtualPath;
        int slashPos = thisPage.LastIndexOf("/"); 
        string pageName = "~" + thisPage.Substring(slashPos, thisPage.Length - slashPos);        

        // Select menu item with matching NavigateUrl property                
        foreach(MenuItem parentMenu in menuNavigation.Items)        
            if(parentMenu.NavigateUrl.ToUpper() == pageName.ToUpper())        
                parentMenu.Selected = true;                
    }

    protected void ddLanguage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // Waiting code: #1

        /*
        if (ddLanguage.SelectedValue != "---")
        {
            Session["CurrentCulture"] = ddLanguage.SelectedValue;
            Server.Transfer(Request.Path);
        }
         * */
    }

    protected void ddLanguage_OnDataBound(object sender, EventArgs e)
    {
        // Waiting code: #1

        //ddLanguage.Items.Insert(0, new ListItem("<Select language>", "---"));
    }
}
