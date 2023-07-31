<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Masters/TaxMaster.aspx.cs" Inherits="TaxMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .Note {
            font-size: 12px;
            color: red;
        }
    </style>
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
    </script>
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Tax Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i>Add New</asp:LinkButton></span> </h5>
        <hr />
        <div class="row" id="divnew" runat="server">
            <div class="col-sm-9 col-xs-12">
                <div class="mydivbrdr">
                    <div class="row p-2">
                        <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label11"><i class="fa fa-address-book" aria-hidden="true"></i>Corporate Office<span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlCorpId" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                                        OnSelectedIndexChanged="ddlCorpId_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCorpId" InitialValue="Select Corporate Office"
                                        ValidationGroup="AddtaxMaster" SetFocusOnError="True" CssClass="vError">Select Corporate Office</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label for="boat">Boat House Name <span class="spStar">*</span></label>
                                <asp:DropDownList ID="ddlBoatHouseId" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBoatHouseId"
                                    ValidationGroup="AddtaxMaster" SetFocusOnError="True" InitialValue="Select Boat House" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div runat="server" visible="false">
                            <div class="form-group">
                                <label for="lblTaxId">Tax Id</label>
                                <asp:TextBox ID="txttaxid" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="10">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label for="lblTaxNamae">Service Name <span class="spStar">*</span></label>
                                <asp:DropDownList ID="ddlServiceName" CssClass="form-control inputboxstyle" runat="server" TabIndex="1">
                                    <asp:ListItem Value="0"> Select Service Name</asp:ListItem>
                                    <asp:ListItem Value="1">Boating</asp:ListItem>
                                    <asp:ListItem Value="2">Boating Other Services</asp:ListItem>
                                  <%--  <asp:ListItem Value="3">Hotel</asp:ListItem>
                                    <asp:ListItem Value="4">Tour</asp:ListItem>--%>
                                    <asp:ListItem Value="5">Restaurant</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlServiceName"
                                    ValidationGroup="AddtaxMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Service Name</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label for="lblCGST">Tax Description <span class="spStar">*</span></label>
                                <asp:TextBox ID="txtTaxDesc" runat="server" AutoComplete="Off" CssClass="form-control" TabIndex="2" MaxLength="100">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTaxDesc"
                                    ValidationGroup="AddtaxMaster" SetFocusOnError="True" CssClass="vError">Enter Tax Description</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            <div class="form-group">
                                <label for="lblSGST">Tax % <span class="spStar">*</span></label>
                                <asp:TextBox ID="txtTaxPerc" runat="server" AutoComplete="Off" CssClass="form-control decimal" TabIndex="3"
                                    MaxLength="5">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTaxPerc"
                                    ValidationGroup="AddtaxMaster" SetFocusOnError="True" CssClass="vError">Enter Tax %</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-1 col-xs-12 text-right pt-4">
                            <div class="form-group">
                                <asp:Button ID="BtnAdd" runat="server" Text="Add" class="btn btn-primary" ValidationGroup="AddtaxMaster" TabIndex="4" OnClick="BtnAdd_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <div id="divAddGrid" runat="server">
                    <div class="table-responsive">
                        <div style="margin-left: auto; margin-right: auto; text-align: center;">
                            <asp:Label ID="lblGridMsgAdd" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </div>
                        <asp:GridView ID="gvAddTaxMaster" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced GridStyle"
                            AutoGenerateColumns="False" DataKeyNames="UniqueId" PageSize="10">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Uid" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUniqueId" runat="server" Text='<%# Bind("UniqueId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service Name" HeaderStyle-CssClass="grdHead" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceId" runat="server" Text='<%# Bind("ServiceId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Description" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxDesc" runat="server" Text='<%# Bind("TaxDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax %" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxPerc" runat="server" Text='<%# Bind("TaxPercentage") %>'></asp:Label>
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
                                            runat="server" Font-Bold="true" ImageUrl="~/images/Delete.png" OnClick="ImgBtnAddDelete_Click"
                                            OnClientClick="return confirm('Are you sure you want to delete this entry?');" />
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

                <div class="mydivbrdr">
                    <div class="row p-2">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label for="lbleffectivefrom" id="lbleffectivefrom" runat="server">
                                    <i class="fa fa-calendar" aria-hidden="true"></i>Effective From <span class="spStar">*</span></label>
                                <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass=" form-control taxdatepicker" AutoComplete="Off" TabIndex="5">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEffectiveFrom"
                                    ValidationGroup="taxMaster" SetFocusOnError="True" CssClass="vError">Enter Effective From</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label for="lblTaxId">Reference Number <span class="spStar">*</span></label>
                                <asp:TextBox ID="txtRefNum" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="20" TabIndex="6">
                                </asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtRefNum"
                                    ValidationGroup="taxMaster" SetFocusOnError="True" CssClass="vError">Enter Reference Number</asp:RequiredFieldValidator>
                           
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label for="lblRefDate" id="Label3" runat="server">
                                    <i class="fa fa-calendar" aria-hidden="true"></i>Reference Date <span class="spStar">*</span></label>
                                <asp:TextBox ID="txtRefDate" runat="server" CssClass="form-control taxdatepicker" AutoComplete="Off" TabIndex="7">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtRefDate"
                                    ValidationGroup="taxMaster" SetFocusOnError="True" CssClass="vError">Enter Reference Date</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-3 col-xs-12">
                <div class="col-sm-12 p-2">
                    <div class="panel panel-success">
                        <div class="panel-heading">Reference Document <span class="spStar">*</span></div>
                        <div class="panel-body">
                            <asp:FileUpload ID="DocUpload" runat="server" TabIndex="8" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="DocUpload"
                                ValidationGroup="taxMaster" SetFocusOnError="True" CssClass="vError">Enter Link</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12" style="position: absolute; bottom: 8px; right: 16px;">
                    <div class="form-submit">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="taxMaster" TabIndex="9" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="10" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>

            <div class="col-sm-12 col-xs-12">
                <span class="Note">*Note - After adding of records only Submit button will be enabled.
                </span>
            </div>
        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvTaxMaster" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="TaxId" PageSize="25000" OnDataBound="gvTaxMaster_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax Id" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxId" runat="server" Text='<%# Bind("TaxId") %>' Font-Bold="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="Boat House Name" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Service Name | Tax Description  | Tax %" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Effective From" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveFrom" runat="server" Text='<%# Bind("EffectiveFrom") %>' ForeColor="Green" Font-Bold="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Effective Till" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveTill" runat="server" Text='<%# Bind("EffectiveTill") %>' ForeColor="Red" Font-Bold="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reference Number" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRefNum" runat="server" Text='<%# Bind("RefNum") %>' Font-Bold="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reference Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRefDate" runat="server" Text='<%# Bind("RefDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Document" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblRefDocLink" runat="server" Text='<%# Bind("RefDocLink") %>'></asp:Label>--%>
                                <asp:ImageButton ID="imgbtnDownload" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" OnClick="imgbtnDownload_Click" ImageUrl="~/images/download.png" CommandArgument='<%# Bind("RefDocLink") %>' ToolTip="Download Reference Document." />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hfUniqueId" />
    <asp:HiddenField runat="server" ID="hfUserId" />
    <asp:HiddenField runat="server" ID="hfCreatedBy" />


    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

