<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Reports/RptTripSheetSummary.aspx.cs" Inherits="Boating_RptTripSheetSummary" %>

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
    <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.min.css" rel="stylesheet" />
    <%--<script src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>--%>
    <script type="text/javascript">

        $(function () {
            $('[id$=gvTripSheetSummary]').append('<tfoot><tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr></tfoot>'),
                $('[id$=gvTripSheetSummary]').prepend($("<thead></thead>").append($('[id$=gvTripSheetSummary]').find("tr:first"))).DataTable({
                    "responsive": true,
                    "sPaginationType": "full_numbers",
                    "footerCallback": function (row, data, start, end, display) {
                        var api = this.api(), data;
                        var intVal = function (i) {
                            return typeof i === 'string' ?
                                i.replace(/[\$,]/g, '') * 1 :
                                typeof i === 'number' ?
                                    i : 0;
                        };

                        $(api.column(8).footer()).html("Total");
                        $(api.column(9).footer()).html(document.getElementById('hfBoatCharge').value).val();
                        $(api.column(10).footer()).html(document.getElementById('hfBoatDeposite').value).val();
                        $(api.column(11).footer()).html(document.getElementById('hfBillAmount').value).val();
                        $(api.column(16).footer()).html(document.getElementById('hfRefundAmt').value).val();
                    }
                });

        });
    </script>
    <style type="text/css">
        input[type="search"] {
            width: 70%;
            height: 34px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857;
            color: #555;
            background-color: #FFF;
            background-image: none;
            border: 1px solid #CCC;
            border-radius: 4px;
            box-shadow: 0px 1px 1px rgba(0, 0, 0, 0.075) inset;
            transition: border-color 0.15s ease-in-out 0s, box-shadow 0.15s ease-in-out 0s;
        }

        table.dataTable tfoot th, table.dataTable tfoot td {
            color: green;
            font: bold;
            font-size: 20px;
            text-align: right;
            padding-right: 5px;
        }

        .col, .col-1, .col-10, .col-11, .col-12, .col-2, .col-3, .col-4, .col-5, .col-6, .col-7, .col-8, .col-9, .col-auto, .col-lg, .col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-auto, .col-md, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-auto, .col-sm, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-auto, .col-xl, .col-xl-1, .col-xl-10, .col-xl-11, .col-xl-12, .col-xl-2, .col-xl-3, .col-xl-4, .col-xl-5, .col-xl-6, .col-xl-7, .col-xl-8, .col-xl-9, .col-xl-auto {
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 1px;
        }
    </style>

    <div class="form-body">
        <h5 class="pghr">Trip Sheet Summary</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row p-2">
                <%--    <div class="col-sm-2 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblboatnum">
                            <i class="fas fa-money-check" aria-hidden="true"></i>
                            Deposit Type
                        </label>

                        <asp:DropDownList ID="ddlDepositType" CssClass="form-control inputboxstyle" runat="server" TabIndex="1">

                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="Y">Refund</asp:ListItem>
                            <asp:ListItem Value="N">Non Refund</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>--%>
                <%-- <div class="col-sm-2 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblboatnum">
                            <i class="fa fa-ship" aria-hidden="true"></i>
                            Boat Type
                        </label>

                        <asp:DropDownList ID="ddlBoatType" CssClass="form-control inputboxstyle" runat="server" TabIndex="2" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>--%>
                <%--   <div class="col-sm-2 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblboatnum">
                            <i class="fas fa-chair" aria-hidden="true"></i>
                            Boat Seater
                        </label>

                        <asp:DropDownList ID="ddlBoatSeater" CssClass="form-control inputboxstyle" runat="server" TabIndex="3" OnSelectedIndexChanged="ddlBoatSeater_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>--%>
                <%--         <div class="col-sm-2 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblboatnum">
                            <i class="fa fa-ship" aria-hidden="true"></i>
                            Boat
                        </label>

                        <asp:DropDownList ID="ddlBoat" CssClass="form-control inputboxstyle" runat="server" TabIndex="4">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>--%>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="5">
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
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="6">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12" style="padding-top: 1.5rem; padding-left: 4rem;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="7" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="8" OnClick="btnReset_Click" />
                </div>

                <div class="col-md-12 col-lg-12 col-sm-12">

                    <div style="float: right">
                        <asp:Button ID="btnPDF" Visible="false" runat="server" class="btn btn-primary" Text="Generate PDF" OnClick="btnPDF_Click" />

                    </div>
                </div>

            </div>

        </div>

        <div runat="server" id="divNoOfTrips">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsgNoOfTrips" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>


                <div class="col-sm-12 col-xs-12" style="max-height: 400px; min-height: 50px; overflow-y: auto; float: right; position: relative;">

                    <asp:GridView ID="gvNoOfTrips" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" DataKeyNames="BoatSeaterId,BoatTypeId" PageSize="10" OnPageIndexChanging="gvNoOfTrips_PageIndexChanging" ShowFooter="true">
                        <columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatSeaterName" runat="server" Text='<%# Bind("BoatSeater") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Number Of Trips" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:LinkButton ID="lblNoOfTrips" runat="server" Text='<%# Bind("NoOfTrips") %>' Font-Bold="true" ForeColor="Brown" OnClick="lblNoOfTrips_Click" Font-Underline="true"></asp:LinkButton>
                                    <%--  <asp:Label ID="lblNoOfTrips" runat="server" Text='<%# Bind("NoOfTrips") %>' Font-Bold="true" ForeColor="Brown"></asp:Label>--%>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Collection Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblCollectionAmt" runat="server" Text='<%# Bind("CollectionAmt") %>' Font-Bold="true" ForeColor="blue"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Refund" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblRefund" runat="server" Text='<%# Bind("ClaimedDeposit") %>' Font-Bold="true" ForeColor="Brown"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="UnClaimed Deposit Amount" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="150px">
                                <itemtemplate>
                                    <asp:Label ID="lblUnclaimDep" runat="server" Text='<%# Bind("UnClaimedDeposit") %>' Font-Bold="true" ForeColor="Brown"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Extended Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblExtenCharge" runat="server" Text='<%# Bind("ExtendedAmt") %>' Font-Bold="true" ForeColor="Brown"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rower Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblRowerAm" runat="server" Text='<%# Bind("Roweramt") %>' Font-Bold="true" ForeColor="Brown"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rower Settlement" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblRowesel" runat="server" Text='<%# Bind("RowerSettlementAmt") %>' Font-Bold="true" ForeColor="Brown"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                        </columns>

                        <headerstyle cssclass="gvHead" />
                        <alternatingrowstyle cssclass="gvRow" />
                        <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                    </asp:GridView>

                </div>
            </div>

        </div>
        <div runat="server" id="divGridList" visible="false">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-12 col-xs-12">
                    <asp:GridView ID="gvTripSheetSummary" runat="server"
                        AutoGenerateColumns="False" DataKeyNames="BookingId">
                        <columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>' Font-Bold="true"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>' Font-Bold="true"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatTypeName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatSeaterName" runat="server" Text='<%# Bind("BoatSeaterName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Name" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatName" runat="server" Text='<%# Bind("BoatName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Duration" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Bind("BookingDuration") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblPaymentTypeName" runat="server" Text='<%# Bind("PaymentTypeName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Charge" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatCharge" runat="server" Text='<%# Bind("BoatCharge") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Deposite" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatDeposit" runat="server" Text='<%# Bind("BoatDeposit") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Bill Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblActualBillAmount" runat="server" Text='<%# Bind("InitNetAmount") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="left" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="Customer Mobile" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Trip End Time" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Bind("TripEndTime") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Travelled Minutes" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblTravelDuration" runat="server" Text='<%# Bind("TravelDuration") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Refund Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblDepRefundAmount" runat="server" Text='<%# Bind("DepRefundAmount") %>' Font-Bold="true"></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                                <footertemplate>
                                    <asp:Label ID="lblTotalRefundAmount" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                                </footertemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Refund Status" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblDepRefundStatus" runat="server" Text='<%# Bind("DepRefundStatus") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                        </columns>

                        <headerstyle cssclass="gvHead" />
                        <alternatingrowstyle cssclass="gvRow" />
                        <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                    </asp:GridView>
                    <asp:HiddenField ID="hfBillAmount" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfRefundAmt" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfBoatCharge" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfBoatDeposite" runat="server" ClientIDMode="Static" />
                </div>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

