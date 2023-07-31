<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RowerBoatAssign.aspx.cs" Inherits="Boating_RowerBoatAssign" %>

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

        <h5 class="pghr">Rower Boat Assign  <span style="float: right;">
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
                            <label for="lblboatid" id="lblrower"><i class="fa fa-user"></i>Rower <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlRower" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Value="0">Select Rower</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRower"
                                ValidationGroup="RowerBoatAssign" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Rower</asp:RequiredFieldValidator>
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
                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 chkbox checkbox-primary" style="padding-left: 0px; padding-top: 30px;">
                                    <asp:CheckBox ID="ChkMultipleTrip" Text=" Multiple Trip Rights" runat="server"
                                        CssClass="chkboxForAutoEnd" RepeatColumns="7" CellPadding="7" CellSpacing="7" TabIndex="17" />
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
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">

                <div style="text-align: right;">
                    <div runat="server" id="Search">
                        Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="16px" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                    </div>
                    <asp:GridView ID="gvRowerBoatAssign" runat="server" AllowPaging="True"
                        CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" DataKeyNames="UniqueId,RowerId,RowerName,BoatTypeId,BoatType,
                    SeaterId,SeaterName,BoatHouseId,BoatHouseName, ActiveStatus,MultiTripRights"
                        PageSize="25000">

                        <Columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowNumber" runat="server" Text='<%#Bind("RowNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rower Name" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="MultiTrip Rights" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <%-- <asp:Label ID="lblBMaster" runat="server" Text='<%# Eval("BMaster").ToString() == "Y" ? "Yes" : "No" %>'></asp:Label>--%>
                                    <asp:Label ID="lblMultiTripRights" runat="server" Text='<%# Eval("MultiTripRights").ToString() == "Y" ? "Yes" : "No" %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                        ImageUrl="~/images/Delete.png" runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to Delete this record?');" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gvHead" />
                        <AlternatingRowStyle CssClass="gvRow" />
                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                    </asp:GridView>
                    <div style="text-align: left;">
                        <asp:Button ID="back" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" Enabled="false" OnClick="back_Click" />
                        &nbsp
                        <asp:Button ID="Next" Visible="true" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="Next_Click" />

                    </div>
                    <div style="text-align: left;">
                        <asp:Button ID="backSearch" runat="server" CssClass="btn btn-color" Visible="false" Text="← Previous" Enabled="false" OnClick="backSearch_Click" />
                        &nbsp
                        <asp:Button ID="NextSearch" Visible="false" CssClass="btn btn-color" runat="server" Text="Next →" OnClick="NextSearch_Click" />
                        &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" Text="← Back To List" OnClick="BackToList_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfUniqueId" runat="server" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>
