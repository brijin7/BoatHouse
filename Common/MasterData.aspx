<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Common/MasterData.aspx.cs" Inherits="MasterData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">


        <div class="col-xs-12" style="padding: 0px;">
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                    <h5 class="pghr">Master Data<span style="float: right;">
                        <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                            <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span></h5>
                    <hr />
                </div>
            </div>


            <div class="col-xs-12" style="padding: 0;" id="divEntry" runat="server">
                <div class="row" style="padding: 0; margin-top: 10px;">

                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label runat="server" id="Label3"><i class="fa fa-ship" aria-hidden="true"></i>Master Type
                              <span class="spStar">*</span> 
                            </label>
                            <asp:DropDownList ID="ddlConfigType" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" TabIndex="1" OnSelectedIndexChanged="ddlConfigType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Master Type</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlConfigType"
                                SetFocusOnError="True" InitialValue="Select Master Type" CssClass="vError">Select Master Type</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server">
                        <div class="form-group">
                            <label runat="server" id="Label1"><i class="fa fa-ship" aria-hidden="true"></i>Master Data Name
                                <span class="spStar">*</span> 
                            </label>
                            <asp:TextBox ID="txtConfigName" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="20" Font-Size="14px" TabIndex="2" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtConfigName"
                                SetFocusOnError="True" CssClass="vError">Enter Data Name</asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>

                <div class="col-xs-12" style="padding: 0; margin-top: 10px;">
                    <div class="col-xs-12 text-center">
                        <span style="display: inline-block;">
                            <asp:Button runat="server" ID="btnSubmit" Text="Submit" Width="100px" CssClass="btn btn-primary" TabIndex="3" OnClick="btnSubmit_Click" />
                        </span>

                        <span style="display: inline-block">
                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" Width="100px" CssClass="btn btn-danger" TabIndex="4" CausesValidation="false" OnClick="btnCancel_Click" />
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-xs-12" runat="server" id="divGrid" visible="false" style="margin-top: 2%">

            <div class="col-xs-12">
                <asp:GridView ID="gvConfiguration" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="TypeId,ConfigId,ConfigName">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Master Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTypeName" runat="server" Text='<%# Bind("TypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText=" Master Data Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblConfigName" runat="server" Text='<%# Bind("ConfigName") %>'></asp:Label>
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
                                    runat="server" Font-Bold="true" ImageUrl="~/Images/Edit.png" OnClick="ImgBtnEdit_Click"
                                    Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');"
                                    Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />

                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');"
                                    Visible='<%# Eval("ActiveStatus").ToString() == "D"? true: false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
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

