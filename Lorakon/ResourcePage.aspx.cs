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

public partial class ResourcePage : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                Database.Interface.open();
                Database.Configuration config = new Database.Configuration();
                if (config.select_all_where_name("Default"))                
                    labelResourcePage.Text = config.News;                
                else                
                    Utils.reportStatus(ref labelStatus, Color.Red, Lang.Configuration + " " + Lang.not_found);                
            }
            catch(Exception ex)
            {
                Utils.reportStatus(ref labelStatus, Color.Red, "ResourcePage.Page_Load: " + ex.Message);
            }
            finally
            {
                Database.Interface.close();
            }            
        }
    }
}
