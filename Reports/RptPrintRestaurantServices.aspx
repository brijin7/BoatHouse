<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptPrintRestaurantServices.aspx.cs" Inherits="Reports_RptPrintRestaurantServices" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />

    <style>
        .tdb {
            border-collapse: collapse;
            border: 1px solid black;
            width: 100%;
            margin: -1px;
        }
    </style>

    <div class="form-body">
        <h5 class="pghr">Restaurant Services</h5>
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
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label2" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />
                </div>

            </div>
        </div>

        <div runat="server" id="divGridList" visible="false">
            <%--<div class="table-responsive">--%>
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
            <div style="text-align: right;">
                <div runat="server" id="Search" style="padding-bottom: 10px">
                    Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="16px" AutoComplete="off" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true" Placeholder="Enter Booking Id"></asp:TextBox>
                </div>
                <asp:GridView ID="GVRestaurant" runat="server" AllowPaging="True"
                    CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId,Status" PageSize="25000">
                    <columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%#Bind("RowNumber") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date & Time" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Service Fare" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblServiceFare" runat="server" Text='<%# Bind("ServiceFare") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblCGST" runat="server" Text='<%# Bind("TaxAmount") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblNetAmount" runat="server" Text='<%# Bind("NetAmount") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Print" HeaderStyle-CssClass="grdHead" Visible="false">
                            <itemtemplate>
                                <asp:ImageButton ID="ImgBtnPrint" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Print.svg" OnClick="ImgBtnPrint_Click" ToolTip="Print" />
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                    </columns>
                    <headerstyle cssclass="gvHead" />
                    <alternatingrowstyle cssclass="gvRow" />
                    <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                </asp:GridView>
                <%--     Newly Added--%>
                <div style="text-align: left;">
                    <asp:Button ID="back" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                    &nbsp
                        <asp:Button ID="Next" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="Next_Click" />

                </div>
                <div style="text-align: left;">
                    <asp:Button ID="backSearch" runat="server" CssClass="btn btn-color" Visible="false" Text="← Previous" Enabled="false" OnClick="backSearch_Click" />
                    &nbsp
                        <asp:Button ID="NextSearch" Visible="false" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="NextSearch_Click" />
                    &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />
                </div>
            </div>
            <%--</div>--%>
            <asp:HiddenField runat="server" ID="hfreason" />
            <ajax:dragpanelextender id="DragPanelExtender4" runat="server" targetcontrolid="pnlRsn" draghandleid="pnlDrag3"></ajax:dragpanelextender>
            <ajax:modalpopupextender id="Mpepnlrsn" runat="server" behaviorid="Mpepnlrsn" targetcontrolid="hfreason" popupcontrolid="pnlRsn"
                backgroundcssclass="modalBackground">
            </ajax:modalpopupextender>

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
                                        ValidationGroup="BoatHouseName" InitialValue="0" SetFocusOnError="True" CssClass="vError">
                                        Select Reason</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3" style="top: 30px;">
                                    <div class="table-responsive">
                                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                            <asp:Button runat="server" Text="Print" ID="RsnSubmit" OnClick="RsnSubmit_Click" CssClass="btn btn-primary " ValidationGroup="BoatHouseName" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </div>


        <div class="row">
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <ajax:dragpanelextender id="DragPanelExtender2" runat="server" targetcontrolid="pnlBillService" draghandleid="pnlDrag3"></ajax:dragpanelextender>
            <ajax:modalpopupextender id="MpeBillService" runat="server" behaviorid="MpeBillService" targetcontrolid="HiddenField1" popupcontrolid="pnlBillService"
                backgroundcssclass="modalBackground">
            </ajax:modalpopupextender>

            <asp:Panel ID="pnlBillService" runat="server" CssClass="Msg" Style="display: none; height: 750px; width: 500px;">
                <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
                    <div class="maincontent text-right">
                        <h4>
                            <asp:ImageButton ID="ImageButton1" OnClick="ImageButton1_Click" runat="server" ImageUrl="~/images/Print.svg" Width="35px" ToolTip="Print" />
                            <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                        </h4>
                    </div>

                    <div id='DivIdToPrint' style="padding: 15px 5px 15px 5px; overflow-y: scroll; height: 640px; width: 500px; text-align: center;">

                        <asp:DataList ID="dtlistRestTicketOther" runat="server" OnItemDataBound="dtlistRestTicketOther_ItemDataBound" Width="100%">
                            <headertemplate>
                            </headertemplate>
                            <itemtemplate>
                                <table style="width: 100%">
                                    <tr>
                                        <td class="text-left font-weight-bold">
                                            <h3>
                                                <asp:Label ID="lblBillOBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                            </h3>
                                        </td>
                                        <td></td>
                                        <td class="text-right font-weight-bold">
                                            <h3>
                                                <asp:Label ID="lblBillOBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                            </h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Image ID="imgOtherQR" runat="server" />
                                        </td>
                                        <td colspan="2">
                                            <h2>
                                                <asp:Label ID="Label18" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                            </h2>
                                            <h3>
                                                <asp:Label ID="Label21" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                            </h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td class="text-right font-weight-bold">
                                            <h4>Total Items :
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
                            </itemtemplate>
                        </asp:DataList>

                        <table runat="server" style="width: 100%;">
                            <tr>
                                <td class="text-left font-weight-bold">
                                    <h3>
                                        <asp:Label ID="lblBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                    </h3>
                                </td>
                                <td colspan="2" class="text-right font-weight-bold">
                                    <h3>
                                        <asp:Label ID="lblBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </h3>
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
                                        <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                    </h3>
                                    <h4>Payment Type :
                                                <asp:Label ID="lblPaymentTypeName" runat="server" Text='<%# Eval("PaymentTypeName") %>'></asp:Label>
                                    </h4>
                                    <h4>Mobile :
                                                <asp:Label ID="lblCustomerMobileNo" runat="server" Text='<%# Eval("CustomerMobileNo") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>
                        </table>

                        <asp:DataList ID="DlOtherReceipt" runat="server" Width="100%" ShowHeader="true" ShowFooter="true"
                            OnItemDataBound="DataList1OnItemDataBound">
                            <headertemplate>
                                <table class="tdb" runat="server" id="divrower" style="width: 100%;">
                                    <tr class="tdb">
                                        <th class="tdb" style="width: 40%; text-align: center">Item Name
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
                            </headertemplate>
                            <itemtemplate>
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
                            </itemtemplate>
                            <footertemplate>
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
                            </footertemplate>
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
                                <asp:Label ID="lblGrandNetAmount" runat="server" Text='<%# Eval("GrandNetAmount") %>'></asp:Label>
                                    </h3>
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
                                                <asp:Label ID="lblGST" runat="server" Text='<%# Eval("GSTNumber") %>'></asp:Label>
                                    </h5>
                                </td>
                            </tr>
                        </table>

                        <table style="width: 100%">
                            <tr runat="server" id="trInsOther">
                                <td colspan="3" style="text-align: left;">
                                    <span style="font-weight: 600;">Instructions :</span>
                                    <asp:DataList ID="dtlisTicketInsOther" runat="server" Style="text-align: left;">
                                        <itemtemplate>
                                            <li>
                                                <asp:Label ID="lblInstructionDtl" runat="server" Text='<%# Bind("InstructionDtl") %>'></asp:Label>
                                            </li>
                                        </itemtemplate>
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
                                    <asp:Label ID="lblPrintDateTime" Font-Names="Roboto-Regular, sans-serif" Font-Size="8px" runat="server"></asp:Label>
                                    ,
                                    <asp:Label ID="lblPrintedByName" Font-Size="8px" Font-Names="Roboto-Regular, sans-serif" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </div>

    </div>
    <%--  <script type="text/javascript">
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

