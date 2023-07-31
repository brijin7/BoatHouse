<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptCreditTripWiseDetails.aspx.cs" Inherits="Reports_RptCreditTripWiseDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
<%--    <link href="~/Scripts/sweetalert.css" rel="stylesheet" />--%>
    <%--   <script src="~/Scripts/jquery-1.10.2.js"></script>--%>
<%--    <script src="~/Scripts/sweetalert.js"></script>--%>
    <style>
        .inline-rb input[type="radio"] {
            width: auto;
            margin-right: 5px;
        }

        .inline-rb label {
            display: inline;
        }

        .mrtop {
            margin-top: 30px;
        }
    </style>

    <div class="form-body" runat="server" id="divShow" visible="false">
        <h5 class="pghr">Credit Booking - Trip Wise Details</h5>
        <hr />
        <br />
        <div class="col-md-12 col-lg-12 col-sm-12">
            <div class="row" style="background-color: khaki;">
                <div class="col-md-8">
                    <asp:RadioButtonList ID="rbtnDateOrMonthWise" TabIndex="1" CssClass="inline-rb" Font-Bold="true" ForeColor="#ffffcc"
                        OnSelectedIndexChanged="rbtnDateOrMonthWise_SelectedIndexChanged"
                        RepeatDirection="Horizontal"
                        CellSpacing="6" CellPadding="6" Width="50%"
                        RepeatColumns="3" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Text="Date Wise"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Month Wise"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <%-----------------------------------------------------DATE WISE-----------------------------------------------------------%>
            <div class="col-md-12 col-sm-12" runat="server" id="divDateWiseRpt" style="margin-top: 25px;">
                <div class="row">
                    <div class="col-sm-2 col-xs-12" runat="server">
                        <label for="lblMonthFrom" id="lblFromdate1"><i class="fa fa-calendar" aria-hidden="true"></i>From Date <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" AutoComplete="off" CssClass="form-control frmDate" runat="server"
                            TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="DateWise" SetFocusOnError="True" CssClass="vError">
                            Select From Date</asp:RequiredFieldValidator>
                    </div>

                    <div class="col-sm-2 col-xs-12" runat="server">
                        <label for="lblMonthTo" id="lblTodate1"><i class="fa fa-calendar" aria-hidden="true"></i>To Date <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" CssClass="form-control toDate" runat="server" AutoComplete="off"
                            TabIndex="3">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="DateWise" SetFocusOnError="True" CssClass="vError">
                            Select To Date</asp:RequiredFieldValidator>
                    </div>

                    <div class="col-sm-2 col-xs-12">
                        <label id="lblDateWiseBoatType"><i class="fas fa-list" aria-hidden="true"></i>Boat Type<span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlDateWiseBoatType" CssClass="form-control" runat="server" TabIndex="4">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <asp:Button ID="btnDateWise" runat="server" OnClick="btnDateWise_Click" TabIndex="5"
                            CssClass="btn btn-primary mrtop" Text="Generate" ValidationGroup="DateWise" />
                    </div>
                </div>
            </div>
            <%-----------------------------------------------------MONTH WISE-----------------------------------------------------------%>
            <div class="col-md-12 col-sm-12" runat="server" id="divMonthWiseRpt" style="margin-top: 50px;" visible="false">
                <div class="row">
                    <div class="col-sm-2 col-xs-12">
                        <label id="lblFinYear"><i class="fa fa-calendar" aria-hidden="true"></i>Financial Year<span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlFinyear" AutoPostBack="true" OnSelectedIndexChanged="ddlFinyear_SelectedIndexChanged" CssClass="form-control" runat="server" TabIndex="2">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlFinyear"
                            ValidationGroup="MonthWise" SetFocusOnError="True" InitialValue="0"
                            CssClass="vError">
                            Select To Financial Year</asp:RequiredFieldValidator>
                    </div>

                    <div class="col-sm-2 col-xs-12" runat="server" id="divMonth" visible="false">
                        <label for="lblMonthTo" id="lblTodate"><i class="fa fa-calendar" aria-hidden="true"></i>Month<span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlMonth" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" TabIndex="3">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="04">April</asp:ListItem>
                            <asp:ListItem Value="05">May</asp:ListItem>
                            <asp:ListItem Value="06">June</asp:ListItem>
                            <asp:ListItem Value="07">July</asp:ListItem>
                            <asp:ListItem Value="08">August</asp:ListItem>
                            <asp:ListItem Value="09">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                            <asp:ListItem Value="01">January</asp:ListItem>
                            <asp:ListItem Value="02">February</asp:ListItem>
                            <asp:ListItem Value="03">March</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlMonth"
                            ValidationGroup="MonthWise" SetFocusOnError="True" InitialValue="0"
                            CssClass="vError">
                            Select From Date</asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-2 col-xs-12" runat="server" id="divFromDate" visible="false">
                        <label for="lblMonthFrom" id="lblFromdate"><i class="fa fa-calendar" aria-hidden="true"></i>From Date <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtmFromDate" CssClass="form-control frmDate" runat="server"
                            TabIndex="4">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtmFromDate"
                            ValidationGroup="MonthWise" SetFocusOnError="True" CssClass="vError">
                            Select From Date</asp:RequiredFieldValidator>
                    </div>

                    <div class="col-sm-2 col-xs-12" runat="server" id="divToDate" visible="false">
                        <label for="lblMonthTo" id="lblmTodate"><i class="fa fa-calendar" aria-hidden="true"></i>To Date <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtmToDate" CssClass="form-control toDate" runat="server"
                            TabIndex="5">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtmToDate"
                            ValidationGroup="MonthWise" SetFocusOnError="True" CssClass="vError">
                            Select To Date</asp:RequiredFieldValidator>
                    </div>


                    <div class="col-sm-2 col-xs-12" runat="server" id="divMonthWiseBoatType" visible="false">
                        <label id="lblMonthWiseBoatType"><i class="fas fa-list" aria-hidden="true"></i>Boat Type<span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlMonthWiseBoatType" CssClass="form-control" runat="server" TabIndex="6">
                        </asp:DropDownList>
                    </div>

                    <div class="col-sm-2 col-xs-12" runat="server" id="divMonthBtn" visible="false">
                        <asp:Button ID="btnMontWise" runat="server" CssClass="btn btn-primary mrtop"
                            OnClick="btnMontWise_Click" Text="Generate" TabIndex="7" ValidationGroup="MonthWise" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

