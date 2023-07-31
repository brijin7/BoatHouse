<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="TripWiseReport.aspx.cs" Inherits="Reports_TripWiseReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">


    <div class="form-body">
        <h5 class="pghr">Trip Wise Collection</h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row" style="padding-left: 15px;">
                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblServices" id="lblServices"><i class="fas fa-list" aria-hidden="true"></i>BoatType<span class="spStar">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlBoatType" CssClass=" form-control" TabIndex="1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlBoatType" InitialValue="0"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                                Select BoatType</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xl-2 col-md-2 col-lg-2  col-sm-2">
                            <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>Booking Date</label>
                            <asp:TextBox ID="txtBookingDate" runat="server" CssClass=" form-control frmDate"
                                AutoComplete="Off" TabIndex="2">
                            </asp:TextBox>
                        </div>


                        <%--  <div class="col-sm-2 col-xs-12" runat="server" id="divPaymentType" visible="true">
                            <label for="lblCategory" id="lblCategory"><i class="fas fa-list" aria-hidden="true"></i>Category<span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlCategory" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="2">
                            </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCategory" InitialValue="0"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select Category</asp:RequiredFieldValidator>

                        </div>
                        <div class="col-sm-3 col-xs-12" runat="server" id="divUserName" visible="true">
                            <label for="lblTypes" id="lblTypes"><i class="fas fa-list" aria-hidden="true"></i>Types <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlTypes" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="3">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTypes" InitialValue="0"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select Types</asp:RequiredFieldValidator>

                        </div>--%>
                        <div class="col-md-3 col-lg-3 col-sm-3" style="margin-top: 30px;">
                            <span style="float: right">
                                <asp:Button ID="btnGenerate" runat="server" Text="Generate" class="btn btn-primary" ValidationGroup="Search" OnClick="btnGenerate_Click" TabIndex="4" />

                            </span>

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




