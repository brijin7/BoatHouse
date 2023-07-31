<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    CodeFile="~/Boating/MaterialPurchase.aspx.cs" Inherits="Department_Boating_MaterialPurchase" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script>
        $(document).ready(function () {
            $(".decimal").keypress(function (event) {
                return isNumber(event, this);
            });
        });

        function isNumber(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (
                (charCode != 45 || $(element).val().indexOf('-') != -1) &&
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&
                (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function minmax(value, min, max) {
            if (parseInt(value) < min || isNaN(parseInt(value)))
                return min;
            else if (parseInt(value) > max)
                return max;
            else return value;
        }
    </script>
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Material Purchase <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                       <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-6 col-xs-12">
                        <div class="row">
                            <div class="col-sm-6 col-xs-12" runat="server" visible="false">
                                <div class="form-group">
                                    <label runat="server" id="Label9"><i class="fas fa-list"></i>Purchase Id</label>
                                    <asp:TextBox ID="txtPurchaseId" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off" onkeypress="return LettersWithSpaceOnly(event);"
                                        MaxLength="50" Font-Size="14px" TabIndex="1">
                                    </asp:TextBox>

                                </div>
                            </div>

                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label2"><i class="fa fa-calendar" aria-hidden="true"></i>Purchase Date <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="form-control datepicker" AutoComplete="off"
                                        MaxLength="50" Font-Size="14px" TabIndex="2">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPurchaseDate"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Select Purchase Date</asp:RequiredFieldValidator>

                                </div>
                            </div>

                            <div class="col-sm-8 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label6"><i class="fa fa-address-book" aria-hidden="true"></i>Vendor Name <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtVendorRemark" runat="server" CssClass="form-control" AutoComplete="off"
                                        MaxLength="100" Font-Size="14px" TabIndex="3">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtVendorRemark"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Vendor Name</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtVendorRemark" ForeColor="Red"
                                        ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="InValid" CssClass="vError" />

                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-8 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label4"><i class="fas fa-list"></i>Item <span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlItem" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" TabIndex="4">
                                        <asp:ListItem Value="0"> Select Item</asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlItem"
                                        ValidationGroup="OfferMaster" InitialValue="0" SetFocusOnError="True" CssClass="vError">Select Item</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label5"><i class="fas fa-list"></i>Received Quantity <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtReceivedQu" runat="server" CssClass="form-control" AutoComplete="off" onkeypress="return isNumber(event);"
                                        MaxLength="7" Font-Size="14px" TabIndex="5" AutoPostBack="true" OnTextChanged="txtReceivedQu_TextChanged">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtReceivedQu"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Received Quantity </asp:RequiredFieldValidator>

                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label3"><i class="fas fa-list"></i>Accepted Quantity <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtAcceptedQu" runat="server" CssClass="form-control" AutoComplete="off" onkeypress="return isNumber(event);"
                                        MaxLength="7" Font-Size="14px" TabIndex="6" OnTextChanged="txtAcceptedQu_TextChanged" AutoPostBack="true">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAcceptedQu"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Accepted Quantity </asp:RequiredFieldValidator>

                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label10"><i class="fas fa-list"></i>Rejected Quantity <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtRejectedQu" runat="server" CssClass="form-control" AutoComplete="off" onkeypress="return isNumber(event);"
                                        MaxLength="12" Font-Size="14px" TabIndex="7" Enabled="false">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtRejectedQu"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Received Quantity </asp:RequiredFieldValidator>

                                </div>
                            </div>

                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label11"><i class="fas fa-list"></i>Rejected Reason <span class="spStar"></span></label>
                                    <asp:TextBox ID="txtRejectedReson" runat="server" CssClass="form-control" AutoComplete="off"
                                        MaxLength="100" Font-Size="14px" TabIndex="8">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtRejectedReson" ForeColor="Red"
                                        ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="InValid!!" ValidationGroup="OfferMaster" CssClass="vError" />

                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label1"><i class="fas fa-money-bill-wave-alt"></i>Purchase Rate <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtPurchaseRate" runat="server" CssClass="form-control decimal" AutoComplete="off" onkeypress="return isNumber(event);"
                                        MaxLength="7" Font-Size="14px" TabIndex="9">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPurchaseRate"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Purchase Rate</asp:RequiredFieldValidator>

                                </div>
                            </div>

                            <div id="divSubmit" runat="server" class="col-sm-6 col-xs-12 form-submit text-center" style="margin-top: 30px;">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" Width="80px" ValidationGroup="OfferMaster" TabIndex="10" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" Width="80px" TabIndex="11" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" Width="80px" CssClass="btn btn-danger" TabIndex="12" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6 col-xs-12">
                        <div id="divAddGrid" runat="server" visible="false">
                            <div class="table-responsive">

                                <asp:GridView ID="gvAddGrid" runat="server" AllowPaging="True" OnRowDataBound="gvAddGrid_RowDataBound"
                                    CssClass="CustomGrid table table-bordered table-condenced" AutoGenerateColumns="False"
                                    DataKeyNames="PurchaseId" PageSize="25000">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="PurchaseId" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseId" runat="server" Text='<%# Bind("PurchaseId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ItemId " HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemDescriptions" runat="server" Text='<%# Bind("ItemDescription") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Received " HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIReceivedQty" runat="server" Text='<%# Bind("ReceivedQty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Accepted" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("AcceptedQty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rejected" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRejectedQty" runat="server" Text='<%# Bind("RejectedQty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Reason" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRejectionReason" runat="server" Text='<%# Bind("RejectionReason") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Purchase Rate" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseRate" runat="server" Text='<%# Bind("PurchaseRate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtnAddEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnAddEdit_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtnAddDelete" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                                    runat="server" Font-Bold="true" ImageUrl="~/images/Delete.png" OnClick="ImgBtnAddDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this entry?');" />

                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>

                                    <HeaderStyle CssClass="gvHead" />
                                    <AlternatingRowStyle CssClass="gvRow" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                </asp:GridView>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divgrid" runat="server">
            <div class="table-responsive">
                <asp:GridView ID="gvMaterialPurchase" runat="server" AllowPaging="True"
                    CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="PurchaseId" PageSize="25000" OnRowDataBound="gvMaterialPurchase_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PurchaseId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseId" runat="server" Text='<%# Bind("PurchaseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Purchase Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("PurchaseDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ItemId " HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Vendor Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblVendorRef" runat="server" Text='<%# Bind("VendorRef") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="No Of Items" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnNoOfItems" runat="server" Text='<%# Bind("NoOfItems") %>' OnClick="lbtnNoOfItems_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" ForeColor="#33cc33" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Purchase Rate" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseRate" runat="server" Text='<%# Bind("PurchaseRate") %>'></asp:Label>
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
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click1" OnClientClick="return confirm('Are you sure you want to Inactive this record?');" />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure you want to Active this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfBoatHouseId" runat="server" />
    <asp:HiddenField ID="hfBoatHouseName" runat="server" />

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <ajax:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="pnlMaterial" DragHandleID="pnlDrag3"></ajax:DragPanelExtender>
    <ajax:ModalPopupExtender ID="MpeMaterial" runat="server" BehaviorID="MpeMaterial" TargetControlID="HiddenField1" PopupControlID="pnlMaterial"
        BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>

    <asp:Panel ID="pnlMaterial" runat="server" CssClass="Msg">
        <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
            <div class="modal-content" style="width: 750px">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Material Details</h5>
                    <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                </div>
                <div class="modal-body">

                    <asp:GridView ID="gvMaterialDetails" runat="server" AllowPaging="True"
                        CssClass="CustomGrid table table-bordered table-condenced" AutoGenerateColumns="False" Width="100%"
                        PageSize="25000">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PurchaseId" HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPurchaseId" runat="server" Text='<%# Bind("PurchaseId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ItemId " HeaderStyle-CssClass="grdHead" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("ItemId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("ItemDescription") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Received Quantity " HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblIReceivedQty" runat="server" Text='<%# Bind("ReceivedQty") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Accepted Quantity" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("AcceptedQty") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rejected Quantity" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblRejectedQty" runat="server" Text='<%# Bind("RejectedQty") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Rejection Reason" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblRejectionReason" runat="server" Text='<%# Bind("RejectionReason") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Purchase Rate" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblPurchaseRate" runat="server" Text='<%# Bind("PurchaseRate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>

                        </Columns>

                        <HeaderStyle CssClass="gvHead" />
                        <AlternatingRowStyle CssClass="gvRow" />
                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

