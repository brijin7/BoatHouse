<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true"
    CodeFile="~/Boating/BoatRateMaster.aspx.cs" Inherits="Boating_BoatRateMaster" UICulture="hi-IN" EnableEventValidation="false" %>

<asp:Content ID="BoatRateMaster" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .panel-group .panel + .panel {
            margin-top: 5px;
        }

        .panel-group .panel {
            margin-bottom: 0;
            border-radius: 4px;
        }

        .panel-success {
            border: 1px;
            border-color: #d6e9c6;
        }

        .panel-Day {
            border: 1px;
            border-color: #e6aeae;
        }

        .panel-Express {
            border: 1px;
            border-color: #bdb0f1;
        }

        .panel {
            margin-bottom: 20px;
            background-color: #fff;
            border: 1px solid transparent;
            border-radius: 4px;
            -webkit-box-shadow: 0 1px 1px rgba(0,0,0,.05);
            box-shadow: 0 1px 1px rgba(0,0,0,.05);
            margin-left: -3px;
        }

        .panel-success > .panel-heading {
            color: #3c763d;
            background-color: #dff0d8;
            border-color: #d6e9c6;
            font-size: 14px;
            font-weight: 800;
            text-align: center;
        }

        .panel-Day > .panel-heading {
            color: #ad2828;
            background-color: #e6aeae;
            border-color: #e6aeae;
            font-size: 14px;
            font-weight: 800;
            text-align: center;
        }

        .panel-Express > .panel-heading {
            color: #2334ad;
            background-color: #bdb0f1;
            border-color: #bdb0f1;
            font-size: 14px;
            font-weight: 800;
            text-align: center;
        }

        .panel-group .panel-heading {
            border-bottom: 0;
        }

        .panel-heading {
            padding: 5px 5px;
            border-bottom: 1px solid transparent;
            border-top-left-radius: 3px;
            border-top-right-radius: 3px;
        }

        .panel-body {
            padding: 10px;
            border: 1px solid #d6e9c6;
        }

        .lblChrg {
            padding: 10px 10px;
        }

        @media (min-width: 576px) {
            .modal-dialog {
                max-width: 700px !important; /* New width for small modal */
            }
        }
    </style>

    <script>

        $(document).ready(function () {
            $('#<%=txtPercentage.ClientID%>').keyup(function () {
                var a = $('#<%=txtPercentage.ClientID%>').val();
                document.getElementById("<%=txtFixedAmount.ClientID%>").value = "0";
                if (a > 100) {
                    alert('Enter Below 100 Percentage');
                    $('#<%=txtPercentage.ClientID%>').val('');
                    $('#<%=txtPercentage.ClientID%>').focus();
                }
                if (a > 0) {
                    if (document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked == true);
                    {
                        document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                        document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                        document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                        document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                        document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                        document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                        document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;

                    }
                }
            });
        });


        $(document).ready(function () {

            $('#<%=txtFixedAmount.ClientID%>').keyup(function () {
                var a = $('#<%=txtFixedAmount.ClientID%>').val();
                document.getElementById("<%=txtPercentage.ClientID%>").value = "0";
                if (a > 0) {


                    if (document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked == true);
                    {
                        document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                        document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                        document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                        document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                        document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                        document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                        document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                        document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;

                    }
                }
            });
        });

        $('#<%=txtTripPerday.ClientID%>').change(function () {

            var a = $('#<%=txtTripPerday.ClientID%>').val();

                   if (a > 100) {
                       alert('Enter Trips Per day Below 100');
                       $('#<%=txtTripPerday.ClientID%>').val('');
                    $('#<%=txtTripPerday.ClientID%>').focus();
                   }
               });

        $(function () {
            var fileupload = $('#<%=fupBtRtLink.ClientID%>');
                var image = $('#divImgPreview');
                image.click(function () {
                    fileupload.click();
                });
            });
    </script>


    <script>
        function ShowImagePreview(input) {
            var fup = document.getElementById("<%=fupBtRtLink.ClientID %>");
            var fileName = fup.value;
            var maxfilesize = 1024 * 1024;
            filesize = input.files[0].size;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            if (ext == "gif" || ext == "GIF" || ext == "PNG" || ext == "png" || ext == "jpg" || ext == "JPG" || ext == "bmp" || ext == "BMP" || ext == "jpeg" || ext == "JPEG" || ext == "svg" || ext == "SVG") {
                if (filesize <= maxfilesize) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#<%=imgBtRtPrev.ClientID%>').prop('src', e.target.result);
                            $('#<%=hfImageCheckValue.ClientID%>').val("1");
                        };
                        reader.readAsDataURL(input.files[0]);
                    }
                }
                else {
                    swal("Please, Upload image file less than or equal to 1 MB !!!");
                    fup.focus();
                    return false;
                }
            }
            else {
                swal("Please, Upload Gif, Jpg, Jpeg, Svg or Bmp Images only !!!");
                fup.focus();
                return false;
            }
        }

        $(document).ready(function () {
            $(".decimal").keypress(function (event) {
                return isNumber(event, this);
            });

        });


        function allowNumbersOnly(e) {
            if (e.key >= 0 && e.key <= 9) {
                return true;
            }
            return false;
        }


        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            let charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
                evt.preventDefault();
            } else {
                return true;
            }
        }
        function minmax(value, min, max) {
            if (parseInt(value) < min || isNaN(parseInt(value)))
                return min;
            else if (parseInt(value) > max)
                return max;
            else return value;
        }
        function minmaxtrips(value, min, max) {
            if (parseInt(value) < min || isNaN(parseInt(value)))
                return min;
            else if (parseInt(value) > max) {
                alert("Maximum Trips Per/Day :" + " " + max);
                return max;
            }
            else return value;
        }

    </script>

    <div class="form-body col-sm-12 col-xs-12">
        <h5 class="pghr">Boat Rate Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <br />
        <span style="float: right">
            <asp:ImageButton ID="btnExportToPdf" runat="server" ImageUrl="~/images/pdf.png" OnClick="btnExportToPdf_Click" Width="35px" TabIndex="3" Visible="true" />
        </span>
        <div id="divEntry" runat="server" visible="false">

            <div class="row">
                <div class="col-sm-9 col-xs-12">
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-sm-3 col-xs-12" runat="server" visible="false">
                                <div class="form-group">
                                    <label for="lblBoatHouse"><i class="fa fa-ship" aria-hidden="true"></i>Boat House</label>
                                    <asp:DropDownList ID="ddlBoatHouse" CssClass="form-control inputboxstyle" runat="server"
                                        TabIndex="1" AutoPostBack="true">
                                        <asp:ListItem Value="0">Select Boat House</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvddlBoathouse" runat="server" ControlToValidate="ddlBoatHouse"
                                        ValidationGroup="BoatRate" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Boat House</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label for="lblBoatType" id="lblboattype"><i class="fa fa-ship" aria-hidden="true"></i>Boat Type <span class="spStar">*</span></label>
                                <asp:DropDownList ID="ddlBoatType" CssClass="form-control inputboxstyle" runat="server"
                                    TabIndex="2" OnSelectedIndexChanged="ddlBoatType_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlBoatType" runat="server" ControlToValidate="ddlBoatType"
                                    ValidationGroup="BoatRate" SetFocusOnError="True" InitialValue="Select Boat Type" CssClass="vError">Select Boat Type</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label for="lblBoatSeatId" id="lbl1"><i class="fas fa-chair"></i>Boat Seater <span class="spStar">*</span></label>
                                <asp:DropDownList ID="ddlBoatSeatId" CssClass="form-control inputboxstyle" runat="server"
                                    TabIndex="3">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlBoatSeatId" runat="server" ControlToValidate="ddlBoatSeatId"
                                    ValidationGroup="BoatRate" SetFocusOnError="True" InitialValue="Select Boat Seater" CssClass="vError">Select Boat Seater</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label for="lblSelfDrive" id="lblselfdrive"><i class="fa fa-car" aria-hidden="true"></i>Self Drive</label>
                                <asp:RadioButtonList ID="rblSelfdrive" runat="server" RepeatDirection="Horizontal" TabIndex="4" CssClass="rbl">
                                    <asp:ListItem Value="A">Allowed</asp:ListItem>
                                    <asp:ListItem Value="N" Selected="true">Not Allowed</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                            <div class="col-sm-3 col-xs-12" id="div1" runat="server">
                                <label for="lblDisOrder">
                                    <i class="fa fa-sort" aria-hidden="true"></i>
                                    Display Order <span class="spStar">*</span>
                                </label>
                                <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="form-control" onkeypress="return isNumber(event)"
                                    AutoComplete="Off" MaxLength="4"
                                    TabIndex="5">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtDisplayOrder" runat="server" ControlToValidate="txtDisplayOrder"
                                    ValidationGroup="BoatRate" SetFocusOnError="True" InitialValue="0" CssClass="vError">Enter Display Order</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label for="lblTime" id="lbltime"><i class="fas fa-flag-checkered"></i>Time Extension</label>
                                <asp:RadioButtonList ID="rblTimeExtension" runat="server" RepeatDirection="Horizontal" TabIndex="6" CssClass="rbl">
                                    <asp:ListItem Value="A">Allowed</asp:ListItem>
                                    <asp:ListItem Value="N" Selected="true">Not Allowed</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label for="txttripPerday"><i class="fa fa-ship" aria-hidden="true"></i>Maximum Trips / Day <span class="spStar">*</span></label>
                                <asp:TextBox ID="txtTripPerday" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="3" TabIndex="7" onkeypress="return isNumber(event)" onkeyup="this.value = minmaxtrips(this.value, 0, 300)">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtTripPerday" runat="server" ControlToValidate="txtTripPerday"
                                    ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Maximum Trips</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label for="lblmintime" id="lblmintime" runat="server">
                                    <i class="fas fa-clock"
                                        aria-hidden="true"></i>Duration (Min) <span class="spStar">*</span></label>
                                <div id="mintime" runat="server">
                                    <asp:DropDownList ID="dlstDurationMin" CssClass="form-control inputboxstyle" runat="server" TabIndex="8"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvdlstDurationMin" runat="server" ControlToValidate="dlstDurationMin"
                                        ValidationGroup="BoatRate" SetFocusOnError="True" InitialValue="Select Duration" CssClass="vError">Select Duration (Min)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label for="lblgracetime" id="lblgracetime" runat="server">
                                    <i class="fas fa-stopwatch"
                                        aria-hidden="true"></i>Grace Time (Min) <span class="spStar">*</span></label>
                                <div id="gracetime" runat="server">
                                    <asp:DropDownList ID="dlstGraceTimeMin" CssClass="form-control inputboxstyle" runat="server" TabIndex="9"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvdlstGraceTimeMin" runat="server" ControlToValidate="dlstGraceTimeMin"
                                        ValidationGroup="BoatRate" SetFocusOnError="True" InitialValue="Select Grace Time" CssClass="vError">Select Grace Time (Min)</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label for="lbldepositty" id="lbldepositty"><i class="fas fa-money-check" aria-hidden="true"></i>Deposit Type</label>
                                <asp:RadioButtonList ID="rblDepositType" runat="server" RepeatDirection="Horizontal" TabIndex="10" CssClass="rbl">
                                    <asp:ListItem Value="P" Selected="True">Percentage</asp:ListItem>
                                    <asp:ListItem Value="F">Fixed</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                            <div class="col-sm-3 col-xs-12" id="divPercentage" runat="server">
                                <label for="txtaddDep">
                                    <i class="fas fa-money-check" aria-hidden="true"></i>
                                    Percentage(<i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                                </label>
                                <asp:TextBox ID="txtPercentage" runat="server" CssClass="form-control decimal" min="0" AutoComplete="Off" MaxLength="3"
                                    TabIndex="11" onkeyup="this.value = minmax(this.value, 0, 100); Amount();">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtPercentage" runat="server" ControlToValidate="txtPercentage"
                                    ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Percentage</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-sm-3 col-xs-12" id="divFixedAmount" runat="server" style="display: none">
                                <label for="txtaddDep">
                                    <i class="fas fa-money-check" aria-hidden="true"></i>
                                    Fixed Amount(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                </label>
                                <asp:TextBox ID="txtFixedAmount" runat="server" CssClass="form-control decimal" AutoComplete="Off" MaxLength="4"
                                    TabIndex="12" onkeyup="Amount();">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtFixedAmount" runat="server" ControlToValidate="txtFixedAmount"
                                    ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Fixed Amount</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                            </div>
                            <div class="col-sm-3 col-xs-12">
                            </div>
                            <div class="col-sm-3 col-xs-12" runat="server">
                                <div class="form-group">
                                    <label runat="server" id="Label7">
                                        <i class="fas fa-baby"></i>
                                        <asp:CheckBox ID="ChkChildApplicable" runat="server" TabIndex="13" />
                                        &nbsp Child Applicable
                                    </label>
                                    <div id="divChildInfantAge" runat="server" style="display: none">
                                        <asp:TextBox ID="txtChildApp" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                            MaxLength="5" Font-Size="14px" TabIndex="14" onkeypress="return isNumber(event)" placeholder="No. of Child Applicable">
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12" runat="server" id="divChild" style="display: none">
                                <div class="form-group">
                                    <label runat="server" id="Label1">
                                        <i class="fas fa-baby"></i>Charge/Child (Incl. Tax) <span class="spStar">*</span>
                                    </label>
                                    <asp:TextBox ID="txtChildCharge" runat="server" CssClass="form-control calculateChild" AutoComplete="Off"
                                        MaxLength="5" Font-Size="14px" TabIndex="15" onkeypress="return isNumber(event)" placeholder="Charge per Child"
                                        AutoPostBack="false">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtChildCharge" runat="server" ControlToValidate="txtChildCharge"
                                        ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Charge per Child</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12" runat="server" id="divActChrg" style="display: none">
                                <div class="form-group">
                                    <label runat="server" id="Label2">
                                        <i class="fas fa-baby"></i>Actual Child Charge
                                    </label>
                                    <asp:TextBox ID="txtActChildChrg" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                        MaxLength="5" Font-Size="14px" TabIndex="15" onkeypress="return isNumber(event)">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12" runat="server" id="divChildTax" style="display: none">
                                <div class="form-group">
                                    <label runat="server" id="Label3">
                                        <i class="fas fa-baby"></i>Tax(<asp:Label ID="lblChildTax" runat="server"></asp:Label>
                                        <i class="fas fa-percentage p-0"></i>)
                                    </label>
                                    <asp:TextBox ID="txtChildTaxAmt" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                        MaxLength="5" Font-Size="14px" TabIndex="15" onkeypress="return isNumber(event)">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-2 col-xs-12">
                    <div class="panel panel-success">
                        <div class="panel-heading"><i class="fa fa-ship" aria-hidden="true"></i>Boat Image</div>
                        <div class="panel-body">
                            <div class="col-sm-12 p-0">
                                <asp:FileUpload ID="fupBtRtLink" runat="server" TabIndex="16" Style="display: none" onchange="ShowImagePreview(this);" />
                                <div class="divImg" id="divImgPreview" style="margin: auto;">
                                    <asp:Image ID="imgBtRtPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                    <div class="imageOverlay">Click To Upload</div>
                                </div>
                                <span style="color: red; font-size: 12px;">Upload Image type of .png, .jpg, .svg upto 1 MB size.</span>
                            </div>
                        </div>
                    </div>
                    <div id="divExtn" runat="server" style="padding-top: 5px;">
                        <asp:LinkButton ID="lbtnExt" runat="server" CssClass="btn btn-info"
                            data-toggle="modal" data-target="#myModal">Add Extension Charge</asp:LinkButton>
                    </div>

                </div>
            </div>
            <div class="row">

                <div class="col-sm-9 col-xs-12">
                    <div class="row col-sm-3 col-sm-12" style="padding-top: 10px;">
                        <h5 class="pghr">Tariff Per Boat<span style="float: right;"></span> </h5>
                    </div>
                    <div class="row m-0" style="padding: 10px; border: 1px solid #dfdfdf; width: 1027px;">

                        <div class="col-sm-0 p-0 mb-0">
                            <div class="col-sm-12" style="padding: 70px 15px 65px 15px">
                                <asp:Label ID="lblBtCharge" runat="server"><b>Boat</b></asp:Label>
                            </div>

                            <div class="col-sm-12" style="padding: 0px 0px 47px 5px;">
                                <asp:Label ID="lblRwCharge" runat="server"><b>Rower</b></asp:Label>
                            </div>
                        </div>

                        <div class="p-0 mb-0" style="width: 314px; height: 250px;">
                            <div class="panel panel-Day">
                                <div class="panel-heading">Week Day Tariff </div>
                                <div class="panel-body">

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblmaxcharge" style="font-size: 11px;">
                                                <i class="fas fa-money-check-alt"></i>
                                                Amount(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtBoatMinCharge" runat="server" CssClass="form-control calculateNormal" AutoComplete="Off" MaxLength="10" TabIndex="17"
                                                onkeypress="return isNumber(event)" AutoPostBack="false">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtBoatMinCharge" runat="server" ControlToValidate="txtBoatMinCharge"
                                                ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Boat Charge(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtBtNorCharges" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="10">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Payment(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtRowerMinCharge" runat="server" AutoComplete="Off" MaxLength="10" TabIndex="18" Text="0"
                                                CssClass="form-control calculateNormal" onkeypress="return isNumber(event)" AutoPostBack="false">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtRowerMinCharge" runat="server" ControlToValidate="txtRowerMinCharge"
                                                ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Total Tax(<asp:Label ID="lblTaxBtNor" runat="server"></asp:Label>
                                                <i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtTaxBtNorChrg" runat="server" AutoComplete="Off" CssClass="form-control" MaxLength="10">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>

                        <div class=" p-0 mb-0" style="width: 314px; height: 250px;">
                            <div class="panel panel-success">
                                <div class="panel-heading">Week End Tariff </div>
                                <div class="panel-body">

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblmaxcharge" style="font-size: 11px;">
                                                <i class="fas fa-money-check-alt"></i>
                                                Amount(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtWkEdAmt" runat="server" CssClass="form-control calculateNormal" AutoComplete="Off" MaxLength="10" TabIndex="19"
                                                onkeypress="return isNumber(event)" AutoPostBack="false">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtWkEdAmt" runat="server" ControlToValidate="txtWkEdAmt"
                                                ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Boat Charge(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtWkEdChrge" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="10">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Payment(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtWkEdPamt" runat="server" AutoComplete="Off" MaxLength="10" TabIndex="20"
                                                CssClass="form-control calculateNormal" onkeypress="return isNumber(event)" AutoPostBack="false">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtWkEdPamt" runat="server" ControlToValidate="txtWkEdPamt"
                                                ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Total Tax(<asp:Label ID="Label6" runat="server"></asp:Label>
                                                <i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="TxtWkEdToTax" runat="server" AutoComplete="Off" CssClass="form-control" MaxLength="10">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class=" p-0 mb-0" style="width: 314px; height: 250px;">
                            <div class="panel panel-Express">
                                <div class="panel-heading">Express Tariff </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Amount(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtBoatPrmMin" runat="server" CssClass="form-control calculatePremium" AutoComplete="Off" MaxLength="10"
                                                onkeypress="return isNumber(event)" TabIndex="21" AutoPostBack="false">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtBoatPrmMin" runat="server" ControlToValidate="txtBoatPrmMin"
                                                ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Boat Charge(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtBtPremCharge" runat="server" AutoComplete="Off" CssClass="form-control"
                                                MaxLength="10">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Payment(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtRowerPrmMin" runat="server" CssClass="form-control calculatePremium" AutoComplete="Off" MaxLength="10"
                                                onkeypress="return isNumber(event)" TabIndex="22" AutoPostBack="false" Text="0">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtRowerPrmMin" runat="server" ControlToValidate="txtRowerPrmMin"
                                                ValidationGroup="BoatRate" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-6 col-xs-10">
                                            <label for="lblrowermincharge" style="font-size: 11px;">
                                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                Total Tax(<asp:Label ID="lblTaxBtPrem" runat="server"></asp:Label>
                                                <i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                                            </label>
                                            <asp:TextBox ID="txtTaxBtPremChrg" runat="server" AutoComplete="Off" CssClass="form-control" MaxLength="10">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-3 col-sm-1" runat="server" style="margin-top: 10px">

                            <label runat="server" id="Label8">
                                <i class="fa fa-ticket"></i>
                                <asp:CheckBox ID="ChkSingleAllwd" runat="server" TabIndex="23" />
                                &nbsp Single Allowed
                            </label>

                        </div>

                        <div class=" row col-sm-3 col-sm-12" id="lblsa" runat="server" style="display: none">
                            <h5 class="pghr">Tariff Per Ticket<span style="float: right;"></span> </h5>
                            <%-- <asp:Label ID="lblTicket" runat="server" Font-Bold="true" CssClass="pghr">Tariff Per Ticket</asp:Label>--%>
                        </div>

                        <div class="row m-0" id="divSinAolwed" runat="server" style="padding: 0px; width: 950px;">

                            <div class="col-sm-0 p-0 mb-0" id="divSAcharge" runat="server" style="display: none">
                                <div class="col-sm-12" style="padding: 70px 15px 65px 15px;">
                                    <asp:Label ID="lblSACharge" runat="server"><b>Boat</b></asp:Label>
                                </div>

                                <div class="col-sm-12" style="padding: 0px 0px 47px 5px;">
                                    <asp:Label ID="lblSARwCharge" runat="server"><b>Rower</b></asp:Label>
                                </div>
                            </div>

                            <div class="col-sm-4 p-0 mb-0" id="DivSAWeekday" runat="server" style="width: 300px !important; height: 184px !important;">
                                <div class="panel panel-Day">
                                    <div class="panel-heading">Week Day Tariff </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-sm-6 col-xs-10">
                                                <label for="lblmaxcharge" style="font-size: 11px;">
                                                    <i class="fas fa-money-check-alt"></i>
                                                    Amount(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                                </label>
                                                <asp:TextBox ID="txtIWDAmt" runat="server" CssClass="form-control calculateNormal" AutoComplete="Off" MaxLength="10" TabIndex="24"
                                                    onkeypress="return isNumber(event)" AutoPostBack="false">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtIWDAmt"
                                                    ValidationGroup="BoatRates" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-xs-10">
                                                <label for="lblrowermincharge" style="font-size: 11px;">
                                                    <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                    Boat Charge(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                                </label>
                                                <asp:TextBox ID="txtIWDBtChrg" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="10">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-xs-10">
                                                <label for="lblrowermincharge" style="font-size: 11px;">
                                                    <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                    Payment(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                                </label>
                                                <asp:TextBox ID="txtIWdPaymt" runat="server" AutoComplete="Off" MaxLength="10" TabIndex="25"
                                                    CssClass="form-control calculateNormal" onkeypress="return isNumber(event)" AutoPostBack="false">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtIWdPaymt"
                                                    ValidationGroup="BoatRates" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-xs-10">
                                                <label for="lblrowermincharge" style="font-size: 11px;">
                                                    <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                    Total Tax(<asp:Label ID="Label9" runat="server"></asp:Label>
                                                    <i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                                                </label>
                                                <asp:TextBox ID="txtIWdToTax" runat="server" AutoComplete="Off" CssClass="form-control" MaxLength="10">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4 p-0 mb-0" id="DivSAWeekEnd" runat="server" style="width: 300px !important; height: 184px !important;">
                                <div class="panel panel-success">
                                    <div class="panel-heading">Week End Tariff </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-sm-6 col-xs-10">
                                                <label for="lblmaxcharge" style="font-size: 11px;">
                                                    <i class="fas fa-money-check-alt"></i>
                                                    Amount(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                                </label>
                                                <asp:TextBox ID="txtIWEAmt" runat="server" CssClass="form-control calculateNormal" AutoComplete="Off" MaxLength="10" TabIndex="26"
                                                    onkeypress="return isNumber(event)" AutoPostBack="false">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtIWEAmt"
                                                    ValidationGroup="BoatRates" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-xs-10">
                                                <label for="lblrowermincharge" style="font-size: 11px;">
                                                    <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                    Boat Charge(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                                </label>
                                                <asp:TextBox ID="txtIWEBtChrg" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="10">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-xs-10">
                                                <label for="lblrowermincharge" style="font-size: 11px;">
                                                    <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                    Payment(<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                                                </label>
                                                <asp:TextBox ID="txtIWEPaymt" runat="server" AutoComplete="Off" MaxLength="10" TabIndex="18"  
                                                    CssClass="form-control calculateNormal" onkeypress="return isNumber(event)" AutoPostBack="false">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtIWEPaymt"
                                                    ValidationGroup="BoatRates" SetFocusOnError="True" CssClass="vError">Enter Minimum Charge</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-xs-10">
                                                <label for="lblrowermincharge" style="font-size: 11px;">
                                                    <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                                    Total Tax(<asp:Label ID="Label10" runat="server"></asp:Label>
                                                    <i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                                                </label>
                                                <asp:TextBox ID="txtIWEToTax" runat="server" AutoComplete="Off" CssClass="form-control" MaxLength="10">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>



                        </div>

                        <div class="col-sm-11" id="divExtnGrid" runat="server" style="margin-top: 10px">
                            <div class="table-responsive" style="overflow-y: visible; height: 135px; max-height: 500px; padding: 0px 0px 0px 42px;">
                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                                <asp:GridView ID="gvExtn" runat="server" AllowPaging="True"
                                    CssClass="CustomGrid table table-bordered table-condenced"
                                    AutoGenerateColumns="False" DataKeyNames="UniqueId">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Boat Type " HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatTypeId" runat="server" Visible="false" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                                <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatTypeName") %>'></asp:Label>
                                                <asp:Label ID="lblBoatSeaterId" runat="server" Visible="false" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                                <asp:Label ID="lblBoatSeaterName" runat="server" Text='<%# Bind("BoatSeaterName") %>'></asp:Label>
                                                <asp:Label ID="lblExtensionType" runat="server" Text='<%# Bind("ExtensionType") %>'></asp:Label>
                                                <asp:Label ID="lblAmtPer" runat="server" Text='<%# Bind("AmtPer") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Extn Type" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExtensionTypeName" runat="server" Text='<%# Bind("ExtensionTypeName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Time" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExtFromTime" runat="server" Text='<%# Bind("ExtFromTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Time" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExtToTime" runat="server" Text='<%# Bind("ExtToTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount Type" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmtType" runat="server" Text='<%# Eval("AmtType").ToString() == "P" ? "(%)" : "(₹)" %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boat Extn" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatExtnTotAmt" runat="server" Text='<%# Bind("BoatExtnTotAmt") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rower Extn" HeaderStyle-CssClass="grdHead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowerExtnCharge" runat="server" Text='<%# Bind("RowerExtnCharge") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Boat Charge" HeaderStyle-CssClass="grdhead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatExtnCharge" runat="server" Text='<%# Bind("BoatExtnCharge") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax" HeaderStyle-CssClass="grdhead">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatExtnTaxAmt" runat="server" Text='<%# Bind("BoatExtnTaxAmt") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Boat House" HeaderStyle-CssClass="grdHead" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoatHouseId" runat="server" Visible="false" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
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

                <div class="col-sm-2 col-xs-12" style="padding-left: 40px; padding-top: 20px;">
                </div>
            </div>

            <div class="col-sm-12 col-xs-12 ">
                <div class="form-submit" style="padding-right: 23%;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="BoatRate" TabIndex="27" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="28" OnClick="btnCancel_Click" />
                </div>
            </div>

        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <asp:GridView ID="gvBoatRate" runat="server" AllowPaging="True" CssClass="gvv display table table-bordered table-condenced"
                    AutoGenerateColumns="False" DataKeyNames="BoatTypeId" PageSize="25000" OnRowDataBound="gvBoatRate_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Display Order" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDisplayOrder" runat="server" Text='<%# Bind("DisplayOrder") %>'></asp:Label>
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

                        <asp:TemplateField HeaderText="Duration (Mins)" HeaderStyle-CssClass="grdHead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatDur" runat="server" Text='<%# Bind("BoatMinTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Grace Time (Mins)" HeaderStyle-CssClass="grdhead" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblboatgracetime" runat="server" Text='<%# Bind("BoatGraceTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat House" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatHouseId" runat="server" Visible="false" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                                <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Deposit Type" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDepositType" runat="server" Text='<%# Bind("DepositType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Week Day Tariff" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatMinTotAmt" runat="server" Text='<%# Bind("BoatMinTotAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Week End Tariff" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblWEBoatMinTotAmt" runat="server" Text='<%# Bind("WEBoatMinTotAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Express Tariff" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatPremTotAmt" runat="server" Text='<%# Bind("BoatPremTotAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText=" Individual Week Day Tariff" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblIWDBoatMinTotAmt" runat="server" Text='<%# Bind("IWDBoatMinTotAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Individual Week End Tariff" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblIWEBoatMinTotAmt" runat="server" Text='<%# Bind("IWEBoatMinTotAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Deposit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lbldeposit" runat="server" Text='<%# Bind("Deposit") %>'></asp:Label>
                                <asp:Label ID="lblDepositTypeName" runat="server" Text='<%# Eval("DepositTypeName").ToString() == "Percentage" ? "(%)" : "(₹)" %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Self Drive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblSelfDrive" runat="server" Text='<%# Bind("SelfDrive") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time Extension" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblTimeExtension" runat="server" Text='<%# Bind("TimeExtension") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="boat min time" HeaderStyle-CssClass="grdhead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatMinTime" runat="server" Text='<%# Bind("BoatMinTime") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="boat min charge" HeaderStyle-CssClass="grdhead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatMinCharge" runat="server" Text='<%# Bind("BoatMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblRowerMinCharge" runat="server" Text='<%# Bind("RowerMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblBoatMinTaxAmt" runat="server" Text='<%# Bind("BoatMinTaxAmt") %>'></asp:Label>
                                <%--    <asp:Label ID="lblDisplayOrder" runat="server" Text='<%# Bind("DisplayOrder") %>'></asp:Label>--%>
                                <asp:Label ID="lblBoatPremMinCharge" runat="server" Text='<%# Bind("BoatPremMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblRowerPremMinCharge" runat="server" Text='<%# Bind("RowerPremMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblBoatPremTaxAmt" runat="server" Text='<%# Bind("BoatPremTaxAmt") %>'></asp:Label>

                                <asp:Label ID="lblPerHeadApplicable" runat="server" Text='<%# Bind("PerHeadApplicable") %>'></asp:Label>
                                <%--  <asp:Label ID="lblWEBoatMinTotAmt" runat="server" Text='<%# Bind("WEBoatMinTotAmt") %>'></asp:Label>--%>
                                <asp:Label ID="lblWEBoatMinCharge" runat="server" Text='<%# Bind("WEBoatMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblWERowerMinCharge" runat="server" Text='<%# Bind("WERowerMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblWEBoatMinTaxAmt" runat="server" Text='<%# Bind("WEBoatMinTaxAmt") %>'></asp:Label>

                                <%-- <asp:Label ID="lblIWDBoatMinTotAmt" runat="server" Text='<%# Bind("IWDBoatMinTotAmt") %>'></asp:Label>--%>
                                <asp:Label ID="lblIWDBoatMinCharge" runat="server" Text='<%# Bind("IWDBoatMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblIWDRowerMinCharge" runat="server" Text='<%# Bind("IWDRowerMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblIWDBoatMinTaxAmt" runat="server" Text='<%# Bind("IWDBoatMinTaxAmt") %>'></asp:Label>

                                <%--  <asp:Label ID="lblIWEBoatMinTotAmt" runat="server" Text='<%# Bind("IWEBoatMinTotAmt") %>'></asp:Label>--%>
                                <asp:Label ID="lblIWEBoatMinCharge" runat="server" Text='<%# Bind("IWEBoatMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblIWERowerMinCharge" runat="server" Text='<%# Bind("IWERowerMinCharge") %>'></asp:Label>
                                <asp:Label ID="lblIWEBoatMinTaxAmt" runat="server" Text='<%# Bind("IWEBoatMinTaxAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Max Trips/Day" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblmaxTripsPerDay" runat="server" Text='<%# Bind("MaxTripsPerDay") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Boat Image" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBoatImageLink" runat="server" Text='<%# Bind("BoatImageLink") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active Status" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ChildApplicable" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblChildApplicable" runat="server" Text='<%# Bind("ChildApplicable") %>'></asp:Label>
                                <asp:Label ID="lblNoofChildApplicable" runat="server" Text='<%# Bind("NoofChildApplicable") %>'></asp:Label>
                                <asp:Label ID="lblChargePerChild" runat="server" Text='<%# Bind("ChargePerChild") %>'></asp:Label>
                                <asp:Label ID="lblChargePerChildTotAmt" runat="server" Text='<%# Bind("ChargePerChildTotAmt") %>'></asp:Label>
                                <asp:Label ID="lblChargePerChildTaxAmt" runat="server" Text='<%# Bind("ChargePerChildTaxAmt") %>'></asp:Label>
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
    <%-- Modal Pop Up --%>

    <div class="modal" id="myModal">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header" style="background-color: #004c8c; color: white">
                    <h4 class="modal-title">Extension Charges</h4>
                    <asp:LinkButton ID="btnPopupClose" runat="server" CssClass="close" OnClick="btnClose_Click" ForeColor="White">&times;</asp:LinkButton>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label for="lblboatid">
                                    Extension Type
                                </label>
                                <asp:DropDownList ID="ddlExtensionType" CssClass="form-control inputboxstyle" runat="server" TabIndex="27">
                                    <%--<asp:ListItem Value="0">Select Extension Type</asp:ListItem>--%>
                                    <asp:ListItem Value="WD">Week Day Tariff</asp:ListItem>
                                    <asp:ListItem Value="WE">Week End Tariff</asp:ListItem>
                                    <asp:ListItem Value="EC">Express Tariff</asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlExtensionType" InitialValue="0"
                                    ValidationGroup="Extension" SetFocusOnError="True" CssClass="vError">Select Extension Type</asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label for="lblboatid">
                                    Extension From Time (Min)
                                </label>
                                <asp:DropDownList ID="ddlExtFromTime" CssClass="form-control inputboxstyle"
                                    runat="server" TabIndex="28">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlExtFromTime"
                                    ValidationGroup="Extension" SetFocusOnError="True" InitialValue="Select From Time (Min)"
                                    CssClass="vError">Select From Time (Min)</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label for="lblboatid">
                                    Extension To Time (Min)
                                </label>
                                <asp:DropDownList ID="ddlExtToTime" CssClass="form-control inputboxstyle" runat="server"
                                    TabIndex="29">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlExtToTime"
                                    ValidationGroup="Extension" SetFocusOnError="True" InitialValue="Select To Time (Min)"
                                    CssClass="vError">Select To Time (Min)</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label for="lblboatid">
                                    Amount Type <span class="spStar">*</span>
                                </label>
                                <asp:RadioButtonList ID="rblAmountType" runat="server" RepeatDirection="Horizontal" TabIndex="30" CssClass="rbl">
                                    <asp:ListItem Value="P" Selected="True">Percentage</asp:ListItem>
                                    <asp:ListItem Value="F">Fixed</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="rblAmountType"
                                    ValidationGroup="Extension" SetFocusOnError="True" CssClass="vError">Select Amount Type</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12" id="divAmtPer" runat="server">
                            <label for="lblmaxcharge">
                                <i class="fas fa-money-check-alt"></i>
                                Percentage (<i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtAmtPer" runat="server" CssClass="form-control" AutoComplete="Off"
                                onkeyup="this.value = minmax(this.value, 0, 100)" MaxLength="10" TabIndex="31">
                            </asp:TextBox>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                        </div>
                        <div class="col-sm-6 col-xs-12" id="divbtAmt" runat="server">
                            <label for="lblmaxcharge">
                                <i class="fas fa-money-check-alt"></i>
                                Boat Amount (<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtBtAmt" runat="server" CssClass="form-control" AutoComplete="Off" ForeColor="RosyBrown" Font-Bold="true"
                                onkeypress="return isNumber(event)" MaxLength="10" TabIndex="32" Enabled="false">
                            </asp:TextBox>
                        </div>
                        <div class="col-sm-6 col-xs-12" id="divrwAmt" runat="server">
                            <label for="lblmaxcharge">
                                <i class="fas fa-money-check-alt"></i>
                                Rower Amount (<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtRwAmt" runat="server" CssClass="form-control" AutoComplete="Off" ForeColor="RosyBrown" Font-Bold="true"
                                onkeypress="return isNumber(event)" MaxLength="10" TabIndex="33" Enabled="false">
                            </asp:TextBox>
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <label for="lblmaxcharge">
                                <i class="fas fa-money-check-alt"></i>
                                Boat Extension (<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtBoatExtnCharge" runat="server" CssClass="form-control" AutoComplete="Off"
                                onkeypress="return isNumber(event)" MaxLength="10" TabIndex="34">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtBoatExtnCharge"
                                ValidationGroup="Extension" SetFocusOnError="True" CssClass="vError">Enter Extension Charge</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <label for="lblrowermincharge">
                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                Boat Charge (<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtBtNorExtCharges" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="10">
                            </asp:TextBox>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12">
                            <label for="lblrowerexterncharge">
                                <i class="fas fa-money-check-alt"></i>
                                Rower Extension (<i class="fas fa-rupee-sign p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtRowerExtnCharge" runat="server" CssClass="form-control" AutoComplete="Off"
                                onkeypress="return isNumber(event)" MaxLength="10" TabIndex="35">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtRowerExtnCharge"
                                ValidationGroup="Extension" SetFocusOnError="True" CssClass="vError">Enter Extension Charge</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <label for="lblrowermincharge">
                                <i class="fa fa-money-bill-alt" aria-hidden="true"></i>
                                Tax (<asp:Label ID="lblTaxBtExt" runat="server"></asp:Label>
                                <i class="fas fa-percentage p-0"></i>) <span class="spStar">*</span>
                            </label>
                            <asp:TextBox ID="txtTaxBtExtChrg" runat="server" AutoComplete="Off" CssClass="form-control" MaxLength="10">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="form-submit text-center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" ValidationGroup="Extension" TabIndex="36"
                            CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                    </div>

                    <div id="divModalGrid" runat="server">
                        <div class="table-responsive" style="overflow-y: visible; height: 135px; max-height: 500px;">
                            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                <asp:Label ID="Label4" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </div>
                            <asp:GridView ID="gvBoatRateExtnChrg" runat="server" AllowPaging="True"
                                CssClass="CustomGrid table table-bordered table-condenced"
                                AutoGenerateColumns="False" DataKeyNames="UniqueId" OnRowDataBound="gvBoatRateExtnChrg_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Boat Type " HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatTypeId" runat="server" Visible="false" Text='<%# Bind("BoatTypeId") %>'></asp:Label>
                                            <asp:Label ID="lblBoatTypeName" runat="server" Text='<%# Bind("BoatTypeName") %>'></asp:Label>
                                            <asp:Label ID="lblBoatSeaterId" runat="server" Visible="false" Text='<%# Bind("BoatSeaterId") %>'></asp:Label>
                                            <asp:Label ID="lblBoatSeaterName" runat="server" Text='<%# Bind("BoatSeaterName") %>'></asp:Label>
                                            <asp:Label ID="lblExtensionType" runat="server" Text='<%# Bind("ExtensionType") %>'></asp:Label>
                                            <asp:Label ID="lblAmtPer" runat="server" Text='<%# Bind("AmtPer") %>'></asp:Label>
                                            <asp:Label ID="lblActiveStatus" runat="server" Text='<%# Bind("ActiveStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Extn Type" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExtensionTypeName" runat="server" Text='<%# Bind("ExtensionTypeName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExtFromTime" runat="server" Text='<%# Bind("ExtFromTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Time" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExtToTime" runat="server" Text='<%# Bind("ExtToTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount Type" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmtType" runat="server" Text='<%# Bind("AmtType") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblAmtTypeName" runat="server" Text='<%# Eval("AmtType").ToString() == "P" ? "(%)" : "(₹)" %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat Extn" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatExtnTotAmt" runat="server" Text='<%# Bind("BoatExtnTotAmt") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rower Extn" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowerExtnCharge" runat="server" Text='<%# Bind("RowerExtnCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Boat Charge" HeaderStyle-CssClass="grdhead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatExtnCharge" runat="server" Text='<%# Bind("BoatExtnCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax" HeaderStyle-CssClass="grdhead">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatExtnTaxAmt" runat="server" Text='<%# Bind("BoatExtnTaxAmt") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Boat House" HeaderStyle-CssClass="grdHead" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBoatHouseId" runat="server" Visible="false" Text='<%# Bind("BoatHouseId") %>'></asp:Label>
                                            <asp:Label ID="lblBoatHouseName" runat="server" Text='<%# Bind("BoatHouseName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgBtnAddEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false"
                                                Width="20" CssClass="imgOutLine" OnClick="ImgBtnAddEdit_Click"
                                                runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ImgBtnAddDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                                runat="server" Font-Bold="true" OnClick="ImgBtnAddDelete_Click" OnClientClick="return confirm('Are you sure to Inactive this record?');" />
                                            <asp:LinkButton ID="ImgBtnAddUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                                runat="server" Font-Bold="true" OnClick="ImgBtnAddUndo_Click" OnClientClick="return confirm('Are you sure to Active this record?');" />
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

                <!-- Modal footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnClose" runat="server" Text="Close" TabIndex="37"
                        CssClass="btn btn-danger" OnClick="btnClose_Click" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfUniqueId" runat="server" />
    <asp:HiddenField ID="hfResponse" runat="server" />
    <asp:HiddenField ID="hfPrevImageLink" runat="server" />
    <asp:HiddenField ID="hfImageCheckValue" runat="server" Value="0" />
    <asp:HiddenField ID="hfTaxValue" runat="server" />
    <asp:HiddenField ID="hfTaxCount" runat="server" />



    <asp:HiddenField ID="hfDepositValue" runat="server" />
    <asp:HiddenField ID="hfTimeExtension" runat="server" />
    <asp:HiddenField ID="hfQtp1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfQtp2" runat="server" ClientIDMode="Static" />

    <script type="text/javascript">

        $(function () {
            var radios = $("[id*=rblDepositType] input[type=radio]");
            radios.change(function () {
                var label = $(this).closest("td").find("label").eq(0);
                // alert("SelectedText: " + label.html() + "\nSelectedValue: " + $(this).val());

                $("[id*=txtPercentage]").val("0");
                $("[id*=txtFixedAmount]").val("0");

                if ($(this).val() == 'P') {
                    document.getElementById("<%=divPercentage.ClientID%>").style.display = "block";
                    document.getElementById("<%=divFixedAmount.ClientID%>").style.display = "none";
                    if ($("[id*=txtPercentage]").val() == "0") {
                        if (document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked == true);
                        {
                            document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                            document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                            document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                            document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                            document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;
                        }
                    }
                }
                else {
                    document.getElementById("<%=divPercentage.ClientID%>").style.display = "none";
                    document.getElementById("<%=divFixedAmount.ClientID%>").style.display = "block";
                    if ($("[id*=txtFixedAmount]").val() == "0") {
                        if (document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked == true);
                        {
                            document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                            document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                            document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                            document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                            document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;
                        }
                    }
                }
            });
        });

        $(function () {
            $("[id*=ChkChildApplicable]").click(function () {

                if ($(this).is(":checked")) {

                    document.getElementById("<%=divChildInfantAge.ClientID%>").style.display = "block";
                    document.getElementById("<%=divChild.ClientID%>").style.display = "block";
                    document.getElementById("<%=divActChrg.ClientID%>").style.display = "block";
                    document.getElementById("<%=divChildTax.ClientID%>").style.display = "block";

                } else {
                    document.getElementById("<%=divChildInfantAge.ClientID%>").style.display = "none";
                    document.getElementById("<%=divChild.ClientID%>").style.display = "none";
                    document.getElementById("<%=divActChrg.ClientID%>").style.display = "none";
                    document.getElementById("<%=divChildTax.ClientID%>").style.display = "none";

                    document.getElementById("<%=txtActChildChrg.ClientID%>").value = "0";
                    document.getElementById("<%=txtChildApp.ClientID%>").value = "";
                    document.getElementById("<%=txtChildCharge.ClientID%>").value = "0";
                    document.getElementById("<%=txtChildTaxAmt.ClientID%>").value = "0";
                }
            });
        });


        $(function () {
            $("#lbtnExt").click(function () {
                showModal();
                showExtnType();
            });
        });

        function showModal() {
            $("#myModal").modal('show');
        }


        function hideModal() {
            $("#myModal").modal('hide');
        }

        function CheckRadioButton() {
        }
        $(function () {
            $("[id*=ddlBoatType]").change(function () {
                /*showExtension();*/
            });
        });
        $(function () {
            $("[id*=ddlBoatSeatId]").change(function () {
                showExtension();
            });
        });

        $(function () {
            $("[id*=rblSelfdrive]").change(function () {
                showCharges();
            });
        });

        $(function () {
            $("[id*=rblTimeExtension]").change(function () {
                showExtension();

            });
        });
        $(function () {
            $("[id*=txtBoatMinCharge]").keyup(function () {
                showExtension();
                showExtnType();
            });
        });
        $(function () {
            $("[id*=txtWkEdAmt]").keyup(function () {
                showExtension();
                showExtnType();
            });
        });

        $(function () {
            $("[id*=txtBoatPrmMin]").keyup(function () {
                showExtension();
                showExtnType();
            });
        });

        $(function () {
            $("[id*=ddlExtFromTime]").change(function () {
                frmTimeCheck();
            });
        });

        $(function () {
            $("[id*=ddlExtToTime]").change(function () {
                toTimeCheck();
            });
        });

        function showCharges() {
           
            var selectedvalue = $('#<%= rblSelfdrive.ClientID %> input:checked').val();
            var button_text = $("#<%=btnSubmit.ClientID%>").val();
            if (button_text == "Submit") {
                if (selectedvalue == 'A') {
                    $("[id*=txtRowerMinCharge]").attr("disabled", "disabled");
                    $("[id*=txtRowerPrmMin]").attr("disabled", "disabled");
                    
                 
                }
                else {
                    $("[id*=txtRowerMinCharge]").removeAttr("disabled");
                    $("[id*=txtRowerPrmMin]").removeAttr("disabled");
                }
            }
        }


        function showExtension() {
            var selectedvalue = $('#<%= rblTimeExtension.ClientID %> input:checked').val();

            if ((parseInt(document.getElementById("<%=ddlBoatType.ClientID%>").value) > 0
                && parseInt(document.getElementById("<%=ddlBoatSeatId.ClientID%>").value) > 0
                && selectedvalue == 'A')
                && (parseFloat(document.getElementById("<%=txtBoatMinCharge.ClientID%>").value) > 0
                    || parseFloat(document.getElementById("<%=txtBoatPrmMin.ClientID%>").value) > 0
                || parseFloat(document.getElementById("<%=txtWkEdAmt.ClientID%>").value) > 0)) {

                document.getElementById("<%=divExtn.ClientID%>").style.display = "block";

                document.getElementById("<%=divAmtPer.ClientID%>").style.display = "block";
                document.getElementById("<%=divbtAmt.ClientID%>").style.display = "block";
                document.getElementById("<%=divrwAmt.ClientID%>").style.display = "block";

                document.getElementById("<%=txtBoatExtnCharge.ClientID%>").readonly = true;
                document.getElementById("<%=txtRowerExtnCharge.ClientID%>").readonly = true;
                document.getElementById("<%=Label8.ClientID%>").style.display = "none";
                document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;
                if (selectedvalue == 'A') {

                    document.getElementById("<%=Label8.ClientID%>").style.display = "none";
                    document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                    document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                    document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                    document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                    document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                    document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                    document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                    document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;


                }
                else {

                    document.getElementById("<%=Label8.ClientID%>").style.display = "block";

                }

            }
            else {

                document.getElementById("<%=divExtn.ClientID%>").style.display = "block";
                if (selectedvalue == 'A') {

                    document.getElementById("<%=Label8.ClientID%>").style.display = "none";
                      document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                      document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                      document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                      document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                      document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                      document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                      document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                      document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                      document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                      document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                      document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                      document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                      document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                      document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;


                  }
                  else {

                      document.getElementById("<%=Label8.ClientID%>").style.display = "block";

                }

            }
        }

        function showExtnType() {
            if (parseFloat(document.getElementById("<%=txtBoatMinCharge.ClientID%>").value) > 0
                && parseFloat(document.getElementById("<%=txtBoatPrmMin.ClientID%>").value) > 0
                && parseFloat(document.getElementById("<%=txtWkEdAmt.ClientID%>").value) > 0) {
                $('[id*=ddlExtensionType] option').each(function () {
                    if ($(this).val() == 'WD' || $(this).val() == 'WE' || $(this).val() == 'EC') {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            }

            if (parseFloat(document.getElementById("<%=txtBoatMinCharge.ClientID%>").value) > 0
                && parseFloat(document.getElementById("<%=txtWkEdAmt.ClientID%>").value) == 0) {
                $('[id*=ddlExtensionType] option').each(function () {
                    if ($(this).val() == 'WD' || $(this).val() != 'WE') {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            }
            if (parseFloat(document.getElementById("<%=txtBoatMinCharge.ClientID%>").value) > 0
                && parseFloat(document.getElementById("<%=txtWkEdAmt.ClientID%>").value) == 0) {
                $('[id*=ddlExtensionType] option').each(function () {
                    if ($(this).val() != 'WD' || $(this).val() == 'WE') {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            }
            if (parseFloat(document.getElementById("<%=txtBoatMinCharge.ClientID%>").value) > 0
                && parseFloat(document.getElementById("<%=txtWkEdAmt.ClientID%>").value) == 0) {
                $('[id*=ddlExtensionType] option').each(function () {
                    if ($(this).val() == 'WE' || $(this).val() != 'EC') {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            }
            if (parseFloat(document.getElementById("<%=txtBoatMinCharge.ClientID%>").value) > 0
                && parseFloat(document.getElementById("<%=txtWkEdAmt.ClientID%>").value) == 0) {
                $('[id*=ddlExtensionType] option').each(function () {
                    if ($(this).val() != 'WD' || $(this).val() == 'EC') {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            }
            if (parseFloat(document.getElementById("<%=txtBoatMinCharge.ClientID%>").value) > 0
                && parseFloat(document.getElementById("<%=txtBoatPrmMin.ClientID%>").value) == 0) {
                $('[id*=ddlExtensionType] option').each(function () {
                    if ($(this).val() == 'WD' || $(this).val() != 'EC') {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            }

            if (parseFloat(document.getElementById("<%=txtBoatMinCharge.ClientID%>").value) == 0
                && parseFloat(document.getElementById("<%=txtBoatPrmMin.ClientID%>").value) > 0) {
                $('[id*=ddlExtensionType] option').each(function () {
                    if ($(this).val() != 'WD' || $(this).val() == 'EC') {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            }
        }

        function frmTimeCheck() {
            var Totime = 0;
            var Fromtime = 0;
            var FromTime = document.getElementById("<%=ddlExtFromTime.ClientID%>").value;
            var ToTime = document.getElementById("<%=ddlExtToTime.ClientID%>").value;
            if (ToTime) {
                Totime = parseInt(ToTime);
            }
            if (FromTime) {
                Fromtime = parseInt(FromTime);
            }
            if (Fromtime != '' && Totime < Fromtime) {
                document.getElementById("<%=ddlExtFromTime.ClientID%>").value = "Select From Time";
                alert("Please ensure that the Start Time is less than or equal to the End Time.");
                return false;
            }
        }

        function toTimeCheck() {
           
            
            var Totime = 0;
            var Fromtime = 0;
            var FromTime = document.getElementById("<%=ddlExtFromTime.ClientID%>").value;
            var ToTime = document.getElementById("<%=ddlExtToTime.ClientID%>").value;
            if (ToTime != '' &&  FromTime != '') {
             Totime =parseInt(ToTime);
             Fromtime = parseInt( FromTime);
                if (Fromtime != '' && Fromtime > Totime) {
                    alert("Please ensure that the End Time is greater than or equal to the Start Time.");
                    return false;
                }
                else if (Totime < Fromtime) {
                    document.getElementById("<%=ddlExtToTime.ClientID%>").value = "Select To Time";
                    alert("Please select End Time greater than Start Time.");
                    return false;
                }
            }
        }

        $(function () {
            var radios = $("[id*=rblAmountType] input[type=radio]");

            radios.change(function () {
                var label = $(this).closest("td").find("label").eq(0);

                $("[id*=txtAmtPer]").val("0");

                if ($(this).val() == 'P') {
                    document.getElementById("<%=divAmtPer.ClientID%>").style.display = "block";
                    document.getElementById("<%=divbtAmt.ClientID%>").style.display = "block";
                    document.getElementById("<%=divrwAmt.ClientID%>").style.display = "block";

                    document.getElementById("<%=txtBoatExtnCharge.ClientID%>").readOnly = true;
                    document.getElementById("<%=txtRowerExtnCharge.ClientID%>").readOnly = true;

                    document.getElementById("<%=txtAmtPer.ClientID%>").value = "";
                    document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = "";
                    document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = "";
                    document.getElementById("<%=txtBtNorExtCharges.ClientID%>").value = "";
                    document.getElementById("<%=txtTaxBtExtChrg.ClientID%>").value = "";

                    document.getElementById("<%=txtBtAmt.ClientID%>").value = "";
                    document.getElementById("<%=txtRwAmt.ClientID%>").value = "";
                }
                else {
                    document.getElementById("<%=divAmtPer.ClientID%>").style.display = "none";
                    document.getElementById("<%=divbtAmt.ClientID%>").style.display = "none";
                    document.getElementById("<%=divrwAmt.ClientID%>").style.display = "none";

                    document.getElementById("<%=txtBoatExtnCharge.ClientID%>").readOnly = false;
                    document.getElementById("<%=txtRowerExtnCharge.ClientID%>").readOnly = false;

                    document.getElementById("<%=txtAmtPer.ClientID%>").value = "";
                    document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = "";
                    document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = "";
                    document.getElementById("<%=txtBtNorExtCharges.ClientID%>").value = "";
                    document.getElementById("<%=txtTaxBtExtChrg.ClientID%>").value = "";

                    document.getElementById("<%=txtBtAmt.ClientID%>").value = "";
                    document.getElementById("<%=txtRwAmt.ClientID%>").value = "";
                }
            });
        });


        $(function () {
            $("[id*=txtAmtPer]").keyup(function () {
                if (document.getElementById("<%=txtAmtPer.ClientID%>").value > "0") {

                    document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = "";
                    document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = "";

                    var selectedvalue = $('#<%= rblDepositType.ClientID %> input:checked').val();
                    if (selectedvalue == "P") {

                        if (document.getElementById("<%=ddlExtensionType.ClientID%>").value == "WD") {
                            //Normal Charge
                            var amtPer = document.getElementById("<%=txtAmtPer.ClientID%>").value;
                            var depPer = document.getElementById("<%=txtPercentage.ClientID%>").value;

                            var btMinChrg = document.getElementById("<%=txtBoatMinCharge.ClientID%>").value * (depPer / 100);
                            var rwMinChrg = document.getElementById("<%=txtRowerMinCharge.ClientID%>").value * (depPer / 100);

                            document.getElementById("<%=txtBtAmt.ClientID%>").value = btMinChrg;
                            document.getElementById("<%=txtRwAmt.ClientID%>").value = rwMinChrg;

                            var totBtExtChrg = btMinChrg * (amtPer / 100);
                            var totRwExtChrg = rwMinChrg * (amtPer / 100);

                            document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = totBtExtChrg;
                            document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = totRwExtChrg;
                        }
                        else if (document.getElementById("<%=ddlExtensionType.ClientID%>").value == "WE") {
                            //Week End Charge
                            var amtPer = document.getElementById("<%=txtAmtPer.ClientID%>").value;
                            var depPer = document.getElementById("<%=txtPercentage.ClientID%>").value;

                            var btWEChrg = document.getElementById("<%=txtWkEdAmt.ClientID%>").value * (depPer / 100);
                            var rwWEChrg = document.getElementById("<%=txtWkEdPamt.ClientID%>").value * (depPer / 100);

                            document.getElementById("<%=txtBtAmt.ClientID%>").value = btWEChrg;
                            document.getElementById("<%=txtRwAmt.ClientID%>").value = rwWEChrg;

                            var totBtExtChrg = btWEChrg * (amtPer / 100);
                            var totRwExtChrg = rwWEChrg * (amtPer / 100);

                            document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = totBtExtChrg;
                            document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = totRwExtChrg;
                        }
                        else { //Premium Charge
                            var amtPer = document.getElementById("<%=txtAmtPer.ClientID%>").value;
                            var depPer = document.getElementById("<%=txtPercentage.ClientID%>").value;

                            var btPremMinChrg = document.getElementById("<%=txtBoatPrmMin.ClientID%>").value * (depPer / 100);
                            var rwPremMinChrg = document.getElementById("<%=txtRowerPrmMin.ClientID%>").value * (depPer / 100);

                            document.getElementById("<%=txtBtAmt.ClientID%>").value = btPremMinChrg;
                            document.getElementById("<%=txtRwAmt.ClientID%>").value = rwPremMinChrg;

                            var totBtExtChrg = btPremMinChrg * (amtPer / 100);
                            var totRwExtChrg = rwPremMinChrg * (amtPer / 100);

                            document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = totBtExtChrg;
                            document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = totRwExtChrg;
                        }
                    }
                    else {
                        if (document.getElementById("<%=ddlExtensionType.ClientID%>").value == "WD") {
                            //Normal Charge
                            var amtPer = document.getElementById("<%=txtAmtPer.ClientID%>").value;
                        var btMinChrg = document.getElementById("<%=txtWkEdAmt.ClientID%>").value;
                        var rwMinChrg = document.getElementById("<%=txtRowerMinCharge.ClientID%>").value;

                        document.getElementById("<%=txtBtAmt.ClientID%>").value = btMinChrg;
                        document.getElementById("<%=txtRwAmt.ClientID%>").value = rwMinChrg;

                        var totBtExtChrg = btMinChrg * (amtPer / 100);
                        var totRwExtChrg = rwMinChrg * (amtPer / 100);

                        document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = totBtExtChrg;
                        document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = totRwExtChrg;
                    }
                    else if (document.getElementById("<%=ddlExtensionType.ClientID%>").value == "WE") {
                        //Week End Charge
                        var amtPer = document.getElementById("<%=txtAmtPer.ClientID%>").value;
                        var btWEChrg = document.getElementById("<%=txtBoatMinCharge.ClientID%>").value;
                        var btWEChrg = document.getElementById("<%=txtWkEdPamt.ClientID%>").value;

                        document.getElementById("<%=txtBtAmt.ClientID%>").value = btWEChrg;
                        document.getElementById("<%=txtRwAmt.ClientID%>").value = btWEChrg;

                        var totBtExtChrg = btWEChrg * (amtPer / 100);
                        var totRwExtChrg = btWEChrg * (amtPer / 100);

                        document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = totBtExtChrg;
                        document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = totRwExtChrg;
                    }
                    else { //Premium Charge
                        var amtPer = document.getElementById("<%=txtAmtPer.ClientID%>").value;
                        var btPremMinChrg = document.getElementById("<%=txtBoatPrmMin.ClientID%>").value;
                        var rwPremMinChrg = document.getElementById("<%=txtRowerPrmMin.ClientID%>").value;

                        document.getElementById("<%=txtBtAmt.ClientID%>").value = btPremMinChrg;
                        document.getElementById("<%=txtRwAmt.ClientID%>").value = rwPremMinChrg;

                        var totBtExtChrg = btPremMinChrg * (amtPer / 100);
                        var totRwExtChrg = rwPremMinChrg * (amtPer / 100);

                        document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = totBtExtChrg;
                        document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = totRwExtChrg;
                        }
                    }

                    var inputnum = document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value;
                    var amt = parseInt(inputnum);
                    var rowerchr = document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value;
                    if (!(inputnum)) {
                        inputnum = 0;
                    }

                    if (!(rowerchr)) {
                        rowerchr = 0;
                    }

                    if (parseFloat(rowerchr) >= parseFloat(inputnum)) {

                        document.getElementById("<%=txtBtNorExtCharges.ClientID%>").value = "0";
                        document.getElementById("<%=txtTaxBtExtChrg.ClientID%>").value = "0";
                        return;
                    }

                    if (document.getElementById("<%=hfTaxValue.ClientID%>").value == '') {
                        alert('Select Tax');

                        document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = "";
                        document.getElementById("<%=txtBtNorExtCharges.ClientID%>").value = "0";
                        document.getElementById("<%=txtTaxBtExtChrg.ClientID%>").value = "0";

                        return
                    }

                    if (document.getElementById("<%=hfTaxValue.ClientID%>").value != 'Nil Tax') {

                        function round(value, decimals) {
                            let sum = Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
                            let gst = parseFloat(sum) * parseFloat(document.getElementById("<%=hfTaxCount.ClientID%>").value);
                            roundamount(inputnum - (parseFloat(rowerchr) + parseFloat(gst)), 2);
                            document.getElementById("<%=txtTaxBtExtChrg.ClientID%>").value = gst;
                            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
                        }

                        round((inputnum / (100 + (parseFloat(document.getElementById("<%=hfTaxValue.ClientID%>").value)
                            * parseFloat(document.getElementById("<%=hfTaxCount.ClientID%>").value))))
                            * parseFloat(document.getElementById("<%=hfTaxValue.ClientID%>").value), 2);

                        function roundamount(value, decimals) {
                            let amount = Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
                            document.getElementById("<%=txtBtNorExtCharges.ClientID%>").value = amount;
                            $("#hfQtp1").val(amount);
                            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
                        }
                    }
                    else {
                        document.getElementById("<%=txtBtNorExtCharges.ClientID%>").value = inputnum;
                        document.getElementById("<%=txtTaxBtExtChrg.ClientID%>").value = "0";
                    }
                }
                else {
                    var btMinChrg = document.getElementById("<%=txtBoatMinCharge.ClientID%>").value;
                    var rwMinChrg = document.getElementById("<%=txtRowerMinCharge.ClientID%>").value;

                    document.getElementById("<%=txtBtAmt.ClientID%>").value = btMinChrg;
                    document.getElementById("<%=txtRwAmt.ClientID%>").value = rwMinChrg;

                    document.getElementById("<%=txtBoatExtnCharge.ClientID%>").value = "";
                    document.getElementById("<%=txtRowerExtnCharge.ClientID%>").value = "";
                }
            });
        });



        $(function () {
            $("[id*=ChkSingleAllwd]").click(function () {

                if ($(this).is(":checked")) {
                    var selectedvalue = $('#<%= rblTimeExtension.ClientID %> input:checked').val();


                    if (selectedvalue == 'A' && document.getElementById("<%=txtPercentage.ClientID%>").value > 0) {
                        document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                            document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                            document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                            document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                            document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;
                        alert("Single Ticket is not Allowed")

                    }
                    if (selectedvalue == 'A' && document.getElementById("<%=txtFixedAmount.ClientID%>").value > 0) {
                        document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                            document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                            document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                            document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                            document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;
                            alert("Single Ticket is not Allowed")

                        }
                        else if (selectedvalue != 'A' && document.getElementById("<%=txtPercentage.ClientID%>").value == "0.00" && document.getElementById("<%=txtFixedAmount.ClientID%>").value == "0.00") {
                            document.getElementById("<%=divSAcharge.ClientID%>").style.display = "block";
                            document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "block";

                            document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "block";
                            document.getElementById("<%=lblsa.ClientID%>").style.display = "block";
                        }
                        else if (selectedvalue != 'A' && document.getElementById("<%=txtPercentage.ClientID%>").value == 0 && document.getElementById("<%=txtFixedAmount.ClientID%>").value == 0) {
                            document.getElementById("<%=divSAcharge.ClientID%>").style.display = "block";
                            document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "block";

                            document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "block";
                            document.getElementById("<%=lblsa.ClientID%>").style.display = "block";
                        }


                        else {
                            document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                            document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                            document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                            document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                            document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                            document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";

                            document.getElementById("<%=ChkSingleAllwd.ClientID%>").checked = false;
                        alert("Single Ticket is not Allowed")

                    }



                }
                else {
                    document.getElementById("<%=lblsa.ClientID%>").style.display = "none";
                    document.getElementById("<%=divSAcharge.ClientID%>").style.display = "none";
                    document.getElementById("<%=DivSAWeekday.ClientID%>").style.display = "none";
                    document.getElementById("<%=DivSAWeekEnd.ClientID%>").style.display = "none";

                    document.getElementById("<%=txtIWDAmt.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWDBtChrg.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWdPaymt.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWdToTax.ClientID%>").value = "0";

                    document.getElementById("<%=txtIWEAmt.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWEBtChrg.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWEPaymt.ClientID%>").value = "0";
                    document.getElementById("<%=txtIWEToTax.ClientID%>").value = "0";
                }
            });
        });
    </script>

    <script type="text/javascript">
</script>
    <%-- <script
  src="https://code.jquery.com/jquery-3.6.4.slim.min.js"
  integrity="sha256-a2yjHM4jnF9f54xUQakjZGaqYs/V1CYvWpoqZzC2/Bw="
  crossorigin="anonymous"></script>--%>
    <%-- <script type="text/javascript" src="https://code.jquery.com/jquery-3.3.1.slim.min.js" 
        integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>--%>

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js"
        integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>

    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>


