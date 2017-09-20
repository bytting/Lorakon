using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;

public partial class Accounts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");            

        if (!Page.IsPostBack)
        {            
            buttonNewPassword.Attributes.Add("onclick", "javascript: return confirm('Lag nytt bruker passord, er du sikker?')");
            buttonAccountUnregisterE.Attributes.Add("onclick", "javascript: return confirm('Er du sikker på at du vil avmelde og slette årets rapporter for denne kontoen?')");

            if(!HttpContext.Current.User.IsInRole("Administrator"))
            {
                tabCreate.Enabled = false;                
            }

            if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Moderator"))
            {
                tabEdit.Enabled = false;                
            }            
        }        

        buttonDeletePendingAccount.Enabled = (ddPendingUsers.SelectedValue == Guid.Empty.ToString() ? false : true);        
    }    

    protected void clearAllCreateFields()
    {
        tbUserName.Text = "";
        tbPassword.Text = "";
        tbConfirmPassword.Text = "";
        tbName.Text = "";
        tbContact.Text = "";
        tbAddress.Text = "";                        
        tbPostal.Text = "";        
        tbEmail.Text = "";
        tbPhone.Text = "";
        tbMobile.Text = "";
        tbFax.Text = "";
        tbWebsite.Text = "";
    }

    protected void clearAllUpdateFields()
    {
        tbUserNameE.Text = "";
        lblAccountID.Text = "";
        tbNewPassword.Text = "";
        tbNameE.Text = "";
        tbContactE.Text = "";
        tbAddressE.Text = "";                        
        tbPostalE.Text = "";        
        tbEmailE.Text = "";
        tbPhoneE.Text = "";
        tbMobileE.Text = "";
        tbFaxE.Text = "";
        tbWebsiteE.Text = "";        
        cbActiveE.Checked = false;
        tbCommentE.Text = "";
        tbLastRegistrationE.Text = "";
        tbRingtestCountE.Text = "";        
    }    

    protected void buttonCreateAccount_OnClick(object sender, EventArgs e)
    {                
        bool accountCreated = false;
        Database.Account account = null;

        if (String.IsNullOrEmpty(tbUserName.Text)
            || String.IsNullOrEmpty(tbPassword.Text)
            || String.IsNullOrEmpty(tbEmail.Text)
            || String.IsNullOrEmpty(tbName.Text) 
            || String.IsNullOrEmpty(tbAddress.Text) 
            || String.IsNullOrEmpty(tbPostal.Text))
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Mangler informasjon");            
            return;
        }

        if (!Utils.isValidEmail(tbEmail.Text))
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Epost adresse har ugyldig format");
            return;
        }        

        if (tbPassword.Text.Length < Membership.MinRequiredPasswordLength)
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Passordet må ha minst " + Membership.MinRequiredPasswordLength.ToString() + " tegn");
            return;
        }

        if (tbPassword.Text != tbConfirmPassword.Text)
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Passordene er ikke like");
            return;
        }

        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServer"])
            || String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServerPort"]))
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Innstillinger for mailserver mangler");
            return;
        }

        Database.Configuration configuration = new Database.Configuration();

        try
        {            
            Membership.ApplicationName = "/Lorakon";            
            
            Database.Interface.open();
            if (!configuration.select_all_where_name("Default"))
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Finner ikke konfigurasjon");
                return;
            }             

            if (Database.Account.accountNameExists(tbName.Text))
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Navnet " + tbName.Text + " finnes allerede");
                Membership.ApplicationName = "/LorakonAdmin";
                return;            
            }

            MembershipCreateStatus status = new MembershipCreateStatus();
            MembershipUser user = Membership.CreateUser(tbUserName.Text, tbPassword.Text, tbEmail.Text, "question", "answer", true, out status);
            if (user == null)
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, Utils.getErrorMessage(status));
                Membership.ApplicationName = "/LorakonAdmin";
                return;
            }                            

            account = new Database.Account(
                Guid.Empty, tbName.Text, tbContact.Text, tbAddress.Text, "Field not active", 
                tbPostal.Text, tbEmail.Text, tbPhone.Text, tbMobile.Text, tbFax.Text, tbWebsite.Text, 
                true, "", 0, 0, "");
            
            accountCreated = account.insert_with_ID((Guid)user.ProviderUserKey);

            if (!String.IsNullOrEmpty(hiddenPendingUser.Value) && hiddenPendingUser.Value != Guid.Empty.ToString())
            {
                Database.PendingAccount pendingAccount = new Database.PendingAccount();
                if (pendingAccount.select_all_where_ID(new Guid(hiddenPendingUser.Value)))
                    pendingAccount.delete_by_ID();
            }

            ddUsers.DataBind();
            ddAccountsA.DataBind();
        }
        catch (Exception ex)
        {
            Membership.DeleteUser(tbUserName.Text);
            if (accountCreated)            
                account.delete_by_ID();                
            Utils.displayStatus(ref labelStatusCreate, Color.Red, ex.Message);
            return;
        }
        finally
        {
            Database.Interface.close();
            Membership.ApplicationName = "/LorakonAdmin";
        }

        string userName = tbUserName.Text;
        string receip = tbEmail.Text;
        string mailTitle = "Ny LORAKON konto tildelt";
        string mailBody = @"Velkommen som bruker av LORAKON nettjenester.<br>
