﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="BookingOtherServices.aspx.cs" Inherits="Boating_BoatBookingFinal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />

    <script lang="javascript" type="text/javascript">

        // Disable F5 Key

        $(document).ready(function () {
            function disableF5(e) {
                if (e.keyCode == 116 || e.keyCode == 17) e.preventDefault();
            };
            $(document).on("keydown", disableF5);
        });

        // Back Button Disable

        $(document).ready(function () {
            history.pushState(null, null, location.href);
            window.onpopstate = function () {
                history.go(1);
            };
        });
    </script>

    <script type="text/javascript">
        function showLoader() {
            document.getElementById("loader").style.display = 'block';
        }

    </script>
    <script type="text/javascript">
        function showHourGlass() {
            document.getElementById("HourGlass").style.display = 'block';
        }

    </script>

    <script lang="javascript" type="text/javascript">
        var myVar = setInterval(myTimer, 0);
        function StartTimer() {
            myVar = setInterval(myTimer, 400);
        }
        function myTimer() {
            const monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];
            const DayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

            var d = new Date();

            var Time = d.toLocaleTimeString();
            var date = DayNames[d.getDay()] + ", " + d.getDate() + "-" + monthNames[d.getMonth()];

            if (date != "" && date != null) {
                document.getElementById("Date").innerText = date;
            }
            if (Time != "" && Time != null) {
                document.getElementById("Time").innerText = Time;
            }

        }
        function StopTimer() {
            clearInterval(myVar);
        }
    </script>

    <style>
        .daterangepicker {
            left: 470px;
        }

        .btntbody table tbody {
            display: flex;
        }

        .dtlBoatType {
            padding: 0px;
            margin-bottom: 5px;
            margin-left: 20px !important;
            border-radius: 17px;
            box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
            overflow: auto;
            transition: all .5s ease;
            max-width: 95%;
        }


        .FinalSummary {
            border: 2px solid lightgray;
            border-radius: 15px;
            padding: 15px;
            font-size: 15px;
            position: relative;
        }

        .grdTextAlign {
            text-align: center;
        }


        /*ul li {
            display: inline;
            padding: 5px;
        }*/

        .btnBoatSeater {
            font-weight: bold;
            width: 100px;
            background-color: rgb(33, 136, 56) !important;
        }

        .Numpnl {
            border-width: 0px;
            visibility: visible;
            position: absolute;
            /*left: 0px !important;*/
            top: 38px !important;
        }

        .button1 {
            background-color: #e0e0e0;
            border-radius: 28px;
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            /*padding: 8px 8px;*/
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: 5px;
        }

            .button1:hover {
                background-color: #5cbf2a;
            }

            .button1:active {
                position: relative;
                top: 1px;
            }

        .btnlabel {
            padding-right: 30px;
            font-size: 30px;
        }

        #spanhovering {
            cursor: pointer;
        }

        #divtoshow {
            position: absolute;
            display: none;
        }

        .buttonNor {
            background-color: white;
            border-radius: 8px;
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            padding: 8px 8px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: 5px;
        }

        .buttonOth {
            background-color: #9ACD32;
            border-radius: 8px;
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            padding: 8px 8px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: 5px;
        }

        .buttonNor:hover {
            background-color: #C71585;
            color: white;
        }

        .buttonNor:active {
            position: relative;
            top: 1px;
        }

        .buttonOth:hover {
            background-color: #C71585;
            color: white;
        }

        .buttonOth:active {
            position: relative;
            top: 1px;
        }


        .buttonPre {
            background-color: gold;
            border-radius: 8px;
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            padding: 8px 8px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: 5px;
        }

            .buttonPre:hover {
                background-color: #C71585;
                color: white;
            }

            .buttonPre:active {
                position: relative;
                top: 1px;
            }

        .btnFinal {
            display: inline-block;
            font-weight: 400;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            border: 1px solid transparent;
            padding: 0.4rem .5rem;
            font-size: 2rem !important;
            line-height: 1.5;
            border-radius: 2.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }

        .payMode {
            height: 60px;
            width: 130px;
            display: inline-block;
            font-weight: 400;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            /*border: 1px solid transparent;
            padding: 0.4rem .5rem;*/
            font-size: 2rem !important;
            line-height: 1.5;
            border-radius: 2.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            outline: none;
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

    <style>
        ::-webkit-scrollbar {
            width: 5px;
            height: 7px;
        }

        ::-webkit-scrollbar-button {
            width: 16px;
            height: 0px;
        }

        ::-webkit-scrollbar-track {
            /*background-image: linear-gradient(to bottom, #ffffff, #ffffff, #ffffff, #ffffff, #ffffff, #f1f5ff, #dcecff, #bfe6ff, #62dbf2, #00cec6, #00bb7c, #00a307);*/
        }

        ::-webkit-scrollbar-thumb {
            border-radius: 8px;
            background-image: linear-gradient(to right top, #051937, #004d7a, #008793, #00bf72, #a8eb12);
        }

            ::-webkit-scrollbar-thumb:hover {
                background-image: linear-gradient(to right top, #051937, #004d7a, #008793, #00bf72, #a8eb12);
            }
    </style>
    <style type="text/css">
        .tooltipDemo {
            position: relative;
            display: inline;
            text-decoration: none;
            left: 5px;
            top: 0px;
        }

            .tooltipDemo:hover:before {
                /* border: solid;
                        border-color: transparent #FF8F35;
                        border-width: 6px 6px 6px 6px;
                        bottom: 30px;
                        content: "";
                        right: 15px;
                        top: 5px;
                        position: absolute;
                        z-index: 95;*/
            }

            .tooltipDemo:hover:after {
                background: #3c66ba;
                border-radius: 4px;
                color: #fff;
                width: 180px;
                left: 0px;
                bottom: -30px;
                content: attr(alt);
                position: absolute;
                padding: 5px 15px;
                z-index: 95;
            }
    </style>

    <script type="text/javascript">
        function Search_Gridview(strKey) {
            var strData = strKey.value.toLowerCase().split(" ");
            var Grid ="<%=GvBoatBooking.ClientID%>";
            var tblData = document.getElementById(Grid);
            var rowData;
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
    </script>

    <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-body">

                <div class="row" runat="server" visible="false" id="divShow">
                    <div class="col-sm-12 col-md-12 text-center">
                        <h5 class="pghr" style="text-align: center; font-size: 25px; display: inline;">Other Services </h5>
                        <span style="float: right; display: inline;">
                            <asp:ImageButton ID="imgbtnNewBook" runat="server" ImageUrl="~/images/Ticket.svg" Width="40px"
                                OnClick="imgbtnNewBook_Click" ToolTip="New Booking" CssClass="pr-2" Visible="false" OnClientClick="StartTimer()" />
                        </span>
                    </div>
                </div>

                <div runat="server" id="divEntry" style="width: 100%">
                    <div class="row">
                        <div class="col-md-4 col-sm-4">
                            <h3 id="Time" style="display: inline; float: left; color: black;"></h3>
                        </div>
                        <div class="col-md-4 col-sm-4 text-center">
                            <h5 class="pghr" style="text-align: center; font-size: 25px; display: inline;">Other Services </h5>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <a href="#" alt="Bulk Other Services" class="tooltipDemo" id="BulkOther" runat="server">
                                 <asp:CheckBox runat="server" Style="zoom: 1.5;" Width="17px" Height="21px" ID="ChkBulk" OnCheckedChanged="ChkBulk_CheckedChanged" AutoPostBack="true" /><b style="font-size: 19px; color: black;">Bulk</b>
                             </a>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <h3 id="Date" style="display: inline; overflow: auto; float: right; padding-left: 20px; color: black;"></h3>
                            <asp:ImageButton ID="imgbtnBookedList" runat="server" Style="float: right" ImageUrl="~/images/Print.svg"
                                Width="35px" OnClick="imgbtnBookedList_Click" OnClientClick="StopTimer()" ToolTip="Ticket Re-Print" />
                        </div>
                        <%-- <h3 id="Time" style="display: inline; float: left; color: black;"></h3>
                            <h2 id="Date" style="display: inline; float: right; color: black;"></h2>--%>
                    </div>

                    <hr />

                    <div class="col-xs-12" style="background-color: cornsilk">
                        <div class="row">
                            <div class="col-md-6 col-sm-6">
                                <b style="font-size: 18px">Booking Count :</b>
                                <asp:LinkButton ID="bblblCount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"
                                    OnClick="bblblCount_Click" Font-Underline="true"></asp:LinkButton>
                            </div>


                            <div class="col-md-6 col-sm-6">
                                <b style="font-size: 18px">Net Amount :</b>
                                <asp:LinkButton ID="bblblNetAmount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"
                                    OnClick="bblblNetAmount_Click" Font-Underline="true"></asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <div class="row" runat="server" id="divBack">
                        <div class="col-sm-7 col-xs-12" style="margin-top: 10px">

                            <asp:DataList ID="dtlOther" runat="server" Width="100%" OnItemDataBound="DtlOther_ItemDataBound">
                                <HeaderTemplate>
                                    <div class="row" style="padding-left: 50px; padding-bottom: 30px;">
                                        <div class="col-sm-6">
                                            <h3>Category</h3>
                                        </div>
                                        <div class="col-sm-6">
                                            <h3>Services Name</h3>
                                        </div>

                                    </div>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <div class="col-sm-12" style="margin-top: 0 !important">
                                        <div class="row dtlBoatType">
                                            <div class="col-sm-4">
                                                <h4 style="padding-top: 15px;">
                                                    <asp:Label ID="lblOthCatId" runat="server" Text='<%# Eval("ConfigId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblOthCatName" runat="server" Text='<%# Eval("ConfigName") %>'></asp:Label>
                                            </div>
                                            <div class="col-sm-8 pl-0 text-center btntbody">
                                                <asp:DataList ID="dtlOtherChild" runat="server" OnItemCommand="dtlOtherChild_ItemCommand">
                                                    <ItemTemplate>

                                                        <asp:Button ID="btnOtherShortName" runat="server" Text='<%# Bind("ShortName") %>' CssClass="button1"></asp:Button>

                                                        <div style="display: none;">
                                                            <asp:Label ID="lblCategoryName" runat="server" CssClass="form-control" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                                            <asp:Label ID="lblOthServiceId" runat="server" CssClass="form-control" Text='<%# Eval("ServiceId") %>'></asp:Label>
                                                            <asp:Label ID="lblOthServiceName" runat="server" CssClass="form-control" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                                            <asp:Label ID="lblServiceTotalAmount" runat="server" CssClass="form-control" Text='<%# Eval("ServiceTotalAmount") %>'></asp:Label>
                                                            <asp:Label ID="lblChargePerItem" runat="server" CssClass="form-control" Text='<%# Eval("AdultCharge") %>'></asp:Label>
                                                            <asp:Label ID="lblChargePerItemTax" runat="server" CssClass="form-control" Text='<%# Eval("ChargePerItemTax") %>'></asp:Label>
                                                            <asp:Label ID="lblTaxId" runat="server" CssClass="form-control" Text='<%# Eval("TaxId") %>'></asp:Label>
                                                            <asp:Label ID="lblTaxName" runat="server" CssClass="form-control" Text='<%# Eval("TaxName") %>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>

                        <div class="col-sm-5" style="margin-top: 5px; padding-right: 30px">
                            <div class="row m-0 pt-4 FinalSummary">

                                <div class="col-sm-12 p-0">
                                    <asp:GridView ID="gvOther" runat="server" CssClass="CustomGrid table table-bordered table-condenced grdTextAlign"
                                        AutoGenerateColumns="False" DataKeyNames="UniqueId, ServiceId, ServiceName,ServiceTotalAmount,ChargePerItem,ChargePerItemTax,
                                        AdultCount,TaxId,TaxName">
                                        <Columns>
                                            <asp:BoundField HeaderText="Service" DataField="ServiceName">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:TemplateField HeaderText="Count">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdultCount" runat="server" Text='<%# Bind("AdultCount") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtherGrandTotalAmount" runat="server" Text='<%# Bind("OtherGrandTotalAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgBtnDeleteOther" runat="server" ImageUrl="~/images/Close.svg" Width="20px" ToolTip="Delete"
                                                        OnClick="ImgBtnDeleteOther_Click" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grdHead" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle CssClass="gvHead" />
                                        <AlternatingRowStyle CssClass="gvRow" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                    </asp:GridView>
                                </div>

                                <div class="col-sm-12 p-0">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th colspan="2" class="text-center table-th">SUMMARY</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <th class="table-th">OTHER CHARGE</th>
                                                <td>₹ <span id="oschar1" runat="server"></span></td>
                                            </tr>
                                            <tr>
                                                <th class="table-th">GST</th>
                                                <td>₹ <span id="bsgst1" runat="server"></span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-sm-12 col-xs-12" style="padding: 15px 0px 0px 0px;">
                                    <div class="row m-0 p-0" runat="server" id="divpaymentType" visible="false" style="width: 100%;">
                                        <div class="col-sm-4 col-xs-12 p-0 text-center">
                                            <asp:DropDownList ID="ddlPaymentType" CssClass="payMode" runat="server"
                                                AutoPostBack="false" TabIndex="13">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 p-0 text-center">
                                            <span class="htmlHigh">
                                                <div class="tooltip-ex">
                                                    <asp:Button ID="btnOtherBooking" runat="server" Text="Submit" class="btn btn-primary btnFinal" ValidationGroup="BookingHeader" TabIndex="14"
                                                        OnClick="btnOtherBooking_Click" Width="150px" Font-Bold="True" Style="margin-left: 10px;" OnClientClick="showLoader();" />
                                                    <span class="tooltip-ex-text">
                                                        <span style="color: white;">Press Amount Button To Submit</span>
                                                    </span>
                                                </div>
                                            </span>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 text-center" style="padding: 0px 0px 0px 30px;">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger btnFinal" TabIndex="15"
                                                OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-xs-12 table-div" runat="server" id="divGridList" visible="false">
                    <div class="col-xs-12 table-responsive">
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </div>

                        <div id="divSearch" runat="server" style="text-align: right;">
                            Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="20px" placeholder="Enter Booking Id" AutoComplete="off" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
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

                        </asp:GridView>
                        <div style="text-align: left;">
                            <asp:Button ID="back" runat="server" CssClass="btn btn-color mg" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                            &nbsp
                                 <asp:Button ID="Next" Visible="true" CssClass="btn btn-color mg" runat="server" Text="Next →" OnClick="Next_Click" />
                            &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlReason"
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

                <div class="row">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <ajax:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="pnlBillService" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
                    <ajax:ModalPopupExtender ID="MpeBillService" runat="server" BehaviorID="MpeBillService" TargetControlID="HiddenField1" PopupControlID="pnlBillService"
                        BackgroundCssClass="modalBackground">
                    </ajax:ModalPopupExtender>

                    <asp:Panel ID="pnlBillService" runat="server" CssClass="Msg" Style="display: none; height: 750px; width: 500px;">
                        <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
                            <div class="maincontent text-right">
                                <h4>
                                    <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="printDiv()" ImageUrl="~/images/Print.svg" Width="35px" ToolTip="Print" />
                                    <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                                </h4>
                            </div>
                            <div id='DivIdToPrint' style="padding: 15px 5px 15px 5px; overflow-y: scroll; height: 640px; width: 500px; text-align: center;">


                                <asp:DataList ID="dtlistTicketOther" runat="server" OnItemDataBound="dtlistTicketOther_ItemDataBound">
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
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/TTDCLogo.svg" />
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


                                <%-- <asp:DataList ID="dtlistTicketOther" runat="server" OnItemDataBound="dtlistTicketOther_ItemDataBound">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%">
                                            <tr>
                                                <td class="text-left font-weight-bold">
                                                    <h3>
                                                        <asp:Label ID="Label15" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label></h3>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold">
                                                    <h3>
                                                        <asp:Label ID="Label17" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label></h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgOtherQR1" runat="server" />
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
                                                <td colspan="3" class="text-center">
                                                    <h1>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/TTDCLogo.svg" /></h1>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgOtherQR" runat="server" />
                                                </td>
                                                <td colspan="2">
                                                    <h2>
                                                        <asp:Label ID="lblBillOBoatHouse" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label></h2>
                                                    <h3>                                                       
                                                        <asp:Label ID="Label16" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>

                                                    </h3>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text-left font-weight-bold" colspan="3">
                                                    <h3>
                                                        <asp:Label ID="lblBillCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label></h3>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text-left" colspan="2">
                                                    <h4>Receipt
                                                     
                                                    </h4>
                                                </td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h4>Total Tickets :
                                                        <asp:Label ID="Label24" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
                                                    </h4>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="3">
                                                    <p style="border: 0.5px dashed black; border-collapse: collapse" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text-left">
                                                    <h5>Service Fare </h5>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h5>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ServiceFare") %>'></asp:Label></h5>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td class="text-left">
                                                    <h5>Tax </h5>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h5>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%# Eval("TaxAmount") %>'></asp:Label></h5>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr style="border: 0.5px solid black;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text-left">
                                                    <h4>NET </h4>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h4>₹<asp:Label ID="lblBillONetAmount" runat="server" Text='<%# Eval("NetAmount") %>'></asp:Label></h4>
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
                                </asp:DataList>--%>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </div>
            </div>

            <asp:HiddenField ID="HiddenField3" runat="server" />
            <ajax:DragPanelExtender ID="DragPanelExtender3" runat="server" TargetControlID="pnlUserCounttl" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
            <ajax:ModalPopupExtender ID="MpeUserCount" runat="server" BehaviorID="MpeUserCount" TargetControlID="HiddenField3" PopupControlID="pnlUserCounttl"
                BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>

            <asp:Panel ID="pnlUserCounttl" runat="server" CssClass="Msg" Style="display: none; min-height: 350px; max-height: 620px; width: 1000px; margin-top: 30px;">
                <asp:Panel ID="Panel3" runat="server" CssClass="drag">
                    <div class="modal-content" style="width: 980px; max-height: 600px; min-height: 300px">
                        <div class="modal-header">
                            <h5 class="modal-title">Other Service Booked Details
                       <asp:Label ID="Label11" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </h5>
                            <asp:ImageButton ID="ImgClose" runat="server" OnClick="ImgClose_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                        </div>
                        <div class="modal-body">

                            <div class="table-responsive">
                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                    <asp:Label ID="Label15" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                                <asp:GridView ID="gvUserCountTotal" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid" ShowFooter="true"
                                    AutoGenerateColumns="False" PageSize="10" OnPageIndexChanging="gvUserCountTotal_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
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

                                    </Columns>
                                    <HeaderStyle CssClass="gvHead" />
                                    <AlternatingRowStyle CssClass="gvRow" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" />
                                </asp:GridView>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>

            <asp:HiddenField ID="hfBBpupup" runat="server" />

            <ajax:DragPanelExtender ID="DPEBBpopup" runat="server" TargetControlID="PnlBBRevenue" DragHandleID="pnlDragBBRevenue"></ajax:DragPanelExtender>
            <ajax:ModalPopupExtender ID="MPEBBpopup" runat="server" BehaviorID="MPEBBpopup" TargetControlID="hfBBpupup" PopupControlID="PnlBBRevenue"
                BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>

            <asp:Panel ID="PnlBBRevenue" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; width: 500px; margin-top: 25px;">
                <asp:Panel ID="pnlDragBBRevenue" runat="server" CssClass="drag">
                    <div class="modal-content">
                        <div class="modal-header" style="background-color: #004c8c; color: white">
                            <h5 class="modal-title">Other Service Payment Details</h5>
                            <asp:ImageButton ID="ImgCloseBB" runat="server" OnClick="ImgCloseBB_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                        </div>
                        <div class="modal-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gvBBpopup" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
                                    AutoGenerateColumns="False" ShowFooter="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Mode" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentModepop" runat="server" Text='<%# Bind("PaymentMode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatBookingRevenuepop" runat="server" Text='<%# Bind("OtherServiceRevenue") %>' Font-Bold="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />

                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gvHead" />
                                    <AlternatingRowStyle CssClass="gvRow" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <div id="loader" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8; display: none">
                <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
                    <img src="../images/BoatGIF.gif" />
            </div>

            <div id="HourGlass" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8; display: none">
                <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
                    <img src="../images/hourglass.gif" width="100px" height="100px" />
            </div>

            </span></span>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnOtherBooking" />
        </Triggers>
    </asp:UpdatePanel>
    <%--   <asp:UpdateProgress ID="UPprocessServer" runat="Server" AssociatedUpdatePanelID="UpdatePanel">
    </asp:UpdateProgress>--%>


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel">
        <ProgressTemplate>

            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
                    <img src="../images/BoatGIF.gif" /></span>
            </div>

            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <script type="text/javascript">
        function printDiv() {

            var divToPrint = document.getElementById('DivIdToPrint');
            var newWin = window.open('', 'Print-Window');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
            setTimeout(function () { newWin.close(); }, 10);
        }
    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

