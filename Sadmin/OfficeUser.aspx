<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="OfficeUser.aspx.cs" Inherits="Sadmin_OfficeUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <style type="text/css">
        .rbl input[type="radio"] {
            margin-left: 10px;
            margin-right: 6px;
        }

        .imageOverlay {
            background: rgba(245, 245, 245, 0.4);
            height: 20px;
            position: absolute;
            bottom: 0px;
            width: 100%;
            text-align: center;
            color: black;
            font-size: 12px;
            font-weight: 500;
        }

        .divImg {
            /*width: 150px;*/
            position: relative;
        }

            .divImg:hover {
                box-shadow: 0 0 20px #dddddd;
                cursor: pointer;
            }

        .chkChoice input {
            margin-right: 5px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            let showPass = document.querySelector("#show_password");
            if (showPass) {
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
            }
           
        });
    </script>

    <div class="form-body">
        <h5 class="pghr">Office User <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-7 col-md-7 col-lg-7 col-xs-12">
                        <span class="frmhdr">Personal Details</span>
                        <div class="mydivbrdr">
                            <div class="row p-2">
                                <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12" style="display: none">
                                    <div class="form-group">
                                        <label for="txtConfigurationType">EmpId <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtEmpId" runat="server" CssClass="form-control">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xl-4 col-md-4 col-lg-4 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtConfigurationType">First Name <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtEmployeeFirstName" runat="server" CssClass="form-control" TabIndex="1" MaxLength="50" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmployeeFirstName" ForeColor="Red"
                                            ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Special Characters Not Allowed" CssClass="vError" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmployeeFirstName"
                                            ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter First Name</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-xl-4 col-md-4 col-lg-4 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtEmployeeLastName">Last Name <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtEmployeeLastName" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtEmployeeLastName" ForeColor="Red"
                                            ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Special Characters Not Allowed" CssClass="vError" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeLastName"
                                            ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter Last Name</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-xl-4 col-md-4 col-lg-4 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtMobNo">Mobile No <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtMobNo" runat="server" CssClass="form-control" TabIndex="3" MaxLength="10" AutoComplete="Off" onkeypress="return isNumber(event);">    </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                            ControlToValidate="txtMobNo" ErrorMessage="Invalid"
                                            ValidationExpression="[0-9]{10}" ForeColor="Red"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMobNo"
                                            ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter Mobile No</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-xl-4 col-md-4 col-lg-4 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtemailid">Email Id</label>
                                        <asp:TextBox ID="txtemailid" runat="server" CssClass="form-control" TabIndex="4" MaxLength="40" AutoComplete="Off">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtemailid"
                                            SetFocusOnError="True" CssClass="vError" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                            Display="Dynamic" ErrorMessage="Invalid email address" ValidationGroup="Employee" />
                                    </div>
                                </div>
                                <div class="col-xl-4 col-md-4 col-lg-4 col-sm-12">
                                    <div class="form-group">
                                        <label for="EmpUserName">User Name <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" TabIndex="5" MaxLength="50" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtUserName"
                                            ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter User Name</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-xl-4 col-md-4 col-lg-4 col-sm-12">
                                    <div class="form-group">
                                        <label for="EmpPassword">Password <span class="spStar">*</span></label>
                                        <div class="input-group-append">
                                            <asp:TextBox ID="txtEmpPassword" runat="server" CssClass="form-control passBox1" TabIndex="6" MaxLength="50" TextMode="Password"
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmpPassword"
                                            ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter Password</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <span class="frmhdr">Department Details</span>
                        <div class="mydivbrdr">
                            <div class="row p-2">
                                <div class="col-md-6 col-lg-6 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtAadharid">Role <span class="spStar">*</span></label>
                                        <asp:RadioButtonList ID="rblRole" runat="server" TabIndex="7" RepeatDirection="Horizontal" CssClass="rbl">
                                            <asp:ListItem Value="S" Selected="True">Sadmin</asp:ListItem>
                                            <asp:ListItem Value="O">Office</asp:ListItem>
                                            <asp:ListItem Value="F">Field</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rblRole"
                                            ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Select Role</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12">
                                    <div class="col-sm-12" style="position: relative; bottom: 0px; right: 0px;">
                                        <div class="form-submit">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Employee" TabIndex="8" OnClick="btnSubmit_Click" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="9" OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-5 col-xs-12">
                        <span class="frmhdr">Module Access Details</span>
                        <div class="mydivbrdr">
                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk">
                                    Module Access <span class="spStar">*</span>
                                </div>

                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-4 col-xs-12" runat="server">
                                            <asp:CheckBox ID="chkModuleMaster" runat="server" CssClass="chkChoice" Text="Master" />
                                        </div>

                                        <div class="col-sm-4 col-xs-12">
                                            <asp:CheckBox ID="chkModuleBoating" runat="server" CssClass="chkChoice" Text="Boating" />
                                        </div>

                                        <div class="col-sm-4 col-xs-12">
                                            <asp:CheckBox ID="chkModuleHotel" runat="server" CssClass="chkChoice" Text="Hotel" />
                                        </div>

                                        <div class="col-sm-4 col-xs-12">
                                            <asp:CheckBox ID="chkModuleTour" runat="server" CssClass="chkChoice" Text="Tour" />
                                        </div>

                                        <div class="col-sm-4 col-xs-12" runat="server">
                                            <asp:CheckBox ID="chkModuleFixedAssets" runat="server" CssClass="chkChoice" Text="Fixed Assets" />
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div runat="server" id="divGrid1">
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <asp:Label ID="lblGridMsgg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvmstrEmployee" runat="server" AutoGenerateColumns="False"
                    CssClass="gvv display table table-bordered table-condenced"
                    DataKeyNames="UserId,EmpID,EmpFirstName,EmpLastName,EmpName,MMaster,MBoating,MHotel,MTour,MFixedAssets,
                            EmpMobileNo,EmpMailId,RoleId,UserName,Password,UserType,ActiveStatus,Createdby">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("EmpName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mobile No.">
                            <ItemTemplate>
                                <asp:Label ID="lblempMobileNo" runat="server" Text='<%# Bind("EmpMobileNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click"
                                    EnableViewState="false" Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click"
                                    OnClientClick="return confirm('Are you sure to Inactive this record?');"
                                    Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click"
                                    OnClientClick="return confirm('Are you sure to Active this record?');" Visible='<%# Eval("ActiveStatus").ToString() == "D"? true: false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hfResponse" />
    <asp:HiddenField runat="server" ID="hfUserId" />
    <asp:HiddenField runat="server" ID="hfImageCheckValue" Value="0" />
    <asp:HiddenField runat="server" ID="hfPassword" />

    <%-- Newly implemented for CSRF Validation--%>
    
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

