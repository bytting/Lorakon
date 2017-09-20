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

public partial class ForgotPassword : System.Web.UI.Page
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

    protected void buttonSendRequest_OnClick(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["UseEmail"] != "yes")
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Sending av epost er deaktivert");
            return;
        }

        if (String.IsNullOrEmpty(tbCompanyName.Text))
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Missing_fields);
            return;
        }

        if (textBoxCAPTCHA.Text == Session["CaptchaImageText"].ToString())
        {
            try
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServer"]) || String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServerPort"]))
                {
                    Utils.displayStatus(ref labelStatus, Color.Red, Lang.Email_no_server);
                    return;
                }

                Database.Configuration configuration = new Database.Configuration();
                Database.Interface.open();
                if (!configuration.select_all_where_name("Default"))
                {
                    Utils.displayStatus(ref labelStatus, Color.Red, Lang.Configuration + " " + Lang.not_found);
                    return;
                }
                Database.Interface.close();

                MailMessage mail = new MailMessage();
                mail.To.Add(configuration.RingtestAdminEmail);
                mail.From = new MailAddress(configuration.RingtestAdminEmail);
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.Default;
                mail.Subject = "LORAKON - Konto har glemt passordet";
                mail.Body = "Konto " + tbCompanyName.Text + " har glemt passordet til LORAKON sidene";
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["MailServer"], Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);

                buttonCancel.Text = Lang.Back;
                buttonSendRequest.Enabled = false;

                Utils.displayStatus(ref labelStatus, Color.SeaGreen, Lang.Email_sent);
            }
            catch (Exception ex)
            {
                textBoxCAPTCHA.Text = "";
                Utils.displayStatus(ref labelStatus, Color.Red, ex.Message);
            }
        }
        else
        {
            textBoxCAPTCHA.Text = "";
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.Numbers_from_picture_doesnt_match);
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
