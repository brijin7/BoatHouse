<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    CodeFile="~/Boating/WeekDaysTariffMaster.aspx.cs" Inherits="Department_Boating_WeekDaysTariffMaster" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="form-body" style="margin: 0 !important;">
        <h5 class="pghr">Week Days Tariff Master </h5>
        <hr />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-8">
                        <div style="padding: 12px 10px 0px 50px;">
                            <div class="row">
                                <div class="col-sm-2 col-xs-12" runat="server" visible="false">
                                    <div class="form-group">
                                        <label runat="server" id="Label3"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Id</label>
                                        <asp:TextBox ID="txtBoatId" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                            MaxLength="2" Font-Size="14px">
                                        </asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-sm-5 col-xs-12">
                                    <div class="form-group">
                                        <label runat="server" id="Label2"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Name <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txthousename" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                            MaxLength="300" Font-Size="14px" TabIndex="1">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txthousename" ForeColor="Red"
                                            ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="InValid!!" CssClass="vError" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txthousename"
                                            ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter House Name</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-5 col-xs-12">
                                    <div class="form-group">
                                        <label runat="server"><i class="fa fa-user" aria-hidden="true"></i>Boat House Manager <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtHouseManager" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="false" TabIndex="2">  
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div id="divWeekDays" runat="server" style="border-style: groove; border-radius: 5px 5px; padding: 10px 0px 0px 10px; background-color: #f7ffb2">

                                <h4 class="pghr" style="">WeekDays / WeekEnd Details </h4>
                                <hr />

                                <div id="divCheckWorkingdays" runat="server" style="padding: 10px 0px 0px 0px">
                                    <div class="row">
                                        <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                            <label runat="server" id="lblWeekDays">Week Days <span class="spStar">*</span></label>
                                            <%--   <asp:CheckBox ID="chkSelectAll" Text="Select All" runat="server" Font-Bold="True" CssClass="chkbox checkbox-primary" />--%>

                                            <div class="chkbox checkbox-primary">
                                                <asp:CheckBoxList ID="chkWorkingDays" runat="server" RepeatDirection="Horizontal" CssClass="styled" AutoPostBack="true"
                                                    OnSelectedIndexChanged="chkWorkingDays_SelectedIndexChanged" TabIndex="5">
                                                    <asp:ListItem Value="1">Sunday</asp:ListItem>
                                                    <asp:ListItem Value="2">Monday</asp:ListItem>
                                                    <asp:ListItem Value="3">Tuesday</asp:ListItem>
                                                    <asp:ListItem Value="4">Wednesday</asp:ListItem>
                                                    <asp:ListItem Value="5">Thursday</asp:ListItem>
                                                    <asp:ListItem Value="6">Friday</asp:ListItem>
                                                    <asp:ListItem Value="7">Saturday</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div id="divCheckWeekend" runat="server" visible="false">
                                    <div class="row">
                                        <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                            <label runat="server" id="lblWeekend">Week End <span class="spStar">*</span></label>
                                            <div class="chkbox checkbox-primary">
                                                <asp:CheckBoxList ID="chkWeekend" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="styled">
                                                    <asp:ListItem Value="1">Sunday</asp:ListItem>
                                                    <asp:ListItem Value="2">Monday</asp:ListItem>
                                                    <asp:ListItem Value="3">Tuesday</asp:ListItem>
                                                    <asp:ListItem Value="4">Wednesday</asp:ListItem>
                                                    <asp:ListItem Value="5">Thursday</asp:ListItem>
                                                    <asp:ListItem Value="6">Friday</asp:ListItem>
                                                    <asp:ListItem Value="7">Saturday</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <br />
                            <div id="divPublicholidays" runat="server" style="border-style: groove; border-radius: 5px 5px; padding: 10px 0px 0px 10px; background-color: #d2fbff"
                                visible="false">
                                <h4 class="pghr" style="">Public Holiday Details </h4>
                                <hr />
                                <div class="row">
                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12" id="divPHolidayDate" runat="server" style="padding-top: 10px">
                                        <label runat="server">Public Holiday Date (Week End Tariff)</label>
                                        <asp:TextBox ID="txtPubHldyDate" runat="server" CssClass="form-control DashfrmDate" AutoComplete="Off" TabIndex="6"
                                            onkeydown="return false;"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPubHldyDescp"
                                            ValidationGroup="Addbutton" SetFocusOnError="True" CssClass="vError">Select Holiday Date</asp:RequiredFieldValidator>

                                    </div>

                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12" id="divPHolidayDescp" runat="server" style="padding-top: 10px">
                                        <label runat="server">Public Holiday Description</label>
                                        <asp:TextBox ID="txtPubHldyDescp" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                            onkeypress="return alpha(event)" TabIndex="7"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPubHldyDescp"
                                            ValidationGroup="Addbutton" SetFocusOnError="True" CssClass="vError">Enter Holiday Description</asp:RequiredFieldValidator>

                                    </div>

                                    <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" id="divadd" runat="server" style="padding-top: 40px">
                                        <label runat="server"></label>
                                        <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" Font-Size="20px" ValidationGroup="Addbutton" OnClick="lbtnNew_Click">
                                             <i class="fas fa-plus-circle"></i>Add</asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="row">

                            <div class="col-sm-10 col-md-10 col-lg-10 col-xs-12" id="divTempGrid" runat="server" style="padding: 12px 15px 0px 65px;">
                                <asp:GridView ID="gvPHData" runat="server" DataKeyNames="HolidayDate"
                                    CssClass="CustomGrid table table-bordered table-condenced" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundField DataField="HolidayDate" HeaderText="Holiday Date" />
                                        <asp:BoundField DataField="WeekDays" HeaderText="Day" />
                                        <asp:BoundField DataField="HolidayDesc" HeaderText="Holiday Description" />

                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgDelete" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                                    runat="server" Font-Bold="true" ImageUrl="~/Images/Delete.png" OnClick="ImgDelete_Click" OnClientClick="return confirm('Are you sure to Delete this record?');" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <HeaderStyle CssClass="gvHead" />
                                    <AlternatingRowStyle CssClass="gvRow" />
                                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                </asp:GridView>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="form-submit">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BoatHouseName" TabIndex="8" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger" TabIndex="9" OnClick="btnCancel_Click" />
        </div>

        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvBoatHouse" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatHouseId" PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BoatHouseId" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat House Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Week Days" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblWeekDays" runat="server" Text='<%# Bind("WeekDays") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tariff Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblWdDescp" runat="server" Text='<%# Eval("WeekDaysDesc").ToString() == "WE" ? " Week End Tariff" : "Week Day Tariff" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Public Holiday Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblHdate" runat="server" Text='<%# Bind("HolidayDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Public Holiday Description" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblHDescp" runat="server" Text='<%# Bind("HolidayDesc") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnDelete" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Delete.png" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this entry?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfUserId" runat="server" />
    <asp:HiddenField ID="hfHouseAddress" runat="server" />
    <asp:HiddenField runat="server" ID="hfCreatedBy" />
    <asp:HiddenField runat="server" ID="hfBoatHouseId" />
    <asp:HiddenField runat="server" ID="hfAddres1" />
    <asp:HiddenField runat="server" ID="hfAddres2" />



    <script type="text/javascript">

        function minmax(value, min, max) {
            if (parseInt(value) < min || isNaN(parseInt(value)))
                return min;
            else if (parseInt(value) > max)
                return max;
            else return value;
        }


        //$(function checkUncheck() {
        //    $("[id*=chkSelectAll]").bind("click", function () {
        //        if ($(this).is(":checked")) {
        //            $("[id*=chkWorkingDays] input").prop("checked", true);
        //            $("[id*=chkWeekend] input").prop("checked", false);
        //            $("[id*=chkSelectAllWe]").prop("checked", false);
        //        }
        //        else {
        //            $("[id*=chkWorkingDays] input").prop("checked", false);
        //            $("[id*=chkWeekend] input").prop("checked", true);
        //            $("[id*=chkSelectAllWe]").prop("checked", true);
        //        }
        //    });

        //    $("[id*=chkWorkingDays] input").bind("click", function () {
        //        if ($("[id*=chkWorkingDays] input:checked").length == $("[id*=chkWorkingDays] input").length) {
        //            $("[id*=chkSelectAll]").prop("checked", true);
        //            // $("[id*=chkSelectAll]").attr("checked", "checked");
        //        }
        //        else {

        //            $("[id*=chkSelectAll]").prop("checked", false);
        //        }

        //    });

        //});

        $(document).ready(function () {
            $(".DashfrmDate").datepicker({
                dateFormat: 'dd/mm/yy',
                numberOfMonths: 1,
                changeMonth: true,
                changeYear: true,
            });


        });

        var specialKeys = new Array();
        specialKeys.push(35); //#
        specialKeys.push(37); //%
        specialKeys.push(43); //+
        specialKeys.push(45); //-
        specialKeys.push(61); //=
        specialKeys.push(64); //@

        function alpha(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        }
    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>
