<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Masters/OtherInfo.aspx.cs" Inherits="OtherInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgOtherPrev.ClientID%>').prop('src', e.target.result);
                    $('#<%=hfImageCheckValue.ClientID%>').val("1");
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        $(function () {
            var fileupload = $('#<%=fupotherLink.ClientID%>');
            var image = $('#divImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });

    </script>

    <div class="form-body col-sm-9 col-xs-12">
        <h5 class="pghr">Other Info <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" style="display: none">
                        <div class="form-group">
                            <label runat="server" id="Label2">Info </label>
                            <asp:TextBox ID="txtinfoid" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" Font-Size="14px" TabIndex="1">
                            </asp:TextBox>
                        </div>
                    </div>

                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label4">Info Name </label>
                            <asp:TextBox ID="txtInfoName" runat="server" CssClass="form-control inputboxstyle" autocomplete="off"
                                MaxLength="10" Font-Size="14px" TabIndex="2">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtInfoName" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Invalid" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtInfoName"
                                ValidationGroup="OtherInfo" SetFocusOnError="True" CssClass="vError">Enter Info Name</asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label7">Info Description</label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="200" Font-Size="14px" TabIndex="3" TextMode="MultiLine">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtDescription"
                                ValidationGroup="OtherInfo" SetFocusOnError="True" CssClass="vError">Enter Info Description</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label3">Info Image Link</label>
                            <span id="spFilePath"></span>
                            <asp:FileUpload ID="fupotherLink" runat="server" Style="display: none" onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image ID="imgOtherPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                <div class="imageOverlay">Click To Upload</div>
                            </div>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="fuLocationimage"
                                ValidationGroup="OtherInfo" SetFocusOnError="True" CssClass="vError">Select Info Image</asp:RequiredFieldValidator>
                            --%>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="OtherInfo" TabIndex="5" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="6" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divgrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvotherinfo" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="InfoId" PageSize="25000" OnRowDataBound="gvotherinfo_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Info" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblInfoId" runat="server" Text='<%# Bind("InfoId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InfoName" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblInfoName" runat="server" Text='<%# Bind("InfoName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InfoDescription" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblInfoDescription" runat="server" Text='<%# Bind("InfoDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="InfoImage" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblimgLink12" runat="server" Visible="false" Text='<%# Bind("InfoImageLink") %>'></asp:Label>
                                <asp:Image ID="InfoImageLink" runat="server" ImageUrl='<%# Eval("InfoImageLink") %>' Height="30px" Width="30px" />


                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ActiveStatus" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%--   <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgBtnDelete" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                runat="server" Font-Bold="true" ImageUrl="~/images/Delete.png" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this entry?');" />
                            <asp:ImageButton ID="ImgBtnUndo" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                runat="server" Font-Bold="true" ImageUrl="~/images/refresh.png" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure you want to Undo this entry?');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');"></asp:LinkButton>

                                <asp:LinkButton ID="ImgBtnUndo" runat="server" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>



    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField runat="server" ID="hfPrevImageLink" />
    <asp:HiddenField runat="server" ID="hfResponse" />
    <asp:HiddenField runat="server" ID="hfImageCheckValue" Value="0" />


    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

