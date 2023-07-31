<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="KioskOtherService.aspx.cs" Inherits="Boating_KioskOtherService" %>

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
    <script>
        function SendOtp() {
            let seconds = 31;
            let button = document.querySelector('#<%=BtnResendSMS.ClientID%>');
            function incrementSeconds() {
                seconds = seconds - 1;
                if (seconds < 10) {
                    button.value = '00:0' + seconds;
                    button.disabled = true;
                }
                else {
                    button.value = '00:' + seconds;
                    button.disabled = true;
                }
                if (seconds == 0) {
                    seconds = 31;
                    button.value = "ReSend SMS";
                    clearInterval(cancel);
                    button.disabled = false;
                }
            }
            var cancel = setInterval(incrementSeconds, 1000);
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

    <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-body">

                <div runat="server" id="divEntry" style="width: 100%">
                    <div class="row">
                        <div class="col-md-4 col-sm-4">
                            <h3 id="Time" style="display: inline; float: left; color: black;"></h3>
                        </div>
                        <div class="col-md-4 col-sm-4 text-center">
                            <h5 class="pghr" style="text-align: center; font-size: 25px; display: inline;">Self Other Services </h5>
                        </div>
                    </div>

                    <hr />
                    <div class="row" runat="server" id="divBack">
                        <div class="col-sm-7 col-xs-12" style="margin-top: 10px">

                            <asp:DataList ID="dtlOther" runat="server" Width="100%" OnItemDataBound="DtlOther_ItemDataBound">
                                <HeaderTemplate>
                                    <div class="row" style="padding-left: 50px; padding-bottom: 30px;">
                                        <div class="col-sm-4">
                                            <h3>Category</h3>
                                        </div>
                                        <div class="col-sm-8">
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
                                            <tr runat="server">
                                                <th class="table-th">Total</th>
                                                <td>₹ <span id="bsTotal" runat="server" style="color: red; font-weight: bolder"></span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-sm-12 col-xs-12" style="border-top: 1px solid #dee2e6; padding: 15px 0px 0px 0px;">
                                    <div class="row m-0 p-0" runat="server" id="trMobileNo" visible="false">
                                        <div class="col-sm-6 col-xs-12 p-0" style="width: 100px">
                                            <asp:TextBox ID="txtMobileNo" runat="server" placeholder="Mobile No"
                                                onkeypress="return isNumber(event)" MaxLength="10" CssClass="form-control textpin" Width="190px" BackColor="White"
                                                ForeColor="Black" BorderColor="Blue" Font-Bold="true" AutoPostBack="true" AutoComplete="off"
                                                OnTextChanged="txtMobileNo_TextChanged"></asp:TextBox>
                                        </div>



                                        <div class="col-sm-6 col-xs-12 p-0" id="divDetails" runat="server" visible="false">
                                            <div>
                                                <asp:Label ID="lblName" runat="server" ForeColor="#124a79" BorderColor="Blue" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:Label ID="lblEmailId" runat="server" ForeColor="#124a79" BorderColor="Blue" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 p-0 text-right" id="divOtpbtn" runat="server" visible="false">
                                            <asp:Button ID="btnOTP" runat="server" Text="Send OTP" class="btn btn-primary" OnClick="btnOTP_Click"
                                                Font-Bold="True" />
                                        </div>
                                        <div class="col-sm-4 col-xs-12 p-0" id="divOtptxt" runat="server" visible="false">
                                            <asp:TextBox ID="txtOTP" runat="server" placeholder="OTP"
                                                onkeypress="return isNumber(event)" MaxLength="4" CssClass="btn btn-primary btnPin" Width="100px" BackColor="White"
                                                ForeColor="Black" BorderColor="Blue" Font-Bold="true" AutoPostBack="true" AutoComplete="off"
                                                OnTextChanged="txtOTP_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2 col-xs-12" id="divImgStatus" runat="server" visible="false">
                                            <asp:Image ID="imgPinStatus" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-12 col-xs-12 p-0" id="btnResend" runat="server"
                                    style="margin-top: 10px; text-align: center" visible="false">
                                    <asp:Button ID="BtnResendSMS" runat="server" Text="ReSend SMS" class="btn btn-primary" TabIndex="4" OnClick="BtnResendSMS_Click" />
                                </div>

                                <div class="col-sm-12 col-xs-12" style="padding: 15px 0px 0px 0px;">
                                    <div class="row m-0 p-0" runat="server" id="divBooking" visible="false" style="width: 100%;">
                                        <div class="col-sm-4 col-xs-12 p-0 text-center">
                                            <asp:DropDownList ID="ddlPaymentType" CssClass="payMode" runat="server"
                                                AutoPostBack="false" TabIndex="13">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 p-0 text-center">
                                            <span class="htmlHigh">
                                                <div class="tooltip-ex">
                                                    <asp:Button ID="btnOtherBooking" runat="server" Text="Submit" class="btn btn-primary btnFinal" ValidationGroup="BookingHeader" TabIndex="14"
                                                        OnClick="btnOtherBooking_Click" Width="150px" Font-Bold="True" Style="margin-left: 10px;" />
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

            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnOtherBooking" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UPprocessServer" runat="Server" AssociatedUpdatePanelID="UpdatePanel">
    </asp:UpdateProgress>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

