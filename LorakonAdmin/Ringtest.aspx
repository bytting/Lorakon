<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Ringtest.aspx.cs" Inherits="Ringtest" ValidateRequest="false" EnableEventValidation="false" Title="LorakonAdmin - Ringtest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<%@ Register Assembly="ZedGraph.Web" Namespace="ZedGraph.Web" TagPrefix="zgw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
    <script type="text/javascript">
        function OnTabChanged(sender, args)
        {
            sender.get_clientStateField().value = sender.saveClientState();                                                
        }                                                                                                          
    </script>          
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">                
    
    <act:TabContainer ID="tabContainerRingtest" runat="server" OnClientActiveTabChanged="OnTabChanged">            
        
        <act:TabPanel ID="tabCreate" runat="server">
        
            <ContentTemplate>                                
                
                <div style="margin:8px">
                
                <asp:MultiView ID="multiViewCreateRingtest" runat="server" ActiveViewIndex="0">
                
                <asp:View ID="viewCreateRingtest" runat="server">
                
                <table>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Start dato" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxStartDate" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>
                            <act:CalendarExtender ID="calStartDate" runat="server" 
                                Format="dd.MM.yyyy" 
                                TargetControlID="textBoxStartDate" />
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Arkiv referanse" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbArchiveRef" runat="server" CssClass="TextBoxClass2x" MaxLength="79"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Kommentar" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxComment" runat="server" TextMode="MultiLine" CssClass="TextBoxClass2x" Height="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="buttonInsert" runat="server" 
                                Text="Opprett" 
                                OnClick="buttonInsert_OnClick" 
                                CssClass="ButtonClass" Width="120px"/>                            
                        </td>
                    </tr>
                </table>                                 
                
                </asp:View>
                
                <asp:View ID="viewEditRingtest" runat="server">
                
                    <table width="100%">
                    <tr>
                    <td>
                    
                    <table>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Start dato" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxStartDateE" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>                            
                            <act:CalendarExtender ID="calStartDateE" runat="server" 
                                Format="dd.MM.yyyy"
                                TargetControlID="textBoxStartDateE" />
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Arkiv referanse" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbArchiveRefE" runat="server" CssClass="TextBoxClass2x" MaxLength="79"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Kommentar" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="textBoxCommentE" runat="server" TextMode="MultiLine" CssClass="TextBoxClass2x" Height="200px"></asp:TextBox>
                        </td>
                    </tr> 
                    <tr>                        
                        <td>                            
                            <asp:CheckBox ID="cbFinishedE" runat="server" Checked="false" Text="Ringtest avsluttet" CssClass="TipText"/>
                        </td>
                    </tr>                   
                    </table>                                 
                    
                    </td>
                    
                    <td>
                        <asp:Label runat="server" Text="Påmeldte" CssClass="TipText"></asp:Label>
                        <br />
                        <asp:ListBox ID="lbRingtestMembers" runat="server" CssClass="TextBoxClass2x" Height="250px"></asp:ListBox>
                    </td>
                    
                    </tr>
                    </table>
                    
                    <br />
                    
                    <table>
                    <tr>
                        <td>
                            <asp:Button ID="buttonUpdateE" runat="server" 
                                Text="Oppdater" 
                                OnClick="buttonUpdateE_OnClick" 
                                CssClass="ButtonClass" Width="120px"/>
                        </td>
                        <td>        
                            <asp:Button ID="buttonGenerateRingtestSheet" runat="server" 
                                Text="Generer ringtest liste" 
                                OnClick="buttonGenerateRingtestSheet_OnClick" 
                                CssClass="ButtonClass"/>                                                                        
                        </td>
                        <td>        
                            <asp:Button ID="buttonShowMemberEmails" runat="server" 
                                Text="Vis epost for påmeldte" 
                                OnClick="buttonShowMemberEmails_OnClick" 
                                CssClass="ButtonClass"/>                                                                        
                        </td>
                        <td>        
                            <asp:TextBox ID="textBoxShowMemberEmails" runat="server" CssClass="TextBoxClass2x"/>                                                                        
                        </td>
                    </tr>
                    </table>
                    
                </asp:View>
                
                </asp:MultiView>
                
                <br />
                <center><asp:Label ID="labelStatusCreate" runat="server" CssClass="StatusMessageClass" /></center>
                
                </div>                
                
            </ContentTemplate>
            
        </act:TabPanel>
        
        <act:TabPanel runat="server" HeaderText="Vis alle ringtester">
        
            <ContentTemplate>
                
                <asp:UpdatePanel ID="updatePanelShowAll" runat="server">
                <ContentTemplate>
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelShowAll" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">
                        
                <asp:Panel ID="panelRingtestStats" runat="server" Visible="false">
                
                    <center>
                        <asp:Label ID="labelRingtestStatsIngress" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                    </center>
                    <br />
                    
                    <zgw:ZedGraphWeb ID="graphStatistics" runat="server"                      
                        Width="900" 
                        Height="550" 
                        RenderMode="ImageTag" 
                        OnRenderGraph="graphStatistics_OnRenderGraph">                    
                    </zgw:ZedGraphWeb>                                
                    
                    <br />
                    <center>
                        <asp:Button ID="buttonRingtestStatsCancel" runat="server" 
                            Text="Lukk statistikk vindu" 
                            OnClick="buttonRingtestStatsCancel_OnClick" 
                            CssClass="ButtonClass"/>
                    </center>
                     
                    <hr />   
                </asp:Panel>
                    
                <br />
                
                <table width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <th align="left">Liste over alle ringtester</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gridShowAllRingtests" runat="server" 
                                Font-Size="X-Small"
                                DataSourceID="dataSourceShowAllRingtests" 
                                DataKeyNames="ID" 
                                AllowSorting="true" 
                                AutoGenerateColumns="false" 
                                Width="100%" 
                                GridLines="None"                                 
                                OnSelectedIndexChanged="gridShowAllRingtests_OnSelectedIndexChanged"
                                OnRowDataBound="gridShowAllRingtests_OnRowDataBound">
                                
                                <HeaderStyle HorizontalAlign="Left" />
                                
                                <Columns>
                                    <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false" HeaderStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="intYear" SortExpression="intYear" HeaderText="År" HeaderStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="dateStart" SortExpression="dateStart" HeaderText="Start dato" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="vchArchiveRef" SortExpression="vchArchiveRef" HeaderText="Arkiv ref." HeaderStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="textComment" SortExpression="textComment" HeaderText="Kommentar" HeaderStyle-HorizontalAlign="Left"/>                        
                                    <asp:CheckBoxField DataField="bitFinished" SortExpression="bitFinished" HeaderText="Avsluttet" HeaderStyle-HorizontalAlign="Left"/>                        
                                </Columns>
                                
                                <SelectedRowStyle BackColor="#F0F8FF" />
                                
                            </asp:GridView>
                        </td>
                    </tr>
                </table>                                
                
                <br />
                
                <center>
                    <asp:Label ID="labelStatusShowAll" runat="server" CssClass="StatusMessageClass" />
                </center>
                
                <asp:SqlDataSource ID="dataSourceShowAllRingtests" runat="server" 
                    SelectCommand="SELECT ID, intYear, dateStart, vchArchiveRef, textComment, bitFinished FROM Ringtest ORDER BY intYear DESC" 
                    ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>">                    
                </asp:SqlDataSource>                                
                
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
            
        </act:TabPanel>
        
        <act:TabPanel ID="tabEdit" runat="server" HeaderText="Rediger ringtestrapporter">
            <ContentTemplate>                                
                
                <div style="margin:8px">
                
                <asp:HiddenField ID="hiddenContactID" runat="server" />
                <asp:HiddenField ID="hiddenAccountName" runat="server" />
                <asp:HiddenField ID="hiddenDetector" runat="server" />
                <asp:HiddenField ID="hiddenRingtestBox" runat="server" />    
                
                <asp:MultiView ID="multiViewRingtestReports" runat="server" ActiveViewIndex="0">
                
                <asp:View ID="viewRingtestReports" runat="server">                
                <table width="100%" cellspacing="0" cellpadding="0">
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Velg årstall" CssClass="TipText"></asp:Label>                            
                            <asp:DropDownList ID="ddYears" runat="server" 
                                AutoPostBack="true" 
                                DataSourceID="dataSourceYears" 
                                DataTextField="intYear" 
                                DataValueField="ID" 
                                OnDataBound="ddYears_OnDataBound" 
                                OnSelectedIndexChanged="ddYears_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>                        
                        <td>
                            <asp:CheckBox ID="cbRingtestReportsEvalOnly" runat="server" 
                                Text="Vis kun rapporter som ønsker evaluering og ikke er godkjent" 
                                Checked="false" 
                                CssClass="TipText"
                                AutoPostback="true" 
                                OnCheckedChanged="cbRingtestReportsEvalOnly_OnCheckChanged"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gridRingtestReports" runat="server" 
                                Font-Size="X-Small" 
                                AllowSorting="true"
                                Width="100%" 
                                GridLines="None" 
                                AutoGenerateColumns="false" 
                                DataKeyNames="ID" 
                                DataSourceID="dataSourceRingtestReports" 
                                OnSelectedIndexChanged="gridRingtestReports_OnSelectedIndexChanged" 
                                OnRowDataBound="gridRingtestReports_OnRowDataBound" 
                                OnSorting="gridRingtestReports_OnSorting">  
                                      
                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                        
                                <Columns>
                                    <asp:BoundField DataField="ID" SortExpression="ID" Visible="false"/>
                                    <asp:BoundField DataField="accountName" SortExpression="accountName" HeaderText="Konto" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="deviceSerialnumber" SortExpression="deviceSerialnumber" HeaderText="Detektor serienr." HeaderStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="boxKNumber" SortExpression="boxKNumber" HeaderText="Boks" HeaderStyle-HorizontalAlign="Left"/>
                                    <asp:CheckBoxField DataField="bitWantEvaluation" SortExpression="bitWantEvaluation" HeaderText="Ønsker eval." HeaderStyle-HorizontalAlign="Left"/>                        
                                    <asp:CheckBoxField DataField="bitEvaluated" SortExpression="bitEvaluated" HeaderText="Evaluert" HeaderStyle-HorizontalAlign="Left"/>                        
                                    <asp:CheckBoxField DataField="bitApproved" SortExpression="bitApproved" HeaderText="Godkjent" HeaderStyle-HorizontalAlign="Left"/>                        
                                    <asp:CheckBoxField DataField="bitAnswerByEmail" SortExpression="bitAnswerByEmail" HeaderText="Svar pr brev" HeaderStyle-HorizontalAlign="Left"/>                        
                                    <asp:CheckBoxField DataField="bitAnswerSent" SortExpression="bitAnswerSent" HeaderText="Svar sendt" HeaderStyle-HorizontalAlign="Left"/>                        
                                </Columns>
                                
                            </asp:GridView>        
                        </td>
                    </tr>
                </table>                                                
                
                <asp:SqlDataSource ID="dataSourceYears" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>" 
                    SelectCommand="SELECT ID, intYear FROM Ringtest ORDER BY intYear DESC" 
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">
                </asp:SqlDataSource>
                
                <asp:SqlDataSource ID="dataSourceRingtestReports" runat="server"
                    ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"                         
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">
                </asp:SqlDataSource>
                
                </asp:View>
                
                <asp:View ID="viewRingtestReport" runat="server">                                    
                    
                    <asp:Table ID="tableRingtestReportHeader" runat="server" 
                        Width="100%" 
                        CellPadding="0" 
                        CellSpacing="0" 
                        Font-Bold="true">
                    
                        <asp:TableRow ID="rowAccount" runat="server">
                            <asp:TableCell ID="cellAccount1" runat="server" Width="25%"></asp:TableCell>
                            <asp:TableCell ID="cellAccount2" runat="server"></asp:TableCell>
                            <asp:TableCell ID="cellAccount3" runat="server"></asp:TableCell>
                            <asp:TableCell ID="cellAccount4" runat="server"></asp:TableCell>
                        </asp:TableRow>
                        
                        <asp:TableRow ID="rowDetector" runat="server">
                            <asp:TableCell ID="cellDetector1" runat="server" Width="25%"></asp:TableCell>
                            <asp:TableCell ID="cellDetector2" runat="server"></asp:TableCell>
                            <asp:TableCell ID="cellDetector3" runat="server"></asp:TableCell>
                            <asp:TableCell ID="cellDetector4" runat="server"></asp:TableCell>
                        </asp:TableRow>
                        
                        <asp:TableRow ID="rowBox" runat="server">
                            <asp:TableCell ID="cellBox1" runat="server" Width="25%"></asp:TableCell>
                            <asp:TableCell ID="cellBox2" runat="server"></asp:TableCell>
                            <asp:TableCell ID="cellBox3" runat="server"></asp:TableCell>
                            <asp:TableCell ID="cellBox4" runat="server"></asp:TableCell>
                        </asp:TableRow>
                        
                        <asp:TableRow ID="rowContact" runat="server">
                            <asp:TableCell ID="cellContact1" runat="server" Width="25%"></asp:TableCell>
                            <asp:TableCell ID="cellContact2" runat="server"></asp:TableCell>
                            <asp:TableCell ID="cellContact3" runat="server"></asp:TableCell>
                            <asp:TableCell ID="cellContact4" runat="server"></asp:TableCell>
                        </asp:TableRow>
                        
                    </asp:Table>
                    
                    <br /><hr /><br />
                    
                    <table width="100%" cellspacing="3" cellpadding="0">                               
                    <tr>
                        <th align="right">Felt</th>
                        <th align="left">Verdi</th>                        
                        <th align="left">Kalkulert</th>
                        <th align="left">Rapport status</th>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="MCA type:" CssClass="TipText"></asp:Label>                            
                        </td>
                        <td>
                            <asp:Label ID="labelMCA" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                
                    </tr>                    
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Måledato:" CssClass="TipText"></asp:Label>                                                        
                        </td>
                        <td colspan="2">
                            <asp:Label ID="labelMeasureDate" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                
                        <td>
                            <asp:CheckBox ID="cbWantEvaluation" runat="server" Enabled="false" Height="20px" Text="Ønsker evaluering" CssClass="TipText"></asp:CheckBox>
                        </td>                    
                    </tr>                    
                    <tr>
                        <td align="right">
                            <asp:Label ID="labelIntegralBackgroundDesc" runat="server" Text="Integral bakgrunn: " CssClass="TipText"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="labelIntegralBackground" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                
                        <td>
                            <asp:CheckBox ID="cbApproved" runat="server" Text="Godkjent" Height="20px" CssClass="TipText" />
                        </td>
                    </tr>
                    <tr>                        
                        <td align="right">
                            <asp:Label ID="labelCountingBackgroundDesc" runat="server" Text="Telletid bakgrunn: " CssClass="TipText"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="labelCountingBackground" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                
                        <td>
                            <asp:CheckBox ID="cbAnswerByEmail" runat="server" Text="Ønsker svar pr brev" Height="20px" Enabled="false" CssClass="TipText"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="labelBackgroundDesc" runat="server" Text="Bakgrunn: " CssClass="TipText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labelBackground" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                
                        <td align="left">
                            <asp:Label ID="labelIbLb" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                        
                        <td>
                            <asp:CheckBox ID="cbEvaluated" runat="server" Text="Evaluert" Height="20px" CssClass="TipText" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="labelGeoEff" runat="server" Text="Geometrifaktor: " CssClass="TipText"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="labelGeometryFactor" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                
                        <td>
                            <asp:CheckBox ID="cbAnswerSent" runat="server" Text="E-post er sendt" Height="20px" Enabled="false" CssClass="TipText"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="labelAvgIntegralSampleDesc" runat="server" Text="Middelverdi: " CssClass="TipText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labelAvgIntegralSample" runat="server" CssClass="ValueText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Telletid middelverdi: " CssClass="TipText"></asp:Label>
                        </td>                        
                        <td>
                            <asp:Label ID="labelAvgLivetimeSample" runat="server" CssClass="ValueText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="labelActivityDesc" runat="server" Text="Aktivitet: " CssClass="TipText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labelActivity" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                
                        <td align="left">
                            <asp:Label ID="labelA" runat="server" CssClass="ValueText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Aktivitet referanse: " CssClass="TipText"></asp:Label>                            
                        </td>
                        <td>
                            <asp:Label ID="labelActivityRef" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                                    
                        <td align="left">
                            <asp:Label ID="labelA0" runat="server" CssClass="ValueText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Referanse dato: " CssClass="TipText"></asp:Label>                                                        
                        </td>
                        <td>
                            <asp:Label ID="labelRefDate" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                    
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Usikkerhet: " CssClass="TipText"></asp:Label>                                                                                    
                        </td>
                        <td>
                            <asp:Label ID="labelUncertainty" runat="server" CssClass="ValueText"></asp:Label>
                        </td>                                                
                        <td align="left">
                            <asp:Label ID="labelSigmaA" runat="server" CssClass="ValueText"></asp:Label>
                        </td>
                    </tr>                                        
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" Text="Feilmargin: " CssClass="TipText"></asp:Label>                                                                                                                
                        </td>
                        <td>
                            <asp:Label ID="labelError" runat="server" CssClass="ValueText"></asp:Label><asp:Label runat="server" Text="%" CssClass="ValueText"></asp:Label>
                        </td>
                    </tr>                                        
                    </table>
                             
                    <br />
                               
                    <table width="100%">                    
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Kommentar" CssClass="TipText"></asp:Label>                                                                                                                                            
                            <br />
                            <asp:TextBox ID="tbComment" runat="server" 
                                TextMode="Multiline" 
                                Height="35px" 
                                Width="100%" 
                                MaxLength="995" 
                                style="border:solid 1px #000000">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>                        
                        <td>                            
                            <asp:Panel runat="server" Width="100%" Height="100px" ScrollBars="Vertical" BorderStyle="Solid" BorderWidth="1px">
                                <asp:GridView ID="gridComments" runat="server" Font-Size="X-Small"                                    
                                    DataKeyNames="ID" 
                                    AutoGenerateColumns="false" 
                                    ShowHeader="false" 
                                    ShowFooter="false" 
                                    GridLines="None">
                                    <Columns>                                    
                                        <asp:BoundField DataField="dateCreated" SortExpression="dateCreated" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top" DataFormatString="[{0:dd.MM.yyyy}]&nbsp;" HtmlEncode="false" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="vchContactName" DataFormatString="{0}:" HtmlEncode="false" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top" SortExpression="vchContactName" HeaderStyle-HorizontalAlign="Left"/>                                        
                                        <asp:BoundField DataField="textComment" SortExpression="textComment" HeaderStyle-HorizontalAlign="Left"/>                                        
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>               
                        </td>
                    </tr>
                    </table>
                    
                    <br />
                    
                    <table cellspacing="2">
                        <tr>
                        <td>
                            <asp:Button ID="buttonRingtestReportUpdate" runat="server" 
                                Text="Oppdater" 
                                OnClick="buttonRingtestReportUpdate_OnClick" 
                                CssClass="ButtonClass" Width="120px"/>
                                
                            <asp:Button ID="buttonRingtestReportSendAnswer" runat="server" 
                                Text="Oppdater og send svar" 
                                OnClick="buttonRingtestReportSendAnswer_OnClick" 
                                CssClass="ButtonClass"/>
                                
                            <asp:Button ID="buttonRingtestReportGenerateLetter" runat="server" 
                                Text="Generer brev" 
                                OnClick="buttonRingtestReportGenerateLetter_OnClick" 
                                CssClass="ButtonClass" Width="120px"/>                                                                                                                        
                    
                            <asp:Button ID="buttonRingtestReportCancel" runat="server" 
                                Text="Avbryt" 
                                OnClick="buttonRingtestReportCancel_OnClick" 
                                CssClass="ButtonClass" Width="120px"/>                                                                        
                        </td>
                        </tr>
                    </table>
                    
                </asp:View>
                
                </asp:MultiView>
                
                <br />
                
                <center>
                    <asp:Label ID="labelStatusEdit" runat="server" CssClass="StatusMessageClass" />
                </center>
                
                </div>                                                
                
            </ContentTemplate>
            
        </act:TabPanel>                
        
        <act:TabPanel ID="tabShowIncomplete" runat="server" HeaderText="Vis ufullstendige rapporter">
            <ContentTemplate>
                
                <asp:UpdatePanel ID="updatePanelShowIncomplete" runat="server">
                <ContentTemplate>
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelShowIncomplete" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">                                        
                    
                    <table width="100%">
                        <tr>
                            <th align="left">
                                Aktive rapporter fra tidligere år som ikke har bedt om evaluering
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbShowIncompleteForCurrentYear" runat="server" 
                                    AutoPostBack="true" 
                                    Checked="false" 
                                    CssClass="TipText"
                                    Text="Inkluder årets rapporter"
                                    OnCheckedChanged="cbShowIncompleteForCurrentYear_OnCheckChanged"/>    
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gridShowIncomplete" runat="server" 
                                    Font-Size="X-Small"
                                    AllowSorting="true"
                                    Width="100%" 
                                    GridLines="None" 
                                    AutoGenerateColumns="false" 
                                    DataKeyNames="reportID" 
                                    DataSourceID="dataSourceIncomplete" 
                                    OnRowDeleting="gridShowIncomplete_OnDeleteCommand">  
                                      
                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                        
                                    <Columns>
                                        <asp:BoundField DataField="reportID" SortExpression="reportID" Visible="false"/>
                                        <asp:BoundField DataField="accountName" SortExpression="accountName" HeaderText="Konto" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ringtestYear" SortExpression="ringtestYear" HeaderText="År" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="deviceSerialnumber" SortExpression="deviceSerialnumber" HeaderText="Detektor serienr." HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:CommandField ShowDeleteButton="true" DeleteText="Slett" HeaderStyle-HorizontalAlign="Left"/>                                                                           
                                    </Columns>
                                
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>                                        
                    
                    <asp:SqlDataSource ID="dataSourceIncomplete" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                        SelectCommand="SELECT RingtestReport.ID AS reportID, Account.vchName AS accountName, Ringtest.intYear AS ringtestYear, Device.vchSerialnumber AS deviceSerialnumber FROM RingtestReport, Account, Device, Ringtest WHERE Account.ID = RingtestReport.AccountID AND Device.ID = RingtestReport.DetectorID AND Ringtest.ID = RingtestReport.RingtestID AND RingtestReport.bitWantEvaluation = 0 AND Ringtest.intYear < @year ORDER BY Ringtest.intYear DESC"                                                
                        DeleteCommand="DELETE FROM RingtestReport WHERE ID = @ID"
                        ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">
                        <SelectParameters>
                            <asp:Parameter Name="year"/>
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:ControlParameter Name="ID" ControlID="gridShowIncomplete" PropertyName="SelectedDataKey"/>
                        </DeleteParameters>
                    </asp:SqlDataSource>
                
                    <br />
                    
                    <center>
                        <asp:Label ID="labelStatusIncomplete" runat="server" CssClass="StatusMessageClass" />
                    </center>
                    
                </div>
                
                </ContentTemplate>
                </asp:UpdatePanel>                                
                
            </ContentTemplate>
        </act:TabPanel>
        
        <act:TabPanel ID="tabAssignBoxes" runat="server" HeaderText="Tilordning av ringtestbokser">
        
            <ContentTemplate>
            
                <asp:UpdatePanel ID="updatePanelBoxes" runat="server">
                <ContentTemplate>
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelBoxes" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">
                
                <table cellspacing="0" cellpadding="0" width="100%">                    
                    <tr>                                                                                                
                        <td>
                            <asp:Label runat="server" Text="Konto" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddUsersAssignBox" runat="server" 
                                CssClass="TextBoxClass2x"
                                AutoPostBack="true" 
                                DataSourceID="dataSourceAccountsAssignBox" 
                                DataTextField="vchName" 
                                DataValueField="ID" 
                                OnSelectedIndexChanged="ddUsersAssignBox_OnSelectedIndexChanged" 
                                OnDataBound="ddUsersAssignBox_OnDataBound" />                                                        
                        </td>                            
                        <td>
                            <asp:CheckBox ID="cbUnassignedAccounts" runat="server" 
                                Text="Vis konti med boks" 
                                Checked="false" 
                                CssClass="TipText"
                                AutoPostBack="true" 
                                OnCheckedChanged="cbUnassignedAccounts_OnCheckChanged"/>
                            <br />
                            <asp:CheckBox ID="cbRegisteredOnly" runat="server" 
                                Visible="false"
                                Text="Vis kun påmeldte" 
                                Checked="true" 
                                CssClass="TipText"
                                AutoPostBack="true" 
                                OnCheckedChanged="cbUnassignedAccounts_OnCheckChanged"/>                                
                        </td>
                    </tr>                                        
                    <tr>                        
                        <td>                            
                            <asp:Label runat="server" Text="Tildelt ringtestboks" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddRingtestBoxes" runat="server" 
                                CssClass="TextBoxClass2x" 
                                DataSourceID="dataSourceRingtestBoxes" 
                                DataTextField="vchKNumber" 
                                DataValueField="ID" 
                                OnDataBound="ddRingtestBoxes_OnDataBound">
                            </asp:DropDownList>                                                        
                        </td>                                                                    
                        <td>
                            <asp:Label runat="server" Text="Tidligere tildelte ringtestbokser i kronologisk rekkefølge" CssClass="TipText"></asp:Label>
                            <br />
                            <asp:ListBox ID="lbBoxHistory" runat="server" CssClass="TextBoxClass2x"></asp:ListBox>                            
                        </td>
                    </tr>                       
                    <tr>
                        <td>
                            <asp:Button ID="buttonAssignBox" runat="server" 
                                Text="Tilordne" 
                                OnClick="buttonAssignBox_OnClick" 
                                CssClass="ButtonClass"/>
                        </td>
                    </tr>                                                                                                                                  
                </table>                    
                
                <br />
                <center><asp:Label ID="labelStatusBoxes" runat="server" CssClass="StatusMessageClass" /></center>
                    
                <asp:SqlDataSource ID="dataSourceAccountsAssignBox" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"                    
                    SelectCommand="SELECT ID, vchName FROM Account WHERE bitActive=1 AND RingtestBoxID <> @boxID ORDER BY vchName ASC"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                    <SelectParameters>
                        <asp:Parameter Name="boxID" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                
                <asp:SqlDataSource ID="dataSourceRingtestBoxes" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"
                    SelectCommand="SELECT ID, vchKNumber FROM RingtestBox ORDER BY vchKNumber"                    
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                        
                </asp:SqlDataSource>                                                                 
                
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                        
            </ContentTemplate>
            
        </act:TabPanel>                
        
    </act:TabContainer>        
    
    <br />
    <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
    
</asp:Content>

