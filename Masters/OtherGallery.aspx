<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Masters/OtherGallery.aspx.cs" Inherits="OtherGallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <%--<script>
        function ShowPreviewVideo(input) {

            if (input.files && input.files[0]) {
                var ImageDir = new FileReader();
                ImageDir.onload = function (e) {
                    $('#prvideo').attr('src', e.target.result);
                }
                ImageDir.readAsDataURL(input.files[0]);
            }
        }
        //function ShowPreview(input) {

        //    if (input.files && input.files[0]) {
        //        var ImageDir = new FileReader();
        //        ImageDir.onload = function (e) {
        //            $('#imgprvImage').attr('src', e.target.result);
        //        }
        //        ImageDir.readAsDataURL(input.files[0]);
        //    }
        //}
    </script>--%>
    <script type="text/javascript">
        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgothimagePrev.ClientID%>').prop('src', e.target.result);
                    $('#<%=hfImageCheckValue.ClientID%>').val("1");
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        $(function () {
            var fileupload = $('#<%=fupothimageLink.ClientID%>');
            var image = $('#divImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });

    </script>

    <script type="text/javascript">
        function ShowPreviewVideo(input) {

            if (input.files && input.files[0]) {
                var ImageDir = new FileReader();
                ImageDir.onload = function (e) {
                    $('#prvideo').attr('src', e.target.result);
                }
                ImageDir.readAsDataURL(input.files[0]);
            }
        }
        $(function () {
            var fileupload = $('#<%=fuVideolink.ClientID%>');
            var image = $('#divvideoPreview');
            image.click(function () {
                fileupload.click();
            });
        });

    </script>

    <div class="form-body col-sm-9 col-xs-12">
        <h5 class="pghr">Other Gallery <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                 <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label for="ddlGallery">Gallery</label>
                            <asp:TextBox ID="txtGalleryId" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="3" MaxLength="10">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label for="lbldistrictid">Type</label>
                            <asp:RadioButtonList ID="rblGalleryType" runat="server" TabIndex="1" CssClass="rbl" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblGalleryType_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="Photo" Value="P" />
                                <asp:ListItem Text="Video" Value="V" />
                            </asp:RadioButtonList>

                        </div>
                    </div>

                </div>

                <div class="row" runat="server" id="divImage" visible="false">
                    <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label3">Image Video Link</label>
                            <%--<asp:FileUpload ID="fuImgvideoLink" runat="server" TabIndex="2" onchange="ShowPreview(this)" />--%>
                            <br />
                            <span id="spFilePath"></span>
                            <asp:FileUpload ID="fupothimageLink" runat="server" Style="display: none" onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image ID="imgothimagePrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                <div class="imageOverlay">Click To Upload</div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row" runat="server" id="divVideo" visible="false">
                    <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label1">Image Video Link</label>
                            <br />
                            <span id="spFilePath1"></span>
                            <asp:FileUpload ID="fuVideolink" runat="server" Style="display: none" onchange="ShowPreviewVideo(this)" />
                            <div class="divImg" id="divvideoPreview">
                                <asp:Image ID="imgothvideoPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                <%-- <video  style="max-width: 200px; max-height: 200px; padding-top: 5px; padding-bottom: 5px" id="prvideo" controls autoplay></video>
                                --%>
                                <div class="imageOverlay">Click To Upload</div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="form-submit">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Gallery" TabIndex="4" OnClick="btnSubmit_Click" CausesValidation="true" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="5" OnClick="btnCancel_Click" />

                </div>
            </div>
        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvOtherGallery" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="GalleryId" PageSize="25000" OnRowDataBound="gvOtherGallery_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gallery Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblGalleryId" runat="server" Text='<%# Bind("GalleryId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image/Video" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblImgVideo" runat="server" Text='<%# Bind("ImageVideoLink") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ActiveStatus" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
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

    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

