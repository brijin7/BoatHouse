<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="TripSheetWeb.aspx.cs" Inherits="Boating_TripSheetWeb_Test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />
    <script type="text/javascript">
        function showHourGlass() {
            document.getElementById("HourGlass").style.display = 'block';
        }

    </script>
    <script lang="javascript" type="text/javascript">
        var myVar = setInterval(myTimer, 0);
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];
        const DayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        function myTimer() {
            var d = new Date();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            var day = d.getDay() + 1;
            //var dateime = d.toLocaleTimeString() + " " + d.getDate() + " " + monthNames[d.getMonth()] + " " + year;
            var date = DayNames[d.getDay()] + ", " + d.getDate() + "-" + monthNames[d.getMonth()];
            var Time = d.toLocaleTimeString();
            document.getElementById("Date").innerText = date;
            document.getElementById("Time").innerText = Time;
        }
    </script>
    <style>
        .panel-group .panel + .panel {
            margin-top: 5px;
        }

        .panel-group .panel {
            margin-bottom: 0;
            border-radius: 4px;
        }

        .panel-success {
            border: 1px;
            border-color: #d6e9c6;
        }

        .panel {
            margin-bottom: 20px;
            background-color: #fff;
            border: 1px solid transparent;
            border-radius: 4px;
            -webkit-box-shadow: 0 1px 1px rgba(0,0,0,.05);
            box-shadow: 0 1px 1px rgba(0,0,0,.05);
        }

        .panel-success > .panel-heading {
            color: #3c763d;
            background-color: #dff0d8;
            border-color: #d6e9c6;
            font-size: 14px;
            font-weight: 800;
            text-align: center;
        }


        .panel-group .panel-heading {
            border-bottom: 0;
        }

        .panel-heading {
            padding: 5px 5px;
            border-bottom: 1px solid transparent;
            border-top-left-radius: 3px;
            border-top-right-radius: 3px;
        }

        .panel-body {
            padding: 10px;
            border: 1px solid #d6e9c6;
        }

        .lblChrg {
            padding: 10px 10px;
        }
    </style>


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
            overflow: hidden;
            transition: all .5s ease;
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
            border-radius: 8px 8px 0px 0px;
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
            background-color: #DB7093;
            border-radius: 8px 8px 0px 0px;
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

        .buttonClose {
            background-color: lightseagreen;
            border-radius: 8px 8px 0px 0px;
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

            .buttonClose:hover {
                background-color: #C71585;
                color: white;
            }

            .buttonClose:active {
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


        .payMode {
            width: 150px;
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
            padding-left: 20px;
        }

        .ddlborder {
            border-radius: 8px;
            border-color: skyblue;
        }

        .BarCodeTextStart {
            width: 500px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
            background-color: #FFF8C6;
        }

        .BarCodeTextStartPin {
            width: 250px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
            /* background-color: #FFF8C6;*/
            margin-left: 10px
        }

        .BarCodeTextEnd {
            width: 500px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
            background-color: #FFF8C6;
        }

        .Msgg {
            border: solid 1px #ffdead00;
            width: 350px;
            border-radius: 5px;
            padding: 10px 10px 10px 10px;
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
        function StartBox() {
            var txtStart = document.getElementById('<%=txtStartDetails.ClientID %>');
            txtStart.focus();
        }
    </script>


    <div class="form-body col-sm-12 col-xs-12">
        <div>
            <div class="row input-group-prepend">
                <div class="d-flex justify-content-start" style="padding-left: 15px; margin-bottom: -5px;" runat="server" visible="false" id="divTripStart">
                    <asp:Button runat="server" ID="btnStart" OnClick="btnStart_Click" Text="Trip Start" CssClass="buttonNor block" />
                </div>
                <div class="d-flex justify-content-center" style="padding-left: 15px; margin-bottom: -5px;" runat="server" visible="false" id="divTripEnd">
                    <asp:Button runat="server" ID="btnEnd" OnClick="btnEnd_Click" Text="Trip End" CssClass="buttonPre block" />
                </div>
                <div class="d-flex justify-content-center" style="padding-left: 15px; margin-bottom: -5px;" runat="server" id="divTripClosed">
                    <asp:Button runat="server" ID="btnClosed" OnClick="btnClosed_Click" Text="Trip Closed" CssClass="buttonClose block" />
                </div>

                <h5 class="pghr" style="padding-top: 1.5rem; padding-left: 3rem; text-align: center; display: inline; font-size: 25px;">Trip Sheet </h5>
                <h3 id="Date" style="display: inline; overflow: auto; float: right; padding-left: 2rem; padding-top: 1.5rem; color: black;"></h3>
                <h3 id="Time" style="display: inline; float: left; color: black; padding-left: 0.5rem; padding-top: 1.5rem;"></h3>


            </div>

        </div>
        <div>
            <div class="table-div" id="divGridStart" runat="server" style="background-color: white; overflow: auto; max-height: 650px; min-height: 550px;">
                <div class="table-responsive" style="overflow-x: hidden">
                    <div class="row">
                        <div class="col-4">
                            <asp:TextBox ID="txtStartDetails" runat="server" CssClass="BarCodeTextStart" placeholder="Scan QRCode" Font-Bold="true" AutoComplete="off" Font-Size="X-Large" AutoPostBack="true" OnTextChanged="txtStartDetails_TextChanged"></asp:TextBox>

                            <a href="IndividualTripSheetWeb.aspx" alt="Individual Trip Sheet" class="tooltipDemo" style="margin-top: 25px; margin-left: 5px">
                                <asp:CheckBox runat="server" Style="zoom: 1.5;" Width="17px" Height="21px" ID="ChkIndTrpSht" onchange="showHourGlass();"
                                    OnCheckedChanged="ChkIndTrpSht_CheckedChanged" AutoPostBack="true" /><b style="font-size: x-large; color: black;">Individual</b>
                            </a>
                        </div>
                        <div class="col-5">
                            <asp:Label ID="lblStartResponse" CssClass="blink" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="X-Large"></asp:Label>
                        </div>
                        <div class="col-3">
                            <asp:TextBox ID="txtStartBookingPin" runat="server" CssClass="BarCodeTextStartPin" placeholder=" Enter Id/Pin" Font-Bold="true" AutoComplete="off" Font-Size="X-Large" AutoPostBack="true"
                                OnTextChanged="txtStartBookingPin_TextChanged"></asp:TextBox>
                        </div>
                    </div>

                    <div class="table-div" id="divMsgStart" runat="server" style="background-color: white;">
                        <div class="table-responsive">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="lblGridMsgStart" runat="server" ForeColor="Green" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="table-div" id="divRowerInfoMsg" runat="server" visible="false">
                        <div class="table-responsive">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="lblRowerInfoMsg" runat="server" ForeColor="Red" CssClass="blink" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <asp:GridView ID="gvTripSheetSettelementStart" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" PageSize="100" DataKeyNames="BoatReferenceNo" OnRowDataBound="gvTripSheetSettelement_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <%-- <%#Container.DataItemIndex+1 %>--%>
                                    <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBDtlPremiumStatus" runat="server" Text='<%# Eval("BDtlPremiumStatus").ToString() == "P" ? "Express" : "Normal" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Serial" HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingSerial" runat="server" Text='<%# Bind("BookingSerial") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boarding Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoardingTime" runat="server" Text='<%# Bind("BoardingTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Duration (Mins)" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoatId" runat="server" Text='<%# Bind("BoatId") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblBoatNum" runat="server" Text='<%# Bind("BoatNum") %>' Visible="false"></asp:Label>
                                    <asp:DropDownList ID="gvddlBoatNumber" runat="server" Font-Bold="true" Font-Size="X-Large" CssClass="ddlborder"
                                        Height="40px" onchange="StartBox()">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Rower Charge" HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowerCharge" runat="server" Text='<%# Bind("RowerCharge") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Rower">
                                <ItemTemplate>
                                    <asp:Label ID="lblSelfDrive" runat="server" Text='<%# Bind("SelfDrive") %>' Visible="false"></asp:Label>
                                    <asp:DropDownList ID="gvddlRowerId" runat="server" Font-Bold="true" Font-Size="X-Large" CssClass="ddlborder"
                                        Height="40px" onchange="StartBox()">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("ExpectedTime") %>' Visible="false"></asp:Label>
                                    <asp:DropDownList ID="gvddlTripStartTime" runat="server" Font-Bold="true" Font-Size="X-Large" CssClass="ddlborder" Height="40px"></asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>--%>

                            <%--   <asp:TemplateField HeaderText="QR Code">
                                <ItemTemplate>
                                    <asp:Image ID="imgOtherQRRc" runat="server" Width="75px" Height="75px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Start" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="75" CssClass="imgOutLine"
                                        runat="server" Font-Bold="true" ImageUrl="~/images/Start-Icon.png" OnClick="ImgBtnStart_Click"
                                        EnableViewState="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="5px" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gvHead" />
                        <AlternatingRowStyle CssClass="gvRow" />
                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                        <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                    </asp:GridView>
                    <div>
                        <asp:Button ID="back" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                        &nbsp
                        <asp:Button ID="Next" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="Next_Click" />
                        &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div>

            <div class="table-div" id="divGridEnd" runat="server" style="background-color: #DB7093; overflow: auto; max-height: 650px; min-height: 550px;" visible="false">
                <div class="table-responsive" style="overflow-x: hidden">
                    <div class="row">
                        <div class="col-4">
                            <asp:TextBox ID="txtEndDetails" runat="server" placeholder="Scan QRcode" CssClass="BarCodeTextEnd" AutoComplete="off" Font-Bold="true" Font-Size="X-Large" AutoPostBack="true" OnTextChanged="txtEndDetails_TextChanged"></asp:TextBox>
                        </div>
                        <div class="col-5">
                            &nbsp&nbsp&nbsp<asp:Label ID="lblEndResponse" runat="server" CssClass="blink" Font-Bold="true" ForeColor="White" Font-Size="X-Large"></asp:Label>
                        </div>
                        <div class="col-3">
                            <asp:TextBox ID="txtEndBookingPin" runat="server" placeholder=" Enter Id/Pin" CssClass="BarCodeTextStartPin" AutoComplete="off"
                                Font-Bold="true" Font-Size="X-Large" AutoPostBack="true" OnTextChanged="txtEndBookingPin_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <div class="table-div" id="divmsgEnd" runat="server" style="background-color: #DB7093;">
                        <div class="table-responsive">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="lblGridMsgEnd" runat="server" ForeColor="Green" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div style="margin-top: 20px">
                        <asp:GridView ID="gvTripSheetSettelementEnd" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                            AutoGenerateColumns="False" PageSize="100" DataKeyNames="BoatReferenceNo">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%-- <%#Container.DataItemIndex+1 %--%>
                                        <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBDtlPremiumStatus" runat="server" Text='<%# Eval("BDtlPremiumStatus").ToString() == "P" ? "Express" : "Normal" %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Serial" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingSerial" runat="server" Text='<%# Bind("BookingSerial") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boarding Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoardingTime" runat="server" Text='<%# Bind("BoardingTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Duration (Mins)" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatId" runat="server" Text='<%# Bind("BoatId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblBoatNum" runat="server" Text='<%# Bind("BoatNum") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rower">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowerId" runat="server" Text='<%# Bind("RowerId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="15%" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="10%" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <%--<asp:TemplateField HeaderText="Trip End Time" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:DropDownList ID="gvddlTripEndTime" runat="server" Font-Bold="true" Font-Size="X-Large" CssClass="ddlborder" Height="40px"></asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                            </asp:TemplateField>--%>

                                <%--  <asp:TemplateField HeaderText="QR Code">
                                <ItemTemplate>
                                    <asp:Image ID="imgOtherQRRc" runat="server" Width="75px" Height="75px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="End" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="75" CssClass="imgOutLine"
                                            runat="server" Font-Bold="true" ImageUrl="~/images/Stop-Icon.png" OnClick="ImgBtnEnd_Click"
                                            EnableViewState="false" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5px" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gvHead" />
                            <AlternatingRowStyle CssClass="gvRow" />
                            <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                            <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:Button ID="EndBack" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="EndBack_Click" />
                        &nbsp
                        <asp:Button ID="EndNext" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="EndNext_Click" />
                        &nbsp
                         <asp:Button ID="EndBackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="EndBackToList_Click" />
                    </div>

                </div>
            </div>
        </div>
        <div>

            <div class="table-div" id="divGridClosed" runat="server" style="background-color: lightseagreen; overflow: auto; max-height: 650px; min-height: 550px;" visible="false">
                <div class="table-responsive" style="overflow-x: hidden">

                    <div style="float: right; margin-bottom: 20px">
                        <div class="col-3">
                            <asp:TextBox ID="txtClosedPin" runat="server" placeholder=" Enter Id/Pin" CssClass="BarCodeTextStartPin" AutoComplete="off"
                                Font-Bold="true" Font-Size="X-Large" AutoPostBack="true" OnTextChanged="txtClosedPin_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <div class="table-div" id="divmsgClosed" runat="server" style="background-color: lightseagreen;">
                        <div class="table-responsive">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="lblGridMsgClosed" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div style="margin-top: 20px">
                        <asp:GridView ID="gvTripSheetSettelementClosed" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                            AutoGenerateColumns="False" PageSize="100" DataKeyNames="BoatReferenceNo">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%--  <%#Container.DataItemIndex+1 %>--%>
                                        <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBDtlPremiumStatus" runat="server" Text='<%# Eval("BDtlPremiumStatus").ToString() == "P" ? "Express" : "Normal" %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatId" runat="server" Text='<%# Bind("BoatId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblBoatNum" runat="server" Text='<%# Bind("BoatNum") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rower">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowerId" runat="server" Text='<%# Bind("RowerId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="10%" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="15%" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Trip End Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Bind("TripEndTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Travelled Minutes" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTravelledMinutes" runat="server" Text='<%# Bind("TraveledTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gvHead" />
                            <AlternatingRowStyle CssClass="gvRow" />
                            <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                            <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:Button ID="CloseBack" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="CloseBack_Click" />
                        &nbsp
                        <asp:Button ID="CloseNext" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="CloseNext_Click" />
                        &nbsp
                         <asp:Button ID="CloseBacktoList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="CloseBacktoList_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <ajax:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="pnlTrip" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
    <ajax:ModalPopupExtender ID="MpeTrip" runat="server" BehaviorID="MpeTrip" TargetControlID="HiddenField1" PopupControlID="pnlTrip"
        BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>

    <asp:Panel ID="pnlTrip" runat="server" CssClass="Msgg">
        <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
            <div class="modal-content" style="width: 500px">
                <div class="modal-header">
                    <%-- <h5 class="modal-title" id="exampleModalLabel">Rower Trip Details For Settlement Id : 
                       <asp:Label ID="lblRowerSettlementId" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </h5>--%>
                    <%-- <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />--%>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblAlertMsg" runat="server" Font-Bold="true"
                        Text="Confirm(Are you sure to End this Trip?)" Font-Size="X-Large"></asp:Label>

                    <label for="lblTripBooking" style="font-size: large; color: black">
                        Booking Id <span style="padding-left: 4.8rem;">:</span>
                    </label>
                    <asp:Label ID="lblPopBookingId" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Red"></asp:Label>
                    <br />
                    <label for="lblTripBookingpin" style="font-size: large; color: black">
                        Booking Pin <span style="padding-left: 4.2rem;">:</span>
                    </label>
                    <asp:Label ID="lblPopBookingPin" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Red"></asp:Label>
                    <br />
                    <label for="lblTripStart" style="font-size: large; color: black">
                        Trip Start Duration <span style="padding-left: 1rem;">:</span>
                    </label>
                    <asp:Label ID="lblStartTime" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Red"></asp:Label>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnPopUpOkay" runat="server" Text="Okay" CssClass="btn btn-primary" OnClick="btnPopUpOkay_Click" />
                    <asp:Button ID="btnPopUpCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnPopUpCancel_Click" />
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>

    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfBoathouseId" runat="server" />
    <asp:HiddenField ID="hfBoathouseName" runat="server" />
    <asp:HiddenField ID="hfBookingTo" runat="server" />
    <asp:HiddenField ID="hfBookingFrom" runat="server" />
    <asp:HiddenField ID="hfBarcodePin" runat="server" />
    <asp:HiddenField ID="hfBookingId" runat="server" />
    <asp:HiddenField ID="hfRefNum" runat="server" />
    <asp:HiddenField ID="hfSelfDrive" runat="server" />
    <asp:HiddenField ID="hfRowerChrg" runat="server" />
    <asp:HiddenField ID="hfBoatName" runat="server" />
    <asp:HiddenField ID="hfRowerId" runat="server" />
    <asp:HiddenField ID="hfBarCodePinEnd" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hfstartvalue" runat="server" />
    <asp:HiddenField ID="hfendvalue" runat="server" />
    <div id="HourGlass" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8; display: none">
        <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
            <img src="../images/hourglass.gif" width="100px" height="100px" />
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

