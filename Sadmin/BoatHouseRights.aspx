<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="BoatHouseRights.aspx.cs" Inherits="Sadmin_BoatHouseRights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">BoatHouse Rights</h5>
        <hr />

        <div runat="server" id="divShow" visible="false">

            <div class="row p-2">
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-users" aria-hidden="true"></i>Employee Name</label>
                        <asp:DropDownList ID="ddlEmpName" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEmpName"
                            ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="Select Employee Name" CssClass="vError">Select Employee Name</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label runat="server" id="Label11"><i class="fa fa-address-book" aria-hidden="true"></i>Corporate Office<span class="spStar">*</span></label>
                        <asp:DropDownList ID="ddlCorpId" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                            OnSelectedIndexChanged="ddlCorpId_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCorpId" InitialValue="Select Corporate Office"
                            ValidationGroup="BoatType" SetFocusOnError="True" CssClass="vError">Select Corporate Office</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-12">
                    <div class="form-group">
                        <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Name</label>
                        <asp:DropDownList ID="ddlBoatHouseId" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBoatHouseId"
                            ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="Select Boat House" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-sm-3 col-xs-12">
                    <div class="col-sm-3 col-xs-12 text-left pt-3">
                        <div class="form-submit" style="text-align: left !important">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BoatType" TabIndex="3" OnClick="btnSubmit_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server" visible="false" class="col-sm-6 col-xs-12">
            <asp:GridView ID="gvBranchDetails" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                AutoGenerateColumns="False" DataKeyNames="UniqueId,UserId,EmpName,BranchType,BranchId,BranchName"
                OnRowDeleting="gvBranchDetails_RowDeleting">
                <Columns>
                    <asp:TemplateField HeaderText="Sl.No." HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="User Id" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblUserId" Text='<%#Bind("UserId") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Employee Name" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Branch Type" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblBranchType" Text='<%#Bind("BranchType") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Branch Name" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:Label ID="lblBranchName" Text='<%#Bind("BranchName") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:Button ID="deleteButton" runat="server" CommandName="Delete" Text="Delete" CssClass="btn-danger"
                                OnClientClick="return confirm('Are you sure you want to delete this record ?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <HeaderStyle CssClass="gvHead" />
                <AlternatingRowStyle CssClass="gvRow" />
                <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
            </asp:GridView>
        </div>

    </div>
    <%-- Newly implemented for CSRF Validation--%>

    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

