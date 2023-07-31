<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintBoat.aspx.cs" Inherits="Boating_PrintBoat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        /*tr { 
          line-height: 0px;
          border:0px solid blue;
          }*/
        td {
            line-height: 8px;
            /*border:1px solid red;*/
        }

            td p {
                margin: 0;
                border: 1px solid black;
                background-color: yellow;
            }

        .f14CNL {
            font-size: 13px;
            /*font-family: Calibri;*/
            font-family: Roboto-Regular, sans-serif;
            font-weight: bold;
            text-align: left;
            padding-top: 5px;
        }

        .f14CNL1 {
            font-size: 13px;
            /*font-family: Calibri;*/
            font-family: Roboto-Regular, sans-serif;
            font-weight: bold;
            text-align: left;
            padding-top: 5px;
            width: 56%;
        }

        .f14CNR {
            font-size: 13px;
            /*font-family: Calibri;*/
            font-family: Roboto-Regular, sans-serif;
            font-weight: normal;
            text-align: right;
            /* padding-right: 10px;*/
            padding-top: 5px;
        }

        .f14CNR1 {
            font-size: 13px;
            /*font-family: Calibri;*/
            font-family: Roboto-Regular, sans-serif;
            font-weight: normal;
            text-align: right;
            /* padding-right: 10px;*/
            padding-top: 5px;
        }

        .f16CN {
            font-size: 16px;
            /*font-family: Calibri;*/
            font-family: Roboto-Regular, sans-serif;
            font-weight: normal;
            line-height: 14px;
            padding-top: 5px;
        }

        .f16CB {
            font-size: 16px;
            /*font-family: Calibri;*/
            font-family: Roboto-Regular, sans-serif;
            font-weight: bold;
            line-height: 14px;
            padding-top: 5px;
        }

        .f14CBL {
            font-size: 13px;
            /*font-family: Calibri;*/
            font-family: Roboto-Regular, sans-serif;
            font-weight: bold;
            text-align: left;
            padding-top: 5px;
        }

        .f14CBR {
            font-size: 13px;
            /*font-family: Calibri;*/
            font-family: Roboto-Regular, sans-serif;
            font-weight: bold;
            text-align: right;
            padding-top: 5px;
        }
    </style>
    <style>
        .insStyleHeader {
            font-size: 16px;
            font-family: Roboto-Regular, sans-serif;
            font-weight: bold;
        }

        .insStyle {
            font-size: 12px;
            font-family: Roboto-Regular, sans-serif;
            font-weight: normal;
            line-height: 10px;
        }

        .rdlWhiteSpace {
            white-space: nowrap;
        }
    </style>

    <style>
        .tdb {
            border: 0px solid black;
            /* border-collapse: collapse;
            border: 0px solid black;*/
            /*   width: 100%;
           margin: -1px;*/
        }
    </style>
    <script
        src="https://code.jquery.com/jquery-3.6.4.min.js"
        integrity="sha256-oP6HI9z1XaZNBrJURtCoUT5SUnxFr8s3BzRl+cbzUq8="
        crossorigin="anonymous"></script>
    <%--    <script src="../Scripts/jquery1.7.1.js"></script>--%>
    <%--   <script type="text/javascript" src="https://code.jquery.com/jquery-1.7.1.min.js"></script>--%>
</head>

