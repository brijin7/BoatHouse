<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="Refund.aspx.cs" Inherits="Boating_Refund" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <style>
        .green {
            color: darkgreen;
            font-weight: 700;
        }

        .red {
            color: red;
            font-weight: 700;
        }

        .pnl1 {
            background-color: #c9d2ff;
            color: #203e9e;
            font-size: 14px;
            font-weight: 700;
        }

        .pnl2 {
            background-color: #dec9ff;
            color: #203e9e;
            font-size: 14px;
            font-weight: 700;
        }
    </style>

    <div class="form-body" />
    <h5 class="pghr">Refund Counter  <span style="float: right;">
        <asp:LinkButton ID="lbtnView" CssClass="lbtnViewLog" OnClick="lbtnView_Click" runat="server"> 
                <i class="fas fa-receipt"></i> View Log Details</asp:LinkButton>
        <asp:LinkButton ID="lblCounter" CssClass="lbtnViewLog" OnClick="lblCounter_Click" Visible="false" runat="server"> 
                <i class="fas fa-receipt"></i> Refund Counter</asp:LinkButton></span>
    </h5>
    <hr />
    <br />
    <div class="mydivbrdr" runat="server" id="divbrdr" visible="false">
        <div class="row p-2">
            <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                <div class="form-group">
                    <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                        <i class="fa fa-calendar" aria-hidden="true"></i>Date<span class="spStar">*</span></label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDate"
                        ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                <asp:Button ID="Button1" runat="server" Text="Search" class="btn btn-primary" OnClick="Button1_Click" ValidationGroup="Search" TabIndex="2" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" OnClick="btnReset_Click" TabIndex="3" />
            </div>
        </div>
    </div>
    <div runat="server" id="divViewLog" visible="false">
        <div class="table-responsive">
            <div style="text-align: right;">
                <asp:GridView ID="GvViewLog" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" PageSize="10" ShowFooter="true" OnPageIndexChanging="GvViewLog_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Requested By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRequestByName" runat="server" Text='<%# Bind("RequestedByName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Requested Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRequestedAmount" runat="server" Text='<%# Bind("RequestedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Requested Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Bind("RequestedDate") %>'></asp:Label>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Paid By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidName" runat="server" Text='<%# Bind("PaidName") %>'></asp:Label>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Paid Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidAmount" runat="server" Text='<%# Bind("PaidAmount") %>'></asp:Label>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Paid Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidDate" runat="server" Text='<%# Bind("PaidDate") %>'></asp:Label>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Payment Status" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentStatus" Font-Bold="true" CssClass='<%# Eval("PaymentStatus").ToString() =="PAID" ? "green" :  "red" %>' runat="server" Text='<%# Bind("PaymentStatus") %>'></asp:Label>

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

    <div id="divEntry" runat="server">
        <div class="mydivbrdr">
            <div id="divRequest" runat="server">



                <div class="col-sm-4 col-md-4 col-lg-4 col-xs-4">
                    <div runat="server" id="divUsername" visible="false" style="overflow: auto; max-height: 300px; max-width: 500px; min-height: 200px; min-width: 50%;">
                        <div class="table-responsive">
                            <asp:GridView ID="GvUsername" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="UserId" PageSize="20" Width="100%" OnPageIndexChanging="GvUsername_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UserId " HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BoatTypeId " HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name " HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="BoatType" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblBookingTotalAmount" runat="server" Text='<%# Bind("Amount")  %>' CommandName="Action" OnClick="lblBookingTotalAmount_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gvHead" />
                                <AlternatingRowStyle CssClass="gvRow" />
                                <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />

                            </asp:GridView>

                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="hfuserid" runat="server" />
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                UserName
                            </label>
                            <%-- <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>--%>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="1" MaxLength="10" ReadOnly="true">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                                ValidationGroup="Refund" SetFocusOnError="True" CssClass="vError">Select User Name</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                BoatType
                            </label>
                            <%-- <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>--%>
                            <asp:TextBox ID="txtBoatType" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="2" MaxLength="10" ReadOnly="true">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtBoatType"
                                ValidationGroup="Refund" SetFocusOnError="True" CssClass="vError">Select BoatType</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div id="divAmount" runat="server" class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblbooking">
                                <i class="fas fa-address-book"></i>
                                Amount
                            </label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="3" MaxLength="10">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAmount"
                                ValidationGroup="Refund" SetFocusOnError="True" CssClass="vError">Enter Request Amount</asp:RequiredFieldValidator>
                        </div>
                    </div>


                    <div class="form-submit col-sm-3" style="padding-top: 30px; margin-right: inherit; margin-left: inherit">

                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Refund" OnClick="btnSubmit_Click" Width="100px" Font-Bold="True"
                            Style="font-size: 20px;"
                            TabIndex="3" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" OnClick="btnCancel_Click" Width="100px" Font-Bold="True"
                                 Style="font-size: 20px;"
                                 TabIndex="4" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div id="divReqAmnt" runat="server">

        <ul class="nav nav-pills" role="tablist">
            <li class="nav-item">
                <asp:LinkButton ID="lbtnByMe" runat="server" OnClick="lbtnByMe_Click" class="nav-link pnl1">
                            Request Raised by Me</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lbtnByOthers" runat="server" OnClick="lbtnByOthers_Click"
                    class="nav-link pnl2">Request Raised by Others</asp:LinkButton>
            </li>
        </ul>

        <div id="divReqByMe" runat="server" visible="false" style="background-color: #c9d2ff;">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblReqByMeMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="GVRequestingAmount" runat="server" CssClass="CustomGrid table table-bordered table-condenced" ShowFooter="true"
                    AutoGenerateColumns="False" DataKeyNames="UniqueId,UserName,UserId,BoatTypeName,RequestedAmount,RequestedBy,PaymentStatus">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requested By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRby" runat="server" Text='<%# Bind("RequestedByName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requested To" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatType" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requested Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRAmount" runat="server" Text='<%# Bind("RequestedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Paid Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidAmount" runat="server" Text='<%# Bind("PaidAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Status" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentStatus" runat="server" Text='<%# Bind("PaymentStatus") %>'
                                    CssClass='<%# Eval("PaymentStatus").ToString() =="PAID" ? "green" :  "red" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Settle" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSettlementAmt" runat="server" Text="PAY" OnClick="btnSettlement_Click"></asp:LinkButton>
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
        </div>

        <div id="divReqByOthers" runat="server" visible="false" style="background-color: #dec9ff;">

            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblReqByOthersMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvRequestAmtByOthers" runat="server" CssClass="CustomGrid table table-bordered table-condenced" ShowFooter="true"
                    AutoGenerateColumns="False" DataKeyNames="UniqueId,UserName,UserId,BoatTypeId,BoatTypeName,RequestedAmount,RequestedBy,PaymentStatus">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requested By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRby" runat="server" Text='<%# Bind("RequestedByName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatType" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requested Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRAmount" runat="server" Text='<%# Bind("RequestedAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Paid Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidAmount" runat="server" Text='<%# Bind("PaidAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Status" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentStatus" runat="server" Text='<%# Bind("PaymentStatus") %>'
                                    CssClass='<%# Eval("PaymentStatus").ToString() =="PAID" ? "green" :  "red" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settle" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnSettlementAmt" runat="server"
                                    Text='<%# Eval("PaymentStatus").ToString() =="PAID" ? "Recieved" :  "Pay" %>'
                                    Enabled='<%# Eval("PaymentStatus").ToString() =="PAID" ? false :  true %>'
                                    OnClick="btnSettlement_Click"></asp:LinkButton>
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




    <div class="modal" id="myModal">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header" style="background-color: #004c8c; color: white">
                    <h4 class="modal-title">ReFund</h4>
                    <button type="button" class="close" data-dismiss="modal" style="color: white">&times;</button>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <div id="divSettle" runat="server" class="col-sm-12 col-xs-12" style="overflow: auto;">
                        <div class="table-responsive">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </div>
                            <asp:GridView ID="gvRefundPay" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
                                AutoGenerateColumns="False" DataKeyNames="RequestedByName" PageSize="10" OnPageIndexChanging="gvRefundPay_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Requested By" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestedBy" runat="server" Text='<%# Bind("RequestedByName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Requested Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestedAmount" runat="server" Text='<%# Bind("RequestedAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                </Columns>
                                <HeaderStyle CssClass="gvHead" />
                                <AlternatingRowStyle CssClass="gvRow" />
                                <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                            </asp:GridView>

                            <div class="row m-0" id="divPaidAmt" runat="server" visible="true">
                                <div class="col-sm-4 col-xs-12">
                                    <asp:Label ID="lblText" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-8 col-xs-12" style="float: right;">
                                    <div class="row m-0 input-group-append">
                                        <div class="col-sm-5 col-xs-12 text-right">
                                            <b>Requested Amount</b>
                                        </div>
                                        <div class="col-sm-5 col-xs-12">
                                            <asp:TextBox ID="txtTotal" runat="server" AutoComplete="fasle" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2 col-xs-12">
                                            <asp:Button ID="btnFinalPay" runat="server" Text="Pay" class="btn btn-success" CausesValidation="true" OnClick="btnFinalPay_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <asp:HiddenField runat="server" ID="HiddenField1" />
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


    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

