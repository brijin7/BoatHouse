<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="BookingRestaurantServices.aspx.cs" Inherits="Restaurants_BookingRestaurantServices" %>

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

    <style>
        .btnDisplay {
            display: none;
        }

        .rect {
            min-width: 150px !important;
            max-width: 195px;
            padding: 5px !important;
        }

        .rectangle {
            min-height: 80px;
            max-height: 80px;
            min-width: 150px !important;
            max-width: 180px !important;
            padding: 5px !important;
        }

        .formcontrolborder {
            border: 1px solid transparent;
            margin-top: 4px;
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
            margin-left: 5px !important;
            border-radius: 10px;
            box-shadow: 8px 14px 38px rgba(39, 44, 49, .06), 1px 3px 8px rgba(39, 44, 49, 0.26);
            overflow: auto;
            transition: all .5s ease;
            max-width: 95%;
        }

        .FinalSummary {
            border: 2px solid lightgray;
            border-radius: 15px;
            /*padding: 15px;*/
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
            /* border-radius: 28px;*/
            border: 1px solid #18ab29;
            display: inline-block;
            cursor: pointer;
            color: #0d0c0c;
            font-family: Arial;
            /*font-size: 28px;*/
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
            cursor: pointer;
        }

        .buttonRes {
            background-color: white;
            cursor: pointer;
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
            background-color: #C71585;
        }

        .buttonRes:hover {
            background-color: #18ab29;
            color: white;
        }

        .buttonRes:active {
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
                background-color: #C71585;
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
            font-size: 1.5rem !important;
            line-height: 1.5;
            border-radius: 2.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }

        .payMode {
            /*width: 150px;*/
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

        .textpin {
            font-weight: 300;
            text-align: center;
            height: 50px;
            border: 1px solid transparent;
            padding: 5px 5px;
            font-size: 1.5rem !important;
            border-radius: 3px;
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
                width: 220px;
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
            var Grid = "<%=GvBoatBooking.ClientID%>";
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

            <div class="form-body m-0">
                <div class="row" runat="server" visible="false" id="divShow">
                    <div class="col-sm-12 col-md-12 text-center">
                        <h5 class="pghr" style="text-align: center; font-size: 25px; display: inline;">Restaurant Services </h5>
                        <span style="float: right; display: inline;">
                            <asp:ImageButton ID="imgbtnNewBook" runat="server" ImageUrl="~/images/Ticket.svg" Width="40px" 
                                OnClick="imgbtnNewBook_Click" OnClientClick="StartTimer()" ToolTip="New Booking" CssClass="pr-2" Visible="false" />
                        </span>
                    </div>
                </div>

                <div runat="server" id="divEntry" style="width: 100%">

                    <div class="row">
                        <div class="col-md-2 col-sm-12">
                            <h3 id="Time" style="display: inline; float: left; color: black;"></h3>
                        </div>
                        <div class="col-md-3 col-sm-12">
                            <h5 class="pghr" style="text-align: center; font-size: 23px; display: inline; padding-top: 5px;">Restaurant Services </h5>
                        </div>

                        <div class="col-md-3 col-sm-12 text-center">
                            <a href="#" alt="Bulk Restaurant Services" class="tooltipDemo">
                                <asp:CheckBox runat="server" Style="zoom: 1.5;" Width="17px" Height="21px" ID="ChkBulk" OnCheckedChanged="ChkBulk_CheckedChanged" AutoPostBack="true" /><b style="font-size: 19px; color: black;">Bulk</b>
                            </a>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="#" alt="Customer Mobile No." class="tooltipDemo">
                                <asp:CheckBox runat="server" Style="zoom: 1.5;" Width="17px" Height="21px" ID="chkCustMobileNo" /><b style="font-size: 19px; color: black;">Mobile No.</b>
                            </a>
                        </div>
                        <div class="col-md-4 col-sm-12">
                            <h3 id="Date" style="display: inline; overflow: auto; float: right; padding-left: 20px; color: black;"></h3>
                            <asp:ImageButton ID="imgbtnBookedList" runat="server" Style="float: right" ImageUrl="~/images/Print.svg"
                                Width="35px" OnClick="imgbtnBookedList_Click" OnClientClick="StopTimer()" ToolTip="Ticket Re-Print" />
                        </div>
                    </div>

                    <hr />

                    <div class="col-xs-12" style="background-color: cornsilk">
                        <div class="row">
                            <div class="col-md-6 col-sm-6">
                                <b style="font-size: 18px">Booking Count :</b>
                                <asp:LinkButton ID="bblblCount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;" OnClick="bblblCount_Click"
                                    Font-Underline="true"></asp:LinkButton>
                            </div>
                            <div class="col-md-6 col-sm-6">
                                <b style="font-size: 18px">Net Amount :</b>
                                <asp:LinkButton ID="bblblNetAmount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"
                                    OnClick="bblblNetAmount_Click"
                                    Font-Underline="true"></asp:LinkButton>
                                <%--<asp:Label ID="bblblNetAmount" runat="server" class="pghr" Style="text-align: center; font-size: 18px; display: inline;"></asp:Label>--%>
                            </div>
                        </div>
                    </div>

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


                    <div class="row m-0" runat="server" id="divBack">
                        <div class="col-sm-8" style="margin-top: 10px">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="lblothermsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">

                                    <asp:DataList ID="dtlOther" runat="server" Width="100%" OnItemDataBound="DtlOther_ItemDataBound" RepeatDirection="Horizontal" RepeatColumns="5">
                                        <ItemTemplate>
                                            <div class="dtlBoatType buttonNor rect" style="width: auto; text-align: center; cursor: pointer" onclick="bind('<%# Eval("CategoryId") %>','<%# Eval("CategoryName") %>');">
                                                <h4 style="padding-top: 5px;">
                                                    <asp:Label ID="lblOthCatId" runat="server" Text='<%# Eval("CategoryId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblOthCatName" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </div>
                            </div>

                            <div style="margin-left: auto; margin-right: auto; text-align: center; margin-top: 15px">
                                <asp:Label ID="lblotherchildmsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </div>

                            <div class="row m-0">
                                <div class="col-sm-12">
                                    <div style="margin-top: 15px; overflow-x: scroll; max-height: 500px; overflow-x: auto">
                                        <asp:DataList ID="dtlOtherChild" runat="server" OnItemCommand="dtlOtherChild_ItemCommand"
                                            RepeatColumns="5" RepeatDirection="Horizontal" Style="max-width: 100% !important; min-width: 10% !important">
                                            <ItemTemplate>

                                                <div class="dtlBoatType buttonRes" style="width: 175px !important; text-align: center; padding: 5px">

                                                    <div class="rectangle" style="text-align: center; margin-top: 3px" onclick="add('<%# Eval("ServiceId") %>','<%# Eval("ServiceName") %>', '<%# Eval("ServiceTotalAmount") %>', '<%# Eval("AdultCharge") %>',
                                                        '<%# Eval("ChargePerItemTax") %>', '1', '<%# Eval("TaxId") %>', '<%# Eval("TaxName") %>','<%# Eval("CategoryName") %>','<%# Eval("AvailableQty") %>','<%# Eval("StockEntryMaintenance") %>');">
                                                        <b style="font-size: 20px">
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("ShortName") %>'></asp:Label></b>
                                                        </br>
                                                            <b style="font-size: 20px; color: green">₹<asp:Label ID="lblServiceTotalAmount" runat="server" Text='<%# Eval("ServiceTotalAmount") %>'></asp:Label></b>
                                                        <b style="color: red; font-size: large">
                                                            <asp:Label ID="qty" runat="server" Text="/ Qty :"> </asp:Label>
                                                            <asp:Label ID="lblAvailableQty" runat="server" Text='<%# Eval("AvailableQty") %>'></asp:Label></b>
                                                    </div>



                                                    <div style="display: none;">
                                                        <asp:Label ID="lblCategoryName" runat="server" CssClass="form-control" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                                        <asp:Label ID="lblOthServiceId" runat="server" CssClass="form-control" Text='<%# Eval("ServiceId") %>'></asp:Label>
                                                        <asp:Label ID="lblOthServiceName" runat="server" CssClass="form-control" Text='<%# Eval("ServiceName") %>'></asp:Label>

                                                        <asp:Label ID="lblStockEntryMaintenance" runat="server" CssClass="form-control" Text='<%# Eval("StockEntryMaintenance") %>'></asp:Label>

                                                        <asp:Label ID="lblChargePerItem" runat="server" CssClass="form-control" Text='<%# Eval("AdultCharge") %>'></asp:Label>
                                                        <asp:Label ID="lblChargePerItemTax" runat="server" CssClass="form-control" Text='<%# Eval("ChargePerItemTax") %>'></asp:Label>
                                                        <asp:Label ID="lblTaxId" runat="server" CssClass="form-control" Text='<%# Eval("TaxId") %>'></asp:Label>
                                                        <asp:Label ID="lblTaxName" runat="server" CssClass="form-control" Text='<%# Eval("TaxName") %>'></asp:Label>
                                                    </div>

                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>

                                    </div>
                                </div>
                            </div>
                            <asp:Button ID="btnGet" runat="server" Text="Button" OnClick="btnGet_Click" CssClass="btnDisplay" />
                            <asp:Button ID="btnBind" runat="server" Text="Button" OnClick="btnBind_Click" CssClass="btnDisplay" />
                        </div>


                        <div class="col-sm-4" style="margin-top: 5px;">
                            <div class="row m-0 pt-4 FinalSummary">
                                <div class="col-sm-12">
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

                                <div class="col-sm-12">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th colspan="2" class="text-center table-th">SUMMARY</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <th class="table-th">ITEM CHARGE</th>
                                                <td>₹ <span id="oschar1" runat="server"></span></td>
                                            </tr>
                                            <tr>
                                                <th class="table-th">GST</th>
                                                <td>₹ <span id="bsgst1" runat="server"></span></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="col-sm-12 col-xs-12" style="padding: 15px 0px 0px 0px;" runat="server" id="divCustMobile" visible="false">
                                                        <div class="col-sm-6 col-xs-12 p-0" style="width: 100px;">
                                                            <asp:TextBox ID="txtCustMobileNo" runat="server" placeholder="Mobile No"
                                                                onkeypress="return isNumber(event)" MaxLength="10" CssClass="form-control textpin" Width="200px" BackColor="White"
                                                                ForeColor="Black" BorderColor="Blue" Font-Bold="true" AutoPostBack="true" AutoComplete="off" OnTextChanged="txtCustMobileNo_TextChanged"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="border-top: none;">
                                                    <div class="col-sm-12 input-group-prepend float-right" runat="server" id="divpaymentType" visible="false" style="padding: 0px;">
                                                        <asp:DropDownList ID="ddlPaymentType" CssClass="payMode" runat="server"
                                                            AutoPostBack="false" TabIndex="13">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp;
                                            <asp:Button ID="btnOtherBooking" runat="server" Text="Submit" class="btn btn-primary btnFinal" ValidationGroup="BookingHeader" TabIndex="14"
                                                OnClick="btnOtherBooking_Click" Width="120px" Font-Bold="True" Style="margin-left: 0px;" OnClientClick="showLoader();" />
                                                        &nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger btnFinal" TabIndex="15"
                                                OnClick="btnCancel_Click" Style="margin-left: 0px;" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server" id="div1">
                        <div class="col-sm-7 pl-0 text-center btntbody">
                        </div>
                    </div>
                </div>

                <div class="col-xs-12 table-div" runat="server" id="divGridList" visible="false">
                    <div class="col-xs-12 table-responsive">
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </div>

                        <div style="text-align: right;">
                            <div runat="server" id="Search" style="padding-bottom: 10px">
                                Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="16px" AutoComplete="off" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true" Placeholder="Enter Booking Id"></asp:TextBox>
                            </div>
                            <asp:GridView ID="GvBoatBooking" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="BookingId,Status" PageSize="25000">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>

                                            <asp:Label ID="lblRowNumber" runat="server" Text='<%#Bind("RowNumber") %>'></asp:Label>
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

                                    <asp:TemplateField HeaderText="Item Fare" HeaderStyle-CssClass="grdHead">
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
                            <%-- NewlyAdded--%>
                            <div style="text-align: left;">
                                <asp:Button ID="back" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                                &nbsp
                        <asp:Button ID="Next" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="Next_Click" />

                            </div>
                            <div style="text-align: left;">
                                <asp:Button ID="backSearch" runat="server" CssClass="btn btn-color" Visible="false" Text="← Previous" Enabled="false" OnClick="backSearch_Click" />
                                &nbsp
                        <asp:Button ID="NextSearch" Visible="false" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="NextSearch_Click" />
                                &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />
                            </div>
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
                                                <td class="text-right">
                                                    <h4>Total Items :
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
                                                        <%--<asp:Label ID="lblBillOBookingType" runat="server" Text='<%# Eval("BookingType") %>'></asp:Label>--%>
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
                                                        <asp:Label ID="lblBillOServiceName" runat="server" Text='<%# Eval("ServiceName") %>' Visible="false"></asp:Label>
                                                    </h4>
                                                </td>
                                                <td class="text-right font-weight-bold" style="padding-right: 10px;">
                                                    <h4>Total Items :
                                                        <asp:Label ID="Label24" runat="server" Text='<%# Eval("NoOfItems") %>'></asp:Label>
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
                                </asp:DataList>

                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </div>
            </div>
            <asp:HiddenField ID="hfServiceId" runat="server" />
            <asp:HiddenField ID="hfStock" runat="server" />
            <asp:HiddenField ID="hfServiceName" runat="server" />
            <asp:HiddenField ID="hfServiceTotalAmount" runat="server" />
            <asp:HiddenField ID="hfChargePerItem" runat="server" />
            <asp:HiddenField ID="hfChargePerItemTax" runat="server" />
            <asp:HiddenField ID="hfAdultCount" runat="server" />
            <asp:HiddenField ID="hfTaxId" runat="server" />
            <asp:HiddenField ID="hfTaxName" runat="server" />
            <asp:HiddenField ID="hfCategoryName" runat="server" />
            <asp:HiddenField ID="hfAvailableQty" runat="server" />
            <asp:HiddenField ID="hfBindCategoryId" runat="server" />
            <asp:HiddenField ID="HiddenField3" runat="server" />
            <%-- Newly Added  hfstartvalue,hfendvalue --%>
            <asp:HiddenField ID="hfstartvalue" runat="server" />
            <asp:HiddenField ID="hfendvalue" runat="server" />
            <ajax:DragPanelExtender ID="DragPanelExtender3" runat="server" TargetControlID="pnlUserCounttl" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
            <ajax:ModalPopupExtender ID="MpeUserCount" runat="server" BehaviorID="MpeUserCount" TargetControlID="HiddenField3" PopupControlID="pnlUserCounttl"
                BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>

            <asp:Panel ID="pnlUserCounttl" runat="server" CssClass="Msg" Style="display: none; min-height: 350px; max-height: 620px; width: 1000px; margin-top: 30px;">
                <asp:Panel ID="Panel3" runat="server" CssClass="drag">
                    <div class="modal-content" style="width: 980px; max-height: 600px; min-height: 300px">
                        <div class="modal-header">
                            <h5 class="modal-title">Restaurant Booked Details
                       <asp:Label ID="Label11" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </h5>
                            <asp:ImageButton ID="ImgClose" runat="server" OnClick="ImgClose_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                        </div>
                        <div class="modal-body">

                            <div class="table-responsive">
                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                    <asp:Label ID="Label15" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                                <asp:GridView ID="gvCountTotal" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="10" ShowFooter="true" OnPageIndexChanging="gvCountTotal_PageIndexChanging">
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

                                        <asp:TemplateField HeaderText="Item Fare" HeaderStyle-CssClass="grdHead">
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
            <div id="loader" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8; display: none">
                <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
                    <img src="../images/BoatGIF.gif" />
            </div>
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

        function add(ServiceId, ServiceName, ServiceTotalAmount, ChargePerItem, ChargePerItemTax, AdultCount, TaxId, TaxName, CategoryName, AvailableQty, StockEntryMaintenance) {
            $('#<%=hfServiceId.ClientID %>').val(ServiceId);
            $('#<%=hfServiceName.ClientID %>').val(ServiceName);
            $('#<%=hfServiceTotalAmount.ClientID %>').val(ServiceTotalAmount);
            $('#<%=hfChargePerItem.ClientID %>').val(ChargePerItem);
            $('#<%=hfChargePerItemTax.ClientID %>').val(ChargePerItemTax);
            $('#<%=hfAdultCount.ClientID %>').val(AdultCount);
            $('#<%=hfTaxId.ClientID %>').val(TaxId);
            $('#<%=hfTaxName.ClientID %>').val(TaxName);
            $('#<%=hfCategoryName.ClientID %>').val(CategoryName);
            $('#<%=hfAvailableQty.ClientID %>').val(AvailableQty);
            $('#<%=hfStock.ClientID %>').val(StockEntryMaintenance);
            <%-- if ($('#<%=hfStock.ClientID %>').val() == "Y")

            {
                if($('#<%=hfAdultCount.ClientID %>').val() <= $('#<%=hfAvailableQty.ClientID %>').val() )
                {
                  
         
            
                }
                else
                {
                    alert("Stock Is Not AVailable")
                    return;
                }

            }--%>

            var btnHidden = $('#<%= btnGet.ClientID %>');
            if (btnHidden != null) {
                btnHidden.click();

            }
        }
        function bind(CategoryId, CategoryName) {
            $('#<%=hfBindCategoryId.ClientID %>').val(CategoryId);
            var btnHidden = $('#<%= btnBind.ClientID %>');
            if (btnHidden != null) {
                btnHidden.click();
            }

        }
    </script>

    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

