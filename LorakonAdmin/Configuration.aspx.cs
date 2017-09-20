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

public partial class Configuration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");

        if (!Page.IsPostBack)
        {
            textBoxEdit.ImageBrowserURL = "../filemanager/browser/default/browser.html?Connector=../../connectors/aspx/connector.aspx";
            textBoxEdit.LinkBrowserURL = "../filemanager/browser/default/browser.html?Connector=../../connectors/aspx/connector.aspx";

            textBoxResources.ImageBrowserURL = "../filemanager/browser/default/browser.html?Connector=../../connectors/aspx/connector.aspx";
            textBoxResources.LinkBrowserURL = "../filemanager/browser/default/browser.html?Connector=../../connectors/aspx/connector.aspx";

            try
            {
                Database.Interface.open();

                Database.Configuration configuration = new Database.Configuration();
                if (configuration.select_all_where_name("Default"))
                {
                    textBoxEdit.Value = configuration.Start;
                    literalShowConfig.Text = configuration.Start;

                    textBoxResources.Value = configuration.News;
                    literalShowResources.Text = configuration.News;
                }
                else
                {
                    Utils.displayStatus(ref labelStatusEditConfig, Color.Red, "Konfigurasjonen 'Default' ikke funnet");
                    Utils.displayStatus(ref labelStatusShowConfig, Color.Red, "Konfigurasjonen 'Default' ikke funnet");
                }

                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Moderator"))
                {
                    tabEditConfig.Enabled = false;

                    tabEditResources.Enabled = false;                    
                }
            }
            catch (Exception ex)
            {
                Utils.displayStatus(ref labelStatusEditConfig, Color.Red, ex.Message);
                Utils.displayStatus(ref labelStatusShowConfig, Color.Red, ex.Message);
            }
            finally
            {
                Database.Interface.close();
            }
        }        
    }

    protected void buttonFrontpageSave_OnClick(object sender, EventArgs e)
    {            
        try
        {
            Database.Interface.open();

            Database.Configuration configuration = new Database.Configuration();
            if(configuration.select_all_where_name("Default"))
            {
                configuration.Start = textBoxEdit.Value;

                if (configuration.update_all_by_name())
                    Utils.displayStatus(ref labelStatusEditConfig, Color.SeaGreen, "Startside informasjon oppdatert");
                else
                    Utils.displayStatus(ref labelStatusEditConfig, Color.Red, "Oppdatering av startside informasjon feilet");

                literalShowConfig.Text = textBoxEdit.Value;
            }
            else
            {
                Utils.displayStatus(ref labelStatusEditConfig, Color.Red, "Konfigurasjonen 'Default' ikke funnet");
            }            
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusEditConfig, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }         
    }

    protected void buttonResourcesSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            Database.Interface.open();

            Database.Configuration configuration = new Database.Configuration();
            if (configuration.select_all_where_name("Default"))
            {
                configuration.News = textBoxResources.Value;

                if (configuration.update_all_by_name())
                    Utils.displayStatus(ref labelStatusEditResources, Color.SeaGreen, "Ressurser oppdatert");
                else
                    Utils.displayStatus(ref labelStatusEditResources, Color.Red, "Oppdatering av ressurser feilet");

                literalShowResources.Text = textBoxResources.Value;
            }
            else
            {
                Utils.displayStatus(ref labelStatusEditResources, Color.Red, "Konfigurasjonen 'Default' ikke funnet");
            }
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusEditConfig, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }
}
