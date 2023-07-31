<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="DepositLateRefund.aspx.cs" Inherits="Boating_TripSheetSettelementV2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">
        function StartBox() {
            var seconds = 2;
            setTimeout(function () {
                setTimeout(HideModalPopup, 1000);            
            }, seconds * 1000);
        };
        function HideModalPopup() {
            $find("MpeTrip").hide();
            return false;
        }
    </script>
    <style>
        .BarCodeTextStart {
            width: 500px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
        }
    </style>
    <div class="form-body col-sm-12 col-xs-12" >

        <h5 class="pghr">Previous Day Deposit Refund<span style="float: right;">

            <asp:LinkButton ID="lbtnNewRefund" CssClass="lbtnNew" runat="server" OnClick="lbtnNewRefund_Click" Visible="true">
                 <i class="fas fa-list"></i> Deposit Refund</asp:LinkButton>
        </span>

        </h5>

        <hr />

        <div runat="server">

            <div class="table-responsive" runat="server" id="divalert" visible="false" style="height: 100%">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblalert" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
            </div>

            <div class="mydivbrdr" runat="server" id="divRefund">
                <div class="row">
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" id="Complaint" runat="server">
                        <div class="form-group">
                            <label for="boat"><i class="fa fa-list" aria-hidden="true"></i>Reference No.<span class="spStar">*</span></label>
                            <asp:TextBox ID="txtReferenceNo" runat="server" CssClass="form-control " AutoComplete="Off" TabIndex="1">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtReferenceNo"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter Reference No.</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="2" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="3" OnClick="btnReset_Click" />
                    </div>
                    <div class="col-sm-12 col-xs-12" runat="server" style="height: 600px; overflow-y: auto; float: left; position: relative; margin-top: 3px;">
                        <div class="table-responsive">

                            <asp:GridView ID="GvBoatBookingTrip" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPinNum" runat="server" Text='<%# Bind("BookingPin") %>' Width="110px" OnClick="lnkPinNum_Click" Font-Bold="true" Font-Size="X-Large"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Duration" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Bind("BookingDuration") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Ref No." HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false" Width="110px"></asp:Label>
                                            <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatType") %>' Width="110px"></asp:Label>
                                            -
                                            <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false" Width="110px"></asp:Label>
                                            <asp:Label ID="lblBoatSeaterName" runat="server" Text='<%# Bind("BoatSeaterName") %>' Width="110px"></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActualBoatId" runat="server" Text='<%# Bind("BoatId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblActualBoatNum" runat="server" Text='<%# Bind("BoatNum") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Rower">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowerId" runat="server" Text='<%# Bind("RowerId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblRowerNames" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Actual Trip Start Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Actual Trip End Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Bind("TripEndTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Travelled Minutes" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTravelDuration" runat="server" Text='<%# Bind("TravelDuration") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Travelled Minutes" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTravelDifference" runat="server" Text='<%# Bind("TravelDifference") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Init Net Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInitNetAmount" runat="server" Text='<%# Bind("InitNetAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Deposit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeposit" runat="server" Text='<%# Bind("Deposit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Refund Duration" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRefundDuration" runat="server" Text='<%# Bind("RefundDuration") %>'></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPreStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                                            <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>' Visible="false"></asp:Label>

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
            </div>

            <div class="row">
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <ajax:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="pnlBillService" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
                <ajax:ModalPopupExtender ID="MpeBillService" runat="server" BehaviorID="MpeBillService" TargetControlID="HiddenField1" PopupControlID="pnlBillService"
                    BackgroundCssClass="modalBackground">
                </ajax:ModalPopupExtender>

                <asp:Panel ID="pnlBillService" runat="server" CssClass="Msg" Style="display: none;">
                    <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
                    </asp:Panel>
                </asp:Panel>
            </div>
        </div>
    </div>

    <!-- Bootstrap -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.min.js"
        integrity="sha384-+sLIOodYLS7CIrQpBjl+C7nPvqq+FbNUBDunl/OZv93DB7Ln/533i8e/mZXLi/P+" crossorigin="anonymous"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js"
        integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
    <div class="modal show" id="myModal" style="width: 87%; padding-right: 756px !important; float: left;">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header" style="background-color: #004c8c; color: white; width: 152%">
                    <h5 class="modal-title">Trip Sheet Settlement Details</h5>
                    <button type="button" class="close" data-dismiss="modal" style="color: white">&times;</button>
                </div>

                <!-- Modal body -->
                <div class="modal-body" style="width: 152%; background-color: white">


                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Booking Date
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                    <asp:Label ID="lblBookingDateDisp" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Booking ID
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:Label ID="lblBookingIdDisp" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Actual Duration
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                    <asp:Label ID="lblTotaltime" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Minutes Taken
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:Label ID="lblTimeDiff" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Extra Minutes
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                    <asp:Label ID="lblExtraTime" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Time Extension
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:Label ID="lblTimeExtension" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Bill Amount
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>

                                <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                    ₹
                                                            <asp:Label ID="lblInitialBillAmountDisp" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Boat Deposit
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    ₹
                                                          <asp:Label ID="lblInitDepositDisp" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                <asp:Label ID="Label9" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        Extra Charge
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    ₹
                                                    <asp:Label ID="lblTotalDeduction" runat="server" Font-Bold="true"></asp:Label>

                                    <asp:Label ID="lblBoatDeduction" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                                    <asp:Label ID="lblRowerdeduction" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                                    <asp:Label ID="lblTaxDeduction" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        <b>
                                            <asp:Label ID="Label2" runat="server">Deposit Refund </asp:Label></b>
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Larger">  ₹</asp:Label>
                                    <asp:Label ID="lblRefundAmt" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Larger"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        <b>
                                            <asp:Label ID="Label5" runat="server">Balance Refund </asp:Label></b>
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:Label ID="Label6" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Larger">  ₹</asp:Label>
                                    <asp:Label ID="lblBalanceAmount" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Larger"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge">
                                        <b>
                                            <asp:Label ID="Label7" runat="server">Total Refund </asp:Label></b>
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:Label ID="Label8" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Larger">  ₹</asp:Label>
                                    <asp:Label ID="lblNetRefund" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Larger"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" style="margin-right: 0px;">
                                        <b>
                                            <asp:Label ID="Label4" runat="server">Customer Mobile </asp:Label></b>
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:TextBox ID="txtCustomerMobile" runat="server" CssClass="form-control" MaxLength="10" Font-Bold="true" Font-Size="Larger"
                                        AutoComplete="off" onkeypress="return isNumber(event)"> </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="vError"
                                        ControlToValidate="txtCustomerMobile" ErrorMessage="RegularExpressionValidator"
                                        ValidationExpression="[0-9]{10}" ValidationGroup="RowerSettlement">Enter 10 Digit Number</asp:RegularExpressionValidator>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCustomerMobile"
                                        ValidationGroup="RowerSettlement" SetFocusOnError="True" CssClass="vError">Enter Customer Mobile No</asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" style="margin-right: 0px;">
                                        <b>
                                            <asp:Label ID="Label3" runat="server">Refund By <span class="spStar">*</span></asp:Label></b>
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlUserName"
                                        InitialValue="Select User"
                                        ValidationGroup="RowerSettlement" SetFocusOnError="True" CssClass="vError">Select Refund By</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>


                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                        </div>
                        <div class="col-sm-7 col-md-7 col-lg-6 col-xs-6" runat="server" id="divrepaymentType" visible="false">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" style="margin-right: 0px; font-size: -0.1rem;">
                                        <b>
                                            <asp:Label ID="lblRepaymentType" runat="server">RePayment Type</asp:Label></b>
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :                                   
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                            <div class="row" style="padding: 3px">

                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12" style="text-align: right">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Refund" class="btn btn-primary" ValidationGroup="RowerSettlement"
                                        TabIndex="3"  OnClick="btnSubmit_Click" Width="170" Font-Bold="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                    <label for="lblmaxcharge">
                                        <b>
                                            <asp:Label ID="lblAlertSlabMsg" runat="server" Font-Bold="true" Font-Size="Larger"></asp:Label></b>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--</div>--%>
                </div>

                <!-- Modal footer -->
                <%-- <div class="modal-footer">
                    <asp:Button ID="btnClose" runat="server" Text="Close" TabIndex="37"
                        CssClass="btn btn-danger" />
                </div>--%>
            </div>
        </div>
    </div>

    <ajax:DragPanelExtender ID="DragPanelExtender1" runat="server" TargetControlID="pnlTrip" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
    <ajax:ModalPopupExtender ID="MpeTrip" runat="server" BehaviorID="MpeTrip" TargetControlID="HiddenField1" PopupControlID="pnlTrip"
        BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>

    <asp:Panel ID="pnlTrip" runat="server" CssClass="Msgg">
        <asp:Panel ID="Panel1" runat="server" CssClass="drag">
            <div class="modal-content" style="width: 400px; height: 250px;">
                <div class="modal-body" style="text-align: center;">
                    <asp:Label ID="lblStart" runat="server" Font-Bold="true" ForeColor="DarkBlue" Font-Size="20px"></asp:Label>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>





    <script>
        $(function () {
            $("#lblSettlement").click(function () {
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


    <script>
        function showModalMob() {
            $("#myModalMob").modal('show');
        }


        function hideModalMob() {
            $("#myModalMob").modal('hide');
        }

    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

