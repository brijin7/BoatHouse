<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintCreditBoat.aspx.cs" Inherits="Boating_PrintCreditBoat" %>


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
    <script
        src="https://code.jquery.com/jquery-3.6.4.min.js"
        integrity="sha256-oP6HI9z1XaZNBrJURtCoUT5SUnxFr8s3BzRl+cbzUq8="
        crossorigin="anonymous"></script>
    <%-- <script type="text/javascript" src="https://code.jquery.com/jquery-1.7.1.min.js"></script>--%>
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
                                    <asp:Label ID="lblUniqueId" runat="server" Text='<%# Eval("UniqueId") %>' Visible="false"></asp:Label>
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
                                <td class="f14CNL">Rower Amount
                                </td>
                                <td></td>
                                <td class="f14CNR">₹
                                <asp:Label ID="lblActualRowerCharge" runat="server" Text='<%# Eval("ActualRowerCharge") %>'></asp:Label>

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
                                <td colspan="2" class="f16CN">SL.No : 
                                    <asp:Label ID="Label15" runat="server" Text='<%# Eval("BoatTypeSno") %>' Font-Size="18px"></asp:Label>
                                    <br />
                                    <br />
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
                                <td class="f14CNL">NET 
                                </td>
                                <td class="f14CNR">₹
                                 <asp:Label ID="lblActualNetAmount" runat="server" Text='<%# Eval("ActualNetAmount") %>'></asp:Label>
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
                                    <asp:Label ID="lblActualBoatCharge" runat="server" Text='<%# Eval("ActualBoatCharge") %>'></asp:Label></td>
                            </tr>
                            <tr runat="server" visible="true">
                                <td class="f14CNL">Rower Charge</td>
                                <td></td>
                                <td class="f14CNR">
                                    <asp:Label ID="lblActualRowerCharge" runat="server" Text='<%# Eval("ActualRowerCharge") %>'></asp:Label></td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.1px solid black; margin: 0px;" />
                                </td>
                            </tr>

                            <tr>
                                <td class="f14CNL">NET
                                </td>
                                <td></td>
                                <td class="f14CNR">₹
                                <asp:Label ID="lblActualNetAmount" runat="server" Text='<%# Eval("ActualNetAmount") %>'></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <hr style="border: 0.2px solid black; margin: 0px;" />
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
