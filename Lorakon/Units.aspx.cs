using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Units : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);
            object UserGUID = User.ProviderUserKey;
            dataSourceDevices.SelectParameters.Add("accountID", UserGUID.ToString());
        }
    }    
}
