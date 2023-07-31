<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptUBBookingIdReport.aspx.cs" Inherits="Reports_UBServiceWiseCollection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">User Based Booking Details</h5>
        <hr />
        <br />


        <div runat="server" visible="true">
            <div class="row" style="padding-left: 15px; padding-top: 1rem; margin-right: -10px; margin-left: -10px;">
                <div class="col-md-12 col-sm-12">
                    <div class="row">

                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblUserName" id="lblUserName"><i class="fas fa-list" aria-hidden="true"></i>UserName<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlUserName" CssClass="form-control inputboxstyle" runat="server" TabIndex="1">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblTypes" id="lbldate"><i class="fa fa-calendar" aria-hidden="true"></i>Booking Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtBookingDate" CssClass="form-control frmDate" runat="server"
                                TabIndex="2">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBookingDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                                Select Booking Date</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-3 col-lg-3 col-sm-3" style="margin-top: 30px;">
                            <span style="float: right">
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

