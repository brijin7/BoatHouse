<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="IndividualTripSheetWeb.aspx.cs" Inherits="Boating_IndividualTripSheetWeb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="IndividualTripSheetWeb" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyleBoatStyle.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />
    <script lang="javascript" type="text/javascript">
        var myVar = setInterval(myTimer, 0);
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
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
            document.getElementById("Ind_Date").innerText = date;
            document.getElementById("Ind_Time").innerText = Time;
        }
        //This Method Is Used To Get Array List Of Boat Reference No And Booking Id
        function getBookingIdAndBoatRefNo() {
            let getBookingId;
            let getBoatReferenceNo;
            let getCheckBox;
            let CheckBoxCheckedOrNot;
            let arrBookingid = [];
            let arrBoarRefNo = [];
            const gv = document.querySelector('#<%=gvTripSheetSettelementStart.ClientID %>');
            const getGv = gv.getElementsByTagName('tr');
            let Index = getGv.length - 1;
            for (let i = 1; i <= Index; i++) {
                getCheckBox = getGv[i].getElementsByTagName('INPUT');
                CheckBoxCheckedOrNot = getCheckBox[0].checked;

                if (CheckBoxCheckedOrNot) {
                    getBookingId = getGv[i].cells[1].innerText;
                    arrBookingid.push(getBookingId);
                    getBoatReferenceNo = getGv[i].cells[3].innerText;
                    arrBoarRefNo.push(getBoatReferenceNo);
                }
            }
            document.querySelector('#<%=hfBookingIdArray.ClientID%>').value = arrBookingid;
            document.querySelector('#<%=hfBoatRefNoArray.ClientID%>').value = arrBoarRefNo;
        }


        function clearGvCheckBox() {
            const gv = document.querySelector('#<%=gvTripSheetSettelementStart.ClientID %>');
            getCheckBox = gv.getElementsByTagName('INPUT');
            let Index = getCheckBox.length;
            for (let i = 0; i < Index; i++) {
                if (getCheckBox[i].checked) {
                    getCheckBox[i].checked = false;
                }
            }
        }
        //This Method Is Used Validate No Of ticket Selected.
        function validateCheckedCheckBox(gvr) {
            let seaterType = document.querySelector('#<%=ddlSeaterType.ClientID%>');
            if (typeof (seaterType.options[seaterType.selectedIndex]) == "undefined") {
                infoalert("Please, Select Boat Type");
                return false;
            }
            let seatCount = seaterType.options[seaterType.selectedIndex].text;
            if (seaterType.selectedIndex == "0") {
                infoalert("Please, Select Seater Type");
                return false;
            }
            let count = seatCount.split('-');
            let CheckedCount = 0;
            const gv = document.querySelector('#<%=gvTripSheetSettelementStart.ClientID %>');
            getCheckBox = gv.getElementsByTagName('INPUT');
            let gvRowCount = getCheckBox.length;
            for (let i = 0; i < gvRowCount; i++) {
                if (getCheckBox[i].checked) {
                    CheckedCount++;

                    if (Number(CheckedCount) > Number(count[0])) {
                        infoalert(`Please, Select Less Than Or Equal To ${count[0]}-Tickets`);
                        return false;
                    }
                    $('#<%=lblCount.ClientID%>').html("Count : " + CheckedCount);
                }
                else {
                    $('#<%=lblCount.ClientID%>').html("Count : " + CheckedCount);
                }
            }
        }



    </script>
    <script type="text/javascript">
        function showHourGlass() {
            document.getElementById("HourGlass").style.display = 'block';
        }

    </script>
    <script type="text/javascript">
        function Tooltip(evt) {
            let gvTripEnd = evt.parentNode;
            if (gvTripEnd.querySelector('span').hasAttribute("title")) {
                let gvBookingId = gvTripEnd.querySelector('span').attributes.getNamedItem("title").textContent;
                gvTripEnd.querySelector('span').removeAttribute("title");
                gvTripEnd.querySelector('span').setAttribute('BookingId', gvBookingId)
            }
        }
    </script>
    <style>
        span:hover {
            position: relative;
        }

        span[BookingId]:hover::after {
            content: attr(BookingId);
            padding: 4px 8px;
            color: #333;
            position: absolute;
            left: 0;
            top: 100%;
            white-space: nowrap;
            box-shadow: 0px 0px 4px #222;
            border-radius: 5px;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            -moz-box-shadow: 0px 0px 4px #222;
            -webkit-box-shadow: 0px 0px 4px #222;
            background-image: -moz-linear-gradient(top, #eeeeee, #cccccc);
            background-image: -webkit-gradient(linear,left top,left bottom,color-stop(0, #eeeeee),color-stop(1, #cccccc));
            background-image: -webkit-linear-gradient(top, #eeeeee, #cccccc);
            background-image: -moz-linear-gradient(top, #eeeeee, #cccccc);
            background-image: -ms-linear-gradient(top, #eeeeee, #cccccc);
            background-image: -o-linear-gradient(top, #eeeeee, #cccccc);
        }
    </style>

    <style type="text/css">
        input[type="checkbox"] {
            height: 30px;
            width: 30px;
        }

        .form-body1 {
            margin: 1px 1px;
            background: #ffffff;
            padding: 10px 10px 0px 10px;
            height: 575px;
        }

        .Ind_Bg1 {
            background-color: white;
            overflow: auto;
            max-height: 545px;
            min-height: 490px;
        }

        .table-div1 {
            /* padding: 10px; */
            margin: 10px 15px;
            background: #fff;
        }
    </style>

    <div class="form-body1 ml-1 col-sm-12 col-xs-12">
        <div class="row input-group-prepend">
            <div class="d-flex justify-content-start ind_TripBtn" runat="server" visible="false" id="divTripStart">
                <%--White Trip Start--%>
                <asp:Button
                    runat="server"
                    ID="btnStart"
                    Text="Trip Start"
                    OnClick="btnTripStart_Click"
                    TabIndex="1"
                    CssClass="buttonNor block" />
            </div>
            <div class="d-flex justify-content-center ind_TripBtn" runat="server" visible="false" id="divTripEnd">
                <%--Red Trip End--%>
                <asp:Button
                    runat="server"
                    ID="btnEnd"
                    Text="Trip End"
                    TabIndex="2"
                    OnClick="btnTripEnd_Click"
                    CssClass="buttonPre block" />
            </div>
            <div class="d-flex justify-content-center ind_TripBtn" runat="server" id="divTripClosed">
                <%--Green Trip Closed--%>
                <asp:Button
                    runat="server"
                    ID="btnClosed"
                    Text="Trip Closed"
                    TabIndex="3"
                    OnClick="btnTripClosed_Click"
                    CssClass="buttonClose block" />
            </div>

            <h5 class="pghr ind_TripHdg">Individual Trip Sheet </h5>
            <h5 id="Ind_Date"></h5>
            <h5 id="Ind_Time"></h5>
            <div>
                <asp:Button ID="BackToTripSheet" runat="server" CssClass="btn btn-primary" Width="151px" Height="50px" Font-Bold="true" Style="margin-top: 10px; margin-left: 25px"
                    OnClientClick="showHourGlass();" OnClick="BackToTripSheet_Click" Text="<<Back To Tripsheet" Visible="true" />
            </div>
        </div>
        <%--div Trip Start--%>
        <div class="table-div1 Ind_Bg1" id="divGridStart" runat="server">
            <div class="table-responsive" style="overflow-x: hidden">
                <div class="row">
                    <div class="col-md-4" runat="server" visible="false">
                        <%--Scan Qr Code--%>
                        <asp:Label
                            ID="lblQrCodeStart"
                            runat="server"
                            Font-Bold="true"
                            CssClass="Ind_lblColor">
                            <i class="fa fa-qrcode" aria-hidden="true"></i>Scan QRCode
                        </asp:Label>

                        <asp:TextBox
                            ID="txtStartDetails"
                            runat="server"
                            CssClass="BarCodeTextStart"
                            placeholder="Scan QRCode"
                            Font-Bold="true"
                            AutoComplete="off"
                            Font-Size="Larger"
                            TabIndex="4"
                            AutoPostBack="true">
                        </asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <%--Boat Type--%>
                        <asp:Label
                            ID="lblBoatType"
                            CssClass="Ind_lblColor"
                            Font-Bold="true"
                            runat="server">
                            <i class="fa fa-ship" aria-hidden="true"></i>Boat Type<span class="spStar">*</span>
                        </asp:Label>
                        <asp:DropDownList
                            ID="ddlBoatType"
                            runat="server"
                            OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged"
                            AutoPostBack="true"
                            CssClass="form-control"
                            TabIndex="5">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="rfvBoatType"
                            runat="server"
                            ControlToValidate="ddlBoatType"
                            CssClass="Ind_Rfv"
                            InitialValue="0"
                            ValidationGroup="AddTicket"
                            ErrorMessage="Please, Select Boat Type">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-3">
                        <%--Seater Type--%>
                        <asp:Label
                            ID="lblSeaterType"
                            CssClass="Ind_lblColor"
                            Font-Bold="true"
                            runat="server">
                            <i class="fas fa-chair" aria-hidden="true"></i>Seater Type<span class="spStar">*</span>
                        </asp:Label>
                        <asp:DropDownList
                            ID="ddlSeaterType"
                            runat="server"
                            AutoPostBack="true"
                            onchange="clearGvCheckBox();"
                            CssClass="form-control"
                            TabIndex="6">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="rfvSeaterType"
                            runat="server"
                            ControlToValidate="ddlSeaterType"
                            CssClass="Ind_Rfv"
                            InitialValue="0"
                            ValidationGroup="AddTicket"
                            ErrorMessage="Please, Select Seater Type">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-3">
                        <h5 class="pghr" style="padding-top: 17px">
                            <asp:Label ID="lblCount" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="XX-Large">  </asp:Label>
                        </h5>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        &nbsp&nbsp&nbsp
                        <asp:Label
                            ID="lblStartResponse"
                            CssClass="blink lbl_sMsgPosition"
                            runat="server"
                            Font-Bold="true"
                            ForeColor="Green"
                            Font-Size="Large">
                        </asp:Label>
                    </div>
                    <div class="col-md-12 table-div" id="divMsgStart" runat="server">
                        <div class="table-responsive">
                            <div class="Ind_lblPosition">
                                <asp:Label
                                    ID="lblGridMsgStart"
                                    runat="server"
                                    ForeColor="Green"
                                    Font-Bold="true"
                                    CssClass="blink"
                                    Font-Size="X-Large">
                                </asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-10" runat="server" id="divGridOnlyGridStart" style="max-height: 388px; overflow: auto">
                        <%--GridView Trip Start--%>
                        <asp:GridView
                            ID="gvTripSheetSettelementStart"
                            runat="server"
                            CssClass="CustomGrid table table-bordered table-condenced"
                            AutoGenerateColumns="False"
                            DataKeyNames="BoatReferenceNo,BookingPin"
                            OnRowDataBound="gvTripSheetSettelementStart_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25px" Font-Bold="true" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblBookingId"
                                            runat="server"
                                            Text='<%# Bind("BookingId") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Duration (Mins)" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblBookingDuration"
                                            runat="server"
                                            Text='<%# Eval("BookingDuration") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Reference No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblBoatReferenceNo"
                                            runat="server"
                                            Text='<%# Bind("BoatReferenceNo") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblBookingPin"
                                            runat="server"
                                            Text='<%# Bind("BookingPin") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="QR Code" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Image
                                            ID="imgOtherQRRc"
                                            runat="server"
                                            Width="75px"
                                            Height="75px" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:CheckBox
                                            ID="gvTripStartChkBox"
                                            runat="server"
                                            Font-Size="X-Large"
                                            OnClick="return validateCheckedCheckBox(this)" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="20px" Height="20px" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gvHead" Height="20px" />
                            <AlternatingRowStyle CssClass="gvRow" />
                            <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                            <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                        </asp:GridView>
                    </div>
                    <div class="col-md-2" id="divbtnAdd" runat="server">
                        <span style="float: left; top: 0; margin-top: 5px;">
                            <asp:Button
                                ID="BtnAdd"
                                runat="server"
                                OnClick="BtnAdd_Click"
                                Text="Start the trip"
                                ValidationGroup="AddTicket"
                                CssClass="btn btn-success"
                                Width="100px"
                                TabIndex="7" />

                        </span>
                    </div>
                </div>
            </div>
        </div>
        <%--div Trip End--%>
        <div class="table-div" id="divGridEnd" runat="server" style="background-color: #DB7093; overflow: auto; max-height: 545px; min-height: 485px;" visible="false">
            <div class="table-responsive" style="overflow-x: hidden">
                <div class="row">
                    <div class="col-md-4" runat="server" visible="false">
                        <%--Scan Qr Code--%>
                        <asp:Label
                            ID="lblQrCodeEnd"
                            runat="server"
                            Font-Bold="true"
                            CssClass="Ind_lblColorScanEnd">
                            <i class="fa fa-qrcode" aria-hidden="true"></i>Scan QRCode
                        </asp:Label>
                        <asp:TextBox
                            ID="txtEndDetails"
                            runat="server"
                            placeholder="Scan QRcode"
                            CssClass="BarCodeTextStart"
                            AutoComplete="off"
                            Font-Bold="true"
                            Font-Size="X-Large"
                            AutoPostBack="true">
                        </asp:TextBox>
                    </div>
                    <div class="col-7">
                        &nbsp&nbsp&nbsp
                        <asp:Label
                            ID="lblEndResponse"
                            runat="server"
                            CssClass="blink"
                            Font-Bold="true"
                            ForeColor="White"
                            Font-Size="X-Large">
                        </asp:Label>
                    </div>
                </div>
                <div class="table-div" id="divmsgEnd" runat="server" style="background-color: #DB7093;">
                    <div class="table-responsive">
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label
                                ID="lblGridMsgEnd"
                                runat="server"
                                ForeColor="Green"
                                Font-Bold="true"
                                Font-Size="X-Large">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-10" style="max-height: 430px; overflow: auto">
                        <%--GridView Trip End--%>
                        <asp:GridView
                            ID="gvTripSheetSettelementEnd"
                            runat="server"
                            CssClass="gvv display table table-bordered table-condenced"
                            AutoGenerateColumns="False"
                            DataKeyNames="BookingId">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblBookingId"
                                            runat="server"
                                            Text='<%# Bind("BookingId") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="BoatId" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActualBoatId" runat="server" onmouseover="Tooltip(this)" Text='<%#Eval("ActualBoatId") %>' ToolTip='<%#Eval("BookingId").ToString().Trim() %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActualBoatNum" runat="server" onmouseover="Tooltip(this)" Text='<%#Eval("ActualBoatNum") %>' ToolTip='<%#Eval("BookingId").ToString().Trim() %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Duration (Mins)" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblBookingDuration"
                                            runat="server"
                                            Text='<%# Eval("BookingDuration") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="RowerName" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblRowerName"
                                            runat="server"
                                            Text='<%# Bind("RowerName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblTripStartTime"
                                            runat="server"
                                            Text='<%# Bind("TripStartTime") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:CheckBox
                                            ID="gvTripEndChkBox"
                                            runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rower Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblRowerId"
                                            runat="server"
                                            Text='<%# Bind("RowerId") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>
                    </div>
                    <%--End Button--%>
                    <div class="col-md-2">
                        <span style="float: left; top: 0; margin-top: 5px;">
                            <asp:ImageButton
                                ID="ImgBtnEnd"
                                ForeColor="#512c88"
                                CausesValidation="false"
                                Font-Underline="false"
                                Width="75"
                                OnClick="ImgBtnEnd_Click"
                                CssClass="imgOutLine"
                                runat="server"
                                Font-Bold="true"
                                ImageUrl="~/images/Stop-Icon.png"
                                EnableViewState="false" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <%--div Trip Close  max-height: 650px; min-height: 550px;--%>
        <div class="table-div" id="divGridClosed" runat="server" style="background-color: lightseagreen; overflow: auto; max-height: 545px; min-height: 485px;" visible="false">
            <div class="table-responsive" style="overflow-x: hidden">
                <div class="table-div" id="divmsgClosed" runat="server" style="background-color: lightseagreen;">
                    <div class="table-responsive">
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label
                                ID="lblGridMsgClosed"
                                runat="server"
                                ForeColor="Red"
                                Font-Bold="true"
                                Font-Size="Large">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-10" style="max-height: 430px; overflow: auto">
                        <asp:GridView
                            ID="gvTripSheetSettelementClosed"
                            runat="server"
                            CssClass="gvv display table table-bordered table-condenced"
                            AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblBookingId"
                                            runat="server"
                                            Text='<%# Bind("BookingId") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Actual Boat Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblActualBoatId"
                                            runat="server"
                                            Text='<%# Bind("ActualBoatId") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Boat Num" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>

                                        <asp:Label ID="lblActualBoatNum" runat="server" onmouseover="Tooltip(this)" Text='<%#Eval("ActualBoatNum") %>' ToolTip='<%#Eval("BookingId").ToString().Trim() %>' />

                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Duration (Mins)" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblBookingDuration"
                                            runat="server"
                                            Text='<%# Eval("BookingDuration") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Rower Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblRowerId"
                                            runat="server"
                                            Text='<%# Bind("RowerId") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Rower Name" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblRowerName"
                                            runat="server"
                                            Text='<%# Bind("RowerName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Trip Start Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblTripStartTime"
                                            runat="server"
                                            Text='<%# Bind("TripStartTime") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Trip End Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblTripEndTime"
                                            runat="server"
                                            Text='<%# Bind("TripEndTime") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Travelled Minutes" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblTravelledMinutes"
                                            runat="server"
                                            Text='<%# Bind("TravelledMinutes") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="25px" VerticalAlign="Middle" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TemplateField>


                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfRowerPopUp" runat="server" />
        <ajax:ModalPopupExtender
            ID="PopUdRowerBoatNo"
            runat="server"
            BehaviorID="PopUdRowerBoatNo"
            TargetControlID="hfRowerPopUp"
            PopupControlID="pnlTrip"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>
        <div id="modPopUpVisible" runat="server" style="display: none;">
            <asp:Panel ID="pnlTrip" runat="server">
                <div class="modal-content Ind_Popup">
                    <div class="modal-header" style="background-color: #124a79;">
                        <asp:Label
                            ID="ModalHeader"
                            CssClass="sp2 gvHead Ind_lblMargin"
                            runat="server"
                            Font-Bold="true"
                            ForeColor="White"
                            Text="Boat Number & Rower">
                        </asp:Label>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-8 offset-md-2">
                                <asp:Label
                                    ID="lblBoatNo"
                                    CssClass="Ind_lblColor"
                                    runat="server">
                                </asp:Label>
                                <i class="fa fa-ship"></i>Boat No<span class="spStar">*</span>
                                <asp:DropDownList
                                    ID="ddlBoatNo"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvBoatNo"
                                    runat="server"
                                    ControlToValidate="ddlBoatNo"
                                    CssClass="Ind_Rfv"
                                    InitialValue="0"
                                    ValidationGroup="BoatNoAndRower"
                                    ErrorMessage="Please, Select Boat No.">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8 offset-md-2">
                                <asp:Label
                                    ID="lblRower"
                                    CssClass="Ind_lblColor"
                                    runat="server">
                                </asp:Label>
                                <i class="fa fa-user"></i>Rower<span class="spStar">*</span>
                                <asp:DropDownList
                                    ID="ddlRower"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvRower"
                                    runat="server"
                                    ControlToValidate="ddlRower"
                                    CssClass="Ind_Rfv"
                                    InitialValue="0"
                                    ValidationGroup="BoatNoAndRower"
                                    ErrorMessage="Please, Select Rower">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8 offset-md-2">
                                <span style="float: right;">
                                    <asp:Button
                                        ID="btnPopUpOkay"
                                        runat="server"
                                        ValidationGroup="BoatNoAndRower"
                                        Text="Okay"
                                        OnClick="btnPopUpOkay_Click"
                                        CssClass="btn btn-primary" />
                                    <asp:Button
                                        ID="btnPopUpCancel"
                                        runat="server"
                                        Text="Cancel"
                                        CausesValidation="false"
                                        OnClick="btnPopUpCancel_Click"
                                        CssClass="btn btn-danger" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField ID="hfBookingIdArray" runat="server" />
    <asp:HiddenField ID="hfBoatRefNoArray" runat="server" />
    <asp:HiddenField ID="hfCheckedCount" runat="server" />
    <div id="HourGlass" style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #0000007d; opacity: 0.8; display: none">
        <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #FFFFFF; font-size: 30px; left: 40%; top: 40%; border-radius: 50px;">
            <img src="../images/hourglass.gif" width="100px" height="100px" />
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

