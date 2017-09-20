using System;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Utils
/// </summary>
public static class Utils
{    
    public static void displayStatus(ref Label label, Color color, string message)
    {
        message.Replace("<", "&lt;");
        message.Replace(">", "&gt;");
        message.Replace("&", "&amp;");
        message.Replace("\"", "&quot;");
        label.ForeColor = color;        
        label.Text = "[" + DateTime.Now.ToLongTimeString() + "] " + message;
    }

    public static void reportStatus(ref Label label, Color color, string message)
    {
        displayStatus(ref label, color, message);

        if (ConfigurationManager.AppSettings["ReportSystemCriticalErrors"].ToLower() == "yes")
        {
            if (ConfigurationManager.AppSettings["UseEmail"] == "yes")
            {
                try
                {
                    Database.Configuration configuration = new Database.Configuration();
                    Database.Interface.open();
                    if (!configuration.select_all_where_name("Default"))
                        return;

                    MailMessage mail = new MailMessage();
                    mail.To.Add(configuration.RingtestAdminEmail);
                    mail.From = new MailAddress(configuration.RingtestAdminEmail);
                    mail.BodyEncoding = System.Text.Encoding.Default;
                    mail.IsBodyHtml = true;
                    mail.Subject = "LORAKON - Feilmelding";
                    mail.Body = "LORAKON - Feilmelding&nbsp;&nbsp;&nbsp;&nbsp;" + DateTime.Now.ToLongTimeString() + "<br><br>" + message;
                    SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["MailServer"], Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]));
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mail);
                }
                catch (Exception ex) { }
                finally
                {
                    Database.Interface.close();
                }
            }
        }
    }

    public static bool isValidEmail(string email)
    {        
        string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";    
        Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);        
        bool valid = false;
        
        if (string.IsNullOrEmpty(email))
        {
            valid = false;
        }
        else
        {            
            valid = check.IsMatch(email);
        }        
        return valid;
    }    

    public static string intToString(int value)
    {
        if (value == int.MinValue)
            return "";

        return value.ToString();
    }

    public static string roundOff(float d, int c)
    {
        if (d == float.MinValue)
            return "";

        string format = "{0:0.0";
        for (int i = 0; i < c-1; i++)
            format += "0";
        format += "}";
        return String.Format(format, d);        
    }

    public static string getErrorMessage(MembershipCreateStatus status)
    {
        switch (status)
        {
            case MembershipCreateStatus.DuplicateUserName:
                return "Brukernavn finnes allerede. Bruk et annet brukernavn";
            case MembershipCreateStatus.DuplicateEmail:
                return "En bruker for den e-mail adressen finnes allerede. Bruk en annen e-mail adresse";
            case MembershipCreateStatus.InvalidPassword:
                return "Passordet er ugyldig";
            case MembershipCreateStatus.InvalidEmail:
                return "E-post adressen er ugyldig";
            case MembershipCreateStatus.InvalidAnswer:
                return "The password retrieval answer provided is invalid. Please check the value and try again.";
            case MembershipCreateStatus.InvalidQuestion:
                return "The password retrieval question provided is invalid. Please check the value and try again.";
            case MembershipCreateStatus.InvalidUserName:
                return "Brukernavnet er ugyldig. Velg et annet brukernavn";
            case MembershipCreateStatus.ProviderError:
                return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            case MembershipCreateStatus.UserRejected:
                return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            default:
                return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        }
    }
}
