<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Sadmin/BHLogin.aspx.cs" Inherits="Sadmin_BHLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Boat House Login </h5>
        <hr />

        <div runat="server" id="divShow" visible="false">
            <div class="mydivbrdr">

                <div class="row p-2">
                    <div class="col-sm-12 offset-sm-3 col-xs-12">
                        <div class="row">
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label11"><i class="fa fa-address-book" aria-hidden="true"></i>Corporate Office<span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlCorpId" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                                         OnSelectedIndexChanged="ddlCorpId_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCorpId"
                                        ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="Select Corporate Office" CssClass="vError">Select Corporate Office</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Name</label>
                                    <asp:DropDownList ID="ddlBoatHouseId" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBoatHouseId"
                                        ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="Select Boat House" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            




                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-sm-3 offset-sm-4 col-xs-12">

                        <div class="form-group text-center">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-primary" ValidationGroup="BoatType" Width="100px" TabIndex="2" OnClick="btnLogin_Click" />
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

