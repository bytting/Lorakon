using System;
using System.Diagnostics;
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
using System.Threading;
using System.Globalization;

using Lang = Resources.Localization;

public partial class CreateUser : System.Web.UI.Page
{
    private Random random = new Random();
    private const string m_DefaultCulture = "nb-NO";

    protected override void InitializeCulture()
    {
        string culture = Convert.ToString(Session["CurrentCulture"]);
        
        if (!string.IsNullOrEmpty(culture)) 
            Culture = culture;
        else 
            Culture = m_DefaultCulture;
    
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        
        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!Page.IsPostBack)
        {
            Session["CaptchaImageText"] = GenerateRandomCode();
        }    
    }

    protected void clearAllFields()
    {
        textBoxFullname.Text = "";
        textBoxContact.Text = "";
        textBoxAddress.Text = "";
        textBoxPostal.Text = "";
        textBoxEmail.Text = "";
        textBoxPhone.Text = "";
        textBoxMobile.Text = "";
        textBoxFax.Text = "";
        textBoxWebsite.Text = "";
        textBoxCAPTCHA.Text = "";
    }

    protected void buttonRequestUser_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(textBoxFullname.Text) 
            || String.IsNullOrEmpty(textBoxAddress.Text) 
            || String.IsNullOrEmpty(textBoxPostal.Text) 
            || String.IsNullOrEmpty(textBoxEmail.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Missing_fields);
            return;
        }

        if (!Utils.isValidEmail(textBoxEmail.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Email_invalid_address);
            return;
        }

        Database.Configuration configuration = new Database.Configuration();

        try
        {
            if (textBoxCAPTCHA.Text == Session["CaptchaImageText"].ToString())
            {
                Database.Interface.open();

                if (!configuration.select_all_where_name("Default"))
                {                    
                    Utils.displayStatus(ref labelStatus, Color.Red, Lang.Configuration + " " + Lang.not_found);
                    return;
                }

                Database.PendingAccount pendingAccount = new Database.PendingAccount(
                    textBoxFullname.Text,
                    textBoxContact.Text,
                    textBoxAddress.Text,
                    "Field not active", 
                    textBoxPostal.Text,
                    textBoxEmail.Text,
                    textBoxPhone.Text,
                    textBoxMobile.Text,
                    textBoxFax.Text,
                    textBoxWebsite.Text);

                pendingAccount.insert_with_ID(Guid.NewGuid());                

                Utils.displayStatus(ref labelStatus, Color.SeaGreen, Lang.Account_request_accepted);
            }
            else
            {
                textBoxCAPTCHA.Text = "";
                Utils.displayStatus(ref labelStatus, Color.Red, Lang.Numbers_from_picture_doesnt_match);
            }
        }
        catch (Exception ex)
        {
            textBoxCAPTCHA.Text = "";
            Utils.reportStatus(ref labelStatus, Color.Red, "CreateUser.buttonRequestUser_OnClick: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }

        if (ConfigurationManager.AppSettings["UseEmail"] == "yes")
        {
            string receip = textBoxEmail.Text;
            string mailTitle = "Forespørsel om ny LORAKON konto";
            string mailBody = "Forespørsel om ny LORAKON konto fra " + textBoxFullname.Text;

            buttonCancel.Text = Resources.Localization.Back;
            buttonRequestUser.Enabled = false;

            clearAllFields();

            try
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServer"]) || String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServerPort"]))
                {
                    Utils.displayStatus(ref labelStatus, Color.Red, Lang.Account_request_accepted + ". " + Lang.Email_no_server);
                    return;
                }

                MailMessage mail = new MailMessage();
                mail.To.Add(configuration.RingtestAdminEmail);
                mail.From = new MailAddress(configuration.RingtestAdminEmail);
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.Default;
                mail.Subject = mailTitle;
                mail.Body = mailBody;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["MailServer"], Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);

                Utils.displayStatus(ref labelStatus, Color.SeaGreen, Lang.Account_request_accepted + ". " + Resources.Localization.Email_sent);
            }
            catch (Exception ex)
            {
                Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
            }
        }
    }

    protected void buttonCancel_OnClick(object sender, EventArgs e)
    {        
        Response.Redirect("~/Login.aspx");
    }

    private string GenerateRandomCode()
    {
        string s = "";
        for (int i = 0; i < 6; i++)
            s = String.Concat(s, this.random.Next(10).ToString());
        return s;
    }
}
