<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="PrintingRights.aspx.cs"
    Inherits="Boating_PrintingRights" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        table, tr, td {
            border: none;
        }
    </style>
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Printing Rights  </h5>
        <hr />
        <div class="mydivbrdr" runat="server" style="margin-top: 20px" id="divEntry" visible="false">
            <div class="col-sm-7 col-xs-12">
                <table class="table table-bordered table-condenced" style="border-collapse: collapse; border: none;">
                    <tr style="background-color: lightgoldenrodyellow">
                        <td style="border: none; background-color: cadetblue;">
                            <label for="boat" style="font-weight: bold; padding: 10px; color: white">Other Service</label></td>
                        <td style="border: none;">
                            <asp:RadioButtonList ID="rblOtherService" runat="server" RepeatDirection="Horizontal" TabIndex="2" CssClass="rbl" ForeColor="Black" Font-Bold="true">
                                <asp:ListItem Value="Both" Selected="true">Both</asp:ListItem>
                                <asp:ListItem Value="Single">Single</asp:ListItem>
                                <asp:ListItem Value="Abstract">Abstract</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>

                    <tr style="border: none;">
                        <td style="border: none;"></td>
                        <td style="border: none;"></td>
                    </tr>

                    <tr style="background-color: lightblue">
                        <td style="border: none; background-color: cadetblue">
                            <label for="boat" style="font-weight: bold; padding: 10px; color: white">Restaurant</label></td>
                        <td style="border: none;">
                            <asp:RadioButtonList ID="rblRestaurant" runat="server" RepeatDirection="Horizontal" TabIndex="2" CssClass="rbl" ForeColor="Black" Font-Bold="true">
                                <asp:ListItem Value="Both" Selected="true">Both</asp:ListItem>
                                <asp:ListItem Value="Single">Single</asp:ListItem>
                                <asp:ListItem Value="Abstract">Abstract</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>

                    <tr style="border: none;">
                        <td style="border: none;"></td>
                        <td style="border: none;"></td>
                    </tr>

                    <tr style="background-color: bisque">
                        <td style="border: none; background-color: cadetblue">
                            <label for="boat" style="font-weight: bold; padding: 10px; color: white">Additional Ticket</label></td>
                        <td style="border: none;">
                            <asp:RadioButtonList ID="rblAdditionalTkt" runat="server" RepeatDirection="Horizontal" TabIndex="2" CssClass="rbl" ForeColor="Black" Font-Bold="true">
                                <asp:ListItem Value="Both" Selected="true">Both</asp:ListItem>
                                <asp:ListItem Value="Single">Single</asp:ListItem>
                                <asp:ListItem Value="Abstract">Abstract</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                </table>
            </div>

            <div class="col-sm-3 col-xs-12" runat="server" id="divSubmit" visible="false">
                <div class="form-submit" style="text-align: left !important">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" TabIndex="5" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger" TabIndex="6" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server" class="col-sm-8 col-xs-12" style="margin-top: 20px">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvPrintingRights" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="UniqueId,BoatHouseId,BoatHouseName,OtherService,Restaurant,AdditionalTicket">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Other Service" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOtherService" runat="server" Text='<%# Bind("OtherService") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Restaurant" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRestaurant" runat="server" Text='<%# Bind("Restaurant") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Additional Ticket" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAdditionalTicket" runat="server" Text='<%# Bind("AdditionalTicket") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" runat="server" ImageUrl="~/images/Edit.png" Height="20px" Width="20px" OnClick="ImgBtnEdit_Click" />

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