Hver bedrift får kun tildelt ett brukernavn og passord. Dersom bedriften har flere ansatte som skal være delaktige i nettverket må brukernavnet og passordet deles mellom disse.<br>
Deres bedrift har fått tildelt følgende brukernavn: " + tbUserName.Text + " og passord: " + tbPassword.Text + @".<br>
For å logge inn på sidene kan følgende lenke benyttes: <a href='" + ConfigurationManager.AppSettings["LorakonURL"] + "'>" + ConfigurationManager.AppSettings["LorakonURL"] + @"</a><br>
NB! Brukernavnet er låst, men brukeren kan selv endre passordet ved behov via siden 'Bedriftens konto'<br><br>
Hilsen Statens Strålevern";

        clearAllCreateFields();
        ddPendingUsers.DataBind();
        ddUsers.DataBind();
        ddAccountsA.DataBind();

        if (ConfigurationManager.AppSettings["UseEmail"] == "yes")
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(receip);
                mail.From = new MailAddress(configuration.RingtestAdminEmail);
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.Default;
                mail.Subject = mailTitle;
                mail.Body = mailBody;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["MailServer"], Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);

                Utils.displayStatus(ref labelStatusCreate, Color.SeaGreen, "Bruker " + userName + " ble opprettet, og e-post er sendt til " + receip);
            }
            catch (Exception ex)
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, ex.Message);
            }
        }
    }

    protected void buttonDeletePendingAccount_OnClick(object sender, EventArgs e)
    {
        if (ddPendingUsers.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Ingen ventende brukere valgt");
            return;
        }

        try
        {
            string delAccount = ddPendingUsers.SelectedItem.Text;
            Database.Interface.open();
            Database.PendingAccount pendingAccount = new Database.PendingAccount();
            pendingAccount.ID = new Guid(ddPendingUsers.SelectedValue);
            pendingAccount.delete_by_ID();            
            ddPendingUsers.DataBind();

            clearAllCreateFields();

            Utils.displayStatus(ref labelStatusCreate, Color.SeaGreen, "Ventende konto '" + delAccount + "' slettet");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void ddPendingUsers_OnDataBound(object sender, EventArgs e)
    {
        ddPendingUsers.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));
    }

    protected void ddPendingUsers_OnSelectedIndexChanged(object sender, EventArgs e)
    {        
        if(ddPendingUsers.SelectedValue == Guid.Empty.ToString())
        {
            hiddenPendingUser.Value = Guid.Empty.ToString();
            clearAllCreateFields();
            return;
        }

        hiddenPendingUser.Value = ddPendingUsers.SelectedValue;

        try
        {
            Database.Interface.open();
            Database.PendingAccount pendingAccount = new Database.PendingAccount();
            pendingAccount.select_all_where_ID(new Guid(ddPendingUsers.SelectedValue));         

            tbName.Text = pendingAccount.Name;
            tbContact.Text = pendingAccount.Contact;
            tbAddress.Text = pendingAccount.Address;            
            tbPostal.Text = pendingAccount.Postal;
            tbEmail.Text = pendingAccount.Email;
            tbPhone.Text = pendingAccount.Phone;
            tbMobile.Text = pendingAccount.Mobile;
            tbFax.Text = pendingAccount.Fax;
            tbWebsite.Text = pendingAccount.Website;
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }        

    protected void ddUsers_OnDataBound(object sender, EventArgs e)
    {        
        ddUsers.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));
    }    

    protected void ddUsers_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddUsers.SelectedValue == Guid.Empty.ToString())
        {
            clearAllUpdateFields();            
            return;
        }

        try
        {
            Database.Interface.open();
            Database.Account account = new Database.Account();
            account.select_all_where_ID(new Guid(ddUsers.SelectedValue));         

            tbNameE.Text = account.Name;
            lblAccountID.Text = account.ID.ToString().ToUpper();
            tbContactE.Text = account.Contact;
            tbAddressE.Text = account.Address;            
            tbPostalE.Text = account.Postal;
            tbEmailE.Text = account.Email;
            tbPhoneE.Text = account.Phone;
            tbMobileE.Text = account.Mobile;
            tbFaxE.Text = account.Fax;
            tbWebsiteE.Text = account.Website;
            cbActiveE.Checked = account.Active;
            tbCommentE.Text = account.Comment;
            tbLastRegistrationE.Text = account.LastRegistrationYear.ToString();
            tbRingtestCountE.Text = account.RingtestCount.ToString();

            Membership.ApplicationName = "/Lorakon";
            MembershipUser user = Membership.GetUser(account.ID);
            tbUserNameE.Text = user.UserName;
            hiddenName.Value = tbNameE.Text;
            Membership.ApplicationName = "/LorakonAdmin";            
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void buttonAccountE_OnClick(object sender, EventArgs e)
    {
        if (ddUsers.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Du må velge en konto først");
            return;
        }

        if (String.IsNullOrEmpty(tbUserNameE.Text)            
            || String.IsNullOrEmpty(tbEmailE.Text)
            || String.IsNullOrEmpty(tbNameE.Text)
            || String.IsNullOrEmpty(tbAddressE.Text)
            || String.IsNullOrEmpty(tbPostalE.Text))
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Mangler informasjon");
            return;
        }

        try
        {
            Database.Interface.open();

            // Sjekk at konto ikke er oppmeldt til ringtest eller kurs hvis active er false
            if (!cbActiveE.Checked)
            {
                Database.Interface.command.Parameters.Clear();
                Database.Interface.command.CommandText = @"
SELECT COUNT(Contact.ID) 
FROM Contact, Course, Contact_Course 
WHERE Course.bitCompleted = 0 
    AND Contact.AccountID = @accountID 
    AND Contact.ID = Contact_Course.ContactID 
    AND Course.ID = Contact_Course.CourseID";
                Database.Interface.command.CommandType = CommandType.Text;
                Database.Interface.command.Parameters.AddWithValue("@accountID", new Guid(ddUsers.SelectedValue));
                int count = (int)Database.Interface.command.ExecuteScalar();
                if (count > 0)
                {
                    Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kan ikke sette konto til deaktivert. Kontoen er oppmeldt til en eller flere kurs");
                    return;
                }
                Database.Interface.command.Parameters.Clear();

                Database.Interface.command.CommandText = @"
SELECT COUNT(RingtestReport.ID) 
FROM RingtestReport
WHERE RingtestReport.bitApproved = 0 
    AND RingtestReport.AccountID = @accountID";
                Database.Interface.command.CommandType = CommandType.Text;
                Database.Interface.command.Parameters.AddWithValue("@accountID", new Guid(ddUsers.SelectedValue));
                count = (int)Database.Interface.command.ExecuteScalar();
                if (count > 0)
                {
                    Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kan ikke sette konto til deaktivert. Kontoen har en eller flere uferdige ringtest rapporter");
                    return;
                }
                Database.Interface.command.Parameters.Clear();
            }

            if (tbNameE.Text.ToLower() != hiddenName.Value.ToLower())
            {
                if (Database.Account.accountNameExists(tbNameE.Text))
                {
                    Utils.displayStatus(ref labelStatusEdit, Color.Red, "Navnet " + tbNameE.Text + " finnes allerede");
                    return;
                }
            }

            Database.Account account = new Database.Account();            

            account.select_all_where_ID(new Guid(ddUsers.SelectedValue));            
            account.Name = tbNameE.Text;
            account.Contact = tbContactE.Text;
            account.Address = tbAddressE.Text;
            account.Postbox = "Field not active";
            account.Postal = tbPostalE.Text;
            account.Email = tbEmailE.Text;
            account.Phone = tbPhoneE.Text;
            account.Mobile = tbMobileE.Text;
            account.Fax = tbFaxE.Text;
            account.Website = tbWebsiteE.Text;
            account.Active = cbActiveE.Checked;
            account.Comment = tbCommentE.Text;
            //account.LastRegistrationYear = Convert.ToInt32(tbLastRegistrationE.Text);
            //account.RingtestCount = Convert.ToInt32(tbRingtestCountE.Text);            

            if (account.update_all_by_ID())
            {
                Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Konto '" + account.Name + "' oppdatert");
                hiddenName.Value = tbNameE.Text;
            }
            else
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Oppdatering av konto feilet");         
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void buttonAccountRegisterE_OnClick(object sender, EventArgs e)
    {
        if (ddUsers.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Du må velge en konto først");
            return;
        }        

        try
        {
            Database.Interface.open();

            Database.Ringtest ringtest = new Database.Ringtest();
            if (ringtest.select_all_where_year(DateTime.Now.Year))
            {
                if (ringtest.Finished)
                {
                    Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ringtesten for " + DateTime.Now.Year.ToString() + " er avsluttet");
                    return;
                }
            }

            Database.Account account = new Database.Account();            

            account.select_all_where_ID(new Guid(ddUsers.SelectedValue));

            if (account.update_Int_by_ID("intLastRegistrationYear", DateTime.Now.Year))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Konto '" + account.Name + "' etter-registrert");
                tbLastRegistrationE.Text = DateTime.Now.Year.ToString();
            }
            else
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Etter-registrering av konto feilet");
            }
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void buttonAccountUnregisterE_OnClick(object sender, EventArgs e)
    {
        if (ddUsers.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Du må velge en konto først");
            return;
        }

        try
        {
            Database.Interface.open();
            Database.Account account = new Database.Account();

            account.select_all_where_ID(new Guid(ddUsers.SelectedValue));                                    

            Database.Ringtest ringtest = new Database.Ringtest();
            if (ringtest.select_all_where_year(DateTime.Now.Year))
            {
                Database.RingtestReport.delete_where_AccountID_and_Year(account.ID, ringtest.ID);                            
            }

            account.RingtestBoxID = Guid.Empty;            

            account.select_LastYear_from_RingtestReport();

            if (!account.update_all_by_ID())
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "buttonAccountUnregisterE_OnClick: Oppdatering av konto feilet");
                return;
            }

            tbLastRegistrationE.Text = account.LastRegistrationYear.ToString();

            Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Konto '" + account.Name + "' avregistrert fra årets ringtest");            
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }        
    }

    protected void ddAccountsA_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        gridDevicesA.DataBind();
    }

    protected void buttonNewPassword_OnClick(object sender, EventArgs e)
    {
        if (ddUsers.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Du må velge en bruker først");
            return;
        }

        if (String.IsNullOrEmpty(tbUserNameE.Text))
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Feltet for brukernavn kan ikke være tomt");
            return;
        }

        if (tbNewPassword.Text.Length < Membership.Provider.MinRequiredPasswordLength)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Passordet må ha minst " + Membership.Provider.MinRequiredPasswordLength.ToString() + " tegn");
            return;
        }

        Membership.ApplicationName = "/Lorakon";        
        MembershipUser u = Membership.GetUser(tbUserNameE.Text, false);

        if (u == null)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Bruker '" + tbUserNameE.Text + "' finnes ikke");
            Membership.ApplicationName = "/LorakonAdmin";
            return;
        }

        try
        {
            string newPassword = u.ResetPassword();

            if (newPassword != null)
            {
                if (!u.ChangePassword(newPassword, tbNewPassword.Text))
                {
                    Utils.displayStatus(ref labelStatusEdit, Color.Red, "Forandre passord feilet");                    
                    return;
                }
            }
            else
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Invalid password answer. Please re-enter and try again");                
                return;
            }
        }
        catch (MembershipPasswordException ex)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Invalid password answer. Please re-enter and try again");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, ex.Message);
        }
        finally
        {
            Membership.ApplicationName = "/LorakonAdmin";            
        }

        Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Nytt passord for bruker " + u.UserName + " er " + tbNewPassword.Text);
        tbNewPassword.Text = "";
    }        

    protected void ddAccountsA_OnDataBound(object sender, EventArgs e)
    {
        ddAccountsA.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));
    }    
}
