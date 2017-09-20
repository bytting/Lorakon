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

public partial class Course : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");

        if (!Page.IsPostBack)
        {            
            if (!HttpContext.Current.User.IsInRole("Administrator"))
            {
                tabCreate.Enabled = false;                
            }
            
            if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Moderator"))            
            {
                tabEdit.Enabled = false;                
            }            
        }        

        tbEmailAddresses.Text = "";
        tbEmailAddresses.Visible = false;

        labelStatusCreate.Text = "";
        labelStatusEdit.Text = "";        
    }

    protected void buttonAddCourse_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(textBoxCourseTitle.Text))
        {            
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Mangler informasjon");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.Course course = new Database.Course(textBoxCourseTitle.Text, textBoxCourseDescription.Text, textBoxCourseComment.Text, false);
            if (!course.insert_with_ID(Guid.NewGuid()))
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Oppretting av kurs feilet");                
                return;
            }

            Utils.displayStatus(ref labelStatusCreate, Color.SeaGreen, "Kurs '" + textBoxCourseTitle.Text + "' opprettet");

            textBoxCourseTitle.Text = "";
            textBoxCourseDescription.Text = "";
            textBoxCourseComment.Text = "";

            ddCourses.DataBind();
            gridShowCourses.DataBind();
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

    protected void ddCourses_OnDataBound(object sender, EventArgs e)
    {
        ddCourses.Items.Insert(0, new ListItem("---", Guid.Empty.ToString()));
    }

    protected void cbCompletedOnly_OnCheckChanged(object sender, EventArgs e)
    {
        textBoxCourseTitleUpd.Text = "";
        textBoxCourseDescriptionUpd.Text = "";
        textBoxCourseCommentUpd.Text = "";
        checkBoxCourseCompleteUpd.Checked = false;
        lbMembers.Items.Clear();

        ddCourses.DataBind();
    }

    protected void ddCourses_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddCourses.SelectedValue == Guid.Empty.ToString())
        {
            textBoxCourseTitleUpd.Text = "";
            textBoxCourseDescriptionUpd.Text = "";
            textBoxCourseCommentUpd.Text = "";
            checkBoxCourseCompleteUpd.Checked = false;
            lbMembers.Items.Clear();
            return;
        }

        try
        {
            Database.Interface.open();

            Database.Course course = new Database.Course();
            if (!course.select_all_where_ID(new Guid(ddCourses.SelectedValue)))
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Kurs '" + ddCourses.SelectedItem.Text + "' ikke funnet");
                return;
            }

            textBoxCourseTitleUpd.Text = course.Title;
            textBoxCourseDescriptionUpd.Text = course.Description;
            textBoxCourseCommentUpd.Text = course.Comment;
            checkBoxCourseCompleteUpd.Checked = course.Completed;

            Database.Contact contact = new Database.Contact();
            Database.Account account = new Database.Account();
            string accountName = "";            
            List<Guid> idList = new List<Guid>();
            Database.Contact.select_ID_from_courseID(new Guid(ddCourses.SelectedValue), ref idList);
            lbMembers.Items.Clear();
            foreach (Guid id in idList)
            {
                contact.select_all_by_ID(id);
                account.select_String_where_ID(contact.AccountID, "vchName", ref accountName);                
                lbMembers.Items.Add(new ListItem(contact.Name + " fra " + accountName + " [" + (contact.Status == "Active" ? "Aktiv" : "Inaktiv") + "]"));
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

    protected void buttonUpdateCourse_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(textBoxCourseTitleUpd.Text) || ddCourses.SelectedValue == Guid.Empty.ToString())
        {            
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Mangler informasjon");      
            return;
        }

        try
        {
            Database.Interface.open();

            Database.Course course = new Database.Course();
            if (!course.select_all_where_ID(new Guid(ddCourses.SelectedValue)))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kurs '" + ddCourses.SelectedItem.Text + "' ikke funnet");                
                return;
            }

            course.Title = textBoxCourseTitleUpd.Text;
            course.Description = textBoxCourseDescriptionUpd.Text;
            course.Comment = textBoxCourseCommentUpd.Text;
            course.Completed = checkBoxCourseCompleteUpd.Checked;

            if (!course.update_all_by_ID())
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Oppdatering av kurs feilet");                
                return;
            }

            Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Kurs '" + textBoxCourseTitleUpd.Text + "' oppdatert");

            textBoxCourseTitleUpd.Text = "";
            textBoxCourseDescriptionUpd.Text = "";
            textBoxCourseCommentUpd.Text = "";
            checkBoxCourseCompleteUpd.Checked = false;
            lbMembers.Items.Clear();

            ddCourses.DataBind();
            gridShowCourses.DataBind();
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

    protected void buttonGenerateEmailAddresses_OnClick(object sender, EventArgs e)
    {
        if (ddCourses.SelectedValue == Guid.Empty.ToString())
        {            
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Du må velge kurs først");      
            return;
        }

        try
        {
            List<Guid> contactList = new List<Guid>();
            Database.Interface.open();
            Database.Contact.select_ID_from_courseID(new Guid(ddCourses.SelectedValue), ref contactList);
            string emails = Database.Contact.select_EmailAddresses(contactList);

            tbEmailAddresses.Visible = true;
            tbEmailAddresses.Text = emails;
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

    protected void cbIncludeCompletedCourses_OnCheckChanged(object sender, EventArgs e)
    {
        if (cbIncludeCompletedCourses.Checked)
            dataSourceShowCourses.SelectCommand = "SELECT ID, vchTitle, textComment, bitCompleted FROM Course ORDER BY vchTitle ASC";
        else dataSourceShowCourses.SelectCommand = "SELECT ID, vchTitle, textComment, bitCompleted FROM Course WHERE bitCompleted = 0 ORDER BY vchTitle";

        textBoxCourseTitle.Text = "";
        textBoxCourseComment.Text = "";
        textBoxCourseDescription.Text = "";
        lbMembers.Items.Clear();

        gridShowCourses.DataBind();
    }

    /*
    protected void buttonDeleteCourse_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(textBoxCourseTitleUpd.Text) || ddCourses.SelectedValue == Guid.Empty.ToString())
        {            
            Utils.displayStatus(ref labelStatus, Color.Red, "Mangler informasjon");
            return;
        }

        Database.Interface.open();

        Database.Course course = new Database.Course();
        course.select_all_where_ID(new Guid(ddCourses.SelectedValue));
        course.delete_links_by_ID();
        course.delete_by_ID();        

        Database.Interface.close();        

        textBoxCourseTitleUpd.Text = "";
        textBoxCourseDescriptionUpd.Text = "";
        textBoxCourseCommentUpd.Text = "";
        checkBoxCourseCompleteUpd.Checked = false;        

        Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Kurs '" + ddCourses.SelectedItem.Text + "' slettet");

        ddCourses.DataBind();
    }
    */    
}
