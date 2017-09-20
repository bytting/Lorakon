<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Units.aspx.cs" Inherits="Units" Title="<%$ Resources:Localization, Title_Units %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">            
    
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
                
    <div>        
        <asp:Label runat="server" Font-Bold="true" Text="<%$ Resources:Localization, Units_header %>"></asp:Label>
    </div>               
    
    <br />
    
    <asp:Panel ID="panelDevices" runat="server" Width="100%" Height="512px" ScrollBars="Vertical" BorderStyle="None">
        <asp:GridView ID="gridDevices" runat="server" 
            Font-Size="X-Small"
            AllowSorting="true"
            Width="96%" 
            GridLines="None" 
            AutoGenerateColumns="false" 
            DataKeyNames="ID" 
            DataSourceID="dataSourceDevices">                    
            
            <HeaderStyle HorizontalAlign="Left" />
            
            <Columns>                               
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false" />
                <asp:BoundField DataField="categoryName" SortExpression="categoryName" HeaderText="<%$ Resources:Localization, Category %>" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="typeName" SortExpression="typeName" HeaderText="<%$ Resources:Localization, Type %>" HeaderStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="vchSerialNumber" SortExpression="vchSerialNumber" HeaderText="<%$ Resources:Localization, Serialnumber %>" HeaderStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="vchStatus" SortExpression="vchStatus" HeaderText="<%$ Resources:Localization, Status %>" HeaderStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="vchOwnership" SortExpression="vchOwnership" HeaderText="<%$ Resources:Localization, Ownership %>" HeaderStyle-HorizontalAlign="Left"/>            
            </Columns>
        </asp:GridView>
    </asp:Panel>
            
    <asp:SqlDataSource ID="dataSourceDevices" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"                                        
        SelectCommand="SELECT Device.ID, DeviceCategory.vchName as categoryName, DeviceType.vchName as typeName, Device.vchSerialNumber, Device.vchStatus, Device.vchOwnership FROM Device, DeviceCategory, DeviceType WHERE Device.AccountID = @accountID AND Device.DeviceCategoryID = DeviceCategory.ID AND Device.DeviceTypeID = DeviceType.ID ORDER BY categoryName"
        ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                
    </asp:SqlDataSource> 
          
    <br /> 
                       
    <center>
        <asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" />
    </center>
    
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
                        
</asp:Content>

