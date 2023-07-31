<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    CodeFile="~/Boating/ViewBoatHouseMaster.aspx.cs" Inherits="Department_Boating_ViewBoatHouseMaster" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .panelheadchk {
            height: 41px;
        }

        .panelbodychk {
            height: 31px;
        }

        #map {
            height: 450px;
            width: 400px;
            border: groove;
            color: black;
        }

        .chkboxForAutoEnd {
            margin-left: 12px;
        }
    </style>

    <div class="form-body">
        <h5 class="pghr">Boat House Master </h5>
        <hr />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-9">
                        <div class="row">
                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server" visible="false">
                                <div class="form-group">
                                    <label runat="server" id="Label14"><i class="fa fa-ship" aria-hidden="true"></i>Corp Id</label>
                                    <asp:TextBox ID="txtCorpId" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                        MaxLength="2" Font-Size="14px" TabIndex="1">
                                    </asp:TextBox>
                                    <label runat="server" id="Label3"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Id</label>
                                    <asp:TextBox ID="txtBoatId" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                        MaxLength="2" Font-Size="14px" TabIndex="1">
                                    </asp:TextBox>


                                </div>
                            </div>

                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label4"><i class="fa fa-map-marker" aria-hidden="true"></i>Location <span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlLocation" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="false" TabIndex="1">
                                        <asp:ListItem Value="0"> Select Location</asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLocation"
                                        ValidationGroup="BoatHouseName" InitialValue="0" SetFocusOnError="True" CssClass="vError">Select Location</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label2"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Name <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txthousename" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                        MaxLength="300" Font-Size="14px" TabIndex="2">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txthousename" ForeColor="Red"
                                        ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="InValid!!" CssClass="vError" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txthousename"
                                        ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter  House Name</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                <div class="form-group">
                                    <label runat="server"><i class="fa fa-user" aria-hidden="true"></i>Boat House Manager <span class="spStar">*</span></label>

                                    <asp:DropDownList ID="ddlHouseManager" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="false" TabIndex="3">
                                        <asp:ListItem Value="0">Select Boat House Manager</asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlHouseManager"
                                        ValidationGroup="BoatHouseName" InitialValue="0" SetFocusOnError="True" CssClass="vError">Select Boat House Manager</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server" id="divAdd1">
                                <div class="form-group">
                                    <label runat="server" class="boataddress"><i class="fa fa-home"></i>Address 1 <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                        MaxLength="100" Font-Size="14px" TabIndex="4">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtAddress1" ForeColor="Red"
                                        ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="InValid!!" CssClass="vError" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtAddress1"
                                        ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Address 1</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server" id="divAdd2">
                                <div class="form-group">
                                    <label runat="server" class="boataddress"><i class="fas fa-store-alt"></i>Address 2 <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtaddress2" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                        MaxLength="100" Font-Size="14px" TabIndex="5">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtaddress2" ForeColor="Red"
                                        ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="InValid!!" CssClass="vError" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtaddress2"
                                        ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Address 2</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" class="boataddress"><i class="fas fa-map-pin"></i>Zip Code <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtZipcode" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto" AutoPostBack="true"
                                        MaxLength="6" Font-Size="14px" TabIndex="6" OnTextChanged="txtZipcode_TextChanged" onkeypress="return isNumber(event)">
                                    </asp:TextBox>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtZipcode"
                                        ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Zip Code</asp:RequiredFieldValidator>
                                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtZipcode" runat="server" Font-Size="Small"
                                        ErrorMessage="Only Numbers allowed and Exactly 6 Digit Number" ValidationExpression="^[0-9]{6}$" ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError"></asp:RegularExpressionValidator>--%>
                                </div>

                            </div>


                        </div>
                        <div id="divCityDisState" runat="server">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label runat="server" class="boataddress"><i class="fas fa-city"></i>City <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                            MaxLength="100" Font-Size="14px" TabIndex="7" Enabled="false">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtCity"
                                            ValidationExpression="[a-zA-Z ]*$" ErrorMessage="InValid!!" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtCity"
                                            ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter City</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label runat="server" class="boataddress"><i class="fas fa-university"></i>District <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                            MaxLength="50" Font-Size="14px" TabIndex="8" Enabled="false">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtDistrict"
                                            ValidationExpression="[a-zA-Z ]*$" ErrorMessage="InValid!!" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtDistrict"
                                            ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter District</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label runat="server" class="boataddress"><i class="fas fa-landmark"></i>State <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                            MaxLength="50" Font-Size="14px" TabIndex="9" Enabled="false">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtState"
                                            ValidationExpression="[a-zA-Z ]*$" ErrorMessage="InValid!!" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtState"
                                            ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter State</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div id="divfromto" runat="server">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblBkfrm" id="Label6" runat="server">
                                            <i class="fa fa-calendar"
                                                aria-hidden="true"></i>Email-Id</label>
                                        <div id="Div2" runat="server">
                                            <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="10">
                                            </asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailId"
                                                ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                Display="Dynamic" ErrorMessage="Invalid !!" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEmailId"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Email-Id</asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblBkfrm" id="lblBkfrm" runat="server">
                                            <i class="fa fa-calendar"
                                                aria-hidden="true"></i>Booking From <span class="spStar">*</span></label>
                                        <div id="mintime" runat="server">

                                            <asp:TextBox ID="txtBookingFrm" TextMode="Time" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="11"
                                                OnTextChanged="txtBookingFrm_TextChanged1" AutoPostBack="false">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtBookingFrm"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Select Booking From</asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblBkTo" id="Label1" runat="server">
                                            <i class="fa fa-calendar"
                                                aria-hidden="true"></i>Booking To <span class="spStar">*</span></label>
                                        <div id="Div1" runat="server">
                                            <asp:TextBox ID="txtBookngTo" TextMode="Time" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="12" OnTextChanged="txtBookngTo_TextChanged" AutoPostBack="false">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBookngTo"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Select Booking To</asp:RequiredFieldValidator>
                                            <input type="hidden" id="hftodate" runat="server" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div id="divMaxMinChild" runat="server">
                            <div class="row">

                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server">
                                    <div class="form-group">
                                        <label runat="server" id="Label7">
                                            <i class="fas fa-baby"></i>
                                            <asp:CheckBox ID="ChkChildApplicable" runat="server" OnCheckedChanged="ChkChildApplicable_CheckedChanged" AutoPostBack="true" />&nbsp Child Applicable</label>
                                    </div>

                                </div>

                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" id="divChild" runat="server" visible="false">
                                    <div class="form-group">
                                        <label runat="server">Maximum Child Age</label>
                                        <asp:TextBox ID="txtMaxChildage" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                            MaxLength="2" Font-Size="14px" TabIndex="13" onkeyup="this.value = minmax(this.value, 0, 20)" placeholder="Max Child Age Is 20">
                                        </asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" id="divInfant" runat="server" visible="false">
                                    <div class="form-group">
                                        <label runat="server">Maximum Infant Age</label>
                                        <asp:TextBox ID="txtMaxInfa" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                            MaxLength="1" Font-Size="14px" TabIndex="14" onkeyup="this.value = minmax(this.value, 0, 9)" placeholder="Max Infant Age Is 9">
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="divCheckWorkingdays" runat="server">

                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblBkfrm" id="Label8" runat="server">
                                            <i class="fa fa-calendar"
                                                aria-hidden="true"></i>GST Number <span class="spStar">*</span></label>
                                        <div id="Div3" runat="server">
                                            <asp:TextBox ID="txtGSTNumber" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="15" MaxLength="25">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtGSTNumber"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter GST Number</asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-9 col-md-9 col-lg-9 col-xs-12">
                                    <div class="panel panel-info">
                                        <div class="panel-heading panelheadchk">
                                            <div class="row">
                                                <div class="col-md-6 col-lg-6 col-sm-12">
                                                    <label runat="server" id="Label5">Working Days <span class="spStar">*</span></label>
                                                    <asp:CheckBox ID="chkSelectAll" Text="Select All" runat="server" Font-Bold="True" CssClass="chkbox checkbox-primary" TabIndex="16" />
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="col-md-6 col-xs-6 chkbox checkbox-primary" style="padding-left: 12px;">
                                                <asp:CheckBoxList ID="chkWorkingDays" runat="server" RepeatDirection="Horizontal" CssClass="styled" RepeatColumns="7" CellPadding="7" CellSpacing="7" TabIndex="17">
                                                    <asp:ListItem Value="1">Sunday</asp:ListItem>
                                                    <asp:ListItem Value="2">Monday</asp:ListItem>
                                                    <asp:ListItem Value="3">Tuesday</asp:ListItem>
                                                    <asp:ListItem Value="4">Wednesday</asp:ListItem>
                                                    <asp:ListItem Value="5">Thursday</asp:ListItem>
                                                    <asp:ListItem Value="6">Friday</asp:ListItem>
                                                    <asp:ListItem Value="7">Saturday</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblBkfrm" id="Label9" runat="server">
                                            <i class="fa fa-clock"
                                                aria-hidden="true"></i>Trip Start Alert Time <span class="spStar">*</span></label>
                                        <div id="Div4" runat="server">
                                            <asp:TextBox ID="txtTripStartAlertMsg" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" AutoComplete="Off" TabIndex="17" MaxLength="10">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTripStartAlertTime" runat="server" ControlToValidate="txtTripStartAlertMsg"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Trip Start Alert Time</asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblBkfrm" id="Label10" runat="server">
                                            <i class="fa fa-clock"
                                                aria-hidden="true"></i>Trip End Alert Time <span class="spStar">*</span></label>
                                        <div id="Div5" runat="server">
                                            <asp:TextBox ID="txtTripEndAlertMsg" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" AutoComplete="Off" TabIndex="18" MaxLength="10">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTripEndAlertTime" runat="server" ControlToValidate="txtTripEndAlertMsg"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Trip End Alert Time</asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblBkfrm" id="Label11" runat="server">
                                            <i class="fa fa-clock"
                                                aria-hidden="true"></i>Refund Duration <span class="spStar">*</span></label>
                                        <div id="Div6" runat="server">
                                            <asp:TextBox ID="txtRefundDuration" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" AutoComplete="Off" TabIndex="19" MaxLength="10">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtRefundDuration"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Refund Duration</asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <div class="form-group">
                                        <label for="lblBkfrm" id="Label12" runat="server">
                                            <i class="fa fa-clock"
                                                aria-hidden="true"></i>Reprint Time <span class="spStar">*</span></label>
                                        <div id="Div7" runat="server">
                                            <asp:TextBox ID="txtReprintTime" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" AutoComplete="Off" TabIndex="20" MaxLength="10">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtReprintTime"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Reprint Time</asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblBhShortcode" id="Label13" runat="server">
                                        <i class="fab fa-wordpress" style="color: #0268ba;"></i>&nbsp;&nbsp;BH Short Code <span class="spStar">*</span></label>
                                    <div id="Div8" runat="server">
                                        <asp:TextBox ID="txtBHShortCode" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="20" MaxLength="4">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtBHShortCode"
                                            ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter BHShortCode</asp:RequiredFieldValidator>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 chkbox checkbox-primary" style="padding-left: 30px; padding-top: 20px;">
                                <asp:CheckBox ID="ChkAutoEndForNoDeposite" Text="Auto End For No-Deposite Boat" runat="server"
                                    CssClass="chkboxForQRcode" RepeatColumns="7" CellPadding="7" CellSpacing="7" TabIndex="17" />
                            </div>


                            <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 chkbox checkbox-primary" style="padding-left: 10px; padding-top: 20px;">
                                <asp:CheckBox ID="ChkForQRcodeGenerate" Text="QRcode Generate in Restaurant Ticket" runat="server"
                                    CssClass="chkboxForQRcode" RepeatColumns="7" CellPadding="7" CellSpacing="7" TabIndex="17" />
                            </div>
                        </div>

                        <div class="panel panel-success">
                            <div class="panel-heading panelheadchk" style="background-color: #e1f5b2 !important;">
                                <div class="row p-0 m-0">
                                    <div class="col-sm-06 col-xs-12">
                                        Boat Extension Details
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body" style="background-color: #f5ffdf !important;">
                                <div class="row p-0 m-0">
                                    <%--  New Extension Charges--%>

                                    <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12  chkbox checkbox-primary " style="padding-left: 50px; padding-top: 10px;">
                                        <asp:CheckBox ID="ChkExtnChargeStatus" Text="Extension Charge" runat="server" AutoPostBack="true" OnCheckedChanged="ChkExtnChargeStatus_CheckedChanged"
                                            CssClass="chkboxForQRcode" RepeatColumns="7" CellPadding="7" CellSpacing="7" TabIndex="17" />
                                    </div>
                                    <%--  New Extension Print--%>
                                    <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12  chkbox checkbox-primary " style="padding-left: 10px; padding-top: 10px;">
                                        <asp:CheckBox ID="ChkExtnPrint" Text="Extension Print" runat="server" AutoPostBack="true" OnCheckedChanged="ChkExtnPrint_CheckedChanged"
                                            CssClass="chkboxForQRcode" RepeatColumns="7" CellPadding="7" CellSpacing="7" TabIndex="17" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="divMapView" runat="server">
                        <div class="col-sm-3" style="margin-left: -13rem">
                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server" id="mapview" visible="false">
                                <div class="form-group">
                                    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB56Km4bH3DEKxXLRZBltsTIm3eYgPqt0k&callback=Function.prototype" type="text/javascript"></script>
                                    <div id="map">

                                        <script type="text/javascript">
                                            var zipcode = document.getElementById("<%=txtZipcode.ClientID%>").value;
                                            var mapcode = new google.maps.Geocoder();
                                            var initialize;
                                            google.maps.visualRefresh = false;

                                            mapcode.geocode({ 'address': zipcode }, function (results, status) {
                                                if (status == google.maps.GeocoderStatus.OK) {
                                                    lat = results[0].geometry.location.lat();
                                                    lng = results[0].geometry.location.lng();

                                                } else {

                                                    document.getElementById('<%= txtAddress1.ClientID%>').value = ' ';
                                                    document.getElementById('<%= txtaddress2.ClientID%>').value = ' ';
                                                }
                                                var map = new google.maps.Map(document.getElementById('map'), {
                                                    zoom: 6,
                                                    center: new google.maps.LatLng(lat, lng),
                                                    mapTypeId: google.maps.MapTypeId.ROADMAP
                                                });

                                                var marker = new google.maps.Marker({
                                                    position: new google.maps.LatLng(lat, lng),
                                                    map: map,

                                                });
                                                google.maps.event.addListener(map, 'click', function (e) {

                                                    var latlng = new google.maps.LatLng(e.latLng.lat(), e.latLng.lng());

                                                    google.maps.event.addListener(map, 'click', function (event) {
                                                        marker.setPosition(event.latLng);
                                                    });

                                                    var geocoder = geocoder = new google.maps.Geocoder();
                                                    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                                                        if (status == google.maps.GeocoderStatus.OK) {
                                                            if (results[0]) {
                                                                var control = results[0].formatted_address;
                                                                const address = control.split(",");

                                                                var add1 = address[0] + address[1];
                                                                var add2 = address[2];
                                                                document.getElementById('<%= txtAddress1.ClientID%>').value = add1;
                                                                document.getElementById('<%= hfAddres1.ClientID %>').value = add1;
                                                                document.getElementById('<%= txtaddress2.ClientID%>').value = add2;
                                                                document.getElementById('<%= hfAddres2.ClientID%>').value = add2;

                                                            }
                                                        }
                                                        for (var component in results[0]['address_components']) {
                                                            for (var i in results[0]['address_components'][component]['types']) {
                                                                if (results[0]['address_components'][component]['types'][i] == "administrative_area_level_1") {

                                                                    state = results[0]['address_components'][component]['long_name'];

                                                                    document.getElementById('<%= txtState.ClientID%>').value = state;

                                                                }


                                                            }
                                                        }


                                                        var address = results[0].address_components;
                                                        var zipcode = '';
                                                        for (var i = 0; i < address.length; i++) {
                                                            if (address[i].types.includes("postal_code")) {
                                                                zipcode = address[i].short_name;
                                                                document.getElementById('<%= txtZipcode.ClientID%>').value = zipcode;

                                                            }

                                                        }
                                                        fetch('https://api.postalpincode.in/pincode/' + zipcode)
                                                            .then(response => response.json())
                                                            .then(data => {
                                                                var json = JSON.stringify(data);
                                                                var obj1 = JSON.parse(json);
                                                                var arrayLength = obj1.length;
                                                                for (var i = 0; i < arrayLength; i++) {

                                                                    var Cityblock = obj1[i].PostOffice[i].Block
                                                                    var District = obj1[i].PostOffice[i].District
                                                                    if (Cityblock == "NA") {
                                                                        continue;
                                                                    }
                                                                    if (District == "NA") {
                                                                        continue;
                                                                    }
                                                                    document.getElementById('<%= txtCity.ClientID%>').value = Cityblock;
                                                                    document.getElementById('<%= txtDistrict.ClientID%>').value = District;

                                                                }

                                                            })
                                                            .catch(error => console.error(error))

                                                    })
                                                });

                                            });
                                            window.addEventListener('load', initialize);

                                        </script>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-submit">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BoatHouseName" TabIndex="20" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger" TabIndex="21" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvBoatHouse" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="CorpId,BoatHouseId,ReprintTime,AutoEndForNoDeposite,QRcodeGenerate,ExtensionPrint,ExtnChargeStatus" PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="CorpId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCorpId" runat="server" Text='<%# Bind("CorpId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatHouseId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat House Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatLocationId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatLocnId" runat="server" Text='<%# Bind("BoatLocnId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatLocnName" runat="server" Text='<%# Bind("BoatLocnName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="House Manager" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseMngId" runat="server" Text='<%# Bind("BoatHouseManager") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Manager Name" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblboatHouseMngName" runat="server" Text='<%# Bind("BoatHouseManagerUserName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking From" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingFrom" runat="server" Text='<%# Bind("BookingFrom") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking To" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBookingTo" runat="server" Text='<%# Bind("BookingTo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Working Days" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkingDays" runat="server" Text='<%# Bind("WorkingDays") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat House Address" HeaderStyle-CssClass="grdHead">

                            <ItemTemplate>
                                <asp:Label ID="lblBHAddress" runat="server" Text='<%#Eval("Address1")+ " " + Eval("Address2")+" " + Eval("City")+ " " + Eval("District")+" "+Eval("State")+" "+ Eval("ZipCode")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address 1" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress1" runat="server" Text='<%# Bind("Address1") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address 2" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress2" runat="server" Text='<%# Bind("Address2") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="District" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDistrict" runat="server" Text='<%# Bind("District") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="State" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ZipCode" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblZipCode" runat="server" Text='<%# Bind("ZipCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="MailId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblmailid" runat="server" Text='<%# Bind("MailId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="MaxChildAge" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMaxChildAge" runat="server" Text='<%# Bind("MaxChildAge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MaxInfantAge" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMaxInfantAge" runat="server" Text='<%# Bind("MaxInfantAge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TaxId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxID" runat="server" Text='<%# Bind("TaxID") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GSTNumber" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblGSTNumber" runat="server" Text='<%# Bind("GSTNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TripStartAlertTime" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTripStartAlertTime" runat="server" Text='<%# Bind("TripStartAlertTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TripEndAlertTime" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTripEndAlertTime" runat="server" Text='<%# Bind("TripEndAlertTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Refund Duration" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRefundDuration" runat="server" Text='<%# Bind("RefundDuration") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BHShortCode" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBHShortCode" runat="server" Text='<%# Bind("BHShortCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnDelete" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Delete.png" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this entry?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfUserId" runat="server" />
    <asp:HiddenField ID="hfHouseAddress" runat="server" />
    <asp:HiddenField runat="server" ID="hfCreatedBy" />
    <asp:HiddenField runat="server" ID="hfBoatHouseId" />
    <asp:HiddenField runat="server" ID="hfAddres1" />
    <asp:HiddenField runat="server" ID="hfAddres2" />



    <script type="text/javascript">

        function minmax(value, min, max) {
            if (parseInt(value) < min || isNaN(parseInt(value)))
                return min;
            else if (parseInt(value) > max)
                return max;
            else return value;
        }


        $(function checkUncheck() {
            $("[id*=chkSelectAll]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=chkWorkingDays] input").prop("checked", true);
                }
                else {
                    $("[id*=chkWorkingDays] input").prop("checked", false);
                }
            });

            $("[id*=chkWorkingDays] input").bind("click", function () {
                if ($("[id*=chkWorkingDays] input:checked").length == $("[id*=chkWorkingDays] input").length) {
                    $("[id*=chkSelectAll]").prop("checked", true);
                    // $("[id*=chkSelectAll]").attr("checked", "checked");
                }
                else {

                    $("[id*=chkSelectAll]").prop("checked", false);
                }


            });
        });
    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

