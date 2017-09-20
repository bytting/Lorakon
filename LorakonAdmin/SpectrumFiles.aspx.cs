using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SpectrumFiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);
        if (User == null)
            Response.Redirect("~/Login.aspx");

        if (!IsPostBack)
        {
            ddSpectrumYear.Items.Clear();
            ddSpectrumYear.Items.Add("");

            int year = DateTime.Now.Year;
            while (year > 2000)
            {
                ddSpectrumYear.Items.Add(year.ToString());
                year--;
            }

            BindImportRules();
            BindGeomRules();
        }
    }

    DataSet GetData(SqlCommand cmd)
    {
        DataSet ds = new DataSet();        
        cmd.Connection = new SqlConnection(Database.Interface.connectionString);
        cmd.Connection.Open();
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        adapter.Fill(ds);
        cmd.Connection.Close();
        return ds;
    }

    private void BindImportRules()
    {
        string strQuery = "select ID, NuclideName, ActivityMin, ActivityMax, ConfidenceMin, CanBeAutoApproved from SpectrumValidationRules";
        SqlCommand cmd = new SqlCommand(strQuery);
        gridViewImportRules.DataSource = GetData(cmd);
        gridViewImportRules.DataBind();
    }

    private void BindGeomRules()
    {
        string strQuery = "select ID, Geometry, Unit, Minimum, Maximum from SpectrumGeometryRules";
        SqlCommand cmd = new SqlCommand(strQuery);
        gridViewGeomRules.DataSource = GetData(cmd);
        gridViewGeomRules.DataBind();
    }

    protected void ddUsers_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string id = ddUsers.SelectedValue;
        string year = ddSpectrumYear.Text;

        if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(year))
            PopulateSpectrums(id, year);
        else PopulateSpectrums(Guid.Empty.ToString(), (-1).ToString());
    }

    protected void ddSpectrumYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = ddUsers.SelectedValue;
        string year = ddSpectrumYear.Text;

        if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(year))
            PopulateSpectrums(id, year);
        else PopulateSpectrums(Guid.Empty.ToString(), (-1).ToString());
    }

    private void PopulateSpectrums(string id, string year)
    {
        dataSourceSpectrums.SelectParameters.Clear();
        dataSourceSpectrums.SelectParameters.Add("AccountID", id);
        dataSourceSpectrums.SelectParameters.Add("year", year);

        gridSpectrums.DataSourceID = "dataSourceSpectrums";
        gridSpectrums.DataBind();
    }

    protected void gridSpectrums_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DownloadSpectrum")
        {            
            int index = Convert.ToInt32(e.CommandArgument);            
            string specId = gridSpectrums.DataKeys[index]["ID"].ToString();
            Response.Redirect("~/DownloadSpectrum.aspx?id=" + specId);            
        }
        else if (e.CommandName == "DownloadReport")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string specId = gridSpectrums.DataKeys[index]["ID"].ToString();            
            Response.Redirect("~/DownloadReport.aspx?id=" + specId);
        }
    }

    protected void AddFirstImportRule(object sender, EventArgs e)
    {
        labelStatusRules.Text = "";

        TextBox txtNuclideName = (TextBox)gridViewImportRules.Controls[0].Controls[0].FindControl("txtNuclideNameFirst");
        if (String.IsNullOrEmpty(txtNuclideName.Text))
        {
            labelStatusRules.Text = "Nuklide navn mangler";
            return;
        }

        double ActivityMin, ActivityMax, ConfidenceMin;

        TextBox txtActivityMinNew = (TextBox)gridViewImportRules.Controls[0].Controls[0].FindControl("txtActivityMinFirst");
        if (String.IsNullOrEmpty(txtActivityMinNew.Text))
        {
            labelStatusRules.Text = "Aktivitet min mangler";
            return;
        }

        TextBox txtActivityMaxNew = (TextBox)gridViewImportRules.Controls[0].Controls[0].FindControl("txtActivityMaxFirst");
        if (String.IsNullOrEmpty(txtActivityMaxNew.Text))
        {
            labelStatusRules.Text = "Aktivitet max mangler";
            return;
        }

        TextBox txtConfidenceMinNew = (TextBox)gridViewImportRules.Controls[0].Controls[0].FindControl("txtConfidenceMinFirst");
        if (String.IsNullOrEmpty(txtConfidenceMinNew.Text))
        {
            labelStatusRules.Text = "Konfidens min mangler";
            return;
        }

        try
        {
            ActivityMin = Convert.ToDouble(txtActivityMinNew.Text);
            ActivityMax = Convert.ToDouble(txtActivityMaxNew.Text);
            ConfidenceMin = Convert.ToDouble(txtConfidenceMinNew.Text);
        }
        catch (Exception ex)
        {
            labelStatusRules.Text = "Ugyldige tall funnet";
            return;
        }

        bool CanBeAutoApproved = Convert.ToBoolean(((CheckBox)gridViewImportRules.Controls[0].Controls[0].FindControl("cbCanBeAutoApprovedFirst")).Checked);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = @"
insert into SpectrumValidationRules(ID, NuclideName, ActivityMin, ActivityMax, ConfidenceMin, CanBeAutoApproved)
values(@ID, @NuclideName, @ActivityMin, @ActivityMax, @ConfidenceMin, @CanBeAutoApproved);
select ID, NuclideName, ActivityMin, ActivityMax, ConfidenceMin, CanBeAutoApproved from SpectrumValidationRules";
        cmd.Parameters.AddWithValue("@ID", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@NuclideName", txtNuclideName.Text);
        cmd.Parameters.AddWithValue("@ActivityMin", ActivityMin);
        cmd.Parameters.AddWithValue("@ActivityMax", ActivityMax);
        cmd.Parameters.AddWithValue("@ConfidenceMin", ConfidenceMin);
        cmd.Parameters.AddWithValue("@CanBeAutoApproved", CanBeAutoApproved);
        gridViewImportRules.DataSource = GetData(cmd);
        gridViewImportRules.DataBind();
    }

    protected void AddNewImportRule(object sender, EventArgs e)
    {
        labelStatusRules.Text = "";

        TextBox txtNuclideName = (TextBox)gridViewImportRules.FooterRow.FindControl("txtNuclideNameNew");
        if(String.IsNullOrEmpty(txtNuclideName.Text))
        {
            labelStatusRules.Text = "Nuklide navn mangler";
            return;
        }        

        double ActivityMin, ActivityMax, ConfidenceMin;

        TextBox txtActivityMinNew = (TextBox)gridViewImportRules.FooterRow.FindControl("txtActivityMinNew");
        if(String.IsNullOrEmpty(txtActivityMinNew.Text))
        {
            labelStatusRules.Text = "Aktivitet min mangler";
            return;
        }

        TextBox txtActivityMaxNew = (TextBox)gridViewImportRules.FooterRow.FindControl("txtActivityMaxNew");
        if (String.IsNullOrEmpty(txtActivityMaxNew.Text))
        {
            labelStatusRules.Text = "Aktivitet max mangler";
            return;
        }

        TextBox txtConfidenceMinNew = (TextBox)gridViewImportRules.FooterRow.FindControl("txtConfidenceMinNew");
        if (String.IsNullOrEmpty(txtConfidenceMinNew.Text))
        {
            labelStatusRules.Text = "Konfidens min mangler";
            return;
        }

        try
        {
            ActivityMin = Convert.ToDouble(txtActivityMinNew.Text);
            ActivityMax = Convert.ToDouble(txtActivityMaxNew.Text);
            ConfidenceMin = Convert.ToDouble(txtConfidenceMinNew.Text);
        }   
        catch(Exception ex)
        {
            labelStatusRules.Text = "Ugyldige tall funnet";
            return;
        }   
          
        bool CanBeAutoApproved = Convert.ToBoolean(((CheckBox)gridViewImportRules.FooterRow.FindControl("cbCanBeAutoApprovedNew")).Checked);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = @"
insert into SpectrumValidationRules(ID, NuclideName, ActivityMin, ActivityMax, ConfidenceMin, CanBeAutoApproved)
values(@ID, @NuclideName, @ActivityMin, @ActivityMax, @ConfidenceMin, @CanBeAutoApproved);
select ID, NuclideName, ActivityMin, ActivityMax, ConfidenceMin, CanBeAutoApproved from SpectrumValidationRules";
        cmd.Parameters.AddWithValue("@ID", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@NuclideName", txtNuclideName.Text);
        cmd.Parameters.AddWithValue("@ActivityMin", ActivityMin);
        cmd.Parameters.AddWithValue("@ActivityMax", ActivityMax);
        cmd.Parameters.AddWithValue("@ConfidenceMin", ConfidenceMin);
        cmd.Parameters.AddWithValue("@CanBeAutoApproved", CanBeAutoApproved);
        gridViewImportRules.DataSource = GetData(cmd);
        gridViewImportRules.DataBind();
    }

    protected void EditImportRule(object sender, GridViewEditEventArgs e)
    {
        labelStatusRules.Text = "";
        gridViewImportRules.EditIndex = e.NewEditIndex;
        BindImportRules();
    }

    protected void CancelEditImportRule(object sender, GridViewCancelEditEventArgs e)
    {
        labelStatusRules.Text = "";
        gridViewImportRules.EditIndex = -1;
        BindImportRules();
    }

    protected void UpdateImportRule(object sender, GridViewUpdateEventArgs e)
    {
        labelStatusRules.Text = "";

        Guid ID = new Guid(((Label)gridViewImportRules.Rows[e.RowIndex].FindControl("lblID")).Text);
        string NuclideName = ((TextBox)gridViewImportRules.Rows[e.RowIndex].FindControl("txtNuclideName")).Text;
        if (String.IsNullOrEmpty(NuclideName))
        {
            labelStatusRules.Text = "Mangler nuklide navn";
            return;
        }
        TextBox txtActivityMin = (TextBox)gridViewImportRules.Rows[e.RowIndex].FindControl("txtActivityMin");
        if (String.IsNullOrEmpty(txtActivityMin.Text))
        {
            labelStatusRules.Text = "Mangler aktivitet min";
            return;
        }

        TextBox txtActivityMax = (TextBox)gridViewImportRules.Rows[e.RowIndex].FindControl("txtActivityMax");
        if (String.IsNullOrEmpty(txtActivityMax.Text))
        {
            labelStatusRules.Text = "Mangler aktivitet max";
            return;
        }

        TextBox txtConfidenceMin = (TextBox)gridViewImportRules.Rows[e.RowIndex].FindControl("txtConfidenceMin");
        if (String.IsNullOrEmpty(txtConfidenceMin.Text))
        {
            labelStatusRules.Text = "Mangler konfidens min";
            return;
        }

        double ActivityMin, ActivityMax, ConfidenceMin;
        try
        {
            ActivityMin = Convert.ToDouble(txtActivityMin.Text);
            ActivityMax = Convert.ToDouble(txtActivityMax.Text);
            ConfidenceMin = Convert.ToDouble(txtConfidenceMin.Text);
        }
        catch(Exception ex)
        {
            labelStatusRules.Text = "Ugyldige tall funnet";
            return;
        }

        bool CanBeAutoApproved = Convert.ToBoolean(((CheckBox)gridViewImportRules.Rows[e.RowIndex].FindControl("cbCanBeAutoApproved")).Checked);        
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = @"
update SpectrumValidationRules set NuclideName=@NuclideName, ActivityMin=@ActivityMin, ActivityMax=@ActivityMax, ConfidenceMin=@ConfidenceMin, CanBeAutoApproved=@CanBeAutoApproved 
where ID=@ID;
select ID, NuclideName, ActivityMin, ActivityMax, ConfidenceMin, CanBeAutoApproved from SpectrumValidationRules";
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.Parameters.AddWithValue("@NuclideName", NuclideName);
        cmd.Parameters.AddWithValue("@ActivityMin", ActivityMin);
        cmd.Parameters.AddWithValue("@ActivityMax", ActivityMax);
        cmd.Parameters.AddWithValue("@ConfidenceMin", ConfidenceMin);
        cmd.Parameters.AddWithValue("@CanBeAutoApproved", CanBeAutoApproved);
        gridViewImportRules.EditIndex = -1;
        gridViewImportRules.DataSource = GetData(cmd);
        gridViewImportRules.DataBind();
    }

    protected void DeleteImportRule(object sender, EventArgs e)
    {
        LinkButton lbRemove = (LinkButton)sender;        
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = @"
delete from SpectrumValidationRules where NuclideName=@NuclideName;
select ID, NuclideName, ActivityMin, ActivityMax, ConfidenceMin, CanBeAutoApproved from SpectrumValidationRules";
        cmd.Parameters.AddWithValue("@NuclideName", lbRemove.CommandArgument);
        gridViewImportRules.DataSource = GetData(cmd);
        gridViewImportRules.DataBind();
    }

    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        BindImportRules();
        gridViewImportRules.PageIndex = e.NewPageIndex;
        gridViewImportRules.DataBind();
    }

    protected void AddFirstGeomRule(object sender, EventArgs e)
    {
        labelStatusRules.Text = "";

        TextBox txtGeometryNameNew = (TextBox)gridViewGeomRules.Controls[0].Controls[0].FindControl("txtGeometryNameFirst");
        if (String.IsNullOrEmpty(txtGeometryNameNew.Text))
        {
            labelStatusRules.Text = "Geometri navn mangler";
            return;
        }

        TextBox txtUnitNew = (TextBox)gridViewGeomRules.Controls[0].Controls[0].FindControl("txtUnitFirst");
        if (String.IsNullOrEmpty(txtUnitNew.Text))
        {
            labelStatusRules.Text = "Enhet mangler";
            return;
        }

        double Minimum, Maximum;

        TextBox txtMinimumNew = (TextBox)gridViewGeomRules.Controls[0].Controls[0].FindControl("txtMinimumFirst");
        if (String.IsNullOrEmpty(txtMinimumNew.Text))
        {
            labelStatusRules.Text = "Minimum mangler";
            return;
        }

        TextBox txtMaximumNew = (TextBox)gridViewGeomRules.Controls[0].Controls[0].FindControl("txtMaximumFirst");
        if (String.IsNullOrEmpty(txtMaximumNew.Text))
        {
            labelStatusRules.Text = "Maximum mangler";
            return;
        }

        try
        {
            Minimum = Convert.ToDouble(txtMinimumNew.Text);
            Maximum = Convert.ToDouble(txtMaximumNew.Text);
        }
        catch (Exception ex)
        {
            labelStatusRules.Text = "Ugyldige tall funnet";
            return;
        }

        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = @"
insert into SpectrumGeometryRules(ID, Geometry, Unit, Minimum, Maximum)
values(@ID, @Geometry, @Unit, @Minimum, @Maximum);
select ID, Geometry, Unit, Minimum, Maximum from SpectrumGeometryRules";
        cmd.Parameters.AddWithValue("@ID", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@Geometry", txtGeometryNameNew.Text);
        cmd.Parameters.AddWithValue("@Unit", txtUnitNew.Text);
        cmd.Parameters.AddWithValue("@Minimum", Minimum);
        cmd.Parameters.AddWithValue("@Maximum", Maximum);
        gridViewGeomRules.DataSource = GetData(cmd);
        gridViewGeomRules.DataBind();
    }

    protected void AddNewGeomRule(object sender, EventArgs e)
    {
        labelStatusRules.Text = "";

        TextBox txtGeometryNameNew = (TextBox)gridViewGeomRules.FooterRow.FindControl("txtGeometryNameNew");
        if (String.IsNullOrEmpty(txtGeometryNameNew.Text))
        {
            labelStatusRules.Text = "Geometri navn mangler";
            return;
        }

        TextBox txtUnitNew = (TextBox)gridViewGeomRules.FooterRow.FindControl("txtUnitNew");
        if (String.IsNullOrEmpty(txtUnitNew.Text))
        {
            labelStatusRules.Text = "Enhet mangler";
            return;
        }

        double Minimum, Maximum;

        TextBox txtMinimumNew = (TextBox)gridViewGeomRules.FooterRow.FindControl("txtMinimumNew");
        if (String.IsNullOrEmpty(txtMinimumNew.Text))
        {
            labelStatusRules.Text = "Minimum mangler";
            return;
        }

        TextBox txtMaximumNew = (TextBox)gridViewGeomRules.FooterRow.FindControl("txtMaximumNew");
        if (String.IsNullOrEmpty(txtMaximumNew.Text))
        {
            labelStatusRules.Text = "Maximum mangler";
            return;
        }        

        try
        {
            Minimum = Convert.ToDouble(txtMinimumNew.Text);
            Maximum = Convert.ToDouble(txtMaximumNew.Text);            
        }
        catch (Exception ex)
        {
            labelStatusRules.Text = "Ugyldige tall funnet";
            return;
        }
        
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = @"
insert into SpectrumGeometryRules(ID, Geometry, Unit, Minimum, Maximum)
values(@ID, @Geometry, @Unit, @Minimum, @Maximum);
select ID, Geometry, Unit, Minimum, Maximum from SpectrumGeometryRules";
        cmd.Parameters.AddWithValue("@ID", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@Geometry", txtGeometryNameNew.Text);
        cmd.Parameters.AddWithValue("@Unit", txtUnitNew.Text);
        cmd.Parameters.AddWithValue("@Minimum", Minimum);
        cmd.Parameters.AddWithValue("@Maximum", Maximum);
        gridViewGeomRules.DataSource = GetData(cmd);
        gridViewGeomRules.DataBind();
    }

    protected void EditGeomRule(object sender, GridViewEditEventArgs e)
    {
        labelStatusRules.Text = "";
        gridViewGeomRules.EditIndex = e.NewEditIndex;
        BindGeomRules();
    }

    protected void CancelEditGeomRule(object sender, GridViewCancelEditEventArgs e)
    {
        labelStatusRules.Text = "";
        gridViewGeomRules.EditIndex = -1;
        BindGeomRules();
    }

    protected void UpdateGeomRule(object sender, GridViewUpdateEventArgs e)
    {
        labelStatusRules.Text = "";

        Guid ID = new Guid(((Label)gridViewGeomRules.Rows[e.RowIndex].FindControl("lblID")).Text);
        string GeometryName = ((TextBox)gridViewGeomRules.Rows[e.RowIndex].FindControl("txtGeometryName")).Text;
        if (String.IsNullOrEmpty(GeometryName))
        {
            labelStatusRules.Text = "Mangler geometri navn";
            return;
        }
        TextBox txtUnit = (TextBox)gridViewGeomRules.Rows[e.RowIndex].FindControl("txtUnit");
        if (String.IsNullOrEmpty(txtUnit.Text))
        {
            labelStatusRules.Text = "Mangler enhet";
            return;
        }

        TextBox txtMinimum = (TextBox)gridViewGeomRules.Rows[e.RowIndex].FindControl("txtMinimum");
        if (String.IsNullOrEmpty(txtMinimum.Text))
        {
            labelStatusRules.Text = "Mangler minimum";
            return;
        }

        TextBox txtMaximum = (TextBox)gridViewGeomRules.Rows[e.RowIndex].FindControl("txtMaximum");
        if (String.IsNullOrEmpty(txtMaximum.Text))
        {
            labelStatusRules.Text = "Mangler maximum";
            return;
        }

        double Minimum, Maximum;
        try
        {
            Minimum = Convert.ToDouble(txtMinimum.Text);
            Maximum = Convert.ToDouble(txtMaximum.Text);            
        }
        catch (Exception ex)
        {
            labelStatusRules.Text = "Ugyldige tall funnet";
            return;
        }
        
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = @"
update SpectrumGeometryRules set Geometry=@Geometry, Unit=@Unit, Minimum=@Minimum, Maximum=@Maximum
where ID=@ID;
select ID, Geometry, Unit, Minimum, Maximum from SpectrumGeometryRules";
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.Parameters.AddWithValue("@Geometry", GeometryName);
        cmd.Parameters.AddWithValue("@Unit", txtUnit.Text);
        cmd.Parameters.AddWithValue("@Minimum", Minimum);
        cmd.Parameters.AddWithValue("@Maximum", Maximum);
        gridViewGeomRules.EditIndex = -1;
        gridViewGeomRules.DataSource = GetData(cmd);
        gridViewGeomRules.DataBind();
    }

    protected void DeleteGeomRule(object sender, EventArgs e)
    {
        LinkButton lbGeomRemove = (LinkButton)sender;
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = @"
delete from SpectrumGeometryRules where Geometry=@Geometry;
select ID, Geometry, Unit, Minimum, Maximum from SpectrumGeometryRules";
        cmd.Parameters.AddWithValue("@Geometry", lbGeomRemove.CommandArgument);
        gridViewImportRules.DataSource = GetData(cmd);
        gridViewImportRules.DataBind();
    }

    protected void OnGeomPaging(object sender, GridViewPageEventArgs e)
    {
        BindGeomRules();
        gridViewGeomRules.PageIndex = e.NewPageIndex;
        gridViewGeomRules.DataBind();
    }
}
