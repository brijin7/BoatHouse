<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Reports/RptAbstractBoatBooking.aspx.cs" Inherits="Boating_RptAbstractBoatBooking" %>


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
        <h5 class="pghr">Abstract Boat Booking</h5>
        <hr />
        <div>
            <div class="row">
                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-sm-2 col-md-2 col-lg-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                                        <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                                        ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                                        Enter From Date</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-md-2 col-lg-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblToDate" id="Label2" runat="server">
                                        <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="2">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                                        ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                                        Enter To Date</asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="col-md-4 col-lg-4 col-sm-4" style="margin-top: 30px;">
                                <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                    <div runat="server" id="divgridbooking" visible="false" style="overflow: auto; max-height: 300px; max-width: 800px; min-height: 200px; min-width: 50%;">
                        <div class="table-responsive">
                            <asp:GridView ID="GvBoatBookingHdr" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="UserId" PageSize="20" OnPageIndexChanging="GvBoatBookingHdr_PageIndexChanging" ShowFooter="true">
                                <columns>
                                    <asp:TemplateField HeaderText="User Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <itemtemplate>
                                            <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBookingusername" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Count" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:LinkButton ID="lblcount" runat="server" Text='<%# Bind("NoCount") %>' CommandName="Action" OnClick="lblcount_Click"></asp:LinkButton>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Boat/Rower Charge/Tax" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatcharge" runat="server" Text='<%# Bind("BoatCharge") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Boat Deposit" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatDeposit" runat="server" Text='<%# Bind("BoatDeposit") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText=" Net Amount" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBookingTotalAmount" runat="server" Text='<%# Bind("TotalAmount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>
                                </columns>
                                <headerstyle cssclass="gvHead" />
                                <alternatingrowstyle cssclass="gvRow" />
                                <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                                <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                            </asp:GridView>
                            <span style="color: gray">Note: Click Booking Count, View an Boat Booking Details</span>
                        </div>
                    </div>
                </div>
            </div>


            <div runat="server" id="divUBService" style="padding-left: 10px; overflow: auto; max-width: 900px; min-width: 100px; max-height: 400px; min-height: 100px;">
                <div class="table-responsive">
                    <asp:GridView ID="GVUBService" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" PageSize="10" Width="100%"
                        ShowFooter="true" Visible="false">
                        <columns>
                            <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <%#Container.DataItemIndex+1 %>
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
                                    <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("BoatSeater") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblCount" runat="server" Text='<%# Bind("Count") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Boat/Rower Charge/Tax" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatCharge" runat="server" Text='<%# Bind("BoatCharge") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Boat Deposit" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatDeposit" runat="server" Text='<%# Bind("BoatDeposit") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                        </columns>
                        <headerstyle cssclass="gvHead" />
                        <alternatingrowstyle cssclass="gvRow" />
                        <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                        <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                    </asp:GridView>
                </div>
            </div>
            <br />

            <div id="divBoatGrid" runat="server" visible="false" style="padding-left: 10px; padding-right: 10px;">
                <div class="table-responsive">
                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                        <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
                    <asp:GridView ID="GvBoatBooking" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" ShowFooter="true" DataKeyNames="BookingId" PageSize="15" OnPageIndexChanging="GvBoatBooking_PageIndexChanging">
                        <columns>
                            <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
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
                                    <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblSeaterType" runat="server" Text='<%#Eval("SeaterType")%>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Count" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblNoCount" runat="server" Text='<%# Bind("NoCount") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat/Rower Charge/Tax" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatCharge" runat="server" Text='<%# Bind("BoatCharge") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Boat Deposit" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatDeposit" runat="server" Text='<%# Bind("BoatDeposit") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Bind("TotalAmount") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>
                        </columns>
                        <headerstyle cssclass="gvHead" />
                        <alternatingrowstyle cssclass="gvRow" />
                        <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                        <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfuserid" runat="server" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>
