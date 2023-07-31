<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Masters/EventGallery.aspx.cs" Inherits="EventGallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgeventPhotoPrev.ClientID%>').prop('src', e.target.result);
                    $('#<%=hfImageCheckValue.ClientID%>').val("1");
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        $(function () {
            var fileupload = $('#<%=fuEventimage.ClientID%>');
            var image = $('#divImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });

    </script>

    <div class="form-body col-sm-9 col-xs-12">
        <h5 class="pghr">Events Gallery <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                        <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12" runat="server" visible="false">
                        <div class="form-group">
                            <label runat="server">Gallery</label>
                            <asp:TextBox ID="txtGalleryId" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" Font-Size="14px" TabIndex="1">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label runat="server">Event</label>
                            <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="2">
                                <asp:ListItem Value="0">Select Event</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEvent"
                                ValidationGroup="Events" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Event</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label3">Gallery Image Link</label>

                            <br />
                            <span id="spFilePath"></span>
                            <asp:FileUpload ID="fuEventimage" runat="server" Style="display: none" onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image ID="imgeventPhotoPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                <div class="imageOverlay">Click To Upload</div>
                            </div>
                            <%--<asp:RequiredFieldValidator ID="lblOutput" runat="server" ControlToValidate="fuEventimage"
                                ValidationGroup="Events" SetFocusOnError="True" CssClass="vError">Select Gallery Image</asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Events" OnClick="btnSubmit_Click" TabIndex="4" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" OnClick="btnCancel_Click" TabIndex="5" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <div class="table-div" id="divgrid" runat="server">
        <div class="table-responsive">
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>

            <asp:GridView ID="gvEventsGallery" runat="server" AllowPaging="True"
                CssClass="gvv display table table-bordered table-condenced"
                AutoGenerateColumns="False" DataKeyNames="GalleryId" PageSize="25000" OnRowDataBound="gvEventsGallery_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead" HeaderStyle-Width="100">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GalleryId" HeaderStyle-CssClass="grdHead" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblGalleryId" runat="server" Text='<%# Bind("GalleryId") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EventId" HeaderStyle-CssClass="grdHead" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEventId" runat="server" Text='<%# Bind("EventId") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Events" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="lblEventName" runat="server" Text='<%# Bind("EventName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Image" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="lblEventImageLink" Visible="false" runat="server" Text='<%# Bind("EventImageLink") %>'></asp:Label>
                            <asp:Image ID="lbEventImgpre" runat="server" ImageUrl='<%# Eval("EventImageLink") %>' Height="30px" Width="30px" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ActiveStatus" HeaderStyle-CssClass="grdHead" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="100">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead" HeaderStyle-Width="100">
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

