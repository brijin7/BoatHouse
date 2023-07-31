<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptReceiptBalanceDetails.aspx.cs" Inherits="Reports_RptReceiptBalanceDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .inline-rb input[type="radio"] {
            width: auto;
            margin-right: 5px;
        }

        .inline-rb label {
            display: inline;
        }
    </style>
    <div class="form-body">
        <h5 class="pghr">Receipt Balance Details Report</h5>
        <hr />
        <div class="col-md-12 col-lg-12 col-sm-12">
            <div class="row" style="background-color: khaki;">
                <div class="col-md-8">
                    <asp:RadioButtonList ID="RadbtnReceiptBalanceDetails" CssClass="inline-rb" Font-Bold="true" ForeColor="#ffffcc" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="RadbtnReceiptBalanceDetails_SelectedIndexChanged" CellSpacing="6" CellPadding="6" Width="100%"
                        RepeatColumns="3" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Text="Receipt Refund Pending List"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Receipt Refund Settled List"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div class="mydivbrdr" runat="server" id="divdate">
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label2" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="Search" TabIndex="3" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" OnClick="btnReset_Click" TabIndex="4" />
                </div>

            </div>
        </div>

    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

