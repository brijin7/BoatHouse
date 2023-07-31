<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptServiceWiseCollection.aspx.cs" Inherits="Reports_BoatHouseReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  <script src="~/Scripts/jquery-1.10.2.js"></script>--%>
    <style>
        .dtlBoatType1 {
            margin-bottom: 8px;
            border-radius: 10px;
            box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
            overflow: hidden;
            transition: all .5s ease;
            margin-top: 6px;
            margin-right: 100px;
            margin-left: 1px;
        }
    </style>
    <style>
        .inline-rb input[type="radio"] {
            width: auto;
            margin-right: 4px;
        }

        .inline-rb label {
            display: inline;
        }
    </style>

    <%-- <script type="text/javascript" src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>--%>
    <script type="text/javascript">
        var calc;
        function calculate2000() {
            var textValue1 = $("#<%=txt2000.ClientID %>").val();
            $('#<%=txtOut2000.ClientID%>').text(2000 * textValue1);

        }

        $(function () {
            $('#txt2000').blur(calc);
            $('#txtOut2000').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate500() {
            var textValue1 = $("#<%=txt500.ClientID %>").val();
            $('#<%=txtOut500.ClientID%>').text(500 * textValue1);

        }

        $(function () {
            $('#txt500').blur(calc);
            $('#txtOut500').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate200() {
            var textValue1 = $("#<%=txt200.ClientID %>").val();
            $('#<%=txtOut200.ClientID%>').text(200 * textValue1);

        }

        $(function () {
            $('#txt200').blur(calc);
            $('#txtOut200').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate100() {
            var textValue1 = $("#<%=txt100.ClientID %>").val();
            $('#<%=txtOut100.ClientID%>').text(100 * textValue1);

        }

        $(function () {
            $('#txt100').blur(calc);
            $('#txtOut100').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate50() {
            var textValue1 = $("#<%=txt50.ClientID %>").val();
            $('#<%=txtOut50.ClientID%>').text(50 * textValue1);

        }

        $(function () {
            $('#txt50').blur(calc);
            $('#txtOut50').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate20() {
            var textValue1 = $("#<%=txt20.ClientID %>").val();
            $('#<%=txtOut20.ClientID%>').text(20 * textValue1);

        }

        $(function () {
            $('#txt20').blur(calc);
            $('#txtOut20').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate10() {
            var textValue1 = $("#<%=txt10.ClientID %>").val();
            $('#<%=txtOut10.ClientID%>').text(10 * textValue1);

        }

        $(function () {
            $('#txt10').blur(calc);
            $('#txtOut10').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate5() {
            var textValue1 = $("#<%=txt5.ClientID %>").val();
            $('#<%=txtOut5.ClientID%>').text(5 * textValue1);

        }

        $(function () {
            $('#txt5').blur(calc);
            $('#txtOut5').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate2() {
            var textValue1 = $("#<%=txt2.ClientID %>").val();
            $('#<%=txtOut2.ClientID%>').text(2 * textValue1);

        }

        $(function () {
            $('#txt2').blur(calc);
            $('#txtOut2').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculate1() {
            var textValue1 = $("#<%=txt1.ClientID %>").val();
            $('#<%=txtOut1.ClientID%>').text(1 * textValue1);

        }

        $(function () {
            $('#txt1').blur(calc);
            $('#txtOut1').blur(calc);
        });
    </script>
    <script type="text/javascript">
        function calculatetotal() {
            var textValue2000 = $("#<%=txt2000.ClientID %>").val();
            var textValue500 = $("#<%=txt500.ClientID %>").val();
            var textValue200 = $("#<%=txt200.ClientID %>").val();
            var textValue100 = $("#<%=txt100.ClientID %>").val();
            var textValue50 = $("#<%=txt50.ClientID %>").val();
            var textValue20 = $("#<%=txt20.ClientID %>").val();
            var textValue10 = $("#<%=txt10.ClientID %>").val();
            var textValue5 = $("#<%=txt5.ClientID %>").val();
            var textValue2 = $("#<%=txt2.ClientID %>").val();
            var textValue1 = $("#<%=txt1.ClientID %>").val();

            var value1 = Number(textValue2000);
            var value2 = Number(textValue500);
            var value3 = Number(textValue200);
            var value4 = Number(textValue100);
            var value5 = Number(textValue50);
            var value6 = Number(textValue20);
            var value7 = Number(textValue10);
            var value8 = Number(textValue5);
            var value9 = Number(textValue2);
            var value10 = Number(textValue1);

            $('#<%=lblDenCount.ClientID%>').text(value1 + value2 + value3 + value4 + value5 + value6 + value7 + value8 + value9 + value10);

        }

        $(function () {
            $('#txt2000').blur(calc);
            $('#txt500').blur(calc);
            $('#txt100').blur(calc);
            $('#txt50').blur(calc);
            $('#txt20').blur(calc);
            $('#txt10').blur(calc);
            $('#txt5').blur(calc);
            $('#txt2').blur(calc);
            $('#txt1').blur(calc);
        });

    </script>
    <script type="text/javascript">
        function CalculateNoteTotal() {
            var textValue2000 = $('#<%=txtOut2000.ClientID%>').text();
            var textValue500 = $('#<%=txtOut500.ClientID %>').text();
            var textValue200 = $('#<%=txtOut200.ClientID %>').text();
            var textValue100 = $('#<%=txtOut100.ClientID %>').text();
            var textValue50 = $('#<%=txtOut50.ClientID %>').text();
            var textValue20 = $('#<%=txtOut20.ClientID %>').text();
            var textValue10 = $('#<%=txtOut10.ClientID %>').text();
            var textValue5 = $('#<%=txtOut5.ClientID %>').text();
            var textValue2 = $('#<%=txtOut2.ClientID %>').text();
            var textValue1 = $('#<%=txtOut1.ClientID %>').text();


            var value1 = Number(textValue2000);
            $('#<%=inptxt2000.ClientID%>').val(value1);
            var value2 = Number(textValue500);
            $('#<%=inptxt500.ClientID%>').val(value2);
            var value3 = Number(textValue200);
            $('#<%=inptxt200.ClientID%>').val(value3);
            var value4 = Number(textValue100);
            $('#<%=inptxt100.ClientID%>').val(value4);
            var value5 = Number(textValue50);
            $('#<%=inptxt50.ClientID%>').val(value5);
            var value6 = Number(textValue20);
            $('#<%=inptxt20.ClientID%>').val(value6);
            var value7 = Number(textValue10);
            $('#<%=inptxt10.ClientID%>').val(value7);
            var value8 = Number(textValue5);
            $('#<%=inptxt5.ClientID%>').val(value8);
            var value9 = Number(textValue2);
            $('#<%=inptxt2.ClientID%>').val(value9);
            var value10 = Number(textValue1);
            $('#<%=inptxt1.ClientID%>').val(value10);
            $('#<%=lblDenTotalAmt.ClientID%>').text(value1 + value2 + value3 + value4 + value5 + value6 + value7 + value8 + value9 + value10);

            var value = $('#<%=lblDenTotalAmt.ClientID%>').text();

            $('#<%=txtfinalvalue.ClientID%>').val(value);
            $('#<%=hfDenomination.ClientID%>').val(value);

            if ($('#<%=hfServiceName.ClientID%>').val() != "") {
                var Inactive = $('#<%=hfServiceName.ClientID%>').val();
            }
            var value = $('#<%=hfDenomination.ClientID%>').val();

            if (Inactive == value) {

                document.getElementById("<%=btnMiniPrint.ClientID%>").disabled = false;
            }
            else if (Inactive < value) {
                document.getElementById("<%=btnMiniPrint.ClientID%>").disabled = true;
            }
            else if (Inactive > value) {
                document.getElementById("<%=btnMiniPrint.ClientID%>").disabled = true;
            }


        }

        $(function () {
            $('#txt2000').blur(calc);
            $('#txt500').blur(calc);
            $('#txt100').blur(calc);
            $('#txt50').blur(calc);
            $('#txt20').blur(calc);
            $('#txt10').blur(calc);
            $('#txt5').blur(calc);
            $('#txt2').blur(calc);
            $('#txt1').blur(calc);
        });

    </script>
    <script type="text/javascript">
        function ZeroKeyPress2000() {
            var text2000 = $("#<%=txt2000.ClientID %>").val();
            if (text2000 == "0") {
                $("#<%=txt2000.ClientID %>").val('');
                $('#<%=txtOut2000.ClientID%>').text('');
            }

        }
    </script>
    <script type="text/javascript">
        function ZeroKeyPress500() {
            var text500 = $("#<%=txt500.ClientID %>").val();
            if (text500 == "0") {
                $("#<%=txt500.ClientID %>").val('');
                $('#<%=txtOut500.ClientID%>').text('');
            }

        }
    </script>

    <script type="text/javascript">
        function ZeroKeyPress200() {
            var text200 = $("#<%=txt200.ClientID %>").val();
            if (text200 == "0") {

                $("#<%=txt200.ClientID %>").val('');
                $('#<%=txtOut200.ClientID%>').text('');
            }

        }
    </script>
    <script type="text/javascript">
        function ZeroKeyPress100() {
            var text100 = $("#<%=txt100.ClientID %>").val();
            if (text100 == "0") {
                $("#<%=txt100.ClientID %>").val('');
                $('#<%=txtOut100.ClientID%>').text('');

            }

        }
    </script>
    <script type="text/javascript">
        function ZeroKeyPress50() {
            var text50 = $("#<%=txt50.ClientID %>").val();
            if (text50 == "0") {
                $("#<%=txt50.ClientID %>").val('');
                $('#<%=txtOut50.ClientID%>').text('');
            }

        }
    </script>
    <script type="text/javascript">
        function ZeroKeyPress20() {
            var text20 = $("#<%=txt20.ClientID %>").val();
            if (text20 == "0") {
                $("#<%=txt20.ClientID %>").val('');
                $('#<%=txtOut20.ClientID%>').text('');

            }

        }
    </script>
    <script type="text/javascript">
        function ZeroKeyPress10() {
            var text10 = $("#<%=txt10.ClientID %>").val();
            if (text10 == "0") {
                $("#<%=txt10.ClientID %>").val('');
                $('#<%=txtOut10.ClientID%>').text('');
            }

        }
    </script>
    <script type="text/javascript">
        function ZeroKeyPress5() {
            var text5 = $("#<%=txt5.ClientID %>").val();
            if (text5 == "0") {
                $("#<%=txt5.ClientID %>").val('');
                $('#<%=txtOut5.ClientID%>').text('');

            }

        }
    </script>
    <script type="text/javascript">
        function ZeroKeyPress2() {
            var text2 = $("#<%=txt2.ClientID %>").val();
            if (text2 == "0") {
                $("#<%=txt2.ClientID %>").val('');
                $('#<%=txtOut2.ClientID%>').text('');

            }

        }
    </script>
    <script type="text/javascript">
        function ZeroKeyPress1() {
            var text1 = $("#<%=txt1.ClientID %>").val();
            if (text1 == "0") {
                $("#<%=txt1.ClientID %>").val('');
                $('#<%=txtOut1.ClientID%>').text('');

            }

        }
    </script>

    <div class="form-body">
        <h5 class="pghr">Service Wise Report</h5>
        <hr />
        <br />

        <div class="row panel-body dtlBoatType1">
            <asp:RadioButton ID="rblServiceWise" runat="server" Checked="true" Style="padding-left: 1rem;" TabIndex="1" Text="Service Wise Collection"
                AutoPostBack="true" OnCheckedChanged="rblServiceWise_CheckedChanged" CssClass="inline-rb" />
            <asp:RadioButton ID="rblRestaurant" runat="server" Style="padding-left: 1rem;" TabIndex="2" Text="Restaurant Collection"
                AutoPostBack="true" OnCheckedChanged="rblRestaurant_CheckedChanged" CssClass="inline-rb" />
            <asp:RadioButton ID="rblMonthWise" runat="server" Style="padding-left: 1rem;" TabIndex="3" Text="Month Wise Collection"
                AutoPostBack="true" OnCheckedChanged="rblMonthWise_CheckedChanged" CssClass="inline-rb" />

            <asp:Label ID="lblLabel1" runat="server" Style="padding-left: 6rem;  font-size: 17px;color: #09bf06;" > Trip Not Started :
                 <asp:Label ID="lblTripNotStartedCount" runat="server" Text="0" > </asp:Label>
            </asp:Label>
              <asp:Label ID="Label4" runat="server" Style="padding-left: 7rem;  font-size: 17px;color:red;" > Trip Not Ended :
                 <asp:Label ID="lblTripNotEndedCount" runat="server" Text="0" > </asp:Label>
            </asp:Label>
        </div>

        <div id="divRestaurant" runat="server" visible="false">
            <div class="row" style="padding-left: 15px; padding-top: 1rem; margin-right: -10px; margin-left: -10px; background-color: #EBF3C7;">
                <div class="row">
                    <div class="col-xl-12 col-md-12 col-lg-12  col-sm-12">
                        <div class="form-group">
                            <asp:RadioButtonList runat="server" ID="rbtnResMonthWise" TabIndex="1" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnResMonthWise_SelectedIndexChanged" CssClass="rbl">

                                <asp:ListItem Value="1" Selected="True">Date Wise</asp:ListItem>
                                <asp:ListItem Value="2">Month Wise</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12" runat="server" id="divResDateWise">
                    <div class="row">
                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblTypes" id="lblResFromdate"><i class="fa fa-calendar" aria-hidden="true"></i>From Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtResFromDate" CssClass="form-control frmDate" runat="server"
                                TabIndex="3">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtResFromDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select Booking Date</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblMonthTo" id="lblResTodate1"><i class="fa fa-calendar" aria-hidden="true"></i>To Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtResToDate" CssClass="form-control toDate" runat="server"
                                TabIndex="10">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtToDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select To Date</asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                            <span style="float: left">
                                <asp:Button ID="btnResDtlRpt" runat="server" Text="Category Wise Report" class="btn btn-primary" ValidationGroup="Search" OnClick="btnResDtlRpt_Click" TabIndex="4" />
                            </span>
                        </div>
                        <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                            <span style="float: left">
                                <asp:Button ID="btnResGenerate" runat="server" Text="Detailed Report" class="btn btn-primary" ValidationGroup="Search" OnClick="btnResGenerate_Click" TabIndex="4" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12" runat="server" id="divResMonthwise" visible="false">
                    <div class="row">
                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblMonthTo" id="lblTodate"><i class="fa fa-calendar" aria-hidden="true"></i>Financial Year<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlResFinYear" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlResFinYear_SelectedIndexChanged" TabIndex="7">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlResFinYear"
                                ValidationGroup="Search" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Financial Year</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-xs-12" runat="server" id="divResMonth" visible="false">
                            <label for="lblMonthTo" id="lblTodate"><i class="fa fa-calendar" aria-hidden="true"></i>Month<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlResMonth" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlResMonth_SelectedIndexChanged" TabIndex="7">

                                <asp:ListItem Value="04">April</asp:ListItem>
                                <asp:ListItem Value="05">May</asp:ListItem>
                                <asp:ListItem Value="06">June</asp:ListItem>
                                <asp:ListItem Value="07">July</asp:ListItem>
                                <asp:ListItem Value="08">August</asp:ListItem>
                                <asp:ListItem Value="09">September</asp:ListItem>
                                <asp:ListItem Value="10">October</asp:ListItem>
                                <asp:ListItem Value="11">November</asp:ListItem>
                                <asp:ListItem Value="12">December</asp:ListItem>
                                <asp:ListItem Value="01">January</asp:ListItem>
                                <asp:ListItem Value="02">February</asp:ListItem>
                                <asp:ListItem Value="03">March</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                        <div class="col-sm-2 col-xs-12" runat="server" id="div3" visible="false">
                            <label for="lblMonthFrom" id="lblFromdate"><i class="fa fa-calendar" aria-hidden="true"></i>From Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtResMonthFromDate" CssClass="form-control frmDate" runat="server"
                                TabIndex="10">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtmFromDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select From Date</asp:RequiredFieldValidator>
                        </div>

                        <div class="col-sm-2 col-xs-12" runat="server" id="div4" visible="false">
                            <label for="lblMonthTo" id="lblTodate"><i class="fa fa-calendar" aria-hidden="true"></i>To Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtResMonthToDate" CssClass="form-control toDate" runat="server"
                                TabIndex="10">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtmToDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select To Date</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                            <span style="float: left">
                                <asp:Button ID="btnResCategory" runat="server" Text="Category Wise Report" class="btn btn-primary" ValidationGroup="Search" OnClick="btnResCategory_Click" TabIndex="4" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divMonthWise" runat="server" visible="false">

            <div class="row" style="padding-left: 15px; padding-top: 1rem; margin-right: -10px; margin-left: -10px; background-color: #dfbef1e6;">
                <div class="row">
                    <div class="col-xl-12 col-md-12 col-lg-12  col-sm-12">
                        <div class="form-group">
                            <asp:RadioButtonList runat="server" ID="rbtnMonthWise" TabIndex="1" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnMonthWise_SelectedIndexChanged" CssClass="rbl">

                                <asp:ListItem Value="1" Selected="True">Date Wise</asp:ListItem>
                                <asp:ListItem Value="2">Month Wise</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12" runat="server" id="divDateWiseRpt">
                    <div class="row">
                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblMonthFrom" id="lblFromdate1"><i class="fa fa-calendar" aria-hidden="true"></i>From Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtFromDate" CssClass="form-control frmDate" runat="server"
                                TabIndex="10">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFromDate"
                                ValidationGroup="MonthSearch" SetFocusOnError="True" CssClass="vError">Select From Date</asp:RequiredFieldValidator>
                        </div>

                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblMonthTo" id="lblTodate1"><i class="fa fa-calendar" aria-hidden="true"></i>To Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtToDate" CssClass="form-control toDate" runat="server"
                                TabIndex="10">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                                ValidationGroup="MonthSearch" SetFocusOnError="True" CssClass="vError">Select To Date</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12" runat="server" id="divMonthWiseRpt" visible="false">
                    <div class="row">
                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblMonthTo" id="lblTodate"><i class="fa fa-calendar" aria-hidden="true"></i>Financial Year<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlFinYear" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFinYear_SelectedIndexChanged" TabIndex="7">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlFinYear"
                                ValidationGroup="MonthSearch" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Financial Year</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-2 col-xs-12" runat="server" id="divMonth" visible="false">
                            <label for="lblMonthTo" id="lblTodate"><i class="fa fa-calendar" aria-hidden="true"></i>Month<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlMonth" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" TabIndex="7">

                                <asp:ListItem Value="04">April</asp:ListItem>
                                <asp:ListItem Value="05">May</asp:ListItem>
                                <asp:ListItem Value="06">June</asp:ListItem>
                                <asp:ListItem Value="07">July</asp:ListItem>
                                <asp:ListItem Value="08">August</asp:ListItem>
                                <asp:ListItem Value="09">September</asp:ListItem>
                                <asp:ListItem Value="10">October</asp:ListItem>
                                <asp:ListItem Value="11">November</asp:ListItem>
                                <asp:ListItem Value="12">December</asp:ListItem>
                                <asp:ListItem Value="01">January</asp:ListItem>
                                <asp:ListItem Value="02">February</asp:ListItem>
                                <asp:ListItem Value="03">March</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                        <div class="col-sm-2 col-xs-12" runat="server" id="divFromDate" visible="false">
                            <label for="lblMonthFrom" id="lblFromdate"><i class="fa fa-calendar" aria-hidden="true"></i>From Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtmFromDate" CssClass="form-control frmDate" runat="server"
                                TabIndex="10">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtmFromDate"
                                ValidationGroup="MonthSearch" SetFocusOnError="True" CssClass="vError">Select From Date</asp:RequiredFieldValidator>
                        </div>

                        <div class="col-sm-2 col-xs-12" runat="server" id="divToDate" visible="false">
                            <label for="lblMonthTo" id="lblTodate"><i class="fa fa-calendar" aria-hidden="true"></i>To Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtmToDate" CssClass="form-control toDate" runat="server"
                                TabIndex="10">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtmToDate"
                                ValidationGroup="MonthSearch" SetFocusOnError="True" CssClass="vError">Select To Date</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12">
                    <div class="row">

                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblServices" id="lblMonthUserName"><i class="fas fa-list" aria-hidden="true"></i>User Name<span class="spStar">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlMonthUserName" CssClass=" form-control" TabIndex="5">
                            </asp:DropDownList>
                        </div>

                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblServices" id="lblMonthServices"><i class="fas fa-list" aria-hidden="true"></i>Services<span class="spStar">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlMonthService" CssClass=" form-control" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlMonthService_SelectedIndexChanged" TabIndex="6">
                                <asp:ListItem Value="0">Select the Services</asp:ListItem>
                                <asp:ListItem Value="1">Boating</asp:ListItem>
                                <asp:ListItem Value="2">Restaurant</asp:ListItem>
                                <asp:ListItem Value="3">Other Services</asp:ListItem>
                                <asp:ListItem Value="4">Additional Ticket</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMonthService" InitialValue="0"
                                ValidationGroup="MonthSearch" SetFocusOnError="True" CssClass="vError">Select Services</asp:RequiredFieldValidator>
                        </div>


                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblCategory" id="lblMonthCategory"><i class="fas fa-list" aria-hidden="true"></i>Category<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlMonthCategory" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonthCategory_SelectedIndexChanged" TabIndex="7">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2 col-xs-12" runat="server" id="divMonthType" visible="false">
                            <label for="lblTypes" id="lblMonthTypes"><i class="fas fa-list" aria-hidden="true"></i>Types <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlMonthTypes" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="8">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblCategoryName" id="lblMonthPayment"><i class="fas fa-list" aria-hidden="true"></i>PaymentType<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlMonthPayment" CssClass="form-control inputboxstyle" runat="server" TabIndex="9">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>



                        <div class="col-sm-2 col-xs-12" runat="server">
                            <div class="row">
                                <div class="col-sm-2 col-xs-12" style="padding-right: 4rem; padding-bottom: 1rem; margin-top: 30px;">
                                    <span style="float: left">
                                        <asp:Button ID="btnMonthDtlRpt" runat="server" Text="Detailed Report" class="btn btn-primary" ValidationGroup="MonthSearch" OnClick="btnMonthDtlRpt_Click" TabIndex="12" />
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divAllServices" runat="server" visible="true">
            <div class="row" style="padding-left: 15px; padding-top: 1rem; margin-right: -10px; margin-left: -10px; background-color: #D5F3C7;">
                <div class="col-md-12 col-sm-12">
                    <div class="row">

                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblTypes" id="lbldate"><i class="fa fa-calendar" aria-hidden="true"></i>Booking Date <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtBookingDate" CssClass="form-control frmDate" runat="server"
                                TabIndex="10" AutoPostBack="true" OnTextChanged="txtBookingDate_TextChanged" >
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBookingDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select Booking Date</asp:RequiredFieldValidator>
                        </div>

                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblCategoryName" id="lblCategoryName"><i class="fas fa-list" aria-hidden="true"></i>PaymentType<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlPaymentType" CssClass="form-control inputboxstyle" runat="server" TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblServices" id="lblServices"><i class="fas fa-list" aria-hidden="true"></i>Services<span class="spStar">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlServices" CssClass=" form-control" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlServices_SelectedIndexChanged" TabIndex="6">
                                <asp:ListItem Value="0">Select the Services</asp:ListItem>
                                <asp:ListItem Value="1">Boating</asp:ListItem>
                                <asp:ListItem Value="2">Restaurant</asp:ListItem>
                                <asp:ListItem Value="3">Other Services</asp:ListItem>
                                <%-- <asp:ListItem Value="4">Cash Refund Report</asp:ListItem>--%>
                                <asp:ListItem Value="5">Additional Ticket</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlServices" InitialValue="0"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select Services</asp:RequiredFieldValidator>
                        </div>

                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblServices" id="lblUserName"><i class="fas fa-list" aria-hidden="true"></i>User Name<span class="spStar">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlUserName" CssClass=" form-control" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged">
                            </asp:DropDownList>

                        </div>
                        <div class="col-sm-2 col-xs-12" runat="server">
                            <label for="lblCategory" id="lblCategory"><i class="fas fa-list" aria-hidden="true"></i>Category<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlCategory" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" TabIndex="7">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>


                        <div class="col-sm-2 col-xs-12" runat="server" id="divType" visible="false">
                            <label for="lblTypes" id="lblTypes"><i class="fas fa-list" aria-hidden="true"></i>Types <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlTypes" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="8" AutoPostBack="true" OnSelectedIndexChanged="ddlTypes_SelectedIndexChanged">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>


                        <div class="col-xs-12 col-sm-12">
                            <div class="row">
                                <div class="col-md-12 col-lg-12 col-sm-12" style="padding-right: 4rem; padding-bottom: 1rem;">
                                    <span style="float: right">
                                        <%--  <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Search" OnClick="btnSubmit_Click" TabIndex="11" />--%>
                                        <asp:Button ID="btnautoend" runat="server" Text="Auto NotEnd Trips" class="btn btn-primary" OnClick="btnautoend_Click" TabIndex="16" />
                                        <asp:Button ID="btnDtlRpt" runat="server" Text="Detailed Report" class="btn btn-primary" ValidationGroup="Search" OnClick="btnDtlRpt_Click" TabIndex="12" />
                                        <asp:Button ID="btnAbstractPrint" runat="server" Text="Abstract Print" class="btn btn-primary" ValidationGroup="Search" OnClick="btnAbstractPrint_Click" TabIndex="14" />
                                        <asp:Button ID="btnPrintHistory" runat="server" Text="Print History" class="btn btn-primary" ValidationGroup="Search" OnClick="Go_Click" TabIndex="15" CausesValidation="false" />

                                    </span>

                                    <span style="float: left">
                                        <asp:Button ID="btnClosed" runat="server" Text="Booking Closed" class="btn btn-danger" ValidationGroup="Search" OnClick="btnClosed_Click" TabIndex="16" />
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12 col-xs-12">
                    <div class="row">
                        <div class="col-sm-8">
                            <div class="row" style="padding-left: 100px;">
                                <div class="col-sm-6 col-xs-12" id="divServiceWise" runat="server" visible="false">
                                    <h5 id="hdrCollection" class="pghr" runat="server" visible="false">Collection</h5>
                                    <div class="table-responsive">

                                        <asp:GridView ID="GvServiceWise" runat="server" AllowPaging="false" CssClass="CustomGrid table table-bordered table-condenced"
                                            AutoGenerateColumns="False" DataKeyNames="Particulars,Count" PageSize="20" Width="100%" ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Particulars " HeaderStyle-CssClass="grdHead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblParticulars" runat="server" Text='<%# Bind("Particulars") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="grdHead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcount" runat="server" Text='<%# Bind("Count") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBookingTotalAmount" runat="server" Text=' <%#Eval("TotalAmount","{0:#.00}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="gvHead" />
                                            <AlternatingRowStyle CssClass="gvRow" />
                                            <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                            <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                                        </asp:GridView>

                                        <h5 id="hdrPayment" class="pghr" runat="server" visible="false">Payment</h5>
                                        <asp:GridView ID="GvServiceWisePayments" runat="server" AllowPaging="false" CssClass="CustomGrid table table-bordered table-condenced"
                                            AutoGenerateColumns="False" DataKeyNames="Particulars,Count" PageSize="20" Width="100%" ShowFooter="true" Visible="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Particulars " HeaderStyle-CssClass="grdHead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblParticularsPaid" runat="server" Text='<%# Bind("Particulars") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="grdHead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcountPaid" runat="server" Text='<%# Bind("Count") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBookingTotalAmountPaid" runat="server" Text=' <%#Eval("Amount","{0:#.00}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="gvHead" />
                                            <AlternatingRowStyle CssClass="gvRow" />
                                            <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                            <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                                        </asp:GridView>

                                        <table runat="server" id="tblBtCash" style="width: 100%; border-collapse: collapse; padding: 10px;" class="CustomGrid table table-bordered table-condenced" visible="false">
                                            <tr class="f14CNL" style="border-collapse: collapse;">
                                                <td class="f14CBL" style="width: 70%;">Cash In Counter</td>
                                                <td class="f14CNR" style="width: 30%; text-align: right;">
                                                    <asp:Label ID="Label3" runat="server" CssClass="f14CNR"></asp:Label>Amount</td>
                                            </tr>
                                            <tr class="f14CNL">
                                                <td class="f14CNL">Total Collected Amount</td>
                                                <td class="f14CNR" style="text-align: right;">
                                                    <asp:Label ID="lblReceivedAmount" runat="server" CssClass="f14CNR"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="f14CNL">Paid / Refunded Amount</td>
                                                <td class="f14CNR" style="text-align: right;">
                                                    <asp:Label ID="lblPaidAmount" runat="server" CssClass="f14CNR"></asp:Label></td>
                                            </tr>
                                            <tr class="f14CNL" visible="false">
                                                <td class="f14CNL" style="font-weight: bold;">Balance</td>
                                                <%--Cash In Hand--%>
                                                <td class="f14CNR" style="width: 30%; text-align: right; font-weight: bold;">
                                                    <asp:Label ID="lblBal" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr class="f14CNL">
                                                <td class="f14CNL">Card</td>
                                                <td class="f14CNR" style="width: 30%; text-align: right;">
                                                    <asp:Label ID="lblCard" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr class="f14CNL">
                                                <td class="f14CNL">Online</td>
                                                <td class="f14CNR" style="width: 30%; text-align: right;">
                                                    <asp:Label ID="lblOnline" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr class="f14CNL">
                                                <td class="f14CNL">UPI</td>
                                                <td class="f14CNR" style="width: 30%; text-align: right;">
                                                    <asp:Label ID="lblUPI" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr class="f14CNL">
                                                <td class="f14CNL">Cash In Hand</td>
                                                <td class="f14CNR" style="width: 30%; text-align: right;">
                                                    <asp:Label ID="lblCashInHand" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr class="f14CNL">
                                                <td class="f14CNL" style="font-weight: bold;">Net Amount</td>
                                                <td class="f14CNR" style="width: 30%; text-align: right; font-weight: bold;">
                                                    <asp:Label ID="lblFinalNetAmount" runat="server"></asp:Label></td>
                                            </tr>

                                        </table>

                                        <%--<span id="spnote" runat="server" style="color: red; font-weight: bold;">Note:Click Print History Button Take Print</span>--%>
                                    </div>
                                </div>

                                <div class="col-sm-6 col-xs-12" id="divCashNote" runat="server" visible="false">
                                    <asp:GridView ID="gvRefundCashfrom" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                        ShowFooter="true" RowStyle-Height="25px" HeaderStyle-Height="25px" CssClass="CustomGrid table table-bordered table-condenced" CellPadding="5">

                                        <Columns>
                                            <asp:TemplateField HeaderText="Cash From " HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true" ItemStyle-Width="70%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingId" runat="server" CssClass="f14CNL" Text='<%# Bind("Particulars") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingDate" runat="server" CssClass="f14CNL" Text='<%# Bind("Amount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle HorizontalAlign="Right" CssClass="f14CNR" Height="25px" Font-Bold="true" />
                                    </asp:GridView>

                                    <asp:GridView ID="gvRefundPayAmount" runat="server" AllowPaging="false" AutoGenerateColumns="False" Width="100%"
                                        ShowFooter="true" RowStyle-Height="25px" HeaderStyle-Height="25px" CssClass="CustomGrid table table-bordered table-condenced" CellPadding="5">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Refund Details" HeaderStyle-CssClass="f14CNL" HeaderStyle-Font-Bold="true" ItemStyle-Width="70%">
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

                                    <table runat="server" id="Table8" style="width: 100%; border-collapse: collapse; padding: 10px;" class="CustomGrid table table-bordered table-condenced">
                                        <tr class="f14CNL" style="border-collapse: collapse;">
                                            <td class="f14CBL" style="width: 70%;">Cash In Counter</td>
                                            <td class="f14CNR" style="width: 30%; text-align: right;">
                                                <asp:Label ID="Label1" runat="server" CssClass="f14CNR"></asp:Label>Amount</td>
                                        </tr>
                                        <tr class="f14CNL">
                                            <td class="f14CNL">Total Rcvd From Counter</td>
                                            <td class="f14CNR" style="text-align: right;">
                                                <asp:Label ID="lblRefundTotalFromCounter" runat="server" CssClass="f14CNR"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="f14CNL">Less Refunded Amount</td>
                                            <td class="f14CNR" style="text-align: right;">
                                                <asp:Label ID="lblRefundLessAmount" runat="server" CssClass="f14CNR"></asp:Label></td>
                                        </tr>
                                        <tr class="f14CNL">
                                            <td class="f14CNL">Cash In Hand</td>
                                            <td class="f14CNR" style="width: 30%; text-align: right;">
                                                <asp:Label ID="lblRefundCashInHand" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="col-sm-5 col-xs-12" id="divDenomination" runat="server" visible="false">
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lblIn2000" runat="server" CssClass="form-control" Width="90" Text="2000 X"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt2000" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                onkeyup="calculate2000();calculatetotal();CalculateNoteTotal();ZeroKeyPress2000();" onkeypress="return isNumber(event);"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut2000" runat="server" CssClass="form-control" Width="90"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lbl500" runat="server" CssClass="form-control" Width="90" Text="500 X"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt500" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate500();calculatetotal();CalculateNoteTotal();ZeroKeyPress500();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut500" runat="server" CssClass="form-control" Width="90" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lbl200" runat="server" CssClass="form-control" Width="90" Text="200 X"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt200" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate200();calculatetotal();CalculateNoteTotal();ZeroKeyPress200();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut200" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lbl100" runat="server" CssClass="form-control" Width="90" Text="100 X"></asp:Label>

                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt100" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate100();calculatetotal();CalculateNoteTotal();ZeroKeyPress100();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut100" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lbl50" runat="server" CssClass="form-control" Width="90" Text="50 X"></asp:Label>

                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt50" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate50();calculatetotal();CalculateNoteTotal();ZeroKeyPress50();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut50" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div>
                                            <asp:Label ID="lbl20" runat="server" CssClass="form-control" Width="90" Text="20 X"></asp:Label>

                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt20" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate20();calculatetotal();CalculateNoteTotal();ZeroKeyPress20();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut20" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lbl10" runat="server" CssClass="form-control" Width="90" Text="10 X"></asp:Label>

                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt10" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate10();calculatetotal();CalculateNoteTotal();ZeroKeyPress10();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut10" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lbl5" runat="server" CssClass="form-control" Width="90" Text="5 X"></asp:Label>

                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt5" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate5();calculatetotal();CalculateNoteTotal();ZeroKeyPress5();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut5" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lbl2" runat="server" CssClass="form-control" Width="90" Text="2 X"></asp:Label>

                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt2" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate2();calculatetotal();CalculateNoteTotal();ZeroKeyPress2();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut2" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lbl1" runat="server" CssClass="form-control" Width="90" Text="1 X"></asp:Label>

                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt1" runat="server" CssClass="form-control" Width="90" MaxLength="100" ondrop="return false;"
                                                oncopy="return false" onpaste="return false" oncut="return false"
                                                AutoComplete="Off" onkeyup="calculate1();calculatetotal();CalculateNoteTotal();ZeroKeyPress1();" onkeypress="return isNumber(event)"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:Label ID="txtOut1" runat="server" CssClass="form-control" Width="90" MaxLength="100" AutoComplete="Off"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="Label2" runat="server" CssClass="form-control" Width="90" Text="Total"></asp:Label>

                                        </div>
                                        <div>
                                            <asp:Label ID="lblDenCount" runat="server" CssClass="form-control" Width="90"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:Label ID="lblDenTotalAmt" runat="server" CssClass="form-control" Width="90"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>

                                            <input type="text" id="txtfinalvalue" runat="server" style="display: none" />
                                            <input type="text" id="inptxt2000" runat="server" style="display: none" />
                                            <input type="text" id="inptxt500" runat="server" style="display: none" />
                                            <input type="text" id="inptxt200" runat="server" style="display: none" />
                                            <input type="text" id="inptxt100" runat="server" style="display: none" />
                                            <input type="text" id="inptxt50" runat="server" style="display: none" />
                                            <input type="text" id="inptxt20" runat="server" style="display: none" />
                                            <input type="text" id="inptxt10" runat="server" style="display: none" />
                                            <input type="text" id="inptxt5" runat="server" style="display: none" />
                                            <input type="text" id="inptxt2" runat="server" style="display: none" />
                                            <input type="text" id="inptxt1" runat="server" style="display: none" />

                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-1 col-xs-12">
                                    <asp:Button ID="btnMiniPrint" runat="server" Text="Mini Print" CssClass="btn btn-primary" ValidationGroup="Search"
                                        TabIndex="13" OnClick="btnMiniPrint_Click" Style="height: 40px; margin-top: 353px;" Visible="false" />
                                </div>
                                <span id="spnote" runat="server" style="color: red; font-weight: bold;">Note:Click Print History Button Take Print</span>

                            </div>
                        </div>
                        <div class="col-sm-4" style="padding-left: 0px; padding-right: 100px;">
                            <div class="col-sm-12 col-xs-12" id="divServiceTypeStatus" runat="server" visible="true" style="background-color: #97e4ff8a; padding-top: 20px; border-radius: 15px;">
                                <div class="table-responsive">
                                    <h5 id="hdrServiceTypeStatus" class="pghr" runat="server">Service Type Status</h5>
                                    <asp:GridView ID="GvServiceTypeStatus" runat="server" AllowPaging="false" CssClass="CustomGrid table table-bordered table-condenced"
                                        AutoGenerateColumns="False" DataKeyNames="Particulars,Status" PageSize="20" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Type " HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParticulars" runat="server" Text='<%# Bind("Particulars") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="grdHead">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
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
        </div>
    </div>


    <ajax:ModalPopupExtender ID="MpeTrip" runat="server" BehaviorID="MpeTrip" TargetControlID="HiddenField1" PopupControlID="pnlTrip"
        BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>

    <asp:Panel ID="pnlTrip" runat="server" CssClass="Msgg" Visible="false">
        <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
            <div class="modal-content" style="margin-top: 10px; width: 1100px; min-height: 400px; max-height: 400px; overflow-x: auto; border-color: #203e9e">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel" style="color: #203e9e">Auto Not Ended Trips : 
                       <asp:Label ID="lblRowerSettlementId" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </h5>
                    <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                </div>
                <div class="modal-body">
                    <div class="form-body col-sm-12 col-xs-12">
                        <div>
                        </div>
                        <div class="table-div" id="divGridEndAll" runat="server" style="background-color: #FFFFFF; overflow: hidden; min-height: 400px;" visible="false">
                            <div class="table-responsive" style="overflow: hidden">

                                <div class="table-div" id="divmsgEndAll" runat="server" style="background-color: #FFFFFF;">
                                    <div class="table-responsive" style="overflow: hidden">
                                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                            <asp:Label ID="lblGridMsgEndAll" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="14px"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:GridView ID="gvTripSheetSettelementEndAll" runat="server" AllowPaging="True" CssClass="table table-bordered table-condenced" PagerSettings-Mode="Numeric"
                                    AutoGenerateColumns="False" PageSize="10" DataKeyNames="BoatReferenceNo" OnRowDataBound="gvTripSheetSettelementEndAll_RowDataBound" OnPageIndexChanging="gvTripSheetSettelementEndAll_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Booking Serial" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingSerial" runat="server" Text='<%# Bind("BookingSerial") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="UserId" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boarding Time" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoardingTime" runat="server" Text='<%# Bind("BoardingTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Premium Status" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPremiumStatus" runat="server" Text='<%# Bind("PremiumStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="10%" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblBoatSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatId" runat="server" Text='<%# Bind("BoatId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblBoatNum" runat="server" Text='<%# Bind("BoatNum") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Rower">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowerId" runat="server" Text='<%# Bind("RowerId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="left" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTripStartTime" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="15%" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Duration (Mins)" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingDuration" runat="server" Text='<%# Eval("BookingDuration") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeposit" runat="server" Text='<%# Eval("BoatDeposit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Trip End Time" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTripEndTime" runat="server" Text='<%# Bind("DefaultEndTime") %>'></asp:Label>
                                                <%-- <asp:Label ID="lblTripEndTimeDefault" runat="server" ></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="15%" Font-Bold="true" Font-Size="14px" />
                                        </asp:TemplateField>


                                    </Columns>

                                    <HeaderStyle CssClass="gvHead" BackColor="#124a79" ForeColor="White" />
                                    <AlternatingRowStyle CssClass="gvRow" />
                                    <PagerStyle HorizontalAlign="Center" />

                                </asp:GridView>
                                <div class="d-flex justify-content-center" style="padding-left: 15px; margin-bottom: 5px;" runat="server" id="divAllEnd">
                                    <asp:Button runat="server" ID="btnAllEnd" OnClick="btnAllEnd_Click" Text="END ALL" CssClass="buttonPre block" />
                                </div>

                            </div>
                        </div>
                    </div>

                </div>
                <%-- <div class="modal-footer">
                    <div class="d-flex justify-content-center" style="padding-left: 15px; margin-bottom: -5px;" runat="server" id="divAllEnd">
                    <asp:Button runat="server" ID="btnAllEnd" OnClick="btnAllEnd_Click" Text="END ALL" CssClass="buttonPre block" />
                </div>
                </div>--%>
            </div>
        </asp:Panel>
    </asp:Panel>

    <asp:HiddenField ID="hfDenomination" runat="server" />
    <asp:HiddenField ID="hfServiceName" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

