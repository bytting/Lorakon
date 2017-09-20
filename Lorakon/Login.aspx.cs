using System;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Lang = Resources.Localization;

public partial class Login : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {   
        if(Request.QueryString["ufc"] != null)
        {
            int ufc = 0;
            try
            {
                ufc = Convert.ToInt32(Request.QueryString["ufc"]);
                if(ufc > 0)
                {
                    Utils.reportStatus(ref labelStatus, Color.Red, "Opplasting av filer kunne ikke fullføres fordi innlogging er utløpt");
                }
            }
            catch { }
        }
    }

    protected void login_OnLoggingIn(object sender, LoginCancelEventArgs e)
    {
        Database.Account account = new Database.Account();

        try
        {
            MembershipUser User = Membership.GetUser(login.UserName);
            if (User == null)
            {                
                return;
            }

            object UserGUID = User.ProviderUserKey;

            Database.Interface.open();            
            if (!account.select_all_where_ID((Guid)UserGUID))
            {
                Utils.reportStatus(ref labelStatus, Color.Red, "Login:login_OnLoggingIn: " + Lang.Account_for_user + " '" + login.UserName + "' " + Lang.not_found);                
                return;
            }

            if (!account.Active)
            {
                Utils.displayStatus(ref labelStatus, Color.SeaGreen, Lang.Account_for_user + " '" + login.UserName + "' " + Lang.is_not_active);
                e.Cancel = true;
            }
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        } 
    }
}
