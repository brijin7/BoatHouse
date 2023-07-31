<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="DeviceAllocation.aspx.cs" Inherits="Sadmin_DeviceAllocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="form-body">
        <h5 class="pghr">Device Allocation </h5>
        <hr />
        <br />
        <div id="divAllServices" runat="server" visible="true" style="width: 100%">
            <div class="col-md-12 col-sm-12">
                <div class="row">

                    <div class="col-xl-3 col-md-3 col-lg-3  col-sm-3">
                        <label for="lblServices" id="lblDevice"><i class="fas fa-list" aria-hidden="true"></i>Device No</label>
                        <asp:DropDownList runat="server" ID="ddlDeviceName" CssClass=" form-control" TabIndex="1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlDeviceName"
                            ValidationGroup="DeviceAllocation" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Device No</asp:RequiredFieldValidator>
                    </div>
                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-3" runat="server">
                        <label for="lblCategory" id="lblCorpId"><i class="fas fa-list" aria-hidden="true"></i>Corporate Office</label>
                        <asp:DropDownList ID="ddlCorpId" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                            OnSelectedIndexChanged="ddlCorpId_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCorpId" InitialValue="Select Corporate Office"
                            ValidationGroup="Mapping" SetFocusOnError="True" CssClass="vError">Select Corporate Office</asp:RequiredFieldValidator>
                    </div>

                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-3" runat="server">
                        <label for="lblCategory" id="lblBoatHouse"><i class="fas fa-list" aria-hidden="true"></i>Boat House</label>
                        <asp:DropDownList ID="ddlBoatHouse" CssClass="form-control inputboxstyle" runat="server" TabIndex="2">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlBoatHouse"
                            ValidationGroup="DeviceAllocation" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                    </div>

                    <div class="col-sm-2 col-xs-12" style="padding-top: 1.7rem;">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="DeviceAllocation" TabIndex="3" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-danger" CausesValidation="false" OnClick="btnCancel_Click" TabIndex="4" />
                    </div>
                </div>
            </div>

        </div>
        <div id="divGrid" runat="server" class="col-sm-12 col-xs-12">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvDeviceAllocation" runat="server" AllowPaging="True"
                    CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="CorpId,CorpName,UniqueId,DeviceNo,BoatHouseId,BoatHouseName,ActiveStatus" OnRowDataBound="gvDeviceAllocation_RowDataBound"
                    PageSize="25000" Style="float: left;">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unique Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUniqueId" runat="server" Text='<%# Bind("UniqueId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Device No" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDeviceNO" runat="server" Text='<%# Bind("DeviceNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatHouse Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="CorpId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCorpId" runat="server" Text='<%# Bind("CorpId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="Corporate" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCorpName" runat="server" Text='<%# Bind("CorpName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatHouse Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                      

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');" />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>

    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

