<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptOnlinePaymentBlockRights.aspx.cs" Inherits="Reports_RptOnlinePaymentBlockRights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">Report - Online Paymemnt Block Rights <span style="float: right;">
            <asp:LinkButton ID="lbtnBack" CssClass="lbtnNew" runat="server" OnClick="lbtnBack_Click">
                <i class="fas fa-receipt"></i>Back to Block Rights</asp:LinkButton></span> </h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row" style="padding-left: 15px;">
                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <div class="form-group">
                                <label for="lblCategoryname" id="lblCategoryname"></label>

                                <asp:DropDownList ID="ddlReason" CssClass="form-control inputboxstyle" runat="server"
                                    TabIndex="2" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged" AutoPostBack="true">

                                    <asp:ListItem Value="1" Selected="True">Blocked</asp:ListItem>
                                    <asp:ListItem Value="2">UnBlocked</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divGrid" runat="server" class="col-sm-12 col-xs-12">
                <div class="table-responsive">
                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                        <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
                    <asp:GridView ID="gvPaymentRights" runat="server" AllowPaging="True"
                        CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                        DataKeyNames="UniqueId,ApplicationType,BranchType,BranchId,BranchName,BlockType,CreatedBy,BlockedDate"
                        PageSize="25000">
                        <columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Application Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblApplicationType" runat="server" Text='<%# Bind("ApplicationType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBranchType" runat="server" Text='<%# Bind("BranchType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Name" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("BranchName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Block Payment Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBlockType" runat="server" Text='<%# Bind("BlockType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Blocked Date" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBlockedDate" runat="server" Text='<%# Bind("BlockedDate") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Blocked Reason" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBlockedReason" runat="server" Text='<%# Bind("BlockReason") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnBlocked Date" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblUnBlockedDate" runat="server" Text='<%# Bind("UnBlockedDate") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnBlocked Reason" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblUnBlockedReason" runat="server" Text='<%# Bind("UnBlockReason") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                        </columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

