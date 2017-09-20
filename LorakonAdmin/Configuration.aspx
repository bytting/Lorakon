<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Configuration.aspx.cs" Inherits="Configuration" Title="LorakonAdmin - Konfigurasjon" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="fck2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server"> 
    <script type="text/javascript">
        function OnTabChanged(sender, args)
        {
            sender.get_clientStateField().value = sender.saveClientState();                                                
        }                                                                                                                          
    </script>                  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">

    <act:TabContainer ID="tabContainerCourse" runat="server" OnClientActiveTabChanged="OnTabChanged">            
        
        <act:TabPanel ID="tabEditConfig" runat="server" HeaderText="Rediger startside">
            <ContentTemplate>                                                            
            
                <table style="width:100%;height:520px;margin:0" cellpadding="0" cellspacing="0">        
                    <tr>
                        <td>    
                            <fck2:FCKeditor ID="textBoxEdit" runat="server" Width="100%" Height="520px" BasePath="~/fckeditor/" >
                            </fck2:FCKeditor>                                        
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            <asp:Button ID="buttonFrontpageSave" runat="server" Text="Lagre" CssClass="ButtonClass" Width="220px" OnClick="buttonFrontpageSave_OnClick"/>
                        </td>
                    </tr>                           
                </table>            
                
                <br />
                <center><asp:Label ID="labelStatusEditConfig" runat="server" CssClass="StatusMessageClass" /></center>                            
                
            </ContentTemplate>
        </act:TabPanel>
        
        <act:TabPanel ID="tabShowConfig" runat="server" HeaderText="Vis startside">
            <ContentTemplate>                            
                
                <div style="margin:8px">                    
            
                <asp:Literal ID="literalShowConfig" runat="server"></asp:Literal>
                
                <br />
                <center><asp:Label ID="labelStatusShowConfig" runat="server" CssClass="StatusMessageClass" /></center>
                
                </div>                
            </ContentTemplate>
        </act:TabPanel>
        
        <act:TabPanel ID="tabEditResources" runat="server" HeaderText="Rediger ressurser">
            <ContentTemplate>                                                            
            
                <table style="width:100%;height:520px;margin:0" cellpadding="0" cellspacing="0">        
                    <tr>
                        <td>                                         
                            <fck2:FCKeditor ID="textBoxResources" runat="server" Width="100%" Height="520px" BasePath="~/fckeditor/" >
                            </fck2:FCKeditor>                                        
                        </td>
                    </tr>    
                    <tr>
                        <td>
                            <asp:Button ID="buttonResourcesSave" runat="server" Text="Lagre" CssClass="ButtonClass" Width="220px" OnClick="buttonResourcesSave_OnClick"/>
                        </td>
                    </tr>                                                   
                </table>            
                
                <br />
                <center><asp:Label ID="labelStatusEditResources" runat="server" CssClass="StatusMessageClass" /></center>                            
                
            </ContentTemplate>
        </act:TabPanel>
        
        <act:TabPanel ID="tabShowResources" runat="server" HeaderText="Vis ressurser">
            <ContentTemplate>                            
                
                <div style="margin:8px">                    
            
                <asp:Literal ID="literalShowResources" runat="server"></asp:Literal>
                
                <br />
                <center><asp:Label ID="labelStatusShowResources" runat="server" CssClass="StatusMessageClass" /></center>
                
                </div>                
            </ContentTemplate>
        </act:TabPanel>
        
    </act:TabContainer>    
    
</asp:Content>

