<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Masters/ImportantContacts.aspx.cs" Inherits="ImportantContacts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="form-body col-sm-9 col-xs-12">
        <h5 class="pghr">Important Contacts <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                        <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label for="lblcity">City</label>
                            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="1">
                                <asp:ListItem Value="0" Text="Select Item"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCity"
                                ValidationGroup="LocationAttraction" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select City</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class=" col-md-3 col-lg-3 col-sm-12" style="display: none">
                        <div class="form-group">
                            <label for="lblcontact">Contact</label>
                            <asp:TextBox ID="txtContactId" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="3" MaxLength="10">
                            </asp:TextBox>

                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label for="txtConfigurationType">Contact Type</label>
                            <asp:DropDownList ID="ddlCOntact" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="2">
                                <asp:ListItem Value="0">Select Contact</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCOntact"
                                ValidationGroup="LocationAttraction" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Contact</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label for="txtConfigurationType">Contact Info</label>
                            <asp:TextBox ID="txtcontactinfo" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="3" MaxLength="100">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtcontactinfo" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Special Characters Not Allowed" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtcontactinfo"
                                ValidationGroup="LocationAttraction" SetFocusOnError="True" CssClass="vError">Enter Contact Info</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-9 col-xs-12">
                        <div class="form-group">
                            <label for="lblcontact">Description</label>
                            <asp:TextBox ID="txtdes" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="4" MaxLength="300" TextMode="MultiLine">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtdes" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Special Characters Not Allowed" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtdes"
                                ValidationGroup="LocationAttraction" SetFocusOnError="True" CssClass="vError">Enter Description</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="LocationAttraction" TabIndex="5" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn  btn-danger" CausesValidation="false" TabIndex="6" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divgrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvImportantContacts" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="ContactId" PageSize="25000" OnRowDataBound="gvImportantContacts_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ContactId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblContactId" runat="server" Text='<%# Bind("ContactId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CityId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCityId" runat="server" Text='<%# Bind("CityId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCityName" runat="server" Text='<%# Bind("CityName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ContactTypeId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblContactTypeId" runat="server" Text='<%# Bind("ContactTypeId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblContactTypeName" runat="server" Text='<%# Bind("ContactTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Contact Info" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblContactInfo" runat="server" Text='<%# Bind("ContactInfo") %>'></asp:Label>
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
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

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

