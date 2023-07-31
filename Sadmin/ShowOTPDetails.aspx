<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="ShowOTPDetails.aspx.cs" Inherits="Sadmin_ShowOTPDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">OTP Details </h5>
        <hr />

        <div class="mydivbrdr" runat="server" id="divShow" visible="false">
            <div class="row p-2">
                <div class="col-sm-6 col-xs-12">
                    <div class="row m-0">
                        <div class="col-sm-6 col-xs-12">
                            <div class="form-group">
                                <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Service Type</label>
                                <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" TabIndex="1"
                                    OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                                    <asp:ListItem Value="SignUp">Sign Up</asp:ListItem>
                                    <asp:ListItem Value="ForgotPwd">Forgot Password</asp:ListItem>
                                    <asp:ListItem Value="BBReceipt">Boating Receipt</asp:ListItem>
                                    <asp:ListItem Value="BOReceipt">Other Service Receipt</asp:ListItem>
                                    <asp:ListItem Value="BRReceipt">Restaurant Receipt</asp:ListItem>
                                    <asp:ListItem Value="DOnlineBooking">Online Booking</asp:ListItem>
                                    <asp:ListItem Value="DUPI">UPI Online</asp:ListItem> 
                                    <asp:ListItem Value="LateDepositRefund">Previous Day Refund</asp:ListItem>                                    
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server" class="col-sm-12 col-xs-12" visible="false">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblPayGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvOTPDetails" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mobile No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MobileNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Code" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Message" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSMSMessage" runat="server" Text='<%# Bind("SMSMessage") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat House Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Media Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblMediaType" runat="server" Text='<%# Bind("MediaType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>
    
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

