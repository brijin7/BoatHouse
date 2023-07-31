<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    CodeFile="SmsBlock.aspx.cs" Inherits="Sadmin_SmsBlock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">SMS Block <span style="float: right;" />
            <%-- <asp:LinkButton ID="lbtnBlockReason" CssClass="lbtnNew" runat="server" OnClick="lbtnBlockReason_Click"> 
                <i class="fas fa-receipt"></i>View Report</asp:LinkButton></span> --%></h5>
        <hr />
        <div class="mydivbrdr" runat="server" id="divEntry">
            <div class="row p-2">
                <div class="col-sm-12 col-xs-12">


                    <div class="row m-0 ml-5">
                        <div class="col-sm-3 col-xs-12" style="background-color: #93ff501a">
                            <div class="form-group">
                                <label for="boat"><i class="fa fa-envelope" aria-hidden="true"></i>Block SMS</label>
                                <asp:RadioButtonList ID="rblSmsBlock" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="3" CssClass="rbl">
                                    <asp:ListItem Value="Y" Selected="true">Block</asp:ListItem>
                                    <asp:ListItem Value="N">UnBlock</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rblSmsBlock"
                                    ValidationGroup="grpSmsBlock" SetFocusOnError="True" CssClass="vError">Select Application Type</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12" style="margin-top: 25px">
                            <div class="form-submit" style="text-align: left !important">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="grpSmsBlock" TabIndex="5" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger" TabIndex="6" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divGrid" runat="server" class="col-sm-6 col-xs-6" visible="false">
            <div class="table-responsive" style="overflow: hidden">
                <div style="text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvSmsBlock" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="MessageBlock">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SMS Status" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSmsBlock" runat="server" Text='<%# Eval("MessageBlock").ToString() == "Y"? "Blocked": "UnBlocked" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click"
                                    EnableViewState="false" Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active Status" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gvHead" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <%-- Newly implemented for CSRF Validation--%>

    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>
