using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlTypes;
using System.Globalization;
using ZedGraph;

using Lang = Resources.Localization;

public partial class Ringtest : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);
                object UserGUID = User.ProviderUserKey;
                hiddenAccountID.Value = UserGUID.ToString();                

                initializePage();
            }
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, "Ringtest.Page_Load: " + ex.Message);
        } 
    }

    protected void initializePage()
    {
        bool ringtestExists = false;
        bool hasStarted = false;

        try
        {
            Database.Interface.open();

            Database.Configuration configuration = new Database.Configuration();
            if (!configuration.select_all_where_name("Default"))
            {
                Utils.reportStatus(ref labelStatus, Color.Red, "Ringtest.initializePage: Configuration not found");
                return;
            }

            Database.Ringtest ringtest = new Database.Ringtest();
            if (ringtest.select_all_where_year(DateTime.Now.Year))
            {
                ringtestExists = true;
                if (DateTime.Now >= ringtest.StartDate)
                    hasStarted = true;

                if (hasStarted && ringtest.Finished)
                {
                    multiViewRingtest.SetActiveView(viewFinished);
                    return;
                }
            }

            Database.Account account = new Database.Account();
            if (!account.select_all_where_ID(new Guid(hiddenAccountID.Value)))
            {
                Utils.reportStatus(ref labelStatus, Color.Red, "Ringtest.initializePage: Account not found");
                return;
            }            

            if (account.LastRegistrationYear != DateTime.Now.Year)
            {
                if (hasStarted)
                {
                    // send info to admin
                    multiViewRingtest.SetActiveView(viewSendMessage);                    
                    labelSendMessage.Text = Lang.RingtestAlreadyStarted;                    
                }
                else
                {
                    multiViewRingtest.SetActiveView(viewRegister);                    
                    labelRegister.Text = Lang.RingtestAccountNotRegistered;                    
                }
                return;
            }

            if (hasStarted)
            {
                hiddenRingtestID.Value = ringtest.ID.ToString();
            }
            else
            {
                multiViewRingtest.SetActiveView(viewNoInit);

                if (ringtestExists)                                    
                    labelInit.Text = Lang.RingtestNotStarted + " " + ringtest.StartDate.ToShortDateString();                
                else                
                    labelInit.Text = Lang.RingtestDateNotDetermined;

                return;
            }

            if (account.RingtestBoxID == Guid.Empty)
            {
                multiViewRingtest.SetActiveView(viewSendMessage);
                labelSendMessage.Text = Lang.RingtestAccountBoxNotAssigned;
                return;
            }
            hiddenRingtestBoxID.Value = account.RingtestBoxID.ToString();

            Database.DeviceCategory category = new Database.DeviceCategory();
            if (!category.select_all_where_name("Detektor"))
            {
                multiViewRingtest.SetActiveView(viewNoInit);
                labelInit.Text = Lang.DatabaseError;
                Utils.reportStatus(ref labelStatus, Color.Red, "Ringtest.initializePage: Category 'Detektor' not found");
                return;
            }             

            List<Database.Identifiers> idList = new List<Database.Identifiers>();
            if (!Database.Device.select_identifiers_where_accountID_categoryID_status(account.ID, category.ID, "Ok", ref idList))
            {
                multiViewRingtest.SetActiveView(viewNoInit);
                labelInit.Text = Lang.RingtestNoDetectors1 + " " 
                    + account.Name + Lang.RingtestNoDetectors2 + " "
                    + configuration.RingtestAdminEmail + " " + Lang.RingtestNoDetectors3;                
                return;
            }

            ddDetector.Items.Add(new ListItem("---", Guid.Empty.ToString()));

            foreach (Database.Identifiers ids in idList)
                ddDetector.Items.Add(new ListItem(ids.Name, ids.ID.ToString()));                        
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, "Ringtest.initializePage: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }

        multiViewRingtest.SetActiveView(viewSelectDetector);                
    }

    protected void buttonRegister_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddRegister.Items.Count <= 0)
            {
                Utils.displayStatus(ref labelStatusRegister, Color.Red, Lang.You_need_min_one_contact_before_register);
                return;
            }

            if (ddRegister.SelectedValue == Guid.Empty.ToString())
            {
                Utils.displayStatus(ref labelStatusRegister, Color.Red, Lang.You_need_to_select_a_contact_before_register);
                return;
            }

            Database.Interface.open();            

            Database.Account account = new Database.Account();
            if (!account.select_all_where_ID(new Guid(hiddenAccountID.Value)))
            {
                Utils.reportStatus(ref labelStatusRegister, Color.Red, "Ringtest.buttonRegister_OnClick: account.select_all_where_ID failed");
                return;
            }

            if (!account.update_Int_by_ID("intLastRegistrationYear", DateTime.Now.Year))
            {
                Utils.reportStatus(ref labelStatusRegister, Color.Red, "Ringtest.buttonRegister_OnClick: account.update_Int_by_ID failed");
                return;
            }
            
            if(!account.update_String_by_ID("vchRingtestContact", ddRegister.SelectedItem.Text))
            {
                Utils.reportStatus(ref labelStatusRegister, Color.Red, "Ringtest.buttonRegister_OnClick: account.update_String_by_ID failed");
                return;
            }
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatusRegister, Color.Red, "Ringtest.buttonRegister_OnClick: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }

        initializePage();
    }

    protected void buttonSendMessage_OnClick(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["UseEmail"] != "yes")
        {
            Utils.displayStatus(ref labelStatusMessage, Color.Red, "Sending av epost er deaktivert");
            return;
        }

        if (String.IsNullOrEmpty(tbMessage.Text))
        {
            Utils.displayStatus(ref labelStatusMessage, Color.Red, Lang.Missing_fields);
            return;
        }

        try
        {
            Database.Interface.open();

            Database.Configuration configuration = new Database.Configuration();            
            if (!configuration.select_all_where_name("Default"))
            {
                Utils.displayStatus(ref labelStatusMessage, Color.Red, Lang.Configuration + " " + Lang.not_found);
                return;
            }          

            Database.Account account = new Database.Account();
            if (!account.select_all_where_ID(new Guid(hiddenAccountID.Value)))
            {
                Utils.reportStatus(ref labelStatusMessage, Color.Red, "Ringtest.buttonSendMessage_OnClick: account.select_all_where_ID failed");
                return;
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServer"]) && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["MailServerPort"]))
            {
                string mailTitle = "Forespørsel fra LORAKON konto " + account.Name + "(" + account.Email + ")";
                string mailBody = tbMessage.Text;                

                MailMessage mail = new MailMessage();
                mail.To.Add(configuration.RingtestAdminEmail);
                mail.From = new MailAddress(configuration.RingtestAdminEmail);
                mail.BodyEncoding = System.Text.Encoding.Default;
                mail.IsBodyHtml = true;
                mail.Subject = mailTitle;
                mail.Body = mailBody;                
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["MailServer"], Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);

                Utils.displayStatus(ref labelStatusMessage, Color.SeaGreen, Lang.Message_sent);
            }
            else
            {
                Utils.displayStatus(ref labelStatusMessage, Color.Red, Lang.Email_no_server);
            }

            tbMessage.Text = "";            
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusMessage, Color.Red, "Ringtest.buttonSendMessage_OnClick: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void buttonDetectorContinue_OnClick(object sender, EventArgs e)
    {
        if (ddDetector.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusSelectDetector, Color.Red, Lang.NoDetectorSelected);
            return;
        }        

        try
        {
            Database.Ringtest ringtest = new Database.Ringtest();
            Database.RingtestReport report = new Database.RingtestReport();
            Database.RingtestBox box = new Database.RingtestBox();

            Database.Interface.open();
            
            if (!ringtest.select_all_where_year(DateTime.Now.Year))
            {
                Utils.reportStatus(ref labelStatusSelectDetector, Color.Red, "Ringtest.buttonDetectorContinue_OnClick: No ringtest found for year " + DateTime.Now.Year.ToString());
                return;
            }            

            if (!report.select_all_where_accountID_detectorID_ringtestID(
                new Guid(hiddenAccountID.Value), 
                new Guid(ddDetector.SelectedValue), 
                ringtest.ID))
            {
                report.AccountID = new Guid(hiddenAccountID.Value);
                report.DetectorID = new Guid(ddDetector.SelectedValue);
                report.RingtestID = ringtest.ID;
                report.RingtestBoxID = new Guid(hiddenRingtestBoxID.Value);
                report.AnswerByEmail = false;                
                report.MeasureDate = SqlDateTime.MinValue.Value;                
                report.RefDate = SqlDateTime.MinValue.Value;

                if (!report.insert_with_ID(Guid.NewGuid()))
                {
                    Utils.reportStatus(ref labelStatusSelectDetector, Color.Red, "Ringtest.buttonDetectorContinue_OnClick: Insert new report failed");
                    return;
                }
            }

            if (!box.select_all_where_ID(report.RingtestBoxID))
            {
                Utils.reportStatus(ref labelStatusSelectDetector, Color.Red, "Ringtest.buttonDetectorContinue_OnClick: Ringtestbox not found");
                return;
            }

            hiddenRingtestReportID.Value = report.ID.ToString();

            multiViewRingtest.SetActiveView(viewRingtestReport);

            Database.Device detector = new Database.Device();
            if (!detector.select_all_where_ID(report.DetectorID))
            {
                Utils.reportStatus(ref labelStatusReport, Color.Red, "Ringtest.buttonDetectorContinue_OnClick: Detector not found");
                return;
            }
            labelReportDetector.Text = detector.SerialNumber;

            if (report.ContactID == Guid.Empty)
                ddCurrentActor.SelectedIndex = -1;
            else ddCurrentActor.SelectedValue = report.ContactID.ToString();
            ddMCAType.SelectedValue = report.MCAType;
            labelBoxWeight.Text = Utils.roundOff(box.Weight, 1) + "g (" + box.KNumber + ")";            
            //calMeasureDate.SelectedDate = report.MeasureDate;
            tbBackground.Text = Utils.roundOff(report.Background, 2);
            tbIntegralBackground.Text = Utils.intToString(report.IntegralBackground);            
            tbCountingBackground.Text = Utils.intToString(report.CountingBackground);            
            tbGeometryFactor.Text = Utils.roundOff(report.GeometryFactor, 2);
            tbActivity.Text = Utils.roundOff(report.Activity, 1);
            tbActivityRef.Text = Utils.roundOff(report.ActivityRef, 1);
            tbUncertainty.Text = Utils.roundOff(report.Uncertainty, 1);
            tbAvgIntegralSample.Text = Utils.roundOff(report.AvgIntegralSample, 1);
            tbAvgLivetimeSample.Text = Utils.roundOff(report.AvgLivetimeSample, 1);            
            //calRefDate.SelectedDate = report.RefDate;
            cbWantEvaluation.Checked = report.WantEvaluation;
            cbAnswerByEmail.Checked = report.AnswerByEmail;
            cbEvaluated.Checked = report.Evaluated;
            cbApproved.Checked = report.Approved;                        
            gridComments.DataSource = Database.RingtestReport.get_comments(report.ID);
            gridComments.DataBind();

            if (report.MeasureDate != SqlDateTime.MinValue.Value)
            {              
                calMeasureDate.SelectedDate = report.MeasureDate;
            }

            if (report.RefDate != SqlDateTime.MinValue.Value)
            {
                calRefDate.SelectedDate = report.RefDate;
            }

            if (report.Approved)
            {
                ddCurrentActor.Enabled = false;
                ddMCAType.Enabled = false;
                tbMeasureDate.Enabled = false;
                tbBackground.ReadOnly = true;
                tbIntegralBackground.ReadOnly = true;
                tbCountingBackground.ReadOnly = true;
                tbGeometryFactor.ReadOnly = true;
                tbActivity.ReadOnly = true;
                tbActivityRef.ReadOnly = true;
                tbUncertainty.ReadOnly = true;
                tbAvgIntegralSample.ReadOnly = true;
                tbAvgLivetimeSample.ReadOnly = true;
                tbRefDate.Enabled = false;
                cbWantEvaluation.Enabled = false;
                cbAnswerByEmail.Enabled = false;
                buttonRingtestReportUpdate.Enabled = false;
            }
            else
            {
                ddCurrentActor.Enabled = true;
                ddMCAType.Enabled = true;
                tbMeasureDate.Enabled = true;
                tbBackground.ReadOnly = false;
                tbIntegralBackground.ReadOnly = false;
                tbCountingBackground.ReadOnly = false;
                tbGeometryFactor.ReadOnly = false;
                tbActivity.ReadOnly = false;
                tbActivityRef.ReadOnly = false;
                tbUncertainty.ReadOnly = false;
                tbAvgIntegralSample.ReadOnly = false;
                tbAvgLivetimeSample.ReadOnly = false;
                tbRefDate.Enabled = true;
                cbWantEvaluation.Enabled = true;
                cbAnswerByEmail.Enabled = true;
                buttonRingtestReportUpdate.Enabled = true;                
            }

            if (ddMCAType.SelectedValue == "Inspector1000")
            {
                tbIntegralBackground.Visible = false;
                labelIntegralBackground.Visible = false;
                tbCountingBackground.Visible = false;
                labelCountingBackground.Visible = false;
                tbBackground.Visible = false;
                labelBackground.Visible = false;
                tbAvgIntegralSample.Visible = false;
                labelAvgIntegralSample.Visible = false;
                tbActivity.Visible = false;
                labelActivity.Visible = false;
                labelGeometryFactor.Text = Lang.Efficiency;
                labelUncertainty.Text = Lang.Uncertainty_insp1000;
            }
            else
            {
                tbIntegralBackground.Visible = true;
                labelIntegralBackground.Visible = true;
                tbCountingBackground.Visible = true;
                labelCountingBackground.Visible = true;
                tbBackground.Visible = true;
                labelBackground.Visible = true;
                tbAvgIntegralSample.Visible = true;
                labelAvgIntegralSample.Visible = true;
                tbActivity.Visible = true;
                labelActivity.Visible = true;
                labelGeometryFactor.Text = Lang.GeometryFactor;
                labelUncertainty.Text = Lang.Uncertainty_serie10;
            }
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusSelectDetector, Color.Red, "Ringtest.buttonDetectorContinue_OnClick: " + ex.Message);
            Utils.displayStatus(ref labelStatusReport, Color.Red, "Ringtest.buttonDetectorContinue_OnClick: " + ex.Message);            
        }
        finally
        {
            Database.Interface.close();
        }         
    }

    protected void ddMCAType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddMCAType.SelectedValue == "Inspector1000")
        {
            tbIntegralBackground.Visible = false;
            labelIntegralBackground.Visible = false;
            tbCountingBackground.Visible = false;
            labelCountingBackground.Visible = false;
            tbBackground.Visible = false;
            labelBackground.Visible = false;
            tbAvgIntegralSample.Visible = false;
            labelAvgIntegralSample.Visible = false;
            tbActivity.Visible = false;
            labelActivity.Visible = false;
            labelGeometryFactor.Text = Lang.Efficiency;
            labelUncertainty.Text = Lang.Uncertainty_insp1000;
        }
        else
        {
            tbIntegralBackground.Visible = true;
            labelIntegralBackground.Visible = true;
            tbCountingBackground.Visible = true;
            labelCountingBackground.Visible = true;
            tbBackground.Visible = true;
            labelBackground.Visible = true;
            tbAvgIntegralSample.Visible = true;
            labelAvgIntegralSample.Visible = true;
            tbActivity.Visible = true;
            labelActivity.Visible = true;
            labelGeometryFactor.Text = Lang.GeometryFactor;
            labelUncertainty.Text = Lang.Uncertainty_serie10;
        }        
    }
    
    protected void ddDetector_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        labelStatusSelectDetector.Text = "";

        buttonDetectorContinue.Enabled = true;
        if (ddDetector.SelectedValue == Guid.Empty.ToString())
        {
            buttonDetectorContinue.Enabled = false;
            labelDetectorInfo.Text = "";            
            return;
        }        

        try
        {
            Database.Interface.open();
            Database.RingtestReport report = new Database.RingtestReport();
            if (report.select_all_where_accountID_detectorID_ringtestID(
                new Guid(hiddenAccountID.Value), 
                new Guid(ddDetector.SelectedValue), 
                new Guid(hiddenRingtestID.Value)))
            {
                labelDetectorInfo.Text = Lang.Evaluated + ": " + (report.Evaluated ? "Ja" : "Nei");
                labelDetectorInfo.Text += "<br>Godkjent: " + (report.Approved ? "Ja" : "Nei");
                if (report.ContactID != Guid.Empty)
                {
                    Database.Contact contact = new Database.Contact();
                    contact.select_all_by_ID(report.ContactID);
                    labelDetectorInfo.Text += "<br>" + Lang.Contact + ": " + contact.Name + "   [" + (contact.Status == "Active" ? "Aktiv" : "Inaktiv") + "]";

                    if (contact.Status != "Active")
                    {
                        buttonDetectorContinue.Enabled = false;
                    }
                }
                else labelDetectorInfo.Text += "<br>" + Lang.Contact + ": " + Lang.None;
            }
            else
            {
                labelDetectorInfo.Text = Lang.NoReportForDetector + ".<br>" + Lang.PressContinueForNewReport;
            }         
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatusSelectDetector, Color.Red, "Ringtest.ddDetector_OnSelectedIndexChanged: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }        

    protected void ddCurrentActor_OnDataBound(object sender, EventArgs e)
    {
        ddCurrentActor.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));        
    }

    protected void buttonRingtestReportUpdate_OnClick(object sender, EventArgs e)
    {
        if (ddCurrentActor.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusReport, Color.Red, Lang.You_must_select_contact_for_this_report);
            return;
        }

        if (String.IsNullOrEmpty(tbMeasureDate.Text) || String.IsNullOrEmpty(tbRefDate.Text) || String.IsNullOrEmpty(tbActivityRef.Text))
        {
            Utils.displayStatus(ref labelStatusReport, Color.Red, Lang.Missing_fields);
            return;
        }

        DateTime measureDate = DateTime.Now;
        DateTime refDate = DateTime.Now;
        try
        {
            measureDate = Convert.ToDateTime(tbMeasureDate.Text);
            refDate = Convert.ToDateTime(tbRefDate.Text);
        }
        catch(Exception except)
        {
            Utils.displayStatus(ref labelStatusReport, Color.Red, Lang.Invalid_date_fields);
            return;
        }        

        try
        {
            Database.Interface.open();
            Database.RingtestReport report = new Database.RingtestReport();
            if (!report.select_all_where_ID(new Guid(hiddenRingtestReportID.Value)))
            {
                Utils.reportStatus(ref labelStatusReport, Color.Red, "Ringtest.buttonRingtestReportUpdate_OnClick: Report not found");
                return;
            }

            if (report.Approved)
            {
                Utils.displayStatus(ref labelStatusReport, Color.Red, Lang.Update_failed_report_approved);
                return;                
            }

            if (cbWantEvaluation.Checked)
            {
                if (ddMCAType.SelectedValue == "Inspector1000")
                {
                    if (ddCurrentActor.SelectedIndex == -1 || String.IsNullOrEmpty(tbGeometryFactor.Text) || String.IsNullOrEmpty(tbActivityRef.Text) 
                        || String.IsNullOrEmpty(tbAvgLivetimeSample.Text) || String.IsNullOrEmpty(tbUncertainty.Text))
                    {
                        Utils.displayStatus(ref labelStatusReport, Color.Red, Lang.Cannot_evaluate_missing_fields);
                        return;
                    }
                }
                else
                {
                    if (ddCurrentActor.SelectedIndex == -1 || String.IsNullOrEmpty(tbBackground.Text) || String.IsNullOrEmpty(tbIntegralBackground.Text) ||
                    String.IsNullOrEmpty(tbCountingBackground.Text) || String.IsNullOrEmpty(tbGeometryFactor.Text) || String.IsNullOrEmpty(tbActivity.Text) ||
                    String.IsNullOrEmpty(tbActivityRef.Text) || String.IsNullOrEmpty(tbAvgIntegralSample.Text) || String.IsNullOrEmpty(tbAvgLivetimeSample.Text))
                    {
                        Utils.displayStatus(ref labelStatusReport, Color.Red, Lang.Cannot_evaluate_missing_fields);
                        return;
                    }
                }                                
            }

            try
            {
                if (ddCurrentActor.SelectedIndex == -1)
                    report.ContactID = Guid.Empty;
                else report.ContactID = new Guid(ddCurrentActor.SelectedValue);
                report.MCAType = ddMCAType.SelectedValue;
                report.Background = String.IsNullOrEmpty(tbBackground.Text) ? float.MinValue : Convert.ToSingle(tbBackground.Text);
                report.IntegralBackground = String.IsNullOrEmpty(tbIntegralBackground.Text) ? int.MinValue : Convert.ToInt32(tbIntegralBackground.Text);
                report.CountingBackground = String.IsNullOrEmpty(tbCountingBackground.Text) ? int.MinValue : Convert.ToInt32(tbCountingBackground.Text);
                report.GeometryFactor = String.IsNullOrEmpty(tbGeometryFactor.Text) ? float.MinValue : Convert.ToSingle(tbGeometryFactor.Text);
                report.Activity = String.IsNullOrEmpty(tbActivity.Text) ? float.MinValue : Convert.ToSingle(tbActivity.Text);
                report.ActivityRef = String.IsNullOrEmpty(tbActivityRef.Text) ? float.MinValue : Convert.ToSingle(tbActivityRef.Text);
                report.Uncertainty = String.IsNullOrEmpty(tbUncertainty.Text) ? float.MinValue : Convert.ToSingle(tbUncertainty.Text);
                report.AvgIntegralSample = String.IsNullOrEmpty(tbAvgIntegralSample.Text) ? float.MinValue : Convert.ToSingle(tbAvgIntegralSample.Text);
                report.AvgLivetimeSample = String.IsNullOrEmpty(tbAvgLivetimeSample.Text) ? float.MinValue : Convert.ToSingle(tbAvgLivetimeSample.Text);
                report.RefDate = refDate;
                calRefDate.SelectedDate = report.RefDate;
                report.MeasureDate = measureDate;
                calMeasureDate.SelectedDate = report.MeasureDate;
                report.WantEvaluation = cbWantEvaluation.Checked;
                report.AnswerByEmail = cbAnswerByEmail.Checked;                
            }
            catch(Exception except2)
            {
                Utils.displayStatus(ref labelStatusReport, Color.Red, Lang.Invalid_fields);
                return;                                                                        
            }

            if (!report.update_all_by_ID())
            {
                Utils.reportStatus(ref labelStatusReport, Color.Red, "Ringtest.buttonRingtestReportUpdate_OnClick: Update report failed");
                return;
            }

            Utils.displayStatus(ref labelStatusReport, Color.SeaGreen, Lang.Report_updated);            
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusReport, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }

        //ddDetector_OnSelectedIndexChanged(sender, e);
    }

    protected void buttonAddComment_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(tbComment.Text))
        {
            Utils.displayStatus(ref labelStatusReport, Color.Red, "Du må skrive inn en kommentar først");
            return;
        }

        try
        {
            Database.Interface.open();
            Database.RingtestReport report = new Database.RingtestReport();
            if (!report.select_all_where_ID(new Guid(hiddenRingtestReportID.Value)))
            {
                Utils.reportStatus(ref labelStatusReport, Color.Red, "Ringtest.buttonAddComment_OnClick: Report not found");
                return;
            }
            
            report.add_comment(ddCurrentActor.SelectedItem.Text, tbComment.Text);
            gridComments.DataSource = Database.RingtestReport.get_comments(report.ID);
            gridComments.DataBind();
            tbComment.Text = "";
            Utils.displayStatus(ref labelStatusReport, Color.SeaGreen, "Kommentar oppdatert");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusReport, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void buttonRingtestReportCancel_OnClick(object sender, EventArgs e)
    {
        multiViewRingtest.SetActiveView(viewSelectDetector);
    }

    protected void graphResults_OnRenderGraph(ZedGraph.Web.ZedGraphWeb z, System.Drawing.Graphics g, ZedGraph.MasterPane masterPane)
    {
        if (hiddenAccountID.Value == null || hiddenAccountID.Value == Guid.Empty.ToString())        
            return;        

        DataSet dataSet = null;
        GraphPane myPane = masterPane[0];

        myPane.Border.Color = Color.White;
        myPane.Title.Text = Lang.OurRingtestResults;;
        myPane.XAxis.Title.Text = Lang.Years;
        myPane.YAxis.Title.Text = "%"; // Lang.ErrorPercent;

        Database.RingtestBox box = new Database.RingtestBox();

        try
        {
            Database.Interface.open();
            dataSet = Database.RingtestReport.select_Year_Error_CalculatedUncertainty_RingtestBoxID_MCAType_ActivityRef_where_AccountID(new Guid(hiddenAccountID.Value));            
        
            PointPairList list = new PointPairList();
            PointPairList eList = new PointPairList();
            PointPairList bList = new PointPairList();

            double maxYear = DateTime.Now.Year;
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                Guid boxID = (Guid)dataSet.Tables[0].Rows[i][3];
                box.select_all_where_ID(boxID);                        

                double year = Convert.ToDouble(dataSet.Tables[0].Rows[i][0]);
                double error = Convert.ToDouble(dataSet.Tables[0].Rows[i][1]);
                double uncertainty = Convert.ToDouble(dataSet.Tables[0].Rows[i][2]);
                string MCAType = dataSet.Tables[0].Rows[i][4].ToString();
                if (MCAType.ToLower() != "serie10")
                {
                    double activity = Convert.ToDouble(dataSet.Tables[0].Rows[i][5]);
                    uncertainty = (uncertainty / activity) * 100.0;
                }

                if(year > maxYear)
                    maxYear = year;

                list.Add(year, error);
                eList.Add(year, error + uncertainty, error - uncertainty);                                        
                bList.Add(year, box.Uncertainty, -box.Uncertainty);                
            }

            myPane.YAxis.Scale.Max = 11.0f;
            myPane.YAxis.Scale.Min = -11.0f;
            myPane.XAxis.Scale.Max = maxYear + 1;
            myPane.XAxis.Scale.Min = maxYear - 10;
            myPane.XAxis.Scale.MinorStep = 1.0;
            myPane.XAxis.Scale.MajorStep = 1.0;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;        
            myPane.Legend.IsVisible = true;        

            LineItem curve = myPane.AddCurve(Lang.Reported_result, list, Color.White, SymbolType.Circle);
            curve.Line.IsVisible = false;
            curve.Symbol.Fill = new Fill(Color.IndianRed);
            curve.Symbol.Size = 6;

            ErrorBarItem errBar = myPane.AddErrorBar(Lang.CalculatedUncertainty, eList, Color.Red);
            errBar.Bar.PenWidth = 1;

            ErrorBarItem boxBar = myPane.AddErrorBar(Lang.RingtestBoxUncertainty, bList, Color.LightGray);
            boxBar.Bar.PenWidth = 12;
            boxBar.Bar.Symbol.IsVisible = false;            
            
            myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, Color.White), 45.0F);                

            const double offset = 0.1;
                
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                PointPair pt = curve.Points[i];

                if (pt.Y >= myPane.YAxis.Scale.Min && pt.Y <= myPane.YAxis.Scale.Max)
                {                
                    TextObj text = new TextObj(pt.Y.ToString("f2"), pt.X + offset, pt.Y, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                    text.FontSpec.Size = 6;
                    text.ZOrder = ZOrder.A_InFront;                
                    text.FontSpec.Border.IsVisible = false;
                    text.FontSpec.Fill.IsVisible = false;                
                    //text.FontSpec.Angle = 90;
                    myPane.GraphObjList.Add(text);

                    //g.DrawLine(pen, new Point((int)(pt.X + 10), (int)pt.Y), new Point((int)(pt.X - 10), (int)pt.Y));                
                }            
            }        

            myPane.YAxis.Scale.MaxGrace = 0.2;
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, "Ringtest.graphResults_OnRenderGraph: " + ex.Message);
            return;
        }
        finally
        {
            Database.Interface.close();
        }
        
        masterPane.AxisChange(g);
    }

    protected void graphStatistics_OnRenderGraph(ZedGraph.Web.ZedGraphWeb z, System.Drawing.Graphics g, ZedGraph.MasterPane masterPane)
    {                
        GraphPane myPane = masterPane[0];        

        myPane.Border.Color = Color.White;
        myPane.Title.Text = Lang.Statistics_for_all_members;
        myPane.XAxis.Title.Text = ""; //Lang.Participants;
        myPane.XAxis.Scale.IsVisible = false;
        myPane.YAxis.Title.Text = "%"; // Lang.ErrorPercent;

        myPane.YAxis.Scale.Max = 11.0f;
        myPane.YAxis.Scale.Min = -11.0f;
        
        myPane.Legend.IsVisible = true;
        myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, Color.White), 45.0F);
        myPane.YAxis.Scale.MaxGrace = 0.2;
        myPane.YAxis.MajorGrid.IsVisible = true;
        myPane.YAxis.MinorGrid.IsVisible = true;

        if (String.IsNullOrEmpty(ddStatistics.SelectedValue))
        {
            myPane.Title.Text = Lang.Statistics_for_all_members_no_results;
            myPane.XAxis.Scale.Max = 1;
            myPane.XAxis.Scale.Min = 0;
            return;
        }

        List<Guid> idList = new List<Guid>();

        try
        {
            Database.Interface.open();
            Database.Account.select_ID(ref idList, Convert.ToInt32(ddStatistics.SelectedValue));            

            PointPairList list = new PointPairList();
            PointPairList local_list = new PointPairList();
            PointPairList eList = new PointPairList();

            int count = 0;

            foreach (Guid id in idList)
            {                
                DataSet dataSet = Database.RingtestReport.select_Error_CalculatedUncertainty_RingtestBoxID_MCAType_ActivityRef_where_AccountID_Year(id, Convert.ToInt32(ddStatistics.SelectedValue));
                if (dataSet.Tables[0].Rows.Count <= 0)
                    continue;                

                double count_offset = (double)count;                
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
                    
                    if(id.ToString() == hiddenAccountID.Value) 
                        local_list.Add(count, error);
                    else list.Add(count, error);

                    eList.Add(count_offset, error + uncertainty, error - uncertainty);                                        
                }                

                count++;                
            }

            if (count < 5)
            {
                myPane.CurveList.Clear();
                Utils.displayStatus(ref labelStatusStatistics, Color.SeaGreen, Lang.Number_of_results_registered_is + " " + count.ToString() + ". " + Lang.Need_5_to_display_data);
            }
            else
            {
                myPane.XAxis.Scale.Max = count;
                myPane.XAxis.Scale.Min = -1;

                LineItem curve = myPane.AddCurve("Andres rapporterte resultater", list, Color.White, SymbolType.Circle);
                curve.Line.IsVisible = false;
                curve.Symbol.Size = 6;
                curve.Symbol.Fill = new Fill(Color.DodgerBlue);

                LineItem local_curve = myPane.AddCurve("Egne rapporterte resultater", local_list, Color.White, SymbolType.Circle);
                local_curve.Line.IsVisible = false;
                local_curve.Symbol.Size = 6;
                local_curve.Symbol.Fill = new Fill(Color.IndianRed);

                ErrorBarItem errBar = myPane.AddErrorBar("Beregent usikkerhet", eList, Color.Red);
                errBar.Bar.PenWidth = 1;                
            }
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusStatistics, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }

        masterPane.AxisChange(g);             
    }

    protected void ddStatistics_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // graphStatistics will be redrawn due to this callback
    }
}
