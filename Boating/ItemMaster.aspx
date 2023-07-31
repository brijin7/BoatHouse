<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Boating/ItemMaster.aspx.cs" Inherits="Department_Boating_ItemMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <%--    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js">--%>
    <%--    </script>--%>

    <script>
        $(document).ready(function () {
            $(".decimal").keypress(function (event) {
                return isNumber(event, this);
            });
        });

        function isNumber(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (
                (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // Check minus and only once.
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // Check for dots and only once.
                (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        $(document).ready(function () {
            $('#<%=txtItemRate.ClientID%>').change(function () {
                var a = $('#<%=txtItemRate.ClientID%>').val();
                if (a < 0) {
                    alert('Item Rate Should Not Accept Neagtive Values');
                    $('#<%=txtItemRate.ClientID%>').val('');
                    $('#<%=txtItemRate.ClientID%>').focus();
                }
            });

            $('#<%=txtOpeninQty.ClientID%>').change(function () {

                var a = $('#<%=txtOpeninQty.ClientID%>').val();

                if (a < 0) {
                    alert('Opening Quantity Should Accept Negative Values');
                    $('#<%=txtOpeninQty.ClientID%>').val('');
                    $('#<%=txtOpeninQty.ClientID%>').focus();
                }
            });

        });
    </script>
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Item Master <span style="float: right;">
            <asp:LinkButton ID="btnNew" CssClass="lbtnNew" runat="server" OnClick="btnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="lblItemName"><i class="fa fa-comments"></i>Item Description <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtItemDescription" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="150" Font-Size="14px" TabIndex="1" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtItemDescription"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" CssClass="vError">Enter Item Description</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12" style="display: none">
                        <div class="form-group">
                            <label runat="server" id="lblItemId">Item Id</label>
                            <asp:TextBox ID="txtItemId" runat="server" CssClass="form-control inputboxstyle" Enabled="false"
                                MaxLength="150" Font-Size="14px" placeholder="Enter Config ID" TabIndex="2">
                            </asp:TextBox>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="txtItemType"><i class="fa fa-list-alt" aria-hidden="true"></i>Item Type <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" TabIndex="3">
                                <asp:ListItem Value="0">Select Item Type</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlItemType"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Item Type</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="txtUOM"><i class="fas fa-ruler-vertical"></i>Unit Of Measure <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlUOM" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" TabIndex="4">
                                <asp:ListItem Value="0">Select Unit Of Measure </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlUOM"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Unit Of Measure</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="txtItemRate"><i class="fas fa-star checked"></i>Item Rate(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtItemRate" runat="server" CssClass="form-control decimal"
                                MaxLength="5" Font-Size="14px" TabIndex="5" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtItemRate"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" CssClass="vError">Enter Item Rate</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label1"><i class="fas fa-list"></i>Opening Quantity <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtOpeninQty" runat="server" CssClass="form-control decimal"
                                MaxLength="5" Font-Size="14px" TabIndex="6" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtOpeninQty"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" CssClass="vError">Enter Opening Quantity</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="ItemMaster" CssClass="btn btn-primary" TabIndex="7" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="btn btn-danger" TabIndex="8" OnClick="btnCancel_Click" />
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
                <asp:GridView ID="gvItemMaster" runat="server" AllowPaging="True"
                    CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="ItemId" PageSize="25000" OnRowDataBound="gvItemMaster_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ItemId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Type" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblItemType" runat="server" Text='<%# Bind("ItemType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Item Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblItemTypeName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Description" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("ItemDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UOM" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUOM" runat="server" Text='<%# Bind("UOM") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit Of Measure" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUOMName" runat="server" Text='<%# Bind("UOMName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Rate" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblItemRate" runat="server" Text='<%# Bind("ItemRate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Opening Quantity" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOpeningQty" runat="server" Text='<%# Bind("OpeningQty") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
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
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

