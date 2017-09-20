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

public partial class Account : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);
                object UserGUID = User.ProviderUserKey;

                dataSourceActors.SelectParameters.Add("accountID", UserGUID.ToString());            

                Database.Interface.open();
                Database.Account account = new Database.Account();

                if (!account.select_all_where_ID((Guid)UserGUID))
                {
                    Utils.reportStatus(ref labelStatus, Color.Red, Lang.Account_for_user + " '" 
                        + HttpContext.Current.User.Identity.Name + "' " + Lang.not_found);
                    return;
                }

                tbUserNameE.Text = HttpContext.Current.User.Identity.Name;
                tbNameE.Text = account.Name;
                hiddenName.Value = tbNameE.Text;
                tbContactE.Text = account.Contact;
                tbAddressE.Text = account.Address;
                //tbPostboxE.Text = account.Postbox;
                tbPostalE.Text = account.Postal;
                tbEmailE.Text = account.Email;
                tbPhoneE.Text = account.Phone;
                tbMobileE.Text = account.Mobile;
                tbFaxE.Text = account.Fax;
                tbWebsiteE.Text = account.Website;                
            }
            catch (Exception ex)
            {
                Utils.reportStatus(ref labelStatus, Color.Red, "Account.Page_Load: " + ex.Message);
            }
            finally
            {
                Database.Interface.close();
            }
        }        
    }

    protected void clearAllUpdateFields()
    {
        tbUserNameE.Text = "";
        tbOldPassword.Text = "";
        tbNewPassword.Text = "";
        tbNewPassword2.Text = "";
        tbNameE.Text = "";
        tbContactE.Text = "";
        tbAddressE.Text = "";
        //tbPostboxE.Text = "";
        tbPostalE.Text = "";
        tbEmailE.Text = "";
        tbPhoneE.Text = "";
        tbMobileE.Text = "";
        tbFaxE.Text = "";
        tbWebsiteE.Text = "";        
    }

    protected void buttonAccountE_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(tbNameE.Text) 
            || String.IsNullOrEmpty(tbAddressE.Text) 
            || String.IsNullOrEmpty(tbPostalE.Text) 
            || String.IsNullOrEmpty(tbEmailE.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Account_update_failed + ". " + Lang.MissingInformation);
            return;
        }

        if (!Utils.isValidEmail(tbEmailE.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Email_invalid_address);
            return;
        }

        try
        {
            MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);
            object UserGUID = User.ProviderUserKey;

            Database.Interface.open();
            
            if (tbNameE.Text.ToLower() != hiddenName.Value.ToLower())
            {
                if (Database.Account.accountNameExists(tbNameE.Text))
                {
                    Utils.displayStatus(ref labelStatus, Color.Red, Lang.The_name + " '" + tbNameE.Text + "' " + Lang.already_exists);
                    return;
                }
            }
            
            Database.Account account = new Database.Account();

            if (!account.select_all_where_ID((Guid)UserGUID))
            {
                Utils.reportStatus(ref labelStatus, Color.Red, 
                    "Account.buttonAccountE_OnClick: " + Lang.Account + " '" + HttpContext.Current.User.Identity.Name + "' " + Lang.not_found);
                return;
            }

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

            if (account.update_all_by_ID())
            {
                hiddenName.Value = tbNameE.Text;
                Utils.displayStatus(ref labelStatus, Color.SeaGreen, Lang.Account_update + " " + Lang.success);
            }
            else
                Utils.reportStatus(ref labelStatus, Color.Red, "Account.buttonAccountE_OnClick: " + Lang.Account_update + " '" + HttpContext.Current.User.Identity.Name + "' " + Lang.failed);
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, "Account.buttonAccountE_OnClick: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void buttonChangePassword_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(tbOldPassword.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Old_password_cant_be_empty);
            return;
        }

        if (String.IsNullOrEmpty(tbNewPassword.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.New_password_cant_be_empty);
            return;
        }

        if (tbNewPassword.Text.Length < Membership.MinRequiredPasswordLength)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Min_letters_in_password_is + " " + Membership.MinRequiredPasswordLength.ToString());
            return;
        }

        if (tbNewPassword.Text != tbNewPassword2.Text)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Passwords_dont_match);
            return;
        }

        MembershipUser u = Membership.GetUser(tbUserNameE.Text, false);
        if (u == null)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, "Account.buttonChangePassword_OnClick: User " + tbUserNameE.Text + " does not exist");            
            return;
        }

        try
        {
            if(u.ChangePassword(tbOldPassword.Text, tbNewPassword.Text))
                Utils.displayStatus(ref labelStatus, Color.SeaGreen, Lang.Password_changed);
            else
                Utils.reportStatus(ref labelStatus, Color.Red, Lang.Password_change_failed);
        }
        catch (Exception ex1)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Password_change_failed);
        }        
    }

    protected void ddActors_OnDataBound(object sender, EventArgs e)
    {
        ddActors.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));
    }

    protected void buttonCreateActor_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(textBoxName.Text) || String.IsNullOrEmpty(textBoxEmail.Text))
        {
            Utils.displayStatus(ref labelStatusActors, Color.Red, Resources.Localization.MissingInformation);
            return;
        }

        if (!Utils.isValidEmail(textBoxEmail.Text))
        {
            Utils.displayStatus(ref labelStatusActors, Color.Red, Lang.Email_invalid_address);
            return;
        }

        try
        {
            MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);
            object UserGUID = User.ProviderUserKey;

            Database.Interface.open();

            Database.Contact contact = new Database.Contact((Guid)UserGUID, textBoxName.Text, textBoxEmail.Text, textBoxPhone.Text, textBoxMobile.Text, ddStatus.SelectedValue);
            if (!contact.insert_with_ID(Guid.NewGuid()))
            {
                Utils.reportStatus(ref labelStatusActors, Color.Red, "Actors.buttonCreateActor_OnClick: Create contact failed");
                return;
            }

            Utils.displayStatus(ref labelStatusActors, Color.SeaGreen, Lang.User + " '" + textBoxName.Text + "' " + Lang.created);

            textBoxName.Text = "";
            textBoxEmail.Text = "";
            textBoxPhone.Text = "";
            textBoxMobile.Text = "";
            ddStatus.SelectedIndex = -1;

            textBoxNameUpd.Text = "";
            textBoxEmailUpd.Text = "";
            textBoxPhoneUpd.Text = "";
            textBoxMobileUpd.Text = "";

            ddActors.DataBind();
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatusActors, Color.Red, "Actors.buttonCreateActor_OnClick: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void ddActors_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddActors.SelectedValue == Guid.Empty.ToString())
        {
            textBoxNameUpd.Text = "";
            textBoxEmailUpd.Text = "";
            textBoxPhoneUpd.Text = "";
            textBoxMobileUpd.Text = "";
            ddStatus.SelectedIndex = -1;
            return;
        }

        try
        {
            Database.Interface.open();
            Database.Contact contact = new Database.Contact();
            if (!contact.select_all_by_ID(new Guid(ddActors.SelectedValue)))
            {
                Utils.reportStatus(ref labelStatusActors, Color.Red, "Actors.ddActors_OnSelectedIndexChanged: Contact not found");
                return;
            }

            textBoxNameUpd.Text = contact.Name;
            textBoxEmailUpd.Text = contact.Email;
            textBoxPhoneUpd.Text = contact.Phone;
            textBoxMobileUpd.Text = contact.Mobile;
            ddStatus.SelectedValue = contact.Status;
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatusActors, Color.Red, "Actors.ddActors_OnSelectedIndexChanged: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void buttonUpdateActor_OnClick(object sender, EventArgs e)
    {
        if (ddActors.SelectedValue == Guid.Empty.ToString())
            return;

        if (String.IsNullOrEmpty(textBoxNameUpd.Text) || String.IsNullOrEmpty(textBoxEmailUpd.Text))
        {
            Utils.displayStatus(ref labelStatusActors, Color.Red, Resources.Localization.MissingInformation);
            return;
        }

        if (!Utils.isValidEmail(textBoxEmailUpd.Text))
        {
            Utils.displayStatus(ref labelStatusActors, Color.Red, Lang.Email_invalid_address);
            return;
        }

        try
        {
            Database.Interface.open();

            Database.Contact contact = new Database.Contact();
            if (!contact.select_all_by_ID(new Guid(ddActors.SelectedValue)))
            {
                Utils.reportStatus(ref labelStatusActors, Color.Red, "Actors.buttonUpdateActor_OnClick: Contact not found");
                return;
            }

            if (ddStatus.SelectedValue != "Active")
            {
                Database.Interface.open();
                                
                    Database.Interface.command.Parameters.Clear();
                    Database.Interface.command.CommandText = @"
SELECT COUNT(Contact.ID) 
FROM Contact, Course, Contact_Course 
WHERE Course.bitCompleted = 0     
    AND Contact.ID = Contact_Course.ContactID 
    AND Course.ID = Contact_Course.CourseID AND Contact.ID = @contactID";
                    Database.Interface.command.CommandType = CommandType.Text;
                    Database.Interface.command.Parameters.AddWithValue("@contactID", contact.ID);
                    int count = (int)Database.Interface.command.ExecuteScalar();
                    if (count > 0)
                    {
                        Utils.displayStatus(ref labelStatusActors, Color.Red, "Kan ikke deaktivere. Den interne brukeren er oppmeldt til en eller flere kurs");
                        return;
                    }
                    Database.Interface.command.Parameters.Clear();
                
                    Database.Interface.command.CommandText = @"
SELECT COUNT(RingtestReport.ID) 
FROM RingtestReport
WHERE RingtestReport.bitApproved = 0 
    AND RingtestReport.ContactID = @contactID";
                    Database.Interface.command.CommandType = CommandType.Text;
                    Database.Interface.command.Parameters.AddWithValue("@contactID", contact.ID);
                    count = (int)Database.Interface.command.ExecuteScalar();
                    if (count > 0)
                    {
                        Utils.displayStatus(ref labelStatusActors, Color.Red, "Kan ikke deaktivere. Den interne brukeren har en eller flere ringtest rapporter");
                        return;
                    }
                    Database.Interface.command.Parameters.Clear();                
            }

            contact.Name = textBoxNameUpd.Text;
            contact.Email = textBoxEmailUpd.Text;
            contact.Phone = String.IsNullOrEmpty(textBoxPhoneUpd.Text) ? "" : textBoxPhoneUpd.Text;
            contact.Mobile = String.IsNullOrEmpty(textBoxMobileUpd.Text) ? "" : textBoxMobileUpd.Text;
            contact.Status = ddStatus.SelectedValue;

            if (!contact.update_all_by_ID())
            {
                Utils.reportStatus(ref labelStatusActors, Color.Red, "Actors.buttonUpdateActor_OnClick: Update contact failed");
                return;
            }

            Utils.displayStatus(ref labelStatusActors, Color.SeaGreen, Lang.Contact + " '" + textBoxNameUpd.Text + "' " + Lang.updated);

            textBoxNameUpd.Text = "";
            textBoxEmailUpd.Text = "";
            textBoxPhoneUpd.Text = "";
            textBoxMobileUpd.Text = "";
            ddStatus.SelectedIndex = -1;

            ddActors.DataBind();
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusActors, Color.Red, "Actors.buttonUpdateActor_OnClick: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }
}
