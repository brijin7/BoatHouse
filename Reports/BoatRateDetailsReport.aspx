<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="BoatRateDetailsReport.aspx.cs" Inherits="Boating_BoatRateDetailsReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="form-body">
        <h5 class="pghr">Boat Rate Details Master </h5>
        <hr />
      
        <div id="divGrid" runat="server">
            <div class="table-responsive" >
            <div id="divBoatRtaeDetails" style=" display: block; min-width: 100%; max-width: 100%; min-height: 200px; max-height: 500px">

                  <span style="float: right; padding-left: 30px;">
                        <asp:ImageButton ID="btnExportToExcel" runat="server" ImageUrl="~/images/Excel.png" OnClick="ExportToExcel_Click" Width="40px" TabIndex="3" Visible="true" />
                    </span>
                <asp:GridView ID="gvBoatRateDetails" runat="server" CssClass="gvv display table table-bordered table-condenced"  OnRowCreated="gvBoatRateDetails_RowCreated" 
                     AutoGenerateColumns="false"  >
                    <Columns>
                        <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSeater" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                 <asp:Label ID="lblSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Self Drive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSelfDrive" runat="server" Text='<%# Bind("SelfDrive") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Time Extension" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTimeExtension" runat="server" Text='<%# Bind("TimeExtension") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Duration" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatMinTime" runat="server" Text='<%# Bind("BoatMinTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Grace Time" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatGraceTime" runat="server" Text='<%# Bind("BoatGraceTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDeposit" runat="server" Text='<%# Bind("Deposit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Boat Charge" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatMinCharge" runat="server" Text='<%# Bind("BoatMinCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Rower" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerMinCharge" runat="server" Text='<%# Bind("RowerMinCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatMinTaxAmt" runat="server" Text='<%# Bind("BoatMinTaxAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatMinTotAmt" runat="server" Text='<%# Bind("BoatMinTotAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Charge" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatPremMinCharge" runat="server" Text='<%# Bind("BoatPremMinCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Rower" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerPremMinCharge" runat="server" Text='<%# Bind("RowerPremMinCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatPremTaxAmt" runat="server" Text='<%# Bind("BoatPremTaxAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatPremTotAmt" runat="server" Text='<%# Bind("BoatPremTotAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Extension Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblExtensionType" runat="server" Text='<%# Bind("ExtensionType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="From" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblExtFromTime" runat="server" Text='<%# Bind("ExtFromTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ExtToTime" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblExtToTime" runat="server" Text='<%# Bind("ExtToTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="AmountType" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountType" runat="server" Text='<%# Bind("AmountType"  ) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Percentage" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPercentage" runat="server" Text='<%# Bind("Percentage") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatExtnCharge" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatExtnCharge" runat="server" Text='<%# Bind("BoatExtnCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="RowerExtnCharge" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerExtnCharge" runat="server" Text='<%# Bind("RowerExtnCharge") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="BoatExtnTaxAmt" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatExtnTaxAmt" runat="server" Text='<%# Bind("BoatExtnTaxAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatExtnTotAmt" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatExtnTotAmt" runat="server" Text='<%# Bind("BoatExtnTotAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                   <%-- <HeaderStyle CssClass="gvHead" BackColor="#124a79" ForeColor="white"/>--%>
                     <HeaderStyle CssClass="gvHead" BackColor="#f9f9f9" ForeColor="black"/>
                    <AlternatingRowStyle CssClass="gvRow" />
                    <FooterStyle BackColor="#f9f9f9" ForeColor="Black" Font-Bold="true" HorizontalAlign="Right" />
                    <%--<FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />--%>
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>
        </div>
    </div>


      <%-- Newly implemented for CSRF Validation--%>
        <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
        <%-- Newly implemented for CSRF Validation--%>
   
</asp:Content>

