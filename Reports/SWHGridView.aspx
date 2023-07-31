<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="SWHGridView.aspx.cs" Inherits="Reports_SWHGridView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body">
        <h5 class="pghr">Service Wise Report History
            <span style="float: right;">
                <asp:LinkButton ID="lbtnServiceReport" CssClass="lbtnNew" runat="server" OnClick="lbtnServiceReport_Click">
                    <i class="fas fa-receipt"></i>Service Wise Report</asp:LinkButton>
            </span>
        </h5>
        <div class="row">
            <div class="col-sm-2 col-xs-12" runat="server">
                <label for="lblTypes" id="lblFromdate"><i class="fa fa-calendar" aria-hidden="true"></i>From Date <span class="spStar">*</span></label>
                <asp:TextBox ID="txtFromDate" CssClass="form-control frmDate" runat="server"
                    TabIndex="10">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtFromDate"
                    ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                    Select From Date</asp:RequiredFieldValidator>
            </div>
            <div class="col-sm-2 col-xs-12" runat="server">
                <label for="lblTypes" id="lblTodate"><i class="fa fa-calendar" aria-hidden="true"></i>To Date <span class="spStar">*</span></label>
                <asp:TextBox ID="txtToDate" CssClass="form-control toDate" runat="server"
                    TabIndex="10">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToDate"
                    ValidationGroup="Search" SetFocusOnError="True" CssClass="vError">
                    Select To Date</asp:RequiredFieldValidator>
            </div>
            <div class="col-sm-2 col-xs-12" style="padding-right: 4rem; padding-top: 2rem;">
                <span style="float: left">
                    <asp:Button ID="btnFilter" runat="server" Text="Search" class="btn btn-primary" ValidationGroup="Search" OnClick="btnFilter_Click" TabIndex="11" />
                </span>
            </div>
        </div>
        <div class="col-sm-12 col-xs-12">
            <div class="row">

                <div class="col-sm-10 col-xs-10" id="divServiceWise" runat="server" visible="false">
                    <div class="table-responsive">

                        <asp:GridView ID="GVServiceWiseHistroy" runat="server" AllowPaging="false" CssClass="gvv display table table-bordered table-condenced"
                            AutoGenerateColumns="False" DataKeyNames="UniqueId,UserName,UserId,CategoryId,CategoryName,BookingDate,ServiceId" PageSize="25000" Width="100%">
                            <columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <itemtemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </itemtemplate>
                                    <itemstyle horizontalalign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UniqueId" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <itemtemplate>
                                        <asp:Label ID="lblUniqueId" runat="server" Text='<%# Bind("UniqueId") %>'></asp:Label>
                                    </itemtemplate>
                                    <itemstyle horizontalalign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking Date" HeaderStyle-CssClass="grdHead">
                                    <itemtemplate>
                                        <asp:LinkButton ID="lblBookingDate" runat="server" Text=' <%#Eval("BookingDate")%>' OnClick="lblBookingDate_Click"></asp:LinkButton>
                                    </itemtemplate>
                                    <itemstyle horizontalalign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Id" HeaderStyle-CssClass="grdHead">
                                    <itemtemplate>
                                        <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                                    </itemtemplate>
                                    <itemstyle horizontalalign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Name " HeaderStyle-CssClass="grdHead">
                                    <itemtemplate>
                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                                    </itemtemplate>
                                    <itemstyle horizontalalign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Service" HeaderStyle-CssClass="grdHead">
                                    <itemtemplate>
                                        <asp:Label ID="lblServices" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                    </itemtemplate>
                                    <itemstyle horizontalalign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Service Total" HeaderStyle-CssClass="grdHead">
                                    <itemtemplate>
                                        <asp:Label ID="lblServiceTotal" runat="server" Text=' <%#Eval("ServiceTotal")%>'></asp:Label>
                                    </itemtemplate>
                                    <itemstyle horizontalalign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <itemtemplate>
                                        <asp:ImageButton ID="ImgBtnDelete" runat="server" OnClick="ImgBtnDelete_Click" ImageUrl="~/images/Delete.png" Height="15px" Width="15px" OnClientClick="return confirm('Are you sure to Delete this record?');" />
                                    </itemtemplate>
                                    <itemstyle horizontalalign="Center" width="15%" />
                                </asp:TemplateField>

                            </columns>
                        </asp:GridView>
                        <span id="spnote" runat="server" style="color: gray">Note: Click Booking Date, View an Denomination Details</span>
                    </div>
                </div>
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
                    <h5 class="modal-title" id="exampleModalLabel">
                        <asp:Label ID="lblRowerSettlementId" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </h5>
                    <asp:ImageButton ID="imgCloseTicket" runat="server" OnClick="imgCloseTicket_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                </div>
                <div class="modal-body">
                    <div class="col-sm-12 col-xs-12">
                        <div class="row">
                            <div class="col-sm-12 col-xs-12" id="divServiceDeno" runat="server" visible="false">
                                <div class="table-responsive">
                                    <asp:GridView ID="GVServiceWiseDenomination" runat="server" AllowPaging="True" CssClass="CustomGrid table table-bordered table-condenced"
                                        AutoGenerateColumns="False" PageSize="20" Width="100%" ShowFooter="true">
                                        <columns>
                                            <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                                <itemtemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </itemtemplate>
                                                <itemstyle horizontalalign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Denomination " HeaderStyle-CssClass="grdHead">
                                                <itemtemplate>
                                                    <asp:Label ID="lblDenomination" runat="server" Text='<%# Bind("Denomination") %>'></asp:Label>
                                                </itemtemplate>
                                                <itemstyle horizontalalign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Denomination Count" HeaderStyle-CssClass="grdHead">
                                                <itemtemplate>
                                                    <asp:Label ID="lblDenominationCount" runat="server" Text='<%# Bind("DenominationCount") %>'></asp:Label>
                                                </itemtemplate>
                                                <itemstyle horizontalalign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Denomination Amount" HeaderStyle-CssClass="grdHead">
                                                <itemtemplate>
                                                    <asp:Label ID="lblDenominationAmount" runat="server" Text=' <%#Eval("DenominationAmount")%>'></asp:Label>
                                                </itemtemplate>
                                                <itemstyle horizontalalign="Right" />
                                            </asp:TemplateField>
                                        </columns>
                                        <headerstyle cssclass="gvHead" />
                                        <alternatingrowstyle cssclass="gvRow" />
                                        <pagerstyle horizontalalign="Center" cssclass="gvPage" />
                                        <footerstyle backcolor="White" forecolor="#000066" font-bold="true" horizontalalign="Right" />
                                    </asp:GridView>
                                    <asp:ImageButton ID="Pdf" runat="server" ImageUrl="~/images/Print.svg" Width="35px" Style="float: right" ToolTip="Print" OnClick="Pdf_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

