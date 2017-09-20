<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Accounts.aspx.cs" Inherits="Accounts" Title="LorakonAdmin - Kontoer" %>

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

    <act:TabContainer ID="tabContainerAccounts" runat="server" OnClientActiveTabChanged="OnTabChanged">            
        
        <act:TabPanel ID="tabCreate" runat="server" HeaderText="Opprett konto">
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
    
                <asp:HiddenField ID="hiddenPendingUser" runat="server" Visible="false"/>
                
                <table cellspacing="2" width="100%">
                    <tr>
                        <th align="left">Opprett en ny brukerkonto</th>                        
                    </tr>                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Bestillte brukerkontoer" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:DropDownList ID="ddPendingUsers" runat="server" 
                                AutoPostBack="true" 
                                CssClass="TextBoxClass2x"
                                DataSourceID="dataSourcePendingAccounts" 
                                DataTextField="vchName" 
                                DataValueField="ID" 
                                OnSelectedIndexChanged="ddPendingUsers_OnSelectedIndexChanged" OnDataBound="ddPendingUsers_OnDataBound"/>                        
                        </td>
                    </tr>       
                    <tr>
                        <td style="height:16px"></td>
                    </tr>                                 
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Brukernavn" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbUserName" CssClass="TextBoxClass2x" MaxLength="255"/>                                                        
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbUserNameFilter" 
                                TargetControlID="tbUserName" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå_" />
                        </td>                        
                    </tr>                    
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Passord. Gyldige tegn er små/store bokstaver, tall og _" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbPassword" TextMode="Password" CssClass="TextBoxClass2x" MaxLength="80"/>                                                                                    
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbPasswordFilter" 
                                TargetControlID="tbPassword" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå_" />
                        </td>                        
                    </tr>                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Gjenta passord. Gyldige tegn er små/store bokstaver, tall og _" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbConfirmPassword" TextMode="Password" CssClass="TextBoxClass2x" MaxLength="80"/>                                                        
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbConfirmPasswordFilter" 
                                TargetControlID="tbConfirmPassword" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå_" />
                        </td>                        
                    </tr>                                            
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Fullt navn på organisasjonen" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbName" CssClass="TextBoxClass2x" MaxLength="127"/>                                                                                    
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbNameFilter" 
                                TargetControlID="tbName" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" 
                                ValidChars="ÆØÅæøå+-,./ " />
                        </td>                        
                    </tr>                                            
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Organisasjonens LORAKON kontaktperson" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox runat="server" ID="tbContact" CssClass="TextBoxClass2x" MaxLength="95"/>                                                        
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbContactFilter" 
                                TargetControlID="tbContact" 
                                FilterType="Custom, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-, " />
                        </td>                        
                    </tr>                                            
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Organisasjonens adresse" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbAddress" CssClass="TextBoxClass2x" MaxLength="255"/>                                                                                    
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbAddressFilter" 
                                TargetControlID="tbAddress" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-, " />
                        </td>                        
                    </tr>                                                                                                        
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Organisasjonens poststed" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbPostal" CssClass="TextBoxClass2x" MaxLength="95"/>                                                                                    
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbPostalFilter" 
                                TargetControlID="tbPostal" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå- " />
                        </td>                        
                    </tr>                                                                                                            
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Organisasjonens e-post" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbEmail" CssClass="TextBoxClass2x" MaxLength="95"/>                                                                                    
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbEmailFilter" 
                                TargetControlID="tbEmail" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="@.!#$%&'*+-/=?^_`{|}~" />
                        </td>                        
                    </tr>                                                                                                                                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Organisasjonens telefon" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox runat="server" ID="tbPhone" CssClass="TextBoxClass2x" MaxLength="15"/>                                                        
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbPhoneFilter" 
                                TargetControlID="tbPhone" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " />
                        </td>                        
                    </tr>                                                                                                                                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Organisasjonens mobiltelefon" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox runat="server" ID="tbMobile" CssClass="TextBoxClass2x" MaxLength="15"/>                                                        
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbMobileFilter" 
                                TargetControlID="tbMobile" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " />
                        </td>                        
                    </tr>                                                                                                                                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Organisasjonens fax" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox runat="server" ID="tbFax" CssClass="TextBoxClass2x" MaxLength="15"/>                                                        
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbFaxFilter" 
                                TargetControlID="tbFax" 
                                FilterType="Custom, Numbers" 
                                ValidChars="+ " />
                        </td>                        
                    </tr>                                                                                        
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Organisasjonens nett-adresse" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox runat="server" ID="tbWebsite" CssClass="TextBoxClass2x" MaxLength="255"/>                                                        
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbWebsiteFilter" 
                                TargetControlID="tbWebsite" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="-_/.:" />
                        </td>                        
                    </tr>                                                                                                                                                        
                    <tr>
                        <td>
                            <asp:Button ID="buttonCreateAccount" runat="server" 
                                Text="Opprett konto og send e-post" 
                                OnClick="buttonCreateAccount_OnClick" 
                                CssClass="ButtonClass"/>
                                
                            <asp:Button ID="buttonDeletePendingAccount" runat="server" 
                                Text="Slett bestillt konto" 
                                OnClick="buttonDeletePendingAccount_OnClick" 
                                CssClass="ButtonClass"/>
                        </td>
                    </tr>
                </table>
                                
                <br />
                
                <center><asp:Label ID="labelStatusCreate" runat="server" CssClass="StatusMessageClass" /></center>
                
                <asp:SqlDataSource ID="dataSourcePendingAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT ID, vchName FROM PendingAccount ORDER BY vchName ASC"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource>                                                       
              
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
            
            </ContentTemplate>
        </act:TabPanel>
        
        <act:TabPanel ID="tabEdit" runat="server" HeaderText="Rediger konto">
            <ContentTemplate>                            
                
            <asp:UpdatePanel ID="updatePanelEdit" runat="server">
            <ContentTemplate>
            
            <div style="float:right;margin:8px">
                <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelEdit" DisplayAfter="0">
                    <ProgressTemplate>
                        <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            
            <div style="margin:8px">
            
                <asp:HiddenField ID="hiddenName" runat="server" Visible="false"/>
                
                <table cellspacing="2" width="100%">
                    <tr>
                        <th align="left">Rediger en brukerkonto</th>                        
                    </tr>                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Velg bruker" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:DropDownList ID="ddUsers" runat="server" 
                                CssClass="TextBoxClass2x" 
                                AutoPostBack="true" 
                                DataSourceID="dataSourceAccounts" 
                                DataTextField="vchName" 
                                DataValueField="ID" 
                                OnSelectedIndexChanged="ddUsers_OnSelectedIndexChanged" 
                                OnDataBound="ddUsers_OnDataBound" />
                        </td>
                        <td>
                            <asp:Label ID="lblAccountID" runat="server" CssClass="TipText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="16px"></td>
                    </tr>                        
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Brukernavn" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="tbUserNameE" ReadOnly="true" Enabled="false" CssClass="TextBoxClass2x" MaxLength="255"/>                                                            
                        </td>                            
                        <td>
                            <asp:Label runat="server" Text="Nytt passord" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="tbNewPassword" runat="server" MaxLength="80" CssClass="TextBoxClass2x"></asp:TextBox>                                                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbNewPasswordFilter" 
                                TargetControlID="tbNewPassword" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå_" />                            
                                
                            <asp:Button ID="buttonNewPassword" runat="server" 
                                Text="Generer nytt passord" 
                                OnClick="buttonNewPassword_OnClick" 
                                CssClass="ButtonClass"/>                                
                        </td>
                    </tr>                        
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Fullt navn på organisasjonen" CssClass="TipText"></asp:Label>
                                <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:TextBox runat="server" ID="tbNameE" CssClass="TextBoxClass2x" MaxLength="127"/>                                                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbNameEFilter" 
                                    TargetControlID="tbNameE" 
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" 
                                    ValidChars="ÆØÅæøå+-,./ " />
                            </td>                            
                        </tr>                                            
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Organisasjonens LORAKON kontaktperson" CssClass="TipText"></asp:Label>
                                <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
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
                                <asp:Label runat="server" Text="Organisasjonens adresse" CssClass="TipText"></asp:Label>
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
                                <asp:Label runat="server" Text="Organisasjonens poststed" CssClass="TipText"></asp:Label>
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
                                <asp:Label runat="server" Text="Organisasjonens e-post" CssClass="TipText"></asp:Label>
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
                                <asp:Label runat="server" Text="Organisasjonens telefon" CssClass="TipText"></asp:Label>                                
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
                                <asp:Label runat="server" Text="Organisasjonens mobiltelefon" CssClass="TipText"></asp:Label>                                
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
                                <asp:Label runat="server" Text="Organisasjonens fax" CssClass="TipText"></asp:Label>                                
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
                                <asp:Label runat="server" Text="Organisasjonens nett-adresse" CssClass="TipText"></asp:Label>                                
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
                                <asp:CheckBox runat="server" ID="cbActiveE" Text="Merk av hvis kontoen er i bruk" CssClass="TipText"/>
                            </td>                            
                        </tr>                                                                                                                                        
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="Kommentarfelt" CssClass="TipText"></asp:Label>                                
                                <br />
                                <asp:TextBox ID="tbCommentE" runat="server" CssClass="TextBoxClass2x"/>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbCommentEFilter" 
                                    TargetControlID="tbCommentE" 
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="ÆØÅæøå-_,.:()!?% " />
                            </td>                            
                        </tr>   
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="Sist registrert for ringtest: " CssClass="TipText"></asp:Label>
                                <asp:Label ID="tbLastRegistrationE" runat="server" CssClass="TipText"/>
                            </td>
                        </tr>                                                                                                                                                             
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="Deltatt i antall ringtester: " CssClass="TipText"></asp:Label>
                                <asp:Label ID="tbRingtestCountE" runat="server" CssClass="TipText"/>
                            </td>
                        </tr>                        
                    </table>
                    
                    <br />
                    
                    <table cellspacing="2">
                    <tr>
                        <td>
                            <asp:Button ID="buttonAccountE" runat="server" Text="Oppdater" OnClick="buttonAccountE_OnClick" CssClass="ButtonClass"/>
                        </td>                            
                        <td>
                            <asp:Button ID="buttonAccountRegisterE" runat="server" Text="Etter-registrer for årets ringtest" OnClick="buttonAccountRegisterE_OnClick" CssClass="ButtonClass"/>
                        </td>
                        <td>
                            <asp:Button ID="buttonAccountUnregisterE" runat="server" Text="Avregistrer og slett årets rapporter" OnClick="buttonAccountUnregisterE_OnClick" CssClass="ButtonClass"/>
                        </td>
                    </tr>
                    </table>                    
                    <br />
                    
                    <center><asp:Label ID="labelStatusEdit" runat="server" CssClass="StatusMessageClass" /></center>
                    
                    <asp:SqlDataSource ID="dataSourceAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                        SelectCommand="SELECT ID, vchName FROM Account ORDER BY vchName ASC"
                        ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                    </asp:SqlDataSource>                                                             
                                    
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                    
            </ContentTemplate>
            
        </act:TabPanel>                                
        
        <act:TabPanel ID="tabAssets" runat="server" HeaderText="Konto rekvisita">
            <ContentTemplate>                        
            
            <asp:UpdatePanel ID="updatePanelAssets" runat="server">
            <ContentTemplate>                        
            
            <div style="float:right;margin:8px">
                <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelAssets" DisplayAfter="0">
                    <ProgressTemplate>
                        <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            
            <div style="margin:8px">
            
                <br />
                <table cellspacing="2">
                <tr>
                    <th align="left">Utstyrslister</th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Velg en konto for å se utstyrsliste" CssClass="TipText"></asp:Label>
                    </td>
                </tr>
                <tr> 
                    <td>                   
                        <asp:DropDownList ID="ddAccountsA" runat="server" AutoPostBack="true" CssClass="TextBoxClass2x"
                            DataSourceID="dataSourceAccountsA" 
                            DataTextField="vchName" 
                            DataValueField="ID" 
                            OnSelectedIndexChanged="ddAccountsA_OnSelectedIndexChanged" 
                            OnDataBound="ddAccountsA_OnDataBound"></asp:DropDownList>                
                    </td>                    
                </tr>
                </table>                
                <br />                                
                
                <asp:GridView ID="gridDevicesA" runat="server" 
                    Font-Size="X-Small"
                    AllowSorting="true"
                    GridLines="None" 
                    Width="100%" 
                    EmptyDataText="<Ingen enheter funnet>"
                    AutoGenerateColumns="false" 
                    DataKeyNames="ID" 
                    DataSourceID="dataSourceDevicesA">
                    
                    <HeaderStyle HorizontalAlign="Left" />
                    
                    <Columns>                               
                        <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="vchSerialNumber" SortExpression="vchSerialNumber" HeaderText="Serienummer" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="categoryName" SortExpression="categoryName" HeaderText="Kategori" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="typeName" SortExpression="typeName" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="vchStatus" SortExpression="vchStatus" HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="vchOwnership" SortExpression="vchOwnership" HeaderText="Eierskap" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="textComment" SortExpression="textComment" HeaderText="Kommentar" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="dateReceivedNew" SortExpression="dateReceivedNew" HeaderText="Mottatt Dato" DataFormatString="{0:dd.MM.yyyy}" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>                        
                    </Columns>
                </asp:GridView>                
                <br />
                
                <center><asp:Label ID="labelStatusAssets" runat="server" CssClass="StatusMessageClass" /></center>
                
                <asp:SqlDataSource ID="dataSourceAccountsA" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT ID, vchName FROM Account ORDER BY vchName ASC"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource> 
                
                <asp:SqlDataSource ID="dataSourceDevicesA" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"                                        
                    SelectCommand="SELECT Device.ID, Device.vchSerialNumber, DeviceCategory.vchName AS categoryName, DeviceType.vchName AS typeName, Device.vchStatus, Device.vchOwnership, Device.textComment, Device.dateReceivedNew FROM Device, DeviceCategory, DeviceType WHERE Device.AccountID = @accountID AND DeviceCategory.ID = Device.DeviceCategoryID AND DeviceType.ID = Device.DeviceTypeID"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">
                    <SelectParameters>
                        <asp:ControlParameter Name="accountID" ControlID="ddAccountsA" PropertyName="SelectedValue"/>                        
                    </SelectParameters>                     
                </asp:SqlDataSource>                 
                                
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
        </act:TabPanel>
        
    </act:TabContainer>    
    
</asp:Content>

