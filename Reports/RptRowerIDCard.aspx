<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RptRowerIDCard.aspx.cs" Inherits="Reports_RptRowerIDCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">Rower Id Card Generation
            <span style="float: right;">
                <asp:LinkButton ID="lbtnRower" CssClass="lbtnNew" runat="server" OnClick="lbtnRower_Click" CausesValidation="false">
                    <i class="fas fa-arrow-circle-left"></i>Back to Rower</asp:LinkButton></span>
        </h5>
        <hr />
        <div class="mydivbrdr">
            <div class="row" style="padding-left: 15px;">
                <div class="col-md-12 col-sm-12">
                    <div class="row">


                        <div class="col-sm-3 col-xs-12" runat="server">
                            <label for="lblPaymentType" id="lblPaymentType">
                                <i class="fa fa-user" aria-hidden="true"></i>Rower Name 
                                <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlRowerName" CssClass="form-control inputboxstyle" runat="server" TabIndex="1">
                            </asp:DropDownList>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlRowerName" InitialValue="0"
                                ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select Rower Name</asp:RequiredFieldValidator>--%>
                        </div>

                        <div class="col-md-3 col-lg-3 col-sm-3" style="margin-top: 30px;">
                            <asp:Button ID="btnGenerate" runat="server" Text="Generate" class="btn btn-primary" ValidationGroup="Search" TabIndex="2"
                                OnClick="btnGenerate_Click" />

                            <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="3"
                                OnClick="btnReset_Click" />
                            <span style="float: right"></span>
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

