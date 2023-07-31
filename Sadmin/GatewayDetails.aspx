<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="GatewayDetails.aspx.cs" Inherits="Sadmin_GatewayDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script lang="javascript" type="text/javascript">

        function numbersonly(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
                if (unicode < 48 || unicode > 57) //if not a number
                    return false //disable key press
            }
        }
    </script>

    <div class="form-body col-sm-10 col-xs-12" runat="server" visible="false" id="divShowDetails">
        <h5 class="pghr">Gateway Details </h5>
        <hr />

        <div class="mydivbrdr">
            <div class="row p-2">
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label runat="server" id="Label1"><i class="fa fa-address-book" aria-hidden="true"></i>Gateway Name <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtGatewayName" runat="server" CssClass="form-control inputboxstyle" MaxLength="20" TabIndex="1" AutoComplete="off" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGatewayName"
                            ValidationGroup="G" SetFocusOnError="True" CssClass="vError">Enter Gateway Name</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label runat="server" id="Label3"><i class="fa fa-address-book" aria-hidden="true"></i>Merchant Id <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtMerchantId" runat="server" CssClass="form-control inputboxstyle" MaxLength="20" onkeypress="return numbersonly(event)" TabIndex="2" AutoComplete="off" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMerchantId"
                            ValidationGroup="G" SetFocusOnError="True" CssClass="vError">Enter Merchant Id</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label runat="server" id="Label4"><i class="fa fa-address-book" aria-hidden="true"></i>Access Code <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtAccessCode" runat="server" CssClass="form-control inputboxstyle" MaxLength="20" TabIndex="3" AutoComplete="off" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAccessCode"
                            ValidationGroup="G" SetFocusOnError="True" CssClass="vError">Enter Access Code</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label runat="server" id="Label2"><i class="fa fa-address-book" aria-hidden="true"></i>Working Key <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtWorkingKey" runat="server" CssClass="form-control inputboxstyle" MaxLength="20" TabIndex="4" AutoComplete="off" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtWorkingKey"
                            ValidationGroup="G" SetFocusOnError="True" CssClass="vError">Enter Working Key</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-12 col-xs-12">
                    <div class="form-submit" style="text-align: right !important">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="G" TabIndex="5" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger" TabIndex="6" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server" class="col-sm-12 col-xs-12">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvGateway" runat="server" AllowPaging="True"
                    CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="UniqueId,GatewayName,MerchantId,AccessCode,WorkingKey,ActiveStatus"
                    PageSize="25000" OnRowDataBound="gvGateway_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gateway Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblGatewayName" runat="server" Text='<%# Bind("GatewayName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Merchant Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblMerchantId" runat="server" Text='<%# Bind("MerchantId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Access Code" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAccessCode" runat="server" Text='<%# Bind("AccessCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Working Key" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkingKey" runat="server" Text='<%# Bind("WorkingKey") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
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
                </asp:GridView>
            </div>
        </div>

    </div>
    <%-- Newly implemented for CSRF Validation--%>
    
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

