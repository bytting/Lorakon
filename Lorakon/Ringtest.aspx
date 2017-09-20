<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Ringtest.aspx.cs" Inherits="Ringtest" Title="<%$ Resources:Localization, Title_Ringtest %>" %>

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
                                                        
    <asp:HiddenField ID="hiddenAccountID" runat="server"/>    
    <asp:HiddenField ID="hiddenRingtestID" runat="server"/>    
    <asp:HiddenField ID="hiddenRingtestBoxID" runat="server"/>
       
    <act:TabContainer ID="tabContainerRingtest" runat="server" OnClientActiveTabChanged="OnTabChanged">            
                
        <act:TabPanel ID="tabRingtest" runat="server" HeaderText="<%$ Resources:Localization, CurrentRingtest %>">
        
            <ContentTemplate>                                                                                                                        
                    
                <asp:UpdatePanel ID="updatePanelReport" runat="server">
                <ContentTemplate>        
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelReport" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">
                            
                <asp:MultiView ID="multiViewRingtest" runat="server">
                
                    <asp:View ID="viewFinished" runat="server">
                    
                        <asp:Label ID="labelFinished" runat="server" CssClass="TipTextBig" Text="<%$ Resources:Localization, RingtestFinished %>"></asp:Label>
                        
                    </asp:View>
                    
                    <asp:View ID="viewRegister" runat="server">
                    
                        <asp:Label ID="labelRegister" runat="server" CssClass="TipTextBig"></asp:Label>
                        <br /><br />
                        <asp:DropDownList ID="ddRegister" runat="server" 
                            DataSourceID="dataSourceInternalUsers" 
                            DataTextField="vchName" 
                            DataValueField="ID" 
                            Width="300px">
                        </asp:DropDownList>
                        <br /><br />
                        <asp:Button ID="buttonRegister" runat="server" 
                            Text="<%$ Resources:Localization, Register %>" 
                            OnClick="buttonRegister_OnClick" 
                            CssClass="ButtonClass"></asp:Button>
                        
                        <br />
                        
                        <center>
                            <asp:Label ID="labelStatusRegister" runat="server" CssClass="StatusMessageClass"/>
                        </center>
                        
                        <asp:SqlDataSource ID="dataSourceInternalUsers" runat="server"                             
                            ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>" 
                            SelectCommand="SELECT ID, vchName FROM Contact WHERE AccountID = @accountID AND vchStatus IN('Active', 'Unknown')"
                            ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                                                
                            <SelectParameters>
                                <asp:ControlParameter Name="accountID" ControlID="hiddenAccountID" PropertyName="Value"/>
                            </SelectParameters>
                        </asp:SqlDataSource>                                                                                                           
                        
                    </asp:View>
                    
                    <asp:View ID="viewNoInit" runat="server">
                    
                        <asp:Label ID="labelInit" runat="server" CssClass="TipTextBig"></asp:Label>
                        <br />
                        <asp:LinkButton ID="linkButtonNoInit" runat="server" Text="<%$ Resources:Localization, Back %>" PostBackUrl="~/Default.aspx"></asp:LinkButton>
                        
                    </asp:View>
                    
                    <asp:View ID="viewSendMessage" runat="server">
                    
                        <asp:Label ID="labelSendMessage" runat="server" CssClass="TipTextBig"></asp:Label>
                        
                        <asp:TextBox ID="tbMessage" runat="server" TextMode="MultiLine" Width="100%" Height="200px"></asp:TextBox>
                        <act:FilteredTextBoxExtender runat="server" 
                            ID="tbMessageFilter" 
                            TargetControlID="tbMessage" 
                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                            ValidChars="ÆØÅæøå-_,.:()!?% " />
                            
                        <asp:Button ID="buttonSendMessage" runat="server" 
                            Text="<%$ Resources:Localization, Send %>" 
                            OnClick="buttonSendMessage_OnClick" 
                            CssClass="ButtonClass"></asp:Button>
                        
                        <br />    
                        <center>
                            <asp:Label ID="labelStatusMessage" runat="server" CssClass="StatusMessageClass"/>
                        </center>
                        
                    </asp:View>
                    
                    <asp:View ID="viewSelectDetector" runat="server">                                                                                                                 
                           
                        <table width="100%">                                                                        
                            <tr>                                
                                <td>
                                    <asp:Label runat="server" Text="<%$ Resources:Localization, SelectDetector %>" CssClass="TipText" />
                                    <br />
                                    <asp:DropDownList ID="ddDetector" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddDetector_OnSelectedIndexChanged"/>
                                </td>                                                    
                                <td style="margin-left:50px">
                                    <asp:Label ID="labelDetectorInfo" runat="server" CssClass="TipTextBig"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:16px"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="buttonDetectorContinue" runat="server" 
                                        Enabled="false"
                                        Text="<%$ Resources:Localization, Continue %>" 
                                        OnClick="buttonDetectorContinue_OnClick" 
                                        CssClass="ButtonClass"/>
                                </td>                                
                            </tr>                             
                        </table> 
                        
                        <br />
                        <center>
                            <asp:Label ID="labelStatusSelectDetector" runat="server" CssClass="StatusMessageClass"/>
                        </center>                                                                                               
                        
                    </asp:View>                                        
                    
                    <asp:View ID="viewRingtestReport" runat="server">                                                                                                                
                        
                        <asp:HiddenField ID="hiddenRingtestReportID" runat="server"/>
                                
                        <center>
                        <asp:Label runat="server" Font-Bold="true" Text="<%$ Resources:Localization, RingtestReportTitle %>" />                        
                        <asp:Label ID="labelReportDetector" runat="server" Font-Bold="true" />
                        </center>                        
                        <br />
                        <asp:Label runat="server" Font-Bold="true" Text="<%$ Resources:Localization, RingtestIngress %>" />
                        
                        <br /><br />
                        
                        <table width="100%">
                        <tr>
                            <th align="left"><asp:Label runat="server" Text="<%$ Resources:Localization, Description %>" /></th>
                            <th align="left"><asp:Label runat="server" Text="<%$ Resources:Localization, Value %>" /></th>                                                                                     
                        </tr>
                        <tr>                            
                            <td><asp:Label runat="server" Text="<%$ Resources:Localization, BoxWeight %>" /></td>                           
                            <td><asp:Label ID="labelBoxWeight" runat="server"></asp:Label></td>                            
                        </tr>
                        <tr>                            
                            <td>
                                <asp:Label runat="server" Text="<%$ Resources:Localization, ContactForReport %>" />
                                <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            </td>                                                       
                            <td>
                                <asp:DropDownList ID="ddCurrentActor" runat="server" Width="154px" DataSourceID="dataSourceRingtestActors" DataTextField="vchName" DataValueField="ID" OnDataBound="ddCurrentActor_OnDataBound"></asp:DropDownList>                                
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, ContactForReport_Tip %>"></asp:Label></td> -->
                        </tr>                        
                        <tr>                            
                            <td><asp:Label runat="server" Text="<%$ Resources:Localization, MCAType %>" /></td>                                                       
                            <td>
                                <asp:DropDownList ID="ddMCAType" runat="server" Width="154px" AutoPostBack="true" OnSelectedIndexChanged="ddMCAType_OnSelectedIndexChanged">                                    
                                    <asp:ListItem Text="<%$ Resources:Localization, OtherMCATypes %>" Value="Inspector1000" Selected="true"></asp:ListItem>
				<asp:ListItem Text="Serie 10/10+" Value="Serie10"></asp:ListItem>
                                </asp:DropDownList>
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, ContactForReport_Tip %>"></asp:Label></td> -->
                        </tr>                        
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="<%$ Resources:Localization, DateForReport %>" />
                                <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            </td>                                                        
                            <td>
                                <asp:TextBox ID="tbMeasureDate" runat="server"></asp:TextBox>                                
                                <act:CalendarExtender ID="calMeasureDate" runat="server" TargetControlID="tbMeasureDate" Format="dd.MM.yyyy"></act:CalendarExtender>
                                <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, DateForReport_Tip %>"></asp:Label></td> -->
                            </td>                            
                        </tr>                        
                        <tr>
                            <td><asp:Label ID="labelIntegralBackground" runat="server" Text="<%$ Resources:Localization, IntegralBackground %>" /></td>
                            <td>
                                <asp:TextBox ID="tbIntegralBackground" runat="server"></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbIntegralBackgroundFilter" 
                                    TargetControlID="tbIntegralBackground" 
                                    FilterType="Numbers" />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, IntegralBackground_Tip %>"></asp:Label></td> -->
                        </tr>
                        <tr>
                            <td><asp:Label ID="labelCountingBackground" runat="server" Text="<%$ Resources:Localization, CountingBackground %>" /></td>
                            <td>
                                <asp:TextBox ID="tbCountingBackground" runat="server"></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbCountingBackgroundFilter" 
                                    TargetControlID="tbCountingBackground" 
                                    FilterType="Numbers" />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, CountingBackground_Tip %>"></asp:Label></td> -->
                        </tr>
                        <tr>
                            <td><asp:Label ID="labelBackground" runat="server" Text="<%$ Resources:Localization, Background %>" /></td>
                            <td>
                                <asp:TextBox ID="tbBackground" runat="server"></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbBackgroundFilter" 
                                    TargetControlID="tbBackground" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="," />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Background_Tip %>"></asp:Label></td> -->
                        </tr>
                        <tr>
                            <td><asp:Label ID="labelGeometryFactor" runat="server" Text="<%$ Resources:Localization, GeometryFactor %>" /></td>
                            <td>
                                <asp:TextBox ID="tbGeometryFactor" runat="server"></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbGeometryFactorFilter" 
                                    TargetControlID="tbGeometryFactor" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="," />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, GeometryFactor_Tip %>"></asp:Label></td> -->
                        </tr>
                        <tr>
                            <td><asp:Label ID="labelAvgIntegralSample" runat="server" Text="<%$ Resources:Localization, AvgIntegralSample %>" /></td>
                            <td>
                                <asp:TextBox ID="tbAvgIntegralSample" runat="server"></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbAvgIntegralSampleFilter" 
                                    TargetControlID="tbAvgIntegralSample" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="," />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, AvgIntegralSample_Tip %>"></asp:Label></td> -->
                        </tr>
                        <tr>
                            <td><asp:Label runat="server" Text="<%$ Resources:Localization, AvgLivetimeSample %>" /></td>
                            <td>
                                <asp:TextBox ID="tbAvgLivetimeSample" runat="server"></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbAvgLivetimeSampleFilter" 
                                    TargetControlID="tbAvgLivetimeSample" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="," />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, AvgLivetimeSample_Tip %>"></asp:Label></td> -->
                        </tr>
                        <tr>
                            <td><asp:Label ID="labelActivity" runat="server" Text="<%$ Resources:Localization, Activity %>" /></td>
                            <td>
                                <asp:TextBox ID="tbActivity" runat="server"></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbActivityFilter" 
                                    TargetControlID="tbActivity" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="," />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Activity_Tip %>"></asp:Label></td> -->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="<%$ Resources:Localization, ReferenceDate %>" />
                                <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbRefDate" runat="server"></asp:TextBox>                                
                                <act:CalendarExtender ID="calRefDate" runat="server" TargetControlID="tbRefDate" Format="dd.MM.yyyy"></act:CalendarExtender>
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, ReferenceDate_Tip %>"></asp:Label></td> -->
                        </tr>                        
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="<%$ Resources:Localization, ActivityReference %>" />
                                <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbActivityRef" runat="server"></asp:TextBox>                                
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbActivityRefFilter" 
                                    TargetControlID="tbActivityRef" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="," />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, ActivityReference_Tip %>"></asp:Label></td> -->
                        </tr>
                        <tr>
                            <td><asp:Label runat="server" ID="labelUncertainty" Text="<%$ Resources:Localization, Uncertainty_serie10 %>" /></td>
                            <td>
                                <asp:TextBox ID="tbUncertainty" runat="server"></asp:TextBox>
                                <act:FilteredTextBoxExtender runat="server" 
                                    ID="tbUncertaintyFilter" 
                                    TargetControlID="tbUncertainty" 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="," />
                            </td>                            
                            <!-- <td><asp:Label runat="server" CssClass="TipText" Text="<%$ Resources:Localization, Uncertainty_Tip %>"></asp:Label></td> -->
                        </tr>                                                
                        <tr>
                            <td><asp:CheckBox ID="cbWantEvaluation" runat="server" Text="<%$ Resources:Localization, WantEvaluation %>"/></td>
                            <td><asp:CheckBox ID="cbAnswerByEmail" runat="server" Text="<%$ Resources:Localization, AnswerByEmail %>"/></td>
                        </tr>                        
                        <tr>
                            <td><asp:CheckBox ID="cbEvaluated" runat="server" Text="<%$ Resources:Localization, Evaluated %>" Enabled="false"/></td>                            
                            <td><asp:CheckBox ID="cbApproved" runat="server" Text="<%$ Resources:Localization, Approved %>" Enabled="false"/></td>                            
                        </tr>                                                                        
                        </table>                                                                                                 
                        
                        <table>
                        <tr>
                            <td>
                                <asp:Button ID="buttonRingtestReportUpdate" runat="server" Text="<%$ Resources:Localization, UpdateReport %>" OnClick="buttonRingtestReportUpdate_OnClick" CssClass="ButtonClass" Width="150px"/>                                                        
                                <asp:Button ID="buttonRingtestReportCancel" runat="server" Text="<%$ Resources:Localization, Cancel %>" OnClick="buttonRingtestReportCancel_OnClick" CssClass="ButtonClass" Width="150px"/>                                                        
                            </td>
                        </tr>
                        </table> 
                        
                        <br /><br /><hr /><br />
                                                
                        <center><b>Rapport kommentarer</b></center>
                        <br />
                        <b>Her kan du legge til kommentarer fortløpende, uavhengig av rapporten ovenfor</b>
                        <br /><br />
                        
                        <table width="100%" cellpadding="0" cellspacing="0">                            
                            <tr>                                
                                <td style="width:80%">                                                                        
                                    <asp:TextBox ID="tbComment" runat="server" Width="770px" MaxLength="995" BorderStyle="Solid" BorderWidth="1px" Style="margin-right:1px"></asp:TextBox>                                                                    
                                </td>                                
                                <td style="width:20%">
                                    <asp:Button ID="buttonAddComment" runat="server" Width="100%" Text="<%$ Resources:Localization, AddComment %>" OnClick="buttonAddComment_OnClick" CssClass="ButtonClass"></asp:Button>                                        
                                </td>
                            </tr>                                                        
                        </table>                                                
                        
                        <table width="100%" cellpadding="0" cellspacing="0">                            
                            <tr>
                                <td style="width:100%">
                                    <asp:Panel ID="Panel1" runat="server" Width="100%" Height="144px" ScrollBars="Vertical" BorderStyle="Solid" BorderWidth="1px" Style="margin-top:1px">
                                        <asp:GridView ID="gridComments" runat="server" Font-Size="X-Small"                                    
                                            DataKeyNames="ID" 
                                            AutoGenerateColumns="false" 
                                            ShowHeader="false" 
                                            ShowFooter="false" 
                                            GridLines="None">
                                            <Columns>                                    
                                                <asp:BoundField DataField="dateCreated" SortExpression="dateCreated" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" DataFormatString="[{0:dd.MM.yyyy}]&nbsp;" HtmlEncode="false" />
                                                <asp:BoundField DataField="vchContactName" DataFormatString="{0}:" HtmlEncode="false" ItemStyle-Wrap="false" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" SortExpression="vchContactName" />                                        
                                                <asp:BoundField DataField="textComment" SortExpression="textComment" ItemStyle-HorizontalAlign="Left"/>                                        
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>               
                                </td>                                                            
                            </tr>
                        </table>
                        
                        <br />
                        
                        <center><asp:Label ID="labelStatusReport" runat="server" CssClass="StatusMessageClass"/></center>
                        
                        <asp:SqlDataSource ID="dataSourceRingtestActors" runat="server"                             
                            ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>" 
                            SelectCommand="SELECT ID, vchName FROM Contact WHERE AccountID = @accountID AND vchStatus IN('Active', 'Unknown')"
                            ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                                                
                            <SelectParameters>
                                <asp:ControlParameter Name="accountID" ControlID="hiddenAccountID" PropertyName="Value"/>
                            </SelectParameters>
                        </asp:SqlDataSource>                                                                                                           
    
                    </asp:View>
                    
                </asp:MultiView>                                
                        
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>                                                                                            
                                
            </ContentTemplate>
            
        </act:TabPanel>
        
        <act:TabPanel ID="tabResults" runat="server" HeaderText="<%$ Resources:Localization, OurRingtestResults %>">
        
            <ContentTemplate> 
            
                <div style="margin:8px">
                
                    <div style="height:30px">
                    </div>
                                                   
                    <zgw:ZedGraphWeb ID="graphResults" runat="server" Width="900" Height="550" RenderMode="ImageTag" OnRenderGraph="graphResults_OnRenderGraph"></zgw:ZedGraphWeb>
                
                </div>
                
            </ContentTemplate>
            
        </act:TabPanel>
        
        <act:TabPanel ID="tabStatistics" runat="server" HeaderText="<%$ Resources:Localization, RingtestStatistics %>">
        
            <ContentTemplate>      
            
                <asp:UpdatePanel ID="updatePanelStatistics" runat="server">
                <ContentTemplate>        
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelStatistics" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">
                 
                <div style="height:30px">
                    <center>
                        <asp:Label runat="server" Text="<%$ Resources:Localization, Statistics_select_year %>" Font-Bold="true"></asp:Label>
                        
                        <asp:DropDownList ID="ddStatistics" runat="server" 
                            DataSourceID="dataSourceStatisticsYear" 
                            DataTextField="year1" DataValueField="year2" 
                            AutoPostBack="true" 
                            Width="120px"
                            OnSelectedIndexChanged="ddStatistics_OnSelectedIndexChanged">
                        </asp:DropDownList>                                                                                      
                    </center>
                </div>               
                
                <zgw:ZedGraphWeb ID="graphStatistics" runat="server"                      
                    Width="900" 
                    Height="550" 
                    RenderMode="ImageTag" 
                    OnRenderGraph="graphStatistics_OnRenderGraph">                    
                </zgw:ZedGraphWeb>                                
                
                <asp:SqlDataSource ID="dataSourceStatisticsYear" runat="server"                             
                    ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>" 
                    SelectCommand="SELECT DISTINCT(Ringtest.intYear) as year1, Ringtest.intYear as year2 FROM Ringtest, RingtestReport WHERE RingtestReport.AccountID = @accountID AND Ringtest.ID = RingtestReport.RingtestID ORDER BY Ringtest.intYear DESC"
                    ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                                                
                    <SelectParameters>
                        <asp:ControlParameter Name="accountID" ControlID="hiddenAccountID" PropertyName="Value"/>
                    </SelectParameters>
                </asp:SqlDataSource>           
    
                <br />
                <center><asp:Label ID="labelStatusStatistics" runat="server" CssClass="StatusMessageClass" /></center>                
                            
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
            
        </act:TabPanel>
        
    </act:TabContainer>                    
    
    <br />
    <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>                    
    
</asp:Content>

