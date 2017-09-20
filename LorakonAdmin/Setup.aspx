<%@ Page Title="LorakonAdmin | Setup" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Setup.aspx.cs" Inherits="Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">

    <div style="margin:8px">
    
        <asp:Label ID="labelDatabase" runat="server"></asp:Label>
        <br />
        <asp:Label ID="labelCatalog" runat="server"></asp:Label>
        <br /><br />
        <asp:Label ID="labelInfo" runat="server"></asp:Label>
        <br /><br />
        <asp:Label runat="server" Text="Bruker: Administrator"></asp:Label>
        <br />
        Epost:<br /><asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
        <br />
        Passord:<br /><asp:TextBox ID="tbPassword1" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        Gjenta passord:<br /><asp:TextBox ID="tbPassword2" runat="server" TextMode="Password"></asp:TextBox>
        <br /><br />
        <asp:Button ID="btnCreatePassword" runat="server" Text="Opprett administrator passord" CssClass="ButtonClass" OnClick="btnCreatePassword_OnClick"/>
        <asp:Button ID="btnUpdatePassword" runat="server" Text="Oppdater administrator passord" CssClass="ButtonClass" OnClick="btnUpdatePassword_OnClick"/>
        <br /><br />
        <asp:Label ID="labelWarning" runat="server" Text="HUSK Å SLETTE DENNE FILEN (Setup.aspx / Setup.aspx.cs) ETTER AT ADMINISTRATOR ER KONFIGURERT!" ForeColor="Red" Font-Bold="true"></asp:Label>

        <br /><br />
        <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
    
    </div>
                        
</asp:Content>

