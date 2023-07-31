<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" Async="true" AutoEventWireup="true" CodeFile="~/Masters/CulturalEvents.aspx.cs" Inherits="CulturalEvents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgCulturalPrev.ClientID%>').prop('src', e.target.result);
                    $('#<%=hfImageCheckValue.ClientID%>').val("1");
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        $(function () {
            var fileupload = $('#<%=fupCulturalLink.ClientID%>');
            var image = $('#divImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });

    </script>

    <div class="form-body col-sm-9 col-xs-12">
        <h5 class="pghr">Cultural Events <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
               <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12" runat="server" visible="false">
                        <div class="form-group">
                            <label runat="server" id="Label4">Event Id</label>
                            <asp:TextBox ID="txtEventId" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" Font-Size="14px" TabIndex="2">
                            </asp:TextBox>

                        </div>
                    </div>
                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label runat="server" id="Label1">Event Name</label>
                            <asp:TextBox ID="txtEventName" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="50" Font-Size="14px" TabIndex="1" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEventName" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Special Characters Not Allowed" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEventName"
                                ValidationGroup="Events" SetFocusOnError="True" CssClass="vError">Enter Event Name</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class=" col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label runat="server">Event Type</label>
                            <asp:DropDownList ID="ddlEventType" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="2">
                                <asp:ListItem Value="0">Select Event Type</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlEventType"
                                ValidationGroup="Events" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Event Type</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label runat="server">Event City</label>

                            <asp:DropDownList ID="ddlEventCity" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="3">
                                <asp:ListItem Value="0">Select Event City</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEventCity"
                                ValidationGroup="Events" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Event City</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-9 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label2">Event Description</label>
                            <asp:TextBox ID="txtEventDes" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="200" Font-Size="14px" TabIndex="4" TextMode="MultiLine" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEventDes"
                                ValidationGroup="Events" SetFocusOnError="True" CssClass="vError">Enter Event Description</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label3">Event Image Link</label>
                            <br />
                            <span id="spFilePath"></span>
                            <asp:FileUpload ID="fupCulturalLink" runat="server" Style="display: none" onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image ID="imgCulturalPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                <div class="imageOverlay">Click To Upload</div>

                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Events" TabIndex="6" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="7" OnClick="btnCancel_Click" />
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

                <asp:GridView ID="gvCulturalEvents" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="EventId" PageSize="25000" OnRowDataBound="gvCulturalEvents_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="EventId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEventId" runat="server" Text='<%# Bind("EventId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Event Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEventName" runat="server" Text='<%# Bind("EventName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EventTypeId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEventTypeId" runat="server" Text='<%# Bind("EventType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Event Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEventTypeName" runat="server" Text='<%# Bind("EventTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EventCityId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEventCityId" runat="server" Text='<%# Bind("EventCity") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Event City" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEventCityName" runat="server" Text='<%# Bind("CityName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Event Image" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEventImageLink" runat="server" Visible="false" Text='<%# Bind("EventImageLink") %>'></asp:Label>
                                <asp:Image ID="lblEventImageLinkImg" runat="server" ImageUrl='<%# Eval("EventImageLink") %>' Height="30px" Width="30px" />

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEventDescription" runat="server" Text='<%# Bind("EventDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
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

