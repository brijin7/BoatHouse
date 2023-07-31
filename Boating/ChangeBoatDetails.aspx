<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="ChangeBoatDetails.aspx.cs" Inherits="Boating_ChangeBoatDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <style>
        .p-2 {
            padding: 0.2rem !important;
        }

        .vl {
            border-left: 1px solid lightgray;
            height: 400px;
        }

        .payMode {
            height: 30px;
            width: 127px;
            display: inline-block;
            font-weight: 400;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            /*border: 1px solid transparent;
            padding: 0.4rem .5rem;*/
            font-size: 1.5rem !important;
            line-height: 1.5;
            border-radius: 2.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            outline: none;
            padding-left: 3px;
            margin-top: -10px;
        }
    </style>
    <div class="form-body" style="margin: 10px -10px;">
        <div id="divEntry1" runat="server">

            <h5 class="pghr">Change Boat Details <span style="float: right;">
                <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton>
                <asp:LinkButton ID="lbtnView" CssClass="lbtnViewLog" runat="server" OnClick="lbtnViewLog_Click"> 
                <i class="fas fa-receipt"></i> View Log Details</asp:LinkButton>
                <asp:LinkButton ID="lbtnGrid" CssClass="lbtnViewLog" runat="server" OnClick="lbtnGrid_Click" Visible="false"> 
                <i class="fas fa-list"></i> View Boat Details</asp:LinkButton></span> </h5>

            <hr />
            <div class="mydivbrdr" id="divEntry" runat="server">
                <div class="row p-2">
                    <div class="col-sm-4 col-xs-12" runat="server">
                        <div class="col-sm-8 col-md-8 col-lg-8 col-xs-12">
                            <div class="form-group">
                                <label for="lblbooking">
                                    <i class="fas fa-address-book"></i>
                                    Booking Id
                                </label>
                                <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control" AutoComplete="Off" OnTextChanged="txtBookingId_TextChanged"
                                    AutoPostBack="true" TabIndex="1" MaxLength="50">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBookingId"
                                    ValidationGroup="BookingRe-trip" SetFocusOnError="True" CssClass="vError">Enter Booking Id</asp:RequiredFieldValidator>
                            </div>

                        </div>


                        <div id="CustomDetails" runat="server" visible="false">
                            <label style="padding-left: 1rem;">Customer MobileNo <span style="padding-left: 1rem;">:</span></label>
                            <asp:Label ID="lblMblNo" runat="server"></asp:Label><br />
                            <label style="padding-left: 1rem;">Booking Date <span style="padding-left: 3.7rem;">:</span></label>
                            <asp:Label ID="lblBookingdate" runat="server"></asp:Label><br />
                            <label style="padding-left: 1rem;">Boat Status <span style="padding-left: 4.4rem;">:</span></label>
                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        </div>
                        <div class="col-sm-3 col-xs-12" runat="server">
                        </div>

                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px; padding-right: 10px;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div id="DivChangeBoatdtl" runat="server" visible="false" style="margin-top: 15px; overflow-y: scroll; max-height: 300px; min-height: 200px; overflow-x: hidden; margin-bottom: 7px;">
                            <div class="table-responsive">

                                <asp:GridView ID="gvChangeBoat" runat="server" AllowPaging="True"
                                    CssClass="CustomGrid table table-bordered table-condenced" AutoGenerateColumns="False"
                                    DataKeyNames="BookingPin" PageSize="25000">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatSeat" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boat Reference No" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPremiumStatus" Text='<%# Bind("PremiumStatus") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNetAmt" Text='<%# Bind("InitNetAmount") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblBookingPin" Text='<%# Bind("BookingPin") %>' runat="server" OnClick="lblBookingPin_Click" />
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

        <div class="row">
            <div class="col-sm-4 col-xs-12">
                <div id="divExistDtl" runat="server" visible="false">
                    <h4>Existing Details</h4>
                    <div class="row p-2 mydivbrdr">
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblBoatType">
                                    <i class="fa fa-ship" aria-hidden="true"></i>
                                    Boat Type
                                </label>
                                <asp:TextBox ID="txtBoatType" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="1" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblBoatSeat">
                                    <i class="fa fa-chair" aria-hidden="true"></i>
                                    Boat Seat
                                </label>
                                <asp:TextBox ID="txtBoatSeat" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="2" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblBoatCharge">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Boat Charge
                                </label>
                                <asp:TextBox ID="txtBoatCharge" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="3" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblRowerChrg">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Rower Charge
                                </label>
                                <asp:TextBox ID="txtRowerCharge" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="4" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblTaxAmt">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Tax Amount
                                </label>
                                <asp:TextBox ID="txtTaxAmt" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="8" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblDeposit">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Boat Deposit
                                </label>
                                <asp:TextBox ID="txtDeposit" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="5" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblOfferAmt">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Offer Amount
                                </label>
                                <asp:TextBox ID="txtOfferAmt" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="6" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblNetAmt">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Net Amount
                                </label>
                                <asp:TextBox ID="txtNetAmt" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="7" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div id="divline1" class="vl" runat="server" visible="false"></div>
            <div class="col-sm-4 col-xs-12">
                <div id="divChangeDetails" runat="server" style="background-color: LightGray; height: 350px; width: 400px; margin-left: -4px;" visible="false">
                    <h4 style="padding-left: 5px;">Change Details</h4>
                    <div class="row p-2 mydivbrdr">
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblBoatType">
                                    <i class="fa fa-ship" aria-hidden="true"></i>
                                    Boat Type
                                </label>
                                <asp:DropDownList ID="ddlBoatType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged" TabIndex="9">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlBoatType" runat="server" ControlToValidate="ddlBoatType" InitialValue="0"
                                    ValidationGroup="ChangeBoat" SetFocusOnError="True" CssClass="vError">Select Boat Type</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblBoatSeat">
                                    <i class="fa fa-chair" aria-hidden="true"></i>
                                    Boat Seat
                                </label>
                                <asp:DropDownList ID="ddlBoatSeat" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBoatSeat_SelectedIndexChanged" TabIndex="10">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlBoatSeat" runat="server" ControlToValidate="ddlBoatSeat" InitialValue="0"
                                    ValidationGroup="ChangeBoat" SetFocusOnError="True" CssClass="vError">Select Boat Seat</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblBoatCharge">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Boat Charge
                                </label>
                                <asp:TextBox ID="txtChangeBoatChrg" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="11" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblRowerChrg">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Rower Charge
                                </label>
                                <asp:TextBox ID="txtChangeRowerChrg" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="12" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblChangeTax">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Tax Amount
                                </label>
                                <asp:TextBox ID="txtChangeTax" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="13" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblBoatDeposit">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Boat Deposit
                                </label>
                                <asp:TextBox ID="txtChangeDeposit" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="13" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-6 col-xs-12" runat="server">
                        </div>

                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblNetAmt">
                                    <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                                    Net Amount
                                </label>
                                <asp:TextBox ID="txtChangeNetAmt" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="14" ReadOnly="true">
                                </asp:TextBox>
                            </div>

                        </div>


                    </div>

                </div>
            </div>
            <div id="divline" class="vl" runat="server" visible="false"></div>
            <div class="col-sm-3 col-xs-12">
                <div id="divChargeDetails" runat="server" style="background-color: #b3ffff; padding-left: 10px; height: 350px; width: 380px; margin-left: -4px;" visible="false">
                    <h4>Charge Details</h4>
                    <hr />
                    <div id="divsameboat" runat="server" visible="false">
                        <asp:Label ID="lblSameBoat" runat="server" Style="font-size: xx-large; color: red"></asp:Label>
                    </div>

                    <div id="divAvailable" runat="server" visible="false" style="text-align: left; padding-top: 2rem;">
                        <label for="lblAvailableBoat" style="font-size: x-large; color: black">
                            Available No.<br />
                            of Boats <span style="padding-left: 3.2rem;">:</span>
                        </label>
                        <asp:Label ID="lblAvailableBoat" runat="server" Style="font-size: x-large; color: green">                               
                        </asp:Label>
                    </div>


                    <div id="divRefund" runat="server" visible="false" style="text-align: left; padding-top: 2rem;">
                        <label for="lblRefund" style="font-size: x-large; color: black">
                            Refund <span style="padding-left: 0.2rem;">:</span><i class="fas fa-rupee-sign" aria-hidden="true"></i>
                        </label>
                        <asp:Label ID="lblRefundAmt" runat="server" Style="font-size: x-large; color: green">                               
                        </asp:Label>
                    </div>
                    <div id="divExtraCharge" runat="server" visible="false" style="text-align: left; padding-top: 2rem;">
                        <label for="lblExtra" style="font-size: x-large; color: black">
                            Extra Charge <span style="padding-left: 0.2rem;">:</span>
                            <i class="fas fa-rupee-sign" aria-hidden="true"></i>
                        </label>
                        <asp:Label ID="lblExtraAmt" runat="server" Style="font-size: x-large; color: green">                               
                        </asp:Label>

                    </div>
                    <div id="divPayment" runat="server" visible="false" style="text-align: left; padding-top: 2rem;">
                        <label for="lblPayment" style="font-size: x-large; color: black">
                            Payment Mode <span style="padding-left: 0.2rem;">:</span>
                        </label>
                        <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="payMode">
                            <%--  <asp:ListItem Value="0">Cash</asp:ListItem>
                             <asp:ListItem Value="1">Card</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                    <div id="divbutton" runat="server" style="padding-top: 4.7rem; padding-left: 15.5rem; text-align: left;">
                        <div class="row">
                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit"
                                ValidationGroup="ChangeBoat" OnClick="btnSubmit_Click" />
                            &nbsp
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-danger" Text="Reset"
                            OnClick="btnReset_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divGridBoatChange" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvBoatChangedDisplay" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingPin">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%--  <%#Container.DataItemIndex+1 %>--%>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
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
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Booked Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookedDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="BoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="BoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat RefNo" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatRefNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Old Net Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOldNetAmt" runat="server" Text='<%# Bind("OldNetAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New Net Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNewNetAmt" runat="server" Text='<%# Bind("NewNetAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Refund Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblExtraRefundAmt" runat="server" Text='<%# Bind("ExtraRefundAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Extra Charge" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblExtraCharge" runat="server" Text='<%# Bind("ExtraCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
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
                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                </asp:GridView>
            </div>
            <div id="divBoatChangePreNext" runat="server">
                <asp:Button ID="BoatChangeback" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="BoatChangeback_Click" />
                &nbsp
              <asp:Button ID="BoatChangeNext" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="BoatChangeNext_Click" />
                &nbsp
            </div>
        </div>

        <div id="divBoatLogDetails" runat="server" visible="false">
            <div class="table-responsive">
                <asp:GridView ID="gvViewBoatLogDetails" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%--  <%#Container.DataItemIndex+1 %>--%>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
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
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Seat" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New BoatId" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNewBoatId" runat="server" Text='<%# Bind("NewBoatId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New Boat Number" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNewBoatNum" runat="server" Text='<%# Bind("NewBoatNum") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New Boat Charge" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNewBoatCharge" runat="server" Text='<%# Bind("NewBoatCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New Rower Charge" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNewRowerCharge" runat="server" Text='<%# Bind("NewRowerCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New Deposit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNewDeposit" runat="server" Text='<%# Bind("NewDeposit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New Net Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNewNetAmount" runat="server" Text='<%# Bind("NewNetAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="New Tax Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNewTaxAmount" runat="server" Text='<%# Bind("NewTaxAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <%--<asp:TemplateField HeaderText="More" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnView" ForeColor="#512c88" CausesValidation="false" Font-Underline="false"
                                    Width="20px" CssClass="imgOutLine" OnClick="ImgBtnView_Click"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/View.png" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                    </Columns>

                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                </asp:GridView>
            </div>
            <div id="divBoatLogPreNext" runat="server">
                <asp:Button ID="BoatLogback" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="BoatLogback_Click" />
                &nbsp
              <asp:Button ID="BoatLogNext" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="BoatLogNext_Click" />
                &nbsp
            </div>
        </div>

    </div>

    <div class="modal" id="myModal">
        <div class="modal-dialog modal-dialog-centered" style="max-width: 1000px !important">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header" style="background-color: #004c8c; color: white">
                    <h4 class="modal-title">Boat Change Log Details</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: white;">
                        <span aria-hidden="true">&times;</span></button>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <div class="row m-0">
                        <div class="col-sm-4 col-xs-12">
                            <div class="row">
                                <div class="col-sm-6 col-xs-12">
                                    <label for="boat">Booking Id </label>
                                </div>
                                <div class="col-sm-1 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <asp:Label ID="lblPopBookId" runat="server" Font-Bold="true">
                                    </asp:Label>
                                </div>
                            </div>
                        </div>
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
                                    <label for="boat">Boat ReferenceNo </label>
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
                    </div>
                    <h5>Old Details</h5>
                    <div id="divModalOldGrid" runat="server">
                        <div class="table-responsive">
                            <asp:GridView ID="gvOldLogDet" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingPin" PageSize="10" OnPageIndexChanging="gvOldLogDet_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Boat Type" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Boat Seat" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldBoatSeat" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Boat Charge" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldBoatChrg" runat="server" Text='<%# Bind("OldBoatCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Rower Charge" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldRowerChrg" runat="server" Text='<%# Bind("OldRowerCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Deposit" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldDeposit" runat="server" Text='<%# Bind("OldDeposit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Offer Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldOffAmt" runat="server" Text='<%# Bind("OldOfferAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Net Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldNetAmt" runat="server" Text='<%# Bind("OldNetAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Old Tax Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopOldTaxAmt" runat="server" Text='<%# Bind("OldTaxAmount") %>'></asp:Label>
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
                                AutoGenerateColumns="False" DataKeyNames="BookingPin" PageSize="10" OnPageIndexChanging="gvNewLogDet_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Boat Type" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Boat Seat" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewBoatSeat" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Boat Charge" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewBoatChrg" runat="server" Text='<%# Bind("NewBoatCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Rower Charge" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewRowerChrg" runat="server" Text='<%# Bind("NewRowerCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Deposit" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewDeposit" runat="server" Text='<%# Bind("NewDeposit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Net Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewNetAmt" runat="server" Text='<%# Bind("NewNetAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="New Tax Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPopNewTaxAmt" runat="server" Text='<%# Bind("NewTaxAmount") %>'></asp:Label>
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
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
    <script type="text/javascript" src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js"
        integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossorigin="anonymous"></script>

    <asp:HiddenField ID="hfBoatHouseId" runat="server" />
    <asp:HiddenField ID="hfBoatHouseName" runat="server" />
    <asp:HiddenField ID="hfUserId" runat="server" />



</asp:Content>

