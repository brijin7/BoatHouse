<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="~/Boating/BoatTypeMaster.aspx.cs" Inherits="BoatType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .h1, h1 {
            font-size: 19px;
            padding-left: 268px;
        }
    </style>
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Boat Type Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label for="boat" id="txtboatType"><i class="fa fa-ship" aria-hidden="true"></i>Boat Type</label>
                            <asp:TextBox ID="txtboatTypeId" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="40" TabIndex="1">
                            </asp:TextBox>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label for="lblBoatHouse" id="lblboathouse"><i class="fa fa-ship" aria-hidden="true"></i>Boat House</label>
                            <asp:DropDownList ID="ddlboathouse" CssClass="form-control inputboxstyle" runat="server"
                                AutoPostBack="true" TabIndex="2">
                                <asp:ListItem Value="0">Select Boat House</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlboathouse"
                                ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatType" id="lblboattype"><i class="fa fa-ship" aria-hidden="true"></i>Boat Type <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtBoatType" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="40" TabIndex="1">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ControlToValidate="txtBoatType" SetFocusOnError="True" CssClass="vError"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Not Valid" ValidationGroup="BoatType" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtBoatType"
                                ValidationGroup="BoatType" SetFocusOnError="True" CssClass="vError">Enter Boat Type</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right pt-3">
                        <div class="form-submit" style="text-align: left !important">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BoatType" TabIndex="3" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="4"
                                OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="GvBoatType" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced GridStyle"
                    AutoGenerateColumns="False" PageSize="25000" DataKeyNames="BoatTypeId" OnRowDataBound="GvBoatType_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatTypeId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblboatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat Type">
                            <ItemTemplate>
                                <asp:Label ID="lblboatTypeName" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatHouseId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblboatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatHouseName" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to Inactive this record?');" />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure you want to Active this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="25%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfUserId" runat="server" />
    <asp:HiddenField ID="hfboattypeId" runat="server" />
    <asp:HiddenField ID="hfboathouse" runat="server" />
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

