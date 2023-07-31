<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    CodeFile="~/Common/ShiftMaster.aspx.cs" Inherits="ShiftMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="container">
        <div class="row">
            <script type="text/javascript">
                function functionx(evt) {
                    if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                        //alert("Allow Only Numbers");
                        return false;
                    }
                }
            </script>

            <div class="form-body col-sm-12 col-xs-12">
                <h5 class="pghr">Shift Master <span style="float: right;">
                    <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="btnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
                <hr />
                <div id="divEntry" runat="server" visible="false">
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                                <div class="form-group">
                                    <label for="lblshiftid">Shift ID</label>
                                    <asp:TextBox ID="txtshiftId" runat="server" CssClass="form-control" AutoComplete="Off">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblshiftName">Shift Name <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtshiftname" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="100" TabIndex="1">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtshiftname"
                                        SetFocusOnError="True" CssClass="vError">Enter Shift Name</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblstarttime">Shift Start Time <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtstarttime" runat="server" TextMode="Time" CssClass="form-control" AutoComplete="Off" TabIndex="2" OnTextChanged="txtstarttime_TextChanged" AutoPostBack="true">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtstarttime"
                                        SetFocusOnError="True" CssClass="vError">Enter Start Time</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblendtime">Shift End Time <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtendtime" runat="server" TextMode="Time" AutoPostBack="True" CssClass="form-control"
                                        AutoComplete="Off" TabIndex="3" OnTextChanged="txtendtime_TextChanged">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtendtime"
                                        SetFocusOnError="True" CssClass="vError">Enter End Time</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row p-2">
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblBreakStartTime">Break Start Time <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtBreakStartTime" runat="server" TextMode="Time" CssClass="form-control" AutoComplete="Off" TabIndex="4"
                                        OnTextChanged="txtBreakStartTime_TextChanged" AutoPostBack="true">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBreakStartTime"
                                        SetFocusOnError="True" CssClass="vError">Enter Break Start Time</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblBreakEndTime">Break End Time <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtBreakendtime" runat="server" TextMode="Time" CssClass="form-control" AutoComplete="Off" TabIndex="5"
                                        OnTextChanged="txtBreakendtime_TextChanged">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtBreakendtime"
                                        SetFocusOnError="True" CssClass="vError">Enter Break End Time</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label for="lblgraceperiod">Grace Period <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtGracePeriod" runat="server" CssClass="form-control" MaxLength="2"
                                        onkeypress="return functionx(event)" AutoComplete="Off" TabIndex="6"> 
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtGracePeriod"
                                        SetFocusOnError="True" CssClass="vError">Enter Grace Period</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12 text-right pt-3">
                                <div class="form-submit">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" TabIndex="7" OnClick="btnSubmit_Click1" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger" TabIndex="8" OnClick="btnCancel_Click" />
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
                        <asp:GridView ID="gvshifMaster" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                            AutoGenerateColumns="False" PageSize="10" DataKeyNames="ShiftId,ActiveStatus,ShiftName,StartTime,GracePeriod,EndTime,BreakStartTime,BreakEndTime" OnPageIndexChanging="gvshifMaster_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ShiftId" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblshiftId" runat="server" Text='<%# Bind("ShiftId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shift Name" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblShiftName" runat="server" Text='<%# Bind("ShiftName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Start Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStartTime" runat="server" Text='<%# Bind("StartTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="End Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEndTime" runat="server" Text='<%# Bind("EndTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Break Start Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBreakStartTime" runat="server" Text='<%# Bind("BreakStartTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Break End Time" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBreakEndTime" runat="server" Text='<%# Bind("BreakEndTime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Grace Period" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGracePeriod" runat="server" Text='<%# Bind("GracePeriod") %>'></asp:Label>
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
                                            runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click"
                                            Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                            runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');"
                                            Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />

                                        <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                            runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');"
                                            Visible='<%# Eval("ActiveStatus").ToString() == "D"? true: false %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
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
    <asp:HiddenField ID="hfshiftId" runat="server" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

