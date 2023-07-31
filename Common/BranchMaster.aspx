<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Common/BranchMaster.aspx.cs" Inherits="Master_BranchMaster" %>

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


        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                <h5 class="pghr">Branch Master 
                              <span style="float: right;">
                                  <asp:LinkButton ID="lbtnOpStatus" CssClass="lbtnNew" runat="server" OnClick="lbtnOpStatus_Click">                          
                                   <i class="fas fa-building"></i> Change Branch Operating Status</asp:LinkButton>
                                  <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span></h5>
                <hr />
            </div>
        </div>

         <div style="padding: 0;" id="divEntry" runat="server">

                    <div class="col-xs-12" style="padding: 0; margin-top: 10px;display: flex;">
                        <div class="col-xs-12 col-sm-2">
                            Corporate Office<strong style="color: Red"> *</strong>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlCorporateOff" CssClass="form-control" TabIndex="1" AutoComplete="off" 
                                Enabled="true" >
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please, Select Corporate Office"
                                ControlToValidate="ddlCorporateOff" SetFocusOnError="True" InitialValue="Select Corporate Office"
                                ValidationGroup="BranchMstr"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-6" id="DivNotOperation" runat="server" visible="false">
                            <asp:Label ID="lblNooperation" runat="server" CssClass="blink" ForeColor="Red" Font-Size="X-Large" Font-Bold="true">* * * THIS BRANCH
