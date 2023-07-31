<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="SadminAccessRights.aspx.cs" Inherits="Sadmin_SadminAccessRights" %>

<asp:Content ID="SadminAccessRights" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .btnPosition {
            margin-top: 30px;
        }

        .starClr {
            color: red;
            font-weight: bold;
        }

        .greenClr {
            color: #198606;
            font-weight: bold;
        }

            .greenClr:hover {
                color: #198606;
            }

        .redClr {
            color: red;
            font-weight: bold;
        }

            .redClr:hover {
                color: #e41515;
            }

        .NoRecords {
            color: red;
            font-weight: bold;
            margin-left: 45%;
        }
    </style>
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Sadmin Access Rights</h5>
        <hr />

        <div class="mydivbrdr" runat="server" id="divForm">
            <div class="row p-2">
                <div class="col-sm-6 col-xs-12">
                    <div class="row m-0">
                        <div class="col-sm-6 col-xs-12">
                            <div class="form-group">
                                <label for="SadminList"><i class="fa fa-user" aria-hidden="true"></i>Sadmin Name<span class="starClr">*</span></label>
                                <asp:DropDownList
                                    ID="ddlSadminList"
                                    runat="server"
                                    CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvSadminList"
                                    runat="server"
                                    CssClass="vError"
                                    ControlToValidate="ddlSadminList"
                                    ErrorMessage="Select Sadmin Name."
                                    InitialValue="0"
                                    ValidationGroup="grpSadmin">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-3 col-xs-6">
                                        <asp:Button
                                            ID="btnSubmit"
                                            runat="server"
                                            CssClass="btn btn-primary btnPosition"
                                            Text="Submit"
                                            OnClick="btnSubmit_Click"
                                            ValidationGroup="grpSadmin" />
                                    </div>
                                    <div class="col-sm-3 col-xs-6">
                                        <asp:Button
                                            ID="btnCancel"
                                            runat="server"
                                            CssClass="btn btn-danger btnPosition"
                                            Text="Reset"
                                            OnClick="btnCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-xs-12">
                <asp:Label
                    ID="lblNoRecords"
                    Visible="false"
                    runat="server"
                    CssClass="blink NoRecords"
                    Text="No Records Found !!!.">
                </asp:Label>
            </div>
            <div class="col-sm-12 col-xs-12">
                <asp:GridView
                    ID="gvSadminDetails"
                    runat="server"
                    DataKeyNames="UniqueId"
                    CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%#Bind("UserId")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%#Bind("UserName")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployee" runat="server" Text='<%#Bind("EmpName")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Role" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserRole" runat="server" Text='<%#Bind("UserRole")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Bind("CreatedBy")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Bind("CreatedDate")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Updated By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Bind("UpdatedBy")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Updated Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUpdateDate" runat="server" Text='<%#Bind("UpdatedDate")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton
                                    ID="ImgBtnEdit"
                                    CausesValidation="false"
                                    Font-Underline="false"
                                    Width="20"
                                    CssClass="imgOutLine"
                                    runat="server"
                                    Font-Bold="true"
                                    OnClick="ImgBtnEdit_Click"
                                    ImageUrl="~/images/Edit.png" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="InActive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkBtnDelete"
                                    CausesValidation="false"
                                    Font-Underline="false"
                                    OnClick="lnkBtnDelete_Click"
                                    Text='<%# Eval("ActiveStatus").ToString() =="A"?"Active":"Inactive"%>'
                                    CssClass='<%# Eval("ActiveStatus").ToString() =="A"?"greenClr":"redClr"%>'
                                    runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <%-- Newly implemented for CSRF Validation--%>
    
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

