<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" Async="true" AutoEventWireup="true" CodeFile="~/Masters/FeedBack.aspx.cs" Inherits="Boating_FeedBack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="form-body col-sm-9 col-xs-12">
        <h5 class="pghr">Feed Back <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i>Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label runat="server" id="Label3"><i class="fa fa-star checked"></i>FeedBack Id</label>
                            <asp:TextBox ID="txtFeedBackId" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off" onkeypress="return isNumber(event);"
                                MaxLength="20" Font-Size="14px" TabIndex="1">
                            </asp:TextBox>

                        </div>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label2">Given By</label>
                            <asp:TextBox ID="txtGivenBy" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off"
                                MaxLength="20" Font-Size="14px" TabIndex="1">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtGivenBy" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Special Characters Not Allowed" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtGivenBy"
                                ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Given By </asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label4">Mobile No</label>
                            <asp:TextBox ID="txtMobileNumbr" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off" onkeypress="return isNumber(event);"
                                MaxLength="10" Font-Size="14px" TabIndex="2">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ControlToValidate="txtMobileNumbr" ErrorMessage="Invalid"
                                ValidationExpression="[0-9]{10}" ForeColor="Red"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMobileNumbr"
                                ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Mobile No</asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label5">Email Id</label>
                            <asp:TextBox ID="txtMailId" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off"
                                MaxLength="60" Font-Size="14px" TabIndex="3">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtMailId"
                                SetFocusOnError="True" CssClass="vError" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                Display="Dynamic" ErrorMessage="Invalid" ValidationGroup="OfferMaster" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMailId"
                                ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Email Id</asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>
                <div class="row p-2">
                    <div class="col-sm-9 col-md-9 col-lg-9 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label6">Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off"
                                MaxLength="300" Font-Size="14px" TabIndex="4" Rows="2" TextMode="MultiLine">
                            </asp:TextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAddress"
                                ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Address</asp:RequiredFieldValidator>

                        </div>
                    </div>

                </div>
                <div class="row p-2">
                    <div class="col-sm-9 col-md-9 col-lg-9 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label1">FeedBack Comment</label>
                            <asp:TextBox ID="txtFeedBackComm" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off" TextMode="MultiLine" onkeypress="return LettersWithSpaceOnly(event);"
                                MaxLength="300" Font-Size="14px" TabIndex="5">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFeedBackComm"
                                ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter FeedBack Comments</asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>
                <div class="row p-2">
                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label8">Action Details</label><br />
                            <asp:TextBox ID="txtActionDetails" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off" TextMode="MultiLine"
                                MaxLength="300" Font-Size="14px" TabIndex="6">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtActionDetails"
                                ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Action Details</asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label7">Home Image Display</label><br />
                            <asp:RadioButtonList ID="rblImagedisplay" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="rbl" TabIndex="7">
                                <asp:ListItem Text="Yes" Value="Y" />
                                <asp:ListItem Text="No" Value="N" Selected="True" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12 col-lg-12 col-sm-12 text-right">
                <div class="form-submit">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="OfferMaster" TabIndex="8" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn  btn-danger" CausesValidation="false" TabIndex="9" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
        <div id="divgrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvFeedBack" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="FeedbackId" PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="FeedbackId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFeedbackId" runat="server" Text='<%# Bind("FeedbackId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Given By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblGivenBy" runat="server" Text='<%# Bind("GivenBy") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MobileNo" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MobileNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mail Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblMailId" runat="server" Text='<%# Bind("MailId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Address") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Feedback" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblFeedbackDet" runat="server" Text='<%# Bind("FeedbackDet") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action Details" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblActionDetails" runat="server" Text='<%# Bind("ActionDetails") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HomePageDisplay" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblHomePageDisplay" runat="server" Text='<%# Bind("HomePageDisplay") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgBtnDelete" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                runat="server" Font-Bold="true" ImageUrl="~/images/Delete.png" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this entry?');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>




    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfDateTime" runat="server" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

