<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Account.aspx.cs" Inherits="Account" Title="<%$ Resources:Localization, Title_Account %>" %>

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

    <act:TabContainer ID="tabContainerAccount" runat="server" OnClientActiveTabChanged="OnTabChanged">            
        
        <act:TabPanel ID="tabAccount" runat="server" HeaderText="<%$ Resources:Localization, Account_information %>">        
        <ContentTemplate>
            
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
    
                <br />
    
                <asp:HiddenField ID="hiddenName" runat="server"/>
    
                <table cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <th align="left" colspan="2">
                        <asp:Label runat="server" Text="<%$ Resources:Localization, Account_header %>"></asp:Label>        
                    </th>
                </tr>
                <tr>
                    <td style="height:16px" colspan="2"></td>
                </tr>
                <tr>
                <td valign="top">
                
                <table>                     
                    <tr>
                        <th align="left">
                            <asp:Label runat="server" Text="Oppdater bruker"></asp:Label>    
                        </th>
                    </tr>
                    <tr>                                                
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Username %>" CssClass="TipText"/>                    
                            <br />
                            <asp:TextBox runat="server" ID="tbUserNameE" ReadOnly="true" Enabled="false" CssClass="TextBoxClass2x"/>                
                        </td>
                    </tr>                                
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, FullName %>" CssClass="TipText" />                
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbNameE" CssClass="TextBoxClass2x" MaxLength="127" />                                                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbNameEFilter" 
                                TargetControlID="tbNameE" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" 
                                ValidChars="ÆØÅæøå-, " />
                        </td>                        
                    </tr>                                            
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, OrgContact %>" CssClass="TipText"/>
                            <br />
                            <asp:TextBox runat="server" ID="tbContactE" CssClass="TextBoxClass2x" MaxLength="95"/>                                
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbContactEFilter" 
                                TargetControlID="tbContactE" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-, " />
                        </td>                        
                    </tr>                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Address_reminder %>" CssClass="TipText" />                
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbAddressE" CssClass="TextBoxClass2x" MaxLength="255"/>                                                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbAddressEFilter" 
                                TargetControlID="tbAddressE" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-, " />
                        </td>                        
                    </tr>                                         
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Postal %>" CssClass="TipText"/>                
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbPostalE" CssClass="TextBoxClass2x" MaxLength="95"/>                                                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbPostalEFilter" 
                                TargetControlID="tbPostalE" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå- " />
                        </td>                        
                    </tr>                                                                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Email %>" CssClass="TipText"/>                
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbEmailE" CssClass="TextBoxClass2x" MaxLength="95"/>                             
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbEmailEFilter" 
                                TargetControlID="tbEmailE" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="@.!#$%&'*+-/=?^_`{|}~" />                               
                        </td>                    
                    </tr>                                                                                                                                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Phone %>" CssClass="TipText" />
                            <br />
                            <asp:TextBox runat="server" ID="tbPhoneE" CssClass="TextBoxClass2x" MaxLength="15"/> 
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbPhoneEFilter" 
                                TargetControlID="tbPhoneE" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " />                               
                        </td>                        
                    </tr>                                                                                                                                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Mobile %>" CssClass="TipText" />
                            <br />
                            <asp:TextBox runat="server" ID="tbMobileE" CssClass="TextBoxClass2x" MaxLength="15"/> 
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbMobileEFilter" 
                                TargetControlID="tbMobileE" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " />                               
                        </td>                    
                    </tr>                                                                                                                                                                                                                                                                      
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Fax %>" CssClass="TipText" />
                            <br />
                            <asp:TextBox runat="server" ID="tbFaxE" CssClass="TextBoxClass2x" MaxLength="15"/>                                
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbFaxEFilter" 
                                TargetControlID="tbFaxE" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " />
                        </td>                    
                    </tr>                                                                                        
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Website %>" CssClass="TipText" />
                            <br />
                            <asp:TextBox runat="server" ID="tbWebsiteE" CssClass="TextBoxClass2x" MaxLength="255"/>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbWebsiteEFilter" 
                                TargetControlID="tbWebsiteE" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="-_/.:" />                                
                        </td>                        
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Button ID="buttonAccountE" runat="server" 
                                Text="<%$ Resources:Localization, Update %>" 
                                OnClick="buttonAccountE_OnClick" 
                                CssClass="ButtonClass" Width="154px"/>
                        </td>
                    </tr>                                                                                                                                                                                                
                </table>    
                
                </td>
                <td style="margin-left:50px" valign="top">
                
                    <table>
                        <tr>
                            <th align="left">Bytte av passord</th>                            
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Password_validchars %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Old %>" />
                                <br />
                                <asp:TextBox ID="tbOldPassword" runat="server" TextMode="Password" MaxLength="80" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbOldPasswordFilter" 
                                    TargetControlID="tbOldPassword" 
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="ÆØÅæøå_" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, New %>" />
                                <br />
                                <asp:TextBox ID="tbNewPassword" runat="server" TextMode="Password" MaxLength="80" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbNewPasswordFilter" 
                                    TargetControlID="tbNewPassword" 
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="ÆØÅæøå_" />                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, RepeatNew %>" />
                                <br />
                                <asp:TextBox ID="tbNewPassword2" runat="server" TextMode="Password" MaxLength="80" CssClass="TextBoxClass2x"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbNewPassword2Filter" 
                                    TargetControlID="tbNewPassword2" 
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="ÆØÅæøå_" />
                            </td>
                        </tr>
                        <tr>                    
                            <td>
                                <asp:Button ID="buttonChangePassword" runat="server" 
                                    Text="<%$ Resources:Localization, ChangePassword %>" 
                                    OnClick="buttonChangePassword_OnClick" 
                                    CssClass="ButtonClass" Width="144px"/>                                
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
            </ContentTemplate>
            </asp:UpdatePanel>    
    
    </ContentTemplate>
    </act:TabPanel>
    
    <act:TabPanel ID="tabActors" runat="server" HeaderText="<%$ Resources:Localization, Organizations_internal_users %>">        
    <ContentTemplate>
        
        
        <asp:UpdatePanel ID="updatePanelActors" runat="server">
        <ContentTemplate>
    
            <div style="float:right;margin:8px">                            
                        
                <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelActors" DisplayAfter="0">
                    <ProgressTemplate>
                        <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                    </ProgressTemplate>
                </asp:UpdateProgress>        
                    
            </div>
    
            <div style="margin:8px">                                                        
        
            <table cellspacing="0" cellpadding="0" width="100%">                
            <tr>
                <th align="left" colspan="2">
                    <asp:Label runat="server" Text="<%$ Resources:Localization, Actors_header %>"></asp:Label>            
                </th>
            </tr>
            <tr>
                <td style="height:16px" colspan="2"></td>
            </tr>
            <tr>
            <td valign="top">
            
                <table> 
                    <tr>                        
                        <th align="left">
                            <asp:Label runat="server" Text="<%$ Resources:Localization, CreateActor %>" />
                        </th>        
                    </tr>           
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Name %>" CssClass="TipText"></asp:Label>                    
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxName" runat="server" CssClass="TextBoxClass2x" MaxLength="95"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxNameFilter" 
                                TargetControlID="textBoxName" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-, " />
                        </td>                                
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, EMail %>" CssClass="TipText"></asp:Label>                    
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxEmail" runat="server" CssClass="TextBoxClass2x" MaxLength="95"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxEmailFilter" 
                                TargetControlID="textBoxEmail" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="@._" /> 
                        </td>                                
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Phone %>" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxPhone" runat="server" CssClass="TextBoxClass2x" MaxLength="15"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxPhoneFilter" 
                                TargetControlID="textBoxPhone" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " />  
                        </td>                
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Mobile %>" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxMobile" runat="server" CssClass="TextBoxClass2x" MaxLength="15"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxMobileFilter" 
                                TargetControlID="textBoxMobile" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " /> 
                        </td>                                
                    </tr>            
                    <tr>                        
                        <td>
                            <asp:Button ID="buttonCreateActor" runat="server" 
                                Text="<%$ Resources:Localization, CreateActor %>" 
                                OnClick="buttonCreateActor_OnClick" 
                                CssClass="ButtonClass" Width="154px"/>
                        </td>
                    </tr>
                </table>        
        
            </td>        
            <td valign="top">
                
                <table>            
                    <tr>                        
                        <th align="left">
                            <asp:Label runat="server" Text="<%$ Resources:Localization, ExistingActors %>" />
                        </th>                    
                    </tr>
                    <tr>                        
                        <td>
                            <asp:DropDownList ID="ddActors" runat="server" 
                                CssClass="TextBoxClass2x"
                                AutoPostBack="true" 
                                DataSourceID="dataSourceActors" 
                                DataTextField="vchName" 
                                DataValueField="ID" 
                                OnSelectedIndexChanged="ddActors_OnSelectedIndexChanged" 
                                OnDataBound="ddActors_OnDataBound"/>
                        </td>    
                    </tr>
                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Name %>" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxNameUpd" runat="server" CssClass="TextBoxClass2x" MaxLength="95"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxNameUpdFilter" 
                                TargetControlID="textBoxNameUpd" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-, " />
                        </td>                
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Email %>" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxEmailUpd" runat="server" CssClass="TextBoxClass2x" MaxLength="95"></asp:TextBox>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxEmailUpdFilter" 
                                TargetControlID="textBoxEmailUpd" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="@.!#$%&'*+-/=?^_`{|}~" /> 
                        </td>                
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Phone %>" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxPhoneUpd" runat="server" CssClass="TextBoxClass2x" MaxLength="15"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxPhoneUpdFilter" 
                                TargetControlID="textBoxPhoneUpd" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " />
                        </td>                
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Mobile %>" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxMobileUpd" runat="server" CssClass="TextBoxClass2x" MaxLength="15"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxMobileUpdFilter" 
                                TargetControlID="textBoxMobileUpd" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " /> 
                        </td>                
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="<%$ Resources:Localization, Status %>" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddStatus" runat="server" 
                                CssClass="TextBoxClass2x"
                                DataSourceID="dataSourceStatus" 
                                DataTextField="text" 
                                DataValueField="value"/>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Button ID="buttonUpdateActor" runat="server" 
                                Text="<%$ Resources:Localization, UpdateActor %>" 
                                OnClick="buttonUpdateActor_OnClick" 
                                CssClass="ButtonClass" Width="154px"/>                            
                        </td>
                    </tr>
                </table>                            
            </td>
            </tr>
            </table>
               
            <br />                                
            
            <center>
                <asp:Label ID="labelStatusActors" runat="server" CssClass="StatusMessageClass" />
            </center>
                                    
            <asp:SqlDataSource ID="dataSourceActors" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                SelectCommand="SELECT ID, vchName FROM Contact WHERE AccountID = @accountID"                        
                ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                                                                                
            </asp:SqlDataSource> 
                
            <asp:XmlDataSource ID="dataSourceStatus" runat="server" DataFile="~/App_Data/ContactStatus.xml"></asp:XmlDataSource>                                                                                                                                                           

            </div>                            
        </ContentTemplate>
        </asp:UpdatePanel>
    
        </ContentTemplate>        
        </act:TabPanel>
        
    </act:TabContainer>
        
</asp:Content>

