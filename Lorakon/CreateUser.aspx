<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="CreateUser" Title="<%$ Resources:Localization, Title_CreateUser %>" %>

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
    
    <asp:Label runat="server" Text="<%$ Resources:Localization, CreateUser_header %>" Font-Bold="true"></asp:Label>
    <br /><br />
    
    <table>    
        <tr>
            <td width="200px">                
                <asp:Label runat="server" Text="<%$ Resources:Localization, Fullname %>" CssClass="TipTextBig"></asp:Label>                                
            </td>
            <td width="320px">                                
                <asp:TextBox ID="textBoxFullname" runat="server" CssClass="TextBoxClass2x" MaxLength="127"></asp:TextBox>
                <font color="#FF0000">*</font>                
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxFullnameFilter" 
                    TargetControlID="textBoxFullname" 
                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" 
                    ValidChars="ÆØÅæøå+-,./ " />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Fullname_description %>"></asp:Label> -->
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Address_reminder %>"></asp:Label>
            </td>
            <td>
            </td>
        </tr>                                                            
        <tr>
            <td>                
                <asp:Label runat="server" Text="<%$ Resources:Localization, Address %>" CssClass="TipTextBig"></asp:Label>                
            </td>
            <td>
                <asp:TextBox ID="textBoxAddress" runat="server" CssClass="TextBoxClass2x" MaxLength="255"></asp:TextBox>
                <font color="#FF0000">*</font>                
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxAddressFilter" 
                    TargetControlID="textBoxAddress" 
                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                    ValidChars="ÆØÅæøå-, " />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Address_description %>"></asp:Label> -->
            </td>
        </tr>                
        <tr>
            <td>                
                <asp:Label runat="server" Text="<%$ Resources:Localization, Postal %>" CssClass="TipTextBig"></asp:Label>                
            </td>
            <td>
                <asp:TextBox ID="textBoxPostal" runat="server" CssClass="TextBoxClass2x" MaxLength="95"></asp:TextBox>
                <font color="#FF0000">*</font>                
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxPostalFilter" 
                    TargetControlID="textBoxPostal" 
                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                    ValidChars="ÆØÅæøå- " />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Postal_description %>"></asp:Label> -->
            </td>
        </tr>                
        <tr>
            <td>
                <asp:Label runat="server" Text="<%$ Resources:Localization, ContactCreate %>" CssClass="TipTextBig"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textBoxContact" runat="server" CssClass="TextBoxClass2x" MaxLength="95"></asp:TextBox>
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxContactFilter" 
                    TargetControlID="textBoxContact" 
                    FilterType="Custom, LowercaseLetters, UppercaseLetters" 
                    ValidChars="ÆØÅæøå-, " />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, ContactCreate_description %>"></asp:Label> -->
            </td>
        </tr>        
        <tr>
            <td>                
                <asp:Label runat="server" Text="Kontakpersonens E-post" CssClass="TipTextBig"></asp:Label>                
            </td>
            <td>
                <asp:TextBox ID="textBoxEmail" runat="server" CssClass="TextBoxClass2x" MaxLength="95"></asp:TextBox>
                <font color="#FF0000">*</font>                
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxEmailFilter" 
                    TargetControlID="textBoxEmail" 
                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                    ValidChars="@.!#$%&'*+-/=?^_`{|}~" />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Email_description %>"></asp:Label> -->
            </td>
        </tr>                
        <tr>
            <td>
                <asp:Label runat="server" Text="<%$ Resources:Localization, Phone %>" CssClass="TipTextBig"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textBoxPhone" runat="server" CssClass="TextBoxClass2x" MaxLength="15"></asp:TextBox>
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxPhoneFilter" 
                    TargetControlID="textBoxPhone" 
                    FilterType="Custom, Numbers" 
                    ValidChars="+ " />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Phone_description %>"></asp:Label> -->
            </td>
        </tr>                
        <tr>
            <td>
                <asp:Label runat="server" Text="<%$ Resources:Localization, Mobile %>" CssClass="TipTextBig"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textBoxMobile" runat="server" CssClass="TextBoxClass2x" MaxLength="15"></asp:TextBox>
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxMobileFilter" 
                    TargetControlID="textBoxMobile" 
                    FilterType="Custom, Numbers" 
                    ValidChars="+ " />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Mobile_description %>"></asp:Label> -->
            </td>
        </tr>                
        <tr>
            <td>
                <asp:Label runat="server" Text="<%$ Resources:Localization, Fax %>" CssClass="TipTextBig"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textBoxFax" runat="server" CssClass="TextBoxClass2x" MaxLength="15"></asp:TextBox>
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxFaxFilter" 
                    TargetControlID="textBoxFax" 
                    FilterType="Custom, Numbers" 
                    ValidChars="+ " />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Fax_description %>"></asp:Label> -->
            </td>
        </tr>                
        <tr>
            <td>
                <asp:Label runat="server" Text="<%$ Resources:Localization, Website %>" CssClass="TipTextBig"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="textBoxWebsite" runat="server" CssClass="TextBoxClass2x" MaxLength="255"></asp:TextBox>
                <act:FilteredTextBoxExtender runat="server" 
                    ID="textBoxWebsiteFilter" 
                    TargetControlID="textBoxWebsite" 
                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                    ValidChars="-_/.:" />
            </td>
            <td>
                <!-- <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Website_description %>"></asp:Label> -->
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
            <td>                
            </td>
            <td>
                <asp:Button ID="buttonRequestUser" runat="server" Text="<%$ Resources:Localization, RequestUser %>" OnClick="buttonRequestUser_OnClick" CssClass="ButtonClass"/>            
                <asp:Button ID="buttonCancel" runat="server" Text="<%$ Resources:Localization, Cancel %>" OnClick="buttonCancel_OnClick" CssClass="ButtonClass"/>
            </td>
        </tr>                                
    </table>
    
    <br />
    
    <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
    
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>

