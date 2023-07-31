<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="MaximumCount.aspx.cs" Inherits="Sadmin_MaximumCount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Maximun Display Count <span style="float: right;"></span></h5>
        <hr />
        <div runat="server" id="divShow" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Name</label>
                            <asp:DropDownList ID="ddlBoatHouseId" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBoatHouseId"
                                ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="Select Boat House" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label for="lblnum">
                                <i class="fa fa-list-ol" aria-hidden="true"></i>
                                Display Count
                            </label>
                            <asp:TextBox ID="txtDispalyCount" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="1" MaxLength="10">
                            </asp:TextBox>

                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" TabIndex="11" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="12" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div id="divgrid" runat="server" style="margin-left: 100px; margin-top: 20px">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvCount" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatHouseId" Width="1000px">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatHouse Name " HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Display MaxCount" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblMaxcount" runat="server" Text='<%# Bind("Maxcount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatHouse Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
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
                                <asp:ImageButton ID="ImgDelete" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/Images/Delete.png" OnClick="ImgDelete_Click" OnClientClick="return confirm('Are you sure to Delete this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                </asp:GridView>
            </div>
        </div>

        <asp:HiddenField ID="hfUserId" runat="server" />
        <asp:HiddenField ID="hfboattypeId" runat="server" />
        <asp:HiddenField ID="hfboathouse" runat="server" />
        <asp:HiddenField ID="hfBoatHouseId" runat="server" />
        <asp:HiddenField ID="hfCreatedBy" runat="server" />
    </div>

    <%-- Newly implemented for CSRF Validation--%>   
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

