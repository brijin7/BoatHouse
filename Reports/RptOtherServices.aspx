<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" Async="true" AutoEventWireup="true" CodeFile="~/Reports/RptOtherServices.aspx.cs" Inherits="Boating_RptOtherServices" %>

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
        <h5 class="pghr">ABSTRACT Other Services</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row" style="padding-left: 15px;">
                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblCategoryname" id="lblCategoryname"><i class="fa fa-ship" aria-hidden="true"></i>Category Name <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlCategory" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblservicename" id="lblservicename"><i class="fa fa-ship" aria-hidden="true"></i>Service Name <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlServiceName" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="2">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>


                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lbllbookingtype" id="lbllbookingtype"><i class="fa fa-ship" aria-hidden="true"></i>Booking Type <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlBookingType" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="2">
                                <asp:ListItem Value="0">All</asp:ListItem>
                                <asp:ListItem Value="I">Independent </asp:ListItem>
                                <asp:ListItem Value="B">Along with Boating </asp:ListItem>
                            </asp:DropDownList>
                        </div>


                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                                Enter From Date</asp:RequiredFieldValidator>
                        </div>

                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
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
                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2" style="margin-top: 30px;">
                            <div class="form-group">
                                <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" style="padding-left: 15px;">
                <div class="col-md-5 col-sm-5">
                    <div runat="server" style="overflow: auto; max-height: 400px; max-width: 400px; min-height: 200px; min-width: 30%;">
                        <div class="table-responsive">
                            <asp:GridView ID="GVabstractsrv" runat="server" AllowPaging="false" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="CategoryName" ShowFooter="true">
                                <columns>
                                    <asp:TemplateField HeaderText="Service Name" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblCategoryName" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
                                        </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblcount" runat="server" Text='<%# Bind("TicketCount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lbltotal" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>
                                </columns>
                                <headerstyle cssclass="gvHead" />
                                <alternatingrowstyle cssclass="gvRow" />
                                <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="col-md-7 col-sm-7" style="padding-right: 35px;">
                    <div runat="server" id="divreslistall" style="overflow: auto; max-height: 400px; max-width: 700px; min-height: 200px; min-width: 30%;">
                        <div class="table-responsive">
                            <asp:GridView ID="GvOtherServiceSummary" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="ServiceName" PageSize="1000" ShowFooter="true">
                                <columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Category" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblCategoryName" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
                                            -
                                            <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Booking Type" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBookingType" runat="server" Text='<%# Bind("BookingType") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Item Amount" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblItemAmount" runat="server" Text='<%# Bind("ItemAmount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="No Of Items" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblNoOfItems" runat="server" Text='<%# Bind("NoOfItems") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Service Fare" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblNoOfItems" runat="server" Text='<%# Bind("ItemFare") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Tax Amount" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblNoOfItems" runat="server" Text='<%# Bind("TaxAmount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblNetAmount" runat="server" Text='<%# Bind("NetAmount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>

                                </columns>
                                <headerstyle cssclass="gvHead" />
                                <alternatingrowstyle cssclass="gvRow" />
                                <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <br />
        <div runat="server" id="divGridList" visible="false">
            <div class="table-responsive">
                <asp:GridView ID="GvOtherServices" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="100" OnPageIndexChanging="GvOtherServices_PageIndexChanging"
                    ShowFooter="true">
                    <columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <%#Container.DataItemIndex+1 %>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Category" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblServiceName" runat="server" Text='<%# Eval("CategoryName")+" - "+ Eval("ServiceName") %>'></asp:Label>
                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Booking Type" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBookingType" runat="server" Text='<%# Bind("BookingType") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="No Of Items" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("NoOfItems") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Service Fare" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblServiceFare" runat="server" Text='<%# Bind("ServiceFare") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax Amount" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblTaxAmount" runat="server" Text='<%# Bind("TaxAmount") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Net Amount" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblNetAmount" runat="server" Text='<%# Bind("NetAmount") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>

                    </columns>

                    <headerstyle cssclass="gvHead" />
                    <pagerstyle forecolor="Green" font-bold="true" font-size="15px" />
                    <alternatingrowstyle cssclass="gvRow" />
                    <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                </asp:GridView>
            </div>
        </div>

    </div>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

