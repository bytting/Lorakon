<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" Title="<%$ Resources:Localization, Title_Login %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">

    <div style="margin:8px">
    
    <table cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <asp:Login ID="login" runat="server"
                MembershipProvider="AspNetSqlMembershipProvider"        
                DestinationPageUrl="~/Default.aspx" 
                CreateUserUrl=""         
                LoginButtonStyle-CssClass="ButtonClass" 
                InstructionText="" 
                TitleText="" 
                CreateUserText=""
                LabelStyle-CssClass="TipTextBig"                
                FailureText="<%$ Resources:Localization, WrongUsernameOrPassword %>" 
                LoginButtonText="<%$ Resources:Localization, Login %>"         
                PasswordLabelText="<%$ Resources:Localization, Password %>" 
                RememberMeText="<%$ Resources:Localization, RememberMe %>"         
                UserNameLabelText="<%$ Resources:Localization, Username %>" 
                TextBoxStyle-CssClass="TextBoxClass2x" 
                DisplayRememberMe="false" 
                OnLoggingIn="login_OnLoggingIn"> 
            </asp:Login>
        </td>
        
        <td valign="top">        
            <table cellspacing="4" cellpadding="4">
            <tr>
                <td>
                    <asp:LinkButton runat="server" Text="<%$ Resources:Localization, RequestUser %>" PostBackUrl="~/CreateUser.aspx"></asp:LinkButton> 
                </td>
                <td>
                    <asp:LinkButton runat="server" Text="<%$ Resources:Localization, Link_ForgotPassword %>" PostBackUrl="~/ForgotPassword.aspx"></asp:LinkButton>  
                </td>
            </tr>    
            </table>        
        </td>
    </tr>
    </table>
    
    <br />
    
    <center>
        <asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass"/>
    </center>  
    
    </div>
    
</asp:Content>

