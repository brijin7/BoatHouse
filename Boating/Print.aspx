<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print.aspx.cs" Inherits="Boating_Print" %>

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

    <script type="text/javascript" src="https://code.jquery.com/jquery-1.7.1.min.js"></script>
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
                                </td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="Label12" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgQRBRoCopy" runat="server" Height="80px" />
                                </td>
                                <td colspan="2" class="f16CN">
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
                                <td class="f14CNL">Rower Amount
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
                                <td class="f14CNR">
                                    <asp:Label ID="Label23" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgQRBBoCopy" runat="server" Height="80px" />
                                </td>
                                <td colspan="2" class="f16CN">
                                    <asp:Label ID="Label25" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                    Boat - Boarding Pass
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
                                <td class="f14CNL">Expected Time
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
                                <td runat="server" class="f14CNL" style="font-size: x-large">Balance 
                                </td>
                                <td></td>
                                <td class="f14CNR" style="font-size: x-large">₹
                                <asp:Label ID="lblBoatBalanceAmount" runat="server" Text='<%# Eval("BalanceAmount") %>'></asp:Label>
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

            <div runat="server" id="divOthr" visible="false">
                <div runat="server" id="divOtherServiceSingle">
                    <asp:DataList ID="dtlistTicketOtherOtr" runat="server" OnItemDataBound="OtrdtlistTicketOther_ItemDataBound">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td class="f14CNL">
                                        <asp:Label ID="lblBillOBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                    </td>
                                    <td></td>
                                    <td class="f14CNR">
                                        <asp:Label ID="lblBillOBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="imgOtherQR" runat="server" Height="80px" />
                                    </td>
                                    <td colspan="2" class="f16CN">
                                        <asp:Label ID="Label18" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                        <asp:Label ID="Label21" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>

                                    <td colspan="3" class="f14CNR">Total Tickets :
                                  <asp:Label ID="Label20" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
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
                </div>

                <div runat="server" id="divOtherServiceAbstract">
                    <table runat="server" style="width: 100%;">
                        <tr>
                            <td class="f14CNL">
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                            </td>
                            <td colspan="2" class="f14CNR">
                                <asp:Label ID="lblBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="imgOtherReceiptQR" runat="server" Height="80px" />
                            </td>
                            <td></td>
                            <td class="f16CN">
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                Payment Type :
                        <asp:Label ID="lblPaymentTypeName" runat="server" Text='<%# Eval("PaymentTypeName") %>'></asp:Label>
                                Mobile :
                        <asp:Label ID="lblCustomerMobileNo" runat="server" Text='<%# Eval("CustomerMobileNo") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <asp:GridView ID="DlOtherReceipt" runat="server" AllowPaging="false" AutoGenerateColumns="False" ShowFooter="true" GridLines="None" RowStyle-Height="20px">
                        <Columns>
                            <asp:TemplateField HeaderText="Service Name" HeaderStyle-CssClass="f14CNL">
                                <ItemTemplate>
                                    <%--  <asp:Label ID="lblCategoryName" runat="server" CssClass="f14CNR" Text='<%# Bind("CategoryName") %>'></asp:Label>--%>
                                    <asp:Label ID="lblServiceName" runat="server" CssClass="f14CNR" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="No 0f Item" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblNoOfItems" runat="server" CssClass="f14CNR" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Item Charge" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemCharge" runat="server" CssClass="f14CNR" Text='<%# Eval("ItemCharge") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax Amount" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblTaxAmount" runat="server" CssClass="f14CNR" Text='<%# Eval("TaxAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAmount" runat="server" CssClass="f14CNR" Text='<%# Eval("NetAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="20px" />

                    </asp:GridView>

                    <table runat="server" style="width: 100%;">
                        <tr>
                            <td colspan="3">
                                <hr style="border: 0.5px solid black;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="f16CB">NET
                            </td>
                            <td colspan="2" class="f16CB" style="text-align: right; padding-right: 5px;">₹<asp:Label ID="lblGrandNetAmount" runat="server" Text='<%# Eval("GrandNetAmount") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr style="border: 0.5px solid black;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="f16CN" style="text-align: center;">GSTIN - 
                        <asp:Label ID="lblGSTOtr" runat="server" Text='<%# Eval("GSTNumber") %>'></asp:Label>
                                <asp:Label ID="lblCustomerGSTNoOtr" runat="server" Text='<%# Eval("CustomerGSTNumber") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <table style="width: 100%">
                        <tr runat="server" id="trInsOther">
                            <td colspan="3" style="text-align: left;">
                                <span class="insStyleHeader">Instructions :</span>
                                <asp:DataList ID="dtlisTicketInsOther" runat="server" Style="text-align: left; margin-top: 10px;">
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
                </div>
            </div>

            <div id="divRestaurant" runat="server" visible="false">
                <div runat="server" id="divRestaurantSingle">
                    <asp:DataList ID="dtlistTicketRestaurant" runat="server" OnItemDataBound="dtlistTicketRestaurant_ItemDataBound">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td class="f14CNL">
                                        <asp:Label ID="lblBillOBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                    </td>
                                    <td></td>
                                    <td class="f14CNR">
                                        <asp:Label ID="lblBillOBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="imgRestaurantQR" runat="server" Height="80px" />
                                    </td>
                                    <td colspan="2" class="f16CN">
                                        <asp:Label ID="Label18" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                        <asp:Label ID="Label21" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>

                                    <td colspan="3" class="f14CNR">Total No. of Items :
                                  <asp:Label ID="Label20" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
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

                    <asp:DataList ID="dtlistTicketRestaurantnoQRcode" runat="server" OnItemDataBound="dtlistTicketRestaurantnoQRcode_ItemDataBound">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td class="f14CNL">
                                        <asp:Label ID="lblBillOBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                    </td>

                                    <td class="f14CNR">
                                        <asp:Label ID="lblBillOBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </td>

                                </tr>

                                <tr>
                                    <td colspan="2" class="f16CN">
                                        <asp:Label ID="Label18" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                        <asp:Label ID="Label21" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                    </td>


                                </tr>
                                <tr>

                                    <td colspan="3" class="f14CNR">Total Tickets :
                                  <asp:Label ID="Label20" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
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
                </div>

                <div runat="server" id="divRestaurantAbstract">
                    <table runat="server" style="width: 100%;">
                        <tr>
                            <td class="f14CNL">
                                <asp:Label ID="lblBookingIdRestaurant" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                            </td>
                            <td colspan="2" class="f14CNR">
                                <asp:Label ID="lblBookingDateRestaurant" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="imgRestaurantQR1" runat="server" Height="80px" />
                            </td>
                            <td></td>
                            <td class="f16CN">
                                <asp:Label ID="lblBoatHouseNameRestaurant" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                <asp:Label ID="lblPaymentType" runat="server" Text='Payment Type :'></asp:Label>
                                <asp:Label ID="lblPaymentTypeNameRestaurant" runat="server" Text='<%# Eval("PaymentTypeName") %>'></asp:Label><br />
                                <asp:Label ID="lblMobile" runat="server" Text=' Mobile :'></asp:Label>
                                <asp:Label ID="lblCustomerMobileNoRestaurant" runat="server" Text='<%# Eval("CustomerMobileNo") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <asp:GridView ID="DlRestaurantReceipt" runat="server" AllowPaging="false" AutoGenerateColumns="False" ShowFooter="true" GridLines="None" RowStyle-Height="20px">
                        <Columns>
                            <asp:TemplateField HeaderText="Service Name" HeaderStyle-CssClass="f14CNL">
                                <ItemTemplate>
                                    <%-- <asp:Label ID="lblCategoryName" runat="server" CssClass="f14CNR" Text='<%# Bind("CategoryName") %>'></asp:Label>--%>
                                    <asp:Label ID="lblServiceName" runat="server" CssClass="f14CNL" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="No of Item" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblNoOfItems" runat="server" CssClass="f14CNR" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Item Charge" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemCharge" runat="server" CssClass="f14CNR" Text='<%# Eval("ItemCharge") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax Amount" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblTaxAmount" runat="server" CssClass="f14CNR" Text='<%# Eval("TaxAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAmount" runat="server" CssClass="f14CNR" Text='<%# Eval("NetAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="20px" />
                    </asp:GridView>

                    <table runat="server" style="width: 100%;">
                        <tr>
                            <td colspan="3">
                                <hr style="border: 0.5px solid black;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="f16CB">NET
                            </td>
                            <td colspan="2" class="f16CB" style="text-align: right; padding-right: 5px;">₹<asp:Label ID="lblGrandNetAmountRestaurant" runat="server" Text='<%# Eval("GrandNetAmount") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr style="border: 0.5px solid black;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="f16CN" style="text-align: center;">GSTIN - 
                        <asp:Label ID="lblGSTNumberRestaurant" runat="server" Text='<%# Eval("GSTNumber") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <table style="width: 100%">
                        <tr runat="server" id="trInsRestaurant">
                            <td colspan="3" style="text-align: left;">
                                <span class="insStyleHeader">Instructions :</span>
                                <asp:DataList ID="dtlisTicketInsRestaurant" runat="server" Style="text-align: left; margin-top: 10px;">
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
                </div>
            </div>

            <div id="divAdditional" runat="server" visible="false">
                <div runat="server" id="divAddtTicketSingle">
                    <asp:DataList ID="dtlistAdditional" runat="server" OnItemDataBound="dtlistAdditional_ItemDataBound">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td class="f14CNL">
                                        <asp:Label ID="lblBillOBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                    </td>
                                    <td></td>
                                    <td class="f14CNR">
                                        <asp:Label ID="lblBillOBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="imgAddlTicketQR" runat="server" Height="80px" />
                                    </td>
                                    <td colspan="2" class="f16CN">
                                        <asp:Label ID="Label18" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label>
                                        <asp:Label ID="Label21" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="f14CNR">Total Tickets :
                                  <asp:Label ID="Label20" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" visible="false">
                                    <td class="text-left">Ticket Fare
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
                </div>

                <div runat="server" id="divAddtTicketAbstract">
                    <table runat="server" style="width: 100%;">
                        <tr>
                            <td class="f14CNL">
                                <asp:Label ID="lblATBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                            </td>
                            <td colspan="2" class="f14CNR">
                                <asp:Label ID="lblATBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="imgAddlTicketQRAbstract" runat="server" Height="80px" />
                            </td>
                            <td></td>
                            <td class="f16CN">
                                <asp:Label ID="lblATBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                Payment Type :
                            <asp:Label ID="lblATPaymentType" runat="server" Text='<%# Eval("PaymentTypeName") %>'></asp:Label><br />
                                Mobile :
                            <asp:Label ID="lblATMobileNo" runat="server" Text='<%# Eval("CustomerMobileNo") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <asp:GridView ID="DlAdditionalTicketReceipt" runat="server" AllowPaging="false" AutoGenerateColumns="False" ShowFooter="true" GridLines="None" RowStyle-Height="20px">
                        <Columns>
                            <asp:TemplateField HeaderText="Service Name" HeaderStyle-CssClass="f14CNL">
                                <ItemTemplate>
                                    <%-- <asp:Label ID="lblCategoryName" runat="server" CssClass="f14CNR" Text='<%# Bind("CategoryName") %>'></asp:Label>--%>
                                    <asp:Label ID="lblServiceName" runat="server" CssClass="f14CNL" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="No of Ticket" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblNoOfItems" runat="server" CssClass="f14CNR" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ticket Fare" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemCharge" runat="server" CssClass="f14CNR" Text='<%# Eval("ItemCharge") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax Amount" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblTaxAmount" runat="server" CssClass="f14CNR" Text='<%# Eval("TaxAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="f14CNR">
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAmount" runat="server" CssClass="f14CNR" Text='<%# Eval("NetAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="20px" />
                    </asp:GridView>

                    <table runat="server" style="width: 100%;">
                        <tr>
                            <td colspan="3">
                                <hr style="border: 0.5px solid black;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="f16CB">NET
                            </td>
                            <td colspan="2" class="f16CB" style="text-align: right; padding-right: 5px;">₹<asp:Label ID="lblAdditionalTicketNetAmt" runat="server" Text='<%# Eval("GrandNetAmount") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr style="border: 0.5px solid black;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="f16CN" style="text-align: center;">GSTIN - 
                        <asp:Label ID="lblAdditionalGstNo" runat="server" Text='<%# Eval("GSTNumber") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div runat="server" id="divEntranceSummary" visible="false">
                <table runat="server" id="Entrance" style="width: 100%; margin-left: 5%;">
                    <tr>
                        <td colspan="2">
                              <img id="Image1" src="<%=Session["CorpLogo"].ToString() == "" ? "../images/Print-Logo-1.png" : Session["CorpLogo"].ToString() %>" style="margin-left: 22%;text-align: center; width: 134px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center">
                            <asp:Label ID="lblBoatHouseNameEnt" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center">
                            <asp:Label ID="lblrptTitle" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table2" style="width: 100%; margin-left: 5%; margin-top: 15px;">
                    <tr>
                        <td colspan="2" class="f14CNL" style="text-align: center; padding-top: 10px; margin-top: 100px">Print Date : 
                            <asp:Label ID="lblEntPrintDate" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <table runat="server" id="Table3" style="width: 100%; margin-left: 5%; margin-top: 15px;">
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Date :</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblEntrptDate" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <asp:GridView ID="gvEntrance" runat="server" AllowPaging="false" AutoGenerateColumns="False" ShowFooter="true"
                    RowStyle-Height="20px" Width="100%" Style="margin-left: 5%;">
                    <Columns>
                        <asp:TemplateField HeaderText="Particular" HeaderStyle-CssClass="f14CNL">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceName" runat="server" CssClass="f14CNR" Text='<%# Bind("Particulars") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="f14CNR">
                            <ItemTemplate>
                                <asp:Label ID="lblCount" runat="server" CssClass="f14CNR" Text='<%# Eval("Count") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNR">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" CssClass="f14CNR" Text='<%# Eval("TotalAmount","{0:#.00}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="20px" Font-Bold="true" />
                </asp:GridView>

                <table runat="server" id="Table15" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2" class="f14CBL tbllh" style="border: 1px solid black; border-collapse: collapse;">Denomination
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 0px;">
                            <asp:GridView ID="gvOtherResDenomination" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" RowStyle-Height="25px" HeaderStyle-Height="25px" CssClass="tbllh" CellPadding="5">
                                <Columns>
                                    <asp:TemplateField HeaderText="Measure" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDenomination" runat="server" CssClass="f14CNL" Text='<%# Bind("Denomination") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="No.of Pieces" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCount" runat="server" CssClass="f14CNL" Text='<%# Bind("Count") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNR" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Style="padding-left: 5px" CssClass="f14CNR" Text='<%# Bind("Amount", "{0:N2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="25px" Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table1" style="width: 100%; margin-left: 5%;">
                    <tr>
                        <td class="f14CNL">Counter:
                            <asp:Label ID="lblUserFullName" runat="server"></asp:Label></td>
                    </tr>
                </table>

                <table runat="server" id="Table23" style="width: 100%; margin-left: 5%; margin-top: 20px; border: 1px solid black; border-collapse: collapse;">
                    <tr>
                        <td colspan="2" class="f14CNL tbllh" style="line-height: 15px; height: 80px;">Bill Clerk
                             <br />
                            <asp:Label ID="lblroBhname" runat="server" CssClass="f14CNR"></asp:Label>
                            <br />
                            PayPre
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f14CNL tbllh" style="line-height: 15px; height: 80px;">Manager
                            <br />
                            <asp:Label ID="lblroBhname1" runat="server" CssClass="f14CNR"></asp:Label>
                            <br />
                            PayPre
                        </td>
                    </tr>
                </table>

            </div>

            <div runat="server" id="divBoatingSummary" visible="false">

                <table runat="server" id="Table5" style="width: 100%; margin-left: 5%;">
                    <tr>
                        <td colspan="2">
                            <img id="Image2" src="<%=Session["CorpLogo"].ToString() == "" ? "../images/Print-Logo-1.png" : Session["CorpLogo"].ToString() %>" style="margin-left: 22%;text-align: center; width: 134px;" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center">
                            <asp:Label ID="lblBoatHouseNameBoat" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center">
                            <asp:Label ID="lblrptTitleBoat" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table6" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2" class="f14CNL" style="text-align: center;">Print Date : 
                            <asp:Label ID="lblEntPrintDateBoat" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <table runat="server" id="Table7" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr style="padding-top: 10px;">
                        <td class="f14CNL">Date :</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblrptDateBoat" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvBoatServiceSummary" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" RowStyle-Height="20px">
                                <Columns>
                                    <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="f14CNL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatType" runat="server" CssClass="f14CNR" Text='<%# Bind("BoatType") %>'></asp:Label>
                                            <asp:Label ID="lblBoatSeater" runat="server" CssClass="f14CNR" Text='<%# Bind("BoatSeater") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="f14CNL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCount" runat="server" CssClass="f14CNR" Text='<%# Bind("Count") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" CssClass="f14CNR" Text='<%# Bind("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="20px" Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table13" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2" class="f14CBL tbllh" style="border: 1px solid black; border-collapse: collapse;">Collection
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="GvBoatServiceWise" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" RowStyle-Height="20px">
                                <Columns>
                                    <asp:TemplateField HeaderText="Particulars" HeaderStyle-CssClass="f14CNL1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParticulars" runat="server" CssClass="f14CNR" Text='<%# Bind("Particulars") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="f14CNL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcount" runat="server" CssClass="f14CNR" Text='<%# Bind("Count") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingTotalAmount" runat="server" CssClass="f14CNR" Text='<%# Bind("TotalAmount" , "{0:N2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="20px" Font-Bold="true" />
                            </asp:GridView>


                        </td>
                    </tr>
                </table>

                <%--  10NOV--%>
                <table runat="server" id="Table16" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2" id="Payments" runat="server" class="f14CBL tbllh" style="border: 1px solid black; border-collapse: collapse;">Payments
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="GvServiceWisePayments" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                Width="100%" ShowFooter="true" RowStyle-Height="20px">
                                <Columns>
                                    <asp:TemplateField HeaderText="Particulars " HeaderStyle-CssClass="f14CNL1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParticularsPaid" runat="server" CssClass="f14CNR" Text='<%# Bind("Particulars") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="f14CNL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcountPaid" runat="server" CssClass="f14CNR" Text='<%# Bind("Count") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingTotalAmountPaid" runat="server" CssClass="f14CNR" Text=' <%#Eval("Amount","{0:#.00}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="20px" Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

                <style>
                    .tbllh {
                        border: 1px solid black;
                        border-collapse: collapse;
                        line-height: 15px;
                        padding: 5px;
                    }

                    .gvlh {
                        border: 1px solid black;
                        border-collapse: collapse;
                        line-height: 20px;
                        padding: 5px;
                    }
                </style>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <table runat="server" id="BoatingCollection" style="width: 100%; margin-left: 5%; margin-top: 20px; border: 1px solid black; border-collapse: collapse;">
                                <tr>
                                    <td colspan="2" class="f14CBL tbllh">Cash In Counter
                                    </td>
                                </tr>
                                <tr style="border: 1px solid black; border-collapse: collapse;">
                                    <td class="f14CNL tbllh">Total Collected Amount</td>
                                    <td class="f14CNR tbllh">
                                        <asp:Label ID="lblReceivedAmount" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="f14CNL tbllh" style="border: 1px solid black; border-collapse: collapse;">Paid / Refunded Amount</td>
                                    <td class="f14CNR tbllh" style="border: 1px solid black; border-collapse: collapse;">
                                        <asp:Label ID="lblPaidAmount" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>
                                <tr visible="false">
                                    <td class="f14CNL tbllh" style="font-weight: bold; border: 1px solid black; border-collapse: collapse;">Balance</td>
                                    <td class="f14CNR tbllh" style="border: 1px solid black; border-collapse: collapse; font-weight: bold;">
                                        <asp:Label ID="lblBal" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="f14CNL tbllh" style="border: 1px solid black; border-collapse: collapse;">Card</td>
                                    <td class="f14CNR tbllh" style="border: 1px solid black; border-collapse: collapse;">
                                        <asp:Label ID="lblCard" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="f14CNL tbllh" style="border: 1px solid black; border-collapse: collapse;">Online</td>
                                    <td class="f14CNR tbllh" style="border: 1px solid black; border-collapse: collapse;">
                                        <asp:Label ID="lblOnline" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="f14CNL tbllh" style="border: 1px solid black; border-collapse: collapse;">UPI</td>
                                    <td class="f14CNR tbllh" style="border: 1px solid black; border-collapse: collapse;">
                                        <asp:Label ID="lblUPI" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>
                                <tr style="border: 1px solid black; border-collapse: collapse;">
                                    <td class="f14CBL tbllh">Cash In Hand</td>
                                    <td class="f14CBR tbllh">
                                        <asp:Label ID="lblCashInHand" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="f14CNL tbllh" style="font-weight: bold; border: 1px solid black; border-collapse: collapse;">Net Amount</td>
                                    <td class="f14CNR tbllh" style="border: 1px solid black; border-collapse: collapse; font-weight: bold;">
                                        <asp:Label ID="lblFinalNetAmount" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>
                <%--  10NOV--%>


                <table runat="server" id="Table14" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2" class="f14CBL tbllh" style="border: 1px solid black; border-collapse: collapse;">Denomination
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 0px;">
                            <asp:GridView ID="gvBoatDenomination" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" RowStyle-Height="25px" HeaderStyle-Height="25px" CssClass="tbllh" CellPadding="5">
                                <Columns>
                                    <asp:TemplateField HeaderText="Measure" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDenomination" runat="server" CssClass="f14CNL" Text='<%# Bind("Denomination") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="No.of Pieces" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCount" runat="server" CssClass="f14CNL" Text='<%# Bind("Count") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNR" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Style="padding-left: 5px" CssClass="f14CNR" Text='<%# Bind("Amount", "{0:N2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="25px" Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table4" style="width: 100%; margin-left: 5%;">
                    <tr>
                        <td class="f14CNL">Counter:
                            <asp:Label ID="lblUserFullNameBoat" runat="server"></asp:Label></td>
                    </tr>
                </table>

                <table runat="server" id="Table22" style="width: 100%; margin-left: 5%; margin-top: 20px; border: 1px solid black; border-collapse: collapse;">
                    <tr>
                        <td colspan="2" class="f14CNL tbllh" style="line-height: 15px; height: 80px;">Bill Clerk
                             <br />
                            <asp:Label ID="lblBBHName" runat="server" CssClass="f14CNR"></asp:Label>
                            <br />
                            PayPre
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f14CNL tbllh" style="line-height: 15px; height: 80px;">Manager
                            <br />
                            <asp:Label ID="lblBBHName1" runat="server" CssClass="f14CNR"></asp:Label>
                            <br />
                            PayPre
                        </td>
                    </tr>
                </table>

            </div>

            <div runat="server" id="divRefundCashRpt" visible="false">
                <table class="f14CBL" runat="server" id="tblRefundCash" style="width: 100%; margin-left: 5%;">
                    <tr>
                        <td colspan="2">
                            <img id="Image3" src="<%=Session["CorpLogo"].ToString() == "" ? "../images/Print-Logo-1.png" : Session["CorpLogo"].ToString() %>" style="margin-left: 22%;text-align: center; width: 134px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center">
                            <asp:Label ID="lblBoatHouseNameRefund" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center">
                            <asp:Label ID="lblrptTitleRefund" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table9" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2" class="f14CNL" style="text-align: center;">Print Date : 
                            <asp:Label ID="lblEntPrintDateRefund" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f14CNL" style="text-align: center;">Report Date : 
                            <asp:Label ID="lblrptDateRefund" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <table runat="server" id="Table10" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvRefundCashfrom" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" RowStyle-Height="25px" HeaderStyle-Height="25px" CssClass="tbllh" CellPadding="5">
                                <Columns>
                                    <asp:TemplateField HeaderText="Cash From" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatSeater" runat="server" CssClass="f14CNR" Text='<%# Bind("Particulars") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNR" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" CssClass="f14CNR" Text='<%# Bind("Amount") %>'></asp:Label>
                                            <%--        <asp:Label ID="Label15" runat="server" CssClass="f14CNR" Text='<%# Bind("Amount", "{0:N2}") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="25px" Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 0px;">
                            <asp:GridView ID="gvRefundPayAmount" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" RowStyle-Height="25px" HeaderStyle-Height="25px" CssClass="tbllh" CellPadding="5">
                                <Columns>
                                    <asp:TemplateField HeaderText="Refund Details" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatSeater" runat="server" CssClass="f14CNL" Text='<%# Bind("BoatType") %>'></asp:Label>
                                            <asp:Label ID="Label15" runat="server" CssClass="f14CNL" Text='<%# Bind("BoatSeater") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNR" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Style="padding-left: 5px" CssClass="f14CNR" Text='<%# Bind("ClaimedDeposit","{0:n}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="25px" Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <style>
                    .tbllh {
                        border: 1px solid black;
                        border-collapse: collapse;
                        line-height: 15px;
                        padding: 5px;
                    }

                    .gvlh {
                        border: 1px solid black;
                        border-collapse: collapse;
                        line-height: 20px;
                        padding: 5px;
                    }
                </style>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <table runat="server" id="Table8" style="width: 100%; margin-left: 5%; margin-top: 20px; border: 1px solid black; border-collapse: collapse;">
                                <tr>
                                    <td colspan="2" class="f14CBL tbllh">Cash In Counter
                                    </td>
                                </tr>
                                <tr style="border: 1px solid black; border-collapse: collapse;">
                                    <td class="f14CNL tbllh">Total Rcvd From Counter</td>
                                    <td class="f14CNR tbllh">
                                        <asp:Label ID="lblRefundTotalFromCounter" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="f14CNL tbllh" style="border: 1px solid black; border-collapse: collapse;">Less Refunded Amount</td>
                                    <td class="f14CNR tbllh" style="border: 1px solid black; border-collapse: collapse;">
                                        <asp:Label ID="lblRefundLessAmount" runat="server" CssClass="f14CNR"></asp:Label></td>
                                </tr>
                                <tr style="border: 1px solid black; border-collapse: collapse;">
                                    <td class="f14CBL tbllh">Cash In Hand</td>
                                    <td class="f14CBR tbllh">
                                        <asp:Label ID="lblRefundCashInHand" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table11" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2" class="f14CBL tbllh" style="border: 1px solid black; border-collapse: collapse;">Denomination
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 0px;">
                            <asp:GridView ID="gvDenomination" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" RowStyle-Height="25px" HeaderStyle-Height="25px" CssClass="tbllh" CellPadding="5">
                                <Columns>
                                    <asp:TemplateField HeaderText="Measure" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDenomination" runat="server" CssClass="f14CNL" Text='<%# Bind("Denomination") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="No.of Pieces" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCount" runat="server" CssClass="f14CNL" Text='<%# Bind("Count") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNR" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <span>&#x20B9;</span><asp:Label ID="lblAmount" runat="server" Style="padding-left: 5px" CssClass="f14CNR" Text='<%# Bind("Amount", "{0:N2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="25px" Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

                <table runat="server" id="Table12" style="width: 100%; margin-left: 5%; margin-top: 20px; border: 1px solid black; border-collapse: collapse;">
                    <tr>
                        <td colspan="2" class="f14CNL tbllh" style="line-height: 15px; height: 80px;">Bill Clerk
                             <br />
                            <asp:Label ID="lblBhn" runat="server" CssClass="f14CNR"></asp:Label>
                            <br />
                            PayPre
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f14CNL tbllh" style="line-height: 15px; height: 80px;">Manager
                            <br />
                            <asp:Label ID="lblbhn1" runat="server" CssClass="f14CNR"></asp:Label>
                            <br />
                            PayPre
                        </td>
                    </tr>
                </table>
            </div>

            <div runat="server" id="divAdditionalTicket" visible="false">
                <table runat="server" id="EntranceAdd" style="width: 100%; margin-left: 5%;">
                    <tr>
                        <td colspan="2">
                              <img id="Image4" src="<%=Session["CorpLogo"].ToString() == "" ? "../images/Print-Logo-1.png" : Session["CorpLogo"].ToString() %>" style="margin-left: 22%;text-align: center; width: 134px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center">
                            <asp:Label ID="lblBoatHouseNameEntAdd" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center">
                            <asp:Label ID="lblrptTitleAdd" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table17" style="width: 100%; margin-left: 5%; margin-top: 15px;">
                    <tr>
                        <td colspan="2" class="f14CNL" style="text-align: center; padding-top: 10px; margin-top: 100px">Print Date : 
                            <asp:Label ID="lblEntPrintDateAdd" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <table runat="server" id="Table18" style="width: 100%; margin-left: 5%; margin-top: 15px;">
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Date :</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblEntrptDateAdd" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <asp:GridView ID="gvEntranceAdd" runat="server" AllowPaging="false" AutoGenerateColumns="False" ShowFooter="true"
                    RowStyle-Height="20px" Width="100%" Style="margin-left: 5%;">
                    <Columns>
                        <asp:TemplateField HeaderText="Particular" HeaderStyle-CssClass="f14CNL">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceName" runat="server" CssClass="f14CNR" Text='<%# Bind("CategoryName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="f14CNR">
                            <ItemTemplate>
                                <asp:Label ID="lblCount" runat="server" CssClass="f14CNR" Text='<%# Eval("TotalCount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNR">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" CssClass="f14CNR" Text='<%# Eval("Amount","{0:#.00}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="20px" Font-Bold="true" />
                </asp:GridView>

                <table runat="server" id="Table19" style="width: 100%; margin-left: 5%; margin-top: 10px;">
                    <tr>
                        <td colspan="2" class="f14CBL tbllh" style="border: 1px solid black; border-collapse: collapse;">Denomination
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 0px;">
                            <asp:GridView ID="gvAdditionalTicketDenomination" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" RowStyle-Height="25px" HeaderStyle-Height="25px" CssClass="tbllh" CellPadding="5">
                                <Columns>
                                    <asp:TemplateField HeaderText="Measure" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDenomination" runat="server" CssClass="f14CNL" Text='<%# Bind("Denomination") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="No.of Pieces" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCount" runat="server" CssClass="f14CNL" Text='<%# Bind("Count") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNR" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Style="padding-left: 5px" CssClass="f14CNR" Text='<%# Bind("Amount", "{0:N2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>

                                </Columns>
                                <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="25px" Font-Bold="true" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table runat="server" id="Table20" style="width: 100%; margin-left: 5%;">
                    <tr>
                        <td class="f14CNL">Counter:
                            <asp:Label ID="lblUserFullNameAdd" runat="server"></asp:Label></td>
                    </tr>
                </table>

                <table runat="server" id="Table21" style="width: 100%; margin-left: 5%; margin-top: 20px; border: 1px solid black; border-collapse: collapse;">
                    <tr>
                        <td colspan="2" class="f14CNL tbllh" style="line-height: 15px; height: 80px;">Bill Clerk
                             <br />
                            <asp:Label ID="lblBhname" runat="server" CssClass="f14CNR"></asp:Label>
                            <br />
                            PayPre
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f14CNL tbllh" style="line-height: 15px; height: 80px;">Manager
                            <br />
                            <asp:Label ID="lblBhname1" runat="server" CssClass="f14CNR"></asp:Label>
                            <br />
                            PayPre
                        </td>
                    </tr>
                </table>

            </div>

            <div runat="server" id="divRefund" visible="false">
                <table runat="server" id="Refund" style="width: 100%;">
                    <tr>
                        <td colspan="2">
                              <img id="Image5" src="<%=Session["CorpLogo"].ToString() == "" ? "../images/Print-Logo-1.png" : Session["CorpLogo"].ToString() %>" style="margin-left: 22%;text-align: center; width: 134px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                            <asp:Label ID="lblRefundBoatHouseName" runat="server" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                            <asp:Label ID="lblRefundHeader" runat="server" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="false" id="lblRefundPayment">
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 3px;">
                            <asp:Label ID="lblRefundVoucher" runat="server" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                </table>

                <table runat="server" id="tblRefundDetails" style="width: 100%; margin-left: 5%; margin-top: 15px;">
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Id </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundBookingId" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Pin </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundBookingPin" runat="server"></asp:Label></td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Type </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundBoatType" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Seater </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundBoatSeater" runat="server"></asp:Label></td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Date </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundDate" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Duration </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundDuration" runat="server"></asp:Label></td>
                    </tr>

                </table>

                <table runat="server" id="tblRefundEligible" style="width: 100%; margin-top: 15px;" visible="false">
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-left: 25px;">
                            <asp:Label ID="lblRefundNotEligible" runat="server" Font-Bold="true" Font-Size="Larger"></asp:Label>
                        </td>
                    </tr>
                </table>

                <table runat="server" id="tblFinalRefund" style="width: 100%; margin-left: 5%; margin-top: 15px;">
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Start Time </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundStartTime" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">End Time </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundEndTime" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Total Duration </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundTotalDuration" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <%--New --%>
                    <tr id="tblBillAmount" style="padding-top: 50px;" runat="server">
                        <td class="f14CNL">Bill Amount </td>
                        <td class="f14CNR">
                            <asp:Label Font-Bold="true" ID="lblBillAmount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="tblDepositAmount" style="padding-top: 50px;" runat="server">
                        <td class="f14CNL">Deposit Amount </td>
                        <td class="f14CNR">
                            <asp:Label Font-Bold="true" ID="lblDepositAmount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="tblExtraCharges" style="padding-top: 50px;" runat="server">
                        <td class="f14CNL">Extra Charges </td>
                        <td class="f14CNR">
                            <asp:Label Font-Bold="true" ID="lblExtraCharges" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr id="tblRefundPayment" style="padding-top: 50px;" runat="server" visible="false">
                        <td class="f14CNL">Payment Type </td>
                        <td class="f14CNR">
                            <asp:Label Font-Bold="true" ID="lblRepaymentType" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr id="tblDesignTable" runat="server" visible="false">
                        <td colspan="2" style="text-align: center;">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 48%">
                                        <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                    </td>
                                    <td style="width: 48%">
                                        <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>


                    <tr id="tblRefundAmount" style="padding-top: 50px; font-size: 15px; font-weight: bold" runat="server" visible="false">
                        <td class="f14CNL">Refund Amount </td>
                        <td class="f14CNR">
                            <asp:Label Font-Bold="true" Font-Size="Larger" ID="lblReundAmount" runat="server"></asp:Label>
                        </td>
                    </tr>





                </table>

                <footer style="text-align: center; padding-top: 2rem; padding-left: 5px;">
                    <table runat="server" id="lblRefundFooter" style="width: 100%; text-align: center">
                        <tr>
                            <td style="text-align: center">*****Thank You Visit Again *****</td>
                        </tr>
                    </table>
                </footer>
            </div>
            <div runat="server" id="divChangeBoat" visible="false">
                <table runat="server" id="ChangeBoat" style="width: 100%;">
                    <tr>
                        <td colspan="2">
                              <img id="Image6" src="<%=Session["CorpLogo"].ToString() == "" ? "../images/Print-Logo-1.png" : Session["CorpLogo"].ToString() %>" style="margin-left: 22%;text-align: center; width: 134px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                            <asp:Label ID="lblChangeBoatHouseName" runat="server" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                            <asp:Label ID="lblChangeBoatHeader" runat="server" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>

                </table>

                <table runat="server" id="ChangeBoatDetails" style="width: 100%; margin-left: 5%; margin-top: 15px;">

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Id </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblChangeBookingId" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Pin </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblChangeBookingPin" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Date </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblChangeBookingDate" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                            <asp:Label ID="lblOldDetails" runat="server" Text="Previous Boat Details" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Type </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblOldBoatType" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Seater </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblOldBoatSeater" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Net Amount</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblChangeOldAmount" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.5px solid black;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                            <asp:Label ID="lblNewDetails" runat="server" Text="New Boat Details" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Type</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblNewBoatType" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Seater</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblNewBoatSeater" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Net Amount</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblChangeNewAmount" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Payment Type</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblChangePaymentMode" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.5px solid black;" />
                        </td>

                    </tr>



                    <tr runat="server" id="ChangeExtraCharge" style="padding-top: 50px; font-size: 16px;">
                        <td class="f14CNL1" style="font-size: large">Extra Charge</td>
                        <td class="f14CNR" style="font-size: large; font-weight: bold;">
                            <asp:Label ID="lblChangeExtraCharge" runat="server"></asp:Label></td>
                    </tr>



                    <tr runat="server" id="ChangeRefundAmount" style="padding-top: 50px; font-size: 16px;">
                        <td class="f14CNL1">Refund Amount</td>
                        <td class="f14CNR">
                            <asp:Label ID="lblChangeRefundAmount" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.5px solid black;" />
                        </td>

                    </tr>

                </table>

            </div>


            <%--<div runat="server" id="divRefCancel" visible="false">
                <table runat="server" id="RefCancel" style="width: 100%;">
                    <tr>
                        <td colspan="2">
                              <img src="../images/Print-Logo-1.png" style="margin-left: 22%;text-align: center; width: 134px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                            <asp:Label ID="lblRefCancelBoatHouseName" runat="server" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                            <asp:Label ID="lblRefCancelHeader" runat="server" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 20px;">
                             <asp:Label ID="Label17" runat="server" Text='Voucher No. : ' Font-Size="18px" Font-Bold="true"></asp:Label>
                              <asp:Label ID="lblRefCancelVoucherNo" runat="server"  Font-Bold="true" Font-Size="18px"></asp:Label>
                        </td>
                    </tr>
                </table>

                <table runat="server" id="tblRefCancelDetails" style="width: 100%; margin-left: 5%; margin-top: 15px;">
  
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Id </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelBookingId" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Pin </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelBookingPin" runat="server"></asp:Label></td>
                    </tr>

                    

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Date </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelDate" runat="server"></asp:Label></td>
                    </tr>

                </table>
                <table runat="server" id="tblFinalRefCancel" style="width: 100%; margin-left: 5%; margin-top: 15px;">
                               <tr style="padding-top: 50px;">
                        <td class="f14CNL">Payment Type </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelPayment" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Charge </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelBoatCharge" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Deposit Amount </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelDepAmnt" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Total Charges </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelTotalCharges" runat="server"></asp:Label>

                        </td>
                    </tr>
                     <tr style="padding-top: 50px;">
                        <td class="f14CNL">Cancellation Charges </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancellationCharges" runat="server"></asp:Label>

                        </td>
                    </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.1px solid black; margin: 0px;" />
                                </td>
                            </tr>
                    <tr id="tblRefCancelAmount" style="padding-top: 50px;" runat="server" visible="false">
                        
                        <td class="f14CNL" style="Font-Size:16px">Refund Amount </td>
                        <td class="f14CNR">
                            <asp:Label Font-Bold="true" ID="lblRefCancelAmount" runat="server" Font-Size="16px"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                                <td colspan="3">
                                    <hr style="border: 0.1px solid black; margin: 0px;" />
                                </td>
                            </tr>

                </table>

                <footer style="text-align: center; padding-top: 2rem; padding-left: 5px;">
                    <table runat="server" id="lblRefCancelFooter" style="width: 100%; text-align: center">
                        <tr>
                            <td style="text-align: center">*****Thank You Visit Again *****</td>
                        </tr>
                    </table>
                </footer>
            </div>--%>
            <div runat="server" id="divRefCancel" visible="false">
                <table runat="server" id="RefCancel" style="width: 100%;">
                    <tr>
                        <td colspan="2">
                              <img id="Image7" src="<%=Session["CorpLogo"].ToString() == "" ? "../images/Print-Logo-1.png" : Session["CorpLogo"].ToString() %>" style="margin-left: 22%;text-align: center; width: 134px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 22px;">
                            <asp:Label ID="lblRefCancelBoatHouseName" runat="server" Font-Bold="true" Font-Size="Larger" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 3px;">
                            <asp:Label ID="lblRefCancelHeader" runat="server" Font-Bold="true" Font-Size="16px" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-right: 3px;">
                            <asp:Label ID="Label17" runat="server" Text='Payment No. : ' Font-Size="16px" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblRefCancelVoucherNo" runat="server" Font-Bold="true" Font-Size="16px"></asp:Label>
                        </td>

                    </tr>
                </table>

                <table runat="server" id="tblRefCancelDetails" style="width: 100%; margin-left: 5%; margin-top: 15px;">

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Id </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelBookingId" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Pin </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelBookingPin" runat="server"></asp:Label></td>
                    </tr>



                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Booking Date </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelDate" runat="server"></asp:Label></td>
                    </tr>

                </table>
                <table runat="server" id="tblFinalRefCancel" style="width: 100%; margin-left: 5%; margin-top: 15px;">
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Payment Type </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelPayment" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Charge </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelBoatCharge" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Deposit Amount </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelDepAmnt" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Total Charges </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancelTotalCharges" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Cancellation Charges </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefCancellationCharges" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.1px solid black; margin: 0px;" />
                        </td>
                    </tr>
                    <tr id="tblRefCancelAmount" style="padding-top: 50px;" runat="server" visible="false">

                        <td class="f14CNL" style="font-size: 16px">Refund Amount </td>
                        <td class="f14CNR">
                            <asp:Label Font-Bold="true" ID="lblRefCancelAmount" runat="server" Font-Size="16px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.1px solid black; margin: 0px;" />
                        </td>
                    </tr>

                </table>

                <footer style="text-align: center; padding-top: 2rem; padding-left: 5px;">
                    <table runat="server" id="lblRefCancelFooter" style="width: 100%; text-align: center">
                        <tr>
                            <td style="text-align: center">*****Thank You Visit Again *****</td>
                        </tr>
                    </table>
                </footer>
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
    </form>
</body>
</html>
