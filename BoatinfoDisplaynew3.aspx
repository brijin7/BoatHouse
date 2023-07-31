<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BoatinfoDisplaynew3.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <meta name="viewport" content="width=device-width, initial-scale=1.0">
   
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
   
    <link href="http://cdn.jsdelivr.net/jcarousel/0.2.8/skins/tango/skin.css" rel="Stylesheet" />
    <script type="text/javascript">
        $(function () {
            $('#mycarousel').jcarousel();
        });
    </script>
    <script type="text/javascript">

</script>

    <script type="text/javascript">
  $(document).ready(function () {
            BoatListCount();
        var aaa   =  $('#<%=txtCarouselTime.ClientID%>').val();
            
             setTimeout(function () {

                 location.reload();
             }, aaa);
       });
    </script>
    <script type="text/javascript">
        function BoatListCount() {
            var baseUrl = '<%=HttpContext.Current.Session["BaseUrl"]%>';
            var BoatHouseId = '<%=HttpContext.Current.Session["BoatHouseId"]%>';

            var Url22 = baseUrl + "BoatListDisplay/BoatType";

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: Url22,
                data: JSON.stringify({
                    "BoatHouseId": BoatHouseId
                }),
                dataType: "json",
                success: function (data) {
                    var count = data.Response.length;
                    if (count <= 3) { $('#<%=txtCarouselTime.ClientID%>').val(15000) }
                    if (count == 3) { $('#<%=txtCarouselTime.ClientID%>').val(15000) }
                    if (count == 4) { $('#<%=txtCarouselTime.ClientID%>').val(25000) }
                    if (count == 5) { $('#<%=txtCarouselTime.ClientID%>').val(25000) }
                    if (count == 6) { $('#<%=txtCarouselTime.ClientID%>').val(25000) }
                    if (count == 7) { $('#<%=txtCarouselTime.ClientID%>').val(35000) }
                    if (count == 8) { $('#<%=txtCarouselTime.ClientID%>').val(35000) }
                    if (count == 9) { $('#<%=txtCarouselTime.ClientID%>').val(35000) }
                    if (count == 10) { $('#<%=txtCarouselTime.ClientID%>').val(45000) }
                    if (count == 11) { $('#<%=txtCarouselTime.ClientID%>').val(45000) }
                    if (count == 12) { $('#<%=txtCarouselTime.ClientID%>').val(45000) }
                    if (count == 13) { $('#<%=txtCarouselTime.ClientID%>').val(55000) }
                    if (count == 14) { $('#<%=txtCarouselTime.ClientID%>').val(55000) }
                    if (count == 15) { $('#<%=txtCarouselTime.ClientID%>').val(55000) }
                    if (count == 16) { $('#<%=txtCarouselTime.ClientID%>').val(65000) }
                    if (count == 17) { $('#<%=txtCarouselTime.ClientID%>').val(65000) }
                    if (count == 18) { $('#<%=txtCarouselTime.ClientID%>').val(65000) }
                    if (count == 19) { $('#<%=txtCarouselTime.ClientID%>').val(75000) }
                    if (count == 20) { $('#<%=txtCarouselTime.ClientID%>').val(75000) }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                }
            });
        }
    </script>

    <style>
        .jcarousel-skin-tango .jcarousel-container {
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            border-radius: 10px;
            background: #ffffff;
            border: 1px solid #ffffff;
            margin-top: -19px;
        }

        .jcarousel-skin-tango .jcarousel-direction-rtl {
            direction: rtl;
        }

        .jcarousel-skin-tango .jcarousel-container-horizontal {
            width: 1163px;
            height: 100px;
            padding: 20px 40px;
           
        }

        .jcarousel-skin-tango .jcarousel-container-vertical {
            width: 75px;
            height: 245px;
            padding: 40px 20px;
        }

        .jcarousel-skin-tango .jcarousel-clip {
            overflow: hidden;
        }


        .jcarousel-skin-tango .jcarousel-clip-horizontal {
            position: relative;
            width: 1160px;
            height: 900px;
        }

        .jcarousel-skin-tango .jcarousel-clip-vertical {
            width: 75px;
            height: 245px;
        }

        .jcarousel-skin-tango .jcarousel-item {
            width: 379px;
            height: 1000px;
        }

        .jcarousel-skin-tango .jcarousel-item-horizontal {
            margin-left: 0;
            margin-right: 10px;
        }

        .jcarousel-skin-tango .jcarousel-direction-rtl .jcarousel-item-horizontal {
            margin-left: 10px;
            margin-right: 0;
        }

        .jcarousel-skin-tango .jcarousel-item-vertical {
            margin-bottom: 10px;
        }

        .jcarousel-skin-tango .jcarousel-item-placeholder {
            background: #fff;
            color: #000;
        }

        /**
 *  Horizontal Buttons
 */
        .jcarousel-skin-tango .jcarousel-next-horizontal {
            position: absolute;
            top: 43px;
            right: 5px;
            width: 32px;
            height: 0px;
            cursor: pointer;
            background-color: #f0f6f9;
            display: none;
            /*background: transparent url(next-horizontal.png) no-repeat 0 0;*/
        }

        .jcarousel-skin-tango .jcarousel-direction-rtl .jcarousel-next-horizontal {
            left: 5px;
            right: auto;
            background-image: url(prev-horizontal.png);
        }

        .jcarousel-skin-tango .jcarousel-next-horizontal:hover,
        .jcarousel-skin-tango .jcarousel-next-horizontal:focus {
            background-position: -32px 0;
        }

        .jcarousel-skin-tango .jcarousel-next-horizontal:active {
            background-position: -64px 0;
        }

        .jcarousel-skin-tango .jcarousel-next-disabled-horizontal,
        .jcarousel-skin-tango .jcarousel-next-disabled-horizontal:hover,
        .jcarousel-skin-tango .jcarousel-next-disabled-horizontal:focus,
        .jcarousel-skin-tango .jcarousel-next-disabled-horizontal:active {
            cursor: default;
            background-position: -96px 0;
        }

        .jcarousel-skin-tango .jcarousel-prev-horizontal {
            position: absolute;
            top: 43px;
            left: 5px;
            width: 32px;
            height: 0px;
            cursor: pointer;
            background-color: #f0f6f9;
            display: none;
            /*background: transparent url(prev-horizontal.png) no-repeat 0 0;*/
        }

        .jcarousel-skin-tango .jcarousel-direction-rtl .jcarousel-prev-horizontal {
            left: auto;
            right: 5px;
            background-image: url(next-horizontal.png);
        }

        .jcarousel-skin-tango .jcarousel-prev-horizontal:hover,
        .jcarousel-skin-tango .jcarousel-prev-horizontal:focus {
            background-position: -32px 0;
        }

        .jcarousel-skin-tango .jcarousel-prev-horizontal:active {
            background-position: -64px 0;
        }

        .jcarousel-skin-tango .jcarousel-prev-disabled-horizontal,
        .jcarousel-skin-tango .jcarousel-prev-disabled-horizontal:hover,
        .jcarousel-skin-tango .jcarousel-prev-disabled-horizontal:focus,
        .jcarousel-skin-tango .jcarousel-prev-disabled-horizontal:active {
            cursor: default;
            background-position: -96px 0;
        }

        /**
 *  Vertical Buttons
 */
        .jcarousel-skin-tango .jcarousel-next-vertical {
            position: absolute;
            bottom: 5px;
            left: 43px;
            width: 32px;
            height: 0px;
            cursor: pointer;
            background: transparent url(next-vertical.png) no-repeat 0 0;
        }

            .jcarousel-skin-tango .jcarousel-next-vertical:hover,
            .jcarousel-skin-tango .jcarousel-next-vertical:focus {
                background-position: 0 -32px;
            }

            .jcarousel-skin-tango .jcarousel-next-vertical:active {
                background-position: 0 -64px;
            }

        .jcarousel-skin-tango .jcarousel-next-disabled-vertical,
        .jcarousel-skin-tango .jcarousel-next-disabled-vertical:hover,
        .jcarousel-skin-tango .jcarousel-next-disabled-vertical:focus,
        .jcarousel-skin-tango .jcarousel-next-disabled-vertical:active {
            cursor: default;
            background-position: 0 -96px;
        }

        .jcarousel-skin-tango .jcarousel-prev-vertical {
            position: absolute;
            top: 5px;
            left: 43px;
            width: 32px;
            height: 0px;
            cursor: pointer;
            background: transparent url(prev-vertical.png) no-repeat 0 0;
        }

            .jcarousel-skin-tango .jcarousel-prev-vertical:hover,
            .jcarousel-skin-tango .jcarousel-prev-vertical:focus {
                background-position: 0 -32px;
            }

            .jcarousel-skin-tango .jcarousel-prev-vertical:active {
                background-position: 0 -64px;
            }

        .jcarousel-skin-tango .jcarousel-prev-disabled-vertical,
        .jcarousel-skin-tango .jcarousel-prev-disabled-vertical:hover,
        .jcarousel-skin-tango .jcarousel-prev-disabled-vertical:focus,
        .jcarousel-skin-tango .jcarousel-prev-disabled-vertical:active {
            cursor: default;
            background-position: 0 -96px;
        }
    </style>
    <style>
        .dtlBoatType {
            margin-bottom: 5px;
            margin-top: 3px;
            margin-left: 3px !important;
            margin-right: 3px !important;
            box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
            overflow: hidden;
            transition: all .5s ease;
            border-radius: 10px;
            max-width: 200%;
            background-color: white;
        }

        .dtlDepaNor {
            margin-bottom: 5px;
            margin-top: 3px;
            margin-left: 3px !important;
            margin-right: 3px !important;
            overflow: hidden;
            transition: all .5s ease;
            border-radius: 10px;
            max-width: 200%;
            background-color: white;
        }

        .dtlDepaPre {
            margin-bottom: 5px;
            margin-top: 3px;
            margin-left: 3px !important;
            margin-right: 3px !important;
            overflow: hidden;
            transition: all .5s ease;
            border-radius: 10px;
            max-width: 200%;
            background-color: white;
        }


        .dtlAvaNor {
            margin-bottom: 5px;
            margin-top: 3px;
            margin-left: 3px !important;
            margin-right: 3px !important;
            
            overflow: hidden;
            transition: all .5s ease;
           
            max-width: 200%;
            background-color:white;
        }

        .dtlAvaPre {
            margin-bottom: 5px;
            margin-top: 3px;
            margin-left: 3px !important;
            margin-right: 3px !important;
            
            overflow: hidden;
            transition: all .5s ease;
           
            max-width: 200%;
            background-color: white;
        }

        table {
            font-family: arial, sans-serif;
            border-collapse: collapse;
            width: 100%;
            margin-left: 2px;
        }

        td {
            text-align: center;
            max-width: 3px;
            min-width: 3px;
            border-bottom: 1px solid #999;
            font-family: inherit;
        }

        th {
            border: 1px solid #dddddd;
            max-width: 10px;
            text-align: center;
            padding: 0px;
            min-width: 10px;
            font-size: 10px;
            font-weight: 850;
            color: white;
            background-color: #7cb5ec;
            font-family: inherit;
        }

        tdpre {
            text-align: center;
            padding: 12px;
            max-width: 20px;
            min-width: 20px;
            border-bottom: 1px solid #999;
        }


        thpre {
            border: 1px solid #dddddd;
            text-align: center;
            padding: 12px;
            max-width: 20px;
            min-width: 20px;
            font-size: 54px;
            font-weight: 850;
            color: white;
            background-color: lightgoldenrodyellow;
        }

        .CSlblBoatHouse {
            text-align: center;
            padding-left: 13px;
            color: #3278be;
            /* /font-family: Rockwell;*/
            /*font-family: Arial, Helvetica, sans-serif;*/
            /*font-family: "Lucida Console", "Courier New", monospace;*/
            /*font-family : Comic Sans MS, Comic Sans, cursive;*/
            /*Copperplate, Papyrus, fantasy   Ink Free*/
            font-family: inherit;
        }
    </style>
   

    <style>
        .sp1 {
            margin-top: -38px;
            font-size: 1.2rem !important;
            font-family: inherit;
            font-weight: bold;
            text-transform: uppercase;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #3278be;
        }


        .sp2 {
            padding: 12px 52px 21px 2px;
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

        .footer {
            position: fixed;
            left: 0;
            bottom: 0;
            width: 100%;
            text-align: center;
        }
    </style>

    <style>
        @-webkit-keyframes track-in-out {
            0% {
                letter-spacing: 1em;
                opacity: 0;
            }

            20% {
                letter-spacing: normal;
                opacity: 1;
            }
        }

        #myText {
            font-size:30px;
            color: #ffffff;
            margin-bottom: 0px;
            word-spacing: 10px;
            text-shadow: 2px 2px 5px grey;
            width: 100%;
            text-align: center;
            font-family: cursive;
            animation-name: track-in-out;
            animation-duration: 7s;
            animation-iteration-count: infinite;
         
        }



        /*body {
            background: white url("images/BTTDCImg.jpg") no-repeat fixed center;
        }*/
        .ClassBoatHouse {
            color: #3278be;
            font-size: 1.2rem !important;
            font-family: inherit;
            text-transform: uppercase;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-top: -62px;
            font-weight: bold;
        }

        .DisDate {
            float: right;
            color: green;
            font-size: 1.5rem !important;
            margin-top: -67px;
        }

        .DisTime {
            color: green;
            font-size: 1.5rem !important;
            margin-top: 12px;
            margin-left: 25px;
        }

        .footer {
            position: fixed;
            left: 0;
            bottom: 0;
            width: 100%;
            text-align: center;
        }
    </style>

    <script lang="javascript" type="text/javascript">
        var myVar = setInterval(myTimer, 0);
        const monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];
        const DayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
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
        /* comman*/

           @media only screen and (min-width: 1030px) 
            {
            .jcarousel-skin-tango .jcarousel-container-horizontal {
                width:913px;
                height: 100px;
                padding: 20px 40px;
                background-color: white;
            }

            .jcarousel-skin-tango .jcarousel-clip-horizontal {
                position: relative;
                width:916px;
                background-color: white;
                height: 670px;
            }

            .jcarousel-skin-tango .jcarousel-item {
                width:  306px;
                height: 669px;
            }

            .imgBoatGif {
                 max-width: 75px;
                min-width: 75px;
                min-height: 80px;
                max-height: 59px;
                margin-top: -69px;
                padding-left: 259px;
            }


            .dtlBoatType {
                margin-bottom: 5px;
                margin-top: 3px;
                margin-left: 3px !important;
                margin-right: 3px !important;
                box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
                overflow: hidden;
                transition: all .5s ease;
                border-radius: 10px;
                max-width: 200%;
                background-color: white;
                height: 633px;
            }

            .dtlDeparture {
               
                border-radius: 10px;
                padding-bottom: 2px;
                margin-bottom: 1px;
                margin-top: 2px;
            }

            th {
                border: 1px solid #dddddd;
                max-width: 10px;
                text-align: center;
                padding: 0px;
                min-width: 10px;
                font-size: 13px;
                font-weight: 850;
                color: white;
                background-color: #7cb5ec;
                font-family: inherit;
            }

            td {
                font-size: 13px;
                font-weight: bolder;
            }
            .img-fluid
            {
             width: 210px; 
             height: 53px !important;
            }
            }

        @media only screen and (min-width: 400px) and (max-width:1020px) {
            .jcarousel-skin-tango .jcarousel-container-horizontal {
                width:539px;
                height: 100px;
                padding: 20px 63px;
                background-color: white;
            }

            .jcarousel-skin-tango .jcarousel-clip-horizontal {
                position: relative;
                width: 415px;
                background-color: white;
                height:676px;
            }

            .jcarousel-skin-tango .jcarousel-item {
                width: 413px;

                height: 680px;
                background-color: white;
            }

            .imgBoatGif {
                width: 80px;
                margin-top: -87px;
                padding-left: 240px;
            }

            .dtlBoatType {
                margin-bottom: 5px;
                margin-top: 3px;
                margin-left: 3px !important;
                margin-right: 3px !important;
                box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
                overflow: hidden;
                transition: all .5s ease;
                border-radius: 10px;
                max-width: 100%;
                background-color: white;
                height: 670px;
            }

            .dtlDeparture {
               
                border-radius: 10px;
                padding-bottom: 2px;
                margin-bottom: 1px;
                margin-top: 2px;
            }
/**/
            th {
                border: 1px solid #dddddd;
                max-width: 10px;
                text-align: center;
                padding: 0px;
                min-width: 10px;
                font-size: 13px;
                font-weight: 850;
                color: white;
                background-color: #7cb5ec;
                font-family: inherit;
            }

            td {
                font-size: 13px;
                font-weight: bolder;
            }
             .ClassBoatHouse {
            color: #3278be;
            font-size: 0.8rem !important;
            font-family: inherit;
            text-transform: uppercase;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-top:-49px;
            font-weight: bold;
        }

        .DisDate {
            float: right;
            color: green;
            font-size:0.8rem !important;
            margin-top: -55px;
        }

        .DisTime {
            color: green;
            font-size: 0.8rem !important;
            margin-top:5px;
            margin-left:5px;
        }
         .sp1 {
            margin-top: -38px;
            font-size: 0.8rem !important;
            font-family: inherit;
            font-weight: bold;
            text-transform: uppercase;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #3278be;
         }
         .img-fluid
         {
             width: 100px; 
             height: 53px !important;
         }
        }

        /*Large screen display 110*/
        @media only screen and (min-width : 1024px) {
            .jcarousel-skin-tango .jcarousel-container-horizontal {
                width: 1051px;
                height: 100px;
                padding: 20px 40px;
                background-color: white;
            }

            .jcarousel-skin-tango .jcarousel-clip-horizontal {
                position: relative;
                width: 1064px;
                background-color: white;
                height: 670px;
            }

            .jcarousel-skin-tango .jcarousel-item {
                width: 345px;
                height: 807px;
            }

            .imgBoatGif {
                 max-width: 75px;
                min-width: 75px;
                min-height: 80px;
                max-height: 59px;
                margin-top: -69px;
                padding-left: 259px;
            }


            .dtlBoatType {
                margin-bottom: 5px;
                margin-top: 3px;
                margin-left: 3px !important;
                margin-right: 3px !important;
                box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
                overflow: hidden;
                transition: all .5s ease;
                border-radius: 10px;
                max-width: 200%;
                background-color: white;
                height: 660px;
            }

            .dtlDeparture {
               
                border-radius: 10px;
                padding-bottom: 2px;
                margin-bottom: 1px;
                margin-top: -19px;
            }

            th {
                border: 1px solid #dddddd;
                max-width: 10px;
                text-align: center;
                padding: 0px;
                min-width: 10px;
                font-size: 13px;
                font-weight: 850;
                color: white;
                background-color: #7cb5ec;
                font-family: inherit;
            }

            td {
                font-size: 13px;
                font-weight: bolder;
            }
            .img-fluid
         {
             width: 210px; 
             height: 53px !important;
         }
        }


        /*Desktop display 100*/
        @media only screen and (min-width : 1224px ) {
            .jcarousel-skin-tango .jcarousel-container-horizontal {
                width: 1165px;
                height: 100px;
                padding: 20px 40px;
                background-color: white;
            }

            .jcarousel-skin-tango .jcarousel-clip-horizontal {
                position: relative;
                width: 1189px;
                height: 710px;
                background-color: white;
            }

            .jcarousel-skin-tango .jcarousel-item {
                width: 390px;
                height: 810px;
            }

            .imgBoatGif {
                /*   width: 80px;*/
                max-width: 88px;
                min-width: 88px;
                min-height: 78px;
                max-height: 50px;
                margin-top: -69px;
                padding-left: 291px;
            }

            .dtlBoatType {
                margin-bottom: 5px;
                margin-top: 3px;
                margin-left: 3px !important;
                margin-right: 3px !important;
                box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
                overflow: hidden;
                transition: all .5s ease;
                max-width: 200%;
                background-color: white;
                height: 700px;
            }

            .dtlDeparture {
                
                border-radius: 10px;
                padding-bottom: 2px;
                margin-bottom: 1px;
                margin-top: -19px;
            }

            th {
                border: 1px solid #dddddd;
                max-width: 76px;
                text-align: center;
                padding: 0px;
                min-width: 57px;
                font-size: 15px;
                font-weight: 850;
                color: white;
                background-color: #7cb5ec;
                font-family: inherit;
            }

            td {
                font-size: 15px;
                font-weight: bolder;
            }
           .img-fluid
         {
             width: 210px; 
             height: 53px !important;
         }

        }
        /*Desktop display 90*/
        @media only screen and (min-width : 1324px) {
            .jcarousel-skin-tango .jcarousel-container-horizontal {
                width:1281px;
                height: 100px;
                padding: 2px 11px;
                background-color: white;
            }

            .jcarousel-skin-tango .jcarousel-clip-horizontal {
                position: relative;
                width: 1318px;
                height: 624px;
                background-color: white;
            }

            .jcarousel-skin-tango .jcarousel-item {
                width: 430px;
                height: 847px;
            }

            .imgBoatGif {
                /*   width: 80px;*/
               max-width:167px;
                min-width: 113px;
                min-height: 79px;
                max-height: 39px;
                margin-top: -90px;
                padding-left: 295px;
            }


            .dtlBoatType {
                margin-bottom: 5px;
                margin-top: 3px;
                margin-left: 3px !important;
                margin-right: 3px !important;
                box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
                overflow: hidden;
                transition: all .5s ease;
                max-width: 100%;
                background-color: white;
                height: 600px;
            }

            .dtlDeparture {
                
                border-radius: 10px;
                padding-bottom: 2px;
                margin-bottom: 1px;
                margin-top:-19px;
            }

            th {
                border: 1px solid #dddddd;
                max-width: 10px;
                text-align: center;
                padding: 0px;
                min-width: 10px;
                font-size: 15px;
                font-weight: 850;
                color: white;
                background-color: #7cb5ec;
                font-family: inherit;
            }

            td {
                font-size: 17px;
                font-weight: bolder;
            }
            .img-fluid
         {
             width: 210px; 
             height: 53px !important;
         }

        }
       
       
    </style>

