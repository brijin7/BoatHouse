<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" Async="true" AutoEventWireup="true"
    CodeFile="~/Common/BranchDesignationMap.aspx.cs" Inherits="DepartmentDesignationMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
  
            <div class="form-body col-sm-12 col-xs-12">
                <h5 class="pghr">Branch Designation Mapping <span style="float: right;">
                    <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
                <hr />
                <div id="divEntry" runat="server">
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label11"><i class="fa fa-address-book" aria-hidden="true"></i>Corporate Office<span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlCorpId" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                                        OnSelectedIndexChanged="ddlCorpId_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCorpId" InitialValue="Select Corporate Office"
                                        ValidationGroup="Mapping" SetFocusOnError="True" CssClass="vError">Select Corporate Office</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblboatnum">
                                        Branch <strong style="color: Red">*</strong>
                                    </label>
                                    <asp:DropDownList runat="server" ID="ddlBranchCode" CssClass="form-control" TabIndex="1" AutoComplete="off"
                                        OnSelectedIndexChanged="dddlBranchCode_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please, Select Branch"
                                        ControlToValidate="ddlBranchCode" SetFocusOnError="True" InitialValue="Select Branch" CssClass="vError"
                                        ValidationGroup="Mapping"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblboatnum">
                                        Designation <strong style="color: Red">*</strong>
                                    </label>
                                    <asp:DropDownList ID="ddlDesignation" CssClass="form-control inputboxstyle" runat="server" TabIndex="2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlDesignation"
                                        ValidationGroup="Mapping" SetFocusOnError="True" InitialValue="Select Designation"
                                        CssClass="vError">Please, Select Designation</asp:RequiredFieldValidator>
                                </div>
                            </div>
                           
                            <div class="col-sm-3 col-xs-12 text-right pt-3">
                                <div class="form-submit">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Mapping"
                                        TabIndex="3" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger"
                                        TabIndex="4" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="divgrid" runat="server">
                    <div class="table-responsive">
                        <asp:GridView ID="gvMapping" runat="server" AllowPaging="false" CssClass="gvv display table table-bordered table-condenced"
                            AutoGenerateColumns="False" DataKeyNames="UniqueId,BranchId,BranchName,Designation,DesignationName,CorpId,ActiveStatus,CreatedBy">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Corporate" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCorpId" runat="server" Text='<%# Bind("CorpId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblCorpName" runat="server" Text='<%# Bind("CorpName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("BranchName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Designation" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Bind("DesignationName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false"
                                            Width="20" CssClass="imgOutLine" runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click"
                                            EnableViewState="false"
                                            Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                            runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click"
                                            Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>'
                                            OnClientClick="return confirm('Are you sure you want to Inactive this record?');" />
                                        <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                            runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click"
                                            OnClientClick="return confirm('Are you sure you want to Active this record?');"
                                            Visible='<%# Eval("ActiveStatus").ToString() == "D"? true: false %>' />
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
       
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

