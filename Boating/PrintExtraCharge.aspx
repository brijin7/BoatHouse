<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintExtraCharge.aspx.cs" Inherits="Boating_PrintExtraCharge" %>

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


            <div runat="server" id="divRefund" visible="false">
                <table runat="server" id="Refund" style="width: 100%;">
                    <tr>
                        <td colspan="2">
                            <img id="Image1" src="<%=Session["CorpLogo"].ToString() == "" ? "../images/Print-Logo-1.png" : Session["CorpLogo"].ToString() %>" style="margin-left: 22%; text-align: center; width: 134px;" />
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
                </table>



                <table runat="server" id="tblRefundDetails" style="width: 100%; margin-left: 5%; margin-top: 5px;">
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

                    <tr>
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
                </table>

                <%--  <table runat="server" id="tblRefundEligible" style="width: 100%; margin-top: 15px;">
                    <tr>
                        <td colspan="2" class="f16CB" style="text-align: center; padding-top: 10px; margin-top: 100px; padding-left: 25px;">
                            <asp:Label ID="lblRefundNotEligible" runat="server" Font-Bold="true" Font-Size="Larger"></asp:Label>
                        </td>
                    </tr>
                </table>--%>

                <%--<table runat="server" id="Tbl1" style="width: 100%; text-align: center">
                        <tr>
                            <td style="text-align: center">-------------------------------------------</td>
                        </tr>
                    </table>--%>





                <table runat="server" id="tblExtraTime" style="width: 100%; margin-left: 5%; margin-top: 2px;">
                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Trip Start Time </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundStartTime" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Trip End Time </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundEndTime" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Extra Duration </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundTotalDuration" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <tr>
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

                </table>



                <table runat="server" id="tblExtraDetails" style="width: 100%; margin-left: 5%; margin-top: 2px;">

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Charge </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblExtraBoatCharge" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Rower Charge </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblExtraRowerCharge" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <%-- <tr style="padding-top: 50px;">
                        <td class="f14CNL">Net Amount </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblExtraNetAmount" runat="server"></asp:Label>

                        </td>
                    </tr>--%>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Tax Amount </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblTax" runat="server"></asp:Label>

                        </td>
                    </tr>


                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.1px solid black; margin: 0px;" />
                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">NET </td>
                        <td class="f14CNR">₹
                            <asp:Label ID="lblExtraNetAmount" runat="server"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr style="border: 0.2px solid black; margin: 0px;" />
                        </td>
                    </tr>
                </table>


                <table runat="server" id="tblExtraDetailsAllowed" style="width: 100%; margin-left: 5%; margin-top: 2px;">

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Boat Deposit </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblBoatDeposit" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Extension Charge </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblExtensionCharge" runat="server"></asp:Label>

                        </td>
                    </tr>


                    <tr style="padding-top: 50px;">
                        <td class="f14CNL">Refund Amount </td>
                        <td class="f14CNR">
                            <asp:Label ID="lblRefundAmount" runat="server"></asp:Label>

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
