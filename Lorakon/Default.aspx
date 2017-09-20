<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="<%$ Resources:Localization, Title_Default %>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="phHead" Runat="Server">    
</asp:Content>    

<asp:Content ID="content2" ContentPlaceHolderID="phMain" Runat="Server">     

    <div style="margin:8px">
    
        <br />
        
        <asp:Literal ID="labelStartpage" runat="server"></asp:Literal>    
        
        <br />            
    
        <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
    
    </div>
    
</asp:Content>                	    	    	    	
