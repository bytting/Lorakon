<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="UploadSpectrums.aspx.cs" Inherits="UploadSpectrums" Title="<%$ Resources:Localization, Title_UploadSpectrums %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">
    
    <br /> 

    <div style="margin:8px">

    <div>
        <asp:Label ID="lblUploadInfo" runat="server" ForeColor="OliveDrab">OBS! Denne funksjonen er kun egnet for spekterfiler produsert med LORAKON-målesystemet (Genie-2000). </asp:Label>        
    </div> 
    <br />
    <div>
        <table>
            <tr>
                <td>Velg CHN filer for opplasting</td>  
                <td>  
                    <asp:FileUpload ID="fileUpload" runat="server" Multiple="Multiple" />
                </td>
                <td>  
                    <asp:Button ID="btnUpload" runat="server" Text="Last opp" OnClick="btnUpload_Click" CssClass="ButtonClass" />  
                </td>  
                <td>
                    <asp:Button ID="btnClearUpload" runat="server" Text="Tøm info" OnClick="btnClearUpload_Click" CssClass="ButtonClass" />  
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RegularExpressionValidator ID="fileUploadValidator" runat="server" ControlToValidate="fileUpload"
                        ErrorMessage="Kun .cnf filer er tillatt"
                        ValidationExpression="(.+\.([Cc][Nn][Ff]))">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>        
        </table>
    </div>

    <br />

    <div>        
        <asp:Table ID="tblUploadFiles" runat="server"></asp:Table>
    </div>

    </div>

    <br />

    <asp:UpdatePanel ID="updatePanelUpload" runat="server">
    <ContentTemplate>        
    
    <div style="float:right;margin:8px">
        <asp:UpdateProgress runat="server" DynamicLayout="false" AssociatedUpdatePanelID="updatePanelUpload" DisplayAfter="0">
            <ProgressTemplate>
                <asp:Image runat="server" ImageUrl="~/Images/progress.gif"></asp:Image>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <div style="margin:8px">
                
    <div>        
        <asp:Label runat="server" Font-Bold="true" Text="Vis tidligere opplastede filer"></asp:Label>
    </div>               
    
    <br />
    
    <asp:DropDownList ID="ddSpectrumYear" runat="server"
        OnSelectedIndexChanged="ddSpectrumYear_SelectedIndexChanged" 
        AutoPostBack="true">
    </asp:DropDownList>

    <asp:Panel ID="panelSpectrums" runat="server" Width="100%" Height="512px" ScrollBars="Vertical" BorderStyle="None">
        <asp:GridView ID="gridSpectrums" runat="server" 
            Font-Size="X-Small"
            AllowSorting="true"
            Width="96%" 
            GridLines="None" 
            AutoGenerateColumns="false" 
            DataKeyNames="ID" 
            DataSourceID="dataSourceSpectrums">
            
            <HeaderStyle HorizontalAlign="Left" />
            
            <Columns>                               
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false" />
                <asp:BoundField DataField="AccountID" HeaderText="<%$ Resources:Localization, AccountID %>" SortExpression="AccountID" HeaderStyle-HorizontalAlign="Left" Visible="false" />
                <asp:BoundField DataField="ExternalID" SortExpression="ExternalID" HeaderText="<%$ Resources:Localization, ID %>" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="<%$ Resources:Localization, CreateDate %>" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="AcquisitionDate" SortExpression="AcquisitionDate" HeaderText="<%$ Resources:Localization, MeasurementDate %>" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Operator" SortExpression="Operator" HeaderText="<%$ Resources:Localization, Operator %>" HeaderStyle-HorizontalAlign="Left"/>                
                <asp:BoundField DataField="Approved" SortExpression="Approved" HeaderText="<%$ Resources:Localization, Approved %>" HeaderStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="ApprovedStatus" SortExpression="ApprovedStatus" HeaderText="<%$ Resources:Localization, ApprovedStatus %>" HeaderStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="Rejected" SortExpression="Rejected" HeaderText="<%$ Resources:Localization, Rejected %>" HeaderStyle-HorizontalAlign="Left"/>
            </Columns>
        </asp:GridView>
    </asp:Panel>
                
    <asp:SqlDataSource ID="dataSourceSpectrums" 
        runat="server" 
        ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"        
        SelectCommandType="StoredProcedure"
        SelectCommand="proc_spectrum_info_select_latest_where_account_year"
        ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">   
        <SelectParameters>            
            <asp:Parameter Name="AccountID" DbType="Guid" />
            <asp:ControlParameter Name="year" ControlID="ddSpectrumYear" DbType="Int32" PropertyName="Text" />
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

