<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="BoatSlotMaster.aspx.cs" Inherits="Boating_BoatSlotMaster" %>

<asp:Content ID="BoatSlotMaster" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Boat Slot Master <span style="float: right; padding-right: 582px">Working Hours   : 
          <asp:Label ID="BoatHseStartTme" runat="server"></asp:Label>&nbsp;
                                        <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>&nbsp;<asp:Label ID="BoatHseEndTme" runat="server"></asp:Label></span> </h5>
        <hr />
        <br />

        <div id="divEntry" runat="server" visible="true">
            <div class="row">
                <div class="col-sm-12 col-xs-12">
                    <div class="mydivbrdr">
                        <div class="row p-2">

                            <div class="col-sm-2 col-xs-12">
                                <label for="lblBoatSeatId" id="lbl1"><i class="fas fa-ship"></i>Slot Type<span class="spStar">*</span></label>
                                <asp:DropDownList ID="ddlSlotType" CssClass="form-control inputboxstyle" runat="server" OnSelectedIndexChanged="ddlSlotType_SelectedIndexChanged" AutoPostBack="true"
                                    TabIndex="2">
                                    <asp:ListItem Value="0">Select Slot Type</asp:ListItem>
                                    <asp:ListItem Value="N">Normal</asp:ListItem>
                                    <asp:ListItem Value="P">Express</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSlotType"
                                    ValidationGroup="BoatSlot" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Slot Type</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <label for="lblBoatType" id="lblboattype"><i class="fa fa-ship" aria-hidden="true"></i>Boat Type <span class="spStar">*</span></label>
                                <asp:DropDownList ID="ddlBoatType" CssClass="form-control inputboxstyle" runat="server"
                                    TabIndex="1" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvBoatType" runat="server" ControlToValidate="ddlBoatType"
                                    ValidationGroup="BoatSlot" SetFocusOnError="True" InitialValue="Select Boat Type" CssClass="vError">Select Boat Type</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <label for="lblBoatSeatId" id="lbl1"><i class="fas fa-chair"></i>Boat Seater <span class="spStar">*</span></label>
                                <asp:DropDownList ID="ddlBoatSeatId" CssClass="form-control inputboxstyle" runat="server"
                                    TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlBoatSeatId_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvSeaterType" runat="server" ControlToValidate="ddlBoatSeatId"
                                    ValidationGroup="BoatSlot" SetFocusOnError="True" InitialValue="Select Boat Seater"
                                    CssClass="vError">Select Boat Seater</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2 col-xs-12" runat="server">

                                <div class="form-group">
                                    <label for="lblboatnum">
                                        <i class="fa fa-calendar" aria-hidden="true"></i>
                                        Slot  Duration
                                    </label>
                                    <div class="input-group-prepend">
                                        <asp:DropDownList ID="dlOh1OpenHours" runat="server" CssClass="form-control" TabIndex="3">
                                            <asp:ListItem Value="0">Select Duration</asp:ListItem>
                                            <asp:ListItem Value="00:20:00">20 Minutes</asp:ListItem>
                                            <asp:ListItem Value="00:30:00">30 Minutes</asp:ListItem>
                                            <asp:ListItem Value="00:40:00">40 Minutes</asp:ListItem>
                                            <asp:ListItem Value="00:50:00">50 Minutes</asp:ListItem>
                                            <asp:ListItem Value="01:00:00">60 Minutes</asp:ListItem>
                                            <asp:ListItem Value="01:10:00">70 Minutes</asp:ListItem>
                                            <asp:ListItem Value="01:20:00">80 Minutes</asp:ListItem>
                                            <asp:ListItem Value="01:30:00">90 Minutes</asp:ListItem>
                                            <asp:ListItem Value="01:40:00">100 Minutes</asp:ListItem>
                                            <asp:ListItem Value="01:50:00">110 Minutes</asp:ListItem>
                                            <asp:ListItem Value="02:00:00">120 Minutes</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvHour" runat="server" ControlToValidate="dlOh1OpenHours"
                                        ValidationGroup="BoatSlot" SetFocusOnError="True" CssClass="vError" InitialValue="0">Enter Hour</asp:RequiredFieldValidator>

                                </div>
                            </div>

                            <div class="col-sm-2 col-xs-12" runat="server" style="padding-top: 30px">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BoatSlot" TabIndex="5" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="6" OnClick="btnCancel_Click" />
                            </div>

                        </div>
                        <div class="row p-2">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-xs-12 text-right">
            </div>
        </div>

        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvBoatSlot" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatTypeId" PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Slot Id" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblSlotId" runat="server" Text='<%# Bind("SlotId") %>'></asp:Label>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Visible="false" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatSeaterId" runat="server" Visible="false" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Slot Start Time" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblSlotStartTime" runat="server" Text='<%# Bind("SlotStartTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Slot End Time" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblSlotEndTime" runat="server" Text='<%# Bind("SlotEndTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Slot Duration (HH:MM)" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblSlotDuration" runat="server" Text='<%# Bind("SlotDuration") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat House" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Slot Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%-- <asp:Label ID="lblSlotType" runat="server"  Text='<%# Bind("SlotType") %>'></asp:Label>--%>
                                <asp:Label ID="lblSlotType" runat="server" Text='<%# Eval("SlotType").ToString() == "N" ? "Normal" : "Express" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trip Count Per Slot" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAvailableCount" runat="server" Text='<%# Bind("AvailableCount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

