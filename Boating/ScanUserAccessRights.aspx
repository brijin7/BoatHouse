<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="ScanUserAccessRights.aspx.cs" Inherits="Boating_ScanUserAccessRights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .chkChoice input {
            margin-right: 5px;
        }
    </style>
    <script>
        $(function checkUncheck() {
            $("[id*=chkSelectAll]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=chkSeaterType] input").prop("checked", true);
                }
                else {
                    $("[id*=chkSeaterType] input").prop("checked", false);
                }
            });

            $("[id*=chkSeaterType] input").bind("click", function () {
                if ($("[id*=chkSeaterType] input:checked").length == $("[id*=chkSeaterType] input").length) {
                    $("[id*=chkSelectAll]").prop("checked", true);
                }
                else {

                    $("[id*=chkSelectAll]").prop("checked", false);
                }
            });
        });
    </script>

    <div class="form-body col-sm-11 col-xs-12">
        <h5 class="pghr">Scan User Access Rights<span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span></h5>
        <hr />
        <br />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblBoatType" id="lblboattype"><i class="fa fa-ship" aria-hidden="true"></i>Boat Type <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlBoatType" CssClass="form-control inputboxstyle" runat="server"
                                TabIndex="2" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlBoatType"
                                ValidationGroup="RowerBoatAssign" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat Type</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblboatid" id="lblrower"><i class="fa fa-user"></i>User <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlRower" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Value="0">Select User</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRower"
                                ValidationGroup="RowerBoatAssign" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select User Name</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-6 col-xs-12">
                        <asp:Label ID="lblSeatMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        <div id="divseater" runat="server" visible="false">
                            <div class="col-sm-12" id="divSeaterTypeall" runat="server">
                                <div class="panel panel-success">
                                    <div class="panel-heading panelheadchk">
                                        Boat Seater <span class="spStar">*</span>
                                        <asp:CheckBox ID="chkSelectAll" Text="Select All" runat="server" Font-Bold="True"
                                            CssClass="chkbox checkbox-primary" TabIndex="3" />
                                    </div>
                                    <div class="panel-body">
                                        <asp:CheckBoxList ID="chkSeaterType" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ValidationGroup="RowerBoatAssign"
                                            CssClass="chkChoice" CellPadding="5" CellSpacing="5" Width="100%" TabIndex="4">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-submit">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="RowerBoatAssign" TabIndex="5" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="6" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvScanUser" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="UniqueId,UserId,UserName,BoatTypeId,BoatType,
                    SeaterId,SeaterName,BoatHouseId,BoatHouseName, ActiveStatus"
                    PageSize="25000" OnRowDataBound="gvScanUser_RowDataBound">

                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Seater Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
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
                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                        </asp:TemplateField>
                        <%--  <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    ImageUrl="~/images/Delete.png" runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to Delete this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </asp:TemplateField>--%>
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
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfUniqueId" runat="server" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>


