<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="NewDashboard.aspx.cs" Inherits="Boating_NewDashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/data.js"></script>
    <script src="https://code.highcharts.com/modules/drilldown.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>

    <style>
        .dtlBoatTypeD {
            padding: 0px;
            margin-bottom: 5px;
            border-radius: 10px;
            box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
            overflow: hidden;
            transition: all .5s ease;
        }

        .dtlBoatType1 {
            padding: 5px;
            margin-bottom: 5px;
            border-radius: 10px;
            box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
            overflow: hidden;
            transition: all .5s ease;
        }

        .highcharts-figure, .highcharts-data-table table {
            min-width: 310px;
            max-width: 800px;
            margin: 1em auto;
        }

        #container {
            height: 400px;
        }

        .panel-body {
            padding: 10px;
            border: 0px solid cadetblue;
            position: center;
        }

        .modal-dialog {
            max-width: 800px;
            max-height: 1000px;
            margin: auto;
        }

        .pnl1 {
            background-color: #add8e6;
            color: #203e9e;
            font-size: 14px;
            font-weight: 700;
        }

        .pnl2 {
            background-color: #6092cd;
            color: #203e9e;
            font-size: 14px;
            font-weight: 700;
        }

        .pnl3 {
            background-color: #D5F3C7;
            color: #203e9e;
            font-size: 14px;
            font-weight: 700;
        }

        .pnl4 {
            background-color: #ffc080;
            color: #203e9e;
            font-size: 14px;
            font-weight: 700;
        }

        .pnl5 {
            background-color: rgb(228, 211, 84);
            color: #203e9e;
            font-size: 14px;
            font-weight: 700;
        }

        .pnl6 {
            background-color: rgb(255,192,203);
            color: #203e9e;
            font-size: 14px;
            font-weight: 700;
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

        .otherserv-list-input {
            display: block;
            padding-left: 10px;
        }

        .abc {
            background-color: #c9d2ff;
            color: #203e9e;
            text-align: center;
            height: 25px;
        }

        .hBtn {
            height: 30px;
            border-radius: 5px;
            width: 70px;
        }
    </style>

    <script>
        $(document).ready(function () {
            $(".DashfrmDate").datepicker({
                dateFormat: 'dd/mm/yy',
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,
                onClose: function () {
                    var date2 = $('.DashfrmDate').datepicker('getDate');
                    date2.setDate(date2.getDate() + 6)
                    $(".DashtoDate").datepicker("setDate", date2);
                }
            });

            $(".DashtoDate").datepicker({
                dateFormat: 'dd/mm/yy',
                maxDate: 0,
                changeMonth: true,
                changeYear: true,
                onClose: function () {
                    var date2 = $('.DashtoDate').datepicker('getDate');
                    date2.setDate(date2.getDate() - 6)
                    $(".DashfrmDate").datepicker("setDate", date2);
                }
            });
        });
    </script>
    <style>
        @media (min-width: 576px) {
            .col-sm-2 {
                -ms-flex: 0 0 16.666667%;
                flex: 0 0 16.666667%;
                max-width: 14.666667%;
            }
        }
    </style>

    <div class="form-body" />
    <div id="divReqAmnt" runat="server">

        <ul class="nav nav-pills" role="tablist" id="ulTabList" runat="server" visible="true">
            <li class="nav-item">
                <asp:LinkButton ID="lbtnBookingSummary" runat="server" class="nav-link pnl1" OnClick="lbtnBookingSummary_Click"
                    CausesValidation="false" Font-Bold="true" ForeColor="Black">Booking Summary</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lbtnRevenueComparison" runat="server" OnClick="lbtnRevenueComparison_Click"
                    CausesValidation="false" Font-Bold="true" ForeColor="Black" class="nav-link pnl4">Revenue Comparison</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lbtnPriceComparison" runat="server" OnClick="lbtnPriceComparison_Click"
                    CausesValidation="false" Font-Bold="true" ForeColor="Black" class="nav-link pnl3">Rate Comparison</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lbtnAbstractBoatCount" runat="server" OnClick="lbtnAbstractBoatCount_Click"
                    CausesValidation="false" Font-Bold="true" ForeColor="Black" class="nav-link pnl2">Boat Count</asp:LinkButton>
            </li>
            <li class="nav-item" runat="server" visible="true">
                <asp:LinkButton ID="lbtnBoatRunningStatus" runat="server" OnClick="lbtnBoatRunningStatus_Click"
                    CausesValidation="false" Font-Bold="true" ForeColor="Black" class="nav-link pnl5">Trip Status</asp:LinkButton>
            </li>
            <li class="nav-item" runat="server" visible="true">
                <asp:LinkButton ID="lbtnBoatUtilization" runat="server" OnClick="lbtnBoatUtilization_Click"
                    CausesValidation="false" Font-Bold="true" ForeColor="Black" class="nav-link pnl6">Boat Utilization</asp:LinkButton>
            </li>
        </ul>

        <%-- BOOKING SUMMARY--%>

        <div id="divBookingSummary" runat="server" visible="false" style="margin-top: -3px; border: 2px solid #add8e6">
            <div id="BookingSummaryHead" runat="server" class="panel-body nav-item" style="background-color: #add8e6; text-align: center"></div>
            <hr />
            <div class="row" id="divnew" runat="server">
                <div class="col-sm-12 col-xs-12">
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-sm-2 col-xs-12" id="divrbtnType" runat="server">
                                <div class="form-group">
                                    <i class="fas fa-bars"></i>
                                    <asp:Label ID="Label9" runat="server" Text="Summary Type" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="rbtnType" CssClass="form-control inputboxstyle" runat="server" TabIndex="2" OnSelectedIndexChanged="rbtnType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="1">Revenue</asp:ListItem>
                                        <asp:ListItem Value="0">Ticket Count</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-group">
                                    <i class="fa fa-ship" aria-hidden="true"></i>
                                    <asp:Label ID="Label4" runat="server" Text="Boat House" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="ddlBoatHouse" CssClass="form-control inputboxstyle" runat="server" TabIndex="1"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlBoatHouse_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12" id="divServicename" runat="server">
                                <div class="form-group">
                                    <i class="fas fa-bars"></i>
                                    <asp:Label ID="Label5" runat="server" Text="Service Name" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="ddlServiceName" CssClass="form-control inputboxstyle" runat="server" TabIndex="2"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlServiceName_SelectedIndexChanged">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem Value="1">Boat Booking</asp:ListItem>
                                        <asp:ListItem Value="2">Other Services</asp:ListItem>
                                        <asp:ListItem Value="3">Restaurant</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12" id="divBoatType" runat="server">
                                <div class="form-group">
                                    <i class="fa fa-ship" aria-hidden="true"></i>
                                    <asp:Label ID="Label6" runat="server" Text="Boat Type" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="ddlBoatType" CssClass="form-control" runat="server"
                                        AutoPostBack="true" TabIndex="3" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlBoatType"
                                        ValidationGroup="" SetFocusOnError="True" InitialValue="Select Boat Type" CssClass="vError">Select Boat Type</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12" id="divBoatSeat" runat="server">
                                <div class="form-group">
                                    <i class="fas fa-chair" aria-hidden="true"></i>
                                    <asp:Label ID="Label7" runat="server" Text="Boat Seater" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="ddlBoatSeater" CssClass="form-control" runat="server" TabIndex="4" OnSelectedIndexChanged="ddlBoatSeater_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBoatSeater"
                                        ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="Select Boat Seater" CssClass="vError">Select Boat Seater</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-group">
                                    <i class="fa fa-calendar" aria-hidden="true"></i>
                                    <asp:Label ID="Label8" runat="server" Text="From Date" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate1" AutoComplete="Off" TabIndex="5" onkeydown="return false;">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate" ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-group">
                                    <i class="fa fa-calendar" aria-hidden="true"></i>
                                    <asp:Label ID="Label2" runat="server" Text="To Date" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate1" AutoComplete="Off" TabIndex="6" onkeydown="return false;">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                                        ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter To Date</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12" style="margin-top: 22px">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn-primary hBtn" ValidationGroup="Search"
                                    TabIndex="7" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn-danger hBtn"
                                    TabIndex="8" OnClick="btnReset_Click" />

                            </div>
                            <div style="margin-top: 22px">
                                <asp:ImageButton ID="imgDownloadExcel" runat="server" ImageUrl="~/images/Excel.png" Width="30px"
                                    OnClick="imgDownloadExcel_Click" Visible="false" ToolTip="Excel Download" />
                            </div>

                        </div>
                    </div>
                    <div class="col-xs-12 dtlBoatType1 text-center" style="border: 1px dashed #add8e6; margin-top: -15px; margin-bottom: 10px; background-color: #FFFF9E">
                        <div class="row">
                            <div class="col-sm-2 col-xs-12">
                                <b style="font-size: 15px">Overall Revenue (INR)</b>
                                <div class="row" style="margin-left: 30%">
                                    <asp:LinkButton ID="lblOverallRevenue" runat="server" class="pghr" Style="text-align: center; font-size: 15px; display: inline;" OnClick="lblOverallRevenue_Click" Font-Underline="true"></asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <b style="font-size: 15px">Boat Booking (INR) </b>
                                <div class="row" style="margin-left: 20%">
                                    <asp:LinkButton ID="lblBoatingOverallRevenue" runat="server" class="pghr" Style="text-align: center; font-size: 15px; display: inline;" OnClick="lblBoatingOverallRevenue_Click" Font-Underline="true"></asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <b style="font-size: 15px">Other Service (INR) </b>
                                <div class="row" style="margin-left: 20%">
                                    <asp:LinkButton ID="lblOSOverallRevenue" runat="server" class="pghr" Style="text-align: center; font-size: 15px; display: inline;" OnClick="lblOSOverallRevenue_Click" Font-Underline="true"></asp:LinkButton>
                                </div>
                            </div>

                            <div class="col-sm-2 col-xs-12" style="float: left">
                                <b style="font-size: 15px">Restaurant (INR) </b>
                                <div class="row" style="margin-left: 25%">
                                    <asp:LinkButton ID="lblRESOverallRevenue" runat="server" class="pghr" Style="text-align: center; font-size: 15px; display: inline;" OnClick="lblRESOverallRevenue_Click" Font-Underline="true"></asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12" style="float: left">
                                <b style="font-size: 15px">Other Revenue (INR) </b>
                                <div class="row" style="margin-left: 40%">
                                    <asp:LinkButton ID="lblOROverallRevenue" runat="server" class="pghr" Style="text-align: center; font-size: 15px; display: inline;" Font-Underline="true" OnClick="lblOROverallRevenue_Click"></asp:LinkButton>
                                </div>
                            </div>
                            <%-- Newly Added Rowercharge--%>
                            <div class="col-sm-2 col-xs-12">
                                <b style="font-size: 15px; color: red">Rower Charges (INR) </b>
                                <div class="row" style="margin-left: 20%">
                                    <asp:LinkButton ID="lblRowerOverallRevenue" runat="server" class="pghr" Style="text-align: center; font-size: 15px; color: red; display: inline;" OnClick="lblRowerOverallRevenue_Click" Font-Underline="true"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 col-sm-12" id="divusergraph" runat="server">
                        <div class="row">
                            <div class="dtlBoatTypeD col-sm-4" runat="server" id="divBoatBookingGraph" style="display: none; background-color: #add8e6;">
                                <div class="form-group">
                                    <figure class="highcharts-figure">
                                        <div id="BoatGraph" style="height: 350px;"></div>
                                        <p class="highcharts-description">
                                        </p>
                                    </figure>
                                </div>
                            </div>
                            <div class="dtlBoatTypeD col-sm-4" runat="server" id="divOtherServiceGraph" style="display: none; background-color: #add8e6">
                                <div class="form-group">
                                    <figure class="highcharts-figure">
                                        <div id="OtherServiceGraph" style="height: 350px;"></div>
                                        <p class="highcharts-description">
                                        </p>
                                    </figure>
                                </div>
                            </div>
                            <div class="dtlBoatTypeD col-sm-4" runat="server" id="divRestaurantGraph" style="display: none; background-color: #add8e6">
                                <figure class="highcharts-figure">
                                    <div id="RestaurantGraph" style="height: 350px;"></div>
                                    <p class="highcharts-description">
                                    </p>
                                </figure>
                            </div>
                        </div>
                    </div>
                    <div class="row m-0" style="margin-top: 6px">
                        <div class="col-sm-3 col-xs-12"></div>
                        <div class="col-sm-3 col-xs-12"></div>
                    </div>
                </div>
            </div>
        </div>

        <%-- ABSTRACT BOAT COUNT--%>

        <div id="divAbstractBoatCount" runat="server" visible="false" style="margin-top: -3px; border: 2px solid #6092cd">
            <div class="panel-body nav-item" style="background-color: #6092cd; text-align: center"></div>
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <i class="fa fa-ship" aria-hidden="true"></i>
                        <asp:Label ID="Label22" runat="server" Text="Boat House" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="abDdlBoatHouse" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="abDdlBoatHouse_SelectedIndexChanged"
                            TabIndex="1">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <i class="fa fa-ship" aria-hidden="true"></i>
                        <asp:Label ID="Label23" runat="server" Text="Boat Type" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="abDdlBoatType" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="abDdlBoatType_SelectedIndexChanged"
                            TabIndex="2">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <i class="fas fa-chair" aria-hidden="true"></i>
                        <asp:Label ID="Label24" runat="server" Text="Boat Seater" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="abDdlBoatSeater" CssClass="form-control inputboxstyle" runat="server"
                            TabIndex="3">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <i class="fas fa-ship" aria-hidden="true"></i>
                        <asp:Label ID="Label25" runat="server" Text="Boat Status" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="ddlBoatStatus" CssClass="form-control inputboxstyle" runat="server"
                            TabIndex="4">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 22px">
                        <asp:Button ID="btnABSubmit" runat="server" Text="Search" class="hBtn btn-primary" ValidationGroup="Search" TabIndex="7" OnClick="btnABSubmit_Click" />
                        <asp:Button ID="btnABReset" runat="server" Text="Reset" CausesValidation="false" class="hBtn btn-danger" TabIndex="8" OnClick="btnABReset_Click" />
                    </div>
                    <div class="col-sm-3 col-xs-12" runat="server" id="divNote1">
                        <b style="font-size: X-Small; font-style: italic">Note : </b>
                        <asp:Label ID="Label29" runat="server" ForeColor="Black" Font-Size="X-Small" Font-Italic="true" Font-Bold="true">
                             “Kodaikanal – 2” Boat Count is same as “Kodaikanal”
                        </asp:Label>
                    </div>
                </div>
                <div runat="server" id="divAbBoatBookingGraph">
                    <div class="row p-2">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <figure class="highcharts-figure" style="border: 2px solid #6092cd">
                                    <div id="ABBoatGraph"></div>
                                    <p class="highcharts-description">
                                    </p>
                                </figure>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:HiddenField ID="hfData" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfLabel" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfBoatHouse" runat="server" ClientIDMode="Static" />

                <script>
                    var data = $("#hfData").val();
                    var label = $("#hfLabel").val();
                    var BoatHouseName = $("#hfBoatHouse").val();

                    data = data.split(',');
                    label = label.split(',');
                    console.log(data);
                    console.log(label);
                    console.log(BoatHouseName);

                    var BHName = document.getElementById("<%=abDdlBoatHouse.ClientID%>");
                    if (BHName.options[BHName.selectedIndex].text == "All") {
                        var BoatHouseName = "All Boat House";
                    }
                    else {
                        var BoatHouseName = BHName.options[BHName.selectedIndex].text;
                    }

                    var BTName = document.getElementById("<%=abDdlBoatType.ClientID%>");
                    if (BTName.options[BTName.selectedIndex].text == "All") {
                        var BoatTypeName = "All Boat Type";
                    }
                    else {
                        var BoatTypeName = BTName.options[BTName.selectedIndex].text;
                    }

                    var sData = new Array();
                    var k = 0;
                    for (var k = 0; k < label.length; k++) {
                        var entry = {
                            name: label[k],
                            y: parseFloat(data[k]),
                            drilldown: true
                        }
                        sData.push(entry);
                    }

                    var sssname = "";
                    sssname = "Click the Boat Type to View BoatHouse Wise details";

                    var xAxisName = "";
                    xAxisName = "Boat Type";

                    Highcharts.chart('ABBoatGraph', {
                        chart: {
                            type: 'column',
                            events: {
                                drilldown: function (e) {
                                    if (!e.seriesOptions) {
                                        var chart = this;
                                        var dataArr = CallChildABBoatBooking(e.point.name);
                                        sssname = "";
                                        this.xAxis[0].setTitle({ text: "Boat House" });
                                        data = {
                                            name: e.point.name,
                                            data: dataArr,
                                        },
                                            setTimeout(function () {
                                                chart.hideLoading();
                                                chart.addSeriesAsDrilldown(e.point, data);
                                            }, 1000);
                                    }
                                },
                                drillup: function (e) {
                                    this.xAxis[0].setTitle({ text: xAxisName });
                                }
                            }
                        },
                        title: {
                            text: 'Boat Count - ' + BoatTypeName,
                            style: { fontWeight: 'bold' }
                        },
                        subtitle: {
                            text: sssname
                        },
                        accessibility: {
                            announceNewData: { enabled: true }
                        },
                        xAxis: {
                            type: 'category',
                            title: {
                                enabled: true,
                                text: xAxisName,
                                style: { fontWeight: 'bold' }
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Boat Count (Numbers)',
                                style: { fontWeight: 'bold' }
                            }
                        },
                        legend: { enabled: false },
                        plotOptions: {
                            series: {
                                borderWidth: 0,
                                dataLabels: { enabled: true, format: '{point.y:,.0f}' }
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                        },
                        series: [{ name: "Summary", colorByPoint: true, data: sData }],
                        drilldown: {
                            drillUpButton: {
                                theme: {
                                    fill: 'white',
                                    'stroke-width': 1,
                                    stroke: 'silver',
                                    r: 0,
                                    states: {
                                        hover: {
                                            fill: 'silver'
                                        },
                                        select: {
                                            stroke: '#039',
                                            fill: 'silver'
                                        }
                                    },
                                    style: { fontWeight: 'bold' }
                                }
                            },
                            series: []
                        }
                    });

                    function CallChildABBoatBooking(name) {
                        var sBoatTypeId = name;
                        var value = document.getElementById("<%=abDdlBoatHouse.ClientID%>");
                        var name = value.options[value.selectedIndex].text;
                        var sBoatHouseId = value.options[value.selectedIndex].value;

                        var BTValue = document.getElementById("<%=abDdlBoatType.ClientID%>");
                        var BTname = BTValue.options[BTValue.selectedIndex].text;
                        var sBTId = BTValue.options[BTValue.selectedIndex].value;

                        var value = document.getElementById("<%=abDdlBoatSeater.ClientID%>");
                        var name = value.options[value.selectedIndex].text;
                        var sBoatSeaterId = value.options[value.selectedIndex].value;

                        var value = document.getElementById("<%=ddlBoatStatus.ClientID%>");
                        var name = value.options[value.selectedIndex].text;
                        var sBoatStatusId = value.options[value.selectedIndex].value;

                        if (sBTId != '0') {
                            var data =
                            {
                                "BoatHouseId": sBoatHouseId, "BoatTypeId": BTname, "BoatSeaterId": sBoatSeaterId,
                                "BoatStatusId": sBoatStatusId
                            };
                        }
                        else {
                            var data =
                            {
                                "BoatHouseId": sBoatHouseId, "BoatTypeId": sBoatTypeId, "BoatSeaterId": sBoatSeaterId,
                                "BoatStatusId": sBoatStatusId
                            };
                        }

                        var Drilldowndata = new Array();
                        var dataUrl = document.getElementById("<%=hfUrl.ClientID%>").value + "GetDashboardBoatCountDrillDown";
                        console.log("dataformat", data);

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: dataUrl,
                            data: JSON.stringify(data),
                            dataType: "json",
                            crossDomain: true,
                            success: function (response) {
                                if (response.TableShow.length > 0) {
                                    console.log("Final", response.TableShow);
                                    for (var i = 0; i < response.TableShow.length; i++) {
                                        var sServiceName = response.TableShow[i].BoatType;
                                        console.log("dsd", sServiceName);
                                        var sServiceCount = response.TableShow[i].BoatCount;
                                        console.log("dsd", sServiceCount);
                                        var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                        Drilldowndata.push(serie);
                                        console.log("Final", serie);
                                    }
                                }
                            },
                            error: function (Result) {
                                console.log("Error");
                            }
                        })
                        return Drilldowndata;
                    }
                </script>
            </div>
        </div>

        <%--PRICE COMPARISON--%>

        <div id="divPriceComparison" runat="server" visible="false" style="margin-top: -3px; border: 2px solid #D5F3C7">
            <div class="panel-body nav-item" style="background-color: #D5F3C7; text-align: center"></div>
            <div class="row">
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <i class="fas fa-bars"></i>
                        <asp:Label ID="Label14" runat="server" Text="Service Name" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="PCddlServiceName" CssClass="form-control inputboxstyle" runat="server" TabIndex="1" OnSelectedIndexChanged="PCddlServiceName_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0"> Select Service</asp:ListItem>
                            <asp:ListItem Value="1">Boating</asp:ListItem>
                            <asp:ListItem Value="2">Other Services</asp:ListItem>
                            <asp:ListItem Value="3">Restaurant</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="PCddlServiceName"
                            ValidationGroup="PCSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Service</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="PCdivBoatType" visible="false">
                    <i class="fa fa-ship" aria-hidden="true"></i>
                    <asp:Label ID="Label15" runat="server" Text="Boat Type" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="PCddlBoatType" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PCddlBoatType_SelectedIndexChanged"
                        TabIndex="2">
                        <asp:ListItem Value="0">Select Boat Type</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="PCddlBoatType"
                        ValidationGroup="PCSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat Type</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="PCdivBoatSeater" visible="false">
                    <i class="fas fa-chair" aria-hidden="true"></i>
                    <asp:Label ID="Label16" runat="server" Text="Boat Seater" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="PCddlBoatSeater" CssClass="form-control inputboxstyle" runat="server" TabIndex="3">
                        <asp:ListItem Value="0">Select Boat Seater</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="PCddlBoatSeater"
                        ValidationGroup="PCSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat Seater</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="divOtherServiceCategory" visible="false">
                    <i class="fa fa-address-book" aria-hidden="true"></i>
                    <asp:Label ID="Label17" runat="server" Text="Category" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddlCategoryOtherService" CssClass="form-control inputboxstyle" runat="server"
                        TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="ddlCategoryOtherService_SelectedIndexChanged">
                        <asp:ListItem Value="0">Select Category</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlCategoryOtherService"
                        ValidationGroup="PCSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Category</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="divOtherServiceServices" visible="false">
                    <i class="fa fa-address-book" aria-hidden="true"></i>
                    <asp:Label ID="Label18" runat="server" Text="Service" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddlServiceOtherService" CssClass="form-control inputboxstyle" runat="server"
                        TabIndex="3">
                        <asp:ListItem Value="0">Select Service</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlServiceOtherService"
                        ValidationGroup="PCSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Service</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="divRestaurantCategory" visible="false">
                    <i class="fa fa-ship" aria-hidden="true"></i>
                    <asp:Label ID="Label19" runat="server" Text="Category" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddlCategoryRestaurant" CssClass="form-control inputboxstyle" runat="server" OnSelectedIndexChanged="ddlCategoryRestaurant_SelectedIndexChanged" AutoPostBack="true"
                        TabIndex="2">
                        <asp:ListItem Value="0">Select Category</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlCategoryRestaurant"
                        ValidationGroup="PCSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Category</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="divRestaurantService" visible="false">
                    <i class="fa fa-list-alt" aria-hidden="true"></i>
                    <asp:Label ID="Label20" runat="server" Text="Item" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddlServiceRestaurant" CssClass="form-control inputboxstyle" runat="server"
                        TabIndex="3">
                        <asp:ListItem Value="0">Select Item Name</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlServiceRestaurant"
                        ValidationGroup="PCSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Service</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-3 col-xs-12" id="divType" runat="server" visible="false">
                    <i class="fas fa-ship"></i>
                    <asp:Label ID="Label21" runat="server" Text="Type" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:RadioButtonList ID="rbtnNormPre" runat="server" RepeatDirection="Horizontal" CssClass="rbl" Font-Bold="true" ForeColor="Black">
                        <asp:ListItem Value="N" Selected="true">Normal 
                        </asp:ListItem>
                        <asp:ListItem Value="P">Premium </asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 20px">
                    <asp:Button ID="btnPCSubmit" runat="server" Text="Search" class="hBtn btn-primary" ValidationGroup="PCSearch" TabIndex="7" OnClick="btnPCSubmit_Click" />
                    <asp:Button ID="btnPCReset" runat="server" Text="Reset" CausesValidation="false" class="hBtn btn-danger" TabIndex="8" OnClick="btnPCReset_Click" />
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3 col-xs-12" style="margin-top: -20px">
                    <b style="font-size: X-Small; font-style: italic;">Note : </b>
                    <asp:Label ID="Label30" runat="server" ForeColor="Black" Font-Size="X-Small" Font-Italic="true" Font-Bold="true">
                             “Kodaikanal – 2” Boat Count is same as “Kodaikanal”
                    </asp:Label>
                </div>
            </div>

            <div class="col-sm-12 col-sm-12" style="background-color: white;" runat="server" id="divGraph" visible="false">
                <div runat="server" id="divPriceComparisonGraph">
                    <div class="row p-2">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <figure class="highcharts-figure" style="border: 2px solid #D5F3C7">
                                    <div id="PCGraph"></div>
                                    <p class="highcharts-description">
                                    </p>
                                </figure>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hfPrice" runat="server" />
            <asp:HiddenField ID="pchfBoathouse" runat="server" />
            <asp:HiddenField ID="hfPremiumPrice" runat="server" />
            <asp:HiddenField ID="pchfheading1" runat="server" />
            <asp:HiddenField ID="pchfheading2" runat="server" />

            <script type="text/javascript">
                var ServiceName = $('#<%=PCddlServiceName.ClientID%>').val();
                var Input1 = $('#<%=pchfheading1.ClientID%>').val();
                var Input2 = $('#<%=pchfheading2.ClientID%>').val();
                var data = $('#<%=hfPrice.ClientID%>').val();
                var datasplit = data.split(',');
                var dataBoathouse = $('#<%=pchfBoathouse.ClientID%>').val();
                var dataBoathousesplit = dataBoathouse.split(',');

                var dataPremium = $('#<%=hfPremiumPrice.ClientID%>').val();
                var dataPremiumsplit = dataPremium.split(',');

                var NormalData = new Array();
                var k = 0;
                for (var k = 0; k < dataBoathousesplit.length; k++) {
                    var entry = {
                        name: dataBoathousesplit[k],
                        y: parseFloat(datasplit[k])
                    }
                    NormalData.push(entry);
                }

                if (ServiceName == "1") {
                    Highcharts.chart('PCGraph', {
                        chart: { type: 'column' },
                        title: {
                            text: Input1 + ' - ' + Input2,
                            style: { fontWeight: 'bold' }
                        },
                        accessibility: {
                            announceNewData: { enabled: true }
                        },
                        xAxis: {
                            type: 'category',
                            title: {
                                enabled: true,
                                text: 'Boat House',
                                style: { fontWeight: 'bold' }
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Rate (INR)',
                                style: { fontWeight: 'bold' }
                            }
                        },
                        legend: { enabled: false },
                        plotOptions: {
                            series: {
                                borderWidth: 0,
                                dataLabels: { enabled: true, format: '{point.y:,.0f}' }
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                        },
                        series: [{ name: "Summary", colorByPoint: true, data: NormalData }],
                    });
                }
                else {
                    Highcharts.chart('PCGraph', {
                        chart: { type: 'column' },
                        title: {
                            text: Input1 + ' - ' + Input2,
                            style: { fontWeight: 'bold' }
                        },
                        accessibility: {
                            announceNewData: { enabled: true }
                        },
                        xAxis: {
                            type: 'category',
                            title: {
                                enabled: true,
                                style: { fontWeight: 'bold' }
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Rate (INR)',
                                style: { fontWeight: 'bold' }
                            }
                        },
                        legend: { enabled: false },
                        plotOptions: {
                            series: {
                                borderWidth: 0,
                                dataLabels: { enabled: true, format: '{point.y:,.0f}' }
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                        },
                        series: [{ name: "Summary", colorByPoint: true, data: NormalData }],
                    });
                }
            </script>
        </div>

        <%-- REVENUE COMPARISON--%>

        <div id="divRevenueComparison" runat="server" visible="false" style="margin-top: -3px; border: 2px solid #ffc080">
            <div id="Div1" runat="server" class="panel-body nav-item" style="background-color: #ffc080; text-align: center"></div>
            <hr />
            <div class="row" id="div2" runat="server">
                <div class="col-sm-12 col-xs-12">
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <i class="fa fa-ship" aria-hidden="true"></i>
                                    <asp:Label ID="Label10" runat="server" Text="Boat House" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="RCddlBoatHouse" CssClass="form-control inputboxstyle" runat="server" TabIndex="1"
                                        AutoPostBack="true" OnSelectedIndexChanged="RCddlBoatHouse_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-group">
                                    <i class="fas fa-bars"></i>
                                    <asp:Label ID="Label11" runat="server" Text="Service Name" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="RCddlServiceName" CssClass="form-control inputboxstyle" runat="server" TabIndex="2"
                                        AutoPostBack="true" OnSelectedIndexChanged="RCddlServiceName_SelectedIndexChanged">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem Value="1">Boat Booking</asp:ListItem>
                                        <asp:ListItem Value="2">Other Services</asp:ListItem>
                                        <asp:ListItem Value="3">Restaurant</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12" id="RCdivBoatType" runat="server">
                                <div class="form-group">
                                    <i class="fa fa-ship" aria-hidden="true"></i>
                                    <asp:Label ID="Label12" runat="server" Text="Boat Type" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="RCddlBoatType" CssClass="form-control" runat="server"
                                        AutoPostBack="true" TabIndex="3" OnSelectedIndexChanged="RCddlBoatType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RCddlBoatType"
                                        ValidationGroup="" SetFocusOnError="True" InitialValue="Select Boat Type" CssClass="vError">Select Boat Type</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12" id="RCdivSeaterType" runat="server">
                                <div class="form-group">
                                    <i class="fas fa-chair" aria-hidden="true"></i>
                                    <asp:Label ID="Label13" runat="server" Text="Boat Seater" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="RCddlSeaterType" CssClass="form-control" runat="server" TabIndex="4" OnSelectedIndexChanged="RCddlSeaterType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="RCddlSeaterType"
                                        ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="Select Boat Seater" CssClass="vError">Select Boat Seater</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-group">
                                    <i class="fa fa-calendar" aria-hidden="true"></i>
                                    <asp:Label ID="Label1" runat="server" Text="From Date" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:TextBox ID="RCtxtFromDate" runat="server" CssClass=" form-control DashfrmDate" AutoComplete="Off" TabIndex="5" onkeydown="return false;">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="RCtxtFromDate"
                                        ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-group">
                                    <i class="fa fa-calendar" aria-hidden="true"></i>
                                    <asp:Label ID="Label3" runat="server" Text="To Date" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    <asp:TextBox ID="RCtxtToDate" runat="server" CssClass="form-control DashtoDate" AutoComplete="Off" TabIndex="6" onkeydown="return false;">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="RCtxtToDate"
                                        ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter To Date</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 22px">
                                <asp:Button ID="RCbtnSearch" runat="server" Text="Search" class="hBtn btn-primary" ValidationGroup="Search" TabIndex="7" OnClick="RCbtnSearch_Click" />
                                <asp:Button ID="RCbtnReset" runat="server" Text="Reset" CausesValidation="false" class="hBtn btn-danger" TabIndex="8" OnClick="RCbtnReset_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 col-sm-12">
                        <div class="row">
                            <div class="dtlBoatTypeD col-sm-4" runat="server" id="divRCBoatGraph" style="display: none; background-color: #ffc080">
                                <div class="form-group">
                                    <figure class="highcharts-figure">
                                        <div id="RCBoatGraph"></div>
                                        <p class="highcharts-description">
                                        </p>
                                    </figure>
                                </div>
                            </div>
                            <div class="dtlBoatTypeD col-sm-4" runat="server" id="divRCOtherServiceGraph" style="display: none; background-color: #ffc080">
                                <div class="form-group">
                                    <figure class="highcharts-figure">
                                        <div id="RCOtherServiceGraph"></div>
                                        <p class="highcharts-description">
                                        </p>
                                    </figure>
                                </div>
                            </div>
                            <div class="dtlBoatTypeD col-sm-4" runat="server" id="divRCRestaurantGraph" style="display: none; background-color: #ffc080">
                                <figure class="highcharts-figure">
                                    <div id="RCRestaurantGraph"></div>
                                    <p class="highcharts-description">
                                    </p>
                                </figure>
                            </div>
                        </div>
                    </div>
                    <div class="row m-0" style="margin-top: 6px">
                        <div class="col-sm-3 col-xs-12"></div>
                        <div class="col-sm-3 col-xs-12"></div>
                    </div>
                </div>
            </div>
        </div>

        <%--BOAT RUNNING STATUS--%>

        <div id="divBoatRunningStatus" runat="server" visible="false" style="margin-top: -3px; border: 2px solid rgb(228, 211, 84)">
            <div class="panel-body nav-item" style="background-color: rgb(228, 211, 84); text-align: center">
            </div>
            <div class="row p-2">
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <i class="fa fa-ship" aria-hidden="true"></i>
                        <asp:Label ID="Label26" runat="server" Text="Boat House" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="BRSddlBoatHouse" CssClass="form-control inputboxstyle" runat="server" TabIndex="1"
                            AutoPostBack="true" OnSelectedIndexChanged="BRSddlBoatHouse_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="BRSddlBoatHouse"
                            ValidationGroup="BRSSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="divBRSBoatType" visible="false">
                    <i class="fa fa-ship" aria-hidden="true"></i>
                    <asp:Label ID="Label27" runat="server" Text="Boat Type" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="BRSddlBoatType" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="BRSddlBoatType_SelectedIndexChanged"
                        TabIndex="2">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>

                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="divBRSBoatSeater" visible="false">
                    <i class="fas fa-chair" aria-hidden="true"></i>
                    <asp:Label ID="Label28" runat="server" Text="Boat Seater" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="BRSddlBoatSeater" CssClass="form-control inputboxstyle" runat="server" TabIndex="3">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3 col-xs-12" style="max-width: 19%">
                    <i class="fas fa-ship"></i>
                    <asp:Label ID="Label36" runat="server" Text="Boat Nature" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:RadioButtonList ID="BRSrbtnBoatNature" runat="server" RepeatDirection="Horizontal" CssClass="rbl" Font-Bold="true" ForeColor="Black">
                        <asp:ListItem Value="N" Selected="true">Normal 
                        </asp:ListItem>
                        <asp:ListItem Value="P">Premium </asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 20px">
                    <asp:Button ID="BRSbtnSearch" runat="server" Text="Search" class="hBtn btn-primary" ValidationGroup="BRSSearch" TabIndex="7" OnClick="BRSbtnSearch_Click" />
                    <asp:Button ID="BRSbtnReset" runat="server" Text="Reset" CausesValidation="false" class="hBtn btn-danger" TabIndex="8" OnClick="BRSbtnReset_Click" />
                </div>
            </div>
            <div class="col-sm-12 col-sm-12" style="background-color: white; width: 150%;" runat="server" id="divBRSGraph" visible="false">
                <div runat="server" id="divBoatRunningStatusGraph">
                    <figure class="highcharts-figure" style="border: 2px solid rgb(228, 211, 84);">
                        <div id="BRSGraph" style="height: 450px"></div>
                        <p class="highcharts-description">
                        </p>
                    </figure>
                </div>
            </div>

            <asp:HiddenField ID="hfBRSHeading" runat="server" />
            <asp:HiddenField ID="hfBRSTotalCapacity" runat="server" />
            <asp:HiddenField ID="hfBRSOnTravelTrips" runat="server" />
            <asp:HiddenField ID="hfBRSBookedTrips" runat="server" />
            <asp:HiddenField ID="hfBRSAvailableTrips" runat="server" />

            <script type="text/javascript">

                var sBRSHeading = $('#<%=hfBRSHeading.ClientID%>').val();
                var sBRSHeadingSplit = sBRSHeading.split(',');

                var sBRSTotalCapacity = $('#<%=hfBRSTotalCapacity.ClientID%>').val();
                var sBRSTotalCapacitySplit = sBRSTotalCapacity.split(',');

                var sBRSOnTravelTrips = $('#<%=hfBRSOnTravelTrips.ClientID%>').val();
                var sBRSOnTravelTripsSplit = sBRSOnTravelTrips.split(',');

                var sBRSBookedTrips = $('#<%=hfBRSBookedTrips.ClientID%>').val();
                var sBRSBookedTripsSplit = sBRSBookedTrips.split(',');

                var sBRSAvailableTrips = $('#<%=hfBRSAvailableTrips.ClientID%>').val();
                var sBRSAvailableTripsSplit = sBRSAvailableTrips.split(',');

                var TCData = new Array();
                var i = 0;
                for (var i = 0; i < sBRSHeadingSplit.length; i++) {
                    var entry = {
                        name: sBRSHeadingSplit[i],
                        y: parseFloat(sBRSTotalCapacitySplit[i])
                    }
                    TCData.push(entry);
                }

                var OTData = new Array();
                var j = 0;
                for (var j = 0; j < sBRSHeadingSplit.length; j++) {
                    var entry = {
                        name: sBRSHeadingSplit[j],
                        y: parseFloat(sBRSOnTravelTripsSplit[j])
                    }
                    OTData.push(entry);
                }

                var BTData = new Array();
                var k = 0;
                for (var k = 0; k < sBRSHeadingSplit.length; k++) {
                    var entry = {
                        name: sBRSHeadingSplit[k],
                        y: parseFloat(sBRSBookedTripsSplit[k])
                    }
                    BTData.push(entry);
                }

                var ATData = new Array();
                var p = 0;
                for (var p = 0; p < sBRSHeadingSplit.length; p++) {
                    var entry = {
                        name: sBRSHeadingSplit[p],
                        y: parseFloat(sBRSAvailableTripsSplit[p])
                    }

                    ATData.push(entry);
                }

                var hBoatHouse = document.getElementById("<%=BRSddlBoatHouse.ClientID%>");
                if (hBoatHouse.options[hBoatHouse.selectedIndex].value == "0") {
                    var BHHeading = "";
                }
                else {
                    var BHHeading = hBoatHouse.options[hBoatHouse.selectedIndex].text;
                }

                var hBoatType = document.getElementById("<%=BRSddlBoatType.ClientID%>");
                if (hBoatType.options[hBoatType.selectedIndex].value == "0") {
                    var BTHeading = "";
                }
                else {
                    var BTHeading = hBoatType.options[hBoatType.selectedIndex].text;
                }

                var hBoatSeat = document.getElementById("<%=BRSddlBoatSeater.ClientID%>");
                if (hBoatSeat.options[hBoatSeat.selectedIndex].value == "0") {
                    var BSHeading = "";
                }
                else {
                    var BSHeading = " ( " + hBoatSeat.options[hBoatSeat.selectedIndex].text + " ) ";
                }

                var sCurrentDateTime = new Date();
                var DateTime = sCurrentDateTime.getDate() + "/" + (sCurrentDateTime.getMonth() + 1) + "/" + sCurrentDateTime.getFullYear() + " "
                    + sCurrentDateTime.getHours() + ":"
                    + sCurrentDateTime.getMinutes() + ":"
                    + sCurrentDateTime.getSeconds();

                Highcharts.chart('BRSGraph', {
                    chart: {
                        type: 'column',
                        events: {
                            load: function () {
                                var chart = this;
                                chart.renderer.text('<span style="font-size:10px">(Click to Hide / Un-hide)</span>', 340, 440)
                                    .attr({
                                        zIndex: 3,
                                        fill: 'black',
                                        fontWeight: 'Bold'
                                    })
                                    .add();
                            }
                        }
                    },
                    title: {
                        text: BHHeading + " - Trip Status (" + DateTime + ")",
                        style: { fontWeight: 'bold' }
                    },
                    subtitle: {
                        text: BTHeading + BSHeading,
                        style: { fontWeight: 'bold' }
                    },
                    accessibility: {
                        announceNewData: { enabled: true }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: 'Boat Type (Seater)',
                            style: { fontWeight: 'bold' }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Trip (Numbers)',
                            style: { fontWeight: 'bold' }
                        }
                    },
                    legend: {
                        borderRadius: 0,
                        backgroundColor: '#FFFFFF',
                        y: -15,
                        x: 0,
                        useHTML: true,
                        shadow: { color: '#000', width: 3, opacity: 0.15, offsetY: 2, offsetX: 1 }
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: { enabled: true, format: '{point.y:,.0f}' }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                    },
                    series: [{ name: "Capacity", color: "gray", data: TCData },
                    { name: "Booked", color: "blue", data: BTData },
                    { name: "On-travel", color: "rgb(241, 92, 128)", data: OTData },
                    { name: "Available", color: "rgb(247, 163, 92)", data: ATData }],
                });
            </script>
        </div>

        <%--BOAT UTILIZATION--%>

        <div id="divBoatUtilization" runat="server" visible="false" style="margin-top: -3px; border: 2px solid rgb(255,192,203)">
            <div class="panel-body nav-item" style="background-color: rgb(255,192,203); text-align: center"></div>
            <div class="row p-2">
                <div class="col-sm-2 col-xs-12">
                    <div class="form-group">
                        <i class="fa fa-ship" aria-hidden="true"></i>
                        <asp:Label ID="Label31" runat="server" Text="Boat House" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="BUddlBoatHouse" CssClass="form-control inputboxstyle" runat="server" TabIndex="1"
                            AutoPostBack="true" OnSelectedIndexChanged="BUddlBoatHouse_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2 col-xs-12">
                    <i class="fa fa-ship" aria-hidden="true"></i>
                    <asp:Label ID="Label32" runat="server" Text="Boat Type" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="BUddlBoatType" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="BUddlBoatType_SelectedIndexChanged"
                        TabIndex="2">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2 col-xs-12">
                    <i class="fas fa-chair" aria-hidden="true"></i>
                    <asp:Label ID="Label33" runat="server" Text="Boat Seater" ForeColor="Black" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="BUddlBoatSeater" CssClass="form-control inputboxstyle" runat="server" TabIndex="3">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2 col-xs-12">
                    <div class="form-group">
                        <i class="fa fa-calendar" aria-hidden="true"></i>
                        <asp:Label ID="Label34" runat="server" Text="From Date" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:TextBox ID="BUFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="5" onkeydown="return false;">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="BUFromDate"
                            ValidationGroup="BUSearch" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-xs-12">
                    <div class="form-group">
                        <i class="fa fa-calendar" aria-hidden="true"></i>
                        <asp:Label ID="Label35" runat="server" Text="To Date" ForeColor="Black" Font-Bold="true"></asp:Label>
                        <asp:TextBox ID="BUToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="6" onkeydown="return false;">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="BUToDate"
                            ValidationGroup="BUSearch" SetFocusOnError="True" CssClass="vError">Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 22px">
                    <asp:Button ID="BUbtnSearch" runat="server" Text="Search" class="hBtn btn-primary" ValidationGroup="BUSearch" TabIndex="7" OnClick="BUbtnSearch_Click" />
                    <asp:Button ID="BUbtnReset" runat="server" Text="Reset" CausesValidation="false" class="hBtn btn-danger" TabIndex="8" OnClick="BUbtnReset_Click" />
                </div>
            </div>
            <div class="col-sm-12 col-sm-12">
                <div class="row">
                    <div class="dtlBoatTypeD col-sm-6" runat="server" id="divBuGraphTripCount" style="display: none; background-color: rgb(255,192,203);">
                        <div class="form-group">
                            <figure class="highcharts-figure" style="border: 2px solid rgb(255,192,203)">
                                <div id="BUGraphTripCount"></div>
                                <p class="highcharts-description">
                                </p>
                            </figure>
                        </div>
                    </div>
                    <div class="dtlBoatTypeD col-sm-6" runat="server" id="divBuGraphRevenue" style="display: none; background-color: rgb(255,192,203)">
                        <div class="form-group">
                            <figure class="highcharts-figure">
                                <div id="BUGraphRevenue"></div>
                                <p class="highcharts-description">
                                </p>
                            </figure>
                        </div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hfBUTotalCapacity" runat="server" />
            <asp:HiddenField ID="hfBUBookedTripsNos" runat="server" />
            <asp:HiddenField ID="hfBUBookedTripsRevenue" runat="server" />
            <asp:HiddenField ID="hfBUUnBookedTrips" runat="server" />
            <asp:HiddenField ID="hfBURevenueLoss" runat="server" />
            <asp:HiddenField ID="hfBUBoatHouse" runat="server" />
            <asp:HiddenField ID="hfBURevenueGain" runat="server" />
            <asp:HiddenField ID="hfBUTotCapacityRevenue" runat="server" />
            <asp:HiddenField ID="hfBUUnBookedRevenue" runat="server" />
            <script type="text/javascript">

                var sBUBoatHouse = $('#<%=hfBUBoatHouse.ClientID%>').val();
                var sBUBoatHouseSplit = sBUBoatHouse.split(',');

                var sBUTotalCapacity = $('#<%=hfBUTotalCapacity.ClientID%>').val();
                var sBUTotalCapacitySplit = sBUTotalCapacity.split(',');

                var sBUBookedTripsNos = $('#<%=hfBUBookedTripsNos.ClientID%>').val();
                var sBUBookedTripsNosSplit = sBUBookedTripsNos.split(',');

                var sBUBookedTripsRevenue = $('#<%=hfBUBookedTripsRevenue.ClientID%>').val();
                var sBUBookedTripsRevenueSplit = sBUBookedTripsRevenue.split(',');

                var sBUUnBookedTrips = $('#<%=hfBUUnBookedTrips.ClientID%>').val();
                var sBUUnBookedTripsSplit = sBUUnBookedTrips.split(',');

                var sBURevenueLoss = $('#<%=hfBURevenueLoss.ClientID%>').val();
                var sBURevenueLossSplit = sBURevenueLoss.split(',');

                var sBURevenueGain = $('#<%=hfBURevenueGain.ClientID%>').val();
                var sBURevenueGainSplit = sBURevenueGain.split(',');

                var sBUhfBUTotCapacityRevenue = $('#<%=hfBUTotCapacityRevenue.ClientID%>').val();
                var sBUhfBUTotCapacityRevenueSplit = sBUhfBUTotCapacityRevenue.split(',');

                var sBUhfBUUnBookedRevenue = $('#<%=hfBUUnBookedRevenue.ClientID%>').val();
                var sBUhfBUUnBookedRevenueSplit = sBUhfBUUnBookedRevenue.split(',');

                //alert(sBUhfBUUnBookedRevenueSplit);

                var BUTCData = new Array();
                var i = 0;
                for (var i = 0; i < sBUBoatHouseSplit.length; i++) {
                    var entry = {
                        name: sBUBoatHouseSplit[i],
                        y: parseFloat(sBUTotalCapacitySplit[i]),
                    }
                    BUTCData.push(entry);
                }

                var BUBTData = new Array();
                var j = 0;
                for (var j = 0; j < sBUBoatHouseSplit.length; j++) {
                    var entry = {
                        name: sBUBoatHouseSplit[j],
                        z: sBURevenueGainSplit[j],
                        y: parseFloat(sBUBookedTripsNosSplit[j])
                    }
                    BUBTData.push(entry);
                }

                var BUBRData = new Array();
                var k = 0;
                for (var k = 0; k < sBUBoatHouseSplit.length; k++) {
                    var entry = {
                        name: sBUBoatHouseSplit[k],
                        z: sBURevenueGainSplit[k],
                        y: parseFloat(sBUBookedTripsRevenueSplit[k])
                    }
                    BUBRData.push(entry);
                }

                var BUUTData = new Array();
                var p = 0;
                for (var p = 0; p < sBUBoatHouseSplit.length; p++) {
                    var entry = {
                        name: sBUBoatHouseSplit[p],
                        z: parseFloat(sBURevenueLossSplit[p]),
                        y: parseFloat(sBUUnBookedTripsSplit[p])
                    }
                    BUUTData.push(entry);
                }

                var BURLData = new Array();
                var p = 0;
                for (var p = 0; p < sBUBoatHouseSplit.length; p++) {
                    var entry = {
                        name: sBUBoatHouseSplit[p],
                        z: parseFloat(sBUUnBookedTripsSplit[p]),
                        y: parseFloat(sBURevenueLossSplit[p])
                    }
                    BURLData.push(entry);
                }

                var BURGData = new Array();
                var u = 0;
                for (var u = 0; u < sBUBoatHouseSplit.length; u++) {
                    var entry = {
                        name: sBUBoatHouseSplit[u],
                        z: parseFloat(sBUBookedTripsNosSplit[u]),
                        y: parseFloat(sBURevenueGainSplit[u])
                    }
                    BURGData.push(entry);
                }

                var BUTotCapData = new Array();
                var t = 0;
                for (var t = 0; t < sBUBoatHouseSplit.length; t++) {
                    var entry = {
                        name: sBUBoatHouseSplit[t],
                        z: parseFloat(sBUBookedTripsNosSplit[t]),
                        y: parseFloat(sBUhfBUTotCapacityRevenueSplit[t])
                    }
                    BUTotCapData.push(entry);
                }

                var BUUnBookedRevenueData = new Array();
                var q = 0;
                for (var q = 0; q < sBUBoatHouseSplit.length; q++) {
                    var entry = {
                        name: sBUBoatHouseSplit[q],
                        z: parseFloat(sBURevenueLossSplit[q]),
                        y: parseFloat(sBUhfBUUnBookedRevenueSplit[q])
                    }
                    BUUnBookedRevenueData.push(entry);
                }

                var hBUBoatHouse = document.getElementById("<%=BUddlBoatHouse.ClientID%>");
                if (hBUBoatHouse.options[hBUBoatHouse.selectedIndex].value == "0") {
                    var BHBUHeading = "(All Boat House)";
                }
                else {
                    var BHBUHeading = " - " + hBUBoatHouse.options[hBUBoatHouse.selectedIndex].text;
                }

                var hBUBoatType = document.getElementById("<%=BUddlBoatType.ClientID%>");
                if (hBUBoatType.options[hBUBoatType.selectedIndex].value == "0") {
                    var BTBUHeading = "";
                }
                else {
                    var BTBUHeading = hBUBoatType.options[hBUBoatType.selectedIndex].text;
                }

                var hBUBoatSeat = document.getElementById("<%=BUddlBoatSeater.ClientID%>");
                if (hBUBoatSeat.options[hBUBoatSeat.selectedIndex].value == "0") {
                    var BSBUHeading = "";
                }
                else {
                    var BSBUHeading = " ( " + hBUBoatSeat.options[hBUBoatSeat.selectedIndex].text + " ) ";
                }

                Highcharts.chart('BUGraphTripCount', {
                    chart: {
                        type: 'column',
                        events: {
                            load: function () {
                                var chart = this;
                                chart.renderer.text('<span style="font-size:10px">(Click to Hide / Un-hide)</span>', 250, 385)
                                    .attr({
                                        zIndex: 3,
                                        fill: 'black',
                                        fontWeight: 'Bold'
                                    })
                                    .add();
                            }
                        }
                    },
                    title: {
                        text: "Boat Utilization " + BHBUHeading,
                        style: { fontWeight: 'bold' }
                    },
                    subtitle: {
                        text: "Trip Count",
                        style: { fontWeight: 'bold' }
                    },
                    accessibility: {
                        announceNewData: { enabled: true }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: 'Boat House',
                            style: { fontWeight: 'bold' }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Trip Count (Numbers)',
                            style: { fontWeight: 'bold' }
                        }
                    },
                    legend: {
                        borderRadius: 0,
                        backgroundColor: '#FFFFFF',
                        y: -15,
                        x: 0,
                        useHTML: true,
                        shadow: { color: '#000', width: 3, opacity: 0.15, offsetY: 2, offsetX: 1 }
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: { enabled: true, format: '{point.y:,.0f}' }
                        }
                    },
                    tooltip: {
                        formatter: function () {
                            if (this.series.name != 'Capacity') {
                                return '<b>' + this.series.name +
                                    '</b><br/> Trip Count (Nos) : ' + this.point.y +
                                    '<br/> Percentage (%) : ' + this.point.z;
                            }
                            else {
                                return '<b>' + this.series.name +
                                    '</b><br/> Trip Count (Nos) : ' + this.point.y
                            }
                        }
                    },
                    series: [{ name: "Capacity", color: "#203e9e", data: BUTCData },
                    { name: "Booked", color: "rgb(241, 92, 128)", data: BUBTData },
                    { name: "UnBooked", color: "rgb(247, 163, 92)", data: BUUTData }],
                });
                Highcharts.setOptions({
                    lang: {
                        thousandsSep: ','
                    }
                });

                Highcharts.chart('BUGraphRevenue', {
                    chart: {
                        type: 'column',
                        events: {
                            load: function () {
                                var chart = this;
                                chart.renderer.text('<span style="font-size:10px">(Click to Hide / Un-hide)</span>', 250, 385)
                                    .attr({
                                        zIndex: 3,
                                        fill: 'black',
                                        fontWeight: 'Bold'
                                    })
                                    .add();
                            }
                        }
                    },
                    title: {
                        text: "Boat Utilization " + BHBUHeading,
                        style: { fontWeight: 'bold' }
                    },
                    subtitle: {
                        text: "Revenue",
                        style: { fontWeight: 'bold' }
                    },
                    accessibility: {
                        announceNewData: { enabled: true }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: 'Boat House',
                            style: { fontWeight: 'bold' }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Revenue (INR)',
                            style: { fontWeight: 'bold' }
                        }
                    },
                    legend: {
                        borderRadius: 0,
                        backgroundColor: '#FFFFFF',
                        y: -15,
                        x: 0,
                        useHTML: true,
                        shadow: { color: '#000', width: 3, opacity: 0.15, offsetY: 2, offsetX: 1 }
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: { enabled: true, format: '{point.y:,.0f}' }
                        }
                    },
                    tooltip: {
                        formatter: function () {
                            if (this.series.name != 'Capacity') {
                                return '<b>' + this.series.name +
                                    '</b><br/> Revenue : ' + Highcharts.numberFormat(this.point.y, 0) +
                                    '<br/> Percentage (%) : ' + this.point.z;
                            }
                            else {
                                return '<b>' + this.series.name +
                                    '</b><br/> Revenue : ' + Highcharts.numberFormat(this.point.y, 0)
                            }
                        }



                    },
                    series: [{ name: "Capacity", color: "#203e9e", data: BUTotCapData },
                    { name: "Booked", color: "green", data: BUBRData },
                    { name: "UnBooked", color: "red", data: BUUnBookedRevenueData }
                    ],
                });
            </script>
        </div>

        <%--OTHER REVENUE POPUP--%>

        <asp:HiddenField ID="hfpopup" runat="server" />

        <ajax:DragPanelExtender ID="DPEPopup" runat="server" TargetControlID="PnlRevenue" DragHandleID="pnlDragRevenue"></ajax:DragPanelExtender>
        <ajax:ModalPopupExtender ID="MpeRevenue" runat="server" BehaviorID="MpeRevenue" TargetControlID="hfpopup" PopupControlID="PnlRevenue"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="PnlRevenue" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; width: 500px; margin-top: 25px;">
            <asp:Panel ID="pnlDragRevenue" runat="server" CssClass="drag">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #004c8c; color: white">
                        <h5 class="modal-title">Other Revenue Details</h5>
                        <asp:ImageButton ID="ImgClose" runat="server" OnClick="ImgClose_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                    </div>
                    <div class="modal-body">
                        <table class="table table-bordered table-condenced CustomGrid">
                            <tr style="font-weight: bold; background-color: #004c8c; color: white" align="Center">
                                <td>Sl No.</td>
                                <td>Other Revenue Type</td>
                                <td>Amount </td>
                            </tr>
                            <tr>
                                <td align="Center">1</td>
                                <td>Extension Charges </td>
                                <td align="Right">
                                    <asp:Label ID="lblExtensionCharges" runat="server" Font-Bold="true"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="Center">2</td>
                                <td>Unclaimed Deposit </td>
                                <td align="Right">
                                    <asp:Label ID="lblUnclaimedDeposit" runat="server" Font-Bold="true"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="Center">3</td>
                                <td>Cancellation Charges </td>
                                <td align="Right">
                                    <asp:Label ID="lblCancellationCharges" runat="server" Font-Bold="true"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="Center">4</td>
                                <td>Re-scheduling Charges</td>
                                <td align="Right">
                                    <asp:Label ID="lblReschedulingCharges" runat="server" Font-Bold="true"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="Center">5</td>
                                <td>Receipt Balance Amount</td>
                                <td align="Right">
                                    <asp:Label ID="lblBalanceAmount" runat="server" Font-Bold="true"></asp:Label></td>
                            </tr>
                            <tr style="font-weight: bold">
                                <td colspan="2" align="Center">TOTAL</td>
                                <td align="Right">
                                    <asp:Label ID="lblTotalCharges" runat="server" ForeColor="green" Font-Bold="true" Font-Size="16px"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>

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

        <%-- OTHER SERVICE REVENUE POPUP--%>

        <asp:HiddenField ID="hfOSpopup" runat="server" />

        <ajax:DragPanelExtender ID="DPEOSpopup" runat="server" TargetControlID="PnlOSRevenue" DragHandleID="pnlDragOSRevenue"></ajax:DragPanelExtender>
        <ajax:ModalPopupExtender ID="MPEOSpopup" runat="server" BehaviorID="MPEOSpopup" TargetControlID="hfOSpopup" PopupControlID="PnlOSRevenue"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="PnlOSRevenue" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; width: 500px; margin-top: 25px;">
            <asp:Panel ID="pnlDragOSRevenue" runat="server" CssClass="drag">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #004c8c; color: white">
                        <h5 class="modal-title">Other Service Revenue Details</h5>
                        <asp:ImageButton ID="ImgCloseOS" runat="server" OnClick="ImgCloseOS_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvOSpopup" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
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
                                            <asp:Label ID="lblPaymentModepopos" runat="server" Text='<%# Bind("PaymentMode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatBookingRevenuepopos" runat="server" Text='<%# Bind("OtherServiceRevenue") %>' Font-Bold="true"></asp:Label>
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

        <%--RESTAURANT REVENUE POPUP--%>

        <asp:HiddenField ID="hfRESpopup" runat="server" />

        <ajax:DragPanelExtender ID="DPERESpopup" runat="server" TargetControlID="PnlRESRevenue" DragHandleID="pnlDragRESRevenue"></ajax:DragPanelExtender>
        <ajax:ModalPopupExtender ID="MPERESpopup" runat="server" BehaviorID="MPERESpopup" TargetControlID="hfRESpopup" PopupControlID="PnlRESRevenue"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="PnlRESRevenue" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; width: 500px; margin-top: 25px;">
            <asp:Panel ID="pnlDragRESRevenue" runat="server" CssClass="drag">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #004c8c; color: white">
                        <h5 class="modal-title">Restaurant Revenue Details</h5>
                        <asp:ImageButton ID="ImgCloseRes" runat="server" OnClick="ImgCloseRes_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvRESpopup" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
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
                                            <asp:Label ID="lblPaymentModepopores" runat="server" Text='<%# Bind("PaymentMode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatBookingRevenuepopres" runat="server" Text='<%# Bind("RestaurantRevenue") %>' Font-Bold="true"></asp:Label>
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

        <%--OVERALL REVENUE POPUP--%>

        <asp:HiddenField ID="hfORpopup" runat="server" />

        <ajax:DragPanelExtender ID="DPEORpopup" runat="server" TargetControlID="PnlORRevenue" DragHandleID="pnlDragORRevenue"></ajax:DragPanelExtender>
        <ajax:ModalPopupExtender ID="MPEORpopup" runat="server" BehaviorID="MPEORpopup" TargetControlID="hfORpopup" PopupControlID="PnlORRevenue"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="PnlORRevenue" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; width: 500px; margin-top: 25px;">
            <asp:Panel ID="pnlDragORRevenue" runat="server" CssClass="drag">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #004c8c; color: white">
                        <h5 class="modal-title">Overall Revenue Details</h5>
                        <asp:ImageButton ID="ImgCloseOR" runat="server" OnClick="ImgCloseOR_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvORpopup" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
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
                                            <asp:Label ID="lblPaymentModepopor" runat="server" Text='<%# Bind("PaymentMode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatBookingRevenuepopor" runat="server" Text='<%# Bind("OverallRevenue") %>' Font-Bold="true"></asp:Label>
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

        <%--Rower Settlement  POPUP--%>

        <asp:HiddenField ID="hfRowerpopup" runat="server" />

        <ajax:DragPanelExtender ID="DPERowerpopup" runat="server" TargetControlID="PnlRowerRevenue" DragHandleID="pnlDragRowerRevenue"></ajax:DragPanelExtender>
        <ajax:ModalPopupExtender ID="MPERowerpopup" runat="server" BehaviorID="MPERowerpopup" TargetControlID="hfRowerpopup" PopupControlID="PnlRowerRevenue"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="PnlRowerRevenue" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; width: 500px; margin-top: 25px;">
            <asp:Panel ID="pnlDragRowerRevenue" runat="server" CssClass="drag">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #004c8c; color: white">
                        <h5 class="modal-title">Rower Revenue Details</h5>
                        <asp:ImageButton ID="ImgCloseRower" runat="server" OnClick="ImgCloseRower_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvRowerpopup" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced CustomGrid"
                                AutoGenerateColumns="False" ShowFooter="true" PageSize="10" OnPageIndexChanging="gvRowerpopup_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rower Name" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowerNamepopores" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowerRevenuepopres" runat="server" Text='<%# Bind("RowerRevenue") %>' Font-Bold="true"></asp:Label>
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

    </div>

    <asp:HiddenField ID="hfUrl" runat="server" />
    <asp:HiddenField ID="hfCount" runat="server" />
    <asp:HiddenField ID="hfStatus" runat="server" />
    <asp:HiddenField ID="hfOtherServiceCount" runat="server" />
    <asp:HiddenField ID="hfOtherServiceCategory" runat="server" />
    <asp:HiddenField ID="hfInput2" runat="server" />
    <asp:HiddenField ID="hfRCategoryName" runat="server" />
    <asp:HiddenField ID="hfRCount" runat="server" />
    <asp:HiddenField ID="hfdCount" runat="server" />
    <asp:HiddenField ID="hfdStatus" runat="server" />
    <asp:HiddenField ID="hfCancelledCount" runat="server" />
    <asp:HiddenField ID="hfCancelledBoatHouse" runat="server" />
    <asp:HiddenField ID="hfTravelledCount" runat="server" />
    <asp:HiddenField ID="hfTravelledBoatHouse" runat="server" />
    <asp:HiddenField ID="hfAllResService" runat="server" />
    <asp:HiddenField ID="hfAllResServiceCount" runat="server" />
    <asp:HiddenField ID="hfRescheduleCharge" runat="server" />
    <asp:HiddenField ID="hfRescheduleBH" runat="server" />
    <asp:HiddenField ID="hfOSBoatHouse" runat="server" />
    <asp:HiddenField ID="hfOSCount" runat="server" />
    <asp:HiddenField ID="hfOSCategory" runat="server" />
    <asp:HiddenField ID="hfRESBoatHouse" runat="server" />
    <asp:HiddenField ID="hfRESCount" runat="server" />
    <asp:HiddenField ID="hfRESCategory" runat="server" />
    <asp:HiddenField ID="hfUserRole" runat="server" />
    <asp:HiddenField ID="hfRcDate" runat="server" />
    <asp:HiddenField ID="hfBookedAmt" runat="server" />
    <asp:HiddenField ID="hfCancelledAmt" runat="server" />
    <asp:HiddenField ID="hfRescheduleAmt" runat="server" />
    <asp:HiddenField ID="hfOtherServiceAmt" runat="server" />
    <asp:HiddenField ID="hfRestaurantAmt" runat="server" />

    <asp:HiddenField ID="hfAllBHID" runat="server" />
    <asp:HiddenField ID="hfAllBHIDOS" runat="server" />
    <asp:HiddenField ID="hfAllBHIDRES" runat="server" />
    <script>
        //BOOKING SUMMARY

        var Sselectedvalue = document.getElementById("<%=rbtnType.ClientID%>");
        if (Sselectedvalue != null) {
            var selectedvalue = Sselectedvalue.options[Sselectedvalue.selectedIndex].value;

            var value = document.getElementById("<%=ddlBoatHouse.ClientID%>");
            if (value.options[value.selectedIndex].text == "All") {
                var name = "All Boat House";
            }
            else {
                var name = value.options[value.selectedIndex].text;
            }
            var sBoatHouseId = value.options[value.selectedIndex].value;

            var ServiceName = document.getElementById("<%=ddlServiceName.ClientID%>");
            var sServiceName = ServiceName.options[ServiceName.selectedIndex].value;

            if (sServiceName == "1") {
                var BoatTypeName = document.getElementById("<%=ddlBoatType.ClientID%>");
                var sBoatTypeName = BoatTypeName.options[BoatTypeName.selectedIndex].value;
                var ssBoatTypeName = BoatTypeName.options[BoatTypeName.selectedIndex].text;

                var BoatSeater = document.getElementById("<%=ddlBoatSeater.ClientID%>");
                var sBoatSeaterId = BoatSeater.options[BoatSeater.selectedIndex].value;
                var ssBoatSeaterName = BoatSeater.options[BoatSeater.selectedIndex].text;
            }

            var subTitle = "";
            var xAxisTitle = "";
            if (sBoatHouseId == "0") {
                if (sBoatTypeName == undefined) {
                    subTitle = "Click the Boat House to view Boat Type wise breakup";
                    xAxisTitle = "Boat Types";
                }
                else if (sBoatTypeName == "0") {
                    subTitle = "Click the Boat House to view Boat Type wise breakup";
                    xAxisTitle = "Boat Types";
                }
                else if (sBoatSeaterId == "0") {
                    subTitle = "Click the Boat House to view Seater wise breakup";
                    xAxisTitle = "Seater Types";
                }
                else {
                    subTitle = "";
                    xAxisTitle = "";
                }
            }
            else {
                if (sBoatTypeName == undefined) {
                    subTitle = "Click the Boat House to view Boat Type wise breakup";
                    xAxisTitle = "Boat Types";
                }
                else if (sBoatTypeName == "0") {
                    subTitle = "Click the Boat House to view Boat Type wise breakup";
                    xAxisTitle = "Boat Types";
                }
                else if (sBoatSeaterId == "0") {
                    subTitle = "Click the Boat House to view Seater wise breakup";
                    xAxisTitle = "Seater Types";
                }
                else {
                    subTitle = "";
                    xAxisTitle = "";
                }
            }

            if (selectedvalue == "0") {

                //BOAT BOOKING - TICKET COUNT

                var data = $('#<%=hfCount.ClientID%>').val();
                var datasplit = data.split(',');

                var dataStatus = $('#<%=hfStatus.ClientID%>').val();
                var dataStatussplit = dataStatus.split(',');

                var dataBHID = $('#<%=hfAllBHID.ClientID%>').val();
                var dataBHIDsplit = dataBHID.split(',');

                var sData = new Array();
                var k = 0;
                for (var k = 0; k < dataStatussplit.length; k++) {
                    var entry = {
                        name: dataStatussplit[k],
                        id: dataBHIDsplit[k],
                        y: parseFloat(datasplit[k]),
                        drilldown: dataStatussplit[k]
                    }
                    sData.push(entry);
                }

                var sbbHeading = "";
                if (sBoatHouseId == "0") {
                    sbbHeading = "Boat House";
                }
                else {
                    sbbHeading = "Boat Status";
                }

                Highcharts.chart('BoatGraph', {
                    chart: {
                        type: 'column',
                        events: {
                            drilldown: function (e) {
                                if (sBoatSeaterId == undefined) {
                                    if (!e.seriesOptions) {
                                        var chart = this;
                                        if (sServiceName == "1") {
                                            if (sBoatTypeName != "0") {
                                                this.xAxis[0].setTitle({ text: xAxisTitle });
                                                if (sBoatHouseId == "0") {
                                                    var dataArr = CallChildBoatBookingSeater(e.point.id);
                                                }
                                                else {
                                                    var dataArr = CallChildBoatBookingSeater(e.point.name);
                                                }
                                            }
                                            else {
                                                this.xAxis[0].setTitle({ text: xAxisTitle });
                                                if (sBoatHouseId == "0") {
                                                    var dataArr = CallChildBoatBooking(e.point.id);
                                                }
                                                else {
                                                    var dataArr = CallChildBoatBooking(e.point.name);
                                                }
                                            }
                                        }
                                        else {
                                            this.xAxis[0].setTitle({ text: xAxisTitle });
                                            if (sBoatHouseId == "0") {
                                                var dataArr = CallChildBoatBooking(e.point.id);
                                            }
                                            else {
                                                var dataArr = CallChildBoatBooking(e.point.name);
                                            }
                                        }
                                        data = {
                                            name: e.point.name,
                                            data: dataArr,
                                        },
                                            setTimeout(function () {
                                                chart.hideLoading();
                                                chart.addSeriesAsDrilldown(e.point, data);
                                            }, 1000);
                                    }
                                }
                                else if (sBoatSeaterId == "0") {
                                    if (!e.seriesOptions) {
                                        var chart = this;
                                        if (sServiceName == "1") {
                                            if (sBoatTypeName != "0") {
                                                this.xAxis[0].setTitle({ text: xAxisTitle });
                                                if (sBoatHouseId == "0") {
                                                    var dataArr = CallChildBoatBookingSeater(e.point.id);
                                                }
                                                else {
                                                    var dataArr = CallChildBoatBookingSeater(e.point.name);
                                                }
                                            }
                                            else {
                                                this.xAxis[0].setTitle({ text: xAxisTitle });
                                                if (sBoatHouseId == "0") {
                                                    var dataArr = CallChildBoatBooking(e.point.id);
                                                }
                                                else {
                                                    var dataArr = CallChildBoatBooking(e.point.name);
                                                }
                                            }
                                        }
                                        else {
                                            this.xAxis[0].setTitle({ text: xAxisTitle });
                                            if (sBoatHouseId == "0") {
                                                var dataArr = CallChildBoatBooking(e.point.id);
                                            }
                                            else {
                                                var dataArr = CallChildBoatBooking(e.point.name);
                                            }
                                        }
                                        data = {
                                            name: e.point.name,
                                            data: dataArr,
                                        },
                                            setTimeout(function () {
                                                chart.hideLoading();
                                                chart.addSeriesAsDrilldown(e.point, data);
                                            }, 1000);
                                    }
                                }
                            },
                            drillup: function (e) {
                                this.xAxis[0].setTitle({ text: sbbHeading });
                            }
                        }
                    },
                    title: {
                        text: 'Boat Booking' + ' - ' + name,
                        style: {
                            fontWeight: 'bold',
                        }
                    },
                    subtitle: {
                        text: subTitle
                    },
                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: sbbHeading,
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Ticket Count',
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:,.0f}'
                            }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                    },
                    series: [
                        {
                            name: "Summary",
                            colorByPoint: true,
                            data: sData
                        }
                    ],
                    drilldown: {
                        drillUpButton: {
                            theme: {
                                fill: 'white',
                                'stroke-width': 1,
                                stroke: 'silver',
                                r: 0,
                                states: {
                                    hover: {
                                        fill: 'silver'
                                    },
                                    select: {
                                        stroke: '#039',
                                        fill: 'silver'
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                }
                            }
                        },
                        series: []
                    }
                });

                //OTHER SERVICE - TICKET COUNT

                var OtherServicedata = $('#<%=hfOtherServiceCount.ClientID%>').val();
                var OtherServicedatasplit = OtherServicedata.split(',');

                var OtherServiceCategory = $('#<%=hfOtherServiceCategory.ClientID%>').val();
                var OtherServiceCategorysplit = OtherServiceCategory.split(',');

                var OSCount = $('#<%=hfOSCount.ClientID%>').val();
                var OSCountsplit = OSCount.split(',');

                var OSCategory = $('#<%=hfOSCategory.ClientID%>').val();
                var OSCategorysplit = OSCategory.split(',');

                var OSBoatHouse = $('#<%=hfOSBoatHouse.ClientID%>').val();
                var OSBoatHousesplit = OSBoatHouse.split(',');

                var OSBoatHouseId = $('#<%=hfAllBHIDOS.ClientID%>').val();
                var OSBoatHouseIdsplit = OSBoatHouseId.split(',');

                var osData = new Array();
                var k = 0;
                for (var k = 0; k < OtherServiceCategorysplit.length; k++) {
                    var entry = {
                        name: OtherServiceCategorysplit[k],
                        id: OSBoatHouseIdsplit[k],
                        y: parseFloat(OtherServicedatasplit[k]),
                        drilldown: true
                    }
                    osData.push(entry);
                }

                var AllOsData = new Array();
                var l = 0;
                for (var l = 0; l < OSCountsplit.length; l++) {
                    var entryAll = {
                        name: OSCategorysplit[l],
                        data: [
                            [OSBoatHousesplit[l], parseFloat(OSCountsplit[l])],
                        ]
                    }
                    AllOsData.push(entryAll);
                }

                var sosHeading = "";
                if (sBoatHouseId == "0") {
                    sosHeading = "Boat House";
                }
                else {
                    sosHeading = "Service Types";
                }

                Highcharts.chart('OtherServiceGraph', {
                    chart: {
                        type: 'column',
                        events: {
                            drilldown: function (e) {
                                if (!e.seriesOptions) {
                                    var chart = this;
                                    this.xAxis[0].setTitle({ text: 'Service Type' });
                                    if (sBoatHouseId == "0") {
                                        var dataArr = CallChildOtherService(e.point.id);
                                    }
                                    else {
                                        var dataArr = CallChildOtherService(e.point.name);
                                    }
                                    data = {
                                        name: e.point.name,
                                        data: dataArr
                                    }
                                    setTimeout(function () {
                                        chart.hideLoading();
                                        chart.addSeriesAsDrilldown(e.point, data);

                                    }, 1000);
                                }
                            },
                            drillup: function (e) {
                                this.xAxis[0].setTitle({ text: sosHeading });
                            },
                        },
                    },
                    title: {
                        text: 'Other Service' + ' - ' + name,
                        style: {
                            fontWeight: 'bold',
                        }
                    },
                    subtitle: {
                        text: 'Click the Boat House to view Service Type wise breakup'
                    },
                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: sosHeading,
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Ticket Count',
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:,.0f}'
                            }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                    },
                    series:
                        [
                            {
                                name: "Summary",
                                colorByPoint: true,
                                data: osData
                            }
                        ],
                    drilldown: {
                        drillUpButton: {
                            theme: {
                                fill: 'white',
                                'stroke-width': 1,
                                stroke: 'silver',
                                r: 0,
                                states: {
                                    hover: {
                                        fill: 'silver'
                                    },
                                    select: {
                                        stroke: '#039',
                                        fill: 'silver'
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                }
                            }
                        },
                        series: []
                    }
                });

                //RESTAURANT - TICKET COUNT

                var resdata = $('#<%=hfRCount.ClientID%>').val();
                var resdatasplit = resdata.split(',');

                var resCategory = $('#<%=hfRCategoryName.ClientID%>').val();
                var resCategorysplit = resCategory.split(',');

                var RESCount = $('#<%=hfRESCount.ClientID%>').val();
                var RESCountsplit = RESCount.split(',');

                var RESCategory = $('#<%=hfRESCategory.ClientID%>').val();
                var RESCategorysplit = RESCategory.split(',');

                var RESBoatHouse = $('#<%=hfRESBoatHouse.ClientID%>').val();
                var RESBoatHousesplit = RESBoatHouse.split(',');

                var RESdataBHID = $('#<%=hfAllBHIDRES.ClientID%>').val();
                var RESdataBHIDsplit = RESdataBHID.split(',');

                var resData = new Array();
                var k = 0;
                for (var k = 0; k < resCategorysplit.length; k++) {
                    var entry = {
                        name: resCategorysplit[k],
                        id: RESdataBHIDsplit[k],
                        y: parseFloat(resdatasplit[k]),
                        drilldown: true
                    }
                    resData.push(entry);
                }

                var AllRESData = new Array();
                var m = 0;
                for (var m = 0; m < RESCountsplit.length; m++) {
                    var entryAllRES = {
                        name: RESCategorysplit[m],
                        data: [
                            [RESBoatHousesplit[m], parseFloat(RESCountsplit[m])],
                        ]
                    }
                    AllRESData.push(entryAllRES);
                }

                var sresHeading = "";
                if (sBoatHouseId == "0") {
                    sresHeading = "Boat House";
                }
                else {
                    sresHeading = "Item Category";
                }
                Highcharts.chart('RestaurantGraph', {
                    chart: {
                        type: 'column',
                        events: {
                            drilldown: function (e) {
                                if (!e.seriesOptions) {
                                    var chart = this;
                                    this.xAxis[0].setTitle({ text: 'Item Category' });
                                    if (sBoatHouseId == "0") {
                                        var dataArr = CallChildRestaurant(e.point.id);
                                    }
                                    else {
                                        var dataArr = CallChildRestaurant(e.point.name);
                                    }

                                    data = {
                                        name: e.point.name,
                                        data: dataArr
                                    }
                                    setTimeout(function () {
                                        chart.hideLoading();
                                        chart.addSeriesAsDrilldown(e.point, data);
                                    }, 1000);
                                }
                            },
                            drillup: function (e) {
                                this.xAxis[0].setTitle({ text: sresHeading });

                            }
                        }
                    },
                    title: {
                        text: 'Restaurant' + ' - ' + name,
                        style: {
                            fontWeight: 'bold',
                        }
                    },
                    subtitle: {
                        text: 'Click the Boat House to view Item Category wise breakup'
                    },
                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: sresHeading,
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Ticket Count',
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:,.0f}'
                            }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                    },
                    series: [
                        {
                            name: "Summary",
                            colorByPoint: true,
                            data: resData
                        }
                    ],
                    drilldown: {
                        drillUpButton: {
                            theme: {
                                fill: 'white',
                                'stroke-width': 1,
                                stroke: 'silver',
                                r: 0,
                                states: {
                                    hover: {
                                        fill: 'silver'
                                    },
                                    select: {
                                        stroke: '#039',
                                        fill: 'silver'
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                }
                            }
                        },
                        series: []
                    }
                });
            }
            else {

                //BOAT BOOKING - REVENUE

                var data = $('#<%=hfCount.ClientID%>').val();
                var datasplit = data.split(',');

                var dataStatus = $('#<%=hfStatus.ClientID%>').val();
                var dataStatussplit = dataStatus.split(',');

                var dataBHID = $('#<%=hfAllBHID.ClientID%>').val();
                var dataBHIDsplit = dataBHID.split(',');

                var sData = new Array();
                var k = 0;
                for (var k = 0; k < dataStatussplit.length; k++) {
                    var entry = {
                        name: dataStatussplit[k],
                        id: dataBHIDsplit[k],
                        y: parseFloat(datasplit[k]),
                        drilldown: dataStatussplit[k]
                    }
                    sData.push(entry);
                }

                /***drill data - booking***/
                var brdrilldata = $('#<%=hfdCount.ClientID%>').val();
                var brdrilldatasplit = brdrilldata.split(',');

                var brdrilldataStatus = $('#<%=hfdStatus.ClientID%>').val();
                var brdrilldataStatussplit = brdrilldataStatus.split(',');

                var brdrillData = new Array();
                var k = 0;

                for (var k = 0; k < brdrilldataStatussplit.length; k++) {
                    var entrybr = {
                        name: brdrilldataStatussplit[k],
                        y: parseFloat(brdrilldatasplit[k]),
                    }
                    brdrillData.push(entrybr);
                }

                /***drill data - cancelled***/
                var crdrilldata = $('#<%=hfCancelledCount.ClientID%>').val();
                var crdrilldatasplit = crdrilldata.split(',');

                var crdrilldataStatus = $('#<%=hfCancelledBoatHouse.ClientID%>').val();
                var crdrilldataStatussplit = crdrilldataStatus.split(',');

                var crdrillData = new Array();
                var k = 0;
                for (var k = 0; k < crdrilldataStatussplit.length; k++) {
                    var entrycr = {
                        name: crdrilldataStatussplit[k],
                        y: parseFloat(crdrilldatasplit[k]),
                    }
                    crdrillData.push(entrycr);
                }

                /***drill data - Reschedule***/
                var rrdrilldata = $('#<%=hfRescheduleCharge.ClientID%>').val();
                var rrdrilldatasplit = rrdrilldata.split(',');

                var rrdrilldataStatus = $('#<%=hfRescheduleBH.ClientID%>').val();
                var rrdrilldataStatussplit = rrdrilldataStatus.split(',');

                var rrdrillData = new Array();
                var k = 0;
                for (var k = 0; k < rrdrilldataStatussplit.length; k++) {
                    var entryrr = {
                        name: rrdrilldataStatussplit[k],
                        y: parseFloat(rrdrilldatasplit[k]),
                    }
                    rrdrillData.push(entryrr);
                }

                var sbbrHeading = "";
                if (sBoatHouseId == "0") {
                    sbbrHeading = "Boat House";
                }
                else {
                    sbbrHeading = "Boat Status";
                }

                Highcharts.setOptions({
                    lang: {
                        thousandsSep: ','
                    }
                });

                Highcharts.chart('BoatGraph', {
                    chart: {
                        type: 'column',
                        events: {
                            drilldown: function (e) {
                                if (sBoatSeaterId == undefined) {
                                    if (!e.seriesOptions) {
                                        var chart = this;
                                        if (sServiceName == "1") {
                                            if (sBoatTypeName != "0") {
                                                this.xAxis[0].setTitle({ text: xAxisTitle });
                                                if (sBoatHouseId == "0") {
                                                    var dataArr = CallChildBoatBookingSeater(e.point.id);
                                                }
                                                else {
                                                    var dataArr = CallChildBoatBookingSeater(e.point.name);
                                                }
                                            }
                                            else {
                                                this.xAxis[0].setTitle({ text: xAxisTitle });
                                                if (sBoatHouseId == "0") {
                                                    var dataArr = CallChildBoatBooking(e.point.id);
                                                }
                                                else {
                                                    var dataArr = CallChildBoatBooking(e.point.name);
                                                }
                                            }
                                        }
                                        else {
                                            this.xAxis[0].setTitle({ text: xAxisTitle });
                                            if (sBoatHouseId == "0") {
                                                var dataArr = CallChildBoatBooking(e.point.id);
                                            }
                                            else {
                                                var dataArr = CallChildBoatBooking(e.point.name);
                                            }
                                        }
                                        data = {
                                            name: e.point.name,
                                            data: dataArr,
                                        },
                                            setTimeout(function () {
                                                chart.hideLoading();
                                                chart.addSeriesAsDrilldown(e.point, data);
                                            }, 1000);
                                    }
                                }
                                else if (sBoatSeaterId == "0") {
                                    if (!e.seriesOptions) {
                                        var chart = this;
                                        if (sServiceName == "1") {
                                            if (sBoatTypeName != "0") {
                                                this.xAxis[0].setTitle({ text: xAxisTitle });
                                                if (sBoatHouseId == "0") {
                                                    var dataArr = CallChildBoatBookingSeater(e.point.id);
                                                }
                                                else {
                                                    var dataArr = CallChildBoatBookingSeater(e.point.name);
                                                }
                                            }
                                            else {
                                                this.xAxis[0].setTitle({ text: xAxisTitle });
                                                if (sBoatHouseId == "0") {
                                                    var dataArr = CallChildBoatBooking(e.point.id);
                                                }
                                                else {
                                                    var dataArr = CallChildBoatBooking(e.point.name);
                                                }
                                            }
                                        }
                                        else {
                                            this.xAxis[0].setTitle({ text: xAxisTitle });
                                            if (sBoatHouseId == "0") {
                                                var dataArr = CallChildBoatBooking(e.point.id);
                                            }
                                            else {
                                                var dataArr = CallChildBoatBooking(e.point.name);
                                            }
                                        }
                                        data = {
                                            name: e.point.name,
                                            data: dataArr,
                                        },
                                            setTimeout(function () {
                                                chart.hideLoading();
                                                chart.addSeriesAsDrilldown(e.point, data);
                                            }, 1000);
                                    }
                                }
                            },
                            drillup: function (e) {
                                this.xAxis[0].setTitle({ text: sbbrHeading });
                            }
                        }
                    },
                    title: {
                        text: 'Boat Booking' + ' - ' + name,
                        style: {
                            fontWeight: 'bold',
                        }
                    },
                    subtitle: {

                        text: subTitle
                    },
                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: sbbrHeading,
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Revenue (INR)',
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:,.0f}'
                            }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                    },
                    series: [
                        {
                            name: "Summary",
                            colorByPoint: true,
                            data: sData
                        },
                    ],
                    drilldown: {
                        drillUpButton: {
                            theme: {
                                fill: 'white',
                                'stroke-width': 1,
                                stroke: 'silver',
                                r: 0,
                                states: {
                                    hover: {
                                        fill: 'silver'
                                    },
                                    select: {
                                        stroke: '#039',
                                        fill: 'silver'
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                }
                            }
                        },
                        series: []
                    }
                });

                //OTHER SERVICE - REVENUE

                var OtherServicedata = $('#<%=hfOtherServiceCount.ClientID%>').val();
                var OtherServicedatasplit = OtherServicedata.split(',');

                var OtherServiceCategory = $('#<%=hfOtherServiceCategory.ClientID%>').val();
                var OtherServiceCategorysplit = OtherServiceCategory.split(',');

                var OSCount = $('#<%=hfOSCount.ClientID%>').val();
                var OSCountsplit = OSCount.split(',');

                var OSCategory = $('#<%=hfOSCategory.ClientID%>').val();
                var OSCategorysplit = OSCategory.split(',');

                var OSBoatHouse = $('#<%=hfOSBoatHouse.ClientID%>').val();
                var OSBoatHousesplit = OSBoatHouse.split(',');

                var OSdataBHID = $('#<%=hfAllBHIDOS.ClientID%>').val();
                var OSdataBHIDsplit = OSdataBHID.split(',');

                var osData = new Array();
                var k = 0;
                for (var k = 0; k < OtherServiceCategorysplit.length; k++) {
                    var entry = {
                        name: OtherServiceCategorysplit[k],
                        id: OSdataBHIDsplit[k],
                        y: parseFloat(OtherServicedatasplit[k]),
                        drilldown: true
                    }
                    osData.push(entry);
                }

                var AllOsData = new Array();
                var l = 0;
                for (var l = 0; l < OSCountsplit.length; l++) {

                    var entryAll = {

                        name: OSCategorysplit[l],
                        data: [
                            [OSBoatHousesplit[l], parseFloat(OSCountsplit[l])],
                        ]
                    }
                    AllOsData.push(entryAll);
                }

                var sosrHeading = "";
                if (sBoatHouseId == "0") {
                    sosrHeading = "Boat House";
                }
                else {
                    sosrHeading = "Service Types";
                }
                Highcharts.setOptions({
                    lang: {
                        thousandsSep: ','
                    }
                });
                Highcharts.chart('OtherServiceGraph', {
                    chart: {
                        type: 'column',
                        events: {
                            drilldown: function (e) {
                                if (!e.seriesOptions) {
                                    var chart = this;
                                    this.xAxis[0].setTitle({ text: 'Service Type' });
                                    if (sBoatHouseId == "0") {
                                        var dataArr = CallChildOtherService(e.point.id);
                                    }
                                    else {
                                        var dataArr = CallChildOtherService(e.point.name);
                                    }
                                    data = {
                                        name: e.point.name,
                                        data: dataArr
                                    }
                                    setTimeout(function () {
                                        chart.hideLoading();
                                        chart.addSeriesAsDrilldown(e.point, data);
                                    }, 1000);
                                }
                            },
                            drillup: function (e) {
                                this.xAxis[0].setTitle({ text: sosrHeading });

                            }
                        }
                    },
                    title: {
                        text: 'Other Service' + ' - ' + name,
                        style: {
                            fontWeight: 'bold',
                        }
                    },
                    subtitle: {
                        text: 'Click the Boat House to view Service Type wise breakup'
                    },
                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: sosrHeading,
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Revenue (INR)',
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:,.0f}'
                            }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                    },
                    series: [
                        {
                            name: "Summary",
                            colorByPoint: true,
                            data: osData
                        }
                    ],
                    drilldown: {
                        drillUpButton: {
                            theme: {
                                fill: 'white',
                                'stroke-width': 1,
                                stroke: 'silver',
                                r: 0,
                                states: {
                                    hover: {
                                        fill: 'silver'
                                    },
                                    select: {
                                        stroke: '#039',
                                        fill: 'silver'
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                }
                            }

                        },
                        series: []
                    }
                });

                //RESTAURANT - REVENUE

                var resdata = $('#<%=hfRCount.ClientID%>').val();
                var resdatasplit = resdata.split(',');

                var resCategory = $('#<%=hfRCategoryName.ClientID%>').val();
                var resCategorysplit = resCategory.split(',');

                var RESCount = $('#<%=hfRESCount.ClientID%>').val();
                var RESCountsplit = RESCount.split(',');

                var RESCategory = $('#<%=hfRESCategory.ClientID%>').val();
                var RESCategorysplit = RESCategory.split(',');

                var RESBoatHouse = $('#<%=hfRESBoatHouse.ClientID%>').val();
                var RESBoatHousesplit = RESBoatHouse.split(',');

                var RESdataBHID = $('#<%=hfAllBHIDRES.ClientID%>').val();
                var RESdataBHIDsplit = RESdataBHID.split(',');

                var resData = new Array();
                var k = 0;
                for (var k = 0; k < resCategorysplit.length; k++) {
                    var entry = {
                        name: resCategorysplit[k],
                        id: RESdataBHIDsplit[k],
                        y: parseFloat(resdatasplit[k]),
                        drilldown: true
                    }
                    resData.push(entry);
                }

                var AllRESData = new Array();
                var m = 0;
                for (var m = 0; m < RESCountsplit.length; m++) {

                    var entryAllRES = {

                        name: RESCategorysplit[m],
                        data: [
                            [RESBoatHousesplit[m], parseFloat(RESCountsplit[m])],
                        ]
                    }
                    AllRESData.push(entryAllRES);
                }
                var sresrHeading = "";
                if (sBoatHouseId == "0") {
                    sresrHeading = "Boat House";
                }
                else {
                    sresrHeading = "Item Category";
                }
                Highcharts.setOptions({
                    lang: {
                        thousandsSep: ','
                    }
                });
                Highcharts.chart('RestaurantGraph', {
                    chart: {
                        type: 'column',
                        events: {
                            drilldown: function (e) {
                                if (!e.seriesOptions) {
                                    var chart = this;
                                    this.xAxis[0].setTitle({ text: 'Item Category' });
                                    if (sBoatHouseId == "0") {
                                        var dataArr = CallChildRestaurant(e.point.id);
                                    }
                                    else {
                                        var dataArr = CallChildRestaurant(e.point.name);
                                    }

                                    data = {
                                        name: e.point.name,
                                        data: dataArr
                                    }

                                    setTimeout(function () {
                                        chart.hideLoading();
                                        chart.addSeriesAsDrilldown(e.point, data);
                                    }, 1000);
                                }
                            },
                            drillup: function (e) {
                                this.xAxis[0].setTitle({ text: sresrHeading });
                            }
                        }
                    },
                    title: {
                        text: 'Restaurant' + ' - ' + name,
                        style: {
                            fontWeight: 'bold',
                        }
                    },
                    subtitle: {
                        text: 'Click the Boat House to view Item Category wise breakup'
                    },
                    accessibility: {
                        announceNewData: {
                            enabled: true
                        }
                    },
                    xAxis: {
                        type: 'category',
                        title: {
                            enabled: true,
                            text: sresrHeading,
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Revenue (INR)',
                            style: {
                                fontWeight: 'bold',
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:,.0f}'
                            }
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                    },
                    series: [
                        {
                            name: "Summary",
                            colorByPoint: true,
                            data: resData
                        }
                    ],
                    drilldown: {
                        drillUpButton: {
                            theme: {
                                fill: 'white',
                                'stroke-width': 1,
                                stroke: 'silver',
                                r: 0,
                                states: {
                                    hover: {
                                        fill: 'silver'
                                    },
                                    select: {
                                        stroke: '#039',
                                        fill: 'silver'
                                    }
                                },
                                style: {
                                    fontWeight: 'bold',
                                }
                            }

                        },
                        series: []
                    }
                });
            }

            // DRILL DOWN FOR RESTAURANT

            function CallChildRestaurant(name) {

                var dataUrl = document.getElementById("<%=hfUrl.ClientID%>").value + "GetDashBoardBookingDetails";

                if ((document.getElementById("<%=ddlBoatHouse.ClientID%>").value) == 0) {
                    if (selectedvalue == "0") {
                        var data = {
                            "QueryType": "RestaurantTicketCountBH", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": name,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": ""
                        };
                    }
                    else {
                        var data = {
                            "QueryType": "RestaurantRevenueBH", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": name,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": ""
                        };
                    }
                }
                else {
                    if (selectedvalue == "0") {
                        var data = {
                            "QueryType": "ByResCatName", "BoatHouseId": sBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": name,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": ""
                        };
                    }
                    else {
                        var data = {
                            "QueryType": "ByResCatNameRev", "BoatHouseId": sBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": name,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": ""
                        };
                    }
                }

                var Drilldowndata = new Array();

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: dataUrl,
                    data: JSON.stringify(data),
                    dataType: "json",
                    crossDomain: true,
                    success: function (response) {
                        if (response.Table.length > 0) {
                            for (var i = 0; i < response.Table.length; i++) {
                                if ((document.getElementById("<%=ddlBoatHouse.ClientID%>").value) == 0) {
                                    var sServiceName = response.Table[i].Category;
                                }
                                else {
                                    var sServiceName = response.Table[i].ServiceName;
                                }
                                if (selectedvalue == "0") {
                                    var sServiceCount = response.Table[i].Count;
                                }
                                else {
                                    var sServiceCount = response.Table[i].NetAmount;
                                }

                                var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                Drilldowndata.push(serie);
                            }
                        }
                    },
                    error: function (Result) {
                        alert("Error");
                    }
                })
                return Drilldowndata;
            }

            // DRILL DOWN FOR OTHER SERVICE

            function CallChildOtherService(name) {
                var dataUrl = document.getElementById("<%=hfUrl.ClientID%>").value + "GetDashBoardBookingDetails";
                if ((document.getElementById("<%=ddlBoatHouse.ClientID%>").value) == 0) {
                    if (selectedvalue == "0") {
                        var data = {
                            "QueryType": "OtherServiceCountBasedOnBoatHouse", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": name,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": ""
                        };
                    }
                    else {
                        var data = {
                            "QueryType": "OtherServiceRevenueBasedOnBoatHouse", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": name,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": ""
                        };
                    }
                }
                else {
                    if (selectedvalue == "0") {
                        var data = {
                            "QueryType": "ByOthserviceCatName", "BoatHouseId": sBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": name,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": ""
                        };
                    }
                    else {
                        var data = {
                            "QueryType": "ByOthserviceCatNameRev", "BoatHouseId": sBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": name,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": ""
                        };
                    }
                }

                var Drilldowndata = new Array();

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: dataUrl,
                    data: JSON.stringify(data),
                    dataType: "json",
                    crossDomain: true,
                    success: function (response) {
                        if (response.Table.length > 0) {
                            for (var i = 0; i < response.Table.length; i++) {
                                if ((document.getElementById("<%=ddlBoatHouse.ClientID%>").value) == 0) {
                                    var sServiceName = response.Table[i].Category;
                                }
                                else {
                                    var sServiceName = response.Table[i].ServiceName;
                                }

                                if (selectedvalue == "0") {
                                    var sServiceCount = response.Table[i].Count;
                                }
                                else {
                                    var sServiceCount = response.Table[i].NetAmount;
                                }
                                var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                Drilldowndata.push(serie);
                            }
                        }
                    },
                    error: function (Result) {
                        alert("Error");
                    }
                })
                return Drilldowndata;
            }

            // DRILL DOWN FOR BOAT BOOKING

            function CallChildBoatBooking(name) {

                var ddlServiceName = document.getElementById("<%=ddlServiceName.ClientID%>");
                var sddlServiceName = ddlServiceName.options[ddlServiceName.selectedIndex].value;
                var dataUrl = document.getElementById("<%=hfUrl.ClientID%>").value + "GetDashBoardBookingDetails";

                if (selectedvalue == "0") {
                    if ((document.getElementById("<%=ddlBoatHouse.ClientID%>").value) == 0) {
                        if (sddlServiceName == '1') {
                            var data = {
                                "QueryType": "BoatBookingBoatTypesCount", "BoatHouseId": sBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                                "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                                "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": "Booked"
                            };
                        }
                        else {
                            var data = {
                                "QueryType": "BoatBookingBoatTypesCount", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                                "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                                "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": "Booked"
                            };
                        }
                    }
                    else {
                        var data = {
                            "QueryType": "BoatBookingBoatTypesCount", "BoatHouseId": sBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": name
                        };
                    }
                }
                else {
                    if ((document.getElementById("<%=ddlBoatHouse.ClientID%>").value) == 0) {
                        if (sddlServiceName == '1') {
                            var data = {
                                "QueryType": "BoatBookingBoatTypesRevenue", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                                "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                                "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": "Booking"
                            };
                        }
                        else {

                            var data = {
                                "QueryType": "BoatBookingBoatTypesRevenue", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                                "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                                "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": "Booking"
                            };

                        }
                    }
                    else {
                        var data = {
                            "QueryType": "BoatBookingBoatTypesRevenue", "BoatHouseId": sBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": name
                        };
                    }
                }

                var Drilldowndata = new Array();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: dataUrl,
                    data: JSON.stringify(data),
                    dataType: "json",
                    crossDomain: true,
                    success: function (response) {
                        if (response.Table.length > 0) {
                            for (var i = 0; i < response.Table.length; i++) {
                                if ((document.getElementById("<%=ddlBoatHouse.ClientID%>").value) == 0) {
                                    if (sddlServiceName == '1') {
                                        var sServiceName = response.Table[i].BoatType;
                                        var sServiceCount = response.Table[i].Field2;
                                        var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                        Drilldowndata.push(serie);
                                    }
                                    else {
                                        var sServiceName = response.Table[i].BoatType
                                        var sServiceCount = response.Table[i].Field2;
                                        var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                        Drilldowndata.push(serie);
                                    }
                                }
                                else {
                                    var sServiceName = response.Table[i].BoatType;
                                    var sServiceCount = response.Table[i].Field2;
                                    var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                    Drilldowndata.push(serie);
                                }
                            }
                        }
                    },
                    error: function (Result) {
                        alert("Error");
                    }
                })
                return Drilldowndata;
            }

            function CallChildBoatBookingSeater(name) {

                var dataUrl = document.getElementById("<%=hfUrl.ClientID%>").value + "GetDashBoardBookingDetails";

                if ((document.getElementById("<%=ddlBoatHouse.ClientID%>").value) == 0) {
                    if (selectedvalue == '0') {
                        var data = {
                            "QueryType": "SeaterCountBasedBH", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": ssBoatTypeName,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": "Booked"
                        }
                    }
                    else {
                        var data = {
                            "QueryType": "SeaterCountBasedBH", "BoatHouseId": name, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": ssBoatTypeName,
                            "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                            "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": "Booking"
                        }
                    }
                }
                else {
                    var data = {
                        "QueryType": "SeaterCountBasedBH", "BoatHouseId": document.getElementById("<%=ddlBoatHouse.ClientID%>").value, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": ssBoatTypeName,
                        "FromDate": $('#<%=txtFromDate.ClientID%>').val(),
                        "ToDate": $('#<%=txtToDate.ClientID%>').val(), "BoatStatus": name
                    }
                }

                var Drilldowndata = new Array();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: dataUrl,
                    data: JSON.stringify(data),
                    dataType: "json",
                    crossDomain: true,
                    success: function (response) {
                        if (response.Table.length > 0) {
                            for (var i = 0; i < response.Table.length; i++) {
                                var sServiceName = response.Table[i].SeaterType;
                                var sServiceCount = response.Table[i].Field2;
                                var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                Drilldowndata.push(serie);
                            }
                        }
                    },
                    error: function (Result) {
                        alert("Error");
                    }
                })
                return Drilldowndata;
            }
        }
    </script>

    <%-- Day WISE REVENUE COMPARISON  --%>
    <%--   <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/data.js"></script>
    <script src="https://code.highcharts.com/modules/drilldown.js"></script>--%>
    <script type="text/javascript">

        var RCBoatHouse = document.getElementById("<%=RCddlBoatHouse.ClientID%>");
        var RCBoatHousename = "";
        if (RCBoatHouse != null) {
            if (RCBoatHouse.options[RCBoatHouse.selectedIndex].text == "All") {
                RCBoatHousename = "All Boat House";
            }
            else {
                RCBoatHousename = RCBoatHouse.options[RCBoatHouse.selectedIndex].text;
            }


            var RCBoatHouseId = RCBoatHouse.options[RCBoatHouse.selectedIndex].value;

            var RCServiceName = document.getElementById("<%=RCddlServiceName.ClientID%>");
            var sRCServiceName = RCServiceName.options[RCServiceName.selectedIndex].value;

            if (sRCServiceName == "1") {
                var RCBoatTypeName = document.getElementById("<%=RCddlBoatType.ClientID%>");
                var sRCBoatTypeName = RCBoatTypeName.options[RCBoatTypeName.selectedIndex].value;
                var ssRCBoatTypeName = RCBoatTypeName.options[RCBoatTypeName.selectedIndex].text;

                var RCBoatSeater = document.getElementById("<%=RCddlSeaterType.ClientID%>");
                var sRCBoatSeaterId = RCBoatSeater.options[RCBoatSeater.selectedIndex].value;
                var ssRCBoatSeaterName = RCBoatSeater.options[RCBoatSeater.selectedIndex].text;
            }

            var RCsubTitle = "";
            var xRCAxisTitle = "";

            var sBookingAmt = $('#<%=hfBookedAmt.ClientID%>').val();
            var sBookingAmtSplit = sBookingAmt.split(',');

            var sCancelAmt = $('#<%=hfCancelledAmt.ClientID%>').val();
            var sCancelAmtSplit = sCancelAmt.split(',');

            var sRescheduleAmt = $('#<%=hfRescheduleAmt.ClientID%>').val();
            var sRescheduleAmtSplit = sRescheduleAmt.split(',');

            var sRcDate = $('#<%=hfRcDate.ClientID%>').val();
            var sRcDateSplit = sRcDate.split(',');

            var RcBookingData = new Array();
            var i = 0;
            for (var i = 0; i < sRcDateSplit.length; i++) {
                var entry = {
                    name: sRcDateSplit[i],
                    y: parseFloat(sBookingAmtSplit[i]),
                    color: '#9CDC25'
                }
                RcBookingData.push(entry);
            }

            var RcCancelData = new Array();
            var i = 0;
            for (var i = 0; i < sRcDateSplit.length; i++) {
                var entry = {
                    name: sRcDateSplit[i],
                    y: parseFloat(sCancelAmtSplit[i]),
                    color: '#EC1525'
                }
                RcCancelData.push(entry);
            }

            var RcReschduleData = new Array();
            var i = 0;
            for (var i = 0; i < sRcDateSplit.length; i++) {
                var entry = {
                    name: sRcDateSplit[i],
                    y: parseFloat(sRescheduleAmtSplit[i]),
                    color: '#288CC2'
                }
                RcReschduleData.push(entry);
            }
            Highcharts.setOptions({
                lang: {
                    thousandsSep: ','
                }
            });
            Highcharts.chart('RCBoatGraph', {
                chart: {
                    type: 'line',
                },
                title: {
                    text: 'Boat Booking' + ' - ' + RCBoatHousename,
                    style: {
                        fontWeight: 'bold',
                    }
                },
                subtitle: {
                    text: RCsubTitle
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category',
                    title: {
                        enabled: true,
                        text: 'Date',
                        style: {
                            fontWeight: 'bold',
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: 'Revenue (INR)',
                        style: {
                            fontWeight: 'bold',
                        }
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:,.0f}'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                },
                series: [
                    {
                        name: "Summary",
                        colorByPoint: true,
                        data: RcBookingData
                    },
                    {
                        name: "Summary",
                        colorByPoint: true,
                        data: RcCancelData
                    },
                    {
                        name: "Summary",
                        colorByPoint: true,
                        data: RcReschduleData
                    }
                ]
            });

            function CallChildBoatBookingRC(name) {
                var RCddlServiceName = document.getElementById("<%=RCddlServiceName.ClientID%>");
                var sRCddlServiceName = RCddlServiceName.options[RCddlServiceName.selectedIndex].value;
                var dataUrl = document.getElementById("<%=hfUrl.ClientID%>").value + "GetDashBoardBookingDetails";

                if ((document.getElementById("<%=RCddlBoatHouse.ClientID%>").value) == 0) {
                    if (sRCddlServiceName == '1') {
                        var data = {
                            "QueryType": "DayWiseRevenueComparisonAllBHBoatTypes", "BoatHouseId": RCBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                            "FromDate": name,
                            "ToDate": name, "BoatStatus": ""
                        };
                    }
                    else {
                        var data = {
                            "QueryType": "DayWiseRevenueComparisonAllBH", "BoatHouseId": document.getElementById("<%=RCddlBoatHouse.ClientID%>").value, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                            "FromDate": name,
                            "ToDate": name, "BoatStatus": ""
                        };
                    }
                }
                else {
                    var data = {
                        "QueryType": "DayWiseRevenueComparisonAllBHBoatTypes", "BoatHouseId": RCBoatHouseId, "BoatTypeId": "", "BoatSeaterId": "", "ServiceType": "",
                        "FromDate": name,
                        "ToDate": name, "BoatStatus": ""
                    };
                }

                var Drilldowndata = new Array();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: dataUrl,
                    data: JSON.stringify(data),
                    dataType: "json",
                    crossDomain: true,
                    success: function (response) {
                        if (response.Table.length > 0) {
                            for (var i = 0; i < response.Table.length; i++) {
                                if ((document.getElementById("<%=RCddlBoatHouse.ClientID%>").value) == 0) {
                                    if (sRCddlServiceName == '1') {
                                        var sServiceName = response.Table[i].BoatType;
                                        var sServiceCount = response.Table[i].BookingAmt;
                                        var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                        Drilldowndata.push(serie);
                                    }
                                    else {
                                        var sServiceName = response.Table[i].BoatHouseName;
                                        var sServiceCount = response.Table[i].BookingAmt;
                                        var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                        Drilldowndata.push(serie);
                                    }
                                }
                                else {
                                    var sServiceName = response.Table[i].BoatType;
                                    var sServiceCount = response.Table[i].BookingAmt;
                                    var serie = { name: sServiceName, y: parseFloat(sServiceCount) };
                                    Drilldowndata.push(serie);
                                }
                            }
                        }
                    },
                    error: function (Result) {
                        alert("Error");
                    }
                })
                return Drilldowndata;
            }

            var sOtherServiceAmt = $('#<%=hfOtherServiceAmt.ClientID%>').val();
            var sOtherServiceAmtSplit = sOtherServiceAmt.split(',');

            var RcOtherServiceData = new Array();
            var i = 0;
            for (var i = 0; i < sRcDateSplit.length; i++) {
                var entry = {
                    name: sRcDateSplit[i],
                    y: parseFloat(sOtherServiceAmtSplit[i]),
                }
                RcOtherServiceData.push(entry);
            }
            Highcharts.setOptions({
                lang: {
                    thousandsSep: ','
                }
            });
            Highcharts.chart('RCOtherServiceGraph', {
                chart: {
                    type: 'line',
                },
                title: {
                    text: 'Other Service ' + ' - ' + RCBoatHousename,
                    style: {
                        fontWeight: 'bold',
                    }
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category',
                    title: {
                        enabled: true,
                        text: 'Date',
                        style: {
                            fontWeight: 'bold',
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: 'Revenue (INR)',
                        style: {
                            fontWeight: 'bold',
                        }
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:,.0f}'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                },
                series: [
                    {
                        name: "Summary",
                        colorByPoint: true,
                        data: RcOtherServiceData
                    }
                ],

            });

            var sRestaurantServiceAmt = $('#<%=hfRestaurantAmt.ClientID%>').val();
            var sRestaurantServiceAmtSplit = sRestaurantServiceAmt.split(',');

            var RcRestaurantData = new Array();
            var i = 0;
            for (var i = 0; i < sRcDateSplit.length; i++) {
                var entry = {
                    name: sRcDateSplit[i],
                    y: parseFloat(sRestaurantServiceAmtSplit[i]),
                }
                RcRestaurantData.push(entry);
            }

            Highcharts.setOptions({
                lang: {
                    thousandsSep: ','
                }
            });

            Highcharts.chart('RCRestaurantGraph', {
                chart: {
                    type: 'line',
                },
                title: {
                    text: 'Restaurant ' + ' - ' + RCBoatHousename,
                    style: {
                        fontWeight: 'bold',
                    }
                },
                accessibility: {
                    announceNewData: {
                        enabled: true
                    }
                },
                xAxis: {
                    type: 'category',
                    title: {
                        enabled: true,
                        text: 'Date',
                        style: {
                            fontWeight: 'bold',
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: 'Revenue (INR)',
                        style: {
                            fontWeight: 'bold',
                        }
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:,.0f}'
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.0f}</b><br/>'
                },
                series: [
                    {
                        name: "Summary",
                        colorByPoint: true,
                        data: RcRestaurantData
                    }
                ],
            });
        }
    </script>

    <%--Excel Download All Revenue--%>

    <div runat="server" id="divExcelRevenue" style="display: none;">
        <asp:GridView ID="grvAllExcelRevenue" HeaderStyle-BackColor="#f4f4f4" HeaderStyle-ForeColor="Black"
            RowStyle-BackColor="White" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#000"
            runat="server" AutoGenerateColumns="false" ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderText="Sno.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Font-Bold="True" />
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#f4f4f4" />
                </asp:TemplateField>

                <asp:BoundField DataField="BoatHouse" HeaderText="Boat House">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left" />
                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#f4f4f4" />
                </asp:BoundField>

                <asp:BoundField DataField="Boating" HeaderText="Boating">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#f4f4f4" />
                </asp:BoundField>

                <asp:BoundField DataField="OtherService" HeaderText="Other Service">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#f4f4f4" />
                </asp:BoundField>

                <asp:BoundField DataField="Restaurant" HeaderText="Restaurant">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#f4f4f4" />
                </asp:BoundField>

                <asp:BoundField DataField="OtherRevenue" HeaderText="Other Revenue">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#f4f4f4" />
                </asp:BoundField>

                <asp:BoundField DataField="Total" HeaderText="Total">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#f4f4f4" />
                </asp:BoundField>

                <asp:BoundField DataField="Average" HeaderText="Average / Day">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#f4f4f4" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

