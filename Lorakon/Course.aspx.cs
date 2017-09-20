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

using Lang = Resources.Localization;

public partial class Course : BasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);            
            hiddenAccountID.Value = User.ProviderUserKey.ToString();

            if (gridCourse.Rows.Count <= 0)
                labelNoCouses.Text = Resources.Localization.Course_header_nocorses;
            else labelNoCouses.Text = "";
        }        
    }

    protected void clearAllActorBits()
    {
        for (int i = 0; i < gridCourseActors.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)gridCourseActors.Rows[i].FindControl("cbCourseActorBit");
            cb.Checked = false;            
        }
    }

    protected void gridCourse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        labelStatus.Text = "";
        gridCourseActors.Visible = true;

        clearAllActorBits();

        Guid courseID = (Guid)gridCourse.SelectedDataKey.Value;

        try
        {
            Database.Interface.open();
            List<Guid> guidList = new List<Guid>();
            if (!Database.Contact.select_ID_from_courseID(courseID, ref guidList))
            {
                Utils.reportStatus(ref labelStatus, Color.Red, "Course.gridCourse_OnSelectedIndexChanged: select_ID_from_courseID failed");
                return;
            }

            foreach (Guid id in guidList)
            {
                for (int i = 0; i < gridCourseActors.Rows.Count; i++)
                {
                    Guid gridContactID = (Guid)gridCourseActors.DataKeys[i][0];

                    if (id == gridContactID)
                    {
                        CheckBox cb = (CheckBox)gridCourseActors.Rows[i].FindControl("cbCourseActorBit");
                        cb.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, "Course.gridCourse_OnSelectedIndexChanged: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }                
    }

    protected void gridCourse_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';this.style.color='#FF0000';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.color='#222222';";

            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gridCourse, "Select$" + e.Row.RowIndex.ToString());            
        }        
    }        

    protected void cbCourseActorBit_OnCheckChanged(object sender, EventArgs e)
    {
        if (gridCourse.SelectedDataKey == null)
        {
            Utils.displayStatus(ref labelStatus, Color.Red, Lang.No_courses_selected);
            clearAllActorBits();
            return;
        }

        Guid courseID = (Guid)gridCourse.SelectedDataKey.Value;

        try
        {
            Database.Interface.open();

            for (int i = 0; i < gridCourseActors.Rows.Count; i++)
            {
                Database.Contact contact = new Database.Contact();
                contact.select_all_by_ID((Guid)gridCourseActors.DataKeys[i][0]);
                CheckBox cb = (CheckBox)gridCourseActors.Rows[i].FindControl("cbCourseActorBit");

                if (cb.Checked)                
                    contact.link_with_courseID(courseID);                                    
                else                
                    contact.unlink_with_courseID(courseID);                                

                Utils.displayStatus(ref labelStatus, Color.SeaGreen, Resources.Localization.StatusCourseUpdated);
            }            
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, "Course.cbCourseActorBit_OnCheckChanged: " + ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }
}
