<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" CodeFile="~/Boating/CancellationRule.aspx.cs" Inherits="CancellationRule" %>

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

    </script>

    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Cancellation Re-Scheduling Rule <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                        <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label for="lbltype"><i class="fas fa-trash"></i>Activity Id</label>
                            <asp:TextBox ID="txtActivityId" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label for="lbltype"><i class="fas fa-trash"></i>Activity Type <span class="spStar">*</span></label>
                            <asp:RadioButtonList ID="rblActivityType" runat="server" CssClass="rbl" Style="margin-top: -9px;" RepeatDirection="Horizontal" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="rblActivityType_SelectedIndexChanged">
                                <asp:ListItem Value="C" Selected="true">Cancellation</asp:ListItem>
                                <asp:ListItem Value="R">Re-scheduling</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="form-group">
                            <label for="lbldescription" runat="server">
                                <i class="fas fa-info"></i>Description <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="100" TabIndex="2">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtDescription"
                                ValidationGroup="CancellationAmount" SetFocusOnError="True" CssClass="vError">Enter Description</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label for="lbltype"><i class="fas fa-trash"></i>Charge Type <span class="spStar">*</span></label>
                            <asp:RadioButtonList ID="rblChargeType" runat="server" Style="margin-top: -9px;" RepeatDirection="Horizontal"
                                TabIndex="3" CssClass="rbl" OnSelectedIndexChanged="rblChargeType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="P">Percentage</asp:ListItem>
                                <asp:ListItem Value="F" Selected="true">Fixed</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblamount">
                                <i class="fas fa-money-check-alt"></i>
                                <asp:Label ID="lblCharges" runat="server">Charges</asp:Label>
                                <asp:Label ID="lblChargePer" runat="server" Visible="false">(<i class="fas fa-percentage p-0"></i>)</asp:Label>
                                <asp:Label ID="lblChargeFix" runat="server" Visible="false">(<i class="fas fa-rupee-sign p-0"></i>)</asp:Label>
                                <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtCharges" runat="server" CssClass="form-control decimal" AutoComplete="Off" TabIndex="4" MaxLength="5">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCharges"
                                ValidationGroup="CancellationAmount" SetFocusOnError="True" CssClass="vError">Enter Charges</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lblamount">
                                <i class="fas fa-money-check-alt"></i>
                                Applicable Before (Hrs) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtAppliBefore" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="5" MaxLength="5" onkeypress="return isNumber(event)">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAppliBefore"
                                ValidationGroup="CancellationAmount" SetFocusOnError="True" CssClass="vError">Enter Applicable Before (Hrs)</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lbleffectivefrom" id="lbleffectivefrom" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>Effective From <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="form-control frmDate" AutoComplete="Off" TabIndex="6">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEffectiveFrom"
                                ValidationGroup="CancellationAmount" SetFocusOnError="True" CssClass="vError">Enter Effective From</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lbleffectivetill" id="Label1" runat="server">
                                <i class="fa fa-calendar" aria-hidden="true"></i>Effective Till <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtEffectiveTill" runat="server" CssClass="form-control toDate1" AutoComplete="Off" TabIndex="7">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffectiveTill"
                                ValidationGroup="CancellationAmount" SetFocusOnError="True" CssClass="vError">Enter Effective Till</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" runat="server" id="divMaxSchedule" visible="false">
                        <div class="form-group">
                            <label for="lblamount">
                                <i class="fa fa-retweet" aria-hidden="true"></i>
                                Max No of Re-Schedule <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtMaxNoOfSch" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="8" MaxLength="5" onkeypress="return isNumber(event)">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtMaxNoOfSch"
                                ValidationGroup="CancellationAmount" SetFocusOnError="True" CssClass="vError">Enter Max No of Schedule</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="CancellationAmount" TabIndex="9" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="10" OnClick="btnCancel_Click" />
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

                <asp:GridView ID="gvCancellationAmount" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="ActivityId" PageSize="25000" OnRowDataBound="gvCancellationAmount_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ActivityId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActivityId" runat="server" Text='<%# Bind("ActivityId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatHouse Id " HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BoatHouse Name" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Activity Type" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActivityType" runat="server" Text='<%# Bind("ActivityType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Charge Type" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblChargeType" runat="server" Text='<%# Bind("ChargeType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Charges" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCharges" runat="server" Text='<%# Bind("Charges") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Applicable Before (Hrs)" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblApplicableBefore" runat="server" Text='<%# Bind("ApplicableBefore") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Effective From" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveFrom" runat="server" Text='<%# Bind("EffectiveFrom") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Effective Till" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveTill" runat="server" Text='<%# Bind("EffectiveTill") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Max No Of Resched" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblMaxNoOfResched" runat="server" Text='<%# Bind("MaxNoOfResched") %>'></asp:Label>
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
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

