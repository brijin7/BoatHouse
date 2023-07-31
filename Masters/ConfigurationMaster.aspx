<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Masters/ConfigurationMaster.aspx.cs" Inherits="ConfigurationMaster"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-8 col-xs-12">
        <h5 class="pghr">Common Masters <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span></h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label for="txtConfigurationType"><i class="fas fa-list"></i>Master Type <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlConfigType" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" TabIndex="1" OnSelectedIndexChanged="ddlConfigType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Master Type</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlConfigType"
                                ValidationGroup="Configuration" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Master Type</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label runat="server" id="VehicleId">Vehicle ID</label>
                            <asp:TextBox ID="txtConfigId" runat="server" CssClass="form-control inputboxstyle" Enabled="false"
                                MaxLength="150" Font-Size="14px" placeholder="Enter Config ID" TabIndex="2">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <i class="fas fa-landmark"></i>
                            <label runat="server" id="VehicleName">Configuration Name</label><span class="spStar">*</span>
                            <asp:TextBox ID="txtConfigName" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" Font-Size="14px" TabIndex="3" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtConfigName"
                                ValidationGroup="Configuration" SetFocusOnError="True" CssClass="vError">Enter Configuration Name</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="Configuration" CssClass="btn btn-primary" TabIndex="4" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="btn btn-danger" TabIndex="5" OnClick="btnCancel_Click" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divFilter" runat="server">
            <div class="mydivbrdr">
                <div class="row">
                    <div class="col-sm-2 col-xs-12">
                        <label for="txtConfigurationType"><i class="fas fa-filter"></i>Filter</label>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <asp:DropDownList ID="ddlConType" runat="server" CssClass="form-control inputboxstyle"
                            MaxLength="50" TabIndex="1" OnSelectedIndexChanged="ddlConType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Select Master Type</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvConfiguration" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
                    AutoGenerateColumns="False" DataKeyNames="typeId,configID" PageSize="10" OnRowDataBound="gvConfiguration_RowDataBound"
                    OnPageIndexChanging="gvConfiguration_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTypeId" runat="server" Text='<%# Bind("TypeId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Config.Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblConfigId" runat="server" Text='<%# Bind("ConfigId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Master Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTypeName" runat="server" Text='<%# Bind("TypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Configuration Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblConfigName" runat="server" Text='<%# Bind("ConfigName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');" />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfActiveStatus" runat="server" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

