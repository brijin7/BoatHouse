<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="PaymentAccountDetails.aspx.cs" Inherits="Masters_PaymentAccountDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-10 col-xs-12">
        <h5 class="pghr">Payment Account Details</h5>
        <hr />
        <div class="mydivbrdr" runat="server">
            <div class="row m-0">
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-user" aria-hidden="true"></i>Account Name <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" TabIndex="1" MaxLength="50" AutoComplete="Off">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAccountName"
                            ValidationGroup="PayAcc" SetFocusOnError="True" CssClass="vError">Enter Account Name</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-mobile" aria-hidden="true"></i>Account No <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50"
                            onkeypress="return isNumber(event)" AutoComplete="Off">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAccountNo"
                            ValidationGroup="PayAcc" SetFocusOnError="True" CssClass="vError">Enter Account No</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-money-check" aria-hidden="true"></i>Bank IFS Code <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtBankIFSC" runat="server" CssClass="form-control" TabIndex="3" MaxLength="15" AutoComplete="Off"
                            OnTextChanged="txtBankIFSC_TextChanged" AutoPostBack="true">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtBankIFSC"
                            ValidationGroup="PayAcc" SetFocusOnError="True" CssClass="vError">Enter Bank IFS Code</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row m-0">
                <div class="col-sm-6 col-xs-12">
                    <div class="form-group row">
                        <div class="col-sm-4 col-xs-12">
                            <label for="boat"><i class="fa fa-university" aria-hidden="true"></i>Bank Name </label>
                        </div>
                        <div class="col-sm-1 col-xs-12">
                            :
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <asp:Label ID="lblBankName" runat="server" Font-Bold="true">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">
                    <div class="form-group row">
                        <div class="col-sm-4 col-xs-12">
                            <label for="boat"><i class="fa fa-university" aria-hidden="true"></i>Bank Branch </label>
                        </div>
                        <div class="col-sm-1 col-xs-12">
                            :
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <asp:Label ID="lblBankBranch" runat="server" Font-Bold="true">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">
                    <div class="form-group row">
                        <div class="col-sm-4 col-xs-12">
                            <label for="boat"><i class="fa fa-money-check" aria-hidden="true"></i>MICR Code </label>
                        </div>
                        <div class="col-sm-1 col-xs-12">
                            :
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <asp:Label ID="lblMICRCode" runat="server" Font-Bold="true">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">
                    <div class="form-group row">
                        <div class="col-sm-4 col-xs-12">
                            <label for="boat"><i class="fa fa-map-marker-alt" aria-hidden="true"></i>City </label>
                        </div>
                        <div class="col-sm-1 col-xs-12">
                            :
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <asp:Label ID="lblCity" runat="server" Font-Bold="true">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">
                    <div class="form-group row">
                        <div class="col-sm-4 col-xs-12">
                            <label for="boat"><i class="fa fa-map-marker-alt" aria-hidden="true"></i>District </label>
                        </div>
                        <div class="col-sm-1 col-xs-12">
                            :
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <asp:Label ID="lblDistrict" runat="server" Font-Bold="true">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-xs-12">
                    <div class="form-group row">
                        <div class="col-sm-4 col-xs-12">
                            <label for="boat"><i class="fa fa-map-marker-alt" aria-hidden="true"></i>State </label>
                        </div>
                        <div class="col-sm-1 col-xs-12">
                            :
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <asp:Label ID="lblState" runat="server" Font-Bold="true">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12 col-xs-12">
                    <div class="form-submit" style="text-align: right !important">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="PayAcc"
                            OnClick="btnSubmit_Click" TabIndex="4" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger"
                            OnClick="btnCancel_Click" TabIndex="5" />
                    </div>
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server" class="col-sm-12 col-xs-12">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblPayGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvPayAccDetails" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="UniqueId" OnRowDataBound="gvPayAccDetails_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("AccountName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAccountNo" runat="server" Text='<%# Bind("AccountNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bank IFSCCode" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBankIFSCCode" runat="server" Text='<%# Bind("BankIFSCCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bank Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBankName" runat="server" Text='<%# Bind("BankName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("BranchName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMICRCode" runat="server" Text='<%# Bind("MICRCode") %>'></asp:Label>
                                <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                                <asp:Label ID="lblDistrict" runat="server" Text='<%# Bind("District") %>'></asp:Label>
                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("State") %>'></asp:Label>
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
    <asp:HiddenField ID="hfUniqueId" runat="server" />

    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

