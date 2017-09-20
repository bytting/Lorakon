<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Units.aspx.cs" Inherits="Units" Title="LorakonAdmin - Enheter" %>

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
                
    <act:TabContainer ID="tabContainerUnits" runat="server" OnClientActiveTabChanged="OnTabChanged">            
                
        <act:TabPanel ID="tabCategories" runat="server" HeaderText="Opprett enhets kategorier og typer">
        
            <ContentTemplate>
                      
                <asp:UpdatePanel ID="updatePanelCategories" runat="server">
                <ContentTemplate>
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelCategories" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">
                
                <table width="100%">
                    <tr>
                        <th align="left">Oppretting av enhets kategorier og typer</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbNewCategory" runat="server" CssClass="TextBoxClass2x" MaxLength="63" />
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbNewCategoryFilter" 
                                TargetControlID="tbNewCategory" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå,()+-_/ " />
                                
                            <asp:Button ID="buttonNewCategory" runat="server" 
                                Text="Opprett kategori" 
                                OnClick="buttonNewCategory_OnClick" 
                                CssClass="ButtonClass" Width="150px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gridCategories" runat="server" 
                                Font-Size="X-Small" 
                                AllowSorting="true"                    
                                GridLines="None" 
                                AutoGenerateColumns="false" 
                                DataKeyNames="ID" 
                                DataSourceID="dataSourceCategories" 
                                OnRowEditing="gridCategories_OnEditCommand" 
                                OnRowCancelingEdit="gridCategories_OnCancelCommand" 
                                OnRowUpdating="gridCategories_OnUpdateCommand" 
                                OnRowDeleting="gridCategories_OnDeleteCommand" 
                                CellSpacing="2">                                         
                                     
                                <HeaderStyle HorizontalAlign="Left" />
                                               
                                <Columns>                               
                                    <asp:BoundField DataField="vchName" SortExpression="vchName" HeaderText="Kategori" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:CommandField ShowEditButton="true" HeaderStyle-HorizontalAlign="Left"/>                                                
                                </Columns>           
                                         
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="height:16px"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddCategories" runat="server"                    
                                AutoPostBack="true" 
                                DataSourceID="dataSourceCategories" 
                                DataTextField="vchName" 
                                DataValueField="ID" 
                                OnSelectedIndexChanged="ddCategories_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbNewType" runat="server" CssClass="TextBoxClass2x" MaxLength="63" />
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbNewTypeFilter" 
                                TargetControlID="tbNewType" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå,()+-_/ " />
                                
                            <asp:Button ID="buttonNewType" runat="server" 
                                Text="Opprett type" 
                                OnClick="buttonNewType_OnClick" 
                                CssClass="ButtonClass" Width="150px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gridTypes" runat="server" 
                                Font-Size="X-Small"
                                AllowSorting="true"                    
                                GridLines="None" 
                                AutoGenerateColumns="false" 
                                DataKeyNames="ID" 
                                DataSourceID="dataSourceTypes" 
                                OnRowEditing="gridTypes_OnEditCommand" 
                                OnRowCancelingEdit="gridTypes_OnCancelCommand" 
                                OnRowUpdating="gridTypes_OnUpdateCommand" 
                                OnRowDeleting="gridTypes_OnDeleteCommand" 
                                CellSpacing="2">                    
                                
                                <HeaderStyle HorizontalAlign="Left" />
                                
                                <Columns>                               
                                    <asp:BoundField DataField="vchName" SortExpression="vchName" HeaderText="Type" HeaderStyle-HorizontalAlign="Left" />                        
                                    <asp:CommandField ShowEditButton="true" HeaderStyle-HorizontalAlign="Left"/>                                                
                                </Columns>
                                
                            </asp:GridView>                                                                
                        </td>
                    </tr>
                </table>                                                                                                                                                                                                                                                                                                
                
                <asp:SqlDataSource ID="dataSourceCategories" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM DeviceCategory ORDER BY vchName ASC"
                    UpdateCommand="UPDATE DeviceCategory SET vchName = @vchName WHERE ID = @ID"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource> 
                
                <asp:SqlDataSource ID="dataSourceTypes" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM DeviceType ORDER BY vchName ASC"
                    UpdateCommand="UPDATE DeviceType SET vchName = @vchName WHERE ID = @ID"                    
                    FilterExpression="DeviceCategoryID='{0}'"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                                                
                    <FilterParameters>
                        <asp:ControlParameter Name="DeviceCategoryID" ControlId="ddCategories" PropertyName="SelectedValue" />
                    </FilterParameters>                    
                </asp:SqlDataSource> 
        
                <br />
                <center><asp:Label ID="labelStatusCategories" runat="server" CssClass="StatusMessageClass" /></center>
        
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
            
        </act:TabPanel>
        
        <act:TabPanel ID="tabCreate" runat="server" HeaderText="Opprett enheter og ringtestbokser">
        
            <ContentTemplate>
            
                <asp:UpdatePanel ID="updatePanelCreate" runat="server">
                <ContentTemplate>
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelCreate" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">                                                                
                
                <table cellpadding="0" cellspacing="2" width="100%" style="text-align:left">
                    <tr>
                        <th align="left">Oppretting av enheter</th>
                    </tr>                    
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Kategori" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:DropDownList ID="ddCategoriesCD" runat="server" 
                                CssClass="TextBoxClass2x" 
                                AutoPostBack="true" 
                                DataSourceID="dataSourceCategoriesCD" 
                                DataTextField="vchName" 
                                DataValueField="ID" 
                                OnSelectedIndexChanged="ddCategoriesCD_OnSelectedIndexChanged">
                            </asp:DropDownList>                                
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Type" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:DropDownList ID="ddTypesCD" runat="server" 
                                CssClass="TextBoxClass2x"
                                DataSourceID="dataSourceTypesCD" 
                                DataTextField="vchName" 
                                DataValueField="ID">
                            </asp:DropDownList>                
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Serienummer" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbSerialnumberCD" runat="server" CssClass="TextBoxClass2x" MaxLength="63"/>                            
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbSerialnumberCDFilter" 
                                TargetControlID="tbSerialnumberCD" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå,.()-_/ " />                                
                                
                            <asp:Button ID="buttonGenerateSerialnumberCD" runat="server" 
                                Text="Generer serienummer" 
                                OnClick="buttonGenerateSerialnumberCD_OnClick" 
                                CssClass="ButtonClass"/>
                        </td>
                    </tr>                           
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Status" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:DropDownList ID="ddStatusCD" runat="server" 
                                CssClass="TextBoxClass2x"
                                DataSourceID="dataSourceStatusCD" 
                                DataTextField="text" 
                                DataValueField="value"/>                                                
                        </td>
                    </tr>                                                            
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Eies av" CssClass="TipText"></asp:Label>                                      
                            <br />
                            <asp:DropDownList ID="ddOwnershipCD" runat="server" 
                                CssClass="TextBoxClass2x"
                                DataSourceID="dataSourceOwnershipCD" 
                                DataTextField="text" 
                                DataValueField="value"/>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Mottatt av Strålevernet" CssClass="TipText"></asp:Label>                            
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>                  
                            <br />
                            <asp:TextBox ID="tbReceivedNewCD" runat="server" CssClass="TextBoxClass2x" />                            
                            <act:CalendarExtender ID="calReceivedNewCD" runat="server" 
                                TargetControlID="tbReceivedNewCD" 
                                Format="dd.MM.yyyy" />
                        </td>
                    </tr>                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Besittes av" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:DropDownList ID="ddAccountCD" runat="server" 
                                CssClass="TextBoxClass2x"
                                DataSourceID="dataSourceAccounts" 
                                DataTextField="vchName" 
                                DataValueField="ID" />                            
                        </td>
                    </tr>                    
                    <tr>                        
                        <td>                            
                            <asp:Button ID="buttonCreateCD" runat="server" 
                                Text="Opprett enhet" 
                                OnClick="buttonCreateCD_OnClick" 
                                CssClass="ButtonClass" Width="150px"/>
                        </td>
                    </tr>                    
                </table>    
                
                <asp:SqlDataSource ID="dataSourceAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT ID, vchName FROM Account ORDER BY vchName ASC"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource> 
                
                <asp:SqlDataSource ID="dataSourceCategoriesCD" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM DeviceCategory ORDER BY vchName ASC"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource> 
                
                <asp:SqlDataSource ID="dataSourceTypesCD" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM DeviceType ORDER BY vchName ASC"                    
                    FilterExpression="DeviceCategoryID='{0}'"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                                                
                    <FilterParameters>
                        <asp:ControlParameter Name="DeviceCategoryID" ControlId="ddCategoriesCD" PropertyName="SelectedValue" />
                    </FilterParameters>
                </asp:SqlDataSource>      
                
                <asp:XmlDataSource ID="dataSourceStatusCD" runat="server" DataFile="~/App_Data/DeviceStatus.xml" />
                <asp:XmlDataSource ID="dataSourceOwnershipCD" runat="server" DataFile="~/App_Data/DeviceOwnership.xml" />
                
                <br />
                <hr />                
                <br />
                 
                 <!-- Bokser -->                                                                                                     
                 
                 <table cellpadding="0" cellspacing="2" width="100%">
                    <tr>
                        <th align="left" colspan="4">
                            Oppretting og redigering av ringtestbokser
                        </th>                        
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <asp:Label runat="server" Text="Usikkerhet oppgis ved 95% konfidensinterval" ForeColor="DarkOrange" Font-Size="XX-Small" />   
                        </td>                        
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="K-Nummer" CssClass="TipText" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbKNumber" runat="server" CssClass="TextBoxClass2x" MaxLength="31"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbKNumberFilter" 
                                TargetControlID="tbKNumber" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars=".-_ " />
                        </td>                    
                        <td align="right">
                            <asp:Label runat="server" Text="Ekstern ID" CssClass="TipText"/>
                        </td>
                        <td>
                            <asp:TextBox ID="tbExternID" runat="server" CssClass="TextBoxClass2x" MaxLength="31"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbExternIDFilter" 
                                TargetControlID="tbExternID" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars=".-_ " />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Ref. Dato" CssClass="TipText" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbRefDate" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>
                            <act:CalendarExtender ID="calRefDate" runat="server" 
                                TargetControlID="tbRefDate" 
                                Format="dd.MM.yyyy" />
                        </td>                    
                        <td align="right">
                            <asp:Label runat="server" Text="Ref. Verdi" CssClass="TipText" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbRefValue" runat="server" CssClass="TextBoxClass2x" />
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbRefValueFilter" 
                                TargetControlID="tbRefValue" 
                                FilterType="Custom, Numbers" 
                                ValidChars="," />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Vekt" CssClass="TipText" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbWeight" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbWeightFilter" 
                                TargetControlID="tbWeight" 
                                FilterType="Custom, Numbers" 
                                ValidChars="," />
                        </td>                    
                        <td align="right">
                            <asp:Label runat="server" Text="Status" CssClass="TipText" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddStatusAss" runat="server" 
                                CssClass="TextBoxClass2x"
                                DataSourceID="dataSourceStatusAss" 
                                DataTextField="text" 
                                DataValueField="value"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Usikkerhet" CssClass="TipText" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbUncertainty" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="tbUncertaintyFilter" 
                                TargetControlID="tbUncertainty" 
                                FilterType="Custom, Numbers" 
                                ValidChars="," />
                        </td>                                                                
                        <td align="right">
                            <asp:Label runat="server" Text="Kommentar" CssClass="TipText" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbComment" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>    
                        <td></td>                    
                        <td colspan="3">
                            <asp:Button ID="buttonCreateBox" runat="server" 
                                Text="Opprett boks" 
                                OnClick="buttonCreateBox_OnClick" 
                                CssClass="ButtonClass" Width="150px"/>
                        </td>                        
                    </tr>
                 </table>                                                                                                                                           
                   
                 <br /><br />                                    
                 
                 <asp:GridView ID="gridBoxes" runat="server" 
                     Font-Size="X-Small" 
                     AllowSorting="true"
                     Width="940px" 
                     GridLines="None" 
                     AutoGenerateColumns="false" 
                     DataKeyNames="ID" 
                     DataSourceID="dataSourceBoxes" 
                     OnRowEditing="gridBoxes_OnEditCommand" 
                     OnRowCancelingEdit="gridBoxes_OnCancelCommand" 
                     OnRowUpdating="gridBoxes_OnUpdateCommand">                    
                   
                    <HeaderStyle HorizontalAlign="Left" />
                    
                    <Columns>                               
                        <asp:BoundField DataField="vchKNumber" SortExpression="vchKNumber" HeaderText="K-Nummer" ItemStyle-Width="10%" ControlStyle-Width="100%" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="vchExternID" SortExpression="vchExternID" HeaderText="Ekstern ID" ItemStyle-Width="10%" ControlStyle-Width="100%" HeaderStyle-HorizontalAlign="Left"/>                        
                        <asp:TemplateField HeaderText="Ref. dato" ItemStyle-Width="10%" ControlStyle-Width="100%" SortExpression="dateRefDate" HeaderStyle-HorizontalAlign="Left">                            
                            <ItemTemplate>                                
                                <asp:Label ID="labelGridRefDate" runat="server" Text='<%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"dateRefDate")).ToShortDateString() %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbGridRefDate" runat="server"></asp:TextBox>
                                <act:CalendarExtender ID="calGridRefDate" runat="server" TargetControlID="tbGridRefDate" SelectedDate="<%#Bind('dateRefDate') %>" Format="dd.MM.yyyy"></act:CalendarExtender>
                            </EditItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Ref. Verdi" ItemStyle-Width="10%" ControlStyle-Width="100%" SortExpression="realRefValue" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="labelRefValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"realRefValue", "{0:0.00}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbRefValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"realRefValue", "{0:0.00}") %>'></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbRefValueFilter" 
                                    TargetControlID="tbRefValue" 
                                    FilterType="Numbers, Custom" ValidChars=","/>
                            </EditItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Usikk." ItemStyle-Width="10%" ControlStyle-Width="100%" SortExpression="realUncertainty" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="labelUncertainty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"realUncertainty", "{0:0.00}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbGridUncertainty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"realUncertainty", "{0:0.00}") %>'></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbGridUncertaintyFilter" 
                                    TargetControlID="tbGridUncertainty" 
                                    FilterType="Numbers, Custom" ValidChars=","/>
                            </EditItemTemplate>
                        </asp:TemplateField>                                                
                        <asp:TemplateField HeaderText="Vekt" ItemStyle-Width="10%" ControlStyle-Width="100%" SortExpression="realWeight" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="labelWeight" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"realWeight", "{0:0.00}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="tbWeight" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"realWeight", "{0:0.00}") %>'></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbWeightFilter" 
                                    TargetControlID="tbWeight" 
                                    FilterType="Numbers, Custom" ValidChars=","/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" ControlStyle-Width="100%" SortExpression="vchStatus" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="labelGridStatus" runat="server" Text="<%#Bind('vchStatus') %>"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddGridStatus" runat="server" DataSourceID="dataSourceStatusAss" DataTextField="text" DataValueField="value" SelectedValue="<%#Bind('vchStatus') %>"></asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="textComment" SortExpression="textComment" HeaderText="Kommentar" ControlStyle-Width="100%"/>                        
                        <asp:CommandField ShowEditButton="true" ItemStyle-Width="5%"/>                                                
                    </Columns>
                </asp:GridView>                                                                
                
                <asp:SqlDataSource ID="dataSourceBoxes" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM RingtestBox ORDER BY vchKNumber ASC"                    
                    UpdateCommand="UPDATE RingtestBox SET vchKNumber = @vchKNumber WHERE ID = @ID"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>" />                        
                
                <asp:XmlDataSource ID="dataSourceStatusAss" runat="server" DataFile="~/App_Data/DeviceStatus.xml"></asp:XmlDataSource>                                                                                   
    
                <br />
                <center><asp:Label ID="labelStatusCreate" runat="server" CssClass="StatusMessageClass" /></center>
                            
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
            
        </act:TabPanel>
        
        <act:TabPanel ID="tabEdit" runat="server" HeaderText="Rediger enheter">
        
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
                                                
                <asp:Panel ID="panelDeviceED" runat="server" Visible="false">                                
                
                <table cellpadding="0" cellspacing="2" width="100%">                                                        
                <tr>
                    <th align="left">Rediger enhet</th>
                </tr>                
                <tr>                    
                    <td>
                        <asp:Label runat="server" Text="Kategori" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddCategoriesEdit" runat="server" 
                            CssClass="TextBoxClass2x" 
                            AutoPostBack="true" 
                            DataSourceID="dataSourceCategoriesEdit" 
                            DataTextField="vchName" 
                            DataValueField="ID" 
                            OnSelectedIndexChanged="ddCategoriesCD_OnSelectedIndexChanged">
                            </asp:DropDownList>                
                    </td> 
                </tr>                
                <tr>                                   
                    <td>
                        <asp:Label runat="server" Text="Type" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddTypesEdit" runat="server" 
                            CssClass="TextBoxClass2x"
                            DataSourceID="dataSourceTypesEdit" 
                            DataTextField="vchName" 
                            DataValueField="ID">
                            </asp:DropDownList>
                    </td>
                </tr>                
                <tr>                    
                    <td>
                        <asp:Label runat="server" Text="Konto" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddAccountED" runat="server" 
                            CssClass="TextBoxClass2x"
                            DataSourceID="dataSourceAccountsED" 
                            DataTextField="vchName" 
                            DataValueField="ID" />
                    </td>                        
                </tr>
                <tr>                    
                    <td>
                        <asp:Label runat="server" Text="Serienummer" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbSerialnumberED" runat="server" CssClass="TextBoxClass2x" MaxLength="63"></asp:TextBox>                        
                        <act:FilteredTextBoxExtender runat="server" 
                            ID="tbSerialnumberEDFilter" 
                            TargetControlID="tbSerialnumberED" 
                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                            ValidChars="ÆØÅæøå,.()-_/ " />                            
                            
                        <asp:Button ID="buttonSerialnumberED" runat="server" 
                            Text="Generer serienummer" 
                            OnClick="buttonGenerateSerialnumberED_OnClick" 
                            CssClass="ButtonClass"/>
                    </td>                        
                </tr>                
                <tr>                        
                    <td>
                        <asp:Label runat="server" Text="Status" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddStatusED" runat="server" 
                            CssClass="TextBoxClass2x" 
                            DataSourceID="dataSourceStatusED" 
                            DataTextField="text" 
                            DataValueField="value" />
                    </td>                
                </tr>                
                <tr>                        
                    <td>
                        <asp:Label runat="server" Text="Eies av" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddOwnershipED" runat="server" 
                            CssClass="TextBoxClass2x" 
                            DataSourceID="dataSourceOwnershipED" 
                            DataTextField="text" 
                            DataValueField="value"/>
                    </td>
                </tr>
                <tr>                    
                    <td>
                        <asp:Label runat="server" Text="Kommentar" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbCommentED" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                        
                    </td>
                </tr>
                <tr>                    
                    <td>
                        <asp:Label runat="server" Text="Mottat av Strålevernet" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbReceivedNewED" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                        
                        <act:CalendarExtender ID="calReceivedNewED" runat="server" 
                            TargetControlID="tbReceivedNewED" 
                            Format="dd.MM.yyyy" />
                    </td>
                </tr>    
                <tr>                    
                    <td>
                        <asp:Button ID="buttonUpdateED" runat="server" 
                            Text="Oppdater" 
                            OnClick="buttonUpdateED_OnClick" 
                            CssClass="ButtonClass" Width="120px"/>                                                                                        
                    </td>
                </tr>                
                </table> 
                
                <asp:SqlDataSource ID="dataSourceCategoriesEdit" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM DeviceCategory ORDER BY vchName ASC"
                    UpdateCommand="UPDATE DeviceCategory SET vchName = @vchName WHERE ID = @ID"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource> 
                
                <asp:SqlDataSource ID="dataSourceTypesEdit" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM DeviceType ORDER BY vchName ASC"
                    UpdateCommand="UPDATE DeviceType SET vchName = @vchName WHERE ID = @ID"                    
                    FilterExpression="DeviceCategoryID='{0}'"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                                                
                    <FilterParameters>
                        <asp:ControlParameter Name="DeviceCategoryID" ControlId="ddCategoriesEdit" PropertyName="SelectedValue" />
                    </FilterParameters>                    
                </asp:SqlDataSource>                                 
                
                <br /><br />
                
                <table cellpadding="0" cellspacing="2" width="100%">                    
                    <tr>
                        <th align="left">
                            Legg til historie kommentar
                        </th>
                    </tr>
                    <tr>                    
                        <td style="width:80%">
                            <asp:TextBox ID="tbAddComment" runat="server" Width="100%" MaxLength="220" BorderStyle="Solid" BorderWidth="1px" />
                        </td>
                        <td style="width:20%">
                            <asp:Button ID="buttonAddComment" runat="server" 
                                Width="100%"
                                Text="Legg til" 
                                OnClick="buttonAddComment_OnClick" 
                                CssClass="ButtonClass" />
                        </td>                        
                    </tr>                    
                </table> 
                
                <table cellpadding="0" cellspacing="2" width="100%">
                    <tr>
                        <td style="width:80%">
                            <asp:Panel runat="server" Width="100%" Height="100px" ScrollBars="Vertical" BorderStyle="Solid" BorderWidth="1px">
                                <asp:GridView ID="gridDeviceHistory" runat="server" 
                                    Font-Size="X-Small" 
                                    Width="100%" 
                                    DataKeyNames="ID" 
                                    AutoGenerateColumns="false" 
                                    ShowHeader="false" 
                                    ShowFooter="false" 
                                    GridLines="None" 
                                    OnRowDeleting="gridDeviceHistory_RowDeleting">                                    
                                    <Columns>                                    
                                        <asp:BoundField DataField="dateCreated" SortExpression="dateCreated" ItemStyle-Wrap="false" DataFormatString="[{0:dd.MM.yyyy}]&nbsp;" HtmlEncode="false" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="vchComment" SortExpression="vchComment" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:CommandField ShowDeleteButton="true" DeleteText="Slett" ControlStyle-ForeColor="Red" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right"/>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>               
                        </td>
                    </tr>
                </table>                                                                                               
                
                <center>
                    <asp:Button ID="buttonCancelED" runat="server" Style="margin-top:3px"
                                Text="Avbryt redigering" 
                                OnClick="buttonCancelED_OnClick" 
                                CssClass="ButtonClass"/>
                </center>                                
                
                <br /><hr />
                
                </asp:Panel>                                                                                                                             
                
                <br />
                <table cellspacing="2" cellpadding="0" width="100%">
                <tr>
                    <th align="left">
                        Velg enhet som skal redigeres
                    </th>
                </tr>                
                <tr>
                    <td>
                        <asp:DropDownList ID="ddCategoriesED" runat="server"
                            CssClass="TextBoxClass2x" 
                            AutoPostBack="true" 
                            DataSourceID="dataSourceCategoriesED" 
                            DataTextField="vchName" 
                            DataValueField="ID" 
                            OnSelectedIndexChanged="ddCategoriesED_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddTypesED" runat="server" 
                            CssClass="TextBoxClass2x"
                            AutoPostBack="true" 
                            DataSourceID="dataSourceTypesED" 
                            DataTextField="vchName" 
                            DataValueField="ID" 
                            OnSelectedIndexChanged="ddTypesED_OnSelectedIndexChanged" />
                    </td>
                </tr>
                </table>                                
                
                <asp:Panel runat="server" Width="100%" Height="460px" ScrollBars="Vertical">
                    <asp:GridView ID="gridDevicesED" runat="server" Font-Size="X-Small"
                        AllowSorting="true"                    
                        Width="96%" 
                        GridLines="None" 
                        AutoGenerateColumns="false" 
                        DataKeyNames="ID" 
                        DataSourceID="dataSourceDevicesED"                     
                        OnSelectedIndexChanged="gridDevicesED_OnSelectedIndexChanged"
                        OnRowDataBound="gridDevicesED_OnRowDataBound"> 
                        
                        <HeaderStyle HorizontalAlign="Left" />
                                           
                        <Columns>                               
                            <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false" />
                            <asp:BoundField DataField="vchSerialNumber" SortExpression="vchSerialNumber" HeaderText="Serienummer" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="vchStatus" SortExpression="vchStatus" HeaderText="Status" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="vchOwnership" SortExpression="vchOwnership" HeaderText="Eierskap" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="textComment" SortExpression="textComment" HeaderText="Kommentar" HeaderStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="dateReceivedNew" SortExpression="dateReceivedNew" HeaderText="Mottatt Dato" DataFormatString="{0:dd.MM.yyyy}" HtmlEncode="false" HeaderStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="vchName" SortExpression="vchName" HeaderText="Konto" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"/>                        
                        </Columns>
                        
                        <SelectedRowStyle BackColor="#F0F8FF" />
                        
                    </asp:GridView>                                
                </asp:Panel>
                
                <asp:SqlDataSource ID="dataSourceCategoriesED" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM DeviceCategory ORDER BY vchName ASC"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource> 
                
                <asp:SqlDataSource ID="dataSourceTypesED" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT * FROM DeviceType ORDER BY vchName ASC"                    
                    FilterExpression="DeviceCategoryID='{0}'"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                                                
                    <FilterParameters>
                        <asp:ControlParameter Name="DeviceCategoryID" ControlId="ddCategoriesED" PropertyName="SelectedValue" />
                    </FilterParameters>
                </asp:SqlDataSource> 
                
                <asp:SqlDataSource ID="dataSourceDevicesED" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"                                        
                    SelectCommand="SELECT Device.ID, Device.vchSerialNumber, Device.vchStatus, Device.vchOwnership, Device.textComment, Device.dateReceivedNew, Account.vchName FROM Device, Account WHERE Device.AccountID = Account.ID AND Device.DeviceCategoryID = @categoryID AND Device.DeviceTypeID = @typeID"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">
                    <SelectParameters>
                        <asp:ControlParameter Name="categoryID" ControlID="ddCategoriesED" PropertyName="SelectedValue"/>
                        <asp:ControlParameter Name="typeID" ControlID="ddTypesED" PropertyName="SelectedValue"/>                        
                    </SelectParameters>                     
                </asp:SqlDataSource>                                 
                
                <asp:SqlDataSource ID="dataSourceAccountsED" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT ID, vchName FROM Account ORDER BY vchName ASC"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource> 
                
                <asp:XmlDataSource ID="dataSourceStatusED" runat="server" DataFile="~/App_data/DeviceStatus.xml"></asp:XmlDataSource>
                <asp:XmlDataSource ID="dataSourceOwnershipED" runat="server" DataFile="~/App_Data/DeviceOwnership.xml"></asp:XmlDataSource>                                                                                                                   

                <br />
                <center><asp:Label ID="labelStatusEdit" runat="server" CssClass="StatusMessageClass" /></center>
                        
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
            
        </act:TabPanel>                                
        
        <act:TabPanel ID="tabDevicesAll" runat="server" HeaderText="Vis alle enheter">
        
            <ContentTemplate>                            
            
            <asp:UpdatePanel ID="updatePanelAll" runat="server">
                <ContentTemplate>
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelAll" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">
                
                <asp:Panel ID="panelDeviceInfo" runat="server" Visible="false">
                    <center><b>Historie</b></center>            
                    <asp:Label ID="labelDeviceInfo" runat="server" Font-Size="X-Small"></asp:Label>
                    <br />
                    <asp:Panel runat="server" Height="90px" ScrollBars="Vertical" BorderStyle="Solid" BorderWidth="1px">
                        <asp:GridView ID="gridDeviceInfo" runat="server" Font-Size="X-Small"                                    
                            DataKeyNames="ID" 
                            AutoGenerateColumns="false" 
                            ShowHeader="false" 
                            ShowFooter="false" 
                            GridLines="None">
                            <Columns>                                    
                                <asp:BoundField DataField="dateCreated" SortExpression="dateCreated" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top" DataFormatString="[{0:dd.MM.yyyy}]&nbsp;" HtmlEncode="false" HeaderStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="vchComment" SortExpression="vchComment" HeaderStyle-HorizontalAlign="Left"/>                                        
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>                                   
                    
                    <center>
                        <asp:Button ID="buttonDeviceInfoCancel" runat="server" Style="margin-top:3px" 
                            Text="Lukk historie panel" 
                            OnClick="buttonDeviceInfoCancel_OnClick" 
                            CssClass="ButtonClass"/>
                    </center>
                    
                    <hr />
                    
                </asp:Panel>                                                
                
                <table width="100%">
                    <tr>
                        <th align="left">Liste over alle enheter</th>
                    </tr>
                    <tr>    
                        <td>
                            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="520px" ScrollBars="Vertical">
                                <asp:GridView ID="gridDevicesAllED" runat="server" 
                                    Font-Size="X-Small" 
                                    ShowFooter="false"
                                    AllowSorting="true"
                                    Width="96%" 
                                    GridLines="None" 
                                    AutoGenerateColumns="false"                 
                                    OnSelectedIndexChanged="gridDevicesAllED_OnSelectedIndexChanged"
                                    OnRowDataBound="gridDevicesAllED_OnRowDataBound"
                                    DataKeyNames="ID" 
                                    DataSourceID="dataSourceDevicesAllED"> 
                                    
                                    <HeaderStyle HorizontalAlign="Left" />
                                                       
                                    <Columns>                               
                                        <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false" />
                                        <asp:BoundField DataField="vchSerialNumber" SortExpression="vchSerialNumber" HeaderText="Serienummer" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="categoryName" SortExpression="categoryName" HeaderText="Kategori" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="typeName" SortExpression="typeName" HeaderText="Type" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="vchStatus" SortExpression="vchStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="vchOwnership" SortExpression="vchOwnership" HeaderText="Eierskap" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="textComment" SortExpression="textComment" HeaderText="Kommentar" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="dateReceivedNew" SortExpression="dateReceivedNew" HeaderText="Mottatt Dato" DataFormatString="{0:dd.MM.yyyy}" HtmlEncode="false" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="vchName" SortExpression="vchName" HeaderText="Konto" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left"/>                        
                                    </Columns>
                                    
                                    <SelectedRowStyle BackColor="#F0F8FF" />
                                    
                                </asp:GridView>                                
                            </asp:Panel>            
                        </td>
                    </tr>
                </table>                                
            
            <asp:SqlDataSource ID="dataSourceDevicesAllED" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"                                        
                SelectCommand="SELECT Device.ID, Device.vchSerialNumber, DeviceCategory.vchName as categoryName, DeviceType.vchName as typeName, Device.vchStatus, Device.vchOwnership, Device.textComment, Device.dateReceivedNew, Account.vchName FROM Device, Account, DeviceCategory, DeviceType WHERE Device.DeviceCategoryID = DeviceCategory.ID AND Device.DeviceTypeID = DeviceType.ID AND Device.AccountID = Account.ID"
                ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                    
            </asp:SqlDataSource> 

            <br />                
            <center><asp:Label ID="labelStatusAll" runat="server" CssClass="StatusMessageClass" /></center>
                
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
            
            </ContentTemplate>
            
        </act:TabPanel>
        
    </act:TabContainer>                
    
</asp:Content>

