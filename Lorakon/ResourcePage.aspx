<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="ResourcePage.aspx.cs" Inherits="ResourcePage" Title="<%$ Resources:Localization, Title_Resources %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">

    <div style="margin:8px">        
    
        <br />
        
        <asp:Literal ID="labelResourcePage" runat="server"></asp:Literal>    
        
        <br />            
    
        <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
    
    </div>
    
</asp:Content>

