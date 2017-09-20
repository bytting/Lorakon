<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" Title="<%$ Resources:Localization, Title_ForgotPassword %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">

    <asp:UpdatePanel ID="updatePanelMain" runat="server">
    <ContentTemplate>
    
    <div style="float:right;margin:8px">
        <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelMain" DisplayAfter="0">
            <ProgressTemplate>
                <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <div style="margin:8px">
    
    <asp:Label runat="server" Text="<%$ Resources:Localization, ForgotPassword_header %>" Font-Bold="true"></asp:Label>
    <br /><br />
    
    <table>    
        <tr>
            <td width="200px">                
                <asp:Label runat="server" Text="<%$ Resources:Localization, Fullname2 %>" CssClass="TipTextBig"></asp:Label>                                
            </td>
            <td width="320px">                
                <asp:TextBox ID="tbCompanyName" runat="server" CssClass="TextBoxClass2x" MaxLength="127"></asp:TextBox>                
                <font color="#FF0000">*</font>                
                <act:FilteredTextBoxExtender runat="server" 
                    ID="tbCompanyNameFilter" 
                    TargetControlID="tbCompanyName" 
                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" 
                    ValidChars="ÆØÅæøå+-,./ " />
            </td>
            <td>                
            </td>
        </tr>
        
        <tr>
            <td>                
                <img src="JpegImage.aspx"/>
            </td>
            <td>
                <asp:TextBox ID="textBoxCAPTCHA" runat="server" CssClass="TextBoxClass2x" MaxLength="32"></asp:TextBox>
                <font color="#FF0000">*</font>                
            </td>
            <td>
                <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, CAPTCHA_description %>"></asp:Label>
            </td>
        </tr>     
        
        <tr>
            <td></td>
            <td>
                <asp:Button ID="buttonSendRequest" runat="server" Text="<%$ Resources:Localization, Send %>" OnClick="buttonSendRequest_OnClick" CssClass="ButtonClass" />
                <asp:Button ID="buttonCancel" runat="server" Text="<%$ Resources:Localization, Cancel %>" OnClick="buttonCancel_OnClick" CssClass="ButtonClass" />
            </td>
        </tr>
    </table>               
    
    <br />
    
    <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
    
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>

