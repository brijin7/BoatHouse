<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Boating/BoatCancellation.aspx.cs" Inherits="BoatCancellation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .panel > .panel-heading {
            background-color: #134a79;
            color: white;
            text-align: center;
            font-weight: bold;
        }

        .blink_me {
            animation: blinker 1s linear infinite;
        }

        .BtnDate {
            display: none;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }
    </style>

    <script>
        $(document).ready(function () {
            var btnHidden = $('#<%= btnBind.ClientID %>');
            var btnHidden1 = $('#<%= btnAdd.ClientID %>');

            $(".StartDate").datepicker({
                dateFormat: 'dd/mm/yy',
                maxDate: 700,
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,
                onSelect: function (selected) {
                    $(".EndDate").datepicker("option", "minDate", selected).attr('readonly', 'readonly');
                }
            });
            $(".EndDate").datepicker({
                dateFormat: 'dd/mm/yy',
                maxDate: 700,
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,
                onSelect: function (selected) {
                    //$(".StartDate").datepicker("option", "minDate", selected),
                    btnHidden1.click();
                    (".EndDate<=.StartDate");

                }
            });

        });
    </script>


    <script language="javascript" type="text/javascript">

        function Selectallcheckbox(val) {
            if (!$(this).is(':checked')) {
                $('input:checkbox').prop('checked', val.checked);
            } else {
                $("#chkroot").removeAttr('checked');
            }
        }
        function SingleCheckBox(val) {
            var gvcheck = document.getElementById('<%=gvBookingDetails.ClientID %>');
            console.log($('input:checkbox:checked').length);
            console.log($('input:checkbox').length);
            var gvcheck = document.getElementById('<%=gvBookingDetails.ClientID %>');
            var inputs = gvcheck.rows[0].getElementsByTagName('input');
            console.log(inputs[0].checked);


            if (($('input:checkbox:checked').length == $('input:checkbox').length ||
                $('input:checkbox:checked').length == $('input:checkbox').length - 1)
                && inputs[0].checked == false) {

                inputs[0].checked = true;
            }
            else {

                inputs[0].checked = false;
            }

        }

    </script>
    <div class="form-body">
        <h5 class="pghr">Boat Cancellation <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
            <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-2 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                Cancellation Type
                            </label>
                            <asp:DropDownList ID="ddlCancellationType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCancellationType_SelectedIndexChanged">
                                <asp:ListItem Value="1" Selected="True">Customer Cancellation </asp:ListItem>
                                <asp:ListItem Value="3" Selected="True">Boat House Cancellation</asp:ListItem>
                                <%--<asp:ListItem Value="2">Bulk Cancellation</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="divBookingId" runat="server" class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                Booking Id
                            </label>
                            <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control" OnTextChanged="txtBookingId_TextChanged" AutoPostBack="true" AutoComplete="Off" TabIndex="4" MaxLength="50" onkeypress="return isNumberKey(event)">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBookingId"
                                ValidationGroup="Cancellation" SetFocusOnError="True" CssClass="vError">Enter Booking Id</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" id="divFromDate" visible="false" runat="server">
                        <div class="form-group">
                            <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control StartDate" AutoComplete="Off" TabIndex="1">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFromDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" id="divToDate" visible="false" runat="server">
                        <div class="form-group">
                            <label for="lblToDate" id="Label11" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control EndDate" AutoComplete="Off" TabIndex="2">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtToDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter To Date</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblreason"><i class="fa fa-window-close"></i>&nbsp;&nbsp;Cancellation Reason</label>
                            <asp:DropDownList ID="ddlReason" CssClass="form-control inputboxstyle" runat="server" TabIndex="3">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlReason" ForeColor="Red"
                                ValidationGroup="Cancellation" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Cancellation Reason</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div id="divBookingPaymentType" class="col-sm-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                Booking (Payment Type)
                            </label>
                            <asp:DropDownList ID="ddlPaymentTypeSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentTypeSearch_SelectedIndexChanged">
                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12" runat="server" id="divRepay" visible="false">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                Re-Payment Type
                            </label>
                            <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4 col-xs-12" runat="server" id="theDivstatement" visible="false">
                    <div class="form-group">
                        <label for="lblstatement"><i class="fas fa-info"></i>Cancellation Statement</label>
                        <asp:TextBox ID="txtstatement" runat="server"
                            CssClass="form-control" AutoComplete="Off" MaxLength="200" Font-Size="14px"
                            TabIndex="3" Style="margin-left: 0px;" TextMode="SingleLine">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtstatement"
                            ValidationGroup="Cancellation" SetFocusOnError="True" CssClass="vError">Enter Cancellation Statement</asp:RequiredFieldValidator>
                    </div>
                </div>
                <asp:Button ID="btnBind" runat="server" Text="Button" CssClass="BtnDate" OnClick="btnBind_Click" />
                <asp:Button ID="btnAdd" runat="server" Text="Button" OnClick="btnAdd_Click" CssClass="BtnDate" />




                <div class="table-div" id="divBulkCancellation" runat="server">
                    <div class="table-responsive" style="overflow-x: auto; width: 98%; max-height: 230px;">
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </div>
                        <asp:GridView ID="gvBulkCancellation" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                            AutoGenerateColumns="False" PageSize="25000" DataKeyNames="BookingId">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Reference No" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Duration" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Bind("BookingDuration") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatDeposit" runat="server" Text='<%# Bind("BoatDeposit") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText=" Net Amount" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblinitNetAmount" runat="server" Text='<%# Bind("initNetAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText=" Boat Charge" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="initBoatCharge" runat="server" Text='<%# Bind("initBoatCharge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rower Charge" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblinitRowerCharge" runat="server" Text='<%# Bind("initRowerCharge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Payment Type" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentType" runat="server" Text='<%# Bind("PaymentType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" onclick="javascript:Selectallcheckbox(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gvHead" />
                            <AlternatingRowStyle CssClass="gvRow" />
                            <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                            <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                        </asp:GridView>
                    </div>
                </div>

                <div class="row">
                    <div id="exittimedetails" runat="server" visible="false" class="col-xs-12 col-sm-12 col-lg-12 col-md-12" style="width: 80%;">

                        <div class="row" style="margin-top: 1%;">
                            <div class="col-sm-12">

                                <div class="table-responsive" style="overflow-x: auto; width: 98%; max-height: 250px;">

                                    <asp:GridView ID="gvBookingDetails" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                        AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="10" ShowFooter="true" OnPageIndexChanging="gvBookingDetails_PageIndexChanging">

                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="left" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookinPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Reference No" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Mobile No" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerMobileNo" runat="server" Text='<%# Bind("CustomerMobileNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Boat Charges" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatCharges" runat="server" Text='<%# Bind("BoatCharges") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Boat Deposit" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatDeposit" runat="server" Text='<%# Bind("BoatDeposit") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInitNetAmount" runat="server" Text='<%# Bind("InitNetAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="CheckBox2" runat="server" onclick="javascript:Selectallcheckbox(this);" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox1" runat="server" onclick="javascript:SingleCheckBox(this);" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="gvHead" />
                                        <AlternatingRowStyle CssClass="gvRow" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                        <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divAmountSummary" runat="server">

                            <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">
                                <div class="panel panel-success">
                                    <div class="panel-heading"><i class="fa fa-popover-header" aria-hidden="true" style="color: darkgreen;"></i>Cancellation Charge Details </div>
                                    <asp:GridView ID="gvNoteDetails" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced"
                                        AutoGenerateColumns="False" PageSize="25000">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hours Before" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApplicableBefore" runat="server" Text='<%# Bind("ApplicableBefore") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Charges" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCharges" runat="server" Text='<%# Bind("Charges") %>'></asp:Label>
                                                    <asp:Label ID="lblChargeType" runat="server" Text='<%# Eval("ChargeType").ToString() == "P" ? "(%)" : "(₹)" %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="col-sm-1">
                            </div>

                            <div class="col-sm-4 col-md-4 col-lg-4" id="divCharges" runat="server">

                                <div class="panel panel-success">
                                    <div class="panel-heading"><i class="fa fa-popover-header" aria-hidden="true" style="color: darkgreen;"></i>Cancellation Charges </div>
                                    <div class="panel-body">
                                        <div class="row" style="padding: 8px;">
                                            <div class="col-sm-6">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="15px">  Boat Charge</asp:Label>

                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="15px">   : </asp:Label>
                                            </div>
                                            <div class="col-sm-4" style="text-align: right">
                                                <asp:Label ID="lblBoatCharges" runat="server" Font-Bold="True" Style="text-align: right"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 8px;">
                                            <div class="col-sm-6">
                                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="15px">    Deposit Amount </asp:Label>

                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="15px">   : </asp:Label>
                                            </div>
                                            <div class="col-sm-4" style="text-align: right">
                                                <asp:Label ID="lblDepositamount" runat="server" Font-Bold="True" Style="text-align: right"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 8px;">
                                            <div class="col-sm-6">
                                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="15px"> Total Charges </asp:Label>

                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="15px">   : </asp:Label>
                                            </div>
                                            <div class="col-sm-4" style="text-align: right">
                                                <asp:Label ID="lblTotalCharges" runat="server" Font-Bold="True" Style="text-align: right"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 8px;">
                                            <div class="col-sm-6">
                                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="15px">  Cancel Charges </asp:Label>

                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="15px">   : </asp:Label>
                                            </div>
                                            <div class="col-sm-4" style="text-align: right">
                                                <asp:Label ID="lblCancelCharges" runat="server" Font-Bold="True" Style="text-align: right"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 8px;">
                                            <div class="col-sm-6 ">
                                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="15px">  Refund Amount </asp:Label>

                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="15px">   : </asp:Label>
                                            </div>
                                            <div class="col-sm-4" style="text-align: right; margin-top: -4px">
                                                <asp:Label ID="lblRefundAmount" runat="server" Font-Bold="True" ForeColor="Blue" Font-Size="25px" Style="text-align: right;"></asp:Label>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="row">
                    <%--        <div class="col-sm-4 offset-sm-4 of col-xs-8" runat="server" id="divRepay" visible="false">
                        <div class="form-group">
                                <label for="lblbooking">
                                    <i class="fas fa-address-book"></i>
                                    Re-Payment Type
                                </label>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                        </div>
                    </div>--%>
                    <div class="form-submit col-sm-12" style="padding-top: 30px; margin-right: inherit; margin-left: inherit">
                        <span style="float: right;">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Cancellation" Width="100px" Font-Bold="True"
                                Style="font-size: 20px;"
                                TabIndex="3" OnClick="btnSubmit_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" Width="100px" Font-Bold="True"
                                 Style="font-size: 20px;"
                                 TabIndex="4" OnClick="btnCancel_Click" /></span>
                    </div>
                </div>
            </div>
        </div>


        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <asp:GridView ID="gvCancelBooking" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced" OnPageIndexChanging="gvCancelBooking_PageIndexChanging"
                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Booking Id " HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Pin " HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reference No" Visible="false" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Mobile No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MobileNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Charge" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblInitBillAmount" runat="server" Text='<%# Bind("BoatCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDepositAmount" runat="server" Text='<%# Bind("DepositAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Charges" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCancelCharges" runat="server" Text='<%# Bind("DeductedCharges") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Refund " HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCancelRefund" runat="server" Text='<%# Bind("Refund") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Payment Type " HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType").ToString().Trim() %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Cancellation Date " HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCancellationDate" runat="server" Text='<%# Bind("CancellationDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

