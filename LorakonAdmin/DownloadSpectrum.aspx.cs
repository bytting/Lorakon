using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class DownloadSpectrum : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MembershipUser User = Membership.GetUser(HttpContext.Current.User.Identity.Name);
        if (User == null)
            Response.Redirect("~/Login.aspx");

        if (!Page.IsPostBack)
        {
            SqlConnection connection = null;

            try
            {
                string specId = Request.QueryString["id"];
                if(String.IsNullOrEmpty(specId))
                {
                    labelStatus.Text = "No spectrum ID query parameter found";
                    return;
                }

                Guid id = new Guid(specId);

                connection = new SqlConnection(Database.Interface.connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand("select SpectrumFileExtension, SpectrumFileContent from SpectrumFile where SpectrumInfoID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader == null || reader.HasRows == false)
                {
                    labelStatus.Text = "No spectrum found for ID: " + specId;
                    return;
                }

                reader.Read();
                string extension = reader["SpectrumFileExtension"].ToString();
                Byte[] bytes = (Byte[])reader["SpectrumFileContent"];
                reader.Close();

                //Response.Buffer = true;
                //Response.Charset = "";
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);                
                Response.Clear();
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.AddHeader("Content-Disposition", "inline; filename=" + specId + extension);
                Response.AddHeader("Content-Length", bytes.Length.ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();                
            }
            catch (Exception ex)
            {
                labelStatus.Text = ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}