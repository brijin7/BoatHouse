<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="ViewTaxMaster.aspx.cs" Inherits="Boating_ViewTaxMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">Tax Master </h5>
        <hr />
    </div>

    <div class="form-body" id="divGrid" runat="server">
        <div class="table-responsive">
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
            <asp:GridView ID="gvTaxMaster" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced GridStyle"
                AutoGenerateColumns="False" DataKeyNames="TaxId" PageSize="25000" OnDataBound="gvTaxMaster_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Tax Id" HeaderStyle-CssClass="grdHead" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblTaxId" runat="server" Text='<%# Bind("TaxId") %>' Font-Bold="true"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="BoatHouseName" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Service Name | Tax Description  | Tax %" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Effective From" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblEffectiveFrom" runat="server" Text='<%# Bind("EffectiveFrom") %>' ForeColor="Green" Font-Bold="true"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Effective Till" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblEffectiveTill" runat="server" Text='<%# Bind("EffectiveTill") %>' ForeColor="Red" Font-Bold="true"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reference Number" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblRefNum" runat="server" Text='<%# Bind("RefNum") %>' Font-Bold="true"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Reference Date" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblRefDate" runat="server" Text='<%# Bind("RefDate") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Document" HeaderStyle-CssClass="grdHead" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDownload" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                runat="server" Font-Bold="true" OnClick="imgbtnDownload_Click" ImageUrl="~/images/download.png" CommandArgument='<%# Bind("RefDocLink") %>' ToolTip="Download Reference Document." />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Active Status" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gvHead" />
                <AlternatingRowStyle CssClass="gvRow" />
                <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
            </asp:GridView>
        </div>
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

