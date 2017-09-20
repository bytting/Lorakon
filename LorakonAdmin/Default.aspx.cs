/*
The author asserts his copyright over this file and all files written by him containing links to this copyright declaration under the terms of the copyright laws in force in the country you are reading this work in. 
This work is not in the public domain. This work is copyright © Dag Robøle 2008. All rights reserved. 
*/
using System;
using System.Drawing;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class _Default : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!HttpContext.Current.User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");

        if (!Page.IsPostBack)
        {
            try
            {
                Database.Interface.open();

                Database.Ringtest.disableOldRingtests();
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
}
