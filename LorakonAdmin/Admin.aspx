<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" Title="LorakonAdmin | Brukere" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
    <script type="text/javascript">
        function OnTabChanged(sender, args)
        {
            sender.get_clientStateField().value = sender.saveClientState();                                                
        }                                                                                                          
    </script> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">    
    
    <act:TabContainer ID="tabContainerAdmin" runat="server" OnClientActiveTabChanged="OnTabChanged">            
                
        <act:TabPanel ID="tabAdmin" runat="server" HeaderText="Administrasjon av brukere">
        
            <ContentTemplate>                                      
                
                <asp:UpdatePanel ID="updatePanelAdmin" runat="server">
                <ContentTemplate>
                
                    <div style="float:right;margin:8px">
                        <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelAdmin" DisplayAfter="0">
                            <ProgressTemplate>
                                <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    
                    <div style="margin:8px">
                        
                    <div style="float:left">
                    <table cellspacing="3" cellpadding="0">
                    <tr>
                        <th align="left">Opprett ny bruker</th>                        
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Navn" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="tbCreateName" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbCreateNameFilter" 
                                TargetControlID="tbCreateName" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-, " />
                        </td>        
                    </tr>    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Tittel" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="tbCreateTitle" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbCreateTitleFilter" 
                                TargetControlID="tbCreateTitle" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå " />
                        </td>        
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Brukernavn" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbCreateUser" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbCreateUserFilter" 
                                TargetControlID="tbCreateUser" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå_" />
                        </td>        
                    </tr>                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Passord" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbCreatePassword" runat="server" TextMode="Password" CssClass="TextBoxClass2x"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbCreatePasswordFilter" 
                                TargetControlID="tbCreatePassword" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå_" />
                        </td>
                    </tr>                    
                    <tr>                    
                        <td>
                            <asp:Label runat="server" Text="Gjenta passord" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbCreatePassword2" runat="server" TextMode="Password" CssClass="TextBoxClass2x"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbCreatePassword2Filter" 
                                TargetControlID="tbCreatePassword2" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå_" />
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Telefon" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="tbCreatePhone" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbCreatePhoneFilter" 
                                TargetControlID="tbCreatePhone" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+" />
                        </td>        
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="e-post" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="tbCreateEmail" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbCreateEmailFilter" 
                                TargetControlID="tbCreateEmail" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="@.!#$%&'*+-/=?^_`{|}~" />
                        </td>        
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Button ID="buttonCreateUser" runat="server" 
                                Text="Opprett bruker" 
                                OnClick="buttonCreateUser_OnClick" 
                                CssClass="ButtonClass"/>
                        </td>
                    </tr>
                    </table>        
                    </div>
                    
                    <div style="float:left; margin-left:50px">                                            
                    
                        <table cellspacing="3" cellpadding="0">                        
                        <tr>
                            <th align="left">Rediger bruker</th>
                        </tr>
                        <tr>                            
                            <td>                                
                                <asp:DropDownList ID="ddUsers" runat="server" 
                                    CssClass="TextBoxClass2x"
                                    AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddUsers_OnSelectedIndexChanged"/>    
                            </td>
                        </tr>
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Navn" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbEditName" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbEditNameFilter" 
                                    TargetControlID="tbEditName" 
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="ÆØÅæøå-, " />
                            </td>        
                        </tr>    
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Tittel" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbEditTitle" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbEditTitleFilter" 
                                    TargetControlID="tbEditTitle" 
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="ÆØÅæøå " />
                            </td>        
                        </tr>
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Telefon" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbEditPhone" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbEditPhoneFilter" 
                                    TargetControlID="tbEditPhone" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="+" />
                            </td>        
                        </tr>
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="e-post" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbEditEmail" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbEditEmailFilter" 
                                    TargetControlID="tbEditEmail" 
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="@.!#$%&'*+-/=?^_`{|}~" />
                            </td>        
                        </tr>
                        <tr>                               
                            <td>
                                <asp:Button ID="buttonUpdateUser" runat="server" Text="Oppdater bruker" OnClick="buttonUpdateUser_OnClick" CssClass="ButtonClass"/>                        
                                <asp:Button ID="buttonDeleteUser" runat="server" Text="Slett bruker" OnClick="buttonDeleteUser_OnClick" CssClass="ButtonClass"/>                    
                            </td>
                        </tr>
                        </table>                                                
                            
                        <br />
                            
                        <table>
                            <tr>
                                <th align="left">Bytte av passord</th>
                            </tr>                            
                            <tr>                                
                                <td>
                                    <asp:Label runat="server" Text="Nytt passord" CssClass="TipText"></asp:Label>                                
                                    <br />
                                    <asp:TextBox ID="tbChangePassword" runat="server" TextMode="Password" CssClass="TextBoxClass2x"></asp:TextBox>                                    
                                    <act:FilteredTextBoxExtender runat="server" 
                                        ID="tbChangePasswordFilter" 
                                        TargetControlID="tbChangePassword" 
                                        FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                        ValidChars="ÆØÅæøå_" />
                                </td>            
                            </tr>                            
                            <tr>                                
                                <td>
                                    <asp:Label runat="server" Text="Gjenta nytt passord" CssClass="TipText"></asp:Label>                                
                                    <br />
                                    <asp:TextBox ID="tbChangePassword2" runat="server" TextMode="Password" CssClass="TextBoxClass2x"></asp:TextBox>
                                    <act:FilteredTextBoxExtender runat="server" 
                                        ID="tbChangePassword2Filter" 
                                        TargetControlID="tbChangePassword2" 
                                        FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                        ValidChars="ÆØÅæøå_" />
                                </td>
                            </tr>
                            <tr>                                
                                <td>
                                    <asp:Button ID="buttonChangePassword" runat="server" Text="Bytt passord" OnClick="buttonChangePassword_OnClick" CssClass="ButtonClass"/>                                   
                                </td>
                            </tr>
                        </table>                                                                             
                                                
                    </div>                
                    
                    <div style="float:left; margin-left:50px">                                            
                    
                        <asp:Table ID="tableRoles" runat="server" CellSpacing="0" CellPadding="0" CaptionAlign="Left" Caption="<b>Roller</b>"></asp:Table>            
                        
                    </div>                                                                                
                    
                    <div style="float:left;clear:both"> 
                        <br />
                        <table>     
                            <tr>
                                <th>Seksjonssjef</th>
                            </tr>                       
                            <tr>                                
                                <td>                                    
                                    <asp:TextBox ID="tbSectionManager" runat="server" MaxLength="79" CssClass="TextBoxClass2x"></asp:TextBox>                                    
                                </td>
                                <td>
                                    <asp:Button ID="buttonUpdateSectionManager" runat="server" 
                                        Text="Oppdater" 
                                        OnClick="buttonUpdateSectionManager_OnClick" 
                                        CssClass="ButtonClass"/>
                                </td>
                            </tr>   
                            <tr>
                                <th>Administrator e-post</th>
                            </tr>
                            <tr>
                                <td>                                    
                                    <asp:TextBox ID="tbRingtestAdminEmail" runat="server" MaxLength="79" CssClass="TextBoxClass2x"></asp:TextBox>                                    
                                    <act:FilteredTextBoxExtender runat="server" 
                                        ID="tbRingtestAdminEmailFilter" 
                                        TargetControlID="tbRingtestAdminEmail" 
                                        FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                        ValidChars="@.!#$%&'*+-/=?^_`{|}~" />
                                </td>
                                <td>
                                    <asp:Button ID="buttonUpdateRingtestAdminEmail" runat="server" 
                                        Text="Oppdater" 
                                        OnClick="buttonUpdateRingtestAdminEmail_OnClick" 
                                        CssClass="ButtonClass"/>
                                </td>
                            </tr>                 
                        </table>                                                
                    </div>
                    
                    <div style="clear:both">
                        <br />
                        <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
                    </div>
                    
                    </div>                                        
                
            </ContentTemplate>
            </asp:UpdatePanel>
        
        </ContentTemplate>    
    </act:TabPanel>
    
    <act:TabPanel ID="tabUser" runat="server" HeaderText="Administrasjon av egen bruker">
        
            <ContentTemplate>                                      
                
                <asp:UpdatePanel ID="updatePanelUser" runat="server">
                <ContentTemplate>
                
                    <div style="float:right;margin:8px">
                        <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelUser" DisplayAfter="0">
                            <ProgressTemplate>
                                <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    
                    <div style="margin:8px">
                    
                     <table cellspacing="3" cellpadding="0">
                        <tr>
                            <th align="left">Oppdater bruker</th>
                        </tr>                        
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Navn" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbEditNameUser" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbEditNameUserFilter" 
                                    TargetControlID="tbEditNameUser" 
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="ÆØÅæøå-, " />
                            </td>        
                        </tr>    
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Tittel" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbEditTitleUser" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbEditTitleUserFilter" 
                                    TargetControlID="tbEditTitleUser" 
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="ÆØÅæøå " />
                            </td>        
                        </tr>
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Telefon" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbEditPhoneUser" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbEditPhoneUserFilter" 
                                    TargetControlID="tbEditPhoneUser" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="+" />
                            </td>        
                        </tr>
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="e-post" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbEditEmailUser" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbEditEmailUserFilter" 
                                    TargetControlID="tbEditEmailUser" 
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="@.!#$%&'*+-/=?^_`{|}~" />
                            </td>        
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="buttonUpdateUserUser" runat="server" 
                                    Text="Oppdater bruker" 
                                    OnClick="buttonUpdateUserUser_OnClick" 
                                    CssClass="ButtonClass"/>                        
                            </td>                        
                        </tr>
                        </table>
                          
                        <br />
                              
                        <table>
                            <tr>
                                <th align="left">Bytte av passord</th>
                            </tr>                                                                                    
                            <tr>            
                                <td>
                                    <asp:Label runat="server" Text="Nytt passord" CssClass="TipText"></asp:Label>                                
                                    <br />
                                    <asp:TextBox ID="tbChangePasswordUser" runat="server" TextMode="Password" CssClass="TextBoxClass2x"></asp:TextBox>
                                    <act:FilteredTextBoxExtender runat="server" 
                                        ID="tbChangePasswordUserFilter" 
                                        TargetControlID="tbChangePasswordUser" 
                                        FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                        ValidChars="ÆØÅæøå_" />
                                </td>            
                            </tr>                            
                            <tr>                                
                                <td>
                                    <asp:Label runat="server" Text="Gjenta nytt passord" CssClass="TipText"></asp:Label>                                
                                    <br />
                                    <asp:TextBox ID="tbChangePassword2User" runat="server" TextMode="Password" CssClass="TextBoxClass2x"></asp:TextBox>
                                    <act:FilteredTextBoxExtender runat="server" 
                                        ID="tbChangePassword2UserFilter" 
                                        TargetControlID="tbChangePassword2User" 
                                        FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                        ValidChars="ÆØÅæøå_" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="buttonChangePasswordUser" runat="server" 
                                        Text="Bytt passord" 
                                        OnClick="buttonChangePasswordUser_OnClick" 
                                        CssClass="ButtonClass"/>                                   
                                </td>
                            </tr>
                        </table>                                                                                  
                        
                    </div>
                    
                    <div style="clear:both">
                        <br />
                        <center><asp:Label ID="labelStatusUser" runat="server" CssClass="StatusMessageClass" /></center>
                    </div>
                    
                </ContentTemplate>
                </asp:UpdatePanel>
            
            </ContentTemplate>
    </act:TabPanel>
    
    </act:TabContainer>
    
</asp:Content>

