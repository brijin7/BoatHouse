<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="EmailIdPwd.aspx.cs" Inherits="Masters_EmailIdPwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-10 col-xs-12">
        <h5 class="pghr">EmailId Password Details</h5>
        <hr />
        <div class="mydivbrdr" runat="server">
            <div class="row m-0">
                <div class="col-sm-4 col-xs-12">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-envelope" aria-hidden="true"></i>Email Id <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" TabIndex="1" MaxLength="50" AutoComplete="Off">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmailId"
                            ValidationGroup="EmlPwd" SetFocusOnError="True" CssClass="vError">Enter Email Id</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtemailid"
                            SetFocusOnError="True" CssClass="vError"
                            ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            Display="Dynamic" ErrorMessage="Invalid email address" ValidationGroup="EmlPwd" />
                    </div>
                </div>
                <div class="col-sm-4 col-xs-12">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-key" aria-hidden="true"></i>Password <span class="spStar">*</span></label>
                        <div class="input-group">
                            <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control passBox1" TabIndex="2" MaxLength="50" TextMode="Password"
                                AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                            </asp:TextBox>
                            <div class="input-group-prepend">
                                <div class="input-group-text p-0">
                                    <button id="show_password" class="btn btn-light pt-1" type="button" style="height: 28px; width: 40px;">
                                        <span class="fa fa-eye-slash icon p-0"></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPwd"
                            ValidationGroup="EmlPwd" SetFocusOnError="True" CssClass="vError">Enter Password</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-4 col-xs-12" runat="server">
                    <label for="lblServices" id="lblEmailServices"><i class="fas fa-list" aria-hidden="true"></i>Service Type<span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlEmailService" CssClass="form-control inputboxstyle" runat="server" TabIndex="3">
                        <asp:ListItem Value="0">Select Services</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPwd"
                        ValidationGroup="EmlPwd" SetFocusOnError="True" CssClass="vError">Select ServiceType</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-12 col-xs-12 ">
                    <div class="form-submit text-right pt-4" style="text-align: right !important">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="EmlPwd"
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
                <asp:GridView ID="gvEmlPwdDetails" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="UniqueId" OnRowDataBound="gvEmlPwdDetails_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEmailId" runat="server" Text='<%# Bind("EmailId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Password" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPassword" runat="server" Text='<%# Bind("Password") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Type" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceTypeId" runat="server" Text='<%# Bind("ServiceTypeId") %>'></asp:Label>
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
                                    runat="server" Font-Bold="true" OnClientClick="return confirm('Are you sure to Inactive this record?');" OnClick="ImgBtnDelete_Click" />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClientClick="return confirm('Are you sure to Active this record?');" OnClick="ImgBtnUndo_Click" />
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

    <script type="text/javascript">
        $(document).ready(function () {
            let showPass = document.querySelector("#show_password");

            showPass.addEventListener("click", function () {

                let passBox = document.querySelector(".passBox1");

                if (passBox.type === "password") {
                    passBox.type = "text";
                    $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                }
                else {
                    passBox.type = "password";
                    $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
            })
        });
    </script>
    <%-- Newly implemented for CSRF Validation--%>
    
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

