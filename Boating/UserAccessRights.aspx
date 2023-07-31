<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Boating/UserAccessRights.aspx.cs"
    Inherits="Boating_UserAccessRights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        label {
            font-weight: 500;
            color: black !important;
            font-size: 0.9rem;
        }

        .panel-group .panel + .panel {
            margin-top: 5px;
        }

        .panel-group .panel {
            margin-bottom: 0;
            border-radius: 4px;
        }

        .panel-success {
            border: 1px;
            border-color: #d6e9c6;
        }

        .panel {
            margin-bottom: 5px;
            background-color: #fff;
            border: 1px solid transparent;
            border-radius: 4px;
            -webkit-box-shadow: 0 1px 1px rgba(0,0,0,.05);
            box-shadow: 0 1px 1px rgba(0,0,0,.05);
        }

        .panel-success > .panel-heading {
            color: #3c763d;
            background-color: #dff0d8;
            border-color: #d6e9c6;
            font-size: 14px;
            font-weight: 800;
            text-align: left;
        }


        .panel-group .panel-heading {
            border-bottom: 0;
        }

        .panel-heading {
            padding: 5px 5px;
            border-bottom: 1px solid transparent;
            border-top-left-radius: 3px;
            border-top-right-radius: 3px;
        }

        .panel-body {
            padding: 0px;
            border: 1px solid #d6e9c6;
        }

        .lblChrg {
            padding: 10px 10px;
        }

        .chkChoi {
            color: white !important;
        }

        .chkChoice input {
            margin-right: 5px;
        }

        .holepagenames {
            padding: 8px;
            border-radius: 2px;
            cursor: pointer;
            transition: 0.2s all linear;
            border: 1px solid #fff;
        }

        .holepagenamesdef {
            background: #daeefd;
            color: #000000;
        }

        .holepagenamesorg {
            background: #fdf0e4;
            color: #000000;
        }

        .holepagenamesdarkpurp {
            background: #d5d5fd;
            color: #000000;
        }

        .holepagenamespurp {
            background: #f1dfff;
            color: #000000;
        }

        .holepagenamesgre {
            background: #f5ffdf;
            color: #000000;
        }

        .dividerprpage {
            border-bottom: 1px dashed #000;
            width: 15px;
            /* height: 5px; */
            position: absolute;
            margin-top: -30px;
            margin-left: -15px;
        }

            .dividerprpage:last-child::after {
                background: #eee;
                content: '';
                position: absolute;
                width: 2px;
                height: 20px;
                margin-top: 2px;
                margin-left: -2px;
            }
    </style>
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">User Access Rights</h5>
        <hr />
        <div id="divEntry" runat="server">
            <div class="row" runat="server">
                <div class="col-sm-3 col-xs-12">
                    <div class="panel panel-success">
                        <div class="panel-heading panelheadchk">
                            User Name <span class="spStar">*</span>
                        </div>
                        <div class="panel-body" style="padding: 10px">
                            <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control" TabIndex="2"
                                OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlUserName"
                                ValidationGroup="Employee" SetFocusOnError="True" InitialValue="Select User Name" CssClass="vError">Select User Name</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <div class="panel panel-success">
                        <div class="panel-heading panelheadchk">
                            Module Type <span class="spStar">*</span>
                        </div>
                        <div class="panel-body" style="padding: 10px">
                            <asp:CheckBox ID="ChkBoatModule" runat="server" CssClass="chkChoice" Text="Boating" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <div class="panel panel-success">
                        <div class="panel-heading panelheadchk">
                            Android Module
                        </div>
                        <div class="panel-body" style="padding: 10px">
                            <asp:CheckBox ID="ChkAnroidOffline" runat="server" CssClass="chkChoice" Text="Offline Rights" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <div class="text-center pt-3">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary"
                            ValidationGroup="Employee" TabIndex="5" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                            class="btn  btn-danger" TabIndex="6" OnClick="btnCancel_Click" Visible="false" />
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="divPages">
                <div class="col-sm-12 col-xs-12">
                    <div class="panel panel-success">
                        <div class="panel-heading panelheadchk">
                            Boating Menu Rights <span class="spStar">*</span>
                        </div>
                        <div class="panel-body">
                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk" style="background-color: #f7d9bf !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12">
                                            <p style="color: black !important">
                                                <asp:CheckBox ID="chkMenuBooking" runat="server" CssClass="chkChoice" Text="Booking" Font-Bold="true" ForeColor="Black"
                                                    OnCheckedChanged="chkMenuBooking_CheckedChanged" AutoPostBack="true" />
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamesorg col-sm-12 col-xs-12" id="divBooking" runat="server">
                                            <asp:CheckBoxList ID="ckblBooking" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"
                                                CellPadding="6" CellSpacing="6" Width="100%" TabIndex="4" RepeatColumns="5"
                                                OnSelectedIndexChanged="ckblBooking_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="bbmb1">Boat Booking</asp:ListItem>
                                                <%--  <asp:ListItem Value="bbmb2">Boat Booking - Other Facilities</asp:ListItem>--%>
                                                <asp:ListItem Value="bbmb3">Bulk Boat Booking</asp:ListItem>
                                                <asp:ListItem Value="bbmb4">Additional Ticket</asp:ListItem>
                                                <asp:ListItem Value="bbmb5">Other Services</asp:ListItem>
                                                <asp:ListItem Value="bbmb6">Bulk Other Services</asp:ListItem>
                                                <asp:ListItem Value="bbmb14">Restaurant Services</asp:ListItem>

                                                <asp:ListItem Value="bbmb8">Trip Sheet</asp:ListItem>

                                                <asp:ListItem Value="bbmb12">Cancellation</asp:ListItem>
                                                <asp:ListItem Value="bbmb13">Re-Scheduling</asp:ListItem>

                                                <asp:ListItem Value="bbmb15">Generate Boarding Pass</asp:ListItem>
                                                <asp:ListItem Value="bbmb16">Generate Manual Ticket</asp:ListItem>
                                                <asp:ListItem Value="bbmb11">Change Boat</asp:ListItem>
                                                <asp:ListItem Value="bbmb9">Change Trip Sheet</asp:ListItem>
                                                <asp:ListItem Value="bbmb10">Re-Trip Entry</asp:ListItem>
                                                <asp:ListItem Value="bbmb7">Kiosk Boat Booking</asp:ListItem>
                                                <%--Newly Added--%>
                                                <asp:ListItem Value="bbmb17">Kiosk Other Services</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk" style="background-color: #88cbff !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12">
                                            <asp:CheckBox ID="chkMenuBoatingSvc" runat="server" CssClass="chkChoice" Text="Boating Services" Font-Bold="true" ForeColor="Black"
                                                OnCheckedChanged="chkMenuBoatingSvc_CheckedChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamesdef col-sm-12 col-xs-12" id="divBookSvc" runat="server">
                                            <asp:CheckBoxList ID="ckblBoatingSvc" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"
                                                CellPadding="6" CellSpacing="6" Width="100%" TabIndex="4" RepeatColumns="5"
                                                OnSelectedIndexChanged="ckblBoatingSvc_Changed" AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk" style="background-color: #bcbcf9 !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12" id="divAddlService" runat="server">
                                            <asp:CheckBox ID="chkMenuAdditionalSvc" runat="server" CssClass="chkChoice" Text="Additional Ticket" Font-Bold="true" ForeColor="Black"
                                                OnCheckedChanged="chkMenuAdditionalSvc_CheckedChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamesdarkpurp col-sm-12 col-xs-12" id="divAddlSvc" runat="server">
                                            <asp:CheckBoxList ID="ckblAddlSvc" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"
                                                CellPadding="6" CellSpacing="6" Width="100%" TabIndex="4" RepeatColumns="5"
                                                OnSelectedIndexChanged="ckblAddlSvc_Changed" AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk" style="background-color: #e1c2f9 !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12" id="divOther" runat="server">
                                            <asp:CheckBox ID="chkMenuOtherSvc" runat="server" CssClass="chkChoice" Text="Other Service" Font-Bold="true" ForeColor="Black"
                                                OnCheckedChanged="chkMenuOtherSvc_CheckedChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamespurp col-sm-12 col-xs-12" id="divOthSvc" runat="server">
                                            <asp:CheckBoxList ID="ckblOtherSvc" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"
                                                CellPadding="6" CellSpacing="6" Width="100%" TabIndex="4" RepeatColumns="5"
                                                OnSelectedIndexChanged="ckblOtherSvc_Changed" AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk" style="background-color: #e1f5b2 !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12" id="divTrip" runat="server">
                                            <asp:CheckBox ID="chkMenuTripSheet" runat="server" CssClass="chkChoice" Text="Trip Sheet" Font-Bold="true" ForeColor="Black"
                                                OnCheckedChanged="chkMenuTripSheet_CheckedChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamesgre col-sm-12 col-xs-12" id="divTripSheet" runat="server">
                                            <asp:CheckBoxList ID="ckblTripSheet" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"
                                                CellPadding="6" CellSpacing="6" Width="100%" TabIndex="4" RepeatColumns="5"
                                                OnSelectedIndexChanged="ckblTripSheet_Changed" AutoPostBack="true">
                                                <asp:ListItem Value="Y">Trip Start</asp:ListItem>
                                                <asp:ListItem Value="Y">Trip End</asp:ListItem>
                                                <asp:ListItem Value="Y">Smart Trip Sheet</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk" style="background-color: #88cbff !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12">
                                            <asp:CheckBox ID="chkMenuTransaction" runat="server" CssClass="chkChoice" Text="Transaction" Font-Bold="true" ForeColor="Black" />

                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamesdef col-sm-12 col-xs-12" id="divTrans" runat="server">
                                            <asp:CheckBoxList ID="ckblTransaction" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"
                                                CellPadding="6" CellSpacing="6" Width="100%" TabIndex="4" RepeatColumns="5"
                                                OnSelectedIndexChanged="ckblTransaction_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="bbmt3">Deposit Refund</asp:ListItem>
                                                <asp:ListItem Value="bbmt4">Rower Settlement</asp:ListItem>
                                                <asp:ListItem Value="bbmt5">Refund Counter</asp:ListItem>
                                                <asp:ListItem Value="bbmt7">Receipt Balance Refund</asp:ListItem>
                                                <asp:ListItem Value="bbmt6">Restaurant Stock Entry</asp:ListItem>
                                                <asp:ListItem Value="bbmt1">Material Purchase</asp:ListItem>
                                                <asp:ListItem Value="bbmt2">Material Issue</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk" style="background-color: #e1c2f9 !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12">
                                            <asp:CheckBox ID="chkMenuDepositRefund" runat="server" CssClass="chkChoice" Text="Deposit Refund" Font-Bold="true"
                                                ForeColor="Black" OnCheckedChanged="chkMenuDepositRefund_CheckedChanged" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamespurp col-sm-12 col-xs-12" id="divDR" runat="server">
                                            <asp:CheckBoxList ID="ckblDepositRefund" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"
                                                CellPadding="6" CellSpacing="6" Width="100%" TabIndex="4" RepeatColumns="5"
                                                OnSelectedIndexChanged="ckblDepositRefund_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="bbmdr1">Scan QR</asp:ListItem>
                                                <asp:ListItem Value="bbmdr2">Pin</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk" style="background-color: #e1f5b2 !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12">
                                            <asp:CheckBox ID="chkMenuReports" runat="server" CssClass="chkChoice" Text="Reports" Font-Bold="true" ForeColor="Black" />
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">

                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamesgre col-sm-12 col-xs-12" id="divRpt" runat="server">
                                            <asp:CheckBoxList ID="ckblReports" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"
                                                CellPadding="6" CellSpacing="6" Width="100%" TabIndex="4" RepeatColumns="5">

                                                <asp:ListItem Value="bbmr1">Print Boat Booking</asp:ListItem>
                                                <asp:ListItem Value="bbmr18">Print Additional Ticket</asp:ListItem>
                                                <asp:ListItem Value="bbmr2">Print Other Services</asp:ListItem>
                                                <asp:ListItem Value="bbmr3">Print Restaurant Services</asp:ListItem>

                                                <asp:ListItem Value="bbmr4">Abstract Boat Booking</asp:ListItem>
                                                <asp:ListItem Value="bbmr19">Abstract Additional Ticket</asp:ListItem>
                                                <asp:ListItem Value="bbmr5">Abstract Other Services</asp:ListItem>
                                                <asp:ListItem Value="bbmr6">Abstract Restaurant Services</asp:ListItem>

                                                <asp:ListItem Value="bbmr8">Boat Wise Trip</asp:ListItem>
                                                <asp:ListItem Value="bbmr9">Trip Sheet Summary</asp:ListItem>
                                                <asp:ListItem Value="bbmr16">Trip Wise Collection</asp:ListItem>
                                                <asp:ListItem Value="bbmr20">Deposit Status</asp:ListItem>
                                                <asp:ListItem Value="bbmr21">Discount Report</asp:ListItem>
                                                <asp:ListItem Value="bbmr22">Cash In Hand</asp:ListItem>

                                                <asp:ListItem Value="bbmr10">Rower Charges</asp:ListItem>
                                                <asp:ListItem Value="bbmr12">Rower Settlement</asp:ListItem>
                                                <asp:ListItem Value="bbmr17">Boat Type Rower List</asp:ListItem>
                                                <asp:ListItem Value="bbmr23">Extended Boat Rides</asp:ListItem>

                                                <asp:ListItem Value="bbmr24">Print Credit Boat Booking</asp:ListItem>
                                                <asp:ListItem Value="bbmr25">Credit Trip Wise Details</asp:ListItem>

                                                <asp:ListItem Value="bbmr7">Available Boats With Capacity</asp:ListItem>
                                                <asp:ListItem Value="bbmr11">Boat Cancellation</asp:ListItem>
                                                <asp:ListItem Value="bbmr13">Challan Register</asp:ListItem>
                                                <asp:ListItem Value="bbmr14">Service Wise Report</asp:ListItem>
                                                <asp:ListItem Value="bbmr15">User Based Booking Details</asp:ListItem>
                                                <asp:ListItem Value="bbmr26">Receipt Balance Report</asp:ListItem>
                                                <asp:ListItem Value="bbmr27">Reprint Report</asp:ListItem>
                                                <asp:ListItem Value="bbmr28">QR Code Generation</asp:ListItem>

                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-success" style="display:none">
                                <div class="panel-heading panelheadchk" style="background-color: #f7d9bf !important;">
                                    <div class="row p-0 m-0">
                                        <div class="col-sm-12 col-xs-12">
                                            Boat Information Display
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="row p-0 m-0">
                                        <div class="holepagenames holepagenamesorg col-sm-12 col-xs-12" id="divBoatInfoDisplay" runat="server">
                                            <asp:CheckBox ID="chkBoatInfoDisp" runat="server" CssClass="chkChoice" Text="Boat Info Display" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <br />
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvAdminAccess" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatHouseId, OfflineRights, MBoating" PageSize="25000" OnPageIndexChanging="gvAdminAccess_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%--<%#Container.DataItemIndex+1 %>--%>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UniqueId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUniqueId" runat="server" Text='<%# Bind("UniqueId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="UserRole" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUserRole" runat="server" Text='<%# Bind("UserRole") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Role Name" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BMaster" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBMaster" runat="server" Text='<%# Eval("BMaster").ToString() == "Y" ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BBooking" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBBooking" runat="server" Text='<%# Eval("BBooking").ToString() == "Y" ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BTransaction" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBTransaction" runat="server" Text='<%# Eval("BTransaction").ToString() == "Y" ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BReports" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBReports" runat="server" Text='<%# Eval("BReports").ToString() == "Y" ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Restaurant" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBRestaurant" runat="server" Text='<%# Eval("BRestaurant").ToString() == "Y" ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat House" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMBoatInfoDisplay" runat="server" Text='<%# Bind("MBoatInfoDisplay") %>'></asp:Label>

                                <asp:Label ID="lblBBoatingService" runat="server" Text='<%# Bind("BBoatingService") %>'></asp:Label>
                                <asp:Label ID="lblBAdditionalService" runat="server" Text='<%# Bind("BAdditionalService") %>'></asp:Label>
                                <asp:Label ID="lblBOtherService" runat="server" Text='<%# Bind("BOtherService") %>'></asp:Label>
                                <asp:Label ID="lblBMBooking" runat="server" Text='<%# Bind("BBMBooking") %>'></asp:Label>
                                <asp:Label ID="lblBMBookingOthers" runat="server" Text='<%# Bind("BBMBookingOthers") %>'></asp:Label>
                                <asp:Label ID="lblBMBulkBooking" runat="server" Text='<%# Bind("BBMBulkBooking") %>'></asp:Label>
                                <asp:Label ID="lblBMAdditionalService" runat="server" Text='<%# Bind("BBMAdditionalService") %>'></asp:Label>
                                <asp:Label ID="lblBMOtherService" runat="server" Text='<%# Bind("BBMOtherService") %>'></asp:Label>
                                <asp:Label ID="lblBMBulkOtherService" runat="server" Text='<%# Bind("BBMBulkOtherService") %>'></asp:Label>
                                <asp:Label ID="lblBMKioskBooking" runat="server" Text='<%# Bind("BBMKioskBooking") %>'></asp:Label>
                                <asp:Label ID="lblBMTripSheet" runat="server" Text='<%# Bind("BBMTripSheet") %>'></asp:Label>
                                <asp:Label ID="lblBTripSheetOptions" runat="server" Text='<%# Bind("BTripSheetOptions") %>'></asp:Label>
                                <%--  new--%>
                                <asp:Label ID="lblBMKioskOtherService" runat="server" Text='<%# Bind("BMKioskOtherService") %>'></asp:Label>
                                <%--  new--%>
                                <asp:Label ID="lblBMChangeTripSheet" runat="server" Text='<%# Bind("BBMChangeTripSheet") %>'></asp:Label>
                                <asp:Label ID="lblBMBoatReTripDetails" runat="server" Text='<%# Bind("BBMBoatReTripDetails") %>'></asp:Label>
                                <asp:Label ID="lblBMChangeBoatDetails" runat="server" Text='<%# Bind("BBMChangeBoatDetails") %>'></asp:Label>
                                <asp:Label ID="lblBMCancellation" runat="server" Text='<%# Bind("BBMCancellation") %>'></asp:Label>
                                <asp:Label ID="lblBMReSchedule" runat="server" Text='<%# Bind("BBMReScheduling") %>'></asp:Label>

                                <%--  new--%>
                                <asp:Label ID="lblBGeneratingBoardingPass" runat="server" Text='<%# Bind("BGeneratingBoardingPass") %>'></asp:Label>
                                <asp:Label ID="lblBGenerateManualTicket" runat="server" Text='<%# Bind("BGenerateManualTicket") %>'></asp:Label>
                                <%--  new--%>



                                <asp:Label ID="lblTMMaterialPur" runat="server" Text='<%# Bind("BTMMaterialPurchase") %>'></asp:Label>
                                <asp:Label ID="lblTMMaterialIss" runat="server" Text='<%# Bind("BTMMaterialIssue") %>'></asp:Label>
                                <asp:Label ID="lblTMTripSheetSettle" runat="server" Text='<%# Bind("BTMTripSheetSettle") %>'></asp:Label>
                                <asp:Label ID="lblTMRowerSettle" runat="server" Text='<%# Bind("BTMRowerSettle") %>'></asp:Label>
                                <asp:Label ID="lblTMRefundCounter" runat="server" Text='<%# Bind("BTMRefundCounter") %>'></asp:Label>
                                <asp:Label ID="lblTMFoodStockEntryMaintance" runat="server" Text='<%# Bind("BTMFoodStockEntryMaintance") %>'></asp:Label>
                                <asp:Label ID="lblTMReceiptBalanceRefund" runat="server" Text='<%# Bind("BTMReceiptBalanceRefund") %>'></asp:Label>
                                <asp:Label ID="lblBDepositRefundOptions" runat="server" Text='<%# Bind("BDepositRefundOptions") %>'></asp:Label>
                                <asp:Label ID="lblRMBooking" runat="server" Text='<%# Bind("BRMBooking") %>'></asp:Label>
                                <asp:Label ID="lblRMOtherSvc" runat="server" Text='<%# Bind("BRMOtherService") %>'></asp:Label>1
                                <asp:Label ID="lblRMRestaurantService" runat="server" Text='<%# Bind("BRMRestaurantService") %>'></asp:Label>

                                <%--Newly Added--%>

                                <asp:Label ID="lblRMAdditionalTicket" runat="server" Text='<%# Bind("BRMAdditionalTicket") %>'></asp:Label>
                                <asp:Label ID="lblRMAbstractAdditionalTicket" runat="server" Text='<%# Bind("BRMAbstractAdditionalTicket") %>'></asp:Label>
                                <asp:Label ID="lblRMDepositStatus" runat="server" Text='<%# Bind("BRMDepositStatus") %>'></asp:Label>
                                <asp:Label ID="lblRMDiscountReport" runat="server" Text='<%# Bind("BRMDiscountReport") %>'></asp:Label>
                                <asp:Label ID="lblRMCashinHands" runat="server" Text='<%# Bind("BRMCashinHands") %>'></asp:Label>
                                <asp:Label ID="lblRMExtendedBoatHouse" runat="server" Text='<%# Bind("BRMExtendedBoatHouse") %>'></asp:Label>
                                <asp:Label ID="lblRMPrintBoatBooking" runat="server" Text='<%# Bind("BRMPrintBoatBooking") %>'></asp:Label>
                                <asp:Label ID="lblRMTripWiseDetails" runat="server" Text='<%# Bind("BRMTripWiseDetails") %>'></asp:Label>
                                <asp:Label ID="lblRMReceiptBalance" runat="server" Text='<%# Bind("BRMReceiptBalance") %>'></asp:Label>
                                <%-- <asp:Label ID="lblRMAbstractBooking" runat="server" Text='<%# Bind("BRMAbstractBooking") %>'></asp:Label>--%>
                                <asp:Label ID="lblRMRePrintReport" runat="server" Text='<%# Bind("BRMRePrintReport") %>'></asp:Label>
                                <asp:Label ID="lblRMQRCodeGeneration" runat="server" Text='<%# Bind("BRMQRCodeGeneration") %>'></asp:Label>



                                <%--Newly Added--%>


                                <asp:Label ID="lblRMAbstractBoatBook" runat="server" Text='<%# Bind("BRMAbstractBoatBook") %>'></asp:Label>
                                <asp:Label ID="lblRMAbstractOthSvc" runat="server" Text='<%# Bind("BRMAbstractOthSvc") %>'></asp:Label>
                                <asp:Label ID="lblRMAbstractResSvc" runat="server" Text='<%# Bind("BRMAbstractResSvc") %>'></asp:Label>

                                <asp:Label ID="lblRMTripSheetSettle" runat="server" Text='<%# Bind("BRMTripSheet") %>'></asp:Label>
                                <asp:Label ID="lblRMAvailBoatCapacity" runat="server" Text='<%# Bind("BRMAvailBoatCapacity") %>'></asp:Label>
                                <asp:Label ID="lblRMBoatwiseTrip" runat="server" Text='<%# Bind("BRMBoatwiseTrip") %>'></asp:Label>

                                <asp:Label ID="lblRMRowerCharges" runat="server" Text='<%# Bind("BRMRowerCharges") %>'></asp:Label>
                                <asp:Label ID="lblRMBoatCancellation" runat="server" Text='<%# Bind("BRMBoatCancellation") %>'></asp:Label>
                                <asp:Label ID="lblRMRowerSettle" runat="server" Text='<%# Bind("BRMRowerSettle") %>'></asp:Label>

                                <asp:Label ID="lblRMChallanRegister" runat="server" Text='<%# Bind("BRMChallanRegister") %>'></asp:Label>
                                <%--<asp:Label ID="lblRMAbstractChallanRegister" runat="server" Text='<%# Bind("BRMAbstractChallanRegister") %>'></asp:Label>--%>
                                <asp:Label ID="lblRMServiceWiseCollection" runat="server" Text='<%# Bind("BRMServiceWiseCollection") %>'></asp:Label>

                                <asp:Label ID="lblRMUserBookingReport" runat="server" Text='<%# Bind("BRMUserBookingReport") %>'></asp:Label>
                                <asp:Label ID="lblRMTripWiseCollection" runat="server" Text='<%# Bind("BRMTripWiseCollection") %>'></asp:Label>
                                <asp:Label ID="lblRMBoatTypeRowerList" runat="server" Text='<%# Bind("BRMBoatTypeRowerList") %>'></asp:Label>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                </asp:GridView>
            </div>
            <div>
                <asp:Button ID="Back" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="Back_Click" />
                &nbsp
                        <asp:Button ID="Next" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="Next_Click" />
                <%--&nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />--%>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfstartvalue" runat="server" />
    <asp:HiddenField ID="hfendvalue" runat="server" />

    <script type="text/javascript">
        $(function checkUncheck() {
            $("[id*=chkMenuBooking]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=ckblBooking] input").prop("checked", true);
                }
                else {
                    $("[id*=ckblBooking] input").prop("checked", false);
                }
            });

            $("[id*=ckblBooking] input").bind("click", function () {
                if ($("[id*=ckblBooking] input:checked").length == $("[id*=ckblBooking] input").length) {
                    $("[id*=chkMenuBooking]").prop("checked", true);
                }
                else if ($("[id*=ckblBooking] input:checked").length == 0) {
                    $("[id*=chkMenuBooking]").prop("checked", false);
                }
                else {

                    $("[id*=chkMenuBooking]").prop("checked", true);
                }
            });
        });
    </script>

    <script type="text/javascript">
        $(function checkUncheck() {
            $("[id*=chkMenuBoatingSvc]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=ckblBoatingSvc] input").prop("checked", true);
                }
                else {
                    $("[id*=ckblBoatingSvc] input").prop("checked", false);
                }
            });

            $("[id*=ckblBoatingSvc] input").bind("click", function () {
                if ($("[id*=ckblBoatingSvc] input:checked").length == $("[id*=ckblBoatingSvc] input").length) {
                    $("[id*=chkMenuBoatingSvc]").prop("checked", true);
                }
                else if ($("[id*=ckblBoatingSvc] input:checked").length == 0) {
                    $("[id*=chkMenuBoatingSvc]").prop("checked", false);
                }
                else {

                    $("[id*=chkMenuBoatingSvc]").prop("checked", true);
                }
            });
        });
    </script>

    <script type="text/javascript">
        $(function checkUncheck() {
            $("[id*=chkMenuAdditionalSvc]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=ckblAddlSvc] input").prop("checked", true);
                }
                else {
                    $("[id*=ckblAddlSvc] input").prop("checked", false);
                }
            });

            $("[id*=ckblAddlSvc] input").bind("click", function () {
                if ($("[id*=ckblAddlSvc] input:checked").length == $("[id*=ckblAddlSvc] input").length) {
                    $("[id*=chkMenuAdditionalSvc]").prop("checked", true);
                }
                else if ($("[id*=ckblAddlSvc] input:checked").length == 0) {
                    $("[id*=chkMenuAdditionalSvc]").prop("checked", false);
                }
                else {

                    $("[id*=chkMenuAdditionalSvc]").prop("checked", true);
                }
            });
        });
    </script>

    <script type="text/javascript">
        $(function checkUncheck() {
            $("[id*=chkMenuOtherSvc]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=ckblOtherSvc] input").prop("checked", true);
                }
                else {
                    $("[id*=ckblOtherSvc] input").prop("checked", false);
                }
            });

            $("[id*=ckblOtherSvc] input").bind("click", function () {
                if ($("[id*=ckblOtherSvc] input:checked").length == $("[id*=ckblOtherSvc] input").length) {
                    $("[id*=chkMenuOtherSvc]").prop("checked", true);
                }
                else if ($("[id*=ckblOtherSvc] input:checked").length == 0) {
                    $("[id*=chkMenuOtherSvc]").prop("checked", false);
                }
                else {

                    $("[id*=chkMenuOtherSvc]").prop("checked", true);
                }
            });
        });
    </script>

    <script type="text/javascript">
        $(function checkUncheck() {
            $("[id*=chkMenuTripSheet]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=ckblTripSheet] input").prop("checked", true);
                }
                else {
                    $("[id*=ckblTripSheet] input").prop("checked", false);
                }
            });

            $("[id*=ckblTripSheet] input").bind("click", function () {
                if ($("[id*=ckblTripSheet] input:checked").length == $("[id*=ckblTripSheet] input").length) {
                    $("[id*=chkMenuTripSheet]").prop("checked", true);
                }
                else if ($("[id*=ckblTripSheet] input:checked").length == 0) {
                    $("[id*=chkMenuTripSheet]").prop("checked", false);
                }
                else {

                    $("[id*=chkMenuTripSheet]").prop("checked", true);
                }
            });
        });
    </script>

    <script type="text/javascript">
        $(function checkUncheck() {
            $("[id*=chkMenuTransaction]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=ckblTransaction] input").prop("checked", true);
                    $("[id*=ckblDepositRefund] input").prop("checked", true);
                    $("[id*=chkMenuDepositRefund]").prop("checked", true);
                }
                else {
                    $("[id*=ckblTransaction] input").prop("checked", false);
                    $("[id*=ckblDepositRefund] input").prop("checked", false);
                    $("[id*=chkMenuDepositRefund]").prop("checked", false);
                }
            });

            $("[id*=ckblTransaction] input").bind("click", function () {
                if ($("[id*=ckblTransaction] input:checked").length == $("[id*=ckblTransaction] input").length) {
                    $("[id*=chkMenuTransaction]").prop("checked", true);
                }
                else if ($("[id*=ckblTransaction] input:checked").length == 0) {
                    $("[id*=chkMenuTransaction]").prop("checked", false);
                }
                else {

                    $("[id*=chkMenuTransaction]").prop("checked", true);
                }
            });

        });

    </script>

    <script type="text/javascript">
        $(function checkUncheck() {
            $("[id*=chkMenuDepositRefund]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=ckblDepositRefund] input").prop("checked", true);
                }
                else {
                    $("[id*=ckblDepositRefund] input").prop("checked", false);
                }
            });

            $("[id*=ckblDepositRefund] input").bind("click", function () {
                if ($("[id*=ckblDepositRefund] input:checked").length == $("[id*=ckblDepositRefund] input").length) {
                    $("[id*=chkMenuDepositRefund]").prop("checked", true);
                }
                else if ($("[id*=ckblDepositRefund] input:checked").length == 0) {
                    $("[id*=chkMenuDepositRefund]").prop("checked", false);
                }
                else {

                    $("[id*=chkMenuDepositRefund]").prop("checked", true);
                }
            });
        });
    </script>

    <script type="text/javascript">
        $(function checkUncheck() {
            $("[id*=chkMenuReports]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=ckblReports] input").prop("checked", true);
                }
                else {
                    $("[id*=ckblReports] input").prop("checked", false);
                }
            });

            $("[id*=ckblReports] input").bind("click", function () {
                if ($("[id*=ckblReports] input:checked").length == $("[id*=ckblReports] input").length) {
                    $("[id*=chkMenuReports]").prop("checked", true);
                }
                else if ($("[id*=ckblReports] input:checked").length == 0) {
                    $("[id*=chkMenuReports]").prop("checked", false);
                }
                else {

                    $("[id*=chkMenuReports]").prop("checked", true);
                }
            });
        });
    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

