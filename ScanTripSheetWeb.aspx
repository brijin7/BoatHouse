<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScanTripSheetWeb.aspx.cs" Inherits="ScanTripSheetWeb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.3/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function showHourGlass() {
            document.getElementById("HourGlass").style.display = 'block';
        }

    </script>
    <style>
        .BarCodeTextStart {
            width: 500px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
            background-color: #FFF8C6;
        }

        .BarCodeTextRower {
            width: 500px;
            height: 40px;
            border-color: skyblue;
            border-radius: 8px;
            background-color: #FFF8C6;
        }

        .txtbox {
            padding-left: 24rem;
            padding-top: 1rem;
        }

        #txtboxLab {
            padding-left: 24rem;
        }

        .blink {
            animation: blink-animation 1s linear infinite;
            -webkit-animation: blink-animation 1s linear infinite;
            /*padding-left: 8rem;*/
            padding-bottom: 0rem;
            padding-top: 0rem;
            color: #068a28;
            float: right;
        }

        .blinkLab {
            animation: blink-animation 1s linear infinite;
            -webkit-animation: blink-animation 1s linear infinite;
            padding-left: 5rem;
            padding-bottom: 0rem;
            padding-top: 0rem;
            color: #5e18b5;
        }

        .blinkend {
            animation: blink-animation 1s linear infinite;
            -webkit-animation: blink-animation 1s linear infinite;
            padding-left: 5rem;
            padding-bottom: 0rem;
            padding-top: 0rem;
            color: red;
            float: right;
        }

        .blinkAlready {
            animation: blink-animation 1s linear infinite;
            -webkit-animation: blink-animation 1s linear infinite;
            padding-bottom: 0rem;
            padding-top: 0rem;
            color: #0a85d1;
            padding-left: 5rem;
            float: right;
        }

        .lblblink {
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

        .slide {
            position: relative;
            left: -300px;
            padding-top: 0rem;
            padding-left: 0rem;
            -webkit-animation: slide 0.5s forwards;
            -webkit-animation-delay: 0s;
            animation: slide 0.5s forwards;
            animation-delay: 0s;
            color: black;
        }

        @-webkit-keyframes slide {
            100% {
                left: 0;
            }
        }

        @keyframes slide {
            100% {
                left: 0;
            }
        }
    </style>

    <script type="text/javascript">
        function HideLabel() {
            var seconds = 5;
            document.getElementById("<%=divTripStarted.ClientID %>").style.display = "none";
            document.getElementById("<%=divGridStart.ClientID %>").style.display = "none";
            setTimeout(function () {
                document.getElementById("<%=divFinalStatus.ClientID %>").style.display = "none";
            }, seconds * 2000);
        };
    </script>

    <script type="text/javascript">
        function HideLabelGrid() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=divTripStarted.ClientID %>").style.display = "block";
                document.getElementById("<%=divGridStart.ClientID %>").style.display = "block";
            }, seconds * 2000);
        };
    </script>


    <script type="text/javascript">
        function HideAlreadyEndLabel() {
            var seconds = 5;
            document.getElementById("<%=divGridStart.ClientID %>").style.display = "none";
            document.getElementById("<%=divTripStarted.ClientID %>").style.display = "none";
            setTimeout(function () {
                document.getElementById("<%=divAlreadyEndStatus.ClientID %>").style.display = "none";
            }, seconds * 2000);
        };
    </script>

    <script type="text/javascript">
        function HideEndLabel() {
            var seconds = 5;
            document.getElementById("<%=divGridStart.ClientID %>").style.display = "none";
            document.getElementById("<%=divTripStarted.ClientID %>").style.display = "none";
            setTimeout(function () {
                document.getElementById("<%=divFinalEndStatus.ClientID %>").style.display = "none";
            }, seconds * 2000);
        };
    </script>

    <script type="text/javascript">
        function HideEndLabelGrid(sec) {
            document.getElementById("<%=divGridStart.ClientID %>").style.display = "none";
            document.getElementById("<%=divTripStarted.ClientID %>").style.display = "none";
            document.getElementById("<%=divExtendedRower.ClientID %>").style.display = "block";
            document.getElementById("<%=imgEnd.ClientID %>").style.display = "none";
            //var seconds = 8;
            setTimeout(function () {
                document.getElementById("<%=divGridStart.ClientID %>").style.display = "Block";
                document.getElementById("<%=divTripStarted.ClientID %>").style.display = "Block";
                document.getElementById("<%=divExtendedRower.ClientID %>").style.display = "none";
                document.getElementById("<%=imgEnd.ClientID %>").style.display = "none";
            }, sec * 2000);
        };
    </script>

    <script type="text/javascript">
        function HideEndLabelGridAlready(sec) {
            //var seconds = 10;
            setTimeout(function () {
                document.getElementById("<%=divGridStart.ClientID %>").style.display = "Block";
                document.getElementById("<%=divTripStarted.ClientID %>").style.display = "Block";
                document.getElementById("<%=divExtendedRower.ClientID %>").style.display = "none";
                document.getElementById("<%=imgEnd.ClientID %>").style.display = "none";
            }, sec * 2000);
        };
    </script>
    <%--     <script type="text/javascript">
        function HideEndLabelGridRower() {
            var seconds = 5;
           
            setTimeout(function () {
                document.getElementById("<%=divGridStart.ClientID %>").style.display = "block";
                document.getElementById("<%=divTripStarted.ClientID %>").style.display = "block";
                 document.getElementById("<%=divExtendedRower.ClientID %>").style.display = "none";
                document.getElementById("<%=imgEnd.ClientID %>").style.display = "none";
            }, seconds * 2000);
        };
    </script>--%>

    <script type="text/javascript">
        function HideStartLabel() {
            var seconds = 2;
            setTimeout(HideModalPopup, 5000);
            return false;
        };
        function HideModalPopup() {
            $find("MpeTrip").hide();
            return false;
        }
    </script>

    <script type="text/javascript">
        function HideLabelRower() {
            var seconds = 5;
            setTimeout(function () {
                setTimeout(HideModalPopup, 3000);
                document.getElementById("<%=txtRowerDetails.ClientID %>").style.display = "none";
                document.getElementById("<%=txtStartDetails.ClientID %>").style.display = "inline-block";
                var txtStart = document.getElementById('<%=txtStartDetails.ClientID %>');
                txtStart.focus();
            }, seconds * 2000);
        };
        function HideModalPopup() {
            $find("MpeTrip").hide();
            return false;
        }
    </script>



    <script type="text/javascript">
        function StartBox() {
            var txtStart = document.getElementById('<%=txtStartDetails.ClientID %>');
            txtStart.focus();
        }
    </script>
    <script type="text/javascript">
        function ShowRowerEndGrid() {
            var seconds = 3;
            document.getElementById("<%=divExtendedRower.ClientID %>").style.display = "block";
            setTimeout(function () {
                document.getElementById("<%=divExtendedRower.ClientID %>").style.display = "none";
            }, seconds * 2000);
        };
    </script>
    <style>
        .img-fluid {
            width: 460px;
            height: 80px !important;
        }

        .sp1 {
            padding: 12px 0px 21px 0px;
            color: #0268ba;
            text-shadow: 0px 1px 0px #f1ebeb, 0px 2px 0px #c7c7c7, 0px 3px 0px #e2e2e2, 0px 4px 0px #b9b6b6, 0px 5px 0px #a5a2a2, 0px 6px 0px #bdb7b7, 0px 7px 0px #777575, 0px 8px 7px #859ed4;
            font-size: 2.0rem !important;
            font-family: 'Arial Rounded MT';
            font-weight: 800;
            text-transform: uppercase;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .sp2 {
            padding: 10px;
            font-size: 1.2rem !important;
            font-family: 'Arial Rounded MT';
            height: 2em;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #3278be;
        }

        .sp3 {
            padding: 12px 52px 21px 2px;
            font-size: 3.0rem !important;
            font-weight: 600;
            text-transform: uppercase;
            display: flex;
            align-items: center;
            justify-content: center;
            -webkit-background-clip: text;
            color: #203e9e;
            font-family: 'Roboto', sans-serif;
        }

        .sp4 {
            padding: -12px 52px 21px 2px;
            font-size: 2.0rem !important;
            font-weight: 400;
            text-transform: uppercase;
            display: flex;
            align-items: center;
            justify-content: center;
            -webkit-background-clip: text;
            font-family: 'Roboto', sans-serif;
        }

        .Status {
            border: 3px solid gray;
            width: 621px;
            max-height: 300px;
            min-height: 140px;
            margin-left: 18px;
            margin-right: 13px;
            padding-top: 0rem;
            border-radius: 10px;
            display: inline-block;
            background-color: white;
            text-align: left;
        }

        .EndStatus {
            border: 3px solid gray;
            width: 621px;
            max-height: 300px;
            min-height: 140px;
            margin-left: 18px;
            margin-right: 13px;
            padding-top: 0rem;
            border-radius: 10px;
            background-color: white;
            display: inline-block;
            text-align: left;
        }

        .AlreadyEndStatus {
            border: 3px solid gray;
            width: 621px;
            max-height: 300px;
            min-height: 140px;
            margin-left: 18px;
            margin-right: 13px;
            padding-top: 0rem;
            border-radius: 10px;
            background-color: white;
            display: inline-block;
            text-align: left;
        }

        .gvCustom {
        }


            .gvCustom .gvHead {
                font-size: 22px;
                color: white;
                background-color: #124a79;
            }

            .gvCustom .gvRow {
                color: black;
                background-color: #f3f3f3;
            }

            /***  Gridview Page Style  **/
            .gvCustom .gvPage {
                text-align: left;
            }

                .gvCustom .gvPage a, .gvPage span {
                    display: block;
                    height: 15px;
                    width: 20px;
                    font-weight: bold;
                    text-align: center;
                    text-decoration: none;
                    margin-left: 5px;
                }

                .gvCustom .gvPage a {
                    background-color: #f5f5f5;
                    color: #008230;
                    border: 1px solid White;
                }

                .gvCustom .gvPage span {
                    background-color: #124a79;
                    color: White;
                    border: 1px solid White;
                }

        .CountStart {
            padding-left: 2rem;
            font-size: 25px;
            float: left;
        }

        .CountEnd {
            padding-left: 2rem;
            font-size: 25px;
            float: right;
        }

        .slideImage {
            position: relative;
            left: -300px;
            padding-top: 0rem;
            padding-left: 0rem;
            -webkit-animation: slide 0.5s forwards;
            -webkit-animation-delay: 0s;
            animation: slide 0.5s forwards;
            animation-delay: 0s;
            color: black;
        }

        @-webkit-keyframes slide {
            100% {
                left: 0;
            }
        }

        @keyframes slide {
            100% {
                left: 0;
            }
        }

        .ImgDisplay {
            padding-left: 2.5rem;
        }

        .ImgStart {
            padding-left: 2.5rem;
        }

        .module {
            margin: 3%;
        }

        .countdown {
            text-align: center;
            font-size: 3rem;
        }

        .countdown__time {
            display: inline-block;
            width: 30%;
            margin: 0 auto;
            position: relative;
            border-radius: 6px;
            color: #000000;
            background-color: rgba(0, 0, 0, .1);
        }

        .countdown__time--green {
            color: green;
            border: 3px solid green;
        }

        .countdown__time--red {
            color: red;
            border: 3px solid red;
            animation: vibrate .2s 0s forwards;
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
    <script lang="javascript" type="text/javascript">
        var myVar = setInterval(myTimer, 0);
        // update the clock every second
        //var myVar= setInterval(updateTime, 0);
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

            $('#Date').text(date);
            $('#Time').text(Time);
            // document.getElementById("Date").innerText = date;
            // document.getElementById("Time").innerText = Time;
        }

    </script>
    <style>
        @media only screen and (min-width : 950px) {
        }


        /*Large screen display 110*/
        @media only screen and (min-width: 1024px) and (max-width:1120px) {
        }

        /*Desktop display 100*/
        @media only screen and (min-width : 1224px) {
            .img-fluid {
                height: 60px !important;
            }

            .sp1 {
                padding: 12px 0px 21px 0px;
                color: #0268ba;
                text-shadow: 0px 1px 0px #f1ebeb, 0px 2px 0px #c7c7c7, 0px 3px 0px #e2e2e2, 0px 4px 0px #b9b6b6, 0px 5px 0px #a5a2a2, 0px 6px 0px #bdb7b7, 0px 7px 0px #777575, 0px 8px 7px #859ed4;
                font-size: 1.5rem !important;
                font-family: 'Arial Rounded MT';
                font-weight: 800;
                text-transform: uppercase;
                display: flex;
                align-items: center;
                justify-content: center;
            }

            .sp2 {
                padding: 10px;
                font-size: 1.0rem !important;
                font-family: 'Arial Rounded MT';
                height: 2em;
                display: flex;
                align-items: center;
                justify-content: center;
                color: #3278be;
            }

            .sp3 {
                padding: 12px 52px 21px 2px;
                font-size: 2.0rem !important;
                font-weight: 600;
                text-transform: uppercase;
                display: flex;
                align-items: center;
                justify-content: center;
                -webkit-background-clip: text;
                color: #203e9e;
                font-family: 'Roboto', sans-serif;
            }

            .sp4 {
                padding: -12px 52px 21px 2px;
                font-size: 1.0rem !important;
                font-weight: 400;
                text-transform: uppercase;
                display: flex;
                align-items: center;
                justify-content: center;
                -webkit-background-clip: text;
                font-family: 'Roboto', sans-serif;
            }

            .Status {
                border: 3px solid gray;
                width: 621px;
                max-height: 300px;
                min-height: 100px;
                margin-left: 18px;
                margin-right: 13px;
                padding-top: 0rem;
                border-radius: 10px;
                background-color: white;
                display: inline-block;
                text-align: left;
            }

            .EndStatus {
                border: 3px solid gray;
                width: 621px;
                max-height: 300px;
                min-height: 90px;
                margin-left: 18px;
                margin-right: 13px;
                padding-top: 0rem;
                border-radius: 10px;
                background-color: white;
                display: inline-block;
                text-align: left;
            }

            .AlreadyEndStatus {
                border: 3px solid gray;
                width: 621px;
                max-height: 300px;
                min-height: 90px;
                margin-left: 18px;
                margin-right: 13px;
                padding-top: 0rem;
                border-radius: 10px;
                background-color: white;
                display: inline-block;
                text-align: left;
            }

            .gvCustom {
            }


                .gvCustom .gvHead {
                    font-size: 15px;
                    color: white;
                    background-color: #124a79;
                }

                .gvCustom .gvRow {
                    color: black;
                    background-color: #f3f3f3;
                }

                /***  Gridview Page Style  **/
                .gvCustom .gvPage {
                    text-align: left;
                }

                    .gvCustom .gvPage a, .gvPage span {
                        display: block;
                        height: 15px;
                        width: 20px;
                        font-weight: bold;
                        text-align: center;
                        text-decoration: none;
                        margin-left: 5px;
                    }

                    .gvCustom .gvPage a {
                        background-color: #f5f5f5;
                        color: #008230;
                        border: 1px solid White;
                    }

                    .gvCustom .gvPage span {
                        background-color: #124a79;
                        color: White;
                        border: 1px solid White;
                    }

            .ImgDisplay {
                padding-left: 2.5rem;
            }

            .CountStart {
                padding-left: 2rem;
                font-size: 25px;
                float: left;
            }

            .CountEnd {
                padding-left: 2rem;
                font-size: 25px;
                float: right;
            }

            .ImgStart {
                padding-left: 2.5rem;
            }
        }


        /*Desktop display 90*/
        @media only screen and (min-width : 1324px) {
            .img-fluid {
                height: 60px !important;
            }

            .sp1 {
                padding: 12px 0px 21px 0px;
                color: #0268ba;
                text-shadow: 0px 1px 0px #f1ebeb, 0px 2px 0px #c7c7c7, 0px 3px 0px #e2e2e2, 0px 4px 0px #b9b6b6, 0px 5px 0px #a5a2a2, 0px 6px 0px #bdb7b7, 0px 7px 0px #777575, 0px 8px 7px #859ed4;
                font-size: 1.5rem !important;
                font-family: 'Arial Rounded MT';
                font-weight: 800;
                text-transform: uppercase;
                display: flex;
                align-items: center;
                justify-content: center;
            }

            .sp2 {
                padding: 10px;
                font-size: 1.0rem !important;
                font-family: 'Arial Rounded MT';
                height: 2em;
                display: flex;
                align-items: center;
                justify-content: center;
                color: #3278be;
            }

            .sp3 {
                padding: 12px 52px 21px 2px;
                font-size: 2.5rem !important;
                font-weight: 600;
                text-transform: uppercase;
                display: flex;
                align-items: center;
                justify-content: center;
                -webkit-background-clip: text;
                color: #203e9e;
                font-family: 'Roboto', sans-serif;
            }

            .sp4 {
                padding: -12px 52px 21px 2px;
                font-size: 1.5rem !important;
                font-weight: 400;
                text-transform: uppercase;
                display: flex;
                align-items: center;
                justify-content: center;
                -webkit-background-clip: text;
                font-family: 'Roboto', sans-serif;
            }

            .Status {
                border: 3px solid gray;
                width: 621px;
                max-height: 300px;
                min-height: 80px;
                margin-left: 18px;
                margin-right: 13px;
                padding-top: 0rem;
                border-radius: 10px;
                background-color: white;
                display: inline-block;
                text-align: left;
            }

            .EndStatus {
                border: 3px solid gray;
                width: 621px;
                max-height: 300px;
                min-height: 60px;
                margin-left: 18px;
                margin-right: 13px;
                padding-top: 0rem;
                border-radius: 10px;
                background-color: white;
                display: inline-block;
                text-align: left;
            }

            .AlreadyEndStatus {
                border: 3px solid gray;
                width: 621px;
                max-height: 300px;
                min-height: 60px;
                margin-left: 18px;
                margin-right: 13px;
                padding-top: 0rem;
                border-radius: 10px;
                background-color: white;
                display: inline-block;
                text-align: left;
            }

            .gvCustom {
            }


                .gvCustom .gvHead {
                    font-size: 20px;
                    color: white;
                    background-color: #124a79;
                }

                .gvCustom .gvRow {
                    color: black;
                    background-color: #f3f3f3;
                }

            .ImgDisplay {
                padding-left: 0.5rem;
            }

            .CountStart {
                padding-left: 2rem;
                font-size: 25px;
                float: left;
            }

            .CountEnd {
                padding-left: 2rem;
                font-size: 25px;
                float: right;
            }

            .ImgStart {
                padding-right: 2.5rem;
            }
        }



        @media only screen and (min-width : 1424px) {
        }


        /*Desktop display 80 lap Top*/
        @media only screen and (min-width : 1524px) {
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            //   Function counts down from 1 minute, display turns orange at 20 seconds and red at 5 seconds.
            var countdownTimer = {
                init: function () {
                    this.cacheDom();
                    this.render();
                },
                cacheDom: function () {
                    this.$el = $('.countdown');
                    this.$time = this.$el.find('.countdown__time');
                    this.$reset = this.$el.find('.countdown__reset');
                },
                // bindEvents: function() {
                //   this.$reset.on('click', this.resetTimer.bind(this));
                // },
                render: function () {
                    var totalTime = 5,
                        display = this.$time;
                    this.startTimer(totalTime, display);
                    this.$time.removeClass('countdown__time--red');
                    this.$time.removeClass('countdown__time--green');
                },
                startTimer: function (duration, display, icon) {
                    var timer = duration, minutes, seconds;
                    var interval = setInterval(function () {
                        minutes = parseInt(timer / 60, 10);
                        seconds = parseInt(timer % 60, 10);
                        minutes = minutes < 10 ? '0' + minutes : minutes;
                        seconds = seconds < 10 ? '0' + seconds : seconds;
                        display.text(minutes + ':' + seconds);
                        if (--timer < 0) {
                            clearInterval(interval);
                        };
                        if (timer <= 5) {
                            display.addClass('countdown__time--green')
                        };
                        if (timer < 2) {
                            display.addClass('countdown__time--red')
                        };
                    }, 1000);
                    //this.$reset.on('click', function () {
                    //    clearInterval(interval);
                    //    countdownTimer.render();
                    //});
                },
            };
            countdownTimer.init();
        });
    </script>
