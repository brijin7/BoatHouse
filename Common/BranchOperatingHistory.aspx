<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Common/BranchOperatingHistory.aspx.cs" Inherits="Master_BranchOperatingHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <style>
        .blink {
            animation: blink-animation 1s linear infinite;
            -webkit-animation: blink-animation 1s linear infinite;
        }

        @keyframes blink-animation {
            50% {
                opacity: 0;
            }
        }

        @-webkit-keyframes blink-animation {
            50% {
                opacity: 0;
            }
        }
    </style>
    <div class="form-body">



        <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
            <h5 class="pghr">Branch Operating History 
                              <span style="float: right;">
                                  <asp:LinkButton ID="lbtnBrnchMstr" CssClass="lbtnNew" runat="server" OnClick="lbtnBrnchMstr_Click">                          
                                   <i class="fas fa-building"></i>Back to Branch Master</asp:LinkButton></span></h5>
            <hr />
        </div>


        <div class="col-12 " style="padding: 0;" id="divEntry" runat="server">
            <div class="col-xs-12" style="padding: 0; margin-top: 10px; display: flex;">
                <div class="col-xs-12 col-sm-2">
                    Corporate Office<strong style="color: Red"> *</strong>
                </div>
                <div class="col-xs-12 col-sm-4">
                    <asp:DropDownList runat="server" ID="ddlCorporateOff" CssClass="form-control" TabIndex="1" AutoComplete="off" Enabled="false">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please, Select Corporate Office"
                        ControlToValidate="ddlCorporateOff" SetFocusOnError="True" InitialValue="Select Corporate Office" CssClass="vError"
                        ValidationGroup="BranchOpHstry"></asp:RequiredFieldValidator>
                </div>

                <div class="col-xs-12 col-sm-2">
                    Branch<strong style="color: Red"> *</strong>
                </div>
                <div class="col-xs-12 col-sm-4">
                    <asp:DropDownList runat="server" ID="ddlBranchCode" CssClass="form-control" TabIndex="2" AutoComplete="off"
                        OnSelectedIndexChanged="ddlBranchCode_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please, Select Branch"
                        ControlToValidate="ddlBranchCode" SetFocusOnError="True" InitialValue="Select Branch" CssClass="vError"
                        ValidationGroup="BranchOpHstry"></asp:RequiredFieldValidator>
                </div>

            </div>

            <div class="col-xs-12" style="padding: 0; margin-top: 10px; display: flex;">
                <div class="col-xs-12 col-sm-2">
                    Operating Status<strong style="color: Red"> *</strong>
                </div>
                <div class="col-xs-12 col-sm-4">
                    <asp:DropDownList runat="server" ID="ddlOpStatus" CssClass="form-control" TabIndex="3">
                        <asp:ListItem Value="Select Operating Status" Text="Select Operating Status"></asp:ListItem>
                        <asp:ListItem Value="Y" Text="Operative"></asp:ListItem>
                        <asp:ListItem Value="N" Text="InOperative"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please, Select Operating Status"
                        ControlToValidate="ddlOpStatus" SetFocusOnError="True"
                        InitialValue="Select Operating Status" CssClass="vError" ValidationGroup="BranchOpHstry"></asp:RequiredFieldValidator>
                </div>

                <div class="col-xs-12 col-sm-2">
                    Operative Date <strong style="color: Red">*</strong>
                </div>
                <div class="col-xs-12 col-sm-4">
                    <asp:TextBox runat="server" ID="txtOpDate" CssClass="form-control myCalMax0" TabIndex="4" MaxLength="100" AutoComplete="off">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Select Operative Date"
                        ControlToValidate="txtOpDate" SetFocusOnError="True" CssClass="vError" ValidationGroup="BranchOpHstry"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="col-xs-12" style="padding: 0; margin-top: 10px; display: flex;place-content: center;">
                <div class="col-xs-12 text-center">
                    <span style="display: inline-block;">
                        <asp:Button runat="server" ID="btnSubmit" Text="Submit" Width="100px" CssClass="btn btn-primary"
                            ValidationGroup="BranchOpHstry" TabIndex="5" OnClick="btnSubmit_Click" />
                    </span>

                    <span style="display: inline-block">
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" Width="100px" CssClass="btn btn-danger"
                            TabIndex="6" CausesValidation="false" OnClick="btnCancel_Click" />
                    </span>
                </div>
            </div>
        </div>



    </div>

    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