</head>
<body>
    <form id="form1" runat="server">

        <div>
            <div style="top: 0; z-index: 99999; background-color: white">
                <div class="row m-0">
                    <div class="col-sm-3 col-xs-12">
                        <img class="img-fluid" src="images/TTDCLogo.svg" alt="PayPre" />
                    </div>
                    <div class="col-sm-9 col-xs-12">
                        <span class="sp1" style="letter-spacing: 3px;">Boating Information Display</span>

                    </div>
                </div>
            </div>

           
            <div class="col-xs-12" runat="server" style="width: 100%">
                <div class="row">
                    <div class="col-xs-3 col-sm-3">
                        <h3 id="Time" class="DisTime"></h3>
                    </div>
                    <div class="col-xs-6 col-sm-6" style="text-align: center; padding-top: 1.5rem;">
                        <asp:Label CssClass="ClassBoatHouse" runat="server" ID="lblBoatHouseName"></asp:Label>
                    </div>
                    <div class="col-xs-3 col-sm-3">
                        <h3 id="Date" class="DisDate"></h3>
                    </div>
                </div>
            </div>
        </div>




      

        <div id="divDatalist" runat="server" style="vertical-align: top;">
            <ul id="mycarousel" class="jcarousel-skin-tango">

                <asp:Repeater ID="dtlBoat" runat="server" OnItemDataBound="dtlBoat_ItemDataBound1">
                    <ItemTemplate>
                        <li>
                            <%-- <asp:DataList ID="dtlBoat" runat="server" Width="200%" OnItemDataBound="dtlBoat_ItemDataBound" RepeatDirection="Horizontal">
                                        <ItemTemplate>--%>
                            <div>
                                <div class ="col-sm-6 col-xs-6" style="margin-top: 0 !important">
                                    <div class="row dtlBoatType" style="overflow: hidden;">

                                        <div class="col-sm-6 col-xs-12 ">
                                            <h2>
                                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Eval("BoatTypeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblBtBoatType" runat="server" Text='<%# Eval("BoatTypeName") %>'
                                                    CssClass="CSlblBoatHouse" Font-Bold="true"></asp:Label></h2>
                                        </div>
                                        <div class="col-sm-6 col-xs-6">

                                            <img src="images/BoatGifDisplay.gif" class="imgBoatGif" />
                                            <%-- <asp:Label ID="BoatImageLink" runat="server" src='<%# Eval("BoatImageLink") %>'
                                                   Class="imgBoatGif" ></asp:Label>--%>
                                            <%-- <img src= '<%# Eval("BoatImageLink") %>' class="imgBoatGif" runat="server" />--%>
                                           <%-- <asp:Image ID="lblBtBoatImgLink" runat="server" ImageUrl='<%# Eval("BoatImageLink") %>' class="imgBoatGif" />--%>

                                        </div>


                                        <div class="dtlDeparture">




                                            <div class="col-sm-12 pl-0 text-center btntbody dtlDepaNor  ">

                                                <asp:DataList ID="dtlBoatchild1" runat="server">
                                                    <HeaderTemplate>
                                                        <h3 style="margin: 0px; text-align: center; font-weight: bolder; color: #058984; font-family: inherit;">Normal Boat - Departure</h3>
                                                        <table>
                                                            <th>Booking Id</th>
                                                            <th>Pin No</th>
                                                            <%--<th>Boat Type</th>--%>
                                                            <th>Seater Type</th>
                                                            <th>Expected Time</th>
                                                        </table>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblNBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblNBookingPIN" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label></td>
                                                                <%--  <td>
                                                                        <asp:Label ID="lblNBoatType" runat="server" Text='<%# Eval("BoatTypeName") %>' ></asp:Label></td>--%>

                                                                <td>
                                                                    <asp:Label ID="lblNBoatSeat" runat="server" Text='<%# Eval("BoatSeaterName") %>'></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblNTripstartTime" runat="server" Text='<%# Eval("ExpectedTime") %>'></asp:Label></td>
                                                            </tr>

                                                        </table>

                                                    </ItemTemplate>


                                                </asp:DataList>
                                            </div>
                                            <div class="col-sm-12 pl-0 text-center btntbody dtlDepaPre  ">

                                                <asp:DataList ID="dtDataChild2" runat="server">
                                                    <HeaderTemplate>

                                                        <%-- <h3 style="font-size: 84px; text-align: center; font-weight: bolder;font-family:Verdana, Geneva, Tahoma, sans-serif; text-shadow: 0 1px 0 hsl(174,5%,80%), 0 2px 0 hsl(174,5%,75%), 0 3px 0 hsl(174,5%,70%), 0 4px 0 hsl(174,5%,66%), 0 5px 0 hsl(174,5%,64%), 0 6px 0 #0063cd, 0 7px 0 #205995, 0 8px 0 hsl(174deg 100% 38%), 0 0 5px rgba(0,0,0,.05), 0 1px 3px rgba(0,0,0,.2), 0 3px 5px rgba(0,0,0,.2), 0 5px 10px rgba(0,0,0,.2), 0 10px 10px rgba(0,0,0,.2), 0 20px 20px rgba(0,0,0,.3);
                                                           padding-top:20px;color:#bd2130;">PREMIUM</h3>--%>
                                                        <h3 style="margin: 0px; text-align: center; font-weight: bolder; font-family: inherit; color: #bd2130; margin-right: -2px; margin-left: -10px;">Premium Boat - Departure</h3>
                                                        <table>
                                                            <th style="background-color: goldenrod">Booking Id</th>
                                                            <th style="background-color: goldenrod">Pin No</th>
                                                            <%-- <th style="background-color:goldenrod">Boat Type</th>--%>
                                                            <th style="background-color: goldenrod">Seater Type</th>
                                                            <th style="background-color: goldenrod">Expected Time</th>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>

                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPBookingId" runat="server" Text='<%# Eval("BookingId") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPBookingPIN" runat="server" Text='<%# Eval("BookingPin") %>'></asp:Label>
                                                                </td>
                                                                <%-- <td>
                                                                        <asp:Label ID="lblPBoatType" runat="server" Text='<%# Eval("BoatTypeName") %>' ></asp:Label></td>--%>



                                                                <td>
                                                                    <asp:Label ID="lblPBoatSeat" runat="server" Text='<%# Eval("BoatSeaterName") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPTripstartTime" runat="server" Text='<%# Eval("ExpectedTime") %>'></asp:Label>
                                                                </td>
                                                            </tr>


                                                        </table>
                                                    </ItemTemplate>

                                                </asp:DataList>

                                            </div>


                                        </div>

                                    </div>
                                </div>
                            </div>

                            <%--  </ItemTemplate>
                                    </asp:DataList>--%>
                    </ItemTemplate> 
                </asp:Repeater>


                   <asp:Repeater ID="dtlBoat2" runat="server" OnItemDataBound="dtlBoat2_ItemDataBound2">
                    <ItemTemplate>
                        <li>
                           
                            <div>
                                <div class ="col-sm-6 col-xs-6" style="margin-top: 0 !important">
                                    <div class="row dtlBoatType" style="overflow: hidden;">

                                        <div class="col-sm-6 col-xs-12 ">
                                            <h2>
                                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Eval("BoatTypeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblBtBoatType" runat="server" Text='<%# Eval("BoatTypeName") %>'
                                                    CssClass="CSlblBoatHouse" Font-Bold="true"></asp:Label></h2>
                                        </div>
                                        <div class="col-sm-6 col-xs-6">

                                            <img src="images/BoatGifDisplay.gif" class="imgBoatGif" />
                                            <%-- <asp:Label ID="BoatImageLink" runat="server" src='<%# Eval("BoatImageLink") %>'
                                                   Class="imgBoatGif" ></asp:Label>--%>
                                            <%-- <img src= '<%# Eval("BoatImageLink") %>' class="imgBoatGif" runat="server" />--%>
                                            <%--<asp:Image ID="lblBtBoatImgLink" runat="server" ImageUrl='<%# Eval("BoatImageLink") %>' class="imgBoatGif" />--%>

                                        </div>


                                       

                                        <div class="dtlAvailable" style=" padding-top: 1px; margin-top: 1px; border-radius: 10px;">
                                            <div class="col-sm-12 pl-0 text-center btntbody dtlAvaNor ">
                                                <asp:DataList ID="dtAvailNrml" runat="server">
                                                    <HeaderTemplate>

                                                        <h3 style="margin: 0px; text-align: center; font-weight: bolder; font-family: inherit; color: #43bd58; margin-right: -2px; margin-left: -10px;">Available Boat Normal</h3>
                                                        <table>
                                                          
                                                            <th style="background-color: rgb(241, 92, 128);">Seater Type</th>
                                                            <th style="background-color: rgb(241, 92, 128);">No Of Trips</th>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>

                                                            <tr>
                                                               
                                                                <td>
                                                                    <asp:Label ID="lblSeaterType" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label>
                                                                </td>
                                                              
                                                                <td>
                                                                    <asp:Label ID="lblRemainTrips" runat="server" Text='<%# Eval("RemainTrips") %>'></asp:Label>
                                                                </td>



                                                            </tr>


                                                        </table>
                                                    </ItemTemplate>

                                                </asp:DataList>
                                            </div>

                                            <div class="col-sm-12 pl-0 text-center btntbody dtlAvaPre ">
                                                <asp:DataList ID="dtAvailPre" runat="server">
                                                    <HeaderTemplate>

                                                       
                                                        <h3 style="margin: 0px; text-align: center; font-weight: bolder; font-family: inherit; color:#ff00d4; margin-right: -2px; margin-left: -10px;">Available Boat Premium</h3>
                                                        <table>
                                                          
                                                            <th style="background-color: forestgreen">Seater Type</th>
                                                            <th style="background-color: forestgreen">No Of Trips</th>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>

                                                            <tr>
                                                               
                                                                <td>
                                                                    <asp:Label ID="lblSeaterType" runat="server" Text='<%# Eval("SeaterType") %>'></asp:Label>
                                                                </td>
                                                              
                                                                <td>
                                                                    <asp:Label ID="lblRemainTrips" runat="server" Text='<%# Eval("RemainTrips") %>'></asp:Label>
                                                                </td>


                                                            </tr>


                                                        </table>
                                                    </ItemTemplate>

                                                </asp:DataList>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>

                          
                    </ItemTemplate>
                </asp:Repeater>

            </ul>


        </div>
       

        <div class="footer">
            <div runat="server" style="background-image: linear-gradient(to bottom, #ffffff, #c1edff, #00e6ff80, #00d88bde, #0ebe06a6);">
                <p id="myText">PayPre Welcomes You</p>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        (function (g) {
            var q = {
                vertical: !1, rtl: !1, start: 1, offset: 1,
                size: null, scroll: 3, visible: null, animation: "normal", easing: "swing", auto: 0, wrap: null, initCallback: null, setupCallback: null,
                reloadCallback: null, itemLoadCallback: null, itemFirstInCallback: null, itemFirstOutCallback: null, itemLastInCallback: null, itemLastOutCallback: null,
                itemVisibleInCallback: null, itemVisibleOutCallback: null,
                animationStepCallback: null, buttonNextHTML: "<div></div>", buttonPrevHTML: "<div></div>",
                buttonNextEvent: "click", buttonPrevEvent: "click", buttonNextCallback: null, buttonPrevCallback: null, itemFallbackDimension: null

            }, m = !1; g(window).bind("load.jcarousel", function () {
                m = !0
            });

            g.jcarousel = function (a, c) {

                this.options = g.extend({}, q, c || {});
                this.autoStopped = this.locked = !1;
                this.buttonPrevState = this.buttonNextState = this.buttonPrev = this.buttonNext = this.list = this.clip = this.container = null;
                if (!c || c.rtl === void 0)
                    this.options.rtl = (g(a).attr("dir") || g("html").attr("dir") || "").toLowerCase() == "rtl"; this.wh = !this.options.vertical ? "width" : "height";
                this.lt = !this.options.vertical ?
                    this.options.rtl ? "right" : "left" : "top";
                for (var b = "", d = a.className.split(" "), f = 0; f < d.length; f++)
                    if (d[f].indexOf("jcarousel-skin") != -1) {
                        g(a).removeClass(d[f]);
                        b = d[f]; break
                    } a.nodeName.toUpperCase() == "UL" || a.nodeName.toUpperCase() == "OL" ? (this.list = g(a), this.clip = this.list.parents(".jcarousel-clip"), this.container = this.list.parents(".jcarousel-container")) : (this.container = g(a), this.list = this.container.find("ul,ol").eq(0), this.clip = this.container.find(".jcarousel-clip")); if (this.clip.size() === 0) this.clip = this.list.wrap("<div></div>").parent();
                if (this.container.size() === 0) this.container = this.clip.wrap("<div></div>").parent(); b !== "" && this.container.parent()[0].className.indexOf("jcarousel-skin") == -1 && this.container.wrap('<div class=" ' + b + '"></div>');

                this.buttonPrev = g(".jcarousel-prev", this.container);
                if (this.buttonPrev.size() === 0 && this.options.buttonPrevHTML !== null)
                    this.buttonPrev = g(this.options.buttonPrevHTML).appendTo(this.container);
                this.buttonPrev.addClass(this.className("jcarousel-prev"));
                this.buttonNext = g(".jcarousel-next", this.container);

                if (this.buttonNext.size() === 0 && this.options.buttonNextHTML !== null) this.buttonNext = g(this.options.buttonNextHTML).appendTo(this.container); this.buttonNext.addClass(this.className("jcarousel-next")); this.clip.addClass(this.className("jcarousel-clip")).css({ position: "relative" }); this.list.addClass(this.className("jcarousel-list")).css({ overflow: "hidden", position: "relative", top: 0, margin: 0, padding: 0 }
                ).css(this.options.rtl ? "right" : "left", 0);
                this.container.addClass(this.className("jcarousel-container")).css({ position: "relative" });
                !this.options.vertical && this.options.rtl && this.container.addClass("jcarousel-direction-rtl").attr("dir", "rtl");
                var j = this.options.visible !== null ? Math.ceil(this.clipping() / this.options.visible) : null,
                    b = this.list.children("li"), e = this;
                if (b.size() > 0) {
                    var h = 0, i = this.options.offset;
                    b.each(function () {
                        e.format(this, i++); h += e.dimension(this, j)
                    });
                    this.list.css(this.wh, h + 100 + "px"); if (!c || c.size === void 0) this.options.size = b.size()
                } this.container.css("display", "block");
                this.buttonNext.css("display", "block");
                this.buttonPrev.css("display", "block");
                this.funcNext = function () {
                    e.next()
                };
                this.funcPrev = function () {
                    e.prev()
                };
                this.funcResize = function () {
                    e.resizeTimer && clearTimeout(e.resizeTimer);
                    e.resizeTimer = setTimeout(function () { e.reload() }, 100)
                };
                this.options.initCallback !== null && this.options.initCallback(this, "init");
                !m && g.browser.safari ? (this.buttons(!1, !1),
                    g(window).bind("load.jcarousel", function () {
                        e.setup()
                    }))
                    : this.setup()
            };
            var f = g.jcarousel; f.fn = f.prototype = { jcarousel: "0.2.8" };
            f.fn.extend = f.extend = g.extend; f.fn.extend({
                setup: function () {
                    this.prevLast = this.prevFirst = this.last = this.first = null; this.animating = !1; this.tail = this.resizeTimer = this.timer = null;
                    this.inTail = !1; if (!this.locked) { this.list.css(this.lt, this.pos(this.options.offset) + "px"); var a = this.pos(this.options.start, !0); this.prevFirst = this.prevLast = null; this.animate(a, !1); g(window).unbind("resize.jcarousel", this.funcResize).bind("resize.jcarousel", this.funcResize); this.options.setupCallback !== null && this.options.setupCallback(this) }
                }, reset: function () { this.list.empty(); this.list.css(this.lt, "0px"); this.list.css(this.wh, "10px"); this.options.initCallback !== null && this.options.initCallback(this, "reset"); this.setup() }, reload: function () {
                    this.tail !== null && this.inTail && this.list.css(this.lt, f.intval(this.list.css(this.lt)) + this.tail); this.tail = null; this.inTail = !1; this.options.reloadCallback !== null && this.options.reloadCallback(this);
                    if (this.options.visible !== null) { var a = this, c = Math.ceil(this.clipping() / this.options.visible), b = 0, d = 0; this.list.children("li").each(function (f) { b += a.dimension(this, c); f + 1 < a.first && (d = b) }); this.list.css(this.wh, b + "px"); this.list.css(this.lt, -d + "px") } this.scroll(this.first, !1)
                }, lock: function () { this.locked = !0; this.buttons() }, unlock: function () { this.locked = !1; this.buttons() }, size: function (a) { if (a !== void 0) this.options.size = a, this.locked || this.buttons(); return this.options.size }, has: function (a, c) {
                    if (c === void 0 || !c) c = a; if (this.options.size !== null && c > this.options.size) c = this.options.size; for (var b = a; b <= c; b++) {
                        var d = this.get(b);
                        if (!d.length || d.hasClass("jcarousel-item-placeholder")) return !1
                    } return !0
                }, get: function (a) { return g(">.jcarousel-item-" + a, this.list) }, add: function (a, c) {
                    var b = this.get(a), d = 0, p = g(c); if (b.length === 0) for (var j, e = f.intval(a), b = this.create(a); ;) { if (j = this.get(--e), e <= 0 || j.length) { e <= 0 ? this.list.prepend(b) : j.after(b); break } } else d = this.dimension(b); p.get(0).nodeName.toUpperCase() == "LI" ? (b.replaceWith(p), b = p) : b.empty().append(c);
                    this.format(b.removeClass(this.className("jcarousel-item-placeholder")), a); p = this.options.visible !== null ? Math.ceil(this.clipping() / this.options.visible) : null; d = this.dimension(b, p) - d; a > 0 && a < this.first && this.list.css(this.lt, f.intval(this.list.css(this.lt)) - d + "px"); this.list.css(this.wh, f.intval(this.list.css(this.wh)) + d + "px"); return b
                }, remove: function (a) { var c = this.get(a); if (c.length && !(a >= this.first && a <= this.last)) { var b = this.dimension(c); a < this.first && this.list.css(this.lt, f.intval(this.list.css(this.lt)) + b + "px"); c.remove(); this.list.css(this.wh, f.intval(this.list.css(this.wh)) - b + "px") } }, next: function () {
                    this.tail !== null && !this.inTail ? this.scrollTail(!1) : this.scroll((this.options.wrap == "both" || this.options.wrap == "last") &&
                        this.options.size !== null && this.last == this.options.size ? 1 : this.first + this.options.scroll)
                }, prev: function () {
                    this.tail !== null && this.inTail ? this.scrollTail(!0) : this.scroll((this.options.wrap == "both" || this.options.wrap == "first") && this.options.size !== null &&
                        this.first == 1 ? this.options.size : this.first - this.options.scroll)
                }, scrollTail: function (a) {
                    if (!this.locked && !this.animating && this.tail) {
                        this.pauseAuto(); var c = f.intval(this.list.css(this.lt)), c = !a ? c - this.tail : c + this.tail; this.inTail = !a;
                        this.prevFirst = this.first; this.prevLast = this.last; this.animate(c)
                    }
                }, scroll: function (a, c) { !this.locked && !this.animating && (this.pauseAuto(), this.animate(this.pos(a), c)) },
                pos: function (a, c) {
                    var b = f.intval(this.list.css(this.lt)); if (this.locked || this.animating) return b;
                    this.options.wrap != "circular" && (a = a < 1 ? 1 : this.options.size && a > this.options.size ? this.options.size : a);
                    for (var d = this.first > a, g = this.options.wrap != "circular" &&
                        this.first <= 1 ? 1 : this.first, j = d ? this.get(g) : this.get(this.last), e = d ? g : g - 1, h = null, i = 0, k = !1, l = 0; d ? --e >= a : ++e < a;) {
                        h = this.get(e); k = !h.length; if (h.length === 0 && (h = this.create(e).addClass(this.className("jcarousel-item-placeholder")), j[d ? "before" : "after"](h), this.first !== null &&
                            this.options.wrap == "circular" && this.options.size !== null && (e <= 0 || e > this.options.size))) j = this.get(this.index(e)), j.length
                                && (h = this.add(e, j.clone(!0))); j = h; l = this.dimension(h); k && (i += l); if (this.first !== null &&
                                    (this.options.wrap == "circular" || e >= 1 && (this.options.size === null || e <= this.options.size))) b = d ? b + l : b - l
                    } for (var g = this.clipping(), m = [], o = 0, n = 0, j = this.get(a - 1), e = a; ++o;) {
                        h = this.get(e); k = !h.length;
                        if (h.length === 0) {
                            h = this.create(e).addClass(this.className("jcarousel-item-placeholder")); if (j.length === 0) this.list.prepend(h);
                            else j[d ? "before" : "after"](h); if (this.first !== null && this.options.wrap == "circular" && this.options.size !== null && (e <= 0 || e > this.options.size)) j = this.get(this.index(e)), j.length && (h = this.add(e, j.clone(!0)))
                        } j = h; l = this.dimension(h); if (l === 0) throw Error("jCarousel: No width/height set for items. This will cause an infinite loop. Aborting..."); this.options.wrap != "circular" && this.options.size !== null && e > this.options.size ? m.push(h) : k && (i += l); n += l; if (n >= g) break; e++
                    } for (h = 0; h < m.length; h++) m[h].remove(); i > 0 && (this.list.css(this.wh, this.dimension(this.list) + i + "px"),
                        d && (b -= i, this.list.css(this.lt, f.intval(this.list.css(this.lt)) - i + "px"))); i = a + o - 1; if (this.options.wrap != "circular" &&
                            this.options.size && i > this.options.size) i = this.options.size; if (e > i) {
                                o = 0; e = i; for (n = 0; ++o;) {
                                    h = this.get(e--); if (!h.length)
                                        break; n += this.dimension(h); if (n >= g) break
                                }
                            } e = i - o + 1; this.options.wrap != "circular" && e < 1 && (e = 1); if (this.inTail && d) b += this.tail, this.inTail = !1;
                    this.tail = null; if (this.options.wrap != "circular" && i == this.options.size && i - o + 1 >= 1 && (d = f.intval(this.get(i).css(!this.options.vertical ? "marginRight" : "marginBottom")), n - d > g)) this.tail = n - g - d; if (c && a === this.options.size && this.tail) b -= this.tail,
                        this.inTail = !0; for (; a-- > e;) b += this.dimension(this.get(a)); this.prevFirst = this.first; this.prevLast = this.last; this.first = e; this.last = i; return b
                }, animate: function (a, c) {
                    if (!this.locked && !this.animating) {
                        this.animating = !0;
                        var b = this, d = function () {
                            b.animating = !1; a === 0 && b.list.css(b.lt, 0); !b.autoStopped && (b.options.wrap == "circular" || b.options.wrap == "both" || b.options.wrap == "last" || b.options.size === null || b.last < b.options.size || b.last == b.options.size && b.tail !== null && !b.inTail) && b.startAuto();
                            b.buttons(); b.notify("onAfterAnimation"); if (b.options.wrap == "circular" && b.options.size !== null) for (var c = b.prevFirst; c <= b.prevLast; c++) c !== null && !(c >= b.first && c <= b.last) && (c < 1 || c > b.options.size) && b.remove(c)
                        }; this.notify("onBeforeAnimation"); if (!this.options.animation || c === !1) this.list.css(this.lt, a + "px"), d(); else {
                            var f = !this.options.vertical ? this.options.rtl ? { right: a } : { left: a } : { top: a }, d = { duration: this.options.animation, easing: this.options.easing, complete: d };
                            if (g.isFunction(this.options.animationStepCallback)) d.step = this.options.animationStepCallback; this.list.animate(f, d)
                        }
                    }
                }, startAuto: function (a) {
                    if (a !== void 0) this.options.auto = a; if (this.options.auto === 0) return this.stopAuto(); if (this.timer === null) {
                        this.autoStopped = !1; var c = this; this.timer = window.setTimeout(function () {
                            c.next();
                        }, this.options.auto * 1E3)
                    }
                }, stopAuto: function () {
                    this.pauseAuto(); this.autoStopped = !0
                }, pauseAuto: function () { if (this.timer !== null) window.clearTimeout(this.timer), this.timer = null }, buttons: function (a, c) {
                    if (a == null && (a = !this.locked && this.options.size !== 0 && (this.options.wrap &&
                        this.options.wrap != "first" || this.options.size === null || this.last < this.options.size),
                        !this.locked && (!this.options.wrap || this.options.wrap == "first") &&
                        this.options.size !== null && this.last >= this.options.size)) a = this.tail !== null && !this.inTail;
                    if (c == null &&
                        (c = !this.locked && this.options.size !== 0 &&
                            (this.options.wrap && this.options.wrap != "last" || this.first > 1), !this.locked &&
                            (!this.options.wrap || this.options.wrap == "last") && this.options.size !== null && this.first == 1))
                        c = this.tail !== null && this.inTail; var b = this;
                    this.buttonNext.size() > 0 ?
                     (this.buttonNext.bind(this.options.buttonNextEvent + ".jcarousel", setTimeout(this.funcNext, 6000)), a &&
                     this.buttonNext.bind(this.options.buttonNextEvent + ".jcarousel", setTimeout(this.funcNext, 6000)),
                     this.buttonNext[a ? "removeClass" : "addClass"](this.className("jcarousel-next-disabled"))
                    .attr("disabled", a ? !1 : !0),
                     this.options.buttonNextCallback !== null
                     && this.buttonNext.data("jcarouselstate") != a &&
                     this.buttonNext.each(function () { b.options.buttonNextCallback(b, this, a) }).data("jcarouselstate", a)) :
                     this.options.buttonNextCallback !== null && this.buttonNextState != a && this.options.buttonNextCallback(b, null, a);

                    if (a == 0) {
                        <%-- document.getElementById('<%=Buttonclick.ClientID%>').click();--%>

                    }

                    // this.buttonPrev.size() > 0 && this.buttonNext.size() < 0 ?
                    //(this.buttonPrev.unbind(this.options.buttonPrevEvent + ".jcarousel", setTimeout(this.funcPrev, 6000)), c
                    //&& this.buttonPrev.bind(this.options.buttonPrevEvent + ".jcarousel", setTimeout(this.funcPrev, 6000)),
                    //this.buttonPrev[c ? "removeClass" : "addClass"](this.className("jcarousel-prev-disabled")).attr("disabled", c ? !1 : !0), this.options.buttonPrevCallback !== null
                    //&& this.buttonPrev.data("jcarouselstate") != c && this.buttonPrev.each(function () {
                    //    b.options.buttonPrevCallback(b, this, c)
                    //}).data("jcarouselstate", c)) : this.options.buttonPrevCallback !== null && this.buttonPrevState != c &&
                    //this.options.buttonPrevCallback(b, null, c);
                    this.buttonNextState = a; this.buttonPrevState = c

                }, notify: function (a) {
                    var c = this.prevFirst === null ? "init" : this.prevFirst < this.first ? "next" : "prev"; this.callback("itemLoadCallback", a, c); this.prevFirst !== this.first && (this.callback("itemFirstInCallback", a, c, this.first), this.callback("itemFirstOutCallback", a, c, this.prevFirst));
                    this.prevLast !== this.last && (this.callback("itemLastInCallback", a, c, this.last),
                        this.callback("itemLastOutCallback", a, c, this.prevLast)); this.callback("itemVisibleInCallback", a, c, this.first, this.last, this.prevFirst, this.prevLast); this.callback("itemVisibleOutCallback", a, c, this.prevFirst, this.prevLast, this.first, this.last)
                }, callback: function (a, c, b, d, f, j, e) {
                    if (!(this.options[a] == null || typeof this.options[a] != "object" && c != "onAfterAnimation")) {
                        var h = typeof this.options[a] == "object" ? this.options[a][c] : this.options[a]; if (g.isFunction(h)) {
                            var i = this; if (d === void 0) h(i, b, c); else if (f === void 0) this.get(d).each(function () { h(i, this, d, b, c) }); else for (var a = function (a) { i.get(a).each(function () { h(i, this, a, b, c) }) }, k = d; k <= f; k++) k !== null && !(k >= j && k <= e) && a(k)
                        }
                    }
                }, create: function (a) { return this.format("<li></li>", a) }, format: function (a, c) {
                    for (var a = g(a), b = a.get(0).className.split(" "), d = 0; d < b.length; d++) b[d].indexOf("jcarousel-") != -1 && a.removeClass(b[d]);
                    a.addClass(this.className("jcarousel-item")).addClass(this.className("jcarousel-item-" + c)).css({ "float": this.options.rtl ? "right" : "left", "list-style": "none" }).attr("jcarouselindex", c); return a
                }, className: function (a) { return a + " " + a + (!this.options.vertical ? "-horizontal" : "-vertical") }, dimension: function (a, c) {
                    var b = g(a); if (c == null) return !this.options.vertical ? b.outerWidth(!0) || f.intval(this.options.itemFallbackDimension) : b.outerHeight(!0) || f.intval(this.options.itemFallbackDimension); else {
                        var d = !this.options.vertical ? c - f.intval(b.css("marginLeft")) - f.intval(b.css("marginRight")) : c - f.intval(b.css("marginTop")) - f.intval(b.css("marginBottom")); g(b).css(this.wh, d + "px");
                        return this.dimension(b)
                    }
                }, clipping: function () {
                    return !this.options.vertical ?
                        this.clip[0].offsetWidth - f.intval(this.clip.css("borderLeftWidth")) - f.intval(this.clip.css("borderRightWidth")) : this.clip[0].offsetHeight - f.intval(this.clip.css("borderTopWidth")) - f.intval(this.clip.css("borderBottomWidth"))
                }, index: function (a, c) { if (c == null) c = this.options.size; return Math.round(((a - 1) / c - Math.floor((a - 1) / c)) * c) + 1 }
            }); f.extend({ defaults: function (a) { return g.extend(q, a || {}) }, intval: function (a) { a = parseInt(a, 10); return isNaN(a) ? 0 : a }, windowLoaded: function () { m = !0 } }); g.fn.jcarousel = function (a) {
                if (typeof a == "string") { var c = g(this).data("jcarousel"), b = Array.prototype.slice.call(arguments, 1); return c[a].apply(c, b) } else return this.each(function () {
                    var b = g(this).data("jcarousel");
                    b ? (a && g.extend(b.options, a), b.reload()) : g(this).data("jcarousel", new f(this, a))
                })
            }
        })(jQuery);
    </script>
    <input type="number" id="txtCarouselTime" runat="server" style="display: none" />
</body>

</html>
