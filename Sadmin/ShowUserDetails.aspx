<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="ShowUserDetails.aspx.cs" Inherits="Sadmin_ShowUserDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">User Details </h5>
        <hr />

        <div class="mydivbrdr" runat="server" id="divShow" visible="false">
            <div class="row p-2">
                <div class="col-sm-12 col-xs-12">
                    <div class="row m-0">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>User Type</label>
                                <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control" TabIndex="1"
                                    OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Select User Type</asp:ListItem>
                                    <asp:ListItem Value="SU">Sadmin User</asp:ListItem>
                                    <asp:ListItem Value="BHU">Boat House User</asp:ListItem>
                                    <asp:ListItem Value="PU">Public User</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-9 col-xs-12" id="divBoatHouse" runat="server" visible="false" style="display:flex;">
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label11"><i class="fa fa-address-book" aria-hidden="true"></i>Corporate Office<span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlCorpId" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                                        OnSelectedIndexChanged="ddlCorpId_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCorpId" InitialValue="Select Corporate Office"
                                        ValidationGroup="BoatType" SetFocusOnError="True" CssClass="vError">Select Corporate Office</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Name</label>
                                    <asp:DropDownList ID="ddlBoatHouseId" runat="server" CssClass="form-control" TabIndex="3"
                                        OnSelectedIndexChanged="ddlBoatHouseId_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBoatHouseId"
                                        ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="Select Boat House" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                                </div>
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
                <asp:GridView ID="gvUserDetails" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="UserId, BoatHouseId">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Password" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPassword" runat="server" Text='<%# Bind("Password") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mobile No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MobileNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEmailId" runat="server" Text='<%# Bind("EmailId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat House Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active Status" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
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

