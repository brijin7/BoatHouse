<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="Generateboardingpass.aspx.cs" Inherits="Boating_Generateboardingpass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style>
        .lblstyle {
            font-size: x-large;
            /*text-align: center;*/
            margin-left: 40%;
            /*margin-top: 40%;*/
        }
    </style>
    <div class="form-body">
        <div class="mydivbrdr" runat="server" id="divAllDetails">
            <h5 class="pghr">Generate Boarding Pass</h5>
            <hr />
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server" visible="false">
                    <div class="form-group">
                        <label for="lblBookingDate" id="lblBookingDate" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>Booking Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtBookingDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                        </asp:TextBox>
                    </div>
                </div>

                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;" runat="server" visible="false">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" OnClick="btnSubmit_Click1" ValidationGroup="Search" TabIndex="2" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" OnClick="btnReset_Click" class="btn  btn-danger" TabIndex="3" />
                </div>

                <div class="col-sm-12 col-xs-12" runat="server" style="height: 600px; overflow-y: auto; float: left; position: static;">
                    <div class="table-responsive" runat="server" id="divShowDetails">

                        <asp:GridView ID="GvGenerateBoardingPass" runat="server" CssClass="gvv display table table-bordered table-condenced"
                            AutoGenerateColumns="False" DataKeyNames="BookingId">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <%--     <asp:LinkButton ID="lnkBookingId" runat="server" OnClick="lnkBookingId_Click" Text='<%# Bind("BookingId") %>' Font-Bold="true" Font-Size="X-Large"></asp:LinkButton>--%>
                                        <asp:Label ID="lnkBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Booking Pin" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBookingPin" runat="server" Text='<%# Bind("BookingPin") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Expected Boat Id" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillAmount" runat="server" Text='<%# Bind("ExpectedBoatId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Expected Boat No" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# Bind("ExpectedBoatNum") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Expected Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRstatus" runat="server" Text='<%# Bind("ExpectedTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Generate" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnBookingId" runat="server" OnClick="lnkbtnBookingId_Click" Text='Generate' Font-Bold="true"
                                            OnClientClick="return confirm('Do you Want to Generate Boarding Pass?');" ForeColor="Green"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                        <asp:HiddenField ID="hfBookingid" runat="server" />
                    </div>
                    <div class="col-xs-12 mt-5" id="divmsbeb" runat="server" visible="false">
                        <span>
                            <asp:Label ID="lblNorecords" runat="server" ForeColor="Red" CssClass="blink lblstyle" Font-Bold="true"></asp:Label>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

