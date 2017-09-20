<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="LorakonAdmin" %>

<asp:Content ID="Content3" ContentPlaceHolderID="phHead" Runat="Server">    
</asp:Content>    

<asp:Content ID="content2" ContentPlaceHolderID="phMain" Runat="Server">         
    
    <div style="margin:8px">
    
    <asp:Label ID="labelStart" runat="server" Text="Velkommen til LORAKON Administrasjon.<br><br>Bruk menyen øverst på siden til å navigere" CssClass="TipTextBig"></asp:Label>
    
    <br />
    
    <center>
        <asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" />
    </center>
    
    </div>        
    
</asp:Content>                	    	    	    	
