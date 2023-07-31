<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" Async="true" AutoEventWireup="true" CodeFile="~/Masters/CarouselMaster.aspx.cs" Inherits="CarouselMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgEmpPhotoPrev.ClientID%>').prop('src', e.target.result);
                    $('#<%=hfImageCheckValue.ClientID%>').val("1");
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        $(function () {
            var fileupload = $('#<%=fupEmpLink.ClientID%>');
            var image = $('#divImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });

    </script>

    <div class="form-body col-sm-6 col-xs-12">
        <h5 class="pghr">Carousel Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-4 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label runat="server">CarouselId</label>

                            <asp:TextBox ID="txtCarouselId" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="150" Font-Size="14px" TabIndex="1">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label for="lblcity">Corporation</label>
                            <asp:DropDownList ID="ddlCorporation" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCorporation"
                                ValidationGroup="Carousel" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Corporation</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label runat="server">App Name</label>

                            <asp:TextBox ID="txtAppName" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="150" Font-Size="14px" TabIndex="2" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAppName" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Invalid" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAppName"
                                ValidationGroup="Carousel" SetFocusOnError="True" CssClass="vError">Enter App Name</asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>
                <div class="row p-2">
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label3">Carousel Image</label>
                            <br />
                            <span id="spFilePath"></span>
                            <asp:FileUpload ID="fupEmpLink" runat="server" Style="display: none" onchange="ShowImagePreview(this);" TabIndex="3" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image ID="imgEmpPhotoPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" TabIndex="5" />
                                <div class="imageOverlay">Click To Upload</div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Carousel" TabIndex="6" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="7" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvcarousel" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="CarouselId" PageSize="25000" OnRowDataBound="gvcarousel_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CarouselId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCarouselId" runat="server" Text='<%# Bind("CarouselId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CorpId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCorpId" runat="server" Text='<%# Bind("CorpID") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corporation" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCorporation" runat="server" Text='<%# Bind("CorpName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="App Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("AppName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblimgLink12" runat="server" Visible="false" Text='<%# Bind("Carousel") %>'></asp:Label>
                                <asp:Image ID="lblCarouselImg" runat="server" ImageUrl='<%# Eval("Carousel") %>' Height="30px" Width="30px" />

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
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
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
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


    <asp:HiddenField runat="server" ID="hfCreatedBy" />
    <asp:HiddenField runat="server" ID="hfPrevImageLink" />
    <asp:HiddenField runat="server" ID="hfResponse" />
    <asp:HiddenField runat="server" ID="hfImageCheckValue" Value="0" />

    <asp:HiddenField ID="hfImageLink" runat="server" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

