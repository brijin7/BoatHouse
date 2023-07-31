<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    CodeFile="~/Boating/BoatSeatMaster.aspx.cs" Inherits="Boating_BoatSeatMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Boat Seater Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">

                    <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fa fa-ship" aria-hidden="true"></i>
                                Boat House 
                            </label>
                            <asp:DropDownList ID="ddlBoatHouseId" CssClass="form-control inputboxstyle" runat="server" TabIndex="2"
                                AutoPostBack="true">
                                <asp:ListItem Value="0"> Select Boat House</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBoatHouseId"
                                ValidationGroup="BoatSeaterMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat House </asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fas fa-chair" aria-hidden="true"></i>
                                Seater Type <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtSeaterType" runat="server" CssClass="form-control" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event)" TabIndex="1" MaxLength="100">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSeaterType"
                                ValidationGroup="BoatSeaterMaster" SetFocusOnError="True" CssClass="vError">Enter Seater Type</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-2 col-xs-12">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fas fa-chair" aria-hidden="true"></i>
                                Number Of Seats <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtNoofseats" runat="server" CssClass="form-control" AutoComplete="Off" onkeypress="return isNumber(event)" TabIndex="3" MaxLength="2">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNoofseats"
                                ValidationGroup="BoatSeaterMaster" SetFocusOnError="True" CssClass="vError">Enter Number Of Seats</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fas fa-chair" aria-hidden="true"></i>
                                Allowed seats (if any restriction)
                            </label>
                            <asp:TextBox ID="txtAllowedSeats" runat="server" CssClass="form-control" AutoComplete="Off" onkeypress="return isNumber(event)" TabIndex="4" MaxLength="2">
                            </asp:TextBox>

                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fas fa-chair" aria-hidden="true"></i>
                                Reason for restriction 
                            </label>
                            <asp:TextBox ID="txtRestrictionReason" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="5" MaxLength="150">
                            </asp:TextBox>

                        </div>
                    </div>

                    <div class="col-sm-2 col-xs-12 text-right pt-3">
                        <div class="form-submit" style="text-align: left !important">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary"
                                ValidationGroup="BoatSeaterMaster" TabIndex="6" OnClick="btnSubmit_Click" CausesValidation="true" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                class="btn btn-danger" TabIndex="7" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divgrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvBoatMaster" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced GridStyle"
                    AutoGenerateColumns="False" DataKeyNames="boatSeaterId" PageSize="25000" OnRowDataBound="gvBoatMaster_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Seater Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblboatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat House Name" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No. Of Seats" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblNoOfSeats" runat="server" Text='<%# Bind("NoOfSeats") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Allowed Seats (If Any Restrictions)" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAllowedNoOfSeats" runat="server" Text='<%# Bind("AllowedNoOfSeats") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reason for Restriction" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRestrictionReason" runat="server" Text='<%# Bind("RestrictionReason") %>'></asp:Label>
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
                            <ItemStyle HorizontalAlign="Center" />
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
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

