<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Reports/RptAvilBoatCap.aspx.cs" Inherits="RptAvilBoatCap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />

    <style type="text/css">
        body {
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

        /*tr:nth-child(even) {
            background-color: #f2f2f2;
        }*/

        .otherserv-list-input {
            display: block;
            padding-left: 10px;
        }
    </style>

    <div class="form-body">
        <h5 class="pghr">Available Boats With Capacity</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label>Boat Status</label>
                        <asp:DropDownList ID="ddlBoatStatus" runat="server" CssClass="form-control inputboxstyle"
                            MaxLength="50" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlBoatStatus_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label>Boat Type</label>
                        <asp:DropDownList ID="ddlBoatType" runat="server" CssClass="form-control inputboxstyle"
                            MaxLength="50" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label>Boat Seat</label>
                        <asp:DropDownList ID="ddlBoatSeattype" runat="server" CssClass="form-control inputboxstyle"
                            MaxLength="50" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddlBoatSeattype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label>Boat Name</label>
                        <asp:DropDownList ID="ddlSeatType" runat="server" CssClass="form-control inputboxstyle"
                            MaxLength="50" TabIndex="4" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label>Trip Date</label>
                        <asp:TextBox ID="txtTripDate" runat="server" CssClass=" form-control datepicker" AutoComplete="Off" AutoPostBack="true" OnTextChanged="txtTripDate_TextChanged" TabIndex="1">
                        </asp:TextBox>

                    </div>
                </div>

                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />
                </div>
            </div>
        </div>
        <div class="row p-2">
            <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                <div class="form-group">
                    <div runat="server" id="divAvailSummary" style="overflow: auto; max-width: 400px; min-width: 100px;">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAvailTripSummary" runat="server" AllowPaging="false" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="false" PageSize="3">
                                <columns>
                                    <asp:TemplateField HeaderText="BoatTypeId" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <itemtemplate>
                                            <asp:label id="lblBoatTypeId" runat="server" text='<%# Bind("BoatTypeId") %>'></asp:label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Trip Capacity" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblTripCapacity" runat="server" Text='<%# Bind("TripStartTime") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trip TotalAmount" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblTripAmount" runat="server" Text='<%# Bind("TripTotalAmount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revenue Capacity" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblRevenueCap" runat="server" Text='<%# Bind("RevenueCount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revenue Amount" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblRevenueAmt" runat="server" Text='<%# Bind("RevenueAmount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                </columns>
                                <headerstyle cssclass="gvHead" />
                                <alternatingrowstyle cssclass="gvRow" />
                                <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <br />

            <div class="col-sm-8 col-md-8 col-lg-8 col-xs-12" runat="server">
                <div class="form-group">
                    <div runat="server" id="divAvailAll" visible="false">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAvailall" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="false" DataKeyNames="BoatTypeId" PageSize="10" Width="80%" OnPageIndexChanging="gvAvailall_PageIndexChanging">
                                <columns>

                                    <asp:TemplateField HeaderText="BoatTypeId" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <itemtemplate>
                                            <asp:label id="lblBoatTypeId" runat="server" text='<%# Bind("BoatTypeId") %>'></asp:label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:label id="lblBoatType" runat="server" text='<%# Bind("BoatType") %>'></asp:label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BoatSeaterId" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead" Visible="true">
                                        <itemtemplate>
                                            <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Boat Name" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatName" runat="server" Text='<%# Bind("BoatName") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BoatStatus" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatStatusId" runat="server" Text='<%# Bind("BoatStatus") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Boat Status" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatStatusName" runat="server" Text='<%# Bind("BoatStatusName") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Max Trips/Day" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblMaxTripsPerDay" runat="server" Text='<%# Bind("MaxTripsPerDay") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                </columns>
                                <headerstyle cssclass="gvHead" />
                                <alternatingrowstyle cssclass="gvRow" />
                                <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>




    </div>




    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

