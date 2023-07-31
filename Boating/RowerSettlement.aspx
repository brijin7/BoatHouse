<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RowerSettlement.aspx.cs" Inherits="Boating_RowerSettlement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-11 col-xs-12">
        <h5 class="pghr">Rower Settlement 
      

        </h5>
        <hr />
        <br />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fa fa-user"></i>
                                Rower <span class="spStar">*</span>
                            </label>
                            <asp:DropDownList ID="ddlRower" runat="server" AutoPostBack="true" CssClass="form-control" TabIndex="1" OnSelectedIndexChanged="ddlRower_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRower"
                                ValidationGroup="RowerSettlement" SetFocusOnError="True" CssClass="vError">Select Rower</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fa fa-ship" aria-hidden="true"></i>
                                Trip Date <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtTripDate" runat="server" CssClass="form-control datepicker" AutoComplete="Off" TabIndex="2"
                                OnTextChanged="txtTripDate_TextChanged" AutoPostBack="true">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTripDate"
                                ValidationGroup="RowerSettlement" SetFocusOnError="True" CssClass="vError">Enter Trip Date</asp:RequiredFieldValidator>
                        </div>
                    </div>


                    <div class="col-sm-4 col-xs-12" style="margin-top: 14px">
                        <div class="form-group">
                            <label for="lblboatid">
                            </label>
                            <asp:RadioButtonList ID="rblSettlement" runat="server"
                                AutoPostBack="true" RepeatDirection="Horizontal" TabIndex="3" CssClass="rbl" OnSelectedIndexChanged="rblSettlement_SelectedIndexChanged">
                                <asp:ListItem Value="TS" Selected="true">To Be Settled</asp:ListItem>
                                <asp:ListItem Value="S">Already Settled</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                    </div>

                    <div class="col-sm-2 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="RowerSettlement"
                                TabIndex="3" CausesValidation="true" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="4"
                                OnClick="btnCancel_Click" />
                        </div>
                    </div>

                </div>


            </div>
        </div>

        <div id="divSummary" runat="server" class="col-sm-12 col-xs-12">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblmsgSummary" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="GVRowerSummary" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
                    AutoGenerateColumns="False" DataKeyNames="TripDate,RowerId,TripCount,BalanceCharge,RowerName" PageSize="10" OnPageIndexChanging="GVRowerSummary_PageIndexChanging"
                    OnRowDataBound="GVRowerSummary_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trip Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTripDate" runat="server" Text='<%# Bind("TripDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rower Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerId" runat="server" Text='<%# Bind("RowerId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rower" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trips" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnTripModal" runat="server" Text='<%# Bind("TripCount") %>' OnClick="lbtnTripModal_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalCharge" runat="server" Text='<%# Bind("TotalCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settled Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidCharge" runat="server" Text='<%# Bind("PaidCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Balance Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBalanceCharge" runat="server" Text='<%# Bind("BalanceCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settle" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSettle" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>

                <div class="row m-0" id="divSettleAmt" runat="server">
                    <div class="col-sm-6 col-xs-12">
                        <asp:Label ID="lblText" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-6 col-xs-12" style="float: right;">
                        <div class="row m-0 input-group-append">
                            <div class="col-sm-5 col-xs-12 text-right">
                                <b>Total Amount</b>
                            </div>
                            <div class="col-sm-5 col-xs-12">
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <asp:Button ID="btnSettlement" runat="server" Text="Pay" class="btn btn-success" CausesValidation="true" OnClick="btnSettlement_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server" class="col-sm-12 col-xs-12">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblPayGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvRowerSettled" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
                    AutoGenerateColumns="False" DataKeyNames="SettlementId, RowerId,SettlementDate,RowerName,SettlementAmt" PageSize="10" OnPageIndexChanging="gvRowerSettled_PageIndexChanging" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settlement Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSettlementId" runat="server" Text='<%# Bind("SettlementId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settlement Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSettlementDate" runat="server" Text='<%# Bind("SettlementDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rower" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settlement Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblSettlementAmt" runat="server" Text='<%# Bind("SettlementAmt") %>' OnClick="lblSettlementAmt_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <asp:Label ID="lblTotal" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>

        <div class="modal" id="myModal">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header" style="background-color: #004c8c; color: white">
                        <h4 class="modal-title">Trip Details</h4>
                        <button type="button" class="close" data-dismiss="modal" style="color: white">&times;</button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-sm-6 col-xs-12">
                                Rower Name: 
                                 <asp:Label ID="lblRowerModal" runat="server" ForeColor="#000066" Font-Bold="true" Font-Size="16px"></asp:Label>
                            </div>
                        </div>
                        <div id="divSettle" runat="server" class="col-sm-12 col-xs-12" style="overflow: auto;">
                            <div class="table-responsive">
                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                                <asp:GridView ID="gvBoatRowerSettle" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid" ShowFooter="true"
                                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="10" OnPageIndexChanging="gvBoatRowerSettle_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Mobile" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Trip End Time" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Bind("TripEndTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Duration" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTravelledMinutes" runat="server" Text='<%# Bind("TravelledMinutes") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Settlement Amount" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSettlementAmt" runat="server" Text='<%# Bind("SettlementAmt") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gvHead" />
                                    <AlternatingRowStyle CssClass="gvRow" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hfNoofTrips" runat="server" />
    <asp:HiddenField ID="hfActRowerCharge" runat="server" />
    <asp:HiddenField ID="hfRowerName" runat="server" />
    <asp:HiddenField ID="hfTripCount" runat="server" />
    <asp:HiddenField ID="hfCurrentDate" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <ajax:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="pnlMaterialItem" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
    <ajax:ModalPopupExtender ID="MpeMaterial" runat="server" BehaviorID="MpeMaterial" TargetControlID="HiddenField1" PopupControlID="pnlMaterialItem"
        BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>

    <asp:Panel ID="pnlMaterialItem" runat="server" CssClass="Msg" Visible="false">
        <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
            <div class="modal-content" style="width: 750px">
                <div class="modal-header">
                    <div class="col-sm-12 col-xs-12">
                        <img class="img-fluid" src="../images/TTDCLogo.svg" alt="Boating" />
                    </div>

                    <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                </div>

                <div class="modal-body">
                    <%-- <h5 class="modal-title" id="exampleModalLabel">Rower Trip Details For Settlement Id : 
                       <asp:Label ID="lblRowerSettlementId" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </h5>--%>
                    <asp:Label ID="lblGvMsgPopup" runat="server"></asp:Label>
                    <asp:GridView ID="gvRowerSettleId" runat="server" AllowPaging="True" ShowFooter="True"
                        CssClass="CustomGrid table table-bordered table-condenced" AutoGenerateColumns="False" Width="100%"
                        PageSize="25000">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Date " HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Settlement Amount" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblActualRowerCharge" runat="server" Text='<%# Bind("ActualRowerCharge") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>

                        <HeaderStyle CssClass="gvHead" />
                        <AlternatingRowStyle CssClass="gvRow" />
                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                        <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" />
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.min.js"
        integrity="sha384-+sLIOodYLS7CIrQpBjl+C7nPvqq+FbNUBDunl/OZv93DB7Ln/533i8e/mZXLi/P+" crossorigin="anonymous"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
    <script type="text/javascript">
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

        $(function () {

            $("[id*=chkSettle]").on("change", function () {

                document.getElementById("<%=divSettleAmt.ClientID%>").style.display = "none";
                var totaltcfsa = 0;
                //loop through each checked row and sum the different columns
                $("[id*=chkSettle]:checked").each(function () {
                    document.getElementById("<%=divSettleAmt.ClientID%>").style.display = "block";
                    var chk = $(this);
                    var tcfsa = chk.parent().siblings(":nth-child(7)").text();

                    totaltcfsa += parseFloat(tcfsa);
                });

                //display results
                $("[id*=txtTotal]").val(totaltcfsa.toFixed(2));
            });
        });
    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

