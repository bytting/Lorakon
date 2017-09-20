<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Course.aspx.cs" Inherits="Course" Title="LorakonAdmin - Kurs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">    
    <script type="text/javascript">
        function OnTabChanged(sender, args)
        {
            sender.get_clientStateField().value = sender.saveClientState();                                                
        }                                                                                                          
    </script>          
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="phMain" Runat="Server">

    <act:TabContainer ID="tabContainerCourse" runat="server" OnClientActiveTabChanged="OnTabChanged">            
        
        <act:TabPanel ID="tabCreate" runat="server" HeaderText="Opprett kurs">
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
    
                <table width="100%">
                    <tr>
                        <th align="left">Opprett kurs</th>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Tittel" CssClass="TipText"></asp:Label>
                            <asp:Label runat="server" Text="*" CssClass="TipText" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:TextBox ID="textBoxCourseTitle" runat="server" MaxLength="511" CssClass="TextBoxClass2x"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxCourseTitleFilter" 
                                TargetControlID="textBoxCourseTitle" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-.,_/(): " />
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Kommentar" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="textBoxCourseComment" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Beskrivelse" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="textBoxCourseDescription" runat="server" TextMode="MultiLine" Width="100%" Height="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>                    
                        <td>                        
                            <asp:Button ID="buttonAddCourse" runat="server" Text="Opprett kurs" OnClick="buttonAddCourse_OnClick" CssClass="ButtonClass"/>
                        </td>
                    </tr>                    
                </table>                                                                                
                
                <br />                    
                
                <center>
                    <asp:Label ID="labelStatusCreate" runat="server" CssClass="StatusMessageClass" />
                </center>
                
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                    
            </ContentTemplate>
        </act:TabPanel>
        
        <act:TabPanel ID="tabEdit" runat="server" HeaderText="Rediger kurs">
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
                
                <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td>
                
                <table>
                    <tr>
                        <th align="left">Rediger kurs</th>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:CheckBox ID="cbCompletedOnly" runat="server" 
                                Checked="false" 
                                Text="Merk av her for å vise fullførte kurs" 
                                AutoPostBack="true"
                                CssClass="TipText" 
                                OnCheckedChanged="cbCompletedOnly_OnCheckChanged"/>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Velg kurs" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:DropDownList ID="ddCourses" runat="server" 
                                CssClass="TextBoxClass2x"
                                AutoPostBack="true" 
                                DataSourceID="dataSourceCourses" 
                                DataTextField="vchTitle" 
                                DataValueField="ID" 
                                OnSelectedIndexChanged="ddCourses_OnSelectedIndexChanged" 
                                OnDataBound="ddCourses_OnDataBound" />
                        </td>
                    </tr>
                    <tr>                    
                        <td>
                            <asp:Label runat="server" Text="Tittel" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="textBoxCourseTitleUpd" runat="server" CssClass="TextBoxClass2x" MaxLength="511"></asp:TextBox>
                            <act:FilteredTextBoxExtender runat="server" 
                                ID="textBoxCourseTitleUpdFilter" 
                                TargetControlID="textBoxCourseTitleUpd" 
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                ValidChars="ÆØÅæøå-.,_/(): " />
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <asp:Label runat="server" Text="Kommentar" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="textBoxCourseCommentUpd" runat="server" CssClass="TextBoxClass2x"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>                        
                        <td align="left">
                            <asp:Label runat="server" Text="Beskrivelse" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:TextBox ID="textBoxCourseDescriptionUpd" runat="server" TextMode="MultiLine" CssClass="TextBoxClass2x" Height="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>                    
                        <td>
                            <asp:CheckBox ID="checkBoxCourseCompleteUpd" runat="server" Text="Merk av her hvis kurset er fullført" CssClass="TipText"/>
                        </td>
                    </tr>                        
                    <tr>
                        <td>
                            <asp:Button ID="buttonUpdateCourse" runat="server" 
                                Text="Oppdater kurs informasjon" 
                                OnClick="buttonUpdateCourse_OnClick" 
                                CssClass="ButtonClass"/>                        
                            <asp:Button ID="buttonGenerateEmailAddresses" runat="server" 
                                Text="Vis adresseliste for deltagere" 
                                OnClick="buttonGenerateEmailAddresses_OnClick" 
                                CssClass="ButtonClass"/> 
                        </td>
                    </tr>
                </table>                                                    
                </td>
                <td>
                <table>
                    <tr>                            
                        <td>
                            <asp:Label runat="server" Text="Påmeldte" CssClass="TipText"></asp:Label>                            
                            <br />
                            <asp:ListBox ID="lbMembers" runat="server" CssClass="TextBoxClass2x" Height="350px"></asp:ListBox>                
                        </td>
                    </tr>
                </table>                                                    
                </td>
                </tr>
                <tr>
                <td>
                                
                <asp:TextBox ID="tbEmailAddresses" runat="server" Visible="false" Width="100%"></asp:TextBox>                                                                                                               
                </td>
                </tr>
                
                </table>                
                
                <br />
                
                <center><asp:Label ID="labelStatusEdit" runat="server" CssClass="StatusMessageClass" /></center>
                
                <asp:SqlDataSource ID="dataSourceCourses" runat="server" 
                    SelectCommand="SELECT * FROM Course WHERE bitCompleted = @completed" 
                    ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>">
                    <SelectParameters>
                        <asp:ControlParameter Name="completed" ControlID="cbCompletedOnly" PropertyName="Checked"/>
                    </SelectParameters>
                </asp:SqlDataSource>
                
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
            
        </act:TabPanel>
        
        
        <act:TabPanel ID="tabShowCourses" runat="server" HeaderText="Vis alle kurs">
            <ContentTemplate>
            
                <asp:UpdatePanel ID="updatePanelShowCourses" runat="server">
                <ContentTemplate>
                
                <div style="float:right;margin:8px">
                    <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelShowCourses" DisplayAfter="0">
                        <ProgressTemplate>
                            <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                
                <div style="margin:8px">
                
                <table width="100%">
                <tr>
                    <th align="left">Vis alle kurs</th>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="cbIncludeCompletedCourses" runat="server" 
                        Checked="false" 
                        AutoPostBack="true" 
                        Text="Inkluder fullførte"
                        CssClass="TipText"
                        OnCheckedChanged="cbIncludeCompletedCourses_OnCheckChanged"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridShowCourses" runat="server" 
                            Font-Size="X-Small" 
                            DataSourceID="dataSourceShowCourses" 
                            DataKeyNames="ID" 
                            AllowSorting="true" 
                            AutoGenerateColumns="false" 
                            Width="100%" 
                            GridLines="None" 
                            CellSpacing="2">
                            
                            <HeaderStyle HorizontalAlign="Left" />
                            
                            <Columns>
                                <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" Visible="false"/>
                                <asp:BoundField DataField="vchTitle" SortExpression="vchTitle" HeaderText="Tittel" HeaderStyle-HorizontalAlign="left"/>
                                <asp:BoundField DataField="textComment" SortExpression="textComment" HeaderText="Kommentar" HeaderStyle-HorizontalAlign="left"/>                                
                                <asp:CheckBoxField DataField="bitCompleted" SortExpression="bitCompleted" HeaderText="Fullført" HeaderStyle-HorizontalAlign="left"/>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                </table>                                                
                
                <asp:SqlDataSource ID="dataSourceShowCourses" runat="server" 
                    SelectCommand="SELECT ID, vchTitle, textComment, bitCompleted FROM Course WHERE bitCompleted = 0 ORDER BY vchTitle" 
                    ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>">                    
                </asp:SqlDataSource>
                
                </div>
                
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>            
        </act:TabPanel>
        
    </act:TabContainer>        
    
</asp:Content>

