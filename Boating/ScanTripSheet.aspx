<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="ScanTripSheet.aspx.cs" Inherits="Boating_ScanTripSheet" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .BarCodeTextStart {
            width: 500px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
            background-color: #FFF8C6;
        }

        .BarCodeTextRower {
            width: 500px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
            background-color: #FFF8C6;
        }

        .blink {
            animation: blink-animation 1s linear infinite;
            -webkit-animation: blink-animation 1s linear infinite;
        }

        @keyframes blink-animation {
            50% {
                opacity: 0;
            }
        }

        @-webkit-keyframes blink-animation {
            50% {
                opacity: 0;
            }
        }
    </style>
    <script type="text/javascript">
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblStartResponse.ClientID %>").style.display = "none";
            }, seconds * 2000);
        };
    </script>

    <script type="text/javascript">
        function HideLabelRower() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblRowerResponse.ClientID %>").style.display = "none";
            }, seconds * 2000);
        };
    </script>

    <script type="text/javascript">
        function StartBox() {
            var txtStart = document.getElementById('<%=txtStartDetails.ClientID %>');
            txtStart.focus();
        }
    </script>


    <div class="form-body col-sm-12 col-xs-12">


        <h5 class="pghr" style="padding-top: 1.5rem; padding-left: 3rem; text-align: center; display: inline; font-size: 25px;">Scan Trip Sheet </h5>
        <div class="row">
            <div class="col-12">
                <asp:TextBox ID="txtStartDetails" runat="server" CssClass="BarCodeTextStart" placeholder="Scan Ticket QRCode" Font-Bold="true" AutoComplete="off" Font-Size="X-Large" AutoPostBack="true" OnTextChanged="txtStartDetails_TextChanged"></asp:TextBox>
            </div>
            <div class="col-12">
                <asp:TextBox ID="txtRowerDetails" runat="server" CssClass="BarCodeTextRower" placeholder="Scan Rower QRCode" Font-Bold="true" AutoComplete="off" Font-Size="X-Large" AutoPostBack="true" OnTextChanged="txtRowerDetails_TextChanged" Visible="false"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <asp:Label ID="lblStartResponse" CssClass="blink" runat="server" Font-Bold="true" ForeColor="#6163b8" Font-Size="X-Large"></asp:Label>
            </div>
            <div class="col-12">
                <asp:Label ID="lblRowerResponse" CssClass="blink" runat="server" Font-Bold="true" ForeColor="#6163b8" Font-Size="X-Large"></asp:Label>
            </div>
        </div>

        <div class="table-div" id="divGridStart" runat="server" style="background-color: white;">
            <div class="table-responsive">
                <asp:GridView ID="gvTripSheetSettelementStart" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatReferenceNo" PageSize="10">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Serial" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingSerial" runat="server" Text='<%# Bind("BookingSerial") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boarding Time" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoardingTime" runat="server" Text='<%# Bind("BoardingTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Duration (Mins)" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("TripStartTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>



                <asp:GridView ID="gvTripSheetSettelementEnd" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatReferenceNo" PageSize="10">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Serial" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingSerial" runat="server" Text='<%# Bind("BookingSerial") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boarding Time" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoardingTime" runat="server" Text='<%# Bind("BoardingTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Duration (Mins)" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("TripStartTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trip End Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("TripEndTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Travelled Minutes" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTraveledMins" runat="server" Text='<%# Eval("TraveledTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>

        <h4 id="header" runat="server" style="padding-top: 1rem; color: palevioletred; font-size: xx-large"><b>Trip Started List</b></h4>
        <div class="table-div" id="divTripStarted" runat="server" style="background-color: white; overflow: auto; max-height: 650px; min-height: 550px;">
            <div class="table-responsive" style="overflow-x: hidden">
                <asp:GridView ID="GvStartedList" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatReferenceNo" PageSize="10" OnPageIndexChanging="GvStartedList_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Pin No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Serial" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingSerial" runat="server" Text='<%# Bind("BookingSerial") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boarding Time" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoardingTime" runat="server" Text='<%# Bind("BoardingTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Duration (Mins)" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("TripStartTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="gvHead" Font-Size="X-Large" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfBoathouseId" runat="server" />
    <asp:HiddenField ID="hfBoathouseName" runat="server" />
    <asp:HiddenField ID="hfBarcodePin" runat="server" />
    <asp:HiddenField ID="hfBarcodePinRowerId" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hfBoatTypeId" runat="server" />
    <asp:HiddenField ID="hfBoatSeaterId" runat="server" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

