<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="DepositStatusReport.aspx.cs" Inherits="Reports_DepositStatusReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">Deposit Status</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row" style="padding-left: 15px;">
                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblServices" id="lblServices"><i class="fas fa-list" aria-hidden="true"></i>Status Type<span class="spStar">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlStatusType" CssClass=" form-control" TabIndex="1">
                                <asp:ListItem Value="0">All Deposits</asp:ListItem>
                                <asp:ListItem Value="1">UnClaimed</asp:ListItem>
                                <asp:ListItem Value="2">Time Extended</asp:ListItem>
                                <asp:ListItem Value="3">Refunded</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>Booking Date</label>
                            <asp:TextBox ID="txtBookingDate" runat="server" CssClass=" form-control frmDate"
                                AutoComplete="Off" TabIndex="2">
                            </asp:TextBox>
                        </div>

                        <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                            <span style="float: left">
                                <asp:Button ID="btnGenerate" runat="server" Text="Generate" class="btn btn-primary" ValidationGroup="Search" OnClick="btnGenerate_Click" TabIndex="3" />
                            </span>
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

