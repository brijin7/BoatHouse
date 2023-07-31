<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Boating/ReSchedule.aspx.cs" Inherits="Boating_Reschedule" Async="true" %>

<asp:Content ID="Reschedule" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .dtlBoatType {
            margin-bottom: 5px;
            margin-left: 20px !important;
            border-radius: 17px;
            box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
            overflow: hidden;
            transition: all .5s ease;
        }

        .FinalSummary {
            border: 2px solid lightgray;
            border-radius: 15px;
            position: relative;
        }

        .btnFinal {
            display: inline-block;
            font-weight: 400;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            border: 1px solid transparent;
            padding: 0.4rem .5rem;
            font-size: 2rem !important;
            line-height: 1.5;
            border-radius: 1.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            cursor: default;
        }

        .payMode {
            display: inline-block;
            font-weight: 400;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            font-size: 2rem !important;
            line-height: 1.5;
            border-radius: 2.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            outline: none;
        }

        .txtboxlen {
            width: 110px;
        }
    </style>
    <script>
        function isNumber(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (
                (charCode != 45 || $(element).val().indexOf('-') != -1) &&
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&
                (charCode < 48 || charCode > 57))
                return false;
            return true;
        }


    </script>
    <script language="javascript" type="text/javascript">

        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }

    </script>
    <script type="text/javascript">
        function Scroll(evt) {
            let divSingGv = document.querySelector('#<%=divSingGv.ClientID%>');
            document.querySelector('#<%=hfScrollX.ClientID%>').value = divSingGv.scrollLeft;
            document.querySelector('#<%=hfScrollY.ClientID%>').value = divSingGv.scrollTop;
        }

        function ScrollChanege() {
            let divSingGv = document.querySelector('#<%=divSingGv.ClientID%>');
            divSingGv.scrollLeft = document.querySelector('#<%=hfScrollX.ClientID%>').value;
            divSingGv.scrollTop = document.querySelector('#<%=hfScrollY.ClientID%>').value;
        }
    </script>

    <script>
        $(document).ready(function () {
            var btnHidden = $('#<%= btnBind.ClientID %>');
            $(".ResDate").datepicker({
                dateFormat: 'dd-mm-yy',
                minDate: 0,
                maxDate: 365,
                changeMonth: true,
                changeYear: true
            });

            var btnHidden1 = $('#<%= btnAdd.ClientID %>');

            $(".FromDate").datepicker({
                dateFormat: 'dd/mm/yy',
                minDate: 0,
                maxDate: 365,
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,
                onSelect: function (selected) {
                    $(".ToDate").datepicker("option", "minDate", selected)
                    btnHidden1.click();
                }
            });

            $(".ToDate").datepicker({
                dateFormat: 'dd/mm/yy',
                minDate: 0,
                maxDate: 365,
                changeMonth: true,
                changeYear: true,
                onSelect: function (selected) {
                    $(".FromDate").datepicker("option", "maxDate", selected),
                        btnHidden1.click();
                }
            });
        });
    </script>

    <script type="text/javascript">
        function Search_Gridview(strKey) {
            var strData = strKey.value.toLowerCase().split(" ");
            var Grid = "<%=GvBoatRescheduling.ClientID%>";
            var tblData = document.getElementById(Grid);
            var rowData;
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
    </script>

    <script type="text/javascript">
        function Tooltip(evt) {
            let gvSingleResc = evt.parentNode;
            if (gvSingleResc.querySelector('span').hasAttribute("title")) {
                let gvBookingDate = gvSingleResc.querySelector('span').attributes.getNamedItem("title").textContent;
                gvSingleResc.querySelector('span').removeAttribute("title");
                gvSingleResc.querySelector('span').setAttribute('BookingDate', gvBookingDate)
            }
        }
    </script>
    <style>
        span {
            position: relative;
        }

            span[BookingDate]:hover::after {
                content: 'Booking Date : ' attr(BookingDate);
                font-weight: bold;
                font-size: 18px;
                padding: 4px 8px;
                color: #124a79;
                background-color: #fff;
                position: absolute;
                left: 0;
                top: 100%;
                white-space: nowrap;
                box-shadow: 0px 0px 4px #222;
                border-radius: 5px;
                -moz-border-radius: 5px;
                -webkit-border-radius: 5px;
                -moz-box-shadow: 0px 0px 4px #222;
                -webkit-box-shadow: 0px 0px 4px #222;
            }
    </style>

    <div class="form-body">
        <div id="divEntry" runat="server" style="overflow-x: hidden;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <h5 class="pghr" style="text-align: center; font-size: 25px; display: inline;">Re-Schedule </h5>
                </div>
                <div class="col-md-6 col-sm-6">
                    <asp:ImageButton ID="imgbuttonPrint" runat="server" Style="float: right" ImageUrl="~/images/Print.svg"
                        Width="35px" OnClick="imgbuttonPrint_Click" />
                </div>
            </div>
            <hr />
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-2 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                Reschedule Type
                            </label>
                            <asp:DropDownList ID="ddlReschedule" CssClass="form-control inputboxstyle" runat="server"
                                AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlReschedule_SelectedIndexChanged">
                                <asp:ListItem Value="1">Customer Reschedule</asp:ListItem>
                                <%--<asp:ListItem Value="2">Bulk Reschedule</asp:ListItem>--%>
                                <asp:ListItem Value="3">Boat House Reschedule</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" id="divSingleReschedule" runat="server" visible="false">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                Booking Id
                            </label>
                            <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control" OnTextChanged="txtBookingId_TextChanged" AutoComplete="Off" AutoPostBack="true" TabIndex="2" MaxLength="50">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBookingId"
                                ValidationGroup="BookingRe-Sch" SetFocusOnError="True" CssClass="vError">Enter Booking Id</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblreason"><i class="fas fa-calendar-times"></i>Rescheduling Reason</label>
                            <asp:DropDownList ID="ddlRescheduleReason" CssClass="form-control inputboxstyle" runat="server" TabIndex="3">
                                <asp:ListItem Value="0">Select Reschedule Reason</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlRescheduleReason"
                                ValidationGroup="BookingRe-Sch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Reschedule Reason</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-2 col-xs-12" runat="server" id="divBookingNewTime" visible="false">
                        <div class="form-group">
                            <label for="lblboatnum">
                                <i class="fa fa-calendar" aria-hidden="true"></i>
                                Delay Hours
                            </label>
                            <div class="input-group-prepend">
                                <asp:DropDownList ID="dlOh1OpenHours" runat="server" CssClass="form-control" TabIndex="4">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlOh1OpenMinutes" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="dlOh1OpenHours"
                                ValidationGroup="BookingRe-Sch" SetFocusOnError="True" CssClass="vError" InitialValue="0">Enter Hour</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlOh1OpenMinutes"
                                ValidationGroup="BookingRe-Sch" SetFocusOnError="True" CssClass="vError" InitialValue="0">Enter Minutes</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" id="divFromDate" visible="false" runat="server">
                        <div class="form-group">
                            <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control FromDate" AutoComplete="Off" TabIndex="1">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFromDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" id="divToDate" visible="false" runat="server">
                        <div class="form-group">
                            <label for="lblToDate" id="Label2" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control ToDate" AutoComplete="Off" TabIndex="2">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtToDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter To Date</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-4 col-xs-12 dtlBoatType" runat="server" id="divNote" visible="false">
                        <div class="form-group">
                            <label for="lblbooking" style="color: red; font-weight: bold">
                                Note : 
                            </label>
                            <div class="input-group-prepend" style="font-weight: bold">
                                Delay Hours should be given in Number of Hours to delay the bookings from Boat Booking Time.
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Button ID="btnBind" runat="server" Text="Button" CssClass="btnDisplay" />
                <asp:Button ID="btnAdd" runat="server" Text="Button" OnClick="btnAdd_Click" CssClass="btnDisplay" />

                <div class="row" runat="server" id="divSingleRescheduleGrid" visible="false">

                    <div class="divSingleRescheduleColumn col-sm-8 col-xs-12" style="max-height: 350px; overflow: auto;" id="divSingGv" runat="server">
                        <div class="table-responsive">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="lblGridMsgSingle" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </div>

                            <asp:GridView ID="gvSingleReschedule" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId,BoatTypeId,BoatSeaterId,PremiumStatus,TimeSlotId" ShowFooter="true" Visible="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="slblBookingId" runat="server" Text='<%# Bind("BookingId") %>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Boat Reference No." Visible="false" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="slblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>' Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Booking PIN" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="slblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rescheduled date" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="slblRescheduledDate"
                                                runat="server"
                                                Text='<%# Bind("RescheduledDate") %>'
                                                onmouseover="Tooltip(this)"
                                                ToolTip='<%# Eval("BookingDate").ToString().Trim() %>'>
                                            </asp:Label>
                                            <%--Previously Booking Date Was Bind Here--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Mobile" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="slblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Address" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="slblCustomerAddress" runat="server" Text='<%# Bind("CustomerAddress") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Type" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="slblPaymentType" runat="server" Text='<%# Bind("PaymentTypeName") %>' Width="20px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Boat Charge" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="slblNetAmount" runat="server" Text='<%# Bind("BoatCharge") %>' Font-Bold="true" Width="20px" ForeColor="Green"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="slblDepositAmount" runat="server" Text='<%# Bind("DepositAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Slot Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="slblSlotTime" runat="server" Text='<%# Bind("SlotTime") %>' Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reschedule Date" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:TextBox
                                                ID="stxtDate"
                                                onchange=" Scroll(this)"
                                                OnTextChanged="stxtDate_TextChanged"
                                                CssClass="form-control txtboxlen ResDate"
                                                AutoPostBack="true"
                                                runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Change Time Slot" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:DropDownList
                                                onchange=" Scroll(this)"
                                                ID="sddlSlotTime"
                                                OnSelectedIndexChanged="sddlSlotTime_SelectedIndexChanged"
                                                CssClass="form-control"
                                                AutoPostBack="true"
                                                runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gvHead" />
                                <AlternatingRowStyle CssClass="gvRow" />
                                <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="col-sm-4 col-xs-12" id="divSummary" runat="server" visible="false" style="width: 40%">
                        <div class="FinalSummary" style="width: auto; text-align: center; margin-bottom: 5px" id="divRCharge" runat="server">
                            <h4 style="padding-top: 15px;">
                                <asp:Label ID="lblReasonName" runat="server" Font-Bold="true"></asp:Label>
                                <div style="text-align: center;">
                                    <asp:Label ID="lblRescheduleChargeFixed" runat="server" Font-Bold="true" ForeColor="Green" Visible="false"></asp:Label>
                                    <asp:Label ID="lblRescheduleChargePercent" runat="server" Font-Bold="true" ForeColor="Green" Visible="false"></asp:Label>
                                </div>
                            </h4>
                        </div>
                        <div class="row m-0 pt-0 FinalSummary">
                            <div class="col-sm-12">
                                <table class="table mt-0 mb-0">
                                    <thead>
                                        <tr>
                                            <th colspan="2" class="text-center table-th" style="font-weight: bold; font-size: large">SUMMARY</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr runat="server" visible="true">
                                            <th class="table-th" style="font-weight: bold; font-size: large">RESCHEDULE CHARGE</th>
                                            <td style="font-weight: bold; font-size: large">₹ <span id="sumTotal" runat="server"></span></td>
                                        </tr>
                                        <tr>
                                            <th class="table-th" style="font-weight: bold; font-size: large">GST</th>
                                            <td style="font-weight: bold; font-size: large">₹ <span id="sumGst" runat="server"></span></td>
                                        </tr>
                                        <tr>
                                            <th class="table-th" style="font-weight: bold; font-size: large">NET AMOUNT</th>
                                            <td style="font-weight: bold; color: green; font-size: large">₹ <span id="SpNetAmount" runat="server"></span></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="col-sm-10 offset-sm-1 input-group-prepend" style="padding: 10px; margin: 0px;">
                                    <asp:DropDownList ID="ddlPaymentType" CssClass="payMode" runat="server" TabIndex="6">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                            <asp:Button ID="sumResheduleCharge" runat="server" Text="Submit" class="btn btn-primary btnFinal" ValidationGroup="BookingRe-Sch" TabIndex="7"
                                                OnClick="btnSubmit_Click" Width="150px" Font-Bold="True" Style="margin-left: 15px;" />
                                    &nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger btnFinal" TabIndex="8"
                                                OnClick="btnCancel_Click" Style="margin-left: 15px;" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12" id="divBoatHouseReschedule" runat="server" visible="false">
                        <asp:Button ID="btnBHReschedule" OnClick="btnBHReschedule_Click" ValidationGroup="BookingRe-Sch" runat="server" CssClass="btn btn-success" Text="Reschedule" />
                        <asp:Button ID="btnBHRescheduleCancel" OnClick="btnBHRescheduleCancel_Click" CausesValidation="false" runat="server" CssClass="btn btn-danger" Text="Cancel" />
                    </div>
                </div>

                <div runat="server" id="divBulkRescheduleGrid" visible="false">
                    <div class="col-sm-12 col-xs-12" style="max-height: 400px; overflow: auto">
                        <div class="table-responsive">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="lblGridMsgBulk" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </div>
                            <asp:GridView ID="gvBulkReschedule" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId" ShowFooter="true" Visible="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="blblBookingId" runat="server" Text='<%# Bind("BookingId") %>' Font-Bold="true" Font-Size="Larger"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="blblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Booking PIN" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="blblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="blblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Mobile" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="blblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Address" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="blblCustomerAddress" runat="server" Text='<%# Bind("CustomerAddress") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Type" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="blblPaymentType" runat="server" Text='<%# Bind("PaymentTypeName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Boat Charge" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="blblNetAmount" runat="server" Text='<%# Bind("NetAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="blblDepositAmount" runat="server" Text='<%# Bind("DepositAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalRefundAmount" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBookingId" runat="server" onclick="Check_Click(this)" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gvHead" />
                                <AlternatingRowStyle CssClass="gvRow" />
                                <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" Font-Size="Larger" />
                            </asp:GridView>
                        </div>


                    </div>
                    <div class="form-submit">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BookingRe-Sch" TabIndex="9" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancelBulk" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="10" OnClick="btnCancel_Click" />
                    </div>
                </div>

            </div>
        </div>
        <div class="col-md-12" runat="server" id="divGridList" visible="false">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <h5 class="pghr" style="text-align: center; font-size: 25px; display: inline;">Re-Schedule </h5>
                </div>
                <div class="col-md-6 col-sm-6">
                    <span style="float: right; display: inline;">
                        <asp:ImageButton ID="imgbtnTicket" runat="server" ImageUrl="~/images/Ticket.svg" Width="40px" OnClick="imgbtnTicket_Click" CssClass="pr-2" Visible="true" />
                    </span>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-12">
                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                        <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>

                    <div style="text-align: right;">
                        Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="20px" onkeyup="Search_Gridview(this)"></asp:TextBox>


                        <asp:GridView ID="GvBoatRescheduling" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                            AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="15" OnPageIndexChanging="GvBoatRescheduling_PageIndexChanging" OnRowDataBound="GvBoatRescheduling_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>' Font-Bold="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>' Font-Bold="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking Old Date" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingOldDate" runat="server" Text='<%# Bind("BookingOldDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking New Date" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingNewDate" runat="server" Text='<%# Bind("BookingNewDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TimeSlot Old" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOldTimeSlot" runat="server" Text='<%# Bind("OldTimeSlot") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TimeSlot New" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewTimeSlot" runat="server" Text='<%# Bind("NewTimeSlot") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rescheduled Charge" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRescheduledCharge" runat="server" Text='<%# Bind("RescheduledCharge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CGST" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCGST" runat="server" Text='<%# Bind("CGST") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SGST" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSGST" runat="server" Text='<%# Bind("SGST") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rescheduled Total Charge" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRescheduledTotalCharge" runat="server" Text='<%# Bind("RescheduledTotalCharge") %>' Font-Bold="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Payment Type" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentName" runat="server" Text='<%# Bind("PaymentName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking Media" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingMedia" runat="server" Text='<%# Bind("BookingMedia") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Bind("CreatedDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gvHead" />
                            <AlternatingRowStyle CssClass="gvRow" />
                            <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <asp:HiddenField ID="hfBookingTo" runat="server" />
    <asp:HiddenField ID="hfBookingFrom" runat="server" />
    <asp:HiddenField ID="hfScrollX" runat="server" />
    <asp:HiddenField ID="hfScrollY" runat="server" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

