<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="AbstractTripSheetReport.aspx.cs" Inherits="Boating_AbstractTripSheetReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .card {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            border-radius: 20px;
            min-width: 35%;
            max-width: 50%;
        }

            .card:hover {
                box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
            }
    </style>
    <div class="form-body">
        <h5 class="pghr">Cash In Hand </h5>
        <hr />
        <br />

        <div class="col-md-12 col-sm-12" runat="server" id="divResDateWise">
            <div class="row">

                <div class="col-sm-2 col-xs-12" runat="server">
                    <label for="lblTypes" id="lblResFromdate"><i class="fa fa-calendar" aria-hidden="true"></i>Date <span class="spStar">*</span></label>
                    <asp:TextBox ID="txtFromDate" CssClass="form-control frmDate" runat="server" TabIndex="1" AutoComplete="Off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate"
                        ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select Booking Date</asp:RequiredFieldValidator>
                </div>

                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px; padding-right: 10px;">
                    <span style="float: left">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" class="btn btn-primary" ValidationGroup="Search" OnClick="btnGenerate_Click" TabIndex="2" />
                    </span>

                    <span style="float: left; padding-left: 30px;">
                        <asp:ImageButton ID="btnExportToExcel" runat="server" ImageUrl="~/images/Excel.png" OnClick="ExportToExcel_Click" Width="40px" TabIndex="3" Visible="false" />
                    </span>
                </div>

                <div class="col-md-8 col-sm-8" style="margin-top: 10px; margin-bottom: 10px; padding-right: 10px; padding-left: 20px" runat="server" id="divCash" visible="false">

                    <div class="card">
                        <h3 style="text-align: center; color: #124a79">CASH IN HAND </h3>
                        <hr />

                        <div class="col-md-12 col-sm-12" style="margin-top: 5px">
                            <div class="row ">

                                <div class="col-sm-5 col-xs-12">
                                    <strong>Collected Balance</strong>
                                </div>
                                <div class="col-sm-1 col-xs-12">: </div>
                                <div class="col-sm-6 col-xs-12" style="padding-left: 40px">
                                    <asp:Label runat="server" ID="lblCollectedBalance" Font-Bold="true" Font-Size="Medium" ForeColor="#333399"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <hr />

                        <div class="col-md-12 col-sm-12" style="margin-top: 5px">
                            <div class="row">
                                <div class="col-sm-5 col-xs-12" runat="server">
                                    <strong>Refunded Balance</strong>
                                </div>
                                <div class="col-sm-1 col-xs-12" runat="server">: </div>
                                <div class="col-sm-6 col-xs-12" runat="server" style="padding-left: 40px">
                                    <asp:Label runat="server" ID="lblRefundedBalance" Font-Bold="true" Font-Size="Medium" ForeColor="#333399"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <hr />

                        <div class="col-md-12 col-sm-12" style="margin-top: 5px">
                            <div class="row">
                                <div class="col-sm-5 col-xs-12">
                                    <strong>Balance</strong>
                                </div>
                                <div class="col-sm-1 col-xs-12">: </div>
                                <div class="col-sm-6 col-xs-12" style="padding-left: 40px">
                                    <asp:Label runat="server" ID="lblBalance" Font-Bold="true" Font-Size="Medium" ForeColor="#333399"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <hr />

                        <div class="col-md-12 col-sm-12" style="margin-top: 5px">
                            <div class="row">
                                <div class="col-sm-5 col-xs-12">
                                    <strong>Ticket Balance</strong>
                                </div>
                                <div class="col-sm-1 col-xs-12">: </div>
                                <div class="col-sm-6 col-xs-12" style="padding-left: 40px">
                                    <asp:Label runat="server" ID="lblTicketBalance" Font-Bold="true" Font-Size="Medium" ForeColor="#333399"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <hr />
                        <div class="col-md-12 col-sm-12" style="margin-top: 5px">
                            <div class="row">
                                <div class="col-sm-5 col-xs-12">
                                    <strong>Rower Settlement</strong>
                                </div>
                                <div class="col-sm-1 col-xs-12">: </div>
                                <div class="col-sm-6 col-xs-12" style="padding-left: 40px">
                                    <asp:Label runat="server" ID="lblRowerSettlement" Font-Bold="true" Font-Size="Large" ForeColor="#333399"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <hr />


                        <div class="col-md-12 col-sm-12" style="margin-top: 5px">
                            <div class="row">
                                <div class="col-sm-5 col-xs-12">
                                    <strong>Cash In Hand</strong>
                                </div>
                                <div class="col-sm-1 col-xs-12">: </div>
                                <div class="col-sm-6 col-xs-12" style="padding-left: 40px">
                                    <asp:Label runat="server" ID="lblFinalCashInHand" Font-Bold="true" Font-Size="Large" ForeColor="#d31642"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div class="table-responsive">
            <div id="divAvailableBoats" style="overflow: auto; display: block; min-width: 100%; max-width: 100%; min-height: 200px; max-height: 500px">

                <asp:GridView ID="gvAbstractReport" runat="server" CssClass="table table-bordered table-condenced CustomGrid"
                    AutoGenerateColumns="false" AllowPaging="true" Width="155%"
                    OnRowCreated="gvAbstractReport_RowCreated" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Nos" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingNos" runat="server" Text='<%# Bind("BookedCount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Charges" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingBoatCharges" runat="server" Text='<%# Bind("BookedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDeposit" runat="server" Text='<%# Bind("BookedDeposit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingTotal" runat="server" Text='<%# Bind("BookedTotal") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Nos" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripNos" runat="server" Text='<%# Bind("BNotStartedCount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Charges" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripCharges" runat="server" Text='<%# Bind("BNotStartedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripDeposit" runat="server" Text='<%# Bind("BNotStartedDeposit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripTotal" runat="server" Text='<%# Bind("BNotStartedTotal") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Nos" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripStartedNos" runat="server" Text='<%# Bind("BStartedCount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Charges" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripStartedBoatCharges" runat="server" Text='<%# Bind("BStartedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripStartedDeposit" runat="server" Text='<%# Bind("BStartedDeposit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripStartedTotal" runat="server" Text='<%# Bind("BStartedTotal") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Nos" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOverallNos" runat="server" Text='<%# Bind("CompletedCount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Charges" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOverallBoatCharges" runat="server" Text='<%# Bind("CompletedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Nos" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDepositBreakNos" runat="server" Text='<%# Bind("TimeExtnCount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Time Extn - Amt" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDepositBreakTime" runat="server" Text='<%# Bind("TimeExtnAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Nos" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDepositBreakTimeNos" runat="server" Text='<%# Bind("DepositRefundCount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Refunded Amt" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRefundedAmt" runat="server" Text='<%# Bind("DepositRefundAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Nos" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRefundedAmtNos" runat="server" Text='<%# Bind("UnClaimedRefundCount"  ) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Not Claimed - Amt" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNotClaimedAmt" runat="server" Text='<%# Bind("UnClaimedRefundAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripCompletedTotal" runat="server" Text='<%# Bind("TotalDeposit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCashInHand" runat="server" Text='<%# Bind("CashInHand") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>


