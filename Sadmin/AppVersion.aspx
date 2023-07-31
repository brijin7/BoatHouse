<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="AppVersion.aspx.cs" Inherits="Sadmin_AppVersion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">App Version <span style="float: right;">
            <asp:LinkButton ID="lbtnBlockReason" CssClass="lbtnNew" runat="server" OnClick="lbtnBlockReason_Click"> 
                
                <i class="fas fa-receipt"></i>View Report</asp:LinkButton>
            <asp:LinkButton ID="lnkback" CssClass="lbtnNew" runat="server" Visible="false" OnClick="lnkback_Click"> 
                     <i class="fas fa-receipt"></i>View Back</asp:LinkButton>
        </span></h5>

        <hr />
        <div class="mydivbrdr" runat="server" id="divEntry">
            <div class="row p-2">
                <div class="col-sm-12 col-xs-12">
                    <div class="row m-0">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label>App Type</label>
                                <asp:DropDownList ID="ddlapp" runat="server" CssClass="form-control inputboxstyle"
                                    MaxLength="10" TabIndex="4">
                                    <asp:ListItem Value="0">Select App Type</asp:ListItem>
                                    <asp:ListItem Value="A">Android</asp:ListItem>
                                    <asp:ListItem Value="AD">Android Department</asp:ListItem>
                                    <asp:ListItem Value="I">IOS</asp:ListItem>
                                    <asp:ListItem Value="T">TV</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlapp" InitialValue="0"
                                    ValidationGroup="A" SetFocusOnError="True" CssClass="vError" ErrorMessage="Please, Select App Type."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12" id="divBoatHouse" runat="server">
                            <div class="form-group">
                                <label id="App Version">App Version</label>
                                <asp:TextBox runat="server" ID="txtversion" CssClass="form-control " TabIndex="2" MaxLength="10" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtversion"
                                    ValidationGroup="A" SetFocusOnError="True" CssClass="vError" ErrorMessage="Please, Enter App Version."></asp:RequiredFieldValidator>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12" style="margin-top: 25px">
                            <%-- <div class="form-submit" style="text-align: left !important">--%>
                            <div class="form-group">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="A" TabIndex="3" OnClick="btnSubmit_Click" />

                            </div>
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
                <asp:GridView ID="gvAppVersion" runat="server" AllowPaging="True"
                    CssClass="table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="AppType,VersionNo"
                    PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="App Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAppType" runat="server" Text='<%# Bind("AppType1") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Version No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblVersionNo" runat="server" Text='<%# Bind("VersionNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Bind("CreatedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Bind("CreatedDate1") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>



                    </Columns>
                    <HeaderStyle CssClass="gvHead" BackColor="#124a79" ForeColor="White" />
                    <AlternatingRowStyle CssClass="gvRow" BackColor="WhiteSmoke" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>

        <div id="divInactive" runat="server" class="col-sm-12 col-xs-12">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg1" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvInactive" runat="server" AllowPaging="True"
                    CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="AppType,VersionNo"
                    PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="App Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAppType" runat="server" Text='<%# Bind("AppType1") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Version No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblVersionNo" runat="server" Text='<%# Bind("VersionNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Bind("CreatedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Bind("CreatedDate1") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Updated By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUpdatedBy" runat="server" Text='<%# Bind("UpdatedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Updated Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUpdatedDate" runat="server" Text='<%# Bind("UpdatedDate1") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <%-- Newly implemented for CSRF Validation--%>
    
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

