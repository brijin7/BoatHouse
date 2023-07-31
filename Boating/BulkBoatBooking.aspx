<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="BulkBoatBooking.aspx.cs" Inherits="Boating_BulkBoatBooking" %>


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

        function numbersonly(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
                if (unicode < 48 || unicode > 57) //if not a number
                    return false //disable key press
            }
        }

        //function myColor() {
        //    $("input[type='submit'][id*=MainContent_dtlBoat_dtlBoatchild]").each(function () { this.style.backgroundColor = '#e0e0e0'; this.addClass("button1"); });
        //}
    </script>
    <script type="text/javascript">
        function ZeroKeyPress1() {
            var text1 = $("#<%=txtNoPersons.ClientID %>").val();
            if (text1 == "0" || text1 == "00" || text1 == "000") {
                $("#<%=txtNoPersons.ClientID %>").val('');
                $('#<%=txtNoPersons.ClientID%>').text('');
            }

        }
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
            myVar = setInterval(myTimer, 300);
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

    <script type="text/javascript">
        function addItem(id) {
            Inactivebutton();
            document.getElementById(id).style.backgroundColor = "#5cbf2a";
            $('#<%=hfInactivebutton.ClientID%>').val(id);
        }

        function Inactivebutton() {
            if ($('#<%=hfInactivebutton.ClientID%>').val() != "") {
                var Inactive = $('#<%=hfInactivebutton.ClientID%>').val();
                document.getElementById(Inactive).style.backgroundColor = "#5cbf2a";
            }
        }


        function addItemOther(id) {
            Inactivebuttonother();
            document.getElementById(id).style.backgroundColor = "#fff708b8";
            $('#<%=hfInactiveOther.ClientID%>').val(id);
        }

        function Inactivebuttonother() {
            if ($('#<%=hfInactiveOther.ClientID%>').val() != "") {
                var Inactive = $('#<%=hfInactiveOther.ClientID%>').val();
                document.getElementById(Inactive).style.backgroundColor = "#fff708b8";
            }
        }
    </script>



    <style>
        .buttonNor {
            background-color: white;
            border-radius: 10px;
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: 'Roboto', sans-serif;
            font-size: 18px;
            font-weight: 500;
            padding: 0.6rem 0.8rem;
            /* padding-left: 5px;
            padding-right: 5px;*/
            text-decoration: none;
            /* text-shadow: 0px 1px 0px #2f6627;*/
            outline: none;
            margin: 3px;
            margin-left: 15px;
            text-align: center;
            text-transform: uppercase;
            line-height: 1.5;
        }

        .buttonPre {
            background-color: gold;
            border-radius: 10px;
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: 'Roboto', sans-serif;
            font-size: 18px;
            font-weight: 500;
            padding: 0.6rem 0.8rem;
            /*padding-left: 5px;
            padding-right: 5px;*/
            text-decoration: none;
            /*text-shadow: 0px 1px 0px #2f6627;*/
            outline: none;
            margin: 3px;
            text-align: center;
            text-transform: uppercase;
            line-height: 1.5;
        }

        .buttonOth {
            background-color: #abf416 /*#9ACD32*/;
            border-radius: 10px;
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: 'Roboto', sans-serif;
            font-size: 18px;
            font-weight: 500;
            padding: 0.6rem 0.8rem;
            /* padding-left: 5px;
            padding-right: 5px;*/
            text-decoration: none;
            /* text-shadow: 0px 1px 0px #2f6627;*/
            outline: none;
            margin: 3px;
            text-align: center;
            text-transform: uppercase;
            line-height: 1.5;
        }


            .buttonNor:hover, .buttonPre:hover, .buttonOth:hover {
                background-color: #203e9e;
                color: white;
            }


            .buttonNor:active, .buttonPre:active, .buttonOth:active {
                position: relative;
                top: 1px;
            }


        .tooltip-Deposit {
            /* Container for our tooltip */
            position: relative;
            cursor: pointer;
        }

            .tooltip-Deposit .tooltip-Deposit-text { /* This is for the tooltip text */
                visibility: hidden;
                width: 340px;
                border: 0px solid black;
                background-color: rgba(0,181,204,1);
                color: white;
                text-align: center;
                padding: 8px 1px;
                border-radius: 11px;
                position: absolute;
                z-index: 1;
                bottom: 27px;
                left: 11%;
                margin-left: -54px;
                margin-bottom: 4px;
            }


            .tooltip-Deposit:hover .tooltip-Deposit-text { /* Makes tooltip text visible when text is hovered on */
                visibility: visible;
            }

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


        .buttonColor {
            background-color: #e0e0e0;
            border-radius: 28px;
            border: 1px solid #18ab29;
            display: inline-block;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            /* padding: 8px 8px; */
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: 1px;
        }

            .buttonColor:hover {
                background-color: #5cbf2a;
                border: 1px solid #5cbf2a;
            }


        .button1 {
            background-color: #e0e0e000;
            border-radius: 28px;
            border: 1px solid #e0e0e000;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            /*padding: 8px 8px;*/
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: 1px;
        }

        .button2 {
            background-color: #f96363;
            border-radius: 28px;
            border: 1px solid #e0e0e000;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: -1px;
        }

        .button1:hover {
            background-color: #5cbf2a;
            border: 1px solid #5cbf2a;
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
            border-radius: 1.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }

        .btnPin {
            display: inline-block;
            font-weight: 300;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            border: 1px solid transparent;
            padding: 5px 5px;
            font-size: 1.5rem !important;
            border-radius: 3px;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }

        .textpin {
            font-weight: 300;
            text-align: center;
            height: 50px;
            border: 1px solid transparent;
            padding: 5px 5px;
            font-size: 1.5rem !important;
            border-radius: 3px;
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

        .select {
            /*text-align: center;
            text-indent: 5px;
            text-align-last: center;
            -moz-text-align-last: center;*/
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
            font-size: 2rem !important;
            line-height: 1.5;
            border-radius: 2.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            outline: none;
            padding-left: 40px;
        }

        .buttonOther {
            background-color: #e0e0e000;
            border-radius: 28px;
            border: 1px solid #e0e0e000;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: 1px;
        }

            .buttonOther:hover {
                background-color: #fff708b8;
            }

        .buttonColorOther {
            background-color: #e0e0e0;
            border-radius: 28px;
            border: 1px solid #18ab29;
            display: inline-block;
            color: #0d0c0c;
            font-family: Arial;
            font-size: 28px;
            /* padding: 8px 8px; */
            text-decoration: none;
            text-shadow: 0px 1px 0px #2f6627;
            outline: none;
            margin: 1px;
        }

            .buttonColorOther:hover {
                background-color: #fff708b8;
                border: 1px solid #fff708b8;
            }

        .blink {
            animation: blink-animation 1s linear infinite;
            -webkit-animation: blink-animation 1s linear infinite;
        }

        @keyframes blink-animation {
            50% {
                opacity: 0;
            }
        }

        @-webkit-keyframes blink-animation {
            50% {
                opacity: 0;
            }
        }
    </style>

    <style>
        .example {
            position: relative;
            padding: 0;
            width: 100px;
            display: block;
        }

        .content {
            margin: 5px;
            margin-left: 20px;
            border-radius: 28px;
            opacity: 1;
            font-size: 23px;
            position: absolute;
            cursor: not-allowed;
            top: 0;
            left: 0;
            color: black;
            font-weight: bolder;
            background-color: #f96363;
            width: 60px;
            height: 46px;
            -webkit-transition: all 100ms ease-out;
            -moz-transition: all 100ms ease-out;
            -o-transition: all 100ms ease-out;
            -ms-transition: all 100ms ease-out;
            transition: all 100ms ease-out;
            text-align: center;
        }

        .example .content:hover {
            opacity: 1;
        }

        .example .content {
            height: 0;
            opacity: 1;
            color: #c8c8c800;
        }

            .example .content:hover .text {
                opacity: 1;
                transform: translateY(5px);
                -webkit-transform: translateY(5px);
                color: white;
                border-radius: 28px;
                background-color: #f96363;
                text-shadow: 1px 1px 2px black, 0 0 25px red, 0 0 5px red;
                margin-top: -21px;
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
            background-color: #272c3100;
        }

        ::-webkit-scrollbar-thumb {
            border-radius: 8px;
            background-color: #dee2e6;
        }

            ::-webkit-scrollbar-thumb:hover {
                background-color: #dee2e6;
            }
    </style>

    <style>
        .box {
            box-shadow: inset 0 -3em 3em rgba(0,0,0,0.1), 0 0 0 2px rgb(255,255,255), 0.3em 0.3em 1em rgba(0,0,0,0.3);
            width: 85%;
            background: white;
            color: black;
            border: 1px solid white;
            /*margin-top: -48%;*/
            height: 400px;
            /*position: fixed;*/
            position: fixed !important;
            top: 15.4em;
            left: 7em;
            right: 0em;
        }

        .box-inner {
            padding: 10px;
        }

        .SlideCheck {
            background-color: white;
            border: none;
            color: #203e9e;
            padding: 5px 32px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 0px 0px;
            cursor: pointer;
            width: 100%;
            position: fixed;
            bottom: 30px;
            right: 0px;
            left: 0px;
            height: 30px;
            font-weight: 600;
        }

        .form-body1 {
            margin: 10px 5px;
            background: #ffffff;
            padding: 10px 2px;
            padding-left: 10px;
        }
    </style>

    <script type="text/javascript">

        function divexpandcollapse(divname) {

            var div = document.getElementById(divname);

            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {

                div.style.display = "inline";

                img.src = "../images/Minus.png";

            } else {

                div.style.display = "none";

                img.src = "../images/Add.png";

            }
        }

    </script>

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
            <div class="form-body1">
                <div class="row" runat="server" visible="false" id="divShow">
                    <div class="col-sm-12 col-md-12 text-center">
                        <h5 class="pghr" style="text-align: center; font-size: 25px; display: inline;">Bulk Boat Booking  </h5>
                        <span style="float: right; display: inline;">
                            <asp:ImageButton ID="imgbtnNewBook" runat="server" ImageUrl="~/images/Ticket.svg" Width="40px" 
                                OnClick="imgbtnNewBook_Click" OnClientClick="StartTimer()" ToolTip="New Booking" CssClass="pr-2" Visible="false" />
                        </span>
                    </div>
                </div>

                <div runat="server" id="divEntry" style="width: 100%">
                    <div class="row">
                        <div class="col-md-2 col-sm-2">
                            <h3 id="Time" style="display: inline; float: left; color: black; font-size: 1.3rem"></h3>
                        </div>
                        <div class="col-md-3 col-sm-3 text-center">
                            <h5 class="pghr" style="text-align: center; font-size: 20px; display: inline;">Bulk Boat Booking  </h5>
                        </div>

                        <div class="col-md-2 col-sm-2">
                            <asp:CheckBox runat="server" Style="zoom: 1.5;" Width="17px" Height="21px" ID="chkCustMobileNo" /><b style="font-size: 20px">Cust Mobile No</b>
                        </div>

                        <div class="col-md-2 col-sm-2">
                            <asp:CheckBox runat="server" Style="zoom: 1.5;" Width="17px" Height="21px" ID="chkGSTNo" OnCheckedChanged="chkGSTNo_CheckedChanged" AutoPostBack="true" /><b style="font-size: 20px">Cust GST No.</b>
                        </div>

                        <div class="col-md-3 col-sm-3">
                            <h3 id="Date" style="display: inline; overflow: auto; float: right; padding-left: 20px; color: black; font-size: 1.3rem"></h3>
                            <asp:ImageButton ID="imgbtnBookedList" runat="server" Style="float: right" ImageUrl="~/images/Print.svg"
                                Width="33px" OnClick="imgbtnBookedList_Click" OnClientClick="StopTimer()" ToolTip="Ticket Re-Print" />
                        </div>
                    </div>

                    <hr />

                    <div class="col-xs-12" style="background-color: cornsilk">
                        <div class="row">
                            <div class="col-md-3 col-sm-3">
                                <b style="font-size: 18px">Booking Count :</b>
                                <asp:LinkButton ID="bblblCount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"
                                    OnClick="bblblCount_Click" Font-Underline="true"></asp:LinkButton>
                            </div>

                            <div class="col-md-3 col-sm-3">
                                <b style="font-size: 18px">Amount :</b>
                                <%-- <asp:LinkButton ID="bblblAmount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"
                                    OnClick="bblblAmount_Click" Font-Underline="true"></asp:LinkButton>--%>
                                <asp:Label ID="bblblAmount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"></asp:Label>
                            </div>

                            <div class="col-md-3 col-sm-3">
                                <b style="font-size: 18px">Deposit :</b>
                                <asp:Label ID="bblblDeposit" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"></asp:Label>
                            </div>
                            <div class="col-md-3 col-sm-3">
                                <b style="font-size: 18px">Net Amount :</b>
                                <asp:LinkButton ID="bblblNetAmount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"
                                    OnClick="bblblNetAmount_Click" Font-Underline="true"></asp:LinkButton>
                                <%--<asp:Label ID="bblblNetAmount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"></asp:Label>--%>
                            </div>
                        </div>
                    </div>

                    <%-- BOAT BOOKING REVENUE POPUP--%>

                    <asp:HiddenField ID="hfBBpupup" runat="server" />

                    <ajax:DragPanelExtender ID="DPEBBpopup" runat="server" TargetControlID="PnlBBRevenue" DragHandleID="pnlDragBBRevenue"></ajax:DragPanelExtender>
                    <ajax:ModalPopupExtender ID="MPEBBpopup" runat="server" BehaviorID="MPEBBpopup" TargetControlID="hfBBpupup" PopupControlID="PnlBBRevenue"
                        BackgroundCssClass="modalBackground">
                    </ajax:ModalPopupExtender>

                    <asp:Panel ID="PnlBBRevenue" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; width: 500px; margin-top: 25px;">
                        <asp:Panel ID="pnlDragBBRevenue" runat="server" CssClass="drag">
                            <div class="modal-content">
                                <div class="modal-header" style="background-color: #004c8c; color: white">
                                    <h5 class="modal-title">Boat Booking Revenue Details</h5>
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
                                                        <asp:Label ID="lblBoatBookingRevenuepop" runat="server" Text='<%# Bind("BoatBookingRevenue") %>' Font-Bold="true"></asp:Label>
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

                    <div class="row" runat="server" id="divBack">

                        <div class="col-sm-7 col-xs-12" style="margin-top: 10px">
                            <div style="padding-bottom: 30px;">

                                <div class="input-group-prepend">
                                    <div>
                                        <label for="Persons" id="lblPersons" style="font-size: 35px;"><i class="fa fa-user" aria-hidden="true"></i>No of Persons</label>
                                    </div>
                                    <div>
                                        <asp:TextBox ID="txtNoPersons" runat="server" AutoComplete="Off"
                                            MaxLength="3" TabIndex="1" Width="100px" onkeyup="ZeroKeyPress1()" onkeypress="return numbersonly(event)" Height="50px" Font-Size="35px" Font-Bold="true">
                                        </asp:TextBox>
                                    </div>
                                    <div>
                                        <asp:Button ID="BackToBooking" runat="server" CssClass="btn btn-primary buttonback" Width="151px" Height="50px" Font-Bold="true" Style="margin-left: 50px;" OnClick="BackToBooking_Click" OnClientClick="showHourGlass();" Text="<< Back To Booking" Visible="true" />
                                    </div>
                                </div>

                                <div class="row input-group-prepend">
                                    <div class="d-flex justify-content-start" style="padding-left: 15px;">
                                        <asp:Button runat="server" ID="btnNor" OnClick="btnNor_Click" Text="Normal Boat" CssClass="buttonNor block" Visible="false" />
                                    </div>
                                    <div class="d-flex justify-content-center">
                                        <asp:Button runat="server" ID="btnPre" OnClick="btnPre_Click" Text="Express Boat" CssClass="buttonPre block" Visible="false" />
                                        <br />
                                        <span id="premimumMsg" runat="server" style="color: red; font-size: 23px;" visible="false"></span>
                                    </div>
                                    <div class="d-flex justify-content-end" style="font-size: 12px">
                                        <asp:Button runat="server" ID="btnOther" OnClick="btnOther_Click" Text="Other Services" CssClass="buttonOth block" Visible="false" />
                                    </div>
                                </div>
                            </div>

                            <asp:DataList ID="dtlBoat" runat="server" OnItemDataBound="DtlBoat_ItemDataBound" Width="100%">
                                <HeaderTemplate>
                                    <div class="row" style="padding-left: 50px; padding-bottom: 30px;">
                                        <div class="col-sm-6">
                                            <h3>Boat Type</h3>
                                        </div>
                                        <div class="col-sm-3">
                                            <h3>Seat Type</h3>
                                        </div>

                                        <div class="input-group-prepend col-sm-3">
                                            <asp:Panel runat="server" ID="pnlBoatCount" Visible="false">
                                                <label for="Boats" id="lblBoats" style="font-size: 16px;"><i class="fa fa-ship" aria-hidden="true"></i>No of Boats</label>
                                                <asp:DropDownList runat="server" ID="dlstBoatCount" AutoPostBack="true" CssClass="select"
                                                    OnSelectedIndexChanged="dtlBoat_SelectedIndexChanged">
                                                    <asp:ListItem>- -</asp:ListItem>
                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                </asp:DropDownList>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div>
                                        <div class="col-sm-12" style="margin-top: 0 !important">
                                            <div class="row dtlBoatType" style="overflow: hidden">
                                                <div class="col-sm-6">
                                                    <h4 style="padding-top: 15px;">
                                                        <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Eval("BoatTypeId") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblBtBoatType" runat="server" Text='<%# Eval("BoatType") %>'></asp:Label></h4>
                                                </div>

                                                <div class="col-sm-6 pl-0 text-center btntbody">
                                                    <asp:DataList ID="dtlBoatchild" runat="server" OnItemCommand="dtlBoatchild_ItemCommand">
                                                        <ItemTemplate>

                                                            <div runat="server" id="divbtnSeatype">
                                                                <span id="<%# Eval("BoatTypeId") %><%# Eval("BoatSeaterId") %>"
                                                                    onclick="addItem('<%# Eval("BoatTypeId") %><%# Eval("BoatSeaterId") %>')" class="buttonColor">

                                                                    <asp:Button ID="btnSeaterType" runat="server" Text='<%# Bind("NoOfSeats") %>'
                                                                        Enabled='<%# Eval("RemainTrips").ToString() == "0"? true: true %>'
                                                                        CssClass='<%# Convert.ToInt16(Eval("RemainTrips").ToString()) <= Convert.ToInt16("0")  ? "button2" :  "button1" %>'
                                                                        Width="60px"></asp:Button>

                                                                    <%-- <asp:Button ID="btnSeaterType" runat="server" Text='<%# Bind("NoOfSeats") %>'
                                                                        Enabled='<%# Eval("RemainTrips").ToString() == "0"? false: true %>'
                                                                        CssClass='<%# Eval("RemainTrips").Equals("0") ? "button2 example content text" : "button1" %>'
                                                                        Width="60px"></asp:Button>--%>
                                                                </span>

                                                                <%-- <div class="example">
                                                                    <div class="content">
                                                                        <div class="text">
                                                                            SOLD
                                                                        </div>
                                                                    </div>
                                                                </div>--%>
                                                            </div>

                                                            <div style="display: none">
                                                                <asp:Label ID="lblBoatTypes" runat="server" Text='<%# Bind("BoatType") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblSeaterTypes" runat="server" Text='<%# Bind("SeaterType") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblNoOfSeats" runat="server" Text='<%# Bind("NoOfSeats") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblBoatTotalCharge" runat="server" Text='<%# Bind("BoatTotalCharge") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblBoatMinCharge" runat="server" Text='<%# Bind("BoatMinCharge") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblRowerMinCharge" runat="server" Text='<%# Bind("RowerMinCharge") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblBoatTaxCharge" runat="server" Text='<%# Bind("BoatTaxCharge") %>' Font-Bold="true" Width="100px"></asp:Label>

                                                                <asp:Label ID="lblDepositType" runat="server" Text='<%# Bind("DepositType") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                                <asp:Label ID="lblDeposit" runat="server" Text='<%# Bind("Deposit") %>' Font-Bold="true" Width="100px"></asp:Label>

                                                                <asp:Label ID="lblBoatMinTime" runat="server" Text='<%# Bind("BoatMinTime") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>


                            <asp:DataList ID="dtlOther" runat="server" Width="100%" OnItemDataBound="DtlOther_ItemDataBound">
                                <HeaderTemplate>
                                    <div class="row" style="padding-left: 50px; padding-bottom: 30px;">
                                        <div class="col-sm-4">
                                            <h3>Category</h3>
                                        </div>
                                        <div class="col-sm-4">
                                            <h3>Services Name</h3>
                                        </div>

                                        <div class="input-group-prepend col-sm-4">
                                            <asp:Panel runat="server" ID="pnlTicketCount" Visible="false">
                                                <label for="Boats" id="lblTickets" style="font-size: 22px;"><i class="fa fa-user" aria-hidden="true"></i>No of Tickets</label>
                                                <asp:DropDownList runat="server" ID="dlstTicketsCount" AutoPostBack="true" CssClass="select"
                                                    OnSelectedIndexChanged="dlstTicketsCount_SelectedIndexChanged">
                                                    <asp:ListItem>- -</asp:ListItem>
                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="11">11</asp:ListItem>
                                                    <asp:ListItem Value="12">12</asp:ListItem>
                                                    <asp:ListItem Value="13">13</asp:ListItem>
                                                    <asp:ListItem Value="14">14</asp:ListItem>
                                                    <asp:ListItem Value="15">15</asp:ListItem>
                                                    <asp:ListItem Value="16">16</asp:ListItem>
                                                    <asp:ListItem Value="17">17</asp:ListItem>
                                                    <asp:ListItem Value="18">18</asp:ListItem>
                                                    <asp:ListItem Value="19">19</asp:ListItem>
                                                    <asp:ListItem Value="20">20</asp:ListItem>
                                                    <asp:ListItem Value="21">21</asp:ListItem>
                                                    <asp:ListItem Value="22">22</asp:ListItem>
                                                    <asp:ListItem Value="23">23</asp:ListItem>
                                                    <asp:ListItem Value="24">24</asp:ListItem>
                                                    <asp:ListItem Value="25">25</asp:ListItem>
                                                </asp:DropDownList>
                                            </asp:Panel>
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

                                                        <div runat="server" style="padding: 3px 4px">
                                                            <span id="<%# Eval("ServiceId") %>"
                                                                onclick="addItemOther('<%# Eval("ServiceId") %>')" class="buttonColorOther">

                                                                <asp:Button ID="btnOtherShortName" runat="server" Text='<%# Bind("ShortName") %>'
                                                                    CssClass="buttonOther"></asp:Button>
                                                            </span>
                                                        </div>

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

                        <div class="col-sm-5" style="margin-top: 5px; padding-right: 20px">
                            <div class="row m-0 pt-4 FinalSummary">
                                <div class="col-sm-12 p-0">
                                    <asp:GridView ID="gvBoatdtl" runat="server" CssClass="CustomGrid table table-bordered table-condenced grdTextAlign"
                                        AutoGenerateColumns="False" DataKeyNames="UniqueId,BoatType,BoatTypeId,SeaterType,SeaterTypeId,BoatCount,BoatTotalCharge,BoatMinCharge,RowerMinCharge,BoatTaxCharge,DepositType,Deposit,BoatMinTime,Status,SlotId,SlotType">
                                        <Columns>
                                            <asp:BoundField HeaderText="Boat Type" DataField="BoatType" />
                                            <asp:BoundField HeaderText="Seater Type" DataField="SeaterType" />
                                            <asp:BoundField HeaderText="Status" DataField="Status" />
                                            <asp:BoundField HeaderText="SlotType" DataField="SlotType" />

                                            <asp:TemplateField HeaderText="Count">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatCount" runat="server" Text='<%# Bind("BoatCount") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="Person" DataField="PersonCount" />
                                            <asp:BoundField HeaderText="Boat Charge" DataField="BoatTotalCharge" />


                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/Close.svg" Width="20px" ToolTip="Delete"
                                                        OnClick="ImgBtnDelete_Click" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grdHead" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="gvHead" />
                                        <AlternatingRowStyle CssClass="gvRow" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                    </asp:GridView>

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
                                    <table class="table mt-4 mb-0">
                                        <thead>
                                            <tr>
                                                <th colspan="2" class="text-center table-th">SUMMARY</th>
                                            </tr>
                                        </thead>
                                        <!-- <tbody id="otherServicesBody"></tbody> -->
                                        <tbody>
                                            <tr>
                                                <th class="table-th">BOAT CHARGE</th>
                                                <td>₹ <span id="bschar1" runat="server"></span></td>
                                            </tr>
                                            <tr>
                                                <th class="table-th">OTHER CHARGE</th>
                                                <td>₹ <span id="oschar1" runat="server"></span></td>
                                            </tr>
                                            <tr>
                                                <th class="table-th">GST</th>
                                                <td>₹ <span id="bsgst1" runat="server"></span></td>
                                            </tr>


                                            <tr class="tooltip-Deposit">
                                                <th class="table-th">DEPOSIT <span class="spStar">*</span>
                                                    <span class="tooltip-Deposit-text" id="toolTipDpt" runat="server">
                                                        <span style="color: white;"></span>
                                                    </span>
                                                </th>
                                                <td>₹ <span id="bsdeposit1" runat="server"></span></td>
                                            </tr>


                                            <tr runat="server">
                                                <th class="table-th">Total</th>
                                                <td>₹ <span id="bsTotal" runat="server" style="color: red; font-weight: bolder"></span></td>
                                            </tr>

                                            <tr runat="server" visible="false" id="divOfferDiscount">
                                                <th class="table-th">Offer/Discount</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlOfferDiscount" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlOfferDiscount_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr runat="server">
                                                <td colspan="2">
                                                    <h6>
                                                        <asp:Label ID="lblOfferAlert" runat="server" Text='' CssClass="blink" Font-Bold="true" ForeColor="#FC7651"></asp:Label>
                                                        <asp:Label ID="lblInfo" runat="server" Text='' CssClass="blink" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                    </h6>
                                                </td>
                                            </tr>

                                            <tr runat="server">
                                                <th class="table-th">Discount
                                                    <asp:Label ID="lblBoatChargeOnly" runat="server" Text=""></asp:Label>
                                                </th>
                                                <td>₹
                                                    <asp:Label ID="lblOfferAmountAndTaxAmount" runat="server" Text="0"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr runat="server">
                                                <td colspan="2">
                                                    <h6>
                                                        <asp:Label ID="lblNoofPersonAlert" runat="server" Text='' CssClass="blink" Font-Bold="true"></asp:Label>
                                                    </h6>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                                <div class="col-sm-9  col-xs-9" id="divGST" runat="server" visible="false">
                                    <%--  <label style="font-weight:bolder;color:black"><b>GST NO</b></label>--%>
                                    <asp:TextBox ID="txtINSGSTNO" runat="server" CssClass="form-control" AutoComplete="Off"
                                        MaxLength="15" TabIndex="6" placeholder="Enter GST No">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtINSGSTNO"
                                        ValidationGroup="ValGSTNO" SetFocusOnError="True" CssClass="vError">Enter GST NO</asp:RequiredFieldValidator>

                                    <br />
                                </div>

                                <div class="col-sm-12 col-xs-12" style="border-top: 1px solid #dee2e6; padding: 15px 0px 0px 0px;" runat="server" id="divCustMobile" visible="false">
                                    <div class="col-sm-6 col-xs-12 p-0" style="width: 100px">
                                        <asp:TextBox ID="txtCustMobileNo" runat="server" placeholder="Mobile No"
                                            onkeypress="return isNumber(event)" MaxLength="10" CssClass="form-control textpin" Width="190px" BackColor="White"
                                            ForeColor="Black" BorderColor="Blue" Font-Bold="true" AutoPostBack="true" AutoComplete="off" OnTextChanged="txtCustMobileNo_TextChanged"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12" style="border-top: 1px solid #dee2e6; padding: 15px 0px 0px 0px;" runat="server" id="divCustPin">
                                    <div class="row m-0 p-0" runat="server" id="divPin" visible="false">
                                        <div class="col-sm-5 col-xs-12 p-0" style="width: 100px">
                                            <asp:TextBox ID="txtPIN" runat="server" placeholder="PIN"
                                                onkeypress="return isNumber(event)" MaxLength="4" CssClass="form-control textpin" Width="120px" BackColor="White"
                                                ForeColor="Black" BorderColor="Blue" Font-Bold="true" AutoPostBack="true" AutoComplete="false" OnTextChanged="txtPIN_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3 col-xs-12 p-0">
                                            <asp:Image ID="imgPinStatus" runat="server" />
                                            <asp:Label ID="lblUserMobileNo" runat="server" Text="" Style="background-color: white;" ForeColor="Black" BorderColor="White" Font-Bold="true" Font-Size="12pt"></asp:Label>
                                        </div>
                                        <div class="col-sm-3 col-xs-12 p-0">
                                            <asp:Button ID="btnDefaultPin" runat="server" Text="Default Pin" class="btn btn-primary btnPin" OnClick="btnDefaultPin_Click" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-xs-12" style="padding: 15px 0px 0px 0px;">
                                    <div class="row m-0 p-0" runat="server" id="divBooking" visible="false">
                                        <div class="col-sm-4 col-xs-12 p-0 text-center">
                                            <asp:DropDownList ID="ddlPaymentType" CssClass="payMode" runat="server" AutoPostBack="false" TabIndex="13"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 p-0 text-center">
                                            <span class="htmlHigh">
                                                <div class="tooltip-ex">
                                                    <asp:Button ID="btnBoatBooking" runat="server" Text="Submit" class="btn btn-primary btnFinal" ValidationGroup="ValGSTNO" TabIndex="14"
                                                        OnClick="btnBoatBooking_Click" Width="150px" Font-Bold="True" Style="margin-left: 10px;" OnClientClick="showLoader();" />
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

                                        <div class="col-sm-12 col-xs-12">
                                            <div class="row" style="padding-top: 10px">
                                                <div class="col-sm-6 col-xs-12 p-0 text-center">
                                                    <asp:TextBox ID="txtAmountPaid" runat="server" CssClass="form-control textpin" AutoComplete="Off"
                                                        MaxLength="5" TabIndex="6" Width="220px" Height="50px" placeholder="Collected Amount"
                                                        ForeColor="Black" BorderColor="Black" Font-Bold="true" onkeyup="BalanceChk()" onkeypress="return isNumber(event)">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-sm-6 col-xs-12 p-0 text-center">
                                                    <asp:TextBox ID="txtBalanceAmnt" runat="server" CssClass="form-control textpin" AutoComplete="Off"
                                                        MaxLength="5" TabIndex="6" Width="220px" Height="50px" placeholder="Balance Amount"
                                                        ForeColor="Black" BorderColor="Black" Font-Bold="true" onkeypress="return isNumber(event)" ReadOnly="true">
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField runat="server" ID="hfchkoffer" />
                            </div>
                        </div>
                    </div>
                </div>

                <style>
                    .pin {
                        font-size: 32px;
                        font-weight: bold;
                        width: 90px;
                        border-radius: 5px;
                        background-color: #ffffff;
                        /*transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;*/
                        padding-left: 5px;
                        padding-right: 5px;
                        outline: none;
                    }
                </style>

                <div class="col-xs-12 table-div" runat="server" id="divGridList" visible="false">
                    <div class="col-xs-12 table-responsive">
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </div>

                        <div id="divSearch" runat="server" style="text-align: right;">
                            Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="20px" placeholder="Enter Booking Id/ Pin" AutoComplete="off" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                        <asp:GridView ID="GvBoatBooking" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                            AutoGenerateColumns="False" DataKeyNames="BookingId,Status,CustomerMobile" PageSize="25000">
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

                                <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Date & Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Customer Mobile" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Type" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingStatus" runat="server" Text='<%# Bind("BookingStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Initial Amount" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInitBillAmount" runat="server" Text='<%# Bind("InitBillAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Other Service Amount" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOtherService" runat="server" Text='<%# Bind("OtherService") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Print" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                            runat="server" Font-Bold="true" ImageUrl="~/images/Print.svg" OnClick="ImgBtnEdit_Click" ToolTip="Print" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="SMS" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBtnSMS" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                            runat="server" Font-Bold="true" ImageUrl="~/images/esms.svg" OnClick="ImgBtnSMS_Click" ToolTip="SMS"
                                            Visible='<%# Eval("CustomerMobile").ToString() == ""? false: true %>' Style="outline: none" />
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
                                <asp:DataList ID="dtlistTicket" runat="server" OnItemDataBound="dtlistTicket_ItemDataBound">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table runat="server" id="divrower" style="width: 100%;">
                                            <tr>
                                                <td class="text-left font-weight-bold">
                                                    <h3>PIN:<asp:Label ID="lblBookingPin" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                                        <asp:Label ID="lblBookingId" runat="server" Text='<%# Eval("BookingId") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Eval("BoatReferenceNo") %>' Visible="false"></asp:Label>
                                                    </h3>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold">
                                                    <h3>
                                                        <asp:Label ID="Label12" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label></h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgQRBRoCopy" runat="server" />
                                                </td>
                                                <td colspan="2" class="font-weight-bold">
                                                    <h4>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                                        Boat - Rower Copy </h4>
                                                    <h6>
                                                        <asp:Label ID="Label6" runat="server" Text='Booking Type:' Font-Bold="false"></asp:Label>
                                                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("PremiumStatus") %>' Font-Bold="true"></asp:Label></h6>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                                    <h6>Bulk Booking Id </h6>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h6>
                                                        <asp:Label ID="Label22" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label></h6>
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
                                                        <asp:Label ID="Label14" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label></h6>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                                    <h6>Boat Number</h6>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h6>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("ActualBoatNum") %>'></asp:Label></h6>
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

                                        <table style="width: 100%;">
                                            <tr>
                                                <td class="text-left font-weight-bold">
                                                    <h3>PIN:<asp:Label ID="Label27" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                                    </h3>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold">
                                                    <h3>
                                                        <asp:Label ID="Label23" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label></h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgQRBBoCopy" runat="server" />
                                                </td>
                                                <td colspan="2" class="font-weight-bold">
                                                    <h4>
                                                        <asp:Label ID="Label25" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label><br />
                                                        Boat - Boarding Pass</h4>

                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                                    <h6>Bulk Booking Id </h6>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h6>
                                                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label></h6>
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
                                                        <asp:Label ID="Label26" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label></h6>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                                    <h6>Boat Number</h6>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h6>
                                                        <asp:Label ID="lblActualBoatNum" runat="server" Text='<%# Eval("ActualBoatNum") %>'></asp:Label></h6>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                                    <h6>Expected Time</h6>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h6>
                                                        <asp:Label ID="lblExpectedTime" runat="server" Text='<%# Eval("ExpectedTime") %>'></asp:Label></h6>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text-left font-weight-bold" style="padding-left: 10px;">
                                                    <h4>NET </h4>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h4>₹
                                                        <asp:Label ID="Label28" runat="server" Text='<%# Eval("InitNetAmount") %>'></asp:Label></h4>
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
                                                        <asp:Label ID="lblOthBookingDate" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label></h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgOtherServiceQR" runat="server" />
                                                </td>
                                                <td colspan="2">
                                                    <h2>
                                                        <asp:Label ID="lblOthBoatHouseName" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label></h2>
                                                    <h3>
                                                        <asp:Label ID="lblOthServiceName" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                                    </h3>
                                                    <%-- <h3>  <asp:Label ID="lblBillOBookingType" runat="server" Text='<%# Eval("BookingType") %>'></asp:Label> </h3>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <h4>NET : ₹
                                                        <asp:Label ID="lblOtherNetAmount" runat="server" Text='<%# Eval("NetAmount") %>'></asp:Label></h4>
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
                                    </ItemTemplate>
                                </asp:DataList>
                                <asp:DataList ID="DLReceipt" runat="server" OnItemDataBound="DLReceipt_ItemDataBound">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%;" runat="server">
                                            <tr>
                                                <td class="text-left font-weight-bold">
                                                    <h3><%--PIN:<asp:Label ID="Label11" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>--%>
                                                    </h3>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold">
                                                    <h3>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("BookingDate") %>'></asp:Label></h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" class="text-center">
                                                    <h1>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/TTDCLogo.svg" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" class="text-center">
                                                    <h2>
                                                        <asp:Label ID="lblBillBoatHouse" runat="server" Text='<%# Eval("BoatHouseName") %>'></asp:Label></h2>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></h1><asp:Image ID="imgBoatBulkReceiptQR" runat="server" />
                                                </td>
                                                <td colspan="2" style="text-align: right">

                                                    <h4 style="float: right">
                                                        <asp:Label ID="Label7" runat="server" Text='Booking Type:' Font-Bold="false"></asp:Label>
                                                        <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Eval("PremiumStatus") %>' Font-Bold="true"></asp:Label>
                                                        <br />
                                                        <br />

                                                        <asp:Label ID="Label9" runat="server" Text='Bulk Booking Id : '></asp:Label>
                                                        <asp:Label ID="lblBillBookingId" runat="server" Text='<%# Eval("BookingId") %>' Font-Bold="true"></asp:Label>
                                                        <br />
                                                        <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text-left font-weight-bold" colspan="3">
                                                    <h3>
                                                        <asp:Label ID="lblBillCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label></h3>
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
                                                    <asp:Label ID="Label31" runat="server" Text='<%# Eval("PaymentTypeName") %>'></asp:Label></td>
                                            </tr>

                                            <tr runat="server" visible="true">
                                                <td class="text-left font-weight-bold">Boat Charge </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <asp:Label ID="lblBillBoatCharge" runat="server" Text='<%# Eval("BFDInitBoatCharge") %>'></asp:Label></td>
                                            </tr>
                                            <tr runat="server" visible="true">
                                                <td class="text-left font-weight-bold">Rower Charge</td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <asp:Label ID="lblRowerCharge" runat="server" Text='<%# Eval("BFDInitRowerCharge") %>'></asp:Label></td>
                                            </tr>

                                            <tr runat="server" visible="true">
                                                <td class="text-left font-weight-bold">Other Service</td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <asp:Label ID="Label29" runat="server" Text='<%# Eval("OtherService") %>'></asp:Label></td>
                                            </tr>

                                            <tr runat="server" visible="true">
                                                <td class="text-left font-weight-bold">Deposit Amount</td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <asp:Label ID="lblBillDeposit" runat="server" Text='<%# Eval("BoatDeposit") %>'></asp:Label></td>
                                            </tr>
                                            <tr runat="server" visible="true">
                                                <td class="text-left font-weight-bold">Tax Amount</td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <asp:Label ID="lblBillCGST" runat="server" Text='<%# Eval("BFDTaxAmount") %>'></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr style="border: 0.5px solid black;" />
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                                <td class="text-left font-weight-bold">
                                                    <h4>Total</h4>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h4>
                                                        <asp:Label ID="Label30" runat="server" Text='<%# Eval("BFDInitNetAmount") %>'></asp:Label></h4>
                                                </td>
                                            </tr>

                                            <tr runat="server">
                                                <td class="text-left font-weight-bold">
                                                    <h4>Discount</h4>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h4>
                                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("InitOfferAmount") %>'></asp:Label></h4>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text-left font-weight-bold">
                                                    <h4>NET </h4>
                                                </td>
                                                <td></td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h4>₹
                                            <asp:Label ID="lblBillinitNetAmount" runat="server" Text='<%# Eval("InitNetAmount") %>'></asp:Label></h4>
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
                                                        <asp:Label ID="lblGST" runat="server" Text='<%# Eval("GSTNumber") %>'></asp:Label></h5>
                                                </td>
                                            </tr>


                                            <tr runat="server" id="trBoatInsBulk">
                                                <td colspan="3" style="text-align: left;">
                                                    <span style="font-weight: 600;">Instructions :</span>
                                                    <asp:DataList ID="dtlisTicketInsBulk" runat="server" Style="text-align: left;">
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
                                </asp:DataList>
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
                            <h5 class="modal-title">Boat Booked Details
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
                                        <asp:TemplateField HeaderText="Customer Mobile" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerMobile" runat="server" Text='<%# Bind("CustomerMobile") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Booking Type" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingStatus" runat="server" Text='<%# Bind("BookingStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Initial Amount" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInitBillAmount" runat="server" Text='<%# Bind("InitBillAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Other Service Amount" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherService" runat="server" Text='<%# Bind("OtherService") %>'></asp:Label>
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



            <asp:HiddenField ID="HiddenField5" runat="server" />
            <ajax:DragPanelExtender ID="DragPanelExtender6" runat="server" TargetControlID="pnlPopup" DragHandleID="Mpepnl"></ajax:DragPanelExtender>
            <ajax:ModalPopupExtender ID="MpepnlPopup" runat="server" BehaviorID="MpepnlPopup" TargetControlID="HiddenField5" PopupControlID="pnlPopup"
                BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>

            <asp:Panel ID="pnlPopup" runat="server" CssClass="Msg" Style="left: -267px; top: -261px; display: none; max-width: 2000px; max-height: 800px; overflow: hidden; height: 800px; width: 2000px; background-color: rgb(71 71 71 / 40%); padding-top: 118px; border: none; border-radius: 8%;">
                <asp:Panel ID="Mpepnl" runat="server" CssClass="drag" Style="height: 450px; width: 505px; margin-top: 170px; margin-left: 709px;">
                    <%-- <div class="modal-header" style="background-color: #004c8c; color: white; margin-top: 0px;">
                            <h4 class="modal-title">Change Time Slot</h4>
                          
                        </div>--%>

                    <div class="modal-body" style="background-color: white; border-radius: 2%;">

                        <asp:Label ID="lblpopup" runat="server" Text="Available time slots are over, do you want to continue the booking?" Font-Bold="true" Font-Size="Large" Style="margin-left: 20px"></asp:Label>

                        <div class="col-xs-12" style="margin-left: 176px; margin-top: 20px;">
                            <asp:Button ID="btnok" runat="server" CssClass="btn btn-primary" TabIndex="8" Style="width: 78px;" Text="Ok" OnClick="btnok_Click" />
                            <asp:Button ID="BtnPopUpCancel" runat="server" CssClass="btn btn-danger" TabIndex="9" CausesValidation="false" Text="Cancel" OnClick="BtnPopUpCancel_Click" />

                        </div>

                    </div>
                </asp:Panel>
            </asp:Panel>

            <asp:HiddenField ID="hfBoatNature" runat="server" />
            <asp:HiddenField ID="hfNatureVisible" runat="server" Value="0" />
            <asp:HiddenField ID="hfInactivebutton" runat="server" />
            <asp:HiddenField ID="hfInactiveOther" runat="server" />


            <div style="margin-top: 18%; display: block;" runat="server">
                <asp:CheckBox ID="ChkAvailBoat" Text="Boat Available List" class="SlideCheck" runat="server" OnCheckedChanged="ChkAvailBoat_CheckedChanged" AutoPostBack="true" onchange="showHourGlass();" />
                <div runat="server" id="divTop" style="padding-top: 12px; position: fixed; display: block">
                    <div id="dvContent" class="box" runat="server">
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/Close.svg" OnClick="ImageButton3_Click" Width="30px" ToolTip="Close" Style="float: right" />
                        <br />
                        <div class="col-sm-12 col-xs-12" runat="server"
                            style="max-height: 300px; min-height: 50px; overflow-y: auto; float: right; position: relative;">
                            <div class="table-responsive" runat="server">
                                <div id="divAvailableBoats" runat="server" style="display: block">
                                    <asp:GridView ID="gvAvailableBoats" runat="server" CssClass="table table-bordered table-condenced CustomGrid" ShowFooter="false"
                                        AutoGenerateColumns="false" AllowPaging="true"
                                        OnRowCreated="grvMergeHeader_RowCreated" OnRowDataBound="gvAvailableBoats_RowDataBound">
                                        <Columns>

                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <a href="JavaScript:divexpandcollapse('div<%# Eval("BoatTypeId") %>');">
                                                        <img id="imgdiv<%# Eval("BoatTypeId") %>" width="15px" border="0" src="../images/Add.png" alt="" /></a>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" VerticalAlign="Middle"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BoatType Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Boat SeaterId" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="No Of Seats" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoOfSeats" runat="server" Text='<%# Bind("NoOfSeats") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Self Drive" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSelfDrive" runat="server" Text='<%# Bind("SelfDrive") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deposit Type" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepositType" runat="server" Text='<%# Bind("DepositType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeposit" runat="server" Text='<%# Bind("Deposit") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Time Extension" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTimeExtension" runat="server" Text='<%# Bind("TimeExtension") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Boat Minimum Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatMinTime" runat="server" Text='<%# Bind("BoatMinTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Boat Extension Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatExtnTime" runat="server" Text='<%# Bind("BoatExtnTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Boat Grace Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBoatGraceTime" runat="server" Text='<%# Bind("BoatGraceTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Maximum Trips Per Day" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaxTripsPerDay" runat="server" Text='<%# Bind("MaxTripsPerDay") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%--Total No.of bOATS--%>
                                            <asp:TemplateField HeaderText="Total Boats" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltotalnofBoats" runat="server" Text='<%# Bind("TotalNoofBoats") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%--Total No.of bOATS--%>

                                            <%--Total No.of Trips--%>
                                            <asp:TemplateField HeaderText="Total Trips" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblttlnofBoats" runat="server" Text='<%# Bind("TotalNoofTrips") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%--Total No.of Trips--%>

                                            <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNormalAvailable" runat="server" Text='<%# Bind("NormalAvailable") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Express" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumAvailable" runat="server" Text='<%# Bind("PremiumAvailable") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PremiumExpTripTime" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumExpTripTime" runat="server" Text='<%# Bind("PremiumExpTripTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NormalExpTripTime" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNormalExpTripTime" runat="server" Text='<%# Bind("NormalExpTripTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NormalMaxFare" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNormalMaxFare" runat="server" Text='<%# Bind("NormalMaxFare") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PremiumMaxFare" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumMaxFare" runat="server" Text='<%# Bind("PremiumMaxFare") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PremiumExpTripTime" HeaderStyle-CssClass="grdHead" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumExpTripTimess" runat="server" Text='<%# Bind("PremiumExpTripTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedNormalOnline" runat="server" Text='<%# Bind("BookedNormalOnline"  ) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Express" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedPremiumOnline" runat="server" Text='<%# Bind("BookedPremiumOnline") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedNormalBoatHouse" runat="server" Text='<%# Bind("BookedNormalBoatHouse") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Express" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedPremiumBoatHouse" runat="server" Text='<%# Bind("BookedPremiumBoatHouse") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNormalWaitingTrip" runat="server" Text='<%# Bind("NormalWaitingTrip") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Express" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumWaitingTrip" runat="server" Text='<%# Bind("PremiumWaitingTrip") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalNormalTrip" runat="server" Text='<%# Bind("TotalNormalTrip") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Express" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPremiumTrip" runat="server" Text='<%# Bind("TotalPremiumTrip") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNormalTripCompleted" runat="server" Text='<%# Bind("NormalTripCompleted") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Express" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPremiumTripCompleted" runat="server" Text='<%# Bind("PremiumTripCompleted") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>

                                                <ItemTemplate>
                                                    <tr>
                                                        <td colspan="100%" style="padding: 0px">
                                                            <div style="padding-left: 132px">
                                                                <div id="div<%# Eval("BoatTypeId") %>" style="overflow: auto; display: none; position: relative; left: 15px; overflow: auto">

                                                                    <asp:GridView ID="gv_Child" runat="server" Width="75%"
                                                                        AutoGenerateColumns="false" DataKeyNames="BoatTypeId" ShowFooter="true">

                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                                                                <ItemTemplate>
                                                                                    <%#Container.DataItemIndex+1 %>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSeaterId" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Available Trips" HeaderStyle-CssClass="grdHead">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblAvailTrps" runat="server" Text='<%# Bind("RemainTrips") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>

                                                                            <%-- New--%>
                                                                            <asp:TemplateField HeaderText="Available Boats" HeaderStyle-CssClass="grdHead">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblAvailBoats" runat="server" Text='<%# Bind("TotalNoofBoats") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <%-- New--%>
                                                                        </Columns>

                                                                        <HeaderStyle BackColor="#4D92C1" ForeColor="White" />

                                                                    </asp:GridView>
                                                                </div>
                                                        </td>

                                                    </tr>

                                                </ItemTemplate>

                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle CssClass="gvHead" />
                                        <AlternatingRowStyle CssClass="gvRow" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hfBalanceAmt" />

            <div id="loader" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8; display: none">
                <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
                    <img src="../images/BoatGIF.gif" />
            </div>

            <div id="HourGlass" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8; display: none">
                <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
                    <img src="../images/hourglass.gif" width="100px" height="100px" />
            </div>


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnBoatBooking" />
        </Triggers>
    </asp:UpdatePanel>
    <%--  <asp:UpdateProgress ID="UPprocessServer" runat="Server" AssociatedUpdatePanelID="UpdatePanel">
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
        window.onload = slideUpDownDiv();

        function slideUpDownDiv() {

            $('#<%=ChkAvailBoat.ClientID%>').click(function () {
                if (this.checked) {
                    $('#dvContent').slideDown();
                    document.getElementById('<%=divAvailableBoats.ClientID%>').style.display = "block";

                }
                else {
                    $('#dvContent').slideUp();
                    document.getElementById('<%=divAvailableBoats.ClientID%>').style.display = "block";

                }
            });
        }
        function CloseToggle() {
            $('#dvContent').slideUp();
            document.getElementById('<%=dvContent.ClientID%>').style.display = "none";
            document.getElementById('<%=ChkAvailBoat.ClientID%>').checked = false;

        }

    </script>


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

    <script type="text/javascript">
        function BalanceChk() {
            var TotalAmt = document.getElementById("<%=btnBoatBooking.ClientID%>").value;
            var AmountPaid = document.getElementById("<%=txtAmountPaid.ClientID%>").value;

            if (AmountPaid == "") {
                $('#<%=txtBalanceAmnt.ClientID%>').val("");
                $("#<%=hfBalanceAmt.ClientID %>").val("0.00");
                document.getElementById("<%=btnBoatBooking.ClientID%>").disabled = false;
            }
            else {
                var Balance = AmountPaid - TotalAmt;
                $('#<%=txtBalanceAmnt.ClientID%>').val(Balance);
                $("#<%=hfBalanceAmt.ClientID %>").val(Balance);
            }
            if (parseFloat(AmountPaid) < parseFloat(TotalAmt)) {
                $('#<%=txtBalanceAmnt.ClientID%>').val("0.00");
                $("#<%=hfBalanceAmt.ClientID %>").val("0.00");
                document.getElementById("<%=btnBoatBooking.ClientID%>").disabled = true;
            }
            if (parseFloat(AmountPaid) >= parseFloat(TotalAmt)) {

                document.getElementById("<%=btnBoatBooking.ClientID%>").disabled = false;
            }
        }
    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>
