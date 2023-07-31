<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" Async="true" CodeFile="~/Masters/Enquiry.aspx.cs" Inherits="Enquiry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-10 col-xs-12">
        <h5 class="pghr">Enquiry <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">

                <div class="row p-2">
                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12" style="display: none">
                        <div class="form-group">
                            <label for="ddlEnquiry">Enquiry Id</label>
                            <asp:TextBox ID="txtEnquiryid" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="10" Font-Size="14px" TabIndex="4">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class=" col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label for="ddlEnquiryType">Enquiry Type</label>
                            <asp:DropDownList ID="ddlEnquiryType" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="1">
                                <asp:ListItem Value="0">Select Enquiry type </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlEnquiryType"
                                ValidationGroup="Enquiry" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Enquiry</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label>Enquired By</label>
                            <asp:TextBox ID="txtEnquiryBy" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                MaxLength="10" Font-Size="14px" TabIndex="4">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEnquiryBy" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Invalid" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEnquiryBy"
                                ValidationGroup="Enquiry" SetFocusOnError="True" CssClass="vError">Enter Enquiry By</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label>Mobile No</label>
                            <asp:TextBox ID="txtMobno" runat="server" CssClass="form-control inputboxstyle" onkeypress="return isNumber(event);" AutoComplete="Off"
                                MaxLength="10" Font-Size="14px" TabIndex="4">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ControlToValidate="txtMobno" ErrorMessage="Invalid"
                                ValidationExpression="[0-9]{10}" ForeColor="Red"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMobno"
                                ValidationGroup="Enquiry" SetFocusOnError="True" CssClass="vError">Enter Mobile No</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label>Email Id</label>
                            <asp:TextBox ID="txtEmailid" runat="server" CssClass="form-control inputboxstyle"
                                AutoComplete="Off" MaxLength="50" Font-Size="14px" TabIndex="4">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmailid"
                                SetFocusOnError="True" CssClass="vError" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                Display="Dynamic" ErrorMessage="Invalid" ValidationGroup="Enquiry" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmailid"
                                ValidationGroup="Enquiry" SetFocusOnError="True" CssClass="vError">Enter Email Id</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-md-9 col-lg-9 col-sm-12">
                        <div class="form-group">
                            <label runat="server" id="Label2">Query Details</label>
                            <asp:TextBox ID="txtQuerydetails" runat="server" CssClass="form-control inputboxstyle"
                                AutoComplete="Off" MaxLength="300" TextMode="MultiLine" Font-Size="14px" TabIndex="5" Rows="2">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtQuerydetails"
                                ValidationGroup="Enquiry" SetFocusOnError="True" CssClass="vError">Enter Query Details</asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>

                <div class="row p-2">
                    <div class="col-md-9 col-lg-9 col-sm-12">
                        <div class="form-group">
                            <label runat="server" id="lbltxtCityDes">Response Details</label>
                            <asp:TextBox ID="txtresponseDetails" runat="server" CssClass="form-control inputboxstyle"
                                AutoComplete="Off" MaxLength="300" TextMode="MultiLine" Font-Size="14px" TabIndex="5" Rows="2">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtresponseDetails"
                                ValidationGroup="Enquiry" SetFocusOnError="True" CssClass="vError">Enter Response Details</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>


                <div class="row p-2">
                    <div class="col-md-9 col-lg-9 col-sm-12">
                        <div class="form-group">
                            <label runat="server" id="Label1">Address</label>
                            <asp:TextBox ID="txtAddr" runat="server" CssClass="form-control inputboxstyle"
                                AutoComplete="auto" MaxLength="300" TextMode="MultiLine" Font-Size="14px" TabIndex="5" Rows="2">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtAddr"
                                ValidationGroup="Enquiry" SetFocusOnError="True" CssClass="vError">Enter Address</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>


                <div class="col-md-12 col-lg-12 col-sm-12 text-right">
                    <div class="form-submit">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Enquiry" TabIndex="6" OnClick="btnSubmit_Click" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="7" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvEnquiry" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="EnquiryId" PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EnquiryId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEnquiryId" runat="server" Text='<%# Bind("EnquiryId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EnquiryType" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEnquiryType" runat="server" Text='<%# Bind("EnquiryType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enquiry Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEnquiryTypeName" runat="server" Text='<%# Bind("EnquiryTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Address" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Address") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Enquired By" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEnquiredBy" runat="server" Text='<%# Bind("EnquiredBy") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Mobile No" HeaderStyle-CssClass="grdHead">
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

                        <asp:TemplateField HeaderText="Query Details" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblQueryDetails" runat="server" Text='<%# Bind("QueryDetails") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Response Details" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblResponseDetails" runat="server" Text='<%# Bind("ResponseDetails") %>'></asp:Label>
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



    <asp:HiddenField runat="server" ID="hfCreatedBy" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

