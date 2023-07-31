<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="OnlinePaymentRights.aspx.cs" Inherits="Sadmin_OnlinePaymentRights" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Online Paymemnt Block Rights <span style="float: right;">
            <asp:LinkButton ID="lbtnBlockReason" CssClass="lbtnNew" runat="server" OnClick="lbtnBlockReason_Click"> 
                <i class="fas fa-receipt"></i>View Report</asp:LinkButton></span> </h5>
        <hr />
        <div class="mydivbrdr" runat="server" id="divEntry">
            <div class="row p-2">
                <div class="col-sm-12 col-xs-12">
                    <div class="row m-0">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Branch Type</label>
                                <asp:DropDownList ID="ddlBranchType" runat="server" CssClass="form-control" TabIndex="1">
                                    <asp:ListItem Value="0">Select Branch Type</asp:ListItem>
                                    <asp:ListItem Value="1">Boating</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlBranchType"
                                    ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Branch Type</asp:RequiredFieldValidator>
                            </div>
                        </div>
                         <div class="col-sm-3 col-xs-12" id="divCorp" runat="server">
                            <div class="form-group">
                                <label runat="server" id="Label11"><i class="fa fa-address-book" aria-hidden="true"></i>Corporate Office<span class="spStar">*</span></label>
                                <asp:DropDownList ID="ddlCorpId" runat="server" TabIndex="2" CssClass="form-control inputboxstyle"
                                    OnSelectedIndexChanged="ddlCorpId_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlCorpId" InitialValue="Select Corporate Office"
                                    ValidationGroup="BoatType" SetFocusOnError="True" CssClass="vError">Select Corporate Office</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12" id="divBoatHouse" runat="server">
                            <div class="form-group">
                                <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Boat House Name</label>
                                <asp:DropDownList ID="ddlBoatHouseId" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBoatHouseId"
                                    ValidationGroup="BoatType" SetFocusOnError="True" InitialValue="Select Boat House" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                            </div>
                        </div>
                       
                    </div>

                    <div class="row m-0">
                        <div class="col-sm-3 col-xs-12" style="background-color: #93ff501a">
                            <div class="form-group">
                                <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Application Type</label>
                                <asp:RadioButtonList ID="rblApplicationType" runat="server" RepeatDirection="Horizontal" TabIndex="3" CssClass="rbl">
                                    <asp:ListItem Value="Department" Selected="true">Department</asp:ListItem>
                                    <asp:ListItem Value="Kiosk">Kiosk</asp:ListItem>
                                    <asp:ListItem Value="Public">Public</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rblApplicationType"
                                    ValidationGroup="BoatType" SetFocusOnError="True" CssClass="vError">Select Application Type</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        &nbsp&nbsp&nbsp
                        <div class="col-sm-3 col-xs-12" style="background-color: #ffa9501a; padding-left: 5px;">
                            <div class="form-group">
                                <label for="boat"><i class="fa fa-ship" aria-hidden="true"></i>Block Payment Type</label>
                                <asp:RadioButtonList ID="rblBlockType" runat="server" RepeatDirection="Horizontal" TabIndex="4" CssClass="rbl">
                                    <asp:ListItem Value="Both" Selected="true">Both</asp:ListItem>
                                    <asp:ListItem Value="PG">Payment Gateway</asp:ListItem>
                                    <asp:ListItem Value="UPI">UPI</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rblBlockType"
                                    ValidationGroup="BoatType" SetFocusOnError="True" CssClass="vError">Select Block Type</asp:RequiredFieldValidator>
                            </div>
                        </div>


                    </div>

                    <div class="row m-0">
                        <div class="col-sm-6 col-xs-12">
                            <div class="form-group">
                                <label runat="server" id="Label1">Block Reason</label>
                                <asp:TextBox ID="txtBlockReason" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                    MaxLength="150" Font-Size="14px" TabIndex="1" TextMode="MultiLine" Rows="3">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtBlockReason"
                                    ValidationGroup="BoatType" SetFocusOnError="True" CssClass="vError">Enter Block Reason</asp:RequiredFieldValidator>
                            </div>
                            <asp:Label ID="Label29" runat="server" ForeColor="Red" Font-Size="Small" Font-Bold="true">
                             **Maximum 150 Characters
                            </asp:Label>
                        </div>
                        <div class="col-sm-3 col-xs-12" style="margin-top: 55px">
                            <div class="form-submit" style="text-align: left !important">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BoatType" TabIndex="5" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn btn-danger" TabIndex="6" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divGrid" runat="server" class="col-sm-12 col-xs-12">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvPaymentRights" runat="server" AllowPaging="True"
                    CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="UniqueId,ApplicationType,BranchType,BranchId,BranchName,BlockType,CreatedBy,BlockedDate"
                    PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Application Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblApplicationType" runat="server" Text='<%# Bind("ApplicationType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchType" runat="server" Text='<%# Bind("BranchType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("BranchName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Block Payment Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBlockType" runat="server" Text='<%# Bind("BlockType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Blocked Date" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBlockedDate" runat="server" Text='<%# Bind("BlockedDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Blocked Reason" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBlockedReason" runat="server" Text='<%# Bind("BlockReason") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="UnBlock" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" runat="server" ForeColor="Green"
                                    Font-Bold="true" OnClick="ImgBtnDelete_Click">UnBlock</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <%-- REASON POPUP--%>

        <asp:HiddenField ID="hfBBpupup" runat="server" />
        <ajax:ModalPopupExtender ID="MPEBBpopup" runat="server" BehaviorID="MPEBBpopup" TargetControlID="hfBBpupup" PopupControlID="PnlBBRevenue"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="PnlBBRevenue" runat="server" CssClass="Msg" Style="display: none; min-height: 200px; width: 500px; margin-top: 25px;">
            <asp:Panel ID="pnlDragBBRevenue" runat="server" CssClass="drag">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #004c8c; color: white">
                        <h5 class="modal-title">UnBlock Reason</h5>
                        <asp:ImageButton ID="ImgCloseBB" runat="server" OnClick="ImgCloseBB_Click" ImageUrl="~/images/Close.svg" Width="30px" ToolTip="Close" />
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12" runat="server">
                                <div class="form-group">
                                    <label runat="server" id="Label3">REASON</label>
                                    <asp:TextBox ID="txtUnBlockReason" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                        MaxLength="150" Font-Size="14px" TabIndex="1" TextMode="MultiLine" Rows="3">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUnBlockReason"
                                        ValidationGroup="Reason" SetFocusOnError="True" CssClass="vError">Enter Reason</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-submit">
                                <asp:Button ID="btnSubmitReason" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Reason" TabIndex="2" OnClick="btnSubmitReason_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
    </div>
    <%-- Newly implemented for CSRF Validation--%>

    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

