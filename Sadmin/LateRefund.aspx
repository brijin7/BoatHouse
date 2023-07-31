<%@ Page Title="Previous Day Refund" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="LateRefund.aspx.cs" Inherits="Sadmin_LateRefund" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="form-body">
        <h5 class="pghr">Previous Day Refund</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control" AutoComplete="Off" TabIndex="1"
                            Enabled="false">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Select Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="boat">
                            <i class="fa fa-ship" aria-hidden="true"></i>Boat House Name
                        <span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlBoatHouseId" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBoatHouseId"
                            ValidationGroup="Search" SetFocusOnError="True" InitialValue="Select Boat House" CssClass="vError">
                            Select Boat House</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Booking Pin No.<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtPin" runat="server" CssClass="form-control " AutoComplete="Off" TabIndex="3">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfgPin" runat="server" ControlToValidate="txtPin"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter Booking Pin No.</asp:RequiredFieldValidator>
                    </div>
                </div>
                <%-- <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" id="Complaint" runat="server" visible="false">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-list" aria-hidden="true"></i>Complaint No.<span class="spStar">*</span></label>
                        
                    </div>
                </div>--%>
                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="4" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="5" OnClick="btnReset_Click" />
                </div>
            </div>
        </div>

        <div runat="server" id="divGridList" visible="false">
            <div class="table-responsive">

                <asp:GridView ID="GvBoatBookingTrip" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="25000">
                    <Columns>

                        <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="110px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPinNum" runat="server" Text='<%# Bind("BookingPin") %>' Width="110px"
                                    Font-Bold="true" Font-Size="Large" Enabled="false" ForeColor="#0072ff"></asp:LinkButton>

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


                        <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActualBoatId" runat="server" Text='<%# Bind("BoatId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblActualBoatNum" runat="server" Text='<%# Bind("BoatNum") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rower" Visible="false">
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
                                <%-- <asp:Label ID="lblRefundPrint" runat="server" Text='<%# Bind("RStatus") %>'></asp:Label>--%>
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
                        <asp:TemplateField HeaderText="Complaint No" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtComplaintNo" runat="server" CssClass="form-control " AutoComplete="Off" TabIndex="2">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtComplaintNo"
                                    ValidationGroup="SMS" SetFocusOnError="True" CssClass="vError">
                            Enter Complaint No.</asp:RequiredFieldValidator>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SMS" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnSMS" ForeColor="#512c88" CausesValidation="true" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/esms.svg" OnClick="ImgBtnSMS_Click" ToolTip="SMS"
                                    Visible="true" Style="outline: none" ValidationGroup="SMS" />
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
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

