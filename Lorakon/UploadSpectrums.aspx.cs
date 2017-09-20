using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UploadSpectrums : System.Web.UI.Page
{
    const int maxTotalContentLength = 1073741824;

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
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {            
            HttpFileCollection hfc = Request.Files;

            int totalLength = 0;
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile hpf = hfc[i];
                if (String.IsNullOrEmpty(hpf.FileName))
                    continue;

                if (Path.GetExtension(hpf.FileName).ToLower() != ".cnf")
                    continue;

                totalLength += hpf.ContentLength;                
            }
            
            if (totalLength >= maxTotalContentLength)
            {
                Utils.reportStatus(ref labelStatus, Color.Red, "Opplasting feilet. Vennligst last opp ferre filer av gangen");
                return;
            }

            List<string> uploadedFileNames = new List<string>();

            string uploadPath = Server.MapPath("~/Spectrums/");

            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile hpf = hfc[i];
                if (String.IsNullOrEmpty(hpf.FileName))
                    continue;

                if (Path.GetExtension(hpf.FileName).ToLower() != ".cnf")
                    continue;

                if (hpf.ContentLength > 0)
                {
                    string saveFileName = Path.Combine(uploadPath, Path.GetFileName(hpf.FileName));
                    uploadedFileNames.Add(Path.GetFileName(hpf.FileName));
                    if (!File.Exists(saveFileName))
                        hpf.SaveAs(saveFileName);
                }
            }

            tblUploadFiles.Rows.Clear();

            TableHeaderRow hrow = new TableHeaderRow();
            TableHeaderCell hcell = new TableHeaderCell();
            hcell.HorizontalAlign = HorizontalAlign.Left;
            hrow.Cells.Add(hcell);
            hcell.Text = "Følgende filer ble opplastet:";
            tblUploadFiles.Rows.Add(hrow);

            foreach (string fname in uploadedFileNames)
            {
                TableRow row = new TableRow();                
                TableCell cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Left;
                row.Cells.Add(cell);
                cell.Text = fname;
                tblUploadFiles.Rows.Add(row);
            }
        }
        catch (Exception ex)
        {
            Utils.reportStatus(ref labelStatus, Color.Red, "UploadSpectrums.btnUpload_Click: " + ex.Message);
        }               
    }

    protected void btnClearUpload_Click(object sender, EventArgs e)
    {
        tblUploadFiles.Rows.Clear();
    }

    protected void ddSpectrumYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(String.IsNullOrEmpty(ddSpectrumYear.Text))
        {
            gridSpectrums.DataSourceID = null;
            gridSpectrums.DataBind();
            labelStatus.Text = "";
            return;
        }

        MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);
        object UserGUID = User.ProviderUserKey;

        gridSpectrums.DataSourceID = "dataSourceSpectrums";
        dataSourceSpectrums.SelectParameters.Clear();
        dataSourceSpectrums.SelectParameters.Add("AccountID", UserGUID.ToString());
        dataSourceSpectrums.SelectParameters.Add("year", ddSpectrumYear.Text);
        gridSpectrums.DataBind();

        labelStatus.Text = "Viser spekter for " + ddSpectrumYear.Text;
    }
}