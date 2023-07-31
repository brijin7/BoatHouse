<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptChalanRegister.aspx.cs" Inherits="Reports_RptChalanRegister" %>

<asp:Content ID="ChallanRegister" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".ChallanDate").datepicker({
                dateFormat: 'dd-mm-yy',
                maxDate: 0,
                changeMonth: true,
                changeYear: true,

            });
        });
    </script>
    <div class="form-body">
        <h5 class="pghr">Challan Register</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row" style="padding-left: 15px;">
                <div class="col-md-12 col-sm-12" style="margin-top: 2%">
                    <div class="row">
                        <div class="col-xl-12 col-md-12 col-lg-12  col-sm-12">
                            <div class="form-group">
                                <asp:RadioButtonList runat="server" ID="rbtnChalan" TabIndex="1" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnChalan_SelectedIndexChanged" CssClass="rbl">
                                    <%--<asp:ListItem Value="0" Selected="True">Abstract Challan</asp:ListItem>
                                    <asp:ListItem Value="1">Boating</asp:ListItem>
                                    <asp:ListItem Value="2">Other service</asp:ListItem>
                                    <asp:ListItem Value="3">Restaurant</asp:ListItem>
                                    <asp:ListItem Value="5">Additional Ticket</asp:ListItem>--%>
                                    <asp:ListItem Value="6">Detailed Challan</asp:ListItem>
                                    <asp:ListItem Value="4">Abstract Challan</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>Challan Date</label>
                            <asp:TextBox ID="txtChallanDate"
                                OnTextChanged="txtChallanDate_TextChanged"
                                AutoPostBack="true"
                                runat="server"
                                CssClass="form-control ChallanDate"
                                AutoComplete="Off"
                                TabIndex="2">
                            </asp:TextBox>
                        </div>
                        <div class="col-sm-3 col-xs-12" runat="server" id="divServiceType" visible="true">
                            <label id="lblServiceType" runat="server">
                                <i class="fa fa-ship" aria-hidden="true"></i>Service Type
                            </label>
                            <asp:DropDownList
                                ID="ddlServiceType"
                                runat="server"
                                CssClass="form-control inputboxstyle"
                                OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged"
                                AutoPostBack="true">
                                <asp:ListItem Value="B" Text="Boating"></asp:ListItem>
                                <asp:ListItem Value="OS" Text="Other service"></asp:ListItem>
                                <asp:ListItem Value="R" Text="Restaurant"></asp:ListItem>
                                <asp:ListItem Value="AT" Text="Additional Ticket"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-3 col-xs-12" runat="server" id="divUserName" visible="false">
                            <label for="lblPaymentType" id="lblPaymentType"><i class="fa fa-user" aria-hidden="true"></i>User Name <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlUserName" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="3" OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlUserName" InitialValue="0"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                                Select User Name</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-xs-12" runat="server" id="divPaymentType" visible="false">
                            <label for="lblPaymentType" id="lblPaymentType"><i class="fas fa-money-check" aria-hidden="true"></i>Payment Type <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlPaymentType" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="3">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                            <asp:Button ID="btnGenerate" runat="server" Text="Generate" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnGenerate_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

