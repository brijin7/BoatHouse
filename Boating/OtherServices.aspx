<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Boating/OtherServices.aspx.cs" Inherits="OtherServices" Async="true" %>

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
                (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // Check minus and only once.
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // Check for dots and only once.
                (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Other Services <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server">
            <div class="mydivbrdr">
                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label5"><i class="fa fa-address-book" aria-hidden="true"></i>Service Type <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control inputboxstyle"
                                TabIndex="1" AutoComplete="Off" AutoPostBack="true" OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged">
                                <asp:ListItem Value="0">Select Service Type</asp:ListItem>
                                <asp:ListItem Value="OS">Other Services</asp:ListItem>
                                <asp:ListItem Value="AB">Additional Boat Ticket</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlServiceType" InitialValue="0"
                                ValidationGroup="Otherservices" SetFocusOnError="True" CssClass="vError">Select Service Type</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label1"><i class="fa fa-address-book" aria-hidden="true"></i>Category <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlCategoryName" runat="server" CssClass="form-control inputboxstyle"
                                TabIndex="2" AutoComplete="Off">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCategoryName" InitialValue="Select Category"
                                ValidationGroup="Otherservices" SetFocusOnError="True" CssClass="vError">Select Category</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                        <div class="form-group">
                            <label runat="server" id="Label3"><i class="fa fa-ship" aria-hidden="true"></i>Service Id</label>
                            <asp:TextBox ID="txtserviceId" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="10" Font-Size="14px" TabIndex="3" />
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label4"><i class="fa fa-address-book" aria-hidden="true"></i>Service Name <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtServiceName" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="15" Font-Size="14px" TabIndex="4" AutoComplete="Off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtServiceName"
                                ValidationGroup="Otherservices" SetFocusOnError="True" CssClass="vError">Enter Service Name</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label runat="server" id="Label2"><i class="fa fa-address-book" aria-hidden="true"></i>Short Name <span class="spStar">*</span></label>
                            <asp:TextBox ID="txtShortName" runat="server" CssClass="form-control inputboxstyle"
                                MaxLength="15" Font-Size="14px" TabIndex="5" AutoComplete="Off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtShortName"
                                ValidationGroup="Otherservices" SetFocusOnError="True" CssClass="vError">Enter Short Name</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label for="lbloption" id="lbltimetax">Tax <span class="spStar">*</span></label>
                            <asp:DropDownList ID="ddlTaxId" runat="server" CssClass="form-control inputboxstyle"
                                TabIndex="6" onchange="SelectTax(this.options[this.selectedIndex].text);">
                                <%-- <asp:ListItem Value="Select">Select Tax </asp:ListItem>--%>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlTaxId"
                                ValidationGroup="Otherservices" SetFocusOnError="True" InitialValue="Select Tax" CssClass="vError">Select Tax</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <i class="fa fa-money-check-alt"></i>Service Total 
                                <span style="color: #3278be;">(Inclusive of Tax) </span>(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            <asp:TextBox ID="txtTotalAmt" runat="server" CssClass="form-control ServiceCharge" AutoPostBack="true" OnTextChanged="txtTotalAmt_TextChanged"
                                onkeypress="return isNumber(event)"
                                MaxLength="7" Font-Size="14px" TabIndex="5" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTotalAmt"
                                ValidationGroup="Otherservices" SetFocusOnError="True" CssClass="vError">Enter Service Total</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="row p-2">

                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <i class="fa fa-money-check-alt"></i>Charge Per Item 
                                (<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            <asp:TextBox ID="txtChargePerItem" runat="server" CssClass="form-control decimal"
                                MaxLength="5" Font-Size="14px" TabIndex="8" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtChargePerItem"
                                ValidationGroup="Otherservices" SetFocusOnError="True" CssClass="vError">Enter Charge Per Item</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <i class="fa fa-money-check-alt"></i>Service Tax
                                (<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            <asp:TextBox ID="txtServiceTax" runat="server" CssClass="form-control decimal"
                                MaxLength="5" Font-Size="14px" TabIndex="9" AutoComplete="Off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtServiceTax"
                                ValidationGroup="Otherservices" SetFocusOnError="True" CssClass="vError">Enter Service Tax</asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row p-2">
                    <div class="col-sm-4 col-xs-12 text-right pt-3">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Otherservices" TabIndex="10" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="11" OnClick="btnCancel_Click" />
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
                <asp:GridView ID="gvOtherServices" runat="server" AllowPaging="True"
                    CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                    DataKeyNames="Category,ServiceId,ServiceName,ShortName,ServiceTotalAmount,ChargePerItem,ChargePerItemTax,TaxID,TaxName,ActiveStatus,ServiceType"
                    PageSize="25000" OnRowDataBound="gvOtherServices_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Category" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Service Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Short Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblShortName" runat="server" Text='<%# Bind("ShortName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Service Amount" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceTotalAmount" runat="server" Text='<%# Bind("ServiceTotalAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tax" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxName" runat="server" Text='<%# Bind("TaxName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <%--  CssClass='<%# Eval("PaymentStatus").ToString() =="PAID" ? "green" :  "red" %>'--%>
                        <asp:TemplateField HeaderText="Service Type">
                            <ItemTemplate>
                                <asp:Label ID="lblOtherServiceType" runat="server" Text='<%# Eval("ServiceType").ToString()=="AB"?"Additional Boat":"Other Services" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
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
    <asp:HiddenField ID="hfservicetax" runat="server" />
    <asp:HiddenField ID="hfserviceCount" runat="server" />
    <asp:HiddenField ID="hfTaxSlab" runat="server" />


    <%--  <script type="text/javascript">

        function SelectTax(Inputval) {

            var selectedText = Inputval;
            var Taxcal = 0;

            var strarray = selectedText.split(',');

            if (strarray != 'Nil Tax') {
                for (var i = 0; i < strarray.length; i++) {
                    var Split1 = strarray[i].toString();

                    var split2 = Split1.split('-');
                    var splitGST = split2[0];
                    var splittax = split2[1];

                    Taxcal += parseFloat(splittax);

                    $("#<%=hfservicetax.ClientID%>").val(parseFloat(splittax));
                }

                $("#<%=hfserviceCount.ClientID%>").val(parseFloat(strarray.length));
                $("#<%=hfTaxSlab.ClientID%>").val(selectedText);
            }
            else {
                $("#<%=hfservicetax.ClientID%>").val(strarray);
                $("#<%=hfserviceCount.ClientID%>").val(parseFloat(strarray.length));
            }
        }
       
    </script>--%>


    <script type="text/javascript">

        function SelectTax(Inputval) {
           
            var selectedText = Inputval;


            if (selectedText == 'Select Tax') {
                $("#<%=hfTaxSlab.ClientID%>").val(selectedText);
                ServiceCharge();
            }
            if (selectedText == 'Nil Tax') {
                $("#<%=hfTaxSlab.ClientID%>").val(selectedText);
                ServiceCharge();
            }

            else {
                var Taxcal = 0;
                var strarray = selectedText.split(',');
                for (var i = 0; i < strarray.length; i++) {
                    var Split1 = strarray[i].toString();

                    var split2 = Split1.split('-');
                    var splitGST = split2[0];
                    var splittax = split2[1];

                    Taxcal += parseFloat(splittax);

                    $("#<%=hfservicetax.ClientID%>").val(Taxcal);
                }


                $("#<%=hfTaxSlab.ClientID%>").val(selectedText);
                ServiceCharge();
            }
        }


    </script>


    <script type="text/javascript">


        function TrimDecimal(number, digits) {
            var multiplier = Math.pow(10, digits),
                adjustedNum = number * multiplier,
                truncatedNum = Math[adjustedNum < 0 ? 'ceil' : 'floor'](adjustedNum);

            return truncatedNum / multiplier;
        };

        function toNumberString(num) {
            if (Number.isInteger(num)) {
                return num + ".0"
            } else {
                return num.toString();
            }
        }


        function ServiceCharge() {

            var ServiceTotal = parseFloat(document.getElementById("<%=txtTotalAmt.ClientID%>").value);
            var Taxcal = 0;
            var FinTax = 0;
            var FinalTax = 0;
            var SerTax = document.getElementById("<%=hfservicetax.ClientID%>").value;
            var per = 100;
            var Taxcal1 = parseFloat(SerTax) + parseFloat(per);

            if (isNaN(ServiceTotal)) {
                ServiceTotal = 0;
            }

            var strarray = document.getElementById("<%=hfTaxSlab.ClientID%>").value.split(',');

            if (strarray == 'Select Tax') {
                document.getElementById("<%=txtTotalAmt.ClientID%>").value = "0";
                document.getElementById("<%=txtChargePerItem.ClientID%>").value = "0";
                document.getElementById("<%=txtServiceTax.ClientID%>").value = "0";
                return;
            }
            if (strarray != 'Nil Tax') {

                for (var i = 0; i < strarray.length; i++) {
                    var Split1 = strarray[i].toString();

                    var split2 = Split1.split('-');
                    var splitGST = split2[0];
                    var splittax1 = split2[1];

                    FinTax += parseFloat(parseFloat((ServiceTotal / Taxcal1) * splittax1).toFixed(3));
                }

                FinalTax = parseFloat(FinTax);
                var ServiceTax = parseFloat(FinalTax.toFixed(3));
                //var splitTaxGSTdecimal = toNumberString(ServiceTax).split('.');
                //var GSTSplit = ((splitTaxGSTdecimal[1] == '102') ? ServiceTax.toFixed(2) : TrimDecimal(ServiceTax, 2))

                var GSTSplit = TrimDecimal(ServiceTax, 2);

                //var GSTSplit = TrimDecimal(ServiceTax, 2);
                //var GSTSplit = ServiceTax.toFixed(2);
                //var GSTSplit = parseFloat(ServiceTax.toFixed(3));
                var splitTaxGST = toNumberString(GSTSplit).split('.');

                var ides = ((splitTaxGST[1] == '01') ? parseInt('0') : parseInt(splitTaxGST[1]));
                var bal = 1;
                var btcharge = (ServiceTotal - ServiceTax);
                var fGST;

                if (ides % 2 == 0) {

                    //fGST = ((splitTaxGST[1] == '10') ? parseInt('0') : TrimDecimal(ServiceTax, 2));
                    //crct
                    fGST = TrimDecimal(ServiceTax, 2);
                    //fGST = truncator(ServiceTax, 2)
                    // fGST = (ServiceTax).toFixed(2);
                    //fGST = parseInt(ServiceTax.toFixed(2));
                    btcharge = (parseFloat(btcharge) + 0.01).toFixed(2);

                }
                else {
                    if (ides.toString().length == 1) {
                        bal = 0;
                        ides = splitTaxGST[1];
                        fGST = splitTaxGST[0] + '.' + (ides);
                        //fGST = splitTaxGST[0] + '.' + (ides);


                    } else {

                        bal = 1;
                        ides = ides + bal;
                        //fGST = splitTaxGST[0] + '.' + (ides)
                        fGST = splitTaxGST[0] + '.' + (ides);
                    }
                    btcharge = (parseFloat(btcharge + 0.01)).toFixed(2);
                }

                var after_gst = parseFloat(fGST) / 2;
                var after_boatCharges = parseFloat(ServiceTotal) - parseFloat(fGST);

                btcharge = TrimDecimal(after_boatCharges, 2);

                document.getElementById("<%=txtChargePerItem.ClientID%>").value = btcharge;
                document.getElementById("<%=txtServiceTax.ClientID%>").value = fGST;

            }
            else {
                document.getElementById("<%=txtChargePerItem.ClientID%>").value = ServiceTotal;
                document.getElementById("<%=txtServiceTax.ClientID%>").value = "0";
            }

        }
    </script>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

