<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Course.aspx.cs" Inherits="Course" Title="<%$ Resources:Localization, Title_Course %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="phMain" Runat="Server">                
        
    <asp:UpdatePanel ID="updatePanelMain" runat="server">
    <ContentTemplate>

    <asp:HiddenField ID="hiddenAccountID" runat="server"/>
    
    <div style="float:right;margin:8px">
        <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelMain" DisplayAfter="0">
            <ProgressTemplate>
                <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <div style="margin:8px">        
    
    <div>        
        <asp:Label runat="server" Font-Bold="true" Text="<%$ Resources:Localization, Course_header %>"></asp:Label>
        <br />
        <asp:Label ID="labelNoCouses" runat="server" Font-Bold="true" ForeColor="DarkGray"></asp:Label>
    </div>
    
    <br />
    
    <asp:GridView ID="gridCourse" runat="server" 
        Font-Size="X-Small"
        AllowSorting="true" 
        Width="100%"
        DataSourceID="dataSourceCourse" 
        AutoGenerateColumns="false"         
        GridLines="None"
        OnSelectedIndexChanged="gridCourse_OnSelectedIndexChanged" 
        OnRowDataBound="gridCourse_OnRowDataBound" 
        DataKeyNames="ID">        
        
        <HeaderStyle HorizontalAlign="Left" />
        <SelectedRowStyle BackColor="#FFFFFF" />                         
                                            
        <Columns>                    
            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"/>
            <asp:BoundField DataField="vchTitle" HeaderText="<%$ Resources:Localization, Course %>" SortExpression="vchTitle" HeaderStyle-HorizontalAlign="Left"/>
            <asp:BoundField DataField="textComment" HeaderText="<%$ Resources:Localization, Comment %>" SortExpression="textComment" HeaderStyle-HorizontalAlign="Left"/>
            <asp:BoundField DataField="textDescription" HeaderText="<%$ Resources:Localization, Description %>" SortExpression="textDescription" HeaderStyle-HorizontalAlign="Left"/>
        </Columns>
        
    </asp:GridView>                 
    
    <br />
    
    <asp:GridView ID="gridCourseActors" runat="server"      
        DataSourceID="dataSourceCourseActors" 
        AutoGenerateColumns="false" 
        DataKeyNames="ID" 
        GridLines="None" 
        Visible="false">                    
        
        <HeaderStyle HorizontalAlign="Left" />
        
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"/>
            <asp:TemplateField HeaderText="<%$ Resources:Localization, Member %>" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:CheckBox ID="cbCourseActorBit" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbCourseActorBit_OnCheckChanged"/>                                                            
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:BoundField DataField="vchName" HeaderText="<%$ Resources:Localization, Name %>" SortExpression="vchName" HeaderStyle-HorizontalAlign="Left" />                    
        </Columns>
        
    </asp:GridView>                
    
    <asp:SqlDataSource ID="dataSourceCourse" runat="server" 
        SelectCommand="SELECT * FROM Course WHERE bitCompleted = 0" 
        ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>" 
        ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">
    </asp:SqlDataSource>        
            
    <asp:SqlDataSource ID="dataSourceCourseActors" runat="server" 
        SelectCommand="SELECT * FROM Contact, Account WHERE Contact.AccountID = @accountID AND Contact.vchStatus IN ('Active', 'Unknown') AND Account.ID = @accountID AND Account.bitActive = 1" 
        ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>" 
        ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">
        <SelectParameters>
            <asp:ControlParameter Name="accountID" ControlID="hiddenAccountID" PropertyName="Value"/>
        </SelectParameters>        
    </asp:SqlDataSource>           
    
    <br />
    
    <center>
        <asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" />
    </center>    
    
    </div>        
    
    </ContentTemplate>
    </asp:UpdatePanel>    
    
</asp:Content>

