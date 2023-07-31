<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Reports/RptBoatCancellation.aspx.cs" Inherits="Reports_RptBoatCancellation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />

    <style type="text/css">
        body {
        }

        .boat-unit {
            height: 150px;
            box-shadow: 0 0 10px 1px #929292;
        }

        .boat-image {
            padding: 10px;
        }

            .boat-image img {
                width: 100%;
                height: 130px;
                box-shadow: -3px 3px 10px #a7a7a7;
            }

        .p10 {
            padding: 10px;
        }

        .boat-type #lblBoatType {
            font-size: 20px;
            font-weight: 500;
            color: #0b5269;
            letter-spacing: .5px;
        }

        .list-heading {
            font-size: 12px;
            display: inline-block;
            color: #3282b8;
            letter-spacing: 0.5px;
        }

        .list-vals {
            font-weight: bold;
            font-size: 14px;
            color: #0f4c75;
            letter-spacing: 1px;
            padding-left: 5px;
        }

        .boat-count-input {
            width: 50%;
            margin: auto;
        }

        .boat-check {
            text-align: center;
            padding: 10px;
            margin-top: 26%;
        }

        .boat-list-chk input {
            height: 25px;
            width: 25px;
        }

        .boat-price-div {
            background: linear-gradient(42deg, white 82%, #151965 0%);
        }

        .price-badge {
            text-align: left;
            color: white;
            z-index: 1;
            right: 0px;
            top: 15px;
            position: absolute;
        }

            .price-badge h6 {
                font-size: 2rem;
            }

        .boat-check-div {
            background: linear-gradient(105deg, #151965 16%, white 8%);
        }

        /*tr:nth-child(even) {
            background-color: #f2f2f2;
        }*/

        .otherserv-list-input {
            display: block;
            padding-left: 10px;
        }
    </style>
    <div class="form-body">
        <h5 class="pghr">Boat Cancellation </h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row p-2">

                <div class="col-sm-2 col-xs-12">
                    <label for="lblBoatType" id="lblboattype"><i class="fa fa-ship" aria-hidden="true"></i>Boat Type <span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlBoatType" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged"
                        TabIndex="1">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>

                </div>

                <div class="col-sm-2 col-xs-12">
                    <label for="lblBoatSeater" id="lblboatSeater"><i class="fa fa-ship" aria-hidden="true"></i>Boat Seater <span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlBoatSeater" CssClass="form-control inputboxstyle" runat="server"
                        TabIndex="2">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>

                </div>

                <div class="col-sm-2 col-xs-12">
                    <label for="lblPaymentType" id="lblPaymentType"><i class="fa fa-ship" aria-hidden="true"></i>Payment Type <span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlPaymentType" CssClass="form-control inputboxstyle" runat="server"
                        TabIndex="3">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>

                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="4">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="BoatCancellation" SetFocusOnError="True" CssClass="vError">
                            Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label2" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="5">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="BoatCancellation" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
                        <%--<asp:CompareValidator ID="cvtxtStartDate" runat="server" ValidationGroup="BoatCancellation"
                            ControlToCompare="txtFromDate" CultureInvariantValues="true"
                            Display="Dynamic" EnableClientScript="true"
                            ControlToValidate="txtToDate" ForeColor="Red"
                            ErrorMessage="From Date must be less than To Date"
                            Type="Date" SetFocusOnError="true" Operator="GreaterThanEqual"
                            Text="From Date must be less than To Date"></asp:CompareValidator>--%>
                    </div>
                </div>

                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="BoatCancellation" TabIndex="6" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="7" OnClick="btnReset_Click" />
                </div>
            </div>
        </div>

        <div runat="server" id="divGridList">
            <div class="table-responsive">
                <div style="text-align: right;">
                    <div id="divSearch" runat="server">
                        Search :
                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter Booking Id" AutoComplete="off" Font-Size="16px" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </div>
                    <asp:GridView ID="gvBoatCancellation" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="10" OnPageIndexChanging="gvBoatCancellation_PageIndexChanging">
                        <columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Reference Number" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Mobile No" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MobileNo") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("BoatSeater") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat House" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblPaymentType" runat="server" Text='<%# Bind("PaymentType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cancel Charges" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblCancelCharges" runat="server" Text='<%# Bind("CancelCharges") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cancel Refund" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblCancelRefund" runat="server" Text='<%# Bind("CancelRefund") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                        </columns>

                        <headerstyle cssclass="gvHead" />
                        <alternatingrowstyle cssclass="gvRow" />
                        <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                    </asp:GridView>
                    <div runat="server" id="divprevnext" style="text-align: left;">
                        <%--Newly added--%>
                        <asp:Button ID="back" runat="server" CssClass="btn btn-color mg" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                        &nbsp
                      <asp:Button ID="Next" Visible="true" CssClass="btn btn-color mg" runat="server" Text="Next →" OnClick="Next_Click" />
                        &nbsp
                      <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />
                        <%--Newly added--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

