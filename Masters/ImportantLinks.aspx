<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Masters/ImportantLinks.aspx.cs" Inherits="ImportantLinks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="form-body col-sm-9 col-xs-12">
        <h5 class="pghr">Important Links <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12" style="display: none">
                        <div class="form-group">
                            <label runat="server" id="Label1">Link</label>
                            <asp:TextBox ID="txtLinkid" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" Font-Size="14px" TabIndex="1">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label2">Link Type</label>
                            <asp:DropDownList ID="ddllinktype" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="1">
                                <asp:ListItem Value="0">Select Link Type</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddllinktype"
                                ValidationGroup="Location" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Link Type</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label3">Link Name</label>
                            <asp:TextBox ID="txtlinkname" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                MaxLength="200" Font-Size="14px" TabIndex="2">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtlinkname" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Invalid" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtlinkname"
                                ValidationGroup="Location" SetFocusOnError="True" CssClass="vError">Enter Link Name</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-9 col-md-9 col-lg-9 col-xs-9">
                        <div class="form-group">
                            <label runat="server" id="Label4">Link Url</label>
                            <asp:TextBox ID="txtLinkUrl" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="200" Font-Size="14px" TabIndex="3" TextMode="MultiLine">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLinkUrl"
                                ValidationGroup="Location" SetFocusOnError="True" CssClass="vError">Enter Link Url</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>



                <div class="form-submit">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Location" TabIndex="4" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="5" OnClick="btnCancel_Click" />

                </div>
            </div>
        </div>
        <div id="divgrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvimportantlinks" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="LinkId" PageSize="25000" OnRowDataBound="gvimportantlinks_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LinkId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblLinkId" runat="server" Text='<%# Bind("LinkId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LinkType" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblLinkType" runat="server" Text='<%# Bind("LinkType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblLinkName" runat="server" Text='<%# Bind("LinkName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link URL" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblLinkURL" runat="server" Text='<%# Bind("LinkURL") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link Type Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblLinkTypeName" runat="server" Text='<%# Bind("LinkTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ActiveStatus" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgBtnDelete" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                runat="server" Font-Bold="true" ImageUrl="~/images/Delete.png" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this entry?');" />
                         <asp:ImageButton ID="ImgBtnUndo" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                runat="server" Font-Bold="true" ImageUrl="~/images/refresh.png" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure you want to Undo this entry?');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');"></asp:LinkButton>

                                <asp:LinkButton ID="ImgBtnUndo" runat="server" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfCreatedBy" runat="server" />

            <%-- Newly implemented for CSRF Validation--%>
        <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
        <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

