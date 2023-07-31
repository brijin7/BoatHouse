<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master"
    AutoEventWireup="true" CodeFile="ChangeTripSheet.aspx.cs" Inherits="Boating_ChangeTripSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function Search_Gridview(strKey) {
            var strData = strKey.value.toLowerCase().split(" ");
            var Grid = "<%=GvChangeBoatBooking.ClientID%>";
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
        function Search_Gridview1(strKey) {
            var strData = strKey.value.toLowerCase().split(" ");
            var Grid = "<%=gvViewLogDetails.ClientID%>";
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
        $(document).ready(function () {
            $(".BookedDate").datepicker({
                dateFormat: 'dd-mm-yy',
                maxDate: 0,
                changeMonth: true,
                changeYear: true,

            });
        });

    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            const date = document.getElementById("<%=txtBookingDate.ClientID %>");
            if (date != null) {
                $(".toDateChange").datepicker({
                    dateFormat: 'dd-mm-yy',
                    minDate: document.getElementById("<%=txtBookingDate.ClientID %>").value,
                    maxDate: 0,
                    maxDate: 365,
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (selected) {
                        $(".BookedDate").datepicker("option", "minDate", selected)
                    }
                });
            }


        });
    </script>
    <div class="form-body form-body-ChangeTripSheet" style="overflow-x: hidden;">
        <h5 class="pghr">Change Trip Sheet <span style="float: right;">
            <asp:LinkButton ID="lbtnView" CssClass="lbtnViewLog" runat="server" OnClick="lbtnViewLog_Click"> 
                <i class="fas fa-receipt"></i>Changed Trip List</asp:LinkButton>
            <asp:LinkButton ID="lbtnGrid" CssClass="lbtnViewLog" runat="server" OnClick="lbtnGrid_Click" Visible="false"> 
                <i class="fas fa-list"></i> Today Trip List</asp:LinkButton></span></h5>
        <hr />
        <div id="divtripSheet" runat="server" visible="false">
            <div class="row">
                <div class="col-xl-3 col-md-3 col-lg-3 col-sm-3" id="divsadmintrpsheet"
                    runat="server" visible="false">
                    <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                        <i class="fa fa-calendar" aria-hidden="true"></i>Booked Date</label>
                    <asp:TextBox ID="txtBookingDate"
                        OnTextChanged="txtBookingDate_TextChanged"
                        AutoPostBack="true"
                        runat="server"
                        CssClass="form-control BookedDate"
                        AutoComplete="Off"
                        TabIndex="2">
                    </asp:TextBox>
                </div>
            </div>
        </div>
        <div runat="server" id="divGridList" visible="false">
            <div class="divDisblePagination mt-2">
                <div class="row  justify-content-end">
                    <div id="divBookingPin_Main" runat="server" class="mr-3 my-3">
                        <asp:TextBox ID="txtBookingPin"
                            OnTextChanged="txtBookingPin_TextChanged"
                            AutoPostBack="true"
                            runat="server"
                            placeholder="Booking Pin/Booking Id"
                            CssClass="form-control"
                            AutoComplete="Off"
                            TabIndex="1">
                        </asp:TextBox>
                    </div>
                    <div class="col-12 col-xs-12 col-sm-12 col-md-12 col-xl-12">
                        <div class="table-responsive" style="overflow-x: hidden;">
                            <asp:GridView ID="GvChangeBoatBooking"
                                runat="server"
                                CssClass="gvv display table table table-bordered table-condenced"
                                AutoGenerateColumns="False"
                                HeaderStyle-BackColor="#124a79"
                                HeaderStyle-ForeColor="white"
                                AllowPaging="true"
                                OnPageIndexChanging="GvChangeBoatBooking_PageIndexChanging"
                                DataKeyNames="BookingId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label
                                                ID="lblRowNumber"
                                                runat="server"
                                                Text='<%# Bind("RowNumber") %>' Width="20px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Reference No" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatRefNo" runat="server" Text='<%# Bind("BoatReferenceNum") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="RowerId" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrowerid" runat="server" Text='<%# Bind("RowerId") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Rower Name" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat TypeId" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat SeatId" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatseatId" runat="server" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Seat" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatseat" runat="server" Text='<%# Bind("SeatType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ActualBoatId" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActualBoatId" runat="server" Text='<%# Bind("ActualBoatId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatNum" runat="server" Text='<%# Bind("ActualBoatNum") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expected Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpectedTime" runat="server" Text='<%# Bind("ExpectedTime") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expected Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbloldTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expected Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbloldTripEndTime" runat="server" Text='<%# Bind("TripEndTime") %>'></asp:Label>
                                            <asp:Label ID="lblOldBoatDuration" runat="server" Text='<%# Bind("BoatDuration") %>'></asp:Label>
                                            <asp:Label ID="lblOldTravelDuration" runat="server" Text='<%# Bind("TravelDuration") %>'></asp:Label>
                                            <asp:Label ID="lblOldBoatDeposit" runat="server" Text='<%# Bind("BoatDeposit") %>'></asp:Label>
                                            <asp:Label ID="lblOldDepRefundAmount" runat="server" Text='<%# Bind("OldDepRefundAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Change" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                                runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
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
                <div id="divBackToTableLsit" runat="server" class="row justify-content-start ml-3 mt-2" visible="false">
                    <div class="col-3 col-xs-3 col-sm-3 col-md-3 col-xl-3 d-block text-left">
                        <asp:Button ID="btnBackToTableList" OnClick="btnBackToTableList_Click" runat="server" Text="← Back To Trip List" CssClass="btn btn-color" />
                    </div>
                </div>
                <div id="divPreviousAndNext" runat="server" class="row justify-content-start ml-3 mt-2" visible="false">
                    <div class="col-3 col-xs-3 col-sm-3 col-md-3 col-xl-3 d-block text-left">
                        <asp:Button ID="btnPrevious" runat="server" Text="← Previous" OnClick="btnPrevious_Click" CssClass="btn btn-color" />
                        <asp:Button ID="btnNext" runat="server" Text="Next →" OnClick="btnNext_Click" CssClass="btn btn-color" />
                    </div>
                </div>
            </div>
        </div>
        <div id="divReScheduleBoat" runat="server" visible="false" style="margin: 10px;">

            <span class="frmhdr">Booking Details</span>
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatNum">
                                <i class="fa fa-ship"></i>
                                Booking Id
                            </label>
                            <asp:TextBox ID="txtBookId" runat="server" CssClass="form-control" Font-Bold="true" Enabled="false">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatNum">
                                <i class="fa fa-ship"></i>
                                Booking Pin
                            </label>
                            <asp:TextBox ID="txtBookPin" runat="server" CssClass="form-control" Font-Bold="true" Enabled="false">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatNum">
                                <i class="fa fa-ship"></i>
                                Boat Type
                            </label>
                            <asp:TextBox ID="txtBtType" runat="server" CssClass="form-control" Font-Bold="true" Enabled="false">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatNum">
                                <i class="fa fa-ship"></i>
                                Boat Seater 
                            </label>
                            <asp:TextBox ID="txtBtSeater" runat="server" CssClass="form-control" Font-Bold="true" Enabled="false">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatNum">
                                <i class="fa fa-ship"></i>
                                Premium Status
                            </label>
                            <asp:TextBox ID="txtPremiumStatus" runat="server" CssClass="form-control" Font-Bold="true" Enabled="false">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatNum">
                                <i class="fa fa-ship"></i>
                                Boat Number :
                            </label>
                            <asp:DropDownList ID="ddlBoatNum" runat="server" CssClass="form-control inputboxstyle" TabIndex="4" Font-Bold="true" Enabled="false">
                                <asp:ListItem Value="0"> Select Boat Number</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlBoatNum" InitialValue="0"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Select Boat Number</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblMbl">
                                <i class="fa fa-mobile"></i>
                                Rower Name :
                            </label>
                            <asp:DropDownList ID="ddlrowername" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="false" TabIndex="1">
                                <asp:ListItem Value="0"> Select Rower Name</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblNewExpectedTime">
                                <i class="fa fa-clock"></i>
                                Expected Time :
                            </label>
                            <asp:DropDownList ID="ddlExpectedTime" runat="server" CssClass="form-control" TabIndex="5" Width="150px" Font-Bold="true" Enabled="false"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlExpectedTime"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Select Expected Time</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblboatnum">
                                Deposit Amount
                            </label>
                            <div class="input-group-prepend">
                                <asp:TextBox ID="txtOldBoatDeposit" runat="server" CssClass="form-control" AutoComplete="Off"
                                    MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <span class="frmhdr">Old Trip Change Details</span>
                <div class="mydivbrdr">
                    <div class="row p-2" style="background-color: #bdd6ec;">
                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblboatnum">
                                    Boat Duration (Mins)
                                </label>
                                <div class="input-group-prepend">
                                    <asp:TextBox ID="txtOldBoatDuration" runat="server" CssClass="form-control" AutoComplete="Off"
                                        MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblboatnum">
                                    <i class="fa fa-calendar" aria-hidden="true"></i>
                                    Trip Start Date Time
                                </label>
                                <div class="input-group-prepend">
                                    <asp:TextBox ID="txtTripStartDate" runat="server" CssClass="form-control"
                                        AutoComplete="Off" MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                                    </asp:TextBox>

                                    <asp:TextBox ID="txtstarttime" TextMode="Time" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="5"
                                        Font-Bold="true" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblboatnum">
                                    <i class="fa fa-calendar" aria-hidden="true"></i>
                                    Trip End Date Time
                                </label>
                                <div class="input-group-prepend">
                                    <asp:TextBox ID="txtenddate" runat="server" CssClass="form-control" AutoComplete="Off"
                                        MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                                    </asp:TextBox>
                                    <asp:TextBox ID="txtendtime" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="100" TabIndex="5"
                                        TextMode="Time" Font-Bold="true" Enabled="false">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblboatnum">
                                    Travel Duration (Mins)
                                </label>
                                <div class="input-group-prepend">
                                    <asp:TextBox ID="txtOldTravelledDuration" runat="server" CssClass="form-control" AutoComplete="Off"
                                        MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblboatnum">
                                    Deposit Refund Amount
                                </label>
                                <div class="input-group-prepend">
                                    <asp:TextBox ID="txtOldDepositRefundAmt" runat="server" CssClass="form-control" AutoComplete="Off"
                                        MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <span class="frmhdr">New Trip Change Details</span>
            <div class="mydivbrdr">
                <div class="row p-2" style="background-color: #b2e4e4">
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblboatnum">
                                Boat Duration (Mins)
                            </label>
                            <asp:TextBox ID="txtNewBoatDuration" runat="server" CssClass="form-control" AutoComplete="Off"
                                MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewBoatDuration"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Enter New Boat Duration</asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblboatnum">
                                <i class="fa fa-calendar" aria-hidden="true"></i>
                                Trip Start Date Time
                            </label>
                            <div class="input-group-prepend">
                                <asp:TextBox ID="txtNewTripStartDate" runat="server" CssClass="form-control frmDate" AutoComplete="Off" MaxLength="100" TabIndex="4" ReadOnly="true" Enabled="false">
                                </asp:TextBox>

                                <asp:TextBox ID="txtNewstarttime" TextMode="Time" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="5" Enabled="false"
                                    OnTextChanged="txtstarttime_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtTripStartDate"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Trip Start Date</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtstarttime"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Trip Start Time</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblboatnum">
                                <i class="fa fa-calendar" aria-hidden="true"></i>
                                Trip End Date Time
                            </label>
                            <div class="input-group-prepend">
                                <asp:TextBox ID="txtNewenddate" runat="server" CssClass="form-control frmDate" AutoComplete="Off" MaxLength="100" TabIndex="4" Enabled="false">
                                </asp:TextBox>

                                <asp:TextBox ID="txtNewendtime" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="100" TabIndex="5"
                                    OnTextChanged="txtendtime_TextChanged" AutoPostBack="true" TextMode="Time">
                                </asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtenddate"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Trip End Date</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtendtime"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Trip End Time</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblboatnum">
                                Travel Duration (Mins)
                            </label>
                            <asp:TextBox ID="txtNewTravelDuration" runat="server" CssClass="form-control" AutoComplete="Off"
                                MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewTravelDuration"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Enter Travel Duration</asp:RequiredFieldValidator>

                        </div>
                    </div>

                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblboatnum">
                                Deposit Refund Amount
                            </label>
                            <asp:TextBox ID="txtNewDepositRefundAmt" runat="server" CssClass="form-control" AutoComplete="Off"
                                MaxLength="100" TabIndex="4" ReadOnly="true" Font-Bold="true">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNewDepositRefundAmt"
                                ValidationGroup="Re-Schedule" SetFocusOnError="True" CssClass="vError">Enter Deposit Refund Amount</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-lg-12 col-sm-12" style="margin-top: 31px; text-align: right">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" class="btn btn-primary" ValidationGroup="Re-Schedule" TabIndex="6" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnReCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger" TabIndex="7" OnClick="btnReCancel_Click" />
                </div>
            </div>
        </div>
        <div id="divLogDetails" runat="server" visible="false">
            <div class="divDisblePagination mt-2">
                <div class="row justify-content-end my-3" id="divLogDetailsBookingPinFilter" runat="server" visible="true">
                    <div id="divBookingPin_Log" runat="server" class="mr-3 my-3 justify-content-end">
                        <asp:TextBox ID="txtBookingPinOrIdLog"
                            OnTextChanged="txtBookingPinOrIdLog_TextChanged"
                            AutoPostBack="true"
                            runat="server"
                            placeholder="Booking Pin/Booking Id"
                            CssClass="form-control"
                            AutoComplete="Off"
                            TabIndex="1">
                        </asp:TextBox>
                    </div>
                    <div class="col-12 col-xs-12 col-sm-12 col-md-12 col-xl-12">
                        <div class="table-responsive" style="overflow-x: hidden;">
                            <asp:GridView
                                ID="gvViewLogDetails"
                                runat="server" CssClass="gvv display table table-bordered table-condenced"
                                HeaderStyle-BackColor="#124a79"
                                HeaderStyle-ForeColor="white"
                                AutoGenerateColumns="False"
                                DataKeyNames="BookingId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowNumberLog" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Reference No" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatRefNo" runat="server" Text='<%# Bind("BoatReferenceNum") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booked Date" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBBookedDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Seat" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeatType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Travel Duration (Mins)" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOldTravelDur" runat="server" Text='<%# Bind("OldTravelDuration") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Trip Start Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewTripStartTime" runat="server" Text='<%# Bind("NewTripStartTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Trip End Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewTripEndTime" runat="server" Text='<%# Bind("NewTripEndTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Travel Duration (Mins)" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewTravelDur" runat="server" Text='<%# Bind("NewTravelDuration") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat TypeId" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                            <asp:Label ID="lblNewBoatNum" runat="server" Text='<%# Bind("NewBoatNum") %>'></asp:Label>
                                            <asp:Label ID="lblNewExpectedTime" runat="server" Text='<%# Bind("NewExpectedTime") %>'></asp:Label>
                                            <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                            <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                            <asp:Label ID="lblNewRowerId" runat="server" Text='<%# Bind("NewRowerId") %>'></asp:Label>
                                            <asp:Label ID="lblNewBoatId" runat="server" Text='<%# Bind("NewBoatId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="More" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgBtnView" ForeColor="#512c88" CausesValidation="false" Font-Underline="false"
                                                Width="20px" CssClass="imgOutLine" OnClick="ImgBtnView_Click"
                                                runat="server" Font-Bold="true" ImageUrl="~/images/View.png" />
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
            <div id="divBackToLog" runat="server" class="justify-content-start ml-5 mt-2" visible="false">
                <div>
                    <asp:Button
                        ID="btnBackToLog"
                        OnClick="btnBackToLog_Click"
                        runat="server"
                        Text="← Back To Trip List"
                        CssClass="btn btn-color" />
                </div>
            </div>
            <div id="divPreviousAndNextLog" runat="server" class="justify-content-start ml-5 mt-2" visible="false">
                <div class="row">
                    <div>
                        <asp:Button
                            ID="btnPreviousLog"
                            runat="server"
                            Text="← Previous"
                            OnClick="btnPreviousLog_Click"
                            CssClass="btn btn-color mr-3" />
                    </div>
                    <div>
                        <asp:Button
                            ID="btnNextLog"
                            runat="server"
                            Text="Next →"
                            OnClick="btnNextLog_Click"
                            CssClass="btn btn-color" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfBoatStatus" runat="server" />
    <asp:HiddenField ID="hfActualId" runat="server" />
    <asp:HiddenField ID="hfBookingDate" runat="server" />
    <div class="modal" id="myModal">
        <div class="modal-dialog modal-dialog-centered" style="max-width: 1000px !important">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header" style="background-color: #004c8c; color: white">
                    <div class="row col-sm-12">
                        <div class="col-sm-3 col-xs-12">
                            <h4 class="modal-title">Trip Log Details</h4>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <h5 class="modal-title" style="margin-top: 3px !important;">Booking Id : 
                                    <asp:Label ID="lblPopBookId" runat="server" Font-Bold="true"></asp:Label></h5>
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <h5 class="modal-title" style="margin-top: 3px !important;">Boat Type : 
                                    <asp:Label ID="lblPopBoatType" runat="server" Font-Bold="true"></asp:Label>
                                (
                                <asp:Label ID="lblPopBoatSeat" runat="server" Font-Bold="true"></asp:Label>
                                )

                            </h5>
                        </div>
                    </div>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: white;">
                        <span aria-hidden="true">&times;</span></button>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <div class="row m-0">
                        <div class="col-sm-4 col-xs-12">
                            <div class="row m-0">
                                <div class="col-sm-6 col-xs-12">
                                    <label for="boat">Booking Pin </label>
                                </div>
                                <div class="col-sm-1 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <asp:Label ID="lblPopBookPin" runat="server" Font-Bold="true">
                                    </asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="row m-0">
                                <div class="col-sm-6 col-xs-12">
                                    <label for="boat">Booking Reference </label>
                                </div>
                                <div class="col-sm-1 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <asp:Label ID="lblPopBookRef" runat="server" Font-Bold="true">
                                    </asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="row">
                                <div class="col-sm-6 col-xs-12">
                                    <label for="boat">Rower </label>
                                </div>
                                <div class="col-sm-1 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <asp:Label ID="lblPopRower" runat="server" Font-Bold="true">
                                    </asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <h5>Old Details</h5>
                    <div id="divModalOldGrid" runat="server">
                        <div class="table-responsive">
                            <asp:GridView ID="gvOldLogDet" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="10" OnPageIndexChanging="gvOldLogDet_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Rower" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldRowerName" runat="server" Text='<%# Bind("OldRowerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Boat Number" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldBoatNum" runat="server" Text='<%# Bind("OldBoatNum") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Expected Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldExpectedTime" runat="server" Text='<%# Bind("OldExpectedTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Trip Start Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldTripStartTime" runat="server" Text='<%# Bind("OldTripStartTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Trip End Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldTripEndTime" runat="server" Text='<%# Bind("OldTripEndTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Deposit Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldDepRefundAmount" runat="server" Text='<%# Bind("OldDepRefundAmount") %>'></asp:Label>
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

                    <h5>New Details</h5>
                    <div id="divModalNewGrid" runat="server">
                        <div class="table-responsive">
                            <asp:GridView ID="gvNewLogDet" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="10" OnPageIndexChanging="gvNewLogDet_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Rower" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Boat Number" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewBoatNum" runat="server" Text='<%# Bind("NewBoatNum") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Expected Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewExpectedTime" runat="server" Text='<%# Bind("NewExpectedTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Trip Start Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewTripStartTime" runat="server" Text='<%# Bind("NewTripStartTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Trip End Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewTripEndTime" runat="server" Text='<%# Bind("NewTripEndTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Deposit Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewDepRefundAmount" runat="server" Text='<%# Bind("NewDepRefundAmount") %>'></asp:Label>
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
    </div>
    <script type="text/javascript">
        $(function () {
            $("#ImgBtnView").click(function () {
                showModal();
            });
        });
        function showModal() {
            $("#myModal").modal('show');
        }
        function hideModal() {
            $("#myModal").modal('hide');
        }
    </script>
    <%--    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js"
        integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>--%>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.min.js"
        integrity="sha384-+sLIOodYLS7CIrQpBjl+C7nPvqq+FbNUBDunl/OZv93DB7Ln/533i8e/mZXLi/P+" crossorigin="anonymous"></script>
    <%--<script type="text/javascript" src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js"
        integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossorigin="anonymous"></script>--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

