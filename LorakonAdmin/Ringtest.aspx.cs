using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.Globalization;
using ZedGraph;

using PdfDocument = iTextSharp.text.Document;
using PdfWriter = iTextSharp.text.pdf.PdfWriter;
using PdfContentByte = iTextSharp.text.pdf.PdfContentByte;
using PdfBaseFont = iTextSharp.text.pdf.BaseFont;
using PdfFont = iTextSharp.text.Font;
using PdfPhrase = iTextSharp.text.Phrase;
using PdfElement = iTextSharp.text.Element;
using PdfPTable = iTextSharp.text.pdf.PdfPTable;
using PdfParagraph = iTextSharp.text.Paragraph;
using PdfChunk = iTextSharp.text.Chunk;
using PdfFontFactory = iTextSharp.text.FontFactory;

public partial class Ringtest : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");            

        if (!Page.IsPostBack)
        {
            buttonInsert.Attributes.Add("onclick", "javascript: return confirm('Ved oppretting av ny ringtest vil alle tilordnede ringtestbokser fraordnes. Vil du fortsette?')");            
            dataSourceAccountsAssignBox.SelectParameters["boxID"].DefaultValue = Guid.Empty.ToString();

            try
            {
                Database.Interface.open();

                Database.Ringtest ringtest = new Database.Ringtest();
                if (ringtest.select_all_where_year(DateTime.Now.Year))
                {
                    tabCreate.HeaderText = "Rediger årets ringtest";
                    multiViewCreateRingtest.SetActiveView(viewEditRingtest);
                    textBoxStartDateE.Text = ringtest.StartDate.ToShortDateString();
                    tbArchiveRefE.Text = ringtest.ArchiveRef;
                    textBoxCommentE.Text = ringtest.Comment;
                    cbFinishedE.Checked = ringtest.Finished;

                    List<string> nameList = new List<string>();
                    Database.Account.select_Name_where_LastRegistrationYear(ref nameList, DateTime.Now.Year);
                    foreach (string name in nameList)
                    {
                        lbRingtestMembers.Items.Add(new ListItem(name));
                    }                    
                }
                else
                {
                    tabCreate.HeaderText = "Opprett årets ringtest";
                    multiViewCreateRingtest.SetActiveView(viewCreateRingtest);
                }

                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Moderator"))
                {
                    tabCreate.Enabled = false;
                    tabEdit.Enabled = false;
                    tabShowIncomplete.Enabled = false;
                    tabAssignBoxes.Enabled = false;                    
                }

                if (!HttpContext.Current.User.IsInRole("Administrator"))
                {
                    cbShowIncompleteForCurrentYear.Enabled = false;
                }

                dataSourceIncomplete.SelectParameters["year"].DefaultValue = DateTime.Now.Year.ToString();                
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

    protected void buttonInsert_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(textBoxStartDate.Text))
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Mangler informasjon");
            return;
        }

        //try
        //{
            Database.Interface.open();

            Database.Ringtest ringtest = new Database.Ringtest(DateTime.Now.Year, DateTime.Parse(textBoxStartDate.Text), tbArchiveRef.Text, textBoxComment.Text);
            if (!ringtest.insert_with_ID_year(Guid.NewGuid(), DateTime.Now.Year))
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Ringtest for " + DateTime.Now.Year.ToString() + " finnes allerede");                
                return;
            }
            else Utils.displayStatus(ref labelStatusCreate, Color.SeaGreen, "Ringtest for " + DateTime.Now.Year.ToString() + " opprettet");

            Database.Account.update_ringtestBoxID(Guid.Empty);        

            multiViewCreateRingtest.SetActiveView(viewEditRingtest);
            tabCreate.HeaderText = "Rediger årets ringtest";
            textBoxStartDateE.Text = ringtest.StartDate.ToShortDateString();
            tbArchiveRefE.Text = ringtest.ArchiveRef;
            textBoxCommentE.Text = ringtest.Comment;
            cbFinishedE.Checked = false;

            ddYears.DataBind();
            gridShowAllRingtests.DataBind();
        /*}
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }*/
            Database.Interface.close();
    }

    protected void buttonUpdateE_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(textBoxStartDateE.Text))
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Mangler informasjon");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.Ringtest ringtest = new Database.Ringtest();
            if (!ringtest.select_all_where_year(DateTime.Now.Year))
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Ringtest for " + DateTime.Now.Year.ToString() + " ikke funnet");
                return;
            }

            ringtest.StartDate = DateTime.Parse(textBoxStartDateE.Text);
            ringtest.ArchiveRef = tbArchiveRefE.Text;
            ringtest.Comment = textBoxCommentE.Text;
            ringtest.Finished = cbFinishedE.Checked;

            if (!ringtest.update_all_by_ID_year())
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Oppdatering av ringtest feilet");
            else
                Utils.displayStatus(ref labelStatusCreate, Color.SeaGreen, "Ringtest oppdatert");

            gridShowAllRingtests.DataBind();
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

    protected void buttonShowMemberEmails_OnClick(object sender, EventArgs e)
    {
        try
        {
            Database.Interface.open();

            string emails = "";
            List<string> emailList = new List<string>();
            Database.Account.select_Email_where_LastRegistrationYear(ref emailList, DateTime.Now.Year);
            foreach (string email in emailList)
                emails += email + ";";

            textBoxShowMemberEmails.Text = emails;
        }
        catch(Exception ex)
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void ddYears_OnDataBound(object sender, EventArgs e)
    {
        ddYears.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));
    }

    private void buildRingtestReportsQuery()
    {
        if (ddYears.SelectedValue == Guid.Empty.ToString())
        {
            dataSourceRingtestReports.SelectCommand = "SELECT RingtestReport.ID, Account.vchName as accountName, Device.vchSerialnumber as deviceSerialnumber, RingtestBox.vchKNumber as boxKNumber, bitWantEvaluation, bitEvaluated, bitApproved, bitAnswerByEmail, bitAnswerSent FROM RingtestReport, Account, Device, RingtestBox WHERE 0 = 1";
        }
        else
        {
            if (cbRingtestReportsEvalOnly.Checked)
            {
                dataSourceRingtestReports.SelectCommand = "SELECT RingtestReport.ID, Account.vchName as accountName, Device.vchSerialnumber as deviceSerialnumber, RingtestBox.vchKNumber as boxKNumber, bitWantEvaluation, bitEvaluated, bitApproved, bitAnswerByEmail, bitAnswerSent FROM RingtestReport, Account, Device, RingtestBox WHERE Account.ID = RingtestReport.AccountID AND Device.ID = RingtestReport.DetectorID AND RingtestBox.ID = RingtestReport.RingtestBoxID AND RingtestReport.RingtestID = '" + ddYears.SelectedValue + "' AND RingtestReport.bitWantEvaluation = 1 AND RingtestReport.bitApproved = 0";
            }
            else
            {
                dataSourceRingtestReports.SelectCommand = "SELECT RingtestReport.ID, Account.vchName as accountName, Device.vchSerialnumber as deviceSerialnumber, RingtestBox.vchKNumber as boxKNumber, bitWantEvaluation, bitEvaluated, bitApproved, bitAnswerByEmail, bitAnswerSent FROM RingtestReport, Account, Device, RingtestBox WHERE Account.ID = RingtestReport.AccountID AND Device.ID = RingtestReport.DetectorID AND RingtestBox.ID = RingtestReport.RingtestBoxID AND RingtestReport.RingtestID = '" + ddYears.SelectedValue + "'";
            }
        }
    }

    protected void ddYears_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        buildRingtestReportsQuery();
        gridRingtestReports.DataBind();
    }

    protected void gridRingtestReports_OnSorting(object sender, EventArgs ev)
    {
        buildRingtestReportsQuery();
    }

    protected void cbRingtestReportsEvalOnly_OnCheckChanged(object sender, EventArgs ev)
    {
        ddYears_OnSelectedIndexChanged(sender, ev);
    }

    protected void gridRingtestReports_OnSelectedIndexChanged(object sender, EventArgs ev)
    {
        try
        {
            Database.Interface.open();

            Database.RingtestReport report = new Database.RingtestReport();
            if (!report.select_all_where_ID((Guid)gridRingtestReports.SelectedDataKey.Value))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ringtest ble ikke funnet");
                return;
            }

            Database.Contact contact = new Database.Contact();
            if (report.ContactID != Guid.Empty)
            {
                if (!contact.select_all_by_ID(report.ContactID))
                {
                    Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kontaktperson ble ikke funnet");
                    return;
                }
            }

            Database.RingtestBox box = new Database.RingtestBox();
            if (!box.select_all_where_ID(report.RingtestBoxID))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ringtestboks ble ikke funnet");
                return;
            }            

            hiddenContactID.Value = report.ContactID.ToString();
            hiddenAccountName.Value = gridRingtestReports.SelectedRow.Cells[1].Text;
            hiddenDetector.Value = gridRingtestReports.SelectedRow.Cells[2].Text;
            hiddenRingtestBox.Value = gridRingtestReports.SelectedRow.Cells[3].Text;

            cellAccount1.Text = "Konto: " + hiddenAccountName.Value;
            cellDetector1.Text = "Detektor: " + hiddenDetector.Value;
            cellBox1.Text = "Ringtest boks: " + hiddenRingtestBox.Value;
            cellBox2.Text = "Referanseverdi: " + Utils.roundOff(box.RefValue, 1);
            cellBox3.Text = "Referansedato: " + box.RefDate.ToShortDateString();
            if (report.ContactID != Guid.Empty)
            {
                cellContact1.Text = "Kontaktperson: " + contact.Name;
                cellContact2.Text = "Telefon: " + contact.Phone;
                cellContact3.Text = "Mobil: " + contact.Mobile;
                cellContact4.Text = "E-post: " + contact.Email;
            }

            if (report.MCAType == "Serie10")
            {
                labelMCA.Text = "Serie 10/10+";
            }
            else
            {
                labelMCA.Text = "Annet (Ikke Serie 10/10+)";                
            }            

            labelMeasureDate.Text = report.MeasureDate.ToShortDateString();
            labelBackground.Text = Utils.roundOff(report.Background, 2);
            labelIntegralBackground.Text = Utils.intToString(report.IntegralBackground);
            labelCountingBackground.Text = Utils.intToString(report.CountingBackground);
            labelGeometryFactor.Text = Utils.roundOff(report.GeometryFactor, 4);
            labelActivity.Text = Utils.roundOff(report.Activity, 1);
            labelActivityRef.Text = Utils.roundOff(report.ActivityRef, 1);
            labelUncertainty.Text = Utils.roundOff(report.Uncertainty, 1);
            labelAvgIntegralSample.Text = Utils.roundOff(report.AvgIntegralSample, 1);
            labelAvgLivetimeSample.Text = Utils.roundOff(report.AvgLivetimeSample, 1);
            labelRefDate.Text = report.RefDate.ToShortDateString();
            labelError.Text = Utils.roundOff(report.Error, 4);                        
            cbWantEvaluation.Checked = report.WantEvaluation;
            cbEvaluated.Checked = report.Evaluated;
            cbApproved.Checked = report.Approved;
            cbAnswerByEmail.Checked = report.AnswerByEmail;
            cbAnswerSent.Checked = report.AnswerSent;            
            gridComments.DataSource = Database.RingtestReport.get_comments(report.ID);
            gridComments.DataBind();

            float tempError = 0f;

            if (report.MCAType.ToLower() == "inspector1000")
            {
                if (report.ActivityRef != float.MinValue)
                {
                    tempError = ((report.ActivityRef - box.RefValue) / box.RefValue) * 100.0f;
                    labelError.Text = Utils.roundOff(tempError, 2);
                    float sigmaA = (report.Uncertainty / report.ActivityRef) * 100.0f;
                    labelSigmaA.Text = Utils.roundOff(sigmaA, 1);
                }
            }
            else
            {                
                if (report.CountingBackground > 0 && report.AvgLivetimeSample > 0 && box.Weight > 0 && report.CountingBackground > 0)
                {
                    float IbLb = (float)report.IntegralBackground / (float)report.CountingBackground;
                    labelIbLb.Text = Utils.roundOff(IbLb, 4);

                    float Vp = box.Weight;
                    float Vf = 200f / Vp;
                    float A = (((report.AvgIntegralSample / report.AvgLivetimeSample) - IbLb) * report.GeometryFactor * Vf);
                    labelA.Text = Utils.roundOff(A, 4);

                    TimeSpan span = report.MeasureDate - report.RefDate;
                    float yearSpan = span.Days / 364.75f;

                    float exp = (-(float)Math.Log(2d) / 30.2f) * yearSpan;
                    float A0 = A / (float)Math.Pow(Math.E, (double)exp);
                    labelA0.Text = Utils.roundOff(A0, 4);

                    float a = (float)Math.Pow(report.GeometryFactor / report.AvgLivetimeSample, 2d);
                    float b = (float)Math.Pow(Math.Sqrt(report.AvgIntegralSample), 2d);
                    float temp_c = (report.AvgIntegralSample / report.AvgLivetimeSample) - report.Background;
                    float c = (float)Math.Pow(temp_c, 2f);
                    float d = (float)Math.Pow((0.03f * report.GeometryFactor), 2d);
                    float f = (float)Math.Pow(temp_c * (report.GeometryFactor / box.Weight), 2d);
                    float g = (float)Math.Pow(0.01f * box.Weight, 2d);
                    float h = (float)Math.Pow(report.GeometryFactor, 2d);
                    float temp_i = (float)Math.Sqrt(report.IntegralBackground) / report.CountingBackground;
                    float i = (float)Math.Pow(temp_i, 2d);
                    float sigmaA = 1.96f * ((Vf * (float)Math.Sqrt(a * b + c * d + f * g + h * i) / A0) * 100.0f);
                    labelSigmaA.Text = Utils.roundOff(sigmaA, 1);                    
                }
                else
                {
                    labelIbLb.Text = "Kan ikke beregnes";
                    labelA.Text = "Kan ikke beregnes";
                    labelA0.Text = "Kan ikke beregnes";
                    labelSigmaA.Text = "Kan ikke beregnes";
                    Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Kan ikke vise kontrollverdier fordi en eller flere felter har ugyldige tall");
                }

                tempError = ((report.ActivityRef - box.RefValue) / box.RefValue) * 100.0f;
                labelError.Text = Utils.roundOff(tempError, 2);
            }

            if (report.MCAType == "Inspector1000")
            {
                labelGeoEff.Text = "Effektivitet: ";
                labelIntegralBackground.Visible = false;
                labelIntegralBackgroundDesc.Visible = false;
                labelCountingBackground.Visible = false;
                labelCountingBackgroundDesc.Visible = false;
                labelBackground.Visible = false;
                labelBackgroundDesc.Visible = false;
                labelIbLb.Visible = false;
                labelAvgIntegralSample.Visible = false;
                labelAvgIntegralSampleDesc.Visible = false;
                labelActivity.Visible = false;
                labelA.Visible = false;
                labelActivityDesc.Visible = false;
            }
            else
            {
                labelGeoEff.Text = "Geometrifaktor: ";
                labelIntegralBackground.Visible = true;
                labelIntegralBackgroundDesc.Visible = true;
                labelCountingBackground.Visible = true;
                labelCountingBackgroundDesc.Visible = true;
                labelIbLb.Visible = true;
                labelBackground.Visible = true;
                labelBackgroundDesc.Visible = true;
                labelAvgIntegralSample.Visible = true;
                labelAvgIntegralSampleDesc.Visible = true;
                labelActivity.Visible = true;
                labelA.Visible = true;
                labelActivityDesc.Visible = true;
            }

            multiViewRingtestReports.SetActiveView(viewRingtestReport);
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

    protected void gridRingtestReports_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';this.style.color='#FF0000';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.color='#222222';";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gridRingtestReports, "Select$" + e.Row.RowIndex);
        }
    }

    protected void buttonRingtestReportUpdate_OnClick(object sender, EventArgs e)
    {
        if (gridRingtestReports.SelectedDataKey == null || (Guid)gridRingtestReports.SelectedDataKey.Value == Guid.Empty)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ingen ringtestrapport er valgt");
            return;
        }

        if (!cbWantEvaluation.Checked)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kan ikke oppdatere en rapport som ikke ønsker evaluering");
            return;
        }

        if (cbApproved.Checked && !cbEvaluated.Checked)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kan ikke godkjenne en rapport som ikke er evaluert");
            return;
        }                

        try
        {
            float error = Convert.ToSingle(labelError.Text);
            if ((error > 10.0 || error < -10.0) && cbApproved.Checked)
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kan ikke godkjenne en rapport med feilmargin større enn 10%");
                return;
            }

            Database.Interface.open();

            Database.RingtestReport report = new Database.RingtestReport();
            if (report.select_all_where_ID((Guid)gridRingtestReports.SelectedDataKey.Value))
            {
                report.Error = error;                
                report.Evaluated = cbEvaluated.Checked;
                report.Approved = cbApproved.Checked;
                if (cbApproved.Checked && cbEvaluated.Checked)
                {
                    if (report.MCAType.ToLower() == "serie10")
                        report.CalculatedUncertainty = Convert.ToSingle(labelSigmaA.Text);
                    else report.CalculatedUncertainty = report.Uncertainty;
                }
                report.update_all_by_ID();
                if (!String.IsNullOrEmpty(tbComment.Text))
                {
                    report.add_comment("Strålevernet", tbComment.Text);
                    gridComments.DataSource = Database.RingtestReport.get_comments(report.ID);
                    gridComments.DataBind();
                    tbComment.Text = "";
                }
            }

            Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Ringtest rapport oppdatert");
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

    protected void buttonRingtestReportSendAnswer_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServer"]) || String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServerPort"]))
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kan ikke sende mail, e-post server og port er ikke konfigurert i 'webAppSettings.config'");
            return;
        }

        if (hiddenContactID.Value == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Rapporten mangler kontakt person. Kan ikke sende svar");
            return;
        }

        if (!cbEvaluated.Checked)
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kan ikke sende svar før rapporten er evaluert");
            return;
        }

        if (!cbApproved.Checked && String.IsNullOrEmpty(tbComment.Text))
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ikke godkjente rapporter må ha kommentar");
            return;
        }

        buttonRingtestReportUpdate_OnClick(sender, e);

        if (ConfigurationManager.AppSettings["UseEmail"] == "yes")
        {
            Database.Configuration configuration = new Database.Configuration();
            Database.RingtestReport report = new Database.RingtestReport();
            Database.RingtestBox box = new Database.RingtestBox();
            Database.Contact contact = new Database.Contact();
            Database.Device detector = new Database.Device();

            try
            {
                Database.Interface.open();

                if (!configuration.select_all_where_name("Default"))
                    throw new Exception("buttonRingtestReportSendAnswer_OnClick: Finner ikke Navn til konfigurasjon");

                if (String.IsNullOrEmpty(configuration.RingtestAdminEmail))
                    throw new Exception("buttonRingtestReportSendAnswer_OnClick: Administrator e-post er ikke angitt");

                if (!report.select_all_where_ID((Guid)gridRingtestReports.SelectedDataKey.Value))
                    throw new Exception("buttonRingtestReportSendAnswer_OnClick: Finner ikke ID til rapporten");

                if (!box.select_all_where_ID(report.RingtestBoxID))
                    throw new Exception("buttonRingtestReportSendAnswer_OnClick: Finner ikke ID til boksen");

                if (!contact.select_all_by_ID(new Guid(hiddenContactID.Value)))
                    throw new Exception("buttonRingtestReportSendAnswer_OnClick: Finner ikke ID til kontaktpersonen");

                if (!detector.select_all_where_ID(report.DetectorID))
                    throw new Exception("buttonRingtestReportSendAnswer_OnClick: Finner ikke ID til detektor");

                if (cbApproved.Checked && !cbAnswerSent.Checked)
                {
                    report.update_Bool_by_ID("bitAnswerSent", true);
                    cbAnswerSent.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, ex.Message);
                return;
            }
            finally
            {
                Database.Interface.close();
            }

            string mailTitle = "LORAKON ringtestresultat for detektor " + detector.SerialNumber;
            string mailBody = "";

            if (cbApproved.Checked)
            {
                string result = "";
                if (report.MCAType == "Inspector1000")
                {
                    float unc = (report.Uncertainty / report.ActivityRef) * 100.0f;
                    result = "Måleresultat: " + Utils.roundOff(report.ActivityRef, 1) + " Bq/kg (" + (char)177 + " " + Utils.roundOff(unc, 1).ToString() + "%)";
                }
                else
                {
                    result = "Måleresultat: " + Utils.roundOff(report.ActivityRef, 1)
                        + " Bq/kg (" + (char)177 + " " + Utils.roundOff(Convert.ToSingle(labelSigmaA.Text), 1) + "%)";
                }
                mailBody = @"
Til " + hiddenAccountName.Value + "<br>&nbsp;&nbsp;&nbsp;&nbsp;v/" + contact.Name + @"<br><br>
Et avvik på opptil 10% fra den nominelle aktiviteten i ringtestprøven anses som akseptabelt.
Spesifikk aktivitet for deres ringtestprøve er bestemt til " + Utils.roundOff(box.RefValue, 1) + " Bq/kg " + (char)177 + " " + box.Uncertainty.ToString() + "% (2 sigma) pr. "
        + box.RefDate.ToShortDateString() + @"<br><br>
Oppsummering av rapporten<br><br>&nbsp;&nbsp;&nbsp;&nbsp;Detektorens serienummer: " + detector.SerialNumber + "<br>&nbsp;&nbsp;&nbsp;&nbsp;" + result
        + "<br>&nbsp;&nbsp;&nbsp;&nbsp;Avvik fra referanseverdi: " + Utils.roundOff(report.Error, 2) + @"%<br><br>
Rapporten er evaluert og godkjent.<br>Vi takker for innsatsen i forbindelse med årets ringtest.<br><br>
Hilsen Statens Strålevern";
            }
            else
            {
                mailBody = @"
Til " + hiddenAccountName.Value + "<br>&nbsp;&nbsp;&nbsp;&nbsp;v/" + contact.Name + @"<br><br>
Et avvik på opptil 10% fra den nominelle aktiviteten i ringtestprøven anses som akseptabelt.<br><br>
Oppsummering av rapporten<br><br>&nbsp;&nbsp;&nbsp;&nbsp;Detektorens serienummer: " + detector.SerialNumber + @"<br>&nbsp;&nbsp;&nbsp;
Måleresultat: " + Utils.roundOff(report.ActivityRef, 1) + @" Bq/kg<br><br>
Dette resultatet er ikke innenfor det aksepterte avviket.<br>
Se kommentar i ringtestrapporten (" + ConfigurationManager.AppSettings["LorakonURL"] + @") for forslag til endringer.<br><br>
Hilsen Statens Strålevern";
            }

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(contact.Email);
                mail.From = new MailAddress(configuration.RingtestAdminEmail);
                mail.BodyEncoding = System.Text.Encoding.Default;
                mail.IsBodyHtml = true;
                mail.Subject = mailTitle;
                mail.Body = mailBody;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["MailServer"], Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);

                Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Svar sendt til " + hiddenAccountName.Value);
            }
            catch (Exception ex)
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, ex.Message);
            }
        }
    }

    protected void buttonRingtestReportCancel_OnClick(object sender, EventArgs e)
    {
        multiViewRingtestReports.SetActiveView(viewRingtestReports);
    }

    protected void cbUnassignedAccounts_OnCheckChanged(object sender, EventArgs e)
    {
        ddUsersAssignBox.DataBind();
    }

    protected void cbShowIncompleteForCurrentYear_OnCheckChanged(object sender, EventArgs e)
    {
        if (cbShowIncompleteForCurrentYear.Checked)
            dataSourceIncomplete.SelectCommand = "SELECT RingtestReport.ID AS reportID, Account.vchName AS accountName, Ringtest.intYear AS ringtestYear, Device.vchSerialnumber AS deviceSerialnumber FROM RingtestReport, Account, Device, Ringtest WHERE Account.ID = RingtestReport.AccountID AND Device.ID = RingtestReport.DetectorID AND Ringtest.ID = RingtestReport.RingtestID AND RingtestReport.bitWantEvaluation = 0 AND Ringtest.intYear <= @year ORDER BY Ringtest.intYear DESC";
        else dataSourceIncomplete.SelectCommand = "SELECT RingtestReport.ID AS reportID, Account.vchName AS accountName, Ringtest.intYear AS ringtestYear, Device.vchSerialnumber AS deviceSerialnumber FROM RingtestReport, Account, Device, Ringtest WHERE Account.ID = RingtestReport.AccountID AND Device.ID = RingtestReport.DetectorID AND Ringtest.ID = RingtestReport.RingtestID AND RingtestReport.bitWantEvaluation = 0 AND Ringtest.intYear < @year ORDER BY Ringtest.intYear DESC";

        gridShowIncomplete.DataBind();
    }

    protected void gridShowIncomplete_OnDeleteCommand(object sender, GridViewDeleteEventArgs e)
    {        
        try
        {
            Database.Interface.open();

            Database.RingtestReport report = new Database.RingtestReport();
            if (!report.select_all_where_ID((Guid)gridShowIncomplete.DataKeys[e.RowIndex].Values[0]))
            {
                Utils.displayStatus(ref labelStatusIncomplete, Color.Red, "Rapport ble ikke funnet");
                return;
            }                                        

            if (report.delete_by_ID())
                Utils.displayStatus(ref labelStatusIncomplete, Color.SeaGreen, "Ringtestrapport slettet");
            else
                Utils.displayStatus(ref labelStatusIncomplete, Color.Red, "Sletting av ringtestrapport feilet");
            
            gridShowIncomplete.DataBind();            
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusIncomplete, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void ddUsersAssignBox_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lbBoxHistory.Items.Clear();

        if (ddUsersAssignBox.SelectedValue == Guid.Empty.ToString())
        {
            ddRingtestBoxes.SelectedIndex = -1;
            return;
        }

        try
        {
            Database.Interface.open();

            Guid userID = new Guid(ddUsersAssignBox.SelectedValue);
            Database.Account account = new Database.Account();

            account.select_all_where_ID(userID);
            ddRingtestBoxes.SelectedValue = account.RingtestBoxID.ToString();

            List<Guid> idList = new List<Guid>();
            Database.RingtestReport.select_RingtestBoxID_where_AccountID(userID, ref idList);

            Database.RingtestBox box = new Database.RingtestBox();
            foreach (Guid id in idList)
            {
                box.select_all_where_ID(id);
                lbBoxHistory.Items.Add(new ListItem(box.KNumber));
            }
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusBoxes, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void ddUsersAssignBox_OnDataBound(object sender, EventArgs e)
    {
        if (ddUsersAssignBox.Items.Count == 0 || ddUsersAssignBox.Items[0].Value != Guid.Empty.ToString())
            ddUsersAssignBox.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));

        if (cbUnassignedAccounts.Checked)
        {
            if (cbRegisteredOnly.Checked)
                dataSourceAccountsAssignBox.SelectCommand = "SELECT ID, vchName FROM Account WHERE bitActive=1 AND RingtestBoxID <> @boxID AND intLastRegistrationYear = '" + DateTime.Now.Year.ToString() + "' ORDER BY vchName ASC";
            else dataSourceAccountsAssignBox.SelectCommand = "SELECT ID, vchName FROM Account WHERE bitActive=1 AND RingtestBoxID <> @boxID ORDER BY vchName ASC";
        }
        else
        {
            if (cbRegisteredOnly.Checked)
                dataSourceAccountsAssignBox.SelectCommand = "SELECT ID, vchName FROM Account WHERE bitActive=1 AND RingtestBoxID = @boxID AND intLastRegistrationYear = '" + DateTime.Now.Year.ToString() + "' ORDER BY vchName ASC";
            else
                dataSourceAccountsAssignBox.SelectCommand = "SELECT ID, vchName FROM Account WHERE bitActive=1 AND RingtestBoxID = @boxID ORDER BY vchName ASC";
        }                        
    }

    protected void buttonAssignBox_OnClick(object sender, EventArgs e)
    {
        if (ddUsersAssignBox.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusBoxes, Color.Red, "Du må velge en konto først");
            return;
        }

        try
        {
            Guid accountID = new Guid(ddUsersAssignBox.SelectedValue);

            Database.Interface.open();
            Database.Ringtest ringtest = new Database.Ringtest();
            if (ringtest.select_all_where_year(DateTime.Now.Year))
            {
                Database.RingtestReport report = new Database.RingtestReport();
                if (report.select_all_where_ringtestID_AccountID_approved(ringtest.ID, accountID, false))
                {
                    Utils.displayStatus(ref labelStatusBoxes, Color.Red, "Kan ikke tilordne ny boks. Konto har en eller fler aktive rapporter");
                    return;
                }
            }
            else
            {
                Utils.displayStatus(ref labelStatusBoxes, Color.Red, "Du må opprette årets ringtest før du kan tilordne bokser");
                return;
            }

            Database.Account account = new Database.Account();
            account.select_all_where_ID(accountID);

            Guid boxID = new Guid(ddRingtestBoxes.SelectedValue);

            account.RingtestBoxID = boxID;            

            if (account.update_all_by_ID())
            {
                ddUsersAssignBox.DataBind();
                lbBoxHistory.Items.Clear();
                ddRingtestBoxes.SelectedValue = Guid.Empty.ToString();
                Utils.displayStatus(ref labelStatusBoxes, Color.SeaGreen, "Ringtestboks tilordnet");
            }
            else
            {
                Utils.displayStatus(ref labelStatusBoxes, Color.Red, "Tilordning av ringtestboks feilet");
            }
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusBoxes, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void ddRingtestBoxes_OnDataBound(object sender, EventArgs e)
    {
        ddRingtestBoxes.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));
    }

    protected void gridShowAllRingtests_OnSelectedIndexChanged(object sender, EventArgs e)    
    {        
        int year = Convert.ToInt32(gridShowAllRingtests.SelectedRow.Cells[1].Text);
        labelRingtestStatsIngress.Text = "Ringtest for " + year.ToString();
        panelRingtestStats.Visible = true;
    }

    protected void gridShowAllRingtests_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';this.style.color='#FF0000';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.color='#222222';";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gridShowAllRingtests, "Select$" + e.Row.RowIndex);
        }
    }

    protected void buttonRingtestStatsCancel_OnClick(object sender, EventArgs e)
    {
        gridShowAllRingtests.SelectedIndex = -1;
        panelRingtestStats.Visible = false;        
    }

    protected void graphStatistics_OnRenderGraph(ZedGraph.Web.ZedGraphWeb z, System.Drawing.Graphics g, ZedGraph.MasterPane masterPane)
    {        
        GraphPane myPane = masterPane[0];

        myPane.Border.Color = Color.White;        
        myPane.Title.Text = "Ringteststatistikk for alle deltagere";
        myPane.XAxis.Title.Text = ""; // "Deltagere";
        myPane.XAxis.Scale.IsVisible = false;        

        myPane.YAxis.Title.Text = "%";
        myPane.YAxis.MajorGrid.IsVisible = true;                
        myPane.YAxis.MinorGrid.IsVisible = true;                

        myPane.YAxis.Scale.Max = 11.0f;
        myPane.YAxis.Scale.Min = -11.0f;        

        myPane.Legend.IsVisible = true;
        
        myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, Color.White), 45.0F);
        myPane.YAxis.Scale.MaxGrace = 0.2;

        if (gridShowAllRingtests.SelectedRow == null)        
        {
            myPane.Title.Text = "Ringteststatistikk for alle deltagere (Ingen resultater funnet)";
            myPane.XAxis.Scale.Max = 1;
            myPane.XAxis.Scale.Min = 0;
            return;
        }

        List<Guid> idList = new List<Guid>();

        try
        {
            int year = Convert.ToInt32(gridShowAllRingtests.SelectedRow.Cells[1].Text);

            Database.Interface.open();
            Database.Account.select_ID(ref idList, year);                        

            PointPairList list = new PointPairList();
            PointPairList eList = new PointPairList();
            PointPairList bList = new PointPairList();

            int count = 0;
            foreach (Guid id in idList)
            {                                
                DataSet dataSet = Database.RingtestReport.select_Error_CalculatedUncertainty_RingtestBoxID_MCAType_ActivityRef_where_AccountID_Year(id, year);
                if(dataSet.Tables[0].Rows.Count <= 0)
                    continue;

                Guid boxID = (Guid)dataSet.Tables[0].Rows[0][2];
                Database.RingtestBox box = new Database.RingtestBox();
                box.select_all_where_ID(boxID);                

                double count_offset = (double)count;
                bList.Add(count_offset, box.Uncertainty, -box.Uncertainty);                

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    double error = Convert.ToDouble(dataSet.Tables[0].Rows[i][0]);
                    double uncertainty = Convert.ToDouble(dataSet.Tables[0].Rows[i][1]);
                    string MCAType = dataSet.Tables[0].Rows[i][3].ToString();
                    if (MCAType.ToLower() != "serie10")
                    {
                        double activity = Convert.ToDouble(dataSet.Tables[0].Rows[i][4]);
                        uncertainty = (uncertainty / activity) * 100.0;
                    }

                    list.Add(count, error);
                    eList.Add(count_offset, error + uncertainty, error - uncertainty);                                        
                }                

                count++;
            }

            myPane.XAxis.Scale.Max = count;
            myPane.XAxis.Scale.Min = -1;

            LineItem curve = myPane.AddCurve("Rapportert resultat", list, Color.White, SymbolType.Circle);
            curve.Line.IsVisible = false;
            curve.Symbol.Size = 6;
            curve.Symbol.Fill = new Fill(Color.SeaGreen);

            ErrorBarItem errBar = myPane.AddErrorBar("Beregnet usikkerhet", eList, Color.Red);
            errBar.Bar.PenWidth = 1;

            ErrorBarItem boxBar = myPane.AddErrorBar("Ringtestboks usikkerhet", bList, Color.LightGray);
            boxBar.Bar.PenWidth = 12;
            boxBar.Bar.Symbol.IsVisible = false;            
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusShowAll, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }

        masterPane.AxisChange(g);    
    }    

    protected void SaveRingtestLetter(string filename)
    {        
        if (gridRingtestReports.SelectedDataKey == null || (Guid)gridRingtestReports.SelectedDataKey.Value == Guid.Empty)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, "Ingen ringtestrapport er valgt");
            return;
        }        

        try
        {               
            Database.Interface.open();

            Database.RingtestReport report = new Database.RingtestReport();
            if (!report.select_all_where_ID((Guid)gridRingtestReports.SelectedDataKey.Value))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Finner ikke rapporten");
                return;
            }

            if (report.AccountID == Guid.Empty)
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ugyldig konto id");
                return;
            }

            Database.Account account = new Database.Account();
            if (!account.select_all_where_ID(report.AccountID))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Finner ikke kontoen");
                return;
            }

            if (report.ContactID == Guid.Empty)
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ugyldig kontakt id");
                return;
            }

            Database.Contact contact = new Database.Contact();
            if (!contact.select_all_by_ID(report.ContactID))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Finner ikke kontaktpersonen");
                return;
            }

            if (report.RingtestID == Guid.Empty)
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ugyldig ringtest id");
                return;
            }

            Database.Ringtest ringtest = new Database.Ringtest();
            if (!ringtest.select_all_where_ID(report.RingtestID))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Finner ikke ringtesten");
                return;
            }

            if (report.DetectorID == Guid.Empty)
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ugyldig detektor id");
                return;
            }

            Database.Device detector = new Database.Device();
            if (!detector.select_all_where_ID(report.DetectorID))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Finner ikke detektoren");
                return;
            }            

            if (report.RingtestBoxID == Guid.Empty)
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Ugyldig ringtestboks id");
                return;
            }

            Database.RingtestBox box = new Database.RingtestBox();
            if (!box.select_all_where_ID(report.RingtestBoxID))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Finner ikke ringtestboksen");
                return;
            }

            Database.Configuration configuration = new Database.Configuration();
            if (!configuration.select_all_where_name("Default"))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Finner ikke konfigurasjon");
                return;
            }

            if (String.IsNullOrEmpty(configuration.SectionManager))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Seksjonssjef er ikke angitt");
                return;
            }

            MemoryStream ms = new MemoryStream();            
            PdfDocument document = new PdfDocument();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);            

            document.Open();

            PdfContentByte cb = writer.DirectContent;

            cb.BeginText();

            PdfBaseFont bf = PdfBaseFont.CreateFont(PdfBaseFont.HELVETICA, PdfBaseFont.CP1252, PdfBaseFont.NOT_EMBEDDED);
            PdfBaseFont bf_bold = PdfBaseFont.CreateFont(PdfBaseFont.HELVETICA_BOLD, PdfBaseFont.CP1252, PdfBaseFont.NOT_EMBEDDED);
            float font_size = 8, font_size_header = 10;

            cb.SetFontAndSize(bf, font_size);

            float leftCursor = 20, topCursor = document.Top - 55, lineSpace = 10;            

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, account.Name, leftCursor, topCursor, 0);
            topCursor -= lineSpace;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "v/" + contact.Name, leftCursor, topCursor, 0);            
            topCursor -= lineSpace;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, account.Address, leftCursor, topCursor, 0);
            topCursor -= lineSpace;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, account.Postal, leftCursor, topCursor, 0);

            topCursor = document.Top - 140;            

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Arkivreferanse: " + ringtest.ArchiveRef, leftCursor, topCursor, 0);
            topCursor -= lineSpace;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Saksbehandler: " + Profile.Name, leftCursor, topCursor, 0);
            topCursor -= lineSpace;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Telefon: " + Profile.Phone, leftCursor, topCursor, 0);
            topCursor -= lineSpace;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "E-post: " + Profile.Email, leftCursor, topCursor, 0);

            leftCursor = document.Right - 20;
            topCursor = document.Top - 150;

            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Dato: " + DateTime.Now.ToShortDateString(), leftCursor, topCursor, 0);

            leftCursor = 20;
            topCursor = document.Top - 255;

            cb.SetFontAndSize(bf_bold, font_size_header);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "RESULTAT AV LORAKON RINGTEST " + ringtest.Year.ToString(), leftCursor, topCursor, 0);

            cb.SetFontAndSize(bf, font_size);
            topCursor -= 22;

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Vi takker for innsatsen i forbindelse med årets ringtest.", leftCursor, topCursor, 0);
            topCursor -= lineSpace * 2;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Et avvik på opptil 10% fra den nominelle aktiviteten i ringtestprøven anses som akseptabelt.", leftCursor, topCursor, 0);
            topCursor -= lineSpace;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "De stasjonene som har et større avvik enn dette må forsøke å identifisere feilkilder, og får eventuelt tilsendt ny ringtest.", leftCursor, topCursor, 0);
            topCursor -= lineSpace * 2;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Ringtestprøvene har individuelt bestemt aktivitet med " + box.Uncertainty.ToString() + "% usikkerhet med 95% konfidensnivå.", leftCursor, topCursor, 0);
            topCursor -= lineSpace;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Spesifikk aktivitet for deres ringtestprøve er bestemt til " + box.RefValue.ToString() + " Bq/kg pr. " + box.RefDate.ToShortDateString(), leftCursor, topCursor, 0);

            topCursor = document.Top - 395;
            leftCursor = document.Right * 0.25f;
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Resultat ringtestmåling", leftCursor, topCursor, 0);
            leftCursor = document.Right * 0.5f;
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Avvik fra referanseverdi", leftCursor, topCursor, 0);
            leftCursor = document.Right * 0.75f;
            if (report.MCAType == "Serie10")
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Usikkerhet(ved 2 sigma)", leftCursor, topCursor, 0);

            topCursor -= lineSpace * 1.5f;

            leftCursor = document.Right * 0.25f;            
            if (report.MCAType == "Serie10")
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Detektor " + detector.SerialNumber + ": " + report.ActivityRef.ToString() + " Bq/kg", leftCursor, topCursor, 0);
            }
            else
            {
                float unc = (report.Uncertainty / report.ActivityRef) * 100.0f;
                if (report.Uncertainty != float.MinValue)
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Detektor " + detector.SerialNumber + ": " + report.ActivityRef.ToString() + " Bq/kg ± " + Utils.roundOff(unc, 1).ToString() + " %", leftCursor, topCursor, 0);
            }            
            leftCursor = document.Right * 0.5f;
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, labelError.Text + " %", leftCursor, topCursor, 0);
            leftCursor = document.Right * 0.75f;
            if (report.MCAType == "Serie10" && labelSigmaA.Text != "Kan ikke beregnes")
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, labelSigmaA.Text + " %", leftCursor, topCursor, 0);

            leftCursor = 20;
            topCursor -= lineSpace * 10f;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Med hilsen Statens Strålevern", leftCursor, topCursor, 0);

            topCursor = document.Top - 635;            
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, configuration.SectionManager, leftCursor, topCursor, 0);
            leftCursor = document.Right / 2f;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Profile.Name, leftCursor, topCursor, 0);
            topCursor -= lineSpace;
            leftCursor = 20f;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Seksjonssjef", leftCursor, topCursor, 0);
            leftCursor = document.Right / 2;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Profile.Title, leftCursor, topCursor, 0);

            cb.EndText();
            
            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            //Response.Close();
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

    protected void SaveRingtestSheet(string filename)
    {
        SqlConnection connection = null;        

        try
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["nrpa_lorakon"].ConnectionString);
            connection.Open();
            string query = @"
