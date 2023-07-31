<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptReprint.aspx.cs" Inherits="Reports_RptReprint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">Reprint Report</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label2" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control frmDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label1" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>Service Type<span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlServiceType" runat="server" TabIndex="3" CssClass="form-control">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">Boat Booking</asp:ListItem>
                            <asp:ListItem Value="2">Other Services</asp:ListItem>
                            <asp:ListItem Value="3">Additional Ticket</asp:ListItem>
                            <asp:ListItem Value="4">Restaurant Services</asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Generate" class="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="Search" TabIndex="4" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" OnClick="btnReset_Click" TabIndex="5" />
                </div>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

