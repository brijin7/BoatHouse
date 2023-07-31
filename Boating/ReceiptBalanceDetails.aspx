<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="ReceiptBalanceDetails.aspx.cs" Inherits="Boating_ReceiptBalanceDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function numbersonly(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
                if (unicode < 48 || unicode > 57) //if not a number
                    return false //disable key press
            }
        }
    </script>
    <style>
        .inline-rb input[type="radio"] {
            width: auto;
            margin-right: 5px;
        }

        .inline-rb label {
            display: inline;
        }
    </style>
    <div class="form-body">
        <h5 class="pghr">Receipt Balance Refund Details</h5>
        <hr />
        <div class="col-md-12 col-lg-12 col-sm-12">
            <div class="row" style="background-color: khaki;">
                <div class="col-md-8">
                    <asp:RadioButtonList ID="RadbtnReceiptBalanceDetails" CssClass="inline-rb" Font-Bold="true" ForeColor="#ffffcc" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="RadbtnReceiptBalanceDetails_SelectedIndexChanged" CellSpacing="6" CellPadding="6" Width="100%"
                        RepeatColumns="3" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Text="Receipt Refund Pending List"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Receipt Refund Settled List"></asp:ListItem>
                        <%--<asp:ListItem Value="2" Text="Receipt Balance Details Report"></asp:ListItem>--%>
                    </asp:RadioButtonList>
                </div>


            </div>

            <div class="mydivbrdr" runat="server" id="divAllDetails">
                <div class="row p-2">
                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblBookingDate" id="lblBookingDate" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>Booking Date<span class="spStar">*</span></label>
                            <asp:TextBox ID="txtBookingDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                            </asp:TextBox>
                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter Booking Date</asp:RequiredFieldValidator>--%>
                        </div>
                    </div>

                    <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                        <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="Search" TabIndex="2" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" OnClick="btnReset_Click" TabIndex="3" />
                    </div>



                    <div class="col-sm-12 col-xs-12" runat="server" style="height: 600px; overflow-y: auto; float: left; position: static;">
                        <div class="table-responsive" runat="server" id="divReceiptBalanceDetails">

                            <asp:GridView ID="GvReceiptBalanceDetails" runat="server" CssClass="gvv display table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBookingId" runat="server" OnClick="lnkBookingId_Click" Text='<%# Bind("BookingId") %>' Font-Bold="true" Font-Size="X-Large"></asp:LinkButton>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Collected Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCollectedAmount" runat="server" Text='<%# Bind("CollectedAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bill Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillAmount" runat="server" Text='<%# Bind("BillAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Balance Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# Bind("BalanceAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRstatus" runat="server" Text='<%# Bind("Rstatus") %>'></asp:Label>
                                            <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                            <asp:HiddenField ID="hfBookingid" runat="server" />
                            <asp:GridView ID="GvSettledList" runat="server" CssClass="gvv display table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsetBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsetBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Collected Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsetCollectedAmount" runat="server" Text='<%# Bind("CollectedAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bill Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsetBillAmount" runat="server" Text='<%# Bind("BillAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Balance Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsetBalanceAmount" runat="server" Text='<%# Bind("BalanceAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsetRstatus" runat="server" Text='<%# Bind("Rstatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="RePayment Type" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsetRePaymentType" runat="server" Text='<%# Bind("RePaymentType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ReFunded By" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsetRefunderName" runat="server" Text='<%# Bind("RefunderName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="HiddenField1" runat="server" />

                            <%--  <ajax:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="pnlBillService" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>--%>
                            <ajax:ModalPopupExtender ID="MpeBillService" runat="server" BehaviorID="MpeBillService"
                                TargetControlID="HiddenField1" PopupControlID="pnlBillService"
                                BackgroundCssClass="modalBackground">
                            </ajax:ModalPopupExtender>

                            <asp:Panel ID="pnlBillService" runat="server" CssClass="Msg" Visible="false">
                                <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
                                    <div class="modal-content" style="width: 600px; max-height: 700px; min-height: 400px;">
                                        <div class="modal-header" style="background-color: #124a79; color: white;">
                                            <h5 class="modal-title">Receipt Balance Details
                                       <asp:Label ID="Label11" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                            </h5>
                                            <asp:ImageButton ID="ImgClose" runat="server" OnClick="ImgClose_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                                    <div class="row" style="padding: 5px">
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <label for="lblmaxcharge">
                                                                Booking Date
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                            :
                                                        </div>
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <asp:Label ID="lblBookingDateDisp" runat="server" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                                    <div class="row" style="padding: 5px">
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <label for="lblmaxcharge">
                                                                Booking ID
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                            :
                                                        </div>
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <asp:Label ID="lblBookingIdDisp" runat="server" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                                    <div class="row" style="padding: 5px">
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <label for="lblmaxcharge">
                                                                Collected Amount
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                            :
                                                        </div>
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <asp:Label ID="lblCollectedAmountDisp" runat="server" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                                    <div class="row" style="padding: 5px">
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <label for="lblmaxcharge">
                                                                Bill Amount
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                            :
                                                        </div>

                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            ₹
                                    <asp:Label ID="lblBillAmountDisp" runat="server" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                                    <div class="row" style="padding: 5px">
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <label for="lblmaxcharge">
                                                                <b>
                                                                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Size="Larger">Refund</asp:Label></b>
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                            :
                                                        </div>
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Larger">  ₹</asp:Label>
                                                            <asp:Label ID="lblBalanceAmountDisp" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="Larger"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                                    <div class="row" style="padding: 5px">
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <label for="lblmaxcharge">
                                                                <b>
                                                                    <asp:Label ID="Label4" runat="server" Font-Bold="true" Font-Size="Larger">Customer Mobile </asp:Label></b>
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                            :
                                                        </div>
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <asp:TextBox ID="txtCustomerMobile" onkeypress="return numbersonly(event)" runat="server" CssClass="form-control" MaxLength="10" Font-Bold="true" Font-Size="Larger"
                                                                AutoComplete="off" TabIndex="1"> </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCustomerMobile"
                                                                ValidationGroup="RowerSettlement" SetFocusOnError="True" CssClass="vError">Enter Customer Mobile No</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12" runat="server" id="divrepaymentType">
                                                    <div class="row" style="padding: 5px">
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <label for="lblmaxcharge">
                                                                <b>
                                                                    <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Size="Larger">RePayment Type </asp:Label></b>
                                                            </label>
                                                        </div>
                                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                            :                                   
                                                        </div>
                                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                                            <asp:DropDownList ID="ddlPaymentType" runat="server" TabIndex="2" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                                    <div class="row" style="padding: 5px">
                                                        <div class="col-sm-6 col-md6 col-lg-6 col-xs-12">
                                                        </div>
                                                        <div class="col-sm-6 col-md6 col-lg-6 col-xs-12" style="text-align: center">
                                                            <asp:Button ID="btnRefund" runat="server" Text="Refund" class="btn btn-primary" Width="100px" ValidationGroup="RowerSettlement"
                                                                TabIndex="3" CausesValidation="true" OnClick="btnRefund_Click" Font-Bold="true" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>


                                        </div>

                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

