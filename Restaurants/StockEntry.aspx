<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="StockEntry.aspx.cs" Inherits="Restaurants_StockEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
     

    <script>
        $(document).ready(function () {
            $(".decimal").keypress(function (event) {
                return isNumber(event, this);
            });

            $(".StartDate").datepicker({
                dateFormat: 'dd/mm/yy',
                maxDate: 700,
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,

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

        $('#<%=txtOpeninQty.ClientID%>').change(function () {

            var a = $('#<%=txtOpeninQty.ClientID%>').val();

            if (a < 0) {
                alert('Opening Quantity Should Accept Negative Values');
                $('#<%=txtOpeninQty.ClientID%>').val('');
                $('#<%=txtOpeninQty.ClientID%>').focus();
            }
        });




    </script>


    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Restaurant Stock Entry <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="btnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <%--Newly Added --%>
        <div class="mydivbrdr" runat="server" id="fromTodate" visible="false">
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDates" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtFromDates"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="lbleffectiveTo" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDates" runat="server" CssClass="form-control frmDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtToDates"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblCatNamee" id="lblidCatName"><i class="fas fa-ruler-vertical" aria-hidden="true"></i>Item Category <span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlCatName" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCatName_SelectedIndexChanged"
                            TabIndex="3">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblCatItem" id="lblidCatItem"><i class="fas fa-ruler-vertical" aria-hidden="true"></i>Item Name <span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlCatItem" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true"
                            TabIndex="3">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2" style="margin-top: 30px;">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />
                </div>
            </div>
        </div>

        <%--Newly Added --%>

        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblItemCtgy"><i class="fas fa-ruler-vertical"></i>Item Category <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" TabIndex="4" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Item Category </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlUOM"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Item Category</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblItemName"><i class="fas fa-ruler-vertical"></i>Item Name <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlItemName" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" TabIndex="4">
                                <asp:ListItem Value="0">Select Item Name </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlUOM"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Item Name</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" runat="server">
                        <label for="lblTypes" id="lblResFromdate"><i class="fa fa-calendar" aria-hidden="true"></i>Date <span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" CssClass="form-control datepicker" runat="server" TabIndex="1" AutoComplete="Off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">Select Purchase Date</asp:RequiredFieldValidator>
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
                            <label runat="server" id="Label1"><i class="fas fa-list"></i>Quantity <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtOpeninQty" runat="server" CssClass="form-control decimal"
                                MaxLength="5" Font-Size="14px" TabIndex="6" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtOpeninQty"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" CssClass="vError">Enter Opening Quantity</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="lblRef"><i class="fas fa-ruler-vertical"></i>Reference<span class="spStar">*</span></label>
                            <asp:TextBox ID="txtRef" runat="server" CssClass="form-control"
                                MaxLength="50" Font-Size="14px" TabIndex="5" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtRef"
                                ValidationGroup="ItemMaster" SetFocusOnError="True" CssClass="vError">Enter Reference</asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>
                <div class="col-md-12 col-lg-12 col-sm-12 text-right">
                    <div class="form-submit">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Employee" OnClick="btnSubmit_Click" TabIndex="14" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" OnClick="btnCancel_Click" TabIndex="15" />

                    </div>
                </div>
            </div>
        </div>
        <div class="table-responsive">
            <div style="margin-left: auto; margin-right: auto; text-align: center;" id="divlabel" runat="server">
                <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
            <div id="divGrid" runat="server" style="margin-top: 20px">
                <asp:GridView ID="gvStockEntry" runat="server" AllowPaging="True"
                    CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False"
                    DataKeyNames="StockId,ItemCategoryId,ItemNameId,Date,UOM,Quantity,Reference" PageSize="25000" OnRowDataBound="gvStockEntry_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Category Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCategoryId" runat="server" Text='<%# Bind("ItemCategoryId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Category Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("ItemCategory") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Item Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblNameId" runat="server" Text='<%# Bind("ItemNameId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
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
                        <%-- <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');" />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>--%>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>

            </div>
            <div runat="server" id="PrevNext" visible="false">
                <asp:Button ID="back" runat="server" CssClass="btn btn-color mg" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                &nbsp;
                      <asp:Button ID="Next" Visible="true" CssClass="btn btn-color mg" runat="server" Text="Next →" OnClick="Next_Click" />
                &nbsp;
                       <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />

            </div>
        </div>

    </div>

    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

