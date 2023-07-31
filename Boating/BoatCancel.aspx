<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Boating/BoatCancel.aspx.cs" Inherits="BoatCancel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .panel > .panel-heading {
            background-color: #006400;
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

    <script language="javascript" type="text/javascript">

        function Selectallcheckbox(val) {
            if (!$(this).is(':checked')) {
                $('input:checkbox').prop('checked', val.checked);
            } else {
                $("#chkroot").removeAttr('checked');
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
                    <div id="divBookingId" runat="server" class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                Booking Id
                            </label>
                            <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control" OnTextChanged="txtBookingId_TextChanged" AutoPostBack="true" AutoComplete="Off" TabIndex="4" MaxLength="10" onkeypress="return isNumberKey(event)">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBookingId"
                                ValidationGroup="Cancellation" SetFocusOnError="True" CssClass="vError">Enter Booking Id</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="form-submit col-sm-3" style="padding-top: 30px; margin-right: inherit; margin-left: inherit">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" Width="100px" Font-Bold="True"
                            Style="font-size: 20px;"
                            TabIndex="4" OnClick="btnCancel_Click" />
                    </div>
                </div>

                <div class="row">
                    <div id="exittimedetails" runat="server" visible="false" class="col-xs-12 col-sm-12 col-lg-12 col-md-12" style="width: 80%;">
                        <div class="row" style="margin-top: 1%;">
                            <div class="col-sm-12">
                                <div class="table-responsive" style="overflow-x: auto; width: 98%; max-height: 250px;">
                                    <asp:GridView ID="gvBookingDetails" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                        AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="10" OnPageIndexChanging="gvBookingDetails_PageIndexChanging">
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
                                                    <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
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
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div class="panel panel-success">
                                    <div class="panel-heading"><i class="fa fa-popover-header" aria-hidden="true" style="color: darkgreen;"></i>Booking Cancel Trip Details </div>
                                    <asp:GridView ID="gvNoteDetails" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                        AutoGenerateColumns="False" PageSize="25000" DataKeyNames="BookingId,BookingPin,BoatReferenceNo,BoatTypeId,BoatType,BoatSeaterId,
                                        SeaterType,BookingDate,PaymentType,TripStatus,PremiumStatus,Status">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Trip Status" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBTripStatus" runat="server" Text='<%# Bind("TripStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cancel" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="ImgBtnCancel" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Cancel"
                                                        runat="server" Font-Bold="true" OnClick="ImgBtnCancel_Click"
                                                        OnClientClick="return confirm('Are you sure you want to Inactive this record?');" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="25%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
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
                                <asp:Label ID="lblPaymentType" runat="server" Text='<%# Bind("PaymentType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
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