SELECT COUNT(a.ID) FROM Account a
WHERE a.intLastRegistrationYear=@year AND a.bitActive=1 AND a.RingtestBoxID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@year", SqlDbType.Int).Value = DateTime.Now.Year;
            command.Parameters.AddWithValue("@id", Guid.Empty);
            int count = (int)command.ExecuteScalar();
            if (count > 0)
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "En eller flere påmeldte kontoer mangler ringtestboks");
                return;
            }
            command.Parameters.Clear();

            query = @"
SELECT a.vchName, a.vchContact, a.vchAddress, a.vchPostbox, a.vchPostal, rb.vchKNumber
FROM Account a, RingtestBox rb
WHERE a.intLastRegistrationYear=@year AND a.bitActive=1 AND a.RingtestBoxID = rb.ID
GROUP BY a.vchName, a.vchContact, a.vchAddress, a.vchPostbox, a.vchPostal, rb.vchKNumber
ORDER BY rb.vchKNumber ASC";
            command.CommandText = query;
            command.Parameters.Add("@year", SqlDbType.Int).Value = DateTime.Now.Year;
            
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Ingen er påmeldt til årets ringtest");
                return;
            }

            MemoryStream ms = new MemoryStream();
            PdfDocument document = new PdfDocument();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            cb.BeginText();

            PdfBaseFont bf = PdfBaseFont.CreateFont(PdfBaseFont.HELVETICA, PdfBaseFont.CP1252, PdfBaseFont.NOT_EMBEDDED);
            PdfBaseFont bf_bold = PdfBaseFont.CreateFont(PdfBaseFont.HELVETICA_BOLD, PdfBaseFont.CP1252, PdfBaseFont.NOT_EMBEDDED);

            float font_size = 8, font_size_header = 10;
            float leftCursor = 20, topCursor = document.Top - 200, lineSpace = 10;

            cb.SetFontAndSize(bf_bold, font_size);

            string name, contact, address, postbox, postal, knumber;
            float top = document.Top;

            writeRingtestListHeader(ref cb, 20, document.Top - 20);
            reader.Read();
            knumber = reader.GetString(5).Trim();
            cb.SetFontAndSize(bf_bold, font_size);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Ringtestboks: " + knumber, leftCursor, topCursor + 22, 0);
            cb.SetFontAndSize(bf, font_size);
            do
            {
                if (knumber != reader.GetString(5).Trim())
                {
                    cb.EndText();
                    document.NewPage();
                    cb.SetFontAndSize(bf, font_size);
                    cb.BeginText();

                    writeRingtestListHeader(ref cb, 20, document.Top - 20);
                    topCursor = document.Top - 200;
                    knumber = reader.GetString(5).Trim();
                    cb.SetFontAndSize(bf_bold, font_size);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Ringtestboks: " + knumber, leftCursor, topCursor + 22, 0);
                    cb.SetFontAndSize(bf, font_size);
                }
                else if (topCursor < document.Bottom + 100)
                {
                    cb.EndText();
                    document.NewPage();
                    cb.SetFontAndSize(bf, font_size);
                    cb.BeginText();

                    topCursor = document.Top - 100;
                }

                name = reader.GetString(0);                
                contact = reader.GetString(1);                
                address = reader.GetString(2);                
                postbox = reader.GetString(3);                
                postal = reader.GetString(4);                               
                
                writeRingtestListEntry(ref cb, topCursor, name, contact, address, postbox, postal);
             
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Mottatt: ", document.Right - 125, topCursor, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Sendt: ", document.Right - 125, topCursor - 16, 0);

                cb.EndText();

                cb.SetLineWidth(0.5f);
                cb.MoveTo(document.Right - 120, topCursor);
                cb.LineTo(document.Right - 20, topCursor);

                cb.MoveTo(document.Right - 120, topCursor - 16);
                cb.LineTo(document.Right - 20, topCursor - 16);

                cb.Stroke();

                cb.SetLineWidth(1.0f);
                cb.MoveTo(20, topCursor - 50);
                cb.LineTo(document.Right - 20, topCursor - 50);                

                cb.Stroke();

                cb.BeginText();

                topCursor -= 80;
            }
            while (reader.Read());

            reader.Close();

            cb.EndText();            

            document.Close();            

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, ex.Message);            
        }
        finally
        {
            if (connection != null)
                connection.Close();            
        }
    }            
    
    protected void writeRingtestListHeader(ref PdfContentByte cb, float left, float top)
    {
        PdfBaseFont bf = PdfBaseFont.CreateFont(PdfBaseFont.HELVETICA, PdfBaseFont.CP1252, PdfBaseFont.NOT_EMBEDDED);
        PdfBaseFont bf_bold = PdfBaseFont.CreateFont(PdfBaseFont.HELVETICA_BOLD, PdfBaseFont.CP1252, PdfBaseFont.NOT_EMBEDDED);
        float font_size = 8, font_size_header = 10;
        float lineSpace = 10;

        cb.SetFontAndSize(bf_bold, font_size_header);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "LORAKON Ringtestliste " + DateTime.Now.Year.ToString(), left, top, 0);
        top -= lineSpace * 2;
        cb.SetFontAndSize(bf, font_size);
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Statens Strålevern", left, top, 0);
        top -= lineSpace;
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Postboks 55", left, top, 0);
        top -= lineSpace;
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "1332 Østerås", left, top, 0);
        top -= lineSpace;
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Telefax: +47 671 47 407", left, top, 0);
        top -= lineSpace * 2;
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Siste bedrift på lista returnerer boksen til adressen ovenfor.", left, top, 0);
    }

    protected void writeRingtestListEntry(ref PdfContentByte cb, float top, string name, string contact, string address, string postbox, string postal)
    {
        int leftCursor = 20;
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, name, leftCursor, top, 0);
        if (!String.IsNullOrEmpty(contact))
        {
            top -= 10;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, contact, leftCursor, top, 0);
        }
        top -= 10;
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address, leftCursor, top, 0);
        /*if (!String.IsNullOrEmpty(postbox))
        {
            top -= 10;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, postbox, leftCursor, top, 0);
        }*/
        top -= 10;
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, postal, leftCursor, top, 0);                
    }

    protected void buttonGenerateRingtestSheet_OnClick(object sender, EventArgs e)
    {                
        SaveRingtestSheet("Ringtestliste_" + DateTime.Now.Year.ToString() + ".pdf");                
    } 

    protected void buttonRingtestReportGenerateLetter_OnClick(object sender, EventArgs e)
    {       
        string accountname = hiddenAccountName.Value;        
        accountname = Regex.Replace(accountname, @"[\s,]", "_");
        accountname = Regex.Replace(accountname, @"[^\w\d_-]", "");

        accountname = accountname.Replace("198", "AE");
        accountname = accountname.Replace("216", "OE");
        accountname = accountname.Replace("197", "AA");
        accountname = accountname.Replace("230", "ae");
        accountname = accountname.Replace("248", "oe");
        accountname = accountname.Replace("229", "aa");

        string serial = hiddenDetector.Value;
        serial = Regex.Replace(serial, @"[\s,.]", "_");
        serial = Regex.Replace(serial, @"[^\w\d_-]", "");                
        string file = accountname + "_" + serial + "_" + DateTime.Now.Year.ToString() + ".pdf";

        file.Trim();                    
        
        SaveRingtestLetter(file);        
    }    
}
