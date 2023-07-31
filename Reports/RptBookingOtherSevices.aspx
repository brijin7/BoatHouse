<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Reports/RptBookingOtherSevices.aspx.cs" Inherits="Boating_RptBookingOtherSevices" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />

    <style type="text/css">
        body {
        }

        .boat-unit {
            height: 150px;
            box-shadow: 0 0 10px 1px #929292;
        }

        .boat-image {
            padding: 10px;
        }

            .boat-image img {
                width: 100%;
                height: 130px;
                box-shadow: -3px 3px 10px #a7a7a7;
            }

        .p10 {
            padding: 10px;
        }

        .boat-type #lblBoatType {
            font-size: 20px;
            font-weight: 500;
            color: #0b5269;
            letter-spacing: .5px;
        }

        .list-heading {
            font-size: 12px;
            display: inline-block;
            color: #3282b8;
            letter-spacing: 0.5px;
        }

        .list-vals {
            font-weight: bold;
            font-size: 14px;
            color: #0f4c75;
            letter-spacing: 1px;
            padding-left: 5px;
        }

        .boat-count-input {
            width: 50%;
            margin: auto;
        }

        .boat-check {
            text-align: center;
            padding: 10px;
            margin-top: 26%;
        }

        .boat-list-chk input {
            height: 25px;
            width: 25px;
        }

        .boat-price-div {
            background: linear-gradient(42deg, white 82%, #151965 0%);
        }

        .price-badge {
            text-align: left;
            color: white;
            z-index: 1;
            right: 0px;
            top: 15px;
            position: absolute;
        }

            .price-badge h6 {
                font-size: 2rem;
            }

        .boat-check-div {
            background: linear-gradient(105deg, #151965 16%, white 8%);
        }

        /*tr:nth-child(even) {
            background-color: #f2f2f2;
        }*/

        .otherserv-list-input {
            display: block;
            padding-left: 10px;
        }
    </style>

    <style>
        .tdb {
            border-collapse: collapse;
            border: 1px solid black;
            width: 100%;
            margin: -1px;
        }
    </style>

    <div class="form-body">
        <h5 class="pghr">Other Services</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label2" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />
                </div>

            </div>
        </div>

        <div runat="server" id="divGridList" visible="false">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                 <div  style="text-align: right;">
                     <div id="divSearch" runat="server">
                            Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="16px" placeholder="Enter Booking Id" AutoComplete="Off" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox></div>
                <asp:GridView ID="GvBoatBooking" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId,Status" PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
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

                        <asp:TemplateField HeaderText="Date & Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingType" runat="server" Text='<%# Bind("BookingType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Item Charge" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblItemCharge" runat="server" Text='<%# Bind("ItemCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="No Of Items" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblNoOfItems" runat="server" Text='<%# Bind("NoOfItems") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Service Fare" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceFare" runat="server" Text='<%# Bind("ServiceFare") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCGST" runat="server" Text='<%# Bind("TaxAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNetAmount" runat="server" Text='<%# Bind("NetAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Print" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnPrint" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Print.svg" OnClick="ImgBtnEdit_Click" ToolTip="Print" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                     <HeaderStyle CssClass="gvHead" />
                                            <AlternatingRowStyle CssClass="gvRow" />
                                            <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                            <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" />
                </asp:GridView>
                <%--Newly added--%>
                  <div runat="server" id="divprevnext" style="text-align: left;">
                      <asp:Button ID="back" runat="server" CssClass="btn btn-color mg" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                                    &nbsp
                      <asp:Button ID="Next" Visible="true" CssClass="btn btn-color mg" runat="server" Text="Next →" OnClick="Next_Click" />
                       &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click"/>
                     <%--Newly added--%>               
                  </div>
            </div>
                </div>
            <asp:HiddenField runat="server" ID="hfreason" />
            <ajax:DragPanelExtender ID="DragPanelExtender4" runat="server" TargetControlID="pnlRsn" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
            <ajax:ModalPopupExtender ID="Mpepnlrsn" runat="server" BehaviorID="Mpepnlrsn" TargetControlID="hfreason" PopupControlID="pnlRsn"
                BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>

            <asp:Panel ID="pnlRsn" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; max-height: 500px; width: 500px; margin-top: 30px;">
                <asp:Panel ID="Panel4" runat="server" CssClass="drag">
                    <div class="modal-content" style="width: 480px; max-height: 200px; min-height: 150px">
                        <div class="modal-header">
                            <h5 class="modal-title">Reason For Reprint
                       <asp:Label ID="Label4" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </h5>
                            <asp:ImageButton ID="CloseRsnButton" runat="server" OnClick="CloseRsnButton_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-9">
                                    <label for="lblCategoryname" id="lblCategoryname"><i class="fa fa-ship" aria-hidden="true"></i>Reason<span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlReason" CssClass="form-control inputboxstyle" runat="server"
                                        TabIndex="1">
                                        <asp:ListItem Value="0">-Select-</asp:ListItem>
                                        <asp:ListItem Value="1">Power Cut</asp:ListItem>
                                        <asp:ListItem Value="2">Printing Problem</asp:ListItem>
                                        <asp:ListItem Value="3">Internet Connection Problem</asp:ListItem>
                                        <asp:ListItem Value="4">Some Other Reasons</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlReason"
                                        ValidationGroup="BoatHouseName" InitialValue="0" SetFocusOnError="True" CssClass="vError">Select Reason</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3" style="top: 30px;">
                                    <div class="table-responsive">
                                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                            <asp:Button runat="server" Text="Print" ID="RsnSubmit" OnClick="RsnSubmit_Click" CssClass="btn btn-primary" ValidationGroup="BoatHouseName" />
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

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <ajax:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="pnlBillService" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
    <ajax:ModalPopupExtender ID="MpeBillService" runat="server" BehaviorID="MpeBillService" TargetControlID="HiddenField1" PopupControlID="pnlBillService"
        BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>



    <asp:Panel ID="pnlBillService" runat="server" CssClass="Msg" Style="display: none; height: 750px; width: 500px;">
        <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
            <div class="maincontent text-right">
                <h4>
                    <%--   <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="printDiv()" ImageUrl="~/images/Print.svg" Width="35px" ToolTip="Print" />--%>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Print.svg" Width="35px" ToolTip="Print" OnClick="ImageButton1_Click" />
                    <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                </h4>
            </div>
            <div id='DivIdToPrint' style="padding: 15px 5px 15px 5px; overflow-y: scroll; height: 640px; width: 500px; text-align: center;">

                <asp:DataList ID="dtlistTicketOther" runat="server" OnItemDataBound="dtlistTicketOther_ItemDataBound" Width="100%">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td class="text-left font-weight-bold">
                                    <h3>
                                        <asp:Label ID="lblBillOBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label></h3>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold">
                                    <h3>
                                        <asp:Label ID="lblBillOBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label></h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgOtherQR" runat="server" />
                                </td>
                                <td colspan="2">
                                    <h2>
                                        <asp:Label ID="Label18" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label></h2>
                                    <h3>
                                        <asp:Label ID="Label21" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                                <td class="text-right font-weight-bold">
                                    <h4>Total Tickets :
                                                        <asp:Label ID="Label20" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td class="text-left">Service Fare
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold">
                                    <asp:Label ID="lblBillOChargePerItem" runat="server" Text='<%# Eval("ChargePerItem") %>'></asp:Label>
                                    *
                                                    <asp:Label ID="lblBillONoOfItems" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                    =
                                                    <asp:Label ID="lblBillOServiceFare" runat="server" Text='<%# Eval("ServiceFare") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td colspan="3">
                                    <asp:Label ID="lblBillOServiceId" runat="server" Text='<%# Eval("ServiceId") %>'
                                        Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center;">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 48%">
                                                <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                            </td>
                                            <td>&#9986;
                                            </td>
                                            <td style="width: 48%">
                                                <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>

                <table runat="server" style="width: 100%;">
                    <tr>
                        <td class="text-left font-weight-bold">
                            <h3>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label></h3>
                        </td>
                        <td colspan="2" class="text-right font-weight-bold">
                            <h3>
                                <asp:Label ID="lblBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label></h3>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="3" class="text-center">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/TTDCLogo.svg" Height="115" />
                        </th>
                    </tr>
                    <tr>
                        <td class="text-left font-weight-bold">
                            <asp:Image ID="imgOtherReceiptQR" runat="server" />
                        </td>
                        <td></td>
                        <td class="text-center font-weight-bold">
                            <h3>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label></h3>
                            <h4>Payment Type :
                                                <asp:Label ID="lblPaymentTypeName" runat="server" Text='<%# Eval("PaymentTypeName") %>'></asp:Label></h4>
                            <h4>Mobile :
                                                <asp:Label ID="lblCustomerMobileNo" runat="server" Text='<%# Eval("CustomerMobileNo") %>'></asp:Label></h4>
                        </td>
                    </tr>
                </table>

                <asp:DataList ID="DlOtherReceipt" runat="server" Width="100%" ShowHeader="true" ShowFooter="true"
                    OnItemDataBound="DataList1OnItemDataBound">
                    <HeaderTemplate>
                        <table class="tdb" runat="server" id="divrower" style="width: 100%;">
                            <tr class="tdb">
                                <th class="tdb" style="width: 40%; text-align: center">Service Name
                                </th>
                                <th class="tdb" style="width: 12%; text-align: center">Charge Per Item
                                </th>
                                <th class="tdb" style="width: 12%; text-align: center">No Of Item
                                </th>
                                <th class="tdb" style="width: 12%; text-align: center">Item Charge
                                </th>
                                <th class="tdb" style="width: 12%; text-align: center">Tax Amount
                                </th>
                                <th class="tdb" style="width: 12%; text-align: center">Net Amount
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="tdb">
                            <tr>
                                <td class="tdb" style="width: 40%; text-align: left">
                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                    -
                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                </td>
                                <td class="tdb" style="width: 12%; text-align: right">
                                    <asp:Label ID="lblChargePerItem" runat="server" Text='<%# Eval("ChargePerItem") %>'></asp:Label>
                                </td>
                                <td class="tdb" style="width: 12%; text-align: center">
                                    <asp:Label ID="lblNoOfItems" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                </td>
                                <td class="tdb" style="width: 12%; text-align: right">
                                    <asp:Label ID="lblItemCharge" runat="server" Text='<%# Eval("ItemCharge") %>'></asp:Label>
                                </td>
                                <td class="tdb" style="width: 12%; text-align: right">
                                    <asp:Label ID="lblTaxAmount" runat="server" Text='<%# Eval("TaxAmount") %>'></asp:Label>
                                </td>
                                <td class="tdb" style="width: 12%; text-align: right">
                                    <asp:Label ID="lblNetAmount" runat="server" Text='<%# Eval("NetAmount") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <table class="tdb" runat="server" id="divrower" style="width: 100%;">
                            <tr>
                                <th class="tdb" style="width: 40%; text-align: right">Total
                                </th>
                                <th class="tdb" style="width: 12%; text-align: right">
                                    <asp:Label ID="lblTotalChargePerItem" runat="server"></asp:Label>
                                </th>
                                <th class="tdb" style="width: 12%; text-align: center">
                                    <asp:Label ID="lblTotalNoOfItems" runat="server"></asp:Label>
                                </th>
                                <th class="tdb" style="width: 12%; text-align: right">
                                    <asp:Label ID="lblTotalItemCharge" runat="server"></asp:Label>
                                </th>
                                <th class="tdb" style="width: 12%; text-align: right">
                                    <asp:Label ID="lblTotalTaxAmount" runat="server"></asp:Label>
                                </th>
                                <th class="tdb" style="width: 12%; text-align: right">
                                    <asp:Label ID="lblTotalNetAmount" runat="server"></asp:Label>
                                </th>
                            </tr>
                        </table>
                    </FooterTemplate>
                </asp:DataList>
                <br />

                <table runat="server" style="width: 100%;">
                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.5px solid black;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-left font-weight-bold">
                            <h3>NET</h3>
                        </td>
                        <td colspan="2" class="text-right font-weight-bold">
                            <h3>₹
                                <asp:Label ID="lblGrandNetAmount" runat="server" Text='<%# Eval("GrandNetAmount") %>'></asp:Label></h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.5px solid black;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="text-center font-weight-bold" style="padding-right: 10px;">
                            <h5>GSTIN -
                                                <asp:Label ID="lblGST" runat="server" Text='<%# Eval("GSTNumber") %>'></asp:Label></h5>
                        </td>
                    </tr>
                </table>

                <table style="width: 100%">
                    <tr runat="server" id="trInsOther">
                        <td colspan="3" style="text-align: left;">
                            <span style="font-weight: 600;">Instructions :</span>
                            <asp:DataList ID="dtlisTicketInsOther" runat="server" Style="text-align: left;">
                                <ItemTemplate>
                                    <li>
                                        <asp:Label ID="lblInstructionDtl" runat="server" Text='<%# Bind("InstructionDtl") %>'></asp:Label>
                                    </li>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center;">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 48%">
                                        <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                    </td>
                                    <td>&#9986;
                                    </td>
                                    <td style="width: 48%">
                                        <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

               <table style="width: 100%; margin-top: 10px;">
                    <tr>
                        <td style="white-space: nowrap; text-align: right;">
                            <asp:Label ID="lblPrintDateTime" Font-Names="Roboto-Regular, sans-serif" Font-Size="8px" runat="server"></asp:Label>,
                        <asp:Label ID="lblPrintedByName" Font-Size="8px" Font-Names="Roboto-Regular, sans-serif" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </asp:Panel>

    <%-- <script>
        function printDiv() {

            var divToPrint = document.getElementById('DivIdToPrint');

            var newWin = window.open('', 'Print-Window');

            newWin.document.open();

            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');

            newWin.document.close();

            setTimeout(function () { newWin.close(); }, 10);

        }
    </script>--%>
        <%-- Newly implemented for CSRF Validation--%>
        <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
        <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

