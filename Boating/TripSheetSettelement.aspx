<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="TripSheetSettelement.aspx.cs" Inherits="Boating_TripSheetSettelementV2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <style>
        .BarCodeTextStart {
            width: 500px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
        }
    </style>

    <script type="text/javascript">
        function StartBox() {
            var seconds = 2;
            setTimeout(function () {
                setTimeout(HideModalPopup, 1000);
                var txtStart = document.getElementById('<%=txtSearchPin.ClientID %>');
                txtStart.focus();
            }, seconds * 1000);
        };
        function HideModalPopup() {
            $find("MpeTrip").hide();
            return false;
        }
    </script>


    <div class="form-body col-sm-12 col-xs-12" onclick="StartBox()" onload="StartBox()">

        <h5 class="pghr">Deposit Details<span style="float: right;">
            <asp:LinkButton ID="lbtnLateRefund" CssClass="lbtnNew" runat="server" OnClick="lbtnLateRefund_Click" Visible="false">
                 <i class="fas fa-list"></i>Previous Day Refund</asp:LinkButton>
            <asp:LinkButton ID="lbtnList" CssClass="lbtnNew" runat="server" OnClick="lbtnList_Click">
                 <i class="fas fa-list"></i>Deposit Settled List</asp:LinkButton>

            <asp:LinkButton ID="lbtnNewRefund" CssClass="lbtnNew" runat="server" OnClick="lbtnNewRefund_Click" Visible="false">
                 <i class="fas fa-list"></i>Deposit Refund</asp:LinkButton>


        </span>

        </h5>

        <%-- <asp:ImageButton ID="imgbtnNewBook" runat="server" ImageUrl="~/images/Ticket.svg" Width="40px" ToolTip="New Booking" CssClass="pr-2"  />--%>
        <hr />
        <div class="col-xs-12" style="background-color: cornsilk">
            <div class="row">
                <div class="col-md-4 col-sm-4">
                    <b style="font-size: 18px">Received Amount :</b>
                    <asp:Label ID="bblblReceivedAmt" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"></asp:Label>

                </div>

                <div class="col-md-4 col-sm-4">
                    <b style="font-size: 18px">Refund Amount :</b>
                    <asp:Label ID="bblblRefundAmt" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"></asp:Label>
                </div>

                <div class="col-md-4 col-sm-4">
                    <b style="font-size: 18px">Cash In Hand :</b>
                    <asp:Label ID="bblblCashInHand" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"></asp:Label>
                </div>

            </div>

        </div>

        <div runat="server">

            <div class="table-responsive" runat="server" id="divalert" visible="false" style="height: 100%">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblalert" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
            </div>

            <div class="mydivbrdr" runat="server" id="divRefund">
                <div class="row">

                    <div class="col-sm-12 col-xs-12" id="divScanQR" runat="server" visible="false" style="text-align: left; margin-top: 5px; margin-bottom: 3px;">
                        <asp:TextBox ID="txtSearchPin" runat="server" placeholder="Scan QRCode" CssClass="BarCodeTextStart" AutoComplete="off"
                            Font-Bold="true" Font-Size="X-Large" AutoPostBack="true" OnTextChanged="txtSearchPin_TextChanged"></asp:TextBox>
                        &nbsp&nbsp&nbsp
                        <asp:Label ID="lblSearchPinResponse" runat="server" Font-Bold="true" ForeColor="#003399" Font-Size="X-Large"></asp:Label>
                        <asp:TextBox ID="txtBookingIdPin" runat="server" placeholder="Enter Booking Id / Pin" AutoComplete="off" Visible="false"
                            Style="width: 250px; height: 40px; border-color: skyblue; border-radius: 8px; float: right;"
                            Font-Bold="true" Font-Size="X-Large" AutoPostBack="true" OnTextChanged="txtBookingIdPin_TextChanged"></asp:TextBox>

                    </div>

                    <div class="col-sm-12 col-xs-12" runat="server" style="height: 600px; overflow-y: auto; float: left; position: relative; margin-top: 3px;">
                        <div class="table-responsive">

                            <asp:GridView ID="GvBoatBookingTrip" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%-- <%#Container.DataItemIndex+1 %>--%>
                                            <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
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
                                            <asp:Label ID="lblRefundPrint" runat="server" Text='<%# Bind("RStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPreStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                                            <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>' Visible="false"></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <%--                                        <asp:TemplateField HeaderText="SMS" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtnSMS" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                                    runat="server" Font-Bold="true" ImageUrl="~/images/esms.svg" OnClick="ImgBtnSMS_Click" ToolTip="SMS"
                                                    Visible='<%# Eval("CustomerMobile").ToString() == ""? false: true %>' Style="outline: none" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="SMS" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgBtnSMS" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                                runat="server" Font-Bold="true" ImageUrl="~/images/esms.svg" OnClick="ImgBtnSMS_Click" ToolTip="SMS"
                                                Visible="true" Style="outline: none" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Print" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgBtnPrint" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                                runat="server" Font-Bold="true" ImageUrl="~/images/Print.svg" OnClick="ImgBtnPrint_Click" ToolTip="Print"
                                                Visible="true" Style="outline: none" />
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
                        <div id="divRefundPreNext" runat="server">
                            <asp:Button ID="Refundback" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="Refundback_Click" />
                            &nbsp
              <asp:Button ID="RefundNext" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="RefundNext_Click" />
                            &nbsp
                        </div>
                    </div>
                </div>
            </div>


            <div class="table-div" id="divGridSettledList" runat="server" visible="false">

                <div class="table-responsive">
                    <%-- New--%>
                    <%--<div class="col-md-12 col-lg-12 col-sm-12">
                        <div style="float: right">
                            <asp:Button runat="server" Text="Export To PDF" ID="btnPdf" OnClick="btnPdf_Click" class="btn btn-primary" />
                        </div>
                    </div>--%>
                    <%-- New--%>

                    <asp:GridView ID="gvTripSheetSettelement" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" DataKeyNames="BoatReferenceNo">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <%--  <%#Container.DataItemIndex+1 %>--%>
                                    <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:TemplateField>

                            <%-- <asp:TemplateField HeaderText="BookingDate" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="BookingId" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat RefNo." HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="BookingPin" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>

                                    <asp:Label ID="lblActualBoatNum" runat="server" Text='<%# Bind("BoatNum") %>'></asp:Label>

                                    <asp:Label ID="lblBoatName" runat="server" Text='<%# Bind("BoatName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Rower">
                                <ItemTemplate>

                                    <asp:Label ID="lblRowerNames" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actual Trip Start Time" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actual Trip End Time">
                                <ItemTemplate>
                                    <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Bind("TripEndTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Travelled Minutes" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblTravelDifference" runat="server" Text='<%# Bind("TravelDifference") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Net Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblActualNetAmount" runat="server" Text='<%# Bind("ActualNetAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Deposit Refund">
                                <ItemTemplate>
                                    <asp:Label ID="lblDepRefundAmount" runat="server" Text='<%# Bind("DepRefundAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Deposit Refund Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblDepRefuDate" runat="server" Text='<%# Bind("DepRefundDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="RePayment Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblRePaymentType" runat="server" Text='<%# Bind("RePaymentType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gvHead" />
                        <AlternatingRowStyle CssClass="gvRow" />
                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                        <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                    </asp:GridView>

                </div>
                <div id="divSettledPreNext" runat="server">
                    <asp:Button ID="SettledBack" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="SettledBack_Click" />
                    &nbsp
              <asp:Button ID="SettledNext" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="SettledNext_Click" />
                    &nbsp
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
                                        TabIndex="3" CausesValidation="true" OnClick="btnSubmit_Click" Width="170" Font-Bold="true" />
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



    <div class="modal show" id="myModalMob" style="width: 87%; padding-right: 756px !important; float: left;">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header" style="background-color: #004c8c; color: white; width: 100%">
                    <h5 class="modal-title">Customer Mobile Number</h5>
                    <asp:ImageButton ID="ImgClose" runat="server" OnClick="ImgClose_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" CausesValidation="false" />
                </div>

                <!-- Modal body -->
                <div class="modal-body" style="width: 91%; background-color: white">
                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                            <div class="row" style="padding: 3px">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" style="margin-right: 0px;">
                                        <b>
                                            <asp:Label ID="lblCusmobGrid" runat="server">Customer Mobile </asp:Label></b>
                                    </label>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right">
                                    <asp:TextBox ID="txtGridMobileNum" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" MaxLength="10" Font-Bold="true" Font-Size="Larger"
                                        AutoComplete="off"> </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtGridMobileNum"
                                        ValidationGroup="UpdateMob" SetFocusOnError="True" CssClass="vError">Enter Customer Mobile No</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="vError"
                                        ControlToValidate="txtGridMobileNum" ErrorMessage="RegularExpressionValidator"
                                        ValidationExpression="[0-9]{10}" ValidationGroup="UpdateMob">Enter 10 Digit Number</asp:RegularExpressionValidator>
                                    <%-- <asp:Label ID="lblinfomsg" runat="server" Text="Enter 10 Digit Number "  ForeColor="Red" Visible="false"></asp:Label>--%>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                            <div class="row" style="padding: 3px">

                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12" style="text-align: right">
                                    <asp:Button ID="btnUpdate" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="UpdateMob"
                                        TabIndex="3" CausesValidation="true" OnClick="btnUpdate_Click" Width="124" Style="margin-right: 135px;" Font-Bold="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal footer -->
                <%-- <div class="modal-footer">
                    <asp:Button ID="btnClose" runat="server" Text="Close" TabIndex="37"
                        CssClass="btn btn-danger" />
                </div>--%>
            </div>
        </div>
    </div>


    <ajax:DragPanelExtender ID="DragPanelExtender3" runat="server" TargetControlID="pnlTrip1" DragHandleID="pnlDrag"></ajax:DragPanelExtender>
    <ajax:ModalPopupExtender ID="MpeTrip1" runat="server" BehaviorID="MpeTrip1" TargetControlID="HiddenField1" PopupControlID="pnlTrip1"
        BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>

    <asp:Panel ID="pnlTrip1" runat="server" CssClass="Msgg">
        <asp:Panel ID="PnlDrag" runat="server" CssClass="drag">
            <div class="modal-content" style="width: 500px">
                <div class="modal-header">
                    <asp:Label ID="lblEnd" runat="server" Font-Bold="true" ForeColor="Blue" Font-Size="20px"></asp:Label>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblAlertMsg" runat="server" Font-Bold="true"
                        Text="Confirm(Are you sure to End this Trip?)" Font-Size="X-Large"></asp:Label>

                    <label for="lblTripBooking" style="font-size: large; color: black">
                        Booking Id <span style="padding-left: 4.8rem;">:</span>
                    </label>
                    <asp:Label ID="lblPopBookingId" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Red"></asp:Label>
                    <br />
                    <label for="lblTripBookingpin" style="font-size: large; color: black">
                        Booking Pin <span style="padding-left: 4.2rem;">:</span>
                    </label>
                    <asp:Label ID="lblPopBookingPin" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Red"></asp:Label>
                    <br />
                    <label for="lblTripStart" style="font-size: large; color: black">
                        Trip Start Duration <span style="padding-left: 1rem;">:</span>
                    </label>
                    <asp:Label ID="lblStartTime" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Red"></asp:Label>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnPopUpOkay" runat="server" Text="Okay" CssClass="btn btn-primary" OnClick="btnPopUpOkay_Click" />
                    <asp:Button ID="btnPopUpCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnPopUpCancel_Click" />
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

