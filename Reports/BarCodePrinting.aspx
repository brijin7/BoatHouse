<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="BarCodePrinting.aspx.cs" Inherits="QR_Code_Generator_BarCodePrinting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">QR Code Generation</h5>
        <hr />
        <br />
        <div class="col-sm-12">
            <div class="row">
                <div class="col-sm-6" runat="server" visible="false">
                    <label runat="server" id="Label2"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Name <span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlBoatHouseName" AutoPostBack="true" runat="server" TabIndex="1" OnSelectedIndexChanged="ddlBoatHouseName_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBoatHouseName"
                        ValidationGroup="Barcode" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat House Name</asp:RequiredFieldValidator>
                </div>

                <div class="col-sm-2">
                    <label for="lblServices" id="lblServices"><i class="fas fa-list" aria-hidden="true"></i>Service Category<span class="spStar">*</span></label>
                    <asp:DropDownList runat="server" ID="ddlServices" CssClass=" form-control" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlServices_SelectedIndexChanged" TabIndex="2">
                        <asp:ListItem Value="0">Sign In</asp:ListItem>
                        <asp:ListItem Value="5">Android App</asp:ListItem>
                        <asp:ListItem Value="1">Boating</asp:ListItem>
                        <asp:ListItem Value="2">Restaurant</asp:ListItem>
                        <asp:ListItem Value="3">Other Services</asp:ListItem>

                    </asp:DropDownList>
                </div>

                <div class="col-sm-2" runat="server" id="serviceType">
                    <label for="lblServices" id="lblBoattypeServices"><i class="fas fa-list" aria-hidden="true"></i>Service Type<span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlBoattypes" runat="server" TabIndex="3" CssClass="form-control"></asp:DropDownList>
                </div>

                <div class="col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnPrint" CssClass="btn btn-primary" OnClick="btnPrint_Click" TabIndex="4" ValidationGroup="Barcode" Text="Print" runat="server" Width="100px" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hfseatertype" />
    <asp:HiddenField runat="server" ID="hfAllSeaterId" />
    <asp:HiddenField runat="server" ID="hfAllSeater" />
    <asp:HiddenField runat="server" ID="hfAllBoatType" />
    <asp:HiddenField runat="server" ID="hfallReport" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

