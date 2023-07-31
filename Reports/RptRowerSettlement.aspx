<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Reports/RptRowerSettlement.aspx.cs" Inherits="Boating_RptRowerSettlement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">Rower Settlement</h5>
        <hr />

        <div class="row">
            <div class="col-xl-12 col-md-12 col-lg-12  col-sm-12">
                <div class="form-group">
                    <asp:RadioButtonList runat="server" ID="rbtnRowerMonthWise" TabIndex="1" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnRowerMonthWise_SelectedIndexChanged" CssClass="rbl">

                        <asp:ListItem Value="1" Selected="True">Date Wise</asp:ListItem>
                        <asp:ListItem Value="2">Month Wise</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div class="col-md-12 col-sm-12" runat="server" id="divRowerDateWise">
            <div class="row p-2">
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblFromDate" id="lbleffectivefrom" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="form-group">
                        <label for="lblToDate" id="Label2" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control frmDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtToDate"
                            ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-md-4 col-lg-4 col-sm-4" style="margin-top: 30px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" TabIndex="3" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" class="btn  btn-danger" TabIndex="4" OnClick="btnReset_Click" />
                    <asp:Button ID="btnGeneratePrint" runat="server" Text="Generate Print" CausesValidation="false" class="btn  btn-primary" TabIndex="5" OnClick="btnGeneratePrint_Click" />
                </div>

            </div>
        </div>

        <div class="col-md-12 col-sm-12" runat="server" id="divRowerMonthWise" visible="false">
            <div class="row p-2">
                <div class="col-sm-2 col-xs-12" runat="server">
                    <label for="lblMonthTo" id="lblMonthFinYear"><i class="fa fa-calendar" aria-hidden="true"></i>Financial Year<span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlRowerFinYear" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRowerFinYear_SelectedIndexChanged" TabIndex="7">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlRowerFinYear"
                        ValidationGroup="SearchMonth" SetFocusOnError="True" InitialValue="0" CssClass="vError">
                        Select Financial Year</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2 col-xs-12" runat="server" id="divRowerMonth" visible="false">
                    <label for="lblMonthTo" id="lblMonth"><i class="fa fa-calendar" aria-hidden="true"></i>Month<span class="spStar">*</span></label>
                    <asp:DropDownList ID="ddlRowerMonth" CssClass="form-control inputboxstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRowerMonth_SelectedIndexChanged" TabIndex="7">

                        <asp:ListItem Value="04">April</asp:ListItem>
                        <asp:ListItem Value="05">May</asp:ListItem>
                        <asp:ListItem Value="06">June</asp:ListItem>
                        <asp:ListItem Value="07">July</asp:ListItem>
                        <asp:ListItem Value="08">August</asp:ListItem>
                        <asp:ListItem Value="09">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                        <asp:ListItem Value="01">January</asp:ListItem>
                        <asp:ListItem Value="02">February</asp:ListItem>
                        <asp:ListItem Value="03">March</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server" visible="false">
                    <div class="form-group">
                        <label for="lblMonthFromDate" id="Label3" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>From Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtMonthFromDate" runat="server" CssClass=" form-control frmDate" AutoComplete="Off" TabIndex="1">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMonthFromDate"
                            ValidationGroup="SearchMonth" SetFocusOnError="True" CssClass="vError">
                            Enter From Date</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" runat="server" visible="false">
                    <div class="form-group">
                        <label for="lblMonthToDate" id="Label4" runat="server">
                            <i class="fa fa-calendar" aria-hidden="true"></i>To Date<span class="spStar">*</span></label>
                        <asp:TextBox ID="txtMonthToDate" runat="server" CssClass="form-control frmDate" AutoComplete="Off" TabIndex="2">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMonthToDate"
                            ValidationGroup="SearchMonth" SetFocusOnError="True" CssClass="vError">
                            Enter To Date</asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="col-md-4 col-lg-4 col-sm-4" style="margin-top: 30px;">
                    <asp:Button ID="btnAbstractPrint" runat="server" Text="Abstract Print" class="btn  btn-primary" ValidationGroup="SearchMonth" TabIndex="5" OnClick="btnAbstractPrint_Click" />
                </div>
            </div>
        </div>

        <div runat="server" id="divGridList" visible="false" class="col-sm-8 col-xs-12" style="overflow: auto; max-height: 800px; max-width: 800px; min-height: 200px; min-width: 30%;">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="grvSettle" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="SettlementId" OnPageIndexChanging="grvSettle_PageIndexChanging" ShowFooter="true">
                    <columns>
                        <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <%#Container.DataItemIndex+1 %>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settlement Id" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblSettlementId" runat="server" Text='<%# Bind("SettlementId") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settlement Date" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblSettlementDate" runat="server" Text='<%# Bind("SettlementDate") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rower" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settlement Amount" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:LinkButton ID="lblSettlementAmt" runat="server" Text='<%# Bind("SettlementAmt") %>' OnClick="lblSettlementAmt_Click"></asp:LinkButton>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                            <footertemplate>
                                <asp:Label ID="lblTotal" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                            </footertemplate>
                        </asp:TemplateField>
                    </columns>
                    <headerstyle cssclass="gvHead" />
                    <alternatingrowstyle cssclass="gvRow" />
                    <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                </asp:GridView>
            </div>
        </div>


        <div runat="server" id="divRowerMonthList" visible="false" class="col-sm-8 col-xs-12" style="overflow: auto; max-height: 800px; max-width: 800px; min-height: 200px; min-width: 30%;">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvRowerMonth" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="SettlementId" OnPageIndexChanging="grvSettle_PageIndexChanging" ShowFooter="true">
                    <columns>
                        <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <%#Container.DataItemIndex+1 %>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rower Id" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblRowerId" runat="server" Text='<%# Bind("RowerId") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rower" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                            </itemtemplate>
                            <itemstyle horizontalalign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settlement Amount" HeaderStyle-CssClass="grdHead">
                            <itemtemplate>
                                <asp:LinkButton ID="lblSettlementAmt" runat="server" Text='<%# Bind("SettlementAmt") %>' OnClick="lblSettlementAmt_Click"></asp:LinkButton>
                            </itemtemplate>
                            <itemstyle horizontalalign="Right" />
                            <footertemplate>
                                <asp:Label ID="lblTotal" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                            </footertemplate>
                        </asp:TemplateField>
                    </columns>
                    <headerstyle cssclass="gvHead" />
                    <alternatingrowstyle cssclass="gvRow" />
                    <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <ajax:dragpanelextender id="DragPanelExtender2" runat="server" targetcontrolid="pnlTrip" draghandleid="pnlDrag3"></ajax:dragpanelextender>
    <ajax:modalpopupextender id="MpeTrip" runat="server" behaviorid="MpeTrip" targetcontrolid="HiddenField1" popupcontrolid="pnlTrip"
        backgroundcssclass="modalBackground">
    </ajax:modalpopupextender>

    <asp:Panel ID="pnlTrip" runat="server" CssClass="Msg">
        <asp:Panel ID="pnlDrag3" runat="server" CssClass="drag">
            <div class="modal-content" style="width: 750px">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Rower Trip Details For Settlement Id : 
                       <asp:Label ID="lblRowerSettlementId" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </h5>
                    <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                </div>
                <div class="modal-body">

                    <asp:GridView ID="gvRowerTripDetails" runat="server" AllowPaging="True"
                        CssClass="CustomGrid table table-bordered table-condenced" AutoGenerateColumns="False" Width="100%"
                        PageSize="25000" ShowFooter="true">
                        <columns>
                            <asp:TemplateField HeaderText="Sno." HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingId" runat="server" Text='<%# Bind("BookingId") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBookingDate" runat="server" Text='<%# Bind("BookingDate") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Boat Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblBoatType" runat="server" Text='<%# Bind("BoatType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Seater Type" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblSeaterType" runat="server" Text='<%# Bind("SeaterType") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Center" />
                                <footerstyle horizontalalign="Center" font-bold="true" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rower Charge" HeaderStyle-CssClass="grdHead">
                                <itemtemplate>
                                    <asp:Label ID="lblActualRowerCharge" runat="server" Text='<%# Bind("ActualRowerCharge") %>'></asp:Label>
                                </itemtemplate>
                                <itemstyle horizontalalign="Right" />
                                <footerstyle horizontalalign="Right" />
                            </asp:TemplateField>
                        </columns>
                        <headerstyle cssclass="gvHead" />
                        <alternatingrowstyle cssclass="gvRow" />
                        <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                        <footerstyle backcolor="White" forecolor="#000066" font-bold="true" />
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

