<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Reports/RptRowerCharges.aspx.cs" Inherits="Boating_RptRowerCharges" %>

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
        <h5 class="pghr">Rower Charges</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row p-2">

                <div class="col-sm-3 col-xs-12">
                    <label for="lblBoatType" id="lblboattype"><i class="fa fa-ship" aria-hidden="true"></i>Boat Type <span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlBoatType" CssClass="form-control inputboxstyle" runat="server"
                        TabIndex="2">
                        <asp:ListItem Value="All">All</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlBoatType"
                        ValidationGroup="RowerCharge" SetFocusOnError="True" InitialValue="Select Boat Type" CssClass="vError">
                        Select Boat Type</asp:RequiredFieldValidator>
                </div>


                <div class="col-sm-3 col-xs-12">
                    <label for="ddlrowerName1" id="ddlrowerName1"><i class="fa fa-ship" aria-hidden="true"></i>Rower Name <span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlrowerName" CssClass="form-control inputboxstyle" runat="server"
                        TabIndex="2">
                        <asp:ListItem Value="All">All</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlrowerName"
                        ValidationGroup="RowerCharge" SetFocusOnError="True" InitialValue="Select Boat Type" CssClass="vError">
                        Select Boat Type</asp:RequiredFieldValidator>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label2" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />
                </div>
            </div>
        </div>

        <div runat="server" id="divGridList" visible="false">
            <div class="table-responsive">
                <div style="text-align: right;" runat="server" id="divSearch">
                    Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="20px" placeholder="Enter Booking Id/ Pin" AutoComplete="off" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                </div>
                <asp:GridView ID="GvRowerCharges" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="25000">
                    <columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Reference Number" HeaderStyle-CssClass="grdHead" Visible="false">
                            <itemtemplate>
                                <asp:Label ID="lblBoatReferenceNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type Name" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rower Name" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Charges" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblActualRowerCharge" runat="server" Text='<%# Bind("ActualRowerCharge") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>

                        <%--  <asp:TemplateField HeaderText="Print" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20px" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Print.svg" OnClick="ImgBtnEdit_Click" ToolTip="Print" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                    </columns>

                    <headerstyle cssclass="gvHead" />
                    <alternatingrowstyle cssclass="gvRow" />
                    <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                </asp:GridView>
                <div runat="server" id="divprevnext" style="text-align: left;">
                    <asp:Button ID="back" runat="server" CssClass="btn btn-color mg" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                    &nbsp
                                 <asp:Button ID="Next" Visible="true" CssClass="btn btn-color mg" runat="server" Text="Next →" OnClick="Next_Click" />
                    &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />
                </div>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

