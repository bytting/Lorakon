using System;
using System.Drawing;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.XPath;

public partial class Setup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string connStr = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder(connStr);            
            labelDatabase.Text = "Database: " + connBuilder.DataSource;
            labelCatalog.Text = "Catalog: " + connBuilder.InitialCatalog;
                    
            try
            {
                MembershipUser admin = Membership.GetUser("Administrator");
                if (admin == null)
                {
                    btnUpdatePassword.Enabled = false;
                    labelInfo.Text = "Eksisterende administrator bruker ble ikke funnet. Feltene nedenfor kan brukes til å opprette ny administrator bruker";
                }
                else
                {
                    btnCreatePassword.Enabled = false;
                    tbEmail.Text = admin.Email;
                    tbEmail.Enabled = false;
                    labelInfo.Text = "Eksisterende administrator bruker ble funnet. Feltene nedenfor kan brukes til å forandre administrator passordet";
                }                                
            }
            catch (Exception ex)
            {
                Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
            }
            finally
            {
                Database.Interface.close();
            }             
        }
    }

    protected void btnCreatePassword_OnClick(object sender, EventArgs e)
    {
        if (tbPassword1.Text.Length < Membership.MinRequiredPasswordLength)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Administrator passordet må ha minst " + Membership.MinRequiredPasswordLength.ToString() + " tegn");
            return;
        }

        if (tbPassword1.Text != tbPassword2.Text)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Passordene er ikke like");
            return;
        }

        if (!Utils.isValidEmail(tbEmail.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Epost adressen er ikke gyldig");
            return;
        }

        try
        {            
            MembershipCreateStatus status = new MembershipCreateStatus();
            MembershipUser admin = Membership.CreateUser("Administrator", tbPassword1.Text, tbEmail.Text, "Admin question?", "Admin answer", true, out status);
            if (admin == null)
            {
                Utils.displayStatus(ref labelStatus, Color.Red, "Kan ikke opprette administrator bruker");
                return;
            }

            ProfileCommon prof = (ProfileCommon)ProfileCommon.Create("Administrator", true);
            prof.Name = "Administrator";
            prof.Phone = "00000000";
            prof.Email = tbEmail.Text;
            prof.Title = "Administrator";
            prof.Save();

            // add roles if missing                   
            XPathDocument doc = new XPathDocument(Server.MapPath("App_Data/roles.xml"));
            XPathNavigator nav = doc.CreateNavigator();
            XPathExpression expr = nav.Compile("/roles/role");
            XPathNodeIterator iterator = nav.Select(expr);
            while (iterator.MoveNext())
            {
                if (!Roles.RoleExists(iterator.Current.Value))
                    Roles.CreateRole(iterator.Current.Value);

                if (!Roles.IsUserInRole("Administrator", iterator.Current.Value))
                    Roles.AddUserToRole("Administrator", iterator.Current.Value);
            }

            // add configuration if missing
            Database.Interface.open();
            Database.Configuration configuration = new Database.Configuration("Default", "", "", "", "");
            configuration.insert_with_ID_name(Guid.NewGuid(), "Default");

            Utils.displayStatus(ref labelStatus, Color.Green, "Administrator ble opprettet");
            btnCreatePassword.Enabled = false;
            tbPassword1.Enabled = false;
            tbPassword2.Enabled = false;
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }    
    }

    protected void btnUpdatePassword_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (tbPassword1.Text.Length < Membership.MinRequiredPasswordLength)
            {
                Utils.displayStatus(ref labelStatus, Color.Red, "Administrator passordet må ha minst " + Membership.MinRequiredPasswordLength.ToString() + " tegn");
                return;
            }

            if (tbPassword1.Text != tbPassword2.Text)
            {
                Utils.displayStatus(ref labelStatus, Color.Red, "Passordene er ikke like");
                return;
            }

            MembershipUser admin = Membership.GetUser("Administrator");            
            if (admin == null)
            {
                Utils.displayStatus(ref labelStatus, Color.Red, "Finner ikke administrator bruker");
                return;
            }

            string newPass = admin.ResetPassword("Admin answer");
            admin.ChangePassword(newPass, tbPassword1.Text);

            Utils.displayStatus(ref labelStatus, Color.Green, "Administrator passordet ble oppdatert");
            btnUpdatePassword.Enabled = false;
            tbPassword1.Enabled = false;
            tbPassword2.Enabled = false;
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
        }
    }
}
