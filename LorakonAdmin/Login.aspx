<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" Title="LorakonAdmin | Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">        

    <table cellpadding="4" cellspacing="4" width="100%">
        <tr>
            <td style="text-align:left; vertical-align:top">
                <asp:Login ID="loginLorakonAdmin" runat="server"
                    MembershipProvider="LorakonProvider" 
                    DestinationPageUrl="~/Default.aspx" 
                    LoginButtonStyle-CssClass="ButtonClass" 
                    LoginButtonStyle-Width="120px"
                    FailureText="Feil brukernavn eller passord" 
                    LoginButtonText="Logg på" 
                    InstructionText="" 
                    PasswordLabelText="Passord&nbsp;" 
                    RememberMeText="Husk meg ved neste pålogging" 
                    TitleText="" 
                    UserNameLabelText="Brukernavn&nbsp;"                     
                    LabelStyle-CssClass="TipTextBig"
                    TextBoxStyle-CssClass="TextBoxClass2x" 
                    DisplayRememberMe="false">
                </asp:Login>    
            </td>
            <td style="text-align:left; vertical-align:top">
                <asp:Label runat="server" Text="Logg på for administrasjon av LORAKON databasen." CssClass="TipTextBig"></asp:Label>
                <br />
                <asp:Label runat="server" Text="Ta kontakt med en LORAKON administrator hvis du ikke har en egen konto." CssClass="TipTextBig"></asp:Label>
                <br />                                
                <asp:Label ID="labelAdminEmail" runat="server" CssClass="TipTextBig"></asp:Label>
            </td>
        </tr>
    </table>
    
    <br />
    <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
    
</asp:Content>

