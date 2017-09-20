using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Units : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");

        if (!Page.IsPostBack)
        {
            tbNewCategoryFilter.ValidChars += "\"";
            tbNewTypeFilter.ValidChars += "\"";

            if (!HttpContext.Current.User.IsInRole("Administrator"))
            {
                tabCategories.Enabled = false;                
                tabCreate.Enabled = false;                
            }

            if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Moderator"))
            {
                tabEdit.Enabled = false;                
            }
        }                
    }

    protected void updateCategories()
    {        
        ddCategories.DataBind();
        ddCategoriesCD.DataBind();        
        ddCategoriesED.DataBind();
        gridCategories.DataBind();        
    }

    protected void updateTypes()
    {        
        ddTypesCD.DataBind();
        ddTypesED.DataBind();
        gridTypes.DataBind();        
    }

    protected void updateBoxes()
    {
        gridBoxes.DataBind();
    }

    protected void buttonNewCategory_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(tbNewCategory.Text))
        {            
            Utils.displayStatus(ref labelStatusCategories, Color.Red, "Feltet 'Kategori navn' må fylles inn");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.DeviceCategory category = new Database.DeviceCategory(tbNewCategory.Text, false);
            if (category.insert_with_ID_name(Guid.NewGuid(), tbNewCategory.Text))
                Utils.displayStatus(ref labelStatusCategories, Color.SeaGreen, "Enhets kategori '" + tbNewCategory.Text + "' opprettet");
            else
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Oppretting av enhets kategori feilet");

            tbNewCategory.Text = "";
            updateCategories();
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }        
    }

    protected void buttonGenerateSerialnumberCD_OnClick(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tbSerialnumberCD.Text))
        {            
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Feltet 'Serienummer' må være tomt");
            return;
        }

        try
        {
            Database.Interface.open();
            tbSerialnumberCD.Text = "NRPA-" + Database.Interface.getNextSerialnumber().ToString();
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

    protected void buttonGenerateSerialnumberED_OnClick(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tbSerialnumberED.Text))
        {            
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Feltet 'Serienummer' må være tomt");
            return;
        }

        try
        {
            Database.Interface.open();
            tbSerialnumberED.Text = "NRPA-" + Database.Interface.getNextSerialnumber().ToString();         
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

    protected void gridCategories_OnEditCommand(object sender, GridViewEditEventArgs e)
    {
        if (!HttpContext.Current.User.IsInRole("Administrator"))
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, "Du må være administrator for å redigere kategorier");
            e.Cancel = true;
            return;
        }
        gridCategories.EditIndex = e.NewEditIndex;        
    }

    protected void gridCategories_OnCancelCommand(object sender, GridViewCancelEditEventArgs e)
    {
        gridCategories.EditIndex = -1;        
    }

    protected void gridCategories_OnUpdateCommand(object sender, GridViewUpdateEventArgs e)
    {
        TextBox tbEditName = (TextBox)gridCategories.Rows[e.RowIndex].Cells[0].Controls[0];
        if (String.IsNullOrEmpty(tbEditName.Text))
        {                        
            Utils.displayStatus(ref labelStatusCategories, Color.Red, "Feltet 'Kategori navn' må fylles inn");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.DeviceCategory category = new Database.DeviceCategory();
            if (!category.select_all_where_ID((Guid)gridCategories.DataKeys[e.RowIndex].Values[0]))
            {
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Kategori ikke funnet");
                return;
            }

            category.Name = tbEditName.Text;
            category.update_all_by_ID();

            gridCategories.EditIndex = -1;
            updateCategories();

            Utils.displayStatus(ref labelStatusCategories, Color.SeaGreen, "Kategori '" + category.Name + "' oppdatert");
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }                
    }

    protected void gridCategories_OnDeleteCommand(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Database.Interface.open();

            Database.DeviceCategory deviceCategory = new Database.DeviceCategory();
            if (!deviceCategory.select_all_where_ID((Guid)gridCategories.DataKeys[e.RowIndex].Values[0]))
            {                
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Kategori ikke funnet");
                return;            
            }

            if (deviceCategory.Sticky)
            {
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Kategorien '" + deviceCategory.Name + "' kan ikke slettes");
                e.Cancel = true;                
                return;
            }
            else
            {
                // sjekk om det er typer tilordnet til denne kategorien
                List<Database.Device> deviceList = new List<Database.Device>();
                if (Database.Device.select_all_where_categoryID(deviceCategory.ID, "vchSerialNumber", ref deviceList))
                {
                    Utils.displayStatus(ref labelStatusCategories, Color.Red, "Kategorien '" + deviceCategory.Name + "' har " + deviceList.Count.ToString() + " enheter i systemet. Slett disse først");
                    e.Cancel = true;                    
                    return;
                }

                if (deviceCategory.delete_by_ID())
                    Utils.displayStatus(ref labelStatusCategories, Color.SeaGreen, "Kategorien '" + deviceCategory.Name + "' slettet");
                else
                    Utils.displayStatus(ref labelStatusCategories, Color.Red, "Sletting av kategori feilet");
            }         

            updateCategories();
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void ddCategories_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        gridTypes.DataBind();        
    }

    protected void buttonNewType_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(tbNewType.Text))
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, "Feltet 'Type navn' må fylles inn");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.DeviceCategoryType deviceType = new Database.DeviceCategoryType(tbNewType.Text, false);
            if (deviceType.insert_with_ID_categoryID_name(Guid.NewGuid(), new Guid(ddCategories.SelectedValue), tbNewType.Text))
                Utils.displayStatus(ref labelStatusCategories, Color.SeaGreen, "Enhets type opprettet");
            else
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Oppretting av enhets type feilet");         

            updateTypes();
            tbNewType.Text = "";
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void gridTypes_OnEditCommand(object sender, GridViewEditEventArgs e)
    {
        if (!HttpContext.Current.User.IsInRole("Administrator"))
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, "Du må være administrator for å redigere kategorityper");
            e.Cancel = true;
            return;
        }
        gridTypes.EditIndex = e.NewEditIndex;        
    }

    protected void gridTypes_OnCancelCommand(object sender, GridViewCancelEditEventArgs e)
    {
        gridTypes.EditIndex = -1;        
    }

    protected void gridTypes_OnUpdateCommand(object sender, GridViewUpdateEventArgs e)
    {
        TextBox tbEditName = (TextBox)gridTypes.Rows[e.RowIndex].Cells[0].Controls[0];
        if (String.IsNullOrEmpty(tbEditName.Text))
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, "Feltet 'Type navn' må fylles inn");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.DeviceCategoryType deviceType = new Database.DeviceCategoryType();
            if (!deviceType.select_all_where_ID((Guid)gridTypes.DataKeys[e.RowIndex].Values[0]))
            {
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Enhetstype ikke funnet");
                return;
            }

            deviceType.Name = tbEditName.Text;

            if (deviceType.update_all_by_ID())
                Utils.displayStatus(ref labelStatusCategories, Color.SeaGreen, "Enhetstype oppdatert");
            else
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Oppdatering av enhetstype feilet");         

            gridTypes.EditIndex = -1;
            updateTypes();
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void gridTypes_OnDeleteCommand(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Database.Interface.open();

            Database.DeviceCategoryType deviceType = new Database.DeviceCategoryType();
            if (!deviceType.select_all_where_ID((Guid)gridTypes.DataKeys[e.RowIndex].Values[0]))
            {
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Enhetstype ikke funnet");
                return;
            }

            if (deviceType.Sticky)
            {
                Utils.displayStatus(ref labelStatusCategories, Color.Red, "Kategori typen " + deviceType.Name + " kan ikke slettes");
                e.Cancel = true;
                Database.Interface.close();
                return;
            }
            else
            {
                // sjekk om det er typer tilordnet til denne typen
                List<Database.Device> deviceList = new List<Database.Device>();
                if (Database.Device.select_all_where_typeID(deviceType.ID, "vchSerialNumber", ref deviceList))
                {
                    Utils.displayStatus(ref labelStatusCategories, Color.Red, "Kategoritypen '" + deviceType.Name + "' har " + deviceList.Count.ToString() + " enheter i systemet. Slett disse først");
                    e.Cancel = true;                    
                    return;
                }

                if (deviceType.delete_by_ID())
                    Utils.displayStatus(ref labelStatusCategories, Color.SeaGreen, "Enhetstype slettet");
                else
                    Utils.displayStatus(ref labelStatusCategories, Color.Red, "Sletting av enhetstype feilet");
            }         

            updateTypes();
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void ddCategoriesCD_OnSelectedIndexChanged(object sender, EventArgs e)
    {        
        ddTypesCD.DataBind();        
    }

    protected void buttonCreateCD_OnClick(object sender, EventArgs e)
    {
        if (ddCategoriesCD.SelectedValue == Guid.Empty.ToString() 
            || ddTypesCD.SelectedValue == Guid.Empty.ToString() 
            || String.IsNullOrEmpty(ddStatusCD.Text) 
            || ddOwnershipCD.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Mangler informasjon");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.Device device = new Database.Device(
                new Guid(ddAccountCD.SelectedValue),
                new Guid(ddCategoriesCD.SelectedValue),
                new Guid(ddTypesCD.SelectedValue),
                tbSerialnumberCD.Text,
                ddStatusCD.SelectedValue,
                ddOwnershipCD.SelectedValue,
                "",
                String.IsNullOrEmpty(tbReceivedNewCD.Text) ? DateTime.MinValue : DateTime.Parse(tbReceivedNewCD.Text));

            if (device.insert_with_ID(Guid.NewGuid()))
                Utils.displayStatus(ref labelStatusCreate, Color.SeaGreen, "Enhet opprettet");
            else
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Oppretting av enhet feilet");         

            tbSerialnumberCD.Text = "";
            gridDevicesED.DataBind();
            gridDevicesAllED.DataBind();
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
    
    protected void ddCategoriesED_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategoriesEdit.SelectedValue = ddCategoriesED.SelectedValue;
        ddCategoriesEdit.DataBind();
        ddTypesED.DataBind();
        ddTypesEdit.DataBind();
        gridDevicesED.DataBind();
        panelDeviceED.Visible = false;
    }

    protected void ddTypesED_OnSelectedIndexChanged(object sender, EventArgs e)
    {        
        gridDevicesED.DataBind();
        panelDeviceED.Visible = false;
    }    

    protected void gridDevicesED_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Database.Interface.open();
            Database.Device device = new Database.Device();
            if (!device.select_all_where_ID((Guid)gridDevicesED.SelectedDataKey.Value))
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Enhet ikke funnet");
                return;
            }

            ddCategoriesEdit.SelectedValue = device.DeviceCategoryID.ToString();
            ddTypesEdit.SelectedValue = device.DeviceTypeID.ToString();
            ddAccountED.SelectedValue = device.AccountID.ToString();
            tbSerialnumberED.Text = device.SerialNumber;
            ddStatusED.SelectedValue = device.Status;
            ddOwnershipED.SelectedValue = device.Ownership;
            tbCommentED.Text = device.Comment;
            if(device.ReceivedNew != DateTime.MinValue)
                calReceivedNewED.SelectedDate = device.ReceivedNew;
            
            gridDeviceHistory.DataSource = device.select_all_comments();
            gridDeviceHistory.DataBind();

            Database.DeviceCategory cat = new Database.DeviceCategory();
            if (!cat.select_all_where_name("Detektor"))         
            {
                Utils.displayStatus(ref labelStatusEdit, Color.Red, "Enhet 'Detektor' ikke funnet");
                return;
            }

            if (ddCategoriesED.SelectedValue == cat.ID.ToString())
            {
                ddCategoriesEdit.Enabled = false;
                ddTypesEdit.Enabled = false;
            }
            else
            {
                ddCategoriesEdit.Enabled = true;
                ddTypesEdit.Enabled = true;
            }        
            panelDeviceED.Visible = true;
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

    protected void gridDevicesED_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';this.style.color='#FF0000';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.color='#222222';";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gridDevicesED, "Select$" + e.Row.RowIndex);            
        }
    }

    protected void gridDeviceHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {                 
        try
        {
            Database.Interface.open();
            Database.Device.delete_comment((Guid)gridDeviceHistory.DataKeys[e.RowIndex].Values[0]);

            Database.Device device = new Database.Device();
            device.select_all_where_ID((Guid)gridDevicesED.SelectedDataKey.Value);
            gridDeviceHistory.DataSource = device.select_all_comments();
            gridDeviceHistory.DataBind();
            gridDeviceInfo.DataSource = device.select_all_comments();
            gridDeviceInfo.DataBind();

            Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Historie kommentar slettet");
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

    protected void gridDevicesAllED_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {                        
            Database.Interface.open();
            Database.Device device = new Database.Device();
            if (!device.select_all_where_ID((Guid)gridDevicesAllED.SelectedDataKey.Value))
            {
                Utils.displayStatus(ref labelStatusAll, Color.Red, "Enhet ikke funnet");
                return;
            }

            labelDeviceInfo.Text = "Historie for enhet " + device.SerialNumber;
            gridDeviceInfo.DataSource = device.select_all_comments();            
            gridDeviceInfo.DataBind();

            panelDeviceInfo.Visible = true;
        }
        catch (Exception ex)
        {
            Utils.displayStatus(ref labelStatusAll, Color.Red, ex.Message);
        }
        finally
        {
            Database.Interface.close();
        }
    }

    protected void gridDevicesAllED_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';this.style.color='#FF0000';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.color='#222222';";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gridDevicesAllED, "Select$" + e.Row.RowIndex);
        }
    }

    protected void buttonUpdateED_OnClick(object sender, EventArgs e)
    {
        if (ddCategoriesED.SelectedValue == Guid.Empty.ToString() 
            || ddTypesED.SelectedValue == Guid.Empty.ToString() 
            || String.IsNullOrEmpty(ddStatusED.Text) 
            || ddOwnershipED.SelectedValue == Guid.Empty.ToString())
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Mangler informasjon");
            return;
        }                

        try
        {
            Database.Interface.open();

            Database.Device device = new Database.Device();
            device.select_all_where_ID((Guid)gridDevicesED.SelectedDataKey.Value);
            if (device.AccountID.ToString() != ddAccountED.SelectedValue)
            {
                Database.Ringtest ringtest = new Database.Ringtest();
                if(ringtest.select_all_where_year(DateTime.Now.Year))
                {
                    Database.RingtestReport report = new Database.RingtestReport();
                    if (report.select_all_where_ringtestID_AccountID_DetectorID_approved(ringtest.ID, device.AccountID, device.ID, false))
                    {
                        Utils.displayStatus(ref labelStatusEdit, Color.Red, "Oppdatering avbrutt. Kan ikke oppdatere innehaver fordi enheten har en aktiv ringtestrapport");
                        return;
                    }
                }
            }
            device.DeviceCategoryID = new Guid(ddCategoriesEdit.SelectedValue);
            device.DeviceTypeID = new Guid(ddTypesEdit.SelectedValue);
            device.AccountID = new Guid(ddAccountED.SelectedValue);
            device.SerialNumber = tbSerialnumberED.Text;
            device.Status = ddStatusED.SelectedValue;
            device.Ownership = ddOwnershipED.SelectedValue;
            device.Comment = tbCommentED.Text;
            device.ReceivedNew = String.IsNullOrEmpty(tbReceivedNewED.Text) ? DateTime.MinValue : DateTime.Parse(tbReceivedNewED.Text);

            if (device.update_by_ID())
                Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Enhet oppdatert");
            else
                Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Oppdatering av enhet feilet");         

            gridDevicesED.SelectedIndex = -1;
            gridDevicesED.DataBind();
            gridDevicesAllED.DataBind();
            panelDeviceED.Visible = false;
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

    protected void buttonCancelED_OnClick(object sender, EventArgs e)
    {
        ddAccountED.SelectedIndex = -1;
        tbSerialnumberED.Text = "";
        ddStatusED.SelectedIndex = -1;
        ddOwnershipED.SelectedIndex = -1;
        tbCommentED.Text = "";        

        gridDevicesED.SelectedIndex = -1;
        panelDeviceED.Visible = false;
    }

    protected void buttonDeviceInfoCancel_OnClick(object sender, EventArgs e)
    {
        gridDevicesAllED.SelectedIndex = -1;        
        panelDeviceInfo.Visible = false;
    }

    protected void buttonCreateBox_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(tbKNumber.Text) 
            || String.IsNullOrEmpty(tbExternID.Text)
            || String.IsNullOrEmpty(tbRefDate.Text)
            || String.IsNullOrEmpty(tbRefValue.Text)
            || String.IsNullOrEmpty(tbUncertainty.Text)
            || String.IsNullOrEmpty(tbWeight.Text))
        {            
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Mangler informasjon");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.RingtestBox box = new Database.RingtestBox(
                tbKNumber.Text,
                tbExternID.Text,
                DateTime.Parse(tbRefDate.Text),                
                Convert.ToSingle(tbRefValue.Text),
                Convert.ToSingle(tbUncertainty.Text),                
                Convert.ToSingle(tbWeight.Text),
                ddStatusAss.SelectedValue,
                tbComment.Text);

            if (box.insert_with_ID_KNumber(Guid.NewGuid(), tbKNumber.Text))
                Utils.displayStatus(ref labelStatusCreate, Color.SeaGreen, "Ringtestboks '" + tbKNumber.Text + "' opprettet");
            else
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Oppretting av ringtestboks feilet");         

            updateBoxes();

            tbKNumber.Text = "";
            tbExternID.Text = "";
            tbRefDate.Text = "";
            tbRefValue.Text = "";
            tbUncertainty.Text = "";        
            tbWeight.Text = "";
            ddStatusAss.SelectedIndex = 0;
            tbComment.Text = "";
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

    protected void gridBoxes_OnEditCommand(object sender, GridViewEditEventArgs e)
    {
        if (!HttpContext.Current.User.IsInRole("Administrator"))
        {
            Utils.displayStatus(ref labelStatusCategories, Color.Red, "Du må være administrator for å redigere ringtestbokser");
            e.Cancel = true;
            return;
        }

        gridBoxes.EditIndex = e.NewEditIndex;                
    }

    protected void gridBoxes_OnCancelCommand(object sender, GridViewCancelEditEventArgs e)
    {
        gridBoxes.EditIndex = -1;        
    }

    protected void gridBoxes_OnUpdateCommand(object sender, GridViewUpdateEventArgs e)
    {
        TextBox KNumber = (TextBox)gridBoxes.Rows[e.RowIndex].Cells[0].Controls[0];
        TextBox ExternID = (TextBox)gridBoxes.Rows[e.RowIndex].Cells[1].Controls[0];        
        TextBox RefDate = gridBoxes.Rows[e.RowIndex].Cells[2].FindControl("tbGridRefDate") as TextBox;
        TextBox RefValue = (TextBox)gridBoxes.Rows[e.RowIndex].Cells[3].FindControl("tbRefValue");
        TextBox Uncertainty = (TextBox)gridBoxes.Rows[e.RowIndex].Cells[4].FindControl("tbGridUncertainty");        
        TextBox Weight = (TextBox)gridBoxes.Rows[e.RowIndex].Cells[5].FindControl("tbWeight");        
        DropDownList Status = gridBoxes.Rows[e.RowIndex].Cells[6].FindControl("ddGridStatus") as DropDownList;
        TextBox Comment = (TextBox)gridBoxes.Rows[e.RowIndex].Cells[7].Controls[0];

        if (String.IsNullOrEmpty(KNumber.Text) 
            || String.IsNullOrEmpty(ExternID.Text) 
            || String.IsNullOrEmpty(RefDate.Text) 
            || String.IsNullOrEmpty(RefValue.Text)
            || String.IsNullOrEmpty(Uncertainty.Text)            
            || String.IsNullOrEmpty(Weight.Text))
        {
            Utils.displayStatus(ref labelStatusCreate, Color.Red, "Mangler informasjon");
            return;
        }

        try
        {
            Database.Interface.open();

            Database.RingtestBox box = new Database.RingtestBox();
            if (!box.select_all_where_ID((Guid)gridBoxes.DataKeys[e.RowIndex].Values[0]))
            {
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Ringtestboks ikke funnet");
                return;
            }

            box.KNumber = KNumber.Text;
            box.ExternID = ExternID.Text;
            box.RefDate = DateTime.Parse(RefDate.Text);
            box.RefValue = Convert.ToSingle(RefValue.Text);
            box.Uncertainty = Convert.ToSingle(Uncertainty.Text);            
            box.Weight = Convert.ToSingle(Weight.Text);
            box.Status = Status.SelectedValue;
            box.Comment = Comment.Text;

            if (box.update_all_by_ID())
                Utils.displayStatus(ref labelStatusCreate, Color.SeaGreen, "Ringtestboks oppdatert");
            else
                Utils.displayStatus(ref labelStatusCreate, Color.Red, "Oppdatering av ringtestboks feilet");         

            gridBoxes.EditIndex = -1;
            updateBoxes();
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

    protected void buttonAddComment_OnClick(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(tbAddComment.Text))
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Kommentarfeltet kan ikke være tomt");
            return;
        }

        try
        {
            Database.Interface.open();
            Database.Device.insert_comment((Guid)gridDevicesED.SelectedDataKey.Value, tbAddComment.Text);
            Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Historie kommentar lagt til");
            tbAddComment.Text = "";

            Database.Device device = new Database.Device();
            device.select_all_where_ID((Guid)gridDevicesED.SelectedDataKey.Value);            
            gridDeviceHistory.DataSource = device.select_all_comments();
            gridDeviceHistory.DataBind();
            gridDeviceInfo.DataSource = device.select_all_comments();
            gridDeviceInfo.DataBind();
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

    protected void buttonDeleteComment_OnClick(object sender, EventArgs e)
    {
        if (gridDevicesED.SelectedDataKey != null)
        {
            try
            {                
                Database.Interface.open();
                Database.Device.delete_comment((Guid)gridDeviceHistory.SelectedDataKey.Value);

                Database.Device device = new Database.Device();
                device.select_all_where_ID((Guid)gridDevicesED.SelectedDataKey.Value);                
                gridDeviceHistory.DataSource = device.select_all_comments();
                gridDeviceHistory.DataBind();
                gridDeviceInfo.DataSource = device.select_all_comments();
                gridDeviceInfo.DataBind();

                Utils.displayStatus(ref labelStatusEdit, Color.SeaGreen, "Historie kommentar slettet");
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
        else
        {
            Utils.displayStatus(ref labelStatusEdit, Color.Red, "Du må velge en kommentar for sletting først");
        }
    }

    /*
    protected void gridBoxes_OnDeleteCommand(object sender, GridViewDeleteEventArgs e)
    {
        Database.Interface.open();

        Database.RingtestBox box = new Database.RingtestBox();
        box.select_all_where_ID((Guid)gridBoxes.DataKeys[e.RowIndex].Values[0]);
        if(box.delete_by_ID())
            Utils.displayStatus(ref labelStatus, Color.SeaGreen, "Ringtest boks slettet");
        else 
            Utils.displayStatus(ref labelStatus, Color.Red, "Sletting av ringtest boks feilet");

        Database.Interface.close();

        updateBoxes();
    } 
     * */
}
