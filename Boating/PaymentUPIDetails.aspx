<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="PaymentUPIDetails.aspx.cs" Inherits="Boating_PaymentUPIDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row" style="margin-left: 1.5rem !important;">

        <div class="form-body col-sm-10 col-xs-12">
            <h5 class="pghr">Payment UPI Details </h5>
            <hr />
            <div class="mydivbrdr" runat="server">
                <div class="row m-2">
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="boat"><i class="fa fa-user" aria-hidden="true"></i>Merchant Name <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1" MaxLength="50" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                ValidationGroup="PayUPI" SetFocusOnError="True" CssClass="vError">Enter Merchant Name</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regName" runat="server" ControlToValidate="txtName"
                                ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter Valid Name" ForeColor="Red" CssClass="vError" />
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="boat"><i class="fa fa-mobile" aria-hidden="true"></i>Mobile No <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" TabIndex="2" MaxLength="10"
                                onkeypress="return isNumber(event)" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMobileNo"
                                ValidationGroup="PayUPI" SetFocusOnError="True" CssClass="vError">Enter Mobile No</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ControlToValidate="txtMobileNo" ErrorMessage="Enter Valid Mobile No"
                                ValidationExpression="[0-9]{10}" ValidationGroup="PayUPI" SetFocusOnError="True" CssClass="vError"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="boat"><i class="fa fa-money-check" aria-hidden="true"></i>UPI Id <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtUPIId" runat="server" CssClass="form-control" TabIndex="3" MaxLength="20" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUPIId"
                                ValidationGroup="PayUPI" SetFocusOnError="True" CssClass="vError">Enter UPI Id</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtUPIId"
                                ValidationExpression="^\w+@\w+$" Text="Enter valid UPI" ForeColor="Red" CssClass="vError" />
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="boat"><i class="fa fa-user" aria-hidden="true"></i>Merchant Code <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtMerchantCode" runat="server" CssClass="form-control" TabIndex="4" MaxLength="20" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMerchantCode"
                                ValidationGroup="PayUPI" SetFocusOnError="True" CssClass="vError">Enter Merchant Code</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="boat"><i class="fa fa-user" aria-hidden="true"></i>Merchant Id <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtMerchantId" runat="server" CssClass="form-control" TabIndex="5" MaxLength="30" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMerchantId"
                                ValidationGroup="PayUPI" SetFocusOnError="True" CssClass="vError">Enter Merchant Id</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12 text-right pt-3">
                        <div class="form-submit" style="text-align: left !important;">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="PayUPI"
                                OnClick="btnSubmit_Click" TabIndex="6" CausesValidation="true" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger"
                                OnClick="btnCancel_Click" TabIndex="7" />
                        </div>
                    </div>
                </div>
            </div>


            <div id="divGrid" runat="server" class="col-sm-12 col-xs-12">
                <div class="table-responsive" style="overflow-x: unset!important;">
                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                        <asp:Label ID="lblPayGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
                    <asp:GridView ID="gvPayUPIDetails" runat="server" CssClass="gvv display table table-bordered table-condenced"
                        AutoGenerateColumns="False" DataKeyNames="UniqueId" OnRowDataBound="gvPayUPIDetails_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mobile No" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MobileNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UPI Id" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblUPIId" runat="server" Text='<%# Bind("UPIId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Merchant Code" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblMerchantCode" runat="server" Text='<%# Bind("MerchantCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Merchant Id" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblMerchantId" runat="server" Text='<%# Bind("MerchantId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
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
                                    <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                        runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to Inactive this record?');" />
                                    <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                        runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure you want to Active this record?');" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gvHead" />
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="form-body col-sm-2 col-xs-12">
            <h5 class="pghr">Account Details </h5>
            <hr />
            <div class="col-sm-12 col-xs-12">
                <div class="row">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-university" aria-hidden="true"></i>Bank Name :</label>
                        <asp:Label ID="lblBankName" runat="server" Font-Bold="true">
                        </asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-xs-12">
                <div class="row">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-money-check" aria-hidden="true"></i>Account No :</label>
                        <asp:Label ID="lblAccNo" runat="server" Font-Bold="true">
                        </asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-xs-12">
                <div class="row">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-money-check" aria-hidden="true"></i>Bank IFSC Code :</label>
                        <asp:Label ID="lblBankIFSCode" runat="server" Font-Bold="true">
                        </asp:Label>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hfUniqueId" runat="server" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

