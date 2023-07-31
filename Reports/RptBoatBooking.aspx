<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Reports/RptBoatBooking.aspx.cs" Inherits="Boating_RptBoatBooking" %>

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

    <div class="form-body">
        <h5 class="pghr">Boat Booking</h5>
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
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />
                </div>
            </div>
        </div>

        <div runat="server" id="divGridList" visible="false">
            <div class="table-responsive">
                <div style="text-align: right;" runat="server" id="divSearch">
                    Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="20px" placeholder="Enter Booking Id/ Pin" AutoComplete="off" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                </div>
                <div runat="server" style="max-height: 550px; min-height: 550px; overflow: auto; overflow-x: hidden">
                    <asp:GridView ID="GvBoatBooking" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" DataKeyNames="BookingId,Status" PageSize="25000">
                        <columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Date & Time" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Customer Mobile" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingStatus" runat="server" Text='<%# Bind("BookingStatus") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Initial Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblInitBillAmount" runat="server" Text='<%# Bind("InitBillAmount") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Other Service Amount" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblOtherService" runat="server" Text='<%# Bind("OtherService") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Print" HeaderStyle-CssClass="grdHead" Visible="false">
                                <itemtemplate>
                                    <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                        runat="server" Font-Bold="true" ImageUrl="~/images/Print.svg" OnClick="ImgBtnEdit_Click" ToolTip="Print" />
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                        </columns>
                        <headerstyle cssclass="gvHead" />
                        <alternatingrowstyle cssclass="gvRow" />
                        <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                    </asp:GridView>

                    <div runat="server" id="divprevnext" style="text-align: left;">
                        <asp:Button ID="back" runat="server" CssClass="btn btn-color mg" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                        &nbsp
                                 <asp:Button ID="Next" Visible="true" CssClass="btn btn-color mg" runat="server" Text="Next →" OnClick="Next_Click" />
                        &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />
                    </div>
                </div>

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
                                        <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Eval("BoatReferenceNo") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblCheckDate" runat="server" Text='<%# Eval("CheckDate") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Eval("TripEndTime") %>' Visible="false"></asp:Label>
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
                                <td colspan="2" class="text-left font-weight-bold" style="white-space: nowrap;">
                                    <h3>
                                        <asp:Label ID="lblResDateHeading" Width="100%" Visible="false" runat="server" Text="Rescheduled On"></asp:Label>
                                    </h3>
                                </td>
                                <td class="text-right font-weight-bold">
                                    <h3>
                                        <asp:Label ID="lblRescheduledDate" runat="server" Visible="false" Text='<%# Eval("RescheduledDate") %>'></asp:Label>
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
                                    <h6>Boat Number</h6>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h6>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("ActualBoatNum") %>'></asp:Label>
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
                                                        <asp:Label ID="lblBillRowerCharge" runat="server" Text='<%# Eval("InitRowerCharge") %>'></asp:Label>
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
                                <td colspan="2" class="text-left font-weight-bold" style="white-space: nowrap;">
                                    <h3>
                                        <asp:Label ID="lblBPResHdg" Width="100%" Visible="false" runat="server" Text="Rescheduled On"></asp:Label>
                                    </h3>
                                </td>
                                <td class="text-right font-weight-bold">
                                    <h3>
                                        <asp:Label ID="lblBPResDate" runat="server" Visible="false" Text='<%# Eval("RescheduledDate") %>'></asp:Label>
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
                                    <h6>
                                        <asp:Label ID="Label11" runat="server" Text='Booking Type:' Font-Bold="false"></asp:Label>
                                        <asp:Label ID="Label16" runat="server" Text='<%# Eval("PremiumStatus") %>' Font-Bold="true"></asp:Label>
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
                                    <h6>Boat Number</h6>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h6>
                                        <asp:Label ID="lblActualBoatNum" runat="server" Text='<%# Eval("ActualBoatNum") %>'></asp:Label>
                                    </h6>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h6>Expected Time</h6>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h6>
                                        <asp:Label ID="lblExpectedTime" runat="server" Text='<%# Eval("ExpectedTime") %>'></asp:Label>
                                    </h6>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                    <h4>NET </h4>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h4>₹
                                                        <asp:Label ID="Label28" runat="server" Text='<%# Eval("InitNetAmount") %>'></asp:Label>
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

                <asp:DataList ID="dtlistTicketOther" runat="server" OnItemDataBound="dtlistTicketOther_ItemDataBound" Width="100%">
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
                                        <asp:Label ID="lblOthBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgOtherServiceQR" runat="server" />
                                </td>
                                <td colspan="2">
                                    <h2>
                                        <asp:Label ID="lblOthBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                    </h2>
                                    <h3>
                                        <asp:Label ID="lblOthServiceName" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                    </h3>
                                    <%-- <h3>  <asp:Label ID="lblBillOBookingType" runat="server" Text='<%# Eval("BookingType") %>'></asp:Label> </h3>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <h4>NET : ₹
                                                        <asp:Label ID="lblOtherNetAmount" runat="server" Text='<%# Eval("NetAmount") %>'></asp:Label>
                                    </h4>
                                </td>

                                <td style="text-align: right;">
                                    <h4>Total Tickets :
                                                        <asp:Label ID="lblOtherNoOfItems" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>

                            <tr runat="server" visible="false">
                                <td class="text-left">Service Fare</td>
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
                                    <asp:Label ID="lblBillOServiceId" runat="server" Text='<%# Eval("ServiceId") %>' Visible="false"></asp:Label>
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
                                        <img id="Image1"  src="<%=Session["CorpLogo"].ToString()== "" ? "../images/TTDCLogo.svg" : Session["CorpLogo"].ToString()  %>" Height="115" />
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
                                <td class="text-left font-weight-bold" colspan="3">
                                    <h3>
                                        <asp:Label ID="lblBillCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                    </h3>
                                    <p>
                                        <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Eval("CustomerMobile") %>' Visible="true"></asp:Label>
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td class="text-left">Customer </td>
                                <td></td>
                                <td class="text-right font-weight-bold">

                                    <asp:Label ID="lblCustomerid" runat="server" Text='<%# Eval("CustomerId") %>' Visible="false"></asp:Label>
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
                                    <asp:Label ID="lblBillBoatCharge" runat="server" Text='<%# Eval("BFDInitBoatCharge") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" visible="true">
                                <td class="text-left font-weight-bold">Rower Charge</td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <asp:Label ID="lblRowerCharge" runat="server" Text='<%# Eval("BFDInitRowerCharge") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AdditionalTicketCharges").ToString() == "0.00"? false: true %>'>
                                <td class="text-left font-weight-bold">Additional Charges</td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("AdditionalTicketCharges") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="text-left font-weight-bold">Deposit Amount</td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <asp:Label ID="lblBillDeposit" runat="server" Text='<%# Eval("BoatDeposit") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="text-left font-weight-bold">Other Service</td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <asp:Label ID="Label29" runat="server" Text='<%# Eval("OtherService") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="text-left font-weight-bold">Tax Amount</td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <asp:Label ID="lblBillCGST" runat="server" Text='<%# Eval("BFDTaxAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.5px solid black;" />
                                </td>
                            </tr>

                            <tr runat="server" visible='<%# Eval("InitOfferAmount").ToString() == "0.00"? false: true %>'>
                                <td class="text-left font-weight-bold">
                                    <h4>Total</h4>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h4>
                                        <asp:Label ID="Label30" runat="server" Text='<%# Eval("BFDInitNetAmount") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>

                            <tr runat="server" visible='<%# Eval("InitOfferAmount").ToString() == "0.00"? false: true %>'>
                                <td class="text-left font-weight-bold">
                                    <h4>Discount</h4>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h4>
                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("InitOfferAmount") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left font-weight-bold">
                                    <h4>NET </h4>
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                    <h4>₹
                                            <asp:Label ID="lblBillinitNetAmount" runat="server" Text='<%# Eval("InitNetAmount") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.5px solid black;" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TotalRefund").ToString() == "0.00"? false: true %>'>
                                <td runat="server" class="text-left font-weight-bold" style="font-size: 21px">Total Refund 
                                </td>
                                <td></td>
                                <td class="text-right font-weight-bold" runat="server" style="font-size: 21px">₹
                                <asp:Label ID="lblTotalRefund" runat="server" Text='<%# Eval("TotalRefund") %>'></asp:Label>
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


    <%--<script>
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

