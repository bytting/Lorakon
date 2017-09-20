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

public partial class Admin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect("~/Login.aspx");            

            string[] roles = Roles.GetAllRoles();
            foreach (string s in roles)
            {
                TableRow row = new TableRow();
                tableRoles.Controls.Add(row);
                TableCell cell = new TableCell();
                row.Controls.Add(cell);
                CheckBox cb = new CheckBox();
                cb.Text = s;
                cb.CssClass = "TipText";
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                cell.Controls.Add(cb);
            }

            if (!Page.IsPostBack)
            {
                populateUserList();

                Database.Interface.open();
                Database.Configuration configuration = new Database.Configuration();
                configuration.select_all_where_name("Default");
                tbSectionManager.Text = configuration.SectionManager;
                tbRingtestAdminEmail.Text = configuration.RingtestAdminEmail;                
                Database.Interface.close();

                if (HttpContext.Current.User.IsInRole("Administrator"))
                {
                    tabUser.Enabled = false;                    
                }
                else
                {
                    tabAdmin.Enabled = false;                    

                    ProfileCommon prof = Profile.GetProfile(HttpContext.Current.User.Identity.Name);
                    tbEditNameUser.Text = prof.Name;
                    tbEditTitleUser.Text = prof.Title;
                    tbEditPhoneUser.Text = prof.Phone;
                    tbEditEmailUser.Text = prof.Email;
                }
            }
        }
        catch (Exception ex)
        {
            if (HttpContext.Current.User.IsInRole("Administrator"))
                Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
            else Utils.displayStatus(ref labelStatusUser, Color.Red, ex.Message);
        }        

        bool isAdministrator = HttpContext.Current.User.IsInRole("Administrator");
        buttonCreateUser.Enabled = isAdministrator;
        buttonChangePassword.Enabled = isAdministrator;
        buttonUpdateUser.Enabled = isAdministrator;
        buttonDeleteUser.Enabled = isAdministrator;
        buttonUpdateSectionManager.Enabled = isAdministrator;

        tbCreateUser.Enabled = isAdministrator;
        tbCreateName.Enabled = isAdministrator;
        tbCreatePhone.Enabled = isAdministrator;
        tbCreateTitle.Enabled = isAdministrator;
        tbCreateEmail.Enabled = isAdministrator;
        tbCreatePassword.Enabled = isAdministrator;
        tbCreatePassword2.Enabled = isAdministrator;

        tbEditName.Enabled = isAdministrator;
        tbEditPhone.Enabled = isAdministrator;
        tbEditTitle.Enabled = isAdministrator;
        tbEditEmail.Enabled = isAdministrator;
        tbChangePassword.Enabled = isAdministrator;
        tbChangePassword2.Enabled = isAdministrator;

        tbSectionManager.Enabled = isAdministrator;
    }    

    protected void populateUserList()
    {
        ddUsers.Items.Clear();
        ddUsers.Items.Add(new ListItem("---", "---"));

        MembershipUserCollection collection = Membership.GetAllUsers();
        foreach (MembershipUser user in collection)        
            ddUsers.Items.Add(new ListItem(user.UserName, user.UserName));        
    }
    
    protected void buttonCreateUser_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(tbCreateUser.Text))
        {            
            Utils.displayStatus(ref labelStatus, Color.Red, "Feltet for bruker navn kan ikke være tomt");
            return;
        }

        if (tbCreatePassword.Text.Length < Membership.Provider.MinRequiredPasswordLength)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Passordet må ha minst " + Membership.Provider.MinRequiredPasswordLength.ToString() + " tegn");
            return;
        }

        if (tbCreatePassword.Text != tbCreatePassword2.Text)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Passordene er ikke like");
            return;
        }

        try
        {
            MembershipCreateStatus status = new MembershipCreateStatus();
            MembershipUser user = Membership.CreateUser(tbCreateUser.Text, tbCreatePassword.Text, "", "question", "answer", true, out status);
            if (user == null)
            {
                Utils.displayStatus(ref labelStatus, Color.Red, Utils.getErrorMessage(status));
                return;
            }

            ProfileCommon prof = (ProfileCommon)ProfileCommon.Create(tbCreateUser.Text, true);
            if(prof == null)
            {
                Membership.DeleteUser(tbCreateUser.Text);
                Utils.displayStatus(ref labelStatus, Color.Red, "Oppretting av profil for bruker " + tbCreateUser.Text + " feilet");
                return;
            }
            prof.Name = tbCreateName.Text;            
            prof.Phone = tbCreatePhone.Text;
            prof.Email = tbCreateEmail.Text;
            prof.Title = tbCreateTitle.Text;
            prof.Save();

            tbCreateUser.Text = "";
            tbCreateName.Text = "";
            tbCreatePhone.Text = "";
            tbCreateEmail.Text = "";
            tbCreateTitle.Text = "";

            populateUserList();

            Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Bruker '" + user.UserName + "' opprettet");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
        }        
    }

    protected void buttonDeleteUser_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(ddUsers.SelectedValue) || ddUsers.SelectedValue == "---")
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Du må velge en bruker først");
            return;
        }

        if (ddUsers.SelectedValue == "Administrator")
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Du kan ikke slette denne brukeren");
            return;
        }

        if (!Membership.DeleteUser(ddUsers.SelectedValue))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Sletting av bruker " + ddUsers.SelectedValue + " feilet");
            return;
        }

        Utils.displayStatus(ref labelStatus, Color.Red, "Bruker '" + ddUsers.SelectedValue + "' slettet");
    }

    protected void buttonUpdateUser_OnClick(object sender, EventArgs e)
    {
        if (ddUsers.SelectedValue == "---")
        {
            ddUsers.SelectedIndex = -1;
            tbEditName.Text = "";
            tbEditTitle.Text = "";
            tbEditPhone.Text = "";
            tbEditEmail.Text = "";                        
            return;
        }

        try
        {
            ProfileCommon prof = Profile.GetProfile(ddUsers.SelectedValue);
            prof.Name = tbEditName.Text;
            prof.Title = tbEditTitle.Text;
            prof.Phone = tbEditPhone.Text;
            prof.Email = tbEditEmail.Text;
            prof.Save();

            ddUsers.SelectedIndex = -1;
            tbEditName.Text = "";
            tbEditTitle.Text = "";
            tbEditPhone.Text = "";
            tbEditEmail.Text = "";            

            Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Bruker '" + prof.Name + "' oppdatert");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
        }                
    }

    protected void ddUsers_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddUsers.SelectedValue == "---")
            {
                for (int i = 0; i < tableRoles.Rows.Count; i++)
                {
                    CheckBox cb = tableRoles.Rows[i].Cells[0].Controls[0] as CheckBox;
                    cb.Checked = false;
                }

                ddUsers.SelectedIndex = -1;
                tbEditName.Text = "";
                tbEditTitle.Text = "";
                tbEditPhone.Text = "";
                tbEditEmail.Text = "";
                return;
            }

            MembershipUser user = Membership.GetUser(ddUsers.SelectedValue);

            ProfileCommon prof = Profile.GetProfile(ddUsers.SelectedValue);
            tbEditName.Text = prof.Name;
            tbEditTitle.Text = prof.Title;
            tbEditPhone.Text = prof.Phone;
            tbEditEmail.Text = prof.Email;                                    

            for (int i = 0; i < tableRoles.Rows.Count; i++)
            {
                CheckBox cb = tableRoles.Rows[i].Cells[0].Controls[0] as CheckBox;
                if (Roles.IsUserInRole(ddUsers.SelectedValue, cb.Text))
                    cb.Checked = true;
                else cb.Checked = false;

                if (ddUsers.SelectedValue == "Administrator")
                {
                    cb.Enabled = false;
                }
                else
                {
                    if (ddUsers.SelectedValue == HttpContext.Current.User.Identity.Name)
                    {
                        cb.Enabled = false;
                    }
                    else
                    {
                        if (HttpContext.Current.User.IsInRole("Administrator"))
                        {
                            cb.Enabled = true;
                        }
                        else
                        {
                            cb.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
        }        
    }

    protected void cb_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddUsers.SelectedValue != "---")
            {
                CheckBox cb = sender as CheckBox;
                if (cb.Checked)
                {
                    if (!Roles.IsUserInRole(ddUsers.SelectedValue, cb.Text))
                        Roles.AddUserToRole(ddUsers.SelectedValue, cb.Text);
                    Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Bruker " + ddUsers.SelectedValue + " lagt til i rolle " + cb.Text);
                }
                else
                {
                    if (Roles.IsUserInRole(ddUsers.SelectedValue, cb.Text))
                        Roles.RemoveUserFromRole(ddUsers.SelectedValue, cb.Text);
                    Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Bruker " + ddUsers.SelectedValue + " fjernet fra rolle " + cb.Text);
                }
            }
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
        }        
    }

    protected void buttonChangePassword_OnClick(object sender, EventArgs e)
    {
        if (ddUsers.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Du må velge en bruker først");
            return;
        }        

        if (tbChangePassword.Text.Length < Membership.Provider.MinRequiredPasswordLength)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Passordet må ha minst " + Membership.Provider.MinRequiredPasswordLength.ToString() + " tegn");
            return;
        }

        if (tbChangePassword.Text != tbChangePassword2.Text)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Passordene er ikke like");
            return;
        }                

        MembershipUser u = Membership.GetUser(ddUsers.SelectedItem.Text, false);

        if (u == null)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Bruker " + Server.HtmlEncode(ddUsers.SelectedItem.Text) + " finnes ikke");            
            return;
        }

        try
        {
            string newPassword = u.ResetPassword();

            if (newPassword != null)
            {
                if (!u.ChangePassword(newPassword, tbChangePassword.Text))
                {
                    Utils.displayStatus(ref labelStatus, Color.Red, "Failed to change password");                    
                    return;
                }
            }
            else
            {
                Utils.displayStatus(ref labelStatus, Color.Red, "Failed to reset password");                
                return;
            }

            Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Nytt passord for bruker '" + u.UserName + "' er " + tbChangePassword.Text);
            tbChangePassword.Text = "";
            tbChangePassword2.Text = "";
        }
        catch (MembershipPasswordException ex)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Invalid password answer. Please re-enter and try again");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
        }                
    }

    protected void buttonUpdateSectionManager_OnClick(object sender, EventArgs e)
    {        
        try
        {
            Database.Interface.open();
            Database.Configuration configuration = new Database.Configuration();
            if (configuration.select_all_where_name("Default"))
            {
                configuration.SectionManager = tbSectionManager.Text;
                configuration.update_all_by_name();
                Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Seksjonssjef oppdatert");
            }
            else
            {
                Utils.displayStatus(ref labelStatus, Color.Red, "Konfigurasjon 'Default' ikke funnet");
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

    protected void buttonUpdateRingtestAdminEmail_OnClick(object sender, EventArgs e)
    {
        if (!Utils.isValidEmail(tbRingtestAdminEmail.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Epost-adressen har ugyldig format");
            return;
        }

        try
        {
            Database.Interface.open();
            Database.Configuration configuration = new Database.Configuration();
            if (configuration.select_all_where_name("Default"))
            {
                configuration.RingtestAdminEmail = tbRingtestAdminEmail.Text;
                configuration.update_all_by_name();
                Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Ringtest administrator epost oppdatert");
            }
            else
            {
                Utils.displayStatus(ref labelStatus, Color.Red, "Konfigurasjon 'Default' ikke funnet");
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

    protected void buttonUpdateUserUser_OnClick(object sender, EventArgs e)
    {                
        try
        {
            ProfileCommon prof = Profile.GetProfile(HttpContext.Current.User.Identity.Name);
            prof.Name = tbEditNameUser.Text;
            prof.Title = tbEditTitleUser.Text;
            prof.Phone = tbEditPhoneUser.Text;
            prof.Email = tbEditEmailUser.Text;
            prof.Save();                        

            Utils.displayStatus(ref labelStatusUser, Color.SeaGreen, "Bruker oppdatert");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusUser, Color.Red, ex.Message);
        }    
    }

    protected void buttonChangePasswordUser_OnClick(object sender, EventArgs e)
    {                
        if (tbChangePasswordUser.Text.Length < Membership.Provider.MinRequiredPasswordLength)
        {
            Utils.displayStatus(ref labelStatusUser, Color.Red, "Passordet må ha minst " + Membership.Provider.MinRequiredPasswordLength.ToString() + " tegn");
            return;
        }

        if (tbChangePasswordUser.Text != tbChangePassword2User.Text)
        {
            Utils.displayStatus(ref labelStatusUser, Color.Red, "Passordene er ikke like");
            return;
        }

        MembershipUser u = Membership.GetUser();        

        if (u == null)
        {
            Utils.displayStatus(ref labelStatusUser, Color.Red, "Bruker ble ikke funnet");
            return;
        }

        try
        {
            string newPassword = u.ResetPassword();

            if (newPassword != null)
            {
                if (!u.ChangePassword(newPassword, tbChangePasswordUser.Text))
                {
                    Utils.displayStatus(ref labelStatusUser, Color.Red, "Failed to change password");
                    return;
                }
            }
            else
            {
                Utils.displayStatus(ref labelStatusUser, Color.Red, "Failed to reset password");
                return;
            }

            Utils.displayStatus(ref labelStatusUser, Color.SeaGreen, "Nytt passord er " + tbChangePasswordUser.Text);
            tbChangePasswordUser.Text = "";
            tbChangePassword2User.Text = "";
        }
        catch (MembershipPasswordException ex)
        {
            Utils.displayStatus(ref labelStatusUser, Color.Red, "Invalid password answer. Please re-enter and try again");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusUser, Color.Red, ex.Message);
        }    
    }
}