<body style="width: 250px" onload="">
    <form id="form1" runat="server">
        <div class="f14CNR">
            <%--<asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="javascript:printDiv('div2Print')" />--%>
            <input id="btnPrint" type="button" value="Print" onclick="javascript: printDiv('div2Print')" style="display: none" />
            <asp:Button ID="btnBack" runat="server" Text="Back" class="btn btn-primary btnFinal" OnClick="btnBack_Click" Style="display: none" />
        </div>
        <div id="div2Print" runat="server">
            <div runat="server" id="divBoat" visible="false">
                <asp:DataList ID="dtlistTicket" runat="server" OnItemDataBound="dtlistTicket_ItemDataBound" Width="100%">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table runat="server" id="divrower" style="width: 100%;">
                            <tr>
                                <td class="f14CNL">PIN:<asp:Label ID="lblBookingPin" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Eval("BookingId") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Eval("BoatReferenceNo") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblCheckDate" runat="server" Text='<%# Eval("CheckDate") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Eval("TripEndTime") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Eval("BoatHouseId") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Eval("BoatTypeId") %>' Visible="false"></asp:Label>
                                </td>
                                <td></td>
                                <td class="f14CNR" style="padding-top: 12px">DATE:
                                    <asp:Label ID="Label12" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="white-space: nowrap;">
                                    <asp:Label ID="lblResDateHeading" CssClass="f14CNL" Width="100%" Visible="false" runat="server" Text="Rescheduled On"></asp:Label>
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblRescheduledDate" runat="server" Visible="false" Text='<%# Eval("RescheduledDate") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Image ID="imgQRBRoCopy" runat="server" Height="80px" />
                                </td>
                                <td colspan="2" class="f16CN">SL.No : 
                                    <asp:Label ID="Label17" runat="server" Text='<%# Eval("BoatTypeSno") %>' Font-Size="18px"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                    Boat - Rower Copy<br />
                                    <asp:Label ID="Label6" runat="server" Text='Booking :'></asp:Label>
                                    <asp:Label ID="Label10" runat="server" Text='<%# Eval("PremiumStatus") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Booking Id
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label22" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Boat Type
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label13" runat="server" Text='<%# Eval("BoatType") %>'></asp:Label>
                                    <asp:Label ID="Label14" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Boat Number
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("ActualBoatNum") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL" style="white-space: nowrap;">Rower Amount
                                </td>
                                <td></td>
                                <td class="f14CNR">₹
                                <asp:Label ID="lblBillRowerCharge" runat="server" Text='<%# Eval("InitRowerCharge") %>'></asp:Label>

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
                                <td class="f14CNL">PIN:<asp:Label ID="Label27" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                </td>
                                <td></td>

                                <td class="f14CNR" style="padding-top: 12px">DATE:
                                    <asp:Label ID="Label23" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="white-space: nowrap;">
                                    <asp:Label ID="lblBPResHdg" CssClass="f14CNL" Width="100%" Visible="false" runat="server" Text="Rescheduled On"></asp:Label>
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblBPResDate" runat="server" Visible="false" Text='<%# Eval("RescheduledDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgQRBBoCopy" runat="server" Height="80px" />
                                </td>
                                <td colspan="2" class="f16CN">SL.No :
                                    <asp:Label ID="Label15" runat="server" Text='<%# Eval("BoatTypeSno") %>' Font-Size="18px"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label25" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                    Boat - Boarding Pass
                                    <br />
                                    <asp:Label ID="Label18" runat="server" Text='Booking :'></asp:Label>
                                    <asp:Label ID="Label20" runat="server" Text='<%# Eval("PremiumStatus") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table style="width: 100%;" id="divBPass1" runat="server">
                            <tr>
                                <td class="f14CNL">Booking Id
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Boat Type
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label19" runat="server" Text='<%# Eval("BoatType") %>'></asp:Label>
                                    <asp:Label ID="Label26" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Duration
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="OneRoundYCD" runat="server" Visible="false">1 Round /</asp:Label>
                                    <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                                    <span>(Mins)</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Boat Number
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblActualBoatNum" runat="server" Text='<%# Eval("ActualBoatNum") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Exp Time Slot
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblExpectedTime" runat="server" Text='<%# Eval("ExpectedTime") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">NET 
                                </td>
                                <td class="f14CNR">₹
                                 <asp:Label ID="Label28" runat="server" Text='<%# Eval("InitNetAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
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
                    </ItemTemplate>
                </asp:DataList>
                <asp:DataList ID="dtlistTicketOther" runat="server" OnItemDataBound="dtlistTicketOther_ItemDataBound" Width="100%">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td class="f14CNL">
                                    <asp:Label ID="lblBillOBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblOthBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgOtherServiceQR" runat="server" Height="80px" />
                                </td>
                                <td colspan="2" class="f16CN">
                                    <asp:Label ID="lblOthBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                    <asp:Label ID="lblOthServiceName" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table style="width: 100%;">
                            <tr>
                                <td class="f14CNL">NET : ₹
                                <asp:Label ID="lblOtherNetAmount" runat="server" Text='<%# Eval("NetAmount") %>'></asp:Label>
                                </td>

                                <td class="f14CNR">Total Tickets :
                                  <asp:Label ID="lblOtherNoOfItems" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <tr runat="server" visible="false">
                            <td class="f14CNR">Service Fare</td>
                            <td></td>
                            <td>
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

                        <table style="width: 100%;">

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
                    </ItemTemplate>
                </asp:DataList>
                <asp:DataList ID="DLReceipt" runat="server" OnItemDataBound="DLReceipt_ItemDataBound" Width="100%">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="3" class="f14CNR"></td>
                            </tr>
                            <tr>
                                <td colspan="3" class="f16CB" style="text-align: center;">
                                    <asp:Label ID="lblBillBoatHouse" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td runat="server" visible="false">
                                    <asp:Image ID="imgBoatBulkReceiptQR" runat="server" Height="80px" />
                                </td>

                                <td colspan="2" class="f16CN" style="text-align: center;">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    <br />
                                    <asp:Label ID="Label7" runat="server" Text='Booking :'></asp:Label>
                                    <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Eval("PremiumStatus") %>'></asp:Label>
                                    <br />
                                    <asp:Label ID="Label9" runat="server" Text='Booking Id : '></asp:Label>
                                    <asp:Label ID="lblBillBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr runat="server">
                                <td colspan="3" class="f16CN">
                                    <asp:Label ID="lblBillCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                    <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Eval("CustomerMobile") %>' Visible="true"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server">
                                <td colspan="3" class="f16CN">
                                    <asp:Label ID="Label11" runat="server" Text='Customer GST: ' Visible='<%# Eval("CustomerGSTNo").ToString() == ""? false: true %>'></asp:Label>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("CustomerGSTNo") %>' Visible='<%# Eval("CustomerGSTNo").ToString() == ""? false: true %>'></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td class="f14CNL">Customer </td>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblCustomerid" runat="server" Text='<%# Eval("CustomerId") %>' Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.5px solid black; margin: 0px;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="f16CB" style="text-align: center; padding-top: 2px;">
                                    <asp:Label ID="Label5" runat="server" Text='Boat Receipt - ' Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblBookingID" runat="server" Text='<%# Eval("BookingId") %>' Font-Size="18px"></asp:Label>
                                </td>
                            </tr>
                        </table>


                        <table style="width: 100%;">

                            <tr runat="server" visible="true">
                                <td class="f14CNL">Payment Type</td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label31" runat="server" Text='<%# Eval("PaymentTypeName") %>'></asp:Label></td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="f14CNL">Boat Charge </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblBillBoatCharge" runat="server" Text='<%# Eval("BFDInitBoatCharge") %>'></asp:Label></td>
                            </tr>
                            <tr runat="server" visible="true">
                                <td class="f14CNL">Rower Charge</td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblRowerCharge" runat="server" Text='<%# Eval("BFDInitRowerCharge") %>'></asp:Label></td>
                            </tr>

                            <tr runat="server" visible='<%# Eval("AdditionalTicketCharges").ToString() == "0.00"? false: true %>'>
                                <td class="f14CNL">Additional Charges</td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label16" runat="server" Text='<%# Eval("AdditionalTicketCharges") %>'></asp:Label></td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="f14CNL">Deposit Amount</td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblBillDeposit" runat="server" Text='<%# Eval("BoatDeposit") %>'></asp:Label></td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="f14CNL">Other Service</td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label29" runat="server" Text='<%# Eval("OtherService") %>'></asp:Label></td>
                            </tr>

                            <tr runat="server" visible="true">
                                <td class="f14CNL">Tax Amount</td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblBillCGST" runat="server" Text='<%# Eval("BFDTaxAmount") %>'></asp:Label></td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.1px solid black; margin: 0px;" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InitOfferAmount").ToString() == "0.00"? false: true %>'>
                                <td class="f14CNL">Total
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label30" runat="server" Text='<%# Eval("BFDInitNetAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InitOfferAmount").ToString() == "0.00"? false: true %>'>
                                <td class="f14CNL">Discount
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("InitOfferAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">NET
                                </td>
                                <td></td>
                                <td class="f14CNR">₹
                                <asp:Label ID="lblBillinitNetAmount" runat="server" Text='<%# Eval("InitNetAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.2px solid black; margin: 0px;" />
                                </td>
                            </tr>

                            <tr runat="server" visible='<%# Eval("BalanceAmount").ToString() == "0"? false: true %>'>
                                <td runat="server" class="f14CNL">Balance in Advance
                                </td>
                                <td></td>
                                <td class="f14CNR">₹
                                <asp:Label ID="lblBoatBalanceAmount" runat="server" Text='<%# Eval("BalanceAmount") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr runat="server" visible='<%# Eval("TotalRefund").ToString() == "0.00"? false: true %>'>
                                <td runat="server" class="f14CNL" style="font-size: large">Refund Amount
                                </td>
                                <td></td>
                                <td class="f14CNR" style="font-size: large; font-weight: bold;">₹
                                <asp:Label ID="lblTotalRefund" runat="server" Text='<%# Eval("TotalRefund") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr runat="server" visible='<%# Eval("BalanceAmount").ToString() == "0"? false: true %>'>
                                <td colspan="3">
                                    <hr style="border: 0.1px solid black; margin: 0px; margin-top: 5px;" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" class="f16CN" style="text-align: center">GSTIN -
                                <asp:Label ID="lblGSTBoat" runat="server" Text='<%# Eval("GSTNumber") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trBoatInsBulk">
                                <td colspan="3" style="text-align: left;">
                                    <span class="insStyleHeader">Instructions :</span>
                                    <asp:DataList ID="dtlisTicketInsBulk" runat="server" Style="text-align: left; margin-top: 10px;">
                                        <ItemTemplate>
                                            <li>
                                                <asp:Label ID="lblInstructionDtl" runat="server" Text='<%# Bind("InstructionDtl") %>' class="insStyle"></asp:Label>
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
                                                <p style="border: 0.2px dashed black; border-collapse: collapse; margin: 0px;" />
                                            </td>
                                            <td>&#9986;</td>
                                            <td style="width: 48%">
                                                <p style="border: 0.5px dashed black; border-collapse: collapse; margin: 0px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div id="divBookingReschedule" runat="server" visible="false">
                <asp:DataList ID="dtlistReschedule" runat="server" Width="100%">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table runat="server" id="tblReschedule" style="width: 100%;">
                            <tr>
                                <td class="f14CNL rdlWhiteSpace">PIN:<asp:Label ID="lblRdlBookingPin" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                </td>
                                <td></td>
                                <td class="f14CNR rdlWhiteSpace">
                                    <asp:Label ID="lblRdlRescheduledDate" runat="server" Text='<%# Eval("RescheduledDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="f16CN" style="text-align: center; font-weight: bold;">
                                    <asp:Label ID="lblRdlBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="f16CN" style="text-align: center;">Boat - Rescheduled Ticket
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Booking Id </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblRdlBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL rdlWhiteSpace">Old Booking Date </td>
                                <td></td>
                                <td class="f14CNR rdlWhiteSpace">
                                    <asp:Label ID="lblRdlOldBookingDate" runat="server" Text='<%# Eval("OldBookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL rdlWhiteSpace">Old Time Slot </td>
                                <td></td>
                                <td class="f14CNR rdlWhiteSpace">
                                    <asp:Label ID="lblRdlOldTimeSlot" runat="server" Text='<%# Eval("OldTimeSlot") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL rdlWhiteSpace">New Booking Date </td>
                                <td></td>
                                <td class="f14CNR rdlWhiteSpace">
                                    <asp:Label ID="lblRdlNewBookingDate" runat="server" Text='<%# Eval("NewBookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL rdlWhiteSpace">New Time Slot </td>
                                <td></td>
                                <td class="f14CNR rdlWhiteSpace">
                                    <asp:Label ID="lblRdlNewTimeSlot" runat="server" Text='<%# Eval("NewTimeSlot") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL rdlWhiteSpace">Rescheduled Charge </td>
                                <td></td>
                                <td class="f14CNR rdlWhiteSpace">₹
                                    <asp:Label ID="lblRedlRescheduledCharge" runat="server" Text='<%# Eval("RescheduledTotalCharge", "{0:N2}").ToString() %>'></asp:Label>
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
                    </ItemTemplate>
                </asp:DataList>
                <asp:DataList ID="dtlistRescheduleSummary" runat="server" Width="107%">
                    <HeaderTemplate></HeaderTemplate>
                    <ItemTemplate>
                        <table runat="server" id="tblSumReschedule" style="width: 100%;">
                            <tr>
                                <td colspan="3" class="f16CN" style="text-align: center; font-weight: bold;">
                                    <asp:Label ID="lblRdlSumBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="f14CNR rdlWhiteSpace" style="text-align: center;">
                                    <asp:Label ID="lblRdlSumRescheduledDate" runat="server" Text='<%# Eval("RescheduledDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="f14CNR" style="text-align: center;">Booking Id :
                                    <asp:Label ID="lblRdlBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 10px;" class="f14CNR"></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.1px solid black; margin: 0px;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="f16CB" style="text-align: center; padding-top: 2px;">
                                    <asp:Label ID="lblRsdlSumBoatReceiptHdg" runat="server" Text='Boat Receipt - ' Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblRdlSumReciptBookingID" runat="server" Text='<%# Eval("BookingId") %>' Font-Size="18px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="3" class="f14CNR"></td>
                            </tr>
                            <tr runat="server">
                                <td class="f14CNL">Payment Type</td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblRsdlSumPaymentTypeName" runat="server" Text='<%# Eval("PaymentType") %>'></asp:Label></td>
                            </tr>
                            <tr runat="server">
                                <td class="f14CNL">Rescheduled Charge</td>
                                <td></td>
                                <td class="f14CNR">₹
                                    <asp:Label ID="lblRsdlRescheduledCharge" runat="server" Text='<%# Eval("RescheduledCharge", "{0:N2}").ToString() %>'></asp:Label></td>
                            </tr>
                            <tr runat="server">
                                <td class="f14CNL">GST</td>
                                <td></td>
                                <td class="f14CNR">₹
                                    <asp:Label ID="lblRsdlSumGST" runat="server" Text='<%# Eval("TaxAmount", "{0:N2}").ToString() %>'></asp:Label></td>
                            </tr>

                            <tr>
                                <td colspan="3" style="width: 100%;">
                                    <hr style="border: 0.2px solid black; margin: 0px;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">NET
                                </td>
                                <td></td>
                                <td class="f14CNR">₹
                                <asp:Label ID="lblRsdlSumNetAmount" runat="server" Text='<%# Eval("NetAmount", "{0:N2}").ToString() %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%;">
                                    <hr style="border: 0.2px solid black; margin: 0px;" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>


            <div runat="server" id="divBoatDeposit" visible="false">
                <asp:DataList ID="dtllistBoatDeposit" runat="server" OnItemDataBound="dtllistBoatDeposit_ItemDataBound" Width="100%">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table runat="server" id="divrowerdep" style="width: 100%;">
                            <tr>
                                <td class="f14CNL">PIN:<asp:Label ID="lblBookingPin" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Eval("BookingId") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Eval("BoatReferenceNo") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblCheckDate" runat="server" Text='<%# Eval("CheckDate") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Eval("TripEndTime") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Eval("BoatHouseId") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Eval("BoatTypeId") %>' Visible="false"></asp:Label>
                                </td>
                                <td></td>
                                <td class="f14CNR" style="padding-top: 12px">DATE:
                                    <asp:Label ID="Label12" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="white-space: nowrap;">
                                    <asp:Label ID="lblResDateHeading" CssClass="f14CNL" Width="100%" Visible="false" runat="server" Text="Rescheduled On"></asp:Label>
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblRescheduledDate" runat="server" Visible="false" Text='<%# Eval("RescheduledDate") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Image ID="imgQRBRoCopy" runat="server" Height="80px" />
                                </td>
                                <td colspan="2" class="f16CN">SL.No : 
                                    <asp:Label ID="Label17" runat="server" Text='<%# Eval("BoatTypeSno") %>' Font-Size="18px"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                    Boat - Rower Copy<br />
                                    <asp:Label ID="Label6" runat="server" Text='Booking :'></asp:Label>
                                    <asp:Label ID="Label10" runat="server" Text='<%# Eval("PremiumStatus") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Booking Id
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label22" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Boat Type
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label13" runat="server" Text='<%# Eval("BoatType") %>'></asp:Label>
                                    <asp:Label ID="Label14" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Boat Number
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("ActualBoatNum") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL" style="white-space: nowrap;">Rower Amount
                                </td>
                                <td></td>
                                <td class="f14CNR">₹
                                <asp:Label ID="lblBillRowerCharge" runat="server" Text='<%# Eval("InitRowerCharge") %>'></asp:Label>

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

                        <table style="width: 100%;" id="divBPassdep" runat="server">
                            <tr>
                                <td class="f14CNL">PIN:<asp:Label ID="Label27" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                </td>
                                <td></td>

                                <td class="f14CNR" style="padding-top: 12px">DATE:
                                    <asp:Label ID="Label23" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="white-space: nowrap;">
                                    <asp:Label ID="lblBPResHdg" CssClass="f14CNL" Width="100%" Visible="false" runat="server" Text="Rescheduled On"></asp:Label>
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblBPResDate" runat="server" Visible="false" Text='<%# Eval("RescheduledDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgQRBBoCopy" runat="server" Height="80px" />
                                </td>
                                <td colspan="2" class="f16CN">SL.No :
                                    <asp:Label ID="Label15" runat="server" Text='<%# Eval("BoatTypeSno") %>' Font-Size="18px"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label25" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                    Boat - Boarding Pass
                                    <br />
                                    <asp:Label ID="Label18" runat="server" Text='Booking :'></asp:Label>
                                    <asp:Label ID="Label20" runat="server" Text='<%# Eval("PremiumStatus") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table style="width: 100%;" id="divBPass1dep" runat="server">
                            <tr>
                                <td class="f14CNL">Booking Id
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Boat Type
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label19" runat="server" Text='<%# Eval("BoatType") %>'></asp:Label>
                                    <asp:Label ID="Label26" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Duration
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="OneRoundYCD" runat="server" Visible="false">1 Round /</asp:Label>
                                    <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                                    <span>(Mins)</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Boat Number
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblActualBoatNum" runat="server" Text='<%# Eval("ActualBoatNum") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">Exp Time Slot
                                </td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblExpectedTime" runat="server" Text='<%# Eval("ExpectedTime") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="f14CNL">NET 
                                </td>
                                <td class="f14CNR">₹
                                 <asp:Label ID="Label28" runat="server" Text='<%# Eval("InitNetAmount") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
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
                    </ItemTemplate>
                </asp:DataList>

            </div>
            <table style="width: 100%; margin-top: 10px;">
                <tr>
                    <td style="white-space: nowrap; text-align: right;">
                        <asp:Label ID="lblPrintDateTime" Font-Names="Roboto-Regular, sans-serif" Font-Size="8px" runat="server"></asp:Label>,
                            <asp:Label ID="lblPrintedByName" Font-Size="8px" Font-Names="Roboto-Regular, sans-serif" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>


        <script type="text/javascript">
            function wait(ms) {
                var start = new Date().getTime();
                var end = start;
                while (end < start + ms) {
                    end = new Date().getTime();
                }
            }
            function printDiv(divId) {
                var divelements = document.getElementById(divId).innerHTML;
                var oldPage = document.body.innerHTML;
                document.body.innerHTML = "<html><head><title></title></head><body>" +
                    divelements + "</body>";

                window.print();
                //setTimeout(function ()
                //{
                //    //window.close();
                //    $("#btnBack").trigger("click");
                //}, 200);
                document.body.innerHTML = oldPage;
                return false;
            }
            (function () {
                var beforePrint = function () {
                    console.log('Functionality to run before printing !');
                };

                var afterPrint = function () {
                    console.log('Functionality to run after printing !');
                    $("#btnBack").trigger("click");
                };

                if (window.matchMedia) {
                    var mediaQueryList = window.matchMedia('print');
                    mediaQueryList.addListener(function (mql) {
                        if (mql.matches) {
                            beforePrint();
                        } else {
                            afterPrint();
                        }
                    });
                }

                window.onbeforeprint = beforePrint;
                window.onafterprint = afterPrint;

            }());

            $(document).ready(function () {

                $("#btnPrint").trigger("click");
                //wait(7000);
                //$("#btnBack").trigger("click");
            });
        </script>
        <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    </form>
</body>
</html>
