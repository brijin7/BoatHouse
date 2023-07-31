<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="ReTripEntryDtl.aspx.cs" Inherits="ReTripEntryDtl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />
    <div class="form-body">
        <div id="divEntry1" runat="server">

            <h5 class="pghr">Re-Trip Entry Details <span style="float: right;">
                <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>

            <hr />
            <div class="mydivbrdr" id="divEntry" runat="server" visible="false">
                <div class="row p-2">
                    <div class="col-sm-6 col-xs-12" runat="server">
                        <div class="col-sm-6 col-xs-12" runat="server">
                            <div class="form-group">
                                <label for="lblbooking">
                                    <i class="fas fa-address-book"></i>
                                    Booking Id
                                </label>
                                <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control" AutoComplete="Off" OnTextChanged="txtBookingId_TextChanged"
                                    AutoPostBack="true" TabIndex="1" MaxLength="50">
                                </asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBookingId"
                                    SetFocusOnError="True" CssClass="vError">Enter Booking Id</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12" runat="server">
                        </div>

                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div id="DivAddRentrydtl" runat="server" visible="false" style="margin-top: 15px; overflow-y: scroll; max-height: 300px; min-height: 200px; overflow-x: hidden; margin-bottom: 7px;">
                            <div class="table-responsive">

                                <asp:GridView ID="gvRentrydtlBookingId" runat="server" AllowPaging="True"
                                    CssClass="CustomGrid table table-bordered table-condenced" AutoGenerateColumns="False"
                                    DataKeyNames="BookingPin" PageSize="25000">
                                    <Columns>

                                        <asp:TemplateField HeaderText="BookingId " HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="BoatType" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="BoatReferenceNo" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="BookedDate" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookedDate" runat="server" Text='<%# Bind("BookedDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
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

        <div id="tripdtls" runat="server" visible="false">
            <h4>TRIP DETAILS</h4>
            <div class="row p-2">
                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-ship" aria-hidden="true"></i>
                            Boat Number
                        </label>
                        <asp:TextBox ID="lblBoatNumber" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="2" ReadOnly="true">
                        </asp:TextBox>
                    </div>
                </div>

                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-ship" aria-hidden="true"></i>
                            Booking Pin
                        </label>
                        <asp:TextBox ID="lblboatpin" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="3" ReadOnly="true">
                        </asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row p-2">
                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-user" aria-hidden="true"></i>
                            Rower
                        </label>
                        <asp:TextBox ID="txtRower" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="4" ReadOnly="true">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-ship" aria-hidden="true"></i>
                            Boat Type
                        </label>
                        <asp:TextBox ID="txtBoatType" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="5" ReadOnly="true">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fas fa-chair" aria-hidden="true"></i>
                            Boat Seater
                        </label>
                        <asp:TextBox ID="txtBoatSeater" runat="server" CssClass="form-control" AutoComplete="Off" AutoPostBack="true" TabIndex="6" ReadOnly="true">
                        </asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row p-2">
                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-calendar" aria-hidden="true"></i>
                            Booked Date
                        </label>
                        <asp:TextBox ID="txtBookedDate" runat="server" CssClass="form-control" AutoComplete="Off"
                            AutoPostBack="true" TabIndex="7" ReadOnly="true">
                        </asp:TextBox>

                    </div>
                </div>
                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-calendar" aria-hidden="true"></i>
                            Start Time
                        </label>
                        <asp:TextBox ID="txtstartTime" runat="server" TextMode="Time" CssClass="form-control"
                            AutoPostBack="true" OnTextChanged="txtstartTime_TextChanged"
                            AutoComplete="Off" TabIndex="8" MaxLength="10" ReadOnly="true">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-calendar" aria-hidden="true"></i>
                            End Time
                        </label>
                        <asp:TextBox ID="txtEndTime" runat="server" TextMode="Time" CssClass="form-control" AutoPostBack="true"
                            AutoComplete="Off" TabIndex="9" MaxLength="10" OnTextChanged="txtEndTime_TextChanged" ReadOnly="true">
                        </asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row p-2">

                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-calendar" aria-hidden="true"></i>
                            ReTrip Start Time
                        </label>
                        <asp:TextBox ID="txtReStartTime" runat="server" TextMode="Time" CssClass="form-control"
                            AutoPostBack="true" OnTextChanged="txtReStartTime_TextChanged"
                            AutoComplete="Off" TabIndex="10" MaxLength="10">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReStartTime"
                            ValidationGroup="BookingRe-trip" SetFocusOnError="True" CssClass="vError">Enter Retrip Start Time</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-calendar" aria-hidden="true"></i>
                            ReTrip End Time
                        </label>
                        <asp:TextBox ID="txtReEndTime" runat="server" TextMode="Time" CssClass="form-control" AutoPostBack="true"
                            AutoComplete="Off" TabIndex="11" MaxLength="10" OnTextChanged="txtReEndTime_TextChanged">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtReEndTime"
                            ValidationGroup="BookingRe-trip" SetFocusOnError="True" CssClass="vError">Enter Retrip End Time</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>


            <div class="row p-2">
                <div class="col-sm-6 col-xs-12" runat="server">
                    <div class="form-group">
                        <label for="lblbooking">
                            <i class="fa fa-calendar" aria-hidden="true"></i>
                            Reason
                        </label>
                        <asp:TextBox ID="txtreason" runat="server" TextMode="MultiLine" CssClass="form-control" AutoComplete="Off" TabIndex="12" MaxLength="10">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtreason"
                            ValidationGroup="BookingRe-trip" SetFocusOnError="True" CssClass="vError">Enter Reason</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-3 col-xs-12" runat="server">
                    <div class="form-group" style="margin-top: 40px;">
                        <label for="lblbooking">
                            <i class="" aria-hidden="true"></i>
                        </label>

                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BookingRe-trip" TabIndex="13" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="14" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>

        </div>
        <div id="divGridReenrty" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvReEnrty" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="25000" OnRowDataBound="gvReEnrty_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BookingId" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatType" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="BoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="BoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("BoatSeat") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BookedDate" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookedDate" runat="server" Text='<%# Bind("BookedDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatNumber" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="BoatId" runat="server" Text='<%# Bind("BoatId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatNumber" runat="server" Text='<%# Bind("BoatNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TripStartTime" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TripEndTime" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Bind("TripEndTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ReTripStartTime" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblReTripStartTime" runat="server" Text='<%# Bind("ReTripStartTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ReTripEndTime" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblReTripEndTime" runat="server" Text='<%# Bind("ReTripEndTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="RowerName" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>

                                <asp:Label ID="RowerId" runat="server" Text='<%# Bind("RowerId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("Rower") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reason" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>


                                <asp:Label ID="lblReason" runat="server" Text='<%# Bind("Reason") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ActiveStatus" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to Inactive this record?');" />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure you want to Active this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="25%" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfBoatHouseId" runat="server" />
    <asp:HiddenField ID="hfBoatHouseName" runat="server" />
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

