<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="SpectrumFiles.aspx.cs" Inherits="SpectrumFiles" ValidateRequest="false" EnableEventValidation="false" Title="LorakonAdmin - Spektrum" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="phMain" Runat="Server">

    <act:TabContainer ID="tabContainerSpectrums" runat="server">
        
        <act:TabPanel ID="tabSpectrums" runat="server" HeaderText="Spekter">
    
        <ContentTemplate>
            <br />
            <asp:DropDownList ID="ddUsers" runat="server"                 
                CssClass="TextBoxClass2x"
                AutoPostBack="true" 
                DataSourceID="dataSourceAccounts"
                DataTextField="vchName"
                DataValueField="ID"                
                OnSelectedIndexChanged="ddUsers_OnSelectedIndexChanged"/>

            <asp:DropDownList ID="ddSpectrumYear" runat="server"
                OnSelectedIndexChanged="ddSpectrumYear_SelectedIndexChanged" 
                AutoPostBack="true">
            </asp:DropDownList>

            <asp:SqlDataSource ID="dataSourceAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:nrpa_lorakon %>"                    
                SelectCommand="SELECT ID, vchName FROM Account WHERE bitActive=1 ORDER BY vchName ASC"
                ProviderName="<%$ ConnectionStrings:nrpa_lorakon.ProviderName %>">                                        
            </asp:SqlDataSource>

            <br /><br />

            <asp:Panel ID="panelSpectrums" runat="server" Width="100%" Height="512px" ScrollBars="Vertical" BorderStyle="None">
                <asp:GridView ID="gridSpectrums" runat="server"
                    Font-Size="X-Small"
                    AllowSorting="true"
                    Width="96%" 
                    GridLines="None" 
                    AutoGenerateColumns="false" 
                    DataKeyNames="ID"
                    DataSourceID="dataSourceSpectrums"
                    OnRowCommand="gridSpectrums_RowCommand">
            
                    <HeaderStyle HorizontalAlign="Left" />
            
                    <Columns>                                                       
                        <asp:BoundField DataField="AccountID" HeaderText="AccountID" SortExpression="AccountID" HeaderStyle-HorizontalAlign="Left" Visible="false" />
                        <asp:BoundField DataField="ExternalID" HeaderText="ID" SortExpression="ExternalID" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="Opprettet dato" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="AcquisitionDate" SortExpression="AcquisitionDate" HeaderText="Måledato" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Operator" SortExpression="Operator" HeaderText="Operator" HeaderStyle-HorizontalAlign="Left"/>                        
                        <asp:BoundField DataField="Approved" SortExpression="Approved" HeaderText="Godkj." HeaderStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="ApprovedStatus" SortExpression="ApprovedStatus" HeaderText="Godkj.Status" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="Rejected" SortExpression="Rejected" HeaderText="Forkastet" HeaderStyle-HorizontalAlign="Left"/>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDownloadSpectrum" runat="server" CommandName="DownloadSpectrum" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="Spektrum" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDownloadReport" runat="server" CommandName="DownloadReport" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="Rapport" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="Spektrum ID" SortExpression="ID" />
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
                    <asp:ControlParameter Name="AccountID" ControlID="ddUsers" DbType="Guid" />
                    <asp:Parameter Name="year" DbType="Int32" />
                </SelectParameters>             
            </asp:SqlDataSource>             

        </ContentTemplate>    
        </act:TabPanel>

        <act:TabPanel ID="tabRules" runat="server" HeaderText="Import regler">
    
        <ContentTemplate>

            <asp:UpdatePanel ID="updatepanelImportRules" runat="server">

                <ContentTemplate>
                    <asp:Label runat="server">Import regler for nuklider</asp:Label>
                    <br />
                    <asp:GridView ID="gridViewImportRules" runat="server" Width="550px" AutoGenerateColumns="false"
                        AlternatingRowStyle-BackColor="#e0e0e0" AllowPaging="true" ShowFooter="true" 
                        OnPageIndexChanging="OnPaging" onrowediting="EditImportRule" onrowupdating="UpdateImportRule" 
                        onrowcancelingedit="CancelEditImportRule" PageSize="10" GridLines="None" Font-Size="X-Small">
                    <Columns>
                    <asp:TemplateField HeaderText="ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                        </ItemTemplate>        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nuklide">
                        <ItemTemplate>
                            <asp:Label ID="lblNuclideName" runat="server" Text='<%# Eval("NuclideName")%>'></asp:Label>
                        </ItemTemplate>    
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNuclideName" runat="server" Text='<%# Eval("NuclideName")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNuclideNameNew" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Aktivitet Min">
                        <ItemTemplate>
                            <asp:Label ID="lblActivityMin" runat="server" Text='<%# Eval("ActivityMin")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtActivityMin" runat="server" Text='<%# Eval("ActivityMin")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtActivityMinNew" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Aktivitet Max">
                        <ItemTemplate>
                            <asp:Label ID="lblActivityMax" runat="server" Text='<%# Eval("ActivityMax")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtActivityMax" runat="server" Text='<%# Eval("ActivityMax")%>'></asp:TextBox>
                        </EditItemTemplate>     
                        <FooterTemplate>
                            <asp:TextBox ID="txtActivityMaxNew" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Konfidens Min">
                        <ItemTemplate>
                            <asp:Label ID="lblConfidenceMin" runat="server" Text='<%# Eval("ConfidenceMin")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtConfidenceMin" runat="server" Text='<%# Eval("ConfidenceMin")%>'></asp:TextBox>
                        </EditItemTemplate>     
                        <FooterTemplate>
                            <asp:TextBox ID="txtConfidenceMinNew" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kan Auto Godkj.">
                        <ItemTemplate>                            
                            <asp:CheckBox ID="cbCanBeAutoApprovedDisabled" runat="server" Checked='<%# Eval("CanBeAutoApproved")%>' Enabled="false"></asp:CheckBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="cbCanBeAutoApproved" runat="server" Checked='<%# Eval("CanBeAutoApproved")%>'></asp:CheckBox>
                        </EditItemTemplate>     
                        <FooterTemplate>
                            <asp:CheckBox ID="cbCanBeAutoApprovedNew" runat="server"></asp:CheckBox>
                    </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbRemove" runat="server" CommandArgument='<%# Eval("NuclideName")%>'
                                OnClientClick="return confirm('Do you want to delete?')" Text="Slett" OnClick="DeleteImportRule"></asp:LinkButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="btnAddImportRule" runat="server" Text="Opprett" OnClick="AddNewImportRule" />
                        </FooterTemplate>                        
                    </asp:TemplateField>
                    <asp:CommandField  ShowEditButton="True" />
                    </Columns>                    
                        <EmptyDataTemplate>
                            <table>
                                <tr>
                                    <td>Nuklide</td>                                                                    
                                    <td>Aktivitet Min.</td>                                                                    
                                    <td>Aktivitet Max.</td>                                                                    
                                    <td>Konfidens Min.</td>                                                                    
                                    <td>Kan Auto Godkj.</td>                                    
                                </tr>                                
                                <tr>
                                    <td><asp:TextBox ID="txtNuclideNameFirst" runat="server"></asp:TextBox></td>
                                    <td><asp:TextBox ID="txtActivityMinFirst" runat="server"></asp:TextBox></td>
                                    <td><asp:TextBox ID="txtActivityMaxFirst" runat="server"></asp:TextBox></td>
                                    <td><asp:TextBox ID="txtConfidenceMinFirst" runat="server"></asp:TextBox></td>
                                    <td><asp:CheckBox ID="cbCanBeAutoApprovedFirst" runat="server"></asp:CheckBox></td>
                                    <td><asp:LinkButton ID="btnAddImportRuleFirst" runat="server" Text="Opprett" OnClick="AddFirstImportRule" /></td>
                                </tr>
                            </table>                                                                                                                                                                                                                                                            
                        </EmptyDataTemplate>
                    </asp:GridView>                    
                
                    <br /><br />
                    
                    <asp:Label runat="server">Import regler for geometrier</asp:Label>
                    <br />
                    <asp:GridView ID="gridViewGeomRules" runat="server" Width="550px" AutoGenerateColumns="false"
                        AlternatingRowStyle-BackColor="#e0e0e0" AllowPaging="true" ShowFooter="true" 
                        OnPageIndexChanging="OnGeomPaging" onrowediting="EditGeomRule" onrowupdating="UpdateGeomRule" 
                        onrowcancelingedit="CancelEditGeomRule" PageSize="10" GridLines="None" Font-Size="X-Small">
                    <Columns>
                    <asp:TemplateField HeaderText="ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                        </ItemTemplate>        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Geometri">
                        <ItemTemplate>
                            <asp:Label ID="lblGeometryName" runat="server" Text='<%# Eval("Geometry")%>'></asp:Label>
                        </ItemTemplate>    
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGeometryName" runat="server" Text='<%# Eval("Geometry")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtGeometryNameNew" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enhet">
                        <ItemTemplate>
                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnit" runat="server" Text='<%# Eval("Unit")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtUnitNew" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Minimum">
                        <ItemTemplate>
                            <asp:Label ID="lblMinimum" runat="server" Text='<%# Eval("Minimum")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMinimum" runat="server" Text='<%# Eval("Minimum")%>'></asp:TextBox>
                        </EditItemTemplate>     
                        <FooterTemplate>
                            <asp:TextBox ID="txtMinimumNew" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Maximum">
                        <ItemTemplate>
                            <asp:Label ID="lblMaximum" runat="server" Text='<%# Eval("Maximum")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMaximum" runat="server" Text='<%# Eval("Maximum")%>'></asp:TextBox>
                        </EditItemTemplate>     
                        <FooterTemplate>
                            <asp:TextBox ID="txtMaximumNew" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbRemoveGeom" runat="server" CommandArgument='<%# Eval("Geometry")%>'
                                OnClientClick="return confirm('Do you want to delete?')" Text="Slett" OnClick="DeleteGeomRule"></asp:LinkButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="btnAddGeomRule" runat="server" Text="Opprett" OnClick="AddNewGeomRule" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:CommandField  ShowEditButton="True" />                        
                    </Columns>                    
                    <EmptyDataTemplate>
                        <table>
                            <tr>
                                <td>Geometri</td>                                                                    
                                <td>Enhet</td>                                                                    
                                <td>Minimum</td>                                                                    
                                <td>Maksimum</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtGeometryNameFirst" runat="server"></asp:TextBox></td>                                
                                <td><asp:TextBox ID="txtUnitFirst" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtMinimumFirst" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtMaximumFirst" runat="server"></asp:TextBox></td>
                                <td><asp:LinkButton ID="btnAddGeomRuleFirst" runat="server" Text="Opprett" OnClick="AddFirstGeomRule" /></td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    </asp:GridView>

                    <center><asp:Label ID="labelStatusRules" runat="server" CssClass="StatusMessageClass" ForeColor="Red" /></center>
                </ContentTemplate>

                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gridViewImportRules" />
                    <asp:AsyncPostBackTrigger ControlID="gridViewGeomRules" />
                </Triggers>

            </asp:UpdatePanel>

        </ContentTemplate>

        </act:TabPanel>
    </act:TabContainer>

    <div style="clear:both">
        <br />
        <center><asp:Label ID="labelStatus" runat="server" CssClass="StatusMessageClass" /></center>
    </div>            

</asp:Content>

