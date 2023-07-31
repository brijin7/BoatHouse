<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Reports/RptBoatwiseTrip.aspx.cs" Inherits="Boating_BoatWiseTrip" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/BoatStyle.css" rel="stylesheet" />

    <div class="form-body">
        <h5 class="pghr">Boat Wise Trip</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label runat="server" id="lblBoatType"><i class="fa fa-ship" aria-hidden="true"></i>Boat Type <span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlBoatType" CssClass="form-control inputboxstyle" runat="server" TabIndex="1"
                            OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label runat="server" id="lblBoatSeater"><i class="fa fa-chair" aria-hidden="true"></i>Boat Seater <span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlBoatSeatId" CssClass="form-control inputboxstyle" runat="server" TabIndex="2"
                            OnSelectedIndexChanged="ddlBoatSeatId_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label runat="server" id="lblBoatSelection"><i class="fa fa-ship" aria-hidden="true"></i>Boat Selection <span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlBoatSelection" CssClass="form-control inputboxstyle" runat="server" TabIndex="3">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="4">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Payment" SetFocusOnError="True" CssClass="vError">
                            Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label2" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control toDate" AutoComplete="Off" TabIndex="5">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Payment" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2" style="padding-top: 2rem;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Payment" TabIndex="6" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="7" OnClick="btnReset_Click" />
                </div>

            </div>
        </div>
        <div style="margin-left: auto; margin-right: auto; text-align: center;" runat="server" id="lblmesg" visible="false">
            <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        </div>
        <div class="row p-2">
            <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                <div class="form-group">

                    <div runat="server" id="divmaxtrips" style="overflow: auto; max-width: 300px; min-width: 100px; max-height: 250px; min-height: 100px;">
                        <div class="table-responsive">

                            <asp:GridView ID="GvMaxTrips" runat="server" AllowPaging="false" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="false" PageSize="3" ShowFooter="true">

                                <columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdhead">
                                        <itemtemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Type Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Trips" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:LinkButton ID="lblTotalTrips" runat="server" OnClick="lblTotalTrips_Click" Font-Underline="false" Text='<%# Bind("Trips") %>'></asp:LinkButton>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>

                                </columns>
                                <headerstyle cssclass="gvHead" />
                                <alternatingrowstyle cssclass="gvRow" />
                                <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                                <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <br />

            <div class="col-sm-8 col-md-8 col-lg-8 col-xs-12">
                <div class="form-group">
                    <div runat="server" id="divGridSummary" visible="false" style="overflow: auto; max-width: 600px; min-width: 300px; max-height: 250px; min-height: 100px;">
                        <div class="table-responsive">
                            <asp:GridView ID="GvSummary" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="false" DataKeyNames="BookingId" PageSize="10" OnPageIndexChanging="GvSummary_pageindexchanging" ShowFooter="true">
                                <columns>
                                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdhead">
                                        <itemtemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Type Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Seater Id" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <itemtemplate>
                                            <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("BoatSeater") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Trips" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:LinkButton ID="lblTrips" runat="server" Text='<%# Bind("Trips") %>' OnClick="lblTrips_Click" Font-Underline="false"></asp:LinkButton>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                                        <itemtemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                        </itemtemplate>
                                        <itemstyle horizontalalign="Right" />
                                    </asp:TemplateField>

                                </columns>
                                <headerstyle cssclass="gvHead" />
                                <alternatingrowstyle cssclass="gvRow" />
                                <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                                <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div runat="server" id="divGridList" visible="false">
            <div class="table-responsive">
                <asp:GridView ID="GvBoatwiseTrip" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BookingId" PageSize="10" ShowFooter="true" OnPageIndexChanging="GvBoatwiseTrip_PageIndexChanging">
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

                        <asp:TemplateField HeaderText="Reference No" HeaderStyle-CssClass="grdHead" Visible="false">
                            <itemtemplate>
                                <asp:Label ID="lblBoatRefNo" runat="server" Text='<%# Bind("BoatReferenceNo") %>'></asp:Label>
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

                        <asp:TemplateField HeaderText="Boat Type Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <itemtemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("BoatSeater") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Name" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblBoatSelection" runat="server" Text='<%# Bind("BoatName") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblAmt" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                        </asp:TemplateField>
                    </columns>
                    <headerstyle cssclass="gvHead" />
                    <alternatingrowstyle cssclass="gvRow" />
                    <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                    <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfBoatId" runat="server" />
    <asp:HiddenField ID="hfBoatTypeId" runat="server" />
    <asp:HiddenField ID="hfBoatSeaterId" runat="server" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

