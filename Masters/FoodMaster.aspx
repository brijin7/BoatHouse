<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Masters/FoodMaster.aspx.cs" Inherits="FoodMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgfoodPhotoPrev.ClientID%>').prop('src', e.target.result);
                    $('#<%=hfImageCheckValue.ClientID%>').val("1");
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        $(function () {
            var fileupload = $('#<%=fupfoodLink.ClientID%>');
            var image = $('#divImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });

    </script>
    <div class="form-body col-sm-9 col-xs-12">
        <h5 class="pghr">Food Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                        <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12" style="display: none">
                        <div class="form-group">
                            <label for="lblfoodid">Food  </label>
                            <asp:TextBox ID="txtFoodId" runat="server" CssClass="form-control" AutoComplete="Off">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class=" col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label for="lblFoodName">Food Name</label>
                            <asp:TextBox ID="txtFoodName" TabIndex="1" runat="server" CssClass="form-control" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFoodName" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Invalid" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFoodName"
                                ValidationGroup="Food" SetFocusOnError="True" CssClass="vError">Enter Food Name</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label for="lblvegnonveg">Food Type </label>
                            <asp:DropDownList ID="ddlfood" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="2">
                                <asp:ListItem Value="0">Select Food Type </asp:ListItem>
                                <asp:ListItem Value="V">Veg </asp:ListItem>
                                <asp:ListItem Value="N">Non Veg </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlfood"
                                ValidationGroup="Food" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Food Type</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                        <div class="form-group">
                            <label for="lbldistrictid">Food City</label>
                            <asp:DropDownList ID="ddlfoodcity" runat="server" CssClass="form-control inputboxstyle" AutoPostBack="true"
                                MaxLength="50" TabIndex="3">
                                <asp:ListItem Value="0">Select Food City </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlfoodcity"
                                ValidationGroup="Food" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Food City</asp:RequiredFieldValidator>
                        </div>
                    </div>


                </div>

                <div class="row p-2">
                    <div class="col-md-9 col-lg-9 col-sm-9">
                        <div class="form-group">
                            <label for="lbldes">Food Description</label>
                            <asp:TextBox ID="txtfoodDes" runat="server" CssClass="form-control" TextMode="MultiLine" AutoComplete="Off" TabIndex="4">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtfoodDes"
                                ValidationGroup="Food" SetFocusOnError="True" CssClass="vError">Enter Food Description</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3 col-lg-3 col-sm-12">
                        <div class="form-group">
                            <label runat="server" id="lblfoodimg">Food Image Link</label>
                            <br />
                            <span id="spFilePath"></span>
                            <asp:FileUpload ID="fupfoodLink" runat="server" Style="display: none" onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image ID="imgfoodPhotoPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                <div class="imageOverlay">Click To Upload</div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Food" TabIndex="6" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="7" OnClick="btnCancel_Click" />
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
                <asp:GridView ID="gvFoodMaster" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="FoodId" PageSize="25000" OnRowDataBound="gvFoodMaster_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Food Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFoodId" runat="server" Text='<%# Bind("FoodId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Food Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblFoodName" runat="server" Text='<%# Bind("FoodName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Food Description" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblFoodDescription" runat="server" Text='<%# Bind("FoodDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Food Image Link" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblFoodImageLink" runat="server" Visible="false" Text='<%# Bind("FoodImageLink") %>'></asp:Label>
                                <asp:Image ID="lblFoodImageLinkpre" runat="server" ImageUrl='<%# Eval("FoodImageLink") %>' Height="30px" Width="30px" />

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="VegNonVeg" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFoodCity" runat="server" Text='<%# Bind("FoodCity") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Food Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblFoodType" runat="server" Text='<%# Bind("FoodType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Food City " HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCityName" runat="server" Text='<%# Bind("CityName") %>'></asp:Label>
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