</head>
<body onload="StartBox()" onclick="StartBox()" style="height: 1000px; overflow: hidden;">

    <form id="bdycolor" runat="server">
        <asp:ScriptManager runat="server">
        </asp:ScriptManager>
        <div style="top: 0; z-index: 99999; background-color: white">
            <div class="row m-0">
                <div class="col-sm-4 col-xs-12">
                    <img class="img-fluid" src="images/TTDCLogo.svg" alt="Boating" style="width: 138px; height: 76px !important;" />
                </div>
                <div class="col-sm-4 col-xs-12">
                    <span class="sp2">Welcome &nbsp&nbsp
                        <asp:Label ID="lblUserName" runat="server" Font-Bold="true"></asp:Label>
                    </span>
                    <span class="sp2">
                        <asp:Label ID="lblBoatHouse" runat="server" Font-Bold="true"></asp:Label>
                    </span>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <span class="sp1">Boating System</span>
                </div>
                <div class="col-sm-1 col-xs-12" style="padding-top: 1rem;">
                    <asp:ImageButton ID="ImageHome" ImageUrl="~/images/home.png" runat="server" OnClick="ImageHome_Click" />
                </div>
            </div>
        </div>
        <div class="row" id="divFullPage" runat="server" style="width: 100%">
            <div class="col-md-3 col-sm-3">
                <h3 id="Time" class="sp4" style="padding-top: 1.5rem; padding-left: 1rem; float: left; display: inline; font-size: 20px;"></h3>
            </div>
            <div class="col-md-6 col-sm-6" style="text-align: center;">
                <h5 class="sp3" style="padding-top: 0.5rem; text-align: center; padding-left: 4.5rem; display: inline; font-size: 25px;">Trip Sheet </h5>
                <a href="IndividualSmartTripSheetWeb.aspx" alt="Individual Trip Sheet" class="tooltipDemo" style="margin-top: 25px; margin-left: 5px">
                    <asp:CheckBox runat="server" Style="zoom: 1.9;" Width="17px" Height="21px" ID="ChkIndTrpSht" onchange="showHourGlass();"
                        OnCheckedChanged="ChkIndTrpSht_CheckedChanged" AutoPostBack="true" /><b style="font-size: xx-large; color: black;">INDIVIDUAL</b>
                </a>
            </div>
            <div class="col-md-3 col-sm-3">
                <h3 id="Date" class="sp4" style="padding-top: 0.5rem; padding-left: 0rem; float: right; display: inline; font-size: 20px;"></h3>
            </div>
        </div>
        <div class="row" style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;">
            <div class="col-xs-12">
                <asp:TextBox ID="txtStartDetails" runat="server" CssClass="BarCodeTextStart"
                    placeholder="Scan Ticket QRCode" Font-Bold="true" AutoComplete="off"
                    Font-Size="40px" AutoPostBack="true" OnTextChanged="txtStartDetails_TextChanged" Style="display: inline-block"></asp:TextBox>
            </div>
            <div class="col-xs-12">
                <asp:TextBox ID="txtRowerDetails" runat="server" CssClass="BarCodeTextRower"
                    placeholder="Scan Rower QRCode" Font-Bold="true" AutoComplete="off"
                    Font-Size="40px" AutoPostBack="true" OnTextChanged="txtRowerDetails_TextChanged" Style="display: none"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 col-md-6 col-sm-6 CountStart">
                <asp:Label ID="lblAlreadyStartedCount" runat="server" Font-Bold="true" Text="34" Style="padding-left: 4px;" ForeColor="#124a79"></asp:Label>
            </div>
            <div class="col-xs-12 col-md-6 col-sm-6 CountEnd">
                <asp:Label ID="lblAlreadyEndedCount" runat="server" Font-Bold="true" Style="float: right; padding-right: 20px;" Text="34" ForeColor="#124a79"></asp:Label>
            </div>
        </div>

        <div id="divFinalStatus" runat="server" style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem; padding-bottom: 1rem; min-height: 750px; max-height: 1000px;">
            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="Status">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Booking Id " Font-Bold="true" Font-Size="40px">
                                </asp:Label>
                                <asp:Label ID="lblFinalBookingId" runat="server" CssClass="blink" Font-Bold="true" Font-Size="40px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="Status">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Booking Pin " Font-Bold="true" Font-Size="40px">
                                </asp:Label>
                                <asp:Label ID="lblFinalBookingPin" runat="server" CssClass="blink" Font-Bold="true" Font-Size="40px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="Status">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Duration(Mins) " Font-Bold="true" Font-Size="40px">                                     
                                </asp:Label>
                                <asp:Label ID="lblFinalBookingDuration" runat="server" CssClass="blink" Font-Bold="true" Font-Size="40px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="Status">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Start Time " Font-Bold="true" Font-Size="40px">
                                </asp:Label>
                                <asp:Label ID="lblFinalStartTime" runat="server" CssClass="blink" Font-Bold="true" Font-Size="40px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="Status">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Boat Type " Font-Bold="true" Font-Size="40px">
                                </asp:Label>
                                <asp:Label ID="lblFinalBoatType" runat="server" CssClass="blink" Font-Bold="true" Font-Size="40px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="Status">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Boat Seater " Font-Bold="true" Font-Size="40px">
                                </asp:Label>
                                <asp:Label ID="lblFinalBoatSeater" runat="server" CssClass="blink" Font-Bold="true" Font-Size="40px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divFinalRower" class="row" runat="server">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="Status">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Rower Name " Font-Bold="true" Font-Size="40px">
                                </asp:Label>
                                <asp:Label ID="lblFinalRowerName" runat="server" CssClass="blink" Font-Bold="true" Font-Size="40px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblStartStatus" runat="server" CssClass="lblblink" Font-Bold="true" ForeColor="White" Font-Size="75px" Style="text-align: center;"></asp:Label>
            <asp:Image ID="ImgBoatType" runat="server" CssClass="ImgStart" />
        </div>

        <div id="divFinalEndStatus" runat="server" style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem; padding-bottom: 1rem; min-height: 750px; max-height: 1000px;">
            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Booking Id " Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblEndBookingId" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Booking Pin" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblEndBookingpin" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Start Time " Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblEndStartTime" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="End Time " Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblFinalEndTime" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Duration(Mins) " Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblEndDuration" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Travelled Mins " Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblEndTravelledMins" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Boat Type " Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblEndBoatType" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Boat Seater " Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblEndBoatSeater" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divEndRower" class="row" runat="server">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="EndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Rower Name " Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblEndRowerName" runat="server" CssClass="blinkend" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblEndStatus" runat="server" CssClass="lblblink" Font-Bold="true" ForeColor="White" Font-Size="75px" Style="text-align: center;"></asp:Label>
            <asp:Image ID="imgEnd" runat="server" ImageUrl="~/images/Trip.png" CssClass="ImgDisplay" />
        </div>

        <div id="divAlreadyEndStatus" runat="server" style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem; padding-bottom: 1rem; min-height: 750px; max-height: 1000px;">
            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="AlreadyEndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Booking Id" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlendBookingId" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="AlreadyEndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Booking Pin" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlendBookingPin" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="AlreadyEndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Start Time" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlendStartTime" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="AlreadyEndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="End Time" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlendEndTime" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="AlreadyEndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Duration(Mins)" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlendDuartion" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="AlreadyEndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Travelled Mins" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlendTravelledMins" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="AlreadyEndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Boat Type" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlreadyBoatType" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="AlreadyEndStatus" id="wrapper">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Boat Seater" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlreadyBoatSeater" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divAlreadyRower" class="row" runat="server">
                <div style="text-align: center; display: inline-block; width: 100%; padding-top: 1rem;" runat="server">
                    <div class="AlreadyEndStatus">
                        <div class="col-sm-12 col-xs-12">
                            <div class="slide">
                                <asp:Label runat="server" Text="Rower Name" Font-Bold="true" Font-Size="35px">
                                </asp:Label>
                                <asp:Label ID="lblAlendRowerName" runat="server" CssClass="blinkAlready" Font-Bold="true" Font-Size="35px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Label ID="lblAlreadyStatus" runat="server" CssClass="lblblink" Font-Bold="true" ForeColor="White" Font-Size="75px" Style="float: center;"></asp:Label>
        </div>

        <%--New Changes on 2021-04-21--%>

        <div class="table-div" id="divRowerInfoMsg" runat="server" visible="false">
            <div class="table-responsive">
                <div style="margin-left: 0; text-align: center;">
                    <asp:Label ID="lblRowerInfoMsg" runat="server" ForeColor="Red" Width="100%" CssClass="blink" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                </div>
            </div>
        </div>
        <%--New Changes end--%>


        <div class="table-div" id="divTripStarted" runat="server" style="background-color: white; padding-left: 2rem; width: 100%; text-align: center; padding-right: 2rem;" visible="false">
            <h4 id="header" runat="server" style="color: palevioletred; font-size: 30px;"><b>Trip Started List</b></h4>
            <div class="table-responsive">
                <asp:GridView ID="GvStartedList" runat="server" CssClass="gvCustom" Width="100%"
                    AutoGenerateColumns="False" DataKeyNames="BoatReferenceNo">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Pin No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Serial" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingSerial" runat="server" Text='<%# Bind("BookingSerial") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boarding Time" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoardingTime" runat="server" Text='<%# Bind("BoardingTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Duration (Mins)" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("TripStartTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>


        <div class="table-div" id="divGridStart" runat="server" style="background-color: white; padding-top: 1rem; padding-left: 2rem; width: 100%; text-align: center; padding-right: 2rem;">
            <div class="table-responsive">
                <h4 id="headerend" runat="server" style="color: palevioletred; font-size: 30px"><b>Trip Ended List</b></h4>
                <asp:GridView ID="gvTripSheetSettelementEnd" runat="server" CssClass="gvCustom" Width="100%"
                    AutoGenerateColumns="False" DataKeyNames="BoatReferenceNo">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Reference No" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Serial" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingSerial" runat="server" Text='<%# Bind("BookingSerial") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boarding Time" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoardingTime" runat="server" Text='<%# Bind("BoardingTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Pin No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Duration (Mins)" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("TripStartTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trip End Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("TripEndTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Travelled Minutes" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTraveledMins" runat="server" Text='<%# Eval("TraveledTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>

        <div class="table-div" id="divExtendedRower" runat="server" style="background-color: white; padding-top: 1rem; padding-left: 2rem; width: 100%; text-align: center; padding-right: 2rem; display: none;">
            <div class="table-responsive">
                <h4 id="h1" runat="server" style="color: palevioletred; font-size: 30px"><b>Extended Rower List</b></h4>
                <asp:GridView ID="gvExtendedRower" runat="server" CssClass="gvCustom" Width="100%"
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rower Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerId" runat="server" Text='<%# Bind("RowerId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rower Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="seater Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Extra Traveled Min." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTravelledMin" runat="server" Text='<%# Bind("TraveledMinutes") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Minimum & Grace Time" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMinGraceTime" runat="server" Text='<%# Bind("MinGraceTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="18px" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>

        <ajax:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="pnlTrip" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
        <ajax:ModalPopupExtender ID="MpeTrip" runat="server" BehaviorID="MpeTrip" TargetControlID="HiddenField1" PopupControlID="pnlTrip"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="pnlTrip" runat="server" CssClass="Msgg">
            <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
                <div class="modal-content" style="width: 700px; height: 230px;">
                    <div class="modal-body" style="text-align: center; padding-top: 3rem;">
                        <asp:Label ID="lblStartResponse" runat="server" Font-Bold="true" ForeColor="#5e18b5" Font-Size="35px"></asp:Label>
                        <asp:Label ID="lblRowerResponse" runat="server" Font-Bold="true" ForeColor="#5e18b5" Font-Size="35px" Visible="false"></asp:Label>
                        <div class="module">
                            <div class="countdown">
                                <div class="countdown__time">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>

        <asp:HiddenField ID="hfBookingId" runat="server" />
        <asp:HiddenField ID="hfCreatedBy" runat="server" />
        <asp:HiddenField ID="hfBoathouseId" runat="server" />
        <asp:HiddenField ID="hfBoathouseName" runat="server" />
        <asp:HiddenField ID="hfBarcodePin" runat="server" />
        <asp:HiddenField ID="hfBarcodePinRowerId" runat="server" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hfBoatTypeId" runat="server" />
        <asp:HiddenField ID="hfBoatSeaterId" runat="server" />
        <asp:HiddenField ID="hfBookingDuration" runat="server" />
        <asp:HiddenField ID="hfBarcodePinRowerName" runat="server" />
        <div id="HourGlass" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8; display: none">
            <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
                <img src="images/hourglass.gif" width="100px" height="100px" />
        </div>
        <%-- Newly implemented for CSRF Validation--%>
        <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
        <%-- Newly implemented for CSRF Validation--%>
    </form>
</body>
</html>
