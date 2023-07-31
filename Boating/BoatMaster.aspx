<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Boating/BoatMaster.aspx.cs" Inherits="BoatMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script>
        $(document).ready(function () {
            $(".decimal").keypress(function (event) {
                return isNumber(event, this);
            });
        });
        function isNumber(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (
                (charCode != 45 || $(element).val().indexOf('-') != -1) &&
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&
                (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function minmax(value, min, max) {
            if (parseInt(value) < min || isNaN(parseInt(value)))
                return min;
            else if (parseInt(value) > max)
                return max;
            else return value;
        }

    </script>

    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Boat Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label for="lblnum">
                                <i class="fa fa-ship" aria-hidden="true"></i>
                                Boat Id
                            </label>
                            <asp:TextBox ID="txtBoatId" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="1" MaxLength="10">
                            </asp:TextBox>

                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatNature"><i class="fa fa-user"></i>Boat Nature <span class="spStar">*</span></label>
                            <asp:RadioButtonList ID="rblboatnature" runat="server" RepeatDirection="Horizontal" TabIndex="7" CssClass="rbl">
                                <asp:ListItem Value="N" Selected="true">Normal</asp:ListItem>
                                <asp:ListItem Value="P">Express</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="rblboatnature"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat Nature</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label for="lblnum">
                                <i class="fa fa-ship" aria-hidden="true"></i>
                                Boat House
                            </label>
                            <asp:DropDownList ID="ddlBoatHouse" CssClass="form-control" runat="server"
                                AutoPostBack="true" TabIndex="2">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlBoatHouse"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fa fa-ship" aria-hidden="true"></i>
                                Boat Type <span class="spStar">*</span>
                            </label>
                            <asp:DropDownList ID="ddlBoatType" CssClass="form-control" runat="server" AutoPostBack="true" TabIndex="3" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlBoatType"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="Select Boat Type" CssClass="vError">Select Boat Type</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblboatid">
                                <i class="fa fa-ship" aria-hidden="true"></i>
                                Boat Seater <span class="spStar">*</span>
                            </label>
                            <asp:DropDownList ID="ddlBoatSeater" CssClass="form-control" runat="server" TabIndex="4">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBoatSeater"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="Select Boat Seater" CssClass="vError">Select Boat Seater</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblnum">
                                <i class="fa fa-ship" aria-hidden="true"></i>
                                Boat Number <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtBoatNum" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="5" MaxLength="10">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtBoatNum" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Not a Valid" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtBoatNum"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" CssClass="vError">Enter Boat Number</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblname">
                                <i class="fas fa-ship"></i>
                                Boat Name <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtBoatName" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="6" MaxLength="25">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtBoatName" ForeColor="Red"
                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Not Valid!" CssClass="vError" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtBoatName"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" CssClass="vError">Enter Boat Name</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblBoatOwner" id="lblBoatOwner"><i class="fa fa-user"></i>Boat Owner <span class="spStar">*</span></label>
                            <asp:RadioButtonList ID="rblBoatOwner" runat="server"
                                AutoPostBack="true" RepeatDirection="Horizontal" TabIndex="7" CssClass="rbl" OnSelectedIndexChanged="rblBoatOwner_SelectedIndexChanged">
                                <asp:ListItem Value="T" Selected="true">Own</asp:ListItem>
                                <asp:ListItem Value="P">Private</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="rblBoatOwner"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat Owner</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row p-2" id="divPrivate" runat="server">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblpaymodel">
                                <i class="fa fa-credit-card" aria-hidden="true"></i>
                                Payment Mode <span class="spStar">*</span>
                            </label>
                            <asp:DropDownList ID="ddlPayModel" CssClass="form-control" runat="server" TabIndex="8">
                                <asp:ListItem Value="0">Select Payment Mode</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPayModel"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Payment Mode</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblpaypercent">
                                <i class="fas fa-money-bill"></i>
                                Payment Percent(<i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtPayPercent" runat="server" CssClass="form-control decimal" AutoComplete="Off" MaxLength="12" TabIndex="9"
                                onkeyup="this.value = minmax(this.value, 0, 100)">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblpayamount">
                                <i class="fas fa-money-check-alt"></i>
                                Payment Amount(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtPayAmt" runat="server" CssClass="form-control decimal" AutoComplete="Off" TabIndex="10" MaxLength="5">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblboatstatus">
                                <i class="fas fa-toggle-on"></i>
                                Boat Status <span class="spStar">*</span>
                            </label>
                            <asp:DropDownList ID="ddlBoatStatus" CssClass="form-control" runat="server" TabIndex="11">
                                <asp:ListItem Value="0">Select Boat Status</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlBoatStatus"
                                ValidationGroup="BoatMaster" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat Status</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BoatMaster" TabIndex="11" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="12" OnClick="btnCancel_Click" />
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
                <div class="col-sm-6 col-xs-12" style="max-height: 300px; min-height: 50px; overflow-y: auto; float: left; position: relative;">
                    <asp:GridView ID="gvCount" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                        AutoGenerateColumns="False" DataKeyNames="BoatTypeId" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Type " HeaderStyle-CssClass="grdHead" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoatTypeId" runat="server" Visible="false" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                    <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatTypeName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoatSeaterId" runat="server" Visible="false" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                    <asp:Label ID="lblBoatSeaterName" runat="server" Text='<%# Bind("BoatSeaterName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblNormal" runat="server" Text='<%# Bind("Normal") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Express" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpress" runat="server" Text='<%# Bind("Express") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="grdHead">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gvHead" />
                        <AlternatingRowStyle CssClass="gvRow" />
                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                        <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                    </asp:GridView>
                </div>

                <asp:GridView ID="gvBoatMaster" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatId" PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatId" runat="server" Text='<%# Bind("BoatId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatHouseId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat House Name" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Seater" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatSeaterName" runat="server" Text='<%# Bind("BoatSeaterName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Nature" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatNature" runat="server" Text='<%# Bind("BoatNature") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Number" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatNum" runat="server" Text='<%# Bind("BoatNum") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatName" runat="server" Text='<%# Bind("BoatName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatTypeId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatTypeId" runat="server" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatStatus" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatStatus" runat="server" Text='<%# Bind("BoatStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Owner" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatOwner" runat="server" Text='<%# Bind("BoatOwner") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Status" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatStatusName" runat="server" Text='<%# Bind("BoatStatusName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatSeaterId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatSeaterId" runat="server" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PaymentModel" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentModel" runat="server" Text='<%# Bind("PaymentModel") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Payment Model" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentModelName" runat="server" Text='<%# Bind("PaymentModelName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PayTypeName" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPayTypeName" runat="server" Text='<%# Bind("PayTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PaymentPercent" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentPercent" runat="server" Text='<%# Bind("PaymentPercent") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PaymentAmount" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentAmount" runat="server" Text='<%# Bind("PaymentAmount") %>'></asp:Label>
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
                        <asp:TemplateField HeaderText="QR Code" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Button ID="GenerateQRCode" runat="server" CssClass="btn btn-primary" Text="Generate" OnClick="GenerateQRCode_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfBoat" runat="server" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

