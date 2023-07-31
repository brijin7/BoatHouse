<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    CodeFile="~/Masters/OfferMaster.aspx.cs" Inherits="OfferMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .panel-group .panel + .panel {
            margin-top: 5px;
        }

        .panel-group .panel {
            margin-bottom: 0;
            border-radius: 4px;
        }

        .panel-success {
            border: 1px;
            border-color: #d6e9c6;
        }

        .panel {
            margin-bottom: 20px;
            background-color: #fff;
            border: 1px solid transparent;
            border-radius: 4px;
            -webkit-box-shadow: 0 1px 1px rgba(0,0,0,.05);
            box-shadow: 0 1px 1px rgba(0,0,0,.05);
        }

        .panel-success > .panel-heading {
            color: #3c763d;
            background-color: #dff0d8;
            border-color: #d6e9c6;
            font-size: 14px;
            font-weight: 800;
            text-align: left;
        }


        .panel-group .panel-heading {
            border-bottom: 0;
        }

        .panel-heading {
            padding: 5px 5px;
            border-bottom: 1px solid transparent;
            border-top-left-radius: 3px;
            border-top-right-radius: 3px;
        }

        .panel-body {
            padding: 10px;
            border: 1px solid #d6e9c6;
        }

        .lblChrg {
            padding: 10px 10px;
        }

        .chkChoice input {
            margin-right: 5px;
        }
    </style>
    <script>
        $(document).ready(function () {


            $(".decimal").keypress(function (event) {
                return isNumber(event, this);
            });
        });

        function isNumber(evt, element) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (
                (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // Check minus and only once.
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // Check for dots and only once.
                (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        $(document).ready(function () {
            $('#<%=txtOffer.ClientID%>').change(function () {
                var a = $('#<%=txtOffer.ClientID%>').val();
                if (a > 100) {
                    alert('Enter Below 100 Percentage');
                    $('#<%=txtOffer.ClientID%>').val('');
                    $('#<%=txtOffer.ClientID%>').focus();
                }
                if (a < 0) {
                    alert('Should Not Accept Negative value ');
                    $('#<%=txtOffer.ClientID%>').val('');
                    $('#<%=txtOffer.ClientID%>').focus();
                }
            });
        })
        $(function checkUncheck() {
            $("[id*=chkSelectAll]").bind("click", function () {
                if ($(this).is(":checked")) {
                    $("[id*=chkBoatHouse] input").prop("checked", true);
                }
                else {
                    $("[id*=chkBoatHouse] input").prop("checked", false);
                }
            });

            $("[id*=chkBoatHouse] input").bind("click", function () {
                if ($("[id*=chkBoatHouse] input:checked").length == $("[id*=chkBoatHouse] input").length) {
                    $("[id*=chkSelectAll]").prop("checked", true);
                }
                else {

                    $("[id*=chkSelectAll]").prop("checked", false);
                }


            });
        });
    </script>


    <div class="form-body">
        <h5 class="pghr">Offer/Discount Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <br />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-8 col-xs-12">
                        <div class="row">
                            <div runat="server" visible="false">
                                <div class="form-group">
                                    <label runat="server" id="Label9"><i class="fas fa-list"></i>Offer Id <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtOfferId" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off" onkeypress="return LettersWithSpaceOnly(event);"
                                        MaxLength="50" Font-Size="14px">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label4"><i class="fa fa-address-book" aria-hidden="true"></i>Offer Type <span class="spStar">*</span></label>
                                    <asp:RadioButtonList ID="rbtnOfferType" runat="server" Style="margin-top: -9px;" TabIndex="1" RepeatDirection="Horizontal" CssClass="rbl">
                                        <asp:ListItem Value="O" Selected="true">Offer</asp:ListItem>
                                        <asp:ListItem Value="D">Discount</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="rbtnOfferType"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Offer Type</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label10"><i class="fa fa-address-book" aria-hidden="true"></i>Offer Category<span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlOfferCat" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                                        OnSelectedIndexChanged="ddlOfferCat_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlOfferCat" InitialValue="Select Offer Category"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Select Offer Category</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label2"><i class="fas fa-list"></i>Offer Name <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtOfferName" runat="server" CssClass="form-control inputboxstyle" AutoComplete="off" onkeypress="return LettersWithSpaceOnly(event);"
                                        MaxLength="50" Font-Size="14px" TabIndex="3">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOfferName"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Offer Name </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label3"><i class="fas fa-money-bill-wave-alt"></i>Amount Type <span class="spStar">*</span></label>
                                    <asp:RadioButtonList ID="rbtnAmountType" runat="server" TabIndex="4" Style="margin-top: -9px;" RepeatDirection="Horizontal" CssClass="rbl">
                                        <asp:ListItem Value="P" Selected="true">Percentage</asp:ListItem>
                                        <asp:ListItem Value="F">Fixed</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rbtnAmountType"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Amount Type</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label5"><i class="fa fa-gift" aria-hidden="true"></i>Offer <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtOffer" runat="server" CssClass="form-control decimal" AutoComplete="off" onkeypress="return isNumber(event);"
                                        MaxLength="5" Font-Size="14px" TabIndex="5">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtOffer"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Enter Offer </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12" id="divMinBillAmt" runat="server" visible="false">
                                <div class="form-group">
                                    <label runat="server" id="Label1"><i class="fas fa-money-check-alt"></i>Minimum Bill Amount <span class="spStar"></span></label>
                                    <asp:TextBox ID="txtMinBillAmt" runat="server" CssClass="form-control decimal" AutoComplete="off" onkeypress="return isNumber(event);"
                                        MaxLength="6" Font-Size="14px" TabIndex="6">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12" id="divMinNoTickets" runat="server" visible="false">
                                <div class="form-group">
                                    <label runat="server" id="Label6"><i class="fas fa-ticket-alt"></i>Minimum No of Tickets <span class="spStar"></span></label>
                                    <asp:TextBox ID="txtMiniTickets" runat="server" CssClass="form-control" AutoComplete="off" onkeypress="return isNumber(event);"
                                        MaxLength="6" Font-Size="14px" TabIndex="7">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label for="lbleffectivefrom" id="Label7" runat="server">
                                        <i class="fa fa-calendar" aria-hidden="true"></i>Effective From <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtEffectivefrm" runat="server" AutoComplete="off" CssClass="form-control OfferfrmDate"
                                        MaxLength="50" Font-Size="14px" TabIndex="8">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffectivefrm"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Select Effective From</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label for="lbleffectivetill" id="Label8" runat="server">
                                        <i class="fa fa-calendar" aria-hidden="true"></i>Effective Till <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtEffectiveTill" runat="server" AutoComplete="off" CssClass="form-control OffertoDate"
                                        MaxLength="50" Font-Size="14px" TabIndex="9">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtEffectiveTill"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Select Effective Till</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label runat="server" id="Label11"><i class="fa fa-address-book" aria-hidden="true"></i>Corporate Office<span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlCorpId" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                                        OnSelectedIndexChanged="ddlCorpId_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCorpId" InitialValue="Select Corporate Office"
                                        ValidationGroup="OfferMaster" SetFocusOnError="True" CssClass="vError">Select Corporate Office</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="col-sm-12" id="divBoatHouseAll" runat="server" visible="false">
                            <div class="panel panel-success">
                                <div class="panel-heading panelheadchk">
                                    Boat House <span class="spStar">*</span>
                                    <asp:CheckBox ID="chkSelectAll" Text="Select All" runat="server" Font-Bold="True"
                                        CssClass="chkbox checkbox-primary" TabIndex="10" />
                                </div>
                                <div class="panel-body">
                                    <asp:CheckBoxList ID="chkBoatHouse" runat="server" RepeatDirection="Horizontal" RepeatColumns="1"
                                        CssClass="chkChoice" CellPadding="5" CellSpacing="5" Width="100%" TabIndex="10">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="form-submit">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="OfferMaster" AutoPostBack="True" TabIndex="11" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="12" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>

        <div id="divgrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="GvOfferMaster" runat="server" AllowPaging="True"
                    CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="OfferId" PageSize="25000" OnRowDataBound="GvOfferMaster_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OfferId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOfferId" runat="server" Text='<%# Bind("OfferId") %>'></asp:Label>
                                <asp:Label ID="lblCorpId" runat="server" Text='<%# Bind("CorpId") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Offer Type" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOfferType" runat="server" Text='<%# Bind("OfferType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Offer Category" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOfferCategoryName" runat="server" Text='<%# Bind("OfferCategoryName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Offer Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOfferName" runat="server" Text='<%# Bind("OfferName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat House Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOfferCategory" runat="server" Text='<%# Bind("OfferCategory") %>'></asp:Label>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Boat House" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount Type" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountType" runat="server" Text='<%# Bind("AmountType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Offer" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOffer" runat="server" Text='<%# Bind("Offer") %>'></asp:Label>
                                <asp:Label ID="lblAmountTypeName" runat="server" Text='<%# Eval("AmountType").ToString() == "P" ? "(%)" : "(₹)" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Minimum Bill Amount" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMinBillAmount" runat="server" Text='<%# Bind("MinBillAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Minimum No Of Tickets" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMinNoOfTickets" runat="server" Text='<%# Bind("MinNoOfTickets") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Effective From" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveFrom" runat="server" Text='<%# Bind("EffectiveFrom") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Effective Till" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveTill" runat="server" Text='<%# Bind("EffectiveTill") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
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
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');" />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfBoatHouseId" runat="server" />
    <asp:HiddenField ID="hfBoatHouseName" runat="server" />


    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

