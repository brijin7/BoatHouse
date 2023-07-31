<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Common/DataType.aspx.cs" Inherits="DataType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Maincontent" Runat="Server">
  <div class="form-body">
       
            <div class="col-xs-12" style="padding: 0px;">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                        <h5 class="pghr">Data Type <span style="float: right;">
                    <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span></h5>
                        <hr />
                    </div>
                </div>
                <div class="form-body" id="divEntry" runat="server">
                    <div class="col-xs-12 col-sm-12" style="padding: 0; margin-top: 10px;">
                        <div class="col-xs-12 col-sm-6">
                            <div class="col-xs-12 col-sm-4">
                              Data Type Name <span class="spStar">*</span>
                            </div>
                            <div class="col-xs-12 col-sm-8">
                                <asp:TextBox ID="txtConfigName" runat="server" CssClass="form-control inputboxstyle"
                                    MaxLength="20" Font-Size="14px" TabIndex="1" AutoComplete="Off">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvConfigName" runat="server" ControlToValidate="txtConfigName"
                                    SetFocusOnError="True" CssClass="vError">Enter Data Type Name</asp:RequiredFieldValidator>
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
            </div>
     
        <div id="divGrid" runat="server" visible="false" style="margin-top: 2%">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvConfiguration" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="TypeId,TypeName,ActiveStatus">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl.No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Data Type Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblconfigName" Text='<%#Bind("TypeName") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ActiveStatus" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblactivestatus" Text='<%#Bind("ActiveStatus") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
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