IS NOT IN OPERATION * * *</asp:Label>
                        </div>

                    </div>

                    <div class="col-xs-12" style="padding: 0; margin-top: 10px;display: flex;">
                        <div class="col-xs-12 col-sm-2">
                            Branch Code<strong style="color: Red"> *</strong>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox runat="server" ID="txtBranchCode" CssClass="form-control" TabIndex="2" MaxLength="10" AutoComplete="off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqBranchCode" runat="server" ErrorMessage="Enter Branch Code"
                                ControlToValidate="txtBranchCode" SetFocusOnError="True" CssClass="vError" ValidationGroup="BranchMstr"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-2">
                            Branch Name<strong style="color: Red"> *</strong>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox runat="server" ID="txtBranchName" CssClass="form-control" TabIndex="3" MaxLength="100" AutoComplete="off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter Branch Name"
                                ControlToValidate="txtBranchName" SetFocusOnError="True" CssClass="vError" ValidationGroup="BranchMstr"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-xs-12" style="padding: 0; margin-top: 10px;display: flex;">
                        <div class="col-xs-12 col-sm-2">
                            Branch Type<strong style="color: Red"> *</strong>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlBranchType" CssClass="form-control" TabIndex="4">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please, Select Branch Type"
                                ControlToValidate="ddlBranchType" SetFocusOnError="True" InitialValue="Select Branch Type"
                                CssClass="vError" ValidationGroup="BranchMstr"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-2">
                            Branch Region<strong style="color: Red"> *</strong>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlBranchRegion" CssClass="form-control" TabIndex="5">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please, Select Branch Region"
                                ControlToValidate="ddlBranchRegion" SetFocusOnError="True"
                                InitialValue="Select Branch Region" CssClass="vError" ValidationGroup="BranchMstr"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-xs-12" style="padding: 0; margin-top: 10px;display: flex;">
                        <div class="col-xs-12 col-sm-2">
                            Address 1
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox runat="server" ID="txtAddress1" CssClass="form-control" TabIndex="6" MaxLength="100" AutoComplete="off">
                            </asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-2">
                            Address 2
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox runat="server" ID="txtAddress2" CssClass="form-control" TabIndex="7" MaxLength="100" AutoComplete="off">
                            </asp:TextBox>
                        </div>
                    </div>

                    <div class="col-xs-12" style="padding: 0; margin-top: 10px;display: flex;">
                        <div class="col-xs-12 col-sm-2">
                            ZipCode
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox runat="server" ID="txtZipCode" CssClass="form-control" TabIndex="8" MaxLength="6" AutoComplete="off"
                                AutoPostBack="true" OnTextChanged="txtZipCode_TextChanged" onkeypress="return isNumber(event)">
                            </asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-2">
                            City
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox runat="server" ID="txtCity" CssClass="form-control" TabIndex="9" MaxLength="50" AutoComplete="off">
                            </asp:TextBox>
                        </div>
                    </div>

                    <div class="col-xs-12" style="padding: 0; margin-top: 10px;display: flex;">
                        <div class="col-xs-12 col-sm-2">
                            District
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox runat="server" ID="txtDistrict" CssClass="form-control" TabIndex="10" MaxLength="50" AutoComplete="off">
                            </asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-2">
                            State
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:TextBox runat="server" ID="txtState" CssClass="form-control" TabIndex="11" MaxLength="50" AutoComplete="off">
                            </asp:TextBox>
                        </div>
                    </div>

                    <div class="col-xs-12" style="padding: 0; margin-top: 10px;display: flex;">
                        <div class="col-xs-12 col-sm-2">
                            Country<strong style="color: Red"> *</strong>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <asp:DropDownList runat="server" ID="ddlCountry" CssClass="form-control" TabIndex="12">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please, Select Country"
                                ControlToValidate="ddlCountry" SetFocusOnError="True"
                                InitialValue="Select Country" CssClass="vError" ValidationGroup="BranchMstr"></asp:RequiredFieldValidator>
                        </div>


                    </div>

                    <div class="col-xs-12" style="padding: 0; margin-top: 10px;display: flex;place-content: center;">
                        <div class="col-xs-12 text-center">
                            <span style="display: inline-block;">
                                <asp:Button runat="server" ID="btnSubmit" Text="Submit" Width="100px" CssClass="btn btn-primary"
                                    ValidationGroup="BranchMstr" TabIndex="13" OnClick="btnSubmit_Click" />
                            </span>

                            <span style="display: inline-block">
                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" Width="100px" CssClass="btn btn-danger"
                                    TabIndex="14" CausesValidation="false" OnClick="btnCancel_Click" />
                            </span>
                        </div>
                    </div>
                </div>


        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;margin-top:1rem;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>

                <asp:GridView ID="gvBranchMaster" runat="server" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="CorpId, BranchCode, BranchId, BranchName, BranchType, 
                            BranchRegion, Address1,Address2,Zipcode,City, District,State,Country,OperatingStatus">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch Code" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchCode" runat="server" Text='<%# Bind("BranchCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("BranchName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch Type" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchTypeName" runat="server" Text='<%# Bind("BranchTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch Region" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchRegionName" runat="server" Text='<%# Bind("BranchRegionName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ZipCode" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblZipcode" runat="server" Text='<%# Bind("Zipcode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="City" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="District" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDistrict" runat="server" Text='<%# Bind("District") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="State" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Country" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCountryName" runat="server" Text='<%# Bind("CountryName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Operating Status" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblOperatingStatus" runat="server" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Operative"
                                    Font-Bold="true" Visible='<%# Eval("OperatingStatus").ToString() == "Y" ? true: false %>'></asp:Label>

                                <asp:Label ID="lblOperatingStatusInactive" runat="server" ForeColor="#e41515" CausesValidation="false" Font-Underline="false"
                                    Text="InOperative" Font-Bold="true" Visible='<%# Eval("OperatingStatus").ToString() == "N" ? true: false %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click"
                                    Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClientClick="return confirm('Are you sure you want to Inactive this record?');"
                                    Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>'
                                    OnClick="ImgBtnDelete_Click" />

                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClientClick="return confirm('Are you sure you want to Active this record?');"
                                    Visible='<%# Eval("ActiveStatus").ToString() == "D"? true: false %>'
                                    OnClick="ImgBtnUndo_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hfBranchId" runat="server" />
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

