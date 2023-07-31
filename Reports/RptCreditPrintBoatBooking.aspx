<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptCreditPrintBoatBooking.aspx.cs" Inherits="Reports_RptCreditPrintBoatBooking" %>


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

    <div class="form-body" runat="server" id="divShow" visible="false">
        <h5 class="pghr">Credit Boat Booking</h5>
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
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control frmDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
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
                <asp:GridView ID="GvBoatBooking" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="25000">
                    <columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <%#Container.DataItemIndex+1 %>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookedPin") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Date & Time" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Type" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingStatus" runat="server" Text='<%# Bind("BookingStatus") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblNetAmount" runat="server" Text='<%# Bind("NetAmount") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Print" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Print.svg" OnClick="ImgBtnEdit_Click" ToolTip="Print" />
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>
                    </columns>
                </asp:GridView>
            </div>
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
    <ajax:dragpanelextender id="DragPanelExtender2" runat="server" targetcontrolid="pnlBillService" draghandleid="pnlDrag3"></ajax:dragpanelextender>
    <ajax:modalpopupextender id="MpeBillService" runat="server" behaviorid="MpeBillService" targetcontrolid="HiddenField1" popupcontrolid="pnlBillService"
        backgroundcssclass="modalBackground">
    </ajax:modalpopupextender>

    <asp:Panel ID="pnlBillService" runat="server" CssClass="Msg" Style="display: none; height: 750px; width: 500px;">
        <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
            <div class="maincontent text-right">
                <h4>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Print.svg" Width="35px" ToolTip="Print" OnClick="ImageButton1_Click" />
                    <%--        <asp:ImageButton ID="ImageButton2" runat="server" OnClientClick="printDiv()" ImageUrl="~/images/Print.svg" Width="35px" ToolTip="Print" OnClick="ImageButton1_Click" />--%>
                    <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                </h4>
            </div>
            <div id='DivIdToPrint' style="padding: 15px 5px 15px 5px; overflow-y: scroll; height: 640px; width: 500px; text-align: center;">
                <asp:DataList ID="dtlistTicket" runat="server" OnItemDataBound="dtlistTicket_ItemDataBound" Width="100%">
                    <headertemplate>
                    </headertemplate>
                    <itemtemplate>
                        <table runat="server" id="divrower" style="width: 100%;">
                            <tr>
                                <td class="text-left font-weight-bold">
                                    <h3>PIN:<asp:Label ID="lblBookingPin" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                        <asp:Label ID="lblBookingId" runat="server" Text='<%# Eval("BookingId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblUniqueId" runat="server" Text='<%# Eval("UniqueId") %>' Visible="false"></asp:Label>
                                    </h3>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold">
                                    <h3>
                                        <asp:Label ID="Label12" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgQRBRoCopy" runat="server" />
                                </td>
                                <td colspan="2" class="font-weight-bold">
                                    <h4>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                        <br />
                                        Boat - Rower Copy </h4>
                                    <h6>
                                        <asp:Label ID="Label6" runat="server" Text='Booking Type:' Font-Bold="false"></asp:Label>
                                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("PremiumStatus") %>' Font-Bold="true"></asp:Label>
                                    </h6>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h6>Booking Id </h6>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h6>
                                        <asp:Label ID="Label22" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                    </h6>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h6>Boat Type</h6>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h6>
                                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("BoatType") %>'></asp:Label>
                                        <asp:Label ID="Label14" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label>
                                    </h6>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h4>Rower Amount</h4>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h4>₹
                                         <asp:Label ID="lblActualRowerCharge" runat="server" Text='<%# Eval("ActualRowerCharge") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" style="text-align: center;">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 48%">
                                                <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                            </td>
                                            <td>&#9986;</td>
                                            <td style="width: 48%">
                                                <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                        </table>

                        <table style="width: 100%;" id="divBPass" runat="server">
                            <tr>
                                <td class="text-left font-weight-bold">
                                    <h3>PIN:<asp:Label ID="Label27" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                    </h3>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold">
                                    <h3>
                                        <asp:Label ID="Label23" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgQRBBoCopy" runat="server" />
                                </td>
                                <td colspan="2" class="font-weight-bold">
                                    <h4>
                                        <asp:Label ID="Label25" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                        <br />
                                        Boat - Boarding Pass</h4>

                                </td>
                            </tr>

                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h6>Booking Id </h6>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h6>
                                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                    </h6>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h6>Boat Type</h6>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h6>
                                        <asp:Label ID="Label19" runat="server" Text='<%# Eval("BoatType") %>'></asp:Label>
                                        <asp:Label ID="Label26" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label>
                                    </h6>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h6>Duration</h6>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h6>
                                        <asp:Label ID="Label15" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                                        (Mins)</h6>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h4>NET </h4>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h4>₹
                                      <asp:Label ID="lblActualNetAmount" runat="server" Text='<%# Eval("ActualNetAmount") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" style="text-align: center;">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 48%">
                                                <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                            </td>
                                            <td>&#9986;</td>
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


                <asp:DataList ID="DLReceipt" runat="server" OnItemDataBound="DLReceipt_ItemDataBound" Width="100%">
                    <headertemplate>
                    </headertemplate>
                    <itemtemplate>
                        <table style="width: 100%;" runat="server">
                            <tr>
                                <td class="text-left font-weight-bold">
                                    <h3><%--PIN:<asp:Label ID="Label11" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>--%>
                                    </h3>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold">
                                    <h3>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="text-center">
                                    <h1>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/TTDCLogo.svg" Height="115" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="text-center">
                                    <h2>
                                        <asp:Label ID="lblBillBoatHouse" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                    </h2>
                                </td>
                            </tr>
                            <tr>
                                <td></h1><asp:Image ID="imgBoatBulkReceiptQR" runat="server" Visible="false" />
                                </td>
                                <td colspan="2" style="text-align: center;">

                                    <h4 style="text-align: center; margin-right: 100px">
                                        <asp:Label ID="Label7" runat="server" Text='Booking Type:' Font-Bold="false"></asp:Label>
                                        <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Eval("PremiumStatus") %>' Font-Bold="true"></asp:Label>
                                        <br />

                                        <asp:Label ID="Label9" runat="server" Text='Booking Id : ' Font-Bold="false"></asp:Label>
                                        <asp:Label ID="lblBillBookingId" runat="server" Text='<%# Eval("BookingId") %>' Font-Bold="true"></asp:Label>
                                        <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" class="text-left"></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <h5>
                                        <asp:Label ID="Label5" runat="server" Text='Boat Receipt - ' Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                        <asp:Label ID="lblBookingID" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                    </h5>
                                </td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="text-left font-weight-bold">Payment Type</td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <asp:Label ID="Label31" runat="server" Text='<%# Eval("PaymentTypeName") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="text-left font-weight-bold">Boat Charge </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <asp:Label ID="lblActualBoatCharge" runat="server" Text='<%# Eval("ActualBoatCharge") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" visible="true">
                                <td class="text-left font-weight-bold">Rower Charge</td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <asp:Label ID="lblActualRowerCharge" runat="server" Text='<%# Eval("ActualRowerCharge") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.5px solid black;" />
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left font-weight-bold">
                                    <h4>NET </h4>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h4>₹
                                            <asp:Label ID="lblActualNetAmount" runat="server" Text='<%# Eval("ActualNetAmount") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.5px solid black;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <h5>GSTIN -
                                                        <asp:Label ID="lblGST" runat="server" Text='<%# Eval("GSTNumber") %>'></asp:Label>
                                    </h5>
                                </td>
                            </tr>


                            <tr runat="server" id="trBoatInsBulk">
                                <td colspan="3" style="text-align: left;">
                                    <span style="font-weight: 600;">Instructions :</span>
                                    <asp:DataList ID="dtlisTicketInsBulk" runat="server" Style="text-align: left;">
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
                                            <td>&#9986;</td>
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
            </div>
        </asp:Panel>
    </asp:Panel>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>
